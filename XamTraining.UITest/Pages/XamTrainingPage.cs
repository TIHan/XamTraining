using System;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace XamTraining.UITest
{
	public class XamTrainingPage : BasePage
	{
		readonly Query crashButton = x => x.Marked("CrashButton");
		readonly Query eventButton = x => x.Marked("EventButton");

		protected override PlatformQuery Trait => new PlatformQuery
		{
			Android = x => x.Marked("XamTrainingPage"),
			iOS = x => x.Marked("XamTrainingPage")
		};


		public XamTrainingPage TapTestEvent()
		{
			app.Tap(eventButton);
			app.Screenshot("Tapped: 'Test Event'");

			return this;
		}

		public XamTrainingPage TapTestCrash()
		{
			app.Tap(crashButton);
			app.Screenshot("Tapped: 'Test Crash'");

			return this;
		}
	}
}
