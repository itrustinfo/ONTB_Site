﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._content_pages.document_search_new
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        GeneralDocuments GD = new GeneralDocuments();
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
                    BindProject();
                    if (Session["searchedit"] != null)
                    {
                        DDlProject.SelectedValue = Session["PUID"].ToString();
                        txtSubmittal.Text = Session["sSubmittal"].ToString();
                        txtDocName.Text = Session["sDocName"].ToString();
                        dtInDate.Text = Session["sDate"].ToString();
                        dtInToDate.Text = Session["sDateTo"].ToString();
                        dtDocDate.Text = Session["sDocDate"].ToString();
                        dtDocToDate.Text = Session["sDocDateTo"].ToString();
                    }
                    else
                    {
                        if(Request.QueryString["PrjUID"]!= null)
                        {
                            DDlProject.SelectedValue = Request.QueryString["PrjUID"].ToString();
                            txtSubmittal.Text = Request.QueryString["Sub"].ToString();
                            txtDocName.Text = Request.QueryString["Doc"].ToString();
                            if (!string.IsNullOrEmpty(Request.QueryString["Indate"].ToString()))
                            {
                                dtInDate.Text = Request.QueryString["Indate"].ToString();
                            }
                            if (!string.IsNullOrEmpty(Request.QueryString["Intodate"].ToString()))
                            {
                                dtInToDate.Text = Request.QueryString["Intodate"].ToString();
                            }
                            if (!string.IsNullOrEmpty(Request.QueryString["DocDate"].ToString()))
                            {
                                dtDocDate.Text = Request.QueryString["DocDate"].ToString();
                            }
                            if (!string.IsNullOrEmpty(Request.QueryString["DocToDate"].ToString()))
                            {
                                dtDocToDate.Text = Request.QueryString["DocToDate"].ToString();
                            }
                            txtPageSize.Text = Request.QueryString["Page"].ToString();



                        }
                        else if (Request.QueryString["loadWkpckg"] != null)
                        {
                            DDlProject.SelectedValue = Request.QueryString["PrjectUID"].ToString();

                        }
                    }
                    DDlProject_SelectedIndexChanged(sender, e);
                    //BindStatus();
                    //BindType();
                    // BindDocuments();
                    BindTypeGD();
                    ViewState["SortDireaction"] = "";
                    ViewState["_sortDirection"] = "";
                    Session["isDownloadNJSE"] = false;
                    DataSet dscheck = new DataSet();
                    dscheck = getdt.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
                    if (dscheck.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dscheck.Tables[0].Rows)
                        {
                            
                            if (dr["Code"].ToString() == "FN") // DOWNLOAD APPROVED DOCUMENTS (FINAL APPROVED BY CLIENT)
                            {
                                ViewState["isDownloadClient"] = "true";
                                Session["isDownloadClient"] = "true";
                            }
                            if (dr["Code"].ToString() == "FM") // DOWNLOAD APPROVED DOCUMENTS (FINAL APPROVED BY NJSEI)
                            {
                                ViewState["isDownloadNJSE"] = "true";
                                Session["isDownloadNJSE"] = "true";
                            }
                        }
                    }
                   
                }
            }
        }

        public string GetSubmittalName(string DocumentID)
        {
            return getdt.getDocumentName_by_DocumentUID(new Guid(DocumentID));
        }
        public string GetDocumentTypeIcon(string DocumentExtn)
        {
            return getdt.GetDocumentTypeMasterIcon_by_Extension(DocumentExtn);
        }

        public string GetDocumentName(string DocumentExtn)
        {
            string retval = getdt.GetDocumentMasterType_by_Extension(DocumentExtn);
            if (retval == null || retval == "")
            {
                return "N/A";
            }
            else
            {
                return retval;
            }
        }

        private void BindStatus()
        {
            ddlstatus.DataSource = getdt.GetStatusForSearch(new Guid(DDLWorkPackage.SelectedValue));
            ddlstatus.DataTextField = "ActualDocument_CurrentStatus";
            ddlstatus.DataValueField = "ActualDocument_CurrentStatus";
            ddlstatus.DataBind();
            ddlstatus.Items.Insert(0, "All");
        }

        private void BindType()
        {
            DataSet ds = getdt.GetDoctypeForSearch(new Guid(DDLWorkPackage.SelectedValue));
            string name = "";
            ddlType.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["DocumentType"].ToString() == "Word")
                {
                   
                    if (name != dr["DocumentType"].ToString())
                    {
                        name = dr["DocumentType"].ToString();
                        ddlType.Items.Add(new ListItem(dr["DocumentType"].ToString(), ".doc"));
                    }
                   
                }
                else if (dr["DocumentType"].ToString() == "Excel")
                {
                    if (name != dr["DocumentType"].ToString())
                    {
                        name = dr["DocumentType"].ToString();
                        ddlType.Items.Add(new ListItem(dr["DocumentType"].ToString(), ".xls"));
                    }

                }
                else
                {
                    ddlType.Items.Add(new ListItem(dr["DocumentType"].ToString(), dr["DocumentExtension"].ToString()));
                }
            }

            //ddlType.DataSource = getdt.GetDoctypeForSearch(new Guid(DDLWorkPackage.SelectedValue));
            //ddlType.DataTextField = "DocumentType";
            //ddlType.DataValueField = "DocumentExtension";
            //ddlType.DataBind();
            ddlType.Items.Insert(0, "All");
        }

        private void BindTypeGD()
        {
            DataSet ds = GD.GetDoctypeForSearchGD();
            string name = "";
            ddlDocType.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["DocumentType"].ToString() == "Word")
                {

                    if (name != dr["DocumentType"].ToString())
                    {
                        name = dr["DocumentType"].ToString();
                        ddlDocType.Items.Add(new ListItem(dr["DocumentType"].ToString(), ".doc"));
                    }

                }
                else if (dr["DocumentType"].ToString() == "Excel")
                {
                    if (name != dr["DocumentType"].ToString())
                    {
                        name = dr["DocumentType"].ToString();
                        ddlDocType.Items.Add(new ListItem(dr["DocumentType"].ToString(), ".xls"));
                    }

                }
                else
                {
                    ddlDocType.Items.Add(new ListItem(dr["DocumentType"].ToString(), dr["DocumentExtension"].ToString()));
                }
            }

            //ddlType.DataSource = getdt.GetDoctypeForSearch(new Guid(DDLWorkPackage.SelectedValue));
            //ddlType.DataTextField = "DocumentType";
            //ddlType.DataValueField = "DocumentExtension";
            //ddlType.DataBind();
            ddlDocType.Items.Insert(0, "All");
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = gettk.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }

            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();
            DDlProject.Items.Add("General Documents");

        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                DataSet ds = new DataSet();
               if(DDlProject.SelectedItem.ToString() == "General Documents")
                {
                    BindDocumentsGD();
                    divMain.Visible = false;
                    divGeneral.Visible = true;
                    divWP.Visible = false;
                }
                else {
                    divMain.Visible = true;
                    divGeneral.Visible = false;
                    divWP.Visible = true;
                    ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();
                    BindStatus();
                    BindType();
                    if (Session["searchedit"] != null)
                    {
                        ddlstatus.SelectedValue = Session["sStatus"].ToString();
                        ddlType.SelectedValue = Session["sType"].ToString();
                    }
                        else if(Request.QueryString["status"] != null)
                        {
                            ddlstatus.SelectedValue = Request.QueryString["status"].ToString();
                            ddlType.SelectedValue = Request.QueryString["type"].ToString();
                        }
                    BindDocuments();
                }
                else
                {
                    DDLWorkPackage.DataSource = null;
                    DDLWorkPackage.DataBind();
                
                }
                }
            }
          
        }

        protected void GrdDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDocuments.PageIndex = e.NewPageIndex;
            HiddenPaging.Value = "true";
            Session["PageIndex"] = GrdDocuments.PageIndex;
            BindDocuments();
           
        }

        private void BindDocuments()
        {
            if (Session["searchedit"] != null)
            {
                GrdDocuments.PageIndex = Convert.ToInt32(Session["PageIndex"]);
            }

          
            Session["searchedit"] = null;
            Session["sSubmittal"] = txtSubmittal.Text;
            Session["sDocName"] = txtDocName.Text;
            Session["sDate"] = dtInDate.Text;
            Session["sDocDate"] = dtDocDate.Text;
            Session["sType"] = ddlType.SelectedValue;
            Session["sStatus"] = ddlstatus.SelectedValue;
            Session["sDateTo"] = dtInToDate.Text;
            Session["sDocDateTo"] = dtDocToDate.Text;

            DateTime InDate = DateTime.Now;
            DateTime DocumentDate = DateTime.Now;
            DateTime InToDate = DateTime.Now;
            DateTime DocumentToDate = DateTime.Now;
            if (!string.IsNullOrEmpty(dtInDate.Text))
            {
                InDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtInDate.Text));
            }
            if (string.IsNullOrEmpty(dtInToDate.Text))
            {
                InToDate = InDate;
            }
           
            if (!string.IsNullOrEmpty(dtDocDate.Text))
            {
                DocumentDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtDocDate.Text));
            }
            if (string.IsNullOrEmpty(dtDocToDate.Text))
            {
                DocumentToDate = DocumentDate;
            }
            if (!string.IsNullOrEmpty(dtInToDate.Text))
            {
                InToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtInToDate.Text));
            }
            if (!string.IsNullOrEmpty(dtDocToDate.Text))
            {
                DocumentToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtDocToDate.Text));
            }


            // validations
            if (dtInDate.Text == "" && dtInToDate.Text != "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Incoming Recv From Date');</script>");
                return;
            }
            else if (dtInDate.Text != "" && dtInToDate.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Incoming Recv To Date');</script>");
                return;
            }
            else if (dtInDate.Text != "" && dtInToDate.Text != "")
            {
                if (InDate > InToDate)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Incoming Recv From Date cannot be greater than Incoming Recv To Date');</script>");
                    return;
                }

            }

            if (dtDocDate.Text == "" && dtDocToDate.Text != "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Document From Date');</script>");
                return;
            }
            else if (dtDocDate.Text != "" && dtDocToDate.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Document To Date');</script>");
                return;
            }
            else if (dtDocDate.Text != "" && dtDocToDate.Text != "")
            {
                if (DocumentDate > DocumentToDate)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Document From Date cannot be greater than Document To Date');</script>");
                    return;
                }
            }

            DataSet ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDlProject.SelectedValue),new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, InDate,DocumentDate,InToDate,DocumentToDate, 4);
            if (dtDocDate.Text != "" && dtInDate.Text != "")
            {
                ds = new DataSet();
                ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 3);
            }
            else if (dtInDate.Text !="")
            {
                ds = new DataSet();
                ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDlProject.SelectedValue),new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 1);
            }
            else if (dtDocDate.Text != "")
            {
                ds = new DataSet();
                ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 2);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                lbldocNos.Text = ds.Tables[0].Rows.Count.ToString();
            }
            else
            {
                lbldocNos.Text = "0";
            }
            
            //  DataSet ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue),"","","","",DocDate,1);



            //if (txtDocName.Text != "" && ddlType.SelectedIndex ==0 && txtSubmittal.Text =="" && ddlstatus.SelectedIndex == 0  && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, "", "", "", DocDate, 2);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, "", "", DocDate, 3);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, "", DocDate, 4);

            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 5);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text =="" )
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 6);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 7);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 8);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 9);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 10);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 11);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 12);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 13);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 14);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 15);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text == "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 16);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 17);
            //}
            //else if (txtDocName.Text != "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 18);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex != 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 19);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text != "" && ddlstatus.SelectedIndex == 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 20);
            //}
            //else if (txtDocName.Text == "" && ddlType.SelectedIndex == 0 && txtSubmittal.Text == "" && ddlstatus.SelectedIndex != 0 && dtDocumentDate.Text != "")
            //{
            //    ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(DDLWorkPackage.SelectedValue), txtDocName.Text, ddlType.SelectedValue, txtSubmittal.Text, ddlstatus.SelectedValue, DocDate, 21);
            //}
            //
            
            
            GrdDocuments.DataSource = ds;
            GrdDocuments.DataBind();
            
            ViewState["datatable"] = ds.Tables[0];

            //
            if (ds.Tables[0].Rows.Count > 10  && int.Parse(txtPageSize.Text) > 10)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + GrdDocuments.ClientID + "', 600, 1300 , 55 ,true); </script>", false);
            }
        }

        protected void GrdDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //LinkButton lnkbtn;
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    foreach (TableCell cell in e.Row.Cells)
            //    {
            //        lnkbtn = (LinkButton)cell.Controls[0];
            //        if (!string.IsNullOrEmpty(GrdDocuments.SortExpression))
            //        {
            //            if (GrdDocuments.SortExpression.Equals(lnkbtn.Text))
            //            {
            //                cell.BackColor = System.Drawing.Color.Crimson;
            //            }
            //        }
            //    }
            //}
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataSet ds = getdt.getTop1_DocumentStatusSelect(new Guid(e.Row.Cells[0].Text));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows[0]["DocumentType"].ToString() == "General Document")
                    {
                        e.Row.Cells[8].Text = "No History";
                    }
                }

            }
        }

        protected void GrdDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                string filename = string.Empty;
                DataSet ds1 = null;
                DataSet ds = getdt.getLatest_DocumentVerisonSelect(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["Doc_FileName"].ToString());
                    //File.Decrypt(path);
                }
                else
                {
                    ds1 = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString() != "")
                        {
                            path = Server.MapPath(ds1.Tables[0].Rows[0]["ActualDocument_Path"].ToString());
                        }
                    }
                }
                // added on  20/10/2020
                ds.Clear();
                ds = getdt.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() != "")
                    {
                        filename = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString() + ds.Tables[0].Rows[0]["ActualDocument_Type"].ToString();
                    }
                }
                //
                string getExtension = System.IO.Path.GetExtension(path);
                string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                getdt.DecryptFile(path, outPath);
                System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                if (file.Exists)
                {
                    int Cnt = getdt.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(UID), new Guid(Session["UserUID"].ToString()), "Downloaded", "Documents");
                    if (Cnt <= 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                    }
                    Response.Clear();

                    // Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                    Response.AddHeader("Content-Length", file.Length.ToString());

                    Response.ContentType = "application/octet-stream";

                    Response.WriteFile(file.FullName);

                    Response.Flush();

                    try
                    {
                        if (File.Exists(outPath))
                        {
                            File.Delete(outPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw
                    }

                    Response.End();

                }
                else
                {

                    //Response.Write("This file does not exist.");
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

                }
            }
        }

        protected void GrdDocuments_Sorting(object sender, GridViewSortEventArgs e)
        {

            DataTable dataTable = ViewState["datatable"] as DataTable;

            //if (dataTable != null)
            //{
            //    DataView dataView = new DataView(dataTable);
            //    dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(e.SortDirection);

            //    GrdDocuments.DataSource = dataView;
            //    GrdDocuments.DataBind();
            //}
            SetSortDirection(ViewState["SortDireaction"].ToString());
            if (dataTable != null)
            {
                //Sort the data.
                dataTable.DefaultView.Sort = e.SortExpression + " " + ViewState["_sortDirection"].ToString();
                GrdDocuments.DataSource = dataTable;
                GrdDocuments.DataBind();
                ViewState["SortDireaction"] = ViewState["_sortDirection"];
            }
        }

       

        //private string ConvertSortDirectionToSql(SortDirection sortDirection)
        //{
        //    string newSortDirection = String.Empty;

        //    switch (sortDirection)
        //    {
        //        case SortDirection.Ascending:
        //            newSortDirection = "ASC";
        //            break;

        //        case SortDirection.Descending:
        //            newSortDirection = "DESC";
        //            break;
        //    }

        //    return newSortDirection;
        //}

        protected void SetSortDirection(string sortDirection)
        {
            if (sortDirection == "ASC")
            {
                ViewState["_sortDirection"] = "DESC";
            }
            else
            {
                ViewState["_sortDirection"] = "ASC";
            }
        }

        protected void btnSubmit_Click1(object sender, EventArgs e)
        {
            try
            {
                //Session["searchedit"] = null;
                //Session["sSubmittal"] = txtSubmittal.Text;
                //Session["sDocName"] = txtDocName.Text;
                //Session["sDate"] = dtInDate.Text;
                //Session["sType"] = ddlType.SelectedValue;
                //Session["sStatus"] = ddlstatus.SelectedValue;
                GrdDocuments.Visible = true;
                GrdDocuments.PageSize = int.Parse(txtPageSize.Text);
                if (HiddenPaging.Value != "true")
                {
                    GrdDocuments.PageIndex = 0;
                }
                BindDocuments();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            Session["searchedit"] = null;
            Response.Redirect("default.aspx");
        }

        protected void GrdDocuments_DataBound(object sender, EventArgs e)
        {
            //int sortedColumnPosition = 0;
            //LinkButton lnkbtn;

            //// Gets position of column whose header text matches SortExpression
            //// of the GridView when column is sorted
            //foreach (TableCell cell in GrdDocuments.HeaderRow.Cells)
            //{
            //    lnkbtn = (LinkButton)cell.Controls[0];
            //    if (lnkbtn.Text == GrdDocuments.SortExpression)
            //    {
            //        break;
            //    }
            //    sortedColumnPosition++;
            //}
            //if (!string.IsNullOrEmpty(GrdDocuments.SortExpression))
            //{
            //    foreach (GridViewRow row in GrdDocuments.Rows)
            //    {
            //        row.Cells[sortedColumnPosition].BackColor = System.Drawing.Color.LavenderBlush;
            //    }
            //}
        }

        protected void btnSubmitGD_Click(object sender, EventArgs e)
        {
            try
            {
                grdGeneralDocuments.PageSize = int.Parse(txtGDRecords.Text);
                if (HiddenPaging.Value != "true")
                {
                    grdGeneralDocuments.PageIndex = 0;
                }
                BindDocumentsGD();
            }
            catch(Exception ex)
            {

            }
        }

        protected void bthClearGD_Click(object sender, EventArgs e)
        {
            Session["searchedit"] = null;
            txtDocNameGD.Text = "";
            ddlDocType.SelectedIndex = 0;
            dtInDateGD.Text = "";
            dtInToDateGD.Text = "";
            dtDocToDateGD.Text = "";
            dtDocDateGD.Text = "";
            BindDocumentsGD();
        }

        private void BindDocumentsGD()
        {
            //if (Session["searchedit"] != null)
            //{
            //    grdGeneralDocuments.PageIndex = Convert.ToInt32(Session["PageIndex"]);
            //}


            //Session["searchedit"] = null;
            //Session["sSubmittal"] = txtSubmittal.Text;
            //Session["sDocName"] = txtDocName.Text;
            //Session["sDate"] = dtInDate.Text;
            //Session["sDocDate"] = dtDocDate.Text;
            //Session["sType"] = ddlType.SelectedValue;
            //Session["sStatus"] = ddlstatus.SelectedValue;
            //Session["sDateTo"] = dtInToDate.Text;
            //Session["sDocDateTo"] = dtDocToDate.Text;

            DateTime InDate = DateTime.Now;
            DateTime DocumentDate = DateTime.Now;
            DateTime InToDate = DateTime.Now;
            DateTime DocumentToDate = DateTime.Now;
            if (!string.IsNullOrEmpty(dtInDateGD.Text))
            {
                InDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtInDateGD.Text));
            }
            if (string.IsNullOrEmpty(dtInToDateGD.Text))
            {
                InToDate = InDate;
            }

            if (!string.IsNullOrEmpty(dtDocDateGD.Text))
            {
                DocumentDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtDocDateGD.Text));
            }
            if (string.IsNullOrEmpty(dtDocToDateGD.Text))
            {
                DocumentToDate = DocumentDate;
            }
            if (!string.IsNullOrEmpty(dtInToDateGD.Text))
            {
                InToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtInToDateGD.Text));
            }
            if (!string.IsNullOrEmpty(dtDocToDateGD.Text))
            {
                DocumentToDate = Convert.ToDateTime(getdt.ConvertDateFormat(dtDocToDateGD.Text));
            }


            // validations
            if (dtInDateGD.Text == "" && dtInToDateGD.Text != "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Incoming Recv From Date');</script>");
                return;
            }
            else if (dtInDateGD.Text != "" && dtInToDateGD.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Incoming Recv To Date');</script>");
                return;
            }
            else if (dtInDateGD.Text != "" && dtInToDateGD.Text != "")
            {
                if (InDate > InToDate)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Incoming Recv From Date cannot be greater than Incoming Recv To Date');</script>");
                    return;
                }

            }

            if (dtDocDateGD.Text == "" && dtDocToDateGD.Text != "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Document From Date');</script>");
                return;
            }
            else if (dtDocDateGD.Text != "" && dtDocToDateGD.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Please enter Document To Date');</script>");
                return;
            }
            else if (dtDocDateGD.Text != "" && dtDocToDateGD.Text != "")
            {
                if (DocumentDate > DocumentToDate)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Alert", "<script language='javascript'>alert('Document From Date cannot be greater than Document To Date');</script>");
                    return;
                }
            }

            DataSet ds = GD.GeneralDocuments_Search(txtDocNameGD.Text, ddlDocType.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 4);
            if (dtDocDate.Text != "" && dtInDate.Text != "")
            {
                ds = new DataSet();
                ds = GD.GeneralDocuments_Search(txtDocNameGD.Text, ddlDocType.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 3);
            }
            else if (dtInDateGD.Text != "")
            {
                ds = new DataSet();
                ds = GD.GeneralDocuments_Search(txtDocNameGD.Text, ddlDocType.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 1);
            }
            else if (dtDocDateGD.Text != "")
            {
                ds = new DataSet();
                ds = GD.GeneralDocuments_Search(txtDocNameGD.Text, ddlDocType.SelectedValue, InDate, DocumentDate, InToDate, DocumentToDate, 2);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblGDnos.Text = ds.Tables[0].Rows.Count.ToString();
            }
            else
            {
                lblGDnos.Text = "0";
            }
            grdGeneralDocuments.DataSource = ds;
            grdGeneralDocuments.DataBind();

            ViewState["datatable"] = ds.Tables[0];

            //
            if (ds.Tables[0].Rows.Count > 10 && int.Parse(txtPageSize.Text) > 10)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeaderNew('" + grdGeneralDocuments.ClientID + "', 600, 1300 , 55 ,true); </script>", false);
            }
        }

        protected void grdGeneralDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdGeneralDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                string path = string.Empty;
                DataSet ds = GD.GeneralDocuments_SelectBy_GeneralDocumentUID(new Guid(UID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    path = Server.MapPath(ds.Tables[0].Rows[0]["GeneralDocument_Path"].ToString());

                    string getExtension = System.IO.Path.GetExtension(path);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdt.DecryptFile(path, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                    if (file.Exists)
                    {

                        int Cnt = getdt.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(UID), new Guid(Session["UserUID"].ToString()), "Downloaded", "General Documents");
                        if (Cnt <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                        }
                        Response.Clear();

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName);

                        Response.Flush();

                        try
                        {
                            if (File.Exists(outPath))
                            {
                                File.Delete(outPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            //throw
                        }

                        Response.End();


                    }
                    else
                    {

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exists.');</script>");

                    }
                }
            }
        }

        protected void grdGeneralDocuments_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable = ViewState["datatable"] as DataTable;

            
            SetSortDirection(ViewState["SortDireaction"].ToString());
            if (dataTable != null)
            {
                //Sort the data.
                dataTable.DefaultView.Sort = e.SortExpression + " " + ViewState["_sortDirection"].ToString();
                grdGeneralDocuments.DataSource = dataTable;
                grdGeneralDocuments.DataBind();
                ViewState["SortDireaction"] = ViewState["_sortDirection"];
            }
        }

        protected void grdGeneralDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdGeneralDocuments.PageIndex = e.NewPageIndex;
            HiddenPaging.Value = "true";
          //  Session["PageIndex"] = GrdDocuments.PageIndex;
            BindDocumentsGD();
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                divMain.Visible = true;
                divGeneral.Visible = false;
                divWP.Visible = true;

                BindStatus();
                BindType();
                if (Session["searchedit"] != null)
                {
                    ddlstatus.SelectedValue = Session["sStatus"].ToString();
                    ddlType.SelectedValue = Session["sType"].ToString();
                }
                BindDocuments();
            }
        }

        public string GetTaskHierarchy_By_DocumentUID(string DocumentUID)
        {
            return getdt.GetTaskHierarchy_By_DocumentUID(new Guid(DocumentUID));
        }


        [WebMethod(EnableSession = true)] //get client side data 
        public static string getData(string ProjectUID,string WorkPackageUID)
        {
            string result = "in server side";
            try
            {
                StringBuilder makeDiv = new StringBuilder();
                makeDiv.Append("<table border='1' width='100%'><tr><td> Submittal Name </td ><td> Document Name </td ><td> Document Type </td><td> Current Status </td><td> Incoming Recev Date</td><td> Document Date </td></tr>");
                DBGetData getdt = new DBGetData();
                DataSet ds = getdt.ActualDocuments_SelectBy_WorkpackageUID_Search(new Guid(ProjectUID), new Guid(WorkPackageUID), "", "All", "", "All", DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, 4);
                for(int i=0; i<=200;i ++)
                {
                    if (i < ds.Tables[0].Rows.Count)
                    {
                        makeDiv.Append("<tr><td>" + ds.Tables[0].Rows[i]["DocName"].ToString() + "</ td ><td> " + ds.Tables[0].Rows[i]["ActualDocument_Name"].ToString() + "</td ><td> Document Type </td><td> Current Status </td><td> Incoming Recev Date</td><td> Document Date </td>");
                    }
                }
                makeDiv.Append("</table>");
                return makeDiv.ToString();
            }
            catch (Exception ex)
            {
             
                return "N/A";
            }
        }
    }
}