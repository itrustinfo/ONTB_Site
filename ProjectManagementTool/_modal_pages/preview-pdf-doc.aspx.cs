using ProjectManager.DAL;
using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Drawing.Printing;
using System.Drawing;
using System.Net;

namespace ProjectManagementTool._modal_pages
{
    public partial class preview_pdf_doc : System.Web.UI.Page
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
                string doc_name =   Request.QueryString["doc_name"]; //Session["pdfImageSrc"].ToString();

                doc_name = Server.MapPath(doc_name);

                FileInfo file = new System.IO.FileInfo(doc_name);

                if (file.Exists)
                {
                    string getExtension = System.IO.Path.GetExtension(doc_name);

                    string filename = Path.GetFileName(doc_name).Replace(getExtension, "") + "_download" + getExtension;


                    string outPath = Server.MapPath("~/_PreviewLoad/" + filename);


                    getdata.DecryptFile(doc_name, outPath);

                    if (getExtension == ".pdf")
                    {
                        btnImgPrint.Visible = false;
                        WebClient User = new WebClient();

                        Byte[] FileBuffer = User.DownloadData(outPath);
                        if (FileBuffer != null)
                        {
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-length", FileBuffer.Length.ToString());
                            Response.BinaryWrite(FileBuffer);
                        }
                    }
                    else
                    {
                        btnImgPrint.Visible = true;
                        image1.ImageUrl = "~/_PreviewLoad/" + filename;
                    }
                   
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File not found.');</script>");
                }

                
            }
        }

    
    }
}