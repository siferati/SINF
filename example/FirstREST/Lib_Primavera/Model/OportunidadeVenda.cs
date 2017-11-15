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
        public string Resumo
        {
            get;
            set;
        }
        public string Entidade
        {
            get;
            set;
        }
        public string TipoEntidade
        {
            get;
            set;
        }
        public DateTime Data
        {
            get;
            set;
        }
        public string Vendedor
        {
            get;
            set;
        }
        public string Origem
        {
            get;
            set;
        }
        public string CicloDeVenda
        {
            get;
            set;
        }
        public string Zona
        {
            get;
            set;
        }
    }
}