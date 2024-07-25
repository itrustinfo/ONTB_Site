﻿<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.documents_contractor_replaced._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
      <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
 <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
 <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
 
      <style type="text/css">
         .hideItem {
         display:none;
     }
  .pager span { color:green;font-weight:bold;font-size:17px;}
   

    </style>
    <script type="text/javascript">
        function BindEvents() {
            $(".showModalPreviewDocument").click(function (e) {
                jQuery.noConflict();
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModDocumentPreview iframe").attr("src", url);
        $("#ModDocumentPreview").modal("show");
            });

            $(".showModalDocumentMail").click(function (e) {
                  jQuery.noConflict();
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModDocumentMail iframe").attr("src", url);
        $("#ModDocumentMail").modal("show");
            });

            $(".showModalDocumentReplacedView").click(function (e) {
                 jQuery.noConflict();
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModViewDocumentReplaced iframe").attr("src", url);
        $("#ModViewDocumentReplaced").modal("show");
            });

            $(".showModalDocumentEdit").click(function (e) {
                 jQuery.noConflict();
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModEditDocument iframe").attr("src", url);
        $("#ModEditDocument").modal("show");
            });

            $(".showModalGeneralDocumentEdit").click(function (e) {
                 jQuery.noConflict();
        e.preventDefault();
        var url = $(this).attr("href");
        $("#ModGeneralDocumentEdit iframe").attr("src", url);
        $("#ModGeneralDocumentEdit").modal("show");
            });

        }
        $(document).ready(function () {
            BindEvents();
            $('[data-toggle="tooltip"]').tooltip();   
        });

        //----
        function MakeStaticHeader(gridId, height, width, headerHeight, isFooter) {
 var tbl = document.getElementById(gridId);
 if (tbl) {
 var DivHR = document.getElementById('DivHeaderRow');
 var DivMC = document.getElementById('DivMainContent');
     var DivFR = document.getElementById('DivFooterRow');
     width = DivMC.offsetWidth;
 //*** Set divheaderRow Properties ****
 DivHR.style.height = headerHeight + 'px';
 DivHR.style.width = (parseInt(width) - 21) + 'px';
 DivHR.style.position = 'relative';
 DivHR.style.top = '0px';
 DivHR.style.zIndex = '10';
 DivHR.style.verticalAlign = 'top';

 //*** Set divMainContent Properties ****
 DivMC.style.width = width + 'px';
 DivMC.style.height = height + 'px';
 DivMC.style.position = 'relative';
 DivMC.style.top = -headerHeight + 'px';
 DivMC.style.zIndex = '1';

 //*** Set divFooterRow Properties ****
 DivFR.style.width = (parseInt(width) - 21) + 'px';
 DivFR.style.position = 'relative';
 DivFR.style.top = -headerHeight + 'px';
 DivFR.style.verticalAlign = 'top';
 DivFR.style.paddingtop = '2px';

 if (isFooter) {
 var tblfr = tbl.cloneNode(true);
 tblfr.removeChild(tblfr.getElementsByTagName('tbody')[0]);
 var tblBody = document.createElement('tbody');
 tblfr.style.width = '100%';
 tblfr.cellSpacing = "0";
 tblfr.border = "0px";
  tblfr.rules = "none";
 //*****In the case of Footer Row *******
 tblBody.appendChild(tbl.rows[tbl.rows.length - 1]);
 tblfr.appendChild(tblBody);
 DivFR.appendChild(tblfr);
 }
 //****Copy Header in divHeaderRow****
 DivHR.appendChild(tbl.cloneNode(true));
 }
}


        function MakeStaticHeaderNew(gridId, height, width, headerHeight, isFooter) {
 var tbl = document.getElementById(gridId);
 if (tbl) {
 var DivHR = document.getElementById('DivHeaderRowNew');
 var DivMC = document.getElementById('DivMainContentNew');
     var DivFR = document.getElementById('DivFooterRowNew');
     width = DivMC.offsetWidth;
 //*** Set divheaderRow Properties ****
 DivHR.style.height = headerHeight + 'px';
 DivHR.style.width = (parseInt(width) - 18) + 'px';
 DivHR.style.position = 'relative';
 DivHR.style.top = '0px';
 DivHR.style.zIndex = '10';
 DivHR.style.verticalAlign = 'top';

 //*** Set divMainContent Properties ****
 DivMC.style.width = width + 'px';
 DivMC.style.height = height + 'px';
 DivMC.style.position = 'relative';
 DivMC.style.top = -headerHeight + 'px';
 DivMC.style.zIndex = '1';

 //*** Set divFooterRow Properties ****
 DivFR.style.width = (parseInt(width) - 18) + 'px';
 DivFR.style.position = 'relative';
 DivFR.style.top = -headerHeight + 'px';
 DivFR.style.verticalAlign = 'top';
 DivFR.style.paddingtop = '2px';

 if (isFooter) {
 var tblfr = tbl.cloneNode(true);
 tblfr.removeChild(tblfr.getElementsByTagName('tbody')[0]);
 var tblBody = document.createElement('tbody');
 tblfr.style.width = '100%';
 tblfr.cellSpacing = "0";
 tblfr.border = "0px";
  tblfr.rules = "none";
 //*****In the case of Footer Row *******
 tblBody.appendChild(tbl.rows[tbl.rows.length - 1]);
 tblfr.appendChild(tblBody);
 DivFR.appendChild(tblfr);
 }
 //****Copy Header in divHeaderRow****
 DivHR.appendChild(tbl.cloneNode(true));
 }
}

function OnScrollDiv(Scrollablediv) {
  document.getElementById('DivHeaderRow').scrollLeft = Scrollablediv.scrollLeft;
document.getElementById('DivFooterRow').scrollLeft = Scrollablediv.scrollLeft;
}

        function OnScrollDivNew(Scrollablediv) {
  document.getElementById('DivHeaderRowNew').scrollLeft = Scrollablediv.scrollLeft;
document.getElementById('DivFooterRowNew').scrollLeft = Scrollablediv.scrollLeft;
}

        $( function() {
      $("input[id$='dtInDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

             $("input[id$='dtDocDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
             });

             $("input[id$='dtInToDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

             $("input[id$='dtDocToDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
             });

            //
            $("input[id$='dtInDateGD']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

             $("input[id$='dtDocDateGD']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
             });

             $("input[id$='dtInToDateGD']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

             $("input[id$='dtDocToDateGD']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });
    });

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <div class="container-fluid">
        
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <asp:Label ID="LblDocumentHeading" Text="Documents List" CssClass="text-uppercase font-weight-bold" runat="server" />
                                       <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                               </h6>
                               <div class="col-md-6 col-lg-4 form-group text-right">
                                   <asp:Button ID="btnSubmit" runat="server" Text="Submit" Visible="false" CssClass="btn btn-primary" OnClick="btnSubmit_Click"></asp:Button>
                    <asp:Button ID="btnback" runat="server" Text="Back To Dashboard" CssClass="btn btn-primary" PostBackUrl="/_content_pages/dashboard/"></asp:Button>
                                  
                </div>
                          </div>
                            </div>



                         <div class="container-fluid" id="ReportFilter" runat="server" visible="false">
                           
                             
            <div class="row">
                <div class="col-sm-12 mb-4" runat="server">
                     <div class="card">
                        <div class="card-body" >   
                            <div class="table-responsive">
                            <table style="width:100%;">
                                <tr>
                                    <td>
                                         <h6 class="card-title text-muted text-uppercase font-weight-bold">Contractor Uploaded Date From</h6>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <h6 class="card-title text-muted text-uppercase font-weight-bold">Contractor Uploaded Date To</h6>
                                    </td>
                                   <td>&nbsp;</td>
                                    <td>
                                        <h6 class="card-title text-muted text-uppercase font-weight-bold">ONTB Ref No.</h6>
                                    </td>
                                     <td>&nbsp;</td>
                                    <td>
                                        <h6 class="card-title text-muted text-uppercase font-weight-bold">Flow</h6>
                                    </td>
                                </tr>
                             <tr>
                                 <td style="width:20%;">
                                     <asp:TextBox ID="dtInDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                 <td>&nbsp;</td>
                                 <td style="width:20%;">
                                     <asp:TextBox ID="dtDocDate" CssClass="form-control" runat="server" placeholder="dd/mm/yyyy" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td style="width:20%;">
                                     <asp:TextBox ID="txtOntbRefNo" CssClass="form-control" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td style="width:20%;">
                                     <asp:DropDownList ID="DDLFlow" runat="server" CssClass="form-control" ></asp:DropDownList>
                                 </td>
                                 <td style="width:7%;">
                                     <asp:Button ID="btnsubmitfilter" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnsubmitfilter_Click"  />
                                 </td>
                             </tr>
                        </table>
                                </div>
                            </div>
                         </div>
                    </div>
                </div>
        </div>

                         <div class="row">
                             <div class="col-md-9">

                             </div>
                             <div class="col-md-2" style="padding:0px">
                                 <asp:DropDownList ID="DDLOption" runat="server" CssClass="form-control" >
                                    
                                     <asp:ListItem>Pending Documents</asp:ListItem>
                                     <asp:ListItem>Action Taken Documents</asp:ListItem>
                                 </asp:DropDownList>
                            </div>
                             <div class="col-md-1" style="padding:0px">
                                 <asp:Button ID="btnOption" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnOption_Click"  />
                             </div>
                                
                         </div> 

                        <div class="table-responsive" id="divMain" runat="server">


                            <asp:Label runat="server" Text="" ID="lblTotalcount"></asp:Label>
                             
                            <div style="overflow: hidden;" id="DivHeaderRow">
    </div>
                            <div style="overflow:scroll;" onscroll="OnScrollDiv(this)" id="DivMainContent">
                         <asp:GridView ID="GrdDocuments" EmptyDataText="No Documents Found." runat="server" Width="100%" AutoGenerateColumns="false" 
                                            AllowPaging="True" DataKeyNames="ActualDocumentUID" CssClass="table table-bordered" OnRowDataBound="GrdDocuments_RowDataBound" OnRowCommand="GrdDocuments_RowCommand" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="#F5F5F5" OnDataBound="GrdDocuments_DataBound" OnPageIndexChanging="GrdDocuments_PageIndexChanging" PageSize="15">
<AlternatingRowStyle BackColor="WhiteSmoke"></AlternatingRowStyle>
                                       <Columns>
                                           <asp:BoundField DataField="SerialNo"  HeaderText="Serial No" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="ProjectName"  HeaderText="Project Name" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                             <asp:BoundField DataField="ActualDocumentUID"  HeaderText="UID" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                            <HeaderStyle HorizontalAlign="Left" />

<ItemStyle CssClass="hideItem"></ItemStyle>
                                            </asp:BoundField>
                                           <asp:TemplateField HeaderText="Submittal Name" SortExpression="DocName">
                                            <ItemTemplate>
                                                <a href="#"  data-toggle="tooltip" data-placement="top" title='<%#GetTaskHierarchy_By_DocumentUID(Eval("DocumentUID").ToString())%>'><i class="fa fa-info-circle" style="color:black;" aria-hidden="true"></i></a>&nbsp;&nbsp; <a href="/_content_pages/documents/Default.aspx?SubmittalUID=<%#Eval("DocumentUID")%>"><%#Eval("DocName")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField ItemStyle-Width="35%" HeaderStyle-Width="35%" HeaderText="Document Name" ItemStyle-HorizontalAlign="Left" SortExpression="ActualDocument_Name">
                                            <ItemTemplate>
                                                       <img width="24" src='../../_assets/images/<%#GetDocumentTypeIcon(Eval("ActualDocument_Type").ToString())%>' alt='<%#Eval("ActualDocument_Type")%>' />  &nbsp;&nbsp;
                                                       <%--<asp:LinkButton ID="lnkviewpdf" class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "view" : "hideItem" %>' runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocument_Path")%>' CommandName="ViewDoc"><%#Eval("ActualDocument_Name")%></asp:LinkButton>--%>
                                                       <a id="ShowFile" href='/_modal_pages/preview-document.aspx?ActualDocumentUID=<%#Eval("ActualDocumentUID")%>&previewpath=<%#Eval("ActualDocument_Path").ToString().Replace('&','!')%>' class='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? "showModalPreviewDocument" : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? "showModalPreviewDocument" : "hideItem" %>'>
                                                          <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("ActualDocument_Name")%>'></asp:Label></a>
                                                       <asp:Label ID="lblName" runat="server" Text='<%#Eval("ActualDocument_Name")%>' Visible='<%#Convert.ToString(Eval("ActualDocument_Type")) == ".pdf" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xls" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".xlsx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".docx" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".doc" ? false : Convert.ToString(Eval("ActualDocument_Type")) == ".PDF" ? false : true %>'></asp:Label>
                                            </ItemTemplate>

<HeaderStyle Width="35%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Document Type" SortExpression="ActualDocument_Type">
                                            <ItemTemplate>
                                                <%#GetDocumentName(Eval("ActualDocument_Type").ToString())%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:BoundField DataField="ActualDocument_CurrentStatus" HeaderText="Current Status" SortExpression="ActualDocument_CurrentStatus">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ProjectRef_Number" HeaderText="ONTB Reference #" SortExpression="ProjectRef_Number">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           <asp:BoundField DataField="Ref_Number" HeaderText="Originator Reference #" SortExpression="Ref_Number">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                             <asp:BoundField DataField="IncomingRec_Date" HeaderText="Incoming Recv. Date" SortExpression="IncomingRec_Date" DataFormatString="{0:dd/MM/yyyy}" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                               <asp:BoundField DataField="Document_Date" HeaderText="Document Date" SortExpression="Document_Date" DataFormatString="{0:dd/MM/yyyy}" >
                                            <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                           
                                           <asp:TemplateField>
                                            <ItemTemplate>              
                                                      <asp:LinkButton ID="LnkDownloadnew" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ActualDocumentUID")%>' CommandName="download">Download</asp:LinkButton>               
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentMail" href="/_modal_pages/view-documenthistory.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>">View History</a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                            <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentReplacedView" href="/_modal_pages/view-documentReplaced.aspx?DocID=<%#Eval("ActualDocumentUID")%>&ProjectUID=<%#Eval("ProjectUID")%>">Replace Document</a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                           
                                           
                                                <asp:BoundField DataField="FlowUID" HeaderText="FlowUID" SortExpression="FlowUID" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                    <HeaderStyle HorizontalAlign="Left" />

<ItemStyle CssClass="hideItem"></ItemStyle>
                                                </asp:BoundField>
                                            <asp:BoundField DataField="DocumentUID" HeaderText="DocumentUID" SortExpression="DocumentUID" ItemStyle-CssClass="hideItem" HeaderStyle-CssClass="hideItem">
                                                    <HeaderStyle HorizontalAlign="Left" />

<ItemStyle CssClass="hideItem"></ItemStyle>
                                                </asp:BoundField>
                                           <%--   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <a class="showModalDocumentEdit" href="/_modal_pages/edit-document.aspx?DocID=<%#Eval("ActualDocumentUID")%>&pUID=<%#Eval("ProjectUID")%>&wUID=<%#Eval("WorkPackageUID")%>&type=search"><span title="Edit" class="fas fa-edit"></span></a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                       </Columns>

<HeaderStyle BackColor="#666666" ForeColor="White"></HeaderStyle>

                                       <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Previous" Position="TopAndBottom" />
                                       <PagerStyle CssClass="pager" />
                                       </asp:GridView>
                         </div>
                            <div id="DivFooterRow" style="overflow:hidden">
    </div>
                            </div>
                         
                        </div>
                    </div>
                </div>
            </div>
        </div>
     <%--View document histroy modal--%>
    <div id="ModDocumentPreview" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Document Preview</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
     <%--View document History  modal--%>
    <div id="ModDocumentMail" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Document History</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" onclick="javascript:closepopup();"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
     <%--View document modal--%>
    <div id="ModViewDocumentReplaced" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View Document Replaced Details</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
       <%--edit document modal--%>
    <div id="ModEditDocument" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit Document</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--edit general document modal--%>
    <div id="ModGeneralDocumentEdit" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Edit General Document</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:500px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</asp:Content>
