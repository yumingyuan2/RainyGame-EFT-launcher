/* Request.cs
 * License: NCSA Open Source License
 * 
 * Copyright: SPT
 * AUTHORS:
 */

#nullable enable

using ComponentAce.Compression.Libs.zlib;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SPT.Launcher.MiniCommon
{
    public class Request : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _session;
        private readonly string _remoteEndPoint;
        private bool _disposed;

        public string Session => _session;
        public string RemoteEndPoint => _remoteEndPoint;

        public Request(string session, string remoteEndPoint)
        {
            _session = session;
            _remoteEndPoint = remoteEndPoint;
            
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = delegate { return true; }
            };
            
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(_remoteEndPoint),
                Timeout = TimeSpan.FromSeconds(30)
            };
            
            // Set default headers
            if (!string.IsNullOrWhiteSpace(_session))
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", $"PHPSESSID={_session}");
                _httpClient.DefaultRequestHeaders.Add("SessionId", _session);
            }
            
            _httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "deflate");
        }

        public async Task<Stream> SendAsync(string url, string method = "GET", string? data = null, bool compress = true)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Request));

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(method), url);

                if (method != "GET" && !string.IsNullOrWhiteSpace(data))
                {
                    // Set request body
                    byte[] bytes = compress ? 
                        SimpleZlib.CompressToBytes(data, zlibConst.Z_BEST_COMPRESSION) : 
                        Encoding.UTF8.GetBytes(data);

                    request.Content = new ByteArrayContent(bytes);
                    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    
                    if (compress)
                    {
                        request.Content.Headers.ContentEncoding.Add("deflate");
                    }
                }

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadAsStreamAsync();
            }
            catch (Exception ex)
            {
                // Log exception properly
                System.Diagnostics.Debug.WriteLine($"Request failed: {ex.Message}");
                throw;
            }
        }

        public Stream Send(string url, string method = "GET", string? data = null, bool compress = true)
        {
            return SendAsync(url, method, data, compress).GetAwaiter().GetResult();
        }

        public string GetJson(string url, bool compress = true)
        {
            using var stream = Send(url, "GET", null, compress);
            if (stream == null) return string.Empty;
            
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return SimpleZlib.Decompress(ms.ToArray(), null);
        }

        public async Task<string> GetJsonAsync(string url, bool compress = true)
        {
            using var stream = await SendAsync(url, "GET", null, compress);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return SimpleZlib.Decompress(ms.ToArray(), null);
        }

        public string PostJson(string url, string data, bool compress = true)
        {
            using var stream = Send(url, "POST", data, compress);
            if (stream == null) return string.Empty;
            
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return SimpleZlib.Decompress(ms.ToArray(), null);
        }

        public async Task<string> PostJsonAsync(string url, string data, bool compress = true)
        {
            using var stream = await SendAsync(url, "POST", data, compress);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return SimpleZlib.Decompress(ms.ToArray(), null);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _httpClient?.Dispose();
                _disposed = true;
            }
        }
    }
}
