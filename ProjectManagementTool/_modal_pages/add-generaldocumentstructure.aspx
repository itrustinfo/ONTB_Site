﻿<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-generaldocumentstructure.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_generaldocumentstructure" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">

    <script type="text/javascript">
        function ShowProgressBar(status) {
           
            if (status == "true") {
                if (document.getElementById("txtstructurename").value != "") {
                    document.getElementById('dvProgressBar').style.visibility = 'visible';
                }
            }
            else {
                document.getElementById('dvProgressBar').style.visibility = 'hidden';
            }
        }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddGeneralDocumentStructure" runat="server">
        <div class="container-fluid" style="min-height:120px;">
            <div class="row">
                <div class="col-sm-12">
                     <div class="form-group">
                        <label class="lblCss" for="txtstructurename">SubFolder Name</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                         <asp:TextBox ID="txtstructurename" CssClass="form-control" runat="server" autocomplete="off" required ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
            </div> 
        </div>
        <div id="dvProgressBar" style=" text-align:center; position:relative; visibility:hidden;" >
                     <img src="../_assets/images/progress.gif" width="40" alt="loading"  /> <span style="color:#006699; font-weight:bold;">Uploading is in progress...</span>
                     </div> 
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClientClick="ShowProgressBar('true')" OnClick="btnSubmit_Click"/>
                </div>
    </form>
</asp:Content>
