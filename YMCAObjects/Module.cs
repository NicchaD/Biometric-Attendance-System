/************************************************************************************************************************/
// Author: Pramod Prakash Pokale
// Created on: 11/16/2016
// Summary of Functionality: It helps to define module wherever it is required.
// Declared in Version: 18.3.1 | YRS-AT-3146 -  YRS enh: Fees - Partial Withdrawal Processing fee 
//
/************************************************************************************************************************/
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name               | Date        | Version No    | Ticket
// ------------------------------------------------------------------------------------------------------
// Pramod Prakash Pokale        | 03/14/2017  |   20.0.3      | YRS-AT-2625 -  YRS enh: change in disability Annuity calculations (TrackIT 24012) 
// Manthan Milan Rajguru        | 09/18/2017  |   20.4.0      | YRS-AT-3665 -  YRS enh: Data Corrections Tool - Admin screen option to create a manual credit 
// Santosh Bura                 | 10/11/2017  |   20.4.0      | YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
// Pramod Prakash Pokale        | 10/03/2018  |   20.6.0      | YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)
// Sanjay GS Rawat              | 10/16/2018  |   20.6.0      | YRS-AT-3101 -YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
// ------------------------------------------------------------------------------------------------------
/************************************************************************************************************************/

using System;

namespace YMCAObjects
{
    public struct Module
    {
        public static readonly string Withdrawal = "WITHDRAWAL";
        public static readonly string QDRO = "QDRO";
        public static readonly string Retirement = "RETIREMENT"; //PPP | 03/14/2017 | YRS-AT-2625 | Will be used to send module in AtsYRSActivityLog table
        public static readonly string DCTools = "DCTOOLS"; //MMR | 09/18/2017 | YRS-AT-3665 | Will be used to send module in AtsYRSActivityLog table for Data Correction tools
        //START: SB | 2017.10.11 | YRS-AT-3324 | will be used to send module name to database and retrive error message based on module name
        public static readonly string Death_Notification = "DEATH_NOTIFICATION";
        public static readonly string Death_Settlement = "DEATH_SETTLEMENT";
        //END: SB | 2017.10.11 | YRS-AT-3324 | will be used to send module name to database and retrive error message based on module name
        public static readonly string LACLoanProcessing = "LACLoanProcessing"; //PPP | 10/03/2018 | YRS-AT-4017 | Will be used for batch log creation in LAC mulitiple Loan Processing
        public static readonly string PaymentManagerEFTLoan = "PM_EFT_TDLOAN"; //SR | 10/16/2018 | YRS-AT-3101 | Will be used for batch log creation in payment manager EFT loan processing.
        public static readonly string PaymentManagerEFTApprovalRejection = "EFTApprovalRejection"; //SR | 10/16/2018 | YRS-AT-3101 | Will be used for batch log creation in payment manager EFT loan processing.
        
    }
}
