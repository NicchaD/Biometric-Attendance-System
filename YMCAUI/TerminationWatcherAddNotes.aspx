<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TerminationWatcherAddNotes.aspx.vb" Inherits="YMCAUI.TerminationWatcherAddNotes" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx"%>


<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
<head>
 <%-- Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
<%--<script type="text/javascript">
    function close() {

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "TerminationWatcherAddNotes.aspx/RefreshPage",
            data: "{}",
            dataType: "json",
            success: function (data) {}
        });
        window.opener.document.forms(0).submit();
        
    }
    window.onbeforeunload = close;
</script>--%>
    <style type="text/css">
        .style1
        {
            height: 16px;
        }
        .style2
        {
            width: 414px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
		<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
		<tr>    <%--Added User Control to show user details --%>
				<td>
					<YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL id="YMCA_Toolbar_WebUserControl1" runat="server" ShowLogoutLinkButton="false" ShowHomeLinkButton="false"></YRSCONTROLS:YMCA_TOOLBAR_WEBUSERCONTROL>
				</td>
			</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="left" style="width:700;"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="12">
			Termination Watcher > Add Notes
			</td>
		</tr>
		<tr>
			<td class="style1">&nbsp;
			</td>
		</tr>
	</table>
    				<table class="Table_WithBorder"  width="700" >
	
					<tr valign="top">
						<td valign="top" align="left" class="style2" >
						 <asp:Label ID="LabelNoDataFound" runat="server" CssClass="Label_Small" Visible="False">No Records Found.</asp:Label>
							<div  style="overflow: scroll; width: 400px; height: 300px; text-align: left" >
								
								 <asp:gridview id="gvNotes" runat="server" cssclass="DataGrid_Grid" width="370px"
                                                    allowsorting="True" allowpaging="True" pagesize="10"  DataKeyNames="TerminationWatcherId" AutoGenerateColumns="false">
													<SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
													<AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
													<RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
													<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
													<Columns>
														<asp:TemplateField Visible="false">
															<ItemTemplate>
																<asp:ImageButton id="ImageButtonSelect" runat="server" ToolTip="Select" CommandName="Select" CausesValidation="False"
																ImageUrl="images\select.gif" AlternateText="Select" CommandArgument="TWNotesID"></asp:ImageButton>
															</ItemTemplate>
														</asp:TemplateField>


														<asp:TemplateField HeaderText="Notes" ItemStyle-Width="350px">
															<ItemTemplate>
															
																	 
																			<div style="width: 350px;word-wrap:break-word; ">
																				<%# Eval("Notes")%>
																			</div>
																		

																	
																														
																
																
															</ItemTemplate>
														</asp:TemplateField>

																										

														<%--<asp:BoundField HeaderText="Notes" DataField="Notes" />--%>
														<asp:BoundField HeaderText="Is Important" DataField="Important" Visible="false" />
														<asp:BoundField HeaderText="TWNotesID" Visible= "false" DataField="TWNotesID" />
														<asp:BoundField headertext="TerminationWatcherId" datafield="TerminationWatcherId" visible="false" />
														
													</Columns>
													 <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="First" LastPageText="Last"/>
                                                    <PagerStyle  Font-Names="Arial" Font-Size="Small"    />  
												</asp:gridview>
							</div>
						</td>
						<td valign="top" align="left">
						 <table width="280" >
						 <tr>
						 <td><asp:Label ID="lblNotes" CssClass="Label_Small" runat="server" Text="Notes:"></asp:Label></td>
						 </tr>
							<tr>
								<td>
									<asp:TextBox ID="txtNotes" cssclass="TextBox_Normal" runat="server" TextMode="MultiLine" Height="165px" 
										Width="160px" ></asp:TextBox>
										
									<asp:RequiredFieldValidator ID="rfvtxtNotes" runat="server" 
										ControlToValidate="txtNotes" Display="Dynamic" 
										ErrorMessage="Notes can not be blank" ValidationGroup="Notes">*</asp:RequiredFieldValidator>
										
								</td>
                            </tr>
							<tr>
								<td nowrap="nowrap"><asp:Label ID="lblBitImportant" CssClass="Label_Small" runat="server" Text="Mark as Important "></asp:Label>
						 
									<asp:CheckBox ID="chkImportanat" runat="server"  />
								</td>
                                </tr>
                                <tr>
                                <%-- 'Start :Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                                <td>
                                     <asp:ValidationSummary runat="server" CssClass="Label_Small" ID="ValidationSummary1" ValidationGroup="Notes" />
                                </td>
                                <%-- 'End :Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                            </tr>
						 </table>
                         
                          </td>
						
                               
                        
					</tr>
                    <tr>
                      
                    </tr>
					<tr>
					<td colspan="2">
						<table cellspacing="0"  width="700">
						<tr align="right">
							<%--<td align="left" class="Td_ButtonContainer">
								<asp:Button  Text="Add" id="btnAdd" CssClass="Button_Normal" runat="server" 
									style="width: 80px;" CausesValidation="False" />
								
							</td>--%>
							<td align="right" class="Td_ButtonContainer">

							<asp:Button  Text="OK" id="btnOK" CssClass="Button_Normal" runat="server" style="width: 80px;" /> &nbsp;
                            <asp:Button  Text="Cancel" id="btnCancel" CssClass="Button_Normal" runat="server" 
									style="width: 80px;" CausesValidation="False"  />&nbsp;
								<%--<asp:Button  Text="Close" id="btnClose" CssClass="Button_Normal" runat="server" 
									style="width: 80px;" CausesValidation="False" />--%>
								
							</td>
							
						</tr>
						</table>
					</td>
									
								</tr>
				</table>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
    </form>
	<%--<script  language="javascript" type="text/javascript" >

		function fnCloseWindow() {
		    window.opener.location.reload();
		    
            
			self.close();
			
			//window.location.href = "TerminationWatcher.aspx";
			//window.opener.location.reload();
			//window.open('TerminationWatcherAddUser.aspx?form=' + Formname, 'YMCAYRS',  'width=750, height=400, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes','');
		}
		</script>--%>
</body>
<!--#include virtual="bottom.html"-->

