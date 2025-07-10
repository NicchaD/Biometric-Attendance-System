<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GroupProperties.aspx.vb" Inherits="YMCAUI.GroupProperties"%>
<%@ Register TagPrefix="y" Namespace ="YMCAUI" Assembly="YMCAUI" %>  
<!--#include virtual="TopNew.htm"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					Group Properties<asp:label id="LabelGroupDetail" runat="server"></asp:label>
				</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<table class="Table_WithBorder" width="695">
		<tr>
			<td align="right"><asp:label id="LabelGroup" runat="server" cssClass="Label_Small">Group</asp:label></td>
			<td align="left"><asp:textbox id="TextBoxGroup" runat="server" width="180" cssClass="TextBox_Normal" maxlength="20"></asp:textbox></td>
			<td>&nbsp;</td>
			<td rowSpan="2"><asp:label id="LabelText" runat="server" Width="154px" cssClass="Label_Small">Set Access to all Secured Items To</asp:label></td>
		</tr>
		<tr>
			<td align="right"><asp:label id="LabelDesc" runat="server" cssClass="Label_Small">Description</asp:label></td>
			<td align="left" colSpan="2"><asp:textbox id="TextboxDesc" runat="server" width="240" cssClass="TextBox_Normal" maxlength="60"></asp:textbox></td>
		</tr>
		<tr>
			<td colSpan="3">&nbsp;</td>
			<td><asp:button id="ButtonFullAccess" runat="server" Width="72px" Text="Full" cssClass="Button_Normal"></asp:button></td>
		</tr>
		<tr>
			<td align="right" colSpan="3"><asp:label id="LabelSecuredItemType" runat="server" cssClass="Label_Small">Display Secured Item Of This Type</asp:label><asp:dropdownlist id="DropDownListItemType" runat="server" Width="140px" AutoPostBack="True" cssClass="DropDown_Normal">
					<asp:ListItem Value="0">All</asp:ListItem>
					<asp:ListItem Value="4">Control</asp:ListItem>
					<asp:ListItem Value="3">Form</asp:ListItem>
					<asp:ListItem Value="1">Menu Bar</asp:ListItem>
					<asp:ListItem Value="2">Menu Pad</asp:ListItem>
				</asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
			<td><asp:button id="ButtonNone" runat="server" Width="72px" Text="None" cssClass="Button_Normal"></asp:button></td>
		</tr>
		<TR>
			<TD align="right" colSpan="3"></TD>
			<TD>
				<asp:button id="ButtonReadOnly" runat="server" cssClass="Button_Normal" Width="72px" Text="Read Only"></asp:button></TD>
		</TR>
		<tr>
			<td colSpan="4">
				<div style="OVERFLOW: auto; WIDTH: 680px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 200px; BORDER-BOTTOM-STYLE: none"><y:YmcaDataGrid id="DataGridGroupProps" runat="server" Width="670px" CssClass="DataGrid_Grid" AutoGenerateColumns="False"
						allowsorting="true">
						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
						<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
						<Columns>
							<asp:TemplateColumn HeaderText="Secured Item" sortexpression="SecuredItem">
								<ItemTemplate>
									<asp:Label id="lblItemCode" runat="server" Text='<%#Container.DataItem("SecuredItemCode")%>' Visible="False">
									</asp:Label>
									<asp:Label id=lblSecuredItem runat="server" Text='<%#Container.DataItem("SecuredItem")%>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Type" sortexpression="Type">
								<ItemTemplate>
									<asp:Label id=lblType runat="server" Text='<%#Container.DataItem("Type")%>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn HeaderText="Access">
								<ItemTemplate>
									<asp:Label id=lblAccess runat="server" Text='<%#Container.DataItem("Access")%>' Visible="False">
									</asp:Label>
									<asp:DropDownList id=DropDownGridAccess runat="server" width="100" DataTextField='<%#Container.DataItem("Access")%>' AutoPostBack="false" cssClass="DropDown_Normal" onselectedIndexchanged="enableSaveCancel">
									</asp:DropDownList>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</y:YmcaDataGrid></div>
			</td>
		</tr>
	</table>
	<table class="Table_WithBorder" width="695">
		<tr>
			<td width="130">&nbsp;</td>
			<td><asp:button id="ButtonPrint" runat="server" width="60" Text="Print.." cssClass="Button_Normal"></asp:button></td>
			<td><asp:button id="ButtonSave" runat="server" width="60" Text="Save" cssClass="Button_Normal"></asp:button></td>
			<td><asp:button id="ButtonCancel" runat="server" width="60" Text="Cancel" cssClass="Button_Normal"></asp:button></td>
			<td><asp:button id="ButtonMembers" runat="server" width="80" Text="Members" cssClass="Button_Normal"></asp:button></td>
			<td><asp:button id="ButtonDelete" runat="server" width="80" Text="Delete" cssClass="Button_Normal"></asp:button></td>
			<td><asp:button id="ButtonAdd" runat="server" width="80" Text="Add" cssClass="Button_Normal"></asp:button></td>
			<td><asp:button id="ButtonOk" runat="server" width="80" Text="OK" cssClass="Button_Normal"></asp:button></td>
		</tr>
	</table>
	<asp:PlaceHolder id="PlaceHolderGroupProps" runat="server"></asp:PlaceHolder>
</form>
<!--#include virtual="bottom.html"-->
