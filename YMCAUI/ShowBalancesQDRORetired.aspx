<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowBalancesQDRORetired.aspx.vb" Inherits="YMCAUI.ShowBalancesQDRORetired" %>

<!--#include virtual="TopNew.htm"-->
<form id="Form1" method="post" runat="server">
    <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="700">
        <tr>
            <td class="Td_BackGroundColorMenu" align="left">
                <cc1:Menu ID="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
                    CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
                    MenuFadeDelay="2" mouseovercssclass="MouseOver">
                </cc1:Menu>
                <selectedmenuitemstyle forecolor="#3B5386" backcolor="#FBC97A"></selectedmenuitemstyle>
            </td>
        </tr>
        <tr>
            <td class="Td_HeadingFormContainer" align="center" colspan="2">
                <img title="image" height="10" alt="image" src="images/spacer.gif" width="10">
                Show Annuities
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
        </tr>
    </table>
    <div class="Div_Center">
        <table class="Table_WithBorder" width="100%">
            <tbody>
                <tr>
                    <td>
                        <div>
                            <table width="100%">
                                <tr>
                                    <%--START: 2017.01.28 | MMR | YRS-AT-3299 | Commented existing code and changed text--%>
                                    <%--<td align="center"><b>Participants's Annuities </b>&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                    <td align="center"><b>Participant's Annuities </b>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <%--END: 2017.01.28 | MMR | YRS-AT-3299 | Commented existing code and changed text--%>
                                    </td>
                                    <td align="right"></td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" width="100%">
                        <%--<div style="OVERFLOW: auto; WIDTH: 700px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; BORDER-BOTTOM-STYLE: none">--%>
                            <asp:datalist id="DataListParticipant" runat="server" datakeyfield="SSNo">
								<HeaderTemplate>
									<table width="100%">
										<TR class="DataGrid_HeaderStyle">
											<TD style="text-align:center">SSNo.
											</TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
											<TD style="text-align:center">Last Name
											</TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
											<TD style="text-align:center">First Name
											</TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
											<TD style="text-align:center">Fund Status
											</TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
										</TR>
								</HeaderTemplate>
								<ItemTemplate>
									<TR>
										<TD>
											<asp:Label id="lblSSno" Runat="Server" cssclass="Label_Small" style="text-align:center">
												<%# DataBinder.Eval(Container.DataItem, "SSNo") %>
											</asp:Label></TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
										<TD>
											<asp:Label id="lblLastNAme" Runat="Server" cssclass="Label_Small" style="text-align:center">
												<%# DataBinder.Eval(Container.DataItem, "LastName") %>
											</asp:Label></TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
										<TD>
											<asp:Label id="lbaFirstName" Runat="Server" cssclass="Label_Small" style="text-align:center">
												<%# DataBinder.Eval(Container.DataItem, "FirstName") %>
											</asp:Label></TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
										<TD>
											<asp:Label id="Label4" Runat="Server" cssclass="Label_Small" text="Retired" style="text-align:center"></asp:Label></TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
									</TR>
									<TR>
										<TD colspan="4">
							 <asp:DataGrid id="DatagridSummaryBalList" runat="server" CssClass="DataGrid_Grid" WIDTH="100%"
												AutoGenerateColumns="false">
												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
												<Columns>
													 <asp:BoundColumn HeaderText="Annuity Source Code" DataField="AnnuitySourceCode"></asp:BoundColumn>
													<asp:BoundColumn HeaderText="Plan Type" DataField="PlanType" />
													<asp:BoundColumn HeaderText="Purchase Date" DataField="PurchaseDate" DataFormatString="{0:MM/dd/yyyy}" />
													<asp:BoundColumn HeaderText="Annuity Type" DataField="AnnuityType" />
													<asp:BoundColumn HeaderText="Current Payment" DataField="CurrentPayment" DataFormatString="{0:F2}" 
 HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Emp PreTax Current Payment" DataField="EmpPreTaxCurrentPayment" DataFormatString="{0:F2}" 
 HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Emp PostTax Current Payment" DataField="EmpPostTaxCurrentPayment" DataFormatString="{0:F2}" 
 HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Ymca PreTax Current Payment" DataField="YmcaPreTaxCurrentPayment" DataFormatString="{0:F2}" 
 HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Emp PreTax Remaining Reserves" DataField="EmpPreTaxRemainingReserves" 
 DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Emp PostTax Remaining Reserves" DataField="EmpPostTaxRemainingReserves" 
 DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="Ymca PreTax Remaining Reserves" DataField="YmcaPreTaxRemainingReserves" 
 DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="SS Levling Amount" DataField="SSLevelingAmt" DataFormatString="{0:F2}" 
 HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="SS Reduction Amount" DataField="SSReductionAmt" DataFormatString="{0:F2}" 
 HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
													<asp:BoundColumn HeaderText="SS Reduction Effective Date" DataField="SSReductionEftDate" DataFormatString="{0:MM/dd/yyyy}" />
												</Columns>
											</asp:DataGrid> 

                                           

										</TD>
									</TR>
								</ItemTemplate>
								<FooterTemplate>
                                </table>
                                </FooterTemplate>
                         </asp:DataList>

                   <%-- </div>--%>
    </TD></TR>
	<div class="Div_Center"></div>
    <tr>
        <td width="100%">
            <div>
                <table width="100%">
                    <tr>
                        <td align="center"><b>Recipient's Annuities </b>&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td align="right"></td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="4">
           <%-- <div style="OVERFLOW: auto; WIDTH: 700px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; BORDER-BOTTOM-STYLE: none">--%>
                <asp:datalist id="DatalistBeneficiary" runat="server" datakeyfield="id">
					<HeaderTemplate>
						<table width="100%">
							<TR class="DataGrid_HeaderStyle">
								<TD style="text-align:center">SSNo.
								</TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
								<TD style="text-align:center">Last Name
								</TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
								<TD style="text-align:center">First Name
								</TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
								<TD style="text-align:center">Fund status
								</TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
							</TR>
					</HeaderTemplate>
					<ItemTemplate>
						<TR>
							<TD>
								<asp:Label id="Label1" Runat="Server" cssclass="Label_Small" style="text-align:center">
									<%# DataBinder.Eval(Container.DataItem, "SSNo") %>
								</asp:Label></TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
							<TD>
								<asp:Label id="Label2" Runat="Server" cssclass="Label_Small" style="text-align:center">
									<%# DataBinder.Eval(Container.DataItem, "LastName") %>
								</asp:Label></TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
							<TD>
								<asp:Label id="Label3" Runat="Server" cssclass="Label_Small" style="text-align:center">
									<%# DataBinder.Eval(Container.DataItem, "FirstName") %>
								</asp:Label></TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
							<TD>
								<asp:Label id="Label6" Runat="Server" cssclass="Label_Small" text="RQ" style="text-align:center"></asp:Label></TD><%-- PPP | 01/23/2017 | YRS-AT-3299 | Added style="text-align:center" --%>
							<TD>
								<asp:Label id="Label5" Runat="Server" cssclass="Label_Small" visible="false">
									<%# DataBinder.Eval(Container.DataItem, "id") %>
								</asp:Label></TD>
						</TR>
						<TR>
							<TD colspan="4">
                                <asp:Label id="lblBeneficiarySummaryBalList" Runat="Server" cssclass="Label_Small" text=""></asp:Label>  <%-- Added by - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098) --%>
   
                               <%-- Commented by START - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098) --%>
								<%--<asp:DataGrid id="DatagridBeneficiarySummaryBalList" runat="server" CssClass="DataGrid_Grid" WIDTH="100%" 
 AutoGenerateColumns="false">
									<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<Columns>
										<asp:BoundColumn HeaderText="Annuity Source Code" DataField="AnnuitySourceCode"></asp:BoundColumn>
										<asp:BoundColumn HeaderText="Plan Type" DataField="PlanType" />
										<asp:BoundColumn HeaderText="Purchase Date" DataField="PurchaseDate" DataFormatString="{0:MM/dd/yyyy}" />
										<asp:BoundColumn HeaderText="Annuity Type" DataField="AnnuityType" />
										<asp:BoundColumn HeaderText="Current Payment" DataField="CurrentPayment" DataFormatString="{0:F2}" 
 HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="Emp PreTax Current Payment" DataField="EmpPreTaxCurrentPayment" DataFormatString="{0:F2}" 
 HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="Emp PostTax Current Payment" DataField="EmpPostTaxCurrentPayment" DataFormatString="{0:F2}" 
 HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="Ymca PreTax Current Payment" DataField="YmcaPreTaxCurrentPayment" DataFormatString="{0:F2}" 
 HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="Emp PreTax Remaining Reserves" DataField="EmpPreTaxRemainingReserves" 
 DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="Emp PostTax Remaining Reserves" DataField="EmpPostTaxRemainingReserves" 
 DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="Ymca PreTax Remaining Reserves" DataField="YmcaPreTaxRemainingReserves" 
 DataFormatString="{0:F2}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="SS Levling Amount" DataField="SSLevelingAmt" DataFormatString="{0:F2}" 
 HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="SS Reduction Amount" DataField="SSReductionAmt" DataFormatString="{0:F2}" 
 HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" />
										<asp:BoundColumn HeaderText="SS Reduction Effective Date" DataField="SSReductionEftDate" DataFormatString="{0:MM/dd/yyyy}" />
									</Columns>
								</asp:DataGrid><table   class="DataGrid_Grid" style="WIDTH: 100%; BORDER-COLLAPSE: collapse" border="1">

                                               --%>
                                
                                 <%-- Commented by  END - Chandra sekar-2016.08.22-YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098) --%>

                                 
							</TD>
						</TR>
					</ItemTemplate>
					<FooterTemplate>
				        </table>
				    </FooterTemplate>
				</asp:datalist>
           <%-- </div>--%>
        </td>
    </tr>
    </TBODY></TABLE></DIV>
	<tr>
        <td class="Td_ButtonContainer" valign="top" colspan="2">
            <table class="Table_WithoutBorder" width="100%">
                <tr>
                    <td class="Td_ButtonContainer" align="right" width="20%">
                        <asp:button id="ButtonOK" runat="server" cssclass="Button_Normal" text="OK" causesvalidation="True"
                            width="60px"></asp:button>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</form>
<!--#include virtual="bottom.html"-->
