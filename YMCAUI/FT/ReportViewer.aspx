<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportViewer.aspx.vb"
    Inherits="YMCAUI.ReportViewer" %>

<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=12.0.1100.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ReportViewer</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    <%-- START:HT:3/18/2020: YRS-AT-4750:Added following code to show barcode on reportviewer --%>
    <style type="text/css">

        @font-face {
            font-family: "IDAutomationHC39M";
            src: url("IDAutomationHC39M.eot");
        }

        @font-face {
            font-family: "IDAutomationHC39M";
            src: url("IDAutomationHC39M.ttf");
        }

        @font-face {
            font-family: "IDAutomationHC39M";
            src: url("IDAutomationHC39M.otf");
        }

        .barcode {
            font-family: IDAutomationHC39M;
        }
  </style> 
     <%-- END:HT:3/18/2020: YRS-AT-4750:Added following code to show barcode on reportviewer --%>     
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <%--START: Added By SG: 2012.01.04: BT-1511--%>
    <div>
        <table id="tableReportOption" class="Table_WithBorder" runat="server" style="width: 100%;"
            visible="false" cellpadding="0" cellspacing="0">
            <tr>
                <td class="Td_ButtonContainer" width="200px">
                    Records Selection Option:
                </td>
                <td class="Td_ButtonContainer">
                    <asp:DropDownList ID="ddlReportOption" AutoPostBack="true" runat="server" CssClass="DropDown_Normal"
                        Width="100px">
                        <asp:ListItem Value="0" Text="All">All</asp:ListItem>
                        <asp:ListItem Value="1" Text="Selected">Selected</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <%--END: Added By SG: 2012.01.04: BT-1511--%>
    <div style="position: absolute; top: 0px; left: 0px;">
        <asp:Button ID="btnFirst" Style="z-index: 105; left: 384px; position: absolute; top: 16px"
            runat="server" Height="24px" Width="32px" Text="<<" Font-Bold="True" Font-Size="Small"
            CssClass="Button_Normal" BorderStyle="Inset" ToolTip="First Page" Visible="False">
        </asp:Button>
        <asp:Button ID="btnNext" Style="z-index: 104; left: 448px; position: absolute; top: 16px"
            runat="server" Height="24px" Width="32px" Text=">" Font-Bold="True" Font-Size="Small"
            CssClass="Button_Normal" BorderStyle="Inset" ToolTip="Next Page" Visible="False">
        </asp:Button>
        <asp:Button ID="btnPrevious" Style="z-index: 103; left: 416px; position: absolute;
            top: 16px" runat="server" Height="24px" Width="32px" Text="<" Font-Bold="True"
            Font-Size="Small" CssClass="Button_Normal" BorderStyle="Inset" ToolTip="Previous page"
            Visible="False"></asp:Button>
        <asp:Button ID="btnLast" Style="z-index: 102; left: 480px; position: absolute; top: 16px"
            runat="server" Text=">>" Height="24px" Width="32px" Font-Bold="True" Font-Size="Small"
            CssClass="Button_Normal" BorderStyle="Inset" ToolTip="Last page" Visible="False">
        </asp:Button>
        <asp:Button ID="btnExport" Style="z-index: 106; left: 528px; position: absolute;
            top: 16px" runat="server" Text="Export to PDF" Font-Bold="True" CssClass="Button_Normal"
            BorderStyle="Inset" Visible="False"></asp:Button>
        <cr:CrystalReportViewer ID="CrystalReportViewer1" Style="z-index: 107; left: 0px;
            position: absolute; top: 8px" runat="server" AutoDataBind="true" Width="350px"
            Height="50px" PrintMode="ActiveX"></cr:CrystalReportViewer>
    </div>
    </form>
    <script src="../JS/YMCA_CRScript.js" type="text/javascript"></script>
</body>
</html>
