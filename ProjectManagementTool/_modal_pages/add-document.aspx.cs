﻿using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager._modal_pages
{
    public partial class add_document : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
        protected static string content;
       // protected static bool inProcess = false;
       // protected static bool processComplete = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                this.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
                if (!IsPostBack)
                {
                    if (Request.QueryString["dUID"] != null)
                    {
                        BindProject();
                        BindWorkPackage();
                        BindOriginator();
                        //BindDocument_Type_Master();
                        BindDocuments();
                        //GetDocumentFolder(new Guid(Request.QueryString["dUID"]));
                        DDLDocumentType_SelectedIndexChanged(sender, e);
                        string PRefNumber = string.Empty;
                        if (Request.QueryString["pUID"] != "00000000-0000-0000-0000-000000000000")
                        {
                            PRefNumber = getdata.GetProjectRef_Number(new Guid(Request.QueryString["pUID"]));
                            txtprefnumber.Text = PRefNumber;
                        }
                        UploadFolder.Visible = false;
                        UploadFile.Visible = true;

                        if (Session["copydocument"] == null)
                        {
                            RBLUploadType.Items.RemoveAt(2);
                            CopiedDocuments.Visible = false;
                        }
                    }
                    else
                    {
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                    if (Request.QueryString["fID"] != null)
                    {
                        string FlowName = getdata.GetFlowName_by_SubmittalID(new Guid(Request.QueryString["dUID"]));
                        string FlowType = getdata.GetFlowTypeBySubmittalUID(new Guid(Request.QueryString["dUID"]));
                        if (FlowName == "Flow 3" || FlowName.Contains("Correspondence"))
                        {
                            spfile.Visible = false;
                        }
                        //
                        if(FlowName == "Online Flow 2")
                        {
                            CoverLetterUpload.Visible = false;
                            UploadFile.Visible = false;
                            UploadFolder.Visible = false;
                            btnSubmit.Visible = false;
                            btnSubmitOlddoc.Visible = true;
                            lblONTbrefno.InnerText = "ONTB Reference Number";
                            txtRefNumber.Text = "dummy_"  + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                            txtRefNumber.Enabled = true;
                            RBLUploadType.Items.RemoveAt(1);
                            RBLSubmissionType.Items.RemoveAt(1);
                            RBLSubmissionType.Items.RemoveAt(1);
                        }
                        //
                        if (WebConfigurationManager.AppSettings["Domain"] == "ONTB" || WebConfigurationManager.AppSettings["Domain"] == "LNT")
                        {
                            if(FlowType == "STP")
                            {
                                txtprefnumber.Enabled = false;
                                txtprefnumber.Text = "";
                                divFileAttachmentDoc.Visible = false;
                            }
                            else if(FlowType == "STP-C")
                            {
                                txtprefnumber.Enabled = false;
                                txtprefnumber.Text = "";
                                divFileAttachmentDoc.Visible = true;
                                UploadFile.Visible = false;
                                RBLUploadType.Items.RemoveAt(1);
                                RBLSubmissionType.Items.RemoveAt(1);
                                RBLSubmissionType.Items.RemoveAt(1);
                            }
                            else if (FlowType == "STP-OB")
                            {
                                txtprefnumber.Enabled = true;
                                txtprefnumber.Text = "";
                                divFileAttachmentDoc.Visible = true;
                                UploadFile.Visible = false;
                                divcorrespondenceuser.Visible = true;
                                divcorrespondenceccto.Visible = true;
                                FileRefNumber.Visible = false;
                                IncomingReceDate.Visible = false;
                                lblONTbrefno.InnerText = "ONTB Reference Number";
                                spONTB.Visible = true;
                                RBLUploadType.Items.RemoveAt(1);
                                RBLSubmissionType.Items.RemoveAt(1);
                                RBLSubmissionType.Items.RemoveAt(1);
                                if(FlowName == "DTL Correspondence")
                                {
                                    divcorrespondenceccto.Visible = true;
                                }
                            }

                        }
                        DataSet dsFlow = getdata.GetDocumentFlows_by_UID(new Guid(Request.QueryString["fID"]));
                        if (dsFlow.Tables[0].Rows.Count > 0)
                        {
                            if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "-1")
                            {
                                Originator.Visible = false;
                                OriginatorNumber.Visible = false;
                                DocumentDate.Visible = false;
                                CoverLetterUpload.Visible = false;
                                UploadFolder.Visible = false;
                                RBLUploadType.Items.RemoveAt(1);
                                IncomingReceDate.Visible = false;
                                PrjRefNumber.Visible = false;
                                DocumentMedia.Visible = false;
                                FileRefNumber.Visible = false;
                                DDLDocumentType.SelectedValue = "Photographs";
                                DataSet dsDoc = getdata.getDocumentsbyDocID(new Guid(Request.QueryString["dUID"]));
                                if (dsDoc.Tables[0].Rows.Count > 0)
                                {
                                    dtDocumentDate.Text = Convert.ToDateTime(dsDoc.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    dtIncomingRevDate.Text = Convert.ToDateTime(dsDoc.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                //RBLUploadType.Items.RemoveAt(2);
                            }
                            else
                            {
                                DDLDocumentType.Items.RemoveAt(2);
                            }
                        }
                    }

                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U")
            {

                ds = TKUpdate.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {

                //ds = TKUpdate.GetProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
                ds = TKUpdate.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {

                //ds = TKUpdate.GetProjects();
                ds = TKUpdate.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();

            if (Request.QueryString["pUID"] != null)
            {
                DDlProject.SelectedValue = Request.QueryString["pUID"].ToString();
            }

        }

        private void BindWorkPackage()
        {
            DDLWorkPackage.DataTextField = "Name";
            DDLWorkPackage.DataValueField = "WorkPackageUID";
            DDLWorkPackage.DataSource = getdata.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
            DDLWorkPackage.DataBind();

            if (Request.QueryString["wUID"] != null)
            {
                DDLWorkPackage.SelectedValue = Request.QueryString["wUID"].ToString();
            }
        }

        private void BindOriginator()
        {
            if (WebConfigurationManager.AppSettings["Domain"] == "ONTB" || WebConfigurationManager.AppSettings["Domain"] == "LNT" || WebConfigurationManager.AppSettings["Domain"] == "Suez")
            {
                RBLOriginator.Items.Insert(0, new ListItem("Contractor","Contractor"));
                RBLOriginator.Items.Insert(1, new ListItem("ONTB", "ONTB"));
                RBLOriginator.Items.Insert(2, new ListItem("BWSSB", "BWSSB"));
                RBLOriginator.Items.Insert(3, new ListItem("Others", "Others"));
                RBLOriginator.Items[0].Selected = true;
                //added on 14/10/2022 for DTL,EE,ACE,CE Correspondence
                string FlowName = getdata.GetFlowName_by_SubmittalID(new Guid(Request.QueryString["dUID"]));
                string FlowType = getdata.GetFlowTypeBySubmittalUID(new Guid(Request.QueryString["dUID"]));
                
                if (FlowType == "STP-OB")
                {
                    RBLOriginator.Items.Clear();
                    if (FlowName.Contains("DTL"))
                    {
                        RBLOriginator.Items.Insert(0, new ListItem("ONTB", "ONTB"));
                        RBLOriginator.Items[0].Selected = true;
                    }
                    else
                    {
                        RBLOriginator.Items.Insert(0, new ListItem("BWSSB", "BWSSB"));
                        RBLOriginator.Items[0].Selected = true;
                    }
                    DataSet ds = getdata.GetCorrespondenceLetterUsersTo(new Guid(Request.QueryString["fID"].ToString()));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        rdcorrespondenceuser.DataTextField = "LetterTo";
                        rdcorrespondenceuser.DataValueField = "LetterToStatus";
                        rdcorrespondenceuser.DataSource = ds;
                        rdcorrespondenceuser.DataBind();
                        rdcorrespondenceuser.Items[0].Selected = true;
                        //
                        chkcorrespondenceccto.DataTextField = "LetterTo";
                        chkcorrespondenceccto.DataValueField = "LetterTo";
                        chkcorrespondenceccto.DataSource = ds;
                        chkcorrespondenceccto.DataBind();
                        
                    }
                }
                //
            }
            else
            {
                DataSet ds = getdata.GetOriginatorMaster();
                RBLOriginator.DataTextField = "Originator_Name";
                RBLOriginator.DataValueField = "Originator_Name";
                RBLOriginator.DataSource = ds;
                RBLOriginator.DataBind();
                // added by zuber on 22/02/2022 for KIADB
                if (getdata.GetClientCodebyWorkpackageUID(new Guid(Request.QueryString["wUID"].ToString())) != "")
                {
                    RBLOriginator.Items.Insert(0, new ListItem(getdata.GetClientCodebyWorkpackageUID(new Guid(Request.QueryString["wUID"].ToString())), getdata.GetClientCodebyWorkpackageUID(new Guid(Request.QueryString["wUID"].ToString()))));
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    RBLOriginator.Items[0].Selected = true;
                }
            }
            
          
           

        }

        private void BindDocuments()
        {
            DDLDocuments.DataTextField = "DocName";
            DDLDocuments.DataValueField = "DocumentUID";
            DDLDocuments.DataSource = getdata.getDocumentsbyDocID(new Guid(Request.QueryString["dUID"]));
            DDLDocuments.DataBind();

            if (Request.QueryString["dUID"] != null)
            {
                DDLDocuments.SelectedValue = Request.QueryString["dUID"].ToString();

                int FlowStepCount = getdata.GetFlowStepCount_by_SubmittalID(new Guid(Request.QueryString["dUID"].ToString()));
                if (FlowStepCount >0)
                {
                    if (FlowStepCount == 1)
                    {
                        DDLDocumentType.SelectedIndex = 1;
                    }
                    else if (FlowStepCount == -1)
                    {
                        DDLDocumentType.SelectedIndex = 2;
                    }
                    else
                    {
                        DDLDocumentType.SelectedIndex = 0;
                    }
                    DDLDocumentType.Enabled = false;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e) // Final Submit
        {
            bool DocExists = false;
            int DocumentCount = 0;
            string CC = string.Empty;
            string FlowName = getdata.GetFlowName_by_SubmittalID(new Guid(Request.QueryString["dUID"]));

            if (dtIncomingRevDate.Text != "" && dtDocumentDate.Text != "")
            {

                DateTime CheckIncomingRec_Date = Convert.ToDateTime(getdata.ConvertDateFormat(dtIncomingRevDate.Text));
                DateTime CheckDocumentDate = Convert.ToDateTime(getdata.ConvertDateFormat(dtDocumentDate.Text));

                if (CheckDocumentDate > CheckIncomingRec_Date)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Document Date connot be greater than Incoming Receive date.');</script>");
                    return;
                }
            }

            //added for Yogesh flow changes on 29/12/2021
            if (!FileUploadCoverPage.HasFile && DDLDocumentType.SelectedValue == "Cover Letter")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please upload a cover letter.');</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
                return;
            }
            if (FlowName == "Flow 3" || FlowName.Contains("Correspondence"))
            {
                // added for flow change by yogesh for correspondance
                if (!FileUploadDoc.HasFile)
                {
                    FileUploadDoc = FileUploadCoverPage;                 
                }
                
            }
            else
            {
                if (!FileUploadDoc.HasFile && DDLDocumentType.SelectedValue == "Cover Letter" && RBLUploadType.SelectedValue == "File")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please upload a document.');</script>");
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
                    return;
                }
            }
            //----------------------------------------------------
            bool FolderasFiles = false;
            if (RBLUploadType.SelectedValue == "Folder" && Request.Files.Count == 1)
            {
                if (String.IsNullOrEmpty(Request.Files[0].FileName))
                {
                    FolderasFiles = true;
                }
            }
         
            if (!FileUploadDoc.HasFile && RBLUploadType.SelectedValue == "File" && DDLDocumentType.SelectedValue == "General Document")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please upload a document.');</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
            }
           
            else if (RBLUploadType.SelectedValue == "Folder" && Request.Files.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please choose a folder to upload documents.');</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
            }
            else if (RBLUploadType.SelectedValue == "Folder" && FileUploadCoverPage.HasFile && Request.Files.Count == 1)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please choose a folder to upload documents.');</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
            }
            else if (RBLUploadType.SelectedValue == "Folder" && FolderasFiles)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('No documents found under choosed folder.Please choose a folder which has documents.');</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
            }
            else if (dtIncomingRevDate.Text != "" && !getdata.IsValidDate(dtIncomingRevDate.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Datetime format is not supported. Please contact System admin.');</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
            }
            else if (!FileUploadDoc.HasFile && RBLUploadType.SelectedValue == "File" && DDLDocumentType.SelectedValue == "Photographs")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please upload a photograph.');</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
            }
            else
            {
                //if (DDLDocumentType.SelectedValue == "Cover Letter" && RBLUploadType.SelectedValue == "File" && FileUploadDoc.HasFiles)
                //{
                //    foreach (HttpPostedFile uploadedFile in FileUploadDoc.PostedFiles)
                //    {
                //        string UploadFileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                //        int Cnt = getdata.Document_Exists_DocumentUID_Name(new Guid(Request.QueryString["dUID"]), UploadFileName);
                //        if (Cnt > 0)
                //        {
                //            DocExists = true;
                //            break;
                //        }
                //    }
                //}

                //if (DDLDocumentType.SelectedValue == "Cover Letter" && RBLUploadType.SelectedValue == "Folder" && Request.Files.Count > 0)
                //{
                //    HttpFileCollection attachments = Request.Files;
                //    for (int i = 0; i < attachments.Count; i++)
                //    {
                //        HttpPostedFile attachment = attachments[i];
                //        if (attachment.ContentLength > 0 && !String.IsNullOrEmpty(attachment.FileName))
                //        {
                //            string sFileName = Path.GetFileNameWithoutExtension(Path.GetFileName(attachment.FileName));
                //            int Cnt = getdata.Document_Exists_DocumentUID_Name(new Guid(Request.QueryString["dUID"]), sFileName);
                //            if (Cnt > 0)
                //            {
                //                DocExists = true;
                //                break;
                //            }
                //        }
                //    }
                //}
                //if (DocExists)
                //{
                //    ModalPopupExtender1.Show();
                //}
                try
                {
                    btnSubmit.Enabled = false;
                    Guid ActualDocumentUID = Guid.NewGuid();
                    string projectId = Request.QueryString["pUID"].ToString();
                    string workpackageid = Request.QueryString["wUID"].ToString();
                    string sDocumentUID = Request.QueryString["dUID"].ToString();
                    string sDate1 = "", sDate2 = "", sDate3 = "", sDate4 = "", sDate5 = "", CoverPagePath = "", sDate6 = "", sDate7 = "", sDate8 = "", sDate9 = "", sDate10 = "", sDate11 = "", sDate12 = "", sDate13 = "", sDate14 = "", sDate15 = "", sDate16 = "", sDate17 = "", sDate18 = "", sDate19 = "", sDate20 = "";
                    DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now, CDate4 = DateTime.Now, CDate5 = DateTime.Now, DocStartDate = DateTime.Now, CDate6 = DateTime.Now, CDate7 = DateTime.Now, CDate8 = DateTime.Now, CDate9 = DateTime.Now, CDate10 = DateTime.Now, CDate11 = DateTime.Now, CDate12 = DateTime.Now, CDate13 = DateTime.Now, CDate14 = DateTime.Now, CDate15 = DateTime.Now, CDate16 = DateTime.Now, CDate17 = DateTime.Now, CDate18 = DateTime.Now, CDate19 = DateTime.Now, CDate20 = DateTime.Now;
                    DataSet ds = getdata.getDocumentsbyDocID(new Guid(Request.QueryString["dUID"]));
                    DateTime startdate;
                    DateTime endate;
                    double TimeDuration = 0;
                    string filepathemail = string.Empty;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string cStatus = "Submitted";
                        DataSet dsFlowcheck =getdata.GetDocumentFlows_by_UID(new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()));
                        if (dsFlowcheck.Tables[0].Rows[0]["Type"] != DBNull.Value)
                        {
                            if (dsFlowcheck.Tables[0].Rows[0]["Type"].ToString() == "STP" || dsFlowcheck.Tables[0].Rows[0]["Type"].ToString() == "STP-C")
                            {
                                cStatus = "Reconciliation";
                            }
                            else if(dsFlowcheck.Tables[0].Rows[0]["Type"].ToString() == "STP-OB")// BWSSB/ONTB Correspondence
                            {
                                cStatus = rdcorrespondenceuser.SelectedValue;
                                if (string.IsNullOrEmpty(txtprefnumber.Text))
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please enter ONTB Ref No field !.');</script>");
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
                                    btnSubmit.Enabled = true;
                                    return;
                                }
                                //added on 23/11/2022
                                if (FlowName == "DTL Correspondence")
                                {
                                   

                                    DataSet dsMUSers = getdata.GetNextUser_By_SubmittalUID(new Guid(Request.QueryString["dUID"]), 7);
                                    if (dsMUSers.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                        {
                                            if (Session["UserUID"].ToString().ToUpper() == druser["Approver"].ToString().ToUpper())
                                            {
                                               if(cStatus.Contains("Submitted to EE"))
                                                {
                                                    cStatus = "Submitted to DTL for EE";
                                                }
                                                else if (cStatus.Contains("Submitted to CE"))
                                                {
                                                    cStatus = "Submitted to DTL for CE";
                                                }
                                                else if (cStatus.Contains("Submitted to ACE"))
                                                {
                                                    cStatus = "Submitted to DTL for ACE";
                                                }
                                            }
                                        }
                                    }
                                }
                                //
                            }
                        }
                        string FileDatetime = DateTime.Now.ToString("dd MMM yyyy hh-mm-ss tt");
                        string sDocumentPath = string.Empty;
                        string DocumentFor = "";

                        string IncomingRec_Date_String = string.Empty;
                        if (dtIncomingRevDate.Text != "")
                        {
                            IncomingRec_Date_String = dtIncomingRevDate.Text;
                        }
                        else
                        {
                            //IncomingRec_Date_String = DateTime.Now.ToString("dd/MM/yyyy");
                            IncomingRec_Date_String = dtDocumentDate.Text;
                        }
                        //IncomingRec_Date_String = IncomingRec_Date_String.Split('/')[1] + "/" + IncomingRec_Date_String.Split('/')[0] + "/" + IncomingRec_Date_String.Split('/')[2];
                        IncomingRec_Date_String = getdata.ConvertDateFormat(IncomingRec_Date_String);
                        DateTime IncomingRec_Date = Convert.ToDateTime(IncomingRec_Date_String);

                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar()", true);
                        string DocumentDate = string.Empty;

                        if (dtDocumentDate.Text != "")
                        {
                            DocumentDate = dtDocumentDate.Text;
                        }
                        else
                        {
                            DocumentDate = DateTime.MinValue.ToString("MM/dd/yyyy");
                        }
                        DocumentDate = getdata.ConvertDateFormat(DocumentDate);
                        DateTime Document_Date = Convert.ToDateTime(DocumentDate);
                        string CoverLetterUID = "";
                        if (FileUploadCoverPage.HasFile)
                        {
                            string Originator = string.Empty;
                            //string DocumentDate = string.Empty;
                            string Extn = System.IO.Path.GetExtension(FileUploadCoverPage.FileName);
                            if (Extn.ToLower() != ".exe" && Extn.ToLower() != ".msi" && Extn.ToLower() != ".db")
                            {
                                if (DDLDocumentType.SelectedItem.Text == "Cover Letter")
                                {
                                    DocumentFor = "Cover Letter";
                                    Originator = RBLOriginator.SelectedValue;
                                    if (dtDocumentDate.Text != "")
                                    {
                                        DocumentDate = dtDocumentDate.Text;
                                    }
                                    else
                                    {
                                        DocumentDate = DateTime.MinValue.ToString("MM/dd/yyyy");
                                    }
                                }
                                else
                                {
                                    DocumentFor = "General Document";
                                    //DocumentDate = DateTime.MinValue.ToString("MM/dd/yyyy");
                                    if (dtDocumentDate.Text != "")
                                    {
                                        DocumentDate = dtDocumentDate.Text;
                                    }
                                    else
                                    {
                                        DocumentDate = DateTime.MinValue.ToString("MM/dd/yyyy");
                                    }
                                }

                                //DocumentDate = DocumentDate.Split('/')[1] + "/" + DocumentDate.Split('/')[0] + "/" + DocumentDate.Split('/')[2];
                                //DocumentDate = getdata.ConvertDateFormat(DocumentDate);
                                //DateTime Document_Date = Convert.ToDateTime(DocumentDate);

                                if (Request.QueryString["tUID"] != "00000000-0000-0000-0000-000000000000")
                                {
                                    //sDocumentPath = "~/" + projectId + "/" + workpackageid + "/" + sDocumentUID + "/" + FileDatetime + "/CoverLetter";
                                    sDocumentPath = "~/" + projectId + "/" + workpackageid + "/CoverLetter";
                                }
                                else if (Request.QueryString["wUID"] != "00000000-0000-0000-0000-000000000000")
                                {
                                    //sDocumentPath = "~/" + workpackageid + "/" + sDocumentUID + "/" + FileDatetime + "/CoverLetter";
                                    sDocumentPath = "~/" + workpackageid + "/CoverLetter";
                                }
                                else if (Request.QueryString["pUID"] != "00000000-0000-0000-0000-000000000000")
                                {
                                    //sDocumentPath = "~/" + projectId + "/" + sDocumentUID + "/" + FileDatetime + "/CoverLetter";
                                    sDocumentPath = "~/" + projectId + "/CoverLetter";
                                }
                                if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(sDocumentPath));
                                }

                                string sFileName = Path.GetFileNameWithoutExtension(FileUploadCoverPage.FileName);
                                //added on 22/11/2022
                                int RandomNo = getdata.GenerateRandomNo();
                                string savedPath = string.Empty;
                               // FileUploadCoverPage.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                                //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + sDocumentUID + "_" + txtDocName.Text + "_1"  + "_enp" + InputFile));
                                if (getdata.IsCoverNameExists(workpackageid, sFileName))
                                {
                                    savedPath = sDocumentPath + "/" + sFileName + "_" + RandomNo + "_1_copy" + Extn;
                                    FileUploadCoverPage.SaveAs(Server.MapPath(savedPath));
                                    CoverPagePath = sDocumentPath + "/" + sFileName + "_" + RandomNo + "_1" + Extn;
                                }
                                else
                                {
                                     savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                                    FileUploadCoverPage.SaveAs(Server.MapPath(savedPath));
                                    CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;
                                }
                                EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));
                                //this is for ONTB/BWSSB Correspondence  email attachments
                                if (FlowName.Contains("DTL") || FlowName.Contains("EE") || FlowName.Contains("ACE") || FlowName.Contains("CE"))
                                {
                                    filepathemail = filepathemail + Server.MapPath(CoverPagePath) + ",";
                                }
                                //
                                CoverLetterUID = Guid.NewGuid().ToString();
                                int RetCount = getdata.DocumentCoverLetter_Insert_or_Update(new Guid(CoverLetterUID), new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                    DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn, DDLDocumentMedia.Items[0].Selected == true ? "true" : "false",
                                    DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                    DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus, Originator, Document_Date);
                                if (RetCount <= 0)
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                }

                                try
                                {
                                    if (File.Exists(Server.MapPath(savedPath)))
                                    {
                                        File.Delete(Server.MapPath(savedPath));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //throw
                                }
                            }
                        }

                        if (RBLUploadType.SelectedValue == "File")
                        {
                            DocumentCount = FileUploadDoc.PostedFiles.Count;
                           

                            foreach (HttpPostedFile uploadedFile in FileUploadDoc.PostedFiles)
                            {
                                //System.Threading.Thread.Sleep(2000);
                                //lblProcessMessage.Text = "Processing file " + Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                                string Extn = System.IO.Path.GetExtension(uploadedFile.FileName);
                                if (Extn.ToLower() != ".exe" && Extn.ToLower() != ".msi" && Extn.ToLower() != ".db")
                                {
                                    if (!checkDocumentExists(Path.GetFileNameWithoutExtension(uploadedFile.FileName), sDocumentUID)) // added on 20/11/2020
                                    {
                                        startdate = DateTime.Now;
                                        content = "Processing file " + Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                                        if (DDLDocumentType.SelectedItem.Text == "General Document")
                                        {
                                            DocumentFor = "General Document";
                                        }
                                        else if (DDLDocumentType.SelectedItem.Text == "Photographs")
                                        {
                                            DocumentFor = "Photographs";
                                        }
                                        else
                                        {
                                            DocumentFor = "Document";
                                        }

                                        if (Request.QueryString["tUID"] != "00000000-0000-0000-0000-000000000000")
                                        {
                                            //sDocumentPath = "~/" + projectId + "/" + workpackageid + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                                            sDocumentPath = "~/" + projectId + "/" + workpackageid + "/Documents";
                                        }
                                        else if (Request.QueryString["wUID"] != "00000000-0000-0000-0000-000000000000")
                                        {
                                            //sDocumentPath = "~/" + workpackageid + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                                            sDocumentPath = "~/" + workpackageid + "/Documents";
                                        }
                                        else if (Request.QueryString["pUID"] != "00000000-0000-0000-0000-000000000000")
                                        {
                                            //sDocumentPath = "~/" + projectId + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                                            sDocumentPath = "~/" + projectId + "/Documents";
                                        }

                                        if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                                        {
                                            Directory.CreateDirectory(Server.MapPath(sDocumentPath));

                                        }

                                        string sFileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                                        //added on 22/11/2022
                                        int RandomNo = getdata.GenerateRandomNo();

                                        string savedPath = string.Empty;
                                        if (DDLDocumentType.SelectedValue != "Photographs")
                                        {
                                            if (getdata.IsDocNameExists(workpackageid, sFileName))
                                            {
                                                savedPath = sDocumentPath + "/" + sFileName + "_" + RandomNo + "_1_copy" + Extn;
                                                //savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                                                uploadedFile.SaveAs(Server.MapPath(savedPath));
                                                CoverPagePath = sDocumentPath + "/" + sFileName + "_" + RandomNo + "_1" + Extn;
                                            }
                                            else
                                            {
                                                savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                                                uploadedFile.SaveAs(Server.MapPath(savedPath));
                                                CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;
                                            }
                                            //FileUploadDoc.SaveAs(Server.MapPath(savedPath));
                                        }
                                        else
                                        {
                                            savedPath = sDocumentPath + "/" + DateTime.Now.Ticks.ToString() + Path.GetFileName(uploadedFile.FileName);
                                            uploadedFile.SaveAs(Server.MapPath(savedPath));
                                            CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;
                                        }
                                        


                                        //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + sDocumentUID + "_" + txtDocName.Text + "_1" + "_enp" + InputFile));


                                        if (DDLDocumentType.SelectedValue != "Photographs")
                                        {
                                            EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));
                                        }
                                        //
                                        endate = DateTime.Now;
                                       
                                        TimeDuration = (endate - startdate).TotalSeconds;
                                        // added on 14/01/2022
                                        ActualDocumentUID = Guid.NewGuid();
                                        //
                                        getdata.InsertIntoDocumentUploadLog(ActualDocumentUID, startdate, endate, new Guid(Session["UserUID"].ToString()), (float)TimeDuration);
                                        //.................................
                                        string UploadFilePhysicalpath = Server.MapPath(CoverPagePath);
                                        string Flow1DisplayName = "", Flow2DisplayName = "", Flow3DisplayName = "", Flow4DisplayName = "", Flow5DisplayName = "";
                                        DataSet dsFlow = getdata.GetDocumentFlows_by_UID(new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()));
                                        if (dsFlow.Tables[0].Rows.Count > 0)
                                        {
                                            if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "-1")
                                            {
                                                Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();

                                                sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                                CDate1 = Convert.ToDateTime(sDate1);

                                                int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow1(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", savedPath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, Flow1DisplayName, RBLOriginator.SelectedValue, Document_Date, "", "", UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                if (cnt <= 0)
                                                {
                                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                }
                                            }
                                            else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "1")
                                            {
                                                Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();

                                                sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                                CDate1 = Convert.ToDateTime(sDate1);

                                                int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow1(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, Flow1DisplayName, RBLOriginator.SelectedValue, Document_Date, "", "", UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                if (cnt <= 0)
                                                {
                                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                }
                                            }
                                            else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                                            {
                                                Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                                Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();

                                                sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                                CDate1 = Convert.ToDateTime(sDate1);


                                                sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                                sDate2 = getdata.ConvertDateFormat(sDate2);
                                                CDate2 = Convert.ToDateTime(sDate2);

                                                int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow2(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                  DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                  DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                  DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                  new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, Flow1DisplayName, Flow2DisplayName, RBLOriginator.SelectedValue, Document_Date, "", "", UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                if (cnt <= 0)
                                                {
                                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                }

                                            }
                                            else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                                            {
                                                Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                                Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                                                Flow3DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();

                                                sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                                CDate1 = Convert.ToDateTime(sDate1);


                                                sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                                sDate2 = getdata.ConvertDateFormat(sDate2);
                                                CDate2 = Convert.ToDateTime(sDate2);

                                                sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                                sDate3 = getdata.ConvertDateFormat(sDate3);
                                                CDate3 = Convert.ToDateTime(sDate3);

                                                sDate4 = DateTime.Now.ToString("dd/MM/yyyy");
                                                //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                                sDate4 = getdata.ConvertDateFormat(sDate4);
                                                CDate4 = Convert.ToDateTime(CDate4);
                                                int cnt = getdata.Document_Insert_or_Update(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                               DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                               DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                               DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                               new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, new Guid(ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString()), CDate3,
                                               Flow1DisplayName, Flow2DisplayName, Flow3DisplayName, RBLOriginator.SelectedValue, Document_Date, UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                if (cnt <= 0)
                                                {
                                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                }

                                            }
                                            else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "4" && dsFlowcheck.Tables[0].Rows[0]["Type"].ToString() != "STP-OB")
                                            {
                                                Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                                Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                                                Flow3DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                                                Flow4DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString();

                                                sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                                CDate1 = Convert.ToDateTime(sDate1);


                                                sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                                sDate2 = getdata.ConvertDateFormat(sDate2);
                                                CDate2 = Convert.ToDateTime(sDate2);

                                                sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                                sDate3 = getdata.ConvertDateFormat(sDate3);
                                                CDate3 = Convert.ToDateTime(sDate3);


                                                sDate4 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                                sDate4 = getdata.ConvertDateFormat(sDate4);
                                                CDate4 = Convert.ToDateTime(sDate4);

                                                int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow4(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                  DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                  DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                  DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                  new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, new Guid(ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString()), CDate3, new Guid(ds.Tables[0].Rows[0]["FlowStep4_UserUID"].ToString()), CDate4,
                                                  Flow1DisplayName, Flow2DisplayName, Flow3DisplayName, Flow4DisplayName, RBLOriginator.SelectedValue, Document_Date, "", "", UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                if (cnt <= 0)
                                                {
                                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                }

                                            }
                                            else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "5" && dsFlowcheck.Tables[0].Rows[0]["Type"].ToString() != "STP-OB")
                                            {
                                                Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                                Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                                                Flow3DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                                                Flow4DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString();
                                                Flow5DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep5_DisplayName"].ToString();

                                                sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                                CDate1 = Convert.ToDateTime(sDate1);


                                                sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                                sDate2 = getdata.ConvertDateFormat(sDate2);
                                                CDate2 = Convert.ToDateTime(sDate2);

                                                sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                                sDate3 = getdata.ConvertDateFormat(sDate3);
                                                CDate3 = Convert.ToDateTime(sDate3);


                                                sDate4 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                                sDate4 = getdata.ConvertDateFormat(sDate4);
                                                CDate4 = Convert.ToDateTime(sDate4);

                                                sDate5 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep5_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate5 = sDate5.Split('/')[1] + "/" + sDate5.Split('/')[0] + "/" + sDate5.Split('/')[2];
                                                sDate5 = getdata.ConvertDateFormat(sDate5);
                                                CDate5 = Convert.ToDateTime(sDate5);

                                                int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow5(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                  DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                  DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                  DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                  new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, new Guid(ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString()), CDate3, new Guid(ds.Tables[0].Rows[0]["FlowStep4_UserUID"].ToString()), CDate4, new Guid(ds.Tables[0].Rows[0]["FlowStep5_UserUID"].ToString()), CDate5,
                                                  Flow1DisplayName, Flow2DisplayName, Flow3DisplayName, Flow4DisplayName, Flow5DisplayName, RBLOriginator.SelectedValue, Document_Date, "", "", UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                if (cnt <= 0)
                                                {
                                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                }
                                            }
                                            else // for all flows with step > 5 ( 6 to 20) added on 07/03/2022
                                            {

                                                int steps = int.Parse(dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString());
                                                sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                                CDate1 = Convert.ToDateTime(sDate1);


                                                sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                                sDate2 = getdata.ConvertDateFormat(sDate2);
                                                CDate2 = Convert.ToDateTime(sDate2);

                                                sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                                sDate3 = getdata.ConvertDateFormat(sDate3);
                                                CDate3 = Convert.ToDateTime(sDate3);


                                                sDate4 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                                sDate4 = getdata.ConvertDateFormat(sDate4);
                                                CDate4 = Convert.ToDateTime(sDate4);
                                                Guid User5 = Guid.NewGuid(), User6 = Guid.NewGuid();
                                                if (steps >= 5)
                                                {
                                                    sDate5 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep5_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    //sDate5 = sDate5.Split('/')[1] + "/" + sDate5.Split('/')[0] + "/" + sDate5.Split('/')[2];
                                                    sDate5 = getdata.ConvertDateFormat(sDate5);
                                                    CDate5 = Convert.ToDateTime(sDate5);
                                                    User5 = new Guid(ds.Tables[0].Rows[0]["FlowStep5_UserUID"].ToString());
                                                }

                                                if (steps >= 6)
                                                {
                                                    sDate6 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep6_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    //sDate5 = sDate5.Split('/')[1] + "/" + sDate5.Split('/')[0] + "/" + sDate5.Split('/')[2];
                                                    sDate6 = getdata.ConvertDateFormat(sDate6);
                                                    CDate6 = Convert.ToDateTime(sDate6);
                                                    User6 = new Guid(ds.Tables[0].Rows[0]["FlowStep6_UserUID"].ToString());
                                                }

                                                
                                                Guid User7 = Guid.NewGuid(), User8 = Guid.NewGuid(), User9 = Guid.NewGuid(), User10 = Guid.NewGuid(), User11 = Guid.NewGuid(), User12 = Guid.NewGuid(), User13 = Guid.NewGuid(), User14 = Guid.NewGuid(), User15 = Guid.NewGuid(), User16 = Guid.NewGuid(), User17 = Guid.NewGuid(), User18 = Guid.NewGuid(), User19 = Guid.NewGuid(), User20 = Guid.NewGuid();

                                                if (steps >= 7)
                                                {
                                                    sDate7 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep7_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    sDate7 = getdata.ConvertDateFormat(sDate7);
                                                    CDate7 = Convert.ToDateTime(sDate7);
                                                    User7 = new Guid(ds.Tables[0].Rows[0]["FlowStep7_UserUID"].ToString());
                                                }

                                                if (steps >= 8)
                                                {
                                                    sDate8 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep8_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    sDate8 = getdata.ConvertDateFormat(sDate8);
                                                    CDate8 = Convert.ToDateTime(sDate8);
                                                    User8= new Guid(ds.Tables[0].Rows[0]["FlowStep8_UserUID"].ToString());
                                                }

                                                if (steps >= 9)
                                                {
                                                    sDate9 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep9_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    sDate9 = getdata.ConvertDateFormat(sDate9);
                                                    CDate9 = Convert.ToDateTime(sDate9);
                                                    User9 = new Guid(ds.Tables[0].Rows[0]["FlowStep9_UserUID"].ToString());
                                                }

                                                if (steps >= 10)
                                                {
                                                    sDate10 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep10_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    sDate10 = getdata.ConvertDateFormat(sDate10);
                                                    CDate10 = Convert.ToDateTime(sDate10);
                                                    User10 = new Guid(ds.Tables[0].Rows[0]["FlowStep10_UserUID"].ToString());
                                                }

                                                if (steps >= 11)
                                                {
                                                    sDate11 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep11_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    sDate11 = getdata.ConvertDateFormat(sDate11);
                                                    CDate11 = Convert.ToDateTime(sDate11);
                                                    User11 = new Guid(ds.Tables[0].Rows[0]["FlowStep11_UserUID"].ToString());
                                                }

                                                if (steps >= 12)
                                                {
                                                    sDate12 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep12_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    sDate12 = getdata.ConvertDateFormat(sDate12);
                                                    CDate12 = Convert.ToDateTime(sDate12);
                                                    User12 = new Guid(ds.Tables[0].Rows[0]["FlowStep12_UserUID"].ToString());
                                                }

                                                if (steps >= 13)
                                                {
                                                    sDate13 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep13_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    sDate13 = getdata.ConvertDateFormat(sDate13);
                                                    CDate13 = Convert.ToDateTime(sDate13);
                                                    User13 = new Guid(ds.Tables[0].Rows[0]["FlowStep13_UserUID"].ToString());
                                                }

                                                if (steps >= 14)
                                                {
                                                    sDate14 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep14_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    sDate14 = getdata.ConvertDateFormat(sDate14);
                                                    CDate14 = Convert.ToDateTime(sDate14);
                                                    User14 = new Guid(ds.Tables[0].Rows[0]["FlowStep14_UserUID"].ToString());
                                                }

                                                if (steps >= 15)
                                                {
                                                    sDate15 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep15_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    sDate15 = getdata.ConvertDateFormat(sDate15);
                                                    CDate15 = Convert.ToDateTime(sDate15);
                                                    User15 = new Guid(ds.Tables[0].Rows[0]["FlowStep15_UserUID"].ToString());
                                                }

                                                if (steps >= 16)
                                                {
                                                    sDate16 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep16_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    sDate16 = getdata.ConvertDateFormat(sDate16);
                                                    CDate16 = Convert.ToDateTime(sDate16);
                                                    User16 = new Guid(ds.Tables[0].Rows[0]["FlowStep16_UserUID"].ToString());
                                                }

                                                if (steps >= 17)
                                                {
                                                    sDate17 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep17_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    sDate17 = getdata.ConvertDateFormat(sDate17);
                                                    CDate17 = Convert.ToDateTime(sDate17);
                                                    User17 = new Guid(ds.Tables[0].Rows[0]["FlowStep17_UserUID"].ToString());
                                                }

                                                if (steps >= 18)
                                                {
                                                    sDate18 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep18_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    sDate18 = getdata.ConvertDateFormat(sDate18);
                                                    CDate18 = Convert.ToDateTime(sDate18);
                                                    User18 = new Guid(ds.Tables[0].Rows[0]["FlowStep18_UserUID"].ToString());
                                                }

                                                if (steps >= 19)
                                                {
                                                    sDate19 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep19_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    sDate19 = getdata.ConvertDateFormat(sDate19);
                                                    CDate19 = Convert.ToDateTime(sDate19);
                                                    User19 = new Guid(ds.Tables[0].Rows[0]["FlowStep19_UserUID"].ToString());
                                                }

                                                if (steps >= 20)
                                                {
                                                    sDate20 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep20_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                    sDate20 = getdata.ConvertDateFormat(sDate20);
                                                    CDate20 = Convert.ToDateTime(sDate20);
                                                    User20 = new Guid(ds.Tables[0].Rows[0]["FlowStep20_UserUID"].ToString());
                                                }

                                                //if(dsFlowcheck.Tables[0].Rows[0]["Type"].ToString() == "STP-OB")
                                                //{
                                                //    txtprefnumber.Text = txtRefNumber.Text;
                                                //}

                                                int cnt = getdata.Document_Insert_or_Update_with_RelativePath_FlowAll(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                  DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                  DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                  DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                  new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, new Guid(ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString()), CDate3, new Guid(ds.Tables[0].Rows[0]["FlowStep4_UserUID"].ToString()), CDate4, User5, CDate5,
                                                  User6, CDate6,
                                                  User7, CDate7,
                                                  User8, CDate8,
                                                  User9, CDate9,
                                                  User10, CDate10,
                                                  User11, CDate11,
                                                  User12, CDate12,
                                                  User13, CDate13,
                                                  User14, CDate14,
                                                  User15, CDate15,
                                                  User16, CDate16,
                                                  User17, CDate17,
                                                  User18, CDate18,
                                                  User19, CDate19,
                                                  User20, CDate20,
                                                  RBLOriginator.SelectedValue, Document_Date, "", "", UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue,steps);
                                                if (cnt <= 0)
                                                {
                                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                }

                                                // added on 01/09/2022 for Contractor Correspondence attachments
                                                #region STP Correspondence code
                                                if (dsFlowcheck.Tables[0].Rows[0]["Type"].ToString() == "STP-C" || dsFlowcheck.Tables[0].Rows[0]["Type"].ToString() == "STP-OB")
                                                {
                                                    DataSet dscheck = getdata.getTop1_DocumentStatusSelect(ActualDocumentUID);
                                                    Guid StatusUID = ActualDocumentUID;
                                                    if(dscheck.Tables[0].Rows.Count > 0)
                                                    {
                                                        StatusUID = new Guid(dscheck.Tables[0].Rows[0]["StatusID"].ToString());
                                                    }
                                                    foreach (HttpPostedFile uploadedFileAtatch in FileAttachmentDoc.PostedFiles)
                                                    {
                                                        //System.Threading.Thread.Sleep(2000);
                                                        //lblProcessMessage.Text = "Processing file " + Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                                                        string ExtnA = System.IO.Path.GetExtension(uploadedFileAtatch.FileName);
                                                        if (ExtnA.ToLower() != ".exe" && ExtnA.ToLower() != ".msi" && ExtnA.ToLower() != ".db")
                                                        {
                                                            
                                                                

                                                                if (Request.QueryString["tUID"] != "00000000-0000-0000-0000-000000000000")
                                                                {
                                                                    //sDocumentPath = "~/" + projectId + "/" + workpackageid + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                                                                    sDocumentPath = "~/" + projectId + "/" + workpackageid + "/Attachments";
                                                                }
                                                                else if (Request.QueryString["wUID"] != "00000000-0000-0000-0000-000000000000")
                                                                {
                                                                    //sDocumentPath = "~/" + workpackageid + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                                                                    sDocumentPath = "~/" + workpackageid + "/Attachments";
                                                                }
                                                                else if (Request.QueryString["pUID"] != "00000000-0000-0000-0000-000000000000")
                                                                {
                                                                    //sDocumentPath = "~/" + projectId + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                                                                    sDocumentPath = "~/" + projectId + "/Attachments";
                                                                }

                                                                if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                                                                {
                                                                    Directory.CreateDirectory(Server.MapPath(sDocumentPath));

                                                                }
                                                                RandomNo = getdata.GenerateRandomNo();
                                                              string sFileNameatttch = Path.GetFileNameWithoutExtension(uploadedFileAtatch.FileName);

                                                                savedPath = sDocumentPath + "/" + RandomNo + Path.GetFileName(uploadedFileAtatch.FileName);
                                                                uploadedFileAtatch.SaveAs(Server.MapPath(savedPath));

                                                                CoverPagePath = sDocumentPath + "/" + sFileNameatttch + "_" + RandomNo + "_1" + Extn;
                                                                EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));
                                                            //this is for Contractor Correspondence reply to contractor email attachments
                                                            if (FlowName.Contains("DTL") || FlowName.Contains("EE") || FlowName.Contains("ACE") || FlowName.Contains("CE"))
                                                            {
                                                                filepathemail = filepathemail + Server.MapPath(CoverPagePath) + ",";
                                                            }
                                                            //

                                                            cnt = getdata.InsertDocumentsAttachments(Guid.NewGuid(), ActualDocumentUID, StatusUID, sFileNameatttch, CoverPagePath, new Guid(Session["UserUID"].ToString()), DateTime.Now);

                                                            if (cnt <= 0)
                                                            {
                                                               // Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting attachment. Please contact administrator');</script>");
                                                            }
                                                        }
                                                        }

                                                    // insert as CCTo in another table if checkbox is selected
                                                    //if (rdcorrespondenceuser.SelectedValue.Contains("Contractor"))
                                                    //{
                                                    //    chkcorrespondenceccto.Items[0].Selected = true;
                                                    //}
                                                    bool forEE = false, forACE = false, forCE = false, forAEE = false, forContractor = false, forDTL = false;
                                                    if (dsFlowcheck.Tables[0].Rows[0]["Type"].ToString() == "STP-OB")
                                                    {
                                                        foreach (ListItem listItem in chkcorrespondenceccto.Items)
                                                        {
                                                            if (listItem.Selected)
                                                            {

                                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), ActualDocumentUID, StatusUID, listItem.Value);
                                                                //need to store CC Email for ONTB/BWSSB Correspondence
                                                                if (listItem.Value == "EE")
                                                                {
                                                                    forEE = true;
                                                                }
                                                                else if (listItem.Value == "ACE")
                                                                {
                                                                    forACE = true;
                                                                }
                                                                else if (listItem.Value == "AEE")
                                                                {
                                                                    forAEE = true;
                                                                }
                                                                else if (listItem.Value == "Contractor")
                                                                {
                                                                    forContractor = true;
                                                                }
                                                                else if (listItem.Value == "DTL")
                                                                {
                                                                    forDTL = true;
                                                                }
                                                                else if (listItem.Value == "CE")
                                                                {
                                                                    forCE = true;
                                                                }
                                                                //
                                                            }
                                                        }
                                                    }
                                                    //
                                                    //
                                                    if (dsFlowcheck.Tables[0].Rows[0]["Type"].ToString() == "STP-OB")
                                                    {
                                                        if (rdcorrespondenceuser.SelectedItem.Text == "EE")
                                                        {
                                                            forEE = true;
                                                        }
                                                        else if (rdcorrespondenceuser.SelectedItem.Text == "ACE")
                                                        {
                                                            forACE = true;
                                                        }
                                                        else if (rdcorrespondenceuser.SelectedItem.Text == "CE")
                                                        {
                                                            forCE = true;
                                                        }
                                                        else if (rdcorrespondenceuser.SelectedItem.Text == "DTL")
                                                        {
                                                            forDTL = true;
                                                        }
                                                        else if (rdcorrespondenceuser.SelectedItem.Text == "AEE")
                                                        {
                                                            forAEE = true;
                                                        }
                                                        else if (rdcorrespondenceuser.SelectedItem.Text == "Contractor")
                                                        {
                                                            forContractor = true;
                                                        }
                                                        //added on 02/12/2022 add to CCto table for submission to user also for display
                                                        getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), ActualDocumentUID, StatusUID, rdcorrespondenceuser.SelectedItem.Text);
                                                    } //added on 08/12/2022
                                                    if (getdata.GetUserClientType(new Guid(workpackageid), Session["UserUID"].ToString()) != "PC")
                                                    {
                                                        DataSet dsMUSers = new DataSet();
                                                        if (FlowName == "DTL Correspondence")
                                                        {
                                                            //PC
                                                            dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 7);
                                                            if (dsMUSers.Tables[0].Rows.Count > 0)
                                                            {
                                                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                {
                                                                    CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                }
                                                            }
                                                            //ee
                                                            if (forEE)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 2);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                            //ACE
                                                            if (forACE)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 3);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                            //CE
                                                            if (forCE)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 4);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                            //AEE
                                                            if (forAEE)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 5);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                            //Contractor
                                                            if (forContractor)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 6);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }
                                                            }
                                                            //
                                                        }
                                                        else if (FlowName == "EE Correspondence")
                                                        {
                                                            //AEE
                                                            if (forAEE)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 2);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                            //ACE
                                                            if (forACE)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 3);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                            //CE
                                                            if (forCE)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 4);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                            //DTL
                                                            if (forDTL)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 5);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                            //Contractor
                                                            if (forContractor)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 6);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                        }
                                                        else if (FlowName == "ACE Correspondence")
                                                        {
                                                            //EE
                                                            if (forEE)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 2);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                            //CE
                                                            if (forCE)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 3);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                            //DTL
                                                            if (forDTL)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 4);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                            //Contractor
                                                            if (forContractor)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 5);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                        }
                                                        else if (FlowName == "CE Correspondence")
                                                        {
                                                            //EE
                                                            if (forEE)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 2);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                            //ACE
                                                            if (forACE)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 3);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                            //DTL
                                                            if (forDTL)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 4);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                            //Contractor
                                                            if (forContractor)
                                                            {
                                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, 5);
                                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                                {
                                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                                    {
                                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                                    }
                                                                }

                                                            }
                                                        }
                                                        //
                                                    }
                                                }


                                                

                                                #endregion
                                            }
                                            // store the origintaor reference no in separate table...added on 05/04/2022
                                            //getdata.InsertorUpdateRefNoHistroy(Guid.NewGuid(), ActualDocumentUID, txtRefNumber.Text, "");
                                            //changed on 07/02/2023
                                            getdata.InsertorUpdateRefNoHistroy(Guid.NewGuid(), ActualDocumentUID, txtRefNumber.Text, txtprefnumber.Text);
                                        }

                                        try
                                        {
                                            if (DDLDocumentType.SelectedValue != "Photographs")
                                            {
                                                if (File.Exists(Server.MapPath(savedPath)))
                                                {
                                                    File.Delete(Server.MapPath(savedPath));
                                                }
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            //throw
                                        }
                                    }
                                }

                            }
                        }
                        else if (RBLUploadType.SelectedValue == "Paste")
                        {
                            startdate = DateTime.Now;
                            var dList = getdata.sfinallist = (List<CopyDocumentFile>)Session["copydocument"];
                            foreach (var dUID in dList)
                            {
                                if (DDLDocumentType.SelectedItem.Text == "General Document")
                                {
                                    DocumentFor = "General Document";
                                }
                                else
                                {
                                    DocumentFor = "Document";
                                }

                                if (Request.QueryString["tUID"] != "00000000-0000-0000-0000-000000000000")
                                {
                                    //sDocumentPath = "~/" + projectId + "/" + workpackageid + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                                    sDocumentPath = "~/" + projectId + "/" + workpackageid + "/Documents";
                                }
                                else if (Request.QueryString["wUID"] != "00000000-0000-0000-0000-000000000000")
                                {
                                    sDocumentPath = "~/" + workpackageid + "/Documents";
                                }
                                else if (Request.QueryString["pUID"] != "00000000-0000-0000-0000-000000000000")
                                {
                                    //sDocumentPath = "~/" + projectId + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                                    sDocumentPath = "~/" + projectId + "/Documents";
                                }

                                if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(sDocumentPath));
                                }

                                DataSet dsdoc = getdata.ActualDocuments_SelectBy_ActualDocumentUID(dUID.DocumentUID);
                                if (dsdoc.Tables[0].Rows.Count > 0)
                                {
                                    string sFileNamewithExtn = Server.MapPath(dsdoc.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
                                    string Extn = System.IO.Path.GetExtension(sFileNamewithExtn);
                                    string sFileNamewithoutExtn = Path.GetFileNameWithoutExtension(sFileNamewithExtn);
                                    File.Copy(sFileNamewithExtn, Server.MapPath(sDocumentPath + "/" + sFileNamewithoutExtn + "_copy2" + Extn));
                                    string savedPath = sDocumentPath + "/" + sFileNamewithoutExtn + "_copy2" + Extn;
                                    CoverPagePath = sDocumentPath + "/" + sFileNamewithoutExtn + "_2" + Extn;
                                    EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));

                                    endate = DateTime.Now;
                                   
                                    TimeDuration = (endate - startdate).TotalSeconds;
                                    ActualDocumentUID = Guid.NewGuid();
                                    getdata.InsertIntoDocumentUploadLog(ActualDocumentUID, startdate, endate, new Guid(Session["UserUID"].ToString()), (float)TimeDuration);
                                    //.................................
                                    string UploadFilePhysicalpath = Server.MapPath(CoverPagePath);
                                    string Flow1DisplayName = "", Flow2DisplayName = "", Flow3DisplayName = "", Flow4DisplayName = "", Flow5DisplayName = "";
                                    DataSet dsFlow = getdata.GetDocumentFlows_by_UID(new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()));
                                    if (dsFlow.Tables[0].Rows.Count > 0)
                                    {
                                        if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "1")
                                        {
                                            Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();

                                            sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                            sDate1 = getdata.ConvertDateFormat(sDate1);
                                            CDate1 = Convert.ToDateTime(sDate1);

                                            int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow1(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                            DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileNamewithoutExtn, txtdesc.Text, 1, Extn,
                                            DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                            DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                            new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, Flow1DisplayName, RBLOriginator.SelectedValue, Document_Date, "", "", UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                            if (cnt <= 0)
                                            {
                                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                            }
                                        }
                                        else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                                        {
                                            Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                            Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();

                                            sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                            sDate1 = getdata.ConvertDateFormat(sDate1);
                                            CDate1 = Convert.ToDateTime(sDate1);


                                            sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                            sDate2 = getdata.ConvertDateFormat(sDate2);
                                            CDate2 = Convert.ToDateTime(sDate2);

                                            int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow2(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                              DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileNamewithoutExtn, txtdesc.Text, 1, Extn,
                                              DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                              DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                              new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, Flow1DisplayName, Flow2DisplayName, RBLOriginator.SelectedValue, Document_Date, "", "", UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                            if (cnt <= 0)
                                            {
                                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                            }

                                        }
                                        else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                                        {
                                            Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                            Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                                            Flow3DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();

                                            sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                            sDate1 = getdata.ConvertDateFormat(sDate1);
                                            CDate1 = Convert.ToDateTime(sDate1);


                                            sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                            sDate2 = getdata.ConvertDateFormat(sDate2);
                                            CDate2 = Convert.ToDateTime(sDate2);

                                            sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                            sDate3 = getdata.ConvertDateFormat(sDate3);
                                            CDate3 = Convert.ToDateTime(sDate3);

                                            sDate4 = DateTime.Now.ToString("dd/MM/yyyy");
                                            //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                            sDate4 = getdata.ConvertDateFormat(sDate4);
                                            CDate4 = Convert.ToDateTime(CDate4);
                                            int cnt = getdata.Document_Insert_or_Update(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                           DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileNamewithoutExtn, txtdesc.Text, 1, Extn,
                                           DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                           DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                           new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, new Guid(ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString()), CDate3,
                                           Flow1DisplayName, Flow2DisplayName, Flow3DisplayName, RBLOriginator.SelectedValue, Document_Date, UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                            if (cnt <= 0)
                                            {
                                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                            }

                                        }
                                        else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "4")
                                        {
                                            Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                            Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                                            Flow3DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                                            Flow4DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString();

                                            sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                            sDate1 = getdata.ConvertDateFormat(sDate1);
                                            CDate1 = Convert.ToDateTime(sDate1);


                                            sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                            sDate2 = getdata.ConvertDateFormat(sDate2);
                                            CDate2 = Convert.ToDateTime(sDate2);

                                            sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                            sDate3 = getdata.ConvertDateFormat(sDate3);
                                            CDate3 = Convert.ToDateTime(sDate3);


                                            sDate4 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                            sDate4 = getdata.ConvertDateFormat(sDate4);
                                            CDate4 = Convert.ToDateTime(sDate4);

                                            int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow4(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                              DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileNamewithoutExtn, txtdesc.Text, 1, Extn,
                                              DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                              DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                              new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, new Guid(ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString()), CDate3, new Guid(ds.Tables[0].Rows[0]["FlowStep4_UserUID"].ToString()), CDate4,
                                              Flow1DisplayName, Flow2DisplayName, Flow3DisplayName, Flow4DisplayName, RBLOriginator.SelectedValue, Document_Date, "", "", UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                            if (cnt <= 0)
                                            {
                                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                            }

                                        }
                                        else
                                        {
                                            Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                            Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                                            Flow3DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                                            Flow4DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString();
                                            Flow5DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep5_DisplayName"].ToString();

                                            sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                            sDate1 = getdata.ConvertDateFormat(sDate1);
                                            CDate1 = Convert.ToDateTime(sDate1);


                                            sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                            sDate2 = getdata.ConvertDateFormat(sDate2);
                                            CDate2 = Convert.ToDateTime(sDate2);

                                            sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                            sDate3 = getdata.ConvertDateFormat(sDate3);
                                            CDate3 = Convert.ToDateTime(sDate3);


                                            sDate4 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                            sDate4 = getdata.ConvertDateFormat(sDate4);
                                            CDate4 = Convert.ToDateTime(sDate4);

                                            sDate5 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep5_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                            //sDate5 = sDate5.Split('/')[1] + "/" + sDate5.Split('/')[0] + "/" + sDate5.Split('/')[2];
                                            sDate5 = getdata.ConvertDateFormat(sDate5);
                                            CDate5 = Convert.ToDateTime(sDate5);

                                            int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow5(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                              DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileNamewithoutExtn, txtdesc.Text, 1, Extn,
                                              DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                              DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                              new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, new Guid(ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString()), CDate3, new Guid(ds.Tables[0].Rows[0]["FlowStep4_UserUID"].ToString()), CDate4, new Guid(ds.Tables[0].Rows[0]["FlowStep5_UserUID"].ToString()), CDate5,
                                              Flow1DisplayName, Flow2DisplayName, Flow3DisplayName, Flow4DisplayName, Flow5DisplayName, RBLOriginator.SelectedValue, Document_Date, "", "", UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                            if (cnt <= 0)
                                            {
                                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                            }
                                        }

                                    }

                                    try
                                    {
                                        if (File.Exists(Server.MapPath(savedPath)))
                                        {
                                            File.Delete(Server.MapPath(savedPath));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //throw
                                    }
                                }
                            }

                            Session["copydocument"] = null;
                        }
                        else // upload using folder
                        {
                            if (Request.Files.Count > 0)
                            {
                                DocumentCount = Request.Files.Count;
                                HttpFileCollection attachments = Request.Files;
                                int NotSupportedFiles = 0;
                                for (int i = 0; i < attachments.Count; i++)
                                {
                                    startdate = DateTime.Now;
                                    HttpPostedFile attachment = attachments[i];
                                    string Extn = System.IO.Path.GetExtension(Path.GetFileName(attachment.FileName));
                                    // Page.ClientScript.RegisterStartupScript(this.GetType(), "Message", "DisplayMessage('Proccesed all  files !')", true);
                                    // Response.Write("All files");
                                    if (Extn.ToLower() != ".exe" && Extn.ToLower() != ".msi" && Extn.ToLower() != ".db")
                                    {
                                        if (!checkFileExists(attachment.FileName, sDocumentUID))
                                        {
                                            if (!String.IsNullOrEmpty(attachment.FileName) && FileUploadCoverPage.FileName != attachment.FileName)
                                            {
                                                //lblProcessMessage.Text = "Processing file " + Path.GetFileNameWithoutExtension(attachment.FileName);
                                                content = "Processing file " + Path.GetFileNameWithoutExtension(attachment.FileName);
                                                if (DDLDocumentType.SelectedItem.Text == "General Document")
                                                {
                                                    DocumentFor = "General Document";
                                                }
                                                else
                                                {
                                                    DocumentFor = "Document";
                                                }

                                                if (Request.QueryString["tUID"] != "00000000-0000-0000-0000-000000000000")
                                                {
                                                    //sDocumentPath = "~/" + projectId + "/" + workpackageid + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                                                    sDocumentPath = "~/" + projectId + "/" + workpackageid + "/Documents";
                                                }
                                                else if (Request.QueryString["wUID"] != "00000000-0000-0000-0000-000000000000")
                                                {
                                                    sDocumentPath = "~/" + workpackageid + "/Documents";
                                                }
                                                else if (Request.QueryString["pUID"] != "00000000-0000-0000-0000-000000000000")
                                                {
                                                    //sDocumentPath = "~/" + projectId + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                                                    sDocumentPath = "~/" + projectId + "/Documents";
                                                }

                                                if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                                                {
                                                    Directory.CreateDirectory(Server.MapPath(sDocumentPath));

                                                }
                                                string RelativePath = attachment.FileName;
                                                string[] Foldername = Path.GetDirectoryName(attachment.FileName).Split('\\');
                                                string sRes = Foldername[Foldername.Length - 1];
                                                string sFileName = Path.GetFileNameWithoutExtension(Path.GetFileName(attachment.FileName));


                                                string savedPath = string.Empty;

                                                if (checkDocumentExists(sFileName, sDocumentUID)) // added on 19/10/2020
                                                {
                                                    attachment.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                                                    //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + sDocumentUID + "_" + txtDocName.Text + "_1" + "_enp" + InputFile));
                                                    savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                                                    CoverPagePath = sDocumentPath + "/" + sFileName + "_" + DateTime.Now.Ticks + Extn;

                                                }//------------------------//
                                                else
                                                {
                                                    attachment.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                                                    //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + sDocumentUID + "_" + txtDocName.Text + "_1" + "_enp" + InputFile));
                                                    savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                                                    CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;
                                                }

                                                //FileUploadDoc.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                                                //attachment.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                                                //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + sDocumentUID + "_" + txtDocName.Text + "_1" + "_enp" + InputFile));
                                                //string savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                                                //CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;

                                                EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));
                                                //
                                                endate = DateTime.Now;
                                               
                                                TimeDuration = (endate - startdate).TotalSeconds;
                                                ActualDocumentUID = Guid.NewGuid();
                                                getdata.InsertIntoDocumentUploadLog(ActualDocumentUID, startdate, endate, new Guid(Session["UserUID"].ToString()), (float)TimeDuration);
                                                //.................................
                                                string UploadFilePhysicalpath = Server.MapPath(CoverPagePath);
                                                string Flow1DisplayName = "", Flow2DisplayName = "", Flow3DisplayName = "", Flow4DisplayName = "", Flow5DisplayName = "";
                                                DataSet dsFlow = getdata.GetDocumentFlows_by_UID(new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()));
                                                if (dsFlow.Tables[0].Rows.Count > 0)
                                                {
                                                    if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "1")
                                                    {
                                                        Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                                        sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                        sDate1 = getdata.ConvertDateFormat(sDate1);
                                                        CDate1 = Convert.ToDateTime(sDate1);

                                                        int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow1(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                        DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                        DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                        DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                        new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, Flow1DisplayName, RBLOriginator.SelectedValue, Document_Date, RelativePath, Foldername[Foldername.Length - 1], UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                        if (cnt <= 0)
                                                        {
                                                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                        }

                                                    }
                                                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                                                    {
                                                        Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                                        Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                                                        sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                        sDate1 = getdata.ConvertDateFormat(sDate1);
                                                        CDate1 = Convert.ToDateTime(sDate1);


                                                        sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                                        sDate2 = getdata.ConvertDateFormat(sDate2);
                                                        CDate2 = Convert.ToDateTime(sDate2);

                                                        int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow2(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                        DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                        DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                        DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                        new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, Flow1DisplayName, Flow2DisplayName, RBLOriginator.SelectedValue, Document_Date, RelativePath, Foldername[Foldername.Length - 1], UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                        if (cnt <= 0)
                                                        {
                                                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                        }
                                                    }
                                                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                                                    {
                                                        Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                                        Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                                                        Flow3DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();

                                                        sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                        sDate1 = getdata.ConvertDateFormat(sDate1);
                                                        CDate1 = Convert.ToDateTime(sDate1);


                                                        sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                                        sDate2 = getdata.ConvertDateFormat(sDate2);
                                                        CDate2 = Convert.ToDateTime(sDate2);

                                                        sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                                        sDate3 = getdata.ConvertDateFormat(sDate3);
                                                        CDate3 = Convert.ToDateTime(sDate3);

                                                        sDate4 = DateTime.MinValue.ToString("dd/MM/yyyy");
                                                        //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                                        sDate4 = getdata.ConvertDateFormat(sDate4);
                                                        CDate4 = Convert.ToDateTime(CDate4);

                                                        int cnt = getdata.Document_Insert_or_Update_with_RelativePath(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                        DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                        DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                        DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                        new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, new Guid(ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString()), CDate3,
                                                        Flow1DisplayName, Flow2DisplayName, Flow3DisplayName, RBLOriginator.SelectedValue, Document_Date, RelativePath, Foldername[Foldername.Length - 1], UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                        if (cnt <= 0)
                                                        {
                                                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                        }
                                                    }
                                                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "4")
                                                    {

                                                        Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                                        Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                                                        Flow3DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                                                        Flow4DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString();

                                                        sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                        sDate1 = getdata.ConvertDateFormat(sDate1);
                                                        CDate1 = Convert.ToDateTime(sDate1);


                                                        sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                                        sDate2 = getdata.ConvertDateFormat(sDate2);
                                                        CDate2 = Convert.ToDateTime(sDate2);

                                                        sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                                        sDate3 = getdata.ConvertDateFormat(sDate3);
                                                        CDate3 = Convert.ToDateTime(sDate3);

                                                        sDate4 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                                        sDate4 = getdata.ConvertDateFormat(sDate4);
                                                        CDate4 = Convert.ToDateTime(sDate4);

                                                        int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow4(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                        DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                        DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                        DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                        new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, new Guid(ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString()), CDate3, new Guid(ds.Tables[0].Rows[0]["FlowStep4_UserUID"].ToString()), CDate4,
                                                        Flow1DisplayName, Flow2DisplayName, Flow3DisplayName, Flow4DisplayName, RBLOriginator.SelectedValue, Document_Date, RelativePath, Foldername[Foldername.Length - 1], UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                        if (cnt <= 0)
                                                        {
                                                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                        }
                                                    }
                                                    else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "5")
                                                    {
                                                        Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                                        Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                                                        Flow3DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                                                        Flow4DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString();
                                                        Flow5DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep5_DisplayName"].ToString();

                                                        sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                        sDate1 = getdata.ConvertDateFormat(sDate1);
                                                        CDate1 = Convert.ToDateTime(sDate1);


                                                        sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                                        sDate2 = getdata.ConvertDateFormat(sDate2);
                                                        CDate2 = Convert.ToDateTime(sDate2);

                                                        sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                                        sDate3 = getdata.ConvertDateFormat(sDate3);
                                                        CDate3 = Convert.ToDateTime(sDate3);

                                                        sDate4 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep5_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                                        sDate4 = getdata.ConvertDateFormat(sDate4);
                                                        CDate4 = Convert.ToDateTime(sDate4);

                                                        sDate5 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep5_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate5 = sDate5.Split('/')[1] + "/" + sDate5.Split('/')[0] + "/" + sDate5.Split('/')[2];
                                                        sDate5 = getdata.ConvertDateFormat(sDate5);
                                                        CDate5 = Convert.ToDateTime(sDate5);

                                                        int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow5(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                        DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                        DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                        DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                        new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, new Guid(ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString()), CDate3, new Guid(ds.Tables[0].Rows[0]["FlowStep4_UserUID"].ToString()), CDate4, new Guid(ds.Tables[0].Rows[0]["FlowStep5_UserUID"].ToString()), CDate5,
                                                        Flow1DisplayName, Flow2DisplayName, Flow3DisplayName, Flow4DisplayName, Flow5DisplayName, RBLOriginator.SelectedValue, Document_Date, RelativePath, Foldername[Foldername.Length - 1], UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                        if (cnt <= 0)
                                                        {
                                                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                            Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
                                                        }
                                                    }
                                                    else // for all flows with step > 5 ( 6 to 20) added on 07/03/2022
                                                    {


                                                        sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                        sDate1 = getdata.ConvertDateFormat(sDate1);
                                                        CDate1 = Convert.ToDateTime(sDate1);


                                                        sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                                        sDate2 = getdata.ConvertDateFormat(sDate2);
                                                        CDate2 = Convert.ToDateTime(sDate2);

                                                        sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                                        sDate3 = getdata.ConvertDateFormat(sDate3);
                                                        CDate3 = Convert.ToDateTime(sDate3);


                                                        sDate4 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                                        sDate4 = getdata.ConvertDateFormat(sDate4);
                                                        CDate4 = Convert.ToDateTime(sDate4);

                                                        sDate5 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep5_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate5 = sDate5.Split('/')[1] + "/" + sDate5.Split('/')[0] + "/" + sDate5.Split('/')[2];
                                                        sDate5 = getdata.ConvertDateFormat(sDate5);
                                                        CDate5 = Convert.ToDateTime(sDate5);

                                                        sDate6 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep6_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                        //sDate5 = sDate5.Split('/')[1] + "/" + sDate5.Split('/')[0] + "/" + sDate5.Split('/')[2];
                                                        sDate6 = getdata.ConvertDateFormat(sDate6);
                                                        CDate6 = Convert.ToDateTime(sDate6);

                                                        int steps = int.Parse(dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString());
                                                        Guid User7 = Guid.NewGuid(), User8 = Guid.NewGuid(), User9 = Guid.NewGuid(), User10 = Guid.NewGuid(), User11 = Guid.NewGuid(), User12 = Guid.NewGuid(), User13 = Guid.NewGuid(), User14 = Guid.NewGuid(), User15 = Guid.NewGuid(), User16 = Guid.NewGuid(), User17 = Guid.NewGuid(), User18 = Guid.NewGuid(), User19 = Guid.NewGuid(), User20 = Guid.NewGuid();

                                                        if (steps >= 7)
                                                        {
                                                            sDate7 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep7_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                            sDate7 = getdata.ConvertDateFormat(sDate7);
                                                            CDate7 = Convert.ToDateTime(sDate7);
                                                            User7 = new Guid(ds.Tables[0].Rows[0]["FlowStep7_UserUID"].ToString());
                                                        }

                                                        if (steps >= 8)
                                                        {
                                                            sDate8 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep8_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                            sDate8 = getdata.ConvertDateFormat(sDate8);
                                                            CDate8 = Convert.ToDateTime(sDate8);
                                                            User8 = new Guid(ds.Tables[0].Rows[0]["FlowStep8_UserUID"].ToString());
                                                        }

                                                        if (steps >= 9)
                                                        {
                                                            sDate9 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep9_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                            sDate9 = getdata.ConvertDateFormat(sDate9);
                                                            CDate9 = Convert.ToDateTime(sDate9);
                                                            User9 = new Guid(ds.Tables[0].Rows[0]["FlowStep9_UserUID"].ToString());
                                                        }

                                                        if (steps >= 10)
                                                        {
                                                            sDate10 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep10_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                            sDate10 = getdata.ConvertDateFormat(sDate10);
                                                            CDate10 = Convert.ToDateTime(sDate10);
                                                            User10 = new Guid(ds.Tables[0].Rows[0]["FlowStep10_UserUID"].ToString());
                                                        }

                                                        if (steps >= 11)
                                                        {
                                                            sDate11 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep11_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                            sDate11 = getdata.ConvertDateFormat(sDate11);
                                                            CDate11 = Convert.ToDateTime(sDate11);
                                                            User11 = new Guid(ds.Tables[0].Rows[0]["FlowStep11_UserUID"].ToString());
                                                        }

                                                        if (steps >= 12)
                                                        {
                                                            sDate12 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep12_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                            sDate12 = getdata.ConvertDateFormat(sDate12);
                                                            CDate12 = Convert.ToDateTime(sDate12);
                                                            User12 = new Guid(ds.Tables[0].Rows[0]["FlowStep12_UserUID"].ToString());
                                                        }

                                                        if (steps >= 13)
                                                        {
                                                            sDate13 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep13_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                            sDate13 = getdata.ConvertDateFormat(sDate13);
                                                            CDate13 = Convert.ToDateTime(sDate13);
                                                            User13 = new Guid(ds.Tables[0].Rows[0]["FlowStep13_UserUID"].ToString());
                                                        }

                                                        if (steps >= 14)
                                                        {
                                                            sDate14 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep14_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                            sDate14 = getdata.ConvertDateFormat(sDate14);
                                                            CDate14 = Convert.ToDateTime(sDate14);
                                                            User14 = new Guid(ds.Tables[0].Rows[0]["FlowStep14_UserUID"].ToString());
                                                        }

                                                        if (steps >= 15)
                                                        {
                                                            sDate15 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep15_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                            sDate15 = getdata.ConvertDateFormat(sDate15);
                                                            CDate15 = Convert.ToDateTime(sDate15);
                                                            User15 = new Guid(ds.Tables[0].Rows[0]["FlowStep15_UserUID"].ToString());
                                                        }

                                                        if (steps >= 16)
                                                        {
                                                            sDate16 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep16_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                            sDate16 = getdata.ConvertDateFormat(sDate16);
                                                            CDate16 = Convert.ToDateTime(sDate16);
                                                            User16 = new Guid(ds.Tables[0].Rows[0]["FlowStep16_UserUID"].ToString());
                                                        }

                                                        if (steps >= 17)
                                                        {
                                                            sDate17 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep17_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                            sDate17 = getdata.ConvertDateFormat(sDate17);
                                                            CDate17 = Convert.ToDateTime(sDate17);
                                                            User17 = new Guid(ds.Tables[0].Rows[0]["FlowStep17_UserUID"].ToString());
                                                        }

                                                        if (steps >= 18)
                                                        {
                                                            sDate18 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep18_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                            sDate18 = getdata.ConvertDateFormat(sDate18);
                                                            CDate18 = Convert.ToDateTime(sDate18);
                                                            User18 = new Guid(ds.Tables[0].Rows[0]["FlowStep18_UserUID"].ToString());
                                                        }

                                                        if (steps >= 19)
                                                        {
                                                            sDate19 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep19_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                            sDate19 = getdata.ConvertDateFormat(sDate19);
                                                            CDate19 = Convert.ToDateTime(sDate19);
                                                            User19 = new Guid(ds.Tables[0].Rows[0]["FlowStep19_UserUID"].ToString());
                                                        }

                                                        if (steps >= 20)
                                                        {
                                                            sDate20 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep20_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                            sDate20 = getdata.ConvertDateFormat(sDate20);
                                                            CDate20 = Convert.ToDateTime(sDate20);
                                                            User20 = new Guid(ds.Tables[0].Rows[0]["FlowStep20_UserUID"].ToString());
                                                        }

                                                        int cnt = getdata.Document_Insert_or_Update_with_RelativePath_FlowAll(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                          DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                          DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                          DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                          new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, new Guid(ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString()), CDate3, new Guid(ds.Tables[0].Rows[0]["FlowStep4_UserUID"].ToString()), CDate4, new Guid(ds.Tables[0].Rows[0]["FlowStep5_UserUID"].ToString()), CDate5,
                                                          new Guid(ds.Tables[0].Rows[0]["FlowStep6_UserUID"].ToString()), CDate6,
                                                          User7, CDate7,
                                                          User8, CDate8,
                                                          User9, CDate9,
                                                          User10, CDate10,
                                                          User11, CDate11,
                                                          User12, CDate12,
                                                          User13, CDate13,
                                                          User14, CDate14,
                                                          User15, CDate15,
                                                          User16, CDate16,
                                                          User17, CDate17,
                                                          User18, CDate18,
                                                          User19, CDate19,
                                                          User20, CDate20,
                                                          RBLOriginator.SelectedValue, Document_Date, "", "", UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue, steps);
                                                        if (cnt <= 0)
                                                        {
                                                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                            Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
                                                        }
                                                    }

                                                    // store the origintaor reference no in separate table...added on 05/04/2022
                                                    //getdata.InsertorUpdateRefNoHistroy(Guid.NewGuid(), ActualDocumentUID, txtRefNumber.Text, "");
                                                    getdata.InsertorUpdateRefNoHistroy(Guid.NewGuid(), ActualDocumentUID, txtRefNumber.Text, txtprefnumber.Text);
                                                }

                                                try
                                                {
                                                    if (File.Exists(Server.MapPath(savedPath)))
                                                    {
                                                        File.Delete(Server.MapPath(savedPath));
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    //throw
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        NotSupportedFiles += 1;
                                    }

                                }
                            }
                        }

                        //Timer1.Enabled = false;
                        //inProcess = false;
                        string sHtmlString = string.Empty;
                        DataSet dsUser = new DataSet();
                        if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                        {
                            dsUser = getdata.getAllUsers();
                        }
                        else if (Session["TypeOfUser"].ToString() == "PA")
                        {
                            //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                            dsUser = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
                        }
                        else
                        {
                            dsUser = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
                        }
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            
                            string ToEmailID = "";
                            string sUserName = "";
                            string DocumentMedia = DDLDocumentMedia.Items[0].Selected == true ? "Hard Copy," : "";
                            DocumentMedia += DDLDocumentMedia.Items[1].Selected == true ? "Soft Copy(PDF)," : "";
                            DocumentMedia += DDLDocumentMedia.Items[2].Selected == true ? "Soft Copy Editable Format," : "";
                            DocumentMedia += DDLDocumentMedia.Items[3].Selected == true ? "Soft Copy Ref.," : "";
                            DocumentMedia += DDLDocumentMedia.Items[4].Selected == true ? "Hard Copy Ref.," : "";
                            DocumentMedia += DDLDocumentMedia.Items[5].Selected == true ? "No Media," : "";
                            for (int i = 0; i < dsUser.Tables[0].Rows.Count; i++)
                            {
                                if (dsUser.Tables[0].Rows[i]["UserUID"].ToString() == Session["UserUID"].ToString())
                                {
                                    ToEmailID = dsUser.Tables[0].Rows[i]["EmailID"].ToString();
                                    sUserName = dsUser.Tables[0].Rows[i]["UserName"].ToString();
                                }
                                else
                                {
                                    if (getdata.GetUserMailAccess(new Guid(dsUser.Tables[0].Rows[i]["UserUID"].ToString()), "documentmail") != 0)
                                    {
                                        CC += dsUser.Tables[0].Rows[i]["EmailID"].ToString() + ",";
                                    }
                                }
                            }
                            CC = CC.TrimEnd(',');
                            sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                   "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                      "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                            sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                               "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                            if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                            {
                                sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                            }
                            else
                            {
                                sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                            }
                            sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                               "</div>";
                            sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + sUserName + " added document's under submittal " + DDLDocuments.SelectedItem.Text + ".</span> <br/><br/></div>";
                            sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                            "<tr><td><b>Workpackage </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLWorkPackage.SelectedItem.Text + "</td></tr>" +
                                            "<tr><td><b>Subject/Description </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtdesc.Text + "</td></tr>" +
                                            "<tr><td><b>Reference Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtprefnumber.Text + "</td></tr>" +
                                             "<tr><td><b>No. of Documents </b></td><td style='text-align:center;'><b>:</b></td><td>" + DocumentCount + "</td></tr>" +
                                             "<tr><td><b>Document Media </b></td><td style='text-align:center;'><b>:</b></td><td>" + DocumentMedia.TrimEnd(',') + "</td></tr>" +
                                            "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + DateTime.Now.ToString("dd MMM yyyy") + "</td></tr>" +
                                            "<tr><td><b>Remarks </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtremarks.Text + "</td></tr>";
                            sHtmlString += "</table></div>";
                            sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";

                            //sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                            //   "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "</head>" +
                            //      "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                            //sHtmlString += "<div style='float:left; width:100%; height:30px;'>" +
                            //                   "Dear, " + "Users" +
                            //                   "<br/><br/></div>";
                            //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Below are the document details. <br/><br/></div>";
                            //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>WorkPackage : " + DDLWorkPackage.SelectedItem.Text + "<br/></div>";
                            //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Submittal Name: " + DDLDocuments.SelectedItem.Text + "<br/></div>";
                            //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Subject/Description : " + txtdesc.Text + "<br/></div>";
                            ////sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Document Type : " + ddlDocType.SelectedValue + "<br/></div>";
                            //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Date : " + DateTime.Now.ToShortDateString() + "<br/></div>";
                            //sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";
                            //string ret = getdata.SendMail(ds.Tables[0].Rows[0]["EmailID"].ToString(), Subject, sHtmlString, CC, Server.MapPath(CoverPagePath));
                            // added on 02/11/2020
                            DataTable dtemailCred = getdata.GetEmailCredentials();
                            Guid MailUID = Guid.NewGuid();
                            if (ToEmailID == "")
                            {
                                ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(Session["UserUID"].ToString()));
                            }
                            
                            getdata.StoreEmaildataToMailQueue(MailUID, new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, "Documents Uploaded !", sHtmlString, CC, filepathemail);
                            
                             //
                                // added on 07/01/2022 for sending mail to next user in line to change status...
                                // if (dsFlowcheck.Tables[0].Rows[0]["Type"].ToString() != "STP-OB")
                                //   {
                                DataSet dsnew = getdata.getTop1_DocumentStatusSelect(ActualDocumentUID);
                                DataSet dsNext = getdata.GetNextStep_By_DocumentUID(ActualDocumentUID, dsnew.Tables[0].Rows[0]["ActivityType"].ToString());

                                sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                       "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                          "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                   "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                }
                                else
                                {
                                    sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                }
                                sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                                   "</div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + sUserName + " added document's under submittal " + DDLDocuments.SelectedItem.Text + ".</span> <br/><br/></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                "<tr><td><b>Workpackage </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLWorkPackage.SelectedItem.Text + "</td></tr>" +
                                                "<tr><td><b>Subject/Description </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtdesc.Text + "</td></tr>" +
                                                "<tr><td><b>Reference Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtprefnumber.Text + "</td></tr>" +
                                                 "<tr><td><b>No. of Documents </b></td><td style='text-align:center;'><b>:</b></td><td>" + DocumentCount + "</td></tr>" +
                                                 "<tr><td><b>Document Media </b></td><td style='text-align:center;'><b>:</b></td><td>" + DocumentMedia.TrimEnd(',') + "</td></tr>" +
                                                "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + DateTime.Now.ToString("dd MMM yyyy") + "</td></tr>" +
                                                "<tr><td><b>Remarks </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtremarks.Text + "</td></tr>";
                                sHtmlString += "</table><br /><br /><div style='color: red'>Kindly note that you are to act on this to complete the next step in document flow.</div></div>";
                                sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";

                                string next = string.Empty;
                                DataSet dsNxtUser = new DataSet();
                                foreach (DataRow dr in dsNext.Tables[0].Rows)
                                {
                                    dsNxtUser = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, int.Parse(dr["ForFlow_Step"].ToString()));
                                    foreach (DataRow druser in dsNxtUser.Tables[0].Rows)
                                    {
                                        ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString()));
                                        if (!next.Contains(ToEmailID))
                                        {
                                            getdata.StoreEmaildataToMailQueue(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, "Documents Uploaded !.Kindly complete the next step !", sHtmlString, "", "");
                                            next += ToEmailID;
                                        }

                                    }
                                }
                                //
                            }
                        }
                  //  }

                    btnSubmit.Enabled = true;
                    Session["SelectedTaskUID1"] = sDocumentUID;
                    if (Request.QueryString["sType"] != null)
                    {
                        Session["ViewDocBy"] = Request.QueryString["sType"].ToString();
                    }
                    else
                    {
                        Session["ViewDocBy"] = "Activity";
                    }
                    //changed on 12/09/2022
                    // Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    divMain.Visible = false;
                    divUploadmsg.Visible = true;
                    btnSubmit.Visible = false;
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
                    btnSubmit.Enabled = true;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('" + ex.Message + "');</script>");
                }


            }
        }

        private string DirectoryFileSearch(string dir, string projectId, string workpackageid, string sDocumentUID)
        {
            string retStr = string.Empty;
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                {
                    DataSet ds = getdata.getDocumentsbyDocID(new Guid(sDocumentUID));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string sDate1 = "", sDate2 = "", sDate3 = "", sDate4 = "";
                        DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now, CDate4 = DateTime.Now;

                        string DocumentFor = string.Empty;
                        string sDocumentPath = string.Empty;
                        string cStatus = "Submitted";
                        string FileDatetime = DateTime.Now.ToString("dd MMM yyyy hh-mm-ss tt");
                        string CoverPagePath = "";
                        string IncomingRec_Date_String = string.Empty;
                        string RelativePath = Path.GetDirectoryName(f);
                        if ((dtIncomingRevDate.FindControl("txtDate") as TextBox).Text != "")
                        {
                            IncomingRec_Date_String = (dtIncomingRevDate.FindControl("txtDate") as TextBox).Text;
                        }
                        else
                        {
                            IncomingRec_Date_String = DateTime.MinValue.ToString("dd/MM/yyyy");
                        }
                        //IncomingRec_Date_String = IncomingRec_Date_String.Split('/')[1] + "/" + IncomingRec_Date_String.Split('/')[0] + "/" + IncomingRec_Date_String.Split('/')[2];
                        IncomingRec_Date_String = getdata.ConvertDateFormat(IncomingRec_Date_String);
                        DateTime IncomingRec_Date = Convert.ToDateTime(IncomingRec_Date_String);
                        if (DDLDocumentType.SelectedItem.Text == "General Document")
                        {
                            DocumentFor = "General Document";
                        }
                        else
                        {
                            DocumentFor = "Document";
                        }

                        if (Request.QueryString["tUID"] != "00000000-0000-0000-0000-000000000000")
                        {
                            sDocumentPath = "~/" + projectId + "/" + workpackageid + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                        }
                        else if (Request.QueryString["wUID"] != "00000000-0000-0000-0000-000000000000")
                        {
                            sDocumentPath = "~/" + workpackageid + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                        }
                        else if (Request.QueryString["pUID"] != "00000000-0000-0000-0000-000000000000")
                        {
                            sDocumentPath = "~/" + projectId + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                        }

                        if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                        {
                            Directory.CreateDirectory(Server.MapPath(sDocumentPath));

                        }

                        string sFileName = Path.GetFileNameWithoutExtension(f);
                        string Extn = System.IO.Path.GetExtension(f);
                        string Foldername = Path.GetDirectoryName(f);
                        FileUploadDoc.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileName + "_1_copy" + Extn));
                        //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + sDocumentUID + "_" + txtDocName.Text + "_1"  + "_enp" + InputFile));
                        string savedPath = sDocumentPath + "/" + sFileName + "_1_copy" + Extn;
                        CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;

                        EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));
                        string UploadFilePhysicalpath = Server.MapPath(CoverPagePath);
                        string Flow1DisplayName = "", Flow2DisplayName = "", Flow3DisplayName = "";
                        DataSet dsFlow = getdata.GetDocumentFlows_by_UID(new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()));
                        if (dsFlow.Tables[0].Rows.Count > 0)
                        {
                            Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                            Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                            Flow3DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                        }
                        //
                        sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                        sDate1 = getdata.ConvertDateFormat(sDate1);
                        CDate1 = Convert.ToDateTime(sDate1);
                        //

                        sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                        sDate2 = getdata.ConvertDateFormat(sDate2);
                        CDate2 = Convert.ToDateTime(sDate2);

                        sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                        //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                        sDate3 = getdata.ConvertDateFormat(sDate3);
                        CDate3 = Convert.ToDateTime(sDate3);

                        sDate4 = DateTime.MinValue.ToString("dd/MM/yyyy");
                        //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                        sDate4 = getdata.ConvertDateFormat(sDate4);
                        CDate4 = Convert.ToDateTime(CDate4);

                        int cnt = getdata.Document_Insert_or_Update_with_RelativePath(Guid.NewGuid(), new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                    DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                    DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                    DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                    new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, new Guid(ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString()), CDate3,
                                    Flow1DisplayName, Flow2DisplayName, Flow3DisplayName, RBLOriginator.SelectedValue, CDate4, RelativePath, Foldername, UploadFilePhysicalpath, "", RBLSubmissionType.SelectedValue);
                        if (cnt <= 0)
                        {
                            retStr = "Error : while inserting Documents";
                        }
                    }
                }
                foreach (string d in Directory.GetDirectories(dir))
                {
                    DirectoryFileSearch(d, projectId, workpackageid, sDocumentUID);
                }
            }
            catch (Exception ex)
            {
                retStr = "Error : " + ex.Message;
            }
            return retStr;
        }

        private void EncryptFile(string inputFile, string outputFile)
        {

            try
            {
                string password = @"myKey123"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                //MessageBox.Show("Encryption failed!", "Error");
            }
        }

        protected void DDLDocumentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LblDocType.Text = DDLDocumentType.SelectedItem.Text;
            if (DDLDocumentType.SelectedItem.Text == "Cover Letter")
            {
                CoverLetterUpload.Visible = true;
                Originator.Visible = true;
                OriginatorNumber.Visible = true;
                DocumentDate.Visible = true;
                LblDateText.Text = "Incoming Recv. Date";
            }
            else if (DDLDocumentType.SelectedItem.Text == "General Document")
            {
                CoverLetterUpload.Visible = false;
                Originator.Visible = false;
                DocumentDate.Visible = true;
                OriginatorNumber.Visible = false;
                LblDateText.Text = "Incoming Recv. Date";
            }
            else
            {
                CoverLetterUpload.Visible = false;
                Originator.Visible = false;
                DocumentDate.Visible = false;
                OriginatorNumber.Visible = false;
                LblDateText.Text = "Incoming Recv. Date";
            }

            //    //DocFlow.Visible = false;
            //    //S1Display.Visible = false;
            //    //S1Date.Visible = false;
            //    //S2Display.Visible = false;
            //    //S2Date.Visible = false;
            //    //S3Display.Visible = false;
            //    //S3Date.Visible = false;
            //    //S4Display.Visible = false;
            //    //S4Date.Visible = false;
            //    //S5Display.Visible = false;
            //    //S5Date.Visible = false;
            //    //DocStartDate.Visible = false;
            //}
            //else
            //{
            //    //DocFlow.Visible = true;
            //    //DocStartDate.Visible = true;
            //    //DocumentFlowChanged();
            //}
            //

            string FlowName = getdata.GetFlowName_by_SubmittalID(new Guid(Request.QueryString["dUID"]));
            if (FlowName == "Flow 3" || FlowName.Contains("Correspondence"))
            {
                LblDocType.Text = "Letter";
            }
        }

        //private void BindDocument_Type_Master()
        //{
        //    DataSet ds = getdata.GetDocument_For_Master();
        //    DDLDocumentType.DataTextField = "DocumentFor_Text";
        //    DDLDocumentType.DataValueField = "DocumentForUID";
        //    DDLDocumentType.DataSource = ds;
        //    DDLDocumentType.DataBind();
        //}

        private void GetDocumentFolder(Guid DocumentUID)
        {
            DataSet ds = getdata.getDocumentsbyDocID(DocumentUID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //string dType = getdata.GetDocumentType_By_Text(ds.Tables[0].Rows[0]["Doc_Type"].ToString());

                //DDLDocumentFlow.SelectedValue = ds.Tables[0].Rows[0]["FlowUID"].ToString();
                //if (ds.Tables[0].Rows[0]["Flow_StartDate"].ToString() != null && ds.Tables[0].Rows[0]["Flow_StartDate"].ToString() != "")
                //{
                //    (dtStartDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Flow_StartDate"].ToString()).ToString("dd/MM/yyyy");
                //}

                if (ds.Tables[0].Rows[0]["Doc_Type"].ToString() == "Cover Letter")
                {
                    //DDLDocumentType.Enabled = false;
                    //DocFlow.Visible = false;
                    //S1Display.Visible = false;
                    //S1Date.Visible = false;
                    //S2Display.Visible = false;
                    //S2Date.Visible = false;
                    //S3Display.Visible = false;
                    //S3Date.Visible = false;
                    //S4Display.Visible = false;
                    //S4Date.Visible = false;
                    //S5Display.Visible = false;
                    //S5Date.Visible = false;
                    //DocStartDate.Visible = false;
                    DDLDocumentType.SelectedIndex = 0;
                }
                else if (ds.Tables[0].Rows[0]["Doc_Type"].ToString() == "General Document")
                {
                    //DDLDocumentType.Enabled = false;
                    //DocFlow.Visible = false;
                    //S1Display.Visible = false;
                    //S1Date.Visible = false;
                    //S2Display.Visible = false;
                    //S2Date.Visible = false;
                    //S3Display.Visible = false;
                    //S3Date.Visible = false;
                    //S4Display.Visible = false;
                    //S4Date.Visible = false;
                    //S5Display.Visible = false;
                    //S5Date.Visible = false;
                    //DocStartDate.Visible = false;
                    DDLDocumentType.SelectedIndex = 1;
                }
                else
                {
                    //DocFlow.Visible = true;
                    //DocStartDate.Visible = true;
                    //DocumentFlowChanged();
                    //DDLDocumentType.Enabled = true;
                    DDLDocumentType.SelectedIndex = 0;
                    //DataSet dsflow = getdata.GetDocumentFlows_by_UID(new Guid(DDLDocumentFlow.SelectedValue));
                    //if (dsflow.Tables[0].Rows.Count > 0)
                    //{
                    //    if (dsflow.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                    //    {
                    //        lblStep1Display.Text = dsflow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                    //        lblStep1Date.Text = dsflow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                    //        lblStep2Display.Text = dsflow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                    //        lblStep2Date.Text = dsflow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                    //        if (ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString() != "")
                    //        {
                    //            ddlSubmissionUSer.SelectedValue = ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString();
                    //            (dtSubTargetDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                    //        }
                    //        if (ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString() != "")
                    //        {
                    //            ddlQualityEngg.SelectedValue = ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString();
                    //            (dtQualTargetDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                    //        }
                    //        S1Display.Visible = true;
                    //        S1Date.Visible = true;
                    //        S2Display.Visible = true;
                    //        S2Date.Visible = true;
                    //        S3Display.Visible = false;
                    //        S3Date.Visible = false;
                    //        S4Display.Visible = false;
                    //        S4Date.Visible = false;
                    //        S5Display.Visible = false;
                    //        S5Date.Visible = false;
                    //    }
                    //    else if (dsflow.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                    //    {
                    //        lblStep1Display.Text = dsflow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                    //        lblStep1Date.Text = dsflow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                    //        lblStep2Display.Text = dsflow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                    //        lblStep2Date.Text = dsflow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                    //        lblStep3Display.Text = dsflow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                    //        lblStep3Date.Text = dsflow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString() + " Target Date";

                    //        if (ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString() != "")
                    //        {
                    //            ddlSubmissionUSer.SelectedValue = ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString();
                    //            (dtSubTargetDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                    //        }
                    //        if (ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString() != "")
                    //        {
                    //            ddlQualityEngg.SelectedValue = ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString();
                    //            (dtQualTargetDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                    //        }
                    //        if (ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString() != "")
                    //        {
                    //            ddlReviewer_B.SelectedValue = ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString();
                    //            (dtRev_B_TargetDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                    //        }

                    //        S1Display.Visible = true;
                    //        S1Date.Visible = true;
                    //        S2Display.Visible = true;
                    //        S2Date.Visible = true;
                    //        S3Display.Visible = true;
                    //        S3Date.Visible = true;
                    //        S4Display.Visible = false;
                    //        S4Date.Visible = false;
                    //        S5Display.Visible = false;
                    //        S5Date.Visible = false;
                    //    }
                    //    else if (dsflow.Tables[0].Rows[0]["Steps_Count"].ToString() == "4")
                    //    {
                    //        lblStep1Display.Text = dsflow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                    //        lblStep1Date.Text = dsflow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                    //        lblStep2Display.Text = dsflow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                    //        lblStep2Date.Text = dsflow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                    //        lblStep3Display.Text = dsflow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                    //        lblStep3Date.Text = dsflow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString() + " Target Date";
                    //        lblStep4Display.Text = dsflow.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString();
                    //        lblStep4Date.Text = dsflow.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString() + " Target Date";
                    //        if (ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString() != "")
                    //        {
                    //            ddlSubmissionUSer.SelectedValue = ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString();
                    //            (dtSubTargetDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                    //        }
                    //        if (ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString() != "")
                    //        {
                    //            ddlQualityEngg.SelectedValue = ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString();
                    //            (dtQualTargetDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                    //        }
                    //        if (ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString() != "")
                    //        {
                    //            ddlReviewer_B.SelectedValue = ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString();
                    //            (dtRev_B_TargetDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                    //        }
                    //        if (ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString() != "")
                    //        {
                    //            ddlReviewer.SelectedValue = ds.Tables[0].Rows[0]["FlowStep4_UserUID"].ToString();
                    //            (dtRevTargetDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                    //        }

                    //        S1Display.Visible = true;
                    //        S1Date.Visible = true;
                    //        S2Display.Visible = true;
                    //        S2Date.Visible = true;
                    //        S3Display.Visible = true;
                    //        S3Date.Visible = true;
                    //        S4Display.Visible = true;
                    //        S4Date.Visible = true;
                    //        S5Display.Visible = false;
                    //        S5Date.Visible = false;
                    //    }
                    //    else
                    //    {
                    //        lblStep1Display.Text = dsflow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                    //        lblStep1Date.Text = dsflow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString() + " Target Date";
                    //        lblStep2Display.Text = dsflow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                    //        lblStep2Date.Text = dsflow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString() + " Target Date";
                    //        lblStep3Display.Text = dsflow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();
                    //        lblStep3Date.Text = dsflow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString() + " Target Date";
                    //        lblStep4Display.Text = dsflow.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString();
                    //        lblStep4Date.Text = dsflow.Tables[0].Rows[0]["FlowStep4_DisplayName"].ToString() + " Target Date";
                    //        lblStep5Display.Text = dsflow.Tables[0].Rows[0]["FlowStep5_DisplayName"].ToString();
                    //        lblStep5Date.Text = dsflow.Tables[0].Rows[0]["FlowStep5_DisplayName"].ToString() + " Target Date";
                    //        if (ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString() != "")
                    //        {
                    //            ddlSubmissionUSer.SelectedValue = ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString();
                    //            (dtSubTargetDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                    //        }
                    //        if (ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString() != "")
                    //        {
                    //            ddlQualityEngg.SelectedValue = ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString();
                    //            (dtQualTargetDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                    //        }
                    //        if (ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString() != "")
                    //        {
                    //            ddlReviewer_B.SelectedValue = ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString();
                    //            (dtRev_B_TargetDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                    //        }
                    //        if (ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString() != "")
                    //        {
                    //            ddlReviewer.SelectedValue = ds.Tables[0].Rows[0]["FlowStep4_UserUID"].ToString();
                    //            (dtRevTargetDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep4_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                    //        }
                    //        if (ds.Tables[0].Rows[0]["FlowStep5_TargetDate"].ToString() != null && ds.Tables[0].Rows[0]["FlowStep5_TargetDate"].ToString() != "")
                    //        {
                    //            ddlApproval.SelectedValue = ds.Tables[0].Rows[0]["FlowStep5_UserUID"].ToString();
                    //            (dtAppTargetDate.FindControl("txtDate") as TextBox).Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep5_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                    //        }
                    //    }
                    //}
                }

            }
        }

        protected void RBLOriginator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBLOriginator.SelectedValue == "Contractor")
            {
                LblDateText.Text = "Incoming Recv. Date";
            }
            else if (RBLOriginator.SelectedValue == "JUIDCo")
            {
                LblDateText.Text = "Letter Dated";
            }
            else if (RBLOriginator.SelectedValue == "NJSEI")
            {
                LblDateText.Text = "Reply Date";
            }
            else
            {
                LblDateText.Text = "Incoming Recv. Date";
            }
        }

        protected void RBLUploadType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBLUploadType.SelectedValue == "Folder")
            {
                UploadFolder.Visible = true;
                UploadFile.Visible = false;
                CopiedDocuments.Visible = false;
            }
            else if (RBLUploadType.SelectedValue == "File")
            {
                UploadFolder.Visible = false;
                UploadFile.Visible = true;
                CopiedDocuments.Visible = false;
            }
            else
            {
                lstDocuments.DataTextField = "DocumentName";
                lstDocuments.DataValueField = "DocumentUID";
                lstDocuments.DataSource= getdata.sfinallist = (List<CopyDocumentFile>)Session["copydocument"];
                lstDocuments.DataBind();
                CopiedDocuments.Visible = true;
                UploadFolder.Visible = false;
                UploadFile.Visible = false;
            }
        }


        // added on 16/10/2020 zuber

        private bool checkFileExists(string filename, string sDocumentUID)
        {
            bool result = false;
            DataSet dsfiles = new DataSet();
            try
            {
                dsfiles = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(sDocumentUID));
                foreach (DataRow dr in dsfiles.Tables[0].Rows)
                {
                    if (dr["ActualDocument_RelativePath"].ToString() == filename)
                    {
                        result = true;
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        // added on 19/10/2020 zuber
        private bool checkDocumentExists(string filename, string sDocumentUID)
        {
            bool result = false;
            DataSet dsfiles = new DataSet();
            try
            {
                dsfiles = getdata.ActualDocuments_SelectBy_DocumentUID(new Guid(sDocumentUID));
                foreach (DataRow dr in dsfiles.Tables[0].Rows)
                {
                    if (dr["ActualDocument_Name"].ToString() == filename)
                    {
                        result = true;
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        protected void btnSubmitOlddoc_Click(object sender, EventArgs e)
        {
            bool DocExists = false;
            int DocumentCount = 0;

            string FlowName = getdata.GetFlowName_by_SubmittalID(new Guid(Request.QueryString["dUID"]));

            if (dtIncomingRevDate.Text != "" && dtDocumentDate.Text != "")
            {

                DateTime CheckIncomingRec_Date = Convert.ToDateTime(getdata.ConvertDateFormat(dtIncomingRevDate.Text));
                DateTime CheckDocumentDate = Convert.ToDateTime(getdata.ConvertDateFormat(dtDocumentDate.Text));

                if (CheckDocumentDate > CheckIncomingRec_Date)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Document Date connot be greater than Incoming Receive date.');</script>");
                    return;
                }
            }

            
            //----------------------------------------------------
            
                
                try
                {
                    btnSubmit.Enabled = false;
                    Guid ActualDocumentUID = Guid.NewGuid();
                    string projectId = Request.QueryString["pUID"].ToString();
                    string workpackageid = Request.QueryString["wUID"].ToString();
                    string sDocumentUID = Request.QueryString["dUID"].ToString();
                    string sDate1 = "", sDate2 = "", sDate3 = "", sDate4 = "", sDate5 = "", CoverPagePath = "", sDate6 = "", sDate7 = "", sDate8 = "", sDate9 = "", sDate10 = "", sDate11 = "", sDate12 = "", sDate13 = "", sDate14 = "", sDate15 = "", sDate16 = "", sDate17 = "", sDate18 = "", sDate19 = "", sDate20 = "";
                    DateTime CDate1 = DateTime.Now, CDate2 = DateTime.Now, CDate3 = DateTime.Now, CDate4 = DateTime.Now, CDate5 = DateTime.Now, DocStartDate = DateTime.Now, CDate6 = DateTime.Now, CDate7 = DateTime.Now, CDate8 = DateTime.Now, CDate9 = DateTime.Now, CDate10 = DateTime.Now, CDate11 = DateTime.Now, CDate12 = DateTime.Now, CDate13 = DateTime.Now, CDate14 = DateTime.Now, CDate15 = DateTime.Now, CDate16 = DateTime.Now, CDate17 = DateTime.Now, CDate18 = DateTime.Now, CDate19 = DateTime.Now, CDate20 = DateTime.Now;
                    DataSet ds = getdata.getDocumentsbyDocID(new Guid(Request.QueryString["dUID"]));
                    DateTime startdate;
                    DateTime endate;
                    double TimeDuration = 0;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string cStatus = "Submitted";
                        DataSet dsFlowcheck = getdata.GetDocumentFlows_by_UID(new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()));
                       
                        string FileDatetime = DateTime.Now.ToString("dd MMM yyyy hh-mm-ss tt");
                        string sDocumentPath = string.Empty;
                        string DocumentFor = "";

                        string IncomingRec_Date_String = string.Empty;
                        if (dtIncomingRevDate.Text != "")
                        {
                            IncomingRec_Date_String = dtIncomingRevDate.Text;
                        }
                        else
                        {
                            //IncomingRec_Date_String = DateTime.Now.ToString("dd/MM/yyyy");
                            IncomingRec_Date_String = dtDocumentDate.Text;
                        }
                        //IncomingRec_Date_String = IncomingRec_Date_String.Split('/')[1] + "/" + IncomingRec_Date_String.Split('/')[0] + "/" + IncomingRec_Date_String.Split('/')[2];
                        IncomingRec_Date_String = getdata.ConvertDateFormat(IncomingRec_Date_String);
                        DateTime IncomingRec_Date = Convert.ToDateTime(IncomingRec_Date_String);

                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar()", true);
                        string DocumentDate = string.Empty;

                        if (dtDocumentDate.Text != "")
                        {
                            DocumentDate = dtDocumentDate.Text;
                        }
                        else
                        {
                            DocumentDate = DateTime.MinValue.ToString("MM/dd/yyyy");
                        }
                        DocumentDate = getdata.ConvertDateFormat(DocumentDate);
                        DateTime Document_Date = Convert.ToDateTime(DocumentDate);
                        string CoverLetterUID = "";
                       
                    //Cover letter upload
                            string Originator = string.Empty;
                            //string DocumentDate = string.Empty;
                            string Extn = ".pdf";
                            if (Extn.ToLower() != ".exe" && Extn.ToLower() != ".msi" && Extn.ToLower() != ".db")
                            {
                                if (DDLDocumentType.SelectedItem.Text == "Cover Letter")
                                {
                                    DocumentFor = "Cover Letter";
                                    Originator = RBLOriginator.SelectedValue;
                                    if (dtDocumentDate.Text != "")
                                    {
                                        DocumentDate = dtDocumentDate.Text;
                                    }
                                    else
                                    {
                                        DocumentDate = DateTime.MinValue.ToString("MM/dd/yyyy");
                                    }
                                }
                                else
                                {
                                    DocumentFor = "General Document";
                                    //DocumentDate = DateTime.MinValue.ToString("MM/dd/yyyy");
                                    if (dtDocumentDate.Text != "")
                                    {
                                        DocumentDate = dtDocumentDate.Text;
                                    }
                                    else
                                    {
                                        DocumentDate = DateTime.MinValue.ToString("MM/dd/yyyy");
                                    }
                                }

                                //DocumentDate = DocumentDate.Split('/')[1] + "/" + DocumentDate.Split('/')[0] + "/" + DocumentDate.Split('/')[2];
                                //DocumentDate = getdata.ConvertDateFormat(DocumentDate);
                                //DateTime Document_Date = Convert.ToDateTime(DocumentDate);

                                if (Request.QueryString["tUID"] != "00000000-0000-0000-0000-000000000000")
                                {
                                    //sDocumentPath = "~/" + projectId + "/" + workpackageid + "/" + sDocumentUID + "/" + FileDatetime + "/CoverLetter";
                                    sDocumentPath = "~/" + projectId + "/" + workpackageid + "/CoverLetter";
                                }
                                else if (Request.QueryString["wUID"] != "00000000-0000-0000-0000-000000000000")
                                {
                                    //sDocumentPath = "~/" + workpackageid + "/" + sDocumentUID + "/" + FileDatetime + "/CoverLetter";
                                    sDocumentPath = "~/" + workpackageid + "/CoverLetter";
                                }
                                else if (Request.QueryString["pUID"] != "00000000-0000-0000-0000-000000000000")
                                {
                                    //sDocumentPath = "~/" + projectId + "/" + sDocumentUID + "/" + FileDatetime + "/CoverLetter";
                                    sDocumentPath = "~/" + projectId + "/CoverLetter";
                                }
                                if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(sDocumentPath));
                                }

                                string sFileName = "Dummy_Cover Letter";
                                string savedPath = "~/dummydocs/Dummy_Cover Letter.pdf";
                                CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;
                                EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));
                                CoverLetterUID = Guid.NewGuid().ToString();
                                int RetCount = getdata.DocumentCoverLetter_Insert_or_Update(new Guid(CoverLetterUID), new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                    DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn, DDLDocumentMedia.Items[0].Selected == true ? "true" : "false",
                                    DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                    DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus, Originator, Document_Date);
                                if (RetCount <= 0)
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                }

                                try
                                {
                                    //if (File.Exists(Server.MapPath(savedPath)))
                                    //{
                                    //    File.Delete(Server.MapPath(savedPath));
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    //throw
                                }
                            }
                        

                        if (RBLUploadType.SelectedValue == "File")
                        {
                           


                            
                                //System.Threading.Thread.Sleep(2000);
                                //lblProcessMessage.Text = "Processing file " + Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                                Extn = ".pdf";
                                if (Extn.ToLower() != ".exe" && Extn.ToLower() != ".msi" && Extn.ToLower() != ".db")
                                {
                                   
                                        startdate = DateTime.Now;
                                       
                                        if (DDLDocumentType.SelectedItem.Text == "General Document")
                                        {
                                            DocumentFor = "General Document";
                                        }
                                        else if (DDLDocumentType.SelectedItem.Text == "Photographs")
                                        {
                                            DocumentFor = "Photographs";
                                        }
                                        else
                                        {
                                            DocumentFor = "Document";
                                        }

                                        if (Request.QueryString["tUID"] != "00000000-0000-0000-0000-000000000000")
                                        {
                                            //sDocumentPath = "~/" + projectId + "/" + workpackageid + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                                            sDocumentPath = "~/" + projectId + "/" + workpackageid + "/Documents";
                                        }
                                        else if (Request.QueryString["wUID"] != "00000000-0000-0000-0000-000000000000")
                                        {
                                            //sDocumentPath = "~/" + workpackageid + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                                            sDocumentPath = "~/" + workpackageid + "/Documents";
                                        }
                                        else if (Request.QueryString["pUID"] != "00000000-0000-0000-0000-000000000000")
                                        {
                                            //sDocumentPath = "~/" + projectId + "/" + sDocumentUID + "/" + FileDatetime + "/Documents";
                                            sDocumentPath = "~/" + projectId + "/Documents";
                                        }

                                        if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                                        {
                                            Directory.CreateDirectory(Server.MapPath(sDocumentPath));

                                        }

                                        string sFileName = "Dummy_Document";

                                        string savedPath = "~/dummydocs/Dummy_Document.pdf";
                                       
                                        CoverPagePath = sDocumentPath + "/" + sFileName + "_1" + Extn;


                                        //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + sDocumentUID + "_" + txtDocName.Text + "_1" + "_enp" + InputFile));


                                        if (DDLDocumentType.SelectedValue != "Photographs")
                                        {
                                            EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));
                                        }
                                        //
                                        endate = DateTime.Now;

                                        TimeDuration = (endate - startdate).TotalSeconds;
                                        // added on 14/01/2022
                                        ActualDocumentUID = Guid.NewGuid();
                                        //
                                        getdata.InsertIntoDocumentUploadLog(ActualDocumentUID, startdate, endate, new Guid(Session["UserUID"].ToString()), (float)TimeDuration);
                                        //.................................
                                        string UploadFilePhysicalpath = Server.MapPath(CoverPagePath);
                                        string Flow1DisplayName = "", Flow2DisplayName = "", Flow3DisplayName = "", Flow4DisplayName = "", Flow5DisplayName = "";
                                        DataSet dsFlow = getdata.GetDocumentFlows_by_UID(new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()));
                                        if (dsFlow.Tables[0].Rows.Count > 0)
                                        {
                                            if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "-1")
                                            {
                                                Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();

                                                sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                                CDate1 = Convert.ToDateTime(sDate1);

                                                int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow1(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", savedPath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, Flow1DisplayName, RBLOriginator.SelectedValue, Document_Date, "", "", UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                if (cnt <= 0)
                                                {
                                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                }
                                            }
                                            else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "1")
                                            {
                                                Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();

                                                sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                                CDate1 = Convert.ToDateTime(sDate1);

                                                int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow1(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, Flow1DisplayName, RBLOriginator.SelectedValue, Document_Date, "", "", UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                if (cnt <= 0)
                                                {
                                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                }
                                            }
                                            else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "2")
                                            {
                                                Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                                Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();

                                                sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                                CDate1 = Convert.ToDateTime(sDate1);


                                                sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                                sDate2 = getdata.ConvertDateFormat(sDate2);
                                                CDate2 = Convert.ToDateTime(sDate2);

                                                int cnt = getdata.Document_Insert_or_Update_with_RelativePath_Flow2(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                                  DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                                  DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                                  DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                                  new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, Flow1DisplayName, Flow2DisplayName, RBLOriginator.SelectedValue, Document_Date, "", "", UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                if (cnt <= 0)
                                                {
                                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                }

                                            }
                                            else if (dsFlow.Tables[0].Rows[0]["Steps_Count"].ToString() == "3")
                                            {
                                                Flow1DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep1_DisplayName"].ToString();
                                                Flow2DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep2_DisplayName"].ToString();
                                                Flow3DisplayName = dsFlow.Tables[0].Rows[0]["FlowStep3_DisplayName"].ToString();

                                                sDate1 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep1_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                                                sDate1 = getdata.ConvertDateFormat(sDate1);
                                                CDate1 = Convert.ToDateTime(sDate1);


                                                sDate2 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep2_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                                                sDate2 = getdata.ConvertDateFormat(sDate2);
                                                CDate2 = Convert.ToDateTime(sDate2);

                                                sDate3 = Convert.ToDateTime(ds.Tables[0].Rows[0]["FlowStep3_TargetDate"].ToString()).ToString("dd/MM/yyyy");
                                                //sDate3 = sDate3.Split('/')[1] + "/" + sDate3.Split('/')[0] + "/" + sDate3.Split('/')[2];
                                                sDate3 = getdata.ConvertDateFormat(sDate3);
                                                CDate3 = Convert.ToDateTime(sDate3);

                                                sDate4 = DateTime.Now.ToString("dd/MM/yyyy");
                                                //sDate4 = sDate4.Split('/')[1] + "/" + sDate4.Split('/')[0] + "/" + sDate4.Split('/')[2];
                                                sDate4 = getdata.ConvertDateFormat(sDate4);
                                                CDate4 = Convert.ToDateTime(CDate4);
                                                int cnt = getdata.Document_Insert_or_Update(ActualDocumentUID, new Guid(projectId), new Guid(workpackageid), new Guid(sDocumentUID), txtprefnumber.Text, txtRefNumber.Text,
                                               DocumentFor, IncomingRec_Date, new Guid(ds.Tables[0].Rows[0]["FlowUID"].ToString()), sFileName, txtdesc.Text, 1, Extn,
                                               DDLDocumentMedia.Items[0].Selected == true ? "true" : "false", DDLDocumentMedia.Items[1].Selected == true ? "true" : "false", DDLDocumentMedia.Items[2].Selected == true ? "true" : "false", DDLDocumentMedia.Items[3].Selected == true ? "true" : "false",
                                               DDLDocumentMedia.Items[4].Selected == true ? "true" : "false", DDLDocumentMedia.Items[5].Selected == true ? "true" : "false", CoverPagePath, txtremarks.Text, txtFileRefNumber.Text, cStatus,
                                               new Guid(ds.Tables[0].Rows[0]["FlowStep1_UserUID"].ToString()), CDate1, new Guid(ds.Tables[0].Rows[0]["FlowStep2_UserUID"].ToString()), CDate2, new Guid(ds.Tables[0].Rows[0]["FlowStep3_UserUID"].ToString()), CDate3,
                                               Flow1DisplayName, Flow2DisplayName, Flow3DisplayName, RBLOriginator.SelectedValue, Document_Date, UploadFilePhysicalpath, CoverLetterUID, RBLSubmissionType.SelectedValue);
                                                if (cnt <= 0)
                                                {
                                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting document. Please contact administrator');</script>");
                                                }

                                            }
                                            
                                            else // for all flows with step > 5 ( 6 to 20) added on 07/03/2022
                                            {

                          
                                                
                                            }
                                // store the origintaor reference no in separate table...added on 05/04/2022
                                //getdata.InsertorUpdateRefNoHistroy(Guid.NewGuid(), ActualDocumentUID, txtRefNumber.Text, "");
                                getdata.InsertorUpdateRefNoHistroy(Guid.NewGuid(), ActualDocumentUID, txtRefNumber.Text, txtprefnumber.Text);
                            }

                                        try
                                        {
                                            if (DDLDocumentType.SelectedValue != "Photographs")
                                            {
                                                //if (File.Exists(Server.MapPath(savedPath)))
                                                //{
                                                //    File.Delete(Server.MapPath(savedPath));
                                                //}
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            //throw
                                        }
                                    
                                }

                            
                        }
                       

                        //Timer1.Enabled = false;
                        //inProcess = false;
                        string sHtmlString = string.Empty;
                        DataSet dsUser = new DataSet();
                        if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                        {
                            dsUser = getdata.getAllUsers();
                        }
                        else if (Session["TypeOfUser"].ToString() == "PA")
                        {
                            //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                            dsUser = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
                        }
                        else
                        {
                            dsUser = getdata.GetUsers_under_ProjectUID(new Guid(DDlProject.SelectedValue));
                        }
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            string CC = string.Empty;
                            string ToEmailID = "";
                            string sUserName = "";
                            string DocumentMedia = DDLDocumentMedia.Items[0].Selected == true ? "Hard Copy," : "";
                            DocumentMedia += DDLDocumentMedia.Items[1].Selected == true ? "Soft Copy(PDF)," : "";
                            DocumentMedia += DDLDocumentMedia.Items[2].Selected == true ? "Soft Copy Editable Format," : "";
                            DocumentMedia += DDLDocumentMedia.Items[3].Selected == true ? "Soft Copy Ref.," : "";
                            DocumentMedia += DDLDocumentMedia.Items[4].Selected == true ? "Hard Copy Ref.," : "";
                            DocumentMedia += DDLDocumentMedia.Items[5].Selected == true ? "No Media," : "";
                            for (int i = 0; i < dsUser.Tables[0].Rows.Count; i++)
                            {
                                if (dsUser.Tables[0].Rows[i]["UserUID"].ToString() == Session["UserUID"].ToString())
                                {
                                    ToEmailID = dsUser.Tables[0].Rows[i]["EmailID"].ToString();
                                    sUserName = dsUser.Tables[0].Rows[i]["UserName"].ToString();
                                }
                                else
                                {
                                    if (getdata.GetUserMailAccess(new Guid(dsUser.Tables[0].Rows[i]["UserUID"].ToString()), "documentmail") != 0)
                                    {
                                        CC += dsUser.Tables[0].Rows[i]["EmailID"].ToString() + ",";
                                    }
                                }
                            }
                            CC = CC.TrimEnd(',');
                            sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                   "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                      "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                            sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                               "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                            if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                            {
                                sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                            }
                            else
                            {
                                sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                            }
                            sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                               "</div>";
                            sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + sUserName + " added document's under submittal " + DDLDocuments.SelectedItem.Text + ".</span> <br/><br/></div>";
                            sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                            "<tr><td><b>Workpackage </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLWorkPackage.SelectedItem.Text + "</td></tr>" +
                                            "<tr><td><b>Subject/Description </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtdesc.Text + "</td></tr>" +
                                            "<tr><td><b>Reference Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtprefnumber.Text + "</td></tr>" +
                                             "<tr><td><b>No. of Documents </b></td><td style='text-align:center;'><b>:</b></td><td>" + DocumentCount + "</td></tr>" +
                                             "<tr><td><b>Document Media </b></td><td style='text-align:center;'><b>:</b></td><td>" + DocumentMedia.TrimEnd(',') + "</td></tr>" +
                                            "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + DateTime.Now.ToString("dd MMM yyyy") + "</td></tr>" +
                                            "<tr><td><b>Remarks </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtremarks.Text + "</td></tr>";
                            sHtmlString += "</table></div>";
                            sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";

                            //sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                            //   "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "</head>" +
                            //      "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                            //sHtmlString += "<div style='float:left; width:100%; height:30px;'>" +
                            //                   "Dear, " + "Users" +
                            //                   "<br/><br/></div>";
                            //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Below are the document details. <br/><br/></div>";
                            //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>WorkPackage : " + DDLWorkPackage.SelectedItem.Text + "<br/></div>";
                            //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Submittal Name: " + DDLDocuments.SelectedItem.Text + "<br/></div>";
                            //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Subject/Description : " + txtdesc.Text + "<br/></div>";
                            ////sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Document Type : " + ddlDocType.SelectedValue + "<br/></div>";
                            //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Date : " + DateTime.Now.ToShortDateString() + "<br/></div>";
                            //sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";
                            //string ret = getdata.SendMail(ds.Tables[0].Rows[0]["EmailID"].ToString(), Subject, sHtmlString, CC, Server.MapPath(CoverPagePath));
                            // added on 02/11/2020
                            DataTable dtemailCred = getdata.GetEmailCredentials();
                            Guid MailUID = Guid.NewGuid();
                            if (ToEmailID == "")
                            {
                                ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(Session["UserUID"].ToString()));
                            }
                            getdata.StoreEmaildataToMailQueue(MailUID, new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, "Documents Uploaded !", sHtmlString, CC, "");
                            //
                            // added on 07/01/2022 for sending mail to next user in line to change status...
                            // if (dsFlowcheck.Tables[0].Rows[0]["Type"].ToString() != "STP-OB")
                            //   {
                            DataSet dsnew = getdata.getTop1_DocumentStatusSelect(ActualDocumentUID);
                            DataSet dsNext = getdata.GetNextStep_By_DocumentUID(ActualDocumentUID, dsnew.Tables[0].Rows[0]["ActivityType"].ToString());

                            sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                   "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                      "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                            sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                               "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                            if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                            {
                                sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                            }
                            else
                            {
                                sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                            }
                            sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                               "</div>";
                            sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + sUserName + " added document's under submittal " + DDLDocuments.SelectedItem.Text + ".</span> <br/><br/></div>";
                            sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                            "<tr><td><b>Workpackage </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLWorkPackage.SelectedItem.Text + "</td></tr>" +
                                            "<tr><td><b>Subject/Description </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtdesc.Text + "</td></tr>" +
                                            "<tr><td><b>Reference Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtprefnumber.Text + "</td></tr>" +
                                             "<tr><td><b>No. of Documents </b></td><td style='text-align:center;'><b>:</b></td><td>" + DocumentCount + "</td></tr>" +
                                             "<tr><td><b>Document Media </b></td><td style='text-align:center;'><b>:</b></td><td>" + DocumentMedia.TrimEnd(',') + "</td></tr>" +
                                            "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + DateTime.Now.ToString("dd MMM yyyy") + "</td></tr>" +
                                            "<tr><td><b>Remarks </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtremarks.Text + "</td></tr>";
                            sHtmlString += "</table><br /><br /><div style='color: red'>Kindly note that you are to act on this to complete the next step in document flow.</div></div>";
                            sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Monitoring Tool.</div></div></body></html>";

                            string next = string.Empty;
                            DataSet dsNxtUser = new DataSet();
                            foreach (DataRow dr in dsNext.Tables[0].Rows)
                            {
                                dsNxtUser = getdata.GetNextUser_By_DocumentUID(ActualDocumentUID, int.Parse(dr["ForFlow_Step"].ToString()));
                                foreach (DataRow druser in dsNxtUser.Tables[0].Rows)
                                {
                                    ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString()));
                                    if (!next.Contains(ToEmailID))
                                    {
                                        getdata.StoreEmaildataToMailQueue(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, "Documents Uploaded !.Kindly complete the next step !", sHtmlString, "", "");
                                        next += ToEmailID;
                                    }

                                }
                            }
                            //
                        }
                    }
                    //  }

                    btnSubmit.Enabled = true;
                    Session["SelectedTaskUID1"] = sDocumentUID;
                    if (Request.QueryString["sType"] != null)
                    {
                        Session["ViewDocBy"] = Request.QueryString["sType"].ToString();
                    }
                    else
                    {
                        Session["ViewDocBy"] = "Activity";
                    }
                    //changed on 12/09/2022
                    // Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    divMain.Visible = false;
                    divUploadmsg.Visible = true;
                    btnSubmit.Visible = false;
                   btnSubmitOlddoc.Visible = false;
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Progress Bar", "ShowProgressBar('false')", true);
                    btnSubmit.Enabled = true;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('" + ex.Message + "');</script>");
                }


            
        }

        //protected void ProcessRecords()
        //{
        //    inProcess = true;


        //    processComplete = true;
        //    content = "";
        //}
        //protected void Timer1_Tick(object sender, EventArgs e)
        //{
        //    if (inProcess)
        //    {
        //        lblProcessMessage.Text = content;
        //    }
        //    if (processComplete)
        //    {
        //        inProcess = false;
        //        Timer1.Enabled = false;
        //    }
        //}
    }
}