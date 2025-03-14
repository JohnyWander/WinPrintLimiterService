using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Printing;
using System.Drawing.Printing;

namespace WinPrintLimiter.PrintControl
{
    internal abstract class PrintControlBase
    {
        private protected bool _ISGlobal;

        private protected string PrinterName;
        private protected string PrintServer;


        private int PagesLimit
        {
            get
            {
                if (_ISGlobal == true)
                {
                    return GlobalLimit.value;
                }
                else
                {
                    return PerPrinterLimit;
                }
            }
           
        }
        private int CurrentCount
        {
            get
            {
                if(_ISGlobal == true)
                {
                    return CurrentGlobalCount.value;
                }
                else
                {
                    return PerPrinterCount;
                }
            }


            set
            {
                if (_ISGlobal == true)
                {
                    CurrentGlobalCount.value = value;
                }
              
            }

        }

        private int PerPrinterCount;
        private protected int PerPrinterLimit;

        private protected SharedInt CurrentGlobalCount;
        private protected SharedInt GlobalLimit;

        


        internal protected PrintControlBase(SharedInt currentGlobalCount,SharedInt globalLimit)
        {
            this.CurrentGlobalCount = currentGlobalCount;
            this.GlobalLimit = globalLimit;
            _ISGlobal = true;
            
        }

        internal protected PrintControlBase(int correntOwnCount,int ownLimit)
        {
            this.PerPrinterLimit = ownLimit;
            this.PerPrinterCount = correntOwnCount;
            _ISGlobal = false;
        }

        

        internal void ListInstalledPrinters() 
        { 
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                MessageBox.Show(printer);
            }
        }

        internal void LocalPrinterMonitor()
        {
            LocalPrintServer printServerLocal = new LocalPrintServer();
            PrintQueue queue = printServerLocal.GetPrintQueue(this.PrinterName);
            PrintingMonitor(queue);
            
            
        }

        internal void RemotePrinterMonitor()

        {
            PrintServer printServer = new PrintServer(this.PrintServer);
            PrintQueue queue = printServer.GetPrintQueue(this.PrinterName);

            PrintingMonitor(queue);
            
            
        }

    
        private void PrintingMonitor(PrintQueue printQueue)
        {
           

            Console.WriteLine("Printing monitor starting");
                while (true)
                {
                    printQueue.Refresh(); // Refresh the queue to get updated jobs
                    PrintJobInfoCollection jobs = printQueue.GetPrintJobInfoCollection();
                    Console.WriteLine($"{CurrentCount}/{PagesLimit}");
                    foreach (PrintSystemJobInfo job in jobs)
                    {
                        if (job.NumberOfPages >= this.PagesLimit)
                        {
                            Console.WriteLine($"Job '{job.Name}' by {job.Submitter} exceeds page limit and will be canceled.");
                            Task.Run(() =>
                            {
                                MessageBox.Show($"Przekroczono limit stron ({PagesLimit}). Pages printed count exceeded ({PagesLimit})");
                            });
                            job.Cancel();  // Cancel the job
                        }
                        else if (CurrentCount <= PagesLimit)
                        {
                            CurrentCount += job.NumberOfPages;
                            continue;
                        }
                        else if (CurrentCount > PagesLimit)
                        {
                            Console.WriteLine($"Przekroczono dzienny limit drukowania ({PagesLimit}) anulowano drukowanie,Daily Limit exceeded ({PagesLimit}) print job cancelled");
                        }
                    }

                    Thread.Sleep(1000);  // Check every 5 seconds
                } 
    }
    }
}
