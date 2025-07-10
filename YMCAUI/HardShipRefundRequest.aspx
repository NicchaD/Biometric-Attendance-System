<%@ Page Language="vb" AutoEventWireup="false" Codebehind="HardShipRefundRequest.aspx.vb" Inherits="YMCAUI.HardShipRefundRequest"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<script language="javascript">
		document.all("TextboxRequestAmount").focus();
		function TextboxRequestAmount_Change()
		{
			if(document.all("TextboxRequestAmount").value =="" )
			{
				
				document.all("TextboxRequestAmount").focus();
			
			}
			else
			{
				if(!isNaN( document.all("TextboxRequestAmount").value))
				{
					if( document.all("TextboxRequestAmount").value > 0)
					{
					
						return;
					}
			
			
				}
				document.all("TextboxRequestAmount").focus();
			}
		}
		function ValidateDecimal()
				{	
					//alert(event.keyCode);
					if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 46)
					{
						event.returnValue = false;
					}
				}	
	</script>
	<div class="Div_Center">
		<table width="100%">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					HardShip Withdrawal Request Processing
				</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<table class="Table_WithBorder" width="698">
		<tr>
			<td><iewc:tabstrip id="TabStripVoluntaryRefund" runat="server" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
					TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
					AutoPostBack="True" Width="700px">
					<iewc:Tab Text=" Account Information "></iewc:Tab>
					<iewc:Tab Text=" Distribution "></iewc:Tab>
				</iewc:tabstrip></td>
		<tr>
			<td><iewc:multipage id="MultiPageVoluntaryRefund" runat="server">
					<iewc:PageView>
						<table class="Table_WithoutBorder" width="100%">
							<tr>
								<td></td>
							</tr>
							<tr>
								<td class="td_Text" align="left" height="0"></td>
							</tr>
							<tr>
								<td align="center">
									<table width="630" align="center">
										<tr>
										   <td align="center" nowrap>
												<asp:Label id="LabelPlanChosen" runat="server" CssClass="Label_Small">PlanChosen </asp:Label>
												<asp:TextBox id="TextboxPlanChosen" runat="server" CssClass="TextBox_Normal" Width="140"></asp:TextBox>
											</td>
											<td align="right" nowrap>
												<asp:Label id="LabelStatus" runat="server" CssClass="Label_Small">Status </asp:Label>
												<asp:TextBox id="TextBoxStatus" runat="server" CssClass="TextBox_Normal" Width="60"></asp:TextBox>
											</td>
											<td align="right" nowrap>
												<asp:Label id="LabelTerminationPIA" runat="server" CssClass="Label_Small">Termination PIA</asp:Label>
												<asp:TextBox id="TextBoxTerminationPIA" runat="server" CssClass="TextBox_Normal" Width="75"></asp:TextBox>
											</td>
											<td align="center" nowrap>
												<asp:Label id="LabelCurrentPIA" runat="server" Width="100px" CssClass="Label_Small">Current PIA</asp:Label>
												<asp:TextBox id="TextBoxCurrentPIA" runat="server" CssClass="TextBox_Normal" Width="75"></asp:TextBox>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td class="td_Text" align="left" height="0"></td>
							</tr>
							<tr>
								<td>&nbsp;</td>
							</tr>
							<TR>
								<TD align="left" class="td_Text_Small" colspan="9">
									&nbsp;Requested Accounts
								</TD>
							<tr>
								<td colspan="9" align="right">
									<DIV style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 150px">
										<asp:DataGrid id="DataGridRequestedAccts" runat="server" Width="680px" CssClass="DataGrid_Grid">
											<HeaderStyle cssclass="DataGrid_HeaderStyle_temp"></HeaderStyle>
											<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
											<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
											<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
										</asp:DataGrid>
									</DIV>
								</td>
							</tr>
							<tr>
								<td colspan="9">&nbsp;</td>
							</tr>
							<TR>
								<TD align="left" class="td_Text_Small" colspan="9">
									&nbsp;Current Accounts
								</TD>
							</TR>
							<tr>
								<td colspan="9" align="right">
									<DIV style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 150px">
										<asp:DataGrid id="DatagridCurrentAccts" runat="server" Width="680px" CssClass="DataGrid_Grid">
											<HeaderStyle cssclass="DataGrid_HeaderStyle_temp"></HeaderStyle>
											<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
											<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
											<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
										</asp:DataGrid>
									</DIV>
								</td>
							</tr>
							<tr>
								<td colspan="9">&nbsp;</td>
							</tr>
							<TR>
								<TD align="left" class="td_Text_Small" colspan="9">
									&nbsp;Non - Funded Accounts
								</TD>
							</TR>
							<tr>
								<td colspan="9" align="right">
									<DIV style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 150px">
										<asp:DataGrid id="DatagridNonFundedContributions" runat="server" Width="680px" CssClass="DataGrid_Grid">
											<HeaderStyle cssclass="DataGrid_HeaderStyle_temp"></HeaderStyle>
											<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
											<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
											<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
										</asp:DataGrid>
									</DIV>
								</td>
							</tr>
						</table>
						<table class="Table_WithOutBorder" width="100%">
							<tr>
								<td>&nbsp;
								</td>
							</tr>
							<tr class="Td_ButtonContainer">
								<td align="right">
									<asp:Button id="ButtonTab1OK" cssClass="Button_Normal" runat="server" Text="OK" width="80"></asp:Button>
								</td>
							</tr>
						</table>
					</iewc:PageView>
					<iewc:PageView>
						<table class="Table_WithBorder" width="698">
							<tr>
								<td width="240"></td>
								<td width="65">
									<asp:label id="LabelYes1" runat="server" Width="30">Yes</asp:label>
									<asp:label id="LabelNo1" runat="server" Width="30">No</asp:label></td>
								<td width="290"></td>
								<td width="65">
									<asp:label id="LabelYes2" runat="server" Width="30">Yes</asp:label>
									<asp:label id="LabelNo2" runat="server" Width="30">No</asp:label></td>
							</tr>
							<tr>
								<td align="left">
									<asp:label id="LabelReleaseSigned" runat="server">Has the Release Form been signed?</asp:label></td>
								<td>
									<asp:radiobutton id="RadioButtonReleaseSignedYes" runat="server" Text=" " GroupName="GrpReleaseSigned"
										autoPostBack="true"></asp:radiobutton>&nbsp;
									<asp:radiobutton id="RadioButtonReleaseSignedNo" runat="server" Text=" " Checked="True" GroupName="GrpReleaseSigned"
										autoPostBack="true"></asp:radiobutton></td>
								<td align="left">
									<asp:label id="LabelAddressUpdating" runat="server">Does the Address need updating?</asp:label></td>
								<td>
									<asp:radiobutton id="RadioButtonAddressUpdatingYes" runat="server" Text=" " GroupName="GrpAddressUpdating"
										autoPostBack="true"></asp:radiobutton>&nbsp;
									<asp:radiobutton id="RadioButtonAddressUpdatingNo" runat="server" Text=" " Checked="True" GroupName="GrpAddressUpdating"
										autoPostBack="true"></asp:radiobutton></td>
							</tr>
							<tr>
								<td align="left">
									<asp:label id="LabelNotarized" runat="server">Has the Release Form been notarized?</asp:label></td>
								<td>
									<asp:radiobutton id="RadioButtonNotarizedYes" runat="server" Text=" " GroupName="GrpNotarized" autoPostBack="true"></asp:radiobutton>&nbsp;
									<asp:radiobutton id="RadioButtonNotarizedNo" runat="server" Text=" " Checked="True" GroupName="GrpNotarized"
										autoPostBack="true"></asp:radiobutton></td>
								<td align="left">
									<asp:label id="LabelRollover" runat="server">Did the participant request a rollover?</asp:label></td>
								<td>
									<asp:radiobutton id="RadioButtonRolloverYes" runat="server" Text=" " ENABLED="FALSE" GroupName="GrpRollover"
										autoPostBack="true"></asp:radiobutton>&nbsp;
									<asp:radiobutton id="RadioButtonRolloverNo" runat="server" Text=" " ENABLED="FALSE" Checked="True"
										GroupName="GrpRollover" autoPostBack="true"></asp:radiobutton></td>
							</tr>
							<tr>
								<td align="left">
									<asp:label id="LabelWaiver" runat="server">Has the spouse signed a waiver?</asp:label></td>
								<td>
									<asp:radiobutton id="RadioButtonWaiverYes" runat="server" Text=" " GroupName="GrpWaiver" autoPostBack="true"></asp:radiobutton>&nbsp;
									<asp:radiobutton id="RadioButtonWaiverNo" runat="server" Text=" " Checked="True" GroupName="GrpWaiver"
										autoPostBack="true"></asp:radiobutton></td>
								<td align="left">
									<asp:label id="LabelAddnlWitholding" runat="server">Did the participant request additional witholding?</asp:label></td>
								<td>
									<asp:radiobutton id="RadioButtonAddnlWitholdingYes" runat="server" Text=" " GroupName="GrpWitholding"
										autoPostBack="true"></asp:radiobutton>&nbsp;
									<asp:radiobutton id="RadioButtonAddnlWitholdingNo" runat="server" Text=" " Checked="True" GroupName="GrpWitholding"
										autoPostBack="true"></asp:radiobutton>
								</td>
							</tr>
						</table>
						<table width="690" class="Table_WithoutBorder">
							<tr>
								<td>
									<table width="250">
										<tr>
											<td>
												<table height="50">
													<tr>
														<td align="left" height="15">
															<asp:radiobutton id="RadioButtonNone" runat="server" Text="Partial Amount" Checked="True" GroupName="AccountRollover"
																autoPostBack="true"></asp:radiobutton></td>
													</tr>
													<tr>
														<td align="left" height="15">
															<asp:radiobutton id="RadioButtonRolloverAll" runat="server" Text="Rollover All" GroupName="AccountRollover"
																autoPostBack="true"></asp:radiobutton></td>
													</tr>
													<tr>
														<td align="left" height="15">
															<asp:radiobutton id="RadioButtonTaxableOnly" runat="server" Width="88px" Text="TaxableOnly" GroupName="AccountRollover"
																autoPostBack="true"></asp:radiobutton></td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td colSpan="2">&nbsp;</td>
										</tr>
										<tr>
											<td class="td_Text" align="left" colSpan="2" height="0"></td>
										</tr>
										<tr>
											<td align="right">
												<asp:label id="Label1" runat="server">TD Available : </asp:label>
												<asp:textbox id="TextboxTDAvailableAmount" runat="server" Width="80" cssClass="TextBox_Normal"></asp:textbox>
											</td>
										</tr>
										<tr>
											<td align="right">
												<asp:label id="Label2" runat="server">Request Amount : </asp:label>
												<asp:textbox id="TextboxRequestAmount" runat="server" Width="80" cssClass="TextBox_Normal" autoPostback="true"></asp:textbox>
											</td>
										</tr>
										<tr>
											<td>&nbsp;
											</td>
										</tr>
										<tr>
											<td class="td_Text" align="left" colSpan="2" height="0"></td>
										</tr>
										<tr>
											<td colSpan="2" align="left">
												<asp:label id="LabelPayee1" runat="server" Width="50">Payee1</asp:label>
												<asp:textbox id="TextBoxPayee1" runat="server" Width="120" cssClass="TextBox_Normal"></asp:textbox></td>
										</tr>
										<tr>
											<td colSpan="2" align="left">
												<div style="OVERFLOW: auto; WIDTH: 250px; HEIGHT: 80px">
													<asp:datagrid id="DataGridPayee1" runat="server" Width="200px" CssClass="DataGrid_Grid" AutoGenerateColumns="False">
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
														<SelectedItemStyle CssClass="DataGrid_SelectedStyle_Total"></SelectedItemStyle>
														<Columns>
															<asp:TemplateColumn>
																<ItemTemplate>
																	<asp:Label id="LabelPayee1AcctType" runat="server" cssclass="Label_Normal" Text='<%# DataBinder.Eval(Container.DataItem, "AcctType") %>'>
																	</asp:Label>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn>
																<ItemTemplate>
																	<asp:Label id="LabelPayee1Taxable" runat="server" autopostback=true OnTextChanged="Text_Changed" cssclass="TextBox_Normal" Width="55px" Text='<%# DataBinder.Eval(Container.DataItem, "Taxable") %>'>
																	</asp:Label>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn>
																<ItemTemplate>
																	<asp:Label id="LabelPayee1NonTaxable" runat="server" autopostback=true OnTextChanged="Text_Changed" cssclass="TextBox_Normal" Width="55px" Text='<%# DataBinder.Eval(Container.DataItem, "NonTaxable") %>'>
																	</asp:Label>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:datagrid>
												</div>
											</td>
										</tr>
										<tr>
											<td colSpan="2" align="left">
												<asp:label id="LabelPayee2" runat="server">Payee2</asp:label>
												<asp:textbox id="TextboxPayee2" runat="server" Width="120" cssClass="TextBox_Normal" autopostback="true"></asp:textbox></td>
										</tr>
										<tr>
											<td align="left">
												<div style="OVERFLOW: auto; WIDTH: 250px; HEIGHT: 80px">
													<asp:datagrid id="DatagridPayee2" runat="server" Width="200px" CssClass="DataGrid_Grid" AutoGenerateColumns="False">
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
														<SelectedItemStyle CssClass="DataGrid_SelectedStyle_Total"></SelectedItemStyle>
														<Columns>
															<asp:TemplateColumn>
																<ItemTemplate>
																	<asp:Label id="LabelAcctType" runat="server" cssclass="Label_Normal" Text='<%# DataBinder.Eval(Container.DataItem, "AcctType") %>'>
																	</asp:Label>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn>
																<ItemTemplate>
																	<asp:TextBox id="TextboxPayee2Taxable" enabled = false runat="server" autopostback=true OnTextChanged="Text_Changed" MaxLength= "10" cssclass="TextBox_Normal" Width="55px" Text='<%# DataBinder.Eval(Container.DataItem, "Taxable") %>'>
																	</asp:TextBox>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn>
																<ItemTemplate>
																	<asp:TextBox id="TextboxPayee2NonTaxable" enabled = false runat="server" autopostback=true OnTextChanged="Text_Changed" MaxLength= "10" cssclass="TextBox_Normal" Width="55px" Text='<%# DataBinder.Eval(Container.DataItem, "NonTaxable") %>'>
																	</asp:TextBox>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:datagrid>
												</div>
											</td>
											<td>&nbsp;
											</td>
										</tr>
										<tr>
											<td colSpan="2" align="left">
												<asp:label id="LabelPayee3" runat="server">Payee3</asp:label>
												<asp:textbox id="TextboxPayee3" runat="server" Width="120" cssClass="TextBox_Normal" autopostback="true"></asp:textbox></td>
										</tr>
										<tr>
											<td colSpan="2" align="left">
												<div style="OVERFLOW: auto; WIDTH: 250px; HEIGHT: 80px">
													<asp:datagrid id="DatagridPayee3" runat="server" Width="140px" CssClass="DataGrid_Grid" AutoGenerateColumns="False">
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
														<SelectedItemStyle CssClass="DataGrid_SelectedStyle_Total"></SelectedItemStyle>
														<Columns>
															<asp:TemplateColumn>
																<ItemTemplate>
																	<asp:Label id="LabelPayee3AcctType" runat="server" cssclass="Label_Normal" Text='<%# DataBinder.Eval(Container.DataItem, "AcctType") %>'>
																	</asp:Label>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn>
																<ItemTemplate>
																	<asp:TextBox id="TextBoxPayee3Taxable" enabled = false runat="server" autopostback=true OnTextChanged="Text_Changed" MaxLength= "10" cssclass="TextBox_Normal" Width="55px" Text='<%# DataBinder.Eval(Container.DataItem, "Taxable") %>'>
																	</asp:TextBox>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn>
																<ItemTemplate>
																	<asp:TextBox id="TextBoxPayee3NonTaxable" enabled = false runat="server" autopostback=true OnTextChanged="Text_Changed" MaxLength= "10" cssclass="TextBox_Normal" Width="55px" Text='<%# DataBinder.Eval(Container.DataItem, "NonTaxable") %>'>
																	</asp:TextBox>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:datagrid>
												</div>
											</td>
										</tr>
									</table>
								</td>
								<td>
									<table width="370" height="50">
										<tr>
											<td align="right">
												<asp:label id="LabelAddress1" runat="server" Width="50px">Address1</asp:label></td>
											<td align="left" colSpan="4">
												<asp:textbox id="TextboxAddress1" runat="server" Width="200" cssClass="TextBox_Normal"></asp:textbox></td>
											<td align="left">
												<asp:button id="ButtonAddress" runat="server" Width="110" Text="AddressUpdate" cssClass="Button_Normal"></asp:button></td>
										</tr>
										<tr>
											<td align="right">
												<asp:label id="LabelAddress2" runat="server" Width="50px">Address2</asp:label></td>
											<td align="left" colSpan="4">
												<asp:textbox id="TextboxAddress2" runat="server" Width="200" cssClass="TextBox_Normal"></asp:textbox></td>
											<td>&nbsp;</td>
										</tr>
										<tr>
											<td align="right">
												<asp:label id="LabelAddress3" runat="server" Width="50px">Address3</asp:label></td>
											<td align="left" colSpan="4">
												<asp:textbox id="TextboxAddress3" runat="server" Width="200" cssClass="TextBox_Normal"></asp:textbox></td>
											<td>&nbsp;</td>
										</tr>
										<tr>
											<td align="right">
												<asp:label id="LabelCity" runat="server" Width="50px">City</asp:label></td>
											<td colSpan="2">
												<asp:textbox id="TextboxCity1" runat="server" Width="120" cssClass="TextBox_Normal" MaxLength="30"></asp:textbox></td>
											<td align="right">
												<asp:label id="LabelZip" runat="server" Width="50px">Zip</asp:label></td>
											<td colSpan="2" align="left">
												<asp:textbox id="TextBoxZip" runat="server" Width="120" cssClass="TextBox_Normal" MaxLength="30"></asp:textbox></td>
										</tr>
										<tr>
											<td align="right">
												<asp:label id="LabelState" runat="server" Width="50px">State</asp:label></td>
											<td>
												<asp:textbox id="TextBoxState" runat="server" Width="170px" cssClass="TextBox_Normal" MaxLength="30"></asp:textbox></td>
										</tr>
										<tr>
											<td align="right">
												<asp:label id="LabelCountry" runat="server" Width="50px">Country</asp:label></td>
											<td>
												<asp:textbox id="TextBoxCountry" runat="server" Width="170px" cssClass="TextBox_Normal" MaxLength="30"></asp:textbox></td>
										</tr>
										<tr>
											<td>&nbsp;</td>
										</tr>
										<tr>
											<td class="td_Text" align="left" colSpan="6" height="0"></td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td align="left">
												<asp:label id="LabelTaxRate" runat="server" Width="56px">Tax Rate</asp:label></td>
											<td align="left">
												<asp:label id="LabelTaxable" runat="server">Taxable</asp:label></td>
											<td align="left">
												<asp:label id="LabelNonTaxable" runat="server" Width="81px">Non Taxable</asp:label></td>
											<td align="left">
												<asp:label id="LabelTax" runat="server">Tax</asp:label></td>
											<td align="left">
												<asp:label id="LabelNet" runat="server">Net</asp:label></td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td align="left">
												<asp:textbox id="TextboxTaxRate" runat="server" Width="40" cssClass="TextBox_Normal" autoPostback="true"></asp:textbox></td>
											<td align="left">
												<asp:textbox id="TextboxTaxable" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
											<td align="left">
												<asp:textbox id="TextboxNonTaxable" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
											<td align="left">
												<asp:textbox id="TextboxTax" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
											<td align="left">
												<asp:textbox id="TextboxNet" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
										</tr>
										<tr>
											<td>
												<asp:label id="Label3" runat="server">TD Amount</asp:label></td>
											<td align="left">
												<asp:textbox id="TextboxHardShipTaxRate" runat="server" Width="40" cssClass="TextBox_Normal"
													autoPostback="true"></asp:textbox></td>
											<td align="left">
												<asp:textbox id="TextboxHardShipAmount" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
											<td align="left">
											</td>
											<td align="left">
												<asp:textbox id="TextboxHardShipTax" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
											<td align="left">
												<asp:textbox id="TextboxHardShipNet" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td align="left">
												<asp:textbox id="TextboxMinDistTaxRate" runat="server" Width="40" cssClass="TextBox_Normal" autoPostback="true"></asp:textbox></td>
											<td align="left">
												<asp:textbox id="TextboxMinDistAmount" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
											<td align="left">
											</td>
											<td align="left">
												<asp:textbox id="TextboxMinDistTaxable" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
											<td align="left">
												<asp:textbox id="TextboxMinDistNet" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td colspan="5" align="left">
												<asp:Label id="LabelRequiredMinDisbAmount" runat="server" CssClass="Error_Message">Required Minimum Distribution</asp:Label>
											</td>
										</tr>
										<tr>
											<td class="td_Text" align="left" colSpan="6" height="0"></td>
										</tr>
										<tr>
											<td colSpan="6">&nbsp;</td>
										</tr>
										<tr>
											<td class="td_Text" align="left" colSpan="6" height="0"></td>
										</tr>
										<tr>
											<td colSpan="2">&nbsp;</td>
											<td align="left">
												<asp:label id="LabelTaxable2" runat="server">Taxable</asp:label></td>
											<td align="left">
												<asp:label id="LabelNonTaxable2" runat="server" Width="81px">Non Taxable</asp:label></td>
											<td>&nbsp;</td>
											<td align="left">
												<asp:label id="LabelNet2" runat="server">Net</asp:label></td>
										</tr>
										<tr>
											<td colSpan="2">&nbsp;</td>
											<td align="left">
												<asp:textbox id="TextboxTaxable2" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
											<td align="left">
												<asp:textbox id="TextboxNonTaxable2" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
											<td>&nbsp;</td>
											<td align="left">
												<asp:textbox id="TextboxNet2" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
										</tr>
										<tr>
											<td class="td_Text" align="left" colSpan="6" height="0"></td>
										</tr>
										<tr>
											<td colSpan="6">&nbsp;</td>
										</tr>
										<tr>
											<td class="td_Text" align="left" colSpan="6" height="0"></td>
										</tr>
										<tr>
											<td colSpan="2">&nbsp;</td>
											<td align="left">
												<asp:label id="LabelTaxable3" runat="server">Taxable</asp:label></td>
											<td align="left">
												<asp:label id="LabelNonTaxable3" runat="server" Width="81px">Non Taxable</asp:label></td>
											<td>&nbsp;</td>
											<td align="left">
												<asp:label id="LabelNet3" runat="server">Net</asp:label></td>
										</tr>
										<tr>
											<td colSpan="2">&nbsp;</td>
											<td>
												<asp:textbox id="TextboxTaxable3" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
											<td>
												<asp:textbox id="TextboxNonTaxable3" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
											<td>&nbsp;</td>
											<td>
												<asp:textbox id="TextboxNet3" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
										</tr>
										<tr>
											<td class="td_Text" align="left" colSpan="6" height="0"></td>
										</tr>
										<tr>
											<td colSpan="5"><div style="OVERFLOW: auto; Height: 100Px">
													<asp:datagrid id="DatagridDeductions" runat="server" Width="100px" CssClass="DataGrid_Grid">
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<Columns>
															<asp:TemplateColumn HeaderText="Selected">
																<ItemTemplate>
																	<asp:CheckBox id="CheckBoxDeduction" runat="server" autoPostBack="true" OnCheckedChanged="CheckBoxDeduction_Checked"></asp:CheckBox>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:datagrid>
												</div>
											</td>
											<td valign="top">
												<table>
													<tr>
														<td>
															<asp:label id="LabelDeductions" runat="server">Deductions</asp:label></td>
													</tr>
													<tr>
														<td>
															<asp:textbox id="TextboxDeductions" runat="server" Width="40" cssClass="TextBox_Normal"></asp:textbox></td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td class="td_Text" align="left" colSpan="6" height="1"></td>
										</tr>
										<tr>
											<td colSpan="2">&nbsp;</td>
											<td align="left">
												<asp:Label id="LabelTaxableFinal" runat="server">Taxable</asp:Label></td>
											<td align="left">
												<asp:Label id="LabelNonTaxableFinal" runat="server" Width="81px">Non Taxable</asp:Label></td>
											<td align="left">
												<asp:Label id="LabelTaxFinal" runat="server">Tax</asp:Label></td>
											<td align="left">
												<asp:Label id="LabelNetFinal" runat="server">Net</asp:Label></td>
										</tr>
										<tr>
											<td colspan="2">&nbsp;</td>
											<td align="left">
												<asp:textbox id="TextboxTaxableFinal" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
											<td align="left">
												<asp:textbox id="TextboxNonTaxableFinal" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
											<td align="left">
												<asp:textbox id="TextboxTaxFinal" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
											<td align="left">
												<asp:textbox id="TextboxNetFinal" runat="server" Width="60" cssClass="TextBox_Normal"></asp:textbox></td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
						<table class="Table_WithOutBorder" width="100%">
							<tr>
								<td>&nbsp;
								</td>
							</tr>
							<tr class="Td_ButtonContainer">
								<td align="right">
									<asp:Button id="ButtonSave" runat="server" Text="Save" cssClass="Button_Normal" width="80"></asp:Button>
									&nbsp; &nbsp;
									<asp:Button id="ButtonOK" cssClass="Button_Normal" runat="server" Text="OK" width="80"></asp:Button>
								</td>
							</tr>
						</table>
					</iewc:PageView>
				</iewc:multipage></td>
		</tr>
	</table>
	<asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder></form>
<script language="javascript">
		document.all("TextboxRequestAmount").focus();
		
</script>
<!--#include virtual="bottom.html"-->
