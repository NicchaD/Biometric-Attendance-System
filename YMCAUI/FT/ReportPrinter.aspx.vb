'***************
' Created a copy of this page from the ReportViewer.aspx page. With last change tracked as the following entry.
' Any future changes made to the ReportViewer.aspx file need to be replicated into this file as well.
' Priya                 19-Feb-2009           None                    Added trim to name as it displays spaces between name in report.
'***************
'**********************************************************************************************************************
'** Modification History    
'**********************************************************************************************************************
'** Modified By        Date(MM/DD/YYYY)     Issue ID        Description  
'**********************************************************************************************************************
'   Nikunj Patel        2010.06.25          None            Base version created from ReportViewer.aspx
'   Ashish Srivastava   2010.06.25          YRS 5.0-1115
'	Imran Bedrekar		2011.07.12		    BT:892-YRS 5.0-1359 : Disability Estimate form
'   Imran Bedrekar      2011.07.20          Add case in callprinter for Disability form
'   Nikunj Patel		2011.10.28			Making changes to allow printing of AltPayeeAnnuityEst report YRS 5.0-1345
'   Shashank Patel      2011.12.06          YRS 5.0-1365 : Need new batch processing option
'	Prasad J			2012.04.11			For BT-1018,YRS 5.0-1561: Add 'Print Letter' and 'Reset Account' buttons to Web info display page
'	Priya Patil			2012.11.08			YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
'   Sanjeev Gupta(SG)   2012.12.05          BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
'   B.Jagadeesh         2015.05.19          BT-2816: YRS 5.0-2495:Remove the print drop down box on Color Full Form
'	Anudeep             2015.06.26          BT:2900: YRS 5.0-2533:Loan Modifications (Default) Project: two new YRS letters
'   Manthan Rajguru     2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
'   Anudeep A           2016.03.07          YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
'   Anudeep A           2016.04.05          YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
'   Manthan Rajguru     2016.05.05          YRS-AT-2909 -  Support Request: printing crystal report com exception LoanDefaultLetter.rpt
'   Shiny C.            2020.04.10          YRS-AT-4852 -  COVID - YRS changes needed for LOANS due to CARE Act (Track IT - 41693)
'**********************************************************************************************************************

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports CrystalDecisions.CrystalReports.ViewerObjectModel
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class ReportPrinter
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
    Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
    Protected WithEvents hiddError As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim strReportName As String
    Dim strModuleName As String
    Protected WithEvents ddlPrinterName As System.Web.UI.WebControls.DropDownList
    Protected WithEvents btnPrintsetting As System.Web.UI.WebControls.Button
    Protected WithEvents hiddReport As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblMenuHeading As System.Web.UI.WebControls.Label

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
        strReportName = CType(Session("strReportName"), String)
        'Added By SG: 2012.12.05: BT-960
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
        Dim l_cmdStatus As String = String.Empty
        If Not Request.QueryString("cmd") Is Nothing Then
            l_cmdStatus = Request.QueryString("cmd").ToString()
        End If


        If l_cmdStatus = "finished" Then
            Return
        End If

        Dim sReportName As String
        'by aparna
        Dim bBoolReport As Boolean
        Dim LstBox As New System.Web.UI.WebControls.ListBox
        Try
            'Added By SG: 2012.12.05: BT-960
            'If strReportName = "Withdrawals" Then
            If strReportName = "Withdrawals_New" AndAlso strModuleName = "CashOut" Then
                lblMenuHeading.Text = "Utilities > Cash Out > Printer Setup"
            Else
                lblMenuHeading.Text = ""
            End If

            If Not Session("ManualTransacts") = "ManualTransacts" Then
                btnExport.Attributes.Add("onclick", "JavaScript:showFileDialogBox()")
            End If
            If Not Session("VRManager") = "VRManager" Then
                btnExport.Attributes.Add("onclick", "JavaScript:showFileDialogBox()")
            End If
            If Not (IsPostBack) Then
                GetPrinterName()
            End If

            '================================
            ''Added By Aparna on 13th April

            If hiddError.Value <> String.Empty Then

                hiddReport.Value = "True"
                sReportName = strReportName

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
                        'Ashish:2010.06.25 YRS 5.0-1115 ,commented
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
                        populateReportsValues()

                        'Added By Ashutosh Patil as on 19-01-2007  for YREN - 3023   
                    Case "Active Default letter"
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
                        'Ashish:2010.06.25 YRS 5.0-1115 ,Start
                        'Start: B.Jagadeesh 2015.05.19 YRS 5.0-2495 Replaced report name from [ANNTYESTLONG] to [ANNTYESTLONG_BATCH]
                    Case "ANNTYESTLONG_BATCH"
                        'End: B.Jagadeesh 2015.05.19 YRS 5.0-2495 Replaced report name from [ANNTYESTLONG] to [ANNTYESTLONG_BATCH]
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
                        'Priya Patil 2012.11.08 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
                        'Added By SG: 2012.12.05: BT-960
                        'Case "Withdrawals"
                    Case "Withdrawals_New"
                        populateReportsValues()
                        'END Priya Patil 2012.11.08 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
                        'Start:AA:07.03.2016 YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
                    Case "New Cashout Letter"
                        populateReportsValues()
                        'End:AA:07.03.2016 YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
                    Case Else
                        bBoolReport = getReport(strReportName)

                End Select
                '=================================================
            End If
        Catch
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

        If Not Session("ListBoxSelectedItems") Is Nothing Then
            ListBoxSelectedItems = Session("ListBoxSelectedItems")
        End If

        'end code Added By Aparna on 17th April

        Try

            sReportName = strReportName

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
                Case "YRSParticipantHistoryReportBySSNo"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("SSNo"))
                    For i = 0 To ListBoxSelectedItems.Items.Count - 1
                        ArrListParamValues.Add(ListBoxSelectedItems.Items(i).Text.ToString())
                    Next

                Case "First Annuity Checks Request"
                    boollogontoDB = True

                Case "Disbursement Request"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("DisbType"))

                Case "AccouToBechanged"
                    boollogontoDB = True
                    ArrListParamValues.Add(Convert.ToDateTime(Session("MonthEndClosingDate")))

                Case "Pre-RetirementDeathBenefitsClaimForm"
                    boollogontoDB = True

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
                    'Ashish:2010.06.25  YRS 5.0-1115, Start
                    'Case "ProspectiveRetirementAllowance"
                    '    boollogontoDB = True
                    '    ArrListParamValues.Add(Session("GUID"))
                    'Start: B.Jagadeesh 2015.05.19 YRS 5.0-2495 Replaced report name from [ANNTYESTLONG] to [ANNTYESTLONG_BATCH]
                Case "ANNTYESTLONG_BATCH"
                    'End: B.Jagadeesh 2015.05.19 YRS 5.0-2495 Replaced report name from [ANNTYESTLONG] to [ANNTYESTLONG_BATCH]
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("GUID"))

                Case "ANNTYESTSHORT"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("GUID"))
                Case "ANNTYESTCOLOR"
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("GUID"))
                    'Ashish:2010.06.25  YRS 5.0-1115, End
                    'IB:BT:892-YRS 5.0-1359 : Disability Estimate form
                Case "DisabilityAnnuityEst", "AltPayeeAnnuityEst"   'NP:2011.10.28:YRS 5.0-1345 - Adding AltPayeeAnnuityEst to the switch case to enable handling of the report
                    boollogontoDB = True
                    ArrListParamValues.Add(Session("GUID"))
                Case "RetireeDeathBenefitsClaimForm"
                    boollogontoDB = True

                    ArrListParamValues.Add(Session("Para1_NameofDeceased"))
                    ArrListParamValues.Add(Session("Para2_NameofClaimant"))

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
                    l_dataSet = YMCARET.YmcaBusinessObject.RefundRequest.YMCARefundLetterHardship(l_string_Persid)

                    If l_dataSet.Tables.Count > 0 Then
                        l_datatable = l_dataSet.Tables(0)
                        If l_datatable.Rows.Count > 0 Then
                            l_datarow = l_datatable.Rows(0)
                        End If
                    End If
                    ArrListParamValues.Add(CType(l_datarow("ContactName"), String).Trim)
                    ArrListParamValues.Add(CType(l_datarow("YmcaName"), String).Trim)
                    ArrListParamValues.Add(CType(l_datarow("Addr1"), String).Trim)
                    ArrListParamValues.Add(CType(l_datarow("Addr2"), String).Trim)
                    ArrListParamValues.Add(CType(l_datarow("Addr3"), String).Trim)
                    l_String_tmp = CType(l_datarow("City"), String).Trim + ", " + CType(l_datarow("StateType"), String).Trim + "   " + CType(l_datarow("zip"), String).Trim
                    ArrListParamValues.Add(CType(l_String_tmp, String).Trim)
                    'Priya 19-Feb-2009 : Added trim to name as it displays spaces between name in report. 
                    l_String_tmp = CType(l_datarow("FirstName"), String).Trim + " "
                    If l_datarow("MiddleName").GetType.ToString().Trim = "System.DBNull" Or CType(l_datarow("MiddleName"), String).Trim = "" Then
                        l_String_tmp = l_String_tmp + CType(l_datarow("LastName"), String).Trim
                    Else
                        l_String_tmp = l_String_tmp + CType(l_datarow("MiddleName"), String).Trim.Substring(0, 1) + ". "
                        l_String_tmp = l_String_tmp + CType(l_datarow("LastName"), String).Trim
                        'End 19-Feb-2009
                    End If
                    ArrListParamValues.Add(CType(l_String_tmp, String).Trim)

                    'Priya 19-Feb-2009 : Added trim to SSNO and added if condition to check null SSNO and length of SSNO
                    Dim l_string_SSNO As String
                    l_string_SSNO = CType(l_datarow("SSNo"), String).Trim
                    If l_string_SSNO <> String.Empty AndAlso l_string_SSNO.Length = 9 Then
                        l_String_tmp = l_string_SSNO.Substring(0, 3) + "-"
                        l_String_tmp += l_string_SSNO.Substring(3, 2) + "-"
                        l_String_tmp += l_string_SSNO.Substring(5, 4)
                    Else
                        l_String_tmp = l_string_SSNO
                    End If

                    'l_String_tmp = CType(l_datarow("SSNo"), String).Substring(0, 3) + "-"
                    'l_String_tmp += CType(l_datarow("SSNo"), String).Substring(3, 2) + "-"
                    'l_String_tmp += CType(l_datarow("SSNo"), String).Substring(5, 4)
                    'End 19-Feb-2009


                    ArrListParamValues.Add(CType(l_String_tmp, String).Trim)

                    ArrListParamValues.Add(CType(l_datarow("YmcaNo"), String).Trim)
                    l_String_tmp = " "

                    ArrListParamValues.Add(CType(l_String_tmp, String).Trim)

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

                Case "YMCA ACH Report" 'Aparna 7thNov-2006
                    boollogontoDB = True
                    ArrListParamValues = CType(Session("arrListParaColl"), ArrayList)

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
                    'By Aparna - as the parameter is removed from the report-14/12/2006
                    'ArrListParamValues.Add(CType(Session("CashOutRangeDesc"), String).ToString.Trim)
                    'Added By Ashutosh Patil as on 19-01-2007  for YREN - 3023   
                    'Start Ashutosh Patil
                    'AA:26.06.2015 BT:2900 YRS 5.0-2533:Modified letter for default and ofset loans
                Case "LoanDefaultLetter"
                    boollogontoDB = True
                    ArrListParamValues.Add(CType(Session("FundNo"), String).ToString.Trim)
                    ArrListParamValues.Add(Session("DefaultDate"))
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
                    'Priya Patil 2012.11.08 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
                    'Added By SG: 2012.12.05: BT-960
                    'Case "Withdrawals"
                Case "Withdrawals_New"
                    If strModuleName = "CashOut" Then
                        boollogontoDB = True
                        ArrListParamValues.Add(CType(Session("PrintBatchID"), Integer))
                        ArrListParamValues.Add("BID".ToString.Trim)
                    End If
                    'END: Added By SG: 2012.12.05: BT-960
                    'END Priya Patil 2012.11.08 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    'Start:AA:07.03.2016 YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
                Case "New Cashout Letter"
                    boollogontoDB = True
                    ArrListParamValues.Add("")
                    ArrListParamValues.Add(CType(Session("PrintBatchID"), String).ToString.Trim)
                    'End:AA:07.03.2016 YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
                Case Else

            End Select

            If sReportName.Trim <> String.Empty Then
                LoadReport(ArrListParamValues, sReportName, boollogontoDB)
            End If

        Catch
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
        Dim dsShellPrinter As DataSet
        Dim dr As DataRow()
        Dim l_dr As DataRow
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
                'End - Manthan Rajguru | 2016.05.05 | YRS-AT-2909 | Subreport dB tables binding
            End If


            'If Not ConfigurationSettings.AppSettings("PRINTREPORTSONSERVER") Is Nothing AndAlso ConfigurationSettings.AppSettings("PRINTREPORTSONSERVER").ToString() <> String.Empty Then

            '    If PrintReportFromServer(ConfigurationSettings.AppSettings("PRINTREPORTSONSERVER")) = False Then

            '        If hiddReport.Value = String.Empty Then
            '            Dim popupScript As String = "<script language='javascript'>DisplayError();</script>"

            '            If (Not Me.IsStartupScriptRegistered("DisplayError")) Then
            '                Page.RegisterStartupScript("DisplayError", popupScript)
            '            End If
            '        End If


            '        CrystalReportViewer1.DataBind()
            '    Else
            '        'print successfuly

            '        Dim popupScript As String = "<script language='javascript'>DisplaySuccess();</script>"
            '        If (Not Me.IsStartupScriptRegistered("PrintClose")) Then
            '            Page.RegisterStartupScript("PrintClose", popupScript)
            '        End If
            '        CrystalReportViewer1.ReportSource = Nothing
            '    End If
            'Else
            '    hiddError.Value += "Configuration PRINTREPORTSONSERVER not defined."
            '    Dim popupScript As String = "<script language='javascript'>DisplayError();</script>"
            '    If (Not Me.IsStartupScriptRegistered("DisplayError")) Then
            '        Page.RegisterStartupScript("DisplayError", popupScript)
            '    End If
            'End If
            If Not ViewState("Dataset_Printer") Is Nothing Then
                dsShellPrinter = ViewState("Dataset_Printer")
                dr = dsShellPrinter.Tables("Printers").Select("[PrinterID]= " & ddlPrinterName.SelectedValue & "")
                YMCARET.YmcaBusinessObject.MetaShellPrintersBOClass.InsertUserPrinter(Session("LoggedUserKey"), dr(0)("ReportID"), Convert.ToInt64(ddlPrinterName.SelectedValue))

                If PrintReportFromServer(dr(0)("PrinterConfigartion")) = False Then

                    If hiddReport.Value = String.Empty Then
                        Dim popupScript As String = "<script language='javascript'>DisplayError();</script>"

                        If (Not Me.IsStartupScriptRegistered("DisplayError")) Then
                            Page.RegisterStartupScript("DisplayError", popupScript)
                        End If
                    End If


                    CrystalReportViewer1.DataBind()
                Else
                    'print successfuly

                    Dim popupScript As String = "<script language='javascript'>DisplaySuccess();</script>"
                    If (Not Me.IsStartupScriptRegistered("PrintClose")) Then
                        Page.RegisterStartupScript("PrintClose", popupScript)
                    End If
                    CrystalReportViewer1.ReportSource = Nothing
                End If
            Else

                Dim popupScript As String = "<script language='javascript'>DisplayError();</script>"
                If (Not Me.IsStartupScriptRegistered("DisplayError")) Then
                    Page.RegisterStartupScript("DisplayError", popupScript)
                End If
                CrystalReportViewer1.DataBind()
            End If





            'CrystalReportViewer1.RefreshReport()

        Catch ex As Exception
            HelperFunctions.LogException("LoadReport", ex)
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

        Catch
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
        Catch
            Throw
        End Try
    End Function

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
    Private Sub btnFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        CrystalReportViewer1.ShowFirstPage()
    End Sub
    Private Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
        CrystalReportViewer1.ShowNextPage()
    End Sub
    Private Sub btnPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrevious.Click
        CrystalReportViewer1.ShowPreviousPage()
    End Sub

    Private Sub btnPrintsetting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintsetting.Click

        Try

            CallPrintReport()

        Catch ex As Exception

        End Try
    End Sub
#Region "All Report Methods"

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
    'Ashish:2010.07.02-YRS 5.0-1115 ,Added print from server functionality
    'parameters = Format of the parameter string should be
    '"PrinterName=HP Printer;PaperType=preprinted;PaperOrientation=Landscape;"

    Private Function PrintReportFromServer(ByVal parameters As String) As Boolean

        Dim params As String() = parameters.Split(";")

        Dim printerName, paperType, paperOrientation As String

        Dim param As String()
        Dim testMode As Boolean = False


        Dim i As Integer
        Try

            For i = 0 To params.Length - 1

                If params(i).Trim <> String.Empty Then

                    param = params(i).Trim.Split("=")

                    If param.Length <> 2 Then

                        'Incomplete parameters specified. Log message in error log
                        hiddError.Value &= "Incomplete parameters specified."
                        Return False

                    End If

                    'Only assign values if valid key value pair is found

                    Select Case param(0).ToLower()

                        Case "printername"

                            printerName = param(1)

                        Case "papertype"

                            paperType = param(1)

                        Case "paperorientation"

                            paperOrientation = param(1)
                        Case "testmode"

                            testMode = Boolean.Parse(param(1))

                    End Select
                End If

            Next

            If printerName <> String.Empty Then
                objRpt.PrintOptions.PrinterName = printerName '//"Microsoft XPS Document Writer";
            Else
                'Printer name not specified. Log a message in the error log
                hiddError.Value &= "Printer name not specified."
                Return False
            End If

            If printerName <> String.Empty AndAlso paperType <> String.Empty Then

                Dim paperSource As System.Drawing.Printing.PaperSource = GetSelectedPaperSource(printerName, paperType)

                If (Not paperSource Is Nothing) Then

                    objRpt.PrintOptions.CustomPaperSource = paperSource

                Else

                    'Paper type not found. Log a message in the error log
                    hiddError.Value &= "Paper Type not found on Printer."
                    Return False

                End If

            End If

            If paperOrientation <> String.Empty Then

                Select Case paperOrientation.ToLower()

                    Case "portrait"

                        objRpt.PrintOptions.PaperOrientation = CrystalDecisions.[Shared].PaperOrientation.Portrait

                    Case "landscape"

                        objRpt.PrintOptions.PaperOrientation = CrystalDecisions.[Shared].PaperOrientation.Landscape

                    Case Else

                        'Invalid orientation specified. Log a message in the error log
                        hiddError.Value &= "Paper orientation is invalid."

                        Return False

                End Select

            End If

            If testMode = False Then

                objRpt.PrintToPrinter(1, False, 0, 0)

            End If
            Return True
        Catch ex As Exception
            HelperFunctions.LogException("", ex)
            hiddError.Value &= "There was a problem printing from the server. Please contact IT support."
            Return False

        End Try

    End Function


    Private Function GetSelectedPaperSource(ByVal printerName As String, ByVal paperSourceName As String) As System.Drawing.Printing.PaperSource

        Dim selectedPaperSource As System.Drawing.Printing.PaperSource = Nothing '//new System.Drawing.Printing.PaperSource();

        Dim printerSettings As System.Drawing.Printing.PrinterSettings = New System.Drawing.Printing.PrinterSettings

        printerSettings.PrinterName = printerName

        Dim paperSource As System.Drawing.Printing.PaperSource

        For Each paperSource In printerSettings.PaperSources

            If (String.Compare(paperSource.SourceName, paperSourceName, True) = 0) Then
                selectedPaperSource = paperSource
                Exit For
            End If
        Next
        Return selectedPaperSource


    End Function

#End Region
    Private Sub GetPrinterName()

        'Dim printerSettings As System.Drawing.Printing.PrinterSettings = New System.Drawing.Printing.PrinterSettings

        'For Each printer As String In printerSettings.InstalledPrinters
        '    'printerSettings.PrinterName = printer 
        '    'If (printerSettings.IsDefaultPrinter) Then
        '    ddlPrinterName.Items.Add(New ListItem(printer, printer))

        'Next
        Dim dsShellPrinter As New DataSet
        Try
            dsShellPrinter = YMCARET.YmcaBusinessObject.MetaShellPrintersBOClass.GetUserShellPrinters(Session("LoggedUserKey"), strReportName)
            If dsShellPrinter.Tables.Count > 0 Then
                If Not dsShellPrinter.Tables("Printers").Rows.Count = 0 Then
                    For Each dr As DataRow In dsShellPrinter.Tables(0).Rows
                        ddlPrinterName.Items.Add(New ListItem(dr("PrinterName").ToString(), dr("PrinterID").ToString()))
                    Next
                    ddlPrinterName.Items.Add(New ListItem("--Select One--", 0))
                    If dsShellPrinter.Tables("UserPrinter").Rows.Count > 0 Then
                        If Not ddlPrinterName.Items.FindByValue(dsShellPrinter.Tables("UserPrinter").Rows(0)("UserPrinterID")) Is Nothing Then
                            ddlPrinterName.SelectedValue = dsShellPrinter.Tables("UserPrinter").Rows(0)("UserPrinterID")
                        Else
                            ddlPrinterName.SelectedValue = 0
                        End If
                    Else
                        ddlPrinterName.SelectedValue = 0
                    End If



                    ViewState("Dataset_Printer") = dsShellPrinter
                Else
                    hiddError.Value += "There are no printers configured with the paper type required for printing this Form. Please print this form manually using the print button of Crystal Viewer."
                End If

            End If
        Catch
            Throw
        End Try
    End Sub

    Private Sub CallPrintReport()
        '================================
        ''Added By Aparna on 13th April
        Dim sReportName As String
        'by aparna
        Dim bBoolReport As Boolean
        Dim LstBox As New System.Web.UI.WebControls.ListBox
        sReportName = strReportName

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
                'Ashish:2010.06.25 YRS 5.0-1115 ,commented
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
                populateReportsValues()

                'Added By Ashutosh Patil as on 19-01-2007  for YREN - 3023   
            Case "Active Default letter"
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
                'Ashish:2010.06.25 YRS 5.0-1115 ,Start
                'Start: B.Jagadeesh 2015.05.19 YRS 5.0-2495 Replaced report name from [ANNTYESTLONG] to [ANNTYESTLONG_BATCH]
            Case "ANNTYESTLONG_BATCH"
                'End: B.Jagadeesh 2015.05.19 YRS 5.0-2495 Replaced report name from [ANNTYESTLONG] to [ANNTYESTLONG_BATCH]
                populateReportsValues()
            Case "ANNTYESTSHORT"
                populateReportsValues()
            Case "ANNTYESTCOLOR"
                populateReportsValues()
                'Ashish:2010.06.22 YRS 5.0-1115 ,End
                'IB:BT:892-YRS 5.0-1359 : Disability Estimate form
            Case "DisabilityAnnuityEst", "AltPayeeAnnuityEst"   'NP:2011.10.28:YRS 5.0-1345 - Adding AltPayeeAnnuityEst to the switch case to enable handling of the report
                populateReportsValues()
                'Added by prasad YRS 5.0-1561: Add 'Print Letter' and 'Reset Account' buttons to Web info display page
            Case "WebIdLetterOleN"
                populateReportsValues()
                'Priya Patil 2012.11.08 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
                'Added By SG: 2012.12.05: BT-960
                'Case "Withdrawals"
            Case "Withdrawals_New"
                populateReportsValues()
                'END Priya Patil 2012.11.08 YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 
                'Start:AA:05.04.2016 YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
            Case "New Cashout Letter"
                populateReportsValues()
                'End:AA:05.04.2016 YRS-AT-2764 - YRS enh: Withdrawals - a. Modify cash-out utility to generate only letters, not Withdrawal Forms, to cash-out candidates
            Case Else
                bBoolReport = getReport(strReportName)

        End Select
        '=================================================
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        objRpt.Close()
        objRpt.Dispose()
        objRpt = Nothing
        CrystalReportViewer1.Dispose()
        CrystalReportViewer1 = Nothing
        GC.Collect()
    End Sub

End Class
