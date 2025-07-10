<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx"%>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AddOfficerWebForm.aspx.vb" Inherits="YMCAUI.AddOfficerWebForm"%>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
<LINK href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
<script src="JS/jquery-1.5.1.min.js" type="text/javascript"></script>
	<script language="javascript">
	    function ValidateSpaceBar() {
	        if (event.keyCode == 32) {
	            event.returnValue = false;
	        }
	        else {
	            event.returnValue = true;
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
    <asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder>
		<asp:Panel id="PanelAddOfficer" runat="server">
			<DIV class="Div_Center">
				<TABLE width="742">
					<TR>
						<TD align="center">
							<YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL id="YMCA_Toolbar_WebUserControl1" runat="server" ShowLogoutLinkButton="false" ShowHomeLinkButton="false"></YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL></TD>
					</TR>
					<TR>
						<TD class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
							Add Officer</TD>
					</TR>
					<TR>
						<TD>&nbsp;
						</TD>
					</TR>
				</TABLE>
			</DIV>
			<DIV class="Div_Center">
				<TABLE class="Table_WithBorder" width="742" border="0">
					<TR vAlign="top">
						<TD>
						<TD>&nbsp;</TD>
						</TD></TR> <!--<tr valign="top">
				<td align="left" width="15%"><asp:label id="LabelName" runat="server" CssClass="Label_Small">Name</asp:label></td>
				<td align="left" width="85%"><asp:textbox id="TextBoxName" runat="server" CssClass="TextBox_Normal" MaxLength="60"></asp:textbox></td>
			</tr>-->  <!--Start:Added by shashi:03-Mar-2010-->
					<TR vAlign="top">
						<TD align="left" width="15%">
							<asp:label id="LabelFundNo" runat="server" CssClass="Label_Small">Fund No.</asp:label></TD>
						<TD align="left" width="85%">
							<asp:textbox id="TextboxFundNo" runat="server" CssClass="TextBox_Normal" MaxLength="20"></asp:textbox>
							<asp:button id="ButtonUnlink" runat="server" CssClass="Button_Normal" CausesValidation="False"
								Width="160" Text="Unlink from participant"></asp:button>
							<asp:button id="ButtonSearch" runat="server" CssClass="Button_Normal" CausesValidation="False"
								Width="120" Text="Search Officer"></asp:button></TD>
					</TR>
					<TR vAlign="top">
						<TD align="left" width="15%">
							<asp:label id="LabelFname" runat="server" CssClass="Label_Small">First Name</asp:label></TD>
						<TD align="left" width="85%">
							<asp:textbox id="TextboxFname" runat="server" CssClass="TextBox_Normal" MaxLength="20"></asp:textbox>
							<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" CssClass="Error_Message" ErrorMessage="First Name cannot be blank"
								ControlToValidate="TextboxFname">*</asp:requiredfieldvalidator></TD>
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
							<asp:requiredfieldvalidator id="Requiredfieldvalidator6" runat="server" CssClass="Error_Message" ErrorMessage="Last Name cannot be blank"
								ControlToValidate="TextboxLname">*</asp:requiredfieldvalidator></TD>
					</TR> <!--End: shashi:03-Mar-2010-->
					<TR vAlign="top">
						<TD align="left" width="15%">
							<asp:label id="LabelTitle" runat="server" CssClass="Label_Small">Title</asp:label></TD>
						<TD align="left" width="85%">
							<asp:textbox id="TextboxTitle" runat="server" CssClass="TextBox_Normal" readonly="true"></asp:textbox>
							<asp:button id="ButtonTitle" runat="server" CssClass="Button_Normal" CausesValidation="False"
								Width="87" Text="Titles" />
                            <%--START: Chandra sekar | 2018.06.07 | YRS-AT-3961 | Replaced existing error message text
							<asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" CssClass="Error_Message" ErrorMessage="Title can't be Blank"
								ControlToValidate="TextboxTitle">*</asp:requiredfieldvalidator></TD> --%>
							<asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" CssClass="Error_Message" ErrorMessage="Title cannot be blank" 
								ControlToValidate="TextboxTitle">*</asp:requiredfieldvalidator></TD>
                            <%--END: Chandra sekar | 2018.06.07 | YRS-AT-3961 | Replaced existing error message text --%>
					</TR>
					<TR vAlign="top">
						<TD align="left" width="10%">
							<asp:label id="LabelTelephone" runat="server" CssClass="Label_Small">Phone No</asp:label></TD>
						<TD align="left" width="90%">
							<TABLE>
								<TR vAlign="top">
									<TD align="left" width="30%">
										<asp:textbox id="TextboxTelephone" runat="server" CssClass="TextBox_Normal" MaxLength="25" width="100%"></asp:textbox>
                                        <%--START: Chandra sekar | 2018.06.07 | YRS-AT-3961 | Replaced existing error message text
										<asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" CssClass="Error_Message" ErrorMessage="Telephone cannot be blank"
											ControlToValidate="TextboxTelephone" Diplay="Dynamic">*</asp:requiredfieldvalidator> --%>
										<asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" CssClass="Error_Message" ErrorMessage="Phone No. cannot be blank" 
											ControlToValidate="TextboxTelephone" Diplay="Dynamic">*</asp:requiredfieldvalidator>
                                        <%--END: Chandra sekar | 2018.06.07 | YRS-AT-3961 | Replaced existing error message text --%>
										<%-- START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
                                        <asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" CssClass="Error_Message" ErrorMessage="Telephone should be numeric"
											ControlToValidate="TextboxTelephone" Diplay="Dynamic" ValidationExpression="[0-9]*">Telephone should be numeric</asp:regularexpressionvalidator></TD>--%>
										<asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" CssClass="Error_Message" ErrorMessage="Please provide valid Telephone number"
											ControlToValidate="TextboxTelephone" Diplay="Dynamic" ValidationExpression="[0-9]*">*</asp:regularexpressionvalidator></TD>
                                        <%--END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
									<TD align="left" width="8%">
										<asp:label id="LabelExtnNo" runat="server" CssClass="Label_Small">Ext.</asp:label></TD>
									<TD align="left" width="62%">
										<asp:textbox id="TextboxExtnNo" runat="server" CssClass="TextBox_Normal" MaxLength="6"></asp:textbox>
										<asp:regularexpressionvalidator id="Regularexpressionvalidator2" runat="server" CssClass="Error_Message" ErrorMessage="ExtnNo should be numeric"
											ControlToValidate="TextboxExtnNo" ValidationExpression="[0-9]*">*</asp:regularexpressionvalidator></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
					<TR vAlign="top">
						<TD align="left" width="15%">
							<asp:label id="LabelOfficersEmail" runat="server" CssClass="Label_Small">Email</asp:label></TD>
						<TD align="left" width="85%">
							<asp:textbox id="TextBoxOficersEmail" runat="server"></asp:textbox>
							<asp:regularexpressionvalidator id="ValidateOfficersEmail" runat="server" CssClass="Error_Message" ErrorMessage="Invalid Email Format"
								ControlToValidate="TextBoxOficersEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:regularexpressionvalidator></TD>
					</TR>
					<TR vAlign="top">
						<TD align="left" width="15%">
							<asp:label id="LabelEffectiveDate" runat="server" CssClass="Label_Small">Effective Date</asp:label></TD>
						<TD align="left" width="85%">
							<uc1:dateusercontrol id="TextboxEffectiveDate" runat="server"></uc1:dateusercontrol></TD>
					</TR>
					<TR>
						<TD align="left" width="15%"></TD>
						<TD align="left" width="85%">
							<asp:validationsummary id="ValidationSummary1" runat="server" CssClass="Error_Message" Width="336px"></asp:validationsummary></TD>
					</TR>
					<TR vAlign="top">
						<TD height="22">&nbsp;</TD>
					</TR>
					<TR vAlign="top">
						<TD class="Td_ButtonContainer" align="right" colSpan="2">
							<asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="87" Text="OK"></asp:button>&nbsp;&nbsp;&nbsp;
							<asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" CausesValidation="False"
								Width="87" Text="Cancel"></asp:button></TD>
					</TR>
				</TABLE>
			</DIV>
		</asp:Panel>
		<asp:Panel id="PanelSearchOfficer" runat="server">
			<DIV class="Div_Center">
				<TABLE cellSpacing="0" width="742" border="0">
					<TR>
						<TD align="center">
							<YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL id="Ymca_toolbar_webusercontrol2" runat="server" ShowHomeLinkButton="false" ShowLogoutLinkButton="false"></YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL></TD>
					</TR>
					<TR>
						<TD class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
							Search Officer</TD>
					</TR>
					<TR>
						<TD>&nbsp;
						</TD>
					</TR>
				</TABLE>
				<TABLE class="Table_WithBorder" width="742">
					<TR width="100%">
						<TD align="left" width="100%">
							<asp:label id="LabelNoRecord" runat="server" CssClass="Label_Small" Visible="False">No Matching Records</asp:label></TD>
						</TD>
					</TR>
					<TR width="100%">
						<TD align="left" width="100%" colspan="3">
							<asp:label id="LabelNoneRecord" runat="server" CssClass="Label_Small" Visible="False">No one with the search criteria currently employed at this YMCA. Please check the following list for a possible match. </asp:label>
						</TD>
					</TR>
					<TR>
						<TD>&nbsp;
						</TD>
					</TR>
					<TR>
						<TD>
							<DIV style="OVERFLOW: auto; WIDTH: 480px; HEIGHT: 200px">
								<asp:DataGrid id="dg" runat="server" CssClass="DataGrid_Grid" Width="461" allowsorting="true"
									AutoGenerateColumns="false" PageSize="500" allowPaging="False" EnableViewState="False">
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
										<asp:BoundColumn DataField="Title" SortExpression="Title" HeaderText="Title"></asp:BoundColumn>
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
							</DIV>
						</TD>
						<TD>&nbsp;</TD>
						<TD vAlign="top" align="right">
							<TABLE width="220">
								<TR>
									<TD colSpan="2">&nbsp;
									</TD>
								</TR>
								<TR>
									<TD colSpan="2">&nbsp;
									</TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:Label id="LabelListFundNo" CssClass="Label_Small" width="100" Runat="server">Fund No.</asp:Label></TD>
									<TD align="left">
										<asp:TextBox id="TextBoxListFundNo" runat="server" CssClass="TextBox_Normal" MaxLength="10" width="120"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:Label id="LabelLastName" CssClass="Label_Small" width="100" Runat="server">Last Name</asp:Label></TD>
									<TD align="left">
										<asp:TextBox id="TextBoxLastName" runat="server" CssClass="TextBox_Normal" MaxLength="30" width="120"></asp:TextBox></TD>
								</TR>
								<TR>
									<TD align="left">
										<asp:Label id="LabelFirstName" CssClass="Label_Small" width="100" Runat="server">First Name</asp:Label></TD>
									<TD align="left">
										<asp:TextBox id="TextBoxFirstName" runat="server" CssClass="TextBox_Normal" MaxLength="20" width="120"></asp:TextBox></TD>
								</TR>
							</TABLE>
							<TABLE align="right">
								<TR>
								</TR>
								<TR>
									<TD align="left">
										<asp:Button id="ButtonFind" runat="server" CssClass="Button_Normal" Text="Find" Width="80" CausesValidation="False"></asp:Button>&nbsp;&nbsp;</TD>
									<TD align="left">
										<asp:Button id="ButtonClear" runat="server" CssClass="Button_Normal" Text="Clear" Width="80"
											CausesValidation="False"></asp:Button></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
				<TABLE>
					<TR>
						<TD>&nbsp;</TD>
					</TR>
				</TABLE>
				<TABLE class="Table_WithoutBorder" cellSpacing="0" width="742">
					<TR>
						<TD class="Td_ButtonContainer" align="right">
							<asp:button id="ButtonCancelSearch" runat="server" CssClass="Button_Normal" Text="Cancel" Width="80"
								CausesValidation="False"></asp:button></TD>
					</TR>
				</TABLE>
			</DIV>
			</form></asp:Panel></FORM> 
<!--#include virtual="bottom.html"-->
