<%@ Page Language="vb" AutoEventWireup="false" Codebehind="EDIOutsourceCheckPrintingForm.aspx.vb" Inherits="YMCAUI.EDIOutsourceCheckPrintingForm" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
	<!--#include virtual="top.html"-->
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>EDIOutsourceCheckPrintignForm</title> 
	
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">
			  .DataGridFixedHeader { FONT-WEIGHT: bold; FONT-SIZE: 11px; COLOR: #000066; FONT-FAMILY: tahoma; POSITION: relative; ; TOP: expression(this.offsetParent.scrollTop); HEIGHT: 22px; BACKGROUND-COLOR: #c9dbed; TEXT-ALIGN: left }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="750" align="center">
				<tr align="center">
					<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
							CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
							MenuFadeDelay="2" mouseovercssclass="MouseOver"><SELECTEDMENUITEMSTYLE BackColor="#FBC97A" ForeColor="#3B5386"></SELECTEDMENUITEMSTYLE>
						</cc1:menu></td>
				</tr>
				<tr>
					<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
						EDI File Creation
					</td>
				</tr>
				<tr>
					<td>&nbsp;
					</td>
				</tr>
			</table>
			<div class="Div_Center">
				<table class="Table_WithBorder" width="730" align="center">
					<tr>
						<td>
							<div class="Div_Center">
								<table class="Table_WithoutBorder" width="740">
									<TBODY>
										<tr>
											<td align="left" width="44%"></td>
											<td align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
										</tr>
										<tr>
											<td style="WIDTH: 181px" align="left"><asp:label id="LabelLatestPayroll" runat="server" CssClass="Label_Small"> Latest Payroll</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxLatestPayroll" CssClass="TextBox_Normal" Runat="server" ReadOnly=True></asp:textbox></td>
										</tr>
										<tr>
											<td style="WIDTH: 181px" align="left"><asp:label id="LabelPayrollprocesson" runat="server" CssClass="Label_Small">
													Payroll Processed On</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxPayrollDate" CssClass="TextBox_Normal" Runat="server" ReadOnly=True></asp:textbox></td>
										</tr>
										<tr>
											<td style="WIDTH: 181px" align="left"></td>
											<td align="left"></td>
										</tr>
										<tr>
											<td style="WIDTH: 181px" align="left"></td>
											<td align="left"></td>
										</tr>
										<tr>
											<td align="left" colSpan="2">
												<table>
													<tr>
														<td align="center" width="30%"><asp:button id="ButtonFind" CssClass="Button_Normal" Runat="server" Width="80px" Text="Find"
																Enabled="False"></asp:button></td>
														<td width="20%"><asp:label id="Label1" runat="server" CssClass="Label_Small">Fund Id</asp:label></td>
														<td align="left" width="25%"><asp:textbox id="TextboxFundId" CssClass="TextBox_Normal" Runat="server" MaxLength="10"></asp:textbox></td>
														<td width="15%"><asp:label id="LabelLastName" runat="server" CssClass="Label_Small">Last Name</asp:label></td>
														<td align="left"><asp:textbox id="TextboxLastName" CssClass="TextBox_Normal" Runat="server"></asp:textbox></td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td style="WIDTH: 181px" align="left"></td>
											<td align="left"></td>
										</tr>
										<tr>
											<td colspan="2" align="left"><asp:label id="LabelRecordsFound" runat="server" CssClass="Label_Small" Visible="False"></asp:label></td>
										</tr>
										<tr>
											<td colSpan="2">
												<div id="Datagrid_Delinquency" style="OVERFLOW: auto; WIDTH: 730px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 300px; BORDER-BOTTOM-STYLE: none"><asp:datagrid id="DataGridEDIList" runat="server" CssClass="DataGrid_Grid" allowsorting="true"
														width="730px" PageSize="15" AutoGenerateColumns="False">
														<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
														<Columns>
															<asp:TemplateColumn HeaderText="Exclude">
																<ItemStyle Width="4%"></ItemStyle>
																<ItemTemplate>
																	<asp:CheckBox ID="checkboxSelect" Runat="server" Checked='<%# Databinder.Eval(Container.DataItem, "bitExcluded") %>'>
																	</asp:CheckBox>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn DataField="FundIDNo" SortExpression="FundIDNo" HeaderText="FundId"></asp:BoundColumn>
															<asp:BoundColumn DataField="FirstName" SortExpression="FirstName" HeaderText="First Name"></asp:BoundColumn>
															<asp:BoundColumn DataField="LastName" SortExpression="LastName" HeaderText="Last Name"></asp:BoundColumn>
															<asp:BoundColumn DataField="NetAmount" SortExpression="NetAmount" HeaderText="Net Amount"></asp:BoundColumn>
															<asp:BoundColumn DataField="Addr1" SortExpression="Addr1" HeaderText="Address"></asp:BoundColumn>
															<asp:BoundColumn DataField="City" SortExpression="City" HeaderText="City"></asp:BoundColumn>
															<asp:BoundColumn DataField="StateType" SortExpression="StateType" HeaderText="State"></asp:BoundColumn>
															<asp:BoundColumn DataField="Zip" SortExpression="Zip" HeaderText="Zip"></asp:BoundColumn>
															<asp:BoundColumn DataField="Country" SortExpression="Country" HeaderText="Country"></asp:BoundColumn>
															<asp:BoundColumn DataField="DisbursmentID" SortExpression="DisbursmentID" HeaderText="DisbursmentID" Visible=False></asp:BoundColumn>
															
														</Columns>
														<PagerStyle Mode="NumericPages"></PagerStyle>
													</asp:datagrid></div>
										<tr>
											<td style="WIDTH: 181px" align="left"></td>
											<td align="left"></td>
										</tr>
									</TBODY>
								</table>
							</div>
						</td>
					</tr>
					<tr>
						<td>
							<table class="Table_WithoutBorder" width="740">
								<tr>
									<td class="Td_ButtonContainer" align="center" width="30%"><asp:button id="ButtonStartProcess" CssClass="Button_Normal" Runat="server" Width="100px" Text="Start Process"
											Enabled="False"></asp:button></td>
									<td class="Td_ButtonContainer" align="center" width="30%"><asp:button id="ButtonReport" CssClass="Button_Normal" Runat="server" Width="100px" Text="Show Report"
											Enabled="False"></asp:button></td>
									<td class="Td_ButtonContainer" align="center" width="30%"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Text="OK" Width="72px"></asp:button></td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</div>
			<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
			<!--#include virtual="bottom.html"--></form>
	</body>
</HTML>
