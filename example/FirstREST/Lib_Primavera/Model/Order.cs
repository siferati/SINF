using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Order
    {
        public string salesOrderId
        {
            get;
            set;
        }

        public string customerId
        {
            get;
            set;
        }

        public string repId
        {
            get;
            set;
        }

        public string productId
        {
            get;
            set;
        }

        public int quantity
        {
            get;
            set;
        }

        public DateTime orderDate
        {
            get;
            set;
        }

        public DateTime deliveryDate
        {
            get;
            set;
        }

        public string status
        {
            get;
            set;
        }

        public string deliveryAddress
        {
            get;
            set;
        }
    }
}