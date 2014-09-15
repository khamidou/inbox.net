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
    public class Namespace
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("namespace_id")]
        public string Namespace_ID { get; set; }

        [JsonProperty("account_id")]
        public string Account_ID { get; set; }

        [JsonProperty("email_address")]
        public string Email_Address { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }

    }
}
