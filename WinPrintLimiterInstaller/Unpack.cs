using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace WinPrintLimiterInstaller
{ 
    internal class Unpack
    {
        Assembly ThisAssembly;
        internal Unpack() 
        {
            this.ThisAssembly = Assembly.GetExecutingAssembly();
        }

        internal void UnpackWinPrintLimiter(string path)
        {
           string resName =  ThisAssembly.GetManifestResourceNames().Single(str => str.EndsWith("WinPrintLimiter.exe"));
           Debug.WriteLine(resName);

           Stream dataStream = ThisAssembly.GetManifestResourceStream(resName);
           byte[] data = new byte[dataStream.Length];
            dataStream.Read(data);

            File.WriteAllBytes(path+"\\"+"WinPrintLimiter.exe", data);          
        }

        internal void UnpackWatchDog(string path)
        {
            string resName = ThisAssembly.GetManifestResourceNames().Single(str => str.EndsWith("WinPrintLimiterWatchdog.exe"));
            Debug.WriteLine(resName);

            Stream dataStream = ThisAssembly.GetManifestResourceStream(resName);
            byte[] data = new byte[dataStream.Length];
            dataStream.Read(data);

            File.WriteAllBytes(path+"\\"+"WinPrintLimiterWatchdog.exe", data);
        }

        internal void UnpackWatchDogDLL(string path)
        {
            string resName = ThisAssembly.GetManifestResourceNames().Single(str => str.EndsWith("WinPrintLimiterWatchdog.dll"));
            Debug.WriteLine(resName);

            Stream dataStream = ThisAssembly.GetManifestResourceStream(resName);
            byte[] data = new byte[dataStream.Length];
            dataStream.Read(data);

            File.WriteAllBytes(path + "\\" + "WinPrintLimiterWatchdog.dll", data);
        }
        

        internal void CreateMiscFiles(string path)
        {
            Directory.CreateDirectory(path + "\\" + "Contexts");
            if (!File.Exists("printers.conf"))
            {
                File.WriteAllText(path+"\\printers.conf","#Below configuration allows you to specify limit for any printer installed\r\n#<Printer>\r\n#PrintServer=print.local.com # or 'local' for local installed printer\r\n#PrinterName=example.hp2050-0323\r\n#DailyPagesLimit=global # global inherits limit from global configuration, 0 or -1 or any integer > 0 - 0 disables printing, -1 stands for unlimited\r\n#</Printer>\r\n");
            }
            else
            {
                File.Copy("printers.conf", path + "\\printers.conf");
            }
        }


    }
}
