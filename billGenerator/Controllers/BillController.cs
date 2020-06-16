using billGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using billGenerator.BusiessLogic;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


namespace billGenerator.Controllers
{
    [ApiController]
    public class BillController : Controller
    {
        [HttpPost]
        [Route("api/Bill/generatePDF")]
        public IActionResult generatePDF([FromBody]BillModel bill)
        {
            try
            {
                DateTime sad = new DateTime();
                bill.BillDate = bill.BillDate.ToLocalTime();
                bill.AmountInWords = NumberToWords.ConvertAmount(bill.GrandTotal);
                var response = createPdf(bill);
                return response;
            }
            catch (Exception ex)
            {
                MemoryStream stream = new MemoryStream();
                return new FileStreamResult(stream, "application/pdf");
            }
        }

        private FileStreamResult createPdf(BillModel bill)
        {
            StringBuilder sb = new StringBuilder();
            sb = populateData(bill);
            PdfPTable table = generateTable(bill);
            StringReader sr = new StringReader(sb.ToString());
            Byte[] res = null;
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Add(table); 
                pdfDoc.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                res = memoryStream.ToArray();
            }
            MemoryStream msnew = new MemoryStream(res);
            return new FileStreamResult(msnew, "application/pdf");
        }

        private StringBuilder populateData(BillModel bill)
        {
            StringBuilder sb=new  StringBuilder();
            sb.Append("<p><h1>Hello World</h1>This is html rendered text</p>");
            sb.Append(String.Format("<body> Grand Total : {0} </body>",
                bill.GrandTotal));
            return sb;
        }

        private PdfPTable generateTable(BillModel bill)
        {
            // Creating a table       
            PdfPTable table = new PdfPTable(3);
            PdfPCell cell = new PdfPCell(new Phrase("Items"));
            cell.Colspan = 3;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);
            foreach (var item in bill.Items)
            {
                table.AddCell(item.Weight.ToString());
                table.AddCell(item.Rate.ToString());
                table.AddCell(item.Price.ToString());
            }
            return table;
        }

    }
}