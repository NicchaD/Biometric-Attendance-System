<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ShowBalancesQDRONonRetired.aspx.vb" Inherits="YMCAUI.ShowBalancesQDRONonRetired"%>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<!--#include virtual="TopNew.htm"-->
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="600">
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" mouseovercssclass="MouseOver" MenuFadeDelay="2" DefaultMouseUpCssClass="mouseup"
					DefaultMouseOverCssClass="mouseover" DefaultMouseDownCssClass="mousedown" DefaultCssClass="menuitem" CssClass="menustyle" Cursor="Pointer"
					HighlightTopMenu="False" Layout="Horizontal"></cc1:menu><SELECTEDMENUITEMSTYLE BackColor="#FBC97A" ForeColor="#3B5386"></SELECTEDMENUITEMSTYLE></td>
		</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="center" colSpan="2"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
				Show Balances
			</td>
		</tr>
		<tr>
			<td>&nbsp;
			</td>
		</tr>
	</table>
	<DIV class="Div_Center">
		<TABLE class="Table_WithBorder" width="698">
			<TBODY>
				<TR>
					<TD>
						<DIV>
							<TABLE width="698">
								<TR>
									<TD align="center"><B>Participant's Balances </B>&nbsp;&nbsp;&nbsp;&nbsp; <%--PPP | 09/21/2016 | YRS-AT-2529 | Changed "Participants's" to "Participant's" --%>
									</TD>
									<TD align="right"></TD>
								</TR>
							</TABLE>
						</DIV>
					</TD>
				</TR>
				<TR>
					<td colSpan="4" width="698">
						<div style="OVERFLOW: scroll; WIDTH: 700px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 150px; BORDER-BOTTOM-STYLE: none">
                            <%--START: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
                            <div id="divParticipantTable" runat="server"></div>
							
                            <%--<asp:DataList id="DataListParticipant" runat="server" DataKeyField="SSNo">
								<HeaderTemplate>
									<TABLE width="698">
										<TR class="DataGrid_HeaderStyle">
											<TD align="center">SSNo.
											</TD>
											<TD align="center">Last Name
											</TD>
											<TD align="center">First Name
											</TD>
											<TD align="center">Fund Status
											</TD>
										</TR>
								</HeaderTemplate>
								<ItemTemplate>
									<TR>
										<TD align="center">
											<asp:Label id="lblSSno" Runat="Server" cssclass="Label_Small">
												<%# DataBinder.Eval(Container.DataItem, "SSNo") %>
											</asp:Label></TD>
										<TD align="center">
											<asp:Label id="lblLastNAme" Runat="Server" cssclass="Label_Small">
												<%# DataBinder.Eval(Container.DataItem, "LastName") %>
											</asp:Label></TD>
										<TD align="center">
											<asp:Label id="lbaFirstName" Runat="Server" cssclass="Label_Small">
												<%# DataBinder.Eval(Container.DataItem, "FirstName") %>
											</asp:Label></TD>
										<TD align="center">
											<asp:Label id="Label4" Runat="Server" cssclass="Label_Small" text="Active"></asp:Label></TD>
									</TR>
									<TR>
										<TD colspan="4">
											<asp:DataGrid id="DatagridSummaryBalList" runat="server" CssClass="DataGrid_Grid" WIDTH="100%"
												AutoGenerateColumns="false">
												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
												<Columns>
													<asp:BoundColumn HeaderText="Acct" DataField="AcctType" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
														<ItemStyle Width="150px"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn HeaderText="Taxable" DataField="PersonalPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
														ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Non-Taxable" DataField="PersonalPostTax" DataFormatString="{0:F2}"
														HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Interest" DataField="PersonalInterestBalance" DataFormatString="{0:F2}"
														HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="YMCA Taxable" DataField="YMCAPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
														ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="YMCA Interest" DataField="YMCAInterestBalance" DataFormatString="{0:F2}"
														HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Acct. Total" DataField="TotalTotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
														ItemStyle-HorizontalAlign="Right" />
												</Columns>
											</asp:DataGrid>
						</div>
					</td>
				</TR>
							</ItemTemplate>
							<FooterTemplate>
		</TABLE>
		</FooterTemplate> </asp:DataList>--%>
                            <%--END: PPP | 11/28/2016 | YRS-AT-3145 & YRS-AT-3265--%>
	</DIV>
	</TD></TR>
	<TR>
		<TD width="698">
			<DIV>
				<TABLE width="100%">
					<TR>
						<TD align="center"><B>Recipient's Balances </B>&nbsp;&nbsp;&nbsp;&nbsp;
						</TD>
						<TD align="right"></TD>
					</TR>
				</TABLE>
			</DIV>
		</TD>
	</TR>
	<TR>
		<td colSpan="4">
			<div style="OVERFLOW: scroll; WIDTH: 700px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 320px; BORDER-BOTTOM-STYLE: none">
                <%--START: PPP | 08/29/2016 | YRS-AT-2529 | Summary report HTML is generated through code behind and gets assigned to divBeneficiaryTable--%>
                <div id="divBeneficiaryTable" runat="server"></div>
				<%--<asp:DataList id="DatalistBeneficiary" runat="server" DataKeyField="id">
					<HeaderTemplate>
						<TABLE width="680">
							<TR class="DataGrid_HeaderStyle">
								<TD align="center">SSNo.
								</TD>
								<TD align="center">Last Name
								</TD>
								<TD align="center">First Name
								</TD>
								<TD align="center">Fund status
								</TD>
							</TR>
					</HeaderTemplate>
					<ItemTemplate>
						<TR>
							<TD align="center">
								<asp:Label id="Label1" Runat="Server" cssclass="Label_Small">
									<%# DataBinder.Eval(Container.DataItem, "SSNo") %>
								</asp:Label></TD>
							<TD align="center">
								<asp:Label id="Label2" Runat="Server" cssclass="Label_Small">
									<%# DataBinder.Eval(Container.DataItem, "LastName") %>
								</asp:Label></TD>
							<TD align="center">
								<asp:Label id="Label3" Runat="Server" cssclass="Label_Small">
									<%# DataBinder.Eval(Container.DataItem, "FirstName") %>
								</asp:Label></TD>
							<TD align="center">
								<asp:Label id="Label6" Runat="Server" cssclass="Label_Small" text="QD"></asp:Label></TD>
							<TD align="center">
								<asp:Label id="Label5" Runat="Server" cssclass="Label_Small" visible="false">
									<%# DataBinder.Eval(Container.DataItem, "id") %>
								</asp:Label></TD>
						</TR>
						<TR>
							<TD colspan="4">
								<asp:DataGrid id="DatagridBeneficiarySummaryBalList" runat="server" CssClass="DataGrid_Grid" WIDTH="100%"
									AutoGenerateColumns="false">
									<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<Columns>
										<asp:BoundColumn HeaderText="Acct" DataField="AcctType" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
											<ItemStyle Width="150px"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn HeaderText="EmpTaxable" DataField="PersonalPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
											ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="EmpNon-Taxable" DataField="PersonalPostTax" DataFormatString="{0:F2}"
											HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="EmpInterest" DataField="PersonalInterestBalance" DataFormatString="{0:F2}"
											HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="YMCATaxable" DataField="YMCAPreTax" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
											ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="YMCAInterest" DataField="YMCAInterestBalance" DataFormatString="{0:F2}"
											HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="Acct Total" DataField="TotalTotal" DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="center"
											ItemStyle-HorizontalAlign="Right" />
									</Columns>
								</asp:DataGrid></div>
		</td>
	</TR>
	</ItemTemplate>
	<FooterTemplate>
				</table>
				</FooterTemplate>
	</asp:DataList>--%>
                <%--END: PPP | 08/29/2016 | YRS-AT-2529 | Summary report HTML is generated through code behind and gets assigned to divBeneficiaryTable--%>
                
    </DIV>
	<TR>
		<TD class="Td_ButtonContainer" vAlign="top" colSpan="2">
			<TABLE class="Table_WithoutBorder" width="100%">
				<TR>
					<TD class="Td_ButtonContainer" align="right" width="20%">
						<asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Text="OK" CausesValidation="True"
							Width="60px"></asp:button></TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
</form> <!--#include virtual="bottom.html"-->
<DIV></DIV>
</TD></TR></TBODY>
<DIV></DIV>
<DIV></DIV>
<DIV></DIV>
<DIV></DIV>
<DIV></DIV>
<DIV></DIV>
<DIV></DIV>
<DIV></DIV>
<DIV></DIV>
<DIV></DIV>
</DIV>
