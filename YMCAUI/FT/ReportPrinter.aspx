<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=12.0.1100.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="../UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="../UserControls/YMCA_Footer_WebUserControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportPrinter.aspx.vb"
    Inherits="YMCAUI.ReportPrinter" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>YMCA YRS</title>
    <LINK href="../CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <script language="javascript">
        function SimulateClientPrintClick() {
            window.moveTo(20000, 20000);
            var cr = document.getElementById('CrystalReportViewer1:_ctl2:_ctl2');
            if (cr != null) {
                cr.click();
            } else {
                alert('There was a problem finding the print control. Please contact IT support.');
                CloseClientWindow();
            }
        }
        function CloseClientWindow() {
            setTimeout('window.close()', 2000);
            //window.close(); 
        }
        function PrintFromServerError() {
            document.getElementById('divPrintSetting').style.display = 'none';
            alert('There was a problem printing from the server. Please contact IT support.');
            window.close();
        }
        function DisplayError() {
            //IB
            document.getElementById('divPrintSetting').style.display = 'none';
            

            var v1 = document.getElementById('hiddError');
            if (v1 != null)
                alert(v1.value);

            window.moveTo(50, 50);
            window.resizeTo(900, 650);
        }
        function DisplaySuccess() {

            alert('Report sent to printer.');
            CloseClientWindow();
        }
        function ChkMandatory() {
            var e = document.getElementById("ddlPrinterName");
            var strprint = e.options[e.selectedIndex].value
            if (strprint == 0) {
                alert('Please select printer');
                return false;
            }
            return true;
            
        }
    </script>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <div id="divPrintSetting">
		
        <table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="100%">
        <tr>
                <td colspan="2">
							<YRSControls:YMCA_Toolbar_WebUserControl id="YMCA_Toolbar_WebUserControl1" runat="server" ShowLogoutLinkButton="false" ShowHomeLinkButton="false"></YRSControls:YMCA_Toolbar_WebUserControl>
                </td>
            </tr>
			<tr>
            <td class="Td_HeadingFormContainer" align="left" colspan="2">
                <img title="image" height="10" alt="image" src="../images/spacer.gif" width="10" />
				<asp:label ID="lblMenuHeading" runat="server"></asp:label>
                
            </td>
        </tr>
			 <tr>
            <td colspan="2">&nbsp;</td>
            
            </tr>
			 <tr>
            <td colspan="2">&nbsp;</td>
            
            </tr>
			 <tr>
            <td colspan="2">&nbsp;</td>
            
            </tr>
            <tr>
                <td class="Label_Small" width="100" align="left">
                
                <asp:Label id="LabelPeinterName" runat="server" cssclass="Label_Small">Printer Name</asp:Label>    
                </td>
                <td class="Label_Small">
                    <asp:DropDownList ID="ddlPrinterName" runat="server" Width="250"> 
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="display:none">
                <td class="Label_Small" width="100">
                   <asp:Label id="Label1" runat="server" cssclass="Label_Small">Paper Type</asp:Label>     
                </td>
                <td class="Label_Small">
                    <asp:DropDownList ID="ddlPaperSize" runat="server">
                        <asp:ListItem Value="A4">A4</asp:ListItem>
                        <asp:ListItem Value="A3">A3</asp:ListItem>
                        <asp:ListItem Value="Legal">Legal</asp:ListItem>
                        <asp:ListItem Value="Letter">Letter</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="display:none">
                <td class="Label_Small" width="100">
                   <asp:Label id="Label2" runat="server" cssclass="Label_Small">Orientation</asp:Label>      
                </td>
                <td class="Label_Small">
                    <asp:RadioButtonList ID="rdolstOrientation" runat="server">
                        <asp:ListItem>Portrait</asp:ListItem>
                        <asp:ListItem>Landscape</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
            <td colspan="2">&nbsp;</td>
            
            </tr>
			 <tr>
            <td colspan="2">&nbsp;</td>
            
            </tr>
			 <tr>
            <td colspan="2">&nbsp;</td>
            
            </tr>
            <tr>
            <td colspan="2" align="center" class="Td_ButtonContainer"><asp:Button ID="btnPrintsetting"  Width="100"  Text="Print" runat="server" ToolTip="Page Setup" BorderStyle="Inset" CssClass="Button_Normal" OnClientClick="javascript: return ChkMandatory();"></asp:Button> </td>
            
            </tr>
        </table>
			<YRSControls:YMCA_Footer_WebUserControl id="YMCA_Footer_WebUserControl1" runat="server"></YRSControls:YMCA_Footer_WebUserControl><br>
    </div>
    <div id="divreport">
    <asp:Button ID="btnFirst" Style="z-index: 104; left: 384px; position: absolute; top: 16px"
        runat="server" Visible="False" ToolTip="First Page" BorderStyle="Inset" CssClass="Button_Normal"
        Font-Size="Small" Font-Bold="True" Text="<<" Width="32px" Height="24px"></asp:Button>
    <asp:Button ID="btnNext" Style="z-index: 103; left: 448px; position: absolute; top: 16px"
        runat="server" Visible="False" ToolTip="Next Page" BorderStyle="Inset" CssClass="Button_Normal"
        Font-Size="Small" Font-Bold="True" Text=">" Width="32px" Height="24px"></asp:Button>
    <asp:Button ID="btnPrevious" Style="z-index: 102; left: 416px; position: absolute;
        top: 16px" runat="server" Visible="False" ToolTip="Previous page" BorderStyle="Inset"
        CssClass="Button_Normal" Font-Size="Small" Font-Bold="True" Text="<" Width="32px"
        Height="24px"></asp:Button>
    <asp:Button ID="btnLast" Style="z-index: 101; left: 480px; position: absolute; top: 16px"
        runat="server" Visible="False" ToolTip="Last page" BorderStyle="Inset" CssClass="Button_Normal"
        Font-Size="Small" Font-Bold="True" Text=">>" Width="32px" Height="24px"></asp:Button>
    <asp:Button ID="btnExport" Style="z-index: 105; left: 528px; position: absolute;
        top: 16px" runat="server" Visible="False" BorderStyle="Inset" CssClass="Button_Normal"
        Font-Bold="True" Text="Export to PDF"></asp:Button>
    <cr:CrystalReportViewer ID="CrystalReportViewer1" Style="z-index: 106; left: 0px;
        position: absolute; top: 8px" runat="server" Width="350px" Height="50px" AutoDataBind="True"
        PrintMode="ActiveX"></cr:CrystalReportViewer>
    <input id="hiddError" style="z-index: 107; left: 304px; position: absolute; top: 328px"
        type="hidden" runat="server">
        <input id="hiddReport" style="z-index: 107; left: 304px; position: absolute; top: 328px"
        type="hidden" runat="server">
        </div>
    </form>
    <script language="javascript">
        if (document.getElementById('hiddError').value != "") {
            document.getElementById('divPrintSetting').style.display = 'none';
            //alert();
        }
    </script>
	<script src="../JS/YMCA_CRScript.js" type="text/javascript"></script>
</body>
</html>
