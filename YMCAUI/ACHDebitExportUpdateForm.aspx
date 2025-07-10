<!--#include virtual="top.html"-->
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ACHDebitExportUpdateForm.aspx.vb" Inherits="YMCAUI.ACHDebitExportUpdateForm"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<HTML>
	<HEAD>
		<title>ACHDebitExportUpdateForm</title>
		<script language="javascript">
/*var theform;
var isIE;
var isNS;

/*
Function to detect the Browser type.
*/
/*function detectBrowser()
{
	if (window.navigator.appName.toLowerCase().indexOf("netscape") > -1) 
		theform = document.forms["Form1"];
	else 
		theform = document.Form1;
		
	//browser detection
	var strUserAgent = navigator.userAgent.toLowerCase();
	isIE = strUserAgent.indexOf("msie") > -1;
	isNS = strUserAgent.indexOf("netscape") > -1;
	
}*/
		function IsValidDate(sender, args)
		{	
			fmt = "MM/DD/YYYY";
			if (fnvalidateGendate_tmp(args,fmt))
			{
				args.IsValid = true;
			}
			else
			{
				args.IsValid = false;
			}
		}
		/*function ValidateNumeric()
				{	
					
					if ((event.keyCode<48) ||(event.keyCode>57))
					{
						//alert(event.keyCode)
						event.returnValue = false;
						
						}
						
									
				}*/
				/*function ValidateDecimal()
				{	
					//alert(event.keyCode);
					//id=document.getElementById(id)
					//alert(id.value)
					if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 46)
					{
						event.returnValue = false;
					}
					
				}	*/	
		
function ValidateDecimal(str)
{
	//alert (event.keyCode);
		//alert (event.keyCode);
	var return_type=0;	 
    var str_pos;
	if ((event.keyCode>=48 && event.keyCode<=57) ||  (event.keyCode==46) )
	{ 
		if(event.keyCode==46)
		{
			if(str.indexOf(".")>=0)
				return false;
			else 
				return true;
		}
		else
		   {
		      if((str.length - str.indexOf("."))>2 && (str.indexOf(".")>=0) )
	             return false;
               else
                return true;
           }
			return true;
	}
	else
	{
		return false;
	}
}


		
		
		
			
/*function FormatAmtControl(ctl){
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
	alert(ctl.id)
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


function HandleAmountFiltering(ctl)
{
alert(ctl.id)r
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

}*/
		</script>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<div class="Div_Center">
				<table width="700">
					<tr>
						<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
							ACHDebit Export Update<asp:label id="LabelHdr" runat="server" CssClass="Td_HeadingFormContainer"></asp:label></td>
					</tr>
					<tr>
						<td>&nbsp;
						</td>
					</tr>
				</table>
			</div>
			<div class="Div_Center" border="0">
				<table class="Table_WithBorder" width="700">
					<tr>
						<td align="left" ><asp:label id="LabelYMCANo" runat="server" CssClass="Label_Small">YMCANo</asp:label></td>
						<td align="left" ><asp:textbox id="TextboxYMCANo" runat="server" CssClass="TextBox_Normal"  Enabled="False"></asp:textbox></td>
					</tr>
					<tr>
						<td align="left" ><asp:label id="LabelYMCAName" runat="server" CssClass="Label_Small">YMCAName</asp:label></td>
						<td align="left" ><asp:textbox id="TextBoxYMCAName" runat="server" CssClass="TextBox_Normal"  Enabled="False"
								width="300"></asp:textbox></td>
					</tr>
					<tr>
						<td align="left" ><asp:label id="LabelAmount" runat="server" CssClass="Label_Small"> Amount</asp:label></td>
						<td align="left" ><asp:textbox id="TextBoxAmount" runat="server" CssClass="TextBox_Normal" Enabled="true"
								name="TextBoxAmount" MaxLength=15></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxAmount" ErrorMessage="Amount cannot be blank">*</asp:requiredfieldvalidator>
							<asp:RangeValidator id="RangeValidator1" runat="server" ErrorMessage=" Out of Range" ControlToValidate="TextBoxAmount"
								MinimumValue="1" MaximumValue="999999999999.99" Type="Double"></asp:RangeValidator></td>
					</tr>
					<tr>
						<td vAlign="top" align="left"><asp:label id="LabelPaymentDate" CssClass="Label_Small" Runat="server">Payment Date</asp:label></td>
						<td vAlign="top" noWrap align="left"><uc1:dateusercontrol id="TextBoxPaymentDate" runat="server" Height="22" autopostback="false"></uc1:dateusercontrol></td>
					</tr>
					<tr>
						<td>&nbsp;
						</td>
					</tr>
					<tr>
                        <td colspan="2" class="Td_ButtonContainer">
                            <table class="Table_WithoutBorder" width="100%" >
                                <tr>
                                    <td align="right"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" CausesValidation="False"
								    Text="Cancel" Width="87"></asp:button></td>
						            <td  align="right" width="87px"><asp:button id="ButtonUpdate" runat="server" CssClass="Button_Normal" Text="Update" Width="87"></asp:button></td>        
                                </tr>
                            </table>
                        </td>

						
					</tr>
					<tr>
						<td width="310" colSpan="2"></td>
					</tr>
				</table>
			</div>
			<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
		</form>
		<!--#include virtual="bottom.html"-->
	</body>
</HTML>
