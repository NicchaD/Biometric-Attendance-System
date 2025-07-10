<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SelectBank.aspx.vb" Inherits="YMCAUI.SelectBank" %>

<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_ToolBar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<title>YMCA YRS </title>
<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
<form id="Form1" method="post" runat="server">
<div class="Div_Center">
    <table width="700" cellspacing="0" border="0">
        <%--<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					&nbsp;Add/View Bank Information<asp:label id="LabelHdr" runat="server" CssClass="Td_HeadingFormContainer"></asp:label></td>
			</tr>--%>
        <tr>
            <td class="Td_HeadingFormContainer" align="left">
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
                <table class="Table_WithBorder" width="100%">
                    <tr>
                        <td align="left">
                            <div style="overflow: auto; width: 360px; border-top-style: none; border-right-style: none;
                                border-left-style: none; height: 200px; border-bottom-style: none">
                                <asp:datagrid id="DataGridSelectBank" runat="server" width="98%" cellpadding="1"
                                    cssclass="DataGrid_Grid" allowsorting="true">
							<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
							<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
							<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
							<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
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
                        <td align="left" valign="top">
                            <table>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelBankNo" runat="server" width="85px" cssclass="Label_Small">Bank No:</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxBankNo" runat="server" width="100" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:label id="LabelBankName" runat="server" width="85px" cssclass="Label_Small">Bank Name:</asp:label>
                                    </td>
                                    <td align="left">
                                        <asp:textbox id="TextBoxBankName" runat="server" width="100" cssclass="TextBox_Normal"></asp:textbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:button id="ButtonFind" cssclass="Button_Normal" width="80" runat="server" text="Find"></asp:button>
                                    </td>
                                    <td align="left">
                                        <asp:button id="ButtonClear" cssclass="Button_Normal" width="80" runat="server" text="Clear"></asp:button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Td_ButtonContainer">
                        </td>
                        <td class="Td_ButtonContainer" align="left">
                            <table cellspacing="0" border="0">
                                <tr>
                                    <td class="Td_ButtonContainer" align="left">
                                        <asp:button id="ButtonCancel" cssclass="Button_Normal" width="80" runat="server"
                                            text="Cancel"></asp:button>
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
<div class="Div_Center">
</div>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
