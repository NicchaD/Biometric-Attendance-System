<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DisbursementsReissue.aspx.vb" Inherits="YMCAUI.DisbursementsReissue"%>
<!--#include virtual="TopNew.htm"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">Disbursement's 
					Breakdown</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<div class="Div_Center">
		<table width="697">
			<tr>
				<td style="display:none"><iewc:tabstrip id="TabStripBreakDown" runat="server" BorderStyle="None" AutoPostBack="True" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
						TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
						Height="30px" Width="690px">
						<iewc:Tab Text="List" ID="tabList"></iewc:Tab>
						
					</iewc:tabstrip></td>
			</tr>
			<tr>
				<td><iewc:multipage id="MultiPageBreakDown" runat="server">
						<iewc:PageView>
							<table class="Table_WithBorder" width="690">
								<tr>
									<td align="left">
										<DIV style="OVERFLOW: auto; WIDTH: 680px; HEIGHT: 80px; TEXT-ALIGN: left;border=1">
											<asp:DataGrid ID="dgDisbursements" Runat="server" Width="680px" CssClass="DataGrid_Grid" CellPadding="0"
												CellSpacing="0" AutoGenerateColumns="false">
												<HeaderStyle cssclass="DataGrid_HeaderStyle_temp"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
												<SelectedItemStyle CssClass="DataGrid_SelectedStyle_temp"></SelectedItemStyle>
												<Columns>
												<asp:BoundColumn DataField="DisbursementID" HeaderText="DisbursementID" visible="false" ></asp:BoundColumn>
												<asp:TemplateColumn>
														<ItemTemplate>
															<asp:ImageButton id="imgdisbursements" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																CommandName="Select" ToolTip="Select"></asp:ImageButton>
														</ItemTemplate>
													</asp:TemplateColumn>
													
													<asp:BoundColumn DataField="Number" HeaderText="Number"></asp:BoundColumn>
													<asp:BoundColumn DataField="GrossAmount" HeaderText="Amount"></asp:BoundColumn>
													<asp:BoundColumn HeaderText="Adjustment Amount"></asp:BoundColumn>
													<asp:BoundColumn HeaderText="Remaining Amount" DataField="GrossAmount"></asp:BoundColumn>
													
												</Columns>
											</asp:DataGrid>
										</DIV>
									</td>
								</tr>
								<tr>
								<td align="left">
									Transactions from database
								</td>
								</tr>
								<tr>
								<td>
								&nbsp;	
								</td>
								</tr>
								<tr>
									<td>
										<DIV style="OVERFLOW: auto; WIDTH: 680px; HEIGHT: 100px; TEXT-ALIGN: center">
											<asp:DataGrid ID="DataGridTransacts" Runat="server" Width="650" CssClass="DataGrid_Grid" CellPadding="0"
												CellSpacing="0" AutoGenerateColumns="false">
												<HeaderStyle cssclass="DataGrid_HeaderStyle_temp"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
												<SelectedItemStyle CssClass="DataGrid_SelectedStyle_temp"></SelectedItemStyle>
												<Columns>
													<asp:TemplateColumn>
														<ItemTemplate>
															<asp:ImageButton id="ImageButtonSel" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																CommandName="Select" ToolTip="Select"></asp:ImageButton>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn DataField="TransactID" HeaderText="TransactID"></asp:BoundColumn>
											<asp:BoundColumn DataField="FundEventId" HeaderText="FundEventId" visible=false></asp:BoundColumn>
											<asp:BoundColumn DataField="AcctType" HeaderText="Acct Type"></asp:BoundColumn>
											<asp:BoundColumn DataField="TType" HeaderText="TType"></asp:BoundColumn>
											<asp:BoundColumn DataField="TDate" HeaderText="TDate"></asp:BoundColumn>
											<asp:BoundColumn DataField="PPreTax" HeaderText="PPreTax"></asp:BoundColumn>
											<asp:BoundColumn DataField="PPostTax" HeaderText="PPostTax"></asp:BoundColumn>
											<asp:BoundColumn DataField="YPreTax" HeaderText="YPreTax"></asp:BoundColumn>
											<asp:BoundColumn DataField="Basis" HeaderText="Basis"></asp:BoundColumn>		
												</Columns>
											</asp:DataGrid>
										</DIV>
									</td>
								</tr>
								<tr>
								<td align="left">
									Transactions for Adjustment
								</td>
								</tr>
								<tr>
								<td>
								&nbsp;	
								</td>
								</tr>
								<tr>
									<td>
										<DIV style="OVERFLOW: auto; WIDTH: 680px; HEIGHT: 100px; TEXT-ALIGN: center">
											<asp:DataGrid ID="DataGridNewTransacts" Runat="server" Width="650" CssClass="DataGrid_Grid" CellPadding="0"
												CellSpacing="0" AutoGenerateColumns="False">
												<HeaderStyle cssclass="DataGrid_HeaderStyle_temp"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
												<SelectedItemStyle CssClass="DataGrid_SelectedStyle_temp"></SelectedItemStyle>
												<Columns>
													<asp:TemplateColumn>
														<ItemTemplate>
															<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																CommandName="Select" ToolTip="Select"></asp:ImageButton>
														</ItemTemplate>
													</asp:TemplateColumn>
													
											<asp:BoundColumn DataField="FundEventId" HeaderText="FundEventId" visible=false></asp:BoundColumn>
											<asp:BoundColumn DataField="AcctType" HeaderText="Acct Type"></asp:BoundColumn>
											<asp:BoundColumn DataField="TType" HeaderText="TType"></asp:BoundColumn>
											<asp:BoundColumn DataField="TDate" HeaderText="TDate"></asp:BoundColumn>
											<asp:BoundColumn DataField="PPreTax" HeaderText="PPreTax"></asp:BoundColumn>
											<asp:BoundColumn DataField="PPostTax" HeaderText="PPostTax"></asp:BoundColumn>
											<asp:BoundColumn DataField="YPreTax" HeaderText="YPreTax"></asp:BoundColumn>
											<asp:BoundColumn DataField="Basis" HeaderText="Basis"></asp:BoundColumn>		
											<asp:BoundColumn DataField="DisbursementID" HeaderText="DisbursementID"></asp:BoundColumn>
												</Columns>
											</asp:DataGrid>
										</DIV>
									</td>
								</tr>
								<tr>
									<td>
										<table>
											<tr>
												<td>
													<asp:Label ID="LabelPreTax" Runat="server" CssClass="Label_Small">Personal Pre Tax</asp:Label></td>
												<td>
													<asp:textbox width="100" runat="server" ID="TextBoxPreTax" CssClass="TextBox_Normal"></asp:textbox>
												</td>
												<td>
													<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="Enter a proper decimal value."
														ValidationExpression="^\d*\.{0,1}\d{1,2}$" ControlToValidate="TextBoxPreTax"></asp:RegularExpressionValidator></td>
											</tr>
											<tr>
												<td>
													<asp:Label ID="LabelPostTax" Runat="server" CssClass="Label_Small">Personal Post Tax</asp:Label></td>
												<td>
													<asp:textbox width="100" runat="server" ID="TextBoxPostTax" CssClass="TextBox_Normal"></asp:textbox></td>
												<td>
													<asp:RegularExpressionValidator id="Regularexpressionvalidator2" runat="server" ErrorMessage="Enter a proper decimal value."
														ValidationExpression="^\d*\.{0,1}\d{1,2}$" ControlToValidate="TextBoxPostTax"></asp:RegularExpressionValidator></td>
											</tr>
											<tr>
												<td>
													<asp:Label ID="LabelYMCAPreTax" Runat="server" CssClass="Label_Small">YMCA Pre Tax</asp:Label></td>
												<td>
													<asp:textbox width="100" runat="server" ID="TextBoxYMCAPreTax" CssClass="TextBox_Normal"></asp:textbox></td>
												<td>
													<asp:RegularExpressionValidator id="Regularexpressionvalidator3" runat="server" ErrorMessage="Enter a proper decimal value."
														ValidationExpression="^\d*\.{0,1}\d{1,2}$" ControlToValidate="TextBoxYMCAPreTax"></asp:RegularExpressionValidator></td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td class="Td_ButtonContainer" align="right">
										<asp:button id="ButtonUpdateDisbursements" CssClass="Button_Normal" Runat="server" Text="Update Disbursements"></asp:button></td>
								</tr>
							</table>
						</iewc:PageView>
						<iewc:PageView>
							<table class="Table_WithBorder" width="690">
								<tr>
									<td>
										<DIV style="OVERFLOW: auto; WIDTH: 680px; HEIGHT: 100px; TEXT-ALIGN: center">
											<asp:DataGrid ID="DataGridDisbursementDetails" Runat="server" Width="580" CssClass="DataGrid_Grid"
												CellPadding="0" CellSpacing="0" AutoGenerateColumns="True">
												<HeaderStyle cssclass="DataGrid_HeaderStyle_temp"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
												<SelectedItemStyle CssClass="DataGrid_SelectedStyle_temp"></SelectedItemStyle>
												<Columns>
													<asp:TemplateColumn>
														<ItemTemplate>
															<asp:ImageButton id="ImageButtonSelect2" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																CommandName="Select" ToolTip="Select"></asp:ImageButton>
														</ItemTemplate>
													</asp:TemplateColumn>
												</Columns>
											</asp:DataGrid>
										</DIV>
									</td>
								</tr>
								<tr>
									<td>
										<table>
											<tr>
												<td></td>
												<td></td>
												<td></td>
												<td>
													<asp:Label ID="LabelTaxablePrinciple" Runat="server" CssClass="Label_Small">Taxable Principle</asp:Label></td>
												<td>
													<asp:textbox width="100" runat="server" ID="TextBoxTaxablePrinciple" CssClass="TextBox_Normal"
														Enabled="False"></asp:textbox></td>
												<td></td>
											</tr>
											<tr>
												<td></td>
												<td></td>
												<td></td>
												<td>
													<asp:Label ID="LabelNonTaxablePrinciple" Runat="server" CssClass="Label_Small">Non TaxablePrinciple</asp:Label></td>
												<td>
													<asp:textbox width="100" runat="server" ID="TextBoxNonTaxablePrinciple" CssClass="TextBox_Normal"
														Enabled="False"></asp:textbox></td>
												<td></td>
											</tr>
											<tr>
												<td></td>
												<td></td>
												<td></td>
												<td>
													<asp:Label ID="LabelTaxWithheldPrinciple" Runat="server" CssClass="Label_Small">Tax Withheld Principle</asp:Label></td>
												<td>
													<asp:textbox width="100" runat="server" ID="TextBoxTaxWithheldPrinciple" CssClass="TextBox_Normal"></asp:textbox></td>
												<td>
													<asp:RegularExpressionValidator id="Regularexpressionvalidator4" runat="server" ErrorMessage="Enter a proper decimal value."
														ValidationExpression="^\d*\.{0,1}\d{1,2}$" ControlToValidate="TextBoxTaxWithheldPrinciple"></asp:RegularExpressionValidator></td>
											</tr>
											<tr>
												<td></td>
												<td></td>
												<td></td>
												<td>
													<asp:Label ID="LabelTaxableInterest" Runat="server" CssClass="Label_Small">Taxable Interest</asp:Label></td>
												<td>
													<asp:textbox width="100" runat="server" ID="TextBoxTaxableInterest" CssClass="TextBox_Normal"
														Enabled="False"></asp:textbox></td>
												<td></td>
											<tr>
												<td>
													<asp:Label ID="LabelFactor" Runat="server" CssClass="Label_Small">Factor</asp:Label></td>
												<td>
													<asp:textbox width="100" runat="server" ID="TextBoxFactor" CssClass="TextBox_Normal" AutoPostBack="True"></asp:textbox></td>
												<td></td>
												<td>
													<asp:Label ID="LabelWithheldInterest" Runat="server" CssClass="Label_Small">Withheld Interest</asp:Label></td>
												<td>
													<asp:textbox width="100" runat="server" ID="TextBoxWithheldInterest" CssClass="TextBox_Normal"></asp:textbox></td>
												<td>
													<asp:RegularExpressionValidator id="Regularexpressionvalidator5" runat="server" ErrorMessage="Enter a proper decimal value."
														ValidationExpression="^\d*\.{0,1}\d{1,2}$" ControlToValidate="TextBoxWithheldInterest"></asp:RegularExpressionValidator></td>
											</tr>
								</tr>
							</table>
				</td>
			</tr>
			<tr>
				<td class="Td_ButtonContainer" align="right">
					<asp:button id="ButtonUpdate" CssClass="Button_Normal" Runat="server" Text="Update Details"></asp:button></td>
			</tr>
		</table>
		</iewc:PageView> </iewc:multipage></td></tr>
		<tr>
			<td>
				<table class="Table_WithBorder" width="690" >
					<tr style="display:none">
						<td><asp:label id="LabelGross" CssClass="Label_Small" Runat="server">Gross</asp:label></td>
						<td><asp:textbox id="TextBoxGross" runat="server" CssClass="TextBox_Normal" width="100" Enabled="False"></asp:textbox></td>
						<td><asp:label id="LabelWithheld" CssClass="Label_Small" Runat="server">W/H</asp:label></td>
						<td><asp:textbox id="TextBoxWithheld" runat="server" CssClass="TextBox_Normal" width="100" Enabled="False"></asp:textbox></td>
						<td><asp:label id="LabelNet" CssClass="Label_Small" Runat="server">Net</asp:label></td>
						<td><asp:textbox id="TextBoxNet" runat="server" CssClass="TextBox_Normal" width="100" Enabled="False"></asp:textbox></td>
					</tr>
					<tr>
						<td class="Td_ButtonContainer" align="right" colSpan="5"><asp:button id="ButtonSave" Width="70" CssClass="Button_Normal" Runat="server" Text="Done"></asp:button></td>
						<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonCancel" Width="70" CssClass="Button_Normal" Runat="server" Text="Cancel"></asp:button></td>
					</tr>
				</table>
			</td>
		</tr>
		</table></div>
	<asp:PlaceHolder id="PlaceHolderMsg" runat="server"></asp:PlaceHolder>
</form>
<!--#include virtual="bottom.html"-->
