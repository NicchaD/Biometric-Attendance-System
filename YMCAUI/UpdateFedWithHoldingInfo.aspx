<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UpdateFedWithHoldingInfo.aspx.vb"
    Inherits="YMCAUI.UpdateFedWithHoldingInfo"  %>

<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_ToolBar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<title>YMCA YRS </title>
<%--START: ML |11/19/2019 |YRS-AT-4719 |Added style tag and script--%>
<style id="Style1" type="text/css" media="screen" runat="server">
        @import '<%= ResolveUrl("~/JS/jquery-ui/base/jquery.ui.all.css")%>';
 </style>
<%--END: ML |11/19/2019 |YRS-AT-4719 |Added style tag and script--%>

<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">

<%--START: ML |11/19/2019 |YRS-AT-4719 |Added style tag and script--%>
<script src='<%= ResolveClientUrl("~/JS/jquery-1.7.2.min.js") %>' type="text/javascript"></script>
<script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
<%--END: ML |11/19/2019 |YRS-AT-4719 |Added style tag and script--%>
<%-- 'Anudeep A              2012-09-22      BT-1126/Additional change YRS 5.0-1246 : : Handling DLIN records --%>
<script language="JavaScript">
    <%--START: ML|11/19/2019|YRS-AT-4719|Added div to display warning message--%>
    function BindEvents() {
        $('#FederalWitholding').dialog({
            autoOpen: false,
            draggable: true,
            close: false,
            resizable: false,
            modal: false,
            width: 450, maxHeight: 450,
            height: 270,
            title: "YMCA YRS",
            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });
    }

    function ShowDialog() {
        $('#FederalWitholding').dialog('open');
        return false;
    }

    function closeDialog(id) {
        $('#' + id).dialog('close');
    }
    <%--END: ML | 11/19/2019 | YRS-AT-4719 |Added div to display warning message--%>
    function _OnBlur_TextBoxAmount() {

        if (isNaN(parseInt(document.Form1.all.txtAddlAmount.value))) {
            //Added for BT-1126
            alert('<%=Resources.withholding.MESSAGE_FEDERAL_WITHOLDING_AMOUNT_CANNOT_CHARACTERS %>');
            document.Form1.all.txtAddlAmount.value = "";


        }

    }
    function _OnBlur_TextBoxExemptions() {


        if (isNaN(parseInt(document.Form1.all.txtExemptions.value))) {
            //Added for BT-1126
            alert('<%=Resources.withholding.MESSAGE_FEDERAL_WITHOLDING_AMOUNT_CANNOT_CHARACTERS %>');

            document.Form1.all.txtExemptions.value = "";

        }

    }
    var theform;
    var isIE;
    var isNS;

    /*
    Function to detect the Browser type.
    */
    function detectBrowser() {
        if (window.navigator.appName.toLowerCase().indexOf("netscape") > -1)
            theform = document.forms["Form1"];
        else
            theform = document.Form1;

        //browser detection
        var strUserAgent = navigator.userAgent.toLowerCase();
        isIE = strUserAgent.indexOf("msie") > -1;
        isNS = strUserAgent.indexOf("netscape") > -1;

    }

    /*
    This function will fire when the control leaves the Text Box.
    The function is responsible for formating the numbers to amount type.
    */
    function FormatAmtControl(ctl) {
        var vMask;
        var vDecimalAfterPeriod;
        var ctlVal;
        var iPeriodPos;
        var sTemp;
        var iMaxLen
        var ctlVal;
        var tempVal;
        ctlVal = ctl.value;
        vDecimalAfterPeriod = 2
        iMaxLen = ctl.maxLength;

        if (isNaN(ctlVal)) {
            // clear the control as this is not a num
            //ctl.value=""
        }
        else {
            ctlVal = ctl.value;
            iPeriodPos = ctlVal.indexOf(".");
            if (iPeriodPos < 0) {
                if (ctl.value.length > (iMaxLen - 3)) {
                    sTemp = ctl.value
                    tempVal = sTemp.substr(0, (iMaxLen - 3)) + ".00";
                }
                else
                    tempVal = ctlVal + ".00"
            }
            else {
                if ((ctlVal.length - iPeriodPos - 1) == 1)
                    tempVal = ctlVal + "0"
                if ((ctlVal.length - iPeriodPos - 1) == 0)
                    tempVal = ctlVal + "00"
                if ((ctlVal.length - iPeriodPos - 1) == 2)
                    tempVal = ctlVal;
                if ((ctlVal.length - iPeriodPos - 1) > 2) {
                    tempVal = ctlVal.substring(0, iPeriodPos + 3);
                }


            }
            ctl.value = tempVal;
        }
    }
    /*
    This function is responsible for filtering the keys pressed and the maintain the amount format of the 
    value in the Text box
    */
    function HandleAmountFiltering(ctl) {
        var iKeyCode, objInput;
        var iMaxLen
        var reValidChars = /[0-9.]/;
        var strKey;
        var sValue;
        var event = window.event || arguments.callee.caller.arguments[0];
        iMaxLen = ctl.maxLength;
        sValue = ctl.value;
        detectBrowser();

        if (isIE) {
            iKeyCode = event.keyCode;
            objInput = event.srcElement;
        } else {
            iKeyCode = event.which;
            objInput = event.target;
        }

        strKey = String.fromCharCode(iKeyCode);

        if (reValidChars.test(strKey)) {
            if (iKeyCode == 46) {
                if (objInput.value.indexOf('.') != -1)
                    if (isIE)
                        event.keyCode = 0;
                    else {
                        if (event.which != 0 && event.which != 8)
                            return false;
                    }
            }
            else {
                if (objInput.value.indexOf('.') == -1) {

                    if (objInput.value.length >= (iMaxLen - 3)) {
                        if (isIE)
                            event.keyCode = 0;
                        else {
                            if (event.which != 0 && event.which != 8)
                                return false;
                        }

                    }
                }
                if ((objInput.value.length == (iMaxLen - 3)) && (objInput.value.indexOf('.') == -1)) {
                    objInput.value = objInput.value + '.';

                }


            }

        }
        else {
            if (isIE)
                event.keyCode = 0;
            else {
                if (event.which != 0 && event.which != 8)
                    return false;
            }
        }

    }
    function ValidateNumeric() {
        if ((event.keyCode < 48) || (event.keyCode > 57)) {
            event.returnValue = false;
        }
    }


</script>
<body onload="self.focus()">
    <form id="Form1" method="post" runat="server">
    <div align="left">
        <table width="700" cellspacing="0" border="0">
            <tr>
                <td class="Td_HeadingFormContainer" align="left">
                <YRSControls:YMCA_ToolBar_WebUserControl ID="Toolbar_Control" runat="server" ShowLogoutLinkButton="false"
                    ShowHomeLinkButton="false" />
                    <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
                </td>
            </tr>
        </table>
        <table id="Table1" border="0" cellspacing="0" width="92%" class="Table_WithBorder" > <%--ML |11/19/2019 |YRS-AT-4719 |Change width size. --%>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td width="139" align="left">
                    <asp:Label ID="LabelAnnuitiesLastName" runat="server" CssClass="Label_Small">Tax Entity</asp:Label>&nbsp;&nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="cmbTaxEntity" runat="server" Width="144px">
                    </asp:DropDownList>
                    <br /> <!--SR:2014.07.09-BT 2593 - UI changes in Beneficiary information page-->
                    <asp:Label ID="LabelDuplicateFedWith" runat="server" ForeColor="Red" Visible="False" CssClass="Label_Small">Cannot add duplicate withholding</asp:Label>
                </td>
            </tr>
            <tr>
                <td width="139" cssclass="Label_Small" align="left">
                    <asp:Label ID="Label1" runat="server" CssClass="Label_Small">Withholding Type</asp:Label>&nbsp;&nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="cmbWithHolding" runat="server" Width="144px" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="139" align="left">
                    <asp:Label ID="Label2" runat="server" CssClass="Label_Small">Exemptions</asp:Label>&nbsp;&nbsp;
                </td>
                <td align="left">
                    <asp:TextBox ID="txtExemptions" runat="server" CssClass="TextBox_Normal" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="139" height="27" align="left">
                    <asp:Label ID="Label3" runat="server" CssClass="Label_Small">Add'l Amount</asp:Label>&nbsp;&nbsp;
                </td>
                <td height="27" align="left">
                    <asp:TextBox ID="txtAddlAmount" runat="server" CssClass="TextBox_Normal" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="139" align="left">
                    <asp:Label ID="Label4" runat="server" CssClass="Label_Small">Marital Status</asp:Label>&nbsp;&nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="cmbMaritalStatus" runat="server" DataValueField="Code" DataTextField="Description"
                        Width="144px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="Td_ButtonContainer" align="right" colspan="2">
                    <asp:Button ID="ButtonOK" runat="server" Text="OK" CssClass="Button_Normal" Width="73px">
                    </asp:Button>
                    <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" CssClass="Button_Normal"
                        Width="73px"></asp:Button>
                </td>
            </tr>
        </table>
    </div>
        <%--START: ML |11/19/2019| YRS-AT-4719 |Modal popup to show federal withholding warning --%>
         <!------------------------------------------------10% Federal Withholding Dialogbox START--------------------------------------------------------->            
     <div id="FederalWitholding" title="YMCA - YRS" style="display: none;">
        <table  border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;width:100%">
            <tr>
                <td rowspan="2" style="width: 10%;">
                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="img2" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divFederalWitholdingMessage">                        
                        <asp:Label ID="lblFederalWitholdingMessage" runat="server" Text=""></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr id="tr2">
                <td style="text-align:right;vertical-align:bottom"  colspan="2">
                   <asp:Button runat="server" id="btnOK" text="OK" cssclass="Button_Normal"  
                        style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" />                    
                   
                    &nbsp;
                    <input type="button" ID="btnCancel" value="Cancel" class="Button_Normal" Style="width: 80px;
                                        color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold;
                                        height: 16pt;" onclick="closeDialog('FederalWitholding');" />
                </td>
            </tr>
        </table>
    </div>
    <!-------------------------------------------------10% Federal Withholding Dialogbox END--------------------------------------------------------->
     <%--END: ML |11/19/2019| YRS-AT-4719 |Modal popup to show federal withholding warning --%>
    </form>
    <!--#include virtual="bottom.html"-->
</body>
