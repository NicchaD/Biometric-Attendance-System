function getkey(e)
{
    if (window.event)
        return window.event.keyCode;
    else if (e)
        return e.which;
    else
        return null;
}

function setkey(e)
{
    if (window.event)
    {
        window.event.keyCode=0;
        return false;
    }
    else
    {
        return false;
    }
}

   function doGetCaretPosition (ctrl) { 	
   var CaretPos = 0;	
    if (document.selection) { 		
        ctrl.focus ();		
        var Sel = document.selection.createRange (); 		
        Sel.moveStart ('character', -ctrl.value.length); 		
        CaretPos = Sel.text.length;	
        }	
      else if (ctrl.selectionStart || ctrl.selectionStart == '0')		
        CaretPos = ctrl.selectionStart; 	
        
        return (CaretPos); } 
        
function isvalidnumber(obj)
{
    obj.value= Trim(obj.value)
    if (obj.value != '')
    { 
        var tanRE =/[.\-eE]/;
        if(!isNaN(obj.value))
        {
            if (obj.value.match(tanRE)) 
            {
                obj.focus();
                return false;
            }
        }
        else
        {
            obj.focus();
            return false;
        }
    }
    return true
}

function Trim(sTargetString)
{
	var sResultString ;
	sResultString = LTrim(sTargetString) ;
	sResultString = RTrim(sResultString) ;
	return sResultString ;	
}

function LTrim(sTargetString)
{
	var i ;
	var len ;
	var iIndexOfNonBlank ;
	var sResultString ;
	
	len = sTargetString.length ;
	for (i=0; i < len; i++)
	    {
		if (sTargetString.charAt(i) != ' ')
		   break ;
	    }
	sResultString = sTargetString.substring(i, len) ;
	return sResultString ;
}


function RTrim(sTargetString)
{
	var i ;
	var len ;
	var iIndexOfNonBlank ;
	var sResultString ;
	
	len = sTargetString.length ;
	for (i=len-1; i>=0; i--)
	    {
		if (sTargetString.charAt(i) != ' ')
		   break ;
	    }
	sResultString = sTargetString.substring(0, i + 1) ;
	return sResultString ;
}
  
function TrimZeroes_control(obj)
{
    var value = obj;
    var j;
    var isNegative;
              isNegative = (value<0);
              if(isNegative)
                j=1
              else
                j= 0
                
    if(value != '0' && value.length > 0)
    {
        for(var i = j;i < value.length; i++)
        {
            if(value.substr(i,1) != '0' || i==value.length-1)
            {
                if(isNegative)
                    return "-" + value.substr(i);
                else
                    return value.substr(i);
            } 
        } 
    }else
    {
        return value;
    }   
}