using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace JenkinsHelp
{
    class ConfigManager
    {
        private static ConfigManager instance;
        public const string KEY_OUTPUT_DEV = "OutputDevice";
        public const string KEY_REFRESH_INTERVAL = "RefreshInterval";
        public const string KEY_JENKINS_VIEW = "JenkinsView";
        public const string KEY_JENKINS_TASK_AUDIO_TEXT = "JenkinsTaskAudioText";
        public const string KEY_JENKINS_TASK_STATUS_AUDIO_TEXT = "JenkinsTaskStatusAudioText";

        public const string KEY_MARK = "@@";
        public const string VALUE_END_MARK = "##";


        public int refreshInterval = 30;
        public string outputDevice = "";
        public List<string> jenkinsViewUrls = new List<string>();
        public Dictionary<string, string> taskAudioTextDict = new Dictionary<string, string>();
        public Dictionary<string, string> taskStatusAudioTextDict = new Dictionary<string, string>();
        public string configText;

        public static ConfigManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigManager();
                }

                return instance;
            }
        }

        public void Init()
        {
            var file = GetConfigFilePathName();
            if(File.Exists(file))
            {
                configText = File.ReadAllText(file);
            }
            else
            {
                configText = "";
            }

            List<string> valueLines;
            var searchIndex = 0;
            //默认设备列表
            valueLines = GetConfigValues(KEY_OUTPUT_DEV, configText, ref searchIndex);
            if (valueLines != null && valueLines.Count > 0)
            {
                outputDevice= valueLines[0];
            }
            Console.WriteLine("outputDevice: " + outputDevice);

            //刷新间隔
            valueLines = GetConfigValues(KEY_REFRESH_INTERVAL, configText, ref searchIndex);
            if(valueLines != null && valueLines.Count > 0)
            {
                int.TryParse(valueLines[0], out refreshInterval);
            }
            Console.WriteLine("refreshInterval: " + refreshInterval);

            //JekinsView分组url
            valueLines = GetConfigValues(KEY_JENKINS_VIEW, configText, ref searchIndex);
            if(valueLines != null && valueLines.Count > 0)
            {
                jenkinsViewUrls = valueLines;
            }
            Console.WriteLine("JenkinsView: ");
            foreach(var VARIABLE in jenkinsViewUrls)
            {
                Console.WriteLine(VARIABLE);
            }

            //Jekins任务播报文本
            valueLines = GetConfigValues(KEY_JENKINS_TASK_AUDIO_TEXT, configText, ref searchIndex);
            if (valueLines != null)
            {
                foreach (var line in valueLines)
                {
                    var ar = line.Split('&');
                    if (ar.Length == 2)
                    {
                        taskAudioTextDict.Add(ar[0], ar[1]);
                    }
                    else
                    {
                        Logger.Log("JenkinsTaskAudioText 格式不正确: " + line);
                    }
                }
            }

            Console.WriteLine("JenkinsTaskAudioText: ");
            foreach(var VARIABLE in taskAudioTextDict)
            {
                Console.WriteLine(VARIABLE.Key + "-->" + VARIABLE.Value);
            }

            //Jekins任务状态文本
            valueLines = GetConfigValues(KEY_JENKINS_TASK_STATUS_AUDIO_TEXT, configText, ref searchIndex);
            if(valueLines != null)
            {
                foreach(var line in valueLines)
                {
                    var ar = line.Split('&');
                    if(ar.Length == 2)
                    {
                        taskStatusAudioTextDict.Add(ar[0], ar[1]);
                    }
                    else
                    {
                        Logger.Log("JenkinsTaskStatusAudioText 格式不正确: " + line);
                    }
                }
            }
            Console.WriteLine("JenkinsTaskAudioText: ");
            foreach(var VARIABLE in taskStatusAudioTextDict)
            {
                Console.WriteLine(VARIABLE.Key + "-->" + VARIABLE.Value);
            }
        }

        public void SetOutputDev(string val)
        {
            var startIndex = configText.IndexOf(KEY_MARK + KEY_OUTPUT_DEV);
            var endIndex = configText.IndexOf(VALUE_END_MARK, startIndex);
            if (startIndex >= 0 && endIndex >= 0)
            {
                startIndex = startIndex + KEY_MARK.Length + KEY_OUTPUT_DEV.Length;
                Console.WriteLine("SetOutputDev remove " + configText.Substring(startIndex, endIndex - startIndex));
                configText = configText.Remove(startIndex, endIndex - startIndex);
                configText = configText.Insert(startIndex, "\n" + val + "\n");
            }
        }

        public void Save()
        {
            File.WriteAllText(GetConfigFilePathName(), configText);
        }

        /// <summary>
        /// 获取一个key的值。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start">从第几个字符开始搜索。<=0都是从0开始搜索</param>
        /// <returns>行列表</returns>
        private List<string> GetConfigValues(string key, string text, ref int start)
        {
            if (start < 0)
            {
                start = 0;
            }

            key = KEY_MARK + key;
            var startIndex = text.IndexOf(key);
            if (startIndex > 0)
            {
                startIndex = startIndex + key.Length;
                var endIndex = text.IndexOf(VALUE_END_MARK, startIndex);
                if (endIndex < 0)
                {
                    endIndex = text.Length;
                }

                start = endIndex;

                var valueText = text.Substring(startIndex, endIndex - startIndex);
                return GetTextLines(valueText);
            }

            return null;
        }

        private List<string> GetTextLines(string text)
        {
            List<string> lines = new List<string>();
            string s = "";
            for(var i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if(c == '\r' || c == '\t')
                {
                    continue;
                }

                if(c == '\n' || i == text.Length - 1)
                {
                    if(!s.StartsWith("//") && s != "")
                    {
                        lines.Add(s);
                    }
                    s = "";
                }
                else
                {
                    s = s + c;
                }
            }

            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }

            return lines;
        }


        private string GetConfigFilePathName()
        {
            return System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\config.txt";
        }
    }
}
