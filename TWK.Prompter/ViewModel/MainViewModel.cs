using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Stylet;
using TWK.Prompter.Events;
using TWK.Prompter.Models;
using TWK.Prompter.Utilities;

namespace TWK.Prompter.ViewModel
{
    public class MainViewModel : Screen, IHandle<ScriptFolderChangedEvent>, INotifyPropertyChanged
    {
        public FlowDocument Document { get; set; }
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
            this.settingsViewModel = settingsViewModel;

            Files = new ObservableCollection<Item>(GetItems(Settings.ScriptFolder));

            eventAggregator.Subscribe(this);
        }

        public void Handle(ScriptFolderChangedEvent m)
        {
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

            foreach (var file in dirInfo.GetFiles().Where(f => f.Extension == ".rtf" || f.Extension == ".docx"))
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
            {
               

                using (var fileStream = new FileStream(item.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {

                    if (item.Path.Contains(".rtf"))
                    {
                        FlowDocument flowDocument = new FlowDocument();
                        TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
                        textRange.Load(fileStream, DataFormats.Rtf);

                        Document = flowDocument;
                    }
                    else
                    {
                        var flowDocumentConverter = new DocxToFlowDocumentConverter(fileStream);
                        flowDocumentConverter.Read();
                        Document = flowDocumentConverter.Document;
                    }
                }

              
            }
        }

        public void ManualPlay()
        {
            //more params like speed, mirror, scale should be passed in?
            var doc = Document;
            this.Document = null;
            var viewModel = new ManualPlayerViewModel(eventAggregator, Settings, doc);

            windowManager.ShowDialog(viewModel);
        }

        public void ShowSettings()
        {
            windowManager.ShowDialog(settingsViewModel);
        }

        public void ScaleDownLarge() { Settings.Scale -= 1; }
        public void ScaleDownSmall() { Settings.Scale -= .1; }
        public void ScaleUpLarge() { Settings.Scale += 1; }
        public void ScaleUpSmall() { Settings.Scale += .1; }
    }
}