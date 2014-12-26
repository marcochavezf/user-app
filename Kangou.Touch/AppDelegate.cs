using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using MonoTouch.CoreLocation;
using Kangou.Core;
using System;
using Google.Maps;
using System.Net;
using System.Text;

namespace Kangou.Touch
{

	[Register ("AppDelegate")]
	public partial class AppDelegate : MvxApplicationDelegate
	{
		UIWindow _window;
		const string MapsApiKey = "AIzaSyBAhaDR8_Icpv2TV6MvuGMV7XR3zQ4P5pA";

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			MapServices.ProvideAPIKey (MapsApiKey);
			Console.WriteLine("attribution text google maps: {0}",Google.Maps.MapServices.OpenSourceLicenseInfo);

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

			UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
			UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);

			return true;
		}

		public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		{
			// reset our badge
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}

		public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
		{
			var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");
			var newDeviceToken = deviceToken.ToString().Replace("<", "").Replace(">", "").Replace(" ", "");

			if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(newDeviceToken))
			{
				//TODO: Put your own logic here to notify your server that the device token has changed/been created!
			}

			//Save device token now
			NSUserDefaults.StandardUserDefaults.SetString(newDeviceToken, "PushDeviceToken");
			Console.WriteLine("Device Token: " + newDeviceToken);

			string token = deviceToken.Description.Replace (" ", string.Empty).Replace ("<", string.Empty).Replace (">", string.Empty);
			string json = "{ \"deviceType\": \"ios\", \"deviceToken\": \"" + token + "\", \"channels\": [\"\"] }";
			var jsonBytes = Encoding.Default.GetBytes (json);
			InvokeInBackground(delegate {
				try{
					WebClient client = new WebClient ();
					client.Headers.Add ("X-Parse-Application-Id","yujtTQp5rfhni26rk7Pfe1nsZ6ysqbDpbETgNZO6");
					client.Headers.Add ("X-Parse-REST-API-Key","UYy34WLKGz8BK951zmHs5wzluwymhKituQLQxQ77");
					client.Headers.Add ("Content-Type","application/json");
					var result = client.UploadData ("https://api.parse.com/1/installations", "POST", jsonBytes);
					Console.WriteLine("Result Parse Installation: {0}", result.ToString());
				}catch(Exception e){
					Console.WriteLine("Exception: {0}",e);
				}
			});
		}

		public override void FailedToRegisterForRemoteNotifications (UIApplication application, NSError error)
		{
			Console.WriteLine("Failed to register for notifications");
		}

		public override void ReceivedRemoteNotification (UIApplication application, NSDictionary userInfo)
		{
			Console.WriteLine("Received Remote Notification!");
		}

	}
}