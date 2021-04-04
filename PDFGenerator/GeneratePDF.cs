using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ExpertPdf.HtmlToPdf;
using System.Drawing;
using ExpertPdf.HtmlToPdf.PdfDocument;
using PDFGenerator.Constants;

namespace PDFServices
{
    public class GeneratePDF
    {

        public void CreatePDf(string pageUrl, string saveDestination)
        {

            #region Create PDF
            try
            {

                PdfConverter pdfConverter = new PdfConverter();
                pdfConverter.InternetSecurityZone = InternetSecurityZone.Trusted;

                //pdfConverter.AuthenticationOptions
                pdfConverter.AuthenticationOptions.Username = HttpContext.Current.User.Identity.Name;
                pdfConverter.HtmlElementsMappingOptions.HtmlTagNames = new string[] { "form" };
                pdfConverter.PdfDocumentOptions.PdfPageSize = ExpertPdf.HtmlToPdf.PdfPageSize.A4;
                pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
                pdfConverter.PdfDocumentOptions.ShowHeader = true;
                pdfConverter.PdfDocumentOptions.ShowFooter = true;
                pdfConverter.PdfDocumentOptions.LeftMargin = 10;
                pdfConverter.PdfDocumentOptions.RightMargin = 10;
                pdfConverter.PdfDocumentOptions.TopMargin = 10;
                pdfConverter.PdfDocumentOptions.BottomMargin = 10;


                #region Header Part Commented
                // pdfConverter.PdfDocumentOptions.ShowHeader = true;
                //  pdfConverter.PdfHeaderOptions.HeaderText = "sample header: " + "header data";
                // pdfConverter.PdfHeaderOptions.HeaderTextColor = System.Drawing.Color.Blue;
                // pdfConverter.PdfHeaderOptions.header = string.empty;
                //  pdfConverter.PdfHeaderOptions.DrawHeaderLine = false;

                // pdfConverter.PdfFooterOptions.FooterText = "Sample footer: " + "foooter Content" +
                // ". You can change color, font and other options";
                // pdfConverter.PdfFooterOptions.FooterTextColor = System.Drawing.Color.Blue;
                // pdfConverter.PdfFooterOptions.DrawFooterLine = false;
                // pdfConverter.PdfFooterOptions.PageNumberText = "Page";
                // pdfConverter.PdfFooterOptions.ShowPageNumber = true;
                //System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                //response.Clear();
                //response.AddHeader("Content-Type", "binary/octet-stream");
                //response.AddHeader("Content-Disposition",
                //  "attachment; filename=" + "ExpertPdf-Trail-" + DateTime.Now.ToShortDateString() + "; size=" + downloadBytes.Length.ToString());
                //response.Flush();
                //response.BinaryWrite(downloadBytes);
                //response.Flush();
                //response.End();

                #endregion                

                ExpertPdf.HtmlToPdf.PdfDocument.Document pdfDocument = pdfConverter.GetPdfDocumentObjectFromUrl(pageUrl);
                PdfFont stdTimesFont = pdfDocument.AddFont(StdFontBaseFamily.TimesRoman);
                stdTimesFont.Size = 50;
                //  byte[] downloadBytes = pdfConverter.GetPdfFromUrlBytes(pageUrl);

                foreach (HtmlElementMapping elementMapping in pdfConverter.HtmlElementsMappingOptions.HtmlElementsMappingResult)
                {

                    foreach (HtmlElementPdfRectangle elementLocationInPdf in elementMapping.PdfRectangles)
                    {
                        // get the PDF page
                        ExpertPdf.HtmlToPdf.PdfDocument.PdfPage pdfPage = pdfDocument.Pages[elementLocationInPdf.PageIndex];

                        {
                            float xPos = -5;
                            float yPos = 370;
                            float rotateAngle = -45;
                            for (int i = 0; i < 2; i++)
                            {

                                TextElement watermarkTextElement = new TextElement(xPos, yPos, PDFReportConstants.WaterMarkText, stdTimesFont);
                                watermarkTextElement.ForeColor = System.Drawing.Color.LightGray;
                                watermarkTextElement.Transparency = 85;
                                watermarkTextElement.Rotate(rotateAngle);

                                // watermarkTemplate.AddElement(watermarkTextElement);
                                pdfPage.AddElement(watermarkTextElement);
                                xPos = 0;
                                rotateAngle = rotateAngle + 5;
                                // xPos = xPos + 100;
                                yPos = yPos + 200;

                            }
                        }
                    }
                }

                string outFile = saveDestination;

                //string filepath =HttpContext.Current.Server.MapPath("~/Templates/Risk Rating Template/Risk_Rating_Template.xlsx");

                pdfDocument.Save(saveDestination);
                #region Commented Part
                // the code below can be replaced by pdfMerger.SaveMergedPDFToFile(outFile);
                //System.IO.FileStream fs = null;
                //fs = new System.IO.FileStream(outFile, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.Read);
                //fs.Write(downloadBytes, 0, downloadBytes.Length);
                //fs.Close();
                #endregion

                System.Diagnostics.Process.Start(outFile);
            }

            catch (Exception ex)
            {
                //LogManager.GetLogger(LogCategory.Error).Log(ex, LoginextLogUtility.EXP_LOG_AND_RETHROW);
            }
            #endregion


        }

        #region Dwnload and delete part
        public void DownloadFile(string source)
        {
            try
            {
                //To Get the physical Path of the file(me2.doc)
                string filepath = source;

                // Create New instance of FileInfo class to get the properties of the file being downloaded
                FileInfo myfile = new FileInfo(filepath);

                // Checking if file exists
                if (myfile.Exists)
                {
                    // Clear the content of the response
                    HttpContext.Current.Response.ClearContent();

                    // Add the file name and attachment, which will force the open/cancel/save dialog box to show, to the header
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + myfile.Name);

                    // Add the file size into the response header
                    HttpContext.Current.Response.AddHeader("Content-Length", myfile.Length.ToString());

                    // Set the ContentType
                    HttpContext.Current.Response.ContentType = "application/pdf";

                    // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                    HttpContext.Current.Response.TransmitFile(myfile.FullName);

                    // End the response
                    HttpContext.Current.Response.End();
                }
            }
            catch (Exception ex)
            {
                //LogManager.GetLogger(LogCategory.Error).Log(ex, LoginextLogUtility.EXP_LOG_AND_RETHROW);

            }
        }

        public void DeleteFile(string filename)
        {
            //Check if file exists
            //Delete copy of the file from the server
            FileInfo deleteFile = new FileInfo(filename);
            if (deleteFile.Exists)
                File.Delete(filename);
        }
        #endregion








    }
}
