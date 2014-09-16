using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inbox_net.DataModel
{
    public sealed class CanonicalTags
    {
        public static readonly string Inbox = "inbox";
        public static readonly string Archive = "archive";
        public static readonly string Drafts = "drafts";
        public static readonly string Sending = "sending";
        public static readonly string Sent = "sent";
        public static readonly string Spam = "spam";
        public static readonly string Starred = "starred";
        public static readonly string Unread = "unread";
        public static readonly string Trash = "trash";
        public static readonly string Unseen = "unseen";
    }
}
