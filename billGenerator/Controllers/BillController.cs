using billGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using billGenerator.BusiessLogic;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using AspNetCore.Reporting;
using System.Data;
using System.Reflection;
using System.Linq;

namespace billGenerator.Controllers
{
    [ApiController]
    public class BillController : Controller
    {

        [AttributeUsage(AttributeTargets.Property)]
        public class DataColumnAttribute : Attribute { }

        private readonly IWebHostEnvironment _webHostEnvironment;

        public BillController(IWebHostEnvironment iWebHostEnvironment)
        {
            this._webHostEnvironment = iWebHostEnvironment;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [HttpPost]
        [Route("api/Bill/generateInvoice")]
        public IActionResult generateInvoice([FromBody] BillModel bill)
        {
            try
            {
                DateTime sad = new DateTime();
                bill.BillDate = bill.BillDate.ToLocalTime();
                var GrandTotal = Convert.ToDouble(bill.GrandTotal.ToString());
                bill.AmountInWords = NumberToWords.ConvertAmount(GrandTotal);
                return PrintInvoice(bill);
            }
            catch (Exception ex)
            {
                MemoryStream stream = new MemoryStream();
                return new FileStreamResult(stream, "application/pdf");
            }
        }


        //[HttpPost]
        //[Route("api/Bill/generatePDF2")]
        private IActionResult PrintInvoice(BillModel bill)
        {
            string mimeType = "";
            int extention = 1;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("ClientName", bill.ClientName);
            var path = $"{this._webHostEnvironment.WebRootPath}";
            path = path.Replace("wwwroot", "Reporting");
            path = path + "\\Invoice.rdlc";
            DataTable dt = new DataTable();
            dt = generateDataSet(bill);
            //SetParameter(dt);
            var data = GetEntities<Invoice>(dt);

            LocalReport lclReport = new LocalReport(path);            
            lclReport.AddDataSource("DataSet1", data);
            var result = lclReport.Execute(RenderType.Pdf, extention, parameters, mimeType);            
            return File(result.MainStream, "application/pdf");
        }

        [HttpPost]
        [Route("api/Bill/generateEstimate")]
        public IActionResult generateEstimate([FromBody] BillModel bill)
        {
            try
            {
                DateTime sad = new DateTime();
                bill.BillDate = bill.BillDate.ToLocalTime();
                var GrandTotal = Convert.ToDouble(bill.GrandTotal.ToString());
                bill.AmountInWords = NumberToWords.ConvertAmount(GrandTotal);
                return PrintEstimate(bill);
            }
            catch (Exception ex)
            {
                MemoryStream stream = new MemoryStream();
                return new FileStreamResult(stream, "application/pdf");
            }
        }

        private IActionResult PrintEstimate(BillModel bill)
        {
            string mimeType = "";
            int extention = 1;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("ClientName", bill.ClientName);
            var path = $"{this._webHostEnvironment.WebRootPath}";
            path = path.Replace("wwwroot", "Reporting");
            path = path + "\\Estimate.rdlc";
            DataTable dt = new DataTable();
            dt = generateDataSet(bill);
            //SetParameter(dt);
            var data = GetEntities<Invoice>(dt);

            LocalReport lclReport = new LocalReport(path);
            lclReport.AddDataSource("DataSet1", data);
            var result = lclReport.Execute(RenderType.Pdf, extention, parameters, mimeType);
            return File(result.MainStream, "application/pdf");
        }

        private DataTable generateDataSet(BillModel dataObject)
        {            
            DataTable dt = new DataTable();
            foreach (var elements in dataObject.GetType().GetProperties())
            {
                if (!elements.PropertyType.Name.Contains("[]"))
                    dt.Columns.Add(elements.Name.ToString());
                else
                if (elements.PropertyType.Name == "Items[]")
                {
                    foreach (var item in dataObject.Items[0].GetType().GetProperties())
                    {
                        dt.Columns.Add(item.Name.ToString());
                    }
                }
            }
            foreach (var item in dataObject.Items)
            {
                dt.Rows.Add(
                  dataObject.ClientName,
                  dataObject.ClientAddress,
                  dataObject.BillDate,
                  dataObject.NoOfArticles,
                  dataObject.CGST,
                  dataObject.SGST,
                  dataObject.IGST,
                  dataObject.Tax,
                  dataObject.GrandTotal,
                  dataObject.AmountInWords,
                  dataObject.GSTIN,
                  dataObject.Transport,
                  item.Weight,
                  item.Rate,
                  item.Price,
                  item.Product,
                  dataObject.TotalPrice,
                  dataObject.RoundOff,
                  dataObject.BillNo
                  );
            }

            return dt;
        }


        private static Dictionary<string,string> SetParameter(DataTable dataTable)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            foreach (DataColumn column in dataTable.Columns)
            {
                foreach(DataRow row in dataTable.Rows)
                {
                    var colName = column.ColumnName;
                    var rowName = row[colName].ToString();
                    parameters.Add(colName, rowName);
                }
            }

            return parameters;

            // To convert the datatable to enumerable to list
            //List<DataRow> list = dataTable.AsEnumerable().ToList();

            //return dataTable.AsEnumerable().Select(row => new BillModel
            //{
            //    //AmountInWords = (row["AmountInWords"].ToString()),
            //    //BillDate = Convert.ToDateTime(row["BillDate"].ToString()),
            //    //CGST = Convert.ToInt32(row["CGST"].ToString()),
            //    //BillNo = Convert.ToInt32(row["BillNo"].ToString()),
            //    //ClientAddress = (row["ClientAddress"].ToString()),
            //    ClientName = row["ClientName"].ToString(),
            //    //GrandTotal = Convert.ToInt32(row["GrandTotal"].ToString()),
            //    //GSTIN = (row["GSTIN"].ToString()),
            //    //IGST = Convert.ToInt32(row["IGST"].ToString()),
            //    NoOfArticles= Convert.ToInt32(row["NoOfArticles"].ToString()),
            //});
        }

       

        /// <summary>
        /// Get entities from DataTable
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        private IEnumerable<T> GetEntities<T>(DataTable dt)
        {
            try
            {
                if (dt == null)
                {
                    return null;
                }

                List<T> returnValue = new List<T>();
                List<string> typeProperties = new List<string>();

                T typeInstance = Activator.CreateInstance<T>();

                foreach (DataColumn column in dt.Columns)
                {
                    var prop = typeInstance.GetType().GetProperty(column.ColumnName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    if (prop != null)
                    {
                        typeProperties.Add(column.ColumnName);
                    }
                }

                foreach (DataRow row in dt.Rows)
                {
                    T entity = Activator.CreateInstance<T>();

                    foreach (var propertyName in typeProperties)
                    {

                        if (row[propertyName] != DBNull.Value)
                        {
                            string str = row[propertyName].GetType().FullName;
                            var proName = propertyName;
                            var ptype = entity.GetType().GetProperty(propertyName).PropertyType;
                            if (entity.GetType().GetProperty(propertyName).PropertyType == typeof(System.String))
                            {
                                object Val = row[propertyName].ToString();
                                entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, Val, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                            }
                            else if (entity.GetType().GetProperty(propertyName).PropertyType == typeof(System.DateTime))
                            {
                                object Val = Convert.ToDateTime(row[propertyName].ToString());
                                entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, Val, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                            }
                            else if (entity.GetType().GetProperty(propertyName).PropertyType == typeof(System.Int32))
                            {
                                object Val = Convert.ToInt32(row[propertyName].ToString());
                                entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, Val, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                            }
                            else if (entity.GetType().GetProperty(propertyName).PropertyType == typeof(System.Int64))
                            {
                                object Val = Convert.ToInt64(row[propertyName].ToString());
                                entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, Val, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                            }
                            else if (entity.GetType().GetProperty(propertyName).PropertyType == typeof(System.Decimal))
                            {
                                object Val = Convert.ToDecimal(row[propertyName].ToString());
                                entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, Val, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                            }
                            else if (entity.GetType().GetProperty(propertyName).PropertyType == typeof(System.Guid))
                            {
                                object Val = Guid.Parse(row[propertyName].ToString());
                                entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, Val, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                            }
                            else
                            {
                                entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, row[propertyName], BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                            }
                        }
                        else
                        {
                            entity.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, null, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, null, null);
                        }
                    }

                    returnValue.Add(entity);
                }

                return returnValue.AsEnumerable();
            }
            catch(Exception ex)
            {
                
            }
            return null;
        }

        //private FileContentResult PDFTool(DataSet rptDataset, string reportName, string pageName)
        //{

        //    string reportType = GlobalReportTypeConstants.PDF;
        //    string mimeNType = string.Empty;
        //    string fileExtension = string.Empty;
        //    byte[] renderedBytes = null;
        //    try
        //    {
        //        //Call report generator
        //        renderedBytes = reportGenerator.GetExportFileFromEmbeddedReport(reportName, rptDataset, reportType, null, null, null, true, out fileExtension, out mimeNType);
        //        Response.AddHeader("content-disposition", "attachment; filename=" + reportName + "." + fileExtension);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return File(renderedBytes, mimeNType);

        //}

    }
}