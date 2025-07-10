<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UnfundedUEINs.aspx.vb" Inherits="YMCAUI.UnfundedUEINs" MasterPageFile="~/MasterPages/YRSMain.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="JS/jquery-1.5.1.min.js"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <%-- Start: AA:03.21.2016 YRS-AT-2594 --%>
    <style>
        .NoEmailexists {
            background-color:orange;            
        }
    </style>
    <%-- End: AA:03.21.2016 YRS-AT-2594 --%>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            CheckReadOnly();   //Shilpa N | 2019.03.11 | YRS-AT-4248 | To check Readonly mode and disable the button and add tooltip 
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    BindEvents();
                }
            }
            BindEvents();
        });

        //START : Shilpa N | 2019.03.11 | YRS-AT-4248 | To check Readonly mode and disable the button and add tooltip 
        function CheckReadOnly() {
            var readOnly = document.getElementById('<%= hdnGenerateEmail.ClientID%>').value;
            var btnGenerateEmail = document.getElementById('btnGenerateEmail');
            if (btnGenerateEmail) {
                if (readOnly != '') {
                document.getElementById('btnGenerateEmail').disabled = true;
                var btnGenerateEmail = document.getElementById('btnGenerateEmail');
                btnGenerateEmail.title = readOnly;
            }
        }
    }
    //END : Shilpa N | 2019.03.11 | YRS-AT-4248 | To check Readonly mode and disable the button and add tooltip 

        function BindEvents() {
            $('#divConfirmDialog').dialog({
                autoOpen: false,
                resizable: false,
                draggable: true,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 350, height: 150,
                title: "Confirmation",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

            $('#divSuccess').dialog({
                autoOpen: false,
                resizable: false,
                draggable: true,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 350, height: 150,
                title: "Saving Information",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

            $('#checkboxSelectAllUnfunded').click(function (event) {  //on click 
                //alert(this.id);
                if (this.checked) { // check select status
                    $('input:checkbox:not(:disabled)').each(function () { //loop through each checkbox //AA:03.21.2016 YRS-AT-2594
                        this.checked = true;  //select all checkboxes
                    });
                } else {
                    $('input:checkbox:not(:disabled)').each(function () { //loop through each checkbox //AA:03.21.2016 YRS-AT-2594
                        this.checked = false; //deselect all checkboxes
                    });
                }
            });
            
            $('#divProgress').dialog({
                autoOpen: false,
                resizable: false,
                draggable: true,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 350, height: 150,
                title: "In progress",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        }

        function UncheckMainCheckbox(obj) {
            if (obj.checked == false) {
                $("#checkboxSelectAllUnfunded").attr('checked', false);
            }
        }

        function Validate() {
            valid = true;

            if ($('input[type=checkbox]:checked:not(:disabled)').length == 0) { //AA:03.21.2016 YRS-AT-2594
                alert($('#<%=hdnValidationError.ClientID%>').val());
                valid = false;
            }

            if (valid) {
                OpenDialog('divConfirmDialog');
            }
        }

        function OpenDialog(id) {
            $("#" + id).dialog('open');
        }

        function CloseDialog(id) {
            $("#" + id).dialog('close');
        }
    </script>
    <div class="div_center">
        <asp:UpdatePanel ID="uplLoanOffSetDefaultUnFreeze" runat="server">
            <ContentTemplate>
                <table class="Td_ButtonContainer" style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="width: 100%">List of UEINs</td>
                    </tr>
                </table>
                <table class="Table_WithBorder" cellpadding="0" cellspacing="0" align="center" width="100%">
                    <tr id="trDefaultLoan" runat="server">
                        <td style="text-align: left; width: 100%">
                            <div style="overflow: auto; height: 400px; width: 100%" id="gvUnfundedUEINHeader">
                                <asp:GridView runat="server" ID="gvUnfundedUEIN" CssClass="DataGrid_Grid"
                                    AllowSorting="true" AutoGenerateColumns="false" SelectedRowStyle-VerticalAlign="Top"
                                    Width="98%" EmptyDataText="No records found." OnSorting="gvUnfundedUEIN_Sorting">
                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                    <%--AA:03.21.2016 YRS-AT-2594Commented this line because to show grid highlighted colour <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />--%> 
                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                    <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="bSelection" HeaderStyle-Width="3%" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="checkboxSelectAllUnfunded" runat="server" />
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="checkboxUnfunded" CommandName="Select" onClick="UncheckMainCheckbox(this)"></asp:CheckBox>
                                                <asp:HiddenField runat="server" ID="hdnUEINTransmittalID" Value='<%#Eval("UEINTransmittalsID")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="YMCAName" SortExpression="YMCAName" HeaderText="YMCA" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle Width="52%" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TransmittalNo" SortExpression="TransmittalNo" HeaderText="Transmittal No" HeaderStyle-HorizontalAlign="Left">
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TransmittalDate" SortExpression="TransmittalDate" HeaderText="Transmittal Date" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:MM/dd/yyyy}">
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AmountDue" SortExpression="AmountDue" HeaderText="Amount Due" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <%-- Start: AA:03.21.2016 YRS-AT-2594 --%>
                                        <asp:BoundField DataField="LPAName" SortExpression="LPAName" HeaderText="Amount Due" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="Amount Due" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="YMCAID" SortExpression="Email" HeaderText="Amount Due" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <%-- End: AA:03.21.2016 YRS-AT-2594 --%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%-- Start: AA:03.21.2016 YRS-AT-2594 --%>
                            <table width="100%">
                                <tr>
                                    
                                    <td style="height: 15px;width:5%" runat="server" class="NoEmailexists" >
                                    </td>
                                            
                                    <td style="text-align:left;width:95%">
                                        <Label ID="lbltdNoEmailexists" class="Label_Small">No email exists for the YMCA contacts whose UEIN highlighted with this colour</Label>
                                    </td>                                    
                                </tr>
                            </table>
                            <%-- End: AA:03.21.2016 YRS-AT-2594 --%>
                        </td>
                    </tr>
                </table>
                <table class="Td_ButtonContainer" style="width: 100%">
                    <tr>
                        <td style="text-align: right">
                            <%--<asp:Button runat="server" ID="btnGenerateEmail" Text="Generate Email" CssClass="Button_Normal" OnClientClick='javascript: return OpenDialog()' OnClick="btnGenerateEmail_Click" />--%>
                            <input type="button" value="Generate Email" id="btnGenerateEmail" class="Button_Normal" onclick="Validate()" /> <!-- Shilpa N | 03/11/2019 | YRS-AT-4248 | Assigned Id to use in function.
                            &nbsp;&nbsp;
                            <asp:Button runat="server" ID="btnClose" Text="Close" CssClass="Button_Normal" OnClick="btnClose_Click" />
                        </td>
                    </tr>
                </table>

                <div id="divConfirmDialog" style="display: none;">
                    <table width="100%" border="0">
                        <tr>
                            <td>
                                <div id="divConfirmationMessage" runat="server"
                                    style="color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; height: 100">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom"></td>
                        </tr>
                        <tr>
                            <td align="right">
                                <div style="border: 1px solid #aaaaaa/*{borderColorContent}*/; background: #ffffff/*{bgColorContent}*/ url(images/ui-bg_flat_75_ffffff_40x100.png)/*{bgImgUrlContent}*/ 50%/*{bgContentXPos}*/ 50%/*{bgContentYPos}*/ repeat-x/*{bgContentRepeat}*/; color: #222222/*{fcContent}*/;">
                                </div>
                                <div>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button runat="server" ID="btnProceed" Text="Proceed" CssClass="Button_Normal" OnClientClick="CloseDialog('divConfirmDialog'); OpenDialog('divProgress');" OnClick="btnProceed_Click"
                                                    Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 10pt; font-weight: bold; height: 16pt;" />
                                                &nbsp;
                                    <input type="button" class="Button_Normal" value="Cancel" onclick="CloseDialog('divConfirmDialog')"
                                        style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 10pt; font-weight: bold; height: 16pt;" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="divProgress" style="display: none;">
                    <div>
                        <table width="100%" border="0">
                            <tr>
                                <td>
                                    <span style="color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; height: 60;">Please wait, emails are getting generated.</span>
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
                <asp:HiddenField ID="hdnValidationError" runat="server" Value="0" />
                <asp:HiddenField ID="hdnGenerateEmail" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
