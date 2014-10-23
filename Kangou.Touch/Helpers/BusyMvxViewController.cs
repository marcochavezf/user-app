using System;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;
using Kangou.Touch.Views;

namespace Kangou.Touch
{
	public abstract class BusyMvxViewController : MvxViewController
	{
		protected BindableProgress _bindableProgress;
		protected bool _popNextToLastViewController = false;
		protected bool _hideBackButton = false;

		public override void ViewDidLoad()
		{
			View = new UIView (){ BackgroundColor = UIColor.White };
			base.ViewDidLoad ();

			_bindableProgress = new BindableProgress(View);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			NavigationItem.HidesBackButton = _popNextToLastViewController || _hideBackButton;
		}
			
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			if (_popNextToLastViewController) {
				PopNextToLastViewController ();
				_popNextToLastViewController = false;
				_hideBackButton = true;
			}
		}

		private void PopNextToLastViewController(){
			var ViewControllers = NavigationController.ViewControllers;
			UIViewController[] newViewControllers = new UIViewController[ViewControllers.Length-1];

			var lengthNewViewControllers = ViewControllers.Length - 2;
			for (int i=0; i < lengthNewViewControllers; i++) 
				newViewControllers [i] = ViewControllers [i];

			newViewControllers [lengthNewViewControllers] = this;
			NavigationController.ViewControllers = newViewControllers;
		}
	}
}

