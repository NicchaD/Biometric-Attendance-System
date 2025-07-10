<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LoanInformation.aspx.vb" Inherits="YMCAUI.LoanInformation" MasterPageFile="~/MasterPages/YRSMain.Master" %>
<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl.ascx" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
<script language="JavaScript">
 javascript:window.history.forward(1);
 function CheckAccess(controlname)
{
var str=String(document.Form1.all.HiddenSecControlName.value);

	if (str.match(controlname)!= null)
	{
		alert("Sorry, You are not authorized to do this activity.");
		return false;
		
	}
	else
	{
	    
	    return true;
		
	}
	
 }

 $(document).ready(function () {
     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
     function EndRequest(sender, args) {
         if (args.get_error() == undefined) {
             BindEvents();
         }
     }
     BindEvents();
 });

 function BindEvents() {
     $('#divConfirmDialog').dialog({
         autoOpen: false,
         resizable: false,
         draggable: true,
         closeOnEscape: false,
         close: false,
         width: 400, height: 260,         
         modal: true,         
         open: function (type, data) {
             $(this).parent().appendTo("form");
             $('a.ui-dialog-titlebar-close').remove();
         }
     });
 }

 function showDialog(text,title) {
     $('#lblMessage').text(text);
     $('#divConfirmDialog').dialog({ title: title });
     $('#divConfirmDialog').dialog("open");     
 }

 function closeDialog() {
     $('#divConfirmDialog').dialog('close');
 }

</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server" ID="MAincontenctHolderLoan">
    <asp:ScriptManagerProxy id="dbScriptManagerProxy" runat="server"> 
    </asp:ScriptManagerProxy>
	<div class="Div_Center" >
  
		<table width="100%" border="0" >
			<!--<tr>
				<td class="Td_HeadingFormContainer" align="left">
					<asp:Label id="LabelHeader" cssclass="Td_HeadingFormContainer" runat="server">Tax Deferred Loan Request Processing</asp:Label>
				</td>
			</tr>-->
            <!--<tr>
				<td class="td_Text_Small" align="left" height="18">
					<asp:label id="LabelTitle" runat="server" cssClass="td_Text_Small"></asp:label></td>
			</tr>-->
         
			<tr>
				<td style="width:100%;"><iewc:tabstrip id="LoansTabStrip" runat="server" Width="100%" Height="30px" TabSelectedStyle="background-color:#93BEEE;color:#000000;text-align:left;"
						TabHoverStyle="background-color:#93BEEE;color:#4172A9;text-align:left;" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:left;border-bottom:none"
						AutoPostBack="True">
						<%--<iewc:Tab Text="List"></iewc:Tab>--%>
						<iewc:Tab Text="General" DefaultStyle="border:solid 1px White;border-left:none;" HoverStyle="border:solid 1px White;border-left:none;" SelectedStyle="border:solid 1px White;border-left:none;"></iewc:Tab>
						<iewc:Tab Text="Employment" DefaultStyle="border:solid 1px White" HoverStyle="border:solid 1px White" SelectedStyle="border:solid 1px White"></iewc:Tab>
						<iewc:Tab Text="Account Contributions" DefaultStyle="width:145;border:solid 1px White" HoverStyle="width:145;border:solid 1px White" SelectedStyle="width:145;border:solid 1px White"></iewc:Tab>
						<iewc:Tab Text="Loans" DefaultStyle="border:solid 1px White" HoverStyle="border:solid 1px White" SelectedStyle="border:solid 1px White"></iewc:Tab>
						<iewc:Tab Text="Notes" DefaultStyle="border:solid 1px White;border-right:none;" HoverStyle="border:solid 1px White;border-right:none;" SelectedStyle="border:solid 1px White;border-right:none;"></iewc:Tab>
					</iewc:tabstrip></td>
			</tr>
<%--			<tr>
				<td style="width:100%;"></td>
			</tr>--%>
		</table>
		<iewc:multipage id="LoansMultiPage" Width="100%" runat="server">
			<%--<iewc:pageview>
				<table width="100%" class="Table_WithBorder">
					<TR>
						<TD align="left" class="td_Text" colspan="3">
							&nbsp;List of Members
						</TD>
					</TR>
					<tr>
						<td>&nbsp;
						</td>
					</tr>
					<tr>
						<td>
							<div style="OVERFLOW: auto; WIDTH: 460px; HEIGHT: 250px">
								<asp:Label ID="LabelNotFound" Runat="server" CssClass="Label_Small" width="120" visible="false"
									align="left">No record(s) found</asp:Label>
								<asp:DataGrid id="DataGridList" runat="server" Width="440" CssClass="DataGrid_Grid" allowsorting="true">
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
										<asp:Label ID="LabelListSSNo" Runat="server" CssClass="Label_Small" width="120">SS No.</asp:Label></td>
									<td align="left">
										<asp:TextBox id="TextBoxListSSNo" runat="server" CssClass="TextBox_Normal" width="120"></asp:TextBox>
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
								</TR>
							</table>
							<table align="center">
								<tr>
									<td align="left">
										<asp:Button id="ButtonFind" runat="server" Text="Find" Width="80" CssClass="Button_Normal" CausesValidation="False"></asp:Button>
										&nbsp;&nbsp;&nbsp;&nbsp;
									</td>
									<td align="left">
										<asp:Button id="ButtonClear" runat="server" Text="Clear" Width="80" CssClass="Button_Normal" CausesValidation="False"></asp:Button>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</iewc:pageview>--%>
			<iewc:pageview>
				<table width="100%" class="Table_WithBorder" style="height:450px;">
					<tr>
						<td colspan="2" align="left" class="td_Text" >
							&nbsp;General
						</td>
					</tr>
					<tr valign="top">
						<td align="left" width="300">
							<table width="100%" align="left">
								<tr>
									
									<td align="left">
										<asp:label id="LabelSSNo" runat="server" CssClass="Label_Small" >
															SS No.</asp:label>
                                    </td>
                                    <td align="left">
										<asp:textbox id="TextBoxSSNo" runat="server" Width="70px" CssClass="TextBox_Normal"></asp:textbox>
                                     </td>
                                </tr>
                                <tr>
									<td align="left">
										<asp:label id="LabelFundNo" runat="server" CssClass="Label_Small">
															Fund No.</asp:label></td>
                                    <td align="left">
										<asp:textbox id="TextBoxFundNo" runat="server" Width="70px" CssClass="TextBox_Normal"></asp:textbox></td>
                                </tr>
                                <tr>
                                    <td align="left">
										<asp:label id="LabelSal" runat="server" CssClass="Label_Small" width="30">
															Sal</asp:label></td>
                                        <td align="left">
										<asp:dropdownlist id="DropDownListSal" runat="server" Width="50" CssClass="DropDown_Normal">
											<asp:ListItem Value=" "></asp:ListItem>
											<asp:ListItem Value="Dr.">Dr.</asp:ListItem>
											<asp:ListItem Value="Mr.">Mr.</asp:ListItem>
											<asp:ListItem Value="Mrs.">Mrs.</asp:ListItem>
											<asp:ListItem Value="Ms.">Ms.</asp:ListItem>
										</asp:dropdownlist>
									</td>
								</tr>
                                <tr>
                                    <td align="left">
										<asp:label id="LabelFirst" runat="server" CssClass="Label_Small" width="30">
															First</asp:label></td>
								    <td align="left">
										<asp:textbox id="TextBoxFirst" runat="server" Width="130px" CssClass="TextBox_Normal"></asp:textbox></td>
                                </tr>
                                <tr>
                                	<td align="left">
										<asp:label id="LabelMiddle" runat="server" CssClass="Label_Small" width="30">
															Middle</asp:label></td>
								    <td align="left">
										<asp:textbox id="TextBoxMiddle" runat="server" Width="130px" CssClass="TextBox_Normal"></asp:textbox></td>
                                </tr>
                                <tr>
                                    <td align="left">
										<asp:label id="LabelLast" runat="server" CssClass="Label_Small" width="30">
															Last</asp:label></td>
								    <td align="left">
										<asp:textbox id="TextBoxLast" runat="server" Width="130px" CssClass="TextBox_Normal"></asp:textbox></td>
                                </tr>
                                <tr>
                                    <td align="left">
										<asp:label id="LabelSuffix" runat="server" CssClass="Label_Small" width="30">
															Suffix</asp:label></td>
								    <td align="left">
										<asp:textbox id="TextBoxSuffix" runat="server" Width="130px" CssClass="TextBox_Normal"></asp:textbox></td>
                                </tr>
                                <tr>	    
										<td align="left">
											<asp:label id="LabelMaritalStatus" runat="server" CssClass="Label_Small">
													Marital Status</asp:label>
                                         </td>
                                         <td align="left">
										        <asp:textbox id="TextBoxMaritalStatus" runat="server" Width="130px" CssClass="TextBox_Normal"></asp:textbox>
                                         </td>
								</tr>
                                <tr>
                                        <td align="left">
											<asp:label id="LabelAge" runat="server" CssClass="Label_Small" >
													Age</asp:label>
                                        </td>
                                         <td align="left">
										    <asp:textbox id="TextBoxAge" runat="server" Width="60px" CssClass="TextBox_Normal"></asp:textbox>
                                        </td>
								</tr>
                                <tr>					    
										<td align="left">
											<asp:label id="LabelVested1" runat="server" CssClass="Label_Small" >
													Vested</asp:label>
                                        </td>
                                        <td align="left">
                                            <asp:textbox id="TextBoxVested1" runat="server" Width="60px" CssClass="TextBox_Normal"></asp:textbox>
                                         </td>
								</tr>
                                <tr>					    
										<td align="left">
											<asp:label id="LabelTerminated" runat="server" CssClass="Label_Small" >
													Terminated</asp:label>
                                        </td>
                                        <td align="left">
										    <asp:textbox id="TextBoxTerminated" runat="server" Width="60px" CssClass="TextBox_Normal"></asp:textbox>
                                        </td>
													    
                                </tr>
								
								
						    </table>
                            </td>
                        <td align="left" style="width:70%">
                            <table align="left" style="width:100%" >
                                <tr style="vertical-align:top;" >
                                    <td align="left" style="width:20%">
										<Label class="Label_Small">Address</Label>
                                    </td>
                                    <td align="left" rowspan="2">
                                        <NewYRSControls:New_AddressWebUserControl runat="server" popupheight="930" ID="AddressWebUserControl1" AllowNote="true" AllowEffDate="true" ClientIDMode="Predictable" />
                                     </td>
                                </tr>
                                <tr style="vertical-align:bottom; margin-top:2px; ">
                                    <td align="left">
										<Label class="Label_Small">Effective Date</Label>
                                    </td>
                                </tr>
								    <%--<tr>
									    <td align="right">
										    <asp:label id="LabelAddress1" runat="server" CssClass="Label_Small" width="80">
															    Address1</asp:label></td>
									    <td colSpan="4">
										    <asp:textbox id="TextBoxAddress1" runat="server" Width="300px" CssClass="TextBox_Normal"></asp:textbox></td>
								    </tr>
								    <tr>
									    <td align="right">
										    <asp:label id="LabelAddress2" runat="server" CssClass="Label_Small" width="80">
															    Address2</asp:label></td>
									    <td colSpan="4">
										    <asp:textbox id="TextBoxAddress2" runat="server" Width="300px" CssClass="TextBox_Normal"></asp:textbox></td>
								    </tr>
								    <tr>
									    <td align="right">
										    <asp:label id="LabelAddress3" runat="server" CssClass="Label_Small" width="80">
															    Address3</asp:label></td>
									    <td colSpan="4">
										    <asp:textbox id="TextBoxAddress3" runat="server" Width="300px" CssClass="TextBox_Normal"></asp:textbox>
									    </td>
								    </tr>
								    <tr>
									    <td align="right">
										    <asp:label id="LabelCity" runat="server" CssClass="Label_Small" width="76">
															    City</asp:label></td>
									    <td colSpan="2">
										    <asp:textbox id="TextBoxCity" runat="server" Width="140px" CssClass="TextBox_Normal"></asp:textbox></td>
									    <td align="right">
										    <asp:label id="LabelZip" runat="server" CssClass="Label_Small" width="80">
															    Zip</asp:label></td>
									    <td align="left">
										    <asp:textbox id="TextBoxZip" runat="server" Width="100px" CssClass="TextBox_Normal"></asp:textbox></td>
								    </tr>
								    <tr>
									    <td align="right">
										    <asp:label id="LabelState" runat="server" CssClass="Label_Small" width="80">State</asp:label></td>
									    <td colSpan="4" align="left">
										    <asp:textbox id="TextBoxState" runat="server" Width="180px" CssClass="TextBox_Normal"></asp:textbox></td>
								    </tr>
								    <tr>
									    <td align="right">
										    <asp:label id="LabelCountry" runat="server" CssClass="Label_Small" width="80">
															    Country</asp:label></td>
									    <td colSpan="4" align="left">
										    <asp:textbox id="TextBoxCountry" runat="server" Width="180px" CssClass="TextBox_Normal"></asp:textbox></td>
								    </tr>--%>
								<tr>
									<td align="left">
										<asp:label id="LabelTelephone" runat="server" CssClass="Label_Small" width="80">
															Telephone</asp:label>
                                    </td>
									<td align="left">
										<asp:textbox id="TextBoxTelephone" runat="server" Width="180" CssClass="TextBox_Normal"></asp:textbox>
                                    </td>
								</tr>
								<tr>
									<td align="left">
										<asp:label id="LabelEmail" runat="server" Width="80" CssClass="Label_Small">
															Email Addr.</asp:label>
                                    </td>
									<td align="left">
										<asp:textbox id="TextBoxEmail" runat="server" Width="180" CssClass="TextBox_Normal"></asp:textbox>
                                     </td>
								</tr>
								<tr>	
									<td align="left" >
										<asp:label id="LabelUpdatedDate" runat="server" CssClass="Label_Small" >
															Updated Date</asp:label>
                                    </td>
									<td align="left">
										<asp:textbox id="TextBoxUpdatedDate" runat="server" Width="140px" CssClass="TextBox_Normal"></asp:textbox>
                                    </td>
								</tr>
                                <tr>
                                    <td align="left">
										<asp:label id="LabelUpdatedBy" runat="server" CssClass="Label_Small" width="80">
															Updated By</asp:label></td>
									<td align="left">
										<asp:textbox id="TextBoxUpdatedBy" runat="server" Width="140px" CssClass="TextBox_Normal"></asp:textbox>
                                     </td>
                            </tr>
                            </table>
                        </td>
                     </tr>
              							
				</table>
			</iewc:pageview>
			<iewc:pageview>
				<table width="100%" class="Table_WithBorder" style="height:450px;">
					<TR style="vertical-align:top;">
						<TD align="left" class="td_Text" colspan="4">
							&nbsp;Employment
						</TD>
					</TR>
					<tr>
						<td>
							&nbsp;
						</td>
					</tr>
					<tr>
					
					<tr style="vertical-align:top;">
						<td>
							<table align="left" width="60%">
								<tr style="vertical-align:top;">
									<td align="right">
										<asp:label id="LabelVested" runat="server" CssClass="Label_Small">Vested</asp:label>
										<asp:textbox id="TextBoxVested" runat="server" CssClass="TextBox_Normal" width="50"></asp:textbox>
									</td>
									<td align="right">
										<asp:label id="LabelYear" runat="server" CssClass="Label_Small">Service  :  Year</asp:label>
										<asp:textbox id="TextBoxYear" runat="server" CssClass="TextBox_Normal" width="50"></asp:textbox>
									</td>
									<td align="right">
										<asp:label id="LabelMonth" runat="server" CssClass="Label_Small">Month</asp:label>
										<asp:textbox id="TextBoxMonth" runat="server" CssClass="TextBox_Normal" width="50"></asp:textbox>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td class="td_Text" align="left" height="0"></td>
					</tr>
					<tr style="vertical-align:top;">
						<td>
							<div style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 400px">
                                <asp:GridView ID="gvEmployment" runat="server" Width="100%" CssClass="DataGrid_Grid" AutoGenerateColumns="false">
                                    
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
									<RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
                                    <Columns>
                                        <asp:BoundField HeaderText="Hire Date" DataField="HireDate" ItemStyle-Width="50px" HeaderStyle-Width="60px" />
                                        <asp:BoundField HeaderText="Term Date" DataField="TermDate" HeaderStyle-Width="65px" />
                                        <asp:BoundField HeaderText="Eligibility Date" DataField="EligibilityDate" HeaderStyle-Width="85px" />
                                        <asp:BoundField HeaderText="Status Description" DataField="StatusDescription" HeaderStyle-Width="115px" />
                                        <asp:BoundField HeaderText="YMCA Name" DataField="YMCAName" />
                                        <asp:BoundField HeaderText="YMCA No" DataField="YMCANo" HeaderStyle-Width="50px" />
                                    </Columns>
                                </asp:GridView>
                                
							</div>
						</td>
					</tr>
				</table>
			</iewc:pageview>
			<iewc:pageview>
				<table width="100%" class="Table_WithBorder" style="height:450px;">
					<TR style="vertical-align:top;">
						<TD align="left" class="td_Text" colspan="4">&nbsp;Account Contributions
						</TD>
					</TR>
					<tr>
						<td>&nbsp;</td>
					</tr>
					
					<tr style="vertical-align:top;">
						<td align="left">
							<table width="690">
								<tr>
									<td align="left">
										<asp:label id="LabelPIATermination" runat="server" CssClass="Label_Small">PIA At Termination</asp:label></td>
									<td align="left">
										<asp:textbox id="TextBoxPIATermination" runat="server" CssClass="TextBox_Normal" style="text-align:right;"></asp:textbox></td>
									<td align="left">
										<asp:label id="LabelCurrentPIA" runat="server" CssClass="Label_Small">Current PIA</asp:label></td>
									<td align="left">
										<asp:textbox id="TextBoxCurrentPIA" runat="server" CssClass="TextBox_Normal" style="text-align:right;"></asp:textbox></td>
								</tr>
							</table>
						</td>
					</tr>
					
					
					<TR style="vertical-align:top;">
						<TD align="left" class="td_Text_Small" colspan="4">&nbsp;Funded Contributions</TD>
					</TR>
					<!-- <tr>
									<td>
										<asp:label id="LabelEmployee" runat="server" CssClass="Label_Small">---------------Employee---------------</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:label id="LabelYMCA" runat="server" CssClass="Label_Small">---------------YMCA---------------</asp:label></td>
								</tr> -->
					<tr style="vertical-align:top;">
						<td align="right">
							<div style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 150px">
                                <asp:GridView ID="gvFundedAccountContributions" runat="server" Width="100%" CssClass="DataGrid_Grid" AutoGenerateColumns="false">
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingRowStyle>
									<RowStyle CssClass="DataGrid_NormalStyle_temp"></RowStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<SelectedRowStyle CssClass="DataGrid_SelectedItem_Total"></SelectedRowStyle>
                                    <Columns>
                                        <asp:BoundField HeaderText ="Account Type" DataField="AccountType" />
                                        <asp:BoundField HeaderText ="Taxable" DataField="Taxable" />
                                        <asp:BoundField HeaderText ="Non-Taxable" DataField="Non-Taxable" />
                                        <asp:BoundField HeaderText ="Interest" DataField="Interest" />
                                        <asp:BoundField HeaderText ="Emp.Total" DataField="Emp.Total" />
                                        <asp:BoundField HeaderText ="YMCA Taxable" DataField="YMCATaxable" />
                                        <asp:BoundField HeaderText ="YMCA Interest" DataField="YMCAInterest" />
                                        <asp:BoundField HeaderText ="YMCA Total" DataField="YMCATotal" />
                                        <asp:BoundField HeaderText ="Total" DataField="Total" />
                                    </Columns>
                                </asp:GridView>
							</div>
                            
						</td>
					</tr>
					<tr>
						<td>
							&nbsp;
						</td>
					</tr>
					<TR>
						<TD align="left" class="td_Text_Small" colspan="4">
							&nbsp; Non - Funded Contributions
						</TD>
					</TR>
					<!-- <tr>
									<td>
										<asp:label id="LabelNonFundedEmployee" runat="server" CssClass="Label_Small">---------------Employee---------------</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:label id="LabelNonFundedYMCA" runat="server" CssClass="Label_Small">---------------YMCA---------------</asp:label></td>
								</tr> -->
					<tr style="vertical-align:top;">
						<td align="right">
							<div style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 150px">
								
                                <asp:GridView ID="gvNonFundedAccountContributions" runat="server" Width="100%" CssClass="DataGrid_Grid" AutoGenerateColumns="false">
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingRowStyle>
									<RowStyle CssClass="DataGrid_NormalStyle_temp"></RowStyle>
									<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									<SelectedRowStyle CssClass="DataGrid_SelectedItem_Total"></SelectedRowStyle>
                                    <Columns>
                                        <asp:BoundField HeaderText ="Account Type" DataField="AccountType" />
                                        <asp:BoundField HeaderText ="Taxable" DataField="Taxable" />
                                        <asp:BoundField HeaderText ="Non-Taxable" DataField="Non-Taxable" />
                                        <asp:BoundField HeaderText ="Interest" DataField="Interest" />
                                        <asp:BoundField HeaderText ="Emp.Total" DataField="Emp.Total" />
                                        <asp:BoundField HeaderText ="YMCA Taxable" DataField="YMCATaxable" />
                                        <asp:BoundField HeaderText ="YMCA Interest" DataField="YMCAInterest" />
                                        <asp:BoundField HeaderText ="YMCA Total" DataField="YMCATotal" />
                                        <asp:BoundField HeaderText ="Total" DataField="Total" />
                                    </Columns>
                                </asp:GridView>
							</div>
						</td>
					</tr>
				</table>
			</iewc:pageview>
			<iewc:pageview>
				<table width="100%" class="Table_WithBorder" style="height:450px;">
					<TR style="vertical-align:top;">
						<TD align="left" class="td_Text" colspan="4">
							&nbsp;Loans
						</TD>
					</TR>

					<tr style="vertical-align:top;display:none">
						<td align="right" class="Td_ButtonContainer" colspan="4">
							<asp:button class="Button_Normal" id="ButtonLoanOptions" Width="110" Runat="server" Text="Loan Options..."
								visible="false"></asp:button>
							&nbsp;&nbsp;
							<asp:button class="Button_Normal" id="ButtonAddItem" Width="100" Runat="server" Text="Add Item"
								visible="false" CausesValidation="False"></asp:button>
						</td>
					</tr>
					<tr style="vertical-align:top;">
						<td colspan="2">
							<div style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 350px">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                                    <ContentTemplate>
                                        <asp:GridView ID="gvLoans" runat="server" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                            <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
									        <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
									        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
									        <SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
									        <Columns>                                                
										        <asp:TemplateField>
											        <ItemTemplate>
												        <asp:ImageButton id="ImagebuttonProcess" runat="server" ImageUrl="images\process.gif" CausesValidation="False"
													        CommandName="Select" ToolTip="Process a Loan"></asp:ImageButton>
											        </ItemTemplate>
										        </asp:TemplateField>
										        <asp:TemplateField>
											        <ItemTemplate>
												        <asp:ImageButton id="Imagebutton2" runat="server" ImageUrl="images\cancel.gif" CausesValidation="False"
													        CommandName="Cancel" ToolTip="Cancel a Loan"></asp:ImageButton>
											        </ItemTemplate>
										        </asp:TemplateField>
                                                <asp:BoundField HeaderText ="PersId" DataField="PersId" />
                                                <asp:BoundField HeaderText ="EmpEventId" DataField="EmpEventId" />
                                                <asp:BoundField HeaderText ="YmcaId" DataField="YmcaId" />
                                                <asp:BoundField HeaderText ="Loan Number" DataField="LoanNumber" />
                                                <asp:BoundField HeaderText ="TD Balance" DataField="TDBalance" />
                                                <asp:BoundField HeaderText ="Requested Amount" DataField="RequestedAmount" />
                                                <asp:BoundField HeaderText ="Request Date" DataField="RequestDate" />
                                                <asp:BoundField HeaderText ="Request Status" DataField="RequestStatus" />
                                                <asp:BoundField HeaderText ="LoanRequestId" DataField="LoanRequestId" />
                                                <asp:BoundField HeaderText ="Original Loan Number" DataField="OriginalLoanNumber" />                                                
                                                <asp:BoundField HeaderText ="SavedPayOffAmount" DataField="SavedPayOffAmount" />
                                                <asp:BoundField HeaderText ="ComputeOn" DataField="ComputeOn" />
                                                <%-- START: MMR | 2018.04.05 | YRS-AT-3929 | Added columns  --%>
                                                <asp:BoundField HeaderText ="Payment Method" DataField="PaymentMethodCode" />
                                                <asp:BoundField HeaderText ="PersBankingEFTId" DataField="PersBankingEFTId" />
                                                <asp:BoundField HeaderText ="DisbursementEFTStatus" DataField="DisbursementEFTStatus" />
									            <%-- END: MMR | 2018.04.05 | YRS-AT-3929 | Added columns  --%>
                                                <%-- START: MMR | 2018.05.30 | YRS-AT-3936 | Added columns  --%>
                                                <asp:BoundField HeaderText ="ONDType" DataField="ONDType" />
                                                <asp:BoundField HeaderText ="AccountType" DataField="AccountType" />
                                                <asp:BoundField HeaderText ="BankABA" DataField="BankABA" />
                                                <asp:BoundField HeaderText ="BankAccountNumber" DataField="BankAccountNumber" />
                                                <asp:BoundField HeaderText ="BankName" DataField="BankName" />
									            <%-- END: MMR | 2018.05.30 | YRS-AT-3936 | Added columns  --%>
                                                <asp:BoundField HeaderText ="CoolingPeriod" DataField="CoolingPeriod" /> <%-- MMR | 2018.08.07 | YRS-AT-4017 | Set index value of datagrid items --%>
                                                <asp:BoundField DataField="ActualRequestStatus" HeaderText="ActualRequestStatus"  HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"></asp:BoundField><%--VC | 2018.10.11 | YRS-AT-3936 | Added ActualRequestStatus column--%>
                                                <asp:BoundField DataField="ErrorMessage" HeaderText="ErrorMessage"  HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"></asp:BoundField><%--PPP | 2018.11.06 | YRS-AT-3936 | Added ErrorMessage column--%>
									        </Columns>
                                        </asp:GridView>								      

                                    </ContentTemplate>
                                </asp:UpdatePanel>
							</div>
						</td>
					</tr>
				</table>
			</iewc:pageview>
			<iewc:pageview>
				<table width="100%" class="Table_WithBorder" style="height:450px;">
					<TR class="Td_ButtonContainer" style="vertical-align:top;">
						<TD align="left"  >
							&nbsp;Notes
						</TD>
                        <td align="right" >
        	                    <asp:Button id="ButtonAddNote" runat="server" Text="Add..." CssClass="Button_Normal"
		                    autogeneratecolumns="false" CausesValidation="False"></asp:Button>
						</td>
					</TR>
<%--					<tr>
						<td align="right" class="Td_ButtonContainer">
							<asp:Button id="ButtonAddNote" runat="server" Width="110px" Text="Add Item" CssClass="Button_Normal"
								autogeneratecolumns="false" CausesValidation="False"></asp:Button>
						</td>
					</tr>--%>
					<tr style="vertical-align:top;">
						<td colspan="2">
							<div style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 350px">
                                <asp:GridView ID="gvNotes" runat="server" Width="100%" CssClass="DataGrid_Grid" AutoGenerateColumns="false">
                                    <HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
									<AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
									<RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
									<SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
									<columns>
										<asp:TemplateField ItemStyle-Width="20px" >
											<ItemTemplate>
												<asp:ImageButton id="Imagebutton1" runat="server" ImageUrl="images\view.gif" CausesValidation="False"
													CommandName="Select" ToolTip=" View Notes "></asp:ImageButton>
											</ItemTemplate>
										</asp:TemplateField>
										<asp:boundField headertext="Notes" datafield="Note" />
										<asp:boundField headertext="Date" datafield="Date" DataFormatString="{0:MM/dd/yyyy}"  ItemStyle-Width="65px" />
										<asp:boundField headertext="Creator" datafield="Creator" ItemStyle-Width="55px" />
										<asp:boundField headertext="UniqueId" datafield="UniqueID" visible="false" />
										<asp:TemplateField HeaderText="Marked As Important" ItemStyle-Width="125px">
											<ItemTemplate>
												<asp:CheckBox ID="CheckBoxImportant" Runat=server AutoPostBack=False Enabled=False Checked='<%# Databinder.Eval(Container.DataItem, "bitImportant") %>'>
												</asp:CheckBox>
											</ItemTemplate>
										</asp:TemplateField>
									</columns>
                                </asp:GridView>								
							</div>
						</td>
					</tr>
					<tr>
						<td colspan="2">&nbsp;</td>
					</tr>
				</table>
			</iewc:pageview>
		</iewc:multipage>
		<table class="Table_WithoutBorder" cellSpacing="0" width="100%">
			<TR>
				<td align="right" class="td_Text">
					<asp:Button id="ButtonLoanSummary" runat="server" Text="Loan Summary" Width="150" CssClass="Button_Normal"
						enabled="false" visible="false" CausesValidation="False"></asp:Button>
				</td>
				<td align="right" class="td_Text">
					<asp:Button id="ButtonOk" runat="server" Text="Close" Width="80" CssClass="Button_Normal" CausesValidation="False"></asp:Button>
				</td>
			</TR>
		</table>
<%--        <table width="750">
        <tr>
                <td  width="100%">
                    <YRSControls:YMCA_Footer_WebUserControl ID="YMCA_Footer_WebUserControl1" runat="server">
                    </YRSControls:YMCA_Footer_WebUserControl>
                </td>
            </tr>
        
    </table>--%>
	</div>
    <div id="divConfirmDialog" style="overflow: visible;">
            <asp:UpdatePanel ID="UpdatePanel6" runat="server" >
                <ContentTemplate>
                    <div>
                        <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                            <tr>
                                <td>
                                    <asp:Label ID="lblMessage" CssClass="Label_Small" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="bottom">
                                    <asp:Button runat="server" ID="btnYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px;
                                        color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                        height: 16pt;"  OnClientClick="Javascript: closeDialog();" />&nbsp;
                                    <asp:Button runat="server" ID="btnNo" Text="No" CssClass="Button_Normal" Style="width: 80px;
                                        color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                        height: 16pt;" OnClientClick="Javascript: closeDialog();"/>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate> 
            </asp:UpdatePanel>
        </div>
	<INPUT id="NotesFlag" type="hidden" name="NotesFlag" runat="server"> <INPUT id="HiddenText" type="hidden" name="HiddenText" runat="server">
	<INPUT id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server">
	<asp:PlaceHolder id="MessageBoxPlaceHolder" runat="server"></asp:PlaceHolder>
</asp:Content>
