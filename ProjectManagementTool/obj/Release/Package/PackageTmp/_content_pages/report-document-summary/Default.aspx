<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Default.aspx.cs" Inherits="ProjectManager._content_pages.report_document_summary.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

     <script>
  $( function() {
    $("input[id$='dtStartDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

      $("input[id$='dtEndDate']").datepicker({
      changeMonth: true,
        changeYear: true,
      dateFormat:'dd/mm/yy'
      });

    });
  </script>
    <style type="text/css">
        .lblalign {
            text-align:left;
        }
    </style>
   <%-- <script type="text/javascript">
        $(document).ready(function () {
            $('#loader').fadeOut();
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
      <div class="container-fluid" id="DivDocumentSummary" runat="server">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                            <h5 id="DocumentSummaryReportName" style="font-weight:bold;" runat="server">Projectwise Document Summary Report for water projects as on date</h5>
                                            
                                            </div>
                        <div class="card-title">
                        <div class="d-flex justify-content-between">
                              <h6 class="text-muted">
                                   <%--<asp:Label ID="LblDocumentSummary" CssClass="text-uppercase font-weight-bold" runat="server" Text="Status of Documents submitted by the Contractor"  />--%>
                              </h6>
                               <div>
                                    <asp:Button ID="btnDocumentSummaryExportExcel" runat="server" Text="Export Excel" Visible="true" CssClass="btn btn-primary" OnClick="btnExportExcel_Click" />
                                    <asp:Button ID="btnDocumentSummaryExportPDF" runat="server" Text="Export PDF" Visible="true" CssClass="btn btn-primary" OnClick="btnExportPDF_Click" />
                               </div>
                          </div>
                            </div>
                        <div class="table-responsive">
                                <asp:GridView ID="GrdDocumentSummary" EmptyDataText="No Documents Found." HeaderStyle-HorizontalAlign="Center" runat="server" Width="100%" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="LightGray"
                                          AllowPaging="false" CssClass="table table-bordered">
                                       <Columns>
                                            <asp:BoundField DataField="Project" HeaderText ="Project" ItemStyle-HorizontalAlign="Center"  />
                                            <asp:BoundField DataField="Total" HeaderText ="Total Documents as per (Online flow-2)"  ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField  HeaderText="Total Number of Pending Documents" ItemStyle-HorizontalAlign="Center"  >
                                                    <ItemTemplate>
                                                        <a target="_blank" class="showModalDocumentView" href="/_content_pages/documents-contractor-replaced/?&Rtype=Pending Documents&PrjUID=<%#Eval("ProjectId")%>"><%#Eval("Pending")%>   </a>
                                                    </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField  HeaderText="Total Number of Action Taken Documents" ItemStyle-HorizontalAlign="Center"  >
                                                    <ItemTemplate>
                                                        <a target="_blank" class="showModalDocumentView" href="/_content_pages/documents-contractor-replaced/?&Rtype=Action Taken Documents&PrjUID=<%#Eval("ProjectId")%>"><%#Eval("ActionTaken")%>   </a>
                                                    </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                       </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                </div>
        </div>
</asp:Content>