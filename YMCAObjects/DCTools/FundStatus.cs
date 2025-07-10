/************************************************************************************************************************/
// Author: Pramod Prakash Pokale
// Created on: 09/18/2017
// Summary of Functionality: Helps to maintain values for session and save operation of DC Tools - Change Fund Status screen 
// Declared in Version: 20.4.0 | YRS-AT-3631 -  YRS enh: Data Corrections Tool -Admin screen function to allow manual update of Fund status from WD or WP to TM or PT (TrackIT 30950) 
//
/************************************************************************************************************************/
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------
// 			                   | 	          |		            | 
// ------------------------------------------------------------------------------------------------------
/************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMCAObjects.DCTools
{
    public class FundStatus
    {
        public int FundNo { get; set; }

        public string PersID { get; set; }

        public string FundEventID { get; set; }

        public string OldFundEventStatus { get; set; }

        public string OldFullFundEventStatus { get; set; }

        public string NewFundEventStatus { get; set; }

        public string NewFullFundEventStatus { get; set; }

        public decimal Balance { get; set; }

        public string SystemNotes { get; set; }

        public string UserNotes { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
        
        public string MiddleName { get; set; }
    }
}
