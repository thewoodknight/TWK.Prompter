using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using Xceed.Wpf.Toolkit;

namespace TWK.Prompter.Converters
{
    //https://stackoverflow.com/questions/343468/richtextbox-wpf-binding
    public class RichTextBoxHelper : DependencyObject
    {
        public static string GetDocumentXaml(DependencyObject obj)
        {
            return (string)obj.GetValue(DocumentXamlProperty);
        }

        public static void SetDocumentXaml(DependencyObject obj, string value)
        {
            obj.SetValue(DocumentXamlProperty, value);
        }

        public static readonly DependencyProperty DocumentXamlProperty = 
            DependencyProperty.RegisterAttached("DocumentXaml", 
                typeof(FlowDocument), 
                typeof(RichTextBoxHelper),
                new FrameworkPropertyMetadata
                {
                    BindsTwoWayByDefault = true,
                    PropertyChangedCallback = Handle
                });

        public static void Handle(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var richTextBox = (RichTextBox)obj;
            if (e.NewValue != null)
                richTextBox.Document = (FlowDocument)e.NewValue; 
            else
                richTextBox.Document = new FlowDocument();

        }

    }
}