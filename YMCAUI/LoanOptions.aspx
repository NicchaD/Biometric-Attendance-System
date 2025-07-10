<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoanOptions.aspx.vb" Inherits="YMCAUI.LoanOptions" EnableEventValidation="false" MasterPageFile="~/MasterPages/YRSMain.Master" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register Assembly="YMCAObjects" Namespace="YMCAObjects" TagPrefix="cc1" %>
<asp:Content ID="cntLoanHead" ContentPlaceHolderID="head" runat="server">
    <script language="JavaScript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    BindEvents();
                }
            }
            BindEvents();
        });

        function BindEvents() {
            $('#divConfirmDialog').dialog({
                autoOpen: false,
                resizable: false,
                draggable: true,
                closeOnEscape: false,
                close: false,
                width: 400, height: 260,
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
<%--START: SR | 2019.02.06 | YRS-AT-2920 | Dialogbox to handle warning message at unfunded Loan Reamortize--%>
            $('#divUnfundedAmountWarningDialog').dialog({
                autoOpen: false,
                resizable: false,
                draggable: true,
                closeOnEscape: false,
                close: false,
                width: 400, height: 260,
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
<%--END: SR | 2019.02.06 | YRS-AT-2920 | Dialogbox to handle warning message at unfunded Loan Reamortize--%>
        }

        function showDialog(text, title) {
            <%--START: PK | 01.15.2019 | YRS-AT-2573 | change of text to html--%>
            $('#' + '<%=lblMessage.ClientID%>').html(text);
            <%--$('#' + '<%=lblMessage.ClientID%>').text(text);%>
            <%--END: PK | 01.15.2019 | YRS-AT-2573 | change of text to html--%>
            $('#divConfirmDialog').dialog({ title: title });
            $('#divConfirmDialog').dialog("open");
        }

        function closeDialog() {
            $('#divConfirmDialog').dialog('close');
        }

        <%--START: SR | 01.30.2019 | YRS-AT-2920 | Creating methods to display/close unfunded amount message.--%>
        function ShowUnfundedAmountDialog(title) {            
            var UnfundedLoanWarningMessage = '<%=UnfundedPaymentWarningMessage%>';
            $('#' + '<%=lblUnfundedAmountWarning.ClientID%>').html(UnfundedLoanWarningMessage);                        
            $('#divUnfundedAmountWarningDialog').dialog({ title: title });
            $('#divUnfundedAmountWarningDialog').dialog("open");
        }

        function IsUnfundedLoanExist() {
            var UnfundedLoanWarning = '<%=Me.IsUnfundedPaymentExist%>';          
             if (UnfundedLoanWarning == 'False') {
               
                return true;
            }
            else {
                //TODO
                
                ShowUnfundedAmountDialog('Loan Re-Amortize');
                return false;
            }
        }        

        function closeUnfundedAmountDialog()
        {
            $('#divUnfundedAmountWarningDialog').dialog('close');
        }
        <%--END: SR | 01.30.2019 |YRS-AT-2920 | Creating methods to display/close unfunded amount message.--%>


    </script>
</asp:Content>

<asp:Content ID="cntLoan" ContentPlaceHolderID="MainContentPlaceHolder" runat="server" ClientIDMode="Predictable">
    <asp:ScriptManagerProxy ID="dbScriptManagerProxy" runat="server">
    </asp:ScriptManagerProxy>
    <div class="Div_Center">
        <table width="100%">
            <tr>
                <td align="left" width="50%" height="18">
                    <asp:Label ID="LabelRequestDate" runat="server" CssClass="Label_Small"></asp:Label></td>
                <td align="right" width="50%" height="18">
                    <asp:Label ID="LabelDetails" CssClass="Label_Small" runat="server"></asp:Label></td>
            </tr>
        </table>
    </div>
    <div class="Div_Center">
        <table width="100%" border="0">
            <tbody>
                <tr style="vertical-align: top;">
                    <td align="left" style="Width: 15%">
                        <iewc:TabStrip ID="LoanOptionsTabStrip" runat="server" AutoPostBack="True" Orientation="Vertical"
                            TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;height:40px; font-size:8pt;color:#ffffff;text-align:left;border:solid 1px White;border-bottom:none"
                            TabHoverStyle="background-color:#93BEEE;color:#4172A9;text-align:left;height:40px;" TabSelectedStyle="background-color:#93BEEE;color:#000000;text-align:left;height:40px;"
                            Width="100%">
                            <iewc:Tab Text="Suspend Loan"></iewc:Tab>
                            <%-- START: MMR | 2018.12.26 | YRS-AT-4130 | Tab name changed from "Unsuspend /Reamortize Loan" to "Unsuspend Loan" --%>
                            <%--<iewc:Tab Text="Unsuspend /<br/>Reamortize Loan"></iewc:Tab>--%>
                            <iewc:Tab Text="Unsuspend Loan"></iewc:Tab>
                            <%-- END: MMR | 2018.12.26 | YRS-AT-4130 | Tab name changed from "Unsuspend /Reamortize Loan" to "Unsuspend Loan" --%>
                            <iewc:Tab Text="Terminate/<br/>Reamortize Loan"></iewc:Tab>
                            <iewc:Tab Text="PayOff Loan"></iewc:Tab>
                            <iewc:Tab Text="Default Loan"></iewc:Tab>
                            <iewc:Tab Text="Offset Loan"></iewc:Tab>
                        </iewc:TabStrip>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <iewc:MultiPage ID="LoanOptionsMultiPage" runat="server" Height="430px">
                                    <iewc:PageView Orientation="Horizontal">
                                        <table width="100%" class="Table_WithBorder" height="300px">
                                            <tr>
                                                <td align="left" class="td_Text" colspan="2">&nbsp;Suspend Loan
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="LabelLoanSuspensionStatus" CssClass="Label_Small" runat="server" ForeColor="Red" Width="600"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="350" align="left">
                                                    <%--Start | SC | 2020.04.10 | YRS-AT-4852 | Modified text for document received--%>
                                                    <%--<asp:Label ID="Label4" CssClass="Label_Small" runat="server">Has the military leave documentation been received?</asp:Label></td>--%>
                                                    <asp:Label ID="LabelLoanSuspension" CssClass="Label_Small" runat="server">Has the relevant documentation been received?</asp:Label></td>
                                                    <%--End | SC | 2020.04.10 | YRS-AT-4852 | Modified text for document received--%>
                                                <td align="left">
                                                    <asp:RadioButton CssClass="RadioButton_Normal" ID="RadioButtonLoanSuspensionYes" runat="server" GroupName="GrpLoanSuspension"
                                                        Text="Yes"></asp:RadioButton>
                                                    <asp:RadioButton CssClass="RadioButton_Normal" ID="RadioButtonLoanSuspensionNo" runat="server" GroupName="GrpLoanSuspension"
                                                        Text="No" Checked="True"></asp:RadioButton></td>
                                            </tr>
                                            <%--Start | SC | 2020.04.10 | YRS-AT-4852 | Added dropdown for selecting reason for loan suspend--%>
                                            <tr>
                                                <td align="left">
                                                <asp:Label ID="LabelReasonForLoanSuspension" runat="server" CssClass="Label_Small">Reason for Loan Suspension</asp:Label></td>
                                                <td align="left">
                                                    <asp:dropdownlist id="DropDownReasonsForLoanSuspension" runat="server" cssclass="DropDown_Normal Warn" AutoPostBack="false"/>                                                   
                                            </tr>
                                            <%--Start | SC | 2020.04.10 | YRS-AT-4852 | Added dropdown for selecting reason for loan suspend--%>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="LabelSuspendDate" runat="server" CssClass="Label_Small">Suspend Date</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <uc1:DateUserControl ID="TextBoxSuspendDate" runat="server"></uc1:DateUserControl>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td align="right" valign="bottom">
                                                    <asp:Button ID="ButtonSuspendLoan" runat="server" CssClass="Button_Normal" Text="Suspend Loan"
                                                        CausesValidation="true" Width="100"></asp:Button>
                                                    <asp:Button ID="ButtonOkSuspendLoan" runat="server" CssClass="Button_Normal"
                                                        Width="80" Text="Close" CausesValidation="false"></asp:Button></td>
                                            </tr>
                                        </table>
                                    </iewc:PageView>
                                    <iewc:PageView>
                                        <table width="100%" class="Table_WithBorder" height="430px">
                                            <tr>
                                                <td align="left" class="td_Text" colspan="2">&nbsp;<%-- Unsuspend / Re-amortize Loan --%> Unsuspend Loan <%-- MMR | 2018.12.26 | YRS-AT-4130 | Title name changed from "Unsuspend /Re-amortize Loan" to "Unsuspend Loan" --%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="LabelLoanUnSuspensionStatus" CssClass="Label_Small" runat="server" ForeColor="Red" Width="600"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <%--Start | SC | 2020.04.11 | YRS-AT-4852 | Modified text as per COVID Care act--%>
                                                <td align="left">
                                                    <asp:Label ID="LabelUnSuspendLoan" runat="server" CssClass="Label_Small">Discharge Date</asp:Label>
                                                   <asp:Label ID="LabelUnSuspendLoanDueToCovid" runat="server" CssClass="Label_Small" Visible="false">Unsuspend Date</asp:Label>
                                                    </td>
                                                <td align="left">
                                                    <uc1:DateUserControl ID="TextBoxUnSuspendDate" runat="server"></uc1:DateUserControl>                                                    
                                                </td>
                                                <%--End | SC | 2020.04.11 | YRS-AT-4852 | Modified text as per COVID Care act--%>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:Button ID="ButtonShowDetails" runat="server" CssClass="Button_Normal" Text="Show Details"
                                                        CausesValidation="false" Width="120"></asp:Button></td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="LabelRemainingAmount" runat="server" CssClass="Label_Small">Re-amortization Amount</asp:Label></td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextBoxRemainingAmount" runat="server" CssClass="TextBox_Normal_Right" ReadOnly="True"
                                                        Width="90"></asp:TextBox>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="LabelRemainingMonths" runat="server" CssClass="Label_Small">Re-amortization Months</asp:Label></td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextBoxRemainingMonths" runat="server" CssClass="TextBox_Normal_Right" ReadOnly="True"
                                                        Width="90"></asp:TextBox>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td align="right" valign="bottom">
                                                    <%-- START: MMR | 2018.12.26 | YRS-AT-4130 | Changed button text from "Unsuspend/Re-amortize Loan" to "Unsuspend Loan" and width from "200" to "150" --%>
                                                    <asp:Button ID="ButtonUnSuspendLoan" runat="server" CssClass="Button_Normal" Text="Unsuspend Loan" 
                                                        CausesValidation="true" Width="150"></asp:Button>
                                                    <%-- END: MMR | 2018.12.26 | YRS-AT-4130 | Changed button text from "Unsuspend/Re-amortize Loan" to "Unsuspend Loan" and width from "200" to "150" --%>
                                                    <asp:Button ID="ButtonOkUnsuspendLoan" runat="server" CssClass="Button_Normal"
                                                        Width="80" Text="Close" CausesValidation="false"></asp:Button></td>
                                            </tr>
                                        </table>
                                    </iewc:PageView>
                                    <iewc:PageView>
                                        <table width="100%" class="Table_WithBorder" height="430px">
                                            <tr>
                                                <td align="left" class="td_Text" colspan="2">&nbsp;Terminate / Re-amortize Loan
                                                </td>
                                            </tr>
                                            <tr style="height: 25px;">
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr style="height: 25px;">
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="LabelCloseLoan" CssClass="Label_Small" runat="server" ForeColor="Red" Width="600"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="height: 25px;">
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <%--Start -- Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Label and Dropdown control to display YMCA --%>
                                            <tr>
                                                <td align="left">    
                                                <asp:Label ID="LabelActiveYMCA" runat="server" CssClass="Label_Small">YMCA</asp:Label></td>
                                                <td align="left">
                                                    <asp:dropdownlist id="DropDownActiveYMCA" runat="server" width="300" cssclass="DropDown_Normal Warn" OnSelectedIndexChanged="DropDownActiveYMCA_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true">  <%--PK | 01.15.2019 | YRS-AT-2573 | Attributes added to postback when dropdowActiveYmca is selected--%>                                                     
                                                    </asp:dropdownlist>                                                   
                                            </tr>
                                            <%--End -- Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Label and Dropdown control to display YMCA --%>
                                            <tr style="height: 25px;">
                                                <td align="left">
                                                    <asp:Label ID="LabelRemainingReamorizeAmount" runat="server" CssClass="Label_Small">Re-amortization Amount</asp:Label></td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextboxRemainingReamorizeAmount" runat="server" CssClass="TextBox_Normal_Right"
                                                        Width="90" ReadOnly="True"></asp:TextBox>
                                            </tr>
                                            <tr style="height: 25px;">
                                                <td align="left">
                                                    <asp:Label ID="LabelRemainingReamorizeMonths" runat="server" CssClass="Label_Small">Re-amortization Months</asp:Label></td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextboxRemainingReamorizeMonths" runat="server" CssClass="TextBox_Normal_Right"
                                                        Width="90" ReadOnly="True"></asp:TextBox>
                                            </tr>
                                             <%--START: PK | 01.15.2019 | YRS-AT-2573 | code added to display dropdown list for payment date and Label--%>
                                            <tr>
                                                <td align="left">    
                                                <asp:Label ID="lblFirstPaymentDate" runat="server" CssClass="Label_Small">First Payment Due Dates (New Loan)</asp:Label></td>
                                                <td align="left">
                                                    <asp:dropdownlist id="DropdownFirstPaymentDate" runat="server" width="100" cssclass="DropDown_Normal Warn" AutoPostBack="true"> <%-- MMR | 2019.01.24 | Added postback property to fire selected index change event --%>
                                                    </asp:dropdownlist>                                                   
                                            </tr>
                                            <tr>
                                                <td  class="auto-style1" colspan="2">
                                                    <asp:Label ID="lblPayDates" runat="server" CssClass ="Label_Small" style="width:500px; font-size:xx-small;">
                                                    </asp:Label>
                                                </td>
                                            </tr>
                                             <%--END: PK | 01.15.2019 | YRS-AT-2573 | code added to display dropdown list for payment date and Label--%>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td align="right" valign="bottom">
                                                    <asp:Button ID="ButtonCloseLoan" runat="server" CssClass="Button_Normal" Text="Terminate /Re-amortize Loan"
                                                        CausesValidation="false" Width="200" OnClientClick="javascript:return IsUnfundedLoanExist();"></asp:Button>
                                                   <%-- SR | 2019.02.06 |YRS-AT-2920 | Added OnclientClick to call javascript function.--%>
                                                    <asp:Button ID="ButtonOkClose" runat="server" CssClass="Button_Normal"
                                                        Width="80" Text="Close" CausesValidation="False"></asp:Button></td>
                                            </tr>
                                        </table>
                                    </iewc:PageView>
                                    <iewc:PageView>
                                        <table width="100%" class="Table_WithBorder" height="430px">
                                            <tr>
                                                <td align="left" class="td_Text" colspan="2">&nbsp;PayOff Loan
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="LabelPayOffLoan" runat="server" ForeColor="Red" CssClass="Label_Small" Width="600"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="bottom" colspan="2">
                                                    <asp:Button ID="ButtonPayOffLoan" runat="server" CssClass="Button_Normal" Text="PayOff Loan"
                                                        CausesValidation="false" Width="100"></asp:Button>
                                                    <asp:Button ID="ButtonOkPayOffLoan" runat="server" CssClass="Button_Normal"
                                                        Width="80" Text="Close" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </iewc:PageView>
                                    <iewc:PageView>
                                        <table width="100%" class="Table_WithBorder" height="430px">
                                            <tr style="vertical-align: top;">
                                                <td align="left" class="td_Text" colspan="3">&nbsp;Default Loan
                                                </td>
                                            </tr>
                                            <tr style="height: 25px">
                                                <td colspan="3">&nbsp;
                                                </td>
                                            </tr>
                                            <tr style="height: 25px">
                                                <td align="left" colspan="3">
                                                    <asp:Label ID="LabelDefaultLoan" runat="server" ForeColor="Red" Width="100%" CssClass="Label_Small"></asp:Label></td>
                                            </tr>
                                            <tr style="height: 25px">
                                                <td colspan="3">&nbsp;
                                                </td>
                                            </tr>

                                            <tr >
                                                <td align="left" style="width:90px;">
                                                    <asp:Label ID="LabelDefaultDate" runat="server" CssClass="Label_Small">Default Date</asp:Label></td>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="TextBoxDefaultDate" runat="server" CssClass="TextBox_Normal" ReadOnly="True"
                                                        Width="90"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr >
                                                <td align="left">
                                                    <asp:Label ID="LabelDefaultAmount" runat="server" CssClass="Label_Small">Default Amount</asp:Label></td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextboxDefaultAmount" runat="server" CssClass="TextBox_Normal_Right" ReadOnly="True"
                                                        Width="90"></asp:TextBox>
                                                    
                                                </td>
                                                <td style="text-align:left;">
                                                    <label class="Label_Small">(Unpaid Principal + Cure Period Interest)</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td  class="Label_Small">Default Unpaid Principal
                                                </td>
                                                <td style="text-align: right;width:86px">
                                                    <asp:Label ID="lblUnpaidPrincipal" runat="server" CssClass="NormalMessageText"></asp:Label>
                                                </td>
                                                <td >&nbsp;</td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="Label_Small">Default Cure Period Interest</label>
                                                </td>
                                                <td style="text-align: right;width:86px">
                                                    <asp:Label ID="lblCRINT" runat="server" CssClass="NormalMessageText"></asp:Label>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>

                                             <tr id="trRTOffset" runat="server">
                                                <td align="left">
                                                    <asp:Label ID="Label1" runat="server" CssClass="Label_Small">Offset Amount</asp:Label></td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextboxRTOffset" runat="server" CssClass="TextBox_Normal_Right" ReadOnly="True"
                                                        Width="90"></asp:TextBox>
                                                    
                                                </td>
                                                <td style="text-align:left;">
                                                    <label class="Label_Small">(Unpaid Principal + Cure Period Interest)</label>
                                                </td>
                                            </tr>
                                            <tr id="trRTOffsetPrincipal" runat="server">
                                                <td  class="Label_Small">Offset Unpaid Principal
                                                </td>
                                                <td style="text-align: right;width:86px">
                                                    <asp:Label ID="LabelRTOffsetPrincipal" runat="server" CssClass="NormalMessageText"></asp:Label>
                                                </td>
                                                <td >&nbsp;</td>

                                            </tr>
                                            <tr id="trRTOffsetInterest" runat="server">
                                                <td>
                                                    <label class="Label_Small">Offset Cure Period Interest</label>
                                                </td>
                                                <td style="text-align: right;width:86px">
                                                    <asp:Label ID="LabelRTOffsetInterest" runat="server" CssClass="NormalMessageText"></asp:Label>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>

                                                <td align="right" valign="bottom" colspan="3">
                                                    <asp:Button ID="ButtonDefaultLoan" runat="server" CssClass="Button_Normal" Text="Default Loan"
                                                        CausesValidation="true" Width="100"></asp:Button>
                                                    <asp:Button ID="ButtonOkDefault" runat="server" CssClass="Button_Normal"
                                                        Width="80" Text="Close" CausesValidation="False"></asp:Button></td>
                                            </tr>
                                        </table>
                                    </iewc:PageView>
                                    <iewc:PageView>
                                        <table width="100%" class="Table_WithBorder" height="430px">
                                            <tr style="vertical-align: top;">
                                                <td align="left" class="td_Text" colspan="3">&nbsp;Offset Loan
                                                </td>
                                            </tr>
                                            <tr style="height: 25px">
                                                <td colspan="3" style="text-align:right" >
                                                    <label class="Label_Small">Age:</label> <asp:Label ID="lblPersonAge" runat="server" CssClass="NormalMessageText"></asp:Label> &nbsp;&nbsp;
                                                    <label class="Label_Small">Employment Status:</label> <asp:Label ID="lblEmpStatus" runat="server" CssClass="NormalMessageText"></asp:Label> 
                                                </td>
                                            </tr>
                                            <tr style="height: 25px">
                                                <td align="left" colspan="3">
                                                    <asp:Label ID="LabelOffsetLoan" runat="server" ForeColor="Red" Width="100%" CssClass="Label_Small"></asp:Label></td>
                                            </tr>
                                            <tr style="height: 25px">
                                                <td colspan="3">&nbsp;
                                                </td>
                                            </tr>

                                            <tr style="height: 25px">
                                                <td align="left">
                                                    <asp:Label ID="Label2" runat="server" CssClass="Label_Small">Offset Date</asp:Label></td>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="TextBoxOffsetDate" runat="server" CssClass="TextBox_Normal" ReadOnly="True"
                                                        Width="90"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label3" runat="server" CssClass="Label_Small">Offset Amount</asp:Label></td>
                                                <td align="left" Width="90">
                                                    <asp:TextBox ID="TextboxOffsetAmount" runat="server" CssClass="TextBox_Normal_Right" ReadOnly="True"
                                                        Width="90"></asp:TextBox>
                                                    </label>                 
                                                </td>
                                                <td style="text-align:left;">
                                                    <asp:Label ID="lblOffsetAmountdetail" runat="server" CssClass="Label_Small"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPrincipal" runat="server" CssClass="Label_Small"></asp:Label>
                                                    
                                                </td>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="lblOffsetUnpaidPrincipal" runat="server" CssClass="NormalMessageText"></asp:Label>
                                                </td>
                                                <td style="width: 475px;">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label CssClass="Label_Small" runat="server" ID="lblOffsetIntdetail"></asp:Label>
                                                    
                                                </td>
                                                <td style="text-align: right;">
                                                    <asp:Label ID="lblOffsetInt" runat="server" CssClass="NormalMessageText"></asp:Label>
                                                    
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td colspan="2" align="right" valign="bottom">
                                                    <asp:Button ID="ButtonOffsetLoan" runat="server" CssClass="Button_Normal" Text="Offset Loan"
                                                        CausesValidation="true" Width="100"></asp:Button>
                                                    <asp:Button ID="ButtonOkOffset" runat="server" CssClass="Button_Normal"
                                                        Width="80" Text="Close" CausesValidation="False"></asp:Button></td>
                                            </tr>
                                        </table>
                                    </iewc:PageView>
                                </iewc:MultiPage>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="divConfirmDialog" style="overflow: visible;">
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
                                    OnClientClick="Javascript: closeDialog();" />&nbsp;
                                    <asp:Button runat="server" ID="btnNo" Text="No" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        OnClientClick="Javascript: closeDialog();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%-- START :SR | YRS-AT-2920 | To handle Unfunded amount warning message --%>
    
    <div id="divUnfundedAmountWarningDialog" style="overflow: visible;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div>
                    <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                        <tr>
                            <td>
                                <asp:Label ID="lblUnfundedAmountWarning" CssClass="Label_Small" runat="server"></asp:Label>
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
                                <asp:Button runat="server" ID="btnUnfundedAmountYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                    OnClientClick="Javascript: closeUnfundedAmountDialog();" />&nbsp;
                                    <asp:Button runat="server" ID="btnUnfundedAmountNo" Text="No" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        OnClientClick="Javascript: closeUnfundedAmountDialog();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%-- END :SR |YRS-AT-2920 | To handle Unfunded amount warning message.--%>
</asp:Content>
