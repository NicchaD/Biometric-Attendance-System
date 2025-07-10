//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		    :	YMCA YRS
// FileName			    :	MetaMessageDA.cs
// Author Name		    :	Shashank Patel
// Creation Time		:	08/03/2014
// Description          :   This class is used to Maintain Message No. which is used to 
//                          get message content from database based on message No.
//                          This class is used for global declartion of Message No. 
//                          which helps for maintenance eaisly and centralized control.  
//                          YRS 5.0-2279 : Add in Administration Screen ability to change messages for YRS or Web site      
//Modification History  :
//Modified Date		Modified By			Description
//*******************************************************************************
//04.15.2015        Anudeep             BT:2699:YRS 5.0-2441 : Modifications for 403b Loans 
//2015.04.27      Dinesh Kanojia         BT:2699: YRS 5.0-2441 : Modifications for 403b Loans

//2015-04-23        B. Jagadeesh        Added Meta Messages List for Annuity Beneficiary
//07.29.2015        Anudeep             YRS 5.0-2441 : Modifications for 403b Loans 
//10.12.2015        Pramod P. Pokale    YRS-AT-2588: implement some basic telephone number validation Rules
//2015.11.04        Manthan Rajguru     YRS-AT-2453: Need ability to select other YMCA association for use in Loan reAmortization
//2015.11.10        Manthan Rajguru     YRS-AT-2669: YRS enh: do not generate Letters or Emails or Loans Offset due to death
//2016.01.18        Anudeep A           YRS-AT-2662: YRS enh: follow up to Loans release - nightly sql server job for Offset and Unfreeze processing
//2016.01.19        Bala                YRS-AT-2398: Customer Service requires a Special Handling alert for officers.
//2016.01.29        Pramod P. Pokale    YRS-AT-2594: YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send) 
//2016.02.03        Manthan Rajguru     YRS-AT-2334 -  Enhancement to YRS YMCA Maintenance-add a suspend participation option
//2016.02.18        Anudeep A           YRS-AT-2640 - YRS enh: Withdrawals Phase2:Sprint2: allow AdminTool link to launch a prepopulated Yrs withdrawal page
//2016.05.05        Manthan Rajguru     YRS-AT-2594 -  YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send)
//2016.07.05        Anudeep A           YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
//11/18/2016        Pramod P. Pokale    YRS-AT-3146 - YRS enh: Fees - Partial Withdrawal Processing fee 
//09/14/2017        Pramod P. Pokale    YRS-AT-3665 - YRS enh: Data Corrections Tool - Admin screen option to create a manual credit
//09/18/2017        Manthan Rajguru     YRS-AT-3665 - YRS enh: Data Corrections Tool - Admin screen option to create a manual credit 
//09/18/2017        Pramod P. Pokale    YRS-AT-3631 - YRS enh: Data Corrections Tool -Admin screen function to allow manual update of Fund status from WD or WP to TM or PT (TrackIT 30950) 
//09/22/2017        Sanjay GS Rawat     YRS-AT-3666 - YRS enh: Data Corrections Tool) -Reverse Annuity
//09/22/2017        Santosh Bura        YRS-AT-3541 -  YRS enh: Data Corrections Tool -Admin screen function to allow modification of RDB amounts (excessive data corrections) 
//09/22/2017        Sanjay GS Rawat     YRS-AT-3666 - YRS enh: Data Corrections Tool) -Reverse Annuity
//08/07/2017        Santosh Bura        YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
//12/14/2017        Santosh Bura        YRS-AT-3756 -  YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) 
//01/11/2018        Santosh Bura        YRS-AT-3324 -  YRS enh: Restrict withdrawals and death for 70 1/2 & older if RMDs not yet generated(TrackIT 28857) MORE DETAILS NEEDED
//04/03/2018        Sanjay GS Rawat     YRS-AT-3101 - YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//04/03/2018        Manthan Rajguru     YRS-AT-3929 -  YRS REPORTS: EFT: Loans ( FIRST EFT PROJECT) -create new "Loan Acknowledgement" Email for EFT/direct deposit 
//04/20/2018        Pramod P. Pokale    YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//06/21/2018        Vinayan C           YRS-AT-3190 -  YRS enh: add warning message for Loan Re-amortization (to prevent duplicate re-amortize efforts
//07/19/2018        Vinayan C           YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
//08/06/2018        Vinayan C           YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab 
//07/24/2018        Santosh Bura        YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)
//09/18/2018        Vinayan C           YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
//10/05/2018        Sanjay GS Rawat     YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
//11/16/2018        Vinayan C           YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
//11/21/2018        Benhan David        YRS-AT-4133 -  YRS enh: Annuity Estimate calculator & Goal Calculator: RDB Plan Rule change NEEDED by DECEMBER 2018 (TrackIT 32497) 
//11/19/2018        Benhan David        YRS-AT-4136 - YRS enh: Person Maintenance screen: RDB Plan Rule change NEEDED by DECEMBER 2018 (TrackIT 32497)
//11/26/2018        Benhan David        YRS-AT-3837 - YRS Enh: Death Benefit Application for RDB rule change NEEDED by DECEMBER 2018 (TrackIT 32497)
//01/09/2019        Megha Lad           YRS-AT-4244 - YRS enh: EFT Loans Maint. OND selection should display prior to disbursement (TrackIT 36462) 
//01/22/2019        Pooja K.            YRS-AT-2573 - YRS enh: add dropdown for first payment due date for Loan ReAmortization (TrackIT 23592) 
//01/15/2019        Megha Lad           YRS-AT-3157 - YRS enh-audit trail db table to track Loan freeze and unfreeze activity and copy emails to IDM (TrackIT 27438)
//01/31/2019        Pramod Pokale       YRS-AT-2920 - YRS enh-do not allow loan reamortizations if unfunded payments exist (TrackIT25480) synch issue 
//03/01/2019        Shilpa N            YRS-AT-4248 - YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
//19/09/2019        Pooja Kumkar        YRS-AT-4598 - YRS enh: State Withholding Project - Annuity Payroll Processing 
//19/09/2019        Pooja Kumkar        YRS-AT-4597 - YRS enh: State Withholding Project - First Annuity Payments (UI design)
//2019.12.12        Pooja Kumkar        YRS-AT-4676 -  State Withholding - Vaildations for exporting file from Payment Manager (First Annuities) 
//11/14/2019        Santosh Bura        YRS-AT-4599 -  YRS enh: State Tax Withholding - New import screen for "First Annuity Payroll Processing" (DAR File)
//01/06/2020        Sanjay Singh        YRS-AT-4602 - YRS enh:State Withholding Project - Export file Annuity Payroll 
//01/16/2020        Shilpa Nagargoje    YRS-AT-4641 - YRS REPORTS: State Tax Withholding- New screen for "Monthly Annuity Payroll Processing" (Reverse Feed) 
//01/23/2020        Pooja Kumkar        YRS-AT-4641 - YRS REPORTS: State Tax Withholding- New screen for "Monthly Annuity Payroll Processing" (Reverse Feed) 
//02/05/2020        Megha Lad           YRS-AT-4769 - YRS enhancement-new Secure Act rule for Annuity Estimate and Annuity Purchase screens (TrackIt- 41078)
//02/18/2020        Megha Lad           YRS-AT-4783 - YRS enhancement-modify Batch Estimates for Secure Act (TrackIt - 41132)
//04/10/2020        Shiny C.            YRS-AT-4852 -  COVID - YRS changes needed for LOANS due to CARE Act (Track IT - 41693) 
//06/05/2020        Pooja Kumkar        YRS-AT-4854 - Validation to check requested refund withdrawal is covid or not. 
//05/05/2020        suvarna B           YRS-AT-4854 -  COVID - Special withdrawal functionality needed due to CARE Act/COVID-19 (Track IT - 41688).
//05/05/2020        Shiny C.            YRS-AT-4874 -  COVID - Special withdrawal functionality needed in YRS processing screen due to CARE Act/COVID-19 (Track IT - 41688) 
//*******************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YMCAObjects
{
    public static class MetaMessageList
    {
        //Start: Bala: 01/19/2019: YRS-AT-2398: Message for special handling request.
        #region Maintenance – Person
        public static readonly int MESSAGE_PART_MAINT_SPECIAL_HANDLING = 1000;
        public static readonly int MESSAGE_PART_MAINT_INVAILD_RELATIONSHIP_CODE_BENEF = 1001;  // SB | 2017.12.14 | YRS-AT-3756 | Add message for adding beneficiary with relationship as spouse for already deceased spouse beneficiary      
        //START : BD : 11/19/2018 : YRS-AT-4136 - Person Maintenance screen: RDB Plan Rule change Messages
        public static readonly int MESSAGE_PART_MAINT_RETIREE_INSRES_NOT_ALLOWED = 1002;
        public static readonly int MESSAGE_PART_MAINT_RETIREE_NOT_ALLOWED = 1003;
        //END : BD : 11/19/2018 : YRS-AT-4136 - Person Maintenance screen: RDB Plan Rule change Messages

        //START: PK | 09/19/2019 | YRS-AT-4598 -  YRS enh: State Withholding Project - Annuity Payroll Processing 
        public static readonly int MESSAGE_STW_EXEMPTIONRANGE = 1010;
        public static readonly int MESSAGE_STW_FLATAMOUNTRANGE = 1011;
        public static readonly int MESSAGE_STW_STATEWITHHOLDINGAMOUNT = 1012;
        public static readonly int MESSAGE_STW_INVALID_STATE = 1013;
        public static readonly int MESSAGE_STW_INVALID_DISBURSEMENT = 1014; 
        public static readonly int MESSAGE_STW_INVALID_MARITALSTATUS = 1015;
        public static readonly int MESSAGE_STW_INVALID_WITHHOLDINGAMOUNTRANGE = 1016;   
        public static readonly int MESSAGE_UNSUPRESS_ADDRESS = 1017;
        public static readonly int MESSAGE_UNSUPRESS_PURCHASEDATE = 1018;
        public static readonly int MESSAGE_UNSUPRESS_FIRSTCHECK = 1019;
        public static readonly int MESSAGE_UNSUPRESS_STATEWITHHOLDING = 1020;
        public static readonly int MESSAGE_UNSUPRESS_FEDERALTAX = 1021;
        public static readonly int MESSAGE_UNSUPRESS_GENDER = 1022;
        public static readonly int MESSAGE_STW_INVALID_FEDERALAMOUNT_CA = 1023;
        public static readonly int MESSAGE_STW_REQUIRED_FEDERALAMOUNT_MA = 1024;
        public static readonly int MESSAGE_STW_REQUIRED_WITHHOLDING_MA = 1025;
        public static readonly int MESSAGE_STW_INVALID_FLATAMOUNT = 1026;
        //END: PK | 09/19/2019 | YRS-AT-4598 -  YRS enh: State Withholding Project - Annuity Payroll Processing 

        #endregion Maintenance – Person

        #region Withdrawal - Regular
        public static readonly int MESSAGE_WITHDRAWALS_SPECIAL_HANDLING = 3001;
        public static readonly int MESSAGE_WITHDRAWAL_INSUFFICIENT_BALANCE_FOR_FEE = 4003; // PPP | 11/18/2016 | YRS-AT-3146 | Message added to show error if there is no sufficient balance in those account-bucket who are defined in new fee charging hierarchy
        #endregion Withdrawal - Regular
        //End: Bala: 01/19/2019: YRS-AT-2398: Message for special handling request.

        #region EDI Exclusion List

        public static readonly int MESSAGE_EDI_ENTER_REASON = 20901;
        public static readonly int MESSAGE_EDI_ENTER_VALID_SSN_FUNDNO = 20902;
        public static readonly int MESSAGE_EDI_INVALID_SSN = 20903;
        public static readonly int MESSAGE_EDI_NOT_ELIGIBLE_FUNDNO = 20904;
        public static readonly int MESSAGE_EDI_NOT_ELIGIBLE_SSN = 20905;
        public static readonly int MESSAGE_EDI_REMOVE_PERSON = 20906;
        public static readonly int MESSAGE_EDI_SAVED_SUCCESSFULLY = 20907;
        public static readonly int MESSAGE_EDI_SEARCH_CRITERIA = 20908;
        public static readonly int MESSAGE_EDI_SSN_BLANK_EXISTS = 20909;
        public static readonly int MESSAGE_EDI_SSN_EXISTS = 20910;
        public static readonly int MESSAGE_EDI_SSN_NOT_EXISTS_IN_EDI = 20911;

        #endregion

        #region Meta Messages List

        public static readonly int MESSAGE_META_MESSAGES_ENTER_MESSAGEDESCRIPTION = 21001;
        public static readonly int MESSAGE_META_MESSAGES_MESSAGEDESCRIPTION_LENGTH = 21002;
        public static readonly int MESSAGE_META_MESSAGES_ENTER_MESSAGEDISPLAYTEXT = 21003;
        public static readonly int MESSAGE_META_MESSAGES_MESSAGEDISPLAYTEXT_LENGTH = 21004;
        public static readonly int MESSAGE_META_MESSAGES_SAVED_SUCCESSFULLY = 21005;
        public static readonly int MESSAGE_META_MESSAGES_EXCEPTION = 21006;
        public static readonly int MESSAGE_META_MESSAGES_DYNAMIC_PARAMETER_MODIFIED = 21007;
        public static readonly int MESSAGE_META_MESSAGES_SAVE_WITHOUT_MODIFIED_MESSAGE = 21008;
        public static readonly int MESSAGE_META_MESSAGES_RELOAD_MESSAGE = 21009;
        public static readonly int MESSAGE_META_MESSAGES_WAIVED_PARTICIPANT_MESSAGE = 21010; // Chandra sekar|2016.07.12 |YRS-AT-2772 
        #endregion

        #region "Disbursement"

        #region "Void Annuity"

        #region "Void Annuity - Transfer"

        public static readonly int MESSAGE_VTA_ARCHIVED_DATA = 13701;
        public static readonly int MESSAGE_VTA_CONFIRM_VOID_TRANSFER = 13702;
        public static readonly int MESSAGE_VTA_ENTER_BIRTH_DATE = 13703;
        public static readonly int MESSAGE_VTA_ENTER_FIRST_NAME = 13704;
        public static readonly int MESSAGE_VTA_ENTER_LAST_NAME = 13705;
        public static readonly int MESSAGE_VTA_FUTURE_BIRTH_DATE = 13706;
        public static readonly int MESSAGE_VTA_INVALID_EMAILID = 13707;
        public static readonly int MESSAGE_VTA_SAME_SSN = 13708;
        public static readonly int MESSAGE_VTA_SELECT_DISBURSEMENT = 13709;
        public static readonly int MESSAGE_VTA_SELECT_RETIREE = 13710;
        public static readonly int MESSAGE_VTA_SHOW_ENTER_VALUE = 13711;
        public static readonly int MESSAGE_VTA_SHOW_TOTAL_RECORDS = 13712;
        public static readonly int MESSAGE_VTA_SSN_EXIST = 13713;
        public static readonly int MESSAGE_VTA_TELEPHONE_LENGTH = 13714;
        public static readonly int MESSAGE_VTA_TRANSFERED_SUCCESSFULLY = 13715;
        public static readonly int MESSAGE_VTA_VALID_SSN = 13716;
        public static readonly int MESSAGE_VTA_SEARCH_LONGER_TIME_VALIDATION = 13717;
        #endregion

        #endregion

        #endregion

        #region Loan

        #region Loan Processing

        public static readonly int MESSAGE_LOAN_AMOUNT_NEGATIVE = 9001;
        public static readonly int MESSAGE_LOAN_AMOUNT_INVALID = 9002;
        public static readonly int MESSAGE_LOAN_CANNOT_CANCEL_AMART_LOAN = 9003;
        public static readonly int MESSAGE_LOAN_CANNOT_CANCEL_TERM_LOAN = 9004;
        public static readonly int MESSAGE_LOAN_CANNOT_CANCEL_ALREADY_CANCELED = 9005;
        public static readonly int MESSAGE_LOAN_CANNOT_CANCEL_EXPIRED = 9006;
        public static readonly int MESSAGE_LOAN_CANNOT_CANCEL_UNSUSPENDED = 9007;
        public static readonly int MESSAGE_LOAN_CANNOT_CANCEL_MULTIPLE_LOANS = 9008;
        public static readonly int MESSAGE_LOAN_CANNOT_PROCESS_DETAILS_MISSING = 9009;
        public static readonly int MESSAGE_LOAN_CANNOT_PROCESS_ONLY_PENDING_NOTEXISTS = 9010;
        public static readonly int MESSAGE_LOAN_CANNOT_CANCEL_SUSPENDED = 9011;
        public static readonly int MESSAGE_LOAN_CANNOT_CANCEL_DEFAULTED_CLOSED_PAIDOFF = 9012;
        public static readonly int MESSAGE_LOAN_CANNOT_ADD_NO_BALANCE = 9013;
        public static readonly int MESSAGE_LOAN_CANNOT_ADD_BALANCE_LESS_THAN_2000 = 9014;
        public static readonly int MESSAGE_LOAN_CANNOT_PROCESS_MULTIPLE_LOANS_EXISTS = 9015;
        public static readonly int MESSAGE_LOAN_NET_AMOUNT_LESSTHAN_ZERO = 9016;
        public static readonly int MESSAGE_LOAN_PERSON_SELECTION_IS_LOST = 9017;
        public static readonly int MESSAGE_LOAN_PARTICIPANT_ACCOUNT_IS_LOCKED_DUE_TO = 9018;
        public static readonly int MESSAGE_LOAN_PARTICIPANT_ACCOUNT_IS_LOCKED = 9019;
        public static readonly int MESSAGE_LOAN_PROCESS_UNSUCCESFULL = 9020;
        public static readonly int MESSAGE_LOAN_REVERSE_PAID_LOAN = 9021;
        public static readonly int MESSAGE_LOAN_PARTICIPANT_HAS_QDRO_REQUEST = 9022;
        public static readonly int MESSAGE_LOAN_TRANSACTION_REF_MISSING_FOR_RT = 9023;
        public static readonly int MESSAGE_LOAN_PROCESS_PARTICIPANT_HAS_NO_ACTIVE_EMPLOYEMENT = 9024;
        public static readonly int MESSAGE_LOAN_EMAIL_SENT_PARTICIPANT = 9025; //PPP | 10/20/2018 | YRS-AT-3101 | Renamed code from MESSAGE_LOAN_EMAIL_SENT to MESSAGE_LOAN_EMAIL_SENT_PARTICIPANT, made it specific for Participant
        public static readonly int MESSAGE_LOAN_EMAIL_MISSING_PARTICIPANT = 9026; //PPP | 10/20/2018 | YRS-AT-3101 | Renamed code from MESSAGE_LOAN_EMAIL_MISSING to MESSAGE_LOAN_EMAIL_MISSING_PARTICIPANT, made it specific for Participant
        public static readonly int MESSAGE_LOAN_EMAIL_ERROR_PARTICIPANT = 9027; //PPP | 10/20/2018 | YRS-AT-3101 | Renamed code from MESSAGE_LOAN_EMAIL_ERROR to MESSAGE_LOAN_EMAIL_ERROR_PARTICIPANT, made it specific for Participant
        public static readonly int MESSAGE_LOAN_PROCESSED_SUCCESFULLY = 9028;
        public static readonly int MESSAGE_LOAN_CANCELLED = 9029;
        public static readonly int MESSAGE_LOAN_CONFIRM_CANCEL = 9030;
        //START: MMR | 2018.03.04 | YRS-AT-3929 | Added messages for email to participant during loan processing with payment mode as EFT and for disbursed loan cancelled with EFT Status as PROOF
        public static readonly int MESSAGE_LOAN_CANNOT_CANCEL_EFT_PENDING = 9031;//9034;
        public static readonly int MESSAGE_LOAN_INVALID_PAYMENT_METHOD = 9032;//9035;
        public static readonly int MESSAGE_LOAN_EMAIL_COPY_NOT_SENT_ERROR = 9033;//9037;
        public static readonly int MESSAGE_LOAN_COOLING_PERIOD_ERROR = 9035;
        //END: MMR | 2018.03.04 | YRS-AT-3929 | Added messages for email to participant during loan processing with payment mode as EFT and for disbursed loan cancelled with EFT Status as PROOF
        //START: PPP | 10/20/2018 | YRS-AT-3101 | Added new messages for notes and email
        public static readonly int MESSAGE_LOAN_CHECK_DISBURSE_NOTES = 9036;
        public static readonly int MESSAGE_LOAN_EFT_DISBURSE_NOTES = 9037;
        public static readonly int MESSAGE_LOAN_DMWEBSERVICE_OFFLINE = 9038;
        public static readonly int MESSAGE_LOAN_PDF_FAILED = 9039;
        public static readonly int MESSAGE_LOAN_EMAIL_MISSING_YMCA = 9040;
        public static readonly int MESSAGE_LOAN_EMAIL_ERROR_YMCA = 9041;
        public static readonly int MESSAGE_LOAN_EMAIL_SENT_YMCA = 9042;
        public static readonly int MESSAGE_LOAN_EMAIL_SENT = 9044;
        public static readonly int MESSAGE_LOAN_EMAIL_ERROR = 9045;
        //END: PPP | 10/20/2018 | YRS-AT-3101 | Added new messages for notes and email
        #endregion

        #region Loan Maintenance

        public static readonly int MESSAGE_LOAN_CAN_BE_DEFAULT_PAID_UNSUSPENDED = 9501;
        public static readonly int MESSAGE_LOAN_SUSPENDED_NOT_OTHERTHAN_PAID = 9502;
        public static readonly int MESSAGE_LOAN_CLOSED_NOT_OTHERTHAN_PAID_SUSPENDED = 9503;
        public static readonly int MESSAGE_LOAN_CANNOT_PROCEED_AS_DISCHARGEDATE_ISGREATERTHAN_ONEMONTH = 9504;
        public static readonly int MESSAGE_LOAN_CANNOT_TERMINATE_LOAN_ORIGINAL_IS_INVALID = 9505;
        public static readonly int MESSAGE_LOAN_CANNOT_TERMINATE_NO_ACTIVE_EMPLOYEMENT_EXISTS = 9506;
        public static readonly int MESSAGE_LOAN_CANNOT_UNSUSPENDED_NO_ACTIVE_EMPLOYEMENT_EXISTS = 9507;
        public static readonly int MESSAGE_LOAN_DISCHARGE_DATE_IS_REQUIRED_REAMORTIZING = 9508;
        public static readonly int MESSAGE_LOAN_CANNOT_CLOSED_BECAUSE = 9509;
        public static readonly int MESSAGE_LOAN_CLOSED_DATE_NOT_LATER_FINAL_DATE = 9510;
        public static readonly int MESSAGE_LOAN_UNSUSPEND_DATE_NOT_LATER_FINAL_DATE = 9511;
        public static readonly int MESSAGE_LOAN_FINAL_PAYMENT_IS_PASSED = 9512;
        public static readonly int MESSAGE_LOAN_FINAL_PAYMENT_IS_MISSING = 9513;
        public static readonly int MESSAGE_LOAN_DETAILS_ID_MISSING = 9514;
        public static readonly int MESSAGE_LOAN_CANNOT_BE_DEFAULTED_VERIFY_RECORDS = 9515;
        // Start | SC | 2020.04.10 | YRS-AT-4852 | Modified the suspend document validation text
        //public static readonly int MESSAGE_LOAN_CANNOT_BE_SUSPENDED_NO_MILATARY_DOC = 9516;
        public static readonly int MESSAGE_LOAN_CANNOT_BE_SUSPENDED_NO_SUPPORTED_DOC = 9516;
        // Start | SC | 2020.04.10 | YRS-AT-4852 | Modified the suspend document validation text
        public static readonly int MESSAGE_LOAN_NOT_SUSPENDED = 9517;
        public static readonly int MESSAGE_LOAN_PAYOFF_BE_DONE = 9518;
        public static readonly int MESSAGE_LOAN_PAYOFF_CANNOT_AS_LOAN_CLOSED = 9519;
        public static readonly int MESSAGE_LOAN_PAYOFF_CANNOT_AS_LOAN_DEFAULTED = 9520;
        public static readonly int MESSAGE_LOAN_PAYOFF_CANNOT_AS_LOAN_UNSUSPEND = 9521;
        public static readonly int MESSAGE_LOAN_PAYOFF_CANNOT_AS_LOAN_PAYOFF = 9522;
        public static readonly int MESSAGE_LOAN_PAYOFF_VERIFY_AMORT_RECORDS = 9523;
        public static readonly int MESSAGE_LOAN_PAYOFF_CAN_BE_PAID = 9524;
        public static readonly int MESSAGE_LOAN_DEFAULT = 9525;
        public static readonly int MESSAGE_LOAN_SUSPEND = 9526;
        public static readonly int MESSAGE_LOAN_TERMINATED = 9527;
        public static readonly int MESSAGE_LOAN_UNSUSPENDED = 9528;
        public static readonly int MESSAGE_LOAN_CLOSED_ALREADY_CLOSED = 9529;
        public static readonly int MESSAGE_LOAN_CLOSED_SUSPENDED = 9530;
        public static readonly int MESSAGE_LOAN_CLOSED_UNSUSPENDED = 9531;
        public static readonly int MESSAGE_LOAN_CLOSED_DEFAULTED = 9532;
        public static readonly int MESSAGE_LOAN_CLOSED_PAYOFF = 9533;
        public static readonly int MESSAGE_LOAN_DEFAULTED_ALREADY_DEFAULTED = 9534;
        public static readonly int MESSAGE_LOAN_DEFAULTED_CLOSED = 9535;
        public static readonly int MESSAGE_LOAN_DEFAULTED_UNSUSPEND = 9536;
        public static readonly int MESSAGE_LOAN_DEFAULTED_PAYOFF = 9537;
        public static readonly int MESSAGE_LOAN_SUSPENDED_ALREADY_SUSPENDED = 9538;
        public static readonly int MESSAGE_LOAN_SUSPENDED_CLOSED = 9539;
        public static readonly int MESSAGE_LOAN_SUSPENDED_UNSUSPENDED = 9540;
        public static readonly int MESSAGE_LOAN_SUSPENDED_DEFAULTED = 9541;
        public static readonly int MESSAGE_LOAN_SUSPENDED_PAYOFF = 9542;
        public static readonly int MESSAGE_LOAN_SUSPENDED_VERIFY_TERM_REASON = 9543;
        public static readonly int MESSAGE_LOAN_UNSUSPENDED_ALREADY_UNSUSPENDED = 9544;
        public static readonly int MESSAGE_LOAN_UNSUSPENDED_CLOSED = 9545;
        public static readonly int MESSAGE_LOAN_UNSUSPENDED_DEFAULTED = 9546;
        public static readonly int MESSAGE_LOAN_UNSUSPENDED_PAYOFF = 9547;
        public static readonly int MESSAGE_LOAN_UNSUSPENDED_BECAUSE = 9548;
        public static readonly int MESSAGE_LOAN_EMAIL_TO_YMCA_ERROR = 9549;
        public static readonly int MESSAGE_LOAN_YMCA_EMAIL_MISSING = 9550;
        public static readonly int MESSAGE_LOAN_PARTICIPANT_EMAIL_MISSING = 9551;
        public static readonly int MESSAGE_LOAN_EMAIL_TO_PARTICIPANT_ERROR = 9552;
        public static readonly int MESSAGE_LOAN_UNFUNDED_PAYMENT_EXISTS_DEFAULTED = 9553;
        public static readonly int MESSAGE_LOAN_START_DATE_MISSING = 9554;
        public static readonly int MESSAGE_LOAN_CANNOT_BE_DEFAULTED = 9555;
        public static readonly int MESSAGE_LOAN_CONFIRM_DEFAULT = 9556;
        public static readonly int MESSAGE_LOAN_CONFIRM_PAYOFF = 9557;
        public static readonly int MESSAGE_LOAN_CONFIRM_REAMORTIZE = 9558;
        public static readonly int MESSAGE_LOAN_CONFIRM_SUSPEND = 9559;
        public static readonly int MESSAGE_LOAN_CONFIRM_UNSUSPEND = 9560;
        public static readonly int MESSAGE_LOAN_CONFIRM_DISCHARGE_DATE_AHEAD = 9561;
        public static readonly int MESSAGE_LOAN_HAS_CLOSED = 9562;
        public static readonly int MESSAGE_LOAN_HAS_SUSPENDED = 9563;
        public static readonly int MESSAGE_LOAN_HAS_UNSUSPENDED = 9564;
        public static readonly int MESSAGE_LOAN_HAS_TERMINATED_NEW_AMORTIZED = 9565;
        public static readonly int MESSAGE_LOAN_EMAIL_SENT_TO_YMCA = 9566;
        public static readonly int MESSAGE_LOAN_EMAIL_SENT_TO_PARTICIAPANT = 9567;
        public static readonly int MESSAGE_LOAN_HAS_DEFAULTED = 9568;       
        //Start: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added for offset loan tab messages
        public static readonly int MESSAGE_LOAN_OFFSET = 9569;
        public static readonly int MESSAGE_LOAN_CONFIRM_OFFSET = 9570;
        public static readonly int MESSAGE_LOAN_HAS_OFFSET = 9571;
        public static readonly int MESSAGE_LOAN_OFFSET_AGE = 9572;
        public static readonly int MESSAGE_LOAN_OFFSET_TERMINATED = 9573;
        public static readonly int MESSAGE_LOAN_OFFSET_RT = 9574;
        //End: AA:2015.03.12 BT:2699:YRS 5.0-2441 : Added for offset loan tab messages 
        public static readonly int MESSAGE_LOAN_OFFSET_DEATH = 9575; //Manthan Rajguru | 2015.11.10 | YRS-AT-2669 | Added for offset loan tab messages
        //Start: AA:2015.07.12 YRS 5.0-2441 
        public static readonly int MESSAGE_LOAN_PAYOFF_UNFUNDED = 9576;
        public static readonly int MESSAGE_LOAN_OFFSET_RT_PORTION = 9576;
        //End: AA:2015.07.12 YRS 5.0-2441 
        public static readonly int MESSAGE_LOAN_CANNOT_TERMINATE_NO_ACTIVE_YMCA_SELECTION = 9578;//Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Added for Terminate / Re-amortize Loan tab messages
        public static readonly int MESSAGE_LOAN_CONFIRM_REAMORTIZATION = 9579; // VC | 2018.06.21 | YRS-AT-3190 |  Added to display recent loan reamortized message in confirmation box
        public static readonly int MESSAGE_LOAN_CANNOT_BE_SUSPENDED_NO_REASON_SELECTED = 9592; // SC | 2020.04.10 | YRS-AT-4852 | Added the suspend reason validation text if not entered
        #endregion
        //START : SB | 2018.07.24 | YRS-AT-4017 | Messages to display in Loan Admin screen for unauthorized access, invalid prime rate interest and successfully updated prime interest rate
        #region "Loan Admin"
        public static readonly int MESSAGE_LOAN_NOT_SELECTED_DISBURSEMENT = 9701;
        public static readonly int MESSAGE_LOAN_INTEREST_RATE_SUCCESSFUL = 9702;
        public static readonly int MESSAGE_LOAN_INTEREST_RANGE = 9703;
        #endregion
        //END : SB | 2018.07.24 | YRS-AT-4017 | Messages to display in Loan Admin screen for unauthorized access, invalid prime rate interest and successfully updated prime interest rate

        //START:2015.04.27      Dinesh Kanojia         BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
        #region Loan Utility
        public static readonly int MESSAGE_LOAN_CONFIRM_UNFREEZED = 9601;
        public static readonly int MESSAGE_LOAN_HAS_UNFREEZED = 9602;        
        public static readonly int MESSAGE_LOAN_HAS_UNFREEZED_EMAIL_ERROR = 9603;
        public static readonly int MESSAGE_LOAN_CONFIRM_PRINT_REPORT = 9604; //2015.01.18 AA :YRS-AT-2662 - Added for auto-offset tab  
        public static readonly int MESSAGE_LOAN_GENERATEYMCALINK_HIDE = 9605; //2016.07.05 AA:YRS-AT-2830 - Added for the auto closed tab to describe that the link was hidden
        #endregion
        //END:2015.04.27      Dinesh Kanojia         BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
        #region Loan Admin
        public static readonly int MESSAGE_LOAN_ADMIN_SEARCH_CRITERIA_MISSING = 9704;//2018.07.19  VC:YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
        public static readonly int MESSAGE_LOAN_ADMIN_LOAN_RATE_NOT_CHANGED = 9729;//VC | 2018.11.14 | YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
        public static readonly int MESSAGE_LOAN_ADMIN_DONT_PROCESS_ON_LAST_BUSINS_DAY = 9731;//VC | 2018.11.22 | YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
        public static readonly int MESSAGE_LOAN_ADMIN_LOAN_DISBURSE_CONFIRMATION = 9732;//VC | 2018.11.22 | YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
        //START: VC | 2018.08.06 | YRS-AT-4018 | Messages to display in person maintenance screen while Approving/Declining loan
        public static readonly int MESSAGE_LOAN_ADMIN_CONFIRM_APPROVE_LOAN = 9714;
        public static readonly int MESSAGE_LOAN_ADMIN_PARTICIPANT_EMAIL_MISSING = 9715;
        public static readonly int MESSAGE_LOAN_ADMIN_PARTICIPANT_EMAIL_SENT = 9716;
        public static readonly int MESSAGE_LOAN_ADMIN_PARTICIPANT_EMAIL_SENT_ERROR = 9717;
        public static readonly int MESSAGE_LOAN_ADMIN_LOAN_APPROVE_SUCCESS = 9718;
        public static readonly int MESSAGE_LOAN_ADMIN_LOAN_DECLINE_SUCCESS = 9719;
        public static readonly int MESSAGE_LOAN_ADMIN_LOAN_PROCESS_IN_PROGRESS = 9728;
        //END: VC | 2018.08.06 | YRS-AT-4018 | Messages to display in person maintenance screen while Approving/Declining loan
        #endregion
        #endregion

        #region RollIn

        #region RollIn_Process
            public static readonly int MESSAGE_ROLLIN_ACTIVE_FUNDEVENT_NOT_FOUND = 15101;
            public static readonly int MESSAGE_ROLLIN_CANCEL_CANCELLED_ROLLIN = 15102;
            public static readonly int MESSAGE_ROLLIN_CANCEL_CLOSED_ROLLIN = 15103;
            public static readonly int MESSAGE_ROLLIN_CHECK_DATE_CANT_BE_BLANK = 15104;
            public static readonly int MESSAGE_ROLLIN_CHECK_DATE_CANT_BE_GRTR_THAN_TODAY = 15105;
            public static readonly int MESSAGE_ROLLIN_CHECK_DATE_LESS_RCVD_DATE = 15106;
            public static readonly int MESSAGE_ROLLIN_CHECK_NO_CANT_BE_BLANK = 15107;
            public static readonly int MESSAGE_ROLLIN_CHECK_NO_SAME = 15108;
            public static readonly int MESSAGE_ROLLIN_CHECK_RCVD_CANT_BE_BLANK = 15109;
            public static readonly int MESSAGE_ROLLIN_CHECKDATE_ATLST_SIX_OLD = 15110;
            public static readonly int MESSAGE_ROLLIN_CHECKRCVD_DATE_CANT_BE_GRTR_THAN_TODAY = 15111;
            public static readonly int MESSAGE_ROLLIN_CHECKRCVD_DATE_NOT_FIRSTDAY_MONTH = 15112;
            public static readonly int MESSAGE_ROLLIN_CHECKRCVDDATE_ATLST_SIX_OLD = 15113;
            public static readonly int MESSAGE_ROLLIN_CHECKRCVDDATE_IS_EARLIER_RCVDDATE = 15114;
            public static readonly int MESSAGE_ROLLIN_CHECKTOTAL_CANT_BE_ZERO = 15115;
            public static readonly int MESSAGE_ROLLIN_INST_CANT_BE_BLANK = 15116;
            public static readonly int MESSAGE_ROLLIN_NON_TAXAMNT_CANT_BE_BLANK = 15117;
            public static readonly int MESSAGE_ROLLIN_PROCESS_CLOSED_ROLLIN = 15118;
            public static readonly int MESSAGE_ROLLIN_RCVD_ATLST_SIX_OLD = 15119;
            public static readonly int MESSAGE_ROLLIN_RCVD_CANT_BE_BLANK = 15120;
            public static readonly int MESSAGE_ROLLIN_RCVD_GRTR_THAN_TODAY = 15121;
            public static readonly int MESSAGE_ROLLIN_SELECT_INFO_SOURCE = 15122;
            public static readonly int MESSAGE_ROLLIN_TAXAMNT_CANT_BE_BLANK = 15123;
            public static readonly int MESSAGE_ROLLIN_CANCELL_ROLLIN = 15124;
            public static readonly int MESSAGE_ROLLIN_SAVING_REQUEST = 15125;
            public static readonly int MESSAGE_ROLLIN_SAVING_RECEIPT = 15126;
            public static readonly int MESSAGE_ROLLIN_CANCEL_CONFIRMATION = 15127;
            public static readonly int MESSAGE_ROLLIN_CREATION_CONFIRMATION = 15128;
            public static readonly int MESSAGE_ROLLIN_PROCESS_CONFIRMATION = 15129;
        #endregion
        #region RollIn_Remainder
            public static readonly int MESSAGE_ROLLIN_REMINDER_NO_HISTORY = 15301;
            public static readonly int MESSAGE_ROLLIN_REMINDER_SELECT_RECORD = 15302;
            public static readonly int MESSAGE_ROLLIN_LIST_ROLLIN_FROM_LAST_DAYS = 15303;
            public static readonly int MESSAGE_ROLLIN_LIST_ROLLIN_FROM_LAST_MONTHS = 15304;
            public static readonly int MESSAGE_ROLLIN_REMINDER_CONFIRM_PRINT = 15305;

        #endregion

        #endregion
//Start: BJ:BT:2570 YMCA 5.0-2380 - 2015-04-23
#region "Annuity Beneficiary"
        //12801-13000
        #region "Annuity Beneficiary Death"
            public static readonly int MESSAGE_ANN_BENE_DEATH_DEATHDATE_BLANK = 12801;
            public static readonly int MESSAGE_ANN_BENE_DEATH_DEATHDATE_LATER_THAN_ANN_PURCHASE_DATE = 12802;
            public static readonly int MESSAGE_ANN_BENE_DEATH_DEATHDATE_NOT_FUTURE = 12803;
            public static readonly int MESSAGE_ANN_BENE_DEATH_DEATHDATE_OVER_6MONTHS_AGO_CONFIRMATION = 12804;
            public static readonly int MESSAGE_ANN_BENE_DEATH_DEATHDATE_BEFORE_DOC_RECEIVED = 12805;
            public static readonly int MESSAGE_ANN_BENE_DEATH_CONFIRM_SAVE = 12806;
            public static readonly int MESSAGE_ANN_BENE_DEATH_DUE_SINCE = 12807;
            public static readonly int MESSAGE_ANN_BENE_DEATH_DEATH_NOTIFICATION_COMPLETED = 12808;
            public static readonly int MESSAGE_ANN_BENE_DEATH_INITIAL_COMM_LETTER_PRINTED_SUCCESSFULLY = 12809;
            public static readonly int MESSAGE_ANN_BENE_DEATH_DEATH_CERTIFICATE_RECEIVED = 12810;
            public static readonly int MESSAGE_ANN_BENE_DEATH_DEATHDATE_LATER_THAN_BIRTH_DATE = 12811;   // SR : 2018.06.29 | YRS-AT-3804 | Added message code to validate annuity beneficiary death date earlier than birth date.


        #endregion

        #region "Annuity Beneficiary Follow-up"
            public static readonly int MESSAGE_ANN_BENE_DEATH_FOLLOWUP_CONFIRM_PRINT = 12901;
            public static readonly int MESSAGE_ANN_BENE_DEATH_FOLLOWUP_SELECT_RECORD = 12902;
            public static readonly int MESSAGE_ANN_BENE_DEATH_FOLLOWUP_CONFIRM_SAVE = 12903;
            public static readonly int MESSAGE_ANN_BENE_DEATH_FOLLOWUP_SAVED_SUCCESSFULLY = 12904;
            public static readonly int MESSAGE_ANN_BENE_DEATH_FOLLOWUP_60DAY_FOLLOWUP_PRINTED_SUCCESSFULLY = 12905;
            public static readonly int MESSAGE_ANN_BENE_DEATH_FOLLOWUP_90DAY_FOLLOWUP_PRINTED_SUCCESSFULLY = 12906;
            public static readonly int MESSAGE_ANN_BENE_DEATH_FOLLOWUP_CHECK_UNCHECK_SAVE_CHANGES = 12907;
        #endregion
#endregion
//End: BJ:BT:2570 YMCA 5.0-2380 -

        //START: PPP | 2015.10.10 | YRS-AT-2588 | Created messages at dbo.AtsMetaMessages for telephone
        #region "General"
        public static readonly int MESSAGE_GEN_OFFICE_TELEPHONE_ERROR = 30001;
        public static readonly int MESSAGE_GEN_HOME_TELEPHONE_ERROR = 30002;
        public static readonly int MESSAGE_GEN_MOBILE_TELEPHONE_ERROR = 30003;
        public static readonly int MESSAGE_GEN_FAX_TELEPHONE_ERROR = 30004;
        public static readonly int MESSAGE_GEN_WORK_TELEPHONE_ERROR = 30005;
        public static readonly int MESSAGE_GEN_TELEPHONE_ERROR = 30006;
        public static readonly int MESSAGE_GEN_PHONENUMBER_ERROR = 30007;
        //Start: AA : 2016.02.18 YRS-At-2640 Created message for the landing page enhancement
        public static readonly int MESSAGE_GEN_LANDINGPAGE_INVALID_DATATYPE_VALUE = 30008;
        public static readonly int MESSAGE_GEN_LANDINGPAGE_INVALID_DATATYPE_FOR_PERSONMAINTENANCE = 30009;
        public static readonly int MESSAGE_GEN_LANDINGPAGE_INVALID_DATATYPE_FOR_YMCAMAINTENANCE = 30010;
        public static readonly int MESSAGE_GEN_LANDINGPAGE_INVALID_DATATYPE_FOR_WITHDRAWALPROCESSSING = 30011;
        public static readonly int MESSAGE_GEN_LANDINGPAGE_INVALID_PAGETYPE = 30012;
        public static readonly int MESSAGE_GEN_LANDINGPAGE_INVALID_REFREQUESTID_VALUE = 30013;
        public static readonly int MESSAGE_GEN_LANDINGPAGE_NONPEND_REFREQUESTID_VALUE = 30014;
        public static readonly int MESSAGE_GEN_LANDINGPAGE_ERROR = 30015;
        public static readonly int MESSAGE_GEN_LANDINGPAGE_PARAMETER_FORMAT = 30016;
        //End: AA : 2016.02.18 YRS-At-2640 Created message for the landing page enhancement
        #endregion "General"
        //END: PPP | 2015.10.10 | YRS-AT-2588 | Created messages at dbo.AtsMetaMessages for telephone

        //START: PPP | 2016.01.29 | YRS-AT-2594 | Created messages at dbo.AtsMetaMessages for unfunded UEIN email generation page
        #region "Unfunded UEIN"
        public static readonly int MESSAGE_UEIN_CONFIRMATION = 17701;
        public static readonly int MESSAGE_UEIN_MAIL_SENT_SUCCESS = 17702;
        public static readonly int MESSAGE_UEIN_MAIL_SENT_ERROR = 17703;
        public static readonly int MESSAGE_UEIN_CHECKBOX_VALIDATION_ERROR = 17704;
        public static readonly int MESSAGE_UEIN_MAIL_COPY_NOT_SENT_ERROR = 17705; //Manthan | 2016.05.05 | YRS-AT-2594 | Created message at dbo.AtsMetaMessages foir mail copy not sent to IDM
        #endregion "Unfunded UEIN"
        //END: PPP | 2016.01.29 | YRS-AT-2594 | Created messages at dbo.AtsMetaMessages for unfunded UEIN email generation page

        //START - Manthan Rajguru | 2016.02.03 | YRS-AT-2669 | Added for YMCA-Maintenance W/T/S/M tab messages
        #region YMCA Maintenance
        public static readonly int MESSAGE_YMCA_WITHDRAWAL_DATE_ERROR = 2001;
        public static readonly int MESSAGE_YMCA_SUSPENSION_DATE_ERROR = 2002;
        public static readonly int MESSAGE_YMCA_SELECTION_MERGING_ERROR = 2003;
        public static readonly int MESSAGE_YMCA_REACTIVATION_DATE_ERROR = 2004;
        public static readonly int MESSAGE_YMCA_PAYROLL_DATE_ERROR = 2005;
        public static readonly int MESSAGE_YMCA_PAYROLL_GREATERTHAN_RESOLUTION_DATE_ERROR = 2006;
        public static readonly int MESSAGE_YMCA_SELECTION_YERDIACCESS_ERROR = 2007;
        public static readonly int MESSAGE_YMCA_MERGE_DATE_ERROR = 2008;
        public static readonly int MESSAGE_YMCA_DATE_MORETHAN_SIXMONTHS_PAST_ERROR = 2009;
        public static readonly int MESSAGE_YMCA_DATE_MORETHAN_SIXMONTHS_FUTURE_ERROR = 2010;
        public static readonly int MESSAGE_YMCA_TERMINATIONDATE_MORETHAN_WITHDRAWALDATE_ERROR = 2011;
        public static readonly int MESSAGE_YMCA_EMAILADDRESS_FINANCEDEPT_NOTFOUND_ERROR = 2012;
        public static readonly int MESSAGE_YMCA_EMAILADDRESS_TDLOANS_NOTFOUND_ERROR = 2013;
        public static readonly int MESSAGE_YMCA_CONFIRM_DIALOG_MESSAGE_TABCHANGE = 2014;
        public static readonly int MESSAGE_YMCA_SUSPEND_WITHDRAWN = 2015;
        #endregion YMCA Maintenance
        //END - Manthan Rajguru | 2016.02.03 | YRS-AT-2669 | Added for YMCA-Maintenance W/T/S/M tab messages

        //START: PPP | 09/14/2017 | YRS-AT-3665 | Created messages at dbo.AtsMetaMessages for DC Tools 
        #region DC Tools
        public static readonly int MESSAGE_DCTOOLS_DEATHBENEFIT_AMOUNT = 21601;
        public static readonly int MESSAGE_DCTOOLS_DEATHBENEFIT_DATE = 21602;
        public static readonly int MESSAGE_DCTOOLS_DEATHBENEFIT_SUCCESS = 21603;
        //START: SB | 09/22/2017 | YRS-AT-3541 | validation message added for Edit Remaining Death Benefit amount screen
        public static readonly int MESSAGE_DCTOOLS_DEATHBENEFIT_NOTES = 21604;
        public static readonly int MESSAGE_DCTOOLS_DEATHBENEFIT_TRANSACTDATE_FUTURE = 21605;
        public static readonly int MESSAGE_DCTOOLS_DEATHBENEFIT_MISMATCH_ERROR = 21606;
        public static readonly int MESSAGE_DCTOOLS_DEATHBENEFIT_LINKAGE_MISSING_ERROR = 21607;
        public static readonly int MESSAGE_DCTOOLS_DEATHBENEFIT_UNCHANGEDRDB_ERROR = 21608;
        public static readonly int MESSAGE_DCTOOLS_DEATHBENEFIT_TRANSACTDATE_EARLIER = 21609;
        //END: SB | 09/22/2017 | YRS-AT-3541 | validation message added for Edit Remaining Death Benefit amount screen

        public static readonly int MESSAGE_DCTOOLS_YMCACREDITS_AMOUNT = 21651;
        public static readonly int MESSAGE_DCTOOLS_YMCACREDITS_RECEIVEDDATE = 21652;
        public static readonly int MESSAGE_DCTOOLS_YMCACREDITS_SUCCESS = 21653;
        //START: MMR | 09/20/2017 | YRS-AT-3665 | validation message added for Add YMCA Credit screen
        public static readonly int MESSAGE_DCTOOLS_YMCACREDITS_TOTALCREDITAMOUNT = 21654; 
        public static readonly int MESSAGE_DCTOOLS_YMCACREDITS_RECEIVEDDATE_FUTURE = 21655;
        public static readonly int MESSAGE_DCTOOLS_YMCACREDITS_NOTES = 21656;
        //END: MMR | 09/20/2017 | YRS-AT-3665 | validation message added for Add YMCA Credit screen

        public static readonly int MESSAGE_DCTOOLS_REVERSEANNUITY_SELECT = 21701;
        public static readonly int MESSAGE_DCTOOLS_REVERSEANNUITY_CHECK_VOID = 21702;
        public static readonly int MESSAGE_DCTOOLS_REVERSEANNUITY_QDRO = 21703;
        public static readonly int MESSAGE_DCTOOLS_REVERSEANNUITY_SUCCESS = 21704;
        public static readonly int MESSAGE_DCTOOLS_REVERSEANNUITY_NOTES = 21705; // 09/22/2017  SR | YRS-AT-3666 - Add message for Annuity compulsion.
        public static readonly int MESSAGE_DCTOOLS_REVERSEANNUITY_EXPERIENCE_DIVIDEND = 21706; // 09/27/2017  SR | YRS-AT-3666 - Add message for experience devidend warning

        //START: PPP | 09/18/2017 | YRS-AT-3631 | Created messages at dbo.AtsMetaMessages for Change Fund Status
        public static readonly int MESSAGE_DCTOOLS_CHANGEFUNDSTATUS_SELECT = 21751;
        public static readonly int MESSAGE_DCTOOLS_CHANGEFUNDSTATUS_SUCCESS = 21752;
        public static readonly int MESSAGE_DCTOOLS_CHANGEFUNDSTATUS_UPDATE = 21753;
        public static readonly int MESSAGE_DCTOOLS_CHANGEFUNDSTATUS_NOTES = 21754;
        //END: PPP | 09/18/2017 | YRS-AT-3631 | Created messages at dbo.AtsMetaMessages for Change Fund Status
        #endregion DC Tools
        //END: PPP | 09/14/2017 | YRS-AT-3665 | Created messages at dbo.AtsMetaMessages for DC Tools 

        //START: SB | 2017.08.07 | YRS-AT-3324 |  ErrorCode to display message to restrict prcoess in Withdrawal, Death Notification & Death Settlement
        #region RMD Generate/Regenerate
        public static readonly int MESSAGE_DEATH_NOTIFICATION_REGEN_RMD_ERR = 18001;
        public static readonly int MESSAGE_WITHDRAWALS_REGEN_RMD_ERR = 18002;
        public static readonly int MESSAGE_WITHDRAWALS_REGEN_RMD_RETRO_TERM_ERR = 18003;
        public static readonly int MESSAGE_DEATHSETTLEMENT_REGEN_RMD_ERR = 18004;
        public static readonly int MESSAGE_DEATHSETTLEMENT_REGEN_RMD_RETRO_TERM_ERR = 18005;
        public static readonly int MESSAGE_DEATHNOTIFICATION_REGEN_RMD_RETRO_TERM_ERR = 18006;
        public static readonly int MESSAGE_WITHDRAWAL_NONELIGIBLE_RMDEXIST_ERR = 18007;
        public static readonly int MESSAGE_DEATHNOTIFICATION_NONELIGIBLE_RMDEXIST_ERR = 18008;
        public static readonly int MESSAGE_DEATHSETTLEMENT_NONELIGIBLE_RMDEXIST_ERR = 18009;
        public static readonly int MESSAGE_WITHDRAWAL_REGEN_ELIGIBLE_RMD_ERR = 18010;
        public static readonly int MESSAGE_DEATHNOTIFICATION_REGEN_ELIGIBLE_RMD_ERR = 18011;
        public static readonly int MESSAGE_DEATHSETTLEMENT_REGEN_ELIGIBLE_RMD_ERR = 18012;
        public static readonly int MESSAGE_WITHDRAWAL_QD_NONELIGIBLE_RMDEXIST_ERR = 18013;
        public static readonly int MESSAGE_DEATHNOTIFICATION_QD_NONELIG_RMDEXIST_ERR = 18014;
        public static readonly int MESSAGE_DEATHSETTLEMENT_QD_NONELIG_RMDEXIST_ERR = 18015;
        public static readonly int MESSAGE_WITHDRAWAL_QD_REGEN_ELIGIBLE_RMD_ERR = 18016;
        public static readonly int MESSAGE_DEATHNOTIFICATION_QD_REGEN_ELIG_RMD_ERR = 18017;
        public static readonly int MESSAGE_DEATHSETTLEMENT_QD_REGEN_ELIG_RMD_ERR = 18018;
        public static readonly int MESSAGE_WITHDRAWALS_QD_REGEN_RMD_RETRO_TERM_ERR = 18019;
        public static readonly int MESSAGE_DEATHNOTIFICATION_QD_REGEN_RMD_RETRO_ERR = 18020;
        public static readonly int MESSAGE_DEATHSETTLEMENT_QD_REGEN_RMD_RETRO_TRM_ERR = 18021;
        public static readonly int MESSAGE_WITHDRAWALS_QD_REGEN_RMD_DECEASED_ERR = 18022;
        public static readonly int MESSAGE_DEATHNOTIFICATION_QD_REGEN_RMD_DECEASD_ERR = 18023;
        public static readonly int MESSAGE_DEATHSETTLEMENT_QD_REGEN_RMD_DECEASED_ERR = 18024;
        //START: SB | 2018.01.09 | YRS-AT-3324 |  20.4.3 | ErrorCode to display message to restrict prcoess in Withdrawal, Death Notification & Death Settlement
        public static readonly int MESSAGE_DEATH_NOTIFY_SETTLE_DECEASED_REGEN_RMD_ERR = 18025;
        public static readonly int MESSAGE_WITHDRAWAL_CURRENT_REGEN_RMD_ERR = 18026;
        public static readonly int MESSAGE_NONELIGIBLE_RMDEXIST_ERR = 18027;
        public static readonly int MESSAGE_NONELIGIBLE_QD_RMDEXIST_ERR = 18028;
        public static readonly int MESSAGE_ELIGIBLE_QD_RMD_RECORDS_MISSING_ERR = 18029;
        public static readonly int MESSAGE_DECEASED_ORIGINAL_PARTI_QD_REGEN_RMD_ERR = 18030;
        //END: SB | 2018.01.09 | YRS-AT-3324 | 20.4.3 | ErrorCode to display message to restrict prcoess in Withdrawal, Death Notification & Death Settlement
        #endregion
        //END: SB | 2017.08.07 | YRS-AT-3324 |  ErrorCode to display message to restrict prcoess in Withdrawal, Death Notification & Death Settlement

        //START: SB | 2017.12.18 | YRS-AT-3756 |  ErrorCode to display message, Used to restrict user from entering invalid RMD Tax rate in Withdrawal & Death Settlement screens(Values from 1 to 9 are restricted)
        #region Withdrawals
        public static readonly int MESSAGE_WITHDRAWAL_RMD_INVALID_TAXRATE_ERROR_MSG = 3002;
        //START : PK | 2020.05.06 | YRS-AT-4854 | Messages for non-covid refund request
        public static readonly int MESSAGE_WITHDRAWAL_PROCESS_NON_COVID_SCREEN = 3003;
        public static readonly int MESSAGE_WITHDRAWAL_EXTEND_NON_COVID_SCREEN = 3004;
        public static readonly int MESSAGE_WITHDRAWAL_CANCEL_NON_COVID_SCREEN = 3005;
        //START : PK | 2020.05.06 | YRS-AT-4854 | Messages for non-covid refund request

        #endregion

        #region Beneficiary Death Settlement
        public static readonly int MESSAGE_BENEF_SETTLE_RMD_INVALID_TAX_ERROR_MSG = 12601;
        #endregion
        //END: SB | 2017.12.18 | YRS-AT-3756 |  ErrorCode to display message, Used to restrict user from entering invalid RMD Tax rate in Withdrawal & Death Settlement screens(Values from 1 to 9 are restricted)

        public static readonly int MESSAGE_LOAN_DISPLAY_FIRST_PAYMENT_DATE = 9588; // PK | 01/21/2019 | YRS-AT-2573 | Added information message, which will be displayed on Re-Amortization tab of Loan Maintainance screen.

        //START: PPP | 04/20/2018 | YRS-AT-3101 | Added Payment Manager messages
        public static readonly int MESSAGE_PM_INVALID_DISBURSEMENT_TYPE = 13301;
        public static readonly int MESSAGE_PM_INVALID_EFT_TYPE = 13302;
        public static readonly int MESSAGE_PM_INVALID_EFT_DISBURSEMENT_STATUS = 13303;
        public static readonly int MESSAGE_PM_MISSING_DISBURSEMENT_EFTID = 13304;
        public static readonly int MESSAGE_PM_MISSING_PERSBANKING_ID = 13305;
        public static readonly int MESSAGE_PM_MISSING_DISBURSEMENT_ID = 13306;
        public static readonly int MESSAGE_PM_MISSING_BANK_ID = 13307;
        public static readonly int MESSAGE_PM_EFT_BANKRESPONSE_COMPLETE = 13308;
        public static readonly int MESSAGE_PM_EFT_BANKRESPONSE_COMPLETE_WITH_ERROR = 13309;
        public static readonly int MESSAGE_PM_SYSTEM_ERROR = 13310;       
        public static readonly int MESSAGE_PM_EMAIL_NOT_SENT_COMMON = 13312;
        public static readonly int MESSAGE_PM_INVALID_FILE = 13313;
        //START: VC | 2018.09.18 | YRS-AT-3101 | Messages to display in Payment manager screen
        public static readonly int MESSAGE_PM_CONFIRM_SELECT_ALL = 13316;
        public static readonly int MESSAGE_PM_CONFIRM_APPROVE_DISBURSEMENT = 13317;
        //public static readonly int MESSAGE_PM_CONFIRM_REJECT_DISBURSEMENT = 13318;
        public static readonly int MESSAGE_PM_MISMATCH_RECALCULATION = 13319;
        //END: VC | 2018.09.18 | YRS-AT-3101 | Messages to display in Payment manager screen

        //START : SR | 2018.10.05 | YRS-AT-3101 | Messages related to Bank EFT file  
   
        public static readonly int MESSAGE_PM_INVALID_PERSON_IN_BATCH = 13325;
        public static readonly int MESSAGE_PM_INVALID_LOAN_STATUS = 13326;
        public static readonly int MESSAGE_PM_INVALID_EFT_FILE_HEADER = 13327;
        public static readonly int MESSAGE_PM_CONFIRM_ZERO_APPROVE_DISBURSEMENT = 13328;
        public static readonly int MESSAGE_PM_ERROR_APPROVAL_EMAIL = 13329;        
        public static readonly int MESSAGE_PM_INVALID_EFT_FILE_PROCESSED = 13331;
        public static readonly int MESSAGE_PM_LOAN_EFT_2ND_REJECTED_NOTES = 13332;
        public static readonly int MESSAGE_PM_LOAN_EFT_DISABLED = 13333;
        public static readonly int MESSAGE_PM_LOAN_EFT_RECORD_SELECTION = 13334;
        public static readonly int MESSAGE_PM_LOAN_EFT_INVALID_BATCHID = 13335;
        //START: VC | 2018.11.16 | YRS-AT-3101 | Message related to EFT and ConfirmEFT
        public static readonly int MESSAGE_PM_LOAN_EFT_CONFIRM_GENERATE_EFT = 13336;
        public static readonly int MESSAGE_PM_LOAN_EFT_GENERATE_EFT_SUCCESS = 13337;
        public static readonly int MESSAGE_PM_LOAN_EFT_SELECT_DISBURSEMENT_TYPE = 13338;
        public static readonly int MESSAGE_PM_LOAN_EFT_FUNDID_NOTFOUND = 13339;
        //END: VC | 2018.11.16 | YRS-AT-3101 | Message related to EFT and ConfirmEFT
        //END : SR | 2018.10.05 | YRS-AT-3101 | Messages related to Bank EFT file

        //START : PK | 12/04/2019 |YRS-AT-4676 -  State Withholding - Vaildations for exporting file from Payment Manager (First Annuities) 
        public static readonly int MESSAGE_PM_DISBURSEMENT_SUCCESS = 13340;
        public static readonly int MESSAGE_PM_DISBURSEMENT_DISPLAY_ERRORWARNINGS = 13341;
        public static readonly int MESSAGE_PM_DISBURSEMENT_FAILURE = 13342;
        public static readonly int MESSAGE_PM_DISBURSEMENT_FAILURENOTE =13343;
        //END : PK | 12/04/2019 |YRS-AT-4676 -  State Withholding - Vaildations for exporting file from Payment Manager (First Annuities) 

        //START : BD : 11/21/2018 : YRS-AT-4133 -  YRS enh: Annuity Estimate calculator & Goal Calculator: RDB Plan Rule change NEEDED by DECEMBER 2018 (TrackIT 32497) 
        #region Retirement
        public static readonly int MESSAGE_RETIREMENT_RDB_RESTRICTED_DUE_TO_2019_PLAN_CHANGE = 42001;
        #endregion
        //END : BD : 11/21/2018 : YRS-AT-4133 -  YRS enh: Annuity Estimate calculator & Goal Calculator: RDB Plan Rule change NEEDED by DECEMBER 2018 (TrackIT 32497) 
        #region Annuity Purchase
        //START : BD : 11/19/2018 : YRS-AT-4136 - Person Maintenance screen: RDB Plan Rule change Messages
        public static readonly int MESSAGE_ANNUITY_PURCHASE_DEATH_BENEFIT_NOT_ALLOWED = 41001;
        public static readonly int MESSAGE_ANNUITY_PURCHASE_RETIREE_NOT_ALLOWED = 41002;
        //END : BD : 11/19/2018 : YRS-AT-4136 - Person Maintenance screen: RDB Plan Rule change Messages
        public static readonly int MESSAGE_STW_STATEAMOUNT_ALERT = 41003; //PK | 09/19/2019 | YRS-AT-4597 -  YRS enh: State Withholding Project - First Annuity Payments (UI design)
        #endregion
        
        #region Annuity Estimate
        //START : ML |2020.02.05 | YRS-AT-4769 - YRS enhancement-new Secure Act rule for Annuity Estimate and Annuity Purchase screens (TrackIt- 41078)
        public static readonly int MESSAGE_ANNUITY_ESTIMATE_JSTEXT_CHRONICALY_NOT_ILL = 10101;
        public static readonly int MESSAGE_ANNUITY_ESTIMATE_JSTEXT_CHRONICALY_ILL = 10102;
        public static readonly int MESSAGE_ANNUITY_ESTIMATE_JSNOTE_CHRONICALY_NOT_ILL = 10103;
        public static readonly int MESSAGE_ANNUITY_ESTIMATE_JSNOTE_CHRONICALY_ILL = 10104;        
        //END : ML |2020.02.05 | YRS-AT-4769 - YRS enhancement-new Secure Act rule for Annuity Estimate and Annuity Purchase screens (TrackIt- 41078)
        public static readonly int MESSAGE_BATCH_ESTIMATE_JSTXT_CHRONICALY_NOT_ILL = 10105; // ML |2020.02.18 | YRS-AT-4783 | Batch estimation report display message.
        #endregion
        
        #region Death Calculator
        //START : BD : 11/26/2018 : YRS-AT-3837 - YRS Enh: Death Benefit Application for RDB rule change NEEDED by DECEMBER 2018 (TrackIT 32497)
        public static readonly int MESSAGE_DEATH_CALC_INSRES_REQUIRED = 43001;
        //END : BD : 11/26/2018 : YRS-AT-3837 - YRS Enh: Death Benefit Application for RDB rule change NEEDED by DECEMBER 2018 (TrackIT 32497)
        #endregion
        //START : Dharmesh : 11/20/2018 : YRS-AT-4136 Adding validation message for participant who enrolled on or after 2019
        public static readonly int MESSAGE_RETIREE_PARTICIPANT_ENROLLED_ON_OR_AFTER_2019 = 1004;
        public static readonly int MESSAGE_RETIREE_PARTICIPANT_ENROLLED_ON_OR_AFTER_2019_FOR_INSURED_RESERVES = 1005;
        public static readonly int MESSAGE_RETIREE_PARTICIPANT_ENROLLED_ON_OR_AFTER_2019_IN_EDIT_BENEFICIARY_FOR_INSURED_RESERVES = 1007;
        public static readonly int MESSAGE_RETIREE_PARTICIPANT_ENROLLED_ON_OR_AFTER_2019_IN_EDIT_BENEFICIARY_FOR_RETIRE = 1008;
        //END : Dharmesh : 11/20/2018 : YRS-AT-4136 Adding validation message for participant who enrolled on or after 2019

        //START: ML | 2019.01.09 | YRS-AT-4244 | YRS enh: EFT Loans Maint. OND selection should display prior to disbursement (TrackIT 36462)    
        public static readonly int MESSAGE_OND_UPDATE = 9735;         
        public static readonly int MESSAGE_DISBURSE_WITHOUTSAVEOND = 9736;
        //END: ML | 2019.01.09| YRS-AT-4244 | YRS enh: EFT Loans Maint. OND selection should display prior to disbursement (TrackIT 36462)      

        public static readonly int TOOLTIP_LOANFREEZE_UNFREEZE_HISTORY = 1009;  //ML | 2019.01.15 |YRS-AT-3157 - Toottip message at History (Loan FreezeUnfreeze) icon

        //START: PPP | 01/25/2019 | YRS-AT-2920 | Added messages for Loan Maintenance screen. 
        public static readonly int MESSAGE_LOAN_UNFUNDED_PAYMENT_EXISTS = 9590;
        public static readonly int MESSAGE_LOAN_UNRECEIVED_PAYMENT_EXISTS = 9591;
        //END: PPP | 01/25/2019 | YRS-AT-2920 | Added messages for Loan Maintenance screen 

        //START: Shilpa N | 03/01/2019 | YRS-AT-4248 | Added messages to display tool tip when application is in Read Only Mode. 
        public static readonly int TOOLTIP_READONLY = 44001;
        public static readonly int READONLY_APPLICATION_HEADER_MESSAGE = 44002;
        //END: Shilpa N | 03/01/2019 | YRS-AT-4248 | Added messages to display tool tip when application is in Read Only Mode. 

       
        #region DAR File Import
        //START: SB | 11/14/2019 | YRS-AT-4599 | Created messages at DAR file import screen
        public static readonly int MESSAGE_IMPORT_DAR_INVALID_FILEEXTENSION = 13401;
        public static readonly int MESSAGE_IMPORT_DARFILE_INVALID_COLUMNHEADERS = 13402;
        public static readonly int MESSAGE_IMPORT_DAR_MISSMATCH_RECORDS = 13403;
        public static readonly int MESSAGE_IMPORT_DAR_SUCCESS = 13404;
        public static readonly int MESSAGE_IMPORT_DAR_DISCARDED = 13405;
        public static readonly int MESSAGE_IMPORT_DAR_FOLDERNOTEXISTS = 13406;
        public static readonly int MESSAGE_DOTNET_EXCEPTION = 13407;
        public static readonly int MESSAGE_SQL_EXCEPTION = 13408;
        public static readonly int MESSAGE_IMPORT_EMPTYDARFILE = 13409;
        //END: SB | 11/14/2019 | YRS-AT-4599 | Created messages at DAR file import screen

        //START: SR | 2020.01.06 | YRS-AT-4602 | Message for payroll proof succes
        public static readonly int MESSAGE_PAYROLL_PROOF_SUCCESS = 13501;
        public static readonly int MESSAGE_PAYROLL_FINAL_SUCCESS = 13506; //PK | 2020.01.23 | YRS-AT-4641 | Message for payroll final success.
        #endregion
        
        //START : SN | 2020.01.16 | YRS-AT-4641 | Messages for reverse feed file
        #region Reverse Feed Import

        public static readonly int MESSAGE_IMPORT_REVERSEFEED_APPROVED_SUCCESS = 13601;
        public static readonly int MESSAGE_IMPORT_REVERSEFEED_DISCARD_SUCCESS = 13602;
        public static readonly int MESSAGE_IMPORT_REVERSEFEED_RECORD_NOT_FOUND = 13603;

        #endregion
        //END : SN | 2020.01.16 | YRS-AT-4641 | Messages for reverse feed file

        //START : PK | 2020.05.06 | YRS-AT-4854 | Messages for COVID 19 
        #region COVID 19 
        public static readonly int MESSAGE_WITHDRAWAL_ADD_COVID = 3501;   // Suvarna B. : YRS-AT-4854 : Added error message to stop to add request if participant has exceed covid amount used limit
        public static readonly int MESSAGE_WITHDRAWAL_PROCESS_COVID_SCREEN = 3502;
        public static readonly int MESSAGE_WITHDRAWAL_EXTEND_COVID_SCREEN = 3503;
        public static readonly int MESSAGE_WITHDRAWAL_CANCEL_COVID_SCREEN = 3504;
        // START : SC | 2020.05.05 | YRS-AT-4874 | Added new messages for Regular COVID Withdrawals Processing
        public static readonly int MESSAGE_WITHDRAWAL_COVID_TAXRATE_NAN = 3515;
        public static readonly int MESSAGE_WITHDRAWAL_COVID_TAXRATE_INVALID = 3516;
        public static readonly int MESSAGE_WITHDRAWAL_COVID_AMOUNT_NAN = 3517;
        public static readonly int MESSAGE_WITHDRAWAL_COVID_AMOUNT_NEGATIVE = 3518;
        public static readonly int MESSAGE_WITHDRAWAL_COVID_INSUFFICIENT_AMT = 3519;
        public static readonly int MESSAGE_WITHDRAWAL_COVID_TAXRATE_NONINTEGER = 3520;
        public static readonly int MESSAGE_WITHDRAWAL_COVID_TOTAL_MISMATCH = 3521;
        // END : SC | 2020.05.05 | YRS-AT-4874 | Added new messages for Regular COVID Withdrawals Processing
        #endregion
        //END : PK | 2020.05.06 | YRS-AT-4854 | Messages for COVID 19
        
    }
}

