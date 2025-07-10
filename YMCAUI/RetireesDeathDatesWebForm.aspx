<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RetireesDeathDatesWebForm.aspx.vb"
    Inherits="YMCAUI.RetireesDeathDatesWebForm" %>

<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<!--#include virtual="TopNew.htm"-->
<head>
    <script type="text/javascript">

        function ShowForm() {
            var check = new Boolean();
            var checkbox = new Boolean();
            check = false;
            checkbox = false;
            //Checking whether any empty textboxes exist
            if ($('#tbForms tr').length == 0) {
                return true;
            }
            $('#tbForms :checked').each(function () {
                var str = new String();
                var addtext = new String();
                str = $(this).attr("id");
                str = str.substring(3, str.length);
                addtext = $(this).closest('tr').find('input:text').attr("value");
                check = true;
                if (addtext != undefined && addtext == '') {
                    showDialog('You have not mentioned any additional info. for one/more of the selected form/additional document entry, do you want to continue ?')
                    checkbox = false;
                    return false;
                    
                }
                  checkbox = true;
            });
              // if there is no empty textbox exists then continue else
              if (checkbox) {
                    saveForms();
              }
            //if user does selected any Additional forms
            if (!check) {
                showDialog('You have not selected any Additional Forms/Documents to be submitted by Beneficiary, do you want to continue ?')
                return false;
            }
            return checkbox;
        }
        //Confirmation dialog box 
        function showDialog(text) {
            $('#lblMessage').text(text);
            $('#ConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                width: 550, height: 150,
                title: "Death Notification",
                buttons: [{ text: "Yes", click: closeconfirmDialogyes }, { text: "No", click: closeconfirmDialog}],
                open: function (type, data) {

                    $('a.ui-dialog-titlebar-close').remove();
                }
            });


            $("#ConfirmDialog").dialog({ modal: true });
            $("#ConfirmDialog").dialog("open");

        }
        function closeconfirmDialog() {

            $("#ConfirmDialog").dialog('close');
            $("#ConfirmDialog").dialog('destroy');
            return false;
        }
        function closeconfirmDialogyes() {

            $("#ConfirmDialog").dialog('close');
            $("#ConfirmDialog").dialog('destroy');
            saveForms();


        }
        //save forms
        function saveForms() {
            var Formlist = new String();
            Formlist = "";
            $('#tbForms :checked').each(function () {
                var str = new String();
                var addtext = new String();
                str = $(this).attr("id");
                str = str.substring(3, str.length);
                check = true;
                addtext = $(this).closest('tr').find('input:text').attr("value");

                if (addtext != undefined && addtext != '') {
                    str = str + ',' + addtext
                }

                if (Formlist == "") {
                    Formlist = str;
                }
                else if (Formlist != "") {
                    Formlist = Formlist + "$$" + str;
                }
            });
            //calling showformclick webmethod from jquery 
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "RetireesDeathDatesWebForm.aspx/ShowFormclick",
                data: "{'Formlist':'" + Formlist + "'}",
                dataType: "json",
                success: function (record) {
                    //Anudeep:15.12.2012 Changed code to Click Show form and insert code
                    //document.forms(0).submit();
                    __doPostBack('<%=ButtonOk.ClientID %>', '');
                },
                failure: function (strReturnStatus) {
                    alert("Error Occured While Opening Report");
                    return false;
                }
            });
            $("#dvForm").dialog('close');
            $("#dvForm").dialog('destroy');

        }
    </script>
</head>
<form id="Form1" method="post" runat="server">
<div class="Div_Center">
    <table width="700" class="Table_withoutBorder" cellspacing="0" border="0">
        <tr>
            <td class="Td_HeadingFormContainer" align="left">
                <img title="image" height="10" alt="image" src="images/spacer.gif" width="10">
                Death Date Sheet
            </td>
        </tr>
    </table>
</div>
 <%-- START: SB: 07/31/2017: YRS-AT-3324: Notification error message --%>
 <div id="DivMainMessage" class="error-msg" runat="server" style="text-align: left;" enableviewstate="false" visible="false"></div>
 <%-- END: SB: 07/31/2017: YRS-AT-3324: Notification error message --%>

   <table width="700" class="Table_withoutBorder" cellspacing="0" border="0">
    <tr>
        <td align="left">
            <asp:validationsummary id="ValidationSummary1" runat="server" enableclientscript="True"
                cssclass="Error_Message"></asp:validationsummary>
        </td>
    </tr>
</table>
<div class="Div_Center">
    <table width="700" class="Table_WithBorder">
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr align="center">
            <td align="right">
                <asp:label id="LabelEnterDeathDate" runat="server" cssclass="Label_Small">Enter Death Date:</asp:label>
            </td>
            <td align="left">
                <uc1:DateUserControl ID="TextBoxDeathDate" runat="server"></uc1:DateUserControl>
            </td>
        </tr>
    </table>
    <%--<table width="100%" id="tbHeader" runat="server" class="Table_WithBorder" style="border-bottom: none;">
        <thead>
            <tr>
                <th class="Label_Small" colspan="2" align="left">
                    Death Benefit Application - Required Forms or Additional Documents&nbsp;
                    <img title="This information will be used while generating Death Benefit application form" class="img_help_small" src="images/help.jpg" />
                </th>
            </tr>
        </thead>
    </table>--%>
    <%--<table width="100%" id="tbForms" runat="server" class="Table_WithBorder" style="border-top: none;">
    </table>--%>
</div>
<div class="Div_Center">
    <table width="700" border="0" cellspacing="0">
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr align="center">
            <td align="right" class="Td_ButtonContainer">
                <asp:button id="ButtonOk" width="73px" runat="server" text="OK" cssclass="Button_Normal"
                    onclientclick=" if(!ShowForm()) return false;"></asp:button>
                <asp:button id="ButtonCancel" width="73px" runat="server" text="Cancel" cssclass="Button_Normal"
                    causesvalidation="False"></asp:button>
            </td>
        </tr>
    </table>
</div>
<div id="ConfirmDialog" title="DeathBenefit" style="overflow: visible;">
    <label id="lblMessage" class="Label_Small" runat="server">
    </label>
</div>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
