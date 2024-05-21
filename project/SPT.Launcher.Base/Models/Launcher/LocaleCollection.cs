/* LocaleCollection.cs
 * License: NCSA Open Source License
 * 
 * Copyright: SPT
 * AUTHORS:
 * waffle.lord
 */


using SPT.Launcher.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SPT.Launcher.Models.Launcher
{
    public class LocaleCollection : INotifyPropertyChanged
    {
        private string _SelectedLocale;
        public string SelectedLocale
        {
            get => _SelectedLocale;
            set
            {
                if (_SelectedLocale != value)
                {
                    _SelectedLocale = value;
                    RaisePropertyChanged(nameof(SelectedLocale));
                    LocalizationProvider.LoadLocalByName(value);
                }
            }
        }

        public ObservableCollection<string> AvailableLocales { get; set; } = LocalizationProvider.GetAvailableLocales();

        public event PropertyChangedEventHandler PropertyChanged;

        public LocaleCollection()
        {
            SelectedLocale = LocalizationProvider.LocaleNameDictionary.GetValueOrDefault(LauncherSettingsProvider.Instance.DefaultLocale, "English");
        }

        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
