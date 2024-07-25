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
    public partial class add_correspondence : System.Web.UI.Page
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

                    BindDocuments();

                    if (Request.QueryString["StatusUID"] != null)
                    {
                        BindStatus();
                        BindDocStatus(new Guid(Request.QueryString["StatusUID"]));
                        btnSubmit.Text = "Save";

                        string corr_type = Request.QueryString["LetterType"].ToString();

                        if (corr_type  == "1")
                        {
                            lblCorrespondenceType.InnerText = "Correspondence Type : Client to Consultant";
                        }
                        else if (corr_type == "2")
                        {
                            lblCorrespondenceType.InnerText = "Correspondence Type : Consultant to Client";
                        }
                        else
                        {
                            lblCorrespondenceType.InnerText = "Correspondence Type : Client Internal";
                        }
                    }
                }
            }
        }
        private void BindDocuments()
        {
             DataSet ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"]));
             DDLDocument.DataTextField = "ActualDocument_Name";
             DDLDocument.DataValueField = "ActualDocumentUID";
             DDLDocument.DataSource = ds;
             DDLDocument.DataBind();
        }

        private void BindStatus()
        {
             DataSet ds1 = getdata.getTop1_DocumentStatusSelect(new Guid(Request.QueryString["DocID"]));
             string SubmittalUID = getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"]));
             string FlowType = getdata.GetFlowTypeBySubmittalUID((new Guid(SubmittalUID)));
             if (ds1.Tables[0].Rows.Count > 0)
             {
                        DataSet ds = getdata.GetStatus_by_UserType_FlowUID(Session["TypeOfUser"].ToString(), ds1.Tables[0].Rows[0]["ActivityType"].ToString(), new Guid(ds1.Tables[0].Rows[0]["FlowUID"].ToString()));
                        if (FlowType == "STP")
                        {
                            ds = getdata.GetStatus_by_UserType_FlowUID_STP(Session["TypeOfUser"].ToString(), ds1.Tables[0].Rows[0]["ActivityType"].ToString(), new Guid(ds1.Tables[0].Rows[0]["FlowUID"].ToString()), new Guid(SubmittalUID), new Guid(Session["UserUID"].ToString()));

                        }
                        DDlStatus.DataTextField = "Update_Status";
                        DDlStatus.DataValueField = "Update_Status";
                        DDlStatus.DataSource = ds;
                        DDlStatus.DataBind();
                        //added on 13/07/2022
                        if (ds1.Tables[0].Rows[0]["ActivityType"].ToString().Contains("Code A") || ds1.Tables[0].Rows[0]["ActivityType"].ToString().Contains("Code B"))
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (dr["Update_Status"].ToString().Contains("Code A") || dr["Update_Status"].ToString().Contains("Code B"))
                                {
                                    DDlStatus.SelectedValue = dr["Update_Status"].ToString();
                                }
                            }
                        }
                        else
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (!dr["Update_Status"].ToString().ToLower().Contains("back"))
                                {
                                    DDlStatus.SelectedValue = dr["Update_Status"].ToString();
                                    return;
                                }
                            }
                        }
                        //
                        if (DDlStatus.Items.Count > 0)
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "Tooltip", "getValueFromCodeBehind('" + DDlStatus.SelectedItem.Text + "')", true);
                            btnSubmit.Visible = true;

                        }
                        else
                        {
                            btnSubmit.Visible = false;
                        }
                        // added on  12/01/2022 for Correspondence ONTB / flow 3 NJSEI

                        string Approver = getdata.GetSubmittal_Approver_By_DocumentUID(new Guid(Request.QueryString["DocID"]));
                        string Reviewer = getdata.GetSubmittal_Reviewer_By_DocumentUID(new Guid(Request.QueryString["DocID"]));
                        if (Session["UserUID"].ToString().ToUpper() == Reviewer.ToUpper())
                        {

                        }
                        else if (Approver != "")
                        {
                            if (Session["UserUID"].ToString().ToUpper() == Approver.ToUpper())
                            {
                                DDlStatus.Items.Remove("Reply");
                                DDlStatus.Items.Remove("Received");
                            }
                            else
                            {
                                DDlStatus.Items.Remove("Closed");
                            }

                        }

             }
        }


        private void BindDocStatus(Guid StatusUID)
        {
            DataSet ds = getdata.getDocumentStatusList_by_StatusUID(StatusUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLDocument.SelectedValue = ds.Tables[0].Rows[0]["DocumentUID"].ToString();
                txtcomments.Text = ds.Tables[0].Rows[0]["Status_Comments"].ToString();
                if (DDlStatus.Items.Count > 0)
                {
                    DDlStatus.SelectedItem.Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
                }
               
                if (ds.Tables[0].Rows[0]["DocumentDate"].ToString() != null && ds.Tables[0].Rows[0]["DocumentDate"].ToString() != "")
                {
                    dtDocumentDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["DocumentDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ds.Tables[0].Rows[0]["LinkToReviewFile"].ToString() != "" && ds.Tables[0].Rows[0]["LinkToReviewFile"].ToString() != null)
                {
                    FileUploadDoc.Enabled = false;
                }
                else
                {
                    FileUploadDoc.Enabled = true;
                }
                
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e) //final submit
        {
            try
            {
                string DocPath = "";
                Guid StatusUID = new Guid();
                string CoverPagePath = string.Empty;
                
                StatusUID = new Guid(Request.QueryString["StatusUID"]);

                Guid CorrespondenceUID = Guid.NewGuid();

                Guid ActualDocumentUID = new Guid(Request.QueryString["DocID"]);

                string LetterType = Request.QueryString["LetterType"].ToString();

                string RefNo = txtrefNumber.Text;

                string status = DDlStatus.SelectedItem.Text;

                DateTime coverLetterDate = Convert.ToDateTime(getdata.ConvertDateFormat(dtDocumentDate.Text));

                Guid userID = new Guid(Session["userUID"].ToString());

                string statusComment = txtcomments.Text;

                string LinkPath = "";

                if (FileUploadDoc.HasFile)
                {

                    LinkPath = "~/" + Request.QueryString["ProjectUID"] + "/" + CorrespondenceUID + "/Link Document";

                    if (!Directory.Exists(Server.MapPath(LinkPath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(LinkPath));
                    }
                    string sFileName = Path.GetFileNameWithoutExtension(FileUploadDoc.FileName);
                    string Extn = System.IO.Path.GetExtension(FileUploadDoc.FileName);

                    FileUploadDoc.SaveAs(Server.MapPath(LinkPath + "/" + sFileName + "_1_copy" + Extn));

                    string savedPath = LinkPath + "/" + sFileName + "_1_copy" + Extn;
                    DocPath = LinkPath + "/" + sFileName + "_1" + Extn;
                    getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DocPath));
                }

                string sDocumentPath = "";

                if (FileUploadCoverLetter.HasFile)
                {

                    sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/" + CorrespondenceUID + "/CoverLetter";

                    if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(sDocumentPath));
                    }

                    string sFileName = Path.GetFileNameWithoutExtension(FileUploadCoverLetter.FileName);
                    string Extn = System.IO.Path.GetExtension(FileUploadCoverLetter.FileName);
                    FileUploadCoverLetter.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                    string savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                    CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;
                    getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));

                }

                getdata.InsertCorrespondence(CorrespondenceUID, ActualDocumentUID, RefNo, StatusUID, status, coverLetterDate, userID, CoverPagePath, DocPath, statusComment, LetterType);

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");

            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('" + ex.Message + "');</script>");
            }
        }
               
        protected void DDlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetAllUSersforPMC();
        }

    }
}
