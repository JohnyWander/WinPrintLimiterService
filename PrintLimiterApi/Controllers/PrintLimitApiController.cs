using Microsoft.AspNetCore.Mvc;

namespace PrintLimiterApi.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class PrintLimitApiController : ControllerBase
    {

        public PrintLimitApiController()
        {

        }

        [HttpGet("ok")]
         public string ok() {
            return "X";
        }

    }
}
