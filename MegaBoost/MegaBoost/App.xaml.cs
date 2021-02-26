using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MegaBoost
{
    public partial class App : Application
    {
        public static VKAPI VkLongPoolClient;
        public App()
        {
            InitializeComponent();
            VkLongPoolClient = new VKAPI("token", "botid", Page1.ProcessMsgFromVk);
            MainPage = new NavigationPage(new Page1());
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
