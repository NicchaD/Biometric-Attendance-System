/************************************************************************************************************************/
// Author: Megha Lad
// Created on: 01/23/2019
// Summary of Functionality: Loan FreezeUnfreeze History
// Declared in Version: 20.6.2 | YRS-AT-3157 -  YRS enh-audit trail db table to track Loan freeze and unfreeze activity and copy emails to IDM (TrackIT 27438)
//
/************************************************************************************************************************/
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name                   | Date          | Version No        | Ticket
// ------------------------------------------------------------------------------------------------------
// 			                        | 	            |		            | 
// ------------------------------------------------------------------------------------------------------
/************************************************************************************************************************/

using System.Data;

namespace YMCAObjects
{
    public class LoanFreezeUnfreezeHistory
    {
       
        public string Status { get; set; }

        public string DateTime { get; set; }

        public string Amount { get; set; }

        public string User { get; set; }
        
        public string Reason { get; set; }
        
    }
}