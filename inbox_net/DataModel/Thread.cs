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
    public class Thread
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("namespace_id")]
        public string Namespace_ID { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("last_message_timestamp")]
        public int Last_Message_Timestamp { get; set; }

        [JsonProperty("first_message_timestamp")]
        public int First_Message_Timestamp { get; set; }

        [JsonProperty("participants")]
        public List<Participant> Participants { get; set; }

        [JsonProperty("snippet")]
        public string Snippet { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }

        [JsonProperty("message_ids")]
        public List<string> Messages { get; set; }

        [JsonProperty("draft_ids")]
        public List<string> Drafts { get; set; }


        public override string ToString()
        {
            return Subject;
        }
    }
}
