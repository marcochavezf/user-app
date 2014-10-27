using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Kangou.Core.ViewModels;
using MBProgressHUD;

namespace Kangou.Touch.Views
{
	public class BindableProgress
	{
		private MTMBProgressHUD _progress;
		private UIView _parent;

		public BindableProgress(UIView parent)
		{
			_parent = parent;
		}

		public bool Visible
		{
			get { return _progress != null; }
			set
			{
				if (Visible == value)
					return;

				if (value)
				{
					_progress = new MTMBProgressHUD(_parent)
					{
						LabelText = "Cargando...",
						RemoveFromSuperViewOnHide = true
					};
					_parent.AddSubview(_progress);
					_progress.Show(true);
				}
				else
				{
					_progress.Hide(true);
					_progress = null;
				}
			}
		}

	}
}