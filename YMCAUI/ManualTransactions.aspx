<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ManualTransactions.aspx.vb"
    Inherits="YMCAUI.ManualTransactions" %>

<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<!--#include virtual="top.html"-->
<script language="javascript">

    var theform;
    var isIE;
    var isNS;

    /*
    Function to detect the Browser type.
    */
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

    /*
    This function will fire when the control leaves the Text Box.
    The function is responsible for formating the numbers to amount type.
    */
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
</script>
<form id="Form1" method="post" runat="server">
<table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="700">
    <tr>
        <td class="Td_BackGroundColorMenu" align="left">
            <cc1:Menu ID="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False"
                Cursor="Pointer" CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown"
                DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup" MenuFadeDelay="2"
                mouseovercssclass="MouseOver">
                <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
            </cc1:Menu>
        </td>
    </tr>
    <tr>
        <td class="Td_HeadingFormContainer" align="left">
            <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
</table>
<div class="Div_Center">
    <table class="Table_WithoutBorder" width="700">
        <tr>
            <td>
                <iewc:TabStrip ID="TabStripManualTrans" runat="server" Width="700px" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
                    TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
                    AutoPostBack="True" Height="30px">
                    <iewc:Tab Text="List"></iewc:Tab>
                    <iewc:Tab Text="Manual Transaction"></iewc:Tab>
                </iewc:TabStrip>
            </td>
        </tr>
    </table>
    <table class="Table_WithoutBorder" width="700">
        <tr>
            <td>
                <iewc:MultiPage ID="MultiPageManualTrans" runat="server">
                    <iewc:PageView>
                        <!-- Start of Manual Transaction Tab : LIST -->
                        <table width="700" class="Table_WithBorder">
                            <tr>
                                <td>
                                    <div class="Div_Center">
                                        <table width="680">
                                            <tr>
                                                <td>
                                                    <table width="320">
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table width="300" class="Table_WithoutBorder">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:label id="LabelFundId" runat="server" cssclass="Label_Small" width="100">Fund ID</asp:label>
                                                            </td>
                                                            <td>
                                                                <asp:textbox id="TextBoxFundId" runat="server" width="160px" cssclass="TextBox_Normal"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:label id="LabelSSNo" runat="server" cssclass="Label_Small" width="100">SS No.</asp:label>
                                                            </td>
                                                            <td>
                                                                <asp:textbox id="TextBoxSSNo" runat="server" width="160px" cssclass="TextBox_Normal"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:label id="LabelLastName" runat="server" cssclass="Label_Small" width="100">Last Name</asp:label>
                                                            </td>
                                                            <td>
                                                                <asp:textbox id="TextBoxLastName" runat="server" width="160px" cssclass="TextBox_Normal"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:label id="LabelFirstName" runat="server" cssclass="Label_Small" width="100">First Name</asp:label>
                                                            </td>
                                                            <td>
                                                                <asp:textbox id="TextBoxFirstName" runat="server" width="160px" cssclass="TextBox_Normal"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table class="Table_WithoutBorder">
                                                        <tr>
                                                            <td>
                                                                <table width="25">
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="right">
                                                                <asp:button id="ButtonFind" runat="server" width="80px" text="Find" cssclass="Button_Normal"
                                                                    causesvalidation="False"></asp:button>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                                <asp:button id="ButtonClear" runat="server" width="80px" text="Clear" cssclass="Button_Normal"
                                                                    causesvalidation="False"></asp:button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:label id="LabelNoRecFound" runat="server" cssclass="Label_Small" width="100"
                                                        visible="false">No Record Found</asp:label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <table width="690" class="Table_WithoutBorder">
                                        <tr>
                                            <!-- Shashi shekhar:2010-12-09: Added one col Fund No  For YRS 5.0-450, BT-643  -->
                                            <td>
                                                <div style="overflow: auto; width: 685px; border-right-style: none; border-left-style: none;
                                                    height: 200px; border-bottom-style: none">
                                                    <asp:datagrid id="DataGridManualTransList" runat="server" width="665px" cssclass="DataGrid_Grid"
                                                        autogeneratecolumns="false" allowsorting="true">
															<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
															<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
															<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
															<selectedItemStyle cssClass="DataGrid_SelectedStyle"></selectedItemStyle>
															<Columns>
																<asp:TemplateColumn>
																	<ItemTemplate>
																		<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																			CommandName="Select" ToolTip="Select"></asp:ImageButton>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:BoundColumn HeaderText="SSN" DataField="SSN" sortexpression="SSN"></asp:BoundColumn>
																<asp:BoundColumn HeaderText="Last Name" DataField="LastName" sortexpression="LastName"></asp:BoundColumn>
																<asp:BoundColumn HeaderText="First Name" DataField="FirstName" sortexpression="FirstName"></asp:BoundColumn>
																<asp:BoundColumn HeaderText="M. Name" DataField="MiddleName" sortexpression="MiddleName"></asp:BoundColumn>
																<asp:BoundColumn HeaderText="YMCA" DataField="YMCAName" sortexpression="YMCAName"></asp:BoundColumn>
																<asp:BoundColumn HeaderText="B.Type" DataField="BeneType" sortexpression="BeneType"></asp:BoundColumn>
																<asp:BoundColumn HeaderText="Status" DataField="Status" sortexpression="Status"></asp:BoundColumn>
																<asp:BoundColumn HeaderText="Hired" DataField="HireDate" DataFormatString="{0:d}" sortexpression="HireDate"></asp:BoundColumn>
																<asp:BoundColumn HeaderText="Term." DataField="TermDate" DataFormatString="{0:d}" sortexpression="TermDate"></asp:BoundColumn>
																<asp:BoundColumn HeaderText="IsArchived" DataField="IsArchived" visible =false></asp:BoundColumn>
                                                                <asp:BoundColumn HeaderText="Fund No" DataField="FundIDNo" visible =false></asp:BoundColumn>
															</Columns>
														</asp:datagrid>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <!-- End of Manual Transaction Tab : LIST -->
                    </iewc:PageView>
                    <iewc:PageView>
                        <!-- Start of Manual Transaction Tab : MANUAL TRANSACTION -->
                        <div class="Div_Center">
                            <table width="700" align="center" class="Table_WithBorder">
                                <tr>
                                    <td>
                                        <table width="240">
                                            <tr>
                                                <td>
                                                    <asp:label id="LabelTransactsHistory" runat="server" width="220">History of Manual Transactions</asp:label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="overflow: auto; width: 205px; border-right-style: none; border-left-style: none;
                                                        height: 350px; border-bottom-style: none">
                                                        <asp:datagrid id="DataGridHistory" runat="server" width="198px" cssclass="DataGrid_Grid"
                                                            autogeneratecolumns="false">
																<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																<Columns>
																	<asp:BoundColumn HeaderText="Date" DataField="dtmcreated" DataFormatString="{0:d}"></asp:BoundColumn>
																	<asp:BoundColumn HeaderText="Creator" DataField="chvcreator"></asp:BoundColumn>
																	<asp:BoundColumn HeaderText="Notes" DataField="txtnote"></asp:BoundColumn>
																</Columns>
															</asp:datagrid>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <font color="#cc0000">
                                                        <asp:checkbox id="CheckBoxClear" runat="server" text="Clear textBoxes after save."
                                                            checked="True" cssclass="CheckBox_Normal"></asp:checkbox>
                                                    </font>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table width="420">
                                            <tr>
                                                <td align="right" width="100">
                                                    HireDate
                                                </td>
                                                <td align="left">
                                                    <asp:textbox id="TextBoxHireDate" runat="server" width="70" cssclass="TextBox_Normal"
                                                        enabled="False"></asp:textbox>
                                                </td>
                                                <td align="right" width="100">
                                                    Term. Date
                                                </td>
                                                <td align="left">
                                                    <asp:textbox id="TextBoxTermDate" runat="server" width="70" cssclass="TextBox_Normal"
                                                        enabled="False"></asp:textbox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" width="100">
                                                    SSN
                                                </td>
                                                <td width="70" colspan="2" align="left">
                                                    <asp:textbox id="TextBoxSSN" runat="server" width="140" cssclass="TextBox_Normal"
                                                        enabled="False"></asp:textbox>
                                                </td>
                                                <td align="center" width="100">
                                                    <font color="#3300ff">Annuity Basis</font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" width="100" valign="middle">
                                                    YMCA
                                                </td>
                                                <td colspan="2" align="left" valign="middle">
                                                    <asp:textbox id="TextBoxYMCA" runat="server" width="140" cssclass="TextBox_Normal"
                                                        enabled="False"></asp:textbox>
                                                </td>
                                                <td align="center" width="90" rowspan="2">
                                                    <table class="Table_WithBorder">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:radiobutton id="RadioButtonPost96" runat="server" text="Post 96" value="PST"
                                                                    groupname="grpAnnuity" checked="True" cssclass="RadioButton_Normal"></asp:radiobutton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:radiobutton id="RadioButtonPre96" runat="server" text="Pre 96" value="PRE" groupname="grpAnnuity"
                                                                    cssclass="RadioButton_Normal"></asp:radiobutton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:radiobutton id="RadioButtonRollIns" runat="server" text="Roll-Ins" value="ROLL"
                                                                    groupname="grpAnnuity" cssclass="RadioButton_Normal"></asp:radiobutton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td align="left">
                                                    <asp:radiobutton id="RadioButtonPrincipal" runat="server" text="Principal" groupname="grpPrincipal"
                                                        checked="True" cssclass="RadioButton_Normal"></asp:radiobutton>
                                                </td>
                                                <td align="left">
                                                    <asp:radiobutton id="RadioButtonInterest" runat="server" text="Interest" groupname="grpPrincipal"
                                                        cssclass="RadioButton_Normal"></asp:radiobutton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" width="100">
                                                    Acct.Type
                                                </td>
                                                <td colspan="3" align="left">
                                                    <asp:dropdownlist id="DropDownListAcctType" runat="server" width="278px" cssclass="DropDown_Normal"></asp:dropdownlist>
                                                    <asp:requiredfieldvalidator id="ReqAcctType" runat="server" errormessage="*" controltovalidate="DropDownListAcctType"></asp:requiredfieldvalidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" width="100">
                                                    Trans.Type
                                                </td>
                                                <td colspan="3" align="left">
                                                    <asp:dropdownlist id="DropDownListTransType" runat="server" width="278px" cssclass="DropDown_Normal"></asp:dropdownlist>
                                                    <asp:requiredfieldvalidator id="ReqTransType" runat="server" errormessage="*" controltovalidate="DropDownListTransType"></asp:requiredfieldvalidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:checkbox id="CheckboxOverride" runat="server" text="Override" cssclass="CheckBox_Normal"
                                                        autopostback="true"></asp:checkbox>
                                                </td>
                                                <td align="center" colspan="3">
                                                    <font color="#3300ff">-----------------YMCA-----------------</font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" width="100">
                                                    YMCA Pre Tax
                                                </td>
                                                <td>
                                                    <asp:textbox id="TextBoxYMCAPreTax" runat="server" width="80" cssclass="TextBox_Normal_Amount"
                                                        enabled="False" text="0.00" maxlength="9"></asp:textbox>
                                                </td>
                                                <td align="right" width="90">
                                                    M. Comp
                                                </td>
                                                <td align="left" valign="top">
                                                    <asp:textbox id="TextBoxMComp" runat="server" width="80" cssclass="TextBox_Normal_Amount"
                                                        enabled="False" text="0.00" maxlength="9"></asp:textbox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td align="center" colspan="3">
                                                    <font color="#3300ff">---------------Participant-----------------</font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" width="100" valign="top">
                                                    Pre Tax
                                                </td>
                                                <td>
                                                    <asp:textbox id="TextBoxPreTax" runat="server" width="80" cssclass="TextBox_Normal_Amount"
                                                        maxlength="9">0.00</asp:textbox>
                                                </td>
                                                <td align="right" width="90" valign="top">
                                                    Post Tax
                                                </td>
                                                <td valign="top" align="left">
                                                    <asp:textbox id="TextBoxPostTax" runat="server" width="80" cssclass="TextBox_Normal_Amount"
                                                        maxlength="9">0.00</asp:textbox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td align="center" colspan="3" valign="top">
                                                    <font color="#3300ff">-------------------Dates------------------</font>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" width="104">
                                                    Acct.Date
                                                </td>
                                                <td valign="top" align="left">
                                                    &nbsp;
                                                    <asp:textbox id="TextBoxAcctDate" runat="server" width="64" cssclass="TextBox_Normal"
                                                        enabled="False"></asp:textbox>
                                                </td>
                                                <td align="right" width="90">
                                                    Rec.Date
                                                </td>
                                                <td valign="top" align="left">
                                                    <uc1:DateUserControl ID="TextBoxRecDate" runat="server"></uc1:DateUserControl>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" width="104">
                                                    Trans.Date
                                                </td>
                                                <td valign="top" align="left">
                                                    &nbsp;
                                                    <uc1:DateUserControl ID="TextBoxTransDate" runat="server"></uc1:DateUserControl>
                                                </td>
                                                <td align="right" width="90">
                                                    Fund Date
                                                </td>
                                                <td valign="top" align="left">
                                                    <uc1:DateUserControl ID="TextBoxFundDate" runat="server"></uc1:DateUserControl>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="90" align="right" valign="top">
                                                    Notes
                                                </td>
                                                <td colspan="3" valign="top" align="left">
                                                    <asp:textbox id="TextboxNotes" runat="server" width="260px" textmode="MultiLine"
                                                        height="75px"></asp:textbox>
                                                    <asp:requiredfieldvalidator id="ReqNotes" runat="server" errormessage="*" controltovalidate="TextboxNotes"></asp:requiredfieldvalidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="90">
                                                    &nbsp;
                                                </td>
                                                <td align="right">
                                                    <asp:button id="ButtonPHR" runat="server" text="PHR" width="71px" cssclass="Button_Normal"
                                                        causesvalidation="false"></asp:button>
                                                </td>
                                                <td align="center">
                                                    <asp:button id="ButtonSave" runat="server" text="Save" width="71px" cssclass="Button_Normal"></asp:button>
                                                </td>
                                                <td align="left">
                                                    <asp:button id="ButtonClearAll" runat="server" text="Clear All" width="71px" cssclass="Button_Normal"
                                                        causesvalidation="False"></asp:button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <!-- End of Manual Transaction Tab : MANUAL TRANSACTION  -->
                    </iewc:PageView>
                </iewc:MultiPage>
            </td>
        </tr>
        <tr>
            <td>
                <table class="Table_WithoutBorder" width="700">
                    <tr>
                        <td class="Td_ButtonContainer" align="right">
                            <asp:button id="ButtonClose" runat="server" cssclass="Button_Normal" width="71px"
                                text="Close" enableviewstate="False" causesvalidation="False"></asp:button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<asp:literal id="LiteralAcctType" runat="server" visible="False"></asp:literal>
<asp:literal id="LiteralTransType" runat="server" visible="False"></asp:literal>
<asp:literal id="LiteralTempIndex" runat="server" visible="False"></asp:literal>
<asp:placeholder id="PlaceHolderManualTrans" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
