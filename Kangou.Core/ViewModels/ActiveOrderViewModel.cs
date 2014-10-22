using System;
using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using System.Diagnostics;

namespace Kangou.Core.ViewModels
{

	public class ActiveOrderViewModel : MvxViewModel
	{
		public void Init(ActiveOrder activeOrder)
		{
			Debug.WriteLine ("ActiveORder: {0}",activeOrder);
		}

		private string _centerText;
		public string CenterText
		{
			get
			{
				return _centerText;
			}
			set
			{
				_centerText = value;
				RaisePropertyChanged(() => CenterText);
			}
		}


		public ICommand DoSomethingCommand
		{
			get
			{
				return new MvxCommand(DoSomething);
			}
		}

		void DoSomething ()
		{
			CenterText = "You Did Something!";
		}

		public ActiveOrderViewModel()
		{
			CenterText = "This is additional Functionality";
		}
	}
}

