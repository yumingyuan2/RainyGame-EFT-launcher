using System.ComponentModel;

namespace SPT.Launcher.Models.SPT
{
    public class SPTVersion : INotifyPropertyChanged
    {
        public int Major;
        public int Minor;
        public int Build;

        public bool HasTag => Tag != null;

        private string _Tag = null;
        public string Tag
        {
            get => _Tag;
            set
            {
                if(_Tag != value)
                {
                    _Tag = value;
                    RaisePropertyChanged(nameof(Tag));
                    RaisePropertyChanged(nameof(HasTag));
                }
            }
        }

        public void ParseVersionInfo(string SPTVersion)
        {
            if (SPTVersion.Contains('-'))
            {
                string[] versionInfo = SPTVersion.Split('-');

                SPTVersion = versionInfo[0];

                Tag = versionInfo[1];
            }

            string[] splitVersion = SPTVersion.Split('.');

            if (splitVersion.Length == 3)
            {
                int.TryParse(splitVersion[0], out Major);
                int.TryParse(splitVersion[1], out Minor);
                int.TryParse(splitVersion[2], out Build);
            }
        }

        public SPTVersion() { }

        public SPTVersion(string SPTVersion)
        {
            ParseVersionInfo(SPTVersion);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public override string ToString()
        {
            return HasTag ? $"{Major}.{Minor}.{Build}-{Tag}" : $"{Major}.{Minor}.{Build}";
        }
    }
}
