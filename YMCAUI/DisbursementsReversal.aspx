<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DisbursementsReversal.aspx.vb" Inherits="YMCAUI.DisbursementsReversal"%>
<!--#include virtual="TopNew.htm"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					Disbursement Reversal</td>
			</tr>
			<tr>
				<td>
					&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="700">
			<tr>
				<td><DIV style="OVERFLOW: auto; WIDTH: 680px; HEIGHT: 120px; TEXT-ALIGN: left">
						<asp:DataGrid ID="DataGridDisbursementReversal" Runat="server" Width="600" CssClass="DataGrid_Grid">
							<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
							<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
							<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
							<columns>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:CheckBox id="CheckBoxSelect" runat="server"></asp:CheckBox>
										<asp:Label id="LabelDisbId" runat="server" visible="false" text='<%# DataBinder.Eval(Container.DataItem, "DisbursementID") %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
							</columns>
						</asp:DataGrid>
					</DIV>
				</td>
			</tr>
			<tr>
				<td align="right"><asp:label id="LabelFactorGroup" runat="server" CssClass="Label_Small">Status</asp:label>
					<asp:DropDownList id="DropDownStatus" runat="server"></asp:DropDownList></td>
			</tr>
		</table>
	</div>
	<table width="700">
		<tr class="Td_ButtonContainer">
			<td align="left" class="Td_ButtonContainer">
				<asp:Button Width="80" Runat="server" ID="ButtonSelectAll" Text="Select All" CssClass="Button_Normal"></asp:Button>
				<asp:Button id="ButtonNone" runat="server" CssClass="Button_Normal" Text="Select None" Visible="False"></asp:Button></td>
			<td align="right" class="Td_ButtonContainer">
				<asp:Button Width="80" Runat="server" ID="ButtonSave" Text="Save" CssClass="Button_Normal"></asp:Button>
				<asp:Button Width="80" Runat="server" ID="ButtonCancel" Text="Cancel" CssClass="Button_Normal"></asp:Button>
			</td>
		</tr>
	</table>
	<asp:PlaceHolder id="PlaceHolder1" runat="server"></asp:PlaceHolder>
</form>
<!--#include virtual="bottom.html"-->
