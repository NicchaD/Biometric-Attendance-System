<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeBehind="DeathBenefitsCalculatorForm.aspx.vb" Inherits="YMCAUI.DeathBenefitsCalculatorForm" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
<asp:scriptmanager enablepartialrendering="true" id="DBScriptManager" runat="server">
    </asp:scriptmanager>
<asp:updateprogress id="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div id="loading">
                <img id="loadingimage" runat="server" src="images/ajax-loader.gif" alt="Loading..." />
            </div>
        </ProgressTemplate>
    </asp:updateprogress>
<table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="700">
    <tr>
        <td class="Td_BackGroundColorMenu" align="left">
            <cc1:Menu ID="Menu1" runat="server" mouseovercssclass="MouseOver" MenuFadeDelay="2"
                DefaultMouseUpCssClass="mouseup" DefaultMouseOverCssClass="mouseover" DefaultMouseDownCssClass="mousedown"
                DefaultCssClass="menuitem" CssClass="menustyle" Cursor="Pointer" HighlightTopMenu="False"
                Layout="Horizontal">
                <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
            </cc1:Menu>
        </td>
    </tr>
    <%--<tr>
			<td class="Td_HeadingFormContainer" align="left">Death Benefits Calculator
				<asp:label id="LabelTitle" runat="server" Width="432px"></asp:label></td>
		</tr>--%>
    <tr>
        <td class="Td_HeadingFormContainer" align="left">
            <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
        </td>
    </tr>
    <tr>
        <td height="5">
        </td>
    </tr>
</table>
<div class="Div_Center">
    <table cellspacing="0" cellpadding="0" width="700">
        <tr>
            <td>
                <iewc:TabStrip ID="TabStripDeathCalc" runat="server" Width="700" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
                    TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
                    AutoPostBack="True" Height="30px">
                    <iewc:Tab Text="List"></iewc:Tab>
                    <iewc:Tab Text="Details"></iewc:Tab>
                </iewc:TabStrip>
            </td>
        </tr>
        <tr>
            <td>
                <iewc:MultiPage ID="MultiPageDeathCalc" runat="server">
                    <iewc:PageView>
                        <div class="Div_Center">
                            <table class="Table_WithBorder" width="700">
                                <tr>
                                    <td>
                                        <table width="680">
                                            <tr>
                                                <td align="left">
                                                    <asp:label id="LabelNoDataFound" runat="server" visible="False">No Matching Records</asp:label>
                                                    <div style="overflow: auto; width: 430px; height: 200px; text-align: left">
                                                        <asp:datagrid id="DataGrid_Search" runat="server" width="414" cssclass="DataGrid_Grid"
                                                            allowsorting="True" enableviewstate="False" allowpaging="False" pagesize="500">
																<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																<selectedItemStyle cssClass="DataGrid_SelectedStyle"></selectedItemStyle>
																<Columns>
																	<asp:TemplateColumn>
																		<ItemTemplate>
																			<asp:ImageButton id="imgbtn" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																				CommandName="Select" ToolTip="Select" EnableViewState="false"></asp:ImageButton>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																</Columns>
															</asp:datagrid>
                                                        <asp:label id="lbl_Search_MoreItems" runat="server" cssclass="Message" visible="False"
                                                            enableviewstate="False" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <span cssclass="Label_Small">Fund No.</span>
                                                            </td>
                                                            <td align="center">
                                                                <asp:textbox id="TextBoxFundNo" runat="server" width="150" cssclass="TextBox_Normal"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span cssclass="Label_Small">SS No.</span>
                                                            </td>
                                                            <td align="center">
                                                                <asp:textbox id="TextBoxSSNo" runat="server" width="150" cssclass="TextBox_Normal"></asp:textbox>
                                                                <asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" errormessage="Invalid SSN"
                                                                    controltovalidate="TextBoxSSNo" validationexpression="\d{3}-\d{2}-\d{4}|\d{9}|[A-Z]\d{8}"
                                                                    enabled="False" visible="false"></asp:regularexpressionvalidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span cssclass="Label_Small">Last Name</span>
                                                            </td>
                                                            <td align="center">
                                                                <asp:textbox id="TextBoxLastName" runat="server" width="150" cssclass="TextBox_Normal"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span cssclass="Label_Small">First Name</span>
                                                            </td>
                                                            <td align="center">
                                                                <asp:textbox id="TextBoxFirstName" runat="server" width="150" cssclass="TextBox_Normal"></asp:textbox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:button id="ButtonFind" runat="server" text="Find" width="80" cssclass="Button_Normal"></asp:button>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                            </td>
                                                            <td align="right">
                                                                <asp:button id="ButtonClear" runat="server" text="Clear" width="80" cssclass="Button_Normal"></asp:button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="680">
                                            <tr>
                                                <td class="Td_ButtonContainer" align="right">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:button id="ButtonSelect" runat="server" text="Select" width="80" cssclass="Button_Normal"></asp:button>
                                                </td>
                                                <td align="right" class="Td_ButtonContainer">
                                                    <asp:button id="ButtonCancel" runat="server" text="Cancel" width="80" cssclass="Button_Normal"></asp:button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </iewc:PageView>
                    <iewc:PageView>
                        <table class="Table_WithBorder" width="700">
                            <tr>
                                <td>
                                    <asp:updatepanel id="UpdatePanel1" runat="server" updatemode="Conditional">
                                     <ContentTemplate>
										<table width="690">
											<%--<tr>
												<td colspan="2">
													<asp:Panel id="pnlMoneySelection" runat="server" visible = "false">
														<table>
															<tr>
																<td><span CssClass="Label_Small">Perform Calculations for: </span></td>
																<td valign="top">
																	<asp:RadioButtonList id="rbtnList_MoneySelection" runat="server" RepeatLayout="flow" RepeatDirection="Horizontal"
																		CssClass="RadioButton_Normal" AutoPostBack="true">
																		<asp:ListItem Value="Retired">Retired Money</asp:ListItem>
																		<asp:ListItem Value="Active">Active Money</asp:ListItem>
																	</asp:RadioButtonList>
																</td>
															</tr>
														</table>
													</asp:Panel>
												</td>
											</tr>--%>
											<tr>
												<br />
                                                <td colspan="2" class="Table_WithBorder" align="left">
													<asp:Label id="lblErr_BeneficiarySettled" runat="server" Visible="False" CssClass="Error_Message"></asp:Label>
													<asp:Label id="lblErr_BeneficiariesNotDefinedProperly" runat="server" Visible="False" CssClass="Error_Message"></asp:Label>
                                                    <asp:Label id="lblInformation" runat="server" Visible="False" CssClass="Error_Message"></asp:Label> <%-- SR | 2015.12.15 | YRS-AT-2718 - Add label to display information messages --%>
												</td>
											</tr>                                          
                                            
											<tr>
                                            
												<td width="100%" align="center" >
                                                <br />
													<table width="100%">
														<tr>
															<td align="left">
																<asp:Label id="LabelDatabaseDeath" runat="server" Width="200" CssClass="Label_Small">Date of Death From Database</asp:Label></td>
															<td align="left">
																<asp:TextBox id="TextBoxDatabaseDeathDate" runat="server" Width="70px" CssClass="TextBox_Normal"></asp:TextBox></td>
														</tr><tr>
															<td align="left">
																<asp:Label id="LabelDeathCalculations" runat="server" Width="200px" CssClass="Label_Small">Date of Death for Calculations</asp:Label></td>
															<td align="left">
																<!-- Replacing Read only date Controls .. 14Nov2005
																<asp:TextBox id="TextBoxCalcDeathDateOLD" readonly="True" runat="server" Width="70px" CssClass="TextBox_Normal"></asp:TextBox>
																<rjs:popcalendar id="PopcalendaCheckDate" runat="server" ScriptsValidators="" Format="mm dd yyyy"
																	Control="TextBoxCalcDeathDateOLD" Separator="/"></rjs:popcalendar>
-->
																<uc1:DateUserControl id="TextBoxCalcDeathDate" runat="server"></uc1:DateUserControl>
															</td>
                                                            <td align="right">
                                                        <table width="100%">
                                                        <tr align="right">
                                                         <%--Anudeep A.2013.01.07 - code commented below for YRS 5.0-1707:New Death Benefit Application form
                                                                <asp:Label id="Label1" runat="server" Width="64px" CssClass="Label_Small">Format</asp:Label>
														        <asp:RadioButton id="RadioButtonMonthly" runat="server" Text="Monthly" Checked="True" CssClass="RadioButton_Normal"
															        width="90" GroupName="Type" AutoPostBack="True"></asp:RadioButton>--%>
                                                            <td>
														        <asp:Button id="ButtonCalculate" runat="server" Width="80px" Text="Calculate" CssClass="Button_Normal"></asp:Button></td>
													        <td> &nbsp;
														        <asp:Button id="ButtonReports" runat="server" Width="80px" Text="Reports" CssClass="Button_Normal"></asp:Button></td>
												        <%--</tr>
												        <tr>--%>
													        <!--	<td align="left">
														        <asp:RadioButton id="RadioButtonAnnual" runat="server" Text="Annual" CssClass="RadioButton_Normal"
															        width="90" GroupName="Type" AutoPostBack="True" visible="false"></asp:RadioButton></td>-->
													        <td > &nbsp;
														        <!--Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up  -->
                                                                <input type="button" id="ButtonForms" runat="server" onclick="Formclick()"  style="width:80px;" Value="Forms" class="Button_Normal" /></td>
                                                        </tr>													
													</table>
                                                   </td>
                                                            </tr>
                                                        </table>
                                                        </td>
                                                        </tr>
                                            
											        <tr>
                                                        

												
											</tr>
											<tr>
												<td colspan="2"><br>
												</td>
											</tr>
                                  
											
                                 
                                        <!--SR:2012.11.19: YRS 5.0 1707: Added -->

                                            <tr>
                                             
                                            <td colspan="2" align="left">   
                                                    <asp:Label id="lbl_JSBeneficiaries" runat="server" class="Label_Small"></asp:Label>                                                   
                                                    <asp:DataGrid ID="DataGridAnnuities" runat="server" Width="680px" CssClass="DataGrid_Grid"
                                                        AutoGenerateColumns="false">
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                        <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                        <Columns>
                                                            <asp:TemplateColumn>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="Imagebutton6" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
                                                                        CommandName="Select" ToolTip="Select"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn HeaderText="SSNo" DataField="Output_txtSSN" />
                                                            <asp:BoundColumn HeaderText="First Name" DataField="Output_txtFirstName" />
                                                            <asp:BoundColumn HeaderText="Last Name" DataField="Output_txtLastName" />
                                                            <asp:BoundColumn HeaderText="Monthly Annuity Amount" DataField="Output_txtTotalMonthlyAnnuityAmount" /> 
                                                            <asp:BoundColumn HeaderText="Annuity Source" DataField="Output_txtAnnuitySource" />                                                                      
                                                            <asp:BoundColumn HeaderText="Annuity Type" DataField="Output_txtAnnuityType" />
                                                            <asp:BoundColumn HeaderText="Plan Type" DataField="Output_txtPlanType" />
                                    <%--                         <asp:BoundColumn HeaderText="Death Benefit" DataField="Output_txtDeathBenefit" />--%>
                                                            <asp:BoundColumn HeaderText="ID" DataField="Output_txtAnnuityID" Visible="false" />
                                                            <asp:BoundColumn HeaderText="AnnuityJointSurvivorsID" DataField="Output_txtJSID" Visible="False" />
                                                            <asp:BoundColumn HeaderText="Due Since (in Days)" HeaderStyle-Width="60px" DataField="Output_DueSince"  />
                                                            <asp:BoundColumn HeaderText="Re_GenerateForm" DataField="Output_Re_GenerateForm" Visible="False" />
                                                            <asp:TemplateColumn HeaderText="View / Print Original Form" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center" >
																<ItemTemplate>
																	<asp:ImageButton id="imgJsForm" runat="server" ImageUrl="images\details.gif" CausesValidation="False"
																		CommandName="RePrint" ToolTip="Select" EnableViewState="false"></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
                                                        </Columns>
                                                    </asp:DataGrid>                                                            
                                            </td>
                                        </tr> 
                                                
                                    <!-- End,SR:2012.11.19: YRS 5.0 1707: Added  -->
                                            <tr>
												<td colspan="2"><br>
												</td>
											</tr>                                            										
									
                                           <!--2012.10.29:SR - YRS 5.0-1707 -->
                                            <tr>
												<td colspan="2" align="left">
													<asp:Label id="lbl_Beneficiaries_All" runat="server" class="Label_Small">List of Beneficiaries:</asp:Label>
													<asp:DataGrid id="DataGrid_BeneficiariesList_ForAll"  AutoGenerateColumns="False"  runat="server" Width="680px" CssClass="DataGrid_Grid">
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<selectedItemStyle cssClass="dataGrid_selectedStyle"></selectedItemStyle>
														<Columns>
															<asp:BoundColumn  DataField="Persid" visible="false" > </asp:BoundColumn >
                                                            <asp:TemplateColumn>
																<ItemTemplate>
																	<asp:ImageButton id="ImagebuttonGrid1" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																		CommandName="Select" ToolTip="Select"></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
                                                            <asp:BoundColumn HeaderText="SSNO" DataField="SSNO" > </asp:BoundColumn >
                                                            <asp:BoundColumn HeaderText="FirstName" DataField="FirstName" > </asp:BoundColumn >
                                                            <asp:BoundColumn HeaderText="LastName" DataField="LastName"> </asp:BoundColumn >
                                                            <asp:BoundColumn HeaderText="RelationShip" DataField="RelationShip"  > </asp:BoundColumn >
                                                            <asp:BoundColumn HeaderText="Member" DataField="Member" > </asp:BoundColumn >
                                                            <asp:BoundColumn HeaderText="Saving" DataField="Saving"  > </asp:BoundColumn >
                                                            <asp:BoundColumn HeaderText="Retire" DataField="Retire"  > </asp:BoundColumn >
                                                            <asp:BoundColumn HeaderText="Insres" DataField="Insres"  > </asp:BoundColumn >
                                                            <asp:BoundColumn HeaderText="Due Since (in Days)" HeaderStyle-Width="60px" DataField="DueSince"  > </asp:BoundColumn >
                                                            <asp:BoundColumn HeaderText="Re-GenerateForm" DataField="Re-GenerateForm" visible="false" > </asp:BoundColumn >
                                                            <asp:TemplateColumn HeaderText="View / Print Original Form" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center" >
																<ItemTemplate>
																	<asp:ImageButton id="imgForm" runat="server" ImageUrl="images\details.gif" CausesValidation="False"
																		CommandName="RePrint" ToolTip="Select" EnableViewState="false"></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>                                                            
														</Columns>
													</asp:DataGrid></td>
											</tr>
                                           <!--2012.10.29:SR - YRS 5.0-1707 -->
											<tr>
												<td colspan="2"><br>
												</td>
											</tr>
                                    <!--2012.10.29:SR - YRS 5.0-1707 -->
                                    		<tr> <!-- 2012.12.17:AA - changed to show retired money and active money separately -->
												<td colspan="2" align="left" id="tdRetire" style="cursor:pointer;" onclick="ToogledRetireDivs()" title="Collapse" class="Td_ButtonContainer" >
                                               <!--'Anudeep A.2013.01.07 - Added for YRS 5.0-1707:New Death Benefit Application form to give image for toggle -->
                                               <table  width="100%"><tr><td> Retired Money   </td><td align="right"> 
                                                      <img src="~/images/collapse.GIF" id="imgRetire" runat="server" />
                                                    </td></tr></table>
                                                    
												</td>
											</tr>
                                            <tr>
												<td colspan="2" align="left">
                                                    <div id="divRetRetired" >
													<asp:label id="lbl_RetirementPlanOptions_ForRetired" runat="server"  class="Label_Small" >List of Options under the Retirement Plan</asp:label>
													<asp:DataGrid id="DataGrid_RetirementPlan_BenefitOptions_ForRetired" runat="server" Width="680px" CssClass="DataGrid_Grid">
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
														<selectedItemStyle cssClass="DataGrid_SelectedStyle_temp"></selectedItemStyle>
														<Columns>
															<asp:TemplateColumn Visible="False">
																<ItemTemplate>
																	<asp:ImageButton id="Imagebutton1" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																		autopostback="true" CommandName="Select" ToolTip="Select" ></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:DataGrid>
                                                    </div>
                                                    </td>
											</tr>
											<tr>
												<td colspan="2"><br>
												</td>
											</tr>
											<tr>
												<td colspan="2" align="left">
                                                <div id="divSavRetired">
													<asp:label id="lbl_SavingsPlanOptions_ForRetired" runat="server"  class="Label_Small" >List of Options under the Savings Plan</asp:label>
													<asp:DataGrid id="DataGrid_SavingsPlan_BenefitOptions_ForRetired" runat="server" Width="680px" CssClass="DataGrid_Grid">
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
														<selectedItemStyle cssClass="DataGrid_SelectedStyle_temp"></selectedItemStyle>
														<Columns>
															<asp:TemplateColumn Visible="False">
																<ItemTemplate>
																	<asp:ImageButton id="Imagebutton1" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																		autopostback="true" CommandName="Select" ToolTip="Select" ></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:DataGrid>
                                                    </div>
                                                    </td>
											</tr>
											<tr>
												<td colspan="2"><br>
												</td>
											</tr>
                                            <tr>
                                            <!-- 2012.12.17:AA - changed to show retired money and active money separately -->
												<td colspan="2" align="left" id="tdActive" title="collapse" style="cursor:pointer;"  onclick ="ToggledActiveDivs()" class="Td_ButtonContainer">
                                                <!--'Anudeep A.2013.01.07 - Added for YRS 5.0-1707:New Death Benefit Application form to give image for toggle -->
                                                    <table  width="100%"><tr><td> Active Money </td><td align="right"> 
                                                     <img src="~/images/collapse.GIF" id="imgActive" runat="server" /> 
												    </td></tr></table>
                                                </td>
											</tr>
                                            
                                            <tr>
                                                
												<td colspan="2" align="left">
                                                    <div id="divRetActive">
													<asp:label id="lbl_RetirementPlanOptions_ForActive" runat="server"  class="Label_Small" >List of Options under the Retirement Plan</asp:label>
													<asp:DataGrid id="DataGrid_RetirementPlan_BenefitOptions_ForActive" runat="server" Width="680px" CssClass="DataGrid_Grid">
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
														<selectedItemStyle cssClass="DataGrid_SelectedStyle_temp"></selectedItemStyle>
														<Columns>
															<asp:TemplateColumn Visible="False">
																<ItemTemplate>
																	<asp:ImageButton id="Imagebutton1" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																		autopostback="true" CommandName="Select" ToolTip="Select" ></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:DataGrid>
                                                    </div>
                                                    </td>
											</tr>
											<tr>
												<td colspan="2"><br>
												</td>
											</tr>
											<tr>
												<td colspan="2" align="left">
                                                 <div id="divSavActive">
													<asp:label id="lbl_SavingsPlanOptions_ForActive" runat="server"  class="Label_Small" >List of Options under the Savings Plan</asp:label>
													<asp:DataGrid id="DataGrid_SavingsPlan_BenefitOptions_ForActive" runat="server" Width="680px" CssClass="DataGrid_Grid">
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
														<selectedItemStyle cssClass="DataGrid_SelectedStyle_temp"></selectedItemStyle>
														<Columns>
															<asp:TemplateColumn Visible="False">
																<ItemTemplate>
																	<asp:ImageButton id="Imagebutton1" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																		autopostback="true" CommandName="Select" ToolTip="Select" ></asp:ImageButton>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:DataGrid>
                                                    </div>
                                                    </td>
											</tr>
											<tr>
												<td colspan="2"><br>
												</td>
											</tr>
                                            
                                    
                                    	
                                  <!-- Ends, 2012.10.29:Sr- YRS 5.0-1707-->
										</table>
                                      </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ButtonCalculate" EventName="Click" />
                                           <asp:PostBackTrigger ControlID ="ButtonReports" />
                                            
										     <asp:PostBackTrigger ControlID="DataGrid_BeneficiariesList_ForAll"  />
                                             <asp:PostBackTrigger ControlID="DataGridAnnuities"/>
                                        </Triggers>
                                    </asp:updatepanel>
                                    <table width="100%" align="right">
                                        <tr>
                                            <td align="right" class="Td_ButtonContainer" width="100%">
                                                <asp:button align="right" id="ButtonOk" runat="server" text="Ok" width="80" cssclass="Button_Normal"></asp:button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </iewc:PageView>
                </iewc:MultiPage>
            </td>
        </tr>
    </table>
</div>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
<!-- Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up -->
<div id="dvForm" runat="server" title="Forms" style="display: none;">
    <table runat="server" id="tbForms">
    <%--<tr>
    <td><asp:checkbox id="chkIDM" runat="server" cssclass="CheckBox_Normal" text="Send to IDM"></asp:checkbox></td>    
    </tr> 
    <tr>
    <td><asp:checkbox id="chkFollowUp" runat="server" cssclass="CheckBox_Normal" text="Follow-up Letters"></asp:checkbox></td>
    </tr> 
    <tr>
    <td>Death Benefit Application - Required Forms or Additional Documents</td>    
    </tr> --%>
    </table>
</div>
<div id="ConfirmDialog" title="DeathBenefit" style="overflow: visible;">
    <label id="lblMessage" class="Label_Small" runat="server">
    </label>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        function EndRequest(sender, args) {
            if (args.get_error() == undefined) {

            }


        }


//        $('#ButtonForms').click(function () {
//            Formclick();
//        });


    });

    function Formclick() {

        if (Date.parse($("#TextBoxCalcDeathDate_TextBoxUCDate").val()) < Date.parse("7/1/2006")) {
            showDialog("Invalid Operation, Report cannot be shown for deceased person before 1 July 2006.", 'OK');
            return;
        }
        return BeneFiciaryAddress();      
    }

    function InitializeFormDialogBox() {
        $('#dvForm').dialog({
            modal: true,
            autoOpen: false,
            draggable: true,
            width: 710, height: 410,
            title: "Death Benefit Application Form",
            buttons: [{ text: "Show Form", id: "Form", click: ShowForm }, { text: "Cancel", click: closedialog}]
        });



    }

    function ShowForm() {

        var check = new Boolean();
        var checkTextbox = new Boolean();
        check = false;
        checkTextbox = false;
        //Checking whether any empty textboxes exitst
        $('#tbForms :checked').each(function () {
            var str = new String();
            var addtext = new String();
            str = $(this).attr("id");
            str = str.substring(3, str.length);
            check = true;
            addtext = $(this).closest('tr').find('input:text').attr("value");
            if (addtext != undefined && addtext == '') {
                showDialog('You have not mentioned any additional info. for one/more of the selected form/additional document entry, do you want to continue ?', 'YESNO')
                checkTextbox = false;
                return false;
            }
            checkTextbox = true;

        });
        // if there is no empty textbox exists then continue else

        if (checkTextbox) {
            saveForms();
        }
        //if atleast one form is not selected then show confirmation dialog box
        if (!check) {
            showDialog('You have not selected any Additional Forms/Documents to be submitted by Beneficiary, do you want to continue ?','YESNO')
            return false;
        }

    }
    //save forms
    function saveForms() {
        $("#ConfirmDialog").dialog('close');

        var Formlist = new String();
        Formlist = "";
        $('#tbForms :checked').each(function () {
            var str = new String();
            var addtext = new String();
            str = $(this).attr("id");
            str = str.substring(3, str.length);
            check = true;
            addtext = $(this).closest('tr').find('input:text').attr("value");

            if (addtext != undefined && addtext != '') {
                str = str + ',' + addtext
            }

            if (Formlist == "") {
                Formlist = str;
            }
            else if (Formlist != "") {
                Formlist = Formlist + "$$" + str;
            }
        });
        //calling showformclick webmethod from jquery 
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "DeathBenefitsCalculatorForm.aspx/ShowFormclick",
            data: "{'Formlist':'" + Formlist + "'}",
            dataType: "json",
            success: function (record) {
                //Anudeep:15.12.2012 Changed code to Click Show form and insert code
                document.forms(0).submit();
            },
            failure: function (strReturnStatus) {
                alert("Error Occured While Opening Report");
                return false;
            }
        });
        $("#dvForm").dialog('close');
        $("#dvForm").dialog('destroy');

    }
    function closedialog() {
        $("#dvForm").dialog('close');
        $("#dvForm").dialog('destroy');
    }
    //Confirmation dialog box 
    function showDialog(text,type) {
        $('#lblMessage').text(text);
        if (type == 'YESNO') {
            $('#ConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                width: 550, height: 150,
                title: "DeathBenefit",
                buttons: [{ text: "Yes", click: saveForms }, { text: "No", click: closeconfirmDialog}],
                open: function (type, data) {

                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        }
        else if (type == 'OK') {
            $('#ConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                width: 550, height: 150,
                title: "Death Benefit Application",
                buttons: [{ text: "OK", click: closeconfirmDialog}],
                open: function (type, data) {

                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        }
        

        $("#ConfirmDialog").dialog({ modal: true });
        $("#ConfirmDialog").dialog("open");

    }

    function closeconfirmDialog(id) {

        $("#ConfirmDialog").dialog('close');
        $("#ConfirmDialog").dialog('destroy');
    }
    function ToggledActiveDivs() {
        //'Anudeep A.2013.01.07 - Added for YRS 5.0-1707:New Death Benefit Application form to give image for toggle 
        if ($('#imgActive').attr('src') == 'images/expand.GIF') {

            $('#imgActive').attr('src', 'images/collapse.GIF');
            $('#tdActive').attr('title', 'Collapse');
        }
        else {
            $('#imgActive').attr('src', 'images/expand.GIF');
            $('#tdActive').attr('title', 'Expand');
        }
        $('#divRetActive').toggle(200);
        $('#divSavActive').toggle(200);
    } 

    function ToogledRetireDivs() {
        //'Anudeep A.2013.01.07 - Added for YRS 5.0-1707:New Death Benefit Application form to give image for toggle 
        if ($('#imgRetire').attr('src') == 'images/expand.GIF') {

            $('#imgRetire').attr('src', 'images/collapse.GIF');
            $('#tdRetire').attr('title', 'Collapse');
        }
        else {
            $('#imgRetire').attr('src', 'images/expand.GIF');
            $('#tdRetire').attr('title', 'Expand');
        }
        $('#divRetRetired').toggle(200);
        $('#divSavRetired').toggle(200);

    }

    function BeneFiciaryAddress() {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "DeathBenefitsCalculatorForm.aspx/GetBeneFiciaryAddress",
            data: "{}",
            dataType: "json",
            success: function (record) {
                /*     'SP 2014.11.20 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN -adding if condition to check if representative details is missing*/
                if (record.d == 2) {
                    showDialog('Representative details are missing, please fill in the representative details from Person Maintenance', 'OK');
                    return false;
                }
                else if (record.d == 1) {
                    showDialog('Beneficiary Address does not exist', 'OK');
                    return false;
                }
                else {
                    InitializeFormDialogBox();
                    $('#dvForm').dialog('open');
                    return true;
                }
            },
            failure: function (record) {
                alert("Error while executing beneficiary address function");
                return false;
            }
        });
    }

</script>
</form>
<!-- Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up -->
<!--#include virtual="bottom.html"-->
