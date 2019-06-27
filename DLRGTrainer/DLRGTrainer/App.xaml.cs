using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace DLRGTrainer
{
	public partial class App : Application
	{
		public static NavigationPage NavPage { get; set; }

		public App ()
		{
			InitializeComponent();

			NavPage = new NavigationPage(new MasterPage())
			{
				BarBackgroundColor = Color.FromRgb(255, 33, 41),
				BarTextColor = Color.FromRgb(255, 242, 0)
			};
			//PageTopChangeColorRandom();

			MainPage = NavPage;
		}

		public static void PageTopChangeColorRandom()
		{
			Random rnd = new Random();

			int[] colorValue = new int[6];

			for (int i = 0; i < 3; i++)
				colorValue[i] = rnd.Next(255);

			for (int i = 2; i < 6; i++)
				colorValue[i] = 255 - colorValue[i - 2];

			NavPage.BarBackgroundColor = Color.FromRgb(colorValue[0], colorValue[1], colorValue[2]);
			NavPage.BarTextColor = Color.FromRgb(colorValue[3], colorValue[4], colorValue[5]);
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
