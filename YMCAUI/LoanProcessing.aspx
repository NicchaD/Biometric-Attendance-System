<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoanProcessing.aspx.vb" Inherits="YMCAUI.LoanProcessing" MasterPageFile="~/MasterPages/YRSPopUp.Master" %>

<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl.ascx" %>
<asp:Content runat="server" ID="Head" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        var theform;
        var isIE;
        var isNS;

        function detectBrowser() {
            if (window.navigator.appName.toLowerCase().indexOf("netscape") > -1)
                theform = document.forms["Form1"];
            else
                theform = document.Form1;

            //browser detection
            var strUserAgent = navigator.userAgent.toLowerCase();
            isIE = strUserAgent.indexOf("msie") > -1;
            isNS = strUserAgent.indexOf("netscape") > -1;

        }
        function ValidateNumeric(ctrl) {

            var iKeyCode, objInput;
            var iMaxLen
            var reValidChars = /[0-9.]/;
            var strKey;
            var sValue;
            var event = window.event || arguments.callee.caller.arguments[0];
            var ctl = ctrl;
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
                // clear the control as this is not a num
                //ctl.value=""
            }
            else {
                ctlVal = ctl.value;
                iPeriodPos = ctlVal.indexOf(".");
                if (iPeriodPos < 0) {
                    if (ctl.value.length > (iMaxLen - 3)) {
                        sTemp = ctl.value
                        tempVal = sTemp.substr(0, (iMaxLen - 3)) + ".00";
                    }
                    else
                        tempVal = ctlVal + ".00"
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
    </script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Maincontent">

    <asp:ScriptManagerProxy ID="dbScriptManagerProxy" runat="server">
    </asp:ScriptManagerProxy>
    <table class="Table_WithBorder" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <table width="100%">
                    <tr style="vertical-align: top">
                        <td class="Table_WithBorder">
                            <table>
                                <tr>
                                    <td></td>
                                    <td align="left">
                                        <asp:Label ID="LabelYes" CssClass="Label_Small" runat="server">Yes</asp:Label>&nbsp;<asp:Label CssClass="Label_Small" ID="LabelNo" runat="server">No</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="250">
                                        <asp:Label ID="LabelLoanRequest" CssClass="Label_Small" runat="server">Has the Loan Request been signed?</asp:Label></td>
                                    <td align="left">
                                        <asp:RadioButton ID="RadioButtonLoanRequestYes" runat="server" Text=" " AutoPostBack="true" GroupName="GrpLoanRequest" CssClass="Label_Small"></asp:RadioButton><asp:RadioButton ID="RadioButtonLoanRequestNo" runat="server" Text=" " AutoPostBack="true" GroupName="GrpLoanRequest"
                                            Checked="true"></asp:RadioButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LabelAddress" CssClass="Label_Small" runat="server">Does the address need updating?</asp:Label></td>
                                    <td align="left">
                                        <asp:RadioButton ID="RadioButtonAddressYes" runat="server" CssClass="Label_Small" Text=" " AutoPostBack="true" GroupName="GrpAddress"></asp:RadioButton><asp:RadioButton ID="RadioButtonAddressNo" runat="server" Text=" " AutoPostBack="true" GroupName="GrpAddress"
                                            Checked="true"></asp:RadioButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td class="Table_WithBorder">
                            <table style="width: 100%">
                                <tr>
                                    <td colspan="2" style="text-align: right; width: 100%">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="ButtonAddress" runat="server" CssClass="Button_Normal" Text="Address" Width="80"
                                                    Enabled="False"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; vertical-align: top; width: 100px">
                                        <label class="Label_Small">Address </label>
                                    </td>
                                    <td>
                                        <NewYRSControls:New_AddressWebUserControl runat="server" PopupHeight="930" ID="AddressWebUserControl1" EnableControls="False" AllowNote="true" AllowEffDate="false" ClientIDMode="Predictable" />
                                    </td>
                                </tr>
                            </table>



                        </td>
                    </tr>

                    <!--YRS 5.0-511-->
                    <tr style='DISPLAY: none'>
                        <td>
                            <asp:Label ID="LabelSpousalWaiver" CssClass="Label_Small" runat="server">Has the spouse signed a waiver?</asp:Label></td>
                        <td align="left">
                            <asp:RadioButton ID="RadioButtonSpousalWaiverYes" runat="server" Text=" " AutoPostBack="true" GroupName="GrpSpousal"></asp:RadioButton><asp:RadioButton ID="RadioButtonSpousalWaiverNo" runat="server" Text=" " AutoPostBack="true" GroupName="GrpSpousal"
                                Checked="true"></asp:RadioButton></td>

                    </tr>
                    <%-- START: MMR | 2018.05.30 | YRS-AT-3936 | Commented existing code to make proper alignment of new section added for payment details--%>
                    <%--<tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2" class="td_Text_Small" height="2"></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>--%>
                    <%-- END: MMR | 2018.05.30 | YRS-AT-3936 | Commented existing code to make proper alignment of new section added for payment details--%>
                    <%-- START: MMR | 2018.05.30 | YRS-AT-3936 | Added new section to display loan payment details--%>
                    <tr>
                        <td class="Table_WithBorder" colspan="2">
                            <table width="100%">
                                <tr>
                                    <td align="left" width="25%">
                                        <asp:Label ID ="lblPaymentMethod" CssClass ="Label_Small" runat="server">Payment Method</asp:Label>
                                    </td>
                                    <td align="left" width="75%">
                                        <asp:TextBox ID ="txtPaymentMethod" CssClass ="TextBox_Normal" Width="120px" ReadOnly="true" runat="server" style="text-align:left;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trBankName" runat="server">
                                    <td align="left" width="25%">
                                        <asp:Label ID ="lblBankName" CssClass ="Label_Small" runat="server">Bank Name</asp:Label>
                                    </td>
                                    <td align="left" width="75%">
                                        <asp:TextBox ID ="txtBankName" CssClass ="TextBox_Normal" Width="140px" ReadOnly="true" runat="server" style="text-align:left;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trAccountType" runat="server">
                                    <td align="left">
                                        <asp:Label ID ="lblAccountType" CssClass ="Label_Small" runat="server">Account Type</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID ="txtAccountType" CssClass ="TextBox_Normal" Width="120px" ReadOnly="true" runat="server" style="text-align:left;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trBankABA" runat="server">
                                    <td align="left">
                                        <asp:Label ID ="lblBankABA" CssClass ="Label_Small" runat="server">Bank ABA #</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID ="txtBankABA" CssClass ="TextBox_Normal" Width="120px" ReadOnly="true" runat="server" style="text-align:right;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trBankAccountNumber" runat="server">
                                    <td align="left">
                                        <asp:Label ID ="lblBankAccountNumber" CssClass ="Label_Small" runat="server">Account #</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID ="txtBankAccountNumber" CssClass ="TextBox_Normal" Width="120px" ReadOnly="true" runat="server" style="text-align:right;"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <%-- END: MMR | 2018.05.30 | YRS-AT-3936 | Added new section to display loan payment details--%>
                    <tr style="vertical-align:top;">
                        <td class="Table_WithBorder">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="LabelAccountBalance" CssClass="Label_Small" runat="server">Account Balance</asp:Label></td>
                                    <td align="left">
                                        <asp:TextBox ID="TextboxAccountBalance" runat="server" CssClass="TextBox_Normal" Width="120"
                                            MaxLength="30" ReadOnly="True" Style="text-align: right;"></asp:TextBox></td>

                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="LabelTDAvailable" CssClass="Label_Small" runat="server">TD Available</asp:Label></td>
                                    <td align="left">
                                        <asp:TextBox ID="TextboxTDAvailable" runat="server" CssClass="TextBox_Normal" Width="120" MaxLength="30"
                                            ReadOnly="True" Style="text-align: right;"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="LabelRequestAmount" CssClass="Label_Small" runat="server">Request Amount</asp:Label></td>
                                    <td align="left">
                                        <asp:TextBox ID="TextboxRequestAmount" runat="server" CssClass="TextBox_Normal" Width="120" MaxLength="30"
                                            ReadOnly="True" Style="text-align: right;"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="td_Text_Small" colspan="2" height="1"></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 175px">
                                        <asp:Label ID="LabelLoanAmount" CssClass="Label_Small" runat="server">Loan Amount</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextboxLoanAmount" runat="server" CssClass="TextBox_Normal" Width="120" MaxLength="30"
                                            ReadOnly="True" Style="text-align: right;"></asp:TextBox></td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LabelFees" CssClass="Label_Small" runat="server">Fees</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextboxFees" runat="server" CssClass="TextBox_Normal" Width="120" MaxLength="30"
                                            ReadOnly="True" Style="text-align: right;"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LabelNet" CssClass="Label_Small" runat="server">Net</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextboxNet" runat="server" CssClass="TextBox_Normal" Width="120" MaxLength="30"
                                            ReadOnly="True" Style="text-align: right;"></asp:TextBox></td>
                                </tr>
                            </table>
                        </td>
                        <td class="Table_WithBorder">
                            <asp:GridView ID="gvDeductions" runat="server" Width="100%" CssClass="DataGrid_Grid" AutoGenerateColumns="False">
                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
                                <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
                                <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                <Columns>
                                     <asp:TemplateField HeaderText="" ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBoxDeduction" runat="server" AutoPostBack="true"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CodeValue" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%#Container.Dataitem("CodeValue")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fee">
                                        <ItemTemplate>
                                            <asp:Label ID="LabelDesc" runat="server" Text='<%#Container.Dataitem("ShortDescription")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TextboxAmount" runat="server" AutoPostBack="true" OnTextChanged="Text_Changed" CssClass="TextBox_Normal" Width="100%" onkeypress="javascript: return ValidateNumeric(this);" onchange="Javascript: FormatAmtControl(this);" Text='<%#Container.Dataitem("Amount")%>' MaxLength="10" Style="text-align: right;">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <%--<asp:DataGrid ID="DatagridDeductions" runat="server" Width="100%" CssClass="DataGrid_Grid" AutoGenerateColumns="False">
                                <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBoxDeduction" runat="server" AutoPostBack="true"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="CodeValue" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%#Container.Dataitem("CodeValue")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Fee">
                                        <ItemTemplate>
                                            <asp:Label ID="LabelDesc" runat="server" Text='<%#Container.Dataitem("ShortDescription")%>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TextboxAmount" runat="server" AutoPostBack="true" OnTextChanged="Text_Changed" CssClass="TextBox_Normal" Width="100%" onkeypress="javascript: return ValidateNumeric(this);" onchange="Javascript: FormatAmtControl(this);" Text='<%#Container.Dataitem("Amount")%>' MaxLength="10" Style="text-align: right;">
                                            </asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <%--<tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="td_Text_Small" height="1"></td>
        </tr>--%>



        <tr>
            <td>
                <table>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <table class="Td_ButtonContainer" style="text-align: right; width: 100%">
                            <tr>
                                <td style="text-align: right; width: 90%">
                                    <asp:Button ID="ButtonProcess" runat="server" Text="Process" Width="80px" Enabled="False" CssClass="Button_Normal"></asp:Button>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Button ID="ButtonOK" runat="server" Text="Close" Width="80px" CssClass="Button_Normal"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
<%--	<asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder></form>
<!--#include virtual="bottom.html"-->--%>
