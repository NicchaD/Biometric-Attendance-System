<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="EmployeeEventsWebForm.aspx.vb" Inherits="YMCAUI.EmployeeEventsWebForm"%>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<table cellSpacing="0" cellPadding="0" width="700" class="Table_WithoutBorder">
		<tr>
			<td class="td_backgroundcolorwhite" colSpan="2"></td>
		</tr>
		<tr>
			<td class="td_backgroundcolorwhite" colSpan="2"></td>
		</tr>
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
					CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
					MenuFadeDelay="2" mouseovercssclass="MouseOver">
					<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
				</cc1:menu>
			</td>
		</tr>
	</table>
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Employee images7" height="10" alt="employee images7" src="images/spacer.gif"
						width="10">Employement Events</td>
			</tr>
		</table>
	</div>
	<table width="700" border="0">
		<tr>
			<td>
				&nbsp;
			</td>
			<td>
				&nbsp;
			</td>
			<td>
				&nbsp;
			</td>
		</tr>
	</table>
	<div>
		<table width="700" class="Table_WithBorder">
			<tr>
				<td align="left" width="305"><asp:label id="LabelLook" runat="server" Width="78px" cssclass="Label_Small">
							Look For</asp:label><asp:textbox id="TextBoxFind" runat="server" CssClass="TextBox_Normal" Width="112px"></asp:textbox>
					<asp:button id="Buttonsearch" runat="server" CssClass="Button_Normal" Width="64px" Text="Search"
						CausesValidation="False"></asp:button>&nbsp;
					<asp:label id="LabelNoRecordFound" runat="server" CssClass="Label_Small" width="130" visible="false">No Record Found</asp:label></td>
			</tr>
			<tr vAlign="top">
				<td vAlign="top" width="305">
					<DIV style="OVERFLOW: auto; WIDTH: 230px; HEIGHT: 210px; TEXT-ALIGN: left">
						<asp:datagrid id="DataGridEmployeeEvents" runat="server" width="212px" cellpadding="1" CssClass="DataGrid_Grid"
							allowsorting="true">
							<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
							<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
							<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
											CommandName="Select" ToolTip="Select"></asp:ImageButton>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
						</asp:datagrid>
					</DIV>
					
				</td>
				<td>
					<table width="420">
						<tr>
							<td align="left" width="200"><asp:label id="LabelEmpEventType" runat="server" Width="136px" cssclass="Label_Small">Emp. Event Type:</asp:label>&nbsp;&nbsp;&nbsp;</td>
							<td align="left"><asp:textbox id="TextBoxEmpEventType" runat="server" width="100" CssClass="TextBox_Normal" readonly="true"
									MaxLength="6"></asp:textbox>&nbsp;
								<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Employee Event Type cannot be blank"
									ControlToValidate="TextBoxEmpEventType">*</asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
						</tr>
						<tr>
							<td align="left" width="200"><asp:label id="LabelShortDesc" runat="server" Width="100px" cssclass="Label_Small">Short Desc:</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxShortDesc" runat="server" width="150" CssClass="TextBox_Normal" readonly="true"
									MaxLength="20"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
						</tr>
						<tr>
							<td align="left" width="200"><asp:label id="LabelLongDesc" runat="server" Width="100px" cssclass="Label_Small">Long Desc</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxLongDesc" runat="server" width="250" CssClass="TextBox_Normal" readonly="true"
									MaxLength="100"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
						</tr>
						<tr>
							<td width="63">&nbsp;</td>
						</tr>
						<tr>
							<td width="63">&nbsp;</td>
						</tr>
						<tr>
							<td width="63">&nbsp;</td>
						</tr>
						<tr>
							<td width="63">&nbsp;</td>
						</tr>
						<tr>
							<td width="63">&nbsp;</td>
						</tr>
						<tr>
							<td width="63">&nbsp;</td>
						</tr>
						<tr>
							<td width="63">&nbsp;</td>
						</tr>
						<tr>
							<td width="107" colspan="2">&nbsp;
								<asp:ValidationSummary id="ValidationSummary1" runat="server" Width="264px"></asp:ValidationSummary></td>
						</tr>
					</table>
					<table>
						<tr valign="bottom">
							<td align="center" class="Td_ButtonContainer"><asp:button id="ButtonSave" Width="60" CssClass="Button_Normal" Runat="server" Text="Save" enabled="false"></asp:button></td>
							<td align="center" class="Td_ButtonContainer"><asp:button id="ButtonCancel" Width="60" CssClass="Button_Normal" Runat="server" Text="Cancel"
									enabled="false" CausesValidation="False"></asp:button></td>
							<td align="center" class="Td_ButtonContainer"><asp:button id="ButtonAdd" Width="60" CssClass="Button_Normal" Runat="server" Text="Add" CausesValidation="False"></asp:button></td>
							<td align="center" class="Td_ButtonContainer"></td>
							<td align="center" class="Td_ButtonContainer"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="64px" Text="OK" CausesValidation="False"></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<asp:PlaceHolder id="PlaceHolder1" runat="server"></asp:PlaceHolder>
</form>
<!--#include virtual="bottom.html"-->
