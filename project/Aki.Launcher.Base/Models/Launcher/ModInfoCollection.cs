using Aki.Launcher.Models.Aki;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Aki.Launcher.Models.Launcher
{
    public class ModInfoCollection : INotifyPropertyChanged
    {
        private int _serverModsCount;
        public int ServerModsCount
        {
            get => _serverModsCount;
            set
            {
                if (_serverModsCount != value)
                {
                    _serverModsCount = value;
                    RaisePropertyChanged(nameof(ServerModsCount));
                }
            }
        }

        private int _profileModsCount;
        public int ProfileModsCount
        {
            get => _profileModsCount;
            set
            {
                if (_profileModsCount != value)
                {
                    _profileModsCount = value;
                    RaisePropertyChanged(nameof(ProfileModsCount));
                }
            }
        }

        private bool _hasProfileOnlyMods;
        public bool HasProfileOnlyMods
        {
            get => _hasProfileOnlyMods;
            set
            {
                if (_hasProfileOnlyMods != value)
                {
                    _hasProfileOnlyMods = value;
                    RaisePropertyChanged(nameof(HasProfileOnlyMods));
                }
            }
        }

        private bool _hasMods;
        public bool HasMods
        {
            get => _hasMods;
            set
            {
                if (_hasMods != value)
                {
                    _hasMods = value;
                    RaisePropertyChanged(nameof(HasMods));
                }
            }
        }

        public ObservableCollection<AkiMod> Mods { get; private set; } = new ObservableCollection<AkiMod>();

        public ModInfoCollection()
        {
            var serverMods = ServerManager.GetLoadedServerMods().Values.ToList();
            var profileMods = ServerManager.GetProfileMods().ToList();

            ServerModsCount = serverMods?.Count() ?? 0;
            ProfileModsCount = profileMods?.Count() ?? 0;

            foreach (var serverMod in serverMods)
            {
                serverMod.InServer = true;
                Mods.Add(serverMod);
            }

            foreach (var profileMod in profileMods)
            {
                var existingMod = Mods.Where(x => x.Name == profileMod.Name && x.Version == profileMod.Version && x.Author == profileMod.Author).FirstOrDefault();

                if (existingMod != null)
                {
                    existingMod.InProfile = true;
                    continue;
                }

                profileMod.InProfile = true;
                Mods.Add(profileMod);
            }

            HasMods = Mods.Count() > 0;
            HasProfileOnlyMods = Mods.Where(x => x.InProfile && !x.InServer).Count() > 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
