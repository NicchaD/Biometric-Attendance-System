<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UserPermissions.aspx.vb" Inherits="YMCAUI.UserPermissions"%>
<%@ Register TagPrefix="y" Namespace ="YMCAUI" Assembly="YMCAUI" %>  
<!--#include virtual="TopNew.htm"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left" height="11"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					Group Permissions To Secured Items
				</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<table class="Table_WithBorder" width="690">
		<tr>
			<td align="right"><asp:label id="Label1" runat="server" width="400" cssClass="Label_Small">Display Secured Items of this Type</asp:label></td>
			<td align="right"><asp:dropdownlist id="DropDownList1" runat="server" cssClass="DropDown_Normal" Width="124px" AutoPostBack="True">
					<asp:ListItem Value="0">All</asp:ListItem>
					<asp:ListItem Value="4">Control</asp:ListItem>
					<asp:ListItem Value="3">Form</asp:ListItem>
					<asp:ListItem Value="1">Menu Bar</asp:ListItem>
					<asp:ListItem Value="2">Menu Pad</asp:ListItem>
				</asp:dropdownlist></td>
		</tr>
		<tr>
			<td colSpan="2">
				<div style="OVERFLOW: auto; WIDTH: 680px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 200px; BORDER-BOTTOM-STYLE: none"><y:YmcaDataGrid id="DataGridUserPermissions1" runat="server" Width="663px" CssClass="DataGrid_Grid"
						allowsorting="true">
						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
						<SelectedItemStyle cssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
						<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
						<Columns>
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:ImageButton id="ImageButtonSelect1" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
										CommandName="Select" ToolTip="Select"></asp:ImageButton>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</y:YmcaDataGrid></div>
		<tr>
			<td>
				<div style="OVERFLOW: auto; WIDTH: 520px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 200px; BORDER-BOTTOM-STYLE: none"><y:YmcaDataGrid id="DataGridUserPermissions2" runat="server" Width="502px" CssClass="DataGrid_Grid"
						AutoGenerateColumns="False" allowsorting="true">
						<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
						<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
						<Columns>
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:ImageButton id="ImagebuttonSelect2" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
										CommandName="Select" ToolTip="Select"></asp:ImageButton>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn Visible="False" HeaderText="Group Key">
								<ItemTemplate>
									<asp:Label runat="server" Width="141px" Text='<%#Container.DataItem("Group Key")%>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Group" sortexpression="Group">
								<ItemTemplate>
									<asp:Label id="lblGroup" runat="server" Width="141px" Text='<%#Container.DataItem("Group")%>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Access">
								<ItemTemplate>
									<asp:Label id=lblAccess runat="server" Text='<%#Container.DataItem("Access")%>' Visible="False">
									</asp:Label>
									<asp:DropDownList id=DrpAccess runat="server" AutoPostBack="false" OnSelectedIndexChanged = "enableButton" cssClass="DropDown_Normal" Width="141px" DataTextField='<%#Container.DataItem("Access")%>'>
									</asp:DropDownList>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</y:YmcaDataGrid></div>
			</td>
			<td>
				<table>
					<tr>
						<td><asp:label id="Label2" runat="server" cssClass="Label_Small" Width="118px" Height="49px">Set Access for all Groups to</asp:label></td>
					</tr>
					<tr>
						<td align="center"><asp:button id="ButtonFull" runat="server" cssClass="Button_Normal" Width="88px" Text="Full"></asp:button></td>
					</tr>
					<tr>
						<td align="center"><asp:button id="ButtonReadOnly" runat="server" width="88" cssClass="Button_Normal" Text="ReadOnly"
								Enabled="False"></asp:button></td>
					</tr>
					<tr>
						<td align="center"><asp:button id="ButtonNone" runat="server" cssClass="Button_Normal" Width="88px" Text="None"></asp:button></td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td colSpan="2">
				<table width="680">
					<TR>
						<td align="left">&nbsp;&nbsp;
							<asp:button id="ButtonMembers" runat="server" cssClass="Button_Normal" Width="88px" Text="Members"
								Enabled="False"></asp:button></td>
						<td>&nbsp;&nbsp;&nbsp;&nbsp;<asp:button id="ButtonSave" runat="server" cssClass="Button_Normal" Width="88px" Text="Save"
								Enabled="False"></asp:button></td>
						<td><asp:button id="ButtonCancel" runat="server" cssClass="Button_Normal" Width="88px" Text="Cancel"
								Enabled="False"></asp:button></td>
						<td align="right"><asp:button id="ButtonOK" runat="server" cssClass="Button_Normal" Width="88px" Text="OK"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
					</TR>
				</table>
			</td>
		</tr>
	</table>
	<asp:PlaceHolder id="PlaceHolder1" runat="server"></asp:PlaceHolder>
</form>
<!--#include virtual="bottom.html"-->
