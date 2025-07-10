//**************************************************************************************************************/
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	BankAcknowledgementHeaderImport.cs
// Author Name		:	Santosh B
// Employee ID		:	
// Email	    	:	
// Contact No		:	
// Creation Time	:	11/21/2019
// Description	    :	Class that used for storing and retreiving data from AtsSTWImportBaseheader Table.
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
     [System.Serializable]
    public class BankAcknowledgementHeaderImport
    {
        public int  UniqueID { get; set; }
        public string DARFilePath { get; set; }
        public string LARFilePath { get; set; }
        public string LARFileName { get; set; }
        public string FileRunDate { get; set; }
        public string DisbursementType { get; set; }
        public string Source { get; set; }
        public string PayrollDate { get; set; }
        public string Status { get; set; }
        public int  TotalRecords { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public decimal TotalStateTaxAmount { get; set; }
        public decimal TotalLocalFlatAmount { get; set; }
        public decimal TotalFedTaxAmount { get; set; }
        public decimal TotalDedcutionAmount { get; set; }
        public decimal TotalNetAmount { get; set; }

         
    }
}



	
	