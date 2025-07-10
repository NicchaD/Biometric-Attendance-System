<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RefundRequestForm.aspx.vb" Inherits="YMCAUI.RefundRequestForm" %>

<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl_new.ascx" %>
<!--#include virtual="top.html"-->
<script language="javascript">
    function OpenCloseWindow(IsWindowOpen) {
        var popupWin = new String();
        popupWin = window.open('UpdateAddressDetails.aspx', 'CustomPopUp', 'width=700, height=600, status=Yes,menubar=no, Resizable=no,top=50,left=120, scrollbars=yes');
        popupWin.close();
    }
    //START |PK | 08/30/2019 | YRS-AT-2670 | Function to bind modal popup.
    function BindEvents() {
        $('#ConfirmDialog').dialog({
            autoOpen: false,
            draggable: true,
            close: false,
            modal: true,
            width: 450, maxHeight: 350,
            height: 150,
            title:"YMCA YRS",
            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });
    }

    function ShowDialog() {
        $('#ConfirmDialog').dialog("open");
        return false;
    }
    function closeDialog(id) {
        $('#' + id).dialog('close');
    }
    //END |PK | 08/30/2019 | YRS-AT-2670 | Function to bind modal popup.
</script>
<form id="Form1" method="post" runat="server">  
    <table class="Table_WithoutBorder" cellspacing="0" width="698">
        <tr>
            <td class="td_backgroundcolorwhite" colspan="2"></td>
        </tr>
        <tr>
            <td class="td_backgroundcolorwhite" colspan="2"></td>
        </tr>
        <tr>
            <td class="Td_BackGroundColorMenu" align="left">
                <cc1:Menu ID="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
                    CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover"
                    DefaultMouseUpCssClass="mouseup" MenuFadeDelay="2" mouseovercssclass="MouseOver">
                    <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
                </cc1:Menu>
            </td>
        </tr>
    </table>
    <div class="Div_Center">
        <table width="698">
            <!--<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
					<asp:label id="LabelHeader" runat="server" cssclass="Td_HeadingFormContainer">Withdrawal Request Maintenance</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
			</tr>-->


            <tr>
                <td class="Td_HeadingFormContainer" align="left">
                    <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
                </td>
            </tr>

            <%-- Start: Bala: 01/19/2019: YRS-AT-2398: Alert message--%>
            <tr>
                <td bgcolor="red" colspan="2" style="text-align: left;">
                    <asp:label id="LabelSpecialHandling" forecolor="White" runat="server" font-size="12px" font-bold="True"></asp:label>
                    <a id="LinkButtonSpecialHandling" style="color: white; font-size: 9px; cursor: pointer;" onclick="showToolTip();" onmouseout="javascript: document.getElementById('lblComments').innerHTML = ''; hideToolTip();">[view details]</a>
                    <asp:hiddenfield id="HiddenFieldOfficersDetails" runat="server" />
                </td>
            </tr>
            <%-- End: Bala: 01/19/2019: YRS-AT-2398: Alert message--%>


            <!--<tr>
				<td class="td_Text_Small" align="left" height="18">&nbsp;&nbsp;&nbsp;
					<asp:label id="LabelTitle" runat="server" cssClass="td_Text_Small" Width="592px">:</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				</td>
			</tr>-->
			<%--START : VC | 2018.11.22 | YRS-AT-4018 | Created div to show error message--%>
             <tr>
                <td>
                    <div id="DivErrorMessage" class="error-msg" runat="server" style="text-align: left;" enableviewstate="false">
                    </div>
                </td>
             </tr>
			<%--END : VC | 2018.11.22 | YRS-AT-4018 | Created div to show error message--%>
            <tr>
                <td>
                    <iewc:TabStrip ID="RefundTabStrip" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
                        TabHoverStyle="background-color:#93BEEE;color:#4172A9;text-align:center;" TabSelectedStyle="background-color:#93BEEE;color:#000000;text-align:center;"
                        Height="30px" Width="697px">
                        <iewc:Tab Text="General"></iewc:Tab>
                        <iewc:Tab Text="Employment"></iewc:Tab>
                        <iewc:Tab Text="AccountContributions"></iewc:Tab>
                        <iewc:Tab Text="Requests"></iewc:Tab>
                        <iewc:Tab Text="Notes"></iewc:Tab>
                    </iewc:TabStrip>
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
        </table>
        <%-- Start: Bala: 01/19/2019: YRS-AT-2398: Officers detail in tooltip--%>
        <div id="Tooltip" runat="server" style="z-index: 1000; width: auto; border-left: 1px solid silver; border-top: 1px solid silver; border-right: 2px solid black; border-bottom: 1px solid #cccccc; padding: 3px; position: absolute; top: 0; left: 0; background: LightYellow; color: black; display: none; /* does not work in ie6 */	font-size: 7pt; font-family: verdana; margin: 0; overflow: visible; text-align: left;">
            <asp:label runat="server" id="lblComments" style="display: block; width: auto; overflow: visible; font-size: x-small;"></asp:label>
        </div>
        <script type="text/javascript">
            if ($("#LabelSpecialHandling").text() == '') {
                $('#LinkButtonSpecialHandling').hide();
            }
        </script>
        <%-- End: Bala: 01/19/2019: YRS-AT-2398: Officers detail in tooltip--%>
        <iewc:MultiPage ID="RefundMultiPage" runat="server">
            <iewc:PageView>
                <table width="100%" class="Table_WithBorder">
                    <tr>
                        <td colspan="2" align="left" class="td_Text">&nbsp;General
                        </td>
                    </tr>
                    <tr valign="top">
                        <td align="left" width="300">
                            <table width="100%" align="left">
                                <tr>

                                    <td align="left">
                                        <asp:label id="LabelSSNo" runat="server" cssclass="Label_Small">
															SS No.</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxSSNo" runat="server" width="70px" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelFundNo" runat="server" cssclass="Label_Small">
															Fund No.</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxFundNo" runat="server" width="70px" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelSal" runat="server" cssclass="Label_Small" width="30">
															Sal</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:dropdownlist id="DropDownListSal" runat="server" width="50" cssclass="DropDown_Normal">
											<asp:ListItem Value=" "></asp:ListItem>
											<asp:ListItem Value="Dr.">Dr.</asp:ListItem>
											<asp:ListItem Value="Mr.">Mr.</asp:ListItem>
											<asp:ListItem Value="Mrs.">Mrs.</asp:ListItem>
											<asp:ListItem Value="Ms.">Ms.</asp:ListItem>
										</asp:dropdownlist>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelFirst" runat="server" cssclass="Label_Small" width="30">
															First</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxFirst" runat="server" width="130px" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelMiddle" runat="server" cssclass="Label_Small" width="30">
															Middle</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxMiddle" runat="server" width="130px" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelLast" runat="server" cssclass="Label_Small" width="30">
															Last</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxLast" runat="server" width="130px" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelSuffix" runat="server" cssclass="Label_Small" width="30">
															Suffix</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxSuffix" runat="server" width="130px" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelMaritalStatus" runat="server" cssclass="Label_Small">
													Marital Status</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxMaritalStatus" runat="server" width="130px" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelAge" runat="server" cssclass="Label_Small">
													Age</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxAge" runat="server" width="60px" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelVested1" runat="server" cssclass="Label_Small">
													Vested</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxVested1" runat="server" width="60px" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelTerminated" runat="server" cssclass="Label_Small">
													Terminated</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxTerminated" runat="server" width="60px" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>

                                </tr>
                            </table>
                        </td>
                        <td align="left">
                            <table align="left" width="100%">
                                <tr valign="top">
                                    <td colspan="2" align="left">
                                        <table width="100%">
                                            <tr valign="top">
                                                <td align="left" width="380">
                                                    <NewYRSControls:New_AddressWebUserControl runat="server" PopupHeight="930" ID="AddressWebUserControl1" AllowNote="true" AllowEffDate="true" />
                                                </td>
                                                <td align="right">
                                                    <asp:button class="Button_Normal" id="ButtonUpdateAddress" width="75" runat="server" text="Address" causesvalidation="False"></asp:button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <%--<tr>
									    <td align="right">
										    <asp:label id="LabelAddress1" runat="server" CssClass="Label_Small" width="80">
															    Address1</asp:label></td>
									    <td colSpan="4">
										    <asp:textbox id="TextBoxAddress1" runat="server" Width="300px" CssClass="TextBox_Normal"></asp:textbox></td>
								    </tr>
								    <tr>
									    <td align="right">
										    <asp:label id="LabelAddress2" runat="server" CssClass="Label_Small" width="80">
															    Address2</asp:label></td>
									    <td colSpan="4">
										    <asp:textbox id="TextBoxAddress2" runat="server" Width="300px" CssClass="TextBox_Normal"></asp:textbox></td>
								    </tr>
								    <tr>
									    <td align="right">
										    <asp:label id="LabelAddress3" runat="server" CssClass="Label_Small" width="80">
															    Address3</asp:label></td>
									    <td colSpan="4">
										    <asp:textbox id="TextBoxAddress3" runat="server" Width="300px" CssClass="TextBox_Normal"></asp:textbox>
									    </td>
								    </tr>
								    <tr>
									    <td align="right">
										    <asp:label id="LabelCity" runat="server" CssClass="Label_Small" width="76">
															    City</asp:label></td>
									    <td colSpan="2">
										    <asp:textbox id="TextBoxCity" runat="server" Width="140px" CssClass="TextBox_Normal"></asp:textbox></td>
									    <td align="right">
										    <asp:label id="LabelZip" runat="server" CssClass="Label_Small" width="80">
															    Zip</asp:label></td>
									    <td align="left">
										    <asp:textbox id="TextBoxZip" runat="server" Width="100px" CssClass="TextBox_Normal"></asp:textbox></td>
								    </tr>
								    <tr>
									    <td align="right">
										    <asp:label id="LabelState" runat="server" CssClass="Label_Small" width="80">State</asp:label></td>
									    <td colSpan="4" align="left">
										    <asp:textbox id="TextBoxState" runat="server" Width="180px" CssClass="TextBox_Normal"></asp:textbox></td>
								    </tr>
								    <tr>
									    <td align="right">
										    <asp:label id="LabelCountry" runat="server" CssClass="Label_Small" width="80">
															    Country</asp:label></td>
									    <td colSpan="4" align="left">
										    <asp:textbox id="TextBoxCountry" runat="server" Width="180px" CssClass="TextBox_Normal"></asp:textbox></td>
								    </tr>--%>
                                <tr>
                                    <td align="left" width="120">
                                        <asp:label id="LabelTelephone" runat="server" cssclass="Label_Small" width="80">
															Telephone</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxTelephone" runat="server" width="180" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelEmail" runat="server" width="80" cssclass="Label_Small">
															Email Addr.</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxEmail" runat="server" width="180" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelUpdatedDate" runat="server" cssclass="Label_Small">
															Updated Date</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxUpdatedDate" runat="server" width="140px" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelUpdatedBy" runat="server" cssclass="Label_Small" width="80">
															Updated By</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxUpdatedBy" runat="server" width="140px" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                </table>
            </iewc:PageView>
            <iewc:PageView>
                <table width="695" class="Table_WithBorder">
                    <tr>
                        <td align="left" class="td_Text" colspan="4">&nbsp;Employment
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                    <tr>
                        <td class="Td_InLineBorder" align="left" height="0"></td>
                    </tr>
                    <tr>
                        <td>
                            <table align="center" width="60%">
                                <tr>
                                    <td align="right">
                                        <asp:label id="LabelVested" runat="server" cssclass="Label_Small">Vested</asp:label>
                                        <asp:textbox id="TextBoxVested" runat="server" cssclass="TextBox_Normal" width="50"></asp:textbox>
                                    </td>
                                    <td align="right">
                                        <asp:label id="LabelYear" runat="server" cssclass="Label_Small">Service  :  Year</asp:label>
                                        <asp:textbox id="TextBoxYear" runat="server" cssclass="TextBox_Normal" width="50"></asp:textbox>
                                    </td>
                                    <td align="right">
                                        <asp:label id="LabelMonth" runat="server" cssclass="Label_Small">Month</asp:label>
                                        <asp:textbox id="TextBoxMonth" runat="server" cssclass="TextBox_Normal" width="50"></asp:textbox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Td_InLineBorder" align="left" height="0"></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <div style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 200px">
                                <asp:datagrid id="DataGridEmployment" runat="server" width="680" cssclass="DataGrid_Grid">
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
								</asp:datagrid>
                            </div>
                        </td>
                    </tr>
                </table>
            </iewc:PageView>
            <iewc:PageView>
                <table width="695" class="Table_WithBorder">
                    <tr>
                        <td align="left" class="td_Text" colspan="4">&nbsp;Account Contributions
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="Td_InLineBorder" align="left" height="0"></td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table width="690">
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelPIATermination" runat="server" cssclass="Label_Small" align="left" width="200"
                                            nowrap>YMCA (Legacy) Acct at Termination</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxPIATermination" runat="server" cssclass="TextBox_Normal" align="left"
                                            width="100" nowrap></asp:textbox>
                                    </td>
                                    <td align="left">
                                        <asp:label id="LabelCurrentPIA" runat="server" cssclass="Label_Small" align="left" width="200"
                                            nowrap>Current YMCA (Legacy) Acct</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxCurrentPIA" runat="server" cssclass="TextBox_Normal" align="left" width="100"
                                            nowrap></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <!--<asp:label id="LabelBATermination" runat="server" CssClass="Label_Small">BA-ER Acct at Termination</asp:label>-->
                                    </td>
                                    <td align="left">
                                        <!--<asp:textbox id="TextBoxBATermination" runat="server" CssClass="TextBox_Normal"></asp:textbox>-->
                                    </td>
                                    <td align="left">
                                        <asp:label id="LabelBACurrentPIA" runat="server" cssclass="Label_Small">Current YMCA Acct</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxBACurrent" runat="server" cssclass="TextBox_Normal" align="left" width="100"
                                            nowrap></asp:textbox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Td_InLineBorder" align="left" height="0"></td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="td_Text_Small" colspan="4">&nbsp;Funded Contributions
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="Td_SubText" colspan="4">&nbsp;Retirement Plan
                        </td>
                    </tr>
                    <!-- <tr>
									<td>
										<asp:label id="LabelEmployee" runat="server" CssClass="Label_Small">---------------Employee---------------</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:label id="LabelYMCA" runat="server" CssClass="Label_Small">---------------YMCA---------------</asp:label></td>
								</tr> -->
                    <tr>
                        <td align="right">
                            <div style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 150px">
                                <asp:datagrid id="DataGridFundedAccountContributions" runat="server" width="680" cssclass="DataGrid_Grid">
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
								</asp:datagrid>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="Td_SubText" colspan="4">&nbsp;Savings Plan
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <div style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 150px">
                                <asp:datagrid id="DataGridFundedAccntContributionsSavingsPlan" runat="server" width="680" cssclass="DataGrid_Grid">
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
								</asp:datagrid>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="td_Text_Small" colspan="4">&nbsp; Non - Funded Contributions
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="Td_SubText" colspan="4">&nbsp; Retirement Plan
                        </td>
                    </tr>
                    <!-- <tr>
									<td>
										<asp:label id="LabelNonFundedEmployee" runat="server" CssClass="Label_Small">---------------Employee---------------</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:label id="LabelNonFundedYMCA" runat="server" CssClass="Label_Small">---------------YMCA---------------</asp:label></td>
								</tr> -->
                    <tr>
                        <td align="right">
                            <div style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 150px">
                                <asp:datagrid id="DataGridNonFundedAccountContributions" runat="server" width="680" cssclass="DataGrid_Grid">
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
								</asp:datagrid>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="Td_SubText" colspan="4">&nbsp; Savings Plan
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <div style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 150px">
                                <asp:datagrid id="DatagridNonFundedAcctContributionSavingsPlan" runat="server" width="680" cssclass="DataGrid_Grid">
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
								</asp:datagrid>
                            </div>
                        </td>
                    </tr>
                </table>
            </iewc:PageView>
            <iewc:PageView>
                <table width="700" class="Table_WithBorder">
                    <tr>
                        <td align="left" class="td_Text" colspan="4">&nbsp;Requests
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Td_ButtonSubContainer">
                            <asp:button class="Button_Normal" id="ButtonAddItem" width="100" runat="server" text="Add..." causesvalidation="False"></asp:button>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 250px">
                                <!-- Modified the width of the columns as per requested Amit 24/12/2009-->
                                <asp:datagrid id="DataGridRequests" runat="server" cssclass="DataGrid_Grid" autogeneratecolumns="false">
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
									<Columns>
										<asp:TemplateColumn>
											<ItemTemplate>
												<asp:ImageButton id="ImagebuttonProcess" runat="server" ImageUrl="images\process.gif" CausesValidation="False" 
													CommandName="ProcessSelect" ToolTip="Process a Refund"></asp:ImageButton>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn>
											<ItemTemplate>
												<asp:ImageButton id="Imagebutton2" runat="server" ImageUrl="images\cancel.gif" CausesValidation="False"
													CommandName="CancelSelect" ToolTip="Cancel a Refund"></asp:ImageButton>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:boundcolumn headertext="Request Date" datafield="RequestDate" />
										<asp:boundcolumn headertext="Status" datafield="RequestStatus" />
										<asp:boundcolumn headertext="Status Date" datafield="StatusDate" />
										<asp:boundcolumn headertext="Expire Date" datafield="ExpireDate" />
										<asp:boundcolumn headertext="Request Type" datafield="RefundType" />
										<asp:boundcolumn headertext="Plan" datafield="PlanType" />
										
										<asp:boundcolumn headertext="Gross Amt."  datafield="Gross Amt." DataFormatString="{0:N}" ><HeaderStyle Width="80px"></HeaderStyle></asp:boundcolumn>
										<asp:boundcolumn headertext="Market Based Amt." datafield="MarketAmount" DataFormatString="{0:N}" ><HeaderStyle Width="80px"></HeaderStyle></asp:boundcolumn>
										<asp:boundcolumn headertext="Net Amt. Excl. Deductions" datafield="Net Amt. Excl. Deductions" DataFormatString="{0:N}" ><HeaderStyle Width="80px"></HeaderStyle></asp:boundcolumn>
										<asp:boundcolumn headertext="P.Fee"  datafield="Processing Fee" DataFormatString="{0:N}" ><HeaderStyle Width="80px"></HeaderStyle></asp:boundcolumn> <%-- Manthan Rajguru | 2016.06.10 | YRS-AT-2962| Add Processing Fee column --%>
										
                                     <asp:boundcolumn headertext="Creator" datafield="Creator" />                                      
                                        <asp:boundcolumn  datafield="IsCovid" visible="false" />   <%--Suvarna B | 2020.05.05 | YRS-AT 4854 | Added backcolor to row for highlighting covid request --%>                                      
                                        <%-- Start - chandrasekar.c:2016.01.11:YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request --%>
                                        <asp:TemplateColumn>
											<ItemTemplate>
										  <asp:LinkButton ID="LinkButtonExtendExpireDate" CommandName="ExtendExpireSelect" runat="server" Visible='<%# IIf(Convert.ToString(Eval("IsRefRequestExtendExpire")) = "1", "True", "False")%>' CssClass="linkbutton" Text="Extend" />
											</ItemTemplate><HeaderStyle Width="80px"></HeaderStyle>
										</asp:TemplateColumn>
                                         <%-- End - chandrasekar.c:2016.01.11:YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request --%> 
									</Columns>
								</asp:datagrid>
                                <!-- Modified the width of the columns as per requested Amit 24/12/2009-->
                            </div>
                        </td>
                    </tr>
                    <%--START : Suvarna B. : 2020.05.05 : YRS-AT-4854 : Added legend for highlight the covid request--%>
                    <tr>
                        <td  style="text-align: left" class="Label_Small">
                            <span class="BG_ColourIsCovid">&nbsp;&nbsp;</span> - Covid Request.
                        </td>
                    </tr>
                    <%--ENDs : Suvarna B. : 2020.05.05 : YRS-AT-4854 : Added legend for highlight the covid request--%>
                </table>
            </iewc:PageView>
            <iewc:PageView>
                <table width="695" class="Table_WithBorder">
                    <tr>
                        <td align="left" class="td_Text" colspan="4">&nbsp;Notes
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Td_ButtonSubContainer">
                            <asp:button id="ButtonAddNote" runat="server" width="110px" text="Add..." cssclass="Button_Normal" causesvalidation="False"></asp:button>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 150px">
                                <asp:datagrid id="DataGridRefundNotes" runat="server" width="670px" cssclass="DataGrid_Grid" autogeneratecolumns="false">
									<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
									<columns>
										<asp:TemplateColumn>
											<ItemTemplate>
												<asp:ImageButton id="Imagebutton1" runat="server" ImageUrl="images\view.gif" CausesValidation="False"
													CommandName="Select" ToolTip=" View Notes "></asp:ImageButton>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:boundcolumn headertext="Notes" datafield="Note" />
										<asp:boundcolumn headertext="Date" datafield="Date" />
										<asp:boundcolumn headertext="Creator" datafield="Creator" />
										<asp:boundcolumn headertext="UniqueId" datafield="UniqueID" visible="false" />
										<asp:TemplateColumn HeaderText="Marked As Important">
											<ItemTemplate>
												<asp:CheckBox ID="CheckBoxImportant" Runat=server AutoPostBack=false Enabled=False Checked='<%# Databinder.Eval(Container.DataItem, "bitImportant") %>'>
												</asp:CheckBox>
											</ItemTemplate>
										</asp:TemplateColumn>
									</columns>
								</asp:datagrid>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </iewc:PageView>
        </iewc:MultiPage>
        <table>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>
        <table class="Table_WithoutBorder" cellspacing="0" width="698">
            <tr>
                <td class="Td_ButtonContainer" align="right">
                    <asp:button id="ButtonOk" runat="server" cssclass="Button_Normal" width="80" text="Close" causesvalidation="False"></asp:button>
                </td>
            </tr>
        </table>
        <input id="NotesFlag" type="hidden" name="NotesFlag" runat="server">
    </div>
    <asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder>

    <%--START |PK | 08/30/2019 | YRS-AT-2670 | Added control to display Peurto rico warning message--%>
    <div id="ConfirmDialog" title="YMCA YRS" style="display: none;">

        <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;">
            <tr>
                <td rowspan="2" style="width: 10%;">
                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="imgConfirmDialogInfo" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divConfirmDialogMessage">
                        Consult with Finance about Hacienda withholding and special federal withholding. Data correction may be required.
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr id="trConfirmDialogOK">
                <td align="right" valign="bottom" colspan="2">
                    <asp:button runat="server" id="btnConfirmDialogOK" text="OK" cssclass="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" />
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <%--END |PK | 08/30/2019 | YRS-AT-2670 | Added control to display Peurto rico warning message--%>
</form>
<!--#include virtual="bottom.html"-->
<script type="text/javascript">
    //Start: Bala: 01/19/2019: YRS-AT-2398: Show details of officer.
    function showToolTip() {
        var elementRef = document.getElementById("Tooltip");
        if (elementRef != null) {
            elementRef.style.position = 'absolute';
            elementRef.style.left = event.clientX + 5 + document.body.scrollLeft;
            elementRef.style.top = event.clientY + 5 + document.body.scrollTop;
            elementRef.style.width = '570';
            elementRef.style.visibility = 'visible';
            elementRef.style.display = 'block';
        }
        var lblNote = document.getElementById("lblComments");

        if (lblNote.innerText == '') {
            lblNote.innerText = $('#HiddenFieldOfficersDetails').val();
            elementRef.style.left = event.clientX + 20 + document.body.scrollTop;
            elementRef.style.width = '300';
        } else {
            lblNote.innerText = ''
            hideToolTip();
        }
    }
    //to hide tool tip when mouse is removed
    function hideToolTip() {
        var elementRef = document.getElementById("Tooltip");
        if (elementRef != null) {
            elementRef.style.visibility = 'hidden';
        }
    }
    //End: Bala: 01/19/2019: YRS-AT-2398: Show details of officer.
</script>
