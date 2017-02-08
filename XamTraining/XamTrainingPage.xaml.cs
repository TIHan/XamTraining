using System;
using Xamarin.Forms;

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
		}
	}
}
