using PrintLimiterApi.Configuration;
using WinPrintLimiterApi.Users;
using static PrintLimiterApi.Program;
namespace PrintLimiterApi.Users
{
    public class RemoteUserContext
    {
        public string UserName;


        public int PrintCount;
        public int PrintLimit;

        internal SharedInt CurrentPagesCount;
        internal SharedInt MaxPages;

        List<PrinterContext> Printers = new List<PrinterContext>();


        public RemoteUserContext(string username)
        {
            CurrentPagesCount.value = 0;
            this.MaxPages.value = Program.GlobalMaxPagesConfig;

            this.UserName = username;

            



        
        }
    }
}
