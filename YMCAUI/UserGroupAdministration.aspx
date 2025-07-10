<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UserGroupAdministration.aspx.vb" Inherits="YMCAUI.UserGroupAdministration"%>
<%@ Register TagPrefix="y" Namespace ="YMCAUI" Assembly="YMCAUI" %>  
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
		<tr>
			<td class="Td_BackGroundColorMenu" align="left">
				<cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
					CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover"
					DefaultMouseUpCssClass="mouseup" MenuFadeDelay="2" mouseovercssclass="MouseOver">
					<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
				</cc1:menu>
			</td>
		</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="left" colSpan="2"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
				User/Group Administration
			</td>
		</tr>
		<tr>
			<td>&nbsp;
			</td>
		</tr>
	</table>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="700">
			<tr>
				<td width="501" height="231">
					<table class="table_withBorder" width="480">
						<tr>
							<td align="left">
							
								<div id="user" style="OVERFLOW: auto; WIDTH: 490px; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 200px; BORDER-BOTTOM-STYLE: none"><y:YmcaDataGrid id="DataGridUsers" runat="server" CssClass="DataGrid_Grid" Width="473px" allowsorting="true">
										<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
										<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
										<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
										<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
										<Columns>
											<asp:TemplateColumn>
												<ItemTemplate>
													<asp:ImageButton id="ImageButtonSelectUser" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
														CommandName="Select" ToolTip="Select"></asp:ImageButton>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
									</y:YmcaDataGrid></div>
							</td>
						</tr>
					</table>
				</td>
				<td vAlign="top">
					<table class="Table_WithOutBorder" width="150">
						<tr>
							<td align="left"><asp:radiobutton id="RadioButtonAllUsers" runat="server" cssClass="RadioButton_Normal" AutoPostBack="True"
									Checked="True" GroupName="GroupUsers" Text="All Users"></asp:radiobutton></td>
						</tr>
						<tr>
							<td align="left"><asp:radiobutton id="RadioButtonActiveUsers" runat="server" cssClass="RadioButton_Normal" AutoPostBack="True"
									GroupName="GroupUsers" Text="Active Users Only"></asp:radiobutton></td>
						</tr>
						<tr>
							<td align="left"><asp:radiobutton id="RadioButtonInactiveUsers" runat="server" cssClass="RadioButton_Normal" AutoPostBack="True"
									GroupName="GroupUsers" Text="Inactive Users"></asp:radiobutton></td>
						</tr>
						<tr>
							<td>&nbsp;</td>
						</tr>
						<tr>
							<td>&nbsp;</td>
						</tr>
						<tr>
							<td>&nbsp;</td>
						</tr>
						<tr>
							<td align="center"><asp:button id="ButtonReports" runat="server" Width="110px" cssClass="Button_Normal" Text="Reports.."
									enabled="false"></asp:button></td>
						</tr>
						<tr>
							<td>&nbsp;</td>
						</tr>
						<tr>
							<td align="center"><asp:button id="ButtonOk" runat="server" Width="110px" cssClass="Button_Normal" Text="OK"></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<table width="500">
						<tr>
							<td align="center"><asp:button id="ButtonUserProperties" runat="server" Width="110px" cssClass="Button_Normal"
									Text="Properties.."></asp:button></td>
							<td align="center"><asp:button id="ButtonUserDelete" runat="server" Width="110px" cssClass="Button_Normal" Text="Delete"></asp:button></td>
							<td align="center"><asp:button id="ButtonNewUser" runat="server" Width="97px" cssClass="Button_Normal" Text="New User..."></asp:button></td>
							<td align="center"><asp:button id="ButtonUserMembership" runat="server" Width="110px" cssClass="Button_Normal"
									Text="Membership"></asp:button></td>
						</tr>
					</table>
				</td>
				<td></td>
			</tr>
			<tr>
				<td>
					<div id="group" style="OVERFLOW: auto; WIDTH: 490px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 200px; BORDER-BOTTOM-STYLE: none" ><y:YmcaDataGrid id="DataGridGroups" runat="server" CssClass="DataGrid_Grid" Width="473px" allowsorting="true">
							<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
							<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
							<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
							<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:ImageButton id="ImagebuttonSelectGroup" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
											CommandName="Select" ToolTip="Select"></asp:ImageButton>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</y:YmcaDataGrid></div>
				</td>
			</tr>
			<tr>
				<td>
					<table width="500">
						<tr>
							<td align="center"><asp:button id="ButtonGroupProperties" runat="server" Width="110px" cssClass="Button_Normal"
									Text="Properties..."></asp:button></td>
							<td align="center"><asp:button id="ButtonGroupDelete" runat="server" Width="110px" cssClass="Button_Normal" Text="Delete"></asp:button></td>
							<td align="center"><asp:button id="ButtonGroupNew" runat="server" Width="110px" cssClass="Button_Normal" Text="New Group..."></asp:button></td>
							<td align="center"><asp:button id="ButtonGroupMembers" runat="server" Width="110px" cssClass="Button_Normal" Text="Members..."></asp:button></td>
						</tr>
					</table>
				</td>
				<td>
					<table class="Table_WithOutBorder" width="150">
						<tr>
							<td align="left"><asp:button id="ButtonPermissions" runat="server" Width="110px" cssClass="Button_Normal" Text="Permissions..."></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
<asp:PlaceHolder id="PlaceHolderUserAdmin" runat="server"></asp:PlaceHolder>
</form>
<!--#include virtual="bottom.html"-->
