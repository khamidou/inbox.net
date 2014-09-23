using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace inbox_net
{
    
    public class Client
    {
        private string api_server = "https://api.inboxapp.com";
        private string auth_server = "https://www.inboxapp.com";
        private string access_token_url = string.Empty;
        private string auth_token = "";
        private HttpClient client;
        private List<KeyValuePair<string, string>> session_headers;
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

        }

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
            Dictionary<string, string> args = new Dictionary<string, string>();
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



        
    }
}
