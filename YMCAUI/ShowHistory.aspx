<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ShowHistory.aspx.vb" Inherits="YMCAUI.ShowHistory"%>
<!--#include virtual="TopNew.htm"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					Show History</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="700">
			<tr>
				<td align="center">
					<DIV style="OVERFLOW: auto; WIDTH: 600px; HEIGHT: 200px"><asp:datagrid id="DataGridShowHistory" width="500px" Runat="server" CssClass="DataGrid_Grid" AutoGenerateColumns="False">
							<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
							<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
							<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
							<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="UniqueID" HeaderText="UniqueID"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="ActionType" HeaderText="Action Type"></asp:BoundColumn>
								<asp:BoundColumn DataField="ActionShortDesc" HeaderText="Action Description"></asp:BoundColumn>
								<asp:BoundColumn DataField="AcctDate" HeaderText="Accounting Date"></asp:BoundColumn>
								<asp:BoundColumn DataField="ActionDate" HeaderText="Action Date"></asp:BoundColumn>
								<asp:BoundColumn DataField="Notes" HeaderText="Notes"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Created" HeaderText="Created"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Creator" HeaderText="Creator"></asp:BoundColumn>
							</Columns>
						</asp:datagrid><asp:label id="LabelNoRecordFound" runat="server" CssClass="Label_Medium" Visible="False">No record history available for the selected disbursement.</asp:label></DIV>
				</td>
			</tr>
			<tr>
				<td class="Td_ButtonContainer" align="right"><asp:button id="ButtonClose" Runat="server" CssClass="Button_Normal" Text="Close" Width="80"></asp:button></td>
			</tr>
		</table>
	</div>
</form>
<!--#include virtual="bottom.html"-->
