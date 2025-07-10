<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RegenerateMRD.aspx.vb"
    Inherits="YMCAUI.RegenerateMRD" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="DataPagerFindInfo" TagName="DataGridPager" Src="UserControls/DataGridPager.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="UserControls/YMCA_Footer_WebUserControl.ascx" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>YMCA YRS</title>
    <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet" />
    <script src="JS/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
    <script type="text/javascript">

        function ConfirmProcess() {
            if (confirm('Do you want to save the Regenerated RMD records?')) {
                return true
            }
            else
                return false
        }

        function showNotes(noteString, e) {

            $('#b').html(noteString);

            $('#a').dialog('open');
            return;

            var posx = 0;
            var posy = 0;
            e = (window.event) ? window.event : e;
            posx = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
            posy = e.clientY + document.body.scrollTop + document.documentElement.scrollTop;

            var finalString = "<strong>Details: </strong><span onclick=\"HideNotes()\" onmouseover=\"this.style.cursor='pointer';\" style=\"color: blue;padding-left: 100px\">Close</span><br /><br />"
            finalString += noteString

            document.getElementById("divNote").innerHTML = finalString;
            document.getElementById("divNote").style.visibility = "visible";
            document.getElementById("divNote").style.top = posy + "px";
            document.getElementById("divNote").style.left = (posx - 250) + "px";


        }

        function HideNotes() {

            document.getElementById("divNote").innerHTML = "";
            document.getElementById("divNote").style.visibility = "hidden";
        }
        
    </script>
    <script type="text/javascript">

        //START : Ready Function
        $(document).ready(function () {
            $('#a').dialog({
                modal: true,
                autoOpen: false,
                buttons: [{ text: "Ok", click: function ()
                { $(this).dialog("close"); }
                }],
                title: 'Disbursement Details',
                height: 200,
                width: 350
            });
        });
        //END : Ready Function

    </script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <div class="Div_Center">
        <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="740">
            <tr>
                <td>
                    <YRSControls:YMCA_Toolbar_WebUserControl ID="YMCA_Toolbar_WebUserControl" runat="server"
                        ShowLogoutLinkButton="true" ShowHomeLinkButton="true"></YRSControls:YMCA_Toolbar_WebUserControl>
                </td>
            </tr>
            <tr>
                <td class="Td_BackGroundColorMenu" align="left">
                    <cc1:Menu ID="Menu1" runat="server" mouseovercssclass="MouseOver" MenuFadeDelay="1"
                        zIndex="100" DefaultMouseUpCssClass="mouseup" DefaultMouseOverCssClass="mouseover"
                        DefaultMouseDownCssClass="mousedown" DefaultCssClass="menuitem" CssClass="menustyle"
                        HighlightTopMenu="false" Layout="Horizontal" EnableViewState="False">
                        <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
                    </cc1:Menu>
                </td>
            </tr>
            <tr>
                <td class="Td_HeadingFormContainer" align="left">
                    <%--<img title="image" height="10" alt="image" src="images/spacer.gif" width="10" />
                    <asp:Label ID="LabelModuleName" CssClass="Td_HeadingFormContainer" runat="server">Find Information</asp:Label>--%>
                    <YRSControls:YMCA_Header_WebUserControl ID="YMCA_Header_WebUserControl1" runat="server">
                    </YRSControls:YMCA_Header_WebUserControl>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div class="Div_Center">
        <table class="Table_WithBorder" width="740">
            <tr>
                <td align="center">
                    <table width="730" class="Table_WithoutBorder" style="height: 360px">
                        <tr valign="top">
                            <td align="right" style="height: 10px; vertical-align: top; position: inherit">
                                <table class="Table_WithOutBorder" id="Table1" cellspacing="1" cellpadding="1" border="0"
                                    width="100%">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="LabelRegenYear" CssClass="Label_Small" runat="server" Text="Select the Year :"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 220px">
                                            <asp:DropDownList ID="DropDownListRegenMRDYear" runat="server" CssClass="DropDown_Normal"
                                                Width="80px">
                                            </asp:DropDownList>
                                            &nbsp;
                                            <asp:Button ID="ButtonRegenMRD" runat="server" Text="Regenerate RMD" Width="120px"
                                                CssClass="Button_Normal" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblErrorMessage" CssClass="Error_Message" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="width: 100%" valign="top">
                                <table class="Table_WithoutBorder" width="100%">
                                    <tr class="Td_InLineBorder">
                                        <td class="Text_Normal" align="left" style="width: 100%">
                                            Generated RMD
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100%">
                                            <%--<asp:Label ID="LabelNoRecords" runat="server" Visible="false" CssClass="Label_Medium"
                                                Text="No RMD records found for this participant."></asp:Label>--%>
                                            <div style="border-style: none; overflow: auto; width: 100%; height: 140px; position: static">
                                                <asp:GridView ID="GridViewGeneratedMRD" runat="server" CssClass="DataGrid_Grid" Width="99%"
                                                    AllowSorting="True" AutoGenerateColumns="false">
                                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Year" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblShowNotes" onmouseover="this.style.cursor='pointer'" ForeColor="Blue"
                                                                    runat="server" Text='<%#Eval("MRDYear")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PlanType" HeaderText="Plan Type" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="CurrentBalance" HeaderText="Previous Year End Balance"
                                                            ItemStyle-Width="30%" />
                                                        <asp:BoundField DataField="MRDAmount" HeaderText="RMD Amount" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="PaidAmount" HeaderText="Paid Amount" ItemStyle-Width="20%" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trRegen" runat="server" style="display: none" class="Td_InLineBorder">
                                        <td class="Text_Normal" align="left" style="width: 100%">
                                            Regenerated RMD
                                        </td>
                                    </tr>
                                    <tr id="trRegenData" runat="server" style="display: none">
                                        <td align="center">
                                            <div style="border-style: none; overflow: auto; width: 100%; height: 140px; position: static">
                                                <asp:GridView ID="GridViewRegenMRD" runat="server" CssClass="DataGrid_Grid" Width="99%"
                                                    AllowSorting="True" AutoGenerateColumns="false">
                                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="chvYear" HeaderText="Year" ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="chvPlanType" HeaderText="Plan Type" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="mnyTotalAmount" HeaderText="Computed Balance" ItemStyle-Width="30%" />
                                                        <asp:BoundField DataField="mnyMRDAmount" HeaderText="RMD Amount" ItemStyle-Width="20%" />
                                                        <asp:BoundField DataField="unSatisfiedAmount" HeaderText="Unsatisfied Amount" ItemStyle-Width="20%" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table class="Table_WithBorder" width="740">
                        <tr>
                            <td align="center" class="Td_ButtonContainer" style="width: 50%">
                                <asp:Button ID="ButtonConfirm" runat="server" Text="Save" Width="80px" CssClass="Button_Normal"
                                    Visible="False" />
                            </td>
                            <td align="center" class="Td_ButtonContainer" style="width: 50%">
                                <asp:Button ID="ButtonCancel" Visible="false" runat="server" Text="Cancel" Width="80px"
                                    CssClass="Button_Normal" />
                                <asp:Button ID="ButtonOk" runat="server" Text="OK" Width="80px" CssClass="Button_Normal" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
       <%-- SR | 2016.05.26 | YRS-AT-1573 : Align YMCA Footer --%>
       <%-- <YRSControls:YMCA_Footer_WebUserControl ID="YMCA_Footer_WebUserControl1" runat="server">
        </YRSControls:YMCA_Footer_WebUserControl>--%>
        <table  width="740px">
            <tr>
                <td>
                    <YRSControls:YMCA_Footer_WebUserControl ID="YMCA_Footer_WebUserControl1" runat="server">
                    </YRSControls:YMCA_Footer_WebUserControl>
                </td>
            </tr>       
        </table>
       <%-- SR | 2016.05.26 | YRS-AT-1573 : Align YMCA Footer --%> 

    </div>
    <div id="a">
        <div id="b">
        </div>
    </div>
    <asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder>
    </form>
</body>
</html>
