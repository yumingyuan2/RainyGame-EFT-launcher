/* ServerManager.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * Merijn Hendriks
 */


using Aki.Launch.Models.Aki;
using Aki.Launcher.MiniCommon;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aki.Launcher
{
    public static class ServerManager
    {
        public static ServerInfo SelectedServer { get; private set; } = null;

        public static bool PingServer()
        {
            string json = "";

            try
            {
                json = RequestHandler.SendPing();

                if(json != null) return true;
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static string GetVersion()
        {
            try
            {
                string json = RequestHandler.RequestServerVersion();

                return Json.Deserialize<string>(json);
            }
            catch
            {
                return "";
            }
        }

        public static string GetCompatibleGameVersion()
        {
            try
            {
                string json = RequestHandler.RequestCompatibleGameVersion();

                return Json.Deserialize<string>(json);
            }
            catch
            {
                return "";
            }
        }

        public static Dictionary<string, AkiServerModInfo> GetLoadedServerMods()
        {
            try
            {
                string json = RequestHandler.RequestLoadedServerMods();

                return Json.Deserialize<Dictionary<string, AkiServerModInfo>>(json);
            }
            catch
            {
                return new Dictionary<string, AkiServerModInfo>();
            }
        }

        public static AkiProfileModInfo[] GetProfileMods()
        {
            try
            {
                string json = RequestHandler.RequestProfileMods();

                return Json.Deserialize<AkiProfileModInfo[]>(json);
            }
            catch
            {
                return new AkiProfileModInfo[] { };
            }
        }

        public static void LoadServer(string backendUrl)
        {
            string json = "";

            try
            {
                RequestHandler.ChangeBackendUrl(backendUrl);
                json = RequestHandler.RequestConnect();
            }
            catch
            {
                SelectedServer = null;
                return;
            }

            SelectedServer = Json.Deserialize<ServerInfo>(json);
        }

        public static async Task LoadDefaultServerAsync(string server)
        {
            await Task.Run(() =>
            {
                LoadServer(server);
            });
        }
    }
}
