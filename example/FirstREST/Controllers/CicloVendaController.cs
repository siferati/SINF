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
    public class CicloVendaController : ApiController
    {
        public CicloVenda Get(string id)
        {
            Lib_Primavera.Model.CicloVenda cvenda = Lib_Primavera.PriIntegration.GetCV(id);
            if (cvenda == null)
            {
                throw new HttpResponseException(
                  Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                return cvenda;
            }
        }

    }
}
