<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master"
    CodeBehind="RMDSpecialQDROLetters.aspx.vb" Inherits="YMCAUI.RMDSpecialQDROLetters" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register Src="~/UserControls/BatchProcessProgressControl.ascx" TagPrefix="uc1" TagName="BatchProcessProgressControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="JS/jquery-1.5.1.min.js"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="JS/jquery-bPopup.js"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
    <style type="text/css">
        .SpecialQDROMsg {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: #333;
            line-height: 18px;
            background-color: #FFFFC6;
            font-weight: normal;
            background-repeat: no-repeat;
            border: 0px solid #C30;
    
            background-position: 4px 4px;
            padding-top: 5px;
            padding-right: 5px;
            padding-bottom: 5px;
            padding-left: 5px;
            overflow: visible;
            margin-bottom: 5px;
        }

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
         
        .BG_ColourFollowUpLetter {
            background-color: #F08080; 
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

        .hide {
            display: none;
        }

        .show {
            display: block;
        }

        .textAlign {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    args.set_errorHandled(true);
                }
            }
        });

        function ontabmousehover(id) {
            $('#' + id).attr('class', 'tabselected');
        }
        function ontabmousehout(id) {
            $('#' + id).attr('class', 'tabNotSelected');
        }

        function OpenPDF(url) {
            try {
                if (!CheckAccessRightsForReprint()) {
                    window.open(url, 'OpenCustomPDF', 'width=1024, height=768, resizable=yes, top=0, left=0, scrollbars=yes ,directories=no ,toolbar=no ,menubar=no, location=no');
                }
                return false;
            }
            catch (err) {
                alert(err.message);
            }
        }

        function CallLetter() {
            window.open('CallReport.aspx', 'ReportPopUp1', 'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
        }

        /* Check Box Selection code */
        function chkall(ival) {
            var f = document.getElementById("gvLetters");
            for (var i = 1; i < f.getElementsByTagName("input").length; i++) {
                if (f.getElementsByTagName("input").item(i).type == "checkbox") {
                    if (f.getElementsByTagName("input").item(i).disabled == false) {
                        f.getElementsByTagName("input").item(i).checked = ival;
                    }
                }
            }
        }

        function Unckeck(obj) {
            var f = document.getElementById("gvLetters");
            if (!obj.checked) {
                f.getElementsByTagName("input").item(0).checked = false;
            }
        }

        function CheckAccessRightsForReprint() {
            var readOnlyWarningMessage = '';
            var isReadOnly = false;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "RMDSpecialQDROLetters.aspx/CheckAccessRightsForReprint",
                data: "{}",
                async: false,
                dataType: "json",
                success: function (data) {
                    isReadOnly = data.d.Value;
                    if (isReadOnly && data.d.MessageList != undefined) {
                        var readOnlyWarningMessage = data.d.MessageList[0];
                        $('#<%=Me.Master.FindControl("DivMainMessage").ClientID%>').html('<DIV class="error-msg">' + readOnlyWarningMessage + '</DIV>');
                    }
                },
                error: function (result) {
                }
            });
            return isReadOnly;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="div_center">
        <asp:ScriptManagerProxy ID="RMDPRintScriptManager" runat="server">
        </asp:ScriptManagerProxy>
        <asp:UpdatePanel ID="uplGenerateRMD" runat="server">
            <ContentTemplate>
                <table class="Table_WithoutBorder" cellpadding="0" cellspacing="0" width="100%" align="center">
                    <tr>
                        <td align="left" valign="middle" style="width: auto">
                            <asp:Label ID="lblMRDMsg" runat="server" CssClass="Label_Small"></asp:Label>
                        </td>
                        <td align="right" cellspacing="0">
                            <table class="td_withoutborder" cellpadding="0" cellspacing="0" style="width: 100%; height: 25px;">
                                <tr>
                                    <td style="width: 33%;">
                                        <asp:LinkButton ID="lnkInitial" Text="Generate Initial Letter" runat="server" CssClass="tabNotSelected"
                                            onmouseover="javascript: ontabmousehover('lnkInitial');" onmouseout="javascript: ontabmousehout('lnkInitial');"></asp:LinkButton>
                                        <asp:Label ID="lbllnkInitial" CssClass="tabSelected" runat="server" Text="Generate Initial Letter"></asp:Label>
                                    </td>
                                    <td style="width: 33%;"> 
                                        <asp:LinkButton ID="lnkFollowup" CssClass="tabNotSelected" Text="Generate Follow-up Letter" runat="server"
                                            onmouseover="javascript: ontabmousehover('lnkFollowup');" onmouseout="javascript: ontabmousehout('lnkFollowup');"></asp:LinkButton>
                                        <asp:Label ID="lbllnkFollowup" CssClass="tabSelected" runat="server" Text="Generate Follow-up Letter"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table class="Table_WithoutBorder" cellpadding="0" cellspacing="0" width="100%" align="center">
                    <tr style="vertical-align: top;">
                        <td class="tabSelectedLink" style="width: 100%; height: 3px; text-align: left;">
                            <asp:Label ID="lblDescription" runat="server" Text="List of person(s) eligible for generating Special QDRO Participants initial letters."></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="Table_WithBorder" cellpadding="0" cellspacing="0" align="center">
                    <tr>
                        <td style="vertical-align: top; width: 145px; text-align: center">
                            <div style="overflow: auto; width: 145px; border-top-style: none; border-right-style: none; border-left-style: none; height: 400px; border-bottom-style: none">
                                <table class="Table_WithBorder" id="Table2" cellspacing="1" cellpadding="1" border="0" style="width: 100%">
                                    <tr class="DataGrid_AlternateStyle">
                                        <td>
                                            <b>Filter Criteria</b>
                                        </td>
                                    </tr>
                                    
                                    <tr class="DataGrid_HeaderStyle" id="trFilterAccountLocked" runat="server">
                                        <td>Account Locked
                                        </td>
                                    </tr>
                                    <tr id="trFilterAccountDDL" runat="server" >
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlAcctLocked" CssClass="LstFilter" Width="120px">
                                                <asp:ListItem Selected="True" Text="Any" Value="All"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid_HeaderStyle" id="trFilterInsufficientBal" runat="server">
                                        <td>Insufficient Balance
                                        </td>
                                    </tr>
                                    <tr id="trFilterInsufficientBalDDL" runat="server">
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlInsufficientBal" CssClass="LstFilter" Width="120px">
                                                <asp:ListItem Selected="True" Text="Any" Value="All"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid_HeaderStyle" id="trFilterFollowupGenerated" runat="server">
                                        <td>
                                            <asp:Label ID="txtFollowup" Text="Follow-up Generated" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trFilterFollowupGeneratedDDL" runat="server">
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlFollowUp" CssClass="LstFilter" Width="120px">
                                                <asp:ListItem Selected="True" Text="Any" Value="All"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                </table>
                                <table class="Table_WithBorder" id="Table3" cellspacing="1" cellpadding="1" border="0" style="width: 100%" runat="server">
                                    <tr>
                                        <td class="Label_Small">Fund No.:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFundId" CssClass="TexBox_Normal" runat="server" MaxLength="20" Width="130px" onkeypress="ValidateNumeric();"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button runat="server" ID="btnSearch" Text="View >>" CssClass="Button_Normal" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td>
                            <table class="Table_WithOutBorder" id="Table1" spellcheck="true" cellspacing="1" cellpadding="1" width="100%">
                                <tr>
                                    <td style="vertical-align: top;text-align: left;" runat="server"> 
                                        <div class="SpecialQDROMsg">
                                            <asp:Label ID="lblSpecialQDROMsg" runat="server" text="This screen shows alternate payees whose first RMD's are due in December instead of April. They were granted an account at the Fund pursuant to a QDRO after the original participant's first RMD year."></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="overflow: scroll; width: 100%; height: 400px">
                                            <asp:GridView runat="server" ID="gvLetters" CssClass="DataGrid_Grid"
                                                AllowSorting="true" AutoGenerateColumns="false" SelectedRowStyle-VerticalAlign="Top"
                                                Width="100%" EmptyDataText="No records found.">
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                                <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="2%">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="chkall(this.checked)" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" onclick="Unckeck(this)" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="2%" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="PerssID" InsertVisible="true" HeaderText="PerssID" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="FundIdNo" SortExpression="FundIdNo" HeaderText="Fund No" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"/>
                                                    <asp:BoundField DataField="PlanType" SortExpression="PlanType" HeaderText="Plan Type" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="9%"/>
                                                    <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="17%"/>
                                                    <asp:BoundField DataField="CurrentBalance" SortExpression="CurrentBalance" HeaderText="Current Balance" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%"/>
                                                    <asp:BoundField DataField="MRDAmount" SortExpression="MRDAmount" HeaderText="RMD Amount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"/>
                                                    <asp:BoundField DataField="PaidAmount" SortExpression="PaidAmount" HeaderText="Paid Amount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="8%"/>
                                                    <asp:BoundField DataField="StatusTypeDescription" SortExpression="StatusTypeDescription" HeaderText="Fund Status" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%"/>
                                                    <asp:BoundField DataField="MRDExpireDate" SortExpression="MRDExpireDate" HeaderText="Due Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="6%"/>
                                                    <asp:BoundField DataField="chrSSNo" SortExpression="SSNo" HeaderText="SSNo" ControlStyle-CssClass="hide" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="chvFirstName" SortExpression="FirstName" HeaderText="FirstName" ControlStyle-CssClass="hide" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="chvLastName" SortExpression="LastName" HeaderText="LastName" ControlStyle-CssClass="hide" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="chvMiddleName" SortExpression="MiddleName" HeaderText="MiddleName" ControlStyle-CssClass="hide" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="InitialLetterLastGenerated" SortExpression="InitialLetterLastGenerated" HeaderText="Initial Letter Last Gen." ControlStyle-CssClass="hide" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="FollowupLetterLastGenerated" SortExpression="FollowupLetterLastGenerated" HeaderText="Follow-up Letter Last Gen." ControlStyle-CssClass="hide" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                    <asp:TemplateField HeaderText="Initial Letter <br/> Last Gen." SortExpression="InitialLetterLastGenerated" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID ="lnkReprintInitialLetter" runat="server" text='<%#Bind("InitialLetterLastGenerated")%>' CommandName="specialQDInitialLetter" ToolTip="Reprint Initial Letter"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Follow-up Letter <br/> Last Gen." SortExpression="FollowupLetterLastGenerated"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="12%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID ="lnkReprintFollowupLetter" runat="server" text='<%#Bind("FollowupLetterLastGenerated")%>' CommandName="specialQDFollowupLetter" ToolTip="Reprint Follow-up Letter"></asp:LinkButton>
                                                            <asp:Label runat="server" ID="lblDash" style="display:none;">-</asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FundEventID" HeaderText="FundEventID" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr runat="server" id="trColorCodingLegends"> 
                                    <td style="text-align: left" colspan="2">
                                        <table style="text-align: left" border="0">
                                            <tr>
                                                <td style="text-align: center" class="Label_Small">  
                                                    <span class="BG_ColourIsLocked">&nbsp;&nbsp;</span> - Account Locked.
                                                </td>
                                                <td style="text-align: center" class="Label_Small">   
                                                    <span class="BG_ColourIsBlocked">&nbsp;&nbsp;</span> - Insufficient Balance in One or Both Plans.
                                                </td>
                                                <td style="text-align: center" class="Label_Small" id="tdFollowUpLegends" runat="server">
                                                    <span id="tdFollow" runat="server" class="BG_ColourFollowUpLetter">&nbsp;&nbsp;</span> - Follow-up Letter Processed.
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table class="Td_ButtonContainer" style="width: 100%">
                    <tr>
                        <td style="text-align: right; width: 720px;">
                            <asp:Button runat="server" ID="btnPrintList" Text="Print List" CssClass="Button_Normal" />
                        </td>
                        <td style="text-align: right; width: 80px;">
                            <asp:Button runat="server" ID="btnPrintLetters" Text="Print Initial Letter" CssClass="Button_Normal" />
                        </td>
                        <td style="text-align: right; width: 40px;">
                            <asp:Button runat="server" ID="btnClose" Text="Close" CssClass="Button_Normal" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <uc1:BatchProcessProgressControl runat="server" id="BatchProcessProgressControl" />
</asp:Content>
