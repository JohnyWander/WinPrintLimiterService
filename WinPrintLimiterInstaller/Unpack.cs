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



    }
}
