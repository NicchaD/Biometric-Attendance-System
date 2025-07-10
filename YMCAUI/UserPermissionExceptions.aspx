<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UserPermissionExceptions.aspx.vb" Inherits="YMCAUI.UserPermissionExceptions"%>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="y" Namespace ="YMCAUI" Assembly="YMCAUI" %>  
<!--#include virtual="TopNew.htm"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table class="Table_WithoutBorder" width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
					Select a Secured Item
				</td>
			</tr>
		</table>
	</div>
	<table class="Table_WithBorder" width="700">
		<tr>
			<td align="left"><asp:label id="Labellook" runat="server" cssClass="Label_Small">Look For</asp:label>&nbsp;
				<asp:textbox id="TextBoxSearch" runat="server" cssClass="TextBox_Normal"></asp:textbox>&nbsp;
				<asp:button id="ButtonSearch" runat="server" cssClass="Button_Normal" Text="Search" Width="64px"></asp:button>&nbsp;&nbsp;<asp:label id="LabelNoRecFound" runat="server" Width="128px" visible="false">No Records Found</asp:label>
			</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td>
				<div style="OVERFLOW: auto; WIDTH: 520px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 200px; BORDER-BOTTOM-STYLE: none"><y:YmcaDataGrid id="DataGridUserPermissionsExceptions" runat="server" Width="502px" AutoGenerateColumns="False"
						CssClass="DataGrid_Grid" allowsorting=true>
						<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
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
							<asp:TemplateColumn visible="false" HeaderText="Secured Item Code">
								<ItemTemplate>
									<asp:Label runat="server" Width="141px" Text='<%#Container.DataItem("SecuredItemCode")%>' ID="lblItemCode" Name = "lblItemCode">
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Secured Item" sortexpression="SecuredItem">
								<ItemTemplate>
									<asp:Label runat="server" Width="141px" Text='<%#Container.DataItem("SecuredItem")%>' ID="lblItem" NAME="lblItem">
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Type" sortexpression="Type">
								<ItemTemplate>
									<asp:Label id="lblType" runat="server" Width="141px" Text='<%#Container.DataItem("Type")%>' NAME="lblType">
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Access">
								<ItemTemplate>
									<asp:DropDownList id="DrpAccess" runat="server" cssClass="DropDown_Normal" Width="117px"></asp:DropDownList>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</y:YmcaDataGrid></div>
			</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td><asp:button id="ButtonSave" runat="server" cssClass="Button_Normal" Text="Save" width="70"></asp:button>&nbsp;&nbsp;
				<asp:button id="ButtonCancel" runat="server" cssClass="Button_Normal" Text="Cancel" width="70"></asp:button></td>
		</tr>
	</table>
</form>
<!--#include virtual="bottom.html"-->
