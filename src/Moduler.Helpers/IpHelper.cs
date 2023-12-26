using RestSharp;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace Moduler.Helpers
{
    public class IpHelper
    {
        public static string GetIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }
        private static string? _HostPublicIp { get; set; }
        public static string? GetHostPublicIp
        {
            get
            {
                if (string.IsNullOrEmpty(_HostPublicIp))
                {
                    string url = "http://ifconfig.me/ip";

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            _HostPublicIp = client.GetStringAsync(url).Result;
                            Console.WriteLine(_HostPublicIp);
                        }
                        catch (HttpRequestException ex)
                        {
                            Console.WriteLine($"Hata oluştu: {ex.Message}");
                        }
                    }
                }
                return _HostPublicIp;
            }
        }
        private static string? _ProxyIp { get; set; }
        public static string? GetProxyIp
        {
            get
            {
                if (MyWebProxy.UseWebProxy && string.IsNullOrEmpty(_ProxyIp))
                {
                    string url = "http://ifconfig.me/ip";
                    var client = MyWebProxy.GetClient(url.ToString());
                    try
                    {
                        var request = new RestRequest(url.ToString(), method: Method.Get);
                        _ProxyIp = client.Get(request).Content;
                        Console.WriteLine(_ProxyIp);
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Hata oluştu: {ex.Message}");
                    }
                }
                return _ProxyIp;
            }
        }
    }
}
