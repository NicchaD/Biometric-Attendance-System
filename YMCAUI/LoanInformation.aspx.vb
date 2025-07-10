'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA_YRS
' FileName			:	LoanInformationForm.aspx.vb
' Author Name		:	Ruchi Saxena
' Employee ID		:	33494
' Email				:	ruchi.saxena@icici-infotech.com    
' Contact No		:	8642
' Creation Time		:	March 9,2006
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
'*******************************************************************************
'Modification History
'***********************************************************************************************************************
'Modified By        Date            Desription
'************************************************************************************************************************
'Ashutosh Patil     09-Feb-2007     Exclusion of YMCA No from message and change in message for Mail sending  
'Ashutosh Patil     22-May-2007     Change in Email Functionality
'Shilpa N         | 02/28/2019  |   YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)
'************************************************************************************************************************
'Name :Shubhrata Tripathi Reason:TD Loans Phase 2 Date:Aug 28th 2006
'Shubhrata jan 8th 2006 to accomodate the changes made in the payoff amount saving in atsLoanRequest table
'Shubhrata Jan 13th 2006 YREN - 3014 to create new loan requests on termination in loan maintenance screen
'Shubhrata - Modified On Jan 25th 2007 Reason YREN-3035 to entail email functionality -- we will be setting 
'ymca no in this screen
'Mohammed Hafiz     6-Nov-2007      YRPS-4024
'Shubhrata Tripathi 28-Mar-208  YMCA Phase 4
'Mohammed Hafiz     8-Sep-2008  YRS 5.0-539
'Ashish Srivastava  28-Jan-2009 Added validation for QDRO Request pending
'Ashish Srivastava  04-Jan-2009 Change QDRO Request Pending message
'Priya              16-March-2009   Change Time of Customer Service Department 8:45 AM to 6:00 previously it was 8:45 AM to 8:00 as per Hafiz mail on March 13, 2009 
'Priya              17-March-2009   Made changes into MailMessage, message selected from database as per hafiz mail on 16-March-2009
'Dilip Yadav        2009.09.18     To limit the display on only 50 characters in Notes grid as per mail received on 17-Sep-09
'Neeraj Singh       12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Shashi Shekhar      12/Feb/2010        Restrict Data Archived Participants To proceed in Find list Except Person 
'Priya              2010-06-30          Changes made for enhancement in vs-2010 
'Shashi Shekhar     24-Dec-2010         For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'Shashi Shekhar     10 Feb 2011         For YRS 5.0-1236 : Need ability to freeze/lock account
'Shashi Shekhar     14 - Feb -2011      For BT-750 While QDRO split message showing wrong.
'Shashi             24 Feb 2011         Replacing Header formating with user control (YRS 5.0-450 )
'Shagufta Chaudhari 2011.09.05          For BT-750, While QDRO split message showing wrong.
'Anudeep            2013.07.02          Bt-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep            2013.08.23          Bt:2160-YRS 5.0-2179:Loan confirmation email to LPA sent to incorrect YMCA
'Anudeep            2013.08.28          Bt:2160-YRS 5.0-2179:Loan confirmation email to LPA sent to incorrect YMCA
'Anudeep            2013.10.29          BT-2278:YRS 5.0-2236 - After entering a YMCA # and hitting the enter key on the keyboard nothing happens.
'Anudeep            2014.02.03          BT:2292:YRS 5.0-2248 - YRS Pin number
'Anudeep A          08-sep-2014         BT:2625 :YRS 5.0-2405-Consistent screen header sections 
'Anudeep A          2015.05.05          BT-2699:YRS 5.0-2441 : Modifications for 403b Loans 
'Manthan Rajguru    2015.09.24          YRS-AT-2550: Custom Control reference given for DatagridcheckBox
'Manthan Rajguru    2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Manthan Rajguru    2018.04.03          YRS-AT-3929 -  YRS REPORTS: EFT: Loans ( FIRST EFT PROJECT) -create new "Loan Acknowledgement" Email for EFT/direct deposit
'Manthan Rajguru    2018.04.17          Removed commented code for tickets handled from year 2007 onwards (Related to YRS-AT-3929)
'Manthan Rajguru    2018.05.30          YRS-AT-3936 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for TD Loan "Request Processing" (TrackIT 33024) 
'Vinayan C          2018.10.11          YRS-AT-4018 -  YRS enh: Loan EFT Project:: “Update” YRS Maintenance: Person: Loan Tab  
'Manthan Rajguru    2018.10.12          YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
'Sanjay GS Rawat    2018.10.31          YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
'Pramod P. Pokale   2018.11.06          YRS-AT-3936 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for TD Loan "Request Processing" (TrackIT 33024)
'Shiny C.           2020.04.20          YRS-AT-4853 - COVID - YRS changes for Loan Limit Enhancement, due to CARE Act (Track IT - 41693) 
'******************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports System.IO
Imports YMCARET.YmcaBusinessObject.MetaMessageBO
Imports YMCAObjects.MetaMessageList


Public Class LoanInformation
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    'AA:2014.02.03:BT:2292:YRS 5.0 - 2248 : Changed the Formname according to the rights given in acuyussi table
    'Dim strFormName As String = New String("LoanInformation.aspx")
    Dim strFormName As String = New String("FindInfo.aspx?Name=LoanRequestAndProcessing")
    'End issue id YRS 5.0-940

    Dim dtFileList As New DataTable
    'Added by Shubhrata Oct 10th 2006
    'The variables are used to set the index for datagrid items
    Const mConst_DataGridLoansIndexOfPersId As Integer = 2
    Const mConst_DataGridLoansIndexOfEmpEventId As Integer = 3
    Const mConst_DataGridLoansIndexOfYmcaId As Integer = 4
    Const mConst_DataGridLoansIndexOfLoanNumber As Integer = 5
    Const mConst_DataGridLoansIndexOfTDBalance As Integer = 6
    Const mConst_DataGridLoansIndexOfRequestedAmount As Integer = 7
    Const mConst_DataGridLoansIndexOfRequestDate As Integer = 8
    Const mConst_DataGridLoansIndexOfRequestStatus As Integer = 9
    Const mConst_DataGridLoansIndexOfLoanRequestId As Integer = 10
    Const mConst_DataGridLoansIndexOfRequestOriginalLoanNumber As Integer = 11
    'Shubhrata Jan 8th 2006 
    Const mConst_DataGridLoansIndexOfSavedPayOffAmt As Integer = 12
    Const mConst_DataGridLoansIndexOfPayOffComputedOn As Integer = 13
    'Shubhrata Jan 8th 2006 
    'START : MMR | 2018.04.05 | YRS-AT-3929 | Set index value of datagrid items
    Const mConst_DataGridLoansIndexOfPaymentMethod As Integer = 14
    Const mConst_DataGridLoansIndexOfPersBankingEFTId As Integer = 15
    Const mConst_DataGridLoansIndexOfEFTPaymentStatus As Integer = 16
    'END : MMR | 2018.04.05 | YRS-AT-3929 | Set index value of datagrid items
    'START: MMR | 2018.05.30 | YRS-AT-3936 | Set index value of datagrid items
    Const mConst_DataGridLoansIndexOfONDType As Integer = 17
    Const mConst_DataGridLoansIndexOfAccountType As Integer = 18
    Const mConst_DataGridLoansIndexOfBankABA As Integer = 19
    Const mConst_DataGridLoansIndexOfBankAccountNumber As Integer = 20
    Const mConst_DataGridLoansIndexOfBankName As Integer = 21 'MMR | 2018.08.07 | YRS-AT-4017 | Set index value of datagrid items
    'END: MMR | 2018.05.30 | YRS-AT-3936 | Set index value of datagrid items
    Const mConst_DataGridLoansIndexOfCoolingPeriod As Integer = 22 'MMR | 2018.08.07 | YRS-AT-4017 | Set index value of datagrid items
    Const mConst_DataGridLoansIndexOfActualRequestStatus As Integer = 23 'VC | 2018.10.11 | YRS-AT-3936 | Set index value of attempted date for changing DEFALT status to DEFAULT
    Const mConst_DataGridLoansIndexOfErrorMessage As Integer = 24 'PPP | 2018.11.06 | YRS-AT-3936 | Set index value of Error Message
    'Added by Shubhrata Oct 10th 2006
    'Priya 08-07-2010  added guiid as readonly
    ReadOnly dummy_Guid As New Guid("00000000-0000-0000-0000-000000000000")
    'End Priya 08-07-2010 

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LabelNotFound As System.Web.UI.WebControls.Label
    Protected WithEvents LoansMultiPage As Microsoft.Web.UI.WebControls.MultiPage
    'Protected WithEvents DataGridList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents gvEmployment As System.Web.UI.WebControls.GridView
    Protected WithEvents gvFundedAccountContributions As System.Web.UI.WebControls.GridView
    Protected WithEvents gvNonFundedAccountContributions As System.Web.UI.WebControls.GridView
    Protected WithEvents gvLoans As System.Web.UI.WebControls.GridView
    Protected WithEvents gvNotes As System.Web.UI.WebControls.GridView
    'Protected WithEvents datagridCheckBoxStatusType As CustomControls.CheckBoxColumn ' Manthan Rajguru 2015-09-24 YRS-AT-2550: Commented as control not used anywhere in the page
    'Protected WithEvents datagridCheckBoxStatusType As YMCAObjects.CommonClass.DataGridCheckBox.CheckBoxColumn

    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddItem As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonLoanOptions As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddNote As System.Web.UI.WebControls.Button

    Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelFundNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFundNo As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelSal As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListSal As System.Web.UI.WebControls.DropDownList

    Protected WithEvents LabelFirst As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirst As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelMiddle As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMiddle As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelLast As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLast As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelSuffix As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSuffix As System.Web.UI.WebControls.TextBox

    Protected WithEvents AddressWebUserControl1 As AddressUserControl

    Protected WithEvents LabelTelephone As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTelephone As System.Web.UI.WebControls.TextBox

    Protected WithEvents TextBoxAge As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxMaritalStatus As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxVested1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTerminated As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelUpdatedBy As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxUpdatedBy As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelEmail As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEmail As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelUpdatedDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxUpdatedDate As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelVested As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxVested As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelYear As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxYear As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelMonth As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxMonth As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelPIATermination As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPIATermination As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelCurrentPIA As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxCurrentPIA As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelFundedEmpTaxable As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFundedEmpTaxable As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelFundedEmpNontaxable As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFundedEmpNontaxable As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelFundedEmpInterest As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFundedEmpInterest As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelFundedEmpTotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFundedEmpTotal As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelFundedYMCATaxable As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFundedYMCATaxable As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelFundedYMCAInterest As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFundedYMCAInterest As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelFundedYMCATotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFundedYMCATotal As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelFundedAcctTotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFundedAcctTotal As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelNonFundedEmpTaxable As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxNonFundedEmpTaxable As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelNonFundedEmpNontaxable As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxNonFundedEmpNontaxable As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelNonFundedEmpInterest As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxNonFundedEmpInterest As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelNonFundedEmpTotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxNonFundedEmpTotal As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelNonFundedYMCATaxable As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxNonFundedYMCATaxable As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelNonFundedYMCAInterest As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxNonFundedYMCAInterest As System.Web.UI.WebControls.TextBox


    Protected WithEvents LabelNonFundedYMCATotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxNonFundedYMCATotal As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelNonFundedAcctTotal As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxNonFundedAcctTotal As System.Web.UI.WebControls.TextBox
    'Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents LabelHeader As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents LoansTabStrip As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents ButtonLoanSummary As System.Web.UI.WebControls.Button

    Protected WithEvents HiddenSecControlName As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HiddenText As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents NotesFlag As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents btnYes As System.Web.UI.WebControls.Button
    'Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
#Region " Form Properties "


    ' To Get / Set terminated flag
    Private Property IsTerminated() As Boolean
        Get
            If Not Session("IsTerminated") Is Nothing Then
                Return (DirectCast(Session("IsTerminated"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsTerminated") = Value
        End Set
    End Property
    'To keep List DataGrid Index
    Private Property ListSessionRowIndex() As Integer
        Get
            If Not (Session("ListRowIndex")) Is Nothing Then
                Return (DirectCast(Session("ListRowIndex"), Integer))
            Else
                Return -1
            End If
        End Get

        Set(ByVal Value As Integer)
            Session("ListRowIndex") = Value
        End Set
    End Property

    'To Keep the Selected PersonID 
    Private Property SessionPersonID() As String
        Get
            If Not (Session("PersonID")) Is Nothing Then
                Return (DirectCast(Session("PersonID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PersonID") = Value
        End Set
    End Property

    'To Keep the FundID for Selected Member
    Private Property SessionFundID() As String
        Get
            If Not (Session("FundID")) Is Nothing Then
                Return (DirectCast(Session("FundID"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("FundID") = Value
        End Set
    End Property
    'Added by shubhrata TD LOANS phase 2--to keep the selected status
    Private Property SessionLoanStatus() As String
        Get
            If Not (Session("LoanStatus")) Is Nothing Then
                Return (DirectCast(Session("LoanStatus"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("LoanStatus") = Value
        End Set
    End Property
    'Added by shubhrata TD LOANS phase 2--to keep the selected status
    Private Property SessionAmountRequested() As String
        Get
            If Not (Session("AmountRequested")) Is Nothing Then
                Return (DirectCast(Session("AmountRequested"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("AmountRequested") = Value
        End Set
    End Property

    Private Property SessionAccountBalance() As String
        Get
            If Not (Session("AccountBalance")) Is Nothing Then
                Return (DirectCast(Session("AccountBalance"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("AccountBalance") = Value
        End Set
    End Property
    Private Property SessionTDBalance() As String
        Get
            If Not (Session("TDBalance")) Is Nothing Then
                Return (DirectCast(Session("TDBalance"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("TDBalance") = Value
        End Set
    End Property

    'To Keep the Notes DataGrid

    Private Property SessionIsNotes() As Boolean
        Get
            If Not (Session("IsNotes")) Is Nothing Then
                Return (CType(Session("IsNotes"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsNotes") = Value
        End Set
    End Property

    'To get / set the Notes ID, which is used in Notes Master form to View Details.
    Private Property SessionNotesIndex() As Integer
        Get
            If Not Session("NotesIndex") Is Nothing Then
                Return (DirectCast(Session("NotesIndex"), Integer))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("NotesIndex") = Value
        End Set
    End Property


    'To Keep the flag to Raise the Notes Popup window
    Private Property SessionIsNotesPopupAllowed() As Boolean
        Get
            If Not (Session("IsNotesPopupAllowed")) Is Nothing Then
                Return (CType(Session("IsNotesPopupAllowed"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsNotesPopupAllowed") = Value
        End Set
    End Property

    Private Property SessionLoanRequestId() As Integer
        Get
            If Not (Session("LoanRequestId")) Is Nothing Then
                Return (DirectCast(Session("LoanRequestId"), Integer))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("LoanRequestId") = Value
        End Set
    End Property

    'Shashi Shekhar:10 feb 2011: for YRS 5.0-1236
    Private Property IsAccountLock() As Boolean
        Get
            If Not Session("IsAccountLock") Is Nothing Then
                Return (CType(Session("IsAccountLock"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsAccountLock") = Value
        End Set
    End Property

    'START: MMR | 2018.04.04 | YRS-AT-3929 | Declared property to store payment method
    Public Property PaymentMethodCode() As String
        Get
            If Not Session("PaymentMethodCode") Is Nothing Then
                Return (CType(Session("PaymentMethodCode"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            Session("PaymentMethodCode") = value
        End Set
    End Property
    'END: MMR | 2018.04.04 | YRS-AT-3929 | Declared property to store payment method

    'START: MMR | 2018.04.11 | YRS-AT-3929 | Declared property to store EFT Payment Status (PENDING/PROOF/APPROVED/REJECTED)
    Public Property DisbursementEFTStatus() As String
        Get
            If Not ViewState("DisbursementEFTStatus") Is Nothing Then
                Return (CType(ViewState("DisbursementEFTStatus"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState("DisbursementEFTStatus") = value
        End Set
    End Property
    'END: MMR | 2018.04.11 | YRS-AT-3929 | Declared property to store EFT Payment Status (PENDING/PROOF/APPROVED/REJECTED)
    'START: MMR | 2018.05.30 | YRS-AT-3936 | Declared property to hold payment details such as Bank ABA, Account no, Account type and OND Type
    Public Property ONDType() As String
        Get
            If Not Session("ONDType") Is Nothing Then
                Return (CType(Session("ONDType"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            Session("ONDType") = value
        End Set
    End Property

    'Property to store bank name 
    Public Property BankName() As String
        Get
            If Not Session("BankName") Is Nothing Then
                Return (CType(Session("BankName"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            Session("BankName") = value
        End Set
    End Property

    Public Property AccountType() As String
        Get
            If Not Session("AccountType") Is Nothing Then
                Return (CType(Session("AccountType"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            Session("AccountType") = value
        End Set
    End Property

    Public Property BankABA() As String
        Get
            If Not Session("BankABA") Is Nothing Then
                Return (CType(Session("BankABA"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            Session("BankABA") = value
        End Set
    End Property

    Public Property BankAccountNumber() As String
        Get
            If Not Session("BankAccountNumber") Is Nothing Then
                Return (CType(Session("BankAccountNumber"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            Session("BankAccountNumber") = value
        End Set
    End Property
    'END: MMR | 2018.05.30 | YRS-AT-3936 | Declared property to hold payment details such as Bank ABA, Account no, Account type and OND Type

    ' Keeping this property name as LoanOriginationID instead of LoanNumber is because Session("LoanNumber") is used to store OriginalLoanNumber
    ' So not to confuse with that going for LoanOriginationID
    Public Property LoanOriginationID() As String
        Get
            If Not ViewState("LoanOriginationID") Is Nothing Then
                Return (CType(ViewState("LoanOriginationID"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState("LoanOriginationID") = value
        End Set
    End Property
#End Region
    ' Dim IDM As New IDforIDM
    Dim IDM As IDMforAll
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim l_StringMessage As String = ""
        Dim l_StringMsgEmail As String = ""
        Dim l_string_ToEmailAddrs As String = ""
        Dim l_boolEmailSent As Boolean = False
        Dim enumMailMsgType As EnumMessageTypes
        Try
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            CheckReadOnlyMode() 'Shilpa N | 02/28/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
            If Not IsPostBack Then
                'START: MMR | 2018.04.10 | YRS-AT-3929 | Clearing property value on first load
                Me.PaymentMethodCode = String.Empty
                Me.DisbursementEFTStatus = String.Empty
                'END: MMR | 2018.04.10 | YRS-AT-3929 | Clearing property value on first load
                Session("ReloadLoan") = Nothing
                'getSecuredControls()
                'Shubhrata Oct 10th 2006
                Session("LoanNumber") = Nothing
                'Shubhrata Oct 10th 2006
                DisableTabstrip()
                'Set TextBoxes to read only
                Me.SetTextBoxReadOnly()
                'by Aparna -Yren 3115 21/03/2007
                'same session being set for two modules
                Session("Reload") = Nothing

                LoadAllTabs()
            End If

            If Not Session("Reload") Is Nothing Then
                If Session("Reload") = True Then
                    Me.LoadMemberNotes(Me.SessionPersonID)
                    Session("Reload") = False
                End If
            End If

            Me.SetTextBoxReadOnly()
            If Not Session("ReloadLoan") Is Nothing Then
                If Session("ReloadLoan") = True Then
                    Me.LoadLoanRequests(Me.SessionPersonID)
                    Me.LoadAccountContribution(Me.SessionFundID)
                    Session("ReloadLoan") = Nothing
                ElseIf Session("ReloadLoan") = False Then
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_PROCESS_UNSUCCESFULL)
                    Session("ReloadLoan") = Nothing
                    Exit Sub
                End If
            End If

            'YREN 3035 Jan 25th 2007
            If CType(Session("LoanProcessed"), Boolean) = True Then
                Dim result As New YMCAObjects.ReturnObject(Of Boolean)
                Dim objMailutility As New YMCARET.YmcaBusinessObject.MailUtil

                Session("LoanProcessed") = False 'MMR: 2018.10.23 | YRS-AT-3929 | Setting session value to false avoid mutiple calls on post back
                'by Aparna 20/09/2007
                Dim l_string_reportname As String = String.Empty
                If Not String.IsNullOrEmpty(Me.PaymentMethodCode) AndAlso Me.PaymentMethodCode.ToUpper = YMCAObjects.PaymentMethod.CHECK Then 'MMR | 2018.04.04 | YRS-AT-3929 | Generate report for participant only when payment mode is "CHECK"
                    IDM = New IDMforAll   'added by hafiz on 4-May-2007            
                    If IDM.DatatableFileList(False) Then
                    Else
                        Throw New Exception("Unable to create dependent table")
                    End If

                    '"Loan Letter to Participant"
                    If Not Session("strReportName_1") Is Nothing Then
                        l_string_reportname = DirectCast(Session("strReportName_1"), String).Trim() + ".rpt"
                        'START: MMR | 2018.08.06 | YRS-AT-3929 | Removed hardcoded doc code "LNLETTR1" passed to method
                        'l_StringMessage = Me.GenerateReports(l_string_reportname, "P", "LNLETTR1")
                        l_StringMessage = Me.GenerateReports(l_string_reportname, "P", YMCAObjects.IDMDocumentCodes.LoanProcessing_ParticipantLetter)
                        'END: MMR | 2018.08.06 | YRS-AT-3929 | Removed hardcoded doc code "LNLETTR1" passed to method
                    End If
                    ' START : SR | 2018.10.31 | YRS-AT-3101 | Stop sending YMCA mail from here as we will send mail from common Mail routine.
                    ''SECOND REPORT TO ASSOCIATION
                    ''added by hafiz on 4-May-2007
                    'If IDM Is Nothing Then
                    '    IDM = New IDMforAll
                    'End If

                    'If Not Session("FTFileList") Is Nothing Then
                    '    IDM.SetdtFileList = DirectCast(Session("FTFileList"), DataTable)
                    'End If
                    ''END: MMR | 2018.04.04 | YRS-AT-3929 | Setting IDM Properties for report generation
                    'If Not Session("strReportName") Is Nothing Then
                    '    l_string_reportname = DirectCast(Session("strReportName"), String).Trim() + ".rpt"
                    '    'START: MMR | 2018.08.06 | YRS-AT-3929 | Removed hardcoded doc code "LNLETTR2" passed to method
                    '    'l_StringMessage = Me.GenerateReports(l_string_reportname, "A", "LNLETTR2")
                    '    l_StringMessage = Me.GenerateReports(l_string_reportname, "A", YMCAObjects.IDMDocumentCodes.LoanProcessing_YMCALetter)
                    '    'END: MMR | 2018.08.06 | YRS-AT-3929 | Removed hardcoded doc code "LNLETTR2" passed to method
                    'End If
                    ' END : SR | 2018.10.31 | YRS-AT-3101 | Stop sending YMCA mail from here as we will send mail from common Mail routine.

                    'Session("LoanProcessed") = False 'MMR: 2018.10.23 | YRS-AT-3929 | Commented existing code as declared above
                    Session("OpenReport") = True

                    '  l_StringMessage = IDM.ExportToPDF
                    'Jan 25th 2007 YREN-3035


                    If l_StringMessage = "" Then
                        'START: MMR | YRS-AT-3929 | Commented existing code as mail sending activity will be template driven
                        'Try
                        'If Not Session("FormYmcaNo") Is Nothing Then
                        '    l_string_ToEmailAddrs = Me.GetToEMailAddrs(Session("FormYmcaNo"))
                        '    If l_string_ToEmailAddrs = "" Then
                        '        l_StringMsgEmail = GetMessageByTextMessageNo(MESSAGE_LOAN_EMAIL_MISSING)
                        '        enumMailMsgType = EnumMessageTypes.Error
                        '    Else

                        '        'Anudeep:28.08.2013 - Bt:2160-YRS 5.0-2179:Changed for displaying email id missing message if email id does not exists
                        '        If Me.SendMail(l_string_ToEmailAddrs) = True Then
                        '            l_StringMsgEmail = GetMessageByTextMessageNo(MESSAGE_LOAN_EMAIL_SENT)
                        '            enumMailMsgType = EnumMessageTypes.Success
                        '        Else
                        '            l_StringMsgEmail = GetMessageByTextMessageNo(MESSAGE_LOAN_EMAIL_ERROR)
                        '            enumMailMsgType = EnumMessageTypes.Error
                        '        End If


                        '    End If
                        'END: MMR | YRS-AT-3929 | Commented existing code as mail sending activity wil be template driven

                        'Anudeep:28.08.2013 - Bt:2160-YRS 5.0-2179:Changed for displaying email id missing message if email id does not exists
                        l_StringMessage = GetMessageByTextMessageNo(MESSAGE_LOAN_PROCESSED_SUCCESFULLY)

                        'START:MMR | YRS-AT-3929 | Commented existing code as mail sending activity wil be template driven
                        'End If 
                        'Catch
                        'Throw
                        'END:MMR | YRS-AT-3929 | Commented existing code as mail sending activity wil be template driven
                        'l_StringMessage = "Error while processing Email"
                        'End Try MMR | YRS-AT-3929 | Commented existing code as mail sending activity wil be template driven

                        If l_StringMessage <> "" Then
                            HelperFunctions.ShowMessageToUser(l_StringMessage, EnumMessageTypes.Success)
                            '     Exit Sub
                        End If
                        'START:MMR | YRS-AT-3929 | Sending mail to ymca using template and commented existing message as all the activity will be done in method itself                        
                        result = objMailutility.SendLoanEmail(YMCAObjects.LoanStatus.DISB, Me.LoanOriginationID, Me.PaymentMethodCode, Nothing)
                        ShowMessage(result)
                        'If l_StringMsgEmail <> "" Then
                        '    HelperFunctions.ShowMessageToUser(l_StringMsgEmail, enumMailMsgType)
                        'End If
                        'END:MMR | YRS-AT-3929 | Sending mail to ymca using template and commented existing message as all the activity will be done in method itself
                    Else
                        HelperFunctions.ShowMessageToUser(l_StringMessage, EnumMessageTypes.Error)
                        '  Exit Sub
                    End If
                    'START: MMR | 2018.04.04 | YRS-AT-3929 | Send email to participant if payment mode is EFT & setting session value to nothing so that report does not open for viewing
                ElseIf Not String.IsNullOrEmpty(Me.PaymentMethodCode) AndAlso Me.PaymentMethodCode.ToUpper = YMCAObjects.PaymentMethod.EFT Then
                    Session("OpenReport") = Nothing
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_PROCESSED_SUCCESFULLY)
                    result = objMailutility.SendLoanEmail(YMCAObjects.LoanStatus.DISB, Me.LoanOriginationID, Me.PaymentMethodCode, "")
                    ShowMessage(result)
                End If
                'END: MMR | 2018.04.04 | YRS-AT-3929 | Send email to participant if payment mode is EFT & setting session value to nothing so that report does not open for viewing
            End If

            If Not Session("OpenReport") Is Nothing Then
                If Session("OpenReport") = True Then
                    Me.OpenReportViewer()
                    Session("OpenReport") = False
                End If
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_Page_load", ex)
        End Try
    End Sub


    Private Sub LoansTabStrip_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles LoansTabStrip.SelectedIndexChange
        Try

            LoansMultiPage.SelectedIndex = LoansTabStrip.SelectedIndex

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_LoansTabStrip_SelectedIndexChange", ex)
        End Try

    End Sub

    Private Sub ButtonAddItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddItem.Click
        Try
            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If
            'End : YRS 5.0-940 
            Dim l_dataset_LoanRequests As DataSet
            If Not Session("LoanRequestsDataSet") Is Nothing Then
                l_dataset_LoanRequests = DirectCast(Session("LoanRequestsDataSet"), DataSet)
                If Not l_dataset_LoanRequests.Tables(0) Is Nothing Then
                    If l_dataset_LoanRequests.Tables(0).Rows.Count > 0 Then
                        Dim l_datarow As DataRow

                        'Here I have checked for all the conditions PAID,DISB,PEND but in case of PAID Request we also need to check if the loan is closed 
                        'in AtsLoanDetails, if the loan is closed a new request can be added but as of now there is no data going in atsLoanDetails from our end
                        'so this check we will put while doing Phase2 of TD Loans
                        For Each l_datarow In l_dataset_LoanRequests.Tables(0).Rows
                            If l_datarow("RequestStatus").ToString.Trim.ToUpper = "PEND" Or l_datarow("RequestStatus").ToString.Trim.ToUpper = "DISB" Or l_datarow("RequestStatus").ToString.Trim.ToUpper = "PAID" Then
                                Exit Sub
                            End If
                        Next
                    End If
                End If
            End If
            If Session("FormYMCAId") = Nothing Then
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_PROCESS_PARTICIPANT_HAS_NO_ACTIVE_EMPLOYEMENT)
                Exit Sub
            End If
            Dim l_double_TDBalance As Double

            If Not Me.SessionAccountBalance Is Nothing Then
                If Me.SessionAccountBalance <> "" Then
                    l_double_TDBalance = Convert.ToDouble(Me.SessionAccountBalance)
                    If l_double_TDBalance >= 2000 Then
                        Dim popupScript As String = "<script language='javascript'>var a;" & _
                        "{a=window.open('LoanRequest.aspx', 'YMCAYRS', " & _
                        "'width=780,height=450, menubar=no,status=yes,Resizable=Yes,top=120,left=120, scrollbars=yes');a.focus(); }" & _
                        " </script>"
                        Dim RegisterScript As String

                        If (Not Me.IsStartupScriptRegistered("PopupScriptLoan")) Then
                            Page.RegisterStartupScript("PopupScriptLoan", popupScript)
                        End If
                    Else
                        HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_ADD_BALANCE_LESS_THAN_2000)
                    End If
                Else
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_ADD_NO_BALANCE)
                End If
            Else
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_ADD_NO_BALANCE)
            End If




        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_ButtonAddItem_Click", ex)
        End Try
    End Sub

    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Try
            'AA:2014.02.03:BT:2292:YRS 5.0-2248 -Added to open the Find Info page after clocking okay button
            Session("Page") = "Person"
            Response.Redirect("FindInfo.aspx?Name=LoanRequestAndProcessing", False)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_ButtonOk_Click", ex)
        End Try

    End Sub

    Private Sub ButtonAddNote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddNote.Click
        Try


            If Me.SessionPersonID <> String.Empty Then


                Dim popupScript As String = "window.open('NotesMasterWebForm.aspx?CF=RR', 'YMCAYRS', " & _
                                    "'width=750, height=360, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')"


                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScriptRRN", popupScript, True)



                If Me.SessionIsNotes = True Then
                    Me.LoadMemberNotes(Me.SessionPersonID)
                    Me.SessionIsNotes = False
                    Me.SessionIsNotesPopupAllowed = True
                End If

            Else
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_PERSON_SELECTION_IS_LOST)
            End If




        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_ButtonAddNote_Click", ex)
        End Try
    End Sub
    'Start:AA:2014.02.03:BT:2292:YRS 5.0-2248 -Commented below code to bacause finding the person part has been moved to Find INfo page
    'Public Sub LoadParticipantGrid()
    '    Dim l_dataset_Participants As DataSet
    '    Try
    '        l_dataset_Participants = YMCARET.YmcaBusinessObject.LoanInformationBOClass.LookUpPerson(Me.TextBoxListSSNo.Text.Trim(), Me.TextBoxListFundNo.Text.Trim(), Me.TextBoxLastName.Text.Trim(), Me.TextBoxFirstName.Text.Trim())

    '        If Not l_dataset_Participants Is Nothing Then
    '            If l_dataset_Participants.Tables("Participants").Rows.Count > 0 Then
    '                Me.DataGridList.DataSource = l_dataset_Participants.Tables("Participants")
    '                Me.DataGridList.DataBind()
    '                Me.LabelNotFound.Visible = False
    '                Me.DataGridList.Visible = True
    '            Else
    '                Me.LabelNotFound.Visible = True
    '                Me.DataGridList.Visible = False
    '            End If
    '        Else
    '            Me.LabelNotFound.Visible = True
    '            Me.DataGridList.Visible = False
    '        End If

    '    Catch sqlex As System.Data.SqlClient.SqlException
    '        If sqlex.Number = 60006 Then
    '            Me.LabelNotFound.Visible = True
    '            Me.DataGridList.Visible = False
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    'Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
    '    Try
    '        TextBoxListSSNo.Text = TextBoxListSSNo.Text.Replace("-", "") 'by Aparna 19/09/2007
    '        If (Me.TextBoxListFundNo.Text = "" And Me.TextBoxListSSNo.Text = "" And Me.TextBoxLastName.Text = "" And Me.TextBoxFirstName.Text = "") Then
    '            MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "This search will take a longer time. It is suggested that you enter a search criteria.", MessageBoxButtons.OK)
    '        Else

    '            Me.DataGridList.SelectedIndex = -1

    '            Me.LoadParticipantGrid()
    '        End If

    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub

    'Private Sub DataGridList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridList.ItemDataBound
    '    Try
    '        e.Item.Cells(1).Visible = False
    '        e.Item.Cells(8).Visible = False
    '        e.Item.Cells(9).Visible = False 'Shashi Shekhar:2010-02-12 :Hide IsArchived Field in grid
    '        e.Item.Cells(10).Visible = False 'Shashi Shekhar:2011-02-10 :Hide IsLock Field in grid
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub

    'Private Sub DataGridList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridList.SelectedIndexChanged
    '    Try


    '        '----Shashi Shekhar:2010-02-12: Code to handle Archived Participants from list--------------
    '        If Me.DataGridList.SelectedItem.Cells(9).Text.ToUpper.Trim() = "TRUE" Then
    '            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
    '            DisableTabstrip()
    '            'LabelTitle.Text = ""
    '            Exit Sub
    '        End If
    '        '---------------------------------------------------------------------------------------



    '        'Me.ButtonLoanSummary.Enabled = False
    '        Dim l_string_PersId As String
    '        Dim l_string_FundEventId As String = ""
    '        Me.SessionAccountBalance = Nothing
    '        Me.SessionTDBalance = Nothing
    '        l_string_PersId = Me.DataGridList.SelectedItem.Cells(1).Text.Trim
    '        l_string_FundEventId = Me.DataGridList.SelectedItem.Cells(8).Text.Trim
    '        Me.SessionPersonID = l_string_PersId
    '        Me.SessionFundID = l_string_FundEventId
    '        Headercontrol.pageTitle = "Loan"
    '        Headercontrol.guiPerssId = l_string_PersId.Trim
    '        'Shashi Shekhar:10 feb 2011: for YRS 5.0-1236
    '        Me.IsAccountLock = False
    '        Me.IsAccountLock = Me.DataGridList.SelectedItem.Cells(10).Text.Trim

    '        EnableTabstrip()
    '        Me.LoadGeneralInformation(l_string_PersId, l_string_FundEventId)
    '        Me.LoadAccountContribution(Me.SessionFundID)
    '        Me.LoadMemberNotes(l_string_PersId)
    '        Me.LoadCurrentPIA(Me.SessionFundID)
    '        LoadLoanRequests(l_string_PersId)

    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub
    'End:AA:2014.02.03:BT:2292:YRS 5.0-2248 -Commented below code to bacause finding the person part has been moved to Find INfo page
    Private Function DisableTabstrip()
        Try
            Me.LoansTabStrip.Items(0).Enabled = False
            Me.LoansTabStrip.Items(1).Enabled = False
            Me.LoansTabStrip.Items(2).Enabled = False
            Me.LoansTabStrip.Items(3).Enabled = False
            Me.LoansTabStrip.Items(4).Enabled = False
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_DisableTabstrip", ex)
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Function


    Private Function EnableTabstrip()
        Try
            Me.LoansTabStrip.Items(0).Enabled = True
            Me.LoansTabStrip.Items(1).Enabled = True
            Me.LoansTabStrip.Items(2).Enabled = True
            Me.LoansTabStrip.Items(3).Enabled = True
            Me.LoansTabStrip.Items(4).Enabled = True
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_EnableTabstrip", ex)
            'Dim l_String_Exception_Message As String
            'l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            'Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Function
    Private Function LoadGeneralInformation(ByVal parameterPersonID As String, ByVal parameterFundEventID As String)

        Dim l_DataSet As DataSet = Nothing
        Dim l_DataTable As DataTable = Nothing
        Session("FormYMCAId") = Nothing
        Dim l_address_Dataset As DataSet

        Try
            'Me.ButtonAddItem.Enabled = True    'commenting for YRS 5.0-539
            l_DataSet = YMCARET.YmcaBusinessObject.LoanInformationBOClass.LookupMemberDetails(parameterPersonID, parameterFundEventID)

            If l_DataSet Is Nothing Then Return 0

            ' To Fill general Information
            l_DataTable = l_DataSet.Tables("Member Details")


            If Not l_DataTable Is Nothing Then
                If l_DataTable.Rows.Count > 0 Then
                    Me.LoadGeneralInfoToControls(l_DataTable.Rows.Item(0))
                    Session("MaritalStatusCode") = l_DataTable.Rows(0).Item(13).ToString
                End If
            End If


            ' To Fill Address Inforamation in General Tab Page
            l_DataTable = l_DataSet.Tables("Member Address")

            If Not l_DataTable Is Nothing Then
                'Session("AddressInfo") = l_DataTable
                If l_DataTable.Rows.Count > 0 Then
                    Me.LoadAddressInfoToControls(l_DataTable.Rows.Item(0))
                End If
            End If
            'l_address_Dataset = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpAddressInfo(parameterPersonID)
            l_address_Dataset = Address.GetAddressByEntity(parameterPersonID, EnumEntityCode.PERSON)
            l_DataTable = l_address_Dataset.Tables("Address")
            Session("AddressInfo") = l_DataTable
            If Not l_DataTable Is Nothing Then
                If l_DataTable.Rows.Count > 0 Then
                    AddressWebUserControl1.LoadAddressDetail(l_DataTable.Select("isPrimary = True"))
                End If
            End If
            ' To fill Employment Information 
            l_DataTable = l_DataSet.Tables("Member Employment")
            'Anudeep:23.08.2013 - Bt:2160-YRS 5.0-2179:Loan confirmation email to LPA sent to incorrect YMCA
            Session("Member Employment") = l_DataTable
            Dim l_datarow As DataRow
            For Each l_datarow In l_DataTable.Rows
                If l_datarow("StatusType").ToString.ToUpper.Trim = "A" Then
                    Session("FormPersonId") = l_datarow("PersonID")
                    Session("FormEmpEventId") = l_datarow("UniqueID")
                    'Session("FormYMCAId") = l_datarow("YMCAID")
                    'YREN-3035 Shubhrata 
                    Session("FormYmcaNo") = l_datarow("YMCANo")
                End If
            Next

            Dim l_dataset_Ymca As DataSet
            l_dataset_Ymca = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetYMCAId(Session("FormPersonId"))
            If Not l_dataset_Ymca Is Nothing Then
                If l_dataset_Ymca.Tables(0).Rows.Count > 0 Then
                    If Not l_dataset_Ymca.Tables(0).Rows(0).Item("YmcaId").GetType.ToString = "System.DBNull" Then
                        Dim l_string_YmcaId As String

                        l_string_YmcaId = DirectCast(l_dataset_Ymca.Tables(0).Rows(0).Item("YmcaId"), String)
                        Session("FormYMCAId") = l_string_YmcaId
                    End If

                End If
            End If
            If Not l_DataTable Is Nothing Then
                LoadEmploymentDetails(l_DataTable)
            End If
            Session("PersonInformation") = l_DataSet

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_LoadGeneralInformation", ex)
        End Try

    End Function


    Private Function LoadEmploymentDetails(ByVal parameterDataTable As DataTable)

        Try
            If Not parameterDataTable Is Nothing Then

                Me.gvEmployment.DataSource = parameterDataTable
                Me.gvEmployment.DataBind()

            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_LoadEmploymentDetails", ex)
        End Try

    End Function

    Private Function LoadAddressInfoToControls(ByVal parameterDataRow As DataRow)
        Try
            If Not parameterDataRow Is Nothing Then

                If (parameterDataRow("Email").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxEmail.Text = String.Empty
                Else
                    Me.TextBoxEmail.Text = DirectCast(parameterDataRow("Email"), String)
                End If

                If (parameterDataRow("PhoneNumber").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxTelephone.Text = String.Empty
                Else
                    Me.TextBoxTelephone.Text = DirectCast(parameterDataRow("PhoneNumber"), String)
                End If
                'by Aparna -YREN-3036

                If (parameterDataRow("Updater").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxUpdatedBy.Text = String.Empty
                Else
                    Me.TextBoxUpdatedBy.Text = DirectCast(parameterDataRow("Updater"), String)
                End If

                If (parameterDataRow("Updated").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxUpdatedDate.Text = String.Empty
                Else
                    Me.TextBoxUpdatedDate.Text = CType(parameterDataRow("Updated"), String)
                End If

                'by Aparna -YREN-3036

            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_LoadAddressInfoToControls", ex)
        End Try

    End Function
    Private Function LoadGeneralInfoToControls(ByVal parameterDataRow As DataRow)
        Try

            If Not parameterDataRow Is Nothing Then

                Me.TextBoxSSNo.Text = CType(parameterDataRow("SS No"), String)
                Me.TextBoxFundNo.Text = CType(parameterDataRow("Fund No"), String)
                Me.TextBoxLast.Text = CType(parameterDataRow("Last Name"), String)
                Me.TextBoxMiddle.Text = CType(parameterDataRow("Middle Name"), String)
                Me.TextBoxFirst.Text = CType(parameterDataRow("First Name"), String)

                'Store the intFundno for the Selected Person,
                ' Modified to pass the FundNo into the IDM, by 34231 on 12/11/2006
                Session("Session_FundNO") = Me.TextBoxFundNo.Text().Trim()


                Dim strSSN As String = Me.TextBoxSSNo.Text.Insert(3, "-")
                strSSN = strSSN.Insert(6, "-")

                If (parameterDataRow("SalutationCode").GetType.ToString = "System.DBNull") Then
                    Me.DropDownListSal.SelectedValue = " "
                Else
                    If CType(parameterDataRow("SalutationCode"), String).Trim = "" Then
                        Me.DropDownListSal.SelectedValue = " "
                    Else
                        Me.DropDownListSal.SelectedValue = CType(parameterDataRow("SalutationCode"), String).Trim
                        'Me.LabelTitle.Text = Me.LabelTitle.Text
                    End If
                End If
                'Commented by Aparna -yren-3036

                If (parameterDataRow("VestingDate").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxVested1.Text = "No"
                    Me.TextBoxVested.Text = "No"

                Else
                    Me.TextBoxVested1.Text = "Yes"
                    Me.TextBoxVested.Text = "Yes"

                End If

                If (parameterDataRow("NonPaid").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxYear.Text = "0"
                    Me.TextBoxMonth.Text = "0"
                Else
                    Me.TextBoxYear.Text = Me.CalculateNonPaid(DirectCast(parameterDataRow("NonPaid"), Integer), "YEAR")
                    Me.TextBoxMonth.Text = Me.CalculateNonPaid(DirectCast(parameterDataRow("NonPaid"), Integer), "MONTH")
                End If

                If (parameterDataRow("StatusType").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxTerminated.Text = String.Empty
                    Me.IsTerminated = False
                Else

                    If ((DirectCast(parameterDataRow("StatusType"), String).Trim = "AE") Or ((DirectCast(parameterDataRow("StatusType"), String).Trim = "PE"))) Then
                        Me.TextBoxTerminated.Text = "No"
                        Me.IsTerminated = False
                    Else
                        Me.TextBoxTerminated.Text = "Yes"
                        Me.IsTerminated = False
                    End If

                End If

                If (parameterDataRow("MaritalStatus").GetType.ToString() = "System.DBNull") Then
                    Me.TextBoxMaritalStatus.Text = "Unknown"
                Else
                    Me.TextBoxMaritalStatus.Text = DirectCast(parameterDataRow("MaritalStatus"), String)
                    Session("MaritalStatus") = DirectCast(parameterDataRow("MaritalStatus"), String)
                End If

                If (parameterDataRow("BirthDate").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxAge.Text = "0.0"
                ElseIf (parameterDataRow("DeathDate").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxAge.Text = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), String.Empty).ToString("00.00")
                Else
                    Me.TextBoxAge.Text = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), CType(parameterDataRow("DeathDate"), String)).ToString("00.00")
                End If
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_LoadGeneralInfoToControls", ex)
        End Try

    End Function

    Private Function CalculateNonPaid(ByVal parameterNonPaid As Integer, ByVal parameterType As String) As Integer

        If parameterType = "YEAR" Then
            Return CType((System.Math.Floor(parameterNonPaid / 12.0)), Integer)
        Else
            Return CType(System.Math.Floor(parameterNonPaid Mod 12.0), Integer)
        End If

    End Function

    Private Function CalculateAge(ByVal parameterDOB As String, ByVal parameterDOD As String) As Decimal
        Try

            If parameterDOB = String.Empty Then Return 0.0

            If parameterDOD = String.Empty Then
                Return (CType(DateDiff(DateInterval.Month, CType(parameterDOB, DateTime), Now.Date), Decimal) / 12.0)
            Else
                Return ((CType(DateDiff(DateInterval.Year, CType(parameterDOB, DateTime), CType(parameterDOD, DateTime)), Decimal)) / 12.0)
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_CalculateAge", ex)
        End Try
    End Function

    Private Function LoadAccountContribution(ByVal parameterFundID As String)

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_GetLoanMaxAmount As String
        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session

        Try
            'AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to display the loan accounts in the tab
            l_DataSet = YMCARET.YmcaBusinessObject.LoanInformationBOClass.LookupMemberAccounts(parameterFundID, True)

            'START : SC | 2020.04.20 | YRS-AT-4853 | Moved the logic to calculate available TD Balance to database as per Covid Care act
            Dim l_FundNumber As Integer
            l_FundNumber = Integer.Parse(Me.TextBoxFundNo.Text().Trim())
            'l_GetLoanMaxAmount = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetLoanMaximumAmount()
            l_GetLoanMaxAmount = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetLoanMaximumAmount(l_FundNumber)
            'END : SC | 2020.04.20 | YRS-AT-4853 | Moved the logic to calculate available TD Balance to database as per Covid Care act

            If l_DataSet Is Nothing Then Return 0

            Dim dt_AccContr_Total As New DataTable
            Dim dt_AccContr_Total_Unfunded As New DataTable
            Dim l_DataRow As DataRow
            Dim l_DataRow_Unfunded As DataRow
            If Not l_DataSet.Tables("AccountForPaidTotal") Is Nothing Then
                dt_AccContr_Total = l_DataSet.Tables("AccountForPaidTotal").Clone


                l_DataRow = dt_AccContr_Total.NewRow()

                l_DataRow("Taxable") = l_DataSet.Tables("AccountForPaidTotal").Compute("SUM (Taxable)", "")
                l_DataRow("Non-Taxable") = l_DataSet.Tables("AccountForPaidTotal").Compute("SUM ([Non-Taxable])", "")
                l_DataRow("Interest") = l_DataSet.Tables("AccountForPaidTotal").Compute("SUM (Interest)", "")
                l_DataRow("Emp.Total") = l_DataSet.Tables("AccountForPaidTotal").Compute("SUM ([Emp.Total])", "")
                l_DataRow("YMCATaxable") = l_DataSet.Tables("AccountForPaidTotal").Compute("SUM (YMCATaxable)", "")
                l_DataRow("YMCAInterest") = l_DataSet.Tables("AccountForPaidTotal").Compute("SUM (YMCAInterest)", "")
                l_DataRow("YMCATotal") = l_DataSet.Tables("AccountForPaidTotal").Compute("SUM (YMCATotal)", "")
                l_DataRow("Total") = l_DataSet.Tables("AccountForPaidTotal").Compute("SUM (Total)", "")
                dt_AccContr_Total.Rows.Add(l_DataRow)


                Me.AddTotalIntoTable(l_DataSet.Tables("AccountForPaid"), dt_AccContr_Total)

            End If
            If Not l_DataSet.Tables("AccountForNonPaidTotal") Is Nothing Then
                dt_AccContr_Total_Unfunded = l_DataSet.Tables("AccountForNonPaidTotal").Clone
                l_DataRow_Unfunded = dt_AccContr_Total_Unfunded.NewRow()

                l_DataRow_Unfunded("Taxable") = l_DataSet.Tables("AccountForNonPaidTotal").Compute("SUM (Taxable)", "")
                l_DataRow_Unfunded("Non-Taxable") = l_DataSet.Tables("AccountForNonPaidTotal").Compute("SUM ([Non-Taxable])", "")
                l_DataRow_Unfunded("Interest") = l_DataSet.Tables("AccountForNonPaidTotal").Compute("SUM (Interest)", "")
                l_DataRow_Unfunded("Emp.Total") = l_DataSet.Tables("AccountForNonPaidTotal").Compute("SUM ([Emp.Total])", "")
                l_DataRow_Unfunded("YMCATaxable") = l_DataSet.Tables("AccountForNonPaidTotal").Compute("SUM (YMCATaxable)", "")
                l_DataRow_Unfunded("YMCAInterest") = l_DataSet.Tables("AccountForNonPaidTotal").Compute("SUM (YMCAInterest)", "")
                l_DataRow_Unfunded("YMCATotal") = l_DataSet.Tables("AccountForNonPaidTotal").Compute("SUM (YMCATotal)", "")
                l_DataRow_Unfunded("Total") = l_DataSet.Tables("AccountForNonPaidTotal").Compute("SUM (Total)", "")
                dt_AccContr_Total_Unfunded.Rows.Add(l_DataRow_Unfunded)


                Me.AddTotalIntoTable(l_DataSet.Tables("AccountForNonPaid"), dt_AccContr_Total_Unfunded)
            End If


            Me.gvFundedAccountContributions.DataSource = l_DataSet.Tables("AccountForPaid")
            Me.gvFundedAccountContributions.DataBind()

            Me.gvNonFundedAccountContributions.DataSource = l_DataSet.Tables("AccountForNonPaid")
            Me.gvNonFundedAccountContributions.DataBind()

            ' To set the selected  Index so that we can show total row in Bold Letters. :)
            Me.SetSelectedIndex(Me.gvFundedAccountContributions, l_DataSet.Tables("AccountForPaid"))
            Me.SetSelectedIndex(Me.gvNonFundedAccountContributions, l_DataSet.Tables("AccountForNonPaid"))


            Session("AccountContribution") = l_DataSet.Tables("AccountForPaid")
            Session("AccountContribution_NonFund") = l_DataSet.Tables("AccountForNonPaid")

            'Calculating the TD Amount Available
            Dim l_drow As DataRow
            Dim l_double_TDBalance As Double = 0
            Dim l_double_AccountBalance As Double = 0
            If Not l_DataSet Is Nothing Then
                If l_DataSet.Tables("AccountForPaid").Rows.Count > 0 Then
                    For Each l_drow In l_DataSet.Tables("AccountForPaid").Rows
                        If l_drow("AccountType").GetType.ToString <> "System.DBNull" And Convert.ToString(l_drow("AccountType")) <> "" Then
                            'Commented - Shubhrata Tripathi 28-Mar-208 YMCA Phase 4
                            'If Convert.ToString(l_drow("AccountType")).ToUpper = "TD" Then
                            'Added - Shubhrata Tripathi 28-Mar-2008 YMCA Phase 4
                            If Convert.ToString(l_drow("AccountType")).ToUpper = "TD" Or Convert.ToString(l_drow("AccountType")).ToUpper = "RT" Then
                                l_double_AccountBalance = l_double_AccountBalance + l_drow("Total")
                                'Me.SessionAccountBalance = l_drow("Total")
                            End If
                        End If
                    Next
                    'START : SC | 2020.04.20 | YRS-AT-4853 | Moved the logic to calculate available TD Balance to database as per Covid Care act
                    'If l_double_AccountBalance > 0 Then
                    '    If (Math.Round((l_double_AccountBalance / 2), 2) < CType(l_GetLoanMaxAmount, Double)) Then
                    '        l_double_TDBalance = l_double_TDBalance + Math.Round(l_double_AccountBalance / 2, 2)
                    '    Else
                    '        l_double_TDBalance = l_double_TDBalance + CType(l_GetLoanMaxAmount, Double)
                    '    End If
                    'End If
                    'Me.SessionTDBalance = l_double_TDBalance
                    Me.SessionTDBalance = Math.Round(CType(l_GetLoanMaxAmount, Double), 2)
                    'END : SC | 2020.04.20 | YRS-AT-4853 | Moved the logic to calculate available TD Balance to database as per Covid Care act
                    Me.SessionAccountBalance = l_double_AccountBalance
                End If
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_LoadAccountContribution", ex)
        End Try


    End Function
    Private Function AddTotalIntoTable(ByVal paramaterSourceDataTable As DataTable, ByVal parameterDataTable As DataTable) As DataTable

        Dim l_DataRow As DataRow
        Dim l_TotalDataRow As DataRow

        Try
            If parameterDataTable Is Nothing Then Return paramaterSourceDataTable

            If Not paramaterSourceDataTable Is Nothing Then

                '' Add a empty row in to DataTable

                l_DataRow = paramaterSourceDataTable.NewRow
                paramaterSourceDataTable.Rows.Add(l_DataRow)

                l_DataRow = paramaterSourceDataTable.NewRow
                paramaterSourceDataTable.Rows.Add(l_DataRow)

                If parameterDataTable.Rows.Count > 0 Then
                    l_TotalDataRow = parameterDataTable.Rows.Item(0) 'by Aparna  10-Aug-2007 to handle error  : no row at position 0
                End If


                l_DataRow = paramaterSourceDataTable.NewRow

                If Not l_TotalDataRow Is Nothing Then


                    l_DataRow("AccountType") = "Total"

                    'l_DataRow("YmcaID") = "0"
                    l_DataRow("YmcaID") = dummy_Guid

                    If (l_TotalDataRow("Taxable").GetType.ToString = "System.DBNull") Then
                        l_DataRow("Taxable") = "0.00"
                    Else
                        l_DataRow("Taxable") = l_TotalDataRow("Taxable")
                    End If

                    If (l_TotalDataRow("Non-Taxable").GetType.ToString = "System.DBNull") Then
                        l_DataRow("Non-Taxable") = "0.00"
                    Else
                        l_DataRow("Non-Taxable") = l_TotalDataRow("Non-Taxable")
                    End If

                    If (l_TotalDataRow("Interest").GetType.ToString = "System.DBNull") Then
                        l_DataRow("Interest") = "0.00"
                    Else
                        l_DataRow("Interest") = l_TotalDataRow("Interest")
                    End If

                    If (l_TotalDataRow("Emp.Total").GetType.ToString = "System.DBNull") Then
                        l_DataRow("Emp.Total") = "0.00"
                    Else
                        l_DataRow("Emp.Total") = l_TotalDataRow("Emp.Total")
                    End If

                    If (l_TotalDataRow("YMCATaxable").GetType.ToString = "System.DBNull") Then
                        l_DataRow("YMCATaxable") = "0.00"
                    Else
                        l_DataRow("YMCATaxable") = l_TotalDataRow("YMCATaxable")
                    End If

                    If (l_TotalDataRow("YMCAInterest").GetType.ToString = "System.DBNull") Then
                        l_DataRow("YMCAInterest") = "0.00"
                    Else
                        l_DataRow("YMCAInterest") = l_TotalDataRow("YMCAInterest")
                    End If

                    If (l_TotalDataRow("YMCATotal").GetType.ToString = "System.DBNull") Then
                        l_DataRow("YMCATotal") = "0.00"
                    Else
                        l_DataRow("YMCATotal") = l_TotalDataRow("YMCATotal")
                    End If

                    If (l_TotalDataRow("Total").GetType.ToString = "System.DBNull") Then
                        l_DataRow("Total") = "0.00"
                    Else
                        l_DataRow("Total") = l_TotalDataRow("Total")
                    End If
                    ''If Session("AccountBalance") Is Nothing Then
                    ''    Session("AccountBalance") = l_DataRow("Total")
                    ''End If
                Else
                    l_DataRow("AccountType") = "Total"
                    'l_DataRow("YmcaID") = "0"
                    l_DataRow("YmcaID") = dummy_Guid
                    l_DataRow("Taxable") = "0.00"
                    l_DataRow("Non-Taxable") = "0.00"
                    l_DataRow("Interest") = "0.00"
                    l_DataRow("Emp.Total") = "0.00"
                    l_DataRow("YMCATaxable") = "0.00"
                    l_DataRow("YMCAInterest") = "0.00"
                    l_DataRow("YMCATotal") = "0.00"
                    l_DataRow("Total") = "0.00"

                End If
                paramaterSourceDataTable.Rows.Add(l_DataRow)
            End If

            Return paramaterSourceDataTable

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_AddTotalIntoTable", ex)
        End Try

    End Function
    Private Function LoadMemberNotes(ByVal parameterPersonID)

        Dim l_DataTable As DataTable
        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session

        Try
            l_DataTable = YMCARET.YmcaBusinessObject.LoanInformationBOClass.MemberNotes(parameterPersonID)

            'by Aparna -yren-3115 15/03/2007
            If l_DataTable.Rows.Count > 0 Then
                Me.NotesFlag.Value = "Notes"
            Else
                Me.NotesFlag.Value = ""
            End If

            Me.gvNotes.DataSource = l_DataTable
            Me.gvNotes.DataBind()
            'by Aparna -yren-3115 15/03/2007


            If Me.NotesFlag.Value = "Notes" Then
                Me.LoansTabStrip.Items(4).Text = "<font color=orange>Notes</font>"
            ElseIf Me.NotesFlag.Value = "MarkedImportant" Then
                Me.LoansTabStrip.Items(4).Text = "<font color=red>Notes</font>"
            Else
                Me.LoansTabStrip.Items(4).Text = "Notes"
            End If
            'by Aparna -yren-3115 15/03/2007
            Session("MemberNotes") = l_DataTable

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_LoadMemberNotes", ex)
        End Try
    End Function

    Private Function LoadCurrentPIA(ByVal parameterFundID As String)

        Dim l_decimal_CurrentPIA As Decimal
        Dim l_decimal_PIATermination As Decimal

        Try

            l_decimal_CurrentPIA = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetCurrentPIA(parameterFundID)
            l_decimal_PIATermination = YMCARET.YmcaBusinessObject.LoanInformationBOClass.GetTerminatePIA(parameterFundID)

            If Me.IsTerminated = False Then
                l_decimal_PIATermination = l_decimal_CurrentPIA
            End If


            Me.TextBoxCurrentPIA.Text = FormatCurrency(l_decimal_CurrentPIA)
            Me.TextBoxPIATermination.Text = FormatCurrency(l_decimal_PIATermination)


            Session("CurrentPIA") = l_decimal_CurrentPIA
            Session("TerminationPIA") = l_decimal_PIATermination


        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_LoadCurrentPIA", ex)
        End Try
    End Function

    Private Function SetSelectedIndex(ByVal parameterDataGrid As GridView, ByVal parameterDataTable As DataTable)

        Try

            If parameterDataTable Is Nothing Then
                parameterDataGrid.SelectedIndex = -1
            Else
                parameterDataGrid.SelectedIndex = (parameterDataTable.Rows.Count - 1)
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_SetSelectedIndex", ex)
        End Try
    End Function
    Private Function SetFundID(ByVal parameterDataRow As DataRow) As String

        Dim l_DataSet As DataSet

        Try
            If parameterDataRow Is Nothing Then
                SessionFundID = String.Empty
            End If

            If (parameterDataRow("FundEventID").GetType.ToString = "String.DBNull") Then
                SessionFundID = String.Empty
            Else
                SessionFundID = CType(parameterDataRow("FundEventID"), String)
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_SetFundID", ex)
        End Try
    End Function

    Private Sub DataGridNotes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvNotes.SelectedIndexChanged
        Try
            If Me.gvNotes.SelectedIndex <> -1 Then
                Me.SessionNotesIndex = Me.gvNotes.SelectedIndex

                Dim popupScript As String = "window.open('NotesMasterWebForm.aspx?CF=RR&MD=VW', 'YMCAYRS', " & _
                        "'width=750, height=400, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes');"


                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScriptRRN", popupScript, True)

            End If


        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_DataGridNotes_SelectedIndexChanged", ex)
        End Try
    End Sub

    Private Sub DataGridNotes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvNotes.RowDataBound
        Try
            ''START: Added by Dilip Yadav : 2009.09.18 : To restrict the text display till only 50 characters.
            If e.Row.Cells(1).Text.Length > 50 Then
                e.Row.Cells(1).Text = e.Row.Cells(1).Text.Substring(0, 50)
            End If
            ''END: Added by Dilip Yadav : 2009.09.18 : To restrict the text display till only 50 characters.

            'by Aparna YREN-3115 15/03/2007
            ' e.Row.Cells(1).Visible = False
            Dim l_checkbox As New CheckBox
            If e.Row.RowType = DataControlRowType.DataRow Then
                l_checkbox = e.Row.FindControl("CheckBoxImportant")
                If l_checkbox.Checked Then
                    Me.NotesFlag.Value = "MarkedImportant"

                End If
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_DataGridNotes_RowDataBound", ex)
        End Try
    End Sub

    Private Sub gvFundedAccountContributions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFundedAccountContributions.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow AndAlso e.Row.Cells(0).Text.ToUpper <> "&NBSP;" Then
                Dim l_decimal_try As Decimal
                l_decimal_try = Convert.ToDecimal(e.Row.Cells(2).Text)
                e.Row.Cells(2).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Row.Cells(3).Text)
                e.Row.Cells(3).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Row.Cells(4).Text)
                e.Row.Cells(4).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Row.Cells(5).Text)
                e.Row.Cells(5).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Row.Cells(6).Text)
                e.Row.Cells(6).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Row.Cells(7).Text)
                e.Row.Cells(7).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Row.Cells(8).Text)
                e.Row.Cells(8).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Row.Cells(1).Text)
                e.Row.Cells(1).Text = FormatCurrency(l_decimal_try)
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_DataGridFundedAccountContributions_RowDataBound", ex)
        End Try
    End Sub
    Public Function FormatCurrency(ByVal paramNumber As Decimal) As String
        Try
            Dim n As String
            Dim m As String()
            Dim myNum As String
            'Changed by Ruchi on 7th March,2006
            Dim myDec As String
            'end of change
            Dim len As Integer
            Dim i As Integer
            Dim val As String
            If paramNumber = 0 Then
                val = 0
            Else
                n = paramNumber.ToString()
                m = (Math.Round(n * 100) / 100).ToString().Split(".")
                myNum = m(0).ToString()

                len = myNum.Length
                Dim fmat(len) As String
                For i = 0 To len - 1
                    fmat(i) = myNum.Chars(i)
                Next
                Array.Reverse(fmat)
                For i = 1 To len - 1
                    If i Mod 3 = 0 Then
                        fmat(i + 1) = fmat(i + 1) & ","
                    End If
                Next
                Array.Reverse(fmat)
                'start of change


                'end of change
                If m.Length = 1 Then
                    val = String.Join("", fmat) + ".00"
                Else
                    myDec = m(1).ToString
                    If myDec.Length = 1 Then
                        myDec = myDec + "0"
                    End If
                    val = String.Join("", fmat) + "." + myDec
                End If

            End If

            Return val

        Catch ex As Exception
            Return paramNumber
        End Try

    End Function

    Private Sub DataGridNonFundedAccountContributions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvNonFundedAccountContributions.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow AndAlso e.Row.Cells(0).Text.ToUpper <> "&NBSP;" Then
                Dim l_decimal_try As Decimal
                l_decimal_try = Convert.ToDecimal(e.Row.Cells(2).Text)
                e.Row.Cells(2).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Row.Cells(3).Text)
                e.Row.Cells(3).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Row.Cells(4).Text)
                e.Row.Cells(4).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Row.Cells(5).Text)
                e.Row.Cells(5).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Row.Cells(6).Text)
                e.Row.Cells(6).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Row.Cells(7).Text)
                e.Row.Cells(7).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Row.Cells(8).Text)
                e.Row.Cells(8).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Row.Cells(1).Text)
                e.Row.Cells(1).Text = FormatCurrency(l_decimal_try)

            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_DataGridNonFundedAccountContributions_RowDataBound", ex)
        End Try
    End Sub

    Private Sub SetTextBoxReadOnly()

        Me.TextBoxSSNo.ReadOnly = True
        Me.TextBoxFundNo.ReadOnly = True
        Me.DropDownListSal.Enabled = False
        Me.TextBoxFirst.ReadOnly = True
        Me.TextBoxMiddle.ReadOnly = True
        Me.TextBoxLast.ReadOnly = True
        Me.TextBoxSuffix.ReadOnly = True
        Me.AddressWebUserControl1.EnableControls = False
        Me.AddressWebUserControl1.MakeReadonly = True
        Me.TextBoxTelephone.ReadOnly = True
        Me.TextBoxUpdatedBy.ReadOnly = True
        Me.TextBoxEmail.ReadOnly = True
        Me.TextBoxUpdatedDate.ReadOnly = True

        Me.TextBoxAge.ReadOnly = True
        Me.TextBoxMaritalStatus.ReadOnly = True
        Me.TextBoxVested1.ReadOnly = True
        Me.TextBoxTerminated.ReadOnly = True
        Me.TextBoxVested.ReadOnly = True
        Me.TextBoxYear.ReadOnly = True
        Me.TextBoxMonth.ReadOnly = True
        Me.TextBoxPIATermination.ReadOnly = True
        Me.TextBoxCurrentPIA.ReadOnly = True


    End Sub
    Private Function LoadLoanRequests(ByVal parameterPersId As String)
        Dim l_dataset_LoanRequests As DataSet
        Try

            l_dataset_LoanRequests = YMCARET.YmcaBusinessObject.LoanInformationBOClass.LookUpLoanRequests(parameterPersId)

            If Not l_dataset_LoanRequests Is Nothing Then
                Me.gvLoans.DataSource = l_dataset_LoanRequests.Tables("LoanRequests")
                Me.gvLoans.DataBind()
                Session("LoanRequestsDataSet") = l_dataset_LoanRequests
                Dim l_datarow As DataRow
                For Each l_datarow In l_dataset_LoanRequests.Tables("LoanRequests").Rows
                    If l_datarow("ActualRequestStatus").ToString.ToUpper = "DISB" Or l_datarow("ActualRequestStatus").ToString.ToUpper = "PAID" Or l_datarow("ActualRequestStatus").ToString.ToUpper = "SUSPND" Or l_datarow("ActualRequestStatus").ToString.ToUpper = "DEFALT" Or l_datarow("ActualRequestStatus").ToString.ToUpper = "REAMRT" Or l_datarow("ActualRequestStatus").ToString.ToUpper = "UNSPND" Then 'VC | 2018.10.11 | YRS-AT-4018 | Replaced RequestStatus with ActualRequestStatus for changing DEFALT status to DEFAULT
                        'Me.ButtonAddItem.Enabled = False   'commenting for YRS 5.0-539
                        'Me.ButtonLoanSummary.Enabled = True
                        Exit For
                        'ElseIf l_datarow("RequestStatus").ToString.ToUpper = "PEND" Or l_datarow("RequestStatus").ToString.ToUpper = "EXP" Then 'commented by hafiz on 6-Nov-2007 for YRPS-4024
                        'START: 'VC | 2018.10.11 | YRS-AT-4018 | commented existing code and added new code for changing DEFALT status to DEFAULT
                        'ElseIf l_datarow("RequestStatus").ToString.ToUpper = "PEND" Then 'modified by hafiz on 6-Nov-2007 for YRPS-4024
                    ElseIf l_datarow("ActualRequestStatus").ToString.ToUpper = "PEND" Then
                        'END: 'VC | 2018.10.11 | YRS-AT-4018 | commented existing code and added new code for changing DEFALT status to DEFAULT
                        'Me.ButtonAddItem.Enabled = False   'commenting for YRS 5.0-539
                        'Me.ButtonLoanSummary.Enabled = False
                        Exit For
                    ElseIf l_datarow("ActualRequestStatus").ToString.ToUpper = "CANCEL" Then 'VC | 2018.10.11 | YRS-AT-4018 | Replaced RequestStatus with ActualRequestStatus for changing DEFALT status to DEFAULT
                        'Me.ButtonAddItem.Enabled = True    'commenting for YRS 5.0-539
                        'Me.ButtonLoanSummary.Enabled = False
                    Else
                        'Me.ButtonAddItem.Enabled = True    'commenting for YRS 5.0-539
                        'Me.ButtonLoanSummary.Enabled = True
                    End If
                Next

            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_LoadLoanRequests", ex)
        End Try
    End Function

    Private Sub gvLoans_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvLoans.RowCancelingEdit
        Try

            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940

            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If

            Dim l_int_count As Integer
            'Anudeep:23.08.2013 - Bt:2160-YRS 5.0-2179:Loan confirmation email to LPA sent to incorrect YMCA
            Dim l_datatable As DataTable

            'Commented By Shubhrata Oct 10th 2006
            'l_int_count = YMCARET.YmcaBusinessObject.LoanInformationBOClass.CheckForLoanId(Convert.ToInt32(e.Item.Cells(9).Text))
            'Added by Shubhrata Oct 10th 2006
            l_int_count = YMCARET.YmcaBusinessObject.LoanInformationBOClass.CheckForLoanId(Convert.ToInt32(gvLoans.Rows(e.RowIndex).Cells(mConst_DataGridLoansIndexOfLoanRequestId).Text))
            Me.SessionLoanStatus = gvLoans.Rows(e.RowIndex).Cells(mConst_DataGridLoansIndexOfActualRequestStatus).Text.ToUpper 'VC | 2018.10.11 | YRS-AT-4018 | Replaced mConst_DataGridLoansIndexOfRequestStatus with mConst_DataGridLoansIndexOfActualRequestStatus for changing DEFALT status to DEFAULT
            Me.SessionLoanRequestId = gvLoans.Rows(e.RowIndex).Cells(mConst_DataGridLoansIndexOfLoanRequestId).Text
            Me.DisbursementEFTStatus = gvLoans.Rows(e.RowIndex).Cells(mConst_DataGridLoansIndexOfEFTPaymentStatus).Text 'MMR | 2018.04.11 | YRS-AT-3929 | Setting disbursed loan EFT payment status in view state which will help us to validate loan request cancellation 
            Me.PaymentMethodCode = gvLoans.Rows(e.RowIndex).Cells(mConst_DataGridLoansIndexOfPaymentMethod).Text
            If gvLoans.Rows(e.RowIndex).Cells(mConst_DataGridLoansIndexOfRequestOriginalLoanNumber).Text.Trim().ToUpper <> "&NBSP;" Then
                Session("LoanNumber") = gvLoans.Rows(e.RowIndex).Cells(mConst_DataGridLoansIndexOfRequestOriginalLoanNumber).Text.Trim()
            Else
                Session("LoanNumber") = 0
            End If
            Me.LoanOriginationID = gvLoans.Rows(e.RowIndex).Cells(mConst_DataGridLoansIndexOfLoanNumber).Text.Trim()

            If Me.CancelSelectLoanRequest(Me.SessionLoanStatus.Trim.ToUpper, l_int_count, Me.SessionLoanRequestId) = True Then
                Exit Sub
            ElseIf gvLoans.Rows(e.RowIndex).Cells(mConst_DataGridLoansIndexOfActualRequestStatus).Text.ToUpper = "PEND" Or gvLoans.Rows(e.RowIndex).Cells(mConst_DataGridLoansIndexOfActualRequestStatus).Text.ToUpper = "DISB" Then 'VC | 2018.10.11 | YRS-AT-4018 | Replaced mConst_DataGridLoansIndexOfRequestStatus with mConst_DataGridLoansIndexOfActualRequestStatus for changing DEFALT status to DEFAULT
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "YMCA-YRS", "showDialog('" + GetMessageByTextMessageNo(MESSAGE_LOAN_CONFIRM_CANCEL) + "','Loan Cancellation');", True)
                Exit Sub
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("gvLoans_RowCancelingEdit", ex)
        End Try
    End Sub


    Private Sub gvLoans_SelectedIndexChanged(ByVal source As Object, ByVal e As System.EventArgs) Handles gvLoans.SelectedIndexChanged
        Dim LabelModuleName As Label
        Dim dictStringParam As Dictionary(Of String, String)
        Try
            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940

            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error)
                Exit Sub
            End If


            'End : YRS 5.0-940 

            Dim l_int_count As Integer
            'Anudeep:23.08.2013 - Bt:2160-YRS 5.0-2179:Loan confirmation email to LPA sent to incorrect YMCA
            Dim l_datatable As DataTable

            'Commented By Shubhrata Oct 10th 2006
            'l_int_count = YMCARET.YmcaBusinessObject.LoanInformationBOClass.CheckForLoanId(Convert.ToInt32(gvLoans.SelectedRow.Cells(9).Text))
            'Added by Shubhrata Oct 10th 2006
            l_int_count = YMCARET.YmcaBusinessObject.LoanInformationBOClass.CheckForLoanId(Convert.ToInt32(gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfLoanRequestId).Text))
            Me.SessionLoanStatus = gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfActualRequestStatus).Text.ToUpper 'VC | 2018.10.11 | YRS-AT-4018 | Replaced mConst_DataGridLoansIndexOfRequestStatus with mConst_DataGridLoansIndexOfActualRequestStatus for changing DEFALT status to DEFAULT
            Me.SessionLoanRequestId = gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfLoanRequestId).Text
            'START: MMR | 2018.04.05 | YRS-AT-3929 | Setting payment mode in view state which will help to identify to send email to participant or not and also showing error message if payment method is invalid
            Me.PaymentMethodCode = gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfPaymentMethod).Text
            If Me.PaymentMethodCode <> YMCAObjects.PaymentMethod.CHECK AndAlso Me.PaymentMethodCode <> YMCAObjects.PaymentMethod.EFT Then
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_INVALID_PAYMENT_METHOD)
                Exit Sub
            End If
            'END: MMR | 2018.04.05 | YRS-AT-3929 | Setting payment mode in view state which will help to identify to send email to participant or not and also showing error message if payment method is invalid
            'START: MMR | 2018.05.30 | YRS-AT-3936 | Setting payment details in session which will be used for displaying payment details in loan processing screen
            Me.ONDType = gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfONDType).Text.ToUpper
            Me.BankName = gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfBankName).Text
            Me.BankName = Me.BankName.Replace("&nbsp;", "").Trim()
            Me.AccountType = gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfAccountType).Text.Trim()
            Me.AccountType = Me.AccountType.Replace("&nbsp;", "").Trim()
            Me.BankABA = gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfBankABA).Text.Trim()
            Me.BankABA = Me.BankABA.Replace("&nbsp;", "").Trim()
            Me.BankAccountNumber = gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfBankAccountNumber).Text.Trim()
            Me.BankAccountNumber = Me.BankAccountNumber.Replace("&nbsp;", "").Trim()
            'END: MMR | 2018.05.30 | YRS-AT-3936 | Setting payment details in session which will be used for displaying payment details in loan processing screen
            If gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfRequestOriginalLoanNumber).Text.Trim().ToUpper <> "&NBSP;" Then
                Session("LoanNumber") = gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfRequestOriginalLoanNumber).Text.Trim()
            Else
                Session("LoanNumber") = 0
            End If
            Me.LoanOriginationID = gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfLoanNumber).Text.Trim()

            If Me.IsAccountLock.ToString.Trim.ToLower = "true" Then

                Dim strSSN As String = Me.TextBoxSSNo.Text 'added by shagufta

                Dim l_dsLockResDetails As DataSet
                Dim l_reasonLock As String
                If Not strSSN = "" Then
                    l_dsLockResDetails = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetLockReasonDetails(strSSN.ToString().Trim)
                End If
                Session("splitfundSSN") = Nothing
                If Not l_dsLockResDetails Is Nothing Then
                    If l_dsLockResDetails.Tables("GetLockReasonDetails").Rows.Count > 0 Then
                        If (l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString() <> "System.DBNull" And l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString() <> "") Then
                            l_reasonLock = l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString().Trim
                        End If
                    End If
                End If
                If l_reasonLock = "" Then
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_PARTICIPANT_ACCOUNT_IS_LOCKED)
                Else
                    dictStringParam = New Dictionary(Of String, String)
                    dictStringParam.Add("REASON", l_reasonLock)
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_PARTICIPANT_ACCOUNT_IS_LOCKED_DUE_TO, dictStringParam)
                End If

                Exit Sub
            End If

            If gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfActualRequestStatus).Text.ToUpper = "PEND" Then 'VC | 2018.10.11 | YRS-AT-4018 | Replaced mConst_DataGridLoansIndexOfRequestStatus with mConst_DataGridLoansIndexOfActualRequestStatus for changing DEFALT status to DEFAULT
                If l_int_count = 1 Then

                    'START: MMR | 2018.08.07 | YRS-AT-4017 | Added validation to not allow loan processing if cooling period is not crossed from current date 
                    Dim coolingPeriod As Date
                    If Not String.IsNullOrEmpty(gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfCoolingPeriod).Text) AndAlso gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfCoolingPeriod).Text.ToLower <> "&nbsp;" Then
                        coolingPeriod = Convert.ToDateTime(gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfCoolingPeriod).Text).Date
                        If coolingPeriod >= Date.Today Then
                            HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_COOLING_PERIOD_ERROR)
                            Exit Sub
                        End If
                    End If
                    'END: MMR | 2018.08.07 | YRS-AT-4017 | Added validation to not allow loan processing if cooling period is not crossed from current date
                    '    If True Then
                    'Added By Ashish 27-Jan-2009, Start
                    If ValidateQDRORequestPending() = True Then
                        HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_PARTICIPANT_HAS_QDRO_REQUEST)
                        Exit Sub
                    End If

                    'START: PPP | 2018/11/06 | YRS-AT-3936 | Applying similar validations which are used at LAC Multi Loan Processing screen
                    Dim errorMessage As String = gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfErrorMessage).Text.Trim.Replace("&nbsp;", "")
                    errorMessage = errorMessage.Replace("&NBSP;", "")
                    If Not String.IsNullOrEmpty(errorMessage) Then
                        errorMessage = errorMessage.Replace("<br />", "")
                        errorMessage = errorMessage.Replace("&lt;br /&gt;", "")
                        HelperFunctions.ShowMessageToUser(errorMessage, EnumMessageTypes.Error)
                        Exit Sub
                    End If
                    'END: PPP | 2018/11/06 | YRS-AT-3936 | Applying similar validations which are used at LAC Multi Loan Processing screen

                    'Added By Ashish 27-Jan-2009, End
                    Me.SessionAmountRequested = gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfRequestedAmount).Text
                    'Start:Anudeep:23.08.2013 - Bt:2160-YRS 5.0-2179:Loan confirmation email to LPA sent to incorrect YMCA
                    l_datatable = Session("Member Employment")
                    If HelperFunctions.isNonEmpty(l_datatable) Then
                        Me.Session("FormYmcaNo") = l_datatable.Select("YMCAID = '" + gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfYmcaId).Text + "'")(0)("YMCANo")
                        'Anudeep:28.08.2013 - Bt:2160-YRS 5.0-2179:Changed for displaying entityid of requested ymca in atsOutputFileIDMTrackingLogs 
                        Me.Session("FormYMCAId") = gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfYmcaId).Text
                    End If
                    'Start:Anudeep:23.08.2013 - Bt:2160-YRS 5.0-2179:Loan confirmation email to LPA sent to incorrect YMCA
                    'Me.SessionLoanRequestId = gvLoans.SelectedRow.Cells(9).Text
                    Dim popupScript As String = "var a;" & _
                    "if(a==null ||a.closed || a.name==undefined){ a=window.open('LoanProcessing.aspx', 'YMCAYRS', " & _
                    "'width=780,height=650, menubar=no,status=yes, Resizable=no,top=80,left=120, scrollbars=no');a.focus(); }" '& _
                    '" </script>"

                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScriptLoan", popupScript, True)

                    LabelModuleName = Master.FindControl("LabelModuleName")
                    If LabelModuleName IsNot Nothing Then
                        Session("Title") = LabelModuleName.Text
                    End If

                ElseIf l_int_count = 0 Then
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_PROCESS_DETAILS_MISSING)
                    Exit Sub
                ElseIf l_int_count > 1 Then
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_PROCESS_MULTIPLE_LOANS_EXISTS)
                    Exit Sub
                End If
                'Shubhrata Jan 15th 2007 YREN - 3014
            ElseIf gvLoans.SelectedRow.Cells(mConst_DataGridLoansIndexOfActualRequestStatus).Text.ToUpper = "REAMRT" Then 'VC | 2018.10.11 | YRS-AT-4018 | Replaced mConst_DataGridLoansIndexOfRequestStatus with mConst_DataGridLoansIndexOfActualRequestStatus for changing DEFALT status to DEFAULT
                If l_int_count = 0 Then
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_PROCESS_DETAILS_MISSING)
                    Exit Sub
                ElseIf l_int_count > 1 Then
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_PROCESS_MULTIPLE_LOANS_EXISTS)
                    Exit Sub
                ElseIf l_int_count = 1 Then
                    YMCARET.YmcaBusinessObject.LoanInformationBOClass.LoanProcessingAfterReamortization(Me.SessionLoanRequestId)
                    Me.LoadLoanRequests(Me.SessionPersonID)
                    Me.LoadAccountContribution(Me.SessionFundID)
                    HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_PROCESSED_SUCCESFULLY)

                    Exit Sub
                End If

                'Shubhrata Jan 15th 2007 YREN - 3014
            Else
                HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANNOT_PROCESS_ONLY_PENDING_NOTEXISTS)
                Exit Sub
            End If

            'End:Shagufta chaudhari:2011.09.05:BT-750





        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_gvLoans_SelectedIndexChanged", ex)
        End Try
    End Sub

    Private Sub DataGridLoans_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLoans.RowDataBound
        Try
            'shubhrata oct10th2006
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(mConst_DataGridLoansIndexOfTDBalance).Style.Add(HtmlTextWriterStyle.TextAlign, TextAlign.Right.ToString)
                e.Row.Cells(mConst_DataGridLoansIndexOfRequestedAmount).Style.Add(HtmlTextWriterStyle.TextAlign, TextAlign.Right.ToString)
            End If
            e.Row.Cells(mConst_DataGridLoansIndexOfRequestedAmount).Style.Add(HtmlTextWriterStyle.Width, "115px")
            e.Row.Cells(mConst_DataGridLoansIndexOfPersId).Visible = False
            e.Row.Cells(mConst_DataGridLoansIndexOfEmpEventId).Visible = False
            e.Row.Cells(mConst_DataGridLoansIndexOfYmcaId).Visible = False
            e.Row.Cells(mConst_DataGridLoansIndexOfLoanRequestId).Visible = False
            'shubhrata oct10th2006
            'Shubhrata Jan 8th 2006
            e.Row.Cells(mConst_DataGridLoansIndexOfSavedPayOffAmt).Visible = False
            e.Row.Cells(mConst_DataGridLoansIndexOfPayOffComputedOn).Visible = False
            'Shubhrata Jan 8th 2006
            'START: MMR | 2018.04.11 | YRS-AT-3929 | Hiding columns which are not to be displayed on loan information listing screen
            e.Row.Cells(mConst_DataGridLoansIndexOfPersBankingEFTId).Visible = False
            e.Row.Cells(mConst_DataGridLoansIndexOfEFTPaymentStatus).Visible = False
            'END: MMR | 2018.04.11 | YRS-AT-3929 | Hiding columns which are not to be displayed on loan information listing screen
            'START: MMR | 2018.05.30 | YRS-AT-3936 | Hiding columns which are not to be displayed on loan information listing screen
            e.Row.Cells(mConst_DataGridLoansIndexOfONDType).Visible = False
            e.Row.Cells(mConst_DataGridLoansIndexOfAccountType).Visible = False
            e.Row.Cells(mConst_DataGridLoansIndexOfBankABA).Visible = False
            e.Row.Cells(mConst_DataGridLoansIndexOfBankAccountNumber).Visible = False
            e.Row.Cells(mConst_DataGridLoansIndexOfBankName).Visible = False 'MMR | 2018.08.07 | YRS-AT-4017 | Hiding columns which are not to be displayed on loan information listing screen
            'END: MMR | 2018.05.30 | YRS-AT-3936 | Hiding columns which are not to be displayed on loan information listing screen
            e.Row.Cells(mConst_DataGridLoansIndexOfCoolingPeriod).Visible = False 'MMR | 2018.08.07 | YRS-AT-4017 | Hiding columns which are not to be displayed on loan information listing screen
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_DataGridLoans_RowDataBound", ex)
        End Try
    End Sub
    'START: MMR | 2018.10.11 | YRS-AT-3101 | Commented existing code and added single routine to cancel loan request
    'Private Function CancelDisbLoanRequest(ByVal parameterLoanRequestId)
    '    Try
    '        Dim l_string_message As String
    '        l_string_message = ""
    '        l_string_message = YMCARET.YmcaBusinessObject.LoanInformationBOClass.CancelDisbLoanRequest(parameterLoanRequestId)
    '        If l_string_message <> "" Then
    '            HelperFunctions.ShowMessageToUser(l_string_message, EnumMessageTypes.Error)
    '        Else
    '            HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANCELLED)
    '        End If

    '        Me.LoadLoanRequests(Me.SessionPersonID)
    '    Catch SQLEx As SqlClient.SqlException
    '        HelperFunctions.ShowMessageToUser(SQLEx.Message.ToString, EnumMessageTypes.Error)
    '        HelperFunctions.LogException("LoanInformation_CancelDisbLoanRequest", SQLEx)
    '    Catch ex As Exception
    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
    '        HelperFunctions.LogException("LoanInformation_CancelDisbLoanRequest", ex)
    '    End Try
    'End Function

    Private Sub CancelLoanRequest()
        Dim errorMessage As String
        Dim objMailutility As YMCARET.YmcaBusinessObject.MailUtil
        Dim result As YMCAObjects.ReturnObject(Of Boolean)
        Try
            result = New YMCAObjects.ReturnObject(Of Boolean)()
            result = YMCARET.YmcaBusinessObject.LoanInformationBOClass.CancelLoanRequest(Me.SessionLoanRequestId, Me.SessionPersonID, Me.PaymentMethodCode, Me.LoanOriginationID)
            ShowMessage(result)
            Me.LoadLoanRequests(Me.SessionPersonID)
        Catch SQLEx As SqlClient.SqlException
            HelperFunctions.LogException("LoanInformation_CancelLoanRequest", SQLEx)
            HelperFunctions.ShowMessageToUser(SQLEx.Message.ToString, EnumMessageTypes.Error)
        Catch ex As Exception
            HelperFunctions.LogException("LoanInformation_CancelLoanRequest", ex)
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
        Finally
            errorMessage = Nothing
            objMailutility = Nothing
            result = Nothing
        End Try
    End Sub
    'END: MMR | 2018.10.11 | YRS-AT-3101 | Commented existing code and added single routine to cancel loan request
    Private Function CancelPendingRequest(ByVal parameterLoanRequestId As Integer)
        Try
            YMCARET.YmcaBusinessObject.LoanInformationBOClass.CancelPendingLoanRequest(parameterLoanRequestId)
            HelperFunctions.ShowMessageToUser(MESSAGE_LOAN_CANCELLED)
            Me.LoadLoanRequests(Me.SessionPersonID)
        Catch SQLEx As SqlClient.SqlException
            HelperFunctions.ShowMessageToUser(SQLEx.Message.ToString, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_CancelPendingRequest", SQLEx)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_CancelPendingRequest", ex)
        End Try
    End Function
    'Added by Shubhrata AUg 28th 2006 Reason : To perform cancellation according to LoanStatus(TD Loans Phase2)
    Private Function CancelSelectLoanRequest(ByVal parameterLoanStatus As String, ByVal parameterLoanDetailsCount As Integer, ByVal parameterLoanRequestId As Integer)
        Try
            Dim intMessageNo As Integer
            Dim l_flag As Boolean
            l_flag = False
            intMessageNo = 0
            If parameterLoanDetailsCount > 1 Then
                intMessageNo = MESSAGE_LOAN_CANNOT_CANCEL_MULTIPLE_LOANS
                l_flag = True
            End If
            Select Case (parameterLoanStatus)
                'Case "REVE"
                '    l_string_message = "To Cancel this reversed loan go to Loan Options."
                '    l_flag = True
                Case "TERM"
                    intMessageNo = MESSAGE_LOAN_CANNOT_CANCEL_TERM_LOAN
                    l_flag = True
                Case "REAMRT"
                    intMessageNo = MESSAGE_LOAN_CANNOT_CANCEL_AMART_LOAN
                    l_flag = True
                Case "PAID"
                    If Not Session("LoanNumber") Is Nothing Then
                        If Session("LoanNumber") = 0 Then
                            intMessageNo = MESSAGE_LOAN_REVERSE_PAID_LOAN
                        ElseIf Session("LoanNumber") <> 0 Then
                            intMessageNo = MESSAGE_LOAN_CANNOT_CANCEL_AMART_LOAN
                        End If
                    End If

                    l_flag = True
                Case "CLOSED", "DEFALT", "PAYOFF"
                    intMessageNo = MESSAGE_LOAN_CANNOT_CANCEL_DEFAULTED_CLOSED_PAIDOFF
                    l_flag = True
                Case "SUSPND"
                    intMessageNo = MESSAGE_LOAN_CANNOT_CANCEL_SUSPENDED
                    l_flag = True
                Case "CANCEL"
                    intMessageNo = MESSAGE_LOAN_CANNOT_CANCEL_ALREADY_CANCELED
                    l_flag = True
                Case "EXP"
                    intMessageNo = MESSAGE_LOAN_CANNOT_CANCEL_EXPIRED
                    l_flag = True
                Case "UNSPND"
                    intMessageNo = MESSAGE_LOAN_CANNOT_CANCEL_UNSUSPENDED
                    l_flag = True
                    'START: MMR | 2018.04.11 | YRS-AT-3929 | Restrict loan cancellation request for disbursed loan if EFT payment status is 'PROOF' 
                Case "DISB"
                    If Not String.IsNullOrEmpty(Me.DisbursementEFTStatus) AndAlso Me.DisbursementEFTStatus.ToUpper = EFTPaymentStatus.PROOF Then
                        intMessageNo = MESSAGE_LOAN_CANNOT_CANCEL_EFT_PENDING
                        l_flag = True
                    End If
                    'END: MMR | 2018.04.11 | YRS-AT-3929 | Restrict loan cancellation request for disbursed loan if EFT payment status is 'PROOF' 
            End Select
            If intMessageNo <> 0 Then
                HelperFunctions.ShowMessageToUser(intMessageNo)
            End If
            Return l_flag
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_CancelSelectLoanRequest", ex)
        End Try
    End Function
    'Added by Shubhrata AUg 28th 2006 Reason : To perform cancellation according to LoanStatus(TD Loans Phase2)
    Private Function OpenReportViewer() As Boolean
        Dim l_bool_Return As Boolean

        Try
            'l_bool_Return = ExportToPDF()
            If l_bool_Return = False Then
                'TODO : code here
                'Return l_bool_Return
            End If

            'Call ReportViewer.aspx 
            ' set the Session Values that are used on Report Viewer.
            Session("PersID") = SessionPersonID()
            ' START : SR | 2018.10.31 | YRS-AT-3101 | Stop sending YMCA mail from here as we will send mail from common Mail routine.
            'Dim popupScript As String = "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
            '"'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');"

            'ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript1", popupScript, True)
            ' END : SR | 2018.10.31 | YRS-AT-3101 | Stop sending YMCA mail from here as we will send mail from common Mail routine.
            Dim popupScript1 As String = "window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp1', " & _
           "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');"

            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript2", popupScript1, True)

            'Copy the Files into the IDM Server
            Try
                ' Call the calling of the ASPX to copy the file.
                ' 34231 Modified to copy the files into IDM on Dec 08,2006.

                Dim popupScript As String = "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');"

                ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript3", popupScript, True)
            Catch ex As Exception

                HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
                HelperFunctions.LogException("LoanInformation_OpenReportViewer", ex)
            End Try

        Catch ex As Exception
            'Shubhrata 
            Session("OpenReport") = False
            'Shubhrata 
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_OpenReportViewer", ex)
        End Try

    End Function

    Private Sub ButtonLoanSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonLoanSummary.Click
        Try
            Session("strReportName") = "Loan Summary"
            Session("PersID") = Me.SessionPersonID
            OpenReportViewerLoanSummary()

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_ButtonLoanSummary_Click", ex)
        End Try
    End Sub
    Private Sub OpenReportViewerLoanSummary()
        Try
            'Call ReportViewer.aspx 
            Dim popupScript As String = "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
            "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')"



            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScript1", popupScript, True)

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_OpenReportViewerLoanSummary", ex)
        End Try

    End Sub
    Private Function GetToEMailAddrs(ByVal paramterYmcaNo As String) As String
        Try
            Dim l_dataset_EMailAddrs As New DataSet
            Dim l_datarow_EMailAddrs As DataRow
            Dim l_string_EMailAddrs As String = ""
            l_dataset_EMailAddrs = YMCARET.YmcaBusinessObject.DelinquencyLettersBOClass.GetList(paramterYmcaNo, 1)
            'we are passing 1 as we need only for TRANSM
            If Not l_dataset_EMailAddrs Is Nothing Then
                If l_dataset_EMailAddrs.Tables.Count > 0 Then
                    If l_dataset_EMailAddrs.Tables(0).Rows.Count > 0 Then
                        l_datarow_EMailAddrs = l_dataset_EMailAddrs.Tables(0).Rows(0)
                        If ((l_datarow_EMailAddrs("EmailAddr")).GetType.ToString <> "System.DBNull" Or (l_datarow_EMailAddrs("EmailAddr")).GetType.ToString <> "") Then
                            l_string_EMailAddrs = l_datarow_EMailAddrs("EmailAddr").ToString().Trim()
                        End If
                    End If
                End If
            End If
            Return l_string_EMailAddrs

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_GetToEMailAddrs", ex)
        End Try
    End Function
    'Private Function SendMail(ByVal ToEmailAdd As String) As Boolean
    '    Try
    '        Dim l_bool_SendEmail As Boolean = False
    '        Dim obj As MailUtil
    '        Dim l_strEmailCC As String = ""
    '        Dim l_Attachments As String = ""
    '        Dim l_str_msg As String

    '        obj = New MailUtil

    '        obj.MailCategory = "TDLoan"
    '        If obj.MailService = False Then Exit Function

    '        'Live Mode
    '        If ToEmailAdd.ToString().Trim() = String.Empty Then
    '            'obj.ToMail = obj.FromMail
    '            Throw New Exception(GetMessageByTextMessageNo(MESSAGE_LOAN_EMAIL_MISSING))
    '        Else
    '            obj.ToMail = ToEmailAdd.ToString().Trim()
    '        End If

    '        'Priya 16-March-2009 Change Time of Customer Service Department 8:45 AM to 6:00 previously it was 8:45 AM to 8:00 as per Hafiz mail on March 13, 2009 
    '        'Priya 17-March-2009 Made changes into MailMessage, message selected from database as per hafiz mail on 16-March-2009
    '        Dim strMailLastParagraphMsg As String
    '        strMailLastParagraphMsg = YMCARET.YmcaBusinessObject.MailBOClass.GetMailLastParagraph().Trim()
    '        obj.MailMessage = "Please open the attached document for important participant loan information." & strMailLastParagraphMsg
    '        'obj.MailMessage = "Please open the attached document for important participant loan information. If you have any questions or need additional information, please feel free to contact our Customer Service Department at 800-RET-YMCA, Monday through Friday, 8:45 AM - 6:00 PM Eastern Time."
    '        'End 17-March-2009

    '        'obj.Subject = "Important Participant Loan Information"
    '        obj.Subject = "Loan Processed for Participant : " & Me.TextBoxLast.Text & "," & Me.TextBoxFirst.Text & " " & Me.TextBoxMiddle.Text.Trim

    '        'Aparna 26/09/2006 To include the pdf files along with the reports
    '        If Session("StringDestFilePath") <> "" Then
    '            ' obj.MailAttachments = Session("StringDestFilePath")
    '            l_Attachments = Session("StringDestFilePath")

    '            'obj.MailAttachments = l_Attachments
    '            obj.MailAttachments.Add(l_Attachments)
    '        Else
    '            'obj.MailAttachments = Nothing

    '        End If

    '        obj.Send()
    '        l_bool_SendEmail = True
    '        Return l_bool_SendEmail


    '    Catch ex As Exception

    '        HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
    '        HelperFunctions.LogException("LoanInformation_SendMail", ex)
    '    End Try

    'End Function

    Public Function GenerateReports(ByVal parameterReportName As String, ByVal ParameterAppType As String, ByVal parameterDoctypecode As String) As String
        Try
            'by Aparna 20/09/2007

            Dim l_ArrListParamValues As New ArrayList
            Dim l_StringErrorMessage As String = String.Empty

            'Assign values to the properties
            IDM.PreviewReport = True
            IDM.LogonToDb = True
            IDM.CreatePDF = True
            IDM.CreateIDX = True
            IDM.CopyFilesToIDM = True
            IDM.AppType = ParameterAppType
            IDM.OutputFileType = "TDLoan_" & parameterDoctypecode.Trim() & " "
            IDM.DocTypeCode = parameterDoctypecode.Trim()

            If Not Session("FormYMCAId") Is Nothing Then
                IDM.YMCAID = Session("FormYMCAId").ToString()
            End If
            If Not Session("PersID") Is Nothing Then
                IDM.PersId = DirectCast(Session("PersID"), String)
                l_ArrListParamValues.Add(DirectCast(Session("PersID"), String).ToString.Trim)
            End If

            IDM.ReportName = parameterReportName.Trim()
            IDM.ReportParameters = l_ArrListParamValues
            l_StringErrorMessage = IDM.ExportToPDF()
            l_ArrListParamValues.Clear()
            Session("FTFileList") = IDM.SetdtFileList
            IDM = Nothing
            Return l_StringErrorMessage
            'BY Aparna 20/09/2007
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_GenerateReports", ex)
        End Try
    End Function

    'Added by Ashish 27-Jan-2009,Start
    Private Function ValidateQDRORequestPending() As Boolean
        Dim l_int_QDRORequestPending As Integer = 0
        Try
            l_int_QDRORequestPending = YMCARET.YmcaBusinessObject.LoanInformationBOClass.ValidateQDRORequestPending(Me.SessionFundID)
            If l_int_QDRORequestPending = 1 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_ValidateQDRORequestPending", ex)
        End Try
    End Function
    'AA:2014.02.03:BT:2292:YRS 5.0-2248 -:Added to load and fill all the tabs data and display the,
    Public Sub LoadAllTabs()
        Dim LabelModuleName As Label
        Try
            EnableTabstrip()
            Me.LoadGeneralInformation(Me.SessionPersonID, Me.SessionFundID)
            Me.LoadAccountContribution(Me.SessionFundID)
            Me.LoadMemberNotes(Me.SessionPersonID)
            Me.LoadCurrentPIA(Me.SessionFundID)
            LoadLoanRequests(Me.SessionPersonID)
            LabelModuleName = Master.FindControl("LabelModuleName")
            If LabelModuleName IsNot Nothing Then
                LabelModuleName.Text = "Activities > TD Loans > Request Processing >  Fund Id " + TextBoxFundNo.Text.Trim + " - " + TextBoxFirst.Text + " " + TextBoxLast.Text
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_LoadAllTabs", ex)
        End Try
    End Sub
    'Added by Ashish 27-Jan-2009,End

    Private Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        Try
            'START: MMR | 2018.10.11 | YRS-AT-3101 | Commented existing code and added single routine to cancel loan request
            'If Me.SessionLoanStatus.Trim.ToUpper = "PEND" Then
            '    Me.CancelPendingRequest(Me.SessionLoanRequestId)
            'ElseIf Me.SessionLoanStatus.Trim.ToUpper = "DISB" Then
            '    Me.CancelDisbLoanRequest(Me.SessionLoanRequestId)
            'End If
            Me.CancelLoanRequest()
            'END: MMR | 2018.10.11 | YRS-AT-3101 | Commented existing code and added single routine to cancel loan request
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("LoanInformation_btnYes_Click", ex)
        End Try
    End Sub

    'START: MMR | 2018.10.22 | YRS-AT-3929 | Show success/error message while sennding mail to YMCA/Participant 
    ''' <summary>
    ''' Display success/Error message based on message no/code.
    ''' </summary>
    ''' <param name="result"> result object</param>
    ''' <remarks></remarks>
    Private Sub ShowMessage(ByVal result As YMCAObjects.ReturnObject(Of Boolean))
        Dim errorNumber As Integer
        Dim metaMessage As YMCAObjects.MetaMessage
        Dim successList As List(Of String)
        Dim errorList As List(Of String)

        Try
            metaMessage = Nothing
            successList = New List(Of String)
            errorList = New List(Of String)

            If result.MessageList.Count > 0 Then
                For Each messageCode As String In result.MessageList
                    Try
                        If Integer.TryParse(messageCode, errorNumber) Then
                            metaMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(messageCode)
                        Else
                            metaMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByCode(messageCode)
                        End If
                    Catch ex As Exception
                        ex = Nothing
                        metaMessage = Nothing
                    End Try
                    If Not metaMessage Is Nothing Then
                        If metaMessage.MessageType = YMCAObjects.EnumMessageTypes.Success Then
                            successList.Add(metaMessage.DisplayText)
                        ElseIf metaMessage.MessageType = YMCAObjects.EnumMessageTypes.Error Then
                            errorList.Add(metaMessage.DisplayText)
                        End If
                    Else
                        errorList.Add(metaMessage.DisplayText)
                    End If
                Next
            End If

            If Not successList Is Nothing Then
                For Each message As String In successList
                    HelperFunctions.ShowMessageToUser(message, EnumMessageTypes.Success)
                Next
            End If
            If Not errorList Is Nothing Then
                For Each message As String In errorList
                    HelperFunctions.ShowMessageToUser(message, EnumMessageTypes.Error)
                Next
            End If
        Catch
            Throw
        Finally
            errorNumber = 0
            metaMessage = Nothing
            successList = Nothing
            errorList = Nothing
        End Try
    End Sub
    'END: MMR | 2018.10.22 | YRS-AT-3929 | Show success/error message while sennding mail to YMCA/Participant

    'START: Shilpa N | 02/28/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            ButtonAddNote.Enabled = False
            ButtonAddNote.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END: Shilpa N | 02/28/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
End Class
