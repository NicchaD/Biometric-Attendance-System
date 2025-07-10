<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TerminationWatcherAddUser.aspx.vb" Inherits="YMCAUI.TerminationWatcherAddUser" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> 
<head>

<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    
    <%--Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484
    <script type="text/javascript">
    //'Start :Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 
        function close() {
        if ('<%= Session("Refresh") %>' == 'YES' )
           window.opener.document.forms(0).submit();

        }
        window.onbeforeunload = close;
        //'End :Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 
</script>--%>
</head>
<body>
    <form id="form1" runat="server">
			<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
			 <%--Added User Control to show user details --%>
            <tr>
				<td>
					<YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL id="YMCA_Toolbar_WebUserControl1" runat="server" ShowLogoutLinkButton="false" ShowHomeLinkButton="false"></YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL>
				</td>
			</tr>
		
		<tr>
			<td class="Td_HeadingFormContainer" align="left" style="width:700;"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="12">
				Termination Watcher > <asp:Label runat="server" ID="lblHead" ></asp:Label>
			</td>
		</tr>
		<tr>
			<td>&nbsp;
			</td>
		</tr>
	</table>

    <div id="divAdd" runat="server" style="OVERFLOW: auto; WIDTH: 100%;">
		<table cellspacing="0" cellpadding="0">
			<tr>
								<td>
									
									<table class="Table_WithBorder" width="735">
								
								<tr>
									<td colspan="2" class="Label_Small"  class="td_Text" align="left" >
										Find Information
									</td>
								</tr>
								<tr valign="top" align="left">

									<td valign="top" width="80%" style="border:1;">
										 <asp:Label cssclass="Label_Small" ID="LabelNoDataFound" runat="server" Visible="False">No Matching Records</asp:Label>
										<div style="border-style: none; border-color: inherit; border-width: 1; OVERFLOW: auto; WIDTH:500px; HEIGHT: 374px; TEXT-ALIGN: left;"  >
										<asp:GridView ID="gvAdd" runat="server" cssclass="DataGrid_Grid" width="500px"
                                                    allowsorting="True"  allowpaging="True" pagesize="10"  DataKeyNames="FundUniqueId,PersID"
												 AutoGenerateColumns="False">
											
												 <SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
													<AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
													<RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
													<HeaderStyle CssClass="DataGrid_HeaderStyle">
													
													</HeaderStyle>
													<Columns>
                                                    
													<asp:BoundField HeaderText="PersonID" DataField="PersID" Visible="false" />
													<asp:BoundField HeaderText="FundEventID" DataField="FundUniqueId" Visible="false" />
														<asp:TemplateField>
																							
														<ItemTemplate>
														
																<asp:ImageButton ID="ImageButtonSelect" runat="server" ToolTip="Select" CommandName="Select" CausesValidation="False"
																	ImageUrl="images\select.gif" AlternateText="Select"></asp:ImageButton>
                                                                    <asp:ImageButton ID="ImageButtonSelected" runat="server" ToolTip="Select" CommandName="Select" CausesValidation="False"
																	ImageUrl="images\selected.gif" Visible="false" AlternateText="Select"></asp:ImageButton>
															</ItemTemplate>
														</asp:TemplateField>
														<asp:BoundField HeaderText="Fund No." SortExpression="FundIDNo" DataField="FundIDNo"  />
														<asp:BoundField HeaderText="SSN No" SortExpression="SSN" DataField="SSN" />
														<asp:BoundField HeaderText="First Name" SortExpression="FirstName" DataField="FirstName" />
														<asp:BoundField HeaderText="Last Name" SortExpression="LastName" DataField="LastName" />
														<asp:BoundField HeaderText="Fund Status" SortExpression="FundStatus" DataField="FundStatus" />	
                                                        <asp:BoundField HeaderText="Watch Type" ></asp:BoundField>
                                                        <asp:BoundField HeaderText="Plan"  /> 																					
														
													</Columns>
														<PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="First" LastPageText="Last"/>
                                                        
                                                    <PagerStyle Font-Bold="false" Font-Size="X-Small"     /> 
													<%--<PagerStyle Visible="False" Mode="NumericPages"></PagerStyle>--%>
										</asp:GridView>
                                        <br />
                                        <asp:Label runat="server" ID="lblTotalCount" Visible="false" CssClass="Label_Small" ></asp:Label>
                                        <br />
                                            <asp:Label runat="server" ID="lblType" CssClass="Label_Small" Text="Type"></asp:Label>
                                            <asp:Label id="lblTypeErr" runat="server" CssClass="Label_Small" Text = "*"></asp:Label>&nbsp;&nbsp;&nbsp;
                                            <asp:DropDownList ID="drdType" runat="server" >
									            <asp:ListItem Value="Withdrawal" Text="Withdrawal"></asp:ListItem>
                                                <asp:ListItem Value="Retirement" Text="Retirement"></asp:ListItem>
    									    </asp:DropDownList>
                                            &nbsp;&nbsp;
							                <asp:Label id="lblPlanType" runat="server" CssClass="Label_Small" Text = "Plan Type"></asp:Label>
								            <asp:Label id="lblPlanTypeErr" runat="server" CssClass="Label_Small" Text = "*"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
								            <asp:DropDownList ID="drdPlantype" runat="server" >
                                            <asp:ListItem Value="-1" Text ="-Select-" Selected="True" ></asp:ListItem>
									                   </asp:DropDownList>	
                                                       <asp:RequiredFieldValidator InitialValue="-1" CssClass="Label_Medium" EnableClientScript="True" 
                                                      ErrorMessage="Please select valid Plan Type."  ID="rfvPlantype" runat="server" ControlToValidate="drdPlantype" Text="*" ></asp:RequiredFieldValidator>
							
							                <asp:ValidationSummary ID="vsPlantype" CssClass="Label_Small" runat="server" />
										
										</div>
									</td>
									<td valign="top">
										 <table>
                                                <tr>
                                                    <td align="left">
                                                        <asp:label cssclass="Label_Small" id="lblFundNo" text="Fund No." runat="server"></asp:label>
                                                    </td>
                                                    <td>
                                                        <asp:textbox width="100" runat="server" id="txtFundno" cssclass="TextBox_Normal"
                                                            maxlength="9"></asp:textbox>
                                                    </td>
                                                    <td>
                                                        <asp:label cssclass="Label_Small" id="lblErrFundNo" text="*" runat="server" forecolor="Red"
                                                            font-bold="True" visible="False"></asp:label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:label cssclass="Label_Small" id="lblSSno" text="SS No." runat="server"></asp:label>
                                                    </td>
                                                    <td>
                                                        <asp:textbox width="100" runat="server" id="txtSSNo" cssclass="TextBox_Normal"
                                                            maxlength="11"></asp:textbox>
                                                    </td>
                                                    <td>
                                                        <asp:label cssclass="Label_Small" id="lblErrSSno" text="*" runat="server" forecolor="Red"
                                                            font-bold="True" visible="False"></asp:label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:label cssclass="Label_Small" id="lblLastName" text="Last Name" runat="server"></asp:label>
                                                    </td>
                                                    <td>
                                                        <asp:textbox width="100" runat="server" id="txtLastName" cssclass="TextBox_Normal"></asp:textbox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="Label_Small" align="left">
                                                        <asp:label cssclass="Label_Small" id="lblFirstName" text="First Name" runat="server"></asp:label>
                                                    </td>
                                                    <td>
                                                        <asp:textbox width="100" runat="server" id="txtFirstName" cssclass="TextBox_Normal"></asp:textbox>
                                                    </td>
                                                    <td>
                                                        <asp:label cssclass="Label_Small" id="lblErrFirstName" text="*" runat="server" forecolor="Red"
                                                            font-bold="True" visible="False"></asp:label>
                                                    </td>
                                               
                                              </tr>
                                                <tr>
                                                    <td>
													
                                                        <asp:Button cssclass="Button_Normal" width="80" runat="server" accesskey="F" id="btnWithdrawalFind"
                                                            text="Find" causesvalidation="False"></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button cssclass="Button_Normal" width="80" runat="server" id="btnWithdrawalClear" text="Clear"
                                                            causesvalidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <table>
                                            <tr>
                                            <td style="width: 120px;">
                                            <asp:Label runat="server" ID="lblAbvr" CssClass="Label_Small">
                                            Abbreviations <br />
                                            Wdl - Withdrawal <br />
                                            Ret - Retirement <br />
                                            Sav - Savings
                                            </asp:Label>
                                            </td>
                                            </tr>
                                            </table>
									</td>

								</tr>
								
								<%--<tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr>--%>
								<tr  class="Td_ButtonContainer">
												<td align="right" class="Td_ButtonContainer" colspan="2">
												<% %>
                                                <asp:Button ID="btnAdd" runat="server" CssClass="Button_Normal" style="width:80px;" text="Add" />&nbsp;

                                                <asp:Button ID="btnAddOk" runat="server" CssClass="Button_Normal" style="width:85px;" text="Add & Close" />&nbsp;

												<asp:Button ID="btnAddClose" CausesValidation="false" runat="server" CssClass="Button_Normal" style="width:80px;" text="Close" />
													
												</td>
												
											</tr>
								
							</table>
									
								</td>
								</tr>
			</table>
			<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
	</div>	
    </form>
	<%--<script  language="javascript" type="text/javascript" >

		function fnCloseWindow() {
			self.close();
			}
		</script>--%>
</body>
<!--#include virtual="bottom.html"-->
