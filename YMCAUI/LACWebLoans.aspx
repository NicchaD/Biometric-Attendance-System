<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LACWebLoans.aspx.vb" MasterPageFile="~/MasterPages/YRSMain.Master" Inherits="YMCAUI.LACWebLoans" %>
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

       .ClassHide {
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

       function SelectAllStatus() {
           if ($('#chkAll').is(":checked")) {
               $("#tblFilerStatus input[type=checkbox]").each(function () {
                   $(this).attr("checked", true);
               });
           }
           else
           {
               $("#tblFilerStatus input[type=checkbox]").each(function () {
                   $(this).attr("checked", false);
               });
           }
       }
       function SelectStatus() {
           var CheckedCount = $("#tblFilerStatus input:checkbox:checked").length;
           if (CheckedCount == 9 && $('#chkAll').is(":checked") == false) {
               $('#chkAll').attr("checked", true);
           }
           else
           {
               $('#chkAll').attr("checked", false);
           }
       }
       function ValidateAlpha() {
           if (((event.keyCode > 64) && (event.keyCode < 91)) || ((event.keyCode > 96) && (event.keyCode < 123)) || ((event.keyCode == 32))) {
               event.returnValue = true;
           }
           else {
               event.returnValue = false;
           }
       }
       function ValidateAmount(evt, element) {
           var charCode = (evt.which) ? evt.which : event.keyCode
           if ((charCode != 46 || $(element).val().indexOf('.') != -1) && (charCode < 48 || charCode > 57))
               return false;
           return true;
       }
       function ValidateFundId(evt, element) {
           var charCode = (evt.which) ? evt.which : event.keyCode
           if ((charCode < 48 || charCode > 57))
               return false;
           return true;
       }
      
       $(document).ready(function () {
           $('#txtAmount').keypress(function (event) {
               return ValidateAmount(event, this)
           });
           $('#txtFundId').keypress(function (event) {
               return ValidateFundId(event, this)
           });
       });

       function EnableDirty() {
           $('#HiddenFieldDirty').val('true');
       }

       function ClearDirty() {
           $('#HiddenFieldDirty').val('false');
       }
   </script>
</asp:Content>
<asp:Content ID ="contentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:PlaceHolder id="PlaceHolderSecurityCheck" runat="server"></asp:PlaceHolder><%--VC | 2018.11.09 | YRS-AT-4017 -  Added placeholder--%>
    <asp:ScriptManagerProxy ID="dbScriptManagerProxy" runat="server"></asp:ScriptManagerProxy>
    <div class="Div_Center" style="width:100%;height:100%;">
        <asp:UpdatePanel ID="upLoan" runat="server">
            <ContentTemplate>
                <div class="Div_Center" style="width: 100%; height: 100%;">
                    <div id="Div1">
                        <table style="width: 100%; vertical-align: top; border: 2px;">
                            <tr>
                                <td id="tdLoansApproval" runat="server" style="width: 20%;" class="tabSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=1')">Loans Pending Approval</td>
                                <td id="Td2" runat="server" style="width: 17%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=2')">Loan Processing</td>
                                <td id="Td3" runat="server" style="width: 17%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=3')">Exceptions/On hold</td>
                                <td id="tdSearch" runat="server" style="width: 15%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=4')">Search</td>
                                <td id="Td5" runat="server" style="width: 17%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=5')">Statistics</td>
                                <td id="Td6" runat="server" style="width: 14%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=6')">Rate</td>
                            </tr>
                        </table>
                        <table class="Table_WithBorder" style="width: 100%; height: 500px;" border="0">
                            <tr runat="server" id="trFilter" visible="false" >
                                <td style="vertical-align: top; height: 25%;" class="Table_WithBorder">

                                    <table border="0" style="width: 100%;">
                                        <tr>
                                            <td class="td_Text" style="width: 100%;">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td id="headerScetion" class="td_Text" style="text-align: left; width: 25%; text-wrap: avoid;">Search Criteria</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top; height: 80%;" colspan="2">
                                                <div id="divFilter">
                                                    <table border="0" style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 18%;">
                                                                <asp:Label ID="lblStartDate" runat="server" CssClass="Label_Small">Start Date:</asp:Label></td>
                                                            <td style="width: 18%;">
                                                                <asp:Label ID="lblEndDate" runat="server" CssClass="Label_Small">End Date:</asp:Label></td>
                                                            <td style="width: 14%;">
                                                                <asp:Label ID="lblFundId" runat="server" CssClass="Label_Small">Fund No.:</asp:Label></td>
                                                            <td style="width: 30%;padding-left:8px;">
                                                                <asp:Label ID="lblName" runat="server" CssClass="Label_Small">Participant Name:</asp:Label></td>
                                                            <td style="width: 22%;">
                                                                <asp:Label ID="lblAmount" runat="server" CssClass="Label_Small">Amount:</asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <YRSCustomControls:CalenderTextBox ID="txtStartDate" runat="server" Width="80px" CssClass="TextBox_Normal Warn" EnableCustomValidator="true" />
                                                            </td>
                                                            
                                                            <td>
                                                                <YRSCustomControls:CalenderTextBox ID="txtEndDate" runat="server" Width="80px" CssClass="TextBox_Normal Warn" EnableCustomValidator="true" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFundId" runat="server" CssClass="TextBox_Normal" Width="100px"></asp:TextBox></td>
                                                            <td style="padding-left:8px;">
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="TextBox_Normal" Width="250px" onkeypress="ValidateAlpha()"></asp:TextBox></td>
                                                            <td>
                                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="TextBox_Normal" Width="125px"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="6">
                                                                <table style="width: 100%;" id="tblFilerStatus">
                                                                    <tr>
                                                                        <td colspan="9" align="left" class="Label_Small">Status:</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="Label_Small">
                                                                            <asp:CheckBox ID="chkAll" onclick="SelectAllStatus()" runat="server" CssClass="CheckBox_Normal" colspan="2" />All</td>
                                                                        <td class="Label_Small">
                                                                            <asp:CheckBox ID="chkCanceled" onclick="SelectStatus()" runat="server" CssClass="CheckBox_Normal" />CANCELED</td>
                                                                        <td class="Label_Small">
                                                                            <asp:CheckBox ID="chkAccepted" onclick="SelectStatus()" runat="server" CssClass="CheckBox_Normal" />ACCEPTED</td>
                                                                        <td class="Label_Small">
                                                                            <asp:CheckBox ID="chkApproved" onclick="SelectStatus()" runat="server" CssClass="CheckBox_Normal" />APPROVED</td>
                                                                        <td class="Label_Small">
                                                                            <asp:CheckBox ID="chkExpired" onclick="SelectStatus()" runat="server" CssClass="CheckBox_Normal" />EXPIRED</td>
                                                                        <td class="Label_Small">
                                                                            <asp:CheckBox ID="chkPaid" onclick="SelectStatus()" runat="server" CssClass="CheckBox_Normal" />PAID</td>
                                                                        <td class="Label_Small">
                                                                            <asp:CheckBox ID="chkPending" onclick="SelectStatus()" runat="server" CssClass="CheckBox_Normal" />PENDING</td>
                                                                        <td class="Label_Small">
                                                                            <asp:CheckBox ID="chkDeclined" onclick="SelectStatus()" runat="server" CssClass="CheckBox_Normal" />DECLINED</td>
                                                                        <td class="Label_Small">
                                                                            <asp:CheckBox ID="chkWithdrawn" onclick="SelectStatus()" runat="server" CssClass="CheckBox_Normal" />WITHDRAWN</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="Label_Small" colspan="9">
                                                                            <asp:CheckBox ID="chkRejected" onclick="SelectStatus()" runat="server" CssClass="CheckBox_Normal" />REJECTED (EFT)</td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" colspan="6">
                                                                <asp:Button ID="btnFind" runat="server" OnClick="btnFind_Click" CssClass="Button_Normal" Text="Find"></asp:Button>
                                                                &nbsp;
                                                                <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" CssClass="Button_Normal" Text="Clear"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td class="Table_WithBorder" style="height: 100%;">
                                    <table style="height: 100%; width: 100%">
                                        <tr>
                                            <td class="td_Text">List Of Loans</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; height: 90%;">
                                                <div style="overflow: auto; width: 100%; height: 100%; border-top-style: none; border-right-style: none; border-left-style: none; position: static; border-bottom-style: none">
                                                    <div>
                                                         <asp:GridView ID="gvLoanPendingApproval" AllowSorting="true" runat="server" CssClass="DataGrid_Grid"
                                                            Width="100%" AutoGenerateColumns="false" EmptyDataText="No records found." ShowHeaderWhenEmpty="true">
                                                            <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle"></HeaderStyle><%--VC | 2018.11.26 | YRS-AT-4017 |Added class 'fixedHeader' to fix grid header while scrolling the grid--%>
                                                            <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
                                                            <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
                                                            <SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Fund No." SortExpression="FundId" HeaderStyle-Width="7%" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="server" ID="lnkFundIdNo" Text='<%# Bind("FundId")%>' CommandName="fundid" CommandArgument='<%# Eval("LoanOriginationId")%>'></asp:LinkButton><%--VC | 2018.09.05 | YRS-AT-4018 - Passing LoanOriginationId as Command argument --%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="DateTime" SortExpression="DateTime" HeaderText="Date/Time" ReadOnly="True" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" ItemStyle-Width="14%" /> <%--VC | 2018.11.14 | YRS-AT-4017 | Added time format with date format--%>
                                                                <asp:BoundField DataField="ParticipantName" SortExpression="ParticipantName" HeaderText="Participant Name" ReadOnly="True" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="28%" />
                                                                <asp:BoundField DataField="LoanAmount" SortExpression="LoanAmount" HeaderText="Loan Amount" ReadOnly="True" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="12%" />
                                                                <asp:BoundField DataField="LoanStatus" SortExpression="LoanStatus" HeaderText="Status" ReadOnly="True" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%" />
                                                                <asp:BoundField DataField="PaymentMethod" SortExpression="PaymentMethod" HeaderText="Payment Method" ReadOnly="True" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="16%" />
                                                                <asp:BoundField DataField="Date" SortExpression="Date" HeaderText="Date" ReadOnly="True" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="ClassHide" ItemStyle-CssClass="ClassHide"/>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 100%; height: 10%;">
                                                <label class="Label_Small">No of record(s) : </label>
                                                <asp:Label id="lblRecords" runat="server" CssClass="Label_Small"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 100%;" class="td_Text">
                                    <asp:Button ID="btnPrintReport" runat="server" CssClass="Button_Normal" Text="Print List" Width="73px"
                                                                CausesValidation="false"></asp:Button>
                                     &nbsp;
                                    <asp:Button ID="btnClose" OnClick="btnClose_Click" runat="server" CssClass="Button_Normal" Text="Close" Width="53px"
                                                                CausesValidation="false"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
</asp:Content>
