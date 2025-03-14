using Microsoft.AspNetCore.Mvc;
using PrintLimiterApi.Users;
using static PrintLimiterApi.Program;
namespace PrintLimiterApi.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class PrintLimitApiController : ControllerBase
    {

        public PrintLimitApiController()
        {

        }

        [HttpPost("hello")]
         public string hello(IFormCollection data)
         {
            if (Program.UserManager.CheckForContextExistance(data["username"].ToString()))
            {
                RemoteUserContext context = Program.UserManager.GetContext(data["username"].ToString());
                
                return $"logged";
               
            }
            else
            {

                Program.UserManager.CreateUserContext(data["username"]);
                return "registered";
            }
           
            
            
            return "X";
         }

    }
}
