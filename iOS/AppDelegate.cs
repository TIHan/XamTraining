using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace XamTraining.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif

			MobileCenter.Start("7391035a-3740-46b7-ac05-5307db9c0bd4", typeof(Analytics), typeof(Crashes));

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
