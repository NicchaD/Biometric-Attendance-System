<%@ Control Language="vb" AutoEventWireup="false" Codebehind="VoidUserControl.ascx.vb" Inherits="YMCAUI.VoidUserControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<script language="javascript">
function selectUnselectRequest(obj)
{
	
	alert(obj.id);
}
</script>
<table class="Table_WithBorder1" cellSpacing="1" cellPadding="0" width="700" border="0">
	<tr>
		<td vAlign="top" colSpan="4"><asp:label id="LabelPayeeSSN" CssClass="Label_Small" Runat="server" Visible="False"></asp:label></td>
	</tr>
	<tr>
		<td vAlign="top" align="left" colSpan="4"><asp:label id="LabelNoRecordFound" CssClass="Label_Small" Runat="server">No Matching Records</asp:label><asp:datalist id="datalistRefund" CssClass="DataGrid_Grid" Runat="server" DataKeyField="RequestId"
				RepeatDirection="Vertical">
				<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
				<HeaderTemplate>
				</HeaderTemplate>
				<FooterTemplate>
				</FooterTemplate>
				<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
				<ItemTemplate>
					<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
						<TR>
							<TD>
								<!--<asp:RadioButton id="rdRefund" GroupName="Refund" AutoPostBack="True" Runat="server" OnCheckedChanged="Radio_CheckedChanged" 
 Checked="False"></asp:RadioButton>-->
								<asp:ImageButton id="Imagebutton1" runat="server" ImageUrl="../images/select.gif" CausesValidation="False" 
 CommandName="Select" ToolTip="Select"></asp:ImageButton></TD>
							<TD class="Label_Small">Type :
								<%# DataBinder.Eval(Container.DataItem, "Type") %>
							</TD>
							<TD class="Label_Small">Requested Date :
								<%# DataBinder.Eval(Container.DataItem, "RequestDate") %>
							</TD>
							<TD class="Label_Small">Total Amount :
								<%# DataBinder.Eval(Container.DataItem, "GrossAmount") %>
							</TD>
						</TR>
						<TR>
							<TD colSpan="4">
								<asp:DataGrid id="dgItemDetails" Runat="server" DataKeyField="DisbursementID" CssClass="DataGrid_Grid" 
 Width="690" AutoGenerateColumns="false">
									<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
									<Columns>
										<asp:TemplateColumn>
											<ItemTemplate>
												<!--<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\checkmark.JPG" CausesValidation="False"
																														CommandName="Select" ToolTip="Select"></asp:ImageButton>-->
												<asp:CheckBox Enabled="False" AutoPostBack="True" OnCheckedChanged="CheckBox_CheckedChanged" Checked="false" 
 Runat="server" ID="chkRefund"></asp:CheckBox>
												<asp:ImageButton Visible="false" id="ImageButtonShowHistory" runat="server" ImageUrl="../images/history.gif" 
 CausesValidation="False" CommandName="ShowHistory" ToolTip="Show History"></asp:ImageButton>&nbsp;
												<asp:ImageButton Visible="true" id="ImagebtnAddress" OnClick="image_OnClick" runat="server" ImageUrl="../images/details.gif" 
 CausesValidation="False" CommandName="ShowAddress" ToolTip="Show Address" CommandArgument="DisbursementID"></asp:ImageButton>
												<asp:Label id="LabelDisbId" runat="server" visible="false" text='<%# DataBinder.Eval(Container.DataItem, "DisbursementID") %>'>
												</asp:Label>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="DisbursementID" HeaderText="DisbursementID" VISIBLE="FALSE"></asp:BoundColumn>
										<asp:BoundColumn DataField="Type" Visible="False" HeaderText="Type"></asp:BoundColumn>
										<asp:BoundColumn DataField="Number" HeaderText="Number"></asp:BoundColumn>
										<asp:BoundColumn DataField="AccountDate" HeaderText="Account Date" DataFormatString="{0:d}"></asp:BoundColumn>
										<asp:BoundColumn DataField="IssuedDate" HeaderText="Issued Date" DataFormatString="{0:d}"></asp:BoundColumn>
										<asp:BoundColumn DataField="GrossAmount" HeaderText="Gross Amt"></asp:BoundColumn>
										<asp:BoundColumn DataField="Amount" HeaderText="Net Amt"></asp:BoundColumn>
										<asp:BoundColumn DataField="PayMethod" HeaderText="Pay Method"></asp:BoundColumn>
										<asp:BoundColumn DataField="CurrencyCode" HeaderText="CurrencyCode" VISIBLE="FALSE"></asp:BoundColumn>
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
								</asp:DataGrid></TD>
						</TR>
					</TABLE>
				</ItemTemplate>
				<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
			</asp:datalist></td>
	</tr>
	<tr>
		<td vAlign="top"><asp:datalist id="datalistAnnuity" CssClass="DataGrid_Grid" Runat="server" DataKeyField="RequestId"
				RepeatDirection="Vertical">
				<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
				<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
				<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
				<HeaderTemplate>
				</HeaderTemplate>
				<ItemTemplate>
					<table width="100%" border="0" cellpadding="0" cellspacing="0">
						<tr>
							<td>
								<asp:ImageButton id="imgAnnuity" runat="server" ImageUrl="../images/select.gif" CausesValidation="False" 
 CommandName="Select" ToolTip="Select"></asp:ImageButton>
							</td>
							<td Class="Label_Small">
								Type :
								<%# DataBinder.Eval(Container.DataItem, "Type") %>
							</td>
							<td Class="Label_Small">
								Issued Date :
								<asp:Label id="lblIssueDate" runat="server" visible="true" text='<%# DataBinder.Eval(Container.DataItem, "IssuedDate") %>'>
								</asp:Label>
								<!--<%# DataBinder.Eval(Container.DataItem, "IssuedDate") %>-->
							</td>
							<td Class="Label_Small">
								Total Amount :
								<%# DataBinder.Eval(Container.DataItem, "Amount") %>
							</td>
						</tr>
						<tr>
							<td colspan="4">
								<asp:DataGrid ID="dgAnnuity" Runat="server" Width="690" CssClass="DataGrid_Grid" AutoGenerateColumns="false">
									<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
									<Columns>
										<asp:TemplateColumn>
											<ItemTemplate>
												<asp:CheckBox Enabled="False" AutoPostBack="True" OnCheckedChanged="CheckBox_CheckedChanged" Checked="false" 
 Runat="server" ID="chkAnnuity"></asp:CheckBox>
												<asp:ImageButton id="imgAnnuityAddress" OnClick="image_OnClick" runat="server" ImageUrl="../images/details.gif" 
 CausesValidation="False" CommandName="Select" ToolTip="Select"></asp:ImageButton>
												<asp:ImageButton Visible="False" id="Imagebutton3" runat="server" ImageUrl="../images/history.gif" 
 CausesValidation="False" CommandName="ShowHistory" ToolTip="Show History"></asp:ImageButton>&nbsp;
												<asp:ImageButton Visible="False" id="Imagebutton4" runat="server" ImageUrl="../images/printico.gif" 
 CausesValidation="False" CommandName="PrintAffidavit" ToolTip="Print Affidavit" enabled="false"></asp:ImageButton>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="DisbursementID" HeaderText="DisbursementID" VISIBLE="false"></asp:BoundColumn>
										<asp:BoundColumn DataField="Type" Visible="False" HeaderText="Type"></asp:BoundColumn>
										<asp:BoundColumn DataField="Number" HeaderText="Number"></asp:BoundColumn>
										<asp:BoundColumn DataField="AccountDate" HeaderText="AccountDate"></asp:BoundColumn>
										<asp:BoundColumn DataField="IssuedDate" HeaderText="IssuedDate"></asp:BoundColumn>
										<asp:BoundColumn DataField="Amount" HeaderText="Amount"></asp:BoundColumn>
										<asp:BoundColumn DataField="PayMethod" HeaderText="PayMethod"></asp:BoundColumn>
										<asp:BoundColumn DataField="CurrencyCode" HeaderText="CurrencyCode" VISIBLE="FALSE"></asp:BoundColumn>
										<asp:BoundColumn DataField="Voided" HeaderText="Voided"></asp:BoundColumn>
										<asp:BoundColumn DataField="Reversed" HeaderText="Reversed"></asp:BoundColumn>
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
				</ItemTemplate>
				<FooterTemplate>
				</FooterTemplate>
			</asp:datalist></td>
	</tr>
	<tr>
		<td><asp:datalist id="datalistTDLoan" CssClass="DataGrid_Grid" Runat="server" DataKeyField="Number"
				RepeatDirection="Vertical">
				<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
				<HeaderTemplate>
				</HeaderTemplate>
				<FooterTemplate>
				</FooterTemplate>
				<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
				<ItemTemplate>
					<table width="100%" border="0" cellpadding="0" cellspacing="0">
						<tr>
							<td>
								<asp:ImageButton id="imgTDLoan" runat="server" ImageUrl="../images/select.gif" CausesValidation="False" 
 CommandName="Select" ToolTip="Select"></asp:ImageButton>
							</td>
							<td Class="Label_Small">
								Type :
								<%# DataBinder.Eval(Container.DataItem, "Type") %>
							</td>
							<td Class="Label_Small">
								Issued Date :
								<%# DataBinder.Eval(Container.DataItem, "IssuedDate") %>
							</td>
							<td Class="Label_Small">
								Total Amount :
								<%# DataBinder.Eval(Container.DataItem, "Amount") %>
							</td>
						</tr>
						<tr>
							<td colspan="4">
								<asp:DataGrid ID="dgLoan" Runat="server" Width="690" CssClass="DataGrid_Grid" AutoGenerateColumns="false">
									<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
									<Columns>
										<asp:TemplateColumn>
											<ItemTemplate>
												<!--<asp:ImageButton id="Imagebutton4" runat="server" ImageUrl="images\checkmark.JPG" CausesValidation="False"
																														CommandName="Select" ToolTip="Select"></asp:ImageButton>-->
												<asp:CheckBox Enabled="False" Runat="server" ID="chkLoan" OnCheckedChanged="CheckBox_CheckedChanged"></asp:CheckBox>
												<asp:ImageButton Visible="False" id="Imagebutton6" runat="server" ImageUrl="../images/history.gif" 
 CausesValidation="False" CommandName="ShowHistory" ToolTip="Show History"></asp:ImageButton>&nbsp;
												<asp:ImageButton Visible="true" id="imgLoanAddress" OnClick="image_OnClick" runat="server" ImageUrl="../images/details.gif" 
 CausesValidation="False" CommandName="ShowAddress" ToolTip="Show Address" CommandArgument="DisbursementID"></asp:ImageButton>
												<asp:Label id="Label2" runat="server" visible="false" text='<%# DataBinder.Eval(Container.DataItem, "DisbursementID") %>'>
												</asp:Label>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="DisbursementID" HeaderText="DisbursementID" VISIBLE="FALSE"></asp:BoundColumn>
										<asp:BoundColumn DataField="Type" Visible="False" HeaderText="Type"></asp:BoundColumn>
										<asp:BoundColumn DataField="Number" HeaderText="Number"></asp:BoundColumn>
										<asp:BoundColumn DataField="AccountDate" HeaderText="AccountDate"></asp:BoundColumn>
										<asp:BoundColumn DataField="IssuedDate" HeaderText="IssuedDate"></asp:BoundColumn>
										<asp:BoundColumn DataField="Amount" HeaderText="Amount"></asp:BoundColumn>
										<asp:BoundColumn DataField="PayMethod" HeaderText="PayMethod"></asp:BoundColumn>
										<asp:BoundColumn DataField="CurrencyCode" HeaderText="CurrencyCode" VISIBLE="FALSE"></asp:BoundColumn>
										<asp:BoundColumn DataField="Voided" HeaderText="Voided"></asp:BoundColumn>
										<asp:BoundColumn DataField="Reversed" HeaderText="Reversed"></asp:BoundColumn>
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
				</ItemTemplate>
				<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
			</asp:datalist></td>
	</tr>
</table>
