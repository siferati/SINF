using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using FirstREST.Lib_Primavera.Model;

namespace FirstREST.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OrdersController : ApiController
    {
        // GET: api/orders/DBAE7851-AC30-11E6-A18F-080027397412
        public Order Get(string id)
        {
            Lib_Primavera.Model.Order order = Lib_Primavera.PriIntegration.GetOrder(id);

            return order;
        }

        [Route("api/vendedores/{id}/orders")]
        public List<Order> GetByRep(string id)
        {
            List<Order> orders = Lib_Primavera.PriIntegration.GetOrdersByRep(id);

            return orders;
        }
    }
}
