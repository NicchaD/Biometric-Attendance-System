
/**************************************************************************************************
* QAS best practice proweb implementation application. This javascript is designed to be run with
* the accompanying html (as found in proweb.html). The code is split into generic objects (com.qas),
* logic (business), event handling (events) and presentation (interface). All style changes should
* be made through proweb.css.
* @author Toby Mostyn
**************************************************************************************************/
// Set the "namespaces" (these mimick the java domain model
var com = new Object();
com.qas = new Object();
var interface = new Object();
var business = new Object();
var events = new Object();

// Constant variables
com.qas.READY_STATE_UNINITIALISED = 0;
com.qas.READY_STATE_LOADING = 1;
com.qas.READY_STATE_LOADED = 2;
com.qas.READY_STATE_INTERACTIVE = 3;
com.qas.READY_STATE_COMPLETE = 4;
com.qas.PROMPTSET = "Default";
com.qas.COUNTRYSET = "USA";
com.qas.MAXIMUM_CONNECTION_ATTEMPTS = 3;

// Variables set by the "constant" DOM objects (treated as variables)
com.qas.AJAXURL = "ProwebHandler.aspx";  //"http://localhost:4247/ProwebHandler.aspx";     //"http://yrsaddredit.ymcaret.org/qas/prowebproxy.aspx"; //document.getElementById("AJAXURL").value;BS:2012.04.18:-1470
com.qas.ADDRESSLAYOUT = "Database layout";  //document.getElementById("ADDRESSLAYOUT").value;
com.qas.FORM_BUTTON_ID = ""; //document.getElementById("FORM_BUTTON_ID").value;
com.qas.RUN_AS_POPUP = "true"; //document.getElementById("RUN_AS_POPUP").value;
com.qas.IGNORE_UNMATCHED_ADDRESSES = "false"; //document.getElementById("IGNORE_UNMATCHED_ADDRESSES").value;
com.qas.IGNORE_CONNECTION_ERROR = "false"; //document.getElementById("IGNORE_CONNECTION_ERROR").value;

// All messages used by the application are stored as constants here for ease of manipulation
com.qas.MSG_PREMPARTIAL = "According to the USPS® the address you entered is missing important "
	+ "secondary information such as an Apartment or Suite. Please enter the missing "
	+ "information in the box below and click 'Search', or choose one of the options on the right.";
com.qas.MSG_PREMPARTIAL_ERROR = "The secondary number you entered could not be found. Please refer "
	+ "to list of options below.";
com.qas.MSG_STREETPARTIAL = "According to the USPS® the street number you entered is either missing "
	+ "or incorrect. Please enter the street number in the box below and click 'Search', or "
	+ "choose one of the options on the right to proceed.";
com.qas.MSG_STREETPARTIAL_ERROR = "The street number you entered could not be found. Please refer "
	+ "to list of options below.";
com.qas.MSG_MULITPLE = "According to the USPS® the address you entered is missing important "
	+ "information in order to match to a single address. Please choose the correct address "
	+ "below or click 'Search', or choose one of the options on the right";
com.qas.MSG_REFINE = "Your address selection covers a range of addresses. Enter your exact details.";
com.qas.MSG_REFINE_ERROR = "Invalid range. Please input another refinement number.";
com.qas.MSG_INTERACTION = "According to the USPS® the address you entered may be incorrect. "
	+ "Please choose one of the options below to proceed. "
com.qas.MSG_NONE = "A match for your address could not be found in the USPS database. "
	+ "Please choose one of the options below to proceed.";
com.qas.WAIT_MESSAGE = "Searching QAS proweb database...";

// Variable to hold the address that was originally entered
var originaladdress;

// Variable to hold the orginal third party button click events
existing_button_onclick = "";

// Set the state of the seach (initally false)
is_verified = false;

// Hold the verify level as a global var so that it can be referred to as the last action
current_widget = "";

// Variable to hold the last verify level
verify_level = "";

// Variable to hold the current search moniker (for refine prompting)
var current_moniker = "";

// Variable to count tries to the server
number_of_connection_attempts = 0;

/**************************************************************************************************
* Constructor for the com.qas.ContentLoader object. This handles all interaction with the server
* and ensures that the return codes are correctly handled.
* @param url The url for the server call
* @param onload The name of the method in the event of success
* @param onerror The name of the method to call in the event of failure
* @param method Http submit method (get/ post)
* @param params An array of parameters to send
* @param contentType The http requeest content type
**************************************************************************************************/
com.qas.ContentLoader = function (onLoad, onError, method, params, contentType) {
	this.req = null;
	this.onLoad = onLoad;
	this.onError = (onError) ? onError : this.defaultError;
	this.url = com.qas.AJAXURL;
	this.loadXMLDoc(method, params, contentType);
}

com.qas.ContentLoader.prototype =
{
	/**
	* Private method to make the http request and call the specified onLoad/ onError as
	* appropriate (also sets the content type).
	*/
	loadXMLDoc: function (method, params, contentType) {
		// Parameters to store the last method call to handle re-submits
		this.last_method = method;
		this.last_params = params;
		this.last_content = contentType;

		if (!method) {
			method = "GET";
		}
		if (!contentType && method == "POST") {
			contentType = "application/x-www-form-urlencoded";
		}
		if (window.XMLHttpRequest) {
			this.req = new XMLHttpRequest();
		}
		else if (window.ActiveXObject) {
			this.req = new ActiveXObject("Microsoft.XMLHTTP");
		}

		if (this.req) {
			try {
				var loader = this;
				this.req.onreadystatechange = function () {
					loader.onReadyState.call(loader);
				}
				this.req.open(method, this.url, true);
				if (contentType) {
					this.req.setRequestHeader("Content-Type", contentType);
				}
				this.req.send(encodeURI(params));
			}
			catch (err) {
				this.onError.call(this);
			}
		}
	},

	/**
	* Method deals with the return from the server and makes the appropriate call as specified
	*/
	onReadyState: function () {
		var req = this.req;
		var ready = req.readyState;

		if (ready == com.qas.READY_STATE_COMPLETE) {
			var httpStatus = req.status;
			if (httpStatus == 200 || httpStatus == 0) {
				this.onLoad.call(this);
			} else {
				if (number_of_connection_attempts < com.qas.MAXIMUM_CONNECTION_ATTEMPTS) {
					number_of_connection_attempts++;
					this.loadXMLDoc(this.last_method, this.last_params, this.last_content);
				} else {
					this.onError.call(this);
				}
			}
		}
	},

	/**
	* The default error message, where no other error method is specified
	*/
	defaultError: function () {
		alert("An error has occurred. Please check the settings in the "
			+ "file proweb.htm and ensure that the server is running correctly");
	}
};

/**************************************************************************************************
* Simple class for managing parameters. Users pass parameters to the class via the addParameter
* method, and then access the correctly formatted string from toString(). No arg constructor.
**************************************************************************************************/
com.qas.AjaxParameters = function () {
	this.parameter_string = "";
}

com.qas.AjaxParameters.prototype =
{
	/**
	* Adds a name/ value pair (both sent as strings)
	*/
	addParameter: function (name, value) {
		this.parameter_string = this.parameter_string +
			name + "=" + value + "&";
	},

	/**
	* Returns all of the parameters as a formatted string ready for HTTP
	*/
	toString: function () {
		return this.parameter_string.slice(0, this.parameter_string.length - 1);
	}
}

/**************************************************************************************************
* A simple datatype class for holding address information. This contains only getters (all setting
* is done via the constructor) but provides a standard format for encapsulation. The constrcutor has:
* @param Address Line One
* @param Address Line Two
* @param Address Line Three
* @param Address City
* @param Address State
* @param Address Zip
**************************************************************************************************/
com.qas.Address = function (lineone, linetwo, linethree, city, state, zip) {
	this.lineone = lineone;
	this.linetwo = linetwo;
	this.linethree = linethree;
	this.city = city;
	this.state = state;
	this.zip = zip;
}

com.qas.Address.prototype =
{
	getLineOne: function () {
		return this.lineone;
	},

	getLineTwo: function () {
		return this.linetwo;
	},

	getLineThree: function () {
		return this.linethree;
	},

	getCity: function () {
		return this.city;
	},

	getState: function () {
		return this.state;
	},

	getZip: function () {
		return this.zip;
	}
}

/**************************************************************************************************
* A simple datatype class for holding multiple information. Simplar to the address class above,
* all setting is done through the constructor - only getters are available methods. Constrcutor has:
* @param Address Text
* @param Postcode
* @param Moniker
* @param Whether this is a full address or not
* @param Score
**************************************************************************************************/
com.qas.PartialResult = function (addresstext, postcode, moniker, is_full_address, score, unresolvablerange) {
	this.addresstext = addresstext;
	this.postcode = postcode;
	this.moniker = moniker;
	this.is_full_address = is_full_address;
	this.score = score;
	this.unresolvablerange = unresolvablerange;
}

com.qas.PartialResult.prototype =
{
	getAddressText: function () {
		return this.addresstext;
	},

	getPostcode: function () {
		return this.postcode;
	},

	getMoniker: function () {
		return this.moniker;
	},

	getScore: function () {
		return this.score;
	},

	isFullAddress: function () {
		return this.is_full_address;
	},

	isUnresolvableRange: function () {
		return this.unresolvablerange;
	}
}

/**************************************************************************************************
* This is the main point of interaction with the server. All searches are made through this class.
* Users are able to perform a standard search, a refine or a format of the information. The class
* has no state. The constructor has:
* @param The function to call on a successful HTTP call
* @param The function to call in the event of an error
**************************************************************************************************/
com.qas.SearchService = function (onLoad, onError) {
	this.onLoad = onLoad;
	this.onError = onError;
}

com.qas.SearchService.prototype =
{
	search: function (lone, ltwo, lthree, city, state, zip) {
		// Create the parameters to send
		var params = new com.qas.AjaxParameters();
		params.addParameter("action", "search");
		params.addParameter("country", com.qas.COUNTRYSET);
		params.addParameter("promptset", com.qas.PROMPTSET);
		params.addParameter("addlayout", com.qas.ADDRESSLAYOUT);
		params.addParameter("searchstring", lone + "," + ltwo + "," +
			lthree + "," + city + "," + state + "," + zip);

		var loader = new com.qas.ContentLoader(this.onLoad, this.onError, "POST", params);
	},

	refine: function (moniker, refinetext) {
		// Create the parameters to send
		var params = new com.qas.AjaxParameters();
		params.addParameter("action", "refine");
		params.addParameter("moniker", moniker);
		params.addParameter("refinetext", refinetext);
		params.addParameter("addlayout", com.qas.ADDRESSLAYOUT);

		var loader = new com.qas.ContentLoader(this.onLoad, this.onError, "POST", params);
	},

	format: function (moniker) {
		// Create the parameters to send
		var params = new com.qas.AjaxParameters();
		params.addParameter("action", "format");
		params.addParameter("addlayout", com.qas.ADDRESSLAYOUT);
		params.addParameter("moniker", moniker);

		var loader = new com.qas.ContentLoader(this.onLoad, this.onError, "POST", params);
	}
};

/**************************************************************************************************
* Functions that implement business logic. All of them have the "business" prefix. However, they 
* are NOT objects as understood above, but rather just a collection of functions.
**************************************************************************************************/

/**************************************************************************************************
* Processes the search results - in almost all situations when a search is performed this is called
* on success as a central decision/ logic point. What to do is dependent of the verify level that
* is returned by the QAS engine.
**************************************************************************************************/
business.processSearchResults = function () {
    var previous_verify_level = verify_level;
    var previous_moniker = current_moniker;


    verify_level = this.req.responseXML.getElementsByTagName
		("verifylevel")[0].firstChild.nodeValue;

    //  Check for a partial result with resolvable ranges (no picklist necessary)
    var j = 0;
    var i = 0;

    if (verify_level == "PremisesPartial" || verify_level == "StreetPartial") {
        var tempresults = business.parsePicklistResults(this.req);
        for (i = 0; i < tempresults.length; i++) {
            //alert(tempresults[i].isUnresolvableRange() + "\n\ri =" + i + "\n\rj=" + j);
            if (tempresults[i].isUnresolvableRange() == "False") { j++; }
        }
        if (j == tempresults.length) { verify_level = "Multiple"; }
    }

    switch (verify_level) {
        case "PremisesPartial":
            var results = business.parsePicklistResults(this.req);
            // If the result is single, force an interaction
            if (results[0].isFullAddress().toLowerCase() == "true") {
                if (results.length == 1) {
                    var sservice = new com.qas.SearchService(
						business.processForcedInteraction,
						business.processSearchError);
                    sservice.refine(results[0].getMoniker(), "");
                    return;
                }
                // Special case - proweb occasionally returns a verified address with    ***This Code is addressed above with the single premise***
                // more than one result?? This handles that situation.   **this code would cause issues in specific refinement cases because the verify levels weren't changing
                //else if (previous_verify_level == verify_level)
                //{
                //	var sservice = new com.qas.SearchService(
                //		business.processSearchResults,
                //		business.processSearchError);
                //	sservice.refine(r esults[0].getMoniker(),"");
                //	return;
                //}
            }

            if (previous_verify_level == verify_level && results.length > 1) {
                interface.showPremPartialError();
                return;
            }
            interface.showPremPartialResults(business.parsePicklistResults(this.req));

            //  Check for a refine error and display message
            current_moniker = results[0].getMoniker();
            if (previous_moniker == current_moniker) {
                interface.showRefineError();
            }

            break;
        case "StreetPartial":
            var results = business.parsePicklistResults(this.req);
            // If the result is single, force an interaction
            if (results.length == 1 && results[0].isFullAddress().toLowerCase() == "true") {
                var sservice = new com.qas.SearchService(
					business.processForcedInteraction,
					business.processSearchError);
                sservice.refine(results[0].getMoniker(), "");
                return;
            }
            if (previous_verify_level == verify_level && results.length > 1) {
                interface.showStreetPartialError();
                return;
            }
            interface.showStreetPartialResults(business.parsePicklistResults(this.req));
            break;
        case "Verified":
            is_verified = true;
            interface.showVerifiedResults(business.parseSingleResult(this.req));
            events.handleThirdPartyButtonClick();
            break;
        case "InteractionRequired":
            interface.showInteractionResults(business.parseSingleResult(this.req));
            break;
        case "Multiple":
            interface.showMultipleResults(business.parsePicklistResults(this.req));
            break;
        case "None":
            if (com.qas.IGNORE_UNMATCHED_ADDRESSES.toLowerCase() == "true") {
                interface.showVerifiedResults(originaladdress);
                events.submitForm();
            }
            else {
                interface.showNoResults();
            }
            break;
        case "Error":
            business.processSearchError(this.req);
        default:
            break;
    }
}

/**************************************************************************************************
* This takes the XML result from the HTTP call and wraps it in an array of  partial classes .
**************************************************************************************************/
business.parsePicklistResults = function (req) {
	var results = new Array();
	var picklistitems = req.responseXML.getElementsByTagName("picklistitem");
	for (var i = 0; i < picklistitems.length; i++) {
		prempartial = new com.qas.PartialResult(
			picklistitems[i].getElementsByTagName("addresstext")[0].firstChild.nodeValue,
			picklistitems[i].getElementsByTagName("postcode")[0].firstChild.nodeValue,
			picklistitems[i].getElementsByTagName("moniker")[0].firstChild.nodeValue,
			picklistitems[i].getElementsByTagName("fulladdress")[0].firstChild.nodeValue,
			picklistitems[i].getElementsByTagName("score")[0].firstChild.nodeValue,
			picklistitems[i].getElementsByTagName("unresolvablerange")[0].firstChild.nodeValue);
		results[i] = prempartial;
	}
	return results;
}

/**************************************************************************************************
* This takes the XML result from the HTTP and wraps it in an address class.
**************************************************************************************************/
business.parseSingleResult = function (req) {
	return new com.qas.Address(
		req.responseXML.getElementsByTagName("lineone")[0].firstChild.nodeValue,
		(req.responseXML.getElementsByTagName("linetwo")[0].firstChild != null) ?
			req.responseXML.getElementsByTagName("linetwo")[0].firstChild.nodeValue : "",
		(req.responseXML.getElementsByTagName("linethree")[0].firstChild != null) ?
			req.responseXML.getElementsByTagName("linethree")[0].firstChild.nodeValue : "",
		req.responseXML.getElementsByTagName("city")[0].firstChild.nodeValue,
		req.responseXML.getElementsByTagName("state")[0].firstChild.nodeValue,
		req.responseXML.getElementsByTagName("zip")[0].firstChild.nodeValue);
}

/**************************************************************************************************
* If the result is a single record premesis partial or a street partial, then display the record
* as an interaction required.
**************************************************************************************************/
business.processForcedInteraction = function () {
	interface.showInteractionResults(business.parseSingleResult(this.req));
}

/**************************************************************************************************
* Standard method for handling a search error. Note that if the user has chosen to ignore this
* error (by setting IGNORE_CONNECTION_ERROR, then the method simply submits the form.
**************************************************************************************************/
business.processSearchError = function () {
    if (com.qas.IGNORE_CONNECTION_ERROR.toLowerCase() == "true") {
        document.getElementById(com.qas.FORM_BUTTON_ID).onclick =
			function () { return true; };
        document.getElementById(com.qas.FORM_BUTTON_ID).click();
        document.forms[0].submit();
    }
    else {
        alert("Unable to verify the address!!!");
//        alert("An error has occurred. Please check the settings in the "
//                			+ "file proweb.htm and ensure that the server is running correctly (you "
//                			+ "can run test_server_setup to check.)");

        //NP:2012.04.20 - removeChild breaks code in IE replacing with the selectDisplay method 
        //document.body.removeChild(document.getElementById("wait_wgt")); //***will break code in IE6***//
        //document.getElementById("cover_wgt").style.display = "none";
        if (com.qas.QASErrorHandler) com.qas.QASErrorHandler();
        interface.selectDisplay("input_wgt");
        $('#divNotes').dialog('widget').show();
    }
}



/**************************************************************************************************
* If the user has entered an incorrect refine range, this method is called.
**************************************************************************************************/
business.processRefineError = function () {
	interface.showRefineError();
}

/**************************************************************************************************
* This method decides which button to display (if the user has specified a form button on the
* html).
**************************************************************************************************/
business.selectInputButton = function () {
	if (com.qas.FORM_BUTTON_ID.length > 0) {
		var button = document.getElementById(com.qas.FORM_BUTTON_ID)
		if (button == null) {
			alert("Configuration: the specified button '"
				+ com.qas.FORM_BUTTON_ID
				+ "' does not exist.");
			return;
		}
		document.getElementById("input_search").style.display = "none";
		if (button.getAttribute("onclick") != null) {
			existing_button_onclick = button.getAttribute("onclick");
		}
		button.onclick = events.handleThirdPartyButtonClick;
	}
}

/**************************************************************************************************
* All event handling methods. Events is not an class, simply a collection of functions.
**************************************************************************************************/

/**************************************************************************************************
* When the original form button is clicked to start the verification/ form submission. This starts
* a basic search with the values in the text input fields.
**************************************************************************************************/
events.handleClickInputButton = function (evt) {
	// Display the cover widget to disable the page
	if (com.qas.RUN_AS_POPUP == "true") {
		interface.showCoverWidget();
		interface.displayWaitingWidget();
		//NP:2012.04.26 - dont want window to scroll to top - window.scrollTo(0, 0);
	}

	var lone = document.getElementById("lineone").value;
	var ltwo = document.getElementById("linetwo").value;
	var lthree = document.getElementById("linethree").value;
	var city = document.getElementById("city").value;
	var state = document.getElementById("state").value;
	var zip = document.getElementById("zip").value;

	originaladdress = new com.qas.Address(lone, ltwo, lthree, city, state, zip);

	var sservice = new com.qas.SearchService(
		business.processSearchResults,
		business.processSearchError);

	sservice.search(lone, ltwo, lthree, city, state, zip);
}
//NP:2012.05.14 - Redefining the handleClickInputButton to accept an address
com.qas.QASCompletedHandler = null;
com.qas.QASErrorHandler = null;
events.performQASSearch = function (a, successHandler, errorHandler) {
     com.qas.QASCompletedHandler = successHandler;
     com.qas.QASErrorHandler = errorHandler;
     // Display the cover widget to disable the page
     if (com.qas.RUN_AS_POPUP == "true") {
         interface.showCoverWidget();
         interface.displayWaitingWidget();
         //NP:2012.04.26 - dont want window to scroll to top - window.scrollTo(0, 0);
     }

     //    var lone = document.getElementById("lineone").value;
     //    var ltwo = document.getElementById("linetwo").value;
     //    var lthree = document.getElementById("linethree").value;
     //    var city = document.getElementById("city").value;
     //    var state = document.getElementById("state").value;
     //    var zip = document.getElementById("zip").value;

     //    originaladdress = new com.qas.Address(lone, ltwo, lthree, city, state, zip);
     originaladdress = new com.qas.Address(a.getLineOne(), a.getLineTwo(), a.getLineThree(), a.getCity(), a.getState(), a.getZip());

     var sservice = new com.qas.SearchService(
		business.processSearchResults,
		business.processSearchError);

     //    sservice.search(lone, ltwo, lthree, city, state, zip);
     sservice.search(a.getLineOne(), a.getLineTwo(), a.getLineThree(), a.getCity(), a.getState(), a.getZip());
     // checkIEVersion('none');
 }

/**************************************************************************************************
* When the refine button is clicked from the refine widget
**************************************************************************************************/
events.handleClickRefineButton = function (evt) {
	if (document.getElementById("refine_input").value.length <= 0) {
		alert("Please enter refinement text");
		return;
	}

	var sservice = new com.qas.SearchService(
		business.processSearchResults,
		business.processRefineError);
	sservice.refine(
		document.getElementById("refine_moniker").value,
		document.getElementById("refine_input").value);
}

/**************************************************************************************************
* Event called when a multiple result link is clicked (forces a refine of the result)
**************************************************************************************************/
events.handleClickMultipleButton = function (evt) {
	var sservice = new com.qas.SearchService(
		business.processSearchResults,
		business.processSearchError);
	sservice.refine(this.getAttribute("moniker"), "");
}

/**************************************************************************************************
* Event called when the premises partial button is clicked (starts a search with an "apt" appendage)
**************************************************************************************************/
events.handleClickPremPartialButton = function (evt) {
	var lone = document.getElementById("lineone").value + " apt " +
		document.getElementById("partial_input").value;
	var ltwo = document.getElementById("linetwo").value;
	var lthree = document.getElementById("linethree").value;
	var city = document.getElementById("city").value;
	var state = document.getElementById("state").value;
	var zip = document.getElementById("zip").value;
	var sservice = new com.qas.SearchService(
		business.processSearchResults,
		business.processSearchError);
	sservice.search(lone, ltwo, lthree, city, state, zip);
}

/**************************************************************************************************
* Event called when the street partial button is clicked (deals with urbanizations amongsth others)
**************************************************************************************************/
events.handleClickStreetPartialButton = function (evt) {
	var lone = document.getElementById("lineone").value;
	var ltwo = document.getElementById("linetwo").value;

	var urbregex = new RegExp("^(urb|urbanisation|urbanización)", "i");
	if (urbregex.exec(document.getElementById("lineone").value) != null) {
		ltwo = document.getElementById("partial_input").value + " " + ltwo;
		document.getElementById("linetwo").value = ltwo;
	}
	else {
		lone = document.getElementById("partial_input").value + " "
			+ lone.replace(/^[0-9]+/, "");
		document.getElementById("lineone").value = lone;
	}

	var lthree = document.getElementById("linethree").value;
	var city = document.getElementById("city").value;
	var state = document.getElementById("state").value;
	var zip = document.getElementById("zip").value;
	var sservice = new com.qas.SearchService(
		business.processSearchResults,
		business.processSearchError);
	sservice.search(lone, ltwo, lthree, city, state, zip);
}

/**************************************************************************************************
* Event called when links from either prem or street partial results are clicked (calls refine)
**************************************************************************************************/
events.handleClickPartialText = function (evt) {
	var sservice = new com.qas.SearchService(
		business.processSearchResults,
		business.processSearchError);

	sservice.refine(this.getAttribute("moniker"), "");
}

/**************************************************************************************************
* Event called when button clicked from the interaction widget
**************************************************************************************************/
events.handleClickInteractionButton = function (evt) {
	is_verified = true;
	interface.showVerifiedResults(new com.qas.Address(
		document.getElementById("interaction_lineone").innerHTML,
		document.getElementById("interaction_linetwo").innerHTML,
		document.getElementById("interaction_linethree").innerHTML,
		document.getElementById("interaction_linecity").innerHTML,
		document.getElementById("interaction_linestate").innerHTML,
		document.getElementById("interaction_linezip").innerHTML));
	events.handleThirdPartyButtonClick();
}

/**************************************************************************************************
* Event called when the users click to view partial results (displays them)
**************************************************************************************************/
events.handleClickShowPartialResults = function (evt) {
	document.getElementById("partial_showlink").innerHTML = "";
	document.getElementById("partial_allresults").style.display = "";
}

/**************************************************************************************************
* Event when the users click "choose this address" from the right hand details widget.
**************************************************************************************************/
events.handleChooseAddressText = function (evt) {
	verify_level = "none";
	interface.clearNonVerifiedWidgets();
	interface.showVerifiedResults(originaladdress);
	com.qas.BYPASS_QAS_VERIFICATION = true;
	events.submitForm();
}

/**************************************************************************************************
* Event when the user clicks "edit this address" from the right habd details widget.
**************************************************************************************************/
events.handleEditAddressText = function (evt) {
	verify_level = "";
	current_moniker = "";
	interface.clearNonVerifiedWidgets();
	interface.showAddressInput(originaladdress);
	document.getElementById("wait_wgt").style.display = "none";
    //BS:2012.05.14: to div notes after edit or choose option select from qas service
	$('#divNotes').dialog('widget').show();
}

/**************************************************************************************************
* This function actually is a response to any event wanting to submit the form in question. It is 
* required as well as the function below because users may wish to ignore unmatched addresses.
**************************************************************************************************/
events.submitForm = function (evt) {
	// If we are not in a form, then do not do anything (there is no third party button)
	if (com.qas.FORM_BUTTON_ID.length <= 0 || document.getElementById(com.qas.FORM_BUTTON_ID) == null) {
		return;
	}

	document.getElementById(com.qas.FORM_BUTTON_ID).onclick =
		function () { return true; };
	document.getElementById(com.qas.FORM_BUTTON_ID).click();
}

/**************************************************************************************************
* When the user click the form button, this method decides what to do. It first runs any javascript
* already associated with the button click event. Once that has run successfully (returned true)
* then it runs the address verification. If it happy that the address is verified (or that the user
* does not want to verify), then it submit the form.
**************************************************************************************************/
events.handleThirdPartyButtonClick = function (evt) {
	// If we are not in a form, then do not do anything (there is no third party button)
	if (com.qas.FORM_BUTTON_ID.length <= 0 || document.getElementById(com.qas.FORM_BUTTON_ID) == null) {
		return;
	}

	// If an error should be ignored, submit the form anyway
	if ((com.qas.IGNORE_CONNECTION_ERROR.toLowerCase() == "true")
	&& (verify_level.toLowerCase() == "none")) {
		document.getElementById(com.qas.FORM_BUTTON_ID).onclick = existing_button_onclick;
		document.getElementById(com.qas.FORM_BUTTON_ID).click();
	}

	// If we are on the first page, run the inital javascript for the page. If that 
	// completes, run the QAS code
	if ((current_widget == "input_wgt") && (is_verified != true)) {
		// Run the existing code for the onclick event, but catch the return
		var return_button;
		try {
			return_value = existing_button_onclick();
		}
		catch (err) {
			return_value = eval(existing_button_onclick.replace(/(^|;| )return /i, ""));
		}

		if (return_value == true || return_value == null) {
			// If the user has chosen to bypass qas, reset the button and click
			if (document.getElementById("BYPASS_QAS_VERIFICATION").value.toLowerCase() == "true") {
				return true;
			}

			events.handleClickInputButton();
		}
		return false;
	}
	else if ((current_widget == "input_wgt") && (is_verified == true)) {
		document.getElementById(com.qas.FORM_BUTTON_ID).click();
		return true;
	}
	else {
		alert("You must select a valid address before you continue");
		return false;
	}
}

/**************************************************************************************************
* This event acts as a general onKeyPress event for those input boxes that submit a widget when
* the return key is pressed.
**************************************************************************************************/
events.handleSubmitOnEnterKey = function (evt) {
	var button;
	if (current_widget == "refine_wgt") {
		button = document.getElementById("refine_btnsubmit");
	}
	else {
		button = document.getElementById("partial_btnsubmit");
	}

	// Deal with browser incompatibility
	if (window.event == null) {
		if (evt && evt.keyCode == 13) {
			button.click();
		}
	}
	else {
		if (window.event && window.event.keyCode == 13) {
			button.click();
		}
	}
}

/**************************************************************************************************
* As well as the above, this form ensures that, when return is pressed by the user, the document/ 
* form is not also submitted (i.e problem only)
**************************************************************************************************/
events.preventSubmissionFormEnter = function (evt) {
	// Deal with browser incompatibility
	if (window.event == null) {
		return !(evt && evt.keyCode == 13);
	}
	else {
		return !(window.event && window.event.keyCode == 13);
	}
}

/**************************************************************************************************
* Interface functions. interface is not a class, but simply a collection of presentation functions
**************************************************************************************************/

/**************************************************************************************************
* Displays the address in the original prompt input fields
**************************************************************************************************/
interface.showAddressInput = function (address) {
    com.qas.QASCompletedHandler(address);
    interface.selectDisplay("input_wgt");
    return;
    //for set AddressUserControl 
    document.getElementById(arr_controls[0]).value = address.getLineOne();
    document.getElementById(arr_controls[1]).value = address.getLineTwo();
    document.getElementById(arr_controls[2]).value = address.getLineThree();
    document.getElementById(arr_controls[3]).value = address.getCity();
    document.getElementById(arr_controls[5]).value = address.getState();
    document.getElementById(arr_controls[7]).value = address.getState();
    document.getElementById(arr_controls[8]).value = address.getZip().replace("-", "");
//    document.getElementById("lineone").value = address.getLineOne();
//    document.getElementById("linetwo").value = address.getLineTwo();
//    document.getElementById("linethree").value = address.getLineThree();
//    document.getElementById("city").value = address.getCity();
//    document.getElementById("state").value = address.getState();
//    document.getElementById("zip").value = address.getLineOne();
    interface.selectDisplay("input_wgt");
}

/**************************************************************************************************
* Display a fully verified result in the original input fields (or an unverified address if the
* setting is that the address need not be verified).
**************************************************************************************************/
interface.showVerifiedResults = function (address) {
    // Remove all other HTML for the form submission
    interface.clearNonVerifiedWidgets();

    if (com.qas.FORM_BUTTON_ID.length <= 0) {
        document.getElementById("input_search").style.display = "none";
    }
    //AA:24.12.2013 Changed for the get the exact variable.
    document.getElementById(arr_controls["QASMatched"]).value = is_verified;
    interface.showAddressInput(address);
   
    interface.selectDisplay("input_wgt");
    $('#divNotes').dialog('widget').show();

}

/**************************************************************************************************
* Displays all multiple results (as a list of links)
**************************************************************************************************/
interface.showMultipleResults = function (results) {
	document.getElementById("multiple_wgt").innerHTML = "";

	var message = document.createElement("dt");
	message.innerHTML = com.qas.MSG_MULITPLE;
	message.className = "qas_message";
	document.getElementById("multiple_wgt").appendChild(message);

	interface.showRightDetails("multiple_wgt");

	var title = document.createElement("dt");
	title.innerHTML = "All potential missing information";
	title.className = "qas_title";
	document.getElementById("multiple_wgt").appendChild(title);

	for (var i = 0; i < results.length; i++) {
		result = document.createElement("dt");
		result.setAttribute("moniker", results[i].getMoniker());
		result.innerHTML = results[i].getAddressText() +
			"<label class='qas_postcode'>" + results[i].getPostcode() + "</label>";
		result.className = "qas_link";
		result.onclick = events.handleClickMultipleButton;

		document.getElementById("multiple_wgt").appendChild(result);
	}
	interface.selectDisplay("multiple_wgt");
}

/**************************************************************************************************
* Show the premises partial results
**************************************************************************************************/
interface.showPremPartialResults = function (results) {
	interface.showPartialResults(
		results, events.handleClickPremPartialButton,
		com.qas.MSG_PREMPARTIAL, "Enter apartment/suite/unit number:");
}

/**************************************************************************************************
* Show the street partial results
**************************************************************************************************/
interface.showStreetPartialResults = function (results) {
	interface.showPartialResults(
		results, events.handleClickStreetPartialButton,
		com.qas.MSG_STREETPARTIAL, "Enter street number:");
}

/**************************************************************************************************
* because the partial (prem and street) share so much functionality, this function is used by both
* (the results, button event, main message and label message are sent as args)
**************************************************************************************************/
interface.showPartialResults = function (results, buttonevent, msg, label_msg) {
    if (results.length == 1) {
        interface.showRefinePrompt(results[0]);
    }
    else {
        document.getElementById("partial_wgt").innerHTML = "";

        var message = document.createElement("dt");
        message.innerHTML = msg;
        message.className = "qas_message";
        document.getElementById("partial_wgt").appendChild(message);

        interface.showRightDetails("partial_wgt");

        var label = document.createElement("label");
        label.innerHTML = label_msg + "&nbsp;&nbsp;";
        document.getElementById("partial_wgt").appendChild(label);

        var input = document.createElement("input");
        input.setAttribute("type", "input");
        input.setAttribute("id", "partial_input");
        input.setAttribute("value", "");
        input.className = "qas_refinebutton";
        input.onkeypress = events.handleSubmitOnEnterKey;
        document.getElementById("partial_wgt").appendChild(input);

        var button = document.createElement("input");
        button.setAttribute("type", "button");
        button.setAttribute("id", "partial_btnsubmit");
        button.setAttribute("value", "Search");
        button.className = "qas_refinebutton";
        button.onclick = buttonevent;
        document.getElementById("partial_wgt").appendChild(button);

        var showlink = document.createElement("dt");
        showlink.className = "qas_link";
        showlink.setAttribute("id", "partial_showlink");
        showlink.innerHTML = "Click here to display all potential matches";
        showlink.onclick = events.handleClickShowPartialResults;
        document.getElementById("partial_wgt").appendChild(showlink);

        var resultholder = document.createElement("dd");
        resultholder.setAttribute("id", "partial_allresults");
        resultholder.style.display = "none";

        for (var i = 0; i < results.length; i++) {
            var result = document.createElement("dt");
            result.setAttribute("moniker", results[i].getMoniker());
            result.className = "qas_link";
            result.innerHTML = results[i].getAddressText() +
				"<label class='qas_postcode'>" + results[i].getPostcode() + "</label>";
            result.onclick = events.handleClickPartialText;
            resultholder.appendChild(result);
        }

        document.getElementById("partial_wgt").appendChild(resultholder);
        interface.selectDisplay("partial_wgt");
        document.getElementById("partial_input").focus();
    }
}

/**************************************************************************************************
* This shows an address that requires confirmation/ interaction
**************************************************************************************************/
interface.showInteractionResults = function (result) {
	document.getElementById("interaction_wgt").innerHTML = "";

	var message = document.createElement("dt");
	message.innerHTML = com.qas.MSG_INTERACTION;
	message.className = "qas_message";
	document.getElementById("interaction_wgt").appendChild(message);

	interface.showRightDetails("interaction_wgt");

	var lone = document.createElement("dt");
	lone.setAttribute("id", "interaction_lineone");
	lone.innerHTML = result.getLineOne();
	document.getElementById("interaction_wgt").appendChild(lone);

	var ltwo = document.createElement("dt");
	ltwo.setAttribute("id", "interaction_linetwo");
	ltwo.innerHTML = result.getLineTwo();
	document.getElementById("interaction_wgt").appendChild(ltwo);

	var lthree = document.createElement("dt");
	lthree.setAttribute("id", "interaction_linethree");
	lthree.innerHTML = result.getLineThree();
	document.getElementById("interaction_wgt").appendChild(lthree);

	var lcity = document.createElement("dt");
	lcity.setAttribute("id", "interaction_linecity");
	lcity.innerHTML = result.getCity();
	document.getElementById("interaction_wgt").appendChild(lcity);

	var lstate = document.createElement("dt");
	lstate.setAttribute("id", "interaction_linestate");
	lstate.innerHTML = result.getState();
	document.getElementById("interaction_wgt").appendChild(lstate);

	var lzip = document.createElement("dt");
	lzip.setAttribute("id", "interaction_linezip");
	lzip.innerHTML = result.getZip();
	document.getElementById("interaction_wgt").appendChild(lzip);

	var button = document.createElement("input");
	button.setAttribute("type", "button");
	button.setAttribute("id", "partial_btnsubmit");
	button.setAttribute("value", "Select Address");
	button.className = "qas_refinebutton";
	button.onclick = events.handleClickInteractionButton;
	document.getElementById("interaction_wgt").appendChild(button);

	interface.selectDisplay("interaction_wgt");
}

/**************************************************************************************************
* Displays the refine prompt widget
**************************************************************************************************/
interface.showRefinePrompt = function (result) {
	document.getElementById("refine_wgt").innerHTML = "";

	var message = document.createElement("dt");
	message.innerHTML = com.qas.MSG_REFINE;
	message.className = "qas_message";
	document.getElementById("refine_wgt").appendChild(message);

	interface.showRightDetails("refine_wgt");

	var input = document.createElement("input");
	input.setAttribute("type", "input");
	input.setAttribute("id", "refine_input");
	input.setAttribute("value", "");
	input.className = "qas_refinebutton";
	input.onkeypress = events.handleSubmitOnEnterKey;
	document.getElementById("refine_wgt").appendChild(input);

	var hinput = document.createElement("input");
	hinput.setAttribute("type", "hidden");
	hinput.setAttribute("id", "refine_moniker");
	hinput.setAttribute("value", result.getMoniker());
	document.getElementById("refine_wgt").appendChild(hinput);

	var button = document.createElement("input");
	button.setAttribute("type", "button");
	button.setAttribute("id", "refine_btnsubmit");
	button.setAttribute("value", "Search");
	button.className = "qas_refinebutton";
	button.onclick = events.handleClickRefineButton
	document.getElementById("refine_wgt").appendChild(button);

	var addtext = document.createElement("dt");
	addtext.innerHTML = result.getAddressText();
	document.getElementById("refine_wgt").appendChild(addtext);

	interface.selectDisplay("refine_wgt");
	document.getElementById("refine_input").focus();
}

/**************************************************************************************************
* Thsi perfoms the logic as to which widget shoudl be displayed (this depends also on whether the
* user has chosen to run proweb as a pop-up or not).
**************************************************************************************************/
interface.selectDisplay = function (div_name) {
	// If the user has chosen not to display pop-ups, simply display normally
	if (com.qas.RUN_AS_POPUP.toLowerCase() != "true") {
		interface.showSelectedDisplay(div_name);
		current_widget = div_name;
		return;
	}
	if (div_name == "input_wgt") {
		document.getElementById("cover_wgt").style.display = "none";
		interface.manageSelectFields("");
		document.body.style.overflow = "auto";
		interface.showSelectedDisplay(div_name);
		document.getElementById("wait_wgt").style.display = "none";
	}
	else {
		//document.body.removeChild(document.getElementById("wait_wgt"));   //***will break code in IE6***//
		interface.showPopupDisplay(div_name);
		document.getElementById(div_name).style.display = "block";
		interface.showCoverWidget();
	}
	current_widget = div_name;
}

/**************************************************************************************************
* Due to a bug in IE6, select boxes are not placed on the correct layer (they show through the
* cover div). This removes and then replaces them where appropriate.
**************************************************************************************************/
interface.manageSelectFields = function (display_type) {
    //BS:2012:05:09:-yrs1470:visible false proweb div  to handle IE6 overlapping issue
    return;
    var selects = document.body.getElementsByTagName("select");
    for (var i = 0; i < selects.length; i++) {
        selects[i].style.display = display_type;
    }

}

/**************************************************************************************************
* Displays the background that disables the page
**************************************************************************************************/
interface.showCoverWidget = function () {
    document.getElementById("cover_wgt").style.display = "block";
    if (document.body.style.overflow = "hidden") {
        document.getElementById("cover_wgt").style.width = "100%";
        document.getElementById("cover_wgt").style.height = "100%";

    }
}

/**************************************************************************************************
* Displays the waiting widget (in case the lookup takes a while)
**************************************************************************************************/
interface.displayWaitingWidget = function () {
	var wait = document.createElement("div");
	wait.setAttribute("id", "wait_wgt");
	wait.className = "wait_message";
	wait.innerHTML = com.qas.WAIT_MESSAGE;
	//document.body.appendChild(wait);       //Appended to cover wgt so Edit This Address link will remove it (Stays on screen in IE despite display = "none"
	document.getElementById("cover_wgt").appendChild(wait);   // will also cause IE to crash if you try to remove it when its appended to the body.. not sure why
}

/**************************************************************************************************
* Displays the pop-up div (if specified in the HTML)
**************************************************************************************************/
interface.showPopupDisplay = function (div_name) {
    document.getElementById("multiple_wgt").style.display = "none";
    document.getElementById("partial_wgt").style.display = "none";
    document.getElementById("refine_wgt").style.display = "none";
    document.getElementById("interaction_wgt").style.display = "none";

    document.getElementById(div_name).style.display = "";
    
}

/**************************************************************************************************
* Function displays the specified widget abnd hides all others
**************************************************************************************************/
interface.showSelectedDisplay = function (div_name) {
	document.getElementById("input_wgt").style.display = "none";
	document.getElementById("multiple_wgt").style.display = "none";
	document.getElementById("partial_wgt").style.display = "none";
	document.getElementById("refine_wgt").style.display = "none";
	document.getElementById("interaction_wgt").style.display = "none";
	document.getElementById("none_wgt").style.display = "none";

	document.getElementById(div_name).style.display = "";
}

/**************************************************************************************************
* This clears all of the widgets of their HTML (does not just hide them).
**************************************************************************************************/
interface.clearNonVerifiedWidgets = function () {
	document.getElementById("multiple_wgt").innerHTML = "";
	document.getElementById("partial_wgt").innerHTML = "";
	document.getElementById("refine_wgt").innerHTML = "";
	document.getElementById("interaction_wgt").innerHTML = "";
	document.getElementById("none_wgt").innerHTML = "";
}

/**************************************************************************************************
* The error as shown when the refine range is incorrect
**************************************************************************************************/
interface.showRefineError = function () {
	alert(com.qas.MSG_REFINE_ERROR);
	document.getElementById("refine_input").value = "";
}

/**************************************************************************************************
* The error shown when the street partial range is incorrect
**************************************************************************************************/
interface.showStreetPartialError = function () {
	alert(com.qas.MSG_STREETPARTIAL_ERROR);
	document.getElementById("partial_input").value = "";
	events.handleClickShowPartialResults();
}

/**************************************************************************************************
* The error shown when the premises partial range is incorrect
**************************************************************************************************/
interface.showPremPartialError = function () {
	alert(com.qas.MSG_PREMPARTIAL_ERROR);
	document.getElementById("partial_input").value = "";
	events.handleClickShowPartialResults();
}

/**************************************************************************************************
* Displays the right hand details box (choose this address/ edit this address)
**************************************************************************************************/
interface.showRightDetails = function (widget) {
	// Remove the right hand details if they exist
	if (document.getElementById("right_details") != null &&
	document.getElementById("right_details").parentElement != null) {
		document.getElementById("right_details").parentElement.
			removeChild(document.getElementById("right_details"));
	}

	var addrss = document.createElement("dt");
	addrss.setAttribute("id", "originaladdress");
	addrss.innerHTML = "You entered:<br><br>"
		+ "<label class='qas_rightdetails'>Line 1:</label>" + originaladdress.getLineOne() + "<br>"
		+ "<label class='qas_rightdetails'>Line 2:</label>" + originaladdress.getLineTwo() + "<br>"
		+ "<label class='qas_rightdetails'>Line 3:</label>" + originaladdress.getLineThree() + "<br>"
		+ "<label class='qas_rightdetails'>City:</label>" + originaladdress.getCity() + "<br>"
		+ "<label class='qas_rightdetails'>State:</label>" + originaladdress.getState() + "<br>"
		+ "<label class='qas_rightdetails'>Zip:</label>" + originaladdress.getZip() + "<br><br>";

	var chooselink = document.createElement("dt");
	chooselink.className = "qas_link";
	chooselink.innerHTML = "Choose this address";
	chooselink.onclick = events.handleChooseAddressText;

	var ortext = document.createElement("dt");
	ortext.setAttribute("id", "ortext");
	ortext.innerHTML = "<br>-- Or --<br><br>";

	var editlink = document.createElement("dt");
	editlink.className = "qas_link";
	editlink.innerHTML = "Edit this address";
	editlink.onclick = events.handleEditAddressText;

	var box = document.createElement("div");
	box.setAttribute("id", "right_details");
	box.className = "proweb_details";
	box.appendChild(addrss);
	box.appendChild(chooselink);
	box.appendChild(ortext);
	box.appendChild(editlink);

	document.getElementById(widget).appendChild(box);
}

/**************************************************************************************************
* This creates a div element and adds it to the display
**************************************************************************************************/
interface.addToProwebWidget = function (widget_name) {
    var widget = document.createElement("div");
    widget.setAttribute("id", widget_name);
    widget.style.zIndex = 1500;
    widget.className = (com.qas.RUN_AS_POPUP.toLowerCase() == "true") ? "proweb_popup" : "proweb_nonpopup";
    document.getElementById("proweb_wgt").appendChild(widget);
}

/**************************************************************************************************
* The display when no match can be found (if the user has chosen to not ignore failed matches)
**************************************************************************************************/
interface.showNoResults = function () {
	document.getElementById("refine_wgt").innerHTML = "";

	var msg = document.createElement("dt");
	msg.setAttribute("id", "none_message");
	msg.innerHTML = com.qas.MSG_NONE;
	msg.className = "qas_message";
	document.getElementById("none_wgt").appendChild(msg);

	interface.showRightDetails("none_wgt");
	document.getElementById("right_details").className = "proweb_details_left";
	interface.selectDisplay("none_wgt");
}

/**************************************************************************************************
* Commands to run on page creation
**************************************************************************************************/
// Add the widgets for the rest of the application
$(document).ready(function () {
    interface.addToProwebWidget("multiple_wgt");
    interface.addToProwebWidget("partial_wgt");
    interface.addToProwebWidget("refine_wgt");
    interface.addToProwebWidget("interaction_wgt");
    interface.addToProwebWidget("none_wgt");
    interface.addToProwebWidget("wait_wgt");

    // Add the cover widget so that the background can be disabled
    var widget = document.createElement("div");
    widget.setAttribute("id", "cover_wgt");
    widget.className = "proweb_cover";
    document.body.appendChild(widget);
    // Add all of the event handling dynamically, so as to remove from the interface
    document.getElementById("input_search").onclick = events.handleClickInputButton;

    // Set up the inteface, so that only the prompt screen is showing by default
    interface.selectDisplay("input_wgt");

    // Check to see which button should handle the prompt event
    business.selectInputButton();

    // Prevent the submission of the form via the enter key
    document.onkeypress = events.preventSubmissionFormEnter;

});
