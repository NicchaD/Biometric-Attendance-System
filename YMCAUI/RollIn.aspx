<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master" CodeBehind="RollIn.aspx.vb" Inherits="YMCAUI.RollIn" EnableEventValidation="false" %>

<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="uc2" TagName="RollUserControl" Src="~/UserControls/RolloverInstitution.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        span.Modalpopup {
            display: block;
            width: 85px;
            padding-top: 5px;
        }

        input.Modalpopup, select.Modalpopup {
            width: 200px;
            font-family: Verdana,Tahoma,Arial,Helvetica,sans-serif;
            font-size: 8pt;
        }

        label.Modalpopup {
            font-family: Verdana,Tahoma,Arial,Helvetica,sans-serif;
            font-size: 8pt;
        }

        textarea.Modalpopup {
            width: 260px;
            height: 240px;
            font-family: Verdana,Tahoma,Arial,Helvetica,sans-serif;
            font-size: 8pt;
        }

        submit.Modalpopup {
            width: 80px;
            color: Black;
            font-family: Verdana, Tahoma, Arial;
            font-size: 8pt;
            font-weight: bold;
            height: 16pt;
        }

        .ui-autocomplete {
            max-height: 100px;
            overflow-y: auto;
            /* prevent horizontal scrollbar */
            overflow-x: hidden;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            height: 100px;
        }
    </style>
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

            $('#divRollover').dialog({
                autoOpen: false,
                resizable: false,
                draggable: true,
                closeOnEscape: false,
                close: false,
                width: 680, height: 860,
                title: "Roll In - Add",
                modal: true,
                buttons: [{ text: "Save", click: CheckRolloverValue }, { text: "Close", click: CloseRolloverDialog }],
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

            $('#divReciepts').dialog({
                autoOpen: false,
                resizable: false,
                draggable: false,
                closeOnEscape: false,
                close: false,
                width: 420, height: 650,
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

            $('#divConfirmDialog').dialog({
                autoOpen: false,
                resizable: false,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 400, height: 280,
                title: "Cancel Roll In",
                buttons: [{ text: "Yes", click: CancelRollover }, { text: "No", click: CloseConfirmDialog }],
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
            $('#dvProcessing').dialog({
                autoOpen: false,
                resizable: false,
                draggable: false,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 300, height: 150,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

        }


        function ShowRolloverDialog(type) {
            $('#divRollover').dialog("open");

            if (type == 'AddrsFill') {
                $('#' + '<%=TextBoxAccountNo.ClientID %>').focus();
            } else {
                $("#<%=TextBoxInstitution.ClientID%>" + "_" + "<%=TextBoxInstitution.ID%>").focus();
            }
            //$("#tdNotes").css('display', 'none');
            //$("#tdOrangeLine").css('display', 'none');

            //$($("#tdNotes").closest('td').prev('td').prev('td')).css('width', '490px');
        }

        function CloseRolloverDialog() {
            $('#divRollover').dialog('close');
            $('#<%=hdnCloseRollinDialog.ClientID%>').val('true');
            clear_dirty();
            document.forms(0).submit();
        }
        function ShowProcessingDialog(Message, divTitle) {
            $('#dvProcessing').dialog({ title: divTitle });
            $('#dvProcessing').dialog("open");
            $('#lblProcessing').text(Message);

        }

        function CloseProcessingDialog() {
            $('#dvProcessing').dialog('close');
        }


        function ShowRecieptsDialog(type) {
            if (type == 'Edit') {
                $('#divReciepts').dialog({
                    buttons: [{ text: "Save", click: SaveReceipts }, { text: "Close", click: CloseRecieptsDialog }],
                    title: "Roll In - Receipt / Process"
                });
                $('#trRquiredflds').css('display', 'inline');
                $('#lblRqrdFldChkRcvdDate').css('display', 'inline');
                $('#lblRqrdFldChkNo').css('display', 'inline');
                $('#lblRqrdFldChkDate').css('display', 'inline');
                $('#lblRqrdFldTxamt').css('display', 'inline');
                $('#lblRqrdFldNTxamt').css('display', 'inline');
            }
            else if (type == 'Read') {
                $('#divReciepts').dialog({
                    buttons: [{ text: "Close", click: CloseRecieptsDialog }],
                    title: "Roll In - View Receipt"
                });
                $('#trRquiredflds').css('display', 'none');
                $('#lblRqrdFldChkRcvdDate').css('display', 'none');
                $('#lblRqrdFldChkNo').css('display', 'none');
                $('#lblRqrdFldChkDate').css('display', 'none');
                $('#lblRqrdFldTxamt').css('display', 'none');
                $('#lblRqrdFldNTxamt').css('display', 'none');
            }
            $('#divReciepts').dialog("open");
            if (type == 'Edit') {
                $("#<%=TextboxheckReceivedDate.ClientID%>" + "_TextBoxUCDate").focus();
            }
        }

        function CloseRecieptsDialog() {
            $('#divReciepts').dialog('close');
        }

        function ShowMessage(strMessage, type) {

            $("#divMessage").html(strMessage);
            $("#divMessage").css('display', 'block');
            if (type == "error") {
                $("#divMessage")[0].className = "error-msg";
            }
            else if (type == "success") {
                $("#divMessage")[0].className = "success-msg";
            }
            else if (type == "info") {
                $("#divMessage")[0].className = "info-msg";
            }
        }

        function ShowRolloverMessage(strMessage, type) {

            $("#divRolloverMessage").html(strMessage);
            $("#divRolloverMessage").css('display', 'block');
            if (type == "error") {
                $("#divRolloverMessage")[0].className = "error-msg";
            }
            else if (type == "success") {
                $("#divRolloverMessage")[0].className = "success-msg";
            }
            else if (type == "info") {
                $("#divRolloverMessage")[0].className = "info-msg";
            }
        }

        function SaveReceipts() {
            var strNontaxableAmount = $("#<%=TextboxNonTaxableAmount.ClientID%>")[0].value;
            var strtaxableAmount = $("#<%=TextboxTaxableAmount.ClientID%>")[0].value;
            var strRecievedDate = $("#<%=TextboxheckReceivedDate.ClientID%>" + "_TextBoxUCDate")[0].value;
            var strCheckNo = $("#<%=TextboxCheckNo.ClientID%>")[0].value;
            var strCheckDate = $("#<%=TextboxCheckDate.ClientID%>" + "_TextBoxUCDate")[0].value;
            var strCheckTotal = $("#<%=TextboxCheckTotal.ClientID%>")[0].value;

            if (strRecievedDate == "") {
                ShowMessage("<%=YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ROLLIN_CHECK_RCVD_CANT_BE_BLANK)%>", "error");
            }
            else if (strCheckNo == "") {
                ShowMessage("<%=YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ROLLIN_CHECK_NO_CANT_BE_BLANK)%>", "error");
        }
        else if (!OnBlurTextboxCheckNo()) {
        }
        else if (strCheckDate == "") {
            ShowMessage("<%=YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ROLLIN_CHECK_DATE_CANT_BE_BLANK)%>", "error");
        }
        else if (strtaxableAmount == "") {
            ShowMessage("<%=YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ROLLIN_TAXAMNT_CANT_BE_BLANK)%>", "error");
        }
        else if (strNontaxableAmount == "") {
            ShowMessage("<%=YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ROLLIN_NON_TAXAMNT_CANT_BE_BLANK)%>", "error");
        }
        else if (strCheckTotal == "" || strCheckTotal == "0" || strCheckTotal == "0.00") {
            ShowMessage("<%=YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ROLLIN_CHECKTOTAL_CANT_BE_ZERO)%>", "error");
        }
        else {
            ShowProcessingDialog('<%=YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ROLLIN_SAVING_RECEIPT)%>', 'Roll In Receipt');
            $.ajax({
                type: "POST",
                url: "RollIn.aspx/SaveReceipt",
                data: "{'NonTaxableAmount':'" + strNontaxableAmount + "','TaxableAmount':'" + strtaxableAmount + "','ReceivedDate':'" + strRecievedDate + "','CheckNo':'" + strCheckNo + "','CheckDate':'" + strCheckDate + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var str = msg.d;
                    if (str[0] == "error") {
                        ShowMessage(str[1], str[0]);
                        CloseProcessingDialog();

                    }
                    else if (str[0] == "success") {
                        CloseRecieptsDialog();
                        document.forms(0).submit();
                    }
                },
                failure: function (msg) {
                    ShowMessage(msg.d, "error");
                }
            });
        }
}

function CheckRolloverValue() {
    var strInstitutionName = $("#<%=TextBoxInstitution.ClientID%>" + "_" + "<%=TextBoxInstitution.ID%>")[0].value;
    var strAccountNo = $("#<%=TextBoxAccountNo.ClientID%>")[0].value;
    var strRecievedDate = $("#<%=TextBoxDateRecieved.ClientID%>" + "_TextBoxUCDate")[0].value;
    var strSourceInfo = String($("#<%=drdInfoSource.ClientID%>")[0].selectedIndex);
    $.ajax({
        type: "POST",
        url: "RollIn.aspx/CheckRolloverValues",
        data: "{'InstitutionName':'" + strInstitutionName + "','AccountNo':'" + strAccountNo + "','ReceivedDate':'" + strRecievedDate + "','SourceInfo':'" + strSourceInfo + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var str = msg.d;
            if (str[0] == "error") {
                ShowRolloverMessage(str[1], str[0]);
            }
            else if (str[0] == "success") {
                $("#divRolloverMessage").css('display', 'none');
                ShowProcessingDialog('<%=YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ROLLIN_SAVING_REQUEST)%>', 'Roll In Request');
                CloseRolloverDialog();                
            }
            clear_dirty();
        },
        failure: function (msg) {
            CloseProcessingDialog();
            ShowRolloverMessage(msg.d, "error");
        }
    });

}

function OnBlurTotalAmount() {
    var Taxableamout = new Number;
    var NonTaxableamout = new Number;
    Taxableamout = Number($("#<%=TextboxTaxableAmount.ClientID%>")[0].value);
    NonTaxableamout = Number($("#<%=TextboxNonTaxableAmount.ClientID%>")[0].value);
    $("#<%=TextboxCheckTotal.ClientID%>")[0].value = roundVal(Taxableamout + NonTaxableamout);
    FormatTotalAmtControl("<%=TextboxCheckTotal.ClientID%>")
}

function roundVal(val) {
    var dec = 2;
    var result = Math.round(val * Math.pow(10, dec)) / Math.pow(10, dec);
    return result;
}

function OnBlurTextboxCheckNo() {
    var _arr = new Array(20);
    var flg = false;
    var str = String($("#<%=TextboxCheckNo.ClientID%>")[0].value);

    for (i = 0; i < str.length; i++) {
        _arr[i] = str.substr(i, 1);
    }

    for (i = 0; i < str.length - 1; i++) {
        if (_arr[i] != _arr[i + 1]) {
            flg = true;
        }
    }

    if (!flg) {
        if (str.length > 1) // DY
        {
            ShowMessage("<%=YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ROLLIN_CHECK_NO_SAME)%>", "error");
            return false;
        }
    }
    else {
        $("#divMessage").css('display', 'none');
    }
    return true;
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

function FormatTotalAmtControl(ctl) {
    var vMask;
    var vDecimalAfterPeriod;
    var ctlVal;
    var iPeriodPos;
    var sTemp;
    var iMaxLen
    var ctlVal;
    var tempVal;
    ctl = document.getElementById(ctl);
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

function ValidateAccountNo() {
    if (((event.keyCode > 64) && (event.keyCode < 91)) || ((event.keyCode > 96) && (event.keyCode < 123)) || ((event.keyCode > 47) && (event.keyCode < 58)) || (event.keyCode == 45)) {

        event.returnValue = true;
    }
    else {
        event.returnValue = false;
    }
}

function ShowConfirmDialog(instName, AcctNo) {
    $('#divConfirmDialog').dialog('open');
    $('#lblinstname').text(instName);
    $('#lblAcctNo').text(AcctNo);
    $('#lblMessage').text('<%=YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ROLLIN_CANCELL_ROLLIN)%>');
}

function CancelRollover() {
    $.ajax({
        type: "POST",
        url: "RollIn.aspx/CancelRollover",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            document.forms(0).submit();
        }
    });
}

function CloseConfirmDialog() {
    $.ajax({
        type: "POST",
        url: "RollIn.aspx/ClearRolloverId",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $('#divConfirmDialog').dialog('close');
        }
    });

}

function GetAddress(instName) {
    var Addr1 = $('#' + '<%= ucAddressAdd.ClientID%>' + "_" + 'TextBoxAddress1').val();
    $.ajax({
        type: "POST",
        url: "RollIn.aspx/GetAddress",
        data: "{'strinstID':'" + instName + "','strAddress':'" + Addr1 + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (record) {
            var strResult = record.d
            if (strResult == '1' || Addr1 != '') {
                document.forms(0).submit();
            }
            $('#' + '<%=TextBoxAccountNo.ClientID %>').focus();

            }
        });
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
    function OpenAddressPopUp() {

        OpenAddress('<%= ucAddressAdd.ClientID%>' + '_TextBoxAddress1',
                     '<%= ucAddressAdd.ClientID%>' + '_TextBoxAddress2',
                     '<%= ucAddressAdd.ClientID%>' + '_TextBoxAddress3',
                     '<%= ucAddressAdd.ClientID%>' + '_TextBoxCity',
                     '<%= ucAddressAdd.ClientID%>' + '_DropDownListCountry',
                     '<%= ucAddressAdd.ClientID%>' + '_DropDownListState',
                     '<%= ucAddressAdd.ClientID%>' + '_TextBoxZip',
                     '<%= ucAddressAdd.ClientID%>' + '_CheckboxIsBadAddress',
                     '<%= ucAddressAdd.ClientID%>' + '_TextBoxEffDate',
                     '<%= ucAddressAdd.ClientID%>' + '_txtFinalNotes',
                     '<%= ucAddressAdd.ClientID%>' + '_chkNotes',
                     '<%= ucAddressAdd.ClientID%>' + '_imgBadAddress',
                     '<%= ucAddressAdd.ClientID%>' + '_LabelNoAddressFound',
                     '<%= ucAddressAdd.ClientID%>' + '_LabelUpdateAddress',
                     '<%= ucAddressAdd.ClientID%>' + '_hdnChangesExist',
                     '<%= ucAddressAdd.ClientID%>' + '_hdnReasons_hid',
                     '<%= ucAddressAdd.ClientID%>' + '_QASMatched',
                     '<%= ucAddressAdd.ClientID%>' + '_hasAddress',
                     '<%= ucAddressAdd.ClientID%>' + '_hdnAddressmode'
                    );
        AddressPopUp('930');
        return false;

    }

    function ChooseButtonClick() {
        $('#' + '<%= hdnChooseButton.ClientID%>').val('true');
    }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server" ClientIDMode="Predictable">
    <table class="Table_WithBorder" style="width: 100%; height: 450px">
        <tr style="vertical-align: top;">
            <td align="left" class="td_Text">&nbsp;General
                 <asp:ScriptManagerProxy ID="dbScriptManagerProxy" runat="server">
                 </asp:ScriptManagerProxy>
            </td>
        </tr>
        <tr style="vertical-align: top; height: 100px">
            <td align="left">
                <table width="100%" align="left">
                    <tr style="vertical-align: top;">

                        <td style="width: 11%; text-align: left;">
                            <label id="LabelFundNo" class="Label_Small">Fund No.</label></td>
                        <td style="width: 25%; text-align: left;">
                            <asp:Label ID="lblFundNo" runat="server" CssClass="NormalMessageText"></asp:Label>
                        </td>

                        <td style="width: 7%; text-align: left;">
                            <label id="LabelSSNo" class="Label_Small">SS No.</label>
                        </td>
                        <td align="left" style="width: 23%">
                            <asp:Label ID="lblSSNo" runat="server" CssClass="NormalMessageText"></asp:Label>
                        </td>
                        <td align="left">
                            <label id="labelName" class="Label_Small">Name</label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblName" runat="server" CssClass="NormalMessageText"></asp:Label>
                        </td>


                    </tr>
                    <tr>

                        <td align="left">
                            <label id="LabelMaritalStatus" class="Label_Small">Marital Status</label>
                        </td>
                        <td style="text-align: left;">
                            <asp:Label ID="lblMaritalStatus" runat="server" CssClass="NormalMessageText"></asp:Label>
                        </td>

                        <td style="text-align: left;">
                            <label id="LabelAge" class="Label_Small">Age</label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblAge" runat="server" CssClass="NormalMessageText"></asp:Label>
                        </td>
                        <td align="left">
                            <label id="LabelTelephone" class="Label_Small">Telephone</label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblTelephone" runat="server" CssClass="NormalMessageText"></asp:Label>
                        </td>
                    </tr>
                    <tr style="vertical-align: top;">

                        <td style="text-align: left;">
                            <label id="lblPartAddrs" class="Label_Small">Address</label>
                        </td>
                        <td style="text-align: left;" colspan="3">
                            <NewYRSControls:New_AddressWebUserControl runat="server" PopupHeight="930" ID="AddressWebUserControl1" AllowNote="true" AllowEffDate="false" ClientIDMode="Predictable" EnableControls="False" />
                        </td>

                        <td align="left">
                            <label id="LabelEmail" class="Label_Small">Email Addr.</label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblEmail" runat="server" CssClass="NormalMessageText"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="td_Text" align="left" width="100%">
                <asp:Label ID="LabelEmployment" runat="server">
											Employment</asp:Label>
            </td>
        </tr>

        <tr style="vertical-align: top;">
            <td>
                <div style="OVERFLOW: auto; WIDTH: 100%; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; POSITION: static; BORDER-BOTTOM-STYLE: none; text-align: left;">
                    <asp:GridView ID="gvRollOverEmp" runat="server" Width="100%" CssClass="DataGrid_Grid" AutoGenerateColumns="False">
                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                        <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
                        <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>

                        <Columns>
                            <asp:BoundField DataField="UniqueId" HeaderText="UniqueId" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="PersonId" HeaderText="PersonId" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="YmcaId" HeaderText="YmcaId" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="FundEventId" HeaderText="FundEventId" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="HireDate" HeaderText="Hire Date" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="65px"></asp:BoundField>
                            <asp:BoundField DataField="Termdate" HeaderText="Term Date" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="65px"></asp:BoundField>
                            <asp:BoundField DataField="EligibilityDate" HeaderText="Eligibility Date" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="85px"></asp:BoundField>
                            <asp:BoundField DataField="Professional" HeaderText="Professional" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="Salaried" HeaderText="Salaried" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="FullTime" HeaderText="FullTime" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="PriorService" HeaderText="PriorService" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="StatusType" HeaderText="StatusType" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="StatusDate" HeaderText="StatusDate" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="45px"></asp:BoundField>
                            <asp:BoundField DataField="YmcaName" HeaderText="Ymca Name"></asp:BoundField>
                            <asp:BoundField DataField="YmcaNo" HeaderText="Ymca No." ItemStyle-Width="55px"></asp:BoundField>
                            <asp:BoundField DataField="PositionType" HeaderText="PositionType" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="PositionDesc" HeaderText="PositionDesc" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="BranchId" HeaderText="BranchId" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="BasicPaymentDate" HeaderText="BasicPaymentDate" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="Active" HeaderText="Active" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="BranchName" HeaderText="BranchName" Visible="False"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>


        <tr style="vertical-align: top;">
            <td>
                <asp:UpdatePanel ID="uplRollin" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%;" cellspacing="0">
                            <tr class="td_Text">
                                <td align="left">Roll In(s)
                        
                                </td>
                                <td align="right">
                                    <asp:Button ID="ButtonAddForm" runat="server" Text="Add..." Width="60" CssClass="Button_Normal"
                                        CausesValidation="False"></asp:Button>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td align="left" colspan="2">
                                    <asp:GridView ID="gvRolloverRoll" runat="server" AutoGenerateColumns="false" Width="100%" EmptyDataRowStyle-CssClass="Label_Small" EmptyDataText="No RollIn(s) record(s) found.">
                                        <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                        <RowStyle CssClass="DataGrid_NormalStyle" />
                                        <SortedAscendingHeaderStyle CssClass="sortasc" />
                                        <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                        <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="15px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButtonSel" runat="server" ToolTip="Process a Rollover" CommandName="Select" CausesValidation="False"
                                                        ImageUrl="images\process.gif" AlternateText="Process a Rollover"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="15px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Imagebutton2" runat="server" ImageUrl="images\cancel.gif" CausesValidation="False"
                                                        CommandName="Cancel" ToolTip="Cancel a Rollover"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="UniqueId" HeaderText="UniqueId" />
                                            <asp:BoundField DataField="FundId" HeaderText="FundId" />
                                            <asp:BoundField DataField="Institution Name" HeaderText="Institution Name" />
                                            <asp:BoundField DataField="PartAccno" HeaderText="Participant Account Number" ItemStyle-Width="170px" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="45px" />
                                            <asp:BoundField DataField="Info Source" HeaderText="Info Source" />
                                            <asp:BoundField DataField="InfoSourceCode" HeaderText="InfoSourceCode" />
                                            <%--Start - Manthan Rajguru:2015.09.10 - YRS 2593:Changed label from "Received Date" to "Request Received Date." and "Request Date" to "Request Registered Date."--%>
                                            <asp:BoundField DataField="Received Date" HeaderText="Request Received Date" ItemStyle-Width="85px" />
                                            <asp:BoundField DataField="RequestDate" HeaderText="Request Registered Date" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="85px" />
                                             <%--End - Manthan Rajguru:2015.09.10 - YRS 2593:Changed label from "Received Date" to "Request Received Date." and "Request Date" to "Request Registered Date."--%>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>

        <tr class="Td_ButtonContainer" style="vertical-align: top;">
            <td align="right">
                <asp:Button ID="ButtonCloseForm" CssClass="Button_Normal" Width="80" runat="server" Text="Close" CausesValidation="False"></asp:Button>
            </td>
        </tr>
    </table>


    <div id="divRollover">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="2">
                    <div id="divRolloverMessage" style="width: 98%;">
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: right;">
                    <label class="Modalpopup">
                        <label class="Modalpopup Error_Message">*</label>Required Fields
                    </label>

                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;
                </td>
            </tr>
            <tr valign="top">
                <td align="left">
                    <label id="LabelInstitution" class="Label_Small" style="text-wrap: none;">
                        Institution Name
                                    <label class="Modalpopup Error_Message">*</label>
                    </label>
                </td>
                <td align="left">

                    <uc2:RollUserControl ID="TextBoxInstitution" runat="server" OnSelectEvent="GetAddress(ui.item.value);" />

                </td>
            </tr>
            <tr>
                <td style="text-align: right;" colspan="2">
                    <asp:LinkButton ID="lnkChooseAddress" runat="server" CssClass="Modalpopup Link_SmallBold" Text="Choose Address" OnClientClick="Javascript: ChooseButtonClick();"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkAdd" runat="server" CssClass="Modalpopup Link_SmallBold" Text="Add Address..."></asp:LinkButton>

                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;
                </td>
            </tr>

            <tr valign="top">
                <td style="text-align: left;">
                    <asp:Label ID="lblInstAddrs" runat="server" CssClass="Label_Small">Address</asp:Label>
                </td>
                <td class="Table_WithBorder">
                    <NewYRSControls:New_AddressWebUserControl runat="server" PopupHeight="930" ID="ucAddressAdd" AllowNote="false" AllowEffDate="false" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel ID="updlAddrs" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvAddrs" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" Width="100%"
                                Visible="true" PageSize="4">
                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                <SortedAscendingHeaderStyle CssClass="sortasc" />
                                <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="15px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButtonSelAddrs" runat="server" ToolTip="Select a address" CommandName="Select" CausesValidation="False"
                                                ImageUrl="images\select.gif" AlternateText="Select a address"></asp:ImageButton>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UniqueId" HeaderText="UniqueId" />
                                    <asp:BoundField DataField="addr1" HeaderText="Address" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="addr2" />
                                    <asp:BoundField DataField="addr3" />
                                    <asp:BoundField DataField="city" HeaderText="City" SortExpression="City" ItemStyle-Width="70px" />
                                    <asp:BoundField DataField="StateName" HeaderText="State" SortExpression="StateName" ItemStyle-Width="110px" />
                                    <asp:BoundField DataField="CountryName" HeaderText="Country" SortExpression="CountryName" ItemStyle-Width="90px" />
                                    <asp:BoundField DataField="zipCode" SortExpression="zipCode" HeaderText="Zip" ItemStyle-Width="60px" />
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="15" FirstPageText="First" LastPageText="Last" />

                            </asp:GridView>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvAddrs" EventName="PageIndexChanging" />
                            <asp:AsyncPostBackTrigger ControlID="gvAddrs" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <label class="Label_Small">
                        Participant Acct. No.
                    </label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxAccountNo" runat="server" CssClass="Modalpopup TextBox_Normal" onkeypress="javascript: return ValidateAccountNo();"
                        MaxLength="25" Width="185"></asp:TextBox>
                </td>
            </tr>
            <tr valign="top">
                <td align="left" style="text-wrap: none;">
                    <label id="lblInfoSource" class="Label_Small">
                        Info Source
                                    <label class="Modalpopup Error_Message">*</label>
                    </label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="drdInfoSource" runat="server" CssClass="Modalpopup DropDown_Normal" Width="100%" viewstate="True" />
                </td>
            </tr>
            <tr valign="top">
                <td align="left">
                    <label id="LabelDateRecieved" class="Label_Small">
                        Date Received
                                    <label class="Modalpopup Error_Message">*</label>
                    </label>
                </td>
                <td align="left">
                    <uc1:DateUserControl ID="TextBoxDateRecieved" cssInput="Modalpopup TextBox_Normal" runat="server"></uc1:DateUserControl>
                </td>
            </tr>
            <tr style="vertical-align: top;">
                <td colspan="2" style="text-align: left;">
                    <asp:CheckBox ID="chkdntGenerateLtr" CssClass="Modalpopup CheckBox_Normal" runat="server" Text="Do not generate Letter of Acceptance" TextAlign="Left" Width="100%" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnCloseRollinDialog" runat="server" />
        <asp:HiddenField ID="hdnChooseButton" runat="server" />
    </div>

    <div id="divReciepts" style="width: 25px">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="divMessage" style="width: 98%;">
                </div>
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr id="trRquiredflds" style="vertical-align: top;">
                        <td colspan="2" style="text-align: right;">
                            <label class="Modalpopup">
                                <label class="Modalpopup Error_Message">*</label>Required Fields
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 40%;">
                            <asp:Label ID="lblInstitutionName" runat="server" CssClass="Label_Small">Institution Name</asp:Label></td>
                        <td>
                            <asp:Label ID="lblInstName" runat="server" CssClass="NormalMessageText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAccountNo" runat="server" CssClass="Label_Small">Account No.</asp:Label></td>
                        <td>
                            <asp:Label ID="lblAccNo" runat="server" CssClass="NormalMessageText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblInfoSourc" runat="server" CssClass="Label_Small">Info Source</asp:Label></td>
                        <td>
                            <asp:Label ID="lblInfos" runat="server" CssClass="NormalMessageText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblReceivedDate" runat="server" CssClass="Label_Small">Request Received Date</asp:Label></td> <%-- MMR | 2018.04.30 | YRS-AT-3712 | Changed label text from "Received Date" to "Request Received Date" --%>
                        <td>
                            <asp:Label ID="lblRcvdDate" runat="server" CssClass="NormalMessageText"></asp:Label>
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <asp:Label ID="LabelCheckReceivedDate" runat="server" CssClass="Label_Small">Check Received Date <label class="Modalpopup Error_Message" id="lblRqrdFldChkRcvdDate" >*</label></asp:Label></td>
                        <td>
                            <uc1:DateUserControl ID="TextboxheckReceivedDate" runat="server" cssInput="Modalpopup TextBox_Normal"></uc1:DateUserControl>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelCheckNumber" runat="server" CssClass="Label_Small"> Check Number <label class="Modalpopup Error_Message" id="lblRqrdFldChkNo" >*</label></asp:Label></td>
                        <td>
                            <asp:TextBox ID="TextboxCheckNo" runat="server" CssClass="Modalpopup TextBox_Normal" MaxLength="16" Width="120px" onblur="Javascript:return  OnBlurTextboxCheckNo();" onkeypress="javascript: return ValidateAccountNo();"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="LabelCheckDate" runat="server" CssClass="Label_Small"> Check Date <label class="Modalpopup Error_Message" id="lblRqrdFldChkDate" >*</label></asp:Label></td>
                        <td style="text-align: left;">
                            <uc1:DateUserControl ID="TextboxCheckDate" runat="server" cssInput="Modalpopup TextBox_Normal"></uc1:DateUserControl>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="LabelTaxableAmount" runat="server" CssClass="Label_Small"> Taxable Amount <label class="Modalpopup Error_Message" id="lblRqrdFldTxamt" >*</label></asp:Label></td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="TextboxTaxableAmount" runat="server" CssClass="Modalpopup TextBox_Normal" Width="120px" Style="text-align: right;" MaxLength="15" onchange="Javascript: FormatAmtControl(this);" onblur="Javascript:return  OnBlurTotalAmount();" onkeypress="javascript: return ValidateNumeric(this);"></asp:TextBox></td>
                    </tr>

                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="Label1NonTaxableAmount" runat="server" CssClass="Label_Small"> Non-Taxable Amount <label class="Modalpopup Error_Message" id="lblRqrdFldNTxamt" >*</label></asp:Label></td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="TextboxNonTaxableAmount" runat="server" CssClass="Modalpopup TextBox_Normal" MaxLength="15"
                                AutoPostBack="false" ReadOnly="False" onblur="Javascript:return  OnBlurTotalAmount();" Width="120px" Style="text-align: right;" onchange="Javascript: FormatAmtControl(this);" onkeypress="javascript: return ValidateNumeric(this);"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="LabelCheckTotal" runat="server" CssClass="Label_Small">Check Total</asp:Label></td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="TextboxCheckTotal" runat="server" CssClass="Modalpopup TextBox_Normal" Width="120px" Style="text-align: right;" Enabled="False"></asp:TextBox></td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="divConfirmDialog" title="Cancel Rollover" style="width: 25px">
        <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
            <tr>
                <td class="Label_Small">
                    <label id="lblMessage"></label>
                    <br />
                    <br />

                    Institute Name :
                    <label id="lblinstname" class="Normaltext"></label>
                    <br />
                    Participant Acct. No. :
                    <label id="lblAcctNo" class="Normaltext"></label>
                </td>
            </tr>
        </table>
    </div>
    <div id="dvProcessing" style="width: 25px">
        <label id="lblProcessing" class="Label_Small"></label>
    </div>

</asp:Content>
