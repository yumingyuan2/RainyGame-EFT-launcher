/* ClientConfig.cs
 * License: NCSA Open Source License
 * 
 * Copyright: SPT
 * AUTHORS:
 */


namespace SPT.Launcher
{
    public class ClientConfig
    {
        public string BackendUrl;
        public string MatchingVersion;

        public ClientConfig()
        {
            BackendUrl = "http://127.0.0.1:6969";
            MatchingVersion = "live";
        }

        public ClientConfig(string backendUrl)
        {
            BackendUrl = backendUrl;
            MatchingVersion = "live";
        }
    }
}
