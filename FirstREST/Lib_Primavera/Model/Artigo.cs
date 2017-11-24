using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Artigo
    {
        public string productId
        {
            get;
            set;
        }

        public string name
        {
            get;
            set;
        }

        public double price
        {
            get;
            set;
        }

        public string VAT
        {
            get;
            set;
        }

        public string size
        {
            get;
            set;
        }

        public string type
        {
            get;
            set;
        }

        public double stock
        {
            get;
            set;
        }

        public double weight
        {
            get;
            set;
        }

        public string description
        {
            get;
            set;
        }
    }
}