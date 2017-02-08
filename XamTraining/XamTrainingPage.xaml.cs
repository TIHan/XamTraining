using System;
using Xamarin.Forms;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace XamTraining
{
	public partial class XamTrainingPage : ContentPage
	{
		public XamTrainingPage()
		{
			InitializeComponent();

			this.TestCrash.Clicked += (sender, e) =>
			{
				throw new Exception("Mobile Center Crash Example");
			};

			this.TestEvent.Clicked += (sender, e) =>
			{
				Analytics.TrackEvent("Mobile Center Event Example");
			};
		}
	}
}
