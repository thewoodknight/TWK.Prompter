using Stylet;
using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using TWK.HotkeyControl;
using TWK.Prompter.Events;

namespace TWK.Prompter.ViewModel
{
    public class PlayerViewModel : Screen
    {
        public double ScrollSpeed { get; set; }
        public bool Playing { get; set; }
        public SettingsManager Settings { get; set; }
        private readonly IEventAggregator eventAggregator;

        public string Text { get; set; }

        WindowsHotkeyService hotkeyservice = new WindowsHotkeyService();

        public PlayerViewModel(IEventAggregator eventAggregator, SettingsManager Settings, string text)
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

            if (Settings.SpeedDownKey != null)
                hotkeyservice.RegisterHotkey(Settings.SpeedDownKey, () => Slower());

            if (Settings.SpeedUpKey != null)
                hotkeyservice.RegisterHotkey(Settings.SpeedUpKey, () => Faster());

            if (Settings.ScrollUpKey != null)
                hotkeyservice.RegisterHotkey(Settings.ScrollUpKey, () => Faster());

            if (Settings.ScrollDownKey != null)
                hotkeyservice.RegisterHotkey(Settings.ScrollDownKey, () => Faster());

            if (Settings.PlayPauseKey != null)
                hotkeyservice.RegisterHotkey(Settings.PlayPauseKey, () => PlayPause());
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

        public void Slower()
        {
            eventAggregator.Publish(new ChangeMadeEvent());
            Settings.ScrollSpeed -= 10;
        }
        public void Faster()
        {
            eventAggregator.Publish(new ChangeMadeEvent());
            Settings.ScrollSpeed += 10;
        }

        public void ScrollUp()
        {
            //TODO
        }

        public void ScrollDown()
        {
            //TODO
        }

        public void PlayPause()
        {
            Playing = !Playing;
            eventAggregator.Publish(new PlayPauseEvent(Playing));
            Console.WriteLine(Playing);
        }
    }
}
