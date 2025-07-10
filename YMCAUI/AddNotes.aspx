<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AddNotes.aspx.vb" Inherits="YMCAUI.AddNotes" enableViewStateMac="True"%>
<!--#include virtual="TopNew.htm"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					<%--Added By Anudeep To show the Vienotes and Add notes --%>
                    <asp:label id="lblHeadtext" runat="server"></asp:label></td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="695">
			<tr>
				<td align="center" colSpan="2"><asp:textbox id="TextBoxNotes" runat="server" CssClass="TextBox_Normal" TextMode="MultiLine"
						Width="670px" Height="144px" rows="1" cols="1" MaxLength="1000"></asp:textbox><asp:label id="LabelNotes" runat="server"></asp:label></td>
			</tr>
			<tr>
				<td align="center" colSpan="2">
					<asp:Label id="LabelImportant" runat="server" CssClass="Label_Medium">Mark As Important</asp:Label>
					<asp:CheckBox id="CheckBoxImportant" runat="server"></asp:CheckBox>
				</td>
			</tr>
			<tr>
				<td class="Td_ButtonContainer" align="right"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="87" Text="Cancel"></asp:button></td>
				<td class="Td_ButtonContainer" align="left"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="87" Text="OK"></asp:button></td>
			</tr>
		</table>
	</div>
	<asp:PlaceHolder id="PlaceHolderMessage" runat="server"></asp:PlaceHolder>
</form>
<!--#include virtual="bottom.html"-->
