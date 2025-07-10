/************************************************************************************************************************/
// Author: Sanjay GS Rawat
// Created on: 10/10/2018
// Summary of Functionality: Loan status structure
// Declared in Version: 20.6.0 | YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//
/************************************************************************************************************************/
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name                   | Date          | Version No        | Ticket
// ------------------------------------------------------------------------------------------------------
// Megha Lad	                    | 2019.01.17   |	20.6.2           | YRS-AT-3157 - YRS enh-audit trail db table to track Loan freeze and unfreeze activity and copy emails to IDM (TrackIT 27438)
// ------------------------------------------------------------------------------------------------------
//************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMCAObjects
{
    public struct LoanStatus
    {
        public static string ACCEPTED = "ACCEPTED";
        public static string APPROVED = "APPROVED";
        public static string CANCEL = "CANCEL";
        public static string DECLINED = "DECLINED";
        public static string EXP = "EXP";
        public static string PAID = "PAID";
        public static string PEND = "PEND";
        public static string REJECTED = "REJECTED";
        public static string WITHDRAWN = "WITHDRAWN";
        public static string DISB = "DISB";
        public static string PROOF = "PROOF";
        //START : ML | 2019.01.17 | YRS-AT-3157 | Loan status for Freeze unfreeze to use in SendMail call
        public static string FREEZE = "FREEZE";
        public static string UNFREEZE = "UNFREEZE";
        //END : ML | 2019.01.17 | YRS-AT-3157 | Loan status for Freeze unfreeze to use in SendMail call        
    }
}
