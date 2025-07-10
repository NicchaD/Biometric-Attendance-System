<%@ Page Language="vb" AutoEventWireup="false" Codebehind="VoidDisbursementReplaceManager.aspx.vb" Inherits="YMCAUI.VoidDisbursementReplaceManager"%>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="uc2" TagName="VoidDisbursement" Src="UserControls/VoidDisbursement.ascx" %>
<%@ Register TagPrefix="uc1" TagName="VoidUserControl" Src="UserControls/VoidUserControl.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<!--#include virtual="top.html"-->
	<script language="javascript">
function _OnBlur_DropdownReason()
{

	if (document.frmVRManager.all('TextBoxNotes').value == null)
	{
	document.frmVRManager.all('TextBoxNotes').value = document.frmVRManager.all('DropDownListReason').value;
	}
	else
	{
	document.frmVRManager.all('TextBoxNotes').value = document.frmVRManager.all('DropDownListReason').value;
	}
	
}
	</script>
	<body>
		<TABLE height="46" cellSpacing="0" cellPadding="0" border="0">
			<form id="frmVRManager" method="post" runat="server">
				<TBODY>
					<TR vAlign="top">
						<TD></TD>
						<TD>
							<table class="Table_WithoutBorder" cellSpacing="0" width="700">
								<tr>
									<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" mouseovercssclass="MouseOver" MenuFadeDelay="2" DefaultMouseUpCssClass="mouseup"
											DefaultMouseOverCssClass="mouseover" DefaultMouseDownCssClass="mousedown" DefaultCssClass="menuitem" CssClass="menustyle" Cursor="Pointer"
											HighlightTopMenu="False" Layout="Horizontal">
											<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
										</cc1:menu></td>
								</tr>
							<%--	<tr>
									<td class="Td_HeadingFormContainer" align="left">&nbsp;
										<asp:label id="LabelHdr" CssClass="Td_HeadingFormContainer" Runat="server"></asp:label><asp:label id="LabelTitle" CssClass="Td_HeadingFormContainer" Runat="server"></asp:label></td>
								</tr>--%>

                                <tr>
									<td class="Td_HeadingFormContainer" align="left">&nbsp;
										<YRSControls:YMCA_Header_WebUserControl ID="HeaderControl"  runat="server"></YRSControls:YMCA_Header_WebUserControl>
                                        </td>
								</tr>



							</table>
						</TD>
					</TR>
					<TR vAlign="top">
						<TD height="391"></TD>
						<TD>
							<div class="center">
								<table width="700">
									<TBODY>
										<tr>
											<td><iewc:tabstrip id="TabStripVRManager" runat="server" Height="30px" Width="700px" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
													TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
													AutoPostBack="True" BorderStyle="None">
													<iewc:Tab Text="List" ID="tabList"></iewc:Tab>
													<iewc:Tab Text="Disbursements" ID="tabGeneral"></iewc:Tab>
												</iewc:tabstrip></td>
										</tr>
										<tr>
											<td><iewc:multipage id="MultiPageVRManager" runat="server">
													<iewc:PageView>
														<table class="Table_WithBorder" width="700">
															<tr valign="top">
																<td>&nbsp;</td>
																<td>&nbsp;</td>
															</tr>
															<tr valign="top">
																<!--Shashi Shekhar:2010-02-15:Added one col in DataGridVRManager IsArchived for data archive impact-->
																<td valign="top">
																	<asp:Label ID="LabelRecordNotFound" Runat="server" CssClass="Label_Small">No Matching Records</asp:Label>
																	<DIV style="OVERFLOW: auto; WIDTH: 400px; HEIGHT: 200px; TEXT-ALIGN: left">
																		<asp:DataGrid ID="DataGridVRManager" Runat="server" Width="400" CssClass="DataGrid_Grid" AutoGenerateColumns="False"
																			allowsorting="true">
																			<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																			<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																			<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																			<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
																			<Columns>
																				<asp:TemplateColumn>
																					<ItemTemplate>
																						<asp:ImageButton id="ImageButtonSel" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																							CommandName="Select" ToolTip="Select"></asp:ImageButton>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																				<asp:BoundColumn DataField="PersID" Visible="False"></asp:BoundColumn>
																				<asp:BoundColumn DataField="SSN" HeaderText="SSN" SortExpression="SSN"></asp:BoundColumn>
																				<asp:BoundColumn DataField="FirstName" HeaderText="First Name" SortExpression="FirstName"></asp:BoundColumn>
																				<asp:BoundColumn DataField="LastName" HeaderText="Last Name" SortExpression="LastName"></asp:BoundColumn>
																				<asp:BoundColumn DataField="MiddleName" HeaderText="Middle Name" SortExpression="MiddleName"></asp:BoundColumn>
																				<asp:BoundColumn DataField="FundIDNo" HeaderText="FundID No" SortExpression="FundIDNo"></asp:BoundColumn>
																				<asp:BoundColumn DataField="BirthDate" HeaderText="Birth Date" Visible="False"></asp:BoundColumn>
																				<asp:BoundColumn DataField="IsArchived" HeaderText="IsArchived" Visible="False"></asp:BoundColumn>
																			</Columns>
																		</asp:DataGrid>
																	</DIV>
																</td>
																<td>
																	<table>
																		<tr>
																			<td>
																				<asp:Label ID="LabelFundNo" Runat="server" CssClass="Label_Small">Fund No.</asp:Label>
																			</td>
																			<td>
																				<asp:textbox width="100" runat="server" ID="TextBoxFundNo" CssClass="TextBox_Normal" MaxLength="10"></asp:textbox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<asp:Label ID="LabelSSNo" Runat="server" CssClass="Label_Small">SS No.</asp:Label>
																			</td>
																			<td>
																				<asp:textbox width="100" runat="server" ID="TextBoxSSNo" CssClass="TextBox_Normal"></asp:textbox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<asp:Label ID="LabelLastName" Runat="server" CssClass="Label_Small">Last Name</asp:Label>
																			</td>
																			<td>
																				<asp:textbox width="100" runat="server" ID="TextBoxLastName" CssClass="TextBox_Normal" MaxLength="30"></asp:textbox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<asp:Label ID="LabelFirstName" Runat="server" CssClass="Label_Small">First Name</asp:Label>
																			</td>
																			<td>
																				<asp:textbox width="100" runat="server" ID="TextBoxFirstName" CssClass="TextBox_Normal" MaxLength="20"></asp:textbox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<asp:Label ID="LabelCheckNo" Runat="server" CssClass="Label_Small">Check #</asp:Label>
																			</td>
																			<td>
																				<asp:textbox width="100" runat="server" ID="TextBoxCheckNo" CssClass="TextBox_Normal" MaxLength="20"></asp:textbox>
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
														<!--Added by imran on 30/10/2009 -->
														<table>
															<tr>
																<td><IMG title="image" height="1" alt="image" src="images/spacer.gif" width="1"></td>
															</tr>
															<tr>
																<td align="left">
																	<table class="Table_WithBorder" width="700">
																		<tr>
																			<td class="Td_ButtonContainer" align="right">
																				<asp:button id="ButtonCloseVR" CssClass="Button_Normal" Runat="server" Width="80" Text="Close"></asp:button></td>
																		</tr>
																	</table>
																</td>
															</tr>
														</table>
													</iewc:PageView>
													<iewc:pageview>
														<table class="Table_WithBorder" border="0" width="700" cellpadding="0" cellspacing="1">
															<tr valign="top">
																<td></td>
															</tr>
															<tr>
																<td colspan="4" valign="top">
																	<asp:Label ID="LabelPayeeSSN" Runat="server" CssClass="Label_Small" Visible="False"></asp:Label>
																	<DIV style="OVERFLOW: auto; WIDTH: 688px; HEIGHT: 250px; TEXT-ALIGN: left; valign: top">
																		<uc1:VoidUserControl id="VoidUserControl1" runat="server"></uc1:VoidUserControl>
																		<uc2:VoidDisbursement id="VoidDisbursement1" runat="server"></uc2:VoidDisbursement>
																	</DIV>
																</td>
															</tr>
															<!--  Added by Imran    -->
															<tr runat="server" id="trDeduction">
																<td colSpan="4">
																	<table border="0">
																		<tr>
																			<td align="center" valign="top">
																				<table height="100%">
																					<tr>
																						<td valign="top">
																							<asp:label id="LabelDeduction" CssClass="Label_Large" runat="server">Deductions</asp:label>
																						</td>
																						<td>
																							<table>
																								<tr>
																									<td Class="Label_Small">Deductions List</td>
																								</tr>
																								<tr>
																									<td>
																										<asp:datagrid id="dgDeduction" Runat="server" CssClass="DataGrid_Grid" Width="260" AutoGenerateColumns="false">
																											<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																											<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																											<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																											<Columns>
																												<asp:TemplateColumn>
																													<ItemTemplate>
																														<asp:CheckBox id="CheckBoxDeduction" runat="server" AutoPostBack="false"></asp:CheckBox>
																													</ItemTemplate>
																												</asp:TemplateColumn>
																												<asp:BoundColumn DataField="CodeValue" Visible="False"></asp:BoundColumn>
																												<asp:BoundColumn DataField="ShortDescription" HeaderText="Description"></asp:BoundColumn>
																												<asp:BoundColumn DataField="Amount" HeaderText="Amount"></asp:BoundColumn>
																											</Columns>
																										</asp:datagrid>
																									</td>
																								</tr>
																							</table>
																						</td>
																					</tr>
																					<tr>
																						<script language="javascript">
																					function ShowExistingDedClick()
																					{
																					 //alert( document.getElementById('dExistingDeductions').style.display);
																					 if (document.getElementById('dExistingDeductions').style.display=='none')
																						{			
																							document.getElementById('dExistingDeductions').style.display = 'block';
																							document.getElementById("hypExistingDeductions").innerHTML  =document.getElementById("hypExistingDeductions").innerHTML.replace('Show','Hide');
																						}
																					else
																						{
																							document.getElementById('dExistingDeductions').style.display = 'none';
																							document.getElementById("hypExistingDeductions").innerHTML = document.getElementById("hypExistingDeductions").innerHTML.replace('Hide','Show');					
																						}
																					
																					}
																				</script>
																						<td></td>
																						<td valign="top" class="Label_Small">
																							<asp:HyperLink NavigateUrl="javascript:ShowExistingDedClick();" runat="server" ID="hypExistingDeductions"
																								Visible="True"></asp:HyperLink>
																						</td>
																					</tr>
																					<tr>
																						<td></td>
																						<td>
																							<div id="dExistingDeductions" style="display:none;">
																								<table>
																									<tr>
																										<td Class="Label_Small"></td>
																									</tr>
																									<tr>
																										<td>
																											<asp:datagrid id="dgExistingDeduction" Runat="server" CssClass="DataGrid_Grid" Width="258" AutoGenerateColumns="false">
																												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																												<Columns>
																													<asp:TemplateColumn>
																														<ItemTemplate>
																															<asp:CheckBox id="CheckBoxDeduction" Checked="True" runat="server" AutoPostBack="false"></asp:CheckBox>
																														</ItemTemplate>
																													</asp:TemplateColumn>
																													<asp:BoundColumn DataField="WithholdingTypeCode" Visible="False"></asp:BoundColumn>
																													<asp:BoundColumn DataField="Description" HeaderText="Description"></asp:BoundColumn>
																													<asp:BoundColumn DataField="Amount" HeaderText="Amount"></asp:BoundColumn>
																												</Columns>
																											</asp:datagrid>
																										</td>
																									</tr>
																								</table>
																							</div>
																						</td>
																					</tr>
																					<tr>
																						<td valign="top">
																							<asp:label id="LabelAddressCheckSent" Runat="server" CssClass="Label_Small">Address Sent:</asp:label>
																						</td>
																						<td>
																							<asp:textbox id="TextBoxAddress" CssClass="TextBox_Normal" width="260" TextMode="MultiLine" Height="55"
																								runat="server" ReadOnly="True"></asp:textbox>
																						</td>
																					</tr>
																		</tr>
																	</table>
																</td>
																<td>
																	<table border="0" height="100%">
																		<tr>
																			<td valign="top">
																				<asp:label id="LabelAccNo" Runat="server" CssClass="Label_Small">A/C Number</asp:label></td>
																			<td>
																				<asp:textbox id="TextBoxAccountNo" CssClass="TextBox_Normal" width="200" runat="server" ReadOnly="True"></asp:textbox></td>
																		</tr>
																		<tr>
																			<td valign="top">
																				<asp:label id="LabelBankInfo" Runat="server" CssClass="Label_Small">Bank Info</asp:label></td>
																			<td>
																				<asp:textbox id="TextBoxBankInfo" CssClass="TextBox_Normal" width="200" TextMode="MultiLine"
																					Height="70px" runat="server" ReadOnly="True"></asp:textbox></td>
																		</tr>
																		<tr>
																			<td valign="top">
																				<asp:label id="LabelEntityType" Runat="server" CssClass="Label_Small">Entity Type</asp:label></td>
																			<td>
																				<asp:textbox id="TextBoxEntityType" CssClass="TextBox_Normal" width="200" runat="server" ReadOnly="True"></asp:textbox></td>
																		</tr>
																		<tr>
																			<td valign="top">
																				<asp:label id="LabelEntityAddress" Runat="server" CssClass="Label_Small">Entity Addr.</asp:label></td>
																			<td>
																				<asp:textbox id="TextBoxEntityAddress" CssClass="TextBox_Normal" width="200" TextMode="MultiLine"
																					Height="70px" runat="server" ReadOnly="True"></asp:textbox></td>
																		</tr>
																		<tr>
																			<td valign="top">
																				<asp:label id="LabelLegalEntity" Runat="server" CssClass="Label_Small">Legal Entity</asp:label></td>
																			<td>
																				<asp:textbox id="TextBoxLegalEntity" CssClass="TextBox_Normal" width="200" runat="server" ReadOnly="True"></asp:textbox></td>
																		</tr>
																	</table>
																</td>
															</tr>
														</table>
											</td>
										</tr>
									<!--End  -->
								</table>
								<!--Added by imran on 30/10/2009 -->
								<table>
									<tr>
										<td><IMG title="image" height="1" alt="image" src="images/spacer.gif" width="1"></td>
									</tr>
									<tr>
										<td align="left">
											<table class="Table_WithBorder" width="700">
												<tr>
													<td align="left" class="Td_ButtonContainer" width="450px">
														<asp:Button ID="ButtonPHR" Runat="server" Text="PHR" Width="70" CssClass="Button_Normal" enabled="true"></asp:Button>
													</td>
													<td align="right" class="Td_ButtonContainer" width="70px">
														<asp:Button ID="ButtonSave" Runat="server" Text="Save" Width="70" CssClass="Button_Normal" Enabled="True"></asp:Button>
														<input type="hidden" id="hdnDisbId" runat="server" NAME="hdnDisbId" /> <input type="hidden" id="hdnListDisbId" runat="server" NAME="hdnListDisbId" />
													</td>
													<td class="Td_ButtonContainer" align="right" width="70px">
														<asp:button id="ButtonClose" CssClass="Button_Normal" Runat="server" Width="80" Text="Close"></asp:button>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
							</iewc:pageview> </iewc:multipage></TD>
					</TR>
					<tr>
						<td><IMG title="image" height="1" alt="image" src="images/spacer.gif" width="1"></td>
					</tr>
					<tr style="DISPLAY: none">
						<td>
							<table class="Table_WithBorder" width="700">
								<tr>
									<td class="Td_ButtonContainer" align="right"></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td><asp:literal id="litPersID" runat="server" Visible="False"></asp:literal><asp:literal id="litDisbID" runat="server" Visible="False"></asp:literal><asp:literal id="litFundID" runat="server" Visible="False"></asp:literal><asp:literal id="litWHAmount" runat="server" Visible="False"></asp:literal><asp:literal id="litGross" runat="server" Visible="False"></asp:literal><asp:literal id="litPayeeId" runat="server" Visible="False"></asp:literal><asp:literal id="litAddressID" runat="server" Visible="False"></asp:literal><asp:literal id="litDisbNbr" runat="server" Visible="False"></asp:literal><asp:placeholder id="PlaceHolderMessageBox" runat="server"></asp:placeholder></td>
					</tr>
				</TBODY>
		</TABLE>
		</DIV></TD></TR></TBODY></TD></TR></TD></FORM></TD></TR></TD></TR> 
		<!--#include virtual="bottom.html"--> 
		</TABLE></TABLE>
	</body>
</HTML>
