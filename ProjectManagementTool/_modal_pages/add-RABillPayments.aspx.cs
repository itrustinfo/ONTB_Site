﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManager.DAL;
using ProjectManagementTool.DAL;
using System.Data;


namespace ProjectManagementTool._modal_pages
{
    public partial class add_RABillPayments : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
        Invoice Invoice = new Invoice();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            if (!IsPostBack)
            {
                //LoadDeductionsMaster();
                               
               if (Request.QueryString["type"]!= null) //edit 
                {
                    LoadInvoice();
                    DataSet ds = getdata.GetRABillsPayments(new Guid(Request.QueryString["PaymentUID"].ToString()));
                    if(ds.Tables[0].Rows.Count > 0)
                    {
                        DDLInvoice.Enabled = false;
                        DDLInvoice.SelectedValue = ds.Tables[0].Rows[0]["InvoiceUID"].ToString();
                        txtRABill.Text = ds.Tables[0].Rows[0]["RABillDesc"].ToString();
                        txtAmnt.Text = ds.Tables[0].Rows[0]["Amount"].ToString();
                        txtNetAmnt.Text = ds.Tables[0].Rows[0]["NetAmount"].ToString();
                        dtPaymentDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["PaymentDate"].ToString()).ToString("dd/MM/yyyy");
                        //
                        LoadInvoiceDeductions();
                        DataSet dsDeduc = new DataSet();
                        dsDeduc = getdata.GetRABillsPaymentsDeductions(new Guid(ds.Tables[0].Rows[0]["PaymentUID"].ToString()));
                        foreach(DataRow dr in dsDeduc.Tables[0].Rows)
                        {
                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction1.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if(dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer1.Text = dr["Percentage"].ToString();
                                    txtDeduction1.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }
                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction2.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer2.Text = dr["Percentage"].ToString();
                                    txtDeduction2.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }
                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction3.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer3.Text = dr["Percentage"].ToString();
                                    txtDeduction3.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }
                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction4.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer4.Text = dr["Percentage"].ToString();
                                    txtDeduction4.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }
                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction5.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer5.Text = dr["Percentage"].ToString();
                                    txtDeduction5.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }
                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction6.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer6.Text = dr["Percentage"].ToString();
                                    txtDeduction6.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }
                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction7.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer7.Text = dr["Percentage"].ToString();
                                    txtDeduction7.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }
                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction8.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer8.Text = dr["Percentage"].ToString();
                                    txtDeduction8.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }
                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction9.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer9.Text = dr["Percentage"].ToString();
                                    txtDeduction9.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }
                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction10.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer10.Text = dr["Percentage"].ToString();
                                    txtDeduction10.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }

                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction11.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer11.Text = dr["Percentage"].ToString();
                                    txtDeduction11.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }

                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction12.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer12.Text = dr["Percentage"].ToString();
                                    txtDeduction12.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }

                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction13.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer13.Text = dr["Percentage"].ToString();
                                    txtDeduction13.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }

                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction14.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer14.Text = dr["Percentage"].ToString();
                                    txtDeduction14.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }

                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction15.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer15.Text = dr["Percentage"].ToString();
                                    txtDeduction15.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }

                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction16.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer16.Text = dr["Percentage"].ToString();
                                    txtDeduction16.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }

                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction17.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer17.Text = dr["Percentage"].ToString();
                                    txtDeduction17.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }

                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction18.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer18.Text = dr["Percentage"].ToString();
                                    txtDeduction18.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }

                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction19.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer19.Text = dr["Percentage"].ToString();
                                    txtDeduction19.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }

                            ds.Clear();
                            ds = getdata.GetDeductionFromDesc(lblDeduction20.InnerText);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (dr["DeductionUID"].ToString() == ds.Tables[0].Rows[0]["UID"].ToString())
                                {
                                    txtDeducPer20.Text = dr["Percentage"].ToString();
                                    txtDeduction20.Text = dr["Amount"].ToString();
                                    continue;
                                }
                            }
                        }
                    }
                }
                else
                {
                    LoadInvoice();
                    LoadInvoiceDeductions();
                }
            }
        }

        private void LoadDeductionsMaster()
        {
            DataSet ds = new DataSet();
            ds = getdata.GetDeductionMaster();
            int count = 0;
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                count++;
                if(count == 1)
                {
                    divD1.Visible = true;
                    lblDeduction1.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 2)
                {
                    divD2.Visible = true;
                    lblDeduction2.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 3)
                {
                    divD3.Visible = true;
                    lblDeduction3.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 4)
                {
                    divD4.Visible = true;
                    lblDeduction4.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 5)
                {
                    divD5.Visible = true;
                    lblDeduction5.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 6)
                {
                    divD6.Visible = true;
                    lblDeduction6.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 7)
                {
                    divD7.Visible = true;
                    lblDeduction7.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 8)
                {
                    divD8.Visible = true;
                    lblDeduction8.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 9)
                {
                    divD9.Visible = true;
                    lblDeduction9.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 10)
                {
                    divD10.Visible = true;
                    lblDeduction10.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 11)
                {
                    divD11.Visible = true;
                    lblDeduction11.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 12)
                {
                    divD12.Visible = true;
                    lblDeduction12.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 13)
                {
                    divD13.Visible = true;
                    lblDeduction13.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 14)
                {
                    divD14.Visible = true;
                    lblDeduction14.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 15)
                {
                    divD15.Visible = true;
                    lblDeduction15.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 16)
                {
                    divD16.Visible = true;
                    lblDeduction16.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 17)
                {
                    divD17.Visible = true;
                    lblDeduction17.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 18)
                {
                    divD18.Visible = true;
                    lblDeduction18.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 19)
                {
                    divD19.Visible = true;
                    lblDeduction19.InnerText = dr["DeductionsDescription"].ToString();
                }
                else if (count == 20)
                {
                    divD20.Visible = true;
                    lblDeduction20.InnerText = dr["DeductionsDescription"].ToString();
                }
            }
        }

        private void LoadInvoiceDeductions()
        {
            DataSet ds = new DataSet();
            if (DDLInvoice.SelectedIndex > 0)
            {
                ClearAlltextBoxes();
                ds = getdata.GetInvoiceDeductions(new Guid(DDLInvoice.SelectedValue));

                int count = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    count++;
                    if (count == 1)
                    {
                        divD1.Visible = true;
                        lblDeduction1.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer1.Text = dr["Percentage"].ToString();
                        txtDeduction1.Text = dr["Amount"].ToString();
                        txtDeducPer1.Enabled = false;
                        txtDeduction1.Enabled = true;
                    }
                    else if (count == 2)
                    {
                        divD2.Visible = true;
                        lblDeduction2.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer2.Text = dr["Percentage"].ToString();
                        txtDeduction2.Text = dr["Amount"].ToString();
                        txtDeducPer2.Enabled = false;
                        txtDeduction2.Enabled = false;
                    }
                    else if (count == 3)
                    {
                        divD3.Visible = true;
                        lblDeduction3.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer3.Text = dr["Percentage"].ToString();
                        txtDeduction3.Text = dr["Amount"].ToString();
                        txtDeducPer3.Enabled = false;
                        txtDeduction3.Enabled = false;
                    }
                    else if (count == 4)
                    {
                        divD4.Visible = true;
                        lblDeduction4.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer4.Text = dr["Percentage"].ToString();
                        txtDeduction4.Text = dr["Amount"].ToString();
                        txtDeducPer4.Enabled = false;
                        txtDeduction4.Enabled = false;
                    }
                    else if (count == 5)
                    {
                        divD5.Visible = true;
                        lblDeduction5.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer5.Text = dr["Percentage"].ToString();
                        txtDeduction5.Text = dr["Amount"].ToString();
                        txtDeducPer5.Enabled = false;
                        txtDeduction5.Enabled = false;
                    }
                    else if (count == 6)
                    {
                        divD6.Visible = true;
                        lblDeduction6.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer6.Text = dr["Percentage"].ToString();
                        txtDeduction6.Text = dr["Amount"].ToString();
                        txtDeducPer6.Enabled = false;
                        txtDeduction6.Enabled = false;
                    }
                    else if (count == 7)
                    {
                        divD7.Visible = true;
                        lblDeduction7.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer7.Text = dr["Percentage"].ToString();
                        txtDeduction7.Text = dr["Amount"].ToString();
                        txtDeducPer7.Enabled = false;
                        txtDeduction7.Enabled = false;
                    }
                    else if (count == 8)
                    {
                        divD8.Visible = true;
                        lblDeduction8.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer8.Text = dr["Percentage"].ToString();
                        txtDeduction8.Text = dr["Amount"].ToString();
                        txtDeducPer8.Enabled = false;
                        txtDeduction8.Enabled = false;
                    }
                    else if (count == 9)
                    {
                        divD9.Visible = true;
                        lblDeduction9.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer9.Text = dr["Percentage"].ToString();
                        txtDeduction9.Text = dr["Amount"].ToString();
                        txtDeducPer9.Enabled = false;
                        txtDeduction9.Enabled = false;
                    }
                    else if (count == 10)
                    {
                        divD10.Visible = true;
                        lblDeduction10.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer10.Text = dr["Percentage"].ToString();
                        txtDeduction10.Text = dr["Amount"].ToString();
                        txtDeducPer10.Enabled = false;
                        txtDeduction10.Enabled = false;
                    }
                    else if (count == 11)
                    {
                        divD11.Visible = true;
                        lblDeduction11.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer11.Text = dr["Percentage"].ToString();
                        txtDeduction11.Text = dr["Amount"].ToString();
                        txtDeducPer11.Enabled = false;
                        txtDeduction11.Enabled = false;
                    }
                    else if (count == 12)
                    {
                        divD12.Visible = true;
                        lblDeduction12.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer12.Text = dr["Percentage"].ToString();
                        txtDeduction12.Text = dr["Amount"].ToString();
                        txtDeducPer12.Enabled = false;
                        txtDeduction12.Enabled = false;
                    }
                    else if (count == 13)
                    {
                        divD13.Visible = true;
                        lblDeduction13.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer13.Text = dr["Percentage"].ToString();
                        txtDeduction13.Text = dr["Amount"].ToString();
                        txtDeducPer13.Enabled = false;
                        txtDeduction13.Enabled = false;
                    }
                    else if (count == 14)
                    {
                        divD14.Visible = true;
                        lblDeduction14.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer14.Text = dr["Percentage"].ToString();
                        txtDeduction14.Text = dr["Amount"].ToString();
                        txtDeducPer14.Enabled = false;
                        txtDeduction14.Enabled = false;
                    }
                    else if (count == 15)
                    {
                        divD15.Visible = true;
                        lblDeduction15.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer15.Text = dr["Percentage"].ToString();
                        txtDeduction15.Text = dr["Amount"].ToString();
                        txtDeducPer15.Enabled = false;
                        txtDeduction15.Enabled = false;
                    }
                    else if (count == 16)
                    {
                        divD16.Visible = true;
                        lblDeduction16.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer16.Text = dr["Percentage"].ToString();
                        txtDeduction16.Text = dr["Amount"].ToString();
                        txtDeducPer16.Enabled = false;
                        txtDeduction16.Enabled = false;
                    }
                    else if (count == 17)
                    {
                        divD17.Visible = true;
                        lblDeduction17.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer17.Text = dr["Percentage"].ToString();
                        txtDeduction17.Text = dr["Amount"].ToString();
                        txtDeducPer17.Enabled = false;
                        txtDeduction17.Enabled = false;
                    }
                    else if (count == 18)
                    {
                        divD18.Visible = true;
                        lblDeduction18.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer18.Text = dr["Percentage"].ToString();
                        txtDeduction18.Text = dr["Amount"].ToString();
                        txtDeducPer18.Enabled = false;
                        txtDeduction18.Enabled = false;
                    }
                    else if (count == 19)
                    {
                        divD19.Visible = true;
                        lblDeduction19.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer19.Text = dr["Percentage"].ToString();
                        txtDeduction19.Text = dr["Amount"].ToString();
                        txtDeducPer19.Enabled = false;
                        txtDeduction19.Enabled = false;
                    }
                    else if (count == 20)
                    {
                        divD20.Visible = true;
                        lblDeduction20.InnerText = dr["DeductionsDescription"].ToString();
                        txtDeducPer20.Text = dr["Percentage"].ToString();
                        txtDeduction20.Text = dr["Amount"].ToString();
                        txtDeducPer20.Enabled = false;
                        txtDeduction20.Enabled = false;
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if(DDLInvoice.SelectedIndex == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please select invoice.');</script>");
                    return;
                }

                if (Session["ProjectUID"] is null || Session["WorkPackageUID"] is null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Project ID/WorkPackageID can not be null.');</script>");
                    return;
                }

                Guid PaymentUID = Guid.NewGuid();
                if (Request.QueryString["type"] != null) //edit 
                {
                    PaymentUID = new Guid(Request.QueryString["PaymentUID"].ToString());
                }
                    Guid FinMonthUID = new Guid(Request.QueryString["monthUID"].ToString());
                Guid InvoiceUID = new Guid(DDLInvoice.SelectedValue);
                float Amount =float.Parse(txtAmnt.Text);
                float NetAmnt = float.Parse(txtNetAmnt.Text);
                float TotalDeductions = 0.0f;
                string RABillDesc = txtRABill.Text;
                Guid DeductionUID = new Guid();
                float DeducAmnt = 0.0f;
                float Deducper = 0.0f;
                int result = 0;
                DataSet ds = new DataSet();
                if(txtDeduction1.Text !="" && txtDeducPer1.Text !="")
                {
                    DeducAmnt = float.Parse(txtDeduction1.Text);
                    Deducper = float.Parse(txtDeducPer1.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction1.InnerText);
                    if(ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                    result = getdata.InsertMobilisationAdvance(Guid.NewGuid(), new Guid(Session["ProjectUID"].ToString()), new Guid(Session["WorkPackageUID"].ToString()), DDLInvoice.SelectedItem.Text, Convert.ToDateTime(getdata.ConvertDateFormat(dtPaymentDate.Text)), Convert.ToDecimal(txtDeduction1.Text), "Debit", false);
                }
                if (txtDeduction2.Text != "" && txtDeducPer2.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction2.Text);
                    Deducper = float.Parse(txtDeducPer2.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction2.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }
                if (txtDeduction3.Text != "" && txtDeducPer3.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction3.Text);
                    Deducper = float.Parse(txtDeducPer3.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction3.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }
                if (txtDeduction4.Text != "" && txtDeducPer4.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction4.Text);
                    Deducper = float.Parse(txtDeducPer4.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction4.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }
                if (txtDeduction5.Text != "" && txtDeducPer5.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction5.Text);
                    Deducper = float.Parse(txtDeducPer5.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction5.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }
                if (txtDeduction6.Text != "" && txtDeducPer6.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction6.Text);
                    Deducper = float.Parse(txtDeducPer6.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction6.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }
                if (txtDeduction7.Text != "" && txtDeducPer7.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction7.Text);
                    Deducper = float.Parse(txtDeducPer7.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction7.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }
                if (txtDeduction8.Text != "" && txtDeducPer8.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction8.Text);
                    Deducper = float.Parse(txtDeducPer8.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction8.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }
                if (txtDeduction9.Text != "" && txtDeducPer9.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction9.Text);
                    Deducper = float.Parse(txtDeducPer9.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction9.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }
                if (txtDeduction10.Text != "" && txtDeducPer10.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction10.Text);
                    Deducper = float.Parse(txtDeducPer10.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction10.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }

                if (txtDeduction11.Text != "" && txtDeducPer11.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction11.Text);
                    Deducper = float.Parse(txtDeducPer11.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction11.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }

                if (txtDeduction12.Text != "" && txtDeducPer12.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction12.Text);
                    Deducper = float.Parse(txtDeducPer12.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction12.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }

                if (txtDeduction13.Text != "" && txtDeducPer13.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction13.Text);
                    Deducper = float.Parse(txtDeducPer13.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction13.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }

                if (txtDeduction14.Text != "" && txtDeducPer14.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction14.Text);
                    Deducper = float.Parse(txtDeducPer14.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction14.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }

                if (txtDeduction15.Text != "" && txtDeducPer15.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction15.Text);
                    Deducper = float.Parse(txtDeducPer15.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction15.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }

                if (txtDeduction16.Text != "" && txtDeducPer16.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction16.Text);
                    Deducper = float.Parse(txtDeducPer16.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction16.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }

                if (txtDeduction17.Text != "" && txtDeducPer17.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction17.Text);
                    Deducper = float.Parse(txtDeducPer17.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction17.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }

                if (txtDeduction18.Text != "" && txtDeducPer18.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction18.Text);
                    Deducper = float.Parse(txtDeducPer18.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction18.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }

                if (txtDeduction19.Text != "" && txtDeducPer19.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction19.Text);
                    Deducper = float.Parse(txtDeducPer11.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction19.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }

                if (txtDeduction20.Text != "" && txtDeducPer20.Text != "")
                {
                    DeducAmnt = float.Parse(txtDeduction20.Text);
                    Deducper = float.Parse(txtDeducPer20.Text);
                    ds.Clear();
                    ds = getdata.GetDeductionFromDesc(lblDeduction20.InnerText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DeductionUID = new Guid(ds.Tables[0].Rows[0]["UID"].ToString());
                    }
                    TotalDeductions = TotalDeductions + DeducAmnt;
                    result = getdata.InsertRABillsDeductions(Guid.NewGuid(), PaymentUID, DeductionUID, DeducAmnt, Deducper);
                }

                string sDate2 = dtPaymentDate.Text;
                //sDate2 = sDate2.Split('/')[1] + "/" + sDate2.Split('/')[0] + "/" + sDate2.Split('/')[2];
                sDate2 = getdata.ConvertDateFormat(sDate2);
                DateTime CDate2 = Convert.ToDateTime(sDate2);
                //added on 29/09/2022
                
                 DataSet dsInvAdd = Invoice.GetInvoiceAdditions_by_InvoiceMaster_UID(InvoiceUID);
                float AdditionsAmnt = 0.0f;
                float TotalAdditions = 0.0f;

                foreach (DataRow dr in dsInvAdd.Tables[0].Rows)
                {
                    AdditionsAmnt = float.Parse(dr["Amount"].ToString());
                  
                    TotalAdditions = TotalAdditions + AdditionsAmnt;

                }
                result = getdata.InsertRABillPayments(PaymentUID, InvoiceUID, RABillDesc, Amount, TotalDeductions, NetAmnt, FinMonthUID,CDate2, TotalAdditions);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>"); 
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //protected void txtDeducPer1_TextChanged(object sender, EventArgs e)
        //{
        //    if(txtDeducPer1.Text !="0" && txtAmnt.Text !="")
        //    {
        //        txtDeduction1.Text = ((float.Parse(txtAmnt.Text) * float.Parse(txtDeducPer1.Text) / 100)).ToString("N2");
        //        txtNetAmnt.Text = (float.Parse(txtAmnt.Text) - float.Parse(txtDeduction1.Text)).ToString("N2");
        //    }
        //}

        private void LoadInvoice()
        {
            try
            {
                if (Request.QueryString["type"] != null)
                {
                    DDLInvoice.DataSource = getdata.GetInvoiceMasterByWkpg(new Guid(Request.QueryString["WkpgUID"].ToString()), 2);
                }
                else
                {
                    DDLInvoice.DataSource = getdata.GetInvoiceMasterByWkpg(new Guid(Request.QueryString["WkpgUID"].ToString()), 1);
                }
                DDLInvoice.DataTextField = "Invoice_Number";
                DDLInvoice.DataValueField = "InvoiceMaster_UID";
                DDLInvoice.DataBind();
                DDLInvoice.Items.Insert(0, "Select Invoice");
                if (DDLInvoice.SelectedIndex > 0)
                {
                    txtAmnt.Text = (getdata.GetSumBillValueForInvoice(new Guid(DDLInvoice.SelectedValue))).ToString();
                    txtNetAmnt.Text = (getdata.GetNetBillValueForInvoice(new Guid(DDLInvoice.SelectedValue))).ToString();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        protected void DDLInvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            // LoadInvoice();
            if (DDLInvoice.SelectedIndex > 0)
            {
                txtAmnt.Text = (getdata.GetSumBillValueForInvoice(new Guid(DDLInvoice.SelectedValue))).ToString();
                txtNetAmnt.Text = (getdata.GetNetBillValueForInvoice(new Guid(DDLInvoice.SelectedValue))).ToString();
            }
            LoadInvoiceDeductions();
        }

        private void ClearAlltextBoxes()
        {
            txtDeduction1.Text = "";
            txtDeducPer1.Text = "";
            divD1.Visible = false;
            //
            txtDeduction2.Text = "";
            txtDeducPer2.Text = "";
            divD2.Visible = false;
            //
            txtDeduction3.Text = "";
            txtDeducPer3.Text = "";
            divD3.Visible = false;
            //
            txtDeduction4.Text = "";
            txtDeducPer4.Text = "";
            divD4.Visible = false;
            //
            txtDeduction5.Text = "";
            txtDeducPer5.Text = "";
            divD5.Visible = false;
            //
            //
            txtDeduction6.Text = "";
            txtDeducPer6.Text = "";
            divD6.Visible = false;
            //
            txtDeduction7.Text = "";
            txtDeducPer7.Text = "";
            divD7.Visible = false;
            //
            txtDeduction8.Text = "";
            txtDeducPer8.Text = "";
            divD8.Visible = false;
            //
            txtDeduction9.Text = "";
            txtDeducPer9.Text = "";
            divD9.Visible = false;
            //
            //
            txtDeduction10.Text = "";
            txtDeducPer10.Text = "";
            divD10.Visible = false;
        }
    }
}