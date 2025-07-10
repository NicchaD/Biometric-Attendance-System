<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LACRate.aspx.vb" MasterPageFile="~/MasterPages/YRSMain.Master" Inherits="YMCAUI.LACRate" %>

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

       $(document).ready(function () {
           BindEvents();
       });

       function BindEvents() {
           $('#ConfirmDialog').dialog({
               autoOpen: false,
               draggable: true,
               close: false,
               modal: true,
               width: 450, maxHeight: 420,
               height: 260,
               title: "Prime Rate",
               open: function (type, data) {
                   $(this).parent().appendTo("form");
                   $('a.ui-dialog-titlebar-close').remove();
               }
           });
       }

       function ShowDialog() {
           $('#ConfirmDialog').dialog("open");
       }

       function closeDialog(id) {
           $('#' + id).dialog('close');
       }
       $('#txtNewPrimeRate').focus(function () {
           $(this).text('');
       });
       function FormatAmtControl(ctl) {
           var vMask;
           var vDecimalAfterPeriod;
           var ctlVal;
           var iPeriodPos;
           var sTemp;
           var iMaxLen
           var ctlVal;
           var tempVal;
           ctlVal = ctl.value;
           vDecimalAfterPeriod = 2
           iMaxLen = ctl.maxLength;

           if (isNaN(ctlVal)) {
           }
           else {
               ctlVal = ctl.value;
               iPeriodPos = ctlVal.indexOf(".");
               if (iPeriodPos < 0) {
                   if (ctl.value.length > (iMaxLen - 3)) {
                       sTemp = ctl.value
                       tempVal = sTemp.substr(0, (iMaxLen - 3)) + ".00";
                   }
                   else {
                       if (ctlVal == "")
                           ctlVal = "0";
                       tempVal = ctlVal + ".00"
                   }
               }
               else {
                   if ((ctlVal.length - iPeriodPos - 1) == 1)
                       tempVal = ctlVal + "0"
                   if ((ctlVal.length - iPeriodPos - 1) == 0)
                       tempVal = ctlVal + "00"
                   if ((ctlVal.length - iPeriodPos - 1) == 2)
                       tempVal = ctlVal;
                   if ((ctlVal.length - iPeriodPos - 1) > 2) {
                       tempVal = ctlVal.substring(0, iPeriodPos + 3);
                   }

               }
               ctl.value = tempVal;
           }
       }

       /*
       This function is responsible for filtering the keys pressed and the maintain the amount format of the 
       value in the Text box
       */
       function HandleAmountFiltering(ctl) {
           var iKeyCode, objInput;
           var iMaxLen
           var reValidChars = /[0-9.-]/;
           var strKey;
           var sValue;
           var event = window.event || arguments.callee.caller.arguments[0];
           iMaxLen = ctl.maxLength;
           sValue = ctl.value;
           detectBrowser();

           if (isIE) {
               iKeyCode = event.keyCode;
               objInput = event.srcElement;
           } else {
               iKeyCode = event.which;
               objInput = event.target;
           }

           strKey = String.fromCharCode(iKeyCode);

           if (reValidChars.test(strKey)) {
               if (iKeyCode == 46) {
                   if (objInput.value.indexOf('.') != -1)
                       if (isIE)
                           event.keyCode = 0;
                       else {
                           if (event.which != 0 && event.which != 8)
                               return false;
                       }
               }
               else {
                   if (objInput.value.indexOf('.') == -1) {

                       if (objInput.value.length >= (iMaxLen - 3)) {
                           if (isIE)
                               event.keyCode = 0;
                           else {
                               if (event.which != 0 && event.which != 8)
                                   return false;
                           }

                       }
                   }
                   if ((objInput.value.length == (iMaxLen - 3)) && (objInput.value.indexOf('.') == -1)) {
                       objInput.value = objInput.value + '.';

                   }


               }

           }
           else {
               if (isIE)
                   event.keyCode = 0;
               else {
                   if (event.which != 0 && event.which != 8)
                       return false;
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
                        <td id="tdLoansApproval" runat="server" style="width: 20%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=1')">Loans Pending Approval</td>
                        <td id="tdLoanProcessing" runat="server" style="width: 17%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=2')">Loan Processing</td>
                        <td id="tdExceptions" runat="server" style="width: 17%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=3')">Exceptions/On hold</td>
                        <td id="tdSearch" runat="server" style="width: 15%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=4')">Search</td>
                        <td id="tdStatistics" runat="server" style="width: 17%;" class="tabNotSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=5')">Statistics</td>
                        <td id="tdRate" runat="server" style="width: 14%;" class="tabSelected" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=6')">Rate</td>
                    </tr>
                </table>
                <table class="Table_WithBorder" style="width:100%;height:500px;" border="0">
            <%-- Rate Tab --%>
                    <tr>
                        <td style="vertical-align:top;height:25%;" class="Table_WithBorder">
                    
                            <table border="0" style="width:100%;">
                                <tr>
                                    <td class="td_Text" style="width:100%;">
                                        <table style="width:100%;">
                                            <tr>
                                                <td id="headerScetion" class="td_Text" style="text-align:left;width:25%;text-wrap:avoid;">Prime Rate</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align:top;height:80%;" colspan="2">
                                        <div id="divFilter">
                                            <table border="0" style="width:100%;">
                                                <caption>
                                                    <br />
                                                    <tr>
                                                        <td style="width:30%;">
                                                            <asp:Label ID="lblCurrentPrimeRate" runat="server" CssClass="Label_Small">Current Prime Rate </asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblRate" runat="server" Text="Label" CssClass="Label_Small"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <asp:Label ID="lblNewPrimeRate" runat="server" CssClass="Label_Small" Width="150px">New Prime Rate (in %)</asp:Label>
                                                            <asp:TextBox ID="txtNewPrimeRate" runat="server" CssClass="TextBox_Normal"  Width="80px" Style="text-align:right;" Text="0.00"  onpaste="return false"  onkeypress="Javascript:return HandleAmountFiltering(this);" onchange="javascript:FormatAmtControl(this);"></asp:TextBox>&nbsp;&nbsp;
                                                            <asp:Button ID="btnEditRate" runat="server" CssClass="Button_Normal" Text="Edit" Width="50px" ></asp:Button>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <asp:Button ID="btnSaveRate" runat="server" CssClass="Button_Normal" Text="Save" Width="50px"></asp:Button>
                                                            &nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnCancelRate" runat="server" CssClass="Button_Normal" Text="Cancel" Width="50px"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </caption>
                                            </table>
                                        </div>
                                
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Table_WithBorder" style="height:75%;">
                            <table style="height:100%;width:100%">
                                <tr>
                                    <td class="td_Text" style="width:100%;">
                                        <table style="width:100%;">
                                            <tr>
                                                <td id="Td1" class="td_Text" style="text-align:left;width:25%;text-wrap:avoid;">Prime Rate History</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:50%;height:90%;">
                                        <div style="overflow: auto; width: 100%;height:100%; border-top-style: none; border-right-style: none;border-left-style: none; position: static; border-bottom-style: none">
                                            <asp:GridView ID="gvLoan" AllowSorting="true" runat="server" CssClass="DataGrid_Grid" Width="40%" AutoGenerateColumns="false" EmptyDataText="No records found.">
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle" /><%--VC | 2018.11.26 | YRS-AT-4017 |Added class 'fixedHeader' to fix grid header while scrolling the grid--%>
                                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                                <SortedAscendingHeaderStyle  CssClass="sortasc" />
                                                <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                                <Columns>
                                                    <asp:BoundField DataField="dtmCreated" HeaderText="Date/Time" ReadOnly="True" ItemStyle-HorizontalAlign ="Left" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" ItemStyle-Width="14%"/>
                                                    <asp:BoundField DataField="rate" HeaderText="Value" ReadOnly="True" ItemStyle-HorizontalAlign ="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="14%"/>
                                                    <asp:BoundField DataField="chvCreator" HeaderText="User" ReadOnly="True" ItemStyle-HorizontalAlign ="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="14%"/>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="text-align: right; width: 100%;height:10%;">
                                        <label class="Label_Small">No of record(s) : </label>
                                        <asp:Label ID="lblcount" runat="server" Text="Label" CssClass="Label_Small"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right;width:100%;" class="td_Text"><asp:Button ID="btnClose" runat="server" Text="Close" class="Button_Normal"/></td>
                    </tr>
                </table>
                <div id="ConfirmDialog" title="Dialog Message" style="display: none;">
                    <div>
                        <table width="100%" height="250px" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;">
                            <tr>
                                <td rowspan="2" style="width: 10%;">
                                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="imgConfirmDialogInfo" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divConfirmDialogMessage">
                                        Are you sure, you want to save?
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
                                    <asp:Button ID="btnConfirmDialogYes" runat="server"  Text="Yes" cssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" OnClientClick="closeDialog('ConfirmDialog');"/>&nbsp;
                                    <input type="button" id="btnConfirmDialogNo" value="No" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        onclick="closeDialog('ConfirmDialog');" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSaveRate" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnConfirmDialogYes" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

