using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Windows.Navigation;

namespace WinPrintLimiter.PrintControl
{
    [Serializable]
    internal class LocalUserContext : UserContextBase,ISerializable
    {
        internal SharedInt GlobalJobsCounter = new SharedInt(0);
        internal SharedInt MaxGlobal;

        internal List<PrinterContext> Printers;


        BinaryFormatter formatter = new BinaryFormatter();
        public LocalUserContext() { }

        protected LocalUserContext(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public LocalUserContext(string userename,int maxPages) : base(userename)
        {
            base.ContextDate = DateTime.Now;
            MaxGlobal = new SharedInt(maxPages);
        }

        internal override string GetCurrentUsername()
        {
            return Environment.UserName;
        }

        internal void BindPrintersToContext()
        {
            Printers.ForEach( P =>
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
      

    }
}
