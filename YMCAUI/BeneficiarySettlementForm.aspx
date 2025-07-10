<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BeneficiarySettlementForm.aspx.vb" Inherits="YMCAUI.BeneficiarySettlementForm"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<!--#include virtual="top.html"-->
 <%-- START : SB | 10/13/2016 | YRS-AT-3095 | References for Jquery defining divMessage property --%>
 <script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="JS/YMCA_JScript.js" type="text/javascript"></script>
    <script language="javascript" src="JS/YMCA_JScript_Warn.js" type="text/javascript"></script>
      <script language="javascript">
          $(document).ready(function (){
                $("#DivMessage").css('display', 'block');
                $("#DivMessage")[0].className = "error-msg";
              
          });
       </script>
 <%-- END : SB | 10/13/2016 | YRS-AT-3095 | References for Jquery defining divMessage property --%>
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" width="700">
		<tr>
			<td class="Td_BackGroundColorMenu" align="left">
				<cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
					CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover"
					DefaultMouseUpCssClass="mouseup" MenuFadeDelay="2" mouseovercssclass="MouseOver">
					<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
				</cc1:menu>
			</td>
		</tr>
		<%--<tr>
			<td class="Td_HeadingFormContainer" align="left">&nbsp;Settle Beneficiaries&nbsp;
				<asp:label id="LabelTitle" runat="server"></asp:label></td>
		</tr>--%>

        <tr>
			<td class="Td_HeadingFormContainer" align="left">
				<YRSControls:YMCA_Header_WebUserControl ID="HeaderControl"  runat="server"></YRSControls:YMCA_Header_WebUserControl>
                </td>
		</tr>
         <%-- START : SB | 10/13/2016 | YRS-AT-3095 | Displaying error message in div --%>
       <tr><td> <div id="DivMessage"  runat="server" style="text-align: left;" enableviewstate="false"> 
        </div></td></tr>
         <%-- END : SB | 10/13/2016 | YRS-AT-3095 | Displaying error message in div --%>
	</table>
	<div class="Div_Center">
		<table width="700">
			<%-- START : SB | 10/12/2017 | YRS-AT-3324 | removing table row --%>
            <%--<tr>
				&nbsp;
			</tr>--%>
			<%-- END : SB | 10/12/2017 | YRS-AT-3324 | removing table row --%>
			<tr>
				<td><iewc:tabstrip id="TabStripBeneficiarySettlement" runat="server" height="30px" Width="700" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
						TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
						AutoPostBack="True" BorderStyle="None">
						<iewc:Tab Text="List" ID="tabList"></iewc:Tab>
						<iewc:Tab Text="ActiveBeneficiaries" ID="tabActive"></iewc:Tab>
						<iewc:Tab Text="RetiredBeneficiaries" ID="tabRetired"></iewc:Tab>
					</iewc:tabstrip></td>
			</tr>
			<tr>
				<td><iewc:multipage id="MultiPageBeneficiarySettlement" runat="server">
						<iewc:PageView>
							<div class="Div_Center">
								<table width="690">
									<tr>
										<td>
											<table width="685" class="Table_WithBorder">
												<tr>
													<td width="440px">
														<DIV style="OVERFLOW: auto; WIDTH: 435px; HEIGHT: 200px; TEXT-ALIGN: left">
															<asp:DataGrid ID="DataGridSearchResults" Runat="server" Width="431" CssClass="DataGrid_Grid" allowsorting="true">
																<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																<selectedItemStyle cssClass="dataGrid_selectedStyle"></selectedItemStyle>
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
														<table width="200">
															<tr>
																<td>
																	<span CssClass="Label_Small">Fund No</span>
																</td>
																<td>
																	<asp:TextBox ID="TextBoxFundNo" Runat="server" CssClass="TextBox_Normal"></asp:TextBox></td>
															</tr>
															<tr>
																<td>
																	<span CssClass="Label_Small">SS No</span>
																</td>
																<td>
																	<asp:TextBox ID="TextBoxSSNo" Runat="server" CssClass="TextBox_Normal"></asp:TextBox></td>
															</tr>
															<tr>
																<td>
																	<span CssClass="Label_Small">Last Name</span></td>
																<td>
																	<asp:TextBox ID="TextBoxLastName" Runat="server" CssClass="TextBox_Normal"></asp:TextBox></td>
															</tr>
															<tr>
																<td>
																	<span CssClass="Label_Small">First Name</span></td>
																<td>
																	<asp:TextBox ID="TextBoxFirstName" Runat="server" CssClass="TextBox_Normal"></asp:TextBox></td>
															</tr>
															<tr>
																<td>
																	&nbsp;&nbsp;&nbsp;
																	<asp:Button Runat="server" ID="ButtonFind" text="Find" CssClass="Button_Normal" Height="20px"
																		Width="65px"></asp:Button></td>
																<td>
																	&nbsp;&nbsp;&nbsp;
																	<asp:Button Runat="server" ID="ButtonClear" text="Clear" CssClass="Button_Normal" Height="20px"
																		Width="65px"></asp:Button></td>
															</tr>
														</table>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
							</div>
						</iewc:PageView>
						<iewc:PageView>
							<table width="690" class="Table_WithBorder">
								<tr>
									<td>
										<table width="685">
											<tr>
												<td align="left">
													<asp:Label ID="LabelActiveBeneficiaries" Runat="server" CssClass="Label_Medium">Active Beneficiaries</asp:Label>
												</td>
											</tr>
											<tr>
												<td align="left">
													<asp:DataGrid ID="DataGridActiveBeneficiaries" Runat="server" Width="500" CssClass="DataGrid_Grid"
														AutoGenerateColumns="false">
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<selectedItemStyle cssClass="dataGrid_selectedStyle"></selectedItemStyle>
														<Columns>
															<asp:TemplateColumn>
																<ItemTemplate>
																	<asp:ImageButton id="Imagebutton1" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																		CommandName="Select" ToolTip="Select"></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn HeaderText="Name" DataField="Name"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Name2" DataField="Name2"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="TaxID" DataField="TaxID"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Rel" DataField="Rel"></asp:BoundColumn>
															<%--Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and changed header text for birth date--%>
                                                            <%--<asp:BoundColumn HeaderText="Birthdate" DataField="Birthdate"></asp:BoundColumn>--%>
															<asp:BoundColumn HeaderText="Birth/Estd. Date" DataField="Birthdate"></asp:BoundColumn>
                                                            <%--End - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and changed header text for birth date--%>
                                                            <asp:BoundColumn HeaderText="Pct" DataField="Pct"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Status" DataField="Status"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Type" DataField="Type"></asp:BoundColumn>
														</Columns>
													</asp:DataGrid>
												</td>
												<td>
													<table>
														<tr>
															<td align="left">
																<asp:Button ID="ButtonSettleActiveBeneficiary" text="Settle Beneficiary" Runat="server" CssClass="Button_Normal"></asp:Button></td>
														</tr>
													</table>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<table width="685">
											<tr>
												<td align="left">
													<asp:Label ID="LabelActiveBenefitOptions_RetirementPlan" Runat="server" CssClass="Label_Medium">Retirement Plan Benefit Options</asp:Label>
												</td>
											</tr>
											<tr>
												<td align="left">
													<asp:DataGrid ID="DataGridActiveBenefitOptions_RetirementPlan" Runat="server" Width="670" CssClass="DataGrid_Grid"
														AutoGenerateColumns="true">
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<selectedItemStyle cssClass="dataGrid_selectedStyle"></selectedItemStyle>
														<Columns>
															<asp:TemplateColumn HeaderText="Select">
																<ItemTemplate>
																	<asp:ImageButton id="RadioLabel_Active_RP" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																		CommandName="Select" ToolTip="Select"></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:DataGrid>
												</td>
												<td>
												</td>
											</tr>
											<TR>
												<TD align="left">
													<asp:Label id="LabelActiveBenefitOptions_SavingsPlan" CssClass="Label_Medium" Runat="server">Savings Plan Benefit Options</asp:Label></TD>
												<TD></TD>
											</TR>
											<TR>
												<TD align="left">
													<asp:DataGrid id="DataGridActiveBenefitOptions_SavingsPlan" CssClass="DataGrid_Grid" Width="670"
														Runat="server" AutoGenerateColumns="true">
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
														<selectedItemStyle cssClass="dataGrid_selectedStyle"></selectedItemStyle>
														<Columns>
															<asp:TemplateColumn HeaderText="Select">
																<ItemTemplate>
																	<!-- <asp:RadioButton id="RadioLabel_Active_SP1" runat="server" AutoPostBack="True" CommandName="Select" /> -->
																	<asp:ImageButton id="RadioLabel_Active_SP" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																		CommandName="Select" ToolTip="Select"></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:DataGrid></TD>
												<TD>
												</TD>
											</TR>
										</table>
									</td>
								</tr>
							</table>
						</iewc:PageView>
						<iewc:PageView>
							<table class="Table_WithBorder">
								<tr>
									<td align="left">
										<asp:Label ID="LabelRetiredBeneficiaries" Runat="server" CssClass="Label_Medium">Retired Beneficiaries</asp:Label>
									</td>
								</tr>
								<tr>
									<td>
										<table width="685">
											<tr>
												<td align="left">
													<asp:DataGrid ID="DataGridRetiredBeneficiaries" Runat="server" Width="500" CssClass="DataGrid_Grid"
														autogeneratecolumns="False">
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<selectedItemStyle cssClass="dataGrid_selectedStyle"></selectedItemStyle>
														<Columns>
															<asp:TemplateColumn>
																<ItemTemplate>
																	<asp:ImageButton id="Imagebutton2" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																		CommandName="Select" ToolTip="Select"></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn HeaderText="Name" DataField="Name"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Name2" DataField="Name2"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="TaxID" DataField="TaxID"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Rel" DataField="Rel"></asp:BoundColumn>
															<%--Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and changed header text for birth date--%>
                                                            <%--<asp:BoundColumn HeaderText="Birthdate" DataField="Birthdate"></asp:BoundColumn>--%>
															<asp:BoundColumn HeaderText="Birth/Estd. Date" DataField="Birthdate"></asp:BoundColumn>
                                                            <%--End - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and changed header text for birth date--%>
                                                            <asp:BoundColumn HeaderText="Pct" DataField="Pct"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Status" DataField="Status"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Type" DataField="Type"></asp:BoundColumn>
														</Columns>
													</asp:DataGrid>
												</td>
												<td>
													<table>
														<tr>
															<td>
																<asp:Button ID="ButtonSettleRetiredBeneficiary" text="Settle Beneficiary" Runat="server" CssClass="Button_Normal"></asp:Button></td>
														</tr>
													</table>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<table width="685">
											<tr>
												<td align="left">
													<asp:Label ID="LabelRetiredBenefitOptions_RetirementPlan" Runat="server" CssClass="Label_Medium">Retirement Plan Benefit Options</asp:Label>
												</td>
											</tr>
											<tr>
												<td align="left">
													<asp:DataGrid ID="DataGridRetiredBenefitOptions_RetirementPlan" Runat="server" Width="660" CssClass="DataGrid_Grid"
														autogeneratecolumns="true">
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<selectedItemStyle cssClass="dataGrid_selectedStyle"></selectedItemStyle>
														<Columns>
															<asp:TemplateColumn HeaderText="Select">
																<ItemTemplate>
																	<asp:ImageButton id="RadioLabel_Retired_RP" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																		CommandName="Select" ToolTip="Select"></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:DataGrid>
												</td>
											</tr>
											<TR>
												<TD align="left">
													<asp:Label id="LabelRetiredBenefitOptions_SavingsPlan" CssClass="Label_Medium" Runat="server">Savings Plan Benefit Options</asp:Label></TD>
											</TR>
											<TR>
												<TD align="left">
													<asp:DataGrid id="DataGridRetiredBenefitOptions_SavingsPlan" CssClass="DataGrid_Grid" Width="660"
														Runat="server" autogeneratecolumns="true">
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
														<selectedItemStyle cssClass="dataGrid_selectedStyle"></selectedItemStyle>
														<Columns>
															<asp:TemplateColumn HeaderText="Select">
																<ItemTemplate>
																	<asp:ImageButton id="RadioLabel_Retired_SP" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																		CommandName="Select" ToolTip="Select"></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:DataGrid>
												</TD>
											</TR>
										</table>
									</td>
								</tr>
							</table>
						</iewc:PageView>
					</iewc:multipage></td>
			</tr>
			<TR>
				<td>
					<table width="697">
						<tr>
							<td class="Td_ButtonContainer" align="right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:button id="ButtonPrint" runat="server" CssClass="Button_Normal" Width="65px" Text="Print"
									Height="20px"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="65px" Text="OK" Height="20px"></asp:button></td>
						</tr>
					</table>
				</td>
			</TR>
		</table>
	</div>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder></TD>
</form>
<!--#include virtual="bottom.html"-->
