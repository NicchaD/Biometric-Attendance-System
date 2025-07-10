<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LACStatistics.aspx.vb" MasterPageFile="~/MasterPages/YRSMain.Master" Inherits="YMCAUI.LACStatistics" %>

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
    </style>
   <script type="text/javascript" language="javascript">
       function navigateUrl(url) {
           $('#<%=(Master.FindControl("UpdateProgress1")).ClientID%>').show();
           window.location.href = url;
       }
   </script>
</asp:Content>
<asp:Content ID ="contentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:PlaceHolder id="PlaceHolderSecurityCheck" runat="server"></asp:PlaceHolder><%--VC | 2018.11.09 | YRS-AT-4017 -  Added placeholder--%>
    <asp:ScriptManagerProxy ID="dbScriptManagerProxy" runat="server"></asp:ScriptManagerProxy>
    <div class="Div_Center" style="width:100%;height:100%;">
        <asp:UpdatePanel ID="upLoan" runat="server">
                <ContentTemplate>
                    <table style="width: 100%; vertical-align: top; border:0px;" >
                        <tr>
                            <td id="tdLoansApproval" runat="server" style="width: 20%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=1')">Loans Pending Approval</td>
                            <td id="tdLoanProcessing" runat="server" style="width: 17%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=2')">Loan Processing</td>
                            <td id="tdExceptions" runat="server" style="width: 17%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=3')">Exceptions/On hold</td>
                            <td id="tdSearch" runat="server" style="width: 15%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=4')">Search</td>
                            <td id="tdStatistics" runat="server" style="width: 17%;" class="tabSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=5')">Statistics</td>
                            <td id="tdRate" runat="server" style="width: 14%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=6')">Rate</td>
                        </tr>
                    </table>
                    <table class="Table_WithBorder" style="width:100%;height:500px;" border="0">
                    <%-- Statistics Tab --%>
                        <tr>
                            <td class="Table_WithBorder" style="height:100%;">
                                <table style="height:100%;width:100%">
                                    <tr>
                                        <td class="td_Text" style="width:100%;">
                                            <table style="width:100%;">
                                                <tr>
                                                    <td id="Td1" class="td_Text" style="text-align:left;width:25%;text-wrap:avoid;">Statistics</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="overflow: auto; width: 100%; height: 100%; border-top-style: none; border-right-style: none; border-left-style: none; position: static; border-bottom-style: none;">
                                                <div>
                                                    <table class="DataGrid_Grid" rules="all" border="1" id="gvloanstatistics" style="width: 25%; border-collapse: collapse;">
                                                        <tr class="DataGrid_HeaderStyle">
                                                            <th scope="col" style="font-size:12px;width:50%;text-align:center;">Status</th>
                                                            <th scope="col" style="font-size:12px;text-align:center;">Count</th>
                                                        </tr>
                                                        <tr class="DataGrid_NormalStyle">
                                                            <td style="color: green; text-indent: 40px; font-size: 12px;text-align:left;">paid</td>
                                                            <td style="padding-right: 40px; font-size: 12px;text-align:right;">
                                                                <asp:Label ID="lblPaid" runat="server" Text="0" ></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="DataGrid_AlternateStyle">
                                                            <td style="color: green; text-indent: 40px; font-size: 12px;text-align:left;">accepted</td>
                                                            <td style="padding-right: 40px; font-size: 12px;text-align:right;">
                                                                <asp:Label ID="lblAccepted" runat="server" Text="0" ></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="DataGrid_NormalStyle">
                                                            <td style="color: red; text-indent: 40px; font-size: 12px;text-align:left;">declined</td>
                                                            <td style="padding-right: 40px; font-size: 12px;text-align:right;">
                                                                <asp:Label ID="lblDeclined" runat="server" Text="0" ></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="DataGrid_AlternateStyle">
                                                            <td style="color: red; text-indent: 40px; font-size: 12px;text-align:left;">canceled</td>
                                                            <td style="padding-right: 40px; font-size: 12px;text-align:right;">
                                                                <asp:Label ID="lblCanceled" runat="server" Text="0" ></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="DataGrid_NormalStyle">
                                                            <td style="color: red; text-indent: 40px; font-size: 12px;text-align:left;">expired</td>
                                                            <td style="padding-right: 40px; font-size: 12px;text-align:right;">
                                                                <asp:Label ID="lblExpired" runat="server" Text="0" ></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="DataGrid_AlternateStyle">
                                                            <td style="color: red; text-indent: 40px; font-size: 12px;text-align:left;">withdrawn</td>
                                                            <td style="padding-right: 40px; font-size: 12px;text-align:right;">
                                                                <asp:Label ID="lblWithdrawn" runat="server" Text="0" ></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="DataGrid_NormalStyle">
                                                            <td style="color: goldenrod; text-indent: 40px; font-size: 12px;text-align:left;">pending</td>
                                                            <td style="padding-right: 40px; font-size: 12px;text-align:right;">
                                                                <asp:Label ID="lblPending" runat="server" Text="0" ></asp:Label></td>
                                                        </tr>
                                                        <tr class="DataGrid_AlternateStyle">
                                                            <td style="color: goldenrod; text-indent: 40px; font-size: 12px;text-align:left;">approved</td>
                                                            <td style="padding-right: 40px; font-size: 12px;text-align:right;">
                                                                <asp:Label ID="lblApproved" runat="server" Text="0" ></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="DataGrid_NormalStyle">
                                                            <td style="color: goldenrod; text-indent: 40px; font-size: 12px;text-align:left;">rejected</td>
                                                            <td style="padding-right: 40px; font-size: 12px;text-align:right;">
                                                                <asp:Label ID="lblRejected" runat="server" Text="0" ></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr class="DataGrid_NormalStyle">
                                                            <td style="font-weight: bold; text-indent: 40px; font-size: 12px;text-align:left;">Total</td>
                                                            <td style="font-weight: bold; padding-right: 40px; font-size: 12px;text-align:right;">
                                                                <asp:Label ID="lblTotal" runat="server" Text="0"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right;width:100%;" class="td_Text"><asp:Button ID="btnClose" runat="server" Text="Close" class="Button_Normal"/></td>
                        </tr>
                    </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

