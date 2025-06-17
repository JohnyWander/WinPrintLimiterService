using System.Drawing.Printing;
using System.Printing;

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
                if (_ISGlobal == true)
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


        internal Action<int> PageCountIncrement;


        internal protected PrintControlBase(SharedInt currentGlobalCount, SharedInt globalLimit)
        {
            this.CurrentGlobalCount = currentGlobalCount;
            this.GlobalLimit = globalLimit;
            _ISGlobal = true;

        }

        internal protected PrintControlBase(int correntOwnCount, int ownLimit)
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
            List<PrintSystemJobInfo> printJob = new List<PrintSystemJobInfo>();


            Console.WriteLine("Printing monitor starting");

            while (true)
            {
                printQueue.Refresh(); // Refresh the queue to get updated jobs
                PrintJobInfoCollection jobs = printQueue.GetPrintJobInfoCollection();
                //Console.WriteLine($"{CurrentCount}/{PagesLimit}");


                if (jobs.Count() != 0)
                {

                    bool JobWasCancelled = false;
                    PrintSystemJobInfo job = jobs.First();

                    if (job.Submitter == Environment.UserName)
                    {



                        bool Finished = false;
                        if (CurrentCount >= PagesLimit)
                        {
                            job.Cancel();
                            Console.WriteLine($"Job was cancelled because limit is reached - {CurrentCount}/{PagesLimit}");
                            Thread.Sleep(4000);
                            JobWasCancelled = true;
                            Finished = true;
                        }


                        while (Finished == false)
                        {
                            job.Refresh();
                            if (job.NumberOfPages > PagesLimit || CurrentCount + job.NumberOfPages > PagesLimit)
                            {
                                Console.WriteLine($"Job was cancelled because limit would be exceeded (Current count - {CurrentCount} + pages of the job - {job.NumberOfPages}) is higher than limit - {PagesLimit} ");

                                job.Cancel();
                                JobWasCancelled = true;
                            }

                            Console.WriteLine("N:" + job.NumberOfPages);
                            printQueue.Refresh();
                            jobs = printQueue.GetPrintJobInfoCollection();
                            if (jobs.Count() == 0)
                            {
                                Thread.Sleep(4000);
                                break;
                            }



                            Thread.Sleep(100);


                        }

                        Console.WriteLine("Finished, final count is: " + job.NumberOfPages);
                        if (JobWasCancelled == false)
                        {
                            CurrentCount += job.NumberOfPages;
                            PageCountIncrement.Invoke(job.NumberOfPages);
                        }
                    }
                }



                Thread.Sleep(100);  // Check every 5 seconds
            }
        }
    }
}
