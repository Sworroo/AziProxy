using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace Proxy_by_Azi
{
    class Program
    {
        private static string refferer;

        static void Main(string[] args)
        {
            Console.WriteLine("Enter your Client ID: ");
            refferer = Console.ReadLine();
            GetProxys();
            Console.WriteLine("\nProxy list loaded!\n");
            start();
            Console.ReadKey();
        }

        public static List<string> ProxyList = new List<string>();
        private static Random rnd = new Random();
        private static bool _start;
        private static int _error;
        private static int _test;
        private static int _send;
        private static string GenerateUniqCode(int len)
        {
            string _allstring = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string _re = "";
            while (_re.Length < len)
            {
                _re += _allstring[rnd.Next(0, _allstring.Length - 1)].ToString();
            }
            return _re;
        }
        public static void GetProxys()
        {
            ProxyList.Clear();
            try
            {
                HttpWebResponse response = (HttpWebResponse)((HttpWebRequest)WebRequest.Create("https://api.proxyscrape.com?request=getproxies&proxytype=http&timeout=1000000&country=all&ssl=all&anonymity=all")).GetResponse();
                string end = new StreamReader(response.GetResponseStream()).ReadToEnd();
                MatchCollection matchCollections = new Regex("[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}:[0-9]{1,5}").Matches(end);
                try
                {
                    try
                    {
                        foreach (object obj in matchCollections)
                        {
                            Match objectValue = (Match)RuntimeHelpers.GetObjectValue(obj);
                            ProxyList.Add(objectValue.Value);
                        }
                    }
                    finally
                    {
                    }
                }
                finally
                {
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Error on loading ProxyList:\n"+error.Message);
            }
        }
        public static void restartAPP()
        {
            GetProxys();
            Console.WriteLine("ProxyList reloaded");
            start();
        }
        private static void start()
        {
            if (ProxyList.Count != 0)
            {
                _start = true;
                int num = 100;
                ThreadPool.SetMinThreads(num, num);
                ThreadPool.SetMaxThreads(num, num);
                try
                {
                    foreach (string state in ProxyList)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(mtd), state);
                    }
                    //Console.WriteLine("END OF PROXYLIST"); //for debug
                    restartAPP();
                }
                catch (Exception error)
                {
                    Console.WriteLine("Smth wrong! \n"+error);
                }
                finally
                {
                }
            }
            else
            {
                Console.WriteLine("Restart me!"); restartAPP();
            }
        }
        public static string Generateint(int len)
        {
            string text = "0123456789";
            string text2 = "";
            while (text2.Length < len)
            {
                text2 += text[rnd.Next(0, text.Length - 1)].ToString();
            }
            return text2;
        }
        public static void mtd(object Proxiess)
        {
            bool start = _start;
            if (start)
            {
                try
                {
                    string host = Proxiess.ToString().Split(new char[]
                              {
                        ':'
                              })[0];
                    string value = Proxiess.ToString().Split(new char[]
                    {
                        ':'
                    })[1];
                    WebProxy proxy = new WebProxy(host, Convert.ToInt32(value));
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.cloudflareclient.com/v0a" + Generateint(3) + "/reg");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.Headers.Add("Accept-Encoding", "gzip");
                    httpWebRequest.ContentType = "application/json; charset=UTF-8";
                    httpWebRequest.Host = "api.cloudflareclient.com";
                    httpWebRequest.KeepAlive = true;
                    httpWebRequest.UserAgent = "okhttp/3.12.1";
                    httpWebRequest.Proxy = proxy;
                    string install_id = GenerateUniqCode(22);
                    string key = GenerateUniqCode(43) + "=";
                    string tos = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fff") + "+07:00";
                    string fcm_token = install_id + ":APA91b" + GenerateUniqCode(134);
                    string referer = refferer;
                    string type = "Android";
                    string locale = "en-GB";
                    var body = new
                    {
                        install_id = install_id,
                        key = key,
                        tos = tos,
                        fcm_token = fcm_token,
                        referrer = referer,
                        warp_enabled = false,
                        type = type,
                        locale = locale
                    };
                    string jsonBody = JsonConvert.SerializeObject(body);
                    using (StreamWriter sw = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        sw.Write(jsonBody);
                    }
                    httpWebRequest.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (StreamReader sw = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string result = sw.ReadToEnd();
                    }
                    httpResponse = null;
                    _test++;
                    _send++;
                    if (_send > 128)
                    {
                        Console.WriteLine(Math.Round((double)_send / 1024, 2, MidpointRounding.AwayFromZero) + " TB Successfully added to your account.");
                    }else
                        Console.WriteLine(_send + " GB Successfully added to your account.");
                }
                catch (Exception ex)
                {
                    _test++;
                    _error++;
                                        /*For DEBUG
                    Console.WriteLine("error: "+_error.ToString());
                    Console.WriteLine(ex.Message);*/
                }
            }
        }
    }
}
