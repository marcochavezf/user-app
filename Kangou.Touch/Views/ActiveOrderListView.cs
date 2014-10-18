using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core;
using Cirrious.MvvmCross.Binding.Touch.Views;

namespace Kangou.Touch.Views
{
	[Register("ActiveOrderListView")]
	public class ActiveOrderListView : RootMvxTableViewController
	{
		private BindableProgress _bindableProgress;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			var viewModel = (ActiveOrderListViewModel)ViewModel;

			//Creating Table
			var source = new MvxStandardTableViewSource (TableView, UITableViewCellStyle.Subtitle, new NSString("Id"), "TitleText Format", UITableViewCellAccessory.None);
			TableView.Source = source;
			TableView.ReloadData();

			_bindableProgress = new BindableProgress(TableView);


			//Binding
			var set = this.CreateBindingSet<ActiveOrderListView, ActiveOrderListViewModel>();

			set.Bind(source).To(vm => vm.ActiveOrderList);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.SelectDataCommand);
			set.Bind(_bindableProgress).For(b => b.Visible).To(vm => vm.IsBusy);
			set.Apply();
		}
	}
}