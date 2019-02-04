using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using Stylet;
using TWKPrompter.Models;

namespace TWKPrompter.ViewModel
{
    public class MainViewModel : Screen 
    {
        //Use Fody to clean up this mess of properties.
        private string _text = "";
        public string Text
        {
            get { return _text; }
            set
            {
                SetAndNotify(ref _text, value);
            }
        }

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

        public ObservableCollection<Item> Files { get; set; }

        public System.Windows.Point RenderOffsetScale
        {
            get { return new System.Windows.Point(_scale / 2, _scale / 2); }

        }
        
        private IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;

        public MainViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
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

        public void LoadFile(object sender, EventArgs e)
        {
            var item = (Item)((TreeView)sender).SelectedItem;//This is gross
            Text = File.ReadAllText(item.Path);

        }

        public void Play()
        {
            //more params like speed, mirror, scale should be passed in
            var viewModel = new PlayerViewModel(eventAggregator, Text);
            windowManager.ShowWindow(viewModel);
        }

        public void ShowSettings()
        {
            var settingsViewModel = new SettingsViewModel(eventAggregator);
            windowManager.ShowDialog(settingsViewModel);
        }
    }
}