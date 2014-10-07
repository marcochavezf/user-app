﻿using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core.ViewModels;

namespace Kangou.Touch.Views
{
	[Register("DropOffListView")]
	public class DropOffListView : MvxTableViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var viewModel = (DropOffListViewModel)ViewModel;

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;

			//Creating Table
			var source = new MvxDeleteStandardTableViewSource (viewModel, TableView, UITableViewCellStyle.Subtitle, new NSString("Id"), "TitleText Address", UITableViewCellAccessory.None);
			TableView.Source = source;

			//Binding
			var set = this.CreateBindingSet<DropOffListView, DropOffListViewModel>();
			set.Bind(source).To(vm => vm.DropOffDataList);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.SelectDataCommand);
			set.Apply();

			TableView.ReloadData();

			//Add Button
			var addButton = new UIBarButtonItem (UIBarButtonSystemItem.Add, (sender, args) => {
				NavigationController.PopViewControllerAnimated (false);
				viewModel.AddDropOffDataCommand.Execute (null);
			});

			//Edit Button
			var editButton = new UIBarButtonItem (UIBarButtonSystemItem.Edit, (sender, args) => {
				if (TableView.Editing)
					TableView.SetEditing (false, true);
				else
					TableView.SetEditing (true, true);
			});

			//Add Navigation Bar
			NavigationItem.SetRightBarButtonItems(new UIBarButtonItem[] {
				addButton,editButton
			},true);

		}
	}
}