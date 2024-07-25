﻿<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_issues._default" EnableEventValidation = "false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
       <style type="text/css">
         .hideItem {
         display:none;
     }
  .pager span { color:green;font-weight:bold;font-size:17px;}
   

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Issues</div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
   <div class="container-fluid" id="IssueSummary" runat="server">
            <div class="row" style="text-align:center;">
                <div class="col-lg-12 col-xl-12" style="text-align:left;">
                    <br />
                    <h6 id="HeadingSummary" runat="server" class="text-muted text-uppercase font-weight-bold">
                                   Issue Summary(<asp:Label ID="LblTotalIssues" runat="server">0</asp:Label>)
                               </h6>
                    </div>
                
                <div class="col-md-6 col-xl-3 mb-2" style="display:none">
                      <div class="card">
                          <div class="card-body">
                               <b>Open Issues</b><br /><b style="color:#444444;"><asp:Label ID="LblOpenIssues" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
                <div class="col-md-6 col-xl-3 mb-2" style="display:none">
                      <div class="card">
                          <div class="card-body">
                               <b>In-Progress Issues</b><br /><b style="color:#444444;"><asp:Label ID="LblInProgressIssues" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
                <div class="col-md-6 col-xl-3 mb-4" style="display:none">
                      <div class="card">
                          <div class="card-body">
                               <b>Closed Issues</b><br /><b style="color:#444444;"><asp:Label ID="LblClosedIssues" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
                 <div class="col-md-6 col-xl-3 mb-4" style="display:none">
                      <div class="card">
                          <div class="card-body">
                               <b>Rejected Issues</b><br /><b style="color:#444444;"><asp:Label ID="LblRejectedIssues" runat="server">0</asp:Label></b>
                         </div>
                     </div>
                </div>
            </div>
        </div>
    <div class="container-fluid" id="IssuesGrid" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
            <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                            <h5 id="headingReport" style="font-weight:bold;" runat="server">Contract Data</h5>
                                            <h5 id="headingProject" runat="server">Contract Data</h5>
                                            </div>

                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <%--<asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Issues List" />--%>
                                                </h6>
                                                
                                                 <div style="float:left;width:25%;text-align:right">
                                                   <div style="float:left;width:35%;text-align:left;font:bold" class="form-control-plaintext"><b>Fliter by User&nbsp;:&nbsp;</b></div>
                                                   <div style="float:left;width:65%;text-align:right">
                                                   <asp:DropDownList ID="DDLUser" CssClass="form-control" Width="200px" runat="server" Font-Bold="true" AutoPostBack="True" OnSelectedIndexChanged="DDLUser_SelectedIndexChanged">
                                                      
                                                        

                                                   </asp:DropDownList></div>
                                               </div> 
                                               <div style="float:left;width:25%;text-align:right">
                                                   <div style="float:left;width:35%;text-align:left;font:bold" class="form-control-plaintext"><b>Fliter by Status&nbsp;:&nbsp;</b></div>
                                                   <div style="float:left;width:65%;text-align:right">
                                                   <asp:DropDownList ID="ddlstatus" CssClass="form-control" Width="200px" runat="server" Font-Bold="true" AutoPostBack="True" OnSelectedIndexChanged="ddlstatus_SelectedIndexChanged">
                                                      
                                                         <%--<asp:ListItem Value="All">All</asp:ListItem>
                                                        <asp:ListItem Value="Open">Open</asp:ListItem>
                         <asp:ListItem Value="In-Progress">In-Progress</asp:ListItem>
                         <asp:ListItem Value="Close">Close</asp:ListItem>
                         <asp:ListItem Value="Rejected">Rejected</asp:ListItem>--%>

                                                   </asp:DropDownList></div>
                                               </div>
                                                <div style="float:none;width:50%;text-align:right">
                                                     
                                                    <asp:Button ID="btnexcelexport" runat="server" Text="Export to Excel" Visible="false" CssClass="btn btn-primary" OnClick="btnexcelexport_Click" />
                                            <asp:Button ID="btnExportReportPDF" runat="server" Text="Export PDF" Visible="false" CssClass="btn btn-primary" OnClick="btnExportReportPDF_Click" />
                                                    <asp:Button ID="btnPrintPDF" runat="server" Text="Print" Visible="false" CssClass="btn btn-primary" OnClick="btnPrintPDF_Click" />
                                            </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                                <asp:GridView ID="GrdIssues" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data Found" AlternatingRowStyle-BackColor="lightGray" Width="100%" CssClass="table table-bordered" AllowPaging="True" OnPageIndexChanging="GrdIssues_PageIndexChanging" PageSize="10" OnRowDataBound="GrdIssues_RowDataBound">
<AlternatingRowStyle BackColor="LightGray"></AlternatingRowStyle>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>

                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Issue_Description" HeaderText="Issue Description">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                         <asp:BoundField DataField="Issued_User" HeaderText="Reporting User" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Issue_Date" HeaderText="Date of Issue Occurrence" DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Actual_Closer_Date" HeaderText="Approving Date"  DataFormatString="{0:dd/MM/yyyy}">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                               
                                                <asp:BoundField DataField="Issue_Status" HeaderText="Status" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="Issue_Remarks" HeaderText="Remarks" >
                                                <HeaderStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                </Columns>
                                                    <PagerSettings Position="TopAndBottom" />
                                                     <PagerStyle CssClass="pager" />
                                                </asp:GridView>

                                            <%-- <div id="printreport" runat="server" style="text-align:right;">
                                         <input type="button" value="Print Report" class="btn btn-primary" onclick="PrintDiv('myDiv')" />
                                        </div>--%>
                                            </div>
                                        </div>
                                    </div>
                </div>
        </div>
        </div>
</asp:Content>
