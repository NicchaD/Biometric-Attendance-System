//**************************************************************************************************************/
// Author: Santosh Bura
// Created on: 09/15/2017
// Summary of Functionality: Class created to Get/Set Remaining Death Benefit amount details in property
// Declared in Version: 20.4.0 | YRS-AT-3541 -  YRS enh: Data Corrections Tool -Admin screen function to allow modification of RDB amounts (excessive data corrections) 

//**************************************************************************************************************
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------
//             | 	      |		   | 
// ------------------------------------------------------------------------------------------------------
//**************************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace YMCAObjects.DCTools
{
    [System.Serializable]
    public class YMCARemainingDeathBenefitAmount
    {

        public string SelectedAnnuityIndex { get; set; }

        public Decimal OldAmount { get; set; }

        public Decimal NewAmount { get; set; }

        public DateTime TransactDate { get; set; }

        public string SystemNotes { get; set; }

        public string UserNotes { get; set; }

        public Decimal NewDifferenceAmount { get; set; }

        public string TransactType { get; set; }

        public string PerssID { get; set; }

        public string AnnuityID { get; set; }

        public string RetireeID { get; set; }

        public Boolean IsMismatchAmount { get; set; }

        public DateTime AdjustmentAllowedDate { get; set; }

        public DateTime AllowedTransactDate { get; set; }

    }
}
