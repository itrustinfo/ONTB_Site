﻿<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-issues.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_issues" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
 $( function() {
    $("input[id$='dtAssignedDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtReportingDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
     });

     $("input[id$='dtApprovingDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
     });

     $("input[id$='dtProposedCloserDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
     });

 });

        //   $(".modal-header .close").click(function(e) {
        //               alert("hide");
        //window.location.reload();
        //         });

        //        function RefreshParent() {
        //    if (window.opener != null && !window.opener.closed) {
        //        window.opener.location.reload();
        //    }
        //}
        //window.onbeforeunload = RefreshParent;
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddIssuesModal" runat="server">
         <asp:HiddenField ID="ProjectHiddenValue" runat="server" />
            <asp:HiddenField ID="WorkPackageHiddenValue" runat="server" />
            <asp:HiddenField ID="TaskHiddenValue" runat="server" />
        <div class="container-fluid" style="max-height:90vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group" id="AcvitityName" runat="server">
                        <label class="lblCss" for="txtactivityname">Activity Name</label>
                         <asp:TextBox ID="txtactivityname" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtIssue_Description">Issue Description</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtIssue_Description" runat="server" TextMode="MultiLine" CssClass="form-control" required Height="120px"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <%--<label class="lblCss" for="ddlAssignedUser">Assigned User</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>--%>
                         <asp:DropDownList ID="ddlAssignedUser" CssClass="form-control"  runat="server" Visible="false"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <%--<label class="lblCss" for="txtdesc" Visible="false">Assigned Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>--%>
                        <asp:TextBox ID="dtAssignedDate" CssClass="form-control" required runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static" Visible="false"></asp:TextBox>
                        
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="ddlReportingUser">Reporting User</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:DropDownList ID="ddlReportingUser" CssClass="form-control"  runat="server"></asp:DropDownList>
                     </div>

                   <%-- <div class="form-group">
                        <label class="lblCss" for="ReportingUser">Reporting User</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="ReportingUser" CssClass="form-control" runat="server"  autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>--%>

                    <div class="form-group">
                        <label class="lblCss" for="dtReportingDate">Reporting Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtReportingDate" CssClass="form-control" required runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <%--<label class="lblCss" for="ddlApprovingUser">Approving User</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>--%>
                         <asp:DropDownList ID="ddlApprovingUser" runat="server" CssClass="form-control" Visible="false" >
                     </asp:DropDownList>
                    </div>

                    
                     <div class="form-group">
                        <%--<label class="lblCss" for="dtApprovingDate">Approving Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>--%>
                        <asp:TextBox ID="dtApprovingDate" CssClass="form-control" Visible="false" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required  ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <%--<label class="lblCss" for="dtProposedCloserDate">Issue Proposed Closure Date</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>--%>
                       <asp:TextBox ID="dtProposedCloserDate" CssClass="form-control" Visible="false" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group" id="IssueStatus" runat="server">
                        <label class="lblCss" for="ddlStatus">Status</label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                         <asp:ListItem>Open</asp:ListItem>
                         <asp:ListItem>In-Progress</asp:ListItem>
                         <asp:ListItem>Close</asp:ListItem>
                         <asp:ListItem>Rejected</asp:ListItem>
                     </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="txtRefNumber">Remarks</label>
                        <asp:TextBox ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" runat="server" ClientIDMode="Static" Height="120px"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="lblCss" for="FileUploadDoc">Issue Document</label>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadDoc" AllowMultiple="true" runat="server" CssClass="custom-file-input"/>
                            <label class="custom-file-label" for="FilDocument">Choose document</label>
                        </div>
                    </div>
                </div>
            </div> 
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
