<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdminSessionManagement.aspx.vb" Inherits="YMCAUI.AdminSessionManagement" MasterPageFile="~/MasterPages/YRSMain.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script lang="text/javaScript">
        function HandleOnClose() {
            var browWidth = document.body.offsetWidth
            browWidth = browWidth - 30
            //alert("hello")
            if (event.clientX > browWidth && event.clientY < -80) {
                window.open("CrossButtonForSession.aspx")
            }
        }

        /* Shashi Shekhar:2009-12-31: comment and shift  CheckAccess(controlname) in common external js file(JS/YMCA_JScript.js). Please check older version from SVN if needed. */

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    BindEvents();
                }
            }
            BindEvents();
        });

        function BindEvents() {
            var allCheckBoxSelector = '#<%=gvSession.ClientID%> input[id*="chkSelectAll"]:checkbox';
            var checkBoxSelector = '#<%=gvSession.ClientID%> .Flag input[id*="chkFlag"]:checkbox';
            $(allCheckBoxSelector).bind('click', function () {
                $(checkBoxSelector).attr('checked', $(this).is(':checked'));
                ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector);
            });
            ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector);


        }

        function ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector) {
            var totalCheckboxes = $(checkBoxSelector),
                        checkedCheckboxes = totalCheckboxes.filter(":checked"),
                        noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
                        allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);
            if (totalCheckboxes.length == 0) allCheckboxesAreChecked = false;
            $(allCheckBoxSelector).attr('checked', allCheckboxesAreChecked);
        }
    </script>
</asp:Content>
<asp:Content ID="ManageSessionContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="ManageSessionScriptManager" runat="server">
    </asp:ScriptManagerProxy>
    
    <div class="Div_Center">
        <asp:UpdatePanel ID="UpdatePanelManageSession" runat="server">
            <ContentTemplate>
                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>  <!-- Shilpa N | 02/22/2019 | YRS-AT-4248 | Added Placeholder to solve runtime error to show message box. -->
                <table class="Table_WithBorder" width="100%" align="center">
                    <tr>
                        <td>
                            <div style="overflow: auto; width: 100%; border-top-style: none; border-right-style: none; border-left-style: none; height: 400px; border-bottom-style: none">
                                <asp:GridView ID="gvSession" runat="server" Width="950px" CssClass="DataGrid_Grid"
                                    AutoGenerateColumns="False" EmptyDataText="No records found." AllowSorting="True" AllowPaging="True" PageSize="15">
                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
                                    <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                    <FooterStyle BackColor="White" Font-Names="Tahoma" Font-Size="11px" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="20px">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkFlag" CssClass="Flag" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="20px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="chvSessionId" HeaderText="Session Id" SortExpression="chvSessionId"></asp:BoundField>
                                        <asp:BoundField DataField="intUserID" HeaderText="User Id" SortExpression="intUserID"></asp:BoundField>
                                        <asp:BoundField DataField="chvUserName" HeaderText="User Name" SortExpression="chvUserName"></asp:BoundField>
                                        <asp:BoundField DataField="HostName" HeaderText="Host Name" SortExpression="HostName"></asp:BoundField>
                                        <asp:BoundField DataField="chvIpAddress" HeaderText="Host Address" SortExpression="chvIpAddress"></asp:BoundField>
                                        <%--<asp:BoundField DataField="chvSessionStatus" HeaderText="Status"></asp:BoundColumn>--%>
                                        <asp:BoundField DataField="dtmLoggedOn" HeaderText="Logged In" SortExpression="dtmLoggedOn"></asp:BoundField>
                                        <%--<asp:BoundField DataField="KillSession" HeaderText="Kill Session"></asp:BoundColumn>--%>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="15"  FirstPageText="First" LastPageText="Last" />
                                    <PagerStyle CssClass="pagination" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="Td_ButtonContainer" align="center">
                            <%--<asp:button id="ButtonSelectAll" runat="server" cssclass="Button_Normal" text="Select All" width="110px"></asp:button>--%>
                            &nbsp;<asp:Button ID="ButtonShow" CssClass="Button_Normal" runat="server" Text="Active Sessions" Width="110px"></asp:Button>
                            &nbsp;<asp:Button ID="ButtonSubmit" CssClass="Button_Normal" runat="server" Text="Kill Session" Width="110px"></asp:Button>
                            &nbsp;<asp:Button ID="ButtonNewSessionKill" runat="server" CssClass="Button_Normal" Text="Prevent Login" Width="110px"></asp:Button>
                            &nbsp;<asp:Button ID="ButtonSendMail" runat="server" CssClass="Button_Normal" Text="Notify Mail" Width="110px"></asp:Button>
                            &nbsp;<asp:Button runat="server" Text="History..." ID="btnHistory" CssClass="Button_Normal" Width="110px" />
                            &nbsp;<asp:Button ID="ButtonOK" runat="server" CssClass="Button_Normal" Text="Close" Width="110px" CausesValidation="False"></asp:Button>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <input id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server">
</asp:Content>
