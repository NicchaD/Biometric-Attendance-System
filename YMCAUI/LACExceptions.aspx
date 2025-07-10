
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LACExceptions.aspx.vb" MasterPageFile="~/MasterPages/YRSMain.Master" Inherits="YMCAUI.LACExceptions" %>

<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl_new.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="~/UserControls/YMCA_Footer_WebUserControl.ascx" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="YRSCustomControls" %>

<asp:Content ID="contentHeader" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    <style type="text/css">
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
        .cursorshow {
            cursor:pointer;
        }
        .hideColumn
        {
            display: none;
        }
    </style>
    <script type="text/javascript" language="javascript">
       
          function navigateUrl(url) {
            $('#<%=(Master.FindControl("UpdateProgress1")).ClientID%>').show();
            window.location.href = url;
        }

        function CallLetter() {
            window.open('CallReport.aspx', 'ReportPopUp1', 'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
        }

        function showToolTip(divId,linkId, Reason) {
            if (null != divId) {
                var elementRef = document.getElementById(divId);
                if (elementRef != null) {
                    elementRef.style.position = 'absolute';
                    elementRef.style.left = event.clientX + 5 + document.body.scrollLeft;
                    elementRef.style.top = event.clientY + 5 + document.body.scrollTop;
                    elementRef.style.width = '270';<%--VC| 10/10/2018 | YRS-AT-4017 | Reduced tooltip width--%>
                    elementRef.style.visibility = 'visible';
                    elementRef.style.display = 'block';
                }
            }
            var lblNote = document.getElementById("<%=lblComments.ClientID%>");
                lblNote.innerHTML = '' + Reason <%--VC| 10/10/2018 | YRS-AT-4017 | Binding to html instead of setting inner text--%>
            }
        

        //to hide tool tip when mouse is removed
        function hideToolTip(divId, linkId) {
            if (null != divId) {
                var elementRef = document.getElementById(divId);
                if (elementRef != null) {
                    elementRef.style.visibility = 'hidden';
                }
            }
            if (null != linkId) {
                var elementBak = document.getElementById(linkId);
                if (elementBak != null) {

                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID ="contentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="dbScriptManagerProxy" runat="server"></asp:ScriptManagerProxy>
    <asp:PlaceHolder id="PlaceHolderSecurityCheck" runat="server"></asp:PlaceHolder><%--VC | 2018.11.09 | YRS-AT-4017 -  Added placeholder--%>
    <div class="Div_Center" style="width:100%;height:100%;">
        <asp:UpdatePanel ID="upLoan" runat="server">
                <ContentTemplate>
                <table style="width: 100%; vertical-align: top; border:0px;" >
                    <tr>
                        <td id="Td1" runat="server" style="width: 20%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=1')">Loans Pending Approval</td>
                        <td id="Td2" runat="server" style="width: 17%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=2')">Loan Processing</td>
                        <td id="Td3" runat="server" style="width: 17%;" class="tabSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=3')">Exceptions/On hold</td>
                        <td id="Td4" runat="server" style="width: 15%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=4')">Search</td>
                        <td id="Td5" runat="server" style="width: 17%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=5')">Statistics</td>
                        <td id="Td6" runat="server" style="width: 14%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=6')">Rate</td>
                    </tr>
                </table>
                <table class="Table_WithBorder" style="width:100%;height:500px;" border="0">
                    <%-- Filter--%>
                    <tr>
                        <td style="vertical-align:top;height:15%;" class="Table_WithBorder">
                            <table border="0" style="width:100%;">
                                <tr>
                                    <td class="td_Text" style="width:100%;">
                                        <table style="width:100%;">
                                            <tr>
                                                <td id="headerSectionFilter" class="td_Text" style="text-align:left;width:100%;">Filter</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align:top;height:40%;" colspan="2">
                                        <div id="divFilter">
                                            <table border="0" style="width:100%;">
                                                <tr>
                                                    <td style="width:10%;"><asp:Label ID="lblFundId" runat="server" CssClass="Label_Small">Fund No.:</asp:Label></td>
                                                    <td style="width:16%;"><asp:TextBox ID="txtFundNo" runat="server" CssClass="TextBox_Normal" Width="100px" maxlength="20" onkeypress="ValidateNumeric();"></asp:TextBox></td>
                                                    <td style="width:10%;" class="Label_Small"><asp:CheckBox ID="chkCHECK" runat="server" CssClass="CheckBox_Normal"  text="CHECK" AutoPostBack="false" Style="display: none;"/></td>
                                                    <td style="width:44%;" class="Label_Small"><asp:CheckBox ID="chkEFT" runat="server" CssClass="CheckBox_Normal" text="EFT" AutoPostBack="false" Style="display: none;"/></td>
                                                    <td style="width:20%;text-align:left;">
                                                        <asp:Button ID="btnFind" runat="server" CssClass="Button_Normal" Text="Find"></asp:Button>
                                                        &nbsp;
                                                        <asp:Button ID="btnClear" runat="server" CssClass="Button_Normal" Text="Clear"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <%-- Pending Loans --%>
                    <tr>
                        <td class="Table_WithBorder" style="height:85%;">
                            <table style="height:100%;width:100%">
                                <tr>
                                    <td style="width:100%;height:90%;">
                                        <%-- Reason Tool tip --%>
                                        <div id="Tooltip" runat="server" style="z-index: 1000; width: 90%; border-left: 1px solid silver;
                                                border-top: 1px solid silver; border-right: 1px solid silver; border-bottom: 1px solid #cccccc;
                                                padding: 3px; position: absolute; top: 0; left: 0; background: LightYellow; color: black;
                                                display: none; /* does not work in ie6 */	font-size: 7pt; font-family: verdana;
                                                margin: 0; overflow: visible;">
                                                <asp:Label runat="server" ID="lblComments" Style="display: block; width: 90%; overflow: visible;
                                                    font-size: x-small;"></asp:Label>
                                            </div>


                                        <div style="overflow: auto; width: 100%;height:100%; border-top-style: none; border-right-style: none;border-left-style: none; position: static; border-bottom-style: none">
                                            <asp:GridView ID="gvLoansForExceptions" AllowSorting="true" runat="server" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="false" EmptyDataText="No records found."> <%--VC | 2018.11.14 | YRS-AT-4017 | Removed paging from grid--%>
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle" /><%--VC | 2018.11.26 | YRS-AT-4017 |Added class 'fixedHeader' to fix grid header while scrolling the grid--%>
                                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                                <SortedAscendingHeaderStyle  CssClass="sortasc" />
                                                <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                                <Columns>
                                                    <asp:BoundField DataField="PersId" HeaderText="PersId" ReadOnly="True" ItemStyle-HorizontalAlign ="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="8%" SortExpression="PersId" Visible="false"/>
                                                    <asp:TemplateField HeaderText="Fund No." HeaderStyle-Width="6%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="FundNo">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="FundNo" SortExpression="FundNo" commandname="personmaintenanceloan" CommandArgument='<%# Eval("FundNo")%>' runat="server" Text='<%# Eval("FundNo")%>'> </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Name" HeaderText="Participant Name" ReadOnly="True" ItemStyle-HorizontalAlign ="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="14%" SortExpression="Name"/>
                                                    <asp:BoundField DataField="YMCAName" HeaderText="YMCA Name" ReadOnly="True" ItemStyle-HorizontalAlign ="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="15%" SortExpression="YMCAName"/>
                                                    <asp:BoundField DataField="SavingsBalance" HeaderText="Savings Balance" ReadOnly="True" ItemStyle-HorizontalAlign ="Right" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="8%" SortExpression="SavingsBalance"/>
                                                    <asp:BoundField DataField="FundStatus" HeaderText="Fund Status" ReadOnly="True" ItemStyle-HorizontalAlign ="Right" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%" SortExpression="FundStatus"/>
                                                    <asp:BoundField DataField="LoanStatus" HeaderText="Status" ReadOnly="True" ItemStyle-HorizontalAlign ="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="4%" SortExpression="LoanStatus"/>
                                                    <asp:BoundField DataField="RequestDate" HeaderText="Request Date" ReadOnly="True" ItemStyle-HorizontalAlign ="Left" HeaderStyle-HorizontalAlign="left" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" ItemStyle-Width="7%" SortExpression="RequestDate"/> <%--VC | 2018.11.14 | YRS-AT-4017 | Added time format with date format--%>
                                                    <asp:BoundField DataField="PaymentMethod" HeaderText="Payment Method" ReadOnly="True" ItemStyle-HorizontalAlign ="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="6%" SortExpression="PaymentMethod"/>
                                                    <asp:BoundField DataField="RequestedAmount" HeaderText="Requested Amt." ReadOnly="True" ItemStyle-HorizontalAlign ="Right" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="8%" SortExpression="RequestedAmount"/>
                                                    <asp:TemplateField HeaderText="OND Requested" HeaderStyle-Width="7%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center" SortExpression="IsONDRequested"> <%--VC | 2018.11.06 | YRS-AT-4017 |Added sort expression--%>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkONDRequested" runat="server" Checked='<%# Eval("IsONDRequested")%>' Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reason" HeaderStyle-Width="12%" ItemStyle-CssClass="cursorshow" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  SortExpression="Reason">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Reason" runat="server"  Text='<%# Eval("Reason").ToString().Substring(0, Math.Min(15, Eval("Reason").ToString().Length)) + "..."%>' />
                                                             <asp:Label ID="ReportReason" runat="server"  Text='<%# Eval("Reason")%>' CssClass="hideColumn" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Link to Loan" HeaderStyle-Width="9%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgProcessing" runat="server"  ImageUrl="images/process.gif" alt="Go to Loan Processing" style="cursor:pointer;" CommandName="loanrequestandprocessing" CommandArgument='<%#Bind("FundNo")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="text-align: right; width: 100%;height:10%;">
                                        <label class="Label_Small" id="lblNoRecord" runat="server">No of record(s) : </label>
                                        <span id="lblRecords" class="Label_Small" runat="server"></span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right;width:100%;" class="td_Text">
                            <asp:Button ID="btnPrintExceptions" runat="server" Text="Print List" class="Button_Normal"/>&nbsp;
                            <asp:Button ID="btnClose" runat="server" Text="Close" class="Button_Normal"/>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnFind" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btnPrintExceptions" EventName="click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

