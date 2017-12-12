using System;
using System.Collections.Generic;
using System.Linq;
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

        // GET: api/vendedores/3/oportunidadeVenda
        [Route("api/vendedores/{id}/oportunidadeVenda")]
        public List<OportunidadeVenda> GetByRep(string id)
        {
            List<OportunidadeVenda> opVenda = Lib_Primavera.PriIntegration.GetOpVendaByRep(id);

            return opVenda;
        }

        public IEnumerable<Lib_Primavera.Model.OportunidadeVenda> Get()
        {
            return Lib_Primavera.PriIntegration.ListaOpVenda();
        }

        public HttpResponseMessage Post(Lib_Primavera.Model.OportunidadeVenda opVenda)
        {
            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();
            erro = Lib_Primavera.PriIntegration.InsereOpVenda(opVenda);
           
            if (erro.Erro == 0)
            {
                var response = Request.CreateResponse(HttpStatusCode.Created, opVenda);
                return response;
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);
            }      
        }

        public HttpResponseMessage Put(string id, Lib_Primavera.Model.OportunidadeVenda opVenda)
        {

            Lib_Primavera.Model.RespostaErro erro = new Lib_Primavera.Model.RespostaErro();

            try
            {
                erro = Lib_Primavera.PriIntegration.UpdOpVenda(id, opVenda);
                if (erro.Erro == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, erro.Descricao);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, erro.Descricao);
                }
            }

            catch (Exception exc)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, erro.Descricao);
            }
        }
    }
}
