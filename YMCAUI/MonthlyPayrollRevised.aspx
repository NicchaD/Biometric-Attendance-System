<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master" Codebehind="MonthlyPayrollRevised.aspx.vb" Inherits="YMCAUI.MonthlyPayrollRevised" %> <%-- MMR | 2017.01.24 | YRS-AT-3288 | Added Master page reference--%>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%--<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>--%> <%-- MMR | 2017.01.24 | YRS-AT-3288 | Commented existing calendar control--%>
<%--<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>--%> <%-- MMR | 2017.01.24 | YRS-AT-3288 | Commented existing menu reference as master page introduced--%>

<%-- START: MMR | 2017.01.24 | YRS-AT-3288 | Added JS functions, CSS link reference and style sheet for date picker control--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    <script src="JS/jquery-ui/JScript-1.7.2.0.min.js" type="text/javascript"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link id="Link1" href="CSS/CustomStyleSheet.css" type="text/css" runat="server" rel="stylesheet" />

<style type="text/css">
.ui-datepicker select.ui-datepicker-month { width: 30%;}
.ui-datepicker select.ui-datepicker-year { width: 29%;}
</style>
</asp:Content>

<asp:Content ID="MonthlyPayrollProcessContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<%--< !-- # include virtual="top.html"-- >--%>
<script language="javascript" type="text/javascript">

    function BindEvents() {
        $('#ConfirmDialog').dialog({
            autoOpen: false,
            draggable: true,
            close: false,
            modal: true,
            width: 450, maxHeight: 300, height: 305,
            title: "Payroll Process",
            open: function (type, data) {
                $(this).parent().appendTo("form");
            }
        });

        $(document).ready(function () {
            $('#<%=TextboxCheckDate.ClientID%>').datepicker({
                minDate: new Date(1900, 0, 1),               
                showOn: 'button',
                buttonImage: 'images/calendar.gif',
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true, buttonText: 'Click here to select date.'
            })

        });

          <%--START: PK | 12-12-2019 |YRS-AT-4677|Code to display link in success message--%>
        $('#divErrorMsgToDisplay').dialog({
            autoOpen: false,
            draggable: true,
            close: false,
            modal: true,
            width: 600,
            maxHeight: 600,
            height: 500,

            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });
         <%--END: PK | 12-12-2019 |YRS-AT-4677|Code to display link in success message--%>
    }
    
    <%--START: PK| 12-12-2019 |YRS-AT-4677 | Code to display link in success message--%>
    function OpenErrorMsgToDisplay() {
        $("#divErrorMsgToDisplay").dialog("open");
        return false;
    }
    <%--END: PK | 12-12-2019 | YRS-AT-4677 | Code to display link in success message--%>

    function ShowDialog(title, text, img) {
        $('#ConfirmDialog').dialog("option", "title", title);

        $('#divConfirmDialogMessage').html(text);

        $('#imgConfirmDialogInfo').hide();
        $('#imgConfirmDialogOk').hide();
        $('#imgConfirmDialogError').hide();
        $('#trConfirmDialogYesNo').hide();
        $('#trConfirmDialogOK').hide();
        if (img == 'error') {
            $('#imgConfirmDialogError').show();
            $('#trConfirmDialogOK').show();
        }
        else if (img == 'infoYesNo') {
            $('#imgConfirmDialogInfo').show();
            $('#trConfirmDialogYesNo').show();
        }
        else if (img == 'ok') {
            $('#imgConfirmDialogOk').show();
            $('#trConfirmDialogOK').show();
        }        
        $('#ConfirmDialog').dialog("open");
    }

    function closeDialog(id) {
        $('#' + id).dialog('close');
    }
</script>
<%-- END: MMR | 2017.01.24 | YRS-AT-3288 | Added JS functions, CSS link reference and style sheet for date picker control--%>

<%-- START: MMR | 2017.01.24 | YRS-AT-3288 | Commented existing code as master page introduced--%>
<%--<form id="Form1" method="post" runat="server">--%>
	<%--<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">--%>
		<%-- START: MMR | 2017.01.24 | YRS-AT-3288 | Commented existing menu reference as master page introduced--%>
        <%--<tr>
			<td class="Td_BackGroundColorMenu" align="left">
							<cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
								CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover"
								DefaultMouseUpCssClass="mouseup" MenuFadeDelay="2" mouseovercssclass="MouseOver">
								<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
							</cc1:menu>
						</td>
		</tr>--%>
		<%-- END: MMR | 2017.01.24 | YRS-AT-3288 | Commented existing menu reference as master page introduced--%>
        <%--<tr>
			<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
				Monthly Payroll
			</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
		</tr>
	</table>--%>
    <%-- END: MMR | 2017.01.24 | YRS-AT-3288 | Commented existing code as master page introduced--%>
    
    <%-- START: MMR | 2017.01.24 | YRS-AT-3288 | Added div main div control, update panel and script manager to avoid postbacks--%>
    <div class="Div_Center" style="width:100%;height:400px;">
        <asp:ScriptManagerProxy  id="dbScriptManagerProxy" runat="server"> 
        </asp:ScriptManagerProxy> 
        <asp:UpdatePanel ID="upMonthlyPayroll" runat="server" >
            <ContentTemplate>    
    <%-- END: MMR | 2017.01.24 | YRS-AT-3288 | Added div main div control, update panel and script manager to avoid postbacks--%>
                <table width="100%" height="550px" border="0"> <%-- MMR | 2017.01.24 | YRS-AT-3288 | Changed the width from 700 to 100%--%>
		            <tr>
                        <td class="Table_WithBorder" width="60%" valign="top">
                            <table width="100%">
                              <tr>
			                    <td align="left"><asp:label id="LabelCheckDate" runat="server" CssClass="Label_Small" width="110">Check Date:</asp:label></td>
			                    <%-- START: MMR | 2017.01.24 | YRS-AT-3288 | Commented existing date user control and added textbox to display date--%>
                                  <%--<td align="left"><uc1:DateUserControl id="TextboxCheckDate" runat="server"></uc1:DateUserControl></td>--%>
                                <td align="left">
                                    <asp:TextBox id="TextboxCheckDate" ReadOnly="False" ValidationGroup="process" MaxLength="10"  Width="80px" CssClass="TextBox_Normal DateControl" onpaste="return false;" runat="server"></asp:TextBox>
				                    <asp:RequiredFieldValidator id="rfDatevalidation" runat="server" Enabled="True" Display="Dynamic" ControlToValidate="TextboxCheckDate"
				                    ErrorMessage="Date cannot be blank" ValidationGroup="process" CssClass="Error_Message">*</asp:RequiredFieldValidator>
				                    <asp:RangeValidator id="RangeValidatorUCDate" runat="server" Enabled="true" Display="Dynamic" ControlToValidate="TextboxCheckDate" ValidationGroup="process" Type="date" MinimumValue="01/01/1900"   CssClass="Error_Message">Invalid Date</asp:RangeValidator>
                                </td>
			                    <%-- END: MMR | 2017.01.24 | YRS-AT-3288 | Commented existing date user control and added textbox to display date--%>
		                    </tr>
		                    <tr>
			                    <td align="left"><asp:label id="LabelUSCheckNo" runat="server" CssClass="Label_Small" width="128px">Next US Check No:</asp:label></td>
			                    <td align="left"><asp:textbox id="TextboxUSCheckNo" runat="server" CssClass="TextBox_Normal" Width="146px"></asp:textbox></td>
		                    </tr>
		                    <tr>
			                    <td align="left"><asp:label id="LabelCanadianCheckNo" runat="server" CssClass="Label_Small" width="192px">Next Canadian Check No:</asp:label></td>
			                    <td align="left"><asp:textbox id="TextboxCanadianCheckNo" runat="server" CssClass="TextBox_Normal" Width="146px"></asp:textbox></td>
		                    </tr>
		                    <tr>
			                    <td align="left"><asp:label id="LabelProofReport" runat="server" CssClass="Label_Small" width="192px">Proof Report Only</asp:label></td>
			                    <td align="left"><asp:checkbox id="CheckBoxProofReport" runat="server" text=" " Checked="True"></asp:checkbox></td>
		                    </tr>
		                    <%-- START: MMR | 2017.01.24 | YRS-AT-3288 | Commented existing code as changed its design location--%>
                            <%--<tr>
			                    <td class="Td_ButtonContainer" align="right"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" width="80" Text="Cancel"></asp:button></td>
			                    <td class="Td_ButtonContainer" align="left"><asp:button id="ButtonRun" runat="server" CssClass="Button_Normal" width="80" Text="Run"></asp:button></td>
		                    </tr>--%>
		                    <%-- END: MMR | 2017.01.24 | YRS-AT-3288 | Commented existing code as changed its design location--%>
                        </table>
                        </td>
                        <%-- START: MMR | 2017.01.24 | YRS-AT-3288 | Added grid to display duplicate/missing address--%>
                        <td class="Table_WithBorder" valign="top" width="40%" >
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <asp:label id="lblHeaderText" runat="server"></asp:label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <div style="width: 100%; border-top-style: none; border-right-style: none; border-left-style: none;
                                            position: static; height: 500px; border-bottom-style: none;overflow:auto;">
                                            <asp:GridView ID="gvDuplicateMissingAddressDetail" runat="server" CssClass="DataGrid_Grid" AlternatingRowStyle-Wrap="true"
                                                            EditRowStyle-Wrap="true" EditRowStyle-Width="1000px" EmptyDataRowStyle-Wrap="true" FooterStyle-Wrap="true"
                                                            HeaderStyle-Wrap="true" PagerStyle-Wrap="true" RowStyle-Wrap="true" SelectedRowStyle-Wrap="true"
                                                                SortedAscendingCellStyle-Wrap="true" SortedDescendingCellStyle-Wrap="true"
                                                            AllowSorting="True" AllowPaging="false" AutoGenerateColumns="False" Width ="300px">
                                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                            <RowStyle CssClass="DataGrid_NormalStyle" />
                                                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Fund No" HeaderStyle-HorizontalAlign="Center"  DataField="FundId" />                                                   
                                                            <asp:BoundField HeaderText="Reason"  HeaderStyle-HorizontalAlign="Center"  DataField="Reason" />     
                                                        </Columns>                                                                                             
                                            </asp:GridView>
                                        </div>  
                                    </td>
                                </tr>
                            </table>
                        </td>
                    <%-- END: MMR | 2017.01.24 | YRS-AT-3288 | Added grid to display duplicate/missing address--%>
		            </tr>       
                    <tr>
			            <td class="Td_ButtonContainer" align="right" colspan="2">
			            <asp:button id="ButtonRun" runat="server" CssClass="Button_Normal" width="80" Text="Run"></asp:button>
                        <asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" width="80" Text="Close"></asp:button>
			            </td>
	                </tr>
	            </table>
         </ContentTemplate> 
     </asp:UpdatePanel>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
<%--</form>--%>
<%--< !-- # include virtual="bottom.html"-- >--%>
   </div>
    <%-- START: MMR | 2017.01.24 | YRS-AT-3288 | Added to display message in jquery dialog box--%>
    <div id="ConfirmDialog" title="Payroll Process" style="display: none;">
        <div>
            <asp:UpdatePanel ID="upConfirmDialogBox" runat="server"  >
            <ContentTemplate>
                <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithOutBorder" style="text-align:left;">
                    <tr style="height:70px;">
                        <td rowspan="2" style="width:10%;">
                            <img src="images/help48.JPG" style="border-width:0px; display: none;" alt="information" id="imgConfirmDialogInfo" />
                            <img src="images/OK48.JPG" style="border-width:0px; display: none;" alt="OK" id="imgConfirmDialogOk" />
                            <img src="images/error.gif" style="border-width:0px; display: none;" alt="Error" id="imgConfirmDialogError" />
                        </td>
                    </tr>
                    <tr style="height:70px;">
                        <td style="vertical-align:middle;">
                            <div id="divConfirmDialogMessage">                             
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr id="trConfirmDialogYesNo">
                        <td align="center" valign="bottom" colspan="2">
                            <asp:Button runat="server" ID="btnConfirmDialogYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px;
                                color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                height: 16pt;" causesvalidation="False" OnClientClick="javascript: closeDialog('ConfirmDialog');" />&nbsp;
                            <input type="button" ID="btnConfirmDialogNo" value="No" class="Button_Normal" Style="width: 80px;
                                color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                height: 16pt;" onclick="closeDialog('ConfirmDialog');" />
                        </td>
                    </tr>
                    <tr id="trConfirmDialogOK">
                        <td align="center" valign="bottom" colspan="2">
                            <asp:Button runat="server" ID="btnConfirmDialogOk" Text="OK" class="Button_Normal" Style="width: 80px;
                                color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                height: 16pt;" causesvalidation="False"  />
                        </td>
                    </tr>
                </table>
           </ContentTemplate>
        </asp:UpdatePanel>
        </div>
    </div>
<%-- END: MMR | 2017.01.24 | YRS-AT-3288 | Added to display message in jquery dialog box--%>

<%--START: ML | 12-12-2019 |YRS-AT-4677 |Div to display when link in success message is clicked.--%>
<div id="divErrorMsgToDisplay" title="YMCA-YRS Errors/Warnings" style="display: none;">
     <asp:UpdatePanel ID="UpdatePanel1" runat="server"  >
            <ContentTemplate>
    <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0"  height="auto;" width="100%" border="0">
        <tr>
            <td>
                <div style="overflow: auto; height: 115px;">
                       
                    <asp:GridView ID="gvWarnErrorMessage" AllowPaging="false" AllowSorting="true" 
                        runat="server" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="false" EmptyDataText="No records found.">
                        <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                        <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                        <RowStyle CssClass="DataGrid_NormalStyle" />
                        <Columns>
                            <asp:BoundField DataField="intFundNo" ReadOnly="True" HeaderText="Fund No." ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%" />
                            <asp:BoundField DataField="chrType" ReadOnly="True" HeaderText="Message Type" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="6%" />
                            <asp:BoundField DataField="chvErrorDesc" ReadOnly="True" HeaderText="Message Description" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="30%" />
                            </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div style="text-align:justify"><asp:label id="lblNote" runat="server" text=""></asp:label></div> 
            </td>
        </tr>
        <tr>
            <td>
                <hr />
            </td>
        </tr>
        <tr>
            <td align="right">
                <input type="button" value="OK" class="Button_Normal" style="Width:73px;" onclick="closeDialog('divErrorMsgToDisplay');" />                    
            </td>
        </tr>
    </table>
                </ContentTemplate>
         </asp:UpdatePanel> 
</div>
<%--END: ML |12-12-2019 |YRS-AT-4677|Div to display when link in success message is clicked.--%>
</asp:Content>
