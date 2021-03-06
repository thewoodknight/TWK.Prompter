using Stylet;
using StyletIoC;
using TWK.Prompter.ViewModel;

namespace TWK.Prompter
{
    public class Bootstrapper : Bootstrapper<MainViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            base.ConfigureIoC(builder);
            
            builder.Autobind(typeof(SettingsManager).Assembly);

            builder.Bind<SettingsManager>().ToSelf().InSingletonScope();

            //This is probably bad, but I'm not sure the best way to use the IoC
            builder.Bind<SettingsViewModel>().ToSelf().InSingletonScope();

        }

        protected override void Configure()
        {
        }
    }
}
