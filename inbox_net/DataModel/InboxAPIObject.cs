using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inbox_net.DataModel
{
    public class InboxAPIObject
    {
        public string ID { get; set; }
        public Type CLS { get; set; }
        public Client API { get; set; }
        public Namespace Namespace { get; set; }

        private InboxAPIObject(Type cls, Client api, Namespace _namespace)
        {
            this.ID = null;
            this.CLS = cls;
            this.API = api;
            this.Namespace = _namespace;
        }

        public InboxAPIObject Create(Type cls, Client api, Namespace _namespace)
        {
            var obj = Activator.CreateInstance(this.CLS, this.API, this.Namespace);
            (obj as InboxAPIObject).CLS = cls;
            return obj as InboxAPIObject;
        }

        public async Task<string> AsJson()
        {
            return await Task.Factory.StartNew(() => JsonConvert.SerializeObject(this));
        }

        public RestfulModelCollection<T> ChildCollection<T>(Dictionary<string, object> filters = null)
        {
            return new RestfulModelCollection<T>(this.API, this.Namespace, filters);
        }

        public async void Save()
        {
            var newObj = Activator.CreateInstance(this.CLS);
            if (this.ID != null)
            {
                newObj = this.API.UpdateResource(this.Namespace, this.CLS, this.ID, await this.AsJson());
            }
            else
            {
                newObj = this.API.CreateResource(this.Namespace, this.CLS, await this.AsJson());
            }
            this.CLS = newObj.GetType();
        }

        public async void Update()
        {
            var newObj = Activator.CreateInstance(this.CLS);
            newObj = this.API.UpdateResource(this.Namespace, this.CLS, this.ID, await this.AsJson());
            this.CLS = newObj.GetType();
        }
    }
}
