using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using TWKPrompter.Models;

namespace TWKPrompter.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            var itemProvider = new ItemProvider();

            var items = itemProvider.GetItems(@"C:\Users\paul\OneDrive\scripts");

            DataContext = items;
        }

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var i = (Item)tv.SelectedItem;
            string text;

            text = File.ReadAllText(i.Path);
            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(text));
            TextRange range = new TextRange(rtbText.Document.ContentStart, rtbText.Document.ContentEnd);
            range.Load(stream, DataFormats.Rtf);

        }
    }
}
