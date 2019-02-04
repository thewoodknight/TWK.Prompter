using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using PropertyChanged;
using Stylet;
using TWKPrompter.Models;

namespace TWKPrompter.ViewModel
{
    public class MainViewModel : Screen
    {
        public string Text { get; set; }
        public double ScrollSpeed { get; set; }
        public bool Playing { get; set; }
        public int Mirror { get; set; }

        [AlsoNotifyFor("RenderOffsetScale")]
        public double Scale { get; set; }

        public ObservableCollection<Item> Files { get; set; }

        public System.Windows.Point RenderOffsetScale
        {
            get { return new System.Windows.Point(Scale / 2, Scale / 2); }

        }

        private IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;
        private readonly SettingsManager settings;
        private readonly SettingsViewModel settingsViewModel;

        public MainViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, SettingsManager Settings, SettingsViewModel settingsViewModel)
        {
            Scale = 2;
            Mirror = -1;
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            settings = Settings;
            this.settingsViewModel = settingsViewModel;
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

            foreach (var file in dirInfo.GetFiles().Where(f => f.Extension == ".rtf"))
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
            windowManager.ShowDialog(settingsViewModel);
        }
    }
}