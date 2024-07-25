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

namespace ProjectManagementTool._modal_pages
{
    public partial class upload_invoice_document : System.Web.UI.Page
    {
        private DBGetData getData = new DBGetData();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (Request.QueryString["InvoiceMaster_UID"] != null && Request.QueryString["WorkpackageUID"] != null)
                {
                    BindDataforDocument_Invoice(Request.QueryString["InvoiceMaster_UID"].ToString());
                }
            }
        }
        private void BindDataforDocument_Invoice(string InvUID)
        {
            DataTable dt = getData.GetInvoiceDocuement(new Guid(InvUID));

            dt.Columns.Add("FileName");
            foreach(DataRow eachRow in dt.Rows)
            {
                string fileName = Path.GetFileName(eachRow.Field<string>("FilePath"));
                eachRow.SetField<string>("FileName", fileName);
            }

            GrdTreeView.DataSource = dt;
            GrdTreeView.DataBind();
            GrdTreeView.Columns[1].Visible = false;
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string sFileDirectory = "~/Documents/Invoices/" + Request.QueryString["InvoiceMaster_UID"];

            if (!Directory.Exists(Server.MapPath(sFileDirectory)))
            {
                Directory.CreateDirectory(Server.MapPath(sFileDirectory));
            }
            string InvoiceUID = Request.QueryString["InvoiceMaster_UID"];

            foreach (HttpPostedFile uploadedFile in ImageUpload.PostedFiles)
            {
                if (uploadedFile.ContentLength > 0 && !String.IsNullOrEmpty(uploadedFile.FileName))
                {
                    
                    string description = "";
                    string sFileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                    string Extn = Path.GetExtension(uploadedFile.FileName);
                    uploadedFile.SaveAs(Server.MapPath(sFileDirectory + "/" + sFileName + Extn));
                    string savedPath = sFileDirectory + "/" + sFileName + Extn;
                    string EncryptPagePath = sFileDirectory + "/" + sFileName + "_DE" + Extn;

                    //Encrypt the file
                    getData.EncryptFile(Server.MapPath(savedPath), Server.MapPath(EncryptPagePath));

                    int Cnt = getData.Invoice_Document_InsertUpdate(Guid.NewGuid(), new Guid(Request.QueryString["InvoiceMaster_UID"]), new Guid(Request.QueryString["WorkpackageUID"]), EncryptPagePath, new Guid(Session["UserUID"].ToString()), description);
                    if (File.Exists(Server.MapPath(savedPath)))
                    {
                        File.Delete(Server.MapPath(savedPath));
                    }
                    if (Cnt <= 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ADDSP-01 there is a problem with this feature. Please contact system admin.');</script>");
                    }
                }
            }
            BindDataforDocument_Invoice(InvoiceUID);
        }

        protected void GrdTreeView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            HiddenField hidInvoiceUID = GrdTreeView.Rows[e.RowIndex].FindControl("documentDeleteuid") as HiddenField;

            int Cnt = getData.InvoiceDocuement_Delete( new Guid(hidInvoiceUID.Value), new Guid(Session["UserUID"].ToString()));
            
            string invBuilID = Request.QueryString["InvoiceMaster_UID"];
            BindDataforDocument_Invoice(invBuilID);
        }

        protected void Download_Click(object sender, EventArgs e)
        {
            {
                string FilePath = ((HtmlAnchor)sender).HRef;
                string getExtension = Path.GetExtension(FilePath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(FilePath);
                string outPath = fileNameWithoutExtension + "_download" + getExtension;

                // Decrypt the file using Server.MapPath for both paths
                getData.DecryptFile(Server.MapPath(FilePath), Server.MapPath(outPath));

                // Download the decrypted file
                DownloadFile(Server.MapPath(outPath));
            }

        }
        //protected void Download_Click(object sender, EventArgs e)
        //{
        //    string FilePath = ((HtmlAnchor)sender).HRef;
        //    string getExtension = Path.GetExtension(FilePath);
        //    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(FilePath);
        //    string outPath = fileNameWithoutExtension + "_download" + getExtension;

        //    // Decrypt the file using Server.MapPath for both paths
        //    getData.DecryptFile(Server.MapPath(FilePath), Server.MapPath(outPath));

        //    // Rename the file to the original filename
        //    string originalFileName = GetOriginalFileName(fileNameWithoutExtension);
        //    string newOutPath = originalFileName + getExtension;

        //    // Rename the file on the server
        //    File.Move(Server.MapPath(outPath), Server.MapPath(newOutPath));

        //    // Download the renamed file
        //    DownloadFile(Server.MapPath(newOutPath));
        //}

        private void DownloadFile(string file)
        {
            var fi = new FileInfo(file);
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fi.Name);
            Response.WriteFile(file);
            Response.End();
        }

        protected string GetOriginalFileName(object fileName)
        {
            if (fileName != null)
            {
                string originalFileName = fileName.ToString();

                // Check if the filename contains "_DE" and remove it
                int deIndex = originalFileName.LastIndexOf("_DE");
                if (deIndex != -1)
                {
                    originalFileName = originalFileName.Remove(deIndex, 3); // Remove "_DE"
                }

                return originalFileName;
            }

            return string.Empty;
        }
    }
}