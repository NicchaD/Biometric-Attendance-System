<!--Start: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015-->
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="YRSCustomControls" %>
<%--<%@ Register TagPrefix="YRSControls" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>--%>
<!--End: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015-->
<%@ Register TagPrefix="YRSControls" TagName="AddressWebUserControl" Src="UserControls/AddressUserControl_new.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_ToolBar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="~/UserControls/YMCA_Footer_WebUserControl.ascx" %>

<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="RetireesPowerAttorneyWebForm.aspx.vb"
    Inherits="YMCAUI.RetireesPowerAttorneyWebForm" %>

<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<html>
<head>
<title>Power of Attorney</title>

    <script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="JS/YMCA_JScript.js" type="text/javascript"></script>
    <script language="javascript" src="JS/YMCA_JScript_Warn.js" type="text/javascript"></script>
    <link id="Link1" href="CSS/CustomStyleSheet.css" type="text/css" runat="server" rel="stylesheet" />
    <script language="JavaScript">

        function _OnBlur_TextBoxZip() {

            var str = String(document.Form1.all.TextBoxZip.value);
            var _arr = new Array(20);
            var flg = false;

            for (i = 0; i < str.length; i++) {
                _arr[i] = str.substr(i, 1);

            }

            for (i = 0; i < str.length - 1; i++) {

                if (_arr[i].toString() == '.') {
                    flg = true;

                }


            }

            if (flg) {
                alert('Zip cannot contain decimal.');
                return;
            }

            if (isNaN(parseInt(document.Form1.all.TextBoxZip.value))) {
                alert('Zip cannot have characters.');
                document.Form1.all.TextBoxZip.value = 0;
                document.Form1.all.TextBoxZip.focus();
            }



        }
        function funcOnChangeText() {

            document.Form1.ButtonSave.disabled = "";
            document.Form1.ButtonCancel.disabled = "";
            return true;


        }
    </script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <div class="center">
        <table class="Table_WithoutBorder" width="100%" cellspacing="0" border="0">
            <tr>
            <td class="Td_HeadingFormContainer" align="left">
                <YRSControls:YMCA_ToolBar_WebUserControl ID="Toolbar_Control" runat="server" ShowLogoutLinkButton="false"
                    ShowHomeLinkButton="false" ShowReleaseLinkButton="false"/>
                <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" PageTitle="Power of Attorney" runat="server"></YRSControls:YMCA_Header_WebUserControl>
            </td>
        </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div class="Div_Center">
        <table class="Table_WithBorder" height="319" cellspacing="0" cellpadding="0" width="100%"
            border="0">
            <tr valign="top">
                <td width="45%" valign="top">
                    <div style="overflow: scroll; width: 95%; height: 380px; text-align: left">
                        <asp:DataGrid ID="DataGridRetireesAttorney" runat="server" Width="290" CellPadding="1"
                            CellSpacing="0" AutoGenerateColumns="false" CssClass="DataGrid_Grid">
                            <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                            <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                            <Columns>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="Imagebutton6" runat="server" CausesValidation="False" ToolTip="Select"
                                            CommandName="Select" ImageUrl="images\select.gif"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="PoaName" HeaderText="Power of Attorney/Personal Rep/Third Party"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="guiAddressId" HeaderText="AddressID">
                                </asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="guiUniqueID" HeaderText="UniqueID"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
                <td width="55%">
                    <table width="100%">
                        <tbody>
                            <tr valign="top">
                                <td width="100%">
                                    <table width="100%" >
                                        <tbody>
                                            <!--Start: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015-->
                                            <tr>
                                                <asp:ValidationSummary
                                                    ID="VSPowerOfAttorney"
                                                    runat="server"
                                                    DisplayMode="List"
                                                    EnableClientScript="true"
                                                    CssClass="Error_Message" />
                                            </tr>
                                            <!--End: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015-->
                                            <tr>
                                                <td align="left" height="21">
                                                    <asp:Label ID="LabelEffective" runat="server"  CssClass="Label_Small"
                                                        Height="11px">Effective Date</asp:Label>
                                                </td>
                                                <td align="left" height="21">
                                                    <!--Start: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015-->
                                                    <YRSCustomControls:CalenderTextBox ID="TextBoxEffective" MaxLength="10" runat="server" 
                                                        CssClass="TextBox_Normal" EnableRequiredFieldValidator="true" EnableCustomValidator="true" Style="width:85px"
                                                        IsFocusRequired="false" RequiredFieldValidatorMessage="Please enter the effective date." CustomValidatorMessage="Please enter valid effective date."
                                                        TrackDateChange="true" yearRange="1900:c+100"></YRSCustomControls:CalenderTextBox>
                                                    <!--End: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015-->
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="115" height="20">
                                                    <asp:Label ID="LabelTermination" runat="server" CssClass="Label_Small">Termination Date</asp:Label>
                                                </td>
                                                <td align="left" height="20">
                                                    <!--Start: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015-->
                                                    <YRSCustomControls:CalenderTextBox ID="TextBoxTermination" MaxLength="10" runat="server" 
                                                        CssClass="TextBox_Normal" EnableCustomValidator="true" IsFocusRequired="false" Style="width:85px"
                                                        TrackDateChange="true" yearRange="1900:c+100" CustomValidatorMessage="Please enter valid termination date."></YRSCustomControls:CalenderTextBox>
                                                    <!--End: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015-->
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="85" height="28">
                                                    <asp:Label ID="LabelFirstName" runat="server" Width="72px" CssClass="Label_Small">First Name</asp:Label>
                                                </td>
                                                <td align="left" height="28">
                                                    <asp:TextBox ID="TextBoxFirstName" runat="server" Width="232px" CssClass="TextBox_Normal"
                                                        MaxLength="70"></asp:TextBox>
                                                    <!--Start: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015-->
                                                    &nbsp;&nbsp;<asp:RequiredFieldValidator ID="RFVFirstName" runat="server"
                                                        ErrorMessage="Please enter the first name" ControlToValidate="TextBoxFirstName" CssClass="Error_Message">*</asp:RequiredFieldValidator>
                                                    <!--End: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015-->
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="85" height="23">
                                                    <asp:Label ID="LabelLastNAme" runat="server" Width="72px" CssClass="Label_Small">Last Name</asp:Label>
                                                </td>
                                                <td align="left" height="23">
                                                    <asp:TextBox ID="TextBoxLastName" runat="server" Width="232px" CssClass="TextBox_Normal"
                                                        MaxLength="70"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="85" height="23">
                                                    <asp:Label ID="LabePoaCategory" runat="server" Width="72px" CssClass="Label_Small">Category</asp:Label>
                                                </td>
                                                <td align="left" height="23">
                                                    <asp:DropDownList ID="DropDownPoaCategory" runat="server"></asp:DropDownList>
                                                    <!--Start: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015-->
                                                    <asp:RequiredFieldValidator ID="RFVCategory" runat="server" ErrorMessage="Please select a valid category" ControlToValidate="dropDownPoaCategory"
                                                        InitialValue="-- Select --" CssClass="Error_Message">*</asp:RequiredFieldValidator>
                                                    <!--End: Bala: YRS-AT-2534 POA Last Name issue 11/18/2015-->
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Table_WithoutBorder" align="left" colspan="2" >
                                                    <YRSControls:AddressWebUserControl ID="AddressWebUserControl1" runat="server" AllowNote="true" AllowEffDate="false" PopupHeight="930"></YRSControls:AddressWebUserControl>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="85">
                                                    <asp:Label ID="LabelComments" runat="server" Width="50px" CssClass="Label_Small">Comments</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="TextboxComments" runat="server" Width="200" CssClass="TextBox_Normal"
                                                        TextMode="MultiLine" Rows="3" Columns="6" Height="50"></asp:TextBox>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
            ControlToValidate="TextBoxAddress1" Visible="False" Enabled="False"></asp:RequiredFieldValidator>
        <asp:Label ID="LabelAddress1" runat="server" Width="50px" CssClass="Label_Small"
            Visible="False">Address1:</asp:Label>
        <asp:TextBox ID="TextBoxAddress1" runat="server" Width="250" CssClass="TextBox_Normal"
            MaxLength="60" Visible="False"></asp:TextBox>
        <asp:Label ID="LabelAddtress2" runat="server" Width="50px" CssClass="Label_Small"
            Visible="False">Address2:</asp:Label>
        <asp:TextBox ID="TextBoxAddress2" runat="server" Width="250" CssClass="TextBox_Normal"
            MaxLength="60" Visible="False"></asp:TextBox>
        <asp:Label ID="LabelAddress3" runat="server" Width="50px" CssClass="Label_Small"
            Visible="False">Address3:</asp:Label>
        <asp:TextBox ID="TextBoxAddress3" runat="server" Width="250" CssClass="TextBox_Normal"
            MaxLength="60" Visible="False"></asp:TextBox>
        <asp:Label ID="LabelCity" runat="server" Width="50px" CssClass="Label_Small" Visible="False">City:</asp:Label>
        <asp:TextBox ID="TextBoxCity" runat="server" Width="200" CssClass="TextBox_Normal"
            MaxLength="30" Visible="False"></asp:TextBox>
        <asp:Label ID="LabelState" runat="server" Width="50px" CssClass="Label_Small" Visible="False">State:</asp:Label>
        <asp:DropDownList ID="DropDownState" runat="server" CssClass="DropDown_Normal" Width="200"
            Visible="False" AutoPostBack="true" Enabled="false">
        </asp:DropDownList>
        <asp:Label ID="LabelZip" runat="server" Width="50px" CssClass="Label_Small" Visible="False">Zip:</asp:Label>
        <asp:TextBox ID="TextBoxZip" runat="server" Width="200" CssClass="TextBox_Normal"
            MaxLength="10" Visible="False"></asp:TextBox>
        <asp:Label ID="LabelCountry" runat="server" Width="50px" CssClass="Label_Small" Visible="False">Country:</asp:Label>
        <asp:DropDownList ID="DropDownCountry" runat="server" CssClass="DropDown_Normal"
            Width="200" Visible="False" AutoPostBack="true" Enabled="false">
        </asp:DropDownList>
    </div>
    <div class="Div_Center">
        <table width="100%" cellspacing="0" border="0">
            <tr>
                <td class="Td_ButtonContainer" align="center">
                    <asp:Button ID="ButtonSave" CssClass="Button_Normal" Width="80" Enabled="False" runat="server"
                        Text="Save"></asp:Button>
                </td>
                <td class="Td_ButtonContainer" align="center">
                    <asp:Button ID="ButtonCancel" CssClass="Button_Normal" Width="80" Enabled="False"
                        runat="server" Text="Cancel" CausesValidation="False"></asp:Button>
                </td>
                <td class="Td_ButtonContainer" align="center">
                    <asp:Button ID="ButtonAdd1" CssClass="Button_Normal" Width="80" runat="server" Text="Add"
                        CausesValidation="False"></asp:Button>
                </td>
                <td class="Td_ButtonContainer" align="right">
                    <asp:Button ID="ButtonOk" CssClass="Button_Normal" Width="80" runat="server" Text="OK"
                        CausesValidation="False"></asp:Button>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%">
        <tr>
                <td  width="100%">
                    <YRSControls:YMCA_Footer_WebUserControl ID="YMCA_Footer_WebUserControl1" runat="server">
                    </YRSControls:YMCA_Footer_WebUserControl>
                </td>
            </tr>
        
    </table>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    <input id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server" value="" />
    </form>
</body>
</html>

