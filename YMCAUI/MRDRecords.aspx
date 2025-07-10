<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MRDRecords.aspx.vb" Inherits="YMCAUI.MRDRecords" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<!--#include virtual="top.html"-->
<head>
    <style type="text/css">
        .style1
        {
            width: 541px;
        }
    </style>
    <script type="text/javascript" src="JS/jquery-1.5.1.min.js"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
</head>
<SCRIPT language="JavaScript">
    function ValidateNumeric() {
        if ((event.keyCode < 48) || (event.keyCode > 57)) {
            event.returnValue = false;
        }
    }

    function initialisedatagrid() {
        //$("#dgMRD1 TR").addClass("AlignControls");
        $("#dgMRD TR TD:nth-child(3)").each(function () { $(this).attr('style', 'text-align: right;'); });
        $("#dgMRD TR TD:nth-child(4)").each(function () { $(this).attr('style', 'text-align: right;'); });
        $("#dgMRD TR TD:nth-child(5)").each(function () { $(this).attr('style', 'text-align: right;'); });
        //$("dgMRD1 table tr:nth-child(2)").attr('style', 'text-align: right;');
        //$("#dgMRD").each(function () { $("dgMRD Columns:nth-child(3)").attr('style', 'text-align: right;');});
    }

    $(document).ready(initialisedatagrid);

</SCRIPT>
<!--<span class="AlignControls" >This is within a AlignControl tag</span>
<table id="dgMRD1" width="100%">
<tr class="DataGrid_AlternateStyle"><td>Test</td><td>Test1</td></tr>
<tr><td>Test2</td><td>Test21</td></tr>
<tr class="DataGrid_AlternateStyle"><td>Test3</td><td>Test31</td></tr>
</table>-->
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
					CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
					MenuFadeDelay="2" mouseovercssclass="MouseOver">
					<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
				</cc1:menu></td>
		</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="left">RMD Records</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
		</tr>
	</table>
	<table class="Table_WithBorder" cellSpacing="0" cellPadding="0" width="700">
		<tr>
			<td align="right" height="22">
				<TABLE class="Table_WithOutBorder" id="Table1"  cellSpacing="1" cellPadding="1" border=0 width="700">
					<TR>
                        <TD align="left" nowrap=true valign="middle" style="width:auto;">
                            <asp:label id="LblMRDDate" 
                                cssclass="Label_Small" text="RMD records last processed for " Runat="server" Font-Names="Arial" 
                                Font-Size="Small">
                                </asp:label>
                                <asp:label id="lblDate" 
                            cssclass="Label_Small" text="" Runat="server" Font-Names="Arial" 
                            Font-Size="Small"></asp:label>
                        </TD>						
						<TD align="right" nowrap=true valign="middle" >
                            <asp:label id="LabelYear" 
                                cssclass="Label_Small" text="Select Year for Display: " Runat="server" Font-Names="Arial" 
                                Font-Size="Small"></asp:label></TD>
						<TD valign="middle" nowrap=true width="80px">
                            <asp:dropdownlist 
                                id="ddlYear" runat="server" AutoPostBack="True" Width="80px" 
                                Font-Names="Arial" Font-Size="Small" style="margin-left: 0px">
							</asp:dropdownlist></TD>
					</TR>					
				</TABLE>
			</td>
		</tr>
		<TR>
			<td align="right">
				<TABLE class="Table_WithOutBorder" id="Table1"  cellSpacing="1" cellPadding="1"
					width="700">
					<tr>
						<td>
							<div style="OVERFLOW: auto; WIDTH: 100%; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 300px; BORDER-BOTTOM-STYLE: none">
                                <asp:datagrid id="dgMRD" runat="server" CssClass="DataGrid_Grid" Width="671px" allowsorting="True"
									AutoGenerateColumns="False" OnSortCommand="SortCommand_OnClick" SelectedItemStyle-VerticalAlign="Top" 
                                    PageSize="20">
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top"></SelectedItemStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<Columns>
										<asp:BoundColumn DataField="FundIdNo" SortExpression="FundIdNo" 
                                            HeaderText="FundId No">
											<HeaderStyle Width="12%" />											
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PlanType" SortExpression="PlanType" 
                                            HeaderText="Plan Type">
											<HeaderStyle Width="12%" />
										</asp:BoundColumn>
										<asp:BoundColumn DataField="CurrentBalance" FooterText="CurrentBalance" 
                                            HeaderText="Current Balance" SortExpression="CurrentBalance" >
                                            <HeaderStyle Width="15%" />                                           
                                        </asp:BoundColumn>
										<asp:BoundColumn DataField="MRDAmount" SortExpression="MRDAmount" 
                                            HeaderText="RMD Amount"  >
											<HeaderStyle Width="15%" />
											 
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PaidAmount" HeaderText="Paid Amount" 
                                            SortExpression="PaidAmount" >
                                            <HeaderStyle Width="13%" />
                                        
                                        </asp:BoundColumn>
										<asp:BoundColumn DataField="MRDExpireDate" SortExpression="MRDExpireDate" 
                                            HeaderText="Expire Date">
											<HeaderStyle Width="12%" />											
										</asp:BoundColumn>
									    <asp:BoundColumn DataField="StatusTypeDescription" HeaderText="Fund Status" 
                                            SortExpression="StatusTypeDescription">
                                            <HeaderStyle Width="15%" />
                                        </asp:BoundColumn>
									</Columns>
								</asp:datagrid></div>
						</td>
					</tr>
                    <td align=center>
                                 <asp:label id="LblMessage" 
                                cssclass="Label_Small"  Runat="server" Font-Names="Arial" 
                                Font-Size="Small" ForeColor="#CC3300"></asp:label>
                    
                    </td>
                    </TABLE>
			</td>
		</TR>
		
	</table>
	<table class="Table_WithOutBorder" width="700">
		<tr>
        
        <td class="Td_ButtonContainer" align="middle"><asp:button id="btnGenerate" accessKey="C" 
                    CssClass="Button_Normal" Text="GENERATE" Runat="server"
					Width="80"></asp:button></td>
                    			
		<td class="Td_ButtonContainer" align="middle"><asp:button id="btnSave" accessKey="C" 
                    CssClass="Button_Normal" Text="OK" Runat="server"
					Width="80"></asp:button></td>
		</tr>
	</table>
	</TABLE><asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder></form>
<!--#include virtual="bottom.html"-->
