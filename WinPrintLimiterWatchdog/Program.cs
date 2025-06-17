using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinPrintLimiterWatchdog
{
    internal static class Program
    {
        [DllImport("kernel32")]
        static extern int AllocConsole();

        static SCH sch = new SCH();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                AllocConsole();

                sch.CreateLimiterTask(null);
                Thread.Sleep(1000);
                sch.StartLimiterTask(null);


                Thread.Sleep(1000);
               
              

                Security security = new Security();
                security.OwnSec();
                //security.LimiterSec();

                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
            catch(Exception e)
            {               
                    Console.WriteLine(e.ToString());
                    EventLog.WriteEntry("WinPrintLimiter5", e.Message, EventLogEntryType.Information);
                while (true)
                {
                    Thread.Sleep(10000);
                }
            }

        }
    }
}