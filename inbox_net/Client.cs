using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using inbox_net.DataModel;
using Newtonsoft.Json;
using System.Net;

namespace inbox_net
{
    
    public class Client
    {
        private string api_server = "https://api.inboxapp.com";
        private string auth_server = "https://www.inboxapp.com";
        private string access_token_url = string.Empty;
        private string auth_token = "";
        private HttpClient client;
        private string access_token;
        private string app_secret;
        private string app_id;

        public Client(string INBOX_APP_ID, 
                      string INBOX_APP_SECRET,
                      string INBOX_ACCESS_TOKEN = null,
                      string API_SERVER = "https://api.inboxapp.com",
                      string AUTH_SERVER = "https://www.inboxapp.com")
        {
            if (!API_SERVER.Contains("://"))
            {
                throw new Exception("When overriding the Inbox API server address, you must include https://");
            }
            this.api_server = API_SERVER;
            this.auth_server = AUTH_SERVER + "/oauth/authorize";
            this.access_token_url = AUTH_SERVER + "/oauth/token";
            this.client = new HttpClient();
            this.client.DefaultRequestHeaders.Add("X-Inbox-API-Wrapper", "dotNET");
            this.Namespaces = new RestfulModelCollection<Namespace>(this);
        }

        public RestfulModelCollection<Namespace> Namespaces { get; set; }

        public string Access_Token
        {
            get
            {
                return this.access_token;
            }
            set
            {
                access_token = value;
                if (value != null)
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic " + Convert.ToBase64String(Utilities.GetBytes(value + ":")));
                }
            }
        }

        public string authenticationUrl(string redirect_uri, string login_hint = "")
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("redirect_uri", redirect_uri);
            args.Add("client_id", this.app_id);
            args.Add("response_type", "code");
            args.Add("scope", "email");
            args.Add("login_hint", login_hint);
            args.Add("state", Utilities.generate_id(10));

            return Utilities.Url_Concat(this.auth_server, args);
        }

        public async Task<string> Token_For_Code(string code)
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>();
            args.Add(new KeyValuePair<string, string>("client_id", this.app_id));
            args.Add(new KeyValuePair<string, string>("client_secret", this.app_secret));
            args.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            args.Add(new KeyValuePair<string, string>("code", code));

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-type", "application/x-www-form-urlencoded");
            headers.Add("Accept", "text/plain");
            
            client.DefaultRequestHeaders.Add("Content-type", "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Add("Accept", "text/plain");
            string response = await (await client.PostAsync(this.access_token_url, new FormUrlEncodedContent(args))).Content.ReadAsStringAsync();

            JObject obj = JObject.Parse(response);
            this.auth_token = obj["access_token"].ToString();
            return this.auth_token;
        }

        private async Task<HttpResponseMessage> ValidateResponse(HttpResponseMessage response)
        {
            Dictionary<HttpStatusCode, string> status_code_to_exc = new Dictionary<HttpStatusCode, string>();
            status_code_to_exc.Add(HttpStatusCode.BadRequest, "APIError");
            status_code_to_exc.Add(HttpStatusCode.NotFound, "NotFoundError");
            status_code_to_exc.Add(HttpStatusCode.Conflict, "ConflictError");
            var request = response.RequestMessage;
            string url = request.RequestUri.ToString();
            var status_code = response.StatusCode;
            string data = await response.Content.ReadAsStringAsync();
            JObject obj = null;
            try
            {
                obj = data != null ? JObject.Parse(data) : null;
            }
            catch (Exception)
            {

            }

            if (status_code == HttpStatusCode.OK)
            {
                return response;
            }
            else if (status_code == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Not Authorized");
            }
            else if (status_code == HttpStatusCode.BadRequest || status_code == HttpStatusCode.NotFound || status_code == HttpStatusCode.Conflict)
            {
                string errorMessage = status_code_to_exc[status_code];
                try
                {
                    JObject responseJSON = JObject.Parse(data);
                    if (responseJSON["message"] != null)
                    {
                        throw new InboxException(url, status_code, data, responseJSON["message"].ToString());
                    }
                    else
                    {
                        throw new InboxException(url, status_code, data, "N/A");
                    }
                }
                catch (Exception)
                {
                    throw new InboxException(url, status_code, data, "Malformed");
                }
                throw new Exception("Error - Your fault...probably");
            }
            else if (status_code == HttpStatusCode.InternalServerError)
            {
                throw new InboxException(url, status_code, data, "ServerError");
            }
            else
            {
                throw new InboxException(url, status_code, data, "Unknown status code");
            }
        }

        public async Task<IEnumerable<T>> GetResources<T>(string _namespace, RestfulModelCollection<T> cls, Dictionary<string, object> filters)
        {
            string prefix = string.Format("/n/{0}", _namespace != null ? _namespace : "");
            string url = string.Format("{0}{1}/{2}", this.api_server, prefix, cls.CollectionName);
            url = Utilities.Url_Concat(url, filters);

            string json = await (await ValidateResponse(await client.GetAsync(url))).Content.ReadAsStringAsync();
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<T>>(json));
        }

        public async Task<string> GetRawResource(string _namespace, string objectType, string id, Dictionary<string, object> filters)
        {
            // Get an indivitual REST resource
            string extra = null;
            if (filters["extra"] != null)
            {
                extra = filters["extra"].ToString();
                filters.Remove("extra");
            }
            string prefix = string.Format("/n/{0}", _namespace != null ? _namespace : "");
            string postfix = string.Format("/{0}", extra != null ? extra : "");
            string url = string.Format("{0}{1}/{2}/{3}{4}", this.api_server, prefix, objectType, id, postfix);
            url = Utilities.Url_Concat(url, filters);
            return await (await ValidateResponse(await client.GetAsync(url))).Content.ReadAsStringAsync();
        }

        public async Task<T> GetResource<T>(string _namespace, string objectType, string id, Dictionary<string, object> filters)
        {


            return default(T);
        }





        internal T CreateResource<T>(Namespace p1, object p2, string p3)
        {
            throw new NotImplementedException();
        }

        internal T UpdateResource<T>(Namespace _namespace, object cls, string id, string json)
        {
            throw new NotImplementedException();
        }



        internal object UpdateResource(Namespace p1, Type type, string p2, string p3)
        {
            throw new NotImplementedException();
        }

        internal object CreateResource(Namespace p1, Type type, string p2)
        {
            throw new NotImplementedException();
        }
    }
}
