﻿using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.NewIssues
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                //ScriptManager.RegisterStartupScript(
                //       UpdatePanel2,
                //       this.GetType(),
                //       "MyAction",
                //       "BindEvents();",
                //       true);
                if (!IsPostBack)
                {
                    HideActionButtons();
                    BindProject();
                    SelectedProject();
                    DDlProject_SelectedIndexChanged(sender, e);
                    Session["ProjectUID"] = DDlProject.SelectedValue;
                    Session["ProjectName"] = DDlProject.SelectedItem.Text;
                }
                else
                {
                    BindIssues("WorkPackage", TreeView1.SelectedNode.Value);
                }
            }
        }


        internal void HideActionButtons()
        {
            AddIssues.Visible = false;
            ViewState["isEdit"] = "false";
            ViewState["isAssignUser"] = "false";
            ViewState["isDelete"] = "false";
            GrdIssues.Columns[10].Visible = false;
            GrdIssues.Columns[11].Visible = false;
            GrdIssues.Columns[12].Visible = false;
            DataSet dscheck = new DataSet();
            dscheck = getdt.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
            if (dscheck.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dscheck.Tables[0].Rows)
                {
                    if (dr["Code"].ToString() == "IA")
                    {
                        AddIssues.Visible = true;
                    }

                    if (dr["Code"].ToString() == "IE")
                    {
                        GrdIssues.Columns[10].Visible = true;
                        ViewState["isEdit"] = "true";
                    }
                    if (dr["Code"].ToString() == "IAU" || dr["Code"].ToString() == "ID")
                    {
                        GrdIssues.Columns[11].Visible = true;
                        ViewState["isAssignUser"] = "true";
                    }
                    if (dr["Code"].ToString() == "ID")
                    {
                        GrdIssues.Columns[12].Visible = true;
                        ViewState["isDelete"] = "true";
                    }
                    
                }
            }
        }

        private void BindProject()
        {
            DataSet ds = new DataSet();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() =="MD" || Session["TypeOfUser"].ToString() == "VP")
            {
                ds = gettk.GetAllProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = getdt.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                ds = getdt.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDlProject.DataTextField = "ProjectName";
                DDlProject.DataValueField = "ProjectUID";
                DDlProject.DataSource = ds;
                DDlProject.DataBind();
            }
        }

        public void PopulateTreeView(DataSet dtParent, TreeNode treeNode, string ParentUID, int Level)
        {
            foreach (DataRow row in dtParent.Tables[0].Rows)
            {
                TreeNode child = new TreeNode
                {
                    Text = LimitCharts(row["Name"].ToString()),
                    Value = Level == 0 ? row["WorkPackageUID"].ToString() : row["TaskUID"].ToString(),
                    Target = Level == 0 ? "WorkPackage" : "Tasks",
                    ToolTip = row["Name"].ToString()
                };

                //if (ParentUID == "")
                //{
                //    TreeView1.Nodes.Add(child);
                //    DataSet dsworkPackage = getdt.GetWorkPackages_By_ProjectUID(new Guid(child.Value));
                //    if (dsworkPackage.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(dsworkPackage, child, child.Value, 1);
                //    }
                //}
                if (ParentUID == "")
                {
                    //treeNode.ChildNodes.Add(child);
                    TreeView1.Nodes.Add(child);
                    DataSet dschild = getdt.GetTasksForWorkPackages(child.Value);
                    //DataTable dtChild = TreeViewBAL.BL.TreeViewBL.GetData("Select ID,Name from Module where ProjID=" + child.Value);
                    if (dschild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dschild, child, child.Value, 1);
                    }

                }
                else if (Level == 1)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet dssubchild = getdt.GetSubTasksForWorkPackages(child.Value);
                    if (dssubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubchild, child, child.Value, 2);
                    }
                }
                else if (Level == 2)
                {
                    treeNode.ChildNodes.Add(child);
                    DataSet dssubtosubchild = getdt.GetSubtoSubTasksForWorkPackages(child.Value);
                    if (dssubtosubchild.Tables[0].Rows.Count > 0)
                    {
                        PopulateTreeView(dssubtosubchild, child, child.Value, 3);
                    }
                }
                //else if (Level == 3)
                //{
                //    treeNode.ChildNodes.Add(child);
                //    DataSet lastchild = getdt.GetSubtoSubtoSubTasksForWorkPackages(child.Value);
                //    if (lastchild.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(lastchild, child, child.Value, 4);
                //    }
                //}
                //else if (Level == 4)
                //{
                //    treeNode.ChildNodes.Add(child);
                //    DataSet lastchild = getdt.GetSubtoSubtoSubtoSubTasksForWorkPackages(child.Value);
                //    if (lastchild.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(lastchild, child, child.Value, 5);
                //    }
                //}
                //else
                //{
                //    treeNode.ChildNodes.Add(child);
                //    DataSet ds = getdt.GetTask_by_ParentTaskUID(new Guid(child.Value));
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        PopulateTreeView(ds, child, child.Value, 6);
                //    }
                //}
            }

        }

        public string LimitCharts(string Desc)
        {
            if (Desc.Length > 100)
            {
                return Desc.Substring(0, 100) + "  . . .";
            }
            else
            {
                return Desc;
            }
        }

        public string ShoworHide(string Desc)
        {
            if (Desc.Length > 45)
            {
                return "More";
            }
            else
            {
                return string.Empty;
            }
        }
        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            BindActivities();
        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                //AddIssues.Visible = true;
                //DataSet ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
                {
                    ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                else
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //DDLWorkPackage.DataTextField = "Name";
                    //DDLWorkPackage.DataValueField = "WorkPackageUID";
                    //DDLWorkPackage.DataSource = ds;
                    //DDLWorkPackage.DataBind();
                    TreeView1.Nodes.Clear();
                    PopulateTreeView(ds, null, "", 0);
                    TreeView1.Nodes[0].Selected = true;
                    TreeView1.CollapseAll();
                    TreeView1.Nodes[0].Expand();
                    //BindActivities();
                    TreeView1_SelectedNodeChanged(sender, e);
                    GrdIssues.Visible = true;
                    Session["WorkPackageUID"] = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                }
                else
                {
                    GrdIssues.Visible = false;
                }

                Session["Project_Workpackage"] = DDlProject.SelectedValue;
            }
            else
            {
                printreport.Visible = false;
                AddIssues.Visible = false;
            }
            
        }

        internal void SelectedProject()
        {
            if (!IsPostBack && Session["Project_Workpackage"] != null)
            {
                string[] selectedValue = Session["Project_Workpackage"].ToString().Split('_');
                if (selectedValue.Length > 1)
                {
                    DDlProject.SelectedValue = selectedValue[0];
                }
                else
                {
                    DDlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                }
            }
        }
        public void BindActivities()
        {

            if (TreeView1.SelectedNode.Target == "WorkPackage")
            {
                ActivityHeading.Text = TreeView1.SelectedNode.Text;
                AddIssues.HRef = "/_modal_pages/add-issues.aspx?IssueFor=WorkPackage&ActivityID=" + TreeView1.SelectedNode.Value + "&AName=" + TreeView1.SelectedNode.Text.Replace('&',' ') + "&PrjID=" + DDlProject.SelectedValue;
                BindIssues("WorkPackage", TreeView1.SelectedNode.Value);
                IssueCountLoad("WorkPackage", TreeView1.SelectedNode.Value);
            }
            else
            {
                ActivityHeading.Text = TreeView1.SelectedNode.Text;
                AddIssues.HRef = "/_modal_pages/add-issues.aspx?IssueFor=Taks&ActivityID=" + TreeView1.SelectedNode.Value + "&AName=" + TreeView1.SelectedNode.Text.Replace('&',' ') + "&PrjID=" + DDlProject.SelectedValue;
                BindIssues("Task", TreeView1.SelectedNode.Value);
                IssueCountLoad("Task", TreeView1.SelectedNode.Value);
            }

            Session["ActivityName"] = ActivityHeading.Text;
        }

        protected void BindIssues(string IssuesFor, string ActivityUID)
        {
            if (IssuesFor == "WorkPackage")
            {
                DataSet ds = getdt.getIssuesList_by_WorkPackageUID(new Guid(ActivityUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    printreport.Visible = true;
                }
                else
                {
                    printreport.Visible = false;
                }
                GrdIssues.DataSource = ds;
                GrdIssues.DataBind();
                GrdPrint.DataSource = ds;
                GrdPrint.DataBind();
                LblTotalIssues.Text = ds.Tables[0].Rows.Count.ToString();
            }
            else
            {
                DataSet ds = getdt.getIssuesList_by_TaskUID(new Guid(ActivityUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    printreport.Visible = true;
                }
                else
                {
                    printreport.Visible = false;
                }
                GrdIssues.DataSource = ds;
                GrdIssues.DataBind();
                GrdPrint.DataSource = ds;
                GrdPrint.DataBind();
                LblTotalIssues.Text = ds.Tables[0].Rows.Count.ToString();
            }

            

        }

        protected void IssueCountLoad(string IssuesFor, string ActivityID)
        {
            if (IssuesFor == "WorkPackage")
            {
                DataSet ds = getdt.Get_Open_Closed_Rejected_Issues_by_WorkPackageUID(new Guid(ActivityID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblOpenIssues.Text = ds.Tables[0].Rows[0]["OpenIssues"].ToString();
                    LblClosedIssues.Text = ds.Tables[0].Rows[0]["ClosedIssues"].ToString();
                    LblRejectedIssues.Text = ds.Tables[0].Rows[0]["RejectedIssues"].ToString();
                    LblInProgressIssues.Text= ds.Tables[0].Rows[0]["InProgressIssues"].ToString();
                }
            }
            else
            {
                DataSet ds = getdt.Get_Open_Closed_Rejected_Issues_by_TaskUID(new Guid(ActivityID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LblOpenIssues.Text = ds.Tables[0].Rows[0]["OpenIssues"].ToString();
                    LblClosedIssues.Text = ds.Tables[0].Rows[0]["ClosedIssues"].ToString();
                    LblRejectedIssues.Text = ds.Tables[0].Rows[0]["RejectedIssues"].ToString();
                    LblInProgressIssues.Text = ds.Tables[0].Rows[0]["InProgressIssues"].ToString();
                }
            }
        }
           
        protected void GrdIssues_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["isEdit"].ToString() == "false")
                {
                    e.Row.Cells[10].Visible = false;
                }
                if (ViewState["isAssignUser"].ToString() == "false")
                {
                    e.Row.Cells[11].Visible = false;
                }
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[12].Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ds.Clear();
                ds = getdt.getUserDetails(new Guid(e.Row.Cells[2].Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["UserName"].ToString();
                }
                ds.Clear();
                //ds = getdt.GetTasksby_TaskUiD(new Guid(e.Row.Cells[2].Text));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["Name"].ToString();
                //}

                

                if (e.Row.Cells[9].Controls.Count == 0)
                {
                    GridViewRow grd_row = e.Row;

                    GridView obj_grdview = new GridView();
                    obj_grdview.Visible = true;

                    DataSet ds1 = getdt.GetUploadedIssueDocuments(e.Row.Cells[14].Text);

                    ds1.Tables[0].Columns.Add("action-1");
                    ds1.Tables[0].Columns.Add("action-2");
                    ds1.Tables[0].Columns.Add("action-3");
                    ds1.Tables[0].Columns.Add("action-4");


                    int count = ds1.Tables[0].Rows.Count;

                    if (count == 0)
                    {
                        LinkButton new_link = new LinkButton();

                        new_link.ID = "No File";
                        new_link.Text = "No File";
                        new_link.Enabled = false;
                        grd_row.Cells[9].Controls.Add(new_link);

                        e.Row.Cells[8].Text = "";
                    }
                    else
                    {
                        obj_grdview.DataSource = ds1.Tables[0];
                        obj_grdview.DataBind();
                        obj_grdview.Attributes.Add("width", "100%");

                        Boolean preview = false;

                        foreach (GridViewRow grd_view_row in obj_grdview.Rows)
                        {

                            grd_view_row.Cells[0].Visible = false;
                            grd_view_row.Cells[1].Visible = false;
                            grd_view_row.Cells[2].Visible = false;

                            
                            LinkButton linkUpload = new LinkButton();

                            linkUpload.ID = "upload_" + grd_view_row.Cells[1].Text;
                            linkUpload.CommandName = "download";
                            linkUpload.CssClass = "fas fa-download";
                            linkUpload.ToolTip = "Download";
                           
                            linkUpload.CommandArgument = grd_view_row.Cells[2].Text + "/" + grd_view_row.Cells[1].Text;

                            linkUpload.Click += linkUpload_Click;

                            grd_view_row.Cells[3].Controls.Add(linkUpload);


                            //-------------------------------------------

                            LinkButton linkInfo = new LinkButton();

                            linkInfo.ID = "info_" + grd_view_row.Cells[1].Text;
                            linkInfo.CommandName = grd_view_row.Cells[0].Text;
                           
                            linkInfo.CssClass = "fas fa-info";
                            linkInfo.ToolTip = grd_view_row.Cells[1].Text;
                            linkInfo.CommandArgument = grd_view_row.Cells[2].Text + "/" + grd_view_row.Cells[1].Text;

                            linkInfo.Click += LinkInfo_Click;

                            grd_view_row.Cells[4].Controls.Add(linkInfo);

                            //-------------------------------------------------------
                            LinkButton linkDelete = new LinkButton();

                            linkDelete.ID = "delete_" + grd_view_row.Cells[1].Text;
                            linkDelete.CommandName = grd_view_row.Cells[0].Text;
                           // linkDelete.Text = "Delete";
                            linkDelete.CssClass = "fas fa-trash";
                            linkDelete.ToolTip = "Delete";
                            linkDelete.CommandArgument = grd_view_row.Cells[2].Text + "/" + grd_view_row.Cells[1].Text;

                            linkDelete.Click += linkDelete_Click;
                           
                            grd_view_row.Cells[5].Controls.Add(linkDelete);

                            //-------------------------------------------------

                            string ext = linkUpload.CommandArgument.Substring(linkUpload.CommandArgument.Length - 3, 3);

                            if (ext == "jpg" || ext == "png" || ext == "pdf")
                            {
                                LinkButton linkView = new LinkButton();

                                linkView.ID = "view_" + grd_view_row.Cells[1].Text;
                                linkView.CommandName = grd_view_row.Cells[0].Text;
                                //  linkView.Text = "View";
                                linkView.CssClass = "fas fa-eye";
                                linkView.ToolTip = "Preview";
                                linkView.CommandArgument = grd_view_row.Cells[2].Text + "/" + grd_view_row.Cells[1].Text;

                                linkView.Click += LinkView_Click;

                                grd_view_row.Cells[6].Controls.Add(linkView);

                                if (ext == "jpg" || ext == "png") preview = true;
                            }
                        }

                        if (!preview)
                            e.Row.Cells[8].Text = "";

                        obj_grdview.HeaderRow.Cells[3].Text = "";
                        obj_grdview.HeaderRow.Cells[4].Text = "";
                        obj_grdview.HeaderRow.Cells[2].Text = "File Name";
                        obj_grdview.HeaderRow.Cells[2].Attributes.Add("width", "50%");

                        obj_grdview.HeaderRow.Cells[0].Visible = false;
                        obj_grdview.HeaderRow.Cells[2].Visible  = false;
                        obj_grdview.HeaderRow.Cells[1].Visible = false;
                        obj_grdview.HeaderRow.Cells[5].Visible = false;

                        obj_grdview.HeaderRow.Visible = false;

                        grd_row.Cells[9].Controls.Add(obj_grdview);
                    }

                   
                }
                else
                {
                    e.Row.Cells[8].Text  = "";
                }

                if (ViewState["isEdit"].ToString() == "false")
                {
                    e.Row.Cells[10].Visible = false;
                }
                if (ViewState["isAssignUser"].ToString() == "false")
                {
                    e.Row.Cells[11].Visible = false;
                }
                if (ViewState["isDelete"].ToString() == "false")
                {
                    e.Row.Cells[12].Visible = false;
                }
                //for db sync check
                if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes" && WebConfigurationManager.AppSettings["SyncStop"] == "No")
                {
                    if (!string.IsNullOrEmpty(e.Row.Cells[13].Text))
                    {
                        if (getdt.checkIssuesSynced(new Guid(e.Row.Cells[13].Text)) > 0)
                        {
                            e.Row.BackColor = System.Drawing.Color.LightYellow;
                        }
                        else
                        {
                            //  e.Row.BackColor = System.Drawing.Color.Green;
                            // e.Row.ForeColor = System.Drawing.Color.White;
                        }
                    }
                }

               
            }
        }

        private void LinkInfo_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LinkView_Click(object sender, EventArgs e)
        {
            if (Session["issue_status_preview"] != null)
            {
                Session["issue_status_preview"] = null;
                return;
            } 
         
            LinkButton new_link = (LinkButton)sender;

            string path = Server.MapPath(new_link.CommandArgument);

            string Extension = System.IO.Path.GetExtension(path);
            string outPath = path.Replace(Extension, "") + "_download" + Extension;
            getdt.DecryptFile(path, outPath);
            System.IO.FileInfo file = new System.IO.FileInfo(outPath);

            string fname = "/Documents/Issues/" + file.Name;

            if (file.Exists )
            {
                if (Extension == ".jpg" || Extension == ".png")
                {
                   ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showImgModal('" + fname + "');", true);
                }
                else if (Extension == ".pdf")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showPdfModal('" + fname + "');", true);
                }
            }
        }

        private void linkDelete_Click(object sender, EventArgs e)
        {
            LinkButton new_link = (LinkButton)sender;

            int file_count = getdt.DeleteUploadedIssueDoc(Convert.ToInt32(new_link.CommandName));

            if (file_count != 0)
            {
                string fileName = Server.MapPath(new_link.CommandArgument);

                if (fileName != null || fileName != string.Empty)
                {
                    if ((System.IO.File.Exists(fileName)))
                    {
                        System.IO.File.Delete(fileName);
                    }
                }
            }

            BindIssues("WorkPackage", TreeView1.SelectedNode.Value);
        }

        private void linkUpload_Click(object sender, EventArgs e)
        {
            LinkButton new_link = (LinkButton)sender;

            string path = Server.MapPath(new_link.CommandArgument);

            string getExtension = System.IO.Path.GetExtension(path);
            string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
            getdt.DecryptFile(path, outPath);
            System.IO.FileInfo file = new System.IO.FileInfo(outPath);

            if (file.Exists)
            {
                Response.Clear();

                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                Response.AddHeader("Content-Length", file.Length.ToString());

                Response.ContentType = "application/octet-stream";

                Response.TransmitFile(outPath);

                Response.End();

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");
            }
        }

        protected void GrdPrint_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ds.Clear();
                ds = getdt.getUserDetails(new Guid(e.Row.Cells[2].Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["UserName"].ToString();
                }
                ds.Clear();
                //ds = getdt.GetTasksby_TaskUiD(new Guid(e.Row.Cells[2].Text));
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    e.Row.Cells[2].Text = ds.Tables[0].Rows[0]["Name"].ToString();
                //}

            }
        }

        protected void GrdIssues_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string IssueUID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                DataSet ds = getdt.getIssuesList_by_UID(new Guid(IssueUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string path = Server.MapPath(ds.Tables[0].Rows[0]["Issue_Document"].ToString());

                    string getExtension = System.IO.Path.GetExtension(path);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdt.DecryptFile(path, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

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

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");

                    }
                }
            }

            if (e.CommandName == "delete")
            {

                int cnt = getdt.Issue_Delete(new Guid(IssueUID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindIssues(TreeView1.SelectedNode.Target, TreeView1.SelectedNode.Value);
                    IssueCountLoad(TreeView1.SelectedNode.Target, TreeView1.SelectedNode.Value);
                }
            }


        }

        protected void GrdIssues_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}