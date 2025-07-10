function CheckDateValue(opt, msg) {
    var mainDate, mm, dd, yyyy;
    mainDate = opt.value;
    if (opt.value != '') {
        if (!ValidateDate(opt.value)) {
            if (msg != "") {
                alert(msg);
            }
            opt.focus();
            return false;
        }
        else {
            return true;
        }

    }
    else {
        return true;
    }
}

/*
Checks if date is in mm/dd/yyyy format
returns false if date is invalid.
*/
function ValidateDate(dateValue) {
    // PPP | 10/15/2015 | YRS-AT-2596 | Changed the function body with new code, which is checking the format using regexp first then checking for valid input values for month, date and year.
    var currVal = dateValue;
    if (currVal == '')
        return false;

    //Declare Regex 
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;

    //Checks for mm/dd/yyyy format.
    /*
    dtMonth = parseInt(dtArray[1]);
    if (dtArray[1].indexOf('0') == 0) // PPP | 10/16/2015 | YRS-AT-2596 | parseInt function is converting 08 and 09 into 0
        dtMonth = parseInt(dtArray[1].replace('0', '')); // PPP | 10/16/2015 | YRS-AT-2596 | parseInt function is converting 08 and 09 into 0

    dtDay = parseInt(dtArray[3]);
    dtYear = parseInt(dtArray[5]);
    */
    //START: PPP | 10/15/2015 | YRS-AT-2596 | Removed the parseInt functionality and confirmed the below code is working
    dtMonth = dtArray[1];
    dtDay = dtArray[3];
    dtYear = dtArray[5];
    //END: PPP | 10/15/2015 | YRS-AT-2596 | Removed the parseInt functionality and confirmed the below code is working

    if (dtYear < 1901)
        return false;

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        //START: PPP | 10/16/2015 | YRS-AT-2596 | it was limiting to find leap years till year 2100
        //var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        var isleap = (dtYear % 4 == 0);
        //END: PPP | 10/16/2015 | YRS-AT-2596 | it was limiting to find leap years till year 2100
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }

    return true;
}

// PPP | 10/15/2015 | YRS-AT-2596 | Removed unused function

function IsValidControlDate(sender, args) {
    // PPP | 10/15/2015 | YRS-AT-2596 | Removed unused code lines
    if (ValidateDate(args.Value)) {
        args.IsValid = true;
    }
    else {
        args.IsValid = false;
    }
}
