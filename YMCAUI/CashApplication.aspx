<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CashApplication.aspx.vb" Inherits="YMCAUI.CashApplication"%>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="YRSControls" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<!--#include virtual="top.html"-->
<script language="javascript">
function FormatYMCANo()
{
	var diff;
	var flg = false;
	var str = String(document.Form1.all.TextBoxYmcaNo.value);
	var len;
	len =str.length;
	if (len==0)
	{
		return false;
	}
	else if (len<6)
	{
		diff=(6-len);
		for (i=0;i<diff;i++)
		{
			str="0" + str
			
		}
	}
document.Form1.all.TextBoxYmcaNo.value=str		
}	
</script>
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" width="700">
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
					CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
					MenuFadeDelay="2" mouseovercssclass="MouseOver">
					<SELECTEDMENUITEMSTYLE BackColor="#FBC97A" ForeColor="#3B5386"></SELECTEDMENUITEMSTYLE>
				</cc1:menu></td>
		</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
				Cash Application<asp:label id="LabelHdr" runat="server" CssClass="Td_HeadingFormContainer"></asp:label>
			</td>
		</tr>
		<tr>
			<td>&nbsp;
			</td>
		</tr>
	</table>
	<div class="center">
		<table width="700">
			<tr>
				<td><iewc:tabstrip id="TabStripCashApplication" runat="server" BorderStyle="None" AutoPostBack="True"
						TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
						TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
						Width="700px" Height="30px">
						<iewc:Tab Text="List" ID="tabList"></iewc:Tab>
						<iewc:Tab Text="Transmittals" ID="tabTransmittals"></iewc:Tab>
					</iewc:tabstrip></td>
			</tr>
			<tr>
				<td><iewc:multipage id="MultiPageCashApplication" runat="server">
						<iewc:PageView>
							<table class="Table_WithBorder" width="695">
								<tr valign="top">
									<td>&nbsp;</td>
									<td>&nbsp;</td>
								</tr>
								<tr valign="top">
									<td valign="top">
										<asp:Label ID="LabelRecordNotFound" Runat="server" CssClass="Label_Small" visible="false">No Matching Records</asp:Label>
										<DIV style="OVERFLOW: auto; WIDTH: 400px; HEIGHT: 200px; TEXT-ALIGN: left">
											<asp:DataGrid ID="DataGridYmca" Runat="server" Width="400" CssClass="DataGrid_Grid" CellPadding="0"
												CellSpacing="0" AutoGenerateColumns="True">
												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
												<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
												<Columns>
													<asp:TemplateColumn>
														<ItemTemplate>
															<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																CommandName="Select" ToolTip="Select"></asp:ImageButton>
														</ItemTemplate>
													</asp:TemplateColumn>
												</Columns>
											</asp:DataGrid>
										</DIV>
									</td>
									<td>
										<table>
											<tr>
												<td>
													<asp:Label ID="LabelYmcaNo" Runat="server" CssClass="Label_Small">YMCA No.</asp:Label>
												</td>
												<td>
													<asp:textbox width="100" runat="server" ID="TextBoxYmcaNo" name="TextBoxYmcaNo" CssClass="TextBox_Normal"
														MaxLength="6"></asp:textbox>
												</td>
											</tr>
											<tr>
												<td>
													<asp:Label ID="LabelYmcaName" Runat="server" CssClass="Label_Small">YMCA Name</asp:Label>
												</td>
												<td>
													<asp:textbox width="100" runat="server" ID="TextBoxYmcaName" CssClass="TextBox_Normal" MaxLength="60"></asp:textbox>
												</td>
											</tr>
											<tr>
												<td>
													<asp:Label ID="LabelCity" Runat="server" CssClass="Label_Small">City</asp:Label>
												</td>
												<td>
													<asp:textbox width="100" runat="server" ID="TextBoxCity" CssClass="TextBox_Normal" MaxLength="30"></asp:textbox>
												</td>
											</tr>
											<tr>
												<td>
													<asp:Label ID="LabelState" Runat="server" CssClass="Label_Small">State</asp:Label>
												</td>
												<td>
													<asp:textbox width="100" runat="server" ID="TextBoxState" CssClass="TextBox_Normal" MaxLength="2"></asp:textbox>
												</td>
											</tr>
											<tr>
												<td align="left">
													<asp:Button Width="80" Runat="server" ID="ButtonFind" Text="Find" CssClass="Button_Normal"></asp:Button>
												</td>
												<td align="right">
													<asp:Button Width="80" Runat="server" ID="ButtonClear" Text="Clear" CssClass="Button_Normal"></asp:Button>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</iewc:PageView>
						<iewc:pageview>
							<table class="Table_WithBorder" width="698">
								<tr>
									<td>
										<table>
											<tr>
												<td align="center" width="50%"><asp:label id="LabelHFundingDate" CssClass="Label_Small" Runat="server"> Funding Date: </asp:label></td>
												<td><YRSControls:DateUserControl onpaste="return true" id="DateusercontrolFundedDate" runat="server"></YRSControls:DateUserControl></td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td colspan="4" align="right" valign="top">
										<asp:Label ID="LabelCredits" Runat="server" CssClass="Label_Small">Credits Available</asp:Label>
										<asp:textbox width="100" runat="server" ID="TextBoxCredits" CssClass="TextBox_Normal_Amount"
											ReadOnly="true"></asp:textbox>
										<asp:ImageButton id="ImageButtonCredits" runat="server" Height="18px" Width="18px" ImageUrl="images/ButtonCashApp.gif"></asp:ImageButton>
									</td>
								</tr>
								<tr>
									<td colspan="4" valign="top">
										<DIV style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 200px; TEXT-ALIGN: left">
											<asp:DataGrid ID="DataGridTransmit" Runat="server" Width="690" CssClass="DataGrid_Grid" CellPadding="0"
												CellSpacing="0" AutoGenerateColumns="False">
												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
												<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
												<Columns>
													<asp:TemplateColumn HeaderText="">
														<ItemTemplate>
															<asp:CheckBox id="CheckBoxSelect" runat="server" autopostback=true OnCheckedChanged="Check_Clicked" Checked='<%# DataBinder.Eval(Container.DataItem, "Slctd") %>'>
															</asp:CheckBox>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="TransmittalNo" HeaderText="Transmittal No" />
													<asp:BoundColumn DataField="TransmittalDate" HeaderText="Transmittal Date" />
													
													<asp:BoundColumn DataField="AmtDue" HeaderText="Total Amount" DataFormatString="{0:N}">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="TotAppliedRcpts" HeaderText="Receipts Applied" DataFormatString="{0:N}">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="TotAppliedCredit" HeaderText="CR Applied" DataFormatString="{0:N}">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="Balance" HeaderText="Balance" DataFormatString="{0:N}">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
												</Columns>
											</asp:DataGrid>
										</DIV>
									</td>
								</tr>
								<tr>
									<td colspan="4" align="right" valign="top">
										<asp:textbox width="70" runat="server" ID="TextBoxTotalAmount" CssClass="TextBox_Normal_Amount"
											ReadOnly="true"></asp:textbox>
										<asp:textbox width="100" runat="server" ID="TextBoxTotalReceipts" CssClass="TextBox_Normal_Amount"
											ReadOnly="true"></asp:textbox>
										<asp:textbox width="70" runat="server" ID="TextBoxTotalCredits" CssClass="TextBox_Normal_Amount"
											ReadOnly="true"></asp:textbox>
										<asp:textbox width="70" runat="server" ID="TextBoxTotalBalance" CssClass="TextBox_Normal_Amount"
											ReadOnly="true"></asp:textbox>
									</td>
								</tr>
								<tr>
									<td width="50%" valign="top">
										<DIV style="OVERFLOW: auto;  HEIGHT: 200px; TEXT-ALIGN: left">
											<asp:DataGrid ID="DataGridReceipts" Runat="server" CssClass="DataGrid_Grid" CellPadding="0" CellSpacing="0"
												AutoGenerateColumns="False" width="100%">
												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
												<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
												<Columns>
													<asp:TemplateColumn>
														<ItemTemplate>
															<asp:ImageButton id="Imagebutton1" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																CommandName="Select" ToolTip="Select"></asp:ImageButton>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="ReceiptId" HeaderText="Receipt Number" />
													<asp:BoundColumn DataField="ReceiptIdDate" HeaderText="Check Date" DataFormatString="{0:d}" />
													<asp:BoundColumn DataField="Receiveddate" HeaderText="Received Date" DataFormatString="{0:d}" />
													<asp:BoundColumn DataField="Amount" HeaderText="Amount" DataFormatString="{0:N}">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundColumn>
												</Columns>
											</asp:DataGrid>
										</DIV>
									</td>
									<td width="4%" valign="top" align="right">
										<asp:ImageButton id="ImageButtonReceipts" runat="server" Height="18px" Width="18px" ImageUrl="images/ButtonCashApp.gif"
											ToolTip="Apply Receipts"></asp:ImageButton>
									</td>
								</tr>
							</table>
						</iewc:pageview>
					</iewc:multipage></td>
			</tr>
			<tr>
				<td><IMG title="image" height="1" alt="image" src="images/spacer.gif" width="1"></td>
			</tr>
			<tr>
				<td>
					<table class="Table_WithBorder" width="695">
						<tr>
							<td class="Td_ButtonContainer" align="right"><asp:button id="ButtonPayItem" CssClass="Button_Normal" Width="80" Runat="server" Text="Pay Item"
									Enabled="False"></asp:button></td>
							<td class="Td_ButtonContainer" align="right"><asp:button id="ButtonClose" CssClass="Button_Normal" Width="80" Runat="server" Text="OK" CausesValidation="False"></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<asp:placeholder id="PlaceHolderMessageBox" runat="server"></asp:placeholder></form>
<!--#include virtual="bottom.html"-->
