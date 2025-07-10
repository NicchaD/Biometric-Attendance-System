<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ErrorPageForm.aspx.vb" Inherits="YMCAUI.ErrorPageForm" MasterPageFile="~/MasterPages/YRSMain.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="ErrorPageContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<div class="Div_Center">

		<table style="width:100%;height:350px;text-align:center;border:none;vertical-align:top;">			
			<tr>
				<td colspan="2" style="text-align:center; vertical-align:top;height:20px">
                    <asp:label id="LabelDBError" runat="server" Visible="False" cssClass="Label_Medium"></asp:label>
				</td>
			</tr>			
			<tr>
				<td colspan="2" style="text-align:center;height:20px;vertical-align:top;">
					<asp:LinkButton id="LinkButtonErrorDetails" runat="server" Width="162px" Font-Size="XX-Small" Font-Names="Arial">See Details</asp:LinkButton>&nbsp;&nbsp;&nbsp;<asp:LinkButton id="LinkbuttonHideDetails" runat="server" Width="181px" Font-Size="XX-Small" CausesValidation="False"
						Font-Names="Arial">Hide Details</asp:LinkButton>
				</td>
			</tr>
			<tr>
				<td colspan="2" style="text-align:center;vertical-align:top;">
					<asp:TextBox id="TextBoxErrorMessage" runat="server" Width="980px" TextMode="MultiLine" ReadOnly="True" BackColor="#FFFFC0" Height="250px"></asp:TextBox>
				</td>				
			</tr>
			<tr>
				<td colspan="2">
					&nbsp;
				</td>
			</tr>		
            <tr>
                <td align="center" class="Td_ButtonContainer" colspan="2">
                    <asp:Button id="ButtonHome" runat="server" CssClass="Button_Normal" Width="64px" Text="Home" Visible="False"></asp:Button>
		            <asp:Button id="ButtonClose" runat="server" Text="Close" CssClass="Button_Normal" Width="64px" Visible="False"></asp:Button>
                </td>
            </tr>
		</table>		
	</div>
</asp:Content>