using MisskeySharp.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MisskeySharp
{
    public class MisskeyService : IDisposable
    {
        private bool _disposed;
        private HttpClient _httpClient;
        //private string _accessToken;

        public string Host
        {
            get;
            private set;
        }

        public string AccessToken
        {
            get;
            private set;
        }

        public bool IsAuthorized
        {
            get => this.AccessToken != null;
        }


        public MisskeyService(string host)
        {
            this.Host = host;

            this._disposed = false;
            this._httpClient = null;

            this.AccessToken = null;
        }


        private void _checkDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().FullName);
        }

        private Uri _getUri(string path, IDictionary<string, string> queryParameters = null)
        {
            var host = this.Host.EndsWith("/") ? this.Host.Substring(0, this.Host.Length - 1) : this.Host;
            var svAbsPath = path[0] == '/' ? path : "/" + path;

            var queryParameterStr = String.Empty;
            if (queryParameters != null && queryParameters.Count > 0)
            {
                queryParameterStr += "?" + String.Join("&", queryParameters.Select(kvp => kvp.Key + "=" + Uri.EscapeDataString(kvp.Value)));
            }

            var uriStr = host + svAbsPath + queryParameterStr;
            return new Uri(uriStr);
        }


        public MisskeyAuthorizeUriInfo GetAuthorizeUri(string appName, string iconUrl, string callbackUri, MisskeyPermissions permissions)
        {
            this._checkDisposed();

            var sessionId = Guid.NewGuid().ToString();

            var permissionStrs = MisskeyPermissionsExtensions.GetAllValues()
                .Where(v => v != 0 && (permissions & v) == v).Select(v => v.ToString().ToLower().Replace("__", "-").Replace("_", ":"));
            var permissionStr = String.Join(",", permissionStrs);

            var authorizeUri = this._getUri($"/miauth/{sessionId}", new Dictionary<string, string>()
            {
                { "name", appName },
                { "icon", iconUrl },
                { "callback", callbackUri },
                { "permission", permissionStr }
            });

            return new MisskeyAuthorizeUriInfo(sessionId, authorizeUri);
        }

        public async Task AuthorizeWithAuthorizeUriAsync(MisskeyAuthorizeUriInfo authorizeUriInfo)
        {
            this._checkDisposed();
            
            var resp = await this.RawPostAsync<object, TokenResponse>($"/api/miauth/{authorizeUriInfo.SessionId}/check");
            if (resp.Ok == false)
            {
                throw new Exception("Failure to get tokens.");
            }

            var tokenString = resp.Token;
            if (String.IsNullOrEmpty(tokenString))
            {
                throw new Exception("Failure to get tokens.");
            }

            this.AccessToken = tokenString;
        }

        public async Task AuthorizeWithAccessTokenAsync(string accessToken)
        {
            this._checkDisposed();
            this.AccessToken = accessToken;

            // 何らかの方法で token の有効性を試す
        }

        public async Task<TResponse> RawGetAsync<TRequest, TResponse>(string path, TRequest request = default(TRequest))
        {
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, this._getUri(path)))
            {
                if (request != null)
                {
                    var requestJson = JsonSerializer.Serialize(request);

                    httpRequest.Headers.Add("Content-Type", "application/json");
                    httpRequest.Content = new StringContent(requestJson);
                }

                return await this.RawRequestAsync<TResponse>(httpRequest);
            }
        }

        public async Task<TResponse> RawPostAsync<TRequest, TResponse>(string path, TRequest request = default(TRequest))
        {
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, this._getUri(path)))
            {
                if (request != null)
                {
                    var requestJson = JsonSerializer.Serialize(request);

                    httpRequest.Headers.Add("Content-Type", "application/json");
                    httpRequest.Content = new StringContent(requestJson);
                }

                return await this.RawRequestAsync<TResponse>(httpRequest);
            }
        }

        public async Task<TResponse> RawRequestAsync<TResponse>(HttpRequestMessage requestMessage)
        {
            this._checkDisposed();
            if (this._httpClient == null)
                this._httpClient = new HttpClient();

            using (var responseMessage = await this._httpClient.SendAsync(requestMessage))
            {
                var contentStream = await responseMessage.Content.ReadAsStreamAsync();

                //if (true)
                //{
                //    using (var sr = new StreamReader(contentStream))
                //    {
                //        var responseBody = sr.ReadToEnd();
                //        Console.WriteLine(responseBody);
                //    }
                //}

                //var respJsonData = await JsonDocument.ParseAsync(contentStream);
                //return respJsonData.Deserialize<TResponse>();

                return await JsonSerializer.DeserializeAsync<TResponse>(contentStream, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });
            }
        }

        public async Task<TApiResponse> Request<TApiRequest, TApiResponse>(string endpoint, TApiRequest requestParam) where TApiRequest : MisskeyApiRequestParam
        {
            var path = "/api/" + endpoint;
            requestParam.I = this.AccessToken;

            return await this.RawGetAsync<TApiRequest, TApiResponse>(path, requestParam);
        }

        public void Dispose()
        {
            this._checkDisposed();
        }
    }
}
