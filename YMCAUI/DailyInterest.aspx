<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DailyInterest.aspx.vb" Inherits="YMCAUI.DailyInterest" MasterPageFile="~/MasterPages/YRSMain.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="JavaScript" src="JS/timepicker.js"></script>
    <script language="javascript">
        function disablectrl(e) {
            if (window.event) {
                key = window.event.keyCode;     //IE
                if (window.event.shiftKey)
                    isShift = true;
                else
                    isShift = false;
            }
            else {
                key = e.which;     //firefox
                if (e.shiftKey)
                    isShift = true;
                else
                    isShift = false;
            }

            if (key == 46 || key == 8) {
                window.event.returnValue = false;
            }
            else {
                window.event.returnValue = false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="DailyInterestContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy ID="ManageSessionScriptManager" runat="server">
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanelDailyInterest" runat="server">
        <ContentTemplate>
            <div class="Div_Center">
                <table class="Table_WithBorder" width="100%" height="450px" border="0" cellspacing="0" cellpadding="0">
                    <tr style="vertical-align: top;">
                        <td>
                            <table width="95%">
                                <tr>
                                    <td height="5%" colspan="4">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="left" class="Label_Small" height="5%" width="150px">Last Run Date/Time
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBoxLastRunDateTime" CssClass="TextBox_Normal" runat="server" Width="170" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td align="left" class="Label_Small" width="150px">Last Run Status
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TextBoxStatus" runat="server" CssClass="TextBox_Normal" Width="150" MaxLength="60" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="5%" colspan="4">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="left" height="5%" class="Label_Small">Current Mode
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:DropDownList ID="DropdownlistCurrentMode" runat="server" Width="120" CssClass="DropDown_Normal" AutoPostBack="True">
                                            <asp:ListItem Value="Select" Selected="True">Select</asp:ListItem>
                                            <asp:ListItem Value="Active">Active</asp:ListItem>
                                            <asp:ListItem Value="Suspended">Suspended</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="5%" colspan="4">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="lef" t class="Label_Small" height="5%">Start Time
                                    </td>
                                    <td align="left" width="343px" colspan="3">
                                        <input runat="server" id="tbStartTime" onblur="validateDatePicker(this)" type="text" maxlength="8"
                                            size="8" value="12:00 pm">
                                        <img id="img_StartTime" runat="server" style="CURSOR: hand" onclick="selectTime(this,tbStartTime)"
                                            alt="Pick a Time!" src="images/timepicker.gif" border="0">
                                        <asp:TextBox ID="TextboxScheduler" runat="server" CssClass="TextBox_Normal" ReadOnly="True" MaxLength="60"
                                            Width="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="5%" colspan="4">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="Label_Small">Description
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="TextboxDescription" CssClass="TextBox_Normal" runat="server" Width="230px" Height="40px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="5%" colspan="4">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="left" height="5%">
                                        <asp:CheckBox ID="chkMailValue" CssClass="CheckBox_Normal" runat="server" Text="Send mail on successful completion of Daily Interest process"></asp:CheckBox></td>
                                </tr>
                                <tr>
                                    <td height="5%" colspan="4">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="left" height="5%">
                                        <input type="hidden" runat="server" id="calendar" name="calendar">
                                        <asp:Label ID="LabelDailyInterestprocess" CssClass="Label_Small" runat="server" Text="Start Time"
                                            Visible="False">Message displayed based on selected Start Time</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="50%" colspan="4">&nbsp;</td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Td_ButtonContainer" align="right" height="5%" >
                            <asp:Button ID="ButtonReports" runat="server" CssClass="Button_Normal" Width="70px" Text="Reports"></asp:Button>&nbsp;
                                        <asp:Button ID="ButtonSave" runat="server" CssClass="Button_Normal" Width="70px" Text="Save"></asp:Button>&nbsp;
                                        <asp:Button ID="ButtonOK" runat="server" CssClass="Button_Normal" Width="70px" Text="Close"></asp:Button>
                        </td>
                    </tr>
                </table>

            </div>
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
