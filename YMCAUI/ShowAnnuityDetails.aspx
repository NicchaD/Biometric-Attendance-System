<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowAnnuityDetails.aspx.vb"
    Inherits="YMCAUI.ShowAnnuityDetails" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_ToolBar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<title>YMCA YRS -</title>
<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
<body>
    <form id="Form1" method="post" runat="server">
    <div class="Div_Center">
        <table class="Table_WithoutBorder" cellspacing="0" width="100%">
            <%--<tr>
                <td class="Td_BackGroundColorMenu" align="left">
                    <cc1:Menu ID="MenuRetireesInformation" runat="server" Layout="Horizontal" HighlightTopMenu="True"
                        ItemPadding="4" ItemSpacing="0" Cursor="Pointer" Font-Names="Verdana" Width="700"
                        CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown"
                        DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup" zIndex="100"
                        MenuFadeDelay="1" mouseovercssclass="MouseOver">
                        <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
                    </cc1:Menu>
                </td>
            </tr>--%>
            <tr>
                <td class="Td_HeadingFormContainer" align="left" colspan="2">
                    <YRSControls:YMCA_ToolBar_WebUserControl ID="Toolbar_Control" runat="server" ShowLogoutLinkButton="false"
                        ShowHomeLinkButton="false" />
                    <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <table id="TableContainer" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <iewc:TabStrip ID="TabStripRetireesInformation" runat="server" Height="30px" Width="100%"
                                    TabSelectedStyle="background-color:#93BEEE;color:#000000;" TabHoverStyle="background-color:#93BEEE;color:#4172A9;"
                                    TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
                                    AutoPostBack="True" BorderStyle="None">
                                    <iewc:Tab Text="Annuity Details" ID="TabStripTabGeneral"></iewc:Tab>
                                    <iewc:Tab Text="Current Values" ID="TabStripTabAnnuities"></iewc:Tab>
                                    <iewc:Tab Text="Adjustments" ID="TabStripTabBeneficiaries"></iewc:Tab>
                                </iewc:TabStrip>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <iewc:MultiPage ID="MultiPageRetireesInformation" runat="server">
                                    <iewc:PageView>
                                        <!-- General PageView Start -->
                                        <table class="Table_WithBorder" width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td align="left" class="td_Text">
                                                    Annuity Details
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="Table_WithBorder">
                                                        <tr>
                                                            <td>
                                                                <table border="0" width="100%" cellspacing="0" class="Table_WithOutBorder" align="left">
                                                                    <tr>
                                                                        <td align="left" width="120">
                                                                            <asp:Label ID="AnnuityType" runat="server" CssClass="Label_Small">Annuity Type : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblAnnuityType" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="AnnuitySource" runat="server" CssClass="Label_Small">Annuity Source : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblAnnuitySource" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" width="120">
                                                                            <asp:Label ID="PensionerType" runat="server" CssClass="Label_Small">Pensioner Type : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblPensionerType" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="PurchaseDate" runat="server" CssClass="Label_Small">Purchase Date : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblPurchaseDate" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" width="120">
                                                                            <asp:Label ID="TaxClass" runat="server" CssClass="Label_Small">Tax Class : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblTaxClass" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="TerminationDate" runat="server" CssClass="Label_Small">Termination Date : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblTerminationDate" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" width="120">
                                                                            <asp:Label ID="DefaultTaxStatus" runat="server" CssClass="Label_Small">Default Tax Status : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblDefaultTaxStatus" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="TerminationReason" runat="server" CssClass="Label_Small">Termination Reason : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblTerminationReason" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" width="120">
                                                                            <asp:Label ID="PersonalPreTaxReserves" runat="server" CssClass="Label_Small">Personal Pre-Tax Reserves : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblPersonalPreTaxReserves" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="PopUpValue" runat="server" CssClass="Label_Small">Pop-Up Value : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblPopUpValue" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" width="120">
                                                                            <asp:Label ID="PersonalPostTaxReserves" runat="server" CssClass="Label_Small">Personal Post Tax Reserves : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblPersonalPostTaxReserves" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" width="120">
                                                                            <asp:Label ID="YMCAReserves" runat="server" CssClass="Label_Small">YMCA Reserves : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblYMCAReserves" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="DeathBenefit" runat="server" CssClass="Label_Small">Death Benefit : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblDeathBenefit" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="SocialSecurityLeveling" runat="server" CssClass="Label_Small">Social Security Leveling : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblSocialSecurityLeveling" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="SocialSecurityReduction" runat="server" CssClass="Label_Small">Social Security Reduction : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblSocialSecurityReduction" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="ReductionEffectiveDate" runat="server" CssClass="Label_Small">Reduction Effective Date : </asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblReductionEffectiveDate" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:CheckBox ID="chkSuppressPayroll" runat="server" CssClass="CheckBox_Normal">
                                                                            </asp:CheckBox>
                                                                            <asp:Label ID="Label5" runat="server" CssClass="Label_Small">Suppressed</asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </iewc:PageView>
                                    <iewc:PageView>
                                        <div class="Div_center">
                                            <table width="700" class="Table_WithBorder" cellspacing="0">
                                                <tr>
                                                    <td colspan="4" align="left" class="td_Text">
                                                        Current Values
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="Label1" runat="server" CssClass="Label_Small">Current Payment </asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="Label2" runat="server" CssClass="Label_Small">Remaining Reserves</asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="Label3" runat="server" CssClass="Label_Small">--------------------</asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="Label4" runat="server" CssClass="Label_Small">-----------------------</asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="PersonalPreTax" runat="server" CssClass="Label_Small">Personal Pre-Tax : </asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblPersonalPreTax" runat="server" CssClass="Label_Small"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="PersonalPreTaxRem" runat="server" CssClass="Label_Small">Personal Pre-Tax : </asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblPersonalPreTaxRem" runat="server" CssClass="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="PersonalPostTax" runat="server" CssClass="Label_Small">Personal Post-Tax : </asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblPersonalPostTax" runat="server" CssClass="Label_Small"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="PersonalPostTaxRes" runat="server" CssClass="Label_Small">Personal Post-Tax : </asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblPersonalPostTaxRes" runat="server" CssClass="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="YMCA" runat="server" CssClass="Label_Small">YMCA : </asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblYMCA" runat="server" CssClass="Label_Small"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="YMCARes" runat="server" CssClass="Label_Small">YMCA : </asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblYMCARes" runat="server" CssClass="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="TotalPayment" runat="server" CssClass="Label_Small">Total Payment : </asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblTotalPayment" runat="server" CssClass="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="SocialSecurity" runat="server" CssClass="Label_Small">Social Security : </asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblSocialSecurity" runat="server" CssClass="Label_Small"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="DeathBenefitRemaining" runat="server" CssClass="Label_Small">Death Benefit Remaining : </asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblDeathBenefitRemaining" runat="server" CssClass="Label_Small"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </iewc:PageView>
                                    <iewc:PageView>
                                        <div class="Div_Left">
                                            <table width="700" class="Table_WithBorder" cellspacing="0">
                                                <tr>
                                                    <td align="left" class="td_Text">
                                                        Adjustments
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr valign="top">
                                                    <td align="center" class="Table_WithOutBorder">
                                                        <div>
                                                            <asp:DataGrid Width="600" ID="DataGridAdjustments" runat="server" CssClass="DataGrid_Grid"
                                                                AutoGenerateColumns="false">
                                                                <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                <Columns>
                                                                    <asp:BoundColumn HeaderText="Adjustment Type" DataField="Adjustment Type" />
                                                                    <asp:BoundColumn HeaderText="Effective Date" DataField="Effective Date" />
                                                                    <asp:BoundColumn HeaderText="Basis Code" DataField="Basis Code" Visible="False" />
                                                                    <asp:BoundColumn HeaderText="Basis Value" DataField="Basis Value" Visible="False" />
                                                                    <asp:BoundColumn HeaderText="Pers Pre Tax Adjustment" DataField="PersPreTaxAdjustment" />
                                                                    <asp:BoundColumn HeaderText="Pers Post Tax Adjustment" DataField="PersPostTaxAdjustment" />
                                                                    <asp:BoundColumn HeaderText="YMCA Adjustment" DataField="YMCA Adjustment" />
                                                                    <asp:BoundColumn HeaderText="Current Payment" DataField="CurrentPayment" />
                                                                    <asp:BoundColumn HeaderText="Pers Pre Tax Payment" DataField="PersPreTaxPayment" />
                                                                    <asp:BoundColumn HeaderText="Pers Post Tax Payment" DataField="PersPostTaxPayment" />
                                                                    <asp:BoundColumn HeaderText="YMCA Payment" DataField="YMCA Payment" />
                                                                    <asp:BoundColumn HeaderText="Social Security Payment" DataField="SocSecPayment" />
                                                                </Columns>
                                                            </asp:DataGrid>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </iewc:PageView>
                                </iewc:MultiPage>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="Table_WithBorder" id="Table6" cellspacing="1" cellpadding="1" width="100%">
                        <tr>
                            <td class="Td_ButtonContainer" align="center" height="10">
                                <asp:Button ID="ButtonClose" runat="server" CssClass="Button_Normal" Width="100PX"
                                    Text="Close"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div class="Div_Left">
    </div>
    <!--#include virtual="bottom.html"-->
    </form>
</body>
