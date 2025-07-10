<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="UserControls/YMCA_Footer_WebUserControl.ascx"%>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CashReceiptsEntryForm.aspx.vb" Inherits="YMCAUI.CashReceiptsEntryForm"%>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<HTML>
	<HEAD>
		<script language="javascript">

			var theform;
			var isIE;
			var isNS;

			/*
			Function to detect the Browser type.
			*/
			//Rahul
			function DropDownListSourceFocus() 
			{
				//if(flag == 1)
				//{
					if(event.keyCode == 13)
					{	
						document.getElementById("DropDownListSource").focus();
						event.returnValue=false;
						
					}
				//}
			/*
				if(flag==2)
				{
					if(event.keyCode == 13)
					{	
						document.getElementById("TextBoxComments").focus();
						event.returnValue=false;
						
					}
				}
				if(flag==3)
				{
					if(event.keyCode == 13)
					{	
						
						event.returnValue=false;
						
					}
				}
				if(flag==4)
				{
					if(event.keyCode == 13)
					{	
						document.getElementById("TextBoxAmount").focus();
						event.returnValue=false;
						
					}
				}*/
			}
			function TextBoxCommentsFocus(  ) 
			{
				//if(flag == 1)
				//{
					if(event.keyCode == 13)
					{	
						document.getElementById("TextBoxComments").focus();
						event.returnValue=false;
						
					}

			}
			function TextBoxRefInfoFocus(  ) 
			{
				//if(flag == 1)
				//{
					if(event.keyCode == 13)
					{	
						
						event.returnValue=false;
						
					}

			}

			function  DoSomeFunction()
			{
				//if(flag == 1)
				//{
					
						document.getElementById("TextBoxRefInfo").focus();
						event.returnValue=false;
						
					

		}

			//Rahul*/
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
				var iMaxLen ;
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
			function FormatYMCANo()
			{	
				var diff;
				var flg = false;
				var str = String(document.Form1.all.TextBoxYMCANo.value);
				var len;
				len =str.length;
				if (len==0)
				{
					return false;
				}
				else if (len<6)
				{
					diff=(6-len);
					for (i=0;i<diff;i++)
					{
						str="0" + str
						
					}
				}
				
				document.Form1.all.TextBoxYMCANo.value=str		
			}	

			function TextBoxComments_onkeydown()
			{
				if ((event.which && event.which == 13) || (event.keyCode && event.keyCode=='13')){
					document.Form1.ButtonAdd.click();
					//alert('add click fired');
					//return false;
				}else{
					//alert('add click not fired');
					return true;
				}
			}
		</script>
		<LINK href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<div class="Div_Center">
				<table class="Table_WithoutBorder" cellSpacing="0" cellpadding=0 width="740">
					<tr>
						<td><YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL id="YMCA_Toolbar_WebUserControl1" runat="server" ShowHomeLinkButton="true" ShowLogoutLinkButton="true"></YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL></td>
					</tr>
					<tr>
						<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
								CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
								MenuFadeDelay="2" mouseovercssclass="MouseOver" EnableViewState="False">
								<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
							</cc1:menu></td>
					</tr>
					<tr>
						<td class="Td_HeadingFormContainer" align="left" colSpan="2">Cash Receipts Entry
						</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
					</tr>
				</table>
			</div>
			<div class="Div_Center">
				<table class="Table_WithBorder" height="390" width="740" border="0">
					<tr>
						<td colSpan="2">&nbsp;</td>
					</tr>
					<tr>
						<td vAlign="top" align="left" width="480">
							<div style="OVERFLOW: auto; WIDTH: 472px; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 280px; BORDER-BOTTOM-STYLE: none"><asp:datagrid id="DataGridCashReceipt" CssClass="DataGrid_Grid" Width="454px" Runat="server" Cellpadding="2">
									<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
									<Columns>
										<asp:TemplateColumn>
											<ItemTemplate>
												<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
													CommandName="Select" ToolTip="Select"></asp:ImageButton>
											</ItemTemplate>
										</asp:TemplateColumn>
									</Columns>
								</asp:datagrid></div>
						</td>
						<td vAlign="top">
							<table width="200" border="0">
								<tr>
									<td align="left" width="75"><asp:label id="LabelYMCANo" runat="server" CssClass="Label_Small" width="80">YMCA No</asp:label></td>
									<td align="left"><asp:textbox id="TextBoxYMCANo" runat="server" CssClass="TextBox_Normal" width="80px" maxlength="6"></asp:textbox><asp:requiredfieldvalidator id="ReqYMCANo" runat="server" ErrorMessage="*" ControlToValidate="TextBoxYMCANo"></asp:requiredfieldvalidator></td>
								</tr>
								<tr>
									<td align="left" width="75" height="17"><asp:label id="LabelSource" runat="server" CssClass="Label_Small" width="80">Source</asp:label></td>
									<td align="left"><asp:dropdownlist onkeypress="if (window.event.keyCode=='13') DoSomeFunction();" id="DropDownListSource"
											runat="server" CssClass="DropDown_Normal" width="88px"></asp:dropdownlist></td>
								</tr>
								<tr>
									<td align="left" width="75"><asp:label id="LabelRefInfo" runat="server" CssClass="Label_Small" width="80">Ref Info</asp:label></td>
									<td align="left"><asp:textbox id="TextBoxRefInfo" runat="server" CssClass="TextBox_Normal" width="88px" maxlength="29"></asp:textbox></td>
								</tr>
								<tr>
									<td align="left" width="75"><asp:label id="LabelCheckDate" runat="server" CssClass="Label_Small" Width="80px">Check Date</asp:label></td>
									<td align="left"><uc1:dateusercontrol id="TextBoxCheckDate" runat="server"></uc1:dateusercontrol></td>
								</tr>
								<tr>
									<td noWrap align="left"><asp:label id="LabelRecDate" runat="server" CssClass="Label_Small" width="64px">Recd. Date</asp:label></td>
									<td align="left"><uc1:dateusercontrol id="TextBoxRecDate" runat="server"></uc1:dateusercontrol></td>
								</tr>
								<tr>
									<td align="left" width="75"><asp:label id="LabelAmount" runat="server" CssClass="Label_Small" width="64px">Amount</asp:label></td>
									<td align="left"><asp:textbox id="TextBoxAmount" runat="server" CssClass="TextBox_Normal_Amount" width="106px"
											maxlength="10"></asp:textbox><asp:requiredfieldvalidator id="ReqAmt" runat="server" ErrorMessage="*" ControlToValidate="TextBoxAmount"></asp:requiredfieldvalidator></td>
								</tr>
								<tr>
									<td align="left" width="75"><asp:label id="LabelComments" runat="server" CssClass="Label_Small" width="72px" Height="16px">Comments</asp:label></td>
									<td align="left"><asp:textbox id="TextBoxComments" runat="server" CssClass="TextBox_Normal" width="106px" MaxLength="60"></asp:textbox></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td colSpan="2">
							<table width="690" border="0">
								<tr>
									<td class="Td_ButtonContainer" align="center" colSpan="2"><asp:button id="ButtonAdd" runat="server" CssClass="Button_Normal" Width="55px" Height="20px"
											Text="Add"></asp:button></td>
									<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" Width="55px" Height="20px"
											Text="Save"></asp:button></td>
									<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="55" Height="20"
											Text="Cancel" CausesValidation="False"></asp:button></td>
									<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonPost" runat="server" CssClass="Button_Normal" Width="55px" Height="20px"
											Text="Post" CausesValidation="False" Enabled="False"></asp:button></td>
									<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="55" Height="20" Text="OK"
											CausesValidation="False"></asp:button></td>
									<td class="Td_ButtonContainer" align="left"><asp:textbox id="TextboxTotal" runat="server" CssClass="TextBox_Normal_Amount" width="110px"
											ReadOnly="True"></asp:textbox></td>
									<td class="Td_ButtonContainer" width="65">&nbsp;</td>
									<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonDelete" runat="server" Enabled="false" CssClass="Button_Normal" Width="74"
											Height="20" CausesValidation="False" Text="Delete"></asp:button></td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
				<YRSControls:YMCA_Footer_WebUserControl id="YMCA_Footer_WebUserControl1" runat="server"></YRSControls:YMCA_Footer_WebUserControl></div>
			<script>document.Form1.all.TextBoxYMCANo.focus();</script>
			<asp:placeholder id="PlaceHolderCashEntry" runat="server"></asp:placeholder></form>
	</body>
</HTML>
