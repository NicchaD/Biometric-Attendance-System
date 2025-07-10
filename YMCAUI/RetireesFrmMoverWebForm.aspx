<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RetireesFrmMoverWebForm.aspx.vb" Inherits="YMCAUI.RetireesFrmMoverWebForm" %>
<!--#include virtual="TopNew.htm"-->
<script language="javascript">
	function btn_OkClick()
	{
			//alert(Form1.ListBoxSelectedItems.value);
			if(Form1.ListBoxSelectedItems.value  !="")
			{
				 window.open('FT\\ReportViewer.aspx', 'YMCAYRS');
			 }
	}

</script>
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table class="Table_WithoutBorder" width="700" border=0 cellspacing=0>
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
					&nbsp;PHR
				</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="700" border=0 cellspacing=0>
			<tr>
				<td>
					<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="680">
						<tr vAlign="top">
							<td><asp:label id="LabelAvailableItems" runat="server" Width="140px" cssclass="Label_Small">Available Items</asp:label></td>
							<td>&nbsp;</td>
							<td><asp:label id="LabelSelectedItems" runat="server" Width="140px" cssclass="Label_Small">Selected Items</asp:label></td>
						</tr>
					</table>
					<table>
						<tr>
							<td>
								<div class="Div_Left">
									<table>
										<tr vAlign="top">
											<td align="left"><asp:listbox id="ListBoxAvailableItem" runat="server" Width="225px" SelectionMode="Multiple"
													Height="208px"></asp:listbox></td>
											<td>
												<table>
													<tr>
														<td align="center"><asp:button id="ButtonAdd" Width="100" cssclass="Button_Normal" Runat="server" Text="Add>"></asp:button></td>
													</tr>
													<tr>
														<td>&nbsp;</td>
													</tr>
													<tr>
														<td align="center"><asp:button id="ButtonAddAll" Width="100" cssclass="Button_Normal" Runat="server" Text="Add All>>"></asp:button></td>
													</tr>
													<tr>
														<td>&nbsp;</td>
													</tr>
													<tr>
														<td align="center"><asp:button id="ButtonRemove" Width="100" cssclass="Button_Normal" Runat="server" Text="< Remove"></asp:button></td>
													</tr>
													<tr>
														<td height="14">&nbsp;</td>
													</tr>
													<tr>
														<td align="center"><asp:button id="ButtonRemoveAll" Width="100" cssclass="Button_Normal" Runat="server" Text="<< Remove All"></asp:button></td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</div>
							</td>
							<td>
								<div class="Div_Right">
									<table>
										<tr>
											<td><asp:listbox id="ListBoxSelectedItems" runat="server" Width="225px" SelectionMode="Multiple"
													Height="208px"></asp:listbox></td>
										</tr>
									</table>
								</div>
							</td>							
						</tr>
						<tr>
							<td colspan=2>&nbsp;</td>
						</tr>						
					</table>
				</td>
			</tr>
			<tr>
				<td colspan=2 class="Td_ButtonContainer" align=right>
					<asp:button id="btnOK" Width="73" cssclass="Button_Normal" Runat="server" Text="OK"></asp:button>
					<asp:button id="btnCancel" cssclass="Button_Normal" Width="73" Text="Cancel" Runat="server"></asp:button>							
				</td>
			</tr>
		</table>
	</div>
</form>
<!--#include virtual="bottom.html"-->
