﻿using Stylet;
using System;
using System.ComponentModel;
using System.Windows.Input;
using TWK.HotkeyControl;
using Newtonsoft.Json;
using System.IO;
using TWK.Prompter.Models;

namespace TWK.Prompter
{
    public class SettingsManager : INotifyPropertyChanged
    {
        private const string settingsFile = "twk.prompter.settings";
        private Settings settings;

        private Settings GetDefaultSettings()
        {
            return new Settings()
            {
                ScriptFolder = @"",
                Mirror = -1,
                Scale = 1,
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsManager()
        {
            if (!File.Exists(settingsFile))
                File.WriteAllText(settingsFile, JsonConvert.SerializeObject(GetDefaultSettings()));

            settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsFile));

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

        public Hotkey ScrollUpKey
        {
            get { return settings.ScrollUpKey; }
            set
            {
                settings.ScrollUpKey = value;
                Save();
            }
        }

        public Hotkey ScrollDownKey
        {
            get { return settings.ScrollDownKey; }
            set
            {
                settings.ScrollDownKey = value;
                Save();
            }
        }

        public double Scale
        {
            get { return settings.Scale; }
            set
            {
                settings.Scale = value;
                Save();
            }
        }

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


        private void Save()
        {
            File.WriteAllText(settingsFile, JsonConvert.SerializeObject(settings));
        }
    }
}
