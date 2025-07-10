<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UpdateBeneficiaries.aspx.vb" EnableEventValidation="false" 
    Inherits="YMCAUI.UpdateBeneficiaries" %>

<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="~/UserControls/YMCA_Footer_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_ToolBar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl_new.ascx" %>
 <html>
 <head>
<title>YMCA YRS </title>
        <script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script>
        <script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="JS/YMCA_JScript.js" type="text/javascript"></script>
    <script language="javascript" src="JS/YMCA_JScript_Warn.js" type="text/javascript"></script>
    <link href="CSS/CustomStyleSheet.css" type="text/css" runat="server" rel="stylesheet" />
    <script language="javascript" type="text/javascript"> </script>
<script type="text/javascript" language="javascript">

    function _OnBlur_TaxableAmount() {
        document.Form1.all("TextboxCheckTotal").value = Number(document.Form1.all("TextboxTaxableAmount").value) + Number(document.Form1.all("TextboxNonTaxableAmount").value);
    }
    function _OnBlur_NonTaxableAmount() {
        document.Form1.all("TextboxCheckTotal").value = Number(document.Form1.all("TextboxTaxableAmount").value) + Number(document.Form1.all("TextboxNonTaxableAmount").value);
    }

    function _OnBlur_TextboxCheckNo() {
        var _arr = new Array(20);
        var flg = false;
        var str = String(document.Form1.all.TextboxCheckNo.value);

        for (i = 0; i < str.length; i++) {
            _arr[i] = str.substr(i, 1);

        }

        for (i = 0; i < str.length - 1; i++) {

            if (_arr[i] != _arr[i + 1]) {
                flg = true;

            }


        }

        if (!flg) {
            alert('All the characters are same.');
            return;
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


                if (ctl.value.length > (iMaxLen - 5)) {
                    if (ctlVal.substr(0, 3) == "100") {
                        tempVal = "100.000";
                    }

                    else {
                        sTemp = ctl.value;
                        //alert(sTemp.substr(0,2));
                        tempVal = sTemp.substr(0, 2) + "." + sTemp.substr(2, iMaxLen - 3);
                    }


                }

                else
                    tempVal = ctlVal + ".0000";
                /*if (ctl.value.length > (iMaxLen-3))
                {
                sTemp = ctl.value
                tempVal = sTemp.substr(0,(iMaxLen-3)) + ".00";
                }
                else
                tempVal = ctlVal + ".00000000"*/
            }
            else {
                /*if ((ctlVal.length - iPeriodPos -1)==1)
                tempVal = ctlVal + "00000000"
                if ((ctlVal.length - iPeriodPos -1)==0)
                tempVal = ctlVal + "00000000"
                if ((ctlVal.length - iPeriodPos -1)==2)
                tempVal = ctlVal + "000000"
                if ((ctlVal.length - iPeriodPos -1)==3)
                tempVal = ctlVal + "00000"
                if ((ctlVal.length - iPeriodPos -1)==4)
                tempVal = ctlVal + "0000"
                if ((ctlVal.length - iPeriodPos -1)==5)
                tempVal = ctlVal + "000"
                if ((ctlVal.length - iPeriodPos -1)==6)
                tempVal = ctlVal + "00"
                if ((ctlVal.length - iPeriodPos -1)==7)
                tempVal = ctlVal + "0"
                if ((ctlVal.length - iPeriodPos -1)==8)
                tempVal = ctlVal;
                if ((ctlVal.length - iPeriodPos -1)>8){
                tempVal = ctlVal.substring(0,iPeriodPos+9);*/


                if ((ctlVal.length - iPeriodPos - 1) == 1)
                    tempVal = ctlVal + "000";
                if ((ctlVal.length - iPeriodPos - 1) == 0)
                    tempVal = ctlVal + "0000";
                if ((ctlVal.length - iPeriodPos - 1) == 2)
                    tempVal = ctlVal + "00";
                if ((ctlVal.length - iPeriodPos - 1) == 3)
                    tempVal = ctlVal + "0";
                if ((ctlVal.length - iPeriodPos - 1) == 4)
                    tempVal = ctlVal;
                if ((ctlVal.length - iPeriodPos - 1) > 4)
                { tempVal = ctlVal.substring(0, iPeriodPos + 5); }
            }


        }
        ctl.value = tempVal;
    }


    /*
    This function is responsible for filtering the keys pressed and the maintain the amount format of the 
    value in the Text box
    */
    function HandleAmountFiltering(ctl) {
        var iKeyCode, objInput;
        var iMaxLen
        //var reValidChars = /[0-9]/;
        var reValidChars = "0123456789."
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

        //if (reValidChars.test(strKey))
        if (reValidChars.indexOf(strKey) != -1) {
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
                /*if(objInput.value.indexOf('.')==-1)
                {
				
                if (objInput.value.length>=(iMaxLen-5))
                {
                if (isIE)
                event.keyCode= 0;
                else
                {
                if(event.which!=0 && event.which!=8)
                return false;
                }
	
                }
                }
		
                if ((objInput.value.length==(iMaxLen-5)) && (objInput.value.indexOf('.')==-1)) 
                {	
			
                //objInput.value = objInput.value + '.';
			
                }*/

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
    //Sanjay R:2013.08.05 - YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
    $(document).ready(function () {
        $("#divWSMessage").dialog
					({
					    modal: true,
					    open: function (event, ui) { $(this).parent('div').find('button:contains("SAVE")').focus(); },
					    autoOpen: false,
					    title: "Process Restricted",
					    width: 570, height: 200,
					    buttons: [{ text: "OK", click: CloseWSMessage}]
					});
    });
    function CloseWSMessage() {
        $(document).ready(function () {
            $("#divWSMessage").dialog('close');
        });
    }
    function openDialog(str, type) {
        $(document).ready(function () {
            if (type == 'Bene') 
                {
                    str = 'Beneficiary add,edit and delete operation can not be performed due to following reason(s). <br/>' + str
                }
            else 
                {
                    str = 'Person\'s Marital Status can not be changed due to following reason(s).<br/>' + str;
                }
            $("#divWSMessage").html(str);
            $("#divWSMessage").dialog('open');
            return false;
        });
    }
    //End, Sanjay R:2013.08.05 - YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
</script>
</head>
<body>
<form id="Form1" method="post" runat="server">
<div class="Div_Center">
    <table width="100%" border="0" cellspacing="0">
        <tr>
            <td class="Td_HeadingFormContainer" align="left">
                <YRSControls:YMCA_ToolBar_WebUserControl ID="Toolbar_Control" runat="server" ShowLogoutLinkButton="false"
                    ShowHomeLinkButton="false" />
                <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
            </td>
        </tr>
        <tr>
        <td>
             <div id="DivMainMessage" class="warning-msg" runat="server" style="text-align: left;" visible="false" enableviewstate="false"/>
            <asp:ValidationSummary ID="vdlSummary" runat="server" ValidationGroup="UpdateBene" DisplayMode="List" ShowSummary="true" ShowMessageBox="false"  CssClass="Error_Message" />
        </td></tr>
        <tr>
            <td valign="top">
            <table style="width:100%" cellspacing="0" cellpadding="0" class="Table_WithBorder">
                <tr>
                    <%--Start - Manthan Rajguru | 2016.07.22 | Changed the width of TD as new control checkbox added for generating phony SSN--%>
                    <td class="Td_ButtonContainer" width="65%">Beneficiary Details  </td>
                    <td  class="Td_ButtonContainer" style="padding-left:2px" width="35%"><asp:Label id="lblRepresentative" runat="server">Representative Details</asp:Label> </td>
                    <%--End - Manthan Rajguru | 2016.07.22 | Changed the width of TD as new control checkbox added for generating phony SSN--%></tr>
                <tr >
                <td width="53%">
                <table  id="Table1" cellpadding="1" width="100%" align="left"
                    cellspacing="0" border="0">
                     <tr height="25px">
                            <td>
                                <label class="Label_Small">Beneficiary Type </label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbdtnlstBeneficiaryType" AutoPostBack="true" CssClass="RadioButton_Normal" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Human" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Non-Human"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>

                        </tr>
                    <tr height="25px">
                        <td align="left" width="125" class="Label_Small"> <%-- Manthan Rajguru | 2016.07.22 | Changed the width of TD as new control checkbox added for generating phony SSN--%>
                           <asp:Label ID="LabelFirstName" runat="server" CssClass="Label_Small"> First Name</asp:Label>
                        </td>
                        <td align="left">
                            <asp:textbox id="txtFirstName" runat="server" cssclass="TextBox_Normal" maxlength="20"></asp:textbox> 
                            <asp:requiredfieldvalidator id="RequiredFirstNamevalidator" runat="server" text="*"
                                controltovalidate="txtFirstName" errormessage="Please enter first name." ValidationGroup="UpdateBene" enabled="true" CssClass="Error_Message"></asp:requiredfieldvalidator>
                        </td>
                        
                    </tr>
                    <tr height="25px">
                        <td align="left" width="125" class="Label_Small"> <%-- Manthan Rajguru | 2016.07.22 | Changed the width of TD as new control checkbox added for generating phony SSN--%>
                           <asp:Label ID="LabelLastName" runat="server" CssClass="Label_Small">Last Name</asp:Label>
                        </td>
                        <td align="left">
                            <asp:textbox id="txtLastName" runat="server" cssclass="TextBox_Normal" maxlength="30"></asp:textbox>
                            <asp:requiredfieldvalidator id="RequiredFieldValidator2" errormessage="Please enter last name." runat="server" text="*"
                                controltovalidate="txtLastName"  ValidationGroup="UpdateBene" CssClass="Error_Message"></asp:requiredfieldvalidator>
                        </td>
                        
                    </tr>
                    
                    <tr height="30px">   <%-- SB | 07/07/2016 | YRS-AT-2382 | Changed height of rows as new button is added to edit SSN --%>
                        <td align="left" width="125" class="Label_Small"> <%-- Manthan Rajguru | 2016.07.22 | Changed the width of TD as new control checkbox added for generating phony SSN as new control checkbox added for generating phony SSN--%>
                            SSN/Tax No
                        </td>
                        <td align="left" >
                            <asp:textbox id="txtSSNNo" runat="server" cssclass="TextBox_Normal" maxlength="9" AutoPostBack="true"></asp:textbox>
                            <asp:CompareValidator ID="CompareOldSSNvalidator" runat="server" ErrorMessage="New SSN matches old SSN. No update required." Text="*"
                                    Operator="NotEqual" ControlToValidate="txtSSNNo" Type="String" CssClass="Error_Message" ValidationGroup="UpdateBene" Enabled="false"/> <%--SB | 07/07/2016 | YRS-AT-2382 | Compare vaildte added to check old  SSN with New SSNs --%>                            
                            <asp:Button ID="ButtonActiveBeneficiariesSSNEdit" runat="server" Text="Edit" CssClass="Button_Normal"
                               Width="53px" CausesValidation="False" Enabled="true"></asp:Button>   <%--Added By: SB | 07/07/2016 | YRS-AT-2382 | New Button is added to edit SSN --%>
                        </td>
                        
                    </tr>
                    <tr>
                        <td> </td>
                        <td colspan="1" align="left">
                            <%--START : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder --%>
                            <%--<asp:CheckBox ID="chkPhonySSN" Text="SSN not available. Generate random phony SSN" runat="server" Visible ="false" Font-Size="XX-Small" Font-Bold="true" Width="375px"  AutoPostBack="true" /> < %--MMR | 2016.07.22 | YRS-AT-2560 | Checkbox to generate phony SSNo --% > --%>
                            <asp:CheckBox ID="chkPhonySSN" Text="SSN not available. Generate random placeholder SSN" runat="server" Visible ="false" Font-Size="XX-Small" Font-Bold="true" Width="375px"  AutoPostBack="true" />
                            <%--END : MMR | 05/03/2018 | YRS-AT-3941 | Replaced Phony keyword with Placeholder --%>
                        </td>
                    </tr>
                    <%--START : SB | 07/07/2016 | YRS-AT-2382 | Added new table Row to capture Reason for SSN change --%>
                    <tr height="34px" id="trSSNChangeReason" runat="server">
                          <td align="left" width="125" class="Label_Small"> <%-- Manthan Rajguru | 2016.07.22 | Changed the width of TD as new control checkbox added for generating phony SSN--%>
                           <asp:Label ID="LabelReasonSSNChange" runat="server" CssClass="Label_Small">SSN Edit Reason</asp:Label>
                        </td>
                        <td align="left">
                        <asp:DropDownList ID="ddlBeneficiariesSSNChangeReason" runat="server" CssClass="DropDown_Normal">
									<asp:ListItem Selected="True" Text="-Select-" Value="-Select-" />
								</asp:DropDownList>
								<asp:RequiredFieldValidator ID="rqvBenefSSNoChangeReason" runat="server" Text="*" ErrorMessage="Please Select Reason."
									InitialValue="-Select-" ValidationGroup="UpdateBene" ControlToValidate="ddlBeneficiariesSSNChangeReason"></asp:RequiredFieldValidator>
                         
                        </td>
                        
                    </tr>
                    <%--END : SB | 07/07/2016 | YRS-AT-2382 | Added new table Row to capture Reason for SSN change --%>    
                    <tr height="25px">
                        <td align="left" width="125" height="27" class="Label_Small"> <%-- Manthan Rajguru | 2016.07.22 | Changed the width of TD as new control checkbox added for generating phony SSN--%>
                            <%--Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and added label for Birth date--%>
                            <%--BirthDate--%>
                            <asp:Label ID="lblBirthDate" runat="server" CssClass="Label_Small">Birth Date</asp:Label>
                            <%--End - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and added label for Birth date--%>
                        </td>
                        <td align="left">
                            <uc1:DateUserControl ID="txtBirthDate"  runat="server"></uc1:DateUserControl>
                        </td>
                        
                    </tr>
                    <tr height="25px">
                        <td align="left" width="125" class="Label_Small"> <%-- Manthan Rajguru | 2016.07.22 | Changed the width of TD as new control checkbox added for generating phony SSN--%>
                            Relationship
                        </td>
                        <td align="left">
                            <asp:dropdownlist id="cmbRelation" runat="server" width="152px" cssclass="Dropdown_Normal"
                                autopostback="false">
					            <asp:ListItem Selected="True"></asp:ListItem>
				            </asp:dropdownlist>
                            <asp:requiredfieldvalidator id="RequiredFieldValidator5" errormessage="Please select relationship." ValidationGroup="UpdateBene" runat="server" text="*"
                                controltovalidate="cmbRelation" CssClass="Error_Message"></asp:requiredfieldvalidator>
                        </td>
                        
                    </tr>
                    <tr style="height:25px;vertical-align:top;">
                        <td align="left" >
                             <Label ID="LabelBenefitPercentage"  class="Label_Small"> Benefit Percentage</Label>
                        </td>
                        <td align="left">
                            <asp:textbox id="txtPercentage" runat="server" cssclass="TextBox_Normal" maxlength="7"></asp:textbox>&nbsp;&nbsp;<asp:requiredfieldvalidator id="RequiredFieldValidator6" runat="server" text="*"
                                controltovalidate="txtPercentage" errormessage="Please enter benefit percentage." CssClass="Error_Message" ValidationGroup="UpdateBene"></asp:requiredfieldvalidator><asp:rangevalidator id="RangeValidator1" runat="server" errormessage="Beneficiary must equal 100  Percent." CssClass="Error_Message" 
                                controltovalidate="txtPercentage" ValidationGroup="UpdateBene" Text="*" maximumvalue="100" minimumvalue="1" type="Double" Display="Dynamic"></asp:rangevalidator>
                        </td>
                    </tr>
                    <tr height="25px">
                        <td align="left" width="125" class="Label_Small"> <%-- Manthan Rajguru | 2016.07.22 | Changed the width of TD as new control checkbox added for generating phony SSN--%>
                            Benefit Group
                        </td>
                        <td align="left">
                            <asp:dropdownlist id="cmbGroup" runat="server" width="152px" cssclass="Dropdown_Normal"
                                autopostback="True">
					<asp:ListItem Selected="True"></asp:ListItem>
				</asp:dropdownlist>
                            <asp:requiredfieldvalidator id="RequiredFieldValidator7" runat="server" text="*" errormessage="Please select benefit group."
                                controltovalidate="cmbGroup" ValidationGroup="UpdateBene" CssClass="Error_Message"></asp:requiredfieldvalidator>
                        </td>
                    </tr>
                    <tr height="25px">
                        <td align="left" width="125" class="Label_Small"> <%-- Manthan Rajguru | 2016.07.22 | Changed the width of TD as new control checkbox added for generating phony SSN--%>
                            Benefit Level
                        </td>
                        <td align="left">
                            <asp:dropdownlist id="cmbLevel" runat="server" width="152px" cssclass="Dropdown_Normal"
                                autopostback="True" enabled="False" >
					<asp:ListItem Selected="True"></asp:ListItem>
				</asp:dropdownlist>
                        </td>
                    </tr>
                    <tr height="25px">
                        <td align="left" width="125" class="Label_Small"> <%-- Manthan Rajguru | 2016.07.22 | Changed the width of TD as new control checkbox added for generating phony SSN--%>
                            Benefit Type
                        </td>
                        <td align="left">
                            <asp:dropdownlist id="cmbType" runat="server" width="152px" cssclass="Dropdown_Normal"
                                autopostback="True" visible="true"></asp:dropdownlist>
                            <asp:requiredfieldvalidator id="RequiredFieldValidator8" runat="server"
                                text="*" controltovalidate="cmbType"  errormessage="Please select benefit type." ValidationGroup="UpdateBene" CssClass="Error_Message"></asp:requiredfieldvalidator>
                        </td>
                    </tr>
                    <%--START: MMR | 2017.12.04 | YRS-AT-3756 | Added dropdown control for deceased beneficiary--%>
                    <tr height="25px">
                        <td align="left" width="125" class="Label_Small">
                            <asp:Label ID ="lblDeceasedBeneficiary" runat="server">Beneficiary of</asp:Label>
                        </td>
                        <td align="left">
                            <asp:dropdownlist id="ddlDeceasedBeneficiary" runat="server" width="152px" cssclass="Dropdown_Normal"
                                autopostback="True" visible="true">                                                                                          
                            </asp:dropdownlist>                          
                        </td>
                    </tr>
                    <%--END: MMR | 2017.12.04 | YRS-AT-3756 | Added dropdown control for deceased beneficiary--%>
                    <tr height="25px">
                        <td></td>
                        <td align="left" > 
                            <asp:LinkButton ID="lnkParticipantAddress" runat="server" CssClass="Link_Small" Text="Use Participant Address"></asp:LinkButton>  </td>
                        
                    </tr>
                    <tr  >
                        <td colspan="2" >
                            
                                <table class="Table_WithoutBorder">
                                    <tr>
                                        <td width="350px" class="Table_WithBorder">
                                            <NewYRSControls:New_AddressWebUserControl runat="server" PopupHeight="930" ID="AddressWebUserControl1" AllowNote="true" AddressFor="BENE" AllowEffDate="true" />        
                                        </td>
                                    </tr>
                                    <tr>
                                    <td align="left">
                                            <asp:Label ID="lblAddressChange" runat="server" Text="Address has been defaulted with respect to SSN." CssClass="Error_Message"  Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                </table>
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                   
                </table>
                    </td>
                  <td valign="top" style="padding-left:2px;border-left-color :#ffd7a7;border-left-width:thin; border-left-style:solid;" >
                    <asp:Panel ID="pnlRepresenattiveDetails" Visible="false" runat="server">
                        <table width="100%" border="0" cellspacing="0" >
                            <tr height="25px">
                                <td align="left">
                                    <label class="Label_Small">
                                        Salutation
                                    </label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlRepSalutaionCode" runat="server" CssClass="DropDown_Normal" Height="16px" Width="150px"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr height="25px">
                                <td align="left" class="Label_Small" height="12">First Name</td>
                                <td align="left" height="12">
                                    <asp:TextBox ID="txtRepFirstName" MaxLength="20" runat="server" CssClass="TextBox_Normal" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr height="25px">
                                <td align="left" class="Label_Small">Last Name</td>
                                <td align="left">
                                    <asp:TextBox ID="txtRepLastName" MaxLength="30" CssClass="TextBox_Normal" runat="server" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr height="25px">
                                <td align="left" class="Label_Small">Telephone</td>
                                <td align="left">
                                    <asp:TextBox ID="txtRepTelephoneNo"  MaxLength="25" CssClass="TextBox_Normal" onkeypress="javascript:ValidateNumeric();" runat="server" Width="149px"></asp:TextBox>
                                </td>
                            </tr>                          
                        </table>
                    </asp:Panel>
                </td>
              </tr>
                <tr>
                        <td class="Td_ButtonContainer" align="right" colspan="2">
                            <asp:button id="ButtonSave" runat="server"  ValidationGroup="UpdateBene" cssclass="Button_Normal" width="95px"
                                text="OK"></asp:button>
                            <asp:button id="ButtonCancel" runat="server" cssclass="Button_Normal" width="73px"
                                text="Cancel" causesvalidation="False"></asp:button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
                <td  width="100%">
                    <YRSControls:YMCA_Footer_WebUserControl ID="YMCA_Footer_WebUserControl1" runat="server">
                    </YRSControls:YMCA_Footer_WebUserControl>
                </td>
            </tr>
        
    </table>
</div>
<div id="divWSMessage"  runat="server" style="display: none;">
    <table width="690px">
        <tr>
            <td valign="top" align="left">
                <span id="spntext" ></span>
            </td>
        </tr>
    </table>
</div>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
<input id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server" value="" />
</form>

</body>
</html>
