

//**************************************************************************************************************/
// Author: Sanjay GS Rawat Rajguru
// Created on: 09/13/2017
// Summary of Functionality: Class created to Get/Set Annuity details in property
// Declared in Version: 20.4.0 | YRS-AT-3666 - YRS enh: Data Corrections Tool) -Reverse Annuity 

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
    public class ReverseAnnuity
    {
        public string DisbursementId { get; set; }

        //public string FundEventId { get; set; }

        //public string PersId { get; set; }

        public string PlanType { get; set; }

        public string PurchaseDate { get; set; }

        public string CurrentPayment { get; set; }

        public string Notes { get; set; }
    }
}




