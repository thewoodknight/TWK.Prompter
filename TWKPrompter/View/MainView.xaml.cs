using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using TWKPrompter.Models;
using TWKPrompter.ViewModel;

namespace TWKPrompter.View
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            var itemProvider = new ItemProvider();

            var items = itemProvider.GetItems(@"C:\Users\paul\OneDrive\scripts");

            tv.DataContext = items;
        }

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var i = (Item)tv.SelectedItem;
            string text;

            text = File.ReadAllText(i.Path);
            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(text));
            TextRange range = new TextRange(rtbText.Document.ContentStart, rtbText.Document.ContentEnd);
            range.Load(stream, DataFormats.Rtf);

            Play();
        }

        private void Play()
        {
            //This needs to get to the Player VM
            var richText = new TextRange(rtbText.Document.ContentStart, rtbText.Document.ContentEnd);

            //surely there is a better way?
            //Messenger.Default.Send<TextRangeMessage>(new TextRangeMessage(richText));

            //var x = new PlayerViewModel();

            ((MainViewModel)this.DataContext).Play(richText);
        }
    }

    public class TextRangeMessage
    {
        public TextRangeMessage(TextRange textRange)
        {
            TextRange = textRange;
        }

        public TextRange TextRange { get; }
    }
}
