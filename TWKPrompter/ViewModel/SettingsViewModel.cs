using Stylet;

namespace TWKPrompter.ViewModel
{
    public class SettingsViewModel : Screen
    {
        
        private readonly IEventAggregator eventAggregator;
        private readonly SettingsManager settings;

        public SettingsViewModel(IEventAggregator eventAggregator, SettingsManager settings)
        {
            this.eventAggregator = eventAggregator;
            this.settings = settings;
        }

        public void Beep()
        {
            var x = settings.Name;
        }
    }
}
