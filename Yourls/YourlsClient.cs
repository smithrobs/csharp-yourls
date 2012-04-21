using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Net;
using System.Web;

namespace Yourls
{
    /// <summary>
    /// yourls - A node module for calling the yourls API
    /// See http://yourls.org/#API for details
    /// about the API requests and responses
    /// </summary>
    public class YourlsClient
    {
        private class Config
        {
            public string api_token { get; set; }
            public string api_url { get; set; }
            public string format { get; set; }
        }

        private Config _config;

        /// <summary>
        /// The main yourls constructor, takes the yourls api url, api token and additional options
        /// </summary>
        /// <param name="url">The URL of the Yourls service being used</param>
        /// <param name="apiKey">The users API token</param>
        public YourlsClient(string url, string apiKey)
        {
            _config = new Config
                            {
                                api_url = url,
                                api_token = apiKey,
                                format = "json"
                            };
        }

        /// <summary>
        /// Generate querystring from a dictionary
        /// </summary>
        /// <param name="items">Dictionary of string key values</param>
        /// <returns></returns>
        private string ToQueryString(Dictionary<string, string> items)
        {
            return "?" + string.Join("&", items.Select(kv => string.Format("{0}={1}", HttpUtility.UrlEncode(kv.Key), HttpUtility.UrlEncode(kv.Value))).ToArray());
        }

        /// <summary>
        /// Generates the URL object to be passed to the HTTP request for a specific
        /// API method call
        /// </summary>
        /// <param name="query">The query object</param>
        /// <returns>The URL object for this request</returns>
        private Uri GenerateNiceUrl(Dictionary<string, string> query)
        {
            return new UriBuilder("http", _config.api_url, 80, "/yourls-api.php", ToQueryString(query)).Uri;
        }

        /// <summary>
        /// Function to do a HTTP Get request with the current query
        /// </summary>
        /// <param name="request">The current request uri</param>
        /// <returns>JSON response</returns>
        private JsonValue DoRequest(Uri request)
        {
            var req = WebRequest.Create(request);
            string responseFromServer;
            using (var response = (HttpWebResponse)req.GetResponse())
            using (var dataStream = response.GetResponseStream())
            using (var reader = new StreamReader(dataStream))
            {
                responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            return JsonValue.Parse(responseFromServer);
        }

        /// <summary>
        /// Request to shorten one long url
        /// </summary>
        /// <param name="longUrl">The URL to be shortened</param>
        /// <returns>Result</returns>
        public dynamic Shorten(string longUrl)
        {
            var query = new Dictionary<string, string>
                            {
                                {"signature", _config.api_token},
                                {"format", _config.format},
                                {"url", longUrl},
                                {"action", "shorturl"}
                            };

            return DoRequest(GenerateNiceUrl(query)).AsDynamic();
        }

        /// <summary>
        /// Request to shorten one long url and return a vanity url
        /// </summary>
        /// <param name="longUrl">The URL to be shortened</param>
        /// <param name="vanityName">The requested vanity url</param>
        /// <returns>Result</returns>
        public dynamic Vanity(string longUrl, string vanityName)
        {
            var query = new Dictionary<string, string>
                            {
                                {"signature", _config.api_token},
                                {"format", _config.format},
                                {"url", longUrl},
                                {"keyword", vanityName},
                                {"action", "shorturl"}
                            };

            return DoRequest(GenerateNiceUrl(query)).AsDynamic();
        }

        /// <summary>
        /// Request to expand a single short url or hash
        /// </summary>
        /// <param name="item">The short url or hash to expand</param>
        /// <returns>Result</returns>
        public dynamic Expand(string item)
        {
            var query = new Dictionary<string, string>
                            {
                                {"signature", _config.api_token},
                                {"format", _config.format},
                                {"shorturl", item},
                                {"action", "expand"}
                            };

            return DoRequest(GenerateNiceUrl(query)).AsDynamic();
        }

        /// <summary>
        /// Request to retrieve stats on a specific short url/hash
        /// </summary>
        /// <param name="item">The short url or hash to get stats on</param>
        /// <returns>Result</returns>
        public dynamic UrlStats(string item)
        {
            var query = new Dictionary<string, string>
                            {
                                {"signature", _config.api_token},
                                {"format", _config.format},
                                {"shorturl", item},
                                {"action", "url-stats"}
                            };

            return DoRequest(GenerateNiceUrl(query)).AsDynamic();
        }
    }
}
