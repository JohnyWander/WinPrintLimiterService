using System.IO;
using WinPrintLimiter.PrintControl;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualBasic.Logging;
using System.Runtime.Serialization;
using WinPrintLimiter.Configuration;
using WinPrintLimiter.PrintControl;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
                Console.ReadLine();Environment.Exit(0);
            }
            else if(mode.Value=="local")
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                string Username = Environment.UserName;

                ConfigRecord maxpagesRecord = configuration.ParsedConfig.Where(conf => conf.Key =="maxpages").FirstOrDefault();
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
                    UserContext = new LocalUserContext(Environment.UserName,MaxPages);

                    RunContextSaver(UserContext);
                }

                List<PrinterContext> printers = new List<PrinterContext>();
                configuration.ParsedPrinterConfig.ForEach(p =>
                {
                   
                    PrinterContext context;
                    if (p.InheritsFromGlobalLimit)
                    {
                         context = new PrinterContext(p.PrintServer, p.PrinterName,UserContext.GlobalJobsCounter,UserContext.MaxGlobal);
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
                    context.TryRegister().Wait();
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