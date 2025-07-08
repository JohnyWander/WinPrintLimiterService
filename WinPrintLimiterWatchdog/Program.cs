using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinPrintLimiterWatchdog
{
    internal static class Program
    {
        [DllImport("kernel32")]
        static extern int AllocConsole();

        static SCH sch = new SCH();
        static PrintControl PC = new PrintControl(); 
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        
        static void Main()
        {
            try
            {
                //AllocConsole();

                bool FoundLimiterProcess = false;

                Exception findException = null;
                FoundLimiterProcess = PC.FindLimiter(out findException,5,10000);

                
                if (!FoundLimiterProcess)
                {
                    log("disabling spooler...");
                    while (true)
                    {
                        PC.DisableSpooler();
                        
                        Thread.Sleep(10000);
                    }
                }
               
              

                Security security = new Security();
                security.OwnSec();
                security.LimiterSec();
                
                while (true)
                {
                    FoundLimiterProcess = PC.FindLimiter(out findException,1,5000);
                    if (!FoundLimiterProcess)
                    {
                        log("disabling spooler...");

                        while (true)
                        {
                            PC.DisableSpooler();
                            
                            Thread.Sleep(10000);
                        }
                    }
                }
            }
            catch(Exception e)
            {               
                    Console.WriteLine(e.ToString());
                    log(e.Message);               
            }

        }

        static void log(string message)
        {
            File.AppendAllText("log.txt", message);
        }
    }
}