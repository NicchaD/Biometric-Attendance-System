<%@ Control Language="vb" AutoEventWireup="false"  Codebehind="AddressWebUserControl.ascx.vb" Inherits="YMCAUI.AddressWebUserControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<link href="CSS/proweb.css" rel="stylesheet" type="text/css" />
<script language="javascript">
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
<script language="javascript">
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
	function ValidateNumeric() {
		if ((event.keyCode < 48) || (event.keyCode > 57)) {
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
			if (/^[0-9]{5,5}$|^[0-9]{5,5}[0-9]{4,4}$/.test(txtZipCodeControl.value) == false) {
				alert('Invalid zip code. Please enter zip in 99999 or 999999999 format.');
				return false;
			}
		} else if (strCountryCode == "CA") {
			//Validate if zipCode is the right format for CA
			if (/^[a-zA-Z][0-9][a-zA-Z] [0-9][a-zA-Z][0-9]$/.test(txtZipCodeControl.value) == false) {
				alert('Invalid zip code. Please enter zip in a9a 9a9 format.');
				return false;
			}
		}
		return true;
	}
	function initializeAddressControl(drdCountry, drdState, txtCountry_hid, txtState_hid, txtZip) {
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
<%-- Warn Class added by prasad :2011-08-26:For BT-895,YRS 5.0-1364 : prompt user to if changes not saved--%>
<P>
	<TABLE id="Table1" style="WIDTH: 360px; HEIGHT: 192px" cellSpacing="1" cellPadding="1"
		width="360" border="0">
		<TR>
			<TD style="WIDTH: 99px; HEIGHT: 23px" align="left"><asp:label id="LabelAddress1" CssClass="Label_Small" runat="server">Address1</asp:label></TD>
			<TD style="HEIGHT: 23px"><asp:textbox id="TextBoxAddress1" CssClass="TextBox_Normal Warn" runat="server" Width="216px" MaxLength="60"></asp:textbox>&nbsp;<asp:requiredfieldvalidator id="reqAddress" runat="server" ErrorMessage="Please enter Address1" ControlToValidate="TextBoxAddress1"
					Display="Dynamic">*</asp:requiredfieldvalidator></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 99px; HEIGHT: 26px" align="left"><asp:label id="LabelAddress2" CssClass="Label_Small" runat="server">Address2</asp:label></TD>
			<TD style="HEIGHT: 26px"><asp:textbox id="TextBoxAddress2" CssClass="TextBox_Normal Warn" runat="server" Width="216px" MaxLength="60"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 99px; HEIGHT: 22px" align="left"><asp:label id="LabelAddress3" CssClass="Label_Small" runat="server">Address3</asp:label></TD>
			<TD style="HEIGHT: 22px"><asp:textbox id="TextBoxAddress3" CssClass="TextBox_Normal Warn" runat="server" Width="216px" MaxLength="60"></asp:textbox></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 99px; HEIGHT: 23px" align="left"><asp:label id="LabelCity" CssClass="Label_Small" runat="server">City</asp:label></TD>
			<TD style="HEIGHT: 23px"><asp:textbox id="TextBoxCity" CssClass="TextBox_Normal Warn" runat="server" Width="216px" MaxLength="30"></asp:textbox>&nbsp;<asp:requiredfieldvalidator id="rqCity" runat="server" ErrorMessage="Please enter the city" ControlToValidate="TextBoxCity"
					Display="Dynamic">*</asp:requiredfieldvalidator>&nbsp;</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 99px; HEIGHT: 29px" align="left"><asp:label id="LabelState" CssClass="Label_Small" runat="server">State</asp:label></TD>
			<TD style="HEIGHT: 29px">
			<select id="DropDownListState" runat="server" class="DropDown_Normal Warn" style="width:218px"></select>
			&nbsp;&nbsp;
			</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 99px; HEIGHT: 1px" align="left"><asp:label id="LabelCountry" CssClass="Label_Small" runat="server">Country</asp:label></TD>
			<TD style="HEIGHT: 1px"><Select id="DropDownListCountry" runat="server" class="DropDown_Normal Warn" style="width:218px"></select>&nbsp;</TD>
		</TR>
		<TR>
			<TD style="WIDTH: 99px" align="left"><asp:label id="LabelZip" CssClass="Label_Small" runat="server">Zip</asp:label></TD>
			<TD><asp:textbox id="TextBoxZip" CssClass="TextBox_Normal Warn" runat="server" Width="216px" MaxLength="10"></asp:textbox>
			</TD>
            <td rowspan="2">
             <img title="Verify Address"  src="images/verify2.gif" id="btnVerify"  onclick="javascript:VerifyAddress('<%= TextBoxAddress1.ClientId %>', '<%= TextBoxAddress2.ClientId %>', '<%= TextBoxAddress3.ClientId %>', '<%= TextBoxCity.ClientId %>', '<%= DropDownListCountry.ClientId %>', '<%= DropDownListState.ClientId %>','<%= DropDownListCountry_hid.ClientId %>','<%= DropDownListState_hid.ClientId %>', '<%= TextBoxZip.ClientId %>','<%= Is_QASMatched.ClientId %>');" />
            </td>
      
		</TR>

        <TR>
			<TD style="WIDTH: 99px" align="left"><asp:HiddenField ID="Is_QASMatched" runat="server"  /></TD>
			<TD></TD>

		</TR>

		<input type="hidden" runat="server" id="DropDownListCountry_hid" />
		<input type="hidden" runat = "server" id="DropDownListState_hid" />
       
	</TABLE>
</P>
<%  If HttpContext.Current.Items("AddressControlDiv") Is Nothing Then
        HttpContext.Current.Items("AddressControlDiv") = "DivAdded"
%>
<%--<div id="proweb_wgt" style="z-index: 1500"></div>--%>
<div id="proweb_wgt"></div>
<%--<div id="divNotes" style="display: none;">--%>

<%--    <div id="input_wgt" class="proweb" style="float: left;">--%>
    <div id="input_wgt" class="proweb">
        <input type="hidden" id="QASMatched"/>
	    <input type="hidden" id="lineone"/><br />
	    <input type="hidden" id="linetwo"/><br />
	    <input type="hidden" id="linethree"/><br />
	    <input type="hidden" id="city"/><br />
	    <input type="hidden" id="zip"/><br />
        <input type="hidden" id="country" />
	    <input type="hidden" id="state" />
	    <input type="hidden" id="country_hid" />
	    <input type="hidden" id="state_hid" />
	    <input type="hidden"id="input_search" value="Submit" style="display: none" />
    </div>
<%--</div>--%>
<script language='javascript' type="text/javascript">
    $(document).ready(function () {
        $("#proweb_wgt").detach().appendTo("body");
        $("#input_wgt").detach().appendTo("body");
       
       
    });

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
            if (ver == 6.0) {
                ManageDropDownForDialog(display_Item);
            }
        }
    }

    function ManageDropDownForDialog(display_Item) {
        //resolve IE6 dropdown overlapping
        var selects = document.body.getElementsByTagName("select");
        for (var i = 0; i < selects.length; i++) {
            selects[i].style.display = display_Item;
        }
    }
    var arr_controls;
    function VerifyAddress(TextBoxAddress1, TextBoxAddress2, TextBoxAddress3, TextBoxCity, DropDownListCountry, DropDownListState, DropDownListCountry_hid, DropDownListState_hid, TextBoxZip, Is_QASMatched) {
        $('#QASMatched').val('false');
        $('#' + Is_QASMatched).val('false');
        //set address control value into input hidden fields for verify address detail
        $('#lineone').val($('#' + TextBoxAddress1).val());
        $('#linetwo').val($('#' + TextBoxAddress2).val());
        $('#linethree').val($('#' + TextBoxAddress3).val());
        $('#city').val($('#' + TextBoxCity).val());
        $('#state_hid').val($('#' + DropDownListState + '_hid').val());
        $('#country_hid').val($('#' + DropDownListCountry + '_hid').val());
        $('#state').val($('#' + DropDownListState).val());
        $('#country').val($('#' + DropDownListCountry).val());
        $('#zip').val($('#' + TextBoxZip).val());

        if (($('#country').val() != "US") || ($('#country').val() == '')) { alert('Country should be US for address verification'); return; }
        checkIEVersion('none');
        var cb = document.getElementById("input_search");
        if (cb) {
            arr_controls = new Array(TextBoxAddress1, TextBoxAddress2, TextBoxAddress3, TextBoxCity, DropDownListCountry, DropDownListState, DropDownListCountry_hid, DropDownListState_hid, TextBoxZip, Is_QASMatched);
            var evt = document.createEventObject();
            //$('#divNotes').dialog('widget').hide();
            // cb.fireEvent("onClick", evt);
            var a = new com.qas.Address(document.getElementById('lineone').value,
                    document.getElementById('linetwo').value,
                    document.getElementById('linethree').value,
                    document.getElementById('city').value,
                    document.getElementById('state_hid').value,
                    document.getElementById('zip').value);
            events.performQASSearch(a, HandleQASCompletedEvent, HandleQASErrorEvent);
        }
    }
    //on verify address image button Method
    function HandleQASErrorEvent()
    { checkIEVersion('inline'); }

    function HandleQASCompletedEvent(address) {
        document.getElementById(arr_controls[0]).value = address.getLineOne();
        document.getElementById(arr_controls[1]).value = address.getLineTwo();
        document.getElementById(arr_controls[2]).value = address.getLineThree();
        document.getElementById(arr_controls[3]).value = address.getCity();
        document.getElementById(arr_controls[5]).value = address.getState();
        document.getElementById(arr_controls[7]).value = address.getState();
        document.getElementById(arr_controls[8]).value = address.getZip().replace("-", "");
        var QAS_Matched = $('#QASMatched').val();
        document.getElementById(arr_controls[9]).value = QAS_Matched;
        checkIEVersion('inline');
    }
</script>
<script src="JS/proweb.js" type="text/javascript"></script>
<% End If%>