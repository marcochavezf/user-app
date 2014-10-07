using Cirrious.CrossCore.IoC;

namespace Kangou.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
			RegisterAppStart<Kangou.Core.ViewModels.RegisterOrderViewModel>();
        }

    }
}