using System;
using System.Text;
using SelectPdf;
using System.IO;
using HttpTriggerListener.Models;
using Microsoft.Extensions.Logging;

namespace HttpTriggerListener.Services
{
    public static class ReportGeneratorService  //: IReportGeneratorService, IDisposable
    {

        private static ILogger _logger;

        public static void init(ILogger logger)
        {
            _logger = logger;
        }

        public static Stream CreateClaimRequestReport(ClaimRequest claimRequest)
        {
            try
            {
                var pdfFileName = $"{claimRequest.FirstName}-{claimRequest.LastName}-{claimRequest.Id}.pdf";
                var pdfFilePath = $"reports/{pdfFileName}";

                var htmlString = CreateHtmlString(claimRequest);
                var pdfStream = HtmlToPdf(htmlString, pdfFilePath);

                return pdfStream;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error {ex.StackTrace}");
                _logger.LogError($"Error {ex.Message}");
                throw ex;
            }
        }

        private static string CreateHtmlString(ClaimRequest claimRequest)
        {
            try
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append($"<h1>Name: {claimRequest.FirstName} {claimRequest.LastName}</h1>");
                stringBuilder.Append($"<h1>User Id: {claimRequest.UserId}</h1>");
                stringBuilder.Append($"<h1>Email: {claimRequest.Email}</h1>");
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error {ex.StackTrace}");
                _logger.LogError($"Error {ex.Message}");
                throw ex;
            }
        }

        private static Stream HtmlToPdf(string htmlString, string pdfFilePath)
        {
            try
            {
                HtmlToPdf converter = new HtmlToPdf();
                int webPageWidth = 1024;
                var pageSize = PdfPageSize.A4;
                var pdfOrientation = PdfPageOrientation.Portrait;

                converter.Options.PdfPageSize = pageSize;
                converter.Options.PdfPageOrientation = pdfOrientation;
                converter.Options.WebPageWidth = webPageWidth;

                PdfDocument doc = converter.ConvertHtmlString(htmlString);
                var pdfStream = new MemoryStream();
                doc.Save(pdfStream);

                return pdfStream;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error {ex.StackTrace}");
                _logger.LogError($"Error {ex.Message}");
                throw ex;
            }
        }
    }
}
