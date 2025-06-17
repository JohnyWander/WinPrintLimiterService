using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace WinPrintLimiterInstaller
{
    internal class SC
    {
         string scCreate = "create WinPrintLimiter4 binPath= \"$path$\" start= auto type= own type= interact";
         string scSecure = @"sdset WinPrintLimiter4 D:(A;;CCLCRPWPDTLOSDRCWDWO;;;BA)(A;;CCLCRPWPDTLO;;;SY)(A;;CCLCRPWPDTLO;;;LS)";
         string scPerms = "config WinPrintLimiter5 obj= \"LocalService\"";
         string scQuery = "query WinPrintLimiter3";
         string scDelete = "delete WinPrintLimiter3";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Arguments"></param>
        /// <returns></returns>
        internal string ScProcess(string Arguments)
        {
            using (Process scProcess = new Process())
            {
                scProcess.StartInfo.FileName = "sc";
                scProcess.StartInfo.Arguments = Arguments;
                scProcess.StartInfo.CreateNoWindow = true;
                scProcess.StartInfo.RedirectStandardError = true;
                scProcess.StartInfo.RedirectStandardOutput = true;
                //scProcess.StartInfo.Verb = "runas";
                scProcess.Start();
                
                scProcess.WaitForExit();
                Debug.WriteLine(scProcess.StandardOutput.ReadToEnd());
                return scProcess.StandardOutput.ReadToEnd();
            }
        }

         internal string CreateService(string path)
         {
            return ScProcess(scCreate.Replace("$path$", path));       
         }

        internal string SecureService()
        {
            return ScProcess(scSecure);
        }

        internal string PermissionsForService()
        {
            return ScProcess(scPerms);
        }

        internal string DeleteService()
        {
            return ScProcess(scDelete);
        }

        internal string DeleteServiceIfExists()
        {
            string output = ScProcess(scQuery);
            if (output.Contains("SERVICE_NAME: WinPrintLimiter"))
            {
                return DeleteService();
            }
            else
            {

                return "No service";
            }
        }

    }
}
