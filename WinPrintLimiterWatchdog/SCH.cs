using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinPrintLimiterWatchdog
{
    internal class SCH
    {

        internal string StartLimiterTask(string path)
        {
            string command = $"schtasks /run /tn \"\\WinPrintLimiter\\test\"";
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

                return sch.StandardOutput.ReadToEnd() + " " + sch.StandardError.ReadToEnd();

            }
        }

    }
}
