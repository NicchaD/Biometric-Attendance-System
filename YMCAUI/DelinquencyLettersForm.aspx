<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DelinquencyLettersForm.aspx.vb" Inherits="YMCAUI.DelinquencyLettersForm" MasterPageFile="~/MasterPages/YRSMain.Master" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>


<asp:Content ID="Delinquencycontenthead" runat="server" ContentPlaceHolderID="head">
        <title>DelinquencyLetters</title> 
		
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript">
		function ChangeHeader(lettertype)
		{
			if(lettertype==4)
			{
			var obj,obj1;
			
			obj=document.getElementById("DataGridHeader");
		//	alert(obj);
				obj =document.getElementById("HeaderFor16thBusDay");
			//	alert(obj);
				obj.style.display="block";
				
				obj1 =document.getElementById("GridHeader");
			//	alert(obj1);
				obj1.style.display="none";				
			}
			else
			{
				var obj =document.getElementById("GridHeader");	
			//	alert(obj);		
				obj.style.display="block";
				var obj1 =document.getElementById("HeaderFor16thBusDay");	
			//	alert(obj1);		
				obj1.style.display="none";
			
			}
		}
		//ChangeHeader(4);
		//$(document).ready(function () { if ($('#lblLetterType').text() == '12th Business Day Letter') { $('#lblLetterType').text('11th Business Day Letter'); } $('#dgLetterType TR:nth-child(3) TD:nth-child(2)').text('11th Business Day Letter'); });
		//$(document).ready(function () { if ($('#lblLetterType').text() == '14th Business Day Letter') { $('#lblLetterType').text('13th Business Day Letter'); } $('#dgLetterType TR:nth-child(4) TD:nth-child(2)').text('13th Business Day Letter'); });
		</script>
	</asp:Content>
<asp:Content ID="DelinquencycontentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
			<%--<table class="Table_WithoutBorder" style="WIDTH: 730px; HEIGHT: 65px" cellSpacing="0" cellPadding="0"
				width="730" align="center">
				<tr align="center">
					<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
							CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
							MenuFadeDelay="2" mouseovercssclass="MouseOver"><SELECTEDMENUITEMSTYLE BackColor="#FBC97A" ForeColor="#3B5386"></SELECTEDMENUITEMSTYLE>
						</cc1:menu></td>
				</tr>
				<tr>
					<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
						Delinquency Letters
					</td>
				</tr>
				<tr>
					<td>&nbsp;
					</td>
				</tr>
			</table>--%>
			<table class="Table_WithBorder" width="100%" align="center">
				<tr>
					<td align="center"><asp:label id="Label1" runat="server" CssClass="Label_Large">Business Days</asp:label></td>
				</tr>
				<tr>
					<td>
						<div style="OVERFLOW: auto; WIDTH: 100%; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 115px; BORDER-BOTTOM-STYLE: none; text-align:center;">
                        <asp:datagrid id="dgLetterType" runat="server" CssClass="DataGrid_Grid" AutoGenerateColumns="False"
								PageSize="5" width="50%">
								<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
								<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
								<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
								<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="SNo" HeaderText="S No."></asp:BoundColumn>
									<asp:BoundColumn DataField="Letter Type" HeaderText="Letter Type"></asp:BoundColumn>
									<asp:BoundColumn DataField="BusinessDate" HeaderText="Business Date" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundColumn>
								</Columns>
								<PagerStyle Mode="NumericPages"></PagerStyle>
							</asp:datagrid></div>
					</td>
				</tr>
				<tr>
					<td align="center">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:label id="lblLetterType" runat="server" CssClass="Label_Medium"></asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					</td>
				</tr>
				<tr>
					<td>
						<table id="DataGridHeader" border="0">
							<tr id="GridHeader" class="DataGrid_HeaderStyle" style="DISPLAY:block">
								<td width="4%">Select</td>
								<td width="7.5%">YMCA No</td>
								<td width="28%">YMCA Name</td>
								<td width="15%">Active Resolution</td>
								<td width="6%"># of Delinq Transmittals</td>
								<td width="10%">Earliest Delinq Transmittal</td>
								<td width="6%">Add'l Accts</td>
								<td width="9%"># Of Employees</td>
								<td width="8%">Urban / Metro</td>
							</tr>
							<tr id="HeaderFor16thBusDay" class="DataGrid_HeaderStyle" style="DISPLAY:none">
								<td width="2%">Select</td>
								<td width="7.5%">YMCA No</td>
								<td width="25%">YMCA Name</td>
								<td width="14%">Active Resolution</td>
								<td width="6%"># of Delinq Transmittals</td>
								<td width="10%">Earliest Delinq Transmittal</td>
								<td width="6%">Add'l Accts</td>
								<td width="9%"># Of Employees</td>
								<td width="8%">Emp/YMCA Contributions</td>
								<td width="8%">Urban / Metro</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<div id="Datagrid_Delinquency" style="OVERFLOW: auto; WIDTH: 100%; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 280px; BORDER-BOTTOM-STYLE: none">
                        <asp:datagrid id="DataGridDelinquency" runat="server" CssClass="DataGrid_Grid" AutoGenerateColumns="False"
								PageSize="15" width="100%" allowsorting="true">
								<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
								<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
								<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
								<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
								<Columns>
									<asp:TemplateColumn HeaderText="Sort" ItemStyle-Width="4.9%">
										<ItemTemplate>
											<asp:CheckBox ID="chkFlag" Runat="server"></asp:CheckBox>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="chrYmcaNo" SortExpression="chrYmcaNo" HeaderText="Sort   " ItemStyle-Width="7.8%"></asp:BoundColumn>
									<asp:BoundColumn DataField="chvYmcaName" SortExpression="chvYmcaName" HeaderText="Sort     " ItemStyle-Width="27%"></asp:BoundColumn>
									<asp:BoundColumn DataField="chvShortDescription" SortExpression="chvShortDescription" HeaderText="Sort             "
										ItemStyle-Width="14%"></asp:BoundColumn>
									<asp:BoundColumn DataField="Counts" SortExpression="Counts" HeaderText="Sort" ItemStyle-Width="8%"></asp:BoundColumn>
									<asp:BoundColumn DataField="MinPayRollDate" SortExpression="MinPayRollDate" HeaderText="Sort" DataFormatString="{0:d}"
										ItemStyle-Width="10%"></asp:BoundColumn>
									<asp:BoundColumn DataField="FundStatus" SortExpression="FundStatus" HeaderText="Sort" ItemStyle-Width="5%"></asp:BoundColumn>
									<asp:BoundColumn DataField="ValidEmail" SortExpression="ValidEmail" HeaderText="ValidEmail" Visible="False"></asp:BoundColumn>
									<asp:BoundColumn DataField="NoOfEmployees" SortExpression="NoOfEmployees" HeaderText="Sort" ItemStyle-Width="9%"></asp:BoundColumn>
									<asp:BoundColumn DataField="EmployeeContribution" SortExpression="EmployeeContribution" HeaderText="Emp/YMCA Contribution"
										Visible="False" ItemStyle-Width="10%"></asp:BoundColumn>
									<asp:BoundColumn DataField="URBAN_OR_METRO" SortExpression="URBAN_OR_METRO" HeaderText="Sort" Visible="true"
										ItemStyle-Width="6%"></asp:BoundColumn>
								</Columns>
								<PagerStyle Mode="NumericPages"></PagerStyle>
							</asp:datagrid></div>
					</td>
				</tr>
				<tr>
					<td>
						<table width="100%">
							<tr>
								<td align="left" width="23%"><asp:label id="LabelYmcas" runat="server" CssClass="Label_Medium">Number of YMCAs Listed  </asp:label></td>
                                   <td align="left" width="28%">
                                    <asp:textbox id="TextBoxYmcaList" runat="server" CssClass="TextBox_Normal" ReadOnly="True"
										Width="50px" ></asp:textbox>
                                   </td>
								<td width="25%">&nbsp;&nbsp;
									<asp:label id="LabelSelectedYmcas" runat="server" CssClass="Label_Medium">Number of YMCAs Selected  </asp:label></td>
                                        <td align="left" width="35%"><asp:textbox id="TextBoxYmcaSelected" runat="server" CssClass="TextBox_Normal" ReadOnly="True"
										Width="50px" align="right" ></asp:textbox></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="Td_ButtonContainer">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:button id="ButtonSelectAll" runat="server" CssClass="Button_Normal" Width="100px" Text="Select All"
							        Enabled="False"></asp:button>
                                </td>
                                <td>
                                    <asp:button id="ButtonUpdateCounter" runat="server" CssClass="Button_Normal" Width="110px" Text="Update Counter"
							        Enabled="TRUE"></asp:button>
                                </td>
                                <td align="center">
                                    <asp:button id="btnSelect" CssClass="Button_Normal" Width="100px" Text="Display Data" Runat="server"></asp:button>            
                                </td>
                                <td align="center">
                                    <asp:button id="ButtonProcess" runat="server" CssClass="Button_Normal" Width="100px" Text="Send Emails"
							        Enabled="False"></asp:button>
                                </td>
                                <td align="right">
                                    <asp:button id="ButtonReport" runat="server" CssClass="Button_Normal" Text="Show Report" Enabled="False"></asp:button>
                                </td>
                                <td align="right">
                                    <asp:button id="ButtonClose" CssClass="Button_Normal" Width="100px" Text="Close" Runat="server"></asp:button>
                                </td>
                            </tr>
                        </table>
                        </td>
				</tr>
			</table>
			<asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder>

</asp:Content>