<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSPopUp.Master" CodeBehind="UpdateSSN.aspx.vb" Inherits="YMCAUI.UpdateSSN" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
	<link href="CSS/CustomStyleSheet.css" rel="stylesheet" type="text/css" />
    <%--Start - Manthan Rajguru | 2016.08.02 | YRS-AT-2382 |Commented existing function to close popup window as it is handled on server side --%>
	<%--<script language='JavaScript' type="text/javascript">		
	function closePopup() { window.close(); }</script>--%>
    <%--End - Manthan Rajguru | 2016.08.02 | YRS-AT-2382 |Commented existing function to close popup window as it is handled on server side --%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Maincontent" runat="server">
    <div class="div_center" style="width:100%; vertical-align:top;">
		<%--Start - Manthan Rajguru | 2016.07.29 | YRS-AT-2382 | Added Div to display error message--%>
        <div style="margin-top: 0px;">
            <asp:ValidationSummary ID="vdlSummary" runat="server" ValidationGroup="EditSSN" DisplayMode="List" ShowSummary="true"  CssClass="Error_Message" ShowMessageBox="false" />
        </div>
		<%--End - Manthan Rajguru | 2016.07.29 | YRS-AT-2382 | Added Div to display error message--%>
		<table class="Table_WithBorder" width="100%">
			<tr valign="top">
			<td colspan='3'>
				<asp:Panel runat="server" ID="pnlEditSSN" Visible="false">
					<table class="Table_WithoutBorder" width="100%">
						<tr>
							<td width="10%" nowrap="true">
								<label id="lblOldSSN" class="Label_Small">
									Old SSN</label>
							</td>
							<td class="Label_Small" width="2%">
								:
							</td>
							<td>
								<asp:Label ID="labelOldSSN" runat="server" CssClass="TextBox_Normal"></asp:Label>
							</td>
						</tr>
						<tr>
							<td width="10%" nowrap="true">
								<label id="lblNewSSN" class="Label_Small">
									New SSN</label>
							</td>
							<td class="Label_Small" width="3%">
								:
							</td>
							<td>
								<asp:TextBox ID="txtNewSSN" runat="server" CssClass="TextBox_Normal" MaxLength="9"></asp:TextBox>
								<asp:RequiredFieldValidator ID="rqvSSN" Text="*" runat="server" ErrorMessage="Please Enter New SSN."
									ValidationGroup="EditSSN" ControlToValidate="txtNewSSN"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareOldSSNvalidator" runat="server" ErrorMessage="New SSN matches old SSN. No update required." Text="*"
                                    Operator="NotEqual" ControlToValidate="txtNewSSN" Type="String" CssClass="Error_Message" ValidationGroup="EditSSN"></asp:CompareValidator> <%--Manthan Rajguru | 2016.07.29 | YRS-AT-2382 | Added compare validator--%>
							</td>
						</tr>
						<tr>
							<td width="15%">
								<label id="lblReason" class="Label_Small">
									Reason</label>
							</td>
							<td class="Label_Small" width="10%">
								:
							</td>
							<td>
								<asp:DropDownList ID="ddlReason" runat="server" CssClass="DropDown_Normal">
									<asp:ListItem Selected="True" Text="-Select-" Value="-Select-" />
								</asp:DropDownList>
								<asp:RequiredFieldValidator ID="rqvReason" runat="server" Text="*" ErrorMessage="Please Select Reason."
									InitialValue="-Select-" ValidationGroup="EditSSN" ControlToValidate="ddlReason"></asp:RequiredFieldValidator>
							</td>
						</tr>
					</table>
				</asp:Panel>
				</td>
			</tr>
			<tr>
				<td colspan='3'>
					<asp:Panel runat="server" ID="pnlViewSSN" Visible="false">
						<table border="0" cellpadding="0" cellspacing="0">
							<tr>
								<td>
									<div style="overflow: auto; width:510px; height: 280px;">
										<asp:GridView ID="gvSSNHistory" Width="500px" AutoGenerateColumns="false" CssClass="DataGrid_Grid" BackColor="White"
											BorderColor="#E7E7FF" RowStyle-Width="50px" BorderStyle="None" BorderWidth="2px"
											CellPadding="3" runat="server" EmptyDataText="No record(s) found.">
											<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
											<AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
											<RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
											<SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
											<Columns>
												<asp:BoundField HeaderText="Old SSN"	HeaderStyle-Width="50px" DataField="chvOldValue" />
												<asp:BoundField HeaderText="New SSN"	HeaderStyle-Width="50px" DataField="chvNewValue" />
												<asp:BoundField HeaderText="Reason"		HeaderStyle-Width="200px" DataField="chvDescription" />
												<asp:BoundField HeaderText="Updated By" HeaderStyle-Width="70px" DataField="chvCreator" />
												<asp:BoundField HeaderText="Updated On" HeaderStyle-Width="130px" DataField="dtmCreated" />
											</Columns>
										</asp:GridView>
									</div>
								</td>
							</tr>
						</table>
					</asp:Panel>
				</td>
			</tr>
			<tr>
				<td colspan='3'>
					<table class="Td_ButtonContainer" width="100%">
						<tr>
							<td align="right">
								<asp:Button ID="btnSSNOk" ValidationGroup="EditSSN" Text="OK" Width="70px" CssClass="Button_Normal"
									runat="server" />
							&nbsp;&nbsp;
								<%--Start - Manthan Rajguru | 2016.08.02 | YRS-AT-2382 |Commented existing function and removed fucntion called to close popup window --%>
                                <%--<asp:Button ID="btnSSNCancel" Text="Cancel" OnClientClick="javascript:closePopup()"--%>
									<asp:Button ID="btnSSNCancel" Text="Cancel" CssClass="Button_Normal" runat="server" />
								<%--End - Manthan Rajguru | 2016.08.02 | YRS-AT-2382 |Commented existing function and removed fucntion called to close popup window --%>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		
		
		
    </div>
	<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>    
</asp:Content>

