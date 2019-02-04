using Stylet;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace TWKPrompter
{
    public class SettingsManager : INotifyPropertyChanged
    {
        public SettingsManager()
        {

        }

        public Key SpeedUpKey
        {
            get { return Properties.Settings.Default.SpeedUpKey;  }
            set
            {
                Properties.Settings.Default.SpeedUpKey = value;
                Save();
            }
        }

        public Key SpeedDownKey
        {
            get { return Properties.Settings.Default.SpeedDownKey; }
            set
            {
                Properties.Settings.Default.SpeedDownKey = value;
                Save();
            }
        }


        public Key BiggerKey
        {
            get { return Properties.Settings.Default.BiggerKey; }
            set
            {
                Properties.Settings.Default.BiggerKey = value;
                Save();
            }
        }

        public Key SmallerKey
        {
            get { return Properties.Settings.Default.SmallerKey; }
            set
            {
                Properties.Settings.Default.SmallerKey = value;
                Save();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Save()
        {
            Properties.Settings.Default.Save();
        }

        public string Name
        {
            get { return "foo"; }
            set => throw new NotImplementedException();
        }
    }
}
