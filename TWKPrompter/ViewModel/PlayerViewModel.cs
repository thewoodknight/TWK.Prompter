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
        private double _scrollspeed = 20;
        public double ScrollSpeed
        {
            get { return _scrollspeed; }
            set
            {
                SetAndNotify(ref _scrollspeed, value);
            }
        }

        private double _scale = 2;
        public double Scale
        {
            get { return _scale; }
            set
            {
                SetAndNotify(ref _scale, value);
                NotifyOfPropertyChange(() => RenderOffsetScale);
            }
        }

        private int _mirror = -1;
        public int Mirror
        {
            get { return _mirror; }
            set
            {
                SetAndNotify(ref _mirror, value);
            }
        }

        private bool _playing = false;
        public bool Playing
        {
            get { return _playing; }
            set
            {
                SetAndNotify(ref _playing, value);
            }
        }


        public System.Windows.Point RenderOffsetScale
        {
            get { return new System.Windows.Point(_scale / 2, _scale / 2); }

        }

        private string _text = "";
        private readonly IEventAggregator eventAggregator;

        public string Text
        {
            get { return _text; }
            set
            {
                SetAndNotify(ref _text, value);
            }
        }

        //These keys can be user configurable later
        Key PlayPauseKey = Key.Q;
        Key FasterKey = Key.D;
        Key SlowerKey = Key.A;
        Key UpKey = Key.W;
        Key DownKey = Key.S;

        Dictionary<Key, Action> ShortcutKeys = new Dictionary<Key, Action>();

        public PlayerViewModel(IEventAggregator eventAggregator, string text)
        {
            this.eventAggregator = eventAggregator;
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
            var x = (Mirror == -1) ? Mirror = 1 : Mirror = -1;
        }
        public void Smaller() { Scale -= 0.5; }
        public void Larger() { Scale += 0.5; }
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
