/************************************************************************************************************************/
// Author: Sanjay GS Rawat
// Created on: 08/03/2018
// Summary of Functionality: Loan DTO
// Declared in Version: 20.6.0 | YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
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
    public class Loan
    {   
        // following will be used as input for common class(Loans.vb)
        public string LoanNumber { get; set; }

        public string SSN { get; set; }
        
        public string PersId { get; set; }
        
        public string FundId { get; set; }
        
        public int FundNo { get; set; }
        
        public string FirstName { get; set; }
        
        public string MiddleName { get; set; }
        
        public string LastName { get; set; }
        
        public string YMCAId { get; set; }
        
        public string YMCANo { get; set; }
        
        public double Amount { get; set; }
        
        public int RequestNo { get; set; }
        
        public string PaymentMethodCode { get; set; }
        
        public DataSet DeductionTable { get; set; }
        
        public string Status { get; set; }
        
        public string Error { get; set; }

        public LoanOperation Participant { get; set; }

        public LoanOperation YMCA { get; set; }
        
        public bool IsAccountLocked { get; set; }
        
        public double ONDAmount { get; set; }

        public System.Collections.Generic.List<string> Messages { get; set; }

    }
}
