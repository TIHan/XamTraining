using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XamTraining.UITest
{
	public class XamTrainingTests : BaseTestFixture
	{
		public XamTrainingTests(Platform platform)
			: base(platform)
		{
		}

		[Test]
		public void MobileCenterEventTest()
		{
			new XamTrainingPage()
				.TapTestEvent()
				.AssertOnPage();
		}

		[Test]
		public void MobileCenterCrashTest()
		{
			new XamTrainingPage()
				.TapTestCrash()
				.AssertOnPage();
		}
	}
}
