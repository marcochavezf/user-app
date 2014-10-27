using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using Kangou.Core.Services.DataStore;
using Kangou.Core.ViewModels;
using Cirrious.CrossCore;

namespace Kangou.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

			RegisterAppStart (new CustomAppStart ());
        }
    }

	public class CustomAppStart : MvxNavigatingObject, IMvxAppStart
	{

		public void Start(object hint = null)
		{
			var dataService = Mvx.Resolve<IDataService>();
			if (dataService.GetUserData () == null)
			{
				ShowViewModel<EditProfileViewModel>();
			}
			else
			{
				ShowViewModel<RegisterOrderViewModel>();
			}
		}
	}
}