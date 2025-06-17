namespace WinPrintLimiter.PrintControl
{
    internal sealed class PrinterContext : PrintControlBase
    {
        internal string PrinterName
        {
            get
            {
                return base.PrinterName;
            }
        }
        internal string PrintServer
        {
            get
            {
                return base.PrintServer;
            }
        }

        internal int GlobalLimit
        {
            get
            {
                return base.GlobalLimit.value;
            }
        }

        internal bool IsGlobalLimit
        {
            get
            {
                return base._ISGlobal;
            }
        }

        internal int PerPrinterLimit
        {
            get
            {
                return base.PerPrinterLimit;
            }
        }



        internal PrinterContext(string printServer, string printerName, int CurrentCount, int MaxPages) : base(CurrentCount, MaxPages)
        {
            base.PrintServer = printServer;
            base.PrinterName = printerName;


        }

        internal PrinterContext(string printServer, string printerName, SharedInt GlobalCurrentCount, SharedInt GlobalMax) : base(GlobalCurrentCount, GlobalMax)
        {
            base.PrinterName = printerName;
            base.PrintServer = printServer;

        }
    }
}
