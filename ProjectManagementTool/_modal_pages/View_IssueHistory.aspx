<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="View_IssueHistory.aspx.cs" Inherits="ProjectManagementTool._modal_pages.View_IssueHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
     <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
                return false;
            }
    </script>

    <style type="text/css">
        .hiddencol { display: none; }
    </style>
    <script type="text/javascript">

        //function showImgModal(s) {

        //    alert(s);
        //    e.preventDefault();
        //    jQuery.noConflict();
        //    $("#ModIssueStatusImage img").attr("src", s);
        //    $("#ModIssueStatusImage img").attr("width", "100%")
        //    $("#ModIssueStatusImage img").attr("height", "auto")
        //    $('#ModIssueStatusImage').modal({ backdrop: 'static', keyboard: false })
        //    $("#ModIssueStatusImage").modal('show');
        //}

        //function showPdfModal(s) {

        //    alert('reached here');
        //    e.preventDefault();
        //    jQuery.noConflict();
        //    $("#ModIssueStatusPdf iframe").attr("src", s);
        //    $("#ModIssueStatusPdf iframe").attr("width", "100%")
        //    $("#ModIssueStatusPdf iframe").attr("height", "auto")
        //    $('#ModIssueStatusPdf').modal({ backdrop: 'static', keyboard: false })
        //    $("#ModIssueStatusPdf").modal('show');
        //}

        $(document).ready(function () {
            $(".showDocModal").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModAddIssueStatus iframe").attr("src", url);
                $('#ModAddIssueStatus').modal({ backdrop: 'static', keyboard: false })
                $("#ModAddIssueStatus").modal("show");
            });

            $(".EditStatusModal").click(function (e) {
                e.preventDefault();
                jQuery.noConflict();
                var url = $(this).attr("href");
                $("#ModEditIssueStatus iframe").attr("src", url);
                $('#ModEditIssueStatus').modal({ backdrop: 'static', keyboard: false })
                $("#ModEditIssueStatus").modal("show");
            });

            //$(".IssueStatusImagePreview").click(function (e) {

            //    e.preventDefault();
            //    jQuery.noConflict();

            //    $("#ModIssuePreview iframe").html("");

            //    var url = $(this).attr("href");

            //    $("#ModIssuePreview iframe").attr("src", url);
            //    $("#ModIssuePreview").modal("show");
            //});

            $(".showDocModal").click(function (e) {

                e.preventDefault();
                jQuery.noConflict();

                $("#ModIssuePreview iframe").html("");

                var url = $(this).attr("href");

                $("#ModIssuePreview iframe").attr("src", url);
                $("#ModIssuePreview iframe").attr("width", "100%")
                $("#ModIssuePreview iframe").attr("height", "600px")
                $("#ModIssuePreview").modal("show");
            });


           
        });
    </script>
    <script type="text/javascript">
        
        function PrintDiv(id) {
            var data = document.getElementById(id).innerHTML;
            var myWindow = window.open(' ', ' ', 'height=600,width=800');
            myWindow.document.write('<html><head><title></title>');
            /*optional stylesheet*/ myWindow.document.write('<link rel="stylesheet" href="Css/style.css" type="text/css" />');
            myWindow.document.write('</head><body >');
            myWindow.document.write(data);
            myWindow.document.write('</body></html>');
            myWindow.document.close(); // necessary for IE >= 10

            myWindow.onload = function () { // necessary if the div contain images

                myWindow.focus(); // necessary for IE >= 10
                myWindow.print();
                myWindow.close();
            };
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddDocumentModal" runat="server">
     
    <div class="container-fluid" style="overflow-y:auto; min-height:83vh;">
         <div class="row">
             <div class="col-sm-12">
              <div style="text-align:center;font-size:large"><b>ONTB-BWSSB Stage 5 Project Monitoring Tool</b></div>
              <div style="text-align:center;font-size:large"><b>Project Name :</b><asp:label ID="lblProject1" runat="server"></asp:label></div> 
              <div style="text-align:center;font-size:large"><b>Issues Module</b></div> 
              <div><span><b>Date : </b></span><b> <%= DateTime.Now.ToString("dd MMM yyyy") %> </b></div>
          </div>
        </div>

        <div class="row" style="margin-top:10px">
           <div class="col-sm-12">
                <div>
                     <label class="lblCss" for="lblWorkPackage">Issue Description</label><b>:</b>
                     <asp:Label ID="LblIssue" class="lblCss" Font-Bold="true" runat="server"></asp:Label>
                </div>
               
               <div>
                     <label class="lblCss" for="lblUser">Reporting User</label><b>:</b>
                     <asp:Label ID="LblUser" class="lblCss" Font-Bold="true" runat="server"></asp:Label>
                </div>
           </div>
        </div>
        <br />

        <div class="row">
            <div class="col-sm-12">
                <div class="table-responsive">
                        <asp:GridView ID="GrdIssueStatus" runat="server" Width="100%" PageSize="10" AllowPaging="true" CssClass="table table-bordered" DataKeyNames="id" EmptyDataText="No Status Found" AutoGenerateColumns="false" OnRowDataBound="GrdIssueStatus_RowDataBound" OnRowCommand="GrdIssueStatus_RowCommand" OnRowDeleting="GrdIssueStatus_RowDeleting" OnPageIndexChanging="GrdIssueStatus_PageIndexChanging">
                        <Columns>
                             <asp:BoundField DataField="Count" HeaderText="Serial No"  >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Issue Description"  >
                            <HeaderStyle HorizontalAlign="Left" CssClass="hiddencol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="issue_user" HeaderText="Issue User" >
                            <HeaderStyle HorizontalAlign="Left" CssClass="hiddencol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="issue_date" HeaderText="Issue Date" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="15%" ItemStyle-Width="15%">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="issue_status" HeaderText="Issue Status">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="doc_name" HeaderText="Attachments" HtmlEncode="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="LinkToReviewFile" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                           </div>
                </div>
            </div>

       
    </div>
 
   
    <div class="modal-footer">
        <div style="padding:10px">
            <asp:Button runat="server" Text="Download All" CssClass="btn btn-primary" OnClick ="LinkDownload_Click" />
            <input type="button" value="Print" class="btn btn-primary" onclick="PrintDiv('pDiv')" />
            <asp:Button ID="btnprintpdf" runat="server" CssClass="btn btn-primary" Text="Print PDF" Visible="false" OnClick="btnprintpdf_Click" />
         </div>
    </div>

      <div id="pDiv" style="display:none">
           <div class="row">
             <div class="col-sm-12">
              <div style="text-align:center;font-size:large"><b>ONTB-BWSSB Stage 5 Project Monitoring Tool</b></div>
              <div style="text-align:center;font-size:large"><b>Project Name :</b><asp:label ID="lblProject" runat="server"></asp:label></div>
               <div style="text-align:center;font-size:large"><b>Issues Module</b></div>  
              <div><span><b>Date : </b></span><b> <%= DateTime.Now.ToString("dd MMM yyyy") %> </b></div>
          </div>
        </div>

        <div class="row" style="margin-top:10px">
           <div class="col-sm-12">
                <div>
                     <label class="lblCss" for="lblWorkPackage">Issue Description</label><b>:</b>
                     <asp:Label ID="Label2" class="lblCss" Font-Bold="true" runat="server"></asp:Label>
                </div>
               
               <div>
                     <label class="lblCss" for="lblUser">Reporting User</label><b>:</b>
                     <asp:Label ID="Label3" class="lblCss" Font-Bold="true" runat="server"></asp:Label>
                </div>
           </div>
        </div>

        <br />

         
        <div class="row">
            <div class="col-sm-12">
                <div class="table-responsive">
                        <asp:GridView ID="GridView1" runat="server" Width="100%" PageSize="10" AllowPaging="true" CssClass="table table-bordered" DataKeyNames="id" EmptyDataText="No Status Found" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" >
                        <Columns>
                            <asp:BoundField DataField="Count" HeaderText="Serial No"  >
                            <HeaderStyle HorizontalAlign="Left"  />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="Issue Description"  >
                            <HeaderStyle HorizontalAlign="Left" CssClass="hiddencol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="issue_user" HeaderText="Issue User" >
                            <HeaderStyle HorizontalAlign="Left" CssClass="hiddencol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="issue_date" HeaderText="Issue Date" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="15%" ItemStyle-Width="15%">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="issue_status" HeaderText="Issue Status">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="doc_name" HeaderText="Attachments" HtmlEncode="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="LinkToReviewFile" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                           </div>
                </div>
            </div>
       
        </div>

     <%--Add Issue status resource modal--%>
    <div id="ModIssuePreview" class="modal it-modal-xl fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Preview</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" ><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:600px;width:700px" loading="lazy"></iframe>
			    </div>
               <div class="modal-footer" style="padding:5px;background-color:black">
                    <div class="row" style="padding:10px;background-color:black;text-align:center"><button type="button" class="btn btn-secondary" data-dismiss="modal" style="color:white">Close</button></div>
              </div>
		    </div>
	    </div>
    </div>
         <div id="ModEditIssueStatus" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Status</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:280px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

         <%--<div id="ModIssueStatusImage" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View Issue Image</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" ><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <img class="embed-responsive" src="" />
			    </div>
              <div class="modal-footer" style="padding:5px;background-color:black">
               <div style="padding:10px;background-color:black;text-align:center"><button type="button" class="btn btn-secondary" data-dismiss="modal" style="color:white">OK</button></div>
              </div>
		    </div>
	    </div>
    </div>--%>

      <%--  <div id="ModIssueStatusPdf" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View Issue pdf</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" ><span aria-hidden="true">&times;</span></button>
			    </div>
			   <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:750px;" loading="lazy"></iframe>
			    </div>
              <div class="modal-footer" style="padding:5px;background-color:black">
               <div style="padding:10px;background-color:black;text-align:center"><button type="button" class="btn btn-secondary" data-dismiss="modal" style="color:white">OK</button></div>
              </div>
		    </div>
	    </div>
    </div>--%>
    </form>
</asp:Content>
