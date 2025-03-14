using PrintLimiterApi.Configuration;
using PrintLimiterApi.Users;

namespace PrintLimiterApi
{
    public class Program
    {
        internal static bool Debug = false;

        internal static UserManager UserManager = new UserManager();

        internal static int GlobalMaxPagesConfig;
        public static void Main(string[] args)
        {
            ApiConfig();
            AspnetConfig(args);
        }

        private static void ApiConfig()
        {
            Configuration.Configuration configuration = new Configuration.Configuration();
            ConfigRecord maxpagesRecord = configuration.ParsedConfig.Where(conf => conf.Key == "maxpages").FirstOrDefault();
            int MaxPages = int.Parse(maxpagesRecord.Value);
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