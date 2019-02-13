using Stylet;

namespace TWKPrompter.ViewModel
{
    public class SettingsViewModel : Screen
    {
        public string Text { get; set; }
        public double ScrollSpeed { get; set; }
        public bool Playing { get; set; }
        public int Mirror { get; set; }
        
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
