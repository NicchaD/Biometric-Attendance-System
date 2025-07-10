<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DeleteBeneficiary.aspx.vb"
    Inherits="YMCAUI.DeleteBeneficiary" %>

<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_ToolBar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>YMCA YRS </title>
    <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    <script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="JS/YMCA_JScript.js" type="text/javascript"></script>
    <script language="javascript" src="JS/YMCA_JScript_Warn.js" type="text/javascript"></script>
    <link id="Link1" href="CSS/CustomStyleSheet.css" type="text/css" runat="server" rel="stylesheet" />
    <script language="javascript" type="text/javascript">
        function IsValidDate(sender, args) {
            fmt = "MM/DD/YYYY";
            if (fnvalidateGendate_tmp(args, fmt)) {
                args.IsValid = true;
            }
            else {
                args.IsValid = false;
            }
        }
        function fnvalidateGendate_tmp(value1, fmt) {
            switch (fmt) {
                case ("MM/DD/YYYY"):
                    //alert("Inside MMDDYYY");
                    for (q = 0; q < fnvalidateGendate_tmp.arguments.length - 1; q++) {
                        indatefieldtext = fnvalidateGendate_tmp.arguments[q];
                        indatefield = value1.Value;
                        if (indatefield.indexOf("-") != -1) {
                            var sdate = indatefield.split("-");
                        }
                        else {
                            var sdate = indatefield.split("/");
                        }
                        var cmpDate;
                        var chkDate = new Date(Date.parse(indatefield))

                        var cmpDate1 = (chkDate.getMonth() + 1) + "/" + (chkDate.getDate()) + "/" + (chkDate.getFullYear());
                        var cmpDate2 = (chkDate.getMonth() + 1) + "/" + (chkDate.getDate()) + "/" + (chkDate.getYear());

                        var indate2 = (Math.abs(sdate[0])) + "/" + (Math.abs(sdate[1])) + "/" + (Math.abs(sdate[2]));

                        var num = sdate[2];
                        var num1 = num + "8";

                        var num2 = num1.length;
                        if (num2 == 3) {
                            cmpDate = cmpDate2;
                        }
                        if (num2 == 5) {
                            cmpDate = cmpDate1;
                        }
                        if (indate2 != cmpDate) {
                            //alert("before invalid");
                            //alert("Invalid date or date format on field "+value1.id);
                            //indatefieldtext.focus();
                            return false;
                        }
                        else {
                            if (cmpDate == "NaN/NaN/NaN") {
                                //alert("before invalid1");
                                //alert("Invalid date or date format on field "+value1.id);
                                //indatefieldtext.focus();
                                return false;
                            }
                        }
                    }
                    return true;
                    break;


                case ("DD/MM/YYYY"):
                    //alert("Inside DDMMYYYY");
                    for (q = 0; q < fnvalidateGendate_tmp.arguments.length - 1; q++) {
                        indatefieldtext = fnvalidateGendate_tmp.arguments[q];
                        indatefield = value1.Value;
                        if (indatefield.indexOf("-") != -1) {
                            var sdate = indatefield.split("-");
                        }
                        else {
                            var sdate = indatefield.split("/");
                        }

                        var cmpDate;
                        indatefield = (Math.abs(sdate[1])) + "/" + (Math.abs(sdate[0])) + "/" + (Math.abs(sdate[2]));
                        var chkDate = new Date(Date.parse(indatefield))

                        var cmpDate1 = (chkDate.getDate()) + "/" + (chkDate.getMonth() + 1) + "/" + (chkDate.getFullYear());
                        var cmpDate2 = (chkDate.getDate()) + "/" + (chkDate.getMonth() + 1) + "/" + (chkDate.getYear());
                        var indate2 = (Math.abs(sdate[0])) + "/" + (Math.abs(sdate[1])) + "/" + (Math.abs(sdate[2]));


                        //alert(indate2)
                        //alert(cmpDate2)
                        var num = sdate[2];
                        var num1 = num + "8";

                        var num2 = num1.length;
                        if (num2 == 3) {
                            cmpDate = cmpDate2;
                        }
                        if (num2 == 5) {
                            cmpDate = cmpDate1;
                        }

                        if (indate2 != cmpDate) {

                            //alert("Invalid date or date format on field " + value1.id);
                            //indatefieldtext.focus();
                            return false;
                        }
                        else {
                            if (cmpDate == "NaN/NaN/NaN") {

                                //alert("Invalid date or date format on field "+value1.id);
                                //indatefieldtext.focus();
                                return false;
                            }
                        }
                    }
                    return true;
                    break;

            }
        }


        //Dinesh.K :2013.08.16 - YRS 5.0-1698:Cross check SSN when entering a date of death
        $(document).ready(function () {
            $("#ConfirmDialog").dialog
					({
					    modal: true,
					    open: function (event, ui) { $(this).parent().appendTo("form"); $(this).parent('div').find('button:contains("SAVE")').focus(); },
					    autoOpen: false,
					    title: "YMCA YRS - Information",
                        width: 500, height: 170
					});
        });

	function CloseWSMessageDeathNotify() {
            $(document).ready(function () {
                $("#ConfirmDialog").dialog('close');
            });
        }
        function openDialogDeathNotify(str) {
            $(document).ready(function () {
                $('#lblMessage1').html(str)
                $("#ConfirmDialog").dialog('open');
                return false;
            });
        }
        //END Dinesh.K :2013.08.16 - YRS 5.0-1698:Cross check SSN when entering a date of death
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="Div_Center">
        <table border="0" cellpadding="0" cellspacing="0" width="700">
            <tr>
                <td class="Td_HeadingFormContainer" align="left">
                    <YRSControls:YMCA_ToolBar_WebUserControl ID="Toolbar_Control" runat="server" ShowLogoutLinkButton="false"
                        ShowHomeLinkButton="false" ShowReleaseLinkButton="false" />
                    <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="Table_WithBorder" id="Table1" cellpadding="1" width="100%" align="left"
                        cellspacing="0" border="0">
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary runat="server" ValidationGroup="a" ID="valSummary" class="Label_Small" />
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="Label_Small">
                                    Benficiary Name</label>
                            </td>
                            <td>
                                <asp:Label class="Label_Small" runat="server" ID="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="Label_Small">
                                    Reason</label>
                            </td>
                            <td>
                                <asp:DropDownList CssClass="DropDown_Normal Warn" runat="server" ID="ddlReason" ValidationGroup="a"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="rfvReason" ControlToValidate="ddlReason"
                                    Text="*" ErrorMessage="Please select reason." ValidationGroup="a" InitialValue="-- Select --"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr id="trDOD" runat="server" visible="false">
                            <td runat="server" id="tdDOD" visible="false">
                                <label class="Label_Small">
                                    Death Date</label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDelBenfDOD" CssClass="TextBox_Normal Warn" runat="server" Width="70"
                                    Visible="false" ValidationGroup="a">
                                </asp:TextBox>
                                <rjs:PopCalendar ID="calDeleteBenfDOD" runat="server" ScriptsValidators="IsValidDate"
                                    Visible="false" Format="mm dd yyyy" Control="txtDelBenfDOD" Separator="/"></rjs:PopCalendar>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtDelBenfDOD"
                                    Text="*" ErrorMessage="Please select Death Date." ValidationGroup="a"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="IsValidDate"
                                    ControlToValidate="txtDelBenfDOD" Display="Dynamic">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <label class="Label_Small">
                                    Comments</label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtComments" TextMode="MultiLine" Columns="23" Rows="5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="Label_Small">
                                    Important</label>
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkImp" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="Td_ButtonContainer" align="right">
                    <asp:Button runat="server" ID="btnSaveReason" Text="OK" Width="110" class="Button_Normal Warn_Dirty"
                        ValidationGroup="a" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button runat="server" ID="btnCancelReason" Text="Cancel" Width="110" class="Button_Normal Warn_Dirty" />
                </td>
            </tr>
        </table>
        <div id="ConfirmDialog" runat="server" style="display: none;">
            <table width="100%" border="0">
                <tr>
                    <td>
                        <div id="lblMessage1" style="color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt;width: 470; height: 120"
                            runat="server">
                            <br />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="bottom">
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <div style="border: 1px solid #aaaaaa/*{borderColorContent}*/; background: #ffffff/*{bgColorContent}*/ url(images/ui-bg_flat_75_ffffff_40x100.png)/*{bgImgUrlContent}*/ 50%/*{bgContentXPos}*/ 50%/*{bgContentYPos}*/ repeat-x/*{bgContentRepeat}*/;
                            color: #222222/*{fcContent}*/;">
                        </div>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button runat="server" ID="btnOK" Text="OK" CssClass="Button_Normal" Style="width: 80px;
                                            color: Black; font-family: Verdana, Tahoma, Arial; font-size: 10pt; font-weight: bold;
                                            height: 16pt;" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        <div id="divWSMessage" runat="server" style="display: none;">
            <table width="690px">
                <tr>
                    <td valign="top" align="left">
                        <span id="spntext"></span>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
