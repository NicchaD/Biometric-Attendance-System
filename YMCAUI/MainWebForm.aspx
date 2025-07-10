<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master" CodeBehind="MainWebForm.aspx.vb" Inherits="YMCAUI.MainWebForm" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .Label
        {
            color: Black;
            font-family: Verdana, Tahoma;
            font-size: 10pt;
            font-weight: bold;
            height: 20px;
            text-align: left;
        }
        /*** Hyperlink Styles ***/
        .Hyperlink
        {
            color: #3B5386;
            font-family: Verdana, Tahoma;
            font-size: 8pt;
            font-weight: bold;
            height: 20px;
            padding-top: 6;
            padding-bottom: 5;
            text-align: left;
        }
        .LinkButton
        {
            color: Gray;
            font-family: Verdana, Tahoma;
            font-size:smaller;
            font-weight: normal;
            height: 20px;
            padding-top: 6;
            padding-bottom: 5;
            text-align: left;
            text-decoration: none;
        }
        .LinkButton:hover
        {
            text-decoration: underline;
            color: #3B5386;
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">    
    <div class="Div_Center" style="height:550;">
        <table width="100%" class="Table_WithBorder" style="height:550;">
            <tr valign="top">
                            <td align="left">
                                &nbsp;
                            </td>
                            <td>
                                <div style="width: 100%; overflow: auto; height: 100%">
                                    <asp:Repeater ID="rptMenuHistory" runat="server" Visible="false" OnItemCommand="LinkButton_Command">
                                        <HeaderTemplate>
                                            <p class="Label">
                                                Your Quick links - top 5 frequently accessed menu links</p><ul>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                        <li style="padding-bottom:10px;">
                                            <asp:HyperLink runat="server" ID="lnkPageName" Text='<%#Eval("chvHyperLinkName")%>'
                                                NavigateUrl='<%#Eval("chvPageNameAndPath")%>' CssClass="Hyperlink">
                                            </asp:HyperLink></li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul><asp:LinkButton runat="server" CssClass="LinkButton" ID="linkClearHistory" CommandName="ClearHistory"
                                                Text="Clear History"></asp:LinkButton>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </td>
                        </tr>
        </table>				
    </div>
    <asp:PlaceHolder ID="PlaceHolderMessage" runat="server"></asp:PlaceHolder>    
</asp:Content>