using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inbox_net.DataModel
{
    public class RestfulModelCollection<T> : ObservableCollection<T>
    {
        const int CHUNK_SIZE = 50;
        const int MAX_ITEMS = Convert.ToInt32(Math.Pow(2,64)) - 1;

        private Namespace Namespace;
        private Client api;
        private Dictionary<string, object> filters;
        private int offset;
        private int limit;
        public string CollectionName { get; set; }

        public RestfulModelCollection(Client _api, Namespace _namespace, Dictionary<string, object> filter = null, int _offset = 0, int _limit = 0) : base()
        {
            if (filter == null)
            {
                this.filters = new Dictionary<string, object>();
            }
            else
            {
                this.filters = filter;
            }
            filters.Add("offset", 0);
            filters.Add("limit", MAX_ITEMS);
            this.Namespace = _namespace;
            this.api = _api;
            this.offset = _offset;
            if (_limit == 0)
            {
                this.limit = MAX_ITEMS;
            }
            else
            {
                this.limit = _limit;
            }
        }
    }
}
