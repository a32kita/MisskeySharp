﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using MisskeySharp.ClientEndpoints;
using MisskeySharp.Entities;
using MisskeySharp.InternalUtils;
using MisskeySharp.Streaming;

namespace MisskeySharp
{
    public class MisskeyService : IDisposable
    {
        private bool _disposed;
        private HttpClient _httpClient;
        

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


        public Notes Notes
        {
            get;
            private set;
        }

        public Users Users
        {
            get;
            private set;
        }

        public I I
        {
            get;
            private set;
        }

        public Hashtags Hashtags
        {
            get;
            private set;
        }

        public Following Following
        {
            get;
            private set;
        }

        public Drive Drive
        {
            get;
            private set;
        }

        public MisskeyStreamingClient Streaming
        {
            get;
            private set;
        }


        public MisskeyService(string host)
        {
            this.Host = host;

            this._disposed = false;
            this._httpClient = null;

            this.AccessToken = null;

            this.Notes = new Notes(this);
            this.Users = new Users(this);
            this.I = new I(this);
            this.Hashtags = new Hashtags(this);
            this.Following = new Following(this);
            this.Drive = new Drive(this);

            this.Streaming = new MisskeyStreamingClient(this);
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

        private string _serializeObject<TObject>(TObject obj)
        {
            return JsonSerializer.Serialize<TObject>(obj, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
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
                throw new MisskeyException("Failure to get tokens.");
            }

            var tokenString = resp.Token;
            if (String.IsNullOrEmpty(tokenString))
            {
                throw new MisskeyException("Failure to get tokens.");
            }

            this.AccessToken = tokenString;
        }

        public async Task AuthorizeWithAccessTokenAsync(string accessToken)
        {
            this._checkDisposed();
            this.AccessToken = accessToken;

            // 何らかの方法で token の有効性を試す
        }

        internal async Task<TResponse> RawGetAsync<TRequest, TResponse>(string path, TRequest request = default(TRequest)) where TResponse : MisskeyApiEntitiesBase, new()
        {
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, this._getUri(path)))
            {
                if (request != null)
                {
                    var requestJson = this._serializeObject(request);
                    var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                    httpRequest.Content = requestContent;
                }

                return await this.RawRequestAsync<TResponse>(httpRequest);
            }
        }

        internal async Task<TResponse> RawPostAsync<TRequest, TResponse>(string path, TRequest request = default(TRequest)) where TResponse : MisskeyApiEntitiesBase, new()
        {
            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, this._getUri(path)))
            {
                if (request != null)
                {
                    var requestJson = this._serializeObject(request);
                    var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                    httpRequest.Content = requestContent;
                }

                return await this.RawRequestAsync<TResponse>(httpRequest);
            }
        }

        internal async Task<TResponse> RawRequestAsync<TResponse>(HttpRequestMessage requestMessage) where TResponse : MisskeyApiEntitiesBase, new()
        {
            this._checkDisposed();
            if (this._httpClient == null)
                this._httpClient = new HttpClient();

            using (var responseMessage = await this._httpClient.SendAsync(requestMessage))
            {
                TResponse respObj = null;
                if (respObj is VoidResponse)
                    return new TResponse();

                try
                {
#if false
                    // 多分高速
                    var contentStream = await responseMessage.Content.ReadAsStreamAsync();
                    
                    respObj = await JsonSerializer.DeserializeAsync<TResponse>(contentStream, new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                    });
#else
                    var json = await responseMessage.Content.ReadAsStringAsync();
                    
                    respObj = JsonSerializer.Deserialize<TResponse>(json, new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                    });
#endif
                }
                catch (Exception ex)
                {
                    respObj = new TResponse();
                    respObj.JsonDeserializeError = ex;
                }

                respObj.HttpStatusCode = (int)responseMessage.StatusCode;
                return respObj;
            }
        }

        //public async Task<TApiResponse> GetAsync<TApiRequest, TApiResponse>(string endpoint, TApiRequest requestParam) where TApiRequest : MisskeyApiEntitiesBase where TApiResponse : MisskeyApiEntitiesBase
        //{
        //    var path = "/api/" + endpoint;
        //    requestParam.I = this.AccessToken;

        //    var resp = await this.RawGetAsync<TApiRequest, TApiResponse>(path, requestParam);
        //    if (resp.IsSuccess == false)
        //    {
        //        throw new MisskeyException(resp.Error);
        //    }

        //    return resp;
        //}

        public async Task<TApiResponse> PostAsync<TApiRequest, TApiResponse>(string endpoint, TApiRequest requestParam) where TApiRequest : MisskeyApiEntitiesBase where TApiResponse : MisskeyApiEntitiesBase, new()
        {
            var path = "/api/" + endpoint;
            requestParam.I = this.AccessToken;

            var resp = await this.RawPostAsync<TApiRequest, TApiResponse>(path, requestParam);
            if (resp.IsSuccess == false)
            {
                throw new MisskeyException(resp.Error);
            }

            return resp;
        }

        public async Task<TApiResponse> PostFormDataAsync<TApiResponse>(string endpoint, IEnumerable<MultipartUploadContent> contents) where TApiResponse : MisskeyApiEntitiesBase, new()
        {
            var path = "/api/" + endpoint;

            using (var multipart = new MultipartFormDataContent())
            {
                multipart.Add(new StringContent(this.AccessToken), "i");
                var streams = new List<Stream>();
                foreach (var content in contents)
                {
                    if (String.IsNullOrEmpty(content.StringContent) == false)
                    {
                        if (content.FileName == null)
                            multipart.Add(new StringContent(content.StringContent), content.Name);
                        else
                            multipart.Add(new StringContent(content.StringContent), content.Name, content.FileName);
                    }
                    else if (content.ByteContent != null)
                    {
                        var stream = new MemoryStream(content.ByteContent);
                        var streamContent = new StreamContent(stream);

                        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(content.ContentType);

                        if (content.FileName == null)
                            multipart.Add(streamContent, content.Name);
                        else
                            multipart.Add(streamContent, content.Name, content.FileName);

                        streams.Add(stream);
                    }
                    else
                    {
                        // 文字列コンテンツでもバイト コンテンツでもない
                        throw new Exception();
                    }
                }

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, this._getUri(path)))
                {
                    requestMessage.Content = multipart;
                    var resp = await this.RawRequestAsync<TApiResponse>(requestMessage);

                    if (resp.IsSuccess == false)
                    {
                        throw new MisskeyException(resp.Error);
                    }

                    foreach (var s in streams)
                    {
                        s?.Dispose();
                    }

                    return resp;
                }
            }
        }

        public void Dispose()
        {
            this._checkDisposed();

            this._httpClient.Dispose();
            this.Streaming.Dispose();

            this._disposed = true;
        }
    }
}
