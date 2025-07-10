<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DataArchive.aspx.vb" Inherits="YMCAUI.DataArchive"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!--#include virtual="top.html"-->
<script language="javascript">

//Function to open the confirmation dialogue box
	function confirm_Retrieve()
		{
		  var NoOfRecord=document.getElementById('<%=hdNoOfMovedRec.ClientID%>').value;     
		  if (confirm(NoOfRecord + " Records selected for retrieval from archive database, do you want to proceed?")==true)
 		     return true;
 		  else
    		return false;
		} 
</script>
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
					CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
					MenuFadeDelay="2" mouseovercssclass="MouseOver">
					<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
				</cc1:menu></td>
		</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="left">Retrieve Archived Data
				<asp:label id="LabelTitle" runat="server" Width="432px"></asp:label></td>
		</tr>
		<tr>
			<td height="5"></td>
		</tr>
	</table>
	<div class="Div_Center">
		<table cellSpacing="0" cellPadding="0" width="700">
			<tr>
				<td><iewc:multipage id="MultiPageDeathCalc" runat="server">
						<iewc:PageView>
							<div class="Div_Center">
								<table class="Table_WithBorder" width="700">
									<tr>
										<td>
											<table width="690">
												<tr>
													<td align="left">
														<asp:label id="LabelNoDataFound" runat="server" Visible="False">No Matching Records</asp:label></td>
												</tr>
												<tr>
													<td align="left" valign="bottom">
														<table>
															<tr id="trSearchGridHeader" runat="server" valign="bottom">
																<td class="Td_HeadingFormContainer" align="left" width="240px">Select data to 
																	retrieve
																	<asp:label id="Label2" runat="server"></asp:label></td>
															</tr>
															<tr>
																<td>
																	<div id="divGridSearch" runat="server" style="OVERFLOW: scroll; WIDTH: 430px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT:180px; BORDER-BOTTOM-STYLE: none">
																		<asp:DataGrid id="DataGrid_Search" runat="server" Width="414" CssClass="DataGrid_Grid" AllowSorting="True"
																			allowPaging="False" PageSize="500" AutoGenerateColumns="False" EnableViewState="False" ShowHeader="true">
																			<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																			<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																			<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																			<selectedItemStyle></selectedItemStyle>
																			<Columns>
																				<asp:TemplateColumn>
																					<ItemTemplate>
																						<asp:CheckBox ID="CheckBoxSelect" Runat="server" EnableViewState="False"></asp:CheckBox>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																				<asp:BoundColumn DataField="PersID" SortExpression="PersID" HeaderText="Pers ID"></asp:BoundColumn>
																				<asp:BoundColumn DataField="SSN" SortExpression="SSN" HeaderText="SS No."></asp:BoundColumn>
																				<asp:BoundColumn DataField="LastName" SortExpression="LastName" HeaderText="Last Name"></asp:BoundColumn>
																				<asp:BoundColumn DataField="FirstName" SortExpression="FirstName" HeaderText="First Name"></asp:BoundColumn>
																				<asp:BoundColumn DataField="MiddleName" SortExpression="MiddleName" HeaderText="Middle Name"></asp:BoundColumn>
																				<asp:BoundColumn DataField="FundStatus" SortExpression="FundStatus" HeaderText="Fund Status"></asp:BoundColumn>
																				<asp:BoundColumn DataField="FundIDNo" SortExpression="FundIDNo" HeaderText="FundID No"></asp:BoundColumn>
																				<asp:BoundColumn DataField="FundUniqueId" SortExpression="FundUniqueId" HeaderText="FundUnique Id"></asp:BoundColumn>
																				<asp:BoundColumn DataField="FundStatusType" SortExpression="FundStatusType" HeaderText="FundStatusType"></asp:BoundColumn>
																			</Columns>
																		</asp:DataGrid>
																		<asp:Label id="lbl_Search_MoreItems" runat="server" CssClass="Message" Visible="False" EnableViewState="False" />
																	</div>
																</td>
															</tr>
														</table>
													</td>
													<td align="right" valign="top">
														<table>
															<TR>
																<td align="left">
																	<span CssClass="Label_Small">Fund No.</span></td>
																<td align="center">
																	<asp:TextBox id="TextBoxFundNo" runat="server" width="150" cssClass="TextBox_Normal"></asp:TextBox>
																</td>
															</TR>
															<TR>
																<td align="left">
																	<span CssClass="Label_Small">SS No.</span></td>
																<td align="center">
																	<asp:TextBox id="TextBoxSSNo" runat="server" width="150" cssClass="TextBox_Normal"></asp:TextBox>
																	<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid SSN" ControlToValidate="TextBoxSSNo"
																		ValidationExpression="\d{3}-\d{2}-\d{4}|\d{9}|[A-Z]\d{8}" Enabled="False" Visible="false"></asp:RegularExpressionValidator>
																</td>
															</TR>
															<TR>
																<td align="left">
																	<span CssClass="Label_Small">Last Name</span></td>
																<td align="center">
																	<asp:TextBox id="TextBoxLastName" runat="server" width="150" cssClass="TextBox_Normal"></asp:TextBox>
																</td>
															</TR>
															<TR>
																<td align="left">
																	<span CssClass="Label_Small">First Name</span></td>
																<td align="center">
																	<asp:TextBox id="TextBoxFirstName" runat="server" width="150" cssClass="TextBox_Normal"></asp:TextBox>
																</td>
															</TR>
															<TR>
																<td align="left">
																	<span CssClass="Label_Small">City</span></td>
																<td align="center">
																	<asp:TextBox id="TextBoxCity" runat="server" width="150" cssClass="TextBox_Normal"></asp:TextBox>
																</td>
															</TR>
															<TR>
																<td align="left">
																	<span CssClass="Label_Small">State</span></td>
																<td align="center">
																	<asp:TextBox id="TextBoxState" runat="server" width="150" cssClass="TextBox_Normal"></asp:TextBox>
																</td>
															</TR>
														</table>
														<table>
															<tr>
																<td>
																	<asp:Button id="ButtonFind" runat="server" Text="Find" Width="80" CssClass="Button_Normal"></asp:Button>
																</td>
																<td align="right">
																	<asp:Button id="ButtonClear" runat="server" Text="Clear" Width="80" CssClass="Button_Normal"></asp:Button>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												
												<tr>
													<td>
														<asp:Button id="ButtonSelectAll" runat="server" Text="Select All" Width="85" CssClass="Button_Normal"
															tooltip="Select All Records."></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
														<asp:Button id="ButtonMove" runat="server" Text="Move" Width="80" CssClass="Button_Normal" tooltip="Move selected record."></asp:Button></td>
												</tr>
												
												<tr>
													<td><hr id="hrSerch" runat="server">
														</hr>
													</td>
												</tr>
												<tr>
													<td>
														<table>
															<tr id="trMovedGridHeader" runat="server" Width="250px">
																<td class="Td_HeadingFormContainer" align="left">Selected data for retrieval
																	<asp:label id="Label3" runat="server"></asp:label></td>
															</tr>
															<tr>
																<td>
																	<div id="divgridMove" runat="server" style="OVERFLOW: scroll; WIDTH: 430px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; POSITION: static; HEIGHT: 150px; BORDER-BOTTOM-STYLE: none">
																		<asp:DataGrid id="DataGrid_Moved" runat="server" Width="414" CssClass="DataGrid_Grid" AllowSorting="True"
																			EnableViewState="False" allowPaging="False" PageSize="500" AutoGenerateColumns="False" ShowHeader="true">
																			<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																			<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																			<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																			<selectedItemStyle cssClass="DataGrid_SelectedStyle"></selectedItemStyle>
																			<Columns>
																				<asp:TemplateColumn>
																					<ItemTemplate>
																						<asp:LinkButton Id="LinkButtonRemove" CommandName="lbtnRemove" Runat="server" tooltip="Remove record."
																							EnableViewState="False">Remove</asp:LinkButton>
																					</ItemTemplate>
																				</asp:TemplateColumn>
																				<asp:BoundColumn DataField="PersID" SortExpression="PersID" HeaderText="Pers ID"></asp:BoundColumn>
																				<asp:BoundColumn DataField="SSN" SortExpression="SSN" HeaderText="SS No."></asp:BoundColumn>
																				<asp:BoundColumn DataField="LastName" SortExpression="LastName" HeaderText="Last Name"></asp:BoundColumn>
																				<asp:BoundColumn DataField="FirstName" SortExpression="FirstName" HeaderText="First Name"></asp:BoundColumn>
																				<asp:BoundColumn DataField="MiddleName" SortExpression="MiddleName" HeaderText="Middle Name"></asp:BoundColumn>
																				<asp:BoundColumn DataField="FundStatus" SortExpression="FundStatus" HeaderText="Fund Status"></asp:BoundColumn>
																				<asp:BoundColumn DataField="FundIDNo" SortExpression="FundIDNo" HeaderText="FundID No"></asp:BoundColumn>
																				<asp:BoundColumn DataField="FundUniqueId" SortExpression="FundUniqueId" HeaderText="FundUnique Id"></asp:BoundColumn>
																				<asp:BoundColumn DataField="FundStatusType" SortExpression="FundStatusType" HeaderText="FundStatusType"></asp:BoundColumn>
																			</Columns>
																		</asp:DataGrid>
																		<asp:Label id="Label1" runat="server" CssClass="Message" Visible="False" EnableViewState="False" />
																	</div>
																</td>
															</tr>
														</table>
													</td>
												
												<tr>
													<td>
														<asp:Button id="ButtonRetrieve" runat="server" Text="Retrieve Data" CssClass="Button_Normal"
															tooltip="Retrieve data."></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
														<asp:Button id="ButtonRemoveAll" runat="server" Text="Remove All" CssClass="Button_Normal" tooltip="Remove All."></asp:Button>
												</tr>
												<table width="680">
													<tr>
														<td align="right" class="Td_ButtonContainer">
															<asp:Button id="ButtonCancel" runat="server" Text="Cancel" Width="80" CssClass="Button_Normal"></asp:Button>
														</td>
													</tr>
												</table>
									</tr>
								</table>
								<tr>
									<td>&nbsp;<INPUT type="hidden" id="hdNoOfMovedRec" runat="server"><INPUT type="hidden" id="hdCount" runat="server"></td>
								</tr>
				</td>
			</tr>
		</table>
	</div>
	</iewc:PageView> </iewc:multipage></td></tr></table></div><asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder></form>
<!--#include virtual="bottom.html"-->
