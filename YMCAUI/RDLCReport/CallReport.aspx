<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CallReport.aspx.vb" Inherits="YMCAUI.CallReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <rsweb:ReportViewer ID="RptViewRMD" runat="server" Font-Names="Verdana"
                    BorderColor="Blue" BackColor="AliceBlue" Width="100%" Height="600px" Font-Size="8pt"
                    InteractiveDeviceInfos="(Collection)" PageCountMode="Actual" EnableTheming="true"
                    ZoomMode="PageWidth" ViewStateMode="Enabled" SizeToReportContent="true" AsyncRendering="false"
                    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                    <%--<LocalReport ReportPath="RDLCReport\Report1.rdlc">
                    </LocalReport>--%>
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
