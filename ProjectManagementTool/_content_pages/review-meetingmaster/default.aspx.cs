﻿using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.review_meetingmaster
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
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
                    BindReviewMeeting();
                }
            }
        }

        private void BindReviewMeeting()
        {
            DataSet ds = getdt.GetMeetingMasters();
            GrdReviewMeeting.DataSource = ds;
            GrdReviewMeeting.DataBind();
        }

        protected void GrdReviewMeeting_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdReviewMeeting.PageIndex = e.NewPageIndex;
            BindReviewMeeting();
        }

        protected void GrdReviewMeeting_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();
            if (e.CommandName == "delete")
            {
                int cnt = getdt.ReviewMeeting_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindReviewMeeting();
                }
            }
        }

        protected void GrdReviewMeeting_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}