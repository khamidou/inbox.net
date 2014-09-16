using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PropertyChanged;

namespace inbox_net.DataModel
{
    [ImplementPropertyChanged]
    public class File
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("namespace_id")]
        public string Namespace_ID { get; set; }

        [JsonProperty("filename")]
        public string FileName { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("content-type")]
        public string Content_Type { get; set; }

        [JsonProperty("message_id")]
        public string Message_ID { get; set; }

        [JsonProperty("is_embedded")]
        public bool IsEmbeded { get; set; }

        public override string ToString()
        {
            return FileName;
        }
    }
}
