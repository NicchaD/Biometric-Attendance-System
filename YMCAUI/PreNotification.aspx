<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PreNotification.aspx.vb" Inherits="YMCAUI.PreNotification" %>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="100%">
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" mouseovercssclass="MouseOver" MenuFadeDelay="2" DefaultMouseUpCssClass="mouseup"
					DefaultMouseOverCssClass="mouseover" DefaultMouseDownCssClass="mousedown" DefaultCssClass="menuitem" CssClass="menustyle" Cursor="Pointer"
					HighlightTopMenu="False" Layout="Horizontal">
					<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
				</cc1:menu></td>
		</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif">
				Pre-Notification
			</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
		</tr>
	</table>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="700">
			<TBODY>
				<tr>
					<td align="left" colSpan="2"><asp:label id="labelLookFor" cssclass="Label_Small" Width="120" text="Look For" Runat="server"></asp:label><asp:textbox id="TextBoxLookFor" cssclass="TextBox_Normal" Width="150" Runat="server"></asp:textbox>&nbsp;
						<asp:button id="ButtonGo" CssClass="Button_Normal" Runat="server" width="50" Text="Search"></asp:button>&nbsp;
						<asp:button id="ButtonSelectAll" CssClass="Button_Normal" Runat="server" width="90" Text="Select All"></asp:button>&nbsp;
						<asp:label id="LabelNoRecordFound" runat="server" CssClass="Label_Small" width="130" visible="false">No Record Found</asp:label></td>
				<tr>
					<td>
						<div style="OVERFLOW: auto; WIDTH: 482px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 300px; BORDER-BOTTOM-STYLE: none"><asp:datagrid id="DataGridPreNotification" runat="server" CssClass="DataGrid_Grid" Width="455px"
								allowsorting="true" AutoGenerateColumns="false" ShowHeader="True">
								<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
								<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
								<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
								<Columns>
									<asp:TemplateColumn>
										<ItemTemplate>
											<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
												CommandName="Select" ToolTip="Select Row"></asp:ImageButton>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn HeaderText="SS No." SortExpression="SS No." DataField="SS No.">
										<ItemStyle Width="80px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn HeaderText="Last Name" SortExpression="Last Name" DataField="Last Name">
										<ItemStyle Width="190px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn HeaderText="First Name" SortExpression="First Name" DataField="First Name">
										<ItemStyle Width="150px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn HeaderText="Middle Name" SortExpression="Middle Name" DataField="Middle Name">
										<ItemStyle Width="100px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn HeaderText="EFT Status" SortExpression="EFT Status" DataField="EFT Status">
										<ItemStyle Width="35px"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn HeaderText="UniqueID" SortExpression="UniqueID" visible="false" DataField="UniqueID">
										<ItemStyle Width="35px"></ItemStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn>
										<ItemTemplate>
											<asp:CheckBox id="CheckBoxSelect" Enabled="True" Checked='<%# Databinder.Eval(Container.DataItem, "Selected") %>' runat="server">
											</asp:CheckBox>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
							</asp:datagrid></div>
					</td>
					<td>
						<table>
							<tr>
								<td align="left"><asp:label id="LabelFirstName" cssclass="Label_Small" text="First Name" Runat="server" width="70"></asp:label></td>
								<td><asp:textbox id="TextBoxFirstName" CssClass="TextBox_Normal" Runat="server" width="100"></asp:textbox></td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelLastName" cssclass="Label_Small" text="Last Name" Runat="server" width="70"></asp:label></td>
								<td><asp:textbox id="TextBoxLastName" CssClass="TextBox_Normal" Runat="server" width="100"></asp:textbox></td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelSsNo" cssclass="Label_Small" text="SS No" Runat="server" width="70"></asp:label></td>
								<td><asp:textbox id="TextBoxSSNo" CssClass="TextBox_Normal" Runat="server" width="100"></asp:textbox></td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelActNo" cssclass="Label_Small" text="Account Num." Runat="server" width="70"></asp:label></td>
								<td><asp:textbox id="TextBoxActNo" CssClass="TextBox_Normal" Runat="server" width="100"></asp:textbox></td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelActType" cssclass="Label_Small" text="Account Type" Runat="server" width="70"></asp:label></td>
								<td><asp:textbox id="TextBoxActType" CssClass="TextBox_Normal" Runat="server" width="100"></asp:textbox></td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelBankName" cssclass="Label_Small" text="Bank Name" Runat="server" width="70"></asp:label></td>
								<td><asp:textbox id="TextBoxBankName" CssClass="TextBox_Normal" Runat="server" width="100"></asp:textbox></td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelBankABA" cssclass="Label_Small" text="Bank ABA" Runat="server" width="70"></asp:label></td>
								<td><asp:textbox id="TextBoxBankABA" CssClass="TextBox_Normal" Runat="server" width="100"></asp:textbox></td>
							</tr>
							<tr>
								<td align="left" height="2"><asp:label id="LabelEfT" cssclass="Label_Small" text="EFT Status" Runat="server" width="40"></asp:label></td>
								<td height="2"><asp:dropdownlist id="DropDownListPTStatus" cssclass="DropDown_Normal" Runat="server" width="100"></asp:dropdownlist></td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelMessage" cssclass="Label_Small" text="Message" Runat="server" width="70"></asp:label></td>
								<td><asp:textbox id="TextBoxMessage" CssClass="TextBox_Normal" Runat="server" width="100" rows="3"
										Columns="1" TextMode="MultiLine" Height="70"></asp:textbox></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colSpan="2">
						<div class="Div_Center">
							<table class="Table_WithOutBorder" width="680">
								<TBODY>
									<tr>
										<td class="Td_ButtonContainer"><asp:button id="buttonPreNote" cssclass="Button_Normal" Runat="server" width="124" Text="Print Pre-Note"></asp:button>&nbsp;&nbsp;
											<asp:button id="buttonGeneratePreNote" cssclass="Button_Normal" Width="128" Runat="server" Text="Generate Pre-Note"></asp:button>&nbsp;&nbsp; <%-- MMR | 2016.11.25 | YRS-AT-3213 | Changed the width from 124px to 128px to display whole text in button --%>
											<asp:button id="buttonUpdateStatus" cssclass="Button_Normal" Width="124" Runat="server" Text="Update EFT Status"></asp:button>&nbsp;&nbsp;
										</td>
										<td class="Td_ButtonContainer"><asp:button id="buttonSave" accessKey="S" cssclass="Button_Normal" Width="50" Runat="server"
												Text="Save"></asp:button>&nbsp;&nbsp;
											<asp:button id="buttonCancel" accessKey="C" cssclass="Button_Normal" Width="50" Runat="server"
												Text="Cancel"></asp:button>&nbsp;&nbsp;
											<asp:button id="buttonEdit" accessKey="E" cssclass="Button_Normal" Width="50" Runat="server"
												Text="Edit"></asp:button>&nbsp;&nbsp;
											<asp:button id="ButtonOK" accessKey="O" cssclass="Button_Normal" Width="50" Runat="server" Text="OK"></asp:button></td>
					</td>
				</tr>
			</TBODY></table>
	</div>
	</TD></TR></TBODY></TABLE></DIV>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
	</form>
<!--#include virtual="bottom.html"-->
