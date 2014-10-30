using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Kangou.Core.ViewModels;
using System;
using SlidingPanels.Lib;
using SlidingPanels.Lib.PanelContainers;
using Kangou.Helpers;

namespace Kangou.Touch.Views
{
	[Register("ActiveOrderListView")]
	public class ActiveOrderListView : RootMvxTableViewController
	{
		private BindableProgress _bindableProgress;
		private ActiveOrderListViewModel _viewModel;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			_viewModel = (ActiveOrderListViewModel)ViewModel;

			//Creating Table
			var source = new MvxStandardTableViewSource (TableView, UITableViewCellStyle.Subtitle, new NSString("Id"), "TitleText Format", UITableViewCellAccessory.None);
			TableView.Source = source;
			TableView.ReloadData();
			TableView.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("background.png"));

			NavigationItem.Title = "Órdenes Activas";
			_bindableProgress = new BindableProgress(TableView);

			//Binding
			var set = this.CreateBindingSet<ActiveOrderListView, ActiveOrderListViewModel>();

			set.Bind(source).To(vm => vm.ActiveOrderList);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.SelectActiveOrderCommand);
			set.Bind(_bindableProgress).For(b => b.Visible).To(vm => vm.IsBusy);
			set.Apply();

			var navigationController = NavigationController as SlidingPanelsNavigationViewController;

			if (Reachability.RemoteHostStatus () == NetworkStatus.NotReachable) {
				var internetErorAlert = new UIAlertView ("No hay conexión a Iternet", ""
					, null, "Ok", null);
				internetErorAlert.Clicked += delegate {
					navigationController.TogglePanel(PanelType.LeftPanel);
				};
				internetErorAlert.Show ();
			}

			ConnectionManager.SocketDisconnected (delegate {
				InvokeOnMainThread(delegate {

					if(StatusOrderViewModel.HasBeenClosedByUser)
						return;

					if(ReviewViewModel.HasBeenClosedByUser)
						return;

					var conecctionLostAlert = new UIAlertView ("Se ha perdido la conexión", "\nVerifica la conexión a internet", null, "Ok");
					conecctionLostAlert.Clicked += delegate {
						navigationController.TogglePanel(PanelType.LeftPanel);
					};
					conecctionLostAlert.Show();
				});
			});

			NavigationItem.BackBarButtonItem = new UIBarButtonItem ("Regresar", UIBarButtonItemStyle.Plain, null); 
			NavigationController.NavigationBar.TintColor = Constants.TINT_COLOR_SECONDARY;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			_viewModel.IsBusy = true;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			_viewModel.PopulateListFromServer ();
			if (ReviewViewModel.HasBeenClosedByUser) {
				var navigationController = NavigationController as SlidingPanelsNavigationViewController;
				navigationController.TogglePanel(PanelType.LeftPanel);
			}

		}
	}
}