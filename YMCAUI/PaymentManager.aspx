<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PaymentManager.aspx.vb" Inherits="YMCAUI.PaymentManager"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<!--#include virtual="top.html"-->
<head>
    <style type="text/css">
        .Button_Normal
        {}
        .Table_WithoutBorder {
            margin-left: 0px;
        }
        .auto-style1 {
            width: 494px;
        }
        .auto-style2 {
            width: 569px;
        }

        .ClassHide {
            display: none;
        }
 /*START : ML| 2019.10.10 | YRS-AT-4642 | css changes to Show text in one line*/ 
        .showtextInOneline {
            white-space:pre ;
        }
 /*END : ML| 2019.10.10 | YRS-AT-4642 | css changes to Show text in one line*/ 
    </style>
</head>
<script>
    function ValidateNumeric() {
        if ((event.keyCode < 48) || (event.keyCode > 57)) {
            event.returnValue = false;
        }
    }
</script>

<%-- Start: SB | 04.24.2018 | YRS-AT-3101 | function to display notification message on the screen after receving EFT approval file --%>
<script type="text/javascript" >
    function BindEvents() {
        $("#divEFTFailedPayments").dialog({
            autoOpen: false,
            resizable: false,
            dialogClass: 'no-close',
            draggable: true,
            closeOnEscape: false,
            width: 600, minheight: 200,
            height: 260,
            modal: true,
            open: function (type, data) {
                $(this).parent().appendTo("form");
            },
            show: {
                effect: "blind",
                duration: 50
            }
        });

        $("#divEFTFailedToSendRejectionEmail").dialog({
            autoOpen: false,
            resizable: false,
            dialogClass: 'no-close',
            draggable: true,
            closeOnEscape: false,
            width: 600, minheight: 200,
            height: 260,
            modal: true,
            open: function (type, data) {
                $(this).parent().appendTo("form");
            },
            show: {
                effect: "blind",
                duration: 50
            }
        });

        $('#divEFTConfirmationDialog').dialog({
            autoOpen: false,
            draggable: true,
            close: false,
            modal: true,
            width: 500, <%--VC | 2018.09.17 | YRS-AT-3101 | Changed width--%>
            maxHeight: 420,
            height: 150,

            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });

         <%--START : VC | 2018.09.17 | YRS-AT-3101 | Code to show reject confirmation message--%>
        $('#divRejectConfirmationDialog').dialog({
            autoOpen: false,
            draggable: true,
            close: false,
            modal: true,
            width: 500,
            maxHeight: 420,
            height: 150,

            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });
        <%--END : VC | 2018.09.17 | YRS-AT-3101 | Code to show reject confirmation message--%>

         <%--START: PK|12-12-2019 |YRS-AT-4601|Code to display link in success message--%>
        $('#divErrorMsgToDisplay').dialog({
            autoOpen: false,
            draggable: true,
            close: false,
            modal: true,
            width: 600,
            maxHeight: 400,
            height: 300,

            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });
         <%--END: PK|12-12-2019 |YRS-AT-4601|Code to display link in success message--%>
    }

    <%--START: PK|12-12-2019 |YRS-AT-4601|Code to display link in success message--%>
    function OpenErrorMsgToDisplay() {
        $("#divErrorMsgToDisplay").dialog("open");
        return false;
    }
    <%--END: PK|12-12-2019 |YRS-AT-4601|Code to display link in success message--%>

    function OpenFailedPaymentDialog() {
        $("#divEFTFailedPayments").dialog("open");
        return false;
    }

    function OpenFailedToSendRejectionEmail() {
        $("#divEFTFailedToSendRejectionEmail").dialog("open");
        return false;
    }

    function OpenEFTConfirmationDialog() { 
        $("#divConfirmDialogMessage").html($('#hfApproveConfirmation').val()); <%--VC | 2018.09.17 | YRS-AT-3101 | Binding confirmation message--%>
        $('#divEFTConfirmationDialog').dialog("open");
        return false;
    }
     <%--START : VC | 2018.09.17 | YRS-AT-3101 | Code to show confirmation messages--%>
    function OpenMismatchConfirmationDialog(txtMessage) {
        $('#divMismatchRecalculationDialog').dialog({
            autoOpen: false,
            draggable: true,
            close: false,
            modal: true,
            width: 450,
            maxHeight: 420,
            height: 120,

            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });
        $("#divMismatchRecalculationMessage").html(txtMessage);
        $('#divMismatchRecalculationDialog').dialog("open");
        return false;
    }

    
      <%--START : VC | 2018.09.17 | YRS-AT-3101 | Code to show confirmation messages--%>

    function CloseDialog(id) {
        $('#' + id).dialog('close');
        return false;
    }
    <%--START : VC | 2018.09.17 | YRS-AT-3101 | Code to perform select and unselect all operation--%>
    var allCheckBoxSelector = '#<%=DataGridPayment.ClientID%> input[id*="chkSelectAll"]:checkbox';
    var checkBoxSelector = '#<%=DataGridPayment.ClientID%> input[id*="CheckBoxSelect"]:checkbox';
    

    var allCheckBoxSelectorConfirmEFT = '#<%=dgEFTDisbursements.ClientID%> input[id*="chkSelectAllEFTDisbursement"]:checkbox';
    var checkBoxSelectorConfirmEFT = '#<%=dgEFTDisbursements.ClientID%> input[id*="chkEFTDisbursement"]:checkbox';

    function CheckUncheckAll() {
        var totalCheckboxes = $(checkBoxSelector),
                    checkedCheckboxes = totalCheckboxes.filter(":checked"),
                    noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
                    allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);
        if (totalCheckboxes.length == 0) allCheckboxesAreChecked = false;
        $(allCheckBoxSelector).attr('checked', allCheckboxesAreChecked);
        if ($("#DataGridPayment").is(':visible')) {
            if (checkedCheckboxes.length == 0) {
                SetPaidButton(false)
            }
            else {
                SetPaidButton(true)
            }
        }
       
    }

    function CheckUncheckAllEFTDisbursement() {
        var totalCheckboxes = $(checkBoxSelectorConfirmEFT),
                    checkedCheckboxes = totalCheckboxes.filter(":checked"),
                    noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
                    allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);
        if (totalCheckboxes.length == 0) allCheckboxesAreChecked = false;
        $(allCheckBoxSelectorConfirmEFT).attr('checked', allCheckboxesAreChecked);
        if ($("#dgEFTDisbursements").is(':visible')) {
            SetPaidButton(allCheckboxesAreChecked)
        }
            
        
    }
    function SelectAllEFTDisbursementConfirmed() {
        $(checkBoxSelectorConfirmEFT).attr('checked', true);
        CheckUncheckAllEFTDisbursement();
        closeConfirmEFtSelectAll();
        SelectAllConfirmEFT();
    }

    function SetPaidButton(IsPaidButtonEnable) {
        if (IsPaidButtonEnable) {
            $("#ButtonPay").attr('disabled', false);
        }
        else{
            $("#ButtonPay").attr('disabled', true);
        }
    }
     <%--END : VC | 2018.09.17 | YRS-AT-3101 | Code to perform select and unselect all operation--%>

    <%--START : VC | 2018.09.17 | YRS-AT-3101 | Setting button click operation--%>
    $(document).ready(function () {
        $(checkBoxSelector).bind('click', function(){
            CheckUncheckAll();
            SelectAllEFTCheck();
        });

        CheckUncheckAll();

        $(allCheckBoxSelector).bind('click', function () {
            $(checkBoxSelector).attr('checked', $(this).is(':checked'));
            CheckUncheckAll();
            SelectAllEFTCheck();
        });

        RecalculateTotalAmount();

        $(checkBoxSelectorConfirmEFT).bind('click', function () {
            CheckUncheckAllEFTDisbursement();
            var totalCheckboxes = $(checkBoxSelectorConfirmEFT);
            var checkedCheckboxes = totalCheckboxes.filter(":checked");
            if ($("#dgEFTDisbursements").is(':visible')) {
                if (checkedCheckboxes.length == 0) {
                    SetPaidButton(false)
                }
                else {
                    SetPaidButton(true)
                }
            }
        });

        $(allCheckBoxSelectorConfirmEFT).bind('click', function () {
            if ($(allCheckBoxSelectorConfirmEFT).is(':checked')) {
                $(allCheckBoxSelectorConfirmEFT).attr('checked', false);
                showConfirmEFTSelectAllDialog();
            }
            else {
                $(allCheckBoxSelectorConfirmEFT).attr('checked', false);
                $(checkBoxSelectorConfirmEFT).attr('checked', false);
                CheckUncheckAllEFTDisbursement();
                SelectAllConfirmEFT();
            }
        });

        $('#dvWebProcessing').dialog({
            autoOpen: false,
            resizable: false,
            draggable: true,
            closeOnEscape: false,
            close: false,
            modal: true,
            width: 350, height: 150,
            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });
        var totalCheckboxes = $(checkBoxSelectorConfirmEFT);
        var checkedCheckboxes = totalCheckboxes.filter(":disabled");
        if (checkedCheckboxes.length > 0) {
            $(allCheckBoxSelectorConfirmEFT).attr('disabled', true);
        }
        else {
            $(allCheckBoxSelectorConfirmEFT).attr('disabled', false);
        }
        CheckUncheckAll();
        CheckUncheckAllEFTDisbursement();
    });
    <%--END : VC | 2018.09.17 | YRS-AT-3101 | Setting button click operation--%>

    <%--START : VC | 2018.09.17 | YRS-AT-3101 | Code to perform recalculations--%>
    function RecalculateTotalAmount() {
        var TotalAmount = 0;
        $("#DataGridPayment tr").each(function () {
            var $checkBox = $(this).find("input[type='checkbox']");
            if ($checkBox.length > 0) {
                var checkBoxId = $checkBox[0].id;
                if ($checkBox.is(':checked') && checkBoxId.indexOf('chkSelectAll') == -1) {
                    $("#ButtonPay").attr('disabled', false);
                    var tds = $(this).find('td');
                    if (tds.length >= 5) {
                        <%--START : SB | 2019.09.13 | YRS-AT-4608 | conversion Code was giving wrong values when the amount is higher than 1 million, code replaced --%>
                        //var Total = tds[5].innerText.replace(',', '');
                        var Total = tds[5].innerText.replace(/,/g, '');
                        <%--END : SB | 2019.09.13 | YRS-AT-4608 | conversion Code was giving wrong values when the amount is higher than 1 million, code replaced --%>
                        TotalAmount += parseFloat(Total);
                    }
                }
            }
        });
        var formatedNumber = number_format(TotalAmount, 2, '.', ',');
        $("#TextBoxNetAmount").val('$' + formatedNumber);
    }

    number_format = function (number, decimals, dec_point, thousands_sep) {
        number = number.toFixed(decimals);

        var nstr = number.toString();
        nstr += '';
        x = nstr.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? dec_point + x[1] : '';
        var rgx = /(\d+)(\d{3})/;

        while (rgx.test(x1))
            x1 = x1.replace(rgx, '$1' + thousands_sep + '$2');

        return x1 + x2;
    }

    function RecalculateTotalAmountEFTDisbursement() {
        var TotalAmount = 0;
        $("#dgEFTDisbursements tr").each(function () {
            var $checkBox = $(this).find("input[type='checkbox']");
            if ($checkBox.length > 0) {
                var checkBoxId = $checkBox[0].id;
                if ($checkBox.is(':checked') && checkBoxId.indexOf('chkSelectAllEFTDisbursement') == -1) {
                    $("#ButtonPay").attr('disabled', false);
                    var tds = $(this).find('td');
                    if (tds.length >= 5) {
                        var Total = tds[5].innerText.replace(',', '');
                        TotalAmount += parseFloat(Total);
                    }
                }
            }
        });
        var formatedNumber = number_format(TotalAmount, 2, '.', ',');
        $("#txtConfirmEFTRecalculate").val('$' + formatedNumber);
    }
    <%--END : VC | 2018.09.17 | YRS-AT-3101 | Code to perform recalculations--%>
    <%--START : VC | 2018.09.17 | YRS-AT-3101 | Code to show and hide dialog boxes--%>
    function ShowWebProcessingDialog(Message, divTitle) {
        $('#dvWebProcessing').dialog({ title: divTitle });
        $('#dvWebProcessing').dialog("open");
        $('#lblProcessing').text(Message);

    }
    function CloseWebProcessingDialog() {
        $('#dvWebProcessing').dialog('close');
    }


    function showConfirmEFTSelectAllDialog() {
        $('#divConfirmEFTSelectAllDialog').dialog({
            autoOpen: false,
            resizable: false,
            draggable: true,
            closeOnEscape: false,
            title: "Select all loans.",
            close: false,
            width: 400, height: 180,
            modal: true,
            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });
        var text = $('#hfSelectAllConfirmation').val();
        $('#lblMessageConfirm').text(text);
        $('#divConfirmEFTSelectAllDialog').dialog({ title: 'Select all loans.' });
        $('#divConfirmEFTSelectAllDialog').dialog("open");
    }

    function closeConfirmEFtSelectAll() {
        $('#divConfirmEFTSelectAllDialog').dialog('close');
    }
    <%--END : VC | 2018.09.17 | YRS-AT-3101 | Code to show and hide dialog boxes--%>

    <%--START : VC | 2018.09.17 | YRS-AT-3101 | Code to Perform select all and recalculations--%>
    function SelectAllEFTCheck() {
        RecalculateTotalAmount();
        //ShowWebProcessingDialog('Please wait process is in progress...', '');
        var TotalAmount = $("#TextBoxNetAmount").val();
        var CheckedStatus = '';
        $(checkBoxSelector).map(function () {
            if ($(this).is(':checked') == true) {
                CheckedStatus += '1' + ',';
            }
            else {
                CheckedStatus += '0' + ',';
            }
        });
        CheckedStatus = CheckedStatus.replace(/,\s*$/, "");
        $.ajax({
            type: "POST",
            url: "PaymentManager.aspx/SelectAllCheckBox",
            data: "{'totalAmount':'" + TotalAmount + "','checkedStatus':'"+CheckedStatus+"'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (returndata) {
                $("#dgEFTDisbursements tr:not(:first-child)").html("");
                //CloseWebProcessingDialog();
            },
            failure: function () {
                //CloseWebProcessingDialog();
                return false;
            }
        });
    }

    function SelectAllConfirmEFT() {
        RecalculateTotalAmountEFTDisbursement();
        var TotalAmount = $("#txtConfirmEFTRecalculate").val();
        var CheckedStatus = '';
        // START: SR | 2018.10.05 | YRS-AT-3101 | Get FundNo for whom checkbox is checked 
        //$(checkBoxSelectorConfirmEFT).map(function () {
        //    if ($(this).is(':checked') == true) {
        //        //CheckedStatus+='1'+',';
        //        var tds = $(this).find('td');
        //        if (tds.length >= 5) {
        //            var Total = tds[5].innerText.replace(',', '');
        //            TotalAmount += parseFloat(Total);
        //        }

        //    }
        //    else {
        //        CheckedStatus += '0' + ',';
        //    }
        //});
        $("#dgEFTDisbursements tr").each(function () {
            var $checkBox = $(this).find("input[type='checkbox']");
            if ($checkBox.length > 0) {
                var checkBoxId = $checkBox[0].id;
               
                if ($checkBox.is(':checked') && checkBoxId.indexOf('chkSelectAllEFTDisbursement') == -1) {
                    var tds = $(this).find('td');
                    if (tds.length >= 5) {
                        CheckedStatus = CheckedStatus + ',' + tds[1].innerText;
                    }
                }
            }
        });
        // END: SR | 2018.10.05 | YRS-AT-3101 | Get FundNo for whom checkbox is checked 
        CheckedStatus = CheckedStatus.replace(/,\s*$/, "");
        $.ajax({
            type: "POST",
            url: "PaymentManager.aspx/SelectAllConfirmEFTCheckBox",
            data: "{'totalAmount':'" + TotalAmount + "','checkedStatus':'" + CheckedStatus + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (returndata) {
                //CloseWebProcessingDialog();
            },
            failure: function () {
                //CloseWebProcessingDialog();
                return false;
            }
        });
    }
    <%--START : VC | 2018.09.17 | YRS-AT-3101 | Code to Perform select all and recalculations--%>

    
  </script>
<%-- End: SB | 04.24.2018 | YRS-AT-3101 | function to display notification message on the screen after receving EFT approval file --%>
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
					CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
					MenuFadeDelay="2" mouseovercssclass="MouseOver">
					<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
				</cc1:menu></td>
		</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="left">Payment Manager</td>
		</tr>
		<tr>
			<td>&nbsp;
				<%-- Start: ML | 2019.12.17 | YRS-AT-4601 | Change message div property for show hide--%>
                <%-- Start: SB | 04.24.2018 | YRS-AT-3101 | Div added to display notification status for EFT --%>
                <%--<div id="divSuccessMessage" class="success-msg" runat="server" style="text-align: left; display:none;" height="10"></div> 
                <div id="divErrorMessage" runat="server" style="text-align: left; display:none;" height="10" enableviewstate ="false" ></div> 
                <div id="divDBErrorMessage" class="error-msg" runat="server" style="text-align: left; display:none;" height="10" enableviewstate ="false" ></div> 
                <div id="divStatusMessage" class="error-msg" runat="server" style="text-align: left; display:none;" height="10" enableviewstate ="false" ></div> --%>

                <div id="divSuccessMessage" class="success-msg" runat="server" style="text-align: left;" visible="false"  height="10" enableviewstate ="false"></div> 
                <div id="divErrorMessage" runat="server" style="text-align: left; " visible="false" height="10" enableviewstate ="false" ></div> 
                <div id="divDBErrorMessage" class="error-msg" runat="server" style="text-align: left;" visible="false" height="10" enableviewstate ="false" ></div> 
                <div id="divStatusMessage" class="error-msg" runat="server" style="text-align: left; "  visible="false" height="10" enableviewstate ="false" ></div> 

                <%-- End: SB | 04.24.2018 | YRS-AT-3101 | Div added to display notification status for EFT --%>
				<%-- End: ML | 2019.12.17 | YRS-AT-4601 | Change message div property for show hide--%>
			</td>
		</tr>
	</table>
    
    <table class="Table_WithBorder" cellSpacing="0" cellPadding="0" width="700">
         <%-- START | SR | 2018.04.10 | Add controls for EFT payment type--%>
		<tr>
            <td colspan ="2">
               
                <TABLE class="Table_WithOutBorder" id="Table2"  cellSpacing="1" cellPadding="1" border=0 align="left" width="100%">
                     <tr align="left"> 
                        <td aligh="left">
                            
                            <%--START : ML | 2019.11.04 | YRS-AT-4601| Change Radiobutton list to individual radiobutton to set new UI--%>
                            <asp:Radiobutton id="rbDisbursementTypeEFT" runat="server" AutoPostBack="True" CssClass="RadioButton_Normal" text="Electronic Bank Payments (EFT)"  checked="True"	GroupName="grpDisbursement" Value="EFT" ></asp:Radiobutton><br />
                            <asp:Radiobutton id="rbDisbursementTypeCHECK" runat="server" AutoPostBack="True" CssClass="RadioButton_Normal" text="Checks" GroupName="grpDisbursement" Value="CHECK"></asp:Radiobutton>
                            &nbsp;<span id="spCurrency" runat ="server" >(
                                <asp:Radiobutton id="rbCurrencyUSA" runat="server" AutoPostBack="True" CssClass="RadioButton_Normal" text="U.S." GroupName="grpCurrency"></asp:Radiobutton>
                                <asp:Radiobutton id="rbCurrencyCanada" runat="server" AutoPostBack="True" CssClass="RadioButton_Normal" text="Canadian"  GroupName="grpCurrency" ></asp:Radiobutton>
                                ) </span><br />                         
                             <asp:Radiobutton id="rbDisbursementTypeAPPROVEDEFT" runat="server" AutoPostBack="True" CssClass="RdioButton_Normal" text="Confirm Electronic Bank Payments (EFT)"	GroupName="grpDisbursement" Value="APPROVED_EFT"></asp:Radiobutton><br />

                            <%--<asp:RadioButtonList ID="rblDisbursementType" runat="server" AutoPostBack="True" CssClass="Label_Small" width ="300px" align="left" > 
                                <asp:ListItem Value="EFT" align="left">Electronic Bank Payments (EFT) </asp:ListItem>
                                <asp:ListItem Value="CHECK" Selected="True" align="left">Checks  
                                    </asp:ListItem>
                                <asp:ListItem Value="APPROVED_EFT" Selected="True" align="left">Confirm Electronic Bank Payments (EFT)</asp:ListItem>
                            </asp:RadioButtonList>--%>
                            <%--END : ML | 2019.11.04 | YRS-AT-4601| Change Radiobutton list to individual radiobutton to set new UI--%>
                        </td>
                    </tr>
                </TABLE>
            </td>
            <td colspan ="3">
                 <TABLE class="Table_WithOutBorder" id="Table5"  cellSpacing="1" cellPadding="1" border=0 width="100%">
                    <tr> 
                        <td align="right" width="60%" colspan ="2"> 
                            <asp:label id="lblEFTType" cssclass="Label_Small" text="EFT Type" Runat="server" width="85px"></asp:label>
                        </td>
                        <td align="right" width="40%" colspan ="1"> 
                            <asp:dropdownlist id="ddlEFTType" runat="server" AutoPostBack="True" Width="170px"></asp:dropdownlist>  <%--  ML| 2019.10.10 | YRS-AT-4642 | Change Width 150 to 170--%>
 						 </td>
                    </tr>
                    <tr> 
                        <td align="right" width="60%" colspan ="2">  
                            <asp:label id="lblCheckType" cssclass="Label_Small" text="Check Type" Runat="server" width="85px"></asp:label>
                        </td>
                        <td align="right" width="40%" colspan ="1"> 
                            <asp:dropdownlist id="ddlCheckType" runat="server" AutoPostBack="True" Width="170px"></asp:dropdownlist>  <%--  ML| 2019.10.10 | YRS-AT-4642 | Change Width 150 to 170--%>
						</td>
                    </tr>
                </TABLE> 
            </td>
         </tr> 
       <%-- END | SR | 2018.04.10 | Add controls for EFT payment type--%> 
	<table class="Table_WithBorder" cellSpacing="0" cellPadding="0" width="700">
		<%--START : VC | 2018.11.29 | YRS-AT-3101 | Code to show Replaced Disbursements checkbox.--%>
        <tr>
			<td align="left" colspan=2 height="15" nowrap=true>
                <asp:checkbox id="CheckBoxReplacedDisbursements" runat="server" CssClass="CheckBox_Normal" AutoPostBack="True" Text=" Replaced Disbursements"></asp:checkbox>
			</td>	
        </tr>
		<%--END : VC | 2018.11.29 | YRS-AT-3101 | Code to show Replaced Disbursements checkbox.--%>
		<tr>
			<td align="right" height="22">
				<TABLE class="Table_WithOutBorder" id="tblWithdrawalOptions" height="56" cellSpacing="1" cellPadding="1" border=0 width="700" runat="server">
					<%--START : VC | 2018.11.29 | YRS-AT-3101 | Commented code to remove Replaced Disbursements checkbox from the table--%>
					<%--<TR>
						<TD align="left" colspan=2 height="15" nowrap=true><asp:checkbox id="CheckBoxReplacedDisbursements" runat="server" CssClass="CheckBox_Normal" AutoPostBack="True"
								Text=" Replaced Disbursements"></asp:checkbox></TD>
						<TD align="right" colspan=2 nowrap=true height="15"><%--<asp:label id="LabelDisbursements" cssclass="Label_Small" text="Disbursements Type" Runat="server">--%><%--</asp:label></TD>
						<TD align="right" nowrap=true height="15" rowSpan="1" width=120>
					</TR>--%>
					<%--END : VC | 2018.11.29 | YRS-AT-3101 | Commented code to remove Replaced Disbursements checkbox from the table--%>
					<tr>
						<td align="left" nowrap=true><asp:label id="LabelWithdrawals" cssclass="Label_Small" text="" Runat="server" visible="False"></asp:label><asp:checkbox id="CheckBoxAllOtherWithdrawals" runat="server" Text="All Other Withdrawals" visible="False"></asp:checkbox></td>
						<td align="left" nowrap=true><asp:checkbox id="CheckBoxHardshipWithdrawals" runat="server" Text="Hardship Withdrawals" visible="False"></asp:checkbox></td>
						
						<td align="left" nowrap=true><asp:checkbox id="CheckBoxDeathWithdrawals" runat="server" Text="Death Withdrawals" visible="False"></asp:checkbox></td>
						<td align="right" colspan=2 nowrap=true><asp:button id="ButtonLoadRefund" CssClass="Button_Normal" Text="Load" Runat="server" visible="False"
								width="150"></asp:button></td>
					</tr>
					<tr>
					<td align="left" nowrap=true><asp:checkbox id="CheckBoxSHIRA" runat="server" Text="Safe Harbor" visible="False"></asp:checkbox></td>
					</tr>
				</TABLE>
			</td>
		</tr>
		<TR>
			<td align="right">
				<TABLE class="Table_WithOutBorder" id="Table1" height="185" cellSpacing="1" cellPadding="1"
					width="700">
					<tr>
						<td>
							<div style="OVERFLOW: auto; WIDTH: 690px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 170px; BORDER-BOTTOM-STYLE: none">
                                <asp:datagrid id="DataGridPayment" runat="server" CssClass="DataGrid_Grid" Width="671px" allowsorting="True"
									AutoGenerateColumns="false">
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<Columns>
										<asp:TemplateColumn>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll"  runat="server"/>
                                            </HeaderTemplate>
										<%--<ItemStyle Width="10px"></ItemStyle>--%>	<%-- ML| 2019.10.10 | YRS-AT-4642 | Change Width --%>									
											<ItemStyle Width="8px"></ItemStyle>
											<ItemTemplate>
												<asp:CheckBox id="CheckBoxSelect" Enabled="True" Checked='<%# Databinder.Eval(Container.DataItem, "Selected") %>' runat="server">
												</asp:CheckBox>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="FundIdNo" SortExpression="FundIdNo" HeaderText="Fund No.">
											<ItemStyle Width="50px"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="SSNO" SortExpression="SSNO" HeaderText="SS No">
											<ItemStyle Width="50px"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="LastName" SortExpression="LastName" HeaderText="Last Name">
										<%--<ItemStyle Width="115px"></ItemStyle>--%>	<%-- ML| 2019.10.10 | YRS-AT-4642 | Change Width --%>									
										<ItemStyle Width="80px"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="FirstName" SortExpression="FirstName" HeaderText="First Name">
                                        <%--<ItemStyle Width="90px"></ItemStyle>--%> <%-- ML| 2019.10.10 | YRS-AT-4642 | Change Width --%>	
                                        <ItemStyle Width="80px"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Type">
											<ItemStyle Width="90px" CssClass="showtextInOneline"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="total" SortExpression="total" HeaderText="Total" DataFormatString="{0:N}">
											<ItemStyle HorizontalAlign="Right" Width="65px"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="DisbursementID" SortExpression="DisbursementID" HeaderText="DisbursementID">
											<ItemStyle HorizontalAlign="Right" Width="65px"></ItemStyle>
										</asp:BoundColumn>
                                        <%-- ML| 2019.10.10 | YRS-AT-4642 | Added new Column for CurrencyCode --%>	
                                        <asp:BoundColumn DataField="CurrencyCode" SortExpression="CurrencyCode" HeaderText="Currency" >
											<ItemStyle  Width="45px"></ItemStyle>
										</asp:BoundColumn>
                                        <%-- ML| 2019.10.10 | YRS-AT-4642 | Added new Column for CurrencyCode --%>	                                      
									</Columns>
								</asp:datagrid>
								<%-- START | SR | 2018.04.10 | Added datagrid for EFT disbursements. It will populate disbursement data for approval and rejection --%>

                                <asp:datagrid id="dgEFTDisbursements" runat="server" CssClass="DataGrid_Grid" Width="671px" allowsorting="True"
									AutoGenerateColumns="false">
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<Columns>
										<asp:TemplateColumn>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAllEFTDisbursement"  runat="server"/>
                                            </HeaderTemplate>
											<ItemStyle Width="3%"></ItemStyle>
											<ItemTemplate>
												<asp:CheckBox id="chkEFTDisbursement" Enabled='<%# DataBinder.Eval(Container.DataItem, "EnableCheckbox")%>' Checked='<%# Databinder.Eval(Container.DataItem, "Selected") %>' runat="server">
												</asp:CheckBox>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="FundIdNo" SortExpression="FundIdNo" HeaderText="Fund No.">
											<ItemStyle Width="10%"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="SSNO" SortExpression="SSNO" HeaderText="SS No">
											<ItemStyle Width="0%"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Name">
											<ItemStyle Width="14%"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Disb. Type">
											<ItemStyle Width="10%"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="DisbursementDate" SortExpression="DisbursementDate" HeaderText="Disb. Date">
											<ItemStyle Width="11%"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="NetAmount" SortExpression="NetAmount" HeaderText="Total" DataFormatString="{0:N}">
											<ItemStyle HorizontalAlign="Right" Width="12%"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="BankName" SortExpression="BankName" HeaderText="Bank Name">
											<ItemStyle Width="26%"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="BankAbaNumber" SortExpression="BankAbaNumber" HeaderText="Bank ABA#">
											<ItemStyle HorizontalAlign="Right" Width="12%"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="DisbursementEFTID" SortExpression="DisbursementEFTID" HeaderText="DisbursementEFTID">
											<ItemStyle Width="0%"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="DisbursementID" SortExpression="DisbursementID" HeaderText="DisbursementID">
											<ItemStyle Width="0%"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="PersBankingEFTID" SortExpression="PersBankingEFTID" HeaderText="PersBankingEFTID">
											<ItemStyle Width="0%"></ItemStyle>
										</asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="BankId" SortExpression="BankId" HeaderText="BankId">
											<ItemStyle Width="0%"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn Visible="False" DataField="ParticipantEmailId" SortExpression="ParticipantEmailId" HeaderText="ParticipantEmailId">
											<ItemStyle Width="0%"></ItemStyle>
										</asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="DisbursementType" SortExpression="DisbursementType" HeaderText="DisbursementType">
											<ItemStyle Width="0%"></ItemStyle>
										</asp:BoundColumn>
                                        <asp:BoundColumn DataField="BatchId" SortExpression="BatchId" HeaderText="Batch ID">
											<ItemStyle HorizontalAlign="Right" Width="12%"></ItemStyle>
										</asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="PersID" SortExpression="PersID" HeaderText="PersID">
											<ItemStyle Width="0%"></ItemStyle>
										</asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="LoanNumber" SortExpression="LoanNumber" HeaderText="LoanNumber">
											<ItemStyle Width="0%"></ItemStyle>
										</asp:BoundColumn>
									</Columns>
								</asp:datagrid>
								<%-- END | SR | 2018.04.10 | Added datagrid for EFT disbursements. It will populate disbursement data for approval and rejection --%>
							</div>
						</td>
					</tr>
				</TABLE>
			</td>
		</TR>

        <%-- Start| SB | 04.09.2018 | YRS-AT-3101 Added file upload control to import the acknowledgement file from the bank for approval of EFT --%>

        <tr>
            <td >
                 <div  id ="divConfirmEFTRecalculate" runat ="server" >
                    <table width="700" border ="0" class="Table_WithBorder"> 
                    <tr vAlign="top">                       
                            <td> 
                                <%--<table class="Table_WithOutBorder" width="280">
								    <tr>
									    <td  align="left">Look For&nbsp;<asp:textbox id="TextBox1" runat="server" cssclass="TextBox_Normal" Width="100px" MaxLength="9"></asp:textbox>
										    &nbsp;
										    <asp:button id="Button2" runat="server" CssClass="Button_Normal" 
                                                Text="Fund Id" Width="70px"
											    enabled="False"></asp:button></td>
								    </tr>
                                </table> --%>
                          
                             </td>
                            <td>  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>
                            <td style="text-align:right;">                                
                                <table class="Table_WithOutBorder" width="250">
								    <tr>
									    <%--<td class="Button_Normal" align="right">
                                            <asp:button id="btnConfirmEFTRecalculate" CssClass="Button_Normal" Text="Recalculate" Runat="server" Width="92px"
											    onclientclick="RecalculateTotalAmountEFTDisbursement();"></asp:button>
									    </td>--%>
                                        <td class="Button_Normal" align="right">
                                            <button id="btnConfirmEFTRecalculate" class="Button_Normal" runat="server" disabled="disabled"  onclick="SelectAllConfirmEFT();">Recalculate</button>
                                        </td>
									    <td align="right" ><asp:textbox id="txtConfirmEFTRecalculate" runat="server" CssClass="TextBox_Normal_Amount" Width="99px"
											    align="left" Font-Bold="True" enabled="False"></asp:textbox></td>
								    </tr>
                                 </table>                               
                            </td>
                        </tr> 
                        </table>
                     </div> 
            </td>
        </tr>

        <tr>
            <td>
                <div  id ="divApprovedEFT" runat ="server" >
				<table class="Table_WithBorder" width="700" border="0">
                   <tr>
						<td width="290" > Import Bank Acknowledgement File :  
                        </td>
                        <td align="right" width="210">
                            <input id="FileFieldEFTAckFile" type="file" runat="server" enableviewstate="true" />
                        </td>
						<td align="left" width="200">
                        	<asp:button id="btnImportEFTBankResponseFile" runat="server" CssClass="Button_Normal" Width="80px" Text="Import"
						    causesValidation="false" Height="21px"></asp:button>
				        </td>
					</tr>
                    <tr>
                        <td colspan="3" style="text-align:left;padding-left:10px;padding-top:10px"> Note : Upon importing the file, approved payments will show as auto-checked / selected.</td>
                    </tr>
                     <tr>
                        <td > &nbsp;</td>
                    </tr>
				</table>
               </div> 
			
            </td>
        </tr>
        <%-- END| SB | 04.09.2018 | YRS-AT-3101 Added file upload control to import the acknowledgement file from the bank for approval of EFT --%>

		<%-- START | SR | 2018.04.10 | Added controls in div for hide/unhide controls based on disbursements Type(CHECK/EFT/APPROVED_EFT). --%>

        <tr>
            <td >
                 <div  id ="divFundNoAndRecalculate" runat ="server" >
                    <table width="700" border ="0" class="Table_WithBorder"> 
                    <tr vAlign="top">                       
                            <td> 
                                <table class="Table_WithOutBorder" width="280">
								    <tr>
									    <td  align="left">Look For&nbsp;<asp:textbox id="TextBoxFundIdNo" runat="server" cssclass="TextBox_Normal" Width="100px" MaxLength="9"></asp:textbox>
										    &nbsp;
										    <asp:button id="ButtonFind" runat="server" CssClass="Button_Normal" 
                                                Text="Fund No." Width="70px"
											    enabled="False"></asp:button></td>
								    </tr>
                                </table> 
                          
                             </td>
                            <td>  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>
                            <td>                                
                                <table class="Table_WithOutBorder" width="250">
								    <tr>
									    <td class="Button_Normal" align="right">
                                            <%--<asp:button id="Button1" CssClass="Button_Normal" Text="Recalculate" Runat="server" Width="92px"
											    enabled="false" onClientClick="RecalculateTotalAmount();" autopostback="false"></asp:button>--%>
                                            <button id="btnRecalculateEFTCheck" class="Button_Normal" runat="server" disabled="disabled"  onclick="SelectAllEFTCheck();">Recalculate</button>
									    </td>
									    <td align="right" ><asp:textbox id="TextBoxNetAmount" runat="server" CssClass="TextBox_Normal_Amount" Width="99px"
											    align="left" Font-Bold="True" enabled="false"></asp:textbox></td>
								    </tr>
                                 </table>                               
                            </td>
                        </tr> 
                        </table>
                     </div> 
            </td>
        </tr>

		<tr>
			<td>
                <div  id ="divCheckSeries" runat ="server" >
                    <%--START : ML| 2019.10.10 | YRS-AT-4642 | Assign table ID and runat to access from Server Side--%>
                     <%--<table width="700"  border ="0" class="Table_WithBorder">--%> 
				<table width="700" id="tblCheckSeries" runat ="server"  border ="0" class="Table_WithBorder"> 
                    <%--END : ML| 2019.10.10 | YRS-AT-4642 | Assign table ID and runat to access from Server Side--%>   
					<tr vAlign="top">
						<td>
							<table class="Table_WithOutBorder" width="280">
								<tr>
									<td><%--Look For&nbsp;<asp:textbox id="TextBoxFundIdNo" runat="server" cssclass="TextBox_Normal" Width="100px" MaxLength="9"></asp:textbox>
										&nbsp;
										<asp:button id="ButtonFind" runat="server" CssClass="Button_Normal" 
                                            Text="Fund Id" Width="70px"
											enabled="False"></asp:button>--%></td>
								</tr>
								<tr>
									<td>&nbsp;
									</td>
								</tr>
								<tr>
									<td>&nbsp;
									</td>
								</tr>
								<%--<tr>
									<td>&nbsp;
									</td>
								</tr>--%>
								<tr>
									<td align="left"><asp:checkbox id="CheckBoxProcessAllSelectedDisbursements" runat="server" CssClass="CheckBox_Normal"
											AutoPostBack="True" Text=" Process All Selected Disbursements" Width="272px" Visible="False"></asp:checkbox></td>
								</tr>
								<tr>
									<TD vAlign="bottom"><asp:button id="ButtonDeselectErroneousDisbursements" runat="server" CssClass="Button_Normal"
											Text="Deselect Erroneous Disbursements" Width="272px" Enabled="False"></asp:button></TD>
								</tr>
							</table>
						</td>
						<td>
							<table class="Table_WithOutBorder" style="WIDTH: 150px;" width="228">
								<tr>
									<td>&nbsp;
									</td>
								</tr>
								<tr>
									<td><asp:checkbox id="CheckBoxManualCheck" runat="server" CssClass="CheckBox_Normal" AutoPostBack="True"
											Text=" Manual Check" Width="128px"></asp:checkbox></td>
								</tr>
								<tr>
									<td vAlign="middle" height="12"><asp:radiobuttonlist id="RadioButtonListUS_Canadian" runat="server" CssClass="RadioButton_Normal" Width="10px"
											Height="4px" RepeatDirection="Horizontal"></asp:radiobuttonlist><asp:radiobutton id="RadioButtonUSA" runat="server" CssClass="RadioButton_Normal" text="U.S." checked="True"
											GroupName="GroupManualCheck"></asp:radiobutton><asp:radiobutton id="RadioButtonCanada" runat="server" CssClass="RadioButton_Normal" text="Canadian"
											GroupName="GroupManualCheck"></asp:radiobutton></td>
								</tr>
								<tr>
									<td align="center"><asp:textbox id="TextBoxManualCheckNo" runat="server" Width="128px" Enabled="False"></asp:textbox></td>
								</tr>
							</table>
						</td>
						<td>
							<table class="Table_WithOutBorder" width="250">
								<tr>
									<td class="Button_Normal" align="right"><%--<asp:button id="Button1" 
                                            CssClass="Button_Normal" Text="Recalculate" Runat="server" Width="92px"
											enabled="false"></asp:button>--%></td>
									<td align="left"><%--<asp:textbox id="TextBoxNetAmount" runat="server" CssClass="TextBox_Normal_Amount" Width="99px"
											align="left" Font-Bold="True"></asp:textbox>--%></td>
								</tr>
								<tr>
									<td align="right"><asp:label id="LabelCheckdate" runat="server" cssclass="Label_Small" text="Check Date"></asp:label></td>
									<td vAlign="top" noWrap align="left"><uc1:dateusercontrol id="TextBoxCheckDate" runat="server" autopostback="false"></uc1:dateusercontrol></td>
								</tr>
								<tr>
									<td>&nbsp;
									</td>
									<td>&nbsp;
									</td>
								</tr>
								<tr>
									<td align="right"><asp:label id="LabelNextUSCheck" runat="server" cssclass="Label_Small" text="Next US Check #"
											DESIGNTIMEDRAGDROP="872"></asp:label></td>
									<td align="left"><asp:textbox id="TextBoxCheckNoUs" runat="server" cssclass="TextBox_Normal" Width="97px"></asp:textbox></td>
								</tr>
								<tr>
									<td align="right"><asp:label id="LabelCanadianCheck" runat="server" cssclass="Label_Small" text="Canadian Check #"></asp:label></td>
									<td align="left"><asp:textbox id="TextBoxCheckNoCanadian" runat="server" cssclass="TextBox_Normal" Width="97px"></asp:textbox></td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
                </div> 
				<%-- END | SR | 2018.04.10 | Added controls in div for hide/unhide controls based on disbursements Type(CHECK/EFT/APPROVED_EFT). --%>
			</td>
		</tr>
	</table>
	<table class="Table_WithOutBorder" width="700">
		<%--<tr>
			<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonSelectNone" 
                    CssClass="Button_Normal" Text="Select None" Runat="server"
					Width="101px"></asp:button></td>
			<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonPrint" accessKey="P" CssClass="Button_Normal" Text="Print" Runat="server"
					Width="80" Enabled="False"></asp:button></td>
			<TD class="Td_ButtonContainer">&nbsp;<asp:button id="_" CssClass="Button_Normal" Text="Pay" Runat="server" Width="80"></asp:button></TD>
			<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonCancel" accessKey="C" CssClass="Button_Normal" Text="Cancel" Runat="server"
					Width="80"></asp:button></td>
		</tr>--%>
        <tr>
			<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonPrint" accessKey="P" CssClass="Button_Normal" Text="Print" Runat="server"
					Width="80" Enabled="False"></asp:button></td>
			<td class="Td_ButtonContainer">&nbsp;<asp:button id="ButtonPay" CssClass="Button_Normal" Text="Generate file" Runat="server" Width="110"></asp:button></td>
            <td class="Td_ButtonContainer" id="tdReject" visible="false"  runat="server">&nbsp;<asp:button id="btnReject" CssClass="Button_Normal" Text="Reject" Runat="server" Width="110"></asp:button></td>
			<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonCancel" accessKey="C" CssClass="Button_Normal" Text="Cancel" Runat="server"
					Width="80"></asp:button></td>
		</tr>
	</table>
	</TABLE><asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>

    <%-- Start: SB | 04.25.2018 | YRS-AT-3101 | Added new pop up to display failed EFT records --%>
    <div id="divEFTFailedPayments" title="EFT - Failed Payment Records" runat="server" style="display: block; overflow: auto;">
        <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td>
                    <div style="overflow: auto; height: 180px">
                        <%--MMR | 2018.12.03 | YRS-AT-3101 | Remove pagination feature--%>
                        <asp:GridView ID="gvEFTFailedPayments" AllowPaging="false" AllowSorting="true" 
                            runat="server" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="false" EmptyDataText="No records found.">
                            <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                            <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                            <RowStyle CssClass="DataGrid_NormalStyle" />
                            <Columns>
                                <asp:BoundField DataField="FundIdNo" ReadOnly="True" HeaderText="Fund ID" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15%" />
                                <asp:BoundField DataField="Name" ReadOnly="True" HeaderText="Name" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="30%" />
                                <%--<asp:BoundField DataField="ExpectedStatus" ReadOnly="True" HeaderText="Expected Status" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20%"/>--%>
                                <asp:TemplateField HeaderText="Expected Status" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <span><%#IIf(Convert.ToString(Eval("Selected")) = True, "APPROVED", "REJECTED")%></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Reason" ReadOnly="True" HeaderText="Failure Reason" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="35%" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <input type="button" value="  OK  " class="Button_Normal" onclick="javascript: return CloseDialog('divEFTFailedPayments');" />
                </td>
            </tr>
        </table>
    </div>
    <%-- End: SB | 04.25.2018 | gvEFTFailedToSendRejectionEmail | YRS-AT-3101 | Added new pop up to display failed EFT records --%>

    <div id="divEFTFailedToSendRejectionEmail" title="EFT - Failed To Send Email" style="display: none;">
        <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td>
                    <div style="overflow: auto; height: 180px">
                        <%--MMR | 2018.12.03 | YRS-AT-3101 | Remove pagination feature--%>
                        <asp:GridView ID="gvEFTFailedToSendRejectionEmail" AllowPaging="false" AllowSorting="true" 
                            runat="server" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="false" EmptyDataText="No records found.">
                            <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                            <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                            <RowStyle CssClass="DataGrid_NormalStyle" />
                            <Columns>
                                <asp:BoundField DataField="FundIdNo" ReadOnly="True" HeaderText="Fund ID" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="15%" />
                                <asp:BoundField DataField="Name" ReadOnly="True" HeaderText="Name" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="30%" />
                                <%--<asp:BoundField DataField="ExpectedStatus" ReadOnly="True" HeaderText="Expected Status" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20%"/>--%>
                                <asp:TemplateField HeaderText="Payment Status" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <span><%#IIf(Convert.ToString(Eval("Selected")) = True, "APPROVED", "REJECTED")%></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Reason" ReadOnly="True" HeaderText="Failure Reason" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="35%" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <input type="button" value="  OK  " class="Button_Normal" onclick="javascript: return CloseDialog('divEFTFailedToSendRejectionEmail');" />
                </td>
            </tr>
        </table>
    </div>

    <%--<div id="divEFTConfirmationDialog" title="EFT Approve/Reject Status" style="display: none;">
        <div>
            <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;">
                            
                <tr>
                    <td>
                        <div id="divConfirmDialogMessage">
                            Are you sure, you want to proceed with Approval/Rejection?
                        </div>
                    </td>
                </tr>
                    <tr>
                    <td>
                        <div id="div2">
                                Approval Status: <span id="spanApprovedRecords" runat="server">0</span> Records 
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="div3">
                                Rejection Status : <span id="spanRecjectedRecords" runat="server">0</span> Records
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
                        <asp:button id="btnYes" CssClass="Button_Normal" Text="Yes" Runat="server" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"></asp:button>
                        <input type="button" value="No" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" onclick="javascript: return CloseDialog('divEFTConfirmationDialog');" />
                    </td>
                </tr>
            </table>
        </div>
    </div>--%>

    <div id="divEFTConfirmationDialog" title="Confirm Electronic Bank Payments (EFT)" style="display: none;">
        <div>
            <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;">
                            
                <tr>
                    <td>
                        <div id="divConfirmDialogMessage">
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
                        <asp:button id="btnYes" CssClass="Button_Normal" Text="Yes" Runat="server" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"></asp:button>
                        <input type="button" value="No" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" onclick="javascript: return CloseDialog('divEFTConfirmationDialog');" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

     <div id="divMismatchRecalculationDialog" title="Mismatch in recalculation" style="display: none;">
        <div>
            <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;">
                            
                <tr>
                    <td>
                        <div id="divMismatchRecalculationMessage">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr id="tr1">
                    <td align="right" valign="bottom" colspan="2">
                        <input type="button" value="OK" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" onclick="javascript: return CloseDialog('divMismatchRecalculationDialog');" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div id="dvWebProcessing">
                <label id="lblProcessing" class="Label_Small"></label>
            </div>
    <div id="divConfirmEFTSelectAllDialog" style="overflow: visible;display:none;">
            
                    <div>
                        <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                            <tr>
                                <td align="left">
                                    <span id="lblMessageConfirm" class="Label_Small"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <IMG title="Image" alt="image" height="40" src="images/spacer.gif" width="10">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="bottom">
                                    <input type="button" ID="btnYesSelectAll" value="Yes" Class="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        OnClick="Javascript: SelectAllEFTDisbursementConfirmed();" />&nbsp;

                                    <input type="button" ID="btnNoSelectAll" value="No" Class="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        OnClick="Javascript: closeConfirmEFtSelectAll();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                
        </div>
    <%--START: PK|12-12-2019 |YRS-AT-4676|Div to display when link in success message is clicked.--%>
    <div id="divErrorMsgToDisplay" title="YMCA-YRS Errors/Warnings" runat="server" style="display: block; overflow: auto;">
        <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td>
                    <div style="overflow: auto; height: 180px">
                       
                        <asp:GridView ID="gvWarnErrorMessage" AllowPaging="false" AllowSorting="true" 
                            runat="server" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="false" EmptyDataText="No records found.">
                            <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                            <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                            <RowStyle CssClass="DataGrid_NormalStyle" />
                            <Columns>
                                <asp:BoundField DataField="intFundNo" ReadOnly="True" HeaderText="Fund No." ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%" />
                                <asp:BoundField DataField="chrType" ReadOnly="True" HeaderText="Message Type" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="6%" />
                                <asp:BoundField DataField="chvErrorDesc" ReadOnly="True" HeaderText="Message Description" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="30%" />
                                </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="text-align:justify"><asp:label id="lblNote" runat="server" text=""></asp:label></div> 
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <input type="button" value="OK" class="Button_Normal" style="Width:73px;" onclick="javascript: return CloseDialog('divErrorMsgToDisplay');" />                    
                </td>
            </tr>
        </table>
    </div>
    <%--END: PK|12-12-2019 |YRS-AT-4676|Div to display when link in success message is clicked.--%>
</form>
<!--#include virtual="bottom.html"-->
