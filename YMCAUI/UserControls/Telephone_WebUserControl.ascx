<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Telephone_WebUserControl.ascx.vb"
    Inherits="YMCAUI.Telephone_WebUserControl" %>
<script language="javascript" src="JS/YMCA_JScript.js" type="text/javascript"></script>
<div class="Div_Center" style="vertical-align:top;">
    <table border="0" id="tbContacts" style="vertical-align:top;" runat="server" width="100%" class="Normaltext">
             <tr valign="top">
            <td colspan="2" align="left">
                <div runat="server" style="vertical-align:top;" id="dvContacts">
                </div>
                </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                <asp:Label ID="LabelNoContacts"  runat="server" CssClass="Normaltext"> No contact information defined. <br/>
                Please click on 'icon' to add or edit contact information.</asp:Label>
            </td>
        </tr>
    </table>
</div>
