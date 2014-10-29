using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using MonoTouch.CoreLocation;
using Kangou.Core;
using System;

namespace Kangou.Touch
{

	[Register ("AppDelegate")]
	public partial class AppDelegate : MvxApplicationDelegate
	{
		UIWindow _window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			_window = new UIWindow (UIScreen.MainScreen.Bounds);

			var presenter = new MvxSlidingPanelsTouchViewPresenter(this, _window);

			var setup = new Setup(this, presenter);
			setup.Initialize();

			var startup = Mvx.Resolve<IMvxAppStart>();
			startup.Start();

			_window.MakeKeyAndVisible();

			if (UIDevice.CurrentDevice.CheckSystemVersion (7, 8)) {
				//Version 8
				var settings = UIUserNotificationSettings.GetSettingsForTypes(
					UIUserNotificationType.Alert
					| UIUserNotificationType.Badge
					| UIUserNotificationType.Sound,
					new NSSet());
				UIApplication.SharedApplication.RegisterUserNotificationSettings (settings);
			}

			return true;
		}

		public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		{
			// reset our badge
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}

	}
}