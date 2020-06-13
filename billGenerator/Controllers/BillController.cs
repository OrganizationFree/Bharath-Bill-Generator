using billGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using billGenerator.BusiessLogic;


namespace billGenerator.Controllers
{
    [ApiController]
    public class BillController : Controller
    {
        [HttpPost]
        [Route("api/Bill/generatePDF")]
        public int generatePDF([FromBody]BillModel bill)
        {
            try
            {
                DateTime sad = new DateTime();
                bill.BillDate = bill.BillDate.ToLocalTime();
                bill.AmountInWords = NumberToWords.ConvertAmount(bill.GrandTotal);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}