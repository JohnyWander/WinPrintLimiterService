using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace WinPrintLimiterWatchdog
{
    internal class PrintControl
    {
        internal bool FindLimiter(out Exception exception,int tries,int delay)
        {
            bool found = false;
            exception = null;

            for (int i = 0; i < tries;i++)
            {
                Thread.Sleep(delay);
                try
                {
                    Process p = Process.GetProcessesByName("WinPrintLimiter")[0];
                    exception = null;
                    found = true;
                    Console.WriteLine("Found Limiter process");
                    break;
                }
                catch (Exception e)
                {
                    exception = e;
                    found = false;
                    exception = e;
                    Console.WriteLine($"Did notFound Limiter process, on try #{i}");
                    Thread.Sleep(delay);
                    continue;
                }
               
            }

            return found;

        }



        internal void DisableSpooler()
        {
            ServiceController service = new ServiceController("Spooler");

            if (service.Status != ServiceControllerStatus.Stopped &&
                service.Status != ServiceControllerStatus.StopPending)
            {
                Console.WriteLine("Stopping Print Spooler service...");
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                Console.WriteLine("Print Spooler service stopped successfully.");
            }
            else
            {
                Console.WriteLine("Print Spooler service is already stopped or stopping.");
            }
        }


    }
}
