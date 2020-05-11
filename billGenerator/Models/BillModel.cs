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
        public int Weight { get; set; }
        public int Rate { get; set; }
        //public int Amount { get; set; }
        //public int GrandTotal { get; set; }
        public int NoOfArticles { get; set; }
        public int CGST { get; set; }
        public int SGST { get; set; }
        public int IGST { get; set; }
        //public int Tax { get; set; }
        //public int Total { get; set; }
        //public string AmountInWords { get; set; }
    }
}
