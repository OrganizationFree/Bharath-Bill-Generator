using System;

namespace billGenerator.Models
{
    public class BillModel
    {
        //public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientAddress { get; set; }
        public DateTime BillDate { get; set; }
        //public string Particulars { get; set; }
        public float Weight { get; set; }
        public float Rate { get; set; }
        public float Price { get; set; }
        //public int GrandTotal { get; set; }
        public int NoOfArticles { get; set; }
        public int CGST { get; set; }
        public int SGST { get; set; }
        public int IGST { get; set; }
        public float Tax { get; set; }
        public float Total { get; set; }
        public string AmountInWords { get; set; }
    }
}
