<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddEditQDRO.aspx.vb" Inherits="YMCAUI.AddEditQDRO" %>

<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_ToolBar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<title>YMCA YRS </title>
<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
<form id="Form1" method="post" runat="server">
 <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="700">
        <tr>
            <td class="Td_HeadingFormContainer" align="left">
                <YRSControls:YMCA_ToolBar_WebUserControl ID="Toolbar_Control" runat="server" ShowLogoutLinkButton="false"
                    ShowHomeLinkButton="false" ShowReleaseLinkButton="false" />
                <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <div class="Div_Center">
                    <table class="Table_WithBorder" width="100%">
                        <tr>
                            <td valign="middle">
                                <div class="Div_Center">
                                    <table class="Table_WithoutBorder" width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td class="Td_ButtonContainer" align="right" colspan="2">
                                                    <asp:button id="ButtonUpdate" runat="server" cssclass="Button_Normal" text="Update Item"
                                                        visible="false" causesvalidation="True" width="90px"></asp:button>
                                                    <asp:button id="ButtonAdd" runat="server" cssclass="Button_Normal" text="Add..."
                                                        causesvalidation="false" width="90px"></asp:button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="middle" align="left">
                                                    <div style="overflow: auto; width: 100%; border-top-style: none; border-right-style: none; border-left-style: none; height: 160px; border-bottom-style: none">
                                                        <asp:datagrid id="DataGridQdroInfo" runat="server" cssclass="DataGrid_Grid" width="100%"
                                                            allowsorting="false">
												<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
												<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
												<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
												<Columns>
													<asp:TemplateColumn HeaderStyle-Width="2%">
														<ItemTemplate>
															<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\edits.gif" CausesValidation="False"
																CommandName="EditSelect" ToolTip="Select"></asp:ImageButton>
														</ItemTemplate>
													</asp:TemplateColumn>
                                                      <asp:TemplateColumn HeaderStyle-Width="2%">
														<ItemTemplate>
															<asp:ImageButton id="ImageButtonView" runat="server" ImageUrl="images\view.gif" CausesValidation="False"
																CommandName="ViewSelect" ToolTip="View Beneficiary Information"></asp:ImageButton>
														</ItemTemplate>
													</asp:TemplateColumn>
												</Columns>
												<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
											</asp:datagrid>
                                                    </div>
                                                </td>
                                                <td valign="top" align="right" width="50%">
                                                    <table class="Table_WithBorder" height="120" border="0">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:label id="LabelQdroType" runat="server" cssclass="Label_Small" width="120">
												QDRO Type</asp:label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:textbox id="TextBoxQdroType" runat="server" cssclass="TextBox_Normal" width="100"
                                                                    readonly="true"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:label id="LabelStatus" runat="server" cssclass="Label_Small" width="90">
												Status</asp:label>
                                                            </td>
                                                            <td align="left">
                                                      <asp:dropdownlist id="DropDownListStatus" runat="server" width="145px" cssclass="DropDown_Normal"> <%-- Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Changed width for alignment--%>
														<asp:ListItem></asp:ListItem>
														<asp:ListItem Value="Pend">Pending</asp:ListItem>
														<asp:ListItem Value="Can">Cancelled</asp:ListItem>
														<asp:ListItem Value="Comp">Completed</asp:ListItem>
														<asp:ListItem Value="Exp">Expired</asp:ListItem>
													</asp:dropdownlist>
                                                                <asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" errormessage="Status can not be blank"
                                                                    controltovalidate="DropDownListStatus" display="Dynamic">*</asp:requiredfieldvalidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:label id="LabelStatusDate" runat="server" cssclass="Label_Small" width="90px">
													Status Date</asp:label>
                                                            </td>
                                                            <td align="left">
                                                                <uc1:DateUserControl ID="TextBoxStatusDate" runat="server"></uc1:DateUserControl>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:label id="LabelDraftDate" runat="server" cssclass="Label_Small" width="90px">
												Draft Date</asp:label>
                                                            </td>
                                                            <td align="left">
                                                                <uc1:DateUserControl ID="TextBoxDraftDate" runat="server"></uc1:DateUserControl>
                                                            </td>
                                                        </tr>
                                                        <%--Start - Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Added Control--%>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:label id="LabelSpouse" runat="server" cssclass="Label_Small" width="90px">
												Spouse</asp:label>
                                                            </td>
                                                            <td>
                                                                <asp:dropdownlist id="DropDownListSpouse" runat="server" width="145px" cssclass="DropDown_Normal" autopostback="false" Enabled ="false">                                                                    
														            <asp:ListItem Value="None" Selected ="true">None</asp:ListItem>
                                                                </asp:dropdownlist>
                                                            </td>
                                                        </tr>
                                                        <%--End -Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Added Control--%>
                                                        <tr>
                                                            <td colspan="2">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table class="Table_WithoutBorder" width="100%" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td class="Td_ButtonContainer2" align="right" width="100%" colspan="2">
                                                                            <asp:literal id="Literal1" runat="server" visible="false"></asp:literal>
                                                                            <asp:button id="ButtonSave" runat="server" cssclass="Button_Normal" text="Save" causesvalidation="True"
                                                                                width="73px"></asp:button>
                                                                            <asp:button id="ButtonCancel" runat="server" cssclass="Button_Normal" text="Cancel"
                                                                                causesvalidation="False" width="73px"></asp:button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            
                                        </tbody>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <%-- START : Chandrasekar - 2016.07.05 - YRS-AT-2481 --%>
                        <tr>
                            <td colspan="2">
                                <div id="divQDROInformation" runat="server" >
                                <table class="Table_WithoutBorder" width="100%" border="0">
                                    <tr>
                                        <td align="left" class="td_Text">QDRO Beneficiaries Information
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                           <asp:label id="lbMgs" runat="server" cssclass="Label_Small">
												 </asp:label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            
                                            <div style="overflow: auto; width: 100%; border-top-style: none; border-right-style: none; border-left-style: none; height: 90px; border-bottom-style: none">
                                                <asp:datagrid id="DatagridQDROInformation" runat="server" cssclass="DataGrid_Grid" width="100%" autogeneratecolumns="False"
                                                    allowsorting="false">
                                                                    <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                    <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                    <Columns>
                                                                       <asp:BoundColumn DataField="FundNo" HeaderText="Fund No"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="FirstName" HeaderText="First Name"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="MiddleName" HeaderText="Middle Name"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="LastName" HeaderText="Last Name"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="QDROType" HeaderText="Type "></asp:BoundColumn>
                                                                         <%--<asp:BoundColumn DataField="DollarAmount" HeaderText="Dollar Amount" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>--%> <%-- Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Changed alignment--%>
                                                                        <%--<asp:BoundColumn DataField="Percentage" HeaderText="Percentage" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>--%>  <%--Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Changed alignment--%>
                                                                       <asp:BoundColumn DataField="Both" HeaderText="Both" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Retirement" HeaderText="Retirement" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Savings" HeaderText="Savings" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                                                         <asp:BoundColumn DataField="RequestDate" HeaderText="Request Date"></asp:BoundColumn>
                                                                        <%--<asp:BoundColumn DataField="PlanType" HeaderText="Plan Type"></asp:BoundColumn>--%>
                                                                        <asp:BoundColumn DataField="Spouse" HeaderText="Spouse"></asp:BoundColumn> <%-- Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | Added column for spouse--%>
                                                                    </Columns>
                                                                    <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                                </asp:datagrid>
                                            </div>

                                        </td>

                                    </tr>
                                     <tr>
                                     
                                    </tr>
                                </table>
                                    </div>
                            </td>
                        </tr>
                        <tr>
                              <td class="Td_ButtonContainer" align="right" width="100%" colspan="2">
                                   <asp:literal id="Literal2" runat="server" visible="false"></asp:literal>
                                   <asp:button id="btnClose" runat="server" cssclass="Button_Normal" text="Close" causesvalidation="false" width="73px"> <%-- Manthan Rajguru | 2016.08.30 | YRS-AT-2488 | setting validation to false--%>

                                   </asp:button> 
                              </td>
                        </tr>
                    </table>
                        <%-- END : Chandrasekar - 2016.07.05 - YRS-AT-2481 --%>
                </div>
            </td>
        </tr>
    </table>
    <asp:placeholder id="PlaceHolderAddEditQDRO" runat="server"></asp:placeholder>     <%-- Chandrasekar - 2016.07.05 - YRS-AT-2481 --%>
</form>
<!--#include virtual="bottom.html"-->
