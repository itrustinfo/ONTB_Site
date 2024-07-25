    <%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-correspondence.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_correspondence" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type="text/javascript">
        // function ShowProgressBar(status) {
        //    if (status == "true") {
               
        //            document.getElementById('dvProgressBar').style.visibility = 'visible';
                
        //    }
        //    else {
        //        document.getElementById('dvProgressBar').style.visibility = 'hidden';
        //    }
        //}
</script>
     <style type="text/css">
        .hiddencol { display: none; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
   
        <div class="container-fluid" style="max-height:76vh; overflow-y:auto;">
        <div class="row">
            <div class="col-sm-12">
                <div class="table-responsive">
                        <asp:GridView ID="GrdViewCorrespondence" runat="server" Width="100%" AllowPaging="false" CssClass="table table-bordered" DataKeyNames="CorrespondenceUID" EmptyDataText="No Status Found" AutoGenerateColumns="false" OnRowDataBound="GrdViewCorrespondence_RowDataBound" OnRowCommand="GrdViewCorrespondence_RowCommand" OnRowDeleting="GrdViewCorrespondence_RowDeleting" >
                        <Columns>
                            <asp:BoundField DataField="CorrespondenceUID" HeaderText="UID" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RefNumber" HeaderText="Ref.Number">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CoverletterDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CoverLetterFile" HeaderText="" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                             <asp:TemplateField HeaderText="Letter">
                                  <ItemTemplate>
                                       <asp:LinkButton ID="lnkdownload1" runat="server" CommandArgument='<%#Eval("CoverLetterFile")%>' CausesValidation="false" CommandName="download1">Download</asp:LinkButton>
                                  </ItemTemplate>
                             </asp:TemplateField>
                            <asp:BoundField DataField="LinkToReviewFile" HeaderText="" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Review">
                                  <ItemTemplate>
                                       <asp:LinkButton ID="lnkdownload2" runat="server" CommandArgument='<%#Eval("LinkToReviewFile")%>' CausesValidation="false" CommandName="download2">Download</asp:LinkButton>
                                  </ItemTemplate>
                             </asp:TemplateField>
                            <asp:BoundField DataField="Status_Comments" HeaderText="Comments">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                  </div>
                </div>
            </div>
           
        </div>
        <%-- <div id="dvProgressBar" style=" text-align:center; position:relative; visibility:hidden;" >
              <img src="../_assets/images/progress.gif" width="40" alt="loading"  /> <span style="color:#006699; font-weight:bold;">Uploading is in progress...</span>
         </div> --%>
        <div class="modal-footer">
            <%--<asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClientClick="return DialogConfirm();" OnClick="btnSubmit_Click" />--%>
        </div>
    </form>
</asp:Content>
