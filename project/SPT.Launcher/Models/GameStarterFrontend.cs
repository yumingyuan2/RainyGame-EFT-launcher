#nullable enable

using SPT.Launcher.Helpers;
using SPT.Launcher.Interfaces;
using SPT.Launcher.Models.Launcher;
using SPT.Launcher.ViewModels.Dialogs;
using SPT.Launcher.ViewModels.Notifications;
using Avalonia.Controls.Notifications;
using Splat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DialogHostAvalonia;

namespace SPT.Launcher.Models
{
    public class GameStarterFrontend : IGameStarterFrontend
    {
        private WindowNotificationManager? notificationManager => Locator.Current.GetService<WindowNotificationManager>();

        public async Task CompletePatchTask(IAsyncEnumerable<PatchResultInfo> task)
        {
            var manager = notificationManager ?? throw new InvalidOperationException("Notification manager not available");
            manager.Show(new SPTNotificationViewModel(null, "", $"{LocalizationProvider.Instance.patching} ..."));

            var iter = task.GetAsyncEnumerator();
            while (await iter.MoveNextAsync())
            {
                var info = iter.Current;
                if (!info.OK)
                {
                    if(info.Status == PatchResultType.InputChecksumMismatch)
                    {
                        string serverVersion = ServerManager.GetVersion();

                        var localeText = string.Format(LocalizationProvider.Instance.file_mismatch_dialog_message, serverVersion);
                        
                        var result = await DialogHost.Show(new ConfirmationDialogViewModel(null, localeText, null, null, LauncherSettingsProvider.Instance.IsDevMode));

                        if(result is bool confirmation && !confirmation)
                        {
                            manager.Show(new SPTNotificationViewModel(null, "", LocalizationProvider.Instance.failed_core_patch, NotificationType.Error));
                            throw new TaskCanceledException();
                        }
                    }
                    else
                    {
                        manager.Show(new SPTNotificationViewModel(null, "", LocalizationProvider.Instance.failed_core_patch, NotificationType.Error));
                        throw new TaskCanceledException();
                    }
                }
            }
        }
    }
}
