using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inbox_net.DataModel
{
    public sealed class MethodURIs
    {
        public static readonly string Namespace = @"n/";
        public static readonly string Tags = @"n/{0}/tags/";
        public static readonly string Thread = @"n/{0}/threads/{1}";
        public static readonly string Threads = @"n/{0}/threads/";
        public static readonly string Message = @"n/{0}/messages/{1}";
        public static readonly string Messages = @"n/{0}/messages/";
        public static readonly string Draft = @"n/{0}/drafts/{1}";
        public static readonly string Drafts = @"n/{0}/drafts/";
        public static readonly string File = @"n/{0}/files/{1}";
        public static readonly string Files = @"n/{0}/files/";
        public static readonly string Send = @"n/{0}/send";
    }
}
