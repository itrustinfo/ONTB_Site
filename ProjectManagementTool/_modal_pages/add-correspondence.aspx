    <%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-correspondence.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_correspondence" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">

     function ShowProgressBar(status) {
     if (status == "true") {
               
         document.getElementById('dvProgressBar').style.visibility = 'visible';
                
      }
      else {
                document.getElementById('dvProgressBar').style.visibility = 'hidden';
           }
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

        window.onload = setValue;

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
       
        <div class="container-fluid" style="max-height:76vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-12">
                     
                    <div class="form-group">
                        <label id="lblCorrespondenceType" runat="server" class="lblCss">Client to Consultant</label>
                    </div>

                    <div class="form-group">
                        <%--<label class="lblCss" for="DDLDocument">Document</label>--%>
                         <asp:DropDownList ID="DDLDocument" CssClass="form-control" Enabled="false" runat="server" Visible="false" ></asp:DropDownList>
                    </div>
                   
                    <div class="form-group" id="divStatus" runat="server">
                        <label class="lblCss" for="DDlStatus">Status</label> &nbsp;<span style="color:red; font-size:1.1rem;">*</span>
                        <%--<i class="nav-link--icon fas fa-question-circle"></i>--%>
                         <asp:DropDownList ID="DDlStatus" CssClass="form-control" onchange="getValue(this)" required runat="server" OnSelectedIndexChanged="DDlStatus_SelectedIndexChanged"></asp:DropDownList>
                        <label id="tooltip" style="margin-top:5px;"></label>
                    </div>
                    <div class="form-group" id="divRef" runat="server">
                        <label class="lblCss" for="txtrefNumber" id="lblrefno" runat="server">Ref. Number</label> &nbsp;<%--<span id="spRef" runat="server" style="color:red; font-size:1.1rem;">*</span>--%>
                        <asp:TextBox ID="txtrefNumber" runat="server" autocomplete="off" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                       
                    </div>
                    <div class="form-group" id="divCD" runat="server">
                        <label class="lblCss" for="dtDocumentDate">Cover Letter Date</label> &nbsp;<span id="spCoverDate" runat="server" style="color:red; font-size:1.1rem;">*</span>
                        <asp:TextBox ID="dtDocumentDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
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
                    
                </div>
            </div> 
        </div>
         <div id="dvProgressBar" style=" text-align:center; position:relative; visibility:hidden;" >
                     <img src="../_assets/images/progress.gif" width="40" alt="loading"  /> <span style="color:#006699; font-weight:bold;">Uploading is in progress...</span>
         </div> 
        <div >
             
             <div class="modal-footer">   
              
             <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
            </div>   
        </div>

    </form>
</asp:Content>
