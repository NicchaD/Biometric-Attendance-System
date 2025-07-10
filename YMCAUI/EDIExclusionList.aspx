<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EDIExclusionList.aspx.vb" Inherits="YMCAUI.EDIExclusionList" MasterPageFile="~/MasterPages/YRSMain.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" >
        function ValidateNumeric() {
            if ((event.keyCode < 48) || (event.keyCode > 57)) {
                event.returnValue = false;
            }
        }

        function fncKeyCtrlVStop() {
            // Check if the control key is pressed.
            // If the Netscape way won't work (event.modifiers is undefined),
            // try the IE way (event.ctrlKey)
            var ctrl = typeof event.modifiers == 'undefined' ?
			event.ctrlKey : event.modifiers & Event.CONTROL_MASK;
            // Check if the 'V' key is pressed.
            // If the Netscape way won't work (event.which is undefined),
            // try the IE way (event.keyCode)
            var v = typeof event.which == 'undefined' ?
			event.keyCode == 86 : event.which == 86;

            // If the control and 'V' keys are pressed at the same time
            if (ctrl && v) {
                // ... discard the keystroke and clear the text box
                window.clipboardData.clearData();
                return false;
            }
            return true;
        }

        function openFindWindow() {
            window.open("FindEDIExlcusionInfo.aspx?Name=EDIExclusionList", "YMCAYRS", "width=750,height=550,menubar=no,status=Yes,Resizable=No,top=50,left=70, scrollbars=yes");
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
            $('#ConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                resizable: false,
                close: false,
                width: 450, height: 340,
                title: "EDI Exclusion",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        }

        function showDialog(id, text) {

            $('#' + id).dialog({ modal: true });
            $('#lblMessage').text(text);
            $('#' + id).dialog("open");
            $("#btnYes").show();
            $("#btnNo").show();
        }

        function closeDialog(id) {

            $('#' + id).dialog('close');
        }

        function disableButton() {
            $('#lblMessage').text('Processing your request...');
            $("#btnYes").hide();
            $("#btnNo").hide();
        }
         </script>
</asp:Content>
<asp:Content ID="EDIExclusionListContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="EDIExclusionListScriptManager" runat="server">
    </asp:ScriptManagerProxy>

    <div class="Div_Center">
        <asp:UpdatePanel ID="UpdatePanelEDIExclusion" runat="server">
            <ContentTemplate>
                <table class="Table_WithBorder" align="center" width="100%">
                    <tbody>
                        <tr style="vertical-align:top;">
                            <td style="HEIGHT: 23px" align="left" >
                                <asp:Label ID="lblLookFor" CssClass="Label_Small" Text="Look For SS No.:" runat="server"></asp:Label>&nbsp;<asp:TextBox ID="txtLookFor" Height="20px" CssClass="TextBox_Normal" Width="152px" runat="server"
                                    MaxLength="9"></asp:TextBox>&nbsp;
								        <asp:Button ID="btnSearch" CssClass="Button_Normal" runat="server" Width="56px" Text="Search"></asp:Button>&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" CssClass="Button_Normal" runat="server" Width ="45px" Text ="Clear" />&nbsp;&nbsp;&nbsp;
								        <asp:Label ID="lblNoRecordFound" runat="server" CssClass="Label_Small" Width="112px" Visible="false">No Record Found</asp:Label>
                                <asp:TextBox ID="txtPerssId" runat="server" Height="20px" Width="72px" MaxLength="30" Visible="False"
                                    ReadOnly="True"></asp:TextBox>
                                      
                                    
                                    
                            </td>
                            <td  class="NormalMessageText" style="text-align:right;">
                                        <label class="Error_Message">*</label>Required Fields
                            </td>
                        </tr>
                        <tr valign="top">
                            <td style="WIDTH: 70%;">
                               <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
                                <div style="OVERFLOW: auto; WIDTH: 100%; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 370px; BORDER-BOTTOM-STYLE: none">
                                    <asp:GridView ID="gvEDI" runat="server" CssClass="DataGrid_Grid" Width="100%" AllowSorting="True"
                                        AutoGenerateColumns="False" AllowPaging="true" PageSize="15">
                                        <SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
                                        <AlternatingRowStyle CssClass="DataGrid_AlternateStyle"></AlternatingRowStyle>
                                        <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                        <Columns>
                                            <%--<asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
                                                        CommandName="Select" ToolTip="Select Row"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImagebuttonDelete" runat="server" ImageUrl="images\delete.gif" CausesValidation="False"
                                                        CommandName="Delete" ToolTip="Remove Person From Exclusion List"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="SSNo" SortExpression="SSNo" HeaderText="SS No." ItemStyle-Width="60px"></asp:BoundField>
                                            <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="First Name" ItemStyle-Width="95px"></asp:BoundField>
                                            <asp:BoundField DataField="MiddleName" SortExpression="MiddleName" HeaderText="Middle Name" ItemStyle-Width="95px"></asp:BoundField>
                                            <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="Last Name" ItemStyle-Width="95px"></asp:BoundField>
                                            <asp:BoundField DataField="PerssId" Visible="False" HeaderText="PerssId"></asp:BoundField>
                                            <asp:BoundField DataField="Reason" SortExpression="Reason" HeaderText="Reason"></asp:BoundField>
                                            <asp:BoundField DataField="UniqueId" Visible="False" HeaderText="UniqueId"></asp:BoundField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="15" FirstPageText="First" LastPageText="Last" />
                                            <PagerStyle CssClass="pagination"  />
                                    </asp:GridView>
                                </div>
                               <%-- </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnNo" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnYes" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnnCancel" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        
                                        <asp:AsyncPostBackTrigger ControlID="gvEDI" EventName="Sorting" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                            </td>
                            <td style="width: 30%; HEIGHT: 312px;text-align:right;">
                                <table style="WIDTH: 90%; HEIGHT: 173px;text-align:right;">
                                    
                                    
                                    <tr>
                                        <td colspan="2"  style="text-align:right;">
                                            <asp:Button ID="btnFind" AccessKey="F" Height="24px" CssClass="Button_Normal" Width="135px"
                                                runat="server" Text="Locate a Person ..."></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 96px; HEIGHT: 20px" align="left">
                                            <asp:Label ID="lblFundNo" CssClass="Label_Small" Text="Account Num." runat="server" Width="70">Fund No.</asp:Label></td>
                                        <td style="HEIGHT: 20px">
                                            <asp:TextBox ID="txtFundNo" Height="20px" CssClass="TextBox_Normal" Width="136px" runat="server"
                                                MaxLength="9" AutoPostBack="True"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 96px" align="left">
                                            <asp:Label ID="lblSSNo" CssClass="Label_Small" Text="SS No." runat="server" Width="70"></asp:Label></td>
                                        <td style="HEIGHT: 20px">
                                            <asp:TextBox ID="txtSSNo" Height="20px" CssClass="TextBox_Normal" Width="136px" runat="server"
                                                AutoPostBack="True"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 96px; HEIGHT: 20px" align="left">
                                            <asp:Label ID="lblLastName" CssClass="Label_Small" Text="Last Name" runat="server" Width="70"></asp:Label></td>
                                        <td style="HEIGHT: 20px">
                                            <asp:TextBox ID="txtLastName" runat="server" Height="20px" CssClass="TextBox_Normal" Width="136px"
                                                MaxLength="30" ReadOnly="True" Enabled="false"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 96px; HEIGHT: 20px" align="left">
                                            <asp:Label ID="lblFirstName" CssClass="Label_Small" Text="First Name" runat="server" Width="72px"></asp:Label></td>
                                        <td style="HEIGHT: 20px">
                                            <asp:TextBox ID="txtFirstName" runat="server" Height="20px" CssClass="TextBox_Normal" Width="136px"
                                                MaxLength="30" ReadOnly="True" Enabled="false"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 96px; HEIGHT: 20px" align="left">
                                            <asp:Label ID="lblMiddleName" CssClass="Label_Small" Text="Last Name" runat="server" Width="84px">Middle Name</asp:Label></td>
                                        <td style="HEIGHT: 20px">
                                            <asp:TextBox ID="txtMiddleName" runat="server" Height="20px" CssClass="TextBox_Normal" Width="136px"
                                                MaxLength="30" ReadOnly="True" Enabled="false" ></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="WIDTH: 96px" align="left">
                                            <asp:Label ID="lblReason" CssClass="Label_Small" Text="Message" runat="server" Width="70">Reason  <label class="Error_Message" >*</label></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtMessage" Height="70" CssClass="TextBox_Normal" Width="136px" runat="server"
                                                Rows="3" Columns="1" TextMode="MultiLine"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button ID="btnAdd" AccessKey="A" Height="24" CssClass="Button_Normal" Width="89" runat="server"
                                                Text="Add To List" Enabled="False"></asp:Button>&nbsp;&nbsp;								            
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="Div_Center">
                                    <table class="Table_WithOutBorder" cellspacing="0" cellpadding="0" border="0" width="100%">
                                        <tbody>
                                            <tr>
                                                <td class="Td_ButtonContainer" align="left" width="20%">
                                                    <asp:Button ID="btnReport" AccessKey="R" Height="24" CssClass="Button_Normal" runat="server"
                                                        Width="89px" Text="Report"></asp:Button>
                                                </td>
                                                <td class="Td_ButtonContainer" align="right" width="80%">
                                                    <asp:Button ID="btnSave" AccessKey="S" Height="24px" CssClass="Button_Normal" Width="89px"
                                                        runat="server" Text="Save" Enabled="False"></asp:Button>&nbsp;&nbsp;
											        <%--<asp:Button ID="buttonDelete" AccessKey="D" Height="24" CssClass="Button_Normal" Width="89"
                                                        runat="server" Text="Delete" Visible="false" Enabled="False"></asp:Button>&nbsp;&nbsp;--%>
                                                    <asp:Button ID="btnnCancel" AccessKey="C" Height="24px" CssClass="Button_Normal Warn_Dirty" Width="89px"
                                                        runat="server" Text="Reset" Enabled="False"></asp:Button>&nbsp;&nbsp;
											        <asp:Button ID="btnOK" AccessKey="O" Height="24px" CssClass="Button_Normal Warn_Dirty" Width="89px"
                                                        runat="server" Text="Close"></asp:Button>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </ContentTemplate>           
        </asp:UpdatePanel>
    </div>
    <!--<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>-->
    <div id="ConfirmDialog" title="EDI Exclusion">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
            <ContentTemplate>
                <div>
                    <table style="width:100%;border:none;" class="formlayout formlayout-bg margin-5px-bottom">
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
                            <td width="100%">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom">
                                <asp:Button runat="server" ID="btnYes" Text="Yes" OnClientClick="javascript: disableButton();" CssClass="Button_Normal" Style="width: 80px;
                                    color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                    height: 16pt;" />&nbsp;
                                <asp:Button runat="server" ID="btnNo" Text="No" CssClass="Button_Normal" Style="width: 80px;
                                    color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                    height: 16pt;" />                               
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
    <%--        <Triggers>                
                <asp:AsyncPostBackTrigger ControlID="btnYes" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnNo" EventName="Click" />                
            </Triggers>--%>
        </asp:UpdatePanel>
    </div>
</asp:Content>
