<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SecurityReports.aspx.vb" Inherits="YMCAUI.SecurityReports"%>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					Select a Report
				</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<table width="700" class="Table_WithBorder">
		<tr>
			<td align="left" colspan="2">
				<asp:RadioButton id="RadioButtonUsersReport" runat="server" Text="Users Report (Select Type)" Width="192px"
					Checked="True" GroupName="ReportType"></asp:RadioButton></td>
		</tr>
		<tr>
			<td width="80"></td>
			<td align="left">
				<asp:RadioButton id="RadioButton" runat="server" Text="Listing" Checked="True" GroupName="UserType"></asp:RadioButton></td>
		</tr>
		<tr>
			<td width="80"></td>
			<td align="left">
				<asp:RadioButton id="RadioButtonGroupMembership" runat="server" Text="GroupMembership" GroupName="UserType"></asp:RadioButton></td>
		</tr>
		<tr>
			<td colspan="2">&nbsp;</td>
		</tr>
		<tr>
			<td colspan="2" align="left"><asp:RadioButton id="RadiobuttonGroupReport" runat="server" Text="Groups Report (groups and their members)"
					Width="296px" GroupName="ReportType"></asp:RadioButton></td>
		</tr>
	</table>
	<table>
		<tr>
			<td>&nbsp;</td>
		</tr>
	</table>
	<table width="700" class="Table_WithBorder">
		<tr>
			<td align="left">
				<asp:Label id="LabelInclude" runat="server" Width="208px">Include these users</asp:Label></td>
		</tr>
		<tr>
			<td align="left"><asp:RadioButton id="RadiobuttonAllUsers" runat="server" Text="All Users" GroupName="IncludeType"
					Checked="True"></asp:RadioButton></td>
		</tr>
		<tr>
			<td align="left"><asp:RadioButton id="RadiobuttonActiveUsers" runat="server" Text="All Active Users" GroupName="IncludeType"></asp:RadioButton></td>
		</tr>
		<tr>
			<td align="left"><asp:RadioButton id="RadiobuttonInactiveUsers" runat="server" Text="All Inactive Users" GroupName="IncludeType"></asp:RadioButton></td>
		</tr>
	</table>
	<table>
		<tr>
			<td>&nbsp;</td>
		</tr>
	</table>
	<table width="700" class="Table_WithBorder">
		<tr>
			<td><asp:Label id="LabelOutputReport" runat="server" Width="208px">Output this report to</asp:Label></td>
		</tr>
		<tr>
			<td colspan="3" align="left"><asp:RadioButton id="RadiobuttonScreen" runat="server" Text="Screen" GroupName="OutputType" Checked="True"></asp:RadioButton></td>
		</tr>
		<tr>
			<td colspan="3" align="left"><asp:RadioButton id="RadiobuttonPrinter" runat="server" Text="Printer" GroupName="OutputType"></asp:RadioButton></td>
		</tr>
		<tr>
			<td align="left"><asp:RadioButton id="RadiobuttonFile" runat="server" Text="File" GroupName="OutputType"></asp:RadioButton></td>
			<td align="right">
				<asp:Button id="ButtonFileBrowse" runat="server" Text="..." Width="24px"></asp:Button></td>
			<td align="left">
				<asp:TextBox id="TextBoxFileBrowse" runat="server" Width="187px"></asp:TextBox></td>
		</tr>
	</table>
	<table>
		<tr>
			<td>&nbsp;</td>
		</tr>
	</table>
	<table width="700">
		<tr>
			<td width="175"></td>
			<td width="175"><asp:Button id="ButtonRun" runat="server" Text="Run" Width="63px"></asp:Button></td>
			<td width="175"><asp:Button id="ButtonOk" runat="server" Text="OK" Width="69px"></asp:Button></td>
			<td width="175"></td>
		</tr>
	</table>
</form>
<!--#include virtual="bottom.html"-->
