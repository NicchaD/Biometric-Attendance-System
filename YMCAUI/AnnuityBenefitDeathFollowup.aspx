<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master" CodeBehind="AnnuityBenefitDeathFollowup.aspx.vb" Inherits="YMCAUI.AnnuityBenefitDeathFollowup" EnableEventValidation="true" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=10,9">
    <script type="text/javascript">
        var CloseDisable = false;
        function ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector) {
            var totalCheckboxes = $(checkBoxSelector);
            var checkedCheckboxes = totalCheckboxes.filter(":checked");
            var noCheckboxesAreChecked = (checkedCheckboxes.length === 0);
            var allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);
            if (totalCheckboxes.length == 0) {
                allCheckboxesAreChecked = false;
            }
            $(allCheckBoxSelector).removeAttr('indeterminate');
            $(allCheckBoxSelector).attr('checked', allCheckboxesAreChecked);
            if (checkedCheckboxes.length > 0 && allCheckboxesAreChecked != true) {
                $(allCheckBoxSelector).attr('indeterminate', true);
            }
            if (checkedCheckboxes.length > 0) {
                $('#<%=btn60Print.ClientID%>').removeAttr("disabled");
                $('#<%=btn90Print.ClientID%>').removeAttr("disabled");
             }
        }

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    BindEvents();
                }
            }
            BindEvents();
        });

        function TabDisplay(hash) {
            $('.AnnBeneFL_tabContent').css('display', 'none');
            $(hash).css('display', 'block');
        }

        function TabChange(_this) {
            $('.AnnBeneFL_tabContent').css('display', 'none');
            $('div.gvTab').removeClass('tabSelected');
            var _href = $(_this).attr('href');
            $(_href).css('display', 'block');
            var hrefLen = _href.length;
            var hdd = $('#<%=hdnSelectedTad.ClientID%>');
            _href = _href.substring(1, hrefLen);
            $(hdd).val(_href);
        };

        function ShowSaveConfirmDialog(str, type) {
            if (type == 'YesNo') {
                $('#divSaveConfirmDialog').dialog({
                    title: "YMCA YRS - Confirmation",
                    buttons: [{ text: "Yes", click: SaveFollowupStatus }, { text: "No", click: CloseSaveConfirmDialog }]
                });
            } else if (type == 'Process') {
                $('#divSaveConfirmDialog').dialog({
                    title: "YMCA YRS - Processing request",
                    buttons: []
                });
            }
            $('#divSaveConfirmDialog').dialog('open');
            $('#lblSaveMessage').text(str);
        }

        function SaveFollowupStatus() {
            $('#<%=hdnSaveResponse.ClientID%>').val('SaveResponse');
            ShowSaveConfirmDialog('Please wait until processing is complete.', 'Process');
            document.forms(0).submit();
        }

        function CloseSaveConfirmDialog() {
            $('#<%=hdnSaveResponse.ClientID%>').val('NoResponse');
            $('#divSaveConfirmDialog').dialog('close');
            $('#lblSaveMessage').text("");
            document.forms(0).submit();
        }

        function ShowConfirmDialog(str) {
            $('#divConfirmDialog').dialog('open');
            $('#lblMessage').text(str);
        }

        function ShowPanel(strBatchId, strModuleType) {
            CallProcess(strBatchId, strModuleType);
        }

        function CallProcess(strbatchId, strModule) {
            OpenProcessScreen();
            CallPrintLetterProcess(strbatchId, 0, "", 0, 0, strModule);
        }

        function ClosePrintDialog() {
            $("#divAnnBeneDeathFollowUp").dialog('close');
            document.forms(0).submit();
        }

        function CallPrintLetterProcess(strbatchId, iCount, strProcessName, iIDXCreated, iPDFCreated, strModule) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "AnnuityBenefitDeathFollowup.aspx/AnnBeneDeathFollowupProcess",
                data: "{'strBatchId': '" + strbatchId + "','iCount':" + iCount + ",'strProcessName':'" + strProcessName + "'," +
                    "'iIDXCreated':" + iIDXCreated + ",'iPDFCreated':" + iPDFCreated + ",'strModule':'" + strModule + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d.strretValue == "success") {
                        CopyFilestoFileServer(1, 1, strbatchId, strModule, strModule);
                        RMDBatchProcessCounter = data;

                        $('#dvPDF').text("Completed");
                        $('#dvIDX').text("Completed");
                        $('#dvReg').text("Completed");

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
                        CallPrintLetterProcess(data.d.strBatchId, data.d.iProcessCount, data.d.strProcessName, data.d.iIdxCreated, data.d.iPdfCreated, strModule);
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

        var RMDBatchProcessCounter
        function CopyFilestoFileServer(strOR, strDEL, strBatchId, strModule) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "FT/CopyFilestoFileServer.aspx/CopytoFileServer",
                data: "{'strOR':'" + strOR + "','strDEL':'" + strDEL + "','strBatchId':'" + strBatchId + "','strModule':'" + strModule + "'}",
                dataType: "json",
                success: function (data) {
                    RMDBatchProcessCounter.d.iPdfCopied = data.d.iIdxCopied;
                    RMDBatchProcessCounter.d.iIdxCopied = data.d.iPdfCopied;
                    if (data.d.strStatus == 'error') {
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
                            $('#dvStatusMsg').text("Some of the records were not proccessed due to problem, please click on view details link for details.");
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
                            $('#dvStatusMsg').text("Annuity Benefit Death Follow-up letter(s) process completed sucessfully.");
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
                            $('#dvStatusMsg').text("Annuity Benefit Death Follow-up letter(s) process completed sucessfully.");
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
                }
            });
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

        function CloseConfirmDialog() {
            $.ajax({
                type: "POST",
                url: "AnnuityBenefitDeathFollowup.aspx/ClearPrintFollowupLettersSession",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $('#divConfirmDialog').dialog('close');
                }
            });
        }

        function PrintFollowupLetters() {
            $.ajax({
                type: "POST",
                url: "AnnuityBenefitDeathFollowup.aspx/PrintFollowupLetters",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CloseConfirmDialog();
                    var strData = String();
                    var str;
                    var strBatchID;
                    var strModule;
                    strData = msg.d;
                    str = strData.split('$$$');
                    strBatchID = str[0];
                    strModule = str[1];
                    ShowPanel(strBatchID, strModule);
                }
            });
        }

        function OpenProcessScreen() {
            $('#divAnnBeneDeathFollowUp').dialog({
                autoOpen: false,
                draggable: true,
                resizable: false,
                show: "fade",
                hide: "fade",
                modal: true,
                close: false,
                title: "Beneficiaries Follow-up Letter(s) - Creation status",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                },
                width: 900,
                height: 700,
                closeOnEscape: false,
                beforeclose: function () {
                    return CloseDisable;
                },
                buttons: [{ text: "Close", click: ClosePrintDialog }]
            });
            $('#divAnnBeneDeathFollowUp').dialog("open");
        }

        function CallLetter() {
            window.open('CallReport.aspx', 'ReportPopUp1', 'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
        }

        function OpenPDF(strFileName) {
            try {
                window.open(strFileName, 'OpenCustomPDF', 'width=900, height=900, resizable=yes, scrollbars=yes ,directories=no ,toolbar=no ,menubar=no, location=no');
            }
            catch (err) {
                alert(err.message);
            }
        }

        function BindEvents() {
            var checkBoxSelector = '#<%=gvPendingFollowupList.ClientID%> input[type=checkbox]:not("input[id*=chkSelectAll]")';
            var allCheckBoxSelector = '#<%=gvPendingFollowupList.ClientID%> input[id*=chkSelectAll]';
            var checkBox60Selector = '#<%=gv60DayFollowup.ClientID%> input[type=checkbox]:not("input[id*=chkSelectAll]")';
            var allCheckBox60Selector = '#<%=gv60DayFollowup.ClientID%> input[id*=chkSelectAll]';

            var checkBox90Selector = '#<%=gv90DayFollowup.ClientID%> input[type=checkbox]:not("input[id*=chkSelectAll]")';
            var allCheckBox90Selector = '#<%=gv90DayFollowup.ClientID%> input[id*=chkSelectAll]';

            //This method is for Pending List
            $(allCheckBoxSelector).bind('click', function () {
                $(checkBoxSelector).attr('checked', $(this).is(':checked'));
                $("#<%=btnSave.ClientID%>").attr('disabled', false);
                ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector);
            });
            $(checkBoxSelector).bind('click', function () {
                $("#<%=btnSave.ClientID%>").attr('disabled', false);
                ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector);
            });
            ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector);

            //This method is for 60 Days List
            $(allCheckBox60Selector).bind('click', function () {
                $(checkBox60Selector).attr('checked', $(this).is(':checked'));
                ToggleCheckUncheckAllOptionAsNeeded(allCheckBox60Selector, checkBox60Selector);
            });
            $(checkBox60Selector).bind('click', function () {
                ToggleCheckUncheckAllOptionAsNeeded(allCheckBox60Selector, checkBox60Selector);
            });
            ToggleCheckUncheckAllOptionAsNeeded(allCheckBox60Selector, checkBox60Selector);

            //This method is for 90 Days List
            $(allCheckBox90Selector).bind('click', function () {
                $(checkBox90Selector).attr('checked', $(this).is(':checked'));
                ToggleCheckUncheckAllOptionAsNeeded(allCheckBox90Selector, checkBox90Selector);
            });
            $(checkBox90Selector).bind('click', function () {
                ToggleCheckUncheckAllOptionAsNeeded(allCheckBox90Selector, checkBox90Selector);
            });
            ToggleCheckUncheckAllOptionAsNeeded(allCheckBox90Selector, checkBox90Selector);

            $(".tabNotSelected").mouseover(function () {
                //$(this).addClass('tabSelected');
                $(this).css("background-color", "#93BEEE");
                $(this).find('input[type="button"]').css('color', '#000000');
                $(this).find('input[type="button"]').css('background-color', '#93BEEE');
            }).mouseout(function () {
                $(this).css("background-color", "#4172A9");
                $(this).find('input[type="button"]').css('color', '#ffffff');
                $(this).find('input[type="button"]').css('background-color', '#4172A9');
                //$(this).addClass('tabNotSelected');
            });


            $('#divSaveConfirmDialog').dialog({
                autoOpen: false,
                resizable: false,
                draggable: false,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 350, height: 250,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

            //Open Print Confirm Dialog
            $('#divConfirmDialog').dialog({
                autoOpen: false,
                resizable: false,
                draggable: false,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 350, height: 250,
                title: "YMCA YRS - Confirmation",
                buttons: [{ text: "Yes", click: PrintFollowupLetters }, { text: "No", click: CloseConfirmDialog }],
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });


        }
    </script>
    <style type="text/css">
        div.gvTab { /*>*/
            display: inline;
            width: 22%; /*32.6%;*/
            margin-left: -1px;
            margin-right: -11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server" ClientIDMode="Predictable">
    <asp:ScriptManagerProxy ID="dbScriptManagerProxy" runat="server">
    </asp:ScriptManagerProxy>
    <div class="Div_Center Table_WithBorder" style="width: 100%; height: 450px;">
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <div id="AnnBeneFLtab">
                    <div style="text-align: left;">
                        <div style="display: inline;">
                            <label class="Label_Small" for="txtFundNoFilter">Fund No.</label>
                        </div>
                        <div style="display: inline;">
                            <asp:TextBox ID="txtFundNoFilter" TabIndex="0" CssClass="TexBox_Normal" MaxLength="10" AutoCompleteType="None" placeholder="Fund No." onkeypress="ValidateNumeric();" runat="server"></asp:TextBox>
                        </div>
                        <div style="display: inline;">
                            <asp:Button ID="btnFilter" class="Button_Normal" TabIndex="1" runat="server" Text="Find" />
                        </div>
                    </div>
                    <div style="text-align: right;margin-top:-20px;">
                        <div tabindex="0" id="tab_gvPending" runat="server" class="gvTab tabSelected">
                            <div>
                                <input type="button" href="#tabContent_gvPending" tabindex="2" id="tab_gvPendingClick" runat="server" onclick="TabChange(this);" value="Pending Follow-up Records" class="tabSelectedLink" />
                            </div>
                        </div>
                        <div tabindex="1" id="tab_gv60Days" runat="server" class="gvTab tabNotSelected">
                            <div>
                                <input type="button" href="#tabContent_gv60Days" id="tab_gv60DayClick" tabindex="3" runat="server" onclick="TabChange(this);" class="tabNotSelectedLink" />
                            </div>
                        </div>
                        <div tabindex="2" id="tab_gv90Days" runat="server" class="gvTab tabNotSelected">
                            <div>
                                <input type="button" href="#tabContent_gv90Days" id="tab_gv90DayClick" tabindex="4" runat="server" onclick="TabChange(this);" class="tabNotSelectedLink" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="tab_gvPendingClick" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="tab_gv60DayClick" EventName="ServerClick" />
                <asp:AsyncPostBackTrigger ControlID="tab_gv90DayClick" EventName="ServerClick" />
            </Triggers>
        </asp:UpdatePanel>

        <div id="tabContent_gvPending" class="AnnBeneFL_tabContent">
            <div style="padding: 3px; background-color: #93BEEE;">
            </div>
            <div>
                <div class="Label_Small" style="text-align: left;">
                    <%-- Start - MMR | 2016.06.24 | YRS-AT-2674 | Commented existing code and added 'are' word to text--%>
                    <%--This is the list of records for whom death documents yet to be received.--%>
                    This is the list of records for whom death documents are yet to be received.
                    <%-- End - MMR | 2016.06.24 | YRS-AT-2674 | Commented existing code and added 'are' word to text--%>
                    </div>
            </div>
            <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                <ContentTemplate>
                    <table style="width: 100%;">
                        <tr style="height: 400px; vertical-align: top;">
                            <td style="width: 100%; text-align: left;">
                                <div style="height: 400px; text-align: left; overflow: scroll">

                                    <asp:GridView ID="gvPendingFollowupList" runat="server" TabIndex="5" AutoGenerateColumns="false" Width="100%" CssClass="Label_Small" EmptyDataText="No pending follow-up lists"
                                        AllowSorting="true" DataKeyNames="guiAnnuityJointSurvivorsID" AllowPaging="true" PageSize="14">
                                        <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                        <RowStyle CssClass="DataGrid_NormalStyle" />
                                        <SortedAscendingHeaderStyle CssClass="sortasc" />
                                        <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                        <Columns>
                                            <%-- Start - MMR | 2016.06.24 | YRS-AT-2674 | Commented existing code and converted headerstyle width in percentage instead of pixel--%>
                                            <%--<asp:TemplateField HeaderStyle-Width="20px">--%>
                                                <asp:TemplateField HeaderStyle-Width="3%">
                                            <%-- End - MMR | 2016.06.24 | YRS-AT-2674 | Commented existing code and converted headerstyle width in percentage instead of pixel--%>
                                            <HeaderTemplate>
                                                    Response Received <%-- MMR | 2016.06.24 | YRS-AT-2674 |Added 'Response Received' text in header --%>
                                                    <asp:CheckBox ID="chkSelectAll" EnableViewState="false" runat="server"/>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" Checked='<%#Eval("DocumentsReceived") %>' CssClass="Warn" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="20px" />
                                            </asp:TemplateField>
                                            <%-- Start - MMR | 2016.06.24 | YRS-AT-2674 | Commented existing code and changed itemstyle width percentage to fit data in grid and also added participant name--%>
                                            <%--<asp:BoundField DataField="Participant Fund No." ItemStyle-Width="13%" SortExpression="Participant Fund No." HeaderText="Participant Fund No." />--%>
                                            <asp:BoundField DataField="Participant Fund No." ItemStyle-Width="7%" SortExpression="Participant Fund No." HeaderText="Participant Fund No." />
                                            <asp:BoundField DataField="Participant Name" ItemStyle-Width="20%" SortExpression="Participant Name" HeaderText="Participant Name" />
                                            <%-- End - MMR | 2016.06.24 | YRS-AT-2674 | Commented existing code and changed itemstyle width percentage to fit data in grid and also added participant name--%>
                                            <asp:BoundField DataField="Beneficiary Name" SortExpression="Beneficiary Name" ItemStyle-Width="20%" HeaderText="Beneficiary Name" />
                                            <asp:BoundField DataField="Original Letter Date" SortExpression="Original Letter Date" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Original Letter Date" ItemStyle-Width="10%" /> <%-- MMR | 2016.06.24 | YRS-AT-2674 | Added itemstyle width percentage to fit data in grid --%>
                                            <asp:BoundField DataField="60 day Follow-up Date" SortExpression="60 day Follow-up Date" DataFormatString="{0:MM/dd/yyyy}" HeaderText="60 day Follow-up Date" ItemStyle-Width="10%" /> <%-- MMR | 2016.06.24 | YRS-AT-2674 | Added itemstyle width percentage to fit data in grid --%>
                                            <asp:BoundField DataField="60 day Follow-up Sent" SortExpression="60 day Follow-up Sent" DataFormatString="{0:MM/dd/yyyy}" HeaderText="60 day Follow-up Sent" ItemStyle-Width="10%"/> <%-- MMR | 2016.06.24 | YRS-AT-2674 | Added itemstyle width percentage to fit data in grid --%>
                                            <asp:BoundField DataField="90 day Follow-up Date" SortExpression="90 day Follow-up Date" DataFormatString="{0:MM/dd/yyyy}" HeaderText="90 day Follow-up Date" ItemStyle-Width="10%"/> <%-- MMR | 2016.06.24 | YRS-AT-2674 | Added itemstyle width percentage to fit data in grid --%>
                                            <asp:BoundField DataField="90 day Follow-up Sent" SortExpression="90 day Follow-up Sent" DataFormatString="{0:MM/dd/yyyy}" HeaderText="90 day Follow-up Sent" ItemStyle-Width="10%"/> <%-- MMR | 2016.06.24 | YRS-AT-2674 | Added itemstyle width percentage to fit data in grid --%>                                      
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="15" FirstPageText="First" LastPageText="Last" />
                                        <PagerStyle CssClass="pagination" />
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr style="vertical-align: top">
                            <td class="Td_ButtonContainer" style="width: 100%;">
                                <table cellspacing="0" width="100%" style="height: 23px">
                                    <tr>
                                        <td align="right">
                                            <asp:Button runat="server" ID="btnSave" TabIndex="6" Enabled="false" Text="Save" class="Button_Normal" Style="width: 80px;" />
                                            <asp:Button runat="server" ID="btnClose" Text="Close" TabIndex="7" class="Button_Normal" Style="width: 80px;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div id="tabContent_gv60Days" class="AnnBeneFL_tabContent" style="display: none;">
            <div style="padding: 3px; background-color: #93BEEE;">
            </div>
            <div>
                <div class="Label_Small" style="text-align: left;">
                    This is the list of records for whom 60 day follow-up letter has to be generated.
                </div>
            </div>
            <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                <ContentTemplate>
                    <table style="width: 100%;">
                        <tr style="height: 400px; vertical-align: top;">
                            <td style="width: 100%; text-align: left;">
                                <div style="height: 400px; text-align: left; overflow: scroll">

                                    <asp:GridView ID="gv60DayFollowup" runat="server" AutoGenerateColumns="false" TabIndex="8" Width="100%" CssClass="Label_Small" EmptyDataText="No pending follow-up lists for 60 days"
                                        AllowSorting="true" DataKeyNames="guiAnnuityJointSurvivorsID" AllowPaging="true" PageSize="14">
                                        <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                        <RowStyle CssClass="DataGrid_NormalStyle" />
                                        <SortedAscendingHeaderStyle CssClass="sortasc" />
                                        <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="20px">
                                                <HeaderTemplate>                                                    
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" CssClass="Warn" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="20px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Participant Fund No." SortExpression="Participant Fund No." ItemStyle-Width="13%" HeaderText="Participant Fund No." />
                                            <%-- Start - MMR | 2016.06.24 | YRS-AT-2674 | Commented existing code and changed itemstyle width percentage to fit data in grid and also added participant name--%>
                                            <asp:BoundField DataField="Participant Name" ItemStyle-Width="20%" SortExpression="Participant Name" HeaderText="Participant Name" />
                                            <%--<asp:BoundField DataField="Beneficiary Name" SortExpression="Beneficiary Name" ItemStyle-Width="55%" HeaderText="Beneficiary Name" />--%>
                                            <asp:BoundField DataField="Beneficiary Name" SortExpression="Beneficiary Name" ItemStyle-Width="40%" HeaderText="Beneficiary Name" />
                                            <%-- End - MMR | 2016.06.24 | YRS-AT-2674 | Commented existing code and changed itemstyle width percentage to fit data in grid and also added participant name--%>
                                            <asp:BoundField DataField="Original Letter Date" SortExpression="Original Letter Date" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Original Letter Date" ItemStyle-Width=""/>
                                            <asp:BoundField DataField="60 day Follow-up Date" SortExpression="60 day Follow-up Date" DataFormatString="{0:MM/dd/yyyy}" HeaderText="60 day Follow-up Date" />
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="15" FirstPageText="First" LastPageText="Last" />
                                        <PagerStyle CssClass="pagination" />
                                    </asp:GridView>

                                </div>
                            </td>
                        </tr>
                        <tr style="vertical-align: top">
                            <td class="Td_ButtonContainer" style="width: 100%;">
                                <table cellspacing="0" width="100%" style="height: 23px">
                                    <tr>
                                        <td align="right">
                                            <asp:Button runat="server" ID="btn60Print" Text="Print" class="Button_Normal" TabIndex="9" Style="width: 80px;" />
                                            <asp:Button runat="server" ID="btn60Close" Text="Close" class="Button_Normal" TabIndex="10"   Style="width: 80px;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btn60Print" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div id="tabContent_gv90Days" class="AnnBeneFL_tabContent" style="display: none;">
            <div style="padding: 3px; background-color: #93BEEE;">
            </div>
            <div>
                <div class="Label_Small" style="text-align: left;">
                    This is the list of records for whom 90 day follow-up letter has to be generated.
                </div>
            </div>
            <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                <ContentTemplate>
                    <table style="width: 100%;">
                        <tr style="height: 400px; vertical-align: top;">
                            <td style="width: 100%; text-align: left;">
                                <div style="height: 400px; text-align: left; overflow: scroll">

                                    <asp:GridView ID="gv90DayFollowup" runat="server" AutoGenerateColumns="false" TabIndex="11" Width="100%" CssClass="Label_Small" EmptyDataText="No pending follow-up lists for 90 days"
                                        AllowSorting="true" DataKeyNames="guiAnnuityJointSurvivorsID" AllowPaging="true" PageSize="14">
                                        <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                        <RowStyle CssClass="DataGrid_NormalStyle" />
                                        <SortedAscendingHeaderStyle CssClass="sortasc" />
                                        <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="20px">
                                                <HeaderTemplate>                                                   
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" CssClass="Warn" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="20px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Participant Fund No." SortExpression="Participant Fund No." ItemStyle-Width="13%" HeaderText="Participant Fund No." />
                                            <%-- Start - MMR | 2016.06.24 | YRS-AT-2674 | Commented existing code and changed itemstyle width percentage to fit data in grid and also added participant name--%>
                                            <asp:BoundField DataField="Participant Name" ItemStyle-Width="20%" SortExpression="Participant Name" HeaderText="Participant Name" />
                                            <%--<asp:BoundField DataField="Beneficiary Name" SortExpression="Beneficiary Name" ItemStyle-Width="55%" HeaderText="Beneficiary Name" />--%>
                                            <asp:BoundField DataField="Beneficiary Name" SortExpression="Beneficiary Name" ItemStyle-Width="40%" HeaderText="Beneficiary Name" />
                                            <%-- End - MMR | 2016.06.24 | YRS-AT-2674 | Commented existing code and changed itemstyle width percentage to fit data in grid and also added participant name--%>
                                            <asp:BoundField DataField="Original Letter Date" SortExpression="Original Letter Date" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Original Letter Date" />
                                            <asp:BoundField DataField="90 day Follow-up Date" SortExpression="90 day Follow-up Date" DataFormatString="{0:MM/dd/yyyy}" HeaderText="90 day Follow-up Date" />
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="15" FirstPageText="First" LastPageText="Last" />
                                        <PagerStyle CssClass="pagination" />
                                    </asp:GridView>

                                </div>
                            </td>
                        </tr>
                        <tr style="vertical-align: top">
                            <td class="Td_ButtonContainer" style="width: 100%;">
                                <table cellspacing="0" width="100%" style="height: 23px">
                                    <tr>
                                        <td align="right">
                                            <asp:Button runat="server" ID="btn90Print" TabIndex="12" Text="Print" class="Button_Normal" Style="width: 80px;" />
                                            <asp:Button runat="server" ID="btn90Close" Text="Close" TabIndex="13" class="Button_Normal" Style="width: 80px;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btn90Print" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <asp:HiddenField ID="hdnSelectedTad" runat="server" Value="tabContent_gvPending" />
        <asp:HiddenField ID="hdnSaveResponse" runat="server" Value="" />
    </div>
    <asp:UpdatePanel runat="server" ID="updatepnl">
        <ContentTemplate>
            <div id="divAnnBeneDeathFollowUp" style="display: none;">
                <table style="width: 100%;">
                    <tr>
                        <td class="Label_Small">Please do not close this window until all activities are complete.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="dvMessage"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="dvShowStatus" class="Label_Small"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="dvStatusMsg" class="Label_Small"></div>
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
                                    <td class="DataGrid_AlternateStyle" width="55%" valign="middle" style="vertical-align: middle; background-color: White">1. Generate letter entries
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="background-color: White; width: 15%;">
                                        <table>
                                            <tr>
                                                <td class="DataGrid_AlternateStyle" style="background-color: White; text-align: center">
                                                    <asp:Image ID="imgRegComplete" runat="server" ImageUrl="~/images/complete.jpg" AlternateText="Complete"
                                                        CssClass="hide" Width="20" Height="20" />
                                                    <div id="dvReg" class="info-Progress">
                                                        In-Progress
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White; width: 30%">
                                        <div id="dvReqProgress" align="center" style="vertical-align: middle">
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
                                                    <div id="dvIDX" class="info-Progress">
                                                        In-Progress
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White">
                                        <div id="dvIDXProgress" align="center" style="vertical-align: middle">
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
                                                    <div id="dvPDF" class="info-Progress">
                                                        In-Progress
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White">
                                        <div id="dvPDFProgress" align="center" style="vertical-align: middle">
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
                                                    <div id="dvCopy" class="info-Progress">
                                                        Pending
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White">
                                        <div id="dvCopiedProgress" align="center" style="vertical-align: middle">
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
                                    <asp:LinkButton ID="lnkShowError" runat="server" ForeColor="#3399ff" OnClientClick="ExceptionShowError(); return false;">View details</asp:LinkButton></asp:Label>
                            </div>
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
            <asp:AsyncPostBackTrigger ControlID="btn60Print" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn90Print" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <div id="divConfirmDialog" title="Cancel Follow-up" style="overflow: visible;">
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                    <tr>
                        <td>
                            <label id="lblMessage" class="Label_Small"></label>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divSaveConfirmDialog" style="overflow: visible;">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                    <tr>
                        <td>
                            <label id="lblSaveMessage" class="Label_Small"></label>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
