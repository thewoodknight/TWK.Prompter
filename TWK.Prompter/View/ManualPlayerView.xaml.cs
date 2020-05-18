using MahApps.Metro.Controls;
using Stylet;
using System.Windows;
using System.Windows.Controls;
using TWK.Prompter.Events;
using TWK.Prompter.ViewModel;

namespace TWK.Prompter.View
{
    public partial class ManualPlayerView : MetroWindow, IHandle<ChangeMadeEvent>
    {
        private bool fullscreen = false;
        private ManualPlayerViewModel pvm { get { return (ManualPlayerViewModel)DataContext; } }

        public ManualPlayerView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            //I'm not sure I love this approach, but it works
            eventAggregator.Subscribe(this);
            Loaded += ManualPlayerView_Loaded;
        }

        private void ManualPlayerView_Loaded(object sender, RoutedEventArgs e)
        {
            FullScreen_Click(null, null);
        }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (!fullscreen)
            {
                IgnoreTaskbarOnMaximize = true;
                ShowMaxRestoreButton = false;
                ShowMinButton = false;
              //  ShowTitleBar = false;
                WindowStyle = WindowStyle.None;
                ResizeMode = ResizeMode.NoResize;

                WindowState = WindowState.Maximized;
            }
            else
            {
                IgnoreTaskbarOnMaximize = false;
                ShowMaxRestoreButton = true;
                ShowMinButton = true;
                ShowTitleBar = true;
                WindowStyle = WindowStyle.SingleBorderWindow;
                ResizeMode = ResizeMode.CanResize;

                WindowState = WindowState.Normal;
            }

            fullscreen = !fullscreen;
        }

        public void Handle(ChangeMadeEvent message)
        {
            switch (message.ChangeType)
            {
                case ChangeMadeEnum.ScrollUp:
                    svText.ScrollToVerticalOffset(svText.VerticalOffset - 100);
                    svText.UpdateLayout();
                    break;

                case ChangeMadeEnum.ScrollDown:
                    svText.ScrollToVerticalOffset(svText.VerticalOffset + 100);
                    svText.UpdateLayout();
                    break;


                // .PageUp() and .PageDown() can't be controlled, but ScrollToVerticalOffset() can have a value passed in, therefore manipulated so it doesn't do a full page down
                case ChangeMadeEnum.PageUp:
                    svText.ScrollToVerticalOffset((svText.VerticalOffset - svText.ActualHeight) - 100);
                    svText.UpdateLayout();
                    break;

                case ChangeMadeEnum.PageDown:
                    svText.ScrollToVerticalOffset((svText.VerticalOffset + svText.ActualHeight) - 100);
                    
                    //svText.PageDown();
                    svText.UpdateLayout();
                    break;
            }

        }
    }
}
