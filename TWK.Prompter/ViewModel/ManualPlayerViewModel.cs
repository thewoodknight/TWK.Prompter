using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TWK.HotkeyControl;
using TWK.Prompter.Events;

namespace TWK.Prompter.ViewModel
{
    public class ManualPlayerViewModel : Screen
    {
        public SettingsManager Settings { get; set; }
        private readonly IEventAggregator eventAggregator;

        public FlowDocument Document { get; set; }

        WindowsHotkeyService hotkeyservice = new WindowsHotkeyService();

        public ManualPlayerViewModel(IEventAggregator eventAggregator, SettingsManager Settings, FlowDocument document)
        {
            this.eventAggregator = eventAggregator;
            this.Settings = Settings;

            InitShortcuts();

            Document = document;

        }

        protected override void OnClose()
        {
            base.OnClose();

            hotkeyservice.UnregisterAllHotkeys();
        }

        // Can't just change the key values without clearing out the old one.
        /*
         ie, PlayPauseKey = Key.Space without first removing the old value wouldn't work
         
        */
        private void InitShortcuts()
        {
            if (Settings.PageUpKey != null)
                hotkeyservice.RegisterHotkey(Settings.PageUpKey, () => PageUp());

            if (Settings.PageDownKey != null)
                hotkeyservice.RegisterHotkey(Settings.PageDownKey, () => PageDown());

            if (Settings.ScrollUpKey != null)
                hotkeyservice.RegisterHotkey(Settings.ScrollUpKey, () => ScrollUp());

            if (Settings.ScrollDownKey != null)
                hotkeyservice.RegisterHotkey(Settings.ScrollDownKey, () => ScrollDown());

        }

        public void MirrorFlip()
        {
            var x = (Settings.Mirror == -1) ? Settings.Mirror = 1 : Settings.Mirror = -1;
        }

        public void ScrollUp()
        {
            eventAggregator.Publish(new ChangeMadeEvent(ChangeMadeEnum.ScrollUp));
        }

        public void ScrollDown()
        {
            eventAggregator.Publish(new ChangeMadeEvent(ChangeMadeEnum.ScrollDown));
        }

        public void PageDown()
        {
            eventAggregator.Publish(new ChangeMadeEvent(ChangeMadeEnum.PageDown));
        }

        public void PageUp()
        {
            eventAggregator.Publish(new ChangeMadeEvent(ChangeMadeEnum.PageUp));
        }
    }
}
