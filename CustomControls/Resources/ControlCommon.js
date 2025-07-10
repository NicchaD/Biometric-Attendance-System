var chkenter = false;

function checkemailonEnterkey(EmailId, e, msg) {
    var gkey = getkey(e);

    if (gkey == 8 || e.keyCode == 46 || (e.keyCode >= 37 && e.keyCode <= 40)) {
        return true;
    }
    else if (getkey(e) == 13) {
        chkenter = true;
        if (isvaliemailid(EmailId, msg) == false) {
            return setkey(e);
        }
    }


}

function CheckEmailId(EmailId, msg) {
    if (chkenter == false) {
        return isvaliemailid(EmailId, msg);
    }
    else {
        chkenter = false;
    }
}

function isvaliemailid(EmailId, msg) {
    //    var EmailIdRE = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})$/
    var EmailIdRE = /^[a-zA-Z][a-zA-Z0-9_\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$/

    if (EmailId.value != '') {

        if (EmailId.value.match(EmailIdRE)) {
            return true;
        }
        else {
            if (msg != "") {
                alert(msg);
                setTimeout(function () { EmailId.focus(); }, 1);

            }
            EmailId.select();
            return false;
        }
    }
}


function CheckNumber(obj, e, msg, allowNeg, allowdec, iscommaSepterated, defa, isPhoneNumber) {
    var gkey = getkey(e);

    if (isPhoneNumber) {
        if (gkey == 40 || gkey == 41 || gkey == 32 || gkey == 45) {
            return true;
        }
    }

    if (gkey < 45 || gkey > 57 || gkey == 47) {

        //8- backspace, 46- del    
        //        if(gkey==8 || e.keyCode==46 || (e.keyCode >37 &&  e.keyCode<=40))
        //         {
        if (gkey == 8 || e.keyCode == 46) {

            return true;
        }
        else if (gkey == 13 || gkey == 0) {

            chkenter = true;
            if (!isvalidNumber(obj, msg, allowNeg, allowdec, iscommaSepterated, defa)) {
                return setkey(e);
            }
            else {
                return true;
            }

        } else {

            return setkey(e);
        }


    }
    else {
        if (gkey == 45) {
            if (allowNeg) {

                if (doGetCaretPosition(obj) != 0) {
                    return setkey(e);
                }
                else if (doGetCaretPosition(obj) == 0 && obj.value.substring(0, 1) == "-") {
                    return setkey(e);
                }
            }
            else {
                return setkey(e);
            }
        }

        if (gkey == 46) {
            if (allowdec) {
                if (obj.value.indexOf(".") > -1)
                    return setkey(e);
            }
            else {
                return setkey(e);
            }
        }

        var valobj = obj.value;
        //alert(obj.value.indexOf("-"));
        if (obj.value.indexOf("-") > -1) {
            if (doGetCaretPosition(obj) == 0) {
                return setkey(e);
            }
        }


    }
}
function formatCurrency(dataobj) {
    var val = dataobj.value;
    var isNegative;
    isNegative = (val < 0);
    if (isNegative)
        val = val.substring(1, val.length);

    data = String(val).replace(/[\$,]/g, '');
    var temp = data.split(".");
    var output = "";
    var re = /(\d{1,3})$/;
    while (temp[0].match(re)) {

        output = "," + RegExp.$1 + output;
        temp[0] = temp[0].replace(re, '');
        re = /(\d{1,3})$/;
    }
    if ("undefined" !== typeof (temp[1]))
        output += "." + String(temp[1]);

    if (isNegative)
        dataobj.value = "-" + output.substring(1);
    else
        dataobj.value = output.substring(1);

}
function CheckNumberblur(obj, msg, allowNeg, allowdec, iscommaSepterated, defa, isPhoneNumber) {
    if (chkenter == false) {
        if (isvalidNumber(obj, msg, allowNeg, allowdec, iscommaSepterated, defa, isPhoneNumber)) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        chkenter = false;
    }

    return true;
}


function CheckDecimal(val) {
    if (val.value != '') {
        var regex = /^(-)?[0-9,]*(\.\d{0,2})?$/;
        if (regex.test(val.value) == false) {
            val1 = val.value.substring(0, val.value.length - 1);
            val.value = val1;
            return false;
        }
    }
}



function isvalidNumber(obj, msg, allowNeg, allowdec, iscommaSepterated, defa, isPhoneNumber) {
    if (iscommaSepterated == 'true') {
        obj.value = String(obj.value).replace(/[\$,]/g, '');
    }

    var NumRE = /^\d*[0-9]$/;

    if (allowNeg && allowdec) {
        NumRE = /^(\-)?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$/;
    }

    if (allowNeg && !allowdec) {
        NumRE = /^(\-)?\d*[0-9]$/;
    }

    if (!allowNeg && allowdec) {
        NumRE = /^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$/;
    }

    if (isPhoneNumber) {
        NumRE = /((^\([0-9]\d{2}\) ?\d{3}-\d{4}$)|(^[0-9]\d{2}-\d{3}-\d{4}$)|(\d{10}))/g; 
        msg = "Please enter Telephone number in correct format";        //HARSHALA : 09/16/2014 : YERDI3I-2416 : BT2653 :  User unable to move out of Telephone field screen locked. (8.1.5)
    }

    if (obj.value != '') {
        if (obj.value.match(NumRE)) {

            if (iscommaSepterated == 'true')
                formatCurrency(obj);
        }
        else {
        	if (msg != null) {
				if(msg !="") alert(msg);
	            setTimeout(function () { obj.focus(); }, 1);
                obj.select();
            }

            return false;
        }
    }

    return true;
}

function insertAtCursor(myField, myValue) {
    if (document.selection) {
        myField.focus();
        sel = document.selection.createRange();

        sel.moveStart('character', 0);
        sel.moveEnd('character', 0);
        sel.select();
        sel.text = '\n';
        sel.select();

    }
    else if (myField.selectionStart || myField.selectionStart == '0') {
        var startPos = myField.selectionStart;
        var endPos = myField.selectionEnd;
        myField.value = myField.value.substring(0, startPos) + myValue + myField.value.substring(endPos, myField.value.length);
        myField.selectionStart = startPos + 1;
        myField.selectionEnd = startPos + 1;
    }
    else {
        myField.value += myValue;
    }
}

function goodchars(e, goods, obj, maxlen, isMultiline) {
    var objvalue = obj.value;
    if (getkey(e) == 13) {
        if (isMultiline == true && getkey(e) == 13) {
            insertAtCursor(obj, '\n\r');
            return setkey(e);
        }
        chkenter = true;

        if (isvalidpassword(obj, goods) == false) {
            setTimeout(function () { obj.focus(); }, 1);
            obj.select();
            return false;
        }
    }
    else if (getkey(e) == 0 || getkey(e) == 8) {
        return true;
    }
    else if (objvalue.length >= maxlen && maxlen != 0) {
        setTimeout(function () { obj.focus(); }, 1);
        return setkey(e);
    }
    else {
        var key, keychar;
        key = getkey(e);
        if (key == null) return true;
        keychar = String.fromCharCode(key);
        keychar = keychar.toLowerCase();
        goods = goods.toLowerCase();
        if (goods.indexOf(keychar) == -1) {
            return setkey(e);
        }
        else {
            return true;
        }
        return false;
    }
}

function validatepassword(obj, goods, maxlen) {
    if (chkenter == false) {
        return isvalidpassword(obj, goods, maxlen);
    }
    else {
        chkenter = false;
    }

}
function isvalidpassword(obj, goods, maxlen) {
    if (obj != '') {

        goods = goods.toLowerCase();
        var val = obj.value;

        if (val.length > maxlen && maxlen != 0) {
            alert("The text exceeds maximum length of " + maxlen + " haracters.");
            setTimeout(function () { obj.focus(); }, 1);
            return false;
        }

        val = val.toLowerCase();
        var splitarr = val.split("");
        var count = -1;
        var rt = true;
        for (var i = 0; i < splitarr.length; i++) {
            if (goods.indexOf(splitarr[i]) < 0) {

                rt = false;
                break;
            }
        }

        if (rt) {
            return true;
        }
        else {
            alert("Text can contain only the following characters: \n " + goods);
            setTimeout(function () { obj.focus(); }, 1);
            obj.select();
            return false;
        }
    }
}

function checkInput(e, obj, maxlen, isMultiline) {
    var val = obj.value;

    if (getkey(e) == 13 || getkey(e) == 0) {
        if (isMultiline == true && getkey(e) == 13) {
            insertAtCursor(obj, '\n');
            return setkey(e);
        }

        chkenter = true;
        if (isvalidinput(obj) == false) {
            return setkey(e);
        }
    }
    else if (getkey(e) == 8) {
        return true;
    }
    else if (val.length >= maxlen && maxlen != 0) {
        return setkey(e);
    }
    else {
        if (getkey(e) == 60 || getkey(e) == 62) {
            return setkey(e);

        }
    }
}

function validateInput(obj, maxlen) {
    var objvalue = obj.value;
    if (chkenter == false) {
        if (objvalue.length > maxlen && maxlen != 0) {
            alert("The text exceeds maximum length of " + maxlen + " characters.");
            setTimeout(function () { obj.focus(); }, 1);
            obj.select();
            return false;

        } else if (isvalidinput(obj) == false) {
            if (document.getElementById(obj.id).Msg != null) {
                alert(document.getElementById(obj.id).Msg);
                setTimeout(function () { obj.focus(); }, 1);
                obj.select();
                return false;
            } else {
                setTimeout(function () { obj.focus(); }, 1);
                obj.select();
                return false;
            }


        }

    } else {
        chkenter = false;
    }

    return true;
}

function isvalidinput(obj) {
    if (obj.value != '') {
        var val = obj.value;
        if (val.indexOf("<") > -1 || val.indexOf(">") > -1) {

            alert("Text cannot contain any of the following characters: \n < > ");
            setTimeout(function () { obj.focus(); }, 1);
            obj.select();
            return false;
        }
    }
} 