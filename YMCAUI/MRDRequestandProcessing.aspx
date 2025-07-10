<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MRDRequestandProcessing.aspx.vb"
    Inherits="YMCAUI.MRDRequestandProcessing" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<!--#include virtual="top.html"-->
<head>
    <style type="text/css">
        .Label_Small
        {
        }
        /*START: SG: 2012.03.16: BT-1011*/
        .BG_ColourIsLocked
        {
            background-color: #90EE90; /*LightGreen*/
        }
        
        .BG_ColourIsBlocked
        {
            background-color: #FFB6C1; /*LightPink*/
        }
        
        .BG_ColourIsMultipleYearMRD
        {
            background-color: #00BFFF; /*DodgerBlue*/
        }
        /*END: SG: 2012.03.16: BT-1011*/
        
        /*
        CSS added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements
        */
        .BG_ColourIsNotEnrollAnnualMRDPayments
        {
            background-color: #FBC97A; /*Red*/
        }
        .BG_ColourInitialLetter
        {
             background-color: #F08080; /*LightSkyBlue*/
        }
        
        
    </style>
</head>
<script language="JavaScript">
    function ValidateNumeric() {
        if ((event.keyCode < 48) || (event.keyCode > 57)) {
            event.returnValue = false;
        }
    }

    function chkall(ival) {
        var f = document.getElementById("dgMRD");
        for (var i = 1; i < f.getElementsByTagName("input").length; i++) {
            if (f.getElementsByTagName("input").item(i).type == "checkbox") {
                if (f.getElementsByTagName("input").item(i).disabled == false) {
                    f.getElementsByTagName("input").item(i).checked = ival;
                    if (!document.getElementById("btnProcess").disabled) {
                        document.getElementById("btnProcess").innerText = "Disburse";
                    }
                }
            }
        }
    }


    function Unckeck(obj) {
        var f = document.getElementById("dgMRD");
        if (!obj.checked) {
            f.getElementsByTagName("input").item(0).checked = false;
        }
    }

    function CheckAllCheckBox() {
        var f = document.getElementById("dgMRD");
        for (var i = 0; i < f.getElementsByTagName("input").length; i++) {
            if (f.getElementsByTagName("input").item(i).type == "checkbox") {
                if (f.getElementsByTagName("input").item(i).disabled == false) {
                    f.getElementsByTagName("input").item(i).checked = true;
                }
            }
        }
    }

    function showdivMsg(strMsg) {
        divblock.style.display = 'block';
        document.getElementById('divblock').innerHTML = strMsg;
    }

</script>
<form id="Form1" method="post" runat="server">
<table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="700">
    <tr>
        <td class="Td_BackGroundColorMenu" align="left">
            <cc1:Menu ID="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False"
                Cursor="Pointer" CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown"
                DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup" MenuFadeDelay="2"
                mouseovercssclass="MouseOver">
                <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
            </cc1:Menu>
        </td>
    </tr>
    <tr>
        <td class="Td_HeadingFormContainer" align="left">
            RMD Batch Process
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
</table>
<div id="divblock">
</div>
<table class="Table_WithBorder" cellspacing="0" cellpadding="0" width="700">
    <tr>
        <td align="right" height="22">
            <table class="Table_WithOutBorder" id="Table1" cellspacing="1" cellpadding="1" border="0"
                width="700">
                <tr>
                    <td align="center" nowrap="true" class="style1" width="140" valign="middle">
                        <asp:label id="LblMRDDate" cssclass="Label_Small" text="RMD Process Date :" runat="server"
                            font-names="Arial" font-size="Small"></asp:label>
                    </td>
                    <td colspan="1" nowrap="true" align="left">
                        <asp:label id="lblDate" cssclass="Label_Small" text="" runat="server" font-names="Arial"
                            font-size="Small"></asp:label>
                    </td>
                    <td>
                        <asp:label id="lblNote" cssclass="Label_Small" text="Note: To Generate Initial Letter click Print Letters button."
                            runat="server" font-names="Arial" font-size="Small"></asp:label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="right">
            <table class="Table_WithOutBorder" id="Table1" cellspacing="1" cellpadding="1" width="700">
                <tr>
                    <td>
                        <div style="overflow: auto; width: 100%; border-top-style: none; border-right-style: none;
                            border-left-style: none; height: 300px; border-bottom-style: none">
                            <asp:datagrid id="dgMRD" runat="server" cssclass="DataGrid_Grid" width="671px" allowsorting="True"
                                autogeneratecolumns="False" onsortcommand="SortCommand_OnClick" selecteditemstyle-verticalalign="Top"
                                  DataKeyField="MRDUniqueID" pagesize="20" >
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top"></SelectedItemStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<Columns>
										<asp:TemplateColumn HeaderText="Select">
                                            <HeaderTemplate>	
                                        	    <asp:CheckBox id="CheckBox2" runat="server" onclick="chkall(this.checked)"></asp:CheckBox>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" onclick="Unckeck(this)"/>
                                            </ItemTemplate>
                                            <HeaderStyle Width="5%" />
                                        </asp:TemplateColumn>
										<asp:BoundColumn DataField="FundIdNo" SortExpression="FundIdNo" 
                                            HeaderText="FundId No">
											<HeaderStyle Width="10%" />
											<ItemStyle Width="50px"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PlanType" SortExpression="PlanType" 
                                            HeaderText="Plan Type">
											<HeaderStyle Width="10%" />
											<ItemStyle Width="50px"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="CurrentBalance" HeaderText="Current Balance" 
                                            SortExpression="CurrentBalance">
                                            <HeaderStyle Width="15%" />
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" />
                                        </asp:BoundColumn>
										<asp:BoundColumn DataField="MRDAmount" SortExpression="MRDAmount" 
                                            HeaderText="RMD Amount" >
											<HeaderStyle Width="12%" />
											<ItemStyle  Width="115px" Font-Bold="False" Font-Italic="False" 
                                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                                                HorizontalAlign="Right"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PaidAmount" HeaderText="Paid Amount" 
                                            SortExpression="PaidAmount" >
                                            <HeaderStyle Width="12%" />
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" />
                                        </asp:BoundColumn>
									    <asp:BoundColumn DataField="StatusTypeDescription" HeaderText="Fund Status" 
                                            SortExpression="StatusTypeDescription">
                                            <HeaderStyle Width="15%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="FundEventID" HeaderText="FundEventID" 
                                            SortExpression="FundEventID" Visible="False"></asp:BoundColumn>
										<asp:BoundColumn DataField="MRDYear" SortExpression="MRDYear" 
                                            HeaderText="Year" Visible="False">
										</asp:BoundColumn>
									    <asp:BoundColumn DataField="Process_Status" HeaderText="Process Status" 
                                            SortExpression="Process_Status" Visible="False">
                                            <HeaderStyle Width="20%" />
                                        </asp:BoundColumn>
                                        <%--Start code Added by dinesh Kanojia BT:2139 : YRS 5.0-2165:RMD enhancements  --%>
                                        <asp:BoundColumn DataField="MRDYear" HeaderText="Year" 
                                            SortExpression="MRDYear">
                                            <HeaderStyle Width="5%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="MRDExpireDate" HeaderText="Due Date" 
                                            SortExpression="MRDExpireDate">
                                            <HeaderStyle Width="15%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="PerssID" HeaderText="PerssID" visible="false" > 
                                        </asp:BoundColumn>
                                        <%--End code Added by dinesh Kanojia BT:2139 : YRS 5.0-2165:RMD enhancements  --%>
                                        <%--Start code Added by dinesh Kanojia BT:2012 : YRS 5.0-2063: Handling RMD candidates --%>
                                        <asp:BoundColumn DataField="bitAnnualMRDRequested" HeaderText="bitAnnualMRDRequested" Visible="False"> 
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Name" HeaderText="Name" Visible="False"> 
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="WithHoldingTax" HeaderText="WithHoldingTax" Visible="False"> 
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="RMDInitialLetterDate" HeaderText="RMDInitialLetterDate" Visible="False"> 
                                        </asp:BoundColumn>
                                        <%--End code Added by dinesh Kanojia BT:2012 : YRS 5.0-2063: Handling RMD candidates --%>
									    <%--<asp:BoundColumn DataField="IsLocked" HeaderText="Locked" 
                                            SortExpression="IsLocked" Visible="False"></asp:BoundColumn>
									    <asp:BoundColumn DataField="IsBlocked" HeaderText="Blocked" Visible="False">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="IsMultipleYearMRD" HeaderText="Multiple Year MRD" 
                                            Visible="False"></asp:BoundColumn>--%>
									</Columns>
								</asp:datagrid>
                            <asp:datagrid id="dgMrdProcessStatus" runat="server" cssclass="DataGrid_Grid" width="671px"
                                allowsorting="True" visible="false" autogeneratecolumns="False" onsortcommand="SortCommand_OnClick"
                                selecteditemstyle-verticalalign="Top" datakeynames="MRDUniqueID" pagesize="20">
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top"></SelectedItemStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<Columns>
										<asp:BoundColumn DataField="FundIdNo" SortExpression="FundIdNo" 
                                            HeaderText="FundId No">
											<HeaderStyle Width="10%" />
											<ItemStyle Width="50px"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PlanType" SortExpression="PlanType" 
                                            HeaderText="Plan Type">
											<HeaderStyle Width="10%" />
											<ItemStyle Width="50px"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="CurrentBalance" HeaderText="Current Balance" 
                                            SortExpression="CurrentBalance">
                                            <HeaderStyle Width="12%" />
                                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Right" />
                                        </asp:BoundColumn>
										<asp:BoundColumn DataField="MRDAmount" SortExpression="MRDAmount" 
                                            HeaderText="RMD Amount" >
											<HeaderStyle Width="12%" />
											<ItemStyle  Width="115px" Font-Bold="False" Font-Italic="False" 
                                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                                                HorizontalAlign="Right"></ItemStyle>
										</asp:BoundColumn>
									    <asp:BoundColumn DataField="StatusTypeDescription" HeaderText="Fund Status" 
                                            SortExpression="StatusTypeDescription">
                                            <HeaderStyle Width="15%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="FundEventID" HeaderText="FundEventID" 
                                            SortExpression="FundEventID" Visible="False"></asp:BoundColumn>
										<asp:BoundColumn DataField="MRDYear" SortExpression="MRDYear" 
                                            HeaderText="Year" Visible="False">
										</asp:BoundColumn>
									    <asp:BoundColumn DataField="Process_Status" HeaderText="Process Status" 
                                            SortExpression="Process_Status" Visible="true">
                                            <HeaderStyle Width="20%" />
                                        </asp:BoundColumn>
									   
									</Columns>
								</asp:datagrid>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <%--SG: 2012.03.16: BT-1011 --%>
                        <%--Start code Added by dinesh Kanojia BT:2012 : YRS 5.0-2063: Handling RMD candidates --%>
                        <%--<span class="BG_ColourIsLocked">&nbsp;&nbsp;</span> - Account locked. <span class="BG_ColourIsBlocked">
                            &nbsp;&nbsp;</span> - Insufficient balance. <span class="BG_ColourIsMultipleYearMRD">
                                &nbsp;&nbsp;</span> - Prior period RMD's exists. <span class="BG_ColourIsAnnualMRDPayments">
                                    &nbsp;&nbsp;</span> - Annual RMD Payment is not set.--%>
                        <%--SG: 2012.03.16: BT-1011 
                        <span style="background-color: LightGreen">&nbsp;&nbsp;</span> - Account locked.
                        <span style="background-color: LightPink">&nbsp;&nbsp;</span> - Insufficient balance.
                        <span style="background-color: LightBlue">&nbsp;&nbsp;</span> - Prior period
                        RMD's exists.
                    <span style="background-color: Chocolate">&nbsp;&nbsp;</span> - Manual cash out
                        --%>
                        <table align="center">
                            <tr>
                                <td align="left">
                                    <span class="BG_ColourIsLocked">&nbsp;&nbsp;</span> - Account locked.
                                </td>
                                <td align="left">
                                    <span class="BG_ColourIsBlocked">&nbsp;&nbsp;</span> - Insufficient balance.
                                </td>
                                <td align="left">
                                    <span class="BG_ColourInitialLetter">&nbsp;&nbsp;</span> - Initial Letter Processed.
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="BG_ColourIsMultipleYearMRD">&nbsp;&nbsp;</span> - Prior period RMD's
                                    exists.
                                </td>
                                <td colspan ="2">
                                    <span class="BG_ColourIsNotEnrollAnnualMRDPayments">&nbsp;&nbsp;</span> - Not Enroll
                                    For Annual RMD Payment.
                                </td>
                            </tr>
                        </table>
                        <%--End code Added by dinesh Kanojia BT:2012 : YRS 5.0-2063: Handling RMD candidates --%>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<table class="Table_WithOutBorder" width="700">
    <tr>
        <td class="Td_ButtonContainer" align="center">
            <asp:button id="btnLoad" accesskey="C" cssclass="Button_Normal" text="Load" runat="server"
                width="80"></asp:button>
        </td>
        <td class="Td_ButtonContainer" align="center">
            <asp:button id="btnProcess" accesskey="C" cssclass="Button_Normal" text="Process Distributions"
                runat="server" width="150" enabled="true"></asp:button>
        </td>
        <td class="Td_ButtonContainer" align="center" colspan="2">
            <asp:button id="btnCloseCurrentMRD" accesskey="C" cssclass="Button_Normal" text="Close Current RMD"
                runat="server" width="150"></asp:button>
        </td>
        <%--START: SG: 2012.03.15: BT-1011 --%>
        <td class="Td_ButtonContainer" align="center">
            <asp:button id="btnPrintLetter" accesskey="C" cssclass="Button_Normal" text="Print Letters"
                runat="server" width="90" enabled="true"></asp:button>
        </td>
        <td class="Td_ButtonContainer" align="center">
            <asp:button id="btnPrintReport" accesskey="C" cssclass="Button_Normal" text="Print Report"
                runat="server" width="90" enabled="false"></asp:button>
        </td>
        <%--END: SG: 2012.03.15: BT-1011 --%>
        <td class="Td_ButtonContainer" align="center">
            <asp:button id="btnCancel" accesskey="C" cssclass="Button_Normal" text="Cancel" runat="server"
                width="80"></asp:button>
        </td>
    </tr>
</table>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
