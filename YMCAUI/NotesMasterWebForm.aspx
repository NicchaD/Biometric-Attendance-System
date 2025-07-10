<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NotesMasterWebForm.aspx.vb" Inherits="YMCAUI.NotesMasterWebForm" MasterPageFile="~/MasterPages/YRSPopUp.Master" %>

<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>

<asp:Content ID="cntNotes" runat="server" ContentPlaceHolderID="Maincontent">
    <div class="Div_Center">
        <table width="700">
           
    </div>
    <div class="Div_Center">
        <table width="700" class="Table_WithBorder">
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <asp:TextBox ID="TextBoxNotes" runat="server" TextMode="MultiLine" Width="680" Height="100px"
                        MaxLength="800"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="LabelImportant" runat="server" CssClass="Label_Medium">Mark As Important</asp:Label>
                    <asp:CheckBox ID="CheckBoxImportant" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="Td_ButtonContainer" align="center">
                    <asp:Button ID="ButtonCancel" runat="server" CssClass="Button_Normal" Width="87" Text="Cancel"></asp:Button>
                    <asp:Button ID="ButtonOK" runat="server" CssClass="Button_Normal" Width="87" Text="OK"></asp:Button>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
