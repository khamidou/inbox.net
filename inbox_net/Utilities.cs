using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace inbox_net
{
    public static class Utilities
    {
        public static string Url_Concat(string url, Dictionary<string, object> args, string fragments = null)
        {
            if (args == null && fragments == null)
            {
                return url;
            }

            // Strip off hashes
            char[] trimChars = {'#'};
            url = url.TrimEnd(trimChars);

            string fragment_tail = "";
            if (fragments != null)
            {
                fragment_tail = "#" + WebUtility.UrlEncode(fragments);
            }

            string args_tail = "";
            if (args != null)
            {
                foreach (var item in args)
                {
                    if (url[url.Length - 1] != '?' || url[url.Length - 1] != '&')
                    {
                        args_tail += url.Contains("?") ? "&" : "?";
                    }
                    args_tail += string.Format("{0}={1}", item.Key, WebUtility.UrlEncode(item.Value.ToString()));
                }
            }

            return url + args_tail + fragment_tail;
        }

        public static string generate_id(int length)
        {
            string guidResult = string.Empty;

            while (guidResult.Length < length)
            {
                // Get the GUID.
                guidResult += Guid.NewGuid().ToString().GetHashCode().ToString("x");
            }

            // Make sure length is valid.
            if (length <= 0 || length > guidResult.Length)
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);

            // Return the first length bytes.
            return guidResult.Substring(0, length);
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
