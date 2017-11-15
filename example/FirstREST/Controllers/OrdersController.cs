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
        // GET: api/orders
        public IEnumerable<Lib_Primavera.Model.Order> Get()
        {
            return Lib_Primavera.PriIntegration.ListaOrders();
        }
    }
}
