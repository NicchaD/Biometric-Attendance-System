<%@ Register TagPrefix="DataPagerFindInfo" TagName="DataGridPager" Src="UserControls/DataGridPager.ascx" %>

<%@ Register TagPrefix="YRSControls" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%--<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl_new.ascx" %>--%>
<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NonRetiredQDRO.aspx.vb"
    Inherits="YMCAUI.NonRetiredQdro" %>

<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%--START: PPP | 11/28/2016 | YRS-AT-3265 --%>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="~/UserControls/YMCA_Footer_WebUserControl.ascx" %>
<%--END: PPP | 11/28/2016 | YRS-AT-3265 --%>

<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<!--#include virtual="top.html"-->


    <%--START: PPP | 11/28/2016 | YRS-AT-3265 --%>
    <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    <script src="JS/jquery-ui/JScript-1.7.2.0.min.js" type="text/javascript"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link id="Link1" href="CSS/CustomStyleSheet.css" type="text/css" runat="server" rel="stylesheet" />
    <%--END: PPP | 11/28/2016 | YRS-AT-3265 --%>
    <script language="javascript" src="JS/YMCA_JScript.js" type="text/javascript"></script>
    <script language="javascript" src="JS/YMCA_JScript_Warn.js" type="text/javascript"></script>

    <title> YMCA YRS</title>
<script type="text/javascript" language="JavaScript">
    <%--START: PPP | 12/28/2016 | YRS-AT-3265 --%>
    function BindEvents() {
        $('#ConfirmDialog').dialog({
            autoOpen: false,
            draggable: true,
            close: false,
            modal: true,
            width: 450, maxHeight: 220,
            title: "YMCA",
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
    <%--END: PPP | 12/28/2016 | YRS-AT-3265 --%>

    <%--START: PPP | 08/29/2016 | YRS-AT-2529 | Following functions are not in use--%>
    <%--
    function _OnBlur_TextBoxAmountWorkSheet() {


        if (isNaN(parseInt(document.Form1.all.TextBoxAmountWorkSheet.value))) {
        //Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
            alert('<%= GetMessageFromResource("MESSAGE_QRDO_AMOUNT_CHARACTERS")%>');
            document.Form1.all.TextBoxAmountWorkSheet.value = 0;
            document.Form1.all.TextBoxAmountWorkSheet.focus();
        }
    }
    function _OnBlur_TextBoxPercentageWorkSheet() {


        if (isNaN(parseInt(document.Form1.all.TextBoxPercentageWorkSheet.value))) {
            //Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
            alert('<%= GetMessageFromResource("MESSAGE_QRDO_PERCENTAGE_CHARACTERS")%>'); 
            document.Form1.all.TextBoxPercentageWorkSheet.value = 0;
            document.Form1.all.TextBoxPercentageWorkSheet.focus();
        }

    }
    --%>
    <%--END: PPP | 08/29/2016 | YRS-AT-2529 | Following functions are not in use--%>
    function ValidateAlphaNumeric() {

        if (((event.keyCode > 64) && (event.keyCode < 91)) || ((event.keyCode > 96) && (event.keyCode < 123)) || ((event.keyCode > 47) && (event.keyCode < 58)) || (event.keyCode == 45)) {

            event.returnValue = true;
        }
        else {
            event.returnValue = false;
        }
    }
    function ValidateNumeric() {
        if ((event.keyCode < 48) || (event.keyCode > 57)) {
            event.returnValue = false;
        }
    }
    function ValidateNumericSSNO(str) {
        if ((event.keyCode < 48) || (event.keyCode > 57)) {

            event.returnValue = false;
        }
        if ((event.keyCode == 80) && str.length == 0) {
            event.returnValue = true;
        }
        if (event.keyCode == 45) {
            event.returnValue = true;
        }

    }
    function ValidateTelephoneNo(str) {
        if (document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value == "US" || document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value == "CA") {
            var val = str.value
            if (((val.length < 10) || (val.length > 10)) && (val.length != 0)) {
                <%--START: PPP | 2015.10.19 | YRS-AT-2588 | Error message replaced with system message text
                alert('<%= GetMessageFromResource("MESSAGE_QDRO_TELEPHONE_LENGTH")%>');--%>
                alert('Please provide valid Telephone number');
                <%--END: PPP | 2015.10.19 | YRS-AT-2588 | Error message replaced with system message text--%>
                str.focus();
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
    function isValidEmail(str) {
        var at = "@"
        var dot = "."
        var lat = str.indexOf(at)
        var lstr = str.length
        var ldot = str.indexOf(dot)
        if (str.length > 0) {
        if (str.indexOf(at) == -1) {
            //Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
            alert('<%= GetMessageFromResource("MESSAGE_QRDO_INVALID_EMAILID")%>')
            return false
        }
        if (str.indexOf(at) == -1 || str.indexOf(at) == 0 || str.indexOf(at) == lstr) {
            //Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
            alert('<%= GetMessageFromResource("MESSAGE_QRDO_INVALID_EMAILID")%>')
            return false
        }
        if (str.indexOf(dot) == -1 || str.indexOf(dot) == 0 || str.indexOf(dot) == lstr) {
            alert('<%= GetMessageFromResource("MESSAGE_QRDO_INVALID_EMAILID")%>') //Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
            return false
        }
        if (str.indexOf(at, (lat + 1)) != -1) {
            alert('<%= GetMessageFromResource("MESSAGE_QRDO_INVALID_EMAILID")%>') //Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
            return false
        }
        if (str.substring(lat - 1, lat) == dot || str.substring(lat + 1, lat + 2) == dot) {
            alert('<%= GetMessageFromResource("MESSAGE_QRDO_INVALID_EMAILID")%>')//Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
            return false
        }
        if (str.indexOf(dot, (lat + 2)) == -1) {
            alert('<%= GetMessageFromResource("MESSAGE_QRDO_INVALID_EMAILID")%>') //Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
            return false
        }
        if (str.indexOf(" ") != -1) {
            alert('<%= GetMessageFromResource("MESSAGE_QRDO_INVALID_EMAILID")%>') //Anudeep:15.02.2012 To get message from resourcefile - BT-1549: Move all Hardcoded Messages to Resource file in Non-Retired QDRO 
            return false
        }
        }
        return true
    }

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
            // neeraj 25102010 BT-637 :if percentage is greater than 100 then make it as 100.
            if (parseFloat(ctl.value) > 100 && ctl.id === '<%= TextBoxPercentageWorkSheet.ClientID %>')
            { ctl.value = "100.00"; }

            ctlVal = ctl.value;

            iPeriodPos = ctlVal.indexOf(".");
            if (iPeriodPos < 0) {
                if (ctl.value.length > (iMaxLen - 3)) {
                    sTemp = ctl.value
                    tempVal = sTemp.substr(0, (iMaxLen - 3)) + ".00";
                }
                else {
                    <%--START: PPP | 09/16/2016 | YRS-AT-2529 | If text box is left blank earlier code was changing it to only ".00". this change will keep "0.00"--%>
                    if (ctlVal == "")
                        ctlVal = "0";
                    <%--END: PPP | 09/16/2016 | YRS-AT-2529 | If text box is left blank earlier code was changing it to only ".00". this change will keep "0.00"--%>
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
                        position: ['center',200],
                        buttons: [{ text: "OK", click: CloseWSMessage }],					    
                        close: function (event, ui) { document.location.href = "FindInfo.aspx?Name=NonRetiredQdro"; } <%--MMR | 2016.11.23 | YRS-AT-3145 | Added to redirect page to find info screen on click od ok button --%>
					});

        <%--START: PPP | 12/29/2016 | YRS-AT-3265 --%>
        $('#chkApplyFees').change(function () {
            EnableDisableTextBox(this);
        });
        <%--END: PPP | 12/29/2016 | YRS-AT-3265 --%>
    });

    function CloseWSMessage() {
        $(document).ready(function () {
            $("#divWSMessage").dialog('close');
        });
    }
    function openDialog(str, type) {
        $(document).ready(function () {
            if (type == 'Bene') {
                <%--START: PPP | 01/04/2017 | YRS-AT-3145 & 3265 --%>
                //str = 'Beneficiary add,edit and delete operation can not be performed due to following reason(s).<br/>' + str
                str = 'Recipient add,edit and delete operation can not be performed due to following reason(s).<br/>' + str
                <%--END: PPP | 01/04/2017 | YRS-AT-3145 & 3265 --%>
            }
            else {
                str = 'QDRO Process can not be performed due to following reasons(s).<br/>' + str;
            }
            $("#divWSMessage").html(str);
            $("#divWSMessage").dialog('open');
            return false;
        });
    }
    //End,Sanjay R:2013.08.05 - YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
</script>


<form id="Form1" method="post" runat="server">

<div class="Div_Center">
    <table class="topbggray" id="Table3" cellspacing="0" cellpadding="0" width="980"> <%--PPP | 11/28/2016 | YRS-AT-3265 | Changed width from 700 to 980--%>
        <tr>
            <td class="Td_BackGroundColorMenu" align="left">
                <cc1:Menu ID="Menu2" runat="server" mouseovercssclass="MouseOver" MenuFadeDelay="2"
                    DefaultMouseUpCssClass="mouseup" DefaultMouseOverCssClass="mouseover" DefaultMouseDownCssClass="mousedown"
                    DefaultCssClass="menuitem" CssClass="menustyle" Cursor="Pointer" HighlightTopMenu="False"
                    Layout="Horizontal">
                    <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
                </cc1:Menu>
            </td>
        </tr>
        
        <tr>
            <td class="Td_HeadingFormContainer" align="left">
                 <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
            </td>
        </tr>
        <%--START: PPP | 12/09/2016 | YRS-AT-2990 | Added div to show negative balance error--%>
        <tr>
            <td style="text-align: left;">
                <div id="DivMainMessage" class="error-msg" runat="server" style="text-align: left; display: none;" enableviewstate="false">
                </div>
                 <%--START: PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Added div to show plan warning, normal waning and success messages--%>
                <div id="DivPlanWarningMessage" class="warning-msg-withRedFont" runat="server" style="text-align: left; display: none;" enableviewstate="false">
                </div>
                <div id="DivWarningMessage" class="warning-msg" runat="server" style="text-align: left; display: none;" enableviewstate="false">
                </div>
                <div id="DivSuccessMessage" class="success-msg" runat="server" style="text-align: left; display: none;" enableviewstate="false">
                </div>
                <%--END: PPP | 12/30/2016 | YRS-AT-3145 & 3265 | Added div to show plan warning, normal waning and success messages--%>
            </td>
        </tr>
        <%--END: PPP | 12/09/2016 | YRS-AT-2990 | Added div to show negative balance error--%>
        <tr>
            <td>
                <iewc:TabStrip ID="QdroMemberActiveTabStrip" runat="server" Height="30px" Width="100%"
                    TabSelectedStyle="background-color:#93BEEE;color:#000000;" TabHoverStyle="background-color:#93BEEE;color:#4172A9;"
                    TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
                    AutoPostBack="True">
                    <%--<iewc:Tab Text="List"></iewc:Tab>--%> <%--MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required instead used existing find info screen to find QDRO person active memeber list --%>
                    <iewc:Tab Text="1. Define Recipient(s)"></iewc:Tab> <%--<iewc:Tab Text="Beneficiary"></iewc:Tab>--%>
                    <iewc:Tab Text="2. View/Split Accounts"></iewc:Tab> <%--<iewc:Tab Text="Accounts"></iewc:Tab>--%>
                    <iewc:Tab Text="3. Manage Fees"></iewc:Tab>
                    <iewc:Tab Text="4. Review & Save"></iewc:Tab> <%--<iewc:Tab Text="Summary"></iewc:Tab>--%>
                    <iewc:Tab Text="5. Status"></iewc:Tab> <%--<iewc:Tab Text="Status"></iewc:Tab>--%>
                </iewc:TabStrip>
            </td>
        </tr>
    </table>
</div>
<div class="Div_Center">
    <table id="Table5" class="Table_WithBorder" cellspacing="0" cellpadding="0" width="980"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 700 to 980--%>
        <tbody>
            <tr>
                <td>
                    <iewc:MultiPage ID="LIstMultiPage" runat="server">
                       <%-- START: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required instead used existing find info screen to find QDRO person active memeber list --%>
                         <%--<iewc:PageView>
                            <!--Start of list page"-->
                            <table width="700">
                                <tr valign="top">
                                    <td height="2px" align="left" font-bold="false">
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td valign="top" align="left" font-bold="false">
                                        <td align="left">
                                            <asp:label id="LabelNoData" runat="server" visible="False" font-bold="false" font-size="X-Small">No Matching Records</asp:label>
                                            <div style="overflow: scroll; width: 470px; border-top-style: none; border-right-style: none;
                                                border-left-style: none; position: static; height: 200px; border-bottom-style: none">
                                                <DataPagerFindInfo:DataGridPager ID="dgPager" runat="server"></DataPagerFindInfo:DataGridPager>
                                                <asp:datagrid id="DataGridList" runat="server" cssclass="DataGrid_Grid" width="470px"
                                                    allowsorting="True" allowpaging="True" pagesize="20">
													<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
													<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
													<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
													<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:ImageButton id="ImageButtonSelect" runat="server" ToolTip="Select" CommandName="Select" CausesValidation="False"
																	ImageUrl="images\select.gif" AlternateText="Select"></asp:ImageButton>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:boundcolumn headertext="PersonID" datafield="PersID" visible="false" />
														<asp:boundcolumn headertext="FundEventID" datafield="FundEventID" visible="false" />
														<asp:boundcolumn headertext="QDRORequestID" datafield="QDRORequestID" visible="false" />
													</Columns>
													<PagerStyle Visible="False" Mode="NumericPages"></PagerStyle>
												</asp:datagrid>
                                            </div>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td align="left">
                                                        <asp:label cssclass="Label_Small" id="LabelFundNoList" text="Fund No." runat="server"></asp:label>
                                                    </td>
                                                    <td>
                                                        <asp:textbox width="100" runat="server" id="TextBoxFundNo" cssclass="TextBox_Normal"
                                                            maxlength="9"></asp:textbox>
                                                    </td>
                                                    <td>
                                                        <asp:label cssclass="Label_Small" id="Label7" text="*" runat="server" forecolor="Red"
                                                            font-bold="True" visible="False"></asp:label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:label cssclass="Label_Small" id="LabelSSNoList" text="SS No." runat="server"></asp:label>
                                                    </td>
                                                    <td>
                                                        <asp:textbox width="100" runat="server" id="TextBoxSSNoList" cssclass="TextBox_Normal" 
                                                            maxlength="11"></asp:textbox> < %-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --% >
                                                    </td>
                                                    <td>
                                                        <asp:label cssclass="Label_Small" id="Label8" text="*" runat="server" forecolor="Red"
                                                            font-bold="True" visible="False"></asp:label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:label cssclass="Label_Small" id="LabelLastNameList" text="Last Name" runat="server"></asp:label>
                                                    </td>
                                                    <td>
                                                        <asp:textbox width="100" runat="server" id="TextBoxLastNameList" cssclass="TextBox_Normal"></asp:textbox> < %-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --% >
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="Label_Small" align="left">
                                                        <asp:label cssclass="Label_Small" id="LabelFirstNameList" text="First Name" runat="server"></asp:label>
                                                    </td>
                                                    <td>
                                                        <asp:textbox width="100" runat="server" id="TextBoxFirstNameList" cssclass="TextBox_Normal"></asp:textbox> < %-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --% >
                                                    </td>
                                                    <td>
                                                        <asp:label cssclass="Label_Small" id="Label10" text="*" runat="server" forecolor="Red"
                                                            font-bold="True" visible="False"></asp:label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="Label_Small" align="left">
                                                        <asp:label cssclass="Label_Small" id="LabelCityList" text="City" runat="server"></asp:label>
                                                    </td>
                                                    <td>
                                                        <asp:textbox width="100" runat="server" id="TextBoxCityList" cssclass="TextBox_Normal"></asp:textbox> < %-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --% >
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="Label_Small" align="left">
                                                        <asp:label cssclass="Label_Small" id="LabelStateList" text="State" runat="server"></asp:label>
                                                    </td>
                                                    <td>
                                                        <asp:textbox width="100" runat="server" id="TextBoxStateList" cssclass="TextBox_Normal"></asp:textbox> < %-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --% >
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
                       <%-- END: MMR | 2016.11.23 | YRS-AT-3145 | Commented existing code as not required instead used existing find info screen to find QDRO person active memeber list --%>
                        <iewc:PageView>
                            <!--Start of Personal"-->
                            <div class="Div_Left">
                                <table width="980px" class="Table_WithOutBorder" cellpadding="0" cellspacing="0"
                                    align="center"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 700 to 980--%>
                                    <tr valign="top">
                                        <td align="center" width="50%">  <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed alight from "left" to "center"--%>
                                            <table> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed align="left"--%>
                                                <tr>
                                                    <td align="left" height="23px" width="83px">
                                                        <asp:label cssclass="Label_Small" id="LabelSSNo" runat="server" text="SSNo"></asp:label> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                        <td align="left" height="23px">
                                                            <asp:textbox cssclass="TextBox_Normal" id="TextBoxSSNo" runat="server" width="200px"
                                                                autopostback="true" maxlength="9"></asp:textbox> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%><%-- PPP | 2017.01.04 | YRS-AT-3145 & 3265 | changed MaxLenght from 11 to 9 --%>
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
                                                            maxlength="6"></asp:textbox> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                </tr>
                                                
                                            </table>
                                        </td>
                                        <td align="left" width="50%">
                                            <table> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed width="100%" align="left"--%>
                                                <tr>
                                                    <td align="left" style="width:132px"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed "nowrap" and replaced it with [style="width:132px"]--%>
                                                        <asp:label cssclass="Label_Small" id="LabelBirthDate" runat="server" text="Birth Date"></asp:label>
                                                    </td>
                                                    <td align="left" height="23px"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed "nowrap" and replaced it with [style="width:23px"]--%>
                                                       <%-- <table>
                                                            <tr>
                                                                <td>--%>
                                                                    <YRSControls:DateUserControl ID="TextBoxBirthDate" runat="server"></YRSControls:DateUserControl>
                                                               <%-- </td>
                                                            </tr>
                                                        </table>--%>
                                                    </td>
                                                </tr>
                                    <%--Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Adding dropdown option for marital status and gender--%> 
                                                <tr>
                                                    <td align="left" height="23px">
                                                        <asp:label cssclass="Label_Small" id="LabelMaritalStatus" runat="server" 
                                                            text="Marital Status"></asp:label> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                    <td align="left" height="23px">
                                                        <asp:dropdownlist Id="DropDownListMaritalStatus" width="140" runat="server" cssclass="DropDown_Normal Warn"
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
                                        <%--End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Adding dropdown option for marital status and gender--%>
                                                <%--Start - MMR | 2016.08.25 |YRS-AT-2488 | Added checkbox for spouse option--%>
                                                <tr>
                                                    <td align="left" height="23px">
                                                            <asp:Label ID="lblSpouse" runat="server" CssClass="Label_Small">Spouse</asp:Label>
                                                    </td>
                                                    <td align="left" height="23px">
                                                            <asp:checkbox id="chkSpouse" runat="server" cssclass ="CheckBox_Normal Warn" checked="true"></asp:checkbox>                                                                  
                                                    </td>
                                                </tr>
                                               <%--Start - MMR | 2016.08.25 |YRS-AT-2488 | Added checkbox for spouse option--%>                                         
                                            </table>
                                        </td>                                       
                                    </tr>
                                    <tr>
                                        <td align="center" width="50%"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed align from left to center--%>
                                            <table> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed align="left"--%>
                                                <tr>
                                                    <td align="left" style="vertical-align:top;width:83px">
                                                            <asp:Label ID="LabelAddress" runat="server" CssClass="Label_Small">Address</asp:Label>
                                                    </td>
                                                    <td align="left" width="225px">
                                                        <NewYRSControls:New_AddressWebUserControl runat="server" ID="AddressWebUserControl1" AllowNote="true" AllowEffDate="true" PopupHeight="530" />        
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="left" width="50%" style="vertical-align:top">
                                            <table width="100%" align="left">
                                                <tr>
                                                    <td align="left" width="132px">
                                                        <asp:label cssclass="Label_Small" runat="server" id="LabelEmail" text="E-mail Addr"></asp:label> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                    <td align="left">
                                                        <asp:textbox cssclass="TextBox_Normal" id="TextBoxEmail" runat="server" width="150px"></asp:textbox> <%-- MMR | 2016.08.23 | YRS-AT-2482 | changed ID Name --%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="77px">
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
                                  </table>
                                    <table width="980" align="center"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 700 to 980--%>
                                        <tr>
                                            <td class="Td_ButtonContainer" align="center" style="width: 25%;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [style="width: 25%;"]--%>
                                                <asp:button id="ButtonAddNewBeneficiary" width="70" cssclass="Button_Normal" runat="server" 
                                                    text="Add New" tooltip="Add New Recipient Information" causesvalidation="False"></asp:button> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed Beneficiary with Recipient--%>
                                            </td>
                                            <td class="Td_ButtonContainer" align="center" style="width: 25%;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [style="width: 25%;"]--%>
                                                <asp:button id="ButtonEditBeneficiary" width="60" cssclass="Button_Normal" runat="server"
                                                    text="Edit" tooltip="Edit New Recipient Information" causesvalidation="False"></asp:button> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed Beneficiary with Recipient--%>
                                            </td>
                                            <td class="Td_ButtonContainer" align="center" style="width: 25%;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [style="width: 25%;"]--%>
                                                <asp:button id="ButtonResetBeneficiary" width="60" cssclass="Button_Normal" runat="server"
                                                    text="Cancel" tooltip="Cancel Add New/Edit" causesvalidation="False"></asp:button>
                                            </td>
                                            <td class="Td_ButtonContainer" valign="bottom" align="center" style="width: 25%;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [style="width: 25%;"] and removed [colspan="4"]--%>
                                                <asp:button id="ButtonAddBeneficiaryToList" width="130" cssclass="Button_Normal" runat="server" onclientclick="Javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxTel);"
                                                    text="Save Recipient" enabled="False" tooltip="Add New Recipient Information to the List"
                                                    causesvalidation="True"></asp:button> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed Text prroperty from "Add To List" to "Save Recipient"--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="width: 100%">
                                                <div align="center" style="overflow: scroll; border-top-style: none; border-right-style: none;
                                                    border-left-style: none; height: 120px; border-bottom-style: none; width:100%;">
                                                    <table style="width:95%">
                                                        <tr>
                                                            <td>
                                                                <asp:datagrid id="DatagridBenificiaryList" cssclass="DataGrid_Grid" autogeneratecolumns="false"
                                                                    runat="server" width="95%">
																		<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																		<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																		<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																		<columns>
																			<asp:TemplateColumn HeaderStyle-Width="2%">
																				<ItemTemplate>
																					<asp:ImageButton id="Imagebutton1" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																						CommandName="Select" ToolTip="Select" AlternateText="Select"></asp:ImageButton>
																				</ItemTemplate>
																			</asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderStyle-Width="2%">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImageDeleteRecipientButton" runat="server" ToolTip="Delete" OnClientClick="javascript: return ConfirmRecipientDelete(this);"
                                                                                        CommandName="Delete" CausesValidation="False" ImageUrl="images\Delete.gif" AlternateText="Delete"></asp:ImageButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
																			<asp:boundcolumn headertext="id." datafield="id" visible="False" />
																			<asp:boundcolumn headertext="SS No." datafield="SSNo" />
																			<asp:boundcolumn headertext="Last Name" datafield="LastName" />
																			<asp:boundcolumn headertext="First Name" datafield="FirstName" />
																			<%--Start - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Commented existing code and added space in header text --%> 
                                                                            <%--<asp:boundcolumn headertext="MiddleName" datafield="MiddleName" />--%>
																			<asp:boundcolumn headertext="Middle Name" datafield="MiddleName" />
																			<%--End - Manthan Rajguru | 2016.08.16 | YRS-AT-2482 | Commented existing code and added space in header text --%> 
																			<asp:boundcolumn headertext="RecpFundEventId" datafield="RecpFundEventId" visible="false" />
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
                                <table width="980px"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 698 to 980--%>
                                    <tr>
                                        <td>
                                            <table width="100%" align="center"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 698 to 100%--%>
                                                <tr>
                                                    <td width="55%" valign="top"> <%--PPP | 08/29/2016 | YRS-AT-2529 | <td width="70%"> changed to <td width="55%" valign="top">--%>
                                                        <table width="75%"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 100% to 75%--%>
                                                            <%--START: PPP | 08/29/2016 | YRS-AT-2529 | Split progress header added--%>
                                                            <tr style="display: normal" runat="server" id="trPlanInProgressHeader">
                                                                <td colspan="2" style="text-align:left; font-size: 12px">
                                                                    Split in progress by <asp:Label id="lblPlanInProgressHeader" runat="server" Text=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <%--END: PPP | 08/29/2016 | YRS-AT-2529 | Split progress header added--%>
                                                            <tr style="display: normal" runat="server" id="trPlanInProgressEmptyRow">
                                                                <td colspan="2" style="text-align:left; vertical-align: top;">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="width:45%"> <%--PPP | 08/29/2016 | YRS-AT-2529 | Added style="width:45%" --%>
                                                                    <asp:label id="LabelHBeneficiarySSno" runat="server" text="Recipient SSNo"
                                                                        cssclass="Label_Small"></asp:label> <%--PPP | 08/29/2016 | YRS-AT-2529 | Removed width="200px" --%> <%--PPP | 01/04/2017 | YRS-AT-3145 & YRS-AT-3265 | Changed "text" from "Beneficiary SSNo" to "Recipient SSNo"--%>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:dropdownlist id="cboBeneficiarySSNo" runat="server" cssclass="DropDown_Normal"
                                                                        width="135px" datatextfield="SSNo" datavaluefield="id" autopostback="true"></asp:dropdownlist>
                                                                </td>
                                                                <%--START: PPP | 08/29/2016 | YRS-AT-2529 | Removed empty column
                                                                <td>
                                                                    &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                                                </td>
                                                                END: PPP | 08/29/2016 | YRS-AT-2529 | Removed empty column--%>
                                                            </tr>
                                                            <%--START: PPP | 08/29/2016 | YRS-AT-2529 | Removed double label which was exists for Amount and Percentage--%>
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
                                                            <%--<tr id="trNeedToDeleteThis" style="display: none">
                                                                <td align="left">
                                                                    <asp : radiobuttonlist id="RadioButtonListSplitAmtType" runat="server" cssclass="RadioButton_Normal"
                                                                        autopostback="true">
																			<asp:ListItem Value="AmountToSplit">Amount To Split</asp:ListItem>
																			<asp:ListItem Value="Percentage" selected="true">Percentage</asp:ListItem>
																		</asp:radiobuttonlist>
                                                                </td>
                                                                <td align="center">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:label id="LabelAmountWorkSheet" runat="server" text="Amount" cssclass="Label_Small"
                                                                                    width="30">Amount</asp:label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:textbox id="TextBoxAmountWorkSheet" runat="server" cssclass="TextBox_Normal"
                                                                                    width="100" enabled="false" autopostback="true"></asp:textbox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:label id="LabelPercentageWorkSheet" runat="server" text="Percentage" cssclass="Label_Small"
                                                                                    width="30">Percentage</asp:label>
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:textbox id="TextBoxPercentageWorkSheet" runat="server" cssclass="TextBox_Normal"
                                                                                    width="100" maxlength="6" enabled="True" autopostback="true"></asp:textbox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>--%>
                                                            <%--END: PPP | 08/29/2016 | YRS-AT-2529 | Removed double label which was exists for Amount and Percentage--%>
                                                            <tr style="display: normal" runat="server" id="trAdjustInterest"> <%--START: PPP | 08/29/2016 | YRS-AT-2529 | Added following attributes: style="display: normal" runat="server" id="trAdjustInterest"--%>
                                                                <td align="left" colspan="2">
                                                                    <asp:checkbox id="ChkAdjustInterest" runat="server" cssclass="CheckBox_Normal" text="Do not adjust interest after QDRO end date"></asp:checkbox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <%--START: PPP | 08/29/2016 | YRS-AT-2529 | Plan type selection is based on link and not through radio button--%>
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
                                                    <%--<td align="left" width="30%" style="display: none" id="tdNeedToDeleteThis" >
                                                        <asp : radiobuttonlist id="RadioButtonListPlanTypes" runat="server" cssclass="RadioButton_Normal"
                                                            autopostback="true">
																<asp:ListItem Value="Retirement">Retirement Plan</asp:ListItem>
																<asp:ListItem Value="Savings">Saving Plan</asp:ListItem>
																<asp:ListItem Value="Both" selected="true">Both Plans</asp:ListItem>
															</asp:radiobuttonlist>
                                                    </td>--%>
                                                    <%--END: PPP | 08/29/2016 | YRS-AT-2529 | Plan type selection is based on link and not through radio button--%>
                                                </tr>
                                            </table>
                                            <table width="100%">
                                                <tr style="height:30px;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [style="height:30px;"]--%>
												<%--2013.10.23 : SP : BT:2257 :Alignment is not proper in QDRO Account Tab Grid column--%>
                                                    <td class="Td_ButtonContainer" align="center" style="width:12%;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed align from "left" to "center" and added [style="width:12%;"]--%>
                                                        <asp:label cssclass="Label_Small" id="LabelBegDate" text="Beginning Date"
                                                            backcolor="#ffcc33" runat="server"></asp:label> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed [width="20pt"]--%>
                                                    </td>
                                                    <td class="Td_ButtonContainer" align="center" style="width:13%;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [style="width:13%;"]--%>
                                                        <asp:textbox cssclass="TextBox_Normal" id="TextboxBegDate" style="width:78px;" runat="server"
                                                            readonly="true" autopostback="true"></asp:textbox> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Replaced [width="50pt"] with [style="width:78px;"]--%>
                                                    </td>
                                                    <td class="Td_ButtonContainer" align="center" style="width:3%;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [style="width:3%;"]--%>
                                                        <rjs:PopCalendar ID="PopcalendarRecDate" runat="server" Separator="/"
                                                            Format="mm dd yyyy" runat="Server"></rjs:PopCalendar> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed [width="40pt"]--%>
                                                    </td>
													<%--2013.10.23 : SP : BT:2257 :Alignment is not proper in QDRO Account Tab Grid column--%>
                                                    <td class="Td_ButtonContainer" align="center" style="width:12%;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed align from "left" to "center" and added [style="width:12%;"]--%>
                                                        <asp:label cssclass="Label_Small" id="LabelEndDate" text="End Date"
                                                            backcolor="#ffcc33" runat="server"></asp:label> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed [width="40pt"]--%>
                                                    </td>
                                                    <td class="Td_ButtonContainer" align="center" style="width:13%;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [style="width:13%;"]--%>
                                                        <asp:textbox cssclass="TextBox_Normal" id="TextboxEndDate" style="width:78px;" runat="server"
                                                            readonly="true" autopostback="true"></asp:textbox> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed [style="width:78px;"]--%>
                                                    </td>
                                                    <td class="Td_ButtonContainer" align="center" style="width:3%;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [style="width:3%;"]--%>
                                                        <rjs:PopCalendar ID="PopcalendarRecDate2" runat="server" Separator="/"
                                                            Format="mm dd yyyy" runat="Server"></rjs:PopCalendar> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed [width="40pt"]--%>
                                                    </td>
                                                    <td class="Td_ButtonContainer" align="center" style="width: 6%;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [style="width:6%;"]--%>
                                                        <asp:button cssclass="Button_Normal" id="ButtonSplit" text="Split" runat="server"
                                                            causesvalidation="False"></asp:button> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed [width="40pt"]--%>
                                                    </td>
                                                    <td class="Td_ButtonContainer" align="center" style="width: 10%;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [style="width:10%;"]--%>
                                                        <asp:button cssclass="Button_Normal" id="ButtonAdjust" text="Adjust"
                                                            runat="server" causesvalidation="False" ></asp:button> <%--PPP | 09/13/2016 | YRS-AT-1973 | Removed 'visible="False"' property--%> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed [width="40pt"]--%>
                                                    </td>
                                                    <td class="Td_ButtonContainer" align="center" style="width: 8%;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [style="width:8%;"]--%>
                                                        <asp:button cssclass="Button_Normal" id="ButtonReset" text="Reset" runat="server"
                                                            autopostback="True" causesvalidation="False" ></asp:button> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed [width="40pt"]--%>
                                                    </td>
                                                    <td class="Td_ButtonContainer" align="center" style="width: 20%;"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [style="width:20%;"]--%>
                                                        <asp:button id="btnShowBalance" cssclass="Button_Normal" runat="server"
                                                            text="Show Balance" enabled="False" causesvalidation="False"></asp:button> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed [width="75pt"]--%>
                                                    </td>
                                                    <!--<asp:Button cssclass="Button_Normal" ID="ButtonSaveQDRO" text="Save" Runat="server"></asp:Button>
											&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-->
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%">  <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 698 to 100%--%>
                                                <tr>
                                                    <td align="center"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed [width="698"]--%>
                                                        <div style="overflow: scroll; width: 100%; border-top-style: none; border-right-style: none;
                                                            border-left-style: none; height: 145px; border-bottom-style: none"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed [width: 698px] to [width: 100%] --%>
                                                            <asp:datagrid id="DataGridWorkSheets" cssclass="DataGrid_Grid" runat="server" headerstyle-horizontalalign="center"
                                                                autogeneratecolumns="false" width="96%">  <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 698 to 96%--%>
																	<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																	<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																	<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																	<Columns>
																		<asp:TemplateColumn HeaderText="Selected" ItemStyle-Width="5%">  <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [ItemStyle-Width="5%"]--%>
																			<ItemTemplate>
																				<asp:CheckBox id="Checkbox1" runat="server" OnCheckedChanged="Check_Clicked" Checked='<%# DataBinder.Eval(Container.DataItem, "selected") %>'>
																				</asp:CheckBox>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:BoundColumn HeaderText="Acct" DataField="AcctType" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                                                        <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
																		<%--<asp:BoundColumn HeaderText="EmpTaxable" DataField="PersonalPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																			ItemStyle-HorizontalAlign="Right" />
																		<asp:BoundColumn HeaderText="EmpNon-Taxable" DataField="PersonalPostTax" DataFormatString="{0:F2}"
																			HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
																		<asp:BoundColumn HeaderText="EmpInterest" DataField="PersonalInterestBalance" DataFormatString="{0:F2}"
																			HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
																		<asp:BoundColumn HeaderText="Total" DataField="PersonalTotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																			ItemStyle-HorizontalAlign="Right" />
																		<asp:BoundColumn HeaderText="YMCATaxable" DataField="YMCAPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																			ItemStyle-HorizontalAlign="Right" />
																		<asp:BoundColumn HeaderText="YMCAInterest" DataField="YMCAInterestBalance" DataFormatString="{0:F2}"
																			HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
																		<asp:BoundColumn HeaderText="Total" DataField="YMCATotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																			ItemStyle-HorizontalAlign="Right" />
																		<asp:BoundColumn HeaderText="Acct Total" DataField="TotalTotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																			ItemStyle-HorizontalAlign="Right" />--%>
                                                                        <asp:BoundColumn HeaderText="Taxable" DataField="PersonalPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																			ItemStyle-HorizontalAlign="Right" />
																		<asp:BoundColumn HeaderText="Non-Taxable" DataField="PersonalPostTax" DataFormatString="{0:F2}"
																			HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
																		<asp:BoundColumn HeaderText="Interest" DataField="PersonalInterestBalance" DataFormatString="{0:F2}"
																			HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
																		<asp:BoundColumn HeaderText="Total" DataField="PersonalTotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																			ItemStyle-HorizontalAlign="Right" />
																		<asp:BoundColumn HeaderText="YMCA Taxable" DataField="YMCAPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																			ItemStyle-HorizontalAlign="Right" />
																		<asp:BoundColumn HeaderText="YMCA Interest" DataField="YMCAInterestBalance" DataFormatString="{0:F2}"
																			HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
																		<asp:BoundColumn HeaderText="Total" DataField="YMCATotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																			ItemStyle-HorizontalAlign="Right" />
																		<asp:BoundColumn HeaderText="Acct. Total" DataField="TotalTotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																			ItemStyle-HorizontalAlign="Right" />
                                                                        <%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
																	</Columns>
																</asp:datagrid>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 698 to 100%--%>
                                            <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
                                            <b>Recipient's Balances</b>
                                            <%--<div>
                                                <table width="698">
                                                    <tr>
                                                        <td align="center">
                                                            <b>Recipient's Balances</b>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>--%>
                                            <%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
                                        </td>
                                    </tr>
                                    <!--<tr>
										<td class="Label_Small" align="center">
											------------------Employee------------- 
											&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
											----------YMCA---------------------
										</td>
									</tr>-->
                                    <tr>
                                        <td align="left" width="100%"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 698 to 100%--%>
                                            <table width="100%"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 698 to 100%--%>
                                                <tr>
                                                    <td align="center">
                                                        <div style="overflow: scroll; width: 100%; border-top-style: none; border-right-style: none;
                                                            border-left-style: none; height: 145px; border-bottom-style: none"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 698 to 100%--%>
                                                            <asp:datagrid id="DataGridWorkSheet2" cssclass="DataGrid_Grid" runat="server" headerstyle-horizontalalign="center"
                                                                autogeneratecolumns="false" itemstyle-horizontalalign="Right" width="96%"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 100% to 96%--%>
																	<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																	<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																	<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																	<Columns>
																		<asp:BoundColumn HeaderText="Acct" DataField="AcctType" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                                                         <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
																		 <%--<asp:TemplateColumn HeaderText="EmpTaxable" HeaderStyle-HorizontalAlign="center">--%>
                                                                         <asp:TemplateColumn HeaderText="Taxable" HeaderStyle-HorizontalAlign="center">
                                                                         <%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
																			<ItemTemplate>
																				<p align="right">
																					<asp:TextBox id="TextboxEmpTaxable" cssclass="TextBox_Normal_AmountRetiree" runat="server" width="100" text='<%# (DataBinder.Eval(Container, "DataItem.PersonalPreTax","{0:0.00}"))%>' onblur="UpdateTotal(this);"></asp:TextBox> <%--PPP | 09/13/2016 | YRS-AT-1973 | Added onblur="UpdateTotal(this);"--%>
																				</p>
																			</ItemTemplate>
																		 </asp:TemplateColumn>
                                                                         <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
																		 <%--<asp:TemplateColumn HeaderText="EmpNon-Taxable" HeaderStyle-HorizontalAlign="center">--%>
                                                                         <asp:TemplateColumn HeaderText="Non-Taxable" HeaderStyle-HorizontalAlign="center">
                                                                         <%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
																			<ItemTemplate>
																				<asp:TextBox id="TextboxEmpNonTaxable" cssclass="TextBox_Normal_AmountRetiree" runat="server" width="100" text='<%# (DataBinder.Eval(Container, "DataItem.PersonalPostTax","{0:0.00}"))%>' onblur="UpdateTotal(this);"></asp:TextBox> <%--PPP | 09/13/2016 | YRS-AT-1973 | Added onblur="UpdateTotal(this);"--%>
																			</ItemTemplate>
																		 </asp:TemplateColumn>
                                                                         <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
																		 <%--<asp:TemplateColumn HeaderText="EmpInterest" HeaderStyle-HorizontalAlign="center">--%>
                                                                         <asp:TemplateColumn HeaderText="Interest" HeaderStyle-HorizontalAlign="center">
                                                                         <%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
																			<ItemTemplate>
																				<asp:TextBox id="TextboxEmpInterest" cssclass="TextBox_Normal_AmountRetiree" runat="server" width="100" text='<%# (DataBinder.Eval(Container, "DataItem.PersonalInterestBalance","{0:0.00}"))%>' onblur="UpdateTotal(this);"></asp:TextBox> <%--PPP | 09/13/2016 | YRS-AT-1973 | Added onblur="UpdateTotal(this);"--%>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:TemplateColumn HeaderText="EmpTotal" HeaderStyle-HorizontalAlign="center">
																			<ItemTemplate>
																				<asp:TextBox id="TextboxEmpTotal" cssclass="TextBox_Normal_AmountRetiree" runat="server" width="100" text='<%# (DataBinder.Eval(Container, "DataItem.PersonalTotal","{0:0.00}"))%>' >
																				</asp:TextBox>
																			</ItemTemplate>
																		</asp:TemplateColumn>
                                                                         <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
																		 <%--<asp:TemplateColumn HeaderText="YMCATaxable" HeaderStyle-HorizontalAlign="center">--%>
                                                                         <asp:TemplateColumn HeaderText="YMCA Taxable" HeaderStyle-HorizontalAlign="center">
                                                                         <%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
																			<ItemTemplate>
																				<asp:TextBox id="TextboxYMCATaxable" cssclass="TextBox_Normal_AmountRetiree" runat="server" width="100" text='<%# (DataBinder.Eval(Container, "DataItem.YMCAPreTax","{0:0.00}"))%>' onblur="UpdateTotal(this);"></asp:TextBox> <%--PPP | 09/13/2016 | YRS-AT-1973 | Added onblur="UpdateTotal(this);"--%>
																			</ItemTemplate>
																		 </asp:TemplateColumn>
																		 <%--<asp:TemplateColumn HeaderText="YMCAInterest" HeaderStyle-HorizontalAlign="center">--%>
                                                                        <asp:TemplateColumn HeaderText="YMCA Interest" HeaderStyle-HorizontalAlign="center">
																			<ItemTemplate>
																				<asp:TextBox id="TextboxYMCAInterest" cssclass="TextBox_Normal_AmountRetiree" runat="server" width="100" text='<%# (DataBinder.Eval(Container, "DataItem.YMCAInterestBalance","{0:0.00}"))%>' onblur="UpdateTotal(this);"></asp:TextBox> <%--PPP | 09/13/2016 | YRS-AT-1973 | Added onblur="UpdateTotal(this);"--%>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:TemplateColumn HeaderText="Total" HeaderStyle-HorizontalAlign="center">
																			<ItemTemplate>
																				<asp:TextBox id="TextboxTotal" cssclass="TextBox_Normal_AmountRetiree" runat="server" width="100" text='<%# (DataBinder.Eval(Container, "DataItem.YMCATotal","{0:0.00}"))%>' >
																				</asp:TextBox>
																			</ItemTemplate>
																		</asp:TemplateColumn>
                                                                         <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
																		 <%--<asp:TemplateColumn HeaderText="Acct Total" HeaderStyle-HorizontalAlign="center">--%>
																		 <asp:TemplateColumn HeaderText="Acct. Total" HeaderStyle-HorizontalAlign="center">
                                                                         <%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
                                                                                <ItemTemplate>
																				<asp:TextBox id="TextboxAcctTotal" cssclass="TextBox_Normal_AmountRetiree" runat="server" width="100" text='<%# (DataBinder.Eval(Container, "DataItem.TotalTotal","{0:0.00}"))%>' ReadOnly="true"></asp:TextBox>  <%--PPP | 09/13/2016 | YRS-AT-1973 | Added 'ReadOnly="true"' property--%>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																	</Columns>
																</asp:datagrid>
                                                        </div>
                                                    </td>
                                                    <!--<td>
														<asp:Label cssclass="Label_Small" ID="Label1" text="Interest" Runat="server"></asp:Label>
														<asp:TextBox CssClass="TextBox_Normal" ID="TextboxInterest" Runat="server" readonly="true"></asp:TextBox>
													</td>-->
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 698 to 100%--%>
                                            <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
                                            <b>Participant's Balances</b>
                                            <%--<div class="Div_Left ">
                                                <table width="698">
                                                    <tr>
                                                        <td align="center">
                                                            <b>Participant's Balances</b>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>--%>
                                            <%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
                                        </td>
                                    </tr>
                                    <!--<tr>
										<td class="Label_Small" align="left" width="768">
											------------------Employee------------- 
											&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
											----------YMCA---------------------
										</td>
									</tr>-->
                                    <tr>
                                        <td align="center" width="100%"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 698 to 100%--%>
                                            <div style="overflow: scroll; width: 100%; border-top-style: none; border-right-style: none;
                                                border-left-style: none; height: 100px; border-bottom-style: none"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 698px to 100%--%>
                                                <asp:datagrid id="DataGridWorkSheet" cssclass="DataGrid_Grid" runat="server" autogeneratecolumns="false"
                                                    width="96%"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 100% to 96%--%>
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<Columns>
                                                            <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
															<%--<asp:BoundColumn HeaderText="Acct" DataField="AcctType" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="EmpTaxable" DataField="PersonalPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																ItemStyle-HorizontalAlign="Right" />
															<asp:BoundColumn HeaderText="EmpNon-Taxable" DataField="PersonalPostTax" DataFormatString="{0:F2}"
																HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
															<asp:BoundColumn HeaderText="EmpInterest" DataField="PersonalInterestBalance" DataFormatString="{0:F2}"
																HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
															<asp:BoundColumn HeaderText="EmpTotal" DataField="PersonalTotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																ItemStyle-HorizontalAlign="Right" />
															<asp:BoundColumn HeaderText="YMCATaxable" DataField="YMCAPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																ItemStyle-HorizontalAlign="Right" />
															<asp:BoundColumn HeaderText="YMCAInterest" DataField="YMCAInterestBalance" DataFormatString="{0:F2}"
																HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
															<asp:BoundColumn HeaderText="Total" DataField="YMCATotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																ItemStyle-HorizontalAlign="Right" />
															<asp:BoundColumn HeaderText="Acct Total" DataField="TotalTotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																ItemStyle-HorizontalAlign="Right" />
															<asp:TemplateColumn HeaderText="Selected">
																<ItemTemplate>
																	<asp:CheckBox id="CheckBoxSelect" runat="server" autopostback="true"></asp:CheckBox>
																</ItemTemplate>
															</asp:TemplateColumn>--%>
                                                            <asp:BoundColumn HeaderText="Acct" DataField="AcctType" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Taxable" DataField="PersonalPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																ItemStyle-HorizontalAlign="Right" />
															<asp:BoundColumn HeaderText="Non-Taxable" DataField="PersonalPostTax" DataFormatString="{0:F2}"
																HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
															<asp:BoundColumn HeaderText="Interest" DataField="PersonalInterestBalance" DataFormatString="{0:F2}"
																HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
															<asp:BoundColumn HeaderText="EmpTotal" DataField="PersonalTotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																ItemStyle-HorizontalAlign="Right" />
															<asp:BoundColumn HeaderText="YMCA Taxable" DataField="YMCAPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																ItemStyle-HorizontalAlign="Right" />
															<asp:BoundColumn HeaderText="YMCA Interest" DataField="YMCAInterestBalance" DataFormatString="{0:F2}"
																HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
															<asp:BoundColumn HeaderText="Total" DataField="YMCATotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																ItemStyle-HorizontalAlign="Right" />
															<asp:BoundColumn HeaderText="Acct. Total" DataField="TotalTotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																ItemStyle-HorizontalAlign="Right" />
															<asp:TemplateColumn HeaderText="Selected">
																<ItemTemplate>
																	<asp:CheckBox id="CheckBox2" runat="server" autopostback="true"></asp:CheckBox>
																</ItemTemplate>
															</asp:TemplateColumn>
                                                            <%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
														</Columns>
													</asp:datagrid>
                                            </div>
                                        </td>
                                    </tr>
                                    <!--<tr>
										&nbsp;&nbsp;&nbsp;
										<td align="right">
											<asp:Button cssclass="Button_Normal" ID="btnReset" text="Reset" Runat="server"></asp:Button>
										</td>
									</tr>-->
                                </table>
                            </div>
                        </iewc:PageView>
                        <%-- START: MMR | 2016.12.02 | YRS-AT-3145 | Added design for new tab manage Fees --%>
                        <iewc:PageView>
				                <table width="100%" cellpadding="10">                                   
					                <tr>
						                <td align="right">
                                            <asp:CheckBox id="chkApplyFees" runat="server" Text ="Add Fee"></asp:CheckBox>																				
						                </td>
					                </tr>
                                    <%--<tr>
						                <td align="center"><B>Balances</B></td>
                                    </tr> --%>                                  
				                </table>
                            <div id="divBalances" class="Div_Center" width="980px" runat="server" style="padding:5px;">
                                <table width="100%" border="0" cellpadding ="10">
                                    <tr>
						                <td align="center"><B>Balances</B></td>
                                    </tr>  
                                </table>
                           </div>
                            <div id="divApplyFees" class="Div_Center" width="980px" runat="server" style="padding:5px;">
                                <table width="100%" border="0" cellpadding ="10">
                                    <tr>
						                <td align="center"><B>Apply Fees</B></td>
                                    </tr>  
                                </table>
                           </div>
                        </iewc:PageView>
                        <%-- END: MMR | 2016.12.02 | YRS-AT-3145 | Added design for new tab manage Fees --%>
                        <iewc:PageView>
                            <!--Start of Summary page"-->
                            <div class="Div_Center">
                                <table width="980px"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 698 to 100%--%>
                                    <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
                                    <tr style="height: 20px">
                                        <td align="center" style="width: 100%;">
                                            <b>Participant's Balances</b>
                                        </td>
                                        <%--<td>
                                            <div>
                                                <table width="698">
                                                    <tr>
                                                        <td align="center">
                                                            <b>Participant's Balances</b>&nbsp;&nbsp;&nbsp;&nbsp; < %--PPP | 09/21/2016 | YRS-AT-2529 | Changed "Participants's" to "Participant's" --% >
                                                        </td>
                                                        <td align="right">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>--%>
                                        <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
                                    </tr>
                                    <tr>
                                        <td> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Removed [colspan="4" width="698"]--%>
                                            <div style="overflow: scroll; width: 100%; border-top-style: none; border-right-style: none;
                                                border-left-style: none; height: 180px; border-bottom-style: none"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 700px to 100%, and height 150px to 180px --%>
                                                <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
                                                <div id="divParticipantTable" runat="server"></div>

                                                <%--<asp:datalist id="DataListParticipant" runat="server" datakeyfield="SSNo" Width="98%">
														<HeaderTemplate>
															<table width="100%">
																<TR class="DataGrid_HeaderStyle">
																	<TD align="center" style="width:25%">SSNo.
																	</TD>
																	<TD align="center" style="width:25%">Last Name
																	</TD>
																	<TD align="center" style="width:25%">First Name
																	</TD>
																	<TD align="center" style="width:25%">Fund Status
																	</TD>
																</TR>
                                                            </table>
														</HeaderTemplate>
														<ItemTemplate>
															<table width="100%">
															<tr>
																<td align="center" style="width:25%">
																	<asp:Label id="lblSSno" Runat="Server" cssclass="Label_Small">
																		<%# DataBinder.Eval(Container.DataItem, "SSNo") %>
																	</asp:Label></td>
																<td align="center" style="width:25%">
																	<asp:Label id="lblLastNAme" Runat="Server" cssclass="Label_Small">
																		<%# DataBinder.Eval(Container.DataItem, "LastName") %>
																	</asp:Label></td>
																<td align="center" style="width:25%">
																	<asp:Label id="lbaFirstName" Runat="Server" cssclass="Label_Small">
																		<%# DataBinder.Eval(Container.DataItem, "FirstName") %>
																	</asp:Label></td>
																<td align="center" style="width:25%">
																	<asp:Label id="Label5" Runat="Server" cssclass="Label_Small" text="Active"></asp:Label></td>
															</tr>
															<tr>
																<td colspan="4">
																	<asp:DataGrid id="DatagridSummaryBalList" runat="server" CssClass="DataGrid_Grid" WIDTH="100%"
																		AutoGenerateColumns="false">
																		<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																		<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																		<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																		<Columns>
																			<asp:BoundColumn HeaderText="Acct" DataField="AcctType" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
																				<ItemStyle Width="100px"></ItemStyle>
																			</asp:BoundColumn>
																			< %--<asp:BoundColumn HeaderText="EmpTaxable" DataField="PersonalPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																				ItemStyle-HorizontalAlign="Right" />
																			<asp:BoundColumn HeaderText="EmpNon-Taxable" DataField="PersonalPostTax" DataFormatString="{0:F2}"
																				HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
																			<asp:BoundColumn HeaderText="EmpInterest" DataField="PersonalInterestBalance" DataFormatString="{0:F2}"
																				HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
																			<asp:BoundColumn HeaderText="YMCATaxable" DataField="YMCAPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																				ItemStyle-HorizontalAlign="Right" />
																			<asp:BoundColumn HeaderText="YMCAInterest" DataField="YMCAInterestBalance" DataFormatString="{0:F2}"
																				HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
																			<asp:BoundColumn HeaderText="Acct Total" DataField="TotalTotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																				ItemStyle-HorizontalAlign="Right" />--% >
                                                                            <asp:BoundColumn HeaderText="Taxable" DataField="PersonalPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																				ItemStyle-HorizontalAlign="Right" />
																			<asp:BoundColumn HeaderText="Non-Taxable" DataField="PersonalPostTax" DataFormatString="{0:F2}"
																				HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
																			<asp:BoundColumn HeaderText="Interest" DataField="PersonalInterestBalance" DataFormatString="{0:F2}"
																				HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
																			<asp:BoundColumn HeaderText="YMCA Taxable" DataField="YMCAPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																				ItemStyle-HorizontalAlign="Right" />
																			<asp:BoundColumn HeaderText="YMCA Interest" DataField="YMCAInterestBalance" DataFormatString="{0:F2}"
																				HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
																			<asp:BoundColumn HeaderText="Acct. Total" DataField="TotalTotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
																				ItemStyle-HorizontalAlign="Right" />
																		</Columns>
																	</asp:DataGrid></td>
															</tr>
                                                            </table>
														</ItemTemplate>
												</asp:DataList>--%>
                                                <%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>

                            </div>
                </td>
            </tr>
            <tr style="height: 20px"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Added [style="height: 20px"]--%>
                <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
                <td align="center" style="width: 100%;">
                    <b>Recipient's Balances</b>
                </td>
                <%--<td width="698">
                    <div>
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <b>Recipient's Balances</b>&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td align="right">
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>--%>
                <%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
            </tr>
            <tr>
                <td colspan="4">
                    <div style="overflow: scroll; width: 100%; border-top-style: none; border-right-style: none;
                        border-left-style: none; height: 220px; border-bottom-style: none"> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Changed width from 700px to 100% and height 32px to 220px--%>
                        <%--START: PPP | 08/29/2016 | YRS-AT-2529 | Summary report HTML is generated through code behind and gets assigned to divBeneficiaryTable--%>
                        <div id="divBeneficiaryTable" runat="server"></div>
                        <%--<asp:datalist id="DatalistBeneficiary" runat="server" datakeyfield="id">
								<HeaderTemplate>
									<TABLE width="700">
										<TR class="DataGrid_HeaderStyle">
											<TD align="center">SSNo.
											</TD>
											<TD align="center">Last Name
											</TD>
											<TD align="center">First Name
											</TD>
											<TD align="center">Fund Status
											</TD>
										</TR>
								</HeaderTemplate>
								<ItemTemplate>
									<TR>
										<TD align="center">
											<asp:Label id="Label1" Runat="Server" cssclass="Label_Small">
												<%# DataBinder.Eval(Container.DataItem, "SSNo") %>
											</asp:Label></TD>
										<TD align="center">
											<asp:Label id="Label2" Runat="Server" cssclass="Label_Small">
												<%# DataBinder.Eval(Container.DataItem, "LastName") %>
											</asp:Label></TD>
										<TD align="center">
											<asp:Label id="Label3" Runat="Server" cssclass="Label_Small">
												<%# DataBinder.Eval(Container.DataItem, "FirstName") %>
											</asp:Label></TD>
										<TD align="center">
											<asp:Label id="Label6" Runat="Server" cssclass="Label_Small" text="QDRO"></asp:Label></TD>
										<TD align="center">
											<asp:Label id="Label4" Runat="Server" cssclass="Label_Small" visible="false">
												<%# DataBinder.Eval(Container.DataItem, "id") %>
											</asp:Label></TD>
									</TR>
									<TR>
										<TD colspan="4">
											<asp:DataGrid id="DatagridBeneficiarySummaryBalList" runat="server" CssClass="DataGrid_Grid" WIDTH="100%"
												AutoGenerateColumns="false">
												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
												<Columns>
													<asp:BoundColumn HeaderText="Acct" DataField="AcctType" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
														<ItemStyle Width="100px"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn HeaderText="EmpTaxable" DataField="PersonalPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
														ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="EmpNon-Taxable" DataField="PersonalPostTax" DataFormatString="{0:F2}"
														HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="EmpInterest" DataField="PersonalInterestBalance" DataFormatString="{0:F2}"
														HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="YMCATaxable" DataField="YMCAPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
														ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="YMCAInterest" DataField="YMCAInterestBalance" DataFormatString="{0:F2}"
														HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Acct Total" DataField="TotalTotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
														ItemStyle-HorizontalAlign="Right" />
												</Columns>
											</asp:DataGrid></TD>
									</TR>
								</ItemTemplate>
								<FooterTemplate>
    </table>
    </FooterTemplate> </asp:DataList>--%>
                        <%--END: PPP | 08/29/2016 | YRS-AT-2529 | Summary report HTML is generated through code behind and gets assigned to divBeneficiaryTable--%>
</div>
</td> </tr> </table> </div> </iewc:PageView>
<iewc:PageView>
    <!--Start of status page"-->
    <table class="Table_WithBorder" width="100%" height="350" cellspacing="0">
        <tr>
            <td align="left" valign="top">
                <table class="Table_WithOutBorder" width="100%" cellspacing="0">
                    <tr>
                        <td align="left" class="td_Text">
                            QDRO Status
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <div style="width: 100%; height: 100%;">
                                <input type="image" name="Imagebutton3" id="Image5" src="images\tick.jpg" />
                                <asp:label id="lblQDROStatus" runat="server" visible="True" font-bold="true" font-size="Small"
                                    class="Label_Large"></asp:label>
                                <br />
                                <br />
                                <!--Anudeep:18.12.2012  Bt-1523-YRS 5.0-1753 Wording change in QDRO settlement page -->
                                <asp:label id="lblMessage" runat="server" visible="True" font-bold="false" font-size="Small"
                                    runat="server" cssclass="Label_Medium"></asp:label>
                                <br />
                                <asp:label id="lblWarning" runat="server" visible="True" font-bold="true" font-size="Small"
                                    runat="server" cssclass="Label_Medium"></asp:label>
                                <br />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 100%; border-top-style: none; border-right-style: none; border-left-style: none;
                                position: static; height: 100%; border-bottom-style: none">
                                <asp:gridview id="gvQDROStatus" runat="server" cssclass="DataGrid_Grid" width="670px"
                                    autogeneratecolumns="false" datakeynames="SSNo" style="width: 100%;">                                                                       
                                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
													            <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
													            <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
													            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                         <Columns>
                                                                            <asp:BoundField  HeaderText="SSN" datafield="SSNO" />
                                                                            <asp:BoundField  HeaderText="First Name" datafield="FirstName" />
                                                                            <asp:BoundField  HeaderText="Middle Name" datafield="MiddleName" />		
								                                            <asp:BoundField  HeaderText="Last Name" datafield="LastName" />																				
								                                            <asp:BoundField  HeaderText="Refund Request" datafield="RefundRequest" />                                                                                                                                                                                                                       															
                                                                            <asp:TemplateField HeaderText="Form & Letter">
                                                                                <ItemTemplate>
                                                                                        <asp:ImageButton ID="ImgForm" runat="server" ToolTip="Print Form & Letter" CausesValidation="False" CommandName="PrintForm" ImageUrl="images\details.gif" CommandArgument='<%# Container.DataItemIndex %>'/>
                                                                                 </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                             <%--<asp:TemplateField HeaderText="Letter">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImgLetter" runat="server" ImageUrl="images\mylist2.gif" CommandName="PrintLetter" CommandArgument='<%# Container.DataItemIndex %>'/>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>--%>
                                                                            <asp:BoundField headertext="Action" datafield="Action" />      
                                                                            <asp:BoundField headertext="RefRequestId" datafield="RefRequestId" visible = "false" ControlStyle-Width="0%"> </asp:BoundField >  
                                                                            <asp:BoundField headertext="ReportName" datafield="ReportName" visible = "false" ControlStyle-Width="0%">  </asp:BoundField >                                                                                 
                                                                           </Columns>
                                                             </asp:gridview>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <div style="width: 100%; height: 100%;">
                                <asp:label id="lblNotes1" runat="server" align="left" visible="True" font-bold="True"
                                    font-size="small" class="Label_Small"><br/> Notes:-</asp:label>
                                <br />
                                <asp:label id="lblNotes2" runat="server" align="left" visible="True" font-bold="false"
                                    font="bold 8px Verdana,Arial,Helvetica" class="Label_Small" runat="server"></asp:label>
                                <br />
                                <br />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Td_ButtonContainer">
                            <asp:button id="btnStatusOK" width="80" cssclass="Button_Normal" runat="server" text="OK"
                                causesvalidation="False"></asp:button>
                        </td>
                    </tr>
                </table>
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
    <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
    <table id="tblButtons" width="980px" runat="server"> 
        <tr>
            <td class="Td_ButtonContainer" align="center" style="width:33%;">
                <asp:button id="btnPrevious" width="80" cssclass="Button_Normal" runat="server"
                    text="Previous" onClientClick="CollectFees()"></asp:button>
            </td>
            <td class="Td_ButtonContainer" align="center" style="width:33%;">
                <asp:button id="btnNext" width="80" cssclass="Button_Normal" runat="server"
                    text="Next" causesvalidation="False" onClientClick="CollectFees()"></asp:button>
                <asp:button id="ButtonDocumentSave" width="80" cssclass="Button_Normal" runat="server"
                    text="Save" visible="false"></asp:button>
            </td>
            <td class="Td_ButtonContainer" align="center" style="width:33%;">
                <asp:button id="btnClose" width="80" cssclass="Button_Normal Warn_Dirty" runat="server"
                    text="Close" causesvalidation="False"></asp:button>
            </td>
        </tr>
    </table>
    <%--<table id="tblButtons" width="700" runat="server">
        <tr>
            <td class="Td_ButtonContainer" align="center">
                <asp:button id="ButtonDocumentSave" width="80" cssclass="Button_Normal" runat="server"
                    text="Save"></asp:button>
            </td>
            <td class="Td_ButtonContainer" align="center">
                <asp:button id="ButtonDocumentCancel" width="80" cssclass="Button_Normal" runat="server"
                    text="Cancel" causesvalidation="False"></asp:button>
            </td>
            <td class="Td_ButtonContainer" align="center">
                <asp:button id="ButtonDocumentOK" width="80" cssclass="Button_Normal" runat="server"
                    text="Close" causesvalidation="False"></asp:button> < %--PPP | 08/29/2016 | YRS-AT-2529 | Changed text="Ok" to text="Close" to be consistent with all other screens--% >
            </td>
        </tr>
    </table>--%>
    <asp:HiddenField ID="HiddenFieldDirty" Value="false" runat="server" /> <%--MMR | 2016.12.02 | YRS-AT-3145 | Added to set value on click of close button to show message for pending operation --%>
    <asp:HiddenField ID="hdnFees" Value="" runat="server" /> <%--MMR | 2016.12.02 | YRS-AT-3145 | Added to set value on click of close button to show message for pending operation --%>
    <%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
</div>
<input id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server" value="" />
</TD></TR></TBODY></TABLE><asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder></DIV>
        <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
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
</form>
<%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
<%--START: PPP | 08/29/2016 | YRS-AT-2529 --%>
<script type="text/javascript" language="JavaScript">
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
        var isDateRangeDefined = $('#<%=TextboxEndDate.ClientID%>').val() != '';

        $('#<%=ButtonSplit.ClientID%>').prop('disabled', true);

        if (isDateRangeDefined) {
            if (isAmountSelected && amount != NaN && amount > 0) {
                $('#<%=ButtonSplit.ClientID%>').prop('disabled', false);
            }
            else if (isPercentageSelected && percentage != NaN && percentage > 0) {
                $('#<%=ButtonSplit.ClientID%>').prop('disabled', false);
            }
        }
    }

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

    <%--START: PPP | 09/13/2016 | YRS-AT-1973 --%>
    function UpdateTotal(control) {
        var isCurrentRow = false;
        var empNonTaxable, empTaxable, empInterest, ymcaNonTaxable, ymcaInterest;
        var table = $('#<%=DataGridWorkSheet2.ClientID%>');
        table.find('tr').each(function (i, el) {
            var $tds = $(this).find('input');

            if ($tds.length > 0) {
                if (control.id == $tds.eq(0).attr('id')) {
                    isCurrentRow = true;
                }
                else if (control.id == $tds.eq(1).attr('id')) {
                    isCurrentRow = true;
                }
                else if (control.id == $tds.eq(2).attr('id')) {
                    isCurrentRow = true;
                }
                else if (control.id == $tds.eq(3).attr('id')) {
                    isCurrentRow = true;
                }
                else if (control.id == $tds.eq(4).attr('id')) {
                    isCurrentRow = true;
                }

                if (isCurrentRow) {
                    empNonTaxable = parseFloat($tds.eq(0).val());
                    empTaxable = parseFloat($tds.eq(1).val());
                    empInterest = parseFloat($tds.eq(2).val());
                    ymcaNonTaxable = parseFloat($tds.eq(3).val());
                    ymcaInterest = parseFloat($tds.eq(4).val());

                    $.ajax({
                        type: "POST",
                        url: "NonRetiredQDRO.aspx/GetUIAdjustedRowTotal",
                        data: '{"empNonTaxable":"' + empNonTaxable + '", "empTaxable":"' + empTaxable + '", "empInterest":"' + empInterest + '", "ymcaNonTaxable":"' + ymcaNonTaxable + '", "ymcaInterest":"' + ymcaInterest + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            var result = response.d;

                            $tds.eq(5).val(result);
                        }
                    });

                }
            }
        });
    }
    <%--END: PPP | 09/13/2016 | YRS-AT-1973 --%>

    <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
    function CollectFees() {
        var participantRetirementFees, participantSavingsFees;
        var recipientRetirementFees, recipientSavingsFees;
        var recipientCounter;
        var recipientFees;// = [];

        if ($('#txtParticipantRetirementFee').length || $('#txtParticipantSavingsFee').length) {

            participantRetirementFees = 0; 
            if ($('#txtParticipantRetirementFee').length) {
                participantRetirementFees = $('#txtParticipantRetirementFee').val();
            }

            participantSavingsFees = 0;
            if ($('#txtParticipantSavingsFee').length) {
                participantSavingsFees = $('#txtParticipantSavingsFee').val();
            }

            recipientCounter = 1;
            recipientFees = '';
            while (true) {
                if ($('#txtRecipientRetirementFee' + recipientCounter).length || $('#txtRecipientSavingsFee' + recipientCounter).length) {
                    recipientRetirementFees = 0;
                    if ($('#txtRecipientRetirementFee' + recipientCounter).length) {
                        recipientRetirementFees = $('#txtRecipientRetirementFee' + recipientCounter).val();
                    }

                    recipientSavingsFees = 0;
                    if ($('#txtRecipientSavingsFee' + recipientCounter).length) {
                        recipientSavingsFees = $('#txtRecipientSavingsFee' + recipientCounter).val();
                    }

                    recipientFees = recipientFees + recipientCounter + '-' + recipientRetirementFees + '-' + recipientSavingsFees + ',';
                }
                else
                    break;
                recipientCounter++;
            }

            //alert('participant fees: ' + participantRetirementFees + ' - ' + participantSavingsFees);
            //alert('Recipient fees: ' + recipientFees);
            $('#<%=hdnFees.ClientID%>').val(participantRetirementFees + '-' + participantSavingsFees + '|' + recipientFees);
        }
    }

    function MaintainFeeTotal() {
        var participantRetirementFees, participantSavingsFees;
        var recipientRetirementFees, recipientSavingsFees;
        var total, retirementTotal, savingsTotal;
        var recipientCounter;

        if ($('#txtParticipantRetirementFee').length || $('#txtParticipantSavingsFee').length) {
            total = 0;
            participantRetirementFees = parseFloat(0);
            if ($('#txtParticipantRetirementFee').length) {
                if ($('#txtParticipantRetirementFee').val() != '')
                    participantRetirementFees = parseFloat($('#txtParticipantRetirementFee').val());
            }

            participantSavingsFees = parseFloat(0);
            if ($('#txtParticipantSavingsFee').length) {
                if ($('#txtParticipantSavingsFee').val() != '')
                    participantSavingsFees = parseFloat($('#txtParticipantSavingsFee').val());
            }

            $('#lblParticipantTotalFee').text(parseFloat(participantRetirementFees + participantSavingsFees).toFixed(2));
            total = participantRetirementFees + participantSavingsFees;
            retirementTotal = participantRetirementFees;
            savingsTotal = participantSavingsFees;
          
            recipientCounter = 1;
            recipientFees = '';
            while (true) {
                if ($('#txtRecipientRetirementFee' + recipientCounter).length || $('#txtRecipientSavingsFee' + recipientCounter).length) {
                    recipientRetirementFees = parseFloat(0);
                    if ($('#txtRecipientRetirementFee' + recipientCounter).length) {
                        if ($('#txtRecipientRetirementFee' + recipientCounter).val() != '')
                            recipientRetirementFees = parseFloat($('#txtRecipientRetirementFee' + recipientCounter).val());
                    }

                    recipientSavingsFees = parseFloat(0);
                    if ($('#txtRecipientSavingsFee' + recipientCounter).length) {
                        if ($('#txtRecipientSavingsFee' + recipientCounter).val() != '')
                            recipientSavingsFees = parseFloat($('#txtRecipientSavingsFee' + recipientCounter).val());
                    }

                    $('#lblRecipientTotalFee' + recipientCounter).text(parseFloat(recipientRetirementFees + recipientSavingsFees).toFixed(2));
                    total += recipientRetirementFees + recipientSavingsFees;
                    retirementTotal += recipientRetirementFees;
                    savingsTotal += recipientSavingsFees;
                }
                else
                    break;

                recipientCounter++;
            }

            $('#lblRetirementTotalFees').text(parseFloat(retirementTotal).toFixed(2));
            $('#lblSavingsTotalFees').text(parseFloat(savingsTotal).toFixed(2));
            $('#lblTotalFees').text(parseFloat(total).toFixed(2));
        }
    }

    function EnableDisableTextBox(chk) {
        var recipientCounter;

        if (!$(chk).is(":checked")) {
            if ($('#txtParticipantRetirementFee').length) {
                $('#txtParticipantRetirementFee').attr("disabled", "disabled");
                $('#txtParticipantRetirementFee').val("0.00");
            }
            if ($('#txtParticipantSavingsFee').length) {
                $('#txtParticipantSavingsFee').attr("disabled", "disabled");
                $('#txtParticipantSavingsFee').val("0.00");
            }
            $('#lblParticipantTotalFee').text("0.00");

            recipientCounter = 1;
            while (true) {
                if ($('#txtRecipientRetirementFee' + recipientCounter).length || $('#txtRecipientSavingsFee' + recipientCounter).length) {
                    if ($('#txtRecipientRetirementFee' + recipientCounter).length) {
                        $('#txtRecipientRetirementFee' + recipientCounter).attr("disabled", "disabled");
                        $('#txtRecipientRetirementFee' + recipientCounter).val("0.00");
                    }
                    if ($('#txtRecipientSavingsFee' + recipientCounter).length) {
                        $('#txtRecipientSavingsFee' + recipientCounter).attr("disabled", "disabled");
                        $('#txtRecipientSavingsFee' + recipientCounter).val("0.00");
                    }
                    $('#lblRecipientTotalFee' + recipientCounter).text("0.00");
                }
                else
                    break;
                recipientCounter++;
            }

            $('#lblRetirementTotalFees').text("0.00");
            $('#lblSavingsTotalFees').text("0.00");
            $('#lblTotalFees').text("0.00");
        }
        else {
            if ($('#txtParticipantRetirementFee').length) {
                $('#txtParticipantRetirementFee').removeAttr("disabled");
                $('#txtParticipantRetirementFee').val($('#hdnParticipantDefaultRetirementFee').val());
            }
            if ($('#txtParticipantSavingsFee').length) {
                $('#txtParticipantSavingsFee').removeAttr("disabled");
                $('#txtParticipantSavingsFee').val($('#hdnParticipantDefaultSavingsFee').val());
            }
            $('#lblParticipantTotalFee').text($('#hdnParticipantDefaultTotalFee').val());

            recipientCounter = 1;
            while (true) {
                if ($('#txtRecipientRetirementFee' + recipientCounter).length || $('#txtRecipientSavingsFee' + recipientCounter).length) {
                    if ($('#txtRecipientRetirementFee' + recipientCounter).length) {
                        $('#txtRecipientRetirementFee' + recipientCounter).removeAttr("disabled");
                        $('#txtRecipientRetirementFee' + recipientCounter).val($('#hdnRecipientDefaultRetirementFee' + recipientCounter).val());
                    }
                    if ($('#txtRecipientSavingsFee' + recipientCounter).length) {
                        $('#txtRecipientSavingsFee' + recipientCounter).removeAttr("disabled");
                        $('#txtRecipientSavingsFee' + recipientCounter).val($('#hdnRecipientDefaultSavingsFee' + recipientCounter).val());
                    }
                    $('#lblRecipientTotalFee' + recipientCounter).text($('#hdnRecipientDefaultTotalFee' + recipientCounter).val());
                }
                else
                    break;
                recipientCounter++;
            }
            $('#lblRetirementTotalFees').text($('#hdnDefaultRetirementTotalFees').val());
            $('#lblSavingsTotalFees').text($('#hdnDefaultSavingsTotalFees').val());
            $('#lblTotalFees').text($('#hdnDefaultTotalFees').val());
        }
    }

    function ConfirmRecipientDelete(ctl) {
        var $row = $(ctl).closest("tr"),       // Finds the closest row <tr> 
        $tds = $row.find("td");             // Finds all children <td> elements

        if ($tds.length > 0) {
            $('#<%=hdnRecipientForDeletion.ClientID%>').val($tds[2].innerText);
            ShowDialog('QDRO', 'Are you sure, you want to delete ' + $tds[4].innerText + ' ' + $tds[3].innerText + '?', 'infoYesNo');
        }

        return false;
    }
    <%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>

</script>
<%--END: PPP | 08/29/2016 | YRS-AT-2529 --%>
<%--< ! --# include virtual="bottom.html"-->--%> <%--PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265 | Not loading footer from bottom.html--%>
