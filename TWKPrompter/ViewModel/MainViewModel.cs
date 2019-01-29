using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using Stylet;
using TWKPrompter.Messages;
using TWKPrompter.Models;

namespace TWKPrompter.ViewModel
{
    public class MainViewModel : Screen 
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

        public void Play(TextRange richText)
        {
            var viewModel = new PlayerViewModel(richText);
            windowManager.ShowWindow(viewModel);
        }

        public System.Windows.Point RenderOffsetScale
        {
            get { return new System.Windows.Point(_scale / 2, _scale / 2); }

        }

        public ObservableCollection<Item> Files { get; set; }

        private IWindowManager windowManager;
        public MainViewModel(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
            Files = new ObservableCollection<Item>(GetItems(@"C:\Users\paul\OneDrive\scripts"));
        }

        public List<Item> GetItems(string path)
        {
            var items = new List<Item>();

            var dirInfo = new DirectoryInfo(path);

            foreach (var directory in dirInfo.GetDirectories())
            {
                var item = new DirectoryItem
                {
                    Name = directory.Name,
                    Path = directory.FullName,
                    Items = GetItems(directory.FullName)
                };

                items.Add(item);
            }

            foreach (var file in dirInfo.GetFiles().Where(f  => f.Extension == ".rtf"))
            {
                
                var item = new FileItem
                {
                    Name = file.Name,
                    Path = file.FullName
                };

                items.Add(item);
            }

            return items;
        }

        public void MirrorFlip()
        {
            var x = (Mirror == -1) ? Mirror = 1 : Mirror = -1;
        }
        public void Smaller() { Scale -= 0.5; }
        public void Larger() { Scale += 0.5; }
        public void Slower() { ScrollSpeed -= 10; }
        public void Faster() { ScrollSpeed += 10; }


    }
}