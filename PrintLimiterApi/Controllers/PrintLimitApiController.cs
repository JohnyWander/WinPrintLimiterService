using Microsoft.AspNetCore.Mvc;
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
            if (UserManager.CheckForContextExistance(data["username"].ToString()))
            {
                return "logged";
               
            }
            else
            {
                UserManager
                return "registered";
            }
           
            
            
            return "X";
         }

    }
}
