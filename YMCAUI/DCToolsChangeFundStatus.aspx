<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master" CodeBehind="DCToolsChangeFundStatus.aspx.vb" Inherits="YMCAUI.DCToolsChangeFundStatus" EnableEventValidation="false" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="YRSCustomControls" %>

<asp:Content ID="contentManualEntryHead" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="JS/YMCA_JScript.js"></script>
</asp:Content>

<asp:Content ID="contentYMCAManualCredit" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="Div_Center" style="width: 100%;">
        <asp:UpdatePanel ID="uplChangeFundStatus" runat="server">
            <ContentTemplate>
                <table class="td_withoutborder" style="width: 100%; vertical-align: top;">
                    <tr>
                        <td id="tdChangeFundStatus" runat="server" class="ActiveTab" style="width: 20%;">1. Change Fund Status</td>
                        <td id="tdReviewAndSubmit" runat="server" class="InActiveDisabledTab" style="width: 20%;">2. Review & Submit</td>
                        <td style="width: 60%;" Align="right">
                            <div id="divRequiredFields" runat ="server">                                               
                                <span class="aestrik">*</span><span class="Label_Small">Required Fields</span>                                                                                                         
                           </div>
                       </td> 
                    </tr>
                    <tr style="height: 2px;">
                        <td colspan="3"></td>
                    </tr>
                </table>
                <table class="Table_WithBorder" style="width: 100%; height: 550px;">
                    <tr style="height: 100%;">
                        <td style="vertical-align: top;">
                            <div id="divChangeFundStatus" runat="server" style="width: 100%; display: block;">
                                <table width="100%" class="Table_WithoutBorder" cellspacing="0" border="0">
                                    <tr style="vertical-align: top;">
                                        <td style="text-align: left;" class="td_Text">Change Fund Status
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr style="vertical-align: top;">
                                        <td style="text-align: left;">
                                            <table style="width: 100%; height: 100%; text-align: left;" cellspacing="4" cellpadding="4" border="0">
                                                <tr style="vertical-align: top; height: 40px;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <span class="Label_Small">Existing Fund Status</span>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label ID="lblExistingFundStatus" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top; height: 40px;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <span class="Label_Small">New Fund Status</span><span class="aestrik" style="vertical-align :top">*</span> 
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:DropDownList ID="ddlNewFundStatus" runat="server" DataValueField="FundStatusType" DataTextField="DisplayFundStatus">
                                                            <asp:ListItem Value="-1" Text="-- Select --"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top; height: 40px;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <span class="Label_Small">Balance</span>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label ID="lblCurrentBalance" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top; height: 40px;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <span id="Span1" runat="server" class="Label_Small">Notes</span><span class="aestrik" style="vertical-align :top">*</span> 
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:TextBox runat="server" ID="txtNotes" TextMode="MultiLine" Rows="5" Style="width: 400px" CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                </table>
                            </div>
                            <div id="divReviewAndSubmit" runat="server" style="width: 100%; display: none;">
                                <table width="100%" class="Table_WithoutBorder" cellspacing="0" border="0">
                                    <tr style="vertical-align: top;">
                                        <td style="text-align: left;" class="td_Text">Review - Change Fund Status
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr style="vertical-align: top;">
                                        <td style="text-align: left;">
                                            <table style="width: 100%; height: 100%; text-align: left;" cellspacing="4" cellpadding="4" border="0">
                                                <tr style="vertical-align: top; height: 40px;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <span class="Label_Small">Existing Fund Status</span>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label id="lblReviewExistingFundStatus" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top; height: 40px;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <span class="Label_Small">New Fund Status</span>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label id="lblReviewNewFundStatus" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top; height: 40px;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <span class="Label_Small">Notes</span>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label id="lblReviewNotes" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; text-align: center;">
                    <tr>
                        <td class="Td_ButtonContainer" style="width: 33%">
                            <asp:Button ID="btnPrevious" runat="server" class="Button_Normal" Text="Previous" Style="display: none; width: 30%;" OnClick="btnPrevious_Click" />
                        </td>
                        <td class="Td_ButtonContainer" style="width: 33%; text-align: center;">
                            <asp:Button ID="btnNext" runat="server" class="Button_Normal" Text="Next" Style="width: 30%;" />
                            <asp:Button ID="btnSaveFundStatusChanges" runat="server" class="Button_Normal" Text="Submit" Style="width: 30%; display: none;" OnClientClick="javscript: return ShowDialog();"/>
                        </td>
                        <td class="Td_ButtonContainer" style="width: 34%; text-align: center;">
                            <asp:Button ID="btnClose" runat="server" class="Button_Normal Warn_Dirty" Text="Close" Style="width: 30%;" />
                        </td>
                    </tr>
                </table>
                <div id="ConfirmDialog" title="Fund Status" style="display: none;">
                    <div>
                        <table width="100%" height="250px" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;">
                            <tr>
                                <td rowspan="2" style="width: 10%;">
                                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="imgConfirmDialogInfo" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divConfirmDialogMessage">
                                        Are you sure, you want to submit?
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                </td>
                            </tr>
                            <tr id="trConfirmDialogYesNo">
                                <td align="right" valign="bottom" colspan="2">
                                    <asp:Button runat="server" ID="btnConfirmDialogYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                         OnClientClick="closeDialog('ConfirmDialog');" CausesValidation="False" />&nbsp; <%--// PK | 03-13-2019 | YRS-AT-4248 | Added OnClientClick property--%>
                                    <input type="button" id="btnConfirmDialogNo" value="No" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        onclick="closeDialog('ConfirmDialog');" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <script type="text/javascript">
                    function BindEvents() {
                        $('#ConfirmDialog').dialog({
                            autoOpen: false,
                            draggable: true,
                            close: false,
                            modal: true,
                            width: 450, maxHeight: 420,
                            height: 260,
                            title: "Change Fund Status",
                            open: function (type, data) {
                                $(this).parent().appendTo("form");
                                $('a.ui-dialog-titlebar-close').remove();
                            }
                        });
                    }

                    function ShowDialog() {
                        $('#ConfirmDialog').dialog("open");
                        return false;
                    }

                    function closeDialog(id) {
                        $('#' + id).dialog('close');
                    }
                </script>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnNext" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnPrevious" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnConfirmDialogYes" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>