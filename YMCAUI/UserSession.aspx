<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserSession.aspx.vb" Inherits="YMCAUI.UserSession" MasterPageFile="~/MasterPages/YRSMain.Master"%>

<asp:Content ID="UserSessionContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="ManageSessionScriptManager" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanelUserSession" runat="server">
        <ContentTemplate>
            <div class="Div_Center">
                <table class="Table_WithBorder" width="100%" height="400px">
                    <tr>
                        <td colspan="3" class="Label_Small" valign="top">
                            <asp:Label runat="server" ID="lblMsg" Text="Please select username to view login history information for user. "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="Label_Small" width="9%" valign="top">
                            Username:
                        </td>
                        <td width="30%" align="left">
                            <asp:DropDownList ID="ddlUsername" runat="server" CssClass="DropDown_Normal Warn"
                                Width="80%">
                            </asp:DropDownList>                            
                        </td>
                        <td align="left" valign="top">
                            <asp:Button ID="btnSearchSession" runat="server" Text="GO" CssClass="Button_Normal" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="3" align="center">
                            <div style="overflow: auto; width: 98%; height: 390px">
                                <asp:GridView ID="gvUserSession" runat="server" CellPadding="1" CellSpacing="0" Width="100%"
                                    AutoGenerateColumns="false" CssClass="DataGrid_Grid" EmptyDataText="No records found."
                                    AllowSorting="True" AllowPaging="True" PageSize="20">
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
                                    <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
                                    <FooterStyle BackColor="White" Font-Names="Tahoma" Font-Size="11px" />
                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundField DataField="HostName" HeaderText="Host Name" SortExpression="HostName">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="chvIpAddress" HeaderText="Host Address" SortExpression="chvIpAddress">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="dtmLoggedOn" HeaderText="Logged In" SortExpression="dtmLoggedOn">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="dtmLoggedOut" HeaderText="Logged Out" SortExpression="dtmLoggedOut">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="chvSessionStatus" HeaderText="Status" SortExpression="chvSessionStatus">
                                        </asp:BoundField>
                                        <%--<asp:BoundField DataField="KillSession" HeaderText="Kill Session"></asp:BoundField>--%>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Left" CssClass="pagination" />
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="15" FirstPageText="First" LastPageText="Last" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="Td_ButtonContainer" align="right" colspan="3">
                            <asp:Button runat="server" Text="Back" ID="btnBack" CssClass="Button_Normal" Width="110px" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>