//**************************************************************************************************************/
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	StateWithholdingDetails.cs
// Author Name		:	Megha Lad
// Employee ID		:	
// Email	    	:	
// Contact No		:	
// Creation Time	:	01/10/2019
// Description	    :	Class For State Withholding Details Participant wise. (AtsSTWPersStateTaxdetails Table).
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
    public class StateWithholdingDetails
    {
        public int  intUniqueID { get; set; }
        public string guiPersID { get; set; }
        public string chvStateCode { get; set; }
        public string chvDisbursementType { get; set; }
        public Boolean  bitStateTaxNotElected { get; set; }
        public string  chvMaritalStatusCode { get; set; }
        public Int32?  intNoOfExemption { get; set; }
        public Double? numAdditionalAmount { get; set; }
        public Double? numFlatAmount { get; set; }
        public Double? numPercentageofFederalWithholding { get; set; }
        public Double? numNewYorkCityAmount { get; set; }
        public Double? numYonkersAmount { get; set; }
        public Double? numComputedTaxAmount { get; set; }      
        public Boolean bitActive {get; set;}
    }
}
