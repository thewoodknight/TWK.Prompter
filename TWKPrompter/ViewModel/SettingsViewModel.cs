using Stylet;

namespace TWKPrompter.ViewModel
{
    public class SettingsViewModel : Screen
    {
        private readonly IEventAggregator eventAggregator;

        public SettingsViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }
    }
}
