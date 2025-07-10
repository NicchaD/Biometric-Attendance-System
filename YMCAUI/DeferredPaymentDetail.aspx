<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DeferredPaymentDetail.aspx.vb" Inherits="YMCAUI.DeferredPaymentDetail" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="UserControls/YMCA_Footer_WebUserControl.ascx"%>

<HTML>
	<HEAD>
		<TITLE>YMCA YRS</TITLE>
		<LINK href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
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
	vDecimalAfterPeriod  = 4
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
</script>
           
			
	</HEAD>
	<BODY>
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
			<tr>
				<td align="center"><YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL id="YMCA_Toolbar_WebUserControl1" runat="server" ShowHomeLinkButton="false" ShowLogoutLinkButton="false"></YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL></td>
			</tr>
			<tr>
				<td class="Td_HeadingFormContainer" align="left">Deferred Rollover Details</td>
			</tr>
		</table>
	</div>
	<table class="Table_WithBorder" width="740" cellSpacing="0" cellPadding="0"align ="center" border="0" >
		<TBODY>
			<tr>
				<td class="td_Text_Small" align="left" height="18" ><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					<asp:label id="LabelTitle" runat="server" cssClass="td_Text_Small"></asp:label></td>
			</tr>
			<tr>
				<td>
					<table class="Table_WithoutBorder" width="100%">
						<tr>
							<td align="left" colSpan="2">Remaining Money in Market Withdrawal
							</td>
							<td><asp:textbox id="txtRemainingMBWMoney" ReadOnly="True" runat="server" cssClass="TextBox_Normal"
									width="80"></asp:textbox></td>
						</tr>
						<tr>
							<td align="left" colSpan="2">Defered Installment Amount
							</td>
							<td><asp:textbox id="txtDeferredInstallmentAmt" ReadOnly="True" runat="server" cssClass="TextBox_Normal"
									width="80"></asp:textbox></td>
						</tr>
						<tr>
							<td class="Td_InLineBorder" align="left" colSpan="4" height="0"></td>
						</tr>
					</table>
					<table class="Table_WithoutBorder" id="TablePartialRollOver" width="100%">
						<tr>
							<td align="left" colSpan="2"><asp:label id="LabelPayee1" runat="server" Width="50">Payee1</asp:label><asp:textbox id="TextBoxPayee1" ReadOnly="True" runat="server" cssClass="TextBox_Normal" Width="120"
									Maxlength="50"></asp:textbox></td>
							<td align="left"><asp:label id="LabelTaxRate" runat="server" Width="56px">Tax Rate</asp:label></td>
							<td align="left"><asp:label id="LabelTaxable" runat="server">Taxable</asp:label></td>
							<td align="left"><asp:label id="LabelNonTaxable" runat="server" Width="81px">Non Taxable</asp:label></td>
							<td align="left"><asp:label id="LabelTax" runat="server">Tax</asp:label></td>
							<td align="left"><asp:label id="LabelNet" runat="server">Net</asp:label></td>
						</tr>
						<tr>
							<td align="left">
								<div style="OVERFLOW: auto; WIDTH: 220px; HEIGHT: 80px"><asp:datagrid id="DataGridPayee1" runat="server" Width="200px" CssClass="DataGrid_Grid" AutoGenerateColumns="False">
										<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
										<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
										<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
										<SelectedItemStyle CssClass="DataGrid_SelectedStyle_Total"></SelectedItemStyle>
										<Columns>
											<asp:BoundColumn DataField="chrAcctType" HeaderText="AcctType">
												<HeaderStyle Width="100px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="mnyTaxable" HeaderText="Taxable">
												<HeaderStyle Width="100px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="mnyNonTaxable" HeaderText="Non Taxable">
												<HeaderStyle Width="100px"></HeaderStyle>
											</asp:BoundColumn>
										</Columns>
									</asp:datagrid></div>
							</td>
							<td>&nbsp;
							</td>
							<td vAlign="top" align="left"><asp:textbox id="TextboxTaxRate" runat="server" cssClass="TextBox_Normal" Width="40" autoPostback="true"></asp:textbox></td>
							<td vAlign="top" align="left"><asp:textbox id="TextboxTaxablePayee1" ReadOnly="True" runat="server" cssClass="TextBox_Normal"
									Width="60"></asp:textbox></td>
							<td vAlign="top" align="left"><asp:textbox id="TextboxNonTaxablePayee1" ReadOnly="True" runat="server" cssClass="TextBox_Normal"
									Width="60"></asp:textbox></td>
							<td vAlign="top" align="left"><asp:textbox id="TextboxTax" runat="server" cssClass="TextBox_Normal" Width="60" autoPostback="true"></asp:textbox></td>
							<td vAlign="top" align="left"><asp:textbox id="TextboxNetPayee1" ReadOnly="True" runat="server" cssClass="TextBox_Normal" Width="60"></asp:textbox></td>
						</tr>
						<tr>
							<td class="Td_InLineBorder" align="left" colSpan="7" height="0"></td>
						</tr>
						<tr>
							<td align="left" colSpan="3"><asp:label id="LabelPayee2" runat="server">Payee2</asp:label><asp:textbox id="TextboxPayee2" runat="server" cssClass="TextBox_Normal" Width="120" Maxlength="50"></asp:textbox></td>
							<td align="left"><asp:label id="LabelTaxable2" runat="server">Taxable</asp:label></td>
							<td align="left"><asp:label id="LabelNonTaxable2" runat="server" Width="81px">Non Taxable</asp:label></td>
							<td>&nbsp;</td>
							<td align="left"><asp:label id="LabelNet2" runat="server">Net</asp:label></td>
						</tr>
						<tr>
							<td align="left" colSpan="3">
								<div style="OVERFLOW: auto; WIDTH: 220px; HEIGHT: 80px"><asp:datagrid id="DatagridPayee2" runat="server" Width="200px" CssClass="DataGrid_Grid" AutoGenerateColumns="False">
										<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
										<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
										<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
										<SelectedItemStyle CssClass="DataGrid_SelectedStyle_Total"></SelectedItemStyle>
										<Columns>
											<asp:BoundColumn DataField="chrAcctType" HeaderText="AcctType">
												<HeaderStyle Width="80px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="mnyTaxable" HeaderText="Taxable">
												<HeaderStyle Width="80px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="mnyNonTaxable" HeaderText="Non Taxable">
												<HeaderStyle Width="60px"></HeaderStyle>
											</asp:BoundColumn>
										</Columns>
									</asp:datagrid></div>
							</td>
							<td vAlign="top" align="left"><asp:textbox id="TextboxTaxablePayee2" ReadOnly="True" runat="server" cssClass="TextBox_Normal"
									Width="60"></asp:textbox></td>
							<td vAlign="top" align="left"><asp:textbox id="TextboxNonTaxablePayee2" ReadOnly="True" runat="server" cssClass="TextBox_Normal"
									Width="60"></asp:textbox></td>
							<td>&nbsp;</td>
							<td vAlign="top" align="left"><asp:textbox id="TextboxNetPayee2" ReadOnly="True" runat="server" cssClass="TextBox_Normal" Width="60"></asp:textbox></td>
						</tr>
						<tr>
							<td class="Td_InLineBorder" align="left" colSpan="7" height="0"></td>
						</tr>
						<tr height="20">
							<td align="left" colSpan="2" rowSpan="2">
								<div style="OVERFLOW: auto; HEIGHT: 100px"><asp:datagrid id="DatagridDeductions" runat="server" Width="200px" CssClass="DataGrid_Grid" Autogeneratecolumns="false">
										<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
										<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
										<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
										<Columns>
											<asp:TemplateColumn>
												<ItemTemplate>
													<asp:CheckBox id="CheckBoxDeduction" runat="server" autoPostBack="True"></asp:CheckBox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="CodeValue" HeaderText="Deductions">
												<HeaderStyle Width="10px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="ShortDescription" HeaderText="Deductions">
												<HeaderStyle Width="100px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Amount" HeaderText="Amount">
												<HeaderStyle Width="60px"></HeaderStyle>
											</asp:BoundColumn>
										</Columns>
									</asp:datagrid></div>
							</td>
							<td align="left"><asp:label id="LabelDeductions" runat="server">Deductions</asp:label></td>
							<td align="left"><asp:label id="LabelTaxableFinal" runat="server">Taxable</asp:label></td>
							<td align="left"><asp:label id="LabelNonTaxableFinal" runat="server" Width="81px">Non Taxable</asp:label></td>
							<td align="left"><asp:label id="LabelTaxFinal" runat="server">Tax</asp:label></td>
							<td align="left"><asp:label id="LabelNetFinal" runat="server">Net</asp:label></td>
						</tr>
						<tr height="80">
							<td vAlign="top" align="left"><asp:textbox id="TextboxDeductions" ReadOnly="True" runat="server" cssClass="TextBox_Normal"
									Width="40"></asp:textbox></td>
							<td vAlign="top" align="left"><asp:textbox id="TextboxTaxableFinal" ReadOnly="True" runat="server" cssClass="TextBox_Normal"
									Width="60"></asp:textbox></td>
							<td vAlign="top" align="left"><asp:textbox id="TextboxNonTaxableFinal" ReadOnly="True" runat="server" cssClass="TextBox_Normal"
									Width="60"></asp:textbox></td>
							<td vAlign="top" align="left"><asp:textbox id="TextboxTaxFinal" ReadOnly="True" runat="server" cssClass="TextBox_Normal" Width="60"></asp:textbox></td>
							<td vAlign="top" align="left"><asp:textbox id="TextboxNetFinal" ReadOnly="True" runat="server" cssClass="TextBox_Normal" Width="60"></asp:textbox></td>
						</tr>
					</table>
					<table class="Table_WithOutBorder" width="100%">
						<tr class="Td_ButtonContainer">
							<td align="right"><asp:button id="ButtonSave" runat="server" cssClass="Button_Normal" width="80" Text="Save"></asp:button>&nbsp; 
								&nbsp;
								<asp:button id="ButtonCancel" runat="server" cssClass="Button_Normal" width="80" Text="Cancel"></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
		</TBODY></table>
	<asp:textbox id="TextBox1" runat="server" EnableViewState="true" visible="false"></asp:textbox>
				<YRSCONTROLS:YMCA_FOOTER_WEBUSERCONTROL id="YMCA_Footer_WebUserControl1" runat="server"></YRSCONTROLS:YMCA_FOOTER_WEBUSERCONTROL></TABLE>

	<asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder>
	</form>
	</BODY>
</HTML>
<script language="javascript">
		if (objRequestAmount){
			document.all("TextboxRequestAmount").focus();
		}		
</script>


