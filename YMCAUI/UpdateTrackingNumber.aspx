<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UpdateTrackingNumber.aspx.vb"
    Inherits="YMCAUI.UpdateTrackingNumber" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="UserControls/YMCA_Footer_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<html>
<head>
    <title>YMCA YRS</title>
    <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
        <script type="text/javascript" src="JS/jquery-1.5.1.min.js"></script>
        <script src="JS/jquery-ui/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
<link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
</head>

<SCRIPT language="JavaScript">
    
    function initialisedatagrid() {
            $("#DataGridRequest TR TD:nth-child(6)").each(function () { $(this).attr('style', 'text-align: right;'); });
              
    }

    $(document).ready(initialisedatagrid);

</SCRIPT>


<body>
    <form id="Form2" method="post" runat="server">
    <table class="Table_WithoutBorder" align="center" width="700">
        <tr>
            <td align="center">
                <YRSControls:YMCA_Toolbar_WebUserControl ID="YMCA_Toolbar_WebUserControl1" runat="server"
                    ShowHomeLinkButton="true" ShowLogoutLinkButton="true"></YRSControls:YMCA_Toolbar_WebUserControl>
            </td>
        </tr>
        <tr>
            <td class="Td_BackGroundColorMenu" align="left">
                <cc1:Menu ID="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False"
                    Cursor="Pointer" CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown"
                    DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup" MenuFadeDelay="2"
                    mouseovercssclass="MouseOver" EnableViewState="False">
                    <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
                </cc1:Menu>
            </td>
        </tr>
        <tr>
            <td class="Td_HeadingFormContainer" align="left">
                Update Tracking Number
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table class="Table_WithBorder" cellspacing="0" align="center" height="305" cellpadding="0"
        width="740">
        <tr>
            <td align="center">
                <table class="Table_WithOutBorder" id="Table1" height="185" cellspacing="0" cellpadding="0"
                    width="740">
                    <tbody>
                        <tr>
                            <td valign="top" align="center">
                                <div style="overflow: auto; width: 700px; border-top-style: none; border-right-style: none;
                                    border-left-style: none; height: 250px; border-bottom-style: none">
									
                                        <asp:DataGrid ID="DataGridRequest" runat="server" CssClass="DataGrid_Grid" Width="675px"
                                        AutoGenerateColumns="false" OnSortCommand="SortCommand_OnClick" AllowSorting="true"
                                        OnEditCommand="Edit_Grid" OnCancelCommand="Cancel_Grid" OnUpdateCommand="Update_Grid"
                                        DataKeyField="UniqueID" >
                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="UniqueID" HeaderText="Id" Visible="false" />
                                             <asp:BoundColumn DataField="FundNo" SortExpression="FundNo" HeaderText="Fund No."
                                                ReadOnly="true" />
                                            <asp:BoundColumn DataField="RequestDate" SortExpression="RequestDate" HeaderText="Request Date"
											 ReadOnly="true" DataFormatString="{0:MM/dd/yyyy}" />
                                            <asp:BoundColumn DataField="LastName" SortExpression="LastName" HeaderText="Last Name"
                                                ReadOnly="true" />
                                            <asp:BoundColumn DataField="FirstName" SortExpression="FirstName" HeaderText="First Name"
                                                ReadOnly="true" />
                                            <asp:BoundColumn DataField="RefundType" SortExpression="RefundType" HeaderText="Request Type"
                                                ReadOnly="true" />
                                            <asp:BoundColumn DataField="Gross Amt." SortExpression="Gross Amt." DataFormatString="{0:N}"
                                                HeaderText="Gross Amt." ReadOnly="true"  />
                                            <asp:BoundColumn DataField="TrackingNo" HeaderText="Tracking No" SortExpression="TrackingNo" />
                                            <asp:EditCommandColumn ButtonType="LinkButton" EditText="Edit" UpdateText="Update"
                                                CancelText="Cancel" ItemStyle-Wrap="false" HeaderText="Edit" HeaderStyle-Wrap="false" />
                                        </Columns>
                                        <PagerStyle Visible="true" Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid><br />
									<asp:Label ID="Label1" runat="server" Text="Label" Visible="False" style="padding-top:80px;display:block;">No records found</asp:Label>
								</div>
									
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </table>
    <table align="center" class="Table_WithOutBorder" width="745">
        <tr>
            <td class="Td_ButtonContainer" colspan =3 align="right">
                <asp:Button ID="btnOK" CssClass="Button_Normal" Text="OK" runat="server" Width="85"> </asp:Button>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="ButtonCancel" AccessKey="C" CssClass="Button_Normal" Text="Cancel"
                    runat="server" Width="80"></asp:Button>
            </td>
           
        </tr>
    </table>
    <YRSControls:YMCA_Footer_WebUserControl ID="YMCA_Footer_WebUserControl1" runat="server">
    </YRSControls:YMCA_Footer_WebUserControl>
    </TABLE>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </form>
</body>
</html>
