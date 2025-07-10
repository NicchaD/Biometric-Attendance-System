<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchContact.aspx.vb" Inherits="YMCAUI.SearchContact"%>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx"%>
<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
<LINK href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
	<script language="javascript">
function clickButton(e, btnLogin){ 
Alert("1");
     /*   var bt = document.getElementById(btnLogin); 
      if (typeof bt == 'object'){ 
      alert("2");
          if(navigator.appName.indexOf("Netscape")>(-1)){ 
                  if (e.keyCode == 13){ 
                        bt.click(); 
                        return false; 
                  } 
            } 
            if (navigator.appName.indexOf("Microsoft Internet Explorer")>(-1)){ 
                  if (event.keyCode == 13){ 
                        bt.click(); 
                        return false; 
                  } 
            
      } }*/ 
}




	</script>
	<form id="Form1" method="post" runat="server">
		<div class="Div_Center">
			<table width="742" border="0" cellspacing="0">
				<tr>
					<td align="center"><YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL id="YMCA_Toolbar_WebUserControl1" runat="server" ShowHomeLinkButton="false" ShowLogoutLinkButton="false"></YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL></td>
				</tr>
				<tr>
					<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
						Search Contact</td>
				</tr>
				<tr>
					<td>&nbsp;
					</td>
				</tr>
			</table>
			<table width="742" class="Table_WithBorder">
				<TR>
					<TD align="left"><asp:label id="LabelNoRecord" runat="server" Visible="False" CssClass="Label_Small">No Matching Records</asp:label></TD>
				</TR>
				<tr>
					<td>&nbsp;
					</td>
				</tr>
				<tr>
					<td>
						<div style="OVERFLOW: auto; WIDTH: 480px; HEIGHT: 200px">
							<asp:DataGrid id="DataGridList" AutoGenerateColumns="false" runat="server" Width="461" CssClass="DataGrid_Grid"
								allowsorting="true">
								<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
								<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
								<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
								<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
								<Columns>
									<asp:TemplateColumn>
										<ItemTemplate>
											<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
												CommandName="Select" ToolTip="Select"></asp:ImageButton>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="FundNo" SortExpression="FundNo" HeaderText="Fund No"></asp:BoundColumn>
									<asp:BoundColumn DataField="LastName" SortExpression="LastName" HeaderText="Last Name"></asp:BoundColumn>
									<asp:BoundColumn DataField="FirstName" SortExpression="FirstName" HeaderText="First Name"></asp:BoundColumn>
								</Columns>
							</asp:DataGrid>
						</div>
					</td>
					<td>&nbsp;</td>
					<td valign="top" align="right">
						<table width="220">
							<tr>
								<td colspan="2">
									&nbsp;
								</td>
							</tr>
							<tr>
								<td colspan="2">
									&nbsp;
								</td>
							</tr>
							<TR>
								<td align="left">
									<asp:Label ID="LabelListFundNo" Runat="server" CssClass="Label_Small" width="100">Fund No.</asp:Label></td>
								<td align="left">
									<asp:TextBox id="TextBoxListFundNo" runat="server" CssClass="TextBox_Normal" width="120" MaxLength="10"></asp:TextBox>
								</td>
							</TR>
							<TR>
								<td align="left">
									<asp:Label ID="LabelLastName" Runat="server" CssClass="Label_Small" width="100">Last Name</asp:Label></td>
								<td align="left">
									<asp:TextBox id="TextBoxLastName" CssClass="TextBox_Normal" runat="server" width="120" MaxLength="30"></asp:TextBox>
								</td>
							</TR>
							<TR>
								<td align="left">
									<asp:Label ID="LabelFirstName" Runat="server" CssClass="Label_Small" width="100">First Name</asp:Label></td>
								<td align="left">
									<asp:TextBox id="TextBoxFirstName" CssClass="TextBox_Normal" runat="server" width="120" MaxLength="20"></asp:TextBox>
								</td>
								<asp:TextBox runat="server" style="DISPLAY:none;VISIBILITY:hidden" ID="Textbox1" />
							</TR>
						</table>
						<table align="right">
							<tr>
							</tr>
							<tr>
								<td align="left">
									<asp:Button id="ButtonFind" runat="server" Text="Find" Width="80" CssClass="Button_Normal"></asp:Button>
									&nbsp;&nbsp;
								</td>
								<td align="left">
									<asp:Button id="ButtonClear" runat="server" Text="Clear" Width="80" CssClass="Button_Normal"
										CausesValidation="False"></asp:Button>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<table>
				<tr>
					<td>&nbsp;</td>
				</tr>
			</table>
			<table class="Table_WithoutBorder" cellSpacing="0" width="742">
				<TR>
					<td class="Td_ButtonContainer" align="right"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="80" Text="Cancel"
							CausesValidation="False"></asp:button></td>
				</TR>
			</table>
		</div>
		<asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder></form>
<!--#include virtual="bottom.html"-->
