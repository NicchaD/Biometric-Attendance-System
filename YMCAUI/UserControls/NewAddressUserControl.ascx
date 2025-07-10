<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NewAddressUserControl.ascx.vb"
    Inherits="YMCAUI.NewAddressUserControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="YRSControls" TagName="DateUserControl" Src="~/UserControls/DateUserControl.ascx" %>
<link href="../CSS/proweb.css" rel="stylesheet" type="text/css" />
<div>
    <p>
        <table id="Table1" style="width: 100%;" cellspacing="1" cellpadding="1" border="0" class="Normaltext">
            <tr>
                <td  align="right" colspan="3" >
                    <%If Me.EnableControls() Then%>
                    <input type="button" value="Edit Address"  id="btnEditaddress" class="Button_Normal" onclick="javascript: if (CheckAccess('<%= hdnRights.value %>') == false) { return false;} OpenAddress('<%= TextBoxAddress1.ClientId %>', '<%= TextBoxAddress2.ClientId %>', '<%= TextBoxAddress3.ClientId %>', '<%= TextBoxCity.ClientId %>', '<%= DropDownListCountry.ClientId %>', '<%= DropDownListState.ClientId %>', '<%= TextBoxZip.ClientId %>', '<%= CheckboxIsBadAddress.ClientId %>','<%= TextBoxEffDate.ClientId %>','<%= txtFinalNotes.ClientId %>','<%= chkNotes.ClientId %>');AddressPopUp();" />
                    <%End If%>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top" width="30%">
                    <asp:Label ID="LabelAddress1" CssClass="Label_Small" runat="server">Address</asp:Label>
                </td>
                <td colspan="2" valign="middle">
                    <span>
                        <asp:RequiredFieldValidator ID="reqAddress" runat="server" ErrorMessage="Please enter Address1"
                            ControlToValidate="TextBoxAddress1" Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:TextBox ID="TextBoxAddress1" Style="display: none;" CssClass="TextBox_Normal Warn"
                            runat="server" Width="216px" MaxLength="60"></asp:TextBox><asp:TextBox ID="TextBoxAddress2"
                                Style="display: none;" CssClass="TextBox_Normal Warn" runat="server" Width="216px"
                                MaxLength="60"></asp:TextBox><asp:TextBox ID="TextBoxAddress3" Style="display: none;"
                                    CssClass="TextBox_Normal Warn" runat="server" Width="216px" MaxLength="60"></asp:TextBox><br />
                        <asp:TextBox ID="TextBoxCity" Style="display: none;" CssClass="TextBox_Normal Warn"
                            runat="server" Width="216px" MaxLength="30"></asp:TextBox><asp:RequiredFieldValidator
                                ID="rqCity" runat="server" ErrorMessage="Please enter the city" ControlToValidate="TextBoxCity"
                                Display="Dynamic"></asp:RequiredFieldValidator>
                        <input type="hidden" runat="server" id="DropDownListState_hid" /><div>
                            <select id="DropDownListState" runat="server" class="DropDown_Normal Warn" style="width: 218px;
                                display: none;">
                            </select></div>
                        <asp:TextBox ID="TextBoxZip" Style="display: none;" CssClass="TextBox_Normal Warn"
                            runat="server" Width="120px" MaxLength="10"></asp:TextBox><br>
                        <div>
                            <select id="DropDownListCountry" runat="server" class="DropDown_Normal Warn" style="width: 218px;
                                display: none;">
                            </select></div>
                        <input type="hidden" runat="server" id="DropDownListCountry_hid" />
                    </span>
                </td>
            </tr>
            <%--  <tr>
                <td align="left">
                    <asp:Label ID="LabelAddress2" CssClass="Label_Small" runat="server">Address2</asp:Label>
                </td>
                <td colspan="2" valign="middle">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="LabelAddress3" CssClass="Label_Small" runat="server">Address3</asp:Label>
                </td>
                <td colspan="2" valign="middle">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="LabelCity" CssClass="Label_Small" runat="server">City</asp:Label>
                </td>
                <td colspan="2" valign="middle">
                    
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="LabelState" CssClass="Label_Small" runat="server">State</asp:Label>
                </td>
                <td colspan="2" valign="middle">
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="LabelCountry" CssClass="Label_Small" runat="server">Country</asp:Label>
                </td>
                <td colspan="2" valign="middle">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="LabelZip" CssClass="Label_Small" runat="server">Zip</asp:Label>
                </td>
                <td valign="middle">
                </td>
                <td rowspan="2" align="right">
                </td>
            </tr>--%>
            <%If AllowEffDate Then%>
            <tr>
                <td align="left" Width="100">
                    <asp:Label ID="LabelEffDate" runat="server" CssClass="Label_Small">Effective Date</asp:Label>
                </td>
                <td align="left" valign="middle" colspan="2">
                    <YRSControls:DateUserControl ID="TextBoxEffDate" runat="server"></YRSControls:DateUserControl>
                </td>
            </tr>
            <%End If%>
            <%If AllowNote Then%>
            <tr>
                <td nowrap align="left" colspan="3" valign="middle">
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                </td>
            </tr>
            <%End If%>
            <tr>
                <td nowrap align="left">
                    <asp:Label ID="IsBadAddress1" runat="server" CssClass="Label_Small">Bad Address</asp:Label>
                </td>
                <td nowrap align="left">
                    <asp:CheckBox ID="CheckboxIsBadAddress" style=" font-weight:normal;" runat="server" 
                        Enabled="false" Width="240"></asp:CheckBox>
                </td>
                
            </tr>
            <tr valign="top">
                <td nowrap align="left" colspan="3">
                    <%--<asp:Label ID="LabelNoAddressFound" runat="server" CssClass="Label_Small">No Address information defined. </br>
                    Please click on ‘Edit Address’ to add or edit Address information.</asp:Label>--%>
                    <asp:HiddenField ID="hdnRights" runat="server" />
                </td>
                
            </tr>
        </table> 
    </p>
    <%  If HttpContext.Current.Items("AddressControlDiv") Is Nothing Then
            HttpContext.Current.Items("AddressControlDiv") = "DivAdded"
        
    %>
    <div id="Div1" style="z-index: 1500">
    </div>
</div>
<asp:HiddenField ID="txtFinalNotes" runat="server" />
<asp:HiddenField ID="chkNotes" runat="server" />
<div id="proweb_wgt" style="z-index: 1500">
</div>
<div id="divNotes" style="display: none;">
    <div id="divAddress" class="AddressLabels">
        <input type="hidden" id="QASMatched" name="QASMatched" />
        <input type="hidden" id="hasAddress" name="hasAddress" />
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
            width: 150px;
            height: 300px;
            font-family: Verdana,Tahoma,Arial,Helvetica,sans-serif;
            font-size: 8pt;
        }
    </style>
    <table border="0" class="AddressLabels" cellspacing="10" style="height: 50%">
        <tr>
            <td rowspan="2" valign="top">
                <span class="AddressLabels Label_Small">Address 1:</span><input id="lineone" class="AddressLabels TextBox_Normal"
                    onchange="javascript:changesEffectiveAddress();" /><br />
                <span class="AddressLabels Label_Small">Address 2:</span><input id="linetwo" class="AddressLabels TextBox_Normal"
                    onchange="javascript:changesEffectiveAddress();" /><br />
                <span class="AddressLabels Label_Small">Address 3:</span><input id="linethree" class="AddressLabels TextBox_Normal"
                    onchange="javascript:changesEffectiveAddress();" /><br />
                <span class="AddressLabels Label_Small">City:</span><input id="city" class="AddressLabels TextBox_Normal"
                    onkeypress="javascript:ValidateAlphaNumeric();" onchange="javascript:changesEffectiveAddress();" /><br />
                <span class="AddressLabels Label_Small">State:</span><select id="state" class="AddressLabels DropDown_Normal"
                    style="width: 200px" onchange="javascript:changesEffectiveAddress();"></select><br />
                <span class="AddressLabels Label_Small">Country:</span><select id="country" class="AddressLabels DropDown_Normal"
                    style="width: 200px" onchange="javascript:changesEffectiveAddress();"></select><br />
                <span class="AddressLabels Label_Small">Zip:</span><input id="zip" onkeypress="javascript:ValidateZipCodeKeyPress(this,'country');"
                    onblur="javascript:validateZipCode(this,'country');" class="AddressLabels TextBox_Normal"
                    onchange="javascript:changesEffectiveAddress();" /><br />
                <span class="AddressLabels Label_Small">Eff. Date:</span><YRSControls:DateUserControl
                    runat="server" ID="effdate" cssInput="AddressLabels TextBox_Normal"></YRSControls:DateUserControl>
                <span class="AddressLabels Label_Small" style="display: inline;">Bad address:</span><input
                    type="checkbox" id="IsbadAddress" class="CheckBox_Normal" onclick="javascript:changesEffectiveAddress();" />
                <img src="images/verify2.gif" id="btnVerify" onclick="VerifyAddress();" title="Verify Address"
                    style="margin-bottom: -5px; margin-top: -10px" />
                <input type="hidden" id="Hidden1" />
                <input type="hidden" id="Hidden2" />
                <input id="Text1" value="Submit" style="display: none" />
            </td>
            <td rowspan="2" style="background-color: Orange">
            </td>
            <td valign="top">
                <span id="LabelReason" class="Label_Small">Reason</span>
                <div id="rblReason" style="border-style: None; width: 150px">
                    <input name="TxtReason" type="text" id="TxtReason" class="AddressLabels TextBox_Normal"
                        style="width: 100px;" onchange="javascript:changeOnNotes();" />
                </div>
            </td>
            <td rowspan="2" style="background-color: Orange">
            </td>
            <td rowspan="2" valign="top">
                <span id="LabelNotes" class="Label_Small">Notes</span><br />
                <span class="CheckBox_Normal">
                    <input id="chkImpt" type="checkbox" name="chkImpt" onclick="javascript:changeOnNotes();" /><label
                        for="chkImpt" class="AddressLabels">Important</label></span><br />
                <textarea cols="" name="txtNotes" rows="" id="txtNotes" class="AddressLabels TextBox_Normal"></textarea>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <span id="LabelSource" class="Label_Small">Source</span>
                <div id="rblSource" style="border-style: None; width: 150px">
                    <input id="txtSource" class="AddressLabels TextBox_Normal" style="width: 100px;"
                        onchange="javascript:changeOnNotes();" />
                </div>
            </td>
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
    function changeSelect(fieldID, newOptions, defSelection) {
        selectField = document.getElementById(fieldID);
        if (selectField == null) return;
        selectField.options.length = 0;
        selectField.options[0] = new Option("- Select one -", "");
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
        if (strCountryCode == null || strCountryCode == "" || txtZipCodeControl.value == "") return false;
        if (strCountryCode == "US") {
            //Validate if zipCode is the right format for US
            return ValidateNumeric();
        } else if (strCountryCode == "CA") {
            //Validate if zipCode is the right format for CA
            return ValidateAlphaNumeric();
        }
        return ValidateAlphaNumeric();
    }

    function validateZipCode(txtZipCodeControl, drdCountryCode) {
        var strCountryCode = document.getElementById(drdCountryCode).value;
        //If country code is not specified then do not validate zipCode
        if (strCountryCode == null || strCountryCode == "" || txtZipCodeControl.value == "") return false;
        if (strCountryCode == "US") {
            //Validate if zipCode is the right format for US
            if (/^[0-9]{5,5}$|^[0-9]{5,5}[0-9]{4,4}$/.test(txtZipCodeControl.value.replace(/\s+/g, '')) == false) {
                alert('Invalid zip code. Please enter zip in 99999 or 999999999 format.');
                return false;
            }
        } else if (strCountryCode == "CA") {
            //Validate if zipCode is the right format for CA
            if (/^[a-zA-Z][0-9][a-zA-Z] [0-9][a-zA-Z][0-9]$/.test(txtZipCodeControl.value.replace(/\s+/g, '')) == false) {
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
                if ($('option:selected', this).text() == '- Select one -') {
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
        dt_NotesReason = NotesReason;
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
            $('#QASMatched').val('true');
        }
        else {
            $('#QASMatched').val('false');
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
        if ($('#QASMatched').val() == 'true') {
            $('#IsbadAddress1').removeAttr('checked');
        }
        $('#state,#country').show();
    }
    //on verify address image button Method
    function VerifyAddress() {
        $('#QASMatched').val('false');
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

        if ($("#lineone").val() == '') {
            alert('Invalid entry for address1');
            return false;
        }
        if ($("#city").val() == '') {
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
                alert('Invalid entry for zip');
                return false;
            }
        }
        if ($("#" + divEffdate).val() == '') {
            alert('Invalid entry for effective date');
            return false;
        }

        return true;
    }
    //Modal popup Ok Click functionality check validation and assign modal popup control value to user control values
    function EditAddressOk_Click() {
        var qasMatched = $('#QASMatched').val();
        var hasAddressChanged = $('#hasAddress').val();
        if (performValidationsOnInputControls() == false) {
            return false;
        }
        var note = getNoteEnteredByUser();
        if (note == '' && IsNotes == true) {
            alert('Please fill reason and source regarding the address changes.');
            return false;
        }
        if (note != '' && hasAddressChanged == '') {
            alert('Address has not yet changed so you cannot save notes data.');
            return false;
        }

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

        //DInesh.k          20-Jun-2013     BUGID:2088:Comma value gets inserted in to the database.
        if (Add1 != '') {
            $('#' + arr_controls["TextBoxAddress1"]).val(Add1 + ', ');
            $('#' + arr_controls["TextBoxAddress1"]).val($("#lineone").val() + ', ');
        }
        else {
            $('#' + arr_controls["TextBoxAddress1"]).val(Add1);
            $('#' + arr_controls["TextBoxAddress1"]).val($("#lineone").val());
        }

        if (Add2 != '') {
            $('#' + arr_controls["TextBoxAddress2"]).val(Add2 + ', ');
        }
        else {
            $('#' + arr_controls["TextBoxAddress2"]).val(Add2);
        }

        if (Add3 != '') {
            $('#' + arr_controls["TextBoxAddress3"]).val(Add3 + ', ');
        }
        else {
            $('#' + arr_controls["TextBoxAddress3"]).val(Add3);
        }

        if (city != '') {
            $('#' + arr_controls["TextBoxCity"]).val(city + ', ');
        }
        else {
            $('#' + arr_controls["TextBoxCity"]).val(city);
        }

        if (zip != '' && state != '') {
            $('#' + arr_controls["TextBoxZip"]).val((', ' + zip + ', ').replace("-", ""));    //.replace("-", "")
        }
        else if (zip != '') {
            $('#' + arr_controls["TextBoxZip"]).val((zip + ', ').replace("-", ""));    //.replace("-", "")
        }
        else {
            $('#' + arr_controls["TextBoxZip"]).val((zip).replace("-", ""));    //.replace("-", "")
        }

        if (state != '') {
            $('#' + arr_controls["DropDownListState"] + "_hid").val(state + ', ');
        }
        else {
            $('#' + arr_controls["DropDownListState"] + "_hid").val(state);
        }

        if (country != '') {
            $('#' + arr_controls["DropDownListCountry"] + "_hid").val(country + '.');
        }
        else {
            $('#' + arr_controls["DropDownListCountry"] + "_hid").val(country);
        }

        setCountries(arr_controls["DropDownListCountry"], arr_controls["DropDownListState"], $("#country_hid").val());
        setStates(arr_controls["DropDownListCountry"], arr_controls["DropDownListState"], $("#state_hid").val());

        //$('#' + arr_controls["QASMatched"]).val($("#QASMatched").val());
        //$('#' + arr_controls["hasAddress"]).val($("#hasAddress").val());
        $('#' + arr_controls["TextBoxEffDate"]).val($("#" + divEffdate).val());
        $('#' + arr_controls["txtFinalNotes"]).val(note);
        if ($("#chkImpt").filter(':checked').length == 1) {
            $('#' + arr_controls["chkNotes"]).val('True'); //.attr('checked', 'checked');
        }
        else {
            $('#' + arr_controls["chkNotes"]).val('False');
        }
        if ($("#IsbadAddress1").filter(':checked').length == 1) {
            $('#' + arr_controls["IsbadAddress1"]).attr('checked', 'checked');
        }
        else { $('#' + arr_controls["IsbadAddress1"]).removeAttr('checked'); }


        $('#' + arr_controls["TextBoxEffDate"]).parent().next().html($('#' + arr_controls["TextBoxEffDate"]).val());
        $('#' + arr_controls["DropDownListCountry"] + ',#' + arr_controls["DropDownListState"] + ',#' + arr_controls["TextBoxZip"] + ',#' + arr_controls["TextBoxAddress1"] + ',#' + arr_controls["TextBoxAddress2"] + ',#' + arr_controls["TextBoxAddress3"] + ',#' + arr_controls["TextBoxCity"] + ',#' + arr_controls["IsbadAddress1"]).each(function () {
            if ($(this).is('select')) {
                if ($('option:selected', this).text() == '- Select one -') {
                    $('#' + $(this)[0].id + '_hid').next().html('');
                } else {
                    $('#' + $(this)[0].id + '_hid').next().html($('option:selected', this).text());
                }
            } else if ($(this).is(':checkbox')) {
                var chk = $(this).is(':checked').toString();
                if (chk == 'false') {
                    return $(this).next().html('No');
                } else {
                    return $(this).next().html('Yes');
                }
            }
            else {
                $(this).next().html($(this).val());
            }
        }
        );
        if (hasAddressChanged == '1') {
            mark_dirty();
        }
        
        arr_controls = null;
        
        
        
        //-------------
        $("#divNotes").dialog('close');
        $("#divNotes").dialog('destroy');
        checkIEVersion('inline');
        if (typeof(EnableControls) != 'undefined') {
            EnableControls();
        }
        if (typeof (document.Form1.all.ButtonActivateAsPrimary) != 'undefined') {
            document.Form1.all.ButtonActivateAsPrimary.disabled = true;
        }

    }
    //Address Modal popup for edit address
    function AddressPopUp() {
        $('#divNotes').dialog
                    ({
                        modal: true,
                        autoOpen: true,
                        closeOnEscape: false,
                        title: "Address Edit Information",
                        width: 650, height: 500,
                        resizable: false,
                        buttons: [{ text: "OK", click: EditAddressOk_Click },
                                    { text: "Cancel", click: function () {
                                        $('#divNotes').dialog('destroy');
                                        checkIEVersion('inline');
                                        clearCtlVal();
                                    }
                                    }
                                 ],
                        close: function (event, ui) {
                            checkIEVersion('inline');
                            clearCtlVal();
                        }
                    });


        //$('#QASMatched').val('false');
        //$('#hasAddress').val('0');

    }
    //to open address modal popup to set original values of address into modal popup
    function OpenAddress(TextBoxAddress1, TextBoxAddress2, TextBoxAddress3, TextBoxCity, DropDownListCountry, DropDownListState, TextBoxZip, CheckboxIsBadAddress, TextBoxEffDate, txtFinalNotes, chkNotes) {
        checkIEVersion('none');
        $('#state').show();
        $('#country').show();
        arr_controls = new Object();
        arr_controls["TextBoxAddress1"] = TextBoxAddress1;
        arr_controls["TextBoxAddress2"] = TextBoxAddress2;
        arr_controls["TextBoxAddress3"] = TextBoxAddress3;
        arr_controls["TextBoxCity"] = TextBoxCity;
        arr_controls["DropDownListState"] = DropDownListState;
        arr_controls["DropDownListCountry"] = DropDownListCountry;
        arr_controls["TextBoxZip"] = TextBoxZip;
        //arr_controls["QASMatched"] = QASMatched;
        //arr_controls["hasAddress"] = hasAddress;
        arr_controls["TextBoxEffDate"] = TextBoxEffDate + "_TextBoxUCDate";
        arr_controls["txtFinalNotes"] = txtFinalNotes;
        arr_controls["chkNotes"] = chkNotes;
        arr_controls["IsbadAddress"] = CheckboxIsBadAddress;
        //arr_controls = new Array(TextBoxAddress1, TextBoxAddress2, TextBoxAddress3, TextBoxCity, DropDownListState, DropDownListCountry, TextBoxZip, QASMatched_hid, txtNotes);
        //Replace Function Added by Dinesh.k on 20/06/2013
        //DInesh.k          20-Jun-2013     BUGID:2088:Comma value gets inserted in to the database.
        $('#lineone').val($('#' + TextBoxAddress1).val().replace(',', ''));
        $('#linetwo').val($('#' + TextBoxAddress2).val().replace(',', ''));
        $('#linethree').val($('#' + TextBoxAddress3).val().replace(',', ''));
        $('#city').val($('#' + TextBoxCity).val().replace(',', ''));
        $('#state_hid').val($('#' + DropDownListState + '_hid').val());
        $('#country_hid').val($('#' + DropDownListCountry + '_hid').val());
        $('#zip').val($('#' + TextBoxZip).val().replace(',', '').replace(' ,', '').replace(',', '').replace(', ', ''));
        $('#' + divEffdate).val($('#' + TextBoxEffDate + "_TextBoxUCDate").val());

        if ($('#' + CheckboxIsBadAddress).filter(':checked').length == 1) {
            $('#IsbadAddress1').attr('checked', 'checked');
        }
        else {
            $('#IsbadAddress1').removeAttr('checked');
        }
        //Notes Field
        if ($('#' + chkNotes).val() == 'True') {
            $('#chkImpt').attr('checked', 'checked');
        } else { $('#chkImpt').removeAttr('checked'); }

        if ($('#' + txtFinalNotes).val() != undefined) {
            setReason(getReason($('#' + txtFinalNotes).val()));
            setSource(getSource($('#' + txtFinalNotes).val()));
            setComment(getComment($('#' + txtFinalNotes).val()));


        }

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
        }
        // html += '<input name="TxtReason" type="text" id="TxtReason" class="AddressLabels TextBox_Normal" style="width: 100px;" onchange="javascript:changeOnNotes();" />';
        // document.getElementById("rblReason").innerHTML = html;
        $('#TxtReason').before(html);

    }
    function CreateSourceRadiolist(dataTable) {
        var i = 0; var html = ''; var inputRadio; var e;
        for (i = 0; i < dataTable.length; i++) {
            e = dataTable[i];
            html += '<input id="rblSource_' + i + '" type="radio" name="rblSource" value=' + e + '  onclick = "javascript:changeOnNotes();" /><label for="rblSource_' + i + '" class="AddressLabels" >' + e + '</label><br />'
        }
        // html += '<input id="txtSource" class="AddressLabels TextBox_Normal" style="width: 100px;" onchange="javascript:changeOnNotes();" />';
        //document.getElementById("rblSource").innerHTML = html;
        $('#txtSource').before(html);
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
        QASMatched_input_hid = 'QASMatched'; hasAddress_input_hid = 'hasAddress';
        var today = new Date();
        var currentDate = (today.getMonth() + 1) + "/" + today.getDate() + "/" + today.getFullYear();
        $('#' + QASMatched_input_hid + ',#' + hasAddress_input_hid).val("1");
        $("#" + divEffdate).val(currentDate);
        IsNotes = true
    }

    function getNoteEnteredByUser() {
        var l_FinalstrNotes = l_strSourceOtherText = l_strReasonOtherText = l_strSource = l_strReason = '';
        if ($("#rblSource input:radio:checked").val() != undefined) {
            l_strSource = $("#rblSource input:radio:checked").val();
        }
        if ($("#rblReason input:radio:checked").val() != undefined) {
            l_strReason = $("#rblReason input:radio:checked").val();
        }
        if ($('#TxtReason').val() != '') {
            l_strReasonOtherText = " (" + $('#TxtReason').val() + ")";
        }
        if ($('#txtSource').val() != '') {
            l_strSourceOtherText = " (" + $('#txtSource').val() + ")";
        }
        //        if (l_strReason != '' || l_strReasonOtherText != '' ||
        //        l_strSource != '' || l_strSourceOtherText != '' || $('#txtNotes').val()) {
        if ((l_strReason != '' || l_strReasonOtherText != '') & (l_strSource != '' || l_strSourceOtherText != '')) {
            return "Address Update: Reason-" + l_strReason + l_strReasonOtherText + "; Source-" + l_strSource + l_strSourceOtherText + "; Comment-" + $('#txtNotes').val() + ".";
        } else {
            return '';
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
        CreateReasonRadiolist(dt_NotesReason);
        CreateSourceRadiolist(dt_NotesSource);
        if ($.browser.msie && $.browser.version < 8)
            $('input[type=radio],[type=checkbox]').live('click', function () {
                $(this).trigger('change');
            });
        //$("#txtSource").disableControl(true);
        // $("#TxtReason").disableControl(true);
        $("#TxtReason").attr('disabled', 'disabled');
        $("#txtSource").attr('disabled', 'disabled');
        //        $(".DisableControl").each(function () { $(this).attr('disabled', 'disabled'); });
        //        $(".EnableControl").each(function () { $(this).removeAttr('disabled', 'disabled'); });

        $("#rblSource").click(function () { rblSource_Click(); });
        $("#rblReason").click(function () { rblReason_Click(); });

        $("#proweb_wgt").detach().appendTo("body");



    });
    function clearCtlVal() {

        $("#rblSource input:radio").removeAttr('checked', 'checked');
        $("#txtSource").val('');

        $("#rblReason input:radio").removeAttr('checked', 'checked');
        $("#TxtReason").val('');
    }
    function rblSource_Click() {
        if ($("#rblSource input:radio:checked").val() == "Other") {
            $("#txtSource").removeAttr('disabled', 'disabled');
        }
        else {
            $("#txtSource").attr('disabled', 'disabled');
            $("#txtSource").val('');
        }
        //        $(".DisableControl", "#rblSource").each(function () { $(this).attr('disabled', 'disabled'); });
        //        $(".EnableControl", "#rblSource").each(function () { $(this).removeAttr('disabled', 'disabled'); });
    }
    function rblReason_Click() {
        if ($("#rblReason input:radio:checked").val() == "Other") {
            //$("#TxtReason").disableControl(false);
            $("#TxtReason").removeAttr('disabled', 'disabled');
        } else {
            // $('#TxtReason').val('').disableControl(true);
            $("#TxtReason").attr('disabled', 'disabled');
            $("#TxtReason").val('');
        }
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
        $('#rblReason input:radio').each(function () {
            // alert($(this).val());
            if ($(this).val() == str) {
                $(this)[0].checked = true; //$(this).attr("checked", "checked");
            } else if ($(this).val() == 'Other' && str.substring(0, 7) == 'Other (') {
                $(this).attr('checked', 'checked');
                $('#TxtReason').val(str.substring(7, str.length - 1));
            }
        }
    	);
    }
    function getSource(str) {
        var start = str.indexOf('Source-');
        start = start + 'Source-'.length;
        var end = str.indexOf(';', start);
        return str.substring(start, end);
    }
    function setSource(str) {
        $('#rblSource input:radio').each(function () {
            if ($(this).val() == str) {
                $(this).attr('checked', 'checked');
            } else if ($(this).val() == 'Other' && str.substring(0, 7) == 'Other (') {
                $(this).attr('checked', 'checked');
                $('#txtSource').val(str.substring(7, str.length - 1));
            }
        }
    	);
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
<script src="../JS/proweb.js" type="text/javascript"></script>
<%  End If%>