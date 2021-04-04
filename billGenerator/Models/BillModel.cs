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
        //public int GrandTotal { get; set; }
        public int NoOfArticles { get; set; }
        public int CGST { get; set; }
        public int SGST { get; set; }
        public int IGST { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
        public string AmountInWords { get; set; }
        public string GSTIN { get; set; }
        public string Transport { get; set; }
        public Items[] Items { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal RoundOff { get; set; }
        public int BillNo { get; set; }

    }
}
