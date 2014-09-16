using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using inbox_net;
using inbox_net.DataModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Test_WinRT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            InboxClient client = new InboxClient("http://inboxdev.cloudapp.net:5555/");
            Namespace ns = await client.GetFirstNamespace();
            client.Namespace = ns.Namespace_ID;
            List<Tag> tags = new List<Tag>(await client.GetTags());
            //Tag tag = await client.GetTag("Test Tag");
            Tag tag = tags.Where(t => t.Name == "TestTag").First();
            Tag tag2 = await client.RenameCustomTag(tag.ID, "Other Test");
        }


    }
}
