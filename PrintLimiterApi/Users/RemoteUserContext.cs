using WinPrintLimiterApi.Users;
namespace PrintLimiterApi.Users
{
    public class RemoteUserContext
    {
        public string UserName;


        public int PrintCount;
        public int PrintLimit;

        internal SharedInt CurrentPagesCount = new SharedInt(0);
        internal SharedInt MaxPages = new SharedInt(0);

        List<PrinterContext> Printers = new List<PrinterContext>();


        public RemoteUserContext(string username)
        {
            CurrentPagesCount.value = 0;
            this.MaxPages.value = Program.GlobalMaxPagesConfig;

            this.UserName = username;






        }
    }
}
