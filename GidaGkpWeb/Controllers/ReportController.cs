using GidaGkpWeb.BAL;
using GidaGkpWeb.Infrastructure.Utility;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace GidaGkpWeb.Controllers
{
    [AdminSessionTimeout]
    public class ReportController : CommonController
    {
        public ActionResult SelectedInterviewCandidate()
        {
            return View();
        }
        public ActionResult RejectedInterviewCandidate()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetSelectedApplicant(int schemeName)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantSubmittedForInterview(schemeName).Where(x => x.InterviewLetterStatus == "Selected").ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportSelectedApplicant(string GridHtml)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "SelectedApplicant.pdf");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportRejectedApplicant(string GridHtml)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "RejectedApplicant.pdf");
            }
        }

        [HttpPost]
        public JsonResult GetRejectedApplicant(int schemeName)
        {
            AdminDetails _details = new AdminDetails();
            var data = _details.GetApplicantSubmittedForInterview(schemeName).Where(x => x.InterviewLetterStatus == "Rejected" ).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }



    }
}