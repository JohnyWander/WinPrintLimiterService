using Microsoft.AspNetCore.Mvc;
using PrintLimiterApi.Users;
using System.Text;
namespace PrintLimiterApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PrintLimitApiController : ControllerBase
    {

        public PrintLimitApiController()
        {

        }

        [HttpPost("incrementcount")]
        public string IncrementCount(IFormCollection data)
        {
            string username = data["username"].ToString();
            string amount = data["amount"];
            Console.WriteLine("increment from:" + username + ", by " + amount);
            RemoteUserContext context = Program.UserManager.Clients.Where(x => x.UserName == username).FirstOrDefault();

            context.CurrentPagesCount.value += int.Parse(amount);

            return "incremented";
        }

        [HttpPost("hello")]
        public string hello(IFormCollection data)
        {
            string username = data["username"].ToString();
            Console.WriteLine("hello from:" + username);
            StringBuilder pconf = new StringBuilder();
            Program.Configuration.ParsedPrinterConfig.ForEach(x =>
            {
                pconf.Append(x.PrintServer + ";");
                pconf.Append(x.PrinterName + ";");
                pconf.Append(x.DailyPagesLimit + ";");
                pconf.Append(x.FriendlyName+";");
                pconf.Append("%endpconf%");

            });
            Console.WriteLine(pconf.ToString());

            RemoteUserContext context = Program.UserManager.Clients.Where(x => x.UserName == username).FirstOrDefault();
            if (context is not null)
            {


                return $"logged;UsedGlobalLimit={context.CurrentPagesCount.value};GlobalLimit={context.MaxPages.value}||{pconf.ToString()}";


            }
            else
            {

                RemoteUserContext con = Program.UserManager.CreateUserContext(username);
                return $"registered;UsedGlobalLimit={con.CurrentPagesCount.value};GlobalLimit={con.MaxPages.value}||{pconf.ToString()}";


            }
            return "X";
        }


    }
}
