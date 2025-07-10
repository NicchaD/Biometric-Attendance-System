<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SplitFundsforSettlement.aspx.vb"
    Inherits="YMCAUI.SplitFundsforSettlement" %>

<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<!--#include virtual="top.html"-->
<script language="javascript" src="JS/YMCA_JScript.js" type="text/javascript"></script>
<script language="javascript" src="JS/YMCA_JScript_Warn.js" type="text/javascript"></script>
<script type="text/javascript" language="JavaScript">
    //Sanjay R:2013.08.05 - YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
    $(document).ready(function () {
        $("#divWSMessage").dialog
					({
					    modal: true,
					    open: function (event, ui) { $(this).parent('div').find('button:contains("SAVE")').focus(); },
					    autoOpen: false,
					    title: "Process Restricted",
					    width: 570, height: 200,
					    position: ['center', 200],
					    buttons: [{ text: "OK", click: CloseWSMessage}]
					});
    });
    function CloseWSMessage() {
        $(document).ready(function () {
            $("#divWSMessage").dialog('close');
        });
    }
    function openDialog(str, type) {
        $(document).ready(function () {
            if (type == 'Bene') {
                str = 'Beneficiary add,edit and delete operation can not be performed due to following reason(s).<br/>' + str
            }
            else {
                str = 'Beneficiary Account Creation process can not be performed due to following reasons(s).\n' + str;
            }
            $("#divWSMessage").html(str);
            $("#divWSMessage").dialog('open');
            return false;
        });
    }
    //End,Sanjay R:2013.08.05 - YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
</script>
<form id="Form2" method="post" runat="server">
<table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="700">
    <tr>
        <td class="Td_BackGroundColorMenu" align="left">
            <cc1:Menu ID="Menu1" runat="server" mouseovercssclass="MouseOver" MenuFadeDelay="2"
                DefaultMouseUpCssClass="mouseup" DefaultMouseOverCssClass="mouseover" DefaultMouseDownCssClass="mousedown"
                DefaultCssClass="menuitem" CssClass="menustyle" Cursor="Pointer" HighlightTopMenu="False"
                Layout="Horizontal">
                <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
            </cc1:Menu>
        </td>
    </tr>
    <%--<tr>
			<td class="Td_HeadingFormContainer" align="left">Split Fund For Settlement
				<asp:label id="LabelTitle" runat="server" Width="432px"></asp:label>
			</td>
		</tr>--%>
    <tr>
        <td class="Td_HeadingFormContainer" align="left">
            <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
</table>
<div class="Div_Center">
    <table cellspacing="0" cellpadding="0" width="700" height="100%">
        <tr>
            <td>
                <iewc:TabStrip ID="TabStripBeneficiarySettlement" runat="server" Width="100%" BorderStyle="None"
                    AutoPostBack="True" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:75;text-align:center;border:solid 1px White;border-bottom:none"
                    TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
                    Height="30px">
                    <iewc:Tab Text="List" ID="tabList"></iewc:Tab>
                    <iewc:Tab Text="Details" ID="tabSplit"></iewc:Tab>
                </iewc:TabStrip>
            </td>
        </tr>
        <tr height="100%" valign="top">
            <td>
                <iewc:MultiPage ID="MultiPageSplitFundsSettlement" runat="server">
                    <iewc:PageView>
                        <div class="Div_Center">
                            <table width="690">
                                <tr>
                                    <td>
                                        <table width="685" class="Table_WithBorder">
                                            <tr>
                                                <td>
                                                    <asp:label id="label_RecorNotFound" runat="server" cssclass="Error_Message" visible="False"></asp:label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="440px">
                                                    <div style="overflow: auto; width: 435px; height: 200px; text-align: left">
                                                        <asp:datagrid id="DataGridSearchResults" runat="server" width="431" cssclass="DataGrid_Grid"
                                                            allowsorting="true">
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
															</asp:datagrid>
                                                    </div>
                                                </td>
                                                <td>
                                                    <table width="200">
                                                        <tr>
                                                            <td>
                                                                <span class="Label_Small">Fund No</span>
                                                            </td>
                                                            <td>
                                                                <asp:textbox id="TextBoxFundNo" runat="server" cssclass="TextBox_Normal"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span class="Label_Small">SS No</span>
                                                            </td>
                                                            <td>
                                                                <asp:textbox id="TextBoxSSNo" runat="server" cssclass="TextBox_Normal"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span class="Label_Small">Last Name</span>
                                                            </td>
                                                            <td>
                                                                <asp:textbox id="TextBoxLastName" runat="server" cssclass="TextBox_Normal"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span class="Label_Small">First Name</span>
                                                            </td>
                                                            <td>
                                                                <asp:textbox id="TextBoxFirstName" runat="server" cssclass="TextBox_Normal"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;&nbsp;&nbsp;
                                                                <asp:button runat="server" id="ButtonFind" text="Find" cssclass="Button_Normal" height="20px"
                                                                    width="65px"></asp:button>
                                                            </td>
                                                            <td>
                                                                &nbsp;&nbsp;&nbsp;
                                                                <asp:button runat="server" id="ButtonClear" text="Clear" cssclass="Button_Normal"
                                                                    height="20px" width="65px"></asp:button>
                                                            </td>
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
                        <table width="100%" class="Table_WithBorder" height="100%" border="0">
                            <tr>
                                <td align="left">
                                    <asp:panel id="Panel_Split_Error_Messages" runat="server">
									<DIV style="OVERFLOW: auto; WIDTH: 98%; HEIGHT: 170px; TEXT-ALIGN: left; PADDING-RIGHT:20PX; PADDING-LEFT: 20px; PADDING-TOP: 25px;">
                                    <%-- Added By Anudeep:29.11.2012 BT-1461:Error Message Is not alighned Properly   --%>
										<asp:Label ID="label_Split_Error_Messages" Runat="server" font-align="left" CssClass="Error_Message"></asp:Label>
									</DIV>
								    </asp:panel>
                                     <asp:panel id="panel_BeneficiaryDetails" runat="server">
						                 <table>
								<tr>
									<td>
										<table width="100%">
											<tr>
												<td align="left">
													<asp:Label ID="LabelActiveBeneficiaries"  Runat="server" CssClass="Label_Medium">List of Beneficiaries</asp:Label>
												</td>
											</tr>
											<tr>
												<td align="left">
													<asp:DataGrid ID="DataGridActiveBeneficiaries" Runat="server" Width="500" CssClass="DataGrid_Grid"
														AutoGenerateColumns="false" Visible="True">
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<selectedItemStyle cssClass="dataGrid_selectedStyle"></selectedItemStyle>
														<Columns>
															<asp:TemplateColumn>
																<ItemTemplate>
																	<asp:ImageButton id="Imagebutton1" runat="server" ImageUrl="images\select.gif" CausesValidation="False" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.UniqueID") %>' ToolTip="Select">
																	</asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn HeaderText="First Name" DataField="Name"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Last Name" DataField="Name2"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Tax ID" DataField="TaxID"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Rel" DataField="Rel"></asp:BoundColumn>
															<%--Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and changed header text for birth date--%>
															<%--<asp:BoundColumn HeaderText="Birth Date" DataField="Birthdate"></asp:BoundColumn>--%>
                                                            <asp:BoundColumn HeaderText="Birth/Estd. Date" DataField="Birthdate"></asp:BoundColumn>
															<%--End - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and changed header text for birth date--%>
															<asp:BoundColumn HeaderText="Benficiary Type" DataField="Type"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Pct" DataField="Pct"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Status" DataField="Status"></asp:BoundColumn>
															<asp:BoundColumn HeaderText="Plan" DataField="Type" visible="False"></asp:BoundColumn>
														</Columns>
													</asp:DataGrid>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								</table>
								        <table border="0" style="border-color:red;">
								<tr>
									<td width="50%">
											<asp:Label ID="Label_Decedent" Runat="server" CssClass="Label_Medium"></asp:Label>
									</td>
									<td>&nbsp;</td>
									<td width=50%>
											<asp:Label ID="Label_Beneficiary" Runat="server" CssClass="Label_Medium"></asp:Label>
									</td>
								</tr>
								<tr>
									<td align="left" style="FONT-SIZE:10px">Account contribution</td>
									<td>&nbsp;</td>
									<td align="left" style="FONT-SIZE:10px">Account contribution</td>
								</tr>
								<tr>
									<td colspan=3 bgColor="#ffcc33">
										<asp:Label BackColor="#ffcc33" ID="LabelActiveBenefitOptions_RetirementPlan" Runat="server"
														CssClass="Label_Medium">Retirement Plan</asp:Label>
									</td>
								</tr>
								<tr>
									<td>
									<asp:DataGrid ID="DataGridActiveBenefitOptions_RetirementPlan" Runat="server" Width="100%" CssClass="DataGrid_Grid"
														AutoGenerateColumns="false" ShowFooter="True">
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<selectedItemStyle cssClass="dataGrid_selectedStyle"></selectedItemStyle>
														<FooterStyle cssclass="DataGrid_HeaderStyle"></FooterStyle>
														<Columns>
															<asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" HeaderText="Acct" FooterText="Total">
																<ItemTemplate>
																	<asp:Label ID="label_Deceased_Ret_Acct" Runat="server">
																		<%# DataBinder.Eval(Container, "DataItem.chrAcctType") %>
																	</asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																	<span class="DataGrid_HeaderStyle">Total</span>
																</FooterTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn HeaderText="Transact" DataField="chrTransactType"></asp:BoundColumn>
															<asp:TemplateColumn HeaderText="Taxable" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																<ItemTemplate>
																	<asp:Label ID="label_Deceased_Ret_taxable" Runat="server" text='<%#DataBinder.Eval(Container.DataItem, "mnyPersonalPreTax","{0:0.00}")%>'>
																	</asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																	<asp:Label Runat="server" align="right" style="font-weight:bold ;FONT-SIZE:11px;" ID="label_Taxable_Deceased_Ret_Total"></asp:Label>
																</FooterTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="NonTaxable" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																<ItemTemplate>
																	<asp:Label ID="label_Deceased_Ret_NonTaxable" align="right" Runat="server" text = '<%#DataBinder.Eval(Container, "DataItem.mnyPersonalPostTax","{0:0.00}") %>'>
																	</asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																	<asp:Label Runat="server" align="right" style="font-weight:bold ;FONT-SIZE:11px;" ID="label_NonTaxable_Deceased_Ret_Total"></asp:Label>
																</FooterTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="YMCA" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																<ItemTemplate>
																	<asp:Label ID="label_Deaceased_Ret_YMCA" align="right" Runat="server" text='<%# DataBinder.Eval(Container, "DataItem.mnyYmcaPreTax","{0:0.00}") %>'>
																	</asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																	<asp:Label Runat="server" align="right" style="font-weight:bold ;FONT-SIZE:11px;" ID="label_YMCA_Deceased_Ret_Total"></asp:Label>
																</FooterTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="Total" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																<ItemTemplate>
																	<asp:Label Runat="server" align="right" ID="label_Deceased_Ret_Total"></asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																	<asp:Label Runat="server" align="right" style="font-weight:bold ;FONT-SIZE:11px;" ID="label_Total_Deceased_Ret_Total"></asp:Label>
																</FooterTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:DataGrid>
													</td>
									<td>&nbsp;</td>
									<td>														
										<asp:DataGrid ID="DatagridBeneficiary_RetirementPlan" ShowFooter="True" Runat="server" Width="100%"
															CssClass="DataGrid_Grid" AutoGenerateColumns="false">
															<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
															<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
															<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
															<selectedItemStyle cssClass="dataGrid_selectedStyle"></selectedItemStyle>
															<FooterStyle cssclass="DataGrid_HeaderStyle"></FooterStyle>
															<Columns>
																<asp:TemplateColumn HeaderText="Acct" HeaderStyle-HorizontalAlign="Center" FooterText="Total">
																	<ItemTemplate>
																		<asp:Label ID="label_Acct" Runat="server">
																			<%# DataBinder.Eval(Container, "DataItem.chrAcctType") %>
																		</asp:Label>
																	</ItemTemplate>
																	<FooterTemplate>
																		<span class="DataGrid_HeaderStyle">Total</span>
																	</FooterTemplate>
																</asp:TemplateColumn>
																<asp:BoundColumn HeaderText="Transact" HeaderStyle-HorizontalAlign="Center" DataField="chrTransactType"></asp:BoundColumn>
																<asp:TemplateColumn HeaderText="Taxable" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																	<ItemTemplate>
																		<asp:Label ID="label_taxable" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "mnyPersonalPreTax","{0:0.00}")%>'>
																		</asp:Label>
																	</ItemTemplate>
																	<FooterTemplate>
																		<asp:Label Runat="server" style="font-weight:bold ;FONT-SIZE:11px;" ID="label_Taxable_Beneficiary_Ret_Total"></asp:Label>
																	</FooterTemplate>
																</asp:TemplateColumn>
																<asp:TemplateColumn HeaderText="NonTaxable" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																	<ItemTemplate>
																		<asp:Label ID="label_NonTaxable" Runat="server" text = '<%# DataBinder.Eval(Container, "DataItem.mnyPersonalPostTax","{0:0.00}") %>'>
																		</asp:Label>
																	</ItemTemplate>
																	<FooterTemplate>
																		<asp:Label Runat="server" style="font-weight:bold ;FONT-SIZE:11px;" ID="label_NonTaxable_Beneficiary_Ret_Total"></asp:Label>
																	</FooterTemplate>
																</asp:TemplateColumn>
																<asp:TemplateColumn HeaderText="YMCA" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																	<ItemTemplate>
																		<asp:Label ID="label_YMCA" Runat="server" text='<%# DataBinder.Eval(Container, "DataItem.mnyYmcaPreTax","{0:0.00}") %>'>
																		</asp:Label>
																	</ItemTemplate>
																	<FooterTemplate>
																		<asp:Label Runat="server" style="font-weight:bold ;FONT-SIZE:11px;" ID="label_YMCA_Beneficiary_Ret_Total"></asp:Label>
																	</FooterTemplate>
																</asp:TemplateColumn>
																<asp:TemplateColumn HeaderText="Total" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																	<ItemTemplate>
																		<asp:Label Runat="server" ID="label_Beneficiary_Ret_Total"></asp:Label>
																	</ItemTemplate>
																	<FooterTemplate>
																		<asp:Label Runat="server" style="font-weight:bold ;FONT-SIZE:11px;" ID="label_Total_Beneficiary_Ret_Total"></asp:Label>
																	</FooterTemplate>
																</asp:TemplateColumn>
															</Columns>
														</asp:DataGrid>
														</td>
								</tr>
								<tr>
									<td colspan=3 bgcolor="#ffcc33" >
										<asp:Label BackColor="#ffcc33" id="LabelActiveBenefitOptions_SavingsPlan" CssClass="Label_Medium"
												Runat="server">Savings Plan</asp:Label>
									</td>
								</tr>
								<tr>
									<td><asp:DataGrid id="DataGridActiveBenefitOptions_SavingsPlan" CssClass="DataGrid_Grid" Width="100%"
														Runat="server" AutoGenerateColumns="false" ShowFooter="True">
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
														<selectedItemStyle cssClass="dataGrid_selectedStyle"></selectedItemStyle>
														<FooterStyle cssclass="DataGrid_HeaderStyle"></FooterStyle>
														<Columns>
															<asp:TemplateColumn HeaderText="Acct" HeaderStyle-HorizontalAlign="Center" FooterText="Total">
																<ItemTemplate>
																	<asp:Label ID="label_Deceased_Acct" Runat="server">
																		<%# DataBinder.Eval(Container, "DataItem.chrAcctType") %>
																	</asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																	<span class="DataGrid_HeaderStyle">Total</span>
																</FooterTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn HeaderText="Transact" HeaderStyle-HorizontalAlign="Center" DataField="chrTransactType"></asp:BoundColumn>
															<asp:TemplateColumn HeaderText="Taxable" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																<ItemTemplate>
																	<asp:Label ID="label_Saving_taxable" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "mnyPersonalPreTax","{0:0.00}")%>'>
																	</asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																	<asp:Label Runat="server" style="font-weight:bold ;FONT-SIZE:11px;" ID="label_Taxable_Deceased_Saving_Total"></asp:Label>
																</FooterTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="NonTaxable" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																<ItemTemplate>
																	<asp:Label ID="label_Deceased_NonTaxable" Runat="server" text = '<%# DataBinder.Eval(Container, "DataItem.mnyPersonalPostTax","{0:0.00}") %>'>
																	</asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																	<asp:Label Runat="server" style="font-weight:bold ;FONT-SIZE:11px;" ID="label_NonTaxable_Deceased_Saving_Total"></asp:Label>
																</FooterTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="YMCA" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																<ItemTemplate>
																	<asp:Label style="FONT-SIZE:11px;" ID="label_Deceased_YMCA" Runat="server" text='<%# DataBinder.Eval(Container, "DataItem.mnyYmcaPreTax","{0:0.00}") %>'>
																	</asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																	<asp:Label Runat="server" style="font-weight:bold ;FONT-SIZE:11px;" ID="label_YMCA_Deceased_Saving_Total"></asp:Label>
																</FooterTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="Total" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																<ItemTemplate>
																	<asp:Label style="FONT-SIZE:11px;" Runat="server" ID="label_Deceased_Saving_Total"></asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																	<asp:Label style="font-weight:bold ;FONT-SIZE:11px;" Runat="server" ID="label_Total_Deceased_Saving_Total"></asp:Label>
																</FooterTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:DataGrid>
													</td>
									<td>&nbsp;</td>
									<td><asp:DataGrid ID="DatagridBeneficiary_SavingPlan" Runat="server" Width="100%" CssClass="DataGrid_Grid"
														AutoGenerateColumns="false" ShowFooter="True">
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<selectedItemStyle cssClass="dataGrid_selectedStyle"></selectedItemStyle>
														<FooterStyle cssclass="DataGrid_HeaderStyle"></FooterStyle>
														<Columns>
															<asp:TemplateColumn HeaderText="Acct" HeaderStyle-HorizontalAlign="Center" FooterText="Total">
																<ItemTemplate>
																	<asp:Label ID="label_Beneficiary_Acct" Runat="server">
																		<%# DataBinder.Eval(Container, "DataItem.chrAcctType") %>
																	</asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																		<span class="DataGrid_HeaderStyle">Total</span>
																</FooterTemplate>
															</asp:TemplateColumn>
															<asp:BoundColumn HeaderText="Transact" HeaderStyle-HorizontalAlign="Center" DataField="chrTransactType"></asp:BoundColumn>
															<asp:TemplateColumn HeaderText="Taxable" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																<ItemTemplate>
																	<asp:Label ID="label_Saving_Beneficiary_taxable" Runat="server" text='<%# DataBinder.Eval(Container.DataItem, "mnyPersonalPreTax","{0:0.00}")%>'>
																	</asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																	<asp:Label style="font-weight:bold ;FONT-SIZE:11px;" Runat="server" ID="label_Taxable_Beneficiary_Saving_Total"></asp:Label>
																</FooterTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="NonTaxable" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																<ItemTemplate>
																	<asp:Label ID="label_Saving_Beneficiary_NonTaxable" Runat="server" text = '<%# DataBinder.Eval(Container, "DataItem.mnyPersonalPostTax","{0:0.00}") %>'>
																	</asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																	<asp:Label style="font-weight:bold ;FONT-SIZE:11px;" Runat="server" ID="label_NonTaxable_Beneficiary_Saving_Total"></asp:Label>
																</FooterTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="YMCA" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																<ItemTemplate>
																	<asp:Label style="FONT-SIZE:11px;" ID="label_Saving_Beneficiary_YMCA" Runat="server" text='<%# DataBinder.Eval(Container, "DataItem.mnyYmcaPreTax","{0:0.00}") %>'>
																	</asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																	<asp:Label Runat="server" style="font-weight:bold ;FONT-SIZE:11px;" ID="label_YMCA_Beneficiary_Saving_Total"></asp:Label>
																</FooterTemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="Total" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
																<ItemTemplate>
																	<asp:Label style="FONT-SIZE:11px;" Runat="server" ID="label_Saving_Beneficiary_Total"></asp:Label>
																</ItemTemplate>
																<FooterTemplate>
																	<asp:Label Runat="server" style="font-weight:bold ;FONT-SIZE:11px;" ID="label_Total_Beneficiary_Saving_Total"></asp:Label>
																</FooterTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:DataGrid>
													</td>
								</tr>
								<tr>
									<td colspan="3" bgcolor="#ffcc33">
									    <asp:Label BackColor="#ffcc33" ID="Label1" Runat="server" CssClass="Label_Medium">Grand Total </asp:Label>
									</td>
								</tr>
								<tr>
									<td>			
										<table cellspacing="2" width="100%" border="1" style="border-collapse:collapse;">
											<tr class="DataGrid_NormalStyle">
												<td width="35" style="font-weight:bold ;FONT-SIZE:11px;">Total</td>
												<td width="60" style="font-weight:bold ;FONT-SIZE:11px;COLOR:white">&nbsp;</td>
												<td width="40" style="font-weight : bold ;FONT-SIZE: 11px;" align="right">
													<asp:Label ID="label_GrandTotal_Deceased_Taxable" align="right" Runat="server" text=""></asp:Label></td>
												<td width="70" style="font-weight : bold ;FONT-SIZE: 11px;" align="right">
													<asp:Label ID="label_GrandTotal_Deceased_NonTaxable" align="right" Runat="server" text=""></asp:Label></td>
												<td width="35" style="font-weight : bold ;FONT-SIZE: 11px;" align="right">
													<asp:Label ID="label_GrandTotal_Deceased_YMCA" align="right" Runat="server" text=""></asp:Label></td>
												<td width="35" style="font-weight : bold ;FONT-SIZE: 11px;" align="right">
													<asp:Label ID="label_GrandTotal_Deceased_Total" align="right" Runat="server" text=""></asp:Label></td>
											</tr>
										</table>
									</td>
									<td>&nbsp;</td>
									<td>
									<table cellspacing="2" width="100%" border="1" style="border-collapse:collapse;">
										<tr class="DataGrid_NormalStyle">
											<td width="35" style="font-weight:bold ;FONT-SIZE:11px;">Total</td>
											<td width="60" style="font-weight:bold ;FONT-SIZE:11px;COLOR:white" align="right">&nbsp;</td>
											<td width="40" style="font-weight : bold ;FONT-SIZE: 11px;" align="right">
												<asp:Label ID="label_GrandTotal_Beneficiary_Taxable" Runat="server" text="">&nbsp;</asp:Label></td>
											<td width="70" style="font-weight : bold ;FONT-SIZE: 11px;" align="right">
												<asp:Label ID="label_GrandTotal_Beneficiary_NonTaxable" Runat="server" text="">&nbsp;</asp:Label></td>
											<td width="35" style="font-weight : bold ;FONT-SIZE: 11px;" align="right">
												<asp:Label ID="label_GrandTotal_Beneficiary_YMCA" Runat="server" text="">&nbsp;</asp:Label></td>
											<td width="35" style="font-weight : bold ;FONT-SIZE: 11px;" align="right">
												<asp:Label ID="label_GrandTotal_Beneficiary_Total" Runat="server" text="">&nbsp;</asp:Label></td>
										</tr>
									</table>
									</td>
								</tr>
								<tr><td colspan="3">&nbsp;</td></tr>
								<tr>
									<td colspan="3" align="center"><asp:Button ID="ButtonSplitfundsforBeneficiaries" text="Split Funds for All Beneficiaries" Runat="server"
														CssClass="Button_Normal"></asp:Button></td>
								</tr>
								</table>
								    </asp:panel>
                                </td>
                            </tr>
                        </table>
                    </iewc:PageView>
                </iewc:MultiPage>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td class="Td_ButtonContainer" align="right" width="512">
                            &nbsp;
                        </td>
                        <td class="Td_ButtonContainer">
                            <asp:button id="ButtonOK" runat="server" cssclass="Button_Normal" width="65px" height="20px"
                                text="OK"></asp:button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<div id="divWSMessage"  runat="server" style="display: none;">
    <table width="690px">
        <tr>
            <td valign="top" align="left">
                <span id="spntext" ></span>
            </td>
        </tr>                    
    </table>
</div>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</TD>
</form>
<!--#include virtual="bottom.html"-->
