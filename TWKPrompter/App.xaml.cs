using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace TWKPrompter
{
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
