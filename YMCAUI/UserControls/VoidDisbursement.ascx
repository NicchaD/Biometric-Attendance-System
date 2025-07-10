<%@ Control Language="vb" AutoEventWireup="false" Codebehind="VoidDisbursement.ascx.vb" Inherits="YMCAUI.VoidDisbursement" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<table class="Table_WithBorder1" cellSpacing="1" cellPadding="0" width="700" border="0">
	<tr>
		<td vAlign="top" colSpan="4"><asp:label id="LabelPayeeSSN" CssClass="Label_Small" Runat="server" Visible="False"></asp:label></td>
	</tr>
	<tr>
		<td vAlign="top">
			<asp:Label ID="LabelRecordNotFound" Runat="server" CssClass="Label_Small">No Matching Records</asp:Label>
			<asp:DataGrid id="Datagrid1" Runat="server" DataKeyField="DisbursementID" CssClass="DataGrid_Grid"
				Width="690" AutoGenerateColumns="false" allowsorting="true">
				<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
				<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
				<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
				<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
				<Columns>
					<asp:TemplateColumn>
						<ItemTemplate>
							<!--<asp:CheckBox  AutoPostBack="True" OnCheckedChanged="CheckBox_CheckedChanged" Checked="false"
								Runat="server" ID="chkRefund"></asp:CheckBox>-->
							<asp:ImageButton Visible="true" id="ImagebtnAddress" OnClick="image_OnClick" runat="server" ImageUrl="../images/select.gif"
								CausesValidation="False" CommandName="Select" ToolTip="Show Address" CommandArgument="DisbursementID"></asp:ImageButton>
							<asp:Label id="Label1" runat="server" visible="false" text='<%# DataBinder.Eval(Container.DataItem, "DisbursementID") %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="DisbursementID" HeaderText="DisbursementID" VISIBLE="FALSE"></asp:BoundColumn>
					<asp:BoundColumn DataField="Type" Visible="False" HeaderText="Type"></asp:BoundColumn>
					<asp:BoundColumn DataField="Number" HeaderText="Number"></asp:BoundColumn>
					<asp:BoundColumn DataField="AccountDate" HeaderText="Account Date" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundColumn>
					<asp:BoundColumn DataField="IssuedDate" HeaderText="Issued Date" SortExpression="IssuedDate" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundColumn>
					<asp:BoundColumn DataField="Amount" HeaderText="Amount"></asp:BoundColumn>
					<asp:BoundColumn DataField="PayMethod" HeaderText="Pay Method"></asp:BoundColumn>
					<asp:BoundColumn DataField="GrossAmount" HeaderText="GrossAmount" VISIBLE="FALSE"></asp:BoundColumn>
					<asp:BoundColumn DataField="Voided" HeaderText="Voided" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn DataField="Reversed" HeaderText="Reversed" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn DataField="LegalEntityType" HeaderText="LegalEntityType" VISIBLE="FALSE"></asp:BoundColumn>
					<asp:BoundColumn DataField="LegalEntityName" HeaderText="LegalEntityName" VISIBLE="FALSE"></asp:BoundColumn>
					<asp:BoundColumn DataField="LegalEntityAddress" HeaderText="LegalEntityAddress" VISIBLE="FALSE"></asp:BoundColumn>
					<asp:BoundColumn DataField="PayeeAddress" HeaderText="PayeeAddress" VISIBLE="FALSE"></asp:BoundColumn>
					<asp:BoundColumn DataField="EFTType" HeaderText="EFTType" VISIBLE="FALSE"></asp:BoundColumn>
					<asp:BoundColumn DataField="AcctNumber" HeaderText="AcctNumber" VISIBLE="FALSE"></asp:BoundColumn>
					<asp:BoundColumn DataField="BankInfo" HeaderText="BankInfo" VISIBLE="FALSE"></asp:BoundColumn>
					<asp:BoundColumn DataField="ActionCode" HeaderText="ActionCode" VISIBLE="FALSE"></asp:BoundColumn>
					<asp:BoundColumn DataField="ActionNotes" HeaderText="ActionNotes" VISIBLE="FALSE"></asp:BoundColumn>
					<asp:BoundColumn DataField="FinalAction" HeaderText="FinalAction" VISIBLE="FALSE"></asp:BoundColumn>
				</Columns>
			</asp:DataGrid>
		</td>
	</tr>
</table>
