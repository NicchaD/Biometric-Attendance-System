<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoanOffsetDefaultUnfreeze.aspx.vb" Inherits="YMCAUI.LoanOffsetDefaultUnfreeze" MasterPageFile="~/MasterPages/YRSMain.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="JS/jquery-1.5.1.min.js"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
    
    
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
        /*Start: AA: YRS-AT-2662 Added below lines to show legend in auto offset tab*/
        .BG_Colour_Loan_Report {
            background-color: #f1995a;
        }
        /*End: AA: YRS-AT-2662 Added end lines to show legend in auto offset tab*/
        
    </style>

    <script type="text/javascript">
        var strReportName;
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

        function ontabmousehover(id) {
            $('#' + id).attr('class', 'tabselected');
        }
        function ontabmousehout(id) {
            $('#' + id).attr('class', 'tabNotSelected');
        }

        function BindEvents() {
            $('#divConfirmDialog').dialog({
                autoOpen: false,
                resizable: false,
                draggable: false,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 350, height: 250,
                title: "UnFreeze Loan",
                buttons: [{ text: "Yes", click: UnFreezeLoan }, { text: "No", click: CloseConfirmDialog }],
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
            //Start: AA: YRS-AT-2662 Added below lines to show legend in auto offset tab 
            $('#dvReportConfirm').dialog({
                autoOpen: false,
                resizable: false,
                draggable: false,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 350, height: 250,
                title: "Print Report",
                buttons: [{ text: "Yes", click: PrintReport }, { text: "No", click: DontPrintReport }],
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

            $('#dvProcessing').dialog({
                autoOpen: false,
                resizable: false,
                draggable: false,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 300, height: 150,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
            
            //End: AA: YRS-AT-2662 Added below lines to show legend in auto offset tab 

           //$('#gvFreezeHeader').css('position', 'relative');
            //$('#gvFreezeHeader').css('cursor', 'default');
            //$('#gvFreezeHeader').css('top', 'expression(' + document.getElementById("gvFreezeHeader").scrollTop - 2 + ')');
            //$('#gvFreezeHeader').css('z-index', '10');

            //Start: Bala: 12.04.2016: YRS-AT-2862: Pagination.
            //var gridHeadergvDefaultedLoans = $('#<%'=gvDefaultedLoans.ClientID%>').clone(true);
            //$(gridHeadergvDefaultedLoans).find("tr:gt(0)").remove();
            //$('#<%'=gvDefaultedLoans.ClientID%> tr th').each(function (i) {
            //  $("th:nth-child(" + (i + 1) + ")", gridHeadergvDefaultedLoans).css('position', 'absolute');
            //  $("th:nth-child(" + (i + 1) + ")", gridHeadergvDefaultedLoans).css('top', 'expression(' + document.getElementById('gvFreezeHeader').scrollTop - 2 + ')');
            //  //$("th:nth-child(" + (i + 1) + ")", gridHeadergvDefaultedLoans).css('top', 'expression(parentNode.parentNode.parentNode.parentNode.scrollTop)');
            //  $("th:nth-child(" + (i + 1) + ")", gridHeadergvDefaultedLoans).css('z-index', '10');
            //  $("th:nth-child(" + (i + 1) + ")", gridHeadergvDefaultedLoans).css('cursor', 'default');
            //});
            //End: Bala: 12.04.2016: YRS-AT-2862: Pagination.
        }

        function UnFreezeLoan() {
            var Val = document.getElementById('hdnValue').value;
            $.ajax({
                type: "POST",
                url: "LoanOffsetDefaultUnfreeze.aspx/UnFreezeLoan",
                data: "{'strLoanDetailsId': '" + Val + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CloseConfirmDialog();
                    document.forms(0).submit();
                }
            });
        }

        function ShowConfirmDialog(strLoanDetailsId, strMessage, strFundID) {
            $('#divConfirmDialog').dialog('open');
            $('#lblMessage').text(strMessage);
            $('#hdnValue').val(strLoanDetailsId);
            $('#hdnFundNo').val(strFundID);
        }

        function CloseConfirmDialog() {
            $('#divConfirmDialog').dialog('close');
        }
        /* Start:AA:09.232015 YRS AT-2609 Added below lines to call SSRS report*/
        function CallLetter() {
            window.open('CallReport.aspx', 'ReportPopUp1', 'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
        }
        /* END:AA:09.232015 YRS AT-2609 Added below lines to call SSRS report*/

        //Start: AA: YRS-AT-2662 Added below code for auto offset tabs 
        function OpenReportViewer(paramReportName) { //AA:07.01.2016 YRS-AT-2830 Added a parameter to open the report viewer based on it.
            try {
                //Start: AA:07.1.2016 YRS-AT-2830 Changed to open the report viewr pages based on the input paramter 
                if (paramReportName == 'Loan Satisfied-PA') {
                    newwindow = window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp_1', 'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
                }
                else {
                    newwindow = window.open('FT\\ReportViewer.aspx', 'ReportPopUp', 'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
                }
                //End: AA:07.1.2016 YRS-AT-2830 Changed to open the report viewr pages based on the input paramter 
                if (window.focus) {newwindow.focus()}
                if (!newwindow.closed) {newwindow.focus()}
            }
            catch (err) {
                alert(err.message);
            }
        }

        function CopyToIDMServer() {
            try{
                var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', 'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');
            }
            catch (err) {
                alert(err.message);
            }
        }

        function PrintReport() {
            try {
                ShowProcessingDialog('Please wait until report gets generated and screen gets loaded.', 'Print Report')
                CloseReportConfirmDialog();
                $.ajax({
                    type: "POST",
                    url: "LoanOffsetDefaultUnfreeze.aspx/PrintReport",
                    data: "{'strReportName': '" + strReportName + "'}", //AA:07.01.2016 YRS-AT-2830 Added a parameter to open the report viewer based on it.
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        OpenReportViewer(strReportName); //AA:07.01.2016 YRS-AT-2830 Added a parameter to open the report viewer based on it.
                        CopyToIDMServer();      
                        document.forms(0).submit();
                        strReportName = ''; //AA:07.01.2016 YRS-AT-2830 Clearing the variable for not to overlap with previous one
                    }
                });
            }
            catch (err) {
                alert(err.message);
            }
        }

        function DontPrintReport() {
            try {                
                $.ajax({
                    type: "POST",
                    url: "LoanOffsetDefaultUnfreeze.aspx/DontPrintReport",
                    data: "{}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {                        
                        CloseReportConfirmDialog();
                        strReportName = ''; //AA:07.01.2016 YRS-AT-2830 Clearing the variable for not to overlap with previous one
                    }
                });
            }
            catch (err) {
                alert(err.message);
            }
        }

        function ShowReportConfirmDialog(strMessage, ReportName) { //AA:07.01.2016 YRS-AT-2830 Added a parameter to open the report viewer based on it.
            strReportName = ReportName; //AA:07.01.2016 YRS-AT-2830 Added a parameter to open the report viewer based on it.
            $('#lblReportMsg').text(strMessage);
            $('#dvReportConfirm').dialog('open');
        }

        function CloseReportConfirmDialog() {            
            $('#dvReportConfirm').dialog('close');
        }

        function ShowProcessingDialog(Message, divTitle) {
            $('#lblProcessing').text(Message);
            $('#dvProcessing').dialog({ title: divTitle });
            $('#dvProcessing').dialog("open");
        }

        function CloseProcessingDialog() {
            $('#dvProcessing').dialog('close');
        }
        //End: AA: YRS-AT-2662 Added below code for auto offset tabs
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="div_center">
        <asp:ScriptManagerProxy ID="RMDPRintScriptManager" runat="server">
        </asp:ScriptManagerProxy>
        <asp:UpdatePanel ID="uplLoanOffSetDefaultUnFreeze" runat="server">
            <ContentTemplate>
                <table class="Td_ButtonContainer" style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="width: 100%">List of Loans
                        </td>
                    </tr>
                </table>
                <table class="Table_WithoutBorder" cellpadding="0" cellspacing="0" width="100%" align="center" border="0">
                    <tr>
                        <td align="left" valign="middle" style="width: auto">
                            <asp:Label ID="lblLoansMsg" runat="server" CssClass="Label_Small"></asp:Label>
                        </td>
                        <td align="right" cellspacing="0">
                            <table class="td_withoutborder" border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 25px;">
                                <tr>
                                    <td style="width: 0%">
                                        <table style="width: 100%" border="0" cellpadding="0" cellspacing="0" class="td_withoutborder">
                                            <tr>
                                                <%--<td class="Label_Small" style="width: 20%">Fund No.:
                                                </td>
                                                <td style="width: 45%">
                                                    <asp:TextBox ID="txtFundId" runat="server" CssClass="TexBox_Normal" Style="width: 100%"></asp:TextBox>
                                                </td>
                                                <td style="width: 35%">
                                                    <asp:Button runat="server" ID="btnFind" Text="Find" CssClass="Button_Normal" />
                                                    <asp:Button runat="server" ID="btnClear" Text="Clear" CssClass="Button_Normal" />
                                                </td><td>&nbsp;
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 20%">
                                        <asp:LinkButton ID="lnkOffset_Default_Ageing" Text="Offset / Default / Aging" runat="server" CssClass="tabNotSelected"
                                            onmouseover="javascript: ontabmousehover('lnkOffset_Default_Ageing');" onmouseout="javascript: ontabmousehout('lnkOffset_Default_Ageing');"></asp:LinkButton>
                                        <asp:Label ID="lblOffset_Default_Ageing" CssClass="tabSelected" runat="server" Text="Offset / Default / Aging"></asp:Label>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:LinkButton ID="lnkDefaultedLoans" Text="Defaulted Loans" runat="server" CssClass="tabNotSelected"
                                            onmouseover="javascript: ontabmousehover('lnkDefaultedLoans');" onmouseout="javascript: ontabmousehout('lnkDefaultedLoans');"></asp:LinkButton>
                                        <asp:Label ID="lblDefaultedLoans" CssClass="tabSelected" runat="server" Text="Defaulted Loans"></asp:Label>
                                    </td>
                                    <td id="tdUnfreeze" runat="server" style="width: 20%">
                                        <asp:LinkButton ID="lnkUnfreeze_Phantom_Int" CssClass="tabNotSelected" Text="Unfreeze Phantom Int." runat="server"
                                            onmouseover="javascript: ontabmousehover('lnkUnfreeze_Phantom_Int');" onmouseout="javascript: ontabmousehout('lnkUnfreeze_Phantom_Int');"></asp:LinkButton>
                                        <asp:Label ID="lblUnfreeze_Phantom_Int" CssClass="tabSelected" runat="server" Text="Unfreeze Phantom Int."></asp:Label>
                                    </td>
                                    <td id="tdOffset" runat="server" style="width: 15%">
                                        <%--Start: AA:01.20.2016 Added below code for auto offset loans tab --%>
                                        <asp:LinkButton ID="lnkOffsetLoans" CssClass="tabNotSelected" Text="Auto-Offset Loans" runat="server"
                                            onmouseover="javascript: ontabmousehover('lnkOffsetLoans');" onmouseout="javascript: ontabmousehout('lnkOffsetLoans');"></asp:LinkButton>
                                        <asp:Label ID="lblOffsetLoans" CssClass="tabSelected" runat="server" Text="Auto-Offset Loans"></asp:Label>
                                        <%--End: AA:01.20.2016 Added below code for auto offset loans tab --%>
                                    </td>
                                    <td id="tdAutoDefault" runat="server" style="width: 15%">
                                        <%--Start: AA:04.18.2016 YRS-AT-2831: Added below code for auto default loans tab --%>
                                        <asp:LinkButton ID="lnkAutoDefaultLoans" CssClass="tabNotSelected" Text="Auto-Default Loans" runat="server"
                                            onmouseover="javascript: ontabmousehover('lnkAutoDefaultLoans');" onmouseout="javascript: ontabmousehout('lnkAutoDefaultLoans');"></asp:LinkButton>
                                        <asp:Label ID="lblAutoDefaultLoans" CssClass="tabSelected" runat="server" Text="Auto-Default Loans"></asp:Label>
                                        <%--End: AA:04.18.2016 YRS-AT-2831: Added below code for auto default loans tab --%>
                                    </td>
                                    <td id="tdAutoClosed" runat="server" style="width: 15%">
                                        <%--Start: AA:06.28.2016 YRS-AT-2830: Added below code for auto closed loans tab --%>
                                        <asp:LinkButton ID="lnkAutoClosedLoans" CssClass="tabNotSelected" Text="Auto-Closed Loans" runat="server"
                                            onmouseover="javascript: ontabmousehover('lnkAutoClosedLoans');" onmouseout="javascript: ontabmousehout('lnkAutoClosedLoans');"></asp:LinkButton>
                                        <asp:Label ID="lblAutoClosedLoans" CssClass="tabSelected" runat="server" Text="Auto-Closed Loans"></asp:Label>
                                        <%--End: AA:06.28.2016 YRS-AT-2830: Added below code for auto closed loans tab --%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table class="Table_WithoutBorder" cellpadding="0" cellspacing="0" width="100%" align="center" border="0">
                    <tr class="tabSelectedLink" style="height:30px">
                        <td style="width: 30%; text-align: left;">
                            <asp:Label ID="lblDescription" runat="server" Text="Offset / Default / Aging"></asp:Label>
                        </td>
                        <%-- Start:AA:09.232015 YRS AT-2609 Added below lines to show filters for default and offset --%>
                        <td style="width: 8%; text-align:right">
                            <label class="Checkbox_Normal">Fund No.: &nbsp;</label></td>
                        <td style="width: 14%;text-align:left">
                            <asp:TextBox ID="txtFundId" runat="server" CssClass="TexBox_Normal" Style="width: 105px"></asp:TextBox></td>
                        <td style="text-align: left;">
                            <asp:CheckBox ID="chkOffsetEligible" runat="server" Text="Show Offset Eligible" CssClass="Checkbox_Normal" AutoPostBack="false" /></td>
                            <td style=" text-align: left;">
                                        <asp:CheckBox ID="chkDefaultEligible" runat="server" Text="Show Default Eligible" CssClass="Checkbox_Normal" AutoPostBack="false" />
                        </td>
                        <td style="width: 15%">
                            <asp:Button runat="server" ID="btnFind" Text="Find" CssClass="Button_Normal" />&nbsp;
                                        <asp:Button runat="server" ID="btnClear" Text="Clear" CssClass="Button_Normal" />
                        </td>
                        <%-- End:AA:09.232015 YRS AT-2609 Added below lines to show filters for default and offset --%>

                    </tr>
                    <%--Start: Bala: 04.20.2016: YRS-AT-2862: Loan Status--%>
                    <tr class="tabSelectedLink">
                        <td style="width: 30%; text-align: left;" colspan="6">
                             <asp:label runat="server" ID="lblLoanStatusDesc" style="font-size: 8pt; text-decoration: none; height: 12pt; color: #333333"></asp:label>
                        </td>
                    </tr>
                    <%--End: Bala: 04.20.2016: YRS-AT-2862: Loan Status--%>
                </table>
                <table class="Table_WithBorder" cellpadding="0" cellspacing="0" align="center" width="100%">
                    <tr runat="server" id="trDefaultAgeing">
                        <td style="text-align: left; width: 100%">
                            <div id="dvOffSetDefaultAgeing" style="overflow: auto; height: 400px; width: 100%">
                                <asp:GridView runat="server" ID="gvOffSetDefaultAgeing" CssClass="DataGrid_Grid"
                                    AllowSorting="true" AutoGenerateColumns="false" SelectedRowStyle-VerticalAlign="Top"
                                    Width="100%" EmptyDataText="No records found.">
                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                    <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle" /> <%--Bala: 04.19.2016: YRS-AT-2862: Freezing header--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Fund No." SortExpression="FundIdNo" HeaderStyle-Width="7%" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkFundIdNo" Text='<%#Bind("FundIdNo")%>' CommandName="FundId" CommandArgument='<%#Bind("FundIdNo")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="" InsertVisible="true" HeaderText="PerssID" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />--%>
                                        <asp:BoundField DataField="LoanDetailsId" InsertVisible="true" HeaderText="LoanDetailsId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-Width="13%" HeaderStyle-HorizontalAlign="Left"></asp:BoundField> <%--Bala: 04.20.2016: YRS-AT-2862: Width increased to 15%--%>  <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 15% to 13%--%>
                                        <asp:BoundField DataField="YMCAName" SortExpression="YMCAName" HeaderText="YMCA Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="20%" ></asp:BoundField> <%--SB | 04.02.2018 | YRS-AT-3101 | Width property added --%>
                                        <asp:BoundField DataField="LoanStatus" SortExpression="LoanStatus" HeaderStyle-Width="4%" HeaderText="Loan Status" HeaderStyle-HorizontalAlign="Left" Visible="false"/> <%--Bala: 04.19.2016: YRS-AT-2862: visibility false--%>
                                        <asp:BoundField DataField="UnPaidAmt" SortExpression="UnPaidAmt" HeaderStyle-Width="8%" HeaderText="Unpaid Amt" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                         <%-- Start| SB | 04.02.2018 | YRS-AT-3101 | Added new field to show payment method --%>
                                        <asp:BoundField DataField="PaymentMethod" SortExpression="PaymentMethod" HeaderText="Payment Method" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="6%" HorizontalAlign="left" />
                                        </asp:BoundField>
                                        <%-- End| SB | 04.02.2018 | YRS-AT-3101 | Added new field to show payment method --%>
                                        <asp:BoundField DataField="Age" SortExpression="Age" HeaderStyle-Width="6%" HeaderText="Age" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FundStatus" SortExpression="FundStatus" HeaderStyle-Width="9%" HeaderText="Fund Status" HeaderStyle-HorizontalAlign="Left"> <%--Bala: 04.21.2016: YRS-AT-2862: Width changed for 5% to 10%--%> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 10% to 9%--%>
                                            <%--<ItemStyle Width="4%" />--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DaysOverdue" SortExpression="DaysOverdue" HeaderStyle-Width="5%" HeaderText="Days Overdue" HeaderStyle-HorizontalAlign="Left"> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 6% to 5%--%>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <%-- AA:01.22.2016 YRS AT-2688 Added below lines to show default date --%>
                                        <asp:BoundField DataField="DefaultDate" SortExpression="DefaultDate" HeaderText="Default Date" HeaderStyle-HorizontalAlign="Left"  DataFormatString="{0:MM/dd/yyyy}">
                                            <ItemStyle Width="5%" HorizontalAlign="Right"  /> <%--Bala: 04.21.2016: YRS-AT-2862: Width changed from 8 to 5 %--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MessageNo" SortExpression="MessageNo" HeaderStyle-Width="7%" HeaderText="Offset Reason" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Width="13%"> <%--Bala: 04.21.2016: YRS-AT-2862: Width changed from 14 to 13 %--%>
                                            <ItemTemplate>
                                                <%--B.Jagadeesh 18.Jun.2015 BT-2699 YRS 5.0-2441 Modifications for 403b Loans--%>
                                                <asp:LinkButton runat="server" ID="lnkMaintenance" Text="Go to Loan Maintenance" CommandName="LoanMaintenance" CommandArgument='<%#Bind("FundIdNo")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- AA:09.232015 YRS AT-2609 Added below lines to filter whether the record is eligible for default or offset --%>
                                        <asp:BoundField DataField="IsOffsetEligible" SortExpression="IsOffsetEligible" HeaderStyle-Width="6%" HeaderText="IsOffsetEligible" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                        
                                    </Columns> 
                                </asp:GridView>
                            </div>
                            <%-- Start:AA:09.232015 YRS AT-2609 Added below lines to show legends for offset or default lines --%>
                            <table align="center">
                                <tr>
                                    <td align="left" class="Label_Small">
                                        <span class="BG_Colour_Loan_Default">&nbsp;&nbsp;</span> - Loans eligible for default.&nbsp&nbsp;
                                    </td>
                                    <td align="left" class="Label_Small">
                                        <span class="BG_Colour_Loan_Offset">&nbsp;&nbsp;</span> - Loans eligible for offset.
                                    </td>
                                </tr>

                            </table>
                            <%-- End:AA:09.232015 YRS AT-2609 Added below lines to show legends for offset or default lines --%>
                        </td>
                    </tr>
                    <tr id="trDefaultLoan" runat="server">
                        <td style="text-align: left; width: 100%">
                            <div style="overflow: auto; height: 400px; width: 100%" id="gvFreezeHeader">
                                <asp:GridView runat="server" ID="gvDefaultedLoans" CssClass="DataGrid_Grid"
                                    AllowSorting="true" AutoGenerateColumns="false" SelectedRowStyle-VerticalAlign="Top"
                                    Width="100%" EmptyDataText="No records found.">
                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                    <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle" /> <%--Bala: 04.19.2016: YRS-AT-2862: Freezing header--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Fund No." SortExpression="FundIdNo" HeaderStyle-Width="7%" HeaderStyle-HorizontalAlign="Left"> <%--Bala: 04.19.2016: YRS-AT-2862: Width changed from 8 to 6 %--%> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 6% to 7%--%>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkFundIdNo" Text='<%#Bind("FundIdNo")%>' CommandName="FundId" CommandArgument='<%#Bind("FundIdNo")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="" InsertVisible="true" HeaderText="PerssID" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />--%>
                                        <asp:BoundField DataField="LoanDetailsId" InsertVisible="true" HeaderText="LoanDetailsId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="12%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="YMCAName" SortExpression="YMCAName" HeaderText="YMCA Name" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="19%" /><%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 20% to 19%--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LoanStatus" SortExpression="LoanStatus" HeaderText="Loan Status" HeaderStyle-HorizontalAlign="Left" Visible="false"> <%--Bala: 04.19.2016: YRS-AT-2862: visibility false--%>
                                            <ItemStyle Width="5%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UnPaidAmt" SortExpression="UnPaidAmt" HeaderText="Default Amt." HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="6%" HorizontalAlign="Right" /> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 7% to 6%--%>
                                        </asp:BoundField>
                                        <%-- Start| SB | 04.02.2018 | YRS-AT-3101 | Added new field to show payment method --%>
                                        <asp:BoundField DataField="PaymentMethod" SortExpression="PaymentMethod" HeaderText="Payment Method" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="6%" HorizontalAlign="left" />
                                        </asp:BoundField>
                                        <%-- End| SB | 04.02.2018 | YRS-AT-3101 | Added new field to show payment method --%>
                                        <asp:BoundField DataField="Age" SortExpression="Age" HeaderText="Age" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="6%" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FundStatus" SortExpression="FundStatus" HeaderText="Fund Status" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="6%" /><%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 8% to 6%--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PhantomInterest" SortExpression="PhantomInterest" HeaderText="Phantom Int." HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="6%" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DefaultedDays" SortExpression="DefaultedDays" HeaderText="Defaulted Days" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="6%" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsTerminated" SortExpression="IsTerminated" HeaderText="IsTerminated" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="0%" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <%-- AA:01.22.2016 YRS AT-2688 Added below lines to show default date --%>
                                        <asp:BoundField DataField="DefaultDate" SortExpression="DefaultDate" HeaderText="Default Date" HeaderStyle-HorizontalAlign="Left"  DataFormatString="{0:MM/dd/yyyy}">
                                            <ItemStyle Width="7%" HorizontalAlign="Right"  /> <%--Bala: 04.19.2016: YRS-AT-2826: Width changed form 8% to 6%--%><%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 6% to 7%--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MessageNo" SortExpression="MessageNo" HeaderStyle-Width="5%" HeaderText="Offset Reason" HeaderStyle-HorizontalAlign="Left"></asp:BoundField> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 6% to 5%--%>
                                        <asp:TemplateField HeaderStyle-Width="19%"> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 14% to 19%--%>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkMaintenance" Text="Go to Loan Maintenance" CommandName="LoanMaintenance" CommandArgument='<%#Bind("FundIdNo")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%-- Start:AA:09.232015 YRS AT-2609 Added below lines to show legends for offset lines --%>
                            <table align="center">
                                <tr>
                                    <td align="left" class="Label_Small">
                                        <span class="BG_Colour_Loan_Offset">&nbsp;&nbsp;</span> - Loans eligible for offset.
                                    </td>
                                </tr>
                            </table>
                            <%-- END:AA:09.232015 YRS AT-2609 Added below lines to show legends for offset lines --%>
                        </td>
                    </tr>
                    <tr id="trUnFreeze" runat="server">
                        <td style="text-align: left; width: 100%">
                            <div id="dvUnfreeze" style="overflow: auto; width: 100%; height: 400px">
                                <asp:GridView runat="server" ID="gvUnfreeze" CssClass="DataGrid_Grid"
                                    AllowSorting="true" AutoGenerateColumns="false" SelectedRowStyle-VerticalAlign="Top"
                                    Width="100%" EmptyDataText="No records found.">
                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                    <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Fund No." SortExpression="FundIdNo" HeaderStyle-Width="6%" HeaderStyle-HorizontalAlign="Left"><%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 8% to 6%--%>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkFundIdNo" Text='<%#Bind("FundIdNo")%>' CommandName="FundId" CommandArgument='<%#Bind("FundIdNo")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="" InsertVisible="true" HeaderText="PerssID" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />--%>
                                        <asp:BoundField DataField="LoanDetailsId" InsertVisible="true" HeaderText="LoanDetailsId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="11%" /> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 12% to 11%--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="YMCAName" SortExpression="YMCAName" HeaderText="YMCA Name" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="21%" /> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 22% to 21%--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LoanStatus" SortExpression="LoanStatus" HeaderText="Loan Status" HeaderStyle-HorizontalAlign="Left" Visible="false"> <%--Bala: 04.19.2016: YRS-AT-2862: visibility false--%>
                                            <ItemStyle Width="7%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UnPaidAmt" SortExpression="UnPaidAmt" HeaderText="Unpaid Amt" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />  <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 7% to 8%--%>
                                        </asp:BoundField>
                                         <%-- Start| SB | 04.02.2018 | YRS-AT-3101 | Added new field to show payment method --%>
                                        <asp:BoundField DataField="PaymentMethod" SortExpression="PaymentMethod" HeaderText="Payment Method" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="11%" HorizontalAlign="left" />
                                        </asp:BoundField>
                                        <%-- End| SB | 04.02.2018 | YRS-AT-3101 | Added new field to show payment method --%>
                                        <asp:BoundField DataField="Age" SortExpression="Age" HeaderStyle-Width="6%" HeaderText="Age" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FundStatus" SortExpression="FundStatus" HeaderText="Fund Status" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="8%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PhantomInterest" SortExpression="PhantomInterest" HeaderText="Phantom Int." HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FrozenDays" SortExpression="FrozenDays" HeaderText="Frozen Days" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <%-- AA:01.22.2016 YRS AT-2688 Added below lines to show default date --%>
                                        <asp:BoundField DataField="DefaultDate" SortExpression="DefaultDate" HeaderText="Default Date" HeaderStyle-HorizontalAlign="Left"  DataFormatString="{0:MM/dd/yyyy}">
                                            <ItemStyle Width="8%" HorizontalAlign="Right"  />
                                        </asp:BoundField> 
                                        <asp:TemplateField HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkUnFreeze" Text="Unfreeze" CommandName="unfreeze" CommandArgument='<%#Bind("LoanDetailsId")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </div>

                        </td>
                    </tr>
                    <%--Start: AA:01.20.2016 Added below code for auto offset loans tab --%>
                    <tr id="trOffsetLoans" runat="server" style="vertical-align:top;">
                        <td style="text-align: left; width: 100%">
                            <div id="divOffsetLoans" style="overflow: auto; width: 100%; height: 400px">
                                <asp:GridView runat="server" ID="gvOffsetLoans" CssClass="DataGrid_Grid"
                                    AllowSorting="true" AutoGenerateColumns="false" SelectedRowStyle-VerticalAlign="Top"
                                    Width="100%" EmptyDataText="No records found.">
                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                    <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle" />
                                    <Columns>
                                       
                                        <asp:TemplateField HeaderText="Fund No." SortExpression="FundIdNo" HeaderStyle-Width="8%" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkFundIdNo" Text='<%#Bind("FundIdNo")%>' CommandName="FundId" CommandArgument='<%#Bind("FundIdNo")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                        <asp:BoundField DataField="PersId" InsertVisible="true" HeaderText="PersId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                        <asp:BoundField DataField="loanrequestid" InsertVisible="true" HeaderText="loanrequestid" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />                                        
                                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="15%" /> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 19% to 15%--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="YMCAName" SortExpression="YMCAName" HeaderText="YMCA Name" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="17%" /> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 22% to 17%--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LoanStatus" SortExpression="LoanStatus" HeaderText="Loan Status" HeaderStyle-HorizontalAlign="Left" Visible="false"> <%--Bala: 04.19.2016: YRS-AT-2862: visibility false--%>
                                            <ItemStyle Width="9%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="OffsetAmt" SortExpression="OffsetAmt" HeaderText="Offset Amt" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                        </asp:BoundField>                                                                                
                                         <%-- Start| SB | 04.02.2018 | YRS-AT-3101 | Added new field to show payment method --%>
                                        <asp:BoundField DataField="PaymentMethod" SortExpression="PaymentMethod" HeaderText="Payment Method" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="11%" HorizontalAlign="left" /> 
                                        </asp:BoundField>
                                        <%-- End| SB | 04.02.2018 | YRS-AT-3101 | Added new field to show payment method --%>
                                        <asp:BoundField DataField="Offsetdate" SortExpression="Offsetdate" HeaderText="Offset Date" HeaderStyle-HorizontalAlign="Left"  DataFormatString="{0:MM/dd/yyyy}">
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MessageNo" SortExpression="MessageNo" HeaderText="Offset Reason" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="9%"  /> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 10% to 9%--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DefaultDate" SortExpression="DefaultDate" HeaderText="Default Date" HeaderStyle-HorizontalAlign="Left"  DataFormatString="{0:MM/dd/yyyy}">
                                            <ItemStyle Width="8%" HorizontalAlign="Right"  />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Width="17%"> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 20% to 17%--%>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkReport" Text="Generate Letter" CommandName="Report" CommandArgument='<%#Bind("DefaultDate")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PrintlettersId" InsertVisible="true" HeaderText="PrintlettersId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />                                        
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <table align="center">
                                <tr>
                                    <td align="left" class="Label_Small">
                                        <span class="BG_Colour_Loan_Report">&nbsp;&nbsp;</span> - Loans for which letter has already been generated.
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                    <%--End: AA:01.20.2016 Added below code for auto offset loans tab --%>
                    <%--Start: AA:04.18.2016 YRS-AT-2831:Added below code for auto defaulted loans tab --%>
                    <tr id="trAutoDefaultLoans" runat="server" style="vertical-align:top;">
                        <td style="text-align: left; width: 100%">
                            <div id="divAutoDefaultLoans" style="overflow: auto; width: 100%; height: 400px">
                                <asp:GridView runat="server" ID="gvAutoDefaultLoans" CssClass="DataGrid_Grid"
                                    AllowSorting="true" AutoGenerateColumns="false" SelectedRowStyle-VerticalAlign="Top"
                                    Width="100%" EmptyDataText="No records found.">
                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                    <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle" /> 
                                    <Columns>                                       
                                        <asp:TemplateField HeaderText="Fund No." SortExpression="FundIdNo" HeaderStyle-Width="8%" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkFundIdNo" Text='<%#Bind("FundIdNo")%>' CommandName="FundId" CommandArgument='<%#Bind("FundIdNo")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                        <asp:BoundField DataField="PersId" InsertVisible="true" HeaderText="PersId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                        <asp:BoundField DataField="loanrequestid" InsertVisible="true" HeaderText="loanrequestid" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />                                        
                                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="21%" /> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 19% to 21%--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="YMCAName" SortExpression="YMCAName" HeaderText="YMCA Name" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="28%" /> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 22% to 28%--%>
                                        </asp:BoundField>                                        
                                        <asp:BoundField DataField="DefaultDate" SortExpression="DefaultDate" HeaderText="Default Date" HeaderStyle-HorizontalAlign="Left"  DataFormatString="{0:MM/dd/yyyy}">
                                            <ItemStyle Width="8%" HorizontalAlign="Right"  />
                                        </asp:BoundField>
                                          <%-- Start| SB | 04.02.2018 | YRS-AT-3101 | Added new field to show payment method --%>
                                        <asp:BoundField DataField="PaymentMethod" SortExpression="PaymentMethod" HeaderText="Payment Method" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="11%" HorizontalAlign="left" />
                                        </asp:BoundField>
                                        <%-- End| SB | 04.02.2018 | YRS-AT-3101 | Added new field to show payment method --%>
                                        <asp:TemplateField HeaderStyle-Width="24%"> <%--SB | 04.02.2018 | YRS-AT-3101 | Width changed for 20% to 24%--%>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkReport" Text="Generate Letter" CommandName="Report" CommandArgument='<%#Bind("DefaultDate")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PrintlettersId" InsertVisible="true" HeaderText="PrintlettersId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />                                        
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <table align="center">
                                <tr>
                                    <td align="left" class="Label_Small">
                                        <span class="BG_Colour_Loan_Report">&nbsp;&nbsp;</span> - Loans for which letter has already been generated.
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                    <%--End: AA:04.18.2016 YRS-AT-2831:Added below code for auto defaulted loans tab --%>
                    <%--Start: AA:06.28.2016 YRS-AT-2830:Added below code for auto closed loans tab --%>
                    <tr id="trAutoClosedLoans" runat="server" style="vertical-align:top;">
                        <td>
                            <div id="divAutoClosedLoans" style="overflow: auto; width: 100%; height: 400px">
                                <asp:GridView runat="server" ID="gvAutoClosedLoans" CssClass="DataGrid_Grid"
                                    AllowSorting="true" AutoGenerateColumns="false" SelectedRowStyle-VerticalAlign="Top"
                                    Width="100%" EmptyDataText="No records found.">
                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                    <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Fund No." SortExpression="FundIdNo" HeaderStyle-Width="9%" HeaderStyle-HorizontalAlign="Left"> <%--SB | 04.02.2018 | YRS-AT-3101 | Width property changed from 10% to 9% --%>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkFundIdNo" Text='<%#Bind("FundIdNo")%>' CommandName="FundId" CommandArgument='<%#Bind("FundIdNo")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PersId" InsertVisible="true" HeaderText="PersId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                        <asp:BoundField DataField="loanrequestid" InsertVisible="true" HeaderText="loanrequestid" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                        <asp:BoundField DataField="LoanNumber" InsertVisible="true" HeaderText="LoanNumber" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="15%" /> <%--SB | 04.02.2018 | YRS-AT-3101 | Width property changed from 25% to 15% --%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="YMCAName" SortExpression="YMCAName" HeaderText="YMCA Name" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="23%" />  <%--SB | 04.02.2018 | YRS-AT-3101 | Width property changed from 25% to 23% --%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ClosedDate" SortExpression="ClosedDate" HeaderText="Closed Date" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:MM/dd/yyyy}">
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                         <%-- Start| SB | 04.02.2018 | YRS-AT-3101 | Added new field to show payment method --%>
                                        <asp:BoundField DataField="PaymentMethod" SortExpression="PaymentMethod" HeaderText="Payment Method" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="11%" HorizontalAlign="left" />
                                        </asp:BoundField>
                                        <%-- End| SB | 04.02.2018 | YRS-AT-3101 | Added new field to show payment method --%>
                                        <asp:TemplateField HeaderStyle-Width="16%">  <%--SB | 04.02.2018 | YRS-AT-3101 | Width property changed from 15% to 16% --%>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkParticipantReport" Text="Generate Participant Letter" CommandName="ReportPartipant" CommandArgument='<%#Bind("PersId")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="16%"> <%--SB | 04.02.2018 | YRS-AT-3101 | Width property changed from 15% to 16% --%>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkYMCAReport" Text="Generate YMCA Letter" CommandName="ReportYMCA" CommandArgument='<%#Bind("PersId")%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PersonPrintlettersId" InsertVisible="true" HeaderText="PersonPrintlettersId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                        <asp:BoundField DataField="YMCAPrintlettersId" InsertVisible="true" HeaderText="YMCAPrintlettersId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                        <asp:BoundField DataField="Ymcaid" InsertVisible="true" HeaderText="Ymcaid" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <table align="center">
                                <tr>
                                    <td align="left" class="Label_Small">
                                        <span class="BG_Colour_Loan_Participant_Report">&nbsp;&nbsp;</span> - Loans for which participant letter has already been generated.<br />
                                        <span class="BG_Colour_Loan_YMCA_Report">&nbsp;&nbsp;</span> - Loans for which YMCA letter has already been generated.<br />
                                        <%-- Start - MMR | 2016.07.21 | YRS-AT-3050 | Corrected mispelled words in the text--%>
                                        <%--<span class="BG_Colour_Loan_Report">&nbsp;&nbsp;</span> - Loans for which Both letter has already been generated.--%>
                                        <span class="BG_Colour_Loan_Report">&nbsp;&nbsp;</span> - Loans for which both letters have already been generated.
                                        <%-- End - MMR | 2016.07.21 | YRS-AT-3050 | Corrected mispelled words in the text--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="NormalMessageText"><b>Note:-</b> <%= YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_LOAN_GENERATEYMCALINK_HIDE)%> </span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <%--End: AA:06.28.2016 YRS-AT-2830:Added below code for auto closed loans tab --%>
                    <tr>
                        <td style="text-align:right;width: 100%">
                            <label class="Label_Small">No of record(s) : </label> <asp:Label ID="LblCount" runat="server" CssClass="Label_Small" ></asp:Label>
                        </td>
                    </tr>
                </table>


                <table class="Td_ButtonContainer" style="width: 100%">
                    <tr>
                        <td style="text-align: right">
                            <%-- AA:09.232015 YRS AT-2609 Added below lines to provide a button to print the list of persons shown in the grid --%>                            
                            <asp:Button runat="server" ID="btnPrintList" Text="Print List" CssClass="Button_Normal" />&nbsp;&nbsp;
                            <asp:Button runat="server" ID="btnClose" Text="Close" CssClass="Button_Normal" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:PlaceHolder ID="PlaceHolderMessage" runat="server"></asp:PlaceHolder>
    <div id="divConfirmDialog" title="UnFreeze Loan" style="overflow: visible;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <input type="hidden" id="hdnValue" />
                <input type="hidden" id="hdnFundNo" />
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
    <%--Start: AA:01.20.2016 Added below code for auto offset loans tab --%>
    <div id="dvReportConfirm" title="UnFreeze Loan" style="overflow: visible;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>                
                <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                    <tr>
                        <td>
                            <label id="lblReportMsg" class="Label_Small"></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                        </td>
                    </tr>
                    <tr id="trNote">
                        <td>
                            <label id="Label1" class="Label_Small">Note : <br /> This report will be copied into IDM for the first time then after it just opens the report</label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="dvProcessing" style="width: 25px">
        <label id="lblProcessing" class="Label_Small"></label>
    </div>
    <%--End: AA:01.20.2016 Added below code for auto offset loans tab --%>
</asp:Content>
