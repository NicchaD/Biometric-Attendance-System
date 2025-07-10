<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="YMCANetWebForm.aspx.vb" Inherits="YMCAUI.YMCAEmailWebForm"%>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<!--#include virtual="TopNew.htm"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table class="Table_WithoutBorder" width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
					Email Information
				</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<div class="center">
		<table class="Table_WithBorder" cellSpacing="0" cellPadding="0" width="700">
			<tr vAlign="top">
				<td vAlign="top">
					<DIV style="OVERFLOW: auto; WIDTH: 280px; HEIGHT: 200px; TEXT-ALIGN: left"><asp:datagrid id="DataGridYMCAEmail" runat="server" autoGenerateColumns="False" OnSortCommand="SortCommand_OnClick"
							allowsorting="True" width="260px" cellpadding="1" CssClass="DataGrid_Grid">
							<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
							<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
							<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
							<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="guiUniqueId"></asp:BoundColumn>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
											CommandName="Select" ToolTip="Select"></asp:ImageButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Email Address" SortExpression="Email Address" HeaderText="Email Address" />
							</Columns>
						</asp:datagrid></DIV>
					<asp:label id="LabelNoRecord" runat="server" Width="128px" Visible="False">No Record found</asp:label></td>
				<td>
					<table style="WIDTH: 383px; HEIGHT: 112px" width="383">
						<TBODY>
							<tr>
								<td>
									<table style="WIDTH: 383px; HEIGHT: 91px">
										<tr>
											<td>
                                                <asp:label id="LabelType" runat="server" Width="50px" cssclass="Label_Small">Type</asp:label>
                                            </td>
                                            <td align="left">
                                                <!--AA:22.10.2013-BT-2258:YRS 5.0-2230:YRS General Email does not update (save properly) -->
                                                <table align="left" width="100%">
                                                    <tr valign="top">
                                                        <td align="left" width="20%">
                                                            <asp:dropdownlist id="DropDownType" runat="server" CssClass="DropDown_Normal" Width="100" enabled="false"></asp:dropdownlist>
                                                        </td>
                                                        <td align="left" width="70%">
                                                        <asp:checkbox id="CheckBoxPrimary" CssClass="CheckBox_Normal" enabled="false" Runat="server"
													            Text="Make this primary" TextAlign="Left" autopostback="true"></asp:checkbox>
                                                        </td>
                                                        <td align="left" width="10%">
                                                             <asp:checkbox id="CheckBoxActive" CssClass="CheckBox_Normal" enabled="false" Runat="server"
													                Text="Active" TextAlign="Left"></asp:checkbox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
										</tr>
										<tr>
											<td>
                                                <asp:label id="LabelEffectiveDate" runat="server" Width="50px" cssclass="Label_Small">Effective Date:</asp:label>
                                            </td>
											<td >
                                                <uc1:dateusercontrol id="TextBoxEffectiveDate" runat="server"></uc1:dateusercontrol>
                                            </td>
										</tr>
										<tr>
											<td>
                                                    <asp:label id="LabelAddress" runat="server" Width="50px" cssclass="Label_Small">Address:</asp:label>
                                            </td>
											<td ><asp:textbox id="TextBoxAddress" runat="server" width="300px" CssClass="TextBox_Normal" MaxLength="70"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxAddress" ErrorMessage="Address cannot be blank">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxAddress"
													ErrorMessage="Email id is not valid" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:regularexpressionvalidator>
                                             </td>
										</tr>
									</table>
								</td>
							</tr>
						</TBODY>
                        </table>
					<asp:validationsummary id="ValidationSummary1" runat="server"></asp:validationsummary></td>
			</tr>
		</table>
	</div>
	<div class="center">
		<table width="700">
			<tr>
				<td class="Td_ButtonContainer">
					<table width="270">
						<tr>
							<td class="Td_ButtonContainer"></td>
						</tr>
					</table>
				</td>
				<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonSave" CssClass="Button_Normal" Width="80" enabled="false" Runat="server"
						Text="Save"></asp:button></td>
				<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonCancel" CssClass="Button_Normal" Width="80" enabled="false" Runat="server"
						Text="Cancel" CausesValidation="False"></asp:button></td>
				<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonAdd" CssClass="Button_Normal" Width="80" Runat="server" Text="Add" CausesValidation="False"></asp:button></td>
				<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonOk" CssClass="Button_Normal" Width="80" Runat="server" Text="OK" CausesValidation="False"></asp:button></td>
			</tr>
		</table>
	</div>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder></form>
<!--#include virtual="bottom.html"-->
