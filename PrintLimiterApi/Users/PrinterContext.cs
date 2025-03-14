using WinPrintLimiterApi.Users;

namespace PrintLimiterApi.Users
{
    public class PrinterContext
    {
        public string PrintServer;
        public string PrinterName;


        internal SharedInt PrinterLimit;
        public bool ISGlobal;

    }
}
