using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using System.Web.Configuration;
using ProjectManagementTool.DAL;
using ProjectManagementTool.Models;

namespace ProjectManager._content_pages.report_document_summary
{
    public partial class Default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    DocumentSummaryReportName.InnerHtml = "Projectwise Document Summary Report for water projects as on " + DateTime.Now.ToString("dd/MM/yyyy");
                    BindView();
                }
            }
        }

        protected void BindView()
        {
            List<tClass2> SummaryList = new List<tClass2>();

            SummaryList.Add(new tClass2 { Project = "CP-02", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "64954970-99dd-4d6b-b379-17df270300ba" });

            SummaryList.Add(new tClass2 { Project = "CP-03", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "d7646a77-98f2-4316-9ecc-59abac159381" });

            SummaryList.Add(new tClass2 { Project = "CP-04", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "5a111cb1-0960-4274-b35e-57a6f5149595" });

            SummaryList.Add(new tClass2 { Project = "CP-07", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "f98ea953-db54-4e16-93a3-09ba0a98d5b1" });

            SummaryList.Add(new tClass2 { Project = "CP-08", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "f98ea953-db54-4e16-93a3-09ba0a98d5b1" });

            SummaryList.Add(new tClass2 { Project = "CP-09", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "2f66efc0-ff27-4cf6-a041-b9b2e05b9217" });

            SummaryList.Add(new tClass2 { Project = "CP-10", Total = 0, Pending = 0, ActionTaken = 0, ProjectId = "7b0c39ce-f72c-4064-a879-609671f5ba27" });

            SummaryList.Add(new tClass2 { Project = "CP-12", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "3810dd45-a4d3-47d6-b85a-f094a0c5b37a" });

            SummaryList.Add(new tClass2 { Project = "CP-13", Total = 0, Pending = 0, ActionTaken = 0, ProjectId= "8a8cbabf-fed7-4def-aba4-c97822bff3f9" });


            DataSet ds = null;
            

            foreach (var item in SummaryList)
            {
                ds = getdt.GetFlow2OlddocsCount(item.Project.ToString(), "Pending Documents");
                if (ds != null)  item.Pending = Convert.ToInt32( ds.Tables[0].Rows[0].ItemArray[0].ToString());

                ds = getdt.GetFlow2OlddocsCount(item.Project.ToString(), "Action Taken Documents");
                if (ds != null) item.ActionTaken = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0].ToString());

                item.Total = item.Pending + item.ActionTaken;
            }

            GrdDocumentSummary.DataSource = SummaryList;

            GrdDocumentSummary.DataBind();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportGridToExcel();
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            ExporttoPDF(GrdDocumentSummary,0,"No");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        //private void ExportGridToExcel()
        //{
           
        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    Response.Charset = "";
        //    string FileName = "Report__document_summary" + DateTime.Now.Ticks + ".xls";

        //    StringWriter strwritter = new StringWriter();
        //    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.ContentType = "application/vnd.ms-excel";
        //    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        //    GrdDocumentSummary.GridLines = GridLines.Both;
        //    GrdDocumentSummary.HeaderStyle.Font.Bold = true;
        //    GrdDocumentSummary.RenderControl(htmltextwrtter);

        //    string s = htmltextwrtter.InnerWriter.ToString();

        //    string x = ""; // WebConfigurationManager.AppSettings["Domain"];
        //    string y = ""; //Session["Username"].ToString();
        //    string z = ""; //Request.QueryString["ProjectName"];

        //    string HTMLstring = "<html><body>" +
        //            "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
        //            "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + x + " Projectwise Document Summary Report for water projects as on date " + y + " </asp:Label><br />" +
        //            "<asp:Label ID='Lbl4' runat='server'>" + z + "</asp:Label><br />" +
        //            "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
        //            "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
        //            s +
        //            "</div>" +
        //            "</div></body></html>";
        //    Response.Write(HTMLstring);
        //    Response.End();

        //}

        //private void ExportGridToPDF()
        //{
        //    GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        //    TableHeaderCell cell = new TableHeaderCell();
        //    cell.Text = "Projectwise Document Summary Report for water projects as on date.";
        //    cell.ColumnSpan = 4;
        //    row.Controls.Add(cell);

        //    row.BackColor = ColorTranslator.FromHtml("#D3D3D3");
        //    GrdDocumentSummary.HeaderRow.Parent.Controls.AddAt(0, row);


        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("Content-Disposition", "attachment;filename=document_summary.pdf");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);
        //    GrdDocumentSummary.RenderControl(hw);

        //    StringReader sr = new StringReader(sw.ToString());
        //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //    pdfDoc.Open();
        //    htmlparser.Parse(sr);
        //    pdfDoc.Close();
        //    Response.Write(pdfDoc);
        //    Response.End();
        //}

        //private void ExporttoPDF(GridView gd, int type, string isPrint)
        //{
        //    try
        //    {

        //        DateTime CDate1 = DateTime.Now;
        //        GridView gdRp = new GridView();
        //        gdRp = gd;

        //        int noOfColumns = 0, noOfRows = 0;
        //        DataTable tbl = null;

        //        if (gdRp.AutoGenerateColumns)
        //        {
        //            tbl = gdRp.DataSource as DataTable; // Gets the DataSource of the GridView Control.
        //            noOfColumns = tbl.Columns.Count;
        //            noOfRows = tbl.Rows.Count;
        //        }
        //        else
        //        {
        //            noOfColumns = gdRp.Columns.Count;
        //            noOfRows = gdRp.Rows.Count;
        //        }

        //        float HeaderTextSize = 9;
        //        float ReportNameSize = 9;
        //        float ReportTextSize = 9;
        //        float ApplicationNameSize = 13;
        //        string ProjectName = ""; // DDlProject.SelectedItem.ToString();

        //        //if (ProjectName.Length > 30)
        //        //{
        //        //    ProjectName = ProjectName.Substring(0, 29) + "..";
        //        //}

        //        // Creates a PDF document

        //        Document document = null;
        //        //if (LandScape == true)
        //        //{
        //        // Sets the document to A4 size and rotates it so that the     orientation of the page is Landscape.
        //        document = new Document(PageSize.A4.Rotate(), 0, 0, 0, 0);
        //        //}
        //        //else
        //        //{
        //        //    document = new Document(PageSize.A4, 0, 0, 15, 5);
        //        //}

        //        // Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
        //        iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);

        //        // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
        //        mainTable.HeaderRows = 4;

        //        // Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
        //        iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(1);

        //        // Creates a phrase to hold the application name at the left hand side of the header.
        //        Phrase phApplicationName = new Phrase(Environment.NewLine + "Projectwise Document Summary Report for water projects as on " + DateTime.Today.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));

        //        mainTable.SetWidths(new float[] { 8, 30, 30, 30 });

        //        // Creates a PdfPCell which accepts a phrase as a parameter.
        //        PdfPCell clApplicationName = new PdfPCell(phApplicationName);
        //        // Sets the border of the cell to zero.
        //        clApplicationName.Border = PdfPCell.NO_BORDER;
        //        // Sets the Horizontal Alignment of the PdfPCell to left.
        //        clApplicationName.HorizontalAlignment = Element.ALIGN_CENTER;

        //        // Creates a phrase to show the current date at the right hand side of the header.
        //        //Phrase phDate = new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

        //        //// Creates a PdfPCell which accepts the date phrase as a parameter.
        //        //PdfPCell clDate = new PdfPCell(phDate);
        //        //// Sets the Horizontal Alignment of the PdfPCell to right.
        //        //clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        //// Sets the border of the cell to zero.
        //        //clDate.Border = PdfPCell.NO_BORDER;

        //        // Adds the cell which holds the application name to the headerTable.
        //        headerTable.AddCell(clApplicationName);
        //        // Adds the cell which holds the date to the headerTable.
        //        //  headerTable.AddCell(clDate);
        //        // Sets the border of the headerTable to zero.
        //        headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

        //        // Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
        //        PdfPCell cellHeader = new PdfPCell(headerTable);
        //        cellHeader.Border = PdfPCell.NO_BORDER;
        //        // Sets the column span of the header cell to noOfColumns.
        //        cellHeader.Colspan = noOfColumns;
        //        // Adds the above header cell to the table.
        //        mainTable.AddCell(cellHeader);

        //        // Creates a phrase which holds the file name.
        //        Phrase phHeader = new Phrase("" + ProjectName); //+ " (" + DDLWorkPackage.SelectedItem.Text + ")");
        //        PdfPCell clHeader = new PdfPCell(phHeader);
        //        clHeader.Colspan = noOfColumns;
        //        clHeader.Border = PdfPCell.NO_BORDER;
        //        clHeader.HorizontalAlignment = Element.ALIGN_CENTER;
        //        mainTable.AddCell(clHeader);



        //        // Creates a phrase for a new line.
        //        Phrase phSpace = new Phrase("\n");
        //        PdfPCell clSpace = new PdfPCell(phSpace);
        //        clSpace.Border = PdfPCell.NO_BORDER;
        //        clSpace.Colspan = noOfColumns;
        //        mainTable.AddCell(clSpace);

        //        // Sets the gridview column names as table headers.
        //        for (int i = 0; i < noOfColumns; i++)
        //        {
        //            Phrase ph = null;

        //            if (gdRp.AutoGenerateColumns)
        //            {
        //                ph = new Phrase(tbl.Columns[i].ColumnName.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
        //            }
        //            else
        //            {
        //                ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
        //            }
        //            PdfPCell cl = new PdfPCell(ph);
        //            if (i == 1)
        //            {
        //                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                mainTable.AddCell(cl);
        //            }
        //            else if (i == 6)
        //            {
        //                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                mainTable.AddCell(cl);
        //            }
        //            else
        //            {
        //                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                mainTable.AddCell(cl);
        //            }

        //        }

        //        // Reads the gridview rows and adds them to the mainTable
        //        for (int rowNo = 0; rowNo <= noOfRows; rowNo++)
        //        {
        //            if (rowNo != noOfRows)
        //            {
        //                for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
        //                {
        //                    if (gdRp.AutoGenerateColumns)
        //                    {
        //                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
        //                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                        PdfPCell cl = new PdfPCell(ph);
        //                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        mainTable.AddCell(cl);
        //                    }
        //                    else
        //                    {
        //                        if (columnNo == 1)
        //                        {
        //                            if (gdRp.Columns[columnNo] is TemplateField)
        //                            {
        //                                DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
        //                                string s = lc.Text.Trim();
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                            else
        //                            {
        //                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
        //                                s = s.Replace("&nbsp;", "");
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                        }
        //                        else if (columnNo == 2)
        //                        {
        //                            if (gdRp.Columns[columnNo] is TemplateField)
        //                            {
        //                                DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
        //                                string s = lc.Text.Trim();
        //                                s = RemoveHtml(s);
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                            else
        //                            {
        //                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
        //                                s = s.Replace("&nbsp;", "");
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                        }
        //                        else if (columnNo == 3)
        //                        {
        //                            if (gdRp.Columns[columnNo] is TemplateField)
        //                            {
        //                                DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
        //                                string s = lc.Text.Trim();
        //                                s = RemoveHtml(s);
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                            else
        //                            {
        //                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
        //                                s = s.Replace("&nbsp;", "");
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (gdRp.Columns[columnNo] is TemplateField)
        //                            {
        //                                DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
        //                                string s = lc.Text.Trim();
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                            else
        //                            {
        //                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
        //                                s = s.Replace("&nbsp;", "");
        //                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
        //                                PdfPCell cl = new PdfPCell(ph);
        //                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                mainTable.AddCell(cl);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (type == 1)
        //                {
        //                    for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
        //                    {
        //                        string s = "Grand Total";
        //                        if (columnNo == 1)
        //                        {
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else if (columnNo == 2)
        //                        {
        //                            s = ViewState["grandtotalcount"].ToString();// Session["AwardedValue"].ToString();
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else if (columnNo == 3)
        //                        {
        //                            s = ViewState["grandtotaldwnldcount"].ToString();// Session["ExpenditureOverallTotal"].ToString();
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else if (columnNo == 4)
        //                        {
        //                            s = ViewState["grandtotalviewcount"].ToString();// Session["TargetedOverallTotal"].ToString() + "%";
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else if (columnNo == 5)
        //                        {
        //                            s = ViewState["GrandTotalDocumentLinkSent"].ToString();// Session["AchievedOverallTotal"].ToString() + "%";
        //                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                        else
        //                        {
        //                            Phrase ph = new Phrase("", FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
        //                            PdfPCell cl = new PdfPCell(ph);
        //                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
        //                            mainTable.AddCell(cl);
        //                        }
        //                    }
        //                }

        //            }

        //            // Tells the mainTable to complete the row even if any cell is left incomplete.
        //            mainTable.CompleteRow();
        //        }

        //        // Gets the instance of the document created and writes it to the output stream of the Response object.
        //        // Gets the instance of the document created and writes it to the output stream of the Response object.
        //        if (isPrint == "Yes")
        //        {
        //            PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "/RegExcel/printpdf.pdf", FileMode.Create));
        //        }
        //        else
        //        {
        //            PdfWriter.GetInstance(document, Response.OutputStream);
        //        }
        //        //PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "mypdf.pdf", FileMode.Create));
        //        // Creates a footer for the PDF document.
        //        int len = 174;
        //        System.Text.StringBuilder time = new System.Text.StringBuilder();
        //        time.Append(DateTime.Now.ToString("hh:mm tt"));
        //        time.Append("".PadLeft(len, ' ').Replace(" ", " "));

        //        iTextSharp.text.Font foot = new iTextSharp.text.Font();
        //        foot.Size = 10;
        //        HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time " + time + "  Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
        //        pdfFooter.Alignment = Element.ALIGN_CENTER;
        //        document.Footer = pdfFooter;
        //        document.Open();
        //        // Adds the mainTable to the document.
        //        document.Add(mainTable);
        //        // Closes the document.
        //        document.Close();
        //        if (isPrint == "Yes")
        //        {
        //            Session["Print"] = true;
        //            Response.Write("<script>window.open ('/PDFPRint.aspx','_blank');</script>");
        //        }
        //        else
        //        {
        //            Response.ContentType = "application/pdf";
        //            Response.AddHeader("content-disposition", "attachment;filename=Report_Issues_" + DateTime.Now.Ticks + ".pdf");
        //            Response.End();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private string RemoveHtml(string htmlString)
        {
            string innerText = "";
            Boolean flag = false;

            foreach (char c in htmlString)
            {

                if (flag)
                {
                    if (c != '<')
                        innerText = innerText + c;
                    else
                        flag = false;
                }

                if (c == '>') flag = true;


            }

            return innerText;
        }

        private void ExporttoPDF(GridView gd, int type, string isPrint)
        {
            try
            {

                DateTime CDate1 = DateTime.Now;
                GridView gdRp = new GridView();
                gdRp = gd;

                int noOfColumns = 0, noOfRows = 0;
                DataTable tbl = null;

                if (gdRp.AutoGenerateColumns)
                {
                    tbl = gdRp.DataSource as DataTable; // Gets the DataSource of the GridView Control.
                    noOfColumns = tbl.Columns.Count;
                    noOfRows = tbl.Rows.Count;
                }
                else
                {
                    noOfColumns = gdRp.Columns.Count;
                    noOfRows = gdRp.Rows.Count;
                }

                float HeaderTextSize = 9;
                float ReportNameSize = 9;
                float ReportTextSize = 9;
                float ApplicationNameSize = 13;
                string ProjectName = ""; // DDlProject.SelectedItem.ToString();

                //if (ProjectName.Length > 30)
                //{
                //    ProjectName = ProjectName.Substring(0, 29) + "..";
                //}

                // Creates a PDF document

                Document document = null;
                //if (LandScape == true)
                //{
                // Sets the document to A4 size and rotates it so that the     orientation of the page is Landscape.
                document = new Document(PageSize.A4.Rotate(), 0, 0, 0, 0);
                //}
                //else
                //{
                //    document = new Document(PageSize.A4, 0, 0, 15, 5);
                //}

                // Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
                iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);

                // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
                mainTable.HeaderRows = 4;

                // Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
                iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(1);

                // Creates a phrase to hold the application name at the left hand side of the header.
                Phrase phApplicationName = new Phrase(Environment.NewLine + "ONTB-BWSSB Stage 5 Project Monitoring Tool", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));

                mainTable.SetWidths(new float[] { 8, 30, 30, 30 });

                // Creates a PdfPCell which accepts a phrase as a parameter.
                PdfPCell clApplicationName = new PdfPCell(phApplicationName);
                // Sets the border of the cell to zero.
                clApplicationName.Border = PdfPCell.NO_BORDER;
                // Sets the Horizontal Alignment of the PdfPCell to left.
                clApplicationName.HorizontalAlignment = Element.ALIGN_CENTER;

                // Creates a phrase to show the current date at the right hand side of the header.
                //Phrase phDate = new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

                //// Creates a PdfPCell which accepts the date phrase as a parameter.
                //PdfPCell clDate = new PdfPCell(phDate);
                //// Sets the Horizontal Alignment of the PdfPCell to right.
                //clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                //// Sets the border of the cell to zero.
                //clDate.Border = PdfPCell.NO_BORDER;

                // Adds the cell which holds the application name to the headerTable.
                headerTable.AddCell(clApplicationName);
                // Adds the cell which holds the date to the headerTable.
                //  headerTable.AddCell(clDate);
                // Sets the border of the headerTable to zero.
                headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                // Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
                PdfPCell cellHeader = new PdfPCell(headerTable);
                cellHeader.Border = PdfPCell.NO_BORDER;
                // Sets the column span of the header cell to noOfColumns.
                cellHeader.Colspan = noOfColumns;
                // Adds the above header cell to the table.
                mainTable.AddCell(cellHeader);

                //   Creates a phrase which holds the file name.
                Phrase phHeader = new Phrase("Projectwise Document Summary Report for water projects as on " + DateTime.Today.ToString("dd/MM/yyyy")); //+ " (" + DDLWorkPackage.SelectedItem.Text + ")");
                PdfPCell clHeader = new PdfPCell(phHeader);
                clHeader.Colspan = noOfColumns;
                clHeader.Border = PdfPCell.NO_BORDER;
                clHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                mainTable.AddCell(clHeader);



                // Creates a phrase for a new line.
                Phrase phSpace = new Phrase("\n");
                PdfPCell clSpace = new PdfPCell(phSpace);
                clSpace.Border = PdfPCell.NO_BORDER;
                clSpace.Colspan = noOfColumns;
                mainTable.AddCell(clSpace);

                // Sets the gridview column names as table headers.
                for (int i = 0; i < noOfColumns; i++)
                {
                    Phrase ph = null;

                    if (gdRp.AutoGenerateColumns)
                    {
                        ph = new Phrase(tbl.Columns[i].ColumnName.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    else
                    {
                        ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    PdfPCell cl = new PdfPCell(ph);
                    if (i == 1)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTable.AddCell(cl)
;
                    }
                    else if (i == 6)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTable.AddCell(cl)
;
                    }
                    else
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTable.AddCell(cl)
;
                    }

                }

                // Reads the gridview rows and adds them to the mainTable
                for (int rowNo = 0; rowNo <= noOfRows; rowNo++)
                {
                    if (rowNo != noOfRows)
                    {
                        for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                        {
                            if (gdRp.AutoGenerateColumns)
                            {
                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                mainTable.AddCell(cl)
;
                            }
                            else
                            {
                                if (columnNo == 1)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl)
;
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl)
;
                                    }
                                }
                                else if (columnNo == 2)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        s = RemoveHtml(s);
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl)
;
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl)
;
                                    }
                                }
                                else if (columnNo == 3)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        s = RemoveHtml(s);
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl)
;
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl)
;
                                    }
                                }
                                else
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl)
;
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl)
;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (type == 1)
                        {
                            for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                            {
                                string s = "Grand Total";
                                if (columnNo == 1)
                                {
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl)
;
                                }
                                else if (columnNo == 2)
                                {
                                    s = ViewState["grandtotalcount"].ToString();// Session["AwardedValue"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl)
;
                                }
                                else if (columnNo == 3)
                                {
                                    s = ViewState["grandtotaldwnldcount"].ToString();// Session["ExpenditureOverallTotal"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl)
;
                                }
                                else if (columnNo == 4)
                                {
                                    s = ViewState["grandtotalviewcount"].ToString();// Session["TargetedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl)
;
                                }
                                else if (columnNo == 5)
                                {
                                    s = ViewState["GrandTotalDocumentLinkSent"].ToString();// Session["AchievedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl)
;
                                }
                                else
                                {
                                    Phrase ph = new Phrase("", FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl)
;
                                }
                            }
                        }

                    }

                    // Tells the mainTable to complete the row even if any cell is left incomplete.
                    mainTable.CompleteRow();
                }

                // Gets the instance of the document created and writes it to the output stream of the Response object.
                // Gets the instance of the document created and writes it to the output stream of the Response object.
                if (isPrint == "Yes")
                {
                    PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "/RegExcel/printpdf.pdf", FileMode.Create));
                }
                else
                {
                    PdfWriter.GetInstance(document, Response.OutputStream);
                }
                //PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "mypdf.pdf", FileMode.Create));
                // Creates a footer for the PDF document.
                int len = 174;
                System.Text.StringBuilder time = new System.Text.StringBuilder();
                time.Append(DateTime.Now.ToString("hh:mm tt"));
                time.Append("".PadLeft(len, ' ').Replace(" ", " "));

                iTextSharp.text.Font foot = new iTextSharp.text.Font();
                foot.Size = 10;
                HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time " + time + "  Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
                pdfFooter.Alignment = Element.ALIGN_CENTER;
                document.Footer = pdfFooter;
                document.Open();
                // Adds the mainTable to the document.
                document.Add(mainTable);
                // Closes the document.
                document.Close();
                if (isPrint == "Yes")
                {
                    Session["Print"] = true;
                    Response.Write("<script>window.open ('/PDFPRint.aspx','_blank');</script>");
                }
                else
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Report_DocumentSummary_" + DateTime.Now.Ticks + ".pdf");
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ExportGridToExcel()
        {


            foreach (GridViewRow r in GrdDocumentSummary.Rows)
            {
                DataBoundLiteralControl lc1 = r.Cells[2].Controls[0] as DataBoundLiteralControl;

                r.Cells[2].Text = RemoveHtml(lc1.Text);

                DataBoundLiteralControl lc2 = r.Cells[3].Controls[0] as DataBoundLiteralControl;

                r.Cells[3].Text = RemoveHtml(lc2.Text);

            }


            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Report__document_summary" + DateTime.Now.Ticks + ".xls";

            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            GrdDocumentSummary.GridLines = GridLines.Both;
            GrdDocumentSummary.HeaderStyle.Font.Bold = true;
            GrdDocumentSummary.RenderControl(htmltextwrtter);


            string s = htmltextwrtter.InnerWriter.ToString();


            string x = ""; // WebConfigurationManager.AppSettings["Domain"];
            string y = ""; //Session["Username"].ToString();
            string z = ""; //Request.QueryString["ProjectName"];

            string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + x + " Projectwise Document Summary Report for water projects as on " + DateTime.Today.ToString("dd/MM/yyyy") + " </asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>" + z + "</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";
            Response.Write(HTMLstring);
            Response.End();

        }



    }
}