'***************
'Changed by - Swopna V
'Changed on - 10 Jan,2008
'Change description - Added code to view EDI Exclusion List report
'***************
'**********************************************************************************************************************
'** Modification History    
'**********************************************************************************************************************
'** Modified By        Date(MM/DD/YYYY)       Issue ID                Description  
'**********************************************************************************************************************
'Priya                  19-Feb-2009           None                    Added trim to name as it displays spaces between name in report.
'Ashish                 2010.06.22          YRS 5.0-1115 
'Imran                  08/Sep/2010         Change "Birefltr" report parameters (only for Hardship )
'Imran                  24/Nov/2010         YRS 5.0-1209 / BT-673- Pass new fund id parameter for RetireeDeathBenefitsClaimForm and Pre-RetirementDeathBenefitsClaimForm report
'Imran                  12/July/2011        BT:892-YRS 5.0-1359 : Disability Estimate form
'Nikunj Patel			2011.10.28			Making changes to allow printing of AltPayeeAnnuityEst report YRS 5.0-1345
'Shashank Patel         2012.01.24          YRS 5.0-1365 : Need new batch processing option
'Bhavna S		        2012.03.03		    For BT-941,YRS 5.0-1432:New report of checks issued after date of death
'Prasad J				2012.04.11			For BT-1018,YRS 5.0-1561: Add 'Print Letter' and 'Reset Account' buttons to Web info display page
'Sanjay R.              21.09.2012          BT:1060/YRS 5.0-YRS 5.0-1346: Open Form and Letter on completion of QDRO beneficiary settlement.
'Anudeep                06-nov-2012         Changes made to get Termination watcher Process Report 
'Sanjeev Gupta(SG)      2012.12.05          BT-1436: YRS 5.0-1729: Parameters need for new report QDROWithdrawaletterunder5k.
'Sanjeev Gupta(SG)      2012.12.17          BT-1511: view cashout form\letters report
'Sanjay R               2012.12.20          YRS 5.0-1707- New death benefit application report
'Sanjeev Gupta(SG)      2012.01.04          BT-1511: view cashout form\letters report
'Anudeep                2013.03.25          YRS 5.0-1664:Annuity Estimate Long Form Enhancement
'Sanjay R               2013.06.26          BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
'Anudeep                2013.08.13          Bt-1512 :YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.   
'Dineshk                2013.09.24          Bt-2139 :YRS 5.0-2165:RMD enhancements 
'Anudeep                2014.05.22          BT:1051 :YRS 5.0-1618 :Enhancements to Roll In process
'Dinesh K               2014.06.10          BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
'B.Jagadeesh            2015.04.20          BT:2570:YRS 5.0-2380 - Annuity Beneficiary Death Initial Popup Letter
'Dinesh Kanojia         2015.06.15          BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
'Anudeep                2015.06.26          BT:2900: YRS 5.0-2533:Loan Modifications (Default) Project: two new YRS letters
'Manthan Rajguru        2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'Anudeep A              2016.07.03          YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
'Bala                   2016.04.22          YRS-AT-2667 - Adjustment to Loan Satisfied-part.rpt
'Sanjay Singh           2016.04.29          YRS-AT-2909 - Support Request: printing crystal report com exception LoanDefaultLetter.rpt 
'Manthan Rajguru        2016.05.05          YRS-AT-2909 -  Support Request: printing crystal report com exception LoanDefaultLetter.rpt
'Sanjay GS Rawat        2018.04.10          YRS-AT-3101 - YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
'Shiny C.               2020.04.10          YRS-AT-4852 -  COVID - YRS changes needed for LOANS due to CARE Act (Track IT - 41693)
'**********************************************************************************************************************

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.CrystalReports.ViewerObjectModel
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports YMCAUI.SessionManager

Public Class ReportViewer
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Dim ParameterFieldsCollection As New CrystalDecisions.Shared.ParameterFields
    'Dim objRpt As New ReportDocument
    Protected WithEvents btnLast As System.Web.UI.WebControls.Button
    Protected WithEvents btnPrevious As System.Web.UI.WebControls.Button
    Protected WithEvents btnNext As System.Web.UI.WebControls.Button
    Protected WithEvents btnFirst As System.Web.UI.WebControls.Button
    Protected WithEvents btnExport As System.Web.UI.WebControls.Button
    Protected WithEvents CrystalReportViewer1 As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents ddlReportOption As System.Web.UI.WebControls.DropDownList
    Protected WithEvents tableReportOption As System.Web.UI.HtmlControls.HtmlTable

    Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Dim strReportName As String
    'Added By SG: 2012.12.05: BT-1436
    Dim strModuleName As String
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
        strReportName = CType(Session("strReportName"), String)
        'Added By SG: 2012.12.05: BT-1436
        strModuleName = CType(Session("strModuleName"), String)
    End Sub

#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'If Session("LoggedUserKey") Is Nothing Then
        '    Response.Redirect("Login.aspx", False)
        '    Exit Sub
        'End If
        'by aparna
        Dim sReportName As String
        'by aparna
        Dim bBoolReport As Boolean
        Dim LstBox As New System.Web.UI.WebControls.ListBox
        Dim strRetdraftfullform As String
        Try
            If Not Session("ManualTransacts") = "ManualTransacts" Then
                btnExport.Attributes.Add("onclick", "JavaScript:showFileDialogBox()")
            End If
            If Not Session("VRManager") = "VRManager" Then
                btnExport.Attributes.Add("onclick", "JavaScript:showFileDialogBox()")
            End If

            'Added By Aparna on 13th April

            sReportName = strReportName
            'Added by Anudeep:25.03.2013:YRS 5.0-1664:Annuity Estimate Long Form Enhancement
            strRetdraftfullform = System.Configuration.ConfigurationSettings.AppSettings("RET_EST_DraftFullForm")

            Select Case sReportName
                Case "YRSParticipantHistoryReportBySSNo"
                    'strReportName = Session("strReportName")
                    LstBox = Session("ListBoxSelectedItems")
                    getYRSParticipantHistoryReportBySSNo(strReportName, LstBox)
                    'populateReportsValues()

                Case "Pre-Notification"
                    'strReportName = Session("strReportName")
                    populateReportsValues()

                Case "DeathBenefitOptions"
                    'strReportName = Session("strReportName")
                    populateReportsValues()
                    'BS:2012.03.03:BT-941,YRS 5.0-1432:- Add report name into Case Section 
                Case "List of Uncashed Checks and Payments Issued After the Date of Death"
                    populateReportsValues()

                Case "First Annuity Checks Request"
                    'strReportName = Session("strReportName")
                    populateReportsValues()

                Case "Disbursement Request"
                    'strReportName = Session("strReportName")
                    populateReportsValues()

                Case "AccouToBechanged"
                    'strReportName = Session("strReportName")
                    populateReportsValues()

                Case "Pre-RetirementDeathBenefitsClaimForm"
                    'strReportName = Session("strReportName")
                    populateReportsValues()
                    'getPreRetirementDeathBenefitsClaimForm(strReportName)

                    'Ashish:2010.06.22 YRS 5.0-1115 ,commented
                    'Case "ProspectiveRetirementAllowance"
                    '    'strReportName = Session("strReportName")
                    '    populateReportsValues()

                Case "RetireeDeathBenefitsClaimForm"
                    'strReportName = Session("strReportName")

                    populateReportsValues()
                    'getRetireeDeathBenefitsClaimForm(strReportName)
                Case "birefltr"
                    'strReportName = Session("strReportName")
                    populateReportsValues()
                    'getYMCARefundLetterHardship(strReportName)
                    'Case Session("strReportName") = "Cashout"
                    '    strReportName = Session("strReportName")
                    '    populateReportsValues()

                Case "ReleaseBlankLess1K", "ReleaseBlank1kto5k", "ReleaseBlankOver5k", "RL_2_FullorPartialRefund", "HardshipWithDrawalofTDForm", "RL_3_FullPIAOver15000", "RL_2_FullorPartialRefund", "RL_2_FullorPartialRefund", "RL_2_FullorPartialRefund", "RL_3_FullPIAOver15000", "DeferredAssociationAnnuityAgreementForm", "RL_6a_Over70PIAOver15000", "RL_6c_Over70PIAUnder5000", "RL_6b_Over70PIA5000-15000", "HWL_4_TDOnly", "HWL_5_TDAndOtherAcc"

                    populateReportsValues()
                Case "Loan Letter to Association"
                    populateReportsValues()
                Case "Loan Summary"
                    populateReportsValues()
                Case "Report Pending Debits"  'Code Added by ashutosh 18 sep
                    populateReportsValues()
                Case "Report Export Debits"  'Code Added by ashutosh 18 sep
                    populateReportsValues()
                    'Aparna Samala Nov 7th 2006 -ACH Debit
                Case "YMCA ACH Report"
                    populateReportsValues()
                    'Aparna Samala 21/09/2006 -Delinquency Letters
                Case "DelinquencyReport"
                    populateReportsValues()
                    'Aparna Samala 29/11/2006 -Cashouts
                Case "Cash Out"
                    tableReportOption.Visible = True
                    populateReportsValues()

                    'Added By Ashutosh Patil as on 19-01-2007  for YREN - 3023   
                    'AA:26.06.2015 BT:2900 YRS 5.0-2533:Modified letter for default and ofset loans
                Case "LoanDefaultLetter"
                    populateReportsValues()
                Case "Military-part"
                    populateReportsValues()
                    ' Start | SC | 2020.04.10 | YRS-AT-4852 | Added new loan suspend report for participant as per COVID Care act
                Case "Loan_Suspension_Ltr_to_Part-COVID-19"
                    populateReportsValues()
                    ' End | SC | 2020.04.10 | YRS-AT-4852 | Added new loan suspend report for participant as per COVID Care act
                Case "Loan Satisfied-part"
                    populateReportsValues()
                    'by Aparna For EDIoutsourceCheck printing. 06 Aug-2007
                Case "EDILogReport"
                    populateReportsValues()
                    'Added by Swopna on 10 Jan,2008,in response to bug id 349
                    '*********
                Case "EDI Exclusion List"
                    populateReportsValues()
                    '*********
                    'Added by Ashish 18-Mar-2009 fro Issue YRS5.0 679
                Case "Loan Reamortization Letters"
                    populateReportsValues()

                    'Ashish:2010.06.22 YRS 5.0-1115 ,Start
                Case "ANNTYESTLONG"
                    populateReportsValues()
                Case "ANNTYESTSHORT"
                    populateReportsValues()
                Case "ANNTYESTCOLOR"
                    populateReportsValues()
                    'Ashish:2010.06.22 YRS 5.0-1115 ,End
                    'BT:892-YRS 5.0-1359 : Disability Estimate form
                Case "DisabilityAnnuityEst", "AltPayeeAnnuityEst"   'NP:2011.10.28:YRS 5.0-1345 - Adding AltPayeeAnnuityEst to the switch case to enable handling of the report
                    populateReportsValues()
                    'Added by prasad YRS 5.0-1561: Add 'Print Letter' and 'Reset Account' buttons to Web info display page
                Case "WebIdLetterOleN"
                    populateReportsValues()
                    'SR:21.09.2012 - BT:1060/YRS 5.0-YRS 5.0-1346: Open Form and Letter on completion of QDRO beneficiary settlement.
                Case "RL_2_FullRefundQDRO"
                    populateReportsValues()
                Case "ReleaseBlankLess1K"
                    populateReportsValues()
                    'Added by Anudeep:07.11.2012 - BT:957/YRS 5.0-1484: Termination Watcher Processed Records Report 
                Case "TerminationWatcher"
                    populateReportsValues()
                    'Added By SG: 2012.12.05: BT-1436
                Case "Withdrawals_New"
                    populateReportsValues()
                    'SR:2012.12.20 - YRS 5.0-1707- New death benefit application report
                    'SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
                    'AA:2013.08.13 - Bt-1512 :YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
                Case "Death Benefit Application", "Death Letter for all beneficiaries", "Death Letter for all beneficiaries 60 day followup", "Death Letter for all beneficiaries 90 day followup"
                    populateReportsValues()
                    'Dineshk                2013.09.24          Bt-2139 :YRS 5.0-2165:RMD enhancements 
                Case "Initial_RMD_Letter_April_Distribution_Deadline"
                    populateReportsValues()
                    'Start: Dinesh K               2014.06.10          BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
                Case "RMD cover letter for check April Deadline"
                    populateReportsValues()
                Case "December RMD letter for check"
                    populateReportsValues()
                Case "RMD Initial Letter", "RMD Followup Letter"
                    populateReportsValues()
                    'END: Dinesh K               2014.06.10          BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
                Case strRetdraftfullform
                    'Added by Anudeep:25.03.2013:YRS 5.0-1664:Annuity Estimate Long Form Enhancement
                    populateReportsValues()
                    'Start:AA:26.05.2014 BT-1051:YRS 5.0-1618 Added for showing report for rollin
                Case "Letter of Acceptance"
                    populateReportsValues()
                    'End:AA:26.05.2014 BT-1051:YRS 5.0-1618 Added for showing report for rollin
                    'Start:JB:16.04.2015 BT-2570:YRS 5.0-2380 Added for showing report of initial communication popup letter
                Case "DB_Initial PopUp Letter"
                    populateReportsValues()
                    'Start:Dinesh Kanojia         2015.06.15        BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
                Case "Withdrawals_PortableProject"
                    populateReportsValues()
                    'Start:Dinesh Kanojia         2015.06.15        BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
                    'Start:AA:07.03.2016 YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
                Case "New Cashout Letter"
                    populateReportsValues()
                    'End:AA:07.03.2016 YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
                    'START : SR | 2018.04.25 | YRS-AT-3101 | New report added to display disbursements ready for approval/ rejections. 
                Case "PendingEFTDisbursements"
                    populateReportsValues()
                Case "ConfirmEFTDisbursements"
                    populateReportsValues()
                    'END : SR | 2018.04.25 | YRS-AT-3101 | New report added to display disbursements ready for approval/ rejections. 
                Case Else
                    'strReportName = Session("strReportName")
                    bBoolReport = getReport(strReportName)


            End Select
            'Added By Aparna on 13th April

            'commented by Aparna to execute new code on 13th april

            ''Put user code to initialize the page here
            'If Session("strReportName") = "YRSParticipantHistoryReportBySSNo" Then
            '    strReportName = Session("strReportName")
            '    LstBox = Session("ListBoxSelectedItems")
            '    getYRSParticipantHistoryReportBySSNo(strReportName, LstBox)
            '    '    ParameterFieldsCollection = Session("ParameterFieldsCollection")
            '    '    bBoolReport = getReport(strReportName, ParameterFieldsCollection)
            '    'ElseIf Session("strReportName") = "AccouTobeChanged" Then
            '    '    strReportName = Session("strReportName")
            '    '    getAccouTobeChanged(strReportName)
            '    'ElseIf Session("strReportName") = "birefltr" Then
            '    '    strReportName = Session("strReportName")
            '    '    getbirefltr(strReportName)
            '    'ElseIf Session("strReportName") = "Cashout" Then
            '    '    strReportName = Session("strReportName")
            '    '    getCashout(strReportName)
            '    'ElseIf Session("strReportName") = "CRNONRETIRED" Then
            '    '    strReportName = Session("strReportName")
            '    '    getCRNONRETIRED(strReportName)
            'ElseIf Session("strReportName") = "DeathBenefitOptions" Then
            '    strReportName = Session("strReportName")
            '    getDeathBenefitOptions(strReportName)
            'ElseIf Session("strReportName") = "Pre-Notification" Then
            '    strReportName = Session("strReportName")
            '    ' 34231 Commented to execute New Code
            '    'getPreNotificationReport(strReportName)
            '    populateReportsValues()
            'ElseIf Session("strReportName") = "First Annuity Checks Request" Then

            '    strReportName = Session("strReportName")
            '    'commented by Aparna to Check New Code

            '    'getFirstAnnuityChecksRequestReport(strReportName)

            '    'commented by Aparna to Check New Code
            '    populateReportsValues()

            'ElseIf Session("strReportName") = "Disbursement Request" Then
            '    strReportName = Session("strReportName")
            '    'commented by Aparna on  13THAPRIL to Check New Code
            '    'getDisbursementRequestReport(strReportName)
            '    'commented by Aparna on 13th April to Check New Code
            '    populateReportsValues()

            'ElseIf Session("strReportName") = "AccouToBechanged" Then
            '    strReportName = Session("strReportName")
            '    'commented by Aparna on  13THAPRIL to Check New Code
            '    'getMEPStatusUpdate(strReportName)
            '    'commented by Aparna on  13THAPRIL to Check New Code
            '    populateReportsValues()



            '    'ElseIf Session("strReportName") = "DeferredAssociationAnnuityAgreementForm" Then
            '    '    strReportName = Session("strReportName")
            '    '    getDeferredAssociationAnnuityAgreementForm(strReportName)
            '    'ElseIf Session("strReportName") = "Disbursement Request" Then
            '    '    strReportName = Session("strReportName")
            '    '    getDisbursementRequest(strReportName)
            '    'ElseIf Session("strReportName") = "HardshipWithdrawalofTDForm" Then
            '    '    strReportName = Session("strReportName")
            '    '    getHardshipWithdrawalofTDForm(strReportName)
            '    'ElseIf Session("strReportName") = "HWL_4_TDOnly" Then
            '    '    strReportName = Session("strReportName")
            '    '    getHWL_4_TDOnly(strReportName)
            '    'ElseIf Session("strReportName") = "HWL_5_TDAndOtherAcc" Then
            '    '    strReportName = Session("strReportName")
            '    '    getHWL_5_TDAndOtherAcc(strReportName)
            '    'ElseIf Session("strReportName") = "JSBeneficiaryDeath3" Then
            '    '    strReportName = Session("strReportName")
            '    '    getJSBeneficiaryDeath3(strReportName)
            'ElseIf Session("strReportName") = "Pre-RetirementDeathBenefitsClaimForm" Then
            '    strReportName = Session("strReportName")
            '    getPreRetirementDeathBenefitsClaimForm(strReportName)
            'ElseIf Session("strReportName") = "ProspectiveRetirementAllowance" Then
            '    strReportName = Session("strReportName")
            '    getProspectiveRetirementAllowance(strReportName)
            '    'ElseIf Session("strReportName") = "ReleaseBlank" Then
            '    '    strReportName = Session("strReportName")
            '    '    getReleaseBlank(strReportName)
            '    'ElseIf Session("strReportName") = "ReleaseBlank1kto5k" Then
            '    '    strReportName = Session("strReportName")
            '    '    getReleaseBlank1kto5k(strReportName)
            '    'ElseIf Session("strReportName") = "ReleaseBlankLess1K" Then
            '    '    strReportName = Session("strReportName")
            '    '    getReleaseBlankLess1K(strReportName)
            '    'ElseIf Session("strReportName") = "ReleaseBlankOver5k" Then
            '    '    strReportName = Session("strReportName")
            '    '    getReleaseBlankOver5k(strReportName)
            'ElseIf Session("strReportName") = "RetireeDeathBenefitsClaimForm" Then
            '    strReportName = Session("strReportName")
            '    getRetireeDeathBenefitsClaimForm(strReportName)
            '    'ElseIf Session("strReportName") = "RL_2_FullorPartialRefund" Then
            '    '    strReportName = Session("strReportName")
            '    '    getRL_2_FullorPartialRefund(strReportName)
            '    'ElseIf Session("strReportName") = "RL_3_FullPIAOver15000" Then
            '    '    strReportName = Session("strReportName")
            '    '    getRL_3_FullPIAOver15000(strReportName)
            '    'ElseIf Session("strReportName") = "RL_6a_Over70PIAOver15000" Then
            '    '    strReportName = Session("strReportName")
            '    '    getRL_6a_Over70PIAOver15000(strReportName)
            '    'ElseIf Session("strReportName") = "RL_6b_Over70PIA5000-15000" Then
            '    '    strReportName = Session("strReportName")
            '    '    getRL_6b_Over70PIA500015000(strReportName)
            '    'ElseIf Session("strReportName") = "RL_6c_Over70PIAUnder5000" Then
            '    '    strReportName = Session("strReportName")
            '    '    getRL_6c_Over70PIAUnder5000(strReportName)
            '    'ElseIf Session("strReportName") = "safeharbor" Then
            '    '    strReportName = Session("strReportName")
            '    '    getsafeharbor(strReportName)
            '    'ElseIf Session("strReportName") = "SafeHarborLetters" Then
            '    '    strReportName = Session("strReportName")
            '    '    getSafeHarborLetters(strReportName)
            '    'ElseIf Session("strReportName") = "TransitionLetters" Then
            '    '    strReportName = Session("strReportName")
            '    '    getTransitionLetters(strReportName)

            'ElseIf Session("strReportName") = "BIREFLTR" Then
            '    strReportName = Session("strReportName")
            '    getYMCARefundLetterHardship(strReportName)
            'Else
            '    strReportName = Session("strReportName")
            '    bBoolReport = getReport(strReportName)
            'End If
            'commented by Aparna to execute new code on 13th april

        Catch 'ex As Exception
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            Throw
        End Try
    End Sub
    Private Function populateReportsValues()
        Dim ArrListParamValues As New ArrayList
        Dim sReportName As String
        Dim boollogontoDB As Boolean
        'Added By Aparna on 17th April
        Dim l_dataSet As DataSet
        Dim l_datatable As DataTable
        Dim l_datarow As DataRow
        Dim l_String_tmp As String
        Dim l_string_Persid As String
        Dim ListBoxSelectedItems As New System.Web.UI.WebControls.ListBox
        Dim i As Integer
        Dim strRetdraftfullform As String
        If Not Session("ListBoxSelectedItems") Is Nothing Then
            ListBoxSelectedItems = Session("ListBoxSelectedItems")
        End If

        'end code Added By Aparna on 17th April

        Try

            sReportName = strReportName

            'Added by Anudeep:25.03.2013:YRS 5.0-1664:Annuity Estimate Long Form Enhancement
            strRetdraftfullform = System.Configuration.ConfigurationSettings.AppSettings("RET_EST_DraftFullForm")

            Select Case sReportName


                Case "Pre-Notification"

                    boollogontoDB = True
                    'Added by Swopna on 10 Jan,2008,in response to bug id 349
                    '*********
                Case "EDI Exclusion List"
                    boollogontoDB = True
                    '*********

                Case "DeathBenefitOptions"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("PersID"), String).ToString.Trim)
                    'By Aparna done on 13th april
                    'BS:2012.03.03:BT-941,YRS 5.0-1432:- Add Report Name and parameter Session("FundNo"),boollogontoDB  into Case Section 
                Case "List of Uncashed Checks and Payments Issued After the Date of Death"
                    boollogontoDB = True
                    ArrListParamValues.Add(Convert.ToInt32(Session("FundNo")))
                Case "YRSParticipantHistoryReportBySSNo"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("SSNo"))
                    For i = 0 To ListBoxSelectedItems.Items.Count - 1
                        ArrListParamValues.Add(ListBoxSelectedItems.Items(i).Text.ToString())
                    Next
                Case "First Annuity Checks Request"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("PaymentMethodCode")) 'SR | 2018.04.25 | YRS-AT-3101 | New parameter added to report to display disbursements based on payment method type.
                Case "Disbursement Request"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("DisbType"))
                Case "AccouToBechanged"
                    boollogontoDB = True
                    ArrListParamValues.Add(Convert.ToDateTime(Session("MonthEndClosingDate")))
                    'commented by Aparna on 18th april 2006 due to some error in code 
                    'used  getPreRetirementDeathBenefitsClaimForm(ByVal sReportName As String)

                Case "Pre-RetirementDeathBenefitsClaimForm"
                    boollogontoDB = True
                    'commented by Aparna 12/12/2007 -YRPS -4121 New parameters added

                    'ArrListParamValues.Add(CType(Session("Para1_MemberName"), String).ToString)
                    'ArrListParamValues.Add(CType(Session("Para2_BeneficiaryName"), String).ToString)
                    'ArrListParamValues.Add(CType(Session("Para3_lcAccountname1"), String).ToString)
                    'ArrListParamValues.Add(CType(Session("Para4_lcNonTax1"), String).ToString)
                    'ArrListParamValues.Add(CType(Session("Para5_lcTaxable1"), String).ToString)
                    'ArrListParamValues.Add(CType(Session("Para6_lcBalance1"), String).ToString)
                    'ArrListParamValues.Add(CType(Session("Para7_lcAccountname2"), String).ToString)
                    'ArrListParamValues.Add(CType(Session("Para8_lcNonTax2"), String).ToString)
                    'ArrListParamValues.Add(CType(Session("Para9_lcTaxable2"), String).ToString)
                    'ArrListParamValues.Add(CType(Session("Para10_lcBalance2"), String).ToString)
                    'ArrListParamValues.Add(CType(Session("Para11_lcAccountnameT"), String).ToString)
                    'ArrListParamValues.Add(CType(Session("Para12_lcNonTaxT"), String).ToString)
                    'ArrListParamValues.Add(CType(Session("Para13_lcTaxableT"), String).ToString)
                    'ArrListParamValues.Add(CType(Session("Para14_lcBalanceT"), String).ToString)


                    ArrListParamValues.Add(CType(Session("Para1_NameofDeceased"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para2_NameofClaimant"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para3_ReservesAcctName"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para4_Reserves_NonTaxable"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para5_Reserves_Taxable"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para6_Reserves_Balance"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para7_Savings_ReservesAcctName"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para8_Savings_NonTaxable"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para9_Savings_Taxable"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para10_Savings_Balance"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para11_DBAcctName"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para12_DB_NonTaxable"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para13_DB_Taxable"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para14_DB_Balance"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para15_TotalAcctname"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para16_Total_NonTaxable"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para17_Total_Taxable"), String).ToString)
                    ArrListParamValues.Add(CType(Session("Para18_Total_Balance"), String).ToString)
                    'commented by Aparna on 18th april 2006 due to some error in code 
                    'By Imran on 24/11/2010 - YRS 5.0-1209 / BT-673- Pass new fund id parameter for RetireeDeathBenefitsClaimForm and Pre-RetirementDeathBenefitsClaimForm report
                    ArrListParamValues.Add(Session("Para_FundId"))


                    'Ashish:2010.06.22  YRS 5.0-1115, Start
                    'Case "ProspectiveRetirementAllowance"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(Session("GUID"))
                    'Ashish:2010.06.22  YRS 5.0-1115, End
                Case "ANNTYESTLONG"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("GUID"))
                Case "ANNTYESTSHORT"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("GUID"))
                Case "ANNTYESTCOLOR"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("GUID"))
                    'Ashish:2010.06.22  YRS 5.0-1115, End
                    'IB:BT:892-YRS 5.0-1359 : Disability Estimate form
                Case "DisabilityAnnuityEst", "AltPayeeAnnuityEst"   'NP:2011.10.28:YRS 5.0-1345 - Adding AltPayeeAnnuityEst to the switch case to enable handling of the report
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("GUID"))
                Case strRetdraftfullform
                    'Added by Anudeep:25.03.2013:YRS 5.0-1664:Annuity Estimate Long Form Enhancement
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("GUID"))
                Case "RetireeDeathBenefitsClaimForm"
                    boollogontoDB = True
                    'by aparna 12/12/2007 -YRPS -4121 Changed the session names
                    'ArrListParamValues.Add(Session("Para1_MemberName"))
                    'ArrListParamValues.Add(Session("Para2_BeneficiaryName"))

                    ArrListParamValues.Add(Session("Para1_NameofDeceased"))
                    ArrListParamValues.Add(Session("Para2_NameofClaimant"))
                    'by aparna 12/12/2007 -YRPS -4121 Changed the session names
                    'By Imran on 24/11/2010 - YRS 5.0-1209 / BT-673- Pass new fund id parameter for RetireeDeathBenefitsClaimForm and Pre-RetirementDeathBenefitsClaimForm report
                    ArrListParamValues.Add(Session("Para_FundId"))

                Case "Loan Letter to Association"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("PersID"))
                Case "Loan Summary"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("PersID"))
                    ArrListParamValues.Add(Session("LoanNumber"))

                Case "birefltr"

                    boollogontoDB = True
                    l_string_Persid = CType(Session("PersonID"), String)

                    If l_string_Persid.Trim = String.Empty Then
                        Throw New Exception("Error: Person Id Not Found")
                    End If
                    'IB: Added on 08/Sep/2010 Change "Birefltr" report parameters list
                    ArrListParamValues.Add(CType(l_string_Persid, String).Trim)
                    ''IB:Report parameter change so below code is Commented 

                    'l_dataSet = YMCARET.YmcaBusinessObject.RefundRequest.YMCARefundLetterHardship(l_string_Persid)

                    'If l_dataSet.Tables.Count > 0 Then
                    '    l_datatable = l_dataSet.Tables(0)
                    '    If l_datatable.Rows.Count > 0 Then
                    '        l_datarow = l_datatable.Rows(0)
                    '    End If
                    'End If

                    'ArrListParamValues.Add(CType(l_datarow("ContactName"), String).Trim)
                    'ArrListParamValues.Add(CType(l_datarow("YmcaName"), String).Trim)
                    'ArrListParamValues.Add(CType(l_datarow("Addr1"), String).Trim)
                    'ArrListParamValues.Add(CType(l_datarow("Addr2"), String).Trim)
                    'ArrListParamValues.Add(CType(l_datarow("Addr3"), String).Trim)
                    'l_String_tmp = CType(l_datarow("City"), String).Trim + ", " + CType(l_datarow("StateType"), String).Trim + "   " + CType(l_datarow("zip"), String).Trim
                    'ArrListParamValues.Add(CType(l_String_tmp, String).Trim)
                    ''Priya 19-Feb-2009 : Added trim to name as it displays spaces between name in report. 
                    'l_String_tmp = CType(l_datarow("FirstName"), String).Trim + " "
                    'If l_datarow("MiddleName").GetType.ToString().Trim = "System.DBNull" Or CType(l_datarow("MiddleName"), String).Trim = "" Then
                    '    l_String_tmp = l_String_tmp + CType(l_datarow("LastName"), String).Trim
                    'Else
                    '    l_String_tmp = l_String_tmp + CType(l_datarow("MiddleName"), String).Trim.Substring(0, 1) + ". "
                    '    l_String_tmp = l_String_tmp + CType(l_datarow("LastName"), String).Trim
                    '    'End 19-Feb-2009
                    'End If
                    'ArrListParamValues.Add(CType(l_String_tmp, String).Trim)

                    ''Priya 19-Feb-2009 : Added trim to SSNO and added if condition to check null SSNO and length of SSNO
                    'Dim l_string_SSNO As String
                    'l_string_SSNO = CType(l_datarow("SSNo"), String).Trim
                    'If l_string_SSNO <> String.Empty AndAlso l_string_SSNO.Length = 9 Then
                    '    l_String_tmp = l_string_SSNO.Substring(0, 3) + "-"
                    '    l_String_tmp += l_string_SSNO.Substring(3, 2) + "-"
                    '    l_String_tmp += l_string_SSNO.Substring(5, 4)
                    'Else
                    '    l_String_tmp = l_string_SSNO
                    'End If

                    ''l_String_tmp = CType(l_datarow("SSNo"), String).Substring(0, 3) + "-"
                    ''l_String_tmp += CType(l_datarow("SSNo"), String).Substring(3, 2) + "-"
                    ''l_String_tmp += CType(l_datarow("SSNo"), String).Substring(5, 4)
                    ''End 19-Feb-2009


                    'ArrListParamValues.Add(CType(l_String_tmp, String).Trim)

                    'ArrListParamValues.Add(CType(l_datarow("YmcaNo"), String).Trim)
                    'l_String_tmp = " "

                    'ArrListParamValues.Add(CType(l_String_tmp, String).Trim)

                    ' 'End comment

                    'l_String_tmp = System.DateTime.Now.AddMonths(1).Now.ToString("MMMM") + " 01, " + System.DateTime.Now.AddYears(1).Year().ToString()
                    'ArrListParamValues.Add(l_String_tmp.Trim)
                    'By aparna -YREN-3027 

                Case "ReleaseBlankLess1K", "ReleaseBlank1kto5k", "RL_2_FullorPartialRefund", "HardshipWithDrawalofTDForm", "RL_3_FullPIAOver15000", "RL_2_FullorPartialRefund", "RL_2_FullorPartialRefund", "RL_2_FullorPartialRefund", "RL_3_FullPIAOver15000", "DeferredAssociationAnnuityAgreementForm", "RL_6a_Over70PIAOver15000", "RL_6c_Over70PIAUnder5000", "RL_6b_Over70PIA5000-15000", "HWL_4_TDOnly", "HWL_5_TDAndOtherAcc"
                    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    boollogontoDB = True
                Case "ReleaseBlankOver5k"

                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    ArrListParamValues.Add(CType(Session("R_minTax"), String).ToString.Trim)

                    'Case "ReleaseBlankLess1K"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    'Case "ReleaseBlank1kto5k"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    'Case "ReleaseBlankOver5k"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    'Case "RL_2_FullorPartialRefund"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    'Case "HardshipWithDrawalofTDForm"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    'Case " RL_3_FullPIAOver15000"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    'Case " RL_2_FullorPartialRefund"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    'Case "RL_2_FullorPartialRefund"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    'Case "RL_2_FullorPartialRefund"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    'Case " RL_3_FullPIAOver15000"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    'Case " DeferredAssociationAnnuityAgreementForm"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    'Case " RL_6a_Over70PIAOver15000"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    'Case "RL_6c_Over70PIAUnder5000"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)

                    'Case "RL_6b_Over70PIA5000"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)



                    'By Aparna done on 13th april

                Case "YMCA ACH Report" 'Aparna 7thNov-2006
                    boollogontoDB = True
                    ArrListParamValues = CType(Session("arrListParaColl"), ArrayList)
                    '  Session("arrListParaColl") = Nothing
                    'Case "YMCA ACH Report" 'Aparna 7thNov-2006
                    '    boollogontoDB = True
                    '    ArrListParamValues = CType(Session("arrListParaColl"), ArrayList)
                    '    Session("arrListParaColl") = Nothing

                    'Aparna -27/09/2006 Delinquency Reports
                Case "DelinquencyReport"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("LoginId"), String).ToString.Trim)
                    'aparna-19/12/2006
                    ' ArrListParamValues.Add(CType(Session("LoggedUserKey"), String).ToString.Trim)
                    ArrListParamValues.Add(CType(Session("HostName"), String).ToString.Trim)
                    'APARNA -including  new parameter -19/12/2006
                    ArrListParamValues.Add(CType(Session("16 days letter"), String).ToString.Trim)


                    'Aparna Samala -29/11/2006 -Cashout Report
                Case "Cash Out"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("CashoutBatchId"), String).ToString.Trim)
                    'Added By SG: 2012.01.04: BT-1511
                    ArrListParamValues.Add(ddlReportOption.SelectedValue)
                    'By Aparna - as the parameter is removed from the report-14/12/2006
                    'ArrListParamValues.Add(CType(Session("CashOutRangeDesc"), String).ToString.Trim)
                    'Added By Ashutosh Patil as on 19-01-2007  for YREN - 3023   
                    'Start Ashutosh Patil
                    'AA:26.06.2015 BT:2900 YRS 5.0-2533:Modified letter for default and ofset loans
                Case "LoanDefaultLetter"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("FundNo"), String).ToString.Trim)
                    'START |  SR | 2016.04.29 | YRS-AT-2909 - Support Request: printing crystal report com exception LoanDefaultLetter.rpt 
                    'ArrListParamValues.Add(Session("DefaultDate"))
                    ArrListParamValues.Add(Convert.ToDateTime(Session("DefaultDate")).ToString("MM/dd/yyyy"))
                    'END |  SR | 2016.04.29 | YRS-AT-2909 - Support Request: printing crystal report com exception LoanDefaultLetter.rpt 
                Case "Military-part"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("perseID"), String).ToString.Trim)
                    ' Start | SC | 2020.04.10 | YRS-AT-4852 | Added new loan suspend report for participant as per COVID Care act
                Case "Loan_Suspension_Ltr_to_Part-COVID-19"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("perseID"), String).ToString.Trim)
                    ' End | SC | 2020.04.10 | YRS-AT-4852 | Added new loan suspend report for participant as per COVID Care act
                Case "Loan Satisfied-part"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("perseID"), String).ToString.Trim)
                    ArrListParamValues.Add(CType(Session("OrigLoanNo"), String).ToString.Trim) 'Bala: 22.04.2016: YRS-AT-2667: Passing loan number which is taken from chvLoanNumber column in AtsLoanDetails table.
                Case "EDILogReport"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("guiProcessId"), String).ToString.Trim)

                    'Added by Ashish 17-Mar-2009 for Issue YRS 5.0 679
                Case "Loan Reamortization Letters"
                    Dim l_ArrListReamortizedPara As ArrayList
                    boollogontoDB = True
                    If Not Session("ReAmortizeRptParameterForParticipant") Is Nothing Then
                        l_ArrListReamortizedPara = CType(Session("ReAmortizeRptParameterForParticipant"), ArrayList)
                        If l_ArrListReamortizedPara.Count = 2 Then
                            ArrListParamValues.Add(Convert.ToInt32(l_ArrListReamortizedPara.Item(0)))
                            ArrListParamValues.Add(Convert.ToInt16(l_ArrListReamortizedPara.Item(1)))
                        End If

                    End If
                    'Added by prasad YRS 5.0-1561: Add 'Print Letter' and 'Reset Account' buttons to Web info display page
                Case "WebIdLetterOleN"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("Person_Info"), String).ToString.Trim)
                    'SR:21.09.2012 - BT:1060/YRS 5.0-YRS 5.0-1346: Open Form and Letter on completion of QDRO beneficiary settlement.
                Case "RL_2_FullRefundQDRO"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                    'Added by Anudeep:07.11.2012 - BT:957/YRS 5.0-1484: Termination Watcher Processed Records Report 
                Case "TerminationWatcher"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("intProcessID"), String).ToString.Trim)
                    'Added By SG: 2012.12.05: BT-1436
                Case "Withdrawals_New"
                    If strModuleName = "QDRONonRet" Then
                        boollogontoDB = True
                        ArrListParamValues.Add(CType(Session("RefRequestsID"), String).ToString.Trim)
                        ArrListParamValues.Add("QID")
                        'Added By SG: 2012.12.17: BT-1511: for viewing form\letters
                    ElseIf strModuleName = "CashOut" Then
                        boollogontoDB = True
                        ArrListParamValues.Add(CType(Session("PrintBatchID"), String).ToString.Trim)
                        ArrListParamValues.Add("BID")
                        'END: Added By SG: 2012.12.17: BT-1511
                    End If
                    'END: Added By SG: 2012.12.05: BT-1436
                    'SR:2012.12.20 - YRS 5.0-1707- New death benefit application report
                    'SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
                    'AA:2013.08.13 - Bt-1512 :YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
                Case "Death Benefit Application", "Death Letter for all beneficiaries", "Death Letter for all beneficiaries 60 day followup", "Death Letter for all beneficiaries 90 day followup"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("intDBAppFormID"), String).ToString.Trim)
                    'Dineshk                2013.09.24          Bt-2139 :YRS 5.0-2165:RMD enhancements 
                Case "Initial_RMD_Letter_April_Distribution_Deadline"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("RMDFundNo"), String).ToString.Trim)
                    'Start:Dinesh K               2014.06.10          BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
                Case "RMD cover letter for check April Deadline"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("RMDFundNo"), String).ToString.Trim)
                Case "December RMD letter for check"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("RMDFundNo"), String).ToString.Trim)
                    ArrListParamValues.Add(CType(Session("RMDYear"), String).ToString.Trim)
                Case "RMD Initial Letter", "RMD Followup Letter"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("RMDBatchFundNo"), String).ToString.Trim)
                    'End:Dinesh K               2014.06.10          BT:2434:YRS 5.0-2315 - RMD additional modifications to processing pages.
                    'Start:AA:26.05.2014 BT-1051:YRS 5.0-1618 Added for showing report for rollin
                Case "Letter of Acceptance"
                    boollogontoDB = True
                    Dim lstAttributes(6) As String
                    lstAttributes = Session("RollInLetter")
                    For Each strAtrribute As String In lstAttributes
                        ArrListParamValues.Add(strAtrribute)
                    Next
                    'End:AA:26.05.2014 BT-1051:YRS 5.0-1618 Added for showing report for rollin
                    'Start:JB:16.04.2015 BT-2570:YRS 5.0-2380 Added for showing report of initial communication popup letter
                Case "DB_Initial PopUp Letter"
                    boollogontoDB = True
                    Dim lstAttributes(1) As String
                    lstAttributes = SessionAnnuityBeneficiaryDeath.AnnBeneDeathInitialLetterParams
                    For Each strAtrribute As String In lstAttributes
                        ArrListParamValues.Add(strAtrribute)
                    Next
                    'End:JB:16.04.2015 BT-2570:YRS 5.0-2380 Added for showing report of initial communication popup letter
                    'Start:Dinesh Kanojia         2015.06.15        BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
                Case "Withdrawals_PortableProject"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("SpecialPrintBatchID"), String).ToString.Trim)
                    ArrListParamValues.Add("BID")
                    'End:Dinesh Kanojia         2015.06.15        BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
                    'Start:AA:07.03.2016 YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
                Case "New Cashout Letter"
                    boollogontoDB = True
                    ArrListParamValues.Add("")
                    ArrListParamValues.Add(CType(Session("PrintBatchID"), String).ToString.Trim)
                    'End:AA:07.03.2016 YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
                    'START : SR | 2018.04.25 | YRS-AT-3101 | New report added to display disbursements ready for approval/ rejections. 
                Case "PendingEFTDisbursements"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("DisbType"))
                Case "ConfirmEFTDisbursements"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("DisbType"))
                    'END : SR | 2018.04.25 | YRS-AT-3101 | New report added to display disbursements ready for approval/ rejections. 
                Case Else

            End Select

            If sReportName.Trim <> String.Empty Then
                LoadReport(ArrListParamValues, sReportName, boollogontoDB)
            End If

        Catch ' ex As Exception
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            Throw
        End Try

    End Function
    Private Function LoadReport(ByVal ArrListParamValues As ArrayList, ByVal sReportName As String, ByVal logontodb As Boolean) As Boolean
        'Dim objRpt As New cryst  
        Dim crCon As New ConnectionInfo
        Dim CrTableLogonInfo As New TableLogOnInfo
        Dim CrTables As Tables
        Dim CrTable As Table
        Dim TableCounter As Integer
        Dim DataSource As String
        Dim DatabaseName As String
        Dim UserID As String
        Dim Password As String
        Dim strXml As String
        Dim sReportPath As String
        Dim paramItem As String
        Try
            DataSource = ConfigurationSettings.AppSettings("DataSource")
            DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            UserID = ConfigurationSettings.AppSettings("UserID")
            Password = ConfigurationSettings.AppSettings("Password")
            sReportPath = ConfigurationSettings.AppSettings("ReportPath")
            sReportPath = sReportPath.Trim + "\\" + sReportName.Trim + ".rpt"

            objRpt.Load(sReportPath)
            'Start Code - Added by hafiz on 10Apr2006
            objRpt.Refresh()
            'End Code - Added by hafiz on 10Apr2006
            CrystalReportViewer1.ReportSource = objRpt
            If ArrListParamValues.Count > 0 Then
                Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
                Dim paramField As CrystalDecisions.Shared.ParameterField
                Dim curValues As New CrystalDecisions.Shared.ParameterValues
                Dim discreteValue As New CrystalDecisions.Shared.ParameterDiscreteValue

                Dim i As Integer
                i = 0

                For i = 0 To ArrListParamValues.Count - 1

                    curValues = New CrystalDecisions.Shared.ParameterValues
                    discreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    paramItem = ArrListParamValues.Item(i)
                    paramField = paramFieldsCollection(i)
                    curValues = paramField.CurrentValues
                    'discreteValue.Value = paramItem.ToString
                    'YRS 5.0-1365 : Need new batch processing option - Start
                    'Handle passing of multiple discrete values here
                    Dim multipleParams As String() = paramItem.Split("|")
                    For j = 0 To multipleParams.Count - 1
                        Dim discrete As New ParameterDiscreteValue()
                        discrete.Value = multipleParams(j)
                        curValues.Add(discrete)
                    Next
                    'YRS 5.0-1365 : Need new batch processing option -End
                    'curValues.Add(discreteValue)
                    'paramField.CurrentValues = curValues
                    objRpt.ParameterFields(i).CurrentValues = curValues
                Next

                'CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
            End If
            'Start Code - Added by Aparna on 17th Apr 2006
            'objRpt.Refresh()
            'End Code - Added by Aparna on 17th Apr 2006

            If logontodb Then
                CrTables = objRpt.Database.Tables

                crCon.ServerName = DataSource
                crCon.DatabaseName = DatabaseName
                crCon.UserID = UserID
                crCon.Password = Password

                For Each CrTable In CrTables
                    CrTableLogonInfo = CrTable.LogOnInfo
                    CrTableLogonInfo.ConnectionInfo = crCon
                    CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                    CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
                Next
                'start - Manthan Rajguru | 2016.05.05 | YRS-AT-2909 | Subreport dB tables binding
                For iSubCount As Integer = 0 To objRpt.Subreports.Count - 1
                    CrTables = objRpt.Subreports(iSubCount).Database.Tables                   
                    For Each CrTable In CrTables
                        CrTableLogonInfo = CrTable.LogOnInfo
                        CrTableLogonInfo.ConnectionInfo = crCon
                        CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                        CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
                    Next
                Next
                'start - Manthan Rajguru | 2016.05.05 | YRS-AT-2909 | Subreport dB tables binding
            End If

            CrystalReportViewer1.DataBind()
            'CrystalReportViewer1.RefreshReport()

        Catch
            Throw
        End Try
    End Function

    Public Function getYRSParticipantHistoryReportBySSNo(ByVal sReportName As String, ByVal ListBoxSelectedItems As System.Web.UI.WebControls.ListBox)
        'Put user code to initialize the page here
        Try
            'by aparna

            'Dim ch As Checking = New Checking
            Dim sReportPath As String
            sReportPath = Server.MapPath("/YMCAUI") + "\Reports\" + sReportName + ".rpt"
            sReportPath = ConfigurationSettings.AppSettings("ReportPath")
            sReportPath = sReportPath.Trim + "\\" + sReportName.Trim + ".rpt"
            Dim logonInfo As TableLogOnInfo = New TableLogOnInfo
            objRpt.Load(sReportPath)


            'Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
            'Dim LstBox As New ListBox
            'Dim ch As Checking = New Checking
            'Dim sReportPath As String
            'sReportPath = Server.MapPath("/YMCAUI") + "\Reports\" + sReportName + ".rpt"
            ''sReportPath = "c:\inetpub\wwwroot\YMCAUI\Reports\" + sReportName
            ''objRpt = New YRSParticipantHistoryReportBySSNo
            'Dim logonInfo As TableLogOnInfo = New TableLogOnInfo
            'objRpt.Load(sReportPath)
            'Dim l_objRpt As New YRSParticipantHistoryReportBySSNo


            'by aparna
            Dim crCon As New ConnectionInfo
            Dim CrTableLogonInfo As New TableLogOnInfo
            Dim CrTables As Tables
            Dim CrTable As Table
            Dim TableCounter As Integer
            Dim DataSource As String
            Dim DatabaseName As String
            Dim UserID As String
            Dim Password As String
            Dim strXml As String
            DataSource = ConfigurationSettings.AppSettings("DataSource")
            DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            UserID = ConfigurationSettings.AppSettings("UserID")
            Password = ConfigurationSettings.AppSettings("Password")

            CrystalReportViewer1.ReportSource = objRpt



            Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
            Dim paramFieldSSNo As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
            Dim curValuesSSNo As CrystalDecisions.Shared.ParameterValues = paramFieldSSNo.CurrentValues
            Dim discreteValueSSNo As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValueSSNo.Value = Session("SSNo") '"111111115"
            curValuesSSNo.Add(discreteValueSSNo)


            Dim paramFieldReportType As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(1)
            Dim curValuesReportType As CrystalDecisions.Shared.ParameterValues = paramFieldReportType.CurrentValues
            Dim i As Integer
            For i = 0 To ListBoxSelectedItems.Items.Count - 1
                Dim discreteValueReportType As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                discreteValueReportType.Value = ListBoxSelectedItems.Items(i).Text.ToString() '"Refunds PHR"
                curValuesReportType.Add(discreteValueReportType)
            Next
            CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection

            CrTables = objRpt.Database.Tables

            ''this should go into the dataconfiguration.config file 
            crCon.ServerName = DataSource
            crCon.DatabaseName = DatabaseName
            crCon.UserID = UserID
            crCon.Password = Password

            For Each CrTable In CrTables
                CrTableLogonInfo = CrTable.LogOnInfo
                CrTableLogonInfo.ConnectionInfo = crCon
                CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                'If your DatabaseName is changing at runtime, specify 
                'the table location. 
                'For example, when you are reporting off of a 
                'Northwind database on SQL server you 
                'should have the following line of code: 
                '---------------------------------------------------
                ''''this should also get into the dataconfiguration.config file "YRS.dbo." 
                '---------------------------------------------------
                'CrTable.Location = "YRS.dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
                CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
            Next
            CrystalReportViewer1.DataBind()
            ''CrystalReportViewer1.HasDrillUpButton = False
            ''CrystalReportViewer1.HasGotoPageButton = False
            ''CrystalReportViewer1.HasPageNavigationButtons = False
            ''CrystalReportViewer1.HasRefreshButton = False
            ''CrystalReportViewer1.HasSearchButton = False
            ''CrystalReportViewer1.HasZoomFactorList = False
            ''CrystalReportViewer1.BestFitPage = True
            ''CrystalReportViewer1.PageZoomFactor = 120
            'l_objRpt.Close()
            'l_objRpt.Dispose()

        Catch 'ex As Exception
            '    Dim l_String_Exception_Message As String
            '    l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            '    Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
            Throw
        End Try
    End Function
    Public Function getReport(ByVal sReportName As String) As Boolean
        Try
            Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
            'Dim ch As Checking = New Checking
            Dim sReportPath As String
            sReportPath = Server.MapPath("/YMCAUI") + "\Reports\" + sReportName + ".rpt"

            'sReportPath = "c:\inetpub\wwwroot\YMCAUI\Reports\" + sReportName
            'objRpt = New YRSParticipantHistoryReportBySSNo
            Dim logonInfo As TableLogOnInfo = New TableLogOnInfo
            objRpt.Load(sReportPath)
            ' Set the logon information for each table.
            Dim DataSource As String
            Dim DatabaseName As String
            Dim UserID As String
            Dim Password As String
            Dim strXml As String
            DataSource = ConfigurationSettings.AppSettings("DataSource")
            DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            UserID = ConfigurationSettings.AppSettings("UserID")
            Password = ConfigurationSettings.AppSettings("Password")
            For Each table1 As CrystalDecisions.CrystalReports.Engine.Table In objRpt.Database.Tables
                ' Get the TableLogOnInfo object.
                logonInfo = table1.LogOnInfo
                ' Set the server or ODBC data source name, database name,
                ' user ID, and password.yrs
                logonInfo.ConnectionInfo.ServerName = DataSource
                logonInfo.ConnectionInfo.DatabaseName = DatabaseName
                logonInfo.ConnectionInfo.UserID = UserID
                logonInfo.ConnectionInfo.Password = Password
                ' Apply the connection information to the table.
                table1.ApplyLogOnInfo(logonInfo)
                table1.Location = DatabaseName & ".dbo." & table1.Location.Substring(table1.Location.LastIndexOf(".") + 1)
            Next
            'start - Manthan Rajguru | 2016.05.05 | YRS-AT-2909 | Subreport dB tables binding
            Dim crCon As ConnectionInfo
            Dim CrTables As Tables

            For iSubCount As Integer = 0 To objRpt.Subreports.Count - 1
                CrTables = objRpt.Subreports(iSubCount).Database.Tables
                crCon.ServerName = DataSource
                crCon.DatabaseName = DatabaseName
                crCon.UserID = UserID
                crCon.Password = Password
                For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
                    logonInfo = CrTable.LogOnInfo
                    logonInfo.ConnectionInfo = crCon
                    CrTable.ApplyLogOnInfo(logonInfo)
                    CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)                   
                Next
            Next
            'End - Manthan Rajguru | 2016.05.05 | YRS-AT-2909 | Subreport dB tables binding
            'If ParameterFieldsColl.Count > 0 Then
            '    CrystalReportViewer1.ParameterFieldInfo = ParameterFieldsColl
            'End If
            CrystalReportViewer1.ReportSource = objRpt
            CrystalReportViewer1.DataBind()
            CrystalReportViewer1.RefreshReport()
        Catch 'ex As Exception
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
            Throw
        End Try
    End Function
    'Private Sub getDeathBenefitOptions(ByVal sReportName As String)
    '    '*******************************************************************************
    '    ' Called From       :	DeathBenefitsCalculatorForm.aspx.vb
    '    ' Author Name		:	Vipul Patel 
    '    ' Employee ID		:	32900 
    '    ' Email				:	vipul.patel@3i-infotech.com
    '    ' Creation Time		:	11/14/2005 
    '    ' Description		:	This form is used to View Death Benefits Options Report
    '    '*******************************************************************************

    '    Try
    '        Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    '        'Dim ch As Checking = New Checking
    '        Dim sReportPath As String
    '        sReportPath = Server.MapPath("/YMCAUI") + "\Reports\" + sReportName + ".rpt"

    '        Dim logonInfo As TableLogOnInfo = New TableLogOnInfo
    '        objRpt.Load(sReportPath)
    '        ' Set the logon information for each table.
    '        Dim DataSource As String
    '        Dim DatabaseName As String
    '        Dim UserID As String
    '        Dim Password As String
    '        Dim strXml As String
    '        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '        UserID = ConfigurationSettings.AppSettings("UserID")
    '        Password = ConfigurationSettings.AppSettings("Password")

    '        'Passing the Parameter
    '        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '        Dim paramFieldSSNo As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '        Dim curValuesSSNo As CrystalDecisions.Shared.ParameterValues = paramFieldSSNo.CurrentValues
    '        Dim discreteValueSSNo As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '        discreteValuePersID.Value = Session("PersID")
    '        curValuesSSNo.Add(discreteValuePersID)


    '        For Each table1 As CrystalDecisions.CrystalReports.Engine.Table In objRpt.Database.Tables
    '            ' Get the TableLogOnInfo object.
    '            logonInfo = table1.LogOnInfo
    '            ' Set the server or ODBC data source name, database name, user ID & password.yrs
    '            logonInfo.ConnectionInfo.ServerName = DataSource
    '            logonInfo.ConnectionInfo.DatabaseName = DatabaseName
    '            logonInfo.ConnectionInfo.UserID = UserID
    '            logonInfo.ConnectionInfo.Password = Password
    '            ' Apply the connection information to the table.
    '            table1.ApplyLogOnInfo(logonInfo)
    '            table1.Location = DatabaseName & ".dbo." & table1.Location.Substring(table1.Location.LastIndexOf(".") + 1)
    '        Next
    '        'If ParameterFieldsColl.Count > 0 Then
    '        '    CrystalReportViewer1.ParameterFieldInfo = ParameterFieldsColl
    '        'End If
    '        CrystalReportViewer1.ReportSource = objRpt
    '        CrystalReportViewer1.DataBind()
    '        CrystalReportViewer1.RefreshReport()
    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Sub
    'Public Function getReport(ByVal sReportName As String, ByVal ParameterFieldsColl As CrystalDecisions.Shared.ParameterFields) As Boolean
    '    Try
    '        Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    '        'Dim ch As Checking = New Checking
    '        Dim sReportPath As String
    '        sReportPath = Server.MapPath("/YMCAUI") + "\Reports\" + sReportName + ".rpt"
    '        'sReportPath = "c:\inetpub\wwwroot\YMCAUI\Reports\" + sReportName
    '        'objRpt = New YRSParticipantHistoryReportBySSNo
    '        Dim logonInfo As TableLogOnInfo = New TableLogOnInfo
    '        objRpt.Load(sReportPath)
    '        ' Set the logon information for each table.
    '        Dim DataSource As String
    '        Dim DatabaseName As String
    '        Dim UserID As String
    '        Dim Password As String
    '        Dim strXml As String
    '        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '        UserID = ConfigurationSettings.AppSettings("UserID")
    '        Password = ConfigurationSettings.AppSettings("Password")
    '        'strXml = System.Configuration.ConfigurationSettings.GetConfig("configurationSection")
    '        For Each table1 As CrystalDecisions.CrystalReports.Engine.Table In objRpt.Database.Tables
    '            ' Get the TableLogOnInfo object.
    '            logonInfo = table1.LogOnInfo
    '            ' Set the server or ODBC data source name, database name,
    '            ' user ID, and password.yrs
    '            logonInfo.ConnectionInfo.ServerName = DataSource
    '            logonInfo.ConnectionInfo.DatabaseName = DatabaseName
    '            logonInfo.ConnectionInfo.UserID = UserID
    '            logonInfo.ConnectionInfo.Password = Password
    '            ' Apply the connection information to the table.
    '            table1.ApplyLogOnInfo(logonInfo)
    '            table1.Location = DataSource & ".dbo." & table1.Location.Substring(table1.Location.LastIndexOf(".") + 1)
    '        Next
    '        If ParameterFieldsColl.Count > 0 Then
    '            CrystalReportViewer1.ParameterFieldInfo = ParameterFieldsColl
    '        End If
    '        CrystalReportViewer1.ReportSource = objRpt
    '        CrystalReportViewer1.DataBind()
    '        CrystalReportViewer1.RefreshReport()

    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = ex.Message.Trim.ToString()
    '        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '        'Throw ex
    '    End Try
    'End Function
    Public Function getRetireeDeathBenefitsClaimForm(ByVal sReportName As String)
        '*******************************************************************************
        ' Called From       :	DeathBenefitsCalculatorForm.aspx.vb
        ' Author Name		:	Vipul Patel 
        ' Employee ID		:	32900 
        ' Email				:	vipul.patel@3i-infotech.com
        ' Creation Time		:	11/14/2005 
        ' Description		:	This form is used to print Forms
        '*******************************************************************************

        Try
            'start code - by aparna on 19th april
            'Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
            'Dim ch As Checking = New Checking
            Dim sReportPath As String
            sReportPath = ConfigurationSettings.AppSettings("ReportPath")
            sReportPath = sReportPath.Trim + "\\" + sReportName.Trim + ".rpt"
            Dim logonInfo As TableLogOnInfo = New TableLogOnInfo
            objRpt.Load(sReportPath)
            'end code - by aparna on 19th april

            'commented by aparna on 19th april
            'Dim objRpt As New RetireeDeathBenefitsClaimForm
            Dim crCon As New ConnectionInfo
            Dim CrTableLogonInfo As New TableLogOnInfo
            Dim CrTables As Tables
            Dim CrTable As Table
            Dim TableCounter As Integer
            Dim DataSource As String
            Dim DatabaseName As String
            Dim UserID As String
            Dim Password As String
            Dim strXml As String
            DataSource = ConfigurationSettings.AppSettings("DataSource")
            DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            UserID = ConfigurationSettings.AppSettings("UserID")
            Password = ConfigurationSettings.AppSettings("Password")
            CrystalReportViewer1.ReportSource = objRpt

            Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
            Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
            Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
            Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue.Value = Session("Para1_MemberName")
            curValues.Add(discreteValue)

            Dim paramField1 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(1)
            Dim curValues1 As CrystalDecisions.Shared.ParameterValues = paramField1.CurrentValues
            Dim discreteValue1 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue1.Value = Session("Para2_BeneficiaryName")
            curValues1.Add(discreteValue1)


            'Dim paramField2 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(2)
            'Dim curValues2 As CrystalDecisions.Shared.ParameterValues = paramField2.CurrentValues
            'Dim discreteValue2 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            'discreteValue2.Value = Session("Para3_MemberSSNo")
            'curValues2.Add(discreteValue2)

            CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
            CrTables = objRpt.Database.Tables

            crCon.ServerName = DataSource
            crCon.DatabaseName = DatabaseName
            crCon.UserID = UserID
            crCon.Password = Password

            For Each CrTable In CrTables
                CrTableLogonInfo = CrTable.LogOnInfo
                CrTableLogonInfo.ConnectionInfo = crCon
                CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
            Next
            CrystalReportViewer1.DataBind()

        Catch ex As SqlException
            Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try

    End Function
    Public Function getPreRetirementDeathBenefitsClaimForm(ByVal sReportName As String)
        '*******************************************************************************
        ' Called From       :	DeathBenefitsCalculatorForm.aspx.vb
        ' Author Name		:	Vipul Patel 
        ' Employee ID		:	32900 
        ' Email				:	vipul.patel@3i-infotech.com
        ' Creation Time		:	11/14/2005 
        ' Description		:	This form is used to print Forms
        '*******************************************************************************
        'Put user code to initialize the page here
        Try
            'Start- Added By Aparna on 18th April 2006
            'Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
            'Dim ch As Checking = New Checking
            Dim sReportPath As String
            sReportPath = ConfigurationSettings.AppSettings("ReportPath")
            sReportPath = sReportPath.Trim + "\\" + sReportName.Trim + ".rpt"
            Dim logonInfo As TableLogOnInfo = New TableLogOnInfo
            objRpt.Load(sReportPath)
            'End- Added By Aparna on 18th April 2006

            'commented By Aparna on 18th April 2006 due to some error in code
            'Dim objRpt As New Pre_RetirementDeathBenefitsClaimForm
            'commented By Aparna on 18th April 2006 due to some error in code
            Dim crCon As New ConnectionInfo
            Dim CrTableLogonInfo As New TableLogOnInfo
            Dim CrTables As Tables
            Dim CrTable As Table
            Dim TableCounter As Integer
            Dim DataSource As String
            Dim DatabaseName As String
            Dim UserID As String
            Dim Password As String
            Dim strXml As String
            DataSource = ConfigurationSettings.AppSettings("DataSource")
            DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            UserID = ConfigurationSettings.AppSettings("UserID")
            Password = ConfigurationSettings.AppSettings("Password")
            CrystalReportViewer1.ReportSource = objRpt

            Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
            Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
            Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
            Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue.Value = Session("Para1_MemberName") '  Session("Member Name")
            curValues.Add(discreteValue)

            Dim paramField1 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(1)
            Dim curValues1 As CrystalDecisions.Shared.ParameterValues = paramField1.CurrentValues
            Dim discreteValue1 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue1.Value = Session("Para2_BeneficiaryName") ' Session("Beneficiary Name")
            curValues1.Add(discreteValue1)

            Dim paramField2 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(2)
            Dim curValues2 As CrystalDecisions.Shared.ParameterValues = paramField2.CurrentValues
            Dim discreteValue2 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue2.Value = Session("Para3_lcAccountname1") ' Session("Reserves")
            curValues2.Add(discreteValue2)


            Dim paramField3 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(3)
            Dim curValues3 As CrystalDecisions.Shared.ParameterValues = paramField3.CurrentValues
            Dim discreteValue3 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue3.Value = Session("Para4_lcNonTax1")  ' Session("Resseves Non-Taxable")
            curValues3.Add(discreteValue3)


            Dim paramField4 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(4)
            Dim curValues4 As CrystalDecisions.Shared.ParameterValues = paramField4.CurrentValues
            Dim discreteValue4 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue4.Value = Session("Para5_lcTaxable1") 'Session("Reserves Taxable")
            curValues4.Add(discreteValue4)


            Dim paramField5 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(5)
            Dim curValues5 As CrystalDecisions.Shared.ParameterValues = paramField5.CurrentValues
            Dim discreteValue5 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue5.Value = Session("Para6_lcBalance1") 'Session("Reserves Balance")
            curValues5.Add(discreteValue5)

            Dim paramField6 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(6)
            Dim curValues6 As CrystalDecisions.Shared.ParameterValues = paramField6.CurrentValues
            Dim discreteValue6 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue6.Value = Session("Para7_lcAccountname2") 'Session("Death Benefit")
            curValues6.Add(discreteValue6)

            Dim paramField7 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(7)
            Dim curValues7 As CrystalDecisions.Shared.ParameterValues = paramField7.CurrentValues
            Dim discreteValue7 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue7.Value = Session("Para8_lcNonTax2") ' Session("DB Non-Taxable")
            curValues7.Add(discreteValue7)

            Dim paramField8 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(8)
            Dim curValues8 As CrystalDecisions.Shared.ParameterValues = paramField8.CurrentValues
            Dim discreteValue8 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue8.Value = Session("Para9_lcTaxable2") 'Session("DB Taxable")
            curValues8.Add(discreteValue8)

            Dim paramField9 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(9)
            Dim curValues9 As CrystalDecisions.Shared.ParameterValues = paramField9.CurrentValues
            Dim discreteValue9 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue9.Value = Session("Para10_lcBalance2")  ' Session("DB Balance")
            curValues9.Add(discreteValue9)

            Dim paramField10 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(10)
            Dim curValues10 As CrystalDecisions.Shared.ParameterValues = paramField10.CurrentValues
            Dim discreteValue10 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue10.Value = Session("Para11_lcAccountnameT")  'Session("Totals")
            curValues10.Add(discreteValue10)

            Dim paramField11 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(11)
            Dim curValues11 As CrystalDecisions.Shared.ParameterValues = paramField11.CurrentValues
            Dim discreteValue11 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue11.Value = Session("Para12_lcNonTaxT")  ' Session("Total Non-Taxable")
            curValues11.Add(discreteValue11)

            Dim paramField12 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(12)
            Dim curValues12 As CrystalDecisions.Shared.ParameterValues = paramField12.CurrentValues
            Dim discreteValue12 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue12.Value = Session("Para13_lcTaxableT")    '       Session("Total Taxable")
            curValues12.Add(discreteValue12)

            Dim paramField13 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(13)
            Dim curValues13 As CrystalDecisions.Shared.ParameterValues = paramField13.CurrentValues
            Dim discreteValue13 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue13.Value = Session("Para14_lcBalanceT")    '       Session("Total Balance")
            curValues13.Add(discreteValue13)

            CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
            CrTables = objRpt.Database.Tables

            ''''this should go into the dataconfiguration.config file 
            crCon.ServerName = DataSource
            crCon.DatabaseName = DatabaseName
            crCon.UserID = UserID
            crCon.Password = Password

            For Each CrTable In CrTables
                CrTableLogonInfo = CrTable.LogOnInfo
                CrTableLogonInfo.ConnectionInfo = crCon
                CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
            Next
            CrystalReportViewer1.DataBind()

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Function
    Public Function getDeathBenefitOptions(ByVal sReportName As String)
        '*******************************************************************************
        ' Called From       :	DeathBenefitsCalculatorForm.aspx.vb
        ' Author Name		:	Vipul Patel 
        ' Employee ID		:	32900 
        ' Email				:	vipul.patel@3i-infotech.com
        ' Creation Time		:	11/14/2005 
        ' Description		:	This form is used to View Death Benefits Options Report
        '*******************************************************************************

        Try
            'commented by aparna on 18th april
            'Dim objRpt As New DeathBenefitOptions

            Dim crCon As New ConnectionInfo
            Dim CrTableLogonInfo As New TableLogOnInfo
            Dim CrTables As Tables
            Dim CrTable As Table
            Dim TableCounter As Integer
            Dim DataSource As String
            Dim DatabaseName As String
            Dim UserID As String
            Dim Password As String
            Dim strXml As String
            DataSource = ConfigurationSettings.AppSettings("DataSource")
            DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            UserID = ConfigurationSettings.AppSettings("UserID")
            Password = ConfigurationSettings.AppSettings("Password")
            CrystalReportViewer1.ReportSource = objRpt

            Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
            Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
            Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
            Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue.Value = Session("PersID")
            curValues.Add(discreteValue)

            CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
            CrTables = objRpt.Database.Tables

            crCon.ServerName = DataSource
            crCon.DatabaseName = DatabaseName
            crCon.UserID = UserID
            crCon.Password = Password

            For Each CrTable In CrTables
                CrTableLogonInfo = CrTable.LogOnInfo
                CrTableLogonInfo.ConnectionInfo = crCon
                CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
            Next
            CrystalReportViewer1.DataBind()

        Catch ex As SqlException
            Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Function

    Public Function getPreNotificationReport(ByVal sReportName As String)
        '*******************************************************************************
        ' Called From       :	PreNotificatiion.aspx.vb
        ' Author Name		:	Ragesh 
        ' Employee ID		:	34231 
        ' Email				:	ragesh.vp@3i-infotech.com
        ' Creation Time		:	11/14/2005 
        ' Description		:	This form is used to View PreNotification Report
        '*******************************************************************************
        'Put user code to initialize the page here
        Try
            'commented by Aparna on 18th April 2006 due to problem-removed report frm project
            'Dim objRpt As New Pre_Notification
            'commented by Aparna on 18th April 2006 due to problem
            Dim crCon As New ConnectionInfo
            Dim CrTableLogonInfo As New TableLogOnInfo
            Dim CrTables As Tables
            Dim CrTable As Table
            Dim TableCounter As Integer
            Dim DataSource As String
            Dim DatabaseName As String
            Dim UserID As String
            Dim Password As String
            Dim strXml As String
            DataSource = ConfigurationSettings.AppSettings("DataSource")
            DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            UserID = ConfigurationSettings.AppSettings("UserID")
            Password = ConfigurationSettings.AppSettings("Password")
            CrystalReportViewer1.ReportSource = objRpt
            objRpt.Refresh()
            'Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
            'Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
            'Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
            'Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            'discreteValue.Value = Session("PersID")
            'curValues.Add(discreteValue)

            'CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
            CrTables = objRpt.Database.Tables

            crCon.ServerName = DataSource
            crCon.DatabaseName = DatabaseName
            crCon.UserID = UserID
            crCon.Password = Password

            For Each CrTable In CrTables
                CrTableLogonInfo = CrTable.LogOnInfo
                CrTableLogonInfo.ConnectionInfo = crCon
                CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
            Next
            CrystalReportViewer1.DataBind()
            CrystalReportViewer1.RefreshReport()

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Function
    Public Function getDisbursementRequestReport(ByVal sReportName As String)
        '*******************************************************************************
        ' Called From       :	PaymentManager.aspx.vb
        ' Author Name		:	Ragesh 
        ' Employee ID		:	34231 
        ' Email				:	ragesh.vp@3i-infotech.com
        ' Creation Time		:	11/14/2005 
        ' Description		:	This form is used to View Disbursement Request Report
        '*******************************************************************************
        'Put user code to initialize the page here
        Try
            'commented by aparna on 19th april
            'Dim objRpt As New Disbursement_Request
            Dim crCon As New ConnectionInfo
            Dim CrTableLogonInfo As New TableLogOnInfo
            Dim CrTables As Tables
            Dim CrTable As Table
            Dim TableCounter As Integer
            Dim DataSource As String
            Dim DatabaseName As String
            Dim UserID As String
            Dim Password As String
            Dim strXml As String
            DataSource = ConfigurationSettings.AppSettings("DataSource")
            DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            UserID = ConfigurationSettings.AppSettings("UserID")
            Password = ConfigurationSettings.AppSettings("Password")
            CrystalReportViewer1.ReportSource = objRpt
            'objRpt.Refresh()
            Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
            Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
            Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
            Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue.Value = "REF"
            curValues.Add(discreteValue)

            CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
            CrTables = objRpt.Database.Tables

            crCon.ServerName = DataSource
            crCon.DatabaseName = DatabaseName
            crCon.UserID = UserID
            crCon.Password = Password

            For Each CrTable In CrTables
                CrTableLogonInfo = CrTable.LogOnInfo
                CrTableLogonInfo.ConnectionInfo = crCon
                CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
            Next
            CrystalReportViewer1.DataBind()
            'CrystalReportViewer1.RefreshReport()

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Function
    Public Function getFirstAnnuityChecksRequestReport(ByVal sReportName As String)
        '*******************************************************************************
        ' Called From       :	PaymentManager.aspx.vb
        ' Author Name		:	Ragesh 
        ' Employee ID		:	34231 
        ' Email				:	ragesh.vp@3i-infotech.com
        ' Creation Time		:	11/14/2005 
        ' Description		:	This form is used to View First Annuity Checks RequestReport
        '*******************************************************************************
        'Put user code to initialize the page here
        Try
            'commented by aparna on 19th april
            'Dim objRpt As New First_Annuity_Checks_Request
            Dim crCon As New ConnectionInfo
            Dim CrTableLogonInfo As New TableLogOnInfo
            Dim CrTables As Tables
            Dim CrTable As Table
            Dim TableCounter As Integer
            Dim DataSource As String
            Dim DatabaseName As String
            Dim UserID As String
            Dim Password As String
            Dim strXml As String
            DataSource = ConfigurationSettings.AppSettings("DataSource")
            DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            UserID = ConfigurationSettings.AppSettings("UserID")
            Password = ConfigurationSettings.AppSettings("Password")
            CrystalReportViewer1.ReportSource = objRpt
            objRpt.Refresh()
            'Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
            'Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
            'Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
            'Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            'discreteValue.Value = "REF"
            'curValues.Add(discreteValue)

            'CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
            CrTables = objRpt.Database.Tables

            crCon.ServerName = DataSource
            crCon.DatabaseName = DatabaseName
            crCon.UserID = UserID
            crCon.Password = Password

            For Each CrTable In CrTables
                CrTableLogonInfo = CrTable.LogOnInfo
                CrTableLogonInfo.ConnectionInfo = crCon
                CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
            Next
            CrystalReportViewer1.DataBind()
            CrystalReportViewer1.RefreshReport()

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Function


    Public Function getMEPStatusUpdate(ByVal sReportName As String)
        '*******************************************************************************
        ' Called From       :	MonthEndProcessing 
        ' Author Name		:	Ragesh.v.p
        ' Employee ID		:	34231
        ' Email				:	ragesh.vp@3i-infotech.com
        ' Creation Time		:	11/25//2005 
        ' Description		:	Status Update
        '*******************************************************************************

        Try

            'Dim objRpt As New AccouTobeChanged
            Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
            Dim sReportPath As String
            sReportPath = ConfigurationSettings.AppSettings("ReportPath")
            sReportPath = sReportPath.Trim + "\\" + sReportName.Trim + ".rpt"
            Dim logonInfo As TableLogOnInfo = New TableLogOnInfo
            objRpt.Load(sReportPath)

            Dim crCon As New ConnectionInfo
            Dim CrTableLogonInfo As New TableLogOnInfo
            Dim CrTables As Tables
            Dim CrTable As Table
            Dim TableCounter As Integer
            Dim DataSource As String
            Dim DatabaseName As String
            Dim UserID As String
            Dim Password As String
            Dim strXml As String
            DataSource = ConfigurationSettings.AppSettings("DataSource")
            DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            UserID = ConfigurationSettings.AppSettings("UserID")
            Password = ConfigurationSettings.AppSettings("Password")
            CrystalReportViewer1.ReportSource = objRpt

            Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
            Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
            Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
            Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue.Value = Convert.ToDateTime(Session("MonthEndClosingDate"))
            curValues.Add(discreteValue)

            CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
            CrTables = objRpt.Database.Tables

            crCon.ServerName = DataSource
            crCon.DatabaseName = DatabaseName
            crCon.UserID = UserID
            crCon.Password = Password

            For Each CrTable In CrTables
                CrTableLogonInfo = CrTable.LogOnInfo
                CrTableLogonInfo.ConnectionInfo = crCon
                CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
            Next
            CrystalReportViewer1.DataBind()
            CrystalReportViewer1.RefreshReport()

        Catch ex As SqlException
            Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("..\ErrorPageForm.aspx?FormType=Popup&Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Function

    Private Sub CrystalReportViewer1_Init(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub
    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        CrystalReportViewer1.ShowLastPage()
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        ''ExportToDisk("PHR")
    End Sub
    ''''Private Sub ExportToDisk(ByVal fileName As String)
    ''''    ' Declare variables and get the export options.
    ''''    Dim ListBoxSelectedItems As New System.Web.UI.WebControls.ListBox
    ''''    ListBoxSelectedItems = Session("ListBoxSelectedItems")
    ''''    Dim exportOpts As New ExportOptions
    ''''    Dim diskOpts As New DiskFileDestinationOptions
    ''''    Dim FPath As String
    ''''    Dim crDiskFileDestinationOptions As DiskFileDestinationOptions

    ''''    objRpt = New YRSParticipantHistoryReportBySSNo
    ''''    Dim crCon As New ConnectionInfo
    ''''    Dim CrTableLogonInfo As New TableLogOnInfo
    ''''    Dim CrTables As Tables
    ''''    Dim CrTable As Table
    ''''    Dim TableCounter As Integer
    ''''    Dim DataSource As String
    ''''    Dim DatabaseName As String
    ''''    Dim UserID As String
    ''''    Dim Password As String
    ''''    Dim strXml As String

    ''''    DataSource = ConfigurationSettings.AppSettings("DataSource")
    ''''    DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    ''''    UserID = ConfigurationSettings.AppSettings("UserID")
    ''''    Password = ConfigurationSettings.AppSettings("Password")
    ''''    CrystalReportViewer1.ReportSource = objRpt

    ''''    Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    ''''    Dim paramFieldSSNo As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    ''''    Dim curValuesSSNo As CrystalDecisions.Shared.ParameterValues = paramFieldSSNo.CurrentValues
    ''''    Dim discreteValueSSNo As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    ''''    discreteValueSSNo.Value = Session("SSNo") '"111111115"
    ''''    curValuesSSNo.Add(discreteValueSSNo)


    ''''    Dim paramFieldReportType As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(1)
    ''''    Dim curValuesReportType As CrystalDecisions.Shared.ParameterValues = paramFieldReportType.CurrentValues
    ''''    Dim i As Integer
    ''''    For i = 0 To ListBoxSelectedItems.Items.Count - 1
    ''''        Dim discreteValueReportType As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    ''''        discreteValueReportType.Value = ListBoxSelectedItems.Items(i).Text.ToString() '"Refunds PHR"
    ''''        curValuesReportType.Add(discreteValueReportType)
    ''''    Next

    ''''    CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    ''''    CrTables = objRpt.Database.Tables

    ''''    ''''this should go into the dataconfiguration.config file 
    ''''    crCon.ServerName = DataSource
    ''''    crCon.DatabaseName = DatabaseName
    ''''    crCon.UserID = UserID
    ''''    crCon.Password = Password

    ''''    For Each CrTable In CrTables
    ''''        CrTableLogonInfo = CrTable.LogOnInfo
    ''''        CrTableLogonInfo.ConnectionInfo = crCon
    ''''        CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    ''''        'If your DatabaseName is changing at runtime, specify 
    ''''        'the table location. 
    ''''        'For example, when you are reporting off of a 
    ''''        'Northwind database on SQL server you 
    ''''        'should have the following line of code: 
    ''''        '---------------------------------------------------
    ''''        ''''this should also get into the dataconfiguration.config file "YRS.dbo." 
    ''''        '---------------------------------------------------
    ''''        'CrTable.Location = "YRS.dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    ''''        CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    ''''    Next
    ''''    CrystalReportViewer1.DataBind()

    ''''    CrystalReportViewer1.HasDrillUpButton = False
    ''''    CrystalReportViewer1.HasGotoPageButton = False
    ''''    CrystalReportViewer1.HasPageNavigationButtons = True
    ''''    CrystalReportViewer1.HasRefreshButton = False
    ''''    CrystalReportViewer1.HasSearchButton = False
    ''''    CrystalReportViewer1.HasZoomFactorList = False
    ''''    CrystalReportViewer1.BestFitPage = True
    ''''    CrystalReportViewer1.PageZoomFactor = 100

    ''''    fileName = Server.MapPath("/YRS/Reports/") + fileName + ".pdf"
    ''''    '''FPath = Server.MapPath("/PDF_Reports/")

    ''''    ' Set the export format.
    ''''    ''exportOpts.ExportFormatType = _
    ''''    ''ExportFormatType.PortableDocFormat
    ''''    ''exportOpts.ExportDestinationType = _
    ''''    ''ExportDestinationType.DiskFile
    ''''    crDiskFileDestinationOptions = New DiskFileDestinationOptions
    ''''    crDiskFileDestinationOptions.DiskFileName = fileName
    ''''    exportOpts = objRpt.ExportOptions

    ''''    With exportOpts
    ''''        .DestinationOptions = crDiskFileDestinationOptions
    ''''        .ExportDestinationType = ExportDestinationType.DiskFile
    ''''        .ExportFormatType = ExportFormatType.PortableDocFormat
    ''''    End With
    ''''    objRpt.Export()
    ''''    ' The following code writes the pdf file 
    ''''    ' to the Client’s browser.
    ''''    Response.ClearContent()
    ''''    Response.ClearHeaders()
    ''''    Response.ContentType = "application/pdf"
    ''''    Response.WriteFile(fileName)
    ''''    Response.Flush()
    ''''    Response.Close()

    ''''    ' delete the exported file from disk
    ''''    System.IO.File.Delete(fileName)

    ''''    ' Set the disk file options.
    ''''    'diskOpts.DiskFileName = "C:\" + fileName
    ''''    'exportOpts.DestinationOptions = diskOpts
    ''''    ' Export the report.

    ''''End Sub
    Private Sub btnFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        CrystalReportViewer1.ShowFirstPage()
    End Sub
    Private Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
        CrystalReportViewer1.ShowNextPage()
    End Sub
    Private Sub btnPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrevious.Click
        CrystalReportViewer1.ShowPreviousPage()
    End Sub
#Region "All Report Methods"
    '''Public Function getAccouTobeChanged(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New AccouTobeChanged
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("@PeriodEnd")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function

    '''Public Function getbirefltr(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New birefltr
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("Contact")
    '''        curValues.Add(discreteValue)


    '''        Dim paramField1 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(1)
    '''        Dim curValues1 As CrystalDecisions.Shared.ParameterValues = paramField1.CurrentValues
    '''        Dim discreteValue1 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue1.Value = Session("YmcaName")
    '''        curValues1.Add(discreteValue1)


    '''        Dim paramField2 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(2)
    '''        Dim curValues2 As CrystalDecisions.Shared.ParameterValues = paramField2.CurrentValues
    '''        Dim discreteValue2 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue2.Value = Session("Addr1")
    '''        curValues2.Add(discreteValue2)


    '''        Dim paramField3 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(3)
    '''        Dim curValues3 As CrystalDecisions.Shared.ParameterValues = paramField3.CurrentValues
    '''        Dim discreteValue3 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue3.Value = Session("addr2")
    '''        curValues3.Add(discreteValue3)


    '''        Dim paramField4 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(4)
    '''        Dim curValues4 As CrystalDecisions.Shared.ParameterValues = paramField4.CurrentValues
    '''        Dim discreteValue4 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue4.Value = Session("addr3")
    '''        curValues4.Add(discreteValue4)


    '''        Dim paramField5 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(5)
    '''        Dim curValues5 As CrystalDecisions.Shared.ParameterValues = paramField5.CurrentValues
    '''        Dim discreteValue5 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue5.Value = Session("CityStateZip")
    '''        curValues5.Add(discreteValue5)

    '''        Dim paramField6 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(6)
    '''        Dim curValues6 As CrystalDecisions.Shared.ParameterValues = paramField6.CurrentValues
    '''        Dim discreteValue6 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue6.Value = Session("Participant")
    '''        curValues6.Add(discreteValue6)

    '''        Dim paramField7 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(7)
    '''        Dim curValues7 As CrystalDecisions.Shared.ParameterValues = paramField7.CurrentValues
    '''        Dim discreteValue7 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue7.Value = Session("SSNo")
    '''        curValues7.Add(discreteValue7)

    '''        Dim paramField8 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(8)
    '''        Dim curValues8 As CrystalDecisions.Shared.ParameterValues = paramField8.CurrentValues
    '''        Dim discreteValue8 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue8.Value = Session("YmcaNo")
    '''        curValues8.Add(discreteValue8)

    '''        Dim paramField9 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(9)
    '''        Dim curValues9 As CrystalDecisions.Shared.ParameterValues = paramField9.CurrentValues
    '''        Dim discreteValue9 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue9.Value = Session("Message")
    '''        curValues9.Add(discreteValue9)

    '''        Dim paramField10 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(10)
    '''        Dim curValues10 As CrystalDecisions.Shared.ParameterValues = paramField10.CurrentValues
    '''        Dim discreteValue10 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue10.Value = Session("NextDate")
    '''        curValues10.Add(discreteValue10)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        ''''this should go into the dataconfiguration.config file 
    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
    '''Public Function getCashout(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New Cashout
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("LetterDate")
    '''        curValues.Add(discreteValue)


    '''        Dim paramField1 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(1)
    '''        Dim curValues1 As CrystalDecisions.Shared.ParameterValues = paramField1.CurrentValues
    '''        Dim discreteValue1 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue1.Value = Session("Name")
    '''        curValues1.Add(discreteValue1)


    '''        Dim paramField2 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(2)
    '''        Dim curValues2 As CrystalDecisions.Shared.ParameterValues = paramField2.CurrentValues
    '''        Dim discreteValue2 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue2.Value = Session("Addr1")
    '''        curValues2.Add(discreteValue2)


    '''        Dim paramField3 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(3)
    '''        Dim curValues3 As CrystalDecisions.Shared.ParameterValues = paramField3.CurrentValues
    '''        Dim discreteValue3 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue3.Value = Session("Addr2")
    '''        curValues3.Add(discreteValue3)


    '''        Dim paramField4 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(4)
    '''        Dim curValues4 As CrystalDecisions.Shared.ParameterValues = paramField4.CurrentValues
    '''        Dim discreteValue4 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue4.Value = Session("Addr3")
    '''        curValues4.Add(discreteValue4)


    '''        Dim paramField5 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(5)
    '''        Dim curValues5 As CrystalDecisions.Shared.ParameterValues = paramField5.CurrentValues
    '''        Dim discreteValue5 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue5.Value = Session("CityStateZip")
    '''        curValues5.Add(discreteValue5)

    '''        Dim paramField6 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(6)
    '''        Dim curValues6 As CrystalDecisions.Shared.ParameterValues = paramField6.CurrentValues
    '''        Dim discreteValue6 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue6.Value = Session("Salutation")
    '''        curValues6.Add(discreteValue6)

    '''        Dim paramField7 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(7)
    '''        Dim curValues7 As CrystalDecisions.Shared.ParameterValues = paramField7.CurrentValues
    '''        Dim discreteValue7 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue7.Value = Session("YmcaName")
    '''        curValues7.Add(discreteValue7)

    '''        Dim paramField8 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(8)
    '''        Dim curValues8 As CrystalDecisions.Shared.ParameterValues = paramField8.CurrentValues
    '''        Dim discreteValue8 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue8.Value = Session("TermDate")
    '''        curValues8.Add(discreteValue8)

    '''        Dim paramField9 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(9)
    '''        Dim curValues9 As CrystalDecisions.Shared.ParameterValues = paramField9.CurrentValues
    '''        Dim discreteValue9 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue9.Value = Session("LetterFooter")
    '''        curValues9.Add(discreteValue9)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        ''''this should go into the dataconfiguration.config file 
    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
    '''Public Function getCRNONRETIRED(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New CRNONRETIRED
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("PersID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function


    '''Public Function getDeferredAssociationAnnuityAgreementForm(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New DeathBenefitOptions
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("GUID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
    '''Public Function getDisbursementRequest(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New Disbursement_Request
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("DisbursementType")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function

    '''Public Function getHardshipWithdrawalofTDForm(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New HardshipWithdrawalofTDForm
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("GUID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
    '''Public Function getHWL_4_TDOnly(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New HWL_4_TDOnly
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("GUID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
    '''Public Function getHWL_5_TDAndOtherAcc(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New HWL_5_TDAndOtherAcc
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("GUID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function

    '''Public Function getJSBeneficiaryDeath3(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New JSBeneficiaryDeath3
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("guiUniqueID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function

    Public Function getProspectiveRetirementAllowance(ByVal sReportName As String)
        'Put user code to initialize the page here
        Try
            'commented by aparna on 19th April
            'Dim objRpt As New ProspectiveRetirementAllowance
            'commented by aparna on 19th April
            Dim crCon As New ConnectionInfo
            Dim CrTableLogonInfo As New TableLogOnInfo
            Dim CrTables As Tables
            Dim CrTable As Table
            Dim TableCounter As Integer
            Dim DataSource As String
            Dim DatabaseName As String
            Dim UserID As String
            Dim Password As String
            Dim strXml As String
            DataSource = ConfigurationSettings.AppSettings("DataSource")
            DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            UserID = ConfigurationSettings.AppSettings("UserID")
            Password = ConfigurationSettings.AppSettings("Password")
            CrystalReportViewer1.ReportSource = objRpt

            Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
            Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
            Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
            Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue.Value = Session("GUID")
            curValues.Add(discreteValue)

            CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
            CrTables = objRpt.Database.Tables

            crCon.ServerName = DataSource
            crCon.DatabaseName = DatabaseName
            crCon.UserID = UserID
            crCon.Password = Password

            For Each CrTable In CrTables
                CrTableLogonInfo = CrTable.LogOnInfo
                CrTableLogonInfo.ConnectionInfo = crCon
                CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
            Next
            CrystalReportViewer1.DataBind()
            CrystalReportViewer1.PrintMode = PrintMode.ActiveX
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = ex.Message.Trim.ToString()
            Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
        End Try
    End Function
    Function getYMCARefundLetterHardship(ByVal sReportName As String)
        '*******************************************************************************
        ' Called From       :	Refund
        ' Author Name		:	Ragesh.v.p
        ' Employee ID		:	34231
        ' Email				:	ragesh.vp@3i-infotech.com
        ' Creation Time		:	March -06//2006 
        ' Description		:	Print out a letter for each YMCA that this person is active in.
        '                       The letter is a notification informing them about the rules regarding
        '                       future deposits in a Tax Deferred account after a hardship refund
        '*******************************************************************************

        Try
            Try
                'Start code -Added by Aparna on 19th April
                Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                Dim sReportPath As String
                sReportPath = ConfigurationSettings.AppSettings("ReportPath")
                sReportPath = sReportPath.Trim + "\\" + sReportName.Trim + ".rpt"
                Dim logonInfo As TableLogOnInfo = New TableLogOnInfo
                objRpt.Load(sReportPath)
                'End code -Added by Aparna on 19th April
                'commented by Aparna on 19th April
                'Dim objRpt As New birefltr
                Dim crCon As New ConnectionInfo
                Dim CrTableLogonInfo As New TableLogOnInfo
                Dim CrTables As Tables
                Dim CrTable As Table
                Dim TableCounter As Integer
                Dim DataSource As String
                Dim DatabaseName As String
                Dim UserID As String
                Dim Password As String
                Dim strXml As String
                Dim l_dataSet As DataSet
                Dim l_datatable As DataTable
                Dim l_datarow As DataRow
                Dim l_String_tmp As String
                Dim l_string_Persid As String

                l_string_Persid = ""

                DataSource = ConfigurationSettings.AppSettings("DataSource")
                DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
                UserID = ConfigurationSettings.AppSettings("UserID")
                Password = ConfigurationSettings.AppSettings("Password")
                l_string_Persid = CType(Session("MCARefundLetterHardship_Persid"), String)

                If l_string_Persid.Trim = String.Empty Then

                    Throw New Exception("Error: Person Id Not Found")

                End If

                l_dataSet = YMCARET.YmcaBusinessObject.RefundRequest.YMCARefundLetterHardship(l_string_Persid)

                If l_dataSet.Tables.Count > 0 Then

                    l_datatable = l_dataSet.Tables(0)

                    If l_datatable.Rows.Count > 0 Then
                        l_datarow = l_datatable.Rows(0)

                        CrystalReportViewer1.ReportSource = objRpt

                        ' Have to  bind the Number of Pages in the Report.
                        'CrystalReportViewer1.ShowNthPag()

                        'objRpt.SetDataSource(l_dataSet)

                        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
                        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
                        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
                        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                        discreteValue.Value = CType(l_datarow("ContactName"), String).Trim
                        curValues.Add(discreteValue)
                        objRpt.ParameterFields(0).CurrentValues = curValues

                        paramField = paramFieldsCollection(1)
                        curValues = paramField.CurrentValues
                        discreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                        discreteValue.Value = CType(l_datarow("YmcaName"), String).Trim
                        curValues.Add(discreteValue)
                        objRpt.ParameterFields(1).CurrentValues = curValues

                        paramField = paramFieldsCollection(2)
                        curValues = paramField.CurrentValues
                        discreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                        discreteValue.Value = CType(l_datarow("Addr1"), String).Trim
                        curValues.Add(discreteValue)
                        objRpt.ParameterFields(2).CurrentValues = curValues

                        paramField = paramFieldsCollection(3)
                        curValues = paramField.CurrentValues
                        discreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                        discreteValue.Value = CType(l_datarow("Addr2"), String).Trim
                        curValues.Add(discreteValue)
                        objRpt.ParameterFields(3).CurrentValues = curValues

                        paramField = paramFieldsCollection(4)
                        curValues = paramField.CurrentValues
                        discreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                        discreteValue.Value = CType(l_datarow("Addr3"), String).Trim
                        curValues.Add(discreteValue)
                        objRpt.ParameterFields(4).CurrentValues = curValues

                        l_String_tmp = CType(l_datarow("City"), String) + ", " + CType(l_datarow("StateType"), String) + "   " + CType(l_datarow("zip"), String)

                        paramField = paramFieldsCollection(5)
                        curValues = paramField.CurrentValues
                        discreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                        discreteValue.Value = l_String_tmp
                        curValues.Add(discreteValue)
                        objRpt.ParameterFields(5).CurrentValues = curValues

                        l_String_tmp = CType(l_datarow("FirstName"), String) + " "

                        If l_datarow("MiddleName").GetType.ToString() = "System.DBNull" Or CType(l_datarow("MiddleName"), String).Trim = "" Then
                            l_String_tmp = l_String_tmp + CType(l_datarow("LastName"), String)
                        Else
                            l_String_tmp = l_String_tmp + CType(l_datarow("MiddleName"), String).Substring(0, 1) + ". "
                            l_String_tmp = l_String_tmp + CType(l_datarow("LastName"), String)

                        End If

                        paramField = paramFieldsCollection(6)
                        curValues = paramField.CurrentValues
                        discreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                        discreteValue.Value = l_String_tmp
                        curValues.Add(discreteValue)
                        objRpt.ParameterFields(6).CurrentValues = curValues

                        l_String_tmp = CType(l_datarow("SSNo"), String).Substring(0, 3) + "-"
                        l_String_tmp += CType(l_datarow("SSNo"), String).Substring(3, 2) + "-"
                        l_String_tmp += CType(l_datarow("SSNo"), String).Substring(5, 4)
                        paramField = paramFieldsCollection(7)
                        curValues = paramField.CurrentValues
                        discreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                        discreteValue.Value = l_String_tmp
                        curValues.Add(discreteValue)
                        objRpt.ParameterFields(7).CurrentValues = curValues

                        l_String_tmp = CType(l_datarow("YmcaNo"), String)
                        paramField = paramFieldsCollection(8)
                        curValues = paramField.CurrentValues
                        discreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                        discreteValue.Value = l_String_tmp
                        curValues.Add(discreteValue)
                        objRpt.ParameterFields(8).CurrentValues = curValues

                        l_String_tmp = " "
                        paramField = paramFieldsCollection(9)
                        curValues = paramField.CurrentValues
                        discreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                        discreteValue.Value = l_String_tmp
                        curValues.Add(discreteValue)
                        objRpt.ParameterFields(9).CurrentValues = curValues

                        l_String_tmp = System.DateTime.Now.AddMonths(1).Now.ToString("MMMM") + " 01, " + System.DateTime.Now.AddYears(1).Year().ToString()

                        paramField = paramFieldsCollection(10)
                        curValues = paramField.CurrentValues
                        discreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                        discreteValue.Value = l_String_tmp
                        curValues.Add(discreteValue)
                        objRpt.ParameterFields(10).CurrentValues = curValues


                        'CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection

                        'CrTables = objRpt.Database.Tables

                        'crCon.ServerName = DataSource
                        'crCon.DatabaseName = DatabaseName
                        'crCon.UserID = UserID
                        'crCon.Password = Password

                        'For Each CrTable In CrTables
                        '    CrTableLogonInfo = CrTable.LogOnInfo
                        '    CrTableLogonInfo.ConnectionInfo = crCon
                        '    CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                        '    CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
                        'Next

                        CrystalReportViewer1.DataBind()
                        CrystalReportViewer1.PrintMode = PrintMode.ActiveX
                    End If
                End If
            Catch ex As Exception
                Dim l_String_Exception_Message As String
                l_String_Exception_Message = ex.Message.Trim.ToString()
                Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
            End Try

        Catch ex As Exception

        End Try
    End Function
    '''Public Function getReleaseBlank(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New ReleaseBlank
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("GUID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
    '''Public Function getReleaseBlank1kto5k(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New ReleaseBlank1kto5k
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("GUID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
    '''Public Function getReleaseBlankLess1K(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New ReleaseBlankLess1K
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("GUID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
    '''Public Function getReleaseBlankOver5k(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New ReleaseBlankOver5k
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("GUID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function


    '''Public Function getRL_2_FullorPartialRefund(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New RL_2_FullorPartialRefund
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("GUID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function

    '''Public Function getRL_3_FullPIAOver15000(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New RL_3_FullPIAOver15000
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("GUID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
    '''Public Function getRL_6a_Over70PIAOver15000(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New RL_6a_Over70PIAOver15000
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("GUID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
    '''Public Function getRL_6b_Over70PIA500015000(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New RL_6b_Over70PIA5000_15000
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("GUID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
    '''Public Function getRL_6c_Over70PIAUnder5000(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New RL_6c_Over70PIAUnder5000
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("GUID")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
    '''Public Function getsafeharbor(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New SafeHarbor
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("LetterDate")
    '''        curValues.Add(discreteValue)


    '''        Dim paramField1 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(1)
    '''        Dim curValues1 As CrystalDecisions.Shared.ParameterValues = paramField1.CurrentValues
    '''        Dim discreteValue1 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue1.Value = Session("Name")
    '''        curValues1.Add(discreteValue1)


    '''        Dim paramField2 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(2)
    '''        Dim curValues2 As CrystalDecisions.Shared.ParameterValues = paramField2.CurrentValues
    '''        Dim discreteValue2 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue2.Value = Session("Addr1")
    '''        curValues2.Add(discreteValue2)


    '''        Dim paramField3 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(3)
    '''        Dim curValues3 As CrystalDecisions.Shared.ParameterValues = paramField3.CurrentValues
    '''        Dim discreteValue3 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue3.Value = Session("Addr2")
    '''        curValues3.Add(discreteValue3)


    '''        Dim paramField4 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(4)
    '''        Dim curValues4 As CrystalDecisions.Shared.ParameterValues = paramField4.CurrentValues
    '''        Dim discreteValue4 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue4.Value = Session("Addr3")
    '''        curValues4.Add(discreteValue4)


    '''        Dim paramField5 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(5)
    '''        Dim curValues5 As CrystalDecisions.Shared.ParameterValues = paramField5.CurrentValues
    '''        Dim discreteValue5 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue5.Value = Session("CityStateZip")
    '''        curValues5.Add(discreteValue5)

    '''        Dim paramField6 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(6)
    '''        Dim curValues6 As CrystalDecisions.Shared.ParameterValues = paramField6.CurrentValues
    '''        Dim discreteValue6 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue6.Value = Session("Salutation")
    '''        curValues6.Add(discreteValue6)

    '''        Dim paramField7 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(7)
    '''        Dim curValues7 As CrystalDecisions.Shared.ParameterValues = paramField7.CurrentValues
    '''        Dim discreteValue7 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue7.Value = Session("YmcaName")
    '''        curValues7.Add(discreteValue7)

    '''        Dim paramField8 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(8)
    '''        Dim curValues8 As CrystalDecisions.Shared.ParameterValues = paramField8.CurrentValues
    '''        Dim discreteValue8 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue8.Value = Session("TermDate")
    '''        curValues8.Add(discreteValue8)

    '''        Dim paramField9 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(9)
    '''        Dim curValues9 As CrystalDecisions.Shared.ParameterValues = paramField9.CurrentValues
    '''        Dim discreteValue9 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue9.Value = Session("LetterFooter")
    '''        curValues9.Add(discreteValue9)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
    '''Public Function getSafeHarborLetters(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        'Dim objRpt As New SafeHarborLetters
    '''        Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    '''        Dim sReportPath As String
    '''        sReportPath = Server.MapPath("/YMCAUI") + "\Reports\" + "SafeHarborLetters" + ".rpt"
    '''        objRpt.Load(sReportPath)
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("Not Opening")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
    '''Public Function getTransitionLetters(ByVal sReportName As String)
    '''    'Put user code to initialize the page here
    '''    Try
    '''        Dim objRpt As New TransitionLetters
    '''        Dim crCon As New ConnectionInfo
    '''        Dim CrTableLogonInfo As New TableLogOnInfo
    '''        Dim CrTables As Tables
    '''        Dim CrTable As Table
    '''        Dim TableCounter As Integer
    '''        Dim DataSource As String
    '''        Dim DatabaseName As String
    '''        Dim UserID As String
    '''        Dim Password As String
    '''        Dim strXml As String
    '''        DataSource = ConfigurationSettings.AppSettings("DataSource")
    '''        DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
    '''        UserID = ConfigurationSettings.AppSettings("UserID")
    '''        Password = ConfigurationSettings.AppSettings("Password")
    '''        CrystalReportViewer1.ReportSource = objRpt

    '''        Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = CrystalReportViewer1.ParameterFieldInfo
    '''        Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
    '''        Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
    '''        Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
    '''        discreteValue.Value = Session("@RunMode")
    '''        curValues.Add(discreteValue)

    '''        CrystalReportViewer1.ParameterFieldInfo = paramFieldsCollection
    '''        CrTables = objRpt.Database.Tables

    '''        crCon.ServerName = DataSource
    '''        crCon.DatabaseName = DatabaseName
    '''        crCon.UserID = UserID
    '''        crCon.Password = Password

    '''        For Each CrTable In CrTables
    '''            CrTableLogonInfo = CrTable.LogOnInfo
    '''            CrTableLogonInfo.ConnectionInfo = crCon
    '''            CrTable.ApplyLogOnInfo(CrTableLogonInfo)
    '''            CrTable.Location = DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
    '''        Next
    '''        CrystalReportViewer1.DataBind()

    '''    Catch ex As Exception
    '''        Dim l_String_Exception_Message As String
    '''        l_String_Exception_Message = ex.Message.Trim.ToString()
    '''        Response.Redirect("..\ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '''    End Try
    '''End Function
#End Region

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        objRpt.Close()
        objRpt.Dispose()
        objRpt = Nothing
        CrystalReportViewer1.Dispose()
        CrystalReportViewer1 = Nothing
        GC.Collect()

    End Sub

    'Added By SG: 2012.01.04: BT-1511
    Private Sub ddlReportOption_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReportOption.SelectedIndexChanged
        populateReportsValues()
    End Sub
End Class
