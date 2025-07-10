<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master" CodeBehind="~/ReverseFeedImport.aspx.vb" Inherits="YMCAUI.ReverseFeedImport" EnableEventValidation="false" %>

<asp:Content ID="contentHeader" ContentPlaceHolderID="head" runat="server">

    <script src="JS/jquery-ui/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />

    <%--   <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    <link href="CSS/CustomStyleSheet.css" rel="stylesheet" type="text/css" />--%>

    <style type="text/css">
        .txtsearch {
            width: 120px;
        }

        .tabSelected {
            font-family: verdana;
            text-align: center;
            font-weight: bold;
            color: #000000;
            background-color: #93BEEE;
            border-bottom: none;
            font-size: 10pt;
            width: 100%;
            height: 25px;
            text-decoration: none;
        }

        .tabNotSelected {
            background-color: #4172A9;
            font-family: verdana;
            font-weight: bold;
            font-size: 10pt;
            color: #ffffff;
            text-align: center;
            border: solid 1px White;
            border-bottom: none;
            width: 100%;
            height: 25px;
            text-decoration: none;
        }

        .tabNotSelectedLink {
            font-family: verdana;
            font-weight: bold;
            font-size: 10pt;
            color: #ffffff;
            width: 100%;
        }

        .tabSelectedLink {
            font-family: verdana;
            font-weight: bold;
            font-size: 10pt;
            color: #000000;
            width: 100%;
            background-color: #93BEEE;
        }

        .tabUnfreezeLink {
            font-family: verdana;
            font-weight: bold;
            font-size: 10pt;
            color: #B90000;
            width: 100%;
        }

        .tabUnfreeze {
            font-family: verdana;
            text-align: center;
            font-weight: bold;
            color: #B90000;
            background-color: #93BEEE;
            border-bottom: none;
            font-size: 10pt;
            width: 100%;
            height: 25px;
            text-decoration: none;
        }

        .hide {
            display: none;
        }

        .show {
            display: block;
        }
     
        .BG_Colour_Loan_Report {
            background-color: #f1995a;
        }
        
        .alignright {
           text-align:right ;
       }
        
    </style>
    <script type="text/javascript" language="javascript">
       
      
        function BindEvents() {           
         
            $('#divShowReverseFeedDifference').dialog({
                autoOpen: false,
                resizable: false,
                dialogClass: 'no-close',
                draggable: true,
                width: 450, minHeight: 420,
                height: 350,
                closeOnEscape: false,
                title: "Differences",
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $(this).closest('.ui-dialog').children('.ui-dialog-content').css('height', '355px');
                }
            });

            $('#divErrorWarning').dialog({
                autoOpen: false,
                resizable: false,
                dialogClass: 'no-close',
                draggable: true,
                width: 450, minHeight: 420,
                height: 350,
                closeOnEscape: false,
                title: "Error/Warning",
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $(this).closest('.ui-dialog').children('.ui-dialog-content').css('height', '230px');
                }
            });
            $('#ConfirmDialogApprove').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                modal: true,
                width: 450, maxHeight: 420,
                height: 260,
                title: "Reverse Feed Import",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

            $('#ConfirmDialogDiscard').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                modal: true,
                width: 450, maxHeight: 420,
                height: 260,
                title: "Reverse Feed Import",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        }
        function ShowDialogRun(id, text) {
            var isOpen = false;
            $('#ConfirmDialogApprove').dialog("open");
            return isOpen;
        }

        function closeDialogRun(id) {
            $('#' + id).dialog('close');
        }

        function ShowDialogDiscard(id, text) {
            var isOpen = false;
            $('#ConfirmDialogDiscard').dialog("open");
            return isOpen;
        }
        function closeDialogDiscard(id) {
            $('#' + id).dialog('close');

        }
        function GetReverseFeedDifferenceDetails(fundId) {
            var importbaseId = $('#hdnImportbaseID').val();
            var exportbaseId = $('#hdnExportBaseID').val();
            $('#lblFundNo').text(fundId);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ReverseFeedImport.aspx/GetReverseFeedDifferenceDetails",
                data: "{ 'requestedFundId': '" + fundId + "','requestedImportBaseId':'" + importbaseId + "','requestedExportBaseId':'" + exportbaseId + "'}",

                datatype: "json",
                success: function (data) {
                    $("#tblDifferencedetails tbody").html("");
                    if (data.d == null) {
                        $("#tblDifferencedetails").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>No records found.</td></tr>");
                    }
                    else if (data.d.length <= 0) {
                        $("#tblDifferencedetails").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>No records found.</td></tr>");
                    }
                    else {
                        for (var i = 0; i < data.d["ReverseFeedDifference"].length; i++) {

                            $("#tblDifferencedetails").append("<tr class='DataGrid_NormalStyle'><td style='width:15%;'>" + data.d["ReverseFeedDifference"][i].Description + "</td><td style='width:15%;text-align:right'>" + data.d["ReverseFeedDifference"][i].CurrentPayrollAmount + "</td><td style='width:15%;text-align:right'>" + data.d["ReverseFeedDifference"][i].LastPayrollAmount + "</td></tr>");

                        }
                    }
                    $("#tblErrorWarning2 tbody").html("");
                    if (data.d["ReverseFeedErrorWarning"].length <= 0) {
                        $("#tblErrorWarning2").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>No records found.</td></tr>");
                    }
                    else if (data.d["ReverseFeedErrorWarning"].length <= 0) {
                        $("#tblErrorWarning2").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>No records found.</td></tr>");
                    }
                    else {
                        for (var i = 0; i < data.d["ReverseFeedErrorWarning"].length; i++) {
                           
                                $("#tblErrorWarning2").append("<tr class='DataGrid_NormalStyle'><td style='width:5%;text-align:left'>" + data.d["ReverseFeedErrorWarning"][i].Type + "</td><td style='width:15%;text-align:left'>" + data.d["ReverseFeedErrorWarning"][i].Reason + "</td></tr>");
                            
                        }
                    }
                },
                error: function (result) {
                    $("#tblDifferencedetails tbody").html("");
                    $("#tblDifferencedetails").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>" + result.responseText + "</td></tr>");
                    $("#tblErrorWarning2 tbody").html("");
                    $("#tblErrorWarning2").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>" + result.responseText + "</td></tr>");
                }
            });
        }
        
            function GetErrorWarning(BaseId,fundNo) {
                var importbaseHeaderId = $('#hdnImportbaseID').val();
               
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "ReverseFeedImport.aspx/GetErrorWarning",
                    data: "{ 'requestedBaseId': '" + BaseId + "','requestedImportBaseId':'" + importbaseHeaderId + "','requestedFundNo':'" + fundNo + "'}",
                    datatype: "json",
                    success: function (data) {
                        $("#tblErrorWarning tbody").html("");
                        if (data.d == null) {
                            $("#tblErrorWarning").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>No records found.</td></tr>");
                        }
                        else if (data.d.length <= 0) {
                            $("#tblErrorWarning").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>No records found.</td></tr>");
                        }
                        else {
                            for (var i = 0; i < data.d.length; i++) {

                                $("#tblErrorWarning").append("<tr class='DataGrid_NormalStyle'><td style='width:5%;text-align:left''>" + data.d[i].FundNo + "</td><td style='width:5%;text-align:left'>" + data.d[i].Type + "</td><td style='width:15%;text-align:left'>" + data.d[i].Reason + "</td></tr>");

                            }
                        }
                    },
                    error: function (result) {
                        $("#tblErrorWarning tbody").html("");
                        $("#tblErrorWarning").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>" + result.responseText + "</td></tr>");
                    }
                });
            }

            function CloseReverseFeedDifferenceDialog() {
                $('#divShowReverseFeedDifference').dialog("close");
                return false;
            }

            function CloseErrorWarningDialog() {
                $('#divErrorWarning').dialog("close");
                return false;
            }

            function ShowErrorWarningDialog(basedid,fundNo) {
                $('#divErrorWarning').dialog("open");
                GetErrorWarning(basedid,fundNo);
                return false;
            }

            function ShowReverseFeedDifferenceDialog(fosender,fid) {                
                $('#divShowReverseFeedDifference').dialog("open");
                GetReverseFeedDifferenceDetails(fid);
                return false;
            }
        
            function CallLetter() {
                window.open('CallReport.aspx', 'ReportPopUp1', 'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
            }
        function ontabmousehover(id) {
            $('#' + id).attr('class', 'tabselected');
        }
        function ontabmousehout(id) {
            $('#' + id).attr('class', 'tabNotSelected');
        }
        $(function () {
            $("[id*=gvAnn] td").bind("click", function () {
                var row = $(this).parent();
                $("[id*=gvAnn] tr").each(function () {
                    if ($(this)[0] != row[0]) {
                        $("td", this).removeClass("selected_row");
                    }
                });
                $("td", row).each(function () {
                    if (!$(this).hasClass("selected_row")) {
                        $(this).addClass("selected_row");
                    } else {
                        $(this).removeClass("selected_row");
                    }
                });
            });
        });
        $(function () {
            $("[id*=gvException] td").bind("click", function () {
                var row = $(this).parent();
                $("[id*=gvException] tr").each(function () {
                    if ($(this)[0] != row[0]) {
                        $("td", this).removeClass("selected_row");
                    }
                });
                $("td", row).each(function () {
                    if (!$(this).hasClass("selected_row")) {
                        $(this).addClass("selected_row");
                    } else {
                        $(this).removeClass("selected_row");
                    }
                });
            });
        });

    </script>
    <style type="text/css">
        .ChangeAnnuitantsMsg {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: #333;
            line-height: 18px;
            background-color: #FFFFC6;
            font-weight: normal;
            background-repeat: no-repeat;
            border: 0px solid #C30;
            background-position: 4px 4px;
            padding-top: 5px;
            padding-right: 5px;
            padding-bottom: 5px;
            padding-left: 5px;
            overflow: visible;
            margin-bottom: 5px;
        }

        .auto-style1 {
            background: #ffcc33;
            font-family: Verdana, Tahoma;
            font-size: 10pt;
            font-weight: bolder;
            height: 14pt;
            width: 100%;
        }
        body
        {
           font-family: Arial;
           font-size: 10pt;
        }
       td 
        {
           cursor: pointer;
        }
       .selected_row
        {
           background-color: #A1DCF2;
        }
    </style>
</asp:Content>
<asp:Content ID="contentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="Div_Center" style="width: 100%; height: 100%;">
       
                <table class="Table_WithoutBorder" cellpadding="0" cellspacing="0" width="100%" align="center" border="0">
                    <tr>
                        <td align="right" cellspacing="0">
                            <table class="td_withoutborder" border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 25px;">
                                <tr>
                                    <td>
                                        <div style="float: left;">
                                            <asp:LinkButton ID="lnkException" Text="Exceptions" runat="server" CssClass="tabNotSelected"
                                                onmouseover="javascript: ontabmousehover('lnkException');" onmouseout="javascript: ontabmousehout('lnkException');"></asp:LinkButton>
                                            <asp:Label ID="lblException" CssClass="tabSelected" runat="server" Text="Exceptions"></asp:Label>
                                        </div>

                                    </td>
                                    <td>
                                        <div style="float: left;">
                                            <asp:LinkButton ID="lnkReviewApprove" Text="Review & Approve" runat="server" CssClass="tabNotSelected"
                                                onmouseover="javascript: ontabmousehover('lnkReviewApprove');" onmouseout="javascript: ontabmousehout('lnkReviewApprove');"></asp:LinkButton>
                                            <asp:Label ID="lblReviewApprove" CssClass="tabSelected" runat="server" Text="Review & Approve"></asp:Label>
                                        </div>
                                    </td>

                                    <td class="TextBox_Normal">
                                        <div style="float: right; text-align: right;" class="Label_Small">
                                            <asp:Label ID="Label3" runat="server" Text="File Import Date : " /><asp:Label ID="lblFileImportDate" runat="server" Text=""></asp:Label>
                                            <br />
                                            <asp:Label ID="Label4" runat="server" Text="Payroll Date : " /><asp:Label ID="lblPayrollDate" runat="server" Text=""></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

                <table class="Table_WithBorder" style="width: 100%; height: 500px;" border="0">                   
                    <tr>
                        <td id="tdReviewApprove" runat="server" style="vertical-align: top; height: 80%;" class="Table_WithBorder">
                            <table border="0" style="width: 100%;">
                                <tr>
                                    <td class="td_Text" style="width: 100%;">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td id="tdHeaderSectionFilter" class="td_Text" style="text-align: left; width: 100%;" runat="server">List of Annuitants</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; text-align: left;">
                                        <div class="ChangeAnnuitantsMsg">
                                            <label id="lblChangeAnnuitantsMsg" runat="server" class="ChangeAnnuitantsMsg">This section shows annuitants comparison between current payroll proof and Bank acknowledgement files (Reverse Feed/LAR).</label>
                                        </div>
                                    </td>
                                </tr>                              
                                <tr>
                                    <td style="width: 100%;">
                                        <div style="overflow: auto; width: 100%; height: 400px; ">
                                            <asp:GridView ID="gvAnn" AllowSorting="true" runat="server" AllowPaging="true" PageSize="1000" OnPageIndexChanging="OnPageIndexChanging" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="false" EnableViewState="true">
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle" />
                                                <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle" />
                                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                                <SortedAscendingHeaderStyle CssClass="sortasc" />
                                                <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                                <Columns>
                                                    <asp:BoundField DataField="FundNo" HeaderText="Fund No."  ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="9%"  ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="LastName" HeaderText="Last Name"  ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="11%"  ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="FirstName" HeaderText="First Name"  ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="11%"  ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="GrossAmount" HeaderText="Gross Amount"  ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="10%"  />
                                                    <asp:BoundField DataField="StateTaxWithheld" HeaderText="State Tax Withheld"  ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="10%" />
                                                    <asp:BoundField DataField="FedTaxWithheld" HeaderText="Fed. Tax Withheld"  ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="10%"  />
                                                    <asp:BoundField DataField="NYCYonkersTaxWithheld" HeaderText="NYC/Yonkers Tax Withheld"  ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="10%"  />
                                                    <asp:BoundField DataField="OtherWithheld" HeaderText="Other Withheld"  ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="8%"  />
                                                    <asp:BoundField DataField="NetAmt" HeaderText="Net Amount"  ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="10%"  />
                                                    <asp:BoundField DataField="Check" HeaderText="Payment Ref#"  ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%"  />
                                                    <asp:TemplateField HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                             <img alt="View Change" src="images/details.gif" onclick="ShowReverseFeedDifferenceDialog(this,<%#Eval("FundNo")%>); return false;" />                                                           
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="pagination"  />
                                            </asp:GridView> 
                                        </div>
                                    </td>
                                </tr>
                                <tr>

                                    <td style="text-align: right; width: 100%; height: 10%;">
                                        <label id="lblRARecordList" class="Label_Small" runat="server">No of record(s) :  </label>
                                        <label id="lblReviewApproveRecords" runat="server" class="Label_Small" ></label>
                                    </td>
                                </tr>
                                <%-- END - Changed annuitants Grid--%>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="tdException" runat="server" style="vertical-align: top; height: 80%;" class="Table_WithBorder">
                            <table border="0" style="width: 100%;">
                                <tr>
                                    <td class="td_Text" style="width: 100%;">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td id="tdHeaderExceptionSection" class="td_Text" style="text-align: left; width: 100%;" runat="server">Exceptions List</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; text-align:left;">
                                        <div class="ChangeAnnuitantsMsg">
                                            <label id="lblExceptionMsg" runat="server" class="ChangeAnnuitantsMsg">This section shows list of errors/warnings encountered during (Reverse Feed/LAR) import.</label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%;">
                                        <div id="divFileError" style="overflow: auto;width:75%;" runat ="server">
                                            <asp:Label id="lblFileError" runat="server" Text="General - Error(s)/Warning(s)" Font-Bold="true" Font-Size="10pt" Height="16pt" visible="false"> </asp:Label>
                                            <asp:GridView ID="gvFileErrWrn"  runat="server" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="false"  >
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle" />
                                                <RowStyle CssClass="DataGrid_NormalStyle" Height="100%" />
                                                <SortedAscendingHeaderStyle CssClass="sortasc" />
                                                <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                                <Columns>
                                                   <asp:BoundField DataField="chrType" HeaderText="Type"  ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%"  ItemStyle-CssClass="ClassHide" />
                                                   <asp:BoundField DataField="chvErrorDesc" HeaderText="Reason"  ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="10%"  ItemStyle-CssClass="ClassHide" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:Label ID="lblAnnuitantsErrWrn" runat="server" Visible="false" Text="Annuitants - Error(s)/Warning(s)" Font-Bold="true" Font-Size="10pt" Height="16pt"> </asp:Label>
                                        <div style="overflow: auto; width: 75%; height:400px; ">
                                            
                                            <asp:GridView ID="gvException" AllowPaging="true" PageSize ="1000" AllowSorting="true" runat="server" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="false"  OnPageIndexChanging="OnPageIndexChangingExceptionGrid">
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle" />
                                                <RowStyle CssClass="DataGrid_NormalStyle" Height="100%" />
                                                <SortedAscendingHeaderStyle CssClass="sortasc" />
                                                <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                                <Columns>
                                                    <asp:BoundField DataField="FUNDNO" HeaderText="Fund No."  ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="10%"  ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="LASTNAME" HeaderText="Last Name"  ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="10%"  ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="FIRSTNAME" HeaderText="First Name"  ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="10%"  ItemStyle-CssClass="ClassHide" />
                                                    <asp:TemplateField HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center">
                                                    <Itemtemplate>
                                                        <asp:ImageButton ID="imgErrorDetails" runat="server" CausesValidation="False" ImageUrl="images\error.gif" CommandName="Error" CommandArgument="<%# Container.DataItemIndex %>"
                                                           ToolTip="" Style="cursor: pointer; height: 15px;" Visible="false" ></asp:ImageButton>
                                                      </ItemTemplate>
                                                    </asp:TemplateField>
                                                      <asp:TemplateField HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center">
                                                    <Itemtemplate>
                                                        <asp:ImageButton ID="imgWarningDetails" runat="server" CausesValidation="False" ImageUrl="images/icon-warning.gif" CommandName="Warning" CommandArgument="<%# Container.DataItemIndex %>"
                                                            ToolTip="" Style="cursor:pointer; height:15px;" Visible="false" ></asp:ImageButton>
                                                            <asp:HiddenField ID="hdnBaseId" runat="server" Value='<%#Eval("BASEID")%>'></asp:HiddenField>
                                                            <asp:HiddenField ID="hdnFundNo" runat="server" Value='<%#Eval("FUNDNO")%>'></asp:HiddenField>
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="text-align: right; width: 100%; height: 10%;">
                                        <label id="lblExpListRecords" class="Label_Small" runat="server">No of record(s) :  </label>
                                        <label id="lblExceptionRecords" runat="server" class="Label_Small"></label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="vertical-align:top;width: 100%;">
                            <td>
                                 <div id="divDARImportSummary" style="width: 100%;display:block;" runat="server">
                                    <table border ="0" style="width: 100%;">
                                        <tr style="vertical-align: bottom;">
                                            <td class="td_Text" >
                                                Summary (All Annuitants - Reverse Feed) 
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: bottom;">
                                        <td>
                                            <table style="width: 100%; " border="0">
                                                 <tr>
                                                    <td align="left" width="15%">
                                                        <asp:Label ID="lblTotalGrossAmount" runat="server" CssClass="Label_Small">Total Gross Amount:</asp:Label>
                                                    </td>
                                                    <td align="right" width="15%" style="padding-right:20px;">
                                                        <span id="lblTotalGrossAmountValue" runat="server" Class="Label_Small" ></span>
                                                    </td>
                                                    <td align="left" width="20%" >
                                                        <asp:Label ID="lblTotalStateTaxWithheld" runat="server" CssClass="Label_Small">Total State Tax Withheld:</asp:Label>
                                                    </td>
                                                    <td align="right" width="15%" style="padding-right:20px;">  
                                                        <span ID="lblTotalStateTaxWithheldValue" runat="server" Class="Label_Small"></span>
                                                    </td>
                                                    <td align="left" width="20%">
                                                        <asp:Label ID="lblTotalFedTaxWithheld" runat="server" CssClass="Label_Small">Total Fed Tax Withheld:</asp:Label>
                                                    </td>
                                                    <td align="right" width="15%" style="padding-right:20px;">
                                                        <span ID="lblTotalFedTaxWithheldValue" runat="server" Class="Label_Small"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="15%">
                                                        <asp:Label ID="lblTotalLocalTaxWithheld" runat="server" CssClass="Label_Small">Total Local and Other Withheld:</asp:Label>
                                                    </td>
                                                    <td align="right" width="15%" style="padding-right:20px;">
                                                        <span ID="lblTotalLocalTaxandOtherWithheldValue" runat="server" Class="Label_Small"></span>
                                                    </td>                                                    
                                                    <td align="left" width="20%">
                                                        <asp:Label ID="lblTotalNetAmount" runat="server" CssClass="Label_Small">Total Net Amount:</asp:Label>
                                                    </td>
                                                    <td align="right" width="15%" style="padding-right:20px;">
                                                       <span ID="lblTotalNetAmountValue" runat="server" Class="Label_Small"></span>
                                                    </td>
                                                     <td align="left" width="20%">
                                                        <asp:Label ID="lblTotalNoOfRecords" runat="server" CssClass="Label_Small">Total Number of Records:</asp:Label>
                                                    </td>
                                                    <td align="right" width="15%" style="padding-right:20px;">
                                                        <span id="lblTotalNoOfRecordsValue" runat="server" Class="Label_Small" ></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                 </table>
                                 </div>
                             </td>
                        </tr>
                    <tr>
                        <td style="text-align: right;" class="auto-style1">
                            <asp:Button ID="btnPrintList" runat="server" Text="Print List" class="Button_Normal" />&nbsp;
                            <asp:Button ID="btnApprove" runat="server" Text="Approve" class="Button_Normal"  OnClientClick="javascript:return ShowDialogRun();"  />&nbsp;   
                            <asp:Button ID="btnDiscard" runat="server" Text="Discard" class="Button_Normal"  OnClientClick="javascript:return ShowDialogDiscard();"  />&nbsp;
                            <asp:Button ID="btnClose" runat="server" Text="Close" class="Button_Normal" />
                            <asp:HiddenField ID="hdnImportbaseID" runat ="server" ClientIDMode="Static"/>
                             <asp:HiddenField ID="hdnExportBaseID" runat ="server" ClientIDMode="Static" />
                        </td>
                    </tr>
                </table>
    
             <div id="divShowReverseFeedDifference" runat="server" style="display: block; overflow: auto;height:160px">
                            
                <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="100%">
                    <tr>
                        <td style="padding-bottom:5px">
                            <span>Fund No:</span> <span ID="lblFundNo"></span>
                        </td>
                    </tr>
                    <tr>

                    </tr>
                    <tr>
                        <td>
                                 <table id="tblDifferencedetails" style="width:100%; BORDER-COLLAPSE: collapse" cellSpacing=0  rules=all border=1   class ="DataGrid_Grid">
                                    <thead class="DataGrid_HeaderStyle" >
                                        <th>Description</th>
                                        <th>Import/Reverse Feed</th>
                                        <th>Export/Current Payroll Proof</th>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                        </td>
                    </tr>
                     <tr>
                        <td colspan="3"><br/></td>
                    </tr>
                     <tr>
                        <td>
                                 <table id="tblErrorWarning2" style="width:100%; BORDER-COLLAPSE: collapse" cellSpacing=0  rules=all border=1   class ="DataGrid_Grid">
                                    <thead >
                                        <th colspan="2" style="height:20px">Error(s)/Warning(s)</th>                                        
                                    </thead>
                                    <thead class="DataGrid_HeaderStyle" >
                                        <th>Type</th>
                                        <th>Description</th>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /> <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button class="Button_Normal" ID="Button1" runat="server" Width="80" Text="OK" OnClientClick="javascript:return CloseReverseFeedDifferenceDialog();"></asp:Button>
                    </td>
                    </tr>
                </table>
           
        </div> 
         <div id="divErrorWarning" runat="server" style="display: block; overflow: auto;height:160px">
                            
                <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="100%">
                    <tr>
                        <td>
                                 <table id="tblErrorWarning" style="width:100%; BORDER-COLLAPSE: collapse" cellSpacing=0  rules=all border=1   class ="DataGrid_Grid">
                                    <thead class="DataGrid_HeaderStyle" >
                                        <th>Fund No.</th>
                                        <th>Type</th>
                                        <th>Reason</th>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br /> <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button class="Button_Normal" ID="Button2" runat="server" Width="80" Text="OK" OnClientClick="javascript:return CloseErrorWarningDialog();"></asp:Button>
                    </td>
                    </tr>
                </table>
           
        </div> 
        <div id="ConfirmDialogApprove" title="DAR Import File" style="display: none;">
                    <div>
                        <table width="100%" height="250px" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;">
                            <tr>
                                <td rowspan="2" style="width: 10%;">
                                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="imgConfirmDialogInfo" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divConfirmDialogMessageRun">
                                        Are you sure you want to approve the Reverse Feed import?
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                </td>
                            </tr>
                            <tr id="trConfirmDialogYesNo">
                                <td align="right" valign="bottom" colspan="2">
                                    <asp:Button ID="btnConfirmDialogYesApprove" runat="server"  Text="Yes" cssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" OnClientClick="closeDialogRun('ConfirmDialogApprove');"/>&nbsp;
                                    <input type="button" id="btnConfirmDialogNoRun" value="No" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        onclick="closeDialogRun('ConfirmDialogApprove');" />
                                </td>
                            </tr>
                        </table>
                    </div>
            </div> 

           <div id="ConfirmDialogDiscard" title="DAR Import File" style="display: none;">
                    <div>
                        <table width="100%" height="250px" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;">
                            <tr>
                                <td rowspan="2" style="width: 10%;">
                                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="img1" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divConfirmDialogMessageDiscard">
                                        Are you sure you want to discard the Reverse Feed import?
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                </td>
                            </tr>
                            <tr id="tr1">
                                <td align="right" valign="bottom" colspan="2">
                                    <asp:Button ID="btnConfirmDialogYesDiscard" runat="server"  Text="Yes" cssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" OnClientClick="closeDialogDiscard('ConfirmDialogDiscard');"/>&nbsp;
                                    <input type="button" id="btnConfirmDialogNoDiscards" value="No" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        onclick="closeDialogDiscard('ConfirmDialogDiscard');" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div> 
    </div>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>  
</asp:Content>

