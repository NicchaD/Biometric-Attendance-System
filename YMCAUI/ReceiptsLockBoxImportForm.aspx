<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="UserControls/YMCA_Footer_WebUserControl.ascx"%>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx"%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReceiptsLockBoxImportForm.aspx.vb" Inherits="YMCAUI.ReceiptsLockBoxImportForm"%>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
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
			
			//Function Made by ashutosh***********
			function CompareDate(strFromDate,strToDate)
			{

				strFromDate=document.getElementById(strFromDate).value
				strToDate=document.getElementById(strToDate).value
				//alert(strFromDate)
				var date1=new Date(strFromDate);
				var date2=new Date(strToDate);
				//alert(date1)
				if(date1 > date2)
				{
					alert('From Date Cannot Be Greater Than To date')
				}	
				else
					return true;
				
			//}
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
			function ValidateNumeric()
			{	
				if ((event.keyCode < 48)||(event.keyCode > 57))
				{
					event.returnValue = false;
				}
			}
		</script>
		<LINK href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<div class="Div_Center">
				<table width="740" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td>
							<YRSControls:YMCA_Toolbar_WebUserControl id="YMCA_Toolbar_WebUserControl1" runat="server" ShowLogoutLinkButton="true" ShowHomeLinkButton="true"></YRSControls:YMCA_Toolbar_WebUserControl>
						</td>
					</tr>
					<tr>
						<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" mouseovercssclass="MouseOver" MenuFadeDelay="2" DefaultMouseUpCssClass="mouseup"
								DefaultMouseOverCssClass="mouseover" DefaultMouseDownCssClass="mousedown" DefaultCssClass="menuitem" CssClass="menustyle" Cursor="Pointer"
								HighlightTopMenu="False" Layout="Horizontal">
								<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
							</cc1:menu></td>
					</tr>
					<tr>
						<td class="Td_HeadingFormContainer" align="left">
							Receipts Lock Box Import
						</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td align="left"><INPUT id="FileField" type="file" runat="server">&nbsp;&nbsp;
							<asp:button id="ButtonFileSelect" runat="server" CssClass="Button_Normal" Width="71px" Height="20px"
								Text="Import" causesValidation="false"></asp:button>&nbsp;&nbsp;
						</td>
					</tr>
				</table>
				<table class="Table_WithBorder" width="740" border="0" height="330">
					<tr>
						<td align="left" valign="top">
							<div style="OVERFLOW: auto; WIDTH: 470px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 270px; BORDER-BOTTOM-STYLE: none">
								<asp:datagrid id="DataGridLockBoxReceipts" CssClass="DataGrid_Grid" Width="580px" Autogeneratecolumns="false"
									Runat="server">
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<Columns>
										<asp:TemplateColumn HeaderText="Select">
											<ItemTemplate>
												<asp:CheckBox ID="chkFlag" autopostback="true" OnCheckedChanged="Check_Clicked" Runat="server"></asp:CheckBox>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn>
											<ItemTemplate>
												<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
													CommandName="Select" ToolTip="Select"></asp:ImageButton>
												<ItemStyle HorizontalAlign="Left" width="10"></ItemStyle>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="YMCANo" HeaderText="YMCANo">
											<ItemStyle HorizontalAlign="Left" width="50"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Name" HeaderText="Name">
											<ItemStyle HorizontalAlign="Left" width="80"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Source" HeaderText="Source">
											<ItemStyle HorizontalAlign="Left" width="40"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="RefInfo" HeaderText="RefInfo">
											<ItemStyle HorizontalAlign="Left" width="40"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PayDate" HeaderText="PayDate">
											<ItemStyle HorizontalAlign="Left" width="60"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="RecDate" HeaderText="RecDate">
											<ItemStyle HorizontalAlign="Left" width="60"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Amount" HeaderText="Amount">
											<ItemStyle HorizontalAlign="Right" width="70"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Code" HeaderText="Code">
											<ItemStyle HorizontalAlign="Left" width="40"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="YmcaId" HeaderText="YmcaId">
											<ItemStyle HorizontalAlign="Left" width="40"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Comments" HeaderText="Comments">
											<ItemStyle HorizontalAlign="Left" width="40"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="ABANum" HeaderText="ABANum">
											<ItemStyle HorizontalAlign="Left" width="65"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="AcctNum" HeaderText="AcctNum">
											<ItemStyle HorizontalAlign="Left" width="65"></ItemStyle>
										</asp:BoundColumn>
									</Columns>
								</asp:datagrid></div>
						</td>
						<td align="right" valign="top">
							<table border="0">
								<tr>
									<td align="left"><asp:label id="LabelYMCANo" runat="server" CssClass="Label_Small">YMCA No</asp:label></td>
									<td align="left"><asp:textbox id="TextBoxYMCANo" runat="server" CssClass="TextBox_Normal" width="100" enabled="false"></asp:textbox><asp:requiredfieldvalidator id="ReqYMCANo" runat="server" ErrorMessage="*" ControlToValidate="TextBoxYMCANo"></asp:requiredfieldvalidator></td>
								</tr>
								<tr>
									<td align="left"><asp:label id="LabelSource" runat="server" CssClass="Label_Small">Source</asp:label></td>
									<td align="left"><asp:dropdownlist id="DropDownListSource" runat="server" CssClass="DropDown_Normal" width="108" enabled="false"></asp:dropdownlist></td>
								</tr>
								<tr>
									<td align="left"><asp:label id="LabelRefInfo" runat="server" CssClass="Label_Small">Ref Info</asp:label></td>
									<td align="left"><asp:textbox id="TextBoxRefInfo" runat="server" CssClass="TextBox_Normal" width="100" enabled="false"
											maxlenth="30"></asp:textbox><asp:requiredfieldvalidator id="ReqRefInfo" runat="server" ErrorMessage="*" ControlToValidate="TextBoxRefInfo"></asp:requiredfieldvalidator></td>
								</tr>
								<tr>
									<td vAlign="baseline" align="left"><asp:label id="LabelPayDate" runat="server" CssClass="Label_Small" Width="64px">Pay Date</asp:label></td>
									<td align="left"><uc1:dateusercontrol id="TextBoxPayDate" runat="server"></uc1:dateusercontrol></td>
								</tr>
								<tr>
									<td vAlign="baseline" align="left"><asp:label id="LabelRecDate" runat="server" CssClass="Label_Small">Rec Date</asp:label></td>
									<td align="left"><uc1:dateusercontrol id="TextBoxRecDate" runat="server"></uc1:dateusercontrol></td>
								</tr>
								<tr>
									<td align="left"><asp:label id="LabelAmount" runat="server" CssClass="Label_Small">Amount</asp:label></td>
									<td align="left"><asp:textbox id="TextBoxAmount" runat="server" CssClass="TextBox_Normal_Amount" width="100" enabled="false"
											maxlength="10" Height="20px"></asp:textbox><asp:requiredfieldvalidator id="ReqAmount" runat="server" ErrorMessage="*" ControlToValidate="TextBoxAmount"></asp:requiredfieldvalidator></td>
								</tr>
								<tr>
									<td align="left"><asp:label id="LabelComments" runat="server" CssClass="Label_Small">Comments</asp:label></td>
									<td align="left"><asp:textbox id="TextBoxComments" runat="server" CssClass="TextBox_Normal" width="100" enabled="false"></asp:textbox></td>
								</tr>
								<tr>
									<td align="left"><asp:label id="LabelABANum" runat="server" CssClass="Label_Small">ABANum</asp:label></td>
									<td align="left"><asp:textbox id="TextBoxABANum" runat="server" CssClass="TextBox_Normal" width="100" enabled="false"></asp:textbox></td>
								</tr>
								<tr>
									<td align="left"><asp:label id="LabelAcctNum" runat="server" CssClass="Label_Small">AcctNum</asp:label></td>
									<td align="left"><asp:textbox id="TextBoxAcctNum" runat="server" CssClass="TextBox_Normal" width="100" enabled="false"></asp:textbox></td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
				<table border="0">
					<tr>
						<td>
							<table class="Table_WithBorder" width="740">
								<tr>
									<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonSelectAll" runat="server" CssClass="Button_Normal" Width="90px" Height="20px"
											Text="Select All" CausesValidation="False"></asp:button></td>
									<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" Width="71px" Height="20px"
											Text="Save"></asp:button></td>
									<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="72px" Height="20px"
											Text="Cancel" causesvalidation="false"></asp:button></td>
									<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonPost" runat="server" CssClass="Button_Normal" Width="72" Height="20" Text="Post"
											causesvalidation="false"></asp:button></td>
									<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonOk" runat="server" CssClass="Button_Normal" Width="71px" Height="20px"
											Text="OK" causesvalidation="false"></asp:button></td>
									<td class="Td_ButtonContainer" align="center"><asp:textbox id="TextBoxGeneral" runat="server" CssClass="TextBox_Normal_Amount" width="100"
											Height="20px" readonly="true"></asp:textbox></td>
									<td class="Td_ButtonContainer"><asp:button id="ButtonAdd" runat="server" CssClass="Button_Normal" Width="72px" Text="Add" causesvalidation="false"></asp:button></td>
									<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonDelete" runat="server" CssClass="Button_Normal" Width="74" Height="20"
											Text="Delete" CausesValidation="False"></asp:button></td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
				<YRSControls:YMCA_Footer_WebUserControl id="YMCA_Footer_WebUserControl1" runat="server"></YRSControls:YMCA_Footer_WebUserControl>
			</div>
			<asp:placeholder id="PlaceHolderLockBox" runat="server"></asp:placeholder>
		</form>
	</body>
</HTML>
