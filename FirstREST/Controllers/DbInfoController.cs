using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FirstREST.Lib_Primavera.Model;

namespace FirstREST.Controllers
{
    public class DbInfoController : ApiController
    {
        // GET: api/dbinfo
        public IEnumerable<Lib_Primavera.Model.DbInfo> Get()
        {
            return Lib_Primavera.PriIntegration.getDbInfo();
        }
    }
}
