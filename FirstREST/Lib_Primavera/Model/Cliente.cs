using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Cliente
    {
        public string customerId
        {
            get;
            set;
        }

        public string fiscalId
        {
            get;
            set;
        }

        public string name
        {
            get;
            set;
        }

        public string address
        {
            get;
            set;
        }
        
        public string phone
        {
            get;
            set;
        }

        public string email
        {
            get;
            set;
        }

        public string status
        {
            get;
            set;
        }

        public int orders
        {
            get;
            set;
        }

        public string description
        {
            get;
            set;
        }

        public string picture
        {
            get;
            set;
        }
    }
}