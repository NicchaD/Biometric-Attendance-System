<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="YRSControls" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>

<%@ Register TagPrefix="DataPagerFindInfo" TagName="DataGridPager" Src="UserControls/DataGridPager.ascx" %>


<%--<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl_new.ascx" %>--%>
<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RetiredQDRO.aspx.vb" Inherits="YMCAUI.RetiredQDRO" %>

<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="~/UserControls/YMCA_Header_WebUserControl.ascx" %>
<%--START: PPP | 11/28/2016 | YRS-AT-3299 --%>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="~/UserControls/YMCA_Footer_WebUserControl.ascx" %>
<%--END: PPP | 11/28/2016 | YRS-AT-3299 --%>


<!--#include virtual="top.html"-->

    <%--START: PPP | 11/28/2016 | YRS-AT-3299 --%>
    <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    <script src="JS/jquery-ui/JScript-1.7.2.0.min.js" type="text/javascript"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link id="Link1" href="CSS/CustomStyleSheet.css" type="text/css" runat="server" rel="stylesheet" />
    <%--END: PPP | 11/28/2016 | YRS-AT-3299 --%>
    <title>YMCA YRS</title>
    <script language="javascript" src="JS/YMCA_JScript.js" type="text/javascript"></script>
    <script language="javascript" src="JS/YMCA_JScript_Warn.js" type="text/javascript"></script>
    

<script language="JavaScript">
    <%--START: MMR | 01/09/2017 | YRS-AT-3299 --%>
    function BindEvents() {
        $('#ConfirmDialog').dialog({
            autoOpen: false,
            draggable: true,
            close: false,
            modal: true,
            width: 450, maxHeight: 220,
            title: "QDRO", <%--MMR | 2017.01.30 | YRS-AT-3299 |Changed title from "YMCA-YRS" to "QDRO"--%>
            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });
    }

    function ShowDialog(title, text, img) {
        $('#ConfirmDialog').dialog("option", "title", title);

        $('#divConfirmDialogMessage').html(text);

        $('#imgConfirmDialogInfo').hide();
        $('#imgConfirmDialogOk').hide();
        $('#imgConfirmDialogError').hide();
        $('#trConfirmDialogYesNo').hide();
        $('#trConfirmDialogOK').hide();
        if (img == 'info') {
            $('#imgConfirmDialogInfo').show();
            $('#trConfirmDialogOK').show();
        }
        else if (img == 'infoYesNo') {
            $('#imgConfirmDialogInfo').show();
            $('#trConfirmDialogYesNo').show();
        }
        else if (img == 'ok') {
            $('#imgConfirmDialogOk').show();
            $('#trConfirmDialogOK').show();
        }
        else if (img == 'error') {
            $('#imgConfirmDialogError').show();
            $('#trConfirmDialogOK').show();
        }
        $('#ConfirmDialog').dialog("open");
    }

    function closeDialog(id) {
        $('#<%=hdnRecipientForDeletion.ClientID%>').val('');
        $('#' + id).dialog('close');
    }
    <%--END: MMR | 01/09/2017 | YRS-AT-3299  --%>


    <%-- START -|Chandra sekar | 2016.08.22 |YRS-AT-3081 | YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098) --%>
    function SelectSplitOption(selectedOption) {
        $('#<%=TextBoxPercentageWorkSheet.ClientID%>').val('0.00'); <%--Set % = 0.00--%>
        $('#<%=TextBoxAmountWorkSheet.ClientID%>').val('0.00'); <%--Set $ = 0.00--%>
        $('#<%=ButtonSplit.ClientID%>').prop('disabled', true);
        if (selectedOption == 'Amount') {
            $('#<%=TextBoxAmountWorkSheet.ClientID%>').prop('disabled', false);
            $('#<%=TextBoxPercentageWorkSheet.ClientID%>').prop('disabled', true);
        }
        else {
            $('#<%=TextBoxAmountWorkSheet.ClientID%>').prop('disabled', true);
            $('#<%=TextBoxPercentageWorkSheet.ClientID%>').prop('disabled', false);
        }
    }
    <%-- END -|Chandra sekar | 2016.08.22 |YRS-AT-3081 | YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098) --%>

    function ValidatePNumeric(str) {

        if ((event.keyCode < 48) || (event.keyCode > 57)) {
            event.returnValue = false;
        }
        if ((event.keyCode == 80 || event.keyCode == 112) && str.length == 0) {
            event.returnValue = true;
        }
        if (event.keyCode == 45) {
            event.returnValue = true;
        }
    }

    function ValidateNumeric() {

        if ((event.keyCode < 48) || (event.keyCode > 57)) {
            event.returnValue = false;
        }
    }
    function ValidateTelephoneNo(str) {
        if (document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value == "US" || document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value == "CA") {
            var val = str.value
            if (((val.length < 10) || (val.length > 10)) && (val.length != 0)) {
                alert('Telephone number must be 10 digits.')
                str.focus()
                return false;
            }
        }

    }
    function SetMaxLengthPhone(str) {
        if (document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value == "US" || document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value == "CA") {
            str.maxLength = 10;
        }
        else {
            str.maxLength = 25;
        }
    }
    function ValidateSSNo(str) {
        var objSSNoValue = "0123456789";
        var objPSSNoVal = "pP";
        var objChar;
        var objCnt;

        objChar = str.charAt(0);
        if (objPSSNoVal.indexOf(objChar) == -1) {
            objCnt = 0;
            objChar = "";
        }
        else {
            objCnt = 1;
            objChar = "";
        }
        for (i = objCnt; i < str.length; i++) {
            objChar = str.charAt(i);
            if (objSSNoValue.indexOf(objChar) == -1) {
                document.Form1.all.TextBoxSpouseSSNo.value = "";
            }
        }
    }

    function Check_Amount_Percentage(str) {
        var ObjValue = "1";
        if (ObjValue > str) {
            document.Form1.all.TextBoxAmountWorkSheet.value = "0.00";
        }
        if (ObjValue > str) {
            document.Form1.all.TextBoxPercentageWorkSheet.value = "0.00";
        }
    }

    <%--START: PPP | 01/20/2017 | YRS-AT-3299 | _OnBlur_TextBoxAmountWorkSheet() and _OnBlur_TextBoxPercentageWorkSheet() are not being called from anywhere , so commenting it --%>
    <%--
    function _OnBlur_TextBoxAmountWorkSheet() {


        if (isNaN(parseInt(document.Form1.all.TextBoxAmountWorkSheet.value))) {
            alert('Amount cannot have characters.');
            document.Form1.all.TextBoxAmountWorkSheet.value = 0;
            document.Form1.all.TextBoxAmountWorkSheet.focus();
            event.returnValue = false;
        }

    }
    function _OnBlur_TextBoxPercentageWorkSheet() {


        if (isNaN(parseInt(document.Form1.all.TextBoxPercentageWorkSheet.value))) {
            alert('Percentage cannot have characters.');
            document.Form1.all.TextBoxPercentageWorkSheet.value = 0;
            document.Form1.all.TextBoxPercentageWorkSheet.focus();
        }

    }
    --%>
    <%--END: PPP | 01/20/2017 | YRS-AT-3299 | _OnBlur_TextBoxAmountWorkSheet() and _OnBlur_TextBoxPercentageWorkSheet() are not being called from anywhere , so commenting it --%>

    function ValidateAlphaNumeric() {

        if (((event.keyCode > 64) && (event.keyCode < 91)) || ((event.keyCode > 96) && (event.keyCode < 123)) || ((event.keyCode > 47) && (event.keyCode < 58)) || (event.keyCode == 45)) {

            event.returnValue = true;
        }
        else {
            event.returnValue = false;
        }
    }
    function isValidEmail(str) {
        var at = "@"
        var dot = "."
        var lat = str.indexOf(at)
        var lstr = str.length
        var ldot = str.indexOf(dot)
        if (str.indexOf(at) == -1) {
            alert("Invalid E-mail ID")
            return false
        }
        if (str.indexOf(at) == -1 || str.indexOf(at) == 0 || str.indexOf(at) == lstr) {
            alert("Invalid E-mail ID")
            return false
        }
        if (str.indexOf(dot) == -1 || str.indexOf(dot) == 0 || str.indexOf(dot) == lstr) {
            alert("Invalid E-mail ID")
            return false
        }
        if (str.indexOf(at, (lat + 1)) != -1) {
            alert("Invalid E-mail ID")
            return false
        }
        if (str.substring(lat - 1, lat) == dot || str.substring(lat + 1, lat + 2) == dot) {
            alert("Invalid E-mail ID")
            return false
        }
        if (str.indexOf(dot, (lat + 2)) == -1) {
            alert("Invalid E-mail ID")
            return false
        }
        if (str.indexOf(" ") != -1) {
            alert("Invalid E-mail ID")
            return false
        }
        return true
    }

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
            // neeraj 08112010 BT-637 :if percentage is greater than 100 then make it as 100.
            if (parseFloat(ctl.value) > 100 && ctl.id === '<%= TextBoxPercentageWorkSheet.ClientID %>')
            { ctl.value = "100.00"; }

            ctlVal = ctl.value;
            iPeriodPos = ctlVal.indexOf(".");
            if (iPeriodPos < 0) {
                if (ctl.value.length > (iMaxLen - 3)) {
                    sTemp = ctl.value
                    tempVal = sTemp.substr(0, (iMaxLen - 3)) + ".00";
                }
                else
                    <%--START: PPP | 01/24/2017 | YRS-AT-3299 | If text box is left blank earlier code was changing it to only ".00". this change will keep "0.00"--%>
                    if (ctlVal == "")
                        ctlVal = "0";
                    <%--END: PPP | 01/24/2017 | YRS-AT-3299 | If text box is left blank earlier code was changing it to only ".00". this change will keep "0.00"--%>
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
        var reValidChars = /[0-9.]/;
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


    //Sanjay R:2013.08.05 - YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
    $(document).ready(function () {
        $("#divWSMessage").dialog
					({
					    modal: true,
					    open: function (event, ui) { $(this).parent('div').find('button:contains("SAVE")').focus(); },
					    autoOpen: false,
					    title: "Process Restricted",
					    width: 570, height: 200,
					    position: ['center', 200],
					    buttons: [{ text: "OK", click: CloseWSMessage }]
					});
    });

    function CloseWSMessage() {
        $(document).ready(function () {
            $("#divWSMessage").dialog('close');
        });
    }
    function openDialog(str, type) {
        $(document).ready(function () {
            if (type == 'Bene') {
                str = 'Beneficiary add,edit and delete operation can not be performed due to following reason(s).<br/>' + str
            }
            else {
                str = 'Retired QDRO Process can not be performed due to following reasons(s).<br/>' + str;
            }
            $("#divWSMessage").html(str);
            $("#divWSMessage").dialog('open');
            return false;
        });
    }
    //End,Sanjay R:2013.08.05 - YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
    $(document).ready(function () {
        $("#divBeneficiaryAnnuityDeatils").dialog
					({
					    modal: true,
					    autoOpen: false,
					    title: "QDRO", <%-- MMR | 2017.01.28 | YRS-AT-3299 | Changed title from "Confirmation" to "QDRO" --%>
					    width: 500,
					    open: function (type, data) {
					        $(this).parent().appendTo("form");
					    }
					});
    });

    function OpenConfirmationMessageBox() {
        $(document).ready(function () {
            $("#divBeneficiaryAnnuityDeatils").height($("#divBeneficiaryAnnuityDeatils").height());
            $("#divBeneficiaryAnnuityDeatils").width($("#divBeneficiaryAnnuityDeatils").width());
            $("#divBeneficiaryAnnuityDeatils").dialog('open');
            //return false;
        });
    }
    function CloseConfirmationMessageBox() {
        $(document).ready(function () {
            $("#divBeneficiaryAnnuityDeatils").dialog('close');
        });
        return false; <%-- PPP | 01/24/2017 | YRS-AT-3299 | This will help to prevent postback --%>
    }
    function OpenNewWindow(strURL) {
        settings = 'width=' + 750 + ',height=' + 600 + ',scrollbars=yes,location=no,directories=no,status=no,menubar=no,toolbar=no,resizable=yes';
        win = window.open(strURL, "RetiredQDRO", settings);
        return false; <%-- PPP | 01/23/2017 | YRS-AT-3299 | This will prevent postback --%>
    }
</script>
<form id="Form1" method="post" runat="server">

<div class="Div_Center">
    <table class="topbggray" id="Table3" cellspacing="0" cellpadding="0" width="980"> <%-- PPP | 01/16/2017 | YRS-AT-3299 | Changed width from 720px to 980 --%>
        <tr>
            <td class="Td_BackGroundColorMenu" align="left" colspan="2"> <%-- PPP | 01/16/2017 | YRS-AT-3299 | Added [ colspan="2"] --%>
                <cc1:Menu ID="Menu1" runat="server" mouseovercssclass="MouseOver" MenuFadeDelay="2"
                    DefaultMouseUpCssClass="mouseup" DefaultMouseOverCssClass="mouseover" DefaultMouseDownCssClass="mousedown"
                    DefaultCssClass="menuitem" CssClass="menustyle" Cursor="Pointer" HighlightTopMenu="False"
                    Layout="Horizontal">
                    <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
                </cc1:Menu>
            </td>
        </tr>
        <%--	<tr>
					<td class="Td_HeadingFormContainer" align="left">QDRO Retirees Information :<asp:label id="LabelHdr" runat="server" CssClass="Td_HeadingFormContainer"></asp:label>
					</td>
				</tr>--%>
        <tr>
            <td class="Td_HeadingFormContainer" align="left" colspan="2"> <%-- PPP | 01/16/2017 | YRS-AT-3299 | Added [ colspan="2"] --%>
                <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
            </td>
        </tr>
        <%--START: MMR | 01/09/2017 | YRS-AT-2990 | Added div to show negative balance error--%>
        <%--<tr>
            <td height="2px">
            </td>
        </tr>--%>
        <tr>
            <td style="text-align: left;" colspan="2">
                <div id="DivMainMessage" class="error-msg" runat="server" style="text-align: left; display: none;" enableviewstate="false">
                </div>
                <div id="DivPlanWarningMessage" class="warning-msg-withRedFont" runat="server" style="text-align: left; display: none;" enableviewstate="false">
                </div>
                <div id="DivWarningMessage" class="warning-msg" runat="server" style="text-align: left; display: none;" enableviewstate="false">
                </div>
                <div id="DivSuccessMessage" class="success-msg" runat="server" style="text-align: left; display: none;" enableviewstate="false">
                </div>
            </td>
        </tr>
        <%--END: MMR | 01/09/2017 | YRS-AT-2990 | Added div to show negative balance error--%>
        <tr>
            <td style="width:60%"> <%-- PPP | 01/16/2017 | YRS-AT-3299 | Added [ style="width:50%"] --%>
                <iewc:TabStrip ID="QdroRetiredTabStrip" runat="server" Width="100%" Height="30px"
                    TabSelectedStyle="background-color:#93BEEE;color:#000000;" TabHoverStyle="background-color:#93BEEE;color:#4172A9;"
                    TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
                    AutoPostBack="True">
                    <%--<iewc:Tab Text="List"></iewc:Tab>--%> <%--MMR | 2017.01.09 | Removed list tab as Find infor screen implemented--%>
                    <%--START: PPP | 01/16/2017 | YRS-AT-3299 | As per requirements changed tab text --%>
                    <iewc:Tab Text="1. Define Recipient(s)"></iewc:Tab> <%--<iewc:Tab Text="Beneficiary"></iewc:Tab>--%>
                    <iewc:Tab Text="2. View/Split Annuities"></iewc:Tab> <%--<iewc:Tab Text="Annuities" Enabled="false"></iewc:Tab>--%>
                    <iewc:Tab Text="3. Review & Save"></iewc:Tab> <%--<iewc:Tab Text="Summary"></iewc:Tab>--%>
                    <%--END: PPP | 01/16/2017 | YRS-AT-3299 | As per requirements changed tab text --%>
                </iewc:TabStrip>
            </td>
            <td style="width:40%"></td> <%-- PPP | 01/16/2017 | YRS-AT-3299 | Added <td> --%>
        </tr>
        <tr>
            <td height="7px">
            </td>
        </tr>
    </table>
</div>
<div class="Div_Center">
    <table id="Table5" class="Table_WithBorder" cellspacing="0" cellpadding="0" width="980"> <%-- PPP | 01/16/2017 | YRS-AT-3299 | Changed width from 720px to 980 --%>
        <tr>
            <td>
                <iewc:MultiPage ID="LIstMultiPage" runat="server">
                    <%--START: MMR | 2017.01.09 | Commented existing code as not required instead used existing find info screen to find QDRO person retired memeber list --%>
                    <%--<iewc:PageView>
                        <!--Start of list page"-->
                        <table width="700">
                            <tr valign="top">
                                <td height="2px" align="left">
                                </td>
                            </tr>
                            <tr valign="top">
                                <td valign="top" align="left">
                                    <asp:label id="LabelNoDataFound" runat="server" visible="False" font-bold="false"
                                        font-size="XX-Small">No Matching Records</asp:label>
                                    <div style="overflow: scroll; width: 470px; border-top-style: none; border-right-style: none;
                                        border-left-style: none; position: static; height: 200px; border-bottom-style: none">
                                        <DataPagerFindInfo:DataGridPager ID="dgPager" runat="server"></DataPagerFindInfo:DataGridPager>
                                        <asp:datagrid width="485" id="DataGridRetireeList" cssclass="DataGrid_Grid" runat="server"
                                            allowsorting="true" allowpaging="True" pagesize="20">
													<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
													<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
													<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
													<columns>
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																	CommandName="Select" ToolTip="Select"></asp:ImageButton>
															</ItemTemplate>
														</asp:TemplateColumn>
													</columns>
													<PagerStyle Visible="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid>
                                    </div>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td align="left">
                                                <asp:label cssclass="Label_Small" id="LabelFundNoList" text="Fund No" runat="server"></asp:label>
                                            </td>
                                            <td>
                                                <asp:textbox width="100" runat="server" id="TextBoxFundNoList" cssclass="TextBox_Normal"
                                                    maxlength="9"></asp:textbox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:label cssclass="Label_Small" id="LabelSSNoList" text="SS No" runat="server"></asp:label>
                                            </td>
                                            <td class="Label_Small">
                                                <asp:textbox width="100" runat="server" id="TextBoxSSNoList" cssclass="TextBox_Normal"
                                                    maxlength="11"></asp:textbox> < %-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name -- % >
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:label cssclass="Label_Small" id="LabelLastNameList" text="Last Name" runat="server"></asp:label>
                                            </td>
                                            <td>
                                                <asp:textbox width="100" runat="server" id="TextBoxLastNameList" cssclass="TextBox_Normal"></asp:textbox> < %-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name -- % >
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label_Small" align="left">
                                                <asp:label cssclass="Label_Small" id="LabelFirstNameList" text="First Name" runat="server"></asp:label>
                                            </td>
                                            <td>
                                                <asp:textbox width="100" runat="server" id="TextBoxFirstNameList" cssclass="TextBox_Normal"></asp:textbox> < %-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name -- % >
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label_Small" align="left">
                                                <asp:label cssclass="Label_Small" id="LabelCityList" text="City" runat="server"></asp:label>
                                            </td>
                                            <td>
                                                <asp:textbox width="100" runat="server" id="TextBoxCityList" cssclass="TextBox_Normal"></asp:textbox> < %-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name -- % >
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label_Small" align="left">
                                                <asp:label cssclass="Label_Small" id="LabelStateList" text="State" runat="server"></asp:label>
                                            </td>
                                            <td>
                                                <asp:textbox width="100" runat="server" id="TextBoxStateList" cssclass="TextBox_Normal"></asp:textbox> < %-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name -- % >
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:button cssclass="Button_Normal" width="80" runat="server" accesskey="F" id="ButtonFindList"
                                                    text="Find" causesvalidation="False"></asp:button>
                                            </td>
                                            <td>
                                                <asp:button cssclass="Button_Normal" width="80" runat="server" id="ButtonClear" text="Clear"
                                                    causesvalidation="False"></asp:button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </iewc:PageView>--%>
                    <%--END: MMR | 2017.01.09 | Commented existing code as not required instead used existing find info screen to find QDRO person retired memeber list--%>
                    <iewc:PageView>
                        <!--Start of Personal"-->
                        <div class="Div_Left">
                            <table width="980px" class="Table_WithOutBorder" cellpadding="0" cellspacing="0" 
                                    align="center"> <%-- PPP | 01/16/2017 | YRS-AT-3299 | Changed width from 696px to 980px --%>
                                    <tr valign="top">
                                        <td align="center" width="50%">
                                            <table>
                                                <tr>
                                                    <td align="left" height="23px" width="83px">
                                                        <asp:label cssclass="Label_Small" id="LabelSSNo" runat="server" text="SSNo"></asp:label> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                        <td align="left" height="23px">
                                                            <asp:textbox cssclass="TextBox_Normal" id="TextBoxSSNo" runat="server" width="200px" 
                                                                autopostback="true" maxlength="9"></asp:textbox> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name, PPP | 01/24/2017 | YRS-AT-3299 | Changed maxlength from 11 to 9 --%>
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" height="23px">
                                                        <asp:label cssclass="Label_Small" id="LabelSal" runat="server" text="Sal."></asp:label> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                    <td align="left" height="23px">
                                                        <asp:dropdownlist id="DropdownlistSal" cssclass="DropDown_Normal" runat="server"
                                                            width="80px"> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
																<asp:ListItem Value="" selected="true"></asp:ListItem>
																<asp:ListItem Value="Dr.">Dr.</asp:ListItem>
																<asp:ListItem Value="Mr.">Mr.</asp:ListItem>
																<asp:ListItem Value="Mrs.">Mrs.</asp:ListItem>
																<asp:ListItem Value="Ms.">Ms.</asp:ListItem>
															</asp:dropdownlist>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" height="23px">
                                                        <asp:label cssclass="Label_Small" id="LabelFirstName" runat="server" text="First"></asp:label> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                    <td align="left" height="23px">
                                                        <asp:textbox cssclass="TextBox_Normal" id="TextBoxFirstName" runat="server" width="200px"></asp:textbox> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                        <asp:requiredfieldvalidator id="reqFirstName" runat="server" errormessage="Please enter FirstName"
                                                            controltovalidate="TextBoxFirstName" display="Dynamic">*</asp:requiredfieldvalidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" height="23px">
                                                        <asp:label cssclass="Label_Small" id="LabelMiddleName" runat="server" text="Middle"></asp:label> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                    <td align="left" height="23px">
                                                        <asp:textbox cssclass="TextBox_Normal" id="TextBoxMiddleName" runat="server" width="200px"></asp:textbox> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" height="23px">
                                                        <asp:label cssclass="Label_Small" id="LabelLastName" runat="server" text="Last"></asp:label> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                    <td align="left" height="23px">
                                                        <asp:textbox cssclass="TextBox_Normal" id="TextBoxLastName" runat="server" width="200px"></asp:textbox> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                        <asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" errormessage="Please enter LastName"
                                                            controltovalidate="TextBoxLastName" display="Dynamic">*</asp:requiredfieldvalidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" height="23px">
                                                        <asp:label cssclass="Label_Small" id="LabelSuffix" runat="server" text="Suffix"></asp:label> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                    <td align="left" height="23px">
                                                        <asp:textbox cssclass="TextBox_Normal" id="TextBoxSuffix" runat="server" width="100px" 
                                                            maxlength="6"></asp:textbox><%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="left" width="50%"  >
                                            <table width="100%" align="left">
                                                <tr>    
                                                     <td  align="left">
                                                        <table align="left">
                                                            <tr>
                                                                <td align="left" width="104px" nowrap >
                                                                    <asp:label cssclass="Label_Small" id="LabelBirthDate" runat="server" text="Birth Date"></asp:label>
                                                                </td>
                                                                <td align="left" nowrap>
                                                                    <%--<table>
                                                                        <tr>
                                                                            <td>--%>
                                                                                <YRSControls:DateUserControl ID="TextBoxBirthDate" runat="server"></YRSControls:DateUserControl>
                                                                            <%--</td>
                                                                        </tr>
                                                                    </table>--%>
                                                                </td>
                                                            </tr>
                                                <%--Start- Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Adding dropdown option for marital status and gender --%>                                                                 
                                                            <tr>
                                                                <td align="left" height="23px">
                                                                    <asp:label cssclass="Label_Small" id="LabelMaritalStatus" runat="server"
                                                                        text="Marital Status"></asp:label> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                                </td>
                                                                <td align="left" height="23px">
                                                                    <asp:dropdownlist Id="DropDownListMaritalStatus" width="150" runat="server" cssclass="DropDown_Normal Warn"
                                                                        autopostback="false"></asp:dropdownlist>
                                                                <asp:comparevalidator id="compValidatorMaritalStatus" runat="server" errormessage="Please Select Marital Status"
                                                                        controltovalidate="DropDownListMaritalStatus" type="String" display="Dynamic" operator="NotEqual"
                                                                         ValueToCompare="SEL">*</asp:comparevalidator>

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" height="23px">
                                                                    <asp:label cssclass="Label_Small" id="LabelGender" runat="server" 
                                                                        text="Gender"></asp:label> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                                </td>
                                                                <td align="left" height="23px">
                                                                    <asp:dropdownlist id="DropDownListGender" runat="server" width="90" cssclass="DropDown_Normal Warn"
                                                                        autopostback="false"></asp:dropdownlist>
                                                                    <asp:comparevalidator id="compValidatorGender" runat="server" errormessage="Please Select Gender"
                                                                        controltovalidate="DropDownListGender" type="String" display="Dynamic" operator="NotEqual"
                                                                         ValueToCompare="SEL">*</asp:comparevalidator>                                                                    
                                                                </td>
                                                            </tr>
                                                 <%--End- Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Adding dropdown option for marital status and gender--%>                                                 

                                                        </table>
                                                      </td>
                                                </tr>
                                            </table>
                                            
                                        </td>
                                    </tr>
                                     <tr valign="top">
                                        <td align="center" width="50%">
                                            <table>
                                                <tr>
                                                    <td align="left" style="vertical-align:top;width:83px">
                                                        <asp:label cssclass="Label_Small" runat="server" id="LabelAddress" text="Address"></asp:label>
                                                    </td>
                                                    <td align="left" width="225px">
                                                        <NewYRSControls:New_AddressWebUserControl runat="server" ID="AddressWebUserControl1" AllowNote="true" AllowEffDate="true" PopupHeight="530"  />      
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="left" width="50%">
                                            <table width="100%" align="left">
                                                <tr>
                                                    <td align="left" width="105px">
                                                        <asp:label cssclass="Label_Small" runat="server" id="LabelEmail" text="E-mail Addr"></asp:label> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                    <td align="left">
                                                        <asp:textbox cssclass="TextBox_Normal" id="TextBoxEmail" runat="server" width="150px"></asp:textbox> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="60">
                                                        <asp:label cssclass="Label_Small" id="LabelTel" runat="server" 
                                                            text="Telephone"></asp:label> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                    <td align="left">
                                                            <asp:textbox cssclass="TextBox_Normal" id="TextBoxTel" runat="server" width="150px"
                                                                maxLength="15" ></asp:textbox> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                </tr>                                                            
                                            </table>
                                        </td>
                                     </tr>                                
                                    <tr>
                                    <td colspan="4">
                                        <table width="100%">
                                            <tr>
                                                <td class="Td_ButtonContainer" align="center">
                                                    <asp:button id="ButtonAddNewBeneficiary" width="70" cssclass="Button_Normal" runat="server"
                                                        text="Add New" causesvalidation="False"></asp:button>
                                                </td>
                                                <td class="Td_ButtonContainer" align="center">
                                                    <asp:button id="ButtonEditBeneficiary" enabled="false" width="60" cssclass="Button_Normal"
                                                        runat="server" text="Edit" causesvalidation="False"></asp:button>
                                                </td>
                                                <td class="Td_ButtonContainer" align="center">
                                                    <asp:button id="ButtonCancelBeneficiary" enabled="false" width="60" cssclass="Button_Normal"
                                                        runat="server" text="Cancel" causesvalidation="False"></asp:button>
                                                </td>
                                    </td>
                                    <td class="Td_ButtonContainer" valign="bottom" align="center">
                                        <asp:button id="btnAddBeneficiaryToList" width="130" cssclass="Button_Normal" tabindex="17"
                                            runat="server" text="Add To List" enabled="False" causesvalidation="True"></asp:button><%-- PPP | 01/24/2017 | YRS-AT-3299 | Changed width from 100 to 130 --%>
                                </tr>
                                    
                                  </table>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="4">
                <div align="center" style="overflow: scroll; border-top-style: none; border-right-style: none;
                    border-left-style: none; height: 120px; border-bottom-style: none">
                    <table width="95%"> <%-- PPP | 01/16/2017 | YRS-AT-3299 | Changed width from 83% to 95% --%>
                        <tr>
                            <td colspan="2">
                                <asp:datagrid id="DatagridBenificiaryList" cssclass="DataGrid_Grid" autogeneratecolumns="false"
                                    runat="server" width="95%"> <%-- PPP | 01/16/2017 | YRS-AT-3299 | Changed width from 100% to 95% --%>
											<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
											<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
											<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
											<columns>
												<asp:TemplateColumn HeaderStyle-Width="2%"> <%-- PPP | 01/16/2017 | YRS-AT-3299 | Added [ HeaderStyle-Width="2%"] --%>
													<ItemTemplate>
														<asp:ImageButton id="Imagebutton1" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
															CommandName="Select" ToolTip="Select"></asp:ImageButton>
													</ItemTemplate>
												</asp:TemplateColumn>
                                                 <%--START: PPP | 01/16/2017 | YRS-AT-3299 | Added "Delete" column in grid, which will help user to delete saved recipient. --%>
												<asp:TemplateColumn HeaderStyle-Width="2%">
                                                     <ItemTemplate>
                                                        <asp:ImageButton ID="ImageDeleteRecipientButton" runat="server" ToolTip="Delete" OnClientClick="javascript: return ConfirmRecipientDelete(this);"
                                                            CommandName="Delete" CausesValidation="False" ImageUrl="images\Delete.gif" AlternateText="Delete"></asp:ImageButton>
                                                        </ItemTemplate>
                                                </asp:TemplateColumn>
                                                 <%--END: PPP | 01/16/2017 | YRS-AT-3299 | Added "Delete" column in grid, which will help user to delete saved recipient. --%>
                                                <asp:boundcolumn headertext="id." datafield="id" visible="false"  />
												<asp:boundcolumn headertext="SS No." datafield="SSNo" />
												<asp:boundcolumn headertext="Last Name" datafield="LastName" />
												<asp:boundcolumn headertext="First Name" datafield="FirstName" />
		    									<%--Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Commented existing code and added space in header text --%> 
												<%--<asp:boundcolumn headertext="MiddleName" datafield="MiddleName" />--%>
												<%--<asp:boundcolumn headertext="FundStatus" datafield="FundStatus" />--%>
		    									<%--End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Commented existing code and added space in header text --%> 
												<asp:boundcolumn headertext="Middle Name" datafield="MiddleName" />
												<%--<asp:boundcolumn headertext="Fund Status" datafield="FundStatus" />--%> <%--PPP | 01/16/2017 | YRS-AT-3299 | Removed FundStatus, because NonRetired QDRO is not displaying it and Retired QDRO is not utilizing it other than displaying it--%>
												<asp:boundcolumn headertext="FlagNewBenf" datafield="FlagNewBenf" visible="false" />
												<asp:boundcolumn headertext="FundEventID" datafield="FundEventID" visible="false" />
												<asp:boundcolumn headertext="RetireeID" datafield="RecptRetireeID" visible="false" /> <%--PPP | 01/19/2017 | YRS-AT-3299 | Kept retiree id also, so that it will be available to fetch along with persID(represented by id), fundEventID--%>
											</columns>
										</asp:datagrid>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
</iewc:PageView>
<iewc:PageView>
    <!--Start of Account page"-->
    <div class="Div_Center">
        <table width="980"> <%-- PPP | 01/16/2017 | YRS-AT-3299 | Changed width from 698 to 980 --%>
            <tr>
                <td>
                    <table width="980" align="center"> <%-- PPP | 01/16/2017 | YRS-AT-3299 | Changed width from 698 to 980 --%>
                        <tr>
                            <td width="100%">
                                <%--START: PPP | 01/16/2017 | YRS-AT-3299 | Restructured the display of plan links and amount, % options --%>
                               <%--<table width="100%">
                                    < %--'START Chandra sekar -2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098) --% >

                                         <tr>
                                             <td width="20%" colspan="4" align="left"><asp:label id="lblCurrentSplitOptions" runat="server"  cssclass="Label_Small"
                                                             >Split in Progress :</asp:label></td>
                                           
                                        </tr>
                                        <tr>
                                             <td width="25%"  align="left">&nbsp;&nbsp;<asp:label id="LabelHBeneficiarySSno" runat="server" width="200px" text="Recipient SSNo"
                                                    cssclass="Label_Small"></asp:label></td>
                                            <td width="20%" align="left">
                                                 <asp:dropdownlist id="DropdownlistBeneficiarySSNo" runat="server" cssclass="DropDown_Normal"
                                                    width="135px" datatextfield="SSNo" datavaluefield="id" autopostback="true"></asp:dropdownlist>
                                            </td>
                                             
                                            <td width="40%" align="left" colspan="2">
                                                <asp:label id="lblBothPlans" runat="server"  cssclass="Label_Small"
                                                             >Both Plan</asp:label>
                                                                  <asp:linkbutton runat="server" id="lnkButtonBothPlans" autopostback="true">Split Both Plans </asp:linkbutton> <asp:label id="lblBothPlansPerOrAmt" runat="server"   cssclass="Label_Small"
                                                             >(same amount or % for both)</asp:label>

                                            </td>
                                             
                                        </tr>
                                        
                                    <tr>
                                        <td align="left" rowspan="3"> 
                                            <asp:radiobuttonlist id="RadioButtonListSplitAmtType" runat="server" cssclass="RadioButton_Normal">
                                                 
														<asp:ListItem Value="AmountToSplit" onclick="SelectSplitOption('Amount')" >Amount To Split</asp:ListItem>
														<asp:ListItem Value="Percentage" onclick="SelectSplitOption('Percentage')">Percentage</asp:ListItem>
													</asp:radiobuttonlist>
                                        </td>
                                        
                                             
                                                    <td align="left">
                                                       < %-- <asp:label id="LabelHAmount" runat="server" text="Amount" cssclass="Label_Small"
                                                            width="30">Amount</asp:label>--% ><asp:textbox id="TextBoxAmountWorkSheet" runat="server" cssclass="TextBox_Normal"
                                                            text="0.00" width="130px" autopostback="true" enabled="False"></asp:textbox>
                                                    </td>
                                                    
                                               
                                                    <td align="left">
                                                       <asp:label id="lblRetirementPlan" runat="server" text="Retirement Plan" cssclass="Label_Small"
                                                            >Retirement Plan</asp:label>
                                                       <asp:linkbutton runat="server" id="lnkButtonRetirementPlan" autopostback="true">Split Retirement Plan</asp:linkbutton>
                                                    </td>
                                                    <td align="left" width="15%">
                                                      &nbsp;
                                                    </td>
                                                 
                                       
                                       
                                    </tr>
                            <tr>
                                
                                <td align="left">< %--<asp:label id="LabelHPercentage" runat="server" text="Percentage" cssclass="Label_Small"
                                                            width="30">Percentage</asp:label>--% >
                                    <asp:textbox id="TextBoxPercentageWorkSheet" runat="server" cssclass="TextBox_Normal"
                                                            text="0.00" width="130px" autopostback="true" maxlength="6"></asp:textbox>
                                </td>
                                 
                                <td align="left">
                                   <asp:label id="lblSavingsPlan" runat="server" text="Savings Plan" cssclass="Label_Small"
                                                             >Savings Plan</asp:label>
                                      <asp:linkbutton runat="server" id="lnkButtonSavingsPlan" autopostback="true">Split Savings Plan</asp:linkbutton> 
                                </td>
                                 <td>&nbsp;</td>

                            </tr>
                            
                        </table>--%>
                                <table width="100%" align="center">
                                    <tr>
                                        <td width="55%" valign="top">
                                            <table width="75%"> 
                                                <tr style="display: normal" runat="server" id="trPlanInProgressHeader">
                                                    <td colspan="2" style="text-align:left; font-size: 12px">
                                                        Split in progress by <asp:Label id="lblPlanInProgressHeader" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="display: normal" runat="server" id="trPlanInProgressEmptyRow">
                                                    <td colspan="2" style="text-align:left; vertical-align: top;">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width:45%"> 
                                                        <asp:label id="LabelHBeneficiarySSno" runat="server" text="Recipient SSNo"
                                                            cssclass="Label_Small"></asp:label> 
                                                    </td>
                                                    <td align="left">
                                                        <asp:dropdownlist id="DropdownlistBeneficiarySSNo" runat="server" cssclass="DropDown_Normal"
                                                            width="135px" datatextfield="SSNo" datavaluefield="id" autopostback="true"></asp:dropdownlist>
                                                    </td>
                                                </tr>
                                                <tr style="display: normal" runat="server" id="trAmountPercentage">
                                                    <td align="left" colspan="2">
                                                        <table width="100%">
                                                            <tr>
				                                                <td style="width:45%"><input id="RadioButtonListSplitAmtType_Amount" type="radio" name="GroupRadioButtonListSplitAmtType" value="AmountToSplit" onclick="SelectSplitOption('Amount')" runat="server" /><label for="RadioButtonListSplitAmtType_0">Amount </label></td>
                                                                <td>
                                                                    <asp:textbox id="TextBoxAmountWorkSheet" runat="server" cssclass="TextBox_Normal"
                                                                        width="100"></asp:textbox>
                                                                </td>
                                                            </tr>
                                                            <tr>
				                                                <td><input id="RadioButtonListSplitAmtType_Percentage" type="radio" name="GroupRadioButtonListSplitAmtType" value="Percentage" onclick="SelectSplitOption('Percentage')" runat="server" /><label for="RadioButtonListSplitAmtType_1">Percentage</label></td>
                                                                <td>
                                                                    <asp:textbox id="TextBoxPercentageWorkSheet" runat="server" cssclass="TextBox_Normal"
                                                                        width="100" maxlength="6"></asp:textbox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="left" style="vertical-align:top;">
                                            <table class="RadioButton_Normal" >
			                                    <tr style="line-height: 25px; vertical-align: top;">
				                                    <td>
                                                        <span id="spanBothPlans" runat="server">
                                                            <asp:LinkButton runat="server" ID="lnbBothPlans" Text="Split Both Plans" onClientClick="javascript: return SetPlanType('Both');"></asp:LinkButton>
                                                            (same amount or % for both)
                                                        </span>
                                                        <asp:Label id="lblBothPlans" runat="server" Text="Both Plans (same amount or % for both)"></asp:Label>
				                                    </td>
			                                    </tr>
                                                <tr style="line-height: 25px; vertical-align: top;">
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="lnbRetirement" Text="Split Retirement Plan" onClientClick="javascript: return SetPlanType('Retirement');"></asp:LinkButton>
                                                        <asp:Label id="lblRetirement" runat="server" Text="Retirement Plan"></asp:Label>
                                                    </td>
			                                    </tr>
                                                <tr style="line-height: 25px; vertical-align: top;">
                                                    <td>
                                                        <asp:LinkButton runat="server" ID="lnbSavings" Text="Split Savings Plan" onClientClick="javascript: return SetPlanType('Savings');"></asp:LinkButton>
                                                        <asp:Label id="lblSavings" runat="server" Text="Savings Plan"></asp:Label>
                                                    </td>
			                                    </tr>
		                                    </table>
                                            <asp:HiddenField runat="server" ID="hdnSelectedPlanType" Value="" />
                                        </td>
                                    </tr>
                                </table>
                                <%--END: PPP | 01/16/2017 | YRS-AT-3299 | Restructured the display of plan links and amount, % options --%>
                    </td>
                    <%-- <td align="left" width="30%">
                                 <table width="100%" border="1">
                                    <tr>
                                       
                                       </tr>
                                     <tr>
                                        <td></td>
                                      </tr>
                                      <tr>
                                            <td><asp:LinkButton runat="server" id="LinkButtonSavingsPlans" >Split Savings Plan</asp:LinkButton></td>
                                    </tr>
                               <%-- <asp:radiobuttonlist id="RadioButtonListPlanType" runat="server" cssclass="RadioButton_Normal"
                                    autopostback="true">
											<asp:ListItem Value="RETIREMENT">Retirement Plan</asp:ListItem>
											<asp:ListItem Value="SAVINGS">Saving Plan</asp:ListItem>
											<asp:ListItem Value="BOTH" selected="true">Both Plans</asp:ListItem>
										</asp:radiobuttonlist> 
                                
                            </table>
                            </td--%>

                               <%--'END Chandra sekar -2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098) --%>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="980">
                    <div>
                        <table style="width:100%">
                            <tr>
                                <td align="left" class="Td_ButtonContainer">
                                    <asp:checkbox id="CheckBoxSpecialDividends" runat="server" text="Include Special Dividends"></asp:checkbox>
                                </td>
                                <td align="center" class="Td_ButtonContainer">
                                    <asp:button id="ButtonSplit" runat="server" text="Split" cssclass="Button_Normal"
                                        enabled="False" causesvalidation="False"></asp:button>
                                </td>
                                <td align="center" class="Td_ButtonContainer">
                                    <asp:button id="ButtonAdjust" runat="server" text="Adjust" cssclass="Button_Normal"
                                        enabled="False" causesvalidation="False"></asp:button>
                                </td>
                                <td align="center" class="Td_ButtonContainer">
                                    <asp:button id="ButtonReset" runat="server" text="Reset" cssclass="Button_Normal"
                                        enabled="False" causesvalidation="False"></asp:button>
                                </td>
                                <td align="center" class="Td_ButtonContainer">
                                    <asp:button id="ButtonShowBalance" runat="server" text="Show Balance" cssclass="Button_Normal"
                                        enabled="false"></asp:button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left" width="980px"> <%-- PPP | 01/17/2017 | YRS-AT-3299 | Changed width from 768 to 980px --%>
                    <div style="overflow: auto; width: 970px; border-top-style: none; border-right-style: none;
                        border-left-style: none; height: 145px; border-bottom-style: none"> <%-- PPP | 01/17/2017 | YRS-AT-3299 | Changed width from 700px to 970px --%>
                        <asp:datagrid id="DataGridWorkSheet" cssclass="DataGrid_Grid" autogeneratecolumns="false"
                            runat="server" width="100%">
									<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<Columns>
                                        <asp:BoundColumn HeaderText="guiAnnuityID" DataField="guiAnnuityID" visible="false"></asp:BoundColumn>
										<asp:TemplateColumn HeaderText="Selected">
											<ItemTemplate>
												<asp:CheckBox id="CheckBoxSelect" runat="server" checked="True"></asp:CheckBox>
											</ItemTemplate>
										</asp:TemplateColumn>
                                         <asp:TemplateColumn HeaderText="Share Benefit">
											<ItemTemplate>
												<asp:CheckBox id="chkShareBenefit" runat="server" checked="True"></asp:CheckBox>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn HeaderText="Annuity Source Code" DataField="AnnuitySourceCode"></asp:BoundColumn>
										<asp:BoundColumn HeaderText="Plan Type" DataField="PlanType" />
										<asp:BoundColumn HeaderText="Purchase Date" DataField="PurchaseDate" DataFormatString="{0:MM/dd/yyyy}" />
										<asp:BoundColumn HeaderText="Annuity Type" DataField="AnnuityType" />
										<asp:BoundColumn HeaderText="Current Payment" DataField="CurrentPayment" HeaderStyle-HorizontalAlign="Left"
											ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="Emp PreTax Current Payment" DataField="EmpPreTaxCurrentPayment" DataFormatString="{0:F2}"
											HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="Emp PostTax Current Payment" DataField="EmpPostTaxCurrentPayment" DataFormatString="{0:F2}"
											HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="Ymca PreTax Current Payment" DataField="YmcaPreTaxCurrentPayment" DataFormatString="{0:F2}"
											HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="Emp PreTax Remaining Reserves" visible="false" DataField="EmpPreTaxRemainingReserves"
											DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="Emp PostTax Remaining Reserves" visible="false" DataField="EmpPostTaxRemainingReserves"
											DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="Ymca PreTax Remaining Reserves" visible="false" DataField="YmcaPreTaxRemainingReserves"
											DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="SS Levling Amount" DataField="SSLevelingAmt" DataFormatString="{0:F2}"
											HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="SS Reduction Amount" DataField="SSReductionAmt" DataFormatString="{0:F2}"
											HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="SS Reduction Effective Date" DataField="SSReductionEftDate" DataFormatString="{0:MM/dd/yyyy}" />
										
									</Columns>
								</asp:datagrid>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="Right" width="748">
                    <!--
														<asp:datagrid id="DatagridPartTotals" CssClass="DataGrid_Grid" AutoGenerateColumns="false" Runat="server"
															width="92%" showheader="false">
															<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
															<Columns>
																<asp:BoundColumn DataField="AcctType" DataFormatString="{0:F2}">
																	<ItemStyle Width="35px" CssClass="DataGrid_HeaderStyle"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="PersonalPreTax" DataFormatString="{0:F2}">
																	<ItemStyle Width="64px"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="PersonalPostTax" DataFormatString="{0:F2}">
																	<ItemStyle Width="95px"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="PersonalInterestBalance" DataFormatString="{0:F2}">
																	<ItemStyle Width="70px"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="YMCAPreTax" DataFormatString="{0:F2}">
																	<ItemStyle Width="68px"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="YMCAInterestBalance" DataFormatString="{0:F2}">
																	<ItemStyle Width="77px"></ItemStyle>
																</asp:BoundColumn>
																<asp:BoundColumn DataField="TotalTotal" DataFormatString="{0:F2}">
																	<ItemStyle Width="57px"></ItemStyle>
																</asp:BoundColumn>
															</Columns>
														</asp:datagrid>
														-->
                </td>
            </tr>
            <tr>
                <td width="980px">
                    <div>
                        <table width="100%">
                            <tr>
                                <td align="center" width="100%">
                                    <b>Recipient's Annuities</b>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <div style="overflow: auto; width: 970px; border-top-style: none; border-right-style: none;
                                    border-left-style: none; height: 145px; border-bottom-style: none"> <%-- PPP | 01/17/2017 | YRS-AT-3299 | Changed width from 700px to 970px --%>
                                    <asp:datagrid id="DataGridRecipientAnnuitiesBalance" cssclass="DataGrid_Grid" autogeneratecolumns="false"
                                        runat="server">
												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
												<Columns>
													<asp:BoundColumn HeaderText="Recipient" DataField="RecipientPersonID" visible="false"></asp:BoundColumn>
													<asp:BoundColumn HeaderText="Annuity Source Code" DataField="AnnuitySourceCode"></asp:BoundColumn>
													<asp:BoundColumn HeaderText="Plan Type" DataField="PlanType" />
													<asp:BoundColumn HeaderText="Purchase Date" DataField="PurchaseDate" DataFormatString="{0:MM/dd/yyyy}" />
													<asp:BoundColumn HeaderText="Annuity Type" DataField="AnnuityType" />
													<asp:TemplateColumn HeaderText="Current Payment">
														<ItemTemplate>
															<asp:TextBox id="TextboxCurrentPayment" cssclass="TextBox_Normal_AmountRetiree" runat="server"   text='<%#Format(DataBinder.Eval(Container, "DataItem.CurrentPayment"),"0.00")%>' >
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="Emp PreTax Current Payment">
														<ItemTemplate>
															<asp:TextBox id="TextboxEmpPreTaxCurrentPayment" enabled="false" cssclass="TextBox_Normal_AmountRetiree" runat="server"   text='<%#Format(DataBinder.Eval(Container, "DataItem.EmpPreTaxCurrentPayment"),"0.00")%>' >
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="Emp PostTax Current Payment">
														<ItemTemplate>
															<asp:TextBox id="TextboxEmpPostTaxCurrentPayment" enabled="false"  cssclass="TextBox_Normal_AmountRetiree" runat="server"    text='<%#Format(DataBinder.Eval(Container, "DataItem.EmpPostTaxCurrentPayment"),"0.00")%>' >
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="Ymca PreTax Current Payment">
														<ItemTemplate>
															<asp:TextBox id="TextboxYmcaPreTaxCurrentPayment" enabled="false" cssclass="TextBox_Normal_AmountRetiree" runat="server"   text='<%#Format(DataBinder.Eval(Container, "DataItem.YmcaPreTaxCurrentPayment"),"0.00")%>' >
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="Emp PreTax Remaining Reserves" visible="false">
														<ItemTemplate>
															<asp:TextBox id="TextboxEmpPreTaxRemainingReserves" visible="false" cssclass="TextBox_Normal_AmountRetiree" runat="server"   text='<%#Format( DataBinder.Eval(Container, "DataItem.EmpPreTaxRemainingReserves"),"0.00")%>'>
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="Emp PostTax Remaining Reserves" visible="false" >
														<ItemTemplate>
															<asp:TextBox id="TextboxEmpPostTaxRemainingReserves" visible="false" cssclass="TextBox_Normal_AmountRetiree" runat="server"   text='<%#Format(DataBinder.Eval(Container, "DataItem.EmpPostTaxRemainingReserves"),"0.00")%>' >
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="Ymca PreTax Remaining Reserves" visible="false">
														<ItemTemplate>
															<asp:TextBox id="TextboxYmcaPreTaxRemainingReserves" visible="false"  cssclass="TextBox_Normal_AmountRetiree" runat="server"   text='<%#Format( DataBinder.Eval(Container, "DataItem.YmcapreTaxRemainingReserves"),"0.00")%>' >
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="SS Levling Amount">
														<ItemTemplate>
															<asp:TextBox id="TextBoxSSLevlingAmount" enabled="false" cssclass="TextBox_Normal_AmountRetiree" runat="server" text='<%#Format(DataBinder.Eval(Container, "DataItem.SSLevelingAmt"),"0.00")%>' >
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn HeaderText="SS Reduction Amount">
														<ItemTemplate>
															<asp:TextBox id="TextboxSSReductionAmount" enabled="false" cssclass="TextBox_Normal_AmountRetiree" runat="server" text='<%#Format( DataBinder.Eval(Container, "DataItem.SSReductionAmt"),"0.00")%>' >
															</asp:TextBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn HeaderText="SS Reduction Effective Date" DataField="SSReductionEftDate" DataFormatString="{0:MM/dd/yyyy}" />
													<asp:BoundColumn HeaderText="guiAnnuityID" DataField="guiAnnuityID" visible="false"></asp:BoundColumn>
												</Columns>
											</asp:datagrid>
                                </div>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="980px">
                    <div>
                        <table width="100%">
                            <tr>
                                <td align="center" width="100%">
                                    <%--START: 2017.01.28 | MMR | YRS-AT-3299 | Commented existing code and changed text--%>
                                    <%--<b>Participants's Annuities</b>--%>
                                    <b>Participant's Annuities</b>
                                    <%--END: 2017.01.28 | MMR | YRS-AT-3299 | Commented existing code and changed text--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="Label_Small" align="center">
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <div style="overflow: auto; width: 970px; border-top-style: none; border-right-style: none;
                                    border-left-style: none; height: 145px; border-bottom-style: none"> <%-- PPP | 01/17/2017 | YRS-AT-3299 | Changed width from 700px to 970px --%>
                                    <asp:datagrid id="DatagridParticipantsBalance" cssclass="DataGrid_Grid" autogeneratecolumns="false"
                                        runat="server">
												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
												<Columns>
													<asp:BoundColumn HeaderText="Annuity Source Code" DataField="AnnuitySourceCode"></asp:BoundColumn>
													<asp:BoundColumn HeaderText="Plan Type" DataField="PlanType" />
													<asp:BoundColumn HeaderText="Purchase Date" DataField="PurchaseDate" DataFormatString="{0:MM/dd/yyyy}" />
													<asp:BoundColumn HeaderText="Annuity Type" DataField="AnnuityType" />
													<asp:BoundColumn HeaderText="Current Payment" DataField="CurrentPayment" DataFormatString="{0:F2}"
														HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Emp PreTax Current Payment" DataField="EmpPreTaxCurrentPayment" DataFormatString="{0:F2}"
														HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Emp PostTax Current Payment" DataField="EmpPostTaxCurrentPayment" DataFormatString="{0:F2}"
														HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Ymca PreTax Current Payment" DataField="YmcaPreTaxCurrentPayment" DataFormatString="{0:F2}"
														HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Emp PreTax Remaining Reserves" visible="false" DataField="EmpPreTaxRemainingReserves"
														DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Emp PostTax Remaining Reserves" visible="false" DataField="EmpPostTaxRemainingReserves"
														DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Ymca PreTax Remaining Reserves" visible="false" DataField="YmcapreTaxRemainingReserves"
														DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="SS Levling Amount" DataField="SSLevelingAmt" DataFormatString="{0:F2}"
														HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="SS Reduction Amount" DataField="SSReductionAmt" DataFormatString="{0:F2}"
														HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="SS Reduction Effective Date" DataField="SSReductionEftDate" DataFormatString="{0:MM/dd/yyyy}"
														HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="guiAnnuityID" DataField="guiAnnuityID" visible="false"></asp:BoundColumn>
												</Columns>
											</asp:datagrid>
                                </div>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</iewc:PageView>
<iewc:PAGEVIEW>
								<!--Start of Summary page"-->
								<div class="Div_Center">
                                    <table width="980px"> <%-- PPP | 01/17/2017 | YRS-AT-3299 | Changed width from 698 to 980px --%>
                                        <tr>
                                            <td>
                                                <div>
                                                    <table width="980px"> <%-- PPP | 01/17/2017 | YRS-AT-3299 | Changed width from 698 to 980px --%>
                                                        <tr>
                                                            <td align="center">
                                                                <%--START: 2017.01.28 | MMR | YRS-AT-3299 | Commented existing code and changed text--%>
                                                                <%--<b>Participants's Annuities</b>--%>
                                                                <b>Participant's Annuities</b>
                                                                <%--END: 2017.01.28 | MMR | YRS-AT-3299 | Commented existing code and changed text--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" width="980"> <%-- PPP | 01/17/2017 | YRS-AT-3299 | Changed width from 698 to 980px --%>
                                                <div style="overflow: scroll; width: 970px; border-top-style: none; border-right-style: none;
                                                    border-left-style: none; height: 190px; border-bottom-style: none"> <%-- PPP | 01/17/2017 | YRS-AT-3299 | Changed width from 700px to 970px --%>
                                                    <asp:datalist id="DataListParticipant" runat="server" datakeyfield="SSNo">
									<HeaderTemplate>
										<TABLE width="950px"> <%-- PPP | 01/17/2017 | YRS-AT-3299 | Changed width from 698 to 950px --%>
											<TR class="DataGrid_HeaderStyle">
												<TD style="text-align:center">SSNo.
												</TD>
												<TD style="text-align:center">Last Name
												</TD>
												<TD style="text-align:center">First Name
												</TD>
												<TD style="text-align:center">Fund Status
												</TD>
											</TR>
									</HeaderTemplate>
									<ItemTemplate>
										<TR>
											<TD>
												<asp:Label id="lblSSno" Runat="Server" cssclass="Label_Small">
													<%# DataBinder.Eval(Container.DataItem, "SSNo") %>
												</asp:Label></TD>
											<TD>
												<asp:Label id="lblLastNAme" Runat="Server" cssclass="Label_Small">
													<%# DataBinder.Eval(Container.DataItem, "LastName") %>
												</asp:Label></TD>
											<TD>
												<asp:Label id="lbaFirstName" Runat="Server" cssclass="Label_Small">
													<%# DataBinder.Eval(Container.DataItem, "FirstName") %>
												</asp:Label></TD>
											<TD>
												<asp:Label id="Label5" Runat="Server" cssclass="Label_Small" text="Retired"></asp:Label>
											</TD>
										</TR>
										<TR>
											<TD colspan="4">
												<asp:DataGrid id="DatagridSummaryBalList" runat="server" CssClass="DataGrid_Grid" WIDTH="100%"
													AutoGenerateColumns="false">
													<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
													<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
													<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
													<Columns>
														<asp:BoundColumn HeaderText="Annuity Source Code" DataField="AnnuitySourceCode"></asp:BoundColumn>
														<asp:BoundColumn HeaderText="Plan Type" DataField="PlanType" />
														<asp:BoundColumn HeaderText="Purchase Date" DataField="PurchaseDate" DataFormatString="{0:MM/dd/yyyy}" />
														<asp:BoundColumn HeaderText="Annuity Type" DataField="AnnuityType" />
														<asp:BoundColumn HeaderText="Current Payment" DataField="CurrentPayment" DataFormatString="{0:F2}"
															HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
														<asp:BoundColumn HeaderText="Emp PreTax Current Payment" DataField="EmpPreTaxCurrentPayment" DataFormatString="{0:F2}"
															HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
														<asp:BoundColumn HeaderText="Emp PostTax Current Payment" DataField="EmpPostTaxCurrentPayment" DataFormatString="{0:F2}"
															HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
														<asp:BoundColumn HeaderText="Ymca PreTax Current Payment" DataField="YmcaPreTaxCurrentPayment" DataFormatString="{0:F2}"
															HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
														<asp:BoundColumn HeaderText="Emp PreTax Remaining Reserves" DataField="EmpPreTaxRemainingReserves"
															DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
														<asp:BoundColumn HeaderText="Emp PostTax Remaining Reserves" DataField="EmpPostTaxRemainingReserves"
															DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
														<asp:BoundColumn HeaderText="Ymca PreTax Remaining Reserves" DataField="YmcapreTaxRemainingReserves"
															DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
														<asp:BoundColumn HeaderText="SS Levling Amount" DataField="SSLevelingAmt" DataFormatString="{0:F2}"
															HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
														<asp:BoundColumn HeaderText="SS Reduction Amount" DataField="SSReductionAmt" DataFormatString="{0:F2}"
															HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
														<asp:BoundColumn HeaderText="SS Reduction Effective Date" DataField="SSReductionEftDate" DataFormatString="{0:MM/dd/yyyy}" />
													</Columns>
												</asp:DataGrid></TD>
										</TR>
									</ItemTemplate>
									<FooterTemplate>
                                    </table>
                                    </FooterTemplate> </asp:DataList>
                                </div>
					</td>
				</tr>
				<tr>
                    <td width="980"> <%-- PPP | 01/23/2017 | YRS-AT-3299 | Changed width from 698 to 980 --%>
                        <div>
                            <table width="100%">
                                <tr>
                                    <td align="center">
                                        <b>Recipient's Annuities</b>&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td align="right">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
				<tr>
                    <td colspan="4">
                        <div style="overflow: scroll; width: 970px; border-top-style: none; border-right-style: none;
                            border-left-style: none; height: 320px; border-bottom-style: none"> <%-- PPP | 01/17/2017 | YRS-AT-3299 | Changed width from 700px to 970px --%>
                            <asp:datalist id="DatalistBeneficiary" runat="server" datakeyfield="id">
							<HeaderTemplate>
								<TABLE width="950px"> <%-- PPP | 01/17/2017 | YRS-AT-3299 | Changed width from 680 to 950px --%>
									<TR class="DataGrid_HeaderStyle">
										<TD style="text-align:center">SSNo.
										</TD>
										<TD style="text-align:center">Last Name
										</TD>
										<TD style="text-align:center">First Name
										</TD>
										<TD style="text-align:center">Fund Status
										</TD>
									</TR>
							</HeaderTemplate>
							<ItemTemplate>
								<TR>
									<TD>
										<asp:Label id="Label1" Runat="Server" cssclass="Label_Small" style="text-align:center">
											<%# DataBinder.Eval(Container.DataItem, "SSNo") %>
										</asp:Label></TD>
									<TD>
										<asp:Label id="Label2" Runat="Server" cssclass="Label_Small" style="text-align:center">
											<%# DataBinder.Eval(Container.DataItem, "LastName") %>
										</asp:Label></TD>
									<TD>
										<asp:Label id="Label3" Runat="Server" cssclass="Label_Small" style="text-align:center">
											<%# DataBinder.Eval(Container.DataItem, "FirstName") %>
										</asp:Label></TD>
									<TD>
										<asp:Label id="Label4" Runat="Server" cssclass="Label_Small" style="text-align:center">
											<%# DataBinder.Eval(Container.DataItem, "FundStatus") %>
										</asp:Label></TD>
									<TD>
										<asp:Label id="Label6" Runat="Server" cssclass="Label_Small" visible="false">
											<%# DataBinder.Eval(Container.DataItem, "id") %>
										</asp:Label></TD>
								</TR>
								<TR>
									<TD colspan="4">
                                         <asp:Label id="lblRecipientAnnutiesDetails" Runat="Server" cssclass="Label_Small" text=""></asp:Label>
										<%--<asp:DataGrid id="DatagridBeneficiarySummaryBalList" runat="server" CssClass="DataGrid_Grid" WIDTH="100%"
											AutoGenerateColumns="false">
											<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
											<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
											<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
											<Columns>
												<asp:BoundColumn HeaderText="Annuity Source Code" DataField="AnnuitySourceCode"></asp:BoundColumn>
												<asp:BoundColumn HeaderText="Plan Type" DataField="PlanType" />
												<asp:BoundColumn HeaderText="Purchase Date" DataField="PurchaseDate" DataFormatString="{0:MM/dd/yyyy}" />
												<asp:BoundColumn HeaderText="Annuity Type" DataField="AnnuityType" />
												<asp:BoundColumn HeaderText="Current Payment" DataField="CurrentPayment" DataFormatString="{0:F2}"
													HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
												<asp:BoundColumn HeaderText="Emp PreTax Current Payment" DataField="EmpPreTaxCurrentPayment" DataFormatString="{0:F2}"
													HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
												<asp:BoundColumn HeaderText="Emp PostTax Current Payment" DataField="EmpPostTaxCurrentPayment" DataFormatString="{0:F2}"
													HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
												<asp:BoundColumn HeaderText="Ymca PreTax Current Payment" DataField="YmcaPreTaxCurrentPayment" DataFormatString="{0:F2}"
													HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
												<asp:BoundColumn HeaderText="Emp PreTax Remaining Reserves" DataField="EmpPreTaxRemainingReserves"
													DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
												<asp:BoundColumn HeaderText="Emp PostTax Remaining Reserves" DataField="EmpPostTaxRemainingReserves"
													DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
												<asp:BoundColumn HeaderText="Ymca PreTax Remaining Reserves" DataField="YmcapreTaxRemainingReserves"
													DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
												<asp:BoundColumn HeaderText="SS Levling Amount" DataField="SSLevelingAmt" DataFormatString="{0:F2}"
													HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
												<asp:BoundColumn HeaderText="SS Reduction Amount" DataField="SSReductionAmt" DataFormatString="{0:F2}"
													HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
												<asp:BoundColumn HeaderText="SS Reduction Effective Date" DataField="SSReductionEftDate" DataFormatString="{0:MM/dd/yyyy}" />
											</Columns>
										</asp:DataGrid>--%></TD>
								</TR>
							</ItemTemplate>
							<FooterTemplate>
		</table>
		</FooterTemplate>
						</asp:datalist>
                        </div>
                    </td>
                </tr> </table> </div> 

</iewc:PAGEVIEW>
<iewc:PageView>
    <table width="100%">
        <tr>
            <td align="center">
                <b>Recipient's Balances</b>
            </td>
            <td align="right">
                <asp:button cssclass="Button_Normal" id="Button1" text="Split" runat="server"></asp:button>
            </td>
        </tr>
    </table>
</iewc:PageView>
</iewc:multipage>
<div id="divWSMessage"  runat="server" style="display: none;">
    <table width="690px">
        <tr>
            <td valign="top" align="left">
                <span id="spntext" ></span>
            </td>
        </tr>                    
    </table>
</div>
<div class="Div_Center">
    <table width="980px">
        <%--START: PPP | 01/24/2017 | YRS-AT-3299 | Old Save, Cancel and Close buttons replaced by new set of Next, Previous and Close buttons --%>
        <%--<tr>
            <td class="Td_ButtonContainer" align="center">
                <asp:button id="ButtonDocumentSave" width="80" text="Save" runat="server" cssclass="Button_Normal"
                    enabled="False" causesvalidation="False"></asp:button>
            </td>
            <td class="Td_ButtonContainer" align="center">
                <asp:button id="ButtonDocumentCancel" width="80" text="Cancel" runat="server" cssclass="Button_Normal"
                    causesvalidation="false"></asp:button>
            </td>
            <td class="Td_ButtonContainer" align="center">
                <asp:button id="ButtonDocumentOK" width="80" text="Close" runat="server" cssclass="Button_Normal"
                    causesvalidation="false"></asp:button> < %-- CS | 2016.09.17 |YRS-AT-3081 | "OK" Button Text is Changed TO "Close", To be consistent with all other screens--% >
            </td>
        </tr>--%>
        <tr>
            <td class="Td_ButtonContainer" align="center" style="width:33%;">
                <asp:button id="btnPrevious" width="80" text="Previous" runat="server" cssclass="Button_Normal"
                    enabled="False" causesvalidation="False"></asp:button>
            </td>
            <td class="Td_ButtonContainer" align="center" style="width:33%;">
                <asp:button id="btnNext" width="80" text="Next" runat="server" cssclass="Button_Normal"
                    causesvalidation="false"></asp:button>
                <asp:button id="btnSave" width="80" cssclass="Button_Normal" runat="server"
                    text="Save" causesvalidation="false"></asp:button>
            </td>
            <td class="Td_ButtonContainer" align="center" style="width:33%;">
                <asp:button id="btnClose" width="80" text="Close" runat="server" cssclass="Button_Normal Warn_Dirty"
                    causesvalidation="false"></asp:button>
                <asp:button id="btnFinalOK" width="80" text="Ok" runat="server" cssclass="Button_Normal"
                    causesvalidation="false" visible="false"></asp:button>
            </td>
        </tr>
        <%--END: PPP | 01/24/2017 | YRS-AT-3299 | Old Save, Cancel and Close buttons replaced by new set of Next, Previous and Close buttons --%>
    </table>
</div>
    <div id="divBeneficiaryAnnuityDeatils" style="display: none;"> <%-- PPP | 01/24/2017 | YRS-AT-3299 | Changed display property from block to none --%>
        <table>
            <%-- START: MMR | 2017.01.28 | YRS-AT-3299 | Added icon for confirm dialog box --%>
            <tr>
                <td rowspan="2" style="width:10%;">
                    <img src="images/help48.JPG" style="border-width:0px;" alt="information" id="img1" />
                </td>
            </tr>
            <%-- END: MMR | 2017.01.28 | YRS-AT-3299 | Added icon for confirm dialog box --%>
            <tr>     
                <td>
                    <table border="0" cellspacing="1" cellpadding="1" style="border-width: thin">
                        <tr>
                            <td align="left"> <asp:label runat="server" cssclass="Label_Small" id="lblMessage" text="Label"></asp:label>
                            </td>
                        </tr>
                        <tr>
                            <td> &nbsp;</td>
                        </tr>
                         <tr>
                            <td>
                                <asp:panel runat="server" id="pnlBeneficiaryAnnuity">
                                <table border="0" cellspacing="1" cellpadding="1">
                                    <tr>
                                        <td>
                                            <asp:gridview runat="server" id="gvBeneficiaryAnnuityDetails" visible="true" runat="server" cssclass="DataGrid_Grid" cellpadding="0" cellspacing="0" autogeneratecolumns="False">
                        
                                <HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												            <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />												
												            <RowStyle CssClass="DataGrid_NormalStyle" />
												            <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                <Columns>
                       
                                     <asp:BoundField DataField="SSN"  HeaderText="SSN" />
						            <asp:BoundField DataField="Name" HeaderText="Name" />	
                                    <asp:BoundField DataField="AnnuityType" HeaderText="AnnuityType" />
                                   <asp:BoundField DataField="ShareBenefit"  HeaderText="Benefit Shared" />
						            <asp:BoundField DataField="NotShareBenefit" HeaderText="Benefit Not Shared" />	
                                    </Columns>
                                 </asp:gridview>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>

                                </table>
                                    </asp:panel>
                            </td>
                        </tr>
                        <tr runat="server" id="trMissedRecipient">
                            <td align="left"> <asp:label runat="server" cssclass="Label_Small" id="lblMissedRecipient" text=""></asp:label>
                            </td>
                        </tr>
                        <tr runat="server" id="trMissedRecipientEmptyRow">
                            <td> &nbsp;</td>
                        </tr>         
                        <tr runat="server" id="trMultiRecipientQuestion">
                            <td align="left"> <asp:label runat="server" cssclass="Label_Small" id="lblMultiRecipientQuestion" text=""></asp:label>
                            </td>
                        </tr>                      
                    </table>
                </td>
           </tr>      
            <tr>
                <td colspan="2"> <%-- MMR | 2017.01.28 | YRS-AT-3299 | Added colspan to align HR element --%>
                    <hr/>
                </td>
            </tr>
            <tr>
                <td  align="right" colspan="2"> <%-- MMR | 2017.01.28 | YRS-AT-3299 | Added colspan to align Yes/No buttons --%>                   
                   <asp:button runat="server" id="btnOk" width="80px" cssclass="Button_Normal" text="Yes" />&nbsp;&nbsp; <%-- MMR | 2017.01.28 | YRS-AT-3299 | Changed text from 'OK' to 'Yes' --%>
                    <asp:button runat="server" id="btnCancel" width="80px" text="No"  cssclass="Button_Normal" onClientClick="javascript: return CloseConfirmationMessageBox();" /> <%-- MMR | 2017.01.28 | YRS-AT-3299 | Changed text from 'Cancel' to 'No' --%>
                </td>
            </tr>
        </table>
    </div>
</TD></TR></TABLE>
    
    <asp:placeholder id="PlaceHolderACHDebitImportProcess" runat="server"></asp:placeholder>
    </DIV>
<input id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server" value="" />
    <%--START: MMR | 01/09/2017 | YRS-AT-3299--%>
        <table width="980">
            <tr>
                <td width="100%">
                    <YRSControls:YMCA_Footer_WebUserControl ID="YMCA_Footer_WebUserControl1" runat="server"></YRSControls:YMCA_Footer_WebUserControl>
                </td>
            </tr>

        </table>

        <div id="ConfirmDialog" title="YMCA" style="display: none;">
            <div>
                <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom" style="text-align:left;">
                    <tr>
                        <td rowspan="2" style="width:10%;">
                            <img src="images/help48.JPG" style="border-width:0px; display: none;" alt="information" id="imgConfirmDialogInfo" />
                            <img src="images/OK48.JPG" style="border-width:0px; display: none;" alt="OK" id="imgConfirmDialogOk" />
                            <img src="images/error.gif" style="border-width:0px; display: none;" alt="Error" id="imgConfirmDialogError" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divConfirmDialogMessage">
                                <ul>
                                    <li>There is another Recipient Pending for Split</li>
                                    <li>Your split will be finalized, and a Fund ID and balances will be created for the alternate payee. </li>
                                    <li>No fee has been applied.</li>
                                </ul>
                                Do you wish to save?
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr id="trConfirmDialogYesNo">
                        <td align="center" valign="bottom" colspan="2">
                            <%--<input type="button" ID="btnYes" value="Yes" class="Button_Normal" Style="width: 80px;
                                color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                height: 16pt;" />&nbsp;--%>
                            <asp:Button runat="server" ID="btnConfirmDialogYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px;
                                color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                height: 16pt;" causesvalidation="False" />&nbsp;
                            <input type="button" ID="btnConfirmDialogNo" value="No" class="Button_Normal" Style="width: 80px;
                                color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                height: 16pt;" onclick="closeDialog('ConfirmDialog');" />
                        </td>
                    </tr>
                    <tr id="trConfirmDialogOK">
                        <td align="center" valign="bottom" colspan="2">
                            <input type="button" ID="btnConfirmDialogOk" value="OK" class="Button_Normal" Style="width: 80px;
                                color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                height: 16pt;" onclick="closeDialog('ConfirmDialog');" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:HiddenField ID="hdnRecipientForDeletion" Value="" runat="server" />
        <asp:HiddenField ID="HiddenFieldDirty" Value="false" runat="server" /> <%--MMR | 2017.01.09 | YRS-AT-3299 | Added to set value on click of close button to show message for pending operation --%>
</form>
<script type="text/javascript" language="JavaScript">
    <%--START: MMR | 2017.01.10 | YRS-AT-3299 | fucntion to show delete beneficiary confirmation message--%>
    function ConfirmRecipientDelete(ctl) {
        var $row = $(ctl).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements

        if ($tds.length > 0) {
            $('#<%=hdnRecipientForDeletion.ClientID%>').val($tds[2].innerText);
            ShowDialog('QDRO', 'Are you sure, you want to delete ' + $tds[4].innerText + ' ' + $tds[3].innerText + '?', 'infoYesNo');
        }

        return false;
    }
    <%--END: MMR | 2017.01.10 | YRS-AT-3299 | fucntion to show delete beneficiary confirmation message--%>

    $('#<%=TextBoxAmountWorkSheet.ClientID%>').blur(function () {
        EnableDisableSplit();
    });

    $('#<%=TextBoxPercentageWorkSheet.ClientID%>').blur(function () {
        EnableDisableSplit();
    });

    function SetPlanType(plan) {
        $('#<%=hdnSelectedPlanType.ClientID%>').val(plan);
        return true;
    }

    function EnableDisableSplit() {
        var isAmountSelected = $("#<%=RadioButtonListSplitAmtType_Amount.ClientID%>").prop("checked");
        var isPercentageSelected = $("#<%=RadioButtonListSplitAmtType_Percentage.ClientID%>").prop("checked");
        var amount = parseFloat($('#<%=TextBoxAmountWorkSheet.ClientID%>').val());
        var percentage = parseFloat($('#<%=TextBoxPercentageWorkSheet.ClientID%>').val());

        $('#<%=ButtonSplit.ClientID%>').prop('disabled', true);
        if (isAmountSelected && amount != NaN && amount > 0) {
            $('#<%=ButtonSplit.ClientID%>').prop('disabled', false);
        }
        else if (isPercentageSelected && percentage != NaN && percentage > 0) {
            $('#<%=ButtonSplit.ClientID%>').prop('disabled', false);
        }
}
</script>
<%--< ! --# include virtual="bottom.html"-->--%> <%--PPP | 01/16/2017 | YRS-AT-3299 | Not loading footer from bottom.html--%>
