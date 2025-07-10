//**************************************************************************************************************/
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	StateWithholdingPersonDetails.cs
// Author Name		:	Pooja Kumkar
// Employee ID		:	
// Email	    	:	
// Contact No		:	
// Creation Time	:	01/10/2019
// Description	    :	Class to declare variable for State Withholding Persoanl details .
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
    public class StateWithholdingPersonDetails
    {
        public int intFundNumber { get; set; }
        public string guiPersID { get; set; }
        public string chrStateCode { get; set; }
        public string chvStateName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public Double? numCurrentAnnuity { get; set; }
    }
}
