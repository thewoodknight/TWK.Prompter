using Stylet;
using System;
using System.ComponentModel;
using System.Windows.Input;
using TWK.HotkeyControl;
using Newtonsoft.Json;
using System.IO;

namespace TWK.Prompter
{
    public class SettingsManager : INotifyPropertyChanged
    {
        private const string settingsFile = "twk.prompter.settings";
        private Settings settings;

        public SettingsManager()
        {
            if (!File.Exists(settingsFile))
                File.WriteAllText(settingsFile, JsonConvert.SerializeObject(new Settings()
                {
                    Mirror = -1,
                    Scale = 1
                }));

            settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsFile));

        }

        public Hotkey SpeedUpKey
        {
            get
            {
                return settings.SpeedUpKey;
            }
            set
            {
                settings.SpeedUpKey = value;
                Save();
            }
        }

        public Hotkey SpeedDownKey
        {
            get { return settings.SpeedDownKey; }
            set
            {
                settings.SpeedDownKey = value;
                Save();
            }
        }


        public Hotkey BiggerKey
        {
            get { return settings.BiggerKey; }
            set
            {
                settings.BiggerKey = value;
                Save();
            }
        }

        public Hotkey SmallerKey
        {
            get { return settings.SmallerKey; }
            set
            {
                settings.SmallerKey = value;
                Save();
            }
        }
        public double Scale { get; set; }
       

        public int Mirror
        {
            get { return settings.Mirror; }
            set
            {
                settings.Mirror = value;
                Save();
            }
        }

        public string ScriptFolder
        {
            get { return settings.ScriptFolder; }
            set
            {
                settings.ScriptFolder = value;
                Save();
            }
        }

        public double ScrollSpeed
        {
            get { return settings.ScrollSpeed; }
            set
            {
                settings.ScrollSpeed = value;
                Save();
            }
        }
       
        public event PropertyChangedEventHandler PropertyChanged;

        private void Save()
        {
            File.WriteAllText(settingsFile, JsonConvert.SerializeObject(settings));
        }
    }
}
