<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CashOut.aspx.vb" Inherits="YMCAUI.CashOut"
    MasterPageFile="~/MasterPages/YRSMain.Master" ValidateRequest="false" EnableEventValidation="false" Trace="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="DataPagerFindInfo" TagName="DataGridPager" Src="UserControls/DataGridPager.ascx" %>
<%--Start: BT-2324: YRS 5.0-2267 - Changes to Cashout master page--%>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <title>CashOutForm</title>
    <script type="text/javascript" src="JS/YMCA_JScript.js"></script>
    <%--<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">--%>
   <%-- <style id="Style1" type="text/css" media="screen" runat="server">
        @import '<%= ResolveUrl("~/JS/jquery-ui/base/jquery.ui.all.css")%>';
    </style>
    <style id="Style2" type="text/css" media="screen" runat="server">
        @import '<%= ResolveUrl("~/CSS/CustomStyleSheet.css")%>';
    </style>
    <script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script>--%>
    <%--<link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />--%>
    <script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <style type="text/css">
        /*START: SG: 2012.08.28: BT-960*/
        .BG_ColourIsDateDiffer {
            background-color: #F08080; /*LightCoral*/
        }

        .BG_ColourIsRMDEligible {
            background-color: #ADFF2F; /*GreenYellow*/
        }
        /*END: SG: 2012.08.28: BT-960*/
        .style1 {
            text-decoration: underline;
        }

        .BG_Colour6070 {
            background-color: #90EE90; /*LightGreen*/
        }

        .BG_Colour7180 {
            background-color: #FFFF00; /*Yellow*/
        }

        .BG_Colour8190 {
            background-color: #FF0000; /*Red*/
        }

        .td_Batch {
            background: #93BEEE;
            font-family: Verdana;
            font-size: 8pt;
            font-weight: bolder;
            height: 10pt;
        }

        .linkbutton {
            color: Black;
            font-family: Verdana;
            font-size: 8pt;
            font-weight: bolder;
            height: 10pt;
        }

        .hide {
            display: none;
        }

        .show {
            display: block;
        }
    </style>
    <script type="text/javascript" language="javascript">
        //Start: Dinesh Kanojia         2014.02.12          BT-2298: YRS 5.0-2259 - Optimizing the Cash out process to improve its performance.


        var winHeight = $(window).height();
        var winWidth = $(window).width();
        //Below function will show and hide counter button from various button events.
        function ShowHideButton(isShow) {
            document.getElementById("btnSelectten").style.display = isShow;
            document.getElementById("btnSelect50").style.display = isShow;
            document.getElementById("btnSelect100").style.display = isShow;
        }


        //Below Jquery code will open modal popup.
        var CloseDisable = false;
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    args.set_errorHandled(true);
                    BindEvents();

                }
            }
            BindEvents();

        });

        function BindEvents() {
            //$('#modalcashout').dialog({
            //    autoOpen: false,
            //    draggable: true,
            //    show: "fade",
            //    hide: "fade",
            //    modal: true,
            //    close: false,
            //    title: "Cash Out - Batch creation status.",
            //    open: function (type, data) {
            //        $(this).parent().appendTo("form");
            //        $('a.ui-dialog-titlebar-close').remove();
            //    },
            //    width: 800,
            //    minheight: 400,
            //    closeOnEscape: false,
            //    //position: ["center","center"],
            //    beforeclose: function () {
            //        return CloseDisable;
            //    }

            //});
        }

        //Below function will set frame visible true when modal popup gets called from code behind.
        //function showPanel(panelID) {
        //    $('#' + panelID).css({ display: "block" });
        //    $('#' + panelID).dialog({ modal: true });
        //    $('#' + panelID).dialog("open");
        //    //            var top, left
        //    //            top = (winHeight - 400) / 2;
        //    //            left = (winWidth - 600) / 2;
        //    //            $('#' + panelID).css({ 'top': top, 'left': left });
        //    $('#frmCashout').attr("visibility", "visible");
        //    $('#frmCashout').attr("height", "300");
        //    $('#frmCashout').attr("width", "750");
        //    $('#frmCashout').attr("src", "CashOutBatchRequestCreation.aspx");
        //}

        //Below function will close the modal popup
        //function closeDialog() {
        //    $('#modalCashOut').dialog('close');
        //    window.location.href = "cashout.aspx";
        //}

        //Code Changed by Dinesh Kanojia to resolve Sys undefined exception.
        //BT: 2629:Cash Out or any other batch process: Progress/loading image to be shown
        function closeDialog() {
            $(document).ready(function () { $('#modalCashOut').dialog('close'); window.location.href = "cashout.aspx"; });
        }

        //Below line of commented for counter buttons. 

        //        window.onload = function () {
        //            var f = document.getElementById("DataGridCashOut");
        //            if (f == null) {
        //                document.getElementById('btnSelectten').style.display = "none";
        //                document.getElementById('btnSelect50').style.display = "none";
        //                document.getElementById('btnSelect100').style.display = "none";
        //                document.getElementById('btnSelect500').style.display = "none";
        //                return;
        //            }
        //            else {
        //                document.getElementById('btnSelectten').style.display = "";
        //                document.getElementById('btnSelect50').style.display = "";
        //                document.getElementById('btnSelect100').style.display = "";
        //                document.getElementById('btnSelect500').style.display = "none";
        //            }
        //        }

        //Below function will selected the records from gridview and update the counter.
        function CallJSCheck(intCount) {

            var f = document.getElementById("DataGridCashOut");
            if (f == null) {
                return;
            }
            var iCount = intCount;
            if (iCount > f.getElementsByTagName("input").length) {
                iCount = f.getElementsByTagName("input").length;
            }

            for (var i = 0; i < f.getElementsByTagName("input").length; i++) {
                if (f.getElementsByTagName("input").item(i).type == "checkbox") {
                    if (f.getElementsByTagName("input").item(i).disabled == false) {
                        if (i >= iCount) {
                            f.getElementsByTagName("input").item(i).checked = false;
                        }
                        else {
                            f.getElementsByTagName("input").item(i).checked = true;
                        }
                    }
                }
            }

            document.getElementById("ButtonUpdateCounter").click();

        }

        //End: Dinesh Kanojia         2014.02.12          BT-2298: YRS 5.0-2259 - Optimizing the Cash out process to improve its performance.

        $(document).ready(function () {
            //            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            //            function EndRequest(sender, args) {
            //                if (args.get_error() == undefined) {

            //                }
            //            }

            $('#ConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                width: 350, height: 250,
                title: "Cash Out",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });


            $('#dvConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                width: 350, height: 350,
                title: "Cashout Batch Request Creation",
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

        });

        function showDialog(id, text, btnokvisibility) {
            $('#' + id).dialog({
                resizable: false,
                height: 150,
                modal: true,
                buttons: { Ok: function () { $(this).dialog("close"); return false; } }
            });
            $('#lblMessage').text(text)
            $('#' + id).dialog("open");
        }

        function OpenProgressPopup() {
            $('#modalCashOut').dialog({
                autoOpen: false,
                draggable: true,
                show: "fade",
                hide: "fade",
                modal: true,
                close: false,
                title: "CashOut - Batch Creation Status",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                },
                width: 900,
                height: 700,
                display: "block",
                closeOnEscape: false
            });
            // buttons: [{ text: "Close", click: ClosePrintDialog }]
            $('#modalCashOut').dialog("open");

        }
        function ClosePrintDialog() {
            $('#modalCashOut').dialog('close');
        }

        function CallArrErrorDataList(strBatchId, strModule) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "FT/CopyFilestoFileServer.aspx/ArrErrorDataList",
                data: "{'strBatchId': '" + strBatchId + "','strModule':'" + strModule + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d == "ArrErrorDataList table not found in database.")
                    {
                        $('#divPopErrorMsg').html("Some internal error occured while creating this batch.");
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
        var CashOutBatchProcessCounter
        function CopyFilestoFileServer(strOR, strDEL, strBatchId, strModule) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "FT/CopyFilestoFileServer.aspx/CopytoFileServer",
                data: "{'strOR':'" + strOR + "','strDEL':'" + strDEL + "','strBatchId':'" + strBatchId + "','strModule':'" + strModule + "'}",
                dataType: "json",
                success: function (data) {
                    //$('#dvDsiplay').html(data.d);
                    CashOutBatchProcessCounter.d.iPdfCopied = data.d.iIdxCopied;
                    CashOutBatchProcessCounter.d.iIdxCopied = data.d.iPdfCopied;
                    if (data.d.strStatus == 'error') {
                        //$('#dvDsiplay').html(data.d.strError);
                        $('#dvStatusMsg').text("Some IDX or PDF files were not copied to IDM server.");
                        CallArrErrorDataList(strBatchId, strModule)
                        $("#trshowErrors")[0].className = "show";
                        UpdateDisplay(CashOutBatchProcessCounter);
                        $('#dvCopy').text("Error");
                        $('#dvCopy')[0].className = "error-Progress";
                    }
                    else {
                        UpdateDisplay(CashOutBatchProcessCounter);

                        if (CashOutBatchProcessCounter.d.iTotalIDXPDFCount < CashOutBatchProcessCounter.d.iTotalCount) {
                            //$('#dvStatusMsg').text("Some of the records were not proccessed due to problem, please click on View Errors link for more information.");
                            CallArrErrorDataList(strBatchId, strModule)
                            $("#trshowErrors")[0].className = "show";
                            UpdateDisplay(CashOutBatchProcessCounter);
                            $('#dvCopy').text("Error");
                            $('#dvCopy')[0].className = "error-Progress";
                        }
                        else if (CashOutBatchProcessCounter.d.iPdfCreated < CashOutBatchProcessCounter.d.iTotalIDXPDFCount || CashOutBatchProcessCounter.d.iIdxCreated < CashOutBatchProcessCounter.d.iTotalIDXPDFCount) {
                            //$('#dvStatusMsg').text("Some IDX or PDF files were not get created.");
                            CallArrErrorDataList(strBatchId, strModule)
                            $("#trshowErrors")[0].className = "show";
                            UpdateDisplay(CashOutBatchProcessCounter);
                            $('#dvCopy').text("Error");
                            $('#dvCopy')[0].className = "error-Progress";
                        }
                        else if (CashOutBatchProcessCounter.d.iPdfCopied == CashOutBatchProcessCounter.d.iTotalIDXPDFCount && CashOutBatchProcessCounter.d.iIdxCopied == CashOutBatchProcessCounter.d.iTotalIDXPDFCount) {
                            $('#dvStatusMsg').text("CashOut Batch Request Creation Process completed sucessfully.");

                            $('#dvCopy').text("Completed");
                            $('#dvPDF').text("Completed");
                            $('#dvIDX').text("Completed");
                            $('#dvReg').text("Completed");

                            $('#dvPDF')[0].className = "success-Progress";
                            $('#dvIDX')[0].className = "success-Progress";
                            $('#dvReg')[0].className = "success-Progress";
                            $('#dvCopy')[0].className = "success-Progress";

                        }
                        else if (CashOutBatchProcessCounter.d.iTotalIDXPDFCount == CashOutBatchProcessCounter.d.iTotalCount) {
                            $('#dvStatusMsg').text("CashOut Batch Request Creation Process completed sucessfully.");

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
                            //$('#dvStatusMsg').text("Internal exception.");
                            CallArrErrorDataList(strBatchId, strModule)
                            $("#trshowErrors")[0].className = "show";
                            UpdateDisplay(CashOutBatchProcessCounter);
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
                    UpdateDisplay(CashOutBatchProcessCounter);
                    $("#trshowErrors")[0].className = 'show';
                    CallArrErrorDataList(strBatchId, strModule);
                }
            });
        }
        function CallProcess(strbatchId, strReportType, strModule, strCashOutType) {
            OpenProgressPopup();
            CallRMDProcess(strbatchId, strReportType, strCashOutType, 0, "", 0, 0, strModule);
            // setTimeout('GetRMDProcessStatus(' + strbatchId + ')', 500);
        }

        //Added By Dinesh kanojia on :08/09/2014: BT:2644: YRS Releases 14.0.0,14.0.1,14.0.2,14.0.3 and 14.0.4 issue in cashout screen
        function openReportPrinter() {
            window.open('FT\\ReportPrinter.aspx', '', 'width=785,height=300, menubar=no, resizable=yes,top=200,left=150, scrollbars=yes, status=yes');
        }

        function CallRMDProcess(strbatchId, strReportType, strCashOutType, iCount, strProcessName, iIDXCreated, iPDFCreated, strModule) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "CashOut.aspx/CashOutBatchCreationProcess",
                data: "{'strBatchId': '" + strbatchId + "','strReportType':'" + strReportType + "','strCashOutType':'" + strCashOutType + "','iCount':" + iCount + ",'strProcessName':'" + strProcessName + "'," +
                    "'iIDXCreated':" + iIDXCreated + ",'iPDFCreated':" + iPDFCreated + ",'strModule':'" + strModule + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d.strretValue == "success") {
                        CopyFilestoFileServer(1, 1, strbatchId, strModule);
                        CashOutBatchProcessCounter = data;
                        $('#dvPDF').text("Completed");
                        $('#dvIDX').text("Completed");
                        $('#dvReg').text("Completed");

                        $('#dvPDF')[0].className = "success-Progress";
                        $('#dvIDX')[0].className = "success-Progress";
                        $('#dvReg')[0].className = "success-Progress";

                    }
                    else if (data.d.strretValue == "error") {
                        UpdateDisplay(data);
                        $('#dvMessage').text("Some error occured.");
                        $("#dvMessage")[0].className = "error-msg";
                        $("#trshowErrors")[0].className = 'show';
                        CallArrErrorDataList(strbatchId, strModule);
                        $('#dvPDF').text("Error");
                        $('#dvIDX').text("Error");
                        $('#dvReg').text("Error");

                        $('#dvPDF')[0].className = "error-Progress";
                        $('#dvIDX')[0].className = "error-Progress";
                        $('#dvReg')[0].className = "error-Progress";
                        $('#dvCopy')[0].className = "info-Progress";

                    }
                    else if (data.d.strretValue == "pending") {
                        CallRMDProcess(data.d.strBatchId, data.d.strReportType, strCashOutType, data.d.iProcessCount, data.d.strProcessName, data.d.iIdxCreated, data.d.iPdfCreated, strModule);
                        UpdateDisplay(data);
                        $('#dvPDF')[0].className = "info-Progress";
                        $('#dvIDX')[0].className = "info-Progress";
                        $('#dvReg')[0].className = "info-Progress";
                        $('#dvCopy')[0].className = "info-Progress";

                    }
                    else {
                        UpdateDisplay(data);
                        $('#dvMessage').text("Some error occured.");
                        $("#dvMessage")[0].className = "error-msg";
                        $("#trshowErrors")[0].className = 'show';
                        CallArrErrorDataList(strbatchId, strModule);
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
                    UpdateDisplay(data);
                    $('#dvMessage').text("Some network error occured.");
                    $("#dvMessage")[0].className = "error-msg";
                    CallArrErrorDataList(strbatchId, strModule);
                    $("#trshowErrors")[0].className = 'show';

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

        function showConfirmdialog() {
            $('#dvConfirmDialog').dialog("open");
        }

        function closeConfirmdialog() {
            $('#dvConfirmDialog').dialog('close');
        }

    </script>
    <%--<script type="text/javascript" language="javascript">

        //Code for checkbox Check all start
        var allCheckBoxSelector = '#<%=DataGridCashOut.ClientID%> input[id*="chkSelectAll"]:checkbox';
        var checkBoxSelector = '#<%=DataGridCashOut.ClientID%> input[id*="CheckBoxSelect"]:checkbox';

        function ToggleCheckUncheckAllOptionAsNeeded() {
            var totalCheckboxes = $(checkBoxSelector),
            checkedCheckboxes = totalCheckboxes.filter(":checked"),
            noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
            allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);

            if (totalCheckboxes.length == 0) allCheckboxesAreChecked = false;
            $(allCheckBoxSelector).attr('checked', allCheckboxesAreChecked);
        }

        $(document).ready(function () {
            $(allCheckBoxSelector).bind('click', function () {
                $(checkBoxSelector).attr('checked', $(this).is(':checked'));
                ToggleCheckUncheckAllOptionAsNeeded();
            });

            $(checkBoxSelector).bind('click', ToggleCheckUncheckAllOptionAsNeeded);
            ToggleCheckUncheckAllOptionAsNeeded();
        })
    </script>--%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="RMDPRintScriptManager" runat="server">
    </asp:ScriptManagerProxy>
    <table cellpadding="0" cellspacing="0" style="width: 100%;" align="center" class="Table_WithBorder">
        <tr>
            <td>
                <table style="width: 100%;" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" height="26" style="width: 50%; margin-left: 480px;">
                            <asp:Label ID="Label2" runat="server" CssClass="Label_Large">&nbsp;Amount</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="DropDownAmount" runat="server" CssClass="DropDown_Normal" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td align="right" style="width: 50%" cellspacing="0">
                            <iewc:TabStrip ID="CashoutTabStrip" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
                                TabHoverStyle="background-color:#93BEEE;color:#4172A9;text-align:center;" TabSelectedStyle="background-color:#93BEEE;color:#000000;text-align:center;"
                                Height="25px" Width="100%" Visible="false" TabIndex="0" SelectedIndex="0" OnSelectedIndexChange="CashoutTabStrip_SelectedIndexChange">
                                <iewc:Tab Text="Manage Batch" DefaultStyle="Width: 50%;"></iewc:Tab>
                                <iewc:Tab Text="Batch Details" DefaultStyle="Width: 50%;"></iewc:Tab>
                            </iewc:TabStrip>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%;" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <iewc:MultiPage ID="CashoutMultiPage" runat="server" Width="100%">
                                <iewc:PageView>
                                    <table width="100%" align="center" cellpadding="2" cellspacing="0">
                                        <tr id="trBatchHeader" runat="server" visible="false">
                                            <td id="tdBatchPanel" runat="server" class="td_Batch" style="width: 50%;" align="left">List of Processed Batches
                                            </td>
                                            <td align="right" class="td_Batch" style="width: 50%;">
                                                <asp:LinkButton ID="LinkProcessBatch" runat="server" CssClass="linkbutton" Text="View Processed Batches" />
                                                <asp:LinkButton ID="LinkUnprocessBatch" runat="server" CssClass="linkbutton" Text="View Unprocessed Batches"
                                                    Visible="false" />&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Label ID="LabelNoBatch" runat="server" CssClass="Label_Small" Visible="false">No Records Found</asp:Label>
                                                <div style="overflow: auto; width: 100%; border-top-style: none; border-right-style: none; border-left-style: none; height: 350px; border-bottom-style: none">
                                                    <asp:GridView ID="GridViewCashoutBatch" runat="server" CssClass="DataGrid_Grid" AutoGenerateColumns="False"
                                                        AllowSorting="true" Width="97%" AllowPaging="true" DataKeyNames="chvBatchId"
                                                        PageSize="15">
                                                        <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                        <RowStyle CssClass="DataGrid_NormalStyle" />
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                                        <Columns>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="ImageSelect" runat="server" ToolTip="Select" ImageUrl="images\select.gif"
                                                                        AlternateText="Select"></asp:Image>
                                                                    <%-- <asp:Image ID="ImageSelected" runat="server" ToolTip="Select" ImageUrl="images\selected.gif" Visible="false" AlternateText="Select"></asp:Image>--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="chvBatchId" SortExpression="chvBatchId" HeaderText="Batch ID" />
                                                            <asp:BoundField DataField="CreatedDate" SortExpression="CreatedDate" HeaderText="Creation Date"
                                                                DataFormatString="{0:d}" />
                                                            <asp:BoundField DataField="ExpireDate" SortExpression="ExpireDate" HeaderText="Expiry Date"
                                                                DataFormatString="{0:d}" />
                                                            <asp:BoundField DataField="BatchCount" SortExpression="BatchCount" HeaderText="No of Participants" />
                                                            <asp:BoundField DataField="BatchCountLookup" SortExpression="BatchCountLookup" HeaderText="No of Participants" />
                                                            <asp:BoundField DataField="ProcessedCount" SortExpression="ProcessedCount" HeaderText="No. of Requests Processed" />
                                                            <asp:BoundField DataField="ProcessedDate" SortExpression="ProcessedDate" HeaderText="Processed Date"
                                                                DataFormatString="{0:d}" />
                                                            <asp:ButtonField ButtonType="Link" Text="Initiate Process" CommandName="Process"
                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="DaysCreatedDiff" />
                                                            <%--<asp:ButtonField ButtonType="Link" Text="View" HeaderText="Report" CommandName="ViewReport"
                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                            <asp:ButtonField ButtonType="Link" Text="Print Forms/Letters" HeaderText="" CommandName="Print"
                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />--%>
                                                            <asp:TemplateField ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ImageButton1" CommandName="ViewReport" ToolTip="View Report"
                                                                        CommandArgument='<%#Eval("chvBatchId")%>' runat="server" ImageUrl="~/images/view.gif" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ImageButton2" CommandName="Print" ToolTip="Print Forms/Letters"
                                                                        runat="server" CommandArgument='<%#Eval("chvBatchId")%>' ImageUrl="~/images/printico.gif" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ImageButton3" CommandName="View" ToolTip="View Forms/Letters"
                                                                        runat="server" CommandArgument='<%#Eval("chvBatchId")%>' ImageUrl="~/images/view.gif" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerSettings Mode="NumericFirstLast" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trColorMessage">
                                            <td align="center" colspan="2"  class="Label_Small">
                                                <span class="BG_Colour6070">&nbsp;&nbsp;&nbsp;</span><span class="Label_Small"> 60 to 70 Days older batches </span>
                                                <span class="BG_Colour7180">&nbsp;&nbsp;&nbsp;</span><span class="Label_Small"> 71 to 80 Days older batches </span>
                                                <span class="BG_Colour8190"> &nbsp;&nbsp;&nbsp;</span> <span class="Label_Small"> 81 to 90 Days older batches </span>
                                                <br />
                                                <asp:Label ID="Label5" runat="server" CssClass="Label_Small" Text="PAST DUE More than 91 days and older batches" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" class="Table_WithBorder" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="Td_ButtonContainer" align="center" width="50%">
                                                <asp:Button ID="ButtonCreateBatch" CssClass="Button_Normal" runat="server" Text="Create Batch" />
                                            </td>
                                            <td class="Td_ButtonContainer" align="center" width="50%">
                                                <asp:Button ID="ButtonOkBatch" CssClass="Button_Normal" Text="OK" Width="100px" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <asp:UpdatePanel runat="server" ID="upParticipant" RenderMode="Inline" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%" align="center" cellpadding="2" cellspacing="0">
                                                <tr id="trListHeader" runat="server" visible="false">
                                                    <td id="tdParticipantPanel" runat="server" class="td_Batch" style="width: 50%;" align="left">
                                                        <div id="test" runat="server">
                                                            &nbsp;Filter&nbsp;<asp:Image ID="buttonFilter" runat="server" ImageUrl="~/images/arrow.gif" />
                                                            &nbsp;<asp:Label ID="labelFilter" runat="server"></asp:Label>
                                                            <cc2:HoverMenuExtender ID="hme2" runat="Server" TargetControlID="test" PopupControlID="PopupMenu"
                                                                HoverCssClass="popupHover" PopupPosition="Bottom" OffsetX="0" OffsetY="0" PopDelay="50">
                                                            </cc2:HoverMenuExtender>
                                                            <asp:Panel CssClass="popupMenu" ID="PopupMenu" runat="server" HorizontalAlign="Left"
                                                                BackColor="#93BEEE">
                                                                <table cellpadding="2">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="linkAll" runat="server" Text="All" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="linkHighlighted" runat="server" Text="Highlighted" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="linkPlain" runat="server" Text="Non-Highlighted" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </div>
                                                    </td>
                                                    <td align="right" class="td_Batch" style="width: 50%;">
                                                        <asp:LinkButton ID="LinkEligibleList" runat="server" Text="View Eligible Participants"
                                                            CssClass="linkbutton" Visible="false" />
                                                        <asp:LinkButton ID="LinkNotEligibleList" runat="server" Text="View Not Eligible Participants"
                                                            CssClass="linkbutton" Visible="false" />&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:Label ID="LabelNoRecords" runat="server" CssClass="Label_Small" Visible="false">No Records Found</asp:Label>
                                                        <div style="overflow: auto; width: 100%; border-top-style: none; border-right-style: none; border-left-style: none; height: 330px; border-bottom-style: none">
                                                            <asp:DataGrid ID="DataGridCashOut" runat="server" CssClass="DataGrid_Grid" AutoGenerateColumns="False"
                                                                AllowSorting="true" Width="97%">
                                                                <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                                <%--<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>--%>
                                                                <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:TemplateColumn ItemStyle-Width="28px">
                                                                        <%--<HeaderTemplate>
                                                                            <asp:CheckBox ID="chkSelectAll" runat="server"></asp:CheckBox>
                                                                        </HeaderTemplate>--%>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="CheckBoxSelect" runat="server"></asp:CheckBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="SSNO" SortExpression="SSNO" HeaderText="SSNo" HeaderStyle-HorizontalAlign="Center"
                                                                        Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="FUNDNo" SortExpression="FUNDNo" HeaderText="Fund Id"
                                                                        ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="LastName" SortExpression="LastName" HeaderText="Last Name"
                                                                        HeaderStyle-HorizontalAlign="Center" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="FirstName" SortExpression="FirstName" HeaderText="Name"
                                                                        HeaderStyle-HorizontalAlign="Center" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-Width="120px"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="StatusType" SortExpression="StatusType" HeaderText="Fund Status"
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="IsVested" SortExpression="IsVested" HeaderText="Vested"
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="MaxTermDate" SortExpression="MaxTermDate" HeaderText="Latest Termn."
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60px"
                                                                        DataFormatString="{0:d}"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="LastContributionDate" SortExpression="LastContributionDate"
                                                                        HeaderText="Latest Rcvd. Cont." HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="90px"
                                                                        ItemStyle-HorizontalAlign="Left" DataFormatString="{0:d}"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="EligibleBalance" SortExpression="EligibleBalance" HeaderText="Total Amt." 
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px"></asp:BoundColumn> <%--SR|2015.10.09| YRS-AT-2463 - "Plan Amt." column renamed to "Total Amt."--%>
                                                                    <asp:BoundColumn DataField="PlansType" SortExpression="PlansType" HeaderText="Plan Type"
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="50px"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="TaxableAmount" Visible="False"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="MiddleName" Visible="False"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="PersonAge" Visible="False"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="FundEventId" Visible="False"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="IsHighlighted" Visible="False"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="IntAddressId" Visible="False"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="RefRequestID" Visible="False"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="IsRMDEligible" Visible="False"></asp:BoundColumn>
                                                                    <%--<asp:BoundColumn DataField="bitCheckCashed" SortExpression="bitCheckCashed" HeaderText="Last Check Cashed"
	                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="CheckIssueDate" SortExpression="CheckIssueDate" HeaderText="Last Check Date"
	                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40px"
	                                                                    DataFormatString="{0:d}"></asp:BoundColumn>--%>
                                                                    <asp:BoundColumn DataField="Remarks" SortExpression="Remarks" HeaderText="Remarks"
                                                                        HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="mnyEstimatedBalance" SortExpression="mnyEstimatedBalance"
                                                                        HeaderText="Est. Bal." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                                        ItemStyle-Width="60px"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="PersonId" Visible="False"></asp:BoundColumn>
                                                                    <%--Start|SR|2015.10.09| YRS-AT-2463 - Added Two new columns(Retirement & Savings) to display eligible Balance in respective Account--%>
                                                                    <asp:BoundColumn DataField="mnyRetirementPlan" HeaderText="Ret. Bal."  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                                        ItemStyle-Width="60px" SortExpression="mnyRetirementPlan"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="mnySavingsPlan" HeaderText="Savings Bal."  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                                        ItemStyle-Width="60px" SortExpression="mnySavingsPlan"></asp:BoundColumn>
                                                                    <%--End|SR|2015.10.09| YRS-AT-2463 - Added Two new columns(Retirement & Savings) to display eligible Balance in respective Account--%>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <table width="100%" runat="server" id="tbUpdateCounter" cellpadding="2" cellspacing="0">
                                                            <tr>
                                                                <td align="left" width="30%">
                                                                    <asp:Label ID="LabelCashouts" runat="server" CssClass="Label_Small">&nbsp;&nbsp;&nbsp;Number of Cashouts Listed</asp:Label>
                                                                </td>
                                                                <td width="20%">
                                                                    <asp:TextBox ID="TextBoxCashoutList" runat="server" CssClass="TextBox_Normal_Amount"
                                                                        ReadOnly="True" Width="50px" Height="20px"></asp:TextBox>
                                                                </td>
                                                                <td align="left" width="30%">
                                                                    <asp:Label ID="LabelTotal" runat="server" CssClass="Label_Small">Total Gross Amount</asp:Label>
                                                                </td>
                                                                <td width="20%">
                                                                    <asp:TextBox ID="TextboxTotalAmt" runat="server" CssClass="TextBox_Normal_Amount"
                                                                        ReadOnly="True" Width="100px" Height="20px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" width="30%">
                                                                    <asp:Label ID="LabelSelectedCashouts" runat="server" CssClass="Label_Small">&nbsp;&nbsp;&nbsp;Number of Cashouts Selected</asp:Label>
                                                                </td>
                                                                <td width="20%">
                                                                    <asp:TextBox ID="TextBoxCshoutsSelected" runat="server" CssClass="TextBox_Normal_Amount"
                                                                        ReadOnly="True" Width="50px" Height="20px"></asp:TextBox>
                                                                </td>
                                                                <td align="left" width="30%">
                                                                    <asp:Label ID="LabelSelectedAmt" runat="server" CssClass="Label_Small">Total Gross Amount Selected</asp:Label>&nbsp;&nbsp;
                                                                </td>
                                                                <td width="20%">
                                                                    <asp:TextBox ID="TextboxTotalAmtSelected" runat="server" CssClass="TextBox_Normal_Amount"
                                                                        ReadOnly="True" Width="100px" Height="20px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <input type="button" id="btnSelectten" value="Select 50" onclick="CallJSCheck(50);"
                                                                        style="display: none" />
                                                                </td>
                                                                <td>
                                                                    <input type="button" id="btnSelect50" value="Select 100" onclick="CallJSCheck(100);"
                                                                        style="display: none" />
                                                                </td>
                                                                <td>
                                                                    <input type="button" id="btnSelect100" value="Select 500" onclick="CallJSCheck(500);"
                                                                        style="display: none" />
                                                                </td>
                                                                <td>
                                                                    <input type="button" id="btnSelect500" value="Select 500" style="display: none" onclick="CallJSCheck(500);" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <%--SG: 2012.08.28: BT-960 --%>
                                                <tr>
                                                    <td align="center" colspan="2" Class="Label_Small">
                                                         <%-- START : SB | 2016.01.17 | YRS-AT-3297 | Displaying records for all RMD elible persons so legend information is updated --%>
                                                         <%-- <span class="BG_ColourIsRMDEligible">&nbsp;</span> <span class="Label_Small"> RMD eligible for present year </span>--%>
                                                         <span class="BG_ColourIsRMDEligible">&nbsp;&nbsp;&nbsp;</span> <span class="Label_Small"> RMD eligible </span>
                                                         <%-- END : SB | 2016.01.17 | YRS-AT-3297 | Displaying records for all RMD elible persons so legend information is updated --%>
                                                        <span class="BG_ColourIsDateDiffer">&nbsp;&nbsp;&nbsp;</span> <span class="Label_Small"> Contribution and Termination date differ </span>
                                                    </td>
                                                </tr>
                                                <%--SG: 2012.08.28: BT-960 --%>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="LinkEligibleList" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="LinkNotEligibleList" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="ButtonUpdateCounter" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="ButtonSelectAll" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="linkPlain" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="linkHighlighted" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="linkAll" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="ButtonProcess" EventName="Click" />
                                            <asp:PostBackTrigger ControlID="btnClose" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <table width="100%" align="center" id="tbAllButton">
                                        <tr>
                                            <td class="Td_ButtonContainer" align="center" width="15%">
                                                <asp:UpdatePanel runat="server" ID="upBatch" RenderMode="Inline" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Button ID="ButtonSelectAll" runat="server" CssClass="Button_Normal" Text="Select All"
                                                            Width="100px" Enabled="False"></asp:Button>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td class="Td_ButtonContainer" align="center" width="15%">
                                                <asp:Button ID="ButtonGetData" runat="server" CssClass="Button_Normal" Text="Get Data"
                                                    Width="100px" Enabled="False"></asp:Button>
                                            </td>
                                            <td class="Td_ButtonContainer" align="center" width="15%">
                                                <asp:Button ID="ButtonUpdateCounter" runat="server" CssClass="Button_Normal" Width="110px"
                                                    Text="Update Counter" Enabled="False"></asp:Button>
                                            </td>
                                            <td class="Td_ButtonContainer" align="center" width="15%">
                                                <asp:Button ID="ButtonProcess" runat="server" CssClass="Button_Normal" Text="Process Cashout"
                                                    Width="150px" Enabled="False"></asp:Button>
                                                <%-- <asp:Button ID="btnProcess" runat="server" CssClass="Button_Normal" Text="Process Cashout"
                                                    Width="150px" Enabled="False"></asp:Button>--%>
                                            </td>
                                            <td class="Td_ButtonContainer" align="center" width="15%">
                                                <asp:Button ID="ButtonReport" runat="server" CssClass="Button_Normal" Text="Show Report"
                                                    Width="100px" Enabled="False"></asp:Button>
                                            </td>
                                            <td class="Td_ButtonContainer" align="center" width="15%">
                                                <asp:Button ID="ButtonOk" CssClass="Button_Normal" Text="OK" Width="100px" runat="server"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                            </iewc:MultiPage>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <div id="ConfirmDialog" style="overflow: visible;">
        <div>
            <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" CssClass="Label_Small" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%--Start: Dinesh Kanojia         2014.02.12          BT-2298: YRS 5.0-2259 - Optimizing the Cash out process to improve its performance.
     below div added to show modal popup for batch request creation--%>
    <%-- <div id="modalcashout" style="display: none;">
        <table style="width: 95%; height: auto">
            <tr>
                <td>
                    <b>Please do not close this window until all activities are complete. </b>
                </td>
            </tr>
            <tr>
                <td>
                    <iframe id="frmCashout" visible="false" frameborder="0"></iframe>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button runat="server" ID="btnClose" Text="Close" CssClass="Button_Normal" />
                </td>
            </tr>
        </table>
    </div>--%>

    <div id="dvConfirmDialog" title="RMD Print Letter" runat="server" style="overflow: visible;">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <div>
                    <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                        <tr>
                            <td>
                                <asp:Label ID="lblConfirmMessage" CssClass="Label_Small" runat="server"></asp:Label>
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
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonProcess" EventName="Click" />
                <asp:PostBackTrigger ControlID="btnClose"/>
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel runat="server" ID="updatepnl">
        <ContentTemplate>
            <div id="modalCashOut" style="display: none;">
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
                                    <td class="DataGrid_AlternateStyle" width="55%" valign="middle" style="vertical-align: middle; background-color: White">1. Request Creation
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
                                                    <div id="dvPDF" runat="server" class="info-Progress" >
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
                    <tr>
                        <td>
                            <div id="dvStatusMsg" runat="server" class="Label_Small"></div>
                        </td>
                    </tr>
                    <tr id="trshowErrors" class="hide">
                        <td>
                            <div>
                                <asp:Label Text="One or more errors were encountered while creating this batch request. Please click here to " runat="server" ID="as" class="Label_Small">
                                    <asp:LinkButton ID="lnkShowError" runat="server" ForeColor="#3399ff" OnClientClick="ExceptionShowError(); return false;">View details</asp:LinkButton></asp:Label>
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

            <div overflow: auto; width: 100%" id="dvShowErrorsMsg">
                <div id="divPopErrorMsg" style="width: 100%">
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonProcess" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnClose" />
        </Triggers>
    </asp:UpdatePanel>

    <%--End: Dinesh Kanojia         2014.02.12          BT-2298: YRS 5.0-2259 - Optimizing the Cash out process to improve its performance. --%>
    <asp:PlaceHolder ID="MessageBoxPlaceHolder" runat="server"></asp:PlaceHolder>
    
</asp:Content>
<%--End: BT-2324: YRS 5.0-2267 - Changes to Cashout master page--%>