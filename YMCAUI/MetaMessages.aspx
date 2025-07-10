<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master" CodeBehind="MetaMessages.aspx.vb" Inherits="YMCAUI.MetaMessages" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var strMessage;
        var strDescription;
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    BindEvents();
                }
            }
            BindEvents();
        });
        function OpenDialog(message, description) {
            strMessage = message;
            strDescription = description;
            $('#divEditMessage').dialog('open');
        }
        function BindEvents() {

            $('#divEditMessage').dialog({
                autoOpen: false,
                resizable: false,
                draggable: true,
                closeOnEscape: false,
                close: false,
                width: 600, height: 930,
                title: "Message - Edit",
                modal: true,
                buttons: [{ text: "Save", click: UpdateMessage }, { text: "Close", click: CloseAndClearValueOfDialog }],
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
            function ClearPopControlValue() {
                $("#<%=txtMessageDisplayText.ClientID%>")[0].value = "";
                $("#<%=txtMessageDescription.ClientID%>")[0].value = "";
            }
            function CloseAndClearValueOfDialog() {
                ClearPopControlValue()
                $('#divEditMessage').dialog('close');
            }
            function UpdateMessage() {
                var strMessageDescription = $("#<%=txtMessageDescription.ClientID%>")[0].value;
                var strDisplayText = $("#<%=txtMessageDisplayText.ClientID%>")[0].value;
                var messageNumber = $("#<%=hdnErrorNumber.ClientID%>")[0].value;
                var dynamicParams = $("#<%=hdnDynamicParam.ClientID%>")[0].value;

                strDisplayText = strDisplayText.replace(/^\s+|\s+$/gm, '');
                strMessageDescription = strMessageDescription.replace(/^\s+|\s+$/gm, '');

                if (strDisplayText.length > 500) {
                    ShowMessage("divMessage", "<%= GetMessage(YMCAObjects.MetaMessageList.MESSAGE_META_MESSAGES_MESSAGEDISPLAYTEXT_LENGTH)%> ", "error");
                    return false;
                }
                if(strMessageDescription.length > 500) {
                    ShowMessage("divMessage", "<%= GetMessage(YMCAObjects.MetaMessageList.MESSAGE_META_MESSAGES_MESSAGEDESCRIPTION_LENGTH)%> ", "error");
                    return false;
                }

                if (strMessage.replace(/^\s+|\s+$/gm, '') == strDisplayText && strDescription.replace(/^\s+|\s+$/gm, '') == strMessageDescription) {
                    ShowMessage("divMessage", "<%= GetMessage(YMCAObjects.MetaMessageList.MESSAGE_META_MESSAGES_SAVE_WITHOUT_MODIFIED_MESSAGE)%> ", "error");
                    return false;
                }

                if (IsDynamicParamModified(strDisplayText, dynamicParams) == false) {
                    ShowMessage("divMessage", "<%=GetMessage(YMCAObjects.MetaMessageList.MESSAGE_META_MESSAGES_DYNAMIC_PARAMETER_MODIFIED)%>", "error")
                    return false;
                }
                try {
                    strDisplayText = strDisplayText.replace(/\\/g, "\\\\").replace(/'/g, "\\'").replace(/\s+/g, " ");
                    strMessageDescription = strMessageDescription.replace(/\\/g, "\\\\").replace(/'/g, "\\'").replace(/\s+/g, " ");

                    $.ajax({
                        type: "POST",
                        url: "MetaMessages.aspx/UpdateMessage",
                        data: "{'MessageNo':'" + messageNumber + "','MessageDescription':'" + (strMessageDescription) + "','MessageDisplayText':'" + (strDisplayText) + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var str = msg.d;

                            if (str[0] == "error") {
                                ShowMessage("divMessage", str[1], str[0]);
                            }
                            else if (str[0] == "success") {
                                CloseAndClearValueOfDialog();
                                document.forms(0).submit();

                            }

                        },
                        failure: function (msg) {
                            ShowMessage("divMessage", msg.d, "error");
                            CloseAndClearValueOfDialog();
                        }
                    });
                } catch (e) {
                    alert(e.message);
                }
            }

        }

        function IsDynamicParamModified(strMessage, strPipeSepeartedParams) {
            var params = strPipeSepeartedParams.split('|');
            var inputstring = new String(strMessage);
            var output = inputstring.match(/\${2}.*?\${2}/g);

            if (params.length > 1 || (params.length == 1 && params[0].replace(/\s+/g, "") != "")) {

                //removed all dynamic parameters
                if (output == null) {
                    return false;
                }

                //add / delete of dynamic variable
                if (params.length != output.length)
                    return false;

                //Update of dynamic variable
                for (var i = 0; i < params.length; i++) {
                    if (inputstring.indexOf(params[i]) <= -1)
                    { return false; }
                }
            }
            else if (params.length == 1 && params[0].replace(/\s+/g, "") == "") {
                //added a new parameter and old parameter is not exists
                if (output != null) {
                    return false;
                }
            }


            {
                return true;
            }
        }
        function SetMessagePreview(type) {
            if (type == "Error") {
                $("#divPreview")[0].className = "error-msg";
            }
            else if (type == "Success") {
                $("#divPreview")[0].className = "success-msg";
            }
            else if (type == "Information") {
                $("#divPreview")[0].className = "info-msg";
            }
            else if (type == "Warning") {
                $("#divPreview")[0].className = "warning-msg";
            }
        }

        function showToolTip(divId, linkId, Tooltips) {
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
                var lblNote = document.getElementById("<%=lblComments.ClientID%>");
                if (null != linkId) {
                    // checking whether tooltips exists or not 
                    if (Tooltips != '') {

                        lblNote.innerText = ' ' + Tooltips
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <asp:ScriptManagerProxy ID="ScMgrProxy" runat="server"></asp:ScriptManagerProxy>

    <div class="Div_Center">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="Table_WithBorder" align="center" width="100%" style="height: 440px">
                    <tbody>
                        <tr>
                            <td style="text-align: right">
                                <asp:Button Width="150" runat="server" ID="btnReloadAllMessage" Text="Reload All Messages" ToolTip="Reload messages into memory." CssClass="Button_Normal"></asp:Button>
                            </td>
                        </tr>
                        <tr style="vertical-align: top; height: 40px">
                            <td>

                                <table>
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="lblFilterDisplayText" runat="server" CssClass="Label_Small">Message Text</asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="lblFilterDescription" runat="server" CssClass="Label_Small">Message Description</asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="lblFilterMessageType" runat="server" CssClass="Label_Small">Message Type</asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="lblFilterModuleName" runat="server" CssClass="Label_Small">Module Name</asp:Label>
                                        </td>
                                        <td style="width: 20%"></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox Width="150" runat="server" ID="txtDisplayText" CssClass="TextBox_Normal" MaxLength="500"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox Width="150" runat="server" ID="txtDescription" CssClass="TextBox_Normal" MaxLength="500"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMessageType" CssClass="DropDown_Normal" Width="100" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlModuleName" CssClass="DropDown_Normal" Width="160" runat="server"></asp:DropDownList>
                                        </td>
                                        <td colspan='2' align="center">
                                            <asp:Button Width="80" runat="server" ID="btnMessageFind" Text="Find" CssClass="Button_Normal"></asp:Button>
                                            &nbsp;
													<asp:Button Width="80" runat="server" ID="btnMessageClear" Text="Clear" CssClass="Button_Normal"></asp:Button>
                                        </td>
                                    </tr>
                                </table>


                            </td>
                        </tr>
                        <tr style="vertical-align: top">
                            <td>

                                <asp:GridView ID="gvMessages" runat="server" CssClass="DataGrid_Grid" AlternatingRowStyle-Wrap="true"
                                    EmptyDataRowStyle-Wrap="true" FooterStyle-Wrap="true" HeaderStyle-Wrap="true" PagerStyle-Wrap="true" RowStyle-Wrap="true"
                                    SelectedRowStyle-Wrap="true" SortedAscendingCellStyle-Wrap="true" Width="100%"
                                    SortedDescendingCellStyle-Wrap="true" AllowSorting="True" AutoGenerateColumns="false"
                                    AllowPaging="True" PageSize="15" DataKeyNames="MessageNo">
                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="btnEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "MessageNo") %>' CommandName="Select" ImageUrl="~/images/edits.gif" AlternateText="Click here to edit" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Message Text" ItemStyle-Width="60%" ReadOnly="true"
                                            DataField="DisplayText" SortExpression="DisplayText"></asp:BoundField>
                                        <asp:BoundField HeaderText="Message Description" Visible="false"
                                            DataField="MessageDescription" SortExpression="MessageDescription"></asp:BoundField>
                                        <asp:BoundField HeaderText="Message Type" ItemStyle-Width="12%" ReadOnly="true"
                                            DataField="DisplayMessageType" SortExpression="DisplayMessageType"></asp:BoundField>
                                        <asp:BoundField HeaderText="Module Name" ItemStyle-Width="26%" ReadOnly="true"
                                            DataField="ModuleName" SortExpression="ModuleName"></asp:BoundField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Left" CssClass="pagination" />
                                </asp:GridView>

                            </td>
                        </tr>
                        <tr style="vertical-align: bottom;">
                            <td>
                                <table width="100%" class="Td_ButtonContainer">
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="ButtonCloseForm" CssClass="Button_Normal" Width="80" runat="server" Text="Close" CausesValidation="False"></asp:Button>
                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="divEditMessage" style="display: block">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div id="divMessage" style="width: 98%;">
                    </div>
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr style="vertical-align: top;">
                            <td colspan="2" style="text-align: right;">
                                <label class="Modalpopup">
                                    <label class="Modalpopup Error_Message">*</label>Required Fields
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label_Small" style="vertical-align: top">Message Text
                                <label class="Modalpopup Error_Message">*</label></td>
                            <td>
                                <asp:TextBox ID="txtMessageDisplayText" CausesValidation="false" Height="100px" Width="400px" runat="server" CssClass="Modalpopup TextBox_Normal" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="Label_Small" style="vertical-align: top">Message  Description
                                <label class="Modalpopup Error_Message">*</label></td>
                            <td>
                                <asp:TextBox ID="txtMessageDescription" CssClass="Modalpopup TextBox_Normal" Height="100px" Width="400px" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="Label_Small">Message Type</td>
                            <td>
                                <asp:Label ID="lblMessageType" CssClass="Modalpopup TextBox_Normal" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="Label_Small">Module Name</td>
                            <td>
                                <asp:HiddenField ID="hdnDynamicParam" runat="server" />
                                <asp:HiddenField ID="hdnErrorNumber" runat="server" />
                                <asp:Label ID="lblModuleName" CssClass="Modalpopup  TextBox_Normal" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="Label_Small">Preview</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <div id="divPreview" style="width: 400px">Sample Text.</div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </div>

    <div id="Tooltip" aria-describedby="" runat="server" class="toolTip">
        <asp:Label runat="server" ID="lblComments" Style="display: block; width: 100%; overflow: visible; font-size: x-small;"></asp:Label>
    </div>

</asp:Content>
