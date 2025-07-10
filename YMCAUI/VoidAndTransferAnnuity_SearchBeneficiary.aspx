<%@ Page Language="vb" AutoEventWireup="false"  MasterPageFile="~/MasterPages/YRSPopUp.Master" CodeBehind="VoidAndTransferAnnuity_SearchBeneficiary.aspx.vb" Inherits="YMCAUI.VoidAndTransferAnnuity_SearchBeneficiary"  %>
<%--<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx"%>

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
</script>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnFind">--%>
        <asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
            <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
            </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Maincontent" runat="server">
      <div class="div_center" style="width:100%; vertical-align:top;">
		
			<%--<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
			 <%--Added User Control to show user details
            <tr>
				<td>
					<YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL id="YMCA_Toolbar_WebUserControl1" runat="server" ShowLogoutLinkButton="false" ShowHomeLinkButton="false"></YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL>
				</td>
			</tr>
		
		<tr>
			<td class="Td_HeadingFormContainer" align="left" style="width:700;"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="12">
				Activities > Disbursements > Void Annuity > Transfer > Search Payee
			</td>
		</tr>
		<tr>
			<td>&nbsp;
			</td>
		</tr>
	</table>--%>

    <div id="divAdd" runat="server" style="OVERFLOW: auto; WIDTH: 100%;">
		<table cellspacing="0" cellpadding="0">
			<tr>
								<td>
								<table class="Table_WithBorder" width="735">								
								
								<tr valign="top" align="left">

									<td valign="top" width="80%" style="border:1;">
										 <asp:Label cssclass="Label_Small" ID="LabelNoDataFound" runat="server" Visible="False">No Matching Records</asp:Label>
										<div style="border-style: none; border-color: inherit; border-width: 1; OVERFLOW: auto; WIDTH:500px; HEIGHT: 374px; TEXT-ALIGN: left;"  >
										<asp:GridView ID="gvSearchPayee" runat="server" cssclass="DataGrid_Grid" width="500px"
                                                    allowsorting="True"  allowpaging="True" pagesize="10"  DataKeyNames="UniqueId"
												 AutoGenerateColumns="False">
											
												 <SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
													<AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
													<RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
													<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass = "DataGrid_AlternateStyle"/> 
													<Columns>
                                                    
													<asp:BoundField HeaderText="PersonID" DataField="UniqueId" Visible="false" />
													<%--<asp:BoundField HeaderText="FundEventID" DataField="FundIDNo" Visible="false" />--%>
														<asp:TemplateField>				
														<ItemTemplate>
														
																<asp:ImageButton ID="ImageButtonSelect" runat="server" ToolTip="Select" CommandName="Select" CausesValidation="False"
																	ImageUrl="images\select.gif" AlternateText="Select"></asp:ImageButton>
                                                                    <asp:ImageButton ID="ImageButtonSelected" runat="server" ToolTip="Select" CommandName="Select" CausesValidation="False"
																	ImageUrl="images\selected.gif" Visible="false" AlternateText="Select"></asp:ImageButton>
															</ItemTemplate>
														</asp:TemplateField>														
														<asp:BoundField HeaderText="SSN No" SortExpression="SSNo" DataField="SSNo" />
                                                        <asp:BoundField HeaderText="Last Name" SortExpression="LastName" DataField="LastName" />
														<asp:BoundField HeaderText="First Name" SortExpression="FirstName" DataField="FirstName" />
                                                        <asp:BoundField HeaderText="Middle Name" SortExpression="MiddleName" DataField="MiddleName" />	
                                                        <asp:BoundField HeaderText="Fund No." SortExpression="FundIdNo" DataField="FundIdNo" Visible="True"   />													
												<%--		<asp:BoundField HeaderText="Fund Status" SortExpression="FundStatus" DataField="FundStatus" />--%>
                                                        <asp:BoundField HeaderText="Salutation" SortExpression="SalutationCode" DataField="SalutationCode" Visible="false"/>
														<asp:BoundField HeaderText="Suffix" SortExpression="SuffixTitle" DataField="SuffixTitle" Visible="false"/>														
                                                        
                                                        	                                                       
													</Columns>
														<PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="First" LastPageText="Last"/>
                                                        <PagerStyle Font-Bold="false" Font-Size="X-Small"     /> 
										</asp:GridView>
                                        <br />
                                        <asp:Label runat="server" ID="lblTotalCount" CssClass="Label_Small" ></asp:Label>                                   
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
													
                                                        <asp:Button cssclass="Button_Normal" width="80" runat="server" accesskey="F"  id="btnFind"
                                                            text="Find" causesvalidation="False"  ></asp:Button>
                                                    </td>
                                                    <td>
                                                        <asp:Button cssclass="Button_Normal" width="80" runat="server" id="btnClear" text="Clear"
                                                            causesvalidation="False"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>                                           
									</td>
								</tr>				
								
							</table>
									
								</td>
								</tr>
			</table>
			<%--<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>--%>
	</div>
          </div>
	</asp:Content>
  <%--  </form>

</body>--%>

