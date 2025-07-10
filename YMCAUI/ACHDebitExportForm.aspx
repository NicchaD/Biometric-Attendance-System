<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ACHDebitExportForm.aspx.vb"
    Inherits="YMCAUI.ACHDebitExportForm" MasterPageFile="~/MasterPages/YRSMain.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
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
            $('#ConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                width: 450, height: 310,
                title: "ACH Debit",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        }

        function showDialog(id, text) {

            $('#' + id).dialog({ modal: true });
            $('#lblMessage').text(text);
            $('#' + id).dialog("open");
            $("#btnYes").show();
            $("#btnNo").show();
        }

        function closeDialog(id) {

            $('#' + id).dialog('close');
        }
    
    </script>
</asp:Content>
<asp:Content ID="ACHDebitExportContentMain" ContentPlaceHolderID="MainContentPlaceHolder"
    runat="server">
    <asp:ScriptManagerProxy ID="Achdebitscriptmanger" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanelAchDebit" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="Div_Center">
                <table class="Td_ButtonContainer" width="100%">
                    <tr valign="top">
                        <td align="right">
                            <asp:Button class="Button_Normal" ID="ButtonDelete" runat="server" Text="Delete..."
                                Width="100"></asp:Button>&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </div>
            <div class="Div_Center">
                <table class="Table_WithBorder" width="100%" border="0">
                    <tr>
                        <td align="center" colspan="4">
                            <div style="overflow: auto; width: 100%; height: 300px; text-align: left">
                                <asp:DataGrid ID="DatagridACHDebitExport" CssClass="DataGrid_Grid" runat="server"
                                    Width="100%" AllowSorting="true" AutoGenerateColumns="False">
                                    <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                    <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                    <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn ItemStyle-Width="25px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBoxSelect" runat="server" Checked='<%# Databinder.Eval(Container.DataItem, "Selected") %>'
                                                    CssClass="CheckBox_Normal"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgeditdetails" runat="server" ImageUrl="~/images/edits.gif"
                                                    CommandName="Edit" ToolTip="Update" AlternateText="Update" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="YMCANo" SortExpression="YMCANo" HeaderText="YMCA No" ItemStyle-Width="55px">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="YMCAName" HeaderText="YMCA Name" SortExpression="YMCAName">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="uniqueid" HeaderText="UniqueId" Visible="False"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="TransmittalNo" HeaderText="TransmittalNo" ItemStyle-Width="105px"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Amount" HeaderText="Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" SortExpression="Amount">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PaymentDate" SortExpression="PaymentDate" HeaderText="Payment Date"
                                            DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="95px"></asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" width="5%">
                            <asp:Label ID="LabelCount" runat="server" CssClass="Label_Small">Count</asp:Label>
                        </td>
                        <td align="left" width="64%">
                            <asp:TextBox ID="TextBoxCount" runat="server" CssClass="TextBox_Normal" ReadOnly="true"
                                Style="text-align: right;"></asp:TextBox>
                        </td>
                        <td align="left" width="5%">
                            <asp:Label ID="LabelTotal" runat="server" CssClass="Label_Small">Total</asp:Label>
                        </td>
                        <td align="left" width="29%">
                            <asp:TextBox ID="TextBoxTotal" runat="server" CssClass="TextBox_Normal" ReadOnly="true"
                                Style="text-align: right;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table class="Td_ButtonContainer" width="100%">
                    <tr valign="top">
                        <td align="center" width="30%">
                            <asp:Button class="Button_Normal" ID="ButtonSelectAll" runat="server" Text="Select All"
                                Width="90" CausesValidation="true" Enabled="true"></asp:Button>
                        </td>
                        <td align="center" width="15%">
                            <asp:Button class="Button_Normal" ID="ButtonPendingReport" runat="server" Text="Pending Report"
                                Width="110" CausesValidation="False"></asp:Button>
                        </td>
                        <td align="center" width="15%">
                            <asp:Button class="Button_Normal" ID="ButtonExport" runat="server" Text="Export"
                                Width="80" Enabled="True"></asp:Button>
                        </td>
                        <td align="center" width="15%">
                            <asp:Button class="Button_Normal" ID="ButtonReCalculate" runat="server" Text="Re-Calculate"
                                Width="95"></asp:Button>
                        </td>
                        <td align="center" width="15%">
                            <asp:Button class="Button_Normal" ID="ButtonCancel" runat="server" Text="Close" Width="80"
                                CausesValidation="False"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonDelete" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ButtonReCalculate" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ButtonExport" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ButtonPendingReport" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSelectAll" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="DatagridACHDebitExport" EventName="EditCommand" />
            <asp:AsyncPostBackTrigger ControlID="btnNo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnYes" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    &nbsp;
    <asp:DataGrid ID="DataGrid1" runat="server">
    </asp:DataGrid>
    <div id="ConfirmDialog" title="Ach Debit">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div>
                    <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage" CssClass="Label_Small" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom">
                                <asp:Button runat="server" ID="btnYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px;
                                    color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                    height: 16pt;" />&nbsp;
                                <asp:Button runat="server" ID="btnNo" Text="No" CssClass="Button_Normal" Style="width: 80px;
                                    color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                    height: 16pt;" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonDelete" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ButtonReCalculate" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ButtonExport" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ButtonPendingReport" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ButtonSelectAll" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="DatagridACHDebitExport" EventName="EditCommand" />
                <asp:AsyncPostBackTrigger ControlID="btnNo" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnYes" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
