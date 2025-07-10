<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx"%>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="UserControls/YMCA_Footer_WebUserControl.ascx"%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UnFundingTransmittalForm.aspx.vb" Inherits="YMCAUI.UnFundingTransmittalForm" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<TITLE>YMCA YRS</TITLE>
		<script language="javascript">
			function FormatYMCANo()
			{
				var diff;
				var flg = false;
				var str = String(document.Form1.all.TextBoxYmcaNo.value);
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
				document.Form1.all.TextBoxYmcaNo.value=str		
			}
		</script>
		<LINK href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
	</HEAD>
	<BODY>
		<FORM id="Form1" method="post" runat="server">
			<div class="Div_Center">
				<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="740">
					<tr>
						<td><YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL id="YMCA_Toolbar_WebUserControl1" runat="server" ShowLogoutLinkButton="true" ShowHomeLinkButton="true"></YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL></td>
					</tr>
					<tr>
						<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
								CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
								MenuFadeDelay="2" mouseovercssclass="MouseOver">
								<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
							</cc1:menu></td>
					</tr>
					<tr>
						<td class="Td_HeadingFormContainer" align="left" colSpan="2"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
							<asp:label id="LabelTitle" runat="server" Width="592px" cssClass="Td_HeadingFormContainer"></asp:label></td>
						<td></td>
					</tr>
					<tr>
						<td>&nbsp;
						</td>
					</tr>
				</table>
			</div>
			<div class="Div_Center">
				<table class="Table_WithBorder" width="740">
					<tr>
						<td align="left"></td>
					</tr>
					<tr>
						<td align="center">
							<table class="Table_WithBorder" width="740">
								<tr vAlign="top" bgColor="#93beee">
									<td colSpan="3" Class="td_Text">
										Transmittals Search
									</td>
								</tr>
								<tr vAlign="top">
									<td vAlign="top"><asp:label id="LabelRecordNotFound" CssClass="Label_Small" Runat="server" visible="false">No Matching Records</asp:label>
										<DIV style="OVERFLOW: auto; WIDTH: 400px; HEIGHT: 170px; TEXT-ALIGN: left"><asp:datagrid id="DataGridUnFundDelSearchResult" CssClass="DataGrid_Grid" Width="400" Runat="server"
												AutoGenerateColumns="false" CellSpacing="0" CellPadding="0" AllowSorting="True" OnSortCommand="DataGridUnFundSearchResult_SortCommand">
												<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
												<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
												<Columns>
													<asp:BoundColumn Visible="False" DataField="YmcaId" HeaderText="YmcaId"></asp:BoundColumn>
													<asp:TemplateColumn>
														<ItemTemplate>
															<asp:CheckBox id="Checkbox1" runat="server" CssClass="CheckBox_Normal"></asp:CheckBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="YMCANo" SortExpression="YMCANo" HeaderText="YMCA No">
														<HeaderStyle Wrap="False"></HeaderStyle>
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="TransmittalNo" SortExpression="TransmittalNo" HeaderText="Transmittal No">
														<HeaderStyle Width="110px"></HeaderStyle>
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="TransmittalDate" SortExpression="TransmittalDate" HeaderText="Transmittal Date"
														DataFormatString="{0:MM/dd/yyyy}">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="AmtDue" SortExpression="AmtDue" HeaderText="Transmittal Amount" DataFormatString="{0:N}">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="AppliedReceipts" SortExpression="AppliedReceipts" HeaderText="Receipt Applied "
														DataFormatString="{0:N}">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="AppliedCredit" SortExpression="AppliedCredit" HeaderText="CR Applied"
														DataFormatString="{0:N}">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="UniqueId" HeaderText="UniqueIdYmcaID"></asp:BoundColumn>
												</Columns>
											</asp:datagrid></DIV>
									</td>
									<td colSpan="2" align=right>
										<table border=0>
											<tr>
												<td width="140"><asp:label id="LabelYmcaNo" CssClass="Label_Small" Runat="server">YMCA No.</asp:label></td>
												<td><asp:textbox id="TextBoxYmcaNo" CssClass="TextBox_Normal" Width="100" Runat="server" MaxLength="6"></asp:textbox></td>
											</tr>
											<tr>
												<td width="140"><asp:label id="LabelTransmittalNo" CssClass="Label_Small" Runat="server">Transmittal No.</asp:label></td>
												<td><asp:textbox id="TextBoxTransmittalNo" runat="server" CssClass="TextBox_Normal" MaxLength="60"
														width="100"></asp:textbox></td>
											</tr>
											<tr>
												<td width="140"><asp:label id="LabelTransmittalSDate" CssClass="Label_Small" Runat="server">Transmittal Start Date</asp:label></td>
												<td><uc1:dateusercontrol id="TextBoxSTransmittalDate" runat="server"></uc1:dateusercontrol></td>
											</tr>
											<tr>
												<td width="140"><asp:label id="LabelTransmittalEDate" CssClass="Label_Small" Runat="server">Transmittal End Date</asp:label></td>
												<td><uc1:dateusercontrol id="TextboxETransmittalDate" runat="server"></uc1:dateusercontrol></td>
											</tr>
											<tr>
												<td width="140"><asp:label id="LabelReceiptNo" CssClass="Label_Small" Runat="server">Receipt No.</asp:label></td>
												<td><asp:textbox id="TextboxRecptNo" CssClass="TextBox_Normal" Width="100" Runat="server"></asp:textbox></td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td align="center"><asp:button id="ButtonAddToList" CssClass="Button_Normal" Width="80" Runat="server" Text="Add to List"></asp:button></td>
									<td align="center"><asp:button id="ButtonFind" CssClass="Button_Normal" Width="80" Runat="server" Text="Find"></asp:button></td>
									<td><asp:button id="ButtonClear" CssClass="Button_Normal" Width="80" Runat="server" Text="Clear"></asp:button></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td align="center">
							<table class="Table_WithBorder" width="100%" border=0>
								<tr vAlign="top">
									<td vAlign="middle" colSpan="3" class="td_Text">
										<asp:label id="LabelSelectedList" runat="server" cssClass="td_Text" Height="20px"></asp:label>
									</td>
								</tr>
								<tr vAlign="top">
									<td align="center">
										<div style="OVERFLOW: auto; WIDTH: 680px; HEIGHT: 200px; TEXT-ALIGN: left"><asp:datagrid id="Datagrid_UnFundDelete_TranList" CssClass="DataGrid_Grid" Width="680px" Runat="server"
												AutoGenerateColumns="False" OnSortCommand="Datagrid_UnFundDelete_TranList_SortCommand" OnItemCommand="Datagrid_UnFundDelete_TranList_ItemCommand" allowsorting="True">
												<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
												<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
												<Columns>
													<asp:BoundColumn Visible="False" DataField="YmcaId" HeaderText="YmcaId"></asp:BoundColumn>
													<asp:TemplateColumn>
														<ItemTemplate>
															<asp:CheckBox id="CheckBoxSelect" runat="server" CssClass="CheckBox_Normal"></asp:CheckBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<ItemTemplate>
															<asp:ImageButton ToolTip="Remove Transmittal" id="ImageButtonRemove" runat="server" CommandName="Remove"
																ImageUrl="images/delete.gif"></asp:ImageButton>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="YMCANo" HeaderText="YMCA No" SortExpression="YMCANo">
														<HeaderStyle Wrap="False"></HeaderStyle>
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="TransmittalNo" HeaderText="Transmittal No" SortExpression="TransmittalNo">
														<HeaderStyle Width="110px"></HeaderStyle>
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="TransmittalDate" HeaderText="Transmittal Date" DataFormatString="{0:MM/dd/yyyy}"
														SortExpression="TransmittalDate">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="AmtDue" HeaderText="Transmittal Amount" DataFormatString="{0:N}" SortExpression="AmtDue">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="AppliedReceipts" HeaderText="Receipt Applied " DataFormatString="{0:N}"
														SortExpression="AppliedReceipts">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="AppliedCredit" HeaderText="CR Applied" DataFormatString="{0:N}" SortExpression="AppliedCredit">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn Visible="false" DataField="UniqueId" HeaderText="UniqueIdYmcaID"></asp:BoundColumn>
												</Columns>
											</asp:datagrid></div>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td>
							<table class="Table_WithBorder" width="740">
								<tr>
									<TD class="Td_ButtonContainer" align="center" width="50%"><asp:button id="ButtonUnFund" runat="server" CssClass="Button_Normal" Width="150px" Text="Un-Fund"
											Visible="False"></asp:button><asp:button id="ButtonDelete" runat="server" CssClass="Button_Normal" Width="150px" Text="Delete"
											Visible="False"></asp:button>&nbsp;
									</TD>
									<TD class="Td_ButtonContainer" align="center" width="50%"><asp:button id="ButtonOk" runat="server" CssClass="Button_Normal" Width="80" Text="OK"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
								</tr>
							</table>
						</td>
					</tr>
					</TD></TR></table>
				<YRSCONTROLS:YMCA_FOOTER_WEBUSERCONTROL id="YMCA_Footer_WebUserControl1" runat="server"></YRSCONTROLS:YMCA_FOOTER_WEBUSERCONTROL><br>
			</div>
			<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder></FORM>
	</BODY>
</HTML>
