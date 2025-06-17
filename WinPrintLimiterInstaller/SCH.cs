using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinPrintLimiterInstaller
{
    internal class SCH
    {
        public  void CreateTaskFolder(string folderName)
        {
            string psScript = $@"
$folderPath = '\{folderName}'
$sched = New-Object -ComObject 'Schedule.Service'
$sched.Connect()
$root = $sched.GetFolder('\')
$root.CreateFolder($folderPath)
";

            ProcessStartInfo psi = new ProcessStartInfo
    {
        FileName = "powershell.exe",
        Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{psScript}\"",
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        CreateNoWindow = true
    };

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Debug.WriteLine("Output:\n" + output);
                if (!string.IsNullOrWhiteSpace(error))
                    Debug.WriteLine("Error:\n" + error);
            }
        }


        internal string CreateWatchdogTask(string path)
        {
            string command = $"schtasks /create /tn \"WinPrintLimiter\\Watchdog\" /tr \"\\\"{path}\\\"\" /sc onlogon /RU SYSTEM /rl HIGHEST /F";
            return SchedulerProccess(command);
        }

        internal string CreateLimiterTask(string path)
        {
            string command = $"schtasks /create /tn \"WinPrintLimiter\\test\" /tr \"notepad.exe\" /sc onlogon /IT";
            return SchedulerProccess(command);
        }


        internal string SchedulerProccess(string command)
        {
            using (Process sch = new Process())
            {
                sch.StartInfo.FileName = "cmd";
                sch.StartInfo.Arguments = $"/C {command}";
                sch.StartInfo.RedirectStandardError = true;
                sch.StartInfo.RedirectStandardOutput = true;
                sch.StartInfo.CreateNoWindow = true;
                sch.Start();
                sch.WaitForExit();

                return sch.StandardOutput.ReadToEnd() + " " +sch.StandardError.ReadToEnd();

            }
        }


    }
}
