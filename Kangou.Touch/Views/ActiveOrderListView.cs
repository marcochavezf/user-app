using CoreGraphics;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using ObjCRuntime;
using UIKit;
using Foundation;
using Kangou.Core;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Kangou.Core.ViewModels;
using System;
using SlidingPanels.Lib;
using SlidingPanels.Lib.PanelContainers;
using Kangou.Helpers;
using System.Collections.Generic;

namespace Kangou.Touch.Views
{
	[Register("ActiveOrderListView")]
	public class ActiveOrderListView : RootMvxViewController
	{
		private BindableProgress _bindableProgress;
		private ActiveOrderListViewModel _viewModel;
		private UILabel _thereIsNotActiveOrdersLabel;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			_viewModel = (ActiveOrderListViewModel)ViewModel;

			var CONTAINER_SIZE = View.Bounds.Size;
			var WIDTH = UIScreen.MainScreen.Bounds.Width;
			var HEIGHT = UIScreen.MainScreen.Bounds.Height;
			var MARGIN_SUBVIEWS = WIDTH * 0.15f;
			var LABEL_FONT_SIZE = 17.5f;
			var LABEL_FONT = "Arial-BoldMT";

			//Creating Table
			var tableView = new UITableView(new CGRect(0, 0, WIDTH, HEIGHT), UITableViewStyle.Plain);
			tableView.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("background.png"));
			var source = new MvxStandardTableViewSource (tableView, UITableViewCellStyle.Subtitle, new NSString("Id"), "TitleText Format", UITableViewCellAccessory.None);
			tableView.Source = source;
			Add(tableView);

			var pYoffset = NavigationController.NavigationBar.Frame.Y + NavigationController.NavigationBar.Frame.Height + MARGIN_SUBVIEWS;

			_thereIsNotActiveOrdersLabel = new UILabel (new CGRect(MARGIN_SUBVIEWS, pYoffset, WIDTH-MARGIN_SUBVIEWS*2, 20));
			_thereIsNotActiveOrdersLabel.Text = "No hay órdenes activas";
			_thereIsNotActiveOrdersLabel.TextAlignment = UITextAlignment.Center;
			_thereIsNotActiveOrdersLabel.Font = UIFont.FromName(LABEL_FONT, LABEL_FONT_SIZE);
			_thereIsNotActiveOrdersLabel.TextColor = UIColor.Gray;
			Add (_thereIsNotActiveOrdersLabel);

			NavigationItem.Title = "Órdenes Activas";
			_bindableProgress = new BindableProgress(View);

			View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("background.png"));

			//Binding
			var set = this.CreateBindingSet<ActiveOrderListView, ActiveOrderListViewModel>();
			set.Bind(source).To(vm => vm.ActiveOrderList);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.SelectActiveOrderCommand);
			set.Bind(_bindableProgress).For(b => b.Visible).To(vm => vm.IsBusy);
			set.Bind(tableView).For("Visibility").To(vm => vm.IsTableVisible).WithConversion("Visibility");
			set.Bind(_thereIsNotActiveOrdersLabel).For("Visibility").To(vm => vm.IsTableVisible).WithConversion("InvertedVisibility");
			set.Apply();

			NavigationItem.BackBarButtonItem = new UIBarButtonItem ("Regresar", UIBarButtonItemStyle.Plain, null); 
			NavigationController.NavigationBar.TintColor = Constants.TINT_COLOR_SECONDARY;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			_viewModel.PublishMessageViewOpened ();
			_thereIsNotActiveOrdersLabel.Hidden = true;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			_viewModel.IsBusy = true;
			_viewModel.PopulateListFromServer ();

			var navigationController = NavigationController as SlidingPanelsNavigationViewController;
			if (Reachability.RemoteHostStatus () == NetworkStatus.NotReachable) {
				var internetErorAlert = new UIAlertView ("No hay conexión a Iternet", "", null, "Ok", null);
				internetErorAlert.Clicked += delegate {
					_viewModel.ActiveOrderList = new List<ActiveOrder> ();
					navigationController.TogglePanel(PanelType.LeftPanel);
				};
				internetErorAlert.Show ();
			}
		}
	}
}