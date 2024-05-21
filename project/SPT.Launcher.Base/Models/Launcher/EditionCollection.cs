/* EditionCollection.cs
 * License: NCSA Open Source License
 * 
 * Copyright: SPT
 * AUTHORS:
 */


using SPT.Launcher.Models.SPT;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SPT.Launcher.Models.Launcher
{
    public class EditionCollection : INotifyPropertyChanged
    {
        private bool _HasSelection;
        public bool HasSelection
        {
            get => _HasSelection;
            set
            {
                if(_HasSelection != value)
                {
                    _HasSelection = value;
                    RaisePropertyChanged(nameof(HasSelection));
                }
            }
        }
        private int _SelectedEditionIndex;
        public int SelectedEditionIndex
        {
            get => _SelectedEditionIndex;
            set
            {
                if (_SelectedEditionIndex != value)
                {
                    _SelectedEditionIndex = value;
                    RaisePropertyChanged(nameof(SelectedEditionIndex));
                }
            }
        }

        private SPTEdition _SelectedEdition;
        public SPTEdition SelectedEdition
        {
            get => _SelectedEdition;
            set
            {
                if (_SelectedEdition != value)
                {
                    _SelectedEdition = value;
                    HasSelection = _SelectedEdition != null;
                    RaisePropertyChanged(nameof(SelectedEdition));
                }
            }
        }
        public ObservableCollection<SPTEdition> AvailableEditions { get; private set; } = new ObservableCollection<SPTEdition>();

        public EditionCollection()
        {
            SelectedEditionIndex = 0;

            foreach(var edition in ServerManager.SelectedServer.editions)
            {
                AvailableEditions.Add(new SPTEdition(edition));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
