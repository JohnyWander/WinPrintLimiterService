using System.IO;
using WinPrintLimiter.Configuration.Exceptions;

namespace WinPrintLimiter.Configuration
{
    internal class Configuration : ConfigurationCore
    {
        private const string ConfigFilename = "WinPrintLimiter.conf";
        private const string PrinterSetupFilename = "printers.conf";

        public bool InheritsFromGlobalConfLimit = false;

        private int _MaxPages;
        public int MaxPages
        {
            get { return _MaxPages; }
            private set { _MaxPages = value; }
        }

        private int _MaxJobs;
        public int MaxJobs
        {
            get { return _MaxJobs; }
            private set { _MaxJobs = value; }
        }


        public Configuration()
        {
            CheckThenCreate();
            CheckPrintcfgThenCreate();
            Parse();
            ParsePrinterConf();
        }

        private bool EnsureConfigExists()
        {
            return File.Exists(ConfigFilename);
        }

        private bool EnsurePrintConfigExists()
        {
            return File.Exists(PrinterSetupFilename);
        }

        private void CheckThenCreate()
        {
            if (!EnsureConfigExists())
            {
                File.WriteAllText(ConfigFilename, base.DefaultConfigurationToString());
            }
        }

        private void CheckPrintcfgThenCreate()
        {
            if (!EnsurePrintConfigExists())
            {
                File.WriteAllText(PrinterSetupFilename, base.PrinterConfigurationExample);
            }
        }

        private void Parse()
        {
            List<string> configurationLines = File.ReadAllLines(ConfigFilename).Where(line => !line.StartsWith("#") && line != string.Empty).ToList();

            configurationLines.ForEach(line =>
            {
                string[] split = line.Split('=');
                string key = split[0];
                string value = split[1];
                // Console.WriteLine(key + "====" + value);
                ConfigRecord rec = base.ConfigData.Where(c => c.Key == key).Select(c => c.Value).FirstOrDefault();
                rec.Key = key;

                if (rec.Constraint is not null)
                {
                    if (rec.Constraint.ConstraintCheck(value))
                    {
                        rec.Value = value;
                        base.ParsedConfig.Add(rec);
                    }
                    else
                    {
                        rec.Constraint.ThrowError(line);
                    }
                }
                else
                {
                    rec.Value = value;
                    base.ParsedConfig.Add(rec);
                }
            });

            ParsedConfig.ForEach(u =>
            {
                Console.WriteLine(u.Value);
            });

        }

        private void ParsePrinterConf()
        {
            List<string> configurationLines = File.ReadAllLines(PrinterSetupFilename).Where(line => !line.StartsWith("#")).ToList();

            int index = 0;
            int indexEnd = 0;
            while (index != -1)
            {
                index = configurationLines.FindIndex(x => x.Contains("<Printer>"));
                indexEnd = configurationLines.FindIndex(x => x.Contains("</Printer>"));

                if (index > 0 && indexEnd > 0)
                {
                    List<string> lines = configurationLines.GetRange(index, indexEnd - index + 1);
                    lines.ForEach(s =>
                    {
                        configurationLines.Remove(s);
                    });

                    if (lines.First() == "<Printer>" && lines.Last() == "</Printer>")
                    {
                        string printServerLine = lines.Where(x => x.Contains("PrintServer")).FirstOrDefault();
                        string printerNameLine = lines.Where(x => x.Contains("PrinterName")).FirstOrDefault();
                        string dailyLimitLine = lines.Where(x => x.Contains("DailyPagesLimit")).FirstOrDefault();


                        if (printServerLine is null) { throw new ConfigurationException($"Printer configuration at line {index} lacks print server name"); }
                        if (printerNameLine is null) { throw new ConfigurationException($"Printer configuration at line {index} lacks printer name"); }
                        if (dailyLimitLine is null) { throw new ConfigurationException($"Printer configuration at line {index} lacks daily limit setting"); }

                        string printServer = printServerLine.Split("=")[1];
                        string printerName = printerNameLine.Split("=")[1];
                        string dailyLimit = dailyLimitLine.Split("=")[1];

                        if (dailyLimit == "global")
                        {
                            InheritsFromGlobalConfLimit = true;
                            dailyLimit = ParsedConfig.Where(name => name.Key == "maxpages").First().Value;
                        }

                        if (InheritsFromGlobalConfLimit)
                        {
                            base.ParsedPrinterConfig.Add(new PrinterConfig(printServer, printerName, int.Parse(dailyLimit), true));
                        }
                        else
                        {
                            base.ParsedPrinterConfig.Add(new PrinterConfig(printServer, printerName, int.Parse(dailyLimit)));
                        }

                        if (Program.Debug)
                        {
                            base.ParsedPrinterConfig.ForEach(printer =>
                            {
                                Console.WriteLine($"PRINTER|{printer.PrintServer}|{printer.PrinterName}|{printer.DailyPagesLimit}");
                            });
                        }
                    }


                }
            }
        }

    }
}
