using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class addtask_targetschedule_revised : System.Web.UI.Page
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
                    BindTaskSchedule();
                }
            }
        }

       
        private void BindYear(DropDownList DDLYear)
        {
            DDLYear.Items.Clear();
            int year = DateTime.Now.Year - 5;
            DDLYear.Items.Add(new ListItem("--Select Year--", ""));
            for (int i = 1; i < 10; i++)
            {
                year = year + 1;
                DDLYear.Items.Add(new ListItem(year.ToString(), year.ToString()));
            }
        }
       

        protected void BindTaskSchedule()
        {
            DataSet ds1 = getdata.GetLatest_TaskScheduleVesrion_By_TaskUID(new Guid(Request.QueryString["TaskUID"]));

            if (ds1.Tables[0].Rows.Count >0)
            {
                float version = float.Parse(ds1.Tables[0].Rows[0]["TaskScheduleVersion"].ToString());

                Session["version"] = version;
                HiddenAction.Value = "Update";
              
                GridView1.DataSource = null;
                GridView1.DataBind();
                DataSet ds = getdata.GetTaskSchedule_By_TaskUID_TaskScheduleVersion1(new Guid(Request.QueryString["TaskUID"]), version);
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
            else
            {
                HiddenAction.Value = "Add";
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Find the DropDownList in the Row.
                DropDownList ddlMonths = (e.Row.FindControl("ddlMonths") as DropDownList);
                ddlMonths.CssClass = "form-control";

                //Add Default Item in the DropDownList.
                ddlMonths.Items.Insert(0, new ListItem("Please select", "00"));
                ddlMonths.Items.Insert(1, new ListItem("Jan", "01"));
                ddlMonths.Items.Insert(2, new ListItem("Feb", "02"));
                ddlMonths.Items.Insert(3, new ListItem("Mar", "03"));
                ddlMonths.Items.Insert(4, new ListItem("Apr", "04"));
                ddlMonths.Items.Insert(5, new ListItem("May", "05"));
                ddlMonths.Items.Insert(6, new ListItem("Jun", "06"));
                ddlMonths.Items.Insert(7, new ListItem("Jul", "07"));
                ddlMonths.Items.Insert(8, new ListItem("Aug", "08"));
                ddlMonths.Items.Insert(9, new ListItem("Sep", "09"));
                ddlMonths.Items.Insert(10, new ListItem("Oct", "10"));
                ddlMonths.Items.Insert(11, new ListItem("Nov", "11"));
                ddlMonths.Items.Insert(12, new ListItem("Dec", "12"));

                //Select the Country of Customer in DropDownList.
                string month = (e.Row.FindControl("lblMonth") as Label).Text;
                ddlMonths.Items.FindByText(month).Selected = true;


                DropDownList ddlYears = (e.Row.FindControl("ddlYears") as DropDownList);
                ddlYears.CssClass = "form-control";

                //Add Default Item in the DropDownList.
                ddlYears.Items.Insert(0, new ListItem("Please select", "0000"));
                ddlYears.Items.Insert(1, new ListItem("2020", "2020"));
                ddlYears.Items.Insert(2, new ListItem("2021", "2021"));
                ddlYears.Items.Insert(3, new ListItem("2022", "2022"));
                ddlYears.Items.Insert(4, new ListItem("2023", "2023"));


                //Select the Country of Customer in DropDownList.
                string year = (e.Row.FindControl("lblYear") as Label).Text;
                ddlYears.Items.FindByText(year).Selected = true;

                TextBox txtPlanned = (e.Row.FindControl("txtScheduleValue") as TextBox);
                txtPlanned.CssClass = "form-control";


                string planned = (e.Row.FindControl("lblScheduleValue") as Label).Text;
                txtPlanned.Text = planned;
            }
        }

        protected void GrdView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('It can not be deleted as there exists data.');</script>");

        }

        protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();

            if (e.CommandName == "delete")
            {
                int cnt = getdata.TaskSchedule_Delete_by_TaskScheduleUID(new Guid(UID));

                if (cnt > 0)
                {
                   BindTaskSchedule();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Warning", "alert('This cannot be deleted. contact system administrator');", true);

                }
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            if (Session["version"] == null)
            {
                Session["version"] = 0;
            }

            float version = float.Parse(Session["version"].ToString());

            if (version == 0)
            {
                bool result = getdata.InsertorUpdateTaskScheduleVesrion(Guid.NewGuid(), new Guid(Request.QueryString["TaskUID"]), "Month");
                if (result)
                {
                    version = 1;
                    Session["version"] = 1;
                }

            }

            DataSet ds = getdata.GetTaskSchedule_By_TaskUID_TaskScheduleVersion1(new Guid(Request.QueryString["TaskUID"]), version);

            ds.Tables[0].Rows.Add(Guid.NewGuid(), "Jan", "2020", 0);

            GridView1.DataSource = ds;
            GridView1.DataBind();

            HiddenAction.Value = "Save";
            btnAddNew.Enabled = false;
            btnCancel.Enabled = true;
            btnSaveNew.Enabled = true;
            btnUpdate.Enabled = false;
        }



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            BindTaskSchedule();

            btnAddNew.Enabled = true;
            btnCancel.Enabled = false;
            btnSaveNew.Enabled = false;
            btnUpdate.Enabled = true;
        }

        protected void btnSaveNew_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow NewRow = GridView1.Rows[GridView1.Rows.Count - 1];

                string ID = NewRow.Cells[0].Text;

                DropDownList ddlMonths = (NewRow.FindControl("ddlMonths") as DropDownList);

                int month = Convert.ToInt32(ddlMonths.SelectedValue);

                DropDownList ddlYears = (NewRow.FindControl("ddlYears") as DropDownList);

                int year = Convert.ToInt32(ddlYears.SelectedValue);

                TextBox txtSchedule = (NewRow.FindControl("txtScheduleValue") as TextBox);

                float ScheduledValue = Convert.ToSingle(txtSchedule.Text);

                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = startDate.AddDays(DateTime.DaysInMonth(startDate.Year, startDate.Month));

                getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), startDate, endDate, ScheduledValue, "Month");

                BindTaskSchedule();

                btnAddNew.Enabled = true;
                btnCancel.Enabled = false;
                btnSaveNew.Enabled = false;
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ATS-01 there is a problem with this feature. Please contact system admin. Description : " + ex.Message + "');</script>");
            }

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // string taskuid = Request.QueryString["TaskUID"];

                string confirmValue = "";

                if (HiddenAction.Value == "Update")
                {
                    confirmValue = Request.Form["confirm_value"];

                    Boolean ver = false;

                    if (confirmValue == "Yes")
                    {
                        ver = getdata.InsertorUpdateTaskScheduleVesrion(Guid.NewGuid(), new Guid(Request.QueryString["TaskUID"]), "Month");
                    }
                }

                DataSet ds1 = getdata.GetLatest_TaskScheduleVesrion_By_TaskUID(new Guid(Request.QueryString["TaskUID"]));

                if (ds1.Tables[0].Rows.Count >0)
                {
                    float version = float.Parse(ds1.Tables[0].Rows[0]["TaskScheduleVersion"].ToString());
                }

                foreach (GridViewRow grdrow in GridView1.Rows)
                {
                    string ID = grdrow.Cells[0].Text;

                    if (string.IsNullOrEmpty(ID))
                        ID = Guid.NewGuid().ToString();

                    DropDownList ddlMonths = (grdrow.FindControl("ddlMonths") as DropDownList);
                    int month = Convert.ToInt32(ddlMonths.SelectedValue);

                    DropDownList ddlYears = (grdrow.FindControl("ddlYears") as DropDownList);
                    int year = Convert.ToInt32(ddlYears.SelectedValue);

                    TextBox txtSchedule = (grdrow.FindControl("txtScheduleValue") as TextBox);
                    float ScheduledValue = Convert.ToSingle(txtSchedule.Text);

                    DateTime startDate = new DateTime(year, month, 1);
                    DateTime endDate = startDate.AddDays(DateTime.DaysInMonth(startDate.Year, startDate.Month));

                    if (confirmValue == "Yes")
                    {
                        getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), startDate, endDate, ScheduledValue, "Month");
                    }
                    else
                    {
                        getdata.InsertorUpdateTaskSchedule(new Guid(ID), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), startDate, endDate, ScheduledValue, "Month");
                    }
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ATS-01 there is a problem with this feature. Please contact system admin. Description : " + ex.Message + "');</script>");
            }
        }

        protected void TxtScheduleValue_Changed(object sender, EventArgs e)
        {
           // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "EnableSaveButton();", true);

        }

    }
}