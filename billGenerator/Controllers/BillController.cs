using billGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace billGenerator.Controllers
{
    [ApiController]
    public class BillController : Controller
    {
        [HttpGet]
        [Route("api/Bill/generatePdf")]
        public string getString()
        {
            //string str=string.Concat(
            //    "ClientName : {0}" +
            //    "ClientAddress : {1}" +
            //    "ClientPhone : {2}" +
            //    "ClientPincode : {3}"
            //    , client.ClientName
            //    ,client.ClientAddress
            //    ,client.ClientPhone
            //    ,client.ClientPincode);
            return "success";
        }

        [HttpPost]
        [Route("api/Bill/generatePdfsad")]
        public string getStringsad([FromBody]BillModel bill)
        {
            DateTime sad = new DateTime();
            //string str=string.Concat(
            //    "ClientName : {0}" +
            //    "ClientAddress : {1}" +
            //    "ClientPhone : {2}" +
            //    "ClientPincode : {3}"
            //    , client.ClientName
            //    ,client.ClientAddress
            //    ,client.ClientPhone
            //    ,client.ClientPincode);
            sad=bill.BillDate;
            sad=sad.ToLocalTime();
            return "success";
        }
    }
}