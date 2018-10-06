using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using EMR.WebAPI.ehr.models;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using EMR.WebAPI.ehr.ansi5010;

namespace EMR.WebAPI.ehr.services
{
    public class ReportServiceController : ApiController
    {
        string dlPath = HttpContext.Current.Server.MapPath("~/ehr/downloads");
        string imgPath = HttpContext.Current.Server.MapPath("~/ehr/images");

        #region Utility Functions
        private bool GetBooleanValue(bool? key)
        {
            return key == null ? false : key.Value;
        }

        private DateTime GetDateTimeValue(DateTime? dateTime)
        {
            return dateTime == null ?
                DateTime.Parse("01/01/1000") : dateTime.Value;
        }

        private string GetDecimalAmountValue(Decimal? dec)
        {
            if (dec == null)
            {
                return "";
            }
            else
            {
                string s = string.Format("{0:n}", dec.Value);
                if (s.EndsWith(".00"))
                {
                    return s.Substring(0, s.Length - 3);
                }
                else
                {
                    return s;
                }
            }
        }
        #endregion

        #region CMS Functions
        private HttpResponseMessage CreateCMS1500(List<Claim> claims, Batch batch, List<PayTo> payToes, string dbname)
        {
            string fileName = dlPath + "\\CMS1500-" + batch.Identifier + ".pdf";

            // OPTION: Save to File
            FileStream fs = new FileStream(fileName, FileMode.Create);

            // OPTION: Display Inline
            MemoryStream ms = new MemoryStream();
            Document doc = new Document(PageSize.LETTER, 0, 0, 0, 0);
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, fs);

            doc.AddAuthor("Marlon Villarama");
            doc.AddTitle("CMS 1500");
            doc.Open();

            Image img = Image.GetInstance(imgPath + "\\cms-1500.png");
            img.ScaleToFit(doc.PageSize.Width, doc.PageSize.Height);

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            Font font = new Font(bf, 8);

            PdfContentByte pcb = pdfWriter.DirectContent;
            ColumnText ct;

            for (int i = 0, n = claims.Count; i < n; i++)
            {
                doc.Add(new Paragraph(""));
                doc.Add(img);

                try
                {
                    Dictionary<string, string> map = MapClaimToCMS(claims[i], payToes, dbname);
                    foreach (string key in CMSCoordinates.Keys)
                    {
                        ct = new ColumnText(pcb);
                        ct.SetSimpleColumn(CMSCoordinates[key]);
                        if (String.IsNullOrEmpty(map[key]) == false)
                        {
                            Paragraph pg = new Paragraph(map[key].ToUpper(), font);
                            pg.SetLeading(1.0f, 0.0f);
                            ct.AddText(pg);
                        }
                        ct.Go();
                    }

                    if (i < (n - 1))
                    {
                        doc.NewPage();
                    }
                }
                catch (Exception ex)
                {
                    doc.Add(new Paragraph(ex.ToString()));
                }
            }

            // TO-DO: Fix saving of PDF file to /ehr/downloads folder
            doc.Close();
            pdfWriter.Close();
            fs.Close();

            // OPTION: Provide option to both save to /ehr/downloads folder and download PDF file at the same time
            HttpResponseMessage msg = BuildInlineMessage(fileName);//, "application/pdf");
            //HttpResponseMessage msg = new HttpResponseMessage();
            return msg;
        }

        private Dictionary<string, Rectangle> CMSCoordinates
        {
            get
            {
                int ury = 10;
                Dictionary<string, Rectangle> coords = new Dictionary<string, Rectangle>
                {
                    { "payer-name", new Rectangle(385, 770, 590, 10) },
                    { "payer-line1", new Rectangle(385, 750, 590, 10) },
                    { "payer-line2", new Rectangle(385, 740, 590, 10) },

                    { "1-medicare", new Rectangle(20, 695, 30, 10) },
                    { "1-medicaid", new Rectangle(69, 695, 79, 10) },
                    { "1-tricare", new Rectangle(121, 695, 131, 10) },
                    { "1-champva", new Rectangle(187, 695, 197, 10) },
                    { "1-ghi", new Rectangle(239, 695, 249, 10) },
                    { "1-feca", new Rectangle(294, 695, 304, 10) },
                    { "1-other", new Rectangle(341, 695, 351, 10) },
                    { "1-a", new Rectangle(385, 695, 590, 10) },

                    { "2", new Rectangle(25, 671, 220, 10) },
                    { "3-mm", new Rectangle(240, 669, 252, 10) },
                    { "3-dd", new Rectangle(262, 669, 274, 10) },
                    { "3-yy", new Rectangle(285, 669, 310, 10) },
                    { "3-m", new Rectangle(319, 671, 326, 10) },
                    { "3-f", new Rectangle(356, 671, 365, 10) },
                    { "4", new Rectangle(385, 671, 590, 10) },

                    { "5-street", new Rectangle(25, 645, 220, 10) },
                    { "5-city", new Rectangle(25, 622, 194, 10) },
                    { "5-state", new Rectangle(204, 622, 220, 10) },
                    { "5-zip", new Rectangle(25, 595, 105, 10) },
                    { "5-phonearea", new Rectangle(126, 595, 144, 10) },
                    { "5-phonenum", new Rectangle(154, 595, 210, 10) },

                    { "6-self", new Rectangle(253, 647, 262, 10) },
                    { "6-spouse", new Rectangle(290, 647, 300, 10) },
                    { "6-child", new Rectangle(319, 647, 329, 10) },
                    { "6-other", new Rectangle(356, 647, 366, 10) },

                    { "7-street", new Rectangle(385, 645, 590, 10) },
                    { "7-city", new Rectangle(385, 622, 590, 10) },
                    { "7-state", new Rectangle(556, 622, 572, 10) },
                    { "7-zip", new Rectangle(385, 595, 590, 10) },
                    { "7-phonearea", new Rectangle(492, 595, 520, 10) },
                    { "7-phonenum", new Rectangle(522, 595, 580, 10) },

                    { "9", new Rectangle(25, 573, 220, 10) },
                    { "9-a", new Rectangle(25, 549, 220, 10) },
                    { "9-d", new Rectangle(25, 474, 220, 10) },

                    { "10-ayes", new Rectangle(268, 548, 275, 10) },
                    { "10-ano", new Rectangle(312, 548, 320, 10) },
                    { "10-byes", new Rectangle(268, 523, 275, 10) },
                    { "10-bno", new Rectangle(312, 523, 320, 10) },
                    { "10-bstate", new Rectangle(342, 524, 356, 10) },
                    { "10-cyes", new Rectangle(268, 499, 275, 10) },
                    { "10-cno", new Rectangle(312, 499, 320, 10) },

                    { "11", new Rectangle(385, 573, 590, 10) },
                    { "11-amm", new Rectangle(402, 546, 414, 10) },
                    { "11-add", new Rectangle(424, 546, 436, 10) },
                    { "11-ayy", new Rectangle(445, 546, 470, 10) },
                    { "11-am", new Rectangle(510, 548, 518, 10) },
                    { "11-af", new Rectangle(562, 548, 570, 10) },
                    { "11-c", new Rectangle(385, 498, 590, 10) },
                    { "11-dyes", new Rectangle(392, 475, 405, 10) },
                    { "11-dno", new Rectangle(429, 475, 441, 10) },

                    { "12-sig", new Rectangle(70, 428, 400, 10) },
                    { "12-date", new Rectangle(282, 428, 400, 10) },
                    { "13-sig", new Rectangle(432, 428, 590, 10) },

                    { "14-mm", new Rectangle(26, 400, 40, 10) },
                    { "14-dd", new Rectangle(50, 400, 64, 10) },
                    { "14-yy", new Rectangle(77, 400, 91, 10) },
                    { "14-qual", new Rectangle(130, 400, 206, 10) },

                    { "15-qual", new Rectangle(242, 400, 258, 10) },
                    { "15-mm", new Rectangle(282, 400, 296, 10) },
                    { "15-dd", new Rectangle(308, 400, 322, 10) },
                    { "15-yy", new Rectangle(334, 400, 348, 10) },

                    { "16-mmfrom", new Rectangle(408, 400, 422, 10) },
                    { "16-ddfrom", new Rectangle(432, 400, 446, 10) },
                    { "16-yyfrom", new Rectangle(460, 400, 474, 10) },

                    { "16-mmto", new Rectangle(512, 400, 526, 10) },
                    { "16-ddto", new Rectangle(536, 400, 550, 10) },
                    { "16-yyto", new Rectangle(566, 400, 580, 10) },

                    { "17", new Rectangle(44, 376, 206, 10) },
                    { "17-a1", new Rectangle(232, 388, 246, 10) },
                    { "17-a2", new Rectangle(254, 388, 370, 10) },
                    { "17-b", new Rectangle(254, 376, 370, 10) },

                    { "18-mmfrom", new Rectangle(408, 376, 422, 10) },
                    { "18-ddfrom", new Rectangle(432, 376, 446, 10) },
                    { "18-yyfrom", new Rectangle(460, 376, 474, 10) },

                    { "18-mmto", new Rectangle(512, 376, 526, 10) },
                    { "18-ddto", new Rectangle(536, 376, 550, 10) },
                    { "18-yyto", new Rectangle(566, 376, 580, 10) },

                    { "20-yes", new Rectangle(393, 352, 406, 10) },
                    { "20-no", new Rectangle(430, 352, 442, 10) },
                    { "20-ch1", new Rectangle(470, 351, 530, 10) },
                    { "20-ch2", new Rectangle(546, 351, 590, 10) },

                    { "21-a", new Rectangle(40, 327, 100, 10) },
                    { "21-b", new Rectangle(134, 327, 190, 10) },
                    { "21-c", new Rectangle(232, 327, 320, 10) },
                    { "21-d", new Rectangle(328, 327, 410, 10) },

                    { "21-e", new Rectangle(40, 315, 100, 10) },
                    { "21-f", new Rectangle(134, 315, 190, 10) },
                    { "21-g", new Rectangle(232, 315, 320, 10) },
                    { "21-h", new Rectangle(328, 315, 410, 10) },

                    { "21-i", new Rectangle(40, 303, 100, 10) },
                    { "21-j", new Rectangle(134, 303, 190, 10) },
                    { "21-k", new Rectangle(232, 303, 320, 10) },
                    { "21-l", new Rectangle(328, 303, 410, 10) },

                    { "22-code", new Rectangle(385, 327, 460, 10) },
                    { "22-ref", new Rectangle(470, 327, 590, 10) },
                    { "23", new Rectangle(385, 302, 590, 10) },

                    { "24-1-mmfrom", new Rectangle(21, 253, 33, 10) },
                    { "24-1-ddfrom", new Rectangle(42, 253, 53, 10) },
                    { "24-1-yyfrom", new Rectangle(64, 253, 76, 10) },
                    { "24-1-mmto", new Rectangle(86, 253, 100, 10) },
                    { "24-1-ddto", new Rectangle(109, 253, 122, 10) },
                    { "24-1-yyto", new Rectangle(131, 253, 145, 10) },
                    { "24-1-pos", new Rectangle(151, 253, 163, 10) },
                    { "24-1-emg", new Rectangle(175, 253, 187, 10) },
                    { "24-1-cpt", new Rectangle(200, 253, 250, 10) },
                    { "24-1-mod1", new Rectangle(254, 253, 280, 10) },
                    { "24-1-mod2", new Rectangle(277, 253, 302, 10) },
                    { "24-1-mod3", new Rectangle(298, 253, 323, 10) },
                    { "24-1-mod4", new Rectangle(320, 253, 345, 10) },
                    { "24-1-ptr", new Rectangle(345, 253, 380, 10) },
                    { "24-1-ch", new Rectangle(385, 253, 440, 10) },
                    { "24-1-unit", new Rectangle(448, 253, 470, 10) },
                    { "24-1-epsdt", new Rectangle(473, 253, 510, 10) },
                    { "24-1-license", new Rectangle(512, 265, 590, 10) },
                    { "24-1-npi", new Rectangle(512, 253, 590, 10) },

                    { "24-2-mmfrom", new Rectangle(21, 228, 33, 10) },
                    { "24-2-ddfrom", new Rectangle(42, 228, 53, 10) },
                    { "24-2-yyfrom", new Rectangle(64, 228, 76, 10) },
                    { "24-2-mmto", new Rectangle(86, 228, 100, 10) },
                    { "24-2-ddto", new Rectangle(109, 228, 122, 10) },
                    { "24-2-yyto", new Rectangle(131, 228, 145, 10) },
                    { "24-2-pos", new Rectangle(151, 228, 163, 10) },
                    { "24-2-emg", new Rectangle(175, 228, 187, 10) },
                    { "24-2-cpt", new Rectangle(200, 228, 250, 10) },
                    { "24-2-mod1", new Rectangle(254, 228, 280, 10) },
                    { "24-2-mod2", new Rectangle(277, 228, 302, 10) },
                    { "24-2-mod3", new Rectangle(298, 228, 323, 10) },
                    { "24-2-mod4", new Rectangle(320, 228, 345, 10) },
                    { "24-2-ptr", new Rectangle(345, 228, 380, 10) },
                    { "24-2-ch", new Rectangle(385, 228, 440, 10) },
                    { "24-2-unit", new Rectangle(448, 228, 470, 10) },
                    { "24-2-epsdt", new Rectangle(473, 228, 510, 10) },
                    { "24-2-license", new Rectangle(512, 240, 590, 10) },
                    { "24-2-npi", new Rectangle(512, 228, 590, 10) },

                    { "24-3-mmfrom", new Rectangle(21, 204, 33, 10) },
                    { "24-3-ddfrom", new Rectangle(42, 204, 53, 10) },
                    { "24-3-yyfrom", new Rectangle(64, 204, 76, 10) },
                    { "24-3-mmto", new Rectangle(86, 204, 100, 10) },
                    { "24-3-ddto", new Rectangle(109, 204, 122, 10) },
                    { "24-3-yyto", new Rectangle(131, 204, 145, 10) },
                    { "24-3-pos", new Rectangle(151, 204, 163, 10) },
                    { "24-3-emg", new Rectangle(175, 204, 187, 10) },
                    { "24-3-cpt", new Rectangle(200, 204, 250, 10) },
                    { "24-3-mod1", new Rectangle(254, 204, 280, 10) },
                    { "24-3-mod2", new Rectangle(277, 204, 302, 10) },
                    { "24-3-mod3", new Rectangle(298, 204, 323, 10) },
                    { "24-3-mod4", new Rectangle(320, 204, 345, 10) },
                    { "24-3-ptr", new Rectangle(345, 204, 380, 10) },
                    { "24-3-ch", new Rectangle(385, 204, 440, 10) },
                    { "24-3-unit", new Rectangle(448, 204, 470, 10) },
                    { "24-3-epsdt", new Rectangle(473, 204, 510, 10) },
                    { "24-3-license", new Rectangle(512, 216, 590, 10) },
                    { "24-3-npi", new Rectangle(512, 204, 590, 10) },

                    { "24-4-mmfrom", new Rectangle(21, 179, 33, 10) },
                    { "24-4-ddfrom", new Rectangle(42, 179, 53, 10) },
                    { "24-4-yyfrom", new Rectangle(64, 179, 76, 10) },
                    { "24-4-mmto", new Rectangle(86, 179, 100, 10) },
                    { "24-4-ddto", new Rectangle(109, 179, 122, 10) },
                    { "24-4-yyto", new Rectangle(131, 179, 145, 10) },
                    { "24-4-pos", new Rectangle(151, 179, 163, 10) },
                    { "24-4-emg", new Rectangle(175, 179, 187, 10) },
                    { "24-4-cpt", new Rectangle(200, 179, 250, 10) },
                    { "24-4-mod1", new Rectangle(254, 179, 280, 10) },
                    { "24-4-mod2", new Rectangle(277, 179, 302, 10) },
                    { "24-4-mod3", new Rectangle(298, 179, 323, 10) },
                    { "24-4-mod4", new Rectangle(320, 179, 345, 10) },
                    { "24-4-ptr", new Rectangle(345, 179, 380, 10) },
                    { "24-4-ch", new Rectangle(385, 179, 440, 10) },
                    { "24-4-unit", new Rectangle(448, 179, 470, 10) },
                    { "24-4-epsdt", new Rectangle(473, 179, 510, 10) },
                    { "24-4-license", new Rectangle(512, 191, 590, 10) },
                    { "24-4-npi", new Rectangle(512, 179, 590, 10) },

                    { "24-5-mmfrom", new Rectangle(21, 154, 33, 10) },
                    { "24-5-ddfrom", new Rectangle(42, 154, 53, 10) },
                    { "24-5-yyfrom", new Rectangle(64, 154, 76, 10) },
                    { "24-5-mmto", new Rectangle(86, 154, 100, 10) },
                    { "24-5-ddto", new Rectangle(109, 154, 122, 10) },
                    { "24-5-yyto", new Rectangle(131, 154, 145, 10) },
                    { "24-5-pos", new Rectangle(151, 154, 163, 10) },
                    { "24-5-emg", new Rectangle(175, 154, 187, 10) },
                    { "24-5-cpt", new Rectangle(200, 154, 250, 10) },
                    { "24-5-mod1", new Rectangle(254, 154, 280, 10) },
                    { "24-5-mod2", new Rectangle(277, 154, 302, 10) },
                    { "24-5-mod3", new Rectangle(298, 154, 323, 10) },
                    { "24-5-mod4", new Rectangle(320, 154, 345, 10) },
                    { "24-5-ptr", new Rectangle(345, 154, 380, 10) },
                    { "24-5-ch", new Rectangle(385, 154, 440, 10) },
                    { "24-5-unit", new Rectangle(448, 154, 470, 10) },
                    { "24-5-epsdt", new Rectangle(473, 154, 510, 10) },
                    { "24-5-license", new Rectangle(512, 166, 590, 10) },
                    { "24-5-npi", new Rectangle(512, 154, 590, 10) },

                    { "24-6-mmfrom", new Rectangle(21, 130, 33, 10) },
                    { "24-6-ddfrom", new Rectangle(42, 130, 53, 10) },
                    { "24-6-yyfrom", new Rectangle(64, 130, 76, 10) },
                    { "24-6-mmto", new Rectangle(86, 130, 100, 10) },
                    { "24-6-ddto", new Rectangle(109, 130, 122, 10) },
                    { "24-6-yyto", new Rectangle(131, 130, 145, 10) },
                    { "24-6-pos", new Rectangle(151, 130, 163, 10) },
                    { "24-6-emg", new Rectangle(175, 130, 187, 10) },
                    { "24-6-cpt", new Rectangle(200, 130, 250, 10) },
                    { "24-6-mod1", new Rectangle(254, 130, 280, 10) },
                    { "24-6-mod2", new Rectangle(277, 130, 302, 10) },
                    { "24-6-mod3", new Rectangle(298, 130, 323, 10) },
                    { "24-6-mod4", new Rectangle(320, 130, 345, 10) },
                    { "24-6-ptr", new Rectangle(345, 130, 380, 10) },
                    { "24-6-ch", new Rectangle(385, 130, 440, 10) },
                    { "24-6-unit", new Rectangle(448, 130, 470, 10) },
                    { "24-6-epsdt", new Rectangle(473, 130, 510, 10) },
                    { "24-6-license", new Rectangle(512, 142, 590, 10) },
                    { "24-6-npi", new Rectangle(512, 130, 590, 10) },

                    { "25", new Rectangle(25, 106, 160, 10) },
                    { "25-ssn", new Rectangle(137, 108, 149, 10) },
                    { "25-ein", new Rectangle(152, 108, 166, 10) },
                    { "26", new Rectangle(195, 106, 280, 10) },
                    { "27-yes", new Rectangle(291, 108, 303, 10) },
                    { "27-no", new Rectangle(328, 108, 341, 10) },
                    { "28", new Rectangle(390, 106, 455, 10) },
                    { "29", new Rectangle(470, 106, 520, 10) },

                    { "31", new Rectangle(30, 60, 175, 10) },
                    { "31-date", new Rectangle(55, 45, 130, 10) },

                    { "32-name", new Rectangle(190, 82, 370, 10) },
                    { "32-line1", new Rectangle(190, 72, 370, 10) },
                    { "32-line2", new Rectangle(190, 62, 370, 10) },
                    { "32-tax", new Rectangle(287, 45, 380, 10) },
                    { "32-npi", new Rectangle(195, 45, 250, 10) },

                    { "33-phonearea", new Rectangle(498, 94, 515, 10) },
                    { "33-phonenum", new Rectangle(525, 94, 590, 10) },
                    { "33-name", new Rectangle(395, 82, 590, 10) },
                    { "33-line1", new Rectangle(395, 72, 590, 10) },
                    { "33-line2", new Rectangle(395, 62, 590, 10) },
                    { "33-npi", new Rectangle(395, 45, 470, 10) },
                };

                return coords;
            }
        }

        private Dictionary<string, string> GetBlankCMSValues()
        {
            List<string> keys = new List<string>
            {
                "payer-name",
                "payer-line1",
                "payer-line2",

                "1-medicare",
                "1-medicaid",
                "1-tricare",
                "1-champva",
                "1-ghi",
                "1-feca",
                "1-other",
                "1-a",

                "2",
                "3-mm",
                "3-dd",
                "3-yy",
                "3-m",
                "3-f",
                "4",

                "5-street",
                "5-city",
                "5-state",
                "5-zip",
                "5-phonearea",
                "5-phonenum",

                "6-self",
                "6-spouse",
                "6-child",
                "6-other",

                "7-street",
                "7-city",
                "7-state",
                "7-zip",
                "7-phonearea",
                "7-phonenum",

                "9",
                "9-a",
                "9-d",

                "10-ayes",
                "10-ano",
                "10-byes",
                "10-bno",
                "10-bstate",
                "10-cyes",
                "10-cno",

                "11",
                "11-amm",
                "11-add",
                "11-ayy",
                "11-am",
                "11-af",
                "11-c",
                "11-dyes",
                "11-dno",

                "12-sig",
                "12-date",
                "13-sig",

                "14-mm",
                "14-dd",
                "14-yy",
                "14-qual",

                "15-qual",
                "15-mm",
                "15-dd",
                "15-yy",

                "16-mmfrom",
                "16-ddfrom",
                "16-yyfrom",

                "16-mmto",
                "16-ddto",
                "16-yyto",

                "17",
                "17-a1",
                "17-a2",
                "17-b",

                "18-mmfrom",
                "18-ddfrom",
                "18-yyfrom",

                "18-mmto",
                "18-ddto",
                "18-yyto",

                "20-yes",
                "20-no",
                "20-ch1",
                "20-ch2",

                "21-a",
                "21-b",
                "21-c",
                "21-d",
                "21-e",
                "21-f",
                "21-g",
                "21-h",
                "21-i",
                "21-j",
                "21-k",
                "21-l",

                "22-code",
                "22-ref",
                "23",

                "24-1-mmfrom",
                "24-1-ddfrom",
                "24-1-yyfrom",
                "24-1-mmto",
                "24-1-ddto",
                "24-1-yyto",
                "24-1-pos",
                "24-1-emg",
                "24-1-cpt",
                "24-1-mod1",
                "24-1-mod2",
                "24-1-mod3",
                "24-1-mod4",
                "24-1-ptr",
                "24-1-ch",
                "24-1-unit",
                "24-1-epsdt",
                "24-1-license",
                "24-1-npi",

                "24-2-mmfrom",
                "24-2-ddfrom",
                "24-2-yyfrom",
                "24-2-mmto",
                "24-2-ddto",
                "24-2-yyto",
                "24-2-pos",
                "24-2-emg",
                "24-2-cpt",
                "24-2-mod1",
                "24-2-mod2",
                "24-2-mod3",
                "24-2-mod4",
                "24-2-ptr",
                "24-2-ch",
                "24-2-unit",
                "24-2-epsdt",
                "24-2-license",
                "24-2-npi",

                "24-3-mmfrom",
                "24-3-ddfrom",
                "24-3-yyfrom",
                "24-3-mmto",
                "24-3-ddto",
                "24-3-yyto",
                "24-3-pos",
                "24-3-emg",
                "24-3-cpt",
                "24-3-mod1",
                "24-3-mod2",
                "24-3-mod3",
                "24-3-mod4",
                "24-3-ptr",
                "24-3-ch",
                "24-3-unit",
                "24-3-epsdt",
                "24-3-license",
                "24-3-npi",

                "24-4-mmfrom",
                "24-4-ddfrom",
                "24-4-yyfrom",
                "24-4-mmto",
                "24-4-ddto",
                "24-4-yyto",
                "24-4-pos",
                "24-4-emg",
                "24-4-cpt",
                "24-4-mod1",
                "24-4-mod2",
                "24-4-mod3",
                "24-4-mod4",
                "24-4-ptr",
                "24-4-ch",
                "24-4-unit",
                "24-4-epsdt",
                "24-4-license",
                "24-4-npi",

                "24-5-mmfrom",
                "24-5-ddfrom",
                "24-5-yyfrom",
                "24-5-mmto",
                "24-5-ddto",
                "24-5-yyto",
                "24-5-pos",
                "24-5-emg",
                "24-5-cpt",
                "24-5-mod1",
                "24-5-mod2",
                "24-5-mod3",
                "24-5-mod4",
                "24-5-ptr",
                "24-5-ch",
                "24-5-unit",
                "24-5-epsdt",
                "24-5-license",
                "24-5-npi",

                "24-6-mmfrom",
                "24-6-ddfrom",
                "24-6-yyfrom",
                "24-6-mmto",
                "24-6-ddto",
                "24-6-yyto",
                "24-6-pos",
                "24-6-emg",
                "24-6-cpt",
                "24-6-mod1",
                "24-6-mod2",
                "24-6-mod3",
                "24-6-mod4",
                "24-6-ptr",
                "24-6-ch",
                "24-6-unit",
                "24-6-epsdt",
                "24-6-license",
                "24-6-npi",

                "25",
                "25-ssn",
                "25-ein",
                "26",
                "27-yes",
                "27-no",
                "28",
                "29",

                "31",
                "31-date",

                "32-name",
                "32-line1",
                "32-line2",
                "32-tax",
                "32-npi",

                "33-phonearea",
                "33-phonenum",
                "33-name",
                "33-line1",
                "33-line2"
            };

            // Reset everything to blank string
            Dictionary<string, string> map = new Dictionary<string, string>();
            foreach(string key in keys)
            {
                map[key] = String.Empty;
            }

            return map;
        }

        private Dictionary<string, string> MapClaimToCMS(Claim claim, List<PayTo> payToes, string dbname)
        {
            Dictionary<string, string> map = GetBlankCMSValues();
            Subscriber subPrimary = claim.PrimarySubscriber;
            Subscriber subSecondary = claim.SecondarySubscriber;
            Patient patient = claim.Patient;
            DateTime dt;

            map["payer-name"] = claim.PrimaryPayer.Name;
            map["payer-line1"] = claim.PrimaryPayer.Address_1;
            map["payer-line2"] = claim.PrimaryPayer.City + ", " +
                claim.PrimaryPayer.State + " " + claim.PrimaryPayer.Zip;

            map["1-medicare"] = claim.ClaimPayerType.ToUpper() == "MEDICARE" ? "X" : "";
            map["1-medicaid"] = claim.ClaimPayerType.ToUpper() == "MEDICAID" ? "X" : "";
            map["1-tricare"] = claim.ClaimPayerType.ToUpper() == "TRICARE" ? "X" : "";
            map["1-champva"] = claim.ClaimPayerType.ToUpper() == "CHIAMPVA" ? "X" : "";
            map["1-ghi"] = claim.ClaimPayerType.ToUpper() == "GHI" ? "X" : "";
            map["1-feca"] = claim.ClaimPayerType.ToUpper() == "FECA" ? "X" : "";
            map["1-other"] = claim.ClaimPayerType.ToUpper() == "OTHER" ? "X": "";
            map["1-a"] = claim.PrimaryPayerMemberID;

            map["2"] = patient == null ?
                subPrimary.LastName + ", " + subPrimary.FirstName :
                patient.LastName + ", " + patient.FirstName;
            dt = patient == null ? GetDateTimeValue(subPrimary.DateOfBirth) :
                GetDateTimeValue(patient.DateOfBirth);
            map["3-mm"] = dt == null ? "" : dt.Month.ToString().PadLeft(2, '0');
            map["3-dd"] = dt == null ? "" : dt.Day.ToString().PadLeft(2, '0');
            map["3-yy"] = dt == null ? "" : dt.Year.ToString();
            map["3-m"] = patient == null ?
                (subPrimary.Gender == "M" == true ? "X" : "") :
                (patient.Gender == "M" == true ? "X" : "");
            map["3-f"] = patient == null ?
                (subPrimary.Gender == "M" == true ? "" : "X") :
                (patient.Gender == "M" == true ? "" : "X");
            map["4"] = subPrimary.LastName + ", " + subPrimary.FirstName;

            // Patient's Address
            map["5-street"] = patient == null ?
                subPrimary.Address_1 : patient.Address_1;
            map["5-city"] = patient == null ?
                subPrimary.City : patient.City;
            map["5-state"] = patient == null ?
                subPrimary.State : patient.State;
            map["5-zip"] = patient == null ?
                subPrimary.Zip : patient.Zip;
            map["5-phonearea"] = patient == null ?
                (String.IsNullOrEmpty(subPrimary.Phone_1) == true ? "" : subPrimary.Phone_1.Substring(0, 3)) :
                (String.IsNullOrEmpty(patient.Phone) == true ? "" : patient.Phone.Substring(0, 3));
            map["5-phonenum"] = patient == null ?
                (String.IsNullOrEmpty(subPrimary.Phone_1) == true ? "" : subPrimary.Phone_1.Substring(3)) :
                (String.IsNullOrEmpty(patient.Phone) == true ? "" : patient.Phone.Substring(3));

            // Patient Relationship
            map["6-self"] = claim.Relationship == "18" ? "X" : "";
            map["6-spouse"] = claim.Relationship == "01" ? "X" : "";
            map["6-child"] = claim.Relationship == "19" ? "X" : "";
            map["6-other"] =
                (claim.Relationship != "18" &&
                claim.Relationship != "19" &&
                claim.Relationship != "01") == true ? "X" : "";

            // Insured Address
            map["7-street"] = subPrimary.Address_1;
            map["7-city"] = subPrimary.City;
            map["7-state"] = subPrimary.State;
            map["7-zip"] = subPrimary.Zip;
            map["7-phonearea"] = 
                String.IsNullOrEmpty(subPrimary.Phone_1) == true ? "" : subPrimary.Phone_1.Substring(0, 3);
            map["7-phonenum"] =
                String.IsNullOrEmpty(subPrimary.Phone_1) == true ? "" : subPrimary.Phone_1.Substring(3);

            if (subSecondary != null)
            {
                map["9"] = subSecondary.LastName + ", " + subSecondary.FirstName;
                map["9-a"] = claim.SecondaryPayerMemberID;
                map["9-d"] = claim.SecondaryPayer.Name;
            }

            map["10-ayes"] = claim.EmploymentRelated == true ? "X" : "";
            map["10-ano"] = claim.EmploymentRelated == true ? "" : "X";

            if (claim.Accident == null)
            {
                map["10-bno"] = "X";
                map["10-cno"] = "X";
            }
            else
            {
                map["10-byes"] = String.IsNullOrEmpty(claim.Accident.Causes) == true ? "" : "X";
                map["10-bstate"] = String.IsNullOrEmpty(claim.Accident.Causes) == true ? claim.Accident.State : "";
                map["10-cyes"] = String.IsNullOrEmpty(claim.Accident.Causes) == true ? "" : "X";
            }

            dt = GetDateTimeValue(subPrimary.DateOfBirth);
            map["11-amm"] = dt.Month.ToString().PadLeft(2, '0');
            map["11-add"] = dt.Day.ToString().PadLeft(2, '0');
            map["11-ayy"] = dt.Year.ToString();
            map["11-am"] = (subPrimary.Gender == "M" == true ? "X" : "");
            map["11-af"] = (subPrimary.Gender == "M" == true ? "" : "X");
            map["11-c"] = String.Empty;
            map["11-dyes"] = subSecondary == null ? "" : "X";
            map["11-dno"] = subSecondary == null ? "X" : "";

            // SIGNATURES
            map["12-sig"] = "SIGNATURE ON FILE";
            dt = GetDateTimeValue(claim.DateCreated);
            map["12-date"] = dt.ToString("MM/dd/yyyy");
            map["13-sig"] = "SIGNATURE ON FILE";

            // Dates
            if (claim.Dates != null)
            {
                dt = GetDateTimeValue(claim.Dates.OnsetOfCurrent);
                if (dt != null)
                {
                    map["14-mm"] = dt.Month.ToString().PadLeft(2, '0');
                    map["14-dd"] = dt.Day.ToString().PadLeft(2, '0');
                    map["14-yy"] = dt.Year.ToString();
                }

                dt = GetDateTimeValue(claim.Dates.Other);
                if (dt != null)
                {
                    map["15-mm"] = dt.Month.ToString().PadLeft(2, '0');
                    map["15-dd"] = dt.Day.ToString().PadLeft(2, '0');
                    map["15-yy"] = dt.Year.ToString();
                }

                dt = GetDateTimeValue(claim.Dates.LastWorked);
                if (dt != null)
                {
                    map["16-mmfrom"] = dt.Month.ToString().PadLeft(2, '0');
                    map["16-ddfrom"] = dt.Day.ToString().PadLeft(2, '0');
                    map["16-yyfrom"] = dt.Year.ToString();
                }

                dt = GetDateTimeValue(claim.Dates.ReturnToWork);
                if (dt != null)
                {
                    map["16-mmto"] = dt.Month.ToString().PadLeft(2, '0');
                    map["16-ddto"] = dt.Day.ToString().PadLeft(2, '0');
                    map["16-yyto"] = dt.Year.ToString();
                }

                // #17 REFERRING PROVIDER - NOT YET IMPLEMENTED

                dt = GetDateTimeValue(claim.Dates.Admission);
                if (dt != null)
                {
                    map["18-mmfrom"] = dt.Month.ToString().PadLeft(2, '0');
                    map["18-ddfrom"] = dt.Day.ToString().PadLeft(2, '0');
                    map["18-yyfrom"] = dt.Year.ToString();
                }

                dt = GetDateTimeValue(claim.Dates.Discharge);
                if (dt != null)
                {
                    map["18-mmto"] = dt.Month.ToString().PadLeft(2, '0');
                    map["18-ddto"] = dt.Day.ToString().PadLeft(2, '0');
                    map["18-yyto"] = dt.Year.ToString();
                }
            }

            map["20-yes"] = claim.OutsideLab == true ? "X" : "";

            if (claim.OutsideLab == true)
            {
                map["20-yes"] = claim.OutsideLab == true ? "X" : "";
                map["20-ch1"] = claim.OutsideLab == true ? GetDecimalAmountValue(claim.OutsideLabCharges) : "";
            }
            else
            {
                map["20-no"] = "X";
            }

            string[] icds = claim.DiagnosisCodes.Split(new char[] { ',' });
            string chars = "abcdefghijkl";
            for (int i = 0, n = icds.Length; i < n; i++)
            {
                map["21-" + chars.Substring(i, 1)] = icds[i];
            }

            //{ "22-code", "23456" }, - Resubmission Code
            //{ "22-ref", "23456" }, - Original Ref No
            //{ "23", "23456" }, - Prior Authorization
            string pre = "";
            for (int i = 1, n = claim.ClaimLines.Count; i <= n; i++)
            {
                ClaimLine line = claim.ClaimLines.Where(x => x.OrderLine == i).ToList()[0];
                pre = "24-" + i.ToString() + "-";

                dt = GetDateTimeValue(claim.DateOfService);
                map[pre + "mmfrom"] = dt.Month.ToString().PadLeft(2, '0');
                map[pre + "ddfrom"] = dt.Day.ToString().PadLeft(2, '0');
                map[pre + "yyfrom"] = dt.Year.ToString().Substring(2);

                dt = GetDateTimeValue(claim.DateOfService);
                map[pre + "mmto"] = dt.Month.ToString().PadLeft(2, '0');
                map[pre + "ddto"] = dt.Day.ToString().PadLeft(2, '0');
                map[pre + "yyto"] = dt.Year.ToString().Substring(2);

                map[pre + "pos"] = claim.PlaceOfService.Code;
                map[pre + "emg"] = "";
                map[pre + "cpt"] = line.CPT;

                if (String.IsNullOrEmpty(line.Modifier) == false)
                {
                    string[] mods = line.Modifier.Split(new char[] { ',' });
                    int modctr = 0;
                    for (int j = 0, o = mods.Length; j < o; j++)
                    {
                        if (String.IsNullOrEmpty(mods[j]) == false)
                        {
                            map[pre + "mod" + (++modctr).ToString()] = mods[j];
                        }
                        else
                        {
                            map[pre + "mod" + (++modctr).ToString()] = "";
                        }
                    }
                }

                map[pre + "ptr"] = line.Pointer;
                map[pre + "ch"] = GetDecimalAmountValue(line.Amount);
                map[pre + "unit"] = line.Unit;
                map[pre + "epsdt"] = line.EPSDT == true ? "X" : "";
                map[pre + "license"] = claim.RenderingProvider.License;
                map[pre + "npi"] = claim.RenderingProvider.NPI;
            }

            map["25"] = String.IsNullOrEmpty(claim.RenderingProvider.EIN) == true ?
                claim.RenderingProvider.SSN : claim.RenderingProvider.EIN;
            map["25-ssn"] = String.IsNullOrEmpty(claim.RenderingProvider.SSN) == true ? "" : "X";
            map["25-ein"] = String.IsNullOrEmpty(claim.RenderingProvider.EIN) == true ? "" : "X";

            map["27-yes"] = claim.AcceptAssignment != "C" ? "X" : "";
            map["27-no"] = claim.AcceptAssignment == "C" ? "X" : "";
            map["28"] = GetDecimalAmountValue(claim.AmountTotal);
            map["29"] = GetDecimalAmountValue(claim.AmountCopay);

            // Provider Signature
            map["31"] = claim.RenderingProvider.LastName + ", " + claim.RenderingProvider.FirstName + ", " +
                claim.RenderingProvider.Credential;
            dt = GetDateTimeValue(claim.DateCreated);
            if (dt != null)
            {
                map["31-date"] = dt.ToString("MM/dd/yyyy");
            }

            // Facility
            map["32-name"] = claim.Facility.Name;
            map["32-line1"] = claim.Facility.Address_1;
            map["32-line2"] = claim.Facility.City + ", " + claim.Facility.State + " " + claim.Facility.Zip;
            map["32-npi"] = claim.Facility.NPI;
            map["32-tax"] = claim.Facility.TaxId;

            // Billing Provider
            if (String.IsNullOrEmpty(claim.BillingProvider.Phone_1) == false)
            {
                map["33-phonearea"] = claim.BillingProvider.Phone_1.Substring(0, 3);
                map["33-phonenum"] = claim.BillingProvider.Phone_1.Substring(3, 3) +
                    "-" + claim.BillingProvider.Phone_1.Substring(6);
            }

            //EHRDB db = new EHRDB();
            //db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);
            PayTo payTo = payToes.Where(x => x.BillingProviderId == claim.BillingProvider.Id &&
                                            x.RenderingProviderId == claim.RenderingProvider.Id).First();

            map["33-name"] = claim.BillingProvider.LastName;
            map["33-line1"] = payTo.Address_1;
            map["33-line2"] = payTo.City + ", " + payTo.State + " " + payTo.Zip;
            map["33-npi"] = claim.BillingProvider.NPI;

            return map;
        }
        #endregion

        #region X837 Functions
        private HttpResponseMessage CreateX837(List<Claim> claims, Batch batch, List<PayTo> payToes, string dbName)
        {
            string fileName = dlPath + "\\X837-" + batch.Identifier + ".txt";

            //X837 x837 = new X837(claims, batch, dbName);
            X837 x837 = new X837(dbName)
            {
                Claims = claims,
                Batch = batch,
                PayToes = payToes
            };

            string output;
            try
            {
                output = x837.WriteX837(null, true); // true -> new line separator per segment
            }
            catch(Exception ex)
            {
                output = ex.ToString();
            }


            FileStream fs = new FileStream(fileName, FileMode.Create);
            MemoryStream ms = new MemoryStream();

            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(output);
            sw.Flush();
            sw.Close();
            fs.Close();

            HttpResponseMessage msg = BuildInlineMessage(fileName);

            return msg;
        }
        #endregion

        private List<Claim> GetValidClaims(EHRDB db, string idString)
        {
            List<Claim> claims = new List<Claim>();
            int id;
            Claim c;

            foreach (string s in idString.Split(new[] { ',' }))
            {
                if (String.IsNullOrEmpty(s) == true)
                {
                    continue;
                }

                try
                {
                    id = int.Parse(s);
                    c = db.Claims.Find(id);
                    if (c != null)
                    {
                        claims.Add(c);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return claims;
        }

        private Batch CreateBatch(EHRDB db, List<Claim> claims, int createdBy, bool create = true)
        {
            List<int> claimIDs = claims.Select(x => x.Id).ToList();
            Batch batch = new Batch();
            batch.Identifier = DateTime.Now.Ticks.ToString();
            batch.ClaimIDs = String.Join(",", claimIDs.ToArray());
            batch.CreatedById = createdBy;
            batch.DateCreated = DateTime.Now;

            if (create == true)
            {
                db.Batches.Add(batch);
                db.SaveChanges();
            }

            return batch;
        }

        private HttpResponseMessage BuildInlineMessage(string fileName)//, string contentType)
        {
            HttpResponseMessage msg = Request.CreateResponse(HttpStatusCode.BadRequest);

            var dataBytes = File.ReadAllBytes(fileName);
            MemoryStream ms = new MemoryStream(dataBytes);

            byte[] buffer = ms.ToArray();
            var contentLength = buffer.Length;

            var statusCode = HttpStatusCode.OK;
            msg = Request.CreateResponse(statusCode);
            msg.Content = new StreamContent(new MemoryStream(buffer));
            msg.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            msg.Content.Headers.ContentDisposition.FileName = Path.GetFileName(fileName);
            msg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return msg;
        }

        private ApiReportParams GetApiParams(string parms)
        {
            ApiReportParams apiParams = new ApiReportParams();

            List<string> sParms = parms.Split(new[] { '|' }).ToList();
            if (sParms.Count <= 0)
            {
                return null;
            }

            if (String.IsNullOrEmpty(sParms[0]) == false)
            {
                apiParams.Type = sParms[0];
            }

            if (String.IsNullOrEmpty(sParms[1]) == false)
            {
                int id = 0;
                try
                {
                    id = int.Parse(sParms[1]);
                    if (id > 0)
                    {
                        apiParams.BatchId = int.Parse(sParms[1]);
                    }
                }
                catch
                {
                    apiParams.BatchId = -1;
                }
            }

            if (sParms.Count > 2)
            {
                if (String.IsNullOrEmpty(sParms[2]) == false)
                {
                    apiParams.ClaimIds = sParms[2];
                }
            }

            if (sParms.Count > 3)
            {
                if (String.IsNullOrEmpty(sParms[3]) == false)
                {
                    apiParams.ClaimIds = sParms[3];
                }
            }

            if (sParms.Count > 4)
            {
                if (String.IsNullOrEmpty(sParms[4]) == false)
                {
                    apiParams.CreateBatch = sParms[4].ToUpper() == "Y";
                }
            }

            return apiParams;
        }

        // REPORTS ENTRY POINT
        [HttpGet]
        [Route("api/report/{dbname}/{parms}")]
        public HttpResponseMessage GetReport(string dbname, string parms)
        {
            ApiReportParams apiParms = GetApiParams(parms);

            HttpResponseMessage msg = new HttpResponseMessage();
            EHRDB db = new EHRDB();
            db.Database.Connection.ConnectionString = db.Database.Connection.ConnectionString.Replace("HK_MASTER", dbname);

            Batch batch = null;
            List<Claim> claims = new List<Claim>();


            if (apiParms.BatchId > 0)
            {
                batch = db.Batches.Find(apiParms.BatchId);
                if (batch != null)
                {
                    claims = GetValidClaims(db, batch.ClaimIDs);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(apiParms.ClaimIds) == false)
                {
                    claims = GetValidClaims(db, apiParms.ClaimIds);
                    batch = CreateBatch(db, claims, apiParms.CreatedById, false);
                }
            }

            if (claims.Count <= 0)
            {
                return msg;
            }

            switch (apiParms.Type.ToUpper())
            {
                case "CMS":
                    msg = CreateCMS1500(claims, batch, db.PayToes.ToList(), dbname);
                    break;
                case "X837":
                    msg = CreateX837(claims, batch, db.PayToes.ToList(), dbname);
                    break;
            }

            return msg;
        }
    }

    public class ApiReportParams
    {
        public ApiReportParams()
        {
            Type = ClaimIds = String.Empty;
            BatchId = -1;
            CreateBatch = false;
            CreatedById = 0;
        }

        public string Type { get; set; }
        public int BatchId { get; set; }
        public string ClaimIds { get; set; }
        public bool CreateBatch { get; set; }
        public int CreatedById { get; set; }
    }
}
