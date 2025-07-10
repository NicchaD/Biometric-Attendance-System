<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AddressUserControl.ascx.vb"
    Inherits="YMCAUI.AddressUserControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="YRSControls" TagName="DateUserControl" Src="~/UserControls/DateUserControl.ascx" %>
<link href='<%= Me.ResolveClientUrl("~/CSS/proweb.css") %>' rel="stylesheet" type="text/css" />
<link href='<%= Me.ResolveClientUrl("~/CSS/CustomStyleSheet.css") %>'   type="text/css" rel="stylesheet" />
<div style="width:100%" >
    <p>
        <table id="Table1" style="width: 100%;" cellspacing="1" cellpadding="1" border="0" class="Table_WithoutBorder">
            <tr valign="top">
                
                <td class="Normaltext" >
                    <asp:Label ID="LabelNoAddressFound" runat="server" CssClass="Normaltext">
                    No Address information defined. <br />
                    </asp:Label>
                    <asp:Label ID="LabelUpdateAddress" runat="server" CssClass="Normaltext">
                    Please click on 'icon' to add or edit Address.</asp:Label>
                    
                        
                        <asp:TextBox ID="TextBoxAddress1" style="display:none;" CssClass="Normaltext" runat="server" Width="216px" MaxLength="60"></asp:TextBox><span></span>
                        <asp:TextBox ID="TextBoxAddress2" style="display:none;" CssClass="Normaltext" runat="server" Width="216px" MaxLength="60"></asp:TextBox><span></span>
                        <asp:TextBox ID="TextBoxAddress3" style="display:none;" CssClass="Normaltext" runat="server" Width="216px" MaxLength="60"></asp:TextBox><span></span><br />
                        <asp:TextBox ID="TextBoxCity" style="display:none;" CssClass="Normaltext" runat="server" Width="216px" MaxLength="30"></asp:TextBox><span></span>
                        <input type="hidden" runat="server" id="DropDownListState_hid" /><span><select id="DropDownListState" runat="server" class="Normaltext" style="width: 218px;display:none;"></select></span><asp:TextBox ID="TextBoxZip" style="display:none;" CssClass="Normaltext"  runat="server" Width="120px" MaxLength="10"></asp:TextBox><span></span><br />
                    <span><select id="DropDownListCountry" runat="server" class="Normaltext" style="width: 218px;display:none;"></select></span>
                    <input type="hidden" runat="server" id="DropDownListCountry_hid" /><span></span>
                        
                       
                    

                </td>
                <td align="right" width="20px">
                     <%If Me.EnableControls() Then%>
                        <!-- Anudeep:25.10.2013:BT:2264 - Added two variables to get check address has modified or address has verified  -->
                        <input  type="image" src="<%= Me.ResolveClientUrl("~/images/Edit-Record.jpg") %>" style="width:15px; height:15px;" id="btnEditaddress" alt="Add/Edit Address"  
                        onclick="javascript:  OpenAddress('<%= TextBoxAddress1.ClientId %>', 
                                                          '<%= TextBoxAddress2.ClientId %>', 
                                                          '<%= TextBoxAddress3.ClientId %>', 
                                                          '<%= TextBoxCity.ClientId %>', 
                                                          '<%= DropDownListCountry.ClientId %>', 
                                                          '<%= DropDownListState.ClientId %>', 
                                                          '<%= TextBoxZip.ClientId %>', 
                                                          '<%= CheckboxIsBadAddress.ClientId %>',
                                                          '<%= TextBoxEffDate.ClientId %>',
                                                          '<%= txtFinalNotes.ClientId %>',
                                                          '<%= chkNotes.ClientId %>',
                                                          '<%= imgBadAddress.ClientId %>',                                                                                                                    
                                                          '<%= LabelNoAddressFound.ClientId %>',
                                                          '<%= LabelUpdateAddress.ClientId %>',
                                                          '<%= hdnChangesExist.ClientId %>',
                                                          '<%= hdnReasons_hid.ClientId %>',
                                                          '<%= QASMatched.ClientId %>',
                                                          '<%= hasAddress.ClientId %>',
                                                          '<%= hdnAddressmode.ClientId %>'
                                                          ) ; 
                                                          AddressPopUp('<%= hdnpopupheight.value %>'); return false;" 
                    />
                    <%End If%>
                    &nbsp;&nbsp;&nbsp;&nbsp;<img id="imgBadAddress" runat="server" alt="Bad Address" width="15" height="15" src="../images/bad address.jpg"/>
                </td>
            </tr>
            
            <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
            <%If AllowEffDate Then%>
            <tr>
                
               
                <td align="left" valign="middle" colspan="2" class="Normaltext">                  
                    <YRSControls:DateUserControl ID="TextBoxEffDate" runat="server"></YRSControls:DateUserControl>
                </td>
                
            </tr>
            <%End If%>
            
            <tr>
                <td nowrap align="left" colspan="3" valign="middle" style="display:none;">
                    <asp:HiddenField ID="hdnChangesExist" runat="server" />
                    <asp:HiddenField ID="hdnpopupheight" runat="server" />
                    <asp:HiddenField ID="hdnRights" runat="server" />
                    <!-- Anudeep:22.08.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module. -->
                    <asp:HiddenField ID="txtFinalNotes" runat="server" />
                    <asp:HiddenField ID="chkNotes" runat="server" />
                    <asp:HiddenField ID="hdnReasons_hid" runat="server" />
                    <!-- Anudeep:25.10.2013:BT:2264 - Added hidden variables because it should not access to other usercontrol  -->
                    <asp:HiddenField ID="QASMatched" runat="server" />
                    <asp:HiddenField ID="hasAddress" runat="server"  />
                    <asp:HiddenField ID="hdnAddressmode" runat="server"  />
                </td>
            </tr>
            
            <tr style="display:none;">
                <td nowrap align="left" colspan="3">
                    <asp:Label ID="IsBadAddress12" runat="server" CssClass="Label_Small">Bad Address</asp:Label>
                
                    <asp:CheckBox ID="CheckboxIsBadAddress" style=" font-weight:normal;" runat="server" 
                        Enabled="false" Width="240"></asp:CheckBox>
                </td>
                
            </tr>
        </table> 
    </p>
    
    <%  If HttpContext.Current.Items("AddressControlDiv") Is Nothing Then
            HttpContext.Current.Items("AddressControlDiv") = "DivAdded"
        
    %>
</div>

<div id="proweb_wgt" style="z-index: 1500">
</div>
<div id="divNotes" style="display: none;">
    <div id="divAddress" class="AddressLabels">
        <div id="input_wgt" class="proweb" style="float: left;">
        </div>
    </div>
    <style type="text/css">
        span.AddressLabels
        {
            display: block;
            width: 85px;
            padding-top: 5px;
        }
        input.AddressLabels, select.AddressLabels
        {
            width: 200px;
            font-family: Verdana,Tahoma,Arial,Helvetica,sans-serif;
            font-size: 8pt;
        }
        label.AddressLabels
        {
            font-family: Verdana,Tahoma,Arial,Helvetica,sans-serif;
            font-size: 8pt;
        }
        textarea.AddressLabels
        {
            width: 260px;
            height: 240px;
            font-family: Verdana,Tahoma,Arial,Helvetica,sans-serif;
            font-size: 8pt;
        }
    </style>
    <table border="0" class="AddressLabels" cellspacing="10" style="height: 50%">
        <tr valign="top">
            <td rowspan="2" valign="top">
                <table>
                    <tr>
                        <td>
                            <span class="AddressLabels Label_Small">Address 1:</span>
                            <input id="lineone" class="AddressLabels TextBox_Normal"
                            onchange="javascript:changesEffectiveAddress();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="AddressLabels Label_Small">Address 2:</span>
                            <input id="linetwo" class="AddressLabels TextBox_Normal"
                            onchange="javascript:changesEffectiveAddress();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="AddressLabels Label_Small">Address 3:</span>
                            <input id="linethree" class="AddressLabels TextBox_Normal"
                            onchange="javascript:changesEffectiveAddress();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="AddressLabels Label_Small">City:</span>
                            <input id="city" class="AddressLabels TextBox_Normal"
                            onkeypress="javascript:ValidateAlphaNumeric();" onchange="javascript:changesEffectiveAddress();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="AddressLabels Label_Small">State:</span>
                            <select id="state" class="AddressLabels DropDown_Normal"
                            style="width: 200px" onchange="javascript:changesEffectiveAddress();"></select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="AddressLabels Label_Small">Country:</span>
                            <select id="country" class="AddressLabels DropDown_Normal"
                            style="width: 200px" onchange="javascript:changesEffectiveAddress();"></select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="AddressLabels Label_Small">Zip:</span>
                            <input id="zip" onkeypress="javascript:ValidateZipCodeKeyPress(this,'country');"
                            onblur="javascript:validateZipCode(this,'country');" class="AddressLabels TextBox_Normal"
                            onchange="javascript:changesEffectiveAddress();" />
                        </td>
                    </tr>
                    <tr id="treffDate">
                        <td>
                            <span class="AddressLabels Label_Small">Eff. Date:</span>
                            <YRSControls:DateUserControl runat="server" ID="effdate" cssInput="AddressLabels TextBox_Normal"></YRSControls:DateUserControl>
                        </td>
                    </tr>
                    <tr>
                        <td>   
                            <span class="AddressLabels Label_Small" style="display: inline;">Bad address:</span>
                            <input type="checkbox" id="IsbadAddress1" class="CheckBox_Normal" onclick="javascript:changesEffectiveAddress();" />
                             <img src="images/verify2.gif" alt="Verify Address" id="btnVerify" onclick="VerifyAddress();" title="Verify Address"
                            style="margin-bottom: -5px; " />
                            <input type="hidden" id="Hidden1" />
                            <input type="hidden" id="Hidden2" />
                            <input id="Text1" value="Submit" style="display: none" />
                        </td>
                    </tr>
                </table>
            </td>
            <% If AllowNote Then%>
            <td id="tdOrangeLine" rowspan="2" style="background-color: Orange">
            </td>
            <td valign="top" id="tdNotes">
                <table>
                    <tr valign="top">
                        <td>
                        <span id="LabelReason" class="Label_Small">Reason:</span>
                        <div id="rblReason" style="border-style: None; width: 150px;display:none;"></div>
                </td>
                        <td></td>
                    </tr>
                    <tr valign="top">
                        
                        <td>
                           <select id="Reason" class="AddressLabels DropDown_Normal"
                                style="width: 150px" onchange="javascript:changeOnNotes();"></select>
             
                        </td>
                        <td>
                            <input name="TxtReason" type="text" id="TxtReason" class="AddressLabels TextBox_Normal"
                                    style="width: 100px;" onchange="javascript:changeOnNotes();" />
                        </td>
                    </tr>
                
                    <tr valign="top">
                        <td>
                            <span id="LabelSource" class="Label_Small">Source:</span>
                            <div id="rblSource" style="border-style: None; width: 150px;display:none;"> </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr valign="top">
                        
                        <td>
                            <select id="Source" class="AddressLabels DropDown_Normal"
                                style="width: 150px" onchange="javascript:changeOnNotes();"></select>
                        </td>
                        <td>
                            <input id="txtSource" class="AddressLabels TextBox_Normal" style="width: 100px;"
                                onchange="javascript:changeOnNotes();" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="2">
                                <span id="LabelNotes" class="Label_Small">Notes:</span>
                                <span class="CheckBox_Normal">
                                    <input id="chkImpt" type="checkbox" name="chkImpt" onclick="javascript:changeOnNotes();" /><label
                                        for="chkImpt" class="AddressLabels">Important</label></span><br />
                                <textarea cols="" name="txtNotes" rows="" id="txtNotes" class="AddressLabels TextBox_Normal"></textarea>
                         </td>
                    </tr>
                
            
        
                </table>
            </td>
            <% End If%>
        </tr>
        
    </table>
    <input type="hidden" id="country_hid" />
    <input type="hidden" id="state_hid" />
    <input id="input_search" value="Submit" style="display: none" />
</div>
<script language="javascript" type="text/javascript">
    var completeStateList = null;
    function getCompleteStateList() {
        if (completeStateList != null) return completeStateList;
        var arr = new Array();
        var i;
        for (i = 0; i < countries.length; i++) {
            if (states[countries[i][0]] != null) {
                arr = arr.concat(states[countries[i][0]]);
            }
        }
        completeStateList = arr;
        return arr;
    }
    function getCountryCodeForState(stateId) {
        var allStateList = getCompleteStateList();
        var i;
        for (i = 0; i < allStateList.length; i++) {
            if (allStateList[i][0] == stateId)

                return allStateList[i][2];
        }
        return null;
    }
    function setCountryCodeForStateId(drdCountryId, drdStateId, stateId) {
        var countryId = getCountryCodeForState(stateId);
        countryList = countries;
        changeSelect(drdCountryId, countryList, countryId);
        setStates(drdCountryId, drdStateId, stateId);
    }
    function setCountries(drdCountryId, drdStateId, defSelCountryId) {
        countryList = countries;
        changeSelect(drdCountryId, countryList, defSelCountryId);
        setStates(drdCountryId, drdStateId, null);
    }
    function setStates(drdCountryId, drdStateId, defSelStateId) {
        cntrySel = document.getElementById(drdCountryId);
        stateList = states[cntrySel.value];
        if (cntrySel.value == "") {
            //stateList = getCompleteStateList();
            stateList = states['US'];  //SR:2011.04.18 - YRS 5.0 1177/BT-588(First time display US states only )
        }
        $("#" + drdCountryId + "_hid").val(cntrySel.value); //Assign country selected value to hidden control
        changeSelect(drdStateId, stateList, defSelStateId);
    }
    function setReasons(dtnotes) {

        var i = 0; var html = ''; var inputRadio; var e;
        var Reason = document.getElementById('Reason');
        Reason.options[0] = new Option('- Select -',"");   
        for (i = 0; i < dtnotes.length; i++) {
            e = dtnotes[i];
            Reason.options[i + 1] = new Option(e);   
        }
    }
    function setSources(dtnotes) {

        var i = 0; var html = ''; var inputRadio; var e;
        var Reason = document.getElementById('Source');
        Reason.options[0] = new Option('- Select -', "");
        for (i = 0; i < dtnotes.length; i++) {
            e = dtnotes[i];
            Reason.options[i + 1] = new Option(e);
        }
    }
    function changeSelect(fieldID, newOptions, defSelection) {
        selectField = document.getElementById(fieldID);
        if (selectField == null) return;
        selectField.options.length = 0;
        selectField.options[0] = new Option("- Select -", "");
        if (newOptions != null) {
            for (i = 0; i < newOptions.length; i++) {
                selectField.options[selectField.length] = new Option(newOptions[i][1], newOptions[i][0]);
            }
            if (defSelection != null) {
                selectField.value = defSelection;
            }
        }
        //Assign state/country selected value to hidden control
        if (document.getElementById(fieldID + "_hid") != null) {
            if (defSelection != null) {
                $("#" + fieldID + "_hid").val(defSelection);
            } else {
                $("#" + fieldID + "_hid").val('');
            }
        } else {
            alert('Error setting up address control');
        }
    }

    function ValidateZipCodeForUS() {
        if (!(event.keyCode == 45 || event.keyCode == 46 || event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57))
        { event.returnValue = false; }

    }

</script>
<script language="javascript" type="text/javascript">
    function fncKeyCityStop() {
        // Check if the control key is pressed.
        // If the Netscape way won't work (event.modifiers is undefined),
        // try the IE way (event.ctrlKey)
        var ctrl = typeof event.modifiers == 'undefined' ? event.ctrlKey : event.modifiers & Event.CONTROL_MASK;

        // Check if the 'V' key is pressed.
        // If the Netscape way won't work (event.which is undefined),
        // try the IE way (event.keyCode)
        var v = typeof event.which == 'undefined' ? event.keyCode == 86 : event.which == 86;

        // If the control and 'V' keys are pressed at the same time
        if (ctrl && v) {
            // ... discard the keystroke and clear the text box
            window.clipboardData.clearData();
            return false;
        }
        return true;
    }

    function fncKeyZipStop() {
        // Check if the control key is pressed.
        // If the Netscape way won't work (event.modifiers is undefined),
        // try the IE way (event.ctrlKey)
        var ctrl = typeof event.modifiers == 'undefined' ? event.ctrlKey : event.modifiers & Event.CONTROL_MASK;

        // Check if the 'V' key is pressed.
        // If the Netscape way won't work (event.which is undefined),
        // try the IE way (event.keyCode)
        var v = typeof event.which == 'undefined' ? event.keyCode == 86 : event.which == 86;

        // If the control and 'V' keys are pressed at the same time
        if (ctrl && v) {
            // ... discard the keystroke and clear the text box
            window.clipboardData.clearData();
            return false;
        }
        return true;
    }

    function ValidateAlpha() {
        if (((event.keyCode > 64) && (event.keyCode < 91)) || ((event.keyCode > 96) && (event.keyCode < 123)) || ((event.keyCode == 32))) {
            event.returnValue = true;
        } else {
            event.returnValue = false;
        }
    }
    function ValidateAlphaNumeric() {
        if (((event.keyCode > 64) && (event.keyCode < 91)) || ((event.keyCode > 96) && (event.keyCode < 123)) || ((event.keyCode > 47) && (event.keyCode < 58)) || (event.keyCode == 45) || ((event.keyCode == 32))) {
            event.returnValue = true;
        } else {
            event.returnValue = false;
        }
    }
    function ValidateZipCodeKeyPress(txtZipCodeControl, drdCountryCode) {
        var strCountryCode = document.getElementById(drdCountryCode).value;
        //If country code is not specified then do not validate zipCode
        if (strCountryCode == null || strCountryCode == "" ) return false;
        if (strCountryCode == "US") {
            //Validate if zipCode is the right format for US
            //return ValidateNumeric();
            return ValidateZipCodeForUS();
        } else if (strCountryCode == "CA") {
            //Validate if zipCode is the right format for CA
            return ValidateAlphaNumeric();
        }
        return ValidateAlphaNumeric();
    }

    function validateZipCode(txtZipCodeControl, drdCountryCode) {
        var strCountryCode = document.getElementById(drdCountryCode).value;
        //If country code is not specified then do not validate zipCode
        if (strCountryCode == null || strCountryCode == "" ) return false;
        if (strCountryCode == "US") {
            //AA:18.11.2013 Added for validating empty zip code for US
            if (txtZipCodeControl.value.replace(/\s+/g, '') == "") {
                alert('Zip code cannot be blank for United States.');
                return false;
            }
            //Validate if zipCode is the right format for US
            //AA:14.10.2013:BT:2042 -YRS 5.0-2092: -Added zipCode validation for checking if "Hyphen" Exists in string
            if (txtZipCodeControl.value.indexOf("-") >= 0) {
                if (/^[0-9]{5,5}-[0-9]{4,4}$/.test(txtZipCodeControl.value.replace(/\s+/g, '')) == false) {
                    alert('Invalid zip code. Please enter zip in 99999 or 999999999 or 99999-9999 format.');
                    return false;
                }
            }
            else if (/^[0-9]{5,5}$|^[0-9]{5,5}[0-9]{4,4}$/.test(txtZipCodeControl.value.replace(/\s+/g, '')) == false) {
                alert('Invalid zip code. Please enter zip in 99999 or 999999999 or 99999-9999 format.');
                return false;
            }
        } else if (strCountryCode == "CA") {
            //Validate if zipCode is the right format for CA
            //AA:18.11.2013 Added for validating empty zip code for CA
            if (txtZipCodeControl.value.replace(/\s+/g, '') == "") {
                alert('Zip code cannot be blank for Canada.');
                return false;
            }
            if (/^[a-zA-Z][0-9][a-zA-Z] [0-9][a-zA-Z][0-9]$/.test(txtZipCodeControl.value) == false) {
                alert('Invalid zip code. Please enter zip in a9a 9a9 format.');
                return false;
            }
        }
        return true;
    }
    function initializeAddressControl2(drdCountry, drdState, txtCountry_hid, txtState_hid, txtZip, txtAddress1, txtAddress2, txtAddress3, txtCity, txtEffDate, chkbadAddress, txtNote, chkImp) {
        $('#' + txtEffDate).parent().hide();
        $('#' + txtEffDate).parent().after('<span >' + $('#' + txtEffDate).val() + '</span>');

        $('#' + drdCountry + ',#' + drdState + ',#' + txtZip + ',#' + txtAddress1 + ',#' + txtAddress2 + ',#' + txtAddress3 + ',#' + txtCity + ',#' + chkbadAddress).each(function () {
            $(this).hide();
            if ($(this).is('select')) {
                $(this).parent().hide();
                //$(this).parent().after('<span class="Label_Small dummy">' + $('option:selected', this).text() + '</span>');
                //  $('#' + $(this)[0].id + '_hid').after('<span class="Label_Small dummy">' + $('option:selected', this).text() + '</span>');
                if ($('option:selected', this).text() == '- Select -') {
                    $('#' + $(this)[0].id + '_hid').after('<span >' + '' + '</span>');
                }
                else {
                    if ($('option:selected', this).text() != '') {
                        $('#' + $(this)[0].id + '_hid').after('<span >' + $('option:selected', this).text() + ' </span>');
                    }
                    else {
                        $('#' + $(this)[0].id + '_hid').after('<span >' + $('option:selected', this).text() + '</span>');
                    }
                }
            } else if ($(this).is(':checkbox')) {
                $(this).after('<span >' + IsBad_Address($(this).is(':checked')) + '</span>');
            } else {
                if ($(this).val() != '') {
                    $(this).after('<span >' + $(this).val() + '</span>');
                }
                else { $(this).after('<span >' + '' + '</span>'); }
            }
        }
               );

    }
    function IsBad_Address(IsCheck) {
        if (IsCheck == false) { return 'No'; } else { return 'Yes'; }
    }
    var dt_NotesReason = '';
    var dt_NotesSource = '';
    function initializeAddressControl(drdCountry, drdState, txtCountry_hid, txtState_hid, txtZip, txtAddress1, txtAddress2, txtAddress3, txtCity, txtEffDate) {
        //dt_NotesReason = NotesReason;
        dt_NotesSource = NotesSource;
        //setCountries(drdCountry, drdState, txtCountry_hid);
        var country = document.getElementById(txtCountry_hid).value;
        var state = document.getElementById(txtState_hid).value;
        setCountries(drdCountry, drdState, country);
        setStates(drdCountry, drdState, state);
        
        //assign event handlers for country select change
        $("#" + drdCountry).change(
			function () {
			    $("#" + txtZip).val('');
			    setStates(drdCountry, drdState, null);
			});
        //assign event handlers for state select change
        $("#" + drdState).change(
			function () {
			    setCountryCodeForStateId(drdCountry, drdState, this.value);
			});
    }
</script>
<script language='javascript' type="text/javascript">
    function getInternetExplorerVersion()
    // Returns the version of Windows Internet Explorer or a -1
    // (indicating the use of another browser).
    {
        var rv = -1; // Return value assumes failure.
        if (navigator.appName == 'Microsoft Internet Explorer') {
            var ua = navigator.userAgent;
            var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
            if (re.exec(ua) != null)
                rv = parseFloat(RegExp.$1);
        }
        return rv;
    }
    function checkIEVersion(display_Item) {
        var ver = getInternetExplorerVersion();
        if (ver > -1) {
            // if (ver == 6.0) {
            ManageDropDownForDialog(display_Item);
            //}
        }
    }

    //set global variable
    var arr_controls;
    var divEffdate = '<%= effdate.FindControl("TextBoxUCDate").ClientId %>';
    var IsNotes = false;

    function ManageDropDownForDialog(display_Item) {
        //resolve IE6 dropdown overlapping
        var selects = document.body.getElementsByTagName("select");
        for (var i = 0; i < selects.length; i++) {
            selects[i].style.display = display_Item;
        }
    }
    function show_confirm() {
        var r = confirm("Address is not yet verified Do you want to continue to save the changes");
        if (r == true) {
            // Anudeep:25.10.2013:BT:2264 - changed to get the Qamatched hidden value Clientid
            $('#' + arr_controls["QASMatched"]).val('true');
        }
        else {
            // Anudeep:25.10.2013:BT:2264 - changed to get the Qamatched hidden value Clientid
            $('#' + arr_controls["QASMatched"]).val('false');
        }
        return r;
    }

    function HandleQASCompletedEvent(address) {
        //alert(document.getElementById('linetwo').value);

        document.getElementById('lineone').value = address.getLineOne();
        document.getElementById('linetwo').value = address.getLineTwo();
        document.getElementById('linethree').value = address.getLineThree();
        document.getElementById('city').value = address.getCity();
        document.getElementById('state').value = address.getState();
        document.getElementById('state_hid').value = address.getState();
        document.getElementById('zip').value = address.getZip().replace("-", "");
        // Anudeep:25.10.2013:BT:2264 - changed to get the Qamatched hidden value Clientid
        if ($('#' + arr_controls["QASMatched"]).val() == 'true') {
            $('#IsbadAddress1').removeAttr('checked');
        }
        $('#state,#country').show();
    }
    //on verify address image button Method
    function VerifyAddress() {
        // Anudeep:25.10.2013:BT:2264 - changed to get the Qamatched hidden value Clientid
        $('#' + arr_controls["QASMatched"]).val('false');
        if (($('#country').val() != "US") || ($('#country').val() == '')) { alert('Country should be US for address verification'); return; }
        //2012.05.21:review changes
        $('#state,#country').hide();
        var cb = document.getElementById("input_search");
        if (cb) {
            $('#divNotes').dialog('widget').hide();
            var evt = document.createEventObject();
            //cb.fireEvent("onClick", evt);
            var a = new com.qas.Address(document.getElementById('lineone').value,
                    document.getElementById('linetwo').value,
                    document.getElementById('linethree').value,
                    document.getElementById('city').value,
                    document.getElementById('state_hid').value,
                    document.getElementById('zip').value);
            events.performQASSearch(a, HandleQASCompletedEvent, HandleQASErrorEvent);
        }


    }
    function HandleQASErrorEvent()
    { $('#state,#country').show(); }
    function performValidationsOnInputControls() {
        <%--Start - Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Storing Current Date and User input date in a variable --%>
        var effDate = $("#" + divEffdate).val();
        var today = new Date();
        var todayDate = new Date(today.getFullYear(), today.getMonth(), today.getDate());
        var inputDate = Date.parse(effDate);
        var currDate = Date.parse(todayDate);
        <%--End - Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Storing Current Date and User input date in a variable --%>
        if ($("#lineone").val().replace(/\s+/g, '') == '') {
            alert('Invalid entry for address1');
            return false;
        }
        if ($("#city").val().replace(/\s+/g, '') == '') {
            alert('Invalid entry for city');
            return false;
        }
        if ($("#country").val() == '') {
            alert('Invalid entry for country');
            return false;
        }
        if ($("#country").val() == 'US') {
            if (isNaN($("#state").val()) == '') {
                alert('Invalid entry for state');
                return false;
            }
            if (validateZipCode($('#zip')[0], 'country') == false) {
                //alert('Invalid entry for zip');
                return false;
            }
        }
        if ($("#country").val() == 'CA') {
            if (isNaN($("#state").val()) == '') {
                alert('Invalid entry for state');
                return false;
            }
            if (validateZipCode($('#zip')[0], 'country') == false) {
                //alert('Invalid entry for zip');
                return false;
        	}
        }

        if ($("#" + divEffdate).val() == '' && $("#treffDate").css('display') != 'none') {
            alert('Invalid entry for effective date');
            return false;
        } 
  
        if (ValidateDate_AddressControl(effDate))  <%-- Manthan Rajguru | 2015.12.15 | YRS-AT-2182 | Validating the effective date is it valid date or not --%>   
        {
            <%--Start - Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Validating two dates and providing message--%>
            if (inputDate > currDate){
                alert('Effective date of address cannot be in the future');
                return false;
            }
            <%--End - Manthan Rajguru | 2015.10.21 | YRS-AT-2182 | Validating two dates and providing message--%>
        }
        else{
            $("#" + divEffdate).val('');
            alert('Invalid entry for effective date');
            return false;
        }            
        return true;
    }

    <%--Start - Manthan Rajguru | 2015.12.15 | YRS-AT-2182 | Function to check invalid date format--%>
    function ValidateDate_AddressControl(dateValue)
    {
       
        var currVal = dateValue;
        if (currVal == '')
            return false;

   
        var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
        var dtArray = currVal.match(rxDatePattern); 

        if (dtArray == null)
            return false;

        dtMonth = dtArray[1];
        dtDay = dtArray[3];
        dtYear = dtArray[5];
      

        if (dtYear < 1900)
            return false;

        if (dtMonth < 1 || dtMonth > 12)
            return false;
        else if (dtDay < 1 || dtDay > 31)
            return false;
        else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
            return false;
        else if (dtMonth == 2) {           
            var isleap = (dtYear % 4 == 0);
             if (dtDay > 29 || (dtDay == 29 && !isleap))
                return false;
        }

        return true;
    }
    <%--End - Manthan Rajguru | 2015.12.15 | YRS-AT-2182 | Function to check invalid date format--%>

    //Modal popup Ok Click functionality check validation and assign modal popup control value to user control values
    function EditAddressOk_Click() {
        // Anudeep:25.10.2013:BT:2264 - Added below code for accessing qamatched and hasaddress hidden variables
        var qasMatched = $('#' + arr_controls["QASMatched"]).val();
        var hasAddressChanged = $('#' + arr_controls["hasAddress"]).val();
        if (performValidationsOnInputControls() == false) {
            return false;
        }
        <% If AllowNote Then%>
        var note = getNoteEnteredByUser();
        if (note == '' && IsNotes == true) {
                alert('Please fill reason and source regarding the address changes.');
                return false;
            }
        if (note != '' && hasAddressChanged == '') {
           alert('Address has not yet changed so you cannot save notes data.');
           return false;
        }
        <% End If %>
        if ($("#country").val() == 'US' && hasAddressChanged == '1' && qasMatched != 'true') {
            //            alert('Please click on the verify address button to validate against the address database.');
            //            return false;
            if (show_confirm() == false) { return false; }
        }
       
        
        //------------ Perform validations - if QAS done or not, zip is correct or not, etc - on sucess copy data to original address controls
        var Add1 = '', Add2 = '', Add3 = '', city = '', state = '', country = '', zip = '';
        Add1 = $("#lineone").val();
        Add2 = $("#linetwo").val();
        Add3 = $("#linethree").val();
        city = $("#city").val();
        state = $("#state_hid").val();
        country = $("#country_hid").val();
        zip = $("#zip").val();
        zip = zip.replace("-", "");
        if ($('#' + arr_controls["TextBoxAddress1"]).val().replace(/\s+/g, '').replace(',', '') != Add1.replace(/\s+/g, '') || $('#' + arr_controls["TextBoxAddress2"]).val().replace(/\s+/g, '').replace(',', '') != Add2.replace(/\s+/g, '') || $('#' + arr_controls["TextBoxAddress3"]).val().replace(/\s+/g, '').replace(',', '') != Add3.replace(/\s+/g, '') || $('#' + arr_controls["TextBoxCity"]).val().replace(/\s+/g, '').replace(',', '') != city.replace(/\s+/g, '') || $('#' + arr_controls["DropDownListState"] + "_hid").val().replace(/\s+/g, '').replace(',', '') != state.replace(/\s+/g, '') || $('#' + arr_controls["DropDownListCountry"] + "_hid").val().replace(/\s+/g, '').replace(',', '') != country.replace(/\s+/g, '') || $('#' + arr_controls["TextBoxZip"]).val().replace(/\s+/g, '').replace(',', '').replace('-', '') != zip.replace(/\s+/g, '') || $('#' + arr_controls["IsbadAddress"]).filter(':checked').length != $("#IsbadAddress1").filter(':checked').length) {
            if (country != 'CA') {
                zip.replace(/\s+/g, '');
            }

            if (zip.length == 9 && country == 'US') {
                zip = zip.substring(0, 5) + "-" + zip.substring(5, 9);
            }

            $('#' + arr_controls["TextBoxAddress1"]).val(Add1 + ',');

            if (Add2.replace(/\s+/g, '') != '') {
                $('#' + arr_controls["TextBoxAddress2"]).val(Add2 + ',');
            }
            else {
                $('#' + arr_controls["TextBoxAddress2"]).val(Add2);
            }

            if (Add3.replace(/\s+/g, '') != '') {
                $('#' + arr_controls["TextBoxAddress3"]).val(Add3 + ',');
            }
            else {
                $('#' + arr_controls["TextBoxAddress3"]).val(Add3);
            }

            if (state != '') {          //'AA:01.10.2013:Bt-2042:Changed to display comma after city if state value exists
                $('#' + arr_controls["TextBoxCity"]).val(city + ',');
            }
            else {
                $('#' + arr_controls["TextBoxCity"]).val(city);
            }


//            if (state != '') {
//                $('#' + arr_controls["DropDownListState"] + "_hid").val(state + ',');
//            }
//            else {
                $('#' + arr_controls["DropDownListState"] + "_hid").val(state);
            //}

            if (country != '') {
                $('#' + arr_controls["DropDownListCountry"] + "_hid").val(country + '.');
            }
            else {
                $('#' + arr_controls["DropDownListCountry"] + "_hid").val(country);
            }

            if (zip != '') {
                $('#' + arr_controls["TextBoxZip"]).val((', ' + zip )); //.val((', ' + zip + ','));   //.replace("-", "")
            }
            else {
                $('#' + arr_controls["TextBoxZip"]).val((zip));    //.replace("-", "")
            }

            //$('#' + arr_controls["DropDownListState"]).val($("#state").val());

            //$('#' + arr_controls["DropDownListCountry"]).val($("#country").val());

            setCountries(arr_controls["DropDownListCountry"], arr_controls["DropDownListState"], $("#country_hid").val());
            setStates(arr_controls["DropDownListCountry"], arr_controls["DropDownListState"], $("#state_hid").val());

            //$('#' + arr_controls["QASMatched"]).val($("#QASMatched").val());
            //$('#' + arr_controls["hasAddress"]).val($("#hasAddress").val());
            $('#' + arr_controls["TextBoxEffDate"]).val($("#" + divEffdate).val());
            <% If AllowNote Then%>
            $('#' + arr_controls["txtFinalNotes"]).val(note);
            if ($("#chkImpt").filter(':checked').length == 1) {
                $('#' + arr_controls["chkNotes"]).val('True'); //.attr('checked', 'checked');
            }
            else {
                $('#' + arr_controls["chkNotes"]).val('False');
            }
            <% End If %>
            if ($("#IsbadAddress1").filter(':checked').length == 1) {
                $('#' + arr_controls["IsbadAddress"]).attr('checked', 'checked');
            }
            else { $('#' + arr_controls["IsbadAddress"]).removeAttr('checked'); }


            $('#' + arr_controls["TextBoxEffDate"]).parent().next().html($('#' + arr_controls["TextBoxEffDate"]).val());
            $('#' + arr_controls["DropDownListCountry"] + ',#' + arr_controls["DropDownListState"] + ',#' + arr_controls["TextBoxZip"] + ',#' + arr_controls["TextBoxAddress1"] + ',#' + arr_controls["TextBoxAddress2"] + ',#' + arr_controls["TextBoxAddress3"] + ',#' + arr_controls["TextBoxCity"] + ',#' + arr_controls["IsbadAddress"]).each(function () {
                if ($(this).is('select')) {
                    if ($('option:selected', this).text() == '- Select -') {
                        $('#' + $(this)[0].id + '_hid').next().html('');
                    } else {
                        $('#' + $(this)[0].id + '_hid').next().html($('option:selected', this).text());
                    }
                } else if ($(this).is(':checkbox')) {
                    var chk = $(this).is(':checked').toString();
                    if (chk == 'false') {
                        $('#' + arr_controls["imgBadAddress"]).css('display', 'none');
                        //return $(this).next().html('No');
                    } else {
                        $('#' + arr_controls["imgBadAddress"]).css('display', 'block');
                        //return $(this).next().html('Yes');
                    }
                }
                else {
                    $(this).next().html($(this).val());
                }
            });
            if (hasAddressChanged == '1') {
                mark_dirty();
                $('#' + arr_controls["hdnChangesExist"]).val('true');
            }
        }
        
        
        
        
        
        //-------------
        $("#divNotes").dialog('close');
        $("#divNotes").dialog('destroy');
        checkIEVersion('inline');
        $('#' + arr_controls["DropDownListCountry"]).css('display','none');
        if (typeof(EnableControls) != 'undefined') {
            EnableControls();
        }
        if (typeof (document.Form1.all.ButtonActivateAsPrimary) != 'undefined') {
            document.Form1.all.ButtonActivateAsPrimary.disabled = true;
        }

        //$('#' + arr_controls["LabelAddress1"]).css('display', 'block');
        //$('#' + arr_controls["LabelEffDate"]).css('display', 'block');
        $('#' + arr_controls["LabelNoAddressFound"]).css('display', 'none');
        $('#' + arr_controls["LabelUpdateAddress"]).css('display', 'none'); 
        arr_controls = null;
    }
    //Address Modal popup for edit address
    function AddressPopUp(hdnpopupheight) {
        $('#divNotes').dialog
                    ({
                        modal: true,
                        autoOpen: true,
                        closeOnEscape: false,
                        title: "Address Add/Edit Information",
                        width: 560, 
                        height: hdnpopupheight,
                        resizable: false,
                        buttons: [{ text: "OK", click: EditAddressOk_Click },
                                    { text: "Cancel", click: function () {
                                        $('#divNotes').dialog('destroy');
                                        checkIEVersion('inline');
                                        settxtReasonval();
                                        settxtSourceval();
                                        $('#' + arr_controls["DropDownListCountry"]).css('display','none');
                                        $('#' + arr_controls["DropDownListState"]).css('display','none');
                                    }
                                    }
                                 ],
                        close: function (event, ui) {
                            checkIEVersion('inline');
                            settxtReasonval();
                            settxtSourceval();
                            $('#' + arr_controls["DropDownListCountry"]).css('display','none');
                            $('#' + arr_controls["DropDownListState"]).css('display','none');
                        }
                    });


        //$('#QASMatched').val('false');
        //$('#hasAddress').val('0');

    }
    //to open address modal popup to set original values of address into modal popup
    // Anudeep:25.10.2013:BT:2264 - Added two variables in method for getting hidden variables of QAmatched and has address to get their client id
    function OpenAddress(TextBoxAddress1, TextBoxAddress2, TextBoxAddress3, TextBoxCity, DropDownListCountry, DropDownListState, TextBoxZip, CheckboxIsBadAddress, TextBoxEffDate, txtFinalNotes, chkNotes, imgBadAddress, LabelNoAddressFound, LabelUpdateAddress, hdnChangesExist, hdnReasons, QASMatched, hasAddress, Addressmode) {
        checkIEVersion('none');
        $('#state').show();
        $('#country').show();
        <% If AllowNote Then%>
        var NotesReason;        
        NotesReason = $('#'+hdnReasons).val().split('$');
        dt_NotesReason = NotesReason;
        setReasons(dt_NotesReason);
        setSources(dt_NotesSource);
        $("#txtSource").val('');
        $("#TxtReason").val('');
        $("#TxtReason").css('display', 'none');
        $("#txtSource").css('display', 'none');
        // Anudeep:25.10.2013:BT:2264 - Added below code for clearing the notes textbox
        $('#txtNotes').val('');
        <% End If %>
        
        // Anudeep:25.10.2013:BT:2264 - Added below code for checking whether address changes does not exists then clears the hasAddress and qamatched hidden variables 
        if ($('#' + hdnChangesExist).val().replace(/\s+/g, '') == '') {
            $('#' + QASMatched).val('');
            $('#' + hasAddress).val('');
        }

        arr_controls = new Object();
        arr_controls["TextBoxAddress1"] = TextBoxAddress1;
        arr_controls["TextBoxAddress2"] = TextBoxAddress2;
        arr_controls["TextBoxAddress3"] = TextBoxAddress3;
        arr_controls["TextBoxCity"] = TextBoxCity;
        arr_controls["DropDownListState"] = DropDownListState;
        arr_controls["DropDownListCountry"] = DropDownListCountry;
        arr_controls["TextBoxZip"] = TextBoxZip;
        // Anudeep:25.10.2013:BT:2264 - Uncommented below line to assinghn clientid's in array
        arr_controls["QASMatched"] = QASMatched;
        arr_controls["hasAddress"] = hasAddress;
        arr_controls["TextBoxEffDate"] = TextBoxEffDate + "_TextBoxUCDate";
        arr_controls["txtFinalNotes"] = txtFinalNotes;
        arr_controls["chkNotes"] = chkNotes;
        arr_controls["IsbadAddress"] = CheckboxIsBadAddress;
        arr_controls["imgBadAddress"] = imgBadAddress;
        //arr_controls["LabelAddress1"] = LabelAddress1;
        //arr_controls["LabelEffDate"] = LabelEffDate;
        arr_controls["LabelNoAddressFound"] = LabelNoAddressFound;
        arr_controls["LabelUpdateAddress"] = LabelUpdateAddress; 
        arr_controls["hdnChangesExist"] = hdnChangesExist;
        arr_controls["Addressmode"] = Addressmode;
        //arr_controls = new Array(TextBoxAddress1, TextBoxAddress2, TextBoxAddress3, TextBoxCity, DropDownListState, DropDownListCountry, TextBoxZip, QASMatched_hid, txtNotes);
        $('#lineone').val($('#' + TextBoxAddress1).val().replace(",", ""));
        $('#linetwo').val($('#' + TextBoxAddress2).val().replace(", ", "").replace(",", ""));
        $('#linethree').val($('#' + TextBoxAddress3).val().replace(", ", "").replace(",", ""));
        $('#city').val($('#' + TextBoxCity).val().replace(",", ""));
        $('#state_hid').val($('#' + DropDownListState + '_hid').val().replace(",", ""));
        $('#country_hid').val($('#' + DropDownListCountry + '_hid').val().replace(",", ""));
        $('#zip').val($('#' + TextBoxZip).val().replace(", ", "").replace(" ,", "").replace(",", "").replace("-", ""));
        <% If AllowNote Then%>
        if ($('#' + Addressmode).val() == "Add") {
            $('#Reason').hide();
            $('#Source').hide();
            $('#LabelReason').hide();
            $('#LabelSource').hide();
        }
        if ($('#' + Addressmode).val() == "Edit") {
            $('#Reason').show();
            $('#Source').show();
            $('#LabelReason').show();
            $('#LabelSource').show();
        }
        <%End If %>
        if ($('#' + TextBoxEffDate + "_TextBoxUCDate").val() != undefined) {
            $('#' + divEffdate).val($('#' + TextBoxEffDate + "_TextBoxUCDate").val());
        }
        else {
            $('#' + 'treffDate').css('display', 'none')
        }

        if ($('#' + CheckboxIsBadAddress).filter(':checked').length == 1) {
            $('#IsbadAddress1').attr('checked', 'checked');
        }
        else {
            $('#IsbadAddress1').removeAttr('checked');
        }
        <% If AllowNote Then%>
        //Notes Field
        if ($('#' + chkNotes).val() == 'True') {
            $('#chkImpt').attr('checked', 'checked');
        } else { $('#chkImpt').removeAttr('checked'); }

        if ($('#' + txtFinalNotes).val() != undefined) {
            if ($('#' + txtFinalNotes).val() != '') {
                setReason(getReason($('#' + txtFinalNotes).val()));
                setSource(getSource($('#' + txtFinalNotes).val()));
                setComment(getComment($('#' + txtFinalNotes).val()));
                
            }
        }
        <%End If %>
        //set dropdown and onchange event
        var country = document.getElementById('country_hid').value;
        var state = document.getElementById('state_hid').value;
        setCountries('country', 'state', country);
        setStates('country', 'state', state);
        //assign event handlers for country select change
        $("#country").change(
			function () {
			    $("#zip").val('');
			    setStates('country', 'state', null);
			});
        //assign event handlers for state select change
        $("#state").change(
			function () {
			    setCountryCodeForStateId('country', 'state', this.value);
			});



    }

    //set reason and source
    function CreateReasonRadiolist(dataTable) {
        var i = 0; var html = ''; var inputRadio; var e;
        for (i = 0; i < dataTable.length; i++) {
            e = dataTable[i];
            html += '<input id="rblReason_' + i + '" type="radio" name="rblReason" value=' + e + ' onclick = "javascript:changeOnNotes();" /><label for="rblReason_' + i + '" class="AddressLabels" >' + e + '</label><br />'
            //setReasons(dt_NotesReason);
        }
        // html += '<input name="TxtReason" type="text" id="TxtReason" class="AddressLabels TextBox_Normal" style="width: 100px;" onchange="javascript:changeOnNotes();" />';
        // document.getElementById("rblReason").innerHTML = html;
        //$('#TxtReason').before(html);

    }
    function CreateSourceRadiolist(dataTable) {
        var i = 0; var html = ''; var inputRadio; var e;
        for (i = 0; i < dataTable.length; i++) {
            e = dataTable[i];
            html += '<input id="rblSource_' + i + '" type="radio" name="rblSource" value=' + e + '  onclick = "javascript:changeOnNotes();" /><label for="rblSource_' + i + '" class="AddressLabels" >' + e + '</label><br />'
            setSources(dt_NotesSource);
        }
        // html += '<input id="txtSource" class="AddressLabels TextBox_Normal" style="width: 100px;" onchange="javascript:changeOnNotes();" />';
        //document.getElementById("rblSource").innerHTML = html;
        //$('#txtSource').before(html);
    }
    //set effective date on notes field change
    function changeOnNotes() {
        var today = new Date();
        var currentDate = (today.getMonth() + 1) + "/" + today.getDate() + "/" + today.getFullYear();
        $("#" + divEffdate).val(currentDate);
        // IsNotes = false;
    }
    //set effective date on addresses field change
    function changesEffectiveAddress() {
        var QASMatched_input_hid, hasAddress_input_hid;
        // Anudeep:25.10.2013:BT:2264 - changed to get the Qamatched & hasAddress hidden value Clientid
        QASMatched_input_hid = arr_controls["QASMatched"]; hasAddress_input_hid = arr_controls["hasAddress"];
        var today = new Date();
        var currentDate = (today.getMonth() + 1) + "/" + today.getDate() + "/" + today.getFullYear();
        $('#' + QASMatched_input_hid + ',#' + hasAddress_input_hid).val("1");
        $("#" + divEffdate).val(currentDate);
        if ($("#" + arr_controls["Addressmode"]).val() == "Edit") {
            IsNotes = true;
        }
        else if ($("#" + arr_controls["Addressmode"]).val() == "Add") {
            IsNotes = false;
        }

    }

    function getNoteEnteredByUser() {
        var l_FinalstrNotes = l_strSourceOtherText = l_strReasonOtherText = l_strSource = l_strReason = '';
        if ($("#Source :selected").val() != undefined) {
            l_strSource = $("#Source :selected").val();
        }
        if ($("#Reason :selected").val() != undefined) {
            l_strReason = $("#Reason :selected").val();
        }
        if ($('#TxtReason').val() != '' && $('#TxtReason').css('color') != 'gray') {
            l_strReasonOtherText = " (" + $('#TxtReason').val() + ")";
        }
        if ($('#txtSource').val() != '' && $('#txtSource').css('color') != 'gray') {
            l_strSourceOtherText = " (" + $('#txtSource').val() + ")";
        }
        //        if (l_strReason != '' || l_strReasonOtherText != '' ||
        //        l_strSource != '' || l_strSourceOtherText != '' || $('#txtNotes').val()) {
        if ($("#" + arr_controls["Addressmode"]).val() == "Edit") {
            if ((l_strReason != '' || l_strReasonOtherText != '') & (l_strSource != '' || l_strSourceOtherText != '')) {
                return "Address Update: Reason-" + l_strReason + l_strReasonOtherText + "; Source-" + l_strSource + l_strSourceOtherText + "; Comment-" + $('#txtNotes').val() + ".";
            } else {
                return '';
            }
        }
        else if ($("#" + arr_controls["Addressmode"]).val() == "Add") {
            if ($('#txtNotes').val().replace(/\s+/g, '') != '') {
                return "Address Add: Comment-" + $('#txtNotes').val() + "."
            }
        }
    }
    (function ($) {
        jQuery.fn.disableControl = function (bDisable) {
            if (bDisable) {
                this.addClass("DisableControl").removeClass("EnableControl");
            } else {
                this.removeClass("DisableControl").addClass("EnableControl");
            }
        }
    })(jQuery);

    $(document).ready(function () {
        <% If AllowNote Then%>
        CreateReasonRadiolist(dt_NotesReason);
        CreateSourceRadiolist(dt_NotesSource);
        if ($.browser.msie && $.browser.version < 8)
            $('input[type=radio],[type=checkbox]').live('click', function () {
                $(this).trigger('change');
            });
        //$("#txtSource").disableControl(true);
        // $("#TxtReason").disableControl(true);
        $("#TxtReason").css('display', 'none');
        $("#txtSource").css('display', 'none');
        //        $(".DisableControl").each(function () { $(this).attr('disabled', 'disabled'); });
        //        $(".EnableControl").each(function () { $(this).removeAttr('disabled', 'disabled'); });
        settxtReasonval();
        settxtSourceval();
        $("#TxtReason").click(function () { clearReasonCtlVal() });
        $("#txtSource").click(function () { clearSourceCtlVal() });
        $("#Source").change(function () { rblSource_Click(); });
        $("#Reason").change(function () { rblReason_Click(); });
        $("#TxtReason").focusout(function () { settxtReasonval(); });
        $("#txtSource").focusout(function () { settxtSourceval(); });
        <% End If %>
        $("#proweb_wgt").detach().appendTo("body");



    });
    function clearReasonCtlVal() {
        if ($("#TxtReason").val() == 'Other' && $('#TxtReason').css('color') == 'gray') {
            $("#TxtReason").val('');
            $("#TxtReason").css('color', 'black');
        }
    }

    function clearSourceCtlVal() {
        //$("#rblSource input:radio").removeAttr('checked', 'checked');
        if ($("#txtSource").val() == 'Other' && $('#txtSource').css('color') == 'gray') {
            $("#txtSource").val('');
            $("#txtSource").css('color', 'black');
        }

    }
    function settxtReasonval() {
        if ($("#TxtReason").val() == '') {
            $("#TxtReason").val('Other');
            $("#TxtReason").css('color', 'gray');
        }

    }

    function settxtSourceval() {
        if ($("#txtSource").val() == '') {
            $("#txtSource").val('Other');
            $("#txtSource").css('color', 'gray');
        }
    }
    function rblSource_Click() {
        if ($("#Source :selected").val() == "Other") {
            $("#txtSource").css('display', 'block');
        }
        else {
            $("#txtSource").css('display', 'none');
            
        }
        settxtSourceval();
        //        $(".DisableControl", "#rblSource").each(function () { $(this).attr('disabled', 'disabled'); });
        //        $(".EnableControl", "#rblSource").each(function () { $(this).removeAttr('disabled', 'disabled'); });
    }
    function rblReason_Click() {
        if ($("#Reason :selected").val() == "Other") {
            //$("#TxtReason").disableControl(false);
            $("#TxtReason").css('display', 'block');
        } else {
            // $('#TxtReason').val('').disableControl(true);
            $("#TxtReason").css('display', 'none');
            
        }
        settxtReasonval();
        //        $(".DisableControl", "#rblReason").each(function () { $(this).attr('disabled', 'disabled'); });
        //        $(".EnableControl", "#rblReason").each(function () { $(this).removeAttr('disabled', 'disabled'); });
    }


    function getReason(str) {
        var start = str.indexOf('Reason-');
        start = start + 'Reason-'.length;
        var end = str.indexOf(';', start);
        return str.substring(start, end);
    }
    function setReason(str) {
        if (str.indexOf('Other') > -1) {
            var substr = str.substring(str.indexOf('(') + 1, str.indexOf(')'));
            str = 'Other';
            $("#TxtReason").val(substr);
            $("#TxtReason").css('display', 'block');
            $("#TxtReason").css('color', 'black');
        }
        $("#Reason option:contains(" + str + ")").attr('selected', 'selected');
        
//        $('#rblReason input:radio').each(function () {
//            // alert($(this).val());
//            if ($(this).val() == str) {
//                $(this)[0].checked = true; //$(this).attr("checked", "checked");
//            } else if ($(this).val() == 'Other' && str.substring(0, 7) == 'Other (') {
//                $(this).attr('checked', 'checked');
//                $('#TxtReason').val(str.substring(7, str.length - 1));
//            }
//        }
//    	);
    }
    function getSource(str) {
        var start = str.indexOf('Source-');
        start = start + 'Source-'.length;
        var end = str.indexOf(';', start);
        return str.substring(start, end);
    }
    function setSource(str) {
        
        if (str.indexOf('Other') > -1) {
            var substr = str.substring(str.indexOf('(') + 1, str.indexOf(')'));
            str = 'Other';
            $("#txtSource").val(substr);
            $("#txtSource").css('display', 'block');
            $("#txtSource").css('color', 'black');
        }
        $("#Source option:contains(" + str + ")").attr('selected', 'selected');
//        $('#rblSource input:radio').each(function () {
//            if ($(this).val() == str) {
//                $(this).attr('checked', 'checked');
//            } else if ($(this).val() == 'Other' && str.substring(0, 7) == 'Other (') {
//                $(this).attr('checked', 'checked');
//                $('#txtSource').val(str.substring(7, str.length - 1));
//            }
//        }
//    	);
    }
    function getComment(str) {
        var start = str.indexOf('Comment-');
        start = start + 'Comment-'.length;
        var end = str.indexOf('.', start);
        return str.substring(start, end);
    }
    function setComment(str) {
        $('#txtNotes').val(str);
    }
  		
</script>
<script src='<%= Me.ResolveClientUrl("~/JS/proweb.js") %>' type="text/javascript"></script>

<%  End If%>

