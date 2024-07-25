﻿using ProjectManager.DAL;
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
    public partial class add_documentstatus : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        string cnnString = ConfigurationManager.ConnectionStrings["PMConnectionString"].ConnectionString;
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
                   
                    BindDocuments();
                    BindOriginator();
                    if (Request.QueryString["StatusUID"] != null)
                    {
                        BindStatus();
                        BindDocStatus(new Guid(Request.QueryString["StatusUID"]));
                        btnSubmit.Text = "Update";

                    }
                    else
                    {
                        DDLDocument.SelectedValue = Request.QueryString["DocID"];
                        btnSubmit.Text = "Submit";
                        BindStatus();
                    }

                    //added on 16/03/2022
                    hdFlowtype.Value = getdata.GetFlowTypeBySubmittalUID(new Guid(getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"]))));
                    if (getdata.GetFlowTypeBySubmittalUID(new Guid(getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"])))) == "STP" || getdata.GetFlowTypeBySubmittalUID(new Guid(getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"])))) == "STP-C" || getdata.GetFlowTypeBySubmittalUID(new Guid(getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"])))) == "STP-OB")
                    {
                        dtStartdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        spRef.Visible = false;
                        spCoverDate.Visible = false;
                        spCUpload.Visible = false;
                        spIncmDate.Visible = false;
                        if (DDlStatus.SelectedItem.ToString().Contains("AE Approval") || DDlStatus.SelectedItem.ToString().Contains("AEE Approval") || DDlStatus.SelectedItem.ToString().Contains("EE Approval") || DDlStatus.SelectedItem.ToString().Contains("ACE Approval") || DDlStatus.SelectedItem.ToString().Contains("CE Approval") || DDlStatus.SelectedItem.ToString().Contains("CE GFC Approval"))
                        {
                            divRef.Visible = false;
                            divCD.Visible = true;
                            divCUpload.Visible = true;
                            divReviewFile.Visible = true;
                            divIncmDate.Visible = false;
                            divUpdateStatus.Visible = false;
                        }
                        else
                        {
                            //for PMC and DTL and PC from salahuddin on 21st MAy 2022...implemeted on 13/06/2022
                            divRef.Visible = false;
                            divCD.Visible = true;
                            divCUpload.Visible = true;
                            divReviewFile.Visible = true;
                            divIncmDate.Visible = false;
                            divUpdateStatus.Visible = false;
                            DDlStatus.Enabled = true;
                            divforward.Visible = false;
                        }

                        if(DDlStatus.SelectedItem.ToString().Contains("Accepted-PMC"))
                        {
                            //divRef.Visible = false;
                            //divCD.Visible = true;
                            //divCUpload.Visible = true;
                            //divReviewFile.Visible = false;
                            //divIncmDate.Visible = false;
                             divUpdateStatus.Visible = false;
                            DDlStatus.Enabled = false;
                        }
                        

                         
                        //if (DDlStatus.SelectedItem.ToString().Contains("Meeting with EE or CE") || DDlStatus.SelectedItem.ToString().Contains("Rejected") || DDlStatus.SelectedItem.ToString().Contains("PMC DTL Review"))
                        //{
                           
                       // }
                    }
                    else
                    {
                        divforward.Visible = false;
                    }
                    if (DDlStatus.SelectedItem.ToString().Contains("CE Approval") || DDlStatus.SelectedItem.ToString().Contains("CE GFC Approval"))
                    {
                        if(DDlStatus.SelectedItem.ToString().Contains("ACE Approval"))
                            divforward.Visible = true;
                        else
                            divforward.Visible = false;
                    }
                    //------------------------------
                    //
                    if (DDlStatus.SelectedItem.ToString().ToLower().Contains("back to pmc"))
                    {
                        divUSers.Attributes.Add("style", "display:block");
                    }
                    else
                    {
                        divUSers.Attributes.Add("style", "display:none");
                    }
                    //
                    if(getdata.GetFlowTypeBySubmittalUID(new Guid(getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"])))) == "STP-C" || getdata.GetFlowTypeBySubmittalUID(new Guid(getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"])))) == "STP-OB")
                    {
                        divReviewFile.Visible = false;
                        divFileAttachmentDoc.Visible = true;
                        divforward.Visible = false;
                    }
                    //added on 28/11/2022 for DTL Correspondence
                    string SubmittalUID = getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"]));
                    string FlowName = getdata.GetFlowName_by_SubmittalID(new Guid(SubmittalUID));
                    ViewState["FlowName"] = FlowName;
                    if (getdata.GetFlowTypeBySubmittalUID(new Guid(getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"])))) == "STP-OB")
                    {
                        divcorrespondenceccto.Visible = true;
                        divRef.Visible = true;
                        if (Session["IsClient"].ToString() == "Y")
                        {

                            lblrefno.InnerText = "BWSSB Ref. Number";
                        }
                        else if (Session["IsONTB"].ToString() == "Y")
                        {

                            lblrefno.InnerText = "ONTB Ref. Number";
                        }
                        //
                       
                        DataSet ds = getdata.GetCorrespondenceLetterUsersTo(new Guid(getdata.GetFlowUIDBySubmittalUID(new Guid(SubmittalUID))));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //
                            chkcorrespondenceccto.DataTextField = "LetterTo";
                            chkcorrespondenceccto.DataValueField = "LetterTo";
                            chkcorrespondenceccto.DataSource = ds;
                            chkcorrespondenceccto.DataBind();
                            if (FlowName == "DTL Correspondence")
                                chkcorrespondenceccto.Items.Insert(0, "Contractor");

                        }
                        //
                        if (DDlStatus.SelectedItem.ToString().Contains("Submitted to"))
                        {
                            //
                            DataSet dsCCUser = getdata.GetCorrespondenceCCToUsers_FirstSubmission(new Guid(Request.QueryString["DocID"]));
                            foreach(DataRow dr in dsCCUser.Tables[0].Rows)
                            {
                                for(int i=0; i < chkcorrespondenceccto.Items.Count; i ++)
                                {
                                   
                                    if(dr["UserType"].ToString() == chkcorrespondenceccto.Items[i].Value)
                                         chkcorrespondenceccto.Items[i].Selected = true;
                                }
                            }
                            //
                            divRef.Visible = false;
                            divCD.Visible = false;
                            divCUpload.Visible = false;
                            divReviewFile.Visible = false;
                            divIncmDate.Visible = false;
                            divUpdateStatus.Visible = false;
                            DDlStatus.Enabled = true;
                            divforward.Visible = false;
                            divFileAttachmentDoc.Visible = false;
                            divcorrespondenceccto.Visible = false;
                            Originator.Visible = false;
                            
                        }
                    }
                    
                    
                    GetAllUSersforPMC();
                 }
            }
        }

        
        private void BindDocuments()
        {
           // DataSet ds = getdata.getDocuments();
            DataSet dsdoc = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"]));
            DDLDocument.DataTextField = "ActualDocument_Name";
            DDLDocument.DataValueField = "ActualDocumentUID";
            DDLDocument.DataSource = dsdoc;
            DDLDocument.DataBind();

            //added on 25/09/2023 
            DataSet ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
               
                //added on 20/08/2022 for latest doc name version
                DataSet dsNew = getdata.getTop1_DocumentStatusSelect(new Guid(Request.QueryString["DocID"].ToString()));
                if (dsNew != null)
                {
                    if (dsNew.Tables[0].Rows[0]["ActivityType"].ToString() != "" && dsNew.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                    {
                        //e.Row.Cells[1].Text = ds.Tables[0].Rows[0]["TopVersion"].ToString();

                        try
                        {
                            string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(dsNew.Tables[0].Rows[0]["Doc_Path"].ToString()));
                            LblDocNameLatest.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2));// + " [ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]"; ;
                        }
                        catch (Exception ex)
                        {
                            LblDocNameLatest.Text = "";
                        }

                    }

                }
            }
         }
        private void BindStatus()
        {
            DataSet ds1 = getdata.getTop1_DocumentStatusSelect(new Guid(Request.QueryString["DocID"]));
            string SubmittalUID = getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"]));
            string FlowType = getdata.GetFlowTypeBySubmittalUID((new Guid(SubmittalUID)));
            if (ds1.Tables[0].Rows.Count > 0)
            {
                //int StepCount = getdata.GetFlowStep_by_FlowUID(new Guid(Request.QueryString["FlowUID"]));
                //DataSet ds = getdata.GetStatus_by_UserType(Session["TypeOfUser"].ToString(), ds1.Tables[0].Rows[0]["ActivityType"].ToString(), StepCount);
                DataSet ds = getdata.GetStatus_by_UserType_FlowUID(Session["TypeOfUser"].ToString(), ds1.Tables[0].Rows[0]["ActivityType"].ToString(), new Guid(ds1.Tables[0].Rows[0]["FlowUID"].ToString()));
                if(FlowType == "STP")
                {
                    ds = getdata.GetStatus_by_UserType_FlowUID_STP(Session["TypeOfUser"].ToString(), ds1.Tables[0].Rows[0]["ActivityType"].ToString(), new Guid(ds1.Tables[0].Rows[0]["FlowUID"].ToString()),new Guid(SubmittalUID),new Guid(Session["UserUID"].ToString()));

                }
                DDlStatus.DataTextField = "Update_Status";
                DDlStatus.DataValueField = "Update_Status";
                DDlStatus.DataSource = ds;
                DDlStatus.DataBind();
                //added on 13/07/2022
                if(ds1.Tables[0].Rows[0]["ActivityType"].ToString().Contains("Code A") || ds1.Tables[0].Rows[0]["ActivityType"].ToString().Contains("Code B"))
                {
                   foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        if(dr["Update_Status"].ToString().Contains("Code A") || dr["Update_Status"].ToString().Contains("Code B"))
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
                        //DDlStatus.Items.Clear();
                        //DDlStatus.Items.Insert(0, "Closed");
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
                //txtbudget.Text = ds.Tables[0].Rows[0]["Activity_Budget"].ToString();
                txtcomments.Text = ds.Tables[0].Rows[0]["Status_Comments"].ToString();
                if (DDlStatus.Items.Count > 0)
                {
                    DDlStatus.SelectedItem.Text = ds.Tables[0].Rows[0]["ActivityType"].ToString();
                }
                if (ds.Tables[0].Rows[0]["ActivityDate"].ToString() != null && ds.Tables[0].Rows[0]["ActivityDate"].ToString() != "")
                {
                    dtStartdate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ActivityDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
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
                if (ds.Tables[0].Rows[0]["Origin"].ToString() != "")
                {
                    RBLOriginator.SelectedValue = ds.Tables[0].Rows[0]["Origin"].ToString();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e) //final submit
        {
            string FlowType = getdata.GetFlowTypeBySubmittalUID(new Guid(getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"]))));
            if (DDlStatus.SelectedItem.ToString() == "Closed" || FlowType  == "STP" || FlowType == "STP-C" || FlowType == "STP-OB")
            {
                try
                {
                    if (dtStartdate.Text == "" || dtStartdate.Text == "dd/MM/YYYY")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please enter Incmg Recived Date/Approval Date.');</script>");
                        dtStartdate.Focus();
                        return;
                    }
                    //added on 29/11/2022
                    if (FlowType == "STP-OB")
                    {
                        if (string.IsNullOrEmpty(txtrefNumber.Text) && !DDlStatus.SelectedItem.ToString().Contains("Forward") && !DDlStatus.SelectedItem.ToString().Contains("Submitted to") && !DDlStatus.SelectedItem.ToString().Contains("Closed by"))
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please enter Ref No.');</script>");
                            txtrefNumber.Focus();
                            return;
                        }
                    }
                    //
                    if (txtcomments.Text =="")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please enter comments.');</script>");
                        txtcomments.Focus();
                        if (DDlStatus.SelectedItem.ToString().ToLower().Contains("back to pmc"))
                        {
                            divUSers.Attributes.Add("style", "display:block");
                        }
                        return;
                    }

                  
                   
                        string DocPath = "";
                    string Subject = string.Empty;
                    string sHtmlString = string.Empty;
                    Guid StatusUID = new Guid();
                    string CoverPagePath = string.Empty;
                    string Status = DDlStatus.SelectedItem.ToString();
                    string Comments = txtcomments.Text;
                    string OriginatorRefNo = string.Empty;
                    string ProjectRefNo = string.Empty;
                    string RefNostring = string.Empty;
                    string filepathemail = string.Empty;
                    // check for all PMC Users
                    int check = 0;
                    //
                    DataSet dsTasks = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"].ToString()));
                    if (dsTasks.Tables[0].Rows.Count > 0)
                    {
                        OriginatorRefNo = dsTasks.Tables[0].Rows[0]["Ref_Number"].ToString();
                        ProjectRefNo = dsTasks.Tables[0].Rows[0]["ProjectRef_Number"].ToString();
                    }
                    //
                    int commentsCount = 0;
                    string categoryname = string.Empty;
                    string pending = string.Empty;
                    if (DDlStatus.SelectedItem.ToString().Contains("Accepted-PMC"))
                    {
                        DataSet getlateststatus = getdata.getTop1_DocumentStatusSelect(new Guid(Request.QueryString["DocID"].ToString()));


                        if (getlateststatus.Tables[0].Rows[0]["ActivityType"].ToString().ToLower().Contains("back to pmc"))
                        {
                            //DataSet dsbackUser = dbget.GetBacktoPMCUsers(new Guid(drnext["ActualDocumentUID"].ToString()));
                            DataSet dsMUSers = getdata.GetBacktoPMCUsers(new Guid(Request.QueryString["DocID"].ToString()));
                            string FlowUID = getdata.GetFlowUIDBySubmittalUID(new Guid(getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"]))));
                            if (dsMUSers.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                {
                                    if (getdata.checkUserAddedDocumentstatus(new Guid(Request.QueryString["DocID"].ToString()), new Guid(druser["PMCUser"].ToString()), getlateststatus.Tables[0].Rows[0]["ActivityType"].ToString()) == 0)
                                    {
                                        DataSet dsuserf = getdata.GetCategoryNameforUser(new Guid(Request.QueryString["ProjectUID"]), new Guid(druser["PMCUser"].ToString()), new Guid(FlowUID));
                                        foreach (DataRow drf in dsuserf.Tables[0].Rows)
                                        {
                                            pending = pending + drf["WorkPackageCategory_Name"].ToString() + " -- (" + getdata.getUserNameby_UID(new Guid(druser["PMCUser"].ToString())) + ") Pending <br/>";
                                        }
                                    }
                                    else
                                    {
                                        commentsCount = commentsCount + 1;
                                    }
                                }
                            }
                            commentsCount = commentsCount = commentsCount + 1;
                            if (dsMUSers.Tables[0].Rows.Count != commentsCount)
                            {
                                //  Status = "Accepted";
                                Status = getlateststatus.Tables[0].Rows[0]["ActivityType"].ToString();
                                DataSet dsuserf = getdata.GetCategoryNameforUser(new Guid(Request.QueryString["ProjectUID"]), new Guid(Session["UserUID"].ToString()), new Guid(FlowUID));
                                foreach (DataRow drf in dsuserf.Tables[0].Rows)
                                {
                                    categoryname = drf["WorkPackageCategory_Name"].ToString();
                                    pending = pending.Replace(categoryname + " -- (" + getdata.getUserNameby_UID(new Guid(Session["UserUID"].ToString())) + ") Pending <br/>", "");
                                }
                                Comments = Session["Username"].ToString() + " (" + categoryname + ") added - " + txtcomments.Text + "<br/>" + "--------------------" + "<br/>" + pending;

                            }
                            else
                            {
                                DataSet dsuserf = getdata.GetCategoryNameforUser(new Guid(Request.QueryString["ProjectUID"]), new Guid(Session["UserUID"].ToString()), new Guid(FlowUID));
                                foreach (DataRow drf in dsuserf.Tables[0].Rows)
                                {
                                    categoryname = drf["WorkPackageCategory_Name"].ToString();
                                }
                                Comments = Session["Username"].ToString() + " (" + categoryname + ") added - " + txtcomments.Text;

                            }
                        }
                        else
                        {
                            DataSet dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 3);
                            string FlowUID = getdata.GetFlowUIDBySubmittalUID(new Guid(getdata.GetSubmittalUID_By_ActualDocumentUID(new Guid(Request.QueryString["DocID"]))));
                            if (dsMUSers.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                {
                                    if (getdata.checkUserAddedDocumentstatus(new Guid(Request.QueryString["DocID"].ToString()), new Guid(druser["Approver"].ToString()), getlateststatus.Tables[0].Rows[0]["ActivityType"].ToString()) == 0)
                                    {
                                        DataSet dsuserf = getdata.GetCategoryNameforUser(new Guid(Request.QueryString["ProjectUID"]), new Guid(druser["Approver"].ToString()), new Guid(FlowUID));
                                        foreach (DataRow drf in dsuserf.Tables[0].Rows)
                                        {
                                            pending = pending + drf["WorkPackageCategory_Name"].ToString() + " -- (" + getdata.getUserNameby_UID(new Guid(druser["Approver"].ToString())) + ") Pending <br/>";
                                        }
                                    }
                                    else
                                    {
                                        commentsCount = commentsCount + 1;
                                    }
                                }
                            }
                            commentsCount = commentsCount = commentsCount + 1;
                            if (dsMUSers.Tables[0].Rows.Count != commentsCount)
                            {
                                //  Status = "Accepted";
                                Status = getlateststatus.Tables[0].Rows[0]["ActivityType"].ToString();
                                DataSet dsuserf = getdata.GetCategoryNameforUser(new Guid(Request.QueryString["ProjectUID"]), new Guid(Session["UserUID"].ToString()), new Guid(FlowUID));
                                foreach (DataRow drf in dsuserf.Tables[0].Rows)
                                {
                                    categoryname = drf["WorkPackageCategory_Name"].ToString();
                                    pending = pending.Replace(categoryname + " -- (" + getdata.getUserNameby_UID(new Guid(Session["UserUID"].ToString())) + ") Pending <br/>", "");
                                }
                                Comments = Session["Username"].ToString() + " (" + categoryname + ") added - " + txtcomments.Text + "<br/>" + "--------------------" + "<br/>" + pending;

                            }
                            else
                            {
                                DataSet dsuserf = getdata.GetCategoryNameforUser(new Guid(Request.QueryString["ProjectUID"]), new Guid(Session["UserUID"].ToString()), new Guid(FlowUID));
                                foreach (DataRow drf in dsuserf.Tables[0].Rows)
                                {
                                    categoryname = drf["WorkPackageCategory_Name"].ToString();
                                }
                                Comments = Session["Username"].ToString() + " (" + categoryname + ") added - " + txtcomments.Text;

                            }
                        }

                    }
                    else if (DDlStatus.SelectedItem.ToString().Contains("Network Design by ONTB") || DDlStatus.SelectedItem.ToString().Contains("ONTB Specialist Verified") || DDlStatus.SelectedItem.ToString() =="Review By ONTB")
                    {
                        Comments = Session["Username"].ToString() + " (" + categoryname + ") added - " + txtcomments.Text;
                    }
                    //added on 05/08/2022 for going back to indvidaul users instead of all users
                    if(DDlStatus.SelectedItem.ToString().ToLower().Contains("back to pmc"))
                    {
                        divUSers.Attributes.Add("style", "display:block");
                        List<ListItem> selectedItems = chkUserList.Items.Cast<ListItem>()
                                   .Where(li => li.Selected)
                                   .ToList();

                        if (selectedItems.Count == 0)
                        {
                            Response.Write("<script>alert('Please check atleast one user');</script>");
                            return;
                        }
                        //
                        getdata.DeleteBacktoPMcUsers(new Guid(Request.QueryString["DocID"].ToString()));
                        foreach (var eachSelectedItems in selectedItems)
                        {
                            int cnt = getdata.InsertBack_To_PMC_Users(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), new Guid(eachSelectedItems.Value));
                            Comments = Comments  + "<br/> ------------------------------------------ <br/> (" + getdata.getUserNameby_UID(new Guid(eachSelectedItems.Value)) + ") Pending <br/>";

                        }
                    }
                    //

                    //
                    if (Request.QueryString["StatusUID"] != null)
                    {
                        StatusUID = new Guid(Request.QueryString["StatusUID"]);
                        Subject = Session["Username"].ToString() + " updated a Status";
                    }
                    else
                    {
                        StatusUID = Guid.NewGuid();
                        Subject = Session["Username"].ToString() + " added a new Status";
                    }
                    //
                    if (DDlStatus.SelectedItem.ToString().Contains("AE Approval") || DDlStatus.SelectedItem.ToString().Contains("EE Approval") || DDlStatus.SelectedItem.ToString().Contains("ACE Approval") || DDlStatus.SelectedItem.ToString().Contains("CE Approval") || DDlStatus.SelectedItem.ToString().Contains("CE GFC Approval"))
                    {
                        //divPassword.Visible = true;

                        //if (txtPassword.Text == "")
                        //{
                        //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please enter your Password to go ahead !.');</script>");
                        //    txtPassword.Focus();
                        //    return;
                        //}
                        //else
                        //{
                        //    DataSet ds = getdata.CheckLogin(Session["UserID"].ToString(), Security.Encrypt(txtPassword.Text));
                        //    if (ds.Tables[0].Rows.Count == 0)
                        //    {
                        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Password is wrong !.');</script>");
                        //        txtPassword.Focus();
                        //        return;
                        //    }
                        //}
                    }
                    //added on 09/12/2022 for PC submitted doc to get approved by DTL and add as atatchment in mail
                    if (DDlStatus.SelectedItem.ToString().Contains("Submitted to"))
                    {
                        DataSet dspath = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"].ToString()));
                        if(dspath.Tables[0].Rows.Count > 0)
                        {
                            filepathemail = filepathemail + Server.MapPath(dspath.Tables[0].Rows[0]["ActualDocument_Path"].ToString()) + ",";
                        }
                        DataSet dsstatus = getdata.getActualDocumentStatusList(new Guid(Request.QueryString["DocID"].ToString()));
                        if(dsstatus.Tables[0].Rows.Count > 0)
                        {
                            string dstatusUID = dsstatus.Tables[0].Rows[0]["StatusUID"].ToString();
                            dspath = getdata.GetDocumentAttachments(new Guid(dstatusUID));
                            foreach(DataRow dr in dspath.Tables[0].Rows)
                            {
                                filepathemail = filepathemail + Server.MapPath(dr["AttachmentFile"].ToString()) + ",";
                            }
                        }
                    }
                     //

                        string sDate1 = "";
                    DateTime CDate1 = DateTime.Now;
                    //
                    sDate1 = dtStartdate.Text;
                    //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                    sDate1 = getdata.ConvertDateFormat(sDate1);
                    CDate1 = Convert.ToDateTime(sDate1);

                    DateTime lastUpdated = getdata.GetDocumentMax_ActualDate(new Guid(Request.QueryString["DocID"]));
                    if (lastUpdated.Date > CDate1.Date)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Actual Date/Incmg Recieved Date should be greater than previous date.');</script>");
                    }
                    else
                    {
                        //---------------------------------------------------
                        if (FileUploadDoc.HasFile)
                        {
                            //string projectName = getdata.getProjectNameby_ProjectUID(new Guid(Request.QueryString["PrjUID"].ToString()));
                            string LinkPath = Request.QueryString["ProjectUID"] + "/" + StatusUID + "/Link Document";
                            if (!Directory.Exists(Server.MapPath(LinkPath)))
                            {
                                Directory.CreateDirectory(Server.MapPath(LinkPath));
                            }
                            string sFileName = Path.GetFileNameWithoutExtension(FileUploadDoc.FileName);
                            string Extn = System.IO.Path.GetExtension(FileUploadDoc.FileName);

                            FileUploadDoc.SaveAs(Server.MapPath(LinkPath + "/" + sFileName + "_1_copy" + Extn));
                            //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + StatusUID + "_" + "1" + "_enp" + InputFile));
                            string savedPath = LinkPath + "/" + sFileName + "_1_copy" + Extn;
                            DocPath = LinkPath + "/" + sFileName + "_1" + Extn;
                            getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DocPath));

                            //DocPath = "~/Documents/" + projectName + "/" + StatusUID + "_" + "1" + InputFile;
                            //EncryptFile(Server.MapPath(savedPath), Server.MapPath(DocPath));
                            //  File.Encrypt(Server.MapPath(DocPath));
                        }
                        //-------------------------------------------
                        string DocumentDate = string.Empty;
                        if (dtDocumentDate.Text != "")
                        {
                            DocumentDate = dtDocumentDate.Text;
                        }
                        else
                        {
                            DocumentDate = dtStartdate.Text;
                        }
                        //DocumentDate = DocumentDate.Split('/')[1] + "/" + DocumentDate.Split('/')[0] + "/" + DocumentDate.Split('/')[2];
                        DocumentDate = getdata.ConvertDateFormat(DocumentDate);
                        DateTime Document_Date = Convert.ToDateTime(DocumentDate);
                        //
                        if (FileUploadCoverLetter.HasFile)
                        {
                            string FileDatetime = DateTime.Now.ToString("dd MMM yyyy hh-mm-ss tt");
                            string sDocumentPath = string.Empty;
                            //sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/" + Request.QueryString["DocID"] + "/" + StatusUID + "/" + FileDatetime + "/CoverLetter";
                            sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/" + StatusUID + "/CoverLetter";

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
                            //this is for Contractor Correspondence reply to contractor email attachments
                            filepathemail = filepathemail + Server.MapPath(CoverPagePath) + ",";
                            //
                        }


                        //
                        //sDate2 = (dtPlannedDate.FindControl("txtDate") as TextBox).Text;
                        //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                        //CDate2 = Convert.ToDateTime(sDate2);
                        int Cnt = getdata.InsertorUpdateDocumentStatus(StatusUID, new Guid(Request.QueryString["DocID"].ToString()), 1, Status, 0, CDate1, DocPath,
                            new Guid(Session["UserUID"].ToString()), Comments, Status, txtrefNumber.Text, Document_Date, CoverPagePath, RBLDocumentStatusUpdate.SelectedValue, RBLOriginator.SelectedValue);
                        if (Cnt > 0)
                        {
                            //DataSet ds = getdata.getAllUsers();
                            //added on 28/11/2022--------------------------------------------------
                            if(RBLOriginator.SelectedValue == "BWSSB")
                            {
                                if(!string.IsNullOrEmpty(txtrefNumber.Text))
                                {
                                    getdata.UpdateBWSSBRefNo(new Guid(Request.QueryString["DocID"].ToString()), txtrefNumber.Text);
                                }
                            }
                            //Update refno for ONTB ......
                            if (RBLOriginator.SelectedValue == "ONTB" && FlowType=="STP-OB")
                            {
                                
                                //getdata.UpdateActualDocsRefNo(new Guid(Request.QueryString["DocID"].ToString()), txtrefNumber.Text, "", 1);
                                //string RefUID = getdata.GetTopRefNoHistory(new Guid(Request.QueryString["DocID"].ToString()));
                                //if (!string.IsNullOrEmpty(RefUID))
                                //{
                                //    getdata.UpdateRefNoHistory(new Guid(RefUID), txtrefNumber.Text, "", 1);
                                //}
                            }
                            //-----------------------------------------------------
                            //Update the target dates 
                            bool storemail = true;
                            if (DDlStatus.SelectedItem.ToString().Contains("Accepted-PMC"))
                            {
                                if(Status !="Accepted" && !Status.ToLower().Contains("back to pmc"))
                                {
                                    getdata.StoreFreshTargetDatesforStatusChange(new Guid(Request.QueryString["DocID"].ToString()), Status);
                                }
                                else
                                {
                                    storemail = false;
                                }
                            }
                            else
                            {
                                getdata.StoreFreshTargetDatesforStatusChange(new Guid(Request.QueryString["DocID"].ToString()), Status);
                            }

                            // added on 01/09/2022 for Contractor Correspondence attachments
                            #region STP Correspondence code
                            if (FlowType == "STP-C" || FlowType == "STP-OB")
                            {

                                if(FileAttachmentDoc.HasFile)
                                {
                                    foreach (HttpPostedFile uploadedFileAtatch in FileAttachmentDoc.PostedFiles)
                                    {
                                        //System.Threading.Thread.Sleep(2000);
                                        //lblProcessMessage.Text = "Processing file " + Path.GetFileNameWithoutExtension(uploadedFile.FileName);

                                        string ExtnA = System.IO.Path.GetExtension(uploadedFileAtatch.FileName);
                                        if (ExtnA.ToLower() != ".exe" && ExtnA.ToLower() != ".msi" && ExtnA.ToLower() != ".db")
                                        {
                                            string FileDatetime = DateTime.Now.ToString("dd MMM yyyy hh-mm-ss tt");
                                            string sDocumentPath = string.Empty;
                                            //sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/" + Request.QueryString["DocID"] + "/" + StatusUID + "/" + FileDatetime + "/CoverLetter";
                                            sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/" + StatusUID + "/Attachments";

                                            if (!Directory.Exists(Server.MapPath(sDocumentPath)))
                                            {
                                                Directory.CreateDirectory(Server.MapPath(sDocumentPath));
                                            }

                                            string sFileNameA = Path.GetFileNameWithoutExtension(uploadedFileAtatch.FileName);

                                            FileAttachmentDoc.SaveAs(Server.MapPath(sDocumentPath + "/" + sFileNameA + "_1_copy" + ExtnA));
                                            string savedPath = sDocumentPath + "/" + sFileNameA + "_1_copy" + ExtnA;
                                            CoverPagePath = sDocumentPath + "/" + sFileNameA + "_1" + ExtnA;
                                            getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(CoverPagePath));

                                            //this is for Contractor Correspondence reply to contractor email attachments
                                            filepathemail = filepathemail + Server.MapPath(CoverPagePath) + ",";
                                            //

                                            int cnt = getdata.InsertDocumentsAttachments(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, sFileNameA, CoverPagePath, new Guid(Session["UserUID"].ToString()), DateTime.Now);

                                            if (cnt <= 0)
                                            {
                                                // Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error:11,There is some problem while inserting attachment. Please contact administrator');</script>");
                                            }
                                        }
                                    }
                                }

                                // insert as CCTo in another table if Contractor checkbox is selected
                                //if (chkcorrespondenceccto.Items[0].Selected)
                                //{
                                //    getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "Contractor");
                                //}
                                foreach (ListItem listItem in chkcorrespondenceccto.Items)
                                {
                                    if (listItem.Selected)
                                    {

                                        getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, listItem.Value);
                                    }
                                }
                            }
                            #endregion


                            DataSet ds = new DataSet();
                            //if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                            //{
                            //    ds = getdata.getAllUsers();
                            //}
                            //else if (Session["TypeOfUser"].ToString() == "PA")
                            //{
                            //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                            ds = getdata.GetUsers_under_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
                            // }
                            //else
                            //{
                            //    ds = getdata.getUsers_by_ProjectUnder(new Guid(Request.QueryString["ProjectUID"]));
                            //}
                            //mail sending
                            if (storemail)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string CC = string.Empty;
                                    string ToEmailID = "";
                                    string sUserName = "";
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        if (ds.Tables[0].Rows[i]["UserUID"].ToString() == Session["UserUID"].ToString())
                                        {
                                            ToEmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                                            sUserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                                        }
                                        else
                                        {
                                            if (getdata.GetUserMailAccess(new Guid(ds.Tables[0].Rows[i]["UserUID"].ToString()), "documentmail") != 0)
                                            {
                                                CC += ds.Tables[0].Rows[i]["EmailID"].ToString() + ",";
                                            }

                                        }
                                    }

                                    //
                                    CC = CC.TrimEnd(',');
                                    sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                      "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                         "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                    sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                       "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                    if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                    {
                                        sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                        RefNostring = "NJSEI Ref Number";
                                    }
                                    else
                                    {
                                        sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                        RefNostring = "ONTB Ref Number";
                                    }
                                    sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                               "</div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><br/>Dear User,<br/><br/><span style='font-weight:bold;'>" + sUserName + " has changed " + DDLDocument.SelectedItem.Text + " status.</span> <br/><br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                    "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLDocument.SelectedItem.Text + "</td></tr>" +
                                                    "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDlStatus.SelectedItem.Text + "</td></tr>" +
                                                    "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + OriginatorRefNo + "</td></tr>" +
                                                    "<tr><td><b>" + RefNostring + "</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNo + "</td></tr>" +
                                                    "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + CDate1.ToString("dd MMM yyyy") + "</td></tr>" +
                                                    "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtcomments.Text + "</td></tr>";
                                    sHtmlString += "</table></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";

                                    // added on 02/11/2020
                                    DataTable dtemailCred = getdata.GetEmailCredentials();
                                    Guid MailUID = Guid.NewGuid();
                                    getdata.StoreEmaildataToMailQueue(MailUID, new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");

                                    //----add here----
                                    // added on 07/01/2022 for sending mail to next user in line to change status...
                                    DataSet dsnew = getdata.getTop1_DocumentStatusSelect(new Guid(Request.QueryString["DocID"]));
                                    DataSet dsNext = getdata.GetNextStep_By_DocumentUID(new Guid(Request.QueryString["DocID"]), dsnew.Tables[0].Rows[0]["ActivityType"].ToString());

                                    sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                    "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                       "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                    sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                       "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                    if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                    {
                                        sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                        RefNostring = "NJSEI Ref Number";
                                    }
                                    else
                                    {
                                        sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                        RefNostring = "ONTB Ref Number";
                                    }
                                    sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                               "</div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + sUserName + " has changed " + DDLDocument.SelectedItem.Text + " status.</span> <br/><br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                    "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLDocument.SelectedItem.Text + "</td></tr>" +
                                                    "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDlStatus.SelectedItem.Text + "</td></tr>" +
                                                     "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + OriginatorRefNo + "</td></tr>" +
                                                    "<tr><td><b>" + RefNostring + "</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNo + "</td></tr>" +
                                                    "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + CDate1.ToString("dd MMM yyyy") + "</td></tr>" +
                                                    "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtcomments.Text + "</td></tr>";
                                    sHtmlString += "</table><br /><br /><div style='color: red'>Kindly note that you are to act on this to complete the next step in document flow.</div></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";
                                    Subject = Subject + ".Kindly complete the next step !";
                                    string next = string.Empty;
                                    DataSet dsNxtUser = new DataSet();
                                    //added on 24/08/20222 for back to  pmc
                                    if (DDlStatus.SelectedItem.ToString().ToLower().Contains("back to pmc"))
                                    {
                                        List<ListItem> selectedItems = chkUserList.Items.Cast<ListItem>()
                                  .Where(li => li.Selected)
                                  .ToList();
                                        foreach (var eachSelectedItems in selectedItems)
                                        {
                                            ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(eachSelectedItems.Value));
                                            if (!next.Contains(ToEmailID))
                                            {
                                                getdata.StoreEmaildataToMailQueue(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, "", "");
                                                next += ToEmailID;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (DataRow dr in dsNext.Tables[0].Rows)
                                        {
                                            dsNxtUser = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"]), int.Parse(dr["ForFlow_Step"].ToString()));
                                            foreach (DataRow druser in dsNxtUser.Tables[0].Rows)
                                            {
                                                ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString()));
                                                if (!next.Contains(ToEmailID))
                                                {
                                                    getdata.StoreEmaildataToMailQueue(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, "", "");
                                                    next += ToEmailID;
                                                }

                                            }
                                        }
                                    }



                                    // -------------------------mail to contractor after DTL has approved
                                    if (DDlStatus.SelectedItem.ToString() == "Code A" || DDlStatus.SelectedItem.ToString() == "Code B" || DDlStatus.SelectedItem.ToString() == "Code C" || DDlStatus.SelectedItem.ToString() == "Network Design DTL Reviewed" || DDlStatus.SelectedItem.ToString() == "ONTB DTL Verified" || DDlStatus.SelectedItem.ToString().Contains("Rejected"))
                                    {
                                        string EmailHeading = string.Empty;
                                        Subject = "Document Status Changed !";
                                        if (DDlStatus.SelectedItem.ToString().Contains("Rejected"))
                                        {
                                            EmailHeading = DDlStatus.SelectedItem.ToString();
                                        }
                                        else if (DDlStatus.SelectedItem.ToString().Contains("Code"))
                                        {
                                            EmailHeading = " has been approved under " + DDlStatus.SelectedItem.ToString() + ".Please submit 9 Hard copies Documents for Review to the client outside the MIS system";
                                        }
                                        else
                                        {
                                            EmailHeading = "Under Client Approval Process";
                                        }
                                        sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                     "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                        "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                        sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                           "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                        if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                        {
                                            sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                            RefNostring = "NJSEI Ref Number";

                                        }
                                        else
                                        {
                                            sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                            RefNostring = "ONTB Ref Number";
                                        }
                                        sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                                   "</div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Contractor,<br/><br/><span style='font-weight:bold;'>Document " + DDLDocument.SelectedItem.Text + EmailHeading + ".</span> <br/><br/></div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                        "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLDocument.SelectedItem.Text + "</td></tr>" +
                                                        "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDlStatus.SelectedItem.Text + "</td></tr>" +
                                                        "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + OriginatorRefNo + "</td></tr>" +
                                                    "<tr><td><b>" + RefNostring + "</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNo + "</td></tr>" +
                                                        "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + CDate1.ToString("dd MMM yyyy") + "</td></tr>" +
                                                        "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtcomments.Text + "</td></tr>";
                                        sHtmlString += "</table></div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";

                                        // added on 30/03/2022 to store mail for Contractor........

                                        DataSet dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 1);
                                        if (dsMUSers.Tables[0].Rows.Count > 0)
                                        {
                                            foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                            {
                                                ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString()));
                                            }
                                        }

                                        getdata.StoreEmaildataToMailQueue(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");
                                    } // mail to contractor and DTL afetr CE has approved
                                    else if (DDlStatus.SelectedItem.ToString().Contains("CE Approval") || DDlStatus.SelectedItem.ToString().Contains("Client CE GFC Approval"))
                                    {
                                        // added on 30/03/2022 to store mail for Contractor........
                                        Subject = "Document Status Changed !";
                                        DataSet dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 1);
                                        if (dsMUSers.Tables[0].Rows.Count > 0)
                                        {
                                            foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                            {
                                                ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString()));
                                            }
                                        }
                                        //To DTL
                                        CC = "ns.rao04@gmail.com";
                                        //--------------------
                                        sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                    "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                       "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                        sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                           "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                        if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                        {
                                            sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                            RefNostring = "NJSEI Ref Number";
                                        }
                                        else
                                        {
                                            sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                            RefNostring = "ONTB Ref Number";
                                        }
                                        sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                                   "</div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Contractor,<br/><br/><span style='font-weight:bold;'>Document " + DDLDocument.SelectedItem.Text + " is " + DDlStatus.SelectedItem.ToString().Replace("Approval", "Approved") + ".</span> <br/><br/></div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                        "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLDocument.SelectedItem.Text + "</td></tr>" +
                                                        "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDlStatus.SelectedItem.Text + "</td></tr>" +
                                                        "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + OriginatorRefNo + "</td></tr>" +
                                                    "<tr><td><b>" + RefNostring + "</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNo + "</td></tr>" +

                                                        "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + CDate1.ToString("dd MMM yyyy") + "</td></tr>";

                                        sHtmlString += "</table></div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";
                                        getdata.StoreEmaildataToMailQueue(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");


                                    }
                                    // this is for Contractor Correspondence 
                                    if(FlowType == "STP-C" || FlowType =="STP-OB")
                                    {
                                        bool forEE = false, forACE = false ,forCE=false,forAEE=false,forContractor=false,forDTL =false;
                                        foreach (ListItem listItem in chkcorrespondenceccto.Items)
                                        {
                                            if (listItem.Selected)
                                            {
                                                if(listItem.Value == "EE")
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
                                            }
                                        }
                                        //
                                        if (DDlStatus.SelectedItem.Text.Contains("by EE") || DDlStatus.SelectedItem.Text.Contains("Reply to EE"))
                                         {
                                            forEE = true;
                                        }
                                         if (DDlStatus.SelectedItem.Text.Contains("by ACE") || DDlStatus.SelectedItem.Text.Contains("Reply to ACE"))
                                        {
                                            forACE = true;
                                        }
                                        if (DDlStatus.SelectedItem.Text.Contains("by CE") || DDlStatus.SelectedItem.Text.Contains("Reply to CE"))
                                        {
                                            forCE = true;
                                        }
                                         if (DDlStatus.SelectedItem.Text.Contains("by DTL") || DDlStatus.SelectedItem.Text.Contains("Reply to DTL"))
                                        {
                                            forDTL = true;
                                        }
                                        if (DDlStatus.SelectedItem.Text.Contains("by AEE") || DDlStatus.SelectedItem.Text.Contains("Reply to AEE"))
                                        {
                                            forAEE = true;
                                        }
                                        if (DDlStatus.SelectedItem.Text.Contains("by Contractor") || DDlStatus.SelectedItem.Text.Contains("Reply to Contractor"))
                                        {
                                            forContractor = true;
                                        }
                                        //
                                        Subject = "Document Status Changed !";
                                        if ((DDlStatus.SelectedItem.Text == "Reply to Contractor by EE" || DDlStatus.SelectedItem.Text == "Reply to Contractor by ACE" || DDlStatus.SelectedItem.Text == "Reply to Contractor by CE") && ViewState["FlowName"].ToString() == "Contractor Correspondence") //DTL,AEE,ACE and CE.
                                        {
                                            //Contractor
                                            DataSet dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 1);
                                            if (dsMUSers.Tables[0].Rows.Count > 0)
                                            {
                                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                {
                                                    ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString()));
                                                }
                                            }
                                            //DTL
                                           dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 5);
                                            if (dsMUSers.Tables[0].Rows.Count > 0)
                                            {
                                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                {
                                                    CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                }
                                            }
                                            //AEE
                                            dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 6);
                                            if (dsMUSers.Tables[0].Rows.Count > 0)
                                            {
                                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                {
                                                    CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                }
                                            }
                                            //EE
                                            dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 7);
                                            if (dsMUSers.Tables[0].Rows.Count > 0)
                                            {
                                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                {
                                                    CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                }
                                            }
                                            //ACE
                                            dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 8);
                                            if (dsMUSers.Tables[0].Rows.Count > 0)
                                            {
                                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                {
                                                    CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                }
                                            }
                                            //CE
                                            dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 9);
                                            if (dsMUSers.Tables[0].Rows.Count > 0)
                                            {
                                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                {
                                                    CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                }
                                            }

                                        }
                                        else if (ViewState["FlowName"].ToString() == "DTL Correspondence") //
                                        {

                                            //DTL
                                            DataSet dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 1);
                                            if (dsMUSers.Tables[0].Rows.Count > 0)
                                            {
                                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                {
                                                    ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString()));
                                                }
                                            }
                                            //PC
                                            dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 7);
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
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 2);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "EE");
                                            }
                                            //ACE
                                            if (forACE)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 3);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "ACE");
                                            }
                                            //CE
                                            if (forCE)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 4);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "CE");
                                            }
                                            //AEE
                                            if (forAEE)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 5);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "AEE");
                                            }
                                            //Contractor
                                            if (forContractor)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 6);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "Contractor");
                                            }
                                            //
                                            getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "DTL");
                                        }
                                        else if (ViewState["FlowName"].ToString() == "EE Correspondence")
                                        {
                                            //EE
                                            DataSet dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 1);
                                            if (dsMUSers.Tables[0].Rows.Count > 0)
                                            {
                                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                {
                                                    ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString()));
                                                }
                                            }
                                            //AEE
                                            if (forAEE)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 2);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "AEE");
                                            }
                                            //ACE
                                            if (forACE)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 3);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "ACE");
                                            }
                                            //CE
                                            if (forCE)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 4);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "CE");
                                            }
                                            //DTL
                                            if (forDTL)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 5);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "DTL");
                                            }
                                            //Contractor
                                            if (forContractor)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 6);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "Contractor");
                                            }
                                            getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "EE");
                                        }
                                        else if (ViewState["FlowName"].ToString() == "ACE Correspondence") //
                                        {
                                            //ACE
                                            DataSet dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 1);
                                            if (dsMUSers.Tables[0].Rows.Count > 0)
                                            {
                                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                {
                                                    ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                }
                                            }
                                            //EE
                                            if (forEE)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 2);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "EE");
                                            }
                                            //CE
                                            if (forCE)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 3);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "CE");
                                            }
                                            //DTL
                                            if (forDTL)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 4);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "DTL");
                                            }
                                            //Contractor
                                            if (forContractor)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 5);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "Contractor");
                                            }
                                            getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "ACE");

                                        }
                                        else if (ViewState["FlowName"].ToString() == "CE Correspondence") //
                                        {
                                            //CE
                                            DataSet dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 1);
                                            if (dsMUSers.Tables[0].Rows.Count > 0)
                                            {
                                                foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                {
                                                    ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                }
                                            }
                                            //EE
                                            if (forEE)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 2);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "EE");
                                            }
                                            //ACE
                                            if (forACE)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 3);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "ACE");
                                            }
                                            //DTL
                                            if (forDTL)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 4);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "DTL");
                                            }
                                            //Contractor
                                            if (forContractor)
                                            {
                                                dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 5);
                                                if (dsMUSers.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                                    {
                                                        CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                                    }
                                                }
                                                getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "Contractor");
                                            }
                                            getdata.InsertIntoCorrespondenceCCToUsers(Guid.NewGuid(), new Guid(Request.QueryString["DocID"].ToString()), StatusUID, "CE");

                                        }
                                        //else if (DDlStatus.SelectedItem.Text == "Reply to Contractor by CE") //ACE,EE,AEE and DTL..
                                        //{
                                        //    Contractor
                                        //    DataSet dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 1);
                                        //    if (dsMUSers.Tables[0].Rows.Count > 0)
                                        //    {
                                        //        foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                        //        {
                                        //            ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString()));
                                        //        }
                                        //    }
                                        //}
                                        //
                                        CC = CC.TrimEnd(',');
                                        sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                   "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                      "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                        sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                           "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                       
                                            sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                            RefNostring = "ONTB Ref Number";
                                        
                                        sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                                   "</div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><br/>Dear User,<br/><br/><span style='font-weight:bold;'>Document " + DDLDocument.SelectedItem.Text + " status changed to " + DDlStatus.SelectedItem.ToString().Replace("Approval", "Approved") + ".</span> <br/><br/></div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                        "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLDocument.SelectedItem.Text + "</td></tr>" +
                                                        "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDlStatus.SelectedItem.Text + "</td></tr>" +
                                                        "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + OriginatorRefNo + "</td></tr>" +
                                                    "<tr><td><b>" + RefNostring + "</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNo + "</td></tr>" +

                                                        "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + CDate1.ToString("dd MMM yyyy") + "</td></tr>";

                                        sHtmlString += "</table></div>";
                                        sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";
                                        getdata.StoreEmaildataToMailQueue(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC,filepathemail);

                                    }
                                    //
                                }
                             }
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");


                        }
                        else
                        {

                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: There is a problem with this feature. Please contact system admin');</script>");

                        }
                    }

                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ADS 12 - There is a problem with this feature. Please contact system admin');</script>");
                }

            }
            else
            {
                
                if (txtrefNumber.Text.Trim()=="")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please enter Reference Number.');</script>");
                    return;
                }
                if (dtDocumentDate.Text == "" || dtDocumentDate.Text=="dd/MM/YYYY")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please enter Cover letter Date.');</script>");
                    return;
                }
                if (dtStartdate.Text == "" || dtStartdate.Text == "dd/MM/YYYY")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please enter Incmg Recived Date/Approval Date.');</script>");
                    return;
                }

                if (!FileUploadCoverLetter.HasFile)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please upload a cover letter.');</script>");
                }
                else
                {
                    try
                    {
                        string DocPath = "";
                        string Subject = string.Empty;
                        string sHtmlString = string.Empty;
                        Guid StatusUID = new Guid();
                        string CoverPagePath = string.Empty;
                        if (Request.QueryString["StatusUID"] != null)
                        {
                            StatusUID = new Guid(Request.QueryString["StatusUID"]);
                            Subject = Session["Username"].ToString() + " updated a Status";
                        }
                        else
                        {
                            StatusUID = Guid.NewGuid();
                            Subject = Session["Username"].ToString() + " added a new Status";
                        }
                        //
                        string OriginatorRefNo = string.Empty;
                        string ProjectRefNo = string.Empty;
                        string RefNostring = string.Empty;
                       
                        //
                        DataSet dsTasks = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"].ToString()));
                        if (dsTasks.Tables[0].Rows.Count > 0)
                        {
                            OriginatorRefNo = dsTasks.Tables[0].Rows[0]["Ref_Number"].ToString();
                            ProjectRefNo = dsTasks.Tables[0].Rows[0]["ProjectRef_Number"].ToString();
                        }
                        //

                        string sDate1 = "";
                        DateTime CDate1 = DateTime.Now;
                        //
                        sDate1 = dtStartdate.Text;
                        //sDate1 = sDate1.Split('/')[1] + "/" + sDate1.Split('/')[0] + "/" + sDate1.Split('/')[2];
                        sDate1 = getdata.ConvertDateFormat(sDate1);
                        CDate1 = Convert.ToDateTime(sDate1);

                        DateTime lastUpdated = getdata.GetDocumentMax_ActualDate(new Guid(Request.QueryString["DocID"]));
                        if (lastUpdated.Date > CDate1.Date)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Actual Date should be greater than previous date.');</script>");
                        }
                        else
                        {
                            if (FileUploadDoc.HasFile)
                            {
                                //string projectName = getdata.getProjectNameby_ProjectUID(new Guid(Request.QueryString["PrjUID"].ToString()));
                                string LinkPath = Request.QueryString["ProjectUID"] + "/" + StatusUID + "/Link Document";
                                if (!Directory.Exists(Server.MapPath(LinkPath)))
                                {
                                    Directory.CreateDirectory(Server.MapPath(LinkPath));
                                }
                                string sFileName = Path.GetFileNameWithoutExtension(FileUploadDoc.FileName);
                                string Extn = System.IO.Path.GetExtension(FileUploadDoc.FileName);

                                FileUploadDoc.SaveAs(Server.MapPath(LinkPath + "/" + sFileName + "_1_copy" + Extn));
                                //FileUploadDoc.SaveAs(Server.MapPath("~/Documents/Encrypted/" + StatusUID + "_" + "1" + "_enp" + InputFile));
                                string savedPath = LinkPath + "/" + sFileName + "_1_copy" + Extn;
                                DocPath = LinkPath + "/" + sFileName + "_1" + Extn;
                                getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DocPath));

                                //DocPath = "~/Documents/" + projectName + "/" + StatusUID + "_" + "1" + InputFile;
                                //EncryptFile(Server.MapPath(savedPath), Server.MapPath(DocPath));
                                //  File.Encrypt(Server.MapPath(DocPath));
                            }
                            string DocumentDate = string.Empty;
                            if (dtDocumentDate.Text != "")
                            {
                                DocumentDate = dtDocumentDate.Text;
                            }
                            else
                            {
                                DocumentDate = DateTime.MinValue.ToString("MM/dd/yyyy");
                            }
                            //DocumentDate = DocumentDate.Split('/')[1] + "/" + DocumentDate.Split('/')[0] + "/" + DocumentDate.Split('/')[2];
                            DocumentDate = getdata.ConvertDateFormat(DocumentDate);
                            DateTime Document_Date = Convert.ToDateTime(DocumentDate);


                            if (FileUploadCoverLetter.HasFile)
                            {
                                string FileDatetime = DateTime.Now.ToString("dd MMM yyyy hh-mm-ss tt");
                                string sDocumentPath = string.Empty;
                                //sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/" + Request.QueryString["DocID"] + "/" + StatusUID + "/" + FileDatetime + "/CoverLetter";
                                sDocumentPath = "~/" + Request.QueryString["ProjectUID"] + "/" + StatusUID + "/CoverLetter";

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


                            //
                            //sDate2 = (dtPlannedDate.FindControl("txtDate") as TextBox).Text;
                            //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                            //CDate2 = Convert.ToDateTime(sDate2);
                            int Cnt = getdata.InsertorUpdateDocumentStatus(StatusUID, new Guid(Request.QueryString["DocID"].ToString()), 1, DDlStatus.SelectedItem.Text, 0, CDate1, DocPath,
                                new Guid(Session["UserUID"].ToString()), txtcomments.Text, DDlStatus.SelectedItem.Text, txtrefNumber.Text, Document_Date, CoverPagePath, RBLDocumentStatusUpdate.SelectedValue, RBLOriginator.SelectedValue);
                            if (Cnt > 0)
                            {
                                //DataSet ds = getdata.getAllUsers();
                                getdata.StoreFreshTargetDatesforStatusChange(new Guid(Request.QueryString["DocID"].ToString()), DDlStatus.SelectedItem.Text);

                                DataSet ds = new DataSet();
                                //if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                                //{
                                //    ds = getdata.getAllUsers();
                                //}
                                //else if (Session["TypeOfUser"].ToString() == "PA")
                                //{
                                    //ds = getdata.getUsers_by_ProjectUnder(new Guid(DDlProject.SelectedValue));
                                    ds = getdata.GetUsers_under_ProjectUID(new Guid(Request.QueryString["ProjectUID"]));
                                //}
                                //else
                                //{
                                //    ds = getdata.getUsers_by_ProjectUnder(new Guid(Request.QueryString["ProjectUID"]));
                                //}

                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string CC = string.Empty;
                                    string ToEmailID = "";
                                    string sUserName = "";
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        if (ds.Tables[0].Rows[i]["UserUID"].ToString() == Session["UserUID"].ToString())
                                        {
                                            ToEmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                                            sUserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                                        }
                                        else
                                        {
                                            if (getdata.GetUserMailAccess(new Guid(ds.Tables[0].Rows[i]["UserUID"].ToString()), "documentmail") != 0)
                                            {
                                                CC += ds.Tables[0].Rows[i]["EmailID"].ToString() + ",";
                                            }
                                           
                                        }
                                    }
                                    // added on 14/03/2022 to store mail for Contractor........
                                    if (DDlStatus.SelectedItem.ToString().Contains("AEE Approval") || DDlStatus.SelectedItem.ToString().Contains("CE Approval") || DDlStatus.SelectedItem.ToString() == "PMC Specialist Review")
                                    {
                                        DataSet dsMUSers = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"].ToString()), 1);
                                        if (dsMUSers.Tables[0].Rows.Count > 0)
                                        {
                                            foreach (DataRow druser in dsMUSers.Tables[0].Rows)
                                            {
                                                CC += getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString())) + ",";
                                            }
                                        }
                                    }
                                    //
                                    CC = CC.TrimEnd(',');
                                    sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                      "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "<style>table, th, td {border: 1px solid black; padding:6px;}</style></head>" +
                                         "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                    sHtmlString += "<div style='width:80%; float:left; padding:1%; border:2px solid #011496; border-radius:5px;'>" +
                                                       "<div style='float:left; width:100%; border-bottom:2px solid #011496;'>";
                                    if (WebConfigurationManager.AppSettings["Domain"] == "NJSEI")
                                    {
                                        sHtmlString += "<div style='float:left; width:7%;'><img src='https://dm.njsei.com/_assets/images/NJSEI%20Logo.jpg' width='50' /></div>";
                                        RefNostring = "NJSEI Ref Number";
                                    }
                                    else
                                    {
                                        sHtmlString += "<div style='float:left; width:7%;'><h2>" + WebConfigurationManager.AppSettings["Domain"] + "</h2></div>";
                                        RefNostring = "ONTB Ref Number";
                                    }
                                    sHtmlString += "<div style='float:left; width:70%;'><h2 style='margin-top:10px;'>Project Monitoring Tool</h2></div>" +
                                               "</div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + sUserName + " has changed " + DDLDocument.SelectedItem.Text + " status.</span> <br/><br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                    "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLDocument.SelectedItem.Text + "</td></tr>" +
                                                    "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDlStatus.SelectedItem.Text + "</td></tr>" +
                                                        "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + OriginatorRefNo + "</td></tr>" +
                                                    "<tr><td><b>" + RefNostring + "</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNo + "</td></tr>" +
                                                    "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + CDate1.ToString("dd MMM yyyy") + "</td></tr>" +
                                                    "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtcomments.Text + "</td></tr>";
                                    sHtmlString += "</table></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";

                                    //sHtmlString = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" + "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                    //       "<head>" + "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" + "</head>" +
                                    //          "<body style='font-family:Verdana, Arial, sans-serif; font-size:12px; font-style:normal;'>";
                                    //sHtmlString += "<div style='float:left; width:100%; height:30px;'>" +
                                    //                   "Dear, " + "Users" +
                                    //                   "<br/><br/></div>";
                                    //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Below are the Status details. <br/><br/></div>";
                                    //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Document Name : " + DDLDocument.SelectedItem.Text + "<br/></div>";
                                    //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Status : " + DDlStatus.SelectedItem.Text + "<br/></div>";
                                    //sHtmlString += "<div style='width:100%; float:left;'><div style='width:100%; float:left;'>Date : " + CDate1.ToShortDateString() + "<br/></div>";
                                    //sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> Project Manager.</div></div></body></html>";
                                    //string ret = getdata.SendMail(ds.Tables[0].Rows[0]["EmailID"].ToString(), Subject, sHtmlString, CC, Server.MapPath(DocPath));
                                    // added on 02/11/2020
                                    DataTable dtemailCred = getdata.GetEmailCredentials();
                                    Guid MailUID = Guid.NewGuid();
                                    getdata.StoreEmaildataToMailQueue(MailUID, new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, CC, "");
                                    //
                                    // added on 07/01/2022 for sending mail to next user in line to change status...
                                    DataSet dsnew = getdata.getTop1_DocumentStatusSelect(new Guid(Request.QueryString["DocID"]));
                                    DataSet dsNext = getdata.GetNextStep_By_DocumentUID(new Guid(Request.QueryString["DocID"]), dsnew.Tables[0].Rows[0]["ActivityType"].ToString());

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
                                    sHtmlString += "<div style='width:100%; float:left;'><br/>Dear Users,<br/><br/><span style='font-weight:bold;'>" + sUserName + " has changed " + DDLDocument.SelectedItem.Text + " status.</span> <br/><br/></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><table style='width:100%;'>" +
                                                    "<tr><td><b>Document Name </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDLDocument.SelectedItem.Text + "</td></tr>" +
                                                    "<tr><td><b>Status </b></td><td style='text-align:center;'><b>:</b></td><td>" + DDlStatus.SelectedItem.Text + "</td></tr>" +
                                                       "<tr><td><b>Originator Ref. Number </b></td><td style='text-align:center;'><b>:</b></td><td>" + OriginatorRefNo + "</td></tr>" +
                                                    "<tr><td><b>" + RefNostring + "</b></td><td style='text-align:center;'><b>:</b></td><td>" + ProjectRefNo + "</td></tr>" +
                                                    "<tr><td><b>Date </b></td><td style='text-align:center;'><b>:</b></td><td>" + CDate1.ToString("dd MMM yyyy") + "</td></tr>" +
                                                    "<tr><td><b>Comments </b></td><td style='text-align:center;'><b>:</b></td><td>" + txtcomments.Text + "</td></tr>";
                                    sHtmlString += "</table><br /><br /><div style='color: red'>Kindly note that you are to act on this to complete the next step in document flow.</div></div>";
                                    sHtmlString += "<div style='width:100%; float:left;'><br/><br/>Sincerely, <br/> MIS System.</div></div></body></html>";
                                    Subject = Subject + ".Kindly complete the next step !";
                                    string next = string.Empty;
                                    DataSet dsNxtUser = new DataSet();
                                    foreach (DataRow dr in dsNext.Tables[0].Rows)
                                    {
                                        dsNxtUser = getdata.GetNextUser_By_DocumentUID(new Guid(Request.QueryString["DocID"]), int.Parse(dr["ForFlow_Step"].ToString()));
                                        foreach (DataRow druser in dsNxtUser.Tables[0].Rows)
                                        {
                                            ToEmailID = getdata.GetUserEmail_By_UserUID_New(new Guid(druser["Approver"].ToString()));
                                            if (!next.Contains(ToEmailID))
                                            {
                                                getdata.StoreEmaildataToMailQueue(Guid.NewGuid(), new Guid(Session["UserUID"].ToString()), dtemailCred.Rows[0][0].ToString(), ToEmailID, Subject, sHtmlString, "", "");
                                                next += ToEmailID;
                                            }
                                           
                                        }
                                    }

                                }
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                            }
                            else
                            {

                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error: There is a problem with this feature. Please contact system admin');</script>");

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code : ADS 12 - There is a problem with this feature. Please contact system admin');</script>");
                    }
                }
            }
        }

        private void BindOriginator()
        {
            DataSet ds = getdata.GetOriginatorMaster();
            RBLOriginator.DataTextField = "Originator_Name";
            RBLOriginator.DataValueField = "Originator_Name";
            RBLOriginator.DataSource = ds;
            RBLOriginator.DataBind();
            // added by zuber on 02/03/2022 for KIADB
            string wkpgUId = getdata.GetWkpkgUIDbyDocUID(new Guid(Request.QueryString["DocID"].ToString()));
            if (getdata.GetClientCodebyWorkpackageUID(new Guid(wkpgUId)) != "")
            {
                RBLOriginator.Items.Insert(0, new ListItem(getdata.GetClientCodebyWorkpackageUID(new Guid(wkpgUId)), getdata.GetClientCodebyWorkpackageUID(new Guid(wkpgUId))));
            }
            //
            if (ds.Tables[0].Rows.Count > 0)
            {
                // RBLOriginator.Items[0].Selected = true;
                if (Session["IsContractor"].ToString() == "Y")
                {
                    if (RBLOriginator.Items.FindByValue("Contractor") != null)
                    {
                        RBLOriginator.Items.FindByValue("Contractor").Selected = true;
                    }
                }
                else if (Session["IsONTB"].ToString() == "Y")
                {
                    if (RBLOriginator.Items.FindByValue("ONTB") != null)
                    {
                        RBLOriginator.Items.FindByValue("ONTB").Selected = true;
                    }
                }
                else if (Session["IsNJSEI"].ToString() == "Y")
                {
                    if (RBLOriginator.Items.FindByValue("NJSEI") != null)
                    {
                        RBLOriginator.Items.FindByValue("NJSEI").Selected = true;
                    }
                }
                else if (Session["IsClient"].ToString() == "Y")
                {
                    if (RBLOriginator.Items.FindByValue("BWSSB") != null)
                    {
                        RBLOriginator.Items.FindByValue("BWSSB").Selected = true;
                    }
                }
                else
                    RBLOriginator.Items[0].Selected = true;
            }
        }

        protected void chkforward_CheckedChanged(object sender, EventArgs e)
        {
            if(chkforward.Checked)
            {
                divStatus.Visible = false;
                txtcomments.Text = "forward of document to next level";
                txtcomments.Enabled = false;
            }
            else
            {
                divStatus.Visible = true;
                txtcomments.Text = "";
                txtcomments.Enabled = true;
            }
        }

        protected void DDlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetAllUSersforPMC();
        }

        private void GetAllUSersforPMC()
        {
            try
            {

                DataSet dsMusers = getdata.GetAllUsersfor_BacktoPMC(new Guid(Request.QueryString["DocID"].ToString()), 3);
                if (dsMusers.Tables[0].Rows.Count > 0)
                {
                    chkUserList.DataSource = dsMusers;
                    chkUserList.DataTextField = "Name";
                    chkUserList.DataValueField = "Approver";
                    chkUserList.DataBind();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}