using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace JenkinsNewsreader
{
    class LineData
    {
        public DateTime time;
        public string text;
    }

    class Program
    {
        private static string[] urls = new[]
        {
            "http://192.160.15.234:8080/view/IG-Release/job/Odyssey-Release-Android-Res/",
            "http://192.160.15.234:8080/view/IG-Release/job/Odyssey-Release-Android/",
            "http://192.160.15.234:8080/view/ODS/job/IG-Dev-iOS-Res/",
            "http://192.160.15.234:8080/view/ODS/job/Odyssey-Dev-Android-Res/",
        };

        private static List<LineData> lines = new List<LineData>();
        private static DateTime startTime;
        private static int textSize = 0;
        private static string taskName;
        private static string fileName;

        static void Main(string[] args)
        {
            string url = "";
            int num = 0;
            GetConfigValue(out url, out num);
            Console.WriteLine("url = " + url);
            Console.WriteLine("num = " + num);
            if (string.IsNullOrEmpty(url) || num < 1)
            {
                Console.WriteLine("config error.");
                return;
            }

            var ii = url.LastIndexOf("/", url.Length - 2);
            taskName = url.Substring(ii + 1, url.Length - 2 - ii);

            startTime = DateTime.Now;
            fileName = $"{taskName} {startTime.Month}-{startTime.Day} {startTime.Hour}_{startTime.Minute}_{startTime.Second}.txt";

            while(true)
            {
                try
                {
                    var postDataStr = "";
                    if (textSize > 0)
                    {
                        postDataStr = "?start=" + textSize;
                    }

                    var logUrl = url + num + "/logText/progressiveHtml" + postDataStr;
                    HttpWebRequest request = HttpWebRequest.Create(logUrl) as HttpWebRequest;
                    request.Method = "Get";
                    Console.WriteLine(logUrl);

                    HttpWebResponse rp = request.GetResponse() as HttpWebResponse;
                    var v1 = rp.GetResponseHeader("X-Text-Size");
                    textSize = Convert.ToInt32(v1);

                    var v2 = rp.GetResponseHeader("X-More-Data");
                    Console.WriteLine("v2 " + v2);

                    StreamReader StreamReader = new StreamReader(rp.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                    UpdateLog(StreamReader.ReadToEnd());

                    if(v2 != "true")
                    {
                        Console.WriteLine("finished");
                        break;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                Thread.Sleep(3000);
            }

            Console.ReadKey();
        }

        static void UpdateLog(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return;
            }

            int lineBegin = 0;
            int lineEnd = 0;
            int index = 0;
            while (lineBegin >= 0 && lineEnd >= 0)
            {
                lineEnd = html.IndexOf("\n", lineBegin);
                string line = "";
                if (lineEnd >= 0)
                {
                    line = html.Substring(lineBegin, lineEnd - lineBegin);
                    lineBegin = lineEnd + 1;
                }
                else
                {
                    line = html.Substring(lineBegin, html.Length - lineBegin);
                }

                if(!string.IsNullOrEmpty(line))
                {
                    var d = new LineData();
                    d.time = System.DateTime.Now;
                    d.text = line;
                    lines.Add(d);
                }

                index++;
            }

            StringBuilder sb = new StringBuilder("");
            foreach (var line in lines)
            {
                var diff = line.time - startTime;
                sb.Append(line.time.ToString("HH:m:s") + "    " + diff.ToString(@"hh\:mm\:ss") + "    " + line.text);
            }
            File.WriteAllText(fileName, sb.ToString());
        }

        public static void Log(string str)
        {
            Console.WriteLine(System.DateTime.Now + ": " + str);
        }

        static void GetConfigValue(out string url, out int num)
        {
            var keyUrl = "url = ";
            var keyNum = "num = ";
            url = "";
            num = 0;
            foreach (var line in File.ReadLines("config.txt"))
            {
                if (line.StartsWith("//"))
                {
                    continue;
                }

                if (line.StartsWith(keyUrl))
                {
                    url = line.Substring(keyUrl.Length);
                }

                if(line.StartsWith(keyNum))
                {
                    var numText = line.Substring(keyNum.Length);
                    if (!int.TryParse(numText, out num))
                    {
                        Console.WriteLine("num value error: " + numText);
                    }
                }
            }
        }
    }
}
