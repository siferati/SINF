using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using FirstREST.Lib_Primavera.Model;

namespace FirstREST.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OportunidadeVendaController : ApiController
    {
        public OportunidadeVenda Get(string id)
        {
            Lib_Primavera.Model.OportunidadeVenda opVenda = Lib_Primavera.PriIntegration.GetOpVenda(id);
            if (opVenda == null)
            {
                throw new HttpResponseException(
                  Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                return opVenda;
            }
        }

        public IEnumerable<Lib_Primavera.Model.OportunidadeVenda> Get()
        {
            return Lib_Primavera.PriIntegration.ListaOpVenda();
        }

    }
}
