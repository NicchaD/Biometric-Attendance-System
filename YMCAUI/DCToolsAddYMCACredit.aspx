<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master" CodeBehind="DCToolsAddYMCACredit.aspx.vb" Inherits="YMCAUI.DCToolsAddYMCACredit" EnableEventValidation="false" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="YRSCustomControls" %>

<asp:Content ID="contentManualEntryHead" ContentPlaceHolderID="head" runat="server">
    <title>YMCAManualCreditEntry</title>
    <script type="text/javascript" src="JS/YMCA_JScript.js"></script>
</asp:Content>

<asp:Content ID="contentYMCAManualCredit" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="Div_Center" style="width: 100%;">
        <asp:ScriptManagerProxy ID="YMCACreditScriptManager" runat="server">
        </asp:ScriptManagerProxy>
        <asp:UpdatePanel ID="uplYMCACredit" runat="server">
            <ContentTemplate>
                <table class="td_withoutborder" style="width: 100%; vertical-align: top;">
                    <tr>
                        <td id="tdApplyCredit" runat="server" class="ActiveTab" style="width: 20%;">1. Apply Credit</td>
                        <td id="tdReviewAndSubmit" runat="server" class="InActiveDisabledTab" style="width: 20%;">2. Review & Submit</td>
                        <td style="width: 60%;">&nbsp;</td>
                    </tr>
                    <tr style="height: 2px;">
                        <td colspan="3"></td>
                    </tr>
                </table>
                <table class="Table_WithBorder" style="width: 100%; height: 550px;">
                    <tr style="height: 100%;">
                        <td style="vertical-align: top;">
                            <div id="divApplyCredit" runat="server" style="width: 100%; display: block;">
                                <table style="width: 100%; height: 100%;">
                                    <%--YMCA Information section--%>
                                    <tr style="vertical-align: top;">
                                        <td style="text-align: left;" class="td_Text">
                                            <asp:Label ID="lblYMCAInfo" runat="server">YMCA Information</asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="vertical-align: top;">
                                        <td style="text-align: left;">
                                            <table style="width: 100%; height: 100%; text-align: left;" cellspacing="4" cellpadding="4">
                                                <tr style="vertical-align: top;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <asp:Label ID="lblYMCANo" runat="server" class="Label_Small">YMCA No.</asp:Label>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label ID="lblDispYMCANo" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <asp:Label ID="lblYMCAName" runat="server" class="Label_Small">YMCA Name</asp:Label>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label ID="lblDispYMCAName" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <asp:Label ID="lblCurrentCredit" runat="server" class="Label_Small">Current Credit</asp:Label>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label ID="lblDispCurrentCredit" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <%--Add Credit section--%>
                                    <tr style="vertical-align: top;">
                                        <td style="text-align: left;" class="td_Text">
                                            <asp:Label ID="lblAddCreditHeaderText" runat="server">Add Credit</asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="vertical-align: top;">
                                        <td style="text-align: left;">
                                            <table style="width: 100%; height: 100%; text-align: left;" cellspacing="4" cellpadding="4">
                                                <tr>
                                                    <td style="text-align: right; width: 100%;height:10%;" colspan="2" >
                                                        <span class="aestrik">*</span><span class="Label_Small">Required Fields</span>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;height:30%;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <label id="lblAmount" class="Label_Small">Amount</label><span class="aestrik">*</span>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="TextBox_Normal Warn" Width="100px" Style="text-align: right;" onkeypress="Javascript:return HandleAmountFiltering(this);" onchange="javascript:FormatAmtControl(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;height:30%;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <label id="lblRecievedDate" class="Label_Small">Received Date</label><span class="aestrik">*</span>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <YRSCustomControls:CalenderTextBox ID="txtReceivedDate" runat="server" Width="100px" CssClass="TextBox_Normal Warn" EnableCustomValidator="true" />
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;height:30%;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <label id="lblNotes" class="Label_Small">Notes</label><span class="aestrik">*</span>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:TextBox ID="txtNotes" runat="server" Width="500px" TextMode="MultiLine" Height="100px" CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divReviewAndSubmit" runat="server" style="width: 100%; display: none;">
                                <table style="width: 100%; height: 100%;">
                                    <tr style="vertical-align: top;">
                                        <td style="text-align: left;" class="td_Text">
                                            <asp:Label ID="lblReviewHeaderText" runat="server">Review - Added YMCA Credit Details</asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="vertical-align: top;">
                                        <td style="text-align: left;">
                                            <table style="width: 100%; height: 100%; text-align: left;" cellspacing="2" cellpadding="6">
                                                <tr style="vertical-align: top;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <asp:Label ID="lblReviewYMCANo" runat="server" class="Label_Small">YMCA No.:</asp:Label>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label ID="lblReviewDispYMCANo" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <asp:Label ID="lblReviewYMCAName" runat="server" class="Label_Small">YMCA Name:</asp:Label>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label ID="lblReviewDispYMCAName" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <asp:Label ID="lblReviewCurrentCredit" runat="server" class="Label_Small">Current Credit:</asp:Label>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label ID="lblReviewDispCurrentCredit" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <asp:Label ID="lblReviewAddedCredit" runat="server" class="Label_Small">Added Credit:</asp:Label>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label ID="lblReviewDispAddedCredit" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <asp:Label ID="lblReviewTotalCredit" runat="server" class="Label_Small">Total Credit:</asp:Label>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label ID="lblReviewDispTotalCredit" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <asp:Label ID="lblReviewCreditSrcCode" runat="server" class="Label_Small">Credit Source Code:</asp:Label>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label ID="lblReviewDispCreditSrcCode" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <asp:Label ID="lblReviewRecdDate" runat="server" class="Label_Small">Received Date:</asp:Label>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label ID="lblReviewDispRecdDate" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <asp:Label ID="lblReviewRecdAcctDate" runat="server" class="Label_Small">Received Accounting Date:</asp:Label>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label ID="lblReviewDispRecdAcctDate" runat="server" class="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td style="text-align: left; width: 20%;">
                                                        <asp:Label ID="lblReviewNotes" runat="server" class="Label_Small">Notes:</asp:Label>
                                                    </td>
                                                    <td style="text-align: left; width: 80%;">
                                                        <asp:Label ID="lblReviewDispNotes" runat="server" class="Label_Small"></asp:Label>
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
                            <asp:Button ID="btnPrevious" runat="server" class="Button_Normal" Text="Previous" Style="display: none; width: 30%;" />
                        </td>
                        <td class="Td_ButtonContainer" style="width: 33%; text-align: center;">
                            <asp:Button ID="btnNext" runat="server" class="Button_Normal" Text="Next" Style="width: 30%;" />
                            <asp:Button ID="btnSaveYMCACredit" runat="server" class="Button_Normal" Text="Submit" Style="width: 30%; display: none;" OnClientClick="javascript:return ShowDialog();" />
                        </td>
                        <td class="Td_ButtonContainer" style="width: 34%; text-align: center;">
                            <asp:Button ID="btnClose" runat="server" CssClass="Button_Normal Warn_Dirty" Text="Close" Style="width: 30%;" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
                <div id="ConfirmDialog" title="YMCA Credits" style="display: none;">
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
                                    <asp:Button ID="btnConfirmDialogYes" runat="server"  Text="Yes" cssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" OnClientClick="closeDialog('ConfirmDialog');"/>&nbsp;
                                    <input type="button" id="btnConfirmDialogNo" value="No" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        onclick="closeDialog('ConfirmDialog');" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <script type="text/javascript">
                    function BindEvents(){
                        $('#ConfirmDialog').dialog({
                            autoOpen: false,
                            draggable: true,
                            close: false,
                            modal: true,
                            width: 450, maxHeight: 420,
                            height: 260,
                            title: "YMCA Credits",
                            open: function (type, data) {
                                $(this).parent().appendTo("form");
                                $('a.ui-dialog-titlebar-close').remove();
                            }
                        });
                    }

                    function ShowDialog(id, text) {
                        var isOpen = false;
                            $('#divConfirmDialogMessage').html(text);
                            $('#ConfirmDialog').dialog("open");
                        return isOpen;
                    }

                    function closeDialog(id) {
                        $('#' + id).dialog('close');
                    }

                    function FormatAmtControl(ctl) {
                        var vMask;
                        var vDecimalAfterPeriod;
                        var ctlVal;
                        var iPeriodPos;
                        var sTemp;
                        var iMaxLen
                        var ctlVal;
                        var tempVal;
                        ctlVal = ctl.value;
                        vDecimalAfterPeriod = 2
                        iMaxLen = ctl.maxLength;

                        if (isNaN(ctlVal)) {
                        }
                        else {
                            ctlVal = ctl.value;
                            iPeriodPos = ctlVal.indexOf(".");
                            if (iPeriodPos < 0) {
                                if (ctl.value.length > (iMaxLen - 3)) {
                                    sTemp = ctl.value
                                    tempVal = sTemp.substr(0, (iMaxLen - 3)) + ".00";
                                }
                                else {
                                    if (ctlVal == "")
                                        ctlVal = "0";
                                    tempVal = ctlVal + ".00"
                                }
                            }
                            else {
                                if ((ctlVal.length - iPeriodPos - 1) == 1)
                                    tempVal = ctlVal + "0"
                                if ((ctlVal.length - iPeriodPos - 1) == 0)
                                    tempVal = ctlVal + "00"
                                if ((ctlVal.length - iPeriodPos - 1) == 2)
                                    tempVal = ctlVal;
                                if ((ctlVal.length - iPeriodPos - 1) > 2) {
                                    tempVal = ctlVal.substring(0, iPeriodPos + 3);
                                }

                            }
                            ctl.value = tempVal;
                        }
                    }

                    /*
                    This function is responsible for filtering the keys pressed and the maintain the amount format of the 
                    value in the Text box
                    */
                    function HandleAmountFiltering(ctl) {
                        var iKeyCode, objInput;
                        var iMaxLen
                        var reValidChars = /[0-9.-]/;
                        var strKey;
                        var sValue;
                        var event = window.event || arguments.callee.caller.arguments[0];
                        iMaxLen = ctl.maxLength;
                        sValue = ctl.value;
                        detectBrowser();

                        if (isIE) {
                            iKeyCode = event.keyCode;
                            objInput = event.srcElement;
                        } else {
                            iKeyCode = event.which;
                            objInput = event.target;
                        }

                        strKey = String.fromCharCode(iKeyCode);

                        if (reValidChars.test(strKey)) {
                            if (iKeyCode == 46) {
                                if (objInput.value.indexOf('.') != -1)
                                    if (isIE)
                                        event.keyCode = 0;
                                    else {
                                        if (event.which != 0 && event.which != 8)
                                            return false;
                                    }
                            }
                            else {
                                if (objInput.value.indexOf('.') == -1) {

                                    if (objInput.value.length >= (iMaxLen - 3)) {
                                        if (isIE)
                                            event.keyCode = 0;
                                        else {
                                            if (event.which != 0 && event.which != 8)
                                                return false;
                                        }

                                    }
                                }
                                if ((objInput.value.length == (iMaxLen - 3)) && (objInput.value.indexOf('.') == -1)) {
                                    objInput.value = objInput.value + '.';

                                }


                            }

                        }
                        else {
                            if (isIE)
                                event.keyCode = 0;
                            else {
                                if (event.which != 0 && event.which != 8)
                                    return false;
                            }
                        }

                    }

                    function EnableDirty() {
                        $('#HiddenFieldDirty').val('true');
                    }

                    function ClearDirty() {
                        $('#HiddenFieldDirty').val('false');
                    }
                </script>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnPrevious" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSaveYMCACredit" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnConfirmDialogYes" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>





