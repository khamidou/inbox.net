using inbox_net.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace inbox_net
{
    public class InboxClient
    {
        private string Inbox_API_Url = "https://slack.com/api/";

        private string APP_ID = null;

        private string APP_Secret = null;

        private string AccessToken = null;

        public string Namespace = null;

        public bool IsAuthorized { get; set; }

        public InboxClient(string api_url)
        {
            Inbox_API_Url = api_url;
        }

        /// <summary>
        /// Creates a new authenticated InboxClient
        /// </summary>
        /// <param name="app_id">Your app id</param>
        /// <param name="app_secret">Your app secret</param>
        /// <param name="accessToken">Previously received Access Token</param>
        public InboxClient(string app_id, string app_secret, string accessToken)
        {
            this.APP_ID = app_id;
            this.APP_Secret = app_secret;
            this.AccessToken = accessToken;
            IsAuthorized = AccessToken != null;
        }


#region Namespaces

        public async Task<Namespace> GetFirstNamespace()
        {
            string json = await HttpGet(GenerateUri(MethodURIs.Namespace));
            return (await DeserializeObjectAsync<IEnumerable<Namespace>>(json)).First();
        }

        public async Task<Namespace> GetNamespace(string _namespace)
        {
            string json = await HttpGet(GenerateUri(MethodURIs.Namespace + _namespace));
            return await DeserializeObjectAsync<Namespace>(json);
        }


#endregion


#region Tags

        public async Task<IEnumerable<Tag>> GetTags()
        {
            string json = await HttpGet(GenerateUri(string.Format(MethodURIs.Tags, this.Namespace)));
            return await DeserializeObjectAsync<IEnumerable<Tag>>(json);
        }

        public async Task<Tag> GetTag(string tagID)
        {
            string json = await HttpGet(GenerateUri(string.Format(MethodURIs.Tags, this.Namespace) + tagID, null));
            return await DeserializeObjectAsync<Tag>(json);
        }

        public async Task<Tag> CreateCustomTag(string tagName)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("name", tagName));
            string json = await HttpPost(GenerateUri(string.Format(MethodURIs.Tags, this.Namespace)), GenerateRequestBody(parameters));
            return await DeserializeObjectAsync<Tag>(json);
        }

        public async Task<Tag> RenameCustomTag(string tagID, string newTagName)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("name", newTagName));
            string json = await HttpPut(GenerateUri(string.Format(MethodURIs.Tags, this.Namespace) + tagID), GenerateRequestBody(parameters));
            return await DeserializeObjectAsync<Tag>(json);
        }

#endregion


#region Threads

        public async Task<Thread> GetThread(string thread_id)
        {
            string json = await HttpGet(GenerateUri(string.Format(MethodURIs.Thread, this.Namespace, thread_id)));
            return await DeserializeObjectAsync<Thread>(json);
        }

        public async Task<IEnumerable<Thread>> GetThreads(string subject = null, string any_email = null, string to = null, string from = null, string cc = null,
                                                          string bcc = null, string tag = null, string filename = null, int limit = 100, int offset = 0, int last_message_before = 0, 
                                                          int last_message_after = 0, int started_before = 0, int started_after = 0)
        {
            List<KeyValuePair<string, string>> parameters = ValidateGetThreadsParams(subject, any_email, to, from, cc, bcc, tag, filename, limit, offset, last_message_before, last_message_after, started_before, started_after);
            string json = await HttpGet(GenerateUri(string.Format(MethodURIs.Threads, this.Namespace), parameters));
            return await DeserializeObjectAsync<IEnumerable<Thread>>(json);
        }

        public async Task<Thread> UpdateThreadTags(string thread_id, string[] add_tags)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("add_tags", StringsToJsonArray(add_tags)));
            return await SendUpdateTagsRequest(thread_id, parameters);
        }

        public async Task<Thread> MarkThreadAsRead(string thread_id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            string[] remove_tags = { "unread" };
            parameters.Add(new KeyValuePair<string, string>("remove_tags", StringsToJsonArray(remove_tags)));
            return await SendUpdateTagsRequest(thread_id, parameters);
        }

        public async Task<Thread> MarkThreadAsUnread(string thread_id)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            string[] add_tags = { "unread" };
            parameters.Add(new KeyValuePair<string, string>("add_tags", StringsToJsonArray(add_tags)));
            return await SendUpdateTagsRequest(thread_id, parameters);
        }

        private async Task<Thread> SendUpdateTagsRequest(string thread_id, List<KeyValuePair<string,string>> parameters)
        {
            string json = await HttpPut(GenerateUri(string.Format(MethodURIs.Thread, this.Namespace, thread_id)), GenerateRequestBody(parameters));
            return await DeserializeObjectAsync<Thread>(json);
        }
       



        private List<KeyValuePair<string,string>> ValidateGetThreadsParams(string subject = null, string any_email = null, string to = null, string from = null, string cc = null,
                                                          string bcc = null, string tag = null, string filename = null, int limit = 100, int offset = 0, int last_message_before = 0, 
                                                          int last_message_after = 0, int started_before = 0, int started_after = 0)
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            if (subject != null)
            {
                parameters.Add(new KeyValuePair<string, string>("subject", subject));
            }
            if (any_email != null)
            {
                parameters.Add(new KeyValuePair<string, string>("any_email", any_email));
            }
            if (to != null)
            {
                parameters.Add(new KeyValuePair<string, string>("to", to));
            }
            if (from != null)
            {
                parameters.Add(new KeyValuePair<string, string>("from", from));
            }
            if (cc != null)
            {
                parameters.Add(new KeyValuePair<string, string>("cc", cc));
            }
            if (bcc != null)
            {
                parameters.Add(new KeyValuePair<string, string>("bcc", bcc));
            }
            if (tag != null)
            {
                parameters.Add(new KeyValuePair<string, string>("tag", tag));
            }
            if (filename != null)
            {
                parameters.Add(new KeyValuePair<string, string>("filename", filename));
            }
            if (limit != 100)
            {
                parameters.Add(new KeyValuePair<string, string>("limit", limit.ToString()));
            }
            if (offset != 0)
            {
                parameters.Add(new KeyValuePair<string, string>("offset", offset.ToString()));
            }
            if (last_message_before != 0)
            {
                parameters.Add(new KeyValuePair<string, string>("last_message_before", last_message_before.ToString()));
            }
            if (last_message_after != 0)
            {
                parameters.Add(new KeyValuePair<string, string>("last_message_after", last_message_after.ToString()));
            }
            if (started_before != 0)
            {
                parameters.Add(new KeyValuePair<string, string>("started_before", started_before.ToString()));
            }
            if (started_after != 0)
            {
                parameters.Add(new KeyValuePair<string, string>("started_after", started_after.ToString()));
            }
            return parameters;
        }

#endregion



        private async Task<string> HttpGet(string callUri)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            try
            {
                response = await client.GetAsync(callUri);
            }
            catch (Exception)
            {

            }
            if (response != null)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            return null;
        }

        private async Task<string> HttpPost(string callUri, string requestBody)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            try
            {
                response = await client.PostAsync(callUri, new StringContent(requestBody));
            }
            catch (Exception)
            {

            }
            if (response != null)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            return null;
        }

        private async Task<string> HttpPut(string callUri, string requestBody)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            try
            {
                response = await client.PutAsync(callUri, new StringContent(requestBody));
            }
            catch (Exception)
            {

            }
            if (response != null)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            return null;
        }

        private string GenerateUri(string methodUrl, IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            StringBuilder sb = new StringBuilder(Inbox_API_Url);
            sb.Append(methodUrl + "?");
            if (parameters != null)
            {
                foreach (KeyValuePair<string, string> parameter in parameters)
                {
                    sb.Append(string.Format("{0}={1}&", parameter.Key, WebUtility.UrlEncode(parameter.Value)));
                }
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        private string GenerateRequestBody(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    if (item.Value[0] == '[' && item.Value[item.Value.Length - 1] == ']')
                    {
                        sb.AppendLine(string.Format("    \"{0}\": {1},", item.Key, item.Value));
                    }
                    else
                    {
                        sb.AppendLine(string.Format("    \"{0}\": \"{1}\",", item.Key, item.Value));
                    }
                }
            }
            sb.Remove(sb.Length - 3, 3);
            sb.AppendLine();
            sb.Append("}");
            return sb.ToString();
        }

        private string StringsToJsonArray(string[] strings)
        {
            StringBuilder sb = new StringBuilder("[");
            for (int i = 0; i < strings.Length; i++)
            {
                sb.AppendFormat("\"{0}\",", strings[i]);
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return sb.ToString();
        }

        private async Task<T> DeserializeObjectAsync<T>(string json)
        {
            if (json != null)
            {
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(json));
            }
            return default(T);
        }

    }
}
