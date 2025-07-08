using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;
using System.Drawing.Printing;
using System.Net;

namespace WinPrintLimiterInstaller.install
{
    internal class InstallProccess
    {
        internal InstallFormData formData;

        Unpack unpack;
        SCH sch;


        public void InstallWDK()
        {
            string wdkUrl = "https://go.microsoft.com/fwlink/?LinkId=2128854"; // WDK for Windows 11 (adjust as needed)
            string installerPath = "wdksetup.exe";

            try
            {
                Console.WriteLine("Downloading WDK...");
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(wdkUrl, installerPath);
                }

                Console.WriteLine("Starting WDK installer...");
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = installerPath,
                    Arguments = "/quiet /norestart",
                    UseShellExecute = true,
                    Verb = "runas" // run as admin
                };

                Process process = Process.Start(psi);
                process.WaitForExit();

                Console.WriteLine($"WDK installer exited with code {process.ExitCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Installation failed: " + ex.Message);
            }
        }
    


        public void InstallXpsV3UsingPowerShell()
        {
            try
            {
                ProcessStartInfo ps = new ProcessStartInfo();
                ps.FileName = "dism.exe";
                ps.Arguments = "/online /enable-feature /featurename:Printing-XPSServices-Features /all";
                ps.Verb = "runas"; // Run as administrator
                ps.UseShellExecute = true;
                ps.RedirectStandardOutput = false;

                Process process = Process.Start(ps);
                process.WaitForExit();

                Console.WriteLine($"DISM exited with code: {process.ExitCode}");
                if (process.ExitCode == 0)
                {
                    Console.WriteLine("Feature enabled successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to enable feature.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error running DISM: " + ex.Message);
            }



            string psScript = @"
$driver = Get-PrinterDriver -Name 'Microsoft XPS Document Writer v3'
if (-not $driver) {
    Add-PrinterDriver -Name 'Microsoft XPS Document Writer v3'
}

";

            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{psScript}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                Verb = "runas" // optional: prompts for admin
            };

            using (var process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Debug.WriteLine("Output: " + output);
                Debug.WriteLine("Error: " + error);
            }
        }
        public void CreatePort(string portName)
        {
            string psCommand = $"Add-PrinterPort -Name \"{portName}\"";
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"-Command \"{psCommand}\"",
                    Verb = "runas", // Run as administrator
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0) { }
          //      throw new Exception($"Error creating port: {error}");
        
        }

        public void InstallVirtualXPS(string printerName)
        {

            string xpsFilePath = @"C:\ProgramData\WPLXPS\job.xps";
            string driverName = "Microsoft XPS Document Writer v4";

            Directory.CreateDirectory(@"C:\ProgramData\WPLXPS\");

            CreatePort(xpsFilePath);



            // Use rundll32 to install printer with a local port (the XPS output path)
            string arguments = $@"printui.dll,PrintUIEntry /if /b ""{printerName}"" /f """" /r ""{xpsFilePath}"" /m ""{driverName}"" /z /q";

            var psi = new ProcessStartInfo
            {
                FileName = "rundll32.exe",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Verb = "runas", // Admin privileges required
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                if (string.IsNullOrWhiteSpace(error))
                    Console.WriteLine("Printer installed successfully.");
                else
                    Console.WriteLine("Error: " + error);
            }
        }


        public InstallProccess(InstallFormData data)
        {
            formData = data;

            if (formData.UseXPS)
            {
                this.InstallWDK();
                //this.InstallXpsV3UsingPowerShell();
                this.InstallVirtualXPS(formData.XPSprinterName);
            }


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
            unpack.UnpackWatchDogDLL($"{formData.InstallPath}\\WinPrintLimiter\\Watchdog");
            unpack.CreateMiscFiles($"{formData.InstallPath}\\WinPrintLimiter\\Limiter");

            if (File.Exists("WinPrintLimiter.conf"))
            { 
                File.Copy("WinPrintLimiter.conf", $"{formData.InstallPath}\\WinPrintLimiter\\Limiter\\WinPrintLimiter.conf",true);
            }

            sch.CreateTaskFolder("WinPrintLimiter");
            string result = sch.CreateWatchdogTask($"{formData.InstallPath}\\WinPrintLimiter\\Watchdog\\WinPrintLimiterWatchdog.exe");
            Console.WriteLine(result);Debug.WriteLine(result);
            Task.Run(() =>
            {
                MessageBox.Show(result);
            });


            string AutorunCommand = $"cd /d \"{formData.InstallPath}\\WinPrintLimiter\\Limiter\"\n" +
                $"start \"\" \"{formData.InstallPath}\\WinPrintLimiter\\Limiter\\WinPrintLimiter.exe\"";
            File.WriteAllText("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\runlimiter.bat", AutorunCommand);
           
        }


    }
}
