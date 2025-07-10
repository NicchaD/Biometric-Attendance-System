<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TerminationWatcher.aspx.vb"
    EnableEventValidation="true" MasterPageFile="~/MasterPages/YRSMain.Master" Inherits="YMCAUI.TerminationWatcher" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <!--Included by Shashi Shekhar:2009-12-23:To use the common js function -->
    <script language="javascript" type="text/javascript">

        //to open popup for adding a new person
        function OpenPopUp(Formname) {
            //Anudeep:20.12.2012 Bt-1525-Security Access rights needs to be provided for Termination watcher.
            if (CheckAccess("btnProcess") == false) {
                return false;
            }
            if (document.getElementById("lbllnkTMApplicant")) {
                //Formname = "Status"
                return false;
            }
            else if (document.getElementById("lbllnkWithdrawalApplicant")) {
                Formname = "Withdrawal"
            }
            else if (document.getElementById("lbllnkRetirementApplicant")) {
                Formname = "Retirement"
            }
            var left = (screen.width / 2) - (750 / 2);
            var top = (screen.height / 2) - (450 / 2);

            window.open('TerminationWatcherAddUser.aspx?form=' + Formname, 'YMCAYRS', 'width=800, status=yes, height=557, menubar=no, resizable=no,top=' + top + ',left=' + left + ', scrollbars=yes', '');
        }

        //to open notes popup
        function OpenNotesWindow(TerminationWatcherID, PersID) {
            var left = (screen.width / 2) - (750 / 2);
            var top = (screen.height / 2) - (450 / 2);
            window.open('TerminationWatcherAddNotes.aspx?TerminationID=' + TerminationWatcherID + '&PersID=' + PersID, 'newwindow', 'width=800, status=yes, height=500, menubar=no,resizable=no,top=' + top + ',left=' + left + ', scrollbars=yes', '');

        }

        //to open process pop up or purge popup
        function OpenPopUpProcessPurge(Formname, Action) {
            if (document.getElementById("lbllnkTMApplicant")) {
                Formname = "Status"
            }
            else if (document.getElementById("lbllnkWithdrawalApplicant")) {
                Formname = "Withdrawal"
            }
            else if (document.getElementById("lbllnkRetirementApplicant")) {
                Formname = "Retirement"
            }

            var left = (screen.width / 2) - (750 / 2);
            var top = (screen.height / 2) - (450 / 2);
            window.open('TerminationWatcherProcessPurge.aspx?form=' + Formname + '&Action=' + Action, 'newwindow', 'width=790, status=yes, height=620, menubar=no,location=no,titlebar=no, resizable=no,top=' + top + ',left=' + left + ', scrollbars=yes', '');
            return false;
        }


        //Code for checkbox Check all, start


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

            $('#aDivSearch').click(function () {
                $('#divsearch').slideToggle(100);
            });
            $('#divsearch').hide();

            BindEvents();


        });

        function BindEvents() {
            var allCheckBoxSelector = '#<%=gvStatusList.ClientID%> input[id*="chkSelectAll"]:checkbox';
            var checkBoxSelector = '#<%=gvStatusList.ClientID%> input[id*="chkSelect"]:checkbox';
            $(allCheckBoxSelector).bind('click', function () {
                $(checkBoxSelector).attr('checked', $(this).is(':checked'));
                ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector);
            });


            //        $(checkBoxSelector).bind('click', ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector));
            ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector);

            $(checkBoxSelector).bind('click', function () {
                mark_dirty();
            });




            $('#ConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                width: 350, height: 250,
                title: "Termination Watcher",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });



        }

        function showDialog(id, text, btnokvisibility) {


            $('#' + id).dialog({ modal: true });
            $('#lblMessage').text(text)
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
        function showToolTip(divId, linkId, Tooltips, notes, createdon, createdby, Fundno) {
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
                    // checking whether tooltips exists or not 
                    if (Tooltips != '') {
                        if (notes != '') {
                            lblNote.innerText = ' ' + Tooltips + '\n FundNo: ' + Fundno + '\n Created On: ' + createdon + '\n Created By: ' + createdby + '\n' + ' ' + notes

                        } else {
                            lblNote.innerText = ' ' + Tooltips + '\n FundNo: ' + Fundno + '\n Created On: ' + createdon + '\n Created By: ' + createdby

                        }
                    } else {
                        if (notes != '') {
                            lblNote.innerText = ' FundNo: ' + Fundno + '\n Created On: ' + createdon + '\n Created By: ' + createdby + '\n' + ' ' + notes
                        }
                        else {
                            lblNote.innerText = ' FundNo: ' + Fundno + '\n Created On: ' + createdon + '\n Created By: ' + createdby
                        }


                    }


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
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="TWScriptManager" runat="server">
    </asp:ScriptManagerProxy>
    <%--    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div id="loading">
                <img id="loadingimage" runat="server" src="images/ajax-loader.gif" alt="Loading..." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
    <%--    <table class="Table_WithoutBorder" cellspacing="0" width="710px">
        <tr>
            <td class="Td_BackGroundColorMenu" align="left" style="width: 710px;">
                <cc1:Menu ID="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False"
                    Cursor="Pointer" CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown"
                    DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup" MenuFadeDelay="2"
                    mouseovercssclass="MouseOver">
                    <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
                </cc1:Menu>
            </td>
        </tr>
        <tr>
            <td class="Td_HeadingFormContainer" valign="top" align="left" style="width: 700px;">
                <img title="image" height="10" alt="image" src="images/spacer.gif" width="10" />
                Utilities > Termination Watcher<asp:Label ID="LabelHdr" Height="50%" runat="server"
                    CssClass="Td_HeadingFormContainer"></asp:Label>
            </td>
        </tr>
    </table>--%>
    <div class="Div_Center">
        <table class="td_withoutborder" width="100%">
            <tbody>
                <tr>
                    <td width="100%">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <table class="td_withoutborder" width="100%">
                                    <tr>
                                        <td id="tdTMApplicant" runat="server" style="background-color: #4172A9; font-family: verdana;
                                            font-weight: bold; font-size: 10pt; color: #ffffff; width: 33%; text-align: center;
                                            border: solid 1px White; border-bottom: none">
                                            <%-- 'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                                            <asp:LinkButton ID="lnkTMApllicant" Text="Terminated Applicants" runat="server" Style="font-family: verdana;
                                                font-weight: bold; font-size: 10pt; color: #ffffff"></asp:LinkButton>
                                            <asp:Label ID="lbllnkTMApplicant" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #000000" runat="server" Text="Terminated Applicants"></asp:Label>
                                        </td>
                                        <td id="tdWithdrawal" runat="server" style="background-color: #4172A9; font-family: verdana;
                                            font-weight: bold; font-size: 10pt; color: #ffffff; width: 33%; text-align: center;
                                            border: solid 1px White; border-bottom: none">
                                            <asp:LinkButton ID="lnkWithdrawal" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #ffffff" Text="Withdrawal Applicants" runat="server"></asp:LinkButton>
                                            <asp:Label ID="lbllnkWithdrawalApplicant" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #000000" runat="server" Text="Withdrawal Applicants"></asp:Label>
                                        </td>
                                        <td id="tdRetirement" runat="server" style="background-color: #4172A9; font-family: verdana;
                                            font-weight: bold; font-size: 10pt; color: #ffffff; width: 33%; text-align: center;
                                            border: solid 1px White; border-bottom: none">
                                            <asp:LinkButton ID="lnkRetirement" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #ffffff" Text="Retirement Applicants" runat="server"></asp:LinkButton>
                                            <asp:Label ID="lbllnkRetirementApplicant" Style="font-family: verdana; font-weight: bold;
                                                font-size: 10pt; color: #000000" runat="server" Text="Retirement Applicants"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="td_Text">
                        <table width="100%" class="td_withoutborder">
                            <tr>
                                <td align="left" class="td_Text">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="LabelGenHdr" runat="server" CssClass="td_Text" Text="Terminated Applicants"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right" class="td_Text">
                                    <a style="font-family: verdana; font-weight: bold; font-size: 9pt; text-decoration: underline;
                                        color: Blue; cursor: pointer;" id="aDivSearch">Filter Search</a>
                                </td>
                                <td align="right" width="100px" class="td_Text">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <!--Removed Onclient click event by Anudeep:15.01.2013 for YRS 5.0-1484:New Utility to replace Refund Watcher program -->
                                            <asp:Button ID="btnAdd" CssClass="Button_Normal" Text="Add..." UseSubmitBehavior="false"
                                                runat="server" Style="width: 80px;" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="lnkTMApllicant" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lnkWithdrawal" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lnkRetirement" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divsearch" style="width: 100%">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table class="Table_WithBorder" width="100%">
                                        <tr>
                                            <td align="left" width="65px">
                                                <asp:Label ID="lblserachFundNo" CssClass="Label_Small" runat="server" Text="Fund No."
                                                    Width="65px"></asp:Label>
                                            </td>
                                            <%--Commented by Anudeep:19-10-2012 as per Observations SSno Should not shown
                                            <td width="55px">
												<asp:Label ID="Label1" CssClass="Label_Small" runat="server" Text="SS No." Width="55px"></asp:Label>
											</td>--%>
                                            <td width="100px">
                                                <asp:Label ID="lblserachFirstName" CssClass="Label_Small" runat="server" Text="First Name"
                                                    Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblserachLastName" runat="server" CssClass="Label_Small" Text="Last Name"
                                                    Width="80px"></asp:Label>
                                            </td>
                                            <td width="40px">
                                                <%-- 'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                                                <asp:Label ID="lblShowInvalidRecords" runat="server" CssClass="Label_Small" Text="Include Invalid"
                                                    Width="40px"></asp:Label>
                                            </td>
                                            <td width="40px">
                                                <%-- 'Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                                                <asp:Label ID="lblShowProccesedRecords" runat="server" CssClass="Label_Small" Text="Include Processed"
                                                    Width="40px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEmployment" runat="server" CssClass="Label_Small" Text="Employment"
                                                    Width="40px"></asp:Label>
                                                    <asp:Label ID="lblWatchertype" runat="server" CssClass="Label_Small" Text="Watcher Type"></asp:Label>
                                            </td>
                                            <td width="60px">
                                                <asp:Label ID="lblUnfundedtransactions" runat="server" CssClass="Label_Small" Text="Unfunded Trans."></asp:Label>
                                            </td>
                                            <td width="70px">
                                                <asp:Label ID="lblLastContributionRecieved" runat="server" CssClass="Label_Small" Text="Last Cont. Rcvd." width="70"></asp:Label>
                                            </td>
                                          <td width="45">
                                          <asp:Button ID="btnSearch" Width="45" CssClass="Button_Normal" runat="server" UseSubmitBehavior="true"
                                                    Text="Find" /> 
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="65px">
                                                <asp:TextBox Width="70px" ID="txtStatusFundNo" Text='<%# Session("SearchFundNo")  %>'
                                                    MaxLength="30" OnTextChanged="txtserachFundNo_TextChanged" runat="server" CssClass="TextBox_Normal"
                                                    Font-Underline="False"></asp:TextBox>
                                            </td>
                                            <%--Commented by Anudeep:19-10-2012 as per Observations SSno Should not shown--%>
                                            <%--<td width="55px">
												<asp:TextBox Width="70px" ID="txtStatusSSno" Text='<%# Session("SearchSSno") %>'
													MaxLength="9" OnTextChanged="txtserachSSN_TextChanged" runat="server" ValidationGroup="Search" CssClass="TextBox_Normal"></asp:TextBox>
											</td>--%>
                                            <td width="100px">
                                                <asp:TextBox ID="txtStatusFirstName" Width="100px" Text='<%# Session("SearchFirstName") %>'
                                                    OnTextChanged="txtserachFirstName_TextChanged" ValidationGroup="Search" MaxLength="20"
                                                    CssClass="TextBox_Normal" runat="server"></asp:TextBox>
                                            </td>
                                            <td width="100px">
                                                <asp:TextBox ID="txtStatusLastName" Width="100px" Text='<%# Session("SearchLastName")  %>'
                                                    OnTextChanged="txtserachLastName_TextChanged" ValidationGroup="Search" EnableViewState="true"
                                                    MaxLength="30" CssClass="TextBox_Normal" runat="server"></asp:TextBox>
                                            </td>
                                            <td width="50x">
                                                <asp:CheckBox ID="chkInvalidRecord" runat="server" CssClass="CheckBox_Normal" />
                                            </td>
                                            <td width="50px">
                                                <asp:CheckBox ID="chkProccessedRecords" runat="server" CssClass="CheckBox_Normal" />
                                            </td>
                                            <td width="50px">
                                                <asp:DropDownList ID="ddlEmployment" runat="server" CssClass="DropDown_Normal">
                                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                    <asp:ListItem Text="Active" Value="AE"></asp:ListItem>
                                                    <asp:ListItem Text="Inactive" Value="IAE"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlWatcherType" runat="server" CssClass="DropDown_Normal">
                                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                    <asp:ListItem Text="Withdrawal" Value="Withdrawal"></asp:ListItem>
                                                    <asp:ListItem Text="Retirement" Value="Retirement"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td> 
                                            <td>                                                
                                                <asp:DropDownList ID="ddlUnFundedtransactions" runat="server" CssClass="DropDown_Normal">
                                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlLastContributionRecieved" runat="server" CssClass="DropDown_Normal">
                                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td width="45">
                                                 <asp:Button ID="btnClear" UseSubmitBehavior="false" CssClass="Button_Normal" runat="server"
                                                    Visible="true" Text="Clear" />
                                            </td>
                                            
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr valign="top">
                    <td valign="top" nowrap="nowrap" width="100%">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        <table class="Table_WithBorder" width="100%" style="height:300px;">
                            <tr valign="top">
                                <td width="100%">

                                            <div id="Tooltip" runat="server" style="z-index: 1000; width: 100%; border-left: 1px solid silver;
                                                border-top: 1px solid silver; border-right: 2px solid black; border-bottom: 1px solid #cccccc;
                                                padding: 3px; position: absolute; top: 0; left: 0; background: LightYellow; color: black;
                                                display: none; /* does not work in ie6 */	font-size: 7pt; font-family: verdana;
                                                margin: 0; overflow: visible;">
                                                <asp:Label runat="server" ID="lblComments" Style="display: block; width: 100%; overflow: visible;
                                                    font-size: x-small;"></asp:Label>
                                            </div>
                                            
                                            <asp:Label CssClass="Label_Small" ID="LabelStatusNoDataFound" runat="server">No Records Found</asp:Label>
                                            <!--width changed Anudeep:15.01.2013 for YRS 5.0-1484:New Utility to replace Refund Watcher program -->
                                            <asp:GridView ID="gvStatusList" runat="server" CssClass="DataGrid_Grid Warn" AlternatingRowStyle-Wrap="true"
                                                EditRowStyle-Wrap="true" EditRowStyle-Width="1000px" EmptyDataRowStyle-Wrap="true"
                                                FooterStyle-Wrap="true" HeaderStyle-Wrap="true" PagerStyle-Wrap="true" RowStyle-Wrap="true"
                                                SelectedRowStyle-Wrap="true" SortedAscendingCellStyle-Wrap="true"
                                                SortedDescendingCellStyle-Wrap="true" AllowSorting="True" AllowPaging="True"
                                                PageSize="15" OnPageIndexChanging="gvStatusList_OnPageIndexChanging" AutoGenerateColumns="False"
                                                DataKeyNames="UniqueId" PagerStyle-Font-Names="Arial" PagerStyle-Font-Size="5px">
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                <%--<AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />--%>
                                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                                <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundField HeaderText="PersonID" DataField="guiPersID" Visible="false" />
                                                    <asp:BoundField HeaderText="FundEventID" DataField="guiFundEventId" Visible="false" />
                                                    <asp:BoundField HeaderText="TerminationWatcherUniqueId" DataField="UniqueId" Visible="false" />
                                                    <asp:TemplateField HeaderStyle-Width="20px">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="20px" />
                                                    </asp:TemplateField>
                                                    <asp:CommandField HeaderStyle-Width="20px" ButtonType="Image" Visible="true" ShowDeleteButton="true"
                                                        DeleteImageUrl="~/images/delete.gif" DeleteText="Delete" />
                                                    <%-- 'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                                                    <asp:CommandField ButtonType="Image" Visible="true" ShowEditButton="true" CancelImageUrl="images\Cancel-Record.jpg"
                                                        EditImageUrl="images\edits.gif" UpdateImageUrl="images\Save.jpg" CancelText="Cancel"
                                                        EditText="Edit" UpdateText="Save" />
                                                    <asp:TemplateField HeaderStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgButtonNote" runat="server" ToolTip="Add/View Notes" CausesValidation="False"
                                                                ImageUrl="images\notes.jpg" AlternateText="AddNote"></asp:ImageButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="20px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Fund No." HeaderStyle-Width="65Px" ItemStyle-Width="65px"
                                                        SortExpression="FundNo" ReadOnly="true" DataField="FundNo">
                                                        <ItemStyle Width="65px" />
                                                    </asp:BoundField>
                                                    <%--Changed by Anudeep:19-10-2012 Visible="False" as per Observations SSno Should not shown--%>
                                                    <asp:BoundField HeaderText="SS No." Visible="false" ItemStyle-Width="55px" ReadOnly="true"
                                                        DataField="chrSSNo" SortExpression="chrSSNo">
                                                        <ItemStyle Width="55px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="FirstName" ReadOnly="true" DataField="FirstName" SortExpression="FirstName"
                                                        Visible="false" />
                                                    <%-- 'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                                                    <asp:BoundField HeaderText="Name" ReadOnly="true" ItemStyle-Width="170px" DataField="LastName"
                                                        SortExpression="LastName" />
                                                    <asp:BoundField HeaderText="Fund Status" HeaderStyle-Width="40px" ReadOnly="true"
                                                        DataField="Status" SortExpression="Status"></asp:BoundField>
                                                    <%-- Start Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                                                    <asp:BoundField HeaderText="Last Cont. Rcvd." ReadOnly="true" DataField="LastcontributionREceived"
                                                        SortExpression="LastcontributionREceived" />
                                                    <asp:BoundField HeaderText="Unfunded Trans." ReadOnly="true" DataField="UnfundedTransactions"
                                                        SortExpression="UnfundedTransactions" />
                                                    <asp:BoundField HeaderText="Watch Type" HeaderStyle-Width="85px" ItemStyle-Width="85px"
                                                        ReadOnly="true" DataField="Type" SortExpression="Type"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Plan" SortExpression="PlanType">
                                                        <%-- 'End,Anudeep:06-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPlantype" runat="server" Text='<%# Bind("PlanType") %>'></asp:Label>
                                                            <asp:DropDownList runat="server" CssClass="DropDown_Normal" ID="drdPlanType" Visible="false"
                                                                AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblPlantype" runat="server" Visible="false" Text='<%# Bind("PlanType") %>'></asp:Label>
                                                            <asp:DropDownList runat="server" CssClass="DropDown_Normal" ID="drdPlanType">
                                                            </asp:DropDownList>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Balance(s)" HeaderStyle-Width="90px" ReadOnly="true"
                                                        DataField="Balance" SortExpression="Balance" DataFormatString="{0:##,##0.00}"
                                                        ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Right" Width="65px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Status" ReadOnly="true" DataField="Processed" Visible="True"
                                                        SortExpression="Processed" />
                                                    <asp:BoundField HeaderText="Termn. Date" ReadOnly="true" DataField="Termn. Date"
                                                        DataFormatString="{0:MM/dd/yyyy}" Visible="true" SortExpression="Termn. Date" />
                                                    <asp:BoundField HeaderText="Action Taken Date" ReadOnly="true" DataField="Action Taken Date"
                                                        DataFormatString="{0:MM/dd/yyyy}" Visible="true" SortExpression="Action Taken Date" />
                                                    <asp:BoundField HeaderText="Created On" ReadOnly="true" HeaderStyle-Width="75px"
                                                        DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="65px" DataField="CreatedDate"
                                                        Visible="true" SortExpression="CreatedDate" />
                                                    <%--Anudeep:12.12.2012 Changes made to show source of watcher --%>
                                                    <asp:BoundField HeaderText="Source" ReadOnly="true" DataField="Source" Visible="true"
                                                        SortExpression="Source" />
                                                    <asp:BoundField HeaderText="Remarks" ReadOnly="true" Visible="True" />
                                                    <%-- 'Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="First" LastPageText="Last" />
                                                <PagerStyle CssClass="pagination"  />
                                            </asp:GridView>
                                                
                                            &nbsp; &nbsp; &nbsp; &nbsp;
                                            <asp:Label runat="server" ID="lblSelctedrecords" Visible="true" CssClass="Label_Small"></asp:Label>
                                            
                                      
                                </td>
                            </tr>
                        </table>
                        <asp:Label runat="server" ID="lblTotalCount" CssClass="Label_Small"></asp:Label>
                          </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="gvStatusList" EventName="RowCancelingEdit" />
                                <asp:AsyncPostBackTrigger ControlID="gvStatusList" EventName="RowDataBound" />
                                <asp:AsyncPostBackTrigger ControlID="gvStatusList" EventName="RowDeleting" />
                                <asp:AsyncPostBackTrigger ControlID="gvStatusList" EventName="RowEditing" />
                                <asp:AsyncPostBackTrigger ControlID="gvStatusList" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="Click" />
                                <%--Anudeep:07-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnYes" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnNo" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnProcess" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lnkTMApllicant" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lnkWithdrawal" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lnkRetirement" EventName="Click" />
                                <%--Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                                <asp:AsyncPostBackTrigger ControlID="chkInvalidRecord" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chkProccessedRecords" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEmployment" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <!--Code Added by Anudeep:09.01.2013 Bt-1545:YRS 5.0-1762:Color code lines in grid if unfunded transactions exist,start -->
                        <%-- Commented by Anudeep:19-10-2012 as per Observations Colours Should not shown For Last Contribution and Unfunded Transaction Recieved ,Start --%>
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <table width="50%">
                                <tr>
                                    <%--<td valign="top">
									    <table class="Table_WithBorder">
										    <tr>
											    <td width="15px" style="height: 15px;" runat="server" id="tdLastcontri">
											    </td>
										    </tr>
									    </table>
								    </td>
                                    <td>
									    <asp:Label ID="lbltdLastcontri" runat="server" CssClass="Label_Small"></asp:Label>
								    </td>--%>
                                    <td valign="top">
                                        <table>
                                            <tr>
                                                <td width="15px" style="height: 15px;" runat="server" id="tdUnfunded">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltdUnfunded" runat="server" CssClass="Label_Small"></asp:Label>
                                    </td>
                                    <%--<td valign="top">
									    <table class="Table_WithBorder">
										    <tr>
											    <td width="15px" style="height: 15px;" runat="server" id="tdUnfundedandLastContri">
											    </td>
										    </tr>
									    </table>
								    </td>
								    <td>
									    <asp:Label ID="lbltdUnfundedandLastContri" runat="server" CssClass="Label_Small"></asp:Label>
								    </td>--%>
                                </tr>
                            </table>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                        <%-- Commented by Anudeep:19-10-2012 as per Observations Colours Should not shown For Last Contribution and Unfunded Transaction Recieved ,End --%>
                        <!--Code Added by Anudeep:09.01.2013 Bt-1545:YRS 5.0-1762:Color code lines in grid if unfunded transactions exist,start -->
                        <asp:Label ID="lblNote" runat="server" CssClass="Label_Small"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table cellspacing="0" width="100%" style="height: 23px">
                            <tr>
                                <td align="left" class="Td_ButtonContainer">
                                    <asp:Button ID="btnRefresh" UseSubmitBehavior="false" runat="server" Text="Refresh"
                                        CssClass="Button_Normal" Style="width: 80px;" />
                                </td>
                                 <td align="center" class="Td_ButtonContainer">
                                    <asp:Button ID="btnPurge" OnClientClick="OpenPopUpProcessPurge('','Purge')" runat="server"
                                        UseSubmitBehavior="false" Text="Purge..." CssClass="Button_Normal" Style="width: 80px;" />
                                    <asp:Button ID="btnProcessAll" runat="server" UseSubmitBehavior="false" Text="Process All"
                                        CssClass="Button_Normal"  />
                             
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnProcess" runat="server" UseSubmitBehavior="false" Text="Process Selected"
                                        CssClass="Button_Normal"/>
                             
                                
                                </td>
                                <%--Commented by Anudeep:19-10-2012 as per Observations Colours Should not shown For Last Contribution and Unfunded Transaction Recieved ,start
                                <td align="left" class="Td_ButtonContainer">
									<asp:Button ID="btnShowProcess" OnClientClick="OpenPopUpProcessPurge('','Process')"
										runat="server" UseSubmitBehavior="false" Text="Show Processed..." CssClass="Button_Normal"
										Style="width: 130px;" visible="false" />
								</td>
								<td align="left" class="Td_ButtonContainer">
									<asp:Button ID="btnShowInvalid" runat="server" OnClientClick="OpenPopUpProcessPurge('','Invalid')"
										UseSubmitBehavior="false" Text="Show Invalid Records..." CssClass="Button_Normal"
										Style="width: 160px;" visible="false" />
								</td>
                                Commented by Anudeep:19-10-2012 as per Observations Colours Should not shown For Last Contribution and Unfunded Transaction Recieved ,End --%>
                                <td align="right" class="Td_ButtonContainer">
                                    <asp:Button ID="btnClose" runat="server" UseSubmitBehavior="false" Text="Close" CssClass="Button_Normal Warn_Dirty"
                                        Style="width: 80px;" />
                                    <input id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server"
                                        clientidmode="Static">
                                </td>
                            </tr>
                        </table>
                     </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lnkTMApllicant" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lnkWithdrawal" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lnkRetirement" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="ConfirmDialog" title="Termination Watcher" style="overflow: visible;">
            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
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
                                    <asp:Button runat="server" ID="btnOK" Text="OK" CssClass="Button_Normal" Style="width: 80px;
                                        color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                        height: 16pt;" OnClick="btnOK_Click" />&nbsp;
                                    <%--Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="Button_Normal"
                                        Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt;
                                        font-weight: bold; height: 16pt;" />&nbsp;
                                    <asp:Button runat="server" ID="btnYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px;
                                        color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                        height: 16pt;" OnClick="btnYes_Click" />&nbsp;
                                    <asp:Button runat="server" ID="btnNo" Text="No" CssClass="Button_Normal" Style="width: 80px;
                                        color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                        height: 16pt;" OnClick="btnNo_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="gvStatusList" EventName="RowCancelingEdit" />
                    <asp:AsyncPostBackTrigger ControlID="gvStatusList" EventName="RowDataBound" />
                    <asp:AsyncPostBackTrigger ControlID="gvStatusList" EventName="RowDeleting" />
                    <asp:AsyncPostBackTrigger ControlID="gvStatusList" EventName="RowEditing" />
                    <asp:AsyncPostBackTrigger ControlID="gvStatusList" EventName="RowUpdating" />
                    <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="Click" />
                    <%--Anudeep:07-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnYes" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnNo" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnProcess" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lnkTMApllicant" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lnkWithdrawal" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lnkRetirement" EventName="Click" />
                    <%--Anudeep:05-nov-2012 Changes made as per Observations listed in bugtraker for yrs 5.0-1484 --%>
                    <asp:AsyncPostBackTrigger ControlID="chkInvalidRecord" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chkProccessedRecords" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlEmployment" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
</asp:Content>
