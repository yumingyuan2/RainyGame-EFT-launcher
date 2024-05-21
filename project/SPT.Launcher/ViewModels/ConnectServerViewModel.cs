using SPT.Launcher.Models.SPT;
using SPT.Launcher.Helpers;
using SPT.Launcher.Models.Launcher;
using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using SPT.Launcher.Controllers;

namespace SPT.Launcher.ViewModels
{
    public class ConnectServerViewModel : ViewModelBase
    {
        private bool noAutoLogin = false;

        public ConnectServerModel connectModel { get; set; } = new ConnectServerModel()
        {
            InfoText = LocalizationProvider.Instance.server_connecting
        };

        public ConnectServerViewModel(IScreen Host, bool NoAutoLogin = false) : base(Host)
        {
            noAutoLogin = NoAutoLogin;

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                Task.Run(async () =>
                {
                   await ConnectServer();
                });
            });
        }

        public async Task ConnectServer()
        {
            LauncherSettingsProvider.Instance.AllowSettings = false;
            
            if (!await ServerManager.LoadDefaultServerAsync(LauncherSettingsProvider.Instance.Server.Url))
            {
                connectModel.ConnectionFailed = true;
                connectModel.InfoText = string.Format(LocalizationProvider.Instance.server_unavailable_format_1,
                    LauncherSettingsProvider.Instance.Server.Name);
                
                LauncherSettingsProvider.Instance.AllowSettings = true;
                return;
            }
            
            bool connected = ServerManager.PingServer();

            connectModel.ConnectionFailed = !connected;

            connectModel.InfoText = connected ? LocalizationProvider.Instance.ok : string.Format(LocalizationProvider.Instance.server_unavailable_format_1, LauncherSettingsProvider.Instance.Server.Name);

            if (connected)
            {
                SPTVersion version = Locator.Current.GetService<SPTVersion>("sptversion");

                version.ParseVersionInfo(ServerManager.GetVersion());
                
                LogManager.Instance.Info($"Connected to server: {ServerManager.SelectedServer.backendUrl} - SPT Version: {version}");

                NavigateTo(new LoginViewModel(HostScreen, noAutoLogin));
            }
            
            LauncherSettingsProvider.Instance.AllowSettings = true;
        }

        public void RetryCommand()
        {
            connectModel.InfoText = LocalizationProvider.Instance.server_connecting;

            connectModel.ConnectionFailed = false;

            Task.Run(async () =>
            {
                await ConnectServer();
            });
        }
    }
}
