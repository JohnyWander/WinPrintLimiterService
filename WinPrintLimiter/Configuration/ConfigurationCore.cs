using System.Text;
using WinPrintLimiter.Configuration.ConfigConstraints;

namespace WinPrintLimiter.Configuration
{
    internal abstract class ConfigurationCore
    {
        protected IDictionary<string, ConfigRecord> ConfigData = new Dictionary<string, ConfigRecord>()
        {
            { "mode",
            new ConfigRecord("mode","local","Working mode - local keeps all data locally and limits printing locally, online (limit domain users on multiple pcs, requires server installation",new ModeConstraint()).Enable() },
            {"host",new ConfigRecord("host","","host address work working mode",null).Disable()},
            {"maxpages",new ConfigRecord("maxpages","-1","Max pages user can print(global option, can be overriden by printer specific congfig,\n# (local mode only) for online mode adjust server settings ").Enable()},


        };

        internal List<ConfigRecord> ParsedConfig = new List<ConfigRecord>();

        internal List<PrinterConfig> ParsedPrinterConfig = new List<PrinterConfig>();

        protected string PrinterConfigurationExample =
            "#Below configuration allows you to specify limit for any printer installed\n" +
            "#<Printer>\n" +
            "#PrintServer=print.local.com # or 'local' for local installed printer\n" +
            "#PrinterName=example.hp2050-0323\n" +
            "#DailyPagesLimit=global # global inherits limit from global configuration, 0 or -1 or any integer > 0 - 0 disables printing, -1 stands for unlimited\n" +

            "#</Printer>\n";


        protected string DefaultConfigurationToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var record in ConfigData)
            {
                sb.AppendLine(record.Value.Dump());
                sb.AppendLine();
            }
            return sb.ToString();
        }





    }
}
