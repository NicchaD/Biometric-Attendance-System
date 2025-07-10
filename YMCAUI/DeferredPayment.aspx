<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DeferredPayment.aspx.vb" Inherits="YMCAUI.DeferredPayment"%>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="UserControls/YMCA_Footer_WebUserControl.ascx"%>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx"%>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<HTML>
	<HEAD>
		<TITLE>YMCA YRS</TITLE>
		<LINK href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
			<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
			<Script language="JavaScript">
function ValidateNumeric()
{
if ((event.keyCode < 48)||(event.keyCode > 57))
	{
		event.returnValue = false;
	}	

}

function Openpopup(formname)
{	
	
	var a;
	if(a==null ||a.closed || a.name==undefined)
	{	a=window.open(formname, 'YMCAYRSDEFERRED', 'width=800,height=650,menubar=no,status=Yes,Resizable=Yes,top=0,left=25,scrollbars=yes');
		//a.location.reload();a.focus();
	}
}
                      
			</Script>
	</HEAD>
	<BODY>
		<form id="Form1" method="post" runat="server">
			<table class="Table_WithoutBorder" align="center"  width="700">
				<tr>
					<td align="center"><YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL id="YMCA_Toolbar_WebUserControl1" runat="server" ShowHomeLinkButton="true" ShowLogoutLinkButton="true"></YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL></td>
				</tr>
				<tr>
					<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
							CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
							MenuFadeDelay="2" mouseovercssclass="MouseOver" EnableViewState="False">
							<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
						</cc1:menu>
					</td>
				</tr>
				<tr>
					<td class="Td_HeadingFormContainer" align="left">Deferred Payment</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
			</table>
			<table class="Table_WithBorder" cellSpacing="0" align="center" height="305" cellPadding="0"
				width="720">
				<TR>
					<td align="center">
						<TABLE class="Table_WithOutBorder" id="Table1" height="185" cellSpacing="0" cellPadding="0"
							width="720">
							<TBODY>
								<tr>
									<td valign="top" align="center" >
										<div style="OVERFLOW: auto; WIDTH: 700px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 250px; BORDER-BOTTOM-STYLE: none">
											<asp:datagrid id="DataGridDeferredPayment" runat="server" CssClass="DataGrid_Grid" Width="690px"
												AutoGenerateColumns="false" allowsorting="True">
												<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
												<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
												<Columns>
													<asp:TemplateColumn>
														<ItemStyle></ItemStyle>
														<ItemTemplate>
															<asp:CheckBox id="CheckBoxSelect" Checked='<%# Databinder.Eval(Container.DataItem, "Selected") %>' Enabled="True" runat="server">
															</asp:CheckBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="intFundIdNo" SortExpression="intFundIdNo" HeaderText="FundId No">
														<HeaderStyle Width="50px" />
													</asp:BoundColumn>
													<asp:BoundColumn DataField="dtmScheduledPaymentDate" SortExpression="dtmScheduledPaymentDate" DataFormatString="{0:dd/MM/yyyy}"
														HeaderText="Schedule Date">
														<HeaderStyle Width="60px" />
													</asp:BoundColumn>
													<asp:BoundColumn DataField="chvRolloverOptions" SortExpression="chvRolloverOptions" HeaderText="Option">
														<ItemStyle></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="intInstPercentage" SortExpression="intInstPercentage" HeaderText="Perc%">
														<ItemStyle></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="chvLastName" SortExpression="chvLastName" HeaderText="Last Name">
														<ItemStyle></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="chvFirstName" SortExpression="chvFirstName" HeaderText="First Name"></asp:BoundColumn>
													<asp:BoundColumn DataField="mnyParticipantInsAmt" SortExpression="mnyParticipantInsAmt" HeaderText="Amount"
														DataFormatString="{0:N}">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="chvRolloverInstitutionName" SortExpression="chvRolloverInstitutionName"
														HeaderText="Payee2"></asp:BoundColumn>
													<asp:BoundColumn DataField="mnyRollOverInsAmt" SortExpression="mnyRollOverInsAmt" HeaderText="Amount"
														DataFormatString="{0:N}">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:TemplateColumn>
														<ItemStyle></ItemStyle>
														<ItemTemplate>
															<a href="#" onclick="javascript:Openpopup('DeferredPaymentDetail.aspx?RefID=<%# Convert.ToString(Databinder.Eval(Container.DataItem, "guiRefRequestID")) %>&InstID=<%# Convert.ToString(Databinder.Eval(Container.DataItem, "intInstallmentID")) %>&FundIdNo=<%# Convert.ToString(Databinder.Eval(Container.DataItem, "intFundIdNo")) %>')" >
																&nbsp;Edit</a>
														</ItemTemplate>
												</asp:TemplateColumn>
												<asp:BoundColumn Visible="False" DataField="guiRefRequestID" HeaderText="guiRefRequestID">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="guiFundEventID" HeaderText="guiDefPaymentID">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="intInstallmentID" HeaderText="intInstallmentID">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
												</Columns>
											</asp:datagrid></div>
									</td>
								</tr>
							</TBODY></TABLE>
					</td>
				</TR>
				<TR>
					<TD>
						<TABLE class="Table_WithOutBorder" width="720" >
							<tr vAlign="top">
								<td width="20%"></td>
								<td width="16%"></td>
								<td width="16%"></td>
								<td width="16%"></td>
								<td width="16%"></td>
								<td width="16%"></td>
							</tr>
							<tr>
								<td>&nbsp;</td>
								<td align="right">Look For&nbsp;</td>
								<td><asp:textbox id="TextBoxFundIdNo" runat="server" cssclass="TextBox_Normal" Width="100px" MaxLength="9"></asp:textbox>&nbsp;</td>
								<td><asp:button id="ButtonFind" runat="server" CssClass="Button_Normal" Text="Fund Id" Width="60px"
										enabled="False"></asp:button></td>
								<td align="right"><asp:button id="ButtonRecalculate" runat="server" CssClass="Button_Normal" Text="Recalculate"
										Width="88px" enabled="False"></asp:button></td>
								<td><asp:textbox id="txtRecalculateTotal" runat="server" cssclass="TextBox_Normal" Width="100px"
										ReadOnly="True" MaxLength="9"></asp:textbox>&nbsp;</td>
							</tr>
						</TABLE>
					</TD>
				</TR>
			</table>
			<table align="center" class="Table_WithOutBorder" width="720">
				<tr>
					<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonSelectNone" CssClass="Button_Normal" Text="Select None" Runat="server"
							Width="85"></asp:button></td>
					<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonPrint" accessKey="P" CssClass="Button_Normal" Text="Print" Runat="server"
							Width="80" Enabled="False"></asp:button></td>
					<TD class="Td_ButtonContainer">&nbsp;<asp:button id="ButtonPay" CssClass="Button_Normal" Text="Pay" Runat="server" Width="80"></asp:button></TD>
					<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonCancel" accessKey="C" CssClass="Button_Normal" Text="Cancel" Runat="server"
							Width="80"></asp:button></td>
				</tr>
			</table>
			<YRSCONTROLS:YMCA_FOOTER_WEBUSERCONTROL id="YMCA_Footer_WebUserControl1" runat="server"></YRSCONTROLS:YMCA_FOOTER_WEBUSERCONTROL></TABLE>
			<asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder>
		</form>
	</BODY>
</HTML>
