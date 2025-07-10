	function _OnBlur_TaxableAmount()
	{
		document.Form1.all("TextboxCheckTotal").value=Number(document.Form1.all("TextboxTaxableAmount").value) +Number(document.Form1.all("TextboxNonTaxableAmount").value);
	}

	function _OnBlur_NonTaxableAmount()
	{
		document.Form1.all("TextboxCheckTotal").value=Number(document.Form1.all("TextboxTaxableAmount").value) +Number(document.Form1.all("TextboxNonTaxableAmount").value);
	}

	function _OnBlur_TextboxCheckNo()
	{
		var _arr = new Array(20);
		var flg = false;
		var str = String(document.Form1.all.TextboxCheckNo.value);

		for(i=0;i<str.length; i++)
		{
			_arr[i] = str.substr(i,1);
		}

		for ( i=0;i<str.length -1;i++)
		{
			if (_arr[i] != _arr[i + 1])
			{
				flg = true;
			}
		}

		if(!flg)
		{
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
	function detectBrowser()
	{
		if (window.navigator.appName.toLowerCase().indexOf("netscape") > -1) 
			theform = document.forms["Form1"];
		else 
		theform = document.Form1;

		//browser detection
		var strUserAgent = navigator.userAgent.toLowerCase();
		isIE = strUserAgent.indexOf("msie") > -1;
		isNS = strUserAgent.indexOf("netscape") > -1;
	}

	function FormatAmtControl(ctl)
	{
		var vMask ;
		var vDecimalAfterPeriod ;
		var ctlVal;
		var iPeriodPos;
		var sTemp;
		var iMaxLen 
		var ctlVal;
		var tempVal;
		ctlVal = ctl.value;
		vDecimalAfterPeriod  = 2
		iMaxLen  = ctl.maxLength;

		if (isNaN(ctlVal))
		{

		}
		else
		{
			ctlVal =  ctl.value;
			iPeriodPos =ctlVal.indexOf(".");

			if (iPeriodPos<0)
			{ 
				if(ctl.value.length >(iMaxLen-5))
				{ 
					if(ctlVal.substr(0,3) =="100")
					{ 
						tempVal= "100.000" ;
					}
					else
					{ 
					sTemp = ctl.value;
					//alert(sTemp.substr(0,2));
					tempVal = sTemp.substr(0,2) + "." + sTemp.substr(2,iMaxLen-3);
					}
				}
				else
					tempVal = ctlVal + ".0000";
				}
			else
			{
				if ((ctlVal.length - iPeriodPos -1)==1)
					tempVal = ctlVal + "000";
				if ((ctlVal.length - iPeriodPos -1)==0)
					tempVal = ctlVal + "0000";
				if ((ctlVal.length - iPeriodPos -1)==2)
					tempVal = ctlVal + "00";
				if ((ctlVal.length - iPeriodPos -1)==3)
					tempVal = ctlVal + "0";
				if ((ctlVal.length - iPeriodPos -1)==4)
					tempVal = ctlVal ;
				if ((ctlVal.length - iPeriodPos -1)>4)
				{
					tempVal = ctlVal.substring(0,iPeriodPos+5);
				}
			}
		}
		ctl.value=tempVal;
	}

	/*
	This function is responsible for filtering the keys pressed and the maintain the amount format of the 
	value in the Text box
	*/
	function HandleAmountFiltering(ctl)
	{
		var iKeyCode, objInput;
		var iMaxLen 
		//var reValidChars = /[0-9],./;
		var reValidChars = "0123456789."
		//var reValidChars = /^\d*(\.\d+)?$/;
		var strKey;
		var sValue;
		var event = window.event || arguments.callee.caller.arguments[0];
		iMaxLen  = ctl.maxLength;
		sValue = ctl.value;
		detectBrowser();

		if (isIE) 
		{
			iKeyCode = event.keyCode;
			objInput = event.srcElement;
		} 
		else 
		{
			iKeyCode = event.which;
			objInput = event.target;
		}

		strKey = String.fromCharCode(iKeyCode);
		//alert(event.keyCode);
		//alert(reValidChars.indexOf(strKey));

		//if (reValidChars.test(strKey))
		if(reValidChars.indexOf(strKey)!=-1)
		{
			//alert(iKeyCode);
			if(iKeyCode==46)
			{
				if(objInput.value.indexOf('.')!=-1)
					if (isIE)
						event.keyCode= 0;
					else
					{
						if(event.which!=0 && event.which!=8)
							return false;
					}
			}
			else
			{
				/*
				if(objInput.value.indexOf('.')==-1)
				{
					if (objInput.value.length>=(iMaxLen-3))
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
				if ((objInput.value.length==(iMaxLen-3)) && (objInput.value.indexOf('.')==-1))
				{
					objInput.value = objInput.value +'.';			
				}*/	
			}
		}
		else{
			if (isIE)
				event.keyCode= 0;
			else
			{
				if(event.which!=0 && event.which!=8)
					return false;
			}
		}
	}

	/*function ValidateNumeric()
	{
		if ((event.keyCode < 48)||(event.keyCode > 57))
		{
			event.returnValue = false;
		}
	}*/

//Added by imran For BT-1083
function ValidateNumeric()
	{
			if(!(event.keyCode==45||event.keyCode==46||event.keyCode==48||event.keyCode==49||event.keyCode==50||event.keyCode==51||event.keyCode==52||event.keyCode==53||event.keyCode==54||event.keyCode==55||event.keyCode==56||event.keyCode==57))	
				{			event.returnValue=false;		}	
		
	}

//Shashi shekhar:2009-12-23:Added security control function	
 function CheckAccess(controlname)
{
var str=String(document.Form1.all.HiddenSecControlName.value);
	//NP:2010.02.26 - onsite change to prevent incorrect matches - if (str.match(controlname)!= null)
	if (str.match(controlname + ',')!= null)
	{
		alert("Sorry, You are not authorized to do this activity.");
		return false;
	}
	else
	{
		return true;
	}
	
}

//Sanjay:2011:01:05 : formatiing textbox value	
function CurrencyFormatted(amount) {
    var i = parseFloat(amount);
    if (isNaN(i)) { i = 0.00; }
    var minus = '';
    if (i < 0) { minus = '-'; }
    i = Math.abs(i);
    i = parseInt((i + .005) * 100);
    i = i / 100;
    s = new String(i);
    if (s.indexOf('.') < 0) { s += '.00'; }
    if (s.indexOf('.') == (s.length - 2)) { s += '0'; }
    s = minus + s;
    return s;
}
//BS:2011.10.17--this method used for rounding decimal value it will accept two parameter one is value which u want to be round and another is decimal value
function roundVal(val, dec) {
    if (dec == null) {
        dec = 0;
    }
    var result = Math.round(val * Math.pow(10, dec)) / Math.pow(10, dec);
    return result;
}
function ShowMessage(controlId,strMessage, type) {
    var strControlID = '#' + controlId;
    $(strControlID).html(strMessage);
    $(strControlID).css('display', 'block');
    if (type == "error") {
        $(strControlID)[0].className = "error-msg";
    }
    else if (type == "success") {
        $(strControlID)[0].className = "success-msg";
    }
    else if (type == "info") {
        $(strControlID)[0].className = "info-msg";
    }
    else if (type == "Warn") {
        $(strControlID)[0].className = "warning-msg";
    }
}