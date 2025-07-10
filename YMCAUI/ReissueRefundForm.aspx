<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReissueRefundForm.aspx.vb"
    Inherits="YMCAUI.ReissueRefundForm" %>

<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="uc2" TagName="RollUserControl" Src="~/UserControls/RolloverInstitution.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<!--#include virtual="top.html"-->
<script src="JS/jquery-1.5.1.min.js" type="text/javascript"></script>
<script src="JS/YMCA_JScript.js" type="text/javascript"></script>

<%--Start:SR:2015.10.01- YRS-AT-2501 - Reissue Withdrawal screen, allow rollovers of taxable only. Here, New Jquery version has been added to support Rollover Payee intellisense function --%>
<script  src='<%= Me.ResolveClientUrl("~/JS/jquery-1.7.2.min.js") %>' type="text/javascript"></script>
<script src='<%= Me.ResolveClientUrl("~/JS/jquery-ui/jquery-ui-1.8.20.custom.min.js") %>' type="text/javascript"></script>
<link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
<style id="Style1" type="text/css" media="screen" runat="server">
    @import '<%= ResolveUrl("~/JS/jquery-ui/base/jquery.ui.all.css")%>';
</style>
<%--End:SR:2015.10.01- YRS-AT-2501 - Reissue Withdrawal screen, allow rollovers of taxable only. Here, New Jquery version has been added to support Rollover Payee intellisense function--%>

<script language="javascript">
    //This function will fire when the control leaves the Text Box.
    //The function is responsible for formating the numbers to amount type


    var $lblMsg = null;

    jQuery.fx.off = true;

    function FormatAmtControlTwoDecimal(ctl) {
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

    //BS:2011.10.10----Start Calculate Additional and Existing Detail

    function HandleDeductionChange() {
        alertMessage('');
        HandleDataChanged();
    }
    function GetSelectedDeductions(gridValue) {
        var l_Decimal_Amount = 0.0;
        // get input control from datagrid like checkbox.
        var inputs = $("input:checked", gridValue)
        var tr;
        var strDeduction;
        //count length of checked value from checkboxes
        for (var i = 0; i < inputs.length; i++) {
            //get closest tr of checked value
            tr = $(inputs[i]).closest("TR");
            //get text value from td which is column no. 3 of table  
            strDeduction = $("TD:nth-child(3)", tr).text();
            l_Decimal_Amount = l_Decimal_Amount + parseFloat(strDeduction);
        }
        return l_Decimal_Amount;
    }

    //this method is used for Additional Deduction
    function GetAdditionalDeduction(array_MRD, array_payee1, array_payee2) {
        //here get exist deduction value from GetSelectedDeductions() if exist
        var dblExistDeductionAmt = GetSelectedDeductions($("#<%=dgExistingDeductions.ClientID%>"));
        //here get Additional deduction value from GetSelectedDeductions() if exist
        var dblAdditDeductionAmt = GetSelectedDeductions($("#<%=DataGridDeductions.ClientID%>"));
        var DeductionsAmount = dblExistDeductionAmt + dblAdditDeductionAmt;

        //here assign net amount of payee1,payee2,mrd from respectively arrays
        var dblPayee1NetAmt = array_payee1[4];
        var dblPayee2NetAmt = array_payee2[4];
        var dblMrdDeduction = array_MRD[4];

        // Apply Deduction to one of the Payee records
        if (DeductionsAmount < dblPayee1NetAmt) {
            adjustDeductionFromArray(array_payee1, DeductionsAmount)
        } else if (DeductionsAmount < dblMrdDeduction) {
            adjustDeductionFromArray(array_MRD, DeductionsAmount)
        } else if (DeductionsAmount < dblPayee2NetAmt) {
            adjustDeductionFromArray(array_payee2, DeductionsAmount)
        } else {
            alertMessage('Unable to adjust deductions from any record');
            if (array_payee1[0] + array_payee1[1] > 0) {
                adjustDeductionFromArray(array_payee1, DeductionsAmount)
            } else if (array_MRD[0] + array_MRD[1] > 0) {
                adjustDeductionFromArray(array_MRD, DeductionsAmount)
            } else if (array_payee1[0] + array_payee1[1] > 0) {
                adjustDeductionFromArray(array_payee2, DeductionsAmount)
            }
            return 1;
        }
        return 0;
    }
    function alertMessage(msg) {
        if (msg == '') {
            $lblMsg = null;
            $("#lblMsg").text(msg).css('top', $("#lblMsg").outerHeight() * -1);
 
        } else {
            $lblMsg = $('#lblMsg');
            $lblMsg.show().width($('body').width()).css('left',0);
            $lblMsg.text(msg).animate({ top: $('body').scrollTop() }, 500);
        }
    }
    function adjustDeductionFromArray(arr, deductionAmt) {
        var dblNet = 0.0;
        dblNet = arr[4] - deductionAmt;
        arr[4] = dblNet; arr[5] = deductionAmt;
    }
    //BS:2011.10.10----End Calculate Additional and Existing Detail

    function AssignDataToTextBoxes(array_MRD, array_payee1, array_payee2) {
        AssignValuesIntoTextBoxs('TextBoxTaxable', 'TextBoxNonTaxable', 'TextBoxRate', 'TextBoxTax', 'TextBoxNet', 'TextBoxDeductions', array_payee1);
        AssignValuesIntoTextBoxs('TextBoxTaxable1', 'TextBoxNonTaxable1', 'TextBoxRate1', 'TextBoxTax1', 'TextBoxNet1', 'TextBoxDeductions1', array_payee2);
        AssignValuesIntoTextBoxs('TextBoxMRDTaxable', 'TextBoxMRDNonTaxable', 'TextBoxMRDRate', 'TextBoxMRDTax', 'TextBoxMRDNet', 'TextBoxMRDDeductions', array_MRD);
    }
    //BS"2011.10.12--Start------handling withholding checkbox
    function EnableDisableWithholdings_Click(ctrl) {
        
        //if CheckBoxWithholding is yes then prorate payee1,payee2,Mrd  textboxes
        //if CheckBoxWithholding is no then reset the payee1,payee2,Mrd  textboxes
        <%--Start/SR/2015.09.30 :YRS-AT-2501- Following lines of code are commented as it is not supported in latest Jquery version--%>
        //var bFlag = $(ctrl).attr('checked');  
        var bFlag;
        if ($(ctrl).is(':checked')) {
            bFlag = true;
        }
        else { bFlag = false }
        <%--End/SR/2015.09.30 :YRS-AT-2501- Following lines of code are commented as it is not supported in latest Jquery version--%>

        if (ctrl.id == 'CheckBoxWithholdingNo') {
            bFlag = !bFlag;
        }
        if (bFlag) {
            $('#CheckBoxWithholdingNo').removeAttr('checked');
        } else {
            $('#CheckBoxWithholdingYes').removeAttr('checked');
            $('#CheckBoxWithholdingNo').attr('checked', 'checked');
        }
        HandleWithholdingControls();
        HandleDataChanged();
    }

    function HandleWithholdingControls() {
        <%--Start/SR/2015.09.30 :YRS-AT-2501- Following lines of code are commented as it is not supported in latest Jquery version--%>
        //here assigned CheckBoxWithholdingYes checked value into chkValue variable
        //var bEnable = $('#CheckBoxWithholdingYes').attr('checked');
        //check if chkValue is true then we will remove disabled and readonly property from rate textboxes
        //if (bEnable == true) {        
        if ($("#CheckBoxWithholdingYes").is(':checked')) {
        <%--End/SR/2015.09.30 :YRS-AT-2501- Following lines of code are commented as it is not supported in latest Jquery version--%>
            $("#TextBoxMRDRate, #TextBoxRate").disableControl(false);
        } else {
            $('#TextBoxMRDRate').val(10);
            //Start - Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code and assigning configurable tax rate to property
            //$('#TextBoxRate').val(20);
            $('#TextBoxRate').val(<%=Me.TextBoxRate.Text%>);
            //End - Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code and assigning configurable tax rate to property
            $("#TextBoxMRDRate, #TextBoxRate").disableControl(true);
        }
    }

    //BS:2011.10.12--End------handling withholding checkbox

    //BS:2011.10.12--Start----handling rollover checkbox
    function CheckBoxRolloverYesNo_Click(ctrl) {
        //if CheckBoxRollover is yes then prorate payee1,payee2,Mrd datatable and textboxes
        //if CheckBoxRollover is no then reset the datatable and textboxes
       
        <%--Start-SR/2015.09.30:YRS-AT-2501- Following line of code are commented as it is not supported in latest Jquery version(1.7.2) and New line of code is added--%>
        //var bFlag = $(ctrl).attr('checked'); 
        var bFlag;
        if ($(ctrl).is(':checked')) {
            bFlag = true;
        }
        else { bFlag = false }
        <%--End-SR/2015.09.30:YRS-AT-2501- Following line of code are commented as it is not supported in latest Jquery version(1.7.2) and New line of code is added--%>
        if (ctrl.id == 'CheckBoxRolloverNo') {
            bFlag = !bFlag;
        }
        if (bFlag) {
            $('#CheckBoxRolloverNo').removeAttr('checked');          
            DisplayRolloveroption(true); <%--2015.09.15:SR-YRS-AT-2501-Handle visibility of Taxable only option--%>
        } else {
            $('#CheckBoxRolloverYes').removeAttr('checked');
            $('#CheckBoxRolloverNo').attr('checked', 'checked');           
            DisplayRolloveroption(false);  <%--2015.09.15:SR-YRS-AT-2501-Handle visibility of Taxable only option--%>
        }
        HandleRolloverControls();
        HandleDataChanged();
    }

    <%--Start/SR/2015.09.30 - New Method has been added to handle visibility of Taxable only option--%>
    function DisplayRolloveroption(value) {
        if (value) {
            $("label[for=chkTaxableOnly],#chkTaxableOnly").show();            
        }
        else {
            $("label[for=chkTaxableOnly],#chkTaxableOnly").hide();
            $('#chkTaxableOnly').removeAttr('checked');
        }
    }
    <%--End/SR/2015.09.30 - New Method has been added to handle visibility of Txaable only option.--%>

    function HandleRolloverControls() {
        <%--Start/SR/2015.09.30:YRS-AT-2501-Following lines of code are commented as it is not supported in latest Jquery version--%>
        //var bEnable = $('#CheckBoxRolloverYes').attr('checked');
        //if (bEnable == true) {
        if ($("#CheckBoxRolloverYes").is(':checked')) {
        <%--End/SR/2015.09.30:YRS-AT-2501 - Following lines of code are commented as it is not supported in latest Jquery version--%>
            $("#TextboxPayee2_TextBoxInstitution, #TextBoxPayeeAmt").disableControl(false);           <%--Start/SR/2015.09.30:YRS-AT-2501 - Payee2 Textbox should have intellisense function. Hence replaced with RolloverInstitution Textbox--%>                   
        } else {
            $("#TextBoxPayeeAmt").disableControl(true);
            $("#TextboxPayee2_TextBoxInstitution").disableControl(true);  <%--SR/2015.09.30:YRS-AT-2501 - Payee2 Textbox should have intellisense function. Hence replaced with RolloverInstitution Textbox--%>                   
            $("#TextBoxPayeeAmt").val(0);
            $("#TextboxPayee2_TextBoxInstitution").val('');  <%--SR/2015.09.30:YRS-AT-2501 - Payee2 Textbox should have intellisense function. Hence replaced with RolloverInstitution Textbox--%>                   
        }
    }
    function EnableDisableSaveButton(intDeduct) {
        // alert('In EnableDisable Save Button');

        $("#ButtonSave").disableControl(true);

        if (($("#TextBoxNet").val() > 0.01 || $("#TextBoxMRDNet").val() > 0.01 || $("#TextBoxNet1").val() > 0.01)
                && ($('#CheckBoxRolloverYes, #CheckBoxRolloverNo').filter(':checked').length == 1)
                && ($('#CheckBoxWithholdingYes, #CheckBoxWithholdingNo').filter(':checked').length == 1)
                && (intDeduct === 0)
                ) {
            $("#ButtonSave").disableControl(false);
        }
        //if net value will be -ive after deduction 
        if ($("#TextBoxNet").val() < 0 || $("#TextBoxMRDNet").val() < 0 || $("#TextBoxNet1").val() < 0) {
            $("#ButtonSave").disableControl(true);
        }
    }
    //BS:2011.10.12--End----handling rollover checkbox

    //BS:2011.11.08:return total taxable and nontaxable amount from payee1 and Mrd datatable
    function Count_Taxable_NonTaxable_Amt(dt) {
        var dbl_Total_Taxable = 0;
        var dbl_Total_NonTaxable = 0;
        for (i = 0; i < dt.length; i++) {
            var e = dt[i];
            dbl_Total_Taxable += parseFloat(e[1]);
            dbl_Total_NonTaxable += parseFloat(e[2]);
        }
        return dbl_Total_Taxable + dbl_Total_NonTaxable;
    }

    //BS:2011.10.10-----Initialization of all controls
    function initializeControl() {
        //Initialize global variables,Array which returns from codebehinde method CreatePayee1Payee2Mrd() 
        var datatable_Payee1 = Payee1DataTable;
        var datatable_Payee2 = Payee2DataTable;
        var datatable_Mrd = MrdDataTable;
        var strDisbursementType = options.DisbursementType;
        var strAllowRollOverNo = options.CanRollOverNo;
        <%--START/SR/2015.09.30:YRS-AT-2501-Reissue Withdrawal screen, allow rollovers of taxable only--%> 
        $("label[for=chkTaxableOnly],#chkTaxableOnly").hide();
        $('#chkTaxableOnly').removeAttr('checked');
        $('#CheckBoxRolloverYes').removeAttr('checked');
        $('#CheckBoxRolloverNo').removeAttr('checked');
        <%--END/SR/2015.09.30:YRS-AT-2501-Reissue Withdrawal screen, allow rollovers of taxable only--%> 
        ComputeTotalTaxableNonTaxable_MaxrowIndex();
        setUpControls();
        setUpEventHandlers(datatable_Payee1, datatable_Payee2, datatable_Mrd, strDisbursementType);
        HandleDataChanged();
        $(window).scroll(function () {
            if ($lblMsg != null) $lblMsg.stop().animate({ top: $('body').scrollTop() }, 500);
        })
       
      
    }
    function setUpControls() {
        //apply Twodecimals class and DisableControl
        $("#TextBoxMRDNonTaxable, #TextBoxMRDTaxable, #TextBoxMRDTax, #TextBoxMRDNet, #TextBoxMRDDeductions").addClass("TwoDecimals").disableControl(true);
        $("#TextBoxNonTaxable, #TextBoxTaxable, #TextBoxTax, #TextBoxNet, #TextBoxDeductions").addClass("TwoDecimals").disableControl(true);
        $("#TextBoxNonTaxable1, #TextBoxTaxable1, #TextBoxTax1, #TextBoxNet1, #TextBoxDeductions1").addClass("TwoDecimals").disableControl(true);

        $("#TextBoxRate, #TextBoxMRDRate").disableControl(true);
        $("#TextBoxPayeeAmt").addClass("TwoDecimals").disableControl(true);
        $("#TextboxPayee2_TextBoxInstitution").disableControl(true); <%--SR/2015.09.30:YRS-AT-2501 - Payee2 Textbox should have intellisense function. Hence replaced with RolloverInstitution Textbox--%>                   

        HandleRolloverControls();
        HandleWithholdingControls();

        //BS:2011.11.08:-reopen issue-YRS 5.0-1451
        //check if only mrd datatable have data then rollover disable
        var dtPayee1 = Count_Taxable_NonTaxable_Amt(Payee1DataTable);
        var dtMrd = Count_Taxable_NonTaxable_Amt(MrdDataTable);
        //If bCanRollover flag is false then disable YES / NO controls for Rollover
        if ((options.CanRollOverNo === 0) || (dtMrd != 0 && dtPayee1 === 0)) {
            $('#CheckBoxRolloverYes').removeAttr('checked');
            $('#CheckBoxRolloverNo').attr('checked', 'checked');
            $("#CheckBoxRolloverYes").disableControl(true);
            $("#CheckBoxRolloverNo").disableControl(true);
        }

    }
    (function ($) {
        jQuery.fn.disableControl = function (bDisable) {
            if (bDisable) {
                this.addClass("DisableControl").removeClass("EnableControl");
            } else {
                this.removeClass("DisableControl").addClass("EnableControl");
            }
        }
    })(jQuery);
    function setUpEventHandlers(datatable_Payee1, datatable_Payee2, datatable_Mrd, strDisbursementType) {
        //Setup events for Rollover CheckBox
        $("#CheckBoxRolloverYes").click(function () {  alertMessage(''); CheckBoxRolloverYesNo_Click(this); });
        $("#CheckBoxRolloverNo").click(function () { alertMessage('');  CheckBoxRolloverYesNo_Click(this); });

        //Setup events for witholding CheckBox
        $("#CheckBoxWithholdingYes").click(function () { alertMessage('');  EnableDisableWithholdings_Click(this); });
        $("#CheckBoxWithholdingNo").click(function () { alertMessage('');  EnableDisableWithholdings_Click(this); });

        $('#TextBoxMRDRate').keypress(function () { HandleAmountFiltering(this) }).change(function () { FormatAmtControlTwoDecimal(this) });
        $('#TextBoxRate').keypress(function () { HandleAmountFiltering(this) }).change(function () { FormatAmtControlTwoDecimal(this) });
        $('#TextBoxPayeeAmt').keypress(function (){ HandleAmountFiltering(this) }).change(function () { FormatAmtControlTwoDecimal(this) });

        //Setup events to enable controls when saving the request
        $("#ButtonSave").click(function () {
            alertMessage('');
            if ($('#CheckBoxRolloverYes').filter(':checked').length == 1) {
                if ($("#TextboxPayee2_TextBoxInstitution").val() === '') {
                    $("#ButtonSave").disableControl(true);
                    $("#TextboxPayee2_TextBoxInstitution").focus(); <%--SR/2015.09.30:YRS-AT-2501 - Payee2 Textbox should have intellisense function. Hence replaced with RolloverInstitution Textbox--%>                   
                    alertMessage('Please enter a valid Payee2 Name.');
                    return false;

                }
                if ($("#TextBoxPayeeAmt").val() === '' || $("#TextBoxPayeeAmt").val() === '0.00') {
                    $("#ButtonSave").disableControl(true);
                    $("#TextBoxPayeeAmt").focus();
                    alertMessage('Please enter a valid Payee2 Amount.');
                    return false;
                }

            }
            //BS:2011.10.31:YRS 5.0-1451: reopen issue
            if (
                ($("#TextBoxMRDNet").val() < 0.01 && ($("#TextBoxMRDTaxable").val() > 0.00 || $("#TextBoxMRDNonTaxable").val() > 0.00))
                ||
                ($("#TextBoxNet").val() < 0.01 && ($("#TextBoxTaxable").val() > 0.00 || $("#TextBoxNonTaxable").val() > 0.00))
                ||
                ($("#TextBoxNet1").val() < 0.01 && ($("#TextBoxTaxable1").val() > 0.00 || $("#TextBoxNonTaxable1").val() > 0.00))
                ) {
                $("#ButtonSave").disableControl(true);
                alertMessage('Net amount can not be less than $0.01.');
                return false;
            }
            //BS:2011.11.17:validate before save data
            if (performValidationsOnInputControls() == false) {
                $("#ButtonSave").disableControl(true);
                return false;
            }
            

            $(".DisableControl").each(function () { $(this).removeAttr('disabled', 'disabled'); });
        })

        $("#TextBoxMRDRate").change(function () { alertMessage(''); TextBoxMRDRate_Changed(); });
        $("#TextBoxRate").change(function () { alertMessage(''); TextBoxRate_Changed(); });
        $("#TextBoxPayeeAmt").change(function (){ alertMessage(''); TextBoxPayeeAmt_Changed() });
        
        <%--Start- SR/2015.09.30:YRS-AT-2501-Reissue Withdrawal screen, allow rollovers of taxable only --%>
        $("#chkTaxableOnly").click(function () {alertMessage(''); chkTaxableOnly_Click(this);});

        function chkTaxableOnly_Click(ctrl) {           
            if ($(ctrl).is(':checked')) {
                var dtPayee1 = Payee1DataTable;
                var dblTotalTaxable = 0.0;
                for (j = 0; j < dtPayee1.length; j++) {
                    var a = dtPayee1[j];
                    dblTotalTaxable += parseFloat(a[1]);
                }
                if (dblTotalTaxable > 0) {
                    $("#TextBoxPayeeAmt").val(dblTotalTaxable);
                    $("#TextBoxPayeeAmt").disableControl(true); 
                    $("#TextboxPayee2_TextBoxInstitution").disableControl(false); <%--SR/2015.09.30:YRS-AT-2501 - Payee2 Textbox should have intellisense function. Hence replaced with RolloverInstitution Textbox--%>                   
                }
                else {
                    alertMessage('No taxable amount.');
                }
            } else {
                //$("#TextBoxPayee2, #TextBoxPayeeAmt").disableControl(true);                 
                $("#TextBoxPayeeAmt").disableControl(false);
                $("#TextboxPayee2_TextBoxInstitution").disableControl(false); <%--SR/2015.09.30:YRS-AT-2501 - Payee2 Textbox should have intellisense function. Hence replaced with RolloverInstitution Textbox--%>                   
                $("#TextBoxPayeeAmt").val(0);
                $("#TextboxPayee2_TextBoxInstitution").val(''); <%--SR/2015.09.30:YRS-AT-2501 - Payee2 Textbox should have intellisense function. Hence replaced with RolloverInstitution Textbox--%>                   
            }
            HandleDataChanged();
        }        
       
        <%--End- SR/2015.09.30:YRS-AT-2501-Reissue Withdrawal screen, allow rollovers of taxable only --%>
    }

    function TextBoxPayeeAmt_Changed() {
        if ($("#TextBoxPayeeAmt").val() === '') {
            $("#TextBoxPayeeAmt").focus();
            alertMessage('Please Enter Payee2 Amount'); return false;
        }
        if (isNaN($("#TextBoxPayeeAmt").val())) {
            alertMessage('Invalid entry'); return false;
        }
        HandleDataChanged();
    }

   <%-- Start- SR/2015.09.30:YRS-AT-2501-Reissue Withdrawal screen, allow rollovers of taxable only. Here, Implemented new logic based on YRS-AT-2501 --%> 
    function HandleDataChanged() {
        if (performValidationsOnInputControls() == false) return false;

        var dataTable = Payee1DataTable;
        var v_payee1Taxable = 0.0;
        var v_payee1Nontaxable = 0.0;
        var v_payee2Taxable = 0.0;
        var v_payee2Nontaxable = 0.0;
        var html_Payee1, html_Payee2;
        var dtpayee2 = new Array();
        var dtpayee1 = new Array();
        var requestedRolloverAmt = $("#TextBoxPayeeAmt").val().replace(',', '');

        requestedRolloverAmt = roundVal(requestedRolloverAmt, 2); //To handle rounding if the user has entered more than 2 decimals        
        
        var dbl_Total_Taxable = 0;
        var dbl_Total_NonTaxable = 0;
        var dblTaxableProrateFactor = 0;
        var dblNonTaxableProrateFactor = 0;
        var dblRemainingBalfterTaxable = 0;
        var dblRunningTotal = 0;
        var dblRequestedTaxableAmount = 0;
        var dblRequestedNonTaxableAmount = 0;

        for (j = 0; j < dataTable.length; j++) {
            var a = dataTable[j];
            dbl_Total_Taxable += parseFloat(a[1]);
            dbl_Total_NonTaxable += parseFloat(a[2]);
        }

        dbl_Total_Taxable = roundVal(dbl_Total_Taxable, 2); <%--Calculate Total Taxable Amount --%>
        dbl_Total_NonTaxable = roundVal(dbl_Total_NonTaxable, 2); <%--Calculate Total Non-Taxable Amount--%>

        var v_totalAmount = dblTotalTaxableNonTaxable;
        <%--If Requested amount is less than Total taxable amount calculate taxable Prorate factor. --%>
        if (requestedRolloverAmt <= dbl_Total_Taxable) {
            if (requestedRolloverAmt > 0) { <%--condition applied to avoid devide by zero exception when rollover option is not selected --%>
                dblTaxableProrateFactor = dbl_Total_Taxable / requestedRolloverAmt;
            }
        }
        else <%--If Requested amount is higher than Total taxable amount calculate taxable & Non-taxable Prorate factor.--%>
        { 
            dblRemainingBalfterTaxable = requestedRolloverAmt - dbl_Total_Taxable;
            if ((requestedRolloverAmt - dblRemainingBalfterTaxable) > 0) {
                dblTaxableProrateFactor = dbl_Total_Taxable / (requestedRolloverAmt - dblRemainingBalfterTaxable);
            }
            else { dblTaxableProrateFactor = 0; }

            if (dblRemainingBalfterTaxable > 0) {
                dblNonTaxableProrateFactor = dbl_Total_NonTaxable / dblRemainingBalfterTaxable;
            }
            else { dblNonTaxableProrateFactor = 0;}
        }

        //        var i = 0; var j = 0;
        var maxAmountAccountRowIndex = intMaxAmountAccountRowIndex;

        //BS:2011.10.31:YRS 5.0-1451: reopen issue      
            for (i = 0; i < dataTable.length; i++) {
            var e = dataTable[i];
            v_payee1Taxable = roundVal(parseFloat(e[1]), 2);
            v_payee1Nontaxable = roundVal(parseFloat(e[2]), 2);        
          
            //If Requested amount is less than Total taxable amount calculate taxable amount and set non-taxable amount as 0. 
            if (requestedRolloverAmt <= dbl_Total_Taxable) {
                if (dblTaxableProrateFactor != 0)
                {
                    dblRequestedTaxableAmount = roundVal(v_payee1Taxable / dblTaxableProrateFactor, 2);
                }
                else
                {
                    dblRequestedTaxableAmount = 0;
                }
                dblRequestedNonTaxableAmount = 0;
            }
            else //If Requested amount is higher than Total taxable amount calculate taxable amount and non-taxable amount. 
            {
                if(dblTaxableProrateFactor != 0)
                {
                    dblRequestedTaxableAmount = roundVal(v_payee1Taxable / dblTaxableProrateFactor, 2);
                }
                else {
                    dblRequestedTaxableAmount = 0;
                }
                if (dblNonTaxableProrateFactor != 0) {
                    dblRequestedNonTaxableAmount = roundVal(v_payee1Nontaxable / dblNonTaxableProrateFactor, 2);
                }
                else {
                    dblRequestedNonTaxableAmount = 0;
                }
            }

            if (i == dataTable.length) {
                dblRequestedTaxableAmount = roundVal(requestedRolloverAmt - (dblRunningTotal + v_payee2Nontaxable), 2);
            }

            dblRunningTotal += roundVal(dblRequestedTaxableAmount, 2);
            dblRunningTotal = roundVal(dblRunningTotal, 2) <%--Track how much money we have paid to Payee2--%>

            if (requestedRolloverAmt <= dbl_Total_Taxable) {
                v_payee2Taxable = roundVal(dblRequestedTaxableAmount, 2);
                v_payee2Nontaxable = 0.0;
            }
            else {
                v_payee2Taxable = roundVal(dblRequestedTaxableAmount, 2);
                v_payee2Nontaxable = roundVal(dblRequestedNonTaxableAmount, 2);
                dblRunningTotal += roundVal(dblRequestedNonTaxableAmount, 2);
            }

            v_payee1Taxable = roundVal(v_payee1Taxable - v_payee2Taxable, 2); //Reduce these amounts from Payee1
            v_payee1Nontaxable = roundVal(v_payee1Nontaxable - v_payee2Nontaxable, 2);
            //After pro rate Amount recreate payee1 table 
            dtpayee1[i] = [e[0], roundVal(v_payee1Taxable, 2), roundVal(v_payee1Nontaxable, 2)];
            //pro rate detail for payee2 table
            dtpayee2[i] = [e[0], roundVal(v_payee2Taxable, 2), roundVal(v_payee2Nontaxable, 2)];
        }
        //Adjust the difference in the taxable and Non taxable component of the highest valued account
        if (dblRunningTotal != requestedRolloverAmt) {
            var diff = dblRunningTotal - requestedRolloverAmt;
            //BS:2011.11.02:YRS 5.0-1451-reopenIssue-adjust nontaxable component
            if (dtpayee1[maxAmountAccountRowIndex][1] != null && dtpayee1[maxAmountAccountRowIndex][1] + diff >= 0) {
                dtpayee1[maxAmountAccountRowIndex][1] = roundVal(dtpayee1[maxAmountAccountRowIndex][1] + diff, 2);
                dtpayee2[maxAmountAccountRowIndex][1] = roundVal(dtpayee2[maxAmountAccountRowIndex][1] - diff, 2);
            }
            else if (dtpayee1[maxAmountAccountRowIndex][2] != null && dtpayee1[maxAmountAccountRowIndex][2] + diff >= 0) {
                dtpayee1[maxAmountAccountRowIndex][2] = roundVal(dtpayee1[maxAmountAccountRowIndex][2] + diff, 2);
                dtpayee2[maxAmountAccountRowIndex][2] = roundVal(dtpayee2[maxAmountAccountRowIndex][2] - diff, 2);
            }
        }
        html_Payee2 = CreateTable(dtpayee2);
        $('#DivPayee2').html(html_Payee2);

        html_Payee1 = CreateTable(dtpayee1);
        $('#DivPayee1').html(html_Payee1);

        var array_payee1 = new Array()
        array_payee1 = Calculate_TotalPayeedetail(dtpayee1, $("#TextBoxRate").val());

        var array_payee2 = new Array()
        array_payee2 = Calculate_TotalPayeedetail(dtpayee2, $("#TextBoxRate1").val());

        //Create Payee detail for MRD from Global MRD table
        var array_MRD = new Array()
        array_MRD = Calculate_TotalPayeedetail(MrdDataTable, $("#TextBoxMRDRate").val());

        var intDeduct = GetAdditionalDeduction(array_MRD, array_payee1, array_payee2);

        AssignDataToTextBoxes(array_MRD, array_payee1, array_payee2);
        EnableDisableSaveButton(intDeduct);
        $(".TwoDecimals").each(function () { $(this).val(CurrencyFormatted($(this).val())); }); //set decimals for all textboxes
        $(".DisableControl").each(function () { $(this).attr('disabled', 'disabled'); });
        $(".EnableControl").each(function () { $(this).removeAttr('disabled', 'disabled'); });
    }
    <%--End- SR/2015.09.30:YRS-AT-2501-Reissue Withdrawal screen, allow rollovers of taxable only--%>

    function performValidationsOnInputControls() {

        if (isNaN($("#TextBoxPayeeAmt").val().replace(',', ''))) {
            alertMessage('Invalid entry');
            $("#TextBoxPayeeAmt").val(0);
            return false;
        }
        if (isNaN($("#TextBoxRate").val())) {
            alertMessage('Invalid entry for Payee 1 Tax Rate');
             return false;
        }
        if (isNaN($("#TextBoxMRDRate").val())) {
            alertMessage('Invalid entry for MRD Tax Rate');
            return false;
        }
        ////here we validate textbox Mrd rate is > 100 or  < 10 then it will prompt msg
        if ($("#TextBoxMRDRate").val() > 100 || $("#TextBoxMRDRate").val() < 10) {
            alertMessage('Invalid Tax Rate entered. Tax Rate should between 10% to 100%.');
            return false;
          }

        //handle Disbursment Type if it is ADT,RDT then taxrate is 0 other
        if (options.DisbursementType == "ADT" || options.DisbursementType == "RDT") {
            if ($("#TextBoxRate").val() > 100 || $("#TextBoxRate").val() < 0) {
                alertMessage("Invalid Tax Rate entered. Tax Rate should between 0% to 100%.");
                return false;
            }
        } else {
            if ($("#TextBoxRate").val() > 100 || $("#TextBoxRate").val() < 20) {
                alertMessage("Invalid Tax Rate entered. Tax Rate should between 20% to 100%.");
                return false;
            }
        }

        if ($("#TextBoxPayeeAmt").val() > roundVal(dblTotalTaxableNonTaxable, 2)) {
            alertMessage('Requested Amount must be less than or equal to Total Refund');
            return false;
        }
        return true;
    }
    var dblTotalTaxableNonTaxable = 0;
    var intMaxAmountAccountRowIndex = 0;

    function ComputeTotalTaxableNonTaxable_MaxrowIndex() {
        var dataTable = Payee1DataTable;
        var v_totalAmount = 0.0;
        var i = 0; var j = 0;
        var maxAmountAccountRowIndex = 0;
        var maxAmountAccountRowValue = 0;
        for (i = 0; i < dataTable.length; i++) {
            var e = dataTable[i];
            v_payee1Taxable = roundVal(parseFloat(e[1]), 2);
            v_payee1Nontaxable = roundVal(parseFloat(e[2]), 2);
            //Identify and track the highest valued account row and its value
            if (v_payee1Taxable + v_payee1Nontaxable > maxAmountAccountRowValue) {
                maxAmountAccountRowValue = v_payee1Taxable + v_payee1Nontaxable;
                maxAmountAccountRowIndex = i;
            }
            v_totalAmount += v_payee1Taxable + v_payee1Nontaxable;
        }
        dblTotalTaxableNonTaxable = v_totalAmount;
        intMaxAmountAccountRowIndex = maxAmountAccountRowIndex
    
    }

 
    function TextBoxRate_Changed() {
        //alert('Called TextBoxRate_Changed()');
        HandleDataChanged();
    }
    function TextBoxMRDRate_Changed() {       
        HandleDataChanged();
    }
    //BS:2011.10.12--Common Functions
    //this method return string value
    function normData(d) {
        if (d == null) return '-';
        if (typeof (d) == typeof (new Date())) { return (d.getUTCMonth() + 1) + '/' + d.getUTCDate() + '/' + d.getUTCFullYear(); };
        if (d === '') return '-';
        if (parseFloat(d) === d) return CurrencyFormatted(d).toString();
        return d.toString();

    }
    function CreateTable(dataTable) {
        //here create dynamic table
        var openTable = '<table class="DataGrid_Grid" cellspacing="0" rules="all" border="1" id="DataGridPayee1" style="width:300px;border-collapse:collapse;">' +
        '<tr align="center" style="width:100px" class="DataGrid_HeaderStyle"><td align="center">Acct Type</td><td align="right">Taxable</td><td align="right">Non Taxable</td></tr>';
        var i = 0; var html = '';
        for (i = 0; i < dataTable.length; i++) {
            var e = dataTable[i];
            html += '<tr style="width:100px">' +
                         '<td align="center"><span class="Label_Normal">' + normData(e[0]) + '</span></td>' +
                         '<td align="right">' + normData(e[1]) + '</td>' +
                         '<td align="right">' + normData(e[2]) + '</td>' +
                                     '</tr>';


        }
        var closeTable = '</table>';
        if (html != null || html != '') {
            html = openTable + html + closeTable;
        } else {
            html = 'No records found !!!';
        }
        return html
    }
    function Calculate_TotalPayeedetail(dataTable, dblTaxRate) {
        var i = 0;
        var dbl_Total_Taxable = 0.0;
        var dbl_Total_NonTaxable = 0.0;
        var dbl_Total_TaxRate = dblTaxRate;
        var dbl_Total_Tax = 0.0;
        var dbl_Total_Net = 0.0;

        for (i = 0; i < dataTable.length; i++) {
            var e = dataTable[i];
            dbl_Total_Taxable += parseFloat(e[1]);
            dbl_Total_NonTaxable += parseFloat(e[2]);
        }
        dbl_Total_Tax = roundVal(parseFloat(dbl_Total_TaxRate / 100) * dbl_Total_Taxable, 2);
        dbl_Total_Net = roundVal(parseFloat(dbl_Total_Taxable + dbl_Total_NonTaxable) - dbl_Total_Tax, 2);

        return [roundVal(dbl_Total_Taxable, 2), roundVal(dbl_Total_NonTaxable, 2), roundVal(dbl_Total_TaxRate, 2), roundVal(dbl_Total_Tax, 2), roundVal(dbl_Total_Net, 2), 0.0];
    }
    function AssignValuesIntoTextBoxs(varTaxable, varNonTaxable, varRate, varTax, varNet, varDeduction, varArray) {
        $("#" + varTaxable).val(varArray[0]);
        $("#" + varNonTaxable).val(varArray[1]);
        $("#" + varRate).val(varArray[2]);
        $("#" + varTax).val(varArray[3]);
        $("#" + varNet).val(varArray[4]);
        $("#" + varDeduction).val(varArray[5]);
    }
    function CurrencyFormatted(amount) {
        var i = parseFloat(amount);
        if (isNaN(i)) { i = 0.00; }
        var minus = '';
        if (i < 0) { minus = '-'; }
        i = Math.abs(i);
        i = parseInt((i + .005) * 100);
        i = i / 100;
        s = new String(i);
        if (s.indexOf('.') < 0) { s += '.00'; }
        if (s.indexOf('.') == (s.length - 2)) { s += '0'; }
        s = minus + s;
        return s;
    }
    //Start: Bala: 01/19/2019: YRS-AT-2398: Show details of officer.
    function showToolTip() {
        var elementRef = document.getElementById("Tooltip");
        if (elementRef != null) {
            elementRef.style.position = 'absolute';
            elementRef.style.left = event.clientX + 5 + document.body.scrollLeft;
            elementRef.style.top = event.clientY + 5 + document.body.scrollTop;
            elementRef.style.width = '570';
            elementRef.style.visibility = 'visible';
            elementRef.style.display = 'block';
        }
        var lblNote = document.getElementById("lblComments");

        if (lblNote.innerText == '') {
            lblNote.innerText = $('#HiddenFieldOfficersDetails').val();
            elementRef.style.left = event.clientX + 20 + document.body.scrollTop;
            elementRef.style.width = '300';
        } else {
            lblNote.innerText = ''
            hideToolTip();
        }
    }
    //to hide tool tip when mouse is removed
    function hideToolTip() {
        var elementRef = document.getElementById("Tooltip");
        if (elementRef != null) {
            elementRef.style.visibility = 'hidden';
        }
    }
    //End: Bala: 01/19/2019: YRS-AT-2398: Show details of officer.
   
</script>
<style>
.message
{
		-webkit-background-size: 40px 40px;
		-moz-background-size: 40px 40px;
		background-size: 40px 40px;			
		background-image: -webkit-gradient(linear, left top, right bottom,
								color-stop(.25, rgba(255, 255, 255, .05)), color-stop(.25, transparent),
								color-stop(.5, transparent), color-stop(.5, rgba(255, 255, 255, .05)),
								color-stop(.75, rgba(255, 255, 255, .05)), color-stop(.75, transparent),
								to(transparent));
		background-image: -webkit-linear-gradient(135deg, rgba(255, 255, 255, .05) 25%, transparent 25%,
							transparent 50%, rgba(255, 255, 255, .05) 50%, rgba(255, 255, 255, .05) 75%,
							transparent 75%, transparent);
		background-image: -moz-linear-gradient(135deg, rgba(255, 255, 255, .05) 25%, transparent 25%,
							transparent 50%, rgba(255, 255, 255, .05) 50%, rgba(255, 255, 255, .05) 75%,
							transparent 75%, transparent);
		background-image: -ms-linear-gradient(135deg, rgba(255, 255, 255, .05) 25%, transparent 25%,
							transparent 50%, rgba(255, 255, 255, .05) 50%, rgba(255, 255, 255, .05) 75%,
							transparent 75%, transparent);
		background-image: -o-linear-gradient(135deg, rgba(255, 255, 255, .05) 25%, transparent 25%,
							transparent 50%, rgba(255, 255, 255, .05) 50%, rgba(255, 255, 255, .05) 75%,
							transparent 75%, transparent);
		background-image: linear-gradient(135deg, rgba(255, 255, 255, .05) 25%, transparent 25%,
							transparent 50%, rgba(255, 255, 255, .05) 50%, rgba(255, 255, 255, .05) 75%,
							transparent 75%, transparent);
		 -moz-box-shadow: inset 0 -1px 0 rgba(255,255,255,.4);
		 -webkit-box-shadow: inset 0 -1px 0 rgba(255,255,255,.4);		
		 box-shadow: inset 0 -1px 0 rgba(255,255,255,.4);
		 width: 100%;
		 border: 2px solid;
		 color: Black;
		 padding: 15px;
		 position: fixed;
	     _position: absolute;
		 text-shadow: 0 1px 0 rgba(0,0,0,.5);
		 -webkit-animation: animate-bg 5s linear infinite;
		 -moz-animation: animate-bg 5s linear infinite;
}
.info {
    background-color: #dddddd; border-color: #bbbbbb; 
}
.error {
    background-color: #de4343; border-color: #c43d3d;
}
.warning {
    background-color: #eaaf51; border-color: #d99a36;
}
.success {
    background-color: #61b832; border-color: #55a12c;
}
.message h3
{
    margin: 0 0 5px 0;													 
}
.message p
{
    margin: 0;													 
}

@-webkit-keyframes animate-bg
{
    from {
        background-position: 0 0;
    }
    to {
       background-position: -80px 0;
    }
}

@-moz-keyframes animate-bg 
{
    from {
        background-position: 0 0;
    }
    to {
       background-position: -80px 0;
    }
}

/*--------------------------------------*/

.centered
{
		 text-align: center;
}

</style>

<form id="Form1" method="post" runat="server">
<table class="Table_WithoutBorder" cellspacing="0" width="700">
    <tr>
        <td class="td_backgroundcolorwhite" colspan="2">
        </td>
    </tr>
    <tr>
        <td class="td_backgroundcolorwhite" colspan="2">
        </td>
    </tr>
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
</table>
<div class="center">
    <table width="700" border="0">
        <tr>
            <td class="Td_HeadingFormContainer">
                <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
            </td>
        </tr>
        <%-- Start: Bala: 01/19/2019: YRS-AT-2398: Alert message--%>
        <tr>
            <td bgcolor="red" colspan="2" style="text-align: left;">
                <asp:Label ID="LabelSpecialHandling" ForeColor="White" runat="server" Font-Size="12px" Visible="False" Font-Bold="True"></asp:Label> 
                <a id="LinkButtonSpecialHandling" style="color: white; font-size: 9px; cursor: pointer;" onclick="showToolTip();" onmouseout="javascript: document.getElementById('lblComments').innerHTML = ''; hideToolTip();">[view details]</a>
                <asp:HiddenField ID="HiddenFieldOfficersDetails" runat="server"/>
            </td>
        </tr>
        <%-- End: Bala: 01/19/2019: YRS-AT-2398: Alert message--%>
    </table>
    <%-- Start: Bala: 01/19/2019: YRS-AT-2398: Officers detail in tooltip--%>
    <div id="Tooltip" runat="server" style="z-index: 1000; width: auto; border-left: 1px solid silver; border-top: 1px solid silver; border-right: 2px solid black; border-bottom: 1px solid #cccccc; padding: 3px; position: absolute; top: 0; left: 0; background: LightYellow; color: black; display: none; /* does not work in ie6 */	font-size: 7pt; font-family: verdana; margin: 0; overflow: visible; text-align: left;">
        <asp:Label runat="server" ID="lblComments" Style="display: block; width: auto; overflow: visible; font-size: x-small;"></asp:Label>
    </div>
    <script type="text/javascript">
        if ($("#LabelSpecialHandling").text() == '') {
            $('#LinkButtonSpecialHandling').hide();
        }
    </script>
    <%-- End: Bala: 01/19/2019: YRS-AT-2398: Officers detail in tooltip--%>
    <iewc:TabStrip ID="TabStripWithdrawalReissue" runat="server" BorderStyle="None" AutoPostBack="True"
        TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
        TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
        Width="700px" Height="30px">
        <iewc:Tab Text="List" ID="tabList"></iewc:Tab>
        <iewc:Tab Text="Disbursements" ID="tabGeneral"></iewc:Tab>
    </iewc:TabStrip>
    <iewc:MultiPage ID="MultiPageVRManager" runat="server">
        <iewc:PageView>
            <table class="Table_WithBorder">
                <tr>
                    <td>
                        <div style="overflow: auto; width: 690px; height: 335px">
                            <asp:datagrid id="DataGridReissue" cssclass="DataGrid_Grid" runat="server" width="690">
										<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
										<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
										<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
										<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
										<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
										<Columns>
											<asp:TemplateColumn>
												<ItemTemplate>
													<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
														CommandName="Select" ToolTip="Select"></asp:ImageButton>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
							</asp:datagrid>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="Td_ButtonContainer" align="right">
                        <asp:button id="ButtonOK" runat="server" cssclass="Button_Normal" width="73px" height="20px"
                            text="OK"></asp:button>
                    </td>
                </tr>
            </table>
        </iewc:PageView>
        <iewc:PageView>
            <table class="Table_WithBorder" width="100%">
                <tbody>
                    <tr>
                        <td>
                            <table border="0" width="100%">
                                <tr>
                                    <td colspan="1">
                                        <asp:label id="LabelRollover" runat="server" cssclass="Label_Small">Does the participant request a rollover?</asp:label>
                                        <%--  <asp:checkbox id="CheckBoxRolloverYes" checked="true" runat="server" text="Yes" autopostback="false"--%>
                                        <asp:checkbox id="CheckBoxRolloverYes" runat="server" text="Yes" autopostback="false"
                                            cssclass="Label_Small"></asp:checkbox>
                                        <asp:checkbox id="CheckBoxRolloverNo" runat="server" text="No" autopostback="False"
                                            cssclass="Label_Small"></asp:checkbox>
                                        <asp:checkbox id="chkTaxableOnly" runat="server" text="Taxable Only" autopostback="false" cssclass="Label_Small"></asp:checkbox>    <%-- SR/2015.09.30:YRS-AT-2501-Reissue Withdrawal screen, allow rollovers of taxable only --%>                                 
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <%-- Start-SR/2015.09.30:YRS-AT-2501-Reissue Withdrawal screen, allow rollovers of taxable only. Following set of HTML code has been updated to add and align new Taxable option --%>
                                          <table class="Table_WithBorder" width="100%">
                                            <tr>
                                                <td align="right" valign="top">
                                                    <table width="100%" align="left" valign="top" >
                                                        <tr>
                                                            <td>
                                                                <asp:label id="LabelPayee1" cssclass="Label_Small" runat="server" associatedcontrolid="TextBoxPayee1">Payee1 </asp:label>
                                                              </td>
                                                         </tr> 
                                                         <tr>
                                                                <td colspan ="2">
                                                                    <asp:textbox id="TextBoxPayee1" runat="server" cssclass="TextBox_Normal" width="300px"></asp:textbox>
                                                                </td>
                                                         </tr>
                                                         <tr>
                                                            <td align="left">
                                                                <div id="DivPayee1">
                                                                </div>
                                                                <%-- <asp:datagrid id="DataGridPayee1" autogeneratecolumns="False" cssclass="DataGrid_Grid"
                                                                    runat="server" width="300px" visible="False">
																			<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																			<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																			<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																			<Columns>
																				<asp:TemplateColumn HeaderText="Acct Type">
																					<ItemTemplate>
																						<asp:Label id="LabelAcctType" runat="server" cssclass="Label_Normal" Text='<%# DataBinder.Eval(Container.DataItem, "AcctType") %>'>
																						</asp:Label>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																				<asp:TemplateColumn HeaderText="Taxable">
																					<ItemTemplate>
																						<asp:TextBox id="txtTaxable" ReadOnly="True"  runat="server"  MaxLength="10" cssclass="TextBox_Normal" Width="100px" Text='<%# DataBinder.Eval(Container.DataItem, "Taxable")%>'>
																						</asp:TextBox>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																				<asp:TemplateColumn HeaderText="Non Taxable">
																					<ItemTemplate>
																						<asp:TextBox id="txtNonTaxable" ReadOnly="True" runat="server"  MaxLength="10" cssclass="TextBox_Normal" Width="100px"  Text='<%# DataBinder.Eval(Container.DataItem, "NonTaxable")%>'>
																						</asp:TextBox>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																			</Columns>
																		</asp:datagrid>  --%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right" >
                                                    <table width="100%" align="right">
                                                        <tr>
                                                            <td nowrap="true">                                                             
                                                                   
                                                                            <asp:label id="LabelPayee2" cssclass="Label_Small" width="150px" runat="server">Payee2 </asp:label>
                                                            </td>
                                                            <td>
                                                                            <asp:label id="LabelPayeeAmt" cssclass="Label_Small" width="150px" runat="server">Payee Amount </asp:label>
                                                            </td>
                                                       </tr>
                                                       <tr>
                                                           <td >                                                               
                                                             <UC2:RollUserControl runat="server" ID="TextboxPayee2"/>
                                                           </td>
                                                          <td>
                                                            <asp:textbox id="TextBoxPayeeAmt" runat="server" cssclass="TextBox_Normal" width="130px" maxlength="12"></asp:textbox>
                                                                <div id="payee1Session">
                                                                </div>
                                                          </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <div id="DivPayee2" >
                                                                </div>
                                                                <%-- <asp:datagrid id="DatagridPayee2" runat="server" cssclass="DataGrid_Grid" width="300px"
                                                                    autogeneratecolumns="False" visible="false">
																			<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																			<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																			<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
																			<SelectedItemStyle CssClass="DataGrid_SelectedStyle_Total"></SelectedItemStyle>
																			<Columns>
																				<asp:TemplateColumn HeaderText="Acct Type">
																					<ItemTemplate>
																						<asp:Label id="LabelAcctType" runat="server" cssclass="Label_Normal" Text='<%# DataBinder.Eval(Container.DataItem, "AcctType") %>'>
																						</asp:Label>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																				<asp:TemplateColumn HeaderText="Taxable">
																					<ItemTemplate>
																						<asp:TextBox id="txtTaxable" enabled="False"  runat="server" autopostback="true" MaxLength="10" cssclass="TextBox_Normal" Width="100px" OnTextChanged="Text_Changed" Text='<%# DataBinder.Eval(Container.DataItem, "Taxable")%>'>
																						</asp:TextBox>
                                                                                     
																					</ItemTemplate>
																				</asp:TemplateColumn>
																				<asp:TemplateColumn HeaderText="Non Taxable">
																					<ItemTemplate>
																						<asp:TextBox id="txtNonTaxable" enabled="False" runat="server" autopostback="true" MaxLength="10" cssclass="TextBox_Normal" Width="100px" OnTextChanged="Text_Changed" Text='<%# DataBinder.Eval(Container.DataItem, "NonTaxable")%>'>
																						</asp:TextBox>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																			</Columns>
																		</asp:datagrid> --%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right" valign="top">
                                                    <table width="100%" align="left">
                                                        <tr>
                                                            <td>
                                                                <asp:label id="LabelPayee3" visible="False" cssclass="Label_Small" runat="server">Payee3 </asp:label>
                                                                <br />
                                                                <asp:textbox id="TextBoxPayee3" runat="server" visible="False" cssclass="TextBox_Normal"
                                                                    width="200px" autopostback="True">Payee3</asp:textbox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:datagrid id="DatagridPayee3" runat="server" visible="False" cssclass="DataGrid_Grid"
                                                                    width="200" autogeneratecolumns="False">
																			<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																			<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																			<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
																			<SelectedItemStyle CssClass="DataGrid_SelectedStyle_Total"></SelectedItemStyle>
																			<Columns>
																				<asp:TemplateColumn HeaderText="Acct Type">
																					<ItemTemplate>
																						<asp:Label id="LabelPayee3AcctType" runat="server" cssclass="Label_Normal" Text='<%# DataBinder.Eval(Container.DataItem, "AcctType") %>'>
																						</asp:Label>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																				<asp:TemplateColumn HeaderText="Taxable">
																					<ItemTemplate>
																						<%--<asp:TextBox id="txtTaxable" enabled="true" runat="server" autopostback="true" MaxLength="10" cssclass="TextBox_Normal" Width="55px" OnTextChanged="Text_Changed" Text='<%# DataBinder.Eval(Container.DataItem, "Taxable")%>'>
																						</asp:TextBox>--%>
                                                                                        <asp:TextBox id="txtTaxable" enabled="true" runat="server" autopostback="true" MaxLength="10" cssclass="TextBox_Normal" Width="55px"  Text='<%# DataBinder.Eval(Container.DataItem, "Taxable")%>'>
																						</asp:TextBox>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																				<asp:TemplateColumn HeaderText="Non Taxable">
																					<ItemTemplate>
																						<%--<asp:TextBox id="txtNonTaxable" enabled="true" runat="server" autopostback="true" MaxLength="10" cssclass="TextBox_Normal" Width="55px" OnTextChanged="Text_Changed" Text='<%# DataBinder.Eval(Container.DataItem, "NonTaxable")%>'>
																						</asp:TextBox>--%>
                                                                                        <asp:TextBox id="txtNonTaxable" enabled="true" runat="server" autopostback="true" MaxLength="10" cssclass="TextBox_Normal" Width="55px" Text='<%# DataBinder.Eval(Container.DataItem, "NonTaxable")%>'>
																						</asp:TextBox>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																			</Columns>
																		</asp:datagrid>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <%-- Start-SR/2015.09.30:YRS-AT-2501-Reissue Withdrawal screen, allow rollovers of taxable only. Following set of HTML code has been updated to add and align new Taxable option --%>
                                    </td>
                                    <tr />
                                    <tr>
                                        <td>
                                            <asp:label id="LabelWithholding" runat="server" cssclass="Label_Small">Does the participant request additional withholding?</asp:label>
                                            <asp:checkbox id="CheckBoxWithholdingYes" runat="server" text="Yes" autopostback="false"
                                                cssclass="Label_Small"></asp:checkbox>
                                            <asp:checkbox id="CheckBoxWithholdingNo" runat="server" text="No" autopostback="false"
                                                cssclass="Label_Small"></asp:checkbox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="5">
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="5">
                                                        <div id="DivMrd">
                                                            <table class="Table_WithBorder" align="left" width="100%">
                                                                <tr>
                                                                    <td class="Td_SubText" colspan="7" align="left">
                                                                        Payment Details
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <asp:label id="LabelTaxable" runat="server" cssclass="Label_Small" width="100px">Taxable</asp:label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:label id="LabelNonTaxable" runat="server" cssclass="Label_Small" width="100px">Non-Taxable</asp:label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:label id="LabelTaxRate" runat="server" cssclass="Label_Small" width="70px">Tax Rate</asp:label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:label id="LabelTax" runat="server" cssclass="Label_Small" width="100px">Tax</asp:label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:label id="LabelDeductions" runat="server" cssclass="Label_Small" width="100px">Deductions</asp:label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:label id="LabelNet" runat="server" cssclass="Label_Small" width="100px">Net</asp:label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="Label_Small" align="right">
                                                                        RMD
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxMRDTaxable" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxMRDNonTaxable" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxMRDRate" runat="server" cssclass="TextBox_Normal" width="70px"
                                                                            maxlength="6"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxMRDTax" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px" maxlength="3"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxMRDDeductions" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxMRDNet" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px"></asp:textbox>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                </tr>
                                                                <tr>
                                                                    <td class="Label_Small" align="right">
                                                                        Payee1
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxTaxable" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxNonTaxable" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxRate" runat="server" cssclass="TextBox_Normal" width="70px"
                                                                            maxlength="6"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxTax" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px" maxlength="3"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxDeductions" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxNet" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px"></asp:textbox>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                </tr>
                                                                <tr>
                                                                    <td class="Label_Small" align="right">
                                                                        Payee2
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxTaxable1" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxNonTaxable1" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxRate1" enabled="false" text="0" runat="server" cssclass="TextBox_Normal"
                                                                            width="70px" maxlength="6"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxTax1" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px" maxlength="3"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxDeductions1" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px"></asp:textbox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:textbox id="TextBoxNet1" enabled="false" runat="server" cssclass="TextBox_Normal"
                                                                            width="100px"></asp:textbox>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <table class="Table_WithBorder" align="left" width="100%">
                                                <tr>
                                                    <td align="left" valign="top" colspan="2">
                                                        <table width="100%" align="left">
                                                            <tr>
                                                                <td>
                                                                    <table border="0">
                                                                        <tr>
                                                                            <td align="left" valign="top">
                                                                                <asp:label id="LabelAddress1" runat="server" cssclass="Label_Small">Address1</asp:label>
                                                                            </td>
                                                                            <td colspan="3">
                                                                                <asp:textbox id="TextBoxAddress1" runat="server" cssclass="TextBox_Normal" width="300px"></asp:textbox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:label id="LabelAddress2" runat="server" cssclass="Label_Small">Address2</asp:label>
                                                                            </td>
                                                                            <td colspan="3">
                                                                                <asp:textbox id="TextBoxAddress2" runat="server" cssclass="TextBox_Normal" width="300px"></asp:textbox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:label id="LabelAddress3" runat="server" cssclass="Label_Small">Address3</asp:label>
                                                                            </td>
                                                                            <td colspan="3">
                                                                                <asp:textbox id="TextBoxAddress3" runat="server" cssclass="TextBox_Normal" width="300px"></asp:textbox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:label id="LabelCity" runat="server" cssclass="Label_Small">City</asp:label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:textbox id="TextBoxCity" runat="server" cssclass="TextBox_Normal" width="100px"></asp:textbox>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:label id="Label2" runat="server" cssclass="Label_Small">Zip</asp:label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:textbox id="TextBoxPin" runat="server" cssclass="TextBox_Normal" width="140px"></asp:textbox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:label id="lblState" runat="server" cssclass="Label_Small">State</asp:label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:textbox id="TextBoxState" runat="server" cssclass="TextBox_Normal" width="100px"></asp:textbox>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:label id="labelCountry" runat="server" cssclass="Label_Small">Country</asp:label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:textbox id="TextBoxCountry" runat="server" cssclass="TextBox_Normal" width="140px"></asp:textbox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <asp:label id="LabelYesNo" cssclass="Label_Small" runat="server" visible="false">Yes No</asp:label>
                                                                    <asp:label id="LabelDeduction" runat="server" cssclass="Label_Small">Apply additional fees</asp:label>
                                                                    <br />
                                                                    <div id="DivDeduction">
                                                                    </div>
                                                                    <asp:datagrid id="DataGridDeductions" autogeneratecolumns="False" cssclass="DataGrid_Grid"
                                                                        runat="server" width="250px" visible="false" clientidmode="Predictable">
																			<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																			<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																			<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																			<Columns>
																				<asp:boundcolumn headertext="CodeValue" datafield="CodeValue" />
																				<asp:TemplateColumn>
																					<ItemTemplate>
																						<asp:CheckBox amount=<%#Container.DataItem("Amount") %> id="CheckBoxDeduction" runat="server" autopostback="false" onclick="HandleDeductionChange()"></asp:CheckBox>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																				<asp:boundcolumn headertext="Description" datafield="ShortDescription" />
																				<asp:boundcolumn headertext="Amount" datafield="Amount"  />
                                                                               </Columns>
																		</asp:datagrid>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <script language="javascript">
                                                                    function ShowExistingDedClick() {
                                                                        if (document.getElementById('dgExistingDeductions').style.display == 'none') {
                                                                            document.getElementById('dgExistingDeductions').style.display = 'block';
                                                                            document.getElementById("hypExistingDeductions").innerHTML = document.getElementById("hypExistingDeductions").innerHTML.replace('Show', 'Hide');
                                                                        }
                                                                        else {
                                                                            document.getElementById('dgExistingDeductions').style.display = 'none';
                                                                            document.getElementById("hypExistingDeductions").innerHTML = document.getElementById("hypExistingDeductions").innerHTML.replace('Hide', 'Show');
                                                                        }
                                                                    }
                                                                </script>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td class="Label_Small">
                                                                                <asp:hyperlink navigateurl="javascript:ShowExistingDedClick();" runat="server" id="hypExistingDeductions"
                                                                                    visible="True"></asp:hyperlink>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:datagrid id="dgExistingDeductions" style="display: none;" autogeneratecolumns="False"
                                                                                    cssclass="DataGrid_Grid" runat="server" width="250px">
																						<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																						<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																						<Columns>
																							<asp:boundcolumn headertext="CodeValue" visible="False" datafield="CodeValue" />
																							<asp:TemplateColumn>
																								<ItemTemplate>
																									<asp:CheckBox id="chkExistingDeductions" runat="server" autoPostBack="false" checked="true" onclick="HandleDeductionChange()"></asp:CheckBox>
																								</ItemTemplate>
																							</asp:TemplateColumn>
																							<asp:boundcolumn headertext="Description" datafield="ShortDescription" />
																							<asp:boundcolumn headertext="Amount" datafield="Amount" />
																						</Columns>
																					</asp:datagrid>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                            </table>
        </iewc:PageView>
    </iewc:MultiPage>
    <table class="Table_WithBorder" width="685">
        <tr>
            <td class="Td_ButtonContainer" align="right" colspan="2">
                <asp:button id="ButtonSave" runat="server" cssclass="Button_Normal" width="73" height="20"
                    text="Save"></asp:button>
                <asp:button id="buttonDetailsOK" runat="server" cssclass="Button_Normal" width="73px"
                    height="20px" text="OK"></asp:button>
            </td>
        </tr>
    </table>
    <asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
</DIV>
<div class="info message" id="lblMsg" style="display:none;text-align:center">
</div>
</html>

