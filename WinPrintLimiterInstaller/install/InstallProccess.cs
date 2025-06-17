using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WinPrintLimiterInstaller.install
{
    internal class InstallProccess
    {
        internal InstallFormData formData;

        Unpack unpack;
        SCH sch;
        public InstallProccess(InstallFormData data)
        {
            formData = data;
            unpack = new Unpack();
            sch = new SCH();
        }

        public void Start()
        {
            if (Directory.Exists($"{formData.InstallPath}\\WinPrintLimiter"))
            {
                Directory.CreateDirectory($"{formData.InstallPath}\\WinPrintLimiter");
            }

            Directory.CreateDirectory($"{formData.InstallPath}\\WinPrintLimiter\\Watchdog");
            Directory.CreateDirectory($"{formData.InstallPath}\\WinPrintLimiter\\Limiter");

            unpack.UnpackWatchDog($"{formData.InstallPath}\\WinPrintLimiter\\Watchdog");
            unpack.UnpackWinPrintLimiter($"{formData.InstallPath}\\WinPrintLimiter\\Limiter");

            sch.CreateTaskFolder("WinPrintLimiter");
            Debug.WriteLine(sch.CreateWatchdogTask($"{formData.InstallPath}\\WinPrintLimiter\\Watchdog\\WinPrintLimiterWatchdog.exe"));


            string AutorunCommand = $"\"{formData.InstallPath}\\WinPrintLimiter\\Limiter\\WinPrintLimiter.exe\"\npause";
            File.WriteAllText("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\runlimiter.bat", AutorunCommand);
           
        }


    }
}
