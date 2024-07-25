<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="preview-pdf-doc.aspx.cs" Inherits="ProjectManagementTool._modal_pages.preview_pdf_doc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <style type="text/css">
        .aclass th {
            background:Green; font-weight:bold; color: white
        }

        .aclass{border: 1px solid black;}
.aclass tr td {background:white; font-weight:bold; color: black}
.aclass
{
    overflow :auto;
}
   .tabpanlecss{
      overflow :scroll;
   }
 .TabStyle .ajax__tab_header
        {
            cursor: pointer;
            background-color: #f1f1f1;
            font-size: 14px;
            font-weight: bold;
            font-family: Arial, Helvetica, sans-serif;
            height: 36px;
            border-bottom: 1px solid #bebebe;
            width:100%;
        }
  .TabStyle .ajax__tab_active .ajax__tab_tab .ajax__tab_outer
        {
            border: 1px solid;
            border-color: #bebebe #bebebe #e1e1e1 #bebebe;
            background-color: #e1e1e1;
            padding: 10px;
            border-bottom: none;
        }
  .TabStyle .ajax__tab_active .ajax__tab_tab .ajax__tab_outer:hover
        {
            border: 1px solid;
            border-color: #bebebe #bebebe #e1e1e1 #bebebe;
            background-color: #e1e1e1;
            padding: 10px;
            border-bottom: none;
        }
          .TabStyle .ajax__tab_tab
        {
            border: 1px solid;
            border-color: #e1e1e1 #e1e1e1 #bebebe #e1e1e1;
            background-color: #f1f1f1;
            color: #777777;
            cursor: pointer;
            text-decoration: none;
            padding: 10px;
        }
        .TabStyle .ajax__tab_tab:hover
        {
            border: 1px solid;
            border-color: #bebebe #bebebe #e1e1e1 #bebebe;
            background-color: #e1e1e1;
            color: #777777;
            cursor: pointer;
            text-decoration: none;
            padding: 10px;
            border-bottom: none;
        }
        .TabStyle .ajax__tab_active .ajax__tab_tab, .TabStyle .ajax__tab_tab, .TabStyle .ajax__tab_header .ajax__tab_tab
        {
            margin: 0px 0px 0px 0px;
        }
  .TabStyle .ajax__tab_body
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 10pt;
            border-top: 0;
            border: 1px solid #bebebe;
            border-top: none;
            padding: 5px;
            background-color: #e1e1e1;
            width:100%;
        }

    </style>

     <script type="text/javascript">
        
        function PrintDiv(id) {
           var data = document.getElementById(id).innerHTML;
            var myWindow = window.open(' ', ' ', 'height=600,width=800');
            myWindow.document.write('<html><head><title></title>');
            /*optional stylesheet*/ myWindow.document.write('<link rel="stylesheet" href="Css/style.css" type="text/css" />');
            myWindow.document.write('</head><body >');
            myWindow.document.write(data);
            myWindow.document.write('</body></html>');
            myWindow.document.close();

            myWindow.focus(); // necessary for IE >= 10
                myWindow.print();
                myWindow.close();// necessary for IE >= 10

            //myWindow.onload = function () { // necessary if the div contain images

                
            //};
        }
     </script>

    <script>
        $(document).ready(function () {
            $(".showPdfModal").click(function (e) {

                e.preventDefault();
                jQuery.noConflict();

                $("#ModPdfPrint iframe").html("");

                var url = $(this).attr("href");

                $("#ModPdfPrint iframe").attr("src", url);
                $("#ModPdfPrint iframe").attr("width", "100%")
                $("#ModPdfPrint iframe").attr("height", "600px")
                $("#ModPdfPrint").modal("show");
            });
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <div class="container-fluid" >
        
        <div class="row">
           <%-- <div class="col-sm-3"></div>--%>
            <div class="col-sm-12" style="overflow-y:hidden">
                
                  <form runat="server">
                      <asp:Button runat="server" CssClass="btn btn-success fa-pull-right" ID="btnImgPrint" Text ="print Image" Visible="false" OnClientClick="PrintDiv('issue_image')" />
                      <div id="issue_image">
                        <asp:Image runat="server" id="image1" Width="100%"></asp:Image>
                      </div>
                  </form>
                </div>
           
        </div>
    </div>

    <div id="ModPdfPrint" class="modal it-modal-xl fade">
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
</asp:Content>

 
