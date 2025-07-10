<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UpdateNotes.aspx.vb" Inherits="YMCAUI.UpdateNotes" %>

<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<!--#include virtual="TopNew.htm"-->
<script language="JavaScript">
    function funcOnChangeText() {

        document.Form1.ButtonOK.disabled = "";
        return true;


    }
</script>
<form id="Form1" method="post" runat="server">
<table width="700" class="Table_WithBorder" cellspacing="0" border="0">
    <tr>
        <td class="Td_HeadingFormContainer" align="left">
            <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
            <asp:textbox id="txtNotes" runat="server" height="150px" width="424px" textmode="MultiLine"
                maxlength="1000"></asp:textbox>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:label id="LabelImportant" runat="server" cssclass="Label_Medium">Mark As Important</asp:label>
            <asp:checkbox id="CheckBoxImportant" runat="server"></asp:checkbox>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="Td_ButtonContainer" align="right">
            <%--Start: 27/12/2015: YRS-AT-1718: Ok button changed to Save --%>
            <%--<asp:button id="ButtonSave" runat="server" width="73px" text="Save" cssclass="Button_Normal"></asp:button>--%>
            <asp:button id="ButtonSave" runat="server" width="73px" text="Save" cssclass="Button_Normal"></asp:button>
            <%--End: 27/12/2015: YRS-AT-1718: Ok button changed to Save --%>
            <asp:button id="ButtonCancel" runat="server" width="73px" text="Cancel" cssclass="Button_Normal"></asp:button>
        </td>
    </tr>
</table>
<asp:placeholder id="PlaceHolderMessage" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
