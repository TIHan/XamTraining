using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace XamTraining.Droid
{
    [Activity(Label = "XamTraining.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            MobileCenter.Configure("11c31c79-f578-4aeb-b6ca-6d6a5b19dd5c");

            App.Init((IAuthenticate)this);

            LoadApplication(new App());
        }

        private MobileServiceUser user;

        public async Task<MobileServiceUser> Authenticate()
        {
            try
            {
                user = await TodoItemManager.Instance.Client.LoginAsync(this, MobileServiceAuthenticationProvider.MicrosoftAccount);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return user;
        }
    }
}
