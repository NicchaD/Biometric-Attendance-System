<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddContactWebForm.aspx.vb"
    Inherits="YMCAUI.AddContactWebForm" %>

<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
<script src="JS/jquery-1.5.1.min.js" type="text/javascript"></script>
<script language="javascript">

	function OnSpace()
	{
		
	if ((event.keyCode < 48)||(event.keyCode > 57))
		{
			event.returnValue = false;
		}
	}
     $(document).ready(function () {
	        $('#TextBoxListFundNo').focus();
	        $('#TextBoxListFundNo').bind("keydown", function () { if (event.which || event.keyCode) { if ((event.which == 13) || (event.keyCode == 13)) { $('#ButtonFind').click(); return false; } } else { return true }; });
	        $('#TextBoxLastName').bind("keydown", function () { if (event.which || event.keyCode) { if ((event.which == 13) || (event.keyCode == 13)) { $('#ButtonFind').click(); return false; } } else { return true }; });
	        $('#TextBoxFirstName').bind("keydown", function () {if (event.which || event.keyCode) { if ((event.which == 13) || (event.keyCode == 13)) { $('#ButtonFind').click(); return false; } } else { return true }; });
	    });
</script>
<form id="Form1" method="post" runat="server">
<asp:panel id="PanelAddContact" runat="server">
			<DIV class="Div_Center">
				<TABLE width="742">
					<TR>
						<TD align="center">
							<YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL id="YMCA_Toolbar_WebUserControl1" runat="server" ShowHomeLinkButton="false" ShowLogoutLinkButton="false"></YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL></TD>
					</TR>
					<TR>
						<TD class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
							Add Contact</TD>
					</TR>
					<TR>
						<TD>&nbsp;
						</TD>
					</TR>
					<TR>
						<TD></TD>
					</TR>
				</TABLE>
			</DIV>
			<DIV class="Div_Center">
				<TABLE class="Table_WithBorder" width="742" border="0">
					<TR vAlign="top">
						<TD align="left" width="15%">
							<asp:label id="LabelFundNo" runat="server" CssClass="Label_Small">Fund No.</asp:label></TD>
						<TD align="left" width="85%">
							<asp:textbox id="TextboxFundNo" runat="server" CssClass="TextBox_Normal" MaxLength="20"></asp:textbox>
							<asp:button id="ButtonUnlink" runat="server" CssClass="Button_Normal" CausesValidation="False"
								Width="160" Text="Unlink from participant"></asp:button>&nbsp;
							<asp:button id="ButtonContact" runat="server" CssClass="Button_Normal" CausesValidation="False"
								Width="120" Text="Search Contact"></asp:button></TD>
					</TR>
					<TR vAlign="top">
						<TD align="left" width="15%">
							<asp:label id="LabelType" runat="server" CssClass="Label_Small">Type</asp:label></TD>
						<TD align="left" width="85%">
							<asp:dropdownlist id="DropDownListType" runat="server" CssClass="DropDown_Normal" Width="154px"></asp:dropdownlist>
							<asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" CssClass="Error_Message" ControlToValidate="DropDownListType"
								ErrorMessage="Type cannot be blank">*</asp:RequiredFieldValidator></TD>
					</TR> <!--<tr valign="top">
				<td align="left" width="15%"><asp:label id="LabelName" runat="server" CssClass="Label_Small">Name</asp:label></td>
				<td align="left" width="85%"><asp:textbox id="TextboxName" runat="server" CssClass="TextBox_Normal" MaxLength="60"></asp:textbox></td>
			</tr>-->  <!--Start:Added by shashi:03-Mar-2010-->
					<TR vAlign="top">
						<TD align="left" width="15%">
							<asp:label id="LabelFname" runat="server" CssClass="Label_Small">First Name</asp:label></TD>
						<TD align="left" width="85%">
							<asp:textbox id="TextboxFname" runat="server" CssClass="TextBox_Normal" MaxLength="20"></asp:textbox>
							<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" CssClass="Error_Message" ControlToValidate="TextboxFname"
								ErrorMessage="First Name cannot be blank">*</asp:requiredfieldvalidator>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
					</TR>
					<TR vAlign="top">
						<TD align="left" width="15%">
							<asp:label id="LabelMname" runat="server" CssClass="Label_Small">Middle Name</asp:label></TD>
						<TD align="left" width="85%">
							<asp:textbox id="TextboxMname" runat="server" CssClass="TextBox_Normal" MaxLength="20"></asp:textbox></TD>
					</TR>
					<TR vAlign="top">
						<TD align="left" width="15%">
							<asp:label id="LabelLname" runat="server" CssClass="Label_Small">Last Name</asp:label></TD>
						<TD align="left" width="85%">
							<asp:textbox id="TextboxLname" runat="server" CssClass="TextBox_Normal" MaxLength="30"></asp:textbox>
							<asp:requiredfieldvalidator id="Requiredfieldvalidator6" runat="server" CssClass="Error_Message" ControlToValidate="TextboxLname"
								ErrorMessage="Last Name cannot be blank">*</asp:requiredfieldvalidator></TD>
					</TR> <!--End: shashi:03-Mar-2010-->
                    <tr valign = "top" >
                     <td align ="left" width="15%">
                     <asp:label id="LabelLtitle" runat="server" CssClass="Label_Small "> Title</asp:label>
                     </td>
                     <td align="left" width="85%">
                     <asp:DropDownList id="DropDownListTitle" runat="server" CssClass="DropDown_Normal" Width="154px"></asp:DropDownList>
                     <asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" CssClass="Error_Message" ControlToValidate="DropDownListTitle"
								ErrorMessage="Title cannot be blank">*</asp:RequiredFieldValidator>
                     </td>
                    </tr>
					<TR vAlign="top">
						<TD align="left" width="10%">
							<asp:label id="LabelTelephone" runat="server" CssClass="Label_Small">Phone No.</asp:label></TD>
						<TD align="left" width="90%">
							<TABLE border="0">
								<TR vAlign="top">
									<TD align="left" width="40%">
										<asp:textbox id="TextboxTelephone" runat="server" CssClass="TextBox_Normal" MaxLength="25" width="100%"></asp:textbox>
										<asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" CssClass="Error_Message" ControlToValidate="TextboxTelephone"
											ErrorMessage="Phone No. cannot be blank">*</asp:requiredfieldvalidator> <%-- MMR | 2018.06.04 | YRS-AT-3961 | Replaced 'Telephone' word with 'Phone No.' in error message text --%>
										<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ControlToValidate="TextboxTelephone"
											ErrorMessage="Please provide valid Telephone number" ValidationExpression="[0-9]*">*</asp:RegularExpressionValidator></TD>
										<%--PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
										<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ControlToValidate="TextboxTelephone"
                                            ErrorMessage="Telephone should be numeric" ValidationExpression="[0-9]*">*</asp:RegularExpressionValidator></TD>--%>
									<TD align="left" width="8%">
										<asp:label id="Label1" runat="server" CssClass="Label_Small">Ext.</asp:label></TD>
									<TD align="left" width="62%">
										<asp:textbox id="TextboxExtnNo" runat="server" CssClass="TextBox_Normal" MaxLength="6"></asp:textbox>
										<asp:RegularExpressionValidator id="Regularexpressionvalidator2" runat="server" CssClass="Error_Message" ControlToValidate="TextboxExtnNo"
											ErrorMessage="ExtnNo should be numeric" ValidationExpression="[0-9]*"></asp:RegularExpressionValidator></TD>
								</TR>
							</TABLE>
						</TD>
						<TD align="left"></TD>
					<TR vAlign="top">
						<TD align="left" width="15%">
							<asp:Label id="LabelContactsEmail" runat="server" CssClass="Label_Small">Email</asp:Label></TD>
						<TD align="left" width="85%">
							<asp:TextBox id="TextBoxContactsEmail" runat="server" CssClass="TextBox_Normal" width="35%"></asp:TextBox>
							<asp:RegularExpressionValidator id="ValidateContactsEmail" runat="server" CssClass="Error_Message" MaxLength="60"
								ControlToValidate="TextBoxContactsEmail" ErrorMessage="Invalid Email Format" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator></TD>
					</TR>
					<TR vAlign="top">
						<TD align="left" width="15%">
							<asp:label id="LabelEffectiveDate" runat="server" CssClass="Label_Small">Effective Date</asp:label></TD>
						<TD align="left" width="85%">
							<uc1:DateUserControl id="TextboxEffectiveDate" runat="server"></uc1:DateUserControl></TD>
					</TR> <!--start of code - Added by hafiz on 16-Nov-2006 for YREN-2884-->
					<TR vAlign="top">
						<TD align="left" width="15%">
							<asp:Label id="LabelContactNotes" runat="server" CssClass="Label_Small">Contact Notes</asp:Label></TD>
						<TD align="left" width="85%">
							<asp:TextBox id="TextboxContactNotes" runat="server" CssClass="TextBox_Normal" MaxLength="75"
								Width="466px"></asp:TextBox></TD>
					</TR> <!--end of code - Added by hafiz on 16-Nov-2006 for YREN-2884-->
					<TR vAlign="top" align="left">
						<TD colSpan="2">
							<asp:ValidationSummary id="ValidationSummary1" runat="server" CssClass="Error_Message"></asp:ValidationSummary></TD>
					</TR>
					<TR>
						<TD class="Td_ButtonContainer" align="right" colSpan="2">
							<asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="87" Text="OK"></asp:button>&nbsp;&nbsp;&nbsp;
							<asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" CausesValidation="False"
								Width="87" Text="Cancel"></asp:button></TD>
					</TR>
				</TABLE>
			</DIV>
			<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
		</asp:panel>
<asp:panel id="PanelSearchContact" runat="server">
			<div class="Div_Center">
				<table width="742" border="0" cellspacing="0">
					<tr>
						<td align="center"><YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL id="Ymca_toolbar_webusercontrol2" runat="server" ShowHomeLinkButton="false" ShowLogoutLinkButton="false"></YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL></td>
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
					<TR width="100%">
						<TD align="left" width="100%" colspan="3">
							<asp:label id="LabelNoneRecord" runat="server" CssClass="Label_Small" Visible="False">No one with the search criteria currently employed at this YMCA. Please check the following list for a possible match. </asp:label>
						</TD>
					</TR>
					<tr>
						<td>&nbsp;
						</td>
					</tr>
					<tr>
						<td>
							<div style="OVERFLOW: auto; WIDTH: 480px; HEIGHT: 200px">
								<asp:DataGrid id="dg" AutoGenerateColumns="false" runat="server" Width="461" CssClass="DataGrid_Grid"
									allowsorting="true" PageSize="500" allowPaging="False" EnableViewState="False">
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
									<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
									<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<Columns>
										<asp:TemplateColumn>
											<ItemTemplate>
												<asp:ImageButton id="ImgBtSel" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
													CommandName="Select" ToolTip="Select"></asp:ImageButton>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:BoundColumn DataField="FundNo" SortExpression="FundNo" HeaderText="Fund No"></asp:BoundColumn>
										<asp:BoundColumn DataField="LastName" SortExpression="LastName" HeaderText="Last Name"></asp:BoundColumn>
										<asp:BoundColumn DataField="FirstName" SortExpression="FirstName" HeaderText="First Name"></asp:BoundColumn>
										<asp:BoundColumn DataField="City" SortExpression="City" HeaderText="City"></asp:BoundColumn>
										<asp:BoundColumn DataField="States" SortExpression="States" HeaderText="State"></asp:BoundColumn>
										<asp:BoundColumn DataField="FundStatus" SortExpression="FundStatus" HeaderText="Fund Status"></asp:BoundColumn>
									</Columns>
								</asp:DataGrid>
								<asp:Label id="lbl_Search_MoreItems" runat="server" CssClass="Label_Small" Visible="False"
									EnableViewState="False" />
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
						<td class="Td_ButtonContainer" align="right"><asp:button id="ButtonCancelSearch" runat="server" CssClass="Button_Normal" Width="80" Text="Cancel"
								CausesValidation="False"></asp:button></td>
					</TR>
				</table>
			</div>
			<asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder>
</form>
</asp:Panel></FORM>
<!--#include virtual="bottom.html"-->
