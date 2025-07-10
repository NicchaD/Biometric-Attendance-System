//**************************************************************************************************************/
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	StateWithholdingInput.cs
// Author Name		:	Megha Lad
// Employee ID		:	
// Email	    	:	
// Contact No		:	
// Creation Time	:	01/10/2019
// Description	    :	Class For State Withholding Input Fields . (AtsSTWStateWiseInputList Table).
// Declared in Version : 20.7.0 | YRS-AT-4598 - YRS enh: State Withholding Project - Annuity Payroll Processing
//**************************************************************************************************************
// MODIFICATION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------------
//**************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMCAObjects
{    
    [Serializable]
    public class StateWithholdingInput
    {
        public string chvStateCode { get; set; }
        public string chvStateName { get; set; }
        public string chvDisplayText { get; set; }
        public bool bitDisbursementType { get; set; }
        public bool bitStateTaxNotElected { get; set; }
        public bool bitMaritalStatusCode { get; set; }
        public bool bitNoOfExemption { get; set; }
        public bool bitAdditionalAmount { get; set; }
        public bool bitFlatAmount { get; set; }
        public bool bitPercentageofFederalWithholding { get; set; }
        public bool bitNewYorkCityAmount { get; set; }
        public bool bitYonkersAmount { get; set; }      
    }
}
