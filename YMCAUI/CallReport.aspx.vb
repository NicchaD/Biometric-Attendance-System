
'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA - YRS
' FileName			:	CallReport.aspx.vb
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
'Anudeep A               06/05/2014          BT:1051:YRS 5.0-1618 :Enhancements to Roll In process 
'Anudeep A               06/09/2014          BT:1051:YRS 5.0-1618 :Enhancements to Roll In process 
'Dinesh K                06/07/2014          BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages
'Anudeep A               24/07/2014          BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages
'Anudeep A               22/09/2015          YRS-AT-2609 -  YRS enh: Loans CSS colors and Reporting functionality
'Anudeep A               12/10/2015          YRS-AT-2662 - YRS enh: follow up to Loans release - nightly sql server job for Offset and Unfreeze processing
'Anudeep A               04/18/2016          YRS-AT-2831 - YRS enh: automate 'Default' of Loans (TrackIT 25242)
'Anudeep A               07/01/2016          YRS-AT-2830 - YRS enh: automate 'Close' of fully paid Loans (TrackIT 25242)
'Manthan Rajguru	     2017.04.26          YRS-AT-3205 -  YRS enh: needed by DECEMBER 2017 - RMD Print letters function for QD participants with first RMD due in December (TrackIT 27977) 
'Santosh B               28/04/2017          YRS-AT-3400 -  YRS enh: due MAY 2017 - RMD Print Letters- Letter to Non-respondents (new screen) 
'                                            YRS-AT-3401 - RMD Print Letters- Satisfied but not elected (new screen)
'Vinayan C               2018.07.19          YRS-AT-4017 -  YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process) 
'Santosh B               2018.07.30          YRS-AT-4017 - YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)
'Manthan Rajguru	     2017.07.26          YRS-AT-4017 - YRS enh: Loan EFT Project: "new" Loan Admin webpages (approve/decline/process)
'Shilpa Nagargoje        01/03/2020          YRS-AT-4599 -  YRS enh: State Tax Withholding - New import screen for "First Annuity Payroll Processing" (DAR File)
'Shilpa Nagargoje        01/11/2020          YRS-AT-4641 -  YRS REPORTS: State Tax Withholding- New screen for "Monthly Annuity Payroll Processing" (Reverse Feed) 
'*********************************************************************************************************************/ 
Imports System
Imports System.Data
Imports System.Web
Imports Microsoft.Reporting.WebForms
Imports Microsoft.ReportingServices
Imports System.Web.UI
Public Class CallReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim rptDataSource As New ReportDataSource
        Dim rptParameter As New ReportParameter()
        Dim strReportName As String
        Dim dtReport As New DataTable
        Dim dtReportError As New DataTable
        Dim rptDataSourceError As New ReportDataSource  'SN | 01/03/2020 | YRS-AT-4599 | Added to pass error dataset to report
        Try
            If Not Page.IsPostBack Then
                'Start  Dinesh Kanojia      2013.10.03          YRS 5.0-2165:RMD enhancements 
                'Print SSRS report from batch process screen
                If Not Session("ReportName") Is Nothing Then
                    strReportName = Session("ReportName").ToString()
                    Select Case strReportName
                        'Print RMD reports.
                        Case "RMD Report"
                            dtReport = SSRSDataset.GetSelectedMRD().Tables(0)
                            If (Not dtReport Is Nothing) Then
                                If dtReport.Rows.Count > 0 Then
                                    rptDataSource = New ReportDataSource("dsSelectedRMDRecords", dtReport)
                                    RptViewRMD.LocalReport.ReportPath = "Reports\" + Session("ReportName").ToString.Trim + ".rdlc"
                                End If
                            End If
                            'Print Processed Disbursement Report
                        Case "Processed Disbursement Report"
                            dtReport = SSRSDataset.GetMRDDisburseRecord().Tables(0)
                            If (Not dtReport Is Nothing) Then
                                If dtReport.Rows.Count > 0 Then
                                    rptDataSource = New ReportDataSource("dsProcessedRMDRecords", dtReport)
                                    RptViewRMD.LocalReport.ReportPath = "Reports\" + Session("ReportName").ToString.Trim + ".rdlc"
                                End If
                            End If
                            'Start: Added By Dinesh.k BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
                        Case "RMDPrintLetterList"
                            dtReport = SSRSDataset.GetPrintLetterRecords()
                            If Not dtReport Is Nothing Then
                                If dtReport.Rows.Count > 0 Then
                                    rptDataSource = New ReportDataSource("dsPrintLetters", dtReport)
                                    RptViewRMD.LocalReport.ReportPath = "Reports\" + Session("ReportName").ToString.Trim + ".rdlc"
                                End If
                            End If
                        Case "RMDBatchProcessPrintLetterList"
                            dtReport = SSRSDataset.GetRMDBatchProcessPrintLetterRecords
                            If Not dtReport Is Nothing Then
                                If dtReport.Rows.Count > 0 Then
                                    rptDataSource = New ReportDataSource("dsRMDProcessPrintList", dtReport)
                                    RptViewRMD.LocalReport.ReportPath = "Reports\" + Session("ReportName").ToString.Trim + ".rdlc"
                                End If
                            End If
                            'End: Added By Dinesh.k BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
                            'Start:AA:05.06.2014 - BT:1051 - YRS 5.0-1618 - Added a case for displaying rollin reminder list
                        Case "Roll In Reminder List"
                            dtReport = SSRSDataset.GetRollinReminderList
                            If (Not dtReport Is Nothing) Then
                                If dtReport.Rows.Count > 0 Then
                                    rptDataSource = New ReportDataSource("dsRollInremainder", dtReport)
                                    RptViewRMD.LocalReport.ReportPath = "Reports\" + Session("ReportName").ToString.Trim + ".rdlc" 'AA:09.06.2014 - BT:1051 - YRS 5.0-1618 - changed the path of the folder
                                End If
                            End If
                            'End:AA:05.06.2014 - BT:1051 - YRS 5.0-1618 - Added a case for displaying rollin reminder list
                            'Start:AA:05.06.2014 -BT:2434:YRS 5.0-2315 - Added a case for displaying rmd non eligible print list
                        Case "RMDBatchProcessNonEligibleList"
                            dtReport = SSRSDataset.GetRMDBatchProcessNonEligibleRecords
                            If Not dtReport Is Nothing Then
                                If dtReport.Rows.Count > 0 Then
                                    rptDataSource = New ReportDataSource("dtRMDBatchNonEligibleList", dtReport)
                                    RptViewRMD.LocalReport.ReportPath = "Reports\" + Session("ReportName").ToString.Trim + ".rdlc"
                                End If
                            End If
                            'End:AA:05.06.2014 -BT:2434:YRS 5.0-2315- Added a case for displaying rmd non eligible print list
                            'Start:AA:09.23.2015 - YRS AT-2609 - Added a case for displaying Loans Utility list
                        Case "Loan_Offset_Default_Aging"
                            dtReport = SSRSDataset.LoanOffsetDefaultAging
                            If HelperFunctions.isNonEmpty(dtReport) Then
                                rptDataSource = New ReportDataSource("dsOffsetDefaultAging", dtReport)
                                RptViewRMD.LocalReport.ReportPath = "Reports\" + Session("ReportName").ToString.Trim + ".rdlc"
                            End If
                        Case "Loan_Default"
                            dtReport = SSRSDataset.Loan_Defaulted
                            If HelperFunctions.isNonEmpty(dtReport) Then
                                rptDataSource = New ReportDataSource("dsLoanDefault", dtReport)
                                RptViewRMD.LocalReport.ReportPath = "Reports\" + Session("ReportName").ToString.Trim + ".rdlc"
                            End If
                        Case "Loan_Unfreeze_Phantom_Interest"
                            dtReport = SSRSDataset.Loan_Unfreeze_Phantom_Interest
                            If HelperFunctions.isNonEmpty(dtReport) Then
                                rptDataSource = New ReportDataSource("dsLoanPhantomInterest", dtReport)
                                RptViewRMD.LocalReport.ReportPath = "Reports\" + Session("ReportName").ToString.Trim + ".rdlc"
                            End If

                            'End:AA:09.23.2015 - YRS AT-2609 - Added a case for displaying Loans Utility list
                            'Start: AA:12.10.2015 Added case for new loan ofsset loans tab grid
                        Case "Auto_Loan_Offset"
                            dtReport = SSRSDataset.Auto_Loan_Offset
                            If HelperFunctions.isNonEmpty(dtReport) Then
                                rptDataSource = New ReportDataSource("dsAutoLoansOffset", dtReport)
                                RptViewRMD.LocalReport.ReportPath = "Reports\" + Session("ReportName").ToString.Trim + ".rdlc"
                            End If
                            'End: AA:12.10.2015 Added case for new loan ofsset loans tab grid
                            'Start: AA:12.10.2015 YRS-AT-2831:Added case for new loan Auto defaulted loans tab grid
                        Case "Auto_Loan_Default"
                            dtReport = SSRSDataset.Auto_Loan_Default
                            If HelperFunctions.isNonEmpty(dtReport) Then
                                rptDataSource = New ReportDataSource("dsAutoLoansDefault", dtReport)
                                RptViewRMD.LocalReport.ReportPath = String.Format("Reports\{0}.rdlc", Session("ReportName").ToString.Trim)
                            End If
                            'Start: AA:12.10.2015 YRS-AT-2831:Added case for new loan Auto defaulted loans tab grid
                            'Start: AA:06.29.2016 YRS-AT-2830:Added case for new loan Auto closed loans tab grid
                        Case "Auto_Loan_Closed"
                            dtReport = SSRSDataset.Auto_Loan_Closed
                            If HelperFunctions.isNonEmpty(dtReport) Then
                                rptDataSource = New ReportDataSource("dsAutoLoansClosed", dtReport)
                                RptViewRMD.LocalReport.ReportPath = String.Format("Reports\{0}.rdlc", Session("ReportName").ToString.Trim)
                            End If
                            'End: AA:06.29.2016 YRS-AT-2830:Added case for new loan Auto closed loans tab grid
                            'START: MMR |2017.04.26 | YRS-AT-3205 | Added case for displaying initial and followup list for special QD participants
                        Case "RMDSpecialQDInitialLetterList"
                            dtReport = SSRSDataset.GetRMDSpecialQDPrintLetterRecords
                            If Not dtReport Is Nothing Then
                                If dtReport.Rows.Count > 0 Then
                                    rptDataSource = New ReportDataSource("printSpecialQDLetters", dtReport)
                                    RptViewRMD.LocalReport.ReportPath = String.Format("Reports\{0}.rdlc", Session("ReportName").ToString.Trim)
                                End If
                            End If
                        Case "RMDSpecialQDFollowupLetterList"
                            dtReport = SSRSDataset.GetRMDSpecialQDPrintLetterRecords
                            If Not dtReport Is Nothing Then
                                If dtReport.Rows.Count > 0 Then
                                    rptDataSource = New ReportDataSource("printSpecialQDLetters", dtReport)
                                    RptViewRMD.LocalReport.ReportPath = String.Format("Reports\{0}.rdlc", Session("ReportName").ToString.Trim)
                                End If
                            End If
                            'END: MMR |2017.04.26 | YRS-AT-3205 | Added case for displaying initial and followup list for special QD participants
                            'START: sb | 2017.04.28 | yrs-at-3400 and 3401 | added case for nonrespondent and rmd satisfied not elected annual letter tab grid	
                        Case "RMDReminderToNonRespondentList"
                            dtReport = SSRSDataset.GetParticipantDetailsForReminderLetter()
                            If HelperFunctions.isNonEmpty(dtReport) Then
                                rptDataSource = New ReportDataSource("dsPrintNonRespondentLetters", dtReport)
                                RptViewRMD.LocalReport.ReportPath = String.Format("reports\{0}.rdlc", Session("reportname").ToString.Trim)
                            End If
                        Case "RMDReminderToAnnualElectionList"
                            dtReport = SSRSDataset.GetParticipantDetailsForReminderLetter()
                            If HelperFunctions.isNonEmpty(dtReport) Then
                                rptDataSource = New ReportDataSource("dsPrintRMDSatisfiedNotElectedLetters", dtReport)
                                RptViewRMD.LocalReport.ReportPath = String.Format("Reports\{0}.rdlc", Session("ReportName").ToString.Trim)
                            End If
                            'END: SB | 2017.04.28 | YRS-AT-3400 and 3401 | Added Case for NonRespondent and RMD satisfied not elected annual letter tab grid	
                            'START: VC | 2018.07.19 | YRS-AT-4017 | Added code for Loan Pending approval and Search tab print functionality
                        Case "WebLoanList"
                            dtReport = SSRSDataset.GetListOfWebLoan
                            If (Not dtReport Is Nothing) Then
                                If dtReport.Rows.Count > 0 Then
                                    rptDataSource = New ReportDataSource("dsWebLoans", dtReport)
                                    RptViewRMD.LocalReport.ReportPath = String.Format("Reports\{0}.rdlc", Session("ReportName").ToString.Trim)
                                End If
                            End If
                            'END: VC | 2018.07.19 | YRS-AT-4017 | Added code for Loan Pending approval and Search tab print functionality
                            'START: SB | 2018.07.30 | YRS-AT-4017 | Added case for Loan Exceptions report tab 
                        Case "LoanExceptions"
                            dtReport = SSRSDataset.GetLoanExceptions()
                            If HelperFunctions.isNonEmpty(dtReport) Then
                                rptDataSource = New ReportDataSource("dsLoanExceptions", dtReport)
                                RptViewRMD.LocalReport.ReportPath = String.Format("Reports\{0}.rdlc", Session("ReportName").ToString.Trim)
                            End If
                            'END: SB | 2018.07.30 | YRS-AT-4017 | Added case for Loan Exceptions report tab 
                            'START: MMR | 07/26/2018 | YRS-AT-4017 | Added case for displaying loan processsing list
                        Case "LoanProcessing"
                            dtReport = SSRSDataset.GetLoanProcesssingList()
                            If HelperFunctions.isNonEmpty(dtReport) Then
                                rptDataSource = New ReportDataSource("dsLoanProcessing", dtReport)
                                RptViewRMD.LocalReport.ReportPath = String.Format("Reports\{0}.rdlc", Session("ReportName").ToString.Trim)
                            End If
                            'END: MMR | 07/26/2018 | YRS-AT-4017 | Added case for displaying loan processsing list
                        Case "DAR_File_Import"
                            dtReport = SSRSDataset.GetSelectedDARFileRecords().Tables(0)
                            dtReportError = SSRSDataset.GetSelectedDARFileRecords().Tables(1)
                            If HelperFunctions.isNonEmpty(dtReport) Then
                                rptDataSource = New ReportDataSource("dsSelectedDARFileRecords", dtReport)
                                rptDataSourceError = New ReportDataSource("dsselectedDARFileErrorlist", dtReportError)
                                RptViewRMD.LocalReport.ReportPath = String.Format("Reports\{0}.rdlc", Session("ReportName").ToString.Trim)
                            End If
                        Case "ReverseFeed_File_Import"
                            dtReport = SSRSDataset.GetReverseFeedRecords().Tables(0)
                            dtReportError = SSRSDataset.GetReverseFeedRecords().Tables(1)
                            If HelperFunctions.isNonEmpty(dtReport) Then
                                rptDataSource = New ReportDataSource("dsSelectedReverseFeedFileRecords", dtReport)
                                rptDataSourceError = New ReportDataSource("dsselectedReverseFeedFileErrorlist", dtReportError)
                                RptViewRMD.LocalReport.ReportPath = String.Format("Reports\{0}.rdlc", Session("ReportName").ToString.Trim)
                            End If
                    End Select
                    If (Not dtReport Is Nothing) Then
                        If dtReport.Rows.Count > 0 Then
                            RptViewRMD.LocalReport.DataSources.Clear()
                            RptViewRMD.LocalReport.DataSources.Add(rptDataSource)
                               If (Not dtReportError Is Nothing) Then
                                    RptViewRMD.LocalReport.DataSources.Add(rptDataSourceError)
                                End If
                                RptViewRMD.LocalReport.Refresh()
                        End If
                        End If
                    End If
                End If
        Catch ex As Exception
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("~/ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
            HelperFunctions.LogException("Call SSRS Reports --> Load_Click ", ex)
            Throw
        End Try
        'END  Dinesh Kanojia      2013.10.03          YRS 5.0-2165:RMD enhancements 
    End Sub
End Class