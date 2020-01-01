
using Microsoft.AspNetCore.Mvc;

namespace LeadChina.PM.WebApi
{
    public class StatusController : ApiControllerBase
    {
        [Route("/status")]
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetStatus()
        {
            return "OK";
        }
    }
}
