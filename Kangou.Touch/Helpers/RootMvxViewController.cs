using System;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.UIKit;
using SlidingPanels.Lib.PanelContainers;
using System.Drawing;
using SlidingPanels.Lib;

namespace Kangou.Touch
{
	public class RootMvxViewController : MvxViewController
	{
		public RootMvxViewController(){

		}

		public RootMvxViewController (string xibName) : base (xibName, null)
		{
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (false);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (false);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (false);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (false);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View = new UIView(){ BackgroundColor = UIColor.White};
			NavigationItem.LeftBarButtonItem = CreateSliderButton("SlideLeft40.png", PanelType.LeftPanel);

			if (NavigationController.ViewControllers.Length > 1) {
				if (UIDevice.CurrentDevice.CheckSystemVersion (7, 8)) {
					//Version 8
					ConvertToRoot();
				} else {
					SlidingPanelsNavigationViewController navController = NavigationController as SlidingPanelsNavigationViewController;
					navController.TogglePanel(PanelType.LeftPanel);
				}
			}
		}

		protected UIBarButtonItem CreateSliderButton(string imageName, PanelType panelType)
		{
			UIButton button = new UIButton(new RectangleF(0, 0, 40f, 40f));
			button.SetBackgroundImage(UIImage.FromBundle(imageName), UIControlState.Normal);
			button.TouchUpInside += delegate
			{
				SlidingPanelsNavigationViewController navController = NavigationController as SlidingPanelsNavigationViewController;
				try
				{
					navController.TogglePanel(panelType);
				} catch (Exception e){
					Console.WriteLine("EXCEPTION CATCHED IN RootMvxViewController! {0}",e);
				}
			};
			return new UIBarButtonItem(button);
		}

		private void ConvertToRoot(){
			NavigationItem.HidesBackButton = true;
			var currentViewControllers = NavigationController.ViewControllers;

			var belowViewControllers = new UIViewController[currentViewControllers.Length];
			UIViewController topViewController = this;

			var itsFound = false;
			var offsetNewViewCotrollers = 0;

			for (int i = 0; i < currentViewControllers.Length; i++) {
				Console.WriteLine ("Current Title: {0}, Memory: {1}", currentViewControllers [i].NavigationItem.Title, currentViewControllers [i]);
				if (this.NavigationItem.Title == currentViewControllers [i].NavigationItem.Title) {
					if (itsFound)
						continue;

					Console.WriteLine ("Top: {0}, Memory: {1}", currentViewControllers [i].NavigationItem.Title, currentViewControllers [i]);

					topViewController = currentViewControllers[i];
					itsFound = true;
				} else {
					belowViewControllers [offsetNewViewCotrollers++] = currentViewControllers [i];
					Console.WriteLine ("Below: {0}, Memory: {1}", currentViewControllers [i].NavigationItem.Title, currentViewControllers [i]);
				}
			}
				
			var newViewControllers = new UIViewController[offsetNewViewCotrollers +1];

			for (int i = 0; i < offsetNewViewCotrollers; i++)
				newViewControllers [i] = belowViewControllers [i];

			newViewControllers [offsetNewViewCotrollers] = topViewController;
			NavigationController.ViewControllers = newViewControllers;

			/*
			for (int i = 0; i < newViewControllers.Length; i++)
				Console.WriteLine ("New Title: {0}, Memory: {1}", newViewControllers [i].NavigationItem.Title, newViewControllers [i]);
			*/

			SlidingPanelsNavigationViewController navController = NavigationController as SlidingPanelsNavigationViewController;
			navController.TogglePanel (PanelType.LeftPanel);
		}
	}
}

