<%@ Page Language="vb" AutoEventWireup="false" Codebehind="VDReplaceDisbursements.aspx.vb" Inherits="YMCAUI.VDReplaceDisbursements"%>
<!--#include virtual="TopNew.htm"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700" border="0">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
					&nbsp;Replace Disbursements
				</td>
			</tr>
		</table>
	</div>
	<table width="700" border="0">
		<tr>
			<td>&nbsp;
			</td>
		</tr>
	</table>
	<div class="Div_Center">
		<table class="Table_WithBorder" cellSpacing="0" cellPadding="0" width="700">
			<tr>
				<td>
					<DIV style="OVERFLOW: auto; WIDTH: 280px; HEIGHT: 200px; TEXT-ALIGN: left">
					<asp:datagrid id="DataGridYMCAAddress" runat="server" CssClass="DataGrid_Grid" cellspacing="0"
							cellpadding="1" width="300" AutoGenerateColumns="False">
							<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
							<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
							<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
							<Columns>
							<asp:BoundColumn DataField="UniqueID" HeaderText="UniqueID" VISIBLE="FALSE"></asp:BoundColumn>
							<asp:BoundColumn DataField="EntityID" HeaderText="EntityID" VISIBLE="FALSE"></asp:BoundColumn>
							<asp:BoundColumn DataField="EntityCode" HeaderText="EntityCode" VISIBLE="FALSE"></asp:BoundColumn>
							<asp:BoundColumn DataField="EffDate" HeaderText="EffDate" VISIBLE="FALSE"></asp:BoundColumn>
							<asp:BoundColumn DataField="AddrCode" HeaderText="AddrCode" VISIBLE="FALSE"></asp:BoundColumn>
							<asp:BoundColumn DataField="Addr1" HeaderText="Address1"></asp:BoundColumn>
							<asp:BoundColumn DataField="Addr2" HeaderText="Addr2" VISIBLE="FALSE"></asp:BoundColumn>
							<asp:BoundColumn DataField="Addr3" HeaderText="Addr3" VISIBLE="FALSE"></asp:BoundColumn>
							<asp:BoundColumn DataField="City" HeaderText="City" VISIBLE="FALSE"></asp:BoundColumn>
							<asp:BoundColumn DataField="State" HeaderText="State" VISIBLE="FALSE"></asp:BoundColumn>
							<asp:BoundColumn DataField="Zip" HeaderText="Zip" VISIBLE="FALSE"></asp:BoundColumn>
							<asp:BoundColumn DataField="Country" HeaderText="Country" VISIBLE="FALSE"></asp:BoundColumn>
							<asp:BoundColumn DataField="PrimaryInd" HeaderText="PrimaryInd" VISIBLE="FALSE"></asp:BoundColumn>
							<asp:BoundColumn DataField="ActiveInd" HeaderText="ActiveInd" VISIBLE="FALSE"></asp:BoundColumn>
							</Columns>
					</asp:datagrid></DIV>
				</td>
				<td>
					<table style="WIDTH: 375px; HEIGHT: 283px" width="375">
						<tr>
							<td>
								<table>
									<tr>
										<td align="left"><asp:label id="LabelAddress1" runat="server" cssclass="Label_Small" Width="100px">Address1:</asp:label></td>
										<td align="left"><asp:textbox id="TextBoxAddress1" runat="server" CssClass="TextBox_Normal" width="300" Enabled="False"></asp:textbox></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelAddress2" runat="server" cssclass="Label_Small" Width="100px">Address2:</asp:label></td>
										<td align="left"><asp:textbox id="TextBoxAddress2" runat="server" CssClass="TextBox_Normal" width="300" Enabled="False"></asp:textbox></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelAddress3" runat="server" cssclass="Label_Small" Width="100px">Address3:</asp:label></td>
										<td align="left"><asp:textbox id="TextBoxAddress3" runat="server" CssClass="TextBox_Normal" width="300" Enabled="False"></asp:textbox></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelCity" runat="server" cssclass="Label_Small" Width="100px">City:</asp:label></td>
										<td align="left"><asp:textbox id="TextBoxCity" runat="server" CssClass="TextBox_Normal" width="100" Enabled="False"></asp:textbox></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelState" runat="server" cssclass="Label_Small" Width="100px">State:</asp:label></td>
										<td align="left"><asp:dropdownlist id="DropDownState" runat="server" CssClass="DropDown_Normal" Width="100" Enabled="False"></asp:dropdownlist></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelZip" runat="server" cssclass="Label_Small" Width="100px">Zip:</asp:label></td>
										<td align="left"><asp:textbox id="TextBoxZip" runat="server" CssClass="TextBox_Normal" width="100" Enabled="False"></asp:textbox></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelCountry" runat="server" cssclass="Label_Small" Width="100px">Country:</asp:label></td>
										<td align="left"><asp:dropdownlist id="DropDownCountry" runat="server" CssClass="DropDown_Normal" Width="100" Enabled="False"></asp:dropdownlist></td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_ButtonContainer" width="400"></td>
				<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonSave" CssClass="Button_Normal" Width="80" Text="Save" Runat="server" Enabled="False"></asp:button></td>
				<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonCancel" CssClass="Button_Normal" Width="80" Text="Cancel" Runat="server"></asp:button></td>
			</tr>
		</table>
	</div>
	<asp:placeholder id="placeholderMessage" runat="server"></asp:placeholder></form>
<!--#include virtual="bottom.html"-->
