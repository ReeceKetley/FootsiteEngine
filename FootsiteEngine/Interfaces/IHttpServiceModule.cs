using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FootsiteEngine.Interfaces
{
    public interface IHttpServiceModule
    {
        void UpdateUserAgent(string userAgent);
        void SetProxy(WebProxy proxy, bool useProxy = false);
        void ToggleProxy(bool state);
        void ClearCookies();
        void AddHeader(string name, string value);
        void RemoveHeader(string name);
        Task<HttpResponseMessage> Get(Uri url, CancellationToken cancellationToken);
        Task<HttpResponseMessage> Post(Uri url, Dictionary<string, string> postVariables, CancellationToken cancellationToken);
    }
}