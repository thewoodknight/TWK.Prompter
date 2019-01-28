using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TWKPrompter.Messages;

namespace TWKPrompter.View
{
    public partial class PlayerView : Window
    {
        private double ScrollSpeed = 10;
        public PlayerView()
        {
            InitializeComponent();
        }

        private void PlayPause(PlayPauseMessage m)
        {
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            if (m.Playing)
            {
                Shift(svText, svText.ScrollableHeight);
            }
        }

        //https://stackoverflow.com/questions/17930481/programmatically-scrolling-a-scrollviewer-with-a-timer-becomes-jerky
        private void Shift(ScrollViewer target, double distance = 20)
        {
            double scrollSpeed = ScrollSpeed;
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

            double animationTime = distance / scrollSpeed;
            DateTime startTime = DateTime.Now;

            EventHandler renderHandler = null;
            renderHandler = (sender, args) =>
            {
                double elapsed = (DateTime.Now - startTime).TotalSeconds;

                if (elapsed >= animationTime)
                {
                    target.ScrollToVerticalOffset(destinationOffset);
                    CompositionTarget.Rendering -= renderHandler;
                }

                //if (vm.Playing)
                    target.ScrollToVerticalOffset(startOffset + (elapsed * ScrollSpeed));
            };

            CompositionTarget.Rendering += renderHandler;
        }
    }
}
