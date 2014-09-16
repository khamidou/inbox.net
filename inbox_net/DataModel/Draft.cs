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
    public class Draft
    {
        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("to")]
        public List<Participant> To { get; set; }

        [JsonProperty("cc")]
        public List<Participant> CC { get; set; }

        [JsonProperty("bcc")]
        public List<Participant> BCC { get; set; }

        [JsonProperty("thread_id")]
        public string Thread_ID { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("file_ids")]
        public List<string> File_IDs { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }


        public override string ToString()
        {
            return Subject;
        }
    }
}
