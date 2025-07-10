/************************************************************************************************************************/
// Author: Manthan Rajguru
// Created on: 09/04/2018
// Summary of Functionality: Loan Opertion DTO
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
namespace YMCAObjects
{
    public class LoanOperation
    {
        //Following will be used to set PDF/Email activity related status which happens during loan processing
        public string Report { get; set; }
        public string AppType { get; set; }
        public string DocTypeCode { get; set; }
        public string FilePath { get; set; }
        public string IDXPath { get; set; }
        public bool IDMStatus { get; set; }
        public string Error { get; set; }
        public string MailMessage { get; set; }
        public bool EmailStatus { get; set; }
        public string EmailErrorMessage { get; set; }
        public string EmailID { get; set; }
        public EnumEmailTemplateTypes EmailTemplate { get; set; }
        public bool IsIDMCopyRequired { get; set; }
        public string IDMDestinationPath { get; set; }
    }
}
