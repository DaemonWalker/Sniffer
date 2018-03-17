using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sniffer.UI.Utils
{
    class UtilMethods
    {
        public static string ShowPort(string portName)
        {
            Process pro = new Process();
            pro.StartInfo.FileName = "cmd.exe";
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.RedirectStandardInput = true;
            pro.StartInfo.RedirectStandardOutput = true;
            pro.StartInfo.RedirectStandardError = true;
            pro.StartInfo.CreateNoWindow = true;

            pro.Start();

            pro.StandardInput.WriteLine("netstat -ano");
            pro.StandardInput.WriteLine("exit");

            Regex reg = new Regex("\\s+", RegexOptions.Compiled);
            string line = null;
            var procName = string.Empty;
            while ((line = pro.StandardOutput.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.StartsWith("TCP", StringComparison.OrdinalIgnoreCase))
                {
                    line = reg.Replace(line, ",");
                    string[] arr = line.Split(',');
                    if (arr[1].EndsWith($":{portName}"))
                    {
                        Console.WriteLine($"{portName}端口的进程ID：{arr[4]}");
                        int pid = Int32.Parse(arr[4]);
                        var targetProc = Process.GetProcessById(pid);
                        procName = targetProc.ProcessName;
                        break;
                    }
                }
            }
            pro.Close();
            return procName;
        }

        public static Encoding ContentEncoding { get; set; }
        public static string GetContent(byte[] buffer, bool toBase64)
        {
            var content = string.Empty;
            if (toBase64)
            {
                content = Convert.ToBase64String(buffer);
            }
            else
            {
                content = ContentEncodingk.GetString(buffer);
            }
            return content;
        }
    }
}
