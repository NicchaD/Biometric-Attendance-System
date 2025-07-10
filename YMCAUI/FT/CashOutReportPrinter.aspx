<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=12.0.1100.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CashOutReportPrinter.aspx.vb"
    Inherits="YMCAUI.CashOutReportPrinter" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Printer Set Up</title>
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
            CloseClientWindow();
//            window.moveTo(50, 50);
//            window.resizeTo(900, 650);
        }
        function DisplaySuccess() {

            alert('Report sent to printer.');
            CloseClientWindow();
        }
        function ChkMandatory() {
        	var e = document.getElementById("ddlPrinterFormName");
        	var f = document.getElementById("ddlPrinterLetterName");
        	var strprint = e.options[e.selectedIndex].value
        	var strprintLetter = f.options[f.selectedIndex].value
        	if (strprint == 0 || strprintLetter == 0) {
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
        <table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="400" border="1">
		<tr>
			<td>
				<table width = "100%">
				

        <br />
		<tr>
		<td class="Label_Small"  colspan="2" align="left"> <asp:Label id="Label1" runat="server" cssclass="Label_Small">Please select the printer for printing Forms.</asp:Label> </td>
			
		</tr>
		 <tr>
            <td colspan="2"  width="250">&nbsp;</td>
            
            </tr>
            <tr>
                <td class="Label_Small" width="250" align="left">
                
                <asp:Label id="LabelPeinterName" runat="server" cssclass="Label_Small">Printer Name</asp:Label>    
                </td>
                <td class="Label_Small">
                    <asp:DropDownList ID="ddlPrinterFormName" runat="server" Width="250"> 
					  </asp:DropDownList>
                </td>
            </tr>
            
            
            
			</table>
			</td>
		</tr>
        </table>
		<br />
		<br />
		<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="400" border="1">
		<tr>
			<td>
				<table width = "100%">
        <br /><br />
		<tr>
		<td class="Label_Small"colspan="2" align="left"> <asp:Label id="Label2" runat="server" cssclass="Label_Small">Please select the printer for printing Letters.<br />(The letter must be printed on Letterhead). </asp:Label> </td>
			
		</tr>
		 <tr>
            <td colspan="2"  width="250">&nbsp;</td>
            
            </tr>
            <tr>
                <td class="Label_Small" width="250" align="left">
                
                <asp:Label id="Label3" runat="server" cssclass="Label_Small">Printer Name</asp:Label>    
                </td>
                <td class="Label_Small">
                    <asp:DropDownList ID="ddlPrinterLetterName" runat="server" Width="250"> 
					
                    </asp:DropDownList>
                </td>
            </tr>
            
            
            <tr>
            <td colspan="2"  width="250">&nbsp;</td>
            
            </tr>
            
			</table>
			</td>
		</tr>
		
        </table>
		<table>
		<tr>
		<td colspan="2"  width="150">&nbsp;</td>
            <td colspan="2" align="center"><asp:Button ID="btnPrintsetting"  Width="100"  Text="Print" runat="server" ToolTip="Page Setup" BorderStyle="Inset" CssClass="Button_Normal" OnClientClick="javascript: return ChkMandatory();"></asp:Button> </td>
            
            </tr>
		</table>
    </div>
    <div id="divreport">
    
    <cr:CrystalReportViewer ID="CrystalReportViewer1" Style="z-index: 106; left: 0px;
        position: absolute; top: 8px" runat="server" Width="350px" Height="50px" AutoDataBind="True"
        PrintMode="ActiveX"></cr:CrystalReportViewer>
		 <cr:CrystalReportViewer ID="CrystalReportViewer2" Style="z-index: 106; left: 0px;
        position: absolute; top: 8px" runat="server" Width="350px" Height="50px" AutoDataBind="True"
        PrintMode="ActiveX" ></cr:CrystalReportViewer>
		 <cr:CrystalReportViewer ID="CrystalReportViewer3" Style="z-index: 106; left: 0px;
        position: absolute; top: 8px" runat="server" Width="350px" Height="50px" AutoDataBind="True"
        PrintMode="ActiveX" ></cr:CrystalReportViewer>
    <input id="hiddError" style="z-index: 107; left: 304px; position: absolute; top: 328px"
        type="hidden" runat="server">
        <input id="hiddReport" style="z-index: 107; left: 304px; position: absolute; top: 328px"
        type="hidden" runat="server">
        </div>
    </form>
    <script language="javascript">
        if (document.getElementById('hiddError').value != "") {
        	document.getElementById('divPrintSetting').style.display = 'none';
        	
        }
    </script>
	<script src="../JS/YMCA_CRScript.js" type="text/javascript"></script>
</body>
</html>
