/* RequestHandler.cs
 * License: NCSA Open Source License
 * 
 * Copyright: SPT
 * AUTHORS:
 */

#nullable enable

using SPT.Launcher.MiniCommon;

namespace SPT.Launcher
{
    public static class RequestHandler
    {
        private static Request? _request;

        public static string GetBackendUrl()
        {
            return _request?.RemoteEndPoint ?? "";
        }

        public static void ChangeBackendUrl(string remoteEndPoint)
        {
            // Dispose existing request and create new one
            _request?.Dispose();
            _request = new Request(GetCurrentSession(), remoteEndPoint);
        }

        public static void ChangeSession(string session)
        {
            string currentEndPoint = GetBackendUrl();
            // Dispose existing request and create new one with updated session
            _request?.Dispose();
            _request = new Request(session, currentEndPoint);
        }

        private static string GetCurrentSession()
        {
            return _request?.Session ?? "";
        }

        private static Request GetOrCreateRequest()
        {
            if (_request == null)
            {
                _request = new Request("", "");
            }
            return _request;
        }

        public static string RequestConnect()
        {
            return GetOrCreateRequest().GetJson("/launcher/server/connect");
        }

        public static string RequestLogin(LoginRequestData data)
        {
            return GetOrCreateRequest().PostJson("/launcher/profile/login", Json.Serialize(data));
        }

        public static string RequestRegister(RegisterRequestData data)
        {
            return GetOrCreateRequest().PostJson("/launcher/profile/register", Json.Serialize(data));
        }

        public static string RequestRemove(LoginRequestData data)
        {
            return GetOrCreateRequest().PostJson("/launcher/profile/remove", Json.Serialize(data));
        }

        public static string RequestAccount(LoginRequestData data)
        {
            return GetOrCreateRequest().PostJson("/launcher/profile/get", Json.Serialize(data));
        }

        public static string RequestProfileInfo(LoginRequestData data)
        {
            return GetOrCreateRequest().PostJson("/launcher/profile/info", Json.Serialize(data));
        }

        public static string RequestExistingProfiles()
        {
            return GetOrCreateRequest().GetJson("/launcher/profiles");
        }

        public static string RequestChangeUsername(ChangeRequestData data)
        {
            return GetOrCreateRequest().PostJson("/launcher/profile/change/username", Json.Serialize(data));
        }

        public static string RequestChangePassword(ChangeRequestData data)
        {
            return GetOrCreateRequest().PostJson("/launcher/profile/change/password", Json.Serialize(data));
        }

        public static string RequestWipe(RegisterRequestData data)
        {
            return GetOrCreateRequest().PostJson("/launcher/profile/change/wipe", Json.Serialize(data));
        }

        public static string SendPing()
        {
            return GetOrCreateRequest().GetJson("/launcher/ping");
        }

        public static string RequestServerVersion()
        {
            return GetOrCreateRequest().GetJson("/launcher/server/version");
        }

        public static string RequestCompatibleGameVersion()
        {
            return GetOrCreateRequest().GetJson("/launcher/profile/compatibleTarkovVersion");
        }

        public static string RequestLoadedServerMods()
        {
            return GetOrCreateRequest().GetJson("/launcher/server/loadedServerMods");
        }

        public static string RequestProfileMods()
        {
            return GetOrCreateRequest().GetJson("/launcher/server/serverModsUsedByProfile");
        }
    }
}
