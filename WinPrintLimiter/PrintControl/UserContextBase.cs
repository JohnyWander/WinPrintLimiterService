using System.Runtime.Serialization;

namespace WinPrintLimiter.PrintControl
{
    [Serializable]
    internal abstract class UserContextBase : ISerializable
    {
        private int _CurrentPagesCount;
        private int _CurrentJobsCount;

        private int _MaxPages;
        private int _MaxJobs;

        internal DateTime ContextDate;

        private string _Username;

        internal List<PrinterContext> Printers;
        public UserContextBase()
        {

        }

        protected UserContextBase(SerializationInfo info, StreamingContext context)
        {
            Username = info.GetString("Username");
            CurrentPagesCount = info.GetInt32("CurrentPagesCount");
            ContextDate = info.GetDateTime("ContextDate");

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", this.Username);
            info.AddValue("CurrentPagesCount", this.CurrentPagesCount);
            info.AddValue("ContextDate", this.ContextDate);
        }


        internal UserContextBase(string username)
        {
            this.Username = username;

        }


        internal void BindPrintersToContext()
        {
            Printers.ForEach(P =>
            {
                Console.WriteLine($"Binding printer{P.PrinterName} from server {P.PrintServer}to user context ");

                if (P.IsGlobalLimit)
                {
                    Console.WriteLine($"Max pages limit is inheritet from global config - MAX amount avaiable: {P.GlobalLimit}");
                }
                else
                {
                    Console.WriteLine($"Max pages limit is set in printer config - MAX amount avaiable: {P.PerPrinterLimit}");
                }

                Thread thread = new Thread(() =>
                {
                    if (P.PrintServer == "local")
                    {
                        P.LocalPrinterMonitor();
                    }
                    else
                    {
                        P.RemotePrinterMonitor();
                    }
                });
                thread.Start();



            });
        }


        internal string Username
        {
            get { return _Username; }
            private set { _Username = value; }
        }


        internal int CurrentPagesCount
        {
            get { return _CurrentJobsCount; }
            private set { _CurrentPagesCount = value; }
        }
        internal int CurrentJobsCount
        {
            get { return _CurrentJobsCount; }
            private set { _CurrentJobsCount = value; }
        }



        internal abstract string GetCurrentUsername();




    }
}
