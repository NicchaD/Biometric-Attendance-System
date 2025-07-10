<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DocumentViewer.aspx.vb" Inherits="YMCAUI.DocumentViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <%--START: MMR | 12/13/2016 | YRS-AT-2685 | Added function to open new window for download option--%>
    <script type="text/javascript" language="JavaScript">
        function OpenDownloader() {
            window.open("PDFDownloader.aspx", "mywindow");
            return false;
        }
    </script>
    <%--END: MMR | 12/13/2016 | YRS-AT-2685 | Added function to open new window for download option--%>
</head>
<body>   
    <form id="form1" runat="server">        
        <table style="width: 100%;"border="0">
            <tr>
                <td align="left">
                        <asp:Button runat="server" ID="btnPrint" Text="Print" CssClass="Button_Normal" Visible="true" />
                        <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="Button_Normal" Visible="true" OnClientClick="javascript: return OpenDownloader();" />  <%-- MMR | 12/13/2016 | YRS-AT-2685 | Called function on click of button--%>                         
                </td>
            </tr>            
            <tr>
                <td>
                    <iframe id="frmRMDPrtLtr" visible="true" frameborder="1" runat ="server" width ="1024" height ="768"></iframe>                   
                </td>
            </tr>            
        </table>    
    </form>
</body>
</html>