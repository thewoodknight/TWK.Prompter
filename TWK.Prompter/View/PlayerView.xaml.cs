using MahApps.Metro.Controls;
using Stylet;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TWK.Prompter.Events;
using TWK.Prompter.ViewModel;

namespace TWK.Prompter.View
{
    public partial class PlayerView : MetroWindow, IHandle<PlayPauseEvent>, IHandle<ChangeMadeEvent>
    {
        private bool fullscreen = false;
        private bool playing = false;
        private PlayerViewModel pvm { get { return (PlayerViewModel)DataContext; } }

        public PlayerView(IEventAggregator eventAggregator)
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
            } else
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
        public void Handle(PlayPauseEvent m)
        {

            playing = m.Playing;

            if (m.Playing)
            {
                
                Shift(svText, svText.ScrollableHeight - svText.VerticalOffset);
            }
        }


        EventHandler renderHandler = null;

        //https://stackoverflow.com/questions/17930481/programmatically-scrolling-a-scrollviewer-with-a-timer-becomes-jerky
        private void Shift(ScrollViewer target, double distance = 20)
        {
            double startOffset = target.VerticalOffset;
            double destinationOffset = target.VerticalOffset + distance;

            if (destinationOffset < 0)
            {
                destinationOffset = 0;
                distance = target.VerticalOffset;
            }

            if (destinationOffset > target.ScrollableHeight)
            {
                destinationOffset = target.ScrollableHeight;
                distance = target.ScrollableHeight - target.VerticalOffset;
            }

            double scrollspeed = pvm.Settings.ScrollSpeed;
            double animationTime = distance / scrollspeed;
            
            DateTime startTime = DateTime.Now;

            
            renderHandler = (sender, args) =>
            {
                double elapsed = (DateTime.Now - startTime).TotalSeconds;

                if (elapsed >= animationTime)
                {
                    target.ScrollToVerticalOffset(destinationOffset);
                    CompositionTarget.Rendering -= renderHandler;
                }

                if (playing)
                    target.ScrollToVerticalOffset(startOffset + (elapsed * scrollspeed));
                

            };

            CompositionTarget.Rendering += renderHandler;
        }

        public void Handle(ChangeMadeEvent message)
        {
            playing = false;
            CompositionTarget.Rendering -= renderHandler;
            renderHandler = null;

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
            }
            

            playing = true;
            Shift(svText, svText.ScrollableHeight - svText.VerticalOffset);
        }
    }
}
