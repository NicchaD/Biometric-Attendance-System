<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RefundProcessing.aspx.vb" Inherits="YMCAUI.RefundProcessing" %>
<%@ Register TagPrefix="NewYRSControls" TagName="AddressWebUserControl_new" Src="~/UserControls/AddressUserControl_new.ascx" %>
<%@ Register TagPrefix="uc2" TagName="RollUserControl" Src="~/UserControls/RolloverInstitution.ascx" %>
<!--#include virtual="TopNew.htm"-->
<script type="text/javascript" src="JS/jquery-1.5.1.min.js"></script>
<script src="JS/jquery-ui/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
<link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
<script language="javascript">
		var objRequestAmount = document.all("TextboxRequestAmount")
		
		if (objRequestAmount){document.all("TextboxRequestAmount").focus();}

		function TextboxRequestAmount_Change()
		{			
			if (objRequestAmount)
			{
				if(document.all("TextboxRequestAmount").value =="" )
				{
					
					document.all("TextboxRequestAmount").focus();
				
				}
				else
				{
					if(!isNaN( document.all("TextboxRequestAmount").value))
					{
						if( document.all("TextboxRequestAmount").value > 0)
						{						
							return;
						}								
					}
					document.all("TextboxRequestAmount").focus();
				}
			}
		}

/*
Function to detect the Browser type.
*/
function detectBrowser()
{
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
function FormatAmtControl(ctl){
	var vMask ;
	var vDecimalAfterPeriod ;
	var ctlVal;
	var iPeriodPos;
	var sTemp;
	var iMaxLen 
	var ctlVal;
	var tempVal;
	ctlVal = ctl.value;
	vDecimalAfterPeriod  = 2
	iMaxLen  = ctl.maxLength;

	if (isNaN(ctlVal))
	{//uncommented by Swopna 6 Mar,2008 against bug-id 263
		// clear the control as this is not a num
		//ctl.value=""
		ctl.value=""
	}
	else{
		ctlVal =  ctl.value;
		iPeriodPos =ctlVal.indexOf(".");
		if (iPeriodPos<0)
		{
			if (ctl.value.length > (iMaxLen-3))
			{
				sTemp = ctl.value
				 tempVal = sTemp.substr(0,(iMaxLen-3)) + ".00";
			}
			else
			tempVal = ctlVal + ".00"
		}
		
		else{
			if ((ctlVal.length - iPeriodPos -1)==1)
				tempVal = ctlVal + "0"
			if ((ctlVal.length - iPeriodPos -1)==0)
				tempVal = ctlVal + "00"
			if ((ctlVal.length - iPeriodPos -1)==2)
				tempVal = ctlVal;
			if ((ctlVal.length - iPeriodPos -1)>2){
				tempVal = ctlVal.substring(0,iPeriodPos+3);
					
			}



		}
		ctl.value=tempVal;
	}
}

/*
This function is responsible for filtering the keys pressed and the maintain the amount format of the 
value in the Text box
*/
	function HandleAmountFiltering(ctl){
	var iKeyCode, objInput;
	var iMaxLen 
	//var reValidChars = /[0-9],./;
	var reValidChars = "0123456789."
	//var reValidChars = /^\d*(\.\d+)?$/;
	var strKey;
	var sValue;
	var event = window.event || arguments.callee.caller.arguments[0];
	iMaxLen  = ctl.maxLength;
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
	
	if(reValidChars.indexOf(strKey)!=-1)
	{
		
		if(iKeyCode==46)
		{
			if(objInput.value.indexOf('.')!=-1)
				if (isIE)
					event.keyCode= 0;
				 else
				 {
				 	 if(event.which!=0 && event.which!=8)
					return false;
				 }
		}
		else
		{
			if(objInput.value.indexOf('.')==-1)
			{
				
				if (objInput.value.length>=(iMaxLen-3))
				{
					if (isIE)
						event.keyCode= 0;
					 else
					 {
					 	 if(event.which!=0 && event.which!=8)
						return false;
					 }
	
				}
			}
			if ((objInput.value.length==(iMaxLen-3)) && (objInput.value.indexOf('.')==-1))
			{
				objInput.value = objInput.value +'.';			
			}	
		}
	}
	else{
		if (isIE)
			event.keyCode= 0;
		 else
		 {
		 	 if(event.which!=0 && event.which!=8)
			 return false;
		 }
	}
if (((event.keyCode < 48)||(event.keyCode > 57)) && event.keyCode != 46)
	{
		event.returnValue = false;
	}
}


function ValidateNumeric()
{
	if (((event.keyCode < 48)||(event.keyCode > 57)) && event.keyCode != 46)
	{
		event.returnValue = false;
	}
}
// ''IB:2011.08.19 - To implement rollover functionality through JQuery'


function HandleRolloverAmount(varFlag) {
    $("#TextboxRolloverAmount").attr('disabled', varFlag);
}

//BT:959- No Validation on refund processing when roll over all selected for MRD refund.
function CheckAllowRollover() {

    if ($("#TextboxNet").val() <= 0 && $("#TextboxNet2").val() <= 0) {
        alert('cannot allow rollover of this amount');
        return false;
    }
    return true;
}


$(document).ready(function () {
    $('#RadioButtonSpecificAmount').click(function () {
        HandleRolloverAmount(false);
    });

    $("#CheckBoxRollovers").click(function ()
    { return CheckAllowRollover(); });
   
});

</script>
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="100%">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					<asp:label id="LabelTitle" runat="server" cssClass="td_Text_Small"></asp:label></td>
			</tr>			
		</table>
	</div>
    <div id="DivMainMessage" class="warning-msg" runat="server" style="text-align: left;" enableviewstate="true" height="10">
         
        </div>
	<table class="Table_WithBorder" width="698">
		<TBODY>
			<tr>
				<td><iewc:tabstrip id="TabStripVoluntaryRefund" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
						TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
						Width="700px" height="30px">
						<iewc:Tab Text=" Account Information "></iewc:Tab>
						<iewc:Tab Text=" Distribution "></iewc:Tab>
					</iewc:tabstrip></td>
			<tr>
				<td><iewc:multipage id="MultiPageVoluntaryRefund" runat="server">
						<iewc:PageView>
							<table class="Table_WithoutBorder" width="100%">
								<tr>
									<td></td>
								</tr>
								<tr>
									<td class="Td_InLineBorder" align="left" height="0"></td>
								</tr>
								<tr>
									<td align="center">
										<table width="630" align="center" border="0">
											<tr>
												<td align="left" nowrap>
													<asp:Label id="LabelPlanChosen" runat="server" CssClass="Label_Small">PlanChosen </asp:Label>
												</td>
												<td align="left" nowrap>
													<asp:TextBox id="TextboxPlanChosen" runat="server" CssClass="TextBox_Normal" Width="140" readonly="true"></asp:TextBox>
												</td>
												<td align="left" nowrap>
													<!-- below line of text is commented by neeraj 08-Sep-09 Issue id YRS 5.0-821 -->
													<asp:Label id="LabelTerminationPIA" runat="server" CssClass="Label_Small"><!-- YMCA (Legacy) Acct at Termination -->&nbsp;&nbsp;&nbsp;&nbsp;YMCA Acct (Legacy) at Termination</asp:Label>&nbsp;&nbsp;&nbsp;
												</td>
												<td align="left" nowrap><asp:TextBox id="TextBoxTerminationPIA" runat="server" CssClass="TextBox_Normal" Width="75"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td align="left" nowrap>
													<asp:Label id="LabelStatus" runat="server" CssClass="Label_Small">Status </asp:Label>
												</td>
												<td align="left" nowrap><asp:TextBox id="TextBoxStatus" runat="server" CssClass="TextBox_Normal" Width="140"></asp:TextBox>
												</td>
												<!-- commented by neeraj 08-Sep-09 Issue id YRS 5.0-821 -->
												<!--<td align="center" nowrap>&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:Label id="LabelCurrentPIA" runat="server" CssClass="Label_Small" visible="false">Current YMCA (Legacy) Acct</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:TextBox id="TextBoxCurrentPIA" runat="server" CssClass="TextBox_Normal" Width="75"></asp:TextBox>
												</td>-->
												<td align="left" nowrap>&nbsp;&nbsp;
													<asp:Label id="Label6" runat="server" CssClass="Label_Small">YMCA Acct at Request</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
												</td>
												<td align="left" nowrap><asp:TextBox id="TextboxCurrentBA" runat="server" CssClass="TextBox_Normal" Width="75"></asp:TextBox>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
								<tr>
									<td class="Td_InLineBorder" align="left" height="0"></td>
								</tr>
								<TR>
									<TD align="left" class="td_Text_Small" colspan="9">
										&nbsp;Requested Accounts
									</TD>
								<tr>
									<td colspan="9" align="right">
										<DIV style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 140px">
											<asp:DataGrid id="DataGridRequestedAccts" runat="server" Width="690px" CssClass="DataGrid_Grid">
												<HeaderStyle cssclass="DataGrid_HeaderStyle_temp"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
												<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
											</asp:DataGrid>
										</DIV>
									</td>
								</tr>
								<TR id="trFirstInstallment" runat="server">
									<TD align="left" class="td_Text_Small" colspan="9">
										&nbsp;First - Installment Accounts
									</TD>
								</TR>
								<tr id="trInstallmentGrid" runat="server">
									<td colspan="9" align="right">
										<DIV style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 140px">
											<asp:DataGrid id="DataGridInstallment" runat="server" Width="690px" CssClass="DataGrid_Grid">
												<HeaderStyle cssclass="DataGrid_HeaderStyle_temp"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
												<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
											</asp:DataGrid>
										</DIV>
									</td>
								</tr>
								<TR>
									<TD align="left" class="td_Text_Small" colspan="9">
										&nbsp;Current Accounts
									</TD>
								</TR>
								<tr>
									<td colspan="9" align="right">
										<DIV style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 140px">
											<asp:DataGrid id="DatagridCurrentAccts" runat="server" Width="690px" CssClass="DataGrid_Grid">
												<HeaderStyle cssclass="DataGrid_HeaderStyle_temp"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
												<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
											</asp:DataGrid>
										</DIV>
									</td>
								</tr>
								<tr>
									<td colspan="9">&nbsp;</td>
								</tr>
								<TR>
									<TD align="left" class="td_Text_Small" colspan="9">
										&nbsp;Non - Funded Accounts
									</TD>
								</TR>
								<tr>
									<td colspan="9" align="right">
										<DIV style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 140px">
											<asp:DataGrid id="DatagridNonFundedContributions" runat="server" Width="690px" CssClass="DataGrid_Grid">
												<HeaderStyle cssclass="DataGrid_HeaderStyle_temp"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
												<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
											</asp:DataGrid>
										</DIV>
									</td>
								</tr>
							</table>
							<table class="Table_WithOutBorder" width="100%">
								<tr class="Td_ButtonContainer">
									<td align="right">
										<asp:Button id="ButtonTab1OK" cssClass="Button_Normal" runat="server" Text="OK" width="80"></asp:Button>
									</td>
								</tr>
							</table>
						</iewc:PageView>
						<iewc:PageView>
							<table  class="Table_WithoutBorder" width="698">
							<!--Commented by Parveen on 03-Nov-2009 to replace radiobutton with Checkbox -->
								<!--<tr>
									<td width="300"></td>
									<td colSpan="1">
										<asp:label id="LabelYes2" runat="server" Width="30">Yes</asp:label>&nbsp;
										<asp:label id="LabelNo2" runat="server" Width="30">No</asp:label>
									</td>
								</tr>-->
								<tr valign="top">
                                    <td width="50%">
                                        <table class="Table_WithoutBorder" width="100%" >
								            <tr>
									            <td align="left" width="270px">
										            <asp:label id="LabelRollover" runat="server">Did the participant request a rollover?</asp:label></td>
									            <td align="left">
										            <!--Commented by Parveen on 03-Nov-2009 to replace radiobutton with Checkbox -->
										            <!--<asp:radiobutton id="RadioButtonRolloverYes" runat="server" Text=" " GroupName="GrpRollover" autoPostBack="true"></asp:radiobutton>&nbsp;
										            <asp:radiobutton id="RadioButtonRolloversNo" runat="server" Text=" " Checked="True" GroupName="GrpRollover"
											            autoPostBack="true"></asp:radiobutton>-->
										            <!--Added By Parveen on 03-Nov-2009-->		
										            <asp:CheckBox id="CheckBoxRollovers" runat="server" autoPostBack="true" Checked="false" ></asp:CheckBox>
										            <!-- Ends Here-->
									            </td>

									            <%--<td align="right">
										            <asp:label id="LabelAddress1" runat="server" Width="60px">Address1</asp:label>
                                                    </td>
									            <td align="left">
										            <asp:textbox id="TextboxAddress1" EnableViewState="true" runat="server" Width="100" cssClass="TextBox_Normal"
											            MaxLength="60"></asp:textbox></td>
									            <td align="right">
										            <asp:label id="LabelCity" runat="server" Width="50px">City</asp:label></td>
									            <td>
										            <asp:textbox id="TextboxCity1" runat="server" Width="100" cssClass="TextBox_Normal" MaxLength="30"></asp:textbox></td>--%>
								            </tr>
								            <tr>
									            <td align="left" width="270px">
										            <asp:label id="LabelAddnlWitholding" runat="server">Did the participant request additional witholding?</asp:label></td>
									            <td align="left">
									            <!--Commented by Parveen on 03-Nov-2009 to replace radiobutton with Checkbox -->
									            <!--<asp:radiobutton id="RadioButtonAddnlWitholdingYes" runat="server" Text=" " GroupName="GrpWitholding"
											            autoPostBack="true"></asp:radiobutton>&nbsp;
										            <asp:radiobutton id="RadioButtonAddnlWitholdingNo" runat="server" Text=" " Checked="True" GroupName="GrpWitholding"
											            autoPostBack="true"></asp:radiobutton>-->
									            <!--Added By Parveen on 03-Nov-2009-->		
									            <asp:CheckBox id="CheckboxAddnlWitholding" runat="server" autoPostBack="true" Checked="false" ></asp:CheckBox>
									            <!--Ends here-->
									            </td>


									            <%--<td align="right">
										            <asp:label id="LabelAddress2" runat="server" Width="60px">Address2</asp:label></td>
									            <td align="left">
										            <asp:textbox id="TextboxAddress2" EnableViewState="true" runat="server" Width="100" cssClass="TextBox_Normal"
											            MaxLength="60"></asp:textbox></td>
									            <td align="right">
										            <asp:label id="LabelZip" runat="server" Width="50px">Zip</asp:label></td>
									            <td>
										            <asp:textbox id="TextBoxZip" runat="server" Width="100" cssClass="TextBox_Normal" MaxLength="30"></asp:textbox></td>--%>
								            </tr>
								            <tr>
									            <td align="left" width="270px">
										            <asp:label id="LabelVoluntaryAccounts" runat="server">Voluntary Accounts Only? </asp:label></td>
									            <td align="left">
										            <!--Commented by Parveen on 03-Nov-2009 to replace radiobutton with Checkbox -->
										            <!--<asp:radiobutton id="RadioButtonVoluntaryRolloverYes" runat="server" Text=" " GroupName="GrpRollovers"
											            autoPostBack="true"></asp:radiobutton>&nbsp;
										            <asp:radiobutton id="RadioButtonVoluntaryRolloverNo" runat="server" Text=" " Checked="True" GroupName="GrpRollovers"
											            autoPostBack="true"></asp:radiobutton>-->
										            <!--Added By Parveen on 03-Nov-2009-->
										            <asp:CheckBox id="CheckboxVoluntaryRollover" runat="server" autoPostBack="true" Checked="false" ></asp:CheckBox>	
										            <!-- Ends Here-->
									            </td>


									            <%--<td align="right">
										            <asp:label id="LabelAddress3" runat="server" Width="60px">Address3</asp:label></td>
									            <td align="left">
										            <asp:textbox id="TextboxAddress3" EnableViewState="true" runat="server" Width="100" cssClass="TextBox_Normal"
											            MaxLength="60"></asp:textbox></td>
									            <td align="right">
										            <asp:label id="LabelState" runat="server" Width="50px">State</asp:label></td>
									            <td>
										            <asp:textbox id="TextBoxState" runat="server" Width="100" cssClass="TextBox_Normal" MaxLength="30"></asp:textbox></td>--%>
								            </tr>
								            <%--<tr>
									            <td></td>
									            <td Width="50"></td>
									            <td></td>
									            <td align="left">
										            <asp:button id="ButtonAddress" runat="server" Width="110" Text="UpdateAddress" cssClass="Button_Normal"
											            visible="False"></asp:button>
									            </td>
									            <td align="right">
										            <asp:label id="LabelCountry" runat="server" Width="50px">Country</asp:label></td>
									            <td>
										            <asp:textbox id="TextBoxCountry" runat="server" Width="100" cssClass="TextBox_Normal" MaxLength="30"></asp:textbox></td>																	
								            </tr>--%>
                                    </table>
                                </td>
                                    <td align="left" class="Table_WithoutBorder" width="50%">
                                            <NewYRSControls:AddressWebUserControl_new ID="AddressWebUserControl1" runat="server" PopupHeight="530" AllowNote="true" AllowEffDate="false" />
                                    </td>
                                    
                                </tr>
							</table>
							<table  class="Table_WithoutBorder" width="698">
								<tr>
									<td class="Td_InLineBorder" align="left" colSpan="6" height="0"></td>
								</tr>
								<tr>
									<td align="left" width="25%" style="display:none">
										<asp:radiobutton id="RadioButtonNone" runat="server" Text="Partial Amount" Checked="True" GroupName="AccountRollover"
											autoPostBack="true"></asp:radiobutton></td>
				<%--</td>--%>
				<td colspan="4" width="60%" align="left"><asp:radiobutton id="RadiobuttonRolloverOnlyFirstInstallment" runat="server" Text="Rollover Only First Installment"
						GroupName="MKTAccountRollover" autoPostBack="true"></asp:radiobutton></td>
                <%-- Start: Bala: 2/4/2016: YRS-AT-2725: Moved here --%>
				<td colspan="4" align="left"><asp:radiobutton id="RadiobuttonRolloverAllInstallment" runat="server" Text="Rollover All Installment(s)"
						GroupName="MKTAccountRollover" autoPostBack="true"></asp:radiobutton></td>
                <%-- End: Bala: 2/4/2016: YRS-AT-2725: Moved here --%>
			</tr>
			<tr>
				<td align="left">
					 <%--Commented by Dinesh Kanojia
                                                Start: 2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only
                                                <asp:radiobutton id="RadioButtonRolloverAll" runat="server" Text="Rollover All" GroupName="AccountRollover"
						autoPostBack="true"></asp:radiobutton>
                     End: 2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only--%>
				</td>
                <%-- Start: Bala: 2/4/2016: YRS-AT-2725: Commented because this control is taken above --%>
				<%--<td colspan="4" align="left"><asp:radiobutton id="RadiobuttonRolloverAllInstallment" runat="server" Text="Rollover All Installment(s)"
						GroupName="MKTAccountRollover" autoPostBack="true"></asp:radiobutton></td>
				<td></td>--%>
                <%-- End: Bala: 2/4/2016: YRS-AT-2725: Commented because this control is taken above --%>
			</tr>
			<tr>
            <%--SR:2013.07.10 : BT 2116 - Alignment changes in withdrawal process screen.--%>   
                 <%-- Commented by Dinesh Kanojia
                 Start: 2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only         
				<td align="left" >
					<asp:radiobutton id="RadioButtonTaxableOnly" runat="server" Text="TaxableOnly" GroupName="AccountRollover" autoPostBack="true" visible="false" Width="35px"></asp:radiobutton>
                    <asp:radiobutton id="RadioButtonSpecificAmount" runat="server" Width="130px" Text="Rollover Amount" GroupName="AccountRollover"></asp:radiobutton>
                    <asp:textbox id="TextboxRolloverAmount" runat="server" width="70px" cssClass="TextBox_Normal" autoPostback="true" align="left" ></asp:textbox>
				</td>--%>
               
                <td align="left">
                    <table>
                        <tr>
                            <td>
                                <asp:radiobutton id="RadioButtonTaxableOnly" runat="server" text="TaxableOnly" groupname="AccountRollover" autopostback="true" width="35px"></asp:radiobutton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:radiobutton id="RadioButtonRolloverAll" runat="server" text="Rollover All" groupname="AccountRollover"
                                    autopostback="true"></asp:radiobutton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:radiobutton id="RadioButtonSpecificAmount" runat="server" width="130px" text="Rollover Amount" groupname="AccountRollover"></asp:radiobutton>
                                <asp:textbox id="TextboxRolloverAmount" runat="server" width="70px" cssclass="TextBox_Normal" autopostback="true" align="left"></asp:textbox>
                            </td>
                        </tr>
                    </table>
                </td>
                 <%--End: 2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only--%>

            	<td align="right" colspan="4" > 
					<asp:label id="LabelRemainingMoneyinMKT" runat="server" Width="270px" >Remaining Money in Market Withdrawal</asp:label>
                    
				</td> 
				
				<td align="right">	<asp:textbox id="TextboxRemainingMoneyinMKT" runat="server" width="80" cssClass="TextBox_Normal" autoPostback="true" readonly="true" ></asp:textbox>				
				</td>
			</tr>
             <%--End,SR:2013.07.10 : BT 2116 - Alignment changes in withdrawal process screen.--%>  
          
			<tr id="trHardshipVoluntary" runat="server">
				<td class="Td_InLineBorder" align="left" colSpan="6" height="0" ></td>
			</tr>
			<tr>
				<td align="left">
					<asp:label id="Label5" runat="server" visible="false" width="35">Voluntary Total</asp:label>
				</td>
				<td align="left">
					<asp:textbox id="TextboxVoluntaryWithdrawalTotal" runat="server" visible="false" width="80" cssClass="TextBox_Normal"
						autoPostback="true" readonly="true"></asp:textbox>
				</td>
				
				<td colspan="3" align="right"> 
					<asp:label id="LabelDeferedInstallmentAmount" runat="server" Width="56px">Defered Installment Amount</asp:label>
				</td>
				
				<td align="right">
					<asp:textbox id="TextboxDeferedInstallmentAmount" runat="server" width="80" cssClass="TextBox_Normal"
						autoPostback="true" readonly="true"></asp:textbox>
				</td>
				
				
			</tr>
			<tr>
				<td align="left">
					<asp:label id="Label1" runat="server" visible="false" width="35">TD Available</asp:label>
				</td>
				<td align="left">
					<asp:textbox id="TextboxTDAvailableAmount" runat="server" visible="false" width="80" cssClass="TextBox_Normal"></asp:textbox>
				</td>
				<td colspan="3" align="right">
					<asp:label id="LabelTotalRolloverAmount" runat="server" Width="56px">Defered Rollover Installment Amount</asp:label>
				</td>
				<td align="right">
			<asp:textbox id="TextboxTotalRolloverAmount" runat="server" width="80" cssClass="TextBox_Normal"
						autoPostback="true" ></asp:textbox>
				</td>
			</tr>
			<tr>
				<td align="left">
					<asp:label id="Label2" runat="server" visible="false" width="35">TD Amount Requested</asp:label>
				</td>
				<td align="left">
					<asp:textbox id="TextboxRequestAmount" runat="server" visible="false" width="80" Maxlength="12"
						cssClass="TextBox_Normal" autoPostback="true"></asp:textbox>
				</td>
				
			</tr>
			<tr>
				<td class="Td_InLineBorder" align="left" colSpan="6" height="0"></td>
			</tr>
	</table>
	<table class="Table_WithoutBorder" width="100%" id="TablePartialRollOver">
		<tr>
			<td colspan="2" align="left"><asp:label id="LabelPayee1" runat="server" Width="50">Payee1</asp:label>
				<asp:textbox id="TextBoxPayee1" runat="server" Width="150px" Maxlength="50" cssClass="TextBox_Normal"></asp:textbox></td>
			<td align="left">
				<asp:label id="LabelTaxRate" runat="server" Width="56px">Tax Rate</asp:label></td>
			<td align="left">
				<asp:label id="LabelTaxable" runat="server">Taxable</asp:label></td>
			<td align="left">
				<asp:label id="LabelNonTaxable" runat="server" Width="81px">Non Taxable</asp:label></td>
			<td align="left">
				<asp:label id="LabelTax" runat="server">Tax</asp:label></td>
			<td align="left">
				<asp:label id="LabelNet" runat="server">Net</asp:label></td>
		</tr>
		<tr>
			<td align="left" rowspan="4">
				<div style="OVERFLOW: auto; WIDTH: 220px; HEIGHT: 80px">
					<asp:datagrid id="DataGridPayee1" runat="server" Width="200px" CssClass="DataGrid_Grid" AutoGenerateColumns="False">
						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
						<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
						<SelectedItemStyle CssClass="DataGrid_SelectedStyle_Total"></SelectedItemStyle>
						<Columns>
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:Label id="LabelPayee1AcctType" runat="server" cssclass="Label_Normal" Text='<%# DataBinder.Eval(Container.DataItem, "AcctType") %>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:Label id="LabelPayee1Taxable" runat="server" autopostback=true OnTextChanged="Text_Changed" cssclass="TextBox_Normal" Width="55px" Text='<%# DataBinder.Eval(Container.DataItem, "Taxable") %>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:Label id="LabelPayee1NonTaxable" runat="server" autopostback=true OnTextChanged="Text_Changed" cssclass="TextBox_Normal" Width="55px" Text='<%# DataBinder.Eval(Container.DataItem, "NonTaxable") %>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></div>
			</td>
			<td>&nbsp;
			</td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxTaxRate" runat="server" Width="40" cssClass="TextBox_Normal" autoPostback="true"></asp:textbox></td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxTaxable" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxNonTaxable" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxTax" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxNet" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
		</tr>
		<tr>
			<td valign="top" width="40px"><asp:label id="Label3" runat="server" visible="false">TD Amount</asp:label></td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxHardShipTaxRate" runat="server" visible="false" Width="40" MaxLength="3"
					cssClass="TextBox_Normal" autoPostback="true"></asp:textbox></td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxHardShipAmount" runat="server" visible="false" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>            
			<td align="left" valign="top">
                <asp:textbox id="textboxHardShipNonTaxableAmount" runat="server" visible="false" Width="60" cssClass="TextBox_Normal"></asp:textbox> <%-- SR : 2019.07.31 | YRS-AT-3870 | Display NonTaxable components --%>
			</td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxHardShip" runat="server" visible="false" Width="60" MaxLength="15" autoPostback="true"
					cssClass="TextBox_Normal"></asp:textbox></td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxHardShipNet" runat="server" visible="false" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
		</tr>
		<tr>
			<td></td>
			
			<td align="left">
				<asp:textbox id="TextboxMinDistTaxRate" runat="server" Width="40" cssClass="TextBox_Normal" autoPostback="true"></asp:textbox></td>
			<td align="left">
				<asp:textbox id="TextboxMinDistAmount" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
            <td align="left">
				<asp:textbox id="TextboxMinDistNonTaxable" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
			<td align="left">
				<asp:textbox id="TextboxMinDistTaxable" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
         
			<td align="left">
				<asp:textbox id="TextboxMinDistNet" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
				<td align="left">
			</td>
		</tr>
		<tr>
			<td colspan="6" align="left">
				<asp:Label id="LabelRequiredMinDisbAmount" runat="server" CssClass="Error_Message">Required Minimum Distribution</asp:Label>
			</td>
		</tr>
		<tr>
			<td class="Td_InLineBorder" align="left" colSpan="7" height="0"></td>
		</tr>
		<tr>
			<td colspan="3" align="left">
                <table >
                    <tr> 
                        <td align="left"  ><asp:label id="LabelPayee2" runat="server">Payee2</asp:label></td>
                        <td align="left" Width="150px"><UC2:RollUserControl runat="server" ID="TextboxPayee2"/></td>  <%--Start:SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name --%>
                    </tr>
                </table>								
                </td>
			<td align="left">
				<asp:label id="LabelTaxable2" runat="server">Taxable</asp:label></td>
			<td align="left">
				<asp:label id="LabelNonTaxable2" runat="server" Width="81px">Non Taxable</asp:label></td>
			<td>&nbsp;</td>
			<td align="left">
				<asp:label id="LabelNet2" runat="server">Net</asp:label></td>
		</tr>
		<tr>
			<td colspan="3" align="left">
				<div style="OVERFLOW: auto; WIDTH: 220px; HEIGHT: 80px">
					<asp:datagrid id="DatagridPayee2" runat="server" Width="200px" CssClass="DataGrid_Grid" AutoGenerateColumns="False">
						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
						<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
						<SelectedItemStyle CssClass="DataGrid_SelectedStyle_Total"></SelectedItemStyle>
						<Columns>
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:Label id="LabelAcctType" runat="server" cssclass="Label_Normal" Text='<%# DataBinder.Eval(Container.DataItem, "AcctType") %>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<ItemTemplate>
									<%--<asp:TextBox id="TextboxPayee2Taxable" enabled = false runat="server" autopostback=true OnTextChanged="Text_Changed" MaxLength= "10" cssclass="TextBox_Normal" Width="55px" Text='<%# DataBinder.Eval(Container.DataItem, "Taxable") %>'>
									</asp:TextBox>--%>
                                    <asp:Label id="LabelTaxable" runat="server" cssclass="Label_Normal" Text='<%# DataBinder.Eval(Container.DataItem, "Taxable") %>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<ItemTemplate>
									<%--<asp:TextBox id="TextboxPayee2NonTaxable" enabled = false runat="server" autopostback=true OnTextChanged="Text_Changed" MaxLength= "10" cssclass="TextBox_Normal" Width="55px" Text='<%# DataBinder.Eval(Container.DataItem, "NonTaxable") %>'>
									</asp:TextBox>--%>
                                     <asp:Label id="LabelNonTaxable" runat="server" cssclass="Label_Normal" Text='<%# DataBinder.Eval(Container.DataItem, "NonTaxable") %>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></div>
			</td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxTaxable2" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxNonTaxable2" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
			<td>&nbsp;</td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxNet2" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
		</tr>
		<tr>
			<td class="Td_InLineBorder" align="left" colSpan="7" height="0"></td>
		</tr>
		<!--
		<tr>
			<td colspan="3" align="left"><asp:label id="LabelPayee3" runat="server">Payee3</asp:label>
				<asp:textbox id="TextboxPayee3" runat="server" Width="120" Maxlength="30" cssClass="TextBox_Normal"
					autopostback="true"></asp:textbox></td>
			<td align="left">
				<asp:label id="LabelTaxable3" runat="server">Taxable</asp:label></td>
			<td align="left">
				<asp:label id="LabelNonTaxable3" runat="server" Width="81px">Non Taxable</asp:label></td>
			<td>&nbsp;</td>
			<td align="left">
				<asp:label id="LabelNet3" runat="server">Net</asp:label></td>
		</tr>
		<tr>
			<td colspan="3" align="left"><div style="OVERFLOW: auto; WIDTH: 220px; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 80px; BORDER-BOTTOM-STYLE: none">
					<asp:datagrid id="DatagridPayee3" runat="server" Width="200px" CssClass="DataGrid_Grid" AutoGenerateColumns="False">
						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
						<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
						<SelectedItemStyle CssClass="DataGrid_SelectedStyle_Total"></SelectedItemStyle>
						<Columns>
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:Label id="LabelPayee3AcctType" runat="server" cssclass="Label_Normal" Text='<%# DataBinder.Eval(Container.DataItem, "AcctType") %>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:TextBox id="TextBoxPayee3Taxable" enabled = false runat="server" autopostback=true OnTextChanged="Text_Changed" MaxLength= "10" cssclass="TextBox_Normal" Width="55px" Text='<%# DataBinder.Eval(Container.DataItem, "Taxable") %>'>
									</asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:TextBox id="TextBoxPayee3NonTaxable" enabled = false runat="server" autopostback=true OnTextChanged="Text_Changed" MaxLength= "10" cssclass="TextBox_Normal" Width="55px" Text='<%# DataBinder.Eval(Container.DataItem, "NonTaxable") %>'>
									</asp:TextBox>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></div>
			</td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxTaxable3" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxNonTaxable3" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
			<td>&nbsp;</td>
			<td align="left" valign="top">
				<asp:textbox id="TextboxNet3" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
		</tr>
		<tr>
			<td class="Td_InLineBorder" align="left" colSpan="7" height="0"></td>
		</tr>-->
		<tr height="20px">
			<td align="left" colspan="2" rowspan="2">
				<div style="OVERFLOW: auto ; Height: 100Px; Width:220px">
					<asp:datagrid id="DatagridDeductions" runat="server" Width="200px" CssClass="DataGrid_Grid" Autogeneratecolumns="false">
						<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
						<Columns>						
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:CheckBox id="CheckBoxDeduction" runat="server" autoPostBack="true" OnCheckedChanged="CheckBoxDeduction_Checked"></asp:CheckBox>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="CodeValue" HeaderText="Deductions" visible="False">
								<HeaderStyle Width="10px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ShortDescription" HeaderText="Deductions">
								<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Amount" HeaderText="Amount">
								<HeaderStyle Width="60px"></HeaderStyle>
								</asp:BoundColumn>
						</Columns>
					</asp:datagrid>
				</div>
			</td>
            <td align="left">
				<asp:label id="lblProcessingFee" runat="server">P. Fee</asp:label></td> <%-- SR | 2016.06.09 | YRS-AT-2962 | Add lable for Processing Fee --%>
			<td align="left">
				<asp:label id="LabelDeductions" runat="server">Deductions</asp:label></td>
			<td align="left">
				<asp:Label id="LabelTaxableFinal" runat="server">Taxable</asp:Label></td>
			<td align="left">
				<asp:Label id="LabelNonTaxableFinal" runat="server" Width="81px">Non Taxable</asp:Label></td>
			<td align="left">
				<asp:Label id="LabelTaxFinal" runat="server">Tax</asp:Label></td>
			<td align="left">
				<asp:Label id="LabelNetFinal" runat="server">Net</asp:Label></td>
		</tr>
		<tr height="80px">
            <td valign="top" align="left">
				<asp:textbox id="txtProcessingFee" runat="server" Width="40" cssClass="TextBox_Normal" ReadOnly="true"></asp:textbox> <%-- SR | 2016.06.09 | YRS-AT-2962 | Add textbox for Processing Fee --%>
			</td>
			<td valign="top" align="left">
				<asp:textbox id="TextboxDeductions" runat="server" Width="40" cssClass="TextBox_Normal"></asp:textbox>
			</td>
			<td valign="top" align="left">
				<asp:textbox id="TextboxTaxableFinal" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
			<td valign="top" align="left">
				<asp:textbox id="TextboxNonTaxableFinal" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
			<td valign="top" align="left">
				<asp:textbox id="TextboxTaxFinal" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
			<td valign="top" align="left">
				<asp:textbox id="TextboxNetFinal" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
		</tr>
	</table>
	<!--Commented By Parveen on 04-Nov-2009-->
	<!--<table>
		<tr>
			<td align="left">
				<asp:label id="LabelReleaseSigned" runat="server">Has the Release Form been signed?
		    	</asp:label>
			</td>
			<td>
				<asp:radiobutton id="RadioButtonReleaseSignedYes" runat="server" Text=" " GroupName="GrpReleaseSigned"
					autoPostBack="true"></asp:radiobutton>&nbsp;
				<asp:radiobutton id="RadioButtonReleaseSignedNo" runat="server" Text=" " Checked="True" GroupName="GrpReleaseSigned"
					autoPostBack="true"></asp:radiobutton>
			</td>
			<td align="left">
				<asp:label id="LabelAddressUpdating" runat="server">Does the Address need updating?
			 </asp:label>
			</td>
			<td>
				<asp:radiobutton id="RadioButtonAddressUpdatingYes" runat="server" Text=" " GroupName="GrpAddressUpdating"
					autoPostBack="true"></asp:radiobutton>&nbsp;
				<asp:radiobutton id="RadioButtonAddressUpdatingNo" runat="server" Text=" " Checked="True" GroupName="GrpAddressUpdating"
					autoPostBack="true"></asp:radiobutton>
			</td>
			<td align="left">
				<asp:label id="LabelNotarized" runat="server">Has the Release Form been notarized?</asp:label></td>
			<td>
				<asp:radiobutton id="RadioButtonNotarizedYes" runat="server" Text=" " GroupName="GrpNotarized" autoPostBack="true"></asp:radiobutton>&nbsp;
				<asp:radiobutton id="RadioButtonNotarizedNo" runat="server" Text=" " Checked="True" GroupName="GrpNotarized"
					autoPostBack="true"></asp:radiobutton>
			</td>
		</tr>
	</table>-->
	<table class="Table_WithOutBorder" width="100%">
		<tr class="Td_ButtonContainer">
			<td align="right">
				<asp:Button id="ButtonSave" runat="server" Text="Save" cssClass="Button_Normal" width="80"></asp:Button>
				&nbsp; &nbsp;
				<asp:Button id="ButtonOK" cssClass="Button_Normal" runat="server" Text="OK" width="80"></asp:Button>
			</td>
		</tr>
	</table>
<!--
	<table>
		<tr>
			<td align="left">
				<asp:label id="LabelWaiver" runat="server">Has the spouse signed a waiver?</asp:label></td>
			<td>
				<asp:radiobutton id="RadioButtonWaiverYes" runat="server" Text=" " GroupName="GrpWaiver" autoPostBack="true"></asp:radiobutton>&nbsp;
				<asp:radiobutton id="RadioButtonWaiverNo" runat="server" Text=" " Checked="True" GroupName="GrpWaiver"
					autoPostBack="true"></asp:radiobutton></td>
			<td align="left" width="300">
				<asp:label id="LabelPersonalMonies" runat="server">Did the participant request Personal Monies Only ? </asp:label></td>
			<td>
				<asp:radiobutton id="RadiobuttonPersonalMoniesYes" runat="server" Text=" " GroupName="PersonalMonies"
					autoPostBack="true"></asp:radiobutton>&nbsp;
				<asp:radiobutton id="RadiobuttonPersonalMoniesNo" runat="server" Text=" " Checked="True" GroupName="PersonalMonies"
					autoPostBack="true"></asp:radiobutton>
			</td>
		</tr>
		</tr>
	</table>
		-->
	</iewc:PageView> </iewc:multipage></TD></TR></TBODY></TABLE><asp:textbox id="TextBox1" runat="server" EnableViewState="true" visible="false"></asp:textbox><asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder></form>
<script language="javascript">
		if (objRequestAmount){
			document.all("TextboxRequestAmount").focus();
		}		
</script>
<!--#include virtual="bottom.html"-->
