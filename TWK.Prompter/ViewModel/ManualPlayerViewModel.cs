using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWK.HotkeyControl;
using TWK.Prompter.Events;

namespace TWK.Prompter.ViewModel
{
    public class ManualPlayerViewModel : Screen
    {
        public SettingsManager Settings { get; set; }
        private readonly IEventAggregator eventAggregator;

        public string Text { get; set; }

        WindowsHotkeyService hotkeyservice = new WindowsHotkeyService();

        public ManualPlayerViewModel(IEventAggregator eventAggregator, SettingsManager Settings, string text)
        {
            this.eventAggregator = eventAggregator;
            this.Settings = Settings;

            InitShortcuts();

            Text = text;

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
            if (Settings.BiggerKey != null)
                hotkeyservice.RegisterHotkey(Settings.BiggerKey, () => Larger());

            if (Settings.SmallerKey != null)
                hotkeyservice.RegisterHotkey(Settings.SmallerKey, () => Smaller());

            if (Settings.ScrollUpKey != null)
                hotkeyservice.RegisterHotkey(Settings.ScrollUpKey, () => ScrollUp());

            if (Settings.ScrollDownKey != null)
                hotkeyservice.RegisterHotkey(Settings.ScrollDownKey, () => ScrollDown());

            //Page Down
            if (Settings.SpeedDownKey != null)
                hotkeyservice.RegisterHotkey(Settings.SpeedDownKey, () => PageDown());


            //PageUp
            if (Settings.SpeedUpKey != null)
                hotkeyservice.RegisterHotkey(Settings.SpeedUpKey, () => PageUp());
        }

        public void MirrorFlip()
        {
            var x = (Settings.Mirror == -1) ? Settings.Mirror = 1 : Settings.Mirror = -1;
        }

        public void Smaller()
        {
            //eventAggregator.Publish(new ChangeMadeEvent());
            Settings.Scale -= 0.5;
        }
        public void Larger()
        {
            //eventAggregator.Publish(new ChangeMadeEvent());
            Settings.Scale += 0.5;
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
