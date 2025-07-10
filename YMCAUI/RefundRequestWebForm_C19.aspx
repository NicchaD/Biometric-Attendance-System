<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RefundRequestWebForm_C19.aspx.vb" Inherits="YMCAUI.RefundRequestWebForm_C19"%>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<!--#include virtual="TopNew.htm"-->
<script language="javascript">
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
    if(((event.keyCode<48)||(event.keyCode>57)) && event.keyCode!=46)
	{
		event.returnValue=false;
	}
}


function ValidateNumeric()
{
	if(((event.keyCode<48)||(event.keyCode>57)) && event.keyCode!=46)
	{
		event.returnValue=false;
	}
}
function showToolTip() {
    var divId, linkId;


    divId = "<%=Tooltip.ClientID%>";
    linkId = "lblComments";

    if (null != divId) {
        var elementRef = document.getElementById(divId);
        if (elementRef != null) {
            elementRef.style.position = 'absolute';
            elementRef.style.left = event.clientX - 373 + document.body.scrollLeft;
            elementRef.style.top = event.clientY + 0 + document.body.scrollTop;
            elementRef.style.width = '380';
            elementRef.style.visibility = 'visible';
            elementRef.style.display = 'block';
        }

        if (null != linkId) {

            var InnerTextMgs = '<p style=line-height:1.25;>IRS Override option will be disabled in case you do not have sufficient permissions.<br/><br/>';
            InnerTextMgs = InnerTextMgs + 'Once IRS override option is selected:<br/>';
            InnerTextMgs = InnerTextMgs + '&nbsp;&nbsp;-The HWL4/HWL5 letter will not be generated and not sent to IDM.<br/>';
            InnerTextMgs = InnerTextMgs + 'During Withdrawal Processing:<br/>'
            InnerTextMgs = InnerTextMgs + '&nbsp;&nbsp;- The letter to LPA will not be generated & not sent to IDM. <br/>'
            InnerTextMgs = InnerTextMgs + '&nbsp;&nbsp;- The email to LPA will not be generated.<br/>'
            InnerTextMgs = InnerTextMgs + '&nbsp;&nbsp;- The 6 months suspension of TD contribution will not happen.</p>'

            var lblNote = document.getElementById("<%=lblComments.ClientID%>");
            lblNote.innerHTML = InnerTextMgs;

                    }
                }
            }

            //to hide tool tip when mouse is removed
            function hideToolTip() {
                var divId, linkId;
                divId = "<%=Tooltip.ClientID%>";
            linkId = "lblComments";
            if (null != divId) {
                var elementRef = document.getElementById(divId);
                if (elementRef != null) {
                    elementRef.style.visibility = 'hidden';
                }
            }
            if (null != linkId) {
                var elementBak = document.getElementById(linkId);
                if (elementBak != null) {

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
    <div id="DivMainMessage" class="warning-msg" runat="server" style="text-align: left;" enableviewstate="true" height="10">  </div> 
	<div class="Div_Center">
		<table class="Table_WithoutBorder" cellSpacing="0" width="100%" > 
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
					Withdrawal Request&nbsp;&nbsp;&nbsp;</td>
                   <td align="right" style="background-color:red;"><font color="white">COVID19 Screen</font></td>				
			</tr>
		</table>
	</div>
	<table class="Table_WithBorder" cellSpacing="0" width="700">
		<tr>
			<td width="722">
				<table class="Table_WithoutBorder" cellSpacing="0" width="690">
					<TBODY>
						<tr>
							<td class="td_Text_Small" align="left" width="270" height="20">FundNo :
								<asp:label id="LabelFundNo" runat="server" cssClass="td_Text_Small"></asp:label></td>
							<td class="td_Text_Small" align="left" colSpan="2" height="20">Name :
								<asp:label id="LabelParticipantName" runat="server" cssClass="td_Text_Small"></asp:label></td>
							<td class="td_Text_Small" align="left" width="170" height="20">
								<asp:label id="LabelSSNo" runat="server" cssClass="td_Text_Small" Visible="False"></asp:label></td>
						</tr>
						<tr>
							<td align="left" colSpan="4">   
								<table>
									<tr>
										<td class="Text_Normal" align="left" width="12.5%">Vested</td>
										<td class="Text_Normal" align="left" width="12.5%"><asp:textbox id="TextboxVested" runat="server" width="50" CssClass="TextBox_Normal"></asp:textbox></td>
										<td width="50">&nbsp;</td>
										<td width="50">&nbsp;</td>
										<td width="50">&nbsp;</td>
										<td class="Text_Normal" align="left" width="12.5%">Terminated</td>
										<td class="Text_Normal" align="right" width="12.5%"><asp:textbox id="TextBoxTerminated" runat="server" width="50" CssClass="TextBox_Normal"></asp:textbox></td>
										<td width="50">&nbsp;</td>
										<td class="Text_Normal" align="right" width="12.5%">Age</td>
										<td class="Text_Normal" align="right" width="12.5%"><asp:textbox id="TextboxAge" runat="server" width="50" CssClass="TextBox_Normal"></asp:textbox></td>
									</tr>
								</table>
							</td>
						</tr>
		</tr>
        <tr>
			<td class="Text_Normal" noWrap align="left" width="360px" colspan ="2"><asp:label id="lblAggregateBALegacyatRequest" runat="server" cssClass="Text_Normal" Text="Aggregate at Request (YMCA Acct + YMCA Acct Legacy)"></asp:label></td>
			<td class="Text_Normal" align="left" width="145px" colspan ="2"><asp:textbox id="txtAggregateBALegacyatRequest" runat="server" CssClass="TextBox_Normal" value=""></asp:textbox></td>			
		</tr>
		<tr>
			<td class="Text_Normal" noWrap align="left" width="145"><asp:label id="LabelBATermination" runat="server" cssClass="Text_Normal" Text="YMCA Acct at Request"></asp:label></td>
			<td class="Text_Normal" align="left" width="135"><asp:textbox id="TextboxBATerminate" runat="server" CssClass="TextBox_Normal" value=""></asp:textbox></td>
			<td class="Text_Normal" noWrap align="left" width="240"><asp:label id="LabelTermination" runat="server" cssClass="Text_Normal" Text="YMCA Acct (Legacy) at Termination"></asp:label></td>
			<td class="Text_Normal" align="left" width="135"><asp:textbox id="TextboxTerminatePIA" runat="server" CssClass="TextBox_Normal"></asp:textbox></td>
		</tr>
		<tr>
            <td colspan="4" align="left">
                <table>
                    <tr>
                        <td style="background-color:#c9dbed;" class="Text_Normal" noWrap align="left" Width="2%"><asp:label id="lblCovidAmountUsed" runat="server" cssClass="Text_Normal" Text="COVID - Amount Used"></asp:label></td>
			            <td style="background-color:#c9dbed;" class="Text_Normal" align="left" width="20%"><asp:textbox id="txtCovidAmountUsed" readonly="true" runat="server" CssClass="TextBox_Disabled"></asp:textbox></td>
                        <td style="background-color:#c9dbed;" class="Text_Normal" noWrap align="left" Width="20%"><asp:label id="lblCovidAmountAvailable" runat="server" cssClass="Text_Normal" Text="Amount Available"></asp:label></td>
			            <td style="background-color:#c9dbed;" class="Text_Normal" align="left" width="20%"><asp:textbox id="txtCovidAmountAvailable" readonly="true" runat="server" CssClass="TextBox_Disabled"></asp:textbox></td>
                        <td style="background-color:#c9dbed;" class="Text_Normal" noWrap align="left" Width="15%"><asp:label id="lblCovidTaxRate" runat="server" cssClass="Text_Normal" Text="Tax Rate"></asp:label></td>
			            <td style="background-color:#c9dbed;" class="Text_Normal" align="left" width="15%"><asp:textbox id="txtCovidTaxRate" readonly="true" runat="server" CssClass="TextBox_Disabled"></asp:textbox></td>
                    </tr>
                    <tr><td><br /></td></tr>
                </table>
            </td>
		</tr>
        <tr>
			<td class="Td_SubText" align="left">Retirement Plan</td>
			<td class="Td_SubText" align="left" width="170"><asp:checkbox id="CheckboxRegular" runat="server" CssClass="CheckBox_Normal" Text="Full" AutoPostBack="True"></asp:checkbox><asp:checkbox id="CheckboxSpecial" runat="server" CssClass="CheckBox_Normal" Text="Special" AutoPostBack="True"></asp:checkbox><asp:checkbox id="CheckboxDisability" runat="server" CssClass="CheckBox_Normal" Text="Disability"
					AutoPostBack="True"></asp:checkbox>
				<asp:CheckBox id="CheckBoxMarket" runat="server" CssClass="CheckBox_Normal" Text="Market Based"
					AutoPostBack="True" visible="False"></asp:CheckBox></td>
			<td class="Td_SubText" align="left" width="170"><asp:checkbox id="CheckboxExcludeYMCA" runat="server" CssClass="CheckBox_Normal" Text="Exclude YMCA Acct."
					AutoPostBack="True"></asp:checkbox></td>
			<td class="Td_SubText" align="left" width="170"><asp:checkbox id="CheckboxVoluntryAccounts" runat="server" CssClass="CheckBox_Normal" Text="Voluntary"
					AutoPostBack="True"></asp:checkbox></td>
		</tr>
		<tr>
			<td class="Td_SubText" align="left"></td>
			<td class="Td_SubText" align="left" width="170"><asp:checkbox id="CheckboxPartialRetirement" runat="server" CssClass="CheckBox_Normal" Text="Partial Amount"
					AutoPostBack="True"></asp:checkbox></td>
			<td class="Td_SubText" align="left" width="170"><asp:textbox id="TextboxPartialRetirement" runat="server" CssClass="TextBox_Normal" visible="False"
					autopostback="True"></asp:textbox></td>
			<td class="Td_SubText" align="left" width="170"></td>
		</tr>
        <tr>
			<td class="Td_SubText" align="left"></td>
			<td class="Td_SubText" align="left" width="170"><asp:checkbox id="CheckboxMRDRetirement" runat="server" CssClass="CheckBox_Normal" Text="RMD"
					AutoPostBack="True" visible="false"></asp:checkbox></td>
			<td class="Td_SubText" align="left"  colspan="2">
            <asp:checkbox id="CheckboxMRDRetirementCurrentYear" runat="server" CssClass="CheckBox_Normal" Text="Include current year"
					AutoPostBack="True" visible="false"></asp:checkbox>
          </td>
            <%--<td class="Td_SubText" align="left" width="170"></td>--%>
		</tr>
		<tr>
			<td align="left" colSpan="4"><asp:label id="LabelMessage" runat="server" visible="false" forecolor="Red"></asp:label></td>
		</tr>
		<tr>
			<td align="left" colSpan="4">
				<div style="OVERFLOW: auto; WIDTH: 100%; POSITION: relative"><asp:datagrid id="DataGridAccContributionRetirement" runat="server" CssClass="DataGrid_Grid" Width="100%"> <%--START | SR | YRS-AT-4055 |Increased width to fit retirement table.--%>
						<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp" height="10"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_NormalStyle_temp" height="10"></ItemStyle>
						<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
					</asp:datagrid></div>
			</td>
		</tr>
		<tr>
			<td align="left" colSpan="4" height="3">&nbsp;
			</td>
		</tr>
		<tr>
			<td height="3"><asp:textbox id="TextboxNonTaxed" runat="server" width="10" height="3" visible="False"></asp:textbox></td>
			<td height="3"><asp:textbox id="TextboxTaxed" runat="server" width="10" height="3" visible="False"></asp:textbox></td>
			<td height="3"><asp:textbox id="TextboxTax" runat="server" width="10" height="3" visible="False"></asp:textbox></td>
			<td height="3"><asp:textbox id="TextboxTaxRate" runat="server" width="10" AutoPostBack="true" height="3" visible="False"></asp:textbox></td>
		</tr>
		<tr>
			<td align="left" colSpan="4" height="0"></td>
		</tr>
		<tr>
			<td class="Td_SubText" align="left">Savings Plan</td>
			<td class="Td_SubText" noWrap align="left" width="170"><asp:checkbox id="CheckboxSavingsVoluntary" runat="server" CssClass="CheckBox_Normal" Text="Voluntary"
					AutoPostBack="True"></asp:checkbox></td>
			<td class="Td_SubText" align="left"  colSpan="2" width="170" >
                <table>
                    <tr>
                        <td style="white-space: nowrap;" width="100"> <asp:checkbox id="CheckboxHardship" runat="server" CssClass="CheckBox_Normal" Text="Hardship"
					AutoPostBack="True" visible="false"></asp:checkbox></td><td  style="white-space: nowrap;" >
                            <div id="DivIRSOverrideRules" runat="server">
               
                 (<asp:CheckBox ID="CheckboxIRSOverride" runat="server" CssClass="CheckBox_Normal" Text="IRS Override"></asp:checkbox>
               
                <img id="imgIRSOverrideRules" onmouseout="hideToolTip();" onmouseover="showToolTip();" src="images/icon-info.jpg" style="height:17px;width:17px;vertical-align:middle;" />)
                            </div>
                             </td>

                    </tr>

                </table>
                        </td>
			 
		</tr>
		<tr>
			<td class="Td_SubText" align="left"></td>
			<td class="Td_SubText" align="left" width="170"><asp:checkbox id="CheckboxPartialSavings" runat="server" CssClass="CheckBox_Normal" Text="Partial Amount"
					AutoPostBack="True"></asp:checkbox></td>
			<td class="Td_SubText" align="left" width="170"><asp:textbox id="TextboxPartialSavings" runat="server" CssClass="TextBox_Normal" visible="False"
					autopostback="True"></asp:textbox></td>
			<td class="Td_SubText" align="left" width="170"></td>
		</tr>
        <tr>
			<td class="Td_SubText" align="left"></td>
			<td class="Td_SubText" align="left" width="170"><asp:checkbox id="CheckboxMRDSavings" runat="server" CssClass="CheckBox_Normal" Text="RMD"
					AutoPostBack="True" visible="false"></asp:checkbox></td>
			<td class="Td_SubText" align="left"  colspan="2">
            <asp:checkbox id="CheckboxMRDSavingsCurrentYear" runat="server" CssClass="CheckBox_Normal" Text="Include current year"
					AutoPostBack="True" visible="false"></asp:checkbox>
            </td>

            <%--<td class="Td_SubText" align="left" width="170"></td>--%>
		</tr>
		<tr>
			<td align="left" colSpan="4">
				<div style="OVERFLOW: auto; WIDTH: 100%; POSITION: relative"><asp:datagrid id="DataGridAccContributionSavings" runat="server" CssClass="DataGrid_Grid" Width="100%"> <%--START | SR | YRS-AT-4055 |Increased width to fit Savings table.--%>
						<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp" height="10"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_NormalStyle_temp" height="10"></ItemStyle>
						<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
					</asp:datagrid></div>
			</td>
		</tr>
		<tr>
			<td align="left" colSpan="4" height="3">&nbsp;</td>
		</tr>
			<!--<td class="Text_Normal" noWrap align="left" width="135"><asp:label id="LabelNonTaxableRetirement" runat="server" cssClass="Text_Normal" Text="Non-Taxable"
					visible="False"></asp:label>-->
			<td height="3"><asp:textbox id="TextboxRetirementNonTaxable" runat="server" value="" width="10" visible="False"
					height="3"></asp:textbox>
                <!--<td class="Text_Normal" noWrap align="left" width="135"><asp:label id="LabelTaxableRetirement" runat="server" cssClass="Text_Normal" Text="Taxable"
					visible="False"></asp:label>-->
				<asp:textbox id="TextboxRetirementTaxable" runat="server" value="" width="10" visible="False"
					height="3"></asp:textbox></td>
        <!--<td class="Text_Normal" noWrap align="left" width="135"><asp:label id="LabelTaxWithheldRetirement" runat="server" cssClass="Text_Normal" Text="Tax-Withheld"
					visible="False"></asp:label>-->
			<td height="3"><asp:textbox id="TextboxRetirementTaxWithheld" runat="server" value="" width="10" visible="False"
					height="3"></asp:textbox>
                <!--<td class="Text_Normal" noWrap align="left" width="135"><asp:label id="LabelNetAmountRetirement" runat="server" cssClass="Text_Normal" Text="Net Amount"
					visible="False"></asp:label>-->
				<asp:textbox id="TextboxRetirementNetAmount" runat="server" value="" width="10" visible="False"
					height="3"></asp:textbox></td>
        <!--<td class="Text_Normal" noWrap align="left" width="135"><asp:label id="LabelNonTaxableSavings" runat="server" cssClass="Text_Normal" Text="Non-Taxable"
					visible="False"></asp:label>-->
			<td height="3"><asp:textbox id="TextboxSavingsNonTaxable" runat="server" value="" width="10" visible="False"
					height="3"></asp:textbox>
                <!--<td class="Text_Normal" noWrap align="left" width="135"><asp:label id="LabelTaxableSavings" runat="server" cssClass="Text_Normal" Text="Taxable" visible="False"></asp:label>-->
				<asp:textbox id="TextboxSavingsTaxable" runat="server" value="" visible="False" width="10" height="3"></asp:textbox></td>
        <!--<td class="Text_Normal" noWrap align="left" width="135"><asp:label id="LabelTaxWithheldSavings" runat="server" cssClass="Text_Normal" Text="Tax-Withheld"
					visible="False"></asp:label>-->
			<td height="3"><asp:textbox id="TextboxSavingsTaxWithheld" runat="server" value="" width="10" visible="False"
					height="3"></asp:textbox>
                <!--<td class="Text_Normal" noWrap align="left" width="135"><asp:label id="LabelNetAmountSavings" runat="server" cssClass="Text_Normal" Text="Net Amount"
					visible="False"></asp:label>-->
				<asp:textbox id="TextboxSavingsNetAmount" runat="server" value="" visible="False" width="10"
					height="3"></asp:textbox></td>
		</tr>
		<tr>
			<td class="Td_InLineBorder" align="left" colSpan="4" height="0"></td>
		</tr>
		<tr>
			<td class="Td_SubText" align="left" colSpan="4" height="10">Total Withdrawal
                <asp:Label id="lblCovidTaxableAmount" runat="server" class="Td_SubText">(COVID Taxable:</asp:Label>
                <asp:Label id="lblDisplayCovidTaxableAmount" runat="server" class="Td_SubText"></asp:Label>
                <asp:Label id="lblCovidNonTaxableAmount" runat="server" class="Td_SubText">, COVID Non-Taxable:</asp:Label>
                <asp:Label id="lblDisplayCovidNonTaxableAmount" runat="server" class="Td_SubText"></asp:Label>)
			</td>
		</tr>
		<tr>
			<td align="left" colSpan="4">
				<div style="OVERFLOW: auto; WIDTH: 100%; POSITION: relative"><asp:datagrid id="DatagridGrandTotal" runat="server" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="False"> <%--START | SR | YRS-AT-4055 |Increased width to fit Consolidated Total table.--%>
						<SelectedItemStyle CssClass="DataGrid_Total"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="DataGrid_Total" height="10"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_Total" height="10"></ItemStyle>
						<HeaderStyle CssClass="DataGrid_Total"></HeaderStyle>
						<Columns>
							<asp:BoundColumn DataField="IsBasicAccount" headertext="      " ItemStyle-Width="19%" Visible="False"></asp:BoundColumn>
							<asp:BoundColumn DataField="AccountType" HeaderText="      " ItemStyle-Width="20%"></asp:BoundColumn>
							<asp:BoundColumn DataField="AccountGroup" HeaderText="AccountGroup" ItemStyle-Width="11%" Visible="False"></asp:BoundColumn>
							<asp:BoundColumn DataField="PlanType" HeaderText="PlanType" ItemStyle-Width="11%" Visible="False"></asp:BoundColumn>
							<asp:BoundColumn DataField="Taxable" HeaderText="Taxable" ItemStyle-Width="11%"></asp:BoundColumn>
							<asp:BoundColumn DataField="Non-Taxable" HeaderText="Non-Taxable" ItemStyle-Width="14%"></asp:BoundColumn>
							<asp:BoundColumn DataField="Interest" HeaderText="Tax Withheld" ItemStyle-Width="11%"></asp:BoundColumn>
							<asp:BoundColumn DataField="Total" HeaderText="Net Amount" ItemStyle-Width="11%"></asp:BoundColumn>
							<asp:BoundColumn DataField="Selected" HeaderText="Selected" ItemStyle-Width="11%" Visible="False"></asp:BoundColumn>
						</Columns>
					</asp:datagrid></div>
			</td>
		</tr>
        <!--Design Changes For Market based Withdrawal Commented By Parveen on 13-Nov-2009 -->
		<!--<tr>
			<td align="left" colSpan="4">
				<div style="OVERFLOW: auto; WIDTH: 690px; POSITION: relative"><asp:datagrid id="DatagridConsolidateTotal" runat="server" CssClass="DataGrid_Grid" Width="690px"
						showheader="False" Visible="false" >
						<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp" height="10"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_NormalStyle_temp" height="10"></ItemStyle>
						<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
					</asp:datagrid></div>
			</td>
		</tr>
				<tr>
			<td align="left" colSpan="4">
				<div style="OVERFLOW: auto; WIDTH: 690px; POSITION: relative"><asp:datagrid id="DataGridAccountContribution" runat="server" CssClass="DataGrid_Grid" visible="false"
						Width="690px">
						<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp" height="10"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_NormalStyle_temp" height="10"></ItemStyle>
						<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
					</asp:datagrid></div>
			</td>
		</tr>
			<tr>
			<td class="Text_Normal" align="left" width="170">Non Taxable &nbsp;</td>
			<td class="Text_Normal" align="left" width="170">Taxable</td>
			<td class="Text_Normal" align="left" width="170">Tax Withheld</td>
			<td class="Text_Normal" align="left" width="170">Rate %</td>
		</tr>-->
		<!--
		<tr>
			<td noWrap align="left"><asp:checkbox id="CheckboxRetirementPlan" runat="server" CssClass="CheckBox_Normal" Text="Retirement Plan"
					AutoPostBack="True" Visible="False"></asp:checkbox></td>
			<td align="left"><asp:checkbox id="CheckboxSavingsPlan" runat="server" CssClass="CheckBox_Normal" Text="Savings Plan"
					AutoPostBack="True" Visible="False" Checked="True"></asp:checkbox></td>
			<td class="Text_Normal" align="left" width="170">Net Amount</td>
			<td class="Text_Normal" align="left" width="170"><asp:textbox id="TextboxNet" runat="server" CssClass="TextBox_Normal"></asp:textbox></td>
		</tr>
		<tr>
			<td class="Td_InLineBorder" align="left" colSpan="4" height="0"></td>
		</tr>
		<tr>
			<td class="Td_SubText" align="left" colSpan="4" height="10">Market Based Withdrawal 
				-
				<asp:label id="LabelMarket" runat="server" cssClass="Text_Normal" visible="True"></asp:label>
			</td>
		</tr>
		<tr>
			<td class="Text_Normal" noWrap align="left" width="135"><asp:label id="LabelFirstInstallment" runat="server" cssClass="Text_Normal" Text="First Inst. Amount"
					visible="False"></asp:label><asp:textbox id="TextboxFirstInstallment" runat="server" CssClass="TextBox_Normal" value="" visible="False"></asp:textbox></td>
			<td class="Text_Normal" noWrap align="left" width="135"><asp:label id="LabelPercentage" runat="server" cssClass="Text_Normal" Text="Percentage" visible="False"></asp:label><asp:textbox id="TextboxPercentage" runat="server" CssClass="TextBox_Normal" value="" visible="False"></asp:textbox></td>
			<td></td>
			<td></td>
		</tr>
		<tr>
			<td class="Text_Normal" noWrap align="left" width="135"><asp:label id="LabelNonTaxableMarket" runat="server" cssClass="Text_Normal" Text="Non-Taxable"
					visible="False"></asp:label><asp:textbox id="TextboxNonTaxableMarket" runat="server" CssClass="TextBox_Normal" value="" visible="False"></asp:textbox></td>
			<td class="Text_Normal" noWrap align="left" width="135"><asp:label id="LabelTaxableMarket" runat="server" cssClass="Text_Normal" Text="Taxable" visible="False"></asp:label><asp:textbox id="TextboxTaxableMarket" runat="server" CssClass="TextBox_Normal" value="" visible="False"></asp:textbox></td>
			<td class="Text_Normal" noWrap align="left" width="135"><asp:label id="LabelTaxWithHeldMarket" runat="server" cssClass="Text_Normal" Text="Tax-Withheld"
					visible="False"></asp:label><asp:textbox id="TextboxTaxWithHeldMarket" runat="server" CssClass="TextBox_Normal" value=""
					visible="False"></asp:textbox></td>
			<td class="Text_Normal" noWrap align="left" width="135"><asp:label id="LabelNetAmount" runat="server" cssClass="Text_Normal" Text="Net Amount" visible="False"></asp:label><asp:textbox id="TextboxNetAmountMarket" runat="server" CssClass="TextBox_Normal" value="" visible="False"></asp:textbox></td>
		</tr>-->
		<!--Design Changes For Market based Withdrawal Commented By Parveen on 13-Nov-2009 -->
		<tr>
			<td align="left" colSpan="4" height="3">&nbsp;
			</td>
		</tr>
		<tr class="Td_ButtonContainer">
			<td class="Text_Normal" align="left" width="170"></td>
			<td class="Text_Normal" align="right" width="170"></td>
			<td class="Text_Normal" align="right" width="170"><asp:button id="ButtonSave" runat="server" Text="Save" Width="80px" cssclass="Button_Normal"></asp:button></td>
			</TD>
			<td class="Td_ButtonContainer" align="right" width="170"><asp:button id="ButtonOK" runat="server" Text="OK" Width="80px" cssclass="Button_Normal"></asp:button></td>
			</TD></tr>
	</table>
	</TD></TR></TBODY></TABLE><asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder>
     <div id="Tooltip" clientidmode="Static" runat="server" style="z-index: 1000; width:auto; border-left: 1px solid silver;
                        border-top: 1px solid silver; border-right: 2px solid black; border-bottom: 1px solid #cccccc;
                        padding: 3px; position: absolute; top: 0; left: 0; background: LightYellow; color: black;
                        display: none; /* does not work in ie6 */	font-size:7px; font-family: verdana; text-align:left;
                        margin: 0; overflow: visible;">
                        <asp:Label runat="server" ID="lblComments" ClientIDMode="Static" Style="display: block; width:auto; overflow: visible;
                            font-size: x-small;"></asp:Label>
                    </div>
</form>
