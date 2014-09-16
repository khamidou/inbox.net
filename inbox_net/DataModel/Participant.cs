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
    public class Participant
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email_Address { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, Email_Address);
        }
    }
}
