<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StatusPageForm.aspx.vb" Inherits="YMCAUI.StatusPageForm" MasterPageFile="~/MasterPages/YRSMain.Master" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" ID="StatusPageMainContent" runat="server">
    <div class="Div_Center">
       <table class="Table_WithBorder" width="100%" border="0" style="vertical-align: top; height: 450px">
            <tr style="vertical-align: top;">
                <td align="left">
                    <asp:Label ID="LabelProcessStatus" runat="server" Visible="False" CssClass="Label_Medium"></asp:Label></td>
            </tr>

            <tr style="vertical-align: top;">
                <td>
                    <asp:TextBox ID="TextBoxProcessStatus" runat="server" Width="100%" Visible="False" TextMode="MultiLine"
                        ReadOnly="True" BackColor="#FFFFC0" Height="150px"></asp:TextBox>
                </td>
                <td>&nbsp;
                </td>
            </tr>
            <tr class="td_buttoncontainer">
                <td colspan="2" style="text-align: center;">
                    <asp:Button ID="ButtonHome" runat="server" CssClass="Button_Normal" Width="64px" Text="Home"
                        Visible="False"></asp:Button>

                    <asp:Button ID="ButtonClose" runat="server" Text="Close" CssClass="Button_Normal" Width="64px"
                        Visible="False"></asp:Button>
                </td>

            </tr>
        </table>
    </div>

</asp:Content>
