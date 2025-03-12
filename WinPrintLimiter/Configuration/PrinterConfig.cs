using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinPrintLimiter.Configuration
{
    internal struct PrinterConfig
    {
        internal string PrintServer;
        internal string PrinterName;
        internal int DailyPagesLimit;

        internal bool InheritsFromGlobalLimit;
        internal PrinterConfig(string PrintServer,string PrinterName,int DailyPagesLimit,bool InheritsFromGlobalLimit = false)
        {
            this.PrintServer = PrintServer;
            this.PrinterName = PrinterName;
            this.DailyPagesLimit = DailyPagesLimit;
            this.InheritsFromGlobalLimit = false;
        }

    }
}
