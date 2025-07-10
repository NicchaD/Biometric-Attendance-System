<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MetaPaperTypes.aspx.vb" Inherits="YMCAUI.MetaPaperTypes" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
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
    <tr>
        <td class="Td_HeadingFormContainer" align="left" height="19">
            <img title="image" height="10" alt="image" src="images/spacer.gif" width="10">
            Paper Types
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
</table>
<div class="Div_Center">
    <table class="Table_WithBorder" height="415" width="696">
        <tr valign="top">
            <td align="left">
                <div>
                    <table class="Table_WithOutBorder" width="680" style="display:none">
                        <tr>
                            <td align="left">
                                <asp:label id="LabelLook" runat="server" cssclass="Label_Small" width="278px">Look For</asp:label>
                                &nbsp;<asp:textbox id="TextBoxFind" runat="server" cssclass="TextBox_Normal" width="112px"></asp:textbox>&nbsp;
                                <asp:button id="ButtonSearch" runat="server" cssclass="Button_Normal" causesvalidation="False"
                                    text="Search" width="80px"></asp:button>
                                <asp:label id="LabelNoRecordFound" runat="server" cssclass="Label_Small" width="117px"
                                    visible="false">No Record found</asp:label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="Div_Center">
                    <table class="Table_WithoutBorderds" height="341" width="680" border=0>
                        <tr>
                            <td>
                                <div style="overflow: auto; width: 300px; border-top-style: none; border-right-style: none;
                                    border-left-style: none; height: 250px; border-bottom-style: none" designtimedragdrop="28">
                                    <asp:datagrid id="DataGridMetaPaperType" runat="server" cssclass="DataGrid_Grid" width="280px"
                                        allowsorting="true" AutoGenerateColumns="false">
											<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
											<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
											<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
											<Columns>
												<asp:TemplateColumn>
													<ItemTemplate>
														<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
															CommandName="Select" ToolTip="Select"></asp:ImageButton>
													</ItemTemplate>
												</asp:TemplateColumn>
                                                <asp:BoundColumn DataField="PaperTypeId" HeaderText="PaperTypeId"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="PaperType" HeaderText="Paper Type"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Description" HeaderText="Description"></asp:BoundColumn>
											</Columns>
											<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
										</asp:datagrid>
                                </div>
                            </td>
                            <td valign="top">
                                <table class="Table_WithoutBorder" height="80" width="312" border=0>
                                   <br />   <br />
                                    <tr valign="top">
                                        <td align="left" valign="top">
                                       
                                            <asp:label id="LabelPaperTypeName" runat="server" cssclass="Label_Small" width="60">
										Paper Type </asp:label>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:textbox id="TextBoxPaperType" runat="server" cssclass="TextBox_Normal" width="146"
                                                readonly="true"></asp:textbox>
                                            <asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" controltovalidate="TextBoxPaperType"
                                                errormessage="Paper Type Name cannot be blank">*</asp:requiredfieldvalidator>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td align="left" valign="top">
                                            <asp:label id="LabelDescription" runat="server" cssclass="Label_Small" width="60">
										Description</asp:label>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:textbox id="TextBoxDescription" runat="server" cssclass="TextBox_Normal"
                                                width="146" readonly="true"></asp:textbox>
                                        </td>
                                       
                                    </tr>
                                   <tr runat="server" id="trprinter">
                                        <td align="left" valign="top">
                                            <asp:label id="LabelPrinter" runat="server" cssclass="Label_Small" width="60">
										Printer</asp:label>
                                        </td>
                                        <td align="left">

                                       <div style="overflow: auto; width: 300px; border-top-style: none; border-right-style: none;
                                    border-left-style: none; height: 100px; border-bottom-style: none" designtimedragdrop="28">
                                         		
                                            <asp:datagrid id="dgPrinter" Runat="server" CssClass="DataGrid_Grid" Width="280" AutoGenerateColumns="false">
																										<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																										<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																										<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																										<Columns>
																											<asp:BoundColumn DataField="PrinterID" Visible="False"></asp:BoundColumn>
																											<asp:BoundColumn DataField="PrinterName" HeaderText="Name"></asp:BoundColumn>
                                                                                                            <asp:BoundColumn DataField="PrinterDescription" HeaderText="Path"></asp:BoundColumn>
																											
																										</Columns>
																									</asp:datagrid>	    
                                             </div>   																	
                                        </td>
                                  
                                    </tr>
                                    <tr runat="server" id="trReport">
                                        <td align="left" valign="top">
                                           	   <asp:label id="LabelReport" runat="server" cssclass="Label_Small" width="60">
										Report</asp:label>
                                        </td>
                                        <td align="left">
                                          <div style="overflow: auto; width: 300; border-top-style: none; border-right-style: none;
                                    border-left-style: none; height: 100px; border-bottom-style: none" designtimedragdrop="28">
                                          <asp:datagrid id="dgReport" Runat="server" CssClass="DataGrid_Grid" Width="280" AutoGenerateColumns="false">
																										<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
																										<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
																										<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
																										<Columns>
																											<asp:BoundColumn DataField="ReportID" Visible="False"></asp:BoundColumn>
																											<asp:BoundColumn DataField="ReportName" HeaderText="Name"></asp:BoundColumn>
																											
																										</Columns>
																									</asp:datagrid>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table class="Table_WithoutBorder" width="312" border=1>
                                 
                                 
                                </table>
                                <asp:hiddenfield runat="server" id="hdnPrinterID"></asp:hiddenfield>
                                <asp:validationsummary id="ValidationSummary1" runat="server"></asp:validationsummary>
                            </td>
                        </tr>
                        <tr>
                            
                            <td width="270">
                            </td>
                            <td>
                                <table class="Table_WithoutBorder" height="30" width="365">
                                    <tr>
                                        <td class="Td_ButtonContainer">
                                            <asp:button id="ButtonSave" runat="server" cssclass="Button_Normal" width="50px"
                                                enabled="false" text="Save"></asp:button>
                                        </td>
                                        <td class="Td_ButtonContainer">
                                            <asp:button id="ButtonCancel" runat="server" cssclass="Button_Normal" width="50px"
                                                causesvalidation="False" enabled="false" text="Cancel"></asp:button>
                                        </td>
                                        <td class="Td_ButtonContainer">
                                            <asp:button id="ButtonDelete" runat="server" cssclass="Button_Normal" width="50px"
                                                 enabled="false" text="Delete" OnClientClick="javascript: return chkPaperType();"  ></asp:button>
                                        </td>
                                        <td class="Td_ButtonContainer">
                                            <asp:button id="ButtonAdd" runat="server" cssclass="Button_Normal" width="50px" causesvalidation="False"
                                                text="Add"></asp:button>
                                        </td>
                                        <td class="Td_ButtonContainer">
                                            <asp:button id="ButtonOK" runat="server" cssclass="Button_Normal" width="50px" causesvalidation="False"
                                                text="OK"></asp:button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
  
</form>
  <script language="javascript">
      function chkPaperType() {
          if (document.getElementById('trReport') != null) {
              alert('This Paper Type is linked with a report. Please change the report and then try again.');
              return false;
          }
          if (document.getElementById('trprinter') != null) {
              alert('This Paper Type is linked with Printer. Please change the Printer and then try again.');
              return false;
          }
          return true;
      }
  </script>
  
<!--#include virtual="bottom.html"-->