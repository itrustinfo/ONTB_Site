using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager;

namespace ProjectManagementTool._modal_pages
{
    public partial class view_correspondence : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (!IsPostBack)
                {
                    BindCorrespondence();
                }
            }
        }

        protected void BindCorrespondence()
        {

            DataSet CorrespondenceList = getdata.getCorrespondenceLetters(new Guid(Request.QueryString["StatusUID"].ToString()), Request.QueryString["LetterType"].ToString());
            
            GrdViewCorrespondence.DataSource = CorrespondenceList;
            GrdViewCorrespondence.DataBind();

        }

        protected void GrdViewCorrespondence_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                LinkButton button1 = (LinkButton)e.Row.Cells[5].FindControl("lnkdownload1");

                if (button1 != null)
                {
                    if (e.Row.Cells[4].Text == "&nbsp;")
                    {
                        button1.Text = "";
                    }
                }

                LinkButton button2 = (LinkButton)e.Row.Cells[7].FindControl("lnkdownload2");

                if (button2 != null)
                {
                    if (e.Row.Cells[6].Text == "&nbsp;")
                    {
                        button2.Text = "";
                    }
                }
            }
        }

        protected void GrdViewCorrespondence_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "download1")
            {
                    string path = Server.MapPath(e.CommandArgument.ToString());
                    string getExtension = Path.GetExtension(path);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdata.DecryptFile(path, outPath);
                    FileInfo file = new FileInfo(outPath);

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

            if (e.CommandName == "download2")
            {
                string path = Server.MapPath(e.CommandArgument.ToString());
                string getExtension = Path.GetExtension(path);
                string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                getdata.DecryptFile(path, outPath);
                FileInfo file = new FileInfo(outPath);

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
        }



        protected void GrdViewCorrespondence_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}
