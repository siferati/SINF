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
    public class ZonaController : ApiController
    {
        public Zona Get(string id)
        {
            Lib_Primavera.Model.Zona zona = Lib_Primavera.PriIntegration.GetZona(id);
            if (zona == null)
            {
                throw new HttpResponseException(
                  Request.CreateResponse(HttpStatusCode.NotFound));
            }
            else
            {
                return zona;
            }
        }

    }
}
