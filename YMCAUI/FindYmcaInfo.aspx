<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master"
    CodeBehind="FindYmcaInfo.aspx.vb" Inherits="YMCAUI.FindYmcaInfo" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        function _OnBlur_TextBox() {
            document.Form1.all.btnFind.focus();
        }
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    $('#<%=txtYmcaNo.ClientID%>').focus();
                }
            }
            $('#<%=txtYmcaNo.ClientID%>').focus();
        });

        function BeginSearch()
        {
            if (event.which || event.keyCode) {
                if ((event.which == 13) || (event.keyCode == 13)) {
                    document.getElementById('<%=btnFind.ClientID%>').click();
                    return false;
                }
            } else {
                return true;
            }
        }

        function ClearAllFields() {
            $('#<%=txtYmcaNo.ClientID%>').val('');
            $('#<%=txtName.ClientID%>').val('');
            $('#<%=txtCity.ClientID%>').val('');
            $('#<%=txtState.ClientID%>').val('');
            return false;
        }

        function FormatYMCANo() {
            var str = $('#<%=txtYmcaNo.ClientID%>').val(); 
            var len;
            len = str.length;
            if (len == 0) {
                return false;
            }
            else if (len < 6) {
                diff = (6 - len);
                for (i = 0; i < diff; i++) {
                    str = "0" + str

                }
            }
            $('#<%=txtYmcaNo.ClientID%>').val(str);
        }
    </script>
    <script src="JS/YMCA_JScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="dbScriptManagerProxy" runat="server">
    </asp:ScriptManagerProxy>
    <div class="Div_Center" style="width: 100%; height: 550">
        <table class="Table_WithBorder" width="100%" style="height: 550px">
            <tr style="height: 100%;">
                <td align="center">
                    <asp:UpdatePanel ID="upRetireeGrid" runat="server">
                        <ContentTemplate>
                            <table width="100%" style="height: 520px">
                                <tr>
                                    <td align="left" width="60%" valign="top">
                                        <asp:Label ID="LabelNoDataFound" runat="server" Visible="False" CssClass="Label_Small">No Matching Records</asp:Label>
                                        <div style="overflow: auto; width: 100%; height: 100%; border-top-style: none; border-right-style: none; border-left-style: none; position: static; border-bottom-style: none">
                                            <asp:GridView ID="gvFindYmcaInfo" AllowPaging="true" AllowSorting="true" PageSize="25"
                                                runat="server" CssClass="DataGrid_Grid" Width="100%">
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                                <SortedAscendingHeaderStyle CssClass="sortasc" />
                                                <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                                <Columns>
                                                    <asp:ButtonField ButtonType="Image" CommandName="Select" Text="Select" ImageUrl="images\select.gif"
                                                        CausesValidation="false" />
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="15" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="pagination" />
                                            </asp:GridView>

                                        </div>
                                    </td>
                                    <td align="right" valign="top" width="35%">
                                        <table class="Table_WithOutBorder" width="100%" align="right">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblYmcaNo" CssClass="Label_Small" Width="100" runat="server">Ymca No.</asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtYmcaNo" runat="server" CssClass="TextBox_Normal" Width="180"
                                                        MaxLength="10" onkeydown="javascript: return BeginSearch()"
                                                        onkeypress="javascript:ValidateNumeric();" onblur="javascript:FormatYMCANo();"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblName" CssClass="Label_Small" Width="100" runat="server">Name</asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="TextBox_Normal" Width="180"
                                                        MaxLength="30" onkeydown="javascript: return BeginSearch()"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblCity" runat="server" CssClass="Label_Small" Width="100">City</asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtCity" runat="server" CssClass="TextBox_Normal" Width="180"
                                                        MaxLength="29" onkeydown="javascript: return BeginSearch()"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblState" runat="server" CssClass="Label_Small" Width="100">State</asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtState" runat="server" CssClass="TextBox_Normal" Width="180"
                                                        MaxLength="29" onkeydown="javascript: return BeginSearch()"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Button ID="btnFind" runat="server" CssClass="Button_Normal" Width="80" Text="Find" OnClientClick="javascript:FormatYMCANo();"></asp:Button>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnClear" runat="server" CssClass="Button_Normal" Width="80" Text="Clear" OnClientClick="javascript: return ClearAllFields();"></asp:Button>
                                                </td>
                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <table width="100%" align="center">
                        <tr>
                            <td class="Td_ButtonContainer" align="right" colspan="3">
                                <asp:Button ID="btnClose" runat="server" CssClass="Button_Normal" Width="80"
                                    Text="Close"></asp:Button>&nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
