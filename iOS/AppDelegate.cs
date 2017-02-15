using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using System.IO;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace XamTraining.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IAuthenticate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

#if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
#endif
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            MobileCenter.Start("7391035a-3740-46b7-ac05-5307db9c0bd4", typeof(Analytics), typeof(Crashes));

            App.Init(this);

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        private MobileServiceUser user;

        public async Task<MobileServiceUser> Authenticate()
        {
            try
            {
                if (user == null)
                {
                    user = await TodoItemManager.Instance.Client.LoginAsync(
                        UIApplication.SharedApplication.KeyWindow.RootViewController,
                        MobileServiceAuthenticationProvider.MicrosoftAccount);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return user;
        }
    }
}
