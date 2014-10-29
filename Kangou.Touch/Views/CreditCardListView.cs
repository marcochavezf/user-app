﻿using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core.ViewModels;
using SlidingPanels.Lib;

namespace Kangou.Touch.Views
{
	[Register("CreditCardListView")]
	public class CreditCardListView : MvxTableViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			var viewModel = (CreditCardListViewModel)ViewModel;

			//Creating Table
			var source = new MvxDeleteStandardTableViewSource (viewModel, TableView, UITableViewCellStyle.Subtitle, new NSString("Id"), "TitleText CreditCardNumber", UITableViewCellAccessory.None);
			TableView.Source = source;

			//Binding
			var set = this.CreateBindingSet<CreditCardListView, CreditCardListViewModel>();
			set.Bind(source).To(vm => vm.CreditCardDataList);
			set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.SelectDataCommand);
			set.Apply();

			TableView.ReloadData();

			//Add Button
			var addButton = new UIBarButtonItem (UIBarButtonSystemItem.Add, (sender, args) => {
				NavigationController.PopViewControllerAnimated (false);
				viewModel.AddCreditCardDataCommand.Execute (null);
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

			TableView.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("background.png"));
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			SlidingGestureRecogniser.EnableGesture = false;
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			SlidingGestureRecogniser.EnableGesture = true;
		}

	}
}