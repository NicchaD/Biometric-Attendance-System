<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master"
    CodeBehind="GenerateRMD.aspx.vb" Inherits="YMCAUI.GenerateRMD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="JS/jquery-1.5.1.min.js"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="JS/jquery-bPopup.js"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />

    <style type="text/css">
        .RMDRowHighlight {
            background-color: #FFB6C1;
        }

        th {
            position: relative;
            cursor: default;
            top: expression(document.getElementById("dvGridView").scrollTop-2);
            z-index: 10;
        }

        DataGrid_HeaderStyle.locked {
            z-index: 99;
        }
    </style>

    <script type="text/javascript">

        function showConfirmdialog() {
            $('#ConfirmDialog').dialog("open");
        }

        function closeConfirmdialog() {
            $('#ConfirmDialog').dialog('close');
        }

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    args.set_errorHandled(true);
                    // BindEvents();
                }
            }
            $('#ConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                width: 350, height: 350,
                title: "Generate RMD",
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        });
        <%--START: MMR | 2016.10.19 | YRS-AT-2922 | Function to change Year in RMD month based on RMD Year selected --%>
        function AppendYearRMDMonth() {                        
            var selRMDyear = $('#dropdownlistRMDYear').find(":selected").text();

            var selRMDMonthDec = "Dec - " + selRMDyear;
            var selRMDMonthMar = "Mar - " + (parseInt(selRMDyear) + 1);

            $("#dropdownlistRMDMonth option").eq(1)[0].innerHTML = selRMDMonthDec;
            $("#dropdownlistRMDMonth option").eq(2)[0].innerHTML = selRMDMonthMar;
           
        }
        <%--END: MMR | 2016.10.19 | YRS-AT-2922 | Fucntion to change Year in RMD month based on RMD Year selected --%>
    </script>
</asp:Content>
<asp:Content ID="cntGenerateRMD" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="GRMDScriptManager" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="uplGenerateRMD" runat="server">
        <ContentTemplate>
            <table class="Table_WithBorder" cellpadding="0" cellspacing="0" width="100%" align="center">
                <tr>
                    <td style="text-align: left; width: auto">
                        <asp:Label ID="lblMRDMsg" CssClass="Label_Small" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkGenerateNextYear" CssClass="Link_SmallBold" runat="server"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2">                       
                        <table class="Table_WithoutBorder" cellpadding="1" cellspacing="0" border="0" width="100%"
                            style="height: 400px; text-align: left;">
                            <tr>
                                <%-- START: MMR | 2016.10.19 |YRS-AT-2922 |Commneted Existing code  --%>
                                <%--<td class="Table_WithBorder" align="left" width="15%">

                                    <div style="overflow: auto; width: 100%; height: 100%">
                                        <table width="100%" align="left">
                                            <tr>
                                                <td class="menuitem">Select RMD Year
                                                </td>
                                            </tr>
                                            <asp:Repeater ID="rptMenuHistory" runat="server" Visible="false">
                                                <ItemTemplate>
                                                    <tr valign="top">
                                                        <td id="liRMDYear" runat="server" style="padding-bottom: 10px; list-style-type: none; text-align: left; width: 100%;">
                                                            <asp:LinkButton runat="server" ID="lnkRMDYear" Text='<%# Eval("Year")%>' OnClick="lnkRMDYear_Click" CssClass="Link_SmallBold"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                </td>--%>
                                <%-- END: MMR | 2016.10.19 |YRS-AT-2922 |Commneted Existing code  --%>

                                <%-- START: MMR | 2016.10.19 |YRS-AT-2922 |Added new design to find data by year,month and fund no  --%>
                                <td style="vertical-align: top; width: 150px; text-align: left">                                   
                                            <div style="overflow: auto; width: 100%; border-top-style: none; border-right-style: none; border-left-style: none; height: 400px; border-bottom-style: none">                                               
                                                            <table class="Table_WithBorder" id="tableSearchCriteria" cellspacing="1" cellpadding="1" border="0" style="width: 100%">
                                                                <tr class="DataGrid_AlternateStyle">
                                                                    <td>
                                                                        <b>Filter Criteria</b>
                                                                    </td>
                                                                </tr>
                                                                <tr class="DataGrid_HeaderStyle">
                                                                    <td>
                                                                        <asp:Label ID="labelYears" Text="RMD Year" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="dropdownlistRMDYear" CssClass = "DropDown_Normal" style="width:100%;" onchange="AppendYearRMDMonth()"></asp:DropDownList> <%--MMR | 2016.10.19 | YRS-AT-2922 | Called function on dropdown change event--%>
                                                                    </td>
                                                                </tr>  
                                                                <tr class="DataGrid_HeaderStyle">
                                                                    <td>Due Date
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="dropdownlistRMDMonth" CssClass ="DropDown_Normal" style="width:100%;" EnableViewState="true">                                                                                                                                                                                            
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>                                                               
                                                            </table>
                                                            <table class="Table_WithBorder" id="tableSearchFundNo" cellspacing="1" cellpadding="1" border="0" style="width: 100%">
                                                                <tr>
                                                                    <td class="Label_Small">Fund No.:
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="textboxFundNo" CssClass ="TextBox_Normal" runat="server" MaxLength="20" Width="100%" onkeypress="ValidateNumeric();"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Button runat="server" ID="buttonSearch" Text="View >>" CssClass="Button_Normal" />
                                                                    </td>
                                                                </tr>
                                                           </table>                                                      
                                            </div>                                                                                          
                        </td>
                       <%-- END: MMR | 2016.10.19 |YRS-AT-2922 |Added new design to find data by year,month and fund no  --%>
                            
                                <td width="85%" valign="top" align="left">

                                    <table class="Table_WithOutBorder" cellpadding="1" cellspacing="1" width="100%" border="1">
                                        <tr style="background-color: #93BEEE; height: 25px;">
                                            <td>
                                                <asp:Label runat="server" ID="lblRMDSelectedYear"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="overflow: scroll; width: 100%; height: 400px" id="dvGridView">
                                                    <asp:GridView runat="server" ID="gvGenerateMRDRecords" CssClass="DataGrid_Grid"
                                                        AllowSorting="true" AutoGenerateColumns="false" SelectedRowStyle-VerticalAlign="Top" Width="100%" EmptyDataText="No records found."> <%-- MMR | 2016.10.19 |YRS-AT-2922 | Display text when no record found at the time of binding data to grid --%>
                                                        <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                                        <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                        <RowStyle CssClass="DataGrid_NormalStyle" />
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                                        <Columns>
                                                            <%-- START: MMR | 2016.10.19 |YRS-AT-2922 | Changed width of columns --%>
                                                            <asp:BoundField DataField="FundIdNo" SortExpression="FundIdNo" HeaderText="Fund No." HeaderStyle-Width="8%"/>
                                                            <asp:BoundField DataField="PlanType" SortExpression="PlanType" HeaderText="Plan Type" HeaderStyle-Width="9%" />
                                                            <asp:BoundField DataField="CurrentBalance" SortExpression="CurrentBalance" HeaderText="Current Balance" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="12%" />                                                        
                                                            <asp:BoundField DataField="Bal.RMD Calculated on" SortExpression="Bal.RMD Calculated on" HeaderText="Bal. RMD Calc. on" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="13%"/> <%--START: MMR | 2016.10.19 |YRS-AT-2922 | Added field to show balance on which RMD amount was calculated--%>
                                                            <asp:BoundField DataField="MRDAmount" SortExpression="MRDAmount" HeaderText="RMD Amount" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="12%" />
                                                            <asp:BoundField DataField="PaidAmount" SortExpression="PaidAmount" HeaderText="Paid Amount" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="11%" />
                                                            <asp:BoundField DataField="MRDExpireDate" SortExpression="MRDExpireDate" HeaderText="Due Date" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="9%" />
                                                            <asp:BoundField DataField="StatusTypeDescription" SortExpression="StatusTypeDescription" HeaderText="Fund Status" HeaderStyle-Width="12%" />
                                                            <asp:BoundField DataField="Ex.RMDEfforts" SortExpression="Ex.RMDEfforts" HeaderText="Exh. RMD Sett. Eff." HeaderStyle-Width="14%" />
                                                            <%-- END: MMR | 2016.10.19 |YRS-AT-2922 | Changed width of columns --%>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td style="text-align: left; height: 5px;">
                                    <table style="text-align: left; height: 5px;">
                                        <tr>
                                            <td class="RMDRowHighlight" style="width: 10px;"></td>
                                            <td>
                                                <label class="Label_Small">Exhausted DB / RMD Settlement Efforts</label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <table class="Table_WithOutBorder" width="100%">
                            <tr>
                                <td class="Td_ButtonContainer" align="right">
                                    <asp:Button runat="server" ID="btnPrintLetters" Text="Print Letters ..." class="Button_Normal" Visible="false" />
                                    &nbsp;&nbsp;&nbsp;
                            <asp:Button runat="server" ID="btnClose" Text="Close" class="Button_Normal" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>    
    </asp:UpdatePanel>
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
</asp:Content>
