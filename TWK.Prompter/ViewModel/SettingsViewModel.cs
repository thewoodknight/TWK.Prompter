using Stylet;
using TWK.Prompter.Events;

namespace TWK.Prompter.ViewModel
{
    public class SettingsViewModel : Screen
    {
        public string Text { get; set; }
        public double ScrollSpeed { get; set; }
        public bool Playing { get; set; }
        public int Mirror { get; set; }
        
        private readonly IEventAggregator eventAggregator;
        public SettingsManager Settings { get; set; }

        public SettingsViewModel(IEventAggregator eventAggregator, SettingsManager settings)
        {
            this.eventAggregator = eventAggregator;
            this.Settings = settings;

            //Safe guard against randomly being set to zero-movement
            if (Settings.ScrollSpeed <= 1)
                Settings.ScrollSpeed = 10;
        }

        public void ChangePath()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result != System.Windows.Forms.DialogResult.OK)
                    return;

                Settings.ScriptFolder = dialog.SelectedPath;
                eventAggregator.Publish(new ScriptFolderChangedEvent());
            }

            
        }
    }
}
