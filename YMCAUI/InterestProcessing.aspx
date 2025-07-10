<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InterestProcessing.aspx.vb" Inherits="YMCAUI.InterestProcessing" MasterPageFile="~/MasterPages/YRSMain.Master" %>

<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script lang="text/javascript">
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
                resizable: false,
                draggable: true,
                closeOnEscape: false,
                close: false,
                width: 420, height: 250,
                title: "Confirm",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
           
        }

        function showDialog(id, text, btnokvisibility) {
            $('#' + id).dialog({ modal: true });
            $('#lblMessage').text(text)
            $('#' + id).dialog("open");
            if (btnokvisibility == 'YES') {
                $("#btnYes").show();
                $("#btnNo").show();
            }
            else {
                $("#btnYes").hide();
                $("#btnNo").hide();
            }
        }

        function closeDialog(id) {
            $('#' + id).dialog('close');
        }

    </script>
</asp:Content>

<asp:Content ID="InterestProcessingContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="TWScriptManager" runat="server">
    </asp:ScriptManagerProxy>
    <div class="Div_Center">
        <table class="Table_WithBorder" style="width: 100%; height: 450px;">
            <tr>
                <td valign="top">
                    <table style="width: 100%; height: 400px; border: none;">
                        <tbody>
                            <tr>
                                <td style="vertical-align: top; width: 160px; height: 40px; text-align: left;">
                                    <asp:Label ID="LabelProcessType" runat="server" CssClass="Label_Small">Process Type</asp:Label>
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="RadioButtonProcessingOption" CssClass="RadioButton_Normal" runat="server" RepeatLayout="Flow" AutoPostBack="True">
                                        <asp:ListItem Value="0">Daily Interest</asp:ListItem>
                                        <asp:ListItem Value="1">Month End Closing</asp:ListItem>
                                    </asp:RadioButtonList></td>
                            </tr>
                            <tr valign="top">
                                <td colspan="2" style="height: 25px;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; height: 25px; text-align: left;">
                                    <asp:Label ID="LabelMonthEnd" runat="server" CssClass="Label_Small">Month-End Closing Date</asp:Label></td>
                                <td align="left">
                                    <uc1:DateUserControl onpaste="return true" ID="DateusercontrolInterestDate" runat="server"></uc1:DateUserControl>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                        </tbody>
                    </table>
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <table width="100%">
                                    <tr>
                                        <td id="prgSt" class="Label_Small" width="34%">&nbsp;</td>
                                        <td id="prgTD" width="1%">&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="Td_ButtonContainer" width="100%" colspan="2" align="right">
                                <asp:UpdatePanel ID="updatePanelInterestProcessing" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="ButtonPost" runat="server" CssClass="Button_Normal" Width="72px" Text="Post"></asp:Button>
                                        <asp:Button ID="ButtonOK" runat="server" CssClass="Button_Normal" Width="72px" Text="Close"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:PlaceHolder ID="MessagePlaceHolder" runat="server"></asp:PlaceHolder>
    <script>
        prgTD.style.width = '1%';
    </script>
    <%--    <script language="javascript">
        document.title ="YMCA YRS - InterestProcessing"
    </script>--%>
    <div id="ConfirmDialog" title="Confirmation" style="overflow: visible;">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
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
                                <asp:Button runat="server" ID="btnYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" />&nbsp;
                                <asp:Button runat="server" ID="btnNo" OnClientClick="Javascript:closeDialog('ConfirmDialog');" Text="No" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnYes" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
