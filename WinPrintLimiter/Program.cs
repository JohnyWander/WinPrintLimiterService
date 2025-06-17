using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WinPrintLimiter.Configuration;
using WinPrintLimiter.PrintControl;

namespace WinPrintLimiter
{
    internal static class Program
    {
        internal static readonly string ContextsPath = "Contexts";
        internal static bool Debug = false;
        [STAThread]
        static void Main()
        {
            Init();
            Interop.AllocConsole();
            // Step One - Configuration checking, parsing
            Configuration.Configuration configuration = new Configuration.Configuration();


            //Get Mode and decide how user context should be created
            ConfigRecord mode = configuration.ParsedConfig.Where(conf => conf.Key == "mode").FirstOrDefault();
            if (mode is null)
            {
                throw new InvalidOperationException("there is not mode setting set, stopping");
                Console.ReadLine(); Environment.Exit(0);
            }
            else if (mode.Value == "local")
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                string Username = Environment.UserName;

                ConfigRecord maxpagesRecord = configuration.ParsedConfig.Where(conf => conf.Key == "maxpages").FirstOrDefault();
                int MaxPages = int.Parse(maxpagesRecord.Value);




                LocalUserContext UserContext;

                if (File.Exists($"{ContextsPath}\\{Username}.dat"))
                {
                    using (var stream = new FileStream($"{ContextsPath}\\{Username}.dat", FileMode.Open))
                    {
                        UserContext = (LocalUserContext)binaryFormatter.Deserialize(stream);

                        RunContextSaver(UserContext);
                    }
                }
                else
                {
                    UserContext = new LocalUserContext(Environment.UserName, MaxPages);

                    RunContextSaver(UserContext);
                }

                List<PrinterContext> printers = new List<PrinterContext>();
                configuration.ParsedPrinterConfig.ForEach(p =>
                {

                    PrinterContext context;
                    if (p.InheritsFromGlobalLimit)
                    {
                        context = new PrinterContext(p.PrintServer, p.PrinterName, UserContext.GlobalJobsCounter, UserContext.MaxGlobal);
                    }
                    else
                    {
                        context = new PrinterContext(p.PrintServer, p.PrinterName, 0, p.DailyPagesLimit);
                    }

                    printers.Add(context);


                });

                UserContext.Printers = printers;
                UserContext.BindPrintersToContext();


            }
            else
            {
                string endpoint = configuration.ParsedConfig.Where(conf => conf.Key == "host").FirstOrDefault().Value;
                Console.WriteLine(endpoint);
                RemoteUserContext context = new RemoteUserContext(endpoint);

                string ServerHello = context.ServerHello().GetAwaiter().GetResult();
                string[] HelloSplit = ServerHello.Split("|");
                string[] StatusInfoSplit = HelloSplit[0].Split(";");
                string PrinterInfo = HelloSplit[1];

                string[] PrinterInfoSplit = PrinterInfo.Split("%endpconf%");
                PrinterInfoSplit = PrinterInfoSplit.Take(PrinterInfoSplit.Length - 1).ToArray();


                string rType = StatusInfoSplit[0];
                string uGlobalLimit = StatusInfoSplit[1].Split("=")[1];
                string globalLimit = StatusInfoSplit[2].Split("=")[1];

                List<PrinterContext> printers = new List<PrinterContext>();
                foreach (string printer in PrinterInfoSplit)
                {

                    string[] split = printer.Split(";");
                    string PrintServer = split[0];
                    string PrinterName = split[1];
                    string Limit = split[2];
                    bool GlobalLimit = int.Parse(Limit) == -99 ? true : false;

                    Console.WriteLine("\nGot configuration:");
                    Console.WriteLine($"Printer name: {PrinterName}");
                    Console.WriteLine($"Print Server: {PrintServer}");
                    Console.WriteLine($"Limit: {Limit}");
                    Console.WriteLine($"Limit is global?: {GlobalLimit}");

                    if (GlobalLimit)
                    {
                        PrinterContext con = new PrinterContext(PrintServer, PrinterName, new SharedInt(int.Parse(uGlobalLimit)), new SharedInt(int.Parse(globalLimit)));
                        con.PageCountIncrement = (int amount) =>
                        {
                            context.IncrementCurrentAmount(amount).GetAwaiter().GetResult();
                        };

                        printers.Add(con);
                    }


                }

                context.Printers = printers;
                context.BindPrintersToContext();



                //  Console.WriteLine(response);
                //string inc = context.IncrementCurrentAmount(4).GetAwaiter().GetResult();

                string response2 = context.ServerHello().GetAwaiter().GetResult();

                Console.WriteLine(response2);

            }












            Console.ReadLine();
        }


        static void Init()
        {
            if (!Directory.Exists(ContextsPath))
            {
                Directory.CreateDirectory(ContextsPath);
            }
        }

        static void RunContextSaver(LocalUserContext userContext)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(5000).Wait();
                    using (var stream = new FileStream($"{ContextsPath}\\{userContext.Username}.dat", FileMode.OpenOrCreate))
                    {
                        binaryFormatter.Serialize(stream, userContext);
                    }
                }
            });
        }
    }
}