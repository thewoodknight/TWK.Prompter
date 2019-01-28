using Microsoft.Win32;
using TWKPrompter.ViewModel;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;


namespace TWKPrompter
{
    public partial class MainWindow : Window
    {

        private MainViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = (MainViewModel)DataContext;

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<PlayPauseMessage>(this, PlayPause);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string text;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                text = File.ReadAllText(openFileDialog.FileName);
                MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(text));
                TextRange range = new TextRange(rtbText.Document.ContentStart, rtbText.Document.ContentEnd);
                range.Load(stream, DataFormats.Rtf);
            }
        }

        //https://stackoverflow.com/questions/17930481/programmatically-scrolling-a-scrollviewer-with-a-timer-becomes-jerky
        private void Shift(ScrollViewer target, double distance = 20)
        {
            double scrollSpeed = vm.ScrollSpeed;
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

                if (vm.Playing)
                    target.ScrollToVerticalOffset(startOffset + (elapsed * vm.ScrollSpeed));
            };

            CompositionTarget.Rendering += renderHandler;
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
