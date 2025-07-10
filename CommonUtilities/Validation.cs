//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	Validation.cs
// Author Name		:	Pramod Prakash Pokale
// Employee ID		:	
// Email			:	
// Contact No		:	
// Creation Time	:	10/10/2015
// Description  	:	Provides validation functions.
//*******************************************************************************
//Modified By           Date            Description
//*******************************************************************************
//Chandra C             02/18/2016      YRS-AT-1483 - YRS enh: Withdrawals Phase2: Move web methods for address updates into 
//                                      sprint 2 - YRSwebService, (GetAddressPhone, UpdateAddressPhone add new Lock Address validation)
//*******************************************************************************

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//START: CC | 02/18/2016 | YRS-AT-1483 | Corrected the namespace
//namespace CommonUtilities
namespace YMCARET.CommonUtilities
//END: CC | 02/18/2016 | YRS-AT-1483 | Corrected the namespace
{
    public class Validation
    {
        /// <summary>
        /// Validates telephone number which belongs to USA and Canada.
        /// </summary>
        /// <param name="number">Telephone number</param>
        /// <returns>True (for valid numbers) / False (for invalid numbers)</returns>
        public static bool Telephone(string number)
        {
            string stNumberStartsWith;
            bool bIsValid = false;
            string stRegExpPatternForUSCATelephone = @"(?x)
                                                        ^
                                                        # fail if...
                                                        (?!
                                                            # repeating numbers
                                                            (\d) \1+ $
                                                            |
                                                            # sequential ascending
                                                            (?:0(?=1)|1(?=2)|2(?=3)|3(?=4)|4(?=5)|5(?=6)|6(?=7)|7(?=8)|8(?=9)|9(?=0)){9} \d $
                                                            |
                                                            # sequential descending
                                                            (?:0(?=9)|1(?=0)|2(?=1)|3(?=2)|4(?=3)|5(?=4)|6(?=5)|7(?=6)|8(?=7)|9(?=8)){9} \d $
                                                        )
                                                        # match any other combinations of 10 digits
                                                        \d{10}
                                                        $
                                                    ";
            if (Regex.IsMatch(number, stRegExpPatternForUSCATelephone))
                bIsValid = true;

            if (bIsValid)
            {
                stNumberStartsWith = number.Substring(0, 1);
                if (stNumberStartsWith == "0" || stNumberStartsWith == "1")
                    bIsValid = false;
            }
            return bIsValid;
        }
    }
}
