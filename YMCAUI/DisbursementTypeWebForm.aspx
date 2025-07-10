<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DisbursementTypeWebForm.aspx.vb" Inherits="YMCAUI.DisbursementTypeWebForm"%>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
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
				<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
					Meta Disbursement Types Maintenance
				</td>
			</tr>
		</table>
	</div>
	<table width="700">
		<tr>
			<td>&nbsp;
			</td>
			<td>&nbsp;
			</td>
			<td>&nbsp;
			</td>
		</tr>
	</table>
	<div class="Div_Center">
		<table class="Table_WithBorder" cellSpacing="0" cellPadding="0" width="698">
			<tr>
				<td>&nbsp;
				</td>
			</tr>
			<tr>
				<td align="left">&nbsp;<asp:label id="LabelLook" runat="server" cssclass="Label_Small" Width="78px">
							Look For</asp:label>&nbsp;
					<asp:textbox id="TextBoxSearch" runat="server" CssClass="TextBox_Normal" Width="112px" MaxLength="4"></asp:textbox>&nbsp; 
					&nbsp;<asp:button id="ButtonSearch" CssClass="Button_Normal" Width="70" Runat="server" Text="Search"
						causevalidation="false"></asp:button>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
			<tr vAlign="top">
				<td vAlign="top">
					<div style="OVERFLOW: auto; WIDTH: 310px; HEIGHT: 170px"><asp:datagrid id="DataGridDisbursement" runat="server" CssClass="DataGrid_Grid" allowsorting="true"
							width="292">
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
						</asp:datagrid></div>
				</td>
				<td>
					<table width="364">
						<tr>
							<td align="left"><asp:label id="LabelDisbursementType" runat="server" cssclass="Label_Small" Width="50px">Disbursement Type:</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxDisbursementType" runat="server" width="175" CssClass="TextBox_Normal"
									MaxLength="4"></asp:textbox>
								<asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" CssClass="Error_Message" ErrorMessage="Type cannot be empty"
									ControlToValidate="TextBoxDisbursementType">*</asp:requiredfieldvalidator>
							</td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelShortDescription" runat="server" cssclass="Label_Small" Width="50px">Short Desc:</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxShortDescription" runat="server" width="175" CssClass="TextBox_Normal"
									MaxLength="20"></asp:textbox>
								<asp:requiredfieldvalidator id="RequiredFieldValidatorDisType" runat="server" CssClass="Error_Message" ErrorMessage="Short Description cannot be empty"
									ControlToValidate="TextBoxShortDescription">*</asp:requiredfieldvalidator>
							</td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelDescription" runat="server" cssclass="Label_Small" Width="50px">Desc:</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxDescription" runat="server" width="175" CssClass="TextBox_Normal" MaxLength="100"></asp:textbox>
								<asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" CssClass="Error_Message" ErrorMessage="Description cannot be empty"
									ControlToValidate="TextBoxDescription">*</asp:requiredfieldvalidator>
							</td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelGLAccountNo" runat="server" cssclass="Label_Small" Width="50px">GL Acct No:</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxGLAccountNo" runat="server" width="175" CssClass="TextBox_Normal" MaxLength="14"></asp:textbox></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelActive" runat="server" cssclass="Label_Small" Width="50px">Active</asp:label></td>
							<td align="left"><asp:checkbox id="CheckBoxActive" runat="server" cssclass="CheckBox_Normal"></asp:checkbox></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelEditable" runat="server" cssclass="Label_Small" Width="50px">Editable</asp:label></td>
							<td align="left"><asp:checkbox id="CheckBoxEditable" runat="server" cssclass="CheckBox_Normal"></asp:checkbox></td>
						</tr>
						<TR>
							<TD colSpan="2">&nbsp;
							</TD>
						</TR>
						<tr>
							<td colSpan="2">
								<asp:ValidationSummary id="ValidationSummary1" runat="server" CssClass="Error_Message"></asp:ValidationSummary>
							</td>
						</tr>
					</table>
					<TABLE cellSpacing="0" width="364">
						<TR vAlign="middle">
							<TD class="Td_ButtonContainer" align="center" height="25"><asp:button id="ButtonSave" CssClass="Button_Normal" Width="70" Runat="server" Text="Save"></asp:button></TD>
							<TD class="Td_ButtonContainer" align="center" height="25"><asp:button id="ButtonCancel" CssClass="Button_Normal" Width="70" Runat="server" Text="Cancel"
									CausesValidation="False"></asp:button></TD>
							<TD class="Td_ButtonContainer" align="center" height="25"><asp:button id="ButtonAdd" CssClass="Button_Normal" Width="70" Runat="server" Text="Add" CausesValidation="False"></asp:button></TD>
							<TD class="Td_ButtonContainer" align="center" height="25"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="64px" Text="OK" CausesValidation="False"></asp:button></TD>
						</TR>
					</TABLE>
					<asp:label id="LabelMessage" runat="server" cssclass="Label_Small"></asp:label></td>
			</tr>
			<TR>
				<TD>&nbsp;
				</TD>
			</TR>
		</table>
		</div><asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder>
</form> <!--#include virtual="bottom.html"-->
