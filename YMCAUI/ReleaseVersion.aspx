<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReleaseVersion.aspx.vb" Inherits="YMCAUI.ReleaseVersion" MasterPageFile="~/MasterPages/YRSMain.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
</asp:content>

<asp:Content ID="ReleaseVersionContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="ManageSessionScriptManager" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanelUserSession" runat="server">
        <ContentTemplate>
            <div class="Div_Center">
                <table class="Table_WithBorder" style="width:100%;height:450px">
                    <tr valign="top">
                        <td>
                            
                                <asp:GridView ID="gvReleaseVersion" runat="server" CellPadding="1" CellSpacing="0"
                                    Width="950px" AutoGenerateColumns="false" CssClass="DataGrid_Grid" EmptyDataText="No records found." AllowSorting="True" AllowPaging="True" PageSize="20">
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
                                    <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                    <FooterStyle BackColor="White" Font-Names="Tahoma" Font-Size="11px" />
                                    <Columns>
                                        <asp:BoundField HeaderStyle-Width="120px" DataField="chvReleaseVersion" HeaderText="Release Version" SortExpression="chvReleaseVersion"></asp:BoundField>
                                        <asp:BoundField HeaderStyle-Width="110px" DataField="chvReleaseServiceVersion" HeaderText="Service Version" SortExpression="chvReleaseServiceVersion">
                                        </asp:BoundField>
                                        <asp:BoundField HeaderStyle-Width="120px" DataField="chvDatabaseScriptVersion" HeaderText="DB Script Version" SortExpression="chvDatabaseScriptVersion">
                                        </asp:BoundField>
                                        <asp:BoundField HeaderStyle-Width="110px" DataField="chvReleaseParentVersionID" HeaderText="Parent Version" SortExpression="chvReleaseParentVersionID">
                                        </asp:BoundField>
                                        <asp:BoundField HeaderStyle-Width="100px" DataField="dtmPatchApplyDate" dataformatstring="{0:MM/dd/yyyy}" HeaderText="Applied Date" SortExpression="dtmPatchApplyDate"></asp:BoundField>
                                        <asp:BoundField HeaderStyle-Width="100px" dataformatstring="{0:MM/dd/yyyy}" DataField="dtmPatchReleaseDate" HeaderText="Release Date" SortExpression="dtmPatchReleaseDate"></asp:BoundField>
                                        <asp:BoundField DataField="chvDescription" HeaderText="Description" SortExpression="chvDescription"></asp:BoundField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Left" CssClass="pagination" />
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="15" FirstPageText="First" LastPageText="Last" />
                                </asp:GridView>
                            
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Td_ButtonContainer">
                            <asp:Button runat="server" ID="btnOK" Text="Close" class="Button_Normal Warn_Dirty" Width="80"/>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>