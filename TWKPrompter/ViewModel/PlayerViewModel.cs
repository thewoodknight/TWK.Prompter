using Stylet;
using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using TWKPrompter.Events;

namespace TWKPrompter.ViewModel
{
    public class PlayerViewModel : Screen
    {
        public double ScrollSpeed { get; set; }
        public bool Playing { get; set; }
        public SettingsManager Settings { get; set; }
        private readonly IEventAggregator eventAggregator;

        public string Text { get; set; }

        //These keys can be user configurable later
        Key PlayPauseKey = Key.Q;
        Key FasterKey = Key.D;
        Key SlowerKey = Key.A;
        Key UpKey = Key.W;
        Key DownKey = Key.S;

        Dictionary<Key, Action> ShortcutKeys = new Dictionary<Key, Action>();

        public PlayerViewModel(IEventAggregator eventAggregator, SettingsManager Settings, string text)
        {
            this.eventAggregator = eventAggregator;
            this.Settings = Settings;
            Text = text;

            InitShortcuts();
        }

        // Can't just change the key values without clearing out the old one.
        /*
         ie, PlayPauseKey = Key.Space without first removing the old value wouldn't work
         
        */
        private void InitShortcuts()
        {
            ShortcutKeys.Clear();
            ShortcutKeys.Add(PlayPauseKey, () => PlayPause());
            ShortcutKeys.Add(SlowerKey, () => Slower());
            ShortcutKeys.Add(FasterKey, () => Faster());
            ShortcutKeys.Add(UpKey, () => Larger());
            ShortcutKeys.Add(DownKey, () => Smaller());

        }

        public void MirrorFlip()
        {
            var x = (Settings.Mirror == -1) ? Settings.Mirror = 1 : Settings.Mirror = -1;
        }
        public void Smaller() { Settings.Scale -= 0.5; }
        public void Larger() { Settings.Scale += 0.5; }
        public void Slower() { ScrollSpeed -= 10; }
        public void Faster() { ScrollSpeed += 10; }

        public void PlayPause()
        {
            Playing = !Playing;
            eventAggregator.Publish(new PlayPauseEvent(Playing));
            Console.WriteLine(Playing);
        }

        public void KeyPressed(object sender, KeyEventArgs e)
        {
            Action a;
            ShortcutKeys.TryGetValue(e.Key, out a);

            a?.Invoke();
        }
    }
}
