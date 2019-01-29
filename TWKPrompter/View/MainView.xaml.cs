using System.Windows;
using System.Windows.Input;
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
            //I don't believe Stylet has Caliburn Micro's "Message Attach", and TreeView doesn't have a command
            ((MainViewModel)this.DataContext).Load(); 
        }
    }
}
