
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DeathBenefitFollowupForm.aspx.vb" 
MasterPageFile="~/MasterPages/YRSMain.Master"  Inherits="YMCAUI.DeathBenefitFollowupForm" %>


<asp:Content ContentPlaceHolderID="head" runat="server">
<title>
   
    YMCA YRS
</title>
    <%--   'SP 2014.12.02 BT-2310\YRS 5.0-2255 -Start--%>
    <style type="text/css">

        .BG_ColorRepMissed {
            background-color:lightblue;
        }
        .BG_ColorPercentageMisMatched {
            background-color:Wheat;
          
        }
        .BG_ColorBothRepMissedPercentageMisMatched {
            background-color:LightSalmon;
        }
    </style>
    <%--   'SP 2014.12.02 BT-2310\YRS 5.0-2255 -End--%>
 <%--<script language="javascript" type="text/javascript" src="JS/YMCA_JScript.js"></script>
<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet" />
<script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script>
<link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
<script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>--%>
    <script language="javascript" type="text/javascript">



        function ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector) {
            var totalCheckboxes = $(checkBoxSelector),
                        checkedCheckboxes = totalCheckboxes.filter(":checked"),
                        noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
                        allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);
            if (totalCheckboxes.length == 0) allCheckboxesAreChecked = false;
            $(allCheckBoxSelector).attr('checked', allCheckboxesAreChecked);
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
            //Code for checkbox Check all, start
            var all60CheckBoxSelector = '#<%=gv60DaysFollowUplist.ClientID%> input[id*="chkSelectAll60Days"]:checkbox';
            var checkBox60Selector = '#<%=gv60DaysFollowUplist.ClientID%> input[id*="chkSelect60Days"]:checkbox';
            var checkBoxSelector = '#<%=gvPendingFollowUplist.ClientID%> input[id*="chkSelect"]:checkbox';

            $(checkBoxSelector).bind('click', function () {
                $("#<%=btnSaveResponse.ClientID%>").attr('disabled', false);
               mark_dirty();
           });

           $(all60CheckBoxSelector).bind('click', function () {
               $(checkBox60Selector).attr('checked', $(this).is(':checked'));
               ToggleCheckUncheckAllOptionAsNeeded(all60CheckBoxSelector, checkBox60Selector);
           });


            //            $(checkBox60Selector).bind('click', ToggleCheckUncheckAllOptionAsNeeded(all60CheckBoxSelector,checkBox60Selector) );
           ToggleCheckUncheckAllOptionAsNeeded(all60CheckBoxSelector, checkBox60Selector);
           $(checkBox60Selector).bind('click', function () {
               mark_dirty();
           });

            //Code for checkbox Check all, start
           var all90CheckBoxSelector = '#<%=gv90DaysFollowUplist.ClientID%> input[id*="chkSelectAll90Days"]:checkbox';
            var checkBox90Selector = '#<%=gv90DaysFollowUplist.ClientID%> input[id*="chkSelect90Days"]:checkbox';

            $(all90CheckBoxSelector).bind('click', function () {
                $(checkBox90Selector).attr('checked', $(this).is(':checked'));
                ToggleCheckUncheckAllOptionAsNeeded(all90CheckBoxSelector, checkBox90Selector);
            });


            //            $(checkBox90Selector).bind('click', ToggleCheckUncheckAllOptionAsNeeded(all90CheckBoxSelector, checkBox90Selector));
            ToggleCheckUncheckAllOptionAsNeeded(all90CheckBoxSelector, checkBox90Selector);
            $(checkBox90Selector).bind('click', function () {
                mark_dirty();
            });

            $('#ConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                width: 350, height: 250,
                title: "Death Benefit Application",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

        }

        function showDialog(id, text, btnokvisibility) {


            $('#' + id).dialog({ modal: true });
            $('#lblMessage').text(text);
            $('#' + id).dialog("open");
            if (btnokvisibility == 'NO') {
                $("#btnYes").show();
                $("#btnNo").show();
                $("#btnOK").hide();
                $("#btnCancel").hide();
            }
            else {
                $("#btnYes").hide();
                $("#btnNo").hide();
                $("#btnOK").show();
                $("#btnCancel").hide();

            }
            if (btnokvisibility == 'Cancel') {
                $("#btnYes").hide();
                $("#btnNo").hide();
                $("#btnOK").show();
                $("#btnCancel").show();
            }
        }

        function closeDialog(id) {

            $('#' + id).dialog('close');
        }
        //Code for checkbox Check all, End

        //To show tooltip of each row
        function showToolTip(divId, linkId, Name, DeathStatus) {
            if (null != divId) {
                var elementRef = document.getElementById(divId);
                if (elementRef != null) {
                    elementRef.style.position = 'absolute';
                    elementRef.style.left = event.clientX + 5 + document.body.scrollLeft;
                    elementRef.style.top = event.clientY + 5 + document.body.scrollTop;
                    elementRef.style.width = '380';
                    elementRef.style.visibility = 'visible';
                    elementRef.style.display = 'block';
                }

                if (null != linkId) {


                    var lblNote = document.getElementById("<%=lblComments.ClientID%>");
                        lblNote.innerText = ' ' + Name + '\n' + ' ' + DeathStatus

                }
            }
        }

        //to hide tool tip when mouse is removed
        function hideToolTip(divId, linkId) {
            if (null != divId) {
                var elementRef = document.getElementById(divId);
                if (elementRef != null) {
                    elementRef.style.visibility = 'hidden';
                }
            }
            if (null != linkId) {
                var elementBak = document.getElementById(linkId);
                if (elementBak != null) {

                }
            }
        }
        function disableButton() {
            $('#lblMessage').text('Processing your request...');
            $("#btnYes").hide();
            $("#btnNo").hide();
            $("#btnOK").hide();
            $("#btnCancel").hide();

        }
        function clear_dirty() {
            $('#HiddenFieldDirty').val(false);
        }

    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server" >
<div class="Div_Center" style="width:100%;height:550; " >
    <asp:ScriptManagerProxy  ID="DBScriptManager" runat="server">
    </asp:ScriptManagerProxy>
    <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div id="loading">
                <img id="loadingimage" runat="server" src="images/ajax-loader.gif" alt="Loading..." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
    <div class="Div_Center" style="width:100%">
        <table class="td_withoutborder" align="center" width="100%">
            <tbody>
                <tr>
                    <td>
                         <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table class="td_withoutborder" width="100%">
                                    <tr>
                                        <td id="tdAllApplicant" runat="server" style="background-color: #4172A9; font-family: verdana;
                                            font-weight: bold; font-size: 10pt; color: #ffffff; width: 33%; text-align: center;
                                            border: solid 1px White; border-bottom: none">
                                             <%-- 'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                                            <asp:LinkButton ID="lnkAllApllicant" Text="Pending Follow-up Records" runat="server" Style="font-family: verdana;
                                                font-weight: bold; font-size: 10pt; color: #ffffff"></asp:LinkButton>
                                            <asp:Label ID="lbllnkAllApplicant" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #000000" runat="server" Text="Pending Follow-up Records" Visible="false"></asp:Label>
                                        </td>
                                        <td id="td60Days" runat="server" style="background-color: #4172A9; font-family: verdana;
                                            font-weight: bold; font-size: 10pt; color: #ffffff; width: 33%; text-align: center;
                                            border: solid 1px White; border-bottom: none">
                                            <asp:LinkButton ID="lnk60Days" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #ffffff" Text="60-day Follow-up Letter" runat="server"></asp:LinkButton>
                                            <asp:Label ID="lbl60Days" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #000000" runat="server" Text="60-day Follow-up Letter" Visible="false"></asp:Label>
                                        </td>
                                        <td id="td90Days" runat="server" style="background-color: #4172A9; font-family: verdana;
                                            font-weight: bold; font-size: 10pt; color: #ffffff; width: 34%; text-align: center;
                                            border: solid 1px White; border-bottom: none">
                                            <asp:LinkButton ID="lnk90Days" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #ffffff" Text="90-day Follow-up Letter" runat="server"></asp:LinkButton>
                                            <asp:Label ID="lbl90Days" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #000000" runat="server" Text="90-day Follow-up Letter" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>     
                            </ContentTemplate>
                            <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="lnk60Days" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lnk90Days" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lnkAllApllicant" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>                       
                    </td>
                </tr>                   
                <tr valign="top">
                    <td valign="top" nowrap="nowrap" >
                        <table class="Table_WithBorder" width ="100%">
                            <tr valign="top"" >
                                <td>                   
                                      <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div id="Tooltip" clientidmode="Static" runat="server" style="z-index: 1000; width: auto; border-left: 1px solid silver;
                                                border-top: 1px solid silver; border-right: 2px solid black; border-bottom: 1px solid #cccccc;
                                                padding: 3px; position: absolute; top: 0; left: 0; background: LightYellow; color: black;
                                                display: none; /* does not work in ie6 */	font-size: 7pt; font-family: verdana;
                                                margin: 0; overflow: visible;">
                                                <asp:Label runat="server" ID="lblComments" ClientIDMode="Static" Style="display: block; width: auto; overflow: visible;
                                                    font-size: x-small;"></asp:Label>
                                            </div>
                                            <div style="overflow: auto; width: 100%; height: 360px; text-align: left">
                                                <asp:Label CssClass="Label_Small" ID="LabelStatusNoDataFound" runat="server"  ></asp:Label>
                                                <asp:GridView ID="gvPendingFollowUplist" runat="server" CssClass="DataGrid_Grid" AlternatingRowStyle-Wrap="true"
                                                    EditRowStyle-Wrap="true" EditRowStyle-Width="1000px" EmptyDataRowStyle-Wrap="true" FooterStyle-Wrap="true"
                                                    HeaderStyle-Wrap="true" PagerStyle-Wrap="true" RowStyle-Wrap="true" SelectedRowStyle-Wrap="true"
                                                    Width="100%" SortedAscendingCellStyle-Wrap="true" SortedDescendingCellStyle-Wrap="true"
                                                    AllowSorting="True" AllowPaging="True" PageSize="10" 
                                                    AutoGenerateColumns="False" DataKeyNames="intDBAFId" PagerStyle-Font-Names="Arial"
                                                    PagerStyle-Font-Size="5px">
                                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Width="20px" HeaderText="Response Received" SortExpression="bitResponseReceived" >
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="20px" />
                                                        </asp:TemplateField>
                                                        <%-- Start - MMR | 2016.06.24 | YRS-AT-2674 | Changed width of Participant Fund No. and displaying participant name in grid --%>
                                                        <%--<asp:BoundField HeaderText="Participant Fund No." DataField="intFundIdNo" ItemStyle-Width="120" SortExpression="intFundIdNo"/>--%>
                                                        <asp:BoundField HeaderText="Participant Fund No." DataField="intFundIdNo" ItemStyle-Width="90" SortExpression="intFundIdNo"/>
                                                        <asp:BoundField HeaderText="Participant Name" DataField="ParticipantName" ItemStyle-Width="120" SortExpression="ParticipantName"/>                                                         
                                                        <%-- End - MMR | 2016.06.24 | YRS-AT-2674 | Changed width of Participant Fund No. and displaying participant name in grid --%>
                                                        <asp:BoundField HeaderText="Beneficiary Name" DataField="chvBeneficiaryName" ItemStyle-Width="180" SortExpression="chvBeneficiaryName" ReadOnly="true" ShowHeader="true" Visible="true" />
                                                        <asp:BoundField HeaderText="Original Letter Date" ItemStyle-Width="100" DataField="dtmOriginalDocument" DataFormatString="{0:MM/dd/yyyy}" SortExpression="dtmOriginalDocument" />
                                                        <asp:BoundField HeaderText="60-day Follow-Up Date" ItemStyle-Width="100" DataFormatString="{0:MM/dd/yyyy}"  />
                                                        <asp:BoundField HeaderText="60-day Follow-Up Sent" ItemStyle-Width="100" DataField= "dtm60dayFollowup" DataFormatString="{0:MM/dd/yyyy}" SortExpression="dtm60dayFollowup" />
                                                        <asp:BoundField HeaderText="90-day Follow-Up Date" ItemStyle-Width="100" DataFormatString="{0:MM/dd/yyyy}"  />
                                                        <asp:BoundField HeaderText="90-day Follow-Up Sent" ItemStyle-Width="100" DataField= "dtm90dayFollowup" DataFormatString="{0:MM/dd/yyyy}" SortExpression="dtm90dayFollowup"  />
                                                    </Columns>
                                                    <PagerSettings PageButtonCount="10" FirstPageText="First" LastPageText="Last" />
                                                    <PagerStyle Font-Names="Arial" Font-Size="Small" />
                                            </asp:GridView>     
                                                <asp:GridView ID="gv60DaysFollowUplist" runat="server" CssClass="DataGrid_Grid" AlternatingRowStyle-Wrap="true"
                                                    EditRowStyle-Wrap="true" EditRowStyle-Width="1000px" EmptyDataRowStyle-Wrap="true" FooterStyle-Wrap="true"
                                                    HeaderStyle-Wrap="true" PagerStyle-Wrap="true" RowStyle-Wrap="true" SelectedRowStyle-Wrap="true"
                                                    Width="100%" SortedAscendingCellStyle-Wrap="true" SortedDescendingCellStyle-Wrap="true"
                                                    AllowSorting="True" AllowPaging="True" PageSize="10" 
                                                    AutoGenerateColumns="False" DataKeyNames="intDBAFId" PagerStyle-Font-Names="Arial"
                                                    PagerStyle-Font-Size="5px">
                                                     <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="20px" SortExpression="bitResponseReceived" >
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkSelectAll60Days" runat="server" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect60Days" runat="server" Checked="true" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="20px" />
                                                        </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Participant Fund No." DataField="intFundIdNo" SortExpression="intFundIdNo"/>
                                                    <asp:BoundField HeaderText="Participant Name" DataField="ParticipantName" SortExpression="ParticipantName"/> <%-- MMR | 2016.06.24 | YRS-AT-2674 | Displaying participant name in grid --%> 
                                                    <asp:BoundField HeaderText="Beneficiary Name" DataField="chvBeneficiaryName" SortExpression="chvBeneficiaryName" />
                                                    <asp:BoundField HeaderText="Original Letter Date" DataField="dtmOriginalDocument" DataFormatString="{0:MM/dd/yyyy}" SortExpression="dtmOriginalDocument" />
                                                    <asp:BoundField HeaderText="60-day Follow-Up Date"  DataFormatString="{0:MM/dd/yyyy}" />
                                                </Columns>
                                            </asp:GridView>  
                                                <asp:GridView ID="gv90DaysFollowUplist"  runat="server" CssClass="DataGrid_Grid" AlternatingRowStyle-Wrap="true"
                                                    EditRowStyle-Wrap="true" EditRowStyle-Width="1000px" EmptyDataRowStyle-Wrap="true" FooterStyle-Wrap="true"
                                                    HeaderStyle-Wrap="true" PagerStyle-Wrap="true" RowStyle-Wrap="true" SelectedRowStyle-Wrap="true"
                                                    Width="100%" SortedAscendingCellStyle-Wrap="true" SortedDescendingCellStyle-Wrap="true"
                                                    AllowSorting="True" AllowPaging="True" PageSize="10" 
                                                    AutoGenerateColumns="False" DataKeyNames="intDBAFId" PagerStyle-Font-Names="Arial"
                                                    PagerStyle-Font-Size="5px">
                                                     <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                <Columns>
                                                     <asp:TemplateField HeaderStyle-Width="20px" SortExpression="bitResponseReceived" >
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkSelectAll90Days" runat="server" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect90Days" runat="server" Checked="true" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="20px" />
                                                        </asp:TemplateField>
                                                      <asp:BoundField HeaderText="Participant Fund No." DataField="intFundIdNo" SortExpression="intFundIdNo"/>
                                                    <asp:BoundField HeaderText="Participant Name" DataField="ParticipantName" SortExpression="ParticipantName"/> <%-- MMR | 2016.06.24 | YRS-AT-2674 | Displaying participant name in grid --%> 
                                                    <asp:BoundField HeaderText="Beneficiary Name" DataField="chvBeneficiaryName" SortExpression="chvBeneficiaryName" />
                                                    <asp:BoundField HeaderText="Original Letter Date" DataField="dtmOriginalDocument" DataFormatString="{0:MM/dd/yyyy}" SortExpression="dtmOriginalDocument" />
                                                    <asp:BoundField HeaderText="90-day Follow-Up Date"  DataFormatString="{0:MM/dd/yyyy}" />
                                                </Columns>
                                            </asp:GridView>

                                            <%--   'SP 2014.12.02 BT-2310\YRS 5.0-2255 -Start--%>
                                                <table  style="width: 100% ;vertical-align:bottom" class="Table_WithoutBorder"  runat="server" visible="false" id="tblLengend">
                                                    <tr>
                                                        <td style="width: 2%" class="BG_ColorRepMissed">
                                                            <%--<label class="BG_ColourRepMissed" style="width: 12px"></label>--%>
                                                        </td>
                                                        <td>
                                                            <label class="Label_Small">Representative details are missing.  </label><span class="Normaltext">(Please visit person maintenance to fill representative details.)</span>
                                                        </td> </tr>
                                                     <tr>
                                                         <td style="width: 2%" class="BG_ColorPercentageMisMatched">

                                                         </td>
                                                              <td>
                                                                <label class="Label_Small">Benefit percentage changed.</label><span class="Normaltext">(Please run death calculator.)</span></td>
                                                        </tr>
                                                        <tr>
                                                         <td style="width: 2%" class="BG_ColorBothRepMissedPercentageMisMatched">

                                                         </td>
                                                              <td>
                                                                <label class="Label_Small">Benefit percentage has been modified and also representative details are missing. </label><span class="Normaltext">(Please fill the representative details & run death calculator.)</span></td>
                                                        </tr>

                                                        <%--<tr>
                                                            <td colspan="2">
                                                                <label class="Label_Small">Note: Please visit person maintenance screen to fill representative details.</label></td>
                                                        </tr>--%>
                                                   
                                                </table>
                                                 <%--   'SP 2014.12.02 BT-2310\YRS 5.0-2255 -End--%>
                                            </div>
                                            
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="lnk60Days" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lnk90Days" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lnkAllApllicant" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="Click" />
									        <asp:PostBackTrigger ControlID="btnYes" />
                                            <asp:AsyncPostBackTrigger ControlID="btnNo" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPrint" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSaveResponse" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="gvPendingFollowUplist" EventName="Sorting" />
                                            <asp:AsyncPostBackTrigger ControlID="gv60DaysFollowUplist" EventName="Sorting" />
                                            <asp:AsyncPostBackTrigger ControlID="gv90DaysFollowUplist" EventName="Sorting" />
                                            <asp:AsyncPostBackTrigger ControlID="gvPendingFollowUplist" EventName="PageIndexChanging" />
                                            <asp:AsyncPostBackTrigger ControlID="gv60DaysFollowUplist" EventName="PageIndexChanging" />
                                            <asp:AsyncPostBackTrigger ControlID="gv90DaysFollowUplist" EventName="PageIndexChanging" />
                                            <asp:PostBackTrigger ControlID="btnClose" />
                                        </Triggers>
                                        </asp:UpdatePanel>
                                  </td> 
                           </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td >
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table cellspacing="0" class="Td_ButtonContainer" width="100%" style="height: 23px">
                                <tr>
                                    <td align="center"  >
                                        <asp:Button ID="btnPrint" runat="server" Text="Print"
                                            CssClass="Button_Normal" Style="width: 100px;" />
                                   </td>
                                   <td  align="center">
                                        <asp:Button ID="btnSaveResponse" runat="server" Enabled="false" Text="Save"
                                            CssClass="Button_Normal" Style="width: 120px;" />
                                    </td>                                
                                
                                    <td align="center" >
                                        <asp:Button ID="btnClose" runat="server" UseSubmitBehavior="false" Text="Close" CssClass="Button_Normal Warn_Dirty"
                                            Style="width: 80px;" />
                                        <input id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lnk60Days" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lnk90Days" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lnkAllApllicant" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnPrint" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSaveResponse" EventName="Click" />
                        </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </tbody>
        </table>
        
    </div>
    <div id="ConfirmDialog" style="overflow: visible;">
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
                            <asp:Button runat="server" ID="btnOK" Text="OK" CssClass="Button_Normal" Style="width: 80px;
                                color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                height: 16pt;"  />&nbsp;
                                 <%--Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="Button_Normal"
                                Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt;
                                font-weight: bold; height: 16pt;" />&nbsp;
                            <asp:Button runat="server" ID="btnYes" Text="Yes" OnClientClick="javascript: disableButton();" CssClass="Button_Normal" Style="width: 80px;
                                color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                height: 16pt;"  />&nbsp;
                            <asp:Button runat="server" ID="btnNo" Text="No" CssClass="Button_Normal" Style="width: 80px;
                                color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                height: 16pt;"/>
                        </td>
                    </tr>
                </table>
            </div>
    </div>      
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    
</div>
</asp:Content>