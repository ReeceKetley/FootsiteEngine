using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FootsiteEngine.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FootsiteEngine.Modules
{
    public class HttpServiceModule : IHttpServiceModule
    {
        public string UserAgent { get; }
        private readonly HttpClientHandler _clientHandler;
        private readonly HttpClient _http;

        public HttpServiceModule(string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0", WebProxy proxy = null)
        {
            if (proxy == null)
            {
                _clientHandler = new HttpClientHandler();
            }
            else
            {
                _clientHandler = new HttpClientHandler();
                _clientHandler.Proxy = proxy;
                _clientHandler.UseProxy = true;
            }
            _http = new HttpClient(_clientHandler);
            UserAgent = userAgent ?? throw new ArgumentNullException(nameof(userAgent));
            Init();
        }
            
        private void Init()
        {
            _http.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            //CheckAgent();   
        }

        public void UpdateUserAgent(string userAgent)
        {
            if (userAgent == null) throw new ArgumentNullException(nameof(userAgent));
            if (_http.DefaultRequestHeaders.Contains("User-Agent"))
            {
                _http.DefaultRequestHeaders.Remove("User-Agent");
            }

            _http.DefaultRequestHeaders.Add("User-Agent", userAgent);
        }

        public void SetProxy(WebProxy proxy, bool useProxy = false)
        {

            _clientHandler.Proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
            _clientHandler.UseProxy = useProxy;
        }

        public void ToggleProxy(bool state)
        {
            if (state)
            {
                if (_clientHandler.Proxy == null)
                {
                    throw new Exception("Proxy not set.");
                }

                _clientHandler.UseProxy = true;
            }
            else
            {
                _clientHandler.UseProxy = false;
            }
        }

        public void ClearCookies()
        {
            _clientHandler.CookieContainer = new CookieContainer();
        }

        public void AddHeader(string name, string value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            _http.DefaultRequestHeaders.Add(name, value);
        }

        public void RemoveHeader(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            if (!_http.DefaultRequestHeaders.Contains(name))
            {
                throw new Exception("Header not present to remove.");
            }
            else
            {
                _http.DefaultRequestHeaders.Remove(name);
            }
        }

        public async Task<HttpResponseMessage> Get(Uri url, CancellationToken cancellationToken)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            try
            {
                var response = await _http.GetAsync(url, cancellationToken);
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error during HTTP GET request. " + e.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> Post(Uri url, Dictionary<string, string> postVariables, CancellationToken cancellationToken)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (postVariables == null) throw new ArgumentNullException(nameof(postVariables));

            try
            {
                var response = await _http.PostAsync(url, new StringContent(GeneratePostData(postVariables)),
                    cancellationToken);
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error during HTTP Post request. " + e.Message);
                return null;
            }
        }

        private string GeneratePostData(Dictionary<string, string> postVariables)
        {
            if (postVariables == null) throw new ArgumentNullException(nameof(postVariables));
            var postContent = "";

            foreach (var postVariable in postVariables)
            {
                postContent += postVariable.Key + "=" + postVariable.Value + "&";
            }

            return postContent;
        }

        private void CheckAgent()
        {
            var result = Get(new Uri("https://www.whatismybrowser.com/detect/what-is-my-user-agent"), new CancellationToken(false));
            Console.WriteLine(result.Result.Text());
        }
    }

    public static class HttpExtensions
    {
        public static string Text(this HttpResponseMessage responseMessage)
        {
            return responseMessage.Content.ReadAsStringAsync().Result;
        }

        public static List<JObject> JsonObjects(this HttpResponseMessage responseMessage)
        {
            List<JObject> jsonObjects = new List<JObject>();
            string pattern = "<script[^>]*>(?:[^<]+|<(?!/script>))+";
            MatchCollection matches = Regex.Matches(responseMessage.Text(), pattern);
            foreach (Match match in matches)
            {
                try
                {
                    var jObject = JObject.Parse(match.Value.ToString().Substring(match.Value.IndexOf('{') - 3));
                    jsonObjects.Add(jObject);
                }
                catch
                {
                    continue;
                }
            }
            return jsonObjects;
        }
    }
}
