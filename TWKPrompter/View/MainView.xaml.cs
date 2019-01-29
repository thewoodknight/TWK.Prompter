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
        }

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var i = (Item)tv.SelectedItem;
            string text;

            text = File.ReadAllText(i.Path);

            //This is the trickier thing to move out of the view and into the view model
            MemoryStream stream = new MemoryStream(Encoding.Default.GetBytes(text));
            TextRange range = new TextRange(rtbText.Document.ContentStart, rtbText.Document.ContentEnd);
            range.Load(stream, DataFormats.Rtf);

        }

        private void Play()
        {
            //This needs to get to the Player VM
            var richText = new TextRange(rtbText.Document.ContentStart, rtbText.Document.ContentEnd);

            //surely there is a better way?
            //A temp file could be saved with the rtbText.Document, then the path of that passed to the player.
            //This is to allow changes to be made in the editor and passed along, rather than just from file.
            ((MainViewModel)this.DataContext).Play(richText);
        }
    }
}
