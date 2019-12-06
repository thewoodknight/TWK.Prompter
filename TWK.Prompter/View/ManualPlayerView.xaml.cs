using MahApps.Metro.Controls;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TWK.Prompter.Events;
using TWK.Prompter.ViewModel;

namespace TWK.Prompter.View
{
    /// <summary>
    /// Interaction logic for ManualPlayerView.xaml
    /// </summary>
    public partial class ManualPlayerView : MetroWindow, IHandle<ChangeMadeEvent>
    {
        private bool fullscreen = false;
        private ManualPlayerViewModel pvm { get { return (ManualPlayerViewModel)DataContext; } }

        public ManualPlayerView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            //I'm not sure I love this approach, but it works
            eventAggregator.Subscribe(this);
        }

        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (!fullscreen)
            {
                IgnoreTaskbarOnMaximize = true;
                ShowMaxRestoreButton = false;
                ShowMinButton = false;
                ShowTitleBar = false;
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

        public void Shift(ScrollViewer target, double distance = 20)
        {

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

                case ChangeMadeEnum.PageUp:
                    svText.PageUp();
                    svText.UpdateLayout();
                    break;

                case ChangeMadeEnum.PageDown:
                    svText.PageDown();
                    svText.UpdateLayout();
                    break;
            }

        }
    }
}
