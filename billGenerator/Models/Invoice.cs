using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace billGenerator.Models
{
    public class Invoice
    {
        public string ClientName { get; set; }
        public string ClientAddress { get; set; }
        public DateTime BillDate { get; set; }
        public int NoOfArticles { get; set; }
        public int CGST { get; set; }
        public int SGST { get; set; }
        public int IGST { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
        public string AmountInWords { get; set; }
        public string GSTIN { get; set; }
        public string Transport { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal RoundOff { get; set; }
        public int BillNo { get; set; }
        public decimal Weight { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public string Product { get; set; }
    }
}
