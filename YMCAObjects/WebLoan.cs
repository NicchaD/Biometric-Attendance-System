/************************************************************************************************************************/
// Author: Vinayan C.
// Created on: 07/19/2018
// Summary of Functionality: Data object for filtering web loans.
// Declared in Version: 20.6.0 | YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)  
//
/************************************************************************************************************************/
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name               | Date        | Version No    | Ticket
// ------------------------------------------------------------------------------------------------------
//                              |             |               | 
/************************************************************************************************************************/

using System;
namespace YMCAObjects
{
    public class WebLoan
    {
        public string FullName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string FundId { get; set; }

        public string SSN { get; set; }

        public string Amount { get; set; }

        public string Status { get; set; }

        // START : VC | 2018.08.20 | YRS-AT-4018 -  Declared properties for Approve and decline operations 

        public string LoanOriginationId { get; set; } 
        
        public string SignatureReceivedDate { get; set; } 
        
        public string Decider { get; set; } 
        
        public string MaritalStatus { get; set; } 
        
        public string DocCode { get; set; }

        public string DeclineComment { get; set; }

        public string DeclineReason { get; set; }

        public string PaymentMethod { get; set; }//VC | 2018.10.12 | YRS-AT-4018 -  Declared Payment Method properties

        // END : VC | 2018.08.20 | YRS-AT-4018 -  Declared properties for Approve and decline operations 
    }
}
