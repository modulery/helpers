using RestSharp;
using System.Net;

namespace Moduler.Helpers
{
    public class MyWebProxyConfig
    {
        public static MyWebProxyConfig GetConfig { get; set; }
        public string WebProxyIp { get; set; }
        public string WebProxyUserName { get; set; }
        public string WebProxyPassword { get; set; }
    }
    public static class MyWebProxy
    {
        public static bool UseWebProxy;
        public static WebProxy GetWebProxy => new WebProxy(MyWebProxyConfig.GetConfig.WebProxyIp)
        {
            Credentials = new NetworkCredential(MyWebProxyConfig.GetConfig.WebProxyUserName, MyWebProxyConfig.GetConfig.WebProxyPassword)
        };
        public static RestClient GetClient(string url)
        {
            var client = new RestClient(url);
            if (UseWebProxy)
            {
                WebProxy proxy = GetWebProxy;
                var clientOptions = new RestClientOptions { Proxy = proxy };
                client = new RestClient(clientOptions);
            }
            else
            {
                var clientOptions = new RestClientOptions();
                client = new RestClient(clientOptions);
            }
            return client;
        }
    }
}
