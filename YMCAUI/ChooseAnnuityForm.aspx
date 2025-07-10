<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ChooseAnnuityForm.aspx.vb" Inherits="YMCAUI.ChooseAnnuityForm"%>
<!--#include virtual="topnew.htm"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" colspan="2" align="left"><img title="Image" height="10" alt="Image" src="images/spacer.gif" width="10"/>
					Annuity Purchase</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
			</tr>
		</table>
	</div>
	<div class="Div_Center">
		<table width="700" class="Table_WithBorder" style="min-height: 600px;">
			<tr>
				<td colspan="2">
					<table width="100%">
						<tr>
							<td align="left" width="50%"><asp:label id="LabelSelectAnnuity" runat="server" CssClass="Label_Large" >Select an Annuity Option</asp:label></td>
							<%--<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>--%>
							<td align="left" width="50%"><asp:label id="LabelAnnuityDescription" runat="server" CssClass="Label_Large" >Annuity Description</asp:label></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td valign="top">				
					
                    <asp:Label id="lblGridMessage" Visible="false" runat="server" CssClass="Label_Small">Calculation of Nearest Age</asp:Label>
						<asp:DataGrid ID="DataGridAnnuityOptions" runat="server" width="310"  CssClass="DataGrids_Grid">
							<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
							<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
							<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
							<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
							<Columns>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:ImageButton id="ImgBtnSelectAnnuityType" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
											CommandName="Select" ToolTip="Select"></asp:ImageButton>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid>
                       
					 <%--   <br />
                        <br />--%>
					
                    <asp:Label id="lblExactAgeGridMessage" Visible="false" runat="server" CssClass="Label_Small">Calculation of Exact Age</asp:Label>
						<asp:DataGrid ID="DataGridAnnuityOptionsExactAgeEffDate" Runat="server" Width="310" 
                            CssClass="DataGrid_Grid">
							<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
							<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
							<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
							<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
							<Columns>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:ImageButton id="ImgBtnNewSelectAnnuityType" runat="server" 
                                            ImageUrl="images\select.gif" CausesValidation="False"
											CommandName="Select" ToolTip="Select"></asp:ImageButton>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid>
                       					
				</td>
				<td valign="top"><asp:textbox id="TextBoxAnnuityDescription" runat="server" CssClass="TextBox_Normal" Width="330px"></asp:textbox></td>
			</tr>
            <tr>
              <td align="left" colspan='2'  >                  
              <asp:Label ID="LabelJAnnuityUnAvailMessage" runat="server" visible="false" CssClass="Label_Small">*N/A = Due to the age difference between you and the Survivor you have selected, this option is not available.</asp:Label>
               </td>
               </tr>
			<tr>
				<td valign="bottom" colspan='2'>
					<table width="700" class="Table_WithOutBorder" cellspacing="0" border="0">
						<tr>							
							<td class="Td_ButtonContainer" colspan='2' align="right"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="87px" Text="OK"></asp:button>
							&nbsp;<asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="87" Text="Cancel"></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
</form>
<!--#include virtual="bottom.html"-->
