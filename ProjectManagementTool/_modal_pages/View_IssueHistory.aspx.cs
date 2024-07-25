using ProjectManagementTool.Models;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class View_IssueHistory : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (Request.QueryString["Issue_Uid"] != null)
                {
                    IssueHistoryBind();

                    //doc_pre.HRef = "/_modal_pages/preview-issue-docs.aspx?IssueUid=" + Request.QueryString["Issue_Uid"];


                }
            }
        }

        private void IssueHistoryBind()
        {
            lblProject1.Text = getdata.getProjectNameby_ProjectUID(new Guid(Request.QueryString["PrjID"]));
            lblProject.Text = lblProject1.Text;

            DataSet ds = getdata.GetIssueHistory_by_Issue_Uid(new Guid(Request.QueryString["Issue_Uid"]));

            List<tClass3> IssueHistories = new List<tClass3>();

            int r_count = 0;

            if (ds.Tables[0].Rows.Count > 0)
            {
                string docName = "";
                string doc_path = "";
                string id = "";
                int objCount = 0;
                Boolean flag = true;
                

                while (flag)
                {
                    
                    id = ds.Tables[0].Rows[objCount].ItemArray[6].ToString();
                    //docName = "<a class='showDocModal' href='/_modal_pages/preview-pdf-doc.aspx?doc_name=" + ds.Tables[0].Rows[objCount].ItemArray[7].ToString() + ds.Tables[0].Rows[objCount].ItemArray[5].ToString() + "'>" + ds.Tables[0].Rows[objCount].ItemArray[5].ToString() + "</a>";
                    docName = "<a target='_blank' href='/_modal_pages/preview-pdf-doc.aspx?doc_name=" + ds.Tables[0].Rows[objCount].ItemArray[7].ToString() + ds.Tables[0].Rows[objCount].ItemArray[5].ToString() + "'>" + ds.Tables[0].Rows[objCount].ItemArray[5].ToString() + "</a>";

                    doc_path = ds.Tables[0].Rows[objCount].ItemArray[5].ToString();

                    for (int j = objCount + 1; j <= ds.Tables[0].Rows.Count - 1; j++)
                    {

                        if (id == ds.Tables[0].Rows[j].ItemArray[6].ToString())
                        {
                            docName = docName + "</br>" + "<a target='_blank'  href='/_modal_pages/preview-pdf-doc.aspx?doc_name=" + ds.Tables[0].Rows[j].ItemArray[7].ToString() + ds.Tables[0].Rows[j].ItemArray[5].ToString() + "'>" + ds.Tables[0].Rows[j].ItemArray[5].ToString() + "</a>"; 
                            objCount = objCount + 1;
                        }
                    }

                    IssueHistories.Add(new tClass3()
                    {
                        count = r_count,
                        description = ds.Tables[0].Rows[objCount].ItemArray[0].ToString(),
                        remarks = ds.Tables[0].Rows[objCount].ItemArray[1].ToString(),
                        issue_user = ds.Tables[0].Rows[objCount].ItemArray[2].ToString(),
                        issue_date = ds.Tables[0].Rows[objCount].ItemArray[3].ToString(),
                        issue_status = ds.Tables[0].Rows[objCount].ItemArray[4].ToString(),
                        doc_name = docName,
                        id = ds.Tables[0].Rows[objCount].ItemArray[6].ToString()
                    });

                    objCount = objCount + 1;

                    if (objCount == ds.Tables[0].Rows.Count) flag = false;
                }
            }

            foreach(tClass3 obj in IssueHistories.OrderBy(a => Convert.ToDateTime(a.issue_date)).ToList())
            {
                r_count = r_count + 1;
                obj.count = r_count;
            }

            GrdIssueStatus.DataSource = IssueHistories.OrderBy(a=>Convert.ToDateTime(a.issue_date)).ToList();
            GrdIssueStatus.DataBind();

           
            if (GrdIssueStatus.Rows.Count>0)
            {
                LblIssue.Text = GrdIssueStatus.Rows[0].Cells[1].Text;
                LblUser.Text = getdata.getUserNameby_UID(new Guid(GrdIssueStatus.Rows[0].Cells[2].Text));

                Label2.Text = GrdIssueStatus.Rows[0].Cells[1].Text;
                Label3.Text = getdata.getUserNameby_UID(new Guid(GrdIssueStatus.Rows[0].Cells[2].Text));
            }

            GridView1.DataSource = IssueHistories.OrderBy(a => Convert.ToDateTime(a.issue_date)).ToList();
            GridView1.DataBind();

            GridView1.HeaderRow.Cells[1].Visible = false;
            GridView1.HeaderRow.Cells[2].Visible = false;
            GridView1.HeaderRow.Cells[7].Visible = false;


        } 
        

        protected void GrdIssueStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToString("dd-MM-yyyy");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[3].Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToString("dd-MM-yyyy");
            }
        }

        private void Obj_grdview_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            return;
        }

          
        protected void GrdIssueStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string IssueStatusUID = e.CommandArgument.ToString();
        }

        protected void GrdIssueStatus_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GrdIssueStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdIssueStatus.PageIndex = e.NewPageIndex;
        }

        protected void LinkDownload_Click(object sender, EventArgs e)
        {
            CreateAndDownloadZipFile(Request.QueryString["Issue_Uid"]);
        }

        protected void CreateAndDownloadZipFile(string issue_uid)
        {
            string zipFolder = Server.MapPath("/Documents/Issues/IssueDocs/");

            if (!Directory.Exists(zipFolder))
                Directory.CreateDirectory(zipFolder);
            else
            {
                // Delete all files from the Directory  
                foreach (string filename in Directory.GetFiles(zipFolder))
                {
                    File.Delete(filename);
                }
            }

            List<DocFile> files = new List<DocFile>();

            List<string> fileNames = new List<string>();

            DataSet ds1 = getdata.GetUploadedIssueDocuments(issue_uid);

            if (ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds1.Tables[0].Rows)
                {

                    string path = Server.MapPath(r.ItemArray[2].ToString() + r.ItemArray[1].ToString());

                    string outPath = zipFolder + "open_" + r.ItemArray[1].ToString();

                    string getExtension = System.IO.Path.GetExtension(path);

                    outPath = outPath.Replace(getExtension, "") + getExtension;

                    getdata.DecryptFile(path, outPath);

                    if (files.Where(a => a.Name.Contains(outPath)).Count() == 0)
                        files.Add(new DocFile { Name = outPath, Position = 0 });
                }

            }

            DataSet ds2 = getdata.GetUploadedAllIssueStatusDocuments(issue_uid);

            if (ds2.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds2.Tables[0].Rows)
                {
                    int p = 0;

                    string path = Server.MapPath(r.ItemArray[2].ToString() + r.ItemArray[1].ToString());

                    string outPath = zipFolder + r.ItemArray[3].ToString() + "_" + r.ItemArray[1].ToString();

                    string getExtension = Path.GetExtension(path);

                    outPath = outPath.Replace(getExtension, "") + getExtension;

                    getdata.DecryptFile(path, outPath);

                    if (r.ItemArray[3].ToString() == "In-Progress") p = 1;

                    DocFile doc_file = new DocFile() { Name = outPath, Position = p };

                    if (files.Where(a => a.Name.Contains(outPath)).Count() == 0)
                        files.Add(doc_file);
                }
            }

            // Save Zip file

            string zipFilePath = Server.MapPath("/Documents/Issues/IssueDocsZip.zip");

            foreach (DocFile item in files.OrderBy(a => a.Position))
            {
                fileNames.Add(item.Name);
            }

            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {
                zip.AddFiles(fileNames, "IssueDocs");//Zip file inside filename  
                zip.Save(zipFilePath);//location and name for creating zip file  
            }

            FileInfo file = new FileInfo(zipFilePath);

            if (file.Exists)
            {
                Response.Clear();

                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                Response.AddHeader("Content-Length", file.Length.ToString());

                Response.ContentType = "application/octet-stream";

                Response.WriteFile(file.FullName);

                Response.End();

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");
            }
        }

        protected void btnprintpdf_Click(object sender, EventArgs e)
        {

        }



    }
}