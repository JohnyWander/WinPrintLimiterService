namespace PrintLimiterApi.Configuration
{
    internal struct PrinterConfig
    {
        internal string PrintServer;
        internal string PrinterName;
        internal string FriendlyName;
        
        internal int DailyPagesLimit;
        

        internal bool InheritsFromGlobalLimit;
        internal PrinterConfig(string PrintServer, string PrinterName,string FriendlyName, int DailyPagesLimit, bool InheritsFromGlobalLimit = false)
        {
            this.PrintServer = PrintServer;
            this.PrinterName = PrinterName;
            this.FriendlyName = FriendlyName;
            this.DailyPagesLimit = DailyPagesLimit;
            this.InheritsFromGlobalLimit = false;
        }

    }
}
