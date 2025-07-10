//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
//Modification History  :
//Modified Date		Modified By			Description
//*******************************************************************************
//10.10.2015        Pramod P. Pokale    YRS-AT-2588: implement some basic telephone number validation Rules
//2016.03.21        Anudeep A           YRS-AT-2594 - YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send)
//2016.07.12        Chandra sekar       YRS-AT-2772 - YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
//11.24.2016        Pramod P. Pokale    YRS-AT-3145 -  YRS enh: Fees - QDRO fee processing 
//                                      YRS-AT-3265 -  YRS enh:improve usability of QDRO split screens (TrackIT 28050)
// 2017.04.27       Manthan Rajguru     YRS-AT-3205 -  YRS enh: needed by DECEMBER 2017 - RMD Print letters function for QD participants with first RMD due in December (TrackIT 27977)    
//04.26.2017        Santosh Bura        YRS-AT-3400 -  YRS enh: due MAY 2017 - RMD Print Letters- Letter to Non-respondents (new screen) 
//                                      YRS-AT-3401 - RMD Print Letters- Satisfied but not elected (new screen)
// 2018.04.25       Sanjay GS Rawat	    YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
//09.04.2018        Santosh Bura        YRS-AT-4081 -  YRS enh: YRS enhancement-generate email to LPA when loan is closed.Track it 34779   
//2018.09.03        Vinayan C           YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab  
//2018.05.15        Manthan Rajguru     YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)
//10.04.2018        Pramod P. Pokale    YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)
//2018.10.11        Manthan Rajguru     YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//2018.10.12        Vinayan C           YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab  
//2018.10.16        Sanjay GS Rawat	    YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
//2019.01.17        Megha Lad           YRS-AT-3157 - YRS enh-audit trail db table to track Loan freeze and unfreeze activity and copy emails to IDM (TrackIT 27438)
//2019.09.18        Pooja Kumkar        YRS-AT-2670 - YRS enh:Hacienda withholding message - Puerto Rico based YMCA orgs 
//*******************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMCAObjects
{
    public enum EnumMessageTypes
    {
        Error,
        Warning,
        Information,
        Success
    }

    //Start: BJ-2015-04-29 BT:2570 YRS 5.0-2380 
    //This class is used to avoid hard code IDM Document Codes in each form.
    //After confirmation with our and YRF team we have to put IDM Document code here.
    //With implementation of this calss we can centralize the IDM Document Codes
    public static class IDMDocumentCodes
    {
        # region "Annuity Beneficiary And Annuity Beneficiary Death Certificate Follow-up"
        //Start-2015.07.22/SR - YRS 5.0-2380 - Updating New Doc code received from Tyd, Chris
        public const string Ann_Bene_Death_InitialLetter = "DECSURLT";
        public const string Ann_Bene_Death_Followup_FirstFollowup = "DESULT60";
        public const string Ann_Bene_Death_Followup_SecondFollowup = "DESULT90";
        //End-2015.07.22/SR - YRS 5.0-2380 - Updating New Doc code received from Tyd, Chris        
        # endregion
        //Start: AA:03.21.2016 YRS-AT-2016 Added below lines for UEIN DOccode
        #region UEIN Summary
        public const string Unfunded_Interest_Summary = "UEINSUMM";
        # endregion
        //End: AA:03.21.2016 YRS-AT-2016 Added below lines for UEIN DOccode

        //START: Chandra sekar | 2016.07.12 | YRS-AT-2772 | DocCode defined for Waived Participant List
        #region Waived Participant List
        public const string Waived_Participant_List = "WAIVLIST";
        #endregion Waived Participant List
        //END: Chandra sekar | 2016.07.12 | YRS-AT-2772 | DocCode defined for Waived Participant List

        //START: MMR | 2017.04.24 | YRS-AT-3205 | Added Doc code for initial and follow-up letter for RMD special QD participants
        # region "RMD Special QDRO"
        public const string RMD_SpecialQD_InitialLetter = "RMDIAPLT";
        public const string RMD_SpecialQD_FollowupLetter = "RMDFAPLT";
        # endregion
        //END: MMR | 2017.04.24 | YRS-AT-3205 | Added Doc code for initial and follow-up letter for RMD special QD participants

        //START: SB | 2017.04.25 | YRS-AT-3400 & 3401 | DocCOde for Non Respondent and RMD Satisfied Not Elected participants
        public const string RMD_NonRespondent_AnnualLetter = "RMDNRLET";
        public const string RMD_SatisfiedNotElected_AnnualLetter = "RMDERLET";
        //END: SB | 2017.04.25 | YRS-AT-3400 & 3401 | DocCOde for Non Respondent and RMD Satisfied Not Elected participants

        //START: MMR | 2018.05.15 | YRS-AT-3929 | Added doc code for letter generated for participant and YMCA during loan processing
        #region EFT Loan Disbursement
        public const string LoanProcessing_ParticipantLetter = "LNLETTR1";
        public const string LoanProcessing_YMCALetter = "LNLETTR2";
        public const string LoanProcessing_ParticipantEmail = "ELDISBUR"; //PPP | 2018.10.04 | YRS-AT-4017 | DocCode for disbursed EFT Loans
        public const string Loan_Cancel_ParticipantEmail = "ELCANCEL";  //MMR | 2018.10.11 | YRS-AT-3101 | DocCode for Canceled Loans
        public const string Loan_Expired_ParticipantEmail = "ELEXPIRE";  //PPP | 2018.10.19 | YRS-AT-3101 | DocCode for Expired Loans
        public const string Loan_Paid_ParticipantEmail = "ELPAIDXX";  //PPP | 2018.10.19 | YRS-AT-3101 | DocCode for Paid Loans
        public const string Loan_EFT_Rejected_ParticipantEmail = "ELEFTREJ";  //PPP | 2018.10.19 | YRS-AT-3101 | DocCode for not in good order EFT payments
        public const string LoanProcessing_LPAEmail = "ELPAIDLP"; // SR | 2018.11.13 | YRS-AT-4017 | DocCode for disbursed EFT Loans - LPA
        #endregion EFT Loan Disbursement
        //END: MMR | 2018.05.15 | YRS-AT-3929 | Added doc code for letter generated for participant and YMCA during loan processing

        //START: SB | 2018.09.04 | YRS-AT-4081 | DocCode for saving email in IDM, this email is sent when loan gets closed due to funding of last amortization
        #region Loan Payed off
        public const string Loan_PayedOff_LPA_Email = "LNPDFLPA";
        # endregion
        //END: SB | 2018.09.04 | YRS-AT-4081 | DocCode for saving email in IDM, this email is sent when loan gets closed due to funding of last amortization

        //START: VC | 2018.03.10 | YRS-AT-4018 | DocCode for saving email in IDM, this email is sent when loan gets approved or declined
        #region Person Maintenance
        public const string Loan_Approve_Email = "ELAPPROV";
        public const string Loan_Decline_Email = "ELDECLIN";
        # endregion
        //END: VC | 2018.03.10 | YRS-AT-4018 | DocCode for saving email in IDM, this email is sent when loan gets approved or declined

        //START: SR | 2018.10.04 | YRS-AT-3101 | DocCode for saving email in IDM, this email is sent through payment manager.
        #region Person Maintenance
        public const string Loan_EFT_Paid_Email = "ELPAIDXX";
        public const string Loan_EFT_payment_Fail_Email = "ELEFTREJ";
        public const string Loan_EFT_payment_Fail_Review_Email = "ELEFTRJ2";
        //START: ML | 2019.01.17 | YRS-AT-3157 | DocCode for saving FreezeUnfreeze Loan email in IDM
        public const string Loan_Freeze = "EMLLNFRZ";
        public const string Loan_Unfreeze = "EMLLNUFZ";
        //END: ML | 2019.01.17 | YRS-AT-3157 | DocCode for saving FreezeUnfreeze Loan email in IDM
        # endregion
        //END: SR | 2018.10.04 | YRS-AT-3101 |  DocCode for saving email in IDM, this email is sent through payment manager.
    }
    //End: BJ-2015-04-29 BT:2570 YRS 5.0-2380 

    //START: PPP | 2015.10.10 | YRS-AT-2588 | TelephoneType defined to identify the telephone number sent for validation
    /// <summary>
    /// Defines type of telephone number
    /// </summary>
    public enum TelephoneType
    {
        Home = 0,
        Mobile = 1,
        Office = 2,
        Fax = 3,
        Work = 4,
        Telephone = 5, // Some pages use caption as Telephone
        PhoneNumber = 6
    }
    //END: PPP | 2015.10.10 | YRS-AT-2588 | TelephoneType defined to identify the telephone number sent for validation

    //START: Chandra sekar | 2016.07.12 | YRS-AT-2772 | Enum to define report types
    public enum EnumReportType
    {
        CRYSTAL,
        RDLC
    }
    //END: Chandra sekar | 2016.07.12 | YRS-AT-2772 | Enum to define report types

    //START: PPP | 11/24/2016 | YRS-AT-3145 | Non retired QDRO split page tabs will be identified by this enum
    public enum EnumNonRetiredQDROTabs
    {
        DefineBeneficiary = 0,
        SplitAccounts = 1,
        ManageFees = 2,
        ReviewAndSave = 3,
        Status = 4
    }

    public struct DBActionType
    {
        public static readonly string CREATE = "C";
        public static readonly string UPDATE = "U";
        public static readonly string DELETE = "D";
    }
    //END: PPP | 11/24/2016 | YRS-AT-3145 | Non retired QDRO split page tabs will be identified by this enum

    // START : SR | 2018.04.25 | YRS-AT-3101 | Added new method to define email templates from BO class.
    public enum EnumEmailTemplateTypes
    {
        NO_TEMPLATE, // PPP | 2018.10.17 | YRS-AT-3101 | Added common email template for mail communication to particpant
        LOAN_CLOSED_PERSONS,
        LOAN_DEFAULTED_PERSONS,
        SERVICE_FAILURE,
        SESSION_MGMNT,
        CASHOUT_BATCH_DISB_SUMMARY,
        HARDSHIP_TD_TERMN_INTIMATION,
        DELINQ_INTIMATION,
        COPY_FAILURE,
        LOAN_PROCESS_INTIMATION,
        LOAN_DEFAULT_INTIMATION,
        LOAN_CLOSED_INTIMATION,
        LOAN_SUSPEND_INTIMATION,
        WITHDRAWAL_NON_US_CANADIAN,
        VOID_DISB_PRIOR_MONTH_WITH_FEES,
        VOID_DISB_PRIOR_MONTH_WITHOUT_FEES,
        VOID_REISSUE_WITHDRAWAL,
        YMCA_MERGER_INTIMATION,
        YMCA_MERGER_LOAN_INTIMATION,
        LOAN_FREEZE_PROCESS,
        LOAN_UNFREEZE_PROCESS,
        LOAN_DEFAULT_CLOSED_PERSONS,
        SPECIAL_DEATH_PROCESSING_REQUIRED,
        UNFUNDED_UEIN_INTIMATION,
        LOAN_PAID_CLOSED_PERSONS,
        // START : VC | 2018.10.12 | YRS-AT-4018 | Email templates for Approve and Decline process of loan.
        LAC_APPROVE_LOAN,
        LAC_DECLINE_LOAN,
        // END : VC | 2018.10.12 | YRS-AT-4018 | Email templates for Approve and Decline process of loan.
        LOAN_STATUS_UPDATE_PERSONS, //MMR | 2018.10.12 | YRS-AT-3101 | Added common email template for mail communication to particpant
        LOAN_PAID_EFT_YMCA, //PPP | 2018.10.19 | YRS-AT-3101 | Template for email which will be sent when EFT loans get marked as PAID
        //LOAN_STATUS_UPDATE_PARTICIPANT, //MMR | 2018.10.12 | YRS-AT-3101 | Added common email template for mail communication to particpant
        LOAN_EFT_PAYMENT_FAILURE,
        //LOAN_PROCESSED_EFT_YMCA  // SR | 2018.10.15 | YRS-AT-3101 | Added common email template for mail communication to particpant
        WITHDRAWAL_PROCESSING_PUERTORICO //PK |09-18-2019 |YRS-AT-2670 | Added email template for processing puertorico withdrawal.
    }
    // END : SR | 2018.04.25 | YRS-AT-3101 | Added new method to define email templates from BO class.
}
