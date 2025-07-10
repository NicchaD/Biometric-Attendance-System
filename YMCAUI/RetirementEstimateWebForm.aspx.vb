'*******************************************************************************
' Project Name		:	33733
' FileName			:	RetirementEstimateWebForm.aspx
' Author Name		:	Prasanna Penumarthy
' Employee ID		:	33733
' Email			    :	prasanna.penumarthy@3i-infotech.com
' Contact No		:	55928745
' Creation Time	    :	6/1/2005 7:28:12 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'************************************************************************************
'Modficiation History
'************************************************************************************
'Modified By		    Date	        Description
'************************************************************************************
' Hafiz                 04Feb06         Cache-Session
' Hafiz                 16Feb06         Bug resolving from Gemini Id : YRST2043, YRST2048, YRST2047
' Hafiz                 17Feb06         Bug resolving from Gemini id : YRST2070, YMCA1977
' Mohammed Hafiz        31Mar06         Enhancement Changes - Consideineshring Termination Date for getting Annuity Types
' Mohammed Hafiz        23-Jan-2007     YREN-3020
' Shubhrata Tripathi    24-Jan-2007     YREN-3017
' Asween                02-Apr-2007     YREN-3255
' Asween                10-Apr-2007     Defects reported by Purushottam 
' Mohammed Hafiz	    16-Apr-2007	    YREN-3257
' Asween        	    28-May-2007	    Plan split Implementation
' Swopna                 18-Jan-2008    YRPS-4535
' Anil                   29-Jan-2008    YRPS 4536
' Swopna                 31-Jan-2008    YRPS-4535
' Mohammed Hafiz        28-Mar-2008     YRPS-4534
' Mohammed Hafiz         8-Apr-2008     Phase IV Changes
' Nikunj Patel          06-May-2008     BT-416 - Fixing issue where the Savings plan amounts were considered while computing values for the Retirement Plan
' Nikunj Patel          07-May-2008     BT-418 - Fixing issue where the Retirement plan grid was not being validated for contributions, start dates and stop dates.
' Nikunj Patel          08-May-2008     BT-417 - Fixing issue where the Account contribution start date for estimate was earlier than the 1st of the month.
' Nikunj Patel          13-Jun-2008     YRS-5.0-457 - Removing the plan selection checkboxes and clearing the results grid on each calculation
'                                       Also on changing plans all accounts are automatically selected.
' Mohammed Hafiz        2-Jul-2008      YRS 5.0-472
' Mohammed Hafiz        9-Jul-2008      YRS 5.0-478
' Ashish Srivastava     30-July-208     YRS 5.0-445
' Mohammed Hafiz        5-Sep-2008      YRS 5.0-445
' Priya                 16-Jan-2006     YRS 5.0-656 Not allowing Plan-wise estimate for QDRO fund event
' Priya                 12-Feb-09       YRS 5.0-620 Incorrect message for person with status RA
' Priya                 04-March-09     BT- 710 Made changes to display SmmaryTab and select employment on AccountsTab on page load
' Ashish                07-Apr-2009     Phase V changes
' Ashish                17-Jul-2009     Resolve Isssue YRS 5.0-830
'Ashish Srivastava      21-Jul-2009    Integrate Issue YRS 5.0-830 from lable version 7.0.3
'Ashish Srivastava      23-Jul-2009    Resolve Issue YRS 5.0-835 
'Ashish Srivastava      30-Jul-2009    Resolve Issue TD account included into estimation even it de-selected  
'Ashish Srivastava      03-Aug-2009    Resolve Issue YRS 5.0-801
'Ashish Srivastava      16-Oct-2009    Display summary Tab on Print Estimate

'Neeraj Singh                   12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Ashish Srivastava      2009.11.17      Commented unused columns in employementDetails datatable,change persID with fundEventID for calculating avg salary
'Priya                  05-Jan-2010     YRS 5.0-983 : Enable all the fields in "Retirement" tab.
'Ashish Srivastava      26-May-2010     Changes required for Migration
'Shashi Shekhar:        5 Aug 2010      :YRS 5.0-1142 -   Change Print button labels 
'Ashish Srivastava      2010.10.11      YRS 5.0-855,BT 624 and optimization of code
'Shashi Shekhar         30-Dec-2010     For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'Sanket vaidya          17-Dec-2010     Added by Sanket to reset the value of Employment drop down in Accounts tab
'Shashi Shekhar         28 Feb 2011         Replacing Header formating with user control (YRS 5.0-450 )
'Ashish Srivastava      2011.04.14      Enable disability retirement when purchasing annuity from saving plan only
'Sanket vaidya          18 Apr 2011     BT-816 : Disability Retirement Estimate Issues.
'Sanket vaidya          31 May 2011     YRS 5.0-1035 : Set paid service - atsFundEvents.intPaid - to zero.
'Imran  Bedrekar        12-July-2011    BT:892-YRS 5.0-1359 : Disability Estimate form
'Ashish Srivastava      2011.07.27      BT-888,
'Sanket Vaidya          2011.07.27      YRS 5.0-1150/BT-596 : Option C reduction amount should not include Dth Ben annty
'Sanket Vaidya          2011.08.18      YRS 5.0-657 : Estimate with SSL amounts
'Sanket Vaidya          2011.08.10      BT-926 : Showing and allowing Retired Death Benefit amount even for 'Savings' plan selected.
'Ashish Srivastava      2011.08.29      Resolve YRS 5.0-1345 :BT-859 Death benefit will not be available for QD participant and invoke Alternate payee report
'Ashish Srivastava      2011.09.13      fix reopen issue YRS 5.0-657/BT-636
'Ashish Srivastava      2011.09.21      YRS-1345/BT-859 commented suppress vidation code for above 60 participant 
'Sanket Vaidya          2011.08.10      YRS 5.0-1329:J&S options available to non-spouse beneficiaries
'Sanket Vaidya          2011.09.30      BT-798 : System should not allow disability retirement for QD and BF fundevents
'Nikunj Patel			2011.10.31		YRS 5.0-1329: Updated message as per email from Raj dt 2011.10.28.
'Shashank Patel         2011.11.03      YRS-1474/BT-952 : Error when selected RDB pctg before RDB is computed
'Shashank Patel         2012.04.12      BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary 
'Shashank Patel         2012.05.21      BT-976/YRS 5.0-1507 - Reopned issue
'Shashank Patel			2012.06.11      BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect (-Reopen)
'Shashank Patel			2012.07.11		BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page 
'Sanjay R               2012.07.30      BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page 
'Anudeep A              2012-09-22      BT-1126/Additional change YRS 5.0-1246 : : Handling DLIN records
'Anudeep                2012-10-13      BT-1238: YRS 5.0-1541:Estimates calculator Handled for clear benificiaries
'Anudeep                2012-10-15      BT-1238: YRS 5.0-1541:Estimates calculator Handled for spouse benificiary 
'Anudeep                2012-11-29      Bt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request
'Anudeep                2012.12.04      Bt-1026/YRS 5.0-1629:Code changes to copy report into IDM folder
'Sanjeev Gupta(SG)      2012.12.06      BT-1432: YRS 5.0-1727:Retirement batch estimates not showing J annuity options correctly.
'Sanjeev Gupta(SG)      2012.12.10      BT-1426: YRS 5.0-1726:Write "*Estate" to beneficiary name field in atsPraReport.
'Sanjeev Gupta(SG)      2013.01.10      BT-1432 (Re-Opened): YRS 5.0-1727: Retirement batch estimates not showing J annuity options correctly.
'Anudeep A              2013.02.08      Bt-1688: YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
'Dinesh Kanojia(DK)     2013.01.30      BT-1262: YRS 5.0-1697: Message to display if multiple active employments Exist.
'Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
'Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
'Dinesh Kanojia(DK)     2013-10-24      BT:Attempted to divide by zero error on Retirement Partial Withdrawal Estimate.
'Shashank Patel			2013.11.12		BT-2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee (re-open) 
'Dinesh Kanojia         2015.04.27      BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
'B. Jagadeesh           2015.05.19      BT:2816: YRS 5.0-2495 Remove the print drop down box on Color Full Form
'Manthan Rajguru        2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod Prakash Pokale  2015.09.28      YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788) 
'chandrasekar.c         2015.11.02      YRS-AT-2610: Restriction of Death Benefit Annuity purchase if participant under minimun service retiree age(55) as of Death benefit annuity purchase restricted date(1/1/2019)
'chandrasekar.c         2015.10.27      YRS-AT-2486: Annuity Estimate calculator-incorrect reserves amount in the report and also in screen when a partial withdrawal is entered
'chandrasekar.c         2015.10.03      YRS-AT-2554: Annuity Estimate Calculator should not restrict QDRO based on yMCA Account balances (TrackIT 23261)
'chandrasekar.c         2015.11.19      YRS-AT-2479: Partial Withdrawal Estimate is allowed or not for terminated Participant with YMCA Legacy Account Balance
'chandrasekar.c         2015.12.14      YRS-AT-2329: Annuity Estimate Calculator - No access to the End Work Date field or Future Salary Effective Date field
'Manthan Rajguru        2016.01.14      YRS-AT-2151: Color estimate long form in batch estimates (retirement annuity estimates)
'Chandra sekar.c        2016.01.19      YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
'Chandra sekar.c        2016.02.22      YRS-AT-2752 - Retirement estimate not calculating projected reserves when estimation is performed for Both plan.
'Chandra sekar.c        2016.03.03      YRS-AT-2659 - YRS bug -PRA calculator should allow exclusion of account then partial withdrawal (TrackIT 24145)
'Chandra sekar.c        2016.05.27      YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
'Chandra sekar.c        2016.06.22      YRS-AT-3010 - YRS bug-annuity estimate calculator not counting partial withdrawal against estimates(TrackIT 26458)
'Manthan Rajguru        2017.02.27      YRS-AT-2625 - YRS enh: change in disability Annuity calculations (TrackIT 24012)
'Sanjay GS Rawat        2017.03.09      YRS-AT-2625 - YRS enh: change in disability Annuity calculations (TrackIT 24012) 
'Santosh Bura           2017.03.09      YRS-AT-2625 - YRS enh: change in disability Annuity calculations (TrackIT 24012)  
'Pramod Prakash Pokale  2017.03.14      YRS-AT-2625 - YRS enh: change in disability Annuity calculations (TrackIT 24012)
'Manthan Rajguru        2017.03.24      YRS-AT-2625 - YRS enh: change in disability Annuity calculations (TrackIT 24012)  
'Sanjay GS Rawat        2017.04.07      YRS-AT-3390 - YRS bug: Annuity calculations Estimates Screen (TrackIT 24012) 
'Pramod Prakash Pokale  2017.04.19      YRS-AT-3390 - YRS bug: Annuity calculations Estimates Screen (TrackIT 24012) 
'Pramod Prakash Pokale  2017.05.26      YRS-AT-2625 - YRS enh: change in disability Annuity calculations (TrackIT 24012)
'Pramod Prakash Pokale  2017.06.13      YRS-AT-2625 - YRS enh: change in disability Annuity calculations (TrackIT 24012)
'Pramod Prakash Pokale  2017.11.16      YRS-AT-3328 - YRS bug-annuity estimates not allowing exclusion of certain accounts (TrackIT 28917) 
'Ranu patel             2018.11.01      YRS-AT-4133 -  YRS enh: Annuity Estimate calculator & Goal Calculator: RDB Plan Rule change NEEDED by DECEMBER 2018 (TrackIT 32497)
'Shilpa N               02/26/2019      YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)   
'Shilpa N               04/08/2019      YRS-AT-3392 -  YRS enh: Retirement estimates generate error message if Annual Salary % is entered without a month (TrackIT 29356) 
'Megha Lad              2019.04.11      YRS-AT-4127 - YRS bug-wrong warning message in Annuity Estimate Calculator for YMCA Account balance max
'Megha Lad              2020.02.05      YRS-AT-4769 - YRS enhancement-new Secure Act rule for Annuity Estimate and Annuity Purchase screens (TrackIt- 41078)
'Manthan Rajguru        2020.05.22      YRS-AT-4885 - Allow 0% Interest in annuity estimate calculator
'********************************************************************************************************************
#Region "Imports"
Imports YMCARET.YmcaBusinessObject
Imports System
Imports System.Data.SqlClient
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports System.Web.Services 'MMR | 2017.03.03 | YRS-AT-2625 | Added namespace to use web attributes

#End Region

Public Class RetirementEstimateWebForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    ''START: Shilpa N | 02/26/2019 | YRS-AT-4248 |Commented the existing code. Wrong Form name was passing changed with the right one.
    'Dim strFormName As String = New String("RetirementEstimateWebForm.aspx")                                 
    Dim strFormName As String = New String("FindInfo.aspx?Name=Estimates")
    'END: Shilpa N | 02/26/2019 | YRS-AT-4248 |Commented the existing code. Wrong Form name was passing changed with the right one.
    'End issue id YRS 5.0-940

#Region "Declarations for variables"

    Public Enum enumMessageBoxType
        Javascript = 0
        DotNet = 1
    End Enum
    'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
    Public Enum BeneficiaryType
        Manual = 0
        Spouse = 1
        NonSpouse = 2
    End Enum

    Dim tnFullBalance As Decimal
    Dim g_String_Exception_Message As String
    Dim dsElectiveAccountsDet As DataSet
    Dim ssAnnuityAlreadyBought As Boolean = False
    Dim businessLogic As RetirementBOClass
    Dim dsSessionStoreDataSet As DataSet
    'YRS 5.0-657 : Estimate with SSL amounts
    Private Const SSLUnAvailableValue As String = "**N/A"

    Protected WithEvents DropDownListProjInterest2 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelProjectedInterest As System.Web.UI.WebControls.Label
    Protected WithEvents ListBoxProjectedInterest As System.Web.UI.WebControls.ListBox
    Protected WithEvents DropDownListProjInterest1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator2 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents CustomValidatorStartDate As WebControls.CustomValidator
    Protected WithEvents CustomvalidatorDataGridElectiveAccounts As WebControls.CustomValidator
    Protected WithEvents LabelRetirementType As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListPlanType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelPlanType As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBox1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelRetireeAge As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxRetireeAge As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonFormOK As System.Web.UI.WebControls.Button
    Protected WithEvents ValidationSummaryRetirementEstimate As System.Web.UI.WebControls.ValidationSummary
    'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Refactoring by renaming the default name to appropriate name
    Protected WithEvents CustomValidatorAnnualSalaryIncreaseEffDate As System.Web.UI.WebControls.CustomValidator
    'End:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Refactoring by renaming the default name to appropriate name
    'Protected WithEvents DatagridElectiveRetirementAccounts As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonUpdateEmployment As System.Web.UI.WebControls.Button
    Protected WithEvents CustomValidatorFutureSalaryDate As System.Web.UI.WebControls.CustomValidator
    'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Added new custom validator control for End Work date to be consistent in showing * text
    Protected WithEvents CustomValidatorEndWorkDate As System.Web.UI.WebControls.CustomValidator
    'End:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Added new custom validator control for End Work date to be consistent in showing * text
    Protected WithEvents LabelRetirementPlan As System.Web.UI.WebControls.Label
    Protected WithEvents LabelWarningMessage As System.Web.UI.WebControls.Label
    Protected WithEvents LabelMultipleEmpExists As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRefundMessage As System.Web.UI.WebControls.Label
    Protected WithEvents LabelSavingsPlan As System.Web.UI.WebControls.Label
    'Protected WithEvents Popcalendar3 As RJS.Web.WebControl.PopCalendar 'PPP | 2015.09.28 | YRS-AT-2596
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    ' BT:892-YRS 5.0-1359 : Disability Estimate form
    Protected WithEvents PanelDisability As System.Web.UI.WebControls.Panel
    Protected WithEvents btnPRADisability As System.Web.UI.WebControls.Button
    'YRS 5.0-657 : Estimate with SSL amounts
    Protected WithEvents LabelSSLWarningMessage As System.Web.UI.WebControls.Label
    'Protected WithEvents DatagridElectiveRetirementAccounts_OnCheckedChanged
    'Ashish YRS 5.0-1345 :BT-859
    Protected WithEvents PanelAlternatePayee As System.Web.UI.WebControls.Panel
    Protected WithEvents btnAlternatePayee As System.Web.UI.WebControls.Button

    'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
    Protected WithEvents LabelJAnnuityUnAvailMessage As System.Web.UI.WebControls.Label
    Protected WithEvents labelRelationship As Label
    Protected WithEvents DropDownRelationShip As DropDownList
    Protected WithEvents LabelNoBeneficiary As Label
    'YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary 
    Protected WithEvents DataGridAnnuityBeneficiaries As DataGrid
    '2012.05.21   SP   BT-976/YRS 5.0-1507 - Reopned 
    Protected WithEvents ButtonClearBeneficiary As Button
    'START: MMR | 2017.02.22 | YRS-AT-2625 | Declared controls
    Protected WithEvents DatagridManualTransactionList As DataGrid
    Protected WithEvents lnkManualTransaction As HtmlAnchor
    Protected WithEvents hdnManualTransaction As HiddenField
    Protected WithEvents lblMessageManualTransaction As Label
    'END: MMR | 2017.02.22 | YRS-AT-2625 | Declared controls
    Protected WithEvents LabelRDBWarningMessage As System.Web.UI.WebControls.Label 'Ranu : YRS-AT-4133
    Protected WithEvents CustomValidatorDDLAnnualSalaryIncreaseEffDate As System.Web.UI.WebControls.CustomValidator 'Shilpa N | 04/08/2019 | YRS-AT-3392 | Added new custom validator
#End Region
    'Added By Ashish 07-Apr-2009 for Phase V
#Region "DatagridElectiveRetirementAccounts Cell Index Constant"
    Private Const RET_SEL_ACCT_CHK As Integer = 0
    Private Const RET_ACCT_TYPE As Integer = 1
    Private Const RET_ACCT_TOTAL As Integer = 2
    Private Const RET_PROJ_BALANCE As Integer = 3
    Private Const RET_EXISTING_CONTRI As Integer = 4
    Private Const RET_CONTRI_RATE As Integer = 5
    Private Const RET_CONTRI_TYPE As Integer = 6
    Private Const RET_CONTRIBUTION As Integer = 7
    Private Const RET_START_DATE As Integer = 8
    Private Const RET_STOP_DATE As Integer = 9
    Private Const RET_EMP_EVENT_ID As Integer = 10
    Private Const RET_VOL_ACCT As Integer = 11
    Private Const RET_BASIC_ACCT As Integer = 12
    Private Const RET_PLANE_TYPE As Integer = 13
    Private Const RET_YMCA_AMT_TOTAL As Integer = 14
    Private Const RET_PERSONAL_TOTAL As Integer = 15
    Private Const RET_LEGACY_ACCT_TYPE As Integer = 16


#End Region
#Region "DatagridElectiveSavingsAccounts Cell Index Constant"
    Private Const SAV_SEL_ACCT_CHK As Integer = 0
    Private Const SAV_ACCT_TYPE As Integer = 1
    Private Const SAV_ACCT_TOTAL As Integer = 2
    Private Const SAV_PROJ_BALANCE As Integer = 3
    Private Const SAV_EXISTING_CONTRI As Integer = 4
    Private Const SAV_CONTRI_RATE As Integer = 5
    Private Const SAV_CONTRI_TYPE As Integer = 6
    Private Const SAV_CONTRIBUTION As Integer = 7
    Private Const SAV_START_DATE As Integer = 8
    Private Const SAV_STOP_DATE As Integer = 9
    Private Const SAV_EMP_EVENT_ID As Integer = 10
    Private Const SAV_VOL_ACCT As Integer = 11
    Private Const SAV_BASIC_ACCT As Integer = 12
    Private Const SAV_PLANE_TYPE As Integer = 13
    Private Const SAV_LEGACY_ACCT_TYPE As Integer = 14
    'Private Const SAV_YMCA_AMT_TOTAL As Integer = 13
    'Private Const SAV_PERSONAL_TOTAL As Integer = 14


#End Region

    'Priya 12-Feb-09:YRS 5.0-620 Incorrect message for person with status RA
#Region "NonEmpty Functions"
    Private Function isNonEmpty(ByRef ds As DataSet) As Boolean
        If ds Is Nothing Then Return False
        If ds.Tables.Count = 0 Then Return False
        If ds.Tables(0).Rows.Count = 0 Then Return False
        Return True
    End Function
    Private Function isNonEmpty(ByRef obj As Object) As Boolean
        If obj Is Nothing Then Return False
        If TypeOf (obj) Is DataSet Then
            Return isNonEmpty(CType(obj, DataSet))
        End If
        If Convert.ToString(obj).Trim = String.Empty Then Return False
        Return True
    End Function
#End Region
    'End 12-Feb-09:YRS 5.0-620

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents PageCaption As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents TextBoxPraAssumption As System.Web.UI.WebControls.TextBox
    Protected WithEvents labelPRAAssumption As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonPRA As System.Web.UI.WebControls.Button
    Protected WithEvents myPlaceholder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents txtRetireeBirthday As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRetirementAge As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRetireeAge As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblProjectedInterest As System.Web.UI.WebControls.Label
    Protected WithEvents lblBenifiaciaryBirthDay As System.Web.UI.WebControls.Label
    Protected WithEvents txtBenifiaciaryBirthDay As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblAnnualSalaryIncreased As System.Web.UI.WebControls.Label
    Protected WithEvents lblModifiedSalary As System.Web.UI.WebControls.Label
    Protected WithEvents txtModifiedSalary As System.Web.UI.WebControls.TextBox
    Protected WithEvents tabStripRetirementEstimate As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents MultiPageRetirementEstimate As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents txtRetirementDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblSSIncrease As System.Web.UI.WebControls.Label
    Protected WithEvents lblSSDecrease As System.Web.UI.WebControls.Label
    Protected WithEvents DatatGridSocialSecurityLevel As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridEmployment As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblSalaryAverage As System.Web.UI.WebControls.Label
    Protected WithEvents txtSalaryAverage As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblLastPaidMonth As System.Web.UI.WebControls.Label
    Protected WithEvents lblModifiedSal As System.Web.UI.WebControls.Label
    Protected WithEvents txtLastPaidSalary As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtModifiedSal As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblStartWorkDate As System.Web.UI.WebControls.Label
    Protected WithEvents txtStartWorkDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblEndWorkDate As System.Web.UI.WebControls.Label
    Protected WithEvents txtEndWorkDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblAnnualSalaryIncrease As System.Web.UI.WebControls.Label

    Protected WithEvents lblLabel1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtLabel1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblProjected As System.Web.UI.WebControls.Label
    Protected WithEvents txtProjected As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblAnnualSalary As System.Web.UI.WebControls.Label
    Protected WithEvents lblLabel As System.Web.UI.WebControls.Label

    Protected WithEvents DataGridPage3Refunds As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DatagridElectiveRetirementAccounts As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DatagridElectiveSavingsAccounts As System.Web.UI.WebControls.DataGrid
    Protected WithEvents CheckBoxRetirementPlan As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBoxSavingsPlan As System.Web.UI.WebControls.CheckBox
    Protected WithEvents hdnDecPartialValue As System.Web.UI.WebControls.HiddenField
    Protected WithEvents hdnDecSavingValue As System.Web.UI.WebControls.HiddenField

    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents Menu2 As skmMenu.Menu

    Protected WithEvents Buttoncalculate As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonPrint As System.Web.UI.WebControls.Button

    'Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label

    Protected WithEvents LabelSSIncrease As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPage1SSIncrease As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSSDecrease As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPage1SSDecrease As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSalaryAverage As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSalaryAverage As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelLastPaidMonth As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLastPaidSalary As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLastPaidMonthDate As System.Web.UI.WebControls.Label

    Protected WithEvents LabelModifiedSal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxModifiedSal As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelFutureSalary As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFutureSalary As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelFutureSalaryEffDate As System.Web.UI.WebControls.Label
    'Commented by CS: 12/14/2015 YRS-AT-2329 : For Changing the DateUserControl to Custom Calender Controls
    'Protected WithEvents TextBoxFutureSalaryEffDate As YMCAUI.DateUserControl
    'Protected WithEvents TextBoxAnnualSalaryIncreaseEffDate As YMCAUI.DateUserControl

    'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Replacing the earlier user control with new custom control as earlier user control did not had flexibility to enable/disable control.    
    Protected WithEvents TextBoxFutureSalaryEffDate As CustomControls.CalenderTextBox
    Protected WithEvents TextBoxAnnualSalaryIncreaseEffDate As CustomControls.CalenderTextBox
    Protected WithEvents TextBoxEndWorkDate As CustomControls.CalenderTextBox
    'End:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Replacing the earlier user control with new custom control as earlier user control did not had flexibility to enable/disable control.    

    Protected WithEvents LabelStartWorkDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxStartWorkDate As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelEndWorkDate As System.Web.UI.WebControls.Label
    'Commented by CS: 12/14/2015 YRS-AT-2329 : For Changing the DateUserControl to Custom Calender Controls
    'Protected WithEvents TextBoxEndWorkDate As YMCAUI.DateUserControl


    Protected WithEvents LabelAnnualSalaryIncrease As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownAnnualSalaryIncrease As System.Web.UI.WebControls.DropDownList

    'Protected WithEvents LabelCurrentYearInterest As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxCurrentYearInterest As System.Web.UI.WebControls.TextBox
    Protected WithEvents ListBoxProjectedYearInterest As System.Web.UI.WebControls.ListBox

    Protected WithEvents LabelProjected As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxProjected As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelAnnualSalary As System.Web.UI.WebControls.Label

    Protected WithEvents LabelLabel As System.Web.UI.WebControls.Label
    Protected WithEvents ListBoxLabel1 As System.Web.UI.WebControls.ListBox

    Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxFirstName As System.Web.UI.WebControls.TextBox

    Protected WithEvents TextboxFirstNameSavings As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxLastNameSavings As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxBeneficiaryBirthDateSavings As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxLastName As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelBirthDate As System.Web.UI.WebControls.Label
    'START: PPP | 2015.09.28 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788)
    'Protected WithEvents TextboxBeneficiaryBirthDate As TextBox
    Protected WithEvents TextboxBeneficiaryBirthDate As CustomControls.CalenderTextBox
    'END: PPP | 2015.09.28 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788)

    Protected WithEvents LabelRetiredDeathBenefit As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxRetiredDeathBenefit As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelPercentageToUse As System.Web.UI.WebControls.Label

    Protected WithEvents LabelAmountToUse As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxAmountToUse As System.Web.UI.WebControls.TextBox

    Protected WithEvents DropDownListRetirementType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DropdownlistPercentageToUse As System.Web.UI.WebControls.DropDownList

    Protected WithEvents TextboxSSIncrease As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelEmployment As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListEmployment As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelMultipleEmp As System.Web.UI.WebControls.Label

    Protected WithEvents LabelAccountType As System.Web.UI.WebControls.Label

    Protected WithEvents LabelContributionType As System.Web.UI.WebControls.Label
    Protected WithEvents LabelContribAmt As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxContribAmtRet As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelRetireeBirthday As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxRetireeBirthday As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelRetirementDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxRetirementDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelRetirementAge As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxRetirementAge As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelBenifiaciaryBirthDay As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxBenifiaciaryBirthDay As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxFromBenefitValue As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelContributionPercentage As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents PopcalendarDate As RJS.Web.WebControl.PopCalendar
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button

    Protected WithEvents YMCA_Toolbar_WebUserControl1 As YMCAUI.YMCA_Toolbar_WebUserControl
    Protected WithEvents YMCA_Footer_WebUserControl1 As YMCAUI.YMCA_Footer_WebUserControl
    'Added by Ashish phase V changes
    Protected WithEvents LabelAcctBalExceedTresholdLimit As System.Web.UI.WebControls.Label
    Protected WithEvents LabelPartialWithdrawal As System.Web.UI.WebControls.Label
    Protected WithEvents LabelEstimateDataChangedMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblProjectedReserves As System.Web.UI.WebControls.Label
    Protected WithEvents lblDeathBenefitUsed As System.Web.UI.WebControls.Label
    Protected WithEvents txtProjectedReserves As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDeathBenefitUsed As System.Web.UI.WebControls.TextBox
    '2010.06.21-Ashish-Issue YRS 5.0.1115
    'Start-- Manthan Rajguru | 2016.01.14 | YRS-AT-2151 | Commented the existing code
    'Protected WithEvents btnPRAShort As System.Web.UI.WebControls.Button
    'Protected WithEvents btnPRAColor As System.Web.UI.WebControls.Button
    'End-- Manthan Rajguru | 2016.01.14 | YRS-AT-2151 | Commented the existing code
    Protected WithEvents tblPrintOption As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblPrintOption As System.Web.UI.WebControls.Label
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    Protected WithEvents chkRetirementAccount As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtRetirementAccount As System.Web.UI.WebControls.TextBox

    Protected WithEvents chkSavingPartialAmount As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtSavingPartialAmount As System.Web.UI.WebControls.TextBox

    Protected WithEvents lblSavingPartialAmountEligible As System.Web.UI.WebControls.Label
    Protected WithEvents lblRetirementPartialAmountEligible As System.Web.UI.WebControls.Label

    '2012.07.11 SP : BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page -Start
    Protected WithEvents LabelProjFinalYrsSalary As Label
    Protected WithEvents TextBoxProjFinalYrsSalary As TextBox
    '2012.07.11 SP : BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page -End
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    'Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request
    Protected WithEvents tblIDMcheck As HtmlTable
    Protected WithEvents chkIDM As CheckBox
    Protected WithEvents ButtonPRAFull As Button
    '23.11.2015:chandrasekar.c :YRS-Ticket:2610 Showing dealth benefit annuity purchase message
    'Commented by CS: 12/11/2015- not showing the message earlier thought of, to be consistent with other screens (retirement processing) where for the same requirement message is not shown.
    ' Protected WithEvents LabelDeathBenefitRestrictionMessage As System.Web.UI.WebControls.Label 


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Code for Loading controls and DropDownLists"
    Private Sub SetControlAttributes()
        Try
            TextBoxModifiedSal.Attributes.Add("onkeypress", "JavaScript:ValidateDecimal();")
            TextBoxModifiedSal.Attributes.Add("onBlur", "JavaScript:_OnBlur_ModifiedSalary();")
            TextBoxFutureSalary.Attributes.Add("onkeypress", "JavaScript:ValidateDecimal();")
            TextboxAmountToUse.Attributes.Add("onkeypress", "JavaScript:ValidateDecimal();")
            TextboxFromBenefitValue.Attributes.Add("onkeypress", "JavaScript:ValidateDecimal();")
            TextBoxRetirementAge.Attributes.Add("onkeypress", "JavaScript:ValidateNumeric();")
            txtRetirementAccount.Attributes.Add("onkeypress", "JavaScript:ValidateNumeric();")
            txtSavingPartialAmount.Attributes.Add("onkeypress", "JavaScript:ValidateNumeric();")

            Me.TextBoxRetirementDate.Attributes.Add("onchange", "javascript:getRetireeRetirementDate();")
            Me.TextBoxRetirementAge.Attributes.Add("onchange", "javascript:RetireeDate();")
            Me.PopcalendarDate.Attributes.Add("onselectionchanged", "javascript:AgeCalc();")
            TextboxFromBenefitValue.ReadOnly = False
            TextBoxRetireeBirthday.ReadOnly = True
            TextBoxRetireeBirthday.BackColor = Color.LightGray
            'START: PPP | 2015.09.28 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788)
            ''2012.05.22 SP :BT-976/YRS 5.0-1507 - Reopned issue
            'Me.TextboxBeneficiaryBirthDate.Attributes.Add("onchange", "javascript:PostBackCalendarBenef();")
            'Me.Popcalendar3.Attributes.Add("OnSelectionChanged", "javascript:PostBackCalendarBenef();")
            ''2012.05.22 SP :BT-976/YRS 5.0-1507 - Reopned issue
            'TextboxBeneficiaryBirthDate.ReadOnly = True
            Me.TextboxBeneficiaryBirthDate.Attributes.Add("onchange", "javascript:PostBackCalendarBenef(this);")
            'END: PPP | 2015.09.28 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788)

            TextBoxPage1SSIncrease.ReadOnly = True
            TextBoxPage1SSIncrease.Text = "0.00"
            TextBoxPage1SSDecrease.ReadOnly = True
            TextBoxPage1SSDecrease.Text = "0.00"

            TextboxSSIncrease.ReadOnly = True

            'TextBoxFutureSalaryEffDate.rReadOnly = True

            'TextBoxAnnualSalaryIncreaseEffDate.rReadOnly = True 'Phase IV Changes

            'TextBoxEndWorkDate.rReadOnly = True
            'YRS 5.0-657 : Estimate with SSL amounts
            If ssAnnuityAlreadyBought = True Then
                TextboxFromBenefitValue.Enabled = False
            End If
        Catch
            Throw
        End Try
    End Sub
    Private Sub LoadDefaultValues()
        Dim i As Integer
        Try
            'START: MMR | 2020.05.22 | YRS-AT-4885 | Added for Interest rate project
            'For i = 1 To 100
            '    ListBoxProjectedYearInterest.Items.Add(i)
            'Next

            'For i = 1 To 15
            '    DropDownListProjInterest2.Items.Add(i)
            'Next
            'Allowing '0%' interest rate option for selection
            For i = 0 To 100
                ListBoxProjectedYearInterest.Items.Add(i)
            Next

            For i = 0 To 15
                DropDownListProjInterest2.Items.Add(i)
            Next
            'END: MMR | 2020.05.22 | YRS-AT-4885 | Added for Interest rate project

            'commented by Swopna in response to YRPS-4535 on 18-Jan-2008
            '***************************
            ' For i = 0 To 90
            'DropdownlistPercentageToUse.Items.Add(i)
            'Next
            '***************************

            'Added by Swopna in response to YRPS-4535 on 18-Jan-2008
            '***************************
            'Commented by Swopna in response to YRPS-4535 on 31-Jan-2008
            'For i = 1 To 90 Step 1
            '    DropdownlistPercentageToUse.Items.Add(i)
            'Next
            'DropdownlistPercentageToUse.Items.Add(0)
            '***************************

            'Added by Swopna in response to YRPS-4535 on 31-Jan-2008
            '***************************
            DropdownlistPercentageToUse.Items.Add(0)
            For i = 90 To 1 Step -1
                DropdownlistPercentageToUse.Items.Add(i)
            Next

            '***************************
            'START: SB | 03/16/2017 | YRS-AT-2625 | Instead of hardcoded interest rate, setting the configured interest rates by default in the projected interested dropdown
            'DropDownListProjInterest2.SelectedValue = 3   
            SetRetireType()
            SetProjectionInterestRate()
            'END: SB | 03/16/2017 | YRS-AT-2625 | Instead of hardcoded interest rate, setting the configured interest rates by default in the projected interested dropdown

            ListBoxProjectedYearInterest.SelectedValue = DropDownListProjInterest2.SelectedValue
            DropdownlistPercentageToUse.SelectedValue = 0

            For i = 0 To 100
                DropDownAnnualSalaryIncrease.Items.Add(i)
            Next
            DropDownAnnualSalaryIncrease.SelectedValue = 0
        Catch
            Throw
        End Try
    End Sub
    Private Sub SetControlFocus(ByVal TextBoxFocus As TextBox)
        Dim l_string_script As String
        Try
            l_string_script = "<script language='Javascript'>" & _
                            "var obj = document.getElementById('" & TextBoxFocus.ID & "');" & _
                            "if (obj!=null){if (obj.disabled==false){obj.focus();}}" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("script_futuresalary_focus")) Then
                Page.RegisterStartupScript("script_futuresalary_focus", l_string_script)
            End If
        Catch
            Throw
        End Try
    End Sub
    'Phase IV Changes
    Private Function IsRetirementBackDated() As Boolean
        Dim l_bool_return As Boolean = False

        If TextBoxRetirementDate.Text.ToString() <> "" Then
            If Convert.ToDateTime(TextBoxRetirementDate.Text) < DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("MM/dd/yyyy") Then
                l_bool_return = True
            End If
        End If

        Return l_bool_return
    End Function
    'Start:Added by chandrasekar.c on 2015.12.17 : YRS-AT-2329: Annuity Estimate Calculator - Checking whether Retirement date is current or Backed Date
    Private Function IsRetirementNotFutureDated() As Boolean
        Dim blnReturn As Boolean = True

        If TextBoxRetirementDate.Text.Trim() <> "" Then
            If Convert.ToDateTime(TextBoxRetirementDate.Text.Trim()) <= DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("MM/dd/yyyy") Then
                blnReturn = False
            End If
        End If

        Return blnReturn
    End Function
    'Start:Added by chandrasekar.c on 2015.12.17 : YRS-AT-2329: Annuity Estimate Calculator - Checking whether Retirement date is current or Backed Date

    'SP     2012.05.15   BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary - Start
    Private Sub BindRelationShipDropDown()
        Dim l_dataset_RelationShips As DataSet
        Dim dtRelationship As New DataTable
        Dim foundRows() As DataRow
        'Start:Modified by Anudeep:08:02:2012 For checking already dataset exists in a session variable on issue:YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
        l_dataset_RelationShips = Session("dataset_RelationShips")
        If l_dataset_RelationShips Is Nothing Then
            l_dataset_RelationShips = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.getRelationShips()
            Session("dataset_RelationShips") = l_dataset_RelationShips
        End If
        'End:Modified by Anudeep:08:02:2012 For checking already dataset exists in a session variable on issue:YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
        If (HelperFunctions.isNonEmpty(l_dataset_RelationShips)) Then
            dtRelationship = l_dataset_RelationShips.Tables(0).Clone()
        End If
        'Modified belowline by Anudeep:08:02:2012 For binding only active relationships on issue:YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
        foundRows = l_dataset_RelationShips.Tables(0).Select("Active = True And CodeValue not in('ES','IN','TR')")
        bindrelationships(foundRows)
        'For Each drRow As DataRow In foundRows
        '    dtRelationship.ImportRow(drRow)
        'Next
        'DropDownRelationShip.DataSource = dtRelationship
        'DropDownRelationShip.DataTextField = "Description"
        'DropDownRelationShip.DataValueField = "CodeValue"
        'DropDownRelationShip.DataBind()
        'DropDownRelationShip.Items.Insert(0, "Select") '2012.05.21 SP: BT-976/YRS 5.0-1507 - Reopned issue
    End Sub
    'SP     2012.05.15   BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary - End
    Private Function LoadEmploymentDetails(ByVal personId As String) As Boolean
        Dim dsPersonEmploymentDetails As New DataSet
        Dim dsActiveEmploymentEvent As New DataSet
        Dim dsElectiveAccounts As New DataSet
        Dim bReturnval As Boolean
        Dim i As Integer
        Try
            '16-Feb-2011 Added by Sanket for disabiling controls when value in drop down is changed
            bReturnval = False
            'If Page.IsPostBack = False Or Me.RetireType = "DISABL" Then 'MMR | 2017.03.14| Commented existing code to allow load employement details on change of retirement type in dropdown
            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
            'dsPersonEmploymentDetails = RetirementBOClass.SearchRetEmpInfo(personId)
            'ASHISH:2010.10.11 code Added for YRS 5.0-855 BT-624
            dsPersonEmploymentDetails = RetirementBOClass.SearchRetEmpInfo(Me.FundEventId, Me.RetireType, IIf(String.IsNullOrEmpty(TextBoxRetirementDate.Text), "01/01/1900", TextBoxRetirementDate.Text)) 'MMR | 2017.03.09 | YRS-AT-2625 | Addeed parameter for retire type and retirement date
            If dsPersonEmploymentDetails.Tables.Count > 0 Then
                If dsPersonEmploymentDetails.Tables(0).Rows.Count > 0 Then
                    Dim l_AE_guiUniqueID As String = String.Empty
                    Dim l_AE_guiYmcaID As String = String.Empty

                    dsActiveEmploymentEvent = RetirementBOClass.SearchActiveEmploymentEvents(personId)
                    If Not dsActiveEmploymentEvent.Tables.Count = 0 Then
                        If Not dsActiveEmploymentEvent.Tables(0).Rows.Count = 0 Then
                            'get the YMCAID of the ActiveEmployment Event
                            If Not dsActiveEmploymentEvent.Tables(0).Rows(0).Item("guiUniqueID") Is DBNull.Value Then
                                l_AE_guiUniqueID = dsActiveEmploymentEvent.Tables(0).Rows(0).Item("guiUniqueID").ToString()
                            End If

                            If Not dsActiveEmploymentEvent.Tables(0).Rows(0).Item("guiYmcaID") Is DBNull.Value Then
                                l_AE_guiYmcaID = dsActiveEmploymentEvent.Tables(0).Rows(0).Item("guiYmcaID").ToString()
                            End If
                            'ASHISH:2010.10.11 code Added for YRS 5.0-855 BT-624
                            If dsActiveEmploymentEvent.Tables(0).Rows.Count > 1 Then
                                'commented by Anudeep on 22-sep for BT-1126
                                'Dim strMsg = "Multiple active employments exist. Please 'Update Employment' and terminate any that are to be excluded from the estimate'"//commented by Anudeep on 22-sep for BT-1126


                                'Dinesh Kanojia:2013.01.30:BT-1262:YRS 5.0-1697: Message to display if multiple active employments Exist.

                                'Dim strMsg = getmessage("MESSAGE_RETIREMENT_ESTIMATE_MULTIPLE_ACTIVE_EMPLOYEMENTS")
                                Dim strMsg = getmessage("MESSAGE_REIREMENT_ESTIMATE_MULTIPLE_ACTIVE_EMPLOYMENTS_EXISTS")

                                LabelMultipleEmpExists.Text = strMsg
                                'LabelMultipleEmpExists.Text = String.Empty
                                LabelMultipleEmpExists.Visible = True


                                DisabledControls()
                                MessageBox.Show(PlaceHolder1, "YRS", getmessage("MESSAGE_REIREMENT_ESTIMATE_MULTIPLE_ACTIVE_EMPLOYMENTS_EXISTS"), MessageBoxButtons.Stop)
                                'Exit Sub
                                bReturnval = True
                            End If
                        End If
                    End If

                    If l_AE_guiYmcaID.ToString() <> String.Empty Then
                        'ASHISH:2010.10.11 code Added for YRS 5.0-855 BT-624
                        'dsElectiveAccounts = RetirementBOClass.SearchElectiveAccounts(personId, l_AE_guiYmcaID)
                        dsElectiveAccounts = RetirementBOClass.SearchElectiveAccounts(personId, "")
                        Session("dsElectiveAccounts") = dsElectiveAccounts
                    End If

                    'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624, I found that this value not used any where in program.
                    '' Mark the latest Start work date
                    'Session("LatestStartWorkDate") = dsPersonEmploymentDetails.Tables(0).Rows(0).Item("Start").ToString()
                    '' Mark the earliest Start work date

                    Dim k As Int16 = dsPersonEmploymentDetails.Tables(0).Rows.Count - 1
                    Session("EarliestStartWorkDate") = dsPersonEmploymentDetails.Tables(0).Rows(k).Item("Start").ToString()

                    PopulateActiveEmployments(dsPersonEmploymentDetails)

                    ' If no active employment is present then disable the 
                    ' Voluntary account grid and display existing account contribution as read only
                    If DropDownListEmployment.Items.Count <= 1 Then
                        If Not dsPersonEmploymentDetails.Tables(0).Rows.Count = 0 Then
                            Session("DefaultEmpEventID") = dsPersonEmploymentDetails.Tables(0).Rows(0).Item("guiEmpEventId").ToString()
                        End If
                    End If

                    Session("dsPersonEmploymentDetails") = dsPersonEmploymentDetails

                    'Loading Employment Information
                    Me.DataGridEmployment.DataSource = dsPersonEmploymentDetails
                    Me.DataGridEmployment.DataBind()
                End If
                'Priya 16-Jan-2006 :YRS 5.0-656 Not allowing Plan-wise estimate for QDRO fund event
                Session("dsPersonEmploymentDetails") = dsPersonEmploymentDetails
                'End YRS 5.0-656 16-Jan-2006 
            End If
            'End If 'MMR | 2017.03.14| Commented existing code to allow load employement details on change of retirement type in dropdown

            ShowEmploymentDetails()
            Return bReturnval
        Catch
            Throw
        End Try
    End Function
    'Dinesh Kanojia:2013.01.30:BT-1262:YRS 5.0-1697: Message to display if multiple active employments Exist.
    Private Sub DisabledControls()
        Buttoncalculate.Enabled = False
        ButtonPrint.Enabled = False
        Me.tabStripRetirementEstimate.SelectedIndex = 1
        Me.MultiPageRetirementEstimate.SelectedIndex = Me.tabStripRetirementEstimate.SelectedIndex
        MultiPageRetirementEstimate.Enabled = False
        TextBoxRetirementDate.Enabled = False
        PopcalendarDate.Enabled = False
        DropDownListRetirementType.Enabled = False
        DropDownListPlanType.Enabled = False
        TextBoxRetirementAge.Enabled = False
        ListBoxProjectedYearInterest.Enabled = False
        DropDownListProjInterest2.Enabled = False
    End Sub

    Private Sub ShowEmploymentDetails()
        Try
            If DataGridEmployment.Items.Count > 0 Then
                If DataGridEmployment.SelectedIndex < 0 Then
                    DataGridEmployment.SelectedIndex = 0
                End If

                Session("selectedEmployment") = DataGridEmployment.SelectedItem.Cells(6).Text
            End If

            'Added by Sanket 
            'If Page.IsPostBack = False Or Me.RetireType.Trim.ToUpper() = "DISABL" Then 'MMR | 2017.03.14| Commented existing code to allow load employement details on change of retirement type in dropdown
            saveActiveSalaryInformation() 'Phase IV Changes
            'ASHISH:2010.10.11 code Added for YRS 5.0-855 BT-624
            loadEmploymentActualSalaryDetails()
            'End If 'MMR | 2017.03.14| Commented existing code to allow load employement details on change of retirement type in dropdown

            ' Display the saved if already modified / actual salary information if it was not modified, only if it is Active Employment
            displayEmploymentModifiedSalaryInformation()
        Catch
            Throw
        End Try
    End Sub
    Private Sub loadEmploymentActualSalaryDetails()

        Dim l_string_EmploymentTermDate As String = String.Empty
        Dim dsSalInfo As New DataSet
        Try

            If DataGridEmployment.Items.Count > 0 Then

                'check if the employment is actually active then just hold the id of the selected employment
                If DataGridEmployment.SelectedItem.Cells(5).Text.ToString() = "&nbsp;" Then
                    l_string_EmploymentTermDate = String.Empty
                Else
                    l_string_EmploymentTermDate = DataGridEmployment.SelectedItem.Cells(5).Text.ToString()
                End If
                'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624 Start
                '    'ASHISH:2009.11.17 -Change PersiD parameter with fundEventID
                '    ' Change as a result of having multiple active employments
                '    If l_string_EmploymentTermDate = String.Empty Then
                '        'dsSalInfo = RetirementBOClass.SearchRetEmpSalInfo( _
                '        '                            DataGridEmployment.SelectedItem.Cells(1).Text(), _
                '        '                            DataGridEmployment.SelectedItem.Cells(2).Text(), _
                '        '                            Convert.ToDateTime(DataGridEmployment.SelectedItem.Cells(4).Text()), _
                '        '                            "")
                '        dsSalInfo = RetirementBOClass.SearchRetEmpSalInfo( _
                '                                    Me.FundEventId, _
                '                                    DataGridEmployment.SelectedItem.Cells(2).Text(), _
                '                                    Convert.ToDateTime(DataGridEmployment.SelectedItem.Cells(4).Text()), _
                '                                    "")
                '    Else
                '        'ASHISH:2009.11.17 -Change PersiD parameter with fundEventID
                '        'dsSalInfo = RetirementBOClass.SearchRetEmpSalInfo( _
                '        '                        DataGridEmployment.SelectedItem.Cells(1).Text().ToUpper(), _
                '        '                        DataGridEmployment.SelectedItem.Cells(2).Text().ToUpper(), _
                '        '                        Convert.ToDateTime(DataGridEmployment.SelectedItem.Cells(4).Text()), _
                '        '                        Convert.ToDateTime(l_string_EmploymentTermDate))
                '        dsSalInfo = RetirementBOClass.SearchRetEmpSalInfo( _
                '                                Me.FundEventId, _
                '                                DataGridEmployment.SelectedItem.Cells(2).Text().ToUpper(), _
                '                                Convert.ToDateTime(DataGridEmployment.SelectedItem.Cells(4).Text()), _
                '                                Convert.ToDateTime(l_string_EmploymentTermDate))
                '    End If
                'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624 End
            End If

            If Not Session("dsPersonEmploymentDetails") Is Nothing Then
                dsSalInfo = DirectCast(Session("dsPersonEmploymentDetails"), DataSet)
            End If

            If dsSalInfo.Tables.Count = 0 And Me.OrgBenTypeIsQDROorRBEN = False Then
                Session("EmpHistoryInfoNotset") = True
                'commented by Anudeep on 22-sep for BT-1126
                'Call ShowCustomMessage("Unable to initialize employment history for the member.", enumMessageBoxType.DotNet)//commented by Anudeep on 22-sep for BT-1126

                Call ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_ESTIMATE_UNABLE_TO_INITIALIZE"), enumMessageBoxType.DotNet)
                Exit Sub
            End If

            'Commented by Ashish 30-July-2008 YRS 5.0-445 ,Start
            'TextBoxSalaryAverage.Text = "0.00"
            'TextBoxLastPaidSalary.Text = "0.00"
            'LabelLastPaidMonthDate.Text = "//"
            'Commented by Ashish 30-July-2008 YRS 5.0-445 ,End

            TextBoxModifiedSal.Text = "0.00"


            TextBoxFutureSalary.Text = "0.00"
            TextBoxFutureSalaryEffDate.Text = ""

            TextBoxStartWorkDate.Text = ""
            TextBoxEndWorkDate.Text = ""

            TextBoxAnnualSalaryIncreaseEffDate.Text = "" 'Phase IV Changes
            DropDownAnnualSalaryIncrease.SelectedValue = 0 'Phase IV Changes


            If dsSalInfo.Tables.Count > 0 Then
                If dsSalInfo.Tables(0).Rows.Count > 0 Then
                    If IsPostBack = False AndAlso dsSalInfo.Tables(1).Rows(0).IsNull("LastSalPaidMonth") = False Then
                        TextBoxRetirementDate.Text = Convert.ToDateTime(Convert.ToDateTime(dsSalInfo.Tables(1).Rows(0).Item("LastSalPaidMonth")).Month & "/01/" & _
                        Convert.ToDateTime(dsSalInfo.Tables(1).Rows(0).Item("LastSalPaidMonth")).Year()).ToString("MM/dd/yyyy")

                        'Age Calculation
                        TextBoxRetirementAge.Text = Convert.ToDecimal(DateDiff(DateInterval.Day, Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime(TextBoxRetirementDate.Text)) / 365).ToString.Substring(0, 2)
                        'Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                        If (TextBoxRetirementAge.Text <> "") Then
                            'Added by chandrasekar.c:Static 55 change to dynamic by using property MinAgeToRetire
                            If Convert.ToInt32(TextBoxRetirementAge.Text.Trim) >= Me.MinAgeToRetire Then
                                chkRetirementAccount.Visible = True
                                txtRetirementAccount.Visible = True

                                chkSavingPartialAmount.Visible = True
                                txtSavingPartialAmount.Visible = True
                            End If
                        End If

                        Me.RetirementDate = TextBoxRetirementDate.Text
                    End If


                    'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                    'Added by Ashish 30-July-2008 YRS 5.0-445 ,Start
                    'If Not l_datarow_SelectedEmpSalInfo("AvgSalary") Is DBNull.Value Then
                    If dsSalInfo.Tables(0).Rows(0).IsNull("AvgSalaryPerEmployment") = False Then
                        TextBoxModifiedSal.Text = Convert.ToDecimal(dsSalInfo.Tables(0).Rows(0).Item("AvgSalaryPerEmployment")).ToString("f2")
                    Else
                        TextBoxModifiedSal.Text = "0.00"
                    End If
                    'Commented by Ashish 30-July-2008 YRS 5.0-445 ,Start
                    'If Not l_datarow_SelectedEmpSalInfo("AvgSalary") Is DBNull.Value Then
                    '    TextBoxSalaryAverage.Text = l_datarow_SelectedEmpSalInfo("AvgSalary").ToString()
                    '    TextBoxSalaryAverage.Text = TextBoxSalaryAverage.Text.Substring(0, TextBoxSalaryAverage.Text.IndexOf(".")) & ".00"
                    '    TextBoxModifiedSal.Text = TextBoxSalaryAverage.Text
                    'End If
                    'Commented by Ashish 30-July-2008 YRS 5.0-445 ,End


                    'Added by Ashish 30-July-2008 YRS 5.0-445 ,End
                    'Commented by Ashish 30-July-2008 YRS 5.0-445 ,Start
                    'If Not l_datarow_SelectedEmpSalInfo("LastPaidSal") Is DBNull.Value Then
                    '    TextBoxLastPaidSalary.Text = l_datarow_SelectedEmpSalInfo("LastPaidSal").ToString()
                    '    TextBoxLastPaidSalary.Text = TextBoxLastPaidSalary.Text.Substring(0, TextBoxLastPaidSalary.Text.IndexOf(".")) & ".00"
                    'End If

                    'If Not l_datarow_SelectedEmpSalInfo("SalDate") Is DBNull.Value Then
                    '    LabelLastPaidMonthDate.Text = Convert.ToDateTime(l_datarow_SelectedEmpSalInfo("SalDate")).ToString("MM/dd/yyyy")
                    'End If
                    'Commented by Ashish 30-July-2008 YRS 5.0-445 ,End
                    If DataGridEmployment.SelectedItem.Cells(5).Text() <> "" Or DataGridEmployment.SelectedItem.Cells(5).Text() <> "&nbsp;" Then
                        TextBoxStartWorkDate.Text = Convert.ToDateTime(DataGridEmployment.SelectedItem.Cells(4).Text()).ToString("MM/dd/yyyy")
                    End If

                    If l_string_EmploymentTermDate <> "" Then 'Phase IV Changes
                        TextBoxEndWorkDate.Text = Convert.ToDateTime(l_string_EmploymentTermDate).ToString("MM/dd/yyyy")
                    End If
                End If
            End If


            ' Participant without Employment records
            If TextBoxRetirementDate.Text = String.Empty Then
                TextBoxRetirementDate.Text = Convert.ToDateTime(DateTime.Today.Month & "/01/" & DateTime.Today.Year).ToString("MM/dd/yyyy")

                'Age Calculation
                TextBoxRetirementAge.Text = Convert.ToDecimal(DateDiff(DateInterval.Day, Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime(TextBoxRetirementDate.Text)) / 365).ToString.Substring(0, 2)
                'Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                If (TextBoxRetirementAge.Text <> "") Then
                    'Added by chandrasekar.c:Static 55 change to dynamic by using property MinAgeToRetire
                    If Convert.ToInt32(TextBoxRetirementAge.Text.Trim) >= Me.MinAgeToRetire Then
                        chkRetirementAccount.Visible = True
                        txtRetirementAccount.Visible = True

                        chkSavingPartialAmount.Visible = True
                        txtSavingPartialAmount.Visible = True
                    End If
                End If

                Me.RetirementDate = TextBoxRetirementDate.Text
            End If
        Catch
            Throw
        End Try
    End Sub

    'Sanket vaidya          18 Apr 2011     BT-816 : Disability Retirement Estimate Issues.
    'This function will return 1st day of followed menth if day is greater than 1 else it will returnsame date
    'For e.f if date is 04/15/2010 thenit will retuen 05/01/2010 else if date is 04/01/2010 it will retuen 04/01/2010
    Private Function GetFirstDayOfNextMonthIfNotFirstOfMonth(ByVal endDate As DateTime) As DateTime

        If (endDate.Day = 1) Then
            Return endDate
        End If

        If (endDate.Month = 12) Then
            Return New DateTime(endDate.Year + 1, 1, 1)
        End If

        Return New DateTime(endDate.Year, endDate.Month + 1, 1)

    End Function

    'Sanket vaidya          18 Apr 2011     BT-816 : Disability Retirement Estimate Issues.
    Private Function GetLastTerminationDate() As DateTime
        Dim dsSalInfo As New DataSet
        Dim lastTermDate As String
        Dim drLastTermDate As DataRow()
        Dim TermDate As DateTime
        Dim strMaxTermDate As DateTime
        Dim drRetDetailsFoundRows As DataRow()

        strMaxTermDate = DateTime.MinValue

        If Session("dsPersonEmploymentDetails") Is Nothing Then
            Return DateTime.MaxValue
        End If

        dsSalInfo = DirectCast(Session("dsPersonEmploymentDetails"), DataSet)

        If (HelperFunctions.isEmpty(dsSalInfo)) Then
            Return DateTime.MinValue
        End If

        drRetDetailsFoundRows = dsSalInfo.Tables(0).Select("dtmTerminationDate='' OR dtmTerminationDate IS NULL")
        'If their is any Active employment axist
        If (drRetDetailsFoundRows.Length > 0) Then
            If HelperFunctions.isNonEmpty(dsSalInfo.Tables(1)) AndAlso dsSalInfo.Tables(1).Rows(0).IsNull("LastSalPaidMonth") = False Then
                TermDate = dsSalInfo.Tables(1).Rows(0).Item("LastSalPaidMonth").ToString()
                Return New DateTime(TermDate.Year, TermDate.Month, 1)
            Else
                Return DateTime.MinValue
            End If
        End If

        For Each dr As DataRow In dsSalInfo.Tables(0).Rows
            If (Convert.ToDateTime(dr.Item("dtmTerminationDate").ToString()) > strMaxTermDate) Then
                strMaxTermDate = Convert.ToDateTime(dr.Item("dtmTerminationDate").ToString())
            End If
        Next

        Return strMaxTermDate


    End Function


    Private Sub setEmploymentControlState()
        Dim l_bool_ActiveEmployment As Boolean
        Dim l_string_EmploymentTermDate As String = String.Empty

        Try
            '16-Feb-2011 Added by Sanket for disabiling controls when value in drop down is changed
            If Me.RetireType.Trim.ToUpper() = "NORMAL" Then
                If DataGridEmployment.Items.Count > 0 Then
                    'check if the employment is actually active then just hold the id of the selected employment
                    If DataGridEmployment.SelectedItem.Cells(5).Text.ToString() = "&nbsp;" Then
                        l_string_EmploymentTermDate = String.Empty
                    Else
                        l_string_EmploymentTermDate = DataGridEmployment.SelectedItem.Cells(5).Text.ToString()
                    End If
                End If

                '' If no employment history is present disable all the salary controls 
                'l_bool_ActiveEmployment = Not IsRetirementBackDated() 'check if it is a back dated retirement
                'Commented by Chandrasekar.C on 2015.12.17 : YRS-AT-2329: Annuity Estimate Calculator - Checking whether retreiment date is Current or Backed date 
                l_bool_ActiveEmployment = IsRetirementNotFutureDated()
                'Start:Added by chandrasekar.c on 2015.12.15 : YRS-AT-2329: Annuity Estimate Calculator - Clearing the calendar fields if the selected employment is not active as they will be disabled going forward in the code if the employment is not active.
                If l_bool_ActiveEmployment = False Then
                    TextBoxFutureSalaryEffDate.Text = ""
                    TextBoxAnnualSalaryIncreaseEffDate.Text = ""
                End If
                'End:Added by chandrasekar.c on 2015.12.15 : YRS-AT-2329: Annuity Estimate Calculator - Clearing the calendar fields if the selected employment is not active as they will be disabled going forward in the code if the employment is not active.

                If l_bool_ActiveEmployment = True Then 'if it is not a back dated retirement then
                    'check if no employment record exists
                    'Commented by Ashish for Phase V changes
                    'l_bool_ActiveEmployment = Not (DataGridEmployment.Items.Count < 0) 
                    'Added by Ashish for phase v changes
                    l_bool_ActiveEmployment = Not (DataGridEmployment.Items.Count = 0)
                End If
                If l_bool_ActiveEmployment = True Then
                    l_bool_ActiveEmployment = (l_string_EmploymentTermDate = "") 'check if the employment is actually active
                End If
                '16-Feb-2011 Added by Sanket for disabiling controls when value in drop down is changed
                'TextBoxModifiedSal.ReadOnly = Not l_bool_ActiveEmployment
                'Added by Ashish on 23-Jul-2009 for Issue YRS 5.0-835, enable future salary & future salary date enable for active employment with status RA,AE,NP,ML
                If Me.FundEventStatus = "PE" Or Me.FundEventStatus = "PEML" Or Me.FundEventStatus = "RE" Or Me.FundEventStatus = "PENP" Or Me.FundEventStatus = "AE" Or Me.FundEventStatus = "RA" Or Me.FundEventStatus = "ML" Or Me.FundEventStatus = "NP" Then
                    TextBoxFutureSalary.ReadOnly = Not l_bool_ActiveEmployment
                    'TextBoxFutureSalaryEffDate.rReadOnly = Not l_bool_ActiveEmployment
                    TextBoxFutureSalaryEffDate.Enabled = l_bool_ActiveEmployment

                    'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - enabling the end work date (termination date) for the selected employment if it is active otherwise disabling the field.
                    TextBoxEndWorkDate.Enabled = (l_string_EmploymentTermDate = "")
                    ButtonUpdateEmployment.Enabled = (l_string_EmploymentTermDate = "")
                    'End:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - enabling the end work date (termination date) for the selected employment if it is active otherwise disabling the field.
                Else
                    TextBoxFutureSalary.ReadOnly = True
                    'TextBoxFutureSalaryEffDate.rReadOnly = True
                    TextBoxFutureSalaryEffDate.Enabled = False
                    'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - disabling the end work date (termination date) field as the selected employment could not be an active empolyment.
                    TextBoxEndWorkDate.Enabled = False
                    ButtonUpdateEmployment.Enabled = False
                    'End:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - No access to the End Work Date field or Future Salary Effective Date field

                    'Commented by Ashish 23-Jul-2009 for Issue YRS 5.0-835,Start
                    ''Added by Ashish 20-July-2009 for Issue-830 ,start
                    'If Me.FundEventStatus = "AE" Or Me.FundEventStatus = "RA" Then
                    '    If TextBoxModifiedSal.Text = String.Empty Or Convert.ToDecimal(IIf(TextBoxModifiedSal.Text = String.Empty, 0, TextBoxModifiedSal.Text)) = 0 Then
                    '        TextBoxFutureSalary.ReadOnly = Not l_bool_ActiveEmployment
                    '        TextBoxFutureSalaryEffDate.Enabled = l_bool_ActiveEmployment
                    '    End If
                    'End If
                    ''Added by Ashish 20-July-2009 for Issue-830 ,End 
                    'Commented by Ashish 23-Jul-2009 for Issue YRS 5.0-835,End
                End If

                'TextBoxStartWorkDate.ReadOnly = Not l_bool_ActiveEmployment
                'TextBoxEndWorkDate.rReadOnly = Not l_bool_ActiveEmployment

                'Commented by CS : 12/14/2015 : YRS-AT-2329: Annuity Estimate Calculator - as the enabling/disabling activity is handled in above code.
                'TextBoxEndWorkDate.Enabled = l_bool_ActiveEmployment

                DropDownAnnualSalaryIncrease.Enabled = l_bool_ActiveEmployment

                'TextBoxAnnualSalaryIncreaseEffDate.rReadOnly = Not l_bool_ActiveEmployment
                TextBoxAnnualSalaryIncreaseEffDate.Enabled = l_bool_ActiveEmployment

                'ButtonUpdateEmployment.Enabled = l_bool_ActiveEmployment 'Commented by chandrasekar.c : 12/14/2015 : YRS-AT-2329: Annuity Estimate Calculator - as the enabling/disabling activity is handled in above code.
                '16-Feb-2011 Added by Sanket for disabiling controls when value in drop down is changed
            ElseIf Me.RetireType.Trim.ToUpper() = "DISABL" Then
                TextBoxFutureSalary.ReadOnly = True
                TextBoxFutureSalaryEffDate.Enabled = False
                TextBoxEndWorkDate.Enabled = False
                DropDownAnnualSalaryIncrease.Enabled = False
                TextBoxAnnualSalaryIncreaseEffDate.Enabled = False
                ButtonUpdateEmployment.Enabled = False
            End If
        Catch
            Throw
        End Try
    End Sub
    Private Sub showGridRowSelection()
        Dim i As Integer
        Dim l_button_Select As ImageButton
        Try
            For i = 0 To Me.DataGridEmployment.Items.Count - 1
                l_button_Select = _
                DataGridEmployment.Items(i).FindControl("ImageButtonSelect")
                If Not l_button_Select Is Nothing Then
                    If i = DataGridEmployment.SelectedIndex Then
                        l_button_Select.ImageUrl = "images\selected.gif"
                    Else
                        l_button_Select.ImageUrl = "images\select.gif"
                    End If
                End If
            Next
        Catch
            Throw
        End Try
    End Sub
    Private Sub setPlanType(ByVal planType As String)
        Try
            DropDownListPlanType.Items.Clear()
            Select Case planType
                Case "R"
                    DropDownListPlanType.Items.Add("Retirement")
                Case "S"
                    DropDownListPlanType.Items.Add("Savings")
                Case "B"
                    DropDownListPlanType.Items.Add("Both")
                    DropDownListPlanType.Items.Add("Retirement")
                    DropDownListPlanType.Items.Add("Savings")
                Case Else
            End Select

            Me.PlanType = planType
            Me.disableDeathBenefitControls(planType)
            ValidateAndShowRDBWarninMessage(planType) ' BD : 2018.11.01 : YRS-AT-4133 - No Active Death Benefit if first enrolled on or after 1/1/2019
            'Me.disableElectiveAccounts(planType)

        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub disableDeathBenefitControls(ByVal planType As String)
        Try
            Dim enableFlag As Boolean
            enableFlag = False
            '  LabelDeathBenefitRestrictionMessage.Text = ""
            ' If Selected PlanType is Savings then 
            ' Reset and Disable DeathBenefit controls
            ' And, Disable Retirement type dropdown
            'Ashish YRS 5.0-1345 :BT-859
            'BT-798 : System should not allow disability retirement for QD and BF fundevents
            'Start:Added by chandrasekar.c on 2015.11.03 for YRS-Ticket-2610 :Restriction of death benefit annuity purchase,This condition is used to disabled or enabled the Death benefit controls based person age is under MinAgeToRetire(55) as of Death Benefit annuity purchase restricted date (1/1/2019)

            If planType = "S" Or Me.OrgBenTypeIsQDROorRBEN = True Or Not YMCARET.YmcaBusinessObject.RetirementBOClass.IsFirstEnrolledBeforeCutOffDate_2019PlanChange(Me.FundEventId) Then 'BD : 2018.11.01 : YRS-AT-4133 - No Active Death Benefit if first enrolled on or after 1/1/2019
                enableFlag = False 'disabled Death benefit controls 
                TextboxRetiredDeathBenefit.Text = 0
                DropdownlistPercentageToUse.SelectedIndex = 0
                TextboxAmountToUse.Text = 0
                'Sanket : BT-926 : Showing and allowing Retired Death Benefit amount even for 'Savings' plan selected.
                Session("mnyDeathBenefitAmount") = Nothing
                'Ashish 2011.04.14 Commented for  Disability changes
                'DropDownListRetirementType.SelectedIndex = 0

                'Start Added by chandrasekar.c on 2015.11.03 : This is used to display the Death benefit annuity purchase restriction message
            ElseIf Me.IsDeathBenefitAnnuityPurchaseRestricted = True Then
                enableFlag = False 'disabled Death benefit controls 
                DropdownlistPercentageToUse.SelectedIndex = 0
                TextboxAmountToUse.Text = 0
                ' Session("mnyDeathBenefitAmount") = Nothing

                ' Dim l_string_DeathBenefitRestrictionMessage As String = getmessage("MESSAGE_RETIREMENT_ESTIMATE_DTH_BENEFIT_ANN_PURCHASE_RESTRICTION").ToString()
                'LabelDeathBenefitRestrictionMessage.Text = l_string_DeathBenefitRestrictionMessage.ToString().Replace("$$RETIRE_SERVICE_MIN_AGE$$", Me.MinAgeToRetire).Replace("$$DTH_BENEFIT_ANN_PURCHASE_RESTRICTION_DATE$$", Me.DeathBenefitAnnuityPurchaseRestrictedDate)
                'End:Added by chandrasekar.c on 2015.11.03 : This is used to display the Death benefit annuity purchase restriction message
                'End:Added by chandrasekar.c on 2015.11.03 for YRS-Ticket-2610 ,This condition is used to disabled or enabled the Death benefit controls based person age is under MinAgeToRetire(55) as of Death Benefit annuity purchase restricted date (1/1/2019)
            Else
                enableFlag = True
            End If

            'Priya 05-Jan-2010     YRS 5.0-983 commented previous code 
            'If IsRetirementBackDated() = True Then
            '    enableFlag = False
            'End If
            'End 05-Jan-2010 YRS 5.0-983
            TextboxRetiredDeathBenefit.Enabled = enableFlag
            DropdownlistPercentageToUse.Enabled = enableFlag
            TextboxAmountToUse.Enabled = enableFlag

            'Ashish 2011.04.14 Commented for enable Disability
            'DropDownListRetirementType.Enabled = enableFlag
        Catch
            Throw
        End Try
    End Sub

    Private Sub disableElectiveAccounts(ByVal planType As String)
        CheckBoxSavingsPlan.Enabled = True
        CheckBoxSavingsPlan.Visible = True
        DatagridElectiveSavingsAccounts.Enabled = True
        DatagridElectiveSavingsAccounts.Visible = True
    End Sub
#End Region

    '#Region "Code for Account Balances and Get Projections"
    'Added new parameter for getting projected balances Phase V changes
    'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
    'Private Function ValidateEstimate(ByVal para_IsCalculateProjBal As Boolean) As DataSet
    Private Function ValidateEstimate(ByVal para_IsCalculateProjBal As Boolean) As Boolean
        'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
        'Dim dsRetEstimateEmployment As New DataSet
        'Dim dtRetEstimateEmployment As DataTable
        'Dim i As Integer
        Dim l_IsRetValid As Boolean = True

        Try

            ' This check will not be applicable if the person is already retired 
            ' ie if the Fund Status is RT or RD
            'Start - Phase IV Changes - Allowing back dated annuity estimate
            'If Convert.ToDateTime(TextBoxRetirementDate.Text) < DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("MM/dd/yyyy") Then
            '    Call ShowCustomMessage("Retirement date should be greater than Todays date.", enumMessageBoxType.DotNet)
            '    Exit Function
            'End If
            'End - Phase IV Changes - Allowing back dated annuity estimate

            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624 Start
            'Dim terminationDateInDB As String = String.Empty

            'dsRetEstimateEmployment = RetirementBOClass.getEmploymentDetails(Me.FundEventId, _
            '"NORMAL", TextBoxRetirementDate.Text)

            'If Not dsRetEstimateEmployment.Tables.Count = 0 Then
            '    If Not dsRetEstimateEmployment.Tables(0).Rows.Count = 0 Then
            '        dtRetEstimateEmployment = dsRetEstimateEmployment.Tables(0)
            '        For i = 0 To dtRetEstimateEmployment.Rows.Count - 1
            '            If Not dtRetEstimateEmployment.Rows(i).Item("dtmTerminationDate") Is DBNull.Value Then
            '                terminationDateInDB = dtRetEstimateEmployment.Rows(i).Item("dtmTerminationDate").ToString
            '            End If
            '        Next
            '    End If
            'End If
            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624 End
            '''''Let's Check the Participant Eligibility
            'ASHISH:YRS-1345/BT-859 commented suppress validation code 
            'If Not Convert.ToDecimal(TextBoxRetirementAge.Text) >= 60 Or Me.RetireType.ToUpper() = "DISABL" Then
            Dim errorMessage As String
            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
            'Dim terminationDate As String
            'terminationDate = terminationDateInDB

            ' First update the correct retirement type.
            SetRetireType()
            'Added by Ashish for phase V changes ,this will not execute when calculate projected balances 
            'Call the BO retirement validate method
            If para_IsCalculateProjBal = False Then
                'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                'If Not RetirementBOClass.IsRetirementValid(errorMessage, True, Me.FundEventId, TextBoxRetirementDate.Text, Me.RetireType, Me.PersonId, Me.SSNO, terminationDate, Me.FundEventStatus, Me.PlanType) Then
                If Not RetirementBOClass.IsRetirementValid(errorMessage, True, Me.FundEventId, TextBoxRetirementDate.Text, Me.RetireType, Me.PersonId, Me.SSNO, Me.FundEventStatus, Me.PlanType, Me.PersonalWithdrawalExists, Me.HasSatisfiedPaidService) Then ' SR | 2017.04.07 | YRS-AT-3390 | Pass HasSatisfiedPaidService parameter to get Paid service to validate insufficient Paid service.
                    errorMessage = combinemessage(errorMessage)
                    Call ShowCustomMessage(errorMessage, enumMessageBoxType.DotNet)

                    'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                    'Exit Function
                    'ASHISH:2010.10.11 code Added for YRS 5.0-855 BT-624
                    l_IsRetValid = False
                End If
            End If
            'End If

            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
            'ValidateEstimate = dsRetEstimateEmployment
            'ASHISH:2010.10.11 code Added for YRS 5.0-855 BT-624
            Return l_IsRetValid
        Catch
            Throw
        End Try
    End Function
    'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
    'Private Function EstimateAnnuities(ByVal dsRetEstimateEmployment As DataSet) As DataSet
    Private Function EstimateAnnuities() As DataSet
        Dim dsRetEstimateEmployment As DataSet
        Dim logDetails As StringBuilder 'PPP | 03/14/2017 | YRS-AT-2625 | Preparing logDetails, which will be used to log data into AtsYRSActivityLog table
        Try
            ' Get the correct elective account details as per the Plan type selected by the user.
            Dim dataSetElectiveAccounts As DataSet
            Dim i As Integer
            dataSetElectiveAccounts = getElectiveAccounts()
            'ASHISH:2010.10.11 code Added for YRS 5.0-855 BT-624 Start
            If Not Session("dsPersonEmploymentDetails") Is Nothing Then
                dsRetEstimateEmployment = DirectCast(Session("dsPersonEmploymentDetails"), DataSet)
            End If
            'UpdateEmployeeInformation()
            'ASHISH:2010.10.11 code Added for YRS 5.0-855 BT-624 End
            'Commented by Ashish for phase V changes ,Start
            ' If Future salary is entered and Effective date is not then set it to Today's date
            'If TextBoxFutureSalary.Text <> String.Empty And TextBoxFutureSalary.Text = String.Empty Then
            '    TextBoxFutureSalary.Text = DateTime.Today.ToShortDateString()
            'End If
            'Commented by Ashish for phase V changes ,Start
            ''Added by Ashish for phase V changes ,Start
            '' If Future salary is entered and Effective date is not then set it to Today's date
            'If TextBoxFutureSalary.Text <> String.Empty And TextBoxFutureSalaryEffDate.Text = String.Empty Then
            '    TextBoxFutureSalaryEffDate.Text = DateTime.Today.AddDays(1).ToShortDateString()
            'End If
            ''Added by Ashish for phase V changes ,End

            'Added by Ashish 31-July-2008 YRS 5.0-445 ,Start
            If TextBoxModifiedSal.Text = String.Empty Then
                TextBoxModifiedSal.Text = "0.00"
            End If
            'Added by Ashish 31-July-2008 YRS 5.0-445 ,End

            'Step 1. Calculate the Account Balances
            Dim hasNoErrors As Boolean = True
            Dim combinedDataset = New DataSet
            Dim errorMessage As String
            Dim warningMessage As String = String.Empty
            Dim isYmcaLegacyAcctTotalExceed As Boolean = False
            Dim isYmcaAcctTotalExceed As Boolean = False
            Dim l_strMaxEmpTerminationDate As String
            Dim bIsRetirementPartial As Boolean = False
            Dim bIsSavingPartial As Boolean = False
            Dim decRetirementPartialAmt As Decimal = 0
            Dim decSavingPartialAmt As Decimal = 0
            Dim decPIA_At_Termination As Decimal = 0 ''Start:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - To Set the YMCA Legacy Amount as on Termination
            businessLogic = Session("businessLogic")
            Dim excludedDataTable As DataTable = getExcludedAccounts()
            Dim employmentDetails As DataTable = Session("employmentSalaryInformation")
            l_strMaxEmpTerminationDate = GetTerminationDateForProjBalValidation(TextBoxRetirementDate.Text.Trim(), employmentDetails, dsRetEstimateEmployment.Tables(0))
            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
            'hasNoErrors = businessLogic.CalculateAccountBalancesAndProjections(dsRetEstimateEmployment _
            ', TextBoxRetireeBirthday.Text, TextBoxFutureSalary.Text, TextBoxRetirementDate.Text _
            ', TextBoxFutureSalaryEffDate.Text, TextBoxModifiedSal.Text, TextBoxEndWorkDate.Text _
            ', Me.PersonId, Me.FundEventId, Me.RetireType _
            ', Convert.ToDouble(ListBoxProjectedYearInterest.SelectedValue), dataSetElectiveAccounts _
            ', DropDownAnnualSalaryIncrease.SelectedValue, TextBoxAnnualSalaryIncreaseEffDate.Text, combinedDataset, True, Me.PlanType _
            ', Me.FundEventStatus, errorMessage, warningMessage, excludedDataTable, employmentDetails, False _
            ', l_strMaxEmpTerminationDate, isYmcaLegacyAcctTotalExceed, isYmcaAcctTotalExceed)

            'START: PPP | 03/20/2017 | YRS-AT-2625 | In disablility case we have to send "NORMAL" average salary, So that from current date to retirement date estimation will work correcty based on "NORMAL"
            If Me.RetireType = "DISABL" Then
                dsRetEstimateEmployment = RetirementBOClass.SearchRetEmpInfo(Me.FundEventId, "NORMAL", IIf(String.IsNullOrEmpty(TextBoxRetirementDate.Text), "01/01/1900", TextBoxRetirementDate.Text))
                employmentDetails = saveActiveSalaryInformation(dsRetEstimateEmployment)
            End If
            'END: PPP | 03/20/2017 | YRS-AT-2625 | In disablility case we have to send "NORMAL" average salary, So that from current date to retirement date estimation will work correcty based on "NORMAL"

            Dim transactionDetails As DataSet 'MMR | 2017.03.03 | YRS-AT-2625 |Declared object variable

            'START: MMR | 2017.03.03 | YRS-AT-2625 | Adding manual transaction details to existing dataset for inclusion in computing average salary           
            If HelperFunctions.isNonEmpty(dsRetEstimateEmployment) Then
                If dsRetEstimateEmployment.Tables.Contains("ManualTransactionDetails") Then
                    dsRetEstimateEmployment.Tables.Remove("ManualTransactionDetails")
                End If
                If Not Me.ManualTransactionDetails Is Nothing Then
                    transactionDetails = Me.ManualTransactionDetails
                    dsRetEstimateEmployment.Tables.Add(transactionDetails.Tables("ManualTransactionDetails").Copy())
                End If
            End If
            'END: MMR | 2017.03.03 | YRS-AT-2625 | Adding manual transaction details to existing dataset for inclusion in computing average salary

            'START: PPP | 11/16/2017 | YRS-AT-3328 | Passing Me.IsPersonTerminated as True because Me.IsPersonTerminated value is used to fetch withdrawal configuration only
            ' So that terminated participants withrawal rules will get applied to Active participants also.
            ' It is passed to "YMCARET.YmcaBusinessObject.RefundRequest.GetRefundRequestConfiguration" inside in RetirementBO class.
            ' OLD
            'hasNoErrors = businessLogic.CalculateAccountBalancesAndProjections(dsRetEstimateEmployment _
            ', TextBoxRetireeBirthday.Text _
            ', TextBoxRetirementDate.Text _
            ', Me.PersonId, Me.FundEventId, Me.RetireType _
            ', Convert.ToDouble(ListBoxProjectedYearInterest.SelectedValue), dataSetElectiveAccounts _
            ', combinedDataset, True, Me.PlanType _
            ', Me.FundEventStatus, errorMessage, warningMessage, excludedDataTable, employmentDetails, False _
            ', l_strMaxEmpTerminationDate, isYmcaLegacyAcctTotalExceed, isYmcaAcctTotalExceed, Convert.ToInt32(TextBoxRetirementAge.Text.Trim), Me.IsPersonTerminated)
            'NEW
            hasNoErrors = businessLogic.CalculateAccountBalancesAndProjections(dsRetEstimateEmployment _
            , TextBoxRetireeBirthday.Text _
            , TextBoxRetirementDate.Text _
            , Me.PersonId, Me.FundEventId, Me.RetireType _
            , Convert.ToDouble(ListBoxProjectedYearInterest.SelectedValue), dataSetElectiveAccounts _
            , combinedDataset, True, Me.PlanType _
            , Me.FundEventStatus, errorMessage, warningMessage, excludedDataTable, employmentDetails, False _
            , l_strMaxEmpTerminationDate, isYmcaLegacyAcctTotalExceed, isYmcaAcctTotalExceed, Convert.ToInt32(TextBoxRetirementAge.Text.Trim), True)
            'END: PPP | 11/16/2017 | YRS-AT-3328 | Passing Me.IsPersonTerminated as True because Me.IsPersonTerminated value is used to fetch withdrawal configuration only

            'Check if any error has been reported.  
            If Not hasNoErrors And (errorMessage <> "R" And errorMessage <> "S") Then
                ShowCustomMessage(combinemessage(errorMessage), enumMessageBoxType.DotNet)
                Exit Function
            End If
            '2013.07.13 SP : BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page 
            TextBoxProjFinalYrsSalary.Text = businessLogic.ProjFinalYearSal.ToString("f2") * 12

            'START: PPP | 03/14/2017 | YRS-AT-2625 | Preparing logDetails, which will be used to log data into AtsYRSActivityLog table
            logDetails = New StringBuilder
            logDetails.AppendLine(String.Format("RetireType:{0},", Me.RetireType))
            logDetails.AppendLine(String.Format("DOB:{0},", TextBoxRetireeBirthday.Text))
            logDetails.AppendLine(String.Format("Retirement Date:{0},", TextBoxRetirementDate.Text))
            logDetails.AppendLine(String.Format("AverageSalary:{0},", businessLogic.AverageSalary.ToString("f2")))
            If HelperFunctions.isNonEmpty(businessLogic.YmcaResolution) Then
                logDetails.AppendLine(String.Format("YmcaResolution:{0},", businessLogic.YmcaResolution.GetXml()))
            End If
            If Not Me.ManualTransactionDetails Is Nothing Then
                logDetails.AppendLine(String.Format("MAPR:{0},", Me.ManualTransactionDetails.GetXml()))
            End If
            logDetails.AppendLine(String.Format("ActualBalanceAtRetirement:{0},", HelperFunctions.ConvertDataTableToXML(businessLogic.ActualBalanceAtRetirement)))
            logDetails.AppendLine(String.Format("TotalBalanceAtRetirement:{0},", HelperFunctions.ConvertDataTableToXML(businessLogic.TotalBalanceAtRetirement)))
            'END: PPP | 03/14/2017 | YRS-AT-2625 | Preparing logDetails, which will be used to log data into AtsYRSActivityLog table

            'START: PPP | 03/17/2017 | YRS-AT-2625 | Preparing C Annuity Guranteed Reserves for Disability Retirement
            If Me.RetireType.ToUpper() = "DISABL" Then
                TextBoxModifiedSal.Text = businessLogic.AverageSalary.ToString("f2") ' PPP | 05/26/2017 | YRS-AT-2625 | Displaying average salary used for calculation

                Dim actualBalanceOfRetirementPlan As Decimal = 0
                Dim actualBalanceOfSavingsPlan As Decimal = 0
                If Me.PlanType.ToUpper() = "R" Or Me.PlanType.ToUpper() = "B" Then
                    If (businessLogic.ActualBalanceAtRetirement.Select("chvPlanType='RETIREMENT'").Length > 0) Then
                        actualBalanceOfRetirementPlan = Convert.ToDecimal(businessLogic.ActualBalanceAtRetirement.Compute("SUM(mnyPersonalPreTax) + SUM(mnyPersonalPostTax) + SUM(mnyYmcaPreTax)", "chvPlanType='RETIREMENT'"))
                    End If
                End If
                If Me.PlanType.ToUpper() = "S" Or Me.PlanType.ToUpper() = "B" Then
                    If (businessLogic.ActualBalanceAtRetirement.Select("chvPlanType='SAVINGS'").Length > 0) Then
                        actualBalanceOfSavingsPlan = Convert.ToDecimal(businessLogic.ActualBalanceAtRetirement.Compute("SUM(mnyPersonalPreTax) + SUM(mnyPersonalPostTax) + SUM(mnyYmcaPreTax)", "chvPlanType='SAVINGS'"))
                    End If
                End If
                CAnnuityGuranteedReservesForDisability = Math.Round(actualBalanceOfRetirementPlan + actualBalanceOfSavingsPlan, 2)
            End If
            'END: PPP | 03/17/2017 | YRS-AT-2625 | Preparing C Annuity Guranteed Reserves for Disability Retirement

            'Added by Ashish for phase V changes ,start
            'set Account exceed flag into session variable
            Session("isYmcaLegacyAcctTotalExceed") = isYmcaLegacyAcctTotalExceed
            Session("isYmcaAcctTotalExceed") = isYmcaAcctTotalExceed
            'Added by Ashish for phase V changes ,End
            'warningMessage = "Warnings"
            If warningMessage <> String.Empty Then
                LabelWarningMessage.Visible = True
                LabelWarningMessage.Text = combinemessage(warningMessage) 'PPP | 03/21/2017 | YRS-AT-2625 | If warningMessage has value then only call combinemessage method
            Else
                LabelWarningMessage.Visible = False
                LabelWarningMessage.Text = String.Empty 'PPP | 03/21/2017 | YRS-AT-2625 | If warningMessage is empty then assign LabelWarningMessage.Text = String.Empty
            End If

            'LabelWarningMessage.Text = combinemessage(warningMessage) 'PPP | 03/21/2017 | YRS-AT-2625 | It is moved above where warningMessage is checked against String.Empty

            ' Check if any of the plan is not having sufficient balance.
            Session("ProceedWithEstimation") = False
            If errorMessage = "R" Or errorMessage = "S" Then
                If errorMessage = "R" Then
                    Session("UsePlan") = "S"
                    'commented by Anudeep on 22-sep for BT-1126
                    'errorMessage = "There isn't enough funds in Retirement Plan. Would you like to use Savings Plan instead?"//commented by Anudeep on 22-sep for BT-1126
                    errorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_NOT_ENOUGH_FUNDS_RETIREMENT")
                Else
                    Session("UsePlan") = "R"
                    'commented by Anudeep on 22-sep for BT-1126
                    'errorMessage = "There isn't enough funds in Savings Plan. Would you like to use Retirement Plan instead?"//commented by Anudeep on 22-sep for BT-1126
                    errorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_NOT_ENOUGH_FUNDS_SAVINGS")
                End If

                Session("ProceedWithEstimation") = True

                ShowCustomMessage(errorMessage, enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)

                Exit Function

            End If

            If chkRetirementAccount.Checked Then
                bIsRetirementPartial = True
                'Start:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - Checking person is terminate or not and as well as if Person is terminated and then get the YMCA(LEGACY) ACCOUNT Balance as on Termination
                If Me.IsPersonTerminated = True Then
                    decPIA_At_Termination = Me.PIA_At_Termination
                End If
                'End :Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - Checking person is terminate or not and as well as if Person is terminated and then get the YMCA(LEGACY) ACCOUNT Balance as on Termination
            End If

            If chkSavingPartialAmount.Checked Then
                bIsSavingPartial = True
            End If

            If txtRetirementAccount.Text.Trim() = String.Empty Then
                decRetirementPartialAmt = "0"
            Else
                decRetirementPartialAmt = txtRetirementAccount.Text.Trim()
            End If


            If txtSavingPartialAmount.Text.Trim() = String.Empty Then
                decSavingPartialAmt = "0"
            Else
                decSavingPartialAmt = txtSavingPartialAmount.Text.Trim()
            End If

            'ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
            'Compute per basistype Retirement plan balance assign into businessLogic.g_dtAcctBalancesByBasisType

            'Commented by chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - To Added two agrument(decPIA_At_Termination,l_bool_IsPersonTerminated)for Seting the YMCA(LEGACY) ACCOUNT Balance as on Termination  
            'businessLogic.calculateFinalAmounts(combinedDataset.Tables(0), "R", Convert.ToInt32(TextBoxRetirementAge.Text.Trim), bIsRetirementPartial, decRetirementPartialAmt)

            'Start:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - Added two agrument(decPIA_At_Termination,l_bool_IsPersonTerminated) for Seting the YMCA(LEGACY) ACCOUNT Balance as on Termination  
            businessLogic.calculateFinalAmounts(combinedDataset.Tables(0), "R", Convert.ToInt32(TextBoxRetirementAge.Text.Trim), bIsRetirementPartial, decRetirementPartialAmt, decPIA_At_Termination, Me.IsPersonTerminated, Me.FundEventStatus) 'Added by : Chandrasekar - 2016.06.22 - YRS-AT-3010 - Added one new Optional parameter for sending fund event status
            'End:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - Added one agrument(decPIA_At_Termination) for Seting the YMCA(LEGACY) ACCOUNT Balance as on Termination  

            'Added by Ashish for phase V part III changes
            'Find Max Annuitized factor and update g_dtAcctBalancesByBasis
            businessLogic.UpdateBasisTypeAsPerAnnuitizeFactor(Me.RetireType, TextBoxRetirementDate.Text)
            ''Step 2. Calculate Death Benefits
            'ASHISH:2011.08.29 Resolve YRS 5.0-1345 :BT-859
            'Sanket : BT-926 : Showing and allowing Retired Death Benefit amount even for 'Savings' plan selected.
            If (Me.PlanType = "B" Or Me.PlanType = "R" Or Me.PlanType = String.Empty) And Me.OrgBenTypeIsQDROorRBEN = False Then
                'START : BD : 2018.11.01 : YRS-AT-4133 - No Active Death Benefit if first enrolled on or after 1/1/2019
                Dim stRDBwarningMessage As String = String.Empty
                tnFullBalance = businessLogic.GetRetiredDeathBenefit(RetireType, Convert.ToDateTime(TextBoxRetirementDate.Text), Convert.ToDateTime(TextBoxRetireeBirthday.Text), Me.FundEventId, stRDBwarningMessage)  'Ranu YRS-AT-4133
                'END : BD : 2018.11.01 : YRS-AT-4133 - No Active Death Benefit if first enrolled on or after 1/1/2019
                Session("mnyDeathBenefitAmount") = tnFullBalance
                TextboxRetiredDeathBenefit.Text = Math.Round(tnFullBalance, 2)
                TextboxAmountToUse.Text = Math.Round(Convert.ToDecimal((TextboxRetiredDeathBenefit.Text) * (DropdownlistPercentageToUse.SelectedValue) / 100), 2)
                tnFullBalance = Convert.ToDecimal(TextboxAmountToUse.Text)

                logDetails.AppendLine(String.Format("TotalDeathBenefitAmount:{0},", TextboxRetiredDeathBenefit.Text)) 'PPP | 03/14/2017 | YRS-AT-2625 | Data will be used to log details in AtsYRSActivityLog from UI
            End If

            'Added by Ashish for phase  v changes, poulate Death benefit used in summary Tab
            If tnFullBalance > 0 Then
                txtDeathBenefitUsed.Text = TextboxAmountToUse.Text
            Else
                txtDeathBenefitUsed.Text = "0.00"
            End If


            'Step 3. Calculate the Annuities
            Dim dtAnnuitiesList As DataTable
            'ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
            Dim dtAnnuitiesList_Ret As DataTable
            Dim dtAnnuitiesList_Sav As DataTable
            Dim dtAnnuitiesList_final As DataTable

            'Dim dsRetEstInfo As DataSet 'Phase IV Changes
            Dim dsParticipantBeneficiaries As DataSet

            'dsRetEstInfo = Session("dsRetEstInfo") ' Beneficiary details from the database 'Phase IV Changes
            'SP:  BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary - Start
            'dsParticipantBeneficiaries = Session("dsParticipantBeneficiaries") ''commented sp: 1507
            dsParticipantBeneficiaries = Session("dsSelectedParticipantBeneficiary")
            'SP:  BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary - End

            Dim dtAnnuitiesFullBalanceParam As DataTable
            Dim dtAnnuitiesParam As DataTable
            Dim benDOB As DateTime
            Dim finalAnnuity As Decimal
            If TextboxBeneficiaryBirthDate.Text <> String.Empty Then
                benDOB = Convert.ToDateTime(TextboxBeneficiaryBirthDate.Text)
                logDetails.AppendLine(String.Format("BeneficiaryDOB:{0},", benDOB.ToString("MM/dd/yyyy"))) 'PPP | 03/14/2017 | YRS-AT-2625 | Preparing logDetails, which will be used to log data into AtsYRSActivityLog table
            Else
                benDOB = New DateTime(1900, 1, 1)
            End If

            If TextboxFromBenefitValue.Text.Trim() = String.Empty Then
                TextboxFromBenefitValue.Text = "0"
            End If
            'Calculate Annuity with old logic

            'Commented by Ashish 30-July-2008 YRS 5.0-445 ,Start
            'dtAnnuitiesList = businessLogic.CalculateAnnuities(0, RetireType _
            ', benDOB, Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime(TextBoxRetirementDate.Text) _
            ', dsParticipantBeneficiaries, Convert.ToDecimal(TextBoxSalaryAverage.Text), 0.0 _
            ', Convert.ToDecimal(TextboxAmountToUse.Text), Me.SSNO, Me.PersonId _
            ', dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)
            'Commented by Ashish 30-July-2008 YRS 5.0-445 ,End

            'Added by Ashish 30-July-2008 YRS 5.0-445 ,Start
            Dim isFullBalancePassed As Boolean = False
            If tnFullBalance > 0 Then
                isFullBalancePassed = True
            End If

            'YRS 5.0-657 : Estimate with SSL amounts
            CheckSSLAvailable()


            If Convert.ToDateTime(TextBoxRetirementDate.Text) < Convert.ToDateTime(Me.ExactAgeEffDate) And Me.RetireType.ToUpper() <> "DISABL" Then
                'Calculate annuity for Retirement plan
                If Me.PlanType.ToUpper() = "R" Or Me.PlanType.ToUpper() = "B" Then


                    'If retirement date is less than Ecact age effective date then use old logic
                    dtAnnuitiesList_Ret = businessLogic.CalculateAnnuities(0, RetireType _
                    , benDOB, Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime(TextBoxRetirementDate.Text) _
                    , dsParticipantBeneficiaries, 0.0, 0.0 _
                    , Convert.ToDecimal(TextboxAmountToUse.Text), Me.SSNO, Me.PersonId _
                    , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)
                    'Added by Ashish 30-July-2008 YRS 5.0-445 ,End
                    'Step 3.1 Get the Combo table
                    'Dim comboTable As DataTable
                    'Calculate annuity for DeathBenefit amount used
                    If (isFullBalancePassed) Then
                        Dim dtAnnuitiesListWithDeathBenefit As DataTable
                        dtAnnuitiesListWithDeathBenefit = businessLogic.CalculateAnnuities(tnFullBalance, RetireType _
                                        , benDOB, Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime(TextBoxRetirementDate.Text) _
                                        , dsParticipantBeneficiaries, 0.0, 0.0 _
                                        , Convert.ToDecimal(TextboxAmountToUse.Text), Me.SSNO, Me.PersonId _
                                        , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)
                        'Added by Ashish 30-July-2008 YRS 5.0-445 ,End
                        'ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
                        'comboTable = RetirementBOClass.CreateComboTable(dtAnnuitiesList, dtAnnuitiesListWithDeathBenefit) 'isFullBalancePassed)
                        'Session("lcAnnuitiesListCursor") = comboTable
                        dtAnnuitiesList_Ret = RetirementBOClass.CreateComboTable(dtAnnuitiesList_Ret, dtAnnuitiesListWithDeathBenefit) 'isFullBalancePassed)
                        'Else
                        '    Session("lcAnnuitiesListCursor") = dtAnnuitiesList
                    End If
                End If
                If Me.PlanType.ToUpper() = "S" Or Me.PlanType.ToUpper() = "B" Then
                    'ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
                    'Compute per basistype Saving plan balance and assign into  businessLogic.g_dtAcctBalancesByBasisType
                    businessLogic.g_dtAcctBalancesByBasisType = Nothing
                    businessLogic.calculateFinalAmounts(combinedDataset.Tables(0), "S", Convert.ToInt32(TextBoxRetirementAge.Text.Trim), bIsSavingPartial, decSavingPartialAmt)
                    'For saving plan no need to update PRE and PST group basis type with effective basistype of PST group,
                    'that is the reson even if retirement type is Disability pass retirement type is NORMAL 
                    'Find Max Annuitized factor and update g_dtAcctBalancesByBasis
                    businessLogic.UpdateBasisTypeAsPerAnnuitizeFactor("NORMAL", TextBoxRetirementDate.Text)
                    'Calculate Annuity For Saving Plan
                    dtAnnuitiesList_Sav = businessLogic.CalculateAnnuities(0, RetireType _
                   , benDOB, Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime(TextBoxRetirementDate.Text) _
                   , dsParticipantBeneficiaries, 0.0, 0.0 _
                   , Convert.ToDecimal(TextboxAmountToUse.Text), Me.SSNO, Me.PersonId _
                   , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)
                End If
                ''If Retirement date > Exact age effective date then use Exact age annuity logic
            ElseIf Convert.ToDateTime(TextBoxRetirementDate.Text) > Convert.ToDateTime(Me.ExactAgeEffDate) Or Me.RetireType.ToUpper() = "DISABL" Then
                'Calculate annuity for Retirement plan with Exact age Fator
                If Me.PlanType.ToUpper() = "R" Or Me.PlanType.ToUpper() = "B" Then
                    'Set businessLogic.g_dtAcctBalancesByBasisType only with Retirement plan balances
                    'ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
                    'businessLogic.g_dtAcctBalancesByBasisType = Nothing
                    'businessLogic.calculateFinalAmounts(combinedDataset.Tables(0), Nothing, 1, 1, "R")
                    If Me.RetireType.ToUpper() = "DISABL" Then
                        dtAnnuitiesList_Ret = businessLogic.CalculateAnnuitiesWithExactAgeForDisability(0, RetireType _
                    , benDOB, Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime(TextBoxRetirementDate.Text) _
                    , dsParticipantBeneficiaries _
                    , Convert.ToDecimal(TextboxAmountToUse.Text), Me.SSNO, Me.PersonId _
                    , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)
                    Else
                        'For Normal Retirement
                        dtAnnuitiesList_Ret = businessLogic.CalculateAnnuitiesWithExactAge(0, RetireType _
                    , benDOB, Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime(TextBoxRetirementDate.Text) _
                    , dsParticipantBeneficiaries _
                    , Convert.ToDecimal(TextboxAmountToUse.Text), Me.SSNO, Me.PersonId _
                    , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)
                    End If



                    'Added by Ashish 30-July-2008 YRS 5.0-445 ,End
                    'Step 3.1 Get the Combo table
                    'Dim comboTable As DataTable

                    If (isFullBalancePassed) Then
                        Dim dtAnnuitiesListWithDeathBenefit As DataTable
                        'Added by Ashish 30-July-2008 YRS 5.0-445 ,Start
                        dtAnnuitiesListWithDeathBenefit = businessLogic.CalculateAnnuitiesWithExactAge(tnFullBalance, RetireType _
                                        , benDOB, Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime(TextBoxRetirementDate.Text) _
                                        , dsParticipantBeneficiaries _
                                        , Convert.ToDecimal(TextboxAmountToUse.Text), Me.SSNO, Me.PersonId _
                                        , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)
                        'Added by Ashish 30-July-2008 YRS 5.0-445 ,End
                        'ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
                        '    comboTable = RetirementBOClass.CreateComboTable(dtAnnuitiesList, dtAnnuitiesListWithDeathBenefit) 'isFullBalancePassed)
                        '    Session("lcAnnuitiesListCursor") = comboTable
                        'Else
                        '    Session("lcAnnuitiesListCursor") = dtAnnuitiesList
                        'ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
                        dtAnnuitiesList_Ret = RetirementBOClass.CreateComboTable(dtAnnuitiesList_Ret, dtAnnuitiesListWithDeathBenefit) 'isFullBalancePassed)
                    End If

                End If

                If Me.PlanType.ToUpper() = "S" Or Me.PlanType.ToUpper() = "B" Then
                    'Set businessLogic.g_dtAcctBalancesByBasisType only with Retirement plan balances
                    'ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
                    businessLogic.g_dtAcctBalancesByBasisType = Nothing
                    businessLogic.calculateFinalAmounts(combinedDataset.Tables(0), "S", Convert.ToInt32(TextBoxRetirementAge.Text.Trim), bIsSavingPartial, decSavingPartialAmt)
                    'For saving plan no need to update PRE and PST group basis type with effective basistype of PST group,
                    'that is the reson even if retirement type is Disability pass retirement type is NORMAL 
                    'Find Max Annuitized factor and update g_dtAcctBalancesByBasis
                    'START: PPP | 05/02/2017 | YRS-AT-2625 | Reverting changes done for 20.0.3 and passing "Normal" as retirement type as we have to preserve the annuity basis types
                    ''START: PPP | 03/15/2017 | YRS-AT-2625 | Preparing annuity basis type change based on retirement type
                    ''businessLogic.UpdateBasisTypeAsPerAnnuitizeFactor("NORMAL", TextBoxRetirementDate.Text)
                    'businessLogic.UpdateBasisTypeAsPerAnnuitizeFactor(RetireType, TextBoxRetirementDate.Text)
                    ''END: PPP | 03/15/2017 | YRS-AT-2625 | Preparing annuity basis type change based on retirement type
                    businessLogic.UpdateBasisTypeAsPerAnnuitizeFactor("NORMAL", TextBoxRetirementDate.Text)
                    'END: PPP | 05/02/2017 | YRS-AT-2625 | Reverting changes done for 20.0.3 and passing "Normal" as retirement type as we have to preserve the annuity basis types
                    'If retirement date is greater than Ecact age effective date then use exact age logic
                    dtAnnuitiesList_Sav = businessLogic.CalculateAnnuitiesWithExactAge(0, RetireType _
                    , benDOB, Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime(TextBoxRetirementDate.Text) _
                    , dsParticipantBeneficiaries _
                    , Convert.ToDecimal(TextboxAmountToUse.Text), Me.SSNO, Me.PersonId _
                    , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)
                End If
            End If

            'START: PPP | 03/14/2017 | YRS-AT-2625 | Data will be used to log details in AtsYRSActivityLog from UI
            If (isFullBalancePassed) Then
                logDetails.AppendLine(String.Format("UsedDeathBenefitAmount:{0},", tnFullBalance.ToString("f2")))
            End If
            'END: PPP | 03/14/2017 | YRS-AT-2625 | Data will be used to log details in AtsYRSActivityLog from UI

            'START: PPP | 03/16/2017 | YRS-AT-2625 | Calculating C reduction amount
            If Me.RetireType.ToUpper() = "DISABL" AndAlso HelperFunctions.isNonEmpty(dtAnnuitiesList_Ret) Then
                Dim reserveReductionPercent As Decimal = 1
                Dim rows As DataRow() = dtAnnuitiesList_Ret.Select("chrAnnuityType='C'")
                If Not rows Is Nothing AndAlso rows.Length > 0 Then
                    'START: PPP | 04/18/2017 | YRS-AT-3390 | IIf function evaluates all the conditions written in it, so there is no use of checking for DBNULL
                    'reserveReductionPercent = IIf(Convert.IsDBNull(rows(0)("mnyReserveReductionPercent")), 1, Convert.ToDecimal(rows(0)("mnyReserveReductionPercent")))
                    If Not Convert.IsDBNull(rows(0)("mnyReserveReductionPercent")) Then
                        reserveReductionPercent = Convert.ToDecimal(rows(0)("mnyReserveReductionPercent"))
                    End If
                    'END: PPP | 04/18/2017 | YRS-AT-3390 | IIf function evaluates all the conditions written in it, so there is no use of checking for DBNULL
                    rows(0)("AnnuityWithoutRDB") = Math.Round((Convert.ToDecimal(rows(0)("AnnuityWithoutRDB")) * reserveReductionPercent), 2)
                End If
            End If
            'END: PPP | 03/16/2017 | YRS-AT-2625 | Calculating C reduction amount

            'Combind both plan annuity
            If Me.PlanType.ToUpper() = "R" Then
                Session("lcAnnuitiesListCursor") = dtAnnuitiesList_Ret
            ElseIf Me.PlanType.ToUpper() = "S" Then
                Session("lcAnnuitiesListCursor") = dtAnnuitiesList_Sav
            Else
                dtAnnuitiesList_final = RetirementBOClass.CombinedRetAndSavAnnuityListTable(dtAnnuitiesList_Ret, dtAnnuitiesList_Sav)
                Session("lcAnnuitiesListCursor") = dtAnnuitiesList_final
            End If



            'Step 4. Calculate the Payments
            CalculatePayments()
            'Added By Ashish for V changes
            DisplayProjectedReserves()
            'YRS 5.0-657 : Estimate with SSL amounts
            If Not Session("lcAnnuitiesListCursor") Is Nothing Then
                combinedDataset.Tables.Add(Session("lcAnnuitiesListCursor"))
                EstimateAnnuities = combinedDataset

                'START: PPP | 03/14/2017 | YRS-AT-2625 | Preparing logDetails, which will be used to log data into AtsYRSActivityLog table
                logDetails.AppendLine(String.Format("DisplayedAnnuity:{0},", HelperFunctions.ConvertDataTableToXML(TryCast(Session("lcAnnuitiesListCursor"), DataTable))))
                'END: PPP | 03/14/2017 | YRS-AT-2625 | Preparing logDetails, which will be used to log data into AtsYRSActivityLog table
            Else
                EstimateAnnuities = Nothing
            End If

            'START: PPP | 03/14/2017 | YRS-AT-2625 | Logging details in AtsYRSActivityLog table
            Call (New Retirement()).LogDetails("ESTIMATE_ANNUITIES", Me.FundNo, YMCAObjects.EntityTypes.PERSON, YMCAObjects.Module.Retirement, logDetails.ToString())
            'END: PPP | 03/14/2017 | YRS-AT-2625 | Logging details in AtsYRSActivityLog table
        Catch
            Throw
        End Try
    End Function
    'YRS 5.0-657 : Estimate with SSL amounts
    Private Function CheckSSLAvailable()
        If Me.RetireType = "DISABL" Then
            TextboxFromBenefitValue.Text = "0.00"
            TextboxSSIncrease.Text = "0.00"
            TextboxFromBenefitValue.Enabled = False
            LabelSSLWarningMessage.Visible = False
        ElseIf Me.RetireType = "NORMAL" Then
            If ssAnnuityAlreadyBought = True Then
                TextboxFromBenefitValue.Enabled = False
            Else
                TextboxFromBenefitValue.Enabled = True
            End If
        End If
    End Function
    Private Sub CalculatePayments()
        Try
            Dim calculateDeathBenefitAnnuity As Boolean
            If Convert.ToDecimal(TextboxAmountToUse.Text) = 0 Then
                calculateDeathBenefitAnnuity = False
            Else
                calculateDeathBenefitAnnuity = True
            End If

            Dim benDOB As DateTime
            If TextboxBeneficiaryBirthDate.Text <> String.Empty Then
                benDOB = Convert.ToDateTime(TextboxBeneficiaryBirthDate.Text)
            Else
                benDOB = New DateTime(1900, 1, 1)
            End If

            Dim dtFinalAnnuity As New DataTable
            Dim lcAnnuitiesListCursor As New DataTable
            lcAnnuitiesListCursor = Session("lcAnnuitiesListCursor")
            dtFinalAnnuity = RetirementBOClass.CalculatePayments(calculateDeathBenefitAnnuity, Convert.ToDecimal(TextboxAmountToUse.Text) _
                , benDOB, Convert.ToDateTime(TextBoxRetireeBirthday.Text) _
                , Convert.ToDateTime(TextBoxRetirementDate.Text), lcAnnuitiesListCursor, dtFinalAnnuity _
                , PlanType, Me.PersonId, Me.FundEventId, "")
            Session("lcAnnuitiesListCursor") = lcAnnuitiesListCursor

            dtFinalAnnuity = RetirementBOClass.CalculateFinalAnnuity(dtFinalAnnuity)

            TextBoxPage1SSIncrease.Text = "0.00"
            TextboxSSIncrease.Text = "0.00"
            TextBoxPage1SSDecrease.Text = "0.00"
            Dim i As Integer
            For i = 0 To lcAnnuitiesListCursor.Rows.Count - 1
                If Not lcAnnuitiesListCursor.Rows(i).Item("mnySSIncrease") = 0 Then
                    TextBoxPage1SSIncrease.Text = Math.Round(lcAnnuitiesListCursor.Rows(i).Item("mnySSIncrease"), 2).ToString
                    TextboxSSIncrease.Text = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSIncrease")), 2).ToString
                End If
                If Not lcAnnuitiesListCursor.Rows(i).Item("mnySSIncrease") = 0 Then
                    TextBoxPage1SSDecrease.Text = Math.Round(lcAnnuitiesListCursor.Rows(i).Item("mnySSDecrease"), 2).ToString
                End If
            Next

            'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
            Dim beneficiaryBirthDate As String = String.Empty
            If TextboxBeneficiaryBirthDate.Text <> String.Empty Then
                'Ashish:2011.10.04-YRS 5.0-1329:J&S
                'If Me.InputBeneficiaryType = BeneficiaryType.Manual Then
                '    beneficiaryBirthDate = Convert.ToDateTime(TextboxBeneficiaryBirthDate.Text)
                'ElseIf Me.InputBeneficiaryType = BeneficiaryType.NonSpouse Then
                '    beneficiaryBirthDate = RetirementBOClass.GetNonSpouseBeneficiaryBirthDate(Me.PersonId)
                'End If
                If Me.InputBeneficiaryType = BeneficiaryType.Manual Or Me.InputBeneficiaryType = BeneficiaryType.NonSpouse Then
                    beneficiaryBirthDate = Convert.ToDateTime(TextboxBeneficiaryBirthDate.Text)
                End If

                If beneficiaryBirthDate <> String.Empty Then
                    Dim ageDiff As Int16
                    ageDiff = YMCARET.YmcaBusinessObject.RetirementBOClass.GetRetireeAndBeneficiaryAgeDiff(TextBoxRetireeBirthday.Text, beneficiaryBirthDate, TextBoxRetirementDate.Text)
                    'START : ML | 20.02.05 | YRS-AT-4769 |J&S Annuities would be displayed as "N/A" based on different Condition.
                    'dtFinalAnnuity = YMCARET.YmcaBusinessObject.RetirementBOClass.AvailJandSAnnuityOptions(ageDiff, dtFinalAnnuity)
                    Dim chronicallyIll As Boolean
                    Dim secureActApplicable As Boolean
                    chronicallyIll = False
                    secureActApplicable = YMCARET.YmcaBusinessObject.RetirementBOClass.IsSecureActApplicable(Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime(beneficiaryBirthDate), DropDownRelationShip.SelectedValue.ToString, Convert.ToDateTime(TextBoxRetirementDate.Text), chronicallyIll)
                    dtFinalAnnuity = YMCARET.YmcaBusinessObject.RetirementBOClass.AvailJandSAnnuityOptions(ageDiff, dtFinalAnnuity, secureActApplicable)

                    If (AreAnyAnnuitiesNA(dtFinalAnnuity)) Then
                        If (chronicallyIll = False) Then
                            LabelJAnnuityUnAvailMessage.Text = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ANNUITY_ESTIMATE_JSNOTE_CHRONICALY_NOT_ILL).DisplayText
                        End If

                        If (chronicallyIll = True) Then
                            LabelJAnnuityUnAvailMessage.Text = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ANNUITY_ESTIMATE_JSNOTE_CHRONICALY_ILL).DisplayText
                        End If
                        'END : ML | 20.02.05 | YRS-AT-4769 |J&S Annuities would be displayed as "N/A" based on different Condition

                        LabelJAnnuityUnAvailMessage.Visible = True
                        Me.AreJAnnuitiesUnavailable = True
                    Else
                        LabelJAnnuityUnAvailMessage.Visible = False
                        Me.AreJAnnuitiesUnavailable = False
                    End If
                End If
                'Added Anudeep:13.10.2012 For BT-1238: YRS 5.0-1541:Estimates calculator ,start
            ElseIf TextboxBeneficiaryBirthDate.Text = String.Empty Then
                Me.AreJAnnuitiesUnavailable = False
                'Added Anudeep:13.10.2012 For BT-1238: YRS 5.0-1541:Estimates calculator ,End
            End If
            'End

            Dim ssBenefit As Decimal
            Dim lnLeveling As Decimal
            If Convert.ToDouble(TextboxFromBenefitValue.Text) > 0 Then
                Dim ssReductionFactor As Decimal = 0
                Dim dsSSReductionFactor As DataSet = RetirementBOClass.getSSReductionFactor(TextBoxRetireeBirthday.Text, TextBoxRetirementDate.Text)
                If (dsSSReductionFactor.Tables.Count > 0) Then
                    If (dsSSReductionFactor.Tables(0).Rows.Count > 0) Then
                        ssReductionFactor = Convert.ToDecimal(dsSSReductionFactor.Tables(0).Rows(0)("numReductionFactor"))
                    End If
                End If

                ssBenefit = Convert.ToDouble(TextboxFromBenefitValue.Text)
                lnLeveling = Math.Round((ssBenefit * ssReductionFactor), 2)

                TextboxSSIncrease.Text = lnLeveling
            End If

            'YRS 5.0-657 : Estimate with SSL amounts
            'Ashish:2011.09.13  YRS 5.0-657
            'If (ssBenefit > 0) Then
            If (lnLeveling > 0) Then
                Dim Copy_dtFinalAnnuity As DataTable
                Copy_dtFinalAnnuity = dtFinalAnnuity.Copy()
                FinalAnnuityAterSSLReduction(lnLeveling, ssBenefit, Copy_dtFinalAnnuity)
                If (AreAllAnnuitiesNA(Copy_dtFinalAnnuity)) Then
                    LabelJAnnuityUnAvailMessage.Visible = False
                    LabelSSLWarningMessage.Visible = False
                    dtFinalAnnuity = Nothing
                    lcAnnuitiesListCursor = Nothing
                    Session("lcAnnuitiesListCursor") = Nothing
                    'commented by Anudeep on 22-sep for BT-1126
                    'ShowCustomMessage("Amount entered would result in negative annuity amounts at age 62. Cannot proceed.", enumMessageBoxType.DotNet)//commented by Anudeep on 22-sep for BT-1126
                    ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_ESTIMATE_NEGATIVE_ANNUITY"), enumMessageBoxType.DotNet)
                    Exit Sub
                Else
                    dtFinalAnnuity = Copy_dtFinalAnnuity
                    If (IsFinalAnnuityNegativeAfterSSL(dtFinalAnnuity)) Then
                        LabelSSLWarningMessage.Visible = True
                    Else
                        LabelSSLWarningMessage.Visible = False
                    End If
                End If
            End If

            'Start-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
            If Me.FundEventStatus = "QD" And DropDownRelationShip.SelectedValue = "SP" Then
                dtFinalAnnuity = YMCARET.YmcaBusinessObject.RetirementBOClass.BlockJandSAnnuityOptionsForQDRO(dtFinalAnnuity)
            End If
            'End-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
            DatatGridSocialSecurityLevel.DataSource = dtFinalAnnuity
            DatatGridSocialSecurityLevel.DataBind()

            'Ashish:2011.09.13  YRS 5.0-657
            'To show annuity 0 in Print report
            If lnLeveling > 0 Then
                AnnuityAfterSSLReduction(lnLeveling, ssBenefit, lcAnnuitiesListCursor)
                If (IsAnnuityNegativeAfterSSL(lcAnnuitiesListCursor)) Then
                    Session("lcAnnuitiesListCursor") = lcAnnuitiesListCursor
                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
    Private Function AreAnyAnnuitiesNA(ByVal p_Annuity As DataTable) As Boolean
        For Each dr In p_Annuity.Rows
            If (dr("Retire") = RetirementBOClass.JSAnnuityUnAvailableValue) Then
                Return True
            End If
        Next
        Return False
    End Function
    'YRS 5.0-657 : Estimate with SSL amounts
    Private Function AnnuityAfterSSLReduction(ByVal p_Leveling As Decimal, ByVal p_SSLBenefit As Decimal, ByRef p_Annuity As DataTable)
        Dim annuityValue As Decimal
        Dim annuityAfter62 As Decimal
        Dim annuityType As String
        If p_Leveling > 0 Then
            For Each dr As DataRow In p_Annuity.Rows
                annuityValue = Convert.ToDecimal(dr("mnyCurrentPayment").ToString())
                annuityType = Convert.ToString(dr("chrAnnuityType"))
                If annuityValue <> 0 Then
                    annuityAfter62 = (annuityValue + p_Leveling) - p_SSLBenefit
                    If (annuityAfter62 <= 0) Then
                        dr("mnyCurrentPayment") = 0
                        dr("mnySurvivorRetiree") = 0
                        dr("mnySurvivorBeneficiary") = 0
                        dr("AnnuityWithoutRDB") = 0
                        p_Annuity.AcceptChanges()
                    End If
                End If
            Next
        End If
    End Function
    'YRS 5.0-657 : Estimate with SSL amounts
    Private Function FinalAnnuityAterSSLReduction(ByVal p_Leveling As Decimal, ByVal p_SSLBenefit As Decimal, ByRef p_Annuity As DataTable)
        Dim annuityValue As Decimal
        Dim annuityAfter62 As Decimal
        For Each dr As DataRow In p_Annuity.Rows
            If (IsNumeric(dr("Retire").ToString())) Then
                annuityValue = Convert.ToDecimal(dr("Retire").ToString())
                annuityAfter62 = (annuityValue + p_Leveling) - p_SSLBenefit
                If (annuityAfter62 <= 0) Then
                    dr("Retire") = Me.SSLUnAvailableValue
                    dr("Survivor") = Me.SSLUnAvailableValue
                    If (dr("Beneficiary") <> String.Empty) Then
                        dr("Beneficiary") = Me.SSLUnAvailableValue
                    End If
                    p_Annuity.AcceptChanges()
                End If
            End If
        Next
    End Function
    'YRS 5.0-657 : Estimate with SSL amounts
    Private Function AreAllAnnuitiesNA(ByVal p_Annuity As DataTable) As Boolean
        Dim dRows As DataRow() = p_Annuity.Select("Retire <> '" + RetirementBOClass.JSAnnuityUnAvailableValue + "'")
        If dRows.Length > 0 Then
            For Each dr As DataRow In dRows
                If dr("Retire") <> Me.SSLUnAvailableValue Then
                    Return False
                End If
            Next
        End If
        Return True
    End Function
    'YRS 5.0-657 : Estimate with SSL amounts
    Private Function IsAnnuityNegativeAfterSSL(ByRef p_Annuity As DataTable) As Boolean
        Dim annuityValue As Decimal
        Dim annuityAfter62 As Decimal

        For Each dr As DataRow In p_Annuity.Rows
            annuityValue = Convert.ToDecimal(dr("mnyCurrentPayment").ToString())
            If (dr("mnyCurrentPayment") = 0) Then
                Return True
            End If
        Next
        Return False
    End Function
    'YRS 5.0-657 : Estimate with SSL amounts
    Private Function IsFinalAnnuityNegativeAfterSSL(ByRef p_Annuity As DataTable) As Boolean
        Dim annuityValue As Decimal
        Dim annuityAfter62 As Decimal
        Dim isNegative As Boolean

        For Each dr As DataRow In p_Annuity.Rows
            If (dr("Retire") = Me.SSLUnAvailableValue) Then
                Return True
            End If
        Next
        Return False
    End Function
    'Added by Ashish for phase V changes ,this function  not in used
    Private Function GetInsuredReserve(ByVal dtProjectedAccounts As DataTable) As DataTable
        Try

            Dim dtExcludedAccounts As DataTable
            dtExcludedAccounts = getExcludedAccounts()

            Dim dtExcludeFromThis As DataTable = dtProjectedAccounts.Copy()
            Dim boolBasicAccountExcluded As Boolean = False

            Dim PlanTypeColumnName As String
            Dim YMCATotalColumnName As String
            Dim PersonalTotalColumnName As String
            Dim BalanceTotalColumnName As String

            If dtExcludeFromThis.Columns.Contains("PlanType") = True Then
                PlanTypeColumnName = "PlanType"
            Else
                PlanTypeColumnName = "chvPlanType"
            End If

            If dtExcludeFromThis.Columns.Contains("YMCATotal") = True Then
                YMCATotalColumnName = "YMCATotal"
            Else
                YMCATotalColumnName = "YMCAAmt"
            End If

            If dtExcludeFromThis.Columns.Contains("PersonalTotal") = True Then
                PersonalTotalColumnName = "PersonalTotal"
            Else
                PersonalTotalColumnName = "PersonalAmt"
            End If

            If dtExcludeFromThis.Columns.Contains("AcctTotal") = True Then
                BalanceTotalColumnName = "AcctTotal"
            Else
                BalanceTotalColumnName = "mnyBalance"
            End If

            If dtExcludedAccounts.Rows.Count > 0 Then
                For Each dr As DataRow In dtExcludedAccounts.Rows
                    For Each drProj As DataRow In dtExcludeFromThis.Rows
                        If drProj("chrAcctType").ToString() = dr("chrAcctType").ToString() Then
                            'checking if the account excluded by user is a basic account and plan type is retirement 
                            If drProj(PlanTypeColumnName).ToString() = "RETIREMENT" AndAlso Convert.ToBoolean(dr("bitBasicAcct")) = True Then
                                'then exclude only the personal side of money for the excluded basic account. 
                                drProj(BalanceTotalColumnName) = drProj(YMCATotalColumnName)

                                'flagging if any single basic account is excluded from the estimate 
                                boolBasicAccountExcluded = True
                            Else
                                drProj(BalanceTotalColumnName) = 0
                            End If
                        Else
                            drProj(BalanceTotalColumnName) = drProj(YMCATotalColumnName) + drProj(PersonalTotalColumnName)
                        End If
                    Next
                Next
            Else
                For Each drProj As DataRow In dtExcludeFromThis.Rows
                    'checking if the account excluded by user is a basic account and plan type is retirement                 
                    If drProj(PlanTypeColumnName).ToString() = "RETIREMENT" Then
                        'then exclude only the personal side of money for the excluded basic account. 
                        drProj(BalanceTotalColumnName) = drProj(YMCATotalColumnName) + drProj(PersonalTotalColumnName)
                    End If
                Next
            End If

            'if any single basic account is excluded from the estimate then 
            'exclude personal side of money from all the basic accounts 
            'and both personal as well as ymca side of money from non-basic account under retirement plan. 
            If boolBasicAccountExcluded = True Then
                'this dataset will be populated with only the basic accounts 
                Dim l_dsMetaAccountTypes As DataSet = RetirementBOClass.SearchMetaAccountTypes()

                For Each drProj As DataRow In dtExcludeFromThis.Rows
                    If drProj(PlanTypeColumnName).ToString() = "RETIREMENT" Then

                        'checking if the account is a basic account then excluding only the personal otherwise personal as well as ymca side of money is excluded from the account. 
                        Dim filterExpression As String = "chrAcctType='" + drProj("chrAcctType").ToString() + "'"

                        If l_dsMetaAccountTypes.Tables(0).[Select](filterExpression).Length > 0 Then

                            'basic account 
                            drProj(BalanceTotalColumnName) = drProj(YMCATotalColumnName)

                        Else

                            'non-basic account 
                            drProj(BalanceTotalColumnName) = 0

                        End If
                    End If
                Next
            End If

            dtExcludeFromThis.AcceptChanges()

            Return dtExcludeFromThis
        Catch
            Throw
        End Try
    End Function

    Private Function PrintEstimate(ByVal paraPrintType As String)
        'Ashish:2010.06.21 YRS 5.0-1115 ,Added parameter printType
        '''declaration of variables
        Dim lcAnnuitiesListCursor As DataTable
        Dim i As Integer
        Dim lcRetireType As String
        Dim dtAccountsBasisByProjection As New DataTable 'moved from module level to local variable since not used any where in the form - on YREN-2814
        Dim dsRetEstimateEmployment As DataSet
        'Start Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request
        Dim l_stringDocType As String = String.Empty
        Dim l_StringReportName As String = String.Empty
        Dim l_ArrListParamValues As New ArrayList
        Dim l_string_OutputFileType As String = String.Empty
        Dim l_StringErrorMessage As String
        'End Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request
        'Commented by Ashish for phase V changes ,Start
        'Dim excludedDataTable As DataTable
        'Dim dtModifiedElectiveAccounts As DataTable
        'Commented by Ashish for phase V changes ,End
        'Added by Ashish for phase V changes 
        Dim l_boolCanCallEstimateAnnuities As Boolean = False
        Try
            lcRetireType = Me.RetireType
            If Session("Print") = "Print" Then
                If TextBoxPraAssumption.Visible = True Then


                    'Added by Ashish for phase V changes ,start
                    If GetProjectedAcctBalances() = False Then
                        SetSocialSecurityDetails()
                        Exit Function
                    End If
                    Dim empEventId As String = String.Empty
                    If DropDownListEmployment.Items.Count > 1 Then
                        empEventId = DropDownListEmployment.Items(1).Value.ToString.Trim.ToUpper
                    End If
                    'populateElectiveAccountsTab(empEventId, True, False)
                    l_boolCanCallEstimateAnnuities = ValidateProjectedBalancesAsPerRefund()
                    SetElectiveAccountSelectionState()
                    ShowProjectedAcctBalancesTotal()
                    SetSocialSecurityDetails()
                    'Added by Ashish for phase V changes ,End 
                    If l_boolCanCallEstimateAnnuities = False Then
                        'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                        'dsRetEstimateEmployment = ValidateEstimate(False)
                        Dim l_dataset_Combined As DataSet
                        'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                        'If Not dsRetEstimateEmployment Is Nothing Then
                        'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                        'l_dataset_Combined = EstimateAnnuities(dsRetEstimateEmployment)
                        If ValidateEstimate(False) Then

                            '2012.05.22 SP:  BT-976/YRS 5.0-1507 - Reopened
                            Me.UpdateSelectedParticiapntBeneficary()

                            l_dataset_Combined = EstimateAnnuities()
                            If l_dataset_Combined Is Nothing Then
                                Exit Function
                            End If
                        Else
                            Exit Function 'Retirement validation failed 
                        End If
                        'Commentd by Ashish for phase V changes ,start
                        'excludedDataTable = getExcludedAccounts()

                        'dtModifiedElectiveAccounts = removeExcludedAccounts(l_dataset_Combined.Tables(0), excludedDataTable)

                        ''dtAccountsBasisByProjection = l_dataset_Combined.Tables(0)
                        'dtAccountsBasisByProjection = dtModifiedElectiveAccounts
                        'Commentd by Ashish for phase V changes ,End

                        'added by Ashish for phase V changes,Start
                        dtAccountsBasisByProjection = l_dataset_Combined.Tables(0)
                        'added by Ashish for phase V changes,End
                        lcAnnuitiesListCursor = l_dataset_Combined.Tables(1)

                        'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
                        Dim beneficiaryBirthDate As String = String.Empty
                        If TextboxBeneficiaryBirthDate.Text <> String.Empty Then
                            'ASHISH:2011.10.04-YRS 5.0-1329
                            'If Me.InputBeneficiaryType = BeneficiaryType.Manual Then
                            '    beneficiaryBirthDate = Convert.ToDateTime(TextboxBeneficiaryBirthDate.Text)
                            'ElseIf Me.InputBeneficiaryType = BeneficiaryType.NonSpouse Then
                            '    beneficiaryBirthDate = RetirementBOClass.GetNonSpouseBeneficiaryBirthDate(Me.PersonId)
                            'End If
                            If Me.InputBeneficiaryType = BeneficiaryType.Manual Or Me.InputBeneficiaryType = BeneficiaryType.NonSpouse Then
                                beneficiaryBirthDate = Convert.ToDateTime(TextboxBeneficiaryBirthDate.Text)
                            End If

                            If beneficiaryBirthDate <> String.Empty Then
                                Dim ageDiff As Int16
                                ageDiff = YMCARET.YmcaBusinessObject.RetirementBOClass.GetRetireeAndBeneficiaryAgeDiff(TextBoxRetireeBirthday.Text, beneficiaryBirthDate, TextBoxRetirementDate.Text)
                                'START : ML | 20.02.05 | YRS-AT-4769 |J&S Annuities would be displayed as "N/A" based on different Condition
                                'YMCARET.YmcaBusinessObject.RetirementBOClass.AvailJandSAnnuityOptionsForPrintEstimate(ageDiff, lcAnnuitiesListCursor)
                                Dim chronicallyIll As Boolean
                                Dim secureActApplicable As Boolean
                                chronicallyIll = False
                                secureActApplicable = YMCARET.YmcaBusinessObject.RetirementBOClass.IsSecureActApplicable(Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime(beneficiaryBirthDate), DropDownRelationShip.SelectedValue.ToString, Convert.ToDateTime(TextBoxRetirementDate.Text), chronicallyIll)
                                YMCARET.YmcaBusinessObject.RetirementBOClass.AvailJandSAnnuityOptionsForPrintEstimate(ageDiff, lcAnnuitiesListCursor, secureActApplicable)
                                'END : ML | 20.02.05 | YRS-AT-4769 |J&S Annuities would be displayed as "N/A" based on different Condition
                            End If
                        End If
                        'End
                    Else
                        Exit Function   'since failed in validation.

                    End If
                    'Start-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                    If Me.FundEventStatus = "QD" And DropDownRelationShip.SelectedValue = "SP" Then
                        YMCARET.YmcaBusinessObject.RetirementBOClass.BlockJandSAnnuityOptionsForPrintEstimateQDRO(lcAnnuitiesListCursor)
                    End If
                    'End-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                    TextBoxPraAssumption.Visible = False

                    '''Shashi Shekhar:5 Aug 2010 :YRS 5.0-1142
                    'ButtonPRA.Visible = False
                    ''Ashish:2010.06.21 YRS 5.0-1115 ,start
                    'btnPRAShort.Visible = False
                    'btnPRAShort.Enabled = False
                    'btnPRAColor.Visible = False
                    'btnPRAColor.Enabled = False

                    ''Ashish:2010.06.21 YRS 5.0-1115 ,End
                    'Ashish YRS 5.0-1345 :BT-859
                    PanelAlternatePayee.Visible = False
                    Panel1.Visible = False
                    'BT:892-YRS 5.0-1359 : Disability Estimate form
                    PanelDisability.Visible = False
                    ''----------------------------------------

                    ButtonCancel.Enabled = True
                    ButtonPrint.Enabled = True
                    Buttoncalculate.Enabled = True
                    tabStripRetirementEstimate.Enabled = True
                    labelPRAAssumption.Visible = False
                    'Added by Ashish 2009.10.15, display summary tab
                    tabStripRetirementEstimate.SelectedIndex = 0
                    Me.MultiPageRetirementEstimate.SelectedIndex = Me.tabStripRetirementEstimate.SelectedIndex
                    '****************Declaration******************************************
                    Dim AgeRetired As Integer = Convert.ToInt32(TextBoxRetirementAge.Text)
                    Dim BenBirthDate As String = TextboxBeneficiaryBirthDate.Text.Trim().ToString()
                    'Dim BeneficiaryName As String = TextboxLastName.Text & " " & TextboxFirstName.Text 'YRPS-4534


                    'START: SG: 2012.12.10: BT-1426:
                    'Dim BeneficiaryName As String = TextboxFirstName.Text.Trim().ToString() & " " & TextboxLastName.Text.Trim().ToString()  'YRPS-4534
                    Dim BeneficiaryName As String = String.Empty

                    If Not String.IsNullOrEmpty(TextboxFirstName.Text) OrElse Not String.IsNullOrEmpty(TextboxLastName.Text) Then
                        BeneficiaryName = TextboxFirstName.Text.Trim().ToString() & " " & TextboxLastName.Text.Trim().ToString()  'YRPS-4534
                    ElseIf Not Session("BeneficiaryName") Is Nothing Then
                        BeneficiaryName = CType(Session("BeneficiaryName"), String).Trim()
                    End If
                    'END: SG: 2012.12.10: BT-1426:

                    Dim PRAAssumption As String = TextBoxPraAssumption.Text.Trim().ToString()
                    Dim RetBirthDate As String = TextBoxRetireeBirthday.Text.Trim().ToString()
                    Dim RetiredDate As String = TextBoxRetirementDate.Text.Trim().ToString()
                    Dim RetiredDeathBen As Decimal = Convert.ToDecimal(TextboxRetiredDeathBenefit.Text)
                    Dim C_Insured As Decimal
                    Dim C_Monthly As Decimal
                    Dim C_Reduction As Decimal
                    Dim C62_Insured As Decimal
                    Dim C62_Monthly As Decimal
                    Dim C62_Reduction As Decimal
                    Dim CS_Insured As Decimal
                    Dim CS_Monthly As Decimal
                    Dim CS_Reduction As Decimal
                    Dim guiUniqueID As Decimal
                    Dim J1_Retiree As Decimal
                    Dim J1_Survivor As Decimal
                    Dim J162_Retiree As Decimal
                    Dim J162_Survivor As Decimal
                    Dim J1I_Retiree As Decimal
                    Dim J1I_Survivor As Decimal
                    Dim J1P_Retiree As Decimal
                    Dim J1P_Survivor As Decimal
                    Dim J1P62_Retiree As Decimal
                    Dim J1P62_Survivor As Decimal
                    Dim J1PS_Retiree As Decimal
                    Dim J1PS_Survivor As Decimal
                    Dim J1S_Retiree As Decimal
                    Dim J1S_Survivor As Decimal
                    Dim J5_Retiree As Decimal
                    Dim J5_Survivor As Decimal
                    Dim J562_Retiree As Decimal
                    Dim J562_Survivor As Decimal
                    Dim J5I_Retiree As Decimal
                    Dim J5I_Survivor As Decimal
                    Dim J5L_Retiree As Decimal
                    Dim J5L_Survivor As Decimal
                    Dim J5L62_Retiree As Decimal
                    Dim J5L62_Survivor As Decimal
                    Dim J5LS_Retiree As Decimal
                    Dim J5LS_Survivor As Decimal
                    Dim J5P_Retiree As Decimal
                    Dim J5P_Survivor As Decimal
                    Dim J5P62_Retiree As Decimal
                    Dim J5P62_Survivor As Decimal
                    Dim J5PS_Retiree As Decimal
                    Dim J5PS_Survivor As Decimal
                    Dim J5S_Retiree As Decimal
                    Dim J5S_Survivor As Decimal
                    Dim J7_Retiree As Decimal
                    Dim J7_Survivor As Decimal
                    Dim J762_Retiree As Decimal
                    Dim J762_Survivor As Decimal
                    Dim J7I_Retiree As Decimal
                    Dim J7I_Survivor As Decimal
                    Dim J7L_Retiree As Decimal
                    Dim J7L_Survivor As Decimal
                    Dim J7L62_Retiree As Decimal
                    Dim J7L62_Survivor As Decimal
                    Dim J7LS_Retiree As Decimal
                    Dim J7LS_Survivor As Decimal
                    Dim J7P_Retiree As Decimal
                    Dim J7P_Survivor As Decimal
                    Dim J7P62_Retiree As Decimal
                    Dim J7P62_Survivor As Decimal
                    Dim J7PS_Retiree As Decimal
                    Dim J7PS_Survivor As Decimal
                    Dim J7S_Retiree As Decimal
                    Dim J7S_Survivor As Decimal
                    Dim M_Retiree As Decimal
                    Dim M62_Retiree As Decimal
                    Dim MI_Retiree As Decimal
                    Dim MS_Retiree As Decimal
                    Dim ZC_Annually As Decimal
                    Dim ZC62_Annually As Decimal
                    Dim ZCS_Annually As Decimal
                    Dim ZJ1_Retiree As Decimal
                    Dim ZJ1_Survivor As Decimal
                    Dim ZJ162_Retiree As Decimal
                    Dim ZJ162_Survivor As Decimal
                    Dim ZJ1I_Retiree As Decimal
                    Dim ZJ1I_Survivor As Decimal
                    Dim ZJ1P_Retiree As Decimal
                    Dim ZJ1P_Survivor As Decimal
                    Dim ZJ1P62_Retiree As Decimal
                    Dim ZJ1P62_Survivor As Decimal
                    Dim ZJ1PS_Retiree As Decimal
                    Dim ZJ1PS_Survivor As Decimal
                    Dim ZJ1S_Retiree As Decimal
                    Dim ZJ1S_Survivor As Decimal
                    Dim ZJ5_Retiree As Decimal
                    Dim ZJ5_Survivor As Decimal
                    Dim ZJ562_Retiree As Decimal
                    Dim ZJ562_Survivor As Decimal
                    Dim ZJ5I_Retiree As Decimal
                    Dim ZJ5I_Survivor As Decimal
                    Dim ZJ5L_Retiree As Decimal
                    Dim ZJ5L_Survivor As Decimal
                    Dim ZJ5L62_Retiree As Decimal
                    Dim ZJ5L62_Survivor As Decimal
                    Dim ZJ5LS_Retiree As Decimal
                    Dim ZJ5LS_Survivor As Decimal
                    Dim ZJ5P_Retiree As Decimal
                    Dim ZJ5P_Survivor As Decimal
                    Dim ZJ5P62_Retiree As Decimal
                    Dim zJ5P62_Survivor As Decimal
                    Dim ZJ5PS_Retiree As Decimal
                    Dim ZJ5PS_Survivor As Decimal
                    Dim ZJ5S_Retiree As Decimal
                    Dim ZJ5S_Survivor As Decimal
                    Dim ZJ7_Retiree As Decimal
                    Dim ZJ7_Survivor As Decimal
                    Dim ZJ762_Retiree As Decimal
                    Dim ZJ762_Survivor As Decimal
                    Dim ZJ7I_Retiree As Decimal
                    Dim ZJ7I_Survivor As Decimal
                    Dim ZJ7L_Retiree As Decimal
                    Dim ZJ7L_Survivor As Decimal
                    Dim zJ7L62_Retiree As Decimal
                    Dim ZJ7L62_Survivor As Decimal
                    Dim ZJ7LS_Retiree As Decimal
                    Dim ZJ7LS_Survivor As Decimal
                    Dim ZJ7P_Retiree As Decimal
                    Dim zJ7P_Survivor As Decimal
                    Dim ZJ7P62_Retiree As Decimal
                    Dim zJ7P62_Survivor As Decimal
                    Dim ZJ7PS_Retiree As Decimal
                    Dim zJ7PS_Survivor As Decimal
                    Dim ZJ7S_Retiree As Decimal
                    Dim ZJ7S_Survivor As Decimal
                    Dim ZM_Retiree As Decimal
                    Dim ZM62_Retiree As Decimal
                    Dim ZMI_Retiree As Decimal
                    Dim ZMS_Retiree As Decimal
                    Dim lnInsuredReserve As Decimal
                    '****************Declaration******************************************
                    'Commented By Ashish for Phase V changes ,Start

                    ''-------
                    'Dim dt As DataTable
                    'dt = GetInsuredReserve(dtAccountsBasisByProjection)
                    ''-------

                    'If Not dtAccountsBasisByProjection.Rows.Count = 0 Then
                    '    If Me.PlanType = "B" Then
                    '        lnInsuredReserve = Math.Round(Convert.ToDecimal(dt.Compute("SUM(mnyBalance)", "")), 2)
                    '    ElseIf Me.PlanType = "R" Then
                    '        lnInsuredReserve = Math.Round(Convert.ToDecimal(dt.Compute("SUM(mnyBalance)", "chvPlanType='RETIREMENT'")), 2)
                    '    Else 'Me.PlanType = "S" Then
                    '        lnInsuredReserve = Math.Round(Convert.ToDecimal(dt.Compute("SUM(mnyBalance)", "chvPlanType='SAVINGS'")), 2)
                    '    End If
                    'End If
                    'Commented By Ashish for Phase V changes ,End

                    'Added by Ashish for Phase V changes ,Start
                    If Not dtAccountsBasisByProjection.Rows.Count = 0 Then
                        '2012.06.11   SP :BT-833/YRS 5.0-1327 -(adding checking retiretype in if condiiton)

                        'Commented By chandrarasekar.c for YRS-AT : 2486 ,Start 
                        '- commenting these lines of code as referring the txtProjectedReserves value as it already has the computed reserves value.
                        'If Me.PlanType = "B" And RetireType.ToUpper() <> "DISABL" Then
                        '	lnInsuredReserve = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(mnyBalance)", "")), 2)
                        'ElseIf Me.PlanType = "B" And RetireType.ToUpper() = "DISABL" Then
                        '	lnInsuredReserve = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(personalAmt)", "")), 2)
                        'ElseIf Me.PlanType = "R" And RetireType.ToUpper() <> "DISABL" Then
                        '	lnInsuredReserve = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(mnyBalance)", "chvPlanType='RETIREMENT'")), 2)
                        'ElseIf Me.PlanType = "R" And RetireType.ToUpper() = "DISABL" Then
                        '	lnInsuredReserve = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(personalAmt)", "chvPlanType='RETIREMENT'")), 2)
                        'ElseIf Me.PlanType = "S" And RetireType.ToUpper() <> "DISABL" Then
                        '	lnInsuredReserve = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(mnyBalance)", "chvPlanType='SAVINGS'")), 2)
                        'ElseIf Me.PlanType = "S" And RetireType.ToUpper() = "DISABL" Then
                        ' 	lnInsuredReserve = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(personalAmt)", "chvPlanType='SAVINGS'")), 2)
                        'End If
                        'Commented By chandrarasekar.c for YRS-AT : 2486 ,End

                        '2012.06.11   SP :BT-833/YRS 5.0-1327:Option (reopen)

                        'Start:Added by chandrasekar.c on 2015.10.27 YRS-AT Ticket-2486 Projected Reserve is displayed in the Print report as the Guaranteed Principal.
                        lnInsuredReserve = Math.Round(Convert.ToDecimal(txtProjectedReserves.Text.Trim()), 2)
                        'End:Added by chandrasekar.c on 2015.10.27 YRS-AT Ticket-2486 Projected Reserve is displayed in the Print report as the Guaranteed Principal.

                    End If

                    'Added by Ashish for Phase V changes ,End
                    Dim ymcaAnnutyValue As Decimal = 0
                    For i = 0 To lcAnnuitiesListCursor.Rows.Count - 1

                        ''''C
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "C" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            C_Insured = lnInsuredReserve
                            'START: PPP | 03/17/2017 | YRS-AT-2625 | In disability C Reserves = Actual Balance of both plans
                            If Me.RetireType.ToUpper() = "DISABL" Then
                                C_Insured = Me.CAnnuityGuranteedReservesForDisability
                            End If
                            'END: PPP | 03/17/2017 | YRS-AT-2625 | In disability C Reserves = Actual Balance of both plans

                            C_Monthly = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)

                            'START: PPP | 03/16/2017 | YRS-AT-2625 | For Disability C Annuity Column: AnnuityWithoutRDB will hold C_Reduction value. 
                            '--Calculation of AnnuityWithoutRDB = (Retirement Plan Disability C Annuity Current Payment * Reduction Factor) + if exists then Savings Plan Disability C Annuity Current Payment 
                            '--So for both retirement type AnnuityWithoutRDB will hold C_Reduction value
                            '--Disability C_Reduction(AnnuityWithoutRDB) calculation is in "EstimateAnnuities" local function 
                            ''C_Reduction = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                            ''Sanket Vaidya          2011.07.27      YRS 5.0-1150/BT-596 : Option C reduction amount should not include Dth Ben annty
                            'If RetireType.ToUpper() = "DISABL" Then '2012.06.11   SP :BT-833/YRS 5.0-1327 -start (adding if conditon)
                            '    ymcaAnnutyValue = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("YmcaAnnuityValue")), 2)
                            '    C_Reduction = (Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("AnnuityWithoutRDB")), 2) - ymcaAnnutyValue)
                            'Else
                            '    C_Reduction = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("AnnuityWithoutRDB")), 2)
                            'End If '2012.06.11   SP :BT-833/YRS 5.0-1327 -end
                            C_Reduction = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("AnnuityWithoutRDB")), 2)
                            'END: PPP | 03/16/2017 | YRS-AT-2625 | For Disability C Annuity Column: AnnuityWithoutRDB will hold C_Reduction value. 
                        End If

                        ''''CS
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "CS" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            C62_Insured = lnInsuredReserve
                            C62_Monthly = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSAfter62")), 2)
                            C62_Reduction = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                            CS_Insured = lnInsuredReserve
                            CS_Monthly = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSBefore62")), 2)
                            CS_Reduction = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                        End If

                        ''''J1
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J1" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J1_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                            J1_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J1I
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J1I" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J1I_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                            J1I_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J1P
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J1P" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J1P_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                            J1P_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J1PS
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J1PS" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J1P62_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSAfter62")), 2)
                            J1P62_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)

                            J1PS_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSBefore62")), 2)
                            J1PS_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J1S
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J1S" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J162_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSAfter62")), 2)
                            J162_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                            J1S_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSBefore62")), 2)
                            J1S_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J5
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J5" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J5_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                            J5_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J5I
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J5I" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J5I_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                            J5I_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J5L
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J5L" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J5L_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                            J5L_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J5LS
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J5LS" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J5L62_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSAfter62")), 2)
                            J5L62_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) * (0.5), 2)
                            J5LS_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSBefore62")), 2)
                            J5LS_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J5P
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J5P" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J5P_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                            J5P_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J5PS
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J5PS" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J5P62_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSAfter62")), 2)
                            J5P62_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                            J5PS_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSBefore62")), 2)
                            J5PS_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J5S
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J5S" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J562_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSAfter62")), 2)
                            J562_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) * (0.5), 2)
                            J5S_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSBefore62")), 2)
                            J5S_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J7
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J7" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J7_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                            J7_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J7I
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J7I" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J7I_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                            J7I_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J7LS
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J7LS" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J7L62_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSAfter62")), 2)
                            J7L62_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) * (0.75), 2)
                            J7LS_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSBefore62")), 2)
                            J7LS_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J7P
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J7P" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J7P_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                            J7P_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J7PS
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J7PS" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J7P62_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSAfter62")), 2)
                            J7P62_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                            J7PS_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSBefore62")), 2)
                            J7PS_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''J7S
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "J7S" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            J762_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSAfter62")), 2)
                            J762_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) * (0.75), 2)
                            J7S_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSBefore62")), 2)
                            J7S_Survivor = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySurvivorBeneficiary")), 2)
                        End If

                        ''''M
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "M" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            M_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                        End If

                        ''''MI
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "MI" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            MI_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                        End If

                        ''''MS
                        If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "MS" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                            M62_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSAfter62")), 2)
                            MS_Retiree = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSBefore62")), 2)
                        End If
                    Next

                    '''' Compute annual values
                    ZC_Annually = 12 * C_Monthly
                    ZCS_Annually = 12 * CS_Monthly
                    ZC62_Annually = 12 * C62_Monthly

                    ZM_Retiree = 12 * M_Retiree
                    ZMI_Retiree = 12 * MI_Retiree
                    ZMS_Retiree = 12 * MS_Retiree
                    ZM62_Retiree = 12 * M62_Retiree

                    ZJ1_Retiree = 12 * J1_Retiree
                    ZJ1I_Retiree = 12 * J1I_Retiree
                    ZJ1S_Retiree = 12 * J1S_Retiree
                    ZJ162_Retiree = 12 * J162_Retiree
                    ZJ1P_Retiree = 12 * J1P_Retiree
                    ZJ1PS_Retiree = 12 * J1PS_Retiree
                    ZJ1P62_Retiree = 12 * J1P62_Retiree

                    ZJ1_Survivor = 12 * J1_Survivor
                    ZJ1I_Survivor = 12 * J1I_Survivor
                    ZJ1S_Survivor = 12 * J1S_Survivor ''''fa.1.8.2004  changed from J1I_Survivor
                    ZJ162_Survivor = 12 * J162_Survivor
                    ZJ1P_Survivor = 12 * J1P_Survivor
                    ZJ1PS_Survivor = 12 * J1PS_Survivor
                    ZJ1P62_Survivor = 12 * J1P62_Survivor

                    ZJ5_Retiree = 12 * J5_Retiree
                    ZJ5I_Retiree = 12 * J5I_Retiree
                    ZJ5S_Retiree = 12 * J5S_Retiree
                    ZJ562_Retiree = 12 * J562_Retiree
                    ZJ5P_Retiree = 12 * J5P_Retiree
                    ZJ5PS_Retiree = 12 * J5PS_Retiree
                    ZJ5P62_Retiree = 12 * J5P62_Retiree
                    ZJ5L_Retiree = 12 * J5L_Retiree
                    ZJ5LS_Retiree = 12 * J5LS_Retiree
                    ZJ5L62_Retiree = 12 * J5L62_Retiree

                    ZJ5_Survivor = 12 * J5_Survivor
                    ZJ5I_Survivor = 12 * J5I_Survivor
                    ZJ5S_Survivor = 12 * J5S_Survivor
                    ZJ562_Survivor = 12 * J562_Survivor
                    ZJ5P_Survivor = 12 * J5P_Survivor
                    ZJ5PS_Survivor = 12 * J5PS_Survivor
                    zJ5P62_Survivor = 12 * J5P62_Survivor
                    ZJ5L_Survivor = 12 * J5L_Survivor
                    ZJ5LS_Survivor = 12 * J5LS_Survivor
                    ZJ5L62_Survivor = 12 * J5L62_Survivor

                    ZJ7_Retiree = 12 * J7_Retiree
                    ZJ7I_Retiree = 12 * J7I_Retiree
                    ZJ7S_Retiree = 12 * J7S_Retiree
                    ZJ762_Retiree = 12 * J762_Retiree
                    ZJ7P_Retiree = 12 * J7P_Retiree
                    ZJ7PS_Retiree = 12 * J7PS_Retiree
                    ZJ7P62_Retiree = 12 * J7P62_Retiree
                    ZJ7L_Retiree = 12 * J7L_Retiree
                    ZJ7LS_Retiree = 12 * J7LS_Retiree
                    zJ7L62_Retiree = 12 * J7L62_Retiree

                    ZJ7_Survivor = 12 * J7_Survivor
                    ZJ7I_Survivor = 12 * J7I_Survivor
                    ZJ7S_Survivor = 12 * J7S_Survivor
                    ZJ762_Survivor = 12 * J762_Survivor

                    zJ7P_Survivor = 12 * J7P_Survivor
                    zJ7PS_Survivor = 12 * J7PS_Survivor
                    zJ7P62_Survivor = 12 * J7P62_Survivor
                    ZJ7L_Survivor = 12 * J7L_Survivor
                    ZJ7LS_Survivor = 12 * J7LS_Survivor
                    ZJ7L62_Survivor = 12 * J7L62_Survivor

                    Dim dsPRAReportValues As New DataSet

                    Dim dtPRAReportParameters As New DataTable("PRAReportValues")

                    dtPRAReportParameters.Columns.Add("GUID")
                    dtPRAReportParameters.Columns.Add("PersID")
                    dtPRAReportParameters.Columns.Add("AgeRetired", GetType(Int32))
                    dtPRAReportParameters.Columns.Add("BenBirthDate")
                    dtPRAReportParameters.Columns.Add("BeneficiaryName")
                    dtPRAReportParameters.Columns.Add("PRAAssumption")
                    dtPRAReportParameters.Columns.Add("RetBirthDate")
                    dtPRAReportParameters.Columns.Add("RetiredDate")

                    dtPRAReportParameters.Columns.Add("RetiredDeathBen", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("C_Insured", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("C_Monthly", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("C_Reduction", GetType(Decimal))

                    dtPRAReportParameters.Columns.Add("C62_Insured", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("C62_Monthly", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("C62_Reduction", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("CS_Insured", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("CS_Monthly", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("CS_Reduction", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J1_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J1_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J162_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J162_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J1I_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J1I_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J1P_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J1P_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J1P62_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J1P62_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J1PS_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J1PS_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J1S_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J1S_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J562_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J562_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5I_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5I_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5L_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5L_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5L62_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5L62_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5LS_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5LS_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5P_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5P_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5P62_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5P62_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5PS_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5PS_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5S_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J5S_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J762_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J762_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7I_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7I_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7L_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7L_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7L62_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7L62_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7LS_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7LS_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7P_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7P_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7P62_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7P62_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7PS_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7PS_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7S_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("J7S_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("M_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("M62_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("MI_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("MS_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZC_Annually", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZC62_Annually", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZCS_Annually", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ1_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ1_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ162_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ162_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ1I_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ1I_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ1P_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ1P_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ1P62_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ1P62_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ1PS_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ1PS_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ1S_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ1S_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ562_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ562_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5I_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5I_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5L_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5L_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5L62_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5L62_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5LS_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5LS_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5P_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5P_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5P62_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("zJ5P62_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5PS_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5PS_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5S_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ5S_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ7_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ7_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ762_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ762_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ7I_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ7I_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ7L_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ7L_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("zJ7L62_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ7L62_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ7LS_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ7LS_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ7P_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("zJ7P_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ7P62_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("zJ7P62_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ7PS_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("zJ7PS_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ7S_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZJ7S_Survivor", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZM_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZM62_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZMI_Retiree", GetType(Decimal))
                    dtPRAReportParameters.Columns.Add("ZMS_Retiree", GetType(Decimal))
                    'Ashish:2010.06.21 YRS 5.0-1115
                    dtPRAReportParameters.Columns.Add("FundNo", GetType(String))


                    Dim drPRAReportValues As DataRow = dtPRAReportParameters.NewRow
                    drPRAReportValues.Item("GUID") = System.Guid.NewGuid.ToString()
                    drPRAReportValues.Item("PersID") = Me.PersonId
                    drPRAReportValues.Item("AgeRetired") = AgeRetired
                    drPRAReportValues.Item("BenBirthDate") = BenBirthDate
                    drPRAReportValues.Item("BeneficiaryName") = BeneficiaryName
                    drPRAReportValues.Item("PRAAssumption") = TextBoxPraAssumption.Text
                    drPRAReportValues.Item("RetBirthDate") = RetBirthDate
                    drPRAReportValues.Item("RetiredDate") = RetiredDate

                    drPRAReportValues.Item("RetiredDeathBen") = RetiredDeathBen
                    drPRAReportValues.Item("C_Insured") = C_Insured
                    drPRAReportValues.Item("C_Monthly") = C_Monthly
                    drPRAReportValues.Item("C_Reduction") = C_Reduction
                    drPRAReportValues.Item("C62_Insured") = C62_Insured
                    drPRAReportValues.Item("C62_Monthly") = C62_Monthly
                    drPRAReportValues.Item("C62_Reduction") = C62_Reduction
                    drPRAReportValues.Item("CS_Insured") = CS_Insured
                    drPRAReportValues.Item("CS_Monthly") = CS_Monthly
                    drPRAReportValues.Item("CS_Reduction") = CS_Reduction

                    drPRAReportValues.Item("J1_Retiree") = J1_Retiree
                    drPRAReportValues.Item("J1_Survivor") = J1_Survivor
                    drPRAReportValues.Item("J162_Retiree") = J162_Retiree
                    drPRAReportValues.Item("J162_Survivor") = J162_Survivor
                    drPRAReportValues.Item("J1I_Retiree") = J1I_Retiree
                    drPRAReportValues.Item("J1I_Survivor") = J1I_Survivor
                    drPRAReportValues.Item("J1P_Retiree") = J1P_Retiree
                    drPRAReportValues.Item("J1P_Survivor") = J1P_Survivor
                    drPRAReportValues.Item("J1P62_Retiree") = J1P62_Retiree
                    drPRAReportValues.Item("J1P62_Survivor") = J1P62_Survivor
                    drPRAReportValues.Item("J1PS_Retiree") = J1PS_Retiree
                    drPRAReportValues.Item("J1PS_Survivor") = J1PS_Survivor
                    drPRAReportValues.Item("J1S_Retiree") = J1S_Retiree
                    drPRAReportValues.Item("J1S_Survivor") = J1S_Survivor
                    drPRAReportValues.Item("J5_Retiree") = J5_Retiree
                    drPRAReportValues.Item("J5_Survivor") = J5_Survivor
                    drPRAReportValues.Item("J562_Retiree") = J562_Retiree
                    drPRAReportValues.Item("J562_Survivor") = J562_Survivor
                    drPRAReportValues.Item("J5I_Retiree") = J5I_Retiree
                    drPRAReportValues.Item("J5I_Survivor") = J5I_Survivor
                    drPRAReportValues.Item("J5L_Retiree") = J5L_Retiree
                    drPRAReportValues.Item("J5L_Survivor") = J5L_Survivor
                    drPRAReportValues.Item("J5L62_Retiree") = J5L62_Retiree
                    drPRAReportValues.Item("J5L62_Survivor") = J5L62_Survivor
                    drPRAReportValues.Item("J5LS_Retiree") = J5LS_Retiree
                    drPRAReportValues.Item("J5LS_Survivor") = J5LS_Survivor
                    drPRAReportValues.Item("J5P_Retiree") = J5P_Retiree
                    drPRAReportValues.Item("J5P_Survivor") = J5P_Survivor
                    drPRAReportValues.Item("J5P62_Retiree") = J5P62_Retiree
                    drPRAReportValues.Item("J5P62_Survivor") = J5P62_Survivor
                    drPRAReportValues.Item("J5PS_Retiree") = J5PS_Retiree
                    drPRAReportValues.Item("J5PS_Survivor") = J5PS_Survivor
                    drPRAReportValues.Item("J5S_Retiree") = J5S_Retiree
                    drPRAReportValues.Item("J5S_Survivor") = J5S_Survivor
                    drPRAReportValues.Item("J7_Retiree") = J7_Retiree
                    drPRAReportValues.Item("J7_Survivor") = J7_Survivor
                    drPRAReportValues.Item("J762_Retiree") = J762_Retiree
                    drPRAReportValues.Item("J762_Survivor") = J762_Survivor
                    drPRAReportValues.Item("J7I_Retiree") = J7I_Retiree
                    drPRAReportValues.Item("J7I_Survivor") = J7I_Survivor
                    drPRAReportValues.Item("J7L_Retiree") = J7L_Retiree
                    drPRAReportValues.Item("J7L_Survivor") = J7L_Survivor
                    drPRAReportValues.Item("J7L62_Retiree") = J7L62_Retiree
                    drPRAReportValues.Item("J7L62_Survivor") = J7L62_Survivor
                    drPRAReportValues.Item("J7LS_Retiree") = J7LS_Retiree
                    drPRAReportValues.Item("J7LS_Survivor") = J7LS_Survivor
                    drPRAReportValues.Item("J7P_Retiree") = J7P_Retiree
                    drPRAReportValues.Item("J7P_Survivor") = J7P_Survivor
                    drPRAReportValues.Item("J7P62_Retiree") = J7P62_Retiree
                    drPRAReportValues.Item("J7P62_Survivor") = J7P62_Survivor
                    drPRAReportValues.Item("J7PS_Retiree") = J7PS_Retiree
                    drPRAReportValues.Item("J7PS_Survivor") = J7PS_Survivor
                    drPRAReportValues.Item("J7S_Retiree") = J7S_Retiree
                    drPRAReportValues.Item("J7S_Survivor") = J7S_Survivor
                    drPRAReportValues.Item("M_Retiree") = M_Retiree
                    drPRAReportValues.Item("M62_Retiree") = M62_Retiree
                    drPRAReportValues.Item("MI_Retiree") = MI_Retiree
                    drPRAReportValues.Item("MS_Retiree") = MS_Retiree
                    drPRAReportValues.Item("ZC_Annually") = ZC_Annually
                    drPRAReportValues.Item("ZC62_Annually") = ZC62_Annually
                    drPRAReportValues.Item("ZCS_Annually") = ZCS_Annually
                    drPRAReportValues.Item("ZJ1_Retiree") = ZJ1_Retiree
                    drPRAReportValues.Item("ZJ1_Survivor") = ZJ1_Survivor
                    drPRAReportValues.Item("ZJ162_Retiree") = ZJ162_Retiree
                    drPRAReportValues.Item("ZJ162_Survivor") = ZJ162_Survivor
                    drPRAReportValues.Item("ZJ1I_Retiree") = ZJ1I_Retiree
                    drPRAReportValues.Item("ZJ1I_Survivor") = ZJ1I_Survivor
                    drPRAReportValues.Item("ZJ1P_Retiree") = ZJ1P_Retiree
                    drPRAReportValues.Item("ZJ1P_Survivor") = ZJ1P_Survivor
                    drPRAReportValues.Item("ZJ1P62_Retiree") = ZJ1P62_Retiree
                    drPRAReportValues.Item("ZJ1P62_Survivor") = ZJ1P62_Survivor
                    drPRAReportValues.Item("ZJ1PS_Retiree") = ZJ1PS_Retiree
                    drPRAReportValues.Item("ZJ1PS_Survivor") = ZJ1PS_Survivor
                    drPRAReportValues.Item("ZJ1S_Retiree") = ZJ1S_Retiree
                    drPRAReportValues.Item("ZJ1S_Survivor") = ZJ1S_Survivor
                    drPRAReportValues.Item("ZJ5_Retiree") = ZJ5_Retiree
                    drPRAReportValues.Item("ZJ5_Survivor") = ZJ5_Survivor
                    drPRAReportValues.Item("ZJ562_Retiree") = ZJ562_Retiree
                    drPRAReportValues.Item("ZJ562_Survivor") = ZJ562_Survivor
                    drPRAReportValues.Item("ZJ5I_Retiree") = ZJ5I_Retiree
                    drPRAReportValues.Item("ZJ5I_Survivor") = ZJ5I_Survivor
                    drPRAReportValues.Item("ZJ5L_Retiree") = ZJ5L_Retiree
                    drPRAReportValues.Item("ZJ5L_Survivor") = ZJ5L_Survivor
                    drPRAReportValues.Item("ZJ5L62_Retiree") = ZJ5L62_Retiree
                    drPRAReportValues.Item("ZJ5L62_Survivor") = ZJ5L62_Survivor
                    drPRAReportValues.Item("ZJ5LS_Retiree") = ZJ5LS_Retiree
                    drPRAReportValues.Item("ZJ5LS_Survivor") = ZJ5LS_Survivor
                    drPRAReportValues.Item("ZJ5P_Retiree") = ZJ5P_Retiree
                    drPRAReportValues.Item("ZJ5P_Survivor") = ZJ5P_Survivor
                    drPRAReportValues.Item("ZJ5P62_Retiree") = ZJ5P62_Retiree
                    drPRAReportValues.Item("zJ5P62_Survivor") = zJ5P62_Survivor
                    drPRAReportValues.Item("ZJ5PS_Retiree") = ZJ5PS_Retiree
                    drPRAReportValues.Item("ZJ5PS_Survivor") = ZJ5PS_Survivor
                    drPRAReportValues.Item("ZJ5S_Retiree") = ZJ5S_Retiree
                    drPRAReportValues.Item("ZJ5S_Survivor") = ZJ5S_Survivor
                    drPRAReportValues.Item("ZJ7_Retiree") = ZJ7_Retiree
                    drPRAReportValues.Item("ZJ7_Survivor") = ZJ7_Survivor
                    drPRAReportValues.Item("ZJ762_Retiree") = ZJ762_Retiree
                    drPRAReportValues.Item("ZJ762_Survivor") = ZJ762_Survivor
                    drPRAReportValues.Item("ZJ7I_Retiree") = ZJ7I_Retiree
                    drPRAReportValues.Item("ZJ7I_Survivor") = ZJ7I_Survivor
                    drPRAReportValues.Item("ZJ7L_Retiree") = ZJ7L_Retiree
                    drPRAReportValues.Item("ZJ7L_Survivor") = ZJ7L_Survivor
                    drPRAReportValues.Item("zJ7L62_Retiree") = zJ7L62_Retiree
                    drPRAReportValues.Item("ZJ7L62_Survivor") = ZJ7L62_Survivor
                    drPRAReportValues.Item("ZJ7LS_Retiree") = ZJ7LS_Retiree
                    drPRAReportValues.Item("ZJ7LS_Survivor") = ZJ7LS_Survivor
                    drPRAReportValues.Item("ZJ7P_Retiree") = ZJ7P_Retiree
                    drPRAReportValues.Item("zJ7P_Survivor") = zJ7P_Survivor
                    drPRAReportValues.Item("ZJ7P62_Retiree") = ZJ7P62_Retiree
                    drPRAReportValues.Item("zJ7P62_Survivor") = zJ7P62_Survivor
                    drPRAReportValues.Item("ZJ7PS_Retiree") = ZJ7PS_Retiree
                    drPRAReportValues.Item("zJ7PS_Survivor") = zJ7PS_Survivor
                    drPRAReportValues.Item("ZJ7S_Retiree") = ZJ7S_Retiree
                    drPRAReportValues.Item("ZJ7S_Survivor") = ZJ7S_Survivor
                    drPRAReportValues.Item("ZM_Retiree") = ZM_Retiree
                    drPRAReportValues.Item("ZM62_Retiree") = ZM62_Retiree
                    drPRAReportValues.Item("ZMI_Retiree") = ZMI_Retiree
                    drPRAReportValues.Item("ZMS_Retiree") = ZMS_Retiree
                    'Ashish:2010.06.21 YRs 5.0-1115
                    drPRAReportValues.Item("FundNo") = Me.FundNo

                    dtPRAReportParameters.Rows.Add(drPRAReportValues)
                    dtPRAReportParameters.GetChanges(DataRowState.Added)
                    dtPRAReportParameters.AcceptChanges()
                    dsPRAReportValues.Tables.Add(dtPRAReportParameters)
                    dsPRAReportValues.AcceptChanges()

                    Dim strGUID As String

                    strGUID = RetirementBOClass.InsertPRA_ReportValues(Me.PersonId, AgeRetired, _
                                 BenBirthDate, _
                                 BeneficiaryName, _
                                 PRAAssumption, _
                                 RetBirthDate, _
                                 RetiredDate, _
                                 RetiredDeathBen, _
                                 C_Insured, _
                                 C_Monthly, _
                                 C_Reduction, _
                                 C62_Insured, _
                                 C62_Monthly, _
                                 C62_Reduction, _
                                 CS_Insured, _
                                 CS_Monthly, _
                                 CS_Reduction, _
                                 J1_Retiree, _
                                 J1_Survivor, _
                                 J162_Retiree, _
                                 J162_Survivor, _
                                 J1I_Retiree, _
                                 J1I_Survivor, _
                                 J1P_Retiree, _
                                 J1P_Survivor, _
                                 J1P62_Retiree, _
                                 J1P62_Survivor, _
                                 J1PS_Retiree, _
                                 J1PS_Survivor, _
                                 J1S_Retiree, _
                                 J1S_Survivor, _
                                 J5_Retiree, _
                                 J5_Survivor, _
                                 J562_Retiree, _
                                 J562_Survivor, _
                                 J5I_Retiree, _
                                 J5I_Survivor, _
                                 J5L_Retiree, _
                                 J5L_Survivor, _
                                 J5L62_Retiree, _
                                 J5L62_Survivor, _
                                 J5LS_Retiree, _
                                 J5LS_Survivor, _
                                 J5P_Retiree, _
                                 J5P_Survivor, _
                                 J5P62_Retiree, _
                                 J5P62_Survivor, _
                                 J5PS_Retiree, _
                                 J5PS_Survivor, _
                                 J5S_Retiree, _
                                 J5S_Survivor, _
                                 J7_Retiree, _
                                 J7_Survivor, _
                                 J762_Retiree, _
                                 J762_Survivor, _
                                 J7I_Retiree, _
                                 J7I_Survivor, _
                                 J7L_Retiree, _
                                 J7L_Survivor, _
                                 J7L62_Retiree, _
                                 J7L62_Survivor, _
                                 J7LS_Retiree, _
                                 J7LS_Survivor, _
                                 J7P_Retiree, _
                                 J7P_Survivor, _
                                 J7P62_Retiree, _
                                 J7P62_Survivor, _
                                 J7PS_Retiree, _
                                 J7PS_Survivor, _
                                 J7S_Retiree, _
                                 J7S_Survivor, _
                                 M_Retiree, _
                                 M62_Retiree, _
                                 MI_Retiree, _
                                 MS_Retiree, _
                                 ZC_Annually, _
                                 ZC62_Annually, _
                                 ZCS_Annually, _
                                 ZJ1_Retiree, _
                                 ZJ1_Survivor, _
                                 ZJ162_Retiree, _
                                 ZJ162_Survivor, _
                                 ZJ1I_Retiree, _
                                 ZJ1I_Survivor, _
                                 ZJ1P_Retiree, _
                                 ZJ1P_Survivor, _
                                 ZJ1P62_Retiree, _
                                 ZJ1P62_Survivor, _
                                 ZJ1PS_Retiree, _
                                 ZJ1PS_Survivor, _
                                 ZJ1S_Retiree, _
                                 ZJ1S_Survivor, _
                                 ZJ5_Retiree, _
                                 ZJ5_Survivor, _
                                 ZJ562_Retiree, _
                                 ZJ562_Survivor, _
                                 ZJ5I_Retiree, _
                                 ZJ5I_Survivor, _
                                 ZJ5L_Retiree, _
                                 ZJ5L_Survivor, _
                                 ZJ5L62_Retiree, _
                                 ZJ5L62_Survivor, _
                                 ZJ5LS_Retiree, _
                                 ZJ5LS_Survivor, _
                                 ZJ5P_Retiree, _
                                 ZJ5P_Survivor, _
                                 ZJ5P62_Retiree, _
                                 zJ5P62_Survivor, _
                                 ZJ5PS_Retiree, _
                                 ZJ5PS_Survivor, _
                                 ZJ5S_Retiree, _
                                 ZJ5S_Survivor, _
                                 ZJ7_Retiree, _
                                 ZJ7_Survivor, _
                                 ZJ762_Retiree, _
                                 ZJ762_Survivor, _
                                 ZJ7I_Retiree, _
                                 ZJ7I_Survivor, _
                                 ZJ7L_Retiree, _
                                 ZJ7L_Survivor, _
                                 zJ7L62_Retiree, _
                                 ZJ7L62_Survivor, _
                                 ZJ7LS_Retiree, _
                                 ZJ7LS_Survivor, _
                                 ZJ7P_Retiree, _
                                 zJ7P_Survivor, _
                                 ZJ7P62_Retiree, _
                                 zJ7P62_Survivor, _
                                 ZJ7PS_Retiree, _
                                 zJ7PS_Survivor, _
                                 ZJ7S_Retiree, _
                                 ZJ7S_Survivor, _
                                 ZM_Retiree, _
                                 ZM62_Retiree, _
                                 ZMI_Retiree, _
                                 ZMS_Retiree, _
                                 Convert.ToDecimal(TextboxFromBenefitValue.Text), _
                                 Convert.ToDecimal(TextboxSSIncrease.Text), _
                                 Convert.ToDecimal(TextboxAmountToUse.Text), _
                  Convert.ToDecimal(DropdownlistPercentageToUse.SelectedValue), Me.FundNo _
                  , Convert.ToDecimal(TextBoxProjFinalYrsSalary.Text.Trim()) _
                    )

                    'Ashish:2010.06.21 YRS 5.01115 ,Start
                    'Session("StrReportName") = "ProspectiveRetirementAllowance"
                    Select Case paraPrintType
                        Case "PRACOLORFULL"
                            Session("StrReportName") = "ANNTYESTLONG"
                            'Start-- Manthan Rajguru | 2016.01.14 | YRS-AT-2151 | Commented the existing code 
                            'Case "PRADRAFTSHORT"
                            '    Session("StrReportName") = "ANNTYESTSHORT"
                            'Case "PRACOLORSHORT"
                            '    Session("StrReportName") = "ANNTYESTCOLOR"
                            'End-- Manthan Rajguru | 2016.01.14 | YRS-AT-2151 | Commented the existing code
                            'IB:BT:892-YRS 5.0-1359 : Disability Estimate form
                        Case "PRADISABL"
                            Session("StrReportName") = "DisabilityAnnuityEst"
                        Case "PRAALTPAYEE"
                            Session("StrReportName") = "AltPayeeAnnuityEst"
                        Case "PRADRAFTFULL"
                            Session("StrReportName") = System.Configuration.ConfigurationSettings.AppSettings("RET_EST_DraftFullForm")
                    End Select

                    'Ashish:2010.06.21 YRS 5.01115 ,End
                    Session("GUID") = strGUID '''"ECA1CD78-AAB9-4701-BEEA-CA4A3252BA6C".Trim

                    'Start Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request
                    'To Send a copy of estimate to IDM
                    If chkIDM.Checked Then
                        l_StringReportName = Session("StrReportName")
                        l_stringDocType = "ANNTYEST"
                        l_ArrListParamValues.Add(CType(strGUID, String).ToString.Trim)
                        l_string_OutputFileType = "AnnuityEsitmate_" & l_stringDocType
                        l_StringErrorMessage = CopyToIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)
                        If l_StringErrorMessage <> "" Then
                            MessageBox.Show(PlaceHolder1, "IDM Copy Error", l_StringErrorMessage, MessageBoxButtons.Stop)
                        End If
                    End If
                    'End Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request

                    ''strGUID
                    'Dim popupScript As String = "<script language='javascript'>" & _
                    '        "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
                    '        "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                    '        "</script>"
                    'If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                    '    Page.RegisterStartupScript("PopupScript1", popupScript)
                    'End If
                    Dim popupScript As String = String.Empty
                    'If print type is Long or short type then call ReportViewer.aspx
                    'Start-- Manthan Rajguru | 2016.01.14 | YRS-AT-2151 | Commented the existing code and removed paraPrintType = "PRADRAFTSHORT"
                    'If paraPrintType = "PRADRAFTFULL" Or paraPrintType = "PRADRAFTSHORT" Or paraPrintType = "PRACOLORFULL" Then 'B.Jagadeesh BT:2816 YRS 5.0-2495 - Added [PRACOLORFULL] in below condition
                    If paraPrintType = "PRADRAFTFULL" Or paraPrintType = "PRACOLORFULL" Then 'B.Jagadeesh BT:2816 YRS 5.0-2495 - Added [PRACOLORFULL] in below condition
                        'End-- Manthan Rajguru | 2016.01.14 | YRS-AT-2151 | Commented the existing code and removed paraPrintType = "PRADRAFTSHORT"
                        popupScript = "<script language='javascript'>openReportViewer()</script>"
                        If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                            Page.RegisterStartupScript("PopupScript1", popupScript)
                        End If
                        'If print type is color type then call ReportPrinter.aspx
                        'YRS 5.0-1345 :BT-859
                        'IB:BT:892-YRS 5.0-1359 : Disability Estimate form
                        'Start-- Manthan Rajguru | 2016.01.14 | YRS-AT-2151 | Commented the existing code and removed paraPrintType = "PRACOLORSHORT"
                        'ElseIf paraPrintType = "PRACOLORSHORT" Or paraPrintType = "PRADISABL" Or paraPrintType = "PRAALTPAYEE" Then 'B.Jagadeesh BT:2816 YRS 5.0-2495 - Removed [PRACOLORFULL] from below condition
                    ElseIf paraPrintType = "PRADISABL" Or paraPrintType = "PRAALTPAYEE" Then 'B.Jagadeesh BT:2816 YRS 5.0-2495 - Removed [PRACOLORFULL] from below condition
                        'End-- Manthan Rajguru | 2016.01.14 | YRS-AT-2151 | Commented the existing code and removed paraPrintType = "PRACOLORSHORT"
                        popupScript = "<script language='javascript'>openReportPrinter()</script>"
                        If (Not Me.IsStartupScriptRegistered("PopupPrintColor")) Then
                            Page.RegisterStartupScript("PopupPrintColor", popupScript)
                        End If

                    End If 'print type if

                End If
            End If
        Catch
            Throw
        End Try


    End Function
    '#End Region

#Region "Other Custom Methods"
    Private Sub ClearSession()
        Try
            Me.RetireType = Nothing
            Me.SSNO = Nothing
            Me.FundEventId = Nothing
            Me.PersonId = Nothing
            Me.PlanType = Nothing
            Me.FundEventStatus = Nothing

            Session("PercentageSelected") = Nothing
            Session("Print") = Nothing

            Session("mnyDeathBenefitAmount") = Nothing
            Session("Page") = Nothing
            Session("StrReportName") = Nothing
            Session("GUID") = Nothing

            Session("DefaultEmpEventID") = Nothing
            Session("dsBasicAccounts") = Nothing
            Session("dsElectiveAccountsDet") = Nothing
            Session("EarliestStartWorkDate") = Nothing
            Session("EmpHistoryInfoNotset") = Nothing
            Session("employmentSalaryInformation") = Nothing
            Session("LatestStartWorkDate") = Nothing
            Session("NoActivePlans") = Nothing
            Session("ProceedWithEstimation") = Nothing
            Session("ProceedWithJSOptionEstimationQDRO") = Nothing 'Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivo
            Session("RE_NoBasicAccountContribution") = Nothing
            Session("selectedEmployment") = Nothing
            Session("UsePlan") = Nothing

            'Phase IV - Start
            Session("dsPersonEmploymentDetails") = Nothing
            Session("dsElectiveAccounts") = Nothing
            Session("dsParticipantBeneficiaries") = Nothing
            Session("businessLogic") = Nothing
            'ASHISH:2011.08.24 Added new property for YRS 5.0-1135
            'BT-798 : System should not allow disability retirement for QD and BF fundevents
            Session("OrgBenTypeIsQDROorRBEN") = Nothing
            Session("RE_RetirementDate") = Nothing
            'Phase IV - End
            'added by ashish for phase V changes ,satrt
            Session("isYmcaLegacyAcctTotalExceed") = Nothing
            Session("isYmcaAcctTotalExceed") = Nothing
            'Ashish:2010.06.21 YRS 5.0-1115 ,Start
            Session("FundNo") = Nothing
            Session("RE_PRAType") = Nothing
            'Ashish:2010.06.21 YRS 5.0-1115 ,End
            'ASHISH:2010.11.16:Added  for YRS 5.0-1215
            Session("RP_ExactAgeEffDate") = Nothing
            'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
            Session("AreJAnnuityUnavailable") = Nothing
            Session("InputBeneficiaryType") = Nothing
            'To keep the value of Retiree Birth date present or not
            Session("RetireeBirthDatePresent") = Nothing
            'SP : BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary
            Session("dsSelectedParticipantBeneficiary") = Nothing
            'Anudeep A:2013.02.08-Bt-1688: YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
            Session("dataset_RelationShips") = Nothing
            Me.ManualTransactionDetails = Nothing 'MMR | 2017.03.03 | YRS-AT-2625 | Clearing session property
        Catch
            Throw
        End Try
    End Sub
    Private Sub ShowCustomMessage(ByVal pstrMessage As String, ByVal pMessageBoxType As enumMessageBoxType)
        Dim alertWindow As String
        Try
            If pMessageBoxType = enumMessageBoxType.Javascript Then
                alertWindow = "<script language='javascript'>" & _
                            "alert('" & pstrMessage & "');" & _
                            "</script>"
                If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                    Page.RegisterStartupScript("alertWindow", alertWindow)
                End If
            ElseIf pMessageBoxType = enumMessageBoxType.DotNet Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", pstrMessage, MessageBoxButtons.OK, True)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub ShowCustomMessage(ByVal pstrMessage As String, ByVal pMessageBoxType As enumMessageBoxType, ByVal msgBoxButtons As MessageBoxButtons)
        Dim alertWindow As String
        Try
            If pMessageBoxType = enumMessageBoxType.Javascript Then
                alertWindow = "<script language='javascript'>" & _
                            "alert('" & pstrMessage & "');" & _
                            "</script>"
                If (Not Me.IsStartupScriptRegistered("alertWindow")) Then
                    Page.RegisterStartupScript("alertWindow", alertWindow)
                End If
            ElseIf pMessageBoxType = enumMessageBoxType.DotNet Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", pstrMessage, msgBoxButtons, True)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub SetRetireType()

        Dim lcRetireType As String
        Try
            If DropDownListRetirementType.SelectedItem.ToString.Trim.ToUpper() <> "" Then
                lcRetireType = DropDownListRetirementType.SelectedItem.ToString.Trim.ToUpper()
                If lcRetireType = "DISABLED" Then
                    lcRetireType = "DISABL"
                Else
                    lcRetireType = "NORMAL"
                End If

                Me.RetireType = lcRetireType
            Else
                If Me.RetireType <> "" Then
                    If Me.RetireType = "NORMAL" Then
                        DropDownListRetirementType.SelectedValue = "NORMAL"
                    ElseIf Me.RetireType = "DISABL" Then
                        DropDownListRetirementType.SelectedValue = "DISABLED"
                    End If
                Else
                    Me.RetireType = "NORMAL"
                    DropDownListRetirementType.SelectedValue = "NORMAL"
                End If
            End If
        Catch
            Throw
        End Try


    End Sub
    Private Sub populateElectiveAccountsTab(ByVal empEventId As String, ByVal loadNonVoluntaryAccounts As Boolean, ByVal boolPlanTypeChanged As Boolean)
        Dim selectedPlan As String
        Dim filterExpression As String = String.Empty
        Dim i As Integer
        'Commented by Ashish for phase V changes
        'Dim dtExcludedAccounts As DataTable
        Dim l_boolAccountExcluded As Boolean = False

        Dim di As DataGridItem
        Dim contributionType As String
        Dim amt As String
        Dim startDate As String
        Dim stopDate As String
        Try
            'Commented by Ashish for phase V changes
            'updateAccountChanges()
            'Added by Ashish For Phase V changes

            'START:Commented by Chandra sekar for YRS-AT:2752
            'dsElectiveAccountsDet = GetElectiveAccoutsByPlan(Me.FundEventId, Me.PlanType, Me.RetirementDate)

            'Session("dsElectiveAccountsDet") = dsElectiveAccountsDet
            'END:Commented by Chandra sekar for YRS-AT:2752
            'START:Chandra sekar.c  2016.02.22  YRS-AT-2752 - Retirement estimate not calculating projected reserves when estimation is performed for Both plan.
            If Session("dsElectiveAccountsDet") Is Nothing Then
                dsElectiveAccountsDet = GetElectiveAccoutsByPlan(Me.FundEventId, Me.PlanType, Me.RetirementDate)
                Session("dsElectiveAccountsDet") = dsElectiveAccountsDet
            Else
                dsElectiveAccountsDet = Session("dsElectiveAccountsDet")
            End If
            'END:Chandra sekar.c  2016.02.22  YRS-AT-2752 - Retirement estimate not calculating projected reserves when estimation is performed for Both plan.
            UpdateElectiveAccountAssumptions(String.Empty)

            selectedPlan = Me.DropDownListPlanType.SelectedValue.ToUpper()
            'Commented by Ashish for phase V changes ,start
            'If boolPlanTypeChanged = False Then
            '    dtExcludedAccounts = getExcludedAccounts()
            'Else
            '    dtExcludedAccounts = New DataTable
            'End If
            'Commented by Ashish for phase V changes ,End

            If DropDownListEmployment.Items.Count <= 1 Then
                'filterExpression = "(AcctTotal > 0)"
                filterExpression = "(YMCATotal > 0 OR PersonalTotal > 0 )"
            Else
                If empEventId = "" Then
                    'filterExpression = "(AcctTotal > 0 AND bitRet_Voluntary = 0)"
                    filterExpression = "((YMCATotal > 0 OR PersonalTotal > 0 OR bitFutureAcctVisible=1 ) AND bitRet_Voluntary = 0)"

                Else
                    'filterExpression = "(AcctTotal > 0 OR bitRet_Voluntary = 1)" ' AND guiEmpEventID = '" & empEventId & "'"
                    filterExpression = "((YMCATotal > 0 OR PersonalTotal > 0) OR bitRet_Voluntary = 1 OR bitFutureAcctVisible=1)" ' AND guiEmpEventID = '" & empEventId & "'"
                End If
            End If

            Dim dsBasicAccounts As DataSet = Session("dsElectiveAccountsDet")
            '04-March-2009 Priya Remove code from selectedPlan = "SAVINGS" Or selectedPlan = "BOTH" and 
            'Put it here because this code should applicable for Retirement ,savings and both plan type.
            If Page.IsPostBack = False Then ' index is set to default only for the first time
                If DropDownListEmployment.Items.Count > 1 Then
                    DropDownListEmployment.SelectedIndex = 1 ' index is set to default only for the first time
                End If
            End If
            'End 04-March-2009
            If selectedPlan = "RETIREMENT" Or selectedPlan = "BOTH" Then
                If loadNonVoluntaryAccounts = True Then
                    'Dim dsBasicAccounts As DataSet = Session("dsBasicAccounts")'Phase IV Changes
                    Dim dvRet As DataView
                    'Commented by Ashish  For Phase V changes, Start
                    'If selectedPlan = "BOTH" Then

                    '    dvRet = New DataView(dsBasicAccounts.Tables(0), _
                    '                              "PlanType='RETIREMENT' AND " & filterExpression, _
                    '                              "bitBasicAcct desc, chrAcctType", _
                    '                              DataViewRowState.CurrentRows)
                    'Else
                    '    dvRet = New DataView(dsBasicAccounts.Tables(0), _
                    '                                    "PlanType = '" & selectedPlan & "' AND " & filterExpression, _
                    '                                    "bitBasicAcct desc, chrAcctType", _
                    '                                    DataViewRowState.CurrentRows)
                    'End If
                    'Commented by Ashish 15-Apr-2009 For Phase V changes, End

                    'Added by Ashish 15-Apr-2009 For Phase V changes, Start
                    If selectedPlan = "BOTH" Then

                        dvRet = New DataView(dsBasicAccounts.Tables("RetireeGroupedElectiveAccounts"), _
                                                  "PlanType='RETIREMENT' AND " & filterExpression, _
                                                  "intSortOrder asc, chrAcctType", _
                                                  DataViewRowState.CurrentRows)
                    Else
                        dvRet = New DataView(dsBasicAccounts.Tables("RetireeGroupedElectiveAccounts"), _
                                                        "PlanType = '" & selectedPlan & "' AND " & filterExpression, _
                                                        "intSortOrder asc, chrAcctType", _
                                                        DataViewRowState.CurrentRows)
                    End If
                    'Added by Ashish 15-Apr-2009 For Phase V changes, End
                    DatagridElectiveRetirementAccounts.AutoGenerateColumns = False
                    DatagridElectiveRetirementAccounts.DataSource = dvRet
                    DatagridElectiveRetirementAccounts.DataBind()

                    'NP:2008.06.13:YRS-5.0-457 - RetirementPlan Checkbox is not required
                    'If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
                    '    CheckBoxRetirementPlan.Checked = True
                    'ElseIf DatagridElectiveRetirementAccounts.Items.Count = 0 Then
                    '    CheckBoxRetirementPlan.Checked = False
                    'End If

                    'CheckAccounts()
                    'If Unchecked, uncheck all the retirement plan account
                    'Ashish for testing Start
                    '    Dim IsBasicAccountExcluded As Boolean = False
                    '    If DatagridElectiveRetirementAccounts.Visible = True Then
                    '        If dtExcludedAccounts.Select("bitBasicAcct='True'").Length > 0 Then
                    '            IsBasicAccountExcluded = True
                    '        End If

                    '        For i = 0 To DatagridElectiveRetirementAccounts.Items.Count - 1
                    '            Dim chk As CheckBox
                    '            chk = DatagridElectiveRetirementAccounts.Items(i).Cells(0).Controls(1)
                    '            l_boolAccountExcluded = False

                    '            'checking if the account is excluded
                    '            If dtExcludedAccounts.Rows.Count > 0 Then
                    '                If dtExcludedAccounts.Select("chrAcctType='" & DatagridElectiveRetirementAccounts.Items(i).Cells(1).Text.ToString().Trim() & "'").Length > 0 Then
                    '                    l_boolAccountExcluded = True
                    '                End If
                    '            End If

                    '            di = DatagridElectiveRetirementAccounts.Items(i)

                    '            contributionType = CType(di.FindControl("DropdownlistContribTypeRet"), DropDownList).SelectedValue
                    '            amt = CType(di.FindControl("TextboxContribAmtRet"), TextBox).Text
                    '            If amt <> "" Then
                    '                amt = Convert.ToDecimal(amt)
                    '            End If

                    '            startDate = CType(di.FindControl("DateusercontrolStartRet"), DateUserControl).Text.Trim()
                    '            stopDate = CType(di.FindControl("DateusercontrolStopRet"), DateUserControl).Text.Trim()

                    '            'if the account is not excluded then showing the account as selected.
                    '            If IsBasicAccountExcluded = True Then
                    '                chk.Checked = False
                    '            ElseIf l_boolAccountExcluded = False And Convert.ToDecimal(DatagridElectiveRetirementAccounts.Items(i).Cells(2).Text.ToString().Trim()) > 0 Then
                    '                'NP:2008.06.15:YRS-5.0-457 - Selecting all accounts on changing plan type
                    '                chk.Checked = True 'dvRet.Item(i).Row("Selected") 'CheckBoxRetirementPlan.Checked
                    '            ElseIf contributionType <> "" Or amt > 0 Or startDate <> "" Or stopDate <> "" Then
                    '                chk.Checked = True
                    '            Else 'if the account is excluded then showing the account as de-selected.
                    '                chk.Checked = False
                    '            End If
                    '        Next
                    '    End If
                    'Ashish for testing End
                    'Added by Ashish for phase V changes ,Start
                    If IsPostBack = False Then
                        Dim l_chkSelect As CheckBox
                        If Not DatagridElectiveRetirementAccounts Is Nothing Then
                            If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
                                For Each dgRetireRow As DataGridItem In Me.DatagridElectiveRetirementAccounts.Items
                                    l_chkSelect = TryCast(dgRetireRow.Cells(RET_SEL_ACCT_CHK).FindControl("CheckboxRet"), CheckBox)
                                    If Not l_chkSelect Is Nothing Then
                                        If Convert.ToDecimal(dgRetireRow.Cells(RET_ACCT_TOTAL).Text.Trim()) > 0 Then
                                            l_chkSelect.Checked = True
                                        Else
                                            If Convert.ToBoolean(dgRetireRow.Cells(RET_BASIC_ACCT).Text.Trim()) = True Then
                                                l_chkSelect.Checked = True
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    End If
                    'Added by Ashish for phase V changes ,End

                End If

            End If

            l_boolAccountExcluded = False

            If selectedPlan = "SAVINGS" Or selectedPlan = "BOTH" Then
                Dim dvSav As DataView
                'dsElectiveAccountsDet = Session("dsElectiveAccountsDet")
                'Commented by Ashish  For Phase V changes, Start
                'If selectedPlan = "BOTH" Then
                '    dvSav = New DataView(dsElectiveAccountsDet.Tables(0), _
                '                                    "PlanType = 'SAVINGS' AND " & filterExpression, _
                '                                    "chrAcctType", _
                '                                    DataViewRowState.CurrentRows)
                'Else
                '    dvSav = New DataView(dsElectiveAccountsDet.Tables(0), _
                '                     "PlanType = '" & selectedPlan & "' AND " & filterExpression, _
                '                    "chrAcctType", _
                '                    DataViewRowState.CurrentRows)
                'End If
                'Commented by Ashish For Phase V changes, End
                'Added by Ashish  For Phase V changes, Start
                If selectedPlan = "BOTH" Then
                    dvSav = New DataView(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts"), _
                                                    "PlanType = 'SAVINGS' AND " & filterExpression, _
                                                    "chrAcctType", _
                                                    DataViewRowState.CurrentRows)
                Else
                    dvSav = New DataView(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts"), _
                                     "PlanType = '" & selectedPlan & "' AND " & filterExpression, _
                                    "chrAcctType", _
                                    DataViewRowState.CurrentRows)
                End If

                'Added  by Ashish  For Phase V changes, End

                '' If no active employment is present then disable the 
                '' Voluntary account grid and display existing account contribution as read only
                'If DropDownListEmployment.Items.Count <= 1 Then
                '    empEventId = Session("DefaultEmpEventID")

                '    If selectedPlan = "BOTH" Then
                '        dvSav = New DataView(dsElectiveAccountsDet.Tables(0), _
                '                                        "PlanType = 'SAVINGS' AND guiEmpEventID = '" & empEventId & "' AND AcctTotal > 0", _
                '                                        "chrAcctType", _
                '                                        DataViewRowState.CurrentRows)
                '    Else
                '        dvSav = New DataView(dsElectiveAccountsDet.Tables(0), _
                '                                         "PlanType = '" & selectedPlan & "' AND guiEmpEventID = '" & empEventId & "' AND AcctTotal > 0", _
                '                                        "chrAcctType", _
                '                                        DataViewRowState.CurrentRows)
                '    End If
                'ElseIf DropDownListEmployment.Items.Count > 1 Then
                '    If Page.IsPostBack = False Then ' index is set to default only for the first time
                '        DropDownListEmployment.SelectedIndex = 1
                '    End If
                'End If

                '04-March-2009 Priya Commented code Put it out side if because this code should applicable for Retirement ,savings and both plan type.
                'If Page.IsPostBack = False Then ' index is set to default only for the first time
                '    If DropDownListEmployment.Items.Count > 1 Then
                '        DropDownListEmployment.SelectedIndex = 1 ' index is set to default only for the first time
                '    End If
                'End If
                'End 04-March-2009

                DatagridElectiveSavingsAccounts.AutoGenerateColumns = False
                DatagridElectiveSavingsAccounts.DataSource = dvSav
                DatagridElectiveSavingsAccounts.DataBind()
                'Commented by Ashish  For Phase V changes, Start
                ''If Unchecked, uncheck all the Savings account
                'For i = 0 To DatagridElectiveSavingsAccounts.Items.Count - 1
                '    Dim chk As CheckBox
                '    chk = DatagridElectiveSavingsAccounts.Items(i).Cells(SAV_SEL_ACCT_CHK).Controls(1)
                '    l_boolAccountExcluded = False

                '    If dtExcludedAccounts.Rows.Count > 0 Then
                '        If dtExcludedAccounts.Select("chrAcctType='" & DatagridElectiveSavingsAccounts.Items(i).Cells(SAV_ACCT_TYPE).Text.ToString().Trim() & "'").Length > 0 Then
                '            l_boolAccountExcluded = True
                '        End If
                '    End If

                '    If l_boolAccountExcluded = False And Convert.ToDecimal(DatagridElectiveSavingsAccounts.Items(i).Cells(SAV_ACCT_TOTAL).Text.ToString().Trim()) > 0 Then
                '        'NP:2008.06.15:YRS-5.0-457 - Selecting all accounts on changing plan type
                '        chk.Checked = True 'dvSav.Item(i).Row("Selected") 'CheckBoxRetirementPlan.Checked
                '    Else
                '        chk.Checked = False
                '    End If
                'Next
                'Commented by Ashish For Phase V changes, End
                'Added by Ashish for phase V changes ,Start
                If IsPostBack = False Then
                    Dim l_chkSelect As CheckBox
                    If Not DatagridElectiveSavingsAccounts Is Nothing Then
                        If DatagridElectiveSavingsAccounts.Items.Count > 0 Then
                            For Each dgSavingRow As DataGridItem In Me.DatagridElectiveSavingsAccounts.Items
                                l_chkSelect = TryCast(dgSavingRow.Cells(SAV_SEL_ACCT_CHK).FindControl("CheckboxSav"), CheckBox)
                                If Not l_chkSelect Is Nothing Then
                                    If Convert.ToDecimal(dgSavingRow.Cells(SAV_ACCT_TOTAL).Text.Trim()) > 0 Then
                                        l_chkSelect.Checked = True
                                    End If
                                    'Dinesh Kanojia         2015.04.27      BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
                                    If dgSavingRow.Cells(1).Text.Trim.ToLower = "ln" Or dgSavingRow.Cells(1).Text.Trim.ToLower = "ld" Then
                                        l_chkSelect.Enabled = True
                                        l_chkSelect.Checked = False
                                        l_chkSelect.Enabled = False
                                    End If

                                End If
                            Next
                        End If
                    End If
                End If
                'Added by Ashish for phase V changes ,End
            End If

            setAccountGridControls()

            'Me.disableElectiveAccounts(Me.PlanType)
            'Added by Ashish for phase V changes
            'ValidateProjectedBalancesAsPerRefund()
            'ShowProjectedAcctBalancesTotal()
        Catch
            Throw
        End Try
    End Sub
    Private Sub setAccountGridControls()
        Try
            Dim selectedPlan As String = Me.DropDownListPlanType.SelectedValue.ToUpper()
            Dim i As Integer
            Dim l_UC_DatePicker As YMCAUI.DateUserControl
            Dim l_DropDownList As DropDownList
            Dim l_TextBox As TextBox
            Dim l_CheckBox As CheckBox
            Dim l_string_AcctTotal As String
            Dim l_string_AcctType As String
            'Added by Ashish for phase V changes
            Dim l_bool_bitBasicAcct As Boolean
            Dim l_bool_bitVoluntaryAcct As Boolean
            '16-Feb-2011 Added by Sanket for disabiling controls when value in drop down is changed
            'If Me.RetireType.Trim.ToUpper() = "NORMAL" Then
            If selectedPlan = "RETIREMENT" Or selectedPlan = "BOTH" Then
                If DropDownListEmployment.Items.Count <= 1 Or IsRetirementBackDated() = True Or (Me.FundEventStatus = "RT" Or Me.FundEventStatus = "RPT") Or Me.RetireType.Trim.ToUpper() = "DISABL" Then
                    If DatagridElectiveRetirementAccounts.Columns.Count >= 9 Then
                        DatagridElectiveRetirementAccounts.Columns(RET_CONTRI_TYPE).Visible = False
                        DatagridElectiveRetirementAccounts.Columns(RET_CONTRIBUTION).Visible = False
                        DatagridElectiveRetirementAccounts.Columns(RET_START_DATE).Visible = False
                        DatagridElectiveRetirementAccounts.Columns(RET_STOP_DATE).Visible = False

                        For i = 0 To Me.DatagridElectiveRetirementAccounts.Items.Count - 1

                            l_DropDownList = DatagridElectiveRetirementAccounts.Items(i).FindControl("DropdownlistContribTypeRet")
                            If Not l_DropDownList Is Nothing Then
                                l_DropDownList.SelectedValue = ""
                            End If

                            l_TextBox = DatagridElectiveRetirementAccounts.Items(i).FindControl("TextboxContribAmt")
                            If Not l_TextBox Is Nothing Then
                                l_TextBox.Text = ""
                            End If

                            l_UC_DatePicker = DatagridElectiveRetirementAccounts.Items(i).FindControl("DateusercontrolStartRet")
                            If Not l_UC_DatePicker Is Nothing Then
                                l_UC_DatePicker.Text = ""
                            End If

                            l_UC_DatePicker = DatagridElectiveRetirementAccounts.Items(i).FindControl("DateusercontrolStopRet")
                            If Not l_UC_DatePicker Is Nothing Then
                                l_UC_DatePicker.Text = ""
                            End If
                            'Commented by Ashish for phase v changes,start
                            'l_string_AcctTotal = DatagridElectiveRetirementAccounts.Items(i).Cells(RET_ACCT_TOTAL).Text.ToString().Trim()
                            'If l_string_AcctTotal = String.Empty Or l_string_AcctTotal = "&nbsp;" Or l_string_AcctTotal = "0" Then
                            '    l_CheckBox = DatagridElectiveRetirementAccounts.Items(i).FindControl("CheckboxRet")
                            '    l_CheckBox.Checked = False
                            'End If
                            'Commented by Ashish for phase v changes,End
                            l_bool_bitBasicAcct = Convert.ToBoolean(DatagridElectiveRetirementAccounts.Items(i).Cells(RET_BASIC_ACCT).Text.ToString().Trim())
                            l_string_AcctTotal = DatagridElectiveRetirementAccounts.Items(i).Cells(RET_ACCT_TOTAL).Text.ToString().Trim()
                            l_bool_bitVoluntaryAcct = Convert.ToBoolean(DatagridElectiveRetirementAccounts.Items(i).Cells(RET_VOL_ACCT).Text.ToString().Trim())
                            If l_string_AcctTotal = String.Empty Or l_string_AcctTotal = "&nbsp;" Or l_string_AcctTotal = "0" Then
                                l_CheckBox = DatagridElectiveRetirementAccounts.Items(i).FindControl("CheckboxRet")
                                If l_bool_bitBasicAcct = False Or l_bool_bitVoluntaryAcct = True Then
                                    l_CheckBox.Checked = False
                                End If

                            End If
                        Next

                    End If
                Else
                    If DatagridElectiveRetirementAccounts.Columns.Count >= 9 Then
                        DatagridElectiveRetirementAccounts.Columns(RET_CONTRI_TYPE).Visible = True
                        DatagridElectiveRetirementAccounts.Columns(RET_CONTRIBUTION).Visible = True
                        DatagridElectiveRetirementAccounts.Columns(RET_START_DATE).Visible = True
                        DatagridElectiveRetirementAccounts.Columns(RET_STOP_DATE).Visible = True
                    End If
                End If
            End If
            If selectedPlan = "SAVINGS" Or selectedPlan = "BOTH" Then
                If DropDownListEmployment.Items.Count <= 1 Or IsRetirementBackDated() = True Or (Me.FundEventStatus = "RT" Or Me.FundEventStatus = "RPT") Or Me.RetireType.Trim.ToUpper() = "DISABL" Then
                    If DatagridElectiveSavingsAccounts.Columns.Count >= 9 Then
                        DatagridElectiveSavingsAccounts.Columns(SAV_CONTRI_TYPE).Visible = False
                        DatagridElectiveSavingsAccounts.Columns(SAV_CONTRIBUTION).Visible = False
                        DatagridElectiveSavingsAccounts.Columns(SAV_START_DATE).Visible = False
                        DatagridElectiveSavingsAccounts.Columns(SAV_STOP_DATE).Visible = False

                        For i = 0 To Me.DatagridElectiveSavingsAccounts.Items.Count - 1

                            l_string_AcctType = DatagridElectiveSavingsAccounts.Items(i).Cells(SAV_ACCT_TYPE).Text.ToString().Trim()
                            l_DropDownList = DatagridElectiveSavingsAccounts.Items(i).FindControl("DropdownlistContribTypeSav")
                            If Not l_DropDownList Is Nothing And l_string_AcctType <> "RT" Then
                                l_DropDownList.SelectedValue = ""
                            End If

                            l_TextBox = DatagridElectiveSavingsAccounts.Items(i).FindControl("TextboxContribAmtSav")
                            If Not l_TextBox Is Nothing Then
                                l_TextBox.Text = ""
                            End If

                            l_UC_DatePicker = DatagridElectiveSavingsAccounts.Items(i).FindControl("DateusercontrolStartSav")
                            If Not l_UC_DatePicker Is Nothing Then
                                l_UC_DatePicker.Text = ""
                            End If

                            l_UC_DatePicker = DatagridElectiveSavingsAccounts.Items(i).FindControl("DateusercontrolStopSav")
                            If Not l_UC_DatePicker Is Nothing Then
                                l_UC_DatePicker.Text = ""
                            End If

                            l_string_AcctTotal = DatagridElectiveSavingsAccounts.Items(i).Cells(SAV_ACCT_TOTAL).Text.ToString().Trim()
                            If (l_string_AcctTotal = String.Empty Or l_string_AcctTotal = "&nbsp;" Or l_string_AcctTotal = "0") Then
                                l_CheckBox = DatagridElectiveSavingsAccounts.Items(i).FindControl("CheckboxSav")
                                l_CheckBox.Checked = False
                            End If
                        Next
                    End If
                Else
                    If DatagridElectiveSavingsAccounts.Columns.Count >= 9 Then
                        DatagridElectiveSavingsAccounts.Columns(SAV_CONTRI_TYPE).Visible = True
                        DatagridElectiveSavingsAccounts.Columns(SAV_CONTRIBUTION).Visible = True
                        DatagridElectiveSavingsAccounts.Columns(SAV_START_DATE).Visible = True
                        DatagridElectiveSavingsAccounts.Columns(SAV_STOP_DATE).Visible = True
                    End If
                End If
            End If

            'ElseIf Me.RetireType.Trim.ToUpper() = "DISABL" Then
            ''For Retiement accouns
            'DatagridElectiveRetirementAccounts.Columns(RET_CONTRI_TYPE).Visible = False
            'DatagridElectiveRetirementAccounts.Columns(RET_CONTRIBUTION).Visible = False
            'DatagridElectiveRetirementAccounts.Columns(RET_START_DATE).Visible = False
            'DatagridElectiveRetirementAccounts.Columns(RET_STOP_DATE).Visible = False
            ''For saving accouns
            'DatagridElectiveSavingsAccounts.Columns(SAV_CONTRI_TYPE).Visible = False
            'DatagridElectiveSavingsAccounts.Columns(SAV_CONTRIBUTION).Visible = False
            'DatagridElectiveSavingsAccounts.Columns(SAV_START_DATE).Visible = False
            'DatagridElectiveSavingsAccounts.Columns(SAV_STOP_DATE).Visible = False
            'End If
        Catch
            Throw
        End Try
    End Sub
#Region "Commented code for Phase V"
    'Commented by Ashish for Phase V changes, two method ClearElectiveAccountAssumptions,updateAccountChanges has same logic 
    ''so merge into one method UpdateElectiveAccountAssumptions
    ''NP:2008.06.17:YRS-5.0-457 - Code to clear any assumptions that have been stored in Session for a particular plan
    ''Expected Input Parameters: R = Retirement Plan
    ''                           S = Savings Plan
    ''                           B = Both Plans
    'Private Sub ClearElectiveAccountAssumptions(ByVal planType As String)
    '    Dim filterExpression As String
    '    Try
    '        dsElectiveAccountsDet = Session("dsElectiveAccountsDet")

    '        Dim selectedRow As DataRow
    '        Dim contributionType As String = String.Empty
    '        Dim amt As String = String.Empty
    '        Dim startDate As String = String.Empty
    '        Dim stopDate As String = String.Empty
    '        Dim newEmpEventID As String = String.Empty
    '        Dim empEventID As String
    '        Dim accountType As String
    '        Dim selected As Boolean = False
    '        Dim dsPersonEmploymentDetails As DataSet
    '        Dim drs() As DataRow
    '        'Added by Ashish 15-Apr-2009 Phase V changes
    '        Dim bool_bitBasicAcct As Boolean

    '        ' Retirement Grid 
    '        If planType = "R" Or planType = "B" Then
    '            If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
    '                For Each di As DataGridItem In DatagridElectiveRetirementAccounts.Items
    '                    ' Fetch values from dataGridRow
    '                    empEventID = di.Cells(RET_EMP_EVENT_ID).Text.ToUpper() 'Phase IV Changes
    '                    accountType = di.Cells(RET_ACCT_TYPE).Text.ToUpper()
    '                    'Added by Ashish 15-Apr-2009 for Phase changes ,Start
    '                    bool_bitBasicAcct = di.Cells(RET_BASIC_ACCT).Text
    '                    'Added by Ashish 15-Apr-2009 for Phase changes ,End
    '                    If bool_bitBasicAcct = False Then

    '                        dsPersonEmploymentDetails = Session("dsPersonEmploymentDetails")
    '                        If dsPersonEmploymentDetails.Tables.Count > 0 Then
    '                            If dsPersonEmploymentDetails.Tables(0).Rows.Count > 0 Then
    '                                drs = dsPersonEmploymentDetails.Tables(0).Select("guiEmpEventId='" & empEventID & "'")
    '                                If drs.Length = 0 Then
    '                                    newEmpEventID = DropDownListEmployment.SelectedValue.ToString.Trim.ToUpper
    '                                End If
    '                            End If
    '                        End If
    '                        'commented by Ashish 15-Apr-2009 for Phase V changes
    '                        'drs = dsElectiveAccountsDet.Tables(0).Select("chrAcctType='" & accountType & "' AND guiEmpEventID='" & empEventID & "'")
    '                        drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select("chrAcctType='" & accountType & "' AND guiEmpEventID='" & empEventID & "'")
    '                        If drs.Length = 0 Then
    '                            'commented by Ashish 15-Apr-2009 for Phase V changes
    '                            'drs = dsElectiveAccountsDet.Tables(0).Select("chrAcctType='" & accountType & "' AND guiEmpEventID='" & newEmpEventID & "'")
    '                            drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select("chrAcctType='" & accountType & "' AND guiEmpEventID='" & newEmpEventID & "'")
    '                        End If
    '                        'Commented by Ashish for Phase V changes
    '                        'selectedRow = drs(0)
    '                        'Added by Ashish for Phase V changes ,Start
    '                        If Not drs Is Nothing AndAlso drs.Length > 0 Then 'Added by Ashish for Phase V changes 
    '                            selectedRow = drs(0)
    '                        End If
    '                        'Added by Ashish for Phase V changes,End
    '                        If Not selectedRow Is Nothing Then 'Added by Ashish for Phase V changes

    '                            '
    '                            If newEmpEventID <> String.Empty And newEmpEventID <> empEventID Then
    '                                selectedRow.Item("guiEmpEventID") = newEmpEventID
    '                            End If
    '                            selectedRow.Item("Selected") = selected
    '                            selectedRow.Item("chrAdjustmentBasisCode") = contributionType
    '                            If amt <> String.Empty Then
    '                                selectedRow.Item("mnyAddlContribution") = Convert.ToDecimal(amt)
    '                            Else
    '                                selectedRow.Item("mnyAddlContribution") = 0
    '                            End If
    '                            selectedRow.Item("dtmEffDate") = startDate
    '                            selectedRow.Item("dtsTerminationDate") = stopDate
    '                        End If 'Added by Ashish for Phase V changes
    '                    End If 'bool_bitBasicAcct = False
    '                Next
    '            End If
    '        End If

    '        ' Savings Grid
    '        If planType = "S" Or planType = "B" Then
    '            If DatagridElectiveSavingsAccounts.Items.Count > 0 Then
    '                For Each di As DataGridItem In DatagridElectiveSavingsAccounts.Items
    '                    ' Fetch values from dataGridRow
    '                    empEventID = di.Cells(SAV_EMP_EVENT_ID).Text.ToUpper()
    '                    accountType = di.Cells(SAV_ACCT_TYPE).Text.ToUpper()
    '                    'Added by Ashish 15-Apr-2009 for Phase changes ,Start
    '                    bool_bitBasicAcct = di.Cells(SAV_BASIC_ACCT).Text
    '                    'Added by Ashish 15-Apr-2009 for Phase changes ,End
    '                    If bool_bitBasicAcct = False Then
    '                        dsPersonEmploymentDetails = Session("dsPersonEmploymentDetails")
    '                        If dsPersonEmploymentDetails.Tables.Count > 0 Then
    '                            If dsPersonEmploymentDetails.Tables(0).Rows.Count > 0 Then
    '                                filterExpression = "guiEmpEventId='" & empEventID & "'"
    '                                drs = dsPersonEmploymentDetails.Tables(0).Select(filterExpression)
    '                                If drs.Length = 0 Then
    '                                    newEmpEventID = DropDownListEmployment.SelectedValue.ToString.Trim.ToUpper
    '                                End If
    '                            End If
    '                        End If
    '                        filterExpression = "chrAcctType='" & accountType & "' AND guiEmpEventID='" & empEventID.ToLower() & "'"
    '                        'commented by Ashish 15-Apr-2009 for Phase V changes
    '                        'drs = dsElectiveAccountsDet.Tables(0).Select(filterExpression)
    '                        drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select(filterExpression)
    '                        If drs.Length = 0 Then
    '                            filterExpression = "chrAcctType='" & accountType & "' AND guiEmpEventID='" & newEmpEventID.ToLower() & "'"
    '                            'commented by Ashish 15-Apr-2009 for Phase V changes
    '                            'drs = dsElectiveAccountsDet.Tables(0).Select(filterExpression)
    '                            drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select(filterExpression)
    '                        End If
    '                        'Commented by Ashish for Phase V changes
    '                        'selectedRow = drs(0)
    '                        'Added by Ashish for Phase V changes ,Start
    '                        If Not drs Is Nothing AndAlso drs.Length > 0 Then 'Added by Ashish for Phase V changes 
    '                            selectedRow = drs(0)
    '                        End If
    '                        'Added by Ashish for Phase V changes,End
    '                        If Not selectedRow Is Nothing Then
    '                            'Added by Ashish for Phase V changes
    '                            If newEmpEventID <> String.Empty And newEmpEventID <> empEventID Then
    '                                selectedRow.Item("guiEmpEventID") = newEmpEventID
    '                            End If

    '                            selectedRow.Item("Selected") = selected
    '                            selectedRow.Item("chrAdjustmentBasisCode") = contributionType

    '                            If amt <> String.Empty Then
    '                                selectedRow.Item("mnyAddlContribution") = Convert.ToDecimal(amt)
    '                            Else
    '                                selectedRow.Item("mnyAddlContribution") = 0
    '                            End If

    '                            selectedRow.Item("dtmEffDate") = startDate
    '                            selectedRow.Item("dtsTerminationDate") = stopDate
    '                        End If
    '                    End If 'If bool_bitBasicAcct = False
    '                Next
    '            End If
    '        End If

    '        If dsElectiveAccountsDet.HasChanges Then
    '            dsElectiveAccountsDet.AcceptChanges()
    '            Session("dsElectiveAccountsDet") = dsElectiveAccountsDet
    '        End If
    '    Catch
    '        Throw
    '    End Try
    'End Sub
    'Private Sub updateAccountChanges()
    '    Dim filterExpression As String
    '    Try
    '        dsElectiveAccountsDet = Session("dsElectiveAccountsDet")

    '        Dim selectedRow As DataRow
    '        Dim contributionType As String
    '        Dim amt As String
    '        Dim startDate As String
    '        Dim stopDate As String
    '        Dim newEmpEventID As String = String.Empty
    '        Dim empEventID As String
    '        Dim accountType As String
    '        Dim selected As Boolean
    '        Dim dsPersonEmploymentDetails As DataSet
    '        Dim drs() As DataRow
    '        'Added By Ashish 15-Apr-2009 for Phase V changes
    '        Dim bool_bitBasicAcct As Boolean

    '        ' Retirement Grid 
    '        If Me.PlanType = "R" Or Me.PlanType = "B" Then
    '            If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
    '                For Each di As DataGridItem In DatagridElectiveRetirementAccounts.Items
    '                    ' Fetch values from dataGridRow
    '                    selected = CType(di.FindControl("CheckBoxRet"), CheckBox).Checked
    '                    contributionType = CType(di.FindControl("DropdownlistContribTypeRet"), DropDownList).SelectedValue
    '                    amt = CType(di.FindControl("TextboxContribAmtRet"), TextBox).Text
    '                    startDate = CType(di.FindControl("DateusercontrolStartRet"), DateUserControl).Text.Trim()
    '                    stopDate = CType(di.FindControl("DateusercontrolStopRet"), DateUserControl).Text.Trim()
    '                    'empEventID = di.Cells(6).Text.ToUpper()'Phase IV Changes

    '                    empEventID = di.Cells(RET_EMP_EVENT_ID).Text.ToUpper()  'Phase IV Changes

    '                    accountType = di.Cells(RET_ACCT_TYPE).Text.ToUpper()
    '                    'Added by Ashish 15-Apr-2009
    '                    bool_bitBasicAcct = di.Cells(RET_BASIC_ACCT).Text
    '                    'Added by Ashish 15-Apr-2009
    '                    If bool_bitBasicAcct = False Then

    '                        dsPersonEmploymentDetails = Session("dsPersonEmploymentDetails")
    '                        If Not dsPersonEmploymentDetails Is Nothing Then
    '                            If dsPersonEmploymentDetails.Tables.Count > 0 Then
    '                                If dsPersonEmploymentDetails.Tables(0).Rows.Count > 0 Then
    '                                    drs = dsPersonEmploymentDetails.Tables(0).Select("guiEmpEventId='" & empEventID & "'")
    '                                    If drs.Length = 0 Then
    '                                        newEmpEventID = DropDownListEmployment.SelectedValue.ToString.Trim.ToUpper
    '                                    End If
    '                                End If
    '                            End If
    '                            'Commented by Ashish 15-Apr-2009 for Phase V changes
    '                            'drs = dsElectiveAccountsDet.Tables(0).Select("chrAcctType='" & accountType & "' AND guiEmpEventID='" & empEventID & "'")
    '                            drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select("chrAcctType='" & accountType & "' AND guiEmpEventID='" & empEventID & "'")
    '                            If drs.Length = 0 Then
    '                                'Commented by Ashish 15-Apr-2009 for Phase V changes
    '                                'drs = dsElectiveAccountsDet.Tables(0).Select("chrAcctType='" & accountType & "' AND guiEmpEventID='" & newEmpEventID & "'")
    '                                drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select("chrAcctType='" & accountType & "' AND guiEmpEventID='" & newEmpEventID & "'")
    '                            End If
    '                            'Commented by Ashish 16-Apr-2009 for phase V changes
    '                            'selectedRow = drs(0)
    '                            'Added by Ashish 16-Feb-2009 for Phase V changes
    '                            If Not drs Is Nothing AndAlso drs.Length > 0 Then

    '                                selectedRow = drs(0)
    '                            End If
    '                            If Not selectedRow Is Nothing Then 'Added by Ashish 16-Feb-2009 for Phase V changes

    '                                If newEmpEventID <> String.Empty And newEmpEventID <> empEventID Then
    '                                    selectedRow.Item("guiEmpEventID") = newEmpEventID
    '                                End If

    '                                selectedRow.Item("Selected") = selected
    '                                selectedRow.Item("chrAdjustmentBasisCode") = contributionType

    '                                If amt <> String.Empty Then
    '                                    selectedRow.Item("mnyAddlContribution") = Convert.ToDecimal(amt)
    '                                Else
    '                                    selectedRow.Item("mnyAddlContribution") = 0
    '                                End If

    '                                If startDate <> String.Empty Then
    '                                    selectedRow.Item("dtmEffDate") = startDate
    '                                Else
    '                                    selectedRow.Item("dtmEffDate") = ""
    '                                End If

    '                                'If stopDate <> String.Empty Then
    '                                selectedRow.Item("dtsTerminationDate") = stopDate
    '                                'End If
    '                            End If
    '                        End If 'Added by Ashish 16-Feb-2009 for Phase V changes

    '                    End If 'bool_bitBasicAcct 
    '                Next
    '            End If
    '        End If

    '        ' Savings Grid
    '        If Me.PlanType = "S" Or Me.PlanType = "B" Then
    '            If DatagridElectiveSavingsAccounts.Items.Count > 0 Then
    '                For Each di As DataGridItem In DatagridElectiveSavingsAccounts.Items
    '                    ' Fetch values from dataGridRow
    '                    selected = CType(di.FindControl("CheckBoxSav"), CheckBox).Checked
    '                    contributionType = CType(di.FindControl("DropdownlistContribTypeSav"), DropDownList).SelectedValue
    '                    amt = CType(di.FindControl("TextboxContribAmtSav"), TextBox).Text
    '                    startDate = CType(di.FindControl("DateusercontrolStartSav"), DateUserControl).Text.Trim()
    '                    stopDate = CType(di.FindControl("DateusercontrolStopSav"), DateUserControl).Text.Trim()
    '                    empEventID = di.Cells(9).Text.ToUpper()
    '                    accountType = di.Cells(1).Text.ToUpper()
    '                    'Added by Ashish 15-Apr-2009
    '                    bool_bitBasicAcct = di.Cells(SAV_BASIC_ACCT).Text
    '                    'Added by Ashish 15-Apr-2009
    '                    If bool_bitBasicAcct = False Then
    '                        dsPersonEmploymentDetails = Session("dsPersonEmploymentDetails")
    '                        If Not dsPersonEmploymentDetails Is Nothing Then
    '                            If dsPersonEmploymentDetails.Tables.Count > 0 Then
    '                                If dsPersonEmploymentDetails.Tables(0).Rows.Count > 0 Then
    '                                    filterExpression = "guiEmpEventId='" & empEventID & "'"
    '                                    drs = dsPersonEmploymentDetails.Tables(0).Select(filterExpression)
    '                                    If drs.Length = 0 Then
    '                                        newEmpEventID = DropDownListEmployment.SelectedValue.ToString.Trim.ToUpper
    '                                    End If
    '                                End If
    '                            End If
    '                            filterExpression = "chrAcctType='" & accountType & "' AND guiEmpEventID='" & empEventID.ToLower() & "'"
    '                            'Commented by Ashish 15-Apr-2009 for Phase V changes
    '                            'drs = dsElectiveAccountsDet.Tables(0).Select(filterExpression)
    '                            drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select(filterExpression)
    '                            If drs.Length = 0 Then
    '                                filterExpression = "chrAcctType='" & accountType & "' AND guiEmpEventID='" & newEmpEventID.ToLower() & "'"
    '                                'Commented by Ashish 15-Apr-2009 for Phase V changes
    '                                'drs = dsElectiveAccountsDet.Tables(0).Select(filterExpression)
    '                                drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select(filterExpression)
    '                            End If
    '                            'Commented by Ashish 16-Apr-2009 for phase V changes
    '                            'selectedRow = drs(0)
    '                            'Added by Ashish 16-Feb-2009 for Phase V changes
    '                            If Not drs Is Nothing AndAlso drs.Length > 0 Then

    '                                selectedRow = drs(0)
    '                            End If
    '                            If Not selectedRow Is Nothing Then 'Added by Ashish 16-Feb-2009 for Phase V changes
    '                                If newEmpEventID <> String.Empty And newEmpEventID <> empEventID Then
    '                                    selectedRow.Item("guiEmpEventID") = newEmpEventID
    '                                End If

    '                                selectedRow.Item("Selected") = selected
    '                                selectedRow.Item("chrAdjustmentBasisCode") = contributionType

    '                                If amt <> String.Empty Then
    '                                    selectedRow.Item("mnyAddlContribution") = Convert.ToDecimal(amt)
    '                                Else
    '                                    selectedRow.Item("mnyAddlContribution") = 0
    '                                End If

    '                                If startDate <> String.Empty Then
    '                                    selectedRow.Item("dtmEffDate") = startDate
    '                                Else
    '                                    selectedRow.Item("dtmEffDate") = ""
    '                                End If
    '                                'Commented by Anil      YRPS - 4536
    '                                'If stopDate <> String.Empty Then
    '                                selectedRow.Item("dtsTerminationDate") = stopDate
    '                                'End If
    '                                'Commented by Anil      YRPS - 4536
    '                            End If
    '                        End If
    '                    End If 'bool_bitBasicAcct 'Added by Ashish 15-Apr-2009
    '                Next
    '            End If
    '        End If

    '        If dsElectiveAccountsDet.HasChanges Then
    '            dsElectiveAccountsDet.AcceptChanges()
    '            Session("dsElectiveAccountsDet") = dsElectiveAccountsDet
    '        End If
    '    Catch
    '        Throw
    '    End Try
    'End Sub
#End Region
    Sub ValidateElectiveAccounts(ByVal source As Object, ByVal args As ServerValidateEventArgs)
        Try
            'commented by Anudeep on 22-sep for BT-1126
            'Dim errorMessage As String = "Invalid Data in Accounts Tab"//commented by Anudeep on 22-sep for BT-1126

            'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - replaced the bullet appearing in the validation summary control with a hyphen for simplicity and more control.
            Dim errorMessage As String = "- " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_INVALID_DATA")
            Dim isValid As Boolean = True
            Dim contributionType As String
            Dim amt As String
            Dim amount As Decimal
            Dim startDate As String
            Dim startDateTime As DateTime
            Dim stopDate As String
            Dim stopDateTime As DateTime
            ' If the person is not having any employment records. eg QDRO participant
            If Session("EarliestStartWorkDate") = Nothing Then
                args.IsValid = True
                Exit Sub
            End If
            Dim earliestStartWorkDate As DateTime = Convert.ToDateTime(Session("EarliestStartWorkDate").ToString().Trim())
            Dim retDate As DateTime = Convert.ToDateTime(TextBoxRetirementDate.Text)
            Dim accountType As String

            'YRS 5.0-478
            Dim EmploymentDetails As DataSet
            Dim EmploymentRecords As DataRow()

            EmploymentDetails = Session("dsPersonEmploymentDetails")
            If DropDownListEmployment.Items.Count > 0 Then
                EmploymentRecords = EmploymentDetails.Tables(0).Select("guiEmpEventId='" & DropDownListEmployment.SelectedValue.Trim() & "'")
                If EmploymentRecords.Length > 0 Then
                    earliestStartWorkDate = EmploymentRecords(0)("Start")
                End If
            End If
            'YRS 5.0-478
            LabelPartialWithdrawal.Text = ""



            Dim selected As Boolean
            'NP:IVP1:2008.05.07 - Adding validation checks for the Retirement plan also.
            '   This is required since the new enhancements allow a user to specify contributions to their non-existing Retirement Plan accounts.
            'Retirement Plan Grid
            If DatagridElectiveRetirementAccounts.Visible = True And DatagridElectiveRetirementAccounts.Items.Count > 0 Then
                For Each di As DataGridItem In DatagridElectiveRetirementAccounts.Items
                    ' Fetch values from dataGridRow
                    selected = CType(di.FindControl("CheckboxRet"), CheckBox).Checked
                    contributionType = CType(di.FindControl("DropdownlistContribTypeRet"), DropDownList).SelectedValue
                    amt = CType(di.FindControl("TextboxContribAmtRet"), TextBox).Text
                    startDate = CType(di.FindControl("DateusercontrolStartRet"), DateUserControl).Text.Trim()
                    stopDate = CType(di.FindControl("DateusercontrolStopRet"), DateUserControl).Text.Trim()
                    accountType = di.Cells(1).Text.ToUpper()

                    If selected = True Then
                        If Not String.IsNullOrEmpty(contributionType) Then
                            If ((accountType <> "RT") And ((amt <> String.Empty And amt <> "0") Or (contributionType <> String.Empty) Or (startDate <> String.Empty))) _
                                Or (accountType = "RT" And ((amt <> String.Empty And amt <> "0") Or startDate <> String.Empty)) Then
                                'If (accountType = "RT" And ((amt <> String.Empty And amt <> "0") Or startDate <> String.Empty)) Then
                                'Check for valid currency value
                                If Not Regex.IsMatch(amt, RetirementBOClass.REG_EX_CURRENCY) And amt <> String.Empty Then
                                    isValid = False
                                    'commented by Anudeep on 22-sep for BT-1126
                                    'errorMessage = errorMessage + "<br> Account: " + accountType + " - Contribution should be in Currency Format"//commented by Anudeep on 22-sep for BT-1126
                                    errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_CONTRIBUTION_CURRENCY_FORMAT")
                                End If

                                'Check for valid values
                                ' Contribution %age should be in the range of 1 - 100
                                If contributionType = "P" Then
                                    amount = Convert.ToDecimal(amt)
                                    If amount < 0 Or amount > 100 Then
                                        isValid = False
                                        'commented by Anudeep on 22-sep for BT-1126
                                        'errorMessage = errorMessage + "<br> Account: " + accountType + " - Contribution should be in the range of 0 - 100"//commented by Anudeep on 22-sep for BT-1126
                                        errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_CONTRIBUTION_RANGE")
                                    End If
                                End If

                                If startDate <> String.Empty Then
                                    startDateTime = Convert.ToDateTime(startDate)
                                    ' Start date is prior to earliestStartWorkDate
                                    If startDateTime < earliestStartWorkDate Then
                                        isValid = False

                                        'errorMessage = errorMessage + "<br> Account: " + accountType + " - Start date cannot be earlier than the first hiredate"//commented by Anudeep on 22-sep for BT-1126
                                        errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_START_DATE_CANNOT_BE_FIRST_HIREDATE")
                                    End If

                                    ' Start date is beyond retirement Date
                                    If startDateTime > retDate Then
                                        isValid = False
                                        'errorMessage = errorMessage + "<br> Account: " + accountType + " - Start date cannot be after the retirement date"//commented by Anudeep on 22-sep for BT-1126
                                        errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_CANNOT_BE_AFTER_RETDATE")
                                    End If

                                    'NP:IVP1:2008.05.08 - BT-417 - Check to prevent back dated contribution start dates
                                    If startDateTime < DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("MM/dd/yyyy") And contributionType <> "L" Then
                                        isValid = False
                                        'errorMessage = errorMessage + "<br> Account: " + accountType + " - Start date cannot be earlier than the first of this month"//commented by Anudeep on 22-sep for BT-1126
                                        errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_EARLIER_THAN_FIRST_OF_MONTH")
                                    End If
                                    'Added by Ashish 27-Jul-2009 , added validation for RT account, RT account start date can not less than next month of current month
                                    If contributionType = "L" Then
                                        If startDateTime < DateTime.Now.AddDays(-DateTime.Now.Day + 1).AddMonths(1).ToString("MM/dd/yyyy") Then
                                            isValid = False
                                            'errorMessage = errorMessage + "<br> Account: " + accountType + " - Start date for lumpsum contribution cannot be within current month. Please use a date on or after " & DateTime.Now.AddDays(-DateTime.Now.Day + 1).AddMonths(1).ToString("MM/dd/yyyy") & "."
                                            'commented by Anudeep on 22-sep for BT-1126
                                            errorMessage = errorMessage + "<br> Account: " + accountType + String.Format(getmessage("MESSAGE_RETIREMENT_ESTIMATE_START_DATE_FOR_LUMPSUM"), DateTime.Now.AddDays(-DateTime.Now.Day + 1).AddMonths(1).ToString("MM/dd/yyyy")) + "."
                                        End If
                                    End If
                                End If

                                If stopDate <> String.Empty Then
                                    stopDateTime = Convert.ToDateTime(stopDate)
                                    ' Start date is after stop date
                                    If startDateTime > stopDateTime Then
                                        isValid = False
                                        'commented by Anudeep on 22-sep for BT-1126
                                        'errorMessage = errorMessage + "<br> Account: " + accountType + " - Stop date has to be after Start Date"
                                        errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_STOP_DATE_AFTER_START_DATE")
                                    End If
                                    ' Stop date is after retirement date
                                    If stopDateTime > retDate Then
                                        isValid = False
                                        'commented by Anudeep on 22-sep for BT-1126
                                        'errorMessage = errorMessage + "<br> Account: " + accountType + " - Stop date cannot be beyond the Retirement Date"
                                        errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_STOP_DATE_RETIREMENT_DATE")
                                    End If
                                End If

                            End If
                            'Start : Code Commented by Dinesh Kanojia on 11/07/2013
                            '' If the plan is selected
                            'If CType(di.FindControl("CheckBoxRet"), CheckBox).Checked Then
                            '    If contributionType = "" Then
                            '        isValid = False
                            '        'commented by Anudeep on 22-sep for BT-1126
                            '        'errorMessage = errorMessage + "<br> Account: " + accountType + " - Please select a valid contribution type"
                            '        errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_SELECT_VALID_CONTRIBUTION")
                            '    End If
                            If Not String.IsNullOrEmpty(contributionType) Then
                                If amt = "0" Or amt = String.Empty Then
                                    isValid = False
                                    'commented by Anudeep on 22-sep for BT-1126
                                    'errorMessage = errorMessage + "<br> Account: " + accountType + " - Please enter some contributions"
                                    errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_ENTER_CONTRIBUTIONS")
                                End If

                                If startDate = String.Empty Then
                                    isValid = False
                                    'commented by Anudeep on 22-sep for BT-1126
                                    'errorMessage = errorMessage + "<br> Account: " + accountType + " - Start date cannot be blank"
                                    errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_STARTDATE_CANNOT_BLANK")
                                End If
                            End If

                            'End If
                            'End : Code Commented by Dinesh Kanojia on 11/07/2013

                        End If ' If account contribution data entered
                    End If ' If selected 
                Next
            End If

            ' Savings Grid
            If DatagridElectiveSavingsAccounts.Visible = True And DatagridElectiveSavingsAccounts.Items.Count > 0 Then
                For Each di As DataGridItem In DatagridElectiveSavingsAccounts.Items
                    ' Fetch values from dataGridRow
                    selected = CType(di.FindControl("CheckboxSav"), CheckBox).Checked
                    contributionType = CType(di.FindControl("DropdownlistContribTypeSav"), DropDownList).SelectedValue
                    amt = CType(di.FindControl("TextboxContribAmtSav"), TextBox).Text
                    startDate = CType(di.FindControl("DateusercontrolStartSav"), DateUserControl).Text.Trim()
                    stopDate = CType(di.FindControl("DateusercontrolStopSav"), DateUserControl).Text.Trim()
                    accountType = di.Cells(1).Text.ToUpper()

                    If selected = True Then
                        If ((accountType <> "RT") And ((amt <> String.Empty And amt <> "0") Or (contributionType <> String.Empty) Or (startDate <> String.Empty))) _
                            Or (accountType = "RT" And ((amt <> String.Empty And amt <> "0") Or startDate <> String.Empty)) Then
                            'If (accountType = "RT" And ((amt <> String.Empty And amt <> "0") Or startDate <> String.Empty)) Then
                            'Check for valid currency value
                            If Not Regex.IsMatch(amt, RetirementBOClass.REG_EX_CURRENCY) And amt <> String.Empty Then
                                isValid = False
                                'commented by Anudeep on 22-sep for BT-1126
                                'errorMessage = errorMessage + "<br> Account: " + accountType + " - Contribution should be in Currency Format"
                                errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_CONTRIBUTION_CURRENCY_FORMAT")
                            End If

                            'Check for valid values
                            ' Contribution %age should be in the range of 1 - 100
                            If contributionType = "P" Then
                                amount = Convert.ToDecimal(amt)
                                If amount < 0 Or amount > 100 Then
                                    isValid = False
                                    'commented by Anudeep on 22-sep for BT-1126
                                    'errorMessage = errorMessage + "<br> Account: " + accountType + " - Contribution should be in the range of 0 - 100"
                                    errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_CONTRIBUTION_RANGE")
                                End If
                            End If

                            If startDate <> String.Empty Then
                                startDateTime = Convert.ToDateTime(startDate)
                                ' Start date is prior to earliestStartWorkDate
                                If startDateTime < earliestStartWorkDate Then
                                    isValid = False
                                    'commented by Anudeep on 22-sep for BT-1126
                                    'errorMessage = errorMessage + "<br> Account: " + accountType + " - Start date cannot be earlier than the first hiredate"
                                    errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_START_DATE_CANNOT_BE_FIRST_HIREDATE")
                                End If

                                ' Start date is beyond retirement Date
                                If startDateTime > retDate Then
                                    isValid = False
                                    'commented by Anudeep on 22-sep for BT-1126
                                    'errorMessage = errorMessage + "<br> Account: " + accountType + " - Start date cannot be after the retirement date"
                                    errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_CANNOT_BE_AFTER_RETDATE")
                                End If

                                'NP:IVP1:2008.05.08 - BT-417 - Check to prevent back dated contribution start dates
                                If startDateTime < DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("MM/dd/yyyy") And contributionType <> "L" Then
                                    isValid = False
                                    'commented by Anudeep on 22-sep for BT-1126
                                    'errorMessage = errorMessage + "<br> Account: " + accountType + " - Start date cannot be earlier than the first of this month"
                                    errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_EARLIER_THAN_FIRST_OF_MONTH")
                                End If

                                'Added by Ashish 27-Jul-2009 , added validation for RT account, RT account start date can not less than next month of current month
                                If contributionType = "L" Then
                                    If startDateTime < DateTime.Now.AddDays(-DateTime.Now.Day + 1).AddMonths(1).ToString("MM/dd/yyyy") Then
                                        isValid = False
                                        'commented by Anudeep on 22-sep for BT-1126
                                        'errorMessage = errorMessage + "<br> Account: " + accountType + " - Start date for lumpsum contribution cannot be within current month. Please use a date on or after " & DateTime.Now.AddDays(-DateTime.Now.Day + 1).AddMonths(1).ToString("MM/dd/yyyy") & "."
                                        errorMessage = errorMessage + "<br> Account: " + accountType + String.Format(getmessage("MESSAGE_RETIREMENT_ESTIMATE_START_DATE_FOR_LUMPSUM"), DateTime.Now.AddDays(-DateTime.Now.Day + 1).AddMonths(1).ToString("MM/dd/yyyy")) + "."
                                    End If
                                End If
                            End If

                            If stopDate <> String.Empty Then
                                stopDateTime = Convert.ToDateTime(stopDate)
                                ' Start date is after stop date
                                If startDateTime > stopDateTime Then
                                    isValid = False
                                    'commented by Anudeep on 22-sep for BT-1126
                                    'errorMessage = errorMessage + "<br> Account: " + accountType + " - Stop date has to be after Start Date"
                                    errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_STOP_DATE_AFTER_START_DATE")
                                End If
                                ' Stop date is after retirement date
                                If stopDateTime > retDate Then
                                    isValid = False
                                    'commented by Anudeep on 22-sep for BT-1126
                                    'errorMessage = errorMessage + "<br> Account: " + accountType + " - Stop date cannot be beyond the Retirement Date"
                                    errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_STOP_DATE_RETIREMENT_DATE")
                                End If
                            End If


                            'Start : Code Commneted by Dinesh On 11/07/2013
                            '' If the plan is selected
                            'If CType(di.FindControl("CheckBoxSav"), CheckBox).Checked Then
                            '    If contributionType = "" Then
                            '        isValid = False
                            '        'commented by Anudeep on 22-sep for BT-1126
                            '        'errorMessage = errorMessage + "<br> Account: " + accountType + " - Please select a valid contribution type"
                            '        errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_SELECT_VALID_CONTRIBUTION")
                            '    End If
                            'End : Code Commneted by Dinesh On 11/07/2013
                            If Not String.IsNullOrEmpty(contributionType) And accountType <> "RT" Then
                                If amt = "0" Or amt = String.Empty Then
                                    isValid = False
                                    'commented by Anudeep on 22-sep for BT-1126
                                    'errorMessage = errorMessage + "<br> Account: " + accountType + " - Please enter some contributions"
                                    errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_ENTER_CONTRIBUTIONS")
                                End If

                                If startDate = String.Empty Then
                                    isValid = False
                                    'commented by Anudeep on 22-sep for BT-1126
                                    'errorMessage = errorMessage + "<br> Account: " + accountType + " - Start date cannot be blank"
                                    errorMessage = errorMessage + "<br> Account: " + accountType + getmessage("MESSAGE_RETIREMENT_ESTIMATE_STARTDATE_CANNOT_BLANK")
                                End If
                            End If
                        End If
                    End If ' If account contribution data entered
                Next
            End If

            If Not isValid Then
                CustomvalidatorDataGridElectiveAccounts.ErrorMessage = errorMessage
                'Added by Ashish 23-Jul-2009
                tabStripRetirementEstimate.SelectedIndex = 3
                Me.MultiPageRetirementEstimate.SelectedIndex = 3 'Me.tabStripRetirementEstimate.SelectedIndex
            End If
            args.IsValid = isValid

        Catch ex As Exception
            'NP:IVP1:2008.05.07 - Adding informative message in case there was a problem with performing validations
            'commented by Anudeep on 22-sep for BT-1126
            'CustomvalidatorDataGridElectiveAccounts.ErrorMessage = "<br> There was a problem with validating the Elective Accounts information"
            CustomvalidatorDataGridElectiveAccounts.ErrorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_VALIDATING_ELECTIVE_ACCOUNTS")
            args.IsValid = False
        End Try

        'args.IsValid = True
    End Sub
    Sub ValidateFutureSalaryDate(ByVal source As Object, ByVal args As ServerValidateEventArgs)
        Dim dateIsValid As Boolean = True
        'Dim errorMessage As String = "Invalid Employment Details"'commented by Anudeep on 22-sep for BT-1126
        'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Refactoring is done for the corresponding custom control has to appear * symbol
        Dim ValidationSummaryHeaderText As String = "- " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_INVALID_EMPLOYEMENT") + "<br>"
        Dim errorMessage As String = ""
        CustomValidatorFutureSalaryDate.ErrorMessage = ""
        CustomValidatorAnnualSalaryIncreaseEffDate.ErrorMessage = ""
        CustomValidatorEndWorkDate.ErrorMessage = ""
        CustomValidatorDDLAnnualSalaryIncreaseEffDate.ErrorMessage = ""  'Shilpa N | 04/08/2019 | YRS-AT-3392 | Added new custom validator to validate annual salary effective date empty or blank
        'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Refactoring is done Setting the Heading to the validate Summary of the Employment Tab 
        Dim IsHeadingAlreadySetInValidationSummaryControl As Boolean = False
        Try
            'Modified Salary Format
            If TextBoxModifiedSal.Text <> String.Empty Then
                If Not Regex.IsMatch(TextBoxModifiedSal.Text, RetirementBOClass.REG_EX_CURRENCY) Then
                    dateIsValid = False

                    'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Refactoring is done for the corresponding custom control has to appear * symbol
                    'Checking whether heading of the Validation Summary Control is set or not
                    If IsHeadingAlreadySetInValidationSummaryControl = False Then
                        IsHeadingAlreadySetInValidationSummaryControl = True 'once Heading is set then this boolean value is set as false i.e it will not set again.
                        errorMessage = ValidationSummaryHeaderText + getmessage("MESSAGE_RETIREMENT_ESTIMATE_INVALID_CURRENCY_FORMAT")
                    Else
                        errorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_INVALID_CURRENCY_FORMAT")
                    End If
                    'errorMessage = errorMessage + "<br> Modified Salary : Invalid currency format"'commented by Anudeep on 22-sep for BT-1126

                    'Commented by CS: 12/14/2015 YRS-AT-2329 : Refactoring is done for the corresponding custom control to display * symbol besides respective date control in the Employment Tab.
                    'errorMessage = errorMessage + getmessage("MESSAGE_RETIREMENT_ESTIMATE_INVALID_CURRENCY_FORMAT")
                    CustomValidatorFutureSalaryDate.ErrorMessage = errorMessage
                End If
            End If

            'Future Salary Format
            If TextBoxFutureSalary.Text <> String.Empty Then
                If Not Regex.IsMatch(TextBoxFutureSalary.Text, RetirementBOClass.REG_EX_CURRENCY) Then
                    dateIsValid = False
                    'errorMessage = errorMessage + "<br> Future Salary : Invalid currency format"'commented by Anudeep on 22-sep for BT-1126

                    'Commented by CS: 12/14/2015 YRS-AT-2329 : Refactoring is done for the corresponding custom control has to appear * symbol in the Employment Tab
                    'errorMessage = errorMessage + getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARY")
                    'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Refactoring is done for the corresponding custom control has to appear * symbol
                    'Checking whether heading of the Validation Summary Control is set or not
                    If IsHeadingAlreadySetInValidationSummaryControl = False Then
                        IsHeadingAlreadySetInValidationSummaryControl = True
                        errorMessage = ValidationSummaryHeaderText + getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARY")
                    Else
                        errorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARY")
                    End If
                    CustomValidatorFutureSalaryDate.ErrorMessage = errorMessage
                End If
            End If

            'Added by Ashish on 23-Jul-2009 Validate if defind future salary  then must be defind salary effective date
            If TextBoxFutureSalary.Text <> String.Empty AndAlso Convert.ToDecimal(TextBoxFutureSalary.Text) > 0 Then
                If TextBoxFutureSalaryEffDate.Text = String.Empty Then
                    dateIsValid = False
                    'commented by Anudeep on 22-sep for BT-1126
                    'errorMessage = errorMessage + "<br> Future Salary Date : Future Salary not used because Start Date is missing."

                    'Commented by CS: 12/14/2015 YRS-AT-2329 : Refactoring is done for the corresponding custom control has to appear * symbol in the Employment Tab
                    'errorMessage = errorMessage + getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARYDATE_MISSING")
                    'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Refactoring is done for the corresponding custom control has to appear * symbol
                    'Checking whether heading of the Validation Summary Control is set or not
                    If IsHeadingAlreadySetInValidationSummaryControl = False Then
                        IsHeadingAlreadySetInValidationSummaryControl = True
                        errorMessage = ValidationSummaryHeaderText + getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARYDATE_MISSING")
                    Else
                        errorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARYDATE_MISSING")
                    End If
                    CustomValidatorFutureSalaryDate.ErrorMessage = errorMessage
                End If
            End If

            'Validate Future Salary date
            If TextBoxFutureSalaryEffDate.Text <> String.Empty Then
                Dim futureSalaryDate As DateTime = Convert.ToDateTime(TextBoxFutureSalaryEffDate.Text)
                If futureSalaryDate <= DateTime.Today Then
                    dateIsValid = False
                    'commented by Anudeep on 22-sep for BT-1126
                    'errorMessage = errorMessage + "<br> Future Salary Date: Should be greater than today's Date"

                    'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Refactoring is done for the corresponding custom control has to appear * symbol
                    'Checking whether heading of the Validation Summary Control is set or not
                    If IsHeadingAlreadySetInValidationSummaryControl = False Then
                        IsHeadingAlreadySetInValidationSummaryControl = True
                        errorMessage = ValidationSummaryHeaderText + getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARY_DATE_GREATER_THAN_TODAYDATE")
                    Else
                        errorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARY_DATE_GREATER_THAN_TODAYDATE")
                    End If

                    'Commented by CS: 12/14/2015 YRS-AT-2329 : Refactoring is done for the corresponding custom control has to appear * symbol in the Employment Tab
                    'errorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARY_DATE_GREATER_THAN_TODAYDATE")
                    CustomValidatorFutureSalaryDate.ErrorMessage = errorMessage
                ElseIf futureSalaryDate > Convert.ToDateTime(TextBoxRetirementDate.Text) Then
                    dateIsValid = False
                    'commented by Anudeep on 22-sep for BT-1126
                    'errorMessage = errorMessage + "<br> Future Salary Date: Should be less than Retirement Date"

                    'Commented by CS: 12/14/2015 YRS-AT-2329 : Refactoring is done for the corresponding custom control has to appear * symbol in the Employment Tab
                    'errorMessage = errorMessage + getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARY_DATE_LESS_THAN_RETIREMENT")
                    'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Refactoring is done for the corresponding custom control has to appear * symbol
                    'Checking whether heading of the Validation Summary Control is set or not
                    If IsHeadingAlreadySetInValidationSummaryControl = False Then
                        IsHeadingAlreadySetInValidationSummaryControl = True
                        errorMessage = ValidationSummaryHeaderText + getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARY_DATE_LESS_THAN_RETIREMENT")
                    Else
                        errorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARY_DATE_LESS_THAN_RETIREMENT")
                    End If

                    CustomValidatorFutureSalaryDate.ErrorMessage = errorMessage
                End If
            End If


            'Validate EndWorkDate
            'Sanket vaidya          18 Apr 2011     BT-816 : Disability Retirement Estimate Issues.
            If (Me.RetireType = "NORMAL") Then
                If TextBoxEndWorkDate.Text <> String.Empty Then
                    Dim endWorkDate As DateTime = Convert.ToDateTime(TextBoxEndWorkDate.Text)
                    If endWorkDate > Convert.ToDateTime(TextBoxRetirementDate.Text) Then ' 2012.07.11 SP :BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page
                        dateIsValid = False
                        'commented by Anudeep on 22-sep for BT-1126
                        'errorMessage = errorMessage + "<br> End Work Date: Should be less than Retirement Date"

                        'Commented by CS: 12/14/2015 YRS-AT-2329 : Refactoring is done for the corresponding custom control has to appear * symbol in the Employment Tab
                        'errorMessage = errorMessage + getmessage("MESSAGE_RETIREMENT_ESTIMATE_END_WORK_DATE")
                        'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Refactoring is done for the corresponding custom control to display * symbol
                        'Checking whether heading of the Validation Summary Control is set or not
                        If IsHeadingAlreadySetInValidationSummaryControl = False Then
                            IsHeadingAlreadySetInValidationSummaryControl = True
                            errorMessage = ValidationSummaryHeaderText + getmessage("MESSAGE_RETIREMENT_ESTIMATE_END_WORK_DATE")
                        Else
                            errorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_END_WORK_DATE")
                        End If

                        CustomValidatorEndWorkDate.ErrorMessage = errorMessage
                    End If
                End If

                If (TextBoxFutureSalaryEffDate.Text <> String.Empty AndAlso TextBoxEndWorkDate.Text <> String.Empty _
                      AndAlso Convert.ToDateTime(TextBoxFutureSalaryEffDate.Text) > Convert.ToDateTime(TextBoxEndWorkDate.Text)) Then
                    dateIsValid = False
                    'commented by Anudeep on 22-sep for BT-1126
                    'errorMessage = errorMessage + "<br> Future Salary Date: Should be less than End Work Date"

                    'Commented by CS: 12/14/2015 YRS-AT-2329 : Refactoring is done for the corresponding custom control has to appear * symbol in the Employment Tab
                    'errorMessage = errorMessage + getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARY_DATE_END_WORK_DATE")  If IsHeadingSetEmploymentValidation = False Then
                    If IsHeadingAlreadySetInValidationSummaryControl = False Then
                        IsHeadingAlreadySetInValidationSummaryControl = True
                        errorMessage = ValidationSummaryHeaderText + getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARY_DATE_END_WORK_DATE")
                    Else
                        errorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_FUTURE_SALARY_DATE_END_WORK_DATE")
                    End If
                    CustomValidatorEndWorkDate.ErrorMessage = errorMessage
                    '    'Added by Ashish 23-Jul-2009
                End If
            End If

            If (Me.RetireType = "DISABL") Then
                Dim lastTD As DateTime
                lastTD = GetLastTerminationDate()
                If lastTD <> DateTime.MinValue Then
                    lastTD = GetFirstDayOfNextMonthIfNotFirstOfMonth(lastTD)
                    If lastTD < Convert.ToDateTime(TextBoxRetirementDate.Text) Then
                        dateIsValid = False
                        'commented by Anudeep on 22-sep for BT-1126
                        'errorMessage = errorMessage + "<br> Retirement Date: Should be less than or equal to first of the month following the last Termination date"

                        'Commented by CS: 12/14/2015 YRS-AT-2329 : Refactoring is done for the corresponding custom control has to diaplay * symbol in the Employment Tab
                        'errorMessage = errorMessage + getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREMENT_DATE")
                        If IsHeadingAlreadySetInValidationSummaryControl = False Then
                            IsHeadingAlreadySetInValidationSummaryControl = True
                            errorMessage = ValidationSummaryHeaderText + getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREMENT_DATE")
                        Else
                            errorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREMENT_DATE")
                        End If
                        CustomValidatorEndWorkDate.ErrorMessage = errorMessage
                    End If
                End If
            End If
            ''Added by Ashish on 23-Jul-2009 ,if define salary annual increase then must define salary effective date
            If Not DropDownAnnualSalaryIncrease Is Nothing Then
                If Convert.ToDecimal(DropDownAnnualSalaryIncrease.SelectedValue) > 0 Then
                    If TextBoxAnnualSalaryIncreaseEffDate.Text = String.Empty Then
                        dateIsValid = False
                        'commented by Anudeep on 22-sep for BT-1126
                        'errorMessage = errorMessage + "<br> Annual Salary Increase Effective Date: Salary Increase not used because Increase Month is missing."

                        'Commented by CS: 12/14/2015 YRS-AT-2329 : Refactoring is done for the corresponding custom control has to appear * symbol in the Employment Tab
                        'errorMessage = errorMessage + getmessage("MESSAGE_RETIREMENT_ESTIMATE_ANNUAL_SALARY_INCREASE")
                        'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Refactoring is done for the corresponding custom control has to appear * symbol
                        'Checking whether heading of the Validation Summary Control is set or not
                        If IsHeadingAlreadySetInValidationSummaryControl = False Then
                            IsHeadingAlreadySetInValidationSummaryControl = True
                            errorMessage = ValidationSummaryHeaderText + getmessage("MESSAGE_RETIREMENT_ESTIMATE_ANNUAL_SALARY_INCREASE")
                        Else
                            errorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_ANNUAL_SALARY_INCREASE")
                        End If

                        'START: Shilpa N | 04/08/2019 | YRS-AT-3392 | Commented the existing code. Assigning Error Message to new custom validator
                        'CustomValidatorAnnualSalaryIncreaseEffDate.ErrorMessage = errorMessage
                        CustomValidatorDDLAnnualSalaryIncreaseEffDate.ErrorMessage = errorMessage
                        'END: Shilpa N | 04/08/2019 | YRS-AT-3392 | Commented the existing code. Assigning Error Message to new custom validator
                    End If
                End If

            End If

            'Validate Annual Salary Increase Effective Date
            If TextBoxAnnualSalaryIncreaseEffDate.Text <> String.Empty Then
                Dim annualSalaryIncEfftDate As DateTime = Convert.ToDateTime(TextBoxAnnualSalaryIncreaseEffDate.Text)
                If annualSalaryIncEfftDate <= DateTime.Today Then
                    dateIsValid = False
                    'commented by Anudeep on 22-sep for BT-1126
                    'errorMessage = errorMessage + "<br> Annual Salary Increase Effective Date: Should be greater than today's Date"

                    'Commented by CS: 12/14/2015 YRS-AT-2329 : Refactoring is done for the corresponding custom control  to diaplay * symbol in the Employment Tab
                    '   errorMessage = errorMessage + getmessage("MESSAGE_RETIREMENT_ESTIMATE_INCREASE_EFFECTIVE_DATE")
                    'Start:Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Refactoring is done for the corresponding custom control has to diaplay * symbol
                    'Checking whether heading of the Validation Summary Control is set or not
                    If IsHeadingAlreadySetInValidationSummaryControl = False Then
                        IsHeadingAlreadySetInValidationSummaryControl = True
                        errorMessage = ValidationSummaryHeaderText + getmessage("MESSAGE_RETIREMENT_ESTIMATE_INCREASE_EFFECTIVE_DATE")
                    Else
                        errorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_INCREASE_EFFECTIVE_DATE")
                    End If
                    CustomValidatorAnnualSalaryIncreaseEffDate.ErrorMessage = errorMessage
                    'SR:2012.07.30 commented below line because annualSalaryIncEfftDate consider only month(BT-1041/YRS 5.0-1599)
                    'ElseIf annualSalaryIncEfftDate >= Convert.ToDateTime(TextBoxRetirementDate.Text) Then '2012.07.11 SP :BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page
                    '                dateIsValid = False
                    '                errorMessage = errorMessage + "<br> Annual Salary Increase Effective Date: Should be less than Retirement Date"
                End If
            End If

            'SR:2012.07.30 :BT-1041/YRS 5.0-1599: validation for future salary effective date and end work date
            'SR:2012.07.30 :BT-1041/YRS 5.0-1599, End

            If Not dateIsValid Then
                'Commented by CS: 12/14/2015 YRS-AT-2329 : Refactoring is done for the corresponding custom control has to appear * symbol in the Employment Tab
                ' CustomValidatorFutureSalaryDate.ErrorMessage = errorMessage
                'added by ashish 23-jul-2009
                tabStripRetirementEstimate.SelectedIndex = 1
                Me.MultiPageRetirementEstimate.SelectedIndex = Me.tabStripRetirementEstimate.SelectedIndex
            End If

        Catch ex As Exception
            dateIsValid = False
            errorMessage = errorMessage + ex.Message
        End Try

        args.IsValid = dateIsValid

    End Sub

    'Made old function and create new function with same name for Phase V changes
    Private Function getElectiveAccounts_Old() As DataSet
        Try
            Me.UpdateElectiveAccountAssumptions(String.Empty)
            Dim newElectiveAccounts As DataSet
            newElectiveAccounts = Session("dsElectiveAccountsDet")
            newElectiveAccounts = newElectiveAccounts.Copy()

            For Each dr As DataRow In newElectiveAccounts.Tables(0).Rows
                Select Case dr("chrAdjustmentBasisCode")
                    Case "M"
                        dr("chrAdjustmentBasisCode") = RetirementBOClass.MONTHLY_PAYMENTS
                    Case "P"
                        dr("chrAdjustmentBasisCode") = RetirementBOClass.MONTHLY_PERCENT_SALARY
                    Case "L"
                        dr("chrAdjustmentBasisCode") = RetirementBOClass.ONE_LUMP_SUM
                    Case "Y"
                        dr("chrAdjustmentBasisCode") = RetirementBOClass.YEARLY_LUMP_SUM_PAYMENT
                End Select
            Next

            ' This will happen when the participant doesnt have any employment records like a QDRO participant 
            If Session("dsElectiveAccounts") Is Nothing Then
                Session("dsElectiveAccounts") = RetirementBOClass.SearchElectiveAccounts(Me.PersonId)
            End If

            Dim existingElectiveAccounts As DataSet
            existingElectiveAccounts = Session("dsElectiveAccounts")
            If Not existingElectiveAccounts.Tables(0).Columns.Contains("Selected") Then
                existingElectiveAccounts.Tables(0).Columns.Add("Selected")
            End If

            Dim electiveAccounts As DataSet
            electiveAccounts = newElectiveAccounts.Clone()

            Dim drows As DataRow()
            Dim EndDateofOriginalContribution As String

            ' Get the Retirement accounts            
            'NP:2008.05.06 - Only load these if the plan type selected is Retirement Plan or Both
            If Me.PlanType = "R" OrElse Me.PlanType = "B" Then
                For Each dr As DataRow In existingElectiveAccounts.Tables(0).Rows
                    If dr("PlanType") = "RETIREMENT" Then
                        electiveAccounts.Tables(0).ImportRow(dr)
                    End If
                Next
                For Each dr As DataRow In newElectiveAccounts.Tables(0).Rows
                    If dr("PlanType") = "RETIREMENT" And dr("Selected") = True Then
                        drows = electiveAccounts.Tables(0).Select("chrAcctType='" + dr("chrAcctType") + "'")
                        If drows.Length > 0 Then
                            If Convert.ToDouble(dr("mnyAddlContribution")) = 0 And Convert.ToDouble(dr("numAddlPctg")) = 0 And dr("dtsTerminationDate").ToString().Trim() <> String.Empty Then
                                drows(0)("dtsTerminationDate") = dr("dtsTerminationDate")
                            Else
                                If dr("dtmEffDate").ToString().Trim() <> String.Empty Then
                                    drows(0)("dtsTerminationDate") = DateAdd(DateInterval.Day, -1, Convert.ToDateTime(dr("dtmEffDate"))).ToString()
                                End If

                                electiveAccounts.Tables(0).ImportRow(dr)
                            End If
                        Else
                            electiveAccounts.Tables(0).ImportRow(dr)
                        End If
                    End If
                Next
            End If

            ' Get the Savings accounts
            'NP:2008.05.06 - Only load these if the plan type selected is Retirement Plan or Both
            If Me.PlanType = "S" Or Me.PlanType = "B" Then
                For Each dr As DataRow In existingElectiveAccounts.Tables(0).Rows
                    If dr("PlanType") = "SAVINGS" Then
                        electiveAccounts.Tables(0).ImportRow(dr)
                    End If
                Next
                For Each dr As DataRow In newElectiveAccounts.Tables(0).Rows
                    If dr("PlanType") = "SAVINGS" And dr("Selected") = True Then
                        drows = electiveAccounts.Tables(0).Select("chrAcctType='" + dr("chrAcctType") + "'")

                        If drows.Length > 0 Then
                            If Convert.ToDouble(dr("mnyAddlContribution")) = 0 And Convert.ToDouble(dr("numAddlPctg")) = 0 And dr("dtsTerminationDate").ToString().Trim() <> String.Empty Then
                                drows(0)("dtsTerminationDate") = dr("dtsTerminationDate")
                            Else
                                If dr("dtmEffDate").ToString().Trim() <> String.Empty Then
                                    drows(0)("dtsTerminationDate") = DateAdd(DateInterval.Day, -1, Convert.ToDateTime(dr("dtmEffDate"))).ToString()
                                End If

                                electiveAccounts.Tables(0).ImportRow(dr)
                            End If
                        Else
                            electiveAccounts.Tables(0).ImportRow(dr)
                        End If
                    End If
                Next
            End If

            electiveAccounts.AcceptChanges()
            getElectiveAccounts_Old = electiveAccounts

        Catch
            Throw
        End Try
    End Function
    Private Sub saveActiveSalaryInformation()
        Dim dsEmploymentDetails As DataSet
        Dim drEmpSalryRow As DataRow
        Dim dtCol As DataColumn
        Try
            ' Create the temporary table 
            'If Session("employmentSalaryInformation") Is Nothing Then 'MMR | 2017.03.14 | Commneted existing code as session will be filled in every time for storing employement details
            Dim dtEmploymentSalary As New DataTable
            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
            'dtEmploymentSalary.Columns.Add("EmpEventID")
            'dtEmploymentSalary.Columns.Add("ModifiedSal")

            'dtEmploymentSalary.Columns.Add("FutureSalary")
            'dtEmploymentSalary.Columns.Add("FutureSalaryEffDate")

            'dtEmploymentSalary.Columns.Add("StartWorkDate")
            'dtEmploymentSalary.Columns.Add("EndWorkDate")
            'dtEmploymentSalary.Columns.Add("AnnualSalaryIncrease")

            'dtEmploymentSalary.Columns.Add("AnnualSalaryIncreaseEffDate") 'Phase IV Changes
            'ASHISH:2010.10.11 code Added for YRS 5.0-855 BT-624
            dtCol = New DataColumn("EmpEventID", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)
            dtCol = New DataColumn("ModifiedSal", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)
            dtCol = New DataColumn("FutureSalary", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)
            dtCol = New DataColumn("FutureSalaryEffDate", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)
            dtCol = New DataColumn("StartWorkDate", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)
            dtCol = New DataColumn("EndWorkDate", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)
            dtCol = New DataColumn("AnnualSalaryIncrease", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)
            dtCol = New DataColumn("AnnualSalaryIncreaseEffDate", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)

            '****************
            'dtEmploymentSalary.Columns.Add("LastSalPaidMonth")
            '2009.11.17-Commented by Ashish ,this columns are not used
            'dtEmploymentSalary.Columns.Add("AvgSalary")
            'dtEmploymentSalary.Columns.Add("LastPaidSal")
            'dtEmploymentSalary.Columns.Add("SalDate")
            '****************
            'ASHISH:2010.10.11 code Added for YRS 5.0-855 BT-624
            If Not Session("dsPersonEmploymentDetails") Is Nothing Then
                dsEmploymentDetails = Session("dsPersonEmploymentDetails")
                If Not dsEmploymentDetails.Tables.Count = 0 AndAlso Not dsEmploymentDetails.Tables(0) Is Nothing Then
                    For Each dtEmpRow As DataRow In dsEmploymentDetails.Tables(0).Rows
                        drEmpSalryRow = dtEmploymentSalary.NewRow

                        drEmpSalryRow("EmpEventID") = dtEmpRow("guiEmpEventId")
                        If dtEmpRow("AvgSalaryPerEmployment").ToString() <> String.Empty Then
                            drEmpSalryRow("ModifiedSal") = Convert.ToDecimal(dtEmpRow("AvgSalaryPerEmployment")).ToString("f2")
                        Else
                            drEmpSalryRow("ModifiedSal") = "0.00"
                        End If
                        If dtEmpRow("Start").ToString() <> String.Empty Then
                            drEmpSalryRow("StartWorkDate") = Convert.ToDateTime(dtEmpRow("Start")).ToString("MM/dd/yyyy")
                        Else
                            drEmpSalryRow("StartWorkDate") = String.Empty
                        End If
                        If dtEmpRow("End").ToString() <> String.Empty Then
                            drEmpSalryRow("EndWorkDate") = Convert.ToDateTime(dtEmpRow("End")).ToString("MM/dd/yyyy")
                        Else
                            drEmpSalryRow("EndWorkDate") = String.Empty
                        End If

                        'drEmpSalryRow("ModifiedSal") = IIf(dtEmpRow("AvgSalaryPerEmployment").ToString() <> String.Empty, Convert.ToDecimal(dtEmpRow("AvgSalaryPerEmployment")).ToString("f2"), String.Empty)
                        ' drEmpSalryRow("StartWorkDate") = IIf(dtEmpRow("Start").ToString() <> String.Empty, Convert.ToDateTime(dtEmpRow("Start")).ToString("MM/dd/yyyy"), String.Empty)
                        ' drEmpSalryRow("EndWorkDate") = IIf(dtEmpRow("End").ToString() <> String.Empty, Convert.ToDateTime(dtEmpRow("End")).ToString("MM/dd/yyyy"), String.Empty)
                        drEmpSalryRow("AnnualSalaryIncrease") = String.Empty
                        drEmpSalryRow("AnnualSalaryIncreaseEffDate") = String.Empty

                        dtEmploymentSalary.Rows.Add(drEmpSalryRow)
                    Next
                End If
            End If

            dtEmploymentSalary.AcceptChanges()

            Session("employmentSalaryInformation") = dtEmploymentSalary

            'Exit Sub 'MMR | 2017.03.14 | Commneted existing code as session will be filled in every time for storing employement details
            'End If 'MMR | 2017.03.14 | Commneted existing code as session will be filled in every time for storing employement details

            Dim selectedEmployment As String = Session("selectedEmployment")
            ' If the previously selected employment is active
            If Session("selectedEmployment") <> String.Empty Then
                Dim employmentSalaryInformation As DataTable = Session("employmentSalaryInformation")
                Dim drs() As DataRow = employmentSalaryInformation.Select("EmpEventID = '" & selectedEmployment & "'")
                Dim dr As DataRow
                If drs.Length > 0 Then ' Update the values in the row
                    dr = drs(0)
                    'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                    'Else ' Create a new row and then update the values
                    '    dr = employmentSalaryInformation.NewRow()
                    '    dr("EmpEventID") = selectedEmployment
                    '    employmentSalaryInformation.Rows.Add(dr)
                End If
                'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                ' dr("ModifiedSal") = TextBoxModifiedSal.Text
                dr("FutureSalary") = TextBoxFutureSalary.Text
                dr("FutureSalaryEffDate") = TextBoxFutureSalaryEffDate.Text
                dr("StartWorkDate") = TextBoxStartWorkDate.Text
                dr("EndWorkDate") = TextBoxEndWorkDate.Text

                dr("AnnualSalaryIncrease") = 0
                dr("AnnualSalaryIncreaseEffDate") = "" 'Phase IV Changes

                '****************
                'Commented by Ashish 30-July-2008 YRS 5.0-445 ,Start
                'If TextBoxSalaryAverage.Text.Trim() <> String.Empty Then
                '    dr("AvgSalary") = TextBoxSalaryAverage.Text.Trim()
                'Else
                '    dr("AvgSalary") = 0
                'End If

                ''****************
                'If TextBoxLastPaidSalary.Text.Trim() <> String.Empty Then
                '    dr("LastPaidSal") = TextBoxLastPaidSalary.Text.Trim()
                'Else
                '    dr("LastPaidSal") = 0
                'End If
                ''****************

                'If LabelLastPaidMonthDate.Text.Trim() <> String.Empty Then
                '    dr("SalDate") = LabelLastPaidMonthDate.Text.Trim()
                'Else
                '    dr("SalDate") = DBNull.Value
                'End If
                ''****************
                'Commented by Ashish 30-July-2008 YRS 5.0-445 ,End
                'Phase IV Changes - Start
                If DropDownAnnualSalaryIncrease.SelectedValue > 0 Then
                    If TextBoxAnnualSalaryIncreaseEffDate.Text.Trim <> "" Then
                        dr("AnnualSalaryIncrease") = DropDownAnnualSalaryIncrease.SelectedValue
                        dr("AnnualSalaryIncreaseEffDate") = TextBoxAnnualSalaryIncreaseEffDate.Text 'Phase IV Changes
                    End If
                End If
                'Phase IV Changes - End

                employmentSalaryInformation.AcceptChanges()
                Session("employmentSalaryInformation") = employmentSalaryInformation
                '''Display termination date in DataGridEmployment
                ''If Not DataGridEmployment Is Nothing Then
                ''    If DataGridEmployment.SelectedIndex <> -1 Then
                ''        Dim drEmpRows As DataRow()
                ''        drEmpRows = employmentSalaryInformation.Select("EmpEventID = '" & selectedEmployment & "'")
                ''        If Not drEmpRows Is Nothing Then

                ''            Dim lblTermDate As Label
                ''            lblTermDate = CType(DataGridEmployment.SelectedItem.Cells(7).FindControl("lblTempTerminationDate"), Label)
                ''            If Not lblTermDate Is Nothing Then
                ''                lblTermDate.Text = Convert.ToDateTime(drEmpRows(0)("EndWorkDate")).ToString("MM/dd/yyyy")
                ''            End If
                ''        End If
                ''    End If
                ''End If
            End If

        Catch
            Throw
        End Try
    End Sub

    'START: PPP | 03/20/2017 | YRS-AT-2625 | Creating overload function of old saveActiveSalaryInformation, which will be used in DISABL case to send normal average salary to "CalculateAccountBalancesAndProjections" function
    Private Function saveActiveSalaryInformation(ByVal dsEmploymentDetails As DataSet) As DataTable
        Dim dtEmploymentSalary As DataTable
        Dim drEmpSalryRow As DataRow
        Dim dtCol As DataColumn
        Try
            dtEmploymentSalary = New DataTable()
            dtCol = New DataColumn("EmpEventID", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)
            dtCol = New DataColumn("ModifiedSal", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)
            dtCol = New DataColumn("FutureSalary", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)
            dtCol = New DataColumn("FutureSalaryEffDate", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)
            dtCol = New DataColumn("StartWorkDate", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)
            dtCol = New DataColumn("EndWorkDate", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)
            dtCol = New DataColumn("AnnualSalaryIncrease", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)
            dtCol = New DataColumn("AnnualSalaryIncreaseEffDate", System.Type.GetType("System.String"))
            dtCol.DefaultValue = String.Empty
            dtEmploymentSalary.Columns.Add(dtCol)

            If Not dsEmploymentDetails Is Nothing Then
                If Not dsEmploymentDetails.Tables.Count = 0 AndAlso Not dsEmploymentDetails.Tables(0) Is Nothing Then
                    For Each dtEmpRow As DataRow In dsEmploymentDetails.Tables(0).Rows
                        drEmpSalryRow = dtEmploymentSalary.NewRow

                        drEmpSalryRow("EmpEventID") = dtEmpRow("guiEmpEventId")
                        If dtEmpRow("AvgSalaryPerEmployment").ToString() <> String.Empty Then
                            drEmpSalryRow("ModifiedSal") = Convert.ToDecimal(dtEmpRow("AvgSalaryPerEmployment")).ToString("f2")
                        Else
                            drEmpSalryRow("ModifiedSal") = "0.00"
                        End If
                        If dtEmpRow("Start").ToString() <> String.Empty Then
                            drEmpSalryRow("StartWorkDate") = Convert.ToDateTime(dtEmpRow("Start")).ToString("MM/dd/yyyy")
                        Else
                            drEmpSalryRow("StartWorkDate") = String.Empty
                        End If
                        If dtEmpRow("End").ToString() <> String.Empty Then
                            drEmpSalryRow("EndWorkDate") = Convert.ToDateTime(dtEmpRow("End")).ToString("MM/dd/yyyy")
                        Else
                            drEmpSalryRow("EndWorkDate") = String.Empty
                        End If

                        drEmpSalryRow("AnnualSalaryIncrease") = String.Empty
                        drEmpSalryRow("AnnualSalaryIncreaseEffDate") = String.Empty

                        dtEmploymentSalary.Rows.Add(drEmpSalryRow)
                    Next
                End If
            End If

            dtEmploymentSalary.AcceptChanges()
            Return dtEmploymentSalary
        Catch
            Throw
        Finally
            dtCol = Nothing
            drEmpSalryRow = Nothing
            dtEmploymentSalary = Nothing
        End Try
    End Function
    'END: PPP | 03/20/2017 | YRS-AT-2625 | Creating overload function of old saveActiveSalaryInformation, which will be used in DISABL case to send normal average salary to "CalculateAccountBalancesAndProjections" function

    Private Sub displayEmploymentModifiedSalaryInformation()
        Try
            Dim l_string_EmploymentTermDate As String = String.Empty
            Dim selectedEmployment As String = String.Empty

            If DataGridEmployment.Items.Count > 0 Then
                'check if the employment is actually active then just hold the id of the selected employment
                If DataGridEmployment.SelectedItem.Cells(5).Text.ToString() = "&nbsp;" Then
                    l_string_EmploymentTermDate = String.Empty
                Else
                    l_string_EmploymentTermDate = DataGridEmployment.SelectedItem.Cells(5).Text.ToString()
                End If

                selectedEmployment = DataGridEmployment.SelectedItem.Cells(6).Text.ToString()
            End If

            If selectedEmployment = "nbsp;" Then
                selectedEmployment = String.Empty
            End If

            Session("selectedEmployment") = selectedEmployment
            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
            'Dim boolEmploymentSalDetailsNotModified As Boolean = False

            ' If the previously selected employment is active
            If selectedEmployment <> String.Empty Then
                If Not Session("employmentSalaryInformation") Is Nothing Then

                    Dim employmentSalaryInformation As DataTable = Session("employmentSalaryInformation")
                    If employmentSalaryInformation.Rows.Count > 0 Then
                        Dim drs() As DataRow = employmentSalaryInformation.Select("EmpEventID = '" & selectedEmployment & "'")
                        Dim dr As DataRow

                        If drs.Length > 0 Then ' Update the values in the row
                            dr = drs(0)

                            '****************
                            'Commented by Ashish 30-July-2008 YRS 5.0-445 ,Start
                            'TextBoxSalaryAverage.Text = "0.00"
                            'LabelLastPaidMonthDate.Text = "//"
                            'TextBoxLastPaidSalary.Text = "0.00"
                            'Commented by Ashish 30-July-2008 YRS 5.0-445 ,End
                            TextBoxModifiedSal.Text = "0.00"



                            TextBoxFutureSalary.Text = "0.00"
                            TextBoxFutureSalaryEffDate.Text = ""

                            TextBoxStartWorkDate.Text = ""
                            TextBoxEndWorkDate.Text = ""

                            TextBoxAnnualSalaryIncreaseEffDate.Text = "" 'Phase IV Changes
                            DropDownAnnualSalaryIncrease.SelectedValue = 0 'Phase IV Changes
                            '****************
                            'Commented by Ashish 30-July-2008 YRS 5.0-445 ,Start
                            'If Not dr("AvgSalary") Is DBNull.Value Then
                            '    TextBoxSalaryAverage.Text = dr("AvgSalary").ToString()
                            '    TextBoxSalaryAverage.Text = TextBoxSalaryAverage.Text.Substring(0, TextBoxSalaryAverage.Text.IndexOf(".")) & ".00"
                            '    TextBoxModifiedSal.Text = TextBoxSalaryAverage.Text
                            'End If
                            'Commented by Ashish 30-July-2008 YRS 5.0-445 ,End
                            'ASHISH:2009.11.17 Commented
                            'Added by Ashish 30-July-2008 YRS 5.0-445 ,Start
                            'If Not dr("AvgSalary") Is DBNull.Value Then

                            '    TextBoxModifiedSal.Text = dr("AvgSalary").ToString().Substring(0, dr("AvgSalary").ToString().IndexOf(".")) & ".00"
                            'End If
                            'Added by Ashish 30-July-2008 YRS 5.0-445 ,End
                            '****************
                            TextBoxFutureSalary.Text = dr("FutureSalary")
                            TextBoxFutureSalaryEffDate.Text = dr("FutureSalaryEffDate")
                            '****************
                            'Commented by Ashish 31-July-2008 YRS 5.0-445 ,Start
                            'TextBoxModifiedSal.Text = dr("ModifiedSal")
                            'Commented by Ashish 31-July-2008 YRS 5.0-445 ,End
                            'Added by Ashish 31-July-2008 YRS 5.0-445 ,Start
                            If dr("ModifiedSal").ToString().Trim() = "" Then
                                TextBoxModifiedSal.Text = "0.00"
                            Else
                                TextBoxModifiedSal.Text = dr("ModifiedSal")
                            End If
                            'Added by Ashish 31-July-2008 YRS 5.0-445 ,End
                            '****************
                            TextBoxStartWorkDate.Text = dr("StartWorkDate")
                            TextBoxEndWorkDate.Text = dr("EndWorkDate")
                            '****************
                            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                            'DropDownAnnualSalaryIncrease.SelectedValue = dr("AnnualSalaryIncrease")
                            If dr("AnnualSalaryIncrease").ToString() <> String.Empty Then
                                DropDownAnnualSalaryIncrease.SelectedValue = dr("AnnualSalaryIncrease")
                            End If
                            TextBoxAnnualSalaryIncreaseEffDate.Text = dr("AnnualSalaryIncreaseEffDate") 'Phase IV Changes

                            '****************
                            'Commented by Ashish 30-July-2008 YRS 5.0-445 ,Start
                            'If Not dr("LastPaidSal") Is DBNull.Value Then
                            '    TextBoxLastPaidSalary.Text = dr("LastPaidSal").ToString()
                            '    TextBoxLastPaidSalary.Text = TextBoxLastPaidSalary.Text.Substring(0, TextBoxLastPaidSalary.Text.IndexOf(".")) & ".00"
                            'End If
                            '****************

                            'If Not dr("SalDate") Is DBNull.Value Then


                            '        LabelLastPaidMonthDate.Text = Convert.ToDateTime(dr("SalDate")).ToString("MM/dd/yyyy")

                            'End If
                            'Commented by Ashish 30-July-2008 YRS 5.0-445 ,End
                            '****************
                            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                            'boolEmploymentSalDetailsNotModified = True
                        End If
                    End If
                End If
            End If
            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
            'If boolEmploymentSalDetailsNotModified = False Then
            '    loadEmploymentActualSalaryDetails()
            'End If

            showGridRowSelection()

            setEmploymentControlState()
        Catch
            Throw
        End Try
    End Sub

    'NP:2008.06.13-YRS-5.0-457 - We no longer need to check or uncheck all accounts based on the Plan Checkbox
    'Private Sub CheckAccounts()
    '    Dim i As Integer

    '    'If Unchecked, uncheck all the retirement plan account
    '    If DatagridElectiveRetirementAccounts.Visible = True Then
    '        For i = 0 To DatagridElectiveRetirementAccounts.Items.Count - 1
    '            Dim chk As CheckBox
    '            chk = DatagridElectiveRetirementAccounts.Items(i).Cells(0).Controls(1)
    '            chk.Checked = CheckBoxRetirementPlan.Checked
    '        Next
    '    End If
    'End Sub
    Private Function getExcludedAccounts() As DataTable
        Dim dtExcludedAccounts As DataTable = New DataTable
        Dim l_dtExcludeColumn As DataColumn
        dtExcludedAccounts.Columns.Add("chrAcctType")
        dtExcludedAccounts.Columns.Add("bitRet_Voluntary")
        dtExcludedAccounts.Columns.Add("bitBasicAcct")
        dtExcludedAccounts.Columns.Add("chvPlanType")
        'Added by Ashish for Phase V changes ,Start
        l_dtExcludeColumn = dtExcludedAccounts.Columns.Add("bitPA", System.Type.GetType("System.Boolean"))
        l_dtExcludeColumn.DefaultValue = False
        l_dtExcludeColumn = dtExcludedAccounts.Columns.Add("bitYA", System.Type.GetType("System.Boolean"))
        l_dtExcludeColumn.DefaultValue = False
        l_dtExcludeColumn = dtExcludedAccounts.Columns.Add("bitEP", System.Type.GetType("System.Boolean"))
        l_dtExcludeColumn.DefaultValue = False
        l_dtExcludeColumn = dtExcludedAccounts.Columns.Add("bitPersonalAmtExcluded", System.Type.GetType("System.Boolean"))
        l_dtExcludeColumn.DefaultValue = False
        l_dtExcludeColumn = dtExcludedAccounts.Columns.Add("bitYmcaAmtExcluded", System.Type.GetType("System.Boolean"))
        l_dtExcludeColumn.DefaultValue = False
        'Added by Ashish for Phase V changes ,End
        dtExcludedAccounts.AcceptChanges()

        Dim accountType As String
        Dim boolVoluntaryAccount As Boolean
        Dim boolBasicAccount As Boolean
        Dim planType As String

        Dim selected As Boolean
        Dim l_string_PlanType As String = Me.PlanType 'DropDownListPlanType.SelectedValue.ToString()
        'Commented by Ashish for Phase V changes ,Start
        '' Non Voluntary Grid
        ''If l_string_PlanType = "RETIREMENT" Or l_string_PlanType = "BOTH" Then
        'If l_string_PlanType = "R" Or l_string_PlanType = "B" Then
        '    If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
        '        For Each di As DataGridItem In DatagridElectiveRetirementAccounts.Items
        '            selected = CType(di.FindControl("CheckboxRet"), CheckBox).Checked
        '            If selected = False Then
        '                accountType = di.Cells(RET_ACCT_TYPE).Text.ToUpper()

        '                boolVoluntaryAccount = Convert.ToBoolean(di.Cells(RET_VOL_ACCT).Text.ToUpper())
        '                boolBasicAccount = Convert.ToBoolean(di.Cells(RET_BASIC_ACCT).Text.ToUpper())
        '                planType = di.Cells(RET_PLANE_TYPE).Text.ToUpper()

        '                Dim dr As DataRow = dtExcludedAccounts.NewRow()
        '                dr("chrAcctType") = accountType

        '                dr("bitRet_Voluntary") = boolVoluntaryAccount
        '                dr("bitBasicAcct") = boolBasicAccount
        '                dr("chvPlanType") = planType

        '                dtExcludedAccounts.Rows.Add(dr)
        '                dtExcludedAccounts.AcceptChanges()
        '            End If
        '        Next
        '    End If
        'End If

        '' Voluntary Grid
        ''If l_string_PlanType = "SAVINGS" Or l_string_PlanType = "BOTH" Then
        'If l_string_PlanType = "S" Or l_string_PlanType = "B" Then
        '    If DatagridElectiveSavingsAccounts.Items.Count > 0 Then
        '        For Each di As DataGridItem In DatagridElectiveSavingsAccounts.Items
        '            selected = CType(di.FindControl("CheckBoxSav"), CheckBox).Checked
        '            If selected = False Then
        '                accountType = di.Cells(SAV_ACCT_TYPE).Text.ToUpper()

        '                boolVoluntaryAccount = Convert.ToBoolean(di.Cells(SAV_VOL_ACCT).Text.ToUpper())
        '                boolBasicAccount = Convert.ToBoolean(di.Cells(SAV_BASIC_ACCT).Text.ToUpper())
        '                planType = di.Cells(SAV_PLANE_TYPE).Text.ToUpper()

        '                Dim dr As DataRow = dtExcludedAccounts.NewRow()
        '                dr("chrAcctType") = accountType

        '                dr("bitRet_Voluntary") = boolVoluntaryAccount
        '                dr("bitBasicAcct") = boolBasicAccount
        '                dr("chvPlanType") = planType

        '                dtExcludedAccounts.Rows.Add(dr)
        '                dtExcludedAccounts.AcceptChanges()
        '            End If
        '        Next
        '    End If
        'End If
        'Commented by Ashish for Phase V changes ,End
        ' Non Voluntary Grid
        'If l_string_PlanType = "RETIREMENT" Or l_string_PlanType = "BOTH" Then
        Dim l_dsElectiveAccountInfo As DataSet
        Dim l_dtElectiveAccountInfo As DataTable
        Dim l_LegacyAcctType As String = String.Empty
        If Not Session("dsElectiveAccountsDet") Is Nothing And TypeOf Session("dsElectiveAccountsDet") Is DataSet Then
            l_dsElectiveAccountInfo = DirectCast(Session("dsElectiveAccountsDet"), DataSet)
            l_dtElectiveAccountInfo = l_dsElectiveAccountInfo.Tables("RetireeElectiveAccountsInformation")
        End If

        If l_string_PlanType = "R" Or l_string_PlanType = "B" Then
            If Not l_dtElectiveAccountInfo Is Nothing Then


                If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
                    For Each di As DataGridItem In DatagridElectiveRetirementAccounts.Items
                        selected = CType(di.FindControl("CheckboxRet"), CheckBox).Checked
                        If selected = False Then
                            accountType = di.Cells(RET_ACCT_TYPE).Text.ToUpper()
                            l_LegacyAcctType = di.Cells(RET_LEGACY_ACCT_TYPE).Text.ToUpper()

                            boolVoluntaryAccount = Convert.ToBoolean(di.Cells(RET_VOL_ACCT).Text.ToUpper())
                            boolBasicAccount = Convert.ToBoolean(di.Cells(RET_BASIC_ACCT).Text.ToUpper())
                            planType = di.Cells(RET_PLANE_TYPE).Text.ToUpper()

                            'create rows in dtExcludedAccounts
                            CreateExcludedElectiveAcctRows(l_dtElectiveAccountInfo, dtExcludedAccounts, planType, l_LegacyAcctType, boolBasicAccount)


                        End If
                    Next
                End If
            End If
        End If

        ' Voluntary Grid
        'If l_string_PlanType = "SAVINGS" Or l_string_PlanType = "BOTH" Then
        If l_string_PlanType = "S" Or l_string_PlanType = "B" Then
            If DatagridElectiveSavingsAccounts.Items.Count > 0 Then
                For Each di As DataGridItem In DatagridElectiveSavingsAccounts.Items
                    selected = CType(di.FindControl("CheckBoxSav"), CheckBox).Checked
                    If selected = False Then
                        accountType = di.Cells(SAV_ACCT_TYPE).Text.ToUpper()
                        l_LegacyAcctType = di.Cells(SAV_LEGACY_ACCT_TYPE).Text.ToUpper()
                        boolVoluntaryAccount = Convert.ToBoolean(di.Cells(SAV_VOL_ACCT).Text.ToUpper())
                        boolBasicAccount = Convert.ToBoolean(di.Cells(SAV_BASIC_ACCT).Text.ToUpper())
                        planType = di.Cells(SAV_PLANE_TYPE).Text.ToUpper()
                        'create exclude account row
                        CreateExcludedElectiveAcctRows(l_dtElectiveAccountInfo, dtExcludedAccounts, planType, l_LegacyAcctType, boolBasicAccount)

                    End If
                Next
            End If
        End If

        getExcludedAccounts = dtExcludedAccounts
    End Function
    Private Sub setPrintText()
        Dim printMsg = String.Empty
        Select Case PlanType
            Case "R"
                'printMsg = "Annuity Estimate based on Retirement Plan only"
                printMsg = getmessage("MESSAGE_RETIREMENT_ESTIMATE_ANNUTITY_RETIREMENT")
            Case "S"
                printMsg = getmessage("MESSAGE_RETIREMENT_ESTIMATE_ANNUTITY_SAVINGS")
                'printMsg = "Annuity Estimate based on Savings Plan only"
            Case "B"
                printMsg = getmessage("MESSAGE_RETIREMENT_ESTIMATE_ANNUTITY_BOTH")
                'printMsg = "Annuity Estimate based on both Retirement Plan and Savings Plan"
            Case Else
        End Select
        'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
        If Me.AreJAnnuitiesUnavailable Then
            'NP:2011.10.31 - Changing the text of the message as per email from Raj dt: 2011.10.28
            'printMsg += ". Due to the age difference between you and the Survivor you have selected, this option is not available."
            'printMsg += "." & vbCrLf & vbCrLf & "Joint & Survivor annuity options marked as *N/A are not available due to the age difference between you and your survivor."
            'START : ML | 20.02.05 | YRS-AT-4769 |New parameter for Non-spouse is disabled or chronically ill
            'printMsg += "." & vbCrLf & vbCrLf & getmessage("MESSAGE_RETIREMENT_ESTIMATE_JOINTSURVIVOUR_AGE_DIFFERENCE")
            Dim chronicallyIll As Boolean = False
            If (chronicallyIll = False) Then
                printMsg += "." & vbCrLf & vbCrLf & YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ANNUITY_ESTIMATE_JSTEXT_CHRONICALY_NOT_ILL).DisplayText
            Else
                printMsg += "." & vbCrLf & vbCrLf & YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_ANNUITY_ESTIMATE_JSTEXT_CHRONICALY_ILL).DisplayText
            End If
            'END : ML | 20.02.05 | YRS-AT-4769 |New parameter for Non-spouse is disabled or chronically ill
        End If

        TextBoxPraAssumption.Text = printMsg
    End Sub
    '2012.02.24 SP:  BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary  - Start
    Private Sub BindBeneficiaryDetails(ByVal personId As String)
        'DataGridAnnuityBeneficiaries
        Dim dsParticipantBeneficiaries As DataSet
        Dim foundRows() As DataRow
        Dim dtParticipantBeneficiaries As New DataTable

        Try
            If Session("dsParticipantBeneficiaries") Is Nothing Then
                dsParticipantBeneficiaries = RetirementBOClass.getParticipantBeneficiaries(personId)
            Else
                dsParticipantBeneficiaries = Session("dsParticipantBeneficiaries")
            End If
            If (HelperFunctions.isNonEmpty(dsParticipantBeneficiaries)) Then
                dtParticipantBeneficiaries = dsParticipantBeneficiaries.Tables(0).Clone()
            End If
            foundRows = dsParticipantBeneficiaries.Tables(0).Select("chvRelationshipCode not in('ES','IN','TR')")
            For Each drRow As DataRow In foundRows
                dtParticipantBeneficiaries.ImportRow(drRow)
            Next
            If (HelperFunctions.isNonEmpty(dtParticipantBeneficiaries)) Then
                DataGridAnnuityBeneficiaries.Visible = True
                DataGridAnnuityBeneficiaries.DataSource = dtParticipantBeneficiaries
                DataGridAnnuityBeneficiaries.DataBind()
            Else
                DataGridAnnuityBeneficiaries.Visible = False
            End If
            LabelNoBeneficiary.Visible = Not (HelperFunctions.isNonEmpty(dtParticipantBeneficiaries))
        Catch
            Throw
        End Try
    End Sub

    Private Sub AddSelectedParticipantBeneficiary(ByVal paraRow As DataRow)
        Dim dsParticipantBenificiary As New DataSet()
        If (Session("dsSelectedParticipantBeneficiary") Is Nothing) Then
            ''Session("dsSelectedParticipantBeneficiary") = New DataSet()
            dsParticipantBenificiary.Tables.Add(CType(Session("dsParticipantBeneficiaries"), DataSet).Tables(0).Clone())
        Else
            dsParticipantBenificiary = CType(Session("dsSelectedParticipantBeneficiary"), DataSet)
        End If

        dsParticipantBenificiary.Tables(0).Rows.Clear()
        dsParticipantBenificiary.Tables(0).ImportRow(paraRow)
        Session("dsSelectedParticipantBeneficiary") = dsParticipantBenificiary
    End Sub

    Private Sub UpdateSelectedParticiapntBeneficary()
        Dim dsParticipantBenificiary As New DataSet()
        Dim dr As DataRow
        If (HelperFunctions.isNonEmpty(Session("dsSelectedParticipantBeneficiary"))) Then
            dsParticipantBenificiary = CType(Session("dsSelectedParticipantBeneficiary"), DataSet)
            If (TextboxBeneficiaryBirthDate.Text.Trim() <> String.Empty) Then
                dr = dsParticipantBenificiary.Tables(0).Rows(0)
                dr("BenBirthDate") = TextboxBeneficiaryBirthDate.Text.Trim()
                dr("BenLastName") = TextboxLastName.Text.Trim()
                dr("BenFirstName") = TextboxFirstName.Text.Trim()
                dr("chvRelationshipCode") = DropDownRelationShip.SelectedValue
                If (DropDownRelationShip.SelectedValue.ToLower() = "sp") Then
                    Me.InputBeneficiaryType = BeneficiaryType.Spouse
                Else
                    Me.InputBeneficiaryType = BeneficiaryType.NonSpouse
                End If
            Else '2012.05.21 SP : BT-976/YRS 5.0-1507 - Reopned issue
                dsParticipantBenificiary.Tables(0).Rows.Clear()
            End If
            dsParticipantBenificiary.Tables(0).AcceptChanges()
            Session("dsSelectedParticipantBeneficiary") = dsParticipantBenificiary
        Else
            Dim dtBenificiary As New DataTable
            dtBenificiary = CType(Session("dsParticipantBeneficiaries"), DataSet).Tables(0).Clone()
            'Session("dsSelectedParticipantBeneficiary") = New DataSet()
            dsParticipantBenificiary.Tables.Add(dtBenificiary)
            If (TextboxBeneficiaryBirthDate.Text.Trim() <> String.Empty) Then
                dr = dsParticipantBenificiary.Tables(0).NewRow()
                dr("BenBirthDate") = TextboxBeneficiaryBirthDate.Text.Trim()
                dr("BenLastName") = TextboxLastName.Text.Trim()
                dr("BenFirstName") = TextboxFirstName.Text.Trim()
                dr("chvRelationshipCode") = DropDownRelationShip.SelectedValue
                If (DropDownRelationShip.SelectedValue.ToLower() = "sp") Then
                    Me.InputBeneficiaryType = BeneficiaryType.Spouse
                Else
                    Me.InputBeneficiaryType = BeneficiaryType.NonSpouse
                End If
                dsParticipantBenificiary.Tables(0).Rows.Add(dr)
                dsParticipantBenificiary.Tables(0).AcceptChanges()
            End If
            Session("dsSelectedParticipantBeneficiary") = dsParticipantBenificiary
        End If

    End Sub

    '2012.02.24 SP:  BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary  - End
    Private Sub LoadBeneficiaryDetails(ByVal personId As String)
        Dim l_datarows_AnnuityBen As DataRow()
        Dim l_datarow_AnnuityBen As DataRow
        Dim dsParticipantBeneficiaries As DataSet
        Dim l_button_select As ImageButton
        Dim i As Int32
        Dim l_bool_RetirementIsBackDated As Boolean
        Dim l_dataset_RelationShips As DataSet
        Dim l_datarow_RelationShips As DataRow()
        Dim l_string_relationshipcode As String
        Try
            'Priya 12-Feb-09 YRS 5.0-620 
            l_bool_RetirementIsBackDated = IsRetirementBackDated()
            If Session("dsParticipantBeneficiaries") Is Nothing Then
                'dsRetEstInfo = RetirementBOClass.SearchRetEstInfo(dsRetireeEstimate.Tables(0).Rows(0).Item("guiUniqueID").ToString())
                dsParticipantBeneficiaries = RetirementBOClass.getParticipantBeneficiaries(personId)
                'Session("dsRetEstInfo") = dsRetEstInfo
                Session("dsParticipantBeneficiaries") = dsParticipantBeneficiaries
            Else
                dsParticipantBeneficiaries = Session("dsParticipantBeneficiaries")
            End If

            If ((l_bool_RetirementIsBackDated = True) Or (Page.IsPostBack = False)) Then
                'If dsRetEstInfo.Tables(0).Rows.Count > 0 Then 'Phase IV Changes
                If dsParticipantBeneficiaries.Tables(0).Rows.Count > 0 Then
                    'Look for spouse with 'PRIMARY' Beneficiary Group
                    'l_datarows_AnnuityBen = dsRetEstInfo.Tables(0).Select("chvRelationshipCode = 'SP'") 'Phase IV Changes
                    'Added By SG: 2012.12.06: BT-1432
                    'l_datarows_AnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvRelationshipCode = 'SP' ")
                    l_datarows_AnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvRelationshipCode = 'SP' AND chvBeneficiaryGroupCode = 'PRIM' ")
                    If l_datarows_AnnuityBen.Length > 0 Then
                        l_datarow_AnnuityBen = l_datarows_AnnuityBen(0)
                        'SP : BT-976/YRS 5.0-1507 - ( if codition)
                        'TextboxBeneficiaryBirthDate.Text = CType(l_datarow_AnnuityBen("BenBirthDate") & "", String)
                        'TextboxLastName.Text = CType(l_datarow_AnnuityBen("BenLastName") & "", String)
                        'TextboxFirstName.Text = CType(l_datarow_AnnuityBen("BenFirstName") & "", String)

                        'SP : BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary
                        'Me.AddSelectedParticipantBeneficiary(l_datarows_AnnuityBen(0))
                        'DropDownRelationShip.SelectedValue = CType(l_datarow_AnnuityBen("chvRelationshipCode") & "", String).Trim()

                        'If (l_datarows_AnnuityBen.Length > 0) Then

                        '    While i < Me.DataGridAnnuityBeneficiaries.Items.Count
                        '        l_button_select = New ImageButton
                        '        l_button_select = Me.DataGridAnnuityBeneficiaries.Items(i).FindControl("ImageButtonAccounts")
                        '        If Me.DataGridAnnuityBeneficiaries.Items(i).Cells(1).Text.Trim().ToUpper() = l_datarow_AnnuityBen("guiUniqueID").ToString().ToUpper() Then
                        '            If Not l_button_select Is Nothing Then
                        '                l_button_select.ImageUrl = "images\selected.gif"
                        '            End If
                        '        Else
                        '            If Not l_button_select Is Nothing Then
                        '                l_button_select.ImageUrl = "images\select.gif"
                        '            End If

                        '        End If
                        '        i = i + 1
                        '    End While

                        'End If
                        'SP : BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary - End
                        Me.InputBeneficiaryType = BeneficiaryType.Spouse
                        'START: Added By SG: 2012.12.06: BT-1432
                    Else
                        'Look for spouse with 'CONTIGENT' Beneficiary Group
                        l_datarows_AnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvRelationshipCode = 'SP' AND chvBeneficiaryGroupCode = 'CONT' ")
                        If l_datarows_AnnuityBen.Length > 0 Then
                            l_datarow_AnnuityBen = l_datarows_AnnuityBen(0)
                            Me.InputBeneficiaryType = BeneficiaryType.Spouse
                        End If
                    End If

                    'Look for non spouse with other Beneficiary Group
                    If l_datarow_AnnuityBen Is Nothing Then
                        l_datarows_AnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg = 100 AND chvRelationshipCode NOT IN ('ES', 'IN', 'TR')")
                        If l_datarows_AnnuityBen.Length = 1 Then
                            l_datarow_AnnuityBen = l_datarows_AnnuityBen(0)
                            'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
                            Me.InputBeneficiaryType = BeneficiaryType.NonSpouse
                        Else
                            l_datarow_AnnuityBen = Nothing
                        End If
                    End If

                    'Commented By SG: 2013.01.10: BT-1432 (Re-Opened)
                    'If l_datarow_AnnuityBen Is Nothing Then
                    '    l_datarows_AnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvBeneficiaryGroupCode = 'CONT' AND intBenefitPctg = 100 AND chvRelationshipCode NOT IN ('ES', 'IN', 'TR')")
                    '    If l_datarows_AnnuityBen.Length = 1 Then
                    '        l_datarow_AnnuityBen = l_datarows_AnnuityBen(0)
                    '        'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
                    '        Me.InputBeneficiaryType = BeneficiaryType.NonSpouse
                    '    Else
                    '        l_datarow_AnnuityBen = Nothing
                    '    End If
                    'End If
                    'END: Added By SG: 2012.12.06: BT-1432

                    ''YRS 5.0-1329:J&S options available to non-spouse beneficiaries
                    ''Me.InputBeneficiaryType = BeneficiaryType.Spouse
                    'Else
                    '    'Look for primary beneficiary
                    '    'l_datarows_AnnuityBen = dsRetEstInfo.Tables(0).Select("chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg = 100") 'Phase IV Changes
                    '    l_datarows_AnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg = 100")
                    '    If l_datarows_AnnuityBen.Length > 0 Then
                    '        l_datarow_AnnuityBen = l_datarows_AnnuityBen(0)
                    '        'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
                    '        Me.InputBeneficiaryType = BeneficiaryType.NonSpouse
                    '    Else
                    '        l_datarow_AnnuityBen = Nothing
                    '    End If
                End If

                'Added By SG: 2012.12.06: BT-1432
                If Not l_datarow_AnnuityBen Is Nothing Then
                    'Start: Anudeep:08.02.2013 calling datagrid selected index change to show the l_datarow_AnnuityBen as selected -Bt-1688: YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
                    For i = 0 To DataGridAnnuityBeneficiaries.Items.Count - 1
                        If DataGridAnnuityBeneficiaries.Items(i).Cells(1).Text.Trim = l_datarow_AnnuityBen.Item("guiUniqueID").ToString().Trim Then
                            DataGridAnnuityBeneficiaries.SelectedIndex = i
                            Exit For
                        End If
                    Next
                    'Changed by Anudeep:12.02.2013 to put repeated code in a method 
                    showselectedbeneficary(l_datarow_AnnuityBen)
                    'Start:Commented by Anudeep:20.01.2013-Bt-1688: YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
                    'TextboxBeneficiaryBirthDate.Text = CType(l_datarow_AnnuityBen("BenBirthDate") & "", String)
                    'TextboxLastName.Text = CType(l_datarow_AnnuityBen("BenLastName") & "", String)
                    'TextboxFirstName.Text = CType(l_datarow_AnnuityBen("BenFirstName") & "", String)

                    'Me.AddSelectedParticipantBeneficiary(l_datarow_AnnuityBen)

                    'DropDownRelationShip.SelectedValue = CType(l_datarow_AnnuityBen("chvRelationshipCode") & "", String).Trim()

                    'If (l_datarows_AnnuityBen.Length > 0) Then

                    '    While i < Me.DataGridAnnuityBeneficiaries.Items.Count
                    '        l_button_select = New ImageButton
                    '        l_button_select = Me.DataGridAnnuityBeneficiaries.Items(i).FindControl("ImageButtonAccounts")
                    '        If Me.DataGridAnnuityBeneficiaries.Items(i).Cells(1).Text.Trim().ToUpper() = l_datarow_AnnuityBen("guiUniqueID").ToString().ToUpper() Then
                    '            If Not l_button_select Is Nothing Then
                    '                l_button_select.ImageUrl = "images\selected.gif"
                    '            End If
                    '        Else
                    '            If Not l_button_select Is Nothing Then
                    '                l_button_select.ImageUrl = "images\select.gif"
                    '            End If

                    '        End If
                    '        i = i + 1
                    '    End While

                    'End If
                    'End:Commented by Anudeep:20.01.2013-Bt-1688: YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
                    'End: Anudeep:08.02.2013 calling datagrid selected index change to show the l_datarow_AnnuityBen as selected 'Start: Anudeep:08.02.2012 Changes made to add active persons in relationship dropdown along with selected beneficary relationship -Bt-1688: YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
                End If
                'Added By SG: 2012.12.06: BT-1432

                'Look for non spouse with multiple beneficiary
                ''SP : 2013.11.12 BT-2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee (adding andalso condition)
                If Not l_datarows_AnnuityBen Is Nothing AndAlso l_datarows_AnnuityBen.Length > 0 Then 'ThenOr l_datarows_AnnuityBen.Length > 0 

                Else
                    'TextboxLastName.Text = ""
                    'TextboxFirstName.Text = ""
                    'TextboxBeneficiaryBirthDate.Text = ""
                    'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
                    Me.InputBeneficiaryType = BeneficiaryType.Manual

                    'SG: 2012.12.10: BT-1426:
                    If l_datarow_AnnuityBen Is Nothing Then
                        ''SP : 2013.11.12 BT-2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee  -start  
                        If (Not dsParticipantBeneficiaries Is Nothing AndAlso HelperFunctions.isNonEmpty(dsParticipantBeneficiaries.Tables(0))) Then
                            If (dsParticipantBeneficiaries.Tables(0).Rows.Count = 1) Then
                                l_datarows_AnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvBeneficiaryGroupCode = 'PRIM' AND BenLastName ='*Estate' AND  chvRelationshipCode ='ES'")
                                If l_datarows_AnnuityBen.Length = 1 Then
                                    Session("BeneficiaryName") = CType(l_datarows_AnnuityBen(0)("BenFirstName") & "", String) + " " + CType(l_datarows_AnnuityBen(0)("BenLastName") & "", String)
                                Else
                                    Session("BeneficiaryName") = Nothing
                                End If
                            End If
                        Else
                            Session("BeneficiaryName") = "*Estate"

                        End If
                        ''SP : 2013.11.12 BT-2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee  -End

                        ''SP : 2013.11.12 BT-2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee  -start -Commented 
                        'l_datarows_AnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg = 100 AND chvRelationshipCode IN ('ES')")
                        'If l_datarows_AnnuityBen.Length = 1 Then
                        '	Session("BeneficiaryName") = CType(l_datarows_AnnuityBen(0)("BenFirstName") & "", String) + " " + CType(l_datarows_AnnuityBen(0)("BenLastName") & "", String)
                        'Else
                        '	Session("BeneficiaryName") = Nothing
                        'End If
                        ''SP : 2013.11.12 BT-2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee  -End -Commented 
                    End If
                    'END: SG: 2012.12.10: BT-1426:
                End If

            End If


            'Priya 05-Jan-2010 YRS 5.0-983 commented code to disable text boxes
            'TextboxLastName.Enabled = Not l_bool_RetirementIsBackDated
            'TextboxFirstName.Enabled = Not l_bool_RetirementIsBackDated
            ''TextboxBeneficiaryBirthDate.Enabled = Not l_bool_RetirementIsBackDated
            'Popcalendar3.Enabled = Not l_bool_RetirementIsBackDated
            ''End 12-Feb-09
            'END 05-Jan-2010 YRS 5.0-983


            'If Page.IsPostBack = False Then
            '    'dsRetEstInfo = RetirementBOClass.SearchRetEstInfo(dsRetireeEstimate.Tables(0).Rows(0).Item("guiUniqueID").ToString())
            '    dsParticipantBeneficiaries = RetirementBOClass.getParticipantBeneficiaries(personId)

            '    'Session("dsRetEstInfo") = dsRetEstInfo
            '    Session("dsParticipantBeneficiaries") = dsParticipantBeneficiaries

            '    'If dsRetEstInfo.Tables(0).Rows.Count > 0 Then 'Phase IV Changes
            '    If dsParticipantBeneficiaries.Tables(0).Rows.Count > 0 Then
            '        ' Look for spouse
            '        'l_datarows_AnnuityBen = dsRetEstInfo.Tables(0).Select("chvRelationshipCode = 'SP'") 'Phase IV Changes
            '        l_datarows_AnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvRelationshipCode = 'SP'")

            '        If l_datarows_AnnuityBen.Length > 0 Then
            '            l_datarow_AnnuityBen = l_datarows_AnnuityBen(0)
            '        Else
            '            'Look for primary beneficiary
            '            'l_datarows_AnnuityBen = dsRetEstInfo.Tables(0).Select("chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg = 100") 'Phase IV Changes
            '            l_datarows_AnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg = 100")

            '            If l_datarows_AnnuityBen.Length > 0 Then
            '                l_datarow_AnnuityBen = l_datarows_AnnuityBen(0)
            '            Else
            '                l_datarow_AnnuityBen = Nothing
            '            End If
            '        End If

            '        If l_datarows_AnnuityBen.Length > 0 Then
            '            l_datarow_AnnuityBen = l_datarows_AnnuityBen(0)
            '            TextboxLastName.Text = CType(l_datarow_AnnuityBen("BenLastName") & "", String)
            '            TextboxFirstName.Text = CType(l_datarow_AnnuityBen("BenFirstName") & "", String)
            '            TextboxBeneficiaryBirthDate.Text = CType(l_datarow_AnnuityBen("BenBirthDate") & "", String)
            '        Else
            '            TextboxLastName.Text = ""
            '            TextboxFirstName.Text = ""
            '            TextboxBeneficiaryBirthDate.Text = ""
            '        End If
            '    End If

            '    l_bool_RetirementIsBackDated = IsRetirementBackDated()

            '    TextboxLastName.Enabled = Not l_bool_RetirementIsBackDated
            '    TextboxFirstName.Enabled = Not l_bool_RetirementIsBackDated
            '    TextboxBeneficiaryBirthDate.Enabled = Not l_bool_RetirementIsBackDated
            'End If
        Catch
            Throw
        End Try
    End Sub

    Private Function LoadPersonDetails(ByVal SSNO As String) As String
        Dim dsPerson As New DataSet
        Dim drPerson As DataRow

        'Loading dataset with Basic Retiree information
        dsPerson = RetirementBOClass.SearchRetireeInfo(SSNO)
        If dsPerson.Tables.Count > 0 Then
            If dsPerson.Tables(0).Rows.Count > 0 Then
                drPerson = dsPerson.Tables(0).Rows(0)
            End If
        End If

        'Set the label text to display the header of this page
        Dim strSSN As String = SSNO
        Headercontrol.PageTitle = "Retirement Estimate"
        Headercontrol.SSNo = SSNO.Trim
        '------------------------------------------------------------------------------------------------------------

        'Loading Employee and Beneficiary information into respective fields
        If drPerson("dtmBirthDate").ToString() <> String.Empty Then
            TextBoxRetireeBirthday.Text = Convert.ToDateTime(drPerson("dtmBirthDate")).ToString("MM/dd/yyyy")
        End If

        Dim l_string_PersId As String = drPerson("guiUniqueID").ToString()

        Return l_string_PersId

    End Function
    Private Sub SetSocialSecurityDetails()
        Dim dtSocialSecurity As New DataTable
        dtSocialSecurity.Columns.Add("Annuity")
        dtSocialSecurity.Columns.Add("Retiree")
        dtSocialSecurity.Columns.Add("Before62")
        dtSocialSecurity.Columns.Add("After62")
        dtSocialSecurity.Columns.Add("Survivor")
        dtSocialSecurity.Columns.Add("Beneficiary")
        dtSocialSecurity.AcceptChanges()

        Me.DatatGridSocialSecurityLevel.DataSource = dtSocialSecurity
        Me.DatatGridSocialSecurityLevel.DataBind()
        'Added by Ashish for Phase v changes
        txtProjectedReserves.Text = "0.00"
        txtDeathBenefitUsed.Text = "0.00"
        LabelSSLWarningMessage.Visible = False
        'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
        LabelJAnnuityUnAvailMessage.Visible = False

    End Sub
    'ASHISH:2011.08.24 Added new property for YRS 5.0-1135
    'BT-798 : System should not allow disability retirement for QD and BF fundevents
    Private Function IsOrgBeneTypeQDROorRBEN() As Boolean
        Dim l_dataset_EligibleFundEvent As DataSet
        Dim lcRetireType As String = ""
        Dim l_string_BenType As String = ""
        Dim l_bool_return As Boolean = False

        lcRetireType = Me.RetireType
        If Not Me.FundEventId Is Nothing Then
            l_dataset_EligibleFundEvent = RetirementBOClass.EligibleFundEvent("ESTIMATE", lcRetireType, Me.FundEventId)
            If Not l_dataset_EligibleFundEvent.Tables.Count = 0 Then
                If Not l_dataset_EligibleFundEvent.Tables(0).Rows.Count = 0 Then
                    If Not l_dataset_EligibleFundEvent.Tables(0).Rows(0)("chrOrigBeneType") Is System.DBNull.Value Then
                        l_string_BenType = l_dataset_EligibleFundEvent.Tables(0).Rows(0)("chrOrigBeneType")
                    End If

                    'BT-798 : System should not allow disability retirement for QD and BF fundevents
                    'If l_string_BenType.Trim().ToUpper() = "QDRO" Then
                    If l_string_BenType.Trim().ToUpper() = "QDRO" Or l_string_BenType.Trim.ToUpper() = "RBEN" Then
                        l_bool_return = True
                    Else
                        l_bool_return = False
                    End If
                End If
            End If
        End If

        Return l_bool_return
    End Function
    Private Sub PopulateActiveEmployments(ByVal dsEmpDetails As DataSet)
        Dim l_ListItem As ListItem
        Dim i As Integer
        Dim l_datatable_empdetails As DataTable
        Dim l_datarow_empdetail As DataRow

        If dsEmpDetails.Tables.Count > 0 Then
            DropDownListEmployment.Items.Clear()
            l_ListItem = New ListItem
            l_ListItem.Text = "---Select Employment---"
            l_ListItem.Value = ""
            DropDownListEmployment.Items.Add(l_ListItem)

            l_datatable_empdetails = dsEmpDetails.Tables(0)
            For Each l_datarow_empdetail In l_datatable_empdetails.Rows
                If l_datarow_empdetail("End").ToString() = String.Empty Then

                    l_ListItem = New ListItem

                    l_ListItem.Text = l_datarow_empdetail("YMCA").ToString() & " (" & _
                    Convert.ToDateTime(l_datarow_empdetail("Start")).ToString("MM/dd/yyyy").Substring(0, 2) & _
                    "/" & Convert.ToDateTime(l_datarow_empdetail("Start").ToString()).Year() & ")"

                    l_ListItem.Value = l_datarow_empdetail("guiEmpEventId").ToString()

                    DropDownListEmployment.Items.Add(l_ListItem)
                End If
            Next
        End If
    End Sub
    'Ashish for V changes ,This function is not in used 
    Private Sub displayAccountBalancesAsPerRefunds()
        Dim dsElectiveAccounts As DataSet
        Dim dtElectiveAccounts As DataTable
        Dim excludedDataTable As DataTable

        Dim dtModifiedElectiveAccounts As DataTable
        Dim dsModifiedElectiveAccounts As New DataSet

        Dim l_bool_BasicAccountExcluded As Boolean
        Dim empEventId As String = String.Empty
        Dim i As Integer
        Try
            If Not Session("dsElectiveAccountsDet") Is Nothing Then
                'Ashish for testing for phase V Start
                'dsElectiveAccounts = Session("dsElectiveAccountsDet")
                'dtElectiveAccounts = dsElectiveAccounts.Tables(0)

                'excludedDataTable = getExcludedAccounts()

                'dtModifiedElectiveAccounts = removeExcludedAccounts(dtElectiveAccounts, excludedDataTable)

                'dsModifiedElectiveAccounts.Tables.Add(dtModifiedElectiveAccounts)
                'Session("dsElectiveAccountsDet") = dsModifiedElectiveAccounts
                'Ashish for testing for phase V End
                If DropDownListEmployment.Items.Count > 1 Then
                    empEventId = DropDownListEmployment.Items(1).Value.ToString.Trim.ToUpper
                End If

                populateElectiveAccountsTab(empEventId, True, False)


            End If
        Catch
            Throw
        End Try
    End Sub
    'Ashish For phase V changes ,this function is not in used 
    Private Function removeExcludedAccounts(ByVal dtAccountBalance As DataTable, ByVal dtExcludedAccounts As DataTable) As DataTable
        Dim dtExcludeFromThis As DataTable = dtAccountBalance.Copy()
        Dim boolBasicAccountExcluded As Boolean = False
        Dim PlanTypeColumnName As String
        Dim YMCATotalColumnName As String
        Dim PersonalTotalColumnName As String
        Dim BalanceTotalColumnName As String

        If dtExcludeFromThis.Columns.Contains("PlanType") = True Then
            PlanTypeColumnName = "PlanType"
        Else
            PlanTypeColumnName = "chvPlanType"
        End If

        If dtExcludeFromThis.Columns.Contains("YMCATotal") = True Then
            YMCATotalColumnName = "YMCATotal"
        Else
            YMCATotalColumnName = "YMCAAmt"
        End If

        If dtExcludeFromThis.Columns.Contains("PersonalTotal") = True Then
            PersonalTotalColumnName = "PersonalTotal"
        Else
            PersonalTotalColumnName = "PersonalAmt"
        End If

        If dtExcludeFromThis.Columns.Contains("AcctTotal") = True Then
            BalanceTotalColumnName = "AcctTotal"
        Else
            BalanceTotalColumnName = "mnyBalance"
        End If


        If dtExcludedAccounts.Rows.Count > 0 Then
            For Each dr As DataRow In dtExcludedAccounts.Rows
                For Each drProj As DataRow In dtExcludeFromThis.Rows
                    If drProj("chrAcctType").ToString() = dr("chrAcctType").ToString() Then
                        'checking if the account excluded by user is a basic account and plan type is retirement 
                        If drProj(PlanTypeColumnName).ToString() = "RETIREMENT" AndAlso Convert.ToBoolean(dr("bitBasicAcct")) = True Then
                            'then exclude only the personal side of money for the excluded basic account. 
                            drProj(BalanceTotalColumnName) = drProj(YMCATotalColumnName)

                            'flagging if any single basic account is excluded from the estimate 
                            boolBasicAccountExcluded = True
                        Else
                            'drProj(BalanceTotalColumnName) = 0
                        End If
                    Else
                        drProj(BalanceTotalColumnName) = drProj(YMCATotalColumnName) + drProj(PersonalTotalColumnName)
                    End If
                Next
            Next
        Else
            For Each drProj As DataRow In dtExcludeFromThis.Rows
                'checking if the account excluded by user is a basic account and plan type is retirement                 
                If drProj(PlanTypeColumnName).ToString() = "RETIREMENT" Then
                    'then exclude only the personal side of money for the excluded basic account. 
                    drProj(BalanceTotalColumnName) = drProj(YMCATotalColumnName) + drProj(PersonalTotalColumnName)
                End If
            Next
        End If

        'if any single basic account is excluded from the estimate then 
        'exclude personal side of money from all the basic accounts 
        'and both personal as well as ymca side of money from non-basic account under retirement plan. 
        If boolBasicAccountExcluded = True Then
            'this dataset will be populated with only the basic accounts 
            Dim l_dsMetaAccountTypes As DataSet = RetirementBOClass.SearchMetaAccountTypes()

            For Each drProj As DataRow In dtExcludeFromThis.Rows
                If drProj(PlanTypeColumnName).ToString() = "RETIREMENT" Then

                    'checking if the account is a basic account then excluding only the personal otherwise personal as well as ymca side of money is excluded from the account. 
                    Dim filterExpression As String = "chrAcctType='" + drProj("chrAcctType").ToString() + "'"

                    If l_dsMetaAccountTypes.Tables(0).[Select](filterExpression).Length > 0 Then

                        'basic account 
                        drProj(BalanceTotalColumnName) = drProj(YMCATotalColumnName)

                    Else

                        'non-basic account 
                        drProj(BalanceTotalColumnName) = 0

                    End If
                End If
            Next

            If Me.PlanType = "R" Then
                'LabelRefundMessage.Text = "Annuity Estimate will be based upon YMCA Account funds only."
                LabelRefundMessage.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_ANNUITY_BASED_ON_YMCA")
            ElseIf Me.PlanType = "B" Then
                'LabelRefundMessage.Text = "Annuity Estimate will be based upon YMCA Account plus Savings Plan funds."
                LabelRefundMessage.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_ANNUITY_BASED_ON_YMCA_BOTH_PLANS")
            End If

            LabelRefundMessage.Visible = True
        Else
            LabelRefundMessage.Visible = False
            LabelRefundMessage.Text = ""
        End If

        dtExcludeFromThis.AcceptChanges()

        Return dtExcludeFromThis
    End Function
#End Region


    Private Sub HandleMessageBox(ByVal ButtonName As String)
        Select Case ButtonName
            Case "PaidServiceCheck"
                If Request.Form("Yes") = "Yes" Then
                    Session("MessageBox") = "Purchase"
                    'commented by Anudeep on 22-sep for BT-1126
                    'ShowCustomMessage("Do you wish to purchase this annuity ?", enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
                    ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_ESTIMATE_PURCHASE_ANNUITY"), enumMessageBoxType.DotNet, MessageBoxButtons.YesNo)
                    Exit Sub
                End If
        End Select
        Session("MessageBox") = Nothing
    End Sub

#Region "Event Handlers"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dsRetEmpInfo As New DataSet
        Dim dsRetEmpSalInfo As New DataSet
        Dim dsElectiveAccounts As New DataSet
        Dim dsActiveEmploymentEvent As New DataSet
        Dim i As Integer

        Try
            Me.Server.ScriptTimeout = 90000000

            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If

            If Me.FundEventId Is Nothing Then
                Response.Redirect("FindInfo.aspx?Name=Estimates", False)
            End If

            If Session("EmpHistoryInfoNotset") = True Then
                Session("EmpHistoryInfoNotset") = False
                Session("EmpHistoryInfoNotset") = Nothing
                Response.Redirect("FindInfo.aspx?Name=Estimates", False)
            End If

            'Redirectingto FindInfo page.If retiree birth date not present
            If Not Session("RetireeBirthDatePresent") Is Nothing Then
                If Me.RetireeBirthDatePresent = False Then
                    Session("RetireeBirthDatePresent") = Nothing
                    Response.Redirect("FindInfo.aspx?Name=Estimates", False)
                End If
            End If

            ' If not active plan is there then return to FindInfo page
            If Session("NoActivePlans") = True Then
                Session("NoActivePlans") = False
                Response.Redirect("FindInfo.aspx?Name=Estimates", False)
            End If

            ' If Sufficient funds are not there take user directed action.
            If Page.IsPostBack = True Then
                If Session("ProceedWithEstimation") = True Then
                    If Request.Form("Yes") = "Yes" Then
                        ' Set the correct plan type
                        Me.PlanType = Session("UsePlan")
                        If Me.PlanType = "S" Then
                            Me.DropDownListPlanType.SelectedValue = "Savings"
                        Else
                            Me.DropDownListPlanType.SelectedValue = "Retirement"
                        End If

                        DropDownListPlanType_SelectedIndexChanged(New Object, New EventArgs)
                        ValidateProjectedBalancesAsPerRefund()

                        ' Calculate the annuity from start
                        Dim dsRetEstimateEmployment As DataSet

                        'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                        'dsRetEstimateEmployment = RetirementBOClass.getEmploymentDetails(Me.FundEventId, "NORMAL", TextBoxRetirementDate.Text)

                        ' If Not dsRetEstimateEmployment Is Nothing Then
                        If Session("Print") = "Print" Then
                            'Ashish:2010.06.21 YRS 5.0-1115 
                            If Not Session("RE_PRAType") Is Nothing Then
                                Call PrintEstimate(CType(Session("RE_PRAType"), String))
                            End If
                        Else
                            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                            'Call EstimateAnnuities(dsRetEstimateEmployment)
                            Call EstimateAnnuities()
                        End If
                        tabStripRetirementEstimate.SelectedIndex = 0
                        Me.MultiPageRetirementEstimate.SelectedIndex = Me.tabStripRetirementEstimate.SelectedIndex
                        ' End If
                        LabelEstimateDataChangedMessage.Visible = False
                        Exit Sub

                    End If
                    LabelEstimateDataChangedMessage.Visible = False

                End If
                'start- Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                If Request.Form("Proceed") = "Proceed" Then
                    Me.AreJAnnuitiesUnavailable = False
                    If Session("Print") = "Print" Then
                        If TextBoxPraAssumption.Visible = False Then
                            labelPRAAssumption.Visible = True
                            TextBoxPraAssumption.Visible = True
                            ButtonPrint.Enabled = False
                            Buttoncalculate.Enabled = False
                            tabStripRetirementEstimate.Enabled = False

                            tblIDMcheck.Visible = True
                            chkIDM.Visible = True
                            chkIDM.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_COPY_TO_IDM")
                            chkIDM.Checked = False
                            If Me.RetireType = "DISABL" Then
                                Panel1.Visible = False
                                PanelDisability.Visible = True
                            Else
                                PanelDisability.Visible = False
                                Panel1.Visible = True
                            End If
                            If (Me.OrgBenTypeIsQDROorRBEN = True) Then
                                Panel1.Visible = False
                                PanelDisability.Visible = False
                                PanelAlternatePayee.Visible = True
                            End If
                            'Anudeep:01.04.2013:Checking whether empty report name exists.. if exists draft full form button will not be visible
                            If System.Configuration.ConfigurationSettings.AppSettings("RET_EST_DraftFullForm") = "" Then
                                ButtonPRAFull.Visible = False
                            End If
                            ''----------------------------------------
                            Exit Sub
                        End If
                    End If
                    LabelJAnnuityUnAvailMessage.Visible = False
                    'Start - Chandra sekar.c  2016.07.19  YRS-AT-3010 - YRS bug-annuity estimate calculator not counting partial withdrawal against estimates(TrackIT 26458)
                    If GetProjectedAcctBalances() = False Then
                        SetSocialSecurityDetails()
                        Exit Sub
                    End If
                    If Not SetElectiveAccountSelectionState() Then
                        ResetPartialSavingGrid()
                        ResetRetireePartialGrid()
                        Exit Sub
                    End If
                    'End- Chandra sekar.c  2016.07.19  YRS-AT-3010 - YRS bug-annuity estimate calculator not counting partial withdrawal against estimates(TrackIT 26458)

                    If ValidateEstimate(False) Then
                        Me.UpdateSelectedParticiapntBeneficary()
                        Call EstimateAnnuities()
                    End If
                    tabStripRetirementEstimate.SelectedIndex = 0
                    Me.MultiPageRetirementEstimate.SelectedIndex = Me.tabStripRetirementEstimate.SelectedIndex

                    LabelEstimateDataChangedMessage.Visible = False
                    Exit Sub

                End If
                'End- Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                'LabelMultipleEmpExists.Visible = False
            End If '--If Page.IsPostBack = True

            SetControlAttributes()  'newly added method by hafiz on 6-Dec-2006 for optimization - YREN-2814


            Dim activePlan As String
            Dim empEventId As String = String.Empty
            If Page.IsPostBack = False Then
                Session("ProceedWithJSOptionEstimationQDRO") = "Initial" 'Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                ExactAgeEffDate = RetirementBOClass.GetExactAgeEffectiveDate()
                businessLogic = New RetirementBOClass
                Session("businessLogic") = businessLogic

                'added by hafiz on 25-Jan-2007 for resetting the session variable
                Session("Print") = Nothing
                Me.RetireType = Nothing
                Session("PercentageSelected") = Nothing
                Session("mnyDeathBenefitAmount") = Nothing
                Session("StrReportName") = Nothing
                Session("GUID") = Nothing
                TextboxFromBenefitValue.Text = "0.00"
                Me.ManualTransactionDetails = Nothing 'MMR | 2017.03.03 | YRS-AT-2625 | Clearing session property on initial page load

                'Loading dataset with Basic Retiree information
                Me.PersonId = LoadPersonDetails(Me.SSNO)

                ' Get the list of Active plans
                activePlan = RetirementBOClass.GetActivePlansForEstimate(Me.PersonId, Me.FundEventId, Me.FundEventStatus) ', previousfundStatus)
                If activePlan = String.Empty Then
                    Session("NoActivePlans") = True
                    'commented by Anudeep on 22-sep for BT-1126
                    'Call ShowCustomMessage("Not active plans to retire with.", enumMessageBoxType.DotNet)
                    Call ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_ESTIMATE_NOT_ACTIVE_PLANS"), enumMessageBoxType.DotNet)
                    Exit Sub
                Else
                    Me.setPlanType(activePlan)
                End If

                ' Check if the FundEventStatus is either RT or RD 
                'as per the requirement given by Mark through mail dated dt:Monday, May 19, 2008 7:20 PM.
                'If Me.FundEventStatus = "RT" Then
                If Me.FundEventStatus = "RT" Or Me.FundEventStatus = "RPT" Then
                    ' Then get the annuity type that he has bought previously  -- Get the details from AtsAnnuities table.
                    Dim dtPurchasedAnnuity As DataTable = businessLogic.GetPurchasedAnnuityDetails(Me.PersonId)
                    ' Check if SS anuity has been bought or not
                    If dtPurchasedAnnuity.Rows.Count > 0 Then
                        For Each dr As DataRow In dtPurchasedAnnuity.Rows
                            If dr("SSLeveling").ToString.Trim = True Then
                                ssAnnuityAlreadyBought = True
                            End If
                        Next
                    End If
                    TextboxFromBenefitValue.Enabled = (ssAnnuityAlreadyBought = False)

                    'DatagridElectiveRetirementAccounts.Enabled = False
                    'DatagridElectiveSavingsAccounts.Enabled = False
                End If

                Call SetRetireType()

                'Loading default values for Interest Rates
                LoadDefaultValues()

                'Commented by  chandresekar.c YRS-Ticket 2610 Death Benefit  Annuity Purchase Restricted 
                'Me.PersonId = LoadPersonDetails(Me.SSNO)

                'Check retiree birth day present or not
                If TextBoxRetireeBirthday.Text <> String.Empty Then
                    Me.RetireeBirthDatePresent = True
                Else
                    'commented by Anudeep on 22-sep for BT-1126
                    ' ShowCustomMessage("Retiree Birth Date is missing.", enumMessageBoxType.DotNet, MessageBoxButtons.Stop)
                    ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREE_BIRTH_DATE"), enumMessageBoxType.DotNet, MessageBoxButtons.Stop)
                    Me.RetireeBirthDatePresent = False
                    Exit Sub
                End If

                'End


                'Session("OrgBenTypeIsQDRO") = IsOrgBeneTypeQDRO()
                'ASHISH:2011.08.29 YRS 5.0-1345 :BT-859 disable retirement type and death benift controls selection for QD participant 
                'BT-798 : System should not allow disability retirement for QD and BF fundevents
                Me.OrgBenTypeIsQDROorRBEN = IsOrgBeneTypeQDROorRBEN()

                If (Me.OrgBenTypeIsQDROorRBEN = True) Then
                    DropDownListRetirementType.Enabled = False
                    DropdownlistPercentageToUse.Enabled = False
                    TextboxAmountToUse.Enabled = False
                End If


                If Me.PersonId.ToString() <> String.Empty Then
                    'Dinesh Kanojia(DK):2013.01.30:BT-1262: YRS 5.0-1697: Message to display if multiple active employments Exist.
                    If LoadEmploymentDetails(Me.PersonId) Then
                        LabelMultipleEmpExists.Text = getmessage("MESSAGE_REIREMENT_ESTIMATE_MULTIPLE_ACTIVE_EMPLOYMENTS_EXISTS")
                        LabelMultipleEmpExists.Visible = True
                    Else
                        LabelMultipleEmpExists.Visible = False
                    End If

                    ' LoadEmploymentDetails(Me.PersonId)

                    'Retirement Date
                    TextBoxRetirementDate.BackColor = Color.LightYellow

                    'Retirement Age
                    TextBoxRetirementAge.BackColor = Color.LightYellow
                Else
                    'participant details could not be fetched from database / invalid ssno passed.
                    Exit Sub
                End If 'Me.PersonId.ToString() <> String.Empty

                'SP : 2012.05.21  : BT-976/YRS 5.0-1507 - Reopned issue
                Session("dsSelectedParticipantBeneficiary") = Nothing

                'SP : 2012.04.12  : BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary -Start 
                If Me.PersonId.ToString() <> String.Empty And Not (Page.IsPostBack) Then
                    Me.BindBeneficiaryDetails(Me.PersonId)
                    Me.BindRelationShipDropDown()
                End If
                'SP : 2012.04.12  : BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary  -End

                'SG: 2012.12.10: BT-1426:
                Session("BeneficiaryName") = Nothing

                LoadBeneficiaryDetails(Me.PersonId) 'Phase IV Changes

                SetSocialSecurityDetails()

                Me.Menu1.DataSource = Server.MapPath("SimpleXML.xml")
                Menu1.DataBind()



                ' Populate the accounts tab
                'dsElectiveAccountsDet = RetirementBOClass.GetElectiveAccountsByPlan(Me.PersonId, Me.PlanType)
                'Commented by Ashish 15-Apr-2009 for Phase V changes, Start

                'dsElectiveAccountsDet = RetirementBOClass.GetElectiveAccountsByPlan(Me.FundEventId, Me.PlanType, Me.RetirementDate)

                'Dim workCol As DataColumn = dsElectiveAccountsDet.Tables(0).Columns.Add("Selected", Type.GetType("System.Boolean"))
                'workCol.DefaultValue = False
                'For Each dr As DataRow In dsElectiveAccountsDet.Tables(0).Rows
                '    If dr("AcctTotal") > 0 Then
                '        dr("Selected") = True
                '    Else
                '        dr("Selected") = False
                '    End If
                'Next
                'dsElectiveAccountsDet.AcceptChanges()

                'Session("dsElectiveAccountsDet") = dsElectiveAccountsDet
                'Commented by Ashish 15-Apr-2009 for Phase V changes, End

                'Added by Ashish 15-Apr-2009 Phase V changes, Start
                'This dataset contains two tables 1.RetireeElectiveAccountsInformation 2.RetireeGroupedElectiveAccounts
                '1.RetireeElectiveAccountsInformation contains individual accounts details
                '2.RetireeGroupedElectiveAccounts contains grouped accounts(Personal,YMCA,Employer Paid) and other account details
                dsElectiveAccountsDet = GetElectiveAccoutsByPlan(Me.FundEventId, Me.PlanType, Me.RetirementDate)
                If Not dsElectiveAccountsDet Is Nothing Then
                    If dsElectiveAccountsDet.Tables.Count > 1 Then
                        Session("dsElectiveAccountsDet") = dsElectiveAccountsDet
                    End If
                End If

                'LabelEstimateDataChangedMessage.Text = "Data for estimate has changed.Please re-calculate."
                'START: SB | 03/14/2017 | YRS-AT-2625 | Function to display the warning estimate message or not after checking for withdrawals taken or not
                WarnUserToRecalculate()
                ' LabelEstimateDataChangedMessage.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_DATA_HAS_CHANGED")     ' Old Code commented replaced by WarnUserToReclaculate()
                'END: SB | 03/14/2017 | YRS-AT-2625 | Function to display the warning estimate message or not after checking for withdrawals taken or not

                LabelEstimateDataChangedMessage.Visible = False
                'Added by Ashish 08-Apr-2009 Phase V changes, End 

                'Priya 12-Feb-09 YRS 5.0-620 Incorrect message for person with status RA
                Dim decRetirementTotal As Decimal = 0
                Dim decSavingsTotal As Decimal = 0
                If (isNonEmpty(dsElectiveAccountsDet)) Then
                    'commented by Ashish 15-Apr-2009 for phase V changes, Start
                    ''check for retirement plan value into dataset
                    'If Not IsDBNull(dsElectiveAccountsDet.Tables(0).Compute("SUM(AcctTotal)", " PlanType = 'RETIREMENT'")) Then

                    '    decRetirementTotal = Convert.ToDecimal(Convert.ToString(dsElectiveAccountsDet.Tables(0).Compute("SUM(AcctTotal)", " PlanType = 'RETIREMENT'")).Trim)
                    'End If
                    ''check for savings plan  value into dataset
                    'If Not IsDBNull(dsElectiveAccountsDet.Tables(0).Compute("SUM(AcctTotal)", " PlanType = 'SAVINGS'")) Then
                    '    decSavingsTotal = Convert.ToDecimal(Convert.ToString(dsElectiveAccountsDet.Tables(0).Compute("SUM(AcctTotal)", " PlanType = 'SAVINGS'")).Trim)
                    'End If
                    'commented by Ashish 15-Apr-2009 for phase V changes, End

                    'Added by Ashish 15-Apr-2009 for phase V changes, Start
                    'check for retirement plan value into dataset
                    If Not IsDBNull(dsElectiveAccountsDet.Tables("RetireeElectiveAccountsInformation").Compute("SUM(AcctTotal)", " PlanType = 'RETIREMENT'")) Then
                        decRetirementTotal = Convert.ToDecimal(Convert.ToString(dsElectiveAccountsDet.Tables("RetireeElectiveAccountsInformation").Compute("SUM(AcctTotal)", " PlanType = 'RETIREMENT'")).Trim)
                    End If
                    'check for savings plan  value into dataset
                    If Not IsDBNull(dsElectiveAccountsDet.Tables("RetireeElectiveAccountsInformation").Compute("SUM(AcctTotal)", " PlanType = 'SAVINGS'")) Then
                        decSavingsTotal = Convert.ToDecimal(Convert.ToString(dsElectiveAccountsDet.Tables("RetireeElectiveAccountsInformation").Compute("SUM(AcctTotal)", " PlanType = 'SAVINGS'")).Trim)
                    End If
                    'Added by Ashish 15-Apr-2009 for phase V changes, End
                End If

                If (decRetirementTotal > 0 And decSavingsTotal <= 0) Then
                    If isNonEmpty(DropDownListPlanType.Items.FindByValue("Retirement")) Then
                        'DropDownListPlanType.SelectedValue = "Retirement"
                        DropDownListPlanType.SelectedIndex = DropDownListPlanType.Items.IndexOf(DropDownListPlanType.Items.FindByValue("Retirement"))
                        activePlan = "R"
                    End If
                ElseIf (decRetirementTotal <= 0 And decSavingsTotal > 0) Then
                    If isNonEmpty(DropDownListPlanType.Items.FindByValue("Savings")) Then
                        'DropDownListPlanType.SelectedValue = "Savings"
                        DropDownListPlanType.SelectedIndex = DropDownListPlanType.Items.IndexOf(DropDownListPlanType.Items.FindByValue("Savings"))
                        activePlan = "S"
                    End If
                Else
                    If isNonEmpty(DropDownListPlanType.Items.FindByValue("Both")) Then
                        'DropDownListPlanType.SelectedValue = "Both"
                        DropDownListPlanType.SelectedIndex = DropDownListPlanType.Items.IndexOf(DropDownListPlanType.Items.FindByValue("Both"))
                        activePlan = "B"
                    End If
                End If
                '04-March-09 Call function to apply functionality of selcted plan type 
                SelectedListPlanType()
                'End 04-March-09
                'DropDownListPlanType_SelectedIndexChanged(New Object, New EventArgs)
                'End 12-Feb-09


                'Dim dsBasicAccounts As DataSet = RetirementBOClass.GetBasicAccountsByPlan(Me.FundEventId)
                'Session("dsBasicAccounts") = dsBasicAccounts

                'CheckBoxSavingsPlan.Visible = False

                If DropDownListEmployment.Items.Count > 2 Then
                    LabelMultipleEmp.Visible = True
                End If

                If DropDownListEmployment.Items.Count > 1 Then
                    empEventId = DropDownListEmployment.Items(1).Value.ToString.Trim.ToUpper
                End If

                Me.populateElectiveAccountsTab(empEventId, True, False)





                'Added by Ashish for phase V changes 
                'ValidateProjectedBalancesAsPerRefund()
                'SetElectiveAccountSelectionState()

                Session("isYmcaLegacyAcctTotalExceed") = Nothing
                Session("isYmcaAcctTotalExceed") = Nothing
                ShowProjectedAcctBalancesTotal()
                disableDeathBenefitControls(activePlan)
                ValidateAndShowRDBWarninMessage(activePlan) ' BD : 2018.11.01 : YRS-AT-4133 - No Active Death Benefit if first enrolled on or after 1/1/2019
                LabelEstimateDataChangedMessage.Visible = False

                'Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request
                tblIDMcheck.Visible = False
            End If      'postback = false


            If Page.IsPostBack = True Then
                If Me.RetirementDate <> TextBoxRetirementDate.Text.ToString() Then

                    Me.RetirementDate = TextBoxRetirementDate.Text.ToString()

                    LoadBeneficiaryDetails(Me.PersonId)

                    activePlan = DropDownListPlanType.SelectedValue.ToString.Substring(0, 1)
                    disableDeathBenefitControls(activePlan)
                    ValidateAndShowRDBWarninMessage(activePlan) ' BD : 2018.11.01 : YRS-AT-4133 - No Active Death Benefit if first enrolled on or after 1/1/2019

                    'populateElectiveAccountsTab(empEventId, True)
                    setAccountGridControls()

                    setEmploymentControlState()

                    'DataGridEmployment_SelectedIndexChanged(New Object, New EventArgs)
                    'START: MMR | 2017.03.14 | YRS-AT-2625 | Commented existing code and added to display average salary for Disability retirement on change of retirement date and hide manual transaction link if retirement date is empty
                    ''START: MMR | 2017.03.14 | YRS-AT-2625 | Added to display average salary for Disability retirement on change of retirement date
                    'If Me.RetireType = "DISABL" Then
                    '    LoadEmploymentDetails(Me.PersonId)
                    'End If
                    ''END: MMR | 2017.03.14 | YRS-AT-2625 | Added to display average salary for Disability retirement on change of retirement date
                    'LoadManualTransactionForDisability(Me.FundEventId, Me.RetireType, TextBoxRetirementDate.Text.Trim())  'MMR | 2017.03.03 | YRS-AT-2625 | Load manual transaction details on change of retirment date 

                    'START: PPP | 2017.05.30 | YRS-AT-2625 | No need to load or unload "Mannual Transactions" on every postback, it is properly handled on Plan Type and Retirement Type dropdown change events
                    '-- As well as LoadEmploymentDetails is also being called on Retirement Type dropdown change

                    'If Me.RetireType = "DISABL" AndAlso TextBoxRetirementDate.Text.Trim() <> "" Then
                    '    LoadEmploymentDetails(Me.PersonId)
                    '    LoadManualTransactionForDisability(Me.FundEventId, Me.RetireType, TextBoxRetirementDate.Text.Trim())
                    'Else
                    '    ResetManualTransactionDetails()
                    'End If
                    'END: PPP | 2017.05.30 | YRS-AT-2625 | No need to load or unload "Mannual Transactions" on every postback, it is properly handled on Plan Type and Retirement Type dropdown change events

                    'END: MMR | 2017.03.14 | YRS-AT-2625 | Commented existing code and added to display average salary for Disability retirement on change of retirement date and hide manual transaction link if retirement date is empty
                End If

                'START: MMR | 2017.03.03 | YRS-AT-2625 | Hiding message and binding grid after selecting manual transactions
                If hdnManualTransaction.Value = "3" Then
                    ShowHideMessageManualTransaction(False)
                End If

                If Not Me.ManualTransactionDetails Is Nothing Then
                    DatagridManualTransactionList.DataSource = Me.ManualTransactionDetails.Tables("ManualTransactionDetails")
                    DatagridManualTransactionList.DataBind()
                End If
                'END: MMR | 2017.03.03 | YRS-AT-2625 | Hiding message and binding grid after selecting manual transactions

                If Me.NoBasicAccountContribution = True Then
                    Me.NoBasicAccountContribution = False
                    tabStripRetirementEstimate.SelectedIndex = 1
                    MultiPageRetirementEstimate.SelectedIndex = 1
                    Call SetControlFocus(TextBoxFutureSalary)
                End If
                'added by Ashish for phase V changes
                LabelAcctBalExceedTresholdLimit.Text = ""

                'If Me.RetirementDate <> TextBoxRetirementDate.Text.ToString() Then
                'setEmploymentControlState()
                'End If
                If Panel1.Visible Or PanelAlternatePayee.Visible Or PanelDisability.Visible Then
                    tblIDMcheck.Visible = True
                    chkIDM.Visible = True
                End If
            Else
                ' START Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                'Added by chandrasekar.c:Static 55 change to dynamic by using property MinAgeToRetire
                If DatagridElectiveRetirementAccounts.Items.Count > 0 And Convert.ToDecimal(TextBoxRetirementAge.Text) >= Me.MinAgeToRetire And DropDownListRetirementType.SelectedValue.ToUpper().Trim = "NORMAL" Then
                    chkRetirementAccount.Visible = True
                    lblRetirementPartialAmountEligible.Text = ""
                    txtRetirementAccount.Visible = True
                    lblSavingPartialAmountEligible.Text = ""
                Else
                    chkRetirementAccount.Visible = False
                    txtRetirementAccount.Visible = False
                    chkRetirementAccount.Checked = False
                    txtRetirementAccount.Text = ""
                End If
                'Added by chandrasekar.c:Static 55 change to dynamic by using property MinAgeToRetire
                If DatagridElectiveSavingsAccounts.Items.Count > 0 And Convert.ToDecimal(TextBoxRetirementAge.Text) >= Me.MinAgeToRetire And DropDownListRetirementType.SelectedValue.ToUpper().Trim = "NORMAL" Then
                    chkSavingPartialAmount.Visible = True
                    txtSavingPartialAmount.Visible = True
                    lblSavingPartialAmountEligible.Text = ""
                    lblRetirementPartialAmountEligible.Text = ""
                Else
                    chkSavingPartialAmount.Visible = False
                    txtSavingPartialAmount.Visible = False
                    chkSavingPartialAmount.Checked = False
                    txtSavingPartialAmount.Text = ""
                End If
                'END Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals


            End If  'postback = true
            'Added By Ashish for phase V changes,Start

            If ViewState("RetirementDate") Is Nothing Then
                ViewState("RetirementDate") = TextBoxRetirementDate.Text
            Else

                If ViewState("RetirementDate") <> TextBoxRetirementDate.Text Then
                    LabelEstimateDataChangedMessage.Visible = True
                    ViewState("RetirementDate") = TextBoxRetirementDate.Text
                End If

            End If
            'Added By Ashish for phase V changes,End
            '2012.05.22 SP :BT-976/YRS 5.0-1507 - Reopned issue -Start
            If ViewState("BenificiaryBirthDate") Is Nothing Then
                ViewState("BenificiaryBirthDate") = TextboxBeneficiaryBirthDate.Text
            Else

                If ViewState("BenificiaryBirthDate") <> TextboxBeneficiaryBirthDate.Text Then
                    LabelEstimateDataChangedMessage.Visible = True
                    ViewState("BenificiaryBirthDate") = TextboxBeneficiaryBirthDate.Text
                    DeselectedAnnuityBeneficiaryGrid()

                End If

            End If

            If DropDownListRetirementType.SelectedValue.ToUpper().Trim = "DISABL" Then
                chkSavingPartialAmount.Visible = False
                chkSavingPartialAmount.Checked = False
                txtSavingPartialAmount.Visible = False
                lblSavingPartialAmountEligible.Text = ""
                lblRetirementPartialAmountEligible.Text = ""
                chkRetirementAccount.Visible = False
                txtRetirementAccount.Visible = False
                chkRetirementAccount.Checked = False
                txtRetirementAccount.Text = ""
            End If


            '2012.05.22 SP :BT-976/YRS 5.0-1507 - Reopned issue -End
        Catch ex As SqlException
            HelperFunctions.LogException("RetirementEstimateWebForm-Page_Load", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-Page_Load", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,start
            Dim strExceptionMsg As String

            If (ex.Message.ToString().Contains("MESSAGE")) Then
                strExceptionMsg = getmessage(ex.Message.ToString())
            Else
                strExceptionMsg = ex.Message.Trim.ToString()
            End If
            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,End
            g_String_Exception_Message = Server.UrlEncode(strExceptionMsg)
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub DataGridEmployment_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridEmployment.SelectedIndexChanged
        Try
            saveActiveSalaryInformation()
            displayEmploymentModifiedSalaryInformation()
        Catch ex As SqlException
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-DataGridEmployment_SelectedIndexChanged", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            ClearSession()
            Session("Page") = "Person"
            Me.Page.Dispose()
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-ButtonCancel_Click", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
        Server.Transfer("FindInfo.aspx?Name=Estimates", False)
    End Sub
    Private Sub tabStripRetirementEstimate_SelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabStripRetirementEstimate.SelectedIndexChange
        Try
            If Me.tabStripRetirementEstimate.SelectedIndex = 2 Then
                'Sanket : BT-926 : Showing and allowing Retired Death Benefit amount even for 'Savings' plan selected.
                If Not Session("mnyDeathBenefitAmount") Is Nothing Then
                    TextboxRetiredDeathBenefit.Text = Math.Round(Convert.ToDecimal(Session("mnyDeathBenefitAmount")), 2)
                End If
            End If
            Me.MultiPageRetirementEstimate.SelectedIndex = Me.tabStripRetirementEstimate.SelectedIndex
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-tabStripRetirementEstimate_SelectedIndexChange", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub DropDownListEmployment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListEmployment.SelectedIndexChanged
        Dim empEventId As String
        Try
            empEventId = DropDownListEmployment.SelectedValue.ToString.Trim.ToUpper
            ' Populate the accounts tab
            'Me.populateElectiveAccountsTab(empEventId, False)
            Me.populateElectiveAccountsTab(empEventId, True, False)
            'Added by Ashish for Phase V changes
            ValidateProjectedBalancesAsPerRefund()
            SetElectiveAccountSelectionState()
            ShowProjectedAcctBalancesTotal()
        Catch ex As SqlException
            HelperFunctions.LogException("RetirementEstimateWebForm-DropDownListEmployment_SelectedIndexChanged", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-DropDownListEmployment_SelectedIndexChanged", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Try

            Buttoncalculate.CausesValidation = False
            ButtonOK.CausesValidation = False
            LabelAccountType.Visible = False
            LabelContributionType.Visible = False
            LabelContributionPercentage.Visible = False
            LabelContribAmt.Visible = False
            ButtonOK.Enabled = False
            DropDownListEmployment.SelectedIndex = -1
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-ButtonOK_Click", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try

    End Sub
    Private Sub ButtonCalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Buttoncalculate.Click
        Dim dsRetEstimateEmployment As DataSet
        Dim l_boolCanCallEstimateAnnuities As Boolean = False
        Dim l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT As DataTable
        Dim decMinPartialWithdrawalLimit As Decimal
        Dim strErrorMessage As String = String.Empty
        Dim bIsCurrencyForamt As Boolean = False
        Try
            'Added by Ashish for phase V
            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            ' START : SB | 03/08/2017 | YRS-AT-2625 | checking if the participant is eligible for annuity purchase if not then do not allow to calculate the estimation
            IsPersonalWithdrawalExists()
            If (Me.HasSatisfiedPaidService = False And Me.PersonalWithdrawalExists = True) Then
                Exit Sub
            End If
            ' END : SB | 03/08/2017 | YRS-AT-2625 | checking if the participant is eligible for annuity purchase if not then do not allow to calculate the estimation
            If chkRetirementAccount.Checked Or chkSavingPartialAmount.Checked Then

                If txtRetirementAccount.Visible = True And txtRetirementAccount.Enabled = True Then
                    If txtRetirementAccount.Text.Trim <> "" Then
                        If Not Regex.IsMatch(txtRetirementAccount.Text.Trim, RetirementBOClass.REG_EX_CURRENCY) And txtRetirementAccount.Text.Trim <> String.Empty Then
                            LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREMENT_PARTIAL_CURRENCY_FORMAT")
                            LabelPartialWithdrawal.Visible = True
                            chkRetirementAccount.Checked = True
                            bIsCurrencyForamt = True
                            '   Exit Sub
                        End If
                    Else
                        LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREMENT_PARTIAL_CURRENCY_FORMAT")
                        LabelPartialWithdrawal.Visible = True
                        chkRetirementAccount.Checked = True
                        bIsCurrencyForamt = True
                        'Exit Sub
                    End If

                End If

                If txtSavingPartialAmount.Visible = True And txtSavingPartialAmount.Enabled = True Then
                    If txtSavingPartialAmount.Text.Trim <> "" Then
                        If Not Regex.IsMatch(txtSavingPartialAmount.Text.Trim, RetirementBOClass.REG_EX_CURRENCY) And txtSavingPartialAmount.Text.Trim <> String.Empty Then
                            LabelPartialWithdrawal.Text += "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_SAVING_PARTIAL_CURRENCY_FORMAT")
                            LabelPartialWithdrawal.Visible = True
                            chkSavingPartialAmount.Checked = True
                            bIsCurrencyForamt = True
                            'Exit Sub
                        End If
                    Else
                        LabelPartialWithdrawal.Text += "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_SAVING_PARTIAL_CURRENCY_FORMAT")
                        LabelPartialWithdrawal.Visible = True
                        chkSavingPartialAmount.Checked = True
                        bIsCurrencyForamt = True
                        'Exit Sub
                    End If

                End If

                If bIsCurrencyForamt Then
                    Exit Sub
                End If


                l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("MIN_PARTIAL_WITHDRAWAL_LIMIT")

                If Not l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT Is Nothing Then
                    If l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT.Rows(0)("Key").ToString.Trim.ToUpper() = "MIN_PARTIAL_WITHDRAWAL_LIMIT" Then
                        decMinPartialWithdrawalLimit = CType(l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT.Rows(0)("Value").ToString, Decimal)
                    End If
                End If

                If txtRetirementAccount.Visible = True And txtRetirementAccount.Enabled = True And txtRetirementAccount.Text <> "" Then
                    If Convert.ToDecimal(txtRetirementAccount.Text) < decMinPartialWithdrawalLimit Then
                        strErrorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREMENT_MIN_PARTIAL_LIMIT").ToString().Replace("$$MIN_PARTIAL_WITHDRAWAL_LIMIT$$", decMinPartialWithdrawalLimit.ToString())
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", strErrorMessage, MessageBoxButtons.Stop)
                        chkRetirementAccount.Checked = True
                        Exit Sub
                    End If
                End If

                If txtSavingPartialAmount.Visible = True And txtSavingPartialAmount.Enabled = True And txtSavingPartialAmount.Text <> "" Then
                    If Convert.ToDecimal(txtSavingPartialAmount.Text) < decMinPartialWithdrawalLimit Then
                        strErrorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREMENT_MIN_PARTIAL_LIMIT").ToString().Replace("$$MIN_PARTIAL_WITHDRAWAL_LIMIT$$", decMinPartialWithdrawalLimit.ToString())
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", strErrorMessage, MessageBoxButtons.Stop)
                        chkSavingPartialAmount.Checked = True
                        Exit Sub
                    End If
                End If

            End If

            Page.Validate()
            If Page.IsValid Then

                '2012.05.22 SP:  BT-976/YRS 5.0-1507 - reopened -Start
                If Not (TextboxLastName.Text = String.Empty AndAlso TextboxFirstName.Text = String.Empty _
                  AndAlso DropDownRelationShip.SelectedIndex = 0 AndAlso TextboxBeneficiaryBirthDate.Text = String.Empty) Then
                    If (TextboxLastName.Text = String.Empty AndAlso TextboxFirstName.Text = String.Empty) Then
                        'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please enter beneficiary first/last name.", MessageBoxButtons.Stop)
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", getmessage("MESSAGE_RETIREMENT_ESTIMATE_ENTER_BENIFICIARY_FIRSTNAME"), MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                    If (DropDownRelationShip.SelectedIndex = 0 AndAlso TextboxBeneficiaryBirthDate.Text = String.Empty) Then
                        'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please enter beneficiary birthdate and relation.", MessageBoxButtons.Stop)
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", getmessage("MESSAGE_RETIREMENT_ESTIMATE_ENTER_BENIFICIARY_BIRTHDATE_RELATION"), MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                    If (DropDownRelationShip.SelectedIndex AndAlso TextboxBeneficiaryBirthDate.Text = String.Empty) Then
                        'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please enter birthdate.", MessageBoxButtons.Stop)
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", getmessage("MESSAGE_RETIREMENT_ESTIMATE_ENTER_BENIFICIARY_BIRTHDATE"), MessageBoxButtons.Stop)
                        Exit Sub
                    ElseIf (DropDownRelationShip.SelectedIndex = 0 AndAlso TextboxBeneficiaryBirthDate.Text <> String.Empty) Then
                        'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select realtionship.", MessageBoxButtons.Stop)
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", getmessage("MESSAGE_RETIREMENT_ESTIMATE_ENTER_BENIFICIARY_RELATION"), MessageBoxButtons.Stop)
                        Exit Sub
                    End If

                End If
                '2012.05.22 SP:  BT-976/YRS 5.0-1507 - reopened -End

                'Start-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                If Session("ProceedWithJSOptionEstimationQDRO") = "Initial" Or Session("ProceedWithJSOptionEstimationQDRO") = "ChangeBeneficiaries" Then
                    Session("Print") = Nothing
                    If Me.FundEventStatus = "QD" And DropDownRelationShip.SelectedValue = "SP" Then
                        MessageBox.Show(PlaceHolder1, " YMCA - YRS", getmessage("MESSAGE_RETIREMENT_ESTIMATE_BLOCK_JSANNUITY_FOR_QDRO"), MessageBoxButtons.ProceedCancel)
                        Session("ProceedWithJSOptionEstimationQDRO") = "ChangeBeneficiaries"
                        Exit Sub
                    End If

                End If
                'End-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                'Added by Ashish for phase V changes ,start
                LabelEstimateDataChangedMessage.Visible = False
                If GetProjectedAcctBalances() = False Then
                    SetSocialSecurityDetails()
                    Exit Sub
                End If
                Dim empEventId As String = String.Empty
                If DropDownListEmployment.Items.Count > 1 Then
                    empEventId = DropDownListEmployment.Items(1).Value.ToString.Trim.ToUpper
                End If
                'Update Projected Balance in Elective Acct Grid


                'populateElectiveAccountsTab(empEventId, True, False)
                l_boolCanCallEstimateAnnuities = ValidateProjectedBalancesAsPerRefund()
                'Start: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                If Not SetElectiveAccountSelectionState() Then
                    ResetPartialSavingGrid()
                    ResetRetireePartialGrid()
                    Exit Sub
                End If
                'End: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                ShowProjectedAcctBalancesTotal()
                SetSocialSecurityDetails()
                'Added by Ashish for phase V changes ,End 
                If l_boolCanCallEstimateAnnuities = False Then
                    SetSocialSecurityDetails()  'NP:2008.06.13:YRS-5.0-457 - Adding code to reset the display of existing annuity computations
                    'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                    'dsRetEstimateEmployment = ValidateEstimate(False)

                    'If Not dsRetEstimateEmployment Is Nothing Then
                    If ValidateEstimate(False) Then
                        saveActiveSalaryInformation()
                        'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                        'Call EstimateAnnuities(dsRetEstimateEmployment)

                        'SP:  BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary
                        Me.UpdateSelectedParticiapntBeneficary()

                        Call EstimateAnnuities()

                        tabStripRetirementEstimate.SelectedIndex = 0
                        Me.MultiPageRetirementEstimate.SelectedIndex = Me.tabStripRetirementEstimate.SelectedIndex
                    End If
                End If
            End If
        Catch ex As SqlException
            HelperFunctions.LogException("RetirementEstimateWebForm-ButtonCalculate_Click", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?DBMessage=" + g_String_Exception_Message, False)
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-ButtonCalculate_Click", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,start
            Dim exmsg As String

            If (ex.Message.ToString().Contains("MESSAGE")) Then
                exmsg = getmessage(ex.Message.ToString())
            Else
                exmsg = ex.Message.Trim.ToString()
            End If
            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,End
            g_String_Exception_Message = Server.UrlEncode(exmsg)
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPrint.Click
        Try
            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            'Added by Ashish for Phase V changes,
            Page.Validate()
            If Page.IsValid Then

                '2012.05.22 SP:  BT-976/YRS 5.0-1507 - reopened -Start
                If Not (TextboxLastName.Text = String.Empty AndAlso TextboxFirstName.Text = String.Empty _
                        AndAlso DropDownRelationShip.SelectedIndex = 0 AndAlso TextboxBeneficiaryBirthDate.Text = String.Empty) Then
                    If (TextboxLastName.Text = String.Empty AndAlso TextboxFirstName.Text = String.Empty) Then
                        'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please enter beneficiary first/last name.", MessageBoxButtons.Stop)
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", getmessage("MESSAGE_RETIREMENT_ESTIMATE_ENTER_BENIFICIARY_FIRSTNAME"), MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                    If (DropDownRelationShip.SelectedIndex = 0 AndAlso TextboxBeneficiaryBirthDate.Text = String.Empty) Then
                        'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please enter beneficiary birthdate and relation.", MessageBoxButtons.Stop)
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", getmessage("MESSAGE_RETIREMENT_ESTIMATE_ENTER_BENIFICIARY_BIRTHDATE_RELATION"), MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                    If (DropDownRelationShip.SelectedIndex AndAlso TextboxBeneficiaryBirthDate.Text = String.Empty) Then
                        'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please enter birthdate.", MessageBoxButtons.Stop)
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", getmessage("MESSAGE_RETIREMENT_ESTIMATE_ENTER_BENIFICIARY_BIRTHDATE"), MessageBoxButtons.Stop)
                        Exit Sub
                    ElseIf (DropDownRelationShip.SelectedIndex = 0 AndAlso TextboxBeneficiaryBirthDate.Text <> String.Empty) Then
                        'MessageBox.Show(PlaceHolder1, "YMCA-YRS", "Please select realtionship.", MessageBoxButtons.Stop)
                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", getmessage("MESSAGE_RETIREMENT_ESTIMATE_ENTER_BENIFICIARY_RELATION"), MessageBoxButtons.Stop)
                        Exit Sub
                    End If

                End If

                '2012.05.22 SP:  BT-976/YRS 5.0-1507 - reopened -End

                Session("Print") = "Print"
                'Start-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                If Session("ProceedWithJSOptionEstimationQDRO") = "Initial" Then
                    If Me.FundEventStatus = "QD" And DropDownRelationShip.SelectedValue = "SP" Then
                        MessageBox.Show(PlaceHolder1, " YMCA - YRS", getmessage("MESSAGE_RETIREMENT_ESTIMATE_BLOCK_JSANNUITY_FOR_QDRO"), MessageBoxButtons.ProceedCancel)
                        Session("ProceedWithJSOptionEstimationQDRO") = "NotInitial"
                        Exit Sub
                    End If
                End If
                'End-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                If TextBoxPraAssumption.Visible = False Then
                    labelPRAAssumption.Visible = True
                    TextBoxPraAssumption.Visible = True
                    ' ButtonPRA.Visible = True
                    ButtonPrint.Enabled = False
                    Buttoncalculate.Enabled = False
                    tabStripRetirementEstimate.Enabled = False
                    Me.setPrintText()
                    ''Shashi Shekhar:5 Aug 2010 :YRS 5.0-1142
                    'Ashish:2010.06.21 YRS 5.0-1115 ,start
                    'btnPRAShort.Visible = True
                    'btnPRAShort.Enabled = True
                    'btnPRAColor.Visible = True
                    'btnPRAColor.Enabled = True
                    'Ashish:2010.06.21 YRS 5.0-1115 ,End
                    'Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request
                    tblIDMcheck.Visible = True
                    chkIDM.Visible = True
                    chkIDM.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_COPY_TO_IDM")
                    chkIDM.Checked = False
                    If Me.RetireType = "DISABL" Then
                        Panel1.Visible = False
                        PanelDisability.Visible = True
                    Else
                        PanelDisability.Visible = False
                        Panel1.Visible = True
                    End If
                    'Ashish YRS 5.0-1345 :BT-859
                    'BT-798 : System should not allow disability retirement for QD and BF fundevents
                    If (Me.OrgBenTypeIsQDROorRBEN = True) Then
                        Panel1.Visible = False
                        PanelDisability.Visible = False
                        PanelAlternatePayee.Visible = True
                    End If
                    'Anudeep:01.04.2013:Checking whether empty report name exists.. if exists draft full form button will not be visible
                    If System.Configuration.ConfigurationSettings.AppSettings("RET_EST_DraftFullForm") = "" Then
                        ButtonPRAFull.Visible = False
                    End If
                    ''----------------------------------------
                    Exit Sub
                    'Else
                    'Ashish:2010.06.21 YRS 5.0-1115 
                    ' Call PrintEstimate()
                    'LabelEstimateDataChangedMessage.Visible = False
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm -- ButtonPrint_Click ", ex)
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    'START--Manthan Rajguru | 2016.01.14 | YRS-AT-2151 | Commented the exisiting code and removed btnPRAShort.Click and btnPRAColor.Click events
    'Private Sub ButtonPRA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPRA.Click, btnPRAShort.Click, btnPRAColor.Click, btnAlternatePayee.Click, ButtonPRAFull.Click
    '    Dim strCommandName As String = String.Empty
    '    Try
    '        If TextBoxPraAssumption.Text.Length > 1000 Then
    '            'commented by Anudeep on 22-sep for BT-1126
    '            'ShowCustomMessage("The number of characters entered have exceeded the maximum limit ", enumMessageBoxType.DotNet, MessageBoxButtons.OK)
    '            ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_ESTIMATE_CHARACTERS_EXCEEDED"), enumMessageBoxType.DotNet, MessageBoxButtons.OK)
    '            Exit Sub
    '        Else
    '            'Commented by Ashish for Phase V changes
    '            'PrintEstimate()
    '            'Added By Ashish for phase V changes ,Start
    '            Page.Validate()
    '            If Page.IsValid() Then
    '                'Ashish:2010.06.21 YRS 5.0-1115
    '                strCommandName = CType(sender, Button).CommandName
    '                Session("RE_PRAType") = strCommandName
    '                PrintEstimate(strCommandName)
    '                'Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request
    '                tblIDMcheck.Visible = False
    '                LabelEstimateDataChangedMessage.Visible = False
    '            End If
    '            'Added By Ashish for phase V changes ,End
    '        End If
    '    Catch ex As Exception
    '        Dim exmsg As String

    '        If (ex.Message.ToString().Contains("MESSAGE")) Then
    '            exmsg = getmessage(ex.Message.ToString())
    '        Else
    '            exmsg = ex.Message.Trim.ToString()
    '        End If
    '        '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,End
    '        g_String_Exception_Message = Server.UrlEncode(exmsg)
    '        Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
    '    End Try
    'End Sub
    Private Sub ButtonPRA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPRA.Click, btnAlternatePayee.Click, ButtonPRAFull.Click
        Dim strCommandName As String = String.Empty
        Try
            If TextBoxPraAssumption.Text.Length > 1000 Then
                'commented by Anudeep on 22-sep for BT-1126
                'ShowCustomMessage("The number of characters entered have exceeded the maximum limit ", enumMessageBoxType.DotNet, MessageBoxButtons.OK)
                ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_ESTIMATE_CHARACTERS_EXCEEDED"), enumMessageBoxType.DotNet, MessageBoxButtons.OK)
                Exit Sub
            Else
                'Commented by Ashish for Phase V changes
                'PrintEstimate()
                'Added By Ashish for phase V changes ,Start
                Page.Validate()
                If Page.IsValid() Then
                    'Ashish:2010.06.21 YRS 5.0-1115
                    strCommandName = CType(sender, Button).CommandName
                    Session("RE_PRAType") = strCommandName
                    PrintEstimate(strCommandName)
                    'Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request
                    tblIDMcheck.Visible = False
                    LabelEstimateDataChangedMessage.Visible = False
                End If
                'Added By Ashish for phase V changes ,End
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-ButtonPRA_Click", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            Dim exmsg As String

            If (ex.Message.ToString().Contains("MESSAGE")) Then
                exmsg = getmessage(ex.Message.ToString())
            Else
                exmsg = ex.Message.Trim.ToString()
            End If
            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,End
            g_String_Exception_Message = Server.UrlEncode(exmsg)
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    'End--Manthan Rajguru | 2016.01.14 | YRS-AT-2151 | Commented the existing code and removed btnPRAShort.Click and btnPRAColor.Click events
    Private Sub DropDownListProjInterest2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DropDownListProjInterest2.SelectedIndexChanged
        Try
            ListBoxProjectedYearInterest.SelectedValue = DropDownListProjInterest2.SelectedValue
            LabelEstimateDataChangedMessage.Visible = True
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-DropDownListProjInterest2_SelectedIndexChanged", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub DropdownlistPercentageToUse_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownlistPercentageToUse.SelectedIndexChanged
        Try
            Session("PercentageSelected") = True
            'Shashank Patel   2011.11.03  YRS-1474/BT-952 : Error when selected RDB pctg before RDB is computed
            If String.IsNullOrEmpty(TextboxRetiredDeathBenefit.Text) Then
                TextboxRetiredDeathBenefit.Text = "0.00"
            End If
            'End YRS-1474/BT-952 : Error when selected RDB pctg before RDB is computed
            TextboxAmountToUse.Text = Math.Round(Convert.ToDecimal((TextboxRetiredDeathBenefit.Text) * (DropdownlistPercentageToUse.SelectedValue) / 100), 2)

            LabelEstimateDataChangedMessage.Visible = True
            'Added by Ashish for phase V changes ,End
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-DropdownlistPercentageToUse_SelectedIndexChanged", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub TextboxAmountToUse_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxAmountToUse.TextChanged
        Try
            If Page.IsPostBack = True Then
                If Me.TextboxAmountToUse.Text <> "" Then
                    If Not Session("PercentageSelected") = True And Not Convert.ToDecimal(TextboxAmountToUse.Text) = 0 Then
                        DropdownlistPercentageToUse.SelectedValue = 0
                    End If
                End If

            End If
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-TextboxAmountToUse_TextChanged", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    'Priya 04-March-2009 mede function of actions which are perform on DropDownList's selected index change event.
    Private Sub SelectedListPlanType()
        SetSocialSecurityDetails()  'This clears the annnuities list on the summary tab
        Dim selectedPlan As String
        Dim i As Integer

        selectedPlan = Me.DropDownListPlanType.SelectedValue
        Select Case selectedPlan
            Case "Retirement"
                selectedPlan = "R"
                'Phase IV Changes - Start
                'CheckBoxRetirementPlan.Visible = True  'NP:2008.06.13:YRS-5.0-457 - We no longer need to show or use this Checkbox

                DatagridElectiveRetirementAccounts.Visible = True
                LabelRetirementPlan.Visible = True  'NP:2008.06.13:YRS-5.0-457 - Instead of Checkbox's label use another label

                'CheckBoxSavingsPlan.Visible = False    'NP:2008.06.13:YRS-5.0-457 - We no longer need to show or use this Checkbox
                'CheckBoxSavingsPlan.Checked = False    'NP:2008.06.13:YRS-5.0-457 - We no longer need to show or use this Checkbox
                'Commented by Ashish 17-Apr-2009 for Phase V chages
                'ClearElectiveAccountAssumptions("S")    'NP:2008.06.17:YRS-5.0-457 - Reset any assumptions being made for what if analysis
                'Added by Ashish for Phase V changes
                UpdateElectiveAccountAssumptions("S")
                DatagridElectiveSavingsAccounts.DataSource = Nothing
                DatagridElectiveSavingsAccounts.DataBind()
                DatagridElectiveSavingsAccounts.Visible = False
                LabelSavingsPlan.Visible = False  'NP:2008.06.13:YRS-5.0-457 - Instead of Checkbox's label use another label
                'Phase IV Changes - End


                'Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                If DropDownListRetirementType.SelectedValue.ToUpper().Trim = "NORMAL" Then

                    'End If
                    chkSavingPartialAmount.Checked = False
                    chkSavingPartialAmount.Visible = False
                    txtSavingPartialAmount.Text = ""
                    txtSavingPartialAmount.Visible = False
                    'Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                    txtSavingPartialAmount.Enabled = False

                    chkRetirementAccount.Checked = False
                    chkRetirementAccount.Visible = True
                    txtRetirementAccount.Visible = True
                    txtRetirementAccount.Text = ""
                    'Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                    txtRetirementAccount.Enabled = False
                End If



                lblRetirementPartialAmountEligible.Text = ""
                lblSavingPartialAmountEligible.Text = ""

                If TextBoxRetirementDate.Text.Trim() <> "" Then 'MMR | 2017.03.24 | YRS-AT-2625 | Added condition to check if retirement date is not empty to avoid error
                    LoadManualTransactionForDisability(Me.FundEventId, Me.RetireType, TextBoxRetirementDate.Text.Trim()) 'MMR | 2017.03.15 | YRS-AT-2625 | Loading manual transaction details for RETIREMENT plan type
                End If

            Case "Savings"
                selectedPlan = "S"
                'Phase IV Changes - Start
                'CheckBoxRetirementPlan.Checked = False     'NP:2008.06.13:YRS-5.0-457 - We no longer need to show or use this Checkbox
                'CheckBoxRetirementPlan.Visible = False     'NP:2008.06.13:YRS-5.0-457 - We no longer need to show or use this Checkbox
                'Commented by Ashish 17-Apr-2009 for Phase V chages
                'ClearElectiveAccountAssumptions("R")    'NP:2008.06.13:YRS-5.0-457 - Reset any assumptions being made for what if analysis
                'Added by Ashish for Phase V changes
                UpdateElectiveAccountAssumptions("R")
                DatagridElectiveRetirementAccounts.DataSource = Nothing
                DatagridElectiveRetirementAccounts.DataBind()
                DatagridElectiveRetirementAccounts.Visible = False
                LabelRetirementPlan.Visible = False     'NP:2008.06.13:YRS-5.0-457 - Instead of Checkbox's label use another label

                'CheckBoxSavingsPlan.Visible = True     'NP:2008.06.13:YRS-5.0-457 - We no longer need to show or use this Checkbox
                LabelSavingsPlan.Visible = True  'NP:2008.06.17:YRS-5.0-457 - Instead of Checkbox's label use another label
                DatagridElectiveSavingsAccounts.Visible = True


                'Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                If DropDownListRetirementType.SelectedValue.ToUpper().Trim = "NORMAL" Then
                    chkSavingPartialAmount.Checked = False
                    chkSavingPartialAmount.Visible = True
                    txtSavingPartialAmount.Text = ""
                    txtSavingPartialAmount.Visible = True
                    'Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                    txtSavingPartialAmount.Enabled = False

                    chkRetirementAccount.Checked = False
                    chkRetirementAccount.Visible = False
                    txtRetirementAccount.Visible = False
                    txtRetirementAccount.Text = ""
                    'Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                    txtRetirementAccount.Enabled = False
                End If

                ResetManualTransactionDetails() 'MMR | 2017.03.15 | YRS-AT-2625 | Manual Transaction link should not be available for SAVINGS plan type

            Case "Both"
                selectedPlan = "B"
                'Phase IV Changes - Start
                'CheckBoxRetirementPlan.Visible = True      'NP:2008.06.13:YRS-5.0-457 - We no longer need to show or use this Checkbox
                DatagridElectiveRetirementAccounts.Visible = True
                LabelRetirementPlan.Visible = True  'NP:2008.06.13:YRS-5.0-457 - Instead of Checkbox's label use another label

                'CheckBoxSavingsPlan.Visible = True         'NP:2008.06.13:YRS-5.0-457 - We no longer need to show or use this Checkbox
                DatagridElectiveSavingsAccounts.Visible = True
                LabelSavingsPlan.Visible = True     'NP:2008.06.13:YRS-5.0-457 - Instead of Checkbox's label use another label
                'Phase IV Changes - End

                'Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                If DropDownListRetirementType.SelectedValue.ToUpper().Trim = "NORMAL" Then
                    chkSavingPartialAmount.Checked = False
                    chkSavingPartialAmount.Visible = True
                    txtSavingPartialAmount.Text = ""
                    txtSavingPartialAmount.Visible = True
                    'Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                    txtSavingPartialAmount.Enabled = False

                    chkRetirementAccount.Checked = False
                    chkRetirementAccount.Visible = True
                    txtRetirementAccount.Visible = True
                    txtRetirementAccount.Text = ""
                    'Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                    txtRetirementAccount.Enabled = False
                End If

                If TextBoxRetirementDate.Text.Trim() <> "" Then 'MMR | 2017.03.24 | YRS-AT-2625 | Added condition to check if retirement date is not empty to avoid error
                    LoadManualTransactionForDisability(Me.FundEventId, Me.RetireType, TextBoxRetirementDate.Text.Trim()) 'MMR | 2017.03.15 | YRS-AT-2625 | Loading manual transaction details for BOTH plan type
                End If

        End Select

        Me.PlanType = selectedPlan

        Me.disableDeathBenefitControls(selectedPlan)

        ValidateAndShowRDBWarninMessage(selectedPlan) ' BD : 2018.11.01 : YRS-AT-4133 - No Active Death Benefit if first enrolled on or after 1/1/2019
        'Me.disableElectiveAccounts(selectedPlan)

        ' Populate the accounts tab
        'Me.populateElectiveAccountsTab(DropDownListEmployment.SelectedValue.ToString.Trim.ToUpper, True, True)

        Me.setPrintText()
        'commented by Ashish for Phase V changes ,start
        'If Me.PlanType = "B" Or Me.PlanType = "R" Then
        '    If LabelRefundMessage.Visible = True Then
        '        If Me.PlanType = "R" Then
        '            LabelRefundMessage.Text = "Annuity Estimate will be based upon YMCA Account funds only."
        '        ElseIf Me.PlanType = "B" Then
        '            LabelRefundMessage.Text = "Annuity Estimate will be based upon YMCA Account plus Savings Plan funds."
        '        End If
        '    End If
        'Else
        '    LabelRefundMessage.Visible = False
        'End If

        'displayAccountBalancesAsPerRefunds()
        'commented by Ashish for Phase V changes ,END
        lblRetirementPartialAmountEligible.Text = ""
        lblSavingPartialAmountEligible.Text = ""
    End Sub
    'End 04-March-2009
    Private Sub DropDownListPlanType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DropDownListPlanType.SelectedIndexChanged
        Try
            'NP:2008.06.13:YRS-5.0-457 - Switching user from any tab to the Accounts tab
            'When a user changes the plan type, they should see the accounts tab so that they
            'know what type of money they are using and what additional contributions they are
            'projecting. Also since the plan changed, the calculations have also changed, so 
            'the summary grid should be blanked if there are any results. Also all  
            'Accounts need to be selected by default when plan type is changed
            ''04-March-2009 Priya Added this into common method.
            'SetSocialSecurityDetails()  'This clears the annnuities list on the summary tab
            ''End 04-March-2009
            Me.tabStripRetirementEstimate.SelectedIndex = 3     'Always go to the Accounts tab
            Me.MultiPageRetirementEstimate.SelectedIndex = 3 'Me.tabStripRetirementEstimate.SelectedIndex

            SelectedListPlanType()
            'Added By Ashish for Phase V changes
            'Added by Ashish for Phase V changes ,Start
            Dim empEventId As String = String.Empty
            If DropDownListEmployment.Items.Count > 1 Then
                empEventId = DropDownListEmployment.Items(1).Value.ToString.Trim.ToUpper
            End If

            populateElectiveAccountsTab(empEventId, True, False)
            'set acct with default selection
            SetDefaultAcctSelectionState()
            'ValidateProjectedBalancesAsPerRefund()
            SetElectiveAccountSelectionState()
            ShowProjectedAcctBalancesTotal()
            LabelEstimateDataChangedMessage.Visible = True

            'YRS 5.0-1035 : Set paid service - atsFundEvents.intPaid - to zero : To clear the error message after plantype changed to Savings
            'If Me.PlanType = "S" Then
            '    LabelRefundMessage.Text = String.Empty
            '    LabelRefundMessage.Visible = False
            'ElseIf Me.RetireType = "DISABL" AndAlso Me.PlanType = "B" Or Me.PlanType = "R" Then
            '    Dim isWithdrawn As Boolean
            '    isWithdrawn = RetirementBOClass.HasBasicMoneyWithdrawn(Me.FundEventId)
            '    If (isWithdrawn) Then
            '        LabelRefundMessage.Visible = True
            '        LabelRefundMessage.Text = "Participant has withdrawn funds from their basic accounts. Check if the participant has the required 60 months of paid service since the withdrawal."
            '    End If
            'End If

            ' START : SB | 03/08/2017 | YRS-AT-2625 | As discussed with team, Personal withdrawal exist validation should be validate only on Recalculate button.Hence commenting below line of code
            ''ASHISH:2011.07.27, BT-888
            'If Me.RetireType = "NORMAL" Then
            '    LabelRefundMessage.Text = String.Empty
            '    LabelRefundMessage.Visible = False
            'ElseIf Me.RetireType = "DISABL" AndAlso (Me.PlanType = "B" Or Me.PlanType = "R") Then
            '    Dim isWithdrawn As Boolean
            '    isWithdrawn = RetirementBOClass.HasBasicMoneyWithdrawn(Me.FundEventId)
            '    If (isWithdrawn) Then
            '        LabelRefundMessage.Visible = True
            '        'LabelRefundMessage.Text = "Participant has withdrawn funds from their basic accounts. Check if the participant has the required 60 months of paid service since the withdrawal."
            '        LabelRefundMessage.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_PARTICIPANT_WITHDRAWAL_FUNDS_FROM_BASIC_ACCOUNTS")
            '    End If
            'End If
            WarnUserToRecalculate()
            ' END : SB | 03/08/2017 | YRS-AT-2625 | As discussed with team, Personal withdrawal exist validation should be validate only on Recalculate button.Hence commenting below line of code

            'Added By Ashish for Phase V changes ,End    
            'selectedPlan = Me.DropDownListPlanType.SelectedValue
            '        'CheckBoxRetirementPlan.Visible = True  'NP:2008.06.13:YRS-5.0-457 - We no longer need to show or use this Checkbox
            lblRetirementPartialAmountEligible.Text = ""
            lblSavingPartialAmountEligible.Text = ""
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-DropDownListPlanType_SelectedIndexChanged", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    'NP:2008.06.13:YRS-5.0-457 - We no longer need to show or use this Checkbox
    'Private Sub CheckBoxRetirementPlan_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxRetirementPlan.CheckedChanged
    '    Try
    '        CheckAccounts()
    '    Catch ex As Exception
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
    '    End Try
    'End Sub
    'Private Sub CheckBoxSavingsPlan_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxSavingsPlan.CheckedChanged
    '    Try
    '        'If Unchecked, uncheck all the retirement plan account
    '        Dim i As Integer
    '        For i = 0 To DatagridElectiveSavingsAccounts.Items.Count - 1
    '            Dim chk As CheckBox
    '            chk = DatagridElectiveSavingsAccounts.Items(i).Cells(0).Controls(1)
    '            chk.Checked = CheckBoxSavingsPlan.Checked
    '        Next
    '    Catch ex As Exception
    '        g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
    '    End Try
    'End Sub
    Private Sub DatagridElectiveRetirementAccounts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridElectiveRetirementAccounts.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

                'showing the already selected contribution type as selected.
                'Commented by Ashish for Phase V changes
                'CType(e.Item.FindControl("DropdownlistContribTypeRet"), DropDownList).SelectedValue = CType(e.Item.DataItem, DataRowView)(0).ToString().Trim() '"Y"
                'Added by Ashish for Phase V changes
                CType(e.Item.FindControl("DropdownlistContribTypeRet"), DropDownList).SelectedValue = CType(e.Item.DataItem, DataRowView)("chrAdjustmentBasisCode").ToString().Trim() '"Y"
                'if any contribution exists then that account will be shown as selected by default for including it in the estimate .
                'Commented by Ashish  for Phase V changes
                'CType(e.Item.FindControl("CheckBoxRet"), CheckBox).Checked = CType(e.Item.DataItem, DataRowView)(10).ToString().Trim()
                'Added by Ashish  for Phase V changes
                'If CType(e.Item.DataItem, DataRowView)("bitBasicAcct") = False Then
                'CType(e.Item.FindControl("CheckBoxRet"), CheckBox).Checked = CType(e.Item.DataItem, DataRowView)("AcctTotal").ToString().Trim()
                ' End If
                'if account group falls under SRT
                'Commented by Ashish for Phase V changes
                'If CType(e.Item.DataItem, DataRowView)(9).ToString().Trim().EndsWith("RT") Then
                'Added by Ashish for Phase V changes
                If CType(e.Item.DataItem, DataRowView)("AcctGroups").ToString().Trim().EndsWith("RT") Then
                    CType(e.Item.FindControl("DropdownlistContribTypeRet"), DropDownList).Enabled = False
                    CType(e.Item.FindControl("DropdownlistContribTypeRet"), DropDownList).SelectedValue = "L"
                End If

                'if the account getting displayed is other voluntary then disabling the contribution making fields.
                'Commented by Ashish  for Phase V changes
                'If CType(e.Item.DataItem, DataRowView)(13).ToString().Trim() = False Then
                'Added by Ashish for Phase V changes
                If CType(e.Item.DataItem, DataRowView)("bitRet_Voluntary").ToString().Trim() = False Then
                    CType(e.Item.FindControl("DropdownlistContribTypeRet"), DropDownList).Enabled = False
                    CType(e.Item.FindControl("TextboxContribAmtRet"), TextBox).Enabled = False
                    CType(e.Item.FindControl("DateusercontrolStartRet"), DateUserControl).Enabled = False
                    CType(e.Item.FindControl("DateusercontrolStopRet"), DateUserControl).Enabled = False
                    'ASHISH:2010.11.22, YRS 5.0-1186 BT656 start
                ElseIf CType(e.Item.DataItem, DataRowView)("bitVolAcctTerminated").ToString().Trim() = True Then
                    CType(e.Item.FindControl("DropdownlistContribTypeRet"), DropDownList).Enabled = False
                    CType(e.Item.FindControl("TextboxContribAmtRet"), TextBox).Enabled = False
                    CType(e.Item.FindControl("DateusercontrolStartRet"), DateUserControl).Enabled = False
                    CType(e.Item.FindControl("DateusercontrolStopRet"), DateUserControl).Enabled = False
                    'ASHISH:2010.11.22, YRS 5.0-1186 BT656 end
                End If

            End If
            If e.Item.ItemType = ListItemType.Footer Then
                e.Item.Cells(RET_ACCT_TYPE).Text = "Selected Total"
                e.Item.CssClass = "Label_Small"
                'Label_Small()
            End If
            'START: PPP | 11/17/2017 | YRS-AT-3328 | Handling exception
            'Catch
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-DatagridElectiveRetirementAccounts_ItemDataBound", ex)
            Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
            'Throw
            'END: PPP | 11/17/2017 | YRS-AT-3328 | Handling exception
        End Try
    End Sub
    Private Sub DatagridElectiveSavingsAccounts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridElectiveSavingsAccounts.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                'showing the already selected contribution type as selected.
                'Commented by Ashish 16-Apr-2009 for Phase V changes
                'CType(e.Item.FindControl("DropdownlistContribTypeSav"), DropDownList).SelectedValue = CType(e.Item.DataItem, DataRowView)(0).ToString().Trim() '"Y"
                'Added by Ashish 16-Apr-2009 for Phase V changes
                CType(e.Item.FindControl("DropdownlistContribTypeSav"), DropDownList).SelectedValue = CType(e.Item.DataItem, DataRowView)("chrAdjustmentBasisCode").ToString().Trim() '"Y"
                'if any contribution exists then that account will be shown as selected by default for including it in the estimate .
                'Commented by Ashish  for Phase V changes
                'CType(e.Item.FindControl("CheckBoxSav"), CheckBox).Checked = CType(e.Item.DataItem, DataRowView)(10).ToString().Trim()
                'Added by Ashish  for Phase V changes
                CType(e.Item.FindControl("CheckBoxSav"), CheckBox).Checked = CType(e.Item.DataItem, DataRowView)("AcctTotal").ToString().Trim()
                'if account group falls under SRT
                'Commented by Ashish  for Phase V changes
                'If CType(e.Item.DataItem, DataRowView)(9).ToString().Trim().EndsWith("RT") Then
                'Added by Ashish  for Phase V changes
                If CType(e.Item.DataItem, DataRowView)("AcctGroups").ToString().Trim().EndsWith("RT") Then
                    CType(e.Item.FindControl("DropdownlistContribTypeSav"), DropDownList).Enabled = False
                    CType(e.Item.FindControl("DropdownlistContribTypeSav"), DropDownList).SelectedValue = "L"
                End If

                'if the account getting displayed is other voluntary then disabling the contribution making fields.
                'Commented by Ashish 16-Apr-2009 for Phase V changes
                'If CType(e.Item.DataItem, DataRowView)(13).ToString().Trim() = False Then
                'Added by Ashish 16-Apr-2009 for Phase V changes
                If CType(e.Item.DataItem, DataRowView)("bitRet_Voluntary").ToString().Trim() = False Then
                    CType(e.Item.FindControl("DropdownlistContribTypeSav"), DropDownList).Enabled = False
                    CType(e.Item.FindControl("TextboxContribAmtSav"), TextBox).Enabled = False
                    CType(e.Item.FindControl("DateusercontrolStartSav"), DateUserControl).Enabled = False
                    CType(e.Item.FindControl("DateusercontrolStopSav"), DateUserControl).Enabled = False
                ElseIf CType(e.Item.DataItem, DataRowView)("bitVolAcctTerminated").ToString().Trim() = True Then
                    CType(e.Item.FindControl("DropdownlistContribTypeSav"), DropDownList).Enabled = False
                    CType(e.Item.FindControl("TextboxContribAmtSav"), TextBox).Enabled = False
                    CType(e.Item.FindControl("DateusercontrolStartSav"), DateUserControl).Enabled = False
                    CType(e.Item.FindControl("DateusercontrolStopSav"), DateUserControl).Enabled = False
                End If

                'Start: Dinesh Kanojia         2015.04.27      BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
                If e.Item.Cells(1).Text.Trim.ToLower = "ln" Or e.Item.Cells(1).Text.Trim.ToLower = "ld" Then
                    CType(e.Item.FindControl("CheckBoxSav"), CheckBox).Enabled = True
                    CType(e.Item.FindControl("CheckBoxSav"), CheckBox).Checked = False
                    CType(e.Item.FindControl("CheckBoxSav"), CheckBox).Enabled = False
                End If
                'End: Dinesh Kanojia         2015.04.27      BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
            End If

            If e.Item.ItemType = ListItemType.Footer Then
                e.Item.Cells(SAV_ACCT_TYPE).Text = "Selected Total"
                e.Item.CssClass = "Label_Small"

            End If
            'START: PPP | 11/17/2017 | YRS-AT-3328 | Handling exception
            'Catch
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-DatagridElectiveSavingsAccounts_ItemDataBound", ex)
            Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
            'Throw
            'END: PPP | 11/17/2017 | YRS-AT-3328 | Handling exception
        End Try
    End Sub
    Private Sub ButtonUpdateEmployment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUpdateEmployment.Click
        Dim g_String_Exception_Message As String
        Try
            saveActiveSalaryInformation()
            'ASHISH:2010.10.11 YRS 5.0-855,BT 624 
            DisplayEmploymentGridWithUpdatedTermDate()
            'Commented by Ashish after Mark confirmation email 6-May-2009
            ''Commented by Ashish for phase V changes
            ''saveActiveSalaryInformation()
            ''Added by Ashish for Phase V changes
            'Page.Validate()
            'If Page.IsValid() Then
            '    GetProjectedAcctBalances()
            '    Dim empEventId As String = String.Empty
            '    If DropDownListEmployment.Items.Count > 1 Then
            '        empEventId = DropDownListEmployment.Items(1).Value.ToString.Trim.ToUpper
            '    End If
            '    populateElectiveAccountsTab(empEventId, True, False)
            '    ValidateProjectedBalancesAsPerRefund()
            '    SetElectiveAccountSelectionState()
            '    ShowProjectedAcctBalancesTotal()
            'End If

            LabelEstimateDataChangedMessage.Visible = True
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-ButtonUpdateEmployment_Click", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub DatatGridSocialSecurityLevel_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatatGridSocialSecurityLevel.ItemDataBound
        Try
            e.Item.Cells(2).Visible = False
            e.Item.Cells(3).Visible = False
            'START: PPP | 11/17/2017 | YRS-AT-3328 | Handling exception
            'Catch
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-DatatGridSocialSecurityLevel_ItemDataBound", ex)
            Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
            'Throw
            'END: PPP | 11/17/2017 | YRS-AT-3328 | Handling exception
        End Try
    End Sub
    Protected Sub DatagridElectiveRetirementAccounts_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim g_String_Exception_Message As String
        'Commented by Ashish for phase v changes ,Start
        'Dim l_UC_DatePicker As YMCAUI.DateUserControl
        'Dim l_DropDownList As DropDownList
        'Dim l_TextBox As TextBox
        'Dim l_CheckBox As CheckBox
        'Dim l_string_AcctType As String
        'Dim i As Integer

        'Dim ck1 As CheckBox = 
        'Dim dgItem As DataGridItem = CType(ck1.NamingContainer, DataGridItem)
        'Dim chk As CheckBox
        'Commented by Ashish for phase v changes ,End
        'Added By Ashish For phase V changes
        Dim l_CheckBox As CheckBox
        Dim dgItem As DataGridItem
        Try
            'Addd by Ashish for Phase V changes ,Start
            l_CheckBox = CType(sender, CheckBox)
            dgItem = CType(l_CheckBox.NamingContainer, DataGridItem)
            RetirementAcctGridRowSelectionChangeValidation(dgItem)
            'update selection state in base grouped datatable and projected balance in acct datagrid 
            SetElectiveAccountSelectionState()
            'calculate projected balance total and display in acct datagrid
            ShowProjectedAcctBalancesTotal()
            LabelEstimateDataChangedMessage.Visible = True

            'Addd by Ashish for Phase V changes ,End
            'commented by Ashish for phase V changes
            'If ck1.Checked = False Then
            '    'Commented by Ashish for Phase V changes, Start
            '    'l_string_AcctType = dgItem.Cells(RET_ACCT_TYPE).Text.ToString().Trim()

            '    'l_DropDownList = dgItem.FindControl("DropdownlistContribTypeRet")
            '    'If Not l_DropDownList Is Nothing And l_string_AcctType <> "RT" Then
            '    '    l_DropDownList.SelectedValue = ""
            '    'End If

            '    'l_TextBox = dgItem.FindControl("TextboxContribAmtRet")
            '    'If Not l_TextBox Is Nothing Then
            '    '    l_TextBox.Text = ""
            '    'End If

            '    'l_UC_DatePicker = dgItem.FindControl("DateusercontrolStartRet")
            '    'If Not l_UC_DatePicker Is Nothing Then
            '    '    l_UC_DatePicker.Text = ""
            '    'End If

            '    'l_UC_DatePicker = dgItem.FindControl("DateusercontrolStopRet")
            '    'If Not l_UC_DatePicker Is Nothing Then
            '    '    l_UC_DatePicker.Text = ""
            '    'End If
            '    'Commented by Ashish for Phase V changes, End

            '    'Added by Ashish for Phase V changes ,Start
            '    ResetRetirementAcctDataGridRow(dgItem)

            '    If dgItem.Cells(RET_BASIC_ACCT).Text.ToString().Trim() = "True" And dgItem.Cells(RET_LEGACY_ACCT_TYPE).Text.ToString().Trim() = "PA" Then
            '        For i = 0 To DatagridElectiveRetirementAccounts.Items.Count - 1

            '            If DatagridElectiveRetirementAccounts.Items(i).Cells(RET_BASIC_ACCT).Text.ToString().Trim() = "False" Then
            '                chk = DatagridElectiveRetirementAccounts.Items(i).Cells(RET_SEL_ACCT_CHK).Controls(1)
            '                chk.Checked = False
            '                ResetRetirementAcctDataGridRow(DatagridElectiveRetirementAccounts.Items(i))
            '            End If

            '        Next i
            '    End If
            '    'Added by Ashish for Phase V changes ,End
            'Else
            '    'Commented by Ashish for Phase V changes, Start
            '    'If dgItem.Cells(11).Text.ToString().Trim() = "True" Then
            '    '    For i = 0 To DatagridElectiveRetirementAccounts.Items.Count - 1

            '    '        If DatagridElectiveRetirementAccounts.Items(i).Cells(11).Text.ToString().Trim() = "True" And DatagridElectiveRetirementAccounts.Items(i).Cells(2).Text.ToString().Trim() <> "" Then
            '    '            If Convert.ToDouble(DatagridElectiveRetirementAccounts.Items(i).Cells(2).Text.ToString().Trim()) > 0 Then
            '    '                chk = DatagridElectiveRetirementAccounts.Items(i).Cells(0).Controls(1)
            '    '                chk.Checked = True
            '    '            End If
            '    '        End If

            '    '    Next i
            '    'End If
            '    'Commented by Ashish for Phase V changes, End
            '    'Added by Ashish for Phase V changes ,Start
            '    If dgItem.Cells(RET_BASIC_ACCT).Text.ToString().Trim() = "True" And dgItem.Cells(RET_LEGACY_ACCT_TYPE).Text.ToString().Trim() = "PA" Then
            '        For i = 0 To DatagridElectiveRetirementAccounts.Items.Count - 1

            '            If DatagridElectiveRetirementAccounts.Items(i).Cells(RET_BASIC_ACCT).Text.ToString().Trim() = "False" And DatagridElectiveRetirementAccounts.Items(i).Cells(RET_ACCT_TOTAL).Text.ToString().Trim() <> "" Then
            '                If Convert.ToDouble(DatagridElectiveRetirementAccounts.Items(i).Cells(RET_ACCT_TOTAL).Text.ToString().Trim()) > 0 Then
            '                    chk = DatagridElectiveRetirementAccounts.Items(i).Cells(RET_SEL_ACCT_CHK).Controls(1)
            '                    chk.Checked = True
            '                End If
            '            End If

            '        Next i
            '    End If
            '    'Added by Ashish for Phase V changes ,End
            'End If
            ''Ashish for Testing
            ''displayAccountBalancesAsPerRefunds()
            ''Ashish for Testing
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-DatagridElectiveRetirementAccounts_OnCheckedChanged", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Commented by Ashish for Phase v changes
            'Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
            'Added By ashish for Phase v changes
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
#End Region

#Region "Properties"
    Public Property RetireType() As String
        Get
            Return CType(Session("RP_RetireType"), String)
        End Get
        Set(ByVal Value As String)
            Session("RP_RetireType") = Value
        End Set
    End Property
    Public Property FundEventId() As String
        Get
            If Not Session("FundId") Is Nothing Then
                Return CType(Session("FundId"), String)
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("FundId") = Value
        End Set
    End Property
    Public Property FundEventStatus() As String
        Get
            If Not Session("RE_FundEventStatus") Is Nothing Then
                Return CType(Session("RE_FundEventStatus"), String)
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("RE_FundEventStatus") = Value
        End Set
    End Property
    Public Property NoBasicAccountContribution() As Boolean
        Get
            If Not Session("RE_NoBasicAccountContribution") Is Nothing Then
                Return CType(Session("RE_NoBasicAccountContribution"), Boolean)
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("RE_NoBasicAccountContribution") = Value
        End Set
    End Property
    Public Property PlanType() As String
        Get
            Return CType(Session("PlanType"), String)
        End Get
        Set(ByVal Value As String)
            Session("PlanType") = Value
        End Set
    End Property
    Public Property RetirementDate() As String
        Get
            Return Session("RE_RetirementDate")
        End Get
        Set(ByVal Value As String)
            Session("RE_RetirementDate") = Value
        End Set
    End Property
    Public Property PersonId() As String
        Get
            Return Session("PersID")
        End Get
        Set(ByVal Value As String)
            Session("PersID") = Value
        End Set
    End Property
    Public Property SSNO() As String
        Get
            Return Session("SSNo")
        End Get
        Set(ByVal Value As String)
            Session("SSNo") = Value
        End Set
    End Property
    Public ReadOnly Property FundNo() As String
        Get
            Return Session("FundNo")
        End Get

    End Property
    'ASHISH:2010.11.16:Added new Property for YRS 5.0-1215
    Public Property ExactAgeEffDate() As String
        Get
            Return CType(Session("RP_ExactAgeEffDate"), String)
        End Get
        Set(ByVal Value As String)
            Session("RP_ExactAgeEffDate") = Value
        End Set
    End Property
    'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
    Public Property AreJAnnuitiesUnavailable() As Boolean
        Get
            If Session("AreJAnnuityUnavailable") Is Nothing Then
                Return False
            Else
                Return CType(Session("AreJAnnuityUnavailable"), Boolean)
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("AreJAnnuityUnavailable") = Value
        End Set
    End Property
    'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
    Public Property InputBeneficiaryType() As Integer
        Get
            Return Session("InputBeneficiaryType")
        End Get
        Set(ByVal Value As Integer)
            Session("InputBeneficiaryType") = Value
        End Set
    End Property
    'ASHISH:2011.08.24 Added new property for YRS 5.0-1135
    'BT-798 : System should not allow disability retirement for QD and BF fundevents
    Public Property OrgBenTypeIsQDROorRBEN() As Boolean
        Get
            If Session("OrgBenTypeIsQDROorRBEN") Is Nothing Then
                Return False
            Else
                Return CType(Session("OrgBenTypeIsQDROorRBEN"), Boolean)
            End If

        End Get
        Set(ByVal Value As Boolean)
            Session("OrgBenTypeIsQDROorRBEN") = Value
        End Set
    End Property
    'Property to set Retiree Birth Date is present or not
    Public Property RetireeBirthDatePresent() As Boolean
        Get
            If Session("RetireeBirthDatePresent") Is Nothing Then
                Return False
            Else
                Return CType(Session("RetireeBirthDatePresent"), Boolean)
            End If

        End Get
        Set(ByVal Value As Boolean)
            Session("RetireeBirthDatePresent") = Value
        End Set
    End Property
    'Added by chandrasekar.c:Property to get and set Retire Service Minimun age 

    Public Property MinAgeToRetire() As Integer
        Get

            'Start - Getting Minimum Service age of the participant
            'Me.MinAgeToRetire =
            If ViewState("MinAgeToRetire") Is Nothing Then

                ViewState("MinAgeToRetire") = RetirementBOClass.GetMinimumAgeToRetire()

            End If

            Return CType(ViewState("MinAgeToRetire"), Integer)
            'End - Getting Minimum Service age of the participant


        End Get
        Set(ByVal Value As Integer)
            ViewState("MinAgeToRetire") = Value
        End Set
    End Property
 
    'Added by chandrasekar.c:Property to get and set Death Benefit Annuity Purchase Restricted Date 
    Public Property DeathBenefitAnnuityPurchaseRestrictedDate() As Date
        Get
            If ViewState("DeathBenefitAnnuityPurchaseRestrictedDate") Is Nothing Then

                'Start:Added by chandrasekar.c on 2015.11.24 for YRS-Ticket-2610 ,This is used get death benefit annuity purchase Restricted Date

                ViewState("DeathBenefitAnnuityPurchaseRestrictedDate") = RetirementBOClass.DeathBenefitAnnuityPurchaseRestrictedDate()

                'End:Added by chandrasekar.c on 2015.11.24 for YRS-Ticket-2610 ,This is used get death benefit annuity purchase Restricted Date

            End If

            Return CType(ViewState("DeathBenefitAnnuityPurchaseRestrictedDate"), Date)

        End Get
        Set(ByVal Value As Date)
            ViewState("DeathBenefitAnnuityPurchaseRestrictedDate") = Value
        End Set
    End Property
    Public Property IsDeathBenefitAnnuityPurchaseRestricted() As Boolean
        Get
            If ViewState("IsDeathBenefitAnnuityPurchaseRestricted") Is Nothing Then

                'Start:Added by chandrasekar.c on 2015.11.24 for YRS-Ticket-2610 ,This is used restricted death benefit annuity purchase based on participant age,if age below then return true else false for above 55 as of 1/12019 

                ViewState("IsDeathBenefitAnnuityPurchaseRestricted") = RetirementBOClass.IsDeathBenefitAnnuityPurchaseRestricted(Me.MinAgeToRetire, TextBoxRetireeBirthday.Text.ToString(), Me.DeathBenefitAnnuityPurchaseRestrictedDate)

                'End:Added by chandrasekar.c on 2015.11.24 for YRS-Ticket-2610 ,This is used restricted death benefit annuity purchase based on participant age,if age below then return true else false for above 55 as of 1/12019 

            End If

            Return CType(ViewState("IsDeathBenefitAnnuityPurchaseRestricted"), Boolean)

        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsDeathBenefitAnnuityPurchaseRestricted") = Value
        End Set
    End Property
    'Start:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - This prperty is used to Checking and set whether person is terminated or not
    Public Property IsPersonTerminated() As Boolean
        Get
            If ViewState("VS_IsPersonTerminated") Is Nothing Then

                ViewState("VS_IsPersonTerminated") = CheckPersonTerminated()

            End If

            Return CType(ViewState("VS_IsPersonTerminated"), Boolean)

        End Get
        Set(ByVal Value As Boolean)
            ViewState("VS_IsPersonTerminated") = Value
        End Set
    End Property
    'End:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - This prperty is used to Checking and set whether person is terminated or not
    'Start:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - This prperty is used to Get/set YMCA(LEGACY) ACCOUNT Balance as on termination if person is terminated
    Public Property PIA_At_Termination() As Decimal
        Get
            If ViewState("VS_PIA_At_Termination") Is Nothing Then

                ViewState("VS_PIA_At_Termination") = RefundRequest.GetTerminatePIA(Me.FundEventId)

            End If

            Return CType(ViewState("VS_PIA_At_Termination"), Decimal)

        End Get
        Set(ByVal Value As Decimal)
            ViewState("VS_PIA_At_Termination") = Value
        End Set
    End Property
    'End:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - This prperty is used to Get/set YMCA(LEGACY) ACCOUNT Balance as on termination if person is terminated

    'START: MMR | 2017.03.01 | YRS-AT-2625 | Declared property for manual transaction details
    Public Property ManualTransactionDetails() As DataSet
        Get
            If Session("ManualTransaction") Is Nothing Then
                Return Nothing
            Else
                Return CType(Session("ManualTransaction"), DataSet)
            End If

        End Get
        Set(ByVal Value As DataSet)
            Session("ManualTransaction") = Value
        End Set
    End Property
    'END: MMR | 2017.03.01 | YRS-AT-2625 | Declared property for manual transaction details

    'START: PPP | 03/17/2017 | YRS-AT-2625 | Property will hold C Annuity Guranteed Reserves For Disability 
    Public Property CAnnuityGuranteedReservesForDisability() As Decimal
        Get
            If Session("CAnnuityGuranteedReservesForDisability") Is Nothing Then
                Return 0
            Else
                Return Convert.ToDecimal(Session("CAnnuityGuranteedReservesForDisability"))
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("CAnnuityGuranteedReservesForDisability") = Value
        End Set
    End Property
    'END: PPP | 03/17/2017 | YRS-AT-2625 | Property will hold C Annuity Guranteed Reserves For Disability 
#End Region

    'YRS 5.0-445
    Private Sub TextBoxRetirementAge_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBoxRetirementAge.TextChanged
        'Added by Ashish  For phase V changes
        Dim empEventId As String = String.Empty
        Dim l_boolExecute As Boolean = False
        Try
            If Me.IsRetirementBackDated() Then
                dsElectiveAccountsDet = GetElectiveAccoutsByPlan(Me.FundEventId, Me.PlanType, Me.RetirementDate)
                Session("dsElectiveAccountsDet") = dsElectiveAccountsDet
                l_boolExecute = True
            Else
                'Commented by Ashish after Mark confirmation email 6-May-2009  
                '    'code for get future projected balances
                '    Page.Validate()
                '    if Page.IsValid 
                '        GetProjectedAcctBalances()
                '        l_boolExecute = True
                '    End If

                'End If
                'If l_boolExecute = True Then
                '    setEmploymentControlState()
                '    If DropDownListEmployment.Items.Count > 1 Then
                '        empEventId = DropDownListEmployment.Items(1).Value.ToString.Trim.ToUpper
                '    End If
                '    populateElectiveAccountsTab(empEventId, True, False)
                '    ValidateProjectedBalancesAsPerRefund()
                '    SetElectiveAccountSelectionState()
                '    ShowProjectedAcctBalancesTotal()
            End If
            setEmploymentControlState()
            'Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
            If TextBoxRetirementAge.Text.Trim <> "" Then
                If (Convert.ToInt32(TextBoxRetirementAge.Text.Trim) >= 55) Then  '' TODO 55 age need to be configured by KEY 
                    If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
                        chkRetirementAccount.Visible = True
                        txtRetirementAccount.Visible = True
                    End If

                    If DatagridElectiveSavingsAccounts.Items.Count > 0 Then
                        chkSavingPartialAmount.Visible = True
                        txtSavingPartialAmount.Visible = True
                    End If

                End If
            End If
            LabelEstimateDataChangedMessage.Visible = True

        Catch ex As Exception

            HelperFunctions.LogException("RetirementEsitmateWebForm -- TextBoxRetirementAge_TextChanged ", ex)
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)

        End Try
    End Sub

#Region "Phase V Changes Commented Code"
    'Added by Ashish 08-Apr-2009 For Phase V Changes,Start

    'Private Function DisplayElectiveAccountsTab() As String
    '    Try

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function
    'Private Function GetGroupedAcctSchema(ByVal para_dtElectiveAccounts As DataTable) As DataTable
    '    Dim l_dtGroupedElectiveAccount As DataTable
    '    Dim l_dtColumn As DataColumn
    '    Try
    '        l_dtGroupedElectiveAccount = New DataTable
    '        If Not para_dtElectiveAccounts Is Nothing Then
    '            l_dtGroupedElectiveAccount = para_dtElectiveAccounts.Clone()

    '        End If
    '        If l_dtGroupedElectiveAccount.Columns.Count > 0 Then
    '            'l_dtColumn = New DataColumn("LegacyAcctNumber", System.Type.GetType("Int32"))
    '            'l_dtGroupedElectiveAccount.Columns.Add(l_dtColumn)

    '            l_dtColumn = New DataColumn("bitExcluded", System.Type.GetType("Boolean"))
    '            l_dtColumn.DefaultValue = False
    '            l_dtGroupedElectiveAccount.Columns.Add(l_dtColumn)

    '            l_dtColumn = New DataColumn("bitEnabled", System.Type.GetType("Boolean"))
    '            l_dtColumn.DefaultValue = True
    '            l_dtGroupedElectiveAccount.Columns.Add(l_dtColumn)


    '            l_dtGroupedElectiveAccount.AcceptChanges()
    '        End If





    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function
    'Private Function GenerateGroupedElectiveAccountsTable(ByRef para_dtGroupedElectiveAccounts As DataTable, ByVal para_dtNonGroupedElectiveAcounts As DataTable) As DataTable
    '    Dim l_dtGroupedElectiveAcct As DataTable
    '    Dim l_ds_ElectiveAccounts As DataSet
    '    Dim l_dt_EllectiveAccounts As DataTable
    '    Try
    '        If Not para_dtGroupedElectiveAccounts Is Nothing AndAlso Not para_dtNonGroupedElectiveAcounts Is Nothing Then

    '            MappedGroupedElectiveAccounts(para_dtGroupedElectiveAccounts, para_dtNonGroupedElectiveAcounts)



    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function
    'Private Function MappedGroupedElectiveAccounts(ByRef para_dtGroupedElectiveAccounts As DataTable, ByVal para_dtNonGroupedElectiveAcounts As DataTable)
    '    Dim l_dtNonGroupedAcctRow As DataRow
    '    Dim l_dtGroupedFindRowArray As DataRow()
    '    Dim l_dtGroupedAcctNewRow As DataRow
    '    Try
    '        If Not para_dtGroupedElectiveAccounts Is Nothing AndAlso Not para_dtNonGroupedElectiveAcounts Is Nothing Then
    '            'make personal account
    '            If para_dtNonGroupedElectiveAcounts.Rows.Count > 0 Then
    '                Dim i As Int32
    '                For i = 0 To para_dtNonGroupedElectiveAcounts.Rows.Count Step 1
    '                    l_dtNonGroupedAcctRow = para_dtNonGroupedElectiveAcounts.Rows(i)
    '                    If Convert.ToBoolean(l_dtNonGroupedAcctRow("bitBasicAcct")) = True Then
    '                        'For personal account
    '                        l_dtGroupedFindRowArray = para_dtGroupedElectiveAccounts.Select("bitBasicAcct=" + l_dtNonGroupedAcctRow("bitBasicAcct") + " And chrAcctType='" + PERSONAL_ACCT + "'")
    '                        If l_dtGroupedFindRowArray.Length > 0 Then
    '                            l_dtGroupedFindRowArray(0)("AcctTotal") = Convert.ToDecimal(l_dtGroupedFindRowArray(0)("AcctTotal")) + Convert.ToDecimal(l_dtNonGroupedAcctRow("PersonalTotal"))
    '                        Else
    '                            'create new row for personal account
    '                            l_dtGroupedAcctNewRow = CreateGroupedAcctNewRow(para_dtGroupedElectiveAccounts, l_dtNonGroupedAcctRow)
    '                            l_dtGroupedAcctNewRow("chrAcctType") = PERSONAL_ACCT
    '                            para_dtGroupedElectiveAccounts.Rows.Add(l_dtGroupedAcctNewRow)
    '                        End If
    '                        para_dtGroupedElectiveAccounts.AcceptChanges()
    '                        'For YMCA account

    '                    Else 'if bitBasicAcct is False
    '                        l_dtGroupedAcctNewRow = CreateGroupedAcctNewRow(para_dtGroupedElectiveAccounts, l_dtNonGroupedAcctRow)
    '                        para_dtGroupedElectiveAccounts.Rows.Add(l_dtGroupedAcctNewRow)

    '                    End If
    '                    para_dtGroupedElectiveAccounts.AcceptChanges()
    '                Next
    '            End If

    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function
    'Private Function CreateGroupedAcctNewRow(ByVal para_dtGroupedAcct As DataTable, ByVal para_dtNonGroupedAcctRow As DataRow) As DataRow
    '    Dim l_dtGroupedAcctNewRow As DataRow
    '    Dim l_NonGroupedItemArray As Object()
    '    Try
    '        If Not para_dtGroupedAcct Is Nothing AndAlso Not para_dtNonGroupedAcctRow Is Nothing Then
    '            l_dtGroupedAcctNewRow = para_dtGroupedAcct.NewRow()
    '            l_NonGroupedItemArray = para_dtNonGroupedAcctRow.ItemArray
    '            Dim i As Int32
    '            If l_NonGroupedItemArray.Length > 0 Then
    '                For i = 0 To l_NonGroupedItemArray.Length - 1 Step 1
    '                    l_dtGroupedAcctNewRow(i) = l_NonGroupedItemArray(i)
    '                Next

    '            End If
    '            Return l_dtGroupedAcctNewRow
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function
    'Private Function AddUpdateGroupedAcctRow(ByRef para_dtGroupedElectiveAcct As DataTable, ByVal para_dtNonGroupedElectiveAcctRow As DataRow, ByVal para_GroupedAcctName As String, ByVal para_AmountType As String)
    '    Dim l_dtGroupedFindRowArray As DataRow()
    '    Dim l_dtGroupedAcctNewRow As DataRow
    '    Dim l_filterExpression As String
    '    Dim l_intLegacyAcctType As Int16
    '    Try
    '        If Not para_dtGroupedElectiveAcct Is Nothing AndAlso Not para_dtNonGroupedElectiveAcctRow Is Nothing Then
    '            If para_dtNonGroupedElectiveAcctRow.GetType.ToString() <> "System.DBNull" Then

    '            End If

    '            l_intLegacyAcctType = Convert.ToInt16(para_dtNonGroupedElectiveAcctRow("LegacyAcctType"))
    '        End If




    '        l_dtGroupedFindRowArray = para_dtGroupedElectiveAcct.Select("bitBasicAcct=" + para_dtNonGroupedElectiveAcctRow("bitBasicAcct") + " And chrAcctType='" + para_GroupedAcctName + "'")
    '        If para_GroupedAcctName Then
    '            If l_dtGroupedFindRowArray.Length > 0 Then
    '                l_dtGroupedFindRowArray(0)("AcctTotal") = Convert.ToDecimal(l_dtGroupedFindRowArray(0)("AcctTotal")) + Convert.ToDecimal(para_dtNonGroupedElectiveAcctRow(para_AmountType))
    '            Else
    '                'create new row for personal account
    '                l_dtGroupedAcctNewRow = CreateGroupedAcctNewRow(para_dtGroupedElectiveAcct, para_dtNonGroupedElectiveAcctRow)
    '                l_dtGroupedAcctNewRow("chrAcctType") = para_GroupedAcctName
    '                para_dtGroupedElectiveAccounts.Rows.Add(l_dtGroupedAcctNewRow)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function
    'Added by Ashish  For Phase V Changes,End
#End Region
#Region "Phase V  Changes"
    'Added by Ashish 15-Apr-2009 For Phase V Changes,Start
    Private Function GetElectiveAccoutsByPlan(ByVal para_FundEventID As String, ByVal para_PlanType As String, ByVal para_RetirementDate As String) As DataSet
        Dim l_dsElctiveAccountsByPlan As DataSet
        Dim l_dtNewColumn As DataColumn
        Try

            l_dsElctiveAccountsByPlan = RetirementBOClass.GetElectiveAccountsByPlan(para_FundEventID, "BOTH", para_RetirementDate)
            If Not l_dsElctiveAccountsByPlan Is Nothing Then
                If l_dsElctiveAccountsByPlan.Tables.Count > 1 Then

                    If Not l_dsElctiveAccountsByPlan.Tables("RetireeElectiveAccountsInformation") Is Nothing Then

                        'Added new column in RetireeElectiveAccountsInformation table
                        l_dtNewColumn = l_dsElctiveAccountsByPlan.Tables("RetireeElectiveAccountsInformation").Columns.Add("Selected", Type.GetType("System.Boolean"))
                        l_dtNewColumn.DefaultValue = False
                        'If l_dsElctiveAccountsByPlan.Tables("RetireeElectiveAccountsInformation").Rows.Count > 0 Then
                        '    'set selected column value 
                        '    For Each dr As DataRow In l_dsElctiveAccountsByPlan.Tables("RetireeElectiveAccountsInformation").Rows
                        '        If dr("AcctTotal") > 0 Then
                        '            dr("Selected") = True
                        '        Else
                        '            dr("Selected") = False
                        '        End If
                        '    Next
                        '    'dsElectiveAccountsDet.AcceptChanges()
                        'End If
                    End If
                    If Not l_dsElctiveAccountsByPlan.Tables("RetireeGroupedElectiveAccounts") Is Nothing Then

                        'Added new column in RetireeGroupedElectiveAccounts Table                        
                        l_dtNewColumn = l_dsElctiveAccountsByPlan.Tables("RetireeGroupedElectiveAccounts").Columns.Add("Selected", Type.GetType("System.Boolean"))
                        l_dtNewColumn.DefaultValue = False
                        l_dtNewColumn = l_dsElctiveAccountsByPlan.Tables("RetireeGroupedElectiveAccounts").Columns.Add("bitEnabled", Type.GetType("System.Boolean"))
                        l_dtNewColumn.DefaultValue = False
                        'set added column value
                        If l_dsElctiveAccountsByPlan.Tables("RetireeGroupedElectiveAccounts").Rows.Count > 0 Then
                            'set selected column value 
                            For Each dr As DataRow In l_dsElctiveAccountsByPlan.Tables("RetireeGroupedElectiveAccounts").Rows
                                If dr("AcctTotal") > 0 Then
                                    dr("Selected") = True
                                Else
                                    If dr("bitBasicAcct") = True Then
                                        dr("Selected") = True
                                    Else
                                        dr("Selected") = False
                                    End If

                                End If
                            Next
                            'dsElectiveAccountsDet.AcceptChanges()
                        End If
                    End If

                End If 'l_dsElctiveAccountsByPlan.Tables.Count
                l_dsElctiveAccountsByPlan.AcceptChanges()
            Else
                Throw New Exception("Unable to retrive account information")
            End If 'l_dsElctiveAccountsByPlan is not nothing

            Return l_dsElctiveAccountsByPlan

        Catch ex As Exception
            Throw
        End Try
    End Function
    'if para_PlanType is not empty then this function clear account assumption 
    ',i.e Plan type changed except both plan
    Private Function UpdateElectiveAccountAssumptions(ByVal para_PlanType)
        Dim filterExpression As String
        Try
            dsElectiveAccountsDet = Session("dsElectiveAccountsDet")

            Dim selectedRow As DataRow
            Dim contributionType As String = String.Empty
            Dim amt As String = String.Empty
            Dim startDate As String = String.Empty
            Dim stopDate As String = String.Empty
            Dim newEmpEventID As String = String.Empty
            Dim empEventID As String
            Dim l_LegacyAccountType As String
            Dim selected As Boolean = False
            Dim dsPersonEmploymentDetails As DataSet
            Dim drs() As DataRow
            'Added by Ashish 15-Apr-2009 Phase V changes
            Dim bool_bitBasicAcct As Boolean
            'if para_PlanType is not empty then this function clear account assumption
            Dim l_PlanType As String = String.Empty

            If para_PlanType Is Nothing Or para_PlanType = String.Empty Then
                l_PlanType = Me.PlanType
            Else
                'Reset account assumption
                startDate = String.Empty
                stopDate = String.Empty
                amt = String.Empty
                contributionType = String.Empty
                selected = False
                l_PlanType = para_PlanType
            End If
            ' Retirement Grid 
            If l_PlanType = "R" Or l_PlanType = "B" Then

                If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
                    For Each di As DataGridItem In DatagridElectiveRetirementAccounts.Items
                        ' Fetch values from dataGridRow

                        empEventID = di.Cells(RET_EMP_EVENT_ID).Text.ToUpper() 'Phase IV Changes
                        l_LegacyAccountType = di.Cells(RET_LEGACY_ACCT_TYPE).Text.ToUpper()
                        'Added by Ashish 15-Apr-2009 for Phase changes ,Start
                        newEmpEventID = String.Empty
                        bool_bitBasicAcct = di.Cells(RET_BASIC_ACCT).Text
                        If para_PlanType = String.Empty Then
                            selected = CType(di.FindControl("CheckboxRet"), CheckBox).Checked
                            contributionType = CType(di.FindControl("DropdownlistContribTypeRet"), DropDownList).SelectedValue
                            amt = CType(di.FindControl("TextboxContribAmtRet"), TextBox).Text
                            startDate = CType(di.FindControl("DateusercontrolStartRet"), DateUserControl).Text.Trim()
                            stopDate = CType(di.FindControl("DateusercontrolStopRet"), DateUserControl).Text.Trim()

                        End If



                        'Ashish:2010.10.11   commented  YRS 5.0-855,BT 624,Start 
                        'dsPersonEmploymentDetails = Session("dsPersonEmploymentDetails")
                        'If dsPersonEmploymentDetails.Tables.Count > 0 Then
                        '    If dsPersonEmploymentDetails.Tables(0).Rows.Count > 0 Then
                        '        drs = dsPersonEmploymentDetails.Tables(0).Select("guiEmpEventId='" & empEventID & "'")
                        '        If drs.Length = 0 Then
                        '            newEmpEventID = DropDownListEmployment.SelectedValue.ToString.Trim.ToUpper
                        '        End If
                        '    End If
                        'End If
                        ''commented by Ashish 15-Apr-2009 for Phase V changes
                        ''drs = dsElectiveAccountsDet.Tables(0).Select("chrAcctType='" & accountType & "' AND guiEmpEventID='" & empEventID & "'")
                        'drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select("chrLegacyAcctType='" & l_LegacyAccountType & "' AND guiEmpEventID='" & empEventID & "'")
                        'If drs.Length = 0 Then
                        '    'commented by Ashish  for Phase V changes ,Start
                        '    'filterExpression = "chrAcctType='" & accountType & "' AND guiEmpEventID='" & newEmpEventID.ToLower() & "'"
                        '    'drs = dsElectiveAccountsDet.Tables(0).Select(filterExpression)
                        '    'commented by Ashish  for Phase V changes ,End
                        '    'Added by Ashish for Phase V changes ,Start
                        '    If newEmpEventID = String.Empty Then
                        '        newEmpEventID = DropDownListEmployment.SelectedValue.ToString.Trim.ToUpper

                        '    End If
                        '    filterExpression = "chrLegacyAcctType='" & l_LegacyAccountType & "' AND guiEmpEventID='" & newEmpEventID.ToLower() & "'"


                        '    drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select(filterExpression)
                        '    If drs.Length = 0 Then
                        '        filterExpression = "chrLegacyAcctType='" & l_LegacyAccountType & "'"
                        '        drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select(filterExpression)
                        '        If drs.Length > 0 Then
                        '            drs(0)("guiEmpEventID") = newEmpEventID
                        '            newEmpEventID = String.Empty
                        '        End If
                        '        'Added by Ashish for Phase V changes ,End
                        '    End If
                        'End If
                        ''Ashish:2010.10.11  commented  YRS 5.0-855,BT 624 ,end
                        'ASHISH"2010.10.11 Added for YRS 5.0-855,BT 624

                        filterExpression = "chrLegacyAcctType='" & l_LegacyAccountType & "'"
                        drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select(filterExpression)

                        'Commented by Ashish for Phase V changes
                        'selectedRow = drs(0)
                        'Added by Ashish for Phase V changes ,Start
                        If Not drs Is Nothing AndAlso drs.Length > 0 Then 'Added by Ashish for Phase V changes 
                            selectedRow = drs(0)
                        End If
                        'Added by Ashish for Phase V changes,End
                        If Not selectedRow Is Nothing Then 'Added by Ashish for Phase V changes

                            '
                            'If newEmpEventID <> String.Empty And newEmpEventID <> empEventID Then
                            '    selectedRow.Item("guiEmpEventID") = newEmpEventID
                            'End If
                            selectedRow.Item("Selected") = selected
                            If bool_bitBasicAcct = False Then

                                selectedRow.Item("guiEmpEventID") = DropDownListEmployment.SelectedValue.ToString.Trim.ToUpper()
                                selectedRow.Item("chrAdjustmentBasisCode") = contributionType
                                If amt <> String.Empty Then
                                    selectedRow.Item("mnyAddlContribution") = Convert.ToDecimal(amt)
                                Else
                                    selectedRow.Item("mnyAddlContribution") = 0
                                End If
                                selectedRow.Item("dtmEffDate") = startDate
                                'ASHISH:2010.10.11 YRS 5.0-855,BT 624
                                If selectedRow.Item("bitVolAcctTerminated") = False Then
                                    selectedRow.Item("dtsTerminationDate") = stopDate
                                End If
                                'clear projected balances
                                If para_PlanType <> String.Empty And selectedRow.Item("bitRet_Voluntary") = True Then
                                    'If selectedRow.Item("AcctTotal") > 0 Then
                                    '    selectedRow.Item("mnyProjectedBalance") = selectedRow.Item("AcctTotal")
                                    'Else
                                    '    selectedRow.Item("mnyProjectedBalance") = 0
                                    'End If
                                    selectedRow.Item("mnyProjectedBalance") = 0
                                End If
                            End If 'bool_bitBasicAcct=False
                        End If 'Added by Ashish for Phase V changes

                    Next
                End If
            End If

            ' Savings Grid
            If l_PlanType = "S" Or l_PlanType = "B" Then
                If DatagridElectiveSavingsAccounts.Items.Count > 0 Then
                    For Each di As DataGridItem In DatagridElectiveSavingsAccounts.Items
                        ' Fetch values from dataGridRow


                        empEventID = di.Cells(SAV_EMP_EVENT_ID).Text.Trim().ToUpper()
                        l_LegacyAccountType = di.Cells(SAV_ACCT_TYPE).Text.Trim().ToUpper()
                        'Added by Ashish 15-Apr-2009 for Phase changes ,Start
                        bool_bitBasicAcct = di.Cells(SAV_BASIC_ACCT).Text
                        newEmpEventID = String.Empty
                        If para_PlanType = String.Empty Then
                            selected = CType(di.FindControl("CheckboxSav"), CheckBox).Checked
                            contributionType = CType(di.FindControl("DropdownlistContribTypeSav"), DropDownList).SelectedValue
                            amt = CType(di.FindControl("TextboxContribAmtSav"), TextBox).Text
                            startDate = CType(di.FindControl("DateusercontrolStartSav"), DateUserControl).Text.Trim()
                            stopDate = CType(di.FindControl("DateusercontrolStopSav"), DateUserControl).Text.Trim()
                        End If

                        'Ashish:2010.10.11   commented  YRS 5.0-855,BT 624,Start 
                        'dsPersonEmploymentDetails = Session("dsPersonEmploymentDetails")
                        'If dsPersonEmploymentDetails.Tables.Count > 0 Then
                        '    If dsPersonEmploymentDetails.Tables(0).Rows.Count > 0 Then
                        '        filterExpression = "guiEmpEventId='" & empEventID & "'"
                        '        drs = dsPersonEmploymentDetails.Tables(0).Select(filterExpression)
                        '        If drs.Length = 0 Then
                        '            newEmpEventID = DropDownListEmployment.SelectedValue.ToString.Trim.ToUpper
                        '        End If
                        '    End If
                        'End If
                        'filterExpression = "chrLegacyAcctType='" & l_LegacyAccountType & "' AND guiEmpEventID='" & empEventID.ToLower() & "'"
                        ''commented by Ashish for Phase V changes
                        ''drs = dsElectiveAccountsDet.Tables(0).Select(filterExpression)
                        'drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select(filterExpression)
                        'If drs.Length = 0 Then
                        '    'commented by Ashish  for Phase V changes ,Start
                        '    'filterExpression = "chrAcctType='" & accountType & "' AND guiEmpEventID='" & newEmpEventID.ToLower() & "'"
                        '    'drs = dsElectiveAccountsDet.Tables(0).Select(filterExpression)
                        '    'commented by Ashish  for Phase V changes ,End
                        '    'Added by Ashish for Phase V changes ,Start
                        '    If newEmpEventID = String.Empty Then
                        '        newEmpEventID = DropDownListEmployment.SelectedValue.ToString.Trim.ToUpper

                        '    End If
                        '    filterExpression = "chrLegacyAcctType='" & l_LegacyAccountType & "' AND guiEmpEventID='" & newEmpEventID.ToLower() & "'"


                        '    drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select(filterExpression)
                        '    If drs.Length = 0 Then
                        '        filterExpression = "chrLegacyAcctType='" & l_LegacyAccountType & "'"
                        '        drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select(filterExpression)
                        '        If drs.Length > 0 Then
                        '            drs(0)("guiEmpEventID") = newEmpEventID
                        '            newEmpEventID = String.Empty
                        '        End If
                        '        'Added by Ashish for Phase V changes ,End
                        '    End If



                        'End If
                        'Ashish:2010.10.11   commented  YRS 5.0-855,BT 624,End 
                        filterExpression = "chrLegacyAcctType='" & l_LegacyAccountType & "'"
                        drs = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select(filterExpression)
                        'Commented by Ashish for Phase V changes
                        'selectedRow = drs(0)
                        'Added by Ashish for Phase V changes ,Start
                        If Not drs Is Nothing AndAlso drs.Length > 0 Then 'Added by Ashish for Phase V changes 
                            selectedRow = drs(0)
                        End If
                        'Added by Ashish for Phase V changes,End
                        If Not selectedRow Is Nothing Then
                            'Added by Ashish for Phase V changes

                            'If newEmpEventID <> String.Empty And newEmpEventID <> empEventID Then
                            '    selectedRow.Item("guiEmpEventID") = newEmpEventID
                            'End If
                            selectedRow.Item("guiEmpEventID") = DropDownListEmployment.SelectedValue.ToString.Trim.ToUpper()
                            selectedRow.Item("Selected") = selected
                            If bool_bitBasicAcct = False Then

                                selectedRow.Item("chrAdjustmentBasisCode") = contributionType

                                If amt <> String.Empty Then
                                    selectedRow.Item("mnyAddlContribution") = Convert.ToDecimal(amt)
                                Else
                                    selectedRow.Item("mnyAddlContribution") = 0
                                End If

                                selectedRow.Item("dtmEffDate") = startDate
                                'ASHISH:2010.10.11 YRS 5.0-855,BT 624
                                If selectedRow.Item("bitVolAcctTerminated") = False Then
                                    selectedRow.Item("dtsTerminationDate") = stopDate
                                End If
                                'clear projected balances
                                If para_PlanType <> String.Empty And selectedRow.Item("bitRet_Voluntary") = True Then
                                    'If selectedRow.Item("AcctTotal") > 0 Then
                                    '    selectedRow.Item("mnyProjectedBalance") = selectedRow.Item("AcctTotal")
                                    'Else
                                    '    selectedRow.Item("mnyProjectedBalance") = 0
                                    'End If
                                    selectedRow.Item("mnyProjectedBalance") = 0
                                End If
                                'selectedRow.Item("dtsTerminationDate") = stopDate
                            End If 'bool_bitBasicAcct=False
                        End If

                    Next
                End If
            End If

            If dsElectiveAccountsDet.HasChanges Then
                dsElectiveAccountsDet.AcceptChanges()
                Session("dsElectiveAccountsDet") = dsElectiveAccountsDet
            End If
        Catch
            Throw
        End Try
    End Function
    Private Function ResetRetirementAcctDataGridRow(ByVal dgRetirementAcctRow As DataGridItem)
        Dim l_UC_DatePicker As YMCAUI.DateUserControl
        Dim l_DropDownList As DropDownList
        Dim l_TextBox As TextBox
        Dim l_string_AcctType As String
        Dim l_LabelProjBalance As Label
        Try
            If Not dgRetirementAcctRow Is Nothing Then
                l_string_AcctType = dgRetirementAcctRow.Cells(RET_ACCT_TYPE).Text.ToString().Trim()

                l_DropDownList = CType(dgRetirementAcctRow.FindControl("DropdownlistContribTypeRet"), DropDownList)
                If Not l_DropDownList Is Nothing And l_string_AcctType <> "RT" Then
                    l_DropDownList.SelectedValue = ""
                End If

                l_TextBox = CType(dgRetirementAcctRow.FindControl("TextboxContribAmtRet"), TextBox)
                If Not l_TextBox Is Nothing Then
                    l_TextBox.Text = "0"
                End If

                l_UC_DatePicker = CType(dgRetirementAcctRow.FindControl("DateusercontrolStartRet"), DateUserControl)
                If Not l_UC_DatePicker Is Nothing Then
                    l_UC_DatePicker.Text = ""
                End If

                l_UC_DatePicker = CType(dgRetirementAcctRow.FindControl("DateusercontrolStopRet"), DateUserControl)
                If Not l_UC_DatePicker Is Nothing Then
                    l_UC_DatePicker.Text = ""
                End If

                l_LabelProjBalance = CType(dgRetirementAcctRow.FindControl("LabelProjectedBalRet"), Label)
                If Not l_LabelProjBalance Is Nothing Then
                    l_LabelProjBalance.Text = "0.00"
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function RetirementAcctGridRowSelectionChangeValidation(ByVal dgRetireAcctChangedRow As DataGridItem)
        Dim l_ChangedRowLegacyAcctType As String = String.Empty
        Dim chkSelected As CheckBox
        Dim l_bool_BasicAcctType As Boolean
        Dim l_bool_ChangedAcctSelected As Boolean
        Dim l_bool_PersonalAcctSelected As Boolean
        Dim l_bool_YmcaAcctSelected As Boolean
        Dim l_bool_EmployerPaidAcctSelected As Boolean
        Dim l_bool_AcctFound As Boolean = False
        Dim l_LegacyAcctType As String = String.Empty
        Dim l_bool_AcctSelected As Boolean
        Dim l_intCounter As Int16
        Dim l_bool_Acct_PA_Found As Boolean = False
        Dim l_bool_Acct_YA_Found As Boolean = False
        Dim l_bool_Acct_EP_Found As Boolean = False

        Try

            If Not dgRetireAcctChangedRow Is Nothing Then
                l_ChangedRowLegacyAcctType = dgRetireAcctChangedRow.Cells(RET_LEGACY_ACCT_TYPE).Text.Trim()
                l_bool_BasicAcctType = dgRetireAcctChangedRow.Cells(RET_BASIC_ACCT).Text.Trim()
                If l_bool_BasicAcctType = True Then

                    chkSelected = CType(dgRetireAcctChangedRow.Cells(RET_SEL_ACCT_CHK).FindControl("CheckboxRet"), CheckBox)
                    If Not chkSelected Is Nothing Then
                        l_bool_ChangedAcctSelected = chkSelected.Checked
                    End If

                    'Get PA,YA & EP selection state
                    For Each dgRetireRow As DataGridItem In Me.DatagridElectiveRetirementAccounts.Items
                        l_LegacyAcctType = dgRetireRow.Cells(RET_LEGACY_ACCT_TYPE).Text.Trim().ToUpper()
                        chkSelected = CType(dgRetireRow.Cells(RET_SEL_ACCT_CHK).FindControl("CheckboxRet"), CheckBox)
                        If Not chkSelected Is Nothing Then
                            l_bool_AcctSelected = chkSelected.Checked
                        End If
                        If l_LegacyAcctType = "PA" Then
                            l_bool_PersonalAcctSelected = chkSelected.Checked
                            l_bool_Acct_PA_Found = True
                        End If
                        If l_LegacyAcctType = "YA" Then
                            l_bool_YmcaAcctSelected = chkSelected.Checked
                            l_bool_Acct_YA_Found = True
                        End If
                        If l_LegacyAcctType = "EP" Then
                            l_bool_EmployerPaidAcctSelected = chkSelected.Checked
                            l_bool_Acct_EP_Found = True
                        End If
                    Next

                    Select Case l_ChangedRowLegacyAcctType
                        Case "YA"



                            For Each dgRetireRow As DataGridItem In Me.DatagridElectiveRetirementAccounts.Items

                                l_LegacyAcctType = dgRetireRow.Cells(RET_LEGACY_ACCT_TYPE).Text.Trim().ToUpper()
                                If l_ChangedRowLegacyAcctType.ToUpper <> l_LegacyAcctType Then

                                    chkSelected = CType(dgRetireRow.Cells(RET_SEL_ACCT_CHK).FindControl("CheckboxRet"), CheckBox)
                                    If Not chkSelected Is Nothing Then
                                        l_bool_AcctSelected = chkSelected.Checked
                                    End If
                                    If l_bool_ChangedAcctSelected = False Then
                                        If l_bool_PersonalAcctSelected And l_bool_Acct_PA_Found = True Then
                                            If l_LegacyAcctType = "PA" Then
                                                chkSelected.Checked = False
                                            Else
                                                If dgRetireRow.Cells(RET_BASIC_ACCT).Text.Trim() = False Then
                                                    chkSelected.Checked = False
                                                    chkSelected.Enabled = False
                                                    ResetRetirementAcctDataGridRow(dgRetireRow)
                                                    'dgRetireRow.Enabled = False
                                                End If
                                            End If
                                        End If
                                    End If

                                End If
                            Next

                        Case "EP"

                            For Each dgRetireRow As DataGridItem In Me.DatagridElectiveRetirementAccounts.Items

                                l_LegacyAcctType = dgRetireRow.Cells(RET_LEGACY_ACCT_TYPE).Text.Trim().ToUpper()
                                If l_ChangedRowLegacyAcctType.ToUpper <> l_LegacyAcctType Then

                                    chkSelected = CType(dgRetireRow.Cells(RET_SEL_ACCT_CHK).FindControl("CheckboxRet"), CheckBox)
                                    If Not chkSelected Is Nothing Then
                                        l_bool_AcctSelected = chkSelected.Checked
                                    End If
                                    If l_bool_ChangedAcctSelected = False Then
                                        If l_bool_PersonalAcctSelected And l_bool_Acct_PA_Found = True Then
                                            If l_LegacyAcctType = "PA" Then
                                                chkSelected.Checked = False
                                            Else
                                                If dgRetireRow.Cells(RET_BASIC_ACCT).Text.Trim() = False Then
                                                    chkSelected.Checked = False
                                                    chkSelected.Enabled = False
                                                    ResetRetirementAcctDataGridRow(dgRetireRow)
                                                    'dgRetireRow.Enabled = False
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            Next


                        Case "PA"
                            For Each dgRetireRow As DataGridItem In Me.DatagridElectiveRetirementAccounts.Items

                                l_LegacyAcctType = dgRetireRow.Cells(RET_LEGACY_ACCT_TYPE).Text.Trim().ToUpper()
                                If l_ChangedRowLegacyAcctType.ToUpper <> l_LegacyAcctType Then

                                    chkSelected = CType(dgRetireRow.Cells(RET_SEL_ACCT_CHK).FindControl("CheckboxRet"), CheckBox)
                                    If Not chkSelected Is Nothing Then
                                        l_bool_AcctSelected = chkSelected.Checked
                                    End If
                                    If l_bool_ChangedAcctSelected = True Then
                                        If l_LegacyAcctType = "YA" Then
                                            chkSelected.Checked = True
                                        End If
                                        If l_LegacyAcctType = "EP" Then
                                            chkSelected.Checked = True
                                        End If
                                        If dgRetireRow.Cells(RET_BASIC_ACCT).Text.Trim() = False Then
                                            If Convert.ToDecimal(dgRetireRow.Cells(RET_ACCT_TOTAL).Text.Trim()) > 0 Then
                                                chkSelected.Checked = True
                                                chkSelected.Enabled = True
                                                'dgRetireRow.Enabled = True
                                            Else
                                                chkSelected.Checked = False
                                                chkSelected.Enabled = True
                                                'dgRetireRow.Enabled = True
                                            End If

                                        End If
                                    Else
                                        If dgRetireRow.Cells(RET_BASIC_ACCT).Text.Trim() = False Then
                                            chkSelected.Checked = False
                                            chkSelected.Enabled = False
                                            ResetRetirementAcctDataGridRow(dgRetireRow)
                                            'dgRetireRow.Enabled = False
                                        End If

                                    End If
                                Else
                                    'if l_LegacyAcctType='PA'
                                    If l_bool_ChangedAcctSelected = False Then
                                        ResetRetirementAcctDataGridRow(dgRetireRow)
                                    End If
                                End If
                            Next


                    End Select
                Else
                    'for non basic account
                    If l_bool_ChangedAcctSelected = False Then
                        ResetRetirementAcctDataGridRow(dgRetireAcctChangedRow)
                    End If
                End If

            End If


        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function CreateExcludedElectiveAcctRows(ByVal para_dtElectiveAccountInfo As DataTable, ByRef para_dtExcludedAccounts As DataTable, ByVal para_planType As String, ByVal para_LegacyAcctType As String, ByVal para_ISBasicAccount As Boolean)
        Dim l_dtElectiveAcctFoundRows As DataRow()
        Dim l_dtExcludedFounRows As DataRow()
        Dim l_dtExcludedAcctRow As DataRow
        Dim l_PlaneType As String
        Dim l_FilterExpression As String = String.Empty
        Dim l_intCounter As Int32
        Dim l_boolYmcaAmtExcluded As Boolean = False
        Dim l_boolPersonalAmtExcluded As Boolean = False
        Try
            If Not para_dtElectiveAccountInfo Is Nothing AndAlso Not para_dtExcludedAccounts Is Nothing AndAlso para_planType <> String.Empty AndAlso para_LegacyAcctType <> String.Empty Then


                Select Case para_LegacyAcctType
                    Case "PA"
                        l_FilterExpression = "bitBasicAcct=True AND bitPA=1 "
                        l_boolPersonalAmtExcluded = True
                    Case "YA"
                        l_FilterExpression = "bitBasicAcct=True AND bitYA=1"
                        l_boolYmcaAmtExcluded = True
                    Case "EP"
                        l_FilterExpression = "bitBasicAcct=True AND bitEP=1"
                        l_boolYmcaAmtExcluded = True
                    Case Else
                        l_FilterExpression = "chrAcctType='" & para_LegacyAcctType & "'"
                End Select

                l_dtElectiveAcctFoundRows = para_dtElectiveAccountInfo.Select(l_FilterExpression)

                If l_dtElectiveAcctFoundRows.Length > 0 Then

                    For l_intCounter = 0 To l_dtElectiveAcctFoundRows.Length - 1 Step 1

                        If para_ISBasicAccount = True Then
                            'check basic account exists
                            l_dtExcludedFounRows = para_dtExcludedAccounts.Select("chrAcctType='" & l_dtElectiveAcctFoundRows(l_intCounter)("chrAcctType") & "'")
                            If l_dtExcludedFounRows.Length = 0 Then
                                'chrAcctType not exists in ExcludedAccount table
                                l_dtExcludedAcctRow = para_dtExcludedAccounts.NewRow()
                                l_dtExcludedAcctRow("chrAcctType") = l_dtElectiveAcctFoundRows(l_intCounter)("chrAcctType")
                                l_dtExcludedAcctRow("bitRet_Voluntary") = l_dtElectiveAcctFoundRows(l_intCounter)("bitRet_Voluntary")
                                l_dtExcludedAcctRow("bitBasicAcct") = l_dtElectiveAcctFoundRows(l_intCounter)("bitBasicAcct")
                                l_dtExcludedAcctRow("chvPlanType") = l_dtElectiveAcctFoundRows(l_intCounter)("PlanType")
                                l_dtExcludedAcctRow("bitPA") = l_dtElectiveAcctFoundRows(l_intCounter)("bitPA")
                                l_dtExcludedAcctRow("bitYA") = l_dtElectiveAcctFoundRows(l_intCounter)("bitYA")
                                l_dtExcludedAcctRow("bitEP") = l_dtElectiveAcctFoundRows(l_intCounter)("bitEP")
                                If para_LegacyAcctType = "PA" Then
                                    l_dtExcludedAcctRow("bitPersonalAmtExcluded") = l_dtElectiveAcctFoundRows(l_intCounter)("bitPA")
                                End If
                                If para_LegacyAcctType = "YA" Then
                                    l_dtExcludedAcctRow("bitYmcaAmtExcluded") = l_dtElectiveAcctFoundRows(l_intCounter)("bitYA")
                                End If
                                If para_LegacyAcctType = "EP" Then
                                    l_dtExcludedAcctRow("bitYmcaAmtExcluded") = l_dtElectiveAcctFoundRows(l_intCounter)("bitEP")
                                End If
                                para_dtExcludedAccounts.Rows.Add(l_dtExcludedAcctRow)

                            Else
                                'if chrAcctType Exists in para_dtExcludedAccounts update
                                Dim i As Int16
                                For i = 0 To l_dtExcludedFounRows.Length - 1 Step 1
                                    l_dtExcludedAcctRow = l_dtExcludedFounRows(i)
                                    Select Case para_LegacyAcctType
                                        Case "PA"
                                            l_dtExcludedAcctRow("bitPersonalAmtExcluded") = l_dtElectiveAcctFoundRows(l_intCounter)("bitPA")
                                        Case "YA"
                                            l_dtExcludedAcctRow("bitYmcaAmtExcluded") = l_dtElectiveAcctFoundRows(l_intCounter)("bitYA")
                                        Case "EP"
                                            l_dtExcludedAcctRow("bitYmcaAmtExcluded") = l_dtElectiveAcctFoundRows(l_intCounter)("bitEP")

                                    End Select

                                Next

                            End If
                        Else
                            'added non basic excluded account
                            l_dtExcludedAcctRow = para_dtExcludedAccounts.NewRow()
                            l_dtExcludedAcctRow("chrAcctType") = l_dtElectiveAcctFoundRows(l_intCounter)("chrAcctType")
                            l_dtExcludedAcctRow("bitRet_Voluntary") = l_dtElectiveAcctFoundRows(l_intCounter)("bitRet_Voluntary")
                            l_dtExcludedAcctRow("bitBasicAcct") = l_dtElectiveAcctFoundRows(l_intCounter)("bitBasicAcct")
                            l_dtExcludedAcctRow("chvPlanType") = l_dtElectiveAcctFoundRows(l_intCounter)("PlanType")
                            l_dtExcludedAcctRow("bitPersonalAmtExcluded") = True
                            l_dtExcludedAcctRow("bitYmcaAmtExcluded") = True
                            para_dtExcludedAccounts.Rows.Add(l_dtExcludedAcctRow)
                        End If

                    Next
                    para_dtExcludedAccounts.AcceptChanges()

                End If

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function GetUpdatedNonGroupedElectiveAcct(ByRef para_dtUpdatedNonGroupedElectiveAcct As DataTable, ByVal para_dsElectiveAccounts As DataSet)


        Dim bool_bitBasicAcct As Boolean
        Dim l_strLegacyAcctType As String = String.Empty
        Dim l_dtNonGroupedElectiveAcct As DataTable
        Dim l_dtGroupedElectiveAcct As DataTable
        Dim l_NonGroupedAcctFoundRows As DataRow()
        Dim l_GroupedAcctFoundRows As DataRow()
        Dim filterExpression As String = String.Empty
        Dim l_strPlanType As String = String.Empty
        Try


            If Not para_dsElectiveAccounts Is Nothing Then
                If para_dsElectiveAccounts.Tables.Count > 1 Then

                    If para_dsElectiveAccounts.Tables.Contains("RetireeElectiveAccountsInformation") AndAlso para_dsElectiveAccounts.Tables.Contains("RetireeGroupedElectiveAccounts") Then
                        para_dtUpdatedNonGroupedElectiveAcct = para_dsElectiveAccounts.Tables("RetireeElectiveAccountsInformation").Clone()
                        If para_dsElectiveAccounts.Tables("RetireeElectiveAccountsInformation").Rows.Count > 0 AndAlso para_dsElectiveAccounts.Tables("RetireeGroupedElectiveAccounts").Rows.Count > 0 Then
                            l_dtNonGroupedElectiveAcct = New DataTable
                            l_dtGroupedElectiveAcct = New DataTable
                            l_dtNonGroupedElectiveAcct = para_dsElectiveAccounts.Tables("RetireeElectiveAccountsInformation")
                            l_dtGroupedElectiveAcct = para_dsElectiveAccounts.Tables("RetireeGroupedElectiveAccounts")
                        End If

                    End If
                End If
            End If

            If Not para_dtUpdatedNonGroupedElectiveAcct Is Nothing AndAlso Not l_dtNonGroupedElectiveAcct Is Nothing AndAlso Not l_dtGroupedElectiveAcct Is Nothing Then
                para_dtUpdatedNonGroupedElectiveAcct = l_dtNonGroupedElectiveAcct.Clone()
                ' Retirement Plane
                'Added basic account rows 
                If Me.PlanType = "R" Or Me.PlanType = "B" Then

                    l_NonGroupedAcctFoundRows = l_dtNonGroupedElectiveAcct.Select("bitBasicAcct=True And PlanType='RETIREMENT'")
                    If l_NonGroupedAcctFoundRows.Length > 0 Then
                        l_GroupedAcctFoundRows = l_dtGroupedElectiveAcct.Select("bitBasicAcct=True And PlanType='RETIREMENT'")
                        For Each nonGroupedAcctRow As DataRow In l_NonGroupedAcctFoundRows
                            If l_GroupedAcctFoundRows.Length > 0 Then
                                nonGroupedAcctRow("guiEmpEventID") = l_GroupedAcctFoundRows(0)("guiEmpEventID")
                                para_dtUpdatedNonGroupedElectiveAcct.ImportRow(nonGroupedAcctRow)
                            End If

                        Next
                    End If
                    'added non basic account and update account contribution
                    l_NonGroupedAcctFoundRows = Nothing
                    l_GroupedAcctFoundRows = l_dtGroupedElectiveAcct.Select("bitBasicAcct=False And PlanType='RETIREMENT'")
                    If l_GroupedAcctFoundRows.Length Then
                        For Each groupedAcctRow As DataRow In l_GroupedAcctFoundRows
                            If groupedAcctRow("Selected") = True Then
                                l_NonGroupedAcctFoundRows = l_dtNonGroupedElectiveAcct.Select("bitBasicAcct=False And PlanType='RETIREMENT' And  chrAcctType='" & groupedAcctRow("chrLegacyAcctType") & "'")
                                If l_NonGroupedAcctFoundRows.Length > 0 Then
                                    l_NonGroupedAcctFoundRows(0)("guiEmpEventID") = groupedAcctRow("guiEmpEventID")
                                    l_NonGroupedAcctFoundRows(0)("chrAdjustmentBasisCode") = groupedAcctRow("chrAdjustmentBasisCode")
                                    l_NonGroupedAcctFoundRows(0)("mnyAddlContribution") = IIf(groupedAcctRow("mnyAddlContribution").ToString() <> String.Empty, Convert.ToDecimal(groupedAcctRow("mnyAddlContribution")), 0)
                                    l_NonGroupedAcctFoundRows(0)("dtmEffDate") = IIf(groupedAcctRow("dtmEffDate").ToString() <> String.Empty, groupedAcctRow("dtmEffDate"), String.Empty)
                                    l_NonGroupedAcctFoundRows(0)("dtsTerminationDate") = IIf(groupedAcctRow("dtsTerminationDate").ToString() <> String.Empty, groupedAcctRow("dtsTerminationDate"), String.Empty)
                                    l_NonGroupedAcctFoundRows(0)("Selected") = groupedAcctRow("Selected")
                                    para_dtUpdatedNonGroupedElectiveAcct.ImportRow(l_NonGroupedAcctFoundRows(0))
                                End If
                            End If
                        Next
                    End If
                End If
                'Saving Plan
                If Me.PlanType = "S" Or Me.PlanType = "B" Then


                    'added non basic account and update account contribution

                    l_NonGroupedAcctFoundRows = Nothing
                    l_GroupedAcctFoundRows = l_dtGroupedElectiveAcct.Select("bitBasicAcct=False And PlanType='SAVINGS'")
                    If l_GroupedAcctFoundRows.Length Then
                        For Each groupedAcctRow As DataRow In l_GroupedAcctFoundRows
                            If groupedAcctRow("Selected") = True Then
                                l_NonGroupedAcctFoundRows = l_dtNonGroupedElectiveAcct.Select("bitBasicAcct=False And PlanType='SAVINGS'  And chrAcctType='" & groupedAcctRow("chrLegacyAcctType") & "'")
                                If l_NonGroupedAcctFoundRows.Length > 0 Then
                                    l_NonGroupedAcctFoundRows(0)("guiEmpEventID") = groupedAcctRow("guiEmpEventID")
                                    l_NonGroupedAcctFoundRows(0)("chrAdjustmentBasisCode") = groupedAcctRow("chrAdjustmentBasisCode")
                                    l_NonGroupedAcctFoundRows(0)("mnyAddlContribution") = IIf(groupedAcctRow("mnyAddlContribution").ToString() <> String.Empty, Convert.ToDecimal(groupedAcctRow("mnyAddlContribution")), 0)
                                    l_NonGroupedAcctFoundRows(0)("dtmEffDate") = IIf(groupedAcctRow("dtmEffDate").ToString() <> String.Empty, groupedAcctRow("dtmEffDate"), String.Empty)
                                    l_NonGroupedAcctFoundRows(0)("dtsTerminationDate") = IIf(groupedAcctRow("dtsTerminationDate").ToString() <> String.Empty, groupedAcctRow("dtsTerminationDate"), String.Empty)
                                    l_NonGroupedAcctFoundRows(0)("Selected") = True
                                    para_dtUpdatedNonGroupedElectiveAcct.ImportRow(l_NonGroupedAcctFoundRows(0))
                                End If
                            End If
                        Next
                    End If

                End If



            End If
            para_dtUpdatedNonGroupedElectiveAcct.AcceptChanges()


        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function getElectiveAccounts() As DataSet
        Dim l_dsNewElectiveAccounts As DataSet
        Dim l_dsUpdatedElectiveAccounts As DataSet
        Dim l_dtUpdatedNonGroupedElectiveAccounts As DataTable
        Try
            Me.UpdateElectiveAccountAssumptions(String.Empty)

            If Not Session("dsElectiveAccountsDet") Is Nothing And TypeOf Session("dsElectiveAccountsDet") Is DataSet Then
                l_dsNewElectiveAccounts = DirectCast(Session("dsElectiveAccountsDet"), DataSet).Copy()
                'l_dsNewElectiveAccounts = l_dsNewElectiveAccounts.Copy()

                For Each dr As DataRow In l_dsNewElectiveAccounts.Tables("RetireeGroupedElectiveAccounts").Rows
                    Select Case dr("chrAdjustmentBasisCode")
                        Case "M"
                            dr("chrAdjustmentBasisCode") = RetirementBOClass.MONTHLY_PAYMENTS
                        Case "P"
                            dr("chrAdjustmentBasisCode") = RetirementBOClass.MONTHLY_PERCENT_SALARY
                        Case "L"
                            dr("chrAdjustmentBasisCode") = RetirementBOClass.ONE_LUMP_SUM
                        Case "Y"
                            dr("chrAdjustmentBasisCode") = RetirementBOClass.YEARLY_LUMP_SUM_PAYMENT
                    End Select
                Next

                ' This will happen when the participant doesnt have any employment records like a QDRO participant 
                If Session("dsElectiveAccounts") Is Nothing Then
                    Session("dsElectiveAccounts") = RetirementBOClass.SearchElectiveAccounts(Me.PersonId)
                End If

                Dim l_dtExistingElectiveAccounts As New DataTable

                If Not Session("dsElectiveAccounts") Is Nothing And TypeOf Session("dsElectiveAccounts") Is DataSet Then

                    If DirectCast(Session("dsElectiveAccounts"), DataSet).Tables.Count > 0 Then
                        l_dtExistingElectiveAccounts = DirectCast(Session("dsElectiveAccounts"), DataSet).Tables(0).Copy()
                        If Not l_dtExistingElectiveAccounts.Columns.Contains("Selected") Then
                            l_dtExistingElectiveAccounts.Columns.Add("Selected", System.Type.GetType("System.Boolean"))
                        End If
                    End If
                End If



                l_dsUpdatedElectiveAccounts = New DataSet

                l_dtUpdatedNonGroupedElectiveAccounts = New DataTable

                GetUpdatedNonGroupedElectiveAcct(l_dtUpdatedNonGroupedElectiveAccounts, l_dsNewElectiveAccounts)
                'l_dsElectiveAccounts = l_dsNewElectiveAccounts.Clone()

                Dim drExistingElectAcctFoundRow As DataRow()
                Dim dtUpdatedNonGroupedAcctCopy As DataTable
                dtUpdatedNonGroupedAcctCopy = l_dtUpdatedNonGroupedElectiveAccounts.Copy()
                ' Get the Retirement accounts            
                'Added existing elective retirement account row for projection & estimate
                'If start date are defind by user then put these start date as termination date for existing elective account row
                If Me.PlanType = "R" Or Me.PlanType = "B" Then

                    For Each drUpdatedNonGroupedCopyRow As DataRow In dtUpdatedNonGroupedAcctCopy.Rows
                        If drUpdatedNonGroupedCopyRow("PlanType") = "RETIREMENT" Then
                            drExistingElectAcctFoundRow = l_dtExistingElectiveAccounts.Select("chrAcctType='" & drUpdatedNonGroupedCopyRow("chrAcctType") & "' And PlanType='" & drUpdatedNonGroupedCopyRow("PlanType") & "'")
                            If drExistingElectAcctFoundRow.Length > 0 Then
                                If drUpdatedNonGroupedCopyRow("dtmEffDate").ToString().Trim() <> String.Empty Then
                                    drExistingElectAcctFoundRow(0)("dtsTerminationDate") = DateAdd(DateInterval.Day, -1, Convert.ToDateTime(drUpdatedNonGroupedCopyRow("dtmEffDate"))).ToString()
                                Else
                                    If drUpdatedNonGroupedCopyRow("dtsTerminationDate").ToString().Trim() <> String.Empty Then
                                        drExistingElectAcctFoundRow(0)("dtsTerminationDate") = drUpdatedNonGroupedCopyRow("dtsTerminationDate")
                                    End If
                                End If
                                l_dtUpdatedNonGroupedElectiveAccounts.ImportRow(drExistingElectAcctFoundRow(0))
                            End If
                        End If

                    Next
                End If

                ' Get the Savings accounts
                'Added existing elective saving accounts row for projection & estimate
                'If start date are defind by user then put these start date as termination date for existing elective account row
                If Me.PlanType = "S" Or Me.PlanType = "B" Then

                    For Each drUpdatedNonGroupedCopyRow As DataRow In dtUpdatedNonGroupedAcctCopy.Rows
                        If drUpdatedNonGroupedCopyRow("PlanType") = "SAVINGS" Then
                            drExistingElectAcctFoundRow = l_dtExistingElectiveAccounts.Select("chrAcctType='" & drUpdatedNonGroupedCopyRow("chrAcctType") & "' And PlanType='" & drUpdatedNonGroupedCopyRow("PlanType") & "'")
                            If drExistingElectAcctFoundRow.Length > 0 Then
                                If drUpdatedNonGroupedCopyRow("dtmEffDate").ToString().Trim() <> String.Empty Then
                                    drExistingElectAcctFoundRow(0)("dtsTerminationDate") = DateAdd(DateInterval.Day, -1, Convert.ToDateTime(drUpdatedNonGroupedCopyRow("dtmEffDate"))).ToString()
                                Else
                                    If drUpdatedNonGroupedCopyRow("dtsTerminationDate").ToString().Trim() <> String.Empty Then
                                        drExistingElectAcctFoundRow(0)("dtsTerminationDate") = drUpdatedNonGroupedCopyRow("dtsTerminationDate")
                                    End If
                                End If

                                l_dtUpdatedNonGroupedElectiveAccounts.ImportRow(drExistingElectAcctFoundRow(0))
                            End If
                        End If

                    Next

                End If
                l_dtUpdatedNonGroupedElectiveAccounts.AcceptChanges()
                l_dsUpdatedElectiveAccounts.Tables.Add(l_dtUpdatedNonGroupedElectiveAccounts)

            End If

            Return l_dsUpdatedElectiveAccounts

        Catch
            Throw
        End Try
    End Function
    'This method get projected balances
    Private Function GetProjectedAcctBalances() As Boolean
        Dim dsRetEstimateEmployment As DataSet
        Dim l_dtNonGroupedElectiveAcct As DataTable
        Dim l_dtNonGroupedProjectedAcctWiseBalances As DataTable
        Dim l_dtGroupedProjectedBalances As DataTable
        Dim l_dtGroupedElectiveAcct As DataTable
        Dim l_boolReturn As Boolean = True
        Try


            'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
            ''True parameter for ignoring validation message
            'dsRetEstimateEmployment = ValidateEstimate(True)
            ValidateEstimate(True)
            If Not Session("dsPersonEmploymentDetails") Is Nothing Then
                dsRetEstimateEmployment = DirectCast(Session("dsPersonEmploymentDetails"), DataSet)
            End If
            If Not dsRetEstimateEmployment Is Nothing Then
                saveActiveSalaryInformation()
                'ASHISH:2010.10.11 code Added for YRS 5.0-855 BT-624
                'UpdateEmployeeInformation()

                ' Get the correct elective account details as per the Plan type selected by the user.
                Dim dataSetElectiveAccounts As DataSet
                Dim i As Integer
                dataSetElectiveAccounts = getElectiveAccounts()





                '' If Future salary is entered and Effective date is not then set it to Today's date
                'If TextBoxFutureSalary.Text <> String.Empty And TextBoxFutureSalaryEffDate.Text = String.Empty Then
                '    TextBoxFutureSalaryEffDate.Text = DateTime.Today.AddDays(1).ToShortDateString()
                'End If

                'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624 Start
                ''Added by Ashish 31-July-2008 YRS 5.0-445 ,Start
                'If TextBoxModifiedSal.Text = String.Empty Then
                '    TextBoxModifiedSal.Text = "0.00"
                'End If

                ''Added by Ashish 31-July-2008 YRS 5.0-445 ,End
                'Dim futureSalErrorMsg As String = String.Empty
                'Dim l_boolBasicAcctExists As Boolean = False
                'Dim isMonthlyPercentageContributionPresent As Boolean = False

                ''chech for any basic account exists in Elective account
                ''Commented by Ashish for Issue YRS 5.0-830
                '' Dim drBasicAcctExists As DataRow() = dataSetElectiveAccounts.Tables("RetireeElectiveAccountsInformation").Select("bitBasicAcct=True AND bitFutureAcctVisible=1 AND AcctTotal >0")
                ''Added by Ashish for Issue YRS 5.0-830 Start
                'Dim drBasicAcctExists As DataRow()
                'If Not Session("dsElectiveAccountsDet") Is Nothing And TypeOf Session("dsElectiveAccountsDet") Is DataSet Then
                '    If DirectCast(Session("dsElectiveAccountsDet"), DataSet).Tables.Contains("RetireeElectiveAccountsInformation") Then
                '        drBasicAcctExists = DirectCast(Session("dsElectiveAccountsDet"), DataSet).Tables("RetireeElectiveAccountsInformation").Select("bitBasicAcct=True AND bitFutureAcctVisible=1 AND AcctTotal >0")
                '    End If

                'End If
                ''Added by Ashish for Issue YRS 5.0-830 End
                'If drBasicAcctExists.Length > 0 Then
                '    l_boolBasicAcctExists = True
                'End If

                ''check for if additional contribution of type Monthly Percentage is prsesent

                'Dim drMonthPerc As DataRow() = dataSetElectiveAccounts.Tables("RetireeElectiveAccountsInformation").Select("chrAdjustmentBasisCode='" & RetirementBOClass.MONTHLY_PERCENT_SALARY & "'")
                'If drMonthPerc.Length > 0 Then
                '    isMonthlyPercentageContributionPresent = True
                'End If

                'If (Not l_boolBasicAcctExists) And (isMonthlyPercentageContributionPresent) Then
                '    'Commented by Ashish 20-Jul-2009 for Issue YRS 5.0-830, start
                '    'If TextBoxFutureSalary.Text = String.Empty Or TextBoxFutureSalary.Text = "0.00" Or TextBoxFutureSalary.Text = "0" Or TextBoxFutureSalaryEffDate.Text = String.Empty Then

                '    '    If Me.FundEventStatus = "PE" Or Me.FundEventStatus = "PEML" Or Me.FundEventStatus = "RE" Then
                '    '        futureSalErrorMsg = "Participant's fund status is Pre-Eligible & does not have contributions to any basic account, so please specify the Future Salary and its Effective Date to proceed further."
                '    '    Else
                '    '        futureSalErrorMsg = "Participant does not have contributions to any basic account, so please specify the Future Salary and its Effective Date to proceed further."
                '    '    End If
                '    'End If
                '    'Commented by Ashish 20-Jul-2009 for Issue YRS 5.0-830, End
                '    'Added by Ashish 20-jul-2009 for Issue YRS 5.0-830 ,start
                '    If TextBoxFutureSalary.Text = String.Empty Or TextBoxFutureSalary.Text = "0.00" Or TextBoxFutureSalary.Text = "0" Or TextBoxFutureSalaryEffDate.Text = String.Empty Then

                '        If Me.FundEventStatus = "PE" Or Me.FundEventStatus = "PEML" Or Me.FundEventStatus = "RE" Then
                '            futureSalErrorMsg = "Participant's fund status is Pre-Eligible & does not have contributions to any basic account, so please specify the Future Salary and its Effective Date to proceed further."
                '        Else
                '            If Me.FundEventStatus = "AE" Or Me.FundEventStatus = "RA" Then
                '                If TextBoxModifiedSal.Text.Trim() = String.Empty Or Convert.ToDecimal(IIf(TextBoxModifiedSal.Text.Trim() = String.Empty, 0, TextBoxModifiedSal.Text.Trim())) = 0 Then
                '                    futureSalErrorMsg = "Participant does not have contributions to any basic account, so please specify the Future Salary and its Effective Date to proceed further."
                '                    'TextBoxFutureSalary.ReadOnly = False
                '                    'TextBoxFutureSalaryEffDate.Enabled = True

                '                End If
                '            End If

                '        End If
                '        'Added by Ashish 20-jul-2009 for Issue YRS 5.0-830 ,End

                '    End If

                'End If
                'If futureSalErrorMsg <> String.Empty Then
                '    'ShowCustomMessage(futureSalErrorMsg, enumMessageBoxType.DotNet)
                '    'Exit Function
                '    'l_boolReturn = False
                '    'Added by Ashish 20-jul-2009 for Issue YRS 5.0-830 ,start
                '    Dim tmpErrorMassage As String
                '    If Me.IsRetirementBackDated = True Then
                '        tmpErrorMassage = "Salary Information missing & estimate cannot proceed further."
                '    Else
                '        tmpErrorMassage = futureSalErrorMsg
                '    End If
                '    ShowCustomMessage(tmpErrorMassage, enumMessageBoxType.DotNet)
                '    Exit Function
                '    l_boolReturn = False
                'End If
                'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624 End
                ''Added by Ashish for Issue YRS 5.0-835 ,Start
                ''Validation for if enter future salary  then must define salary effective date 

                'Dim l_ValidationErrorMessage As String = String.Empty
                'If TextBoxFutureSalary.Enabled AndAlso TextBoxFutureSalaryEffDate.Enabled AndAlso TextBoxFutureSalary.Text.Trim() <> String.Empty Then
                '    If Convert.ToDecimal(TextBoxFutureSalary.Text.Trim()) > 0 AndAlso TextBoxFutureSalaryEffDate.Text.Trim() = String.Empty Then
                '        l_ValidationErrorMessage = "Future Salary Date : Please define future salary date. <br>"
                '    End If
                'End If
                ''Validation for if enter annual salary increment then must define salary increment effective date 
                'If Not DropDownAnnualSalaryIncrease Is Nothing AndAlso DropDownAnnualSalaryIncrease.Enabled AndAlso TextBoxAnnualSalaryIncreaseEffDate.Enabled Then
                '    If Convert.ToDecimal(IIf(DropDownAnnualSalaryIncrease.SelectedValue.Trim() = String.Empty, 0, DropDownAnnualSalaryIncrease.SelectedValue.Trim())) > 0 AndAlso TextBoxAnnualSalaryIncreaseEffDate.Text.Trim() = String.Empty Then
                '        l_ValidationErrorMessage = "Annual Salary Increase Effective Date: Please define annual salary effective date."
                '    End If

                'End If
                'If l_ValidationErrorMessage <> String.Empty Then
                '    ShowCustomMessage(l_ValidationErrorMessage, enumMessageBoxType.DotNet)
                '    Exit Function
                '    l_boolReturn = False
                'End If
                ''Added by Ashish for Issue YRS 5.0-835 ,End

                'Step 1. Calculate the Account Balances
                Dim hasNoErrors As Boolean = True
                Dim combinedDataset = New DataSet
                Dim errorMessage As String
                Dim warningMessage As String = String.Empty
                Dim isYmcaLegacyAcctTotalExceed As Boolean = False
                Dim isYmcaAcctTotalExceed As Boolean = False
                Dim l_strMaxEmpTerminationDate As String
                Dim bIsRetirementPartial As Boolean = False
                Dim bIsSavingPartial As Boolean = False
                Dim decRetirementPartialAmt As Decimal = 0
                Dim decSavingPartialAmt As Decimal = 0
                businessLogic = Session("businessLogic")
                'Dim excludedDataTable As DataTable = getExcludedAccounts()
                Dim excludedDataTable As DataTable = New DataTable
                Dim employmentDetails As DataTable = Session("employmentSalaryInformation")
                'get max employe termination date
                l_strMaxEmpTerminationDate = GetTerminationDateForProjBalValidation(TextBoxRetirementDate.Text.Trim(), employmentDetails, dsRetEstimateEmployment.Tables(0))
                'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                'hasNoErrors = businessLogic.CalculateAccountBalancesAndProjections(dsRetEstimateEmployment _
                ', TextBoxRetireeBirthday.Text, TextBoxFutureSalary.Text, TextBoxRetirementDate.Text _
                ', TextBoxFutureSalaryEffDate.Text, TextBoxModifiedSal.Text, TextBoxEndWorkDate.Text _
                ', Me.PersonId, Me.FundEventId, Me.RetireType _
                ', Convert.ToDouble(ListBoxProjectedYearInterest.SelectedValue), dataSetElectiveAccounts _
                ', DropDownAnnualSalaryIncrease.SelectedValue, TextBoxAnnualSalaryIncreaseEffDate.Text, combinedDataset, True, Me.PlanType _
                ', Me.FundEventStatus, errorMessage, warningMessage, excludedDataTable, employmentDetails, True _
                ', l_strMaxEmpTerminationDate, isYmcaLegacyAcctTotalExceed, isYmcaAcctTotalExceed)
                'hasNoErrors = businessLogic.CalculateAccountBalancesAndProjections(dsRetEstimateEmployment _
                ', TextBoxRetireeBirthday.Text, TextBoxRetirementDate.Text _
                ', Me.PersonId, Me.FundEventId, Me.RetireType _
                ', Convert.ToDouble(ListBoxProjectedYearInterest.SelectedValue), dataSetElectiveAccounts _
                ', combinedDataset, True, Me.PlanType _
                ', Me.FundEventStatus, errorMessage, warningMessage, excludedDataTable, employmentDetails, True _
                ', l_strMaxEmpTerminationDate, isYmcaLegacyAcctTotalExceed, isYmcaAcctTotalExceed)
                Dim transactionDetails As DataSet 'MMR | 2017.03.03 | YRS-AT-2625 | Declared object variable

                If chkRetirementAccount.Checked Then
                    bIsRetirementPartial = True
                End If

                If chkSavingPartialAmount.Checked Then
                    bIsSavingPartial = True
                End If

                If txtRetirementAccount.Text.Trim() = String.Empty Then
                    decRetirementPartialAmt = "0"
                Else
                    decRetirementPartialAmt = txtRetirementAccount.Text.Trim()
                End If


                If txtSavingPartialAmount.Text.Trim() = String.Empty Then
                    decSavingPartialAmt = "0"
                Else
                    decSavingPartialAmt = txtSavingPartialAmount.Text.Trim()
                End If

                'START: PPP | 03/20/2017 | YRS-AT-2625 | In disablility case we have to send "NORMAL" average salary, So that from current date to retirement date estimation will work correcty based on "NORMAL"
                If Me.RetireType = "DISABL" Then
                    dsRetEstimateEmployment = RetirementBOClass.SearchRetEmpInfo(Me.FundEventId, "NORMAL", IIf(String.IsNullOrEmpty(TextBoxRetirementDate.Text), "01/01/1900", TextBoxRetirementDate.Text))
                    employmentDetails = saveActiveSalaryInformation(dsRetEstimateEmployment)
                End If
                'END: PPP | 03/20/2017 | YRS-AT-2625 | In disablility case we have to send "NORMAL" average salary, So that from current date to retirement date estimation will work correcty based on "NORMAL"

                'START: MMR | 2017.03.03 | YRS-AT-2625 | Adding manual transaction details to existing dataset for inclusion in computing average salary
                If dsRetEstimateEmployment.Tables.Contains("ManualTransactionDetails") Then
                    dsRetEstimateEmployment.Tables.Remove("ManualTransactionDetails")
                End If
                If Not Me.ManualTransactionDetails Is Nothing Then                    
                    transactionDetails = Me.ManualTransactionDetails
                    dsRetEstimateEmployment.Tables.Add(transactionDetails.Tables("ManualTransactionDetails").Copy())
                End If
                'END: MMR | 2017.03.03 | YRS-AT-2625 | Adding manual transaction details to existing dataset for inclusion in computing average salary

                'START: PPP | 11/16/2017 | YRS-AT-3328 | Passing Me.IsPersonTerminated as True because Me.IsPersonTerminated value is used to fetch withdrawal configuration only
                ' So that terminated participants withrawal rules will get applied to Active participants also.
                ' It is passed to "YMCARET.YmcaBusinessObject.RefundRequest.GetRefundRequestConfiguration" inside in RetirementBO class.
                ' OLD
                ' hasNoErrors = businessLogic.CalculateAccountBalancesAndProjections(dsRetEstimateEmployment _
                ', TextBoxRetireeBirthday.Text, TextBoxRetirementDate.Text _
                ', Me.PersonId, Me.FundEventId, Me.RetireType _
                ', Convert.ToDouble(ListBoxProjectedYearInterest.SelectedValue), dataSetElectiveAccounts _
                ', combinedDataset, True, "B" _
                ', Me.FundEventStatus, errorMessage, warningMessage, excludedDataTable, employmentDetails, True _
                ', l_strMaxEmpTerminationDate, isYmcaLegacyAcctTotalExceed, isYmcaAcctTotalExceed, Convert.ToInt32(TextBoxRetirementAge.Text.Trim), Me.IsPersonTerminated)
                ' NEW
                hasNoErrors = businessLogic.CalculateAccountBalancesAndProjections(dsRetEstimateEmployment _
               , TextBoxRetireeBirthday.Text, TextBoxRetirementDate.Text _
               , Me.PersonId, Me.FundEventId, Me.RetireType _
               , Convert.ToDouble(ListBoxProjectedYearInterest.SelectedValue), dataSetElectiveAccounts _
               , combinedDataset, True, "B" _
               , Me.FundEventStatus, errorMessage, warningMessage, excludedDataTable, employmentDetails, True _
               , l_strMaxEmpTerminationDate, isYmcaLegacyAcctTotalExceed, isYmcaAcctTotalExceed, Convert.ToInt32(TextBoxRetirementAge.Text.Trim), True)
                'END: PPP | 11/16/2017 | YRS-AT-3328 | Passing Me.IsPersonTerminated as True because Me.IsPersonTerminated value is used to fetch withdrawal configuration only

                'Check if any error has been reported.  
                If Not hasNoErrors And (errorMessage <> "R" And errorMessage <> "S") Then
                    errorMessage = getmessage(errorMessage)
                    ShowCustomMessage(errorMessage, enumMessageBoxType.DotNet)
                    Return False
                    'Exit Function
                End If

                'warningMessage = "Warnings"
                If warningMessage <> String.Empty Then
                    LabelWarningMessage.Visible = True
                Else
                    LabelWarningMessage.Visible = False
                End If

                LabelWarningMessage.Text = combinemessage(warningMessage)
                'set Account exceed flag into session variable
                Session("isYmcaLegacyAcctTotalExceed") = isYmcaLegacyAcctTotalExceed
                Session("isYmcaAcctTotalExceed") = isYmcaAcctTotalExceed
                'get account wise balances 
                If isNonEmpty(combinedDataset) Then
                    If Not Session("dsElectiveAccountsDet") Is Nothing Then
                        If Session("dsElectiveAccountsDet").Tables.Count > 1 Then
                            l_dtNonGroupedProjectedAcctWiseBalances = New DataTable
                            l_dtNonGroupedElectiveAcct = CType(Session("dsElectiveAccountsDet").Tables("RetireeElectiveAccountsInformation"), DataTable)
                            l_dtGroupedElectiveAcct = CType(Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts"), DataTable)
                            'l_dtNonGroupedElectiveAcct = l_dtNonGroupedElectiveAcct.Copy()
                            l_dtNonGroupedProjectedAcctWiseBalances = l_dtNonGroupedElectiveAcct.Clone()
                            'get Account wise total balances
                            GetNonGroupedAcctWiseBalances(l_dtNonGroupedProjectedAcctWiseBalances, l_dtNonGroupedElectiveAcct, combinedDataset.Tables(0))
                            If l_dtNonGroupedProjectedAcctWiseBalances.Rows.Count > 0 Then
                                l_dtGroupedProjectedBalances = CreateNonGroupedAcctToGroupedAcctTable(l_dtNonGroupedProjectedAcctWiseBalances)
                                If Not l_dtGroupedProjectedBalances Is Nothing Then
                                    UpdateProjectedBalanceInBaseGroupedAcctTable(l_dtGroupedElectiveAcct, l_dtGroupedProjectedBalances)
                                End If

                            End If

                            RefreshElectiveAccountsTab(True) 'PPP | 03/17/2017 | YRS-AT-2625 | Following method will compare original accounts loaded on Accounts tab for retirement with account results after projection
                        End If
                    End If
                    'if isNonEmpty( 

                    'GetAccountWiseBalances(combinedDataset)
                End If


                tabStripRetirementEstimate.SelectedIndex = 3
                Me.MultiPageRetirementEstimate.SelectedIndex = 3 'Me.tabStripRetirementEstimate.SelectedIndex

            End If
            Return l_boolReturn
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function GetNonGroupedAcctWiseBalances(ByRef para_dtNonGroupedAcctWiseTotalBalances As DataTable, ByVal para_NonGroupedElectiveAccount As DataTable, ByVal para_dtProjectedDetailsAcctBalances As DataTable)
        Dim ProjectedYMCAAmt As Double
        Dim ProjectedPersonalAmt As Double
        Dim mnyProjectedBalances As Double
        Dim l_drNonGroupedTotalBalancesRow As DataRow
        Dim l_dtNew_NonGroupedElectiveAccount As DataTable
        Dim l_dtNonGroupedProjectedDetailsRow As DataRow()
        Try
            If Not para_dtNonGroupedAcctWiseTotalBalances Is Nothing AndAlso Not para_NonGroupedElectiveAccount Is Nothing AndAlso Not para_dtProjectedDetailsAcctBalances Is Nothing Then
                l_dtNew_NonGroupedElectiveAccount = para_NonGroupedElectiveAccount.Copy()

                For Each drNonGroupedElectiveAcctRow As DataRow In l_dtNew_NonGroupedElectiveAccount.Rows
                    l_dtNonGroupedProjectedDetailsRow = para_dtProjectedDetailsAcctBalances.Select("chrAcctType='" & drNonGroupedElectiveAcctRow("chrAcctType") & "' AND chvPlanType='" & drNonGroupedElectiveAcctRow("PlanType") & "'")
                    If l_dtNonGroupedProjectedDetailsRow.Length > 0 Then
                        ProjectedPersonalAmt = Convert.ToDouble(para_dtProjectedDetailsAcctBalances.Compute("SUM(PersonalAmt)", "chrAcctType='" & drNonGroupedElectiveAcctRow("chrAcctType") & "' AND chvPlanType='" & drNonGroupedElectiveAcctRow("PlanType") & "'"))
                        ProjectedYMCAAmt = Convert.ToDouble(para_dtProjectedDetailsAcctBalances.Compute("SUM(YMCAAmt)", "chrAcctType='" & drNonGroupedElectiveAcctRow("chrAcctType") & "' AND chvPlanType='" & drNonGroupedElectiveAcctRow("PlanType") & "'"))
                        mnyProjectedBalances = ProjectedPersonalAmt + ProjectedYMCAAmt
                        drNonGroupedElectiveAcctRow("mnyProjectedPersBalance") = Math.Round(ProjectedPersonalAmt, 2)
                        drNonGroupedElectiveAcctRow("mnyProjectedYmcaBalance") = Math.Round(ProjectedYMCAAmt, 2)
                        drNonGroupedElectiveAcctRow("mnyProjectedBalance") = Math.Round(mnyProjectedBalances, 2)

                        para_dtNonGroupedAcctWiseTotalBalances.ImportRow(drNonGroupedElectiveAcctRow)
                    End If
                Next
                para_dtNonGroupedAcctWiseTotalBalances.AcceptChanges()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function CreateNonGroupedAcctToGroupedAcctTable(ByVal para_dtNonGroupedAcctWiseProjectedBalance As DataTable) As DataTable
        Dim l_dtGroupedProjectedBalances As DataTable
        Dim l_dtNew_dtNonGroupedAcctWiseProjectedBalance As DataTable
        Dim l_drNonGroupedAcctFoundRow As DataRow()
        Dim l_filterExpression As String = String.Empty
        Dim mnyProjectedTotal As Double
        Dim l_drGroupedProjectedBalanceNewRow As DataRow
        Try
            l_dtGroupedProjectedBalances = New DataTable
            l_dtGroupedProjectedBalances.Columns.Add(New DataColumn("chrLegacyAcctType"))
            l_dtGroupedProjectedBalances.Columns.Add(New DataColumn("bitBasicAcct"))
            l_dtGroupedProjectedBalances.Columns.Add(New DataColumn("PlanType"))
            l_dtGroupedProjectedBalances.Columns.Add(New DataColumn("mnyProjectedBalance", System.Type.GetType("System.Decimal")))

            If Not para_dtNonGroupedAcctWiseProjectedBalance Is Nothing Then
                If para_dtNonGroupedAcctWiseProjectedBalance.Rows.Count > 0 Then
                    l_dtNew_dtNonGroupedAcctWiseProjectedBalance = para_dtNonGroupedAcctWiseProjectedBalance.Copy()
                    'create personal acct row
                    l_filterExpression = "bitBasicAcct=True AND bitPA=1 AND PlanType='RETIREMENT'"
                    l_drNonGroupedAcctFoundRow = l_dtNew_dtNonGroupedAcctWiseProjectedBalance.Select(l_filterExpression)
                    If l_drNonGroupedAcctFoundRow.Length > 0 Then
                        mnyProjectedTotal = l_dtNew_dtNonGroupedAcctWiseProjectedBalance.Compute("SUM(mnyProjectedPersBalance)", l_filterExpression)
                        l_drGroupedProjectedBalanceNewRow = l_dtGroupedProjectedBalances.NewRow
                        l_drGroupedProjectedBalanceNewRow("chrLegacyAcctType") = "PA"
                        l_drGroupedProjectedBalanceNewRow("bitBasicAcct") = l_drNonGroupedAcctFoundRow(0)("bitBasicAcct")
                        l_drGroupedProjectedBalanceNewRow("PlanType") = l_drNonGroupedAcctFoundRow(0)("PlanType")
                        l_drGroupedProjectedBalanceNewRow("mnyProjectedBalance") = mnyProjectedTotal
                        l_dtGroupedProjectedBalances.Rows.Add(l_drGroupedProjectedBalanceNewRow)
                    End If
                    'create Ymca acct row
                    l_filterExpression = "bitBasicAcct=True AND bitYA=1 AND PlanType='RETIREMENT'"
                    l_drNonGroupedAcctFoundRow = l_dtNew_dtNonGroupedAcctWiseProjectedBalance.Select(l_filterExpression)
                    If l_drNonGroupedAcctFoundRow.Length > 0 Then
                        mnyProjectedTotal = l_dtNew_dtNonGroupedAcctWiseProjectedBalance.Compute("SUM(mnyProjectedYmcaBalance)", l_filterExpression)
                        l_drGroupedProjectedBalanceNewRow = l_dtGroupedProjectedBalances.NewRow
                        l_drGroupedProjectedBalanceNewRow("chrLegacyAcctType") = "YA"
                        l_drGroupedProjectedBalanceNewRow("bitBasicAcct") = l_drNonGroupedAcctFoundRow(0)("bitBasicAcct")
                        l_drGroupedProjectedBalanceNewRow("PlanType") = l_drNonGroupedAcctFoundRow(0)("PlanType")
                        l_drGroupedProjectedBalanceNewRow("mnyProjectedBalance") = mnyProjectedTotal
                        l_dtGroupedProjectedBalances.Rows.Add(l_drGroupedProjectedBalanceNewRow)

                    End If
                    'Create Employer Paid Acct row
                    l_filterExpression = "bitBasicAcct=True AND bitEP=1 AND PlanType='RETIREMENT'"
                    l_drNonGroupedAcctFoundRow = l_dtNew_dtNonGroupedAcctWiseProjectedBalance.Select(l_filterExpression)
                    If l_drNonGroupedAcctFoundRow.Length > 0 Then
                        mnyProjectedTotal = l_dtNew_dtNonGroupedAcctWiseProjectedBalance.Compute("SUM(mnyProjectedYmcaBalance)", l_filterExpression)
                        l_drGroupedProjectedBalanceNewRow = l_dtGroupedProjectedBalances.NewRow
                        l_drGroupedProjectedBalanceNewRow("chrLegacyAcctType") = "EP"
                        l_drGroupedProjectedBalanceNewRow("bitBasicAcct") = l_drNonGroupedAcctFoundRow(0)("bitBasicAcct")
                        l_drGroupedProjectedBalanceNewRow("PlanType") = l_drNonGroupedAcctFoundRow(0)("PlanType")
                        l_drGroupedProjectedBalanceNewRow("mnyProjectedBalance") = mnyProjectedTotal
                        l_dtGroupedProjectedBalances.Rows.Add(l_drGroupedProjectedBalanceNewRow)

                    End If
                    'create row for non basic or voluntary acct
                    l_filterExpression = "bitBasicAcct=False "
                    l_drNonGroupedAcctFoundRow = l_dtNew_dtNonGroupedAcctWiseProjectedBalance.Select(l_filterExpression)
                    If l_drNonGroupedAcctFoundRow.Length > 0 Then
                        Dim i As Int32
                        'Dim l_drNonGroupedVoluntaryAcctFoundRow As DataRow()
                        For i = 0 To l_drNonGroupedAcctFoundRow.Length - 1
                            'l_drNonGroupedVoluntaryAcctFoundRow = l_dtNew_dtNonGroupedAcctWiseProjectedBalance.Select(l_filterExpression & " AND chrAcctType='" & l_drNonGroupedAcctFoundRow(i)("chrAcctType") & "'")
                            'If l_drNonGroupedVoluntaryAcctFoundRow.Length > 0 Then
                            l_drGroupedProjectedBalanceNewRow = l_dtGroupedProjectedBalances.NewRow
                            l_drGroupedProjectedBalanceNewRow("chrLegacyAcctType") = l_drNonGroupedAcctFoundRow(i)("chrAcctType")
                            l_drGroupedProjectedBalanceNewRow("bitBasicAcct") = l_drNonGroupedAcctFoundRow(i)("bitBasicAcct")
                            l_drGroupedProjectedBalanceNewRow("PlanType") = l_drNonGroupedAcctFoundRow(i)("PlanType")
                            l_drGroupedProjectedBalanceNewRow("mnyProjectedBalance") = l_drNonGroupedAcctFoundRow(i)("mnyProjectedBalance")
                            l_dtGroupedProjectedBalances.Rows.Add(l_drGroupedProjectedBalanceNewRow)
                            'End If
                        Next
                    End If

                End If

            End If
            Return l_dtGroupedProjectedBalances
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'This method update projected account balance in base grouped Ellective account table 
    Private Function UpdateProjectedBalanceInBaseGroupedAcctTable(ByRef para_dtGroupedBaseElectiveAcct As DataTable, ByVal para_dtGroupedProjectedAcctBalance As DataTable)
        Dim l_filterExpression As String
        'Dim l_drGroupedBaseElectiveAcctFoundRow As DataRow()
        Dim l_drGroupedProjAcctBalFoundRow As DataRow()
        Try
            If Not para_dtGroupedBaseElectiveAcct Is Nothing AndAlso Not para_dtGroupedProjectedAcctBalance Is Nothing Then
                'Commented by Ashish for Phase V part III changes
                'For Each drProjAcctBalRow As DataRow In para_dtGroupedProjectedAcctBalance.Rows
                '    l_filterExpression = "bitBasicAcct=" & drProjAcctBalRow("bitBasicAcct") & " AND PlanType='" & drProjAcctBalRow("PlanType") & "' AND chrLegacyAcctType='" & drProjAcctBalRow("chrLegacyAcctType") & "'"
                '    l_drGroupedBaseElectiveAcctFoundRow = para_dtGroupedBaseElectiveAcct.Select(l_filterExpression)
                '    If l_drGroupedBaseElectiveAcctFoundRow.Length > 0 Then
                '        l_drGroupedBaseElectiveAcctFoundRow(0)("mnyProjectedBalance") = drProjAcctBalRow("mnyProjectedBalance")

                '    End If
                'Next

                For Each drGroupedBaseElectiveAcctRow As DataRow In para_dtGroupedBaseElectiveAcct.Rows
                    l_filterExpression = "PlanType='" & drGroupedBaseElectiveAcctRow("PlanType") & "' AND chrLegacyAcctType='" & drGroupedBaseElectiveAcctRow("chrLegacyAcctType") & "'"
                    l_drGroupedProjAcctBalFoundRow = para_dtGroupedProjectedAcctBalance.Select(l_filterExpression)
                    If l_drGroupedProjAcctBalFoundRow.Length > 0 Then
                        drGroupedBaseElectiveAcctRow("mnyProjectedBalance") = l_drGroupedProjAcctBalFoundRow(0)("mnyProjectedBalance")
                    Else
                        drGroupedBaseElectiveAcctRow("mnyProjectedBalance") = 0

                    End If
                Next
                para_dtGroupedBaseElectiveAcct.AcceptChanges()
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'This method display selected account total in Account tab 
    Private Function ShowProjectedAcctBalancesTotal()
        Dim l_SelectedProjTotal As Decimal = 0
        Dim l_SelectedAcctTotal As Decimal = 0
        Dim l_dtGroupedElectiveAcct As DataTable
        Dim l_strSelectedProjTotal As String = String.Empty
        Dim l_drGroupAcctFoundRow As DataRow()
        Dim chkSelected As CheckBox
        Try
            'get RetireeGroupedElectiveAccounts from sesion variable
            If Not Session("dsElectiveAccountsDet") Is Nothing And TypeOf Session("dsElectiveAccountsDet") Is DataSet Then
                If Not Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts") Is Nothing Then
                    l_dtGroupedElectiveAcct = DirectCast(Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts"), DataTable)
                End If
            End If

            If Not l_dtGroupedElectiveAcct Is Nothing Then

                If Me.PlanType = "R" OrElse Me.PlanType = "B" Then

                    l_SelectedProjTotal = 0
                    l_SelectedAcctTotal = 0
                    l_drGroupAcctFoundRow = l_dtGroupedElectiveAcct.Select("PlanType='RETIREMENT' AND Selected=True")
                    If l_drGroupAcctFoundRow.Length > 0 Then
                        l_SelectedProjTotal = Convert.ToDecimal(l_dtGroupedElectiveAcct.Compute("SUM(mnyProjectedBalance)", "PlanType='RETIREMENT' AND Selected=True"))
                    End If

                    'l_SelectedAcctTotal = Convert.ToDecimal(l_dtGroupedElectiveAcct.Compute("SUM(AcctTotal)", "PlanType='RETIREMENT' AND Selected=True"))
                    Dim lblProjectedTotalBal As Label
                    'Added by Dinesh Kanojia
                    'Start: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                    If chkRetirementAccount.Checked = False And String.IsNullOrEmpty(txtRetirementAccount.Text.Trim) Then
                        'End: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                        If Not DatagridElectiveRetirementAccounts Is Nothing Then
                            If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
                                lblProjectedTotalBal = CType(CType(DatagridElectiveRetirementAccounts.Controls(0).Controls(DatagridElectiveRetirementAccounts.Items.Count + 1).Controls(RET_PROJ_BALANCE), TableCell).FindControl("LabelProjectedTotalBalRet"), Label)
                                If Not lblProjectedTotalBal Is Nothing Then
                                    lblProjectedTotalBal.Text = String.Format("{0:0.00}", l_SelectedProjTotal)
                                End If
                            End If
                        End If
                    End If
                End If
                'CType(CType(DatagridElectiveRetirementAccounts.Controls(0).Controls(DatagridElectiveRetirementAccounts.Items.Count + 1).Controls(RET_PROJ_BALANCE), TableCell).FindControl("LabelProjectedTotaBalRet"), Label).Text()
                If Me.PlanType = "S" Or Me.PlanType = "B" Then

                    l_SelectedProjTotal = 0
                    l_SelectedAcctTotal = 0
                    l_drGroupAcctFoundRow = l_dtGroupedElectiveAcct.Select("PlanType='SAVINGS' AND Selected=True")
                    If l_drGroupAcctFoundRow.Length > 0 Then
                        l_SelectedProjTotal = Convert.ToDecimal(l_dtGroupedElectiveAcct.Compute("SUM(mnyProjectedBalance)", "PlanType='SAVINGS' AND Selected=True"))
                    End If

                    'l_SelectedAcctTotal = Convert.ToDecimal(l_dtGroupedElectiveAcct.Compute("SUM(AcctTotal)", "PlanType='RETIREMENT' AND Selected=True"))
                    'Added by DInesh Kanojia
                    Dim lblProjectedTotalBal As Label
                    'Start: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                    If chkSavingPartialAmount.Checked = False And String.IsNullOrEmpty(txtSavingPartialAmount.Text.Trim) Then
                        'End: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                        If Not DatagridElectiveSavingsAccounts Is Nothing Then
                            If DatagridElectiveSavingsAccounts.Items.Count > 0 Then
                                lblProjectedTotalBal = CType(CType(DatagridElectiveSavingsAccounts.Controls(0).Controls(DatagridElectiveSavingsAccounts.Items.Count + 1).Controls(SAV_PROJ_BALANCE), TableCell).FindControl("LabelProjectedTotalBalSav"), Label)
                                If Not lblProjectedTotalBal Is Nothing Then
                                    lblProjectedTotalBal.Text = String.Format("{0:0.00}", l_SelectedProjTotal)
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'This method enable or disable YmcaLegacy or YmcaAccount based on threshold check 
    Private Function ValidateProjectedBalancesAsPerRefund() As Boolean
        Dim l_LegacyAcctType As String = String.Empty
        Dim l_ProjectedBalances As Decimal = 0
        Dim chkSelected As CheckBox
        Dim l_PlanType As String = String.Empty
        Dim isYmcaLegacyAcctTotalExceed As Boolean = False
        Dim isYmcaAcctTotalExceed As Boolean = False
        Dim l_boolShowMessage As Boolean = False
        Dim l_strMessage As String = String.Empty
        Dim l_dtGroupedElectiveAcct As DataTable
        Dim l_drGroupedAcctFoundRow As DataRow()
        Dim l_boolBitBasicAcct As Boolean

        Try
            'get RetireeGroupedElectiveAccounts from sesion variable
            If Not Session("dsElectiveAccountsDet") Is Nothing Then
                If Not Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts") Is Nothing Then
                    l_dtGroupedElectiveAcct = CType(Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts"), DataTable)
                End If
            End If

            If Not l_dtGroupedElectiveAcct Is Nothing Then

                'Retirement plan
                If Me.PlanType = "R" OrElse Me.PlanType = "B" Then
                    If Not Session("isYmcaLegacyAcctTotalExceed") Is Nothing Then
                        isYmcaLegacyAcctTotalExceed = Session("isYmcaLegacyAcctTotalExceed")
                    End If
                    If Not Session("isYmcaAcctTotalExceed") Is Nothing Then
                        isYmcaAcctTotalExceed = Session("isYmcaAcctTotalExceed")
                    End If


                    If Not DatagridElectiveRetirementAccounts Is Nothing Then
                        For Each dgRetItemRow As DataGridItem In DatagridElectiveRetirementAccounts.Items
                            'Vaildation for Ymca Legacy Acct
                            l_LegacyAcctType = dgRetItemRow.Cells(RET_LEGACY_ACCT_TYPE).Text.Trim()
                            l_boolBitBasicAcct = Convert.ToBoolean(dgRetItemRow.Cells(RET_BASIC_ACCT).Text.Trim())
                            l_PlanType = dgRetItemRow.Cells(RET_PLANE_TYPE).Text.Trim().ToUpper()
                            chkSelected = CType(dgRetItemRow.Cells(RET_SEL_ACCT_CHK).FindControl("CheckboxRet"), CheckBox)

                            'l_ProjectedBalances = IIf(dgRetItemRow.Cells(RET_PROJ_BALANCE).Text.Trim() = String.Empty, 0, Convert.ToDecimal(dgRetItemRow.Cells(RET_PROJ_BALANCE).Text.Trim()))
                            If l_LegacyAcctType <> String.Empty And Not chkSelected Is Nothing And l_PlanType <> String.Empty Then
                                l_drGroupedAcctFoundRow = l_dtGroupedElectiveAcct.Select("bitBasicAcct=" & l_boolBitBasicAcct & " AND PlanType='" & l_PlanType & "' AND chrLegacyAcctType='" & l_LegacyAcctType & "'")
                                If l_drGroupedAcctFoundRow.Length > 0 Then
                                    Select Case l_LegacyAcctType
                                        Case "YA"
                                            If isYmcaLegacyAcctTotalExceed Then
                                                If l_drGroupedAcctFoundRow(0)("Selected") = False Then
                                                    l_boolShowMessage = True
                                                End If
                                                chkSelected.Checked = True
                                                chkSelected.Enabled = False

                                            Else
                                                chkSelected.Checked = l_drGroupedAcctFoundRow(0)("Selected")
                                                chkSelected.Enabled = True
                                            End If

                                        Case "EP"
                                            If isYmcaAcctTotalExceed Then
                                                If l_drGroupedAcctFoundRow(0)("Selected") = False Then
                                                    l_boolShowMessage = True
                                                End If
                                                chkSelected.Checked = True
                                                chkSelected.Enabled = False

                                            Else
                                                chkSelected.Checked = l_drGroupedAcctFoundRow(0)("Selected")
                                                chkSelected.Enabled = True
                                            End If
                                        Case "PA"
                                            chkSelected.Checked = l_drGroupedAcctFoundRow(0)("Selected")
                                            If l_drGroupedAcctFoundRow(0)("Selected") = False Then

                                                RetirementAcctGridRowSelectionChangeValidation(dgRetItemRow)

                                            End If

                                        Case Else
                                            If l_boolBitBasicAcct = False Then
                                                chkSelected.Checked = l_drGroupedAcctFoundRow(0)("Selected")
                                                If chkSelected.Checked = False Then
                                                    ResetRetirementAcctDataGridRow(dgRetItemRow)
                                                End If
                                            End If


                                    End Select
                                End If

                            End If
                        Next
                        ' if l_boolShowMessage true then reset acct selection, 
                        If l_boolShowMessage Then
                            For Each dgRetItemRow As DataGridItem In DatagridElectiveRetirementAccounts.Items

                                l_LegacyAcctType = dgRetItemRow.Cells(RET_LEGACY_ACCT_TYPE).Text.Trim()
                                chkSelected = CType(dgRetItemRow.Cells(RET_SEL_ACCT_CHK).FindControl("CheckboxRet"), CheckBox)
                                If l_LegacyAcctType <> String.Empty And Not chkSelected Is Nothing Then
                                    Select Case l_LegacyAcctType
                                        Case "PA"
                                            chkSelected.Checked = True
                                            chkSelected.Enabled = True
                                        Case "YA"
                                            chkSelected.Checked = True

                                        Case "EP"
                                            chkSelected.Checked = True

                                        Case Else
                                            If dgRetItemRow.Cells(RET_BASIC_ACCT).Text.Trim() = False Then
                                                If Convert.ToDecimal(IIf(dgRetItemRow.Cells(RET_ACCT_TOTAL).Text.Trim() = String.Empty, 0, dgRetItemRow.Cells(RET_ACCT_TOTAL).Text.Trim())) > 0 Then
                                                    chkSelected.Checked = True
                                                    chkSelected.Enabled = True
                                                    'dgRetireRow.Enabled = True
                                                Else
                                                    chkSelected.Checked = False
                                                    chkSelected.Enabled = True
                                                    'dgRetireRow.Enabled = True
                                                End If

                                            End If
                                    End Select
                                End If
                            Next
                            'l_strMessage = "Current account selection is no longer valid.  Please select accounts to exclude from this estimate on the Accounts tab."
                            l_strMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_CURRENT_ACCOUNT_IS_INVALID")
                            LabelAcctBalExceedTresholdLimit.Text = l_strMessage
                            LabelAcctBalExceedTresholdLimit.Visible = True

                        End If
                        'end if l_boolShowMessage true
                    End If
                    'end if DatagridElectiveRetirementAccounts not nothing
                End If
                'end if Retirement plan end

                'Saving plan
                'If Me.PlanType = "S" OrElse Me.PlanType = "B" Then
                '    If Not DatagridElectiveSavingsAccounts Is Nothing Then
                '        For Each dgSavItemRow As DataGridItem In DatagridElectiveSavingsAccounts.Items

                '            l_LegacyAcctType = dgSavItemRow.Cells(SAV_LEGACY_ACCT_TYPE).Text.Trim()
                '            l_boolBitBasicAcct = Convert.ToBoolean(dgSavItemRow.Cells(SAV_BASIC_ACCT).Text.Trim())
                '            l_PlanType = dgSavItemRow.Cells(SAV_PLANE_TYPE).Text.Trim().ToUpper()
                '            chkSelected = CType(dgSavItemRow.Cells(SAV_SEL_ACCT_CHK).FindControl("CheckboxSav"), CheckBox)
                '            If l_LegacyAcctType <> String.Empty And Not chkSelected Is Nothing And l_PlanType <> String.Empty Then
                '                l_drGroupedAcctFoundRow = l_dtGroupedElectiveAcct.Select("bitBasicAcct=" & l_boolBitBasicAcct & " AND PlanType='" & l_PlanType & "' AND chrLegacyAcctType='" & l_LegacyAcctType & "'")
                '                If l_drGroupedAcctFoundRow.Length > 0 Then
                '                    chkSelected.Checked = l_drGroupedAcctFoundRow(0)("Selected")
                '                End If
                '            End If
                '        Next
                '    End If
                'End If
            End If
            'end if  l_dtGroupedElectiveAcct is not nothing
            Return l_boolShowMessage
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Private Function ValidateEmployerPaidProjBalancesAsPerRefund(ByVal para_ProjectedBalance As Decimal)
    '    Dim l_RetireeCurrentAge As Int32
    '    Dim l_DateTimeRetirementDate As DateTime
    '    Dim l_strTerminationDate As String = String.Empty
    '    Dim l_strInputTerminationDate As String = String.Empty
    '    Dim l_strTerminationDateInDB As String = String.Empty
    '    Dim dsEmploymentDetails As New DataSet
    '    Dim dtEmpDetails As DataTable
    '    Dim l_dtEmpSalarInfo As DataTable
    '    Try
    '        If TextBoxRetireeBirthday.Text.Trim() <> String.Empty And TextBoxRetirementDate.Text.Trim() <> String.Empty Then

    '            l_RetireeCurrentAge = YMCARET.YmcaBusinessObject.DateAndTime.DateDiffNew(DateIntervalNew.Year, Convert.ToDateTime(TextBoxRetireeBirthday.Text.Trim()), DateTime.Now)
    '            l_DateTimeRetirementDate = Convert.ToDateTime(TextBoxRetirementDate.Text.Trim())


    '            'get dbTerminationdate
    '            Dim i As Integer
    '            Dim terminationDateInDB As String = String.Empty

    '            dsEmploymentDetails = RetirementBOClass.getEmploymentDetails(Me.FundEventId, _
    '            "NORMAL", TextBoxRetirementDate.Text)
    '            If Not dsEmploymentDetails Is Nothing Then
    '                dtEmpDetails = dsEmploymentDetails.Tables(0)
    '            End If
    '            If Not Session("employmentSalaryInformation") Is Nothing Then
    '                l_dtEmpSalarInfo = CType(Session("employmentSalaryInformation"), DataTable)
    '            End If


    '            l_strTerminationDate = GetTerminationDateForProjBalValidation(TextBoxRetirementDate.Text.Trim(), l_dtEmpSalarInfo, dtEmpDetails)
    '        End If
    '    Catch ex As Exception
    '            Throw ex

    '        End Try
    'End Function

    'This method return latest termination date from all employement, It can be dbtermination date or user specifed termination date.
    'If termination date does not found then return retirement date.
    Private Function GetTerminationDateForProjBalValidation(ByVal para_RetirementDate As String, ByVal para_dtEmploymentSalaryInfo As DataTable, ByVal para_dtEmploymentDetails As DataTable) As String
        Dim l_dtEmpTermination As DataTable
        Dim l_drEmpSalaryFoundRow As DataRow()
        Dim l_drEmpTerminationNewRow As DataRow
        Dim l_strTerminationDate As String = String.Empty
        Try
            l_dtEmpTermination = New DataTable
            l_dtEmpTermination.Columns.Add(New DataColumn("guiEmpEventID", System.Type.GetType("System.String")))
            l_dtEmpTermination.Columns.Add(New DataColumn("TerminationDate", System.Type.GetType("System.DateTime")))
            If Not para_dtEmploymentDetails Is Nothing AndAlso Not para_dtEmploymentSalaryInfo Is Nothing Then
                If para_dtEmploymentDetails.Rows.Count > 0 Then
                    For Each drEmpDetailRow As DataRow In para_dtEmploymentDetails.Rows
                        l_drEmpTerminationNewRow = l_dtEmpTermination.NewRow()
                        l_drEmpTerminationNewRow("guiEmpEventID") = drEmpDetailRow("guiEmpEventID").ToString().ToUpper()

                        If drEmpDetailRow.Item("dtmTerminationDate") Is DBNull.Value Or drEmpDetailRow.Item("dtmTerminationDate").ToString() = String.Empty Then
                            If para_dtEmploymentSalaryInfo.Rows.Count > 0 Then
                                l_drEmpSalaryFoundRow = para_dtEmploymentSalaryInfo.Select("EmpEventID='" & drEmpDetailRow("guiEmpEventID").ToString().Trim().ToUpper() & "'")
                                If l_drEmpSalaryFoundRow.Length > 0 Then

                                    If l_drEmpSalaryFoundRow(0).Item("EndWorkDate") Is DBNull.Value Or l_drEmpSalaryFoundRow(0).Item("EndWorkDate").ToString() = String.Empty Then
                                        l_drEmpTerminationNewRow("TerminationDate") = para_RetirementDate
                                    Else
                                        l_drEmpTerminationNewRow("TerminationDate") = l_drEmpSalaryFoundRow(0)("EndWorkDate")
                                    End If
                                Else
                                    l_drEmpTerminationNewRow("TerminationDate") = para_RetirementDate
                                End If
                            Else
                                l_drEmpTerminationNewRow("TerminationDate") = para_RetirementDate
                            End If
                        Else
                            l_drEmpTerminationNewRow("TerminationDate") = Convert.ToDateTime(drEmpDetailRow("dtmTerminationDate")).ToString("MM/dd/yyyy")
                        End If
                        l_dtEmpTermination.Rows.Add(l_drEmpTerminationNewRow)
                    Next
                    l_dtEmpTermination.AcceptChanges()
                End If
            End If
            Dim l_drTerminationFoundRow As DataRow() = l_dtEmpTermination.Select("TerminationDate=MAX(TerminationDate)")
            If l_drTerminationFoundRow.Length > 0 Then
                l_strTerminationDate = l_drTerminationFoundRow(0)("TerminationDate")
            End If
            Return l_strTerminationDate
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Private Function GetTotalBasedOnAcctType(ByVal l_dtGroupedElectiveAcct As DataTable, ByVal REFUND_MAX_PIA As Decimal, ByVal BA_MAX_LIMIT_55_ABOVE As Decimal, ByVal strPlanType As String) As Decimal ' CS:YRS-AT-2479:12/11/2015: Commented the line to add new input parameters in this method.
    Private Function GetTotalBasedOnAcctType(ByVal l_dtGroupedElectiveAcct As DataTable, ByVal REFUND_MAX_PIA As Decimal, ByVal BA_MAX_LIMIT_55_ABOVE As Decimal, ByVal strPlanType As String, IsPersonTerminated As Boolean, decPIA_At_Termination As Decimal, REFUND_MAX_PIA_AT_TERM As Decimal, decYMCAAcctAndYMCALegacyAcctBalances As Decimal, bIsBALegacyCombinedAccountRule As Boolean, BA_LEGACY_MAX_COMBINED_LIMIT As Decimal) As Decimal ' Added by Chandra sekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
        Dim l_drGroupedAcctFoundRow As DataRow()
        Dim decTotalPlanAmount As Decimal = 0
        Dim mnyProjectedBalance As Decimal = 0
        Dim chkSelected As CheckBox 'Added by Chandrasekar.c on 2016.03.03 YRS-AT-2659 - YRS bug -PRA calculator should allow exclusion of account then partial withdrawal (TrackIT 24145)
        Dim l_LegacyAcctType, l_PlanType As String
        Dim l_boolBitBasicAcct As Boolean
        If strPlanType = "R" Or strPlanType = "B" Then
            For Each dgItemRow As DataGridItem In DatagridElectiveRetirementAccounts.Items
                l_LegacyAcctType = dgItemRow.Cells(RET_LEGACY_ACCT_TYPE).Text.Trim()
                l_boolBitBasicAcct = Convert.ToBoolean(dgItemRow.Cells(RET_BASIC_ACCT).Text.Trim())
                l_PlanType = dgItemRow.Cells(RET_PLANE_TYPE).Text.Trim().ToUpper()
                chkSelected = CType(dgItemRow.Cells(RET_SEL_ACCT_CHK).FindControl("CheckboxRet"), CheckBox) 'Added by Chandrasekar.c on 2016.03.03 YRS-AT-2659 - YRS bug -PRA calculator should allow exclusion of account then partial withdrawal (TrackIT 24145)
                l_drGroupedAcctFoundRow = l_dtGroupedElectiveAcct.Select("bitBasicAcct=" & l_boolBitBasicAcct & " AND PlanType='" & l_PlanType & "' AND chrLegacyAcctType='" & l_LegacyAcctType & "'")
                'If l_drGroupedAcctFoundRow.Length > 0 Then 'Comment by Chandrasekar on 03.03.2016 YRS-AT-2659 - YRS bug -PRA calculator should allow exclusion of account then partial withdrawal (TrackIT 24145)
                If chkSelected.Checked And l_drGroupedAcctFoundRow.Length > 0 Then 'Added by Chandrasekar.c on 2016.03.03 YRS-AT-2659 - YRS bug -PRA calculator should allow exclusion of account then partial withdrawal (TrackIT 24145)

                    If Me.FundEventStatus <> "QD" Then  'Start:Added by Chandra sekar.c on 2015.11.02 QD is not restricted on partial withdrawal in case of Account balance greater/Less $25000 or greater $5000 
                        ' If new rule Sum of YMCA Account and YMCA Legacy Account will assigned otherwise indivdual Account balance will assigned
                        mnyProjectedBalance = IIf(bIsBALegacyCombinedAccountRule = True, decYMCAAcctAndYMCALegacyAcctBalances, l_drGroupedAcctFoundRow(0)("mnyProjectedBalance"))
                        Select Case l_drGroupedAcctFoundRow(0)("chrAcctType").ToString.ToUpper().Trim
                            Case "YMCA(LEGACY) ACCOUNT"

                                'Start:Added by Chandrasekar.c on 2015.11.19 YRS-AT-2479 : Partial Withdrawal Estimate is allowed or not for terminated Participant with YMCA(LEGACY) ACCOUNT Balance
                                'if partial withdrawal was opted from retirement / savings plan.
                                If chkRetirementAccount.Checked Or chkSavingPartialAmount.Checked Then
                                    'if the person's projected balance is less than max pia allowed (configured as $25K) and he/she is not terminated then consider the legacy account for partial withdrawal what if analysis.
                                    ' If Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")) < REFUND_MAX_PIA Then   CS:YRS-AT-2479:12/11/2015: Commented the line to add new Condition for checking whether person is not terminated.
                                    If Convert.ToDecimal(mnyProjectedBalance) < REFUND_MAX_PIA And IsPersonTerminated = False And bIsBALegacyCombinedAccountRule = False Then
                                        decTotalPlanAmount += l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")
                                        'if the person's legacy account balance as of terminated date is less than max pia allowed (configured as $25K) and he/she is terminated then consider the legacy account for partial withdrawal what if analysis.
                                        'START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                                        'If (YMCA Acount + YMCA (legacy)Account)  < 50000 and Combined rule
                                    ElseIf Convert.ToDecimal(mnyProjectedBalance) <= BA_LEGACY_MAX_COMBINED_LIMIT And bIsBALegacyCombinedAccountRule = True Then
                                        decTotalPlanAmount += l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")
                                        'If (YMCA Acount + YMCA (legacy)Account)  > 50000 and YMCA (legacy)Account at termination < 25000 and Combined rule
                                    ElseIf Convert.ToDecimal(mnyProjectedBalance) > BA_LEGACY_MAX_COMBINED_LIMIT And decPIA_At_Termination <= REFUND_MAX_PIA_AT_TERM And bIsBALegacyCombinedAccountRule = True Then
                                        decTotalPlanAmount += l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")
                                        'If YMCA (legacy)Account at termination < 25000 And Person terminated and NO Combined rule
                                    ElseIf decPIA_At_Termination < REFUND_MAX_PIA_AT_TERM And IsPersonTerminated = True And bIsBALegacyCombinedAccountRule = False Then
                                        'END : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                                        decTotalPlanAmount += l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")
                                    End If
                                Else
                                    decTotalPlanAmount += mnyProjectedBalance
                                End If

                                'End:Added by Chandrasekar.c on 2015.11.19 YRS-AT-2479 : Partial Withdrawal Estimate is allowed or not for terminated Participant with YMCA(LEGACY) ACCOUNT  Balance
                            Case "YMCA ACCOUNT"
                                If chkRetirementAccount.Checked Or chkSavingPartialAmount.Checked Then
                                    'START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                                    'If (YMCA Acount )  < 25000 and Existing rule
                                    If Convert.ToDecimal(mnyProjectedBalance) <= BA_LEGACY_MAX_COMBINED_LIMIT And bIsBALegacyCombinedAccountRule = True Then
                                        decTotalPlanAmount += l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")
                                        'If (YMCA Acount + YMCA (legacy)Account)  <= 50000 and Combined rule
                                    ElseIf Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")) < BA_MAX_LIMIT_55_ABOVE Then
                                        decTotalPlanAmount += l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")
                                        'END : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                                    End If
                                Else
                                    decTotalPlanAmount += l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")
                                End If
                            Case Else
                                decTotalPlanAmount += l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")
                        End Select

                    Else
                        decTotalPlanAmount += l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")
                        'End:Added by chandrasekar.c on 2015.11.02 QD is not restricted on partial withdrawal in case of Account balance greater/Less $25000 or greater $5000 
                    End If

                End If
            Next
        End If
        'If strPlanType = "S" Or strPlanType = "B" Then
        '    decTotalPlanAmount += l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")
        'End If
        Return decTotalPlanAmount
    End Function

    'This function update projected balances and selection acct selection state in base grouped datatable
    Private Function SetElectiveAccountSelectionState() As Boolean
        Dim l_dtGroupedElectiveAcct As DataTable
        Dim l_drGroupedAcctFoundRow As DataRow()
        Dim l_LegacyAcctType As String = String.Empty
        Dim l_boolBitBasicAcct As Boolean
        Dim chkSelected As CheckBox
        Dim l_PlanType As String = String.Empty
        Dim l_LabelProjAcctBal As Label
        Dim decTotalProjRet, decPartialAmount, decAccountTotal As Decimal
        Dim decCheckBalAmount As Decimal = 0
        Dim lblProjectedTotalBalRet As Label
        Dim tblFooterTotal As Table
        Dim grdFooterTotal As DataGridItem
        Dim filterExpression As String = String.Empty
        Dim l_DataTable_Min_Partial_Withdrawal_Limit, l_Datatable_REFUND_MAX_PIA, l_DataTable_BA_MAX_LIMIT_55_ABOVE, l_DataTable_PIAMinToRetire, l_Datatable_SS_MIN_AGE As DataTable
        Dim dvRet As DataView
        Dim bIsExclude As Boolean = False
        Dim MinimumPIAToRetire, MIN_PARTIAL_WITHDRAWAL_LIMIT, REFUND_MAX_PIA, BA_MAX_LIMIT_55_ABOVE, SS_MIN_AGE, REFUND_MAX_PIA_AT_TERM As Decimal
        'Start:Added by Chandrasekar.c on 2015.11.19 YRS-AT-2479 : Added one variable one is for  YMCA Legacy Account as of termination 
        Dim decPIA_At_Termination As Decimal = 0
        'Start:Added by Chandrasekar.c on 2015.11.19 YRS-AT-2479 : Added one variable one is for  YMCA Legacy Account as of termination 
        'START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
        Dim BA_LEGACY_MAX_COMBINED_LIMIT As Decimal
        Dim dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT As DataTable
        Dim decYMCAAcctAndYMCALegacyAcctBalances As Decimal
        Dim blIsBACombinedVaildationMessage As Boolean = False
        Dim bIsBALegacyCombinedAccountRule As Boolean = False
        Dim IsYmcaLegacyValidationMgs As Boolean = False
        Dim IsYmcaAccountValidation As Boolean = False
        Dim decProjectedBalance As Decimal = 0
        'END : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
        Try

            hdnDecSavingValue.Value = "0"
            hdnDecPartialValue.Value = "0"
            If Not Session("dsElectiveAccountsDet") Is Nothing Then
                If Not Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts") Is Nothing Then
                    l_dtGroupedElectiveAcct = CType(Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts"), DataTable)
                End If
            End If

            If Not l_dtGroupedElectiveAcct Is Nothing Then
                'Retirement Plan
                LabelPartialWithdrawal.Visible = True
                LabelPartialWithdrawal.Text = ""
                'START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                lblRetirementPartialAmountEligible.Text = ""
                lblSavingPartialAmountEligible.Text = ""
                'END : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                'Added By Dinesh Kanojia
                'Start: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                l_Datatable_SS_MIN_AGE = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("SS_MIN_AGE")
                l_DataTable_PIAMinToRetire = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("PIA_MINIMUM_TO_RETIRE")
                l_DataTable_Min_Partial_Withdrawal_Limit = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("MIN_PARTIAL_WITHDRAWAL_LIMIT")
                ''START : Commented by  Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                'l_Datatable_REFUND_MAX_PIA = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("REFUND_MAX_PIA")
                'l_DataTable_BA_MAX_LIMIT_55_ABOVE = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("BA_MAX_LIMIT_55_ABOVE")
                ''END : Commented by  Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                If TextBoxRetirementAge.Text.Trim <> "" Then 'MMR | 2017.03.24 | YRS-AT-2625 | Added condition to check if retirement date is not empty to avoid error
                    'START : ML| 2019.04.09 |YRS-AT-4127 - YRS bug-wrong warning message in Annuity Estimate Calculator for YMCA Account balance max
                    ' Passing Me.IsPersonTerminated as True because Me.IsPersonTerminated value is used to fetch withdrawal configuration only
                    ' So that terminated participants withrawal rules will get applied to Active participants also.
                    ' It is passed to "YMCARET.YmcaBusinessObject.RefundRequest.GetRefundRequestConfiguration" inside in RetirementBO class.
                    ' dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundRequestConfiguration(Convert.ToInt32(TextBoxRetirementAge.Text.Trim), Me.IsPersonTerminated ') 'START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                    dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundRequestConfiguration(Convert.ToInt32(TextBoxRetirementAge.Text.Trim), True)
                    'END : ML| 2019.04.09 |YRS-AT-4127 - YRS bug-wrong warning message in Annuity Estimate Calculator for YMCA Account balance max
                End If
                If Not l_DataTable_PIAMinToRetire Is Nothing Then
                    If l_DataTable_PIAMinToRetire.Rows(0)("Key").ToString.Trim.ToUpper() = "PIA_MINIMUM_TO_RETIRE" Then
                        MinimumPIAToRetire = CType(l_DataTable_PIAMinToRetire.Rows(0)("Value").ToString, Decimal)
                    End If
                End If

                If Not l_DataTable_Min_Partial_Withdrawal_Limit Is Nothing Then
                    If l_DataTable_Min_Partial_Withdrawal_Limit.Rows(0)("Key").ToString.Trim.ToUpper() = "MIN_PARTIAL_WITHDRAWAL_LIMIT" Then
                        MIN_PARTIAL_WITHDRAWAL_LIMIT = CType(l_DataTable_Min_Partial_Withdrawal_Limit.Rows(0)("Value").ToString, Decimal)
                    End If
                End If
                ''START : Commented by  Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                'If Not l_Datatable_REFUND_MAX_PIA Is Nothing Then
                '    If l_Datatable_REFUND_MAX_PIA.Rows(0)("Key").ToString.Trim.ToUpper() = "REFUND_MAX_PIA" Then
                '        REFUND_MAX_PIA = CType(l_Datatable_REFUND_MAX_PIA.Rows(0)("Value").ToString, Decimal)
                '    End If
                'End If

                'If Not l_DataTable_BA_MAX_LIMIT_55_ABOVE Is Nothing Then
                '    If l_DataTable_BA_MAX_LIMIT_55_ABOVE.Rows(0)("Key").ToString.Trim.ToUpper() = "BA_MAX_LIMIT_55_ABOVE" Then
                '        BA_MAX_LIMIT_55_ABOVE = CType(l_DataTable_BA_MAX_LIMIT_55_ABOVE.Rows(0)("Value").ToString, Decimal)
                '    End If
                'End If

                'If Not l_Datatable_SS_MIN_AGE Is Nothing Then
                '    If l_Datatable_SS_MIN_AGE.Rows(0)("Key").ToString.Trim.ToUpper() = "SS_MIN_AGE" Then
                '        SS_MIN_AGE = CType(l_Datatable_SS_MIN_AGE.Rows(0)("Value").ToString, Decimal)
                '    End If
                'End If

                'If Not l_DataTable_BA_MAX_LEGACY_COMBINED_LIMIT Is Nothing Then
                '    BA_LEGACY_MAX_COMBINED_LIMIT = CType(l_DataTable_BA_MAX_LEGACY_COMBINED_LIMIT.Rows(0)("numMaxCombinedAmtUnderBasicAccount").ToString, Decimal)
                'End If
                'END : Commented by Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                ''START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)

                If HelperFunctions.isNonEmpty(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT) Then
                    SS_MIN_AGE = CType(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows(0)("MinAge").ToString, Decimal)
                    BA_LEGACY_MAX_COMBINED_LIMIT = CType(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows(0)("YmcaCombinedBasicAccountLimit").ToString, Decimal)
                    BA_MAX_LIMIT_55_ABOVE = CType(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows(0)("YmcaAccountLimit").ToString, Decimal)
                    REFUND_MAX_PIA = CType(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows(0)("YmcaLegacyAccountLimit").ToString, Decimal)
                    REFUND_MAX_PIA_AT_TERM = CType(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows(0)("YmcaLegacyAccountAtTermLimit").ToString, Decimal)
                    bIsBALegacyCombinedAccountRule = CType(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows(0)("bitIsBALegacyCombinedRule").ToString, Boolean)
                End If

                ''End : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)



                decTotalProjRet = 0

                'Start:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - Checking person is terminate or not and as well as if Person is terminated and then get the YMCA(LEGACY) ACCOUNT Balance as on Termination
                If Me.IsPersonTerminated = True Then
                    decPIA_At_Termination = Me.PIA_At_Termination
                    'START : ML| 2019.04.09 |YRS-AT-4127 - If participant is Active then participant termination balance will be particpant current balance
                Else
                    decPIA_At_Termination = RefundRequest.GetCurrentPIA(Me.FundEventId)
                    'END : ML| 2019.04.09 |YRS-AT-4127 - If participant is Active then participant termination balance will be particpant current balance
                End If
                'End:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - Checking person is terminate or not and as well as if Person is terminated and then get the YMCA(LEGACY) ACCOUNT Balance as on Termination

                'Commented By chandrarasekar.C : YRS-AT-2479-For validate the YMCA(LEGACY) ACCOUNT Balance of Participant at time of termination
                'decTotalProjRet = GetTotalBasedOnAcctType(l_dtGroupedElectiveAcct, REFUND_MAX_PIA, BA_MAX_LIMIT_55_ABOVE, Me.PlanType)
                'START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                decYMCAAcctAndYMCALegacyAcctBalances = GetYMCAAcctAndYMCALegacyAcctBalances(l_dtGroupedElectiveAcct) ' Getting Sum of Ymca account and YMCA Legacy Account

                decTotalProjRet = GetTotalBasedOnAcctType(l_dtGroupedElectiveAcct, REFUND_MAX_PIA, BA_MAX_LIMIT_55_ABOVE, Me.PlanType, Me.IsPersonTerminated, decPIA_At_Termination, REFUND_MAX_PIA_AT_TERM, decYMCAAcctAndYMCALegacyAcctBalances, bIsBALegacyCombinedAccountRule, BA_LEGACY_MAX_COMBINED_LIMIT)
                'END : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                ' decTotalProjRet = GetTotalBasedOnAcctType(l_dtGroupedElectiveAcct, REFUND_MAX_PIA, BA_MAX_LIMIT_55_ABOVE, Me.PlanType, Me.IsPersonTerminated, decPIA_At_Termination)

                'END: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals            


                'Dinesh Kanojia(DK)     2013-10-24      BT:Attempted to divide by zero error on Retirement Partial Withdrawal Estimate.
                If ((Me.PlanType = "R" Or Me.PlanType = "B") And decTotalProjRet > 0) Then

                    If Not DatagridElectiveRetirementAccounts Is Nothing Then
                        If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
                            Dim decTotalEstimate As Decimal = 0
                            Dim decEligibleAmount As Decimal = 0
                            lblRetirementPartialAmountEligible.Visible = True
                            For Each dgItemRow As DataGridItem In DatagridElectiveRetirementAccounts.Items
                                l_LegacyAcctType = dgItemRow.Cells(RET_LEGACY_ACCT_TYPE).Text.Trim()
                                l_boolBitBasicAcct = Convert.ToBoolean(dgItemRow.Cells(RET_BASIC_ACCT).Text.Trim())
                                l_PlanType = dgItemRow.Cells(RET_PLANE_TYPE).Text.Trim().ToUpper()
                                chkSelected = CType(dgItemRow.Cells(RET_SEL_ACCT_CHK).FindControl("CheckboxRet"), CheckBox)
                                l_LabelProjAcctBal = CType(dgItemRow.Cells(RET_PROJ_BALANCE).FindControl("LabelProjectedBalRet"), Label)

                                l_drGroupedAcctFoundRow = l_dtGroupedElectiveAcct.Select("bitBasicAcct=" & l_boolBitBasicAcct & " AND PlanType='" & l_PlanType & "' AND chrLegacyAcctType='" & l_LegacyAcctType & "'")
                                If Not chkSelected Is Nothing And l_drGroupedAcctFoundRow.Length > 0 Then
                                    'Added By Dinesh Kanojia
                                    l_drGroupedAcctFoundRow(0)("Selected") = chkSelected.Checked
                                    ' set Retirement Acct datagrid display projected balance 
                                    ' commented by chandrasekar.c This logic is moved  below to sum of  account type amount if account type is checked 
                                    'Start Added by Dinesh Kanojia
                                    'Start: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                                    If chkRetirementAccount.Checked And Not String.IsNullOrEmpty(txtRetirementAccount.Text.Trim) And DropDownListRetirementType.SelectedValue.ToUpper().Trim = "NORMAL" Then


                                        decPartialAmount = decTotalProjRet / Convert.ToDecimal(txtRetirementAccount.Text.Trim)
                                        decAccountTotal = Math.Round(Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")) / decPartialAmount, 2)
                                        decProjectedBalance = IIf(bIsBALegacyCombinedAccountRule = True, decYMCAAcctAndYMCALegacyAcctBalances, l_drGroupedAcctFoundRow(0)("mnyProjectedBalance"))

                                        'The YMCA Legacy and YMCA Account balances validations are not required to be applied for QDRO beneficiaries.
                                        If Me.FundEventStatus <> "QD" Then 'Start:Added by Chandrasekar.c on 2015.11.02 YRS-AT-2554:QD is not restricted on partial withdrawal in case of Account balance greater/Less $25000 or greater $5000
                                            dgItemRow.ForeColor = Color.Black
                                            Select Case l_drGroupedAcctFoundRow(0)("chrAcctType").ToString.ToUpper().Trim
                                                Case "YMCA(LEGACY) ACCOUNT"

                                                    'If the persons projected balance is greater than max pia (configured as $25k) 
                                                    'and he/she is not terminated then the estimation process should not consider the legacy account for partial withdrawal what if analysis.
                                                    'If Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance").ToString.ToUpper().Trim) > REFUND_MAX_PIA Then
                                                    'IF legacyAcc > 25000 and ActivePerson and existing Rule
                                                    If Convert.ToDecimal(decProjectedBalance) > REFUND_MAX_PIA And Me.IsPersonTerminated = False And bIsBALegacyCombinedAccountRule = False Then 'Added by Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                                                        l_LabelProjAcctBal.Text = String.Format("{0:0.00}", l_drGroupedAcctFoundRow(0)("mnyProjectedBalance"))

                                                        'START : ML| 2019.04.09 | YRS-AT-4127 - Based on flag messages will handle at below in common code
                                                        'LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_LEGACY_ACCOUNT_REFUND_MAX_PIA").ToString().Replace("$$REFUND_MAX_PIA$$", REFUND_MAX_PIA.ToString())
                                                        IsYmcaLegacyValidationMgs = True
                                                        'END : ML| 2019.04.09 | YRS-AT-4127 - Based on flag messages will handle at below in common code

                                                        'Commented By chandrarasekar.C for YRS-AT-2486
                                                        'decTotalEstimate += Convert.ToDecimal(l_LabelProjAcctBal.Text)

                                                        chkSelected.Checked = True
                                                        dgItemRow.ForeColor = Color.Red
                                                        decCheckBalAmount = Convert.ToDecimal(l_LabelProjAcctBal.Text)
                                                        bIsExclude = True

                                                        'If the persons legacy account balance as on termination date is greater than max pia (configured as $25k) 
                                                        'and he/she is terminated then the estimation process should not consider the legacy account for partial withdrawal what if analysis.
                                                        'START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                                                        'If (YMCA Acount + YMCA (legacy)Account)  > 50000  and Combined rule
                                                    ElseIf Convert.ToDecimal(decProjectedBalance) > BA_LEGACY_MAX_COMBINED_LIMIT And bIsBALegacyCombinedAccountRule = True Then
                                                        If decPIA_At_Termination > REFUND_MAX_PIA_AT_TERM Then 'And also YMCA (legacy)Account at Termination > 25000
                                                            l_LabelProjAcctBal.Text = String.Format("{0:0.00}", l_drGroupedAcctFoundRow(0)("mnyProjectedBalance"))
                                                            chkSelected.Checked = True
                                                            dgItemRow.ForeColor = Color.Red
                                                            decCheckBalAmount = Convert.ToDecimal(l_LabelProjAcctBal.Text)
                                                            bIsExclude = True
                                                            IsYmcaLegacyValidationMgs = True
                                                        Else
                                                            dgItemRow.ForeColor = Color.Black
                                                            l_LabelProjAcctBal.Text = String.Format("{0:0.00}", Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")) - decAccountTotal)
                                                        End If
                                                        'END : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                                                    ElseIf decPIA_At_Termination > REFUND_MAX_PIA_AT_TERM And Me.IsPersonTerminated = True And bIsBALegacyCombinedAccountRule = False Then  'Start:Added by Chandrasekar.c on 2015.11.19 YRS-AT-2479 : Partial Withdrawal Estimate is allowed or not for terminated Participant with YMCA(LEGACY) ACCOUNT Balance

                                                        l_LabelProjAcctBal.Text = String.Format("{0:0.00}", l_drGroupedAcctFoundRow(0)("mnyProjectedBalance"))

                                                        'START : ML| 2019.04.09 | YRS-AT-4127 - Based on flag messages will handle at below in common code
                                                        'LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_LEGACY_ACCOUNT_REFUND_MAX_PIA_AT_TERMINATION").ToString().Replace("$$REFUND_MAX_PIA$$", REFUND_MAX_PIA.ToString())
                                                        IsYmcaLegacyValidationMgs = True
                                                        'END : ML| 2019.04.09 | YRS-AT-4127 - Based on flag messages will handle at below in common code
                                                        chkSelected.Checked = True
                                                        dgItemRow.ForeColor = Color.Red
                                                        decCheckBalAmount = Convert.ToDecimal(l_LabelProjAcctBal.Text)
                                                        bIsExclude = True
                                                        'End:Added by Chandrasekar.c on 2015.11.19 YRS-AT-2479 : Partial Withdrawal Estimate is allowed or not for terminated Participant with YMCA(LEGACY) ACCOUNT Balance
                                                    Else
                                                        dgItemRow.ForeColor = Color.Black
                                                        l_LabelProjAcctBal.Text = String.Format("{0:0.00}", Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")) - decAccountTotal)

                                                        'Commented By chandrarasekar.C for YRS-AT-2486
                                                        'decTotalEstimate += Convert.ToDecimal(l_LabelProjAcctBal.Text)
                                                    End If
                                                Case "YMCA ACCOUNT"
                                                    If Not String.IsNullOrEmpty(TextBoxRetirementAge.Text.Trim) And Convert.ToDecimal(TextBoxRetirementAge.Text.Trim) >= SS_MIN_AGE Then
                                                        If Convert.ToDecimal(decProjectedBalance) > BA_LEGACY_MAX_COMBINED_LIMIT Then
                                                            If Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")) > BA_MAX_LIMIT_55_ABOVE Then
                                                                l_LabelProjAcctBal.Text = String.Format("{0:0.00}", l_drGroupedAcctFoundRow(0)("mnyProjectedBalance"))
                                                                'START : ML| 2019.04.09 | YRS-AT-4127 - Messages Handle based on flag at  below
                                                                'If bIsBALegacyCombinedAccountRule = False Then
                                                                '    LabelPartialWithdrawal.Text += "<br>" + getmessage("MESSAGE_RETIREMENT_ESTIMATE_BA_MAX_LIMIT_55_ABOVE").ToString().Replace("$$SS_MIN_AGE$$", SS_MIN_AGE.ToString()).Replace("$$BA_MAX_LIMIT_55_ABOVE$$", BA_MAX_LIMIT_55_ABOVE.ToString())
                                                                'Else
                                                                ' IsYmcaAccountValidation = True
                                                                'End If
                                                                    IsYmcaAccountValidation = True
                                                                'END : ML| 2019.04.09 | YRS-AT-4127 - Messages Handle based on flag at  below
                                                                'Commented By chandrarasekar.c for YRS-AT-2486 
                                                                'decTotalEstimate += Convert.ToDecimal(l_LabelProjAcctBal.Text)
                                                                dgItemRow.ForeColor = Color.Red
                                                                chkSelected.Checked = True
                                                                decCheckBalAmount = Convert.ToDecimal(l_LabelProjAcctBal.Text)
                                                                bIsExclude = True
                                                            Else
                                                                dgItemRow.ForeColor = Color.Black
                                                                l_LabelProjAcctBal.Text = String.Format("{0:0.00}", Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")) - decAccountTotal)
                                                            End If

                                                        Else
                                                            dgItemRow.ForeColor = Color.Black
                                                            l_LabelProjAcctBal.Text = String.Format("{0:0.00}", Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")) - decAccountTotal)
                                                        End If
                                                    End If
                                                Case Else
                                                    l_LabelProjAcctBal.Text = String.Format("{0:0.00}", Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")) - decAccountTotal)

                                                    'Commented By chandrarasekar.c for YRS-AT-2486 
                                                    ' decTotalEstimate += Convert.ToDecimal(l_LabelProjAcctBal.Text)
                                            End Select

                                            'START : ML| 2019.04.09 |YRS-AT-4127 - Handle warning message based on If (IsYmcaLegacyValidationMgs and IsYmcaAccountValidation) both or any one validations are true and participant is Active/Inactive  
                                            'START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
                                            'If IsYmcaLegacyValidationMgs And IsYmcaAccountValidation Then
                                            '    LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_BA_COMBINED_ACCOUNT").ToString().Replace("$$BA_MAX_COMBINED_LIMIT$$", BA_MAX_LIMIT_55_ABOVE.ToString())
                                            '    LabelPartialWithdrawal.Text += "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_LEGACY_ACCOUNT_REFUND_MAX_PIA_AT_TERMINATION").ToString().Replace("$$REFUND_MAX_PIA$$", REFUND_MAX_PIA_AT_TERM.ToString())
                                            'ElseIf IsYmcaLegacyValidationMgs Then
                                            '    LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_LEGACY_ACCOUNT_REFUND_MAX_PIA_AT_TERMINATION").ToString().Replace("$$REFUND_MAX_PIA$$", REFUND_MAX_PIA_AT_TERM.ToString())
                                            'ElseIf IsYmcaAccountValidation Then
                                            '    LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_BA_COMBINED_ACCOUNT").ToString().Replace("$$BA_MAX_COMBINED_LIMIT$$", BA_MAX_LIMIT_55_ABOVE.ToString())
                                            'End If
                                            'END : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)

                                            'YRS-AT-4127 - YMCA Legacy Balance and YMCA Account balance both are under limit and particpant is Active
                                            If IsYmcaLegacyValidationMgs And IsYmcaAccountValidation And Me.IsPersonTerminated = True Then
                                                LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_BA_COMBINED_ACCOUNT").ToString().Replace("$$BA_MAX_COMBINED_LIMIT$$", BA_MAX_LIMIT_55_ABOVE.ToString())
                                                LabelPartialWithdrawal.Text += "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_LEGACY_ACCOUNT_REFUND_MAX_PIA_AT_TERMINATION").ToString().Replace("$$REFUND_MAX_PIA$$", REFUND_MAX_PIA_AT_TERM.ToString())
                                                'YRS-AT-4127 - YMCA Legacy Balance and YMCA Account balance both are under limit and particpant is Terminated
                                            ElseIf IsYmcaLegacyValidationMgs And IsYmcaAccountValidation And Me.IsPersonTerminated = False Then
                                                LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_LEGACY_ACCOUNT_REFUND_MAX_PIA").ToString().Replace("$$REFUND_MAX_PIA$$", REFUND_MAX_PIA.ToString())
                                                LabelPartialWithdrawal.Text += "<br>" + getmessage("MESSAGE_RETIREMENT_ESTIMATE_BA_MAX_LIMIT_55_ABOVE").ToString().Replace("$$SS_MIN_AGE$$", SS_MIN_AGE.ToString()).Replace("$$BA_MAX_LIMIT_55_ABOVE$$", BA_MAX_LIMIT_55_ABOVE.ToString())
                                                'YRS-AT-4127 - YMCA Legacy Balance is under limit and particpant is Active
                                            ElseIf IsYmcaLegacyValidationMgs And Me.IsPersonTerminated = True Then
                                                LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_LEGACY_ACCOUNT_REFUND_MAX_PIA_AT_TERMINATION").ToString().Replace("$$REFUND_MAX_PIA$$", REFUND_MAX_PIA_AT_TERM.ToString())
                                                'YRS-AT-4127 - YMCA Legacy Balance is under limit and particpant is Terminated
                                            ElseIf IsYmcaLegacyValidationMgs And Me.IsPersonTerminated = False Then
                                                LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_LEGACY_ACCOUNT_REFUND_MAX_PIA").ToString().Replace("$$REFUND_MAX_PIA$$", REFUND_MAX_PIA.ToString())
                                                'YRS-AT-4127 - YMCA Account Balance is under limit and particpant is Active
                                            ElseIf IsYmcaAccountValidation And Me.IsPersonTerminated = True Then
                                                LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_BA_COMBINED_ACCOUNT").ToString().Replace("$$BA_MAX_COMBINED_LIMIT$$", BA_MAX_LIMIT_55_ABOVE.ToString())
                                                'YRS-AT-4127 - YMCA Account Balance is under limit and particpant is Terminated
                                            ElseIf IsYmcaAccountValidation And Me.IsPersonTerminated = False Then
                                                LabelPartialWithdrawal.Text = "<br>" + getmessage("MESSAGE_RETIREMENT_ESTIMATE_BA_MAX_LIMIT_55_ABOVE").ToString().Replace("$$SS_MIN_AGE$$", SS_MIN_AGE.ToString()).Replace("$$BA_MAX_LIMIT_55_ABOVE$$", BA_MAX_LIMIT_55_ABOVE.ToString())
                                            End If
                                            'END :  ML| 2019.04.09 |YRS-AT-4127 - Handle warning message based on If (IsYmcaLegacyValidationMgs and IsYmcaAccountValidation) both or any one validations are true and participant is Active/Inactive  
                                        Else

                                            l_LabelProjAcctBal.Text = String.Format("{0:0.00}", Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")) - decAccountTotal)

                                        End If  'End:Added by Chandrasekar.c on 2015.11.02 YRS-AT-2554: QD is not restricted on partial withdrawal in case of Account balance greater/Less $25000 or greater $5000

                                    Else
                                        l_LabelProjAcctBal.Text = String.Format("{0:0.00}", Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")))
                                        'Commented By chandrarasekar.c for YRS-AT-2486
                                        'decTotalEstimate += Convert.ToDecimal(l_LabelProjAcctBal.Text)
                                        'Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                                        dgItemRow.ForeColor = Color.Black
                                    End If
                                    'Start:Added by Chandrasekar.c on 2015.10.27 YRS Ticket-2486 Adding projected Balances based of account type checked in the Retriement Plan and set to Projected Reserve in the Summary Tab
                                    If chkSelected.Checked = True Then
                                        ' l_LabelProjAcctBal.Text = String.Format("{0:0.00}", l_drGroupedAcctFoundRow(0)("mnyProjectedBalance"))
                                        decTotalEstimate += Convert.ToDecimal(l_LabelProjAcctBal.Text)

                                    Else
                                        l_LabelProjAcctBal.Text = "0.00"
                                        If l_boolBitBasicAcct = False Then
                                            If l_drGroupedAcctFoundRow(0)("bitRet_Voluntary") = True Then
                                                l_drGroupedAcctFoundRow(0)("mnyProjectedBalance") = 0
                                            End If
                                        End If
                                    End If
                                    'Ends:Added by Chandrasekar.c on 2015.10.27 Adding projected Balances based of account type checked in the Retriement Plan and set to Projected Reserve in the Summary Tab

                                    'Ends Added by Dinesh Kanojia
                                End If
                            Next

                            If (decCheckBalAmount > 0) Then
                                If Not String.IsNullOrEmpty(txtRetirementAccount.Text.Trim) And Convert.ToDecimal(decTotalProjRet) > 0 Then
                                    If Convert.ToDecimal(txtRetirementAccount.Text) > Convert.ToDecimal(decTotalProjRet) Then
                                        ' decEligibleAmount = decTotalProjRet
                                        If decTotalProjRet > (MinimumPIAToRetire + MIN_PARTIAL_WITHDRAWAL_LIMIT) Then
                                            decEligibleAmount = decTotalProjRet
                                            If bIsExclude = False Then
                                                decEligibleAmount = decEligibleAmount - MinimumPIAToRetire
                                            End If
                                            Dim strMessage As String = getmessage("MESSAGE_RETIRMENT_MAXIMUM_ELIGIBLE_AMOUNT")
                                            lblRetirementPartialAmountEligible.Text = strMessage.Replace("$$EligibleAmount$$", decEligibleAmount.ToString())
                                        End If
                                        MessageBox.Show(PlaceHolder1, "YMCA-YRS", getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREMENT_GREATER_TOTAL_AMOUNT"), MessageBoxButtons.Stop)
                                        chkSelected.Checked = True
                                        LabelPartialWithdrawal.Text = ""
                                        l_dtGroupedElectiveAcct.AcceptChanges()

                                        If Not Session("dsElectiveAccountsDet") Is Nothing Then
                                            If Not Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts") Is Nothing Then
                                                Dim dsfinal As DataSet
                                                dsSessionStoreDataSet = CType(Session("dsElectiveAccountsDet"), DataSet)
                                                dsSessionStoreDataSet.Tables.Remove("RetireeGroupedElectiveAccounts")
                                                dsSessionStoreDataSet.Tables.Add(l_dtGroupedElectiveAcct)

                                                tblFooterTotal = CType(DatagridElectiveRetirementAccounts.Controls(0), Table)
                                                grdFooterTotal = CType(tblFooterTotal.Controls(tblFooterTotal.Controls.Count - 1), DataGridItem)
                                                lblProjectedTotalBalRet = CType(grdFooterTotal.FindControl("LabelProjectedTotalBalRet"), Label)
                                                lblProjectedTotalBalRet.Text = decTotalEstimate.ToString()
                                            End If
                                        End If

                                        Return False
                                        Exit Function
                                    End If
                                End If
                            ElseIf Not String.IsNullOrEmpty(txtRetirementAccount.Text.Trim) Then
                                If Convert.ToDecimal(txtRetirementAccount.Text) > Convert.ToDecimal(decTotalProjRet) Then
                                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREMENT_GREATER_TOTAL_AMOUNT"), MessageBoxButtons.Stop)
                                    If decTotalProjRet > (MinimumPIAToRetire + MIN_PARTIAL_WITHDRAWAL_LIMIT) Then
                                        decEligibleAmount = decTotalProjRet
                                        decEligibleAmount = decEligibleAmount - MinimumPIAToRetire
                                        Dim strMessage As String = getmessage("MESSAGE_RETIRMENT_MAXIMUM_ELIGIBLE_AMOUNT")
                                        'lblRetirementPartialAmountEligible.Text = "Maximum eligible amount for partial value is: " + decEligibleAmount.ToString()
                                        lblRetirementPartialAmountEligible.Text = strMessage.Replace("$$EligibleAmount$$", decEligibleAmount.ToString())
                                    End If

                                    chkSelected.Checked = True
                                    LabelPartialWithdrawal.Text = ""
                                    l_dtGroupedElectiveAcct.AcceptChanges()

                                    If Not Session("dsElectiveAccountsDet") Is Nothing Then
                                        If Not Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts") Is Nothing Then
                                            Dim dsfinal As DataSet
                                            dsSessionStoreDataSet = CType(Session("dsElectiveAccountsDet"), DataSet)
                                            dsSessionStoreDataSet.Tables.Remove("RetireeGroupedElectiveAccounts")
                                            dsSessionStoreDataSet.Tables.Add(l_dtGroupedElectiveAcct)

                                            tblFooterTotal = CType(DatagridElectiveRetirementAccounts.Controls(0), Table)
                                            grdFooterTotal = CType(tblFooterTotal.Controls(tblFooterTotal.Controls.Count - 1), DataGridItem)
                                            lblProjectedTotalBalRet = CType(grdFooterTotal.FindControl("LabelProjectedTotalBalRet"), Label)
                                            lblProjectedTotalBalRet.Text = decTotalEstimate.ToString()

                                        End If
                                    End If

                                    Return False
                                    Exit Function
                                End If
                            End If

                            tblFooterTotal = CType(DatagridElectiveRetirementAccounts.Controls(0), Table)
                            grdFooterTotal = CType(tblFooterTotal.Controls(tblFooterTotal.Controls.Count - 1), DataGridItem)
                            lblProjectedTotalBalRet = CType(grdFooterTotal.FindControl("LabelProjectedTotalBalRet"), Label)
                            lblProjectedTotalBalRet.Text = decTotalEstimate.ToString()
                            If Not String.IsNullOrEmpty(txtRetirementAccount.Text.Trim) Then
                                If Convert.ToDecimal(lblProjectedTotalBalRet.Text.Trim) <= MinimumPIAToRetire Then
                                    LabelPartialWithdrawal.Text = "Minimum $" & MinimumPIAToRetire.ToString("##,##0") & " balance required after Withdrawal for Retirement plan, so this withdrawal is not permitted as requested."
                                    chkSelected.Checked = True
                                    l_dtGroupedElectiveAcct.AcceptChanges()

                                    If Not Session("dsElectiveAccountsDet") Is Nothing Then
                                        If Not Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts") Is Nothing Then
                                            Dim dsfinal As DataSet
                                            dsSessionStoreDataSet = CType(Session("dsElectiveAccountsDet"), DataSet)
                                            dsSessionStoreDataSet.Tables.Remove("RetireeGroupedElectiveAccounts")
                                            dsSessionStoreDataSet.Tables.Add(l_dtGroupedElectiveAcct)

                                            tblFooterTotal = CType(DatagridElectiveRetirementAccounts.Controls(0), Table)
                                            grdFooterTotal = CType(tblFooterTotal.Controls(tblFooterTotal.Controls.Count - 1), DataGridItem)
                                            lblProjectedTotalBalRet = CType(grdFooterTotal.FindControl("LabelProjectedTotalBalRet"), Label)
                                            lblProjectedTotalBalRet.Text = decTotalEstimate.ToString()

                                        End If
                                    End If
                                    Return False
                                End If
                            End If
                            hdnDecPartialValue.Value = lblProjectedTotalBalRet.Text
                            'End: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                        End If
                    End If
                    'Start: Dinesh Kanojia(DK)     2013-10-24      BT:Attempted to divide by zero error on Retirement Partial Withdrawal Estimate.
                    'Add Condition to restrict invalid message prompt By Dinesh Kanojia(DK) 2013-11-07 YRS 5.0-1443:Include partial withdrawals
                ElseIf Me.PlanType <> "S" And chkRetirementAccount.Checked Then
                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", getmessage("MESSAGE_RETIREMENT_ESTIMATE_THRESHOLD_EXCLUSION"), MessageBoxButtons.Stop)
                    Return False
                    Exit Function
                    'End: Dinesh Kanojia(DK)     2013-10-24      BT:Attempted to divide by zero error on Retirement Partial Withdrawal Estimate.
                End If

                'Saving Plan
                If (Me.PlanType = "S" Or Me.PlanType = "B") Then
                    If Not DatagridElectiveSavingsAccounts Is Nothing Then
                        If DatagridElectiveSavingsAccounts.Items.Count > 0 Then
                            Dim decTotalEstimate As Decimal = 0
                            Dim decEligibleAmount As Decimal = 0
                            lblSavingPartialAmountEligible.Visible = True
                            Dim datviewSav As DataView
                            Dim dtNewSavingsPlan As DataTable
                            Dim StrFilter As String

                            'Start: Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                            StrFilter = "((YMCATotal > 0 OR PersonalTotal > 0) OR bitRet_Voluntary = 1 OR bitFutureAcctVisible=1)"

                            datviewSav = New DataView(l_dtGroupedElectiveAcct, _
                                                                 "PlanType = 'SAVINGS' AND " & StrFilter, _
                                                                "chrAcctType", _
                                                                DataViewRowState.CurrentRows)
                            dtNewSavingsPlan = datviewSav.ToTable()
                            'End: Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                            For Each dgItemRow As DataGridItem In DatagridElectiveSavingsAccounts.Items
                                l_LegacyAcctType = dgItemRow.Cells(SAV_LEGACY_ACCT_TYPE).Text.Trim()
                                l_boolBitBasicAcct = Convert.ToBoolean(dgItemRow.Cells(SAV_BASIC_ACCT).Text.Trim())
                                l_PlanType = dgItemRow.Cells(SAV_PLANE_TYPE).Text.Trim().ToUpper()
                                chkSelected = CType(dgItemRow.Cells(SAV_SEL_ACCT_CHK).FindControl("CheckboxSav"), CheckBox)
                                l_LabelProjAcctBal = CType(dgItemRow.Cells(SAV_PROJ_BALANCE).FindControl("LabelProjectedBalSav"), Label)
                                l_drGroupedAcctFoundRow = l_dtGroupedElectiveAcct.Select("bitBasicAcct=" & l_boolBitBasicAcct & " AND PlanType='" & l_PlanType & "' AND chrLegacyAcctType='" & l_LegacyAcctType & "'")
                                'Start: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals


                                If dtNewSavingsPlan.Rows.Count > 0 Then
                                    For idtCount As Integer = 0 To dtNewSavingsPlan.Rows.Count - 1
                                        If (dtNewSavingsPlan.Rows(idtCount)("chrAcctType").ToString().Trim.ToUpper = l_LegacyAcctType.Trim.ToUpper And Convert.ToBoolean(dtNewSavingsPlan.Rows(idtCount)("Selected").ToString().Trim) = chkSelected.Checked) Then
                                            Session("chrAcctType") = l_LegacyAcctType
                                            l_LabelProjAcctBal.Text = Convert.ToDecimal(l_LabelProjAcctBal.Text) + Convert.ToDecimal(dtNewSavingsPlan.Rows(idtCount)("mnyProjectedBalance").ToString().Trim)
                                        End If
                                    Next
                                End If

                                If Not chkSelected Is Nothing And l_drGroupedAcctFoundRow.Length > 0 And Convert.ToDecimal(l_LabelProjAcctBal.Text) > 0 Then

                                    l_drGroupedAcctFoundRow(0)("Selected") = chkSelected.Checked
                                    ' set Saving Acct datagrid display projected balance 
                                    If chkSelected.Checked = True Then
                                        l_LabelProjAcctBal.Text = String.Format("{0:0.00}", l_drGroupedAcctFoundRow(0)("mnyProjectedBalance"))
                                        If chkSavingPartialAmount.Checked And Not String.IsNullOrEmpty(txtSavingPartialAmount.Text.Trim) And DropDownListRetirementType.SelectedValue.ToUpper().Trim = "NORMAL" Then
                                            decTotalProjRet = 0
                                            decTotalProjRet = Convert.ToDecimal(l_dtGroupedElectiveAcct.Compute("SUM(mnyProjectedBalance)", "PlanType='" + l_PlanType + "'"))
                                            If (decTotalProjRet > 0) Then
                                                decPartialAmount = decTotalProjRet / Convert.ToDecimal(txtSavingPartialAmount.Text.Trim)
                                                decAccountTotal = Math.Round(Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")) / decPartialAmount, 2)
                                                l_LabelProjAcctBal.Text = String.Format("{0:0.00}", Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance")) - decAccountTotal)
                                                decTotalEstimate += Convert.ToDecimal(l_LabelProjAcctBal.Text)
                                            End If
                                        Else
                                            l_LabelProjAcctBal.Text = Convert.ToDecimal(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance"))
                                            decTotalEstimate += Convert.ToDecimal(l_LabelProjAcctBal.Text)
                                        End If
                                    Else
                                        l_LabelProjAcctBal.Text = "0.00"
                                        If l_boolBitBasicAcct = False Then
                                            If l_drGroupedAcctFoundRow(0)("bitRet_Voluntary") = True Then
                                                l_drGroupedAcctFoundRow(0)("mnyProjectedBalance") = 0
                                                'Dinesh Kanojia(DK)     2013-10-24      BT:Attempted to divide by zero error on Retirement Partial Withdrawal Estimate.
                                                'decTotalEstimate = 0
                                            End If
                                        End If
                                    End If
                                End If
                                'Start: Dinesh Kanojia         2015.04.27      BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
                                If dgItemRow.Cells(1).Text.Trim.ToLower = "ln" Or dgItemRow.Cells(1).Text.Trim.ToLower = "ld" Then
                                    CType(dgItemRow.Cells(SAV_SEL_ACCT_CHK).FindControl("CheckboxSav"), CheckBox).Enabled = True
                                    CType(dgItemRow.Cells(SAV_SEL_ACCT_CHK).FindControl("CheckboxSav"), CheckBox).Checked = False
                                    CType(dgItemRow.Cells(SAV_SEL_ACCT_CHK).FindControl("CheckboxSav"), CheckBox).Enabled = False
                                End If
                                'End: Dinesh Kanojia         2015.04.27      BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
                            Next
                            'Start: Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                            If Not String.IsNullOrEmpty(txtSavingPartialAmount.Text) And decTotalProjRet > 0 Then
                                If Convert.ToDecimal(txtSavingPartialAmount.Text) > Convert.ToDecimal(decTotalProjRet) Then
                                    MessageBox.Show(PlaceHolder1, "YMCA-YRS", getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREMENT_GREATER_TOTAL_AMOUNT"), MessageBoxButtons.Stop)
                                    If decTotalProjRet > (MinimumPIAToRetire + MIN_PARTIAL_WITHDRAWAL_LIMIT) Then
                                        Dim strMessage As String = getmessage("MESSAGE_RETIRMENT_MAXIMUM_ELIGIBLE_AMOUNT")
                                        decEligibleAmount = decTotalProjRet - MinimumPIAToRetire
                                        lblSavingPartialAmountEligible.Text = strMessage.Replace("$$EligibleAmount$$", decEligibleAmount.ToString())
                                        chkSelected.Checked = True
                                        l_dtGroupedElectiveAcct.AcceptChanges()
                                        If Not Session("dsElectiveAccountsDet") Is Nothing Then
                                            If Not Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts") Is Nothing Then
                                                Dim dsfinal As DataSet
                                                dsSessionStoreDataSet = CType(Session("dsElectiveAccountsDet"), DataSet)
                                                dsSessionStoreDataSet.Tables.Remove("RetireeGroupedElectiveAccounts")
                                                dsSessionStoreDataSet.Tables.Add(l_dtGroupedElectiveAcct)
                                            End If
                                        End If
                                    End If
                                    Return False
                                    Exit Function
                                ElseIf decTotalEstimate < MinimumPIAToRetire Then
                                    LabelPartialWithdrawal.Text = "Minimum $" & MinimumPIAToRetire.ToString("##,##0") & " balance required after Withdrawal for savings plan, so this withdrawal is not permitted as requested."
                                    lblSavingPartialAmountEligible.Text = ""
                                    If Not Session("dsElectiveAccountsDet") Is Nothing Then
                                        If Not Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts") Is Nothing Then
                                            Dim dsfinal As DataSet
                                            dsSessionStoreDataSet = CType(Session("dsElectiveAccountsDet"), DataSet)
                                            dsSessionStoreDataSet.Tables.Remove("RetireeGroupedElectiveAccounts")
                                            dsSessionStoreDataSet.Tables.Add(l_dtGroupedElectiveAcct)
                                        End If
                                    End If
                                    Return False
                                    Exit Function
                                End If
                            End If
                            'End: Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                            'Start: Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                            tblFooterTotal = CType(DatagridElectiveSavingsAccounts.Controls(0), Table)
                            grdFooterTotal = CType(tblFooterTotal.Controls(tblFooterTotal.Controls.Count - 1), DataGridItem)
                            lblProjectedTotalBalRet = CType(grdFooterTotal.FindControl("LabelProjectedTotalBalsav"), Label)
                            lblProjectedTotalBalRet.Text = decTotalEstimate.ToString()
                            hdnDecSavingValue.Value = lblProjectedTotalBalRet.Text

                            'decEligibleAmount = decTotalEstimate
                            'decEligibleAmount = decEligibleAmount - (MIN_PARTIAL_WITHDRAWAL_LIMIT + MinimumPIAToRetire)
                            'lblSavingPartialAmountEligible.Text = "Eligible amount for partial value is: " + decEligibleAmount.ToString()

                            'End: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                            'End: Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
                        End If
                    End If
                End If

                l_dtGroupedElectiveAcct.AcceptChanges()


                If Not Session("dsElectiveAccountsDet") Is Nothing Then
                    If Not Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts") Is Nothing Then
                        Dim dsfinal As DataSet
                        dsSessionStoreDataSet = CType(Session("dsElectiveAccountsDet"), DataSet)
                        dsSessionStoreDataSet.Tables.Remove("RetireeGroupedElectiveAccounts")
                        dsSessionStoreDataSet.Tables.Add(l_dtGroupedElectiveAcct)
                    End If
                End If
            End If
            'Endif l_dtGroupedElectiveAcct is not nothing
            'Start: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
            Return True
            'End: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Protected Sub DatagridElectiveSavingsAccounts_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim g_String_Exception_Message As String
        Dim l_CheckBox As CheckBox
        Dim dgItem As DataGridItem

        Try
            l_CheckBox = CType(sender, CheckBox)
            dgItem = CType(l_CheckBox.NamingContainer, DataGridItem)

            If l_CheckBox.Checked = False Then
                ResetSavingAcctDataGridRow(dgItem)
            End If
            SetElectiveAccountSelectionState()
            ShowProjectedAcctBalancesTotal()
            LabelEstimateDataChangedMessage.Visible = True
        Catch ex As Exception
            g_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try
    End Sub
    Private Function ResetSavingAcctDataGridRow(ByVal dgSavingAcctRow As DataGridItem)
        Dim l_UC_DatePicker As YMCAUI.DateUserControl
        Dim l_DropDownList As DropDownList
        Dim l_TextBox As TextBox
        Dim l_string_AcctType As String
        Dim l_LabelProjBalance As Label
        Try
            If Not dgSavingAcctRow Is Nothing Then
                l_string_AcctType = dgSavingAcctRow.Cells(SAV_ACCT_TYPE).Text.ToString().Trim()

                l_DropDownList = CType(dgSavingAcctRow.FindControl("DropdownlistContribTypeSav"), DropDownList)
                If Not l_DropDownList Is Nothing And l_string_AcctType <> "RT" Then
                    l_DropDownList.SelectedValue = ""
                End If

                l_TextBox = CType(dgSavingAcctRow.FindControl("TextboxContribAmtSav"), TextBox)
                If Not l_TextBox Is Nothing Then
                    l_TextBox.Text = "0"
                End If

                l_UC_DatePicker = CType(dgSavingAcctRow.FindControl("DateusercontrolStartSav"), DateUserControl)
                If Not l_UC_DatePicker Is Nothing Then
                    l_UC_DatePicker.Text = ""
                End If

                l_UC_DatePicker = CType(dgSavingAcctRow.FindControl("DateusercontrolStopSav"), DateUserControl)
                If Not l_UC_DatePicker Is Nothing Then
                    l_UC_DatePicker.Text = ""
                End If

                l_LabelProjBalance = CType(dgSavingAcctRow.FindControl("LabelProjectedBalSav"), Label)
                If Not l_LabelProjBalance Is Nothing Then
                    l_LabelProjBalance.Text = "0.00"
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function SetDefaultAcctSelectionState()
        Dim l_dtGroupedElectiveAcct As DataTable
        Dim l_drGroupedAcctFoundRow As DataRow()
        Dim l_LegacyAcctType As String = String.Empty
        Dim l_PlanType As String = String.Empty
        Dim l_boolBitBasicAcct As Boolean

        Dim l_chkSelect As CheckBox
        Try
            If Not Session("dsElectiveAccountsDet") Is Nothing Then
                If Not Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts") Is Nothing Then
                    l_dtGroupedElectiveAcct = CType(Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts"), DataTable)
                End If
            End If
            If Not l_dtGroupedElectiveAcct Is Nothing Then
                'Retirement Plan
                If Me.PlanType = "R" Or Me.PlanType = "B" Then
                    'Set default selection state for Retirement plan
                    If Not DatagridElectiveRetirementAccounts Is Nothing Then
                        If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
                            For Each dgRetireRow As DataGridItem In Me.DatagridElectiveRetirementAccounts.Items
                                l_chkSelect = CType(dgRetireRow.Cells(RET_SEL_ACCT_CHK).FindControl("CheckboxRet"), CheckBox)
                                l_LegacyAcctType = dgRetireRow.Cells(RET_LEGACY_ACCT_TYPE).Text.Trim()
                                l_PlanType = dgRetireRow.Cells(RET_PLANE_TYPE).Text.Trim().ToUpper()
                                l_boolBitBasicAcct = Convert.ToBoolean(dgRetireRow.Cells(RET_BASIC_ACCT).Text.Trim())

                                If Not l_chkSelect Is Nothing Then
                                    If dgRetireRow.Cells(1).Text.Trim().ToLower = "ln" Or dgRetireRow.Cells(1).Text.Trim().ToLower = "ld" Then
                                        l_chkSelect.Checked = False
                                    End If
                                    If Convert.ToDecimal(dgRetireRow.Cells(RET_ACCT_TOTAL).Text.Trim()) > 0 Then
                                        l_chkSelect.Checked = True
                                    Else
                                        If l_boolBitBasicAcct = False Then
                                            l_drGroupedAcctFoundRow = l_dtGroupedElectiveAcct.Select("bitBasicAcct=" & l_boolBitBasicAcct & " AND PlanType='" & l_PlanType & "' AND chrLegacyAcctType='" & l_LegacyAcctType & "'")
                                            If l_drGroupedAcctFoundRow.Length > 0 Then
                                                If l_drGroupedAcctFoundRow(0)("Selected") = True Then
                                                    l_chkSelect.Checked = l_drGroupedAcctFoundRow(0)("Selected")
                                                Else
                                                    l_chkSelect.Checked = False
                                                    ResetRetirementAcctDataGridRow(dgRetireRow)
                                                End If

                                            End If
                                        Else
                                            l_chkSelect.Checked = True
                                        End If
                                    End If
                                    ''if plan type Both
                                    'If Me.PlanType = "B" Then
                                    '    l_drGroupedAcctFoundRow = l_dtGroupedElectiveAcct.Select("bitBasicAcct=" & l_boolBitBasicAcct & " AND PlanType='" & l_PlanType & "' AND chrLegacyAcctType='" & l_LegacyAcctType & "'")
                                    '    If l_drGroupedAcctFoundRow.Length > 0 Then
                                    '        l_chkSelect.Checked = l_drGroupedAcctFoundRow(0)("Selected")
                                    '    End If
                                    'Else
                                    '    'if plan type R
                                    '    If Convert.ToDecimal(dgRetireRow.Cells(RET_ACCT_TOTAL).Text.Trim()) > 0 Then
                                    '        l_chkSelect.Checked = True
                                    '    End If
                                    'End If

                                End If
                            Next
                        End If
                    End If
                End If

                'Saving Plan
                If Me.PlanType = "S" Or Me.PlanType = "B" Then
                    'Set default selection state for Saving plan
                    If Not DatagridElectiveSavingsAccounts Is Nothing Then
                        If DatagridElectiveSavingsAccounts.Items.Count > 0 Then
                            For Each dgSavingRow As DataGridItem In Me.DatagridElectiveSavingsAccounts.Items
                                l_chkSelect = CType(dgSavingRow.Cells(SAV_SEL_ACCT_CHK).FindControl("CheckboxSav"), CheckBox)
                                l_LegacyAcctType = dgSavingRow.Cells(SAV_LEGACY_ACCT_TYPE).Text.Trim()
                                l_boolBitBasicAcct = Convert.ToBoolean(dgSavingRow.Cells(SAV_BASIC_ACCT).Text.Trim())
                                l_PlanType = dgSavingRow.Cells(SAV_PLANE_TYPE).Text.Trim().ToUpper()
                                If Not l_chkSelect Is Nothing Then

                                    If Convert.ToDecimal(dgSavingRow.Cells(SAV_ACCT_TOTAL).Text.Trim()) > 0 Then
                                        l_chkSelect.Checked = True
                                    Else
                                        l_drGroupedAcctFoundRow = l_dtGroupedElectiveAcct.Select("bitBasicAcct=" & l_boolBitBasicAcct & " AND PlanType='" & l_PlanType & "' AND chrLegacyAcctType='" & l_LegacyAcctType & "'")
                                        If l_drGroupedAcctFoundRow.Length > 0 Then
                                            If l_drGroupedAcctFoundRow(0)("Selected") = True Then
                                                l_chkSelect.Checked = l_drGroupedAcctFoundRow(0)("Selected")
                                            Else
                                                l_chkSelect.Checked = False
                                                ResetSavingAcctDataGridRow(dgSavingRow)
                                            End If

                                        End If

                                    End If
                                    'If Me.PlanType = "B" Then
                                    '    l_drGroupedAcctFoundRow = l_dtGroupedElectiveAcct.Select("bitBasicAcct=" & l_boolBitBasicAcct & " AND PlanType='" & l_PlanType & "' AND chrLegacyAcctType='" & l_LegacyAcctType & "'")
                                    '    If l_drGroupedAcctFoundRow.Length > 0 Then
                                    '        l_chkSelect.Checked = l_drGroupedAcctFoundRow(0)("Selected")
                                    '    End If
                                    'Else
                                    '    If Convert.ToDecimal(dgSavingRow.Cells(SAV_ACCT_TOTAL).Text.Trim()) > 0 Then
                                    '        l_chkSelect.Checked = True
                                    '    End If
                                    'End If

                                End If
                                'Start: Dinesh Kanojia         2015.04.27      BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
                                If dgSavingRow.Cells(1).Text.Trim().ToLower = "ln" Or dgSavingRow.Cells(1).Text.Trim().ToLower = "ld" Then
                                    l_chkSelect.Enabled = True
                                    l_chkSelect.Checked = False
                                    l_chkSelect.Enabled = False
                                End If
                                'End: Dinesh Kanojia         2015.04.27      BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
                            Next
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function DisplayProjectedReserves()
        Dim l_SelectedRetProjTotal As Decimal = 0
        Dim l_SelectedSavProjTotal As Decimal = 0
        Dim l_ProjectedReserves As Decimal = 0
        Dim l_dtGroupedElectiveAcct As DataTable
        Dim l_drGroupAcctFoundRow As DataRow()

        Try
            'get RetireeGroupedElectiveAccounts from sesion variable
            If Not Session("dsElectiveAccountsDet") Is Nothing Then
                If Not Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts") Is Nothing Then
                    l_dtGroupedElectiveAcct = CType(Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts"), DataTable)
                End If
            End If

            If Not l_dtGroupedElectiveAcct Is Nothing Then



                If Me.PlanType = "R" OrElse Me.PlanType = "B" Then

                    l_SelectedRetProjTotal = 0

                    l_drGroupAcctFoundRow = l_dtGroupedElectiveAcct.Select("PlanType='RETIREMENT' AND Selected=True")
                    If l_drGroupAcctFoundRow.Length > 0 Then
                        ''l_SelectedRetProjTotal = Convert.ToDecimal(l_dtGroupedElectiveAcct.Compute("SUM(mnyProjectedBalance)", "PlanType='RETIREMENT' AND Selected=True"))
                        If hdnDecPartialValue.Value = "" Or hdnDecPartialValue.Value = "0" Then
                            hdnDecPartialValue.Value = Convert.ToDecimal(l_dtGroupedElectiveAcct.Compute("SUM(mnyProjectedBalance)", "PlanType='RETIREMENT' AND Selected=True")).ToString()
                        End If
                    End If

                    ''l_SelectedAcctTotal = Convert.ToDecimal(l_dtGroupedElectiveAcct.Compute("SUM(AcctTotal)", "PlanType='RETIREMENT' AND Selected=True"))
                    'Dim lblProjectedTotalBal As Label
                    'If Not DatagridElectiveRetirementAccounts Is Nothing Then
                    '    If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
                    '        lblProjectedTotalBal = CType(CType(DatagridElectiveRetirementAccounts.Controls(0).Controls(DatagridElectiveRetirementAccounts.Items.Count + 1).Controls(RET_PROJ_BALANCE), TableCell).FindControl("LabelProjectedTotalBalRet"), Label)
                    '        If Not lblProjectedTotalBal Is Nothing Then
                    '            lblProjectedTotalBal.Text = l_SelectedProjTotal
                    '        End If
                    '    End If
                    'End If

                End If
                'CType(CType(DatagridElectiveRetirementAccounts.Controls(0).Controls(DatagridElectiveRetirementAccounts.Items.Count + 1).Controls(RET_PROJ_BALANCE), TableCell).FindControl("LabelProjectedTotaBalRet"), Label).Text()
                If Me.PlanType = "S" Or Me.PlanType = "B" Then

                    l_SelectedSavProjTotal = 0

                    l_drGroupAcctFoundRow = l_dtGroupedElectiveAcct.Select("PlanType='SAVINGS' AND Selected=True")
                    If l_drGroupAcctFoundRow.Length > 0 Then
                        'l_SelectedSavProjTotal = Convert.ToDecimal(l_dtGroupedElectiveAcct.Compute("SUM(mnyProjectedBalance)", "PlanType='SAVINGS' AND Selected=True"))
                        If hdnDecSavingValue.Value = "" Or hdnDecSavingValue.Value = "0" Then
                            hdnDecSavingValue.Value = Convert.ToDecimal(l_dtGroupedElectiveAcct.Compute("SUM(mnyProjectedBalance)", "PlanType='SAVINGS' AND Selected=True"))
                        End If

                    End If

                    ''l_SelectedAcctTotal = Convert.ToDecimal(l_dtGroupedElectiveAcct.Compute("SUM(AcctTotal)", "PlanType='RETIREMENT' AND Selected=True"))
                    'Dim lblProjectedTotalBal As Label
                    'If Not DatagridElectiveSavingsAccounts Is Nothing Then
                    '    If DatagridElectiveSavingsAccounts.Items.Count > 0 Then
                    '        lblProjectedTotalBal = CType(CType(DatagridElectiveSavingsAccounts.Controls(0).Controls(DatagridElectiveSavingsAccounts.Items.Count + 1).Controls(SAV_PROJ_BALANCE), TableCell).FindControl("LabelProjectedTotalBalSav"), Label)
                    '        If Not lblProjectedTotalBal Is Nothing Then
                    '            lblProjectedTotalBal.Text = l_SelectedProjTotal
                    '        End If
                    '    End If
                    'End If
                End If


                '    'Code Added by DInesh Kanojia
                '    'Start: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals


                '    If hdnDecPartialValue.Value = "" Then
                '        hdnDecPartialValue.Value = "0"
                '    End If

                '    If hdnDecSavingValue.Value = "" Then
                '        hdnDecSavingValue.Value = "0"
                '    End If

                '    l_SelectedRetProjTotal = hdnDecPartialValue.Value
                '    l_SelectedSavProjTotal = hdnDecSavingValue.Value

                '    l_ProjectedReserves = l_SelectedRetProjTotal + l_SelectedSavProjTotal

                '    'End : Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                '    'l_ProjectedReserves = l_SelectedRetProjTotal + l_SelectedSavProjTotal
                '    If l_ProjectedReserves > 0 Then
                '        txtProjectedReserves.Text = String.Format("{0:0.00}", l_ProjectedReserves)
                '    Else
                '        txtProjectedReserves.Text = "0.00"
                '    End If
                'End If
            End If
            'Code Added by DInesh Kanojia
            'Start: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
            If hdnDecPartialValue.Value = "" Then
                hdnDecPartialValue.Value = "0"
            End If

            If hdnDecSavingValue.Value = "" Then
                hdnDecSavingValue.Value = "0"
            End If

            l_SelectedRetProjTotal = hdnDecPartialValue.Value
            l_SelectedSavProjTotal = hdnDecSavingValue.Value

            l_ProjectedReserves = l_SelectedRetProjTotal + l_SelectedSavProjTotal

            'End : Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
            'l_ProjectedReserves = l_SelectedRetProjTotal + l_SelectedSavProjTotal
            If l_ProjectedReserves > 0 Then
                txtProjectedReserves.Text = String.Format("{0:0.00}", l_ProjectedReserves)
            Else
                txtProjectedReserves.Text = "0.00"
            End If
            'End : Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Private Function UpdateProjBalanceInAcctGrids()
    '    Dim l_dtGroupedElectiveAcct As DataTable
    '    Dim l_drGroupedAcctFoundRow As DataRow()
    '    Dim l_LegacyAcctType As String = String.Empty
    '    Dim l_boolBitBasicAcct As Boolean
    '    Dim l_PlanType As String = String.Empty
    '    Dim l_LabelProjAcctBal As Label
    '    Try
    '        If Not Session("dsElectiveAccountsDet") Is Nothing Then
    '            If Not Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts") Is Nothing Then
    '                l_dtGroupedElectiveAcct = CType(Session("dsElectiveAccountsDet").Tables("RetireeGroupedElectiveAccounts"), DataTable)
    '            End If
    '        End If
    '        If Not l_dtGroupedElectiveAcct Is Nothing Then
    '            'Retirement Plan
    '            If Me.PlanType = "R" Or Me.PlanType = "B" Then
    '                If Not DatagridElectiveRetirementAccounts Is Nothing Then
    '                    If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
    '                        For Each dgItemRow As DataGridItem In DatagridElectiveRetirementAccounts.Items
    '                            l_LegacyAcctType = dgItemRow.Cells(RET_LEGACY_ACCT_TYPE).Text.Trim()
    '                            l_boolBitBasicAcct = Convert.ToBoolean(dgItemRow.Cells(RET_BASIC_ACCT).Text.Trim())
    '                            l_PlanType = dgItemRow.Cells(RET_PLANE_TYPE).Text.Trim().ToUpper()

    '                            l_LabelProjAcctBal = CType(dgItemRow.Cells(RET_PROJ_BALANCE).FindControl("LabelProjectedBalRet"), Label)

    '                            l_drGroupedAcctFoundRow = l_dtGroupedElectiveAcct.Select("bitBasicAcct=" & l_boolBitBasicAcct & " AND PlanType='" & l_PlanType & "' AND chrLegacyAcctType='" & l_LegacyAcctType & "'")
    '                            If l_drGroupedAcctFoundRow.Length > 0 And Not l_LabelProjAcctBal Is Nothing Then

    '                            End If
    '                        Next
    '                    End If
    '                End If
    '            End If
    '            'Saving Plan
    '            If Me.PlanType = "S" Or Me.PlanType = "B" Then
    '                If Not DatagridElectiveSavingsAccounts Is Nothing Then
    '                    If DatagridElectiveSavingsAccounts.Items.Count > 0 Then
    '                        For Each dgItemRow As DataGridItem In DatagridElectiveSavingsAccounts.Items
    '                            l_LegacyAcctType = dgItemRow.Cells(SAV_LEGACY_ACCT_TYPE).Text.Trim()
    '                            l_boolBitBasicAcct = Convert.ToBoolean(dgItemRow.Cells(SAV_BASIC_ACCT).Text.Trim())
    '                            l_PlanType = dgItemRow.Cells(SAV_PLANE_TYPE).Text.Trim().ToUpper()

    '                            l_LabelProjAcctBal = CType(dgItemRow.Cells(SAV_PROJ_BALANCE).FindControl("LabelProjectedBalSav"), Label)

    '                            l_drGroupedAcctFoundRow = l_dtGroupedElectiveAcct.Select("bitBasicAcct=" & l_boolBitBasicAcct & " AND PlanType='" & l_PlanType & "' AND chrLegacyAcctType='" & l_LegacyAcctType & "'")
    '                            If l_drGroupedAcctFoundRow.Length > 0 And Not l_LabelProjAcctBal Is Nothing Then
    '                            End If
    '                        Next
    '                    End If
    '                End If
    '            End If


    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    Private Sub DropDownListRetirementType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListRetirementType.SelectedIndexChanged
        Try 'PPP | 11/17/2017 | YRS-AT-3328 | Handling exception
            Session("employmentSalaryInformation") = Nothing 'MMR |2017.03.14 | YRS-AT-2625 | Clearing session value as session will be filled in evertime on dropdown change
            LabelEstimateDataChangedMessage.Visible = True
            SetRetireType()
            SetProjectionInterestRate() 'SR | 2017.03.09 | YRS-AT-2625 | Set default interest rate based on retire type.
            'LoadEmploymentDetails(Me.PersonId) 'MMR | 2017.03.14 | YRS-AT-2625 | Commented and move code to section where retire type is checked
            setAccountGridControls()

            SetSocialSecurityDetails()             ' SB | 03/14/2017 | YRS-AT-2625 | Function which clears the summary tab of participant elismated annuities 
            TextBoxProjFinalYrsSalary.Text = "0.00"

            Dim dsSalInfo As New DataSet

            'START: MMR | 2017.03.14 | YRS-AT-2625 | Commented and moved code to section where retire type is checked
            'If Not Session("dsPersonEmploymentDetails") Is Nothing Then
            '    dsSalInfo = DirectCast(Session("dsPersonEmploymentDetails"), DataSet)
            'End If
            'END: MMR | 2017.03.14 | YRS-AT-2625 | Commented and moved code to section where retire type is checked
            'YRS 5.0-1345 :BT-859
            PanelAlternatePayee.Visible = False
            'BT:892-YRS 5.0-1359 : Disability Estimate form
            PanelDisability.Visible = False
            Panel1.Visible = False
            'Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request
            tblIDMcheck.Visible = False
            TextBoxPraAssumption.Visible = False
            labelPRAAssumption.Visible = False
            ButtonPrint.Enabled = True
            Buttoncalculate.Enabled = True
            tabStripRetirementEstimate.Enabled = True
            'YRS 5.0-657 : Estimate with SSL amounts
            CheckSSLAvailable()
            '16-Feb-2011 Added by Sanket for disabiling controls when value in drop down is changed


            If Me.RetireType = "DISABL" Then
                'Session("employmentSalaryInformation") = Nothing 'MMR | 2017.03.14 | YRS-AT-2625 | Commented existing code as not required
                'To remove any previous error mesaages related to Normal
                LabelWarningMessage.Text = String.Empty
                LabelRDBWarningMessage.Text = String.Empty 'Ranu : YRS-AT-4133
                Dim lastTD As DateTime
                lastTD = GetLastTerminationDate()
                'Sanket vaidya          18 Apr 2011     BT-816 : Disability Retirement Estimate Issues.
                If (lastTD <> DateTime.MinValue) Then
                    Me.RetirementDate = GetFirstDayOfNextMonthIfNotFirstOfMonth(lastTD)
                    TextBoxRetirementDate.Text = Me.RetirementDate
                Else
                    Me.RetirementDate = DateTime.Now.ToString("MM/01/yyyy")
                    TextBoxRetirementDate.Text = Me.RetirementDate
                End If

                'Age Calculation
                TextBoxRetirementAge.Text = Convert.ToDecimal(DateDiff(DateInterval.Day, Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime(TextBoxRetirementDate.Text)) / 365).ToString.Substring(0, 2)

                LoadEmploymentDetails(Me.PersonId) 'MMR | 2017.03.14 | YRS-AT-2625 | Added to load employement details        

                'YRS 5.0-1035 : Set paid service - atsFundEvents.intPaid - to zero
                LoadManualTransactionForDisability(Me.FundEventId, Me.RetireType, TextBoxRetirementDate.Text.Trim()) 'MMR | 2017.02.27 | YRS-AT-2625 | Loading manual transaction details for retire type 'Disability'

                'START: SB | 03/08/2017 | YRS-AT-2625 | As discussed with team, Personal withdrawal exist validation should be validate only on Recalculate button.Hence commenting below line of code
                'If Me.PlanType = "B" Or Me.PlanType = "R" Then
                '    Dim isWithdrawn As Boolean
                ' isWithdrawn = RetirementBOClass.HasBasicMoneyWithdrawn(Me.FundEventId) 

                '    If (isWithdrawn) Then
                '        LabelRefundMessage.Visible = True
                '        'LabelRefundMessage.Text = "Participant has withdrawn funds from their basic accounts. Check if the participant has the required 60 months of paid service since the withdrawal."
                '        LabelRefundMessage.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_PARTICIPANT_WITHDRAWAL_FUNDS_FROM_BASIC_ACCOUNTS")
                '    End If
                'End If
                'END: SB | 03/08/2017 | YRS-AT-2625 | As discussed with team, Personal withdrawal exist validation should be validate only on Recalculate button.Hence commenting below line of code

            End If

            If Me.RetireType = "NORMAL" Then

                'YRS 5.0-1035 : Set paid service - atsFundEvents.intPaid - to zero : To clear the error message after validation in calculate click            
                'LabelRefundMessage.Text = String.Empty     ' SR | 03/08/2017 | YRS-AT-2625 | Code is commented to check only visibility of label.
                LabelRefundMessage.Visible = False

                'START: MMR | 2017.02.27 | YRS-AT-2625 | Hiding message and link for manual transaction for 'MORMAL' Retire type
                ShowHideManualTransactionLink(False)
                ShowHideMessageManualTransaction(False)
                Me.ManualTransactionDetails = Nothing
                ResetManualTransactionDetails()
                'END: MMR | 2017.02.27 | YRS-AT-2625 | Hiding message and link for manual transaction for 'MORMAL' Retire type

                'START: MMR | 2017.03.14 | YRS-AT-2625 | Setting session value in dataset
                If Not Session("dsPersonEmploymentDetails") Is Nothing Then
                    dsSalInfo = DirectCast(Session("dsPersonEmploymentDetails"), DataSet)
                End If
                'END: MMR | 2017.03.14 | YRS-AT-2625 | Setting session value in dataset

                If HelperFunctions.isNonEmpty(dsSalInfo) Then
                    If dsSalInfo.Tables(1).Rows(0).IsNull("LastSalPaidMonth") = False Then
                        TextBoxRetirementDate.Text = Convert.ToDateTime(Convert.ToDateTime(dsSalInfo.Tables(1).Rows(0).Item("LastSalPaidMonth")).Month & "/01/" & _
                        Convert.ToDateTime(dsSalInfo.Tables(1).Rows(0).Item("LastSalPaidMonth")).Year()).ToString("MM/dd/yyyy")

                        'Age Calculation
                        TextBoxRetirementAge.Text = Convert.ToDecimal(DateDiff(DateInterval.Day, Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime(TextBoxRetirementDate.Text)) / 365).ToString.Substring(0, 2)

                        Me.RetirementDate = TextBoxRetirementDate.Text
                    End If
                End If

                LoadEmploymentDetails(Me.PersonId) 'MMR | 2017.03.14 | YRS-AT-2625 | Added to load employement details 

                'IB:BT:892 YRS 5.0-1359 : Disability Estimate form
                TextBoxPraAssumption.Visible = False
                labelPRAAssumption.Visible = False


            End If
            WarnUserToRecalculate()          ' SB | 03/14/2017 | YRS-AT-2625 | Function to display the recalculate warning estimate message.


            '17-Feb-2011 Added by Sanket to reset the value of Employment drop down in Accounts tab
            If DropDownListEmployment.Items.Count > 1 Then
                DropDownListEmployment.SelectedIndex = 1
            End If

            Dim selectedPlan As String

            selectedPlan = Me.DropDownListPlanType.SelectedValue
            Select Case selectedPlan
                Case "Retirement"
                    'Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                    If DropDownListRetirementType.SelectedValue.ToUpper().Trim = "NORMAL" Then

                        'End If
                        chkSavingPartialAmount.Checked = False
                        chkSavingPartialAmount.Visible = False
                        txtSavingPartialAmount.Text = ""
                        txtSavingPartialAmount.Visible = False
                        'Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                        txtSavingPartialAmount.Enabled = False

                        chkRetirementAccount.Checked = False
                        chkRetirementAccount.Visible = True
                        txtRetirementAccount.Visible = True
                        txtRetirementAccount.Text = ""
                    End If
                    'Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                Case "Savings"

                    'Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                    If DropDownListRetirementType.SelectedValue.ToUpper().Trim = "NORMAL" Then

                        'End If
                        chkSavingPartialAmount.Checked = False
                        chkSavingPartialAmount.Visible = True
                        txtSavingPartialAmount.Text = ""
                        txtSavingPartialAmount.Visible = True
                        'Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                        txtSavingPartialAmount.Enabled = False

                        chkRetirementAccount.Checked = False
                        chkRetirementAccount.Visible = False
                        txtRetirementAccount.Visible = False
                        txtRetirementAccount.Text = ""
                    End If
                    'Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals

                Case "Both"

                    'Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                    If DropDownListRetirementType.SelectedValue.ToUpper().Trim = "NORMAL" Then

                        'End If
                        chkSavingPartialAmount.Checked = False
                        chkSavingPartialAmount.Visible = True
                        txtSavingPartialAmount.Text = ""
                        txtSavingPartialAmount.Visible = True
                        'Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                        txtSavingPartialAmount.Enabled = False

                        chkRetirementAccount.Checked = False
                        chkRetirementAccount.Visible = True
                        txtRetirementAccount.Visible = True
                        txtRetirementAccount.Text = ""
                    End If
                    'Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals

            End Select

            LabelPartialWithdrawal.Text = ""
            lblRetirementPartialAmountEligible.Text = ""
            lblSavingPartialAmountEligible.Text = ""
            'START: PPP | 11/17/2017 | YRS-AT-3328 | Handling exception
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-DropDownListRetirementType_SelectedIndexChanged", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception
            Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
        'END: PPP | 11/17/2017 | YRS-AT-3328 | Handling exception
    End Sub
    'Private Sub PopcalendarDate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PopcalendarDate.SelectionChanged
    '    LabelEstimateDataChangedMessage.Visible = True
    'End Sub
    'Added by Ashish 15-Apr-2009 For Phase V Changes,End

#End Region

    'Private Function UpdateEmployeeInformation()
    '    Dim dsEmpRetInfo As DataSet
    '    Dim dtEmpModifedSalInfo As DataTable
    '    Dim drFoundRows As DataRow()

    '    Try
    '        If (Not Session("dsPersonEmploymentDetails") Is Nothing) And (Not Session("employmentSalaryInformation") Is Nothing) Then
    '            dsEmpRetInfo = DirectCast(Session("dsPersonEmploymentDetails"), DataSet)
    '            dtEmpModifedSalInfo = DirectCast(Session("employmentSalaryInformation"), DataTable)
    '            For Each drRetEmpRow As DataRow In dsEmpRetInfo.Tables(0).Rows
    '                drFoundRows = dtEmpModifedSalInfo.Select("EmpEventID = '" & drRetEmpRow("guiEmpEventId").ToString().ToUpper() & "'")
    '                If drFoundRows.Length > 0 Then
    '                    If drFoundRows(0)("FutureSalary").ToString() <> String.Empty Then
    '                        drRetEmpRow("numFutureSalary") = Convert.ToDecimal(drFoundRows(0)("FutureSalary"))
    '                    Else
    '                        drRetEmpRow("numFutureSalary") = "0.00"
    '                    End If
    '                    If drFoundRows(0)("FutureSalaryEffDate").ToString() <> String.Empty Then
    '                        drRetEmpRow("dtmFutureSalaryDate") = Convert.ToDateTime(drFoundRows(0)("FutureSalaryEffDate")).ToString("MM/dd/yyyy")
    '                        'Else
    '                        '    drRetEmpRow("dtmFutureSalaryDate") = String.Empty
    '                    End If
    '                    If drFoundRows(0)("AnnualSalaryIncreaseEffDate").ToString() <> String.Empty Then
    '                        drRetEmpRow("dtmAnnualSalaryIncreaseEffDate") = Convert.ToDateTime(drFoundRows(0)("AnnualSalaryIncreaseEffDate")).ToString("MM/dd/yyyy")
    '                        'Else
    '                        '    drRetEmpRow("dtmAnnualSalaryIncreaseEffDate") = String.Empty
    '                    End If
    '                    If drFoundRows(0)("AnnualSalaryIncrease").ToString() <> String.Empty Then
    '                        drRetEmpRow("numAnnualPctgIncrease") = Convert.ToDecimal(drFoundRows(0)("AnnualSalaryIncrease"))
    '                    Else
    '                        drRetEmpRow("numAnnualPctgIncrease") = "0.00"
    '                    End If


    '                End If
    '            Next
    '            dsEmpRetInfo.AcceptChanges()
    '            Session("dsPersonEmploymentDetails") = dsEmpRetInfo
    '        End If

    '    Catch ex As Exception

    '    End Try
    'End Function
    Private Function DisplayEmploymentGridWithUpdatedTermDate()
        Dim dtEmploymentSalaryInformation As DataTable
        Dim drEmploymentRecord As DataRow()
        Dim lblTermDate As Label = Nothing
        Try
            If Session("employmentSalaryInformation") Is Nothing AndAlso DataGridEmployment Is Nothing Then
                Exit Function
            End If
            dtEmploymentSalaryInformation = CType(Session("employmentSalaryInformation"), DataTable)
            If dtEmploymentSalaryInformation.Rows.Count > 0 AndAlso DataGridEmployment.Items.Count > 0 Then
                For Each dgRow As DataGridItem In DataGridEmployment.Items
                    drEmploymentRecord = dtEmploymentSalaryInformation.Select("EmpEventID='" + dgRow.Cells(6).Text.ToUpper() + "'")
                    If drEmploymentRecord.Length > 0 Then
                        lblTermDate = Nothing
                        lblTermDate = CType(dgRow.Cells(7).FindControl("lblTempTerminationDate"), Label)
                        If Not lblTermDate Is Nothing Then
                            If drEmploymentRecord(0)("EndWorkDate").ToString() <> String.Empty Then
                                lblTermDate.Text = Convert.ToDateTime(drEmploymentRecord(0)("EndWorkDate")).ToString("MM/dd/yyyy")
                            Else
                                lblTermDate.Text = String.Empty
                            End If
                        End If
                    End If
                Next

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'BT:892-YRS 5.0-1359 : Disability Estimate form

    '2012.05.21 SP - BT-976/YRS 5.0-1507 - Reopned issue -Start
    Private Sub DeselectedAnnuityBeneficiaryGrid()
        Try

            If (Me.DataGridAnnuityBeneficiaries.Visible) Then
                Dim l_button_select As ImageButton
                Dim i As Integer
                DataGridAnnuityBeneficiaries.SelectedIndex = -1
                While i < Me.DataGridAnnuityBeneficiaries.Items.Count
                    l_button_select = New ImageButton
                    l_button_select = Me.DataGridAnnuityBeneficiaries.Items(i).FindControl("ImageButtonAccounts")
                    If Not l_button_select Is Nothing Then
                        l_button_select.ImageUrl = "images\select.gif"
                    End If

                    i = i + 1
                End While

            End If
        Catch ex As Exception
            Throw
        End Try


    End Sub
    '2012.05.21 SP - BT-976/YRS 5.0-1507 - Reopned issue - End

    Private Sub btnPRADisability_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPRADisability.Click
        Dim strCommandName As String = String.Empty
        Try

            ' START : SB | 03/08/2017 | YRS-AT-2625 | checking if the participant is eligible for annuity purchase if not then do not allow to calculate the estimation
            IsPersonalWithdrawalExists()
            If (Me.HasSatisfiedPaidService = False And Me.PersonalWithdrawalExists = True) Then
                Exit Sub
            End If
            ' END : SB | 03/08/2017 | YRS-AT-2625 | checking if the participant is eligible for annuity purchase if not then do not allow to calculate the estimation

            If TextBoxPraAssumption.Text.Length > 1000 Then
                'commented by Anudeep on 22-sep for BT-1126
                'ShowCustomMessage("The number of characters entered have exceeded the maximum limit ", enumMessageBoxType.DotNet, MessageBoxButtons.OK)
                ShowCustomMessage(getmessage("MESSAGE_RETIREMENT_ESTIMATE_CHARACTERS_EXCEEDED"), enumMessageBoxType.DotNet, MessageBoxButtons.OK)
                Exit Sub
            Else
                'Commented by Ashish for Phase V changes
                'PrintEstimate()
                'Added By Ashish for phase V changes ,Start
                Page.Validate()
                If Page.IsValid() Then
                    'Ashish:2010.06.21 YRS 5.0-1115
                    strCommandName = CType(sender, Button).CommandName

                    Session("RE_PRAType") = strCommandName
                    PrintEstimate(strCommandName)
                    'Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request
                    tblIDMcheck.Visible = False
                    LabelEstimateDataChangedMessage.Visible = False
                End If
                'Added By Ashish for phase V changes ,End
            End If
        Catch ex As Exception

            HelperFunctions.LogException("RetirementEstimateWebForm -- btnPRADisability_click ", ex)

            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,start
            Dim exmsg As String

            If (ex.Message.ToString().Contains("MESSAGE")) Then
                exmsg = getmessage(ex.Message.ToString())
            Else
                exmsg = ex.Message.Trim.ToString()
            End If
            '   Added  Anudeep:2012-09-22 for BT-1126/Additional change YRS 5.0-1246,End
            g_String_Exception_Message = Server.UrlEncode(exmsg)
            Server.Transfer("ErrorPageForm.aspx?Message=" + g_String_Exception_Message, False)
        End Try

    End Sub

    Private Sub DataGridAnnuityBeneficiaries_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridAnnuityBeneficiaries.SelectedIndexChanged
        'Start:Commented by Anudeep:12.02.2013-Bt-1688: YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
        'Dim i As Integer
        'Dim retirementType As String
        'Dim l_button_select As ImageButton
        'Dim l_dataset_RelationShips As DataSet
        'Dim l_datarow_RelationShips As DataRow()
        'End:Commented by Anudeep:12.02.2013-Bt-1688: YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
        '2012.05.21 SP : BT-976/YRS 5.0-1507 - Reopned issue
        Dim drrow As DataRow()
        Try
            Session("ProceedWithJSOptionEstimationQDRO") = "ChangeBeneficiaries" 'Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor

            drrow = CType(Session("dsParticipantBeneficiaries"), DataSet).Tables(0).Select("guiUniqueID ='" + Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(1).Text.Trim + "'")
            'Changed by Anudeep:12.02.2013 to put repeated code in a method -Bt-1688: YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
            showselectedbeneficary(drrow(0))
            'Start:Commented by Anudeep:12.02.2013-Bt-1688: YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
            'While i < Me.DataGridAnnuityBeneficiaries.Items.Count
            '    l_button_select = New ImageButton
            '    l_button_select = Me.DataGridAnnuityBeneficiaries.Items(i).FindControl("ImageButtonAccounts")
            '    If i = Me.DataGridAnnuityBeneficiaries.SelectedIndex Then
            '        If Not l_button_select Is Nothing Then
            '            l_button_select.ImageUrl = "images\selected.gif"
            '        End If
            '    Else
            '        If Not l_button_select Is Nothing Then
            '            l_button_select.ImageUrl = "images\select.gif"
            '        End If

            '    End If
            '    i = i + 1
            'End While
            'Me.AddSelectedParticipantBeneficiary(drrow(0))

            'If Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(3).Text.Trim.ToUpper() <> "&NBSP;" Then
            '    TextboxFirstName.Text = Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(3).Text
            'Else
            '    TextboxFirstName.Text = String.Empty
            'End If
            'If Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(4).Text.Trim.ToUpper() <> "&NBSP;" Then
            '    TextboxLastName.Text = Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(4).Text
            'Else
            '    TextboxLastName.Text = String.Empty
            'End If
            'If Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(5).Text.Trim.ToUpper() <> "&NBSP;" Then
            '    TextboxBeneficiaryBirthDate.Text = Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(5).Text
            'Else
            '    TextboxBeneficiaryBirthDate.Text = String.Empty
            'End If
            'If Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(6).Text.Trim.ToUpper() <> "&NBSP;" Then

            '    l_dataset_RelationShips = Session("dataset_RelationShips")
            '    If Not l_dataset_RelationShips Is Nothing Then
            '        l_datarow_RelationShips = l_dataset_RelationShips.Tables(0).Select("(Active = True Or Codevalue = '" + Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(6).Text.Trim() + "') And CodeValue not in('ES','IN','TR')")
            '        If l_datarow_RelationShips.Length > 0 Then
            '            DropDownRelationShip.DataSource = Nothing
            '            DropDownRelationShip.DataSource = l_datarow_RelationShips.CopyToDataTable()
            '            DropDownRelationShip.DataTextField = "Description"
            '            DropDownRelationShip.DataValueField = "CodeValue"
            '            DropDownRelationShip.DataBind()
            '            DropDownRelationShip.Items.Insert(0, "Select")
            '        End If
            '    End If

            '    DropDownRelationShip.SelectedValue = Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(6).Text.Trim()

            'End If
            'End:Commented by Anudeep:12.02.2013-Bt-1688: YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
            '2012.05.21 SP - BT-976/YRS 5.0-1507 - Reopned issue
            LabelEstimateDataChangedMessage.Visible = True
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-DataGridAnnuityBeneficiaries_SelectedIndexChanged", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception
            Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    '2012.05.21 SP - BT-976/YRS 5.0-1507 - Reopned issue -Start
    Private Sub DropDownRelationShip_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownRelationShip.SelectedIndexChanged
        Try
            If (DropDownRelationShip.SelectedIndex) Then
                DeselectedAnnuityBeneficiaryGrid()
                LabelEstimateDataChangedMessage.Visible = True
            End If
            'Added Anudeep:16.10.2012 For BT-1238: YRS 5.0-1541:Estimates calculator ,start
            If DropDownRelationShip.SelectedValue = "SP" Then
                Session("ProceedWithJSOptionEstimationQDRO") = "ChangeBeneficiaries" 'Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
                Me.AreJAnnuitiesUnavailable = False
            Else
                Me.AreJAnnuitiesUnavailable = True
            End If
            'Added Anudeep:16.10.2012 For BT-1238: YRS 5.0-1541:Estimates calculator ,End
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-DropDownRelationShip_SelectedIndexChanged", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub ButtonClearBeneficiary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClearBeneficiary.Click
        Try
            DeselectedAnnuityBeneficiaryGrid()
            LabelEstimateDataChangedMessage.Visible = True
            TextboxLastName.Text = String.Empty
            TextboxFirstName.Text = String.Empty
            TextboxBeneficiaryBirthDate.Text = String.Empty
            DropDownRelationShip.SelectedIndex = -1
        Catch ex As Exception
            HelperFunctions.LogException("RetirementEstimateWebForm-ButtonClearBeneficiary_Click", ex) 'PPP | 11/17/2017 | YRS-AT-3328 | Logging exception

            Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    '2012.09.21 added by anudeep :start
    '2012.09.21 added by anudeep :start
    'gets the message from resource file
    Public Function getmessage(ByVal resourcemessage As String)
        Try

            Dim strMessage As String
            Try
                strMessage = GetGlobalResourceObject("RetirementMessages", resourcemessage).ToString()
            Catch ex As Exception
                strMessage = resourcemessage
            End Try
            Return strMessage
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Combines the two or more messages if there are any group messages
    Public Function combinemessage(ByVal resourcemessages As String)
        Try

            Dim strMessage As String
            Dim i As Integer
            Dim strMessages() As String
            strMessages = resourcemessages.Split(",")
            For i = 0 To UBound(strMessages)
                strMessage += getmessage(strMessages(i))
            Next i
            Return strMessage
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    '2012.09.21 added by anudeep :end

    'Start Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request
    'To set IDM Properties and copy report to IDM
    Private Function CopyToIDM(ByVal l_StringDocType As String, ByVal l_StringReportName As String, ByVal l_ArrListParamValues As ArrayList, ByVal l_string_OutputFileType As String) As String
        Dim l_StringErrorMessage As String = String.Empty
        Dim IDM As New IDMforAll
        Try
            ' Anudeep 04.12.2012 Code changes to copy report into IDM folder
            'gets the columns for idm and stored in session varilable 
            If IDM.DatatableFileList(False) Then
                Session("FTFileList") = IDM.SetdtFileList
            End If
            If Not Session("FTFileList") Is Nothing Then
                IDM.SetdtFileList = DirectCast(Session("FTFileList"), DataTable)
            End If
            'Assign values to the properties
            IDM.PreviewReport = True
            IDM.LogonToDb = True
            IDM.CreatePDF = True
            IDM.CreateIDX = True
            IDM.CopyFilesToIDM = True
            IDM.AppType = "P"

            If Not Session("PersId") Is Nothing Then
                IDM.PersId = DirectCast(Session("PersId"), String)
            End If

            IDM.DocTypeCode = l_StringDocType
            IDM.OutputFileType = l_string_OutputFileType
            IDM.ReportName = l_StringReportName.ToString().Trim & ".rpt"
            IDM.ReportParameters = l_ArrListParamValues

            l_StringErrorMessage = IDM.ExportToPDF()
            l_ArrListParamValues.Clear()

            Session("FTFileList") = IDM.SetdtFileList
            'Start: Anudeep 04.12.2012 Code changes to copy report into IDM folder
            If Not Session("FTFileList") Is Nothing Then
                Try
                    ' Call the calling of the ASPX to copy the file.
                    Dim popupScriptCopytoServer As String = "<script language='javascript'>" & _
                    "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                    "</script>"
                    If (Not Me.IsStartupScriptRegistered("PopupCopytoServer")) Then
                        Page.RegisterStartupScript("PopupCopytoServer", popupScriptCopytoServer)
                    End If

                Catch
                    Throw
                End Try
            End If
            'End: Anudeep 04.12.2012 Code changes to copy report into IDM folder
            Return l_StringErrorMessage
        Catch
            Throw
        Finally
            IDM = Nothing
        End Try
    End Function
    'End Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request

    'START Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
#Region "Annuitiy Estimate for partial withdrawal"

    Private Sub chkRetirementAccount_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRetirementAccount.CheckedChanged

        If chkRetirementAccount.Checked Then
            For iCount As Integer = 0 To DatagridElectiveRetirementAccounts.Items.Count - 1
                Dim chkbox As CheckBox
                chkbox = CType(DatagridElectiveRetirementAccounts.Items(iCount).FindControl("CheckboxRet"), CheckBox)
                'chkbox.Enabled = False
                txtRetirementAccount.Visible = True
                txtRetirementAccount.Enabled = True
                txtRetirementAccount.Text = ""
            Next
            lblRetirementPartialAmountEligible.Text = ""
            lblRetirementPartialAmountEligible.Visible = False
        Else
            'Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
            'ResetRetireePartialGrid()
            LabelEstimateDataChangedMessage.Visible = True
            LabelEstimateDataChangedMessage.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_DATA_HAS_CHANGED")
            WarnUserToRecalculate()          ' SB | 03/14/2017 | YRS-AT-2625 | Function to display the warning estimate message or not after checking for withdrawals taken or not
            ' LabelEstimateDataChangedMessage.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_DATA_HAS_CHANGED")   ' SB | 03/14/2017 | YRS-AT-2625 | Function to display the warning estimate message or not after checking for withdrawals taken or not
            txtRetirementAccount.Enabled = False
            txtRetirementAccount.Text = ""
            lblRetirementPartialAmountEligible.Text = ""
            lblRetirementPartialAmountEligible.Visible = False
        End If
    End Sub

    Private Sub txtRetirementAccount_OnTextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRetirementAccount.TextChanged
        Dim selectedPlan As String = Me.DropDownListPlanType.SelectedValue.ToUpper()
        Dim l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT As DataTable
        Dim decMinPartialWithdrawalLimit As Decimal
        Dim strErrorMessage As String = String.Empty
        Try
            LabelPartialWithdrawal.Text = ""
            LabelEstimateDataChangedMessage.Text = ""
            lblRetirementPartialAmountEligible.Text = ""
            lblRetirementPartialAmountEligible.Visible = False
            LabelEstimateDataChangedMessage.Visible = True
            If txtRetirementAccount.Visible = True And txtRetirementAccount.Enabled = True Then
                If txtRetirementAccount.Text.Trim <> "" Then

                    WarnUserToRecalculate()          ' SB | 03/14/2017 | YRS-AT-2625 | Function to display the warning estimate message or not after checking for withdrawals taken or not
                    ' LabelEstimateDataChangedMessage.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_DATA_HAS_CHANGED")       ' SB | 03/14/2017 | YRS-AT-2625 | Function to display the warning estimate message or not after checking for withdrawals taken or not

                    If Not Regex.IsMatch(txtRetirementAccount.Text.Trim, RetirementBOClass.REG_EX_CURRENCY) And txtRetirementAccount.Text.Trim <> String.Empty Then
                        LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREMENT_PARTIAL_CURRENCY_FORMAT")
                        LabelPartialWithdrawal.Visible = True
                        LabelEstimateDataChangedMessage.Visible = False
                        Exit Sub
                    End If

                Else
                    LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREMENT_PARTIAL_CURRENCY_FORMAT")
                    LabelPartialWithdrawal.Visible = True
                    LabelEstimateDataChangedMessage.Visible = False
                    Exit Sub
                End If
            End If

            If PlanType = "R" Or PlanType = "B" Then
                If selectedPlan = "BOTH" Or selectedPlan = "RETIREMENT" Then
                    If txtRetirementAccount.Text.Trim <> "" Then
                        l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("MIN_PARTIAL_WITHDRAWAL_LIMIT")

                        If Not l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT Is Nothing Then
                            If l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT.Rows(0)("Key").ToString.Trim.ToUpper() = "MIN_PARTIAL_WITHDRAWAL_LIMIT" Then
                                decMinPartialWithdrawalLimit = CType(l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT.Rows(0)("Value").ToString, Decimal)
                            End If
                        End If

                        If Convert.ToDecimal(txtRetirementAccount.Text) < decMinPartialWithdrawalLimit Then
                            strErrorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREMENT_MIN_PARTIAL_LIMIT").ToString().Replace("$$MIN_PARTIAL_WITHDRAWAL_LIMIT$$", decMinPartialWithdrawalLimit.ToString())
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strErrorMessage, MessageBoxButtons.Stop)
                            Exit Sub
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub ResetRetireePartialGrid()
        Dim l_SelectedProjTotal As Decimal = 0
        Dim l_SelectedAcctTotal As Decimal = 0
        Dim l_dtGroupedElectiveAcct As DataTable
        Dim l_strSelectedProjTotal As String = String.Empty
        Dim filterExpression As String = "(YMCATotal > 0 OR PersonalTotal > 0 )"
        Dim l_drGroupAcctFoundRow As DataRow()
        Dim dvRet As DataView

        Try


            filterExpression = "((YMCATotal > 0 OR PersonalTotal > 0) OR bitRet_Voluntary = 1 OR bitFutureAcctVisible=1)"
            dsElectiveAccountsDet = GetElectiveAccoutsByPlan(Me.FundEventId, Me.PlanType, Me.RetirementDate)
            'dvRet = New DataView(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts"), "PlanType='RETIREMENT' AND " & filterExpression, "intSortOrder asc, chrAcctType", DataViewRowState.CurrentRows)
            If Not dsSessionStoreDataSet Is Nothing Then
                If dsSessionStoreDataSet.Tables.Count > 0 Then
                    If dsSessionStoreDataSet.Tables("RetireeGroupedElectiveAccounts").Rows.Count > 0 Then
                        dvRet = New DataView(dsSessionStoreDataSet.Tables("RetireeGroupedElectiveAccounts"), "PlanType='RETIREMENT' AND " & filterExpression, "intSortOrder asc, chrAcctType", DataViewRowState.CurrentRows)

                        l_SelectedProjTotal = 0
                        l_SelectedAcctTotal = 0
                        l_drGroupAcctFoundRow = dsSessionStoreDataSet.Tables("RetireeGroupedElectiveAccounts").Select("PlanType='RETIREMENT' AND " & filterExpression & " AND Selected=True")

                        If l_drGroupAcctFoundRow.Length > 0 Then
                            l_SelectedProjTotal = Convert.ToDecimal(dsSessionStoreDataSet.Tables("RetireeGroupedElectiveAccounts").Compute("SUM(mnyProjectedBalance)", "PlanType='RETIREMENT' AND Selected=True"))
                        End If


                    Else
                        dvRet = New DataView(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts"), "PlanType='RETIREMENT' AND " & filterExpression, "intSortOrder asc, chrAcctType", DataViewRowState.CurrentRows)
                        l_SelectedProjTotal = 0
                        l_SelectedAcctTotal = 0
                        l_drGroupAcctFoundRow = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select("PlanType='RETIREMENT' AND " & filterExpression & " AND Selected=True")

                        If l_drGroupAcctFoundRow.Length > 0 Then
                            l_SelectedProjTotal = Convert.ToDecimal(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Compute("SUM(mnyProjectedBalance)", "PlanType='RETIREMENT' AND Selected=True"))
                        End If

                    End If
                End If
            Else
                dvRet = New DataView(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts"), "PlanType='RETIREMENT' AND " & filterExpression, "intSortOrder asc, chrAcctType", DataViewRowState.CurrentRows)
                l_SelectedProjTotal = 0
                l_SelectedAcctTotal = 0
                l_drGroupAcctFoundRow = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select("PlanType='RETIREMENT' AND " & filterExpression & " AND Selected=True")

                If l_drGroupAcctFoundRow.Length > 0 Then
                    l_SelectedProjTotal = Convert.ToDecimal(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Compute("SUM(mnyProjectedBalance)", "PlanType='RETIREMENT' AND Selected=True"))
                End If

            End If


            Session("dsElectiveAccountsDet") = dsElectiveAccountsDet

            'chkRetirementAccount.Checked = False
            DatagridElectiveRetirementAccounts.AutoGenerateColumns = False
            DatagridElectiveRetirementAccounts.DataSource = dvRet
            DatagridElectiveRetirementAccounts.DataBind()


            Dim l_chkSelect As CheckBox
            If Not DatagridElectiveRetirementAccounts Is Nothing Then
                If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
                    For Each dgRetireRow As DataGridItem In Me.DatagridElectiveRetirementAccounts.Items
                        l_chkSelect = TryCast(dgRetireRow.Cells(RET_SEL_ACCT_CHK).FindControl("CheckboxRet"), CheckBox)
                        If Not l_chkSelect Is Nothing Then
                            If Convert.ToDecimal(dgRetireRow.Cells(RET_ACCT_TOTAL).Text.Trim()) > 0 Then
                                l_chkSelect.Checked = True
                            Else
                                If Convert.ToBoolean(dgRetireRow.Cells(RET_BASIC_ACCT).Text.Trim()) = True Then
                                    l_chkSelect.Checked = True
                                End If
                            End If
                        End If
                    Next
                End If
            End If


            'l_SelectedProjTotal = 0
            'l_SelectedAcctTotal = 0
            'l_drGroupAcctFoundRow = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select("PlanType='RETIREMENT' AND " & filterExpression & " AND Selected=True")

            'If l_drGroupAcctFoundRow.Length > 0 Then
            '    l_SelectedProjTotal = Convert.ToDecimal(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Compute("SUM(mnyProjectedBalance)", "PlanType='RETIREMENT' AND Selected=True"))
            'End If

            Dim lblProjectedTotalBal As Label
            If Not DatagridElectiveRetirementAccounts Is Nothing Then
                If DatagridElectiveRetirementAccounts.Items.Count > 0 Then
                    lblProjectedTotalBal = CType(CType(DatagridElectiveRetirementAccounts.Controls(0).Controls(DatagridElectiveRetirementAccounts.Items.Count + 1).Controls(RET_PROJ_BALANCE), TableCell).FindControl("LabelProjectedTotalBalRet"), Label)
                    If Not lblProjectedTotalBal Is Nothing Then
                        lblProjectedTotalBal.Text = String.Format("{0:0.00}", l_SelectedProjTotal)
                    End If
                End If
            End If

            setAccountGridControls()
        Catch ex As Exception
            Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub

    Private Sub chkSavingPartialAmount_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSavingPartialAmount.CheckedChanged

        If chkSavingPartialAmount.Checked Then
            For iCount As Integer = 0 To DatagridElectiveSavingsAccounts.Items.Count - 1
                Dim chkbox As CheckBox
                chkbox = CType(DatagridElectiveSavingsAccounts.Items(iCount).FindControl("CheckboxSav"), CheckBox)
                'chkbox.Enabled = False
                txtSavingPartialAmount.Visible = True
                txtSavingPartialAmount.Enabled = True
                txtSavingPartialAmount.Text = ""
            Next
            lblSavingPartialAmountEligible.Text = ""
            lblSavingPartialAmountEligible.Visible = False
        Else
            'Dinesh Kanojia(DK)     2013.10.10      BT-943: YRS 5.0-1443:Include partial withdrawals - Re-Opened
            'ResetPartialSavingGrid()
            LabelEstimateDataChangedMessage.Visible = True
            WarnUserToRecalculate() ' SB | 03/14/2017 | YRS-AT-2625 | Function to display the warning estimate message or not after checking for withdrawals taken or not
            ' LabelEstimateDataChangedMessage.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_DATA_HAS_CHANGED")           ' SB | 03/14/2017 | YRS-AT-2625 | Function to display the warning estimate message or not after checking for withdrawals taken or not

            txtSavingPartialAmount.Enabled = False
            txtSavingPartialAmount.Text = ""
            lblSavingPartialAmountEligible.Text = ""
            lblSavingPartialAmountEligible.Visible = False
        End If

    End Sub

    Private Sub txtSavingPartialAmount_OnTextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSavingPartialAmount.TextChanged
        Dim selectedPlan As String = Me.DropDownListPlanType.SelectedValue.ToUpper()
        Dim l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT As DataTable
        Dim decMinPartialWithdrawalLimit As Decimal
        Dim lblProjectedTotalBalRet As Label
        Dim strErrorMessage As String = String.Empty

        Try
            LabelPartialWithdrawal.Text = ""
            lblSavingPartialAmountEligible.Text = ""
            LabelEstimateDataChangedMessage.Visible = True
            lblSavingPartialAmountEligible.Visible = False
            If txtSavingPartialAmount.Visible = True And txtSavingPartialAmount.Enabled = True Then
                If txtSavingPartialAmount.Text.Trim <> "" Then
                    WarnUserToRecalculate() ' SB | 03/14/2017 | YRS-AT-2625 | Function to display the warning estimate message or not after checking for withdrawals taken or not
                    ' LabelEstimateDataChangedMessage.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_DATA_HAS_CHANGED")        ' SB | 03/14/2017 | YRS-AT-2625 | Function to display the warning estimate message or not after checking for withdrawals taken or not

                    If Not Regex.IsMatch(txtSavingPartialAmount.Text.Trim, RetirementBOClass.REG_EX_CURRENCY) And txtSavingPartialAmount.Text.Trim <> String.Empty Then
                        LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_SAVING_PARTIAL_CURRENCY_FORMAT")
                        LabelPartialWithdrawal.Visible = True
                        LabelEstimateDataChangedMessage.Visible = False
                        Exit Sub
                    End If
                Else
                    LabelPartialWithdrawal.Text = "<br> " + getmessage("MESSAGE_RETIREMENT_ESTIMATE_SAVING_PARTIAL_CURRENCY_FORMAT")
                    LabelPartialWithdrawal.Visible = True
                    LabelEstimateDataChangedMessage.Visible = False
                    Exit Sub
                End If
            End If

            If PlanType = "S" Or PlanType = "B" Then
                If selectedPlan = "BOTH" Or selectedPlan = "SAVINGS" Then
                    If txtSavingPartialAmount.Text.Trim <> "" Then
                        l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("MIN_PARTIAL_WITHDRAWAL_LIMIT")
                        If Not l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT Is Nothing Then
                            If l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT.Rows(0)("Key").ToString.Trim.ToUpper() = "MIN_PARTIAL_WITHDRAWAL_LIMIT" Then
                                decMinPartialWithdrawalLimit = CType(l_DataTable_MIN_PARTIAL_WITHDRAWAL_LIMIT.Rows(0)("Value").ToString, Decimal)
                            End If
                        End If

                        If Convert.ToDecimal(txtSavingPartialAmount.Text) < decMinPartialWithdrawalLimit Then
                            strErrorMessage = getmessage("MESSAGE_RETIREMENT_ESTIMATE_RETIREMENT_MIN_PARTIAL_LIMIT").ToString().Replace("$$MIN_PARTIAL_WITHDRAWAL_LIMIT$$", decMinPartialWithdrawalLimit.ToString())
                            MessageBox.Show(PlaceHolder1, "YMCA-YRS", strErrorMessage, MessageBoxButtons.Stop)
                            Exit Sub
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub ResetPartialSavingGrid()
        Dim l_SelectedProjTotal As Decimal = 0
        Dim l_SelectedAcctTotal As Decimal = 0
        Dim l_dtGroupedElectiveAcct As DataTable
        Dim l_strSelectedProjTotal As String = String.Empty
        Dim l_drGroupAcctFoundRow As DataRow()
        Dim filterExpression As String = "(YMCATotal > 0 OR PersonalTotal > 0 )"
        Dim dvSav As DataView
        Dim bDefaultCheck As Boolean = False
        Try

            'filterExpression = "((YMCATotal > 0 OR PersonalTotal > 0) OR bitRet_Voluntary = 1 OR bitFutureAcctVisible=1)"
            dsElectiveAccountsDet = GetElectiveAccoutsByPlan(Me.FundEventId, Me.PlanType, Me.RetirementDate)

            dvSav = New DataView(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts"), "PlanType = 'SAVINGS' ", "chrAcctType", DataViewRowState.CurrentRows) 'AND " & filterExpression

            If Not dsSessionStoreDataSet Is Nothing Then
                If dsSessionStoreDataSet.Tables.Count > 0 Then
                    If dsSessionStoreDataSet.Tables("RetireeGroupedElectiveAccounts").Rows.Count > 0 Then
                        dvSav = New DataView(dsSessionStoreDataSet.Tables("RetireeGroupedElectiveAccounts"), "PlanType = 'SAVINGS'", "chrAcctType", DataViewRowState.CurrentRows) ' AND " & filterExpression
                        bDefaultCheck = True
                        l_SelectedProjTotal = 0
                        l_SelectedAcctTotal = 0
                        l_drGroupAcctFoundRow = dsSessionStoreDataSet.Tables("RetireeGroupedElectiveAccounts").Select("PlanType='SAVINGS' AND Selected=True ")
                        If l_drGroupAcctFoundRow.Length > 0 Then
                            l_SelectedProjTotal = Convert.ToDecimal(dsSessionStoreDataSet.Tables("RetireeGroupedElectiveAccounts").Compute("SUM(mnyProjectedBalance)", "PlanType='SAVINGS' AND Selected=True"))
                        End If
                    Else
                        dvSav = New DataView(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts"), "PlanType = 'SAVINGS' ", "chrAcctType", DataViewRowState.CurrentRows) 'AND " & filterExpression
                        l_SelectedProjTotal = 0
                        l_SelectedAcctTotal = 0
                        l_drGroupAcctFoundRow = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select("PlanType='SAVINGS' AND Selected=True ")
                        If l_drGroupAcctFoundRow.Length > 0 Then
                            l_SelectedProjTotal = Convert.ToDecimal(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Compute("SUM(mnyProjectedBalance)", "PlanType='SAVINGS' AND Selected=True"))
                        End If
                    End If
                End If
            Else
                dvSav = New DataView(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts"), "PlanType = 'SAVINGS' AND " & filterExpression, "chrAcctType", DataViewRowState.CurrentRows)
                l_SelectedProjTotal = 0
                l_SelectedAcctTotal = 0
                l_drGroupAcctFoundRow = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select("PlanType='SAVINGS' AND Selected=True ")
                If l_drGroupAcctFoundRow.Length > 0 Then
                    l_SelectedProjTotal = Convert.ToDecimal(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Compute("SUM(mnyProjectedBalance)", "PlanType='SAVINGS' AND Selected=True"))
                End If
            End If


            Session("dsElectiveAccountsDet") = dsElectiveAccountsDet

            'chkRetirementAccount.Checked = False
            DatagridElectiveSavingsAccounts.AutoGenerateColumns = False
            DatagridElectiveSavingsAccounts.DataSource = dvSav
            DatagridElectiveSavingsAccounts.DataBind()






            Dim l_chkSelect As CheckBox
            Dim l_LegacyAcctType As String = String.Empty
            If Not DatagridElectiveSavingsAccounts Is Nothing Then
                If DatagridElectiveSavingsAccounts.Items.Count > 0 Then
                    For Each dgSavingRow As DataGridItem In Me.DatagridElectiveSavingsAccounts.Items
                        l_chkSelect = TryCast(dgSavingRow.Cells(SAV_SEL_ACCT_CHK).FindControl("CheckboxSav"), CheckBox)
                        l_LegacyAcctType = dgSavingRow.Cells(SAV_LEGACY_ACCT_TYPE).Text.Trim()
                        If Not l_chkSelect Is Nothing Then
                            If Convert.ToDecimal(dgSavingRow.Cells(SAV_ACCT_TOTAL).Text.Trim()) > 0 Then
                                l_chkSelect.Checked = True
                            End If



                            'Check Default CheckBox
                            If bDefaultCheck Then
                                Dim dtSavings As DataTable
                                dtSavings = dvSav.ToTable
                                If dtSavings.Rows.Count > 0 Then
                                    For iCount As Integer = 0 To dtSavings.Rows.Count - 1
                                        If Not Session("chrAcctType") Is Nothing Then
                                            If Convert.ToBoolean(dtSavings.Rows(iCount)("Selected").ToString().Trim) And l_LegacyAcctType.Trim.ToUpper = Session("chrAcctType").ToString.Trim.ToUpper Then
                                                l_chkSelect.Checked = True
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        End If
                        'Start: Dinesh Kanojia         2015.04.27      BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
                        If dgSavingRow.Cells(1).Text.Trim().ToLower = "ln" Or dgSavingRow.Cells(1).Text.Trim().ToLower = "ld" Then
                            l_chkSelect.Enabled = True
                            l_chkSelect.Checked = False
                            l_chkSelect.Enabled = False
                        End If
                        'End: Dinesh Kanojia         2015.04.27      BT:2699: YRS 5.0-2441 : Modifications for 403b Loans
                    Next
                End If
            End If

            'Change for new observation 
            'l_SelectedProjTotal = 0
            'l_SelectedAcctTotal = 0
            'l_drGroupAcctFoundRow = dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Select("PlanType='SAVINGS' AND Selected=True ")
            'If l_drGroupAcctFoundRow.Length > 0 Then
            '    l_SelectedProjTotal = Convert.ToDecimal(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts").Compute("SUM(mnyProjectedBalance)", "PlanType='SAVINGS' AND Selected=True"))
            'End If

            Dim lblProjectedTotalBal As Label
            If Not DatagridElectiveSavingsAccounts Is Nothing Then
                If DatagridElectiveSavingsAccounts.Items.Count > 0 Then
                    lblProjectedTotalBal = CType(CType(DatagridElectiveSavingsAccounts.Controls(0).Controls(DatagridElectiveSavingsAccounts.Items.Count + 1).Controls(SAV_PROJ_BALANCE), TableCell).FindControl("LabelProjectedTotalBalSav"), Label)
                    If Not lblProjectedTotalBal Is Nothing Then
                        lblProjectedTotalBal.Text = String.Format("{0:0.00}", l_SelectedProjTotal)
                    End If
                End If
            End If

            setAccountGridControls()
        Catch ex As Exception
            Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub


#End Region
    'END Dinesh Kanojia(DK)     2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
    'Start: Added by Anudeep A:2013.02.08-Bt-1688: YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0
    Public Function showselectedbeneficary(ByVal drrow As DataRow)
        Dim l_dataset_RelationShips As DataSet
        Dim l_datarow_RelationShips As DataRow()
        Dim i As Integer
        Dim l_button_select As ImageButton
        Try
            While i < Me.DataGridAnnuityBeneficiaries.Items.Count
                l_button_select = New ImageButton
                l_button_select = Me.DataGridAnnuityBeneficiaries.Items(i).FindControl("ImageButtonAccounts")
                If i = Me.DataGridAnnuityBeneficiaries.SelectedIndex Then
                    If Not l_button_select Is Nothing Then
                        l_button_select.ImageUrl = "images\selected.gif"
                    End If
                Else
                    If Not l_button_select Is Nothing Then
                        l_button_select.ImageUrl = "images\select.gif"
                    End If

                End If
                i = i + 1
            End While

            Me.AddSelectedParticipantBeneficiary(drrow)

            If Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(3).Text.Trim.ToUpper() <> "&NBSP;" Then
                TextboxFirstName.Text = Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(3).Text
            Else
                TextboxFirstName.Text = String.Empty
            End If
            If Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(4).Text.Trim.ToUpper() <> "&NBSP;" Then
                TextboxLastName.Text = Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(4).Text
            Else
                TextboxLastName.Text = String.Empty
            End If
            If Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(5).Text.Trim.ToUpper() <> "&NBSP;" Then
                TextboxBeneficiaryBirthDate.Text = Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(5).Text
            Else
                TextboxBeneficiaryBirthDate.Text = String.Empty
            End If
            If Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(6).Text.Trim.ToUpper() <> "&NBSP;" Then

                l_dataset_RelationShips = Session("dataset_RelationShips")
                If Not l_dataset_RelationShips Is Nothing Then
                    l_datarow_RelationShips = l_dataset_RelationShips.Tables(0).Select("(Active = True Or Codevalue = '" + Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(6).Text.Trim() + "') And CodeValue not in('ES','IN','TR')")
                    If l_datarow_RelationShips.Length > 0 Then
                        bindrelationships(l_datarow_RelationShips)
                    End If
                End If

                DropDownRelationShip.SelectedValue = Me.DataGridAnnuityBeneficiaries.SelectedItem.Cells(6).Text.Trim()

            End If
        Catch
            Throw
        End Try
    End Function
    Public Sub bindrelationships(ByVal drrows As DataRow())
        Try
            DropDownRelationShip.DataSource = Nothing
            DropDownRelationShip.DataSource = drrows.CopyToDataTable()
            DropDownRelationShip.DataTextField = "Description"
            DropDownRelationShip.DataValueField = "CodeValue"
            DropDownRelationShip.DataBind()
            DropDownRelationShip.Items.Insert(0, "Select")
        Catch
            Throw
        End Try
    End Sub
    'End: Added by Anudeep A:2013.02.08-Bt-1688: YRS 5.0-1873:Allow relationship values (in atsLookups) with bitActive = 0

    'Start:Added by Chandrasekar.c on 2015.11.19 YRS-AT-2479 :This method is used to find out whether person is active or terminated.
    Private Function CheckPersonTerminated() As Boolean
        Dim dsPersonEmploymentDetails As New DataSet
        Dim drPersonEmploymentDetailsFoundRow As DataRow()
        Dim blnIsPersonTerminated As Boolean = True

        If Not Session("dsPersonEmploymentDetails") Is Nothing Then

            dsPersonEmploymentDetails = DirectCast(Session("dsPersonEmploymentDetails"), DataSet)

            If HelperFunctions.isNonEmpty(dsPersonEmploymentDetails) Then
                'checking if person has any active employment.
                drPersonEmploymentDetailsFoundRow = dsPersonEmploymentDetails.Tables(0).Select("dtmTerminationDate='' OR dtmTerminationDate IS NULL")

                If drPersonEmploymentDetailsFoundRow.Length > 0 Then

                    blnIsPersonTerminated = False

                End If

            End If

        End If

        Return blnIsPersonTerminated

    End Function
    'End:Added by Chandrasekar.c on 2015.11.19 YRS-AT-2479 :This method is used to find out whether person is active or terminated.
    'START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)
    Private Function GetYMCAAcctAndYMCALegacyAcctBalances(l_dtGroupedElectiveAcct As DataTable) As Decimal
        Dim l_LegacyAcctType As String = String.Empty
        Dim l_boolBitBasicAcct As Boolean
        Dim l_drGroupedAcctFoundRow As DataRow()
        Dim chkSelected As CheckBox
        Dim l_PlanType As String = String.Empty
        Dim mnyYmcaAndLegecyBalance As Decimal = 0
        For Each dgItemRow As DataGridItem In DatagridElectiveRetirementAccounts.Items
            l_LegacyAcctType = dgItemRow.Cells(RET_LEGACY_ACCT_TYPE).Text.Trim()
            l_boolBitBasicAcct = Convert.ToBoolean(dgItemRow.Cells(RET_BASIC_ACCT).Text.Trim())
            l_PlanType = dgItemRow.Cells(RET_PLANE_TYPE).Text.Trim().ToUpper()
            chkSelected = CType(dgItemRow.Cells(RET_SEL_ACCT_CHK).FindControl("CheckboxRet"), CheckBox)
            l_drGroupedAcctFoundRow = l_dtGroupedElectiveAcct.Select("bitBasicAcct=" & l_boolBitBasicAcct & " AND PlanType='" & l_PlanType & "' AND chrLegacyAcctType='" & l_LegacyAcctType & "'")
            If chkSelected.Checked And l_drGroupedAcctFoundRow.Length > 0 Then
                Select Case l_drGroupedAcctFoundRow(0)("chrAcctType").ToString.ToUpper().Trim
                    Case "YMCA(LEGACY) ACCOUNT"
                        mnyYmcaAndLegecyBalance += Convert.ToDouble(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance"))
                    Case "YMCA ACCOUNT"
                        mnyYmcaAndLegecyBalance += Convert.ToDouble(l_drGroupedAcctFoundRow(0)("mnyProjectedBalance"))
                End Select
            End If
        Next
        Return mnyYmcaAndLegecyBalance
    End Function
    'END : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates)

    'START: MMR | 2017.02.22 | YRS-AT-2625 | Displaying manual transaction list
    Private Sub LoadManualTransactionForDisability(ByVal fundEventId As String, ByVal retireeType As String, ByVal retirementDate As Date)
        'START: PPP | 2017.05.30 | YRS-AT-2625 | After clicking on calculate if page shows message to change plan from "Both" to "Retirement" then
        '-- code was again loading Manual Transactions and forcing user to select them on next "Calculate" button click
        '-- So to avoid it checking same session set before asking for Plan Type change
        Dim isSinglePlanChangeMessageShown As Boolean = False
        If Not Session("ProceedWithEstimation") Is Nothing AndAlso Session("ProceedWithEstimation") = True Then
            isSinglePlanChangeMessageShown = True
        End If

        If Not isSinglePlanChangeMessageShown Then
            'END: PPP | 2017.05.30 | YRS-AT-2625 | After clicking on calculate if page shows message to change plan from "Both" to "Retirement" then
            Dim transactionList As New DataSet
            ShowHideManualTransactionLink(False)
            ShowHideMessageManualTransaction(False)
            If retireeType = "DISABL" AndAlso DropDownListPlanType.SelectedValue.ToUpper() <> "SAVINGS" Then 'MMR | 2017.02.22 | YRS-AT-2625 | Added validation dialog box should not open for savings plan type
                transactionList = YMCARET.YmcaBusinessObject.RetirementBOClass.GetManualTransactions(fundEventId, retireeType, retirementDate)
                If HelperFunctions.isNonEmpty(transactionList) Then
                    ShowHideManualTransactionLink(True)
                    hdnManualTransaction.Value = "2"
                    ShowHideMessageManualTransaction(True)
                    lblMessageManualTransaction.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_MANUALTRANSACTION_EXISTS")
                    DatagridManualTransactionList.DataSource = transactionList.Tables("ManualTransactionDetails")
                    Me.ManualTransactionDetails = transactionList
                Else
                    DatagridManualTransactionList.DataSource = Nothing
                End If
                DatagridManualTransactionList.DataBind()
            End If
            'START: PPP | 2017.06.13 | YRS-AT-2625 | If MAPR are not being fetched again then bind the old data to grid DatagridManualTransactionList
        Else
            DatagridManualTransactionList.DataSource = Me.ManualTransactionDetails
            DatagridManualTransactionList.DataBind()
            'END: PPP | 2017.06.13 | YRS-AT-2625 | If MAPR are not being fetched again then bind the old data to grid DatagridManualTransactionList
        End If 'PPP | 2017.05.30 | YRS-AT-2625 
    End Sub
    'END: MMR | 2017.02.22 | YRS-AT-2625 | Displaying manual transaction list

    'START: MMR | 2017.02.22 | YRS-AT-2625 | Show/Hide Manual Transaction link
    Private Sub ShowHideManualTransactionLink(ByVal isVisible As Boolean)
        lnkManualTransaction.Visible = isVisible
    End Sub
    'END: MMR | 2017.02.22 | YRS-AT-2625 | Show/Hide Manual Transaction link

    'START: MMR | 2017.02.22 | YRS-AT-2625 | Show/Hide Manual Transaction Message
    Private Sub ShowHideMessageManualTransaction(ByVal isVisible As Boolean)
        lblMessageManualTransaction.Visible = isVisible
    End Sub
    'END: MMR | 2017.02.22 | YRS-AT-2625 | Show/Hide Manual Transaction Message

    'START: MMR | 2017.03.03 | YRS-AT-2625 | Storing selected manual transaction for calculating estimates
    <WebMethod(True)> _
    Public Shared Function GetSelectedManualTransactions(ByVal uniqueIDs As String)
        Dim transactionList As New DataSet

        If Not HttpContext.Current.Session("ManualTransaction") Is Nothing Then
            transactionList = DirectCast(HttpContext.Current.Session("ManualTransaction"), DataSet)
            For Each transactionListRow As DataRow In transactionList.Tables("ManualTransactionDetails").Rows
                transactionListRow("Selected") = True
                If Not String.IsNullOrEmpty(uniqueIDs) AndAlso uniqueIDs.IndexOf(Convert.ToString(transactionListRow("UniqueId")), System.StringComparison.CurrentCultureIgnoreCase) >= 0 Then
                    transactionListRow("Selected") = False
                End If
            Next
            HttpContext.Current.Session("ManualTransaction") = transactionList
        End If
    End Function
    'END: MMR | 2017.03.03 | YRS-AT-2625 | Storing selected manual transaction for calculating estimates

    'START | SR | 2017.03.09 | YRS-AT-2625 - YRS enh: change in disability Annuity calculations (TrackIT 24012) 
    Private Sub SetProjectionInterestRate()
        Dim MetaNormalProjInterestRate As DataTable
        Dim ProjInterestRate As Double = 0
        Try
            ' Get projection interest rate based on retire type
            If Me.RetireType = "DISABL" Then
                MetaNormalProjInterestRate = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("EST_DISABLE_PROJECTED_INTEREST_RATE")
            Else
                MetaNormalProjInterestRate = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("EST_NORMAL_PROJECTED_INTEREST_RATE")
            End If

            If HelperFunctions.isNonEmpty(MetaNormalProjInterestRate) Then
                For Each drRow As DataRow In MetaNormalProjInterestRate.Rows
                    ProjInterestRate = If(drRow("Value").ToString() = String.Empty, 0, Convert.ToDouble(drRow("Value")))
                Next
            End If

            ' Assign default value to project interest dropdown list.
            If ProjInterestRate > 0 Then
                DropDownListProjInterest2.SelectedValue = ProjInterestRate
                ListBoxProjectedYearInterest.SelectedValue = ProjInterestRate
            End If

        Catch
            Throw
        End Try

    End Sub
    'END | SR | 2017.03.09 | YRS-AT-2625 - YRS enh: change in disability Annuity calculations (TrackIT 24012) 

    'START: MMR | 2017.03.14 | YRS-AT-2625 | Resetting manual transaction details
    Private Sub ResetManualTransactionDetails()
        ShowHideManualTransactionLink(False)
        ShowHideMessageManualTransaction(False)
        Me.ManualTransactionDetails = Nothing
        hdnManualTransaction.Value = 1
        DatagridManualTransactionList.DataSource = Nothing
    End Sub
    'START: MMR | 2017.03.14 | YRS-AT-2625 | Resetting manual transaction details

    ' START : SB | 03/08/2017 | YRS-AT-2625 | Creating a common function to check any withdrawal is taken and participant is eligible for annuity 
    Private Function IsPersonalWithdrawalExists() As Boolean
        Dim isWithdrawn As Boolean = False
        Dim hasSatisfiedPaidServiceAfterWithdrawal As Boolean = False
        Me.PersonalWithdrawalExists = False
        Me.HasSatisfiedPaidService = False
        LabelRefundMessage.Text = String.Empty
        LabelRefundMessage.Visible = False
        Dim withdrawalDetails As DataSet

        If Me.RetireType = "DISABL" AndAlso (Me.PlanType = "B" Or Me.PlanType = "R") Then
            withdrawalDetails = RetirementBOClass.HasBasicMoneyWithdrawn(Me.FundEventId, Me.RetirementDate)
            If (HelperFunctions.isNonEmpty(withdrawalDetails)) Then
                isWithdrawn = Convert.ToBoolean(withdrawalDetails.Tables(0).Rows(0).Item("IsWithdrawalExists").ToString())
                hasSatisfiedPaidServiceAfterWithdrawal = Convert.ToBoolean(withdrawalDetails.Tables(0).Rows(0).Item("SatisfiedEligiblePaidService").ToString())
            End If
            If (hasSatisfiedPaidServiceAfterWithdrawal = False And isWithdrawn = True) Then
                LabelRefundMessage.Visible = True
                LabelEstimateDataChangedMessage.Visible = False
                LabelRefundMessage.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_PARTICIPANT_WITHDRAWAL_FUNDS_FROM_BASIC_ACCOUNTS")
            End If
            Me.HasSatisfiedPaidService = hasSatisfiedPaidServiceAfterWithdrawal
            Me.PersonalWithdrawalExists = isWithdrawn
        End If
        Return isWithdrawn
    End Function
    ' END : SB | 03/08/2017 | YRS-AT-2625 | Creating a common function to check any withdrawal is taken and participant is eligible for annuity

    ' START : SB | 03/08/2017 | YRS-AT-2625 | Property Defined 
    Public Property PersonalWithdrawalExists() As Boolean
        Get
            If Not ViewState("PersonalWithdrawalExists") Is Nothing Then
                Return CType(ViewState("PersonalWithdrawalExists"), Boolean)
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("PersonalWithdrawalExists") = Value
        End Set
    End Property

    Public Property HasSatisfiedPaidService() As Boolean
        Get
            If Not ViewState("HasSatisfiedPaidService") Is Nothing Then
                Return CType(ViewState("HasSatisfiedPaidService"), Boolean)
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("HasSatisfiedPaidService") = Value
        End Set
    End Property
    ' END : SB | 03/08/2017 | YRS-AT-2625 | Property Defined 

    ' START : SB | 03/08/2017 | YRS-AT-2625 | Creating a method to display warn recalculate estimate message
    Private Sub WarnUserToRecalculate()
        LabelEstimateDataChangedMessage.Visible = True
        LabelEstimateDataChangedMessage.Text = getmessage("MESSAGE_RETIREMENT_ESTIMATE_DATA_HAS_CHANGED")
    End Sub
    ' END : SB | 03/08/2017 | YRS-AT-2625 | Creating a method to display warn recalculate estimate message

    'START: PPP | 03/17/2017 | YRS-AT-2625 | Following method will compare original accounts loaded on Accounts tab for retirement with account results after projection
    '--If projected result has more accounts than the original loaded accounts then it will load the accounts again with projected results
    '-- PROBLEM: 
    '-- 1. person is terminated 
    '-- 2. opting for Disability Retirement 
    '-- 3. does not have any contribution against account type which is active in his YMCA Resolution (BA)
    '-- For e.g. Has balance in SA, RT and TD, and current active resolution of YMCA is for BA
    '-- While loading accounts tab it checks for 'YMCATotal > 0 OR PersonalTotal > 0' condition, where BA (YMCA Accounts) gets excluded
    '-- But program is projecting balance from retirement date through age 60 for active account type (BA)
    '-- So after projection, BA (YMCA Accounts) will also have balance in mnyProjectedBalance column
    '-- This method will help to refresh and reload the accounts tab basedon projected balance also
    Private Sub RefreshElectiveAccountsTab(ByVal loadNonVoluntaryAccounts As Boolean)
        Dim selectedPlan As String
        Dim filterExpression As String
        Dim newFilterExpression As String
        Dim retirementOriginalRecords, retirementNewRecords As DataView
        Try
            If DropDownListEmployment.Items.Count <= 1 And Not Session("dsElectiveAccountsDet") Is Nothing Then
                dsElectiveAccountsDet = Session("dsElectiveAccountsDet")

                filterExpression = "(YMCATotal > 0 OR PersonalTotal > 0 )"
                newFilterExpression = "(YMCATotal > 0 OR PersonalTotal > 0 OR mnyProjectedBalance > 0)"

                selectedPlan = Me.DropDownListPlanType.SelectedValue.ToUpper()

                If selectedPlan = "RETIREMENT" Or selectedPlan = "BOTH" Then
                    If loadNonVoluntaryAccounts = True Then
                        retirementOriginalRecords = New DataView(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts"), _
                                                      String.Format("PlanType='RETIREMENT' AND {0}", filterExpression), _
                                                      "intSortOrder asc, chrAcctType", _
                                                      DataViewRowState.CurrentRows)

                        retirementNewRecords = New DataView(dsElectiveAccountsDet.Tables("RetireeGroupedElectiveAccounts"), _
                                                        String.Format("PlanType='RETIREMENT' AND {0}", newFilterExpression), _
                                                        "intSortOrder asc, chrAcctType", _
                                                        DataViewRowState.CurrentRows)

                        If (retirementOriginalRecords.Count < retirementNewRecords.Count) Then
                            DatagridElectiveRetirementAccounts.AutoGenerateColumns = False
                            DatagridElectiveRetirementAccounts.DataSource = retirementNewRecords
                            DatagridElectiveRetirementAccounts.DataBind()

                            setAccountGridControls()
                        End If
                    End If
                End If
            End If
        Catch
            Throw
        Finally
            selectedPlan = Nothing
            filterExpression = Nothing
            newFilterExpression = Nothing
            retirementOriginalRecords = Nothing
            retirementNewRecords = Nothing
        End Try
    End Sub
    'END: PPP | 03/17/2017 | YRS-AT-2625 | Following method will compare original accounts loaded on Accounts tab for retirement with account results after projection
    'START : BD : 2018.11.01 : YRS-AT-4133 - No Active Death Benefit if first enrolled on or after 1/1/2019
    Private Sub ValidateAndShowRDBWarninMessage(ByVal planType As String)
        LabelRDBWarningMessage.Visible = False
        If (Me.PlanType = "B" Or Me.PlanType = "R" Or Me.PlanType = String.Empty) _
            And Me.OrgBenTypeIsQDROorRBEN = False _
            And Not YMCARET.YmcaBusinessObject.RetirementBOClass.IsFirstEnrolledBeforeCutOffDate_2019PlanChange(Me.FundEventId) Then
            If Not YMCARET.YmcaBusinessObject.RetirementBOClass.IsEffectiveDateNull(Me.FundEventId) Then
                LabelRDBWarningMessage.Visible = True
                LabelRDBWarningMessage.Text = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_RETIREMENT_RDB_RESTRICTED_DUE_TO_2019_PLAN_CHANGE, YMCARET.YmcaBusinessObject.RetirementBOClass.GetRDBPlanChangeCutOffDateReplacementDict()).DisplayText()
            End If
        End If
    End Sub
    'END : BD : 2018.11.01 : YRS-AT-4133 - No Active Death Benefit if first enrolled on or after 1/1/2019
End Class
