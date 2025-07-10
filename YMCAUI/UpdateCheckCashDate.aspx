<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UpdateCheckCashDate.aspx.vb" Inherits="YMCAUI.UpdateCheckCashDate"%>
<!--#include virtual="top.html"-->
<script language="javascript">
		<!--
function Restrict()
{
	if( event.keyCode == 8 )
	{
		return false;
	}
	return true;
}
		-->
</script>
<!--<style type="text/css">
.DataGridFixedHeader { FONT-WEIGHT: bold; FONT-SIZE: 11px; COLOR: #000066; FONT-FAMILY: tahoma; POSITION: relative; ; TOP: expression(this.offsetParent.scrollTop - 3); HEIGHT: 22px; BACKGROUND-COLOR: #c9dbed; TEXT-ALIGN: left }
</style>-->
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="740">
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
					CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
					MenuFadeDelay="2" mouseovercssclass="MouseOver">
					<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
				</cc1:menu></td>
		</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="left">Import Check Cashed Dates
				<asp:label id="LabelTitle" runat="server" Width="432px"></asp:label></td>
		</tr>
		<tr>
			<td height="5"></td>
		</tr>
	</table>
	<div class="Div_Center">
		<table cellSpacing="0" cellPadding="0" width="740">
			<TBODY>
				<tr>
					<td>
						<div class="Div_Center">
							<table class="Table_WithBorder" width="740">
								<tr>
									<td>
										<table width="690">
											<tr>
												<td align="left">
													<p><asp:label id="err_lbl" runat="server" ForeColor="Red"></asp:label></p>
												</td>
											</tr>
											<tr>
												<td>
													<table width="100%" border="0">
														<tr>
														</tr>
														<tr>
															<td align="center">Import&nbsp;File:</td>
															<td align="left"><INPUT id="FileField" onkeydown="return Restrict();" onbeforeeditfocus="return false;"
																	type="file" name="FileField" runat="server" enableviewstate="true">&nbsp;&nbsp;
																<asp:button id="ButtonImport" runat="server" CssClass="Button_Normal" Width="80px" Height="21px"
																	causesValidation="false" Text="Import"></asp:button>&nbsp;&nbsp;
															</td>
															<!--<asp:checkbox id="first_row" runat="server" Text="First Row Contains Headers" Checked="true"></asp:checkbox><br>--></tr>
													</table>
												</td>
											</tr>
											<tr>
												<td vAlign="bottom" align="center">
													<table>
														<tr>
															<br>
															<br>
															<td><asp:label id="Label1" runat="server" Visible="False">No Matching Records</asp:label>
																<DIV id="Div1" style="OVERFLOW: auto; WIDTH: 650px; HEIGHT: 185px; TEXT-ALIGN: left"
																	runat="server"><asp:datagrid id="DataGrid_Check" runat="server" CssClass="DataGrid_Grid " Width="630" AllowSorting="True"
																		allowPaging="False" PageSize="500" AutoGenerateColumns="false" EnableViewState="False" ShowHeader="true">
																		<HeaderStyle cssclass="DataGrid_HeaderStyle "></HeaderStyle>
																		<ItemStyle CssClass="DataGrid_NormalStyle "></ItemStyle>
																		<selectedItemStyle></selectedItemStyle>
																		<Columns>
																			<asp:TemplateColumn>
																				<ItemTemplate>
																					<asp:CheckBox ID="CheckBoxSelect" Runat="server" EnableViewState="False"></asp:CheckBox>
																				</ItemTemplate>
																			</asp:TemplateColumn>
																			<asp:BoundColumn DataField="col000" HeaderText="Fund ID" SortExpression="col000"></asp:BoundColumn>
																			<asp:BoundColumn DataField="col001" HeaderText="Participant Name" SortExpression="col001"></asp:BoundColumn>
																			<asp:BoundColumn DataField="col002" HeaderText="Check Date" DataFormatString="{0:MM/dd/yyyy}" SortExpression="col002"></asp:BoundColumn>
																			<asp:BoundColumn DataField="col003" HeaderText="Net Amount" SortExpression="col003"></asp:BoundColumn>
																			<asp:BoundColumn DataField="col004" HeaderText="Check Number" SortExpression="col004"></asp:BoundColumn>
																			<asp:BoundColumn DataField="guiUniqueIds" HeaderText="gui UniqueId" SortExpression="guiUniqueIds"
																				Visible="False"></asp:BoundColumn>
																		</Columns>
																	</asp:datagrid><asp:label id="lbl_Search_MoreItems" runat="server" CssClass="Message" Visible="False" EnableViewState="False"></asp:label></DIV>
															</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td>&nbsp;</td>
											</tr>
											<tr>
												<td align="left">&nbsp;&nbsp;
													<asp:button id="ButtonSelectAll" runat="server" CssClass="Button_Normal" Width="80" Text="Select All"
														tooltip="Select All Records."></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:button id="ButtonSelectNone" runat="server" CssClass="Button_Normal" Width="90" Text="Select None"
														tooltip="Uncheck all records."></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:button id="ButtonSelectMatching" runat="server" CssClass="Button_Normal" Width="110" Text="Select Matching"
														tooltip="Make default selection."></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" Width="80" Text="Save" tooltip="Process selected record."></asp:button></td>
											</tr>
											<tr>
												<td>&nbsp;</td>
											</tr>
											<tr id="trNote" runat="server">
												<td align="left">
													<table>
														<tr>
															<td>&nbsp;&nbsp;</td>
															<td bgcolor="#faf8cc" width="3%" align="left"></td>
															<td Class="Td_BottomNoteContainer" align="left">&nbsp;Indicates check numbers that 
																were not found in the system.</td>
														</tr>
														<tr>
															<td>&nbsp;&nbsp;</td>
															<td bgcolor="#82cafa" width="3%" align="left"></td>
															<td Class="Td_BottomNoteContainer" align="left">&nbsp;Indicates check numbers that 
																have multiple disbursement records in the system</td>
														</tr>
														<tr>
															<td>&nbsp;&nbsp;</td>
															<td bgcolor="#f9966b" width="3%" height="5%" align="left"></td>
															<td Class="Td_BottomNoteContainer" align="left">&nbsp;Indicates records where 
																either the Amount, Issued Date or the Fund Id number does not match with the 
																data in the system.</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
											</tr>
											<tr>
												<td>&nbsp;</td>
											</tr>
										</table>
										<table width="730">
											<tr>
												<td class="Td_ButtonContainer" align="right"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="80" Text="Cancel"></asp:button></td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</div>
					</td>
				</tr>
			</TBODY></table>
	</div>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder></form>
<!--#include virtual="bottom.html"-->
