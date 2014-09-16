using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace inbox_net.DataModel
{
    [ImplementPropertyChanged]
    public class Tag
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("namespace_id")]
        public string Namespace_ID { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }


        public override string ToString()
        {
            return Name;
        }
    }
}
