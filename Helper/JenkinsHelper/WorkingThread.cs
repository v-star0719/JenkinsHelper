
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SpeechLib;

namespace JenkinsHelp
{
    enum JobStatus
    {
        None,
        Building,
        Success,
        Failed,
        Unstable,
        Aborted,
        Others,
    }

    class JobInfo
    {
        public string name;
        public string readableName;
        public JobStatus status;

        public JobInfo(string nm)
        {
            name = nm;
            readableName = nm.Replace("-", " ");
        }

        //<tr id = "job_Odyssey-OnlineTest-Android" .. </tr>
        public void Update(string html)
        {
            var flag = "alt=\"";
            var start = html.IndexOf(flag) + flag.Length;
            var end = html.IndexOf("\"", start + 1);
            var tex = html.Substring(start, end - start);
            if (tex == "Success")
            {
                ChangeState(JobStatus.Success);
            }
            else if (tex == "Failed")
            {
                ChangeState(JobStatus.Failed);
            }
            else if (tex == "Unstable")
            {
                ChangeState(JobStatus.Unstable);
            }
            else if (tex == "In progress")
            {
                ChangeState(JobStatus.Building);
            }
            else if (tex == "Aborted")
            {
                ChangeState(JobStatus.Aborted);
            }
            else
            {
                ChangeState(JobStatus.Others);
            }
        }

        void ChangeState(JobStatus sta)
        {
            if (sta != status)
            {
                Logger.Log(name + " " + status.ToString() + " => " + sta);
            }

            if (status == JobStatus.Building && sta != JobStatus.Building)
            {
                var speech = GetSpeechText(name);
                var stText = GetStateText(sta);
                WorkingThread.BroadcastMsg(speech + " " + stText);
            }

            status = sta;
            //Console.WriteLine(name + "   " + status.ToString());
        }

        public static string GetName(string html)
        {
            var start = html.IndexOf("\"") + 1;
            var end = html.IndexOf("\"", start + 1);
            //读出来是job_开头的，把前缀去掉
            var name = html.Substring(start, end - start);
            start = name.IndexOf("_") + 1;
            return name.Substring(start);
        }

        public static string GetSpeechText(string jobName)
        {
            string rt;
            if (WorkingThread.TaskAudioTextDict.TryGetValue(jobName, out rt))
            {
                return rt;
            }

            return jobName;
        }

        public static string GetStateText(JobStatus st)
        {
            string rt;
            if(WorkingThread.TaskStatusAudioTextDict.TryGetValue(st.ToString(), out rt))
            {
                return rt;
            }
            return rt.ToString();
        }
    }

    class WorkingThread
    {
        static List<JobInfo> jobs = new List<JobInfo>();
        public static int volume = 100;
        public static bool exist = false;
        public static int refreshInterval = 30;
        private static string outputDevName = ""; //输出设备
        private static int branch = 0;
        private static Dictionary<string, string> taskAudioTextDict = new Dictionary<string, string>();
        private static Dictionary<string, string> taskStatusAudioTextDict = new Dictionary<string, string>();
        private static List<string> jenkinsViewUrls = new List<string>();

        public static DateTime NextRefreshTime { get; private  set; }

        public static Dictionary<string, string> TaskAudioTextDict
        {
            get => taskAudioTextDict;
            set
            {
                taskAudioTextDict.Clear();
                foreach (var kv in value)
                {
                    taskAudioTextDict.Add(kv.Key, kv.Value);
                }
            }
        }

        public static Dictionary<string, string> TaskStatusAudioTextDict
        {
            get => taskStatusAudioTextDict;
            set
            {
                taskStatusAudioTextDict.Clear();
                foreach(var kv in value)
                {
                    taskStatusAudioTextDict.Add(kv.Key, kv.Value);
                }
            }
        }

        public static List<string> JenkinsViewUrls
        {
            get => jenkinsViewUrls;
            set
            {
                jenkinsViewUrls.Clear();
                jenkinsViewUrls.AddRange(value);
            }
        }

        public static string OutputDevName
        {
            get => outputDevName;
            set
            {
                outputDevName = value;
                Logger.Log("switch to output dev:" + value);
            }
        }

        public static int Branch
        {
            get => branch;
            set
            {
                branch = value;
                jobs.Clear();
                Logger.Log("switch to branch: " + value);
            }
        }

        public static void MainThread()
        {
            //BroadcastMsg("Hello Baby~");
            Thread.Sleep(1000);
            NextRefreshTime = DateTime.Now.AddSeconds(1);
            while(true)
            {
                if (exist)
                {
                    break;
                }

                string url = null;
                if(Branch < jenkinsViewUrls.Count)
                {
                    url = jenkinsViewUrls[Branch];
                }

                if (string.IsNullOrEmpty(url))
                {
                    Logger.Log("url index = " + Branch + " not exist");
                }
                else
                {
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    request.Method = "Get";
                    try
                    {
                        HttpWebResponse rp = request.GetResponse() as HttpWebResponse;
                        StreamReader StreamReader = new StreamReader(rp.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                        UpdateJobs(StreamReader.ReadToEnd());
                    }
                    catch (Exception e)
                    {
                        //网络异常
                        Logger.Log(e.Message);
                    }
                }

                NextRefreshTime = DateTime.Now.AddSeconds(refreshInterval);
                Thread.Sleep(refreshInterval * 1000);
            }
        }

        //todo 完整分析html的元素
        static void UpdateJobs(string html)
        {
            var flag = "<tr id=\"job_";
            var index = 0;
            List<string> jobHtmls = new List<string>();
            while (index >= 0)
            {
                index = html.IndexOf(flag, index);
                if (index >= 0)
                {
                    var end = html.IndexOf(flag, index + flag.Length);
                    if (end <= 0)
                    {
                        end = html.IndexOf("</tbody>", index);
                    }

                    var jobHtml = html.Substring(index, end - index);
                    var name = JobInfo.GetName(jobHtml);
                    var job = GetJobInfo(name);
                    job.Update(jobHtml);

                    //Console.WriteLine("==============================================");
                    index = end; //下次从end的位置开始搜索
                }
            }
        }

        static JobInfo GetJobInfo(string jobName)
        {
            foreach (var info in jobs)
            {
                if (info.name == jobName)
                {
                    return info;
                }
            }

            var info2 = new JobInfo(jobName);
            info2.name = jobName;
            jobs.Add(info2);
            Logger.Log("add job " + info2.name);
            return info2;
        }

        public static void BroadcastMsg(string text)
        {
            Logger.Log("BroadcastMsg: " + text);
            try
            {
                SpVoice voice = new SpVoice();
                voice.Volume = volume;
                voice.Rate = -1;
                if(FindDevice(voice, OutputDevName) == null)
                {
                    FindDevice(voice, null);
                }
                voice.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
            catch(Exception error)
            {
                MessageBox.Show("Speak error", "JenkinsHelper", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static SpObjectToken FindDevice(SpVoice voice, string target)
        {
            //for遍历
            //var outputs = voice.GetAudioOutputs();
            //for (int i = 0; i < outputs.Count; i++)
            //{
            //    SpObjectToken o = outputs.Item(i);
            //    string desc = o.GetDescription();
            //    if(target == null || desc.Contains(target))
            //    {
            //        voice.AudioOutput = o;
            //        return o;
            //    }
            //}

            //foreach遍历
            foreach(object speechObjectToken in voice.GetAudioOutputs())
            {
                var o = speechObjectToken as SpeechLib.SpObjectToken;
                string desc = o.GetDescription();
                if(target == null || desc.Contains(target))
                {
                    voice.AudioOutput = o;
                    return o;
                }
            }

            return null;
        }
    }
}

