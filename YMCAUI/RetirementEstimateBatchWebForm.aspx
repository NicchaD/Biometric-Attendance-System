<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RetirementEstimateBatchWebForm.aspx.vb" Inherits="YMCAUI.RetirementEstimateBatchWebForm" MasterPageFile="~/MasterPages/YRSMain.Master" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="~/UserControls/DateUserControl.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .LblMessageText {
            font-weight: bold;
            font-size: 8pt;
            font-family: Verdana, Tahoma, Arial, Helvetica, sans-serif;
        }
    </style>
    <%--HARSHALA-2012.03.15 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : END--%>
    <script type="text/javascript" language="javascript">

        var eventObjId;
        //'****************************************************************************************************************************
        // Change History
        //****************************************************************************************************************************
        //2012.03.05    Harshala Trimukhe   Bt Id:1007 Batch Estimate observation.
        //****************************************************************************************************************************
        function openReportPrinter() {
            //window.open('FT\\ReportPrinter.aspx', '', 'width=450,height=250, menubar=no, resizable=yes,top=200,left=150, scrollbars=yes, status=yes');
            window.open('FT\\ReportPrinter.aspx', '', 'width=785,height=300, menubar=no, resizable=yes,top=200,left=150, scrollbars=yes, status=yes');
        }

        function openReportViewer() {
            window.open('FT\\ReportViewer.aspx', 'ReportPopUp', 'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
        }

        // 2012.02.09	SP :  Made changes to remove add button & parameter screen
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
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 500, height: 250,
                title: "Retirement Batch Estimate",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

            OpenProgressDialog();
            $('#TextBoxYmcaNo').blur(
                    function () {
                        var diff;
                        var str = $(this).val();
                        var len;
                        var strvalue = null;
                        strvalue = str;
                        len = str.length;

                        if (len == 0) {
                            return false;
                        }
                        else if (len < 6) {
                            diff = (6 - len);
                            for (i = 0; i < diff; i++) {
                                strvalue = "0" + strvalue
                            }

                            $(this).val(strvalue);
                        }

                        return false;
                    });

            $("#lblMsg").dialog({
                modal: true,
                bgiframe: true,
                width: 350, height: 250,
                title: "YRS Message",
                autoOpen: false
            });



            $(allCheckBoxSelector).bind('click', function () {
                $(checkBoxSelector).attr('checked', $(this).is(':checked'));
                ToggleCheckUncheckAllOptionAsNeeded();

            });

            $(checkBoxSelector).bind('click', ToggleCheckUncheckAllOptionAsNeeded);

            ToggleCheckUncheckAllOptionAsNeeded();


            $(allCheckBoxParamSelector).bind('click', function () {
                $(checkBoxParamSelector).attr('checked', $(this).is(':checked'));

                ToggleCheckUncheckAllParamOptionAsNeeded();
            });

            $(checkBoxParamSelector).bind('click', ToggleCheckUncheckAllParamOptionAsNeeded);

            ToggleCheckUncheckAllParamOptionAsNeeded();

            // HARSHALA-2012.03.22 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : START
            $('#ButtonRemoveList').bind('click', function () {
                if ($("#<%=gvParameter.ClientID%> tr").length < 1) {
                    // HARSHALA-2012.03.22 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : START
                    //alert("There are no items to remove.");//commented on 22-sep for BT-1126
                    alertMessage('<%=getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_REMOVE_ITEM") %>');
                    // HARSHALA-2012.03.22 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : END
                }
            });
            // HARSHALA-2012.03.22 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : END

            $('#ButtonExecute').bind('click', function () {
                if ($("#<%=gvParameter.ClientID%> tr").length < 1) {
                    // HARSHALA-2012.03.22 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : START
                    // alert("There are no items.");//commented on 22-sep for BT-1126
                    alertMessage('<%=getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_SELECT_ITEM") %>');
                    // HARSHALA-2012.03.22 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : END
                    return false;
                }
            });

        }




        var $lblMsg = null;
        //jQuery.fx.off = true;
        function alertMessage(msg) {
            if (msg == '') {
                $lblMsg = null;
                
            } else {
                $("#lblMsg").html(msg);
                $('#lblMsg').dialog('option',
                    'buttons', { "OK": function () { $(this).dialog("close"); } });                
                $("#lblMsg").dialog('open');
                
            }
        }

        function confirmMsg(objId, Msg) {
            eventObjId = objId;
            $('#lblMsg').html(Msg);
            $('#lblMsg').dialog('option',
            'buttons', {
                "Yes": function () {
                    __doPostBack(eventObjId, '');
                    $(this).dialog("close");
                },
                "No": function () {
                    $(this).dialog("close");
                }
            });
            $('#lblMsg').dialog('open'); return false;
        }

        // HARSHALA-2012.03.22 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : END
        var allCheckBoxSelector = '#<%=gvFindInfo.ClientID%> input[id*="chkSelectAll"]:checkbox';
        var checkBoxSelector = '#<%=gvFindInfo.ClientID%> input[id*="chkSelect"]:checkbox';

        function ToggleCheckUncheckAllOptionAsNeeded() {
            var totalCheckboxes = $(checkBoxSelector),
                        checkedCheckboxes = totalCheckboxes.filter(":checked"),
                        noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
                        allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);
            if (totalCheckboxes.length == 0) allCheckboxesAreChecked = false;
            $(allCheckBoxSelector).attr('checked', allCheckboxesAreChecked);
        }

                
        //--------------- Datagrid parameter- start  -----------------
        var allCheckBoxParamSelector = '#<%=gvParameter.ClientID%> input[id*="chkAll"]:checkbox';
        var checkBoxParamSelector = '#<%=gvParameter.ClientID%> input[id*="chkParamSelect"]:checkbox';

        function ToggleCheckUncheckAllParamOptionAsNeeded() {
            var totalCheckboxes = $(checkBoxParamSelector),
                        checkedCheckboxes = totalCheckboxes.filter(":checked"),
                        noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
                        allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);
            if (totalCheckboxes.length == 0) allCheckboxesAreChecked = false;
            $(allCheckBoxParamSelector).attr('checked', allCheckboxesAreChecked);
        }

              
        function ChkSave() {
            if ($("#<%=gvParameter.ClientID%> tr").length > 1) {
                //added on 22-sep for BT-1126
                confirmMsg('<%=ButtonSave.UniqueID%>', '<%=getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_SAVE_PARTICIPANTS") %>');
                return false;
            }
            else {
                //alert("There are no items to save.");//commented on 22-sep for BT-1126
                alertMessage('<%=getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_NO_SAVE_PARTICIPANTS") %>');
                return false;
            }
        }
       
        function ChkClearList() {

            if ($('#HiddenFieldSavedListCount').val() == "0") {
                //added on 22-sep for BT-1126
                alertMessage('<%=getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_NO_CLEAR_PARTICIPANTS") %>');
                return false;
            }
            else {
                //if (!confirm("This will remove the participants that are saved in the database. Are your sure you wish to continue?")) return false;                
                //added on 22-sep for BT-1126
                confirmMsg('<%= ButtonClearList.UniqueID %>', '<%=getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_CLEAR_PARTICIPANTS") %>');
                return false;
            }
        }
       
        function ChkShowList() {
            if ($('#HiddenFieldSavedListCount').val() == "0") {
                //added on 22-sep for BT-1126
                alertMessage('<%=getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_NO_SHOW_PARTICIPANTS") %>');
                return false;
            }
            else {
                if ($("#<%=gvParameter.ClientID%> tr").length > 1) {
                    //added on 22-sep for BT-1126
                    confirmMsg('<%= ButtonShowList.UniqueID%>', '<%=getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_SHOW_PARTICIPANTS") %>');
                    return false;
                }
                else {
                    __doPostBack('<%= ButtonShowList.UniqueID%>', '');
                }
            }
        }
       

        //2012.02.09	SP :  Made changes to remove add button & parameter screen
        function CloseConfirmDialog() {
            $('#divConfirmDialog').dialog('close');
        }
        function ShowConfirmDialog(instName, AcctNo) {
            $('#divConfirmDialog').dialog('open');
        }

        function OpenProgressDialog() {
            $('#divProgress').dialog({
                autoOpen: false,
                resizable: false,
                draggable: true,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 550, height: 250,
                title: "Retirement Batch Estimate",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        }
        function ShowProcessingDialog() {
            $("#divProgress").dialog('open');
            $('#labelMessage').text('<%=getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_PROCESSING")%>');
        }
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="dbScriptManagerProxy" runat="server">
    </asp:ScriptManagerProxy>
    <div class="Div_Center">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td>
                    <iewc:TabStrip ID="tabStripRetirementEstimate" runat="server" Visible="true" AutoPostBack="True" causesvalidation="true"
                        TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:79;text-align:center;border:solid 1px White;border-bottom:none"
                        TabHoverStyle="background-color:#93BEEE;color:#4172A9;text-align:center;"
                        TabSelectedStyle="background-color:#93BEEE;color:#000000;text-align:center;" Width="730px" Height="30px">
                        <iewc:Tab Text="Manage List"></iewc:Tab>
                        <iewc:Tab Text="Parameters"></iewc:Tab>
                        <iewc:Tab Text="Exceptions"></iewc:Tab>
                    </iewc:TabStrip>
                </td>
            </tr>
            <tr>
                <td>
                    <iewc:MultiPage ID="MultiPageRetirementEstimate" runat="server" Style="overflow: hidden;">
                        <iewc:PageView>
                            <asp:UpdatePanel ID="upManageList" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="Div_Center">
                                        <table class="Table_WithBorder" width="100%">
                                            <tr>
                                                <td align="left" class="td_Text" colspan="2">Manage List
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left" width="67%">
                                                                <!--<asp:Label ID="LabelNoDataFound" runat="server" Visible="False">No Matching Records</asp:Label>-->
                                                                <div style="overflow: auto; width: 100%; border-top-style: none; border-right-style: none; border-left-style: none; height: 400px; border-bottom-style: none">
                                                                    <%--<DataPagerFindInfo:DataGridPager ID="dgPager" runat="server"></DataPagerFindInfo:DataGridPager>--%>
                                                                    <%--HARSHALA-2012.03.15 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : Added Pager Size 2000 and set AllowPaging to false--%>
                                                                    <asp:GridView ID="gvFindInfo" runat="server" CssClass="DataGrid_Grid" Width="100%" AllowSorting="True" AllowPaging="false" PageSize="15" AutoGenerateColumns="false" EmptyDataText="No Matching Records">
                                                                        <SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
                                                                        <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
                                                                        <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
                                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                        <Columns>
                                                                            <asp:BoundField DataField="FundEventId" HeaderText="FundEventId" SortExpression="FundEventId" />
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSelect" runat="server" ItemStyle-Width="10px" />
                                                                                    <%--<asp:HiddenField ID="hdnValue" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "PersID") %>' />--%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="PersID" HeaderText="PersID" SortExpression="PersID" />
                                                                            <asp:BoundField DataField="SSN" HeaderText="SSN" SortExpression="SSN" ItemStyle-Width="60px" />
                                                                            <asp:BoundField DataField="FundIDNo" HeaderText="Fund No" SortExpression="FundIDNo" ItemStyle-Width="60px" />
                                                                            <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
                                                                            <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName"  />
                                                                            <asp:BoundField DataField="FundStatus" HeaderText="Fund Status" SortExpression="FundStatus" ItemStyle-Width="70px" />
                                                                            <asp:BoundField DataField="Age" HeaderText="Age" HeaderStyle-HorizontalAlign="Left" SortExpression="Age" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="30px" />
                                                                            <asp:BoundField DataField="BirthDate" HeaderText="BirthDate" SortExpression="BirthDate" />
                                                                            <asp:BoundField DataField="Retirement" HeaderText="Retirement" HeaderStyle-HorizontalAlign="Left" SortExpression="Retirement" ItemStyle-Width="85px" />
                                                                            <asp:BoundField DataField="Savings" HeaderText="Savings" HeaderStyle-HorizontalAlign="Left" SortExpression="Savings" ItemStyle-Width="65px" />
                                                                            <asp:BoundField DataField="Counts" HeaderText="Count" HeaderStyle-HorizontalAlign="Left" />
                                                                            <%--                                                                        <asp:TemplateColumn>
								                                            <HeaderTemplate>
								            	                                <asp:ImageButton id="imgAddAll" runat="server" ImageUrl="images\add_all.gif" CausesValidation="False" CommandName="AddAll" ToolTip="Add all items to list"></asp:ImageButton>
								                                            </HeaderTemplate>
								                                            <ItemTemplate>
									                                            <asp:ImageButton id="imgAdd" runat="server" ImageUrl="images\add_row.jpg" CausesValidation="False"
										                                            CommandName="Add"  ToolTip="Add to list"></asp:ImageButton>
								                                            </ItemTemplate>
								                                            </asp:TemplateColumn> 
                                                                            --%>
                                                                        </Columns>
                                                                        <PagerSettings Mode="NumericFirstLast"></PagerSettings>
                                                                        <PagerStyle CssClass="pagination" />
                                                                    </asp:GridView>
                                                                    <%--HARSHALA-2012.03.15 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : START--%>
                                                                    <asp:Label ID="lbl_Search_MoreItems" runat="server" CssClass="LblMessageText" Visible="False"
                                                                        EnableViewState="False" />
                                                                    <%--HARSHALA-2012.03.15 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : END--%>
                                                                </div>
                                                            </td>
                                                            <td align="right" valign="top">
                                                                <table class="Table_WithOutBorder" width="100%" align="right">
                                                                    <tr>
                                                                        <td>Find Person(s)
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table class="Table_WithBorder" width="100%" align="right">
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="LabelFundNo" CssClass="Label_Small" Width="100" runat="server">Fund No.</asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:TextBox ID="TextboxFundNo" runat="server" CssClass="TextBox_Normal" Width="150"
                                                                                            MaxLength="10"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" height="24">
                                                                                        <asp:Label ID="LabelSSNo" CssClass="Label_Small" Width="100" runat="server">SS No.</asp:Label>
                                                                                    </td>
                                                                                    <td align="center" height="26">
                                                                                        <asp:TextBox ID="TextBoxSSNo" runat="server" CssClass="TextBox_Normal" Width="150"
                                                                                            MaxLength="11"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="LabelLastName" CssClass="Label_Small" Width="100" runat="server">Last Name</asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:TextBox ID="TextBoxLastName" runat="server" CssClass="TextBox_Normal" Width="150"
                                                                                            MaxLength="30"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="LabelFirstName" CssClass="Label_Small" Width="100" runat="server">First Name</asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:TextBox ID="TextBoxFirstName" runat="server" CssClass="TextBox_Normal" Width="150"
                                                                                            MaxLength="20"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="LabelAssociation" runat="server" CssClass="Label_Small" Width="100">Association</asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:TextBox ID="TextBoxAssociation" runat="server" CssClass="TextBox_Normal" Width="150"
                                                                                            MaxLength="200"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2" style="text-align: right;">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Button ID="ButtonFind" runat="server" CssClass="Button_Normal" Width="70" Text="Find"
                                                                                                        CausesValidation="False"></asp:Button>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Button ID="ButtonClear" runat="server" CssClass="Button_Normal" Width="70" Text="Clear"
                                                                                                        CausesValidation="False"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>

                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan='2'></td>
                                                                                </tr>
                                                                            </table>


                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: center;">
                                                                            <table class="Table_WithOutBorder" width="100%" align="right">
                                                                                <tr>
                                                                                    <td style="text-align: center;">
                                                                                        <br />
                                                                                        - OR -<br />
                                                                                        <br />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Import Association
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table class="Table_WithBorder" width="100%" align="right">
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="LabelImportAssociation" runat="server" CssClass="Label_Small" Width="100">Association</asp:Label>
                                                                                    </td>
                                                                                    <td align="center">
                                                                                        <asp:TextBox ID="TextBoxYmcaNo" runat="server" CssClass="TextBox_Normal" Width="150" MaxLength="6"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr style="vertical-align:top;">
                                                                                    <td>
                                                                                        <label id="Label1" class="Label_Small" >Exclude Fund Events (do not import the persons with these fund events) </label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <%--<asp:CheckBox ID="chkEXEPE" runat="server" />--%>
                                                                                        <asp:CheckBoxList ID="chkList" runat="server" RepeatLayout="Table" RepeatDirection="Vertical" CssClass="CheckBox_Normal">
                                                                                            <asp:ListItem Text="Pre-Eligible (PE)" Value="PE" Enabled="true" Selected="False"></asp:ListItem>
                                                                                            <asp:ListItem Text="Retired Active (RA)" Value="RA" Enabled="true" Selected="False"></asp:ListItem>
                                                                                        </asp:CheckBoxList>
                                                                                    </td>
                                                                                </tr>
                                                                                <%--<tr>
                                                                                    <td>
                                                                                        <label id="Label2" class="Label_Small" style="width: 100px">Exclude RA</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkEXERA" runat="server" />
                                                                                    </td>
                                                                                </tr>--%>
                                                                                <tr>
                                                                                    <td align="left">&nbsp;</td>
                                                                                    <td align="center">
                                                                                        <asp:Button ID="ButtonImportAssociation" runat="server" CssClass="Button_Normal"
                                                                                            Width="160" CausesValidation="False" Text="Import Association"></asp:Button>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="left">
                                                                <table id="tblShowNotes" runat="server" visible="false">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <span style="background-color: LightPink; width: 2%;">&nbsp;&nbsp;&nbsp;</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblNotes" runat="server" CssClass="Label_Small" Visible="false"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <%-- <asp:Button ID="ButtonAddList" runat="server" CssClass="Button_Normal" Text="Add all" Width='80'
                                                                        CausesValidation="False" />--%>
                                                            <%--<asp:Button ID="ButtonRemoveList" runat="server" CssClass="Button_Normal" Text="Remove from list" Width='140'
                                                                        CausesValidation="False" />   &nbsp;&nbsp;  --%>
                                                            <%-- <asp:Button ID="ButtonSave" runat="server" CssClass="Button_Normal" CausesValidation="False" 
                                                                        Text="Save list" />--%>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="left">
                                                                <%--	<input type="button" id="ButtonMtnToggle" class="Button_Normal" disabled="disabled" value="Toggle" style="margin-left:10px;"/>--%>
                                                                <!--<input id="ButtonMtnSelectAll" type="button" class="Button_Normal" value="Select All" />-->
                                                                <!--<input id="ButtonMtnDeSelectAll" type="button" class="Button_Normal" value="Select None" />-->
                                                                <asp:Button ID="ButtonAddList" runat="server" CssClass="Button_Normal" Text="Add to List" CausesValidation="False" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2"></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ButtonFind" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ButtonClear" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ButtonImportAssociation" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ButtonAddList" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </iewc:PageView>
                        <iewc:PageView>
                            <asp:UpdatePanel ID="upParameters" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="Div_Center">
                                        <table cellspacing="0" cellpadding="0" width="100%" class="Table_WithBorder" style="padding-right: 4px;">
                                            <tr>
                                                <td align="left" class="td_Text" colspan="2">Parameters
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" style="padding-top: 3px;">
                                                    <table class="Table_WithOutBorder" width="100%" height="355px" align="right">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="LabelAge" CssClass="Label_Small" runat="server">Age</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="DropDownListAge" runat="server" Width="90" CssClass="DropDown_Normal"
                                                                    AutoPostBack="false">
                                                                </asp:DropDownList>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="LabelAccount" CssClass="Label_Small" runat="server">Plan Type</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="DropDownListPlanType" runat="server" Width="90" CssClass="DropDown_Normal"
                                                                    AutoPostBack="false">
                                                                    <asp:ListItem Value="A">All (any)</asp:ListItem>
                                                                    <asp:ListItem Value="B">Both</asp:ListItem>
                                                                    <asp:ListItem Value="R">Retirement</asp:ListItem>
                                                                    <asp:ListItem Value="S">Savings</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="LabelRDB" CssClass="Label_Small" runat="server">Retired Death Benefit</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="DropdownlistPercentageToUse" runat="server" ValidationGroup="Saveparam"
                                                                    Width="90" CssClass="DropDown_Normal" AutoPostBack="false">
                                                                </asp:DropDownList>
                                                                %
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="LabelInterestRate" CssClass="Label_Small" runat="server">Interest Rates</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="DropDownlistProjectedYearInterest" ValidationGroup="Saveparam"
                                                                    runat="server" Width="90" CssClass="DropDown_Normal" Visible="true">
                                                                </asp:DropDownList>
                                                                %
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="LabelSalaryPercentage" runat="server" CssClass="Label_Small">Salary Increase</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="DropDownListSalaryPercentage" ValidationGroup="Saveparam" runat="server"
                                                                    Width="90" CssClass="DropDown_Normal" Visible="true">
                                                                </asp:DropDownList>
                                                                %
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="LabelFutureSalaryEffDate" runat="server" CssClass="Label_Small">Future Salary Date</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <uc1:DateUserControl ID="TextBoxFutureSalaryEffDate" FormatValidatorErrorMessage1="Invalid Date"
                                                                    ValidationGroup="Saveparam" runat="server" onpaste="return false"></uc1:DateUserControl>
                                                                <%--<asp:DropDownList ID="DropDownListFutureSalaryEffMon" runat="server"  Width="136px"
                                                                        CssClass="DropDown_Normal" Height="22" Visible="true">
                                                                    </asp:DropDownList>--%>
                                                                <asp:CustomValidator ID="CustomValidatorFutureSalaryDate" ValidationGroup="Saveparam"
                                                                    runat="server" CssClass="Error_Message" ErrorMessage="" OnServerValidate="ValidateFutureSalaryDate"
                                                                    EnableClientScript="True" Display="Dynamic">*</asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span class="Label_Small">Report to print</span>
                                                            </td>
                                                            <%-- Start - Manthan Rajguru:2016.01.14 - YRS 2151:Commented existing one --%>
                                                           <%-- <td align="left"></td>--%>  
                                                        <%--</tr>
                                                        <tr>--%>
                                                            <%-- End - Manthan Rajguru:2016.01.14 - YRS 2151:Commented existing one --%>
                                                            <td align="left">
                                                                <asp:RadioButtonList ID="rdFormToPrint" runat="server" RepeatDirection="Horizontal" CssClass="RadioButton_Normal">
                                                                    <%--<asp:ListItem Value="ANNTYESTCOLOR">Color Form</asp:ListItem>--%> <%-- Manthan Rajguru:2016.01.14 - YRS 2151:Commented to remove Color Form --%>
                                                                    <%--Start:05/19/2015 BJ YRS 5.0-2495 Replace report name from [ANNTYESTLONG] to [ANNTYESTLONG_BATCH]--%>
                                                                    <asp:ListItem Value="ANNTYESTLONG_BATCH" Selected="true">Color Form</asp:ListItem> <%-- Manthan Rajguru:2016.01.14 - YRS 2151:Added Selected property --%>
                                                                    <%--End:05/19/2015 BJ YRS 5.0-2495 Replace report name from [ANNTYESTLONG] to [ANNTYESTLONG_BATCH]--%>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <!-- Start: Bala: 2/2/2016: YRS-AT-2677: Lable change -->
                                                            <td align="left" colspan="2">
                                                                <%--<asp:Label ID="LabelPraAssumption" Text="Assumption" runat="server" ValidationGroup="Saveparam" CssClass="Label_Small"></asp:Label>--%>
                                                                <asp:Label ID="LabelPraAssumption" Text="Assumption - 1" runat="server" ValidationGroup="Saveparam" CssClass="Label_Small"></asp:Label><br />
                                                                <asp:Label ID="LabelSubPraAssumptionForEligible" Text="" runat="server" ValidationGroup="Saveparam" CssClass="Label_Small" style="font-weight : 100"></asp:Label>
                                                            </td>  
                                                            <!-- End: Bala: 2/2/2016: YRS-AT-2677: Lable change -->                                                         
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="left">
                                                                <asp:TextBox ID="TextBoxPraAssumption" runat="server" Width="230" Height="90px" TextMode="MultiLine"
                                                                    CssClass="TextBox_Normal" MaxLength="800"></asp:TextBox>
                                                                <asp:CustomValidator ID="CustomValidatorAssumption" runat="server" ValidationGroup="Saveparam"
                                                                    CssClass="Error_Message" ErrorMessage="" OnServerValidate="ValidateAssumption"
                                                                    EnableClientScript="True" Display="Dynamic">*</asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                        <!-- Start: Bala: 2/2/2016: YRS-AT-2677: Adding assumption for non eligible participants -->
                                                        <tr>
                                                            <td align="left" colspan="2">
                                                                <asp:Label ID="LabelPraAssumptionForNonEligible" Text="Assumption - 2" runat="server" ValidationGroup="Saveparam" CssClass="Label_Small"></asp:Label> <br />
                                                                <asp:Label ID="LabelSubPraAssumptionForNonEligible" Text="" runat="server" ValidationGroup="Saveparam" CssClass="Label_Small" style="font-weight : 100"></asp:Label>
                                                            </td>
                                                            <td align="left">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="left">
                                                                <asp:TextBox ID="TextBoxPraAssumptionForNonEligible" runat="server" Width="230" Height="90px" TextMode="MultiLine"
                                                                    CssClass="TextBox_Normal" MaxLength="800"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">                                                               
                                                                <b class="Label_Small">Note: </b><asp:Label ID="LabelAssumptionHeader" Text="Only one of the two assumptions will be considered during estimation." runat="server" ValidationGroup="Saveparam" CssClass="Label_Small" style="font-weight: 100"></asp:Label><br />
                                                            </td>
                                                        </tr>
                                                        <!-- End: Bala: 2/2/2016: YRS-AT-2677: Adding assumption for non eligible participants -->
                                                        <tr>
                                                            <td colspan="2" align="left">
                                                                <asp:ValidationSummary ID="ValidationSummaryRetirementEstimate" ValidationGroup="Saveparam"
                                                                    runat="server" CssClass="Error_Message"></asp:ValidationSummary>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <%--button save--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right" rowspan="11" valign="top" style="padding-top: 3px;">
                                                    <!-- Start: Bala: 2/82/2016: YRS-AT-2677: Div height is increased -->
                                                    <%--<div style="overflow: scroll; width: 690px; border-top-style: none; border-right-style: none; border-left-style: none; height: 320px; border-bottom-style: none">--%>
                                                    <div style="overflow: scroll; width: 690px; border-top-style: none; border-right-style: none; border-left-style: none; height: 550px; border-bottom-style: none">
                                                        <!-- End: Bala: 2/82/2016: YRS-AT-2677: Div height is increased -->
                                                        <%--<DataPagerFindInfo:DataGridPager ID="DgPagerParam" runat="server" />--%>
                                                        <asp:GridView ID="gvParameter" runat="server" CssClass="DataGrid_Grid" Width="670px" AllowSorting="True" AllowPaging="false" PageSize="10" AutoGenerateColumns="false" EmptyDataText="No records added">
                                                            <SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
                                                            <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
                                                            <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
                                                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                            <Columns>
                                                                <asp:BoundField DataField="FundEventId" HeaderText="FundEventId" SortExpression="FundEventId" />
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkAll" runat="server" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkParamSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PersID" HeaderText="PersID" SortExpression="PersID" />
                                                                <asp:BoundField DataField="SSN" HeaderText="SSN" SortExpression="SSN" />
                                                                <asp:BoundField DataField="FundIDNo" HeaderText="Fund No" SortExpression="FundIDNo" />
                                                                <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
                                                                <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
                                                                <asp:BoundField DataField="FundStatus" HeaderText="FundStatus" SortExpression="FundStatus" />
                                                                <asp:BoundField DataField="Age" HeaderText="Age" SortExpression="Age" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="BirthDate" HeaderText="BirthDate" SortExpression="BirthDate" />
                                                                <asp:BoundField DataField="Retirement" HeaderText="Retirement" SortExpression="Retirement" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="Savings" HeaderText="Savings" SortExpression="Savings" HeaderStyle-HorizontalAlign="Center" />
                                                                <%--
                                                                <asp:TemplateColumn>
								                                    <HeaderTemplate>
								            	                        <asp:ImageButton id="imgRemoveAll" runat="server" ImageUrl="images\remove_all.gif" CausesValidation="False" CommandName="AddAll" ToolTip="Remove all items from list"></asp:ImageButton>
								                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton id="imgRemove" runat="server" ImageUrl="images\delete_row.jpg" CausesValidation="False" CommandName="Remove"  ToolTip="Remove from list"></asp:ImageButton>
								                                    </ItemTemplate>
								                                </asp:TemplateColumn> 
                                                                --%>
                                                            </Columns>
                                                            <PagerSettings Mode="NumericFirstLast"></PagerSettings>
                                                            <PagerStyle CssClass="pagination" />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <%--    <tr>
                                                 <td align="center" colspan="2"> <asp:Button ID="ButtonSaveParamater" ValidationGroup="Saveparam" Visible="false" runat="server" CssClass="Button_Normal" Width="80"
                                                                        Text="Save"></asp:Button> &nbsp; &nbsp;
                                                  <asp:Button ID="ButtonExecute" ValidationGroup="Saveparam" runat="server" CssClass="Button_Normal" Width="70"  Text="Execute"></asp:Button>
                                                  </td>
                                                  <td  align="left" > 
                                                          </td>      
                                                 </tr>--%>
                                            <tr>
                                                <td colspan="2" align="right">
                                                    <br />
                                                    <%--<input  id="ButtonParaToggle" type="button" value="Toggle" disabled="disabled" style="margin-left:5px;" class="Button_Normal"/>--%>
                                                    <!--<input id="ButtonParaSelectAll" type="button" class="Button_Normal" value="Select All" />-->
                                                    <!--<input id="ButtonParaDeSelectAll" type="button" class="Button_Normal" value="Select None" />-->
                                                    <asp:Button ID="ButtonRemoveList" runat="server" CssClass="Button_Normal" Text="Remove from List" CausesValidation="False" />
                                                    <asp:Button ID="ButtonExecute" ValidationGroup="Saveparam" runat="server" CssClass="Button_Normal" Text="Execute" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ButtonRemoveList" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ButtonExecute" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </iewc:PageView>
                        <iewc:PageView>
                            <div class="Div_Center">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                                <ContentTemplate>
                                <table cellspacing="0" cellpadding="0" width="100%" height="450px" class="Table_WithBorder">
                                    <tr>
                                        <td colspan="2" align="left" valign="top" class="td_Text">Exceptions
                                        </td>
                                    </tr>
                                    <tr style="vertical-align:top;">
                                        <td align="left" colspan="2">
                                            <%--<div style="overflow: scroll; width: 100%; border-top-style: none; border-right-style: none; border-left-style: none; position: static; height: 355px; border-bottom-style: none">--%>
                                                <asp:GridView ID="gvExceptions" runat="server" CssClass="DataGrid_Grid" Width="100%" AllowSorting="True" AllowPaging="True" PageSize="20" AutoGenerateColumns="False">
                                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
                                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
                                                    <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="SSN" HeaderStyle-Width="10%" HeaderText="SSN"></asp:BoundField>
                                                        <asp:BoundField DataField="FundNo" HeaderStyle-Width="10%" HeaderText="FundNo"></asp:BoundField>
                                                        <asp:BoundField DataField="FirstName" HeaderStyle-Width="15%" HeaderText="FirstName"></asp:BoundField>
                                                        <asp:BoundField DataField="LastName" HeaderStyle-Width="15%" HeaderText="LastName"></asp:BoundField>
                                                        <asp:BoundField DataField="Reason" HeaderText="Reason"></asp:BoundField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast"></PagerSettings>
                                                    <PagerStyle CssClass="pagination"></PagerStyle>
                                                </asp:GridView>
                                            <%--</div>--%>
                                        </td>
                                    </tr>
                                </table>
                                    </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ButtonRemoveList" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ButtonExecute" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            </div>
                        </iewc:PageView>
                    </iewc:MultiPage>
                </td>
            </tr>
            <%--<tr>
                <td align="right">
                    &nbsp;
                </td>
            </tr>--%>
            <tr>
                <td class="td_Text" align="right">
                    <asp:Button ID="ButtonSave" runat="server" CssClass="Button_Normal" CausesValidation="False"
                        Text="Save this List" OnClientClick="javascript: return ChkSave();" />
                    <asp:Button ID="ButtonClearList" runat="server" CssClass="Button_Normal" Text="Clear Saved List"
                        CausesValidation="False" OnClientClick="javascript: return ChkClearList();" />
                    <asp:Button ID="ButtonShowList" runat="server" CssClass="Button_Normal" Visible="true"
                        CausesValidation="False" Text="Retrieve Saved List" OnClientClick="javascript: return ChkShowList();" />
                    <asp:Button ID="ButtonCancel" runat="server" CssClass="Button_Normal" Width="70"
                        Text="Close" CausesValidation="False"></asp:Button>
                </td>
            </tr>
        </table>
    </div>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    <div class="info message" id="lblMsg" style="display: none; text-align: left; vertical-align: middle; padding-top: 20px;">
    </div>
    <div id="divConfirmDialog" title="Cancel Rollover" style="width: 25px">
        <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
            <tr>
                <td class="Label_Small">
                    <label id="lblMessage"><%=getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_CONFIRM_PROCESS")%></label>
                </td>
            </tr>
            <tr>
                <td>
                    <img title="image" height="20" alt="image" src="images/spacer.gif" width="10" />
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td align="right" valign="bottom">
                    <asp:Button runat="server" ID="btnYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                        OnClientClick="javascript: CloseConfirmDialog(); ShowProcessingDialog();" />&nbsp;                    
                    <input type="button" id="btnNo" value="No"  class="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" onclick="javascript: CloseConfirmDialog();" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divProgress" style="overflow: visible;">
        <div>
            <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                <tr>
                    <td>
                        <asp:Label ID="labelMessage" CssClass="Label_Small" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="HiddenFieldSavedListCount" runat="server" Value="0" />
</asp:Content>
