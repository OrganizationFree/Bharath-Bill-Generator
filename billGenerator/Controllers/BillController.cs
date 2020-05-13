using billGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using System;

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
                sad = bill.BillDate;
                sad = sad.ToLocalTime();
                return 1;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }
    }
}