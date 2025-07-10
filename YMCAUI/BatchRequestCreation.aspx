<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BatchRequestCreation.aspx.vb"
    Inherits="YMCAUI.BatchRequestCreation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="JS/jquery-1.2.6.pack.js"></script>
    <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet" />
    <script language="javascript" src="JS/YMCA_JScript.js"></script>
    <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    <script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
    <script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <style type="text/css">
        .hide
        {
            display: none;
        }
        .show
        {
            display: block;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function windowrefreshPage(i, strQueryString, strBatchId, strModuleType) {
            if (strQueryString == "") {
                setTimeout(function () {
                    window.location.href = "BatchRequestCreation.aspx?count=" + i + "&submit=true";
                }, 1000);
            }
            else {
                setTimeout(function () {
                    window.location.href = "BatchRequestCreation.aspx?count=" + i + "&submit=true&Form=" + strQueryString + "&strBatchId=" + strBatchId + "&strModuleType=" + strModuleType;
                }, 1000);
            }
        }

        function openReportPrinter() {

            window.open('FT\\ReportPrinter.aspx', '', 'width=785,height=300, menubar=no, resizable=yes,top=200,left=150, scrollbars=yes, status=yes');
        }

        function FormSubmit(strQueryString, strBatchId, strModuleType) {

            if (strQueryString == "") {
                setTimeout(function () {
                    window.location.href = "BatchRequestCreation.aspx?submit=true";
                }, 1000);
            }
            else {
                setTimeout(function () {
                    window.location.href = "BatchRequestCreation.aspx?submit=true&Form=" + strQueryString + "&strBatchId=" + strBatchId + "&strModuleType=" + strModuleType;
                }, 1000);
            }
        }

        function ExceptionShowError() {
            $('#divErrorMsg').dialog({
                autoOpen: false,
                draggable: false,
                resizable: false,
                close: false,
                modal: true,
                width: 400, height: 250,
                title: "Error Message",
                open: function (type, data) {
                }
            });
            $('#divErrorMsg').css({ display: "block" });
            $('#divErrorMsg').dialog("open");
        }

        
    </script>
</head>
<body oncontextmenu="return false;">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellspacing="0" cellpadding="0" align="left" width="530px" class="Table_WithBorder">
            <tr>
                <td>
                    <div id="dvMsg" runat="server">
                    </div>
                    <%--<asp:Button ID="btnShowerroe" Text="Show Error" CssClass="Button_Normal" OnClientClick="ExceptionShowError()" />--%>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblMsg" Visible="false" class="Label_Small"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" align="center" width="99%" cssclass="DataGrid_Grid">
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center" class="Label_Small">
                                <u>Activities Performed</u>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td align="right" width="55%" class="DataGrid_HeaderStyle" style="text-align: center">
                                <u>Activities</u>
                            </td>
                            <td align="center" width="15%" class="DataGrid_HeaderStyle" style="text-align: left">
                                <u>Status</u>
                            </td>
                            <td align="center" width="30%" class="DataGrid_HeaderStyle" style="text-align: center">
                                <u>Progress</u>
                            </td>
                        </tr>
                        <tr>
                            <td class="DataGrid_AlternateStyle" valign="middle" style="vertical-align: middle; 
                                background-color: White" id="tdProcessCreation" runat="server" width="55%" >
                                
                            </td>
                            <td class="DataGrid_AlternateStyle" style="background-color: White" width="15%">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Image ID="imgRegComplete" runat="server" ImageUrl="~/images/complete.jpg" AlternateText="Complete"
                                                CssClass="hide" Width="20" Height="20" />
                                            <asp:Image ID="imgRegWaiting" runat="server" ImageUrl="~/images/waiting.gif" AlternateText="Please wait..."
                                                Visible="false" />
                                            <div id="dvReg" runat="server">
                                                In-Progress
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White" width="30%">
                                <div runat="server" id="dvReqProgress" align="center" style="vertical-align: middle">
                                    Inprocess
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" class="DataGrid_AlternateStyle" style="vertical-align: middle;
                                background-color: White">
                                2. Generate IDX document(s)
                            </td>
                            <td class="DataGrid_AlternateStyle" style="background-color: White">
                                <table>
                                    <tr>
                                        <td align="center">
                                            <asp:Image ID="imgIDXComplete" runat="server" ImageUrl="~/images/complete.jpg" AlternateText="Complete"
                                                CssClass="hide" Width="20" Height="20" />
                                            <asp:Image ID="imgIDXWaiting" runat="server" ImageUrl="~/images/waiting.gif" AlternateText="Please wait..."
                                                Visible="false" />
                                            <div id="dvIDX" runat="server">
                                                In-Progress
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White">
                                <div runat="server" id="dvIDXProgress" align="center" style="vertical-align: middle">
                                    Inprocess
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" class="DataGrid_AlternateStyle" style="vertical-align: middle;
                                background-color: White">
                                3. Generate PDF document(s)
                            </td>
                            <td class="DataGrid_AlternateStyle" style="background-color: White">
                                <table>
                                    <tr>
                                        <td align="center">
                                            <asp:Image ID="imgPDFComplete" runat="server" ImageUrl="~/images/complete.jpg" AlternateText="Complete"
                                                CssClass="hide" Width="20" Height="20" />
                                            <asp:Image ID="imgPDFWaiting" runat="server" ImageUrl="~/images/waiting.gif" AlternateText="Please wait..."
                                                Visible="false" />
                                            <div id="dvPDF" runat="server">
                                                In-Progress
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White">
                                <div runat="server" id="dvPDFProgress" align="center" style="vertical-align: middle">
                                    Inprocess
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" class="DataGrid_AlternateStyle" style="vertical-align: middle;
                                background-color: White">
                                4. Copy Generated IDX and PDF document(s) to IDM
                            </td>
                            <td class="DataGrid_AlternateStyle" style="background-color: White">
                                <table>
                                    <tr>
                                        <td align="center">
                                            <asp:Image ID="imgCopiedComplete" runat="server" ImageUrl="~/images/complete.jpg"
                                                AlternateText="Complete" CssClass="hide" Width="20" Height="20" />
                                            <asp:Image ID="imgCopiedWaiting" runat="server" ImageUrl="~/images/waiting.gif" AlternateText="Please wait..."
                                                Visible="false" />
                                            <div id="dvCopy" runat="server">
                                                Pending
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White">
                                <div runat="server" id="dvCopiedProgress" align="center" style="vertical-align: middle">
                                    Pending
                                </div>
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
            <tr id="trshowErrors" runat="server">
                <td>
                    <asp:Label ID="lblException" runat="server" Visible="false" class="Label_Small" Text="There were some error(s) while creating this batch request please click here to ">
                        <asp:LinkButton ID="lnkShowError" runat="server" OnClientClick="ExceptionShowError(); return false;">View Error(s)</asp:LinkButton></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none" id="divErrorMsg">
        <asp:GridView ID="gvBatchRequestError" runat="server" AutoGenerateColumns="false"
            Width="95%" CssClass="DataGrid_Grid" PageSize="15">
            <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
            <RowStyle CssClass="DataGrid_NormalStyle" />
            <HeaderStyle CssClass="DataGrid_HeaderStyle" />
            <Columns>
                <asp:BoundField DataField="FundNo" SortExpression="FundNo" HeaderText="Fund No."
                    ItemStyle-Width="10%" />
                <asp:BoundField DataField="Errors" SortExpression="Errors" HeaderText="Errors" ItemStyle-Width="20%" />
                <asp:BoundField DataField="Decription" SortExpression="Decription" HeaderText="Description"
                    ItemStyle-Width="60%" />
            </Columns>
            <PagerSettings Mode="NumericFirstLast" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>
