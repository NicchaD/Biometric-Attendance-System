'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA - YRS
' FileName			:	SSRSDataset.vb
' Author Name		:	Dinesh Kanojia  
' Employee ID		:	
' Email				:	
' Contact No		:	
' Creation Time		:	
' Program Specification Name	:	
' Unit Test Plan Name			:	
'
' Designed by			:	
' Designed on			:	
'
'*******************************************************************************

'**********************************************************************************************************************  
'** Modification History    
'**********************************************************************************************************************    
'** Modified By         Date(MM/DD/YYYY)    Description  
'**********************************************************************************************************************  
'   Anudeep A           06/05/2014           BT:1051:YRS 5.0-1618 :Enhancements to Roll In process 
'   Dinesh K            07/06/2014           BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
'   Anudeep A           09/22/2015           YRS-AT-2609 -  YRS enh: Loans CSS colors and Reporting functionality
'   Anudeep A           12/10/2015           YRS-AT-2662 - YRS enh: follow up to Loans release - nightly sql server job for Offset and Unfreeze processing
'   Anudeep A           04/18/2016           YRS-AT-2831 - YRS enh: automate 'Default' of Loans (TrackIT 25242)
'   Anudeep A           07/01/2016           YRS-AT-2830 - YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
'   Chandra sekar       2016.07.12           YRS-AT-2772 - YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
'   Manthan Rajguru		2017.05.04           YRS-AT-3205 - YRS enh: needed by DECEMBER 2017 - RMD Print letters function for QD participants with first RMD due in December (TrackIT 27977)
'   Santosh Bura        2017.04.28           YRS-AT-3400 & YRS-AT-3401
'   Vinayan C           2018.07.19           YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'   Santosh Bura        2018.08.02           YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'   Manthan Rajguru		2018.07.26           YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'   Shilpa Nagargoje    01/03/2020           YRS-AT-4599 -  YRS enh: State Tax Withholding - New import screen for "First Annuity Payroll Processing" (DAR File)
'   Shilpa Nagargoje    01/11/2020           YRS-AT-4641 -  YRS REPORTS: State Tax Withholding- New screen for "Monthly Annuity Payroll Processing" (Reverse Feed) 

'*********************************************************************************************************************/ 
Imports System.Data
Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging

Public Class SSRSDataset
    Public Shared Function GetSelectedMRD() As DataSet
        Dim dsSelectedMRD As New DataSet
        Dim objMRDRequestandProcessing As New MRDRequestandProcessing
        dsSelectedMRD = objMRDRequestandProcessing.GetSelectedRMDRecords()
        Return dsSelectedMRD
    End Function

    Public Shared Function GetMRDDisburseRecord() As DataSet
        Dim dsDisburseMRD As New DataSet
        Dim objMRDRequestandProcessing As New MRDRequestandProcessing
        dsDisburseMRD.Tables.Add(objMRDRequestandProcessing.GetMRDDisbursement())
        Return dsDisburseMRD
    End Function
    'Start:   Dinesh K            07/06/2014           BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
    Public Shared Function GetRMDBatchProcessPrintLetterRecords() As DataTable
        Dim dtRMDBatchProcessPrintLetters As DataTable
        Dim objRMDBatchRequestAndProcessing As New RMDBatchRequestAndProcessing
        dtRMDBatchProcessPrintLetters = objRMDBatchRequestAndProcessing.GetRMDBatchProcessPrintLetters
        Return dtRMDBatchProcessPrintLetters
    End Function

    Public Shared Function GetPrintLetterRecords() As DataTable
        Dim objRMDPrintLetters As New RMDPrintLetters
        Return objRMDPrintLetters.GetPrintLetters()
    End Function
    'END:   Dinesh K            07/06/2014           BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.

    'Start:AA:05.06.2014 - BT:1051 - YRS 5.0-1618 - Added a case for displaying rollin reminder list
    Public Shared Function GetRollinReminderList() As DataTable
        Dim dtPrintLetters As New DataTable
        dtPrintLetters = RollInReminderForm.GetRollinPrintList
        Return dtPrintLetters
    End Function
    'ENd:AA:05.06.2014 - BT:1051 - YRS 5.0-1618 - Added a case for displaying rollin reminder list
    Public Shared Function GetRMDBatchProcessNonEligibleRecords() As DataTable
        Dim dtRMDBatchProcessPrintLetters As DataTable
        Dim objRMDBatchRequestAndProcessing As New RMDBatchRequestAndProcessing
        dtRMDBatchProcessPrintLetters = objRMDBatchRequestAndProcessing.GetRMDBatchProcessNonEligibleList
        Return dtRMDBatchProcessPrintLetters
    End Function
    'Start:AA:09.23.2015 - YRS AT-2609 - Added a case for displaying Loans Utility list
    Public Shared Function LoanOffsetDefaultAging() As DataTable
        Dim dtLoanOffsetDefaultAging As New DataTable
        dtLoanOffsetDefaultAging = SessionManager.SessionLoanUtility.dtOffset_Default_Ageing
        Return dtLoanOffsetDefaultAging
    End Function
    Public Shared Function Loan_Defaulted() As DataTable
        Dim dtLoanDefaulted As New DataTable
        dtLoanDefaulted = SessionManager.SessionLoanUtility.dtDefaulted
        Return dtLoanDefaulted
    End Function
    Public Shared Function Loan_Unfreeze_Phantom_Interest() As DataTable
        Dim dtLoanPhantomInterest As New DataTable
        dtLoanPhantomInterest = SessionManager.SessionLoanUtility.dtUnfreeze
        Return dtLoanPhantomInterest
    End Function
    'End:AA:09.23.2015 - YRS AT-2609 - Added a case for displaying Loans Utility list
    'Start: AA:12.10.2015 Added case for new loan ofsset loans tab grid
    Public Shared Function Auto_Loan_Offset() As DataTable
        Return SessionManager.SessionLoanUtility.dtAutoLoansOffset
    End Function
    'End: AA:12.10.2015 Added case for new loan ofsset loans tab grid
    'Start: AA:04.18.2016 YRS-AT-2831:Added case for new loan Auto Defaulted loans tab grid
    Public Shared Function Auto_Loan_Default() As DataTable
        Return SessionManager.SessionLoanUtility.dtAutoLoansDefault
    End Function
    'End: AA:04.18.2016 YRS-AT-2831:Added case for new loan Auto Defaulted loans tab grid
    'Start: AA:06.29.2016 YRS-AT-2830:Added function for new loan Auto Closed loans tab grid
    Public Shared Function Auto_Loan_Closed() As DataTable
        Return SessionManager.SessionLoanUtility.dtAutoLoansClosed
    End Function
    'End: AA:06.29.2016 YRS-AT-2830:Added function for new loan Auto Closed loans tab grid

    'Start: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)
    Public Shared Function WaivedParticipants(ByVal guiYMCAID As String) As DataTable
        Return YMCARET.YmcaBusinessObject.YMCABOClass.GetListOfWaivedParticipants(guiYMCAID).Tables(0)
    End Function
    'End: Chandra sekar | 2016.07.12 | YRS-AT-2772 | YRS enh: Waiver of Participation Tracking (TrackIT 24931) (dependency YERDI)

    'START: MMR | 2017.04.26 | YRS-AT-3205 | Get Print list records for Special QDRO Initial/Followup letter
    Public Shared Function GetRMDSpecialQDPrintLetterRecords() As DataTable
        Return SessionManager.SessionParticipantRMDs.SpecialQDROLetters
    End Function
    'END: MMR | 2017.04.26 | YRS-AT-3205 | Get Print list records for Special QDRO Initial/Followup letter

    'START: SB | 2017.04.28 | YRS-AT-3400 and 3401 | Added function for NonRespondent and RMD satisfied not elected annual letter tab grid	
    Public Shared Function GetParticipantDetailsForReminderLetter() As DataTable
        Return SessionManager.SessionParticipantRMDs.ReminderLetters
    End Function
    'END: SB | 2017.04.28 | YRS-AT-3400 and 3401 | Added function for NonRespondent and RMD satisfied not elected annual letter tab grid

    'START: VC | 2018.07.19 | YRS-AT-4017 | Added function for Loan Pending approval and Search tab Grid
    Public Shared Function GetListOfWebLoan() As DataTable
        Return SessionManager.SessionLoanAdmin.ListOfWebLoan
    End Function
    'END: VC | 2018.07.19 | YRS-AT-4017 | Added function for Loan Pending approval and Search tab Grid

    'START: SB | 2018.08.02 | YRS-AT-4017 | Added function for Loan Exception tab Grid
    Public Shared Function GetLoanExceptions() As DataTable
        Return SessionManager.SessionLoanUtility.dtLoanExceptions
    End Function
    'END: SB | 2018.08.02 | YRS-AT-4017 | Added function for Loan Exception tab Grid

    'START: MMR | 07/26/2018 | YRS-AT-4017 | Added function for Loan processing tab in Loan Admin
    Public Shared Function GetLoanProcesssingList() As DataTable
        Return SessionManager.SessionLoanAdmin.LoanProcessingList
    End Function
    'END: MMR | 07/26/2018 | YRS-AT-4017 | Added function for Loan processing tab in Loan Admin
    'START: SN | 01/02/2020 | YRS-AT-4599 | Added function for PrintList button in DAR Import screen
    Public Shared Function GetSelectedDARFileRecords() As DataSet
        Return SessionManager.SessionLoanUtility.dsSelectedDARFileRecords
    End Function
    'END: SN | 01/02/2020 | YRS-AT-4599 | Added function for PrintList button in DAR Import screen
    'START: SN | 01/11/2020 | YRS-AT-4641  | Added function for PrintList button in Reverse Feed screen
    Public Shared Function GetReverseFeedRecords() As DataSet
        Return SessionManager.SessionLoanUtility.GetReverseFeedRecords
    End Function
    'END: SN | 01/11/2020 | YRS-AT-4641 | Added function for PrintList button in Reverse Feed screen
End Class
