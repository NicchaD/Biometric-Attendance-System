<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LoanRequest.aspx.vb" Inherits="YMCAUI.LoanRequest" %>
<!--#include virtual="TopNew.htm"-->
<script language="javascript">


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
	<table class="Table_WithoutBorder" cellSpacing="0" width="700">
		<tr>
			<td class="td_backgroundcolorwhite" colSpan="2"></td>
		</tr>
		<tr>
			<td class="td_backgroundcolorwhite" colSpan="2"></td>
		</tr>
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"></td>
		</tr>
	</table>
	<div class="Div_Center">
		<table class="Table_WithoutBorder" cellSpacing="0" width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><asp:label id="LabelTitle" runat="server" cssClass="td_Text_Small"></asp:label>
				</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<table class="Table_WithBorder" cellSpacing="0" width="700">
		<TBODY>
			<tr>
				<td class="td_Text_Small" align="left" height="20"><asp:label id="LabelTitle1" runat="server" cssClass="td_Text_Small">Personal Details</asp:label></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td>
					<table class="Table_WithoutBorder" cellSpacing="0" width="690">
						<TBODY>
							<tr>
								<td class="Text_Normal" align="left" width="170">SS.No</td>
								<td class="Text_Normal" align="left" width="170">First</td>
								<td class="Text_Normal" align="left" width="170">Middle</td>
								<td class="Text_Normal" align="left" width="170">Last</td>
								<td class="Text_Normal" align="left" width="170">Age</td>
							</tr>
							<tr>
								<td class="Text_Normal" align="left" width="170"><asp:textbox id="TextBoxSSNo" runat="server" ReadOnly="True" CssClass="TextBox_Normal"></asp:textbox></td>
								<td class="Text_Normal" align="left" width="170"><asp:textbox id="TextBoxFirstName" runat="server" ReadOnly="True" CssClass="TextBox_Normal"></asp:textbox></td>
								<td class="Text_Normal" align="left" width="170"><asp:textbox id="TextBoxMiddleName" runat="server" ReadOnly="True" CssClass="TextBox_Normal"></asp:textbox></td>
								<td class="Text_Normal" align="left" width="170"><asp:textbox id="TextBoxLastName" runat="server" ReadOnly="True" CssClass="TextBox_Normal"></asp:textbox></td>
								<td class="Text_Normal" align="left" width="170"><asp:textbox id="TextboxAge" runat="server" ReadOnly="True" CssClass="TextBox_Normal"></asp:textbox></td>
							</tr>
							<tr>
								<td colSpan="5">&nbsp;</td>
							</tr>
							<tr>
								<td colSpan="4">&nbsp;
								</td>
							</tr>
							<tr>
								<td class="td_Text_Small" align="left" colSpan="5">Total Amounts
								</td>
							</tr>
							<tr>
								<td align="left" colSpan="4">&nbsp;
								</td>
							</tr>
							<tr>
								<td colSpan="5">
									<table>
										<tr>
											<td class="Text_Normal" align="left"><asp:label id="LabelAccountBalance" runat="server" cssClass="Label_Small">Account Balance</asp:label></td>
											<td class="Text_Normal" align="left"><asp:textbox id="TextboxAccountBalance" runat="server" ReadOnly="True" CssClass="TextBox_Normal"></asp:textbox></td>
											<td class="Text_Normal" align="left"><asp:label id="LabelAmountAvailable" runat="server" cssClass="Label_Small">Amount Available</asp:label></td>
											<td class="Text_Normal" align="left"><asp:textbox id="TextboxAmountAvailable" runat="server" ReadOnly="True" CssClass="TextBox_Normal"></asp:textbox></td>
											<td class="Text_Normal" align="left"><asp:label id="LabelAmountRequested" runat="server" cssClass="Label_Small">Amount Requested</asp:label></td>
											<td class="Text_Normal" align="left"><asp:textbox id="TextboxAmountRequested" runat="server" CssClass="TextBox_Normal" MaxLength="10"
													onpaste="return false"></asp:textbox></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td align="left" colSpan="5">&nbsp;
								</td>
							</tr>
							<tr>
								<td class="td_Text_Small" align="left" colSpan="5">&nbsp;
								</td>
							</tr>
							<tr>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<td class="Text_Normal" align="left" width="170"></td>
								<td class="Text_Normal" align="right" width="170">Processing Fee Withheld &nbsp;</td>
								<td class="Text_Normal" align="left" width="170"><asp:textbox id="TextboxFeeWithheld" runat="server" CssClass="TextBox_Normal_Amount" 
										Readonly="True"></asp:textbox></td>
								<td class="Text_Normal" align="left" width="170"></td>
							</tr>
							<tr>
								<td align="left" colSpan="5" height="3">&nbsp;
								</td>
							</tr>
							<tr class="Td_ButtonContainer">
								<td class="Text_Normal" align="left" width="170"></td>
								<td class="Text_Normal" align="left" width="170"></td>
								<td class="Text_Normal" align="right" width="170"></td>
								<td class="Text_Normal" align="right" width="170"><asp:button id="ButtonSave" runat="server" Text="Save" cssclass="Button_Normal" Width="80px"></asp:button></td>
				</td>
				<td class="Td_ButtonContainer" align="right" width="170"><asp:button id="ButtonOK" runat="server" Text="OK" cssclass="Button_Normal" Width="80px"></asp:button></td>
				</TD></tr>
		</TBODY>
	</table>
	</TD></TR></TBODY></TABLE><asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder></form>
<!--#include virtual="bottom.html"-->
</TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></FORM>
