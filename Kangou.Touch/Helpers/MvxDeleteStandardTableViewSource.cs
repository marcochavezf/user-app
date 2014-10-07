using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;
using System.Windows.Input;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Bindings;
using Kangou.Core;

namespace Kangou.Touch.Views
{

	public class MvxDeleteStandardTableViewSource : MvxStandardTableViewSource
	{

		private IDelete _viewModel;

		#region Constructors
		public MvxDeleteStandardTableViewSource(IDelete viewModel, UITableView tableView, UITableViewCellStyle style, NSString cellIdentifier, IEnumerable<MvxBindingDescription> descriptions, UITableViewCellAccessory tableViewCellAccessory = 0) 
			: base(tableView, style, cellIdentifier, descriptions, tableViewCellAccessory)
		{
			_viewModel = viewModel;
		}

		public MvxDeleteStandardTableViewSource(IDelete viewModel, UITableView tableView, string bindingText) : base(tableView, bindingText)
		{
			_viewModel = viewModel;
		}

		public MvxDeleteStandardTableViewSource(IDelete viewModel, UITableView tableView, NSString cellIdentifier) : base(tableView, cellIdentifier)
		{
			_viewModel = viewModel;
		}

		public MvxDeleteStandardTableViewSource(IDelete viewModel, UITableView tableView) : base(tableView)
		{
			_viewModel = viewModel;
		}

		public MvxDeleteStandardTableViewSource(IDelete viewModel, UITableView tableView, UITableViewCellStyle style, NSString cellId, string binding, UITableViewCellAccessory accessory)
			: base(tableView, style, cellId, binding, accessory)
		{
			_viewModel = viewModel;
		}
		#endregion

		public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
		{
			return true;
		}

		public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			switch (editingStyle)
			{
			case UITableViewCellEditingStyle.Delete:
				_viewModel.DeleteDataByIndex (indexPath.Row);
				break;
			case UITableViewCellEditingStyle.None:
				break;
			}
		}

		public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return UITableViewCellEditingStyle.Delete;
		}

		public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
		{
			return false;
		}

	}

}
