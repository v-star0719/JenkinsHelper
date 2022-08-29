using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JenkinsHelp
{
    class Logger
    {
        public static Action<string> outputLogCallback;
        private static StringBuilder logs = new StringBuilder("");

        public static void Init(Action<string> callback)
        {
            outputLogCallback = callback;
        }

        public static void Log(string str)
        {
            var text = System.DateTime.Now + ": " + str;
            Console.WriteLine(text);
            logs.AppendLine(text);
            outputLogCallback(logs.ToString());
        }
    }
}
