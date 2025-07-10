'
' Project Name		:	YMCA-YRS
' FileName			:	HelperFunctions.vb
' Author Name		:	 
' Employee ID		:	
' Email				:	
' Contact No		:	
' Creation Time		:	
' Program Specification Name	: 
'*******************************************************************************

'************************************************************************************
'Modified By        Date            Description
'*********************************************************************************************************************
'Shashank Patel     2013.09.25		BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
'Shashank Patel     2014.02.04		BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN
'Anudeep Adusumilli 2014.05.26      BT-1051:YRS 5.0-1618 :Enhancements to Roll In process
'B. Jagadeesh       2015.04.28      BT-2570:YRS 5.0-2380 :Added enum type AnnBeneDeathFollowup
'Anudeep A          2015.11.09      YRS-AT-2660 - YRS enh: new loan letter and email for payoff defaulted loans(closed status) TrackIT 24153
'Bala               2016.01.05      YRS-AT-1972 - YRS-enh: Special death processing required.
'Anudeep A          2015.12.07      YRS-AT-2662 - YRS enh: follow up to Loans release - nightly sql server job for Offset and Unfreeze processing
'Anudeep A          2016.03.21      YRS-AT-2594 - YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send)
'Bala               2016.04.27      YRS-AT-2667 - Adjustment to Loan Satisfied-part.rpt (for DEFAULTED LOANS)
'Anudeep A          2016.04.12      YRS-AT-2831 - YRS enh: automate 'Default' of Loans (TrackIT 25242)
'Anudeep A          2016.07.01      YRS-AT-2830 - YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
'Santosh Bura       2016.07.07      YRS-AT-2382 - Capture 'Reason' for change of beneficiary SSN and Annty bene SSN (TrackIT 19856) 
'Chandra sekar      2016.08.22      YRS-AT-3081 - YRS Enh: request to QDRO function (Retired ) to allow different amt or percent (TrackIT 23098)
'Manthan Rajguru    2017.05.09      YRS-AT-3205 -  YRS enh: needed by DECEMBER 2017 - RMD Print letters function for QD participants with first RMD due in December (TrackIT 27977) 
'Manthan Rajguru    2018.04.03      YRS-AT-3929 -  YRS REPORTS: EFT: Loans ( FIRST EFT PROJECT) -create new "Loan Acknowledgement" Email for EFT/direct deposit  
'Sanjay GS Rawat    2018.04.10      YRS-AT-3101 - YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
'Chandra sekar      2018.05.23      YRS-AT-3270 - YRS enh-email notifications for updates made Contacts tab (TrackIT 27727)
'Vinayan C          2018.08.06      YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab  
'*********************************************************************************************************************
Public Enum EnumEntityCode
    PERSON
    YMCA
    DBEN
    ABEN
    INST 'AA:26.05.2014 BT-1051:YRS 5.0-1618 Added enum type for institution
End Enum

'SP : 2013.09.25 : BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
Public Enum EnumMessageTypes
    [Error]
    Warning
    Information
    Success
End Enum

'BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN
Public Enum EnumAuditLogColumn
    chrSSNo
End Enum

'JB:BT-2570\YRS 5.0-2380:Added enum type AnnBeneDeathFollowup
Public Enum BatchProcess
    RMDBatchProcess
    RMDPrintLetters
    RMDRollins
    CashOutProcess
    AnnBeneDeathFollowup
End Enum

Public Enum EnumEmailTemplateTypes
    LOAN_CLOSED_PERSONS
    LOAN_DEFAULTED_PERSONS
    SERVICE_FAILURE
    SESSION_MGMNT
    CASHOUT_BATCH_DISB_SUMMARY
    HARDSHIP_TD_TERMN_INTIMATION
    DELINQ_INTIMATION
    COPY_FAILURE
    LOAN_PROCESS_INTIMATION
    LOAN_DEFAULT_INTIMATION
    LOAN_CLOSED_INTIMATION
    LOAN_SUSPEND_INTIMATION
    WITHDRAWAL_NON_US_CANADIAN
    VOID_DISB_PRIOR_MONTH_WITH_FEES
    VOID_DISB_PRIOR_MONTH_WITHOUT_FEES
    VOID_REISSUE_WITHDRAWAL
    YMCA_MERGER_INTIMATION
    YMCA_MERGER_LOAN_INTIMATION
    LOAN_FREEZE_PROCESS
    LOAN_UNFREEZE_PROCESS
    LOAN_DEFAULT_CLOSED_PERSONS 'AA:11.19.2015 YRS-AT-2660 Added a new enum email template for the default closed loans
    SPECIAL_DEATH_PROCESSING_REQUIRED 'Bala: 01/05/2016: YRS-AT-1972: Added a new email template for the special death processing.
    UNFUNDED_UEIN_INTIMATION ' AA:03/18/2016 YRS-AT-2594 Added
    LOAN_PAID_CLOSED_PERSONS 'Bala: 04/27/2016: YRS-AT-2667: Added a new email template for closing paid loand
    LOAN_EFT_PAYMENT_FAILURE ' SR | 2018.04.18 | YRS-AT-3101 | Added a new email template for sending mail in case of EFT loan payment rejected.
    'START : Chandra sekar | 2018.05.23 | YRS-AT-3270 | YRS enh-email notifications for updates made Contacts/Officers tab (TrackIT 27727)
    YMCA_NEW_CONTACT_NOTIFICATION
    YMCA_NEW_OFFICER_NOTIFICATION
    'END : Chandra sekar | 2018.05.23 | YRS-AT-3270 | YRS enh-email notifications for updates made Contacts/Officers tab (TrackIT 27727)
    LOAN_PROCESSED_EFT_YMCA 'MMR | 2018.05.18 | YRS-AT-3929 | Added mail template for email to YMCA for loan processing with payment mode as EFT
    'START : VC | 2018.08.06 | YRS-AT-4018 -  Added email templates for approve and decline pending loans
    LAC_APPROVE_LOAN
    LAC_DECLINE_LOAN
    'END : VC | 2018.08.06 | YRS-AT-4018 -  Added email templates for approve and decline pending loans
End Enum
'Start: AA:12.10.2015 Added enum below to check which tab is using 
Public Enum LoanUtilityTab
    Aging
    [Default]
    Freeze
    Offset
    AutoDefault 'AA:04.12.2016 YRS-AT-2831 Added 
    AutoClosed 'AA:06.29.2016 YRS-AT-2830 Added for the auto closed loans tab
End Enum
'End: AA:12.10.2015 Added enum below to check which tab is using 

'START : SB | 07/07/2016 | YRS-AT-2382 | Defining beneficiary types
Public Enum EnumBeneficiaryTypes
    HBENE
    NHBENE
End Enum
'END: SB | 07/07/2016 | YRS-AT-2382 | Defining beneficiary types
'START: Chandra sekar | 2016.08.22 | YRS-AT-3081  Added enum below to check Split Type in the Retiree QDRO Split modules
Public Enum EnumPlanTypes
    BOTH
    RETIREMENT
    SAVINGS
End Enum
'End: Chandra sekar | 2016.08.22 | YRS-AT-3081  Added enum below to check Split Type in the Retiree QDRO Split modules

'START: MMR | 05/09/2017 | YRS-AT-3205 | Enum to identify active tab on print letters pages.
Public Enum PrintLetterTabStrips
    NONE
    INITIAL
    FOLLOWUP
    REPRINT
End Enum
'END: MMR | 05/09/2017 | YRS-AT-3205 | Enum to identify active tab on print letters pages.

'START: VC | 2018.09.24 | YRS-AT-4018 | Struct to identify loan location WEB or YRS.
Public Structure LoanLocation
    Public Shared WEB As String = "WEB"
    Public Shared YRS As String = "YRS"
End Structure
'END: VC | 2018.09.24 | YRS-AT-4018 | Struct to identify loan location WEB or YRS.