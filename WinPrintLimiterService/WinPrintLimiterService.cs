using System.ServiceProcess;

namespace WinPrintLimiterService
{
    public partial class WinPrintLimiterService : ServiceBase
    {
        string pName = "WinPrintLimiter";


        public WinPrintLimiterService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
           
        }

        protected override void OnStop()
        {
        }

          





    }
}
