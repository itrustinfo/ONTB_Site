<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="ProjectManagementTool.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <script type="text/javascript">

       function testopen() {
           window.open("http://122.185.231.235/index.html", "", "menubar=0,scrollbars=0,resizable=0,width=1000,height=550,left=200,top=50");
       }
   </script>

         
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <iframe src="http://184.168.122.102:1229/" width="90%" height="400" name="iframe"></iframe>
            
        </div>
        <asp:Button ID="Button1" runat="server" Text="Open URL" OnClientClick="javascript:testopen();" />
    </form>
</body>
</html>
