using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Reporting.WebForms;

namespace LogiNext.Infrastructure.PDFServices
{
    public class ReportGenerator
    {
        private const string pdfDeviceInfo = "<DeviceInfo>" +
                                                       "  <OutputFormat>PDF</OutputFormat>" +
                                                       "  <PageWidth>14.2625in</PageWidth>" +
                                                       "  <PageHeight>9in</PageHeight>" +
                                                       "  <MarginTop>0.5in</MarginTop>" +
                                                       "  <MarginLeft>1in</MarginLeft>" +
                                                       "  <MarginRight>1in</MarginRight>" +
                                                       "  <MarginBottom>0.5in</MarginBottom>" +
                                                       "</DeviceInfo>";

        public byte[] GetPDF(string reportPath, DataSet reportData, string formatNm)
        {
            Warning[] warnings; string[] streamids; string mimeType; string encoding; string filenameExtension;
            Microsoft.Reporting.WebForms.LocalReport locReport = new LocalReport();
            locReport.ReportPath = reportPath;
            foreach (DataTable dtbl in reportData.Tables)
            {
                locReport.DataSources.Add(new ReportDataSource(string.Format("{0}_{1}", reportData.DataSetName, dtbl.TableName), dtbl));
            }
            byte[] pdfBytes = locReport.Render(formatNm, null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            return pdfBytes;
        }
        /// <summary>
        /// Generates PDF using embedded rdlc file in 
        /// </summary>
        /// <param name="reportName">Name of the rdlc file without extension</param>
        /// <param name="reportData">DataSet for generating the report</param>
        /// <param name="formatNm">Format Name like PDF, EXCEL etc.</param>
        /// <param name="dataSrcDataTableMapping">Mapping between the rdlc DataSource name and the DataTable name</param>
        /// <param name="subReportNames">If the main report has any sub reports, pass the sub report names here</param>
        /// <returns>Exported file in the format specified</returns>
        public byte[] GetPDF(string reportName, DataSet reportData, string formatNm, Dictionary<string, string> dataSrcDataTableMapping, List<string> subReportNames)
        {
            return GetPDFFromEmbeddedReport(reportName, reportData, formatNm, null, dataSrcDataTableMapping, subReportNames);
        }
        public byte[] GetPDFFromEmbeddedReport(string reportName, DataSet reportData, string formatNm)
        {
            return GetPDFFromEmbeddedReport(reportName, reportData, formatNm, null, null);
        }
        public byte[] GetPDFFromEmbeddedReport(string reportName, DataSet reportData, string formatNm, Dictionary<string, string> dataSrcDataTableMapping, List<string> subReportNames)
        {
            return GetPDFFromEmbeddedReport(reportName, reportData, formatNm, null, dataSrcDataTableMapping, subReportNames);
        }
        /// <summary>
        ///  This method is for the reports with the parameters        
        /// </summary>
        /// <param name="reportName"></param>
        /// <param name="reportData"></param>
        /// <param name="formatNm"></param>
        /// <returns></returns>
        public byte[] GetPDFFromEmbeddedReport(string reportName, DataSet reportData, string formatNm, IEnumerable<ReportParameter> reportParams)
        {
            return GetPDFFromEmbeddedReport(reportName, reportData, formatNm, reportParams, null, null);
        }
        public byte[] GetPDFFromEmbeddedReport(string reportName, DataSet reportData, string formatNm, IEnumerable<ReportParameter> reportParams, Dictionary<string, string> dataSrcDataTableMapping, List<string> subReportNames = null)
        {
            return GetExportFileFromEmbeddedReport(reportName, reportData, formatNm, reportParams, dataSrcDataTableMapping, subReportNames);
        }
        public byte[] GetEmbeddedReportExportFile(string reportName, DataSet reportData, string formatNm, Dictionary<string, string> dataSrcDataTableMapping, List<string> subReportNames = null)
        {
            return GetExportFileFromEmbeddedReport(reportName, reportData, formatNm, null, dataSrcDataTableMapping, subReportNames);
        }
        public byte[] GetExportFileFromEmbeddedReport(string reportName, DataSet reportData, string formatNm, Dictionary<string, string> dataSrcDataTableMapping, List<string> subReportNames)
        {
            return GetExportFileFromEmbeddedReport(reportName, reportData, formatNm, null, dataSrcDataTableMapping, subReportNames);
        }
        public byte[] GetExportFileFromEmbeddedReport(string reportName, DataSet reportData, string formatNm, IEnumerable<ReportParameter> reportParams, Dictionary<string, string> dataSrcDataTableMapping, List<string> subReportNames = null)
        {
            string fileExtension = string.Empty;
            string mimeType = string.Empty;
            return GetExportFileFromEmbeddedReport(reportName, reportData, formatNm, reportParams, dataSrcDataTableMapping, subReportNames, out fileExtension, out mimeType);
        }
        /// <summary>
        /// Export RDLC report to excel format. If the reportViewer rendering extension supports .xlsx extension, the export will be with .xlsx format else it will be .xls format.
        /// </summary>
        /// <param name="reportName">Name of the rdlc file without extension</param>
        /// <param name="reportData">DataSet for generating the report</param>
        /// <param name="dataSrcDataTableMapping">Mapping between the rdlc DataSource name and the DataTable name</param>
        /// <param name="subReportNames">If the main report has any sub reports, pass the sub report names here</param>
        /// <param name="sheetNameMapping">If the sheet names of the excel needs to be changed, use this parameter. The generated sheetname is the key. This parameter is used only if Rendering Extension supports .xlsx format.</param>
        /// <param name="fileExtension">Extension of the generated file</param>
        /// <returns>Exported Excel file</returns>
        //public byte[] GetExcelFileFromEmbeddedReport(string reportName, DataSet reportData, Dictionary<string, string> dataSrcDataTableMapping, List<string> subReportNames, Dictionary<string, string> sheetNameMapping, out string fileExtension)
        //{
        //    string formatNm = "EXCEL";
        //    IEnumerable<ReportParameter> reportParams = null;
        //    byte[] excelBytes = GetExportFileFromEmbeddedReport(reportName, reportData, formatNm, reportParams, dataSrcDataTableMapping, subReportNames, out fileExtension);

        //    if (fileExtension.ToUpper() == "XLSX" && sheetNameMapping != null && sheetNameMapping.Count > 0)
        //    {
        //        byte[] fixedExcelBytes = (new ExcelProcessor()).FixSheetNames(excelBytes, sheetNameMapping);
        //        return fixedExcelBytes;
        //    }

        //    return excelBytes;
        //}
        public byte[] GetExportFileFromEmbeddedReport(string reportName, DataSet reportData, string formatNm, IEnumerable<ReportParameter> reportParams, Dictionary<string, string> dataSrcDataTableMapping, List<string> subReportNames, out string fileExtension, out string mimeNType)
        {
            return this.GetExportFileFromEmbeddedReport(reportName, reportData, formatNm, reportParams, dataSrcDataTableMapping, subReportNames, false, out fileExtension, out mimeNType);
        }
        public byte[] GetExportFileFromEmbeddedReport(string reportName, DataSet reportData, string formatNm, IEnumerable<ReportParameter> reportParams, Dictionary<string, string> dataSrcDataTableMapping, List<string> subReportNames, bool includeDeviceInfo, out string fileExtension, out string mimeNType)
        {
            Warning[] warnings; string[] streamids; string mimeType; string encoding; string filenameExtension;

            //Set deviceInfo parameter
            string deviceInfo = null;
            if (includeDeviceInfo)
            {
                if (formatNm == "PDF")
                    deviceInfo = pdfDeviceInfo;
            }

            Microsoft.Reporting.WebForms.LocalReport locReport = new LocalReport();
            locReport.LoadReportDefinition(GetResourceStream(reportName.Trim()));
            if (dataSrcDataTableMapping != null && dataSrcDataTableMapping.Count > 0)
            {
                foreach (var dataSrcMapping in dataSrcDataTableMapping)
                {
                    if (reportData.Tables.Contains(dataSrcMapping.Value))
                    {
                        locReport.DataSources.Add(new ReportDataSource(dataSrcMapping.Key, reportData.Tables[dataSrcMapping.Value]));
                    }
                }
            }
            else
            {
                foreach (DataTable dtbl in reportData.Tables)
                {
                    //locReport.DataSources.Add(new ReportDataSource(string.Format("{0}_{1}", reportData.DataSetName, dtbl.TableName), dtbl));
                    locReport.DataSources.Add(new ReportDataSource(string.Format("{0}", reportData.DataSetName), dtbl));
                }
            }
            if (subReportNames != null && subReportNames.Count > 0)
            {
                foreach (string subReport in subReportNames)
                {
                    locReport.LoadSubreportDefinition(subReport, GetResourceStream(subReport));
                }
                locReport.SubreportProcessing += new SubreportProcessingEventHandler(locReport_SubreportProcessing);
            }
            if (formatNm.ToUpper() == "EXCEL")
            {
                RenderingExtension[] renderExts = locReport.ListRenderingExtensions();
                foreach (RenderingExtension renExt in renderExts)
                {
                    if (renExt.Name.ToUpper() == "EXCELOPENXML")
                    {
                        formatNm = renExt.Name;
                        break;
                    }
                }

            }
            if (reportParams != null) locReport.SetParameters(reportParams);
            byte[] pdfBytes = locReport.Render(formatNm, deviceInfo, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            fileExtension = filenameExtension;
            mimeNType = mimeType;
            return pdfBytes;
        }

        void locReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            foreach (ReportDataSource dataSrc in ((Microsoft.Reporting.WebForms.LocalReport)sender).DataSources)
            {
                e.DataSources.Add(dataSrc);
            }
        }
        private Stream GetResourceStream(string reportName)
        {
            Assembly assembly = AppDomain.CurrentDomain.Load("LogiNext.Reporting,Version=1.0.0.0,Culture=neutral,PublicKeyToken=a39e71996fc5a955");
            Stream rptStream = assembly.GetManifestResourceStream("LogiNext.Reporting.Reports.User." + reportName.Trim() + ".rdlc");
            return rptStream;
        }
    }
}

