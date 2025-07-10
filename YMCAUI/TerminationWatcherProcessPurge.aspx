<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TerminationWatcherProcessPurge.aspx.vb"
	Inherits="YMCAUI.TerminationWatcherProcessPurge"  %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx"%>

<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet" />
<script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script>
<link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
<script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
<script type="text/javascript">
    //'Start :Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 
    function close() {
        if ('<%= Session("Refresh") %>' == 'YES')
            window.opener.document.forms(0).submit();

    }
    window.onbeforeunload = close;
    //'End :Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 
</script>


<body>
	<form id="form1" runat="server">
	<table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="735">
		 <%--Added User Control to show user details --%>
        <tr>
				<td>
					<YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL id="YMCA_Toolbar_WebUserControl1" runat="server" ShowLogoutLinkButton="false" ShowHomeLinkButton="false"></YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL>
				</td>
			</tr>
        <tr>
			<td class="Td_HeadingFormContainer" align="left" style="width: 735;">
				<asp:Label runat="server" ID="lblHead"></asp:Label>
			</td>
		</tr>
		<tr><td> &nbsp; </td></tr>
        <tr>
        
			<td class="Table_WithBorder" width="735" >
                <image id="imgHelp" src="images\help.jpg" width="20" height="20"></image>
                &nbsp;

				<asp:Label runat="server" ID="lblpurgedescr" CssClass="Label_Medium" ></asp:Label>
            
			</td>
		</tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
	</table>
	<div id="divAdd" runat="server" style="overflow: auto; width: 100%;">
		<table cellspacing="0" cellpadding="0">
			<tr>
				<td>
					<table class="Table_WithBorder" width="735">
						<tr>
							<td colspan="2">
						</td>
						</tr>
						<tr valign="top">
							<td valign="top" width="100%">
								<asp:Label CssClass="Label_Small" ID="LabelNoDataFound" runat="server" Visible="False">No Records Found.</asp:Label>
								<div id="divPurge" runat="server" style="overflow: auto; width: 735px; height:325px; text-align: left">

									<asp:GridView ID="gvPurge" runat="server" CssClass="DataGrid_Grid" Width="1100px"
										AllowSorting="True" AllowPaging="True" PageSize="10" AutoGenerateColumns="False"
										DataKeyNames="UniqueId">
										<SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
										<AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
										<RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
										<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
										<Columns>
											<asp:BoundField HeaderText="PersonID" DataField="guiPersID" Visible="false" />
											<asp:BoundField HeaderText="FundEventID" DataField="guiFundEventId" Visible="false" />
											<asp:BoundField HeaderText="TerminationWatcherUniqueId" DataField="UniqueId" Visible="false" />
											<asp:TemplateField>
												<HeaderTemplate>
													<asp:CheckBox ID="chkSelectAll" runat="server" />
												</HeaderTemplate>
												<ItemTemplate>
													<asp:CheckBox ID="chkSelect" runat="server" />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField SortExpression="FundNo" HeaderText="Fund No.">
												<ItemTemplate>
													<asp:Label ID="lblfundNO" runat="server" Text='<%# Bind("FundNo") %>'></asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField SortExpression="FirstName" HeaderStyle-Width="60" HeaderText="First Name">
												<ItemTemplate>
													<asp:Label runat="server" ID="lblFirstName" Text='<%# Bind("FirstName") %>'></asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField SortExpression="LastName" HeaderStyle-Width="60" HeaderText="Last Name">
												<ItemTemplate>
													<asp:Label runat="server" ID="lblLastName" Text='<%# Bind("LastName") %>'></asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField HeaderText="Fund Status" HeaderStyle-Width="80" SortExpression="Status" DataField="Status" />
											<asp:BoundField HeaderText="Last Cont. Rcvd."  HeaderStyle-Width="60px" SortExpression="LastcontributionREceived"
												DataField="LastcontributionREceived" />
											<asp:BoundField HeaderText="Unfunded Trans." HeaderStyle-Width="55px" SortExpression="UnfundedTransactions"
												DataField="UnfundedTransactions" />
											<asp:BoundField HeaderText="Watch Type" HeaderStyle-Width="70" SortExpression="Type" DataField="Type" />
											<asp:BoundField HeaderText="Plan" SortExpression="PlanType" DataField="PlanType" />
											<asp:BoundField HeaderText="Balance(s)" DataFormatString="{0:##,##0.00}"  ItemStyle-HorizontalAlign="Right"
															HeaderStyle-HorizontalAlign="Left" SortExpression="Balance" HeaderStyle-Width="80px" DataField="Balance" />
											<asp:BoundField HeaderText="Status" ReadOnly="true" DataField="Processed" Visible="True" SortExpression="Processed" />
                                            <asp:BoundField HeaderText="Created On" ReadOnly="true" DataField="CreatedDate" Visible="true" SortExpression="CreatedDate" />
                                            <asp:BoundField HeaderText="Termn. Date" ReadOnly="true" DataField="Termn. Date"  DataFormatString="{0:MM/dd/yyyy}" Visible="true" SortExpression="Termn. Date" />
                                                        <asp:BoundField HeaderText="Action Taken Date" ReadOnly="true" DataField="Action Taken Date"  DataFormatString="{0:MM/dd/yyyy}" Visible="true" SortExpression="Action Taken Date" HeaderStyle-Width="80" />
                                             <%--Anudeep:12.12.2012 Changes made to show source of watcher --%>
                                             <asp:BoundField HeaderText="Source" ReadOnly="true" DataField="Source" Visible="true" SortExpression="Source" HeaderStyle-Width="50" />
										</Columns>
										<PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="First" LastPageText="Last" />
										<PagerStyle CssClass="pagination"  />
										</asp:GridView>
                                        </div>
									<div id="divProcess" runat="server" style="overflow: auto; width: 735px; height:325px; text-align: left">
                                    <asp:GridView ID="gvProcessed" runat="server" CssClass="DataGrid_Grid" Width="735px"
										AllowSorting="True" AllowPaging="True" PageSize="10" AutoGenerateColumns="False"
										DataKeyNames="UniqueId">
										<SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
										<AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
										<RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
										<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
										<Columns>
											<asp:BoundField HeaderText="PersonID" DataField="guiPersID" Visible="false" />
											<asp:BoundField HeaderText="FundEventID" DataField="guiFundEventId" Visible="false" />
											<asp:BoundField HeaderText="TerminationWatcherUniqueId" DataField="UniqueId" Visible="false" />
									
											<asp:TemplateField SortExpression="FundNo" HeaderText="Fund No.">
										
												<ItemTemplate>
													<asp:Label ID="lblfundNO" runat="server" Text='<%# Bind("FundNo") %>'></asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField SortExpression="FirstName" HeaderText="First Name">
									
												<ItemTemplate>
													<asp:Label runat="server" ID="lblFirstName" Text='<%# Bind("FirstName") %>'></asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField SortExpression="LastName" HeaderText="Last Name">
									
												<ItemTemplate>
													<asp:Label runat="server" ID="lblLastName" Text='<%# Bind("LastName") %>'></asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField HeaderText="Fund Status" SortExpression="Status" DataField="Status" />
											<asp:BoundField HeaderText="Last Cont. Rcvd." HeaderStyle-Width="40px"  SortExpression="LastcontributionREceived"
												 DataField="LastcontributionREceived" />
											<asp:BoundField HeaderText="Unfunded Trans."  HeaderStyle-Width="55px"  SortExpression="UnfundedTransactions"
												 DataField="UnfundedTransactions" />
											<asp:BoundField HeaderText="Watch Type" SortExpression="Type" DataField="Type" />
											<asp:BoundField HeaderText="Plan" SortExpression="PlanType" DataField="PlanType" />
											<asp:BoundField HeaderText="Balance(s)" DataFormatString="{0:##,##0.00}" HeaderStyle-Width="80px"  ItemStyle-HorizontalAlign="Right"
															HeaderStyle-HorizontalAlign="Left" SortExpression="Balance" DataField="Balance" />
                                           <asp:BoundField HeaderText="Status" ReadOnly="true" DataField="Processed" Visible="True" SortExpression="Processed" />
                                           <asp:BoundField HeaderText="Created On" ReadOnly="true" DataField="CreatedDate" Visible="true" SortExpression="CreatedDate" />
                                           <%--Anudeep:12.12.2012 Changes made to show source of watcher --%>
                                           <asp:BoundField HeaderText="Source" ReadOnly="true" DataField="Source" Visible="true" SortExpression="Source" />
										</Columns>
										<PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="First"
											LastPageText="Last" />
										<PagerStyle Font-Names="Arial" Font-Size="Small" />
									
									</asp:GridView>
                                    
								</div>
                                <asp:Label runat="server" ID="lblTotalCount" CssClass="Label_Small" ></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								&nbsp;
							</td>
						</tr>
						<tr>
							<td align="left">
							
                            	<%--<table width="100%">
									<tr>
										<td valign="top">
											<table class="Table_WithBorder">
												<tr>
													<td width="15px" align="left" runat="server" style="height: 15px; ;" id="tdLastcontri"
														nowrap="nowrap">
													</td>
												</tr>
											</table>
										</td>
										<td>
											<asp:Label ID="lbltdLastcontri" runat="server" CssClass="Label_Small"></asp:Label>
										</td>
										<td valign="top">
											<table class="Table_WithBorder">
												<tr>
													<td width="15px" align="left" style="height: 15px;" runat="server" id="tdUnfunded">
													</td>
												</tr>
											</table>
										</td>
										<td>
											<asp:Label ID="lbltdUnfunded" runat="server" CssClass="Label_Small"></asp:Label>
										</td>
										<td valign="top">
											<table class="Table_WithBorder">
												<tr>
													<td width="15px" align="left" style="height: 15px;" runat="server" id="tdUnfundedandLastContri">
													</td>
												</tr>
											</table>
										</td>
										<td>
											<asp:Label ID="lbltdUnfundedandLastContri" runat="server" CssClass="Label_Small"></asp:Label>
										</td>
									</tr>
								</table>--%>
							
                            </td>
						</tr>
						<tr class="Td_ButtonContainer">
							<td align="right" class="Td_ButtonContainer" colspan="2">
								<asp:Button ID="btnDelete" runat="server" CssClass="Button_Normal" Style="width: 80px;"
									Text="Delete" />&nbsp;
								<asp:Button ID="btnClose" runat="server" CssClass="Button_Normal" Style="width: 80px;"
									Text="Close" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
	</div>
	</form>
	<script language="javascript" type="text/javascript">
		//Code for check all Check boxes Start
		var allCheckBoxSelector = '#<%=gvPurge.ClientID%> input[id*="chkSelectAll"]:checkbox';
		var checkBoxSelector = '#<%=gvPurge.ClientID%> input[id*="chkSelect"]:checkbox';

		function ToggleCheckUncheckAllOptionAsNeeded() {
			var totalCheckboxes = $(checkBoxSelector),
                        checkedCheckboxes = totalCheckboxes.filter(":checked"),
                        noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
                        allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);
			if (totalCheckboxes.length == 0) allCheckboxesAreChecked = false;
			$(allCheckBoxSelector).attr('checked', allCheckboxesAreChecked);
		}

		$(document).ready(function () {
			$(allCheckBoxSelector).bind('click', function () {
				$(checkBoxSelector).attr('checked', $(this).is(':checked'));
				ToggleCheckUncheckAllOptionAsNeeded();
			});

			$(checkBoxSelector).bind('click', ToggleCheckUncheckAllOptionAsNeeded);

			ToggleCheckUncheckAllOptionAsNeeded();
		});



		//Code for check all Check boxes End
	</script>
</body>
<!--#include virtual="bottom.html"-->
