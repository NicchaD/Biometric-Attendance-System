'************************************************************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
' Project Name		:	YMCA_YRS
' FileName			:	LoanOptions.aspx.vb
' Author Name		:	Shubhrata Tripathi
' Employee ID		:	34774
' Email				:	shubhrata.tripathi@3i-infotech.com    
' Contact No		:	8642
' Creation Time		:	Aug 28th ,2006
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'*******************************************************************************
'Modification History
'***********************************************************************************************************************
'Modified By            Date            Desription
'************************************************************************************************************************
'Ashutosh Patil         09-Feb-2007     Regarding Ragesh's mail for sending emails with letters 
'                                       to the YMCA person YREN - 3023
'Ashutosh Patil         19-Apr-2007     Loan letters to be called from one IDM class only. Setting properties for IDM Class
'Ashutosh Patil 
'And Shubrata Tripathi  23-Apr-2007     Validation of Loan start date
'Ashutosh Patil         21-May-2007     UseDefault Key will be used from AtsMetaConfiguration instead of Web.Config

'************************************************************************************************************************
'Shubhrata Jan 8th 2006 to save pay off details(amount and computation date) as per Ragesh's mail on Jan6th2006
'YREN - 3012
'Ashutosh Patil as on 18-Jan-2007 for showing the reports and writing to IDM for YREN - 3023
'Shubhrata jan 28th 2006 YREN-3034 To add controls for showing default amount.
'Paramesh K. on July 30th 2008 for fixing issue YRS 5.0 467
'Priya Jawale           16-March-2009   Change Time of Customer Service Department 8:45 AM to 6:00 previously it was 8:45 AM to 8:00 as per Hafiz mail on March 13, 2009 
'Priya Jawale           17-March-2009   For Issue YRS 5.0 717  Made changes into MailMessage, message selected from database as per hafiz mail on 16-March-2009
'Ashish Srivastava      17-March-2009   Generate Loan Re-amortization letter for YRS 5.0 679
'Nikunj Patel           2009.04.07      Cleaning up Mail Util service.
'Nikunj Patel           2009.04.22      Changing code to fetch the Email Id of the TRANSM YMCA Contact for the specified YMCA Id
'Sanjay Rawat           2009.06.22      To cange the DocCode for DEFAULT Type
'Neeraj Singh           12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Priya                  2010-06-30       Changes made for enhancement in vs-2010 
'Shashi Shekhar         24-Dec-2010      For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'Shashi                 28 Feb 2011         Replacing Header formating with user control (YRS 5.0-450 )
'Shagufta               15 July 2011    For YRS 5.0-1320,BT-829:letter when loan is paid off
'BhavnaS                02 July 2012    YRS 5.0-734 : Add code to avoid possible double-click
'Shashank Patel			2014.01.15		BT-2314\YRS 5.0-2260 - YRS generating blank loan payoff letters 
'Anudeep A              2014.08.26      BT:2625 :YRS 5.0-2405-Consistent screen header sections 
'Anudeep A              2015.04.02      BT:2699 :YRS 5.0-2411-Modifications for 403b Loans
'Anudeep                2015.06.26      BT:2900: YRS 5.0-2533:Loan Modifications (Default) Project: two new YRS letters
'Anudeep A              2015.08.05      YRS 5.0-2441-Modifications for 403b Loans
'Manthan Rajguru        2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Anudeep A              2015.10.26      YRS-AT-2533 - Loan Modifications (Default) Project: two new YRS letters
'Anudeep A              2015.10.26      YRS-AT-2533 - Loan Modifications (Default) Project: two new YRS letters
'Manthan Rajguru        2015.11.04      YRS-AT-2453: Need ability to select other YMCA association for use in Loan reAmortization
'Manthan Rajguru        2015.11.10      YRS-AT-2669: YRS enh: do not generate Letters or Emails or Loans Offset due to death
'Anudeep A              2015.11.09      YRS-AT-2660 - YRS enh: new loan letter and email for payoff defaulted loans(closed status) TrackIT 24153
'Manthan Rajguru        2016.02.03      YRS-AT-2453: Need ability to select other YMCA association for use in Loan reAmortization (Re-Work)
'Bala                   2016.04.27      YRS-AT-2667 - Adjustment to Loan Satisfied-part.rpt (for DEFAULTED LOANS)
'Anudeep A              2016.04.26      YRS-AT-2831 - YRS enh: automate 'Default' of Loans (TrackIT 25242)
'Bala                   2016.05.03      YRS-AT-2667 - Adjustment to Loan Satisfied-part.rpt (for DEFAULTED LOANS)
'Vinayan c              2018.06.21      YRS-AT-3190 - YRS enh: add warning message for Loan Re-amortization (to prevent duplicate re-amortize efforts
'Santosh B              2018.11.13      YRS-AT-3101 - YRS-AT-3101 -  YRS enh: EFT Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
'Manthan R              2019.01.08      YRS-AT-4130 -  YRS bug: Loan Maintenance:Unsuspend/ReAmortize Loan function does not flow properly(TRACKIT#35525)
'Pooja K                2019.01.15      YRS-AT-2573 - YRS enh: add dropdown for first payment due date for Loan ReAmortization (TrackIT 23592)
'Manthan R              2019.01.23      YRS-AT-4118 -  YRS enh-unsuspended loans when reamortized must be extended by the suspension period(Track it 35417) 
'Sanjay R               2019.02.06      YRS-AT-2920 -  YRS enh-do not allow loan re-amortizations if unfunded payments exist (TrackIT25480)
'Shilpa N               2019.03.25      YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547) 
'Shiny C.               2020.04.10      YRS-AT-4852 -  COVID - YRS changes needed for LOANS due to CARE Act (Track IT - 41693) 
'***********************************************************************************************************************
Imports System.IO
Imports YMCARET.YmcaBusinessObject.MetaMessageBO
Imports YMCAObjects.MetaMessageList

Public Class LoanOptions
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("FindInfo.aspx?Name=LoanOptions")
    'End issue id YRS 5.0-940
    Dim IDMAll As New IDMforAll
    Dim l_ltr_msg As String = ""
    Dim l_str_LoanStatus As String

    Private m_int_const_Suspend As Int16 = 0
    Private m_int_const_Unsuspend As Int16 = 1
    Private m_int_const_Terminate As Int16 = 2
    Private m_int_const_PayOff As Int16 = 3
    Private m_int_const_Default As Int16 = 4
    Private m_int_const_Offset As Int16 = 5  'AA:15.04.2015 BT:2699:yrs 5.0-2441 -Added a new tab Offset
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents LoanOptionsTabStrip As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents LoanOptionsMultiPage As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents RadioButtonLoanSuspensionYes As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonLoanSuspensionNo As System.Web.UI.WebControls.RadioButton
    'Protected WithEvents LabelHdr As System.Web.UI.WebControls.Label
    'Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDetails As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRequestDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelLoanSuspension As System.Web.UI.WebControls.Label
    Protected WithEvents LabelSuspendDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelUnSuspendLoan As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRemainingAmount As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRemainingMonths As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDefaultLoan As System.Web.UI.WebControls.Label
    Protected WithEvents LabelDefaultDate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelPayOffLoan As System.Web.UI.WebControls.Label
    Protected WithEvents LabelLoanSuspensionStatus As System.Web.UI.WebControls.Label
    Protected WithEvents LabelLoanUnSuspensionStatus As System.Web.UI.WebControls.Label
    Protected WithEvents LabelCloseLoan As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRemainingReamorizeAmount As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRemainingReamorizeMonths As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxRemainingReamorizeAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxRemainingReamorizeMonths As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxRemainingAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxRemainingMonths As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxDefaultAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonPayOffLoan As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSuspendLoan As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonUnSuspendLoan As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonShowDetails As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCloseLoan As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonDefaultLoan As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOkDefault As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOkClose As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOkPayOffLoan As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOkUnsuspendLoan As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOkSuspendLoan As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonShowDefaultAmount As System.Web.UI.WebControls.Button
    Protected WithEvents LabelDefaultAmount As System.Web.UI.WebControls.Label
    'Start -- Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Added new label and dropdown control to display Active YMCA
    Protected WithEvents DropDownActiveYMCA As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelActiveYMCA As System.Web.UI.WebControls.Label
    'End -- Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Added new label and dropdown control to display Active YMCA
    'START: PK | 01.15.2019 | YRS-AT-2573 | Added Dropdown to display list of payment date and label to display selected date from dropdown
    Protected WithEvents DropdownFirstPaymentDate As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblPayDates As System.Web.UI.WebControls.Label
    'END: PK | 01.15.2019 | YRS-AT-2573 | Added Dropdown to display list of payment date and label to display selected date from dropdown
    Protected WithEvents TextBoxSuspendDate As YMCAUI.DateUserControl
    Protected WithEvents TextBoxUnSuspendDate As YMCAUI.DateUserControl
    Protected WithEvents TextBoxDefaultDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidatorSuspendDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredfieldvalidatorUnSuspendDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    'Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    Protected WithEvents btnYes As System.Web.UI.WebControls.Button
    Protected WithEvents btnNo As System.Web.UI.WebControls.Button
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label

    'Start:AA:02.04.2015 BT:2699:yrs 5.0-2441 - Added new labels to dispaly unpaid pricnipal and cureperiod interest and phantom interest in default and offset loan tabs
    Protected WithEvents lblUnpaidPrincipal As System.Web.UI.WebControls.Label
    Protected WithEvents lblCRINT As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxOffsetAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxOffsetDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblOffsetUnpaidPrincipal As System.Web.UI.WebControls.Label
    Protected WithEvents lblOffsetInt As System.Web.UI.WebControls.Label
    Protected WithEvents lblOffsetAmountdetail As System.Web.UI.WebControls.Label
    Protected WithEvents lblOffsetIntdetail As System.Web.UI.WebControls.Label
    Protected WithEvents lblPrincipal As System.Web.UI.WebControls.Label
    Protected WithEvents LabelOffsetLoan As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonOkOffset As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOffsetLoan As System.Web.UI.WebControls.Button
    Protected WithEvents lblPersonAge As System.Web.UI.WebControls.Label
    Protected WithEvents lblEmpStatus As System.Web.UI.WebControls.Label

    Protected WithEvents TextboxRTOffset As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelRTOffsetPrincipal As System.Web.UI.WebControls.Label
    Protected WithEvents LabelRTOffsetInterest As System.Web.UI.WebControls.Label

    Protected WithEvents trRTOffset As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trRTOffsetPrincipal As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trRTOffsetInterest As System.Web.UI.HtmlControls.HtmlTableRow

    'End:AA:02.04.2015 BT:2699:yrs 5.0-2441 - Added new labels to dispaly unpaid pricnipal and cureperiod interest and phantom interest in default and offset loan tabs
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    'START : SR | 2019.02.06 | YRS-AT-2920 | YRS enh-do not allow loan re-amortizations if unfunded payments exist (TrackIT25480)
    Protected WithEvents btnUnfundedAmountYes As System.Web.UI.WebControls.Button
    Protected WithEvents btnUnfundedAmountNo As System.Web.UI.WebControls.Button
    Protected WithEvents lblUnfundedAmountWarning As System.Web.UI.WebControls.Label
    'END : SR | 2019.02.06 | YRS-AT-2920 | YRS enh-do not allow loan re-amortizations if unfunded payments exist (TrackIT25480)

    Protected WithEvents DropDownReasonsForLoanSuspension As System.Web.UI.WebControls.DropDownList 'SC | 2020.04.10 | YRS-AT-4852 | Added new dropdown for selecting reason for suspend Loan
    Protected WithEvents LabelUnSuspendLoanDueToCovid As System.Web.UI.WebControls.Label 'SC | 2020.04.10 | YRS-AT-4852 | Added new label for Unsusped date due to COVID
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
#Region "Properties"
    'To Keep the Selected PersonID 
    Private Property SessionPersonID() As String
        Get
            If Not (Session("PersonID")) Is Nothing Then
                Return (CType(Session("PersonID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PersonID") = Value
        End Set
    End Property
    Private Property SessionSSNo() As String
        Get
            If Not (Session("SSNo")) Is Nothing Then
                Return (CType(Session("SSNo"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("SSNo") = Value
        End Set
    End Property
    Private Property SessionLoanStatus() As String
        Get
            If Not (Session("LoanStatus")) Is Nothing Then
                Return (CType(Session("LoanStatus"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("LoanStatus") = Value
        End Set
    End Property
    Private Property SessionLoanRequestId() As Integer
        Get
            If Not (Session("LoanRequestId")) Is Nothing Then
                Return (CType(Session("LoanRequestId"), Integer))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("LoanRequestId") = Value
        End Set
    End Property
    'Shubhrata YREN - 3014
    Private Property SessionOrigLoanNo() As Integer
        Get
            If Not (Session("OrigLoanNo")) Is Nothing Then
                Return (CType(Session("OrigLoanNo"), Integer))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("OrigLoanNo") = Value
        End Set
    End Property
    Private Property SessionEmpEventId() As String
        Get
            If Not (Session("EmpEventId")) Is Nothing Then
                Return (CType(Session("EmpEventId"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("EmpEventId") = Value
        End Set
    End Property
    Private Property SessionYmcaId() As String
        Get
            If Not (Session("YmcaId")) Is Nothing Then
                Return (CType(Session("YmcaId"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("YmcaId") = Value
        End Set
    End Property
    Private Property SessionYmcaNo() As String
        Get
            If Not (Session("LoanYMCANo")) Is Nothing Then
                Return (CType(Session("LoanYMCANo"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("LoanYMCANo") = Value
        End Set
    End Property
    'Session("TDBalance")
    Private Property SessionTDBalance() As Double
        Get
            If Not (Session("TDBalance")) Is Nothing Then
                Return (CType(Session("TDBalance"), Double))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Double)
            Session("TDBalance") = Value
        End Set
    End Property


    'Shubhrata YREN - 3014
    Private Property SessionParameterRequestField() As String
        Get
            If Not (Session("ParameterRequestField")) Is Nothing Then
                Return (CType(Session("ParameterRequestField"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("ParameterRequestField") = Value
        End Set
    End Property
    Private Property SessionFundID() As String
        Get

            If Not (Session("FundID")) Is Nothing Then
                Return (CType(Session("FundID"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("FundID") = Value
        End Set
    End Property
    'Start:AA:10.26.2015  YRS-AT-2533 : Added to store default date while offset a loan
    Private Property Offsetevent_Defaultdate As String
        Get
            If Not (ViewState("Offsetevent_Defaultdate")) Is Nothing Then
                Return (CType(ViewState("Offsetevent_Defaultdate"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(value As String)
            ViewState("Offsetevent_Defaultdate") = value
        End Set
    End Property
    'End:AA:10.26.2015  YRS-AT-2533 :  Added to store default date while offset a loan
    'START: MMR | 2018.12.26 | YRS-AT-4130 | Store success message
    Private Property SuccessMessage() As String
        Get
            If Not ViewState("SuccessMessage") Is Nothing Then
                Return (DirectCast(ViewState("SuccessMessage"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("SuccessMessage") = Value
        End Set
    End Property
    'END: MMR | 2018.12.26 | YRS-AT-4130 | Store success message
    'Start:AA:11.09.2015 YRS-AT-2660 Added to store the loan status before update to verify whether the loan is being closed from paid or default status
    Private Property PreviousLoanStatus As String
        Get
            If Not (ViewState("PreviousLoanStatus")) Is Nothing Then
                Return (CType(ViewState("PreviousLoanStatus"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(value As String)
            ViewState("PreviousLoanStatus") = value
        End Set
    End Property
    'End:AA:11.09.2015 YRS-AT-2660 Added to store the loan status before update to verify whether the loan is being closed from paid or default status

    'START: PK | 01.21.2019 | YRS-AT-2573 | Following message will be displayed on screen with default first payment date
    Private Property DefaultFirstPaymentDateMessage As String
        Get
            If Not (ViewState("DefaultFirstPaymentDateMessage")) Is Nothing Then
                Return (CType(ViewState("DefaultFirstPaymentDateMessage"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(value As String)
            ViewState("DefaultFirstPaymentDateMessage") = value
        End Set
    End Property
    'START: MMR | 2019.01.23 | YRS-AT-4118 | Store Active YMCA Details
    Private Property ActiveYmcaDetails As DataSet
        Get
            If Not (Session("ActiveYmcaDetails")) Is Nothing Then
                Return (CType(Session("ActiveYmcaDetails"), DataSet))
            Else
                Return Nothing
            End If
        End Get
        Set(value As DataSet)
            Session("ActiveYmcaDetails") = value
        End Set
    End Property
    'END: MMR | 2019.01.23 | YRS-AT-4118 | Store Active YMCA Details


    'START | SR | 2019.01.29 | YRS-AT-2920 | Declared Property to check unfunded amount exists or not while Reammortize loan.
    Public Property UnfundedPaymentWarningMessage As String
        Get
            If Not (ViewState("UnfundedPaymentWarningMessage")) Is Nothing Then
                Return (CType(ViewState("UnfundedPaymentWarningMessage"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(value As String)
            ViewState("UnfundedPaymentWarningMessage") = value
        End Set
    End Property

    Public Property IsUnfundedPaymentExist As Boolean
        Get
            If Not (ViewState("IsUnfundedPaymentExist")) Is Nothing Then
                Return (CType(ViewState("IsUnfundedPaymentExist"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            ViewState("IsUnfundedPaymentExist") = value
        End Set
    End Property
    'END | SR | 2019.01.29 | YRS-AT-2920 | Declared Property to check unfunded amount exists or not while Reammortize loan.
    'Start | SC | 2020.04.10 | YRS-AT-4852 | Added property for identifying if participant on military leave
    Public Property IsParticipantOnMilitaryLeave As Boolean
        Get
            If Not (ViewState("IsParticipantOnMilitaryLeave")) Is Nothing Then
                Return (CType(ViewState("IsParticipantOnMilitaryLeave"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            ViewState("IsParticipantOnMilitaryLeave") = value
        End Set
    End Property
    'End | SC | 2020.04.10 | YRS-AT-4852 | Added property for identifying if participant on military leave
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim LabelModuleName As Label
        Dim loanStatus As String  'SB | 2018.11.13 | YRS-AT-3101 | To display loan status as DEFAULT instead of DEFALT on UI
        Try
            'Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            'Menu1.DataBind()
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", True)
                Exit Sub
            End If
            Dim strSSN As String = Me.SessionSSNo.Insert(3, "-")
            strSSN = strSSN.Insert(6, "-")
            '-----------------------------------------------------------------------------------------------------------------
            ' Shashi Shekhar: 24 - dec - 2010: For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
            'Me.LabelTitle.Text = "Tax Deferred Loans -- Loan Maintenance" + "--" + Session("LastName") + ", " + Session("FirstName") + " " + Session("MiddleName") + ",SS#: " + strSSN
            'If Not Session("FundNo") Is Nothing Then
            '    'Me.LabelTitle.Text = "Tax Deferred Loans -- Loan Maintenance" + "--" + Session("LastName") + ", " + Session("FirstName") + " " + Session("MiddleName") + ",Fund No: " + Session("FundNo").ToString().Trim()
            '    'Shashi: 28 Feb 2011:  Replacing Header formating with user control (YRS 5.0-450 )
            '    'Headercontrol.pageTitle = "Tax Deferred Loans -- Loan Maintenance"
            '    'Headercontrol.SSNo = Me.SessionSSNo.Trim            '    
            'End If
            '----------------------------------------------------------------------------------------------------------------------------------------------
            'START: SB | 2018.11.13 | YRS-AT-3101 | To display loan status as DEFAULT instead of DEFALT on UI
            'Me.LabelDetails.Text = "Loan Status" + " : " + Me.SessionLoanStatus.Trim.ToUpper
            loanStatus = Me.SessionLoanStatus.Trim.ToUpper
            If (loanStatus = "DEFALT") Then
                loanStatus = "DEFAULT"
            End If
            Me.LabelDetails.Text = "Loan Status" + " : " + loanStatus
            'END: SB | 2018.11.13 | YRS-AT-3101 | To display loan status as DEFAULT instead of DEFALT on UI

            Me.LabelRequestDate.Text = "Loan Request Date" + " : " + Session("RequestDate")
            If IsPostBack Then
                'If Not Request.Form("Yes") Is Nothing Then
                '    If Request.Form("Yes") = "Yes" Then
                '        Me.CloseLoan()
                '    End If
                'End If
                'Me.SetTabStrip(Me.SessionLoanStatus.Trim.ToUpper, 0)
                'If Not Request.Form("Yes") Is Nothing Then
                '    If Request.Form("Yes") = "Yes" And ViewState("UnsuspendYes") = Nothing Then
                '        Dim l_string_message As String = ""
                '        l_string_message = Me.CancelLoan(Me.SessionParameterRequestField)
                '        If l_string_message <> "" Then
                '            l_string_message = l_string_message & l_ltr_msg
                '            'by Aparna 05/10/2007 IE7
                '            'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", l_string_message, MessageBoxButtons.OK, False)
                '            HelperFunctions.ShowMessageToUser(l_string_message, EnumMessageTypes.Success)
                '        End If
                '    End If
                '    If Request.Form("Yes") = "Yes" And ViewState("UnsuspendYes") = "Yes" Then
                '        Me.UnsuspendLoan()
                '        ViewState("UnsuspendYes") = Nothing
                '    End If
                'End If
                'If Not Request.Form("OK") Is Nothing Then
                '    If ViewState("Cancel") = True Then
                '        If Me.SessionParameterRequestField = "CLOSED" Or Me.SessionParameterRequestField = "DEFALT" Or Me.SessionParameterRequestField = "PAYOFF" Then
                '            ViewState("TabClicked") = Nothing
                '            Response.Redirect("FindInfo.aspx?Name=LoanOptions", False)
                '        Else
                '            Me.SetTabStrip(Me.SessionLoanStatus.Trim.ToUpper)
                '            Session("parameterRequestField") = Nothing
                '            ViewState("Cancel") = False
                '        End If
                '    End If
                'End If
                'START: MMR | 2018.12.26 | YRS-AT-4130 | Added to maintain success message on UI as it does not remain static on screen
                If Not String.IsNullOrEmpty(Me.SuccessMessage) Then
                    HelperFunctions.ShowMessageToUser(SuccessMessage, EnumMessageTypes.Success)
                    Me.SuccessMessage = Nothing
                End If
                'END: MMR | 2018.12.26 | YRS-AT-4130 | Added to maintain success message on UI as it does not remain static on screen
            End If
            If Not IsPostBack Then
                Session("l_string_message") = Nothing
                Session("ds_closeloandetails") = Nothing
                Session("l_bitvalid") = Nothing
                Session("stringMessage") = Nothing
                ViewState("TabClicked") = Nothing
                ViewState("UnsuspendYes") = Nothing
                'Start: Bala: 05.03.2016: YRS-AT-2667: Clearing report session.
                Session("strReportName") = Nothing
                Session("strReportName_1") = Nothing
                'End: Bala: 05.03.2016: YRS-AT-2667: Clearing report session.
                Me.SetTabStrip(Me.SessionLoanStatus.Trim.ToUpper)
                'Me.IsPayOffValid()
                'Me.SetTextinLabelsOnLoad(Me.SessionLoanStatus.Trim.ToUpper)
                Me.LoadSuspenddate(Me.SessionLoanRequestId)
                If Not Session("FundNo") Is Nothing Then
                    LabelModuleName = Master.FindControl("LabelModuleName")
                    If LabelModuleName IsNot Nothing Then
                        LabelModuleName.Text = "Activities > TD Loans > Loan Maintenance >  Fund Id " + Session("FundNo").ToString() ' + " - " + TextBoxFirst.Text + " " + TextBoxLast.Text
                    End If

                End If
            End If
            'Start | SC | 2020.04.10 | YRS-AT-4852 | Check if Loan suspended due to COVID
            If Me.SessionLoanStatus.Trim.ToUpper = "SUSPND" And YMCARET.YmcaBusinessObject.LoanInformationBOClass.IsLoanSuspendedDueToCovid(SessionLoanRequestId) Then
                LabelUnSuspendLoan.Visible = False
                LabelUnSuspendLoanDueToCovid.Visible = True
            End If
            'End | SC | 2020.04.10 | YRS-AT-4852 | Check if Loan suspended due to COVID
            CheckReadOnlyMode() 'Shilpa N | 03/25/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_Page_load", ex)
        End Try
    End Sub
#Region "Events"
    Private Sub LoanOptionsTabStrip_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles LoanOptionsTabStrip.SelectedIndexChange
        Try
            Me.LoanOptionsMultiPage.SelectedIndex = Me.LoanOptionsTabStrip.SelectedIndex
            ViewState("TabClicked") = True
            If Me.LoanOptionsTabStrip.SelectedIndex = 0 Then
                Me.TextBoxSuspendDate.RequiredDate = True
                Me.TextBoxUnSuspendDate.RequiredDate = False
                ' Me.TextBoxDefaultDate.RequiredDate = False
            ElseIf Me.LoanOptionsTabStrip.SelectedIndex = 1 Then
                Me.TextBoxSuspendDate.RequiredDate = False
                Me.TextBoxUnSuspendDate.RequiredDate = True
                'Me.TextBoxDefaultDate.RequiredDate = False
            ElseIf Me.LoanOptionsTabStrip.SelectedIndex = 4 Then
                Me.TextBoxSuspendDate.RequiredDate = False
                Me.TextBoxUnSuspendDate.RequiredDate = False
                'Me.TextBoxDefaultDate.RequiredDate = True
            Else
                Me.TextBoxSuspendDate.RequiredDate = False
                Me.TextBoxUnSuspendDate.RequiredDate = False
                'Me.TextBoxDefaultDate.RequiredDate = False
            End If
            If Me.LoanOptionsTabStrip.SelectedIndex = 2 Then
                If Me.SessionLoanStatus.Trim.ToUpper = "UNSPND" Then
                    Me.LoadLoanCloseDetails(Me.SessionLoanRequestId, "UNSPND")
                Else
                    Me.LoadLoanCloseDetails(Me.SessionLoanRequestId, "PAID")
                End If

            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptionsTabStrip_SelectedIndexChange", ex)
        End Try

    End Sub

    Private Sub ButtonPayOffLoan_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles ButtonPayOffLoan.Command
        Try

            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)

                Exit Sub
            End If
            'End : YRS 5.0-940
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "ButtonPayOffLoan_Command", ex)
        End Try
    End Sub

    Private Sub ButtonPayOffLoan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonPayOffLoan.Click
        Try
            Dim l_int_count As Integer
            Dim l_string_message As String
            l_string_message = ""
            l_int_count = 0
            Dim l_bool_flag As Boolean
            'AA:06.05.2015 BT: 2699 : YRS 5.0-2441 - Added to allow DEFAULT loans to PAY OFF
            If Me.SessionLoanStatus.Trim.ToUpper = "PAID" Or Me.SessionLoanStatus.Trim.ToUpper = "UNSPND" Or Me.SessionLoanStatus.Trim.ToUpper = "DEFALT" Then
                Me.SessionParameterRequestField = "PAYOFF"
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_PAYOFF) + "','Loan Pay-Off');", True)
                Exit Sub
            Else
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_PAYOFF_CAN_BE_PAID)
                Exit Sub
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "ButtonPayOffLoan_Click", ex)
        End Try
    End Sub

    Private Sub ButtonSuspendLoan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSuspendLoan.Click
        Try
            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Dim l_string_message As String
            l_string_message = ""
            If Me.SessionLoanStatus.ToUpper.Trim = "PAID" Then
                'Start | SC | 2020.04.10 | YRS-AT-4852 | If no Loan suspension reason present
                If DropDownReasonsForLoanSuspension.SelectedIndex = -1 Then
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_BE_SUSPENDED_NO_REASON_SELECTED)
                    Exit Sub
                End If
                'End | SC | 2020.04.10 | YRS-AT-4852 | If no Loan suspension reason present
                If Me.RadioButtonLoanSuspensionYes.Checked = True Then
                    Me.SessionParameterRequestField = "SUSPND"
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_SUSPEND) + "','Loan Suspend');", True)
                    Exit Sub
                ElseIf Me.RadioButtonLoanSuspensionNo.Checked = True Then
                    'HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_BE_SUSPENDED_NO_MILATARY_DOC)
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_BE_SUSPENDED_NO_SUPPORTED_DOC) 'End | SC | 2020.04.10 | YRS-AT-4852 | If no supported document
                    Exit Sub
                End If
            Else
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_SUSPENDED_NOT_OTHERTHAN_PAID)
                Exit Sub
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "ButtonSuspendLoan_Click", ex)
        End Try
    End Sub

    Private Sub ButtonUnSuspendLoan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonUnSuspendLoan.Click
        Try
            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            'End : YRS 5.0-940
            Dim l_string_message As String
            l_string_message = ""
            If Me.IsUnsuspensionDateValid = True Then
                'here we are automatically calculating payoff amount in case user doesnt clicks ShowDetails

                Me.UnsuspendLoan()
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "ButtonUnSuspendLoan_Click", ex)
        End Try
    End Sub

    Private Sub ButtonCloseLoan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCloseLoan.Click
        ' START: SR | 2019.01.31 | YRS-AT-2920 | commented existing code and moved code to common method so that it can be called from Yes button of unfunded loan exist warning message and button "Terminate /Re-amortize Loan" .
        Try
            CloseLoan()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "ButtonUnSuspendLoan_Click", ex)
        End Try
        'Dim parameters As Dictionary(Of String, String) 'PK | 01.15.2019 | YRS-AT-2573 | code added here to show selected date from dropdown
        'Try
        '    'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
        '    Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

        '    If Not checkSecurity.Equals("True") Then
        '        HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
        '        Exit Sub
        '    End If
        '    'End : YRS 5.0-940
        '    'Start -- Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Display message after validating YMCA selection
        '    If Me.DropDownActiveYMCA.SelectedValue = "" Then
        '        HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_TERMINATE_NO_ACTIVE_YMCA_SELECTION)
        '        Exit Sub
        '    End If
        '    'End -- Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Display message after validating YMCA selection
        '    If Me.SessionLoanStatus.Trim.ToUpper = "PAID" Or Me.SessionLoanStatus.Trim.ToUpper = "UNSPND" Then
        '        If Me.IsActiveMultipleYMCA(Me.SessionLoanRequestId) = True Then
        '            If Me.SessionOrigLoanNo = 0 Then
        '                ' if Loan No is nt present then it cant proceed further.
        '                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_TERMINATE_LOAN_ORIGINAL_IS_INVALID)
        '                Exit Sub
        '            Else
        '                Me.SessionParameterRequestField = "CLOSED"
        '                'START : VC | 2018.06.21 | YRS-AT-3190 | To check whether the loan is recently reamortized, if yes display detailed confirmation message else display confirmation message only
        '                'ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_REAMORTIZE) + "','Loan Re-Amortize');", True)
        '                ''BS:2012.07.02: YRS 5.0-734:to avoid duplicacy 
        '                'Session("terminate_re-amortize") = "TerminateReamortize"

        '                'START: PK | 01.15.2019 | YRS-AT-2573 |code added here to show selected date from dropdown 
        '                'If (IsLoanReamortizedEarlier(Me.SessionLoanRequestId)) Then
        '                '    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_REAMORTIZATION) + "','Loan Re-Amortize');", True)
        '                'Else
        '                '    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_REAMORTIZE) + "','Loan Re-Amortize');", True)
        '                'End If

        '                parameters = New Dictionary(Of String, String)
        '                parameters.Add("FirstPaymentDate", DropdownFirstPaymentDate.SelectedValue)
        '                If (IsLoanReamortizedEarlier(Me.SessionLoanRequestId)) Then
        '                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_REAMORTIZATION, parameters) + "','Loan Re-Amortize');", True)
        '                Else
        '                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_REAMORTIZE, parameters) + "','Loan Re-Amortize');", True)
        '                End If
        '                'END: PK | 01.15.2019 | YRS-AT-2573 | code added here to show selected date from dropdown

        '                Session("terminate_re-amortize") = "TerminateReamortize"
        '                'END : VC | 2018.06.21 | YRS-AT-3190 | To check whether the loan is recently reamortized, if yes display detailed confirmation message else display confirmation message only
        '                Exit Sub
        '            End If
        '        ElseIf Me.IsActiveMultipleYMCA(Me.SessionLoanRequestId) = False Then
        '            Me.ButtonCloseLoan.Enabled = False
        '            HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_TERMINATE_NO_ACTIVE_EMPLOYEMENT_EXISTS)
        '            Me.LabelCloseLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_CANNOT_TERMINATE_NO_ACTIVE_EMPLOYEMENT_EXISTS)
        '            Exit Sub
        '        End If
        '        'ElseIf Me.EnableDefaultRadioButtons(Me.SessionLoanRequestId) = False Then
        '        '    MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Due date has passed current date.", MessageBoxButtons.Stop)
        '        'End If
        '    ElseIf Me.SessionLoanStatus.Trim.ToUpper <> "PAID" Or Me.SessionLoanStatus.Trim.ToUpper <> "UNSPND" Then
        '        HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CLOSED_NOT_OTHERTHAN_PAID_SUSPENDED)
        '    End If
        'Catch ex As Exception
        '    HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        '    HelperFunctions.LogException("LoanOptions_" + "ButtonUnSuspendLoan_Click", ex)
        'End Try
        ' END: SR | 2019.01.31 | YRS-AT-2920 | commented existing code and moved code to common method so that it can be called from Yes button of unfunded loan exist warning message and button "Terminate /Re-amortize Loan" .
    End Sub

    Private Sub ButtonDefaultLoan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDefaultLoan.Click
        Try
            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            'End : YRS 5.0-940

            Dim l_string_message As String
            l_string_message = ""
            Dim l_datetime_defaultdate As DateTime
            If Me.SessionLoanStatus.Trim.ToUpper = "PAID" Or Me.SessionLoanStatus.Trim.ToUpper = "UNSPND" Then
                'the system displays 90 days past due date by default,in case a user enters a date greater than the 
                'default date we will prompt him,if he enters date less than default date we r not to allow him to proceed.
                'If Me.TextBoxDefaultDate.Text.Trim.length > 0 Then
                '    l_datetime_defaultdate = Convert.ToDateTime(Me.TextBoxDefaultDate.Text.Trim).Date
                '    If l_datetime_defaultdate > Session("DefaultDate_Automatic") Then
                '        Me.SessionParameterRequestField = "DEFALT"
                '        MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Default date is greater than 90 days past due date.Do you still want to default the loan?", MessageBoxButtons.YesNo)
                '        Exit Sub
                '    ElseIf l_datetime_defaultdate < Session("DefaultDate_Automatic") Then
                '        MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Default date is less than 90 days past due date.", MessageBoxButtons.Stop)
                '        Exit Sub
                '    Else
                '        Me.SessionParameterRequestField = "DEFALT"
                '        MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Are you sure you want to default the loan?", MessageBoxButtons.YesNo)
                '        Exit Sub
                '    End If
                'End If
                Me.SessionParameterRequestField = "DEFALT"
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_DEFAULT) + "','Loan Default');", True)
                'BS:2012.07.02: YRS 5.0-734:to avoid duplicate loan request  
                Session("DefaultLoan") = "DefaultLoan"
                Exit Sub
            Else
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CAN_BE_DEFAULT_PAID_UNSUSPENDED)
                Exit Sub
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "ButtonDefaultLoan_Click", ex)
        End Try
    End Sub
    ''Private Sub ButtonCancelLoan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancelLoan.Click
    ''    Try
    ''        Dim l_string_message As String
    ''        l_string_message = ""
    ''        If Me.IsValidCancel(Me.SessionLoanStatus, Me.SessionLoanRequestId) = True Then
    ''            l_string_message = "Loan has been cancelled."
    ''            Me.CancelLoanUpdateAmortization(Me.SessionLoanStatus, "CANCEL", Me.SessionLoanRequestId, "", "", l_string_message)
    ''        Else
    ''            Exit Sub
    ''        End If
    ''    Catch ex As Exception
    ''        Dim l_String_Exception_Message As String
    ''        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    ''        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    ''    End Try
    ''End Sub

    'Private Sub ButtonCancelLoan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancelLoan.Click
    '    Try
    '        Dim l_string_message As String
    '        l_string_message = ""
    '        'If Me.IsValidCancel(Me.SessionLoanRequestId, Me.SessionLoanStatus.Trim.ToUpper) = True Then
    '        If Me.SessionLoanStatus.Trim.ToUpper = "REVE" Then
    '            l_string_message = "Loan has been cancelled.  Note:Report is missing."
    '            Me.CancelLoanUpdateAmortization("CANCEL", Me.SessionLoanStatus.Trim.ToUpper, Me.SessionLoanRequestId, "", "", l_string_message, "Cancelled")
    '            'Session("ReloadLoan") = True
    '            'Dim msg As String
    '            'msg = msg + "<Script Language='JavaScript'>"

    '            'msg = msg + "window.opener.document.forms(0).submit();"

    '            'msg = msg + "</Script>"
    '            'Response.Write(msg)
    '        Else
    '            If Me.SessionLoanStatus.Trim.ToUpper = "PAID" Then
    '                l_string_message = "Reverse the request through Void Disbursement->Void Loan->Reverse Screen to cancel it."
    '            ElseIf Me.SessionLoanStatus.Trim.ToUpper = "SUSPND" Then
    '                l_string_message = "Cannot cancel suspended loan."
    '            End If
    '            MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", l_string_message, MessageBoxButtons.Stop)
    '            Exit Sub
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
    Private Sub ButtonShowDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonShowDetails.Click
        Try
            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            'End : YRS 5.0-940
            If Me.SessionLoanStatus.Trim.ToUpper = "SUSPND" Then
                If Me.TextBoxUnSuspendDate.Text <> "" Then
                    Session("ds_closeloandetails") = Nothing
                    Session("l_string_message") = Nothing

                    Session("UnsuspendDate") = Me.TextBoxUnSuspendDate.Text.Trim

                    Me.LoadLoanCloseDetails(Me.SessionLoanRequestId, "Unsuspend")
                ElseIf Me.TextBoxUnSuspendDate.Text = "" Then
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_DISCHARGE_DATE_IS_REQUIRED_REAMORTIZING)
                    Exit Sub
                End If
            Else
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_NOT_SUSPENDED)
                Exit Sub
            End If


        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "ButtonShowDetails_Click", ex)
        End Try
    End Sub
    'Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
    '    Try
    '        Dim msg As String
    '        msg = msg + "<Script Language='JavaScript'>"

    '        msg = msg + "self.close();"

    '        msg = msg + "</Script>"
    '        Response.Write(msg)
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
    Public Sub ButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOkClose.Click, ButtonOkDefault.Click, ButtonOkPayOffLoan.Click, ButtonOkSuspendLoan.Click, ButtonOkUnsuspendLoan.Click, ButtonOkOffset.Click
        Try
            Me.ClearSessions()
            'AA:2014.09.11: BT:2628:YRS 5.0-2405 -Added to open the Find Info page after clicking close button
            Session("Page") = "LoanOptions"
            Response.Redirect("FindInfo.aspx?Name=LoanOptions", False)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "ButtonClick", ex)
        End Try
    End Sub
#End Region
#Region "Functions"
    Private Function IsValidCancel(ByVal parameterLoanRequestId As Integer, ByVal parameterLoanStatus As String)
        Try
            Dim l_flag_IsValidCancel As Boolean
            Dim l_int_output As Integer
            l_flag_IsValidCancel = False
            l_int_output = 0
            'COde to validate cancel....
            l_int_output = YMCARET.YmcaBusinessObject.LoanInformationBOClass.IsLoanCancelValid(parameterLoanRequestId, parameterLoanStatus)
            If l_int_output = 1 Then
                l_flag_IsValidCancel = True
            End If
            Return l_flag_IsValidCancel

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "IsValidCancel", ex)
        End Try
    End Function
    Private Function IsValidSuspension() As Boolean
        Try
            Dim l_isvalidsuspension As Integer
            l_isvalidsuspension = YMCARET.YmcaBusinessObject.LoanInformationBOClass.IsValidSuspension(Me.SessionLoanRequestId)
            If l_isvalidsuspension = 1 Then
                Me.LabelLoanSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_SUSPEND)
                Me.ButtonSuspendLoan.Enabled = True
                Me.IsParticipantOnMilitaryLeave = True ' SC | 2020.04.10 | YRS-AT-4852 | If Loan suspension due to Military leave
                Return True
            ElseIf l_isvalidsuspension = 2 Then
                Me.LabelLoanSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_SUSPENDED_VERIFY_TERM_REASON)
                Me.ButtonSuspendLoan.Enabled = False
                Return False
                ' Start | SC | 2020.04.10 | YRS-AT-4852 | If Loan suspension due to COVID
            ElseIf l_isvalidsuspension = 3 Then
                Me.LabelLoanSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_SUSPEND)
                Me.ButtonSuspendLoan.Enabled = True
                Me.IsParticipantOnMilitaryLeave = False
                Return True
                ' End | SC | 2020.04.10 | YRS-AT-4852 | If Loan suspension due to COVID
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "IsValidSuspension", ex)
        End Try
    End Function
    Private Function CancelLoan(ByVal parameterRequestField As String) As String
        Try
            Dim l_string_message As String = ""
            Dim l_string_reasonCode As String = "" ' SC | 2020.04.10 | YRS-AT-4852 | Capture Loan suspension reason
            Select Case (parameterRequestField)
                Case "SUSPND"
                    ' Start | SC | 2020.04.10 | YRS-AT-4852 | Capture Loan suspension reason
                    If (DropDownReasonsForLoanSuspension.SelectedIndex > -1) Then
                        l_string_reasonCode = DropDownReasonsForLoanSuspension.SelectedItem.Value
                    End If
                    'If Me.CancelLoanUpdateAmortization("SUSPND", "SUSPND", Me.SessionLoanRequestId, Me.TextBoxSuspendDate.Text, " ", " ", Nothing, 0, 0) = False Then
                    If Me.CancelLoanUpdateAmortization("SUSPND", "SUSPND", Me.SessionLoanRequestId, Me.TextBoxSuspendDate.Text, " ", " ", Nothing, 0, 0, l_string_reasonCode) = False Then
                        l_string_message = ""
                    Else
                        'Added and Commented By Ashutosh Patil as on 20-01-2007 for YREN - 3023
                        'l_string_message = "Loan request has been suspended. Note:Report is missing."
                        l_string_message = GetMessageByTextMessageNo(MESSAGE_LOAN_HAS_SUSPENDED)
                        Call GenerateReportsForLoan("SUSPEND", l_string_reasonCode)
                    End If
                    ' End | SC | 2020.04.10 | YRS-AT-4852 | Capture Loan suspension reason
                Case "UNSPND"
                    If Me.CancelLoanUpdateAmortization("UNSPND", "UNSPND", Me.SessionLoanRequestId, " ", Me.TextBoxUnSuspendDate.Text, " ", Nothing, CType(Session("PayOffAmount"), Double)) = False Then
                        l_string_message = ""
                    Else
                        l_string_message = GetMessageByTextMessageNo(MESSAGE_LOAN_HAS_UNSUSPENDED)
                    End If
                Case "PAYOFF"
                    If Me.CancelLoanUpdateAmortization("CLOSED", "PAYOFF", Me.SessionLoanRequestId, " ", " ", " ", Nothing, 0) = False Then
                        l_string_message = ""
                    Else
                        'l_string_message = "Loan request has been suspended. Note:Report is missing."
                        'Added and Commented By Ashutosh Patil as on 20-01-2007 for YREN - 3023
                        'l_string_message = "Loan has been Closed. Note:Report is missing."
                        l_string_message = GetMessageByTextMessageNo(MESSAGE_LOAN_HAS_CLOSED)
                        Call GenerateReportsForLoan("PAYOFF")
                    End If
                Case "CLOSED"
                    'BhavnaS 02 July 2012   YRS 5.0-734 : Add code to avoid possible double-click
                    If Session("terminate_re-amortize") = "TerminateReamortize" Then
                        Dim l_datatable_CreateNewRequest As DataTable
                        Dim l_dataset_CreateNewRequest As New DataSet
                        'Added by Ashish 17-Mar-2009 
                        Dim l_str_NewYmcaId As String = String.Empty
                        Dim l_intNewLoanNumber As Integer = 0
                        Dim l_strMessage As String = String.Empty
                        l_datatable_CreateNewRequest = Me.MakeRequestDataTable()
                        'Added by Ashish 17-Mar-2009
                        If Not l_datatable_CreateNewRequest Is Nothing Then
                            If l_datatable_CreateNewRequest.Rows.Count > 0 Then
                                l_str_NewYmcaId = l_datatable_CreateNewRequest.Rows(0)("guiYmcaid").ToString()
                            End If
                        End If
                        l_dataset_CreateNewRequest.Tables.Add(l_datatable_CreateNewRequest)
                        l_dataset_CreateNewRequest.Tables(0).TableName = "LoanNewRequests"
                        If Me.CancelLoanUpdateAmortization("TERM", "CLOSED", Me.SessionLoanRequestId, " ", " ", " ", l_dataset_CreateNewRequest, 0) = False Then
                            l_string_message = ""
                        Else
                            l_string_message = GetMessageByTextMessageNo(MESSAGE_LOAN_HAS_TERMINATED_NEW_AMORTIZED)
                            'Added by Ashish Srivastava 17-Mar-2009 ,Start

                            'Get New Loan Number
                            l_intNewLoanNumber = GetNewReamortizedLoanNumber(Me.SessionOrigLoanNo)
                            If l_intNewLoanNumber <> 0 And l_str_NewYmcaId <> String.Empty Then
                                'Call GenerateReamortizeReport
                                l_strMessage = GenerateReamortizeReport(Me.SessionOrigLoanNo, l_intNewLoanNumber, Me.SessionYmcaId, l_str_NewYmcaId, Me.SessionPersonID)
                                If l_strMessage <> String.Empty Then
                                    l_string_message &= vbCrLf & l_strMessage
                                End If

                            End If

                            'Added by Ashish Srivastava 17-Mar-2009 ,End

                        End If
                    End If
                    Session("terminate_re-amortize") = Nothing

                Case "DEFALT"

                    'YMCARET.YmcaBusinessObject.LoanInformationBOClass.InsertDisbursementsEarlyLoanPayOff(Me.SessionPersonID, Me.SessionLoanRequestId)
                    'BS:2012.07.02: YRS 5.0-734:to avoid duplicate loan request  
                    If Session("DefaultLoan") = "DefaultLoan" Then

                        If Me.TextBoxDefaultDate.Text.Trim.Length > 0 Then
                            Session("DefaultDate") = Me.TextBoxDefaultDate.Text
                        End If
                        If Me.CancelLoanUpdateAmortization("DEFALT", "DEFALT", Me.SessionLoanRequestId, " ", " ", Me.TextBoxDefaultDate.Text, Nothing, 0) = False Then
                            l_string_message = ""
                        Else
                            l_string_message = GetMessageByTextMessageNo(MESSAGE_LOAN_HAS_DEFAULTED)
                            Call GenerateReportsForLoan("DEFAULT")
                        End If
                    End If
                    Session("DefaultLoan") = Nothing
                Case "OFFSET"
                    Dim intOffseteventReason As Integer
                    If Session("OffsetLoan") = "OffsetLoan" Then
                        If ViewState("OffseteventReason") IsNot Nothing Then
                            intOffseteventReason = ViewState("OffseteventReason")
                        End If
                        'Start:AA:10.26.2015  YRS-AT-2533 : changed to assign defaut date as a parameter to report while offseting a loan
                        ''Start:AA:26.06.2015 BT:2900 YRS 5.0-2533:Added below code to get report it takes offset as input for the same report of which is used for open default
                        'If Me.TextBoxOffsetDate.Text.Trim.Length > 0 Then
                        '    Session("DefaultDate") = Me.TextBoxOffsetDate.Text
                        'End If
                        ''End:AA:26.06.2015 BT:2900 YRS 5.0-2533:Added below code to get report it takes offset as input for the same report of which is used for open default
                        If Offsetevent_Defaultdate IsNot Nothing Then
                            Session("DefaultDate") = Offsetevent_Defaultdate
                        End If
                        'End:AA:10.26.2015  YRS-AT-2533 : changed to assign defaut date as a parameter to report while offseting a loan

                        If Me.CancelLoanUpdateAmortization("OFFSET", "OFFSET", Me.SessionLoanRequestId, " ", " ", Me.TextBoxOffsetDate.Text, Nothing, 0, intOffseteventReason) Then
                            l_string_message = GetMessageByTextMessageNo(MESSAGE_LOAN_HAS_OFFSET)
                            If Not intOffseteventReason = MESSAGE_LOAN_OFFSET_DEATH Then 'Manthan Rajguru | 2015.11.10 | YRS-AT-2669 | Added validation to stop generating letter and email.
                                Call GenerateReportsForLoan("OFFSET") 'AA:26.06.2015 BT:2900 YRS 5.0-2533:Modified letter for default and ofset loans
                            End If
                        Else
                            l_string_message = ""
                        End If
                    End If
                    Session("OffsetLoan") = Nothing

            End Select
            Return l_string_message
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "CancelLoan", ex)
        End Try
    End Function
    Private Function CancelLoanUpdateAmortization(ByVal parameterLoanStatus As String, ByVal parameterRequestField As String, ByVal parameterLoanRequestId As Integer, ByVal parameterSuspendDate As String, ByVal parameterUnSuspendDate As String, ByVal parameterDefaultDate As String, ByVal parameterCreateNewRequest As DataSet, ByVal parameterPayOffAmount As Double, Optional ByVal intOffseteventReason As Integer = 0, Optional ByVal strSuspendReasonCode As String = "") As Boolean
        Try
            Me.ClearLabel()
            YMCARET.YmcaBusinessObject.LoanInformationBOClass.CancelLoanUpdateAmortization(parameterLoanStatus, parameterRequestField, parameterLoanRequestId, parameterSuspendDate, parameterUnSuspendDate, parameterDefaultDate, parameterCreateNewRequest, parameterPayOffAmount, intOffseteventReason, strSuspendReasonCode)
            Me.DisableButtons()
            ViewState("Cancel") = True
            Session("parameterRequestField") = parameterRequestField
            PreviousLoanStatus = SessionLoanStatus 'AA:11.09.2015 YRS-AT-2660 Added to store previous loan status befre update to check whether loan is getting pay-off from PAID or DEFALT
            Me.SessionLoanStatus = parameterLoanStatus
            'Me.SetTextinLabels(parameterRequestField)
            Return True
        Catch SQLEx As SqlClient.SqlException
            HelperFunctions.ShowMessageToUser(SQLEx.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "CancelLoanUpdateAmortization", SQLEx)
            Return False
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "CancelLoanUpdateAmortization", ex)
        End Try
    End Function
    Private Function LoadLoanCloseDetails(ByVal parameterLoanRequestId As Integer, ByVal parameterCalledBy As String) As Boolean
        Try
            Dim ds_closeloandetails As New DataSet
            Dim activeYmcaDetails As New DataSet 'MMR | 2019.01.23 | YRS-AT-4118 | Declared local variable to store YMCA details
            Dim intMessageNo As Integer
            Dim parameterStatus As Integer
            Dim l_string_UnsuspendDate As String = ""
            'START: MMR | 2019.01.23 | YRS-AT-4118 | Declared local variable
            Dim rowYmca As DataRow()
            Dim rowPayDates As DataRow()
            Dim ymcaId As String
            Dim firstPaymentDate As String = ""
            'END: MMR | 2019.01.23 | YRS-AT-4118 | Declared local variable
            intMessageNo = 0
            If Me.SessionLoanStatus.Trim.ToUpper = "PAID" Then
                parameterStatus = 0
            ElseIf Me.SessionLoanStatus.Trim.ToUpper = "SUSPND" Or Me.SessionLoanStatus.Trim.ToUpper = "UNSPND" Then
                parameterStatus = 1
            End If
            If Session("UnsuspendDate") Is Nothing Then
                l_string_UnsuspendDate = String.Empty
            Else
                l_string_UnsuspendDate = Session("UnsuspendDate")
            End If

            'START: MMR | 2019.01.23 | YRS-AT-4118 | Getting Active YMCA Details based on which re-amortization details will be fetched
            activeYmcaDetails = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetActiveYmcaDetails(parameterLoanRequestId)
            If HelperFunctions.isNonEmpty(activeYmcaDetails) Then
                Me.ActiveYmcaDetails = activeYmcaDetails
                If HelperFunctions.isNonEmpty(activeYmcaDetails.Tables("EmployeeActiveYMCA")) Then
                    rowYmca = activeYmcaDetails.Tables("EmployeeActiveYMCA").Select(String.Format("IsDefault = {0}", 1))
                    If rowYmca.Length > 0 Then
                        ymcaId = Convert.ToString(rowYmca(0)("YMCAID"))
                    End If
                End If

                If HelperFunctions.isNonEmpty(activeYmcaDetails.Tables("FirstPayDates")) Then
                    rowPayDates = activeYmcaDetails.Tables("FirstPayDates").Select(String.Format("IsDefaultDate = {0}", 1))
                    If rowPayDates.Length > 0 Then
                        firstPaymentDate = Convert.ToString(rowPayDates(0)("PayDatesForDisplay"))
                    End If
                End If
            End If

            If Me.SessionLoanStatus.Trim.ToUpper = "SUSPND" AndAlso String.IsNullOrEmpty(ymcaId) Then
                ymcaId = Me.SessionYmcaId
            End If
            'END: MMR | 2019.01.23 | YRS-AT-4118 | Getting Active YMCA Details based on which re-amortization details will be fetched
            ds_closeloandetails = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetLoanDetailsForCloseLoan(parameterLoanRequestId, parameterStatus, l_string_UnsuspendDate, ymcaId, firstPaymentDate, intMessageNo) 'MMR | 2019.01.23 | YRS-AT-4118 | Added parameter ymcaid and first payment date based on which reamortization amount/months will be computed and fetched for display in UI
            Session("l_string_message") = intMessageNo

            Session("ds_closeloandetails") = ds_closeloandetails
            If intMessageNo = 0 Then
                If Not ds_closeloandetails Is Nothing Then
                    If Not ds_closeloandetails.Tables("ReamortizedAmount") Is Nothing Then
                        If ds_closeloandetails.Tables("ReamortizedAmount").Rows.Count > 0 Then
                            If Not IsDBNull(ds_closeloandetails.Tables("ReamortizedAmount").Rows(0).Item("RemainingReamortizedAmount")) Then
                                If parameterCalledBy = "Automatic" Then
                                    Session("PayOffAmount") = ds_closeloandetails.Tables("ReamortizedAmount").Rows(0).Item("RemainingReamortizedAmount")
                                Else
                                    Me.TextboxRemainingReamorizeAmount.Text = ds_closeloandetails.Tables("ReamortizedAmount").Rows(0).Item("RemainingReamortizedAmount")
                                    Me.TextBoxRemainingAmount.Text = ds_closeloandetails.Tables("ReamortizedAmount").Rows(0).Item("RemainingReamortizedAmount")
                                End If

                            End If
                        End If
                    End If
                End If

                If Not ds_closeloandetails.Tables("ReamortizedMonths") Is Nothing Then
                    If ds_closeloandetails.Tables("ReamortizedMonths").Rows.Count > 0 Then
                        If Not IsDBNull(ds_closeloandetails.Tables("ReamortizedMonths").Rows(0).Item("RemainingReamortizedMonths")) Then
                            If parameterCalledBy <> "Automatic" Then
                                Me.TextboxRemainingReamorizeMonths.Text = ds_closeloandetails.Tables("ReamortizedMonths").Rows(0).Item("RemainingReamortizedMonths")
                                Me.TextBoxRemainingMonths.Text = ds_closeloandetails.Tables("ReamortizedMonths").Rows(0).Item("RemainingReamortizedMonths")
                            End If

                        End If
                    End If
                End If

                'Start -- Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Code to display Active YMCA in Dropdownlist
                Me.DropDownActiveYMCA.Enabled = False 'PK | 01.15.2019 | YRS-AT-2573 | Setting YMCA dropdown as disabled
                'START: MMR | 2019.01.23 | YRS-AT-4118 | Replaced dataset reference as data being fetched from different source 'old - ds_closeloandetails, new - activeYmcaDetails'
                Me.DropDownActiveYMCA.DataSource = Nothing
                If Me.DropDownActiveYMCA.Items.Count > 0 Then
                    Me.DropDownActiveYMCA.Items.Clear()
                End If
                Me.DropDownActiveYMCA.DataBind()
                If HelperFunctions.isNonEmpty(activeYmcaDetails) Then
                    If HelperFunctions.isNonEmpty(activeYmcaDetails.Tables("EmployeeActiveYMCA")) Then
                        Me.DropDownActiveYMCA.DataSource = activeYmcaDetails.Tables("EmployeeActiveYMCA")
                        'END: MMR | 2019.01.23 | YRS-AT-4118 | Replaced dataset reference as data being fetched from different source 'old - ds_closeloandetails, new - activeYmcaDetails'
                        Me.DropDownActiveYMCA.DataTextField = "DisplayText"
                        Me.DropDownActiveYMCA.DataValueField = "YMCAID"
                        Me.DropDownActiveYMCA.DataBind()

                        'START: PK | 01.15.2019 | YRS-AT-2573 | Enabling the YMCA dropdown and selecting 1st Ymca as selected
                        Me.DropDownActiveYMCA.Enabled = IIf(Me.DropDownActiveYMCA.Items.Count = 1, False, True)
                        DropDownActiveYMCA.SelectedIndex = 0
                        'END: PK | 01.15.2019 | YRS-AT-2573 | Enabling the YMCA dropdown and selecting 1st Ymca as selected
                    End If
                    'End If
                    'START: PK | 01.15.2019 | YRS-AT-2573 | Dropped the "---Select--" option menu from Ymca list
                    'Dim li As ListItem = New ListItem("---Select---", "")
                    'Me.DropDownActiveYMCA.SelectedIndex = IIf(Me.DropDownActiveYMCA.Items.Count = 2, 1, 0)
                    'Me.DropDownActiveYMCA.Enabled = IIf(Me.DropDownActiveYMCA.Items.Count = 2, False, True)
                    'END: PK | 01.15.2019 | YRS-AT-2573 | Dropped the "---Select--" option menu from Ymca list
                    'End -- Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Code to display Active YMCA in Dropdownlist

                    'START: PPP | 01.22.2019 | YRS-AT-2573 | Populating list for first payment dates 
                    If HelperFunctions.isNonEmpty(activeYmcaDetails.Tables("FirstPayDates")) Then
                        Me.PopulateFirstPaymentDateDropDown(activeYmcaDetails.Tables("FirstPayDates"))
                    End If
                End If
                'END: PPP | 01.22.2019 | YRS-AT-2573 | Populating list for first payment dates 

                'START: SR | 2019.01.29 | YRS-AT-2920 | Get unfunded loan exist warning message. 
                If HelperFunctions.isNonEmpty(ds_closeloandetails.Tables("UnfundedLoanPayment")) Then
                    Me.IsUnfundedPaymentExist = Convert.ToBoolean(ds_closeloandetails.Tables("UnfundedLoanPayment").Rows(0).Item("IsUnfunded"))
                    Me.UnfundedPaymentWarningMessage = GetMessageByTextMessageNo(MESSAGE_LOAN_UNRECEIVED_PAYMENT_EXISTS)
                Else
                    Me.IsUnfundedPaymentExist = False
                    Me.UnfundedPaymentWarningMessage = ""
                End If
                'END: SR | 2019.01.29 | YRS-AT-2920 | Get unfunded loan exist warning message. 

                Return True
            Else
                If parameterCalledBy = "UNSPND" Then
                    Me.ButtonUnSuspendLoan.Enabled = False
                    If intMessageNo <> 9590 Then 'MMR | 2019.02.14 | YRS-AT-2920 | Avoid showind unfunded payment validation message in red text after sucessfully unsuspending the loan
                        Me.LabelLoanUnSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_UNSUSPENDED_BECAUSE) & " " & GetMessageByTextMessageNo(intMessageNo) & ""
                    End If
                End If
                Me.ButtonCloseLoan.Enabled = False
                'START: MMR | 2019.02.14 | YRS-AT-2920 | Clearing dropdown values if error is there
                'Clearing YMCA Dropdown
                Me.DropDownActiveYMCA.DataSource = Nothing
                If Me.DropDownActiveYMCA.Items.Count > 0 Then
                    Me.DropDownActiveYMCA.Items.Clear()
                End If
                Me.DropDownActiveYMCA.DataBind()

                'Clearing First payment Dropdown
                Me.DropdownFirstPaymentDate.DataSource = Nothing
                If Me.DropdownFirstPaymentDate.Items.Count > 0 Then
                    Me.DropdownFirstPaymentDate.Items.Clear()
                End If
                Me.DropdownFirstPaymentDate.DataBind()
                'END: MMR | 2019.02.14 | YRS-AT-2920 | Clearing dropdown values if error is there

                Me.LabelCloseLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_CANNOT_CLOSED_BECAUSE) & " " & GetMessageByTextMessageNo(intMessageNo) & ""
                HelperFunctions.ShowMessageToUser(intMessageNo)
                Return False
            End If


        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "LoadLoanCloseDetails", ex)
        End Try
    End Function
    'Private Function EnableDefaultRadioButtons(ByVal parameterLoanRequestId As Integer) As Boolean
    '    Try
    '        Dim l_bitvalid As Integer
    '        Dim l_flag As Boolean
    '        Dim l_string_message As String
    '        l_flag = False
    '        If Session("l_bitvalid") Is Nothing Then
    '            l_bitvalid = YMCARET.YmcaBusinessObject.LoanInformationBOClass.IsLoanUnpaidFromCureperiod(parameterLoanRequestId, l_string_message)
    '            Session("l_bitvalid") = l_bitvalid
    '            Session("stringMessage") = l_string_message
    '            If l_string_message = "" Then
    '                Me.ButtonDefaultLoan.Enabled = True
    '            End If
    '        ElseIf Not Session("l_bitvalid") Is Nothing Then
    '            l_bitvalid = CType(Session("l_bitvalid"), Integer)
    '            l_string_message = CType(Session("stringMessage"), String)
    '        End If
    '        If l_string_message = "" Then
    '            If l_bitvalid = 0 Then
    '                '    Me.RadiobuttonDefaultLoanCompulsoryNo.Enabled = True
    '                '    Me.RadiobuttonDefaultLoanCompulsoryYes.Enabled = True
    '                '    Me.RadiobuttonDefaultLoanStatusNo.Enabled = True
    '                '    Me.RadiobuttonDefaultLoanStatusYes.Enabled = True
    '                l_flag = True
    '            Else
    '                '    Me.RadiobuttonDefaultLoanCompulsoryNo.Enabled = False
    '                '    Me.RadiobuttonDefaultLoanCompulsoryYes.Enabled = False
    '                '    Me.RadiobuttonDefaultLoanStatusNo.Enabled = True
    '                '    Me.RadiobuttonDefaultLoanStatusYes.Enabled = True
    '                l_flag = False
    '            End If

    '        ElseIf l_string_message <> "" Then
    '            'Me.RadiobuttonDefaultLoanCompulsoryNo.Enabled = False
    '            'Me.RadiobuttonDefaultLoanCompulsoryYes.Enabled = False
    '            'Me.RadiobuttonDefaultLoanStatusNo.Enabled = False
    '            'Me.RadiobuttonDefaultLoanStatusYes.Enabled = False
    '            Me.ButtonDefaultLoan.Enabled = False
    '            Me.LabelDefaultLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_CANNOT_BE_DEFAULTED_VERIFY_RECORDS)
    '            Exit Function
    '        End If

    '        Return l_flag
    '    Catch ex As Exception
    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
    '        HelperFunctions.LogException("LoanOptions_" + "EnableDefaultRadioButtons", ex)
    '    End Try
    'End Function
    'Private Function CloseLoan()
    '    Try
    '        Dim l_string_message As String
    '        l_string_message = ""
    '        If Me.SessionLoanStatus.Trim.ToUpper = "PAID" Then
    '            l_string_message = "The loan has been closed.Please create a new loan request."
    '            Me.CancelLoanUpdateAmortization("CLOSED", "CLOSED", Me.SessionLoanRequestId, " ", " ", l_string_message)
    '        Else
    '            MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Cannot close the loan.", MessageBoxButtons.Stop)
    '            Exit Function
    '        End If
    '    Catch
    '        Throw
    '    End Try
    'End Function
    Private Function IsActiveMultipleYMCA(ByVal parameterLoanRequestId As Integer)
        Try
            Dim l_int_count As Integer
            l_int_count = 0
            'Code to check whether a person is having more than one active employment in Ymcas
            l_int_count = YMCARET.YmcaBusinessObject.LoanInformationBOClass.IsActiveMultipleYMCA(parameterLoanRequestId)
            If l_int_count = 0 Then
                Return False
            ElseIf l_int_count > 0 Then
                Return True
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "IsActiveMultipleYMCA", ex)
        End Try

    End Function
    Private Function IsPayOffValid() As Boolean
        Try
            Dim l_int_count As Integer = 0
            l_int_count = YMCARET.YmcaBusinessObject.LoanInformationBOClass.IsLoanPayOffValid(Me.SessionLoanRequestId)
            If l_int_count = 0 Then
                Me.ButtonPayOffLoan.Enabled = True
                Me.LabelPayOffLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_PAYOFF_BE_DONE)
                Return True
                'AA:2015.08.05 YRS 5.0-2441:: Added below lines to show  the unfunded payments exists for pay-off
            ElseIf l_int_count = -1 Then
                Me.ButtonPayOffLoan.Enabled = False
                Me.LabelPayOffLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_PAYOFF_UNFUNDED)
                Return True
            ElseIf l_int_count > 0 Then
                Me.ButtonPayOffLoan.Enabled = False
                Me.LabelPayOffLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_PAYOFF_VERIFY_AMORT_RECORDS)
                Return False
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "IsPayOffValid", ex)
        End Try
    End Function
    Private Function LoadSuspenddate(ByVal parameterLoanRequestId As Integer)
        Try
            Dim l_string_suspenddate As String
            l_string_suspenddate = ""
            l_string_suspenddate = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetLoanDetailsForLoanOptions(parameterLoanRequestId)
            If l_string_suspenddate <> "" Then
                Me.TextBoxSuspendDate.Text = Convert.ToDateTime(l_string_suspenddate).Date
            Else
                Me.TextBoxSuspendDate.Text = ""
            End If
            If Me.SessionLoanStatus = "SUSPND" Then
                Me.RadioButtonLoanSuspensionNo.Enabled = False
                Me.RadioButtonLoanSuspensionYes.Enabled = False
                Me.TextBoxSuspendDate.Enabled = False
            Else
                Me.RadioButtonLoanSuspensionNo.Enabled = True
                Me.RadioButtonLoanSuspensionYes.Enabled = True
                Me.TextBoxSuspendDate.Enabled = True
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "LoadSuspenddate", ex)
        End Try
    End Function
    Private Function DisableButtons()
        Try
            'Me.ButtonCancelLoan.Enabled = False
            Me.ButtonCloseLoan.Enabled = False
            'Me.ButtonDefaultLoan.Enabled = False
            Me.ButtonPayOffLoan.Enabled = False
            Me.ButtonSuspendLoan.Enabled = False
            'Me.ButtonUnSuspendLoan.Enabled = False
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "DisableButtons", ex)
        End Try
    End Function
    Private Function SetTextinLabels(ByVal parameterRequestField As String)
        Try

            Select Case (parameterRequestField)
                'Case "REVE"
                '    'LabelCancelLoan.Text = "You cannot cancel the loan as it is already cancelled."
                '    LabelLoanSuspensionStatus.Text = "You cannot suspend the loan as it is cancelled."
                '    LabelLoanUnSuspensionStatus.Text = "You cannot unsuspend the loan as it is cancelled."
                '    LabelPayOffLoan.Text = "Loan PayOff cannot be done as the loan is cancelled."
                '    LabelDefaultLoan.Text = "You cannot default the loan as it is cancelled."
                '    LabelCloseLoan.Text = "You cannot close the loan as it is cancelled."
                Case "SUSPND"
                    'LabelCancelLoan.Text = "You cannot cancel the loan as it is suspended."
                    LabelLoanSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_SUSPENDED_ALREADY_SUSPENDED)
                    'LabelLoanUnSuspensionStatus.Text = "You cannot unsuspend the loan as it is cancelled."
                    'LabelPayOffLoan.Text = "Loan PayOff cannot be done as the loan is suspended."
                    'LabelDefaultLoan.Text = "You cannot default the loan as it is suspended."
                    LabelCloseLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_CLOSED_SUSPENDED)
                Case "UNSPND"
                    'LabelCancelLoan.Text = "You cannot cancel the loan as it is unsuspended."
                    LabelLoanSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_SUSPENDED_UNSUSPENDED)
                    LabelLoanUnSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_UNSUSPENDED_ALREADY_UNSUSPENDED)
                    LabelPayOffLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_PAYOFF_CANNOT_AS_LOAN_UNSUSPEND)
                    LabelDefaultLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_DEFAULTED_UNSUSPEND)
                    LabelCloseLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_CLOSED_UNSUSPENDED)
                Case "CLOSED"
                    'LabelCancelLoan.Text = "You cannot cancel the loan as it is closed."
                    LabelLoanSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_SUSPENDED_CLOSED)
                    LabelLoanUnSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_UNSUSPENDED_CLOSED)
                    LabelPayOffLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_PAYOFF_CANNOT_AS_LOAN_CLOSED)
                    LabelDefaultLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_DEFAULTED_CLOSED)
                    LabelCloseLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_CLOSED_ALREADY_CLOSED)
                Case "PAYOFF"
                    'LabelCancelLoan.Text = "You cannot cancel the loan as the loan payoff is done."
                    LabelLoanSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_SUSPENDED_PAYOFF)
                    LabelLoanUnSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_UNSUSPENDED_PAYOFF)
                    LabelPayOffLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_PAYOFF_CANNOT_AS_LOAN_PAYOFF)
                    LabelDefaultLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_DEFAULTED_PAYOFF)
                    LabelCloseLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_CLOSED_PAYOFF)
                Case "DEFALT"
                    'LabelCancelLoan.Text = "You cannot cancel the loan as the loan is default."
                    LabelLoanSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_SUSPENDED_DEFAULTED)
                    LabelLoanUnSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_UNSUSPENDED_DEFAULTED)
                    LabelPayOffLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_PAYOFF_CANNOT_AS_LOAN_DEFAULTED)
                    LabelDefaultLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_DEFAULTED_ALREADY_DEFAULTED)
                    LabelCloseLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_CLOSED_DEFAULTED)
            End Select
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "SetTextinLabels", ex)
        End Try
    End Function
    'Private Function SetTextinLabelsOnLoad(ByVal parameterRequestStatus As String)
    '    Try

    '        Select Case (parameterRequestStatus)
    '            Case "SUSPND"
    '                LabelLoanSuspensionStatus.Text = "You cannot suspend the loan as it is already suspended."

    '                'LabelPayOffLoan.Text = "Loan PayOff can be done."
    '                LabelDefaultLoan.Text = "You can default the loan."
    '                LabelCloseLoan.Text = "You cannot close the loan as it is suspended."
    '            Case "PAID"
    '                'LabelLoanSuspensionStatus.Text = "You can suspend the loan."
    '                LabelLoanUnSuspensionStatus.Text = "You cannot unsuspend the loan."
    '                'LabelPayOffLoan.Text = "Loan PayOff can be done."

    '        End Select
    '    Catch
    '        Throw
    '    End Try
    'End Function
    Private Function SetTabStrip(ByVal parameterLoanStatus As String)
        Try
            ' Start | SC | 2020.04.10 | YRS-AT-4852 | Modfied logic to set tabstip based on valid suspension and if Participant is on military leave
            'Dim l_flag_suspnd As Boolean
            'l_flag_suspnd = False
            ' End | SC | 2020.04.10 | YRS-AT-4852 | Modfied logic to set tabstip based on valid suspension and if Participant is on military leave
            Select Case (parameterLoanStatus)
                Case "PAID"
                    'condition to check the enabling/disabling of suspnd loan status
                    If Me.IsValidSuspension = True Then
                        Me.LoanOptionsTabStrip.Items(m_int_const_Suspend).Enabled = True
                        Me.LoanOptionsTabStrip.SelectedIndex = 0
                        Me.LoanOptionsMultiPage.SelectedIndex = Me.LoanOptionsTabStrip.SelectedIndex
                        Me.LabelLoanSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_SUSPEND)
                        ' Start | SC | 2020.04.10 | YRS-AT-4852 | SC | 2020.04.10 | YRS-AT-4852 | Populate dropdown list with reasons of loan suspension
                        'l_flag_suspnd = True
                        'ElseIf Me.IsValidSuspension = False Then
                        PopulateReasonForLoanSuspension()
                    Else
                        ' End | SC | 2020.04.10 | YRS-AT-4852 | SC | 2020.04.10 | YRS-AT-4852 | Populate dropdown list with reasons of loan suspension
                        Me.LoanOptionsTabStrip.Items(m_int_const_Suspend).Enabled = False
                    End If
                    'For paid loans Unsuspnd loans will be disabled
                    Me.LoanOptionsTabStrip.Items(m_int_const_Unsuspend).Enabled = False

                    Me.LabelCloseLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_TERMINATED)
                    ' Start | SC | 2020.04.10 | YRS-AT-4852 | Enable only Suspend tab if Participant is on military leave
                    'If l_flag_suspnd = True Then
                    If IsParticipantOnMilitaryLeave = True Then
                        ' End | SC | 2020.04.10 | YRS-AT-4852 | Enable only Suspend tab if Participant is on military leave
                        Me.LoanOptionsTabStrip.SelectedIndex = 0
                        Me.LoanOptionsMultiPage.SelectedIndex = Me.LoanOptionsTabStrip.SelectedIndex
                        'AA:2015.08.05 YRS 5.0-2441:: Added below lines to hide tabs for participants who are eligible for suspend
                        Me.LoanOptionsTabStrip.Items(m_int_const_PayOff).Enabled = False
                        Me.LoanOptionsTabStrip.Items(m_int_const_Terminate).Enabled = False
                        Me.LoanOptionsTabStrip.Items(m_int_const_Default).Enabled = False
                        Me.LoanOptionsTabStrip.Items(m_int_const_Offset).Enabled = False
                        ' Start | SC | 2020.04.10 | YRS-AT-4852 | Enable only Suspend tab if Participant is on military leave
                        'ElseIf l_flag_suspnd = False Then
                    Else
                        ' End | SC | 2020.04.10 | YRS-AT-4852 | Enable only Suspend tab if Participant is on military leave
                        Me.LoanOptionsTabStrip.SelectedIndex = 2
                        Me.LoanOptionsMultiPage.SelectedIndex = Me.LoanOptionsTabStrip.SelectedIndex
                        'l_flag_selindex = True
                    End If
                    'Pay off enable/disable will require validation
                    If Me.IsPayOffValid = True Then
                        Me.LoanOptionsTabStrip.Items(m_int_const_PayOff).Enabled = True

                        'Added by Paramesh K. on July 30th 2008
                        'For selecting the Payoff Loan tab as default if the payments are all received
                        '**************
                        Me.LoanOptionsTabStrip.SelectedIndex = 3
                        Me.LoanOptionsMultiPage.SelectedIndex = Me.LoanOptionsTabStrip.SelectedIndex
                        '**************

                        'Me.LabelPayOffLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_PAYOFF_BE_DONE)
                        Me.LoanOptionsTabStrip.Items(m_int_const_Terminate).Enabled = False
                        Me.LoanOptionsTabStrip.Items(m_int_const_Default).Enabled = False
                        Me.LoanOptionsTabStrip.Items(m_int_const_Offset).Enabled = False
                        ' Start | SC | 2020.04.10 | YRS-AT-4852 | Enable only Suspend tab if Participant is on military leave
                        'ElseIf Me.IsPayOffValid = False And l_flag_suspnd = False Then
                    ElseIf Me.IsPayOffValid = False And IsParticipantOnMilitaryLeave = False Then
                        ' End | SC | 2020.04.10 | YRS-AT-4852 | Enable only Suspend tab if Participant is on military leave
                        Me.LoanOptionsTabStrip.Items(m_int_const_PayOff).Enabled = False
                        'Treminate/Re-amortize loan will be enabled for paid 
                        Me.LoanOptionsTabStrip.Items(m_int_const_Terminate).Enabled = True
                        If IsLoanUnpaidFromCureperiod(SessionLoanRequestId) Then
                            Me.LabelDefaultLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_DEFAULT)
                            SetOffsetDefaultTabstrip(parameterLoanStatus)
                        Else
                            Me.LoanOptionsTabStrip.Items(m_int_const_Default).Enabled = False
                            Me.LoanOptionsTabStrip.Items(m_int_const_Offset).Enabled = False
                        End If
                        Me.LoanOptionsMultiPage.SelectedIndex = Me.LoanOptionsTabStrip.SelectedIndex
                    End If


                Case "SUSPND"
                    Me.LoanOptionsTabStrip.Items(m_int_const_Suspend).Enabled = False
                    Me.LoanOptionsTabStrip.Items(m_int_const_Unsuspend).Enabled = True
                    Me.LabelLoanUnSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_UNSUSPENDED)
                    Me.LoanOptionsTabStrip.Items(m_int_const_Terminate).Enabled = False
                    Me.LoanOptionsTabStrip.Items(m_int_const_PayOff).Enabled = False
                    Me.LoanOptionsTabStrip.Items(m_int_const_Default).Enabled = False
                    Me.LoanOptionsTabStrip.SelectedIndex = 1
                    Me.LoanOptionsMultiPage.SelectedIndex = Me.LoanOptionsTabStrip.SelectedIndex
                    Me.LoanOptionsTabStrip.Items(m_int_const_Offset).Enabled = False
                Case "UNSPND"
                    Me.LoanOptionsTabStrip.Items(m_int_const_Suspend).Enabled = False
                    Me.LoanOptionsTabStrip.Items(m_int_const_Unsuspend).Enabled = False
                    Me.LoanOptionsTabStrip.Items(m_int_const_Terminate).Enabled = True
                    Me.LabelCloseLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_TERMINATED)
                    If ViewState("TabClicked") <> "True" Then
                        Me.LoanOptionsTabStrip.SelectedIndex = 2
                        Me.LoanOptionsMultiPage.SelectedIndex = Me.LoanOptionsTabStrip.SelectedIndex
                        Me.ButtonCloseLoan.Enabled = True
                    Else
                        Me.LoanOptionsTabStrip.SelectedIndex = Me.LoanOptionsTabStrip.SelectedIndex
                        Me.ButtonCloseLoan.Enabled = False
                    End If
                    If Me.IsPayOffValid = True Then
                        Me.LoanOptionsTabStrip.Items(m_int_const_PayOff).Enabled = True
                        Me.LabelPayOffLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_PAYOFF_BE_DONE)
                    ElseIf Me.IsPayOffValid = False Then
                        Me.LoanOptionsTabStrip.Items(m_int_const_PayOff).Enabled = False
                    End If
                    Me.LoanOptionsTabStrip.Items(m_int_const_Default).Enabled = False
                    Me.LoanOptionsTabStrip.Items(m_int_const_Offset).Enabled = False
                    Me.LoanOptionsMultiPage.SelectedIndex = Me.LoanOptionsTabStrip.SelectedIndex
                    Me.LabelDefaultLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_DEFAULT)
                    'START:MMR | 01/08/2019 | YRS-AT-4130 | Disabling Controls after successfully unsuspending the loan
                    Me.ButtonUnSuspendLoan.Enabled = False
                    Me.ButtonShowDetails.Enabled = False
                    Me.TextBoxUnSuspendDate.Enabled = False
                    'END:MMR | 01/08/2019 | YRS-AT-4130 | Disabling Controls after successfully unsuspending the loan
                Case "DEFALT"
                    If Me.IsPayOffValid = True Then
                        Me.LoanOptionsTabStrip.Items(m_int_const_PayOff).Enabled = True
                        Me.LoanOptionsTabStrip.SelectedIndex = 3
                        Me.LoanOptionsMultiPage.SelectedIndex = Me.LoanOptionsTabStrip.SelectedIndex
                        'Me.LabelPayOffLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_PAYOFF_BE_DONE)
                        Me.LoanOptionsTabStrip.Items(m_int_const_Suspend).Enabled = False
                        Me.LoanOptionsTabStrip.Items(m_int_const_Unsuspend).Enabled = False
                        Me.LoanOptionsTabStrip.Items(m_int_const_Terminate).Enabled = False
                        Me.LoanOptionsTabStrip.Items(m_int_const_Default).Enabled = False
                        Me.LoanOptionsTabStrip.Items(m_int_const_Offset).Enabled = False
                    Else
                        Me.LoanOptionsTabStrip.Items(m_int_const_Suspend).Enabled = False
                        Me.LoanOptionsTabStrip.Items(m_int_const_Unsuspend).Enabled = False
                        Me.LoanOptionsTabStrip.Items(m_int_const_Terminate).Enabled = False
                        Me.LoanOptionsTabStrip.Items(m_int_const_PayOff).Enabled = False
                        Me.LoanOptionsTabStrip.Items(m_int_const_Default).Enabled = False
                        SetOffsetDefaultTabstrip(parameterLoanStatus)
                        Me.LoanOptionsMultiPage.SelectedIndex = Me.LoanOptionsTabStrip.SelectedIndex
                    End If
                Case Else
                    Me.LoanOptionsTabStrip.Items(m_int_const_Suspend).Enabled = False
                    Me.ButtonSuspendLoan.Enabled = False
                    Me.LoanOptionsTabStrip.Items(m_int_const_Unsuspend).Enabled = False
                    Me.LoanOptionsTabStrip.Items(m_int_const_Terminate).Enabled = False
                    Me.LoanOptionsTabStrip.Items(m_int_const_PayOff).Enabled = False
                    Me.LoanOptionsTabStrip.Items(m_int_const_Default).Enabled = False
                    Me.LoanOptionsTabStrip.Items(m_int_const_Offset).Enabled = False
            End Select
            If Me.LoanOptionsTabStrip.SelectedIndex = 0 Then
                Me.TextBoxSuspendDate.RequiredDate = True
                Me.TextBoxUnSuspendDate.RequiredDate = False
                'Me.TextBoxDefaultDate.RequiredDate = False
            ElseIf Me.LoanOptionsTabStrip.SelectedIndex = 1 Then
                Me.TextBoxSuspendDate.RequiredDate = False
                Me.TextBoxUnSuspendDate.RequiredDate = True
                'Me.TextBoxDefaultDate.RequiredDate = False
            ElseIf Me.LoanOptionsTabStrip.SelectedIndex = 4 Then
                Me.TextBoxSuspendDate.RequiredDate = False
                Me.TextBoxUnSuspendDate.RequiredDate = False
                'Me.TextBoxDefaultDate.RequiredDate = True
            Else
                Me.TextBoxSuspendDate.RequiredDate = False
                Me.TextBoxUnSuspendDate.RequiredDate = False
                'Me.TextBoxDefaultDate.RequiredDate = False
            End If

            If Me.LoanOptionsTabStrip.SelectedIndex = 2 Then
                If Me.SessionLoanStatus.Trim.ToUpper = "UNSPND" Then
                    Me.LoadLoanCloseDetails(Me.SessionLoanRequestId, "UNSPND")
                Else
                    Me.LoadLoanCloseDetails(Me.SessionLoanRequestId, "PAID")
                End If
            End If
            'If Me.LoanOptionsTabStrip.SelectedIndex = 4 Then
            '    Me.EnableDefaultRadioButtons(Me.SessionLoanRequestId)
            'End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "SetTabStrip", ex)
        End Try
    End Function
    Private Function ClearLabel()
        Try
            LabelLoanSuspensionStatus.Text = " "
            LabelLoanUnSuspensionStatus.Text = " "
            LabelPayOffLoan.Text = " "
            LabelDefaultLoan.Text = " "
            LabelCloseLoan.Text = " "
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "ClearLabel", ex)
        End Try

    End Function
    Private Function ClearSessions()
        Try
            Session("parameterRequestField") = Nothing
            Session("l_string_message") = Nothing
            Session("ds_closeloandetails") = Nothing
            Session("LastName") = Nothing
            Session("FirstName") = Nothing
            Session("MiddleName") = Nothing
            Session("RequestDate") = Nothing
            Session("SSNo") = Nothing
            Session("LastName") = Nothing
            Session("FirstName") = Nothing
            Session("MiddleName") = Nothing
            Session("PersonID") = Nothing
            Session("FundID") = Nothing
            Session("LoanStatus") = Nothing
            Session("LoanRequestId") = Nothing
            Session("RequestDate") = Nothing
            Session("l_bitvalid") = Nothing
            Session("stringMessage") = Nothing
            Session("PayOffAmount") = Nothing
            Session("UnsuspendDate") = Nothing
            'Session("DefaultDate_Automatic") = Nothing
            ViewState("TabClicked") = Nothing
            Offsetevent_Defaultdate = Nothing 'AA:10.26.2015 YRS-AT-2533 added to clear default date
            PreviousLoanStatus = Nothing 'AA:11.09.2015 YRS-AT-2660 added to clear loan status before
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "ClearSessions", ex)
        End Try

    End Function
    Private Function SetDefaultDate(ByVal parameterLoanRequestId As Integer)
        Try
            Dim l_string_defaultdate As String
            Dim l_date_todaydate As Date
            'Dim int_fundresult As Integer'AA:26.04.2016 YRS-AT-2831:Commented as it is checking with the funded date not received date 
            l_string_defaultdate = ""
            'l_string_defaultdate = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetDefaultDate(parameterLoanRequestId, int_fundresult)'AA:26.04.2016 YRS-AT-2831 Commented as it is checking with the funded date not received date 
            l_string_defaultdate = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetDefaultDate(parameterLoanRequestId)


            If l_string_defaultdate <> "" Then
                Me.TextBoxDefaultDate.Text = Convert.ToDateTime(l_string_defaultdate).Date
                'Session("DefaultDate_Automatic") = Me.TextBoxDefaultDate.Text.Trim
                Me.SetDefaultAmount(parameterLoanRequestId, Me.TextBoxDefaultDate.Text.Trim())
            Else
                Me.TextBoxDefaultDate.Text = ""
            End If

            'Added By Ashutosh Patil as on 06-Feb-07
            'YREN- 3034 Point-2
            'If int_fundresult <> 1 Then 'AA:26.04.2016 YRS-AT-2831:Commented as it is checking with the funded date not received date 
            l_date_todaydate = Today.Date()
            'If l_date_todaydate > Convert.ToDateTime(l_string_defaultdate).Date Then
            'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Default date is greater than 90 days past due date.", MessageBoxButtons.OK)
            'Modified By Ashutosh Patil and Shubhratha Tripathi
            'If in case l_string_defaultdate is null or l_string_defaultdate=""
            If l_string_defaultdate <> "" Then
                If l_date_todaydate < Convert.ToDateTime(l_string_defaultdate).Date Then
                    Call MessageDetails(GetMessageByTextMessageNo(MESSAGE_LOAN_CANNOT_BE_DEFAULTED))
                    Exit Function
                End If
                If l_date_todaydate < Convert.ToDateTime(l_string_defaultdate).Date Then
                    Call MessageDetails(GetMessageByTextMessageNo(MESSAGE_LOAN_CANNOT_BE_DEFAULTED))
                    Exit Function
                End If
            ElseIf l_string_defaultdate = "" Then
                Call MessageDetails(GetMessageByTextMessageNo(MESSAGE_LOAN_START_DATE_MISSING))
                Exit Function
            End If
            'Start:AA:26.04.2016 YRS-AT-2831:Commented as it is checking with the funded date not received date so allowing to default the loan
            'Else
            'Call MessageDetails(GetMessageByTextMessageNo(MESSAGE_LOAN_UNFUNDED_PAYMENT_EXISTS_DEFAULTED))
            'Exit Function
            'End If
            'End:AA:26.04.2016 YRS-AT-2831:Commented as it is checking with the funded date not received date so allowing to default the loan



        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "SetDefaultDate", ex)
        End Try
    End Function
    Private Function SetDefaultAmount(ByVal parameterLoanRequestId As Integer, ByVal parameterDefaultDate As String)
        Try
            'Dim l_double_DefaultAmount As Double()
            Dim dsDefaultAmount As DataSet
            Dim drDefaultAmount As DataRow()
            If parameterDefaultDate <> "" Then
                dsDefaultAmount = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetDefaultAmount(parameterLoanRequestId, parameterDefaultDate)
                'Start:AA:02.04.2015 BT:2699:yrs 5.0-2441 - Added new labels to display unpaid pricnipal and cureperiod interest
                If HelperFunctions.isNonEmpty(dsDefaultAmount) Then
                    drDefaultAmount = dsDefaultAmount.Tables(0).Select("chrAcctType = 'TD'")
                    Me.TextboxDefaultAmount.Text = drDefaultAmount(0)("Principal") + drDefaultAmount(0)("Interest")
                    Me.lblUnpaidPrincipal.Text = drDefaultAmount(0)("Principal")
                    Me.lblCRINT.Text = drDefaultAmount(0)("Interest")
                    If dsDefaultAmount.Tables(0).Rows.Count > 1 Then
                        drDefaultAmount = dsDefaultAmount.Tables(0).Select("chrAcctType = 'RT'")
                        Me.TextboxRTOffset.Text = drDefaultAmount(0)("Principal") + drDefaultAmount(0)("Interest")
                        Me.LabelRTOffsetPrincipal.Text = drDefaultAmount(0)("Principal")
                        Me.LabelRTOffsetInterest.Text = drDefaultAmount(0)("Interest")
                    Else
                        trRTOffset.Visible = False
                        trRTOffsetPrincipal.Visible = False
                        trRTOffsetInterest.Visible = False
                        TextboxRTOffset.Text = ""
                    End If
                Else
                    ButtonDefaultLoan.Enabled = False
                End If
                'End:AA:02.04.2015 BT:2699:yrs 5.0-2441 - Added new labels to display unpaid pricnipal and cureperiod interest
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "SetDefaultAmount", ex)
        End Try
    End Function
    Private Function IsUnsuspensionDateValid() As Boolean
        Try
            Dim l_string_message As String = ""
            If CType(Me.TextBoxUnSuspendDate.Text.Trim, System.DateTime) > System.DateTime.Now.Date Then
                If CType(Me.TextBoxUnSuspendDate.Text.Trim, System.DateTime) > System.DateTime.Now.AddMonths(1) Then
                    'message for unsuspend date cannot be gretaer than 1 month ffrom 2days'date stop the process
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_PROCEED_AS_DISCHARGEDATE_ISGREATERTHAN_ONEMONTH)
                End If
                If CType(Me.TextBoxUnSuspendDate.Text.Trim, System.DateTime) <= System.DateTime.Now.AddMonths(1) Then
                    'message for unsuspend date  greater than from 2days'date warning message 
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_DISCHARGE_DATE_AHEAD).Replace("\", "\\").Replace("'", "\'") + "','Loan Unsuspend');", True)
                    ViewState("UnsuspendYes") = "Yes"
                End If
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "IsUnsuspensionDateValid", ex)
        End Try
    End Function
    Private Sub UnsuspendLoan()
        Try
            If Me.TextBoxUnSuspendDate.Text.Trim.Length > 0 Then
                Session("UnsuspendDate") = Me.TextBoxUnSuspendDate.Text.Trim
            Else
                Session("UnsuspendDate") = Nothing
            End If

            If Me.LoadLoanCloseDetails(Me.SessionLoanRequestId, "Automatic") = True Then
                If Me.IsActiveMultipleYMCA(Me.SessionLoanRequestId) = True Then
                    Me.SessionParameterRequestField = "UNSPND"
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_UNSUSPEND) + "','Loan Unsuspend');", True)
                    Exit Sub
                ElseIf Me.IsActiveMultipleYMCA(Me.SessionLoanRequestId) = False Then
                    Me.LabelLoanUnSuspensionStatus.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_CANNOT_UNSUSPENDED_NO_ACTIVE_EMPLOYEMENT_EXISTS)

                    Me.ButtonUnSuspendLoan.Enabled = False
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_UNSUSPENDED_NO_ACTIVE_EMPLOYEMENT_EXISTS)
                    Exit Sub
                End If
            End If


        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "UnsuspendLoan", ex)
        End Try
    End Sub
    'this function creates a new loan request data table
    Private Function MakeRequestDataTable() As DataTable
        Try
            Dim l_dataset_RequestSchema As New DataSet
            Dim l_datatable_CreateNewRequest As DataTable
            Dim l_drow_NewLoanRequest As DataRow
            Dim loanFinaPaymentDate As String 'MMR | 2019.01.23 | YRS-AT-4118 | Declared local variable to store loan final payment date

            l_dataset_RequestSchema = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetRequestSchema()
            l_datatable_CreateNewRequest = l_dataset_RequestSchema.Tables(0).Clone()
            l_drow_NewLoanRequest = l_datatable_CreateNewRequest.NewRow()
            l_drow_NewLoanRequest("guiPersid") = Me.SessionPersonID


            'MR: 2453 - Rework - Start - fetching relevent emp event id based on user selected ymca for re-amortization.
            Dim l_string_EmpEventId As String = ""
            Dim l_ds_LoanDetails As DataSet = Session("ds_closeloandetails")
            'START: MMR | 2019.01.23 | YRS-AT-4118 | Adding Active YMCA Details to existing dataset
            If HelperFunctions.isNonEmpty(Me.ActiveYmcaDetails) Then
                If HelperFunctions.isNonEmpty(Me.ActiveYmcaDetails.Tables("EmployeeActiveYMCA")) Then
                    l_ds_LoanDetails.Tables.Add(Me.ActiveYmcaDetails.Tables("EmployeeActiveYMCA").Copy())
                End If
            End If
            'END: MMR | 2019.01.23 | YRS-AT-4118 | Adding Active YMCA Details to existing dataset
            Dim l_datarows As DataRow()
            If Not l_ds_LoanDetails Is Nothing Then
                If l_ds_LoanDetails.Tables("EmployeeActiveYMCA").Rows.Count > 0 Then
                    l_datarows = l_ds_LoanDetails.Tables("EmployeeActiveYMCA").Select("Convert(YMCAID,'System.String') = '" + Me.DropDownActiveYMCA.SelectedValue + "'")

                    If l_datarows.Length > 0 Then
                        l_string_EmpEventId = l_datarows(0)("EMPEVENTID").ToString()

                    Else
                        l_string_EmpEventId = Me.SessionEmpEventId
                    End If
                End If
                'START: MMR | 2019.01.23 | YRS-AT-4118 | Adding to save loan final payment date
                If HelperFunctions.isNonEmpty(l_ds_LoanDetails.Tables("LoanFinalPaymentDate")) Then
                    loanFinaPaymentDate = Convert.ToString(l_ds_LoanDetails.Tables("LoanFinalPaymentDate").Rows(0).Item("LoanFinalPaymentDate"))
                End If
                'END: MMR | 2019.01.23 | YRS-AT-4118 | Adding to save loan final payment date
            End If
            If l_datarows Is Nothing Then
                l_string_EmpEventId = Me.SessionEmpEventId
            End If

            'l_drow_NewLoanRequest("guiEmpEventId") = Me.SessionEmpEventId
            l_drow_NewLoanRequest("guiEmpEventId") = l_string_EmpEventId
            'MR: 2453 - Rework - End

            'Commented by Ashish 4-Aug-2008 YRS 5.0-489, Start
            'l_drow_NewLoanRequest("guiYmcaid") = Me.SessionYmcaId
            'Commented by Ashish 4-Aug-2008 YRS 5.0-489, End

            'Added by Ashish 4-Aug-2008 YRS 5.0-489, Start
            l_drow_NewLoanRequest("guiYmcaid") = Me.DropDownActiveYMCA.SelectedValue 'GetPersCurrentYmcaID(Me.SessionPersonID) 'Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Commented existing method and assigning YMCA ID on dropdown selection.
            'Added by Ashish 4-Aug-2008 YRS 5.0-489,End
            'toido TD Balance
            If Me.TextBoxRemainingAmount.Text.Trim.Length = 0 Then
                l_drow_NewLoanRequest("mnyTDBalance") = "0.0"
            ElseIf Me.TextBoxRemainingAmount.Text.Trim.Length > 0 Then
                l_drow_NewLoanRequest("mnyTDBalance") = Me.SessionTDBalance
            End If
            If Me.TextBoxRemainingAmount.Text.Trim.Length = 0 Then
                l_drow_NewLoanRequest("mnyAmtRequested") = "0.0"
            ElseIf Me.TextBoxRemainingAmount.Text.Trim.Length > 0 Then
                l_drow_NewLoanRequest("mnyAmtRequested") = Me.TextBoxRemainingAmount.Text.Trim
            End If

            l_drow_NewLoanRequest("chvOrigLoanNumber") = Me.SessionOrigLoanNo
            l_drow_NewLoanRequest("dtmLoanFirstPaymentDate") = DropdownFirstPaymentDate.SelectedValue 'START: PK | 01.15.2019 | YRS-AT-2573 | code added to save selected value from DropdownFirstPaymentDate into database
            If Not String.IsNullOrEmpty(loanFinaPaymentDate) Then
                l_drow_NewLoanRequest("dtmLoanFinalPaymentDate") = Convert.ToDateTime(loanFinaPaymentDate) 'MMR | 2019.01.23 | YRS-AT-4118 | Adding to save loan final payment date
            End If
            l_datatable_CreateNewRequest.Rows.Add(l_drow_NewLoanRequest)

            Return l_datatable_CreateNewRequest
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "MakeRequestDataTable", ex)
        End Try
    End Function
#End Region
    'Private Function GenerateReportsForLoan(ByVal prmLoanStatus As String) As String
    Private Function GenerateReportsForLoan(ByVal prmLoanStatus As String, Optional ByVal prmReasonCode As String = "") As String
        '****************************************************************************************************************
        ' Called From          : LoanOptions.aspx 
        ' Author Name		   : Ashutosh Patil
        ' Employee ID		   : 36307
        ' Extn                 : 8568
        ' Email				   : ashutosh.patil@3i-infotech.com
        ' Creation Date		   : 01/19/2007
        ' Description		   : This function is for calling the Reports related to Payoff,Suspend and Default Loan
        ' Modified By          : 
        ' Modified On          : 
        ' Reason for Change    : 
        '****************************************************************************************************************
        Try
            Dim l_ymca_reportname As String
            Dim l_participant_reportname As String
            Dim l_string_message As String
            Dim dtFileList As New DataTable
            Dim l_StringReportName As String
            Dim l_RefRequestRow As DataRow
            Dim l_DataTable_RefRequest As DataTable
            Dim l_int_count As Integer
            Dim l_participant_DocCode As String ' SC | 2020.04.10 | YRS-AT-4852 | define Doc code for participant
            Dim l_ymca_DocCode As String ' SC | 2020.04.10 | YRS-AT-4852 | define Doc code for ymca
            l_ymca_reportname = ""
            l_participant_reportname = ""
            l_string_message = ""
            Session("PersID") = Me.SessionPersonID
            l_str_LoanStatus = prmLoanStatus
            If IDMAll.DatatableFileList(False) = False Then
                Throw New Exception("Unable to Process Loan Letters, Could not create dependent table")
            End If
            'Modified By Ashutosh Patil as on 18-Apr-2007
            'set properties for Loan Letters 
            Select Case (prmLoanStatus)
                Case "SUSPEND"
                    'Letter to participant
                    ' Start | SC | 2020.04.10 | YRS-AT-4852 | Added changes to report as per COVID Care act
                    'l_participant_reportname = "Military-part.rpt"
                    If prmReasonCode = "LNSPML" Then
                        l_participant_reportname = "Military-part.rpt"
                        l_ymca_reportname = "Military-PA.rpt"
                        l_participant_DocCode = "LNLETTR3"
                        l_ymca_DocCode = "LNLETTR4"
                    ElseIf prmReasonCode = "LNSPPR" Then
                        l_participant_reportname = "Loan_Suspension_Ltr_to_Part-COVID-19.rpt"
                        l_ymca_reportname = "Loan_Suspension_Ltr_to_LPA-COVID-19.rpt"
                        l_participant_DocCode = "LNSUPTCV"
                        l_ymca_DocCode = "LNSULPCV"
                    End If
                    Call PreviewReport(l_participant_reportname, SessionPersonID, 0)
                    ' Call SetProperties(l_participant_reportname, "LNLETTR3")
                    Call SetProperties(l_participant_reportname, l_participant_DocCode)
                    l_string_message = IDMAll.ExportToPDF
                    'Letter to Ymca
                    Session("FormYMCAId") = SessionYmcaId
                    'l_ymca_reportname = "Military-PA.rpt"
                    Call PreviewReport(l_ymca_reportname, SessionPersonID, 1)
                    'Call SetProperties(l_ymca_reportname, "LNLETTR4")
                    Call SetProperties(l_ymca_reportname, l_ymca_DocCode)
                    l_string_message = IDMAll.ExportToPDF
                    ' End | SC | 2020.04.10 | YRS-AT-4852 | Added changes to report as per COVID Care act
                Case "PAYOFF"
                    'Start AA:11.09.2015 YRS-AT-2660:Changed to use new report for default loan closing
                    'Start: Bala: 04.27.2016: YRS-AT-2667: Email sent when closing defaulted and paid loan also.
                    'If PreviousLoanStatus = "DEFALT" Then
                    If PreviousLoanStatus = "DEFALT" Or PreviousLoanStatus = "PAID" Then
                        Session("FormYMCAId") = Nothing 'Bala: 04.29.2016: YRS-AT-2667: Clearing session to make sure the email is not generated to ymca lpa.
                        SendMailToParticipant(SessionPersonID)
                        'Else
                        'End: Bala: 04.27.2016: YRS-AT-2667: Email sent when closing defaulted and paid loan also.
                        'Letter to participant
                        l_participant_reportname = "Loan Satisfied-part.rpt"
                        Call PreviewReport(l_participant_reportname, SessionPersonID, 0)
                        Call SetProperties(l_participant_reportname, "LNLETTR5")
                        l_string_message = IDMAll.ExportToPDF
                        'Letter to Ymca
                        'Shagufta  18 July 2011    For YRS 5.0-1320,BT-829:letter when loan is paid off
                        'Start: Bala: 04.27.2016: YRS-AT-2667: Generating report and email for ymca only if the participant is active in same Y.
                        'If IsActiveYMCAEmployement(Me.SessionPersonID, Me.SessionYmcaId) = True Then ''SP 2014.01.15 - BT-2314\YRS 5.0-2260 - YRS generating blank loan payoff letters (passed Me.SessionYmcaId )
                        If PreviousLoanStatus <> "DEFALT" And IsActiveYMCAEmployement(Me.SessionPersonID, Me.SessionYmcaId) = True Then ''SP 2014.01.15 - BT-2314\YRS 5.0-2260 - YRS generating blank loan payoff letters (passed Me.SessionYmcaId )
                            'End: Bala: 04.27.2016: YRS-AT-2667: Generating report and email for ymca only if the participant is active in same Y.
                            l_ymca_reportname = "Loan Satisfied-PA.rpt"
                            Session("FormYMCAId") = SessionYmcaId
                            Call PreviewReport(l_ymca_reportname, SessionPersonID, 1)
                            Call SetProperties(l_ymca_reportname, "LNLETTR6")
                            l_string_message = IDMAll.ExportToPDF
                            'Else 'AA:11.09.2015 YRS-AT-2660:commented because there is no elsepart

                        End If
                    End If
                    'End AA:11.09.2015 YRS-AT-2660:Changed to use new report for default loan closing
                Case "DEFAULT", "OFFSET" 'AA:26.06.2015 BT:2900 YRS 5.0-2533:Modified letter for default and ofset loans
                    l_participant_reportname = "LoanDefaultLetter.rpt"
                    Call PreviewReport(l_participant_reportname, SessionPersonID, 0)
                    'Call SetProperties(l_participant_reportname, "LNLETTR1")
                    Call SetProperties(l_participant_reportname, "LOANDFTP") ' Added by sanjay
                    l_string_message = IDMAll.ExportToPDF
                    If (l_string_message = "") Then
                        SendMailToParticipant(SessionPersonID)
                    End If
            End Select
            'For YREN - 3023
            'Added By Ashutosh Patil as on 09-Feb-2007
            'This is for sedning emails to the corresponding YMCA Person for Payoff and Suspension letters.
            If l_ymca_reportname <> "" Then
                Call SendEmailToYMCAPerson(Me.SessionYmcaId)
            End If
            Session("FTFileList") = IDMAll.SetdtFileList
            l_int_count = IDMAll.SetdtFileList.Rows.Count
            If l_int_count > 0 Then
                Try
                    'Call the calling of the ASPX to copy the file.
                    Dim popupScript As String = "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                    "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript", popupScript, True)
                Catch ex As Exception
                    HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
                    HelperFunctions.LogException("LoanOptions_" + "GenerateReportsForLoan", ex)
                    Exit Function
                End Try
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "GenerateReportsForLoan", ex)
        End Try
    End Function
    Private Function PreviewReport(ByVal p_string_ReportName As String, ByVal p_parameter_RequestField As String, ByVal int_Level As Integer) As Boolean
        Try
            Select Case int_Level
                Case 0
                    Dim popupScript As String
                    Session("strReportName") = Left(p_string_ReportName, p_string_ReportName.IndexOfAny("."))
                    Session("perseID") = p_parameter_RequestField
                    '  Session("R_ReportToLoad") = True
                    popupScript = "window.open('FT\\ReportViewer.aspx', 'ReportPopUp_1', " & _
                    "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');"
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript1", popupScript, True)
                Case 1
                    Dim popupScript As String
                    Session("strReportName_1") = Left(p_string_ReportName, p_string_ReportName.IndexOfAny("."))
                    Session("perseID") = p_parameter_RequestField
                    popupScript = "window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp_2', " & _
                    "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');"
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript2", popupScript, True)
            End Select
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "PreviewReport", ex)
        End Try
    End Function
    Private Function MessageDetails(ByVal l_string_msg As String)
        Me.LabelDefaultLoan.Text = l_string_msg
        ButtonDefaultLoan.Enabled = False
    End Function
    Private Function GetToEMailAddrs(ByVal paramterYmcaId As String) As String
        '*******************************************************************************************
        'This function will get the email Id of the corresponding YMCA person  
        'For YREN- 3023
        'Added By Ashutosh P Patil as on 09-Feb-2007
        '*******************************************************************************************
        Try
            Dim l_string_EMailAddrs As String = String.Empty
            l_string_EMailAddrs = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetYMCAEmailId(paramterYmcaId)
            '1-TRANSM
            Return l_string_EMailAddrs
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "GetToEMailAddrs", ex)
        End Try
    End Function
    Private Function SendMail(ByVal ToEmailAdd As String) As Boolean
        '**************************************************************************************************
        'This function will send email along with corresponding letters to the corresponding YCMA person
        'For YREN- 3023
        'Added By Ashutosh P Patil as on 09-Feb-2007
        '***************************************************************************************************
        Dim dictParameters As Dictionary(Of String, String)
        Try
            Dim obj As MailUtil
            obj = New MailUtil

            'Start:AA:11.19.2015 YRS-AT-2660 Added to send email when loan is closed after defaulted
            'Start: Bala: 04.27.2016: YRS-AT-2667: Triggering email in when closing the both defaulted and paid loan.
            dictParameters = New Dictionary(Of String, String)()
            If Session("FirstName") IsNot Nothing Then
                dictParameters.Add("FirstName", Session("FirstName"))
            End If
            If Session("LastName") IsNot Nothing Then
                dictParameters.Add("LastName", Session("LastName"))
            End If
            If PreviousLoanStatus = "DEFALT" And SessionLoanStatus.Trim.ToUpper = "CLOSED" Then
                'dictParameters = New Dictionary(Of String, String)()
                'If Session("FirstName") IsNot Nothing Then
                '    dictParameters.Add("FirstName", Session("FirstName"))
                'End If
                'If Session("LastName") IsNot Nothing Then
                '    dictParameters.Add("LastName", Session("LastName"))
                'End If
                MailUtil.SendMail(EnumEmailTemplateTypes.LOAN_DEFAULT_CLOSED_PERSONS, "", ToEmailAdd, "", "", dictParameters, "", Nothing, Mail.MailFormat.Html)
                Return True
            ElseIf PreviousLoanStatus = "PAID" And SessionLoanStatus.Trim.ToUpper = "CLOSED" And Session("FormYMCAId") = Nothing Then
                MailUtil.SendMail(EnumEmailTemplateTypes.LOAN_PAID_CLOSED_PERSONS, "", ToEmailAdd, "", "", dictParameters, "", Nothing, Mail.MailFormat.Html)
                Return True
            End If
            'End: Bala: 04.27.2016: YRS-AT-2667: Triggering email in when closing the both defaulted and paid loan.
            'Start:AA:11.19.2015 YRS-AT-2660 Added to send email when loan is closed after defaulted
            obj.MailCategory = "TDLoan"
            If obj.MailService = False Then
                Return False
            End If
            If ToEmailAdd.ToString().Trim() = String.Empty Then
                obj.ToMail = obj.FromMail
            Else
                obj.ToMail = ToEmailAdd.ToString().Trim()
            End If
            'Priya 16-March-2009 Change Time of Customer Service Department 8:45 AM to 6:00 previously it was 8:45 AM to 8:00 as per Hafiz mail on March 13, 2009 
            'obj.MailMessage = "Please open the attached document for important participant loan information. If you have any questions or need additional information, please feel free to contact our Customer Service Department at 800-RET-YMCA, Monday through Friday, 8:45 AM - 6:00 PM Eastern Time."
            'Commented by Priya 17-March-2009 
            'Added by Priya 17-March-2009 ,Start 
            Dim strMailLastParagraphMsg As String = String.Empty
            strMailLastParagraphMsg = YMCARET.YmcaBusinessObject.MailBOClass.GetMailLastParagraph().Trim()
            'Added by Priya 17-March-2009 ,End
            If l_str_LoanStatus = "DEFAULT" OrElse l_str_LoanStatus = "OFFSET" Then
                obj.MailMessage = "Please read the attached letter for important information regarding your Tax-Deferred Savings Plan loan."
            Else
                obj.MailMessage = "Please open the attached document for important participant loan information." & strMailLastParagraphMsg
            End If

            If l_str_LoanStatus = "DEFAULT" Then
                obj.Subject = "Important Information Regarding Your Loan"
            Else
                obj.Subject = "Important Participant Loan Information"
            End If

            If Not Session("StringDestFilePath") Is Nothing Then
                obj.MailAttachments.Add(Session("StringDestFilePath"))
            End If
            obj.Send()
            Return True
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "SendMail", ex)
        End Try
    End Function
    Private Sub SendEmailToYMCAPerson(ByVal YmcaId As String)
        '*****************************************************************************************************************
        'This function will get the email details and will send it to the corresponding YMCA person  
        'For YREN- 3023
        'Added By Ashutosh Patil as on 09-Feb-2007
        '*****************************************************************************************************************
        'Added by Ashish 20-Mar-2009
        Dim l_strYmcaEmailStatusMesssage As String = String.Empty
        Try
            'Dim l_StringMsgEmail As String = ""
            Dim l_string_ToEmailAddrs As String = ""
            l_string_ToEmailAddrs = Me.GetToEMailAddrs(YmcaId)
            'l_string_ToEmailAddrs = Me.GetToEMailAddrs(SessionYmcaNo)
            If l_string_ToEmailAddrs = "" Then
                'l_string_ToEmailAddrs = ConfigurationSettings.AppSettings("FromEMailIdForTDLoans")
                l_string_ToEmailAddrs = String.Empty
                'l_StringMsgEmail = " Email Id Missing for YMCA :"
                'Commented by Ashish 20-Mar-2009 
                'l_ltr_msg = " Email Id is Missing."
                'Added by Ashish  20-Mar-2009 ,Start
                l_strYmcaEmailStatusMesssage = GetMessageByTextMessageNo(MESSAGE_LOAN_YMCA_EMAIL_MISSING)
            Else
                'l_StringMsgEmail = " Email has been sent to YMCA :"
                l_strYmcaEmailStatusMesssage = GetMessageByTextMessageNo(MESSAGE_LOAN_EMAIL_SENT_TO_YMCA)
            End If
            If Me.SendMail(l_string_ToEmailAddrs) = False Then
                'l_ltr_msg = "Loan " & l_letter_type & " successfully and " & l_StringMsgEmail & Session("FormYmcaNo")
                'l_ltr_msg = l_StringMsgEmail & SessionYmcaId
                'l_ltr_msg = l_StringMsgEmail
                'Else
                l_strYmcaEmailStatusMesssage = GetMessageByTextMessageNo(MESSAGE_LOAN_EMAIL_TO_YMCA_ERROR)
            End If
            If l_ltr_msg.ToString <> String.Empty Then
                l_ltr_msg &= vbCrLf & l_strYmcaEmailStatusMesssage
            Else
                l_ltr_msg = l_strYmcaEmailStatusMesssage
            End If
        Catch
            l_ltr_msg = GetMessageByTextMessageNo(MESSAGE_LOAN_EMAIL_TO_YMCA_ERROR)
        End Try
    End Sub
    'Shubhrata YREN-3034
    'Private Sub ButtonShowDefaultAmount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonShowDefaultAmount.Click
    '    Try
    '        If Me.TextBoxDefaultDate.Text.Trim().length > 0 Then
    '            Me.SetDefaultAmount(Me.SessionLoanRequestId, Me.TextBoxDefaultDate.Text.Trim())
    '        End If

    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message)
    '    End Try
    'End Sub
    Private Function SetProperties(ByVal paramstrReportname As String, ByVal paramstrlnletter As String) As String
        'Mofifed By Ashutosh Patil as on 18-Apr-2007
        'For Loan Report
        Dim par_Arrylist As New ArrayList
        Try
            IDMAll.PreviewReport = True
            IDMAll.LogonToDb = True
            IDMAll.CreatePDF = True
            IDMAll.CreateIDX = True
            IDMAll.CopyFilesToIDM = True
            IDMAll.PersId = Me.SessionPersonID
            IDMAll.ReportName = paramstrReportname.ToString
            IDMAll.DocTypeCode = paramstrlnletter.ToString
            'IDMAll.OutputFileType = "LOAN"
            IDMAll.OutputFileType = paramstrlnletter.ToString
            ' Start | SC | 2020.04.10 | YRS-AT-4852 | Added new doccode for COVID letter
            'If paramstrlnletter.ToString = "LOANDFTP" Or paramstrlnletter.ToString = "LNLETTR1" Or paramstrlnletter.ToString = "LNLETTR3" Or paramstrlnletter.ToString = "LNLETTR5" Then
            If paramstrlnletter.ToString = "LOANDFTP" Or paramstrlnletter.ToString = "LNLETTR1" Or paramstrlnletter.ToString = "LNLETTR3" Or paramstrlnletter.ToString = "LNLETTR5" Or paramstrlnletter.ToString = "LNSUPTCV" Then
                ' End | SC | 2020.04.10 | YRS-AT-4852 | Added new doccode for COVID letter
                IDMAll.AppType = "P"
            Else
                IDMAll.AppType = "A"
                IDMAll.YMCAID = SessionYmcaId
            End If
            'AA:26.06.2015 BT:2900 YRS 5.0-2533:Modified letter for default and ofset loans
            If paramstrReportname = "LoanDefaultLetter.rpt" Then
                par_Arrylist.Add(CType(Session("FundNo"), String).ToString.Trim)
                par_Arrylist.Add(Session("DefaultDate"))
                'Start: Bala: 04.27.2016: YRS-AT-2667: Passing loan no in addition based on the changes in Loan Satisfied-Part.rpt report.
            ElseIf paramstrReportname = "Loan Satisfied-part.rpt" Then
                par_Arrylist.Add(CType(SessionPersonID, String).ToString.Trim)
                par_Arrylist.Add(CType(Session("OrigLoanNo"), String).ToString.Trim)
                'End: Bala: 04.27.2016: YRS-AT-2667: Passing loan no in addition based on the changes in Loan Satisfied-Part.rpt report.
            Else
                par_Arrylist.Add(CType(SessionPersonID, String).ToString.Trim)
            End If
            IDMAll.ReportParameters = par_Arrylist
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "SetProperties", ex)
        End Try
    End Function
    'Added by Ashish 4-Aug-2008 New Function for YRS 5.0-489, to get person current YMCA ID  
    'Start -- Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Commented existing method as not required
    'Private Function GetPersCurrentYmcaID(ByVal parameterPersonId As String) As String
    '    Dim l_string_YmcaId As String
    '    Try
    '        l_string_YmcaId = String.Empty
    '        Dim l_dataset_Ymca As DataSet
    '        l_dataset_Ymca = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetYMCAId(parameterPersonId)
    '        If Not l_dataset_Ymca Is Nothing Then
    '            If l_dataset_Ymca.Tables(0).Rows.Count > 0 Then
    '                If Not l_dataset_Ymca.Tables(0).Rows(0).Item("YmcaId").GetType.ToString = "System.DBNull" Then
    '                    l_string_YmcaId = CType(l_dataset_Ymca.Tables(0).Rows(0).Item("YmcaId"), String)
    '                End If
    '            End If
    '        End If
    '        If l_string_YmcaId.Equals(String.Empty) Then
    '            l_string_YmcaId = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetPersonLatestYmcaId(parameterPersonId)
    '        End If
    '        Return l_string_YmcaId
    '    Catch ex As Exception
    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
    '        HelperFunctions.LogException("LoanOptions_" + "GetPersCurrentYmcaID", ex)
    '    End Try
    'End Function
    'End -- Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Commented existing method as not required
    'Added by Ashish 17-Mar-2009 New Function for YRS 5.0-679, Get Re-Amortized new Loan Number
    Private Function GetNewReamortizedLoanNumber(ByVal paraOrigLoanNumber As String) As Integer
        Try
            Dim l_intNewLoanNumber As Integer
            l_intNewLoanNumber = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetReAmortizeLoanNumber(paraOrigLoanNumber)
            Return l_intNewLoanNumber
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "GetNewReamortizedLoanNumber", ex)
        End Try
    End Function
    Private Function GenerateReamortizeReport(ByVal paraOrigLoanNumber As Integer, ByVal paraNewLoanNumber As Integer, ByVal paraOldYmcaId As String, ByVal paraNewYmcaID As String, ByVal paraPersonID As String) As String
        Dim l_intReason As Int16 = 0
        Dim l_strReportName As String
        Dim l_intParticipantLetterType As Int16 = 0
        Dim l_intYmcaLetterType As Int16 = 0
        Dim l_strParticipantIDMMesage As String = String.Empty
        Dim l_strYmcaIDMMessage As String = String.Empty
        Dim l_strIDMMessageForYmcaAndParticipant As String = String.Empty
        Try
            If paraOrigLoanNumber <> 0 AndAlso paraNewLoanNumber <> 0 AndAlso paraOldYmcaId.ToString() <> String.Empty AndAlso paraNewYmcaID.ToString() <> String.Empty AndAlso paraPersonID.ToString() <> String.Empty Then
                l_strReportName = "Loan Reamortization Letters.rpt"
                'check for senario 1, Employment changed
                If paraOldYmcaId <> paraNewYmcaID Then
                    l_intReason = 1
                Else
                    'check for reason, Payroll changed  or data correction
                    l_intReason = GetReamortizedReason(paraNewYmcaID)
                End If
                If l_intReason <> 0 Then
                    '************************************************************************************************
                    'original Letter type in Loan Reamortization Letters Report
                    '1->Participant loan-new Y with existing loan
                    '2->Particiapnt loan-reamortized under original term
                    '3->Participant loan-payroll schedule change
                    '4->Ymca loan-new participant with existing loan
                    '5->Ymca loan-reamortized under original term
                    '6->Ymca loan-payroll schedule changed
                    '************************************************************************************************
                    'mapped reason with report letter type
                    Select Case l_intReason
                        Case 1
                            l_intParticipantLetterType = 1
                            l_intYmcaLetterType = 4
                        Case 2
                            l_intParticipantLetterType = 3
                            l_intYmcaLetterType = 6
                        Case 3
                            l_intParticipantLetterType = 2
                            l_intYmcaLetterType = 5
                        Case Else
                            l_intParticipantLetterType = 2
                            l_intYmcaLetterType = 5
                    End Select
                    If IDMAll.DatatableFileList(False) = False Then
                        Throw New Exception("Unable to Process Loan Letters, Could not create dependent table")
                    End If
                    'generate report for YMCA
                    l_strYmcaIDMMessage = GenerateReamortizedYmcaReport(paraNewLoanNumber, l_intYmcaLetterType, paraNewYmcaID, paraPersonID, l_strReportName)
                    'generate report for participant
                    l_strParticipantIDMMesage = GenerateReamortizedParticipantReport(paraNewLoanNumber, l_intParticipantLetterType, paraNewYmcaID, paraPersonID, l_strReportName)
                    If Not l_strYmcaIDMMessage Is Nothing Then
                        If l_strYmcaIDMMessage.ToString() <> String.Empty Then
                            l_strIDMMessageForYmcaAndParticipant = l_strYmcaIDMMessage & vbCrLf
                        End If
                    End If
                    'set message
                    If Not l_strParticipantIDMMesage Is Nothing Then
                        If l_strParticipantIDMMesage.ToString() <> String.Empty Then
                            l_strIDMMessageForYmcaAndParticipant &= l_strParticipantIDMMesage
                        End If
                    End If
                    'Copy files to server
                    Dim l_int_count As Int32 = 0
                    Session("FTFileList") = IDMAll.SetdtFileList
                    l_int_count = IDMAll.SetdtFileList.Rows.Count
                    If l_int_count > 0 Then
                        'Call the calling of the ASPX to copy the file.
                        Dim popupScript As String = "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp_5', " & _
                        "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"

                        ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript5", popupScript, True)
                    End If
                End If
            Else
                l_strIDMMessageForYmcaAndParticipant = "Unable to generate loan re-amortized letters"
            End If
            Return l_strIDMMessageForYmcaAndParticipant
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "GenerateReamortizeReport", ex)
        End Try
    End Function
    Private Function GetReamortizedReason(ByVal paraYmcaID As String) As Integer
        Try
            Dim l_intReamortizeReason As Int16
            l_intReamortizeReason = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetReAmortizeReason(paraYmcaID)
            Return l_intReamortizeReason
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "GetReamortizedReason", ex)
        End Try
    End Function
    Private Function GenerateReamortizedParticipantReport(ByVal paraNewLoanNumber As Integer, ByVal paraParticipantLetterType As Int16, ByVal paraYmcaID As String, ByVal paraPersonID As String, ByVal paraReportName As String) As String
        Dim l_strParticipantIDM_Message As String = String.Empty
        Try
            'Preview Report
            PreviewReamortizedReport(paraReportName, paraNewLoanNumber, paraParticipantLetterType, 0)
            'Set IDM Properties
            SetIDMPropertyForReamortize(paraNewLoanNumber, paraParticipantLetterType, paraYmcaID, paraPersonID, "LNRAMRTP", paraReportName)
            'call export to PDF
            l_strParticipantIDM_Message = IDMAll.ExportToPDF()
            'sent mail to participant
            If (l_strParticipantIDM_Message = "") Then
                SendMailToParticipant(paraPersonID)
            End If
            Return l_strParticipantIDM_Message
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "GenerateReamortizedParticipantReport", ex)
        End Try
    End Function
    Private Function GenerateReamortizedYmcaReport(ByVal paraNewLoanNumber As Integer, ByVal paraYmcaLetterType As Int16, ByVal paraYmcaID As String, ByVal paraPersonID As String, ByVal paraReportName As String) As String
        Dim l_strYmcaIDM_Message As String = String.Empty
        Try
            'Preview Report
            PreviewReamortizedReport(paraReportName, paraNewLoanNumber, paraYmcaLetterType, 1)
            'Set IDM Properties
            SetIDMPropertyForReamortize(paraNewLoanNumber, paraYmcaLetterType, paraYmcaID, paraPersonID, "LNRAMRTA", paraReportName)
            'call export to PDF
            l_strYmcaIDM_Message = IDMAll.ExportToPDF()
            'sent mail to Ymca
            If (l_strYmcaIDM_Message = "") Then
                SendEmailToYMCAPerson(paraYmcaID)
            End If
            Return l_strYmcaIDM_Message
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "GenerateReamortizedYmcaReport", ex)
        End Try
    End Function
    Private Function SetIDMPropertyForReamortize(ByVal paraNewLoanNumber As Integer, ByVal paraReamortizedLetterType As Int16, ByVal paraYmcaID As String, ByVal paraPersonID As String, ByVal paraDocTypeCode As String, ByVal paraReportName As String) As Boolean
        Dim par_Arrylist As ArrayList
        Try
            IDMAll.PreviewReport = True
            IDMAll.LogonToDb = True
            IDMAll.CreatePDF = True
            IDMAll.CreateIDX = True
            IDMAll.CopyFilesToIDM = True
            IDMAll.PersId = paraPersonID
            IDMAll.ReportName = paraReportName
            IDMAll.DocTypeCode = paraDocTypeCode
            IDMAll.OutputFileType = paraDocTypeCode
            If paraDocTypeCode.ToString.ToUpper = "LNRAMRTP" Then
                IDMAll.AppType = "P"
            Else
                IDMAll.AppType = "A"
                IDMAll.YMCAID = paraYmcaID
            End If
            par_Arrylist = New ArrayList
            par_Arrylist.Add(paraNewLoanNumber)
            par_Arrylist.Add(paraReamortizedLetterType)
            IDMAll.ReportParameters = par_Arrylist
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "SetIDMPropertyForReamortize", ex)
        End Try
    End Function
    Private Function PreviewReamortizedReport(ByVal p_string_ReportName As String, ByVal paraNewLoanNumber As Integer, ByVal paraReamortizedLetterType As Int16, ByVal int_Level As Integer) As Boolean
        Dim l_ArrayReamortizeRptPara As ArrayList
        Try
            l_ArrayReamortizeRptPara = New ArrayList
            Select Case int_Level
                Case 0
                    Dim popupScript As String
                    Session("strReportName") = Left(p_string_ReportName, p_string_ReportName.IndexOfAny("."))
                    l_ArrayReamortizeRptPara.Add(paraNewLoanNumber)
                    l_ArrayReamortizeRptPara.Add(paraReamortizedLetterType)
                    Session("ReAmortizeRptParameterForParticipant") = l_ArrayReamortizeRptPara
                    popupScript = "window.open('FT\\ReportViewer.aspx', 'ReportPopUp_3', " & _
                    "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');"

                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript3", popupScript, True)
                Case 1
                    Dim popupScript As String
                    Session("strReportName_1") = Left(p_string_ReportName, p_string_ReportName.IndexOfAny("."))
                    l_ArrayReamortizeRptPara.Add(paraNewLoanNumber)
                    l_ArrayReamortizeRptPara.Add(paraReamortizedLetterType)
                    Session("ReAmortizeRptParameterForYmca") = l_ArrayReamortizeRptPara
                    popupScript = "window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp_4', " & _
                    "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');"
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript4", popupScript, True)
            End Select
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "PreviewReamortizedReport", ex)
        End Try
    End Function
    Private Function SendMailToParticipant(ByVal paraPersID As String)
        '*****************************************************************************************************************
        'This function will get the email details and will send it to the Participant  
        'For YRS 5.0 679
        'Added By Ashish Srivastava as on 20-Mar-2009
        '*****************************************************************************************************************
        Dim l_strParticipantToEmailAddrs As String = String.Empty
        Dim l_strParticipantMailStatusMessage As String = String.Empty
        Try
            If paraPersID.ToString() <> String.Empty Then
                l_strParticipantToEmailAddrs = Me.GetParticipantToEmailAddress(paraPersID)
                If Not l_strParticipantToEmailAddrs Is Nothing Then
                    If l_strParticipantToEmailAddrs.ToString() = String.Empty Then
                        l_strParticipantMailStatusMessage = " " + GetMessageByTextMessageNo(MESSAGE_LOAN_PARTICIPANT_EMAIL_MISSING)
                    Else
                        l_strParticipantMailStatusMessage = " " + GetMessageByTextMessageNo(MESSAGE_LOAN_EMAIL_SENT_TO_PARTICIAPANT)
                    End If


                    If Me.SendMail(l_strParticipantToEmailAddrs) = False Then
                        l_strParticipantMailStatusMessage = " " + GetMessageByTextMessageNo(MESSAGE_LOAN_EMAIL_TO_PARTICIPANT_ERROR)
                    End If
                End If
                If l_ltr_msg.ToString() <> String.Empty Then
                    l_ltr_msg &= vbCrLf & l_strParticipantMailStatusMessage
                Else
                    l_ltr_msg = l_strParticipantMailStatusMessage
                End If
            End If
        Catch
            If l_ltr_msg.ToString() <> String.Empty Then
                l_ltr_msg &= vbCrLf & GetMessageByTextMessageNo(MESSAGE_LOAN_EMAIL_TO_PARTICIPANT_ERROR)
            Else
                l_ltr_msg = GetMessageByTextMessageNo(MESSAGE_LOAN_EMAIL_TO_PARTICIPANT_ERROR)
            End If
        End Try
    End Function

    Private Function GetParticipantToEmailAddress(ByVal paraPersID As String) As String
        Dim l_strParticipantEmailAddrs As String = String.Empty
        Try
            l_strParticipantEmailAddrs = YMCARET.YmcaBusinessObject.YMCACommonBOClass.GetParticipantValidEmailAddress(paraPersID)
            Return l_strParticipantEmailAddrs
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "GetParticipantToEmailAddress", ex)
        End Try
    End Function
    ''Shagufta-15 July 2011    For YRS 5.0-1320,BT-829:letter when loan is paid off
    'SP 2014.01.15 BT:2314\YRS 5.0-2260 - YRS generating blank loan payoff letters 
    ''Added new parameter parameterGuiYMCAId
    Private Function IsActiveYMCAEmployement(ByVal parameterguiPersId As String, ByVal parameterGuiYMCAId As String)
        Try
            Dim l_int_count As Integer
            l_int_count = 0
            'Code to check whether a person is having active employment or not
            l_int_count = YMCARET.YmcaBusinessObject.LoanInformationBOClass.IsActiveYMCAEmployement(parameterguiPersId, parameterGuiYMCAId)
            If l_int_count = 0 Then
                Return False
            ElseIf l_int_count > 0 Then
                Return True
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "IsActiveYMCAEmployement", ex)
        End Try

    End Function
    'Start:AA:26.08.2013 BT:2628 YRS 5.0-2405 -  Added for loan maintenance actions
    Private Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        Try
            If ViewState("UnsuspendYes") = Nothing Then
                Dim l_string_message As String = ""
                l_string_message = Me.CancelLoan(Me.SessionParameterRequestField)
                If l_string_message <> "" Then
                    l_string_message = l_string_message & l_ltr_msg
                    If Me.SessionParameterRequestField = "CLOSED" Or Me.SessionParameterRequestField = "DEFALT" Or Me.SessionParameterRequestField = "PAYOFF" Or Me.SessionParameterRequestField = "OFFSET" Then
                        ViewState("TabClicked") = Nothing
                        Response.Redirect("MainWebForm.aspx", False)
                        Session("blnSuccessful") = True
                        Session("LoanMaintenanceAction") = l_string_message + " Fund Id: " + Session("FundNo").ToString()
                    Else
                        'START: MMR | 2018.12.26 | YRS-AT-4130 | Commented as message does not remain static on screen and storing message in viewstate object to access it on page load event for displaying on UI
                        'HelperFunctions.ShowMessageToUser(l_string_message, EnumMessageTypes.Success)
                        Me.SuccessMessage = l_string_message
                        'END: MMR | 2018.12.26 | YRS-AT-4130 | Commented as message does not remain static on screen and storing message in viewstate object to access it on page load event for displaying on UI
                        Me.SetTabStrip(Me.SessionLoanStatus.Trim.ToUpper)
                        Session("parameterRequestField") = Nothing
                        ViewState("Cancel") = False
                        'AA:2015.08.05 YRS 5.0-2441:: Added below line to enable the tabs.
                        ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "document.forms(0).submit();", True)
                    End If
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "closeDialog();", True)
                End If
            End If
            If ViewState("UnsuspendYes") = "Yes" Then
                Me.UnsuspendLoan()
                Me.SetTabStrip(Me.SessionLoanStatus.Trim.ToUpper)
                ViewState("UnsuspendYes") = Nothing
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "btnYes_Click", ex)
        End Try
    End Sub
    'End:AA:26.08.2013 BT:2628 YRS 5.0-2405 -  Added for loan maintenance actions

    'Start:AA:02.04.2015 BT:2699:yrs 5.0-2441 - Added new labels to display unpaid pricnipal and cureperiod interest
    Private Sub SetOffsetAmount(ByVal parameterLoanRequestId As Integer, ByVal parameterOffsetDate As String, ByVal parameterLoanStatus As String)
        Try
            Dim dblOffsetAmount As Decimal()
            Dim strEmpStatus, strAge As String
            If parameterOffsetDate <> "" Then
                dblOffsetAmount = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetOffsetAmount(parameterLoanRequestId, parameterOffsetDate, parameterLoanStatus, strEmpStatus, strAge)
                lblEmpStatus.Text = strEmpStatus
                lblPersonAge.Text = strAge
                Me.TextboxOffsetAmount.Text = dblOffsetAmount(0) + dblOffsetAmount(1)
                Me.lblOffsetUnpaidPrincipal.Text = dblOffsetAmount(0)
                Me.lblOffsetInt.Text = dblOffsetAmount(1)
                If parameterLoanStatus = "DEFALT" Then
                    Me.lblPrincipal.Text = "Default Amount"
                    Me.lblOffsetIntdetail.Text = "Phantom Interest"
                    lblOffsetAmountdetail.Text = "(Default Amount + Phantom Interest)"
                ElseIf parameterLoanStatus = "PAID" Then
                    Me.lblPrincipal.Text = "Unpaid Principal"
                    Me.lblOffsetIntdetail.Text = "Cure Period Interest"
                    lblOffsetAmountdetail.Text = "(Unpaid Principal + Cure Period Interest)"
                End If

            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "SetOffsetAmount", ex)
        End Try
    End Sub
    Private Function IsOffset() As Boolean
        Try
            Dim blnResult As Boolean = False
            Dim intOffseteventReason As Integer
            blnResult = YMCARET.YmcaBusinessObject.LoanInformationBOClass.IsLoanOffsetValid(Me.SessionLoanRequestId, intOffseteventReason)
            If blnResult Then
                ViewState("OffseteventReason") = intOffseteventReason
            End If
            Return blnResult
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "IsPayOffValid", ex)
        End Try
    End Function

    Private Sub SetOffsetDefaultTabstrip(ByVal parameterLoanStatus As String)
        Try
            If IsOffset() Then
                Me.LoanOptionsTabStrip.Items(m_int_const_Offset).Enabled = True
                Me.LoanOptionsTabStrip.SelectedIndex = m_int_const_Offset
                Me.LoanOptionsTabStrip.Items(m_int_const_Default).Enabled = False
                Me.SetOffsetDate(Me.SessionLoanRequestId, parameterLoanStatus)
            ElseIf parameterLoanStatus = "PAID" Then
                Me.LoanOptionsTabStrip.Items(m_int_const_Default).Enabled = True
                Me.LoanOptionsTabStrip.SelectedIndex = m_int_const_Default
                Me.LoanOptionsTabStrip.Items(m_int_const_Offset).Enabled = False
                Me.SetDefaultDate(Me.SessionLoanRequestId)
            End If

            Me.LoanOptionsMultiPage.SelectedIndex = Me.LoanOptionsTabStrip.SelectedIndex
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "SetOffsetTabstrip", ex)
        End Try
    End Sub

    Private Sub ButtonOffsetLoan_Click(sender As Object, e As EventArgs) Handles ButtonOffsetLoan.Click
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            Me.SessionParameterRequestField = "OFFSET"
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_OFFSET) + "','Loan Offset');", True)
            Session("OffsetLoan") = "OffsetLoan"
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "ButtonOffsetLoan_Click", ex)
        End Try
    End Sub

    Private Sub SetOffsetDate(ByVal parameterLoanRequestId As Integer, ByVal parameterLoanStatus As String)
        Dim strOffsetdate As String
        Dim intOffsetMessageNo As Integer
        Dim strDefaultdate As String
        Dim dictDates As Dictionary(Of String, String)
        Try
            strOffsetdate = String.Empty
            'Start:AA:10.26.2015  YRS-AT-2533 : changed to get the default and offset dates
            strDefaultdate = String.Empty
            dictDates = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetOffsetAndDefaultDates(parameterLoanRequestId, parameterLoanStatus)
            If dictDates IsNot Nothing AndAlso dictDates.Count > 0 Then
                strOffsetdate = IIf(dictDates.ContainsKey("OffsetDate"), dictDates.Item("OffsetDate").Trim, String.Empty)
                strDefaultdate = IIf(dictDates.ContainsKey("DefaultDate"), dictDates.Item("DefaultDate").Trim, String.Empty)
                If Not String.IsNullOrEmpty(strOffsetdate) Then 'AA:10.26.2015  YRS-AT-2533 : Handled to assign offset date to textbox
                    Me.TextBoxOffsetDate.Text = Convert.ToDateTime(strOffsetdate).Date
                    Me.SetOffsetAmount(parameterLoanRequestId, Me.TextBoxOffsetDate.Text.Trim(), parameterLoanStatus)
                Else
                    Me.TextBoxOffsetDate.Text = ""
                End If
                If Not String.IsNullOrEmpty(strDefaultdate) Then 'AA:10.26.2015  YRS-AT-2533 : Handled to assign default date to property
                    Offsetevent_Defaultdate = strDefaultdate
                End If
                'End:AA:10.26.2015  YRS-AT-2533 : changed to get the default and offset dates
                If ViewState("OffseteventReason") IsNot Nothing Then
                    intOffsetMessageNo = ViewState("OffseteventReason")
                    LabelOffsetLoan.Text = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(intOffsetMessageNo)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function IsLoanUnpaidFromCureperiod(ByVal parameterLoanRequestId As Integer)
        Dim bitvalid As Boolean
        Dim strMessage As String
        Try
            bitvalid = YMCARET.YmcaBusinessObject.LoanInformationBOClass.IsLoanUnpaidFromCureperiod(parameterLoanRequestId, strMessage)
            If Not String.IsNullOrEmpty(strMessage) Then
                HelperFunctions.ShowMessageToUser(strMessage, EnumMessageTypes.Error)
            End If
            Return bitvalid
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'End:AA:02.04.2015 BT:2699:yrs 5.0-2441 - Added new labels to display unpaid pricnipal and cureperiod interest

    'START: VC | 2018.06.21 | YRS-AT-3190 | Verifying whether loan is remortized within threshold period or after it.
    Private Function IsLoanReamortizedEarlier(ByVal loanRequestId As Integer) As Boolean
        Return YMCARET.YmcaBusinessObject.LoanInformationBOClass.IsLoanReamortizedEarlier(loanRequestId)
    End Function
    'END: VC | 2018.06.21 | YRS-AT-3190 | Verifying whether loan is remortized within threshold period or after it.

    'START: PK | 01.15.2019 | YRS-AT-2573 | code added to populate dropdown and to handle values selected in dropdownActiveYmca and added label to show the date selected from dropdown list
    Private Function PopulateFirstPaymentDateDropDown(ByVal loanRequestId As Integer, ByVal ymcaId As String)
        Dim listForFirstPaymentDate As DataTable = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetListForFirstPaymentDate(loanRequestId, ymcaId)
        If Not listForFirstPaymentDate Is Nothing Then
            If listForFirstPaymentDate.Rows.Count > 0 Then
                PopulateFirstPaymentDateDropDown(listForFirstPaymentDate)
            End If
        End If
    End Function

    Private Function PopulateFirstPaymentDateDropDown(ByVal listForFirstPaymentDate As DataTable)
        Dim parameters As Dictionary(Of String, String)

        If Not listForFirstPaymentDate Is Nothing Then
            If listForFirstPaymentDate.Rows.Count > 0 Then
                Me.DropdownFirstPaymentDate.DataSource = listForFirstPaymentDate
                Me.DropdownFirstPaymentDate.DataValueField = ("PayDatesForDisplay").ToString.Trim
                Me.DropdownFirstPaymentDate.DataBind()

                Me.DropdownFirstPaymentDate.SelectedIndex = 0
                Me.DropdownFirstPaymentDate.Enabled = True

                parameters = New Dictionary(Of String, String)
                parameters.Add("DefaultFirstPaymentDate", Me.DropdownFirstPaymentDate.SelectedValue)
                lblPayDates.Text = GetMessageByTextMessageNo(YMCAObjects.MetaMessageList.MESSAGE_LOAN_DISPLAY_FIRST_PAYMENT_DATE, parameters)
            End If
        End If
    End Function

    Protected Sub DropDownActiveYMCA_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownActiveYMCA.SelectedIndexChanged
        Try
            Me.PopulateFirstPaymentDateDropDown(Me.SessionLoanRequestId, DropDownActiveYMCA.SelectedValue)
            'START: MMR | 2019.01.23 | YRS-AT-4118 | Fetching re-amoratization amount/months based on selected YMCA
            Me.LoadLoanCloseDetails(Me.SessionLoanRequestId, DropDownActiveYMCA.SelectedValue, DropdownFirstPaymentDate.SelectedValue)
            'END: MMR | 2019.01.23 | YRS-AT-4118 | Fetching re-amoratization amount/months based on selected YMCA
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_DropDownActiveYMCA_SelectedIndexChanged", ex)
        End Try
    End Sub
    'END: PK | 01.15.2019 | YRS-AT-2573 | code added to populate dropdown and to handle values selected in dropdownActiveYmca and added label to show the date selected from dropdown list
    ' Start | SC | 2020.04.10 | YRS-AT-4852 | Added method to populate Loan suspend reasons into Dropdown List
    Private Sub PopulateReasonForLoanSuspension()
        Dim dsReasons As DataSet
        Dim reasonDescription As String
        Try
            dsReasons = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetLoanSuspendReasons(Me.SessionLoanRequestId)
            If HelperFunctions.isNonEmpty(dsReasons) Then
                DropDownReasonsForLoanSuspension.DataSource = dsReasons.Tables(0)
                DropDownReasonsForLoanSuspension.DataTextField = "Reason"
                DropDownReasonsForLoanSuspension.DataValueField = "Code"
                DropDownReasonsForLoanSuspension.DataBind()
            End If
            If DropDownReasonsForLoanSuspension.Items.Count > 0 Then
                DropDownReasonsForLoanSuspension.SelectedIndex = 1
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_PopulateReasonForLoanSuspension", ex)
        End Try
    End Sub
    ' Start | SC | 2020.04.10 | YRS-AT-4852 | Added method to populate Loan suspend reasons into Dropdown List
    'START : MMR | 2019.01.23 | YRS-AT-4118 | Added to display Reamortization amount/months on change of YMCA and firsPaymentDate selection from dropdown.
    Private Function LoadLoanCloseDetails(ByVal loanRequestId As Integer, ByVal ymcaId As String, ByVal firstPaymentDate As String) As Boolean
        Dim closeLoanDetails As New DataSet
        Dim messageNo, status As Integer
        Dim unsuspendDate As String = ""

        Try
            status = 0
            If Me.SessionLoanStatus.Trim.ToUpper = "PAID" Then
                status = 0
            ElseIf Me.SessionLoanStatus.Trim.ToUpper = "SUSPND" Or Me.SessionLoanStatus.Trim.ToUpper = "UNSPND" Then
                status = 1
            End If
            If Not Session("UnsuspendDate") Is Nothing Then
                unsuspendDate = Session("UnsuspendDate")
            End If

            closeLoanDetails = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetLoanDetailsForCloseLoan(loanRequestId, status, unsuspendDate, ymcaId, firstPaymentDate, messageNo)

            If messageNo = 0 Then
                If HelperFunctions.isNonEmpty(closeLoanDetails) Then
                    If HelperFunctions.isNonEmpty(closeLoanDetails.Tables("ReamortizedAmount")) Then
                        Me.TextboxRemainingReamorizeAmount.Text = closeLoanDetails.Tables("ReamortizedAmount").Rows(0).Item("RemainingReamortizedAmount")
                        Me.TextBoxRemainingAmount.Text = closeLoanDetails.Tables("ReamortizedAmount").Rows(0).Item("RemainingReamortizedAmount")
                    End If

                    If HelperFunctions.isNonEmpty(closeLoanDetails.Tables("ReamortizedMonths")) Then
                        Me.TextboxRemainingReamorizeMonths.Text = closeLoanDetails.Tables("ReamortizedMonths").Rows(0).Item("RemainingReamortizedMonths")
                        Me.TextBoxRemainingMonths.Text = closeLoanDetails.Tables("ReamortizedMonths").Rows(0).Item("RemainingReamortizedMonths")
                    End If
                End If
                Return True
            Else
                Me.ButtonCloseLoan.Enabled = False
                Me.LabelCloseLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_CANNOT_CLOSED_BECAUSE) & " " & GetMessageByTextMessageNo(messageNo) & ""
                HelperFunctions.ShowMessageToUser(messageNo)
                Return False
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "LoadLoanCloseDetails", ex)
        Finally
            closeLoanDetails = Nothing
            messageNo = 0
            status = 0
            unsuspendDate = String.Empty
        End Try
    End Function
    'END : MMR | 2019.01.23 | YRS-AT-4118 | Added to display Reamortization amount/months on change of YMCA and firsPaymentDate selection from dropdown.
    'START: MMR | 2019.01.23 | YRS-AT-4118 | Fetching re-amoratization amount/months based on selected first payment date
    Private Sub DropdownFirstPaymentDate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropdownFirstPaymentDate.SelectedIndexChanged
        Me.LoadLoanCloseDetails(Me.SessionLoanRequestId, DropDownActiveYMCA.SelectedValue, DropdownFirstPaymentDate.SelectedValue)
    End Sub
    'END: MMR | 2019.01.23 | YRS-AT-4118 | Fetching re-amoratization amount/months based on selected first payment date

    ' START: SR | 2019.01.31 | YRS-AT-2920 | To Close Loan at Unfunded Amount warning message Yes button
    Private Sub btnUnfundedAmountYes_Click(sender As Object, e As EventArgs) Handles btnUnfundedAmountYes.Click
        Try
            CloseLoan()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanOptions_" + "btnYes_Click", ex)
        End Try
    End Sub
    ' END: SR | 2019.01.31 | YRS-AT-2920 | To Close Loan at Unfunded Amount warning message Yes button

    ' START: SR | 2019.01.31 | YRS-AT-2920 | Close loan at Unfunded Amount warning message Yes button

    ' END: SR | 2019.01.31 | YRS-AT-2920 | Created common procedure which will be called from Yes button of unfunded loan exist warning message and button "Terminate /Re-amortize Loan" .


    ' START: SR | 2019.01.31 | YRS-AT-2920 | Created common procedure which will be called from Yes button of unfunded loan exist warning message and button "Terminate /Re-amortize Loan" .
    Public Sub CloseLoan()
        Dim parameters As Dictionary(Of String, String) 'PK | 01.15.2019 | YRS-AT-2573 | code added here to show selected date from dropdown
        Try
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If

            'Start -- Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Display message after validating YMCA selection
            If Me.DropDownActiveYMCA.SelectedValue = "" Then
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_TERMINATE_NO_ACTIVE_YMCA_SELECTION)
                Exit Sub
            End If
            'End -- Manthan Rajguru | 2015.11.04 | YRS-AT-2453 | Display message after validating YMCA selection

            If Me.SessionLoanStatus.Trim.ToUpper = "PAID" Or Me.SessionLoanStatus.Trim.ToUpper = "UNSPND" Then
                If Me.IsActiveMultipleYMCA(Me.SessionLoanRequestId) = True Then
                    If Me.SessionOrigLoanNo = 0 Then
                        ' if Loan No is nt present then it cant proceed further.
                        HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_TERMINATE_LOAN_ORIGINAL_IS_INVALID)
                        Exit Sub
                    Else
                        Me.SessionParameterRequestField = "CLOSED"

                        'START: PK | 01.15.2019 | YRS-AT-2573 |code added here to show selected date from dropdown 
                        'If (IsLoanReamortizedEarlier(Me.SessionLoanRequestId)) Then
                        '    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_REAMORTIZATION) + "','Loan Re-Amortize');", True)
                        'Else
                        '    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_REAMORTIZE) + "','Loan Re-Amortize');", True)
                        'End If

                        parameters = New Dictionary(Of String, String)
                        parameters.Add("FirstPaymentDate", DropdownFirstPaymentDate.SelectedValue)
                        If (IsLoanReamortizedEarlier(Me.SessionLoanRequestId)) Then
                            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_REAMORTIZATION, parameters) + "','Loan Re-Amortize');", True)
                            Session("terminate_re-amortize") = "TerminateReamortize"
                            '''START: SR | 2019.01.31 | YRS-AT-2920 | commented session moved inside If else statement so that in all 3 cases (LoanReamortizedearlier/UnfundedPaymentExist/sUnfundedPayment not Exist), loan will be terminated appropriately.
                        ElseIf Not (IsUnfundedPaymentExist) Then 'If unfunded payment does not exists then diplay or prompt message.
                            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_REAMORTIZE, parameters) + "','Loan Re-Amortize');", True)
                            Session("terminate_re-amortize") = "TerminateReamortize"
                        ElseIf (IsUnfundedPaymentExist) Then 'If unfunded payment does not exists then do not diplay or prompt message.
                            Session("terminate_re-amortize") = "TerminateReamortize"
                            btnYes_Click(btnYes, EventArgs.Empty)
                        End If
                        'END:SR | 2019.01.31 | YRS-AT-2920 | commented session moved inside If else statement so that in all 3 cases (LoanReamortizedearlier/UnfundedPaymentExist/sUnfundedPayment not Exist), loan will be terminated appropriately.
                        'END: PK | 01.15.2019 | YRS-AT-2573 | code added here to show selected date from dropdown

                        'Session("terminate_re-amortize") = "TerminateReamortize" 'SR | 2019.01.31 | YRS-AT-2920 | commented session moved inside If else statement so that in all 3 cases (LoanReamortizedearlier/UnfundedPaymentExist/sUnfundedPayment not Exist), loan will be terminated appropriately.

                        Exit Sub
                    End If
                ElseIf Me.IsActiveMultipleYMCA(Me.SessionLoanRequestId) = False Then
                    Me.ButtonCloseLoan.Enabled = False
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_TERMINATE_NO_ACTIVE_EMPLOYEMENT_EXISTS)
                    Me.LabelCloseLoan.Text = GetMessageByTextMessageNo(MESSAGE_LOAN_CANNOT_TERMINATE_NO_ACTIVE_EMPLOYEMENT_EXISTS)
                    Exit Sub
                End If

            ElseIf Me.SessionLoanStatus.Trim.ToUpper <> "PAID" Or Me.SessionLoanStatus.Trim.ToUpper <> "UNSPND" Then
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CLOSED_NOT_OTHERTHAN_PAID_SUSPENDED)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ' END: SR | 2019.01.31 | YRS-AT-2920 | Created common procedure which will be called from Yes button of unfunded loan exist warning message and button "Terminate /Re-amortize Loan" .
    'START: Shilpa N | 03/25/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            ButtonPayOffLoan.Enabled = False
            ButtonPayOffLoan.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END: Shilpa N | 03/25/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class
