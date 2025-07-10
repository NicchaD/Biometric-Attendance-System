<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="UserControls/YMCA_Footer_WebUserControl.ascx"%>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx"%>
<%@ Register tagprefix="DataPagerFindInfo" Tagname="DataGridPager" Src="UserControls/DataGridPager.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FindEDIExlcusionInfo.aspx.vb" Inherits="YMCAUI.FindEDIExlcusionInfo" MasterPageFile="~/MasterPages/YRSPopUp.Master" %>
<asp:Content ID="cntHead" ContentPlaceHolderID="Head" runat="server" >
		<title>YMCA YRS</title>
		<script type="text/javascript" language="javascript">
		function _OnBlur_TextBox()
		{		
			document.Form1.all.btnFind.focus();		
		}
		function ValidateNumeric()
		{
			if ((event.keyCode < 48)||(event.keyCode > 57))
			{
			event.returnValue = false;
			}
		}
		</script>
		<%--<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">--%>
		
	</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="Maincontent">
			<%--<div class="Div_Center">
				<table class="Table_WithoutBorder" style="WIDTH: 100%; HEIGHT: 48px" cellSpacing="0" cellPadding="0">
					<tr>
						<td>
							<YRSControls:YMCA_Toolbar_WebUserControl id="YMCA_Toolbar_WebUserControl1" runat="server" ShowLogoutLinkButton="false" ShowHomeLinkButton="false"></YRSControls:YMCA_Toolbar_WebUserControl>
						</td>
					</tr>
					<tr>
						<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
							Find Information
						</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
					</tr>
				</table>
			</div>--%>
	<div class="Div_Center">
    <asp:ScriptManagerProxy ID="EDIExclusionListFindInfoScriptManager" runat="server">
    </asp:ScriptManagerProxy>
        <asp:UpdatePanel ID="UpdatePanelFindInfoEDIExclusion" runat="server">
            <ContentTemplate>
		        <table class="Table_WithBorder" style="WIDTH: 100%;" >
			        <tr style="vertical-align:top;">
				        <td align="left">
					        <table style="WIDTH: 100%;">
						        <tr style="vertical-align:top;">
							        <td style="WIDTH: 70%" align="left"><asp:label id="lblNoDataFound" CssClass="Label_Small" runat="server" Visible="False">No Matching Records</asp:label>
								        <div style="OVERFLOW: auto; WIDTH: 100%; vertical-align:top;height:280px; ">
                                            <%--<DATAPAGERFINDINFO:DATAGRIDPAGER id="dgPager" runat="server"></DATAPAGERFINDINFO:DATAGRIDPAGER>--%>
                                            <%--<asp:datagrid id="DataGridFindInfo" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False"
										        Width="100%" AllowSorting="True" AllowPaging="True" CssClass="DataGrid_Grid" >
										        <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
										        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
										        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
										        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
										        <Columns>
											        <asp:TemplateColumn>
												        <ItemTemplate>
													        <asp:ImageButton id="ImageButtonSelect" runat="server" ToolTip="Select" CommandName="Select" CausesValidation="False"
														        ImageUrl="images\select.gif" AlternateText="Select"></asp:ImageButton>
												        </ItemTemplate>
											        </asp:TemplateColumn>
											        <asp:BoundColumn DataField="SSNo" SortExpression="SSNo" HeaderText="SSNo"></asp:BoundColumn>
											        <asp:BoundColumn DataField="FirstName" SortExpression="FirstName" HeaderText="First Name"></asp:BoundColumn>
											        <asp:BoundColumn DataField="MiddleName" SortExpression="MiddleName" HeaderText="Middle Name"></asp:BoundColumn>
											        <asp:BoundColumn DataField="LastName" SortExpression="LastName" HeaderText="Last Name"></asp:BoundColumn>
											        <asp:BoundColumn Visible="False" DataField="FundIDNo" SortExpression="FundIDNo" HeaderText="FundNo"></asp:BoundColumn>
											        <asp:BoundColumn Visible="False" DataField="PersID" HeaderText="UniqueId"></asp:BoundColumn>
										        </Columns>
										        <PagerStyle Visible="False" Mode="NumericPages"></PagerStyle>
									        </asp:datagrid>--%>
                                            <asp:GridView ID="gvdFindInfo" runat="server" CssClass="DataGrid_Grid" Width="100%" AllowSorting="True"
                                        AutoGenerateColumns="False" AllowPaging="true" PageSize="10">
                                        <SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
                                        <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
                                        <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
                                                        CommandName="Select" ToolTip="Select Row"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                            <asp:BoundField DataField="SSNo" SortExpression="SSNo" HeaderText="SSNo." ItemStyle-Width="60px" ></asp:BoundField>
                                            <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="First Name" ItemStyle-Width="100px" ></asp:BoundField>
                                            <asp:BoundField DataField="MiddleName" SortExpression="MiddleName" HeaderText="Middle Name" ItemStyle-Width="95px" ></asp:BoundField>
                                            <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="Last Name"></asp:BoundField>
                                            <asp:BoundField DataField="FundIDNo" HeaderText="FundIDNo"></asp:BoundField>
                                            <asp:BoundField DataField="PersID" SortExpression="PersID" HeaderText="PersID" ></asp:BoundField>                                            
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="15" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle CssClass="pagination"  />
                                    </asp:GridView>
                                            

								        </div>
							        </td>
							        <td align="left">
								        <P></P>
								        <P>
									        <TABLE class="Table_WithOutBorder" id="Table1" style="WIDTH: 100%; " >
										        <TR style="vertical-align:top;">
											        <TD style="WIDTH: 95px; HEIGHT: 14px" align="left"><asp:label id="lblFundNo" CssClass="Label_Small" width="88px" Runat="server">Fund No.</asp:label></TD>
											        <TD style="HEIGHT: 14px" align="center"><asp:textbox id="txtFundNo" runat="server" CssClass="TextBox_Normal" width="150"
													        MaxLength="10"></asp:textbox></TD>
										        </TR>
										        <TR>
											        <TD style="WIDTH: 95px; HEIGHT: 18px" align="left" height="18"><asp:label id="lblSSNo" CssClass="Label_Small" width="88px" Runat="server">SS No.</asp:label></TD>
											        <TD style="HEIGHT: 18px" align="center" height="18"><asp:textbox id="txtSSNo" runat="server" CssClass="TextBox_Normal" width="150" 
													        MaxLength="9"></asp:textbox></TD>
										        </TR>
										        <TR>
											        <TD style="WIDTH: 95px; HEIGHT: 18px" align="left"><asp:label id="lblLastName" CssClass="Label_Small" width="88px" Runat="server">Last Name</asp:label></TD>
											        <TD style="HEIGHT: 18px" align="center"><asp:textbox id="txtLastName" runat="server" CssClass="TextBox_Normal" width="150" 
													        MaxLength="30"></asp:textbox></TD>
										        </TR>
                                                <TR>
											        <TD style="WIDTH: 95px; HEIGHT: 17px" align="left"><asp:label id="lblFirstName" CssClass="Label_Small" width="88px" Runat="server">First Name</asp:label></TD>
											        <TD style="HEIGHT: 17px" align="center"><asp:textbox id="txtFirstName" runat="server" CssClass="TextBox_Normal" width="150" 
													        MaxLength="20"></asp:textbox></TD>
										        </TR>
										        
										        <TR>
											        <TD style="WIDTH: 95px; HEIGHT: 12px" align="left"><asp:label id="lblCity" runat="server" CssClass="Label_Small" width="88px">City</asp:label></TD>
											        <TD style="HEIGHT: 12px" align="center"><asp:textbox id="txtCity" runat="server" CssClass="TextBox_Normal" width="150" 
													        maxlength="29"></asp:textbox></TD>
										        </TR>
										        <TR>
											        <TD style="WIDTH: 95px" align="left"><asp:label id="lblState" runat="server" CssClass="Label_Small" width="88px">State</asp:label></TD>
											        <TD align="center"><asp:textbox id="txtState" runat="server" CssClass="TextBox_Normal" width="150" 
													        maxlength="29"></asp:textbox></TD>
										        </TR>
									        </TABLE>
									        <TABLE id="Table2">
										        <TR style="vertical-align:top;">
											        <TD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
												        <asp:button id="btnFind" runat="server" Width="80" CssClass="Button_Normal" Text="Find"></asp:button></TD>
											        <TD align="right"><asp:button id="btnClear" runat="server" Width="80" CssClass="Button_Normal" Text="Clear"></asp:button></TD>
										        </TR>
									        </TABLE>
								        </P>
							        </td>
						        </tr>
					        </table>
					        <TABLE id="Table3" class="Td_ButtonContainer" style="WIDTH: 100%;">
						        <TR>
								        <TD style="width:100%" class="Td_ButtonContainer" align="right"><asp:button id="btnClose" runat="server" Width="80" CssClass="Button_Normal" Text="Close"></asp:button></TD>
						        </TR>
					        </TABLE>
					        </td>
			        </tr>
		        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
		<%--<YRSControls:YMCA_Footer_WebUserControl id="YMCA_Footer_WebUserControl1" runat="server"></YRSControls:YMCA_Footer_WebUserControl>--%>
	</div>
			<%--<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>--%>
</asp:Content>
