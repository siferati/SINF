﻿using System;
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

        // GET: api/vendedores/A017    
        public Vendedor Get(string id)
        {
            Lib_Primavera.Model.Vendedor vendedor = Lib_Primavera.PriIntegration.GetVendedor(id);

            return vendedor;
        }
    }
}
