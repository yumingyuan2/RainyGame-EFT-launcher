/* AccountManager.cs
 * License: NCSA Open Source License
 * 
 * Copyright: SPT
 * AUTHORS:
 * waffle.lord
 */


using System;
using SPT.Launcher.Helpers;
using SPT.Launcher.MiniCommon;
using SPT.Launcher.Models.SPT;
using SPT.Launcher.Models.Launcher;
using System.Threading.Tasks;
using SPT.Launcher.Controllers;

namespace SPT.Launcher
{
    public enum AccountStatus
    {
        OK = 0,
        NoConnection = 1,
        LoginFailed = 2,
        RegisterFailed = 3,
        UpdateFailed = 4
    }

    public static class AccountManager
    {
        private const string STATUS_FAILED = "FAILED";
        private const string STATUS_OK = "OK";
        public static AccountInfo SelectedAccount { get; private set; } = null;
        public static ProfileInfo SelectedProfileInfo { get; private set; } = null;

        public static void Logout() => SelectedAccount = null;

        public static async Task<AccountStatus> LoginAsync(LoginModel Creds)
        {
            return await LoginAsync(Creds.Username, Creds.Password);
        }

        public static async Task<AccountStatus> LoginAsync(string username, string password)
        {
            LoginRequestData data = new LoginRequestData(username, password);
            string id = STATUS_FAILED;
            string json = "";

            try
            {
                id = await RequestHandler.RequestLogin(data);

                if (id == STATUS_FAILED)
                {
                    return AccountStatus.LoginFailed;
                }

                json = await RequestHandler.RequestAccount(data);
            }
            catch
            {
                return AccountStatus.NoConnection;
            }

            SelectedAccount = Json.Deserialize<AccountInfo>(json);
            RequestHandler.ChangeSession(SelectedAccount.id);

            await UpdateProfileInfoAsync();

            return AccountStatus.OK;
        }

        public static async Task UpdateProfileInfoAsync()
        {
            LoginRequestData data = new LoginRequestData(SelectedAccount.username, SelectedAccount.password);
            string profileInfoJson = await RequestHandler.RequestProfileInfo(data);

            if (profileInfoJson != null)
            {
                ServerProfileInfo serverProfileInfo = Json.Deserialize<ServerProfileInfo>(profileInfoJson);
                SelectedProfileInfo = new ProfileInfo(serverProfileInfo);
            }
        }

        public static async Task<ServerProfileInfo[]> GetExistingProfilesAsync()
        {
            string profileJsonArray = await RequestHandler.RequestExistingProfiles();

            if(profileJsonArray != null)
            {
                ServerProfileInfo[] miniProfiles = Json.Deserialize<ServerProfileInfo[]>(profileJsonArray);

                if (miniProfiles != null && miniProfiles.Length > 0)
                {
                    return miniProfiles;
                }
            }

            return [];
        }

        public static async Task<AccountStatus> RegisterAsync(string username, string password, string edition)
        {
            RegisterRequestData data = new RegisterRequestData(username, password, edition);
            string registerStatus = STATUS_FAILED;

            try
            {
                registerStatus = await RequestHandler.RequestRegister(data);

                if (registerStatus != STATUS_OK)
                {
                    return AccountStatus.RegisterFailed;
                }
            }
            catch
            {
                return AccountStatus.NoConnection;
            }
            
            LogManager.Instance.Info($"Account Registered: {username}");

            return await LoginAsync(username, password);
        }

        public static async Task<AccountStatus> RemoveAsync()
        {
            LoginRequestData data = new LoginRequestData(SelectedAccount.username, SelectedAccount.password);

            try
            {
                string json = await RequestHandler.RequestRemove(data);

                if(Json.Deserialize<bool>(json))
                {
                    SelectedAccount = null;
                    
                    LogManager.Instance.Info($"Account Removed: {data.username}");

                    return AccountStatus.OK;
                }
                else
                {
                    LogManager.Instance.Error($"Failed to remove account: {data.username}");
                    return AccountStatus.UpdateFailed;
                }
            }
            catch
            {
                LogManager.Instance.Error($"Failed to remove account: {data.username} - NO CONNECTION");
                return AccountStatus.NoConnection;
            }
        }

        public static async Task<AccountStatus> ChangeUsernameAsync(string username)
        {
            ChangeRequestData data = new ChangeRequestData(SelectedAccount.username, SelectedAccount.password, username);
            string json = STATUS_FAILED;

            try
            {
                json = await RequestHandler.RequestChangeUsername(data);

                if (json != STATUS_OK)
                {
                    return AccountStatus.UpdateFailed;
                }
            }
            catch
            {
                return AccountStatus.NoConnection;
            }

            ServerSetting DefaultServer = LauncherSettingsProvider.Instance.Server;

            if (DefaultServer.AutoLoginCreds != null)
            {
                DefaultServer.AutoLoginCreds.Username = username;
            }

            SelectedAccount.username = username;
            LauncherSettingsProvider.Instance.SaveSettings();

            return AccountStatus.OK;
        }

        public static async Task<AccountStatus> ChangePasswordAsync(string password)
        {
            ChangeRequestData data = new ChangeRequestData(SelectedAccount.username, SelectedAccount.password, password);
            string json = STATUS_FAILED;

            try
            {
                json = await RequestHandler.RequestChangePassword(data);

                if (json != STATUS_OK)
                {
                    return AccountStatus.UpdateFailed;
                }
            }
            catch
            {
                return AccountStatus.NoConnection;
            }

            ServerSetting DefaultServer = LauncherSettingsProvider.Instance.Server;

            if (DefaultServer.AutoLoginCreds != null)
            {
                DefaultServer.AutoLoginCreds.Password = password;
            }

            SelectedAccount.password = password;
            LauncherSettingsProvider.Instance.SaveSettings();

            return AccountStatus.OK;
        }

        public static async Task<AccountStatus> WipeAsync(string edition)
        {
            RegisterRequestData data = new RegisterRequestData(SelectedAccount.username, SelectedAccount.password, edition);
            string json = STATUS_FAILED;

            try
            {
                json = await RequestHandler.RequestWipe(data);

                if (json != STATUS_OK)
                {
                    LogManager.Instance.Error($"Failed to wipe account: {data.username}");
                    return AccountStatus.UpdateFailed;
                }
            }
            catch
            {
                LogManager.Instance.Error($"Failed to wipe account: {data.username} - NO CONNECTION");
                return AccountStatus.NoConnection;
            }

            SelectedAccount.edition = edition;
            SelectedAccount.wipe = true;
            LogManager.Instance.Info($"Account Wiped: {data.username} -> {edition}");
            return AccountStatus.OK;
        }
    }
}
