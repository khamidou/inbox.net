using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inbox_net.DataModel
{
    [ImplementPropertyChanged]
    public class Message
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("from")]
        public List<Participant> From { get; set; }

        [JsonProperty("to")]
        public List<Participant> To { get; set; }

        [JsonProperty("cc")]
        public List<Participant> CC { get; set; }

        [JsonProperty("bcc")]
        public List<Participant> BCC { get; set; }

        //[JsonProperty("to")]
        //public List<string> To_Strings { get; set; }

        [JsonProperty("date")]
        public int Date { get; set; }

        [JsonProperty("thread")]
        public string Thread_ID { get; set; }

        [JsonProperty("files")]
        public List<File> Files { get; set; }

        [JsonProperty("snippet")]
        public string Snippet { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        public override string ToString()
        {
            return Subject;
        }
    }
}
