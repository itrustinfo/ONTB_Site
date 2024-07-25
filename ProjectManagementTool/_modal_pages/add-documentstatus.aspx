﻿    <%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-documentstatus.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_documentstatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">

         function DialogConfirm()
         {
             //alert("Hi");
            // alert(document.getElementById("<%=hdFlowtype.ClientID%>").value);
             if (document.getElementById("<%=hdFlowtype.ClientID%>").value == "STP-C" || document.getElementById("<%=hdFlowtype.ClientID%>").value == "STP-OB") {
                 var filePath = document.getElementById('<%= this.FileUploadCoverLetter.ClientID %>').value;
                 if (filePath.length < 1) {
                     if (confirm("Cover Letter is not added.Do you want to continue.Click OK to continue and Cancel to go and add Cover Letter!") == true) {

                         document.getElementById('dvProgressBar').style.visibility = 'visible';
                         return true;
                     }
                     else {

                         return false;
                     }
                 }
                 else {
                     document.getElementById('dvProgressBar').style.visibility = 'visible';
                     return true;
                 }
             }
             else {
                  document.getElementById('dvProgressBar').style.visibility = 'visible';
                 return true;
             }
           
        }


         function ShowProgressBar(status) {
            if (status == "true") {
               
                    document.getElementById('dvProgressBar').style.visibility = 'visible';
                
            }
            else {
                document.getElementById('dvProgressBar').style.visibility = 'hidden';
            }
        }



        function getValue(obj)
        {
            var Desc = '';
            var text = obj.options[obj.selectedIndex].innerHTML;

            if (text.includes("Back to PMC")) {
                document.getElementById("modal_master_body_divUSers").style.display = "block";
            }
            else {
                 document.getElementById("modal_master_body_divUSers").style.display = "none";
            }

            if (text == "Code A") {
                Desc = "* Approved, manufacture/construction may commence";
            }
            else if (text == "Code B")
            {
                Desc = "* Acceptable subject to changes indicated.Resubmit for approval but manufacture / construction may commence.";
            }
            else if (text == "Code H")
            {
                Desc = "* Returned without review.";
            }
            else if (text == "Code C")
            {
                Desc = "* Amend as comments indicated and resubmit for approval.";
            }
            else if (text == "Code E")
            {
                Desc = "* Amend as comments indicated resubmit for record.";
            }
            else if (text == "Code G")
            {
                Desc = "* Drawing of this category is for information and hence not required to be approved.";
            }
            else if (text == "Code D")
            {
                Desc = "* Comments noted in Letter/memo attached to forwarding transmittal No.";
            }
            else if (text == "Code F")
            {
                Desc = "* Comments noted in Letter/memo attached to forwarding transmittal No....... dated......... Amend as comments indicated and resubmit for record.";
            }
            document.getElementById("tooltip").innerHTML = Desc;
            //alert(text);
            if (text == "Closed") {
                document.getElementById("divUpdateStatus").style.display = "none";
                document.getElementById("divReviewFile").style.display = "none";
            }
            else {
                  document.getElementById("divUpdateStatus").style.display = "block";
                document.getElementById("divReviewFile").style.display = "block";
            }
        }

        function setValue() {
          
            var value = document.getElementById("<%=DDlStatus.ClientID%>");  
   
   var text = value.options[value.selectedIndex].text;  
  // alert("Text:-" +" "+ text);  
          //  alert(text);
             if (text == "Closed") {
                document.getElementById("divUpdateStatus").style.display = "none";
                document.getElementById("divReviewFile").style.display = "none";
            }
            else {
                  document.getElementById("divUpdateStatus").style.display = "block";
                document.getElementById("divReviewFile").style.display = "block";
            }

        }

        function getValueFromCodeBehind(obj)
        {
            var Desc = '';
            var text = obj;

            

            if (text == "Code A") {
                Desc = "* Approved, manufacture/construction may commence";
            }
            else if (text == "Code B")
            {
                Desc = "* Acceptable subject to changes indicated.Resubmit for approval but manufacture / construction may commence.";
            }
            else if (text == "Code H")
            {
                Desc = "* Returned without review.";
            }
            else if (text == "Code C")
            {
                Desc = "* Amend as comments indicated and resubmit for approval.";
            }
            else if (text == "Code E")
            {
                Desc = "* Amend as comments indicated resubmit for record.";
            }
            else if (text == "Code G")
            {
                Desc = "* Drawing of this category is for information and hence not required to be approved.";
            }
            else if (text == "Code D")
            {
                Desc = "* Comments noted in Letter/memo attached to forwarding transmittal No.";
            }
            else if (text == "Code F")
            {
                Desc = "* Comments noted in Letter/memo attached to forwarding transmittal No....... dated......... Amend as comments indicated and resubmit for record.";
            }
            document.getElementById("tooltip").innerHTML = Desc;

            
        }
 $( function() {
    $("input[id$='dtDocumentDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtStartdate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
        });


        window.onload  = setValue;
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="Hidden1" runat="server" />
        <asp:HiddenField ID="hdDialog" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdFlowtype" runat="server" ></asp:HiddenField>
        <div class="container-fluid" style="max-height:76vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                     
                    <div class="form-group">
                        <label class="lblCss" for="DDLDocument">Document</label>
                         <asp:DropDownList ID="DDLDocument" CssClass="form-control" Enabled="false" runat="server" Visible="false"></asp:DropDownList>
                        <asp:Label runat="server" ID="LblDocNameLatest" Text="Label" CssClass="form-control"></asp:Label>
                    </div>
                    <div class="form-group" id="divforward" runat="server">
                       
                        <asp:CheckBox ID="chkforward" runat="server" Text="&nbsp;forward of document to next level" CssClass="form-control" AutoPostBack="True" OnCheckedChanged="chkforward_CheckedChanged"/>
                         
                    </div>
                    <div class="form-group" id="divStatus" runat="server">
                        <label class="lblCss" for="DDlStatus">Status</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <%--<i class="nav-link--icon fas fa-question-circle"></i>--%>
                         <asp:DropDownList ID="DDlStatus" CssClass="form-control" onchange="getValue(this)" required runat="server" OnSelectedIndexChanged="DDlStatus_SelectedIndexChanged"></asp:DropDownList>
                        <label id="tooltip" style="margin-top:5px;"></label>
                    </div>
                     <div class="form-group" id="divUSers" runat="server" style="overflow:scroll;display:block">
                        
                        <label class="lblCss" for="chkUserList">Select Users</label>
                         <div style="overflow:scroll">
                        <asp:CheckBoxList runat="server" CssClass="form-control chkChoice" ID="chkUserList" RepeatLayout="Table" RepeatDirection="Vertical" height="100px"  ></asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="form-group" id="Originator" runat="server">
                        <label class="lblCss" for="DDLOriginator">Originator</label>
                          <asp:RadioButtonList ID="RBLOriginator" runat="server" Width="100%" CssClass="lblCss" CellPadding="5" RepeatDirection="Horizontal" RepeatColumns="4">
                             </asp:RadioButtonList>
                    </div>
                    <div class="form-group" id="divcorrespondenceccto" runat="server" visible="false">
                        <label class="lblCss" for="chkcorrespondenceccto">CC To</label>
                        <asp:CheckBoxList ID="chkcorrespondenceccto" runat="server" RepeatDirection="Horizontal" RepeatColumns="6" AutoPostBack="false" CssClass="lblCss" CellPadding="5" CellSpacing="5">
                            
                        </asp:CheckBoxList>
                               
                             
                    </div>
                    <div class="form-group" id="divUpdateStatus" runat="server">
                        <label class="lblCss" for="RBL">Update Status to</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <%--<i class="nav-link--icon fas fa-question-circle"></i>--%>
                         <asp:RadioButtonList ID="RBLDocumentStatusUpdate" runat="server" BorderStyle="None" RepeatDirection="Horizontal" CssClass="form-control" required>
                             <asp:ListItem Value="Current" Selected="True">&nbsp;Current Document&nbsp;&nbsp;</asp:ListItem>
                             <asp:ListItem Value="All">&nbsp;All Related Documents</asp:ListItem>
                         </asp:RadioButtonList>
                    </div>

                    <div class="form-group" id="divRef" runat="server">
                        <label class="lblCss" for="txtrefNumber" id="lblrefno" runat="server">Ref. Number</label> &nbsp;<span id="spRef" runat="server" style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="txtrefNumber" runat="server" autocomplete="off" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                       
                    </div>
                    <div class="form-group" id="divCD" runat="server">
                        <label class="lblCss" for="dtDocumentDate">Cover Letter Date</label> &nbsp;<span id="spCoverDate" runat="server" style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtDocumentDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>
                    <div class="form-group" id="divIncmDate" runat="server" >
                        <label class="lblCss" for="dtStartdate">Incoming Recv. Date/Approval Date</label> &nbsp;<span id="spIncmDate" runat="server" style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtStartdate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                    </div>

                    <div class="form-group" id="divCUpload" runat="server">
                        <label class="lblCss" for="FilCoverLetter">Choose Cover Letter</label> &nbsp;<span id="spCUpload" runat="server" style="color:red; font-size:1.1rem;">*</span>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadCoverLetter" runat="server" CssClass="custom-file-input" />

                            <%--<asp:FileUpload ID="FilCoverLetter" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" for="FilCoverLetter">Choose covering letter</label>
                        </div>
                    </div>

                    <div class="form-group" id="divReviewFile" runat="server">
                        <label class="lblCss" for="FilCoverLetter">Choose Review File</label>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadDoc" runat="server" CssClass="custom-file-input" />

                            <%--<asp:FileUpload ID="FilCoverLetter" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" for="FilCoverLetter">Choose review file</label>
                        </div>
                    </div>
                     <div class="form-group" id="divFileAttachmentDoc" runat="server" visible="false">
                        <label class="lblCss" for="FilDocument">Choose attachment/s</label>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileAttachmentDoc" runat="server" AllowMultiple="true" CssClass="custom-file-input"  onchange="folderMsg(this)"/>

                            <%--<asp:FileUpload ID="FilDocument" CssClass="custom-file-input" runat="server" ClientIDMode="Static" /> --%>
                            <label class="custom-file-label" id="SelectedFilesAttachment" for="FileAttachmentDoc">Choose attachment/s</label>
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="lblCss" for="txtcomments">Comments</label>
                        <asp:TextBox ID="txtcomments" CssClass="form-control" TextMode="MultiLine" runat="server" ClientIDMode="Static" Height="150px" MaxLength="3000"></asp:TextBox>
                    </div>
                     <div class="form-group" id="divPassword" runat="server" visible="false">
                        <label class="lblCss" for="txtcomments">Enter Password</label>
                        <asp:TextBox ID="txtPassword" CssClass="form-control" TextMode="Password" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>
            </div> 
        </div>
         <div id="dvProgressBar" style=" text-align:center; position:relative; visibility:hidden;" >
                     <img src="../_assets/images/progress.gif" width="40" alt="loading"  /> <span style="color:#006699; font-weight:bold;">Uploading is in progress...</span>
                     </div> 
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClientClick="return DialogConfirm();" OnClick="btnSubmit_Click" />
                </div>
    </form>
</asp:Content>
