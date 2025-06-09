/* RequestHandler.cs
 * License: NCSA Open Source License
 * 
 * Copyright: SPT
 * AUTHORS:
 */


using SPT.Launcher.MiniCommon;
using System.Threading.Tasks;

namespace SPT.Launcher
{
    public static class RequestHandler
    {
        private static Request request = new Request(null, "");

        public static string GetBackendUrl()
        {
            return request.RemoteEndPoint;
        }

        public static void ChangeBackendUrl(string remoteEndPoint)
        {
            request.RemoteEndPoint = remoteEndPoint;
        }

        public static void ChangeSession(string session)
        {
            request.Session = session;
        }

        public static async Task<string> RequestConnect()
        {
            return await request.GetJsonAsync("/launcher/server/connect");
        }

        public static async Task<string> RequestLogin(LoginRequestData data)
        {
            return await request.PostJsonAsync("/launcher/profile/login", Json.Serialize(data));
        }

        public static async Task<string> RequestRegister(RegisterRequestData data)
        {
            return await request.PostJsonAsync("/launcher/profile/register", Json.Serialize(data));
        }

        public static async Task<string> RequestRemove(LoginRequestData data)
        {
            return await request.PostJsonAsync("/launcher/profile/remove", Json.Serialize(data));
        }

        public static async Task<string> RequestAccount(LoginRequestData data)
        {
            return await request.PostJsonAsync("/launcher/profile/get", Json.Serialize(data));
        }

        public static async Task<string> RequestProfileInfo(LoginRequestData data)
        {
            return await request.PostJsonAsync("/launcher/profile/info", Json.Serialize(data));
        }

        public static async Task<string> RequestExistingProfiles()
        {
            return await request.GetJsonAsync("/launcher/profiles");
        }

        public static async Task<string> RequestChangeUsername(ChangeRequestData data)
        {
            return await request.PostJsonAsync("/launcher/profile/change/username", Json.Serialize(data));
        }

        public static async Task<string> RequestChangePassword(ChangeRequestData data)
        {
            return await request.PostJsonAsync("/launcher/profile/change/password", Json.Serialize(data));
        }

        public static async Task<string> RequestWipe(RegisterRequestData data)
        {
            return await request.PostJsonAsync("/launcher/profile/change/wipe", Json.Serialize(data));
        }

        public static async Task<string> SendPing()
        {
            return await request.GetJsonAsync("/launcher/ping");
        }

        public static async Task<string> RequestServerVersion()
        {
            return await request.GetJsonAsync("/launcher/server/version");
        }

        public static async Task<string> RequestCompatibleGameVersion()
        {
            return await request.GetJsonAsync("/launcher/profile/compatibleTarkovVersion");
        }

        public static async Task<string> RequestLoadedServerMods()
        {
            return await request.GetJsonAsync("/launcher/server/loadedServerMods");
        }

        public static async Task<string> RequestProfileMods()
        {
            return await request.GetJsonAsync("/launcher/server/serverModsUsedByProfile");
        }
    }
}
