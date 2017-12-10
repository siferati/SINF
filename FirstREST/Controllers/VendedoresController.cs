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
    public class VendedoresController : ApiController
    {
        // GET: api/vendedores
        public IEnumerable<Lib_Primavera.Model.Vendedor> Get()
        {
            return Lib_Primavera.PriIntegration.ListaVendedores();
        }

        // GET: api/vendedores/3    
        public Vendedor Get(string id)
        {
            Lib_Primavera.Model.Vendedor vendedor = Lib_Primavera.PriIntegration.GetVendedor(id);

            return vendedor;
        }

        // POST: api/vendedores/
        public HttpResponseMessage Post(Lib_Primavera.Model.Vendedor vendedor)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            erro = Lib_Primavera.PriIntegration.InsereVendedorObj(vendedor);

            if (erro.Erro == 0)
            {
                var response = Request.CreateResponse(HttpStatusCode.Created, vendedor);
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }
    }
}
