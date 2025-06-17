using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace WinPrintLimiterService
{
    internal static class Program
    {
   
        static void Main()
        {
            //StartProcessAs start = new StartProcessAs();
            //start.Main();

            
            //Thread.Sleep(10000);

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new WinPrintLimiterService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
