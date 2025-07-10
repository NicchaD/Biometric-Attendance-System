<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master" ValidateRequest="false" CodeBehind="RMDBatchRequestAndProcessing.aspx.vb" Inherits="YMCAUI.RMDBatchRequestAndProcessing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="JS/jquery-1.5.1.min.js"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="JS/jquery-bPopup.js"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />

    <style type="text/css">
        .BG_ColourIsLocked {
            background-color: #90EE90; /*LightGreen*/
        }

        .BG_ColourIsBlocked {
            background-color: #FFB6C1; /*LightPink*/
        }

        .BG_ColourIsMultipleYearMRD {
            background-color: #00BFFF; /*DodgerBlue*/
        }

        .BG_ColourIsNotEnrollAnnualMRDPayments {
            background-color: #FBC97A; /*Red*/
        }

        .BG_ColourInitialLetter {
            background-color: #F08080; /*LightSkyBlue*/
        }


        .hide {
            display: none;
        }

        .show {
            display: block;
        }

        .LstFilter {
            width: 120px;
            height: 50px;
            font-family: Arial;
            font-size: 11px;
        }

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
        }
    </style>


    <script type="text/javascript">
        var CloseDisable = false;
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    args.set_errorHandled(true);
                    //BindEvents();
                }
            }
            //BindEvents();
            //$('#btnClose').click(function () {
            //    $('#modalRMD').dialog('close');
            //    //window.location.href = "RMDBatchRequestAndProcessing.aspx?Process=complete";
            //    return false;
            //});

            $('#btnProcess').click(function () {
                //  BindEvents();
            });

            $('#ConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                width: 350, height: 350,
                title: "RMD Batch Request and Process",
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        });

        function BindEvents() {
            EligibleRMDCheckBoxSelection();
            NonEligibleRMDCheckBoxSelection();
        }

        function EligibleRMDCheckBoxSelection() {
            var allEligibleRMDCheckBoxSelector = '#<%=gvRMDProcess.ClientID%> input[id*="chkSelectAllEligible"]:checkbox';
            var checkBoxSelectorEligibleRMD = '#<%=gvRMDProcess.ClientID%> input[id*="chkRMDProcessEligible"]:checkbox';

            $(allEligibleRMDCheckBoxSelector).bind('click', function () {
                $(checkBoxSelectorEligibleRMD).attr('checked', $(this).is(':checked'));
                ToggleCheckUncheckAllOptionAsNeeded(allEligibleRMDCheckBoxSelector, checkBoxSelectorEligibleRMD);
            });

            ToggleCheckUncheckAllOptionAsNeeded(allEligibleRMDCheckBoxSelector, checkBoxSelectorEligibleRMD);

            $(checkBoxSelectorEligibleRMD).bind('click', function () {
                mark_dirty();
            });

        }


        function NonEligibleRMDCheckBoxSelection() {
            var allNonEligibleRMDCheckBoxSelector = '#<%=gvNonEligible.ClientID%> input[id*="chkSelectAllNonEligible"]:checkbox';
            var checkBoxSelectorNonEligibleRMD = '#<%=gvNonEligible.ClientID%> input[id*="chkRMDProcessNonEligible"]:checkbox';
            $(allNonEligibleRMDCheckBoxSelector).bind('click', function () {
                $(checkBoxSelectorNonEligibleRMD).attr('checked', $(this).is(':checked'));
                ToggleCheckUncheckAllOptionAsNeeded(allNonEligibleRMDCheckBoxSelector, checkBoxSelectorNonEligibleRMD);
            });

            ToggleCheckUncheckAllOptionAsNeeded(allNonEligibleRMDCheckBoxSelector, checkBoxSelectorNonEligibleRMD);

            $(checkBoxSelectorNonEligibleRMD).bind('click', function () {
                mark_dirty();
            });

        }


        function ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector) {
            var totalCheckboxes = $(checkBoxSelector),
                        checkedCheckboxes = totalCheckboxes.filter(":checked"),
                        noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
                        allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);
            if (totalCheckboxes.length == 0) allCheckboxesAreChecked = false;
            $(allCheckBoxSelector).attr('checked', allCheckboxesAreChecked);
        }

        function windowrefreshPage(i) {
            setTimeout(function () {
                window.location.href = "RMDBatchRequestAndProcessing.aspx?count=" + i + "&submit=true";
            }, 1000);
        }

        function OpenProgressPopup() {
            $('#modalRMD').dialog({
                autoOpen: false,
                draggable: true,
                show: "fade",
                hide: "fade",
                modal: true,
                close: false,
                title: "RMD - Batch Process Status",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                },
                width: 900,
                height: 700,
                display: "block",
                closeOnEscape: false
                //position: ["center","center"],
                // beforeclose: function () {
                //    return CloseDisable;
                // }

            });
            buttons: [{ text: "Close", click: ClosePrintDialog }]
            $('#modalRMD').dialog("open");

        }
        function ClosePrintDialog() {
            $('#modalRMD').dialog('close');
        }

        function CallProcess(strbatchId, strReportType, strModule) {
            OpenProgressPopup();
            CallRMDProcess(strbatchId, strReportType, 0, "", 0, 0, strModule);
            // setTimeout('GetRMDProcessStatus(' + strbatchId + ')', 500);
        }

        function closeDialog(id) {
            $('#' + id).dialog('close');
        }


        function OpenPDF(strFileName) {
            try {
                window.open(strFileName, 'OpenCustomPDF', 'width=900, height=900, resizable=yes, scrollbars=yes ,directories=no ,toolbar=no ,menubar=no, location=no');
            }
            catch (err) {
                alert(err.message);
            }
        }

        function CallArrErrorDataList(strBatchId, strModule) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "FT/CopyFilestoFileServer.aspx/ArrErrorDataList",
                data: "{'strBatchId': '" + strBatchId + "','strModule':'" + strModule + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d == "ArrErrorDataList table not found in database.") {
                        $('#divPopErrorMsg').html("Some internal error occured while creating this batch. Please refer the RMD Process Status tab for details.");
                    }
                    else {
                        $('#divPopErrorMsg').html(data.d);
                    }
                },
                error: function (result) {
                    $('#divPopErrorMsg').text("Some network error occured while copying files to IDM server.");
                }
            });
        }
        var RMDBatchProcessCounter
        function CopyFilestoFileServer(strOR, strDEL, strBatchId, strModule) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "FT/CopyFilestoFileServer.aspx/CopytoFileServer",
                data: "{'strOR':'" + strOR + "','strDEL':'" + strDEL + "','strBatchId':'" + strBatchId + "','strModule':'" + strModule + "'}",
                dataType: "json",
                success: function (data) {
                    //$('#dvDsiplay').html(data.d);
                    RMDBatchProcessCounter.d.iPdfCopied = data.d.iIdxCopied;
                    RMDBatchProcessCounter.d.iIdxCopied = data.d.iPdfCopied;
                    if (data.d.strStatus == 'error') {
                        //$('#dvDsiplay').html(data.d.strError);
                        $('#dvStatusMsg').text("Some IDX or PDF files were not copied to IDM server.");
                        CallArrErrorDataList(strBatchId, strModule)
                        $("#trshowErrors")[0].className = "show";
                        UpdateDisplay(RMDBatchProcessCounter);
                        $('#dvCopy').text("Error");
                        $('#dvCopy')[0].className = "error-Progress";
                    }
                    else {
                        UpdateDisplay(RMDBatchProcessCounter);

                        if (RMDBatchProcessCounter.d.iTotalIDXPDFCount < RMDBatchProcessCounter.d.iTotalCount) {
                            $('#dvStatusMsg').text("Some of the records were not proccessed due to problem, please refer the RMD Process Status tab for details.");
                            CallArrErrorDataList(strBatchId, strModule)
                            $("#trshowErrors")[0].className = "show";
                            UpdateDisplay(RMDBatchProcessCounter);
                            $('#dvCopy').text("Error");
                            $('#dvCopy')[0].className = "error-Progress";
                        }
                        else if (RMDBatchProcessCounter.d.iPdfCreated < RMDBatchProcessCounter.d.iTotalIDXPDFCount || RMDBatchProcessCounter.d.iIdxCreated < RMDBatchProcessCounter.d.iTotalIDXPDFCount) {
                            $('#dvStatusMsg').text("Some IDX or PDF files were not created.");
                            CallArrErrorDataList(strBatchId, strModule)
                            $("#trshowErrors")[0].className = "show";
                            UpdateDisplay(RMDBatchProcessCounter);
                            $('#dvCopy').text("Error");
                            $('#dvCopy')[0].className = "error-Progress";
                        }
                        else if (RMDBatchProcessCounter.d.iPdfCopied == RMDBatchProcessCounter.d.iTotalIDXPDFCount && RMDBatchProcessCounter.d.iIdxCopied == RMDBatchProcessCounter.d.iTotalIDXPDFCount) {
                            $('#dvStatusMsg').text("RMD Batch Process completed sucessfully.");

                            $('#dvCopy').text("Completed");
                            $('#dvPDF').text("Completed");
                            $('#dvIDX').text("Completed");
                            $('#dvReg').text("Completed");

                            $('#dvPDF')[0].className = "success-Progress";
                            $('#dvIDX')[0].className = "success-Progress";
                            $('#dvReg')[0].className = "success-Progress";
                            $('#dvCopy')[0].className = "success-Progress";


                        }
                        else if (RMDBatchProcessCounter.d.iTotalIDXPDFCount == RMDBatchProcessCounter.d.iTotalCount) {
                            $('#dvStatusMsg').text("RMD Batch Process completed sucessfully.");

                            $('#dvCopy').text("Completed");
                            $('#dvPDF').text("Completed");
                            $('#dvIDX').text("Completed");
                            $('#dvReg').text("Completed");

                            $('#dvPDF')[0].className = "success-Progress";
                            $('#dvIDX')[0].className = "success-Progress";
                            $('#dvReg')[0].className = "success-Progress";
                            $('#dvCopy')[0].className = "success-Progress";


                        }
                        else {
                            $('#dvStatusMsg').text("Internal exception.");
                            CallArrErrorDataList(strBatchId, strModule)
                            $("#trshowErrors")[0].className = "show";
                            UpdateDisplay(RMDBatchProcessCounter);
                            $('#dvCopy').text("Error");
                            $('#dvCopy')[0].className = "error-Progress";
                        }

                        //$('#dvCopy').text("");
                        //$('#dvPDF').text("");
                        //$('#dvIDX').text("");
                        //$('#dvReg').text("");

                        $('#imgCopiedComplete').css("display", "none");
                        $('#imgPDFComplete').css("display", "none");
                        $('#imgIDXComplete').css("display", "none");
                        $('#imgRegComplete').css("display", "none");

                    }
                },
                error: function (result) {
                    UpdateDisplay(RMDBatchProcessCounter);
                    $("#trshowErrors")[0].className = 'show';
                    CallArrErrorDataList(strBatchId, strModule);
                }
            });
        }

        function CallRMDProcess(strbatchId, strReportType, iCount, strProcessName, iIDXCreated, iPDFCreated, strModule) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "RMDBatchRequestAndProcessing.aspx/RMDProcess",
                data: "{'strBatchId': '" + strbatchId + "','strReportType':'" + strReportType + "','iCount':" + iCount + ",'strProcessName':'" + strProcessName + "'," +
                    "'iIDXCreated':" + iIDXCreated + ",'iPDFCreated':" + iPDFCreated + ",'strModule':'" + strModule + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d.strretValue == "success") {
                        CopyFilestoFileServer(1, 1, strbatchId, strModule, 'RMDBatchProcess');
                        RMDBatchProcessCounter = data;

                        $('#dvPDF').text("Sucess");
                        $('#dvIDX').text("Sucess");
                        $('#dvReg').text("Sucess");

                        $('#dvPDF')[0].className = "success-Progress";
                        $('#dvIDX')[0].className = "success-Progress";
                        $('#dvReg')[0].className = "success-Progress";

                    }
                    else if (data.d.strretValue == "error") {
                        $('#dvMessage').text("Some error occured.");
                        $("#dvMessage")[0].className = "error-msg";
                        $("#trshowErrors")[0].className = 'show';
                        CallArrErrorDataList(strbatchId, strModule);
                        UpdateDisplay(data);
                        $('#dvPDF').text("Error");
                        $('#dvIDX').text("Error");
                        $('#dvReg').text("Error");

                        $('#dvPDF')[0].className = "error-Progress";
                        $('#dvIDX')[0].className = "error-Progress";
                        $('#dvReg')[0].className = "error-Progress";
                        $('#dvCopy')[0].className = "info-Progress";
                    }
                    else if (data.d.strretValue == "pending") {
                        CallRMDProcess(data.d.strBatchId, data.d.strReportType, data.d.iProcessCount, data.d.strProcessName, data.d.iIdxCreated, data.d.iPdfCreated, strModule);
                        UpdateDisplay(data);
                        $('#dvPDF')[0].className = "info-Progress";
                        $('#dvIDX')[0].className = "info-Progress";
                        $('#dvReg')[0].className = "info-Progress";
                        $('#dvCopy')[0].className = "info-Progress";

                    }
                    else {
                        $("#dvMessage")[0].className = "error-msg";
                        $("#trshowErrors")[0].className = 'show';
                        CallArrErrorDataList(strbatchId, strModule);
                        UpdateDisplay(data);


                        $('#dvPDF').text("Error");
                        $('#dvIDX').text("Error");
                        $('#dvReg').text("Error");
                        $('#dvPDF')[0].className = "error-Progress";
                        $('#dvIDX')[0].className = "error-Progress";
                        $('#dvReg')[0].className = "error-Progress";
                        $('#dvCopy')[0].className = "info-Progress";


                    }
                },
                error: function (result) {
                    $('#dvMessage').text("Some network error occured.");
                    $("#dvMessage")[0].className = "error-msg";
                    $("#trshowErrors")[0].className = 'show';
                    CallArrErrorDataList(strbatchId, strModule);
                    UpdateDisplay(data);
                    $('#dvPDF').text("Error");
                    $('#dvIDX').text("Error");
                    $('#dvReg').text("Error");

                    $('#dvPDF')[0].className = "error-Progress";
                    $('#dvIDX')[0].className = "error-Progress";
                    $('#dvReg')[0].className = "error-Progress";
                    $('#dvCopy')[0].className = "info-Progress";
                }
            });
        }

        var iPreviousValue = 0;
        function UpdateDisplay(data) {
            var strStatus = ''
            strStatus = "Total No. of records under process " + data.d.iTotalCount + ".";
            //strStatus += " <br /> No. of eligible records for process " + data.d.iTotalIDXPDFCount + ".";
            $('#dvShowStatus').html(strStatus);
            $('#dvReqProgress').text(data.d.iTotalIDXPDFCount + " out of " + data.d.iTotalCount + ".");
            $('#dvIDXProgress').text(data.d.iIdxCreated + " out of " + data.d.iTotalCount + ".");
            $('#dvPDFProgress').text(data.d.iPdfCreated + " out of " + data.d.iTotalCount + ".");
            $('#dvCopiedProgress').text(data.d.iPdfCopied + " out of " + data.d.iTotalCount + ".");
        }

        function ExceptionShowError() {
            $('#dvShowErrorsMsg').dialog({
                autoOpen: false,
                draggable: false,
                resizable: false,
                close: false,
                modal: true,
                width: 500, height: 400,
                title: "Error Message",
                open: function (type, data) {
                }
            });
            $('#dvShowErrorsMsg').css({ display: "block" });
            $('#dvShowErrorsMsg').dialog("open");
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

        function showConfirmdialog() {
            $('#ConfirmDialog').dialog("open");
        }

        function closeConfirmdialog() {
            $('#ConfirmDialog').dialog('close');
        }



        /* Check Box Selection code */

        function chkall(ival, gvObj) {
            var f = document.getElementById(gvObj);
            for (var i = 1; i < f.getElementsByTagName("input").length; i++) {
                if (f.getElementsByTagName("input").item(i).type == "checkbox") {
                    if (f.getElementsByTagName("input").item(i).disabled == false) {
                        f.getElementsByTagName("input").item(i).checked = ival;
                    }
                }
            }
        }

        function Unckeck(obj, gvObj) {
            var f = document.getElementById(gvObj);
            if (!obj.checked) {
                f.getElementsByTagName("input").item(0).checked = false;
            }
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="div_center">
        <asp:ScriptManagerProxy ID="RMDPRintScriptManager" runat="server">
        </asp:ScriptManagerProxy>
        <asp:UpdatePanel ID="uplRMDProcess" runat="server">
            <ContentTemplate>
                <table class="Table_WithOutBorder" id="Table3" cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tr>
                        <td align="left" nowrap="true" valign="middle" width="40%"> <%--MMR | 2016.10.19 | YRS-AT-2922 | Changed the width--%>

                            <asp:Label runat="server" ID="lblRMDLastProcessDate" class="Label_Small"></asp:Label></td>

                        <td align="right" cellspacing="0">
                            <table class="td_withoutborder" cellpadding="0" cellspacing="0" style="width: 100%; height: 25px;">
                                <tr>
                                    <td id="tdInitial" style="width: 35%">
                                        <%--START: MMR | 2016.10.19 | YRS-AT-2922 | Changed the Link text--%>
                                        <asp:LinkButton ID="lnkEligiblePerson" Text="Process Eligible Persons" runat="server" CssClass="tabNotSelected"
                                            onmouseover="javascript: ontabmousehover('lnkEligiblePerson');" onmouseout="javascript: ontabmousehout('lnkEligiblePerson');"></asp:LinkButton>
                                        <asp:Label ID="lbllnkEligible" CssClass="tabSelected" runat="server" Text="Process Eligible Persons"></asp:Label>
                                    </td>
                                    <td id="tdFollowup" style="width: 35%">
                                        <asp:LinkButton ID="lnkNotEligiblePerson" CssClass="tabNotSelected" Text="View Non-Eligible Persons" runat="server"
                                            onmouseover="javascript: ontabmousehover('lnkNotEligiblePerson');" onmouseout="javascript: ontabmousehout('lnkNotEligiblePerson');"></asp:LinkButton>
                                        <asp:Label ID="lbllnkNonEligible" CssClass="tabSelected" runat="server" Text="View Non-Eligible Persons"></asp:Label>
                                    </td>
                                    <td id="tdProcessStatus" style="width: 35%">
                                        <asp:LinkButton ID="lnkProcessStatus" CssClass="tabNotSelected" Text="View Batch Status" runat="server"
                                            onmouseover="javascript: ontabmousehover('lnkProcessStatus');" onmouseout="javascript: ontabmousehout('lnkProcessStatus');"></asp:LinkButton>
                                        <asp:Label ID="lblProcessStatus" CssClass="tabSelected" runat="server" Text="View Batch Status"></asp:Label>
                                        <%--END: MMR | 2016.10.19 | YRS-AT-2922 | Changed the Link text--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table class="Table_WithoutBorder" cellpadding="0" cellspacing="0" width="  100%" align="center" valign="center">
                    <tr style="vertical-align: top;">
                        <td class="tabSelectedLink" style="width: 50%; text-align: left;">
                            <asp:Label runat="server" ID="lblTabName" color="#333333" Font-Bold="true" Font-Size="13px" Font-Names="Verdana,Arial,Helvetica"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="Table_WithBorder" id="tblEligibleRMDProcess" cellspacing="1" cellpadding="1" width="100%" runat="server" visible="true">
                    <tr valign="top">
                        <td style="vertical-align: top; width: 10%; text-align: left">
                            <div style="overflow: auto; width: 100%; border-top-style: none; border-right-style: none; border-left-style: none; height: 360px; border-bottom-style: none">
                                <table class="Table_WithBorder" id="Table1" cellspacing="1" cellpadding="1" border="0" width="100%" height="100%">
                                    <tr class="DataGrid_AlternateStyle" valign="top">
                                        <td>
                                            <b>Filter Criteria</b>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid_HeaderStyle" valign="top">
                                        <td>Year
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td align="center">
                                            <%--<asp:ListBox ID="lstRmdYear" CssClass="LstFilter" runat="server" SelectionMode="Single"></asp:ListBox>--%>
                                            <asp:DropDownList ID="ddlRMDYear" CssClass="LstFilter" runat="server"></asp:DropDownList>
                                            <asp:Button runat="server" ID="btnRMDFilter" Text="View >>" CssClass="Button_Normal" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td class="Table_WithBorder">
                            <div style="overflow: auto; width: 100%; border-top-style: none; border-right-style: none; border-left-style: none; height: 350px; border-bottom-style: none">
                                <asp:GridView runat="server" ID="gvRMDProcess" CssClass="DataGrid_Grid" Width="100%"
                                    AllowSorting="true" AutoGenerateColumns="false" SelectedRowStyle-VerticalAlign="Top" EmptyDataText="No records found."
                                    BorderColor="White">
                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                    <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox runat="server" ID="chkSelectAllEligible" onclick="chkall(this.checked,'gvRMDProcess')" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkRMDProcessEligible" onclick="Unckeck(this,'gvRMDProcess')" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FundIdNo" SortExpression="FundIdNo" HeaderText="Fund No." HeaderStyle-Width="10%" ItemStyle-Width="50px" />
                                        <asp:BoundField DataField="PlanType" SortExpression="PlanType" HeaderText="Plan Type" HeaderStyle-Width="10%" ItemStyle-Width="50px" />
                                        <asp:BoundField DataField="CurrentBalance" SortExpression="CurrentBalance" HeaderText="Current Balance" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="MRDAmount" SortExpression="MRDAmount" HeaderText="RMD Amount" HeaderStyle-Width="12%" ItemStyle-Width="115px" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="PaidAmount" SortExpression="PaidAmount" HeaderText="Paid Amount" HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="StatusTypeDescription" SortExpression="StatusTypeDescription" HeaderStyle-Width="15%"
                                            HeaderText="Fund Status" />
                                        <asp:BoundField DataField="FundEventID" HeaderText="FundEventID" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="MRDYear" HeaderText="Year" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="Process_Status" HeaderText="Process Status" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="MRDExpireDate" SortExpression="MRDExpireDate" HeaderText="Due Date" HeaderStyle-Width="15%" />
                                        <asp:BoundField DataField="PerssID" HeaderText="Pers ID" HeaderStyle-Width="15%" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="bitAnnualMRDRequested" HeaderText="bitAnnualMRDRequested" HeaderStyle-Width="15%" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="IsLocked" HeaderText="Locked" SortExpression="IsLocked" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="IsBlocked" HeaderText="Blocked" SortExpression="IsBlocked" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="IsMultipleYearMRD" HeaderText="IsMultipleYearMRD" SortExpression="IsMultipleYearMRD" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="MRDUniqueID" HeaderText="MRDUniqueID" SortExpression="MRDUniqueID" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="MiddleName" HeaderText="MiddleName" SortExpression="MRDUniqueID" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="SSNO" HeaderText="SSNO" SortExpression="SSNO" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <%--START | SR | 2016.04.05 | YRS-AT-2843 - YRS enh-RMD Utility should use J&S table if primary bene is spouse more than 10 yrs younger (TrackIT 25233) --%>
                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" HtmlEncode ="false" NullDisplayText =" "  />
                                        <asp:BoundField DataField="RmdTaxableAmount" HeaderText="RmdTaxableAmount" SortExpression="RmdTaxableAmount" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="RmdNonTaxableAmount" HeaderText="RmdNonTaxableAmount" SortExpression="RmdNonTaxableAmount" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <%--END | SR | 2016.04.05 | YRS-AT-2843 - YRS enh-RMD Utility should use J&S table if primary bene is spouse more than 10 yrs younger (TrackIT 25233)--%>
                                        <%--  START | SB | 2017.01.17 | YRS-AT-3297 - Added column to determine whether the record is eligible for cashout or not --%>
                                        <asp:BoundField DataField="IsCashOutEligible" HeaderText="IsRmdCashoutEligible" SortExpression="IsRmdCashoutEligible" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <%--  END | SB | 2017.01.17 | YRS-AT-3297 - Added column to determine whether the record is eligible for cashout or not --%>
                                          </Columns>
                                </asp:GridView>
                                <asp:GridView ID="gvProcessStatus" Width="100%" runat="server" CssClass="DataGrid_Grid" Visible="false" AllowSorting="true" EmptyDataText="No records found." AutoGenerateColumns="false">
                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                    <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                    <Columns>
                                        <asp:BoundField DataField="FUNDNo" SortExpression="FundIdNo" HeaderText="Fund No." HeaderStyle-Width="10%" ItemStyle-Width="50px" />
                                        <asp:BoundField DataField="PlanType" SortExpression="PlanType" HeaderText="Plan Type" HeaderStyle-Width="10%" ItemStyle-Width="50px" />
                                        <asp:BoundField DataField="CurrentBalance" SortExpression="CurrentBalance" HeaderText="Current Balance" HeaderStyle-Width="12%" />
                                        <asp:BoundField DataField="MRDAmount" SortExpression="MRDAmount" HeaderText="RMD Amount" HeaderStyle-Width="12%" />
                                        <asp:BoundField DataField="StatusTypeDescription" SortExpression="StatusTypeDescription" HeaderText="Fund Status" HeaderStyle-Width="15%" />
                                        <asp:BoundField DataField="FundEventID" SortExpression="FundEventID" HeaderText="FundEventID" HeaderStyle-Width="15%" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="Process_Status" SortExpression="Process_Status" HeaderText="Process Status" HeaderStyle-Width="20%" ItemStyle-Width="30%" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                                                    <td align="center" colspan="2" Class="Label_Small">
                                                        <span class="BG_ColourInitialLetter">&nbsp;&nbsp;&nbsp;</span> <span class="Label_Small">RMD amount recalculated due to Beneficiary change. </span>
                                                        <span class="BG_ColourIsLocked">&nbsp;&nbsp;&nbsp;</span> <span class="Label_Small">RMD amount calculated for sole spouse beneficiary more than 10 years younger.</span>
                                                        
                                                  </td>
                   </tr>
                    <%-- START : SB | 2016.01.17 | YRS-AT-3297 | Displaying new color code for cashout eligible records legends--%>
                     <tr><td colspan="2" Class="Label_Small"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="BG_ColourSubjectToCashoutLetter">&nbsp;&nbsp;&nbsp;</span> Subject to CashOut.</td></tr>
                     <%-- END : SB | | 2016.01.17 | YRS-AT-3297 | Displaying new color code for cashout eligible records legends--%> 
                </table>
                <table class="Table_WithBorder" id="tblNonEligibleRMDProcess" cellspacing="1" cellpadding="1" width="100%" runat="server" visible="false">
                    <tr>
                        <td style="vertical-align: top; width: 10%; text-align: left">
                            <div style="overflow: auto; width: 100%; border-top-style: none; border-right-style: none; border-left-style: none; height: 460px; border-bottom-style: none">
                                <table class="Table_WithBorder" id="Table2" cellspacing="1" cellpadding="1" border="0" width="100%" height="100%">
                                    <tr class="DataGrid_AlternateStyle">
                                        <td>
                                            <b>Filter Criteria</b>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid_HeaderStyle">
                                        <td>Account Locked
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlAcctLocked" CssClass="LstFilter">
                                                <asp:ListItem Value="all" Text="Any" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="yes" Text="Yes" ></asp:ListItem>
                                                <asp:ListItem Value="no" Text="No"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid_HeaderStyle">
                                        <td>Insufficient Balance
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlInsufficientBal" CssClass="LstFilter">
                                                <asp:ListItem Value="all" Selected="True" Text="Any"></asp:ListItem>
                                                <asp:ListItem Value="yes" Text="Yes"></asp:ListItem>
                                                <asp:ListItem Value="no" Text="No"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid_HeaderStyle">
                                        <td>Prior RMD's Exist<%--Prior RMD Exist--%> <%--MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and changed "RMD" to "RMD's"--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlPriorRMD" CssClass="LstFilter">
                                                <asp:ListItem Value="all" Selected="True" Text="Any"></asp:ListItem>
                                                <asp:ListItem Value="yes" Text="Yes"></asp:ListItem>
                                                <asp:ListItem Value="no" Text="No"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid_HeaderStyle">
                                        <td>Enrolled - Annual RMD  <%--  SB | 30/04/2018 | YRS-AT-3809 | Renamed from "Not Enrolled - Annual RMD " to "Enrolled-Annual RMD" removed the word "Not" --%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlNotEnrolled" CssClass="LstFilter">
                                                <asp:ListItem Value="all" Selected="True" Text="Any"></asp:ListItem>
                                                <asp:ListItem Value="yes" Text="Yes"></asp:ListItem>
                                                <asp:ListItem Value="no" Text="No"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid_HeaderStyle">
                                        <td>Year
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlNonEligibleRMDYear" CssClass="LstFilter"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid_HeaderStyle">
                                        <td>Pending Request
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlPendReq" CssClass="LstFilter">
                                                <asp:ListItem Value="all" Text="Any" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="yes" Text="Yes"></asp:ListItem>
                                                <asp:ListItem Value="no" Text="No"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button runat="server" ID="btnView" Text="View >>" CssClass="Button_Normal" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td valign="top" class="Table_WithBorder">
                            <div style="OVERFLOW: auto; WIDTH: 100%; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 450px; BORDER-BOTTOM-STYLE: none">
                                <asp:GridView runat="server" ID="gvNonEligible" CssClass="DataGrid_Grid" EmptyDataText="No records found."
                                    AllowSorting="true" AutoGenerateColumns="false" SelectedRowStyle-VerticalAlign="Top" Width="100%">
                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                    <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                    <Columns>
                                        <%--<asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox runat="server" ID="chkSelectAllNonEligible" onclick="chkall(this.checked,'gvNonEligible')" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkRMDProcessNonEligible" onclick="Unckeck(this,'gvNonEligible')" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="5%" />
                                        </asp:TemplateField>--%>
                                        <asp:BoundField DataField="FundIdNo" SortExpression="FundIdNo" HeaderText="Fund No." HeaderStyle-Width="10%" ItemStyle-Width="50px" />
                                        <asp:BoundField DataField="PlanType" SortExpression="PlanType" HeaderText="Plan Type" HeaderStyle-Width="10%" ItemStyle-Width="50px" />
                                        <asp:BoundField DataField="CurrentBalance" SortExpression="CurrentBalance" HeaderText="Curr. Bal" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="MRDAmount" SortExpression="MRDAmount" HeaderText="RMD Amt" HeaderStyle-Width="8%" ItemStyle-Width="115px" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="PaidAmount" SortExpression="PaidAmount" HeaderText="Paid Amt" HeaderStyle-Width="12%" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="StatusTypeDescription" SortExpression="StatusTypeDescription" HeaderStyle-Width="12%" HeaderText="Fund Status" />
                                        <asp:BoundField DataField="FundEventID" HeaderText="FundEventID" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="MRDYear" HeaderText="Year" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="Process_Status" HeaderText="Process Status" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="MRDExpireDate" SortExpression="MRDExpireDate" HeaderText="Due Date" HeaderStyle-Width="8%" />
                                        <asp:BoundField DataField="PerssID" HeaderText="Pers ID" HeaderStyle-Width="15%" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="bitAnnualMRDRequested" HeaderText="bitAnnualMRDRequested" HeaderStyle-Width="15%" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="IsLocked" HeaderText="Locked" SortExpression="IsLocked" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="IsBlocked" HeaderText="Blocked" SortExpression="IsBlocked" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="IsMultipleYearMRD" HeaderText="IsMultipleYearMRD" SortExpression="IsMultipleYearMRD" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
                                        <asp:BoundField DataField="PendingRequest" HeaderText="PendingRequest" SortExpression="PendingRequest" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr class="hide">
                        <td colspan="2">
                            <div>
                                <table align="center">
                                    <tr>
                                        <td align="left" class="Label_Small">
                                            <span class="BG_ColourIsLocked">&nbsp;&nbsp;</span> <span class="Label_Small">- Account locked.</span>
                                        </td>
                                        <td align="left" class="Label_Small">
                                            <span class="BG_ColourIsBlocked">&nbsp;&nbsp;</span> <span class="Label_Small">- Insufficient balance.</span>
                                        </td>
                                        <td align="left" class="Label_Small">
                                            <span class="BG_ColourInitialLetter">&nbsp;&nbsp;</span> <span class="Label_Small">- Initial Letter Processed.</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label_Small">
                                            <span class="BG_ColourIsMultipleYearMRD">&nbsp;&nbsp;</span> <span class="Label_Small">- Prior period RMD's
                                    exists.</span>
                                        </td>
                                        <td colspan="2" class="Label_Small">
                                            <span class="BG_ColourIsNotEnrollAnnualMRDPayments">&nbsp;&nbsp;</span> <span class="Label_Small">- Not Enroll
                                    For Annual RMD Payment.</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <table class="Table_WithBorder" id="tblRMDProcessStatus" cellspacing="1" cellpadding="1" width="100%" runat="server" visible="false">
                    <tr>
                        <td style="vertical-align: top; width: 15%; text-align: left" class="Table_WithBorder">
                            <div style="overflow: auto; width: 100%; border-top-style: none; border-right-style: none; border-left-style: none; height: 420px; border-bottom-style: none">
                                <table class="Table_WithBorder" id="Table4" cellspacing="1" cellpadding="1" border="0" style="width:100%">
                                    <tr class="DataGrid_AlternateStyle">
                                        <td>
                                            <%--START: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and changed text--%>
                                            <%--<b>RMD BatchID(s)</b>--%>
                                            <b>Filter Criteria</b>
                                            <%--END: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and changed text--%>
                                        </td>
                                    </tr>
                                    <%--START: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and Added new controls for search criteria --%>
                                    <%--<asp:Repeater ID="rptRMDBatchId" runat="server">
                                        <ItemTemplate>
                                            <tr valign="top">
                                                <td id="liRMDProcessBatchId" runat="server" style="padding-bottom: 10px; list-style-type: none; text-align: left; width: 100%;">
                                                    <asp:LinkButton runat="server" ID="lnkRMDProcessBatchId" Text='<%# Eval("BatchId")%>' OnClick="lnkRMDProcessBatchId_Click" CssClass="Link_SmallBold"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>--%>
                                    <tr class="DataGrid_HeaderStyle">
                                        <td>
                                            <asp:Label ID="labelYears" Text="RMD Year" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList runat="server" ID="dropdownlistRMDYear" CssClass = "DropDown_Normal" style="width:100%;" AutoPostBack="true"></asp:DropDownList> <%--MMR | 2016.10.19 | YRS-AT-2922 | Called function on dropdown change event--%>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid_HeaderStyle">
                                        <td>
                                            <asp:Label ID="labelMonths" Text="Due Dates" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList runat="server" ID="dropdownlistRMDMonth" CssClass = "DropDown_Normal" style="width:100%;" DataTextFormatString="{0:MM/dd/yyyy}" AutoPostBack="true"></asp:DropDownList> <%--MMR | 2016.10.19 | YRS-AT-2922 | Called function on dropdown change event--%>
                                        </td>
                                    </tr>    
                                    <tr class="DataGrid_HeaderStyle">
                                        <td>Batch Id
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList runat="server" ID="dropdownBatchId" CssClass ="DropDown_Normal" style="width:100%;" EnableViewState="true">                                                                                                                                                                                            
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr></tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button runat="server" ID="buttonSearch" Text="View >>" CssClass="Button_Normal" />
                                        </td>
                                    </tr>   
                                    <%--END: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and Added new controls for search criteria--%>
                                </table>
                            </div>
                        </td>
                        <td class="Table_WithBorder" valign="top">
                            <div style="overflow: auto; width: 100%; border-top-style: none; border-right-style: none; border-left-style: none; height: 430px; border-bottom-style: none;">
                                <table class="Table_WithBorder" border="0" width="100%">
                                    <tr style="height:5%;"> <%--MMR | 2016.10.19 | YRS-AT-2922 | Defined height property--%>
                                        <td class="Td_ButtonContainer">RMD Batch Process Status:
                                        </td>
                                    </tr>
                                    <tr style="height: 62%" valign="top"> <%--MMR | 2016.10.19 | YRS-AT-2922 | Changed the height from 200px to 62%--%>
                                        <td>
                                            <asp:GridView ID="gvProcessBatchStatus" Width="100%" runat="server" CssClass="DataGrid_Grid" AllowSorting="true" PageSize="13" PagerSettings-Mode="Numeric" AllowPaging="true" EmptyDataText="No records found." AutoGenerateColumns="false"> <%--MMR | 2016.10.19 | YRS-AT-2922 | Changed the page size--%>
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                                <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                                <Columns>
                                                    <asp:BoundField DataField="FUNDNo" SortExpression="FUNDNo" HeaderText="Fund No." HeaderStyle-Width="7%" ItemStyle-Width="50px" />
                                                    <asp:BoundField DataField="PlanType" SortExpression="PlanType" HeaderText="Plan Type" HeaderStyle-Width="7%" ItemStyle-Width="50px" />
                                                    <asp:BoundField DataField="CurrentBalance" SortExpression="CurrentBalance" HeaderText="Cur. Balance" HeaderStyle-Width="8%" />
                                                    <asp:BoundField DataField="MRDAmount" SortExpression="MRDAmount" HeaderText="RMD Amt" HeaderStyle-Width="7%" />
                                                    <asp:BoundField DataField="StatusTypeDescription" SortExpression="StatusTypeDescription" HeaderText="Status" HeaderStyle-Width="8%" />
                                                    <%--START: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and changed datafield to show process status--%>
                                                    <%--<asp:BoundField DataField="Process_Status" SortExpression="Process_Status" HeaderText="Process Status" HeaderStyle-Width="20%" ItemStyle-Width="30%" />--%>
                                                    <asp:BoundField DataField="DisplayRemarks" SortExpression="DisplayRemarks" HeaderText="Process Status" HeaderStyle-Width="20%" ItemStyle-Width="30%" />
                                                    <%--END: MMR | 2016.10.19 | YRS-AT-2922 | Commented existing code and changed datafield to show process status--%>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr style="height:5%;"> <%--MMR | 2016.10.19 | YRS-AT-2922 | Defined height property--%>
                                        <td class="Td_ButtonContainer">Exception List:
                                        </td>
                                    </tr>
                                    <tr style="height: 28%" valign="top"> <%--MMR | 2016.10.19 | YRS-AT-2922 | Changed the height from 170px to 28%--%>
                                        <td>
                                            <asp:GridView ID="gvRMDBatchErrorList" Width="100%" runat="server" CssClass="DataGrid_Grid" PageSize="4" PagerSettings-Mode="Numeric" AllowPaging="true" AllowSorting="true" EmptyDataText="No records found." AutoGenerateColumns="false"> <%--MMR | 2016.10.19 | YRS-AT-2922 | Changed the page size--%>
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                                <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                                <Columns>
                                                    <asp:BoundField DataField="FundNo" SortExpression="FundNo" HeaderText="Fund No." HeaderStyle-Width="10%" ItemStyle-Width="50px" />
                                                    <asp:BoundField DataField="Errors" SortExpression="Errors" HeaderText="Errors" HeaderStyle-Width="40%" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" ControlStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description" HeaderStyle-Width="50%" ItemStyle-Width="30%" />
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <table class="Table_WithOutBorder" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="Td_ButtonContainer" align="left" width="20%">
                            <asp:Button runat="server" ID="btnCloseCurrentMRD" Text="Close Current RMD..." CssClass="Button_Normal" Style="width: 150px;" Visible="false" />
                            &nbsp;&nbsp;
                        </td>
                        <td class="Td_ButtonContainer" align="right">
                            <asp:Button runat="server" ID="btnPrintReport" Text="Print List" CssClass="Button_Normal" />
                            &nbsp;&nbsp;
                            <asp:Button runat="server" ID="btnProcess" Text="Disburse" CssClass="Button_Normal" Style="width: 150px;" />
                            &nbsp;&nbsp;
                            <asp:Button runat="server" ID="btnCancel" Text="Close" CssClass="Button_Normal" Style="width: 80px;" />
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="ConfirmDialog" title="RMD Print Letter" runat="server" style="overflow: visible;">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <div>
                    <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage" CssClass="Label_Small" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom">

                                <asp:Button runat="server" ID="btnYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                    OnClientClick="javascript: closeConfirmdialog();" />&nbsp;
                                    <asp:Button runat="server" ID="btnNo" Text="No" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        OnClientClick="javascript: closeConfirmdialog();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel runat="server" ID="updatepnl">
        <ContentTemplate>
            <div id="modalRMD" style="display: none;">
                <table style="width: 100%;">
                    <tr>
                        <td class="Label_Small">Please do not close this window until all activities are complete.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="dvMessage" runat="server"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="dvShowStatus" runat="server" class="Label_Small"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="dvStatusMsg" runat="server" class="Label_Small"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" align="center" width="99%" cssclass="DataGrid_Grid" id="tblIDMDetails">
                                <tr>
                                    <td colspan="3" align="center" class="Label_Small">
                                        <u>Activities Performed</u>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
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
                                    <td class="DataGrid_AlternateStyle" width="55%" valign="middle" style="vertical-align: middle; background-color: White">1. Request and Processing
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="background-color: White; width: 15%;">
                                        <table>
                                            <tr>
                                                <td class="DataGrid_AlternateStyle" style="background-color: White; text-align: center">
                                                    <asp:Image ID="imgRegComplete" runat="server" ImageUrl="~/images/complete.jpg" AlternateText="Complete"
                                                        CssClass="hide" Width="20" Height="20" />
                                                    <div id="dvReg" runat="server" class="info-Progress">
                                                        In-Progress
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White; width: 30%">
                                        <div runat="server" id="dvReqProgress" align="center" style="vertical-align: middle">
                                            In-Process
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="DataGrid_AlternateStyle" valign="middle" style="vertical-align: middle; background-color: White">2. Generate IDX document(s)
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="background-color: White">
                                        <table>
                                            <tr>
                                                <td class="DataGrid_AlternateStyle" style="background-color: White; text-align: center">
                                                    <asp:Image ID="imgIDXComplete" runat="server" ImageUrl="~/images/complete.jpg" AlternateText="Complete"
                                                        CssClass="hide" Width="20" Height="20" />
                                                    <div id="dvIDX" runat="server" class="info-Progress">
                                                        In-Progress
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White">
                                        <div runat="server" id="dvIDXProgress" align="center" style="vertical-align: middle">
                                            In-Process
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="DataGrid_AlternateStyle" valign="middle" style="vertical-align: middle; background-color: White">3. Generate PDF document(s)
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="background-color: White">
                                        <table>
                                            <tr>
                                                <td class="DataGrid_AlternateStyle" style="background-color: White; text-align: center">
                                                    <asp:Image ID="imgPDFComplete" runat="server" ImageUrl="~/images/complete.jpg" AlternateText="Complete"
                                                        CssClass="hide" Width="20" Height="20" />
                                                    <div id="dvPDF" runat="server" class="info-Progress">
                                                        In-Progress
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White">
                                        <div runat="server" id="dvPDFProgress" align="center" style="vertical-align: middle">
                                            In-Process
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="DataGrid_AlternateStyle" valign="middle" style="vertical-align: middle; background-color: White">4. Copy Generated IDX and PDF document(s) to IDM
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="background-color: White">
                                        <table>
                                            <tr>
                                                <td class="DataGrid_AlternateStyle" style="background-color: White; text-align: center">
                                                    <asp:Image ID="imgCopiedComplete" runat="server" ImageUrl="~/images/complete.jpg"
                                                        AlternateText="Complete" CssClass="hide" Width="20" Height="20" />
                                                    <div id="dvCopy" runat="server" class="info-Progress">
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
                    <tr id="trshowErrors" class="hide">
                        <td>
                            <div>
                                <asp:Label ID="lblException" runat="server" class="Label_Small" Text="One or more errors where encounter while processing this batch. Please click here ">
                                    <asp:LinkButton ID="lnkShowError" runat="server" ForeColor="#3399ff" OnClientClick="ExceptionShowError(); return false;">view details</asp:LinkButton></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" ID="btnClose" Text="Close" CssClass="Button_Normal" Style="width: 60px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" />
                        </td>
                    </tr>
                </table>
            </div>

            <div style="display: none; overflow: auto" id="dvShowErrorsMsg">
                <div id="divPopErrorMsg">
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnProcess" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
