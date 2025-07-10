<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master"  EnableEventValidation="false"
	CodeBehind="CashApplicationFundingIndividual.aspx.vb" Inherits="YMCAUI.CashApplicationFundingIndividual" %>
	<%@ Register TagPrefix="YRSControls" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--<script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script>
<link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
<script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
<script src="JS/YMCA_JScript_Warn.js" type="text/javascript"></script>--%>
<style type="text/css">
.ui-datepicker select.ui-datepicker-month { width: 30%;}
.ui-datepicker select.ui-datepicker-year { width: 29%;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <script language="javascript" type="text/javascript">
	 //Code for checkbox Check all, start
        var allCheckBoxSelector = '#<%=gvTrn.ClientID%> input[id*="chkall"]:checkbox';
        var checkBoxSelector = '#<%=gvTrn.ClientID%> input[id*="chkSel"]:checkbox';

        function ToggleCheckUncheckAllOptionAsNeeded() {
            var totalCheckboxes = $(checkBoxSelector),
                        checkedCheckboxes = totalCheckboxes.filter(":checked"),
                        noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
                        allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);
            if (totalCheckboxes.length == 0) allCheckboxesAreChecked = false;
            $(allCheckBoxSelector).attr('checked', allCheckboxesAreChecked);
        }





        $(document).ready(function () {
        	$.ajaxSetup({ cache: false });
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    BindEvents();

                   }
                 

            }
        
            BindEvents();         

        });

        function BindEvents() {

                	$(allCheckBoxSelector).bind('click', function () {
        		$(checkBoxSelector).attr('checked', $(this).is(':checked'));
        		ToggleCheckUncheckAllOptionAsNeeded();
        	});

        	$(checkBoxSelector).bind('click', ToggleCheckUncheckAllOptionAsNeeded);

        	ToggleCheckUncheckAllOptionAsNeeded();

        	$('#<%=TextBoxYmcaNo.ClientID%>').bind('blur', function () {
        		var diff;
        		var flg = false;
        		var str = $('#<%=TextBoxYmcaNo.ClientID%>').val().trim();
        		var len;
        		len = str.length;
        		if (len == 0) {
        			return false;
        		}
        		else if (len < 6) {
        			diff = (6 - len);
        			for (i = 0; i < diff; i++) {
        				str = "0" + str

        			}
        		}
        		$('#<%=TextBoxYmcaNo.ClientID%>').val(str); 
        	});

        	$(checkBoxSelector).bind('click', ToggleCheckUncheckAllOptionAsNeeded);
        	ToggleCheckUncheckAllOptionAsNeeded();

        	$(document).ready(function () {
        		$('#ConfirmDialog').dialog({
        			autoOpen: false,
        			draggable: true,
        			close: false,
        			width: 350, height: 250,
        			title: "Cash Application - Person",
        			open: function (type, data) {
        				$(this).parent().appendTo("form");
        				$('a.ui-dialog-titlebar-close').remove();
        			}
        		});
        	});
        	function ToggleSearch(strId) {
        		alert(strId);
								//$('#' + strId).toggle(100);
         }
        
        	$(document).ready(function () {
        		$('#lnkModifyYmcaSearch').click(function () {
        			$('#divYmcaSearch').toggle(100);
        		});
        	});
        	

        	$(document).ready(function () {
        		$('#lnkbtnModifySearch').click(function () {
        			$('#divSearchCriteria').toggle(100);
        		});
        	});
        	

        		$.strPad = function (i, l, s) {
        			var o = i.toString();
        			if (!s) { s = '0'; }
        			while (o.length < l) {
        				o = s + o;
        			}
        			return o;
        		};

        		$(document).ready(function () {
        			$('#<%=dtUserFundeddate.ClientID%>').datepicker({ minDate: new Date(1900, 10 - 1, 25), 
						maxDate: 0 ,
						showOn: 'button',
						buttonImage: 'images/calendar.gif',
						buttonImageOnly: true,
						changeMonth: true,
						changeYear: true,buttonText: 'Click here to select date.'
					})
				
        		});
        
        }
    
	</script>
	<asp:ScriptManagerProxy ID="ScriptMgrProxyCashApplication"  runat="server">
	</asp:ScriptManagerProxy>
	<asp:HiddenField ID="HiddenFieldDirty" ClientIDMode="Static" Value="false" runat="server" />
	<div class="Div_Center" style="height: 550;">
		<table width="100%" style="height:550" class="Table_WithBorder">
			<tr >
			
				<td class="Td_Form_Header"  align="left"> <%--Header --%>
				<asp:UpdatePanel ID="UpdatePanel2" runat="server"  >
					<ContentTemplate>
					<img title="image" height="8" alt="image" src="images/spacer.gif" width="3px"/>
					<asp:Label ID="lblHdr" runat="server" CssClass="Td_Form_Header">Search YMCA</asp:Label>
					</ContentTemplate></asp:UpdatePanel>
				</td>
			
			</tr>
			<tr> <%--Tab List--%>	
			<td valign="top" >
			<asp:UpdatePanel ID="updtPnlTab" runat="server"  >
					<ContentTemplate>
				<table class="td_withoutborder" width="100%">
                                    <tr>
                                        <td id="tdSearchYMCA" runat="server"  style="background-color: #4172A9; font-family: verdana;
                                            font-weight: bold; font-size: 11pt; color: #ffffff; width: 33%; text-align: center;
                                            border: solid 1px White; border-bottom: none;height:28px">                                           
                                            <asp:LinkButton ID="lnkSearchYMCA" Text="Search YMCA" runat="server" Style="font-family: verdana;
                                                font-weight: bold; font-size: 10pt; color: #ffffff"></asp:LinkButton>
                                            <asp:Label ID="lblSearchYMCA" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #000000" runat="server" Text="Search YMCA"></asp:Label>
                                        </td>
                                        <td id="tdSearchPerson" runat="server" style="background-color: #4172A9; font-family: verdana;
                                            font-weight: bold; font-size: 11pt; color: #ffffff; width: 33%; text-align: center;
                                            border: solid 1px White; border-bottom: none;height:28px">
                                            <asp:LinkButton ID="lnkSearchPerson" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #ffffff" Text="Select Person & Transmittal" runat="server"></asp:LinkButton>
                                            <asp:Label ID="lblSearchPerson" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #000000" runat="server" Text="Select Person & Transmittal"></asp:Label>
                                        </td>
                                        <td id="tdSelectTransaction" runat="server" style="background-color: #4172A9; font-family: verdana;
                                            font-weight: bold; font-size: 11pt; color: #ffffff; width: 34%; text-align: center;
                                            border: solid 1px White; border-bottom: none;height:28px">
                                            <asp:LinkButton ID="lnkSelectTransaction" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #ffffff" Text="Select Transaction(s) & Apply Funds" runat="server"></asp:LinkButton>
                                            <asp:Label ID="lblSelectTransaction" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #000000" runat="server" Text="Select Transaction(s) & Apply Funds"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
				
					</ContentTemplate>
					
					</asp:UpdatePanel>
			</td>
			</tr>
			<tr>
			<td valign="top"  > <%--Tabs details--%>
				<asp:UpdatePanel ID="up" runat="server"   >
					<ContentTemplate>
			
					 <asp:MultiView id="MV" runat="server" ActiveViewIndex=0>
						<asp:View id="ViewYMCASearch" runat="server">
						
					
							<table width="100%"  >
						
								<tr valign="top">									
									<td></td>
								</tr>
								<tr id="trYmcaSearch" runat="server" visible="true">
								<td>
								<table cellspacing='0' width="100%">
								<tr>
								<td class="td_Text" >
										<span id="spnSeletedYMCA" runat="server"></span>
									</td>
									<td align="right" class="td_Text"> <a href='#' ID="lnkModifyYmcaSearch"  style="font-size:smaller;text-align:right">Hide/Modify Search</a></td>
								</tr>
								</table>
								</td>
								
								</tr>
								
								<tr>
								<td valign="top"  width="80%">
								<div id="divYmcaSearch">
										<table width="80%">
											<tr>
												<td colspan='5' class="Label_Small">
													Search YMCA
												</td>
											</tr>
											<tr>
												<td>
													<asp:Label ID="LabelYmcaNo" runat="server" CssClass="Label_Small">YMCA No.</asp:Label>
												</td>
												<td>
													<asp:Label ID="LabelYmcaName" runat="server" CssClass="Label_Small">YMCA Name</asp:Label>
												</td>
												<td>
													<asp:Label ID="LabelCity" runat="server" CssClass="Label_Small">City</asp:Label>
												</td>
												<td>
													<asp:Label ID="LabelState" runat="server" CssClass="Label_Small">State</asp:Label>
												</td>
												<td>
												</td>
											</tr>
											<tr>
												<td>
													<asp:TextBox Width="100" runat="server" ID="TextBoxYmcaNo" name="TextBoxYmcaNo" CssClass="TextBox_Normal"
														MaxLength="6"></asp:TextBox>
												</td>
												<td>
													<asp:TextBox Width="100" runat="server" ID="TextBoxYmcaName" CssClass="TextBox_Normal"
														MaxLength="60"></asp:TextBox>
												</td>
												<td>
													<asp:TextBox Width="100" runat="server" ID="TextBoxCity" CssClass="TextBox_Normal"
														MaxLength="30"></asp:TextBox>
												</td>
												<td>
													<asp:TextBox Width="100" runat="server" ID="TextBoxState" CssClass="TextBox_Normal"
														MaxLength="2"></asp:TextBox>
												</td>
												<td colspan='2' align="center">
													<asp:Button Width="80" runat="server" ID="btnYmcaFind"  Text="Find" CssClass="Button_Normal">
													</asp:Button>
													&nbsp;
													<asp:Button Width="80" runat="server" ID="btnYmcaClear" Text="Clear" CssClass="Button_Normal">
													</asp:Button>
												</td>
											</tr>
										</table>
										</div>
									</td>
								</tr>
								<tr>
								<td  class="Label_Small">
								<span id="spnSelectYMCA" runat="server" visible="false"> Select a YMCA</span>
								</td>
								</tr>
								<tr valign="top">
									<td valign="top" width="40%">
										<asp:Label ID="LabelRecordNotFound" Runat="server" CssClass="Label_Small" visible="false">No Matching Records</asp:Label>
										<DIV style="OVERFLOW: auto;  HEIGHT: 556px; vertical-align:top; TEXT-ALIGN: left">
											<asp:GridView ID="gvYmca"   AllowSorting="True"  AllowPaging="True" PageSize="20" Runat="server" Width="60%" CssClass="DataGrid_Grid" CellPadding="0"
												CellSpacing="0" AutoGenerateColumns="false" DataKeyNames="UniqueId">
												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												<AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />												
												<RowStyle CssClass="DataGrid_NormalStyle" />
												<SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
												<Columns>
													<asp:TemplateField>
														<ItemTemplate>
														<asp:LinkButton runat="server" ID="lnkSelect" CommandName="Select" Text="Select" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "YMCANo") %>'></asp:LinkButton>
															<%--<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																CommandName="Select" ToolTip="Select"></asp:ImageButton>--%>
														</ItemTemplate>
													</asp:TemplateField>
													<asp:BoundField DataField="UniqueId" Visible="false" HeaderText="UniqueId" />
													<asp:BoundField DataField="YMCANo" HeaderText="YMCA No."  SortExpression="YMCANo" />
													<asp:BoundField DataField="YMCAName" HeaderText="YMCA Name" SortExpression="YMCAName" />
													<asp:BoundField DataField="City" HeaderText="City" SortExpression="City" />
													<asp:BoundField DataField="State" HeaderText="State" SortExpression="State" />
												</Columns>
											</asp:GridView>
										</DIV>
									</td>
									
								</tr>
								<tr>
									<td valign="bottom">
										<table class="Table_WithBorder"  cellspacing='0'  width="100%">
											<tr>
												<td class="Td_ButtonContainer" align="right">
													<asp:Button ID="btnSearchYmcaClose"  CssClass="Button_Normal" Width="80" runat="server"
														Text="Close"></asp:Button>
												</td>
												<td class="Td_ButtonContainer" align="right">
													<asp:Button ID="btnSearchYmcaNext" CssClass="Button_Normal" Width="80" runat="server"
														Text="Next"></asp:Button>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								</table>
					
					
						</asp:View>

							<asp:View id="ViewTransmittal" runat="server">
				
							<table  width="100%" cellpadding='0' cellspacing='0' style="height:556px;">
							<tr> 
							<td align="right"> 
								</td>
								</tr>
								<tr id="trSearchPanel" runat="server" visible="true">
								<td>
								<table cellspacing='0' width="100%">
								<tr>
								<td class="td_Text" >
										<asp:Label id="spnSelectedPerson" runat="server"></asp:Label>
									</td>
									<td align="right" class="td_Text"> <a href='#' ID="lnkbtnModifySearch"  visible="false" style="font-size:smaller;text-align:right">Hide/Modify Search</a></td>
								</tr>
								</table>
								</td>
								
								</tr>
								<tr>
									<td  valign="top">
										<div id="divSearchCriteria" >
											<table border="0" cellpadding="0" cellspacing="0" style="text-align:center;">
											
												<tr>
													<td >
														<table width="100%" cellspacing="0" >
															<tr><td colspan='9' class="Label_Small">Search Person</td></tr>
															<tr>
																<td class="Label_Small" style="text-align:left;">
																	Fund No :
																</td>
																<td>&nbsp;</td>
																
																<td class="Label_Small" style="text-align:left;">
																	SSN :
																</td>
																<td>&nbsp;</td>
																<td class="Label_Small" style="text-align:left;">
																	First Name :
																</td>
																<td>&nbsp;</td>
																<td class="Label_Small" style="text-align:left;">
																	Last Name :
																</td>
																<td>&nbsp;</td>
																<td style="text-align:left;">
																</td>
															</tr>
															<tr>
																<td style="text-align:left;">
																	<asp:TextBox runat="server" Width="100px" ID="txtSearchFundNo" CssClass="TextBox_Normal"></asp:TextBox>
																</td><td>&nbsp;</td>
																<td style="text-align:left;">
																	<asp:TextBox runat="server" Width="100px" ID="txtSearchSSN" CssClass="TextBox_Normal"></asp:TextBox>
																</td><td>&nbsp;</td>
																<td style="text-align:left;">
																	<asp:TextBox runat="server" Width="100px" ID="txtSearchFirstName" CssClass="TextBox_Normal"></asp:TextBox>
																</td><td>&nbsp;</td>
																<td style="text-align:left;">
																	<asp:TextBox runat="server" Width="100px" ID="txtSearchLastName" CssClass="TextBox_Normal"></asp:TextBox>
																</td><td>&nbsp;</td>
																<td  align="left" >
																	<asp:Button runat="server" Width="80px" ID="btnPersonFind" Text="Find" CssClass="Button_Normal" />
																	&nbsp;&nbsp;<asp:Button  Width="80px" ID="btnPersonClear" runat="server" Text="Clear" CssClass="Button_Normal" />
																</td>
															</tr>
														<tr>
													<td>
													&nbsp;
													</td>
													</tr>
														</table>
													</td>
												</tr>
												<tr><td  class="Label_Small" align="left"> <span id="spnSelectPerson" runat="server" visible="false">Select a Person </span> </td></tr>
												<tr>
													<td>
												
														<div style="Z-INDEX: 0; height: 210px;TEXT-ALIGN: left; OVERFLOW: auto" id="divpersonsearch">
														<asp:GridView ID="gvPerson" AllowSorting="True"  AllowPaging="True" PageSize="10" Visible="true" runat="server" Width="80%" CssClass="DataGrid_Grid"
															CellPadding="0" CellSpacing="0"  DataKeyNames="FundEventID"  AutoGenerateColumns="False">
															 <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
															<RowStyle CssClass="DataGrid_NormalStyle" />
															<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
															<Columns>
																<asp:TemplateField>
																	<ItemTemplate>
																		<asp:LinkButton runat="server" ID="lnkSelect"  CommandName="Select"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FundNo") %>' Text="Select"></asp:LinkButton>
																	</ItemTemplate>
																</asp:TemplateField>
																<asp:BoundField DataField="YmcaId" HeaderText="YmcaId" Visible="false" />
																<asp:BoundField DataField="FundEventID" HeaderText="FundEventID" Visible="false"/>
																<asp:BoundField DataField="SSN" HeaderText="SSN" SortExpression="SSN" />
																<asp:BoundField DataField="FundNo" HeaderText="Fund No." SortExpression="FundNo" />
																<asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName"></asp:BoundField>
																<asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName"></asp:BoundField>
															</Columns>
															<EmptyDataTemplate>
															No Matching Record(s)
															</EmptyDataTemplate>
														</asp:GridView>
														</div>
													</td>
												</tr>
											</table>
										</div>
									</td>
								</tr>
								<tr>
								<td  valign="top" style="width:10%">
								<div id="divTransmittal" runat="server" visible="true">
								<table width="100%">
								<tr>
								<td></td>
								</tr>
								<tr><td  class="Label_Small" align="left"> <span id="spnSelectTransmittal" runat="server" visible="false">Select a Transmittal for funding Transaction(s) under it </span> </td></tr><tr><td  class="Label_Small" align="left"> <span id="Span2" runat="server" visible="false">Select a Person </span> </td></tr>
								<tr>
								<td colspan='2'>
								
								<div style="overflow: auto;  height: 210px; text-align: left">
											<asp:GridView ID="gvTransmittal" Visible="true" runat="server" AllowSorting="True"  AllowPaging="True" PageSize="10" DataKeyNames="UniqueId" Width="70%" CssClass="DataGrid_Grid"
												CellPadding="0" CellSpacing="0" AutoGenerateColumns="False">
												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												<AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />												
												<RowStyle CssClass="DataGrid_NormalStyle" />
												<SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
												
												<Columns>
													<asp:TemplateField HeaderText="">
														<ItemTemplate>														
																<asp:LinkButton runat="server" ID="lnkSelect" CommandName="Select"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TransmittalNo") %>' Text="Select" ></asp:LinkButton>
														</ItemTemplate>
													</asp:TemplateField>
													<asp:BoundField DataField="UniqueId" Visible="false" />
													<asp:BoundField DataField="TransmittalNo" HeaderText="Transmittal No" SortExpression="TransmittalNo" />
													<asp:BoundField DataField="TransmittalDate" HeaderText="Transmittal Date" SortExpression="TransmittalDate" />
													<asp:BoundField DataField="AmtDue" HeaderText="Transmittal Total" DataFormatString="{0:N}" SortExpression="AmtDue">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="AmountPaid" HeaderText="Amount Paid" DataFormatString="{0:N}" SortExpression="AmountPaid">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>													
													<asp:BoundField DataField="Balance" HeaderText="Amt. Due this Participant" DataFormatString="{0:N}" SortExpression="Balance">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
												</Columns>
											</asp:GridView>
										</div>
								</td>
								</tr>
								</table>
								</div>
								</td>
									
								</tr>
								<tr>
									<td valign="bottom">
										<table class="Table_WithBorder" cellspacing='0'  width="100%">
											<tr>
												<td class="Td_ButtonContainer" align="left">
													<asp:Button ID="btnPersonPrevious" CssClass="Button_Normal" Width="80" runat="server" Text="Previous">
													</asp:Button>
												</td>
												<td class="Td_ButtonContainer" align="center">
													<asp:Button ID="btnPersonClose" CssClass="Button_Normal" Width="80" runat="server" Text="Close">
													</asp:Button>
												</td>
												<td class="Td_ButtonContainer" align="right">
													<asp:Button ID="btnPersonNext" CssClass="Button_Normal" Width="80" runat="server" Text="Next">
													</asp:Button>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
							
				
						</asp:View>
							<asp:View id="ViewTransaction" runat="server">
						
					
						<table width="100%" cellpadding='0' cellspacing='0' style="height:556px">
						<tr class="Td_Form_Header">
						<td align="left" ><asp:Literal id="spnFundDetails" runat="server"></asp:Literal>
						</td>
						<td  align="right"><span id="spnSelectedTransmittal" runat="server"></span>
						</td>
						</tr>
						<tr> 
						<td valign="top" align="center" style="padding:2px 100px 3px 100px" colspan='2'>
						
						<table width="85%" cellpadding='0' cellspacing='0' >
						
						
						<tr>
						<td colspan='2'>
						<table width="100%"  cellpadding ='0' cellspacing='0' >
						
											<tr>
												<td align="left" width="20%"><asp:label id="lblFundingDate" CssClass="Label_Small" Runat="server"> Funding Date: </asp:label></td>
												<td align="left" colspan='3' >
												
												<%--<YRSControls:DateUserControl  id="dtUserFundeddate" runat="server"></YRSControls:DateUserControl>--%>
												<asp:TextBox id="dtUserFundeddate" ReadOnly="False" ValidationGroup="process" MaxLength="10"  Width="80px" CssClass="TextBox_Normal DateControl" onpaste="return false;" runat="server"></asp:TextBox>
												<asp:RequiredFieldValidator id="rfDatevalidation" runat="server" Enabled="True" Display="Dynamic" ControlToValidate="dtUserFundeddate"
													ErrorMessage="Date cannot be blank" ValidationGroup="process" CssClass="Error_Message">*</asp:RequiredFieldValidator>
													<asp:RangeValidator id="RangeValidatorUCDate" runat="server" Enabled="true" Display="Dynamic" ControlToValidate="dtUserFundeddate" ValidationGroup="process" Type="Date" MinimumValue="01/01/1900"   CssClass="Error_Message">Invalid Date</asp:RangeValidator>
												</td>
											</tr>
							<tr>
						<td colspan='4'>&nbsp;</td>
						</tr>
												<tr>
													<td >
														<asp:label id="lblAmountDue" CssClass="Label_Small" Runat="server"> Amount Due: </asp:label>
													</td>	
												<td>
													<asp:Label id="lblAmountPaid" Runat="server" CssClass="Label_Small">Amount Paid:</asp:Label>
													</td>
												<td >
														<asp:label id="lblAmountPaidNotUse" CssClass="Label_Small" Runat="server"> Amount Paid not Used: </asp:label>
													</td>	
												<td  >
													<asp:Label id="lblCreditAvaiable" Runat="server" CssClass="Label_Small">Credit Available:</asp:Label>
													</td>
												
												</tr>

												<tr>
													
													<td >
														<asp:TextBox ID="txtAmountDue" width="70px"   CssClass="TextBox_Normal" ReadOnly="true" runat="server"></asp:TextBox>
														
													</td>
													<td >
														<asp:TextBox ID="txtAountPaid" width="70px"  CssClass="TextBox_Normal" ReadOnly="true" runat="server"></asp:TextBox>
													</td>
												
												<td >
													<asp:textbox width="100px" runat="server" id="txtAmountPaidNotUse"  CssClass="TextBox_Normal"	ReadOnly="true"></asp:textbox>
													<asp:ImageButton id="imgbtnAmountPaidNotUse" runat="server" Height="18px" Width="18px" AlternateText="Click here to apply/Un-apply Amount" ImageUrl="images/Cash-UnApplied.jpg"></asp:ImageButton>
												</td>
												
												<td >
													<asp:textbox width="100px" runat="server" id="txtCreditAvailable"  CssClass="TextBox_Normal"	ReadOnly="true"></asp:textbox>
													<asp:ImageButton id="imgbtnCreditAvailable"  AlternateText="Click here to apply/Un-apply Amount" runat="server" Height="18px" Width="18px" ImageUrl="images/Cash-UnApplied.jpg"></asp:ImageButton>
												</td>
												</tr>
											</table>
						</td>
						</tr>
						<tr>
						<td colspan='2'>&nbsp;</td>
						</tr>
						<tr>
						<td colspan='2' > <span  class="Label_Small" id="spnReceipts" runat="server"> Select Receipt to fund.</span></td>
						</tr>
						<tr>
						<td>
						<div style="OVERFLOW: auto;  HEIGHT: 60px; TEXT-ALIGN: left;" >
						<asp:Label ID="lblReceiptNoRecord" Runat="server" CssClass="Label_Small" visible="false">No Receipt Records</asp:Label>				
									    <asp:GridView ID="gvReceipts" EmptyDataText="No receipt(s) available." Runat="server" CssClass="DataGrid_Grid" CellPadding="0" CellSpacing="0"
												AutoGenerateColumns="False" width="93%">
												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												<AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />												
												<RowStyle CssClass="DataGrid_NormalStyle" />
												<SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
												<Columns>
													<asp:TemplateField>
														<ItemTemplate>
														<asp:LinkButton runat="server" ID="lnkSelect" CommandName="Select" CommandArgument='<%# Container.DataItemIndex %>' Text="Select"></asp:LinkButton>
															<%--<asp:ImageButton id="imgbtnReceipts" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
																CommandName="Select" ToolTip="Select"></asp:ImageButton>--%>
														</ItemTemplate>
													</asp:TemplateField>
													<asp:BoundField DataField="ReceiptId" HeaderText="Receipt Number" />
													<asp:BoundField DataField="ReceiptIdDate" HeaderText="Check Date" DataFormatString="{0:d}" />
													<asp:BoundField DataField="Receiveddate" HeaderText="Received Date" DataFormatString="{0:d}" />
													<asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:N}">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
												</Columns>
											</asp:GridView>
										</div>
						</td>
						<td valign="top" >
						<asp:ImageButton id="imgBtnReceipts" runat="server" Height="18px" Width="18px" ImageUrl="images/Cash-UnApplied.jpg"
								ToolTip="Apply Receipts"></asp:ImageButton>
						</td>
						</tr>
						<tr>
						<td colspan='2'>&nbsp;</td>
						</tr>
						<tr>
						<td  class="Label_Small">Select Transaction(s) to fund.</td><td class="Label_Small"> Total Transaction(s) Amount :  $<asp:Label ID="lblTransmitalAmount" runat="server" Text="0.00"></asp:Label></td>
						</tr>
						<tr>
						<td colspan='2'>
						<div style="OVERFLOW: auto;  height:250px;  TEXT-ALIGN: left">
						
						<asp:GridView ID="gvTrn"  Visible="true" Runat="server" Width="93%" CssClass="DataGrid_Grid" CellPadding="0"
												CellSpacing="0" AutoGenerateColumns="False" DataKeyNames="UniqueId">
												<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
												<AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />												
												<RowStyle CssClass="DataGrid_NormalStyle" />
												<SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
												<Columns>
													<asp:TemplateField HeaderText="">
													  <HeaderTemplate>
                                                                <asp:CheckBox ID="chkall"  runat="server" />
                                                            </HeaderTemplate>
														<ItemTemplate>
															<asp:CheckBox id="chkSel"   Checked='<%# DataBinder.Eval(Container.DataItem, "Slctd") %>'  runat="server" >
															</asp:CheckBox>
														</ItemTemplate>
													</asp:TemplateField>
													<asp:BoundField DataField="AccountType"  Visible="true" HeaderText="Account Type" />
													<asp:BoundField DataField="TransactionAmount" FooterStyle-HorizontalAlign="Right" HeaderText="Transaction Amount" DataFormatString="{0:N}">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="Balance" FooterStyle-HorizontalAlign="Right" HeaderText="Balance" DataFormatString="{0:N}">
														<ItemStyle HorizontalAlign="Right"></ItemStyle>
													</asp:BoundField>
													<asp:BoundField DataField="TransactionDate" Visible="true" HeaderText="Transaction Date" />
													<asp:BoundField DataField="TransactionType" HeaderText="Transaction Type" >														
													</asp:BoundField>
												</Columns>
											
											</asp:GridView>
						</div>
						</td>
						</tr>
                            <tr>
						<td colspan='2'>&nbsp;</td>
						</tr>
                            <tr>                           
                                 <td class="Label_Small">
                                     Total Selected Transaction(s) Amount :  $<asp:Label ID="lblTotalSelectedAmount" runat="server" Text="0.00"></asp:Label>
                                </td>                                          
                                     <td class="Label_Small">
                                   Total Applied Transaction(s) Amount :  $<asp:Label ID="lblTotalAppliedAmount" runat="server" Text="0.00"></asp:Label>
                                    
                                </td> 

                            </tr>				
						</table>
						
						</td>
						</tr>
						
						
						<tr>
						<td colspan='2' >&nbsp;</td>
						</tr>
					
							<tr>
								<td valign="bottom"  colspan='2'>
									<table class="Table_WithBorder" width="100%" cellspacing='0'>
										<tr>
											<td class="Td_ButtonContainer" align="left">
												<asp:Button ID="btnTransactionPrevious" CssClass="Button_Normal" Width="80" runat="server"
													Text="Previous"></asp:Button>
											</td>
											<td class="Td_ButtonContainer" align="center">
												<asp:Button ID="btnTransactionClose" CssClass="Button_Normal" Width="80" runat="server"
													Text="Close"></asp:Button>
											</td>
                                            	<td class="Td_ButtonContainer" align="center">
												<asp:Button ID="btnUpdateAmount" ValidationGroup="Process" CssClass="Button_Normal" Width="120" runat="server"
													Text="Update Total"></asp:Button>
											</td>
											<td class="Td_ButtonContainer" align="right">
												<asp:Button ID="btnTransactionProcess" ValidationGroup="process" CssClass="Button_Normal" Width="80" runat="server"
													Text="Process"></asp:Button>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					
						</asp:View>
					
					</asp:MultiView>
					</ContentTemplate>
					</asp:UpdatePanel>
				</td>
			</tr>
			<tr>
				<td><IMG title="image" height="1" alt="image" src="images/spacer.gif" width="1"></td>
			</tr>
		
		</table>
	</div>	 
		<asp:placeholder id="PlaceHolderMessageBox" runat="server"></asp:placeholder>
		
</asp:Content>
