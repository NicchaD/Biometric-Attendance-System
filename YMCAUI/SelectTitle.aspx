<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SelectTitle.aspx.vb" Inherits="YMCAUI.SelectTitle"%>
<!--#include virtual="TopNew.htm"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700" border="0" cellspacing="0">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					Select A Position Title</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<table class="Table_WithBorder" width="690">
		<tr>
			<td>
				<table width="690" border="0">
					<tr>
						<td align="left" width="10%">
							<asp:label id="LabelLook" runat="server" cssClass="Label_Small">Look For</asp:label>
						</td>
						<td align="left" width="90%">
							<asp:textbox id="TextBoxLook" runat="server"></asp:textbox><asp:button id="ButtonSearch" runat="server" Text="Search"></asp:button>
						</td>
					</tr>
					<TR>
						<TD align="left" width="10%"></TD>
						<TD align="left" width="90%"><asp:label id="LabelNoRecord" runat="server" Visible="False" Width="112px" ForeColor="Red">No record found</asp:label></TD>
					</TR>
				</table>
				<table width="690" border="0">
					<tr>
						<td align="center">
							<div style="OVERFLOW: auto; WIDTH: 680px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 150px; BORDER-BOTTOM-STYLE: none">
								<asp:datagrid id="DataGridSelectTitle" runat="server" width="660" allowsorting="true" CssClass="DataGrid_Grid">
									<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<Columns>
										<asp:TemplateColumn>
											<ItemTemplate>
												<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
													CommandName="Select" ToolTip="Select"></asp:ImageButton>
											</ItemTemplate>
										</asp:TemplateColumn>
									</Columns>
								</asp:datagrid>
							</div>
						</td>
					</tr>
				</table>
				<table>
					<tr>
						<td>&nbsp;</td>
					</tr>
				</table>
				<table width="690" cellspacing="0">
					<tr>
						<td class="Td_ButtonContainer" align="right"><asp:button id="ButtonCancel" runat="server" width="80" cssClass="Button_Normal" Text="Cancel"></asp:button></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</form>
<!--#include virtual="bottom.html"-->
