<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" MasterPageFile="~/MasterPages/YRSLogin.Master"  Inherits="YMCAUI.Login" %>

<asp:Content ID="content1" ContentPlaceHolderID="MainContent" runat="server" >
<table width="100%" class="Table_WithOutBorder" height="100%" border="0" >
    <tr>
        <td valign="top">
            <div class="Div_Center">
                <asp:label runat="server" id="lblMsg"></asp:label>
                <table class="Table_WithoutBorder" width="100%" border="0">
                    <tr>
                        <td align="left">
                           &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <img src="images/spacer.gif" width="350" height="1" alt="image">
                        </td>
                    </tr>
                    <tr>
                        <td class="Label_Small" style="text-align:justify; line-height : 15px; letter-spacing:0.5px;padding-top:15px">
                            <span style="font-weight:lighter">This system is for the use of YMCA Retirement Fund authorized users only. Individuals using this computer system are subject to having all of their activities on this system monitored, recorded, copied, audited and inspected by YMCA Retirement Fund system software and authorized YMCA Retirement Fund personnel, to the maximum extent permitted by law. Anyone using this system expressly consents to such monitoring, recording, copying, auditing and inspection. In addition, anyone using this system is advised that if such monitoring, recording, copying, auditing or inspection reveals possible evidence of a violation of any YMCA Retirement Fund policy or criminal activity, authorized YMCA Retirement Fund personnel may provide the evidence to supervisory personnel and law enforcement officials and anyone using this system expressly consents to such sharing of such information.</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </td>
        <td>
            <img src="images/spacer.gif" width="1" height="1" alt="image">
        </td>
        <td valign="top" width="40%" align="right">            
            <table width="100%" align="right" border="0" >
                <tr>
                     <td align="right" colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td  align="right" width="30%" >
                        &nbsp;
                    </td>
                    <td  align="right" width="70%">
                        <table width="100%" align="right" cellspacing="0">
                            <tr>
                                <td align="left" width="100%">
                                    <img src="images/login2.gif" width="200" height="24" alt="image">
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" width="100%" align="left">
                                    <table  class="Table_Login">
                                        <tr>
                                           <td colspan="3">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="left" width="90" >
                                                <img src="images/spacer.gif" width="1" height="1" alt="image">
                                                <asp:label id="LabelUserId" runat="server" cssclass="Label_Small">Username</asp:label>
                                            </td>
                                            
                                            <td align="left" >
                                    
                                                <asp:textbox id="TextBoxUserID" runat="server" ClientIDMode="Static" cssclass="TextBox_Normal" width="120"
                                                     maxlength="20" ></asp:textbox>
                                                </td>
                                                <td width="50"></td>
                                
                                        </tr>
                                        <tr valign="top">
                                            <td align="left" >
                                                <img src="images/spacer.gif" width="1" height="1" alt="image">
                                                <asp:label id="LabelPassword" runat="server" cssclass="Label_Small">Password</asp:label>
                                            </td>
                                            
                                            <td align="left" width="120">
                                    
                                                <asp:textbox id="TextBoxPassword" runat="server" ClientIDMode="Static" textmode="Password" cssclass="TextBox_Normal"
                                                    width="120" maxlength="30" ></asp:textbox>
                                             </td>
                                             <td></td>
                                
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            
                                            <td valign="top" align="left">
                                                <table>
                                                    <tr>
                                                        <td >
                                                            <%--<asp:imagebutton id="OKImageButton" runat="server" imageurl="images/submit.gif" bordercolor="White"
                                                                alternatetext="Submit" ></asp:imagebutton>--%>
                                                            <asp:button id="OKButton" runat="server" text="Login" CssClass="Button_Normal PreventDoubleClick" ></asp:button> <%--ML | 2019.04.16 | YRS-AT-4388 | PreventDoubleClick cssClass added to prevent double click at login button--%>
                                                        </td>
                                                        <td >
                                                            <%--<asp:imagebutton id="ResetImageButton" runat="server" CausesValidation="false" imageurl="images/reset.gif"
                                                                bordercolor="White" alternatetext="Reset"></asp:imagebutton>--%>
                                                            <asp:button id="ResetButton" runat="server" CausesValidation="false" text="Reset" CssClass="Button_Normal"></asp:button>
                                                
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>                
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            &nbsp;
        </td>
    </tr>   
</table>
<asp:placeholder id="PlaceHolderLogin" runat="server"></asp:placeholder>
<%--<asp:CustomValidator ID="vldtrCustom" Display="None" runat="server"></asp:CustomValidator>--%>
</asp:Content>

