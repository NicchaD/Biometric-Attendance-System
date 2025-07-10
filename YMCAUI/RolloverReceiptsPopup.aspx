<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RolloverReceiptsPopup.aspx.vb" Inherits="YMCAUI.RolloverReceiptsPopup"%>
<!--#include virtual="TopNew.htm"-->
<script language="javascript">
	
function _OnBlur_TaxableAmount(){
document.Form1.all("TextboxCheckTotal").value=Number(document.Form1.all("TextboxTaxableAmount").value) +Number(document.Form1.all("TextboxNonTaxableAmount").value);
}
function _OnBlur_NonTaxableAmount(){
document.Form1.all("TextboxCheckTotal").value=Number(document.Form1.all("TextboxTaxableAmount").value) +Number(document.Form1.all("TextboxNonTaxableAmount").value);
}

/*
function _OnFocus_NonTaxableAmount(){
document.Form1.all("TextboxNonTaxableAmount").value=''
}

function _OnFocus_TaxableAmount(){
document.Form1.all("TextboxTaxableAmount").value=''
}*/
function _OnBlur_TextboxCheckNo()
{
	var _arr = new Array(20);
	var flg = false;
	var str = String(document.Form1.all.TextboxCheckNo.value);
	
	for(i=0;i<str.length; i++)
	{
		_arr[i] = str.substr(i,1);
			
	}
	
	for ( i=0;i<str.length -1;i++)
	{
	
		if (_arr[i] != _arr[i + 1])
		{
			flg = true;
			
		}
		
			
	}
	
	if(!flg)
	{
	//Added by Dilip yadav : BT - 939
	  if(str.length > 1) // DY
	  {
		alert('All the characters are same.');
			return;
	  }
	}
	
	
}	



var theform;
var isIE;
var isNS;

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
	{
		// clear the control as this is not a num
		//ctl.value=""
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
			//tempVal = ctlVal
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
	var reValidChars = /[0-9.]/;
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

	if (reValidChars.test(strKey))
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

}
</script>
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					Rollin Receipts</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="695">
			<tr>
				<td>
					<table>
						<tr>
							<td align="left"><asp:label id="LabelSSNo" runat="server" CssClass="Label_Small">SS No.</asp:label></td>
							<td align="left"><asp:label id="LabelFirst" runat="server" CssClass="Label_Small">First</asp:label></td>
							<td align="left"><asp:label id="LabelLast" runat="server" CssClass="Label_Small">Last</asp:label></td>
						</tr>
						<tr>
							<td><asp:textbox id="TextBoxSSNo" runat="server" CssClass="TextBox_Normal" Enabled="False"></asp:textbox></td>
							<td><asp:textbox id="TextBoxFirst" runat="server" CssClass="TextBox_Normal" Enabled="False"></asp:textbox></td>
							<td><asp:textbox id="TextBoxlast" runat="server" CssClass="TextBox_Normal" Enabled="False"></asp:textbox></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td align="left">
					<table width="690">
						<tr>
							<td><asp:label id="LabelInstitutionName" runat="server" CssClass="Label_Small">Institution Name</asp:label></td>
							<td><asp:label id="LabelInfoSource" runat="server" CssClass="Label_Small">Info Source</asp:label></td>
							<td><asp:label id="LabelStatus" runat="server" CssClass="Label_Small">Status</asp:label></td>
							<td><asp:label id="LabelDateReceived" runat="server" CssClass="Label_Small">Date Received</asp:label></td>
						</tr>
						<tr>
							<td><asp:textbox id="TextboxInstitution" runat="server" CssClass="TextBox_Normal" Enabled="False"
									width="194"></asp:textbox></td>
									<td><asp:textbox id="TextboxInfoSource" runat="server" CssClass="TextBox_Normal" Enabled="False"
									width="194"></asp:textbox></td>
							<td><asp:textbox id="TextboxStatus" runat="server" CssClass="TextBox_Normal" Enabled="False"></asp:textbox></td>
							<td><asp:textbox id="TextboxDateReceived" runat="server" CssClass="TextBox_Normal" Enabled="False"></asp:textbox></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<hr>
				</td>
			</tr>
			<tr>
				<td align="left">
					<table width="690">
						<tr>
							<td><asp:label id="LabelCheckReceivedDate" runat="server" CssClass="Label_Small">Check Received Date</asp:label></td>
							<td><asp:label id="LabelCheckNumber" runat="server" CssClass="Label_Small">Check Number</asp:label></td>
							<td><asp:label id="LabelCheckDate" runat="server" CssClass="Label_Small">CheckDate</asp:label></td>
						</tr>
						<tr>
							<td vAlign="top"><uc1:DateUserControl id="TextboxheckReceivedDate" runat="server"></uc1:DateUserControl></td>
							<td vAlign="top"><asp:textbox id="TextboxCheckNo" runat="server" CssClass="TextBox_Normal" MaxLength="16"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" ControlToValidate="TextboxCheckNo" ErrorMessage="Check No cannot be blank."></asp:requiredfieldvalidator></td>
							<td vAlign="top"><uc1:DateUserControl id="TextboxCheckDate" runat="server"></uc1:DateUserControl></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<table width="650">
						<tr>
							<td align="left"><asp:label id="LabelTaxableAmount" runat="server" CssClass="Label_Small">Taxable Amount</asp:label></td>
							<td align="left"><asp:label id="Label1NonTaxableAmount" runat="server" CssClass="Label_Small">Non-Taxable Amount</asp:label></td>
							<td align="left"><asp:label id="LabelCheckTotal" runat="server" CssClass="Label_Small">Check Total</asp:label></td>
						</tr>
						<tr>
							<td vAlign="top" align="left"><asp:textbox id="TextboxTaxableAmount" runat="server" CssClass="TextBox_Normal" MaxLength="9"
									autopostback="false"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" ControlToValidate="TextboxTaxableAmount"
									ErrorMessage="Taxable amount cannot be blank."></asp:requiredfieldvalidator></td>
							<td vAlign="top" align="left"><asp:textbox id="TextboxNonTaxableAmount" runat="server" CssClass="TextBox_Normal" MaxLength="9"
									autopostback="false" ReadOnly="False"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator5" runat="server" ControlToValidate="TextboxNonTaxableAmount"
									ErrorMessage="NonTaxable amount cannot be blank."></asp:requiredfieldvalidator></td>
							<td vAlign="top" align="left"><asp:textbox id="TextboxCheckTotal" runat="server" CssClass="TextBox_Normal" Enabled="False"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator6" runat="server" ControlToValidate="TextboxCheckTotal"
									ForeColor="White"></asp:requiredfieldvalidator></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td class="Td_ButtonContainer" align="left">
					<table height="30" width="688">
						<tr>
							<td class="Td_ButtonContainer" width="555"><asp:button id="ButtonAdd" runat="server" CssClass="Button_Normal" CausesValidation="False"
									Text="Add" Width="70px"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" Text="Save" Width="70px"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" CausesValidation="False"
									Text="Cancel" Width="70px"></asp:button></td>
							<td></td>
							<TD width="9"></TD>
							<TD></TD>
							<TD class="Td_ButtonContainer"><asp:button id="ButtonOk" runat="server" CssClass="Button_Normal" Enabled="False" CausesValidation="False"
									Text="OK" Width="70px"></asp:button></TD>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form> <!--#include virtual="bottom.html"-->
