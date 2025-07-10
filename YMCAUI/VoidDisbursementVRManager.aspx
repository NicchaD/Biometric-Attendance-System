<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="VoidUserControl" Src="UserControls/VoidUserControl.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VoidDisbursementVRManager.aspx.vb"
    Inherits="YMCAUI.VoidDisbursementVRManager_1" %>

<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<!--#include virtual="top.html"-->
<script language="javascript">
    function _OnBlur_DropdownReason() {

        if (document.frmVRManager.all('TextBoxNotes').value == null) {
            document.frmVRManager.all('TextBoxNotes').value = document.frmVRManager.all('DropDownListReason').value;
        }
        else {
            document.frmVRManager.all('TextBoxNotes').value = document.frmVRManager.all('DropDownListReason').value;
        }

    }
</script>
<body>
    <table height="46" cellspacing="0" cellpadding="0" border="0">
        <form id="frmVRManager" method="post" runat="server">
        <tbody>
            <tr valign="top">
                <td>
                </td>
                <td>
                    <table class="Table_WithoutBorder" cellspacing="0" width="700">
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
                        <tr>
                            <td class="Td_HeadingFormContainer" align="left">
                                <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr valign="top">
                <td height="391">
                </td>
                <td>
                    <div class="center">
                        <table width="700">
                            <tbody>
                                <tr>
                                    <td>
                                        <iewc:TabStrip ID="TabStripVRManager" runat="server" Height="30px" Width="700px"
                                            TabSelectedStyle="background-color:#93BEEE;color:#000000;" TabHoverStyle="background-color:#93BEEE;color:#4172A9;"
                                            TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
                                            AutoPostBack="True" BorderStyle="None">
                                            <iewc:Tab Text="List" ID="tabList"></iewc:Tab>
                                            <iewc:Tab Text="Disbursements" ID="tabGeneral"></iewc:Tab>
                                        </iewc:TabStrip>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <iewc:MultiPage ID="MultiPageVRManager" runat="server">
                                            <iewc:PageView>
                                                <table class="Table_WithBorder" width="700">
                                                    <tr valign="top">
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr valign="top">
                                                        <!--Shashi Shekhar:2010-02-15:Added one col in DataGridVRManager IsArchived for data archive impact-->
                                                        <td valign="top">
                                                            <asp:Label ID="LabelRecordNotFound" runat="server" CssClass="Label_Small">No Matching Records</asp:Label>
                                                            <div style="overflow: auto; width: 400px; height: 200px; text-align: left">
                                                                <asp:DataGrid ID="DataGridVRManager" runat="server" Width="400" CssClass="DataGrid_Grid"
                                                                    AutoGenerateColumns="False" AllowSorting="true">
                                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                    <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                    <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                                    <Columns>
                                                                        <asp:TemplateColumn>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImageButtonSel" runat="server" ImageUrl="images\select.gif"
                                                                                    CausesValidation="False" CommandName="Select" ToolTip="Select"></asp:ImageButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="PersID" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="SSN" HeaderText="SSN" SortExpression="SSN"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="FirstName" HeaderText="First Name" SortExpression="FirstName">
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="LastName" HeaderText="Last Name" SortExpression="LastName">
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="MiddleName" HeaderText="Middle Name" SortExpression="MiddleName">
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="FundIDNo" HeaderText="FundID No" SortExpression="FundIDNo">
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="BirthDate" HeaderText="Birth Date" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="IsArchived" HeaderText="IsArchived" Visible="False">
                                                                        </asp:BoundColumn>
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LabelFundNo" runat="server" CssClass="Label_Small">Fund No.</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox Width="100" runat="server" ID="TextBoxFundNo" CssClass="TextBox_Normal"
                                                                            MaxLength="10"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LabelSSNo" runat="server" CssClass="Label_Small">SS No.</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox Width="100" runat="server" ID="TextBoxSSNo" CssClass="TextBox_Normal"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LabelLastName" runat="server" CssClass="Label_Small">Last Name</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox Width="100" runat="server" ID="TextBoxLastName" CssClass="TextBox_Normal"
                                                                            MaxLength="30"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LabelFirstName" runat="server" CssClass="Label_Small">First Name</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox Width="100" runat="server" ID="TextBoxFirstName" CssClass="TextBox_Normal"
                                                                            MaxLength="20"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LabelCheckNo" runat="server" CssClass="Label_Small">Check #</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox Width="100" runat="server" ID="TextBoxCheckNo" CssClass="TextBox_Normal"
                                                                            MaxLength="20"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Button Width="80" runat="server" ID="ButtonFind" Text="Find" CssClass="Button_Normal">
                                                                        </asp:Button>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:Button Width="80" runat="server" ID="ButtonClear" Text="Clear" CssClass="Button_Normal">
                                                                        </asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!--Added by imran on 30/10/2009 -->
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <img title="image" height="1" alt="image" src="images/spacer.gif" width="1">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <table class="Table_WithBorder" width="700">
                                                                <tr>
                                                                    <td class="Td_ButtonContainer" align="right">
                                                                        <asp:Button ID="ButtonCloseVR" CssClass="Button_Normal" runat="server" Width="80"
                                                                            Text="Close"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </iewc:PageView>
                                            <iewc:PageView>
                                                <table class="Table_WithBorder" border="0" width="700" cellpadding="0" cellspacing="1">
                                                    <tr valign="top">
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" valign="top">
                                                            <asp:Label ID="LabelPayeeSSN" runat="server" CssClass="Label_Small"></asp:Label>
                                                            <div style="overflow: auto; width: 688px; height: 250px; text-align: left; valign: top">
                                                                <uc1:VoidUserControl ID="VoidUserControl1" runat="server"></uc1:VoidUserControl>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <!--  Added by Imran    -->
                                                    <!--<tr runat="server" id="trDeduction">
																	<td colSpan="4">
																		<table width="690">
																			<TR>
																				<TD align="center"><asp:label id="LabelDeduction" CssClass="Label_Large" runat="server">Deductions</asp:label></TD>
																				<TD vAlign="middle" align="left"></TD>
																			</TR>
																			<tr>
																				<td align="center"><asp:datagrid id="DataGridDeductions" Runat="server" CssClass="DataGrid_Grid" Width="100">
																						<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																						<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																						<Columns>
																							<asp:TemplateColumn HeaderText="Selected">
																								<ItemTemplate>
																									<asp:CheckBox id="CheckBoxDeduction" runat="server" autoPostBack="true"></asp:CheckBox>
																								</ItemTemplate>
																							</asp:TemplateColumn>
																						</Columns>
																					</asp:datagrid></td>
																			</tr>
																		</table>
																	</td>
																</tr>-->
                                                    <tr>
                                                        <td colspan="4">
                                                            <table width="690">
                                                                <tr>
                                                                    <td rowspan="2" valign="top">
                                                                        <asp:Label ID="LabelAddressCheckSent" runat="server" CssClass="Label_Small">Address Check Sent:</asp:Label>
                                                                    </td>
                                                                    <td rowspan="2">
                                                                        <asp:TextBox ID="TextBoxAddress" CssClass="TextBox_Normal" Width="200" TextMode="MultiLine"
                                                                            Height="90" runat="server" ReadOnly="True"></asp:TextBox>
                                                                    </td>
                                                                    <td valign="top">
                                                                        <asp:Label ID="LabelAccNo" runat="server" CssClass="Label_Small">A/C Number</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TextBoxAccountNo" CssClass="TextBox_Normal" Width="200" runat="server"
                                                                            ReadOnly="True"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">
                                                                        <asp:Label ID="LabelBankInfo" runat="server" CssClass="Label_Small">Bank Info</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TextBoxBankInfo" CssClass="TextBox_Normal" Width="200" TextMode="MultiLine"
                                                                            Height="60" runat="server" ReadOnly="True"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">
                                                                        <asp:Label ID="LabelEntityType" runat="server" CssClass="Label_Small">Entity Type</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TextBoxEntityType" CssClass="TextBox_Normal" Width="200" runat="server"
                                                                            ReadOnly="True"></asp:TextBox>
                                                                    </td>
                                                                    <td rowspan="2" valign="top">
                                                                        <asp:Label ID="LabelEntityAddress" runat="server" CssClass="Label_Small">Entity Addr.</asp:Label>
                                                                    </td>
                                                                    <td rowspan="2">
                                                                        <asp:TextBox ID="TextBoxEntityAddress" CssClass="TextBox_Normal" Width="200" TextMode="MultiLine"
                                                                            Height="56" runat="server" ReadOnly="True"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">
                                                                        <asp:Label ID="LabelLegalEntity" runat="server" CssClass="Label_Small">Legal Entity</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TextBoxLegalEntity" CssClass="TextBox_Normal" Width="200" runat="server"
                                                                            ReadOnly="True"></asp:TextBox>
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
                                                        <td>
                                                            <img title="image" height="1" alt="image" src="images/spacer.gif" width="1">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <table class="Table_WithBorder" width="700">
                                                                <tr>
                                                                    <td align="left" class="Td_ButtonContainer" width="450px">
                                                                        <asp:Button ID="ButtonPHR" runat="server" Text="PHR" Width="70" CssClass="Button_Normal"
                                                                            Enabled="true"></asp:Button>
                                                                    </td>
                                                                    <td align="right" class="Td_ButtonContainer" width="70px">
                                                                        <asp:Button ID="ButtonSave" runat="server" Text="Save" Width="70" CssClass="Button_Normal"
                                                                            Enabled="True"></asp:Button>
                                                                        <input type="hidden" id="hdnDisbId" runat="server" name="hdnDisbId" />
                                                                        <input type="hidden" id="hdnListDisbId" runat="server" name="hdnListDisbId" />
                                                                    </td>
                                                                    <td class="Td_ButtonContainer" align="right" width="70px">
                                                                        <asp:Button ID="ButtonClose" CssClass="Button_Normal" runat="server" Width="80" Text="Close">
                                                                        </asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </iewc:PageView>
                                        </iewc:MultiPage>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <img title="image" height="1" alt="image" src="images/spacer.gif" width="1">
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td>
                                        <table class="Table_WithBorder" width="700">
                                            <tr>
                                                <td class="Td_ButtonContainer" align="right">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="litPersID" runat="server" Visible="False"></asp:Literal><asp:Literal
                                            ID="litDisbID" runat="server" Visible="False"></asp:Literal><asp:Literal ID="litFundID"
                                                runat="server" Visible="False"></asp:Literal><asp:Literal ID="litWHAmount" runat="server"
                                                    Visible="False"></asp:Literal><asp:Literal ID="litGross" runat="server" Visible="False"></asp:Literal><asp:Literal
                                                        ID="litPayeeId" runat="server" Visible="False"></asp:Literal><asp:Literal ID="litAddressID"
                                                            runat="server" Visible="False"></asp:Literal><asp:Literal ID="litDisbNbr" runat="server"
                                                                Visible="False"></asp:Literal><asp:PlaceHolder ID="PlaceHolderMessageBox" runat="server">
                                                                </asp:PlaceHolder>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </td>
            </tr>
        </tbody>
        </TD></TR></TD></form>
        </TD></TR></TD></TR>
        <!--#include virtual="bottom.html"-->
    </table>
    </TABLE>
</body>
</html>
