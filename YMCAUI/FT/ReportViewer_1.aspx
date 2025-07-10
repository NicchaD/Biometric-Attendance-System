<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportViewer_1.aspx.vb"
    Inherits="YMCAUI.ReportViewer_1" %>

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
    </style>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <%--START: Added By SG: 2012.01.04: BT-1511--%>
    <div>
        <table id="tableReportOption" class="Table_WithBorder" runat="server" width="100%"
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
    <div style="position: absolute;">
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
            runat="server" Height="24px" Width="32px" Text=">>" Font-Bold="True" Font-Size="Small"
            CssClass="Button_Normal" BorderStyle="Inset" ToolTip="Last page" Visible="False">
        </asp:Button>
        <asp:Button ID="btnExport" Style="z-index: 106; left: 528px; position: absolute;
            top: 16px" runat="server" Text="Export to PDF" Font-Bold="True" CssClass="Button_Normal"
            BorderStyle="Inset" Visible="False"></asp:Button>
        <cr:CrystalReportViewer ID="CrystalReportViewer1" Style="z-index: 107; left: 0px;
            position: absolute; top: 8px" runat="server" Height="50px" Width="350px" AutoDataBind="true"
            PrintMode="ActiveX"></cr:CrystalReportViewer>
    </div>
    </form>
    <script src="../JS/YMCA_CRScript.js" type="text/javascript"></script>
</body>
</html>
