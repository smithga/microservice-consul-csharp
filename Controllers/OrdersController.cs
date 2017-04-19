using System.Web.Http;

namespace MicroservicesExample.Controllers
{

    [Route("orders")]
    public class OrdersController : ApiController
    {

        public string Get()
        {
            return "Orders Service";
        }
    }

}
