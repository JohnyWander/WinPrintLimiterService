using PrintLimiterApi.Configuration;
using PrintLimiterApi.Users;

namespace PrintLimiterApi
{
    public class Program
    {
        internal static bool Debug = false;

        internal static UserManager UserManager = new UserManager();

        internal static int GlobalMaxPagesConfig;

        internal static Configuration.Configuration Configuration;

        public static void Main(string[] args)
        {
            ApiConfig();
            AspnetConfig(args);
        }

        private static void ApiConfig()
        {
            Configuration = new Configuration.Configuration();

            ConfigRecord maxpagesRecord = Configuration.ParsedConfig.Where(conf => conf.Key == "maxpages").FirstOrDefault();
            GlobalMaxPagesConfig = int.Parse(maxpagesRecord.Value);

            Configuration.ParsedPrinterConfig.ForEach(x =>
            {
                Console.WriteLine(x.PrintServer);
            });
        }

        private static void AspnetConfig(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }



    }
}