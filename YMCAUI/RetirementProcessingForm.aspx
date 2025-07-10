<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master" CodeBehind="RetirementProcessingForm.aspx.vb" EnableEventValidation="false"
    Inherits="YMCAUI.RetirementProcessingForm" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="YRSControls" TagName="AddressWebUserControl" Src="UserControls/AddressUserControl_new.ascx" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="YRSCustomControls" %> <%-- PPP | 2015.09.25 | YRS-AT-2596 --%>
<%@ Register TagPrefix="YRSStateTaxControls" TagName="StateWithholdingListing_WebUserControl" Src="~/UserControls/StateWithholdingListingControl.ascx" %><%-- ML |20.09.2019| YRS-AT-4597 |Added refrence of Statewithholding User control --%>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <%--START: MMR | 2017.03.03 | YRS-AT-2625 |Add style to set close icon display property--%>
    <style>
        .no-close .ui-dialog-titlebar-close {display: none; }
    </style>
    <%--EMD: MMR | 2017.03.03 | YRS-AT-2625 |Add style to set close icon display property--%>
    <script language="javascript" type="text/javascript">
        /* Shashi Shekhar:2009-12-31: comment and shift ValidateNumeric() in common external js file(JS/YMCA_JScript.js). Please check older version from SVN if needed. */

    	$(document).ready(function () {
    		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
    		function EndRequest(sender, args) {
    			if (args.get_error() == undefined) {
    				BindEvents();
    			}
    		}
    		BindEvents();
    	});
    	
		

        function ValidateDecimal() {
            //alert(event.keyCode);
            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 46) {
                event.returnValue = false;
            }
        }
        function test() {
            /*document.Form1.all.TextboxOldRetDate.value = document..all.TextBoxRetirementDate.value
            alert(document.Form1.all.TextboxOldRetDate.value);*/

        }
        function onRetirementDateChangeDoPostback() {
            document.Form1.submit();
        }
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

        <%-- START: PPP | 2015.10.05 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788) 
        "ValidateDate" javascript function is part of the CustomControl.Resources.CalenderTextBox.js file which can be accessed without adding the javascript reference --%>
        ////2012.05.28 SP:   BT-975/YRS 5.0-1508 - 
        //function PostBackCalendarBenef() {
        //    document.Form1.submit();
        //}
        function PostBackCalendarBenef(control) {
            if (ValidateDate(control.value, '')) { 
                document.Form1.submit();
            }
        }
        <%-- END: PPP | 2015.10.05 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788) --%>
        
        //Sanjay R:2013.08.05 - YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
        function BindEvents() { 
	    //$(document).ready(function () {
            $("#divWSMessage").dialog
					({
					    modal: true,
					    open: function (event, ui) { $(this).parent('div').find('button:contains("SAVE")').focus(); },
					    autoOpen: false,
					    title: "Process Restricted",
					    width: 570, height: 200,
					    buttons: [{ text: "OK", click: CloseWSMessage}]
					});
     

					  $('#ConfirmDialog').dialog({
					  	autoOpen: false,
					  	draggable: true,
					  	close: false,
					  	//width: 520, height: 300,
					  	title: "Retirement Processing",
					  	open: function (type, data) {
					  		$(this).parent().appendTo("form");
					  		$('a.ui-dialog-titlebar-close').remove();
					  	}
					  });
					// });
                        
      <%--START: MMR | 2017.03.03 | YRS-AT-2625 | Displaying manual transactions list in pop up --%>
            $('#divTransactionList').dialog({
                autoOpen: false,
                resizable: false,
                dialogClass: 'no-close',
                draggable: true,
                width: 630, minheight: 200,
                height: 550,
                closeOnEscape: false,
                title: "Manage Manual Transactions",
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                }
            });

            //Check/uncheck all checkboxes in list according to main checkbox 
            $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeHeader']:checkbox").click(function () {
					       //Header checkbox is checked or not
					       var bool = $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeHeader']:checkbox").is(':checked');
                //check and check the checkboxes on basis of Boolean value
                $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeRow']:checkbox").attr('checked', bool);
            });


            $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeRow']:checkbox").click(function () {
                //Get number of checkboxes in list either checked or not checked
                var totalCheckboxes = $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeRow']:checkbox").size();
                //Get number of checked checkboxes in list
                var checkedCheckboxes = $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeRow']:checkbox:checked").size();
                //check and uncheck the header checkbox on the basis of difference of both values
                $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeHeader']:checkbox").attr('checked', totalCheckboxes == checkedCheckboxes);

            });
        }

        function CloseTransactionDialog() {
            $('#divTransactionList').dialog("close");
        }

        function getSelectedManualTransaction() {
            var getAllUnSelectedUniqueId = "";
            $('#<%=hdnManualTransaction.ClientID%>').val("3");
            var checkedCheckboxes = $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeRow']:checkbox:not(:checked)")
            checkedCheckboxes.each(function (chk) {
                var getUniqueId = $($(checkedCheckboxes[chk]).closest('tr').find('.hideGridColumn')).text();
                getAllUnSelectedUniqueId = getAllUnSelectedUniqueId + getUniqueId + ",";
            });

            $.ajax({
                type: "POST",
                url: "RetirementProcessingForm.aspx/GetSelectedManualTransactions",
                data: "{'uniqueIDs':'" + getAllUnSelectedUniqueId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    CloseTransactionDialog();
                    if ($('#<%=hdnSourceManualTransaction.ClientID%>').val() == "1") {
                        $('#DivWarningMessage').hide();
                    }
                    else if ($('#<%=hdnSourceManualTransaction.ClientID%>').val() == "2") {
                        $('#<%=ButtonReCalculate.ClientID%>').click();
                    }
                }
            });
        }
        <%--END: MMR | 2017.03.03 | YRS-AT-2625 | Displaying manual transactions list in pop up --%>

        <%-- START: MMR | 2017.03.17 | YRS-AT-2625 | Added to set hidden field value on click of no button in message box--%>
        function setHiddenfieldValue() {
            $('#hdnMessage').val("True");
        }
        <%-- END: MMR | 2017.03.17 | YRS-AT-2625 | Added to set hidden field value on click of no button in message box--%>
        
        <%--START: MMR | 2017.03.03 | YRS-AT-2625 | Commented existing code and moved at the end to show pop-up--%>
        <%--function showDialog(id, text, btnokvisibility) {
            $('#' + id).dialog({ modal: true });
            //Start: AA:03.16.2016 YRS-AT-2599 Added below code to show confirmation dailog
            if (id = 'ConfirmDialog') {
                if (text.indexOf('##') != -1) {
                    var arrtext = text.split("##");
                    var finaltext = arrtext[arrtext.length - 1] + '<ul>';//Heading title
                    for (var i = 0; i < arrtext.length - 2; i++) {
                        finaltext += '<li>' + arrtext[i] + '</li>';//Points
                    }
                    finaltext += '</ul><br/>' + arrtext[arrtext.length - 2];//confirmation message
                    $('#lblMessage').html(finaltext);
                    var divheight = 150 + (arrtext.length * 100);// PP:03.22.2016 YRS-AT-2599 Changed the height of dialog box 
                    var divWidth = 600;
                    $('#' + id).dialog({ width: divWidth, height: divheight });
                }
                else {
                    $('#lblMessage').text(text);
                    $('#' + id).dialog({ width: 480, height: 250 });// PP:03.22.2016 YRS-AT-2599 Changed the width and height of dialog box
                }
            }
            //End: AA:03.16.2016 YRS-AT-2599 Added below code to show confirmation dailog
            $('#' + id).dialog("open");
            //SP 2014.02.20 BT-2436 -Start
            if (btnokvisibility == "Cancel") {
                $("#ButtonYes").hide();
                $("#ButtonCancelYes").show();
            }
            else {
                $("#ButtonCancelYes").hide();
                $("#ButtonYes").show();
            }
            //SP 2014.02.20 BT-2436 -End
            $("#btnNo").show();
        } --%>
        <%--END: MMR | 2017.03.03 | YRS-AT-2625 | Commented existing code and moved at the end to show pop-up--%>

        function CloseWSMessage() {
            $(document).ready(function () {
                $("#divWSMessage").dialog('close');
            });
        }
    
        function openDialog(str, type) {
            $(document).ready(function () {
                //InitializeTerminationWatcherDialogBox();
                if (type == 'Bene') {
                    str = 'Beneficiary add,edit and delete operation can not be performed due to following reason(s). <br/>' + str
                }
                else {
                    str = 'Person\'s Marital Status can not be changed due to following reason(s). <br/>' + str;
                }
                $("#divWSMessage").html(str);
                $("#divWSMessage").dialog('open');
                return false;
            });
        }
        //End, Sanjay R:2013.08.05 - YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)

    

       
		function ShowWindow() {
			//alert(document.getElementById("TrIncludeSSLevelling"));
			//alert(Request["RetType"].toString());
			var val = getQuerystring('RetType');

			if (val == "Disability") {
				document.getElementById('TrIncludeSSLevelling').style.display = "none";
				document.getElementById('TrIncludeSSLevellingValue').style.display = "none";
			}
			else {
				document.getElementById('TrIncludeSSLevelling').style.display = "block";
				document.getElementById('TrIncludeSSLevellingValue').style.display = "block";
			}
		}

		function getQuerystring(key, default_) {
			if (default_ == null) default_ = "";
			key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
			var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
			var qs = regex.exec(window.location.href);
			if (qs == null)
				return default_;
			else
				return qs[1];
		}

		$(document).ready(function () {
			ShowWindow();


		});
		function CloseDialog() {

			$("#ConfirmDialog").dialog('close');

		}
		//SR:2013.08.05 - YRS 5.0-2070 : Create tooltip to display web service message.   
		function showToolTip(message, Type) {
			var elementRef = document.getElementById("<%=Tooltip.ClientID%>");
			if (elementRef != null) {
				elementRef.style.position = 'absolute';
				elementRef.style.left = event.clientX + 5 + document.body.scrollLeft;
				elementRef.style.top = event.clientY + 5 + document.body.scrollTop;
				elementRef.style.width = '630';
				elementRef.style.visibility = 'visible';
				elementRef.style.display = 'block';
			}
			var lblNote = document.getElementById("<%=lblComments.ClientID%>");
			// checking whether tooltips exists or not
			if (Type == 'Pers') {
				lblNote.innerText = 'Person\'s Marital Status can not be changed due to following reason(s).\n' + message;
			}
			else if (Type == 'Bene') {
				lblNote.innerText = 'Beneficiary add,edit and delete operation can not be performed due to following reason(s). \n' + message;
			}
			else {
				lblNote.innerText = message;
			}
		}
		//to hide tool tip when mouse is removed
		function hideToolTip() {
			var elementRef = document.getElementById("<%=Tooltip.ClientID%>");
			if (elementRef != null) {
				elementRef.style.visibility = 'hidden';
			}
		
	}
		//End, SR:2013.08.05 - YRS 5.0-2070 : Create tooltip to display web service message.   
</script>
</asp:Content>
	<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
		<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
		</asp:ScriptManagerProxy>
        <div id="divErrorMsg" class="error-msg" runat="server" style="text-align: left; display: none;" enableviewstate="false"></div> <%-- PK | 2019.12.13 | YRS-AT-4598 | Added div to display error message for state tax validation --%>
        <div id="DivWarningMessage" class="warning-msg" runat="server" style="text-align: left; display: none;" enableviewstate="false"> <%-- MMR | 2017.03.01 | YRS-AT-2625 | Added div to display warning message for manual Transaction--%> 
                </div>  
    <div class="Div_Center" style ="width:100%">
		
        <table class='Table_WithOutBorder"' width="100%">
          <%--  <tr>
                <td>                   
					2012.05.28 SP :1508 : added new lable for diaplying message
					<asp:Label ID="LabelEstimateDataChangedMessage" runat="server" Visible="false"  Text='' CssClass="Error_Message"></asp:Label>
					
                </td>
            </tr>    --%>      
            <tr>
                <td width="100%">
                    <iewc:TabStrip ID="TabStripRetireesInformation" runat="server" Width="100%" BorderStyle="None"
                        AutoPostBack="True" 
						TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
                        TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
                        Height="30px">
                        <iewc:Tab Text="Annuity" ID="tabAnnuity" DefaultStyle="width:12%"></iewc:Tab>
                        <%--START : ML | 2019.09.20 | YRA-At-4597 | Rename Federal withholding to Tax withholding--%>
                       <%-- <iewc:Tab Text="Federal Withholding" ID="tabFederal" DefaultStyle="width:16%"></iewc:Tab>--%>
                        <iewc:Tab Text="Tax Withholding" ID="tabTax" DefaultStyle="width:16%"></iewc:Tab>
                        <%--END : ML | 2019.09.20 | YRA-At-4597 | Rename Federal withholding to Tax withholding--%>
                        <iewc:Tab Text="General Withholding" ID="tabGeneral" DefaultStyle="width:16%"></iewc:Tab>
                        <iewc:Tab Text="Death Benefit Beneficiaries" ID="tabDeathBenefit" DefaultStyle="width:22%"></iewc:Tab>
                        <iewc:Tab Text="Annuity Beneficiary" ID="tabAnnuityBeneficiary" DefaultStyle="width:14%"></iewc:Tab>
                        <iewc:Tab Text="Notes" ID="tabNotes" DefaultStyle="width:10%"></iewc:Tab>
                        <iewc:Tab Text="Purchase" ID="tabPurchase" DefaultStyle="width:10%"></iewc:Tab>
                    </iewc:TabStrip>
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <iewc:MultiPage ID="MultiPageRetirementProcessing" runat="server" EnableViewState="true">
                        <iewc:PageView>
                            <table class="Table_WithBorder" width="100%">
                                <tr>
                                    <td>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td colspan="4" class="td_Text">
                                                    Annuity
                                                </td>
                                            </tr>
											<tr>
												<td colspan='4'>
													<table width="100%" border="0" cellpadding='0' cellspacing='0'>
														<tr>
															<td  style="width:120px">
																<asp:Label ID="LabelRetirementType" runat="server" CssClass="Label_Small">Retirement Type : </asp:Label>
															</td>
															<td align="left" style="width:200px">
																<asp:Label ID="lblRetirementType" runat="server" CssClass="TextBox_Normal"></asp:Label>
															</td>
															<td  style="width:80px">
																<asp:Label ID="LabelBirthDateRet" runat="server" CssClass="Label_Small">Birth Date : </asp:Label>	
															</td>
															<td align="left" style="width:250px">
																	<asp:TextBox ID="TextBoxBirthDateRet" runat="server" BorderStyle="NotSet" Width="70px" ReadOnly="True"
																	CssClass="TextBox_Normal"></asp:TextBox>
															</td>
															<td style="width:80px">
															<asp:Label ID="LabelRetirementDate" runat="server" CssClass="Label_Small" Width="120px">Retirement Date : </asp:Label>
															</td>
															<td align="left">
															<uc1:DateUserControl ID="TextBoxRetirementDate" runat="server" AutoPostBack ="true">
																</uc1:DateUserControl>
															
															</td>
														</tr>
													    <%-- START: MMR | 2017.03.03 | YRS-AT-2625 | Added to show link for manual transactions --%>
                                                        <tr>
                                                            <td colspan="4"></td>
                                                            <td colspan="2" align="right" class="Link_SmallBold" style="height:20px; vertical-align:bottom;">
                                                                <a href="#" id="lnkManualTransaction" runat="server" style="font-size:11px" visible="false"  onclick="ShowTransactionDialog('1');">Manage Manual Transactions</a>
                                                            </td>
                                                        </tr>
													    <%-- END: MMR | 2017.03.03 | YRS-AT-2625 | Added to show link for manual transactions --%>
													</table>
												</td>
											 </tr>
                                           <%-- <tr>
                                                <td align="right">
                                                  
                                                </td>
                                                <td align="left">
                                                    <asp:dropdownlist id="DropDownListRetirementType" runat="server" Width="176px" CssClass="DropDown_Normal"
																AutopostBack="True">
																<asp:ListItem Value="NORMAL" Selected="true">Normal</asp:ListItem>
																<asp:ListItem Value="DISABLED">Disabled</asp:ListItem>
															</asp:dropdownlist>
                                                  
                                                </td>
                                                <td align="right">
                                                   
                                                </td>
                                                <td align="left">
                                                   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td align="right">
                                                   
                                                </td>
                                                <td align="left">
                                                   
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td colspan="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <asp:CheckBox ID="CheckBoxRetPlan" runat="server" Text="Retirement Plan" CssClass="Label_Medium"
                                                        Checked="true" AutoPostBack="true"></asp:CheckBox>
                                                </td>
                                                <td colspan="2" align="left" >
                                                    <asp:CheckBox ID="CheckBoxSavPlan"  runat="server" Text="Savings Plan" CssClass="Label_Medium"
                                                        Checked="true" AutoPostBack="true"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td colspan="2" valign="top" style="width:50%">
                                                    <table border="0" width="100%" class="Table_WithBorder">
                                                        <tr>
                                                            <td>
                                                                <table width="100%" bgcolor="#ffffff">
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="LabelAnnuitySelectRet" runat="server" CssClass="Label_Small">Annuity</asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="TextBoxAnnuitySelectRet" runat="server" Width="184px" CssClass="TextBox_Normal"
                                                                                Text="M" ReadOnly="True"></asp:TextBox>&nbsp;
                                                                            <asp:Button ID="ButtonSelectRet" runat="server" Text="Select" CssClass="Button_Normal"
                                                                                CausesValidation="false"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right" valign="top">
                                                                        </td>
                                                                        <td align="left">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right" nowrap>
                                                                            <asp:Label ID="LabelTaxableRet" runat="server" CssClass="Label_Small">Taxable</asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="TextBoxTaxableRet" runat="server" Width="184px" ReadOnly="True"
                                                                                CssClass="TextBox_Normal"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right" nowrap>
                                                                            <asp:Label ID="LabelNonTaxableRet" runat="server" CssClass="Label_Small">Non Taxable</asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="TextBoxNonTaxableRet" runat="server" Width="184px" ReadOnly="True"
                                                                                CssClass="TextBox_Normal"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right" nowrap >
                                                                            <asp:Label ID="LabelTotalPaymentRet" runat="server" CssClass="Label_Small">Total Payment</asp:Label>
                                                                        </td>
                                                                        <td align="left" >
                                                                            <asp:TextBox ID="TextBoxTotalPaymentRet" runat="server" Width="184px" ReadOnly="True"
                                                                                CssClass="TextBox_Normal"></asp:TextBox>
                                                                                
                                                                           <%--  <asp:Label ID="LabelDBIncluded" runat="server" ToolTip="Death Benefit is included in this amount"
                                                                                CssClass="Label_Small" > </asp:Label> --%>
                                                                                <%-- Added By Anudeep:29.11.2012 To show Help image   --%>
                                                                                 <asp:Image ID ="imgHelp" Visible="False" Height="20" style="vertical-align:middle;" Width="20" ToolTip="Death Benefit is included in this amount" runat="server" ImageUrl="~/images/help.jpg" /> 
                                                                            
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right" nowrap>
                                                                            <asp:Label ID="LabelTotalReservesRet" runat="server" CssClass="Label_Small">Total Reserves</asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="TextBoxReservesRet" runat="server" Width="184px" ReadOnly="True"
                                                                                CssClass="TextBox_Normal"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" colspan="2">
                                                                            <asp:Label ID="LabelDeathBenefitDetails" runat="server" CssClass="Label_Medium">Death Benefit Options</asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="LabelRetirementDeathBenefit" runat="server" CssClass="Label_Small">Retirement Death Benefit</asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="TextBoxRetiredBenefit" runat="server" Width="177px" CssClass="TextBox_Normal"
                                                                                ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label ID="LabelPercentageToUse" runat="server" CssClass="Label_Small">% To Use</asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:DropDownList ID="DropDownListPercentage" runat="server" Width="178px" Height="24px"
                                                                                CssClass="DropDown_Normal" AutoPostBack="true" MaxLength="10">
                                                                            </asp:DropDownList>
                                                                            <asp:ListBox ID="ListBoxPercentage" runat="server" Width="177px" Height="24px" CssClass="DropDown_Normal"
                                                                                Visible="false"></asp:ListBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right" nowrap>
                                                                            <asp:Label ID="LabelAmountToUse" runat="server" CssClass="Label_Small">$ Amount to use</asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="TextBoxAmount" runat="server" Width="178" CssClass="TextBox_Normal"
                                                                                Text="0.00" AutoPostBack="true" onkeypress="ValidateDecimal()" MaxLength="10" ReadOnly="true" ></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td colspan="2" valign="top" style="width:50%">
                                                    <table width="100%" >
                                                        <tr>
                                                            <td>
                                                                <table border="0" width="100%"  class="Table_WithBorder">
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" bgcolor="#ffffff">
                                                                              <%--  <tr>
                                                                                    <td align="center" colspan="2">
                                                                                    </td>
                                                                                </tr>--%>
                                                                                <tr>
                                                                                    <td align="right" style="width:40%">
                                                                                        <asp:Label ID="LabelAnnuitySelectSav" runat="server" CssClass="Label_Small">Annuity</asp:Label>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:TextBox ID="TextBoxAnnuitySelectSav" runat="server" Width="184px" CssClass="TextBox_Normal"
                                                                                            Text="M" ReadOnly="True"></asp:TextBox>&nbsp;
                                                                                        <asp:Button ID="ButtonSelectSav" runat="server" Text="Select" CssClass="Button_Normal"
                                                                                            CausesValidation="false"></asp:Button>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr valign="top">
                                                                                    <td align="right" valign="top">
                                                                                    </td>
                                                                                    <td align="left">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right" style="width:40%">
                                                                                        <asp:Label ID="LabelTaxableSav" runat="server" CssClass="Label_Small">Taxable</asp:Label>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:TextBox ID="TextBoxTaxableSav" runat="server" Width="184px" ReadOnly="True"
                                                                                            CssClass="TextBox_Normal"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right" style="width:40%">
                                                                                        <asp:Label ID="LabelNonTaxableSav" runat="server" CssClass="Label_Small">Non Taxable</asp:Label>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:TextBox ID="TextBoxNonTaxableSav" runat="server" Width="184px" ReadOnly="True"
                                                                                            CssClass="TextBox_Normal"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right" nowrap style="width:40%">
                                                                                        <asp:Label ID="LabelTotalPaymentSav" runat="server" CssClass="Label_Small">Total Payment</asp:Label>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:TextBox ID="TextBoxTotalPaymentSav" runat="server" Width="184px" ReadOnly="True"
                                                                                            CssClass="TextBox_Normal"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right" nowrap style="width:40%">
                                                                                        <asp:Label ID="LabelTotalReservesSav" runat="server" CssClass="Label_Small">Total Reserves</asp:Label>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:TextBox ID="TextBoxReservesSav" runat="server" Width="184px" ReadOnly="True"
                                                                                            CssClass="TextBox_Normal"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="2">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr id="TrIncludeSSLevelling">
                                                            <td align="left" colspan="2">
                                                                <asp:CheckBox ID="CheckboxIncludeSSLevelling" runat="server" CssClass="Label_Medium"
                                                                    Text="Include Social Security Leveling" AutoPostBack="true"></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="TrIncludeSSLevellingValue">
                                                            <td align="left" colspan='2'>
                                                                <table border="0" width="100%"  class="Table_WithBorder">
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" bgcolor="#ffffff">
                                                                               <%-- <tr>
                                                                                    <td colspan="2">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right">
                                                                                    </td>
                                                                                    <td align="left">
                                                                                    </td>
                                                                                </tr>--%>
                                                                                <tr>
                                                                                    <td align="right" style="width:40%">
                                                                                        <asp:Label ID="LabelSSBenefit" runat="server" CssClass="Label_Medium">Benefit Value</asp:Label>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:TextBox ID="TextBoxSSBenefit" width="184px" runat="server" CssClass="TextBox_Normal" Text="0.00"
                                                                                            ReadOnly="True" MaxLength="10"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right" style="width:40%">
                                                                                        <asp:Label ID="LabelSSIncrease" runat="server" CssClass="Label_Medium">SS Increase</asp:Label>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:TextBox ID="TextBoxSSIncrease" width="184px"  runat="server" CssClass="TextBox_Normal" ReadOnly="True"
                                                                                            Text="0.00"></asp:TextBox>
                                                                                        <asp:Label ID="LabelBefore62" runat="server" CssClass="Label_Small" Visible="False">(before 62)</asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right" style="width:40%">
                                                                                        <asp:Label ID="LabelSSDecrease"  runat="server" CssClass="Label_Small" Visible="False">SS Decrease</asp:Label>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:TextBox ID="TextBoxSSDecrease"  runat="server" Width="184px" CssClass="TextBox_Normal"
                                                                                            ReadOnly="True" Visible="False" Text="0.00"></asp:TextBox>
                                                                                        <asp:Label ID="LabelAfter62" runat="server" CssClass="Label_Small" Visible="False">(after 62)</asp:Label>
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
												<td colspan="4">
                                                <table class="Table_WithBorder" width="100%" border="0" align="left">
                                                    <tr>
                                                        <td nowrap width="15%">
                                                            <asp:Label ID="LabelExistingAnnuities" runat="server" CssClass="Label_Small">Existing Annuities</asp:Label>
                                                        </td>
                                                        <td nowrap width="15%">
                                                            <asp:Label ID="LabelCurrentPurchase" runat="server" CssClass="Label_Small">Current Purchase</asp:Label>
                                                        </td>
                                                        <td nowrap width="20%">
                                                            <asp:Label ID="LabelSSLevelling" runat="server" CssClass="Label_Small">Social Security Leveling</asp:Label>
                                                        </td>
                                                        <td nowrap width="10%">
                                                            <asp:Label ID="LabelLevellingBefore62" runat="server" CssClass="Label_Small">Before 62</asp:Label>
                                                        </td>
                                                        <td nowrap width="20%">
                                                            <asp:Label ID="LabelLevellingSSBenefit" runat="server" CssClass="Label_Small">Social Security Benefit</asp:Label>
                                                        </td>
                                                        <td nowrap width="20%">
                                                            <asp:Label ID="LabelLevellingAfter62" runat="server" CssClass="Label_Small">After 62</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" nowrap>
                                                            <asp:TextBox ID="TextboxExistingAnnuities" Width="110" runat="server" CssClass="TextBox_Normal"
                                                                ReadOnly="True" Text="0.00"></asp:TextBox>&nbsp;&nbsp;+&nbsp;
                                                        </td>
                                                        <td align="left" nowrap>
                                                            <asp:TextBox ID="TextboxCurrentPurchase" Width="110" runat="server" CssClass="TextBox_Normal"
                                                                ReadOnly="True" Text="0.00"></asp:TextBox>&nbsp;&nbsp;+&nbsp;
                                                        </td>
                                                        <td align="left" nowrap>
                                                            <asp:TextBox ID="TextboxSSLevelling" Width="150" runat="server" CssClass="TextBox_Normal"
                                                                ReadOnly="True" Text="0.00"></asp:TextBox>&nbsp;&nbsp;=&nbsp;
                                                        </td>
                                                        <td align="left" nowrap>
                                                            <asp:TextBox ID="TextboxLevellingBefore62" Width="75" runat="server" CssClass="TextBox_Normal"
                                                                ReadOnly="True" Text="0.00"></asp:TextBox>&nbsp;&nbsp;-&nbsp;
                                                        </td>
                                                        <td align="left" nowrap>
                                                            <asp:TextBox ID="TextboxLevellingSSBenefit" Width="150" runat="server" CssClass="TextBox_Normal"
                                                                ReadOnly="True" Text="0.00"></asp:TextBox>&nbsp;&nbsp;=&nbsp;
                                                        </td>
                                                        <td align="left" nowrap>
                                                            <asp:TextBox ID="TextboxLevellingAfter62" Width="85" runat="server" CssClass="TextBox_Normal"
                                                                ReadOnly="True" Text="0.00"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
												</td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </iewc:PageView>
                        <iewc:PageView>
                            <table width="100%" border="0" style="height:400px">
                                <tr>
                                    <td valign="top">
                                        <table class="Table_WithBorder" width="100%" height="100%" cellspacing="0">
                                            <tr>
                                                <td align="left" class="Td_Text">
                                                    Federal Withholding
                                                </td>
                                                <td align="right" class="Td_ButtonContainer">
                                                    <%--START :  ML |20.09.2019| YRS-AT-4597 |Button Width added --%>
                                                  <%-- <asp:Button runat="server" ID="ButtonAdd" Text="Add..." CssClass="Button_Normal"
                                                        CausesValidation="False"></asp:Button>--%> 
                                                    <asp:Button runat="server" ID="ButtonAdd" Text="Add..." Width="90px" CssClass="Button_Normal" CausesValidation="False"></asp:Button> 
                                                    <%--END : ML |20.09.2019| YRS-AT-4597 |Button Width added --%>
                                                    <asp:Button runat="server" ID="ButtonUpdate" Visible="false" Text="Update Item" CssClass="Button_Normal"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center" valign="top">
                                                     <div style="overflow: auto; width: 100%; height: 140px; text-align: left"><%-- ML |20.09.2019| YRS-AT-4597 |Added new div to maintain grid height --%>
                                                    <asp:DataGrid ID="DataGridFederalWithholding" runat="server" Width="100%" CssClass="DataGrid_Grid"
                                                        AutoGenerateColumns="false">
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                        <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderStyle-Width="2%">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ImageButtonEdit" runat="server" ToolTip="Edit" CommandName="Select"
                                                                        CausesValidation="False" ImageUrl="~/images/edits.gif" AlternateText="Edit">
                                                                    </asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn HeaderText="Exemptions" DataField="Exemptions" HeaderStyle-Width="20%" />
                                                            <asp:BoundColumn HeaderText="Add'l Amount" DataField="Add'l Amount" HeaderStyle-Width="23%" />
                                                            <asp:BoundColumn HeaderText="Type" DataField="Type" HeaderStyle-Width="20%" />
                                                            <asp:BoundColumn HeaderText="Tax Entity" DataField="Tax Entity" HeaderStyle-Width="20%"/>
                                                            <asp:BoundColumn HeaderText="Marital Status" DataField="Marital Status" HeaderStyle-Width="15%" />
                                                            <asp:BoundColumn HeaderText="ID" DataField="FedWithdrawalID" Visible="false" />
                                                        </Columns>
                                                    </asp:DataGrid>
                                                         </div> <%-- ML |20.09.2019| YRS-AT-4597 |Div Ended For Federal Withholding--%>
                                                </td>
                                            </tr>
                                            <%--START : ML |20.09.2019| YRS-AT-4597 |State withholding user control added.--%>
                                            <tr>                                              
                                                <%--DO NOT CHANGE PreFixID value as it is used to Manage UserPermission. Page Wise PrefixID will be different--%>
                                                <YRSStateTaxControls:StateWithholdingListing_WebUserControl ID ="stwListUserControl" ClientIDMode="Static" PreFixID="annuityProcess" runat="server" />
                                            </tr>
                                             <%--END : ML |20.09.2019| YRS-AT-4597|State withholding user control added  --%>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </iewc:PageView>
                        <iewc:PageView>
                            <table width="100%"   style="height:400px">
                                <tr>
                                    <td valign="top">
                                        <table class="Table_WithBorder" width="100%" height="100%" cellspacing="0">
                                            <tr>
                                                <td align="left" class="td_Text">
                                                    General Withholding
                                                </td>
                                                <td align="right" class="Td_ButtonContainer">
                                                    <asp:Button runat="server" ID="ButtonAddGeneralWithholding" Text="Add..." CssClass="Button_Normal"
                                                        CausesValidation="False"></asp:Button>
                                                    <asp:Button runat="server" ID="ButtonUpdateGeneralWithholding" Visible="false" Text="Update Item"
                                                        CssClass="Button_Normal" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left" valign="top">
                                                    <asp:DataGrid ID="DataGridGeneralWithholding" runat="server" Width="100%" CssClass="DataGrid_Grid"
                                                        AutoGenerateColumns="false">
                                                        <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderStyle-Width="2%">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ImagebuttonEdit" runat="server" ToolTip="Edit" CommandName="Select"
                                                                        CausesValidation="False" ImageUrl="~/images/edits.gif" AlternateText="Edit">
                                                                    </asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn HeaderText="Type" DataField="Type" HeaderStyle-Width="28%" />
                                                            <asp:BoundColumn HeaderText="Add'l Amount" DataField="Add'l Amount" HeaderStyle-Width="30%" />
                                                            <asp:BoundColumn HeaderText="Start Date" DataField="Start Date" HeaderStyle-Width="20%" />
                                                            <asp:BoundColumn HeaderText="End Date" DataField="End Date" HeaderStyle-Width="20%" />
                                                            <asp:BoundColumn HeaderText="ID" DataField="GenWithdrawalID" Visible="false" />
                                                        </Columns>
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </iewc:PageView>
                        <iewc:PageView>
                            <table width="100%" class="Table_WithBorder" style="height:400px">
                                <tr>
                                    <td align="left" class="td_Text">
                                        Beneficiaries Details
                                        <asp:Image ID="imgLockBeneficiary" runat="server" ImageUrl="Images/lock-yellow.png" visible="false" Width="20px" height="20px"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <table id="Table1" width="100%">
                                            <tr>
                                                <td valign="top">
                                                    <asp:Label runat="server" Width="150" CssClass="Label_Large" ID="LabelNotSet">Beneficiaries cannot be set</asp:Label>
                                                    <div style="overflow: auto; height: 120px; text-align: left;">
                                                        <asp:DataGrid ID="DataGridActiveBeneficiaries" runat="server" Width="100%" CssClass="DataGrid_Grid" AutoGenerateColumns="false"> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 |Not allow columns to auto-generate--%>
                                                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                            <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                            <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                            <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="CheckboxActBen" runat="server" Checked="True" CssClass="CheckBox_Normal">
                                                                        </asp:CheckBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <%--Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and added bounded columns to datagrid--%>
                                                                <asp:BoundColumn DataField="UniqueId" HeaderText="UniqueId"></asp:BoundColumn>
																<asp:BoundColumn DataField="PersId" HeaderText="PersId"></asp:BoundColumn>
																<asp:BoundColumn DataField="BenePersId" HeaderText="BenePersId"></asp:BoundColumn>
																<asp:BoundColumn DataField="BeneFundEventId" HeaderText="BeneFundEventId"></asp:BoundColumn>
																<asp:BoundColumn DataField="Name" HeaderText="Name"></asp:BoundColumn>
																<asp:BoundColumn DataField="Name2" HeaderText="Name2"></asp:BoundColumn>
																<asp:BoundColumn DataField="TaxID" HeaderText="TaxID"></asp:BoundColumn>
																<asp:BoundColumn DataField="Rel" HeaderText="Rel"></asp:BoundColumn>
																<asp:BoundColumn DataField="Birthdate" HeaderText="Birth/Estd. Date"></asp:BoundColumn>
																<asp:BoundColumn DataField="BeneficiaryTypeCode" HeaderText="BeneficiaryTypeCode"></asp:BoundColumn>
																<asp:BoundColumn DataField="Groups" HeaderText="Groups"></asp:BoundColumn>
																<asp:BoundColumn DataField="Lvl" HeaderText="Lvl"></asp:BoundColumn>
																<asp:BoundColumn DataField="DeathFundEventStatus" HeaderText="DeathFundEventStatus"></asp:BoundColumn>
																<asp:BoundColumn DataField="BeneficiaryStatusCode" HeaderText="BeneficiaryStatusCode"></asp:BoundColumn>
																<asp:BoundColumn DataField="Pct" HeaderText="Pct"></asp:BoundColumn>
																<asp:BoundColumn DataField="PlanType" HeaderText="PlanType"></asp:BoundColumn>
																<asp:BoundColumn DataField="RepFirstName" HeaderText="RepFirstName"></asp:BoundColumn>
																<asp:BoundColumn DataField="RepFirstName" HeaderText="RepFirstName"></asp:BoundColumn>
																<asp:BoundColumn DataField="RepFirstName" HeaderText="RepFirstName"></asp:BoundColumn>
																<asp:BoundColumn DataField="RepFirstName" HeaderText="RepFirstName"></asp:BoundColumn>
                                                                <%--End - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and changed header text for birth date--%>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:Button ID="ButtonMoveBeneficiaries" runat="server" Text="Move Selected Beneficiaries"
                                                        CssClass="Button_Normal" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:90%">
                                                    <div style="overflow: auto; width: 100%; height: 120px; text-align: left">
                                                        <asp:DataGrid ID="DataGridRetiredBeneficiaries" AutoGenerateColumns="false" runat="server" Width="100%" CssClass="DataGrid_Grid">
                                                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                            <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                            <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                            <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Imagebutton2" runat="server" ToolTip="Select" CommandName="Select"
                                                                            CausesValidation="False" ImageUrl="~/images/select.gif" AlternateText="Select">
                                                                        </asp:ImageButton>
																		  </ItemTemplate>
                                                                </asp:TemplateColumn>
																<%--	 <asp:TemplateColumn>
                                                                    <ItemTemplate>
																		 <asp:ImageButton ID="ImagebuttonDelete" runat="server" ToolTip="Delete" CommandName="Delete"
                                                                            CausesValidation="False" ImageUrl="~/images/delete.gif" AlternateText="Delete">
                                                                        </asp:ImageButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>--%>
																<asp:BoundColumn DataField="Name" HeaderText="Name"></asp:BoundColumn>
																<asp:BoundColumn DataField="Name2" HeaderText="Name2"></asp:BoundColumn>
																<asp:BoundColumn DataField="TaxID" HeaderText="TaxID"></asp:BoundColumn>
																<asp:BoundColumn DataField="Rel" HeaderText="Rel."></asp:BoundColumn>
																<%--Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and changed header text for birth date--%>
                                                                <%--<asp:BoundColumn DataField="Birthdate" HeaderText="Birthdate"></asp:BoundColumn>--%>
																<asp:BoundColumn DataField="Birthdate" HeaderText="Birth/Estd. Date"></asp:BoundColumn>
																<%--End - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and changed header text for birth date--%>
                                                                <asp:BoundColumn DataField="Groups" HeaderText="Groups"></asp:BoundColumn>
																<asp:BoundColumn DataField="Lvl" HeaderText="Lvl"></asp:BoundColumn>
																<asp:BoundColumn DataField="Pct" HeaderText="Pct"></asp:BoundColumn>
																<asp:BoundColumn DataField="BeneficiaryTypeCode" HeaderText="Type"></asp:BoundColumn>
																<asp:BoundColumn DataField="UniqueId" Visible="false" HeaderText="Type"></asp:BoundColumn>
																<asp:BoundColumn DataField="NewID" Visible="false" HeaderText="Type"></asp:BoundColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>
                                                </td>
                                                <td style="width:10%" >
                                                    <table id="Table2" >
                                                        <tr>
                                                            <td colspan="5">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:Label ID="LabelRetiredDBR" runat="server" CssClass="Label_Small">Retired DB %</asp:Label>
                                                            </td>
                                                            <td colspan="3" align="center">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="LabelPrimaryR" runat="server" CssClass="Label_Small">Primary</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TextBoxPrimaryR" runat="server" Width="50px" Enabled="false" CssClass="TextBox_Normal"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="ButtonPriR" runat="server" Text="E" CssClass="Button_Normal" CausesValidation="False">
                                                                </asp:Button>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="LabelCont1R" runat="server" CssClass="Label_Small">Cont1</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TextBoxCont1R" runat="server" Width="50px" Enabled="false" CssClass="TextBox_Normal"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="ButtonCont1R" runat="server" Text="E" CssClass="Button_Normal" CausesValidation="False">
                                                                </asp:Button>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="LabelCont2R" runat="server" CssClass="Label_Small">Cont2</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TextBoxCont2R" runat="server" Width="50px" Enabled="false" CssClass="TextBox_Normal"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="ButtonCont2R" runat="server" Text="E" CssClass="Button_Normal" CausesValidation="False" Visible="false">
                                                                </asp:Button>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="LabelCont3R" runat="server" CssClass="Label_Small">Cont3</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TextBoxCont3R" runat="server" Width="50px" Enabled="false" CssClass="TextBox_Normal"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="ButtonCont3R" runat="server" Text="E" CssClass="Button_Normal" CausesValidation="False" Visible="false">
                                                                </asp:Button>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" >
                                                    <table id="Table3" width="100%">
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Button ID="ButtonAddRetired" runat="server" Text="Add" Width="50px" CssClass="Button_Normal"
                                                                    CausesValidation="False"></asp:Button>
                                                          
															&nbsp;
                                                                <asp:Button ID="ButtonEditRetired" runat="server" Text="Edit" Width="50px" CssClass="Button_Normal" CausesValidation="False" /> 
                                                           
															&nbsp;
                                                                <asp:Button ID="ButtonDeleteRetired" runat="server" Text="Delete" Width="50px" CssClass="Button_Normal"
                                                                    CausesValidation="False"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </iewc:PageView>
                        <iewc:PageView>
                            <table width="100%" class="Table_WithBorder" height="330" border="0">
								<tr>
									<td align="left" valign="top">
										<%--YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -start --%>
										<table width="100%" border="0">
											<tr>
												<td align="left" class="td_Text" colspan="2">
													Joint and Survivor Beneficiary Information
												</td>
											</tr>
											<tr valign="top">
												<td style="width: 50%">
													<table width="100%" border="0" style="height: 420" class="Table_WithBorder">
														<tr>
															<td align="left" colspan="2">
																<asp:Label ID="LabelRetirementBeneficiary" runat="server" Visible="true" CssClass="Label_Small">Retirement Plan</asp:Label>
															</td>
														</tr>
														<tr>
															<td align="left" colspan="2" valign="top">
																<div runat="server" id="divRetbenef" style="overflow: auto; width: 100%; border-top-style: none;
																	border-right-style: none; border-left-style: none; position: static; height: 100px;
																	border-bottom-style: none">
																	<asp:DataGrid ID="DataGridAnnuityBeneficiaries" runat="server" Width="100%" CssClass="DataGrid_Grid"
																		AutoGenerateColumns="false">
																		<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
																		<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																		<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																		<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
																		<Columns>
																			<asp:TemplateColumn>
																				<ItemTemplate>
																					<asp:ImageButton ID="ImageButtonAccounts" runat="server" ToolTip="Select" CommandName="Select"
																						CausesValidation="False" ImageUrl="images\select.gif" AlternateText="Select">
																					</asp:ImageButton>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:BoundColumn HeaderText="guiUniqueID" DataField="guiUniqueID" Visible="false" />
																			<asp:BoundColumn HeaderText="SSN" DataField="chrBeneficiaryTaxNumber" />
																			<asp:BoundColumn HeaderText="First Name" DataField="BenFirstName" />
																			<asp:BoundColumn HeaderText="Last Name" DataField="BenLastName" />
																			<asp:BoundColumn HeaderText="Birth Date" DataField="BenBirthDate" DataFormatString="{0:MM/dd/yyyy}" />
																			<asp:BoundColumn HeaderText="Rel" DataField="chvRelationshipCode" Visible="false" />
																			<asp:BoundColumn HeaderText="Rel." DataField="Relationship" />
																			<asp:BoundColumn HeaderText="Group" DataField="chvBeneficiaryGroupCode" />
																		</Columns>
																	</asp:DataGrid>
																	<asp:Label ID="LabelNoBeneficiary" Text="No Beneficiary is defined" runat="server"
																		CssClass="Label_Small" Visible="false"></asp:Label>
																</div>
															</td>
														</tr>
														<tr>
															<td align="left" colspan='2'>
																&nbsp;
															</td>
														</tr>
														<tr>
															<td align="left">
																<asp:Label ID="LabelSSNo2Ret" runat="server" Visible="true" CssClass="Label_Small">SS No.</asp:Label>
															</td>
															<td align="left">
																<asp:TextBox ID="TextBoxAnnuitySSNoRet" runat="server" Width="177px" Visible="true"
																	CssClass="TextBox_Normal"></asp:TextBox>
																<asp:RequiredFieldValidator ID="RequiredFieldValidatorAnnuitySSNoRet" runat="server"
																	ErrorMessage="* SSNo cannot be blank" ToolTip="SSNo cannot be blank" Text="*"
																	ControlToValidate="TextBoxAnnuitySSNoRet" Enabled="true" CssClass="Error_Message"></asp:RequiredFieldValidator>
															</td>
														</tr>
														<tr>
															<td align="left">
																<asp:Label ID="LabelLastName2Ret" runat="server" Visible="true" CssClass="Label_Small">Last Name</asp:Label>
															</td>
															<td align="left">
																<asp:TextBox ID="TextBoxAnnuityLastNameRet" runat="server" Width="177" Visible="true"
																	CssClass="TextBox_Normal"></asp:TextBox>
																<asp:RequiredFieldValidator ID="RequiredfieldvalidatorAnnuityLastNameRet" runat="server"
																	ErrorMessage="* Last Name cannot be blank" ToolTip="Last Name cannot be blank"
																	Text="*" ControlToValidate="TextBoxAnnuityLastNameRet" Enabled="true" CssClass="Error_Message"></asp:RequiredFieldValidator>
															</td>
														</tr>
														<tr>
															<td align="left">
																<asp:Label ID="LabelFirstName2Ret" runat="server" Visible="true" CssClass="Label_Small">First Name</asp:Label>
															</td>
															<td align="left">
																<asp:TextBox ID="TextBoxAnnuityFirstNameRet" runat="server" Width="177" Visible="true"
																	CssClass="TextBox_Normal"></asp:TextBox>
																<asp:RequiredFieldValidator ID="RequiredfieldvalidatorAnnuityFirstNameRet" runat="server"
																	ErrorMessage="* First Name cannot be blank" ToolTip="First Name cannot be blank"
																	Text="*" ControlToValidate="TextBoxAnnuityFirstNameRet" Enabled="true" CssClass="Error_Message"></asp:RequiredFieldValidator>
															</td>
														</tr>
														<tr>
															<td align="left" nowrap>
																<asp:Label ID="LabelMiddleName2Ret" runat="server" Visible="true" CssClass="Label_Small">Middle Name</asp:Label>
															</td>
															<td align="left">
																<asp:TextBox ID="TextBoxAnnuityMiddleNameRet" runat="server" Width="177" Visible="true"
																	CssClass="TextBox_Normal"></asp:TextBox>
															</td>
														</tr>
														<tr>
															<td align="left">
																<asp:Label ID="LabelBirthDate2Ret" runat="server" Visible="true" CssClass="Label_Small">Birth Date</asp:Label>
															</td>
															<td align="left">
																<div runat="server" id="spnBoxAnnuityBirthDateRet">
                                                                    <%-- START: PPP | 2015.09.25 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788) --%>
                                                                    <YRSCustomControls:CalenderTextBox ID="TextBoxAnnuityBirthDateRet" runat="server" CssClass="TextBox_Normal"
                                                                        Width="156" MaxLength="10" EnableCustomValidator="true" IsFocusRequired="false" TrackDateChange="true" yearRange="1900:c+100"></YRSCustomControls:CalenderTextBox>
																	<%--<asp:TextBox ID="TextBoxAnnuityBirthDateRet" runat="server" CssClass="TextBox_Normal"
																		Width="156" ReadOnly="True"></asp:TextBox>
																	<rjs:PopCalendar ID="Popcalendar3" runat="server" Separator="/" Control="TextBoxAnnuityBirthDateRet"
																		Format="mm dd yyyy"></rjs:PopCalendar>
																	<asp:RequiredFieldValidator ID="RequiredfieldvalidatorAnnuityBirthDateRet" runat="server"
																		ErrorMessage="* Birth Date cannot be blank" ToolTip="Birth Date cannot be blank"
																		Text="*" ControlToValidate="TextBoxAnnuityBirthDateRet" Enabled="true" CssClass="Error_Message"></asp:RequiredFieldValidator>--%>
                                                                    <%-- END: PPP | 2015.09.25 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788) --%>
																</div>
															</td>
														</tr>
														<tr>
															<td align="left">
																<asp:Label ID="LabelRealtionRet" runat="server" Visible="true" CssClass="Label_Small">Relation</asp:Label>
															</td>
															<td align="left">
																<asp:DropDownList ID="DropDownRelationShipRet" CssClass="DropDown_Normal" Width="174"
																	runat="server" AutoPostBack="true">
																</asp:DropDownList>
																<asp:RequiredFieldValidator ID="RequiredfieldvalidatornRelationShipRet" runat="server"
																	ErrorMessage="*Please select relationship" ToolTip="Please select relationship"
																	Text="*" ControlToValidate="DropDownRelationShipRet" InitialValue="Select" Enabled="true"
																	CssClass="Error_Message"></asp:RequiredFieldValidator>
															</td>
														</tr>
                                                        <tr>
                                                            <td colspan="2" align="right">
                                                                <asp:LinkButton ID="lnkParticipantAddressRet" runat="server" CssClass="Link_Small" Text="Use Participant Address"></asp:LinkButton> 
                                                            </td>
                                                        </tr>
														<tr>
															<td colspan="2">
																<YRSControls:AddressWebUserControl ID="AddressWebUserControlRet" AddressFor="BENE"
																	runat="server" AllowEffDate="false" AllowNote="true" IsFromBenificarySettlement="true"
																	PopupHeight="930" ClientIDMode="Predictable"></YRSControls:AddressWebUserControl>
															</td>
														</tr>
														<tr>
															<td align="right">
															</td>
															<td align="right">
																<asp:Button ID="ButtonClearBeneficiary" runat="server" CausesValidation="false" Text="Clear" CssClass="Button_Normal" />
															</td>
														</tr>
													</table>
												</td>
												<td style="width: 50%">
													<table width="100%" border="0" style="height: 420" class="Table_WithBorder">
														<tr>
															<td align="left" colspan='2'>
																<asp:Label ID="LabelSavingsBenefeciary" runat="server" Visible="true" CssClass="Label_Small">Savings Plan</asp:Label>
															</td>
														</tr>
														<tr>
															<td align="left" colspan='2' valign="top">
																<div runat="server" id="divSavbenef" style="overflow: auto; width: 100%; border-top-style: none;
																	border-right-style: none; border-left-style: none; position: static; height: 100px;
																	border-bottom-style: none">
																	<asp:DataGrid ID="DataGridAnnuityBeneficiariesSav" runat="server" Width="100%" CssClass="DataGrid_Grid"
																		AutoGenerateColumns="false">
																		<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
																		<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																		<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																		<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
																		<Columns>
																			<asp:TemplateColumn>
																				<ItemTemplate>
																					<asp:ImageButton ID="ImageButtonAccounts" runat="server" ToolTip="Select" CommandName="Select"
																						CausesValidation="False" ImageUrl="images\select.gif" AlternateText="Select">
																					</asp:ImageButton>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:BoundColumn HeaderText="guiUniqueID" DataField="guiUniqueID" Visible="false" />
																			<asp:BoundColumn HeaderText="SSN" DataField="chrBeneficiaryTaxNumber" />
																			<asp:BoundColumn HeaderText="First Name" DataField="BenFirstName" />
																			<asp:BoundColumn HeaderText="Last Name" DataField="BenLastName" />
																			<asp:BoundColumn HeaderText="Birth Date" DataField="BenBirthDate" DataFormatString="{0:MM/dd/yyyy}" />
																			<asp:BoundColumn HeaderText="Rel" DataField="chvRelationshipCode" Visible="false" />
																			<asp:BoundColumn HeaderText="Rel." DataField="Relationship" />
																			<asp:BoundColumn HeaderText="Group" DataField="chvBeneficiaryGroupCode" />
																		</Columns>
																	</asp:DataGrid>
																	<asp:Label ID="LabelNoBeneficiarySav" Text="No Beneficiary is defined" runat="server"
																		CssClass="Label_Small" Visible="false"></asp:Label>
																</div>
															</td>
														</tr>
														<tr>
															<td align="left" colspan='2'>
																&nbsp;
															</td>
														</tr>
														<tr>
															<td align="left">
																<asp:Label ID="LabelSSNo2Sav" runat="server" Visible="true" CssClass="Label_Small">SS No.</asp:Label>
															</td>
															<td align="left">
																<asp:TextBox ID="TextBoxAnnuitySSNoSav" runat="server" Width="177" Visible="true"
																	CssClass="TextBox_Normal"></asp:TextBox>
																<asp:RequiredFieldValidator ID="RequiredFieldValidatorAnnuitySSNoSav" runat="server"
																	ErrorMessage="* SSNo cannot be blank" ToolTip="SSNo cannot be blank" Text="*"
																	ControlToValidate="TextBoxAnnuitySSNoSav" Enabled="true" CssClass="Error_Message"></asp:RequiredFieldValidator>
															</td>
														</tr>
														<tr>
															<td align="left">
																<asp:Label ID="LabelLastName2Sav" runat="server" Visible="true" CssClass="Label_Small">Last Name</asp:Label>
															</td>
															<td align="left">
																<asp:TextBox ID="TextBoxAnnuityLastNameSav" runat="server" Width="177" Visible="true"
																	CssClass="TextBox_Normal"></asp:TextBox>
																<asp:RequiredFieldValidator ID="RequiredfieldvalidatorAnnuityLastNameSav" runat="server"
																	ErrorMessage="* Last Name cannot be blank" ToolTip="Last Name cannot be blank"
																	Text="*" ControlToValidate="TextBoxAnnuityLastNameSav" Enabled="true" CssClass="Error_Message"></asp:RequiredFieldValidator>
															</td>
														</tr>
														<tr>
															<td align="left">
																<asp:Label ID="LabelFirstName2Sav" runat="server" Visible="true" CssClass="Label_Small">First Name</asp:Label>
															</td>
															<td align="left">
																<asp:TextBox ID="TextBoxAnnuityFirstNameSav" runat="server" Width="177" Visible="true"
																	CssClass="TextBox_Normal"></asp:TextBox>
																<asp:RequiredFieldValidator ID="RequiredfieldvalidatorAnnuityFirstNameSav" runat="server"
																	ErrorMessage="* First Name cannot be blank" ToolTip="First Name cannot be blank"
																	Text="*" ControlToValidate="TextBoxAnnuityFirstNameSav" Enabled="true" CssClass="Error_Message"></asp:RequiredFieldValidator>
															</td>
														</tr>
														<tr>
															<td align="left" nowrap>
																<asp:Label ID="LabelMiddleName2Sav" runat="server" Visible="true" CssClass="Label_Small">Middle Name</asp:Label>
															</td>
															<td align="left" colspan="2">
																<asp:TextBox ID="TextBoxAnnuityMiddleNameSav" runat="server" Width="177" Visible="true"
																	CssClass="TextBox_Normal"></asp:TextBox>
															</td>
														</tr>
														<tr>
															<td align="left">
																<asp:Label ID="LabelBirthDate2Sav" runat="server" Visible="true" CssClass="Label_Small">Birth Date</asp:Label>
															</td>
															<td align="left">
																<div runat="server" id="spnBoxAnnuityBirthDateSav">
                                                                    <%-- START: PPP | 2015.09.25 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788) --%>
                                                                    <YRSCustomControls:CalenderTextBox ID="TextBoxAnnuityBirthDateSav" runat="server" CssClass="TextBox_Normal"
                                                                        Width="156" MaxLength="10" EnableCustomValidator="true" IsFocusRequired="false" TrackDateChange="true" yearRange="1900:c+100"></YRSCustomControls:CalenderTextBox>
																	<%--<asp:TextBox ID="TextBoxAnnuityBirthDateSav" runat="server" Width="156" ReadOnly="true"
																		CssClass="TextBox_Normal"></asp:TextBox>
																	<rjs:PopCalendar ID="PopcalendarSaving" runat="server" Separator="/" Control="TextBoxAnnuityBirthDateSav"
																		Format="mm dd yyyy"></rjs:PopCalendar>
																	<asp:RequiredFieldValidator ID="RequiredfieldvalidatorAnnuityBirthDateSav" runat="server"
																		ErrorMessage="* Birth Date cannot be blank" ToolTip="Birth Date cannot be blank"
																		Text="*" ControlToValidate="TextBoxAnnuityBirthDateSav" Enabled="true" CssClass="Error_Message"></asp:RequiredFieldValidator>--%>
                                                                    <%-- END: PPP | 2015.09.25 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788) --%>
																</div>
															</td>
														</tr>
														<tr>
															<td align="left">
																<asp:Label ID="LabelRealtionSav" runat="server" Visible="true" CssClass="Label_Small">Relation</asp:Label>
															</td>
															<td align="left">
																<asp:DropDownList ID="DropDownRelationShipSav" CssClass="DropDown_Normal" Width="174"
																	runat="server" AutoPostBack="true">
																</asp:DropDownList>
																<asp:RequiredFieldValidator ID="RequiredfieldvalidatornRelationShipSav" runat="server"
																	ErrorMessage="*Please select relationship" ToolTip="Please select relationship"
																	Text="*" ControlToValidate="DropDownRelationShipSav" InitialValue="Select" Enabled="true"
																	CssClass="Error_Message"></asp:RequiredFieldValidator>
															</td>
														</tr>
                                                        <tr>
                                                            <td colspan="2" align="right">
                                                                <asp:LinkButton ID="lnkParticipantAddressSav" runat="server" CssClass="Link_Small" Text="Use Participant Address"></asp:LinkButton> 
                                                            </td>
                                                        </tr>
														<tr>
															<td colspan="2">
																<YRSControls:AddressWebUserControl ID="AddressWebUserControlSav" AddressFor="BENE"
																	runat="server" AllowEffDate="false" AllowNote="true" IsFromBenificarySettlement="true"
																	PopupHeight="930" ClientIDMode="Predictable"></YRSControls:AddressWebUserControl>
															</td>
														</tr>
														<tr>
															<td align="right">
															</td>
															<td align="right">
																<asp:Button ID="ButtonClearBeneficiarySav" CausesValidation="false" runat="server" Text="Clear" CssClass="Button_Normal" />
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</table>
										<%--YRS 5.0-1508 - Add grid of beneficiaries to select for annuity beneficiary -end --%>
										<%--commented for issue 1508 2012.05.18--%>
										<%--<table width="100%" border="0"> 
                                            <tr>
                                                <td colspan="6" align="left" class="td_Text">
                                                    Joint and Survivor Beneficiary Information
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6" align="left">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="center">
                                                    <asp:Label ID="LabelRetirementBeneficiary" runat="server" Visible="False" CssClass="Label_Small">Retirement Plan</asp:Label>
                                                </td>
                                                <td colspan="3" align="center">
                                                    <asp:Label ID="LabelSavingsBenefeciary" runat="server" Visible="False" CssClass="Label_Small">Savings Plan</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="LabelSSNo2Ret" runat="server" Visible="False" CssClass="Label_Small">SS No.</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextBoxAnnuitySSNoRet" runat="server" Width="177" Visible="False"
                                                        CssClass="TextBox_Normal"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorAnnuitySSNoRet" runat="server"
                                                        ErrorMessage="* SSNo cannot be blank" ToolTip="SSNo cannot be blank" Text="*"
                                                        ControlToValidate="TextBoxAnnuitySSNoRet" Enabled="false" CssClass="Error_Message"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="LabelSSNo2Sav" runat="server" Visible="False" CssClass="Label_Small">SS No.</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextBoxAnnuitySSNoSav" runat="server" Width="177" Visible="False"
                                                        CssClass="TextBox_Normal"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorAnnuitySSNoSav" runat="server"
                                                        ErrorMessage="* SSNo cannot be blank" ToolTip="SSNo cannot be blank" Text="*"
                                                        ControlToValidate="TextBoxAnnuitySSNoSav" Enabled="false" CssClass="Error_Message"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="LabelLastName2Ret" runat="server" Visible="False" CssClass="Label_Small">Last Name</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextBoxAnnuityLastNameRet" runat="server" Width="177" Visible="False"
                                                        CssClass="TextBox_Normal"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:RequiredFieldValidator ID="RequiredfieldvalidatorAnnuityLastNameRet" runat="server"
                                                        ErrorMessage="* Last Name cannot be blank" ToolTip="Last Name cannot be blank"
                                                        Text="*" ControlToValidate="TextBoxAnnuityLastNameRet" Enabled="false" CssClass="Error_Message"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="LabelLastName2Sav" runat="server" Visible="False" CssClass="Label_Small">Last Name</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextBoxAnnuityLastNameSav" runat="server" Width="177" Visible="False"
                                                        CssClass="TextBox_Normal"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:RequiredFieldValidator ID="RequiredfieldvalidatorAnnuityLastNameSav" runat="server"
                                                        ErrorMessage="* Last Name cannot be blank" ToolTip="Last Name cannot be blank"
                                                        Text="*" ControlToValidate="TextBoxAnnuityLastNameSav" Enabled="false" CssClass="Error_Message"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="LabelFirstName2Ret" runat="server" Visible="False" CssClass="Label_Small">First Name</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextBoxAnnuityFirstNameRet" runat="server" Width="177" Visible="False"
                                                        CssClass="TextBox_Normal"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:RequiredFieldValidator ID="RequiredfieldvalidatorAnnuityFirstNameRet" runat="server"
                                                        ErrorMessage="* First Name cannot be blank" ToolTip="First Name cannot be blank"
                                                        Text="*" ControlToValidate="TextBoxAnnuityFirstNameRet" Enabled="false" CssClass="Error_Message"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="LabelFirstName2Sav" runat="server" Visible="False" CssClass="Label_Small">First Name</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextBoxAnnuityFirstNameSav" runat="server" Width="177" Visible="False"
                                                        CssClass="TextBox_Normal"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:RequiredFieldValidator ID="RequiredfieldvalidatorAnnuityFirstNameSav" runat="server"
                                                        ErrorMessage="* First Name cannot be blank" ToolTip="First Name cannot be blank"
                                                        Text="*" ControlToValidate="TextBoxAnnuityFirstNameSav" Enabled="false" CssClass="Error_Message"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" nowrap>
                                                    <asp:Label ID="LabelMiddleName2Ret" runat="server" Visible="False" CssClass="Label_Small">Middle Name</asp:Label>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="TextBoxAnnuityMiddleNameRet" runat="server" Width="177" Visible="False"
                                                        CssClass="TextBox_Normal"></asp:TextBox>
                                                </td>
                                                <td align="left" nowrap>
                                                    <asp:Label ID="LabelMiddleName2Sav" runat="server" Visible="False" CssClass="Label_Small">Middle Name</asp:Label>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="TextBoxAnnuityMiddleNameSav" runat="server" Width="177" Visible="False"
                                                        CssClass="TextBox_Normal"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="LabelBirthDate2Ret" runat="server" Visible="False" CssClass="Label_Small">Birth Date</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextBoxAnnuityBirthDateRet" runat="server" Width="177" Visible="False"
                                                        CssClass="TextBox_Normal"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:RequiredFieldValidator ID="RequiredfieldvalidatorAnnuityBirthDateRet" runat="server"
                                                        ErrorMessage="* Birth Date cannot be blank" ToolTip="Birth Date cannot be blank"
                                                        Text="*" ControlToValidate="TextBoxAnnuityBirthDateRet" Enabled="false" CssClass="Error_Message"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="LabelBirthDate2Sav" runat="server" Visible="False" CssClass="Label_Small">Birth Date</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextBoxAnnuityBirthDateSav" runat="server" Width="177" Visible="False"
                                                        CssClass="TextBox_Normal"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:RequiredFieldValidator ID="RequiredfieldvalidatorAnnuityBirthDateSav" runat="server"
                                                        ErrorMessage="* Birth Date cannot be blank" ToolTip="Birth Date cannot be blank"
                                                        Text="*" ControlToValidate="TextBoxAnnuityBirthDateSav" Enabled="false" CssClass="Error_Message"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="LabelSpouseRet" runat="server" Visible="False" CssClass="Label_Small">Spouse</asp:Label>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:CheckBox ID="chkSpouseRet" runat="server" Visible="False" CssClass="CheckBox_Normal">
                                                    </asp:CheckBox>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="LabelSpouseSav" runat="server" Visible="False" CssClass="Label_Small">Spouse</asp:Label>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:CheckBox ID="chkSpouseSav" runat="server" Visible="False" CssClass="CheckBox_Normal">
                                                    </asp:CheckBox>
                                                </td>
                                            </tr>
                                        </table>--%>
									</td>
								</tr>
                            </table>
                        </iewc:PageView>
                        <iewc:PageView>
                            <table width="100%" class="Table_WithBorder"  style="height:400px" cellspacing="0">
                                <tr>
                                    <td align="left" class="td_Text">
                                        Notes
                                    </td>
                                    <td align="right" class="Td_ButtonContainer">
                                        <asp:Button ID="ButtonAddItemNotes" runat="server" Text="Add..." CssClass="Button_Normal"
                                            CausesValidation="False"></asp:Button>
                                        <asp:Button ID="ButtonView" runat="server" Text="View Item" Visible="false" CssClass="Button_Normal"
                                            Enabled="false" CausesValidation="False"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" valign="top">
                                        <div style="overflow: auto; width: 100%; height: 400px; text-align: left">
                                            <asp:DataGrid ID="DataGridNotes" runat="server" Width="100%" CssClass="DataGrid_Grid"
                                                AutoGenerateColumns="false">
                                                <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                <Columns>
                                                    <asp:TemplateColumn HeaderStyle-Width="2%">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImageButtonNotes" runat="server" ToolTip="View" CommandName="Select"
                                                                CausesValidation="False" ImageUrl="~/images/view.gif" AlternateText="View">
                                                            </asp:ImageButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn HeaderText="UniqueId" DataField="UniqueId" Visible="false" />
                                                    <asp:BoundColumn HeaderText="PersonId" DataField="PersonId" Visible="false" />
                                                    <asp:BoundColumn HeaderText="NoteType" DataField="NoteType" Visible="false" />
                                                    <asp:BoundColumn HeaderText="Date" DataField="Date" HeaderStyle-Width="23%" />
                                                    <asp:BoundColumn HeaderText="Creator" DataField="Creator"  HeaderStyle-Width="10%" />
                                                    <asp:BoundColumn HeaderText="Note" DataField="Note"  HeaderStyle-Width="55%" />
                                                    <asp:TemplateColumn HeaderText="Mark As Important"  HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <%-- Start: SB: 03/15/2019: BT-12078 : Disable bitImported checkbox--%>
                                                            <%--<asp:CheckBox ID="CheckBoxImportant" runat="server" AutoPostBack="True" OnCheckedChanged="Check_Clicked"
                                                                Enabled="False" Checked='<%# Databinder.Eval(Container.DataItem, "bitImportant") %>'>--%>
                                                             <asp:CheckBox ID="CheckBoxImportant" runat="server" 
                                                                Enabled="False" Checked='<%# Databinder.Eval(Container.DataItem, "bitImportant") %>'>
                                                             <%-- End: SB: 03/15/2019: BT-12078 : Disable bitImported checkbox--%>
                                                            </asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </iewc:PageView>
                        <iewc:PageView>
                            <table class="Table_WithBorder" width="100%" style="height:400px">
                                <tr>
                                    <td valign="top">
                                        <table class="Table_WithOutBorder" width="100%" border="0">
                                            <tr>
                                                <td colspan="4" class="td_Text">
                                                    Purchase
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="center">
                                                    <asp:Panel ID="pnlAllowAnnuityInSummaryTab" Visible="false" runat="server">
                                                        <table class="Table_WithOutBorder" width="90%" border="0">
                                                            <tr>
                                                                <td >
                                                                    <asp:Label ID="lblSummarySelectedAnnuityRet" runat="server"></asp:Label>
                                                                </td>
                                                                <td >
                                                                    <asp:Label ID="lblSummarySelectedAnnuitySav" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td  align="left">
                                                                    <asp:RadioButtonList ID="rdbListAnnuityOptionRet" runat="server" RepeatDirection="Vertical"
                                                                        AutoPostBack="true" CssClass="DataGrid_NormalStyle">
                                                                        <asp:ListItem Value="OLD"></asp:ListItem>
                                                                        <asp:ListItem Value="NEW"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:RadioButtonList ID="rdbListAnnuityOptionSav" runat="server" RepeatDirection="Vertical"
                                                                        AutoPostBack="true" CssClass="DataGrid_NormalStyle">
                                                                        <asp:ListItem Value="OLD"></asp:ListItem>
                                                                        <asp:ListItem Value="NEW"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="center">
												    
                                                    <asp:DataGrid ID="DatagridPurchase" runat="server" Width="100%" ShowFooter="True"
                                                        CssClass="DataGrid_Grid" AutoGenerateColumns="False">
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                        <FooterStyle CssClass="DataGrid_HeaderStyle"></FooterStyle>
                                                        <Columns> <%--2012.08.01:SR: BT-753/YRS 5.0-1270 : Columng width and formatting changed --%>
                                                            <asp:BoundColumn ItemStyle-Width="245" HeaderText="Source" DataField="Source" FooterText="Total"
                                                                FooterStyle-Font-Bold="True" FooterStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-Width="245" HeaderStyle-HorizontalAlign="Left" />
														    <asp:BoundColumn ItemStyle-Width="145" HeaderText="Option" DataField="Option" 
                                                                FooterStyle-Font-Bold="True" FooterStyle-HorizontalAlign="Center" 
																ItemStyle-HorizontalAlign="Center" FooterStyle-Width="145" HeaderStyle-HorizontalAlign="Center" />
                                                            <asp:BoundColumn ItemStyle-Width="135" HeaderText="Monthly Payment" DataField="MP"
                                                                FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" FooterStyle-Width="135" FooterStyle-HorizontalAlign="Right"
                                                                HeaderStyle-HorizontalAlign="Center" />
                                                            <asp:BoundColumn ItemStyle-Width="135" HeaderText="Regular (First Check)" DataField="RFC"
                                                                FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" FooterStyle-Width="135" FooterStyle-HorizontalAlign="Right"
                                                                HeaderStyle-HorizontalAlign="Center" />
                                                            <asp:BoundColumn ItemStyle-Width="135" HeaderText="Dividend (First Check)" DataField="DFC"
                                                                FooterStyle-Font-Bold="True" ItemStyle-HorizontalAlign="Right" FooterStyle-Width="135" FooterStyle-HorizontalAlign="Right"
                                                                HeaderStyle-HorizontalAlign="Center" />
                                                        </Columns>
                                                    </asp:DataGrid>
                                                    <asp:DataGrid ID="DatagridWithheld" runat="server" Width="100%" ShowHeader="False"
                                                        ShowFooter="True" CssClass="DataGrid_Grid" AutoGenerateColumns="False">
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                        <FooterStyle CssClass="DataGrid_HeaderStyle"></FooterStyle>
                                                        <Columns> <%--2012.07.13	SP: BT-753/YRS 5.0-1270 : purchase page -start (adding FooterStyle-Width property to each bound column--%>
                                                            <asp:BoundColumn ItemStyle-Width="392" DataField="Source" FooterText="Net*" FooterStyle-Font-Bold="True"
                                                                ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left"  FooterStyle-Width="392" HeaderStyle-HorizontalAlign="left" /><%-- ML |20.09.2019| YRS-AT-4597 |Asteric added   --%>
                                                            <asp:BoundColumn ItemStyle-Width="135" DataField="MP" FooterStyle-Font-Bold="True"
                                                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Width="135"/>
                                                            <asp:BoundColumn ItemStyle-Width="135" DataField="RFC" FooterStyle-Font-Bold="True"
                                                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Width="135"/>
                                                            <asp:BoundColumn ItemStyle-Width="135" DataField="DFC" FooterStyle-Font-Bold="True"
                                                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Width="135"/> <%--2012.07.13	SP: BT-753/YRS 5.0-1270 : purchase page -end--%>
                                                                <%--End-2012.08.01:SR: BT-753/YRS 5.0-1270 : Columng width and formatting changed --%>
                                                        </Columns>
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                             <%--START : ML |20.09.2019| YRS-AT-4597 |Alert Message for State Withholding Deducation --%>
                                            <tr>
                                                <td colspan="4">                                                   
                                                     <asp:Label ID="lblStateWithholdingMessage" runat="server" CssClass="Label_Small"> </asp:Label> 
                                                </td>
                                            </tr>
                                            <%--END : ML |20.09.2019| YRS-AT-4597 |Alert Message for State Withholding Deducation --%>
                                            <tr>
                                                <td colspan="4">
                                            </tr>

                                            <tr>
                                                <td align="left" colspan="4">
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="LabelNoOfMonthsInFirstCheck" runat="server" CssClass="Label_Small">No. of months in first check:</asp:Label>
                                                    <asp:Label ID="LabelNoOfMonthsInFirstCheckValue" runat="server" CssClass="Label_Small"></asp:Label>
                                                </td>
                                             </tr>
                                          
                                            
                                            
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LabelMonthlyCheck" Visible="False" runat="server" CssClass="Label_Small">Monthly Check</asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="LabelAnnuity2" Visible="False" runat="server" CssClass="Label_Small">Annuity</asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="LabelDeathBenefit" Visible="False" runat="server" CssClass="Label_Small">Death Benefit</asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="LabelTotal" Visible="False" runat="server" CssClass="Label_Small">Total</asp:Label>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="LabelGross" Visible="False" runat="server" CssClass="Label_Small">Gross</asp:Label>
                                                    <asp:TextBox ID="TextBoxMonthlyGrossAnnuity" Visible="False" runat="server" Width="120"
                                                        CssClass="TextBox_Normal"></asp:TextBox>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="LabelMonthlyGrossDB" Visible="False" runat="server"></asp:Label>
                                                    <asp:TextBox ID="TextBoxMonthlyGrossDB" Visible="False" runat="server" Width="120"
                                                        CssClass="TextBox_Normal"></asp:TextBox>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="LabelMonthlyGrossTotal" Visible="False" runat="server"></asp:Label>
                                                    <asp:TextBox ID="TextBoxMonthlyGrossTotal" Visible="False" runat="server" Width="120"
                                                        CssClass="TextBox_Normal"></asp:TextBox>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="LabelWithheld" Visible="False" runat="server" CssClass="Label_Small">Withheld</asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="TextBoxMonthlyWithheld" Visible="False" runat="server" Width="120"
                                                        CssClass="TextBox_Normal"></asp:TextBox>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="LabelNet" Visible="False" runat="server" CssClass="Label_Small">Net</asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="TextBoxMonthlyNetTotal" Visible="False" runat="server" Width="120"
                                                        CssClass="TextBox_Normal"></asp:TextBox>&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table class="Table_WithOutBorder" width="100%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="LabelFirstCheck" Visible="False" runat="server" CssClass="Label_Small">First Check</asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="LabelAnnuity3" Visible="False" runat="server" CssClass="Label_Small">Annuity</asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="LabelDeathBenefit2" Visible="False" runat="server" CssClass="Label_Small">Death Benefit</asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="LabelTotal2" Visible="False" runat="server" CssClass="Label_Small">Total</asp:Label>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="LabelGross2" runat="server" Visible="False" CssClass="Label_Small">Gross</asp:Label>
                                                    <asp:TextBox ID="TextBoxFirstCheckGrossAnnuity" Visible="False" runat="server" Width="120"
                                                        CssClass="TextBox_Normal"></asp:TextBox>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="LabelFirstCheckGrossDB" Visible="False" runat="server"></asp:Label>
                                                    <asp:TextBox ID="TextBoxFirstCheckGrossDB" Visible="False" runat="server" Width="120"
                                                        CssClass="TextBox_Normal"></asp:TextBox>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="LabelFirstCheckGrossTotal" Visible="False" runat="server"></asp:Label>
                                                    <asp:TextBox ID="TextBoxFirstCheckGrossTotal" Visible="False" runat="server" Width="120"
                                                        CssClass="TextBox_Normal"></asp:TextBox>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="LabelWithheld2" Visible="False" runat="server" CssClass="Label_Small">Withheld</asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="TextBoxFirstCheckWithheld" Visible="False" runat="server" Width="120"
                                                        CssClass="TextBox_Normal"></asp:TextBox>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="LabelNet2" Visible="False" runat="server" CssClass="Label_Small">Net</asp:Label>&nbsp;
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="TextBoxFirstCheckNet" Visible="False" runat="server" Width="120"
                                                        CssClass="TextBox_Normal"></asp:TextBox>&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    
                                    <td align="right" class="Td_ButtonContainer">
											<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
												<asp:Button ID="ButtonPurchase" Text="Purchase" runat="server" CssClass="Button_Normal">
												</asp:Button>
												</ContentTemplate>
											<Triggers>
            										<asp:AsyncPostBackTrigger ControlID="ButtonPurchase" EventName="Click" />
													<asp:AsyncPostBackTrigger ControlID="ButtonCancel" EventName="Click" />
													<asp:AsyncPostBackTrigger ControlID="ButtonYes"  EventName="Click"/>													
											</Triggers>
										</asp:UpdatePanel>
                                    </td>
                                            
                                </tr>
                            </table>
                        </iewc:PageView>
                    </iewc:MultiPage>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <table class="Table_WithBorder" width="100%" cellspacing="0">
                        <tr>
                            <td class="Td_ButtonContainer" align="left">
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="ButtonReCalculate" runat="server" CausesValidation="true" CssClass="Button_Normal" Text="Re-Calculate" OnClientClick="javascript:return ShowTransactionDialog('2');"> <%-- MMR | 2017.03.06 | YRS-AT-2625 | Added javascript fucntion on click event to open dialog--%>
                                </asp:Button>
                            </td>
                            <td class="Td_ButtonContainer" align="right">
                            </td>
                            <td class="Td_ButtonContainer" align="right">
							<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
							<ContentTemplate>
                                <asp:Button ID="ButtonCancel" runat="server" CssClass="Button_Normal" Text="Close"
                                    CausesValidation="False"></asp:Button><asp:Button ID="ButtonFormOK" runat="server"
                                        Width="70px" CssClass="Button_Normal" Text="OK" Visible="False"></asp:Button>
										</ContentTemplate>
										<Triggers>
            
											<asp:AsyncPostBackTrigger ControlID="ButtonPurchase" EventName="Click" />
											<asp:AsyncPostBackTrigger ControlID="ButtonCancel" EventName="Click" />
											<asp:PostBackTrigger ControlID="ButtonCancelYes" />
										</Triggers>
										</asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </table>
    </div>
      <div id="divWSMessage"  runat="server" style="display: none;">
    <table width="690px">
        <tr>
            <td valign="top" align="left">
                <span id="spntext" ></span>
            </td>
                                                       
        </tr>
    </table>
    </div>
    <div id="Tooltip" runat="server" style="z-index: 1000; width: auto; border-left: 1px solid silver;
            border-top: 1px solid silver; border-right: 2px solid black; border-bottom: 1px solid #cccccc;
            padding: 3px; position: absolute; top: 0; left: 0; background: LightYellow; color: black;
            display: none; /* does not work in ie6 */	font-size: 7pt; font-family: verdana;
            margin: 0; overflow: visible; text-align:left;">
            <asp:Label runat="server" ID="lblComments" Style="display: block; width: auto; overflow: visible;
            font-size: x-small;"></asp:Label>
        </div>
    <input id="NotesFlag" type="hidden" name="NotesFlag" runat="server"/>
	


<div id="ConfirmDialog" style="display:none;">
    <asp:UpdatePanel ID="upConfirmation" runat="server" UpdateMode="Conditional">
        <ContentTemplate>          
            <div>
                <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                    <tr>
                        <td>
                            <asp:Label ID="lblMessage" CssClass="Label_Small" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <img title="image" height="20" alt="image" src="images/spacer.gif" width="10" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="bottom">
                            <asp:Button runat="server" ID="ButtonYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px;
                                color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                height: 16pt;" />&nbsp;                           
							<asp:Button runat="server" ID="ButtonCancelYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px;
								color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
								height: 16pt;" CausesValidation="false" />&nbsp;
                            <input type="button" ID="btnNo" value="No" class="Button_Normal" onclick="CloseDialog();"
                                Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt;
                                font-weight: bold; height: 16pt;" />
                            <%--START: MMR | 2017.03.16 | YRS-AT-2625 | Defined asp button to allow page postback --%>
                            <asp:Button runat="server" ID="ButtonNo" Text="No" CssClass="Button_Normal" Style="width: 80px;
                                color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                height: 16pt;"  OnClientClick="javascript:setHiddenfieldValue();"/>&nbsp;
                            <%--END: MMR | 2017.03.16 | YRS-AT-2625 | Defined asp button to allow page postback --%>
                        </td>
                    </tr>
                </table>
            <asp:HiddenField runat="server" ID ="hdnMessage"/> <%-- MMR | 2017.03.17 | YRS-AT-2625 | Added hidden field for set retirement date on click of no button in backdated disability message box--%>                
            </div>
        </ContentTemplate>
        <Triggers>
            				<asp:AsyncPostBackTrigger ControlID="ButtonPurchase" EventName="Click" />
											<asp:AsyncPostBackTrigger ControlID="ButtonCancel" EventName="Click" />
											<asp:AsyncPostBackTrigger ControlID="ButtonYes"  EventName="Click"/>
                                            <asp:PostBackTrigger ControlID="ButtonNo"/> <%-- MMR | 2017.03.17 | YRS-AT-2625 | Added postback trigger event to allow page back to set original retirement date--%>
											
        </Triggers>
    </asp:UpdatePanel>
    </div>
        <%--START: MMR | 2017.02.22 | YRS-AT-2625 | Added Grid to display manual transaction list--%>
        <div id="divTransactionList" runat="server" style="display:none;overflow:auto;" >
            <div>
            <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                     <td class="Label_Small">The following manual transactions exist.<br />Please select the ones which are to be considered for computing average salary prior to retirement.This average is used to project contributions and interest through age 60. <%--MMR | 2017.03.16 | YRS-AT-2625 | Removed <BR> tag to display message in line--%>
                    </td>
                </tr>
                <tr><td height="10px"></td></tr> <%--MMR | 2017.03.16 | YRS-AT-2625 | Added for space between text and grid--%>
                <tr>
                    <td>
                        <div style="overflow:auto;height: 150px">
                            <asp:DataGrid ID="DatagridManualTransactionList" runat="server" Visible="True" Width="95%" AutoGenerateColumns="false">
                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                            <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                            <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                <Columns>
                                    <asp:TemplateColumn ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAccountTypeHeader" runat="server" Checked="true">
                                            </asp:CheckBox>
                                        </HeaderTemplate>
                                        <ItemTemplate >
                                            <asp:CheckBox ID="chkAccountTypeRow" runat="server" Checked='<%#Eval("Selected")%>'>
                                            </asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="UniqueId" ReadOnly="True" HeaderText="UniqueID" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn"/>
                                    <asp:BoundColumn DataField="AccountType" ReadOnly="True" HeaderText="Account Type" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center"/>
                                    <asp:BoundColumn DataField="TransactType" ReadOnly="True" HeaderText="Transact Type" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" Visible="false"/>
                                    <asp:BoundColumn DataField="MonthComp" ReadOnly="True" HeaderText="Month Comp." ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}" HeaderStyle-HorizontalAlign="Center"/>                                                                       
                                    <asp:BoundColumn DataField="PersonalPreTax" ReadOnly="True" HeaderText="Personal Pre Tax" ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}" HeaderStyle-HorizontalAlign="Center"/>
                                    <asp:BoundColumn DataField="PersonalPostTax" ReadOnly="True" HeaderText="Personal Post Tax" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}" HeaderStyle-HorizontalAlign="Center"/>
                                    <asp:BoundColumn DataField="YmcaPreTax" ReadOnly="True" HeaderText="Ymca Pre Tax" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}" HeaderStyle-HorizontalAlign="Center"/>
                                    <asp:BoundColumn DataField="TransactDate" ReadOnly="True" HeaderText="Transact Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center"/>
                                    <asp:BoundColumn DataField="ReceivedDate" ReadOnly="True" HeaderText="Received Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center"/>
                                    <asp:BoundColumn DataField="FundedDate" ReadOnly="True" HeaderText="Funded Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center"/>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                  </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;">
                        <input type="button" name="btnTransactionListOk" value="  OK  " class="Button_Normal" onclick="getSelectedManualTransaction();" />&nbsp;
                        <input type="button" name="btnTransactionListCancel" value="Cancel" class="Button_Normal" onclick="CloseTransactionDialog()" />
                    </td>
                </tr>
            </table>
        </div>
            <asp:HiddenField runat="server" ID ="hdnManualTransaction" Value="1"/> <%-- MMR | 2017.03.01 | YRS-AT-2625 | Added hidden field for manual Transaction link--%>
            <asp:HiddenField runat="server" ID ="hdnSourceManualTransaction" Value="1" /> <%-- MMR | 2017.03.01 | YRS-AT-2625 | Added hidden field for manual Transaction link--%> 
        </div>
    <%--END: MMR | 2017.03.03 | YRS-AT-2625 | Added Grid to display manual transaction list--%>

    <%--START: MMR | 2017.03.03 | YRS-AT-2625 |Added function to open manual transaction dialog--%>
        <script language="javascript" type="text/javascript">
            function ShowTransactionDialog(source) {
                var isOpen = true;
                $('#<%=hdnSourceManualTransaction.ClientID%>').val(source);
                $(document).ready(function () {
                    $("#divTransactionList").dialog("option", "title", "Manage Manual Transactions");
                    //source "1" indicates dialog is opened through link
                    if (source == 1) {
                        $('#divTransactionList').dialog("open");
                        //Get number of checkboxes in list either checked or not checked
                        var totalCheckboxes = $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeRow']:checkbox").size();
                        //Get number of checked checkboxes in list
                        var checkedCheckboxes = $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeRow']:checkbox:checked").size();
                        //check and uncheck the header checkbox on the basis of difference of both values
                        $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeHeader']:checkbox").attr('checked', totalCheckboxes == checkedCheckboxes);
                    }
                        //source "2" indicates dialog is opened through Recalculate button
                    else if (source == 2) {
                        // Hidden field value 2 indicates manual transaction exists but not selected
                        if ($('#<%=hdnManualTransaction.ClientID%>').val() == "2" && $('#CheckBoxRetPlan').is(':checked')) { //Added validation dialog should not open if only savings plan selected
                            $('#divTransactionList').dialog("open");
                            isOpen = false;
                        }
                    }
                });

                return isOpen;
            }

            <%--START: MMR | 2017.03.03 | YRS-AT-2625 | Added to show pop-up on load--%>
            function showDialog(id, text, btnokvisibility) {               
                $('#' + id).dialog({ modal: true });
                //Start: AA:03.16.2016 YRS-AT-2599 Added below code to show confirmation dailog
                if (id = 'ConfirmDialog') {
                    if (text.indexOf('##') != -1) {
                        var arrtext = text.split("##");
                        var finaltext = arrtext[arrtext.length - 1] + '<ul>';//Heading title
                        for (var i = 0; i < arrtext.length - 2; i++) {
                            finaltext += '<li>' + arrtext[i] + '</li>';//Points
                        }
                        finaltext += '</ul><br/>' + arrtext[arrtext.length - 2];//confirmation message
                        $('#lblMessage').html(finaltext);
                        var divheight = 150 + (arrtext.length * 100);// PP:03.22.2016 YRS-AT-2599 Changed the height of dialog box 
                        var divWidth = 600;
                        $('#' + id).dialog({ width: divWidth, height: divheight });
                    }
                    else {
                        $('#lblMessage').text(text);
                        $('#' + id).dialog({ width: 480, height: 275 });// Changed the height of dialog box to '275' from '250'
                    }
                }
                //End: AA:03.16.2016 YRS-AT-2599 Added below code to show confirmation dailog
                $('#' + id).dialog("open");
                //SP 2014.02.20 BT-2436 -Start
                if (btnokvisibility == "Cancel") {
                    $("#ButtonYes").hide();
                    $("#ButtonNo").hide();

                    $("#ButtonCancelYes").show();
                    $("#btnNo").show();
                }
                else {
                    $("#ButtonCancelYes").hide();
                    $("#btnNo").hide();

                    $("#ButtonYes").show();
                    $("#ButtonNo").show();                    
                }
                //SP 2014.02.20 BT-2436 -End
                
             }
            <%--END: MMR | 2017.03.03 | YRS-AT-2625 | Added to show pop-up on load--%>     
        </script>
	<%--END: MMR | 2017.03.03 | YRS-AT-2625 |Added function to open manual transaction dialog--%>
	</asp:Content>


