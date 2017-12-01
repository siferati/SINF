using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class OportunidadeVenda
    {
        public string OportunidadeID
        {
            get;
            set;
        }
        public string DescricaoOp
        {
            get;
            set;
        }
        public string Entidade
        {
            get;
            set;
        }
        public DateTime Data
        {
            get;
            set;
        }
        public string Local
        {
            get;
            set;
        }
        public string VendedorCod
        {
            get;
            set;
        }
    }
}