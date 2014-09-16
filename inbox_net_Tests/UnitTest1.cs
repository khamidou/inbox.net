using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using inbox_net;

namespace inbox_net_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async void TestMethod1()
        {
            InboxClient client = new InboxClient("http://inboxappmachine.cloudapp.net:5555/");
            await client.GetFirstNamespace();
        }
    }
}
