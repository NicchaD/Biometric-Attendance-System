<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RefundBeneficiary.aspx.vb"
    Inherits="YMCAUI.RefundBeneficiary" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="uc2" TagName="RollUserControl" Src="~/UserControls/RolloverInstitution.ascx" %>
<!--#include virtual="topnew.htm"-->
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
        function ValidateRange(objControl, minvalue, maxvalue) {
            //alert("Called ValidateRange with " + minvalue + ", " + maxvalue);
            if (objControl.value < (minvalue * 1)) {
                alert("Beyond range. Less than Minimum Value. Value must be between " + minvalue + " and " + maxvalue + ".");
                //	objControl.focus()
                return false;

            }
            if (objControl.value > (maxvalue * 1)) {
                alert("Beyond range. Greater than Maximum Value. Value must be between " + minvalue + " and " + maxvalue + ".");
                //	objControl.focus()
                return false;
            }
            return true;
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

        /*
        This function is responsible for filtering the keys pressed and the maintain the amount format of the 
        value in the Text box
        */
        function HandleAmountFilteringWithNoDecimals(ctl) {
            var iKeyCode, objInput;
            var iMaxLen
            var reValidChars = /[0-9]/;
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


        // 'SR:2011.01.10 - To implement rollover functionality through JQuery'
        //Start - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
        //function initializeControls(dblTaxable_RP, dblNonTaxable_RP, dblTaxable_SP, dblNonTaxable_SP) {
        function initializeControls(dblTaxable_RP, dblNonTaxable_RP, dblTaxable_SP, dblNonTaxable_SP, dblRMDTaxable_RP, dblRMDNonTaxable_RP, dblRMDTaxable_SP, dblRMDNonTaxable_SP) {
            //End - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
            //Start - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
            //$("input[name='RolloverOptions_RP']").change(function () { HandleRadioButtonChange('RolloverOptions_RP', dblTaxable_RP, dblNonTaxable_RP, 'TextBoxTaxable_RP', 'RolloverTaxable_RP', 'TextBoxNonTaxable_RP', 'RolloverNonTaxable_RP', 'TextBoxTaxRate_RP', 'TextBoxTax_RP', 'TextBoxNet_RP', 'RolloverNet_RP', 'RolloverPayeeName_RP_TextBoxInstitution', 'RolloverPartialAmount_RP'); });  //SR:2014.08.19 :BT 2632/YRS 5.0-2404- Rollover Payee textbox name changed from RolloverPayeeName_RP to RolloverPayeeName_RP_TextBoxInstitution since we are using itellisenese control for rollover payee name
            //$("input[name='RolloverOptions_SP']").change(function () { HandleRadioButtonChange('RolloverOptions_SP', dblTaxable_SP, dblNonTaxable_SP, 'TextBoxTaxable_SP', 'RolloverTaxable_SP', 'TextBoxNonTaxable_SP', 'RolloverNonTaxable_SP', 'TextBoxTaxRate_SP', 'TextBoxTax_SP', 'TextBoxNet_SP', 'RolloverNet_SP', 'RolloverPayeeName_SP_TextBoxInstitution', 'RolloverPartialAmount_SP'); });  //SR:2014.08.19 :BT 2632/YRS 5.0-2404- Rollover Payee textbox name changed from RolloverPayeeName_RP to RolloverPayeeName_RP_TextBoxInstitution since we are using itellisenese control for rollover payee name
            //START : SB | 2016.11.29 | YRS-AT-3022 | Commented old code and in new code HandleRadioButton function additional chkRolloverToOwnIRA paramter is added
            // $("input[name='RolloverOptions_RP']").change(function () { HandleRadioButtonChange('RolloverOptions_RP', dblTaxable_RP, dblNonTaxable_RP, 'TextBoxTaxable_RP', 'RolloverTaxable_RP', 'TextBoxNonTaxable_RP', 'RolloverNonTaxable_RP', 'TextBoxTaxRate_RP', 'TextBoxTax_RP', 'TextBoxNet_RP', 'RolloverNet_RP', 'RolloverPayeeName_RP_TextBoxInstitution', 'RolloverPartialAmount_RP', dblRMDTaxable_RP, dblRMDNonTaxable_RP); });
            //$("input[name='RolloverOptions_SP']").change(function () { HandleRadioButtonChange('RolloverOptions_SP', dblTaxable_SP, dblNonTaxable_SP, 'TextBoxTaxable_SP', 'RolloverTaxable_SP', 'TextBoxNonTaxable_SP', 'RolloverNonTaxable_SP', 'TextBoxTaxRate_SP', 'TextBoxTax_SP', 'TextBoxNet_SP', 'RolloverNet_SP', 'RolloverPayeeName_SP_TextBoxInstitution', 'RolloverPartialAmount_SP', dblRMDTaxable_SP, dblRMDNonTaxable_SP); });  //SR:2014.08.19 :BT 2632/YRS 5.0-2404- Rollover Payee textbox name changed from RolloverPayeeName_RP to RolloverPayeeName_RP_TextBoxInstitution since we are using itellisenese control for rollover payee name
            $("input[name='RolloverOptions_RP']").change(function () { HandleRadioButtonChange('RolloverOptions_RP', dblTaxable_RP, dblNonTaxable_RP, 'TextBoxTaxable_RP', 'RolloverTaxable_RP', 'TextBoxNonTaxable_RP', 'RolloverNonTaxable_RP', 'TextBoxTaxRate_RP', 'TextBoxTax_RP', 'TextBoxNet_RP', 'RolloverNet_RP', 'RolloverPayeeName_RP_TextBoxInstitution', 'RolloverPartialAmount_RP', dblRMDTaxable_RP, dblRMDNonTaxable_RP, 'chkRolloverToOwnIRA_RP'); });   //   Added parameter RolloverToOwnIRA checkbox  for Retirement Plan which toggles as per condition
            $("input[name='RolloverOptions_SP']").change(function () { HandleRadioButtonChange('RolloverOptions_SP', dblTaxable_SP, dblNonTaxable_SP, 'TextBoxTaxable_SP', 'RolloverTaxable_SP', 'TextBoxNonTaxable_SP', 'RolloverNonTaxable_SP', 'TextBoxTaxRate_SP', 'TextBoxTax_SP', 'TextBoxNet_SP', 'RolloverNet_SP', 'RolloverPayeeName_SP_TextBoxInstitution', 'RolloverPartialAmount_SP', dblRMDTaxable_SP, dblRMDNonTaxable_SP, 'chkRolloverToOwnIRA_SP'); });   //   Added parameter RolloverToOwnIRA checkbox for Savings Plan  which toggles as per condition
            //END : SB | 2016.11.29 | YRS-AT-3022 | Commented old code and in new code HandleRadioButton function additional chkRolloverToOwnIRA paramter is added                                                                                                                                                                                                                                                                                                                                                                                                                                                             

            //End - Manthan | 2016.07.01 | YRS-AT-2911 | Commeneted existing function and added RMD taxable and non-taxable values
            $("#RolloverPartialAmount_RP, #RolloverPartialAmount_SP").addClass("TwoDecimals");
            $("#RolloverPartialAmount_RP").change(function () { Calculate_Partial_RP(dblTaxable_RP, dblNonTaxable_RP); });
            $("#RolloverPartialAmount_SP").change(function () { Calculate_Partial_SP(dblTaxable_SP, dblNonTaxable_SP); });

            $("#TextBoxTax_RP").change(function () { Calculate_TaxRate_RP(); });
            $("#TextBoxTax_SP").change(function () { Calculate_TaxRate_SP(); });
            $("#TextBoxTaxRate_RP").change(function () { Calculate_Tax_RP(); });
            $("#TextBoxTaxRate_SP").change(function () { Calculate_Tax_SP(); });
            $("#RolloverTaxable_RP").blur(function () { Calculate_Taxable_RP(dblTaxable_RP, dblNonTaxable_RP); });
            $("#RolloverNonTaxable_RP").blur(function () { Calculate_NonTaxable_RP(dblTaxable_RP, dblNonTaxable_RP); });
            $("#RolloverTaxable_SP").blur(function () { Calculate_Taxable_SP(dblTaxable_SP, dblNonTaxable_SP); });
            $("#RolloverNonTaxable_SP").blur(function () { Calculate_NonTaxable_SP(dblTaxable_SP, dblNonTaxable_SP); });

            $("#TextBoxTax_RP").addClass("TwoDecimals");
            $("#TextBoxTax_SP").addClass("TwoDecimals");
            $("#TextBoxTaxable_RP").addClass("TwoDecimals").addClass("DisableControl");
            $("#TextBoxNonTaxable_RP").addClass("TwoDecimals").addClass("DisableControl");
            $("#TextBoxTaxable_SP").addClass("TwoDecimals").addClass("DisableControl");
            $("#TextBoxNonTaxable_SP").addClass("TwoDecimals").addClass("DisableControl");
            $("#TextBoxNet_RP").addClass("TwoDecimals").addClass("DisableControl");
            $("#TextBoxNet_SP").addClass("TwoDecimals").addClass("DisableControl");
            //Start - Manthan | 2016.07.01 | YRS-AT-2911 | disabling RMD control
            $("#txtRMDTaxable_RP").addClass("TwoDecimals").addClass("DisableControl");
            $("#txtRMDTaxable_SP").addClass("TwoDecimals").addClass("DisableControl");
            $("#txtRMDNonTaxable_RP").addClass("TwoDecimals").addClass("DisableControl");
            $("#txtRMDNonTaxable_SP").addClass("TwoDecimals").addClass("DisableControl");
            $("#txtRMDNet_RP").addClass("TwoDecimals").addClass("DisableControl");
            $("#txtRMDNet_SP").addClass("TwoDecimals").addClass("DisableControl");
            //End - Manthan | 2016.07.01 | YRS-AT-2911 | disabling RMD control
            $("#RolloverTaxable_RP").addClass("TwoDecimals").addClass("DisableControl");
            $("#RolloverNonTaxable_RP").addClass("TwoDecimals").addClass("DisableControl");
            $("#RolloverTaxable_SP").addClass("TwoDecimals").addClass("DisableControl");
            $("#RolloverNonTaxable_SP").addClass("TwoDecimals").addClass("DisableControl");
            $("#RolloverNet_RP").addClass("TwoDecimals").addClass("DisableControl");
            $("#RolloverNet_SP").addClass("TwoDecimals").addClass("DisableControl");
            $("#TextboxTotalTax").addClass("TwoDecimals").addClass("DisableControl");
            $("#TextboxTotalTaxable").addClass("TwoDecimals").addClass("DisableControl");
            $("#TextboxTotalNet").addClass("TwoDecimals").addClass("DisableControl");
            $("#TextboxDeductions").addClass("TwoDecimals").addClass("DisableControl"); <%--Manthan | YRS-AT-2206 | 14.04.2016 --%>
            $("#TextboxTotalNonTaxable").addClass("TwoDecimals").addClass("DisableControl");

            $(".TwoDecimals").each(function () { $(this).val(CurrencyFormatted($(this).val())); }); //set decimals for all textboxes
            $(".DisableControl").each(function () { $(this).attr('disabled', 'disabled'); });

            var varOption_RP = $("input[name='RolloverOptions_RP']:checked").val();
            var varOption_SP = $("input[name='RolloverOptions_SP']:checked").val();
            $("#RolloverPartialAmount_RP, #RolloverPartialAmount_SP").addClass("TwoDecimals");

            if (varOption_RP != 'Partial') {
                $("#RolloverPartialAmount_RP").attr('disabled', 'disabled');
            }
            if (varOption_SP != 'Partial') {
                $("#RolloverPartialAmount_SP").attr('disabled', 'disabled');
            }

            //Start-SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries
            $("#txtRMDTaxRate_RP").change(function () { Calculate_RMDTax_RP(); });
            $("#txtRMDTaxRate_SP").change(function () { Calculate_RMDTax_SP(); });
            $("#txtRMDTax_RP").change(function () { Calculate_RMDTaxRate_RP(); });
            $("#txtRMDTax_SP").change(function () { Calculate_RMDTaxRate_SP(); });
            //End-SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries

            // START : SB | 2016.11.29 | YRS-AT-3022 | Initialise the checkbox as unchecked and disabled as Rollover none option is by default selected
            $("#chkRolloverToOwnIRA_RP").attr('checked', false)
            $("#chkRolloverToOwnIRA_RP").attr('disabled', true);
            $("#chkRolloverToOwnIRA_SP").attr('checked', false)
            $("#chkRolloverToOwnIRA_SP").attr('disabled', true);

            var varRelationshipWithParticipant = $("#hdnRelationshipWithParticipant").val();   // SB  | 2016.11.29 | YRS-AT-3022 | To Find relationship of participant and benificiary and assigning to variable
            if ( varRelationshipWithParticipant != 'SP') {      // SB  | 2016.11.29 | YRS-AT-3022 | If relation is not spouse then do not display RolloverToOwnIRA option
                $("#trIsRolloverToOwnIRA_RP").closest("tr").hide();
                $("#trIsRolloverToOwnIRA_SP").closest("tr").hide();
            }
            else                                                // SB  | 2016.11.29 | YRS-AT-3022 | If relation is  spouse then  display RolloverToOwnIRA option
            {
                $("#trIsRolloverToOwnIRA_RP").closest("tr").show();
                $("#trIsRolloverToOwnIRA_SP").closest("tr").show();
            }
            // END : SB | 2016.11.29 | YRS-AT-3022 | Initialise the checkbox as unchecked and disabled as Rollover none option is by default selected

        }
        //Start - Manthan | 2016.07.01 | YRS-AT-2911 | YRS enhancement-RMD for benes if nontaxable greater than RMD amount allow rollover taxable only
        // YRS-AT-2911- Declared global variables for identifying RMD rollover taxable option available  and rollover option selected
        var IsRMDRollOver_RP;
        var IsRMDRollOver_SP;
        var RolloverOpt;

        // YRS-AT-2911 - Commented exisiting function and passed RMD taxable and non-taxable values to fuction
        //function HandleRadioButtonChange(RolloverOptions, dblTaxable, dblNonTaxable, varTaxable, varRolloverTaxable, varNonTaxable, varRolloverNonTaxable, varTaxRate, varTax, varNet, varRolloverNet, varRolloverPayeeName, varRolloverPartialAmount) {
        function HandleRadioButtonChange(RolloverOptions, dblTaxable, dblNonTaxable, varTaxable, varRolloverTaxable, varNonTaxable, varRolloverNonTaxable, varTaxRate, varTax, varNet, varRolloverNet, varRolloverPayeeName, varRolloverPartialAmount, dblRMDTaxable, dblRMDNonTaxable, bitIsRolloverToOwnIRA) {   // SB | 2016.11.29 | YRS-AT-3022 | Added parameter RolloverToOwnIRA checkbox id 
            // YRS-AT-2911 - Initialized variables to store RMD control object
            var txtRMDTaxRate_RP = $("#txtRMDTaxRate_RP")[0];
            var txtRMDTaxRate_SP = $("#txtRMDTaxRate_SP")[0];
            var varOption = $("input[name='" + RolloverOptions + "']:checked").val();
            // YRS-AT-2911 - Initialized variables to store rollover option selected plan wise
            var RolloverOpt_RP = $("input[name='RolloverOptions_RP']:checked").val();
            var RolloverOpt_SP = $("input[name='RolloverOptions_SP']:checked").val();  
            RolloverOpt = undefined;
            IsRMDRollOver_RP = undefined;
            IsRMDRollOver_SP = undefined;
            
            
            switch (varOption) {
                case "all":
                    $("#" + varRolloverTaxable).val(roundVal(parseFloat(dblTaxable)));
                    $("#" + varRolloverNonTaxable).val(roundVal(parseFloat(dblNonTaxable)));
                    //$("#" + varRolloverTaxable).attr('disabled', 'disabled');
                    //$("#" + varRolloverNonTaxable).attr('disabled', 'disabled');
                    $("#" + varRolloverPayeeName).attr('disabled', false).attr('readonly', false).val('');
                    $("#" + varRolloverPartialAmount).attr('disabled', 'disabled');
                    $("#" + varRolloverPartialAmount).val(roundVal(parseFloat(dblTaxable)) + roundVal(parseFloat(dblNonTaxable)));                    
                    break;
                    //SR/2014.10.07/BT 2672/YRS 5.0-2422:Add taxable option for rollover
                case "taxable":
                    // YRS-AT-2911 - Implemented logic to rollover RMD taxable amt if rollover taxable option selected
                     RolloverOpt = $(this).append(" Clicked")[0].event.srcElement.id;
                     if (txtRMDTaxRate_RP != undefined && RolloverOpt == "RolloverOptions_RP_2") {
                         <%--START: MMR | 2017.12.11 | YRS-AT-3742 | Commented existing code and used hidden field value to identify non-taxable greater than RMD amount for retirement plan--%>
                         //if ($('#txtRMDNet_RP').val() != undefined && $('#txtRMDNet_RP').val() > 0.00 && RolloverOpt_RP == "taxable" && (parseFloat(dblNonTaxable)) >= (parseFloat(dblRMDTaxable) + parseFloat($('#txtRMDNonTaxable_RP').val()))) {
                         if ($('#txtRMDNet_RP').val() != undefined && $('#txtRMDNet_RP').val() > 0.00 && RolloverOpt_RP == "taxable" && $('#hdnIsNonTaxableGreaterThanRMDRET').val() == "1") {
                            <%--END: MMR | 2017.12.11 | YRS-AT-3742 | Commented existing code and used hidden field value to identify non-taxable greater than RMD amount for retirement plan--%>
                             $("#" + varRolloverTaxable).val(roundVal(parseFloat(dblTaxable)) + parseFloat($('#txtRMDTaxable_RP').val()));
                             $("#" + varRolloverPartialAmount).val(roundVal(parseFloat(dblTaxable)) + parseFloat($('#txtRMDTaxable_RP').val()));
                             IsRMDRollOver_RP = true;
                         }
                         else {
                             $("#RolloverTaxable_RP").val(roundVal(parseFloat(dblTaxable)));
                             $("#RolloverPartialAmount_RP").val(roundVal(parseFloat(dblTaxable)));
                             IsRMDRollOver_RP = false;                               
                             // START | SR | 2017.05.04 | YRS-AT-3173 - YRS enh: if RMD amount greater than non-taxable give warning for " Taxable Only" selection (TrackIT 27473)  
                             if (parseFloat($('#txtRMDTaxable_RP').val()) > 0)
                             {
                                 $("#<%=RolloverOptions_SP.ClientID%>").find('input').prop('disabled', true);
                                 DisplayAlertMessage(RolloverOptions, dblTaxable, dblNonTaxable, varTaxable, varRolloverTaxable, varNonTaxable, varRolloverNonTaxable, varTaxRate, varTax, varNet, varRolloverNet, varRolloverPayeeName, varRolloverPartialAmount, dblRMDTaxable, dblRMDNonTaxable, bitIsRolloverToOwnIRA)
                             }
                             // END | SR | 2017.05.04 | YRS-AT-3173 - YRS enh: if RMD amount greater than non-taxable give warning for " Taxable Only" selection (TrackIT 27473)                               
                        }
                    }
                     else if (txtRMDTaxRate_SP != undefined && RolloverOpt == "RolloverOptions_SP_2") {
                         <%--START: MMR | 2017.12.11 | YRS-AT-3742 | Commented existing code and used hidden field value to identify non-taxable greater than RMD amount for retirement plan--%>
                         //if ($('#txtRMDNet_SP').val() != undefined && $('#txtRMDNet_SP').val() > 0.00 && RolloverOpt_SP == "taxable" && (parseFloat(dblNonTaxable)) >= (parseFloat(dblRMDTaxable) + parseFloat($('#txtRMDNonTaxable_SP').val()))) {
                         if ($('#txtRMDNet_SP').val() != undefined && $('#txtRMDNet_SP').val() > 0.00 && RolloverOpt_SP == "taxable" && $('#hdnIsNonTaxableGreaterThanRMDSAV').val() == "1") {
                            <%--END: MMR | 2017.12.11 | YRS-AT-3742 | Commented existing code and used hidden field value to identify non-taxable greater than RMD amount for retirement plan--%>
                             $("#" + varRolloverTaxable).val(roundVal(parseFloat(dblTaxable)) + parseFloat($('#txtRMDTaxable_SP').val()));
                            $("#" + varRolloverPartialAmount).val(roundVal(parseFloat(dblTaxable)) + parseFloat($('#txtRMDTaxable_SP').val()));
                            IsRMDRollOver_SP = true;
                        }
                        else {
                             $("#RolloverTaxable_SP").val(roundVal(parseFloat(dblTaxable)));
                             $("#RolloverPartialAmount_SP").val(roundVal(parseFloat(dblTaxable)));
                             IsRMDRollOver_SP = false;
                             // START | SR | 2017.05.04 | YRS-AT-3173 - YRS enh: if RMD amount greater than non-taxable give warning for " Taxable Only" selection (TrackIT 27473)  
                             if (parseFloat($('#txtRMDTaxable_SP').val()) > 0) {
                                 $("#<%=RolloverOptions_RP.ClientID%>").find('input').prop('disabled', true);
                                 DisplayAlertMessage(RolloverOptions, dblTaxable, dblNonTaxable, varTaxable, varRolloverTaxable, varNonTaxable, varRolloverNonTaxable, varTaxRate, varTax, varNet, varRolloverNet, varRolloverPayeeName, varRolloverPartialAmount, dblRMDTaxable, dblRMDNonTaxable, bitIsRolloverToOwnIRA)
                             }
                             // END | SR | 2017.05.04 | YRS-AT-3173 - YRS enh: if RMD amount greater than non-taxable give warning for " Taxable Only" selection (TrackIT 27473)  
                        }
                     }
                     else if (txtRMDTaxRate_RP == undefined && RolloverOpt == "RolloverOptions_RP_2") {
                         $("#RolloverTaxable_RP").val(roundVal(parseFloat(dblTaxable)));
                         $("#RolloverPartialAmount_RP").val(roundVal(parseFloat(dblTaxable)));
                     }
                     else if (txtRMDTaxRate_SP == undefined && RolloverOpt == "RolloverOptions_SP_2") {
                         $("#RolloverTaxable_SP").val(roundVal(parseFloat(dblTaxable)));
                         $("#RolloverPartialAmount_SP").val(roundVal(parseFloat(dblTaxable)));
                     }
                    
                    //YRS-AT-2911- Commented existing code
                        //$("#" + varRolloverTaxable).val(roundVal(parseFloat(dblTaxable)));
                        //$("#" + varRolloverPartialAmount).val(roundVal(parseFloat(dblTaxable)));
                        $("#" + varRolloverNonTaxable).val(0);
                        $("#" + varRolloverTaxable).attr('disabled', 'disabled');
                        //$("#" + varRolloverPartialAmount).val(roundVal(parseFloat(dblTaxable)));
                        $("#" + varRolloverPartialAmount).attr('disabled', 'disabled');
                        $("#" + varRolloverNonTaxable).attr('disabled', 'disabled');
                        $("#" + varRolloverPayeeName).attr('disabled', false);
                        $("#" + varRolloverPayeeName).attr('readonly', false);
                        $("#" + varRolloverPayeeName).val('');                    
                    break;
                case "Partial":
                    $("#" + varRolloverTaxable).val(0);
                    $("#" + varRolloverNonTaxable).val(0);
                    $("#" + varRolloverPayeeName).removeAttr('disabled').attr('readonly', false).val('');
                    $("#" + varRolloverPartialAmount).removeAttr('disabled').val('');
                    //YRS-AT-2911- Setting the variable value to false if rollover specific option selected
                    if (txtRMDTaxRate_RP != undefined && RolloverOptions == 'RolloverOptions_RP') {
                        IsRMDRollOver_RP = false;
                    }
                    else if (txtRMDTaxRate_SP != undefined && RolloverOptions == 'RolloverOptions_SP') {
                        IsRMDRollOver_SP = false;
                    }                   
                    break;
                default:
                    $("#" + varRolloverTaxable).val(0);
                    $("#" + varRolloverNonTaxable).val(0);
                    //$("#" + varRolloverTaxable).attr('disabled', 'disabled');
                    //$("#" + varRolloverNonTaxable).attr('disabled', 'disabled');
                    $("#" + varRolloverPayeeName).attr('disabled', 'disabled').val('');
                    $("#" + varRolloverPartialAmount).attr('disabled', 'disabled').val(0);
                    //YRS-AT-2911- Setting the variable value to false if rollover none option selected
                    if (txtRMDTaxRate_RP != undefined && RolloverOptions == 'RolloverOptions_RP') {
                        IsRMDRollOver_RP = false;
                    }
                    else if (txtRMDTaxRate_SP != undefined && RolloverOptions == 'RolloverOptions_SP') {
                        IsRMDRollOver_SP = false;
                    }                   
                    break;
            }
            // START : SB | 2016.11.29 | YRS-AT-3022 | Enable RolloverToOwnIRA checkbox for Rollover options
            $("#" + bitIsRolloverToOwnIRA).attr('checked', false)       //  SB | 2016.11.29 | YRS-AT-3022 | Uncheck RolloverToOwnIRA checkbox
            if (varOption == "all" || varOption == "taxable" || varOption == "Partial") {
                if ($("#" + bitIsRolloverToOwnIRA).is(':disabled')) {
                    $("#" + bitIsRolloverToOwnIRA).removeAttr('disabled');
                }
            }
            else
            {
                $("#" + bitIsRolloverToOwnIRA).attr('disabled', true);
            }
            // END : SB | 2016.11.29 | YRS-AT-3022 | Enable RolloverToOwnIRA checkbox for Rollover options

            //YRS-AT-2911 - Commeneted existing function and added RMD taxable and non-taxable parameters
            //AdjustRolloverValues(dblTaxable, dblNonTaxable, varTaxable, varRolloverTaxable, varNonTaxable, varRolloverNonTaxable, varTaxRate, varTax, varNet, varRolloverNet)
            AdjustRolloverValues(dblTaxable, dblNonTaxable, varTaxable, varRolloverTaxable, varNonTaxable, varRolloverNonTaxable, varTaxRate, varTax, varNet, varRolloverNet, dblRMDTaxable, dblRMDNonTaxable)
            //End - Manthan | 2016.07.01 | YRS-AT-2911 | YRS enhancement-RMD for benes if nontaxable greater than RMD amount allow rollover taxable only
        }

        
        // START | SR | 2017.05.04 | YRS-AT-3173 - YRS enh: if RMD amount greater than non-taxable give warning for " Taxable Only" selection (TrackIT 27473)  
        function DisplayAlertMessage(RolloverOptions, dblTaxable, dblNonTaxable, varTaxable, varRolloverTaxable, varNonTaxable, varRolloverNonTaxable, varTaxRate, varTax, varNet, varRolloverNet, varRolloverPayeeName, varRolloverPartialAmount, dblRMDTaxable, dblRMDNonTaxable, bitIsRolloverToOwnIRA) {   
            //var msg = 'Refund Beneficiary';
            var div = $("<div>Because the RMD is greater than the total non-taxable amount, and RMD cannot be rolled over, the distribution paid directly to the beneficiary will include some taxable money. Do you wish to proceed?</div>");
            div.dialog({
                title: "Refund Beneficiary",
                width: 500, height: 190,
                buttons: [
                            {
                                text: "Yes",
                                click: function () {
                                    if (RolloverOpt != undefined && RolloverOpt == "RolloverOptions_RP_2") {
                                        $("#<%=RolloverOptions_SP.ClientID%>").find('input').prop('disabled', false);
                                    }
                                    else if (RolloverOpt != undefined && RolloverOpt == "RolloverOptions_SP_2") {
                                        $("#<%=RolloverOptions_RP.ClientID%>").find('input').prop('disabled', false);
                                    }
                                    div.dialog("close");
                                }
                            },
                            {
                                text: "No",
                                click: function (){
                                                    
                                                    if (RolloverOpt != undefined && RolloverOpt == "RolloverOptions_RP_2") {
                                                        $("#<%=RolloverOptions_SP.ClientID%>").find('input').prop('disabled', false);
                                                        $("#RolloverOptions_RP_0").attr('checked', true);
                                                    }
                                                    else if  (RolloverOpt != undefined && RolloverOpt == "RolloverOptions_SP_2") {
                                                        $("#<%=RolloverOptions_RP.ClientID%>").find('input').prop('disabled', false);
                                                        $("#RolloverOptions_SP_0").attr('checked', true);
                                                    }
                                                    ResetRolloverOptions(RolloverOptions, dblTaxable, dblNonTaxable, varTaxable, varRolloverTaxable, varNonTaxable, varRolloverNonTaxable, varTaxRate, varTax, varNet, varRolloverNet, varRolloverPayeeName, varRolloverPartialAmount, dblRMDTaxable, dblRMDNonTaxable, bitIsRolloverToOwnIRA);    // SB | 2016.11.29 | YRS-AT-3022 | Added parameter RolloverToOwnIRA checkbox id                                                    
                                                    div.dialog("close");
                                        }
                            }]
            });

        }        
        
        function ResetRolloverOptions(RolloverOptions, dblTaxable, dblNonTaxable, varTaxable, varRolloverTaxable, varNonTaxable, varRolloverNonTaxable, varTaxRate, varTax, varNet, varRolloverNet, varRolloverPayeeName, varRolloverPartialAmount, dblRMDTaxable, dblRMDNonTaxable, bitIsRolloverToOwnIRA) {   // SB | 2016.11.29 | YRS-AT-3022 | Added parameter RolloverToOwnIRA checkbox id 
            // YRS-AT-2911 - Initialized variables to store RMD control object
            var txtRMDTaxRate_RP = $("#txtRMDTaxRate_RP")[0];
            var txtRMDTaxRate_SP = $("#txtRMDTaxRate_SP")[0];
            var varOption = $("input[name='" + RolloverOptions + "']:checked").val();
            // YRS-AT-2911 - Initialized variables to store rollover option selected plan wise
            var RolloverOpt_RP = $("input[name='RolloverOptions_RP']:checked").val();
            var RolloverOpt_SP = $("input[name='RolloverOptions_SP']:checked").val();
            RolloverOpt = undefined;
            IsRMDRollOver_RP = undefined;
            IsRMDRollOver_SP = undefined;

            switch (varOption) {
                case "all":
                    break;
                case "taxable":
                    break;
                default:
                    $("#" + varRolloverTaxable).val(0);
                    $("#" + varRolloverNonTaxable).val(0);                   
                    $("#" + varRolloverPayeeName).attr('disabled', 'disabled').val('');
                    $("#" + varRolloverPartialAmount).attr('disabled', 'disabled').val(0);
                    //YRS-AT-2911- Setting the variable value to false if rollover none option selected
                    if (txtRMDTaxRate_RP != undefined && RolloverOptions == 'RolloverOptions_RP') {
                        IsRMDRollOver_RP = false;
                    }
                    else if (txtRMDTaxRate_SP != undefined && RolloverOptions == 'RolloverOptions_SP') {
                        IsRMDRollOver_SP = false;
                    }
                    break;
            }
          
            $("#" + bitIsRolloverToOwnIRA).attr('checked', false)     
            if (varOption == "all" || varOption == "taxable" || varOption == "Partial") {
                if ($("#" + bitIsRolloverToOwnIRA).is(':disabled')) {
                    $("#" + bitIsRolloverToOwnIRA).removeAttr('disabled');
                }
            }
            else {
                $("#" + bitIsRolloverToOwnIRA).attr('disabled', true);
            }           
            AdjustRolloverValues(dblTaxable, dblNonTaxable, varTaxable, varRolloverTaxable, varNonTaxable, varRolloverNonTaxable, varTaxRate, varTax, varNet, varRolloverNet, dblRMDTaxable, dblRMDNonTaxable)            
        }
        // END | SR | 2017.05.04 | YRS-AT-3173 - YRS enh: if RMD amount greater than non-taxable give warning for " Taxable Only" selection (TrackIT 27473)  

        function Calculate_TaxRate(varTaxRate, varTaxable, varNonTaxable, varTax, varNet) {
            var tax, taxable, taxRate;
            tax = parseFloat($('#' + varTax).val());
            taxable = parseFloat($('#' + varTaxable).val());
            if (isNaN(taxable) || taxable == 0) { taxRate = 0; tax = 0; }
            else if (isNaN(tax)) { taxRate = 0; tax = 0; }
            else if (tax > taxable) { tax = taxable; taxRate = 100; alert('Tax cannot be greater than Taxable'); }
            else { taxRate = tax / taxable * 100; }

            $('#' + varTaxRate).val(taxRate);
            $('#' + varTax).val(tax);
            $('#' + varNet).val(roundVal(taxable + parseFloat($('#' + varNonTaxable).val()) - tax));
        }

        function Calculate_Tax(varTaxRate, varTaxable, varNonTaxable, varTax, varNet) {
            if (($('#' + varTaxRate).val() != "") && ($('#' + varTaxable).val() != "")) {
                $('#' + varTax).val(roundVal(parseFloat($('#' + varTaxable).val()) * (parseFloat($('#' + varTaxRate).val()) / 100)));
            }
            $('#' + varNet).val(roundVal((parseFloat($('#' + varTaxable).val()) + parseFloat($('#' + varNonTaxable).val())) - parseFloat($('#' + varTax).val())));
            //BS:2011.09.09:BT:739--put condition for rollover retrie and saving 
            if ((($("#TextBoxNet_RP").val()) < 0.01) && (($("#TextBoxTaxable_RP").val()) != 0 || ($("#TextBoxNonTaxable_RP").val()) != 0)) {
                alert('Net amount can not be less than $0.01.');
                return;
            }
            if ((($("#TextBoxNet_SP").val()) < 0.01) && (($("#TextBoxTaxable_SP").val()) != 0 || ($("#TextBoxNonTaxable_SP").val()) != 0)) {
                alert('Net amount can not be less than $0.01.');
                return;
            }
        }

        function Calculate_Tax_RP() {
            if ((parseFloat($("#TextBoxTaxRate_RP").val()) < 0) || (parseFloat($("#TextBoxTaxRate_RP").val()) > 100)) {
                alert('Tax rate must be between 0 and 100.');
                return;
            }
            Calculate_Tax('TextBoxTaxRate_RP', 'TextBoxTaxable_RP', 'TextBoxNonTaxable_RP', 'TextBoxTax_RP', 'TextBoxNet_RP');
            Calculate_Total();
        }

        function Calculate_Tax_SP() {
            if ((parseFloat($("#TextBoxTaxRate_SP").val()) < 0) || (parseFloat($("#TextBoxTaxRate_SP").val()) > 100)) {
                alert('Tax rate must be between 0 and 100.');
                return;
            }
            Calculate_Tax('TextBoxTaxRate_SP', 'TextBoxTaxable_SP', 'TextBoxNonTaxable_SP', 'TextBoxTax_SP', 'TextBoxNet_SP');
            Calculate_Total();
        }

        function Calculate_TaxRate_RP() {
            if ((parseFloat($("#TextBoxTax_RP").val()) < 0) || (parseFloat($("#TextBoxTax_RP").val()) > parseFloat($("#TextBoxTaxable_RP").val()))) {
                alert('Tax amount must be between 0 and value in Taxable Box');
                return;
            }
            Calculate_TaxRate('TextBoxTaxRate_RP', 'TextBoxTaxable_RP', 'TextBoxNonTaxable_RP', 'TextBoxTax_RP', 'TextBoxNet_RP');
            Calculate_Total();
        }

        function Calculate_TaxRate_SP() {
            if ((parseFloat($("#TextBoxTax_SP").val()) < 0) || (parseFloat($("#TextBoxTax_SP").val()) > parseFloat($("#TextBoxTaxable_SP").val()))) {
                alert('Tax amount must be between 0 and value in Taxable Box');
                return;
            }
            Calculate_TaxRate('TextBoxTaxRate_SP', 'TextBoxTaxable_SP', 'TextBoxNonTaxable_SP', 'TextBoxTax_SP', 'TextBoxNet_SP');
            Calculate_Total();
        }

        function roundVal(val) {
            var dec = 2;
            var result = Math.round(val * Math.pow(10, dec)) / Math.pow(10, dec);
            return result;
        }
        //Start - Manthan | 2016.07.01 | YRS-AT-2911 | YRS enhancement-RMD for benes if nontaxable greater than RMD amount allow rollover taxable only
        //YRS-AT-2911 - Commented existing function and added RMD taxable and non-taxable parameters
        //function AdjustRolloverValues(dblTaxable, dblNonTaxable, varTaxable, varRolloverTaxable, varNonTaxable, varRolloverNonTaxable, varTaxRate, varTax, varNet, varRolloverNet) {
        function AdjustRolloverValues(dblTaxable, dblNonTaxable, varTaxable, varRolloverTaxable, varNonTaxable, varRolloverNonTaxable, varTaxRate, varTax, varNet, varRolloverNet, dblRMDTaxable, dblRMDNonTaxable) {
            //YRS-AT-2911 - Commented existing code
            //if (parseFloat($("#" + varRolloverTaxable).val()) > parseFloat(dblTaxable) || parseFloat(dblTaxable) == 0 || (isNaN($('#' + varRolloverTaxable).val()))) {
            //    $("#" + varRolloverTaxable).val(0);
            //}
            //if (parseFloat($("#" + varRolloverNonTaxable).val()) > parseFloat(dblNonTaxable) || parseFloat(dblNonTaxable) == 0 || (isNaN($("#" + varRolloverNonTaxable).val()))) {
            //    $("#" + varRolloverNonTaxable).val(0);
            //}

            //YRS-AT-2911 - Implemeted logic to recalulate values if RMD rollover option available and rollover taxable only option selected
            if (IsRMDRollOver_RP != undefined || IsRMDRollOver_SP != undefined) {                
                if (IsRMDRollOver_RP == true && RolloverOpt != undefined && RolloverOpt == "RolloverOptions_RP_2") {
                    $("#txtRMDTaxable_RP").val(0);
                    $("#txtRMDTax_RP").val(0);
                    $("#TextBoxTaxable_RP").val(0);
                    <%--START: MMR | 2017.12.11 | YRS-AT-3742 | Commented as below logic getting handled on server side
                    $("#TextBoxNonTaxable_RP").val(parseFloat(dblNonTaxable) - parseFloat(dblRMDTaxable));
                    $("#txtRMDNonTaxable_RP").val(parseFloat(dblRMDNonTaxable) + parseFloat(dblRMDTaxable));
                    END: MMR | 2017.12.11 | YRS-AT-3742 | Commented as below logic getting handled on server side--%>
                    $("#txtRMDNet_RP").val((parseFloat($("#txtRMDTaxable_RP").val()) + parseFloat($("#txtRMDNonTaxable_RP").val())) - parseFloat($("#txtRMDTax_RP").val()));
                    $("#txtRMDTaxable_RP").addClass("TwoDecimals");
                    $("#txtRMDTax_RP").addClass("TwoDecimals");                    
                }
                else if (IsRMDRollOver_RP == false) {
                    $("#txtRMDTax_RP").val(roundVal(parseFloat(dblRMDTaxable) * (parseFloat($("#txtRMDTaxRate_RP").val()) / 100)));
                    $("#txtRMDTaxable_RP").val(roundVal(parseFloat(dblRMDTaxable)));
                    <%--$("#txtRMDNonTaxable_RP").val(roundVal(parseFloat(dblRMDNonTaxable)));--%> <%--MMR | 2017.12.11 | YRS-AT-3742 | Commented as below logic getting handled on server side --%>
                    $("#txtRMDNet_RP").val(roundVal((parseFloat(dblRMDTaxable) + parseFloat(dblRMDNonTaxable)) - parseFloat($("#txtRMDTax_RP").val())));
                    $("#TextBoxTaxable_RP").val(roundVal(parseFloat(dblTaxable) - parseFloat($("#RolloverTaxable_RP").val())));
                    <%--$("#TextBoxNonTaxable_RP").val(parseFloat(dblNonTaxable));--%> <%--MMR | 2017.12.11 | YRS-AT-3742 | Commented as below logic getting handled on server side--%>
                }
                    if (IsRMDRollOver_SP == true && RolloverOpt != undefined && RolloverOpt == "RolloverOptions_SP_2") {
                        $("#txtRMDTaxable_SP").val(0);                        
                        $("#txtRMDTax_SP").val(0);
                        $("#TextBoxTaxable_SP").val(0);
                        <%--START: MMR | 2017.12.11 | YRS-AT-3742 | Commented as below logic getting handled on server side
                        $("#TextBoxNonTaxable_SP").val(parseFloat(dblNonTaxable) - parseFloat(dblRMDTaxable));
                        $("#txtRMDNonTaxable_SP").val(parseFloat(dblRMDNonTaxable) + parseFloat(dblRMDTaxable));
                        END: MMR | 2017.12.11 | YRS-AT-3742 | Commented as below logic getting handled on server side--%>
                        $("#txtRMDNet_SP").val((parseFloat($("#txtRMDTaxable_SP").val()) + parseFloat($("#txtRMDNonTaxable_SP").val())) - parseFloat($("#txtRMDTax_SP").val()));
                        $("#txtRMDTaxable_SP").addClass("TwoDecimals");                        
                        $("#txtRMDTax_SP").addClass("TwoDecimals");                       
                    }
                    else if (IsRMDRollOver_SP == false) {
                        $("#txtRMDTax_SP").val(roundVal(parseFloat(dblRMDTaxable) * (parseFloat($("#txtRMDTaxRate_SP").val()) / 100)));
                        $("#txtRMDTaxable_SP").val(roundVal(parseFloat(dblRMDTaxable)));
                        <%--$("#txtRMDNonTaxable_SP").val(roundVal(parseFloat(dblRMDNonTaxable)));--%> <%--MMR | 2017.12.11 | YRS-AT-3742 | Commented as below logic getting handled on server side --%>
                        $("#txtRMDNet_SP").val(roundVal((parseFloat(dblRMDTaxable) + parseFloat(dblRMDNonTaxable)) - parseFloat($("#txtRMDTax_SP").val())));
                        $("#TextBoxTaxable_SP").val(roundVal(parseFloat(dblTaxable) - parseFloat($("#RolloverTaxable_SP").val())));
                        <%--$("#TextBoxNonTaxable_SP").val(parseFloat(dblNonTaxable));--%> <%--MMR | 2017.12.11 | YRS-AT-3742 | Commented as below logic getting handled on server side --%>
                    }
                //$("#" + varNonTaxable).val(roundVal(parseFloat(dblNonTaxable) - parseFloat($("#" + varRolloverNonTaxable).val())));
                $("#" + varTax).val(roundVal(parseFloat($("#" + varTaxable).val()) * (parseFloat($("#" + varTaxRate).val()) / 100)));
                $("#" + varNet).val(roundVal(parseFloat($("#" + varTaxable).val()) + parseFloat($("#" + varNonTaxable).val()) - parseFloat($("#" + varTax).val())));
                $("#" + varRolloverNet).val(roundVal(parseFloat($("#" + varRolloverTaxable).val()) + parseFloat($("#" + varRolloverNonTaxable).val())));
                Calculate_Total();
                $(".TwoDecimals").each(function () { $(this).val(CurrencyFormatted($(this).val())); });
            }
            else {               
                $("#" + varTaxable).val(roundVal(parseFloat(dblTaxable) - parseFloat($("#" + varRolloverTaxable).val())));
                $("#" + varNonTaxable).val(roundVal(parseFloat(dblNonTaxable) - parseFloat($("#" + varRolloverNonTaxable).val())));
                $("#" + varTax).val(roundVal(parseFloat($("#" + varTaxable).val()) * (parseFloat($("#" + varTaxRate).val()) / 100)));
                $("#" + varNet).val(roundVal(parseFloat($("#" + varTaxable).val()) + parseFloat($("#" + varNonTaxable).val()) - parseFloat($("#" + varTax).val())));
                $("#" + varRolloverNet).val(roundVal(parseFloat($("#" + varRolloverTaxable).val()) + parseFloat($("#" + varRolloverNonTaxable).val())));
                Calculate_Total();
                $(".TwoDecimals").each(function () { $(this).val(CurrencyFormatted($(this).val())); });
            }           
            //End - Manthan | 2016.07.01 | YRS-AT-2911 | YRS enhancement-RMD for benes if nontaxable greater than RMD amount allow rollover taxable only
        }
        //Start - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
        //function Calculate_Taxable_RP(dblTaxable, dblNonTaxable) {
        function Calculate_Taxable_RP(dblTaxable, dblNonTaxable, dblRMDTaxable, dblRMDNonTaxable) {
        //End - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
            if (parseFloat($("#RolloverTaxable_RP").val()) > parseFloat(dblTaxable)) {
                alert('Amount can not be greater than Taxable amount.');
            }
            if (isNaN($("#RolloverTaxable_RP").val())) {
                alert('Invalid entry');
            }
            //Start - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
            //AdjustRolloverValues(dblTaxable, dblNonTaxable, 'TextBoxTaxable_RP', 'RolloverTaxable_RP', 'TextBoxNonTaxable_RP', 'RolloverNonTaxable_RP', 'TextBoxTaxRate_RP', 'TextBoxTax_RP', 'TextBoxNet_RP', 'RolloverNet_RP');
            AdjustRolloverValues(dblTaxable, dblNonTaxable, 'TextBoxTaxable_RP', 'RolloverTaxable_RP', 'TextBoxNonTaxable_RP', 'RolloverNonTaxable_RP', 'TextBoxTaxRate_RP', 'TextBoxTax_RP', 'TextBoxNet_RP', 'RolloverNet_RP', dblRMDTaxable, dblRMDNonTaxable);
            //End - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
        }
        //Start - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
        //function Calculate_NonTaxable_RP(dblTaxable, dblNonTaxable) {
        function Calculate_NonTaxable_RP(dblTaxable, dblNonTaxable, dblRMDTaxable, dblRMDNonTaxable) {
        //End - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
            if (parseFloat($("#RolloverNonTaxable_RP").val()) > parseFloat(dblNonTaxable)) {
                alert('Amount can not be greater than Non-Taxable amount.');
            }
            if (isNaN($("#RolloverNonTaxable_RP").val())) {
                alert('Invalid entry');
            }
            //Start - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
            //AdjustRolloverValues(dblTaxable, dblNonTaxable, 'TextBoxTaxable_RP', 'RolloverTaxable_RP', 'TextBoxNonTaxable_RP', 'RolloverNonTaxable_RP', 'TextBoxTaxRate_RP', 'TextBoxTax_RP', 'TextBoxNet_RP', 'RolloverNet_RP');
            AdjustRolloverValues(dblTaxable, dblNonTaxable, 'TextBoxTaxable_RP', 'RolloverTaxable_RP', 'TextBoxNonTaxable_RP', 'RolloverNonTaxable_RP', 'TextBoxTaxRate_RP', 'TextBoxTax_RP', 'TextBoxNet_RP', 'RolloverNet_RP', dblRMDTaxable, dblRMDNonTaxable);
            //End - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
        }
        //Start - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
        //function Calculate_Taxable_SP(dblTaxable, dblNonTaxable) {
        function Calculate_Taxable_SP(dblTaxable, dblNonTaxable, dblRMDTaxable, dblRMDNonTaxable) {
        //End - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
            if (parseFloat($("#RolloverTaxable_SP").val()) > parseFloat(dblTaxable)) {
                alert('Amount can not be greater than Taxable amount.');
            }
            if (isNaN($("#RolloverTaxable_SP").val())) {
                alert('Invalid entry.');
            }
            //Start - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
            //AdjustRolloverValues(dblTaxable, dblNonTaxable, 'TextBoxTaxable_SP', 'RolloverTaxable_SP', 'TextBoxNonTaxable_SP', 'RolloverNonTaxable_SP', 'TextBoxTaxRate_SP', 'TextBoxTax_SP', 'TextBoxNet_SP', 'RolloverNet_SP');
            AdjustRolloverValues(dblTaxable, dblNonTaxable, 'TextBoxTaxable_SP', 'RolloverTaxable_SP', 'TextBoxNonTaxable_SP', 'RolloverNonTaxable_SP', 'TextBoxTaxRate_SP', 'TextBoxTax_SP', 'TextBoxNet_SP', 'RolloverNet_SP', dblRMDTaxable, dblRMDNonTaxable);
            //End - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
        }
        //Start - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
        //function Calculate_NonTaxable_SP(dblTaxable, dblNonTaxable) {
        function Calculate_NonTaxable_SP(dblTaxable, dblNonTaxable, dblRMDTaxable, dblRMDNonTaxable) {
        //End - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
            if (parseFloat($("#RolloverNonTaxable_SP").val()) > parseFloat(dblNonTaxable)) {
                alert('Amount can not be greater than Non-Taxable amount.');
            }
            if (isNaN($("#RolloverNonTaxable_SP").val())) {
                alert('Invalid entry.');
            }
            //Start - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
            //AdjustRolloverValues(dblTaxable, dblNonTaxable, 'TextBoxTaxable_SP', 'RolloverTaxable_SP', 'TextBoxNonTaxable_SP', 'RolloverNonTaxable_SP', 'TextBoxTaxRate_SP', 'TextBoxTax_SP', 'TextBoxNet_SP', 'RolloverNet_SP');
            AdjustRolloverValues(dblTaxable, dblNonTaxable, 'TextBoxTaxable_SP', 'RolloverTaxable_SP', 'TextBoxNonTaxable_SP', 'RolloverNonTaxable_SP', 'TextBoxTaxRate_SP', 'TextBoxTax_SP', 'TextBoxNet_SP', 'RolloverNet_SP', dblRMDTaxable, dblRMDNonTaxable);
            //End - Manthan | 2016.07.01 | YRS-AT-2911 | Commented existing function and added RMD taxable and non-taxable values
        }

        function Calculate_Total() {
            var dbltax_RP = 0;
            var dbltax_SP = 0;
            var dblTaxable_RP = 0;
            var dblNonTaxable_RP = 0;
            var dblTaxable_SP = 0;
            var dblNonTaxable_SP = 0;
            var dblRollTax_RP = 0;
            var dblRollNT_RP = 0;
            var dblRollTax_SP = 0;
            var dblRollNT_SP = 0;
            //Start-SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries
            var dblRMDTaxable_RP = 0;
            var dblRMDNonTaxable_RP = 0;
            var dblRMDTaxable_SP = 0;
            var dblRMDNonTaxable_SP = 0;
            var dblRMDTax_RP = 0;
            var dblRMDTax_SP = 0;
            //End-SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries

            if (!isNaN($("#TextBoxTax_RP").val())) {
                dbltax_RP = parseFloat($("#TextBoxTax_RP").val());
            }
            if (!isNaN($("#TextBoxTax_SP").val())) {
                dbltax_SP = parseFloat($("#TextBoxTax_SP").val());
            }
            if (!isNaN($("#TextBoxTaxable_RP").val())) {
                dblTaxable_RP = parseFloat($("#TextBoxTaxable_RP").val());
            }
            if (!isNaN($("#TextBoxNonTaxable_RP").val())) {
                dblNonTaxable_RP = parseFloat($("#TextBoxNonTaxable_RP").val());
            }
            if (!isNaN($("#TextBoxTaxable_SP").val())) {
                dblTaxable_SP = parseFloat($("#TextBoxTaxable_SP").val());
            }
            if (!isNaN($("#TextBoxNonTaxable_SP").val())) {
                dblNonTaxable_SP = parseFloat($("#TextBoxNonTaxable_SP").val());
            }
            if (!isNaN($("#RolloverTaxable_RP").val())) {
                dblRollTax_RP = parseFloat($("#RolloverTaxable_RP").val());
            }
            if (!isNaN($("#RolloverNonTaxable_RP").val())) {
                dblRollNT_RP = parseFloat($("#RolloverNonTaxable_RP").val());
            }
            if (!isNaN($("#RolloverTaxable_SP").val())) {
                dblRollTax_SP = parseFloat($("#RolloverTaxable_SP").val());
            }
            if (!isNaN($("#RolloverNonTaxable_SP").val())) {
                dblRollNT_SP = parseFloat($("#RolloverNonTaxable_SP").val());
            }
            //Start-SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries
            if (!isNaN($("#txtRMDTax_RP").val())) {
                dblRMDTax_RP = parseFloat($("#txtRMDTax_RP").val());
            }

            if (!isNaN($("#txtRMDTax_SP").val())) {
                dblRMDTax_SP = parseFloat($("#txtRMDTax_SP").val());
            }

            if (!isNaN($("#txtRMDTaxable_RP").val())) {
                dblRMDTaxable_RP = parseFloat($("#txtRMDTaxable_RP").val());
            }

            if (!isNaN($("#txtRMDTaxable_SP").val())) {
                dblRMDTaxable_SP = parseFloat($("#txtRMDTaxable_SP").val());
            }

            if (!isNaN($("#txtRMDNonTaxable_RP").val())) {
                dblRMDNonTaxable_RP = parseFloat($("#txtRMDNonTaxable_RP").val());
            }

            if (!isNaN($("#txtRMDNonTaxable_SP").val())) {
                dblRMDNonTaxable_SP = parseFloat($("#txtRMDNonTaxable_SP").val());
            }


            //End-SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries
            $("#TextboxTotalTax").val(roundVal(dbltax_RP + dbltax_SP + dblRMDTax_RP + dblRMDTax_SP));
            $("#TextboxTotalTaxable").val(roundVal(dblTaxable_RP + dblTaxable_SP + dblRollTax_RP + dblRollTax_SP + dblRMDTaxable_RP + dblRMDTaxable_SP));
            $("#TextboxTotalNonTaxable").val(roundVal(dblNonTaxable_RP + dblNonTaxable_SP + dblRollNT_RP + dblRollNT_SP + dblRMDNonTaxable_RP + dblRMDNonTaxable_SP));
            <%--Start - Manthan | 2016.04.22 | YRS-AT-2206 | Reducing deductions amt from total payable amt  --%>
            //$("#TextboxTotalNet").val(roundVal(parseFloat($("#TextboxTotalTaxable").val()) + parseFloat($("#TextboxTotalNonTaxable").val()) - parseFloat($("#TextboxTotalTax").val()))); 
            $("#TextboxTotalNet").val(roundVal(parseFloat($("#TextboxTotalTaxable").val()) + parseFloat($("#TextboxTotalNonTaxable").val()) - parseFloat($("#TextboxTotalTax").val()) - parseFloat($("#TextboxDeductions").val())));
            <%--End - Manthan | 2016.04.22 | YRS-AT-2206 | Reducing deductions amt from total payable amt  --%>

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
        // ''SR:2011.01.10 - Ends here

        //Start - Manthan | 2016.07.01 | YRS-AT-2911 | Commeneted existing function and added RMD taxable and non-taxable values
        //function Calculate_Partial_RP(dblTaxable, dblNonTaxable) {
        function Calculate_Partial_RP(dblTaxable, dblNonTaxable, dblRMDTaxable, dblRMDNonTaxable) {
        //End - Manthan | 2016.07.01 | YRS-AT-2911 | Commeneted existing function and added RMD taxable and non-taxable values
            if (isNaN($("#RolloverPartialAmount_RP").val())) {
                alert('Invalid entry');
                $("#RolloverPartialAmount_RP, #RolloverTaxable_RP, #RolloverNonTaxable_RP").val(0);
            }
            var partialAmount = parseFloat($("#RolloverPartialAmount_RP").val());
            var totalAmount = dblTaxable + dblNonTaxable;
            if (partialAmount > totalAmount || partialAmount < 0) {
                alert('Amount must be a positive value not greater than the sum of taxable and non-taxable money.');
                $("#RolloverPartialAmount_RP, #RolloverTaxable_RP, #RolloverNonTaxable_RP").val(0);
            }
            else {
                //Start:2014.10.07/SR/BT 2672/YRS 5.0-2422:allow rollovoers of pre-tax money only.
                if (partialAmount <= dblTaxable) {
                    $("#RolloverTaxable_RP").val(partialAmount);
                    $("#RolloverNonTaxable_RP").val(0);
                }
                if (partialAmount > dblTaxable) {
                    $("#RolloverTaxable_RP").val(dblTaxable);
                    $("#RolloverNonTaxable_RP").val(roundVal(partialAmount - dblTaxable));
                }
                //$("#RolloverNonTaxable_RP").val(dblNonTaxable * partialAmount / totalAmount);
                //$("#RolloverTaxable_RP").val(roundVal(partialAmount - parseFloat($("#RolloverNonTaxable_RP").val()))); //SR:2014.05.27 - BT 2550: RoundVal Method applied for further comparision
                //End:2014.10.07/SR/BT 2672/YRS 5.0-2422:allow rollovoers of pre-tax money only              
            }
            IsRMDRollOver_RP = undefined; //Start - MMR | YRS-AT-2911 | 2016.07.01 |Setting undefined value to variable for calculation when specific amt taken for rollover
            //Start - Manthan | 2016.07.01 | YRS-AT-2911 | Commeneted existing function and added RMD taxable and non-taxable values
            //AdjustRolloverValues(dblTaxable, dblNonTaxable, 'TextBoxTaxable_RP', 'RolloverTaxable_RP', 'TextBoxNonTaxable_RP', 'RolloverNonTaxable_RP', 'TextBoxTaxRate_RP', 'TextBoxTax_RP', 'TextBoxNet_RP', 'RolloverNet_RP');
            AdjustRolloverValues(dblTaxable, dblNonTaxable, 'TextBoxTaxable_RP', 'RolloverTaxable_RP', 'TextBoxNonTaxable_RP', 'RolloverNonTaxable_RP', 'TextBoxTaxRate_RP', 'TextBoxTax_RP', 'TextBoxNet_RP', 'RolloverNet_RP', dblRMDTaxable, dblRMDNonTaxable);
            //End - Manthan | 2016.07.01 | YRS-AT-2911 | Commeneted existing function and added RMD taxable and non-taxable values
        }
        //Start - Manthan | 2016.07.01 | YRS-AT-2911 | Commeneted existing function and added RMD taxable and non-taxable values
        //function Calculate_Partial_SP(dblTaxable, dblNonTaxable) {
        function Calculate_Partial_SP(dblTaxable, dblNonTaxable, dblRMDTaxable, dblRMDNonTaxable) {
        //End - Manthan | 2016.07.01 | YRS-AT-2911 | Commeneted existing function and added RMD taxable and non-taxable values
            if (isNaN($("#RolloverPartialAmount_SP").val())) {
                alert('Invalid entry');
                $("#RolloverPartialAmount_SP, #RolloverTaxable_SP, #RolloverNonTaxable_SP").val(0);
            }
            var partialAmount = parseFloat($("#RolloverPartialAmount_SP").val());
            var totalAmount = dblTaxable + dblNonTaxable;
            if (partialAmount > totalAmount || partialAmount < 0) {
                alert('Amount must be a positive value not greater than the sum of taxable and non-taxable money.');
                $("#RolloverPartialAmount_SP, #RolloverTaxable_SP, #RolloverNonTaxable_SP").val(0);
            }
            else {
                //Start:2014.10.07/SR/BT 2672/YRS 5.0-2422:allow rollovoers of pre-tax money only.
                if (partialAmount <= dblTaxable) {
                    $("#RolloverTaxable_SP").val(partialAmount);
                    $("#RolloverNonTaxable_SP").val(0);
                }
                if (partialAmount > dblTaxable) {
                    $("#RolloverTaxable_SP").val(dblTaxable);
                    $("#RolloverNonTaxable_SP").val(roundVal(partialAmount - dblTaxable));
                }           
            //$("#RolloverNonTaxable_SP").val(dblNonTaxable * partialAmount / totalAmount);
            //$("#RolloverTaxable_SP").val(roundVal(partialAmount - parseFloat($("#RolloverNonTaxable_SP").val()))); //SR:2014.05.27 - BT 2550: RoundVal Method applied for further comparision
            //End:2014.10.07/SR/BT 2672/YRS 5.0-2422:allow rollovoers of pre-tax money only
            }
            IsRMDRollOver_SP = undefined; //Start - MMR | YRS-AT-2911 | 2016.07.01 |Setting undefined value to variable for calculation when specific amt taken for rollover
            //Start - Manthan | 2016.07.01 | YRS-AT-2911 | Commeneted existing function and added RMD taxable and non-taxable values
            //AdjustRolloverValues(dblTaxable, dblNonTaxable, 'TextBoxTaxable_SP', 'RolloverTaxable_SP', 'TextBoxNonTaxable_SP', 'RolloverNonTaxable_SP', 'TextBoxTaxRate_SP', 'TextBoxTax_SP', 'TextBoxNet_SP', 'RolloverNet_SP');
            AdjustRolloverValues(dblTaxable, dblNonTaxable, 'TextBoxTaxable_SP', 'RolloverTaxable_SP', 'TextBoxNonTaxable_SP', 'RolloverNonTaxable_SP', 'TextBoxTaxRate_SP', 'TextBoxTax_SP', 'TextBoxNet_SP', 'RolloverNet_SP', dblRMDTaxable, dblRMDNonTaxable);
            //End - Manthan | 2016.07.01 | YRS-AT-2911 | Commeneted existing function and added RMD taxable and non-taxable values
        }

        //Start-SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries
        function Calculate_RMDTax_RP() {

            // START: SB | 2017.12.18 | YRS-AT-3756 | Check the RMD tax rate is not between 1 to 9 (Minimum RMD Tax Rate). Commenting old validation  code.
            //if ((parseFloat($("#txtRMDTaxRate_RP").val()) < 0) || (parseFloat($("#txtRMDTaxRate_RP").val()) > 100)) {
            //    alert('Tax rate must be between 0 and 100.');
            //    return;
            //}

            var varMinRMDTaxRate = $("#hdnMinRMDTaxRate").val();   
            var varRMDTaxRateErrorMessage = $("#hdnRMDTaxRateErrorMessage").val();
            var varRMDTaxToValidate = parseFloat($("#txtRMDTaxRate_RP").val());
           
            if (!((varRMDTaxToValidate == 0) || ((varRMDTaxToValidate >= varMinRMDTaxRate) && (varRMDTaxToValidate <= 100))))
            {
                alert(varRMDTaxRateErrorMessage);
                $("#txtRMDTaxRate_RP").val(varMinRMDTaxRate);
            }
            // END: SB | 2017.12.18 | YRS-AT-3756 | Check the RMD tax rate is not between 1 to 9 (Minimum RMD Tax Rate). Commenting old validation  code.

            Calculate_Tax('txtRMDTaxRate_RP', 'txtRMDTaxable_RP', 'txtRMDNonTaxable_RP', 'txtRMDTax_RP ', 'txtRMDNet_RP');
            Calculate_Total();
        }

        function Calculate_RMDTax_SP() {

            // START: SB | 2017.12.18 | YRS-AT-3756 | Check the RMD tax rate is not between 1 to 9 (Minimum RMD Tax Rate). Commenting old validation  code.
            //if ((parseFloat($("#txtRMDTaxRate_SP").val()) < 0) || (parseFloat($("#txtRMDTaxRate_SP").val()) > 100)) {
            //    alert('Tax rate must be between 0 and 100.');
            //    return;
            //}

            var varMinRMDTaxRate = $("#hdnMinRMDTaxRate").val();  
            var varRMDTaxRateErrorMessage = $("#hdnRMDTaxRateErrorMessage").val();
            var varRMDTaxToValidate = parseFloat($("#txtRMDTaxRate_SP").val());
           
            if (!((varRMDTaxToValidate == 0) || ((varRMDTaxToValidate >= varMinRMDTaxRate) && (varRMDTaxToValidate <= 100)))) {
                alert(varRMDTaxRateErrorMessage);
                $("#txtRMDTaxRate_SP").val(varMinRMDTaxRate);
            }
            // END: SB | 2017.12.18 | YRS-AT-3756 | Check the RMD tax rate is not between 1 to 9 (Minimum RMD Tax Rate). Commenting old validation  code.

            Calculate_Tax('txtRMDTaxRate_SP', 'txtRMDTaxable_SP', 'txtRMDNonTaxable_SP', 'txtRMDTax_SP ', 'txtRMDNet_SP');
            Calculate_Total();
        }

        function Calculate_RMDTaxRate_RP() {
            if ((parseFloat($("#txtRMDTax_RP").val()) < 0) || (parseFloat($("#txtRMDTax_RP").val()) > parseFloat($("#txtRMDTaxable_RP").val()))) {
                alert('Tax amount must be between 0 and value in Taxable Box');
                $("#txtRMDTax_RP").val(0)
                return;
            }
            Calculate_TaxRate('txtRMDTaxRate_RP', 'txtRMDTaxable_RP', 'txtRMDNonTaxable_RP', 'txtRMDTax_RP ', 'txtRMDNet_RP');
            Calculate_Total();
        }

        function Calculate_RMDTaxRate_SP() {
            if ((parseFloat($("#txtRMDTax_SP").val()) < 0) || (parseFloat($("#txtRMDTax_SP").val()) > parseFloat($("#txtRMDTaxable_SP").val()))) {
                alert('Tax amount must be between 0 and value in Taxable Box');
                $("#txtRMDTax_SP").val(0)
                return;
            }
            Calculate_TaxRate('txtRMDTaxRate_SP', 'txtRMDTaxable_SP', 'txtRMDNonTaxable_SP', 'txtRMDTax_SP ', 'txtRMDNet_SP');
            Calculate_Total();
        }

        //function openDialog(ht) {
        //    $("#divBeneficiaryRMDs").dialog({ height: ht, modal: true,resizable: false,draggable: false})
        //      $("#divBeneficiaryRMDs").dialog('open');
        //       return false;

        //}

        $(document).ready(function () {
            $("#divBeneficiaryRMDs").dialog
                        ({
                            modal: true,
                            autoOpen: false,
                            title: "RMD Breakup(Yearwise)",
                            width: 275,
                            buttons: [{ text: "Close", click: CloseBeneficiaryRMDs }]
                        });
            <%--Start - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>
            $('#divDeductions').dialog
                ({
                    autoOpen: false,
                    resizable: false,
                    draggable: true,
                    closeOnEscape: false,
                    close: false,
                    width: 300, height: 325,
                    title: "Deductions",
                    modal: true,
                    buttons: [{ text: "Save",click: SaveDeduction }, { text: "Cancel", click: CloseDeductionsDialog }],
                    open: function (type, data) {
                        $(this).parent().appendTo("form");
                        $('a.ui-dialog-titlebar-close').remove();
                    }
                });
            <%--End - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>
                     
        });

        function CloseBeneficiaryRMDs() {
            $("#divBeneficiaryRMDs").dialog('close');
        }
        //End-SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries

        function LoadBeneficiaryRMDs(Type) {           
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "RefundBeneficiary.aspx/Binddata",
                data: "{'Type':'" + Type + "'}",
                dataType: "json",
                success: function (data) {
                    var strData = $.parseJSON(data.d);
                    $("#gvBeneficiaryRMDs tr").remove();
                    $("#gvBeneficiaryRMDs").append("<tr class='DataGrid_HeaderStyle'><td style='width:50;text-align:left'>Year</td><td style='width:100;text-align:left'>RMD Taxable</td><td style='width:110;text-align:left'>RMD NonTaxable</td></tr>");
                    for (var i = 0; i < strData.length; i++) {
                        if (i % 2 == 0) {
                            $("#gvBeneficiaryRMDs").append("<tr class='DataGrid_AlternateStyle_temp'><td style='text-align:left'>" + strData[i].intYear + "</td><td style='text-align:right'>" + strData[i].RMDTaxableAmount + "</td><td style='text-align:right'>" + strData[i].RMDNonTaxableAmount + "</td></tr>");
                        }
                        else {
                            $("#gvBeneficiaryRMDs").append("<tr class='DataGrid_NormalStyle_temp'><td style='text-align:left'>" + strData[i].intYear + "</td><td style='text-align:right'>" + strData[i].RMDTaxableAmount + "</td><td style='text-align:right'>" + strData[i].RMDNonTaxableAmount + "</td></tr>");
                        }


                    }
                    if (strData.length <= 0) {
                        $("#divBeneficiaryRMDs").height($("#divBeneficiaryRMDs").height());
                        $("#divBeneficiaryRMDs").width($("#divBeneficiaryRMDs").width());
                    }
                    $("#gvBeneficiaryRMDs").css('background-color', 'gray');

                    $("#divBeneficiaryRMDs").dialog({ title: 'RMD Amounts by Year -' + Type, width: 300 })  //SR:2013.05.29-YRS 5.0-2188-Changed title on Mark's suggestion
                    $("#divBeneficiaryRMDs").dialog('open');
                },
                error: function (result) {

                }
            });
           
        }

         <%--Start - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>

        function ShowDeductionDialog(type) {           
            $('#divDeductions').dialog('open');
        }
        function ValidateNumeric() {
            if (!(event.keyCode == 46 || event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57))
            { event.returnValue = false; }
        }
        function CloseDeductionsDialog() {
            $("#divDeductions").dialog('close');
            HideMessage();
            ClearValues();
        }

        function EnableTextbox(rownumber) {          
            var txtFundCost = $($($("#dgDeductions input[id*='chkBoxDeduction']:checkbox")[rownumber]).closest('tr').find('input[id*="txtFundCostAmt"]'));

            txtFundCost.val('');
            if (txtFundCost.is(':disabled'))
            {
                txtFundCost.removeAttr('disabled');
            }
            else{
                txtFundCost.attr('disabled', true);
            }
        }

        function ShowMessage(strMessage, type) {

            $("#divMessage").html(strMessage);
            $("#divMessage").css('display', 'block');
            if (type == "error") {
                $("#divMessage")[0].className = "error-msg";
            }
        }

        function HideMessage(){
        $("#divMessage").css('display', 'none');
        }
        // Show validation message and save deductions on save button in dialog
        function SaveDeduction() {
            HideMessage();          
            if (!($("#<%=dgDeductions.ClientID%> input[id*='txtFundCostAmt']:text").is(':disabled')) && ($("#<%=dgDeductions.ClientID%> input[id*='txtFundCostAmt']:text").val() == "" || $("#<%=dgDeductions.ClientID%> input[id*='txtFundCostAmt']:text").val() == "0.00"))
            {
                ShowMessage('Please provide Fund Costs', 'error');
                return;
            }
            getSelectedDeductions();
            getTotalDeductions();
        }

        function ClearValues() {
            $("#<%=dgDeductions.ClientID%> input[id*='chkBoxDeduction']:checkbox:checked").removeAttr('checked');
            $("#<%=dgDeductions.ClientID%> input[id*='txtFundCostAmt']:text").val('');
            $("#<%=dgDeductions.ClientID%> input[id*='txtFundCostAmt']:text").prop('disabled', true);

        }
        // Display total amt of selected deductions from datagrid in deduction textbox
        function getTotalDeductions() {
            var totDed = 0.00;
            var lblAmt;
            var checkedCheckboxes = $("#<%=dgDeductions.ClientID%> input[id*='chkBoxDeduction']:checkbox:checked");
            checkedCheckboxes.each(function (chk) {
                var txtFundCost = $($(checkedCheckboxes[chk]).closest('tr').find('input[id*="txtFundCostAmt"]:text'));
                if (typeof (txtFundCost[0]) != "undefined" && !(txtFundCost.is(':disabled'))) {
                    totDed = parseFloat(totDed) + parseFloat(txtFundCost.val());
                }
                else {
                    lblAmt = $($(checkedCheckboxes[chk]).closest('tr').find('span[id*="lblAmount"]'));
                    totDed = parseFloat(totDed) + parseFloat(lblAmt.text());
                }
            });

            $("#TextboxDeductions").val(parseFloat(totDed).toFixed(2));
            $("#TextboxTotalNet").val(roundVal(parseFloat($("#TextboxTotalTaxable").val()) + parseFloat($("#TextboxTotalNonTaxable").val()) - parseFloat($("#TextboxTotalTax").val()) - parseFloat($("#TextboxDeductions").val()))); <%--Manthan | YRS-AT-2206 | 14.04.2016 --%>
            CloseDeductionsDialog();
        }
        // Display selected deductions in grid previously selected for first time on click of deductions link
        function selectedDedcutionValues(str) {
            if (str != null) {
                var strSplit = str.substring(0, str.length - 1).split("#");

                for (var i = 0 ; i < strSplit.length; i++) {
                    if (!(strSplit[i] == '')) {
                        if (strSplit[i].indexOf("|") > -1) {
                            var strtextfundcost = strSplit[i].split("|");
                            $("#" + strtextfundcost[0]).attr('checked', 'true');
                            var txtFndcosts = $($("#" + strtextfundcost[0]).closest('td').next('td').next('td').find('input[id*="txtFundCostAmt"]:text'));                           
                            txtFndcosts.prop('disabled', false);
                            txtFndcosts.val(strtextfundcost[1]);
                        }
                        else {
                            $("#" + strSplit[i]).attr('checked', 'true');
                        }
                    }
                }
            }
            ShowDeductionDialog('open');
        }
        //Concatenating checkbox control Id,description and amt values selected in grid in a string variable and accessing it through web method
        function getSelectedDeductions() {
            var checkedChkboxes = $("#<%=dgDeductions.ClientID%> input[id*='chkBoxDeduction']:checkbox:checked");
            var getDedVal = "";
            var getappendedDedVal = "";
            var getValue = "";
            checkedChkboxes.each(function (chk) {
                var getDesc = $($(checkedChkboxes[chk]).closest('td').next('td')).text();
                var txt = $($(checkedChkboxes[chk]).closest('tr').find('input[id*="txtFundCostAmt"]:text'));
                if (typeof (txt[0]) != "undefined" && !(txt.is(':disabled'))) {
                    getValue = txt.val();
                }
                else {
                    getValue = $($(checkedChkboxes[chk]).closest('td').next('td').next('td')).text();

                }
                var getChkID = $(this).attr('id');
                if (getDesc == 'Fund Costs') {
                    getDedVal = getChkID + ":" + getDesc + ":" + getValue;
                }
                else {
                    getDedVal = getChkID + ":" + getDesc;
                }

                getappendedDedVal += getDedVal.concat("", "##");
            });

            var strDedVal = getappendedDedVal.substring(0, getappendedDedVal.length - 2);
            $.ajax({
                type: "POST",
                url: "RefundBeneficiary.aspx/SaveDeductionValues",
                data: "{'strDeductionval':'" + strDedVal + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    CloseDeductionsDialog();

                },
                failure: function (msg) {
                    ShowMessage(msg.d, "error");
                }
            });
        }
        //Calling web method to get values for selected deductions in grid
        function getSelectedDedValues() {
            $.ajax({
                type: "POST",
                url: "RefundBeneficiary.aspx/GetSelectedDeductionVal",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var str = msg.d;
                    selectedDedcutionValues(str);
                },
                failure: function (msg) {
                    ShowMessage(msg.d, "error");
                }
            });
        }

        <%--End - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>                                
    </script>
<body>
    <form id="Form1" method="post" runat="server" submitdisabledcontrols="true">
        <div class="Div_Center">
            <table class="Table_WithoutBorder" cellspacing="0" width="700">
                <tr>
                    <td class="Td_BackGroundColorMenu" align="left">
                        <cc1:Menu ID="MenuRetireesInformation" runat="server" mouseovercssclass="MouseOver"
                            MenuFadeDelay="1" zIndex="100" DefaultMouseUpCssClass="mouseup" DefaultMouseOverCssClass="mouseover"
                            DefaultMouseDownCssClass="mousedown" DefaultCssClass="menuitem" CssClass="menustyle"
                            Width="700" Font-Names="Verdana" Cursor="Pointer" ItemSpacing="0" ItemPadding="4"
                            HighlightTopMenu="True" Layout="Horizontal">
                            <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
                        </cc1:Menu>
                    </td>
                </tr>
                <%--<tr>
					<td class="Td_HeadingFormContainer" align="left" colSpan="2"> Beneficiary Withdrawal 
						<asp:label id="LabelTitle" runat="server"></asp:label></td>
				</tr>--%>
                <tr>
                    <td class="Td_HeadingFormContainer" align="left" colspan="2">
                        <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
                    </td>
                </tr>
                <tr>
                <td>
                    &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <table class="Table_WithBorder" width="695">
            <tbody>
                <tr>
                    <td align="left">
                        <table cellspacing="0" width="694" border="0">
                            <tr>
                            <td>
                            </td>
                            </tr>
                            <tr>
                                <td>
                                    <table class="Table_WithOutBorder" id="tblRP" cellspacing="0" cellpadding="3" width="100%"
                                        border="0" runat="server">
                                        <tr>
                                       <%--SR:2014.07.15 -BT 2593: UI changes in Beneficiary information page. Changes made throghout the page --%>
                                        <td class="td_Text">
                                            Retirement Plan Options:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table id="Table6" border="0">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="LabelPayee1" runat="server" CssClass="Label_Small">Payee 1 : </asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxPayee1Name_RP" TabIndex="99" runat="server" CssClass="TextBox_Normal"
                                                                ReadOnly="True" Enabled="False" Width="336px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="Table7">
                                                    <tr>
                                                        <td align="left" width="200">&nbsp;
                                                        </td>
                                                        <td align="left" width="65">Tax rate
                                                        </td>
                                                        <td align="left" width="85">Taxable
                                                        </td>
                                                        <td align="left" width="85">Non-Taxable
                                                        </td>
                                                        <td align="left" width="85">Tax
                                                        </td>
                                                        <td align="left" width="85">Net
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">Refund Amount (by check):
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxTaxRate_RP" TabIndex="1" CssClass="TextBox_Normal" runat="server"
                                                                Width="53px" AutoPostBack="False" ReadOnly="False" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxTaxable_RP" TabIndex="1" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxNonTaxable_RP" TabIndex="1" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxTax_RP" TabIndex="1" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" AutoPostBack="False" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxNet_RP" TabIndex="1" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%--Start:SR:2014.05.06: YRS 5.0-2188: Beneficiaries RMDs --%>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblRMD_Rp" runat="server" CssClass="Label_Small" runat="server" Visible="false"> RMD Amount (by check): </asp:Label>
                                                            <asp:LinkButton ID="lnkRMD_RP" Text="RMD Amount (by check):" runat="server" OnClientClick="javascript: LoadBeneficiaryRMDs('Retirement Plan'); return false;" ToolTip="Please click here to see RMD amount break-up year wise" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtRMDTaxRate_RP" TabIndex="1" CssClass="TextBox_Normal" runat="server"
                                                                Width="53px" AutoPostBack="False" ReadOnly="False" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <%--Start - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 | Enable RMD control to allow values to be updated--%>
                                                            <asp:TextBox ID="txtRMDTaxable_RP" TabIndex="1" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" Enabled="true" style="text-align:right"  ></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtRMDNonTaxable_RP" TabIndex="1" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" Enabled="true" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtRMDTax_RP" TabIndex="1" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" AutoPostBack="False" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtRMDNet_RP" TabIndex="1" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" Enabled="true" style="text-align:right"></asp:TextBox>
                                                            <%--End - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 | Enable RMD control to allow values to be updated--%>
                                                        </td>
                                                    </tr>

                                                </table>
                                                <%--End:SR:2014.05.06: YRS 5.0-2188: Beneficiaries RMDs --%>
                                                <table class="Table_WithBorder" id="tblRolloverOptions_RP" width="100%" border="0" runat="server">
                                                    <tr>
                                                        <td>Rollover Options:
                                                        </td>
                                                    </tr>                                                   
                                                    <tr>
                                                        <td>
                                                            <table id="Table5">
                                                                <tr>
                                                                    <td align="left" width="180" rowspan="3">&nbsp;
                                                                    <asp:RadioButtonList ID="RolloverOptions_RP" TabIndex="2" runat="server" Width="160px"
                                                                        AutoPostBack="False" RepeatLayout="Table" Height="46px" BorderWidth="2px" BorderStyle="Solid">
                                                                        <asp:ListItem Value="none" Selected="True">Rollover none</asp:ListItem>
                                                                        <asp:ListItem Value="all">Rollover All</asp:ListItem>
                                                                        <asp:ListItem Value="taxable">Taxable Only </asp:ListItem> 
                                                                        <asp:ListItem Value="Partial">Specific Amount</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                    </td>
                                                                    <td align="left" colspan="6">
                                                                        <table id="Table8">

                                                                             <%--START : SB | 2016.11.29 | YRS-AT-3022 | Added RolloverToOwnIRA checkbox for Retirement Plan--%>
                                                                             <tr runat="server" id="trIsRolloverToOwnIRA_RP" >
                                                                                <td colspan="2" align="left" ><asp:CheckBox ID="chkRolloverToOwnIRA_RP" runat="server" text="Rollover to own IRA or eligible plan " TextAlign="left"/>
                                                                                <%-- <span class="ui-button-icon-primary ui-icon ui-icon-clock" style="float: center; cursor: pointer" title="View Sample Check"/>--%>  <%-- For future reference to display icon displaying the pattern of cheque --%>
                                                                                </td>
                                                                            </tr>
                                                                              <%--END : SB | 2016.11.29 | YRS-AT-3022 | Added RolloverToOwnIRA checkbox for Retirement Plan--%>

                                                                            <tr>
                                                                                <td align="left" >
                                                                                    <asp:Label ID="LabelPayee2" runat="server" CssClass="Label_Small">Rollover Payee: </asp:Label> 
                                                                                </td>
                                                                                <td width="300px">                                                                                                                                                                  
                                                                                    <UC2:RollUserControl runat="server" ID="RolloverPayeeName_RP" /> <%--Start:SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name --%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" width="85">Amount
                                                                    </td>
                                                                    <td align="left" width="85">Taxable
                                                                    </td>
                                                                    <td align="left" width="85">Non-Taxable
                                                                    </td>
                                                                    <td align="left" width="85">&nbsp;
                                                                    </td>
                                                                    <td align="left" width="85">Net
                                                                    </td>
                                                                    <td align="left" width="40"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:TextBox runat="server" ID="RolloverPartialAmount_RP" CssClass="TextBox_Normal"
                                                                            TabIndex="4" Width="79px" style="text-align:right" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="RolloverTaxable_RP" TabIndex="4" runat="server" CssClass="TextBox_Normal"
                                                                            Width="79px" AutoPostBack="False" EnableViewState="True" style="text-align:right"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="RolloverNonTaxable_RP" TabIndex="4" runat="server" CssClass="TextBox_Normal"
                                                                            Width="79px" AutoPostBack="False" EnableViewState="True" style="text-align:right"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left"></td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="RolloverNet_RP" TabIndex="4" runat="server" CssClass="TextBox_Normal"
                                                                            Width="79px" style="text-align:right"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4">
                                                                        <asp:Label ID="lblMaxRollover_RP" runat="server" CssClass="Label_Small"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan ="6">
                                                                          <asp:Label ID="lblRMDNotes_RP" runat="server" CssClass="Label_Small" Style="color: #f00">  Note: Rollover All option is not available as beneficiary must satisfy RMD requirement.</asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td>&nbsp;
                                            </td>

                                        </tr>--%>
                                    </table>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>&nbsp;
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <table class="Table_WithOutBorder" id="tblSP" cellspacing="0" cellpadding="3" width="100%"
                                        border="0" runat="server">
                                        <tr>
                                            <td class="td_Text">TD Savings Plan Options:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table id="Table1" border="0">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="Label1" runat="server" CssClass="Label_Small">Payee 1 : </asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxPayee1Name_SP" TabIndex="99" runat="server" CssClass="TextBox_Normal"
                                                                Enabled="False" Width="336px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="Table3">
                                                    <tr>
                                                        <td align="left" width="200">&nbsp;
                                                        </td>
                                                        <td align="left" width="65">Tax rate
                                                        </td>
                                                        <td align="left" width="85">Taxable
                                                        </td>
                                                        <td align="left" width="85">Non-Taxable
                                                        </td>
                                                        <td align="left" width="85">Tax
                                                        </td>
                                                        <td align="left" width="85">Net
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">Refund Amount (by check):
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxTaxRate_SP" TabIndex="51" runat="server" CssClass="TextBox_Normal"
                                                                Width="53px" AutoPostBack="False" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxTaxable_SP" TabIndex="51" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxNonTaxable_SP" TabIndex="51" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxTax_SP" TabIndex="51" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" AutoPostBack="False" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxNet_SP" TabIndex="51" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%--Start:SR:2014.05.06: YRS 5.0-2188: RMDs for Beneficiaries --%>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:LinkButton ID="lnkRMD_SP" Text="RMD Amount (by check):" runat="server" OnClientClick="javascript:LoadBeneficiaryRMDs('Saving Plan'); return false;" ToolTip="Please click here to see RMD amount break-up year wise" />
                                                            <asp:Label ID="lblRMD_SP" runat="server" CssClass="Label_Small" runat="server" Visible="false"> RMD Amount (by check): </asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtRMDTaxRate_SP" TabIndex="1" CssClass="TextBox_Normal" runat="server"
                                                                Width="53px" AutoPostBack="False" ReadOnly="False" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <%--Start - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 | Enable RMD control to allow values to be updated--%>
                                                            <asp:TextBox ID="txtRMDTaxable_SP" TabIndex="1" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" Enabled="true" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtRMDNonTaxable_SP" TabIndex="1" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" Enabled="true" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtRMDTax_SP" TabIndex="1" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" AutoPostBack="False" style="text-align:right"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtRMDNet_SP" TabIndex="1" runat="server" CssClass="TextBox_Normal"
                                                                Width="79px" Enabled="true" style="text-align:right"></asp:TextBox>
                                                            <%--End - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 | Enable RMD control to allow values to be updated--%>
                                                        </td>
                                                    </tr>
                                                    <%--End:SR:2014.05.06: YRS 5.0-2188:RMDs for Beneficiaries --%>
                                                </table>
                                                <table class="Table_WithBorder" id="tblRolloverOptions_SP" cellspacing="1" cellpadding="1"
                                                    width="100%" border="0" runat="server">
                                                    <tr>
                                                        <td>Rollover Options:
                                                        </td>
                                                    </tr>                                                   
                                                    <tr>
                                                        <td>
                                                            <table id="Table10">
                                                                <tr>
                                                                    <td align="left" width="180" rowspan="3">&nbsp;
                                                                    <asp:RadioButtonList ID="RolloverOptions_SP" TabIndex="52" runat="server" Width="160px"
                                                                        AutoPostBack="False" RepeatLayout="Table" Height="46px" BorderWidth="2px" BorderStyle="Solid">
                                                                        <asp:ListItem Value="none" Selected="True">Rollover none</asp:ListItem>
                                                                        <asp:ListItem Value="all">Rollover All</asp:ListItem>
                                                                         <asp:ListItem Value="taxable">Taxable Only </asp:ListItem>  
                                                                        <asp:ListItem Value="Partial">Specific Amount</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                    </td>
                                                                    <td align="left" colspan="6">
                                                                        <table id="Table11">

                                                                             <%--START : SB | 2016.11.29 | YRS-AT-3022 | Added RolloverToOwnIRA checkbox for Savings Plan--%>
                                                                             <tr runat="server" id="trIsRolloverToOwnIRA_SP">
                                                                                <td colspan="2" align="left" ><asp:CheckBox ID="chkRolloverToOwnIRA_SP" runat="server" text="Rollover to own IRA or eligible plan " TextAlign="left"/>
                                                                                <%-- <span class="ui-button-icon-primary ui-icon ui-icon-clock" style="float: center; cursor: pointer" title="View Sample Check"/>--%><%-- For future reference to display icon displaying the pattern of cheque --%>
                                                                                </td>
                                                                            </tr>
                                                                             <%--END : SB | 2016.11.29 | YRS-AT-3022 | Added RolloverToOwnIRA checkbox for Savings Plan--%>

                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="Label11" runat="server" CssClass="Label_Small">Rollover Payee: </asp:Label>
                                                                                </td>
                                                                                <td width="300px">                                                                                     
                                                                                     <UC2:RollUserControl runat="server" ID="RolloverPayeeName_SP"/> <%--Start:SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name --%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" width="85">Amount
                                                                    </td>
                                                                    <td align="left" width="85">Taxable
                                                                    </td>
                                                                    <td align="left" width="85">Non-Taxable
                                                                    </td>
                                                                    <td align="left" width="85">&nbsp;
                                                                    </td>
                                                                    <td align="left" width="85">Net
                                                                    </td>
                                                                    <td align="left" width="40"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:TextBox runat="server" ID="RolloverPartialAmount_SP" TabIndex="54" CssClass="TextBox_Normal"
                                                                            Width="79px" style="text-align:right"/>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="RolloverTaxable_SP" TabIndex="54" runat="server" CssClass="TextBox_Normal"
                                                                            Width="79px" AutoPostBack="False" EnableViewState="True" style="text-align:right"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="RolloverNonTaxable_SP" TabIndex="54" runat="server" CssClass="TextBox_Normal"
                                                                            Width="79px" AutoPostBack="False" EnableViewState="True" style="text-align:right"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left"></td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="RolloverNet_SP" TabIndex="54" runat="server" CssClass="TextBox_Normal"
                                                                            Width="79px" style="text-align:right"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4">
                                                                        <asp:Label ID="lblMaxRollover_SP" runat="server" CssClass="Label_Small"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan ="6">
                                                                        <asp:Label ID="lblRMDNotes_SP" runat="server" CssClass="Label_Small" Style="color: #f00">  Note: Rollover All option is not available as beneficiary must satisfy RMD requirement.</asp:Label>
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
                            <tr>
                                <td></td>
                            </tr>
                            <%--Start - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td align="left" height="18">
                                                <a href="#" ID="lnkDeductions" onclick="javascript:getSelectedDedValues(); return false;">Deductions:</a>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblDedunctionsmsg" runat="server" CssClass="Label_Small" Style="color: #f00">(Please click Deductions link to apply fees and deductions) </asp:Label>                      
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <%--End - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>                          
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td align="left" width="266" colspan="2" height="20"></td>
                                            <td align="left" width="85" height="20">Taxable
                                            </td>
                                            <td align="left" width="85" height="20">Non-Taxable
                                            </td>
                                            <td align="left" width="85" height="20">Tax
                                            </td>
                                            <%--Start - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>
                                            <td align="left" width="85" height="20">Deductions
                                            </td>
                                            <%--End - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>
                                            <td align="left" width="85" height="20">Net
                                            </td>
                                            <td align="left" width="40" height="20"></td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="200" colspan="2"></td>
                                            <td align="left">
                                                <asp:TextBox ID="TextboxTotalTaxable" runat="server" CssClass="TextBox_Normal" Width="79px" style="text-align:right"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TextboxTotalNonTaxable" runat="server" CssClass="TextBox_Normal" Width="79px" style="text-align:right"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TextboxTotalTax" runat="server" CssClass="TextBox_Normal" Width="79px" style="text-align:right"></asp:TextBox>
                                            </td>
                                            <%--start - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>
                                            <td align="left">
                                                <asp:TextBox ID="TextboxDeductions" runat="server" CssClass="TextBox_Normal" Width="79px" style="text-align:right"></asp:TextBox>
                                            </td>
                                            <%--End - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>
                                            <td align="left">
                                                <asp:TextBox ID="TextboxTotalNet" runat="server" CssClass="TextBox_Normal" Width="79px" style="text-align:right"></asp:TextBox>
                                            </td>
                                            <td align="left"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                            <td>
                                &nbsp;
                            </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" class="Td_ButtonContainer">
                                        <tr>
                                            <td align="right" width="324"></td>
                                            <td align="right">
                                                <asp:Button ID="ButtonSave" runat="server" CssClass="Button_Normal" Width="94px"
                                                    Text="Save"></asp:Button>
                                            </td>
                                            <td align="right">
                                                <asp:Button ID="ButtonOk" runat="server" CssClass="Button_Normal" Text="Ok" Width="92px"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="divBeneficiaryRMDs" runat="server" style="display: block;">
            <table id="gvBeneficiaryRMDs" border="0" cellspacing="1" cellpadding="1" style="border-width: thin">
                <tbody></tbody>
            </table>
        </div>
        <%--start - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>
        <div id="divDeductions" runat="server" style="OVERFLOW: auto;display:block;">
            <table width="100%">
                <tr>
                    <td>
            <div id="divMessage" style="width: 75%;">               
             </div>
                </td>
                </tr>
                <tr>
                    <td>
            <asp:datagrid id="dgDeductions" runat="server" Width="200px" CssClass="DataGrid_Grid" Autogeneratecolumns="false">
						<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
						<Columns>						
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:CheckBox id="chkBoxDeduction" runat="server"></asp:CheckBox>
								</ItemTemplate>                               
							</asp:TemplateColumn>                           
							<asp:BoundColumn DataField="CodeValue" HeaderText="Deductions" visible="False">
								<HeaderStyle Width="10px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ShortDescription" HeaderText="Deductions">
								<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>								
                            <asp:TemplateColumn HeaderText="Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount")%>'></asp:Label>
                                    <asp:TextBox ID ="txtFundCostAmt" runat="server" Visible="false" Width="70px" Enabled ="false" onkeypress="javascript: ValidateNumeric();" onchange="javascript: $(this).val(CurrencyFormatted($(this).val()));"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>   
						</Columns>
					</asp:datagrid>
                </td>
                </tr>
            </table>                       
        </div>
        <%--End - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>
       <input type="hidden" name="hdnRelationshipWithParticipant" id="hdnRelationshipWithParticipant" value="" runat="server">  <%-- SB | 2016.11.29 | YRS-AT-3022 | Hidden field variable to hold Relationship value between participant and beneficiary --%>
        <%--START: MMR | 2017.12.11 | YRS-AT-3742 | Declaring hidden field variable to hold plan for RMD greater than non-taxable in case of rollover option selected --%>
        <asp:HiddenField ID="hdnIsNonTaxableGreaterThanRMDRET" runat="server" Value="0" />
        <asp:HiddenField ID="hdnIsNonTaxableGreaterThanRMDSAV" runat="server" Value="0" />
        <%--END: MMR | 2017.12.11 | YRS-AT-3742 | Declaring hidden field variable to hold plan for RMD greater than non-taxable in case of rollover option selected --%>
		 <input type="hidden" name="hdnMinRMDTaxRate" id="hdnMinRMDTaxRate" value="" runat="server">  <%-- SB | 2017.12.18 | YRS-AT-3756 | Hidden field variable to hold Minimum RMD tax rate --%>
       	 <input type="hidden" name="hdnRMDTaxRateErrorMessage" id="hdnRMDTaxRateErrorMessage" value="" runat="server">  <%-- SB | 2017.12.18 | YRS-AT-3756 | Hidden field variable to hold error message when invalid RMD tax rate is entered  --%>
         </TD></TD></TR></TBODY></TABLE><asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        <!-- General PageView End -->
    </form>
    </TD></TR></TBODY></TABLE>
</body>
