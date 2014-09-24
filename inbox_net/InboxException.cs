using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace inbox_net
{
    public class InboxException : Exception
    {
        public string Url { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Data { get; set; }
        public string Message { get; set; }

        public InboxException(string url, HttpStatusCode statusCode, string data, string message)
        {
            Url = url;
            StatusCode = statusCode;
            Data = data;
            Message = message;
        }

        public InboxException()
        {

        }
    }
}
