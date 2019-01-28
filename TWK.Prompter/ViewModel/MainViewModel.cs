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
using TWK.Prompter.Models;

namespace TWK.Prompter.ViewModel
{
    public class MainViewModel : Screen
    {
        public string Text { get; set; }
        public ObservableCollection<Item> Files { get; set; }
        public SettingsManager Settings { get; set; }

        private IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;
        private readonly SettingsViewModel settingsViewModel;

        public MainViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, SettingsManager Settings, SettingsViewModel settingsViewModel)
        {

            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.Settings = Settings;
            this.Settings.Scale = 2;
            this.settingsViewModel = settingsViewModel;

            if (string.IsNullOrEmpty(Settings.ScriptFolder))
            {    //prompt
                Settings.ScriptFolder = @"C:\Users\paul\OneDrive\scripts";
            }
            Files = new ObservableCollection<Item>(GetItems(Settings.ScriptFolder));
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
            var item = (Item)((TreeView)sender).SelectedItem;//This is gross, but TreeView doesnt' have a bindable SelectedItem
            if (item != null)
                Text = File.ReadAllText(item.Path);

        }

        public void Play()
        {
            //more params like speed, mirror, scale should be passed in?
            var viewModel = new PlayerViewModel(eventAggregator, Settings, Text);
            windowManager.ShowWindow(viewModel);
        }

        public void ShowSettings()
        {
            windowManager.ShowDialog(settingsViewModel);
        }
    }
}