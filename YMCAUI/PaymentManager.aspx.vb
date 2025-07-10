'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA Yrs
' FileName			:	PaymentManager.aspx.vb
' Author Name		:	Ragesh V.P
' Employee ID		:	34231
' Email				:	janhavi.shetye@3i-infotech.com
' Contact No		:	5592835
' Creation Time		:	5/18/05 2:04:08 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	<<Please put the brief description here...>>
' Cache to Session       :   Ragesh 34231 02/02/06 Cache to Session
'Modification History
'** Date(MM/DD/YYYY)            Author          Description
'** 05/08/2009                  Amit			Phase V Changes
'** 06/03/2009                  Amit            Modification done for the change requests
'** 06/09/2009                  Amit            Bug fix regarding the date field
'****************************************************
'Modification History
'*******************************************************************************
'Modified by                    Date            Description
'*******************************************************************************
'Neeraj Singh                   12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Shashi Shekhar                 17/Nov/2009     Added Code  To Resolve the Issue No YRS 1026 
'Shashi shekhar                 2009-11-26      Change UI* code to simplify the data fetching process
'Shashi Shekhar Singh           10-04-2010      For BT- 579
'Shagufta Chaudhari             2011.08.24      For BT-875, Mismatch on the Total Net Amount message when I click on Pay button.
'Priya Patil					28.09.2012		BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
'Manthan Rajguru                2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Sanjay GS Rawat                2018.04.10      YRS-AT-3101 - YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
'Vinayan C                      2018.09.11      YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
'Manthan Rajguru                2018.12.03      YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
'Shilpa N                       2019.03.18      YRS-AT-4248 -  YRS enh: YRS read-only access during MonthEnd Processing (TrackIT 35547)    
'Megha Lad                      2019.10.10      YRS-AT-4601 - YRS enh: State Withholding Project - Export file First Annuity Payroll
'												YRS-AT-4642 - State Withholding - Additional column to Payment Manager grid
'Pooja Kumkar                   2019.12.12      YRS-AT-4676 -  State Withholding - Vaildations for exporting file from Payment Manager (First Annuities) 
'Manthan Rajguru                2020.03.16      YRS-AT-4601 - YRS enh: State Withholding Project - Export file First Annuity Payroll
'*******************************************************************************
#Region "Imports"
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Threading
Imports System.Reflection
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.DataRow
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Globalization
Imports YMCAObjects ' SR | 2018.04.25 | YRS-AT-3101 | Added to retrive messages defined in metaMessage file.
#End Region

Public Class PaymentManager
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("PaymentManager.aspx")



    'End issue id YRS 5.0-940

    Private Enum LoadDatasetMode
        Table
        Session
    End Enum

#Region "Properties"
    'session_bool_LoadDisbursement
    'Dim g_bool_Mismatch As Boolean
    Private Property sessionMismatch() As Boolean
        Get
            If Not (Session("Mismatch")) Is Nothing Then
                Return (CType(Session("Mismatch"), Boolean))
            Else
                Return False
            End If
        End Get

        Set(ByVal Value As Boolean)
            Session("Mismatch") = Value
        End Set
    End Property
    Private Property sessionboolLoadDisbursement() As Boolean
        Get
            If Not (Session("boolLoadDisbursement")) Is Nothing Then
                Return (CType(Session("boolLoadDisbursement"), Boolean))
            Else
                Return False
            End If
        End Get

        Set(ByVal Value As Boolean)
            Session("boolLoadDisbursement") = Value
        End Set
    End Property
    Private Property SessionAllOtherWithdrawals() As Boolean
        Get
            If Not (Session("AllOtherWithdrawals")) Is Nothing Then
                Return (CType(Session("AllOtherWithdrawals"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("AllOtherWithdrawals") = Value
        End Set
    End Property
    '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
    Private Property SessionShiraWithdrawals() As Boolean
        Get
            If Not (Session("ShiraWithdrawals")) Is Nothing Then
                Return (CType(Session("ShiraWithdrawals"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("ShiraWithdrawals") = Value
        End Set
    End Property
    Private Property SessionShiraMRD() As Boolean
        Get
            If Not (Session("SessionShiraMRD")) Is Nothing Then
                Return (CType(Session("SessionShiraMRD"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("SessionShiraMRD") = Value
        End Set
    End Property
    'END 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

    Private Property SessionDeathWithdrawals() As Boolean
        Get
            If Not (Session("DeathWithdrawals")) Is Nothing Then
                Return (CType(Session("DeathWithdrawals"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("DeathWithdrawals") = Value
        End Set
    End Property
    Private Property SessionHardWithdrawals() As Boolean
        Get
            If Not (Session("HardWithdrawals")) Is Nothing Then
                Return (CType(Session("HardWithdrawals"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("HardWithdrawals") = Value
        End Set
    End Property


    Private Property SessionLongUSCheckNo() As Int32
        Get
            If Not (Session("Long_USCheckNo")) Is Nothing Then
                Return (CType(Session("Long_USCheckNo"), System.Int32))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Int32)
            Session("Long_USCheckNo") = Value
        End Set
    End Property

    Private Property SessionCharUSCheckSeriesPrefix() As String
        Get
            If Not (Session("Long_USCheckSeriesPrefix")) Is Nothing Then
                Return (CType(Session("Long_USCheckSeriesPrefix"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("Long_USCheckSeriesPrefix") = Value
        End Set
    End Property
    Private Property SessionCHARCANADACheckSeriesPrefix() As String
        Get
            If Not (Session("Char_CANADACheckSeriesPrefix")) Is Nothing Then
                Return (CType(Session("Char_CANADACheckSeriesPrefix"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("Long_CANADACheckSeriesPrefix") = Value
        End Set
    End Property

    Private Property SessionLongCANADACheckNo() As Int32
        Get
            If Not (Session("Long_CANADACheckNo")) Is Nothing Then
                Return (CType(Session("Long_CANADACheckNo"), System.Int32))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Int32)
            Session("Long_CANADACheckNo") = Value
        End Set
    End Property

    Private Property SessionLongRefundCheckNo() As Int32
        Get
            If Not (Session("Long_RefundCheckNo")) Is Nothing Then
                Return (CType(Session("Long_RefundCheckNo"), System.Int32))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Int32)
            Session("Long_RefundCheckNo") = Value
        End Set
    End Property

    '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
    Private Property SessionLongSHIRAMCheckNo() As Int32
        Get
            If Not (Session("Long_SHIRAMCheckNo")) Is Nothing Then
                Return (CType(Session("Long_SHIRAMCheckNo"), System.Int32))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Int32)
            Session("Long_SHIRAMCheckNo") = Value
        End Set
    End Property

    Private Property SessionLongTDLoanCheckNo() As Int32
        Get
            If Not (Session("Long_TDLoanCheckNo")) Is Nothing Then
                Return (CType(Session("Long_TDLoanCheckNo"), System.Int32))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Int32)
            Session("Long_TDLoanCheckNo") = Value
        End Set
    End Property
    'Aparna -16/10/2006
    Private Property SessionLongEXPCheckNo() As Int32
        Get
            If Not (Session("Long_EXPCheckNo")) Is Nothing Then
                Return (CType(Session("Long_EXPCheckNo"), System.Int32))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Int32)
            Session("Long_EXPCheckNo") = Value
        End Set
    End Property
    'Shagufta Chaudhari:2011.08.24: For BT-875, Mismatch on the Total Net Amount message when I click on Pay button.
    'Session conversion from Double to Decimal
    Private Property SessionDoubleTotalNetAmount() As Decimal
        Get
            If Not (HttpContext.Current.Session("Double_TotalNetAmount")) Is Nothing Then
                Return (CType(HttpContext.Current.Session("Double_TotalNetAmount"), System.Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            HttpContext.Current.Session("Double_TotalNetAmount") = Value
        End Set


    End Property 'End:Shagufta chaudhari:2011.08.24:BT-875

    Private Property SessionCheckDate() As Date
        Get
            If Not (Session("Date_CheckDate")) Is Nothing Then
                Return (CType(Session("Date_CheckDate"), System.DateTime).Date())
            Else
                Return Convert.ToDateTime("01/01/1900")
            End If
        End Get
        Set(ByVal Value As Date)
            Session("Date_CheckDate") = Value
        End Set
    End Property

    Private Property SessionAccountingDate() As Date
        Get
            If Not (Session("Date_AccountDate")) Is Nothing Then
                Return (CType(Session("Date_AccountDate"), System.DateTime).Date())
            Else
                Return Convert.ToDateTime("01/01/1900")
            End If
        End Get
        Set(ByVal Value As Date)
            Session("Date_AccountDate") = Value
        End Set
    End Property

    Private Property SessionPMStartDate() As DateTime
        Get
            If Not (Session("Date_PMstartDate")) Is Nothing Then
                Return (CType(Session("Date_PMstartDate"), System.DateTime))
            Else
                Return System.DateTime.Now()
            End If
        End Get
        Set(ByVal Value As DateTime)
            Session("Date_PMstartDate") = Value
        End Set
    End Property

    Private Property SessionDataLoadMode() As Boolean
        Get
            If Not (Session("Bool_DataLoadMode")) Is Nothing Then
                Return (CType(Session("Bool_DataLoadMode"), Boolean))
            Else
                Return False
            End If
        End Get

        Set(ByVal Value As Boolean)
            Session("Bool_DataLoadMode") = Value
        End Set
    End Property

    Private Property Session_datatable_DisbursementsALL() As DataTable
        Get
            If Not (Session("g_datatable_DisbursementsALL")) Is Nothing Then

                Return (DirectCast(Session("g_datatable_DisbursementsALL"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("g_datatable_DisbursementsALL") = Value
        End Set
    End Property

    Private Property Session_datatable_DisbursementsREF() As DataTable
        Get
            If Not (Session("g_datatable_DisbursementsREF")) Is Nothing Then

                Return (DirectCast(Session("g_datatable_DisbursementsREF"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("g_datatable_DisbursementsREF") = Value
        End Set
    End Property

    Private Property Session_datatable_DisbursementsANN() As DataTable
        Get
            If Not (Session("g_datatable_DisbursementsANN")) Is Nothing Then

                Return (DirectCast(Session("g_datatable_DisbursementsANN"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("g_datatable_DisbursementsANN") = Value
        End Set
    End Property
    'added by ruchi to add a session property for loan
    Private Property Session_datatable_DisbursementsTDLoan() As DataTable
        Get
            If Not (Session("g_datatable_DisbursementsTDLoan")) Is Nothing Then

                Return (DirectCast(Session("g_datatable_DisbursementsTDLoan"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("g_datatable_DisbursementsTDLoan") = Value
        End Set
    End Property

    'Phase V Changes-start April 08,2009' added by Amit to add a session property for Hardship withdrawal
    'Private Property Session_datatable_HWDisbursement() As DataTable
    '    Get
    '        If Not (Session("g_datatable_HWDisbursement")) Is Nothing Then

    '            Return (CType(Session("g_datatable_HWDisbursement"), DataTable))
    '        Else
    '            Return Nothing
    '        End If
    '    End Get

    '    Set(ByVal Value As DataTable)
    '        Session("g_datatable_HWDisbursement") = Value
    '    End Set
    'End Property
    ''added by amit to add a session property for death withdrawal
    'Private Property Session_datatable_DWDisbursement() As DataTable
    '    Get
    '        If Not (Session("g_datatable_DWDisbursement")) Is Nothing Then

    '            Return (CType(Session("g_datatable_DWDisbursement"), DataTable))
    '        Else
    '            Return Nothing
    '        End If
    '    End Get

    '    Set(ByVal Value As DataTable)
    '        Session("g_datatable_DWDisbursement") = Value
    '    End Set
    'End Property
    'Phase V Changes-end April 08,2009'
    'added by Aparna to add a session property for EXP Dividends - 16/10/2006
    Private Property Session_datatable_DisbursementsEXP() As DataTable
        Get
            If Not (Session("g_datatable_DisbursementsEXP")) Is Nothing Then

                Return (DirectCast(Session("g_datatable_DisbursementsEXP"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("g_datatable_DisbursementsEXP") = Value
        End Set
    End Property
    Private Property Session_datatable_Disbursements() As DataTable
        Get
            If Not (Session("g_datatable_Disbursements")) Is Nothing Then

                Return (DirectCast(Session("g_datatable_Disbursements"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("g_datatable_Disbursements") = Value
        End Set
    End Property

    Private Property Session_datatable_disbursementType() As DataTable
        Get
            If Not (Session("g_datatable_disbursementType")) Is Nothing Then

                Return (DirectCast(Session("g_datatable_disbursementType"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("g_datatable_disbursementType") = Value
        End Set
    End Property

    Private Property Session_datatable_DisbursementsREF_REPL() As DataTable
        Get
            If Not (Session("g_datatable_DisbursementsREF_REPL")) Is Nothing Then

                Return (DirectCast(Session("g_datatable_DisbursementsREF_REPL"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("g_datatable_DisbursementsREF_REPL") = Value
        End Set
    End Property

    Private Property Session_datatable_DisbursementsANN_REPL() As DataTable
        Get
            If Not (Session("g_datatable_DisbursementsANN_REPL")) Is Nothing Then

                Return (DirectCast(Session("g_datatable_DisbursementsANN_REPL"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("g_datatable_DisbursementsANN_REPL") = Value
        End Set
    End Property
    'APARNA
    Private Property Session_datatable_DisbursementsTDLoan_REPL() As DataTable
        Get
            If Not (Session("g_datatable_DisbursementsTDLoan_REPL")) Is Nothing Then

                Return (DirectCast(Session("g_datatable_DisbursementsTDLoan_REPL"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("g_datatable_DisbursementsTDLoan_REPL") = Value
        End Set
    End Property
    Private Property Session_datatable_DisbursementsEXP_REPL() As DataTable
        Get
            If Not (Session("g_datatable_DisbursementsEXP_REPL")) Is Nothing Then

                Return (DirectCast(Session("g_datatable_DisbursementsEXP_REPL"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("g_datatable_DisbursementsEXP_REPL") = Value
        End Set
    End Property
    'APARNA
    Private Property Session_datatable_DisbursementsALL_REPL() As DataTable
        Get
            If Not (Session("g_datatable_DisbursementsALL_REPL")) Is Nothing Then

                Return (DirectCast(Session("g_datatable_DisbursementsALL_REPL"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("g_datatable_DisbursementsALL_REPL") = Value
        End Set
    End Property

    Private Property SessionStartPay() As Boolean
        Get
            If Not (Session("Bool_StartPay")) Is Nothing Then
                Return (CType(Session("Bool_StartPay"), Boolean))
            Else
                Return False
            End If
        End Get

        Set(ByVal Value As Boolean)
            Session("Bool_StartPay") = Value
        End Set
    End Property
    'Ádded by shashi:2009-12-02:taking this property to keep all cashed data in seperate table
    Private Property Session_datatable_DisbursementsCACHE() As DataTable
        Get
            If Not (Session("g_datatable_DisbursementsCACHE")) Is Nothing Then

                Return (DirectCast(Session("g_datatable_DisbursementsCACHE"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("g_datatable_DisbursementsCACHE") = Value
        End Set
    End Property

    ' START : SR | 2018.04.10 | YRS-AT-3101 | declare property to store EFT disbursement type.
    Public Property EFTDisbursementType() As DataTable
        Get
            If Not (Session("EFTDisbursementType")) Is Nothing Then
                Return (DirectCast(Session("EFTDisbursementType"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            Session("EFTDisbursementType") = Value
        End Set
    End Property
    ' Define property to store Payment method type 
    Public Property PaymentMethodType() As String
        Get
            If Not (ViewState("PaymentMethodType")) Is Nothing Then
                Return (CType(ViewState("PaymentMethodType"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("PaymentMethodType") = Value
        End Set
    End Property

    ' Define property to get count of disbursement ready for approval process.
    Public Property ApprovalEFTCount() As String
        Get
            If Not (ViewState("ApprovalEFTCount")) Is Nothing Then
                Return (CType(ViewState("ApprovalEFTCount"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("ApprovalEFTCount") = Value
        End Set
    End Property

    ' Define property to get count of disbursement ready for rejection process.
    Public Property RejectionEFTCount() As String
        Get
            If Not (ViewState("RejectionEFTCount")) Is Nothing Then
                Return (CType(ViewState("RejectionEFTCount"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("RejectionEFTCount") = Value
        End Set
    End Property

    ' Defined property to get database driven EFT Header.
    Public Property EFTHeader() As String
        Get
            If Not (ViewState("EFTHeader")) Is Nothing Then
                Return (CType(ViewState("EFTHeader"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("EFTHeader") = Value
        End Set
    End Property

    ' END : SR | 2018.04.10 | YRS-AT-3101 | declare property to store EFT disbursement type.

    'START: PPP | 04/11/2018 | YRS-AT-3101 
    'Holds EFT disbursements which are pending for approval
    Private Property EFTDisbursementsPendingForApproval() As DataTable
        Get
            If Not (Session("EFTDisbursementsPendingForApproval")) Is Nothing Then

                Return (DirectCast(Session("EFTDisbursementsPendingForApproval"), DataTable))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataTable)
            Session("EFTDisbursementsPendingForApproval") = Value
        End Set
    End Property
    'END: PPP | 04/11/2018 | YRS-AT-3101 


    'START : VC | 2018.09.18 | YRS-AT-3101 -  Declared property to store confirmation message for select all operation in Confirm EFT Grid
    Public Property SelectAllConfirmation() As String
        Get
            If Not (ViewState("SelectAllConfirmation")) Is Nothing Then
                Return (CType(ViewState("SelectAllConfirmation"), String))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("SelectAllConfirmation") = Value
        End Set
    End Property
    'END : VC | 2018.09.18 | YRS-AT-3101 -  Declared property to store confirmation message for select all operation in Confirm EFT Grid

    'START : VC | 2018.09.18 | YRS-AT-3101 -  Declared property to store confirmation message for Approve operation in Confirm EFT Grid
    Public Property ApproveConfirmation() As String
        Get
            If Not (ViewState("ApproveConfirmation")) Is Nothing Then
                Return (CType(ViewState("ApproveConfirmation"), String))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("ApproveConfirmation") = Value
        End Set
    End Property
    'END : VC | 2018.09.18 | YRS-AT-3101 -  Declared property to store confirmation message for Approve operation in Confirm EFT Grid

    ''START : SR | 2018.10.04 | YRS-AT-3101 - Define Properties for EFTBatch list & FileImport flag.
    Private Property BatchList() As List(Of String)
        Get
            If Not (Session("BatchList")) Is Nothing Then

                Return (DirectCast(Session("BatchList"), List(Of String)))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As List(Of String))
            Session("BatchList") = Value
        End Set
    End Property

    ' Defined property to get File Import status.
    Public Property IsFileImported() As Boolean
        Get
            If Not (ViewState("IsFileImported")) Is Nothing Then
                Return (CType(ViewState("IsFileImported"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsFileImported") = Value
        End Set
    End Property

    Public Property ImportedBankFileName() As String
        Get
            If Not (ViewState("ImportedBankFileName")) Is Nothing Then
                Return (CType(ViewState("ImportedBankFileName"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("ImportedBankFileName") = Value
        End Set
    End Property

    Public Property SelectedEFTDisbursementType() As String
        Get
            If Not (ViewState("SelectedEFTDisbursementType")) Is Nothing Then
                Return (CType(ViewState("SelectedEFTDisbursementType"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("SelectedEFTDisbursementType") = Value
        End Set
    End Property
    ''END : SR | 2018.10.04 | YRS-AT-3101 - Define Properties for EFTBatch list & FileImport flag.

    'START : MMR | 2018.10.04 | YRS-AT-3101 - Define Properties for EFTBatch list & FileImport flag.
    Public Property IsPaymentOutSourcingKeyON() As Boolean
        Get
            If Not (ViewState("IsPaymentOutSourcingKeyON")) Is Nothing Then
                Return (CType(ViewState("IsPaymentOutSourcingKeyON"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsPaymentOutSourcingKeyON") = Value
        End Set
    End Property
    'END : MMR | 2018.10.04 | YRS-AT-3101 - Define Properties for EFTBatch list & FileImport flag.


#End Region

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ChkReplacedDisbursements As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TextBoxNetAmount As System.Web.UI.WebControls.TextBox ' SR | 2018.10.24 | YRS-AT-3101 - Renamed from TextBoxboxNetAmount to TextBoxNetAmount.
    Protected WithEvents TextBoxCheckNoUs As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxCheckNoCanadian As System.Web.UI.WebControls.TextBox
    Protected WithEvents CheckBoxProcessAllSelectedDisbursements As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBoxManualCheck As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TextBoxManualCheckNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridPaymentManager As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonDeselectErroneousDisbursements As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonPrint As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonPay As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridPayment As System.Web.UI.WebControls.DataGrid
    Protected WithEvents RadioButtonUSA As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonCanada As System.Web.UI.WebControls.RadioButton
    Protected WithEvents LabelCheckdate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNextUSCheck As System.Web.UI.WebControls.Label
    Protected WithEvents LabelCanadianCheck As System.Web.UI.WebControls.Label
    Protected WithEvents Menu1 As skmMenu.Menu
    'Protected WithEvents ButtonSelectNone As System.Web.UI.WebControls.Button 'VC | 2018.09.11 | YRS-AT-3101 |  Commented to remove ButtonSelectNone
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents CheckBoxReplacedDisbursements As System.Web.UI.WebControls.CheckBox
    'START: SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
    Protected WithEvents lblCheckType As System.Web.UI.WebControls.Label
    Protected WithEvents ddlCheckType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblEFTType As System.Web.UI.WebControls.Label
    Protected WithEvents ddlEFTType As System.Web.UI.WebControls.DropDownList

    'START : ML | 2019.11.04 | YRS-AT-4601| Change Radiobutton list to individual radiobutton to set new UI
    ' Protected WithEvents rblDisbursementType As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rbDisbursementTypeEFT As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbDisbursementTypeCHECK As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbDisbursementTypeAPPROVEDEFT As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbCurrencyCanada As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbCurrencyUSA As System.Web.UI.WebControls.RadioButton
    Protected WithEvents spCurrency As HtmlGenericControl
    Protected WithEvents tblCheckSeries As HtmlTable
    'END : ML | 2019.11.04 | YRS-AT-4601| Change Radiobutton list to individual radiobutton to set new UI

    Protected WithEvents divFundNoAndRecalculate As HtmlGenericControl
    Protected WithEvents divCheckSeries As HtmlGenericControl
    Protected WithEvents divApprovedEFT As HtmlGenericControl
    Protected WithEvents tblWithdrawalOptions As HtmlTable
    'END: SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
    Protected WithEvents dgEFTDisbursements As System.Web.UI.WebControls.DataGrid 'PPP | 04/11/2018 | YRS-AT-3101 | Declared control which will host EFT payments which are in 'PROOF' mode
    Protected WithEvents divSuccessMessage As System.Web.UI.HtmlControls.HtmlGenericControl  'SB | 2018.04.24 | YRS-AT-3101 | Declaring div to display EFT status
    Protected WithEvents divErrorMessage As System.Web.UI.HtmlControls.HtmlGenericControl  'SB | 2018.04.24 | YRS-AT-3101 | Declaring div to display EFT status
    Protected WithEvents divStatusMessage As System.Web.UI.HtmlControls.HtmlGenericControl  'SR | 2018.04.24 | YRS-AT-3101 | Declaring div to display EFT status
    Protected WithEvents divDBErrorMessage As System.Web.UI.HtmlControls.HtmlGenericControl  'SR | 2018.04.24 | YRS-AT-3101 | Declaring div to display EFT db status
    Protected WithEvents btnImportEFTBankResponseFile As System.Web.UI.WebControls.Button 'SB | 2018.04.09 | YRS-AT-3101 | Declaring import button for file upload for EFT approval
    Protected WithEvents FileFieldEFTAckFile As System.Web.UI.HtmlControls.HtmlInputFile 'SB | 2018.04.09 | YRS-AT-3101 | Declaring import button for file upload for EFT approval
    Protected WithEvents gvEFTFailedPayments As System.Web.UI.WebControls.GridView 'SB | 2018.04.25 | YRS-AT-3101 | Declaring datagrid to display Failed EFT records
    Protected WithEvents gvEFTFailedToSendRejectionEmail As System.Web.UI.WebControls.GridView
    Protected WithEvents spanApprovedRecords As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents spanRecjectedRecords As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents btnYes As System.Web.UI.WebControls.Button

    Protected WithEvents RadioButtonListUS_Canadian As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LabelWithdrawals As System.Web.UI.WebControls.Label

    'Date Fix
    Protected WithEvents TextBoxCheckDate As YMCAUI.DateUserControl
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Dim g_datatable_Disbursements As DataTable
    Dim g_datatable_DisbursementsALL As DataTable
    Dim g_datatable_disbursementType As DataTable
    Dim g_datatable_DisbursementsREF As DataTable
    Dim g_datatable_DisbursementsANN As DataTable
    Dim g_datatable_DisbursementsTDLoan As DataTable
    Dim g_datatable_DisbursementsEXP As DataTable
    Dim g_datatable_DisbursementsREF_REPL As DataTable
    Dim g_datatable_DisbursementsANN_REPL As DataTable
    Dim g_datatable_DisbursementsALL_REPL As DataTable
    Dim g_datatable_DisbursementsTDLoan_REPL As DataTable
    Dim g_datatable_DisbursementsEXP_REPL As DataTable
    Dim g_datatable_DisbursementsCACHE As DataTable 'Ádded by shashi:-2009-12-02
    Dim g_String_filterExpr As String
    Dim g_bool_FirstLoad As Boolean
    Dim g_bool_First_Time As Boolean
    Dim g_bool_setAmount As Boolean




    'Amit Phase V Changes-Start April 08,2009
    'Dim g_bool_setGrid As Boolean
    Dim g_datatable_DWDisbursement As DataTable
    Dim g_datatable_HWDisbursement As DataTable
    ' Dim DisbursementType As String
    Protected WithEvents TextBoxFundIdNo As System.Web.UI.WebControls.TextBox

    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    'Added for gemini issue 786
    Protected WithEvents CheckBoxAllOtherWithdrawals As System.Web.UI.WebControls.CheckBox
    '28.09.2012	BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
    Protected WithEvents CheckBoxSHIRA As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBoxHardshipWithdrawals As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBoxDeathWithdrawals As System.Web.UI.WebControls.CheckBox
    Dim l_string_checkRefundWithDrawalAll As String
    Dim l_string_checkRefundWithDrawalHard As String
    Dim l_string_checkRefundWithDrawalDeath As String
    Dim l_string_checkRefundWithDrawalShira As String
    Protected WithEvents ButtonLoadRefund As System.Web.UI.WebControls.Button
    'Phase V Changes-End April 08,2009
    'START : VC | 2018.09.19 | YRS-AT-3101 -  Declared new controls
    Protected WithEvents txtConfirmEFTRecalculate As System.Web.UI.WebControls.TextBox
    Protected WithEvents divConfirmEFTRecalculate As HtmlGenericControl
    Protected WithEvents btnRecalculateEFTCheck As System.Web.UI.HtmlControls.HtmlButton
    Protected WithEvents btnConfirmEFTRecalculate As System.Web.UI.HtmlControls.HtmlButton
    'END : VC | 2018.09.19 | YRS-AT-3101 -  Declared new controls
    Protected WithEvents gvWarnErrorMessage As System.Web.UI.WebControls.GridView ' PK|12-12-2019 | YRS-AT-4676 | Control declaration
    Protected WithEvents lblNote As System.Web.UI.WebControls.Label 'ML | 2019.12.19 | YRS-AT 4676 | Label to display Note.
    Dim objPaymenManagerBO As New YMCARET.YmcaBusinessObject.PaymentManagerBOClass
    Dim objPaymentManagerWrapperBo As New YMCARET.YmcaBusinessObject.PaymentManagerBOWraperClass

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    'START: PPP | 04/16/2018 | YRS-AT-3101 | Declaring indexses to access EFT Disbursement Grid columns
    Dim checkBoxEFTGridIndex = 0
    Dim fundIdNoEFTGridIndex = 1
    Dim SSNOEFTGridIndex = 2
    Dim nameEFTGridIndex = 3
    Dim descriptionEFTGridIndex = 4
    Dim disbursementDateEFTGridIndex = 5
    Dim netAmountEFTGridIndex = 6
    Dim bankNameEFTGridIndex = 7
    Dim bankAbaNumberEFTGridIndex = 8
    Dim disbursementEFTIDEFTGridIndex = 9
    Dim disbursementIDEFTGridIndex = 10
    Dim persBankingEFTIDEFTGridIndex = 11
    Dim bankIdEFTGridIndex = 12
    Dim participantEmailIdEFTGridIndex = 13
    Dim disbursementTypeEFTGridIndex = 14
    'END: PPP | 04/16/2018 | YRS-AT-3101 | Declaring indexses to access EFT Disbursement Grid columns

#Region "Page Load"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'DataGridPayment.DataSource = CommonModule.CreateDataSource
        'DataGridPayment.DataBind()
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YRS", "BindEvents();", True) 'PPP | 04/27/2018 | YRS-AT-3101 | Registering JQuery dialog
        'Me.LabelCheckdate.AssociatedControlID = Me.TextBoxCheckDate.ID
        Me.LabelNextUSCheck.AssociatedControlID = Me.TextBoxCheckNoUs.ID
        Me.LabelCanadianCheck.AssociatedControlID = Me.TextBoxCheckNoCanadian.ID
        ' START | SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType. Also added code for EFT type dropdown list.
        'Me.LabelDisbursements.AssociatedControlID = Me.DropDownListlDisbursementType.ID
        Me.lblCheckType.AssociatedControlID = Me.ddlCheckType.ID
        Me.lblEFTType.AssociatedControlID = Me.ddlEFTType.ID
        ' END | SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType. Also added code for EFT type dropdown list.
        g_bool_setAmount = True

        'Amit Phase V Changes-Start 
        ' g_bool_setGrid = False
        'Phase V Changes-End 
        Try
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            If Not Me.IsPostBack Then
                'START :ML| 2019.10.10 | YRS-AT-4601 | Call method to Set flag based on database key for Payment Outsourcing Go live date
                SetPaymentOutsourcingKey() ' Method to set payment outsoucing key based on database date
                spCurrency.Visible = False
                rbCurrencyUSA.Checked = True
                rbCurrencyCanada.Checked = False

                divErrorMessage.Visible = False
                divSuccessMessage.Visible = False
                divStatusMessage.Visible = False
                divDBErrorMessage.Visible = False
                'END :ML| 2019.10.10 | YRS-AT-4601 | Call method to Set flag based on database key for Payment Outsourcing Go live date
                g_String_filterExpr = " "
                Me.SessionPMStartDate = System.DateTime.Now.Date()
                g_bool_First_Time = True

                ClearSessions()

                populateDropDownDisbursementType()
                ' START : SR | 2018.04.10 | YRS-AT-3101 | initialize page with default Check option.

                'START :ML |2019.11.04 | YRS-AT-4601 | Radiobutton list to individial radiobutton UI changes 
                rbDisbursementTypeEFT.Checked = True
                'rblDisbursementType.SelectedValue = PaymentMethod.EFT
                'END :ML |2019.11.04 | YRS-AT-4601 | Radiobutton list to individial radiobutton UI changes 

                ddlEFTType.Enabled = True
                ddlCheckType.Enabled = False
                PaymentMethodType = PaymentMethod.EFT
                CheckBoxReplacedDisbursements.Visible = False
                PopulateEFTDisbursementType()
                divFundNoAndRecalculate.Visible = False
                divConfirmEFTRecalculate.Visible = False 'VC | 2018.09.19 | YRS-AT-3101 -  To hide recalculation controls
                divCheckSeries.Visible = False
                SetCheckSeriesControlsVisibility(False)
                SetWithdrawalControls(False)
                divApprovedEFT.Visible = False
                ' END : SR | 2018.04.10 | YRS-AT-3101 | initialize page with default Check option.
                ' PopulateData(LoadDatasetMode.Table)

                SetValuesInControles()

                If objPaymenManagerBO.bool_NegetivePayments = True Then
                    ShowMessage()
                End If

                Me.TextBoxFundIdNo.Attributes.Add("onkeypress", "javascript:ValidateNumeric();")

                ViewState("IsDefaultExist") = "True"
                ButtonPrint.Enabled = False  ' SR | 2018.10.24 | YRS-AT-3101 - Bydefault Print button will be disbaled.
                ButtonPay.Enabled = False ' SR | 2018.10.24 | YRS-AT-3101 - Bydefault pay button will be disbaled.

            Else

                If Request.Form("Yes") = "Yes" Then

                    'PopulateData(LoadDatasetMode.Session)'commented by shashi:2009-12-01
                    'PopulateData() 'Added by shashi:2009-12-01
                    PersistSelectionsFromGrid()

                    If Me.SessionStartPay = True Then
                        ' START | SR | 2018.04.13 | YRS-AT-3101 | Select Check or EFT File generation based on Payment Method.
                        If PaymentMethodType = PaymentMethod.CHECK Then
                            Me.PAY()
                        ElseIf PaymentMethodType = PaymentMethod.EFT Then
                            Me.PAY_EFT()
                            ' END | SR | 2018.04.13 | YRS-AT-3101 | Select Check or EFT File generation based on Payment Method.
                        End If
                    End If

                    Me.SessionStartPay = False

                ElseIf Request.Form("OK") = "OK" Then
                    ' START | SR | 2018.04.13 | YRS-AT-3101 | perform following operation only if Payment method type is EFT/CHECK.
                    If PaymentMethodType = PaymentMethod.CHECK Or PaymentMethodType = PaymentMethod.EFT Then
                        'Added  the if condition for the load disbursement
                        'Added by shashi:2009-12-02
                        If (Session("MandCheck") = "False") Then
                            Session("MandCheck") = "True"
                        Else
                            If Me.sessionboolLoadDisbursement = False Then
                                'PopulateData(LoadDatasetMode.Session)'Commented by shashi:2009-12-01
                                'PopulateData() 'Added by shashi:2009-12-01
                                PersistSelectionsFromGrid()
                                SetNetAmount()
                            End If
                        End If
                    End If
                    ' END | SR | 2018.04.13 | YRS-AT-3101 | perform following operation only if Payment method type is EFT/CHECK.
                Else
                    'PopulateData(LoadDatasetMode.Session)
                End If
                'If CheckBoxAllOtherWithdrawals.Checked = True Or CheckBoxDeathWithdrawals.Checked = True Or CheckBoxHardshipWithdrawals.Checked = True Then
                '    ButtonLoadRefund.Visible = True
                'Else
                '    ButtonLoadRefund.Visible = False
                'End If
                'START | ML | 2019.12.12 | YRS-AT-4601 | Commented Old code to handle Success and Error messages.
                ' START | SR | 2018.04.13 | YRS-AT-3101 | Hide div control for sucess and error message.
                'divErrorMessage.Style("display") = "none"
                'divSuccessMessage.Style("display") = "none"
                'divStatusMessage.Style("display") = "none"
                'divDBErrorMessage.Style("display") = "none"

                ' END | SR | 2018.04.13 | YRS-AT-3101 |  Hide div control for sucess and error message.
                'END  | ML | 2019.12.12 | YRS-AT-4601 | Commented Old code to handle Success and Error messages.
            End If
            'START : VC | 2018.09.19 | YRS-AT-3101 -  Store select all confirmation message in viewstate
            SelectAllConfirmation = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_CONFIRM_SELECT_ALL)
            Page.ClientScript.RegisterHiddenField("hfSelectAllConfirmation", SelectAllConfirmation.ToString())
            'END : VC | 2018.09.19 | YRS-AT-3101 -  Store select all confirmation message in viewstate
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
#End Region

#Region "Private Functions"
    'Amit Phase V Changes-Start 
    Private Function populateDropDownDisbursementType()
        Try
            objPaymenManagerBO.getDisbursementType()
            g_datatable_disbursementType = CType(objPaymenManagerBO.datatable_disbursementType, DataTable)
            Me.SessionAccountingDate = objPaymenManagerBO.DateTime_AcctDate
            Me.Session_datatable_disbursementType = g_datatable_disbursementType
            'START: SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
            Me.ddlCheckType.Items.Clear()
            Me.ddlCheckType.Items.Add("")
            'END: SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
            For Each l_Row As DataRow In g_datatable_disbursementType.Rows
                If l_Row("chrDisbursementType").GetType.ToString() <> "System.DBNull" Then
                    'START: SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
                    'Me.DropDownListlDisbursementType.Items.Add(CType(l_Row("chvDescription"), String).Trim())
                    Me.ddlCheckType.Items.Add(CType(l_Row("chvDescription"), String).Trim())
                    'END: SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
                End If
            Next
        Catch ex As Exception
            Throw
        End Try
    End Function

    'Amit Phase V Changes-End




    'Commented by shashi:2009-11-28 : Not used now
    'Private Function PopulateGrid()
    '    ' Dim l_Cache As CacheManager
    '    Dim l_string_disbursementTypeCode As String
    '    Dim l_string_disbursementType As String
    '    Dim l_Datatow_DisbursementTypeRow As DataRow()
    '    Dim l_Datarow_DisbursementRows As DataRow()
    '    Dim l_String_filterExpr As String
    '    Dim l_double_sumTotal As Double
    '    Dim l_datarow As DataRow
    '    Try
    '        'l_Cache = CacheFactory.GetCacheManager()
    '        l_string_disbursementType = DropDownListlDisbursementType.SelectedItem.Text.Trim()
    '        g_String_filterExpr = " "
    '        l_string_disbursementTypeCode = " "
    '        g_datatable_disbursementType = Me.Session_datatable_disbursementType
    '        If l_string_disbursementType.Trim() = String.Empty Then
    '            l_string_disbursementType = " "
    '        Else
    '            l_String_filterExpr = " chvDescription = '" + l_string_disbursementType + "'"
    '            l_Datatow_DisbursementTypeRow = g_datatable_disbursementType.Select(l_String_filterExpr)
    '            If Not l_Datatow_DisbursementTypeRow(0) Is Nothing Then
    '                If l_Datatow_DisbursementTypeRow(0)("chrDisbursementType").GetType.ToString() <> "System.DBNull" Then
    '                    l_string_disbursementTypeCode = CType(l_Datatow_DisbursementTypeRow(0)("chrDisbursementType"), String)
    '                End If
    '                g_String_filterExpr = "DisbursementType = '" + l_string_disbursementTypeCode + "'"
    '            End If
    '        End If
    '        IdentiFyDataTable()
    '        'l_double_sumTotal = SumFields(g_datatable_Disbursements, "total")
    '        'Me.TextBoxNetAmount.Text = System.Convert.ToString(l_double_sumTotal).Trim()
    '        'Amit Phase V Changes-Start 
    '        'If g_bool_setGrid = False Then
    '        BindDataintheGrid()
    '        'End If
    '        'Phase V Changes-end 
    '    Catch ex As Exception
    '        'Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '        Throw
    '    End Try
    'End Function
    'Coomented by shashi:2009-11-28 : Not used now
    'Private Function BindDataintheGrid()
    '    Dim i As Long
    '    Dim l_SelDataGridItem As DataGridItem
    '    Dim l_Find_Datatow As DataRow()
    '    Dim l_String_Search As String
    '    Dim l_CheckBox As CheckBox

    '    Try
    '        i = 0
    '        If Me.SessionDataLoadMode = True Then
    '            For Each l_DataGridItem As DataGridItem In Me.DataGridPayment.Items
    '                l_CheckBox = l_DataGridItem.FindControl("CheckBoxSelect")
    '                'Amit Phase V Changes-Start 
    '                l_String_Search = "DisbursementID = '" + l_DataGridItem.Cells(7).Text + "'"
    '                'Phase V Changes-End 
    '                l_Find_Datatow = g_datatable_Disbursements.Select(l_String_Search)
    '                If (Not l_CheckBox Is Nothing) And (l_Find_Datatow.Length > 0) Then
    '                    If l_CheckBox.Checked = True Then
    '                        l_Find_Datatow(0)("Selected") = 1
    '                    Else
    '                        l_Find_Datatow(0)("Selected") = 0
    '                    End If
    '                End If
    '                i = i + 1
    '            Next
    '            'g_datatable_Disbursements.AcceptChanges()
    '        End If
    '        Me.SessionDataLoadMode = True
    '        Me.DataGridPayment.DataSource = Nothing
    '        Me.DataGridPayment.DataSource = g_datatable_Disbursements.DefaultView
    '        Me.DataGridPayment.DataBind()
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function





    'Shashi Shekhar:2009-11-26: Function to bind the data (Merge the functionalities of previously used Function PopulateGrid() and Function BindDataintheGrid())
    Private Function PersistSelectionsFromGrid()
        Dim i As Long
        Dim l_SelDataGridItem As DataGridItem
        Dim l_Find_Datatow As DataRow()
        Dim l_String_Search As String
        Dim l_CheckBox As CheckBox
        'Dim expr As String = String.Empty


        Try

            g_datatable_Disbursements = Me.Session_datatable_DisbursementsALL

            i = 0
            ''If Me.SessionDataLoadMode = True Then
            For Each l_DataGridItem As DataGridItem In Me.DataGridPayment.Items
                l_CheckBox = l_DataGridItem.FindControl("CheckBoxSelect")
                'Amit Phase V Changes-Start 
                l_String_Search = "DisbursementID = '" + l_DataGridItem.Cells(7).Text + "'"
                'Phase V Changes-End 
                l_Find_Datatow = g_datatable_Disbursements.Select(l_String_Search)
                If (Not l_CheckBox Is Nothing) And (l_Find_Datatow.Length > 0) Then
                    If l_CheckBox.Checked = True Then
                        l_Find_Datatow(0)("Selected") = 1
                    Else
                        l_Find_Datatow(0)("Selected") = 0
                    End If
                End If
                i = i + 1
            Next
            'g_datatable_Disbursements.AcceptChanges()
            '' End If



            ''--------------------------------------------------------------------
            'Dim l_StringDisbursementType As String
            'l_StringDisbursementType = DropDownListlDisbursementType.SelectedItem.Text.Trim()
            'l_StringDisbursementType = l_StringDisbursementType.PadRight(19, " ")
            'If l_StringDisbursementType.Substring(0, 6).Trim().ToUpper = "REFUND" Then
            '    l_StringDisbursementType = "REF"
            'End If

            ''------------------------------------------------------------------------------

            'If Me.CheckBoxReplacedDisbursements.Checked = False Then expr = "Replaceable=False"


            'If l_StringDisbursementType = "REF" Then

            '    Dim l_refundtype As String = String.Empty
            '    If (l_string_checkRefundWithDrawalAll <> String.Empty) Then
            '        l_refundtype = ",3"
            '    End If
            '    If (l_string_checkRefundWithDrawalDeath <> String.Empty) Then
            '        l_refundtype = l_refundtype + ",2"
            '    End If
            '    If (l_string_checkRefundWithDrawalHard <> String.Empty) Then
            '        l_refundtype = l_refundtype + ",1,"
            '    End If
            '    If (l_refundtype <> String.Empty) Then
            '        l_refundtype = l_refundtype.Remove(0, 1)
            '        If l_refundtype.Substring(l_refundtype.Length - 1) = "," Then
            '            l_refundtype = l_refundtype.Remove(l_refundtype.Length - 1, 1)
            '        End If

            '        If expr = String.Empty Then
            '            expr = expr + "RefundType in (" + l_refundtype + ")"
            '        Else
            '            expr = expr + " AND RefundType in (" + l_refundtype + ")"
            '        End If

            '    End If

            'End If

            'Dim dv As DataView = g_datatable_Disbursements.DefaultView
            'dv.RowFilter = expr
            'g_datatable_Disbursements = dv.Table

            ' Dim expr As String = PrepareExpression()
            ' g_datatable_Disbursements = GetFilteredDataTable(Me.g_datatable_Disbursements, expr)
            'Me.Session_datatable_Disbursements = g_datatable_Disbursements

            ''Me.SessionDataLoadMode = True
            BindData()
        Catch ex As Exception
            Throw
        Finally

        End Try
    End Function
    'Binds data in the grid from the session variable
    Private Function BindData()
        g_datatable_Disbursements = Me.Session_datatable_DisbursementsALL

        Me.DataGridPayment.DataSource = Nothing
        If Not g_datatable_Disbursements Is Nothing Then
            Me.DataGridPayment.DataSource = g_datatable_Disbursements.DefaultView
        End If
        Me.DataGridPayment.DataBind()
        ' START | SR | 2018.04.17 | YRS-AT-3101 | If Check or EFT data displayed in grid the Hide Approved EFT grid.
        If HelperFunctions.isNonEmpty(g_datatable_Disbursements) Then
            ButtonPrint.Enabled = True
        Else
            ButtonPrint.Enabled = False
        End If

        dgEFTDisbursements.DataSource = Nothing
        dgEFTDisbursements.DataBind()
        dgEFTDisbursements.Style("Display") = "none"
        DataGridPayment.Style("Display") = "block"
        ' END | SR | 2018.04.17 | YRS-AT-3101 | If Check or EFT data displayed in grid the Hide Approved EFT grid.
    End Function
    Private Function SetNetAmount()
        Dim l_double_sumTotal As Double
        Dim nfi As New CultureInfo("en-US", False)
        Try
            nfi.NumberFormat.CurrencyGroupSeparator = ","
            nfi.NumberFormat.CurrencySymbol = "$"
            nfi.NumberFormat.CurrencyDecimalDigits = 2
            ' START : SR | 2018.04.20 | YRS-AT-3101 | select datagrid columns based on disbursement type.
            If (PaymentMethodType = PaymentMethod.CHECK Or PaymentMethodType = PaymentMethod.EFT) Then
                l_double_sumTotal = SumFields(g_datatable_Disbursements, "total")
                Me.TextBoxNetAmount.Text = System.Convert.ToString(l_double_sumTotal.ToString("C", nfi)).Trim() 'VC | 2018.09.19 | YRS-AT-3101 -  Set calculated total amount to textbox in case of CHECK or EFT
            ElseIf PaymentMethodType = PaymentMethod.APPROVED_EFT Then
                l_double_sumTotal = SumFields(EFTDisbursementsPendingForApproval, "NetAmount")
                Me.txtConfirmEFTRecalculate.Text = System.Convert.ToString(l_double_sumTotal.ToString("C", nfi)).Trim() 'VC | 2018.09.19 | YRS-AT-3101 -  Set calculated total amount to textbox in case of CONFIRM EFT
            End If
            ' END : SR | 2018.04.20 | YRS-AT-3101 | select datagrid columns based on disbursement type.
            'Me.TextBoxNetAmount.Text = System.Convert.ToString(l_double_sumTotal.ToString("C", nfi)).Trim()'VC | 2018.09.19 | YRS-AT-3101 -  Commented existing code

            Me.SessionDoubleTotalNetAmount = l_double_sumTotal
        Catch ex As Exception
            ' Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
            Throw
        End Try
    End Function

    'Private Function PopulateDataOld(ByVal LoadDataMode As LoadDatasetMode) As Boolean
    '    'Dim l_Cache As CacheManager
    '    Dim l_double_sumTotal As Double
    '    Dim l_String_filterExpr As String
    '    Dim replaceble As Integer

    '    If CheckBoxReplacedDisbursements.Checked = True Then
    '        replaceble = 1
    '    Else
    '        replaceble = 0
    '    End If

    '    Try
    '        'l_Cache = CacheFactory.GetCacheManager()
    '        Select Case LoadDataMode
    '            Case LoadDatasetMode.Table
    '                objPaymenManagerBO.getdisbursementswithoutfunding(DisbursementType, replaceble, checkRefundWithDrawal)
    '                'Populate the Data Grid
    '                g_datatable_DisbursementsALL = CType(objPaymenManagerBO.datatable_Disbursements, DataTable)
    '                g_datatable_DisbursementsREF = CType(objPaymenManagerBO.datatable_REFdDisbursements, DataTable)
    '                g_datatable_DisbursementsANN = CType(objPaymenManagerBO.datatable_ANNdDisbursements, DataTable)
    '                g_datatable_DisbursementsTDLoan = CType(objPaymenManagerBO.datatable_TDLoanDisbursements, DataTable)
    '                'Aparna
    '                g_datatable_DisbursementsEXP = CType(objPaymenManagerBO.datatable_EXPDisbursements, DataTable)
    '                'Aparna
    '                g_datatable_DisbursementsREF_REPL = CType(objPaymenManagerBO.datatable_REFdDisbursementsREPL, DataTable)
    '                g_datatable_DisbursementsANN_REPL = CType(objPaymenManagerBO.datatable_ANNDisbursementsREPL, DataTable)
    '                g_datatable_DisbursementsALL_REPL = CType(objPaymenManagerBO.datatable_DisbursementsREPL, DataTable)
    '                'Aparna
    '                g_datatable_DisbursementsTDLoan_REPL = CType(objPaymenManagerBO.datatable_TDLoanDisbursementsREPL, DataTable)
    '                g_datatable_DisbursementsEXP_REPL = CType(objPaymenManagerBO.datatable_EXPDisbursementsREPL, DataTable)
    '                'Aparna
    '                'Amit Phase V Changes-Start April 08,2009
    '                g_datatable_DWDisbursement = CType(objPaymenManagerBO.datatable_DWDisbursement, DataTable)
    '                g_datatable_HWDisbursement = CType(objPaymenManagerBO.datatable_HWDisbursement, DataTable)
    '                'Phase V Changes-End April 08,2009
    '                IdentiFyDataTable(" ")
    '                '/b Ragesh 34231 02/02/06 Cache to Session
    '                ' Add dataset into Casche
    '                Me.Session_datatable_DisbursementsALL = g_datatable_DisbursementsALL
    '                Me.Session_datatable_DisbursementsREF = g_datatable_DisbursementsREF
    '                Me.Session_datatable_DisbursementsANN = g_datatable_DisbursementsANN
    '                Me.Session_datatable_DisbursementsTDLoan = g_datatable_DisbursementsTDLoan
    '                'Aparna
    '                Me.Session_datatable_DisbursementsEXP = g_datatable_DisbursementsEXP
    '                'Aparna
    '                'Amit Phase V Changes-Start April 08,2009
    '                Me.Session_datatable_DWDisbursement = g_datatable_DWDisbursement
    '                Me.Session_datatable_HWDisbursement = g_datatable_HWDisbursement
    '                'Phase V Changes-end April 08,2009
    '                Me.Session_datatable_DisbursementsALL = g_datatable_Disbursements
    '                'Me.Session_datatable_disbursementType = g_datatable_disbursementType
    '                Me.Session_datatable_DisbursementsREF_REPL = g_datatable_DisbursementsREF_REPL
    '                Me.Session_datatable_DisbursementsANN_REPL = g_datatable_DisbursementsANN_REPL
    '                Me.Session_datatable_DisbursementsALL_REPL = g_datatable_DisbursementsALL_REPL
    '                'APARNA
    '                Me.Session_datatable_DisbursementsTDLoan_REPL = g_datatable_DisbursementsTDLoan_REPL
    '                Me.Session_datatable_DisbursementsEXP_REPL = g_datatable_DisbursementsEXP_REPL
    '                'APARNA
    '                '/e Ragesh 34231 02/02/06 Cache to Session
    '                ' Populate Combo Box.
    '                Me.SessionCheckDate = objPaymenManagerBO.DateTime_CheckDate
    '                'Me.SessionAccountingDate = objPaymenManagerBO.DateTime_AcctDate
    '                Me.SessionLongCANADACheckNo = objPaymenManagerBO.Int32CheckNoCanada + 1
    '                Me.SessionLongRefundCheckNo = objPaymenManagerBO.Int32CheckNoRefund + 1
    '                Me.SessionLongUSCheckNo = objPaymenManagerBO.Int32CheckNoPayrol + 1
    '                Me.SessionLongTDLoanCheckNo = objPaymenManagerBO.Int32CheckNoTDLoan + 1
    '                'Aparna
    '                Me.SessionLongEXPCheckNo = objPaymenManagerBO.Int32CheckNoEXP + 1
    '                'Aparna
    '                'Me.DropDownListlDisbursementType.Items.Clear()
    '                'Me.DropDownListlDisbursementType.Items.Add("")

    '                'For Each l_Row As DataRow In g_datatable_disbursementType.Rows
    '                '    If l_Row("chrDisbursementType").GetType.ToString() <> "System.DBNull" Then
    '                '        Me.DropDownListlDisbursementType.Items.Add(CType(l_Row("chvDescription"), String).Trim())
    '                '    End If
    '                'Next
    '                PopulateGrid()
    '                SetNetAmount()
    '                SetValuesInControles()
    '            Case LoadDatasetMode.Session
    '                '/b Ragesh 34231 02/02/06 Cache to Session
    '                'g_datatable_disbursementType = CType(l_Cache.GetData("g_datatable_disbursementType"), DataTable)
    '                'g_datatable_DisbursementsALL = CType(l_Cache.GetData("g_datatable_DisbursementsALL"), DataTable)
    '                'g_datatable_Disbursements = CType(l_Cache.GetData("g_datatable_Disbursements"), DataTable)
    '                'g_datatable_DisbursementsREF = CType(l_Cache.GetData("g_datatable_DisbursementsREF"), DataTable)
    '                'g_datatable_DisbursementsANN = CType(l_Cache.GetData("g_datatable_DisbursementsANN"), DataTable)
    '                'g_datatable_DisbursementsREF_REPL = CType(l_Cache.GetData("g_datatable_DisbursementsREF_REPL"), DataTable)
    '                'g_datatable_DisbursementsANN_REPL = CType(l_Cache.GetData("g_datatable_DisbursementsANN_REPL"), DataTable)
    '                'g_datatable_DisbursementsALL_REPL = CType(l_Cache.GetData("g_datatable_DisbursementsALL_REPL"), DataTable)
    '                g_datatable_DisbursementsALL = Me.Session_datatable_DisbursementsALL
    '                g_datatable_DisbursementsREF = Me.Session_datatable_DisbursementsREF
    '                g_datatable_DisbursementsANN = Me.Session_datatable_DisbursementsANN
    '                g_datatable_DisbursementsTDLoan = Me.Session_datatable_DisbursementsTDLoan
    '                'Aparna
    '                g_datatable_DisbursementsEXP = Me.Session_datatable_DisbursementsEXP
    '                'Aparna
    '                'g_datatable_Disbursements = Me.Session_datatable_DisbursementsALL
    '                g_datatable_disbursementType = Me.Session_datatable_disbursementType
    '                g_datatable_DisbursementsREF_REPL = Me.Session_datatable_DisbursementsREF_REPL
    '                g_datatable_DisbursementsANN_REPL = Me.Session_datatable_DisbursementsANN_REPL
    '                g_datatable_DisbursementsALL_REPL = Me.Session_datatable_DisbursementsALL_REPL
    '                'APARNA
    '                g_datatable_DisbursementsTDLoan_REPL = Me.Session_datatable_DisbursementsTDLoan_REPL
    '                g_datatable_DisbursementsEXP_REPL = Me.Session_datatable_DisbursementsEXP_REPL
    '                'APARNA
    '                'Amit Phase V Changes-Start April 08,2009
    '                g_datatable_DWDisbursement = Me.Session_datatable_DWDisbursement
    '                g_datatable_HWDisbursement = Me.Session_datatable_HWDisbursement
    '                'Phase V Changes-End April 08,2009
    '                '/e Ragesh 34231 02/02/06 Cache to Session
    '                'Me.SessionDataLoadMode = False
    '                PopulateGrid()
    '                If g_bool_setAmount = True Then
    '                    'Phase V Changes-Start
    '                    If g_bool_setGrid = False Then
    '                        'Phase V Changes-End 
    '                        SetNetAmount()
    '                    End If
    '                End If
    '                SetValuesInControles()
    '        End Select
    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Function

    Private Function PopulateData() As Boolean
        'Dim l_Cache As CacheManager
        Dim l_double_sumTotal As Double
        Dim l_String_filterExpr As String
        Dim l_string_DisbursementType As String
        Dim l_StringDisbursement As String
        Dim l_string_checkRefundWithDrawal As String
        Try

            ' START | SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
            l_StringDisbursement = ddlCheckType.SelectedItem.Text.Trim()
            '    'l_StringDisbursement = DropDownListlDisbursementType.SelectedItem.Text.Trim()
            ' END | SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType

            l_StringDisbursement = l_StringDisbursement.PadRight(19, " ")
            l_string_checkRefundWithDrawal = String.Empty
            l_string_DisbursementType = String.Empty

            'START : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Change Annuity Payroll Option to First Annuity Payment
            'If l_StringDisbursement.Substring(0, 15).Trim().ToUpper = "ANNUITY PAYROLL" Then
            If l_StringDisbursement.Substring(0, 13).Trim().ToUpper = "FIRST ANNUITY" Then
                'END : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Change Annuity Payroll Option to First Annuity Payment
                l_string_DisbursementType = "ANN"
            ElseIf l_StringDisbursement.Substring(0, 6).Trim().ToUpper = "REFUND" Then
                l_string_DisbursementType = "REF"

                'Added to check the values of the parameters Gemini issue 786-Amit
                CheckboxParameterValues()
                ' START : SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
                'ElseIf DropDownListlDisbursementType.SelectedItem.Text.Trim.ToUpper = "TAXDEFERRED LOAN" Then
            ElseIf (ddlCheckType.SelectedItem.Text.Trim.ToUpper = "TAXDEFERRED LOAN") Then   ' SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
                ' END : SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
                l_string_DisbursementType = "TDLOAN"
                'Aparna -16/10/2006
            ElseIf l_StringDisbursement.Substring(0, 19).Trim().ToUpper = "EXPERIENCE DIVIDEND" Then
                l_string_DisbursementType = "EXP"
            ElseIf l_StringDisbursement.Substring(0, 19).Trim().ToUpper = "SAFE HARBOR" Then
                l_string_DisbursementType = "REF"
                l_string_checkRefundWithDrawalShira = "SHIRA"
            End If

            Dim l_All As String = l_string_checkRefundWithDrawalAll
            Dim l_HARD As String = l_string_checkRefundWithDrawalHard
            Dim l_DEATH As String = l_string_checkRefundWithDrawalDeath
            '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
            Dim l_SHIRA As String = l_string_checkRefundWithDrawalShira

            Dim l_DISBTYPE As String = l_string_DisbursementType

            'Reset the parameters if the data has already been fetched for it
            If ViewState("IsAllOtherExist") = "True" Then l_All = String.Empty
            If ViewState("IsHardExist") = "True" Then l_HARD = String.Empty
            If ViewState("IsDeathExist") = "True" Then l_DEATH = String.Empty
            '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
            If ViewState("IsShiraExist") = "True" Then l_SHIRA = String.Empty
            If ViewState("DisbursementType") = l_string_DisbursementType Then l_DISBTYPE = String.Empty


            'Call the below logic in all cases except if it is REF and no parameters have been specified
            '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 added shira in if condition
            If Not ((l_string_DisbursementType = "REF" AndAlso l_All = String.Empty AndAlso l_HARD = String.Empty AndAlso l_SHIRA = String.Empty AndAlso l_DEATH = String.Empty) _
             OrElse (l_string_DisbursementType <> "REF" AndAlso l_DISBTYPE = String.Empty)) Then

                objPaymenManagerBO.getdisbursementswithoutfunding(l_string_DisbursementType, l_All, l_HARD, l_DEATH, l_SHIRA, PaymentMethodType) ' SR | 2018.04.11 | YRS-AT-3101 | Added new parameter as PaymentMethodType to retrieve disbursement records based on PaymentMethodType

                If l_All <> String.Empty Then ViewState("IsAllOtherExist") = "True"
                If l_HARD <> String.Empty Then ViewState("IsHardExist") = "True"
                If l_DEATH <> String.Empty Then ViewState("IsDeathExist") = "True"
                '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                If l_SHIRA <> String.Empty Then ViewState("IsShiraExist") = "True"
                If l_DISBTYPE <> String.Empty Then ViewState("DisbursementType") = l_DISBTYPE

                'START : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Filter data based on Currency code
                If Me.IsPaymentOutSourcingKeyON Then 'MMR| 2020.03.16 | YRS-AT-4601 | Added condition to check payment outsourcing key is on
                    If ((l_string_DisbursementType = "ANN") And (rbCurrencyUSA.Checked)) Then
                        If ((From n In CType(objPaymenManagerBO.datatable_Disbursements, DataTable).AsEnumerable() Where n.Field(Of String)("CurrencyCode").Contains("U") Select n).Count > 0) Then
                            g_datatable_DisbursementsALL = (From n In CType(objPaymenManagerBO.datatable_Disbursements, DataTable).AsEnumerable() Where n.Field(Of String)("CurrencyCode").Contains("U") Select n).CopyToDataTable()
                        Else
                            g_datatable_DisbursementsALL = objPaymenManagerBO.datatable_Disbursements.Clone
                        End If
                    ElseIf ((l_string_DisbursementType = "ANN") And (rbCurrencyCanada.Checked)) Then
                        If ((From n In CType(objPaymenManagerBO.datatable_Disbursements, DataTable).AsEnumerable() Where n.Field(Of String)("CurrencyCode").Contains("C") Select n).Count > 0) Then
                            g_datatable_DisbursementsALL = (From n In CType(objPaymenManagerBO.datatable_Disbursements, DataTable).AsEnumerable() Where n.Field(Of String)("CurrencyCode").Contains("C") Select n).CopyToDataTable()
                        Else
                            g_datatable_DisbursementsALL = objPaymenManagerBO.datatable_Disbursements.Clone
                        End If
                    End If
                Else
                    g_datatable_DisbursementsALL = CType(objPaymenManagerBO.datatable_Disbursements, DataTable)
                End If 'MMR| 2020.03.16 | YRS-AT-4601,YRS-AT-4642 | Added condition to check payment outsourcing key is on

                'END : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Filter data based on Currency code

                If Me.Session_datatable_DisbursementsCACHE Is Nothing Then
                    Me.Session_datatable_DisbursementsALL = g_datatable_DisbursementsALL
                    g_datatable_DisbursementsCACHE = g_datatable_DisbursementsALL.Copy()
                    ' Me.Session_datatable_DisbursementsCACHE = g_datatable_DisbursementsALL.Copy()
                    Me.Session_datatable_DisbursementsCACHE = g_datatable_DisbursementsCACHE
                Else
                    For Each dr As DataRow In g_datatable_DisbursementsALL.Rows
                        Me.Session_datatable_DisbursementsCACHE.ImportRow(dr)
                    Next
                    Me.Session_datatable_DisbursementsCACHE = RemoveDuplicateRows(Me.Session_datatable_DisbursementsCACHE, "DisbursementID")
                End If

                Me.SessionCheckDate = objPaymenManagerBO.DateTime_CheckDate
                Me.SessionLongCANADACheckNo = objPaymenManagerBO.Int32CheckNoCanada + 1
                Me.SessionLongRefundCheckNo = objPaymenManagerBO.Int32CheckNoRefund + 1
                'START : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Check number will not autoincrement in case First Annuity -Check - US
                If l_StringDisbursement.Substring(0, 13).Trim().ToUpper = "FIRST ANNUITY" And Me.IsPaymentOutSourcingKeyON Then
                    Me.SessionLongUSCheckNo = 0
                Else
                    Me.SessionLongUSCheckNo = objPaymenManagerBO.Int32CheckNoPayrol + 1
                End If
                'END : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Check number will not autoincrement in case First Annuity -Check - US
                Me.SessionLongTDLoanCheckNo = objPaymenManagerBO.Int32CheckNoTDLoan + 1
                Me.SessionLongEXPCheckNo = objPaymenManagerBO.Int32CheckNoEXP + 1
                '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                Me.SessionLongSHIRAMCheckNo = objPaymenManagerBO.Int32CheckNoSHIRAM + 1
            End If

            'Added by shashi:2009-12-04:To resolve the Issue no BT 1050
            Dim expr As String = PrepareExpression()
            Session("expr") = expr
            HandleReplaceableCheckBox()

        Catch ex As Exception
            Throw
            'Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Function

    Private Sub HandleReplaceableCheckBox()
        If Me.Session_datatable_DisbursementsCACHE Is Nothing Then
            'If data is not available then clear all the controls and reset all values and exit
            BindData()
            SetNetAmount()
            SetValuesInControles()
            Exit Sub
        End If

        Dim expr As String
        expr = Session("expr")
        If Me.CheckBoxReplacedDisbursements.Checked <> True Then
            If expr = "" Then
                expr = expr & "Replaceable=False"
            Else
                expr = expr & "AND Replaceable=False"
            End If
        End If

        g_datatable_DisbursementsALL = GetFilteredDataTable(Me.Session_datatable_DisbursementsCACHE, expr)
        Me.Session_datatable_DisbursementsALL = g_datatable_DisbursementsALL

        g_datatable_Disbursements = Me.Session_datatable_DisbursementsALL

        For Each l_DataRow As DataRow In g_datatable_Disbursements.Rows
            If Not (l_DataRow Is Nothing) Then
                l_DataRow("Selected") = 1
            End If
        Next

        BindData()
        SetNetAmount()
        SetValuesInControles()

    End Sub
    Function SumFields(ByVal parameter_DataTable As DataTable, ByVal Parameter_Column As String) As String
        'Shagufta Chaudhari:2011.08.24: For BT-875, Mismatch on the Total Net Amount message when I click on Pay button.
        'l_Double_sum conversion from Double to Decimal
        Dim l_Double_sum As Decimal
        'End:Shagufta chaudhari:2011.08.24:BT-875
        l_Double_sum = 0.0
        If parameter_DataTable Is Nothing Then Return l_Double_sum
        Try
            For Each l_DataRow As DataRow In parameter_DataTable.Rows
                If l_DataRow(Parameter_Column).GetType.ToString() <> "System.DBNull" And Convert.ToInt32(l_DataRow("Selected")) = 1 Then
                    l_Double_sum = l_Double_sum + Convert.ToDouble(l_DataRow(Parameter_Column))
                End If
            Next
            SumFields = l_Double_sum
        Catch ex As Exception
            'Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
            Throw
        End Try
    End Function

    'Function IdentiFyDataTableOld(ByVal parameter_string As String)
    '    Try
    '        If parameter_string.Trim = "REF" Then
    '            If Me.CheckBoxReplacedDisbursements.Checked = True Then
    '                g_datatable_Disbursements = g_datatable_DisbursementsREF_REPL
    '            Else
    '                'Amit Phase V Changes-start April 08,2009
    '                If Me.RadioButtonListRefundSelect.SelectedValue = "DEATH" Then
    '                    g_datatable_Disbursements = g_datatable_DWDisbursement
    '                ElseIf Me.RadioButtonListRefundSelect.SelectedValue = "HARD" Then
    '                    g_datatable_Disbursements = g_datatable_HWDisbursement
    '                Else
    '                    g_datatable_Disbursements = g_datatable_DisbursementsREF
    '                End If
    '                'Phase V Changes-end April 08,2009
    '            End If
    '        ElseIf parameter_string.Trim = "ANN" Then
    '            If Me.CheckBoxReplacedDisbursements.Checked = True Then
    '                g_datatable_Disbursements = g_datatable_DisbursementsANN_REPL
    '            Else
    '                g_datatable_Disbursements = g_datatable_DisbursementsANN
    '            End If
    '            'changed by ruchi to set the table to td loan disbursement table 
    '        ElseIf parameter_string.Trim = "TDLOAN" Then
    '            'APARNA
    '            If Me.CheckBoxReplacedDisbursements.Checked = True Then
    '                g_datatable_Disbursements = g_datatable_DisbursementsTDLoan_REPL
    '            Else
    '                g_datatable_Disbursements = g_datatable_DisbursementsTDLoan
    '            End If
    '            'Changed by Aparna to change the data table to EXP Disbursements
    '        ElseIf parameter_string.Trim = "EXP" Then
    '            If Me.CheckBoxReplacedDisbursements.Checked = True Then
    '                g_datatable_Disbursements = g_datatable_DisbursementsEXP_REPL
    '            Else
    '                g_datatable_Disbursements = g_datatable_DisbursementsEXP
    '            End If
    '        Else
    '            If Me.CheckBoxReplacedDisbursements.Checked = True Then
    '                g_datatable_Disbursements = g_datatable_DisbursementsALL_REPL
    '            Else
    '                g_datatable_Disbursements = g_datatable_DisbursementsALL
    '            End If
    '        End If
    '    Catch ex As Exception
    '        If Me.CheckBoxReplacedDisbursements.Checked = True Then

    '            g_datatable_Disbursements = g_datatable_DisbursementsALL_REPL
    '        Else
    '            g_datatable_Disbursements = g_datatable_DisbursementsALL
    '        End If
    '    End Try
    'End Function



    'Commented by shashi:2009-11-28: Now not used this functionalities is handled in binddata() function
    '-----------------------------------------------------------------------------------------------------

    'Function IdentiFyDataTable()
    '    Try

    '        If Me.CheckBoxReplacedDisbursements.Checked = True Then
    '            g_datatable_Disbursements = Me.Session_datatable_DisbursementsALL_REPL
    '        Else
    '            g_datatable_Disbursements = Me.Session_datatable_DisbursementsALL
    '        End If

    '    Catch ex As Exception

    '        'commented by Ragesh
    '        'If Me.CheckBoxReplacedDisbursements.Checked = True Then
    '        '    g_datatable_Disbursements = Me.Session_datatable_DisbursementsALL_REPL
    '        'Else
    '        '    g_datatable_Disbursements = Me.Session_datatable_DisbursementsALL
    '        'End If
    '        Throw
    '    End Try
    'End Function

    '--------------------------------------------------------------------------------------------------------

    Function SetValuesInControles()
        Dim l_StringDisbursement As String
        Dim l_tempstring As String
        Dim l_longcheckNoUS As Int32
        Dim l_longcheckNoCanada As Int32
        Try
            ' START | SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType. also, if payment methos is EFT then exit function.   
            'START :ML |2019.11.04 | YRS-AT-4601 |Change Radiobuttonlist to Radiobutton individual
            'If (rblDisbursementType.SelectedValue = PaymentMethod.EFT Or rblDisbursementType.SelectedValue = PaymentMethod.APPROVED_EFT) Then
            If (rbDisbursementTypeEFT.Checked = True Or rbDisbursementTypeAPPROVEDEFT.Checked = True) Then
                'END :ML |2019.11.04 | YRS-AT-4601 |Change Radiobuttonlist to Radiobutton individual
                Exit Function
            End If
            'l_StringDisbursement = DropDownListlDisbursementType.SelectedItem.Text.Trim()
            l_StringDisbursement = ddlCheckType.SelectedItem.Text.Trim()   ' SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
            ' END | SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType.  also, if payment methos is EFT then exit function. .

            l_StringDisbursement = l_StringDisbursement.PadRight(19, " ")
            l_tempstring = CType(Me.TextBoxCheckDate.Text, String).ToString
            If l_tempstring.Trim = String.Empty Then
                Me.TextBoxCheckDate.Text = CType(Me.SessionCheckDate, String).Trim()
            End If

            ' Check No Should hold the new value
            'Me.TextBoxCheckNoUs.Text = Me.SessionLongUSCheckNo.ToString.Trim()
            'Me.TextBoxCheckNoCanadian.Text = Me.SessionLongCANADACheckNo.ToString.Trim()
            l_longcheckNoUS = 0
            Try
                l_longcheckNoUS = Convert.ToInt32(Me.TextBoxCheckNoUs.Text)
            Catch ex As Exception
                l_longcheckNoUS = 0
            End Try
            l_longcheckNoCanada = 0
            Try
                l_longcheckNoCanada = Convert.ToInt32(Me.TextBoxCheckNoCanadian.Text)
            Catch ex As Exception
                l_longcheckNoCanada = 0
            End Try
            'START : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Change Annuity Payroll Option to First Annuity Payment
            'If l_StringDisbursement.Substring(0, 15).Trim().ToUpper = "ANNUITY PAYROLL" Then
            If l_StringDisbursement.Substring(0, 13).Trim().ToUpper = "FIRST ANNUITY" Then
                If Me.IsPaymentOutSourcingKeyON = True Then
                    l_longcheckNoUS = 0
                    Me.TextBoxCheckNoUs.Text = 0
                Else
                    If l_longcheckNoUS = 0 Then Me.TextBoxCheckNoUs.Text = Me.SessionLongUSCheckNo.ToString.Trim()
                End If
                'END : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Change Annuity Payroll Option to First Annuity Payment

                If l_longcheckNoUS = 0 Then Me.TextBoxCheckNoUs.Text = Me.SessionLongUSCheckNo.ToString.Trim()
                If l_longcheckNoCanada = 0 Then Me.TextBoxCheckNoCanadian.Text = Me.SessionLongCANADACheckNo.ToString.Trim()

                If Me.CheckBoxManualCheck.Checked = True Then
                    Me.TextBoxCheckNoCanadian.Enabled = False
                    Me.TextBoxCheckNoUs.Enabled = False
                Else
                    Me.TextBoxCheckNoCanadian.Enabled = True
                    Me.TextBoxCheckNoUs.Enabled = True
                End If

                'Me.TextBoxCheckDate.Text = CType(Me.SessionCheckDate, String).Trim()
                Me.TextBoxCheckDate.Enabled = True

                If g_datatable_Disbursements.Rows.Count > 0 Then
                    Me.ButtonPay.Enabled = True
                    Me.ButtonPrint.Enabled = True
                Else
                    Me.ButtonPay.Enabled = False
                    Me.ButtonPrint.Enabled = False
                End If

            ElseIf l_StringDisbursement.Substring(0, 6).Trim().ToUpper = "REFUND" Then

                If l_longcheckNoUS = 0 Then Me.TextBoxCheckNoUs.Text = Me.SessionLongRefundCheckNo.ToString.Trim()

                '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                'Code Commneted by Dinesh kanojia on 20/05/2013: BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
                'If CheckBoxSHIRA.Checked = True Then Me.TextBoxCheckNoUs.Text = Me.SessionLongSHIRAMCheckNo.ToString.Trim()

                'If Me.SessionShiraWithdrawals = True Then Me.TextBoxCheckNoUs.Text = Me.SessionLongSHIRAMCheckNo.ToString.Trim()

                If l_longcheckNoCanada = 0 Then Me.TextBoxCheckNoCanadian.Text = Me.SessionLongCANADACheckNo.ToString.Trim()
                If Me.CheckBoxManualCheck.Checked = True Then
                    Me.TextBoxCheckNoCanadian.Enabled = False
                    Me.TextBoxCheckNoUs.Enabled = False
                Else
                    Me.TextBoxCheckNoCanadian.Enabled = True
                    Me.TextBoxCheckNoUs.Enabled = True

                End If
                'Me.TextBoxCheckDate.Text = CType(Me.SessionCheckDate, String).Trim()
                Me.TextBoxCheckDate.Enabled = True
                If Not g_datatable_Disbursements Is Nothing Then
                    If g_datatable_Disbursements.Rows.Count > 0 Then
                        Me.ButtonPay.Enabled = True
                        Me.ButtonPrint.Enabled = True
                    Else
                        Me.ButtonPay.Enabled = False
                        Me.ButtonPrint.Enabled = False
                    End If
                End If
                'Start : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
            ElseIf l_StringDisbursement.Substring(0, 19).Trim().ToUpper = "SAFE HARBOR" Then

                If l_longcheckNoUS = 0 Then Me.TextBoxCheckNoUs.Text = Me.SessionLongRefundCheckNo.ToString.Trim()
                Me.TextBoxCheckNoUs.Text = Me.SessionLongSHIRAMCheckNo.ToString.Trim()
                If l_longcheckNoCanada = 0 Then Me.TextBoxCheckNoCanadian.Text = Me.SessionLongCANADACheckNo.ToString.Trim()
                If Me.CheckBoxManualCheck.Checked = True Then
                    Me.TextBoxCheckNoCanadian.Enabled = False
                    Me.TextBoxCheckNoUs.Enabled = False
                Else
                    Me.TextBoxCheckNoCanadian.Enabled = True
                    Me.TextBoxCheckNoUs.Enabled = True

                End If
                Me.TextBoxCheckDate.Enabled = True
                If Not g_datatable_Disbursements Is Nothing Then
                    If g_datatable_Disbursements.Rows.Count > 0 Then
                        Me.ButtonPay.Enabled = True
                        Me.ButtonPrint.Enabled = True
                    Else
                        Me.ButtonPay.Enabled = False
                        Me.ButtonPrint.Enabled = False
                    End If
                End If
                'End : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
            ElseIf l_StringDisbursement.Trim().ToUpper = "TAXDEFERRED LOAN" Then

                If l_longcheckNoUS = 0 Then Me.TextBoxCheckNoUs.Text = Me.SessionLongTDLoanCheckNo.ToString.Trim
                If l_longcheckNoCanada = 0 Then Me.TextBoxCheckNoCanadian.Text = Me.SessionLongCANADACheckNo.ToString.Trim()
                If Me.CheckBoxManualCheck.Checked = True Then
                    Me.TextBoxCheckNoCanadian.Enabled = False
                    Me.TextBoxCheckNoUs.Enabled = False
                Else
                    Me.TextBoxCheckNoCanadian.Enabled = True
                    Me.TextBoxCheckNoUs.Enabled = True
                End If
                'Me.TextBoxCheckDate.Text = CType(Me.SessionCheckDate, String).Trim()
                Me.TextBoxCheckDate.Enabled = True
                If Not g_datatable_Disbursements Is Nothing Then
                    If g_datatable_Disbursements.Rows.Count > 0 Then
                        Me.ButtonPay.Enabled = True
                        Me.ButtonPrint.Enabled = True
                    Else
                        Me.ButtonPay.Enabled = False
                        Me.ButtonPrint.Enabled = False
                    End If
                End If

                'Aparna -16/10/2006

            ElseIf l_StringDisbursement.Substring(0, 19).Trim().ToUpper = "EXPERIENCE DIVIDEND" Then
                If l_longcheckNoUS = 0 Then Me.TextBoxCheckNoUs.Text = Me.SessionLongEXPCheckNo.ToString.Trim
                If l_longcheckNoCanada = 0 Then Me.TextBoxCheckNoCanadian.Text = Me.SessionLongCANADACheckNo.ToString.Trim()
                If Me.CheckBoxManualCheck.Checked = True Then
                    Me.TextBoxCheckNoCanadian.Enabled = False
                    Me.TextBoxCheckNoUs.Enabled = False
                Else
                    Me.TextBoxCheckNoCanadian.Enabled = True
                    Me.TextBoxCheckNoUs.Enabled = True
                End If

                'Me.TextBoxCheckDate.Text = CType(Me.SessionCheckDate, String).Trim()
                Me.TextBoxCheckDate.Enabled = True
                If Not g_datatable_Disbursements Is Nothing Then
                    If g_datatable_Disbursements.Rows.Count > 0 Then
                        Me.ButtonPay.Enabled = True
                        Me.ButtonPrint.Enabled = True
                    Else
                        Me.ButtonPay.Enabled = False
                        Me.ButtonPrint.Enabled = False
                    End If
                End If

                'Aparna -16/10/2006

            Else
                Me.TextBoxCheckNoUs.Text = String.Empty
                Me.TextBoxCheckNoCanadian.Text = String.Empty
                Me.TextBoxCheckDate.Text = String.Empty
                Me.TextBoxCheckDate.Enabled = False
                Me.TextBoxCheckNoCanadian.Enabled = False
                Me.TextBoxCheckNoUs.Enabled = False
                Me.ButtonPay.Enabled = False
                Me.ButtonPrint.Enabled = False
            End If

            If Me.CheckBoxManualCheck.Checked = True Then
                Me.RadioButtonCanada.Enabled = True
                Me.RadioButtonUSA.Enabled = True
                Me.TextBoxManualCheckNo.Enabled = True
                Me.CheckBoxProcessAllSelectedDisbursements.Enabled = False
            Else
                Me.RadioButtonCanada.Enabled = False
                Me.RadioButtonUSA.Enabled = False
                Me.TextBoxManualCheckNo.Enabled = False
                Me.CheckBoxProcessAllSelectedDisbursements.Enabled = True
            End If
            Me.TextBoxNetAmount.Enabled = False ' SR | 2018.10.24 | YRS-AT-3101 - Renamed from TextBoxboxNetAmount to TextBoxNetAmount.
        Catch ex As Exception
            Throw
        End Try

    End Function

    Function ShowMessage()
        Dim l_String_Message As String
        Try
            l_String_Message = "Warning - Found NEGATIVE disbursements." + vbCrLf + _
                           "Please check with Help Desk to take appropriate action." + vbCrLf + _
                           "Payment manager will continue without these negative " + vbCrLf + _
                           "disbursements and further warnings."
            MessageBox.Show(PlaceHolder1, "Negative Disbursements Warning", l_String_Message, MessageBoxButtons.OK)
        Catch ex As Exception
            'Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
            Throw
        End Try
    End Function

    Function ValidateData() As Boolean
        Dim l_string_Message As String
        Dim l_dataTable As DataTable
        Dim l_DataRows As DataRow()
        Dim l_String_FilterExpr As String
        Dim l_longTemp As Int32

        Try
            ValidateData = True
            l_dataTable = g_datatable_Disbursements
            l_String_FilterExpr = "Selected = 1"
            l_DataRows = l_dataTable.Select(l_String_FilterExpr)
            l_longTemp = 0
            If ValidateCheckNumbers() = False Then
                ValidateData = False
                Exit Function
            End If
            If l_DataRows.Length <= 0 Then
                MessageBox.Show(PlaceHolder1, "Pay disbursements", "Please select at least one disbursement for processing.", MessageBoxButtons.Stop)
                ValidateData = False
                Exit Function
            End If
            'START : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | No need to check Validation for First Annuity -Check -Us currency
            Dim l_StringDisbursement As String
            l_StringDisbursement = ddlCheckType.SelectedItem.Text.Trim.ToUpper
            l_StringDisbursement = l_StringDisbursement.PadRight(19, " ")
            If ((Me.IsPaymentOutSourcingKeyON = False) Or (l_StringDisbursement.Substring(0, 13).Trim().ToUpper = "FIRST ANNUITY") And (rbCurrencyCanada.Checked)) Or (l_StringDisbursement.Substring(0, 13).Trim().ToUpper <> "FIRST ANNUITY") Then 'MMR| 2020.03.16 | YRS-AT-4601 | Added condition to check payment outsourcing key is on

                'Added an if condition for validating that its not a manual check feed
                If Me.CheckBoxManualCheck.Checked = False Then
                    'Added to validate the check date in case of Manual Check feed
                    If Convert.ToDateTime(Me.TextBoxCheckDate.Text.Trim()) < Convert.ToDateTime(CType(System.DateTime.Now.Date(), String)) And Me.CheckBoxManualCheck.Checked = False Then
                        MessageBox.Show(PlaceHolder1, "YRS-DateCheck", "Check date cannot be lesser than current date", MessageBoxButtons.Stop)
                        ValidateData = False
                        Exit Function
                    End If
                    If Convert.ToDateTime(Me.TextBoxCheckDate.Text.Trim()) > Me.SessionAccountingDate Then
                        MessageBox.Show(PlaceHolder1, "YRS-DateCheck", "Check date cannot exceed current accounting period " + Me.SessionAccountingDate.ToShortDateString, MessageBoxButtons.Stop)
                        ValidateData = False
                        Exit Function
                    End If
                End If

                If Me.CheckBoxManualCheck.Checked = True Then
                    If Me.TextBoxManualCheckNo.Text.Trim() = String.Empty Then
                        MessageBox.Show(PlaceHolder1, "Manual Check Feed", "Please enter a valid check number for manual check feed", MessageBoxButtons.Stop)
                        ValidateData = False
                        Exit Function
                    End If
                    If l_DataRows.Length > 1 Then
                        MessageBox.Show(PlaceHolder1, "Manual check feed", "Manual check feed is allowed for ONE disbursement at a time." + vbCrLf + "Please de-select disbursements except one you want to process.", MessageBoxButtons.Stop)
                        ValidateData = False
                        Exit Function
                    End If
                    'Added to validate the check date in case of Manual Check feed
                    ValidateData = CheckAccountingdateOnManual()
                    'Added to validate the check date in case of Manual Check feed

                End If
            End If
            'END : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | START : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | No need to check Validation for First Annuity -Check -Us currency
        Catch ex As Exception
            ValidateData = False
        End Try
    End Function
    'Added to validate the check date in case of Manual Check feed
    Private Function CheckAccountingdateOnManual() As Boolean

        Dim dAcctDate As DateTime = Me.SessionAccountingDate
        Dim dtfirstdayofPreviousMonth As DateTime
        Dim dtenddayofnextmonth As DateTime

        'dAcctDate = this.DateTime_AccountingDate; 
        dAcctDate = dAcctDate.AddMonths(1)
        Dim dTemp As DateTime = dAcctDate
        While dAcctDate.Month = dTemp.Month
            dAcctDate = dAcctDate.AddDays(1)
        End While
        dtenddayofnextmonth = dAcctDate.AddDays(-1)

        dAcctDate = Me.SessionAccountingDate.AddMonths(-2)

        dTemp = dAcctDate
        While dAcctDate.Month = dTemp.Month
            dAcctDate = dAcctDate.AddDays(1)
        End While
        dtfirstdayofPreviousMonth = dAcctDate

        Dim l_stringMessage As String
        l_stringMessage = "Please enter the valid check date"
        If Convert.ToDateTime(Me.TextBoxCheckDate.Text.Trim()) < dtfirstdayofPreviousMonth Or Convert.ToDateTime(Me.TextBoxCheckDate.Text.Trim()) > dtenddayofnextmonth Then
            MessageBox.Show(PlaceHolder1, "YRS-DateCheck", "Manual Check date should be in the range of " + dtfirstdayofPreviousMonth.ToShortDateString + " And " + dtenddayofnextmonth.ToShortDateString, MessageBoxButtons.Stop)
            CheckAccountingdateOnManual = False
            Exit Function
        End If
        CheckAccountingdateOnManual = True
    End Function
    'Added to validate the check date in case of Manual Check feed


    Function ValidateCheckNumbers() As Boolean
        'Validate Check#
        Dim l_longTemp As Int32
        Dim l_StringDisbursement As String
        Dim l_longOriginalCheckNOUS As Int32
        Dim l_longOriginalCheckNOCanada As Int32

        Try
            ' START : SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType.
            'l_StringDisbursement = DropDownListlDisbursementType.SelectedItem.Text.Trim.ToUpper
            l_StringDisbursement = ddlCheckType.SelectedItem.Text.Trim.ToUpper
            ' START : SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType.

            'aparna
            'l_StringDisbursement = l_StringDisbursement.PadRight(16, " ")
            l_StringDisbursement = l_StringDisbursement.PadRight(19, " ")
            'Assign  the check Numbers to the default.
            'START : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Change Annuity Payroll Option to First Annuity Payment
            'If l_StringDisbursement.Substring(0, 15).Trim().ToUpper = "ANNUITY PAYROLL" Then
            If l_StringDisbursement.Substring(0, 13).Trim().ToUpper = "FIRST ANNUITY" Then
                If Me.IsPaymentOutSourcingKeyON = True Then
                    l_longOriginalCheckNOUS = 0
                Else
                    l_longOriginalCheckNOUS = Me.SessionLongUSCheckNo
                End If
                'END : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Change Annuity Payroll Option to First Annuity Payment

                l_longOriginalCheckNOCanada = Me.SessionLongCANADACheckNo
            ElseIf l_StringDisbursement.Substring(0, 6).Trim().ToUpper = "REFUND" Then
                l_longOriginalCheckNOUS = Me.SessionLongRefundCheckNo
                '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

                'Start: Dinesh Kanojia : 20/05/2013: BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
                'If Me.CheckBoxSHIRA.Checked = True Then
                '    l_longOriginalCheckNOUS = Me.SessionLongSHIRAMCheckNo
                'End If
                l_longOriginalCheckNOCanada = Me.SessionLongCANADACheckNo
            ElseIf l_StringDisbursement.Substring(0, 19).Trim().ToUpper = "SAFE HARBOR" Then
                l_longOriginalCheckNOUS = Me.SessionLongSHIRAMCheckNo
                l_longOriginalCheckNOCanada = Me.SessionLongCANADACheckNo
                'END: Dinesh Kanojia : 20/05/2013: BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
            ElseIf l_StringDisbursement.Trim().ToUpper = "TAXDEFERRED LOAN" Then
                l_longOriginalCheckNOUS = Me.SessionLongTDLoanCheckNo
                l_longOriginalCheckNOCanada = Me.SessionLongCANADACheckNo
                'aparna
            ElseIf l_StringDisbursement.Substring(0, 19).Trim().ToUpper = "EXPERIENCE DIVIDEND" Then
                l_longOriginalCheckNOUS = Me.SessionLongEXPCheckNo
                l_longOriginalCheckNOCanada = Me.SessionLongCANADACheckNo
            End If
            'aparna
            Try
                l_longTemp = l_longOriginalCheckNOUS 'Convert.ToInt32(Me.TextBoxCheckNoUs.Text)
            Catch ex As Exception
                l_longTemp = 0
            End Try
            If l_longTemp < l_longOriginalCheckNOUS Then
                MessageBox.Show(PlaceHolder1, "Pay disbursements", "Check Number cannot be less than " + l_longOriginalCheckNOUS.ToString.Trim, MessageBoxButtons.Stop)
                ValidateCheckNumbers = False
                Exit Function
            End If
            l_longTemp = 0
            Try
                l_longTemp = Convert.ToInt32(Me.TextBoxCheckNoCanadian.Text)
            Catch ex As Exception
                l_longTemp = 0
            End Try
            If l_longTemp < l_longOriginalCheckNOCanada Then
                MessageBox.Show(PlaceHolder1, "Pay disbursements", "Check Number cannot be less than " + l_longOriginalCheckNOCanada.ToString.Trim, MessageBoxButtons.Stop)
                ValidateCheckNumbers = False
                Exit Function
            End If
            ValidateCheckNumbers = True
        Catch
            Throw
        End Try
    End Function

    '*****************************************modification done for the payment manager
    Private Function SetVisibleProperty(ByVal l_bool_Flag As Boolean)
        'Me.RadioButtonListRefundSelect.Visible = l_bool_Flag
        'Me.RadioButtonListRefundSelect.SelectedIndex = -1
        'Commented the above line of code for the Gemini issue 786-Amit
        Me.CheckBoxAllOtherWithdrawals.Visible = l_bool_Flag
        Me.CheckBoxHardshipWithdrawals.Visible = l_bool_Flag
        Me.CheckBoxDeathWithdrawals.Visible = l_bool_Flag
        Me.ButtonLoadRefund.Visible = l_bool_Flag
        Me.LabelWithdrawals.Visible = l_bool_Flag
        '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
        'Me.CheckBoxSHIRA.Visible = l_bool_Flag

        'START: PPP | 04/27/2018 | YRS-AT-3101 | Hiding or Showing Withdrawal options strip based on flag
        If l_bool_Flag Then
            tblWithdrawalOptions.Style("display") = "normal"
        Else
            tblWithdrawalOptions.Style("display") = "none"
        End If
        'END: PPP | 04/27/2018 | YRS-AT-3101 | Hiding or Showing Withdrawal options strip based on flag
    End Function
    '*****************************************modification done for the payment manager
    'Amit Phase V Changes-End

    Function PAY()
        Dim l_bool_fund As Boolean
        Dim l_Int32_checknbrUS As Int32
        Dim l_Int32_checknbrCANADA As Int32
        Dim l_StringDisbursement As String
        Dim l_string_AnnuityType As String
        Dim l_String_FilterExpr As String
        Dim l_String_ManualCheckNo As String
        Dim l_dateTimeCheckDate As DateTime
        Dim arrayFileList(4, 4) As String
        Dim l_dataTableFinalDisbursement As DataTable
        Dim i As Integer
        Dim popupScript As String
        '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
        Dim l_bool_ShiraM As Boolean = False
        'END 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
        Dim successMessage As String, emailMessage As String 'MMR | 2018.12.03 | YRS-AT-3101 | Declared variables to store disbursement & email status message
        Try
            l_bool_fund = Me.CheckBoxManualCheck.Checked
            l_Int32_checknbrUS = Convert.ToInt32(Me.TextBoxCheckNoUs.Text.Trim())
            l_Int32_checknbrCANADA = Convert.ToInt32(Me.TextBoxCheckNoCanadian.Text.Trim())

            l_dateTimeCheckDate = CType(Me.TextBoxCheckDate.Text, DateTime)
            ' START : SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType. also,Select disbursement type based on payment method.
            'l_StringDisbursement = DropDownListlDisbursementType.SelectedItem.Text.Trim()
            'START :ML |2019.11.04 | YRS-AT-4601 |Change Radiobuttonlist to Radiobutton individual
            'If (rblDisbursementType.SelectedValue = PaymentMethod.EFT Or rblDisbursementType.SelectedValue = PaymentMethod.APPROVED_EFT) Then
            If (rbDisbursementTypeEFT.Checked = True Or rbDisbursementTypeAPPROVEDEFT.Checked = True) Then
                'END :ML |2019.11.04 | YRS-AT-4601 |Change Radiobuttonlist to Radiobutton individual
                l_StringDisbursement = ddlEFTType.SelectedItem.Text.Trim()
            Else
                l_StringDisbursement = ddlCheckType.SelectedItem.Text.Trim()
            End If
            ' END : SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType. also,Select disbursement type based on payment method.


            l_StringDisbursement = l_StringDisbursement.PadRight(19, " ")

            'START : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Change Annuity Payroll Option to First Annuity Payment
            'If l_StringDisbursement.Substring(0, 15).Trim().ToUpper = "ANNUITY PAYROLL" Then
            If l_StringDisbursement.Substring(0, 13).Trim().ToUpper = "FIRST ANNUITY" Then
                'END : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Change Annuity Payroll Option to First Annuity Payment
                l_string_AnnuityType = "ANN"
            ElseIf l_StringDisbursement.Substring(0, 6).Trim().ToUpper = "REFUND" Then
                l_string_AnnuityType = "REF"
                ' START | SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType. also,Select disbursement type based on payment method.
                'ElseIf DropDownListlDisbursementType.SelectedItem.Text.Trim.ToUpper = "TAXDEFERRED LOAN" Then
            ElseIf ddlCheckType.SelectedItem.Text.Trim.ToUpper = "TAXDEFERRED LOAN" Or ddlEFTType.SelectedItem.Text.Trim.ToUpper = "TAXDEFERRED LOAN" Then
                ' END | SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType. also,Select disbursement type based on payment method.
                l_string_AnnuityType = "TDLOAN"
                'Aparna -16/10/2006
            ElseIf l_StringDisbursement.Substring(0, 19).Trim().ToUpper = "EXPERIENCE DIVIDEND" Then
                l_string_AnnuityType = "EXP"
                'Aparna -16/10/2006
                'Start: Dinesh Kanojia : 20/05/2013:BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list 
            ElseIf l_StringDisbursement.Substring(0, 19).Trim().ToUpper = "SAFE HARBOR" Then
                l_string_AnnuityType = "REF"
                l_bool_ShiraM = True
                'End: Dinesh Kanojia : 20/05/2013:BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
            Else
                l_string_AnnuityType = String.Empty
            End If

            '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
            'Start : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
            'If CheckBoxSHIRA.Checked = True Then
            '    l_bool_ShiraM = True
            'End If
            'End : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
            'END 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
            If Me.CheckBoxManualCheck.Checked = True Then
                l_String_ManualCheckNo = Me.TextBoxManualCheckNo.Text
            Else
                l_String_ManualCheckNo = String.Empty
            End If

            If ValidateData() Then

                l_dataTableFinalDisbursement = ArrangeRecordstoSortOrder()
                l_String_FilterExpr = "Selected = 1"
                'START: MMR | 2018.12.03 | YRS-AT-3101 | Added columns for email activity
                If Not l_dataTableFinalDisbursement.Columns.Contains("IsEmailSent") Then
                    l_dataTableFinalDisbursement.Columns.Add(HelperFunctions.CreateDataTableColumn("IsEmailSent", "System.Boolean", "0"))
                End If
                If Not l_dataTableFinalDisbursement.Columns.Contains("ReasonCode") Then
                    l_dataTableFinalDisbursement.Columns.Add(HelperFunctions.CreateDataTableColumn("ReasonCode", "System.String", ""))
                End If
                If Not l_dataTableFinalDisbursement.Columns.Contains("Reason") Then
                    l_dataTableFinalDisbursement.Columns.Add(HelperFunctions.CreateDataTableColumn("Reason", "System.String", ""))
                End If
                'END: MMR | 2018.12.03 | YRS-AT-3101 | Added columns for email activity
                objPaymenManagerBO.dtExpceptionLogDetails = Nothing ' ML : YRS-AT-4676 | 2019.12.16 | Set error datatable as empty.
                lblNote.Text = "Note:" + YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PM_DISBURSEMENT_FAILURENOTE).DisplayText() ' ML : YRS-AT-4676 | 2019.12.16 | Set error Note.
                objPaymenManagerBO.IsPaymentOutSourcingKeyON = Me.IsPaymentOutSourcingKeyON
                If (objPaymenManagerBO.Pay(l_dataTableFinalDisbursement, l_dateTimeCheckDate, l_bool_fund, l_Int32_checknbrUS, l_Int32_checknbrCANADA, l_string_AnnuityType, l_String_FilterExpr, l_String_ManualCheckNo, l_bool_ShiraM)) Then
                    'START: MMR | 2018.12.03 | YRS-AT-3101 | Commented as message displayed in div on top instead of server side message
                    'MessageBox.Show(PlaceHolder1, "Process disbursements", "Successfully processed disbursements", MessageBoxButtons.OK)
                    successMessage = "Successfully processed disbursements"
                    If ddlCheckType.SelectedItem.Text.Trim.ToUpper = "TAXDEFERRED LOAN" Then
                        emailMessage = DisplayEmailErrorForCheck(l_dataTableFinalDisbursement)
                        If Not String.IsNullOrEmpty(emailMessage) Then
                            successMessage = String.Format("{0}{1}", successMessage, emailMessage)
                        End If
                    End If

                    'START: PK | 12-12-2019 | YRS-AT-4676 |Code to display link in success message
                    'MessageBox.Show(PlaceHolder1, "Process disbursements", successMessage, MessageBoxButtons.OK)                    
                    If (HelperFunctions.isNonEmpty(objPaymenManagerBO.dtExpceptionLogDetails)) Then

                        gvWarnErrorMessage.DataSource = objPaymenManagerBO.dtExpceptionLogDetails
                        gvWarnErrorMessage.DataBind()
                        successMessage = successMessage + YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PM_DISBURSEMENT_DISPLAY_ERRORWARNINGS).DisplayText()
                    End If
                    divSuccessMessage.InnerHtml = successMessage
                    divSuccessMessage.Visible = True
                    'END: PK | 12-12-2019 | YRS-AT-4676 |Code to display link in success message

                    'END: MMR | 2018.12.03 | YRS-AT-3101 | Commented as message diplayed in div on top instead of server side message
                    'Added by Shashi Shekhar on 17 nov 2009 to resolve the Issue No 1026 on BT
                    'ButtonSelectNone.Text = "Select None" 'VC | 2018.09.11 | YRS-AT-3101 |  Commented to remove ButtonSelectNone

                    Me.SessionDataLoadMode = False
                    Me.TextBoxManualCheckNo.Text = String.Empty
                    Me.TextBoxCheckNoUs.Text = String.Empty
                    Me.TextBoxCheckNoCanadian.Text = String.Empty
                    'start of commenting to implement datatable
                    ''''For i = 0 To 4
                    ''''    arrayFileList(i, 0) = CType(objPaymenManagerBO.arrayNameFileList(i)(0), String)
                    ''''    arrayFileList(i, 1) = CType(objPaymenManagerBO.arrayNameFileList(i)(1), String)
                    ''''    arrayFileList(i, 2) = CType(objPaymenManagerBO.arrayNameFileList(i)(2), String)
                    ''''    arrayFileList(i, 3) = CType(objPaymenManagerBO.arrayNameFileList(i)(3), String)
                    ''''Next
                    'end of commenting
                    'start of change by ruchi
                    Session("FTFileList") = Nothing
                    Session("FTFileList") = objPaymenManagerBO.DataTableNameFileList
                    ''CALLING ASPX PAGE
                    popupScript = "<script language='javascript'>" & _
                      "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1', 'FileCopyPopUp', " & _
                      "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                      "</script>"
                    If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                        Page.RegisterStartupScript("PopupScript1", popupScript)
                    End If
                    'end of change

                    '-----------------------------------------------------------------------------------------------------
                    'Added by shashi:2009-12-02
                    'DistroyCashedData() 'ML | YRS-AT-4601 | 2019.12.03 | Change method name due to spelling mistake
                    DistroyCachedData()
                    Me.PopulateData()

                    Me.SessionDataLoadMode = True
                    'START : ML | 2019.10.22 | YRS-AT-4601 | Handle Success and failure for Northern Trust Bank file exported 
                Else
                    Dim errorMessage As String

                    errorMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PM_DISBURSEMENT_FAILURE).DisplayText
                    If (HelperFunctions.isNonEmpty(objPaymenManagerBO.dtExpceptionLogDetails)) Then
                        gvWarnErrorMessage.DataSource = objPaymenManagerBO.dtExpceptionLogDetails
                        gvWarnErrorMessage.DataBind()
                        errorMessage = errorMessage + YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_PM_DISBURSEMENT_DISPLAY_ERRORWARNINGS).DisplayText()
                    End If

                    divDBErrorMessage.InnerHtml = errorMessage
                    divDBErrorMessage.Visible = True

                    Me.SessionDataLoadMode = False
                    Me.TextBoxManualCheckNo.Text = String.Empty
                    Me.TextBoxCheckNoUs.Text = String.Empty
                    Me.TextBoxCheckNoCanadian.Text = String.Empty

                    DistroyCachedData()
                    Me.PopulateData()

                    Me.SessionDataLoadMode = True
                    'END : ML | 2019.10.22 | YRS-AT-4601 | Handle Success and failure for Northern Trust Bank file exported 
                End If
            End If

        Catch ex As Exception

            Dim l_string_ErrorMsg As String
            If ex.Message <> "" Then
                If objPaymenManagerBO.string_ErrorString <> String.Empty Then
                    l_string_ErrorMsg = objPaymenManagerBO.string_ErrorString
                    objPaymenManagerBO.string_ErrorString = String.Empty
                    Me.ButtonDeselectErroneousDisbursements.Enabled = True
                    MessageBox.Show(PlaceHolder1, "Error processing disbursements", l_string_ErrorMsg, MessageBoxButtons.Stop)
                Else
                    'l_string_ErrorMsg = ex.Message()
                    l_string_ErrorMsg = "ERROR processing disbursements." + vbCrLf + _
                    "Processed disbursements are reverted to their original state." + vbCrLf + _
                    "Please check with help desk for disbursements causing error" + vbCrLf + _
                    "de-select disbursements having error and continue processing."
                    Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString() + vbCrLf + " " + l_string_ErrorMsg), False)
                End If
            Else
                Throw
                'Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
            End If
        End Try
    End Function

    Function ArrangeRecordstoSortOrder() As DataTable
        Dim i As Long
        Dim l_SelDataGridItem As DataGridItem
        Dim l_Find_Datatow As DataRow()
        Dim l_String_Search As String
        Dim l_CheckBox As CheckBox
        Dim l_DataTable As DataTable
        Try

            l_DataTable = g_datatable_Disbursements.Clone()
            For Each l_DataGridItem As DataGridItem In Me.DataGridPayment.Items
                l_CheckBox = l_DataGridItem.FindControl("CheckBoxSelect")
                If l_CheckBox.Checked = True Then
                    'Phase V Changes-Start
                    l_String_Search = "DisbursementID = '" + l_DataGridItem.Cells(7).Text + "'"
                    'Phase V Changes-End 
                    l_Find_Datatow = g_datatable_Disbursements.Select(l_String_Search)
                    If l_Find_Datatow.Length > 0 Then
                        l_DataTable.ImportRow(l_Find_Datatow(0))
                        'Else

                        '    MessageBox.Show(PlaceHolder1, "Payment Manager", "Error while reading records from the user(s) selection", MessageBoxButtons.Stop)

                        '    Exit For
                    End If
                End If
                i = i + 1
            Next
            ArrangeRecordstoSortOrder = l_DataTable
        Catch ex As Exception
            Throw
        End Try
    End Function

    Function deselecterroneousDisbursement()
        Dim l_dataTable As DataTable
        Dim l_findTable As DataTable
        Dim foundrows() As DataRow
        Dim l_String_Filter
        Try
            l_dataTable = objPaymenManagerBO.GetErroneousDisbursements(Me.SessionPMStartDate)
            l_findTable = g_datatable_Disbursements
            For Each l_DataRow As DataRow In l_dataTable.Rows
                l_String_Filter = " Selected = 1 "
                foundrows = l_findTable.Select(l_String_Filter)
                If foundrows.Length > 0 Then
                    For Each l_SelectedDataRow As DataRow In foundrows
                        If Convert.ToString(l_SelectedDataRow("DisbursementID")).Trim() = Convert.ToString(l_DataRow("DisbursementID")).Trim() Then
                            l_SelectedDataRow("Selected") = 0
                            Exit For
                        End If
                    Next
                End If
            Next

            g_datatable_Disbursements.AcceptChanges()
            Me.SessionDataLoadMode = False
            'PopulateGrid()'Commented by shashi:2009-11-28
            SetNetAmount()
            'SetValuesInControles()
            Me.SessionDataLoadMode = True
        Catch ex As Exception
            Throw
        End Try

    End Function
    'Phase V Changes-Start
    Private Function ClearSessions()
        Me.Session_datatable_DisbursementsALL = Nothing
        'Commented by shashi:2009-12-04:Not used now
        'Me.Session_datatable_DisbursementsALL_REPL = Nothing 
        Me.Session_datatable_DisbursementsCACHE = Nothing 'added by shashi:2009-12-02 

        Me.EFTDisbursementsPendingForApproval = Nothing ' PPP | 04/30/2018 | YRS-AT-3101 | This session holds EFT disbursements pending for approval or rejection which needs to be cleared before any action is performed

        ' commented as not in use 
        'Me.Session_datatable_DisbursementsREF = Nothing
        'Me.Session_datatable_DisbursementsANN = Nothing
        'Me.Session_datatable_DisbursementsTDLoan = Nothing
        'Me.Session_datatable_DWDisbursement = Nothing
        'Me.Session_datatable_HWDisbursement = Nothing
        'Me.Session_datatable_DisbursementsANN_REPL = Nothing
        'Me.Session_datatable_DisbursementsALL_REPL = Nothing
        'Me.Session_datatable_DisbursementsTDLoan_REPL = Nothing
        'Me.Session_datatable_DisbursementsEXP_REPL = Nothing
        clearFundIdandGridIndex()
    End Function

    Private Function LoadDataGrid()
        Dim l_CheckBox As CheckBox
        Dim l_bool_flag As Boolean
        Dim l_StringDisbursement As String
        Try
            l_bool_flag = True
            'ButtonSelectNone.Text = "Select None" 'VC | 2018.09.11 | YRS-AT-3101 |  Commented to remove ButtonSelectNone
            Me.SessionDataLoadMode = False

            '----------------------------------------------------------------
            'Shashi Shekhar:2009-11-26 
            CheckboxParameterValues() 'Set global parameter value according to check box state
            Me.PopulateData()

            '----------------------------------------------------------------------------------
            ' Me.PopulateData(LoadDatasetMode.Table) 'commented by shashi:2009-11-26
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function clearFundIdandGridIndex()
        Try
            'Amit Phase V Changes-start
            'fundId No text box should be cleared
            TextBoxFundIdNo.Text = String.Empty
            ' START : SR | 2018.04.23 | YRS-AT-3101 | clear datagrid based on payment method selected.
            If PaymentMethodType = PaymentMethod.CHECK Or PaymentMethodType = PaymentMethod.EFT Then
                'the selected index of the grid needs to be cleared
                DataGridPayment.SelectedIndex = -1
                'Amit Phase V Changes-end
            Else
                'the selected index of the grid needs to be cleared
                dgEFTDisbursements.SelectedIndex = -1
            End If
            ' END : SR | 2018.04.23 | YRS-AT-3101 | clear datagrid based on payment method selected.
        Catch ex As Exception
            Throw
        End Try
    End Function
    'Phase V Changes-End
#End Region

#Region "Events"
    ' START : SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
    'Private Sub DropDownListlDisbursementType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DropDownListlDisbursementType.SelectedIndexChanged
    Private Sub ddlCheckType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCheckType.SelectedIndexChanged
        ' END : SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
        Try

            '-------------------------------------------------------------------------------------------------------------
            'Start:Added by Shashi Shekhar:2009-11-26: To reset the view state
            ViewState("IsAllOtherExist") = "False"
            ViewState("IsHardExist") = "False"
            ViewState("IsDeathExist") = "False"
            '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
            ViewState("IsShiraExist") = "False"
            ViewState("IsDefaultExist") = "True"
            ViewState("IsDisbTypeChanged") = "True"
            ViewState("DisbursementType") = String.Empty
            ClearSessions()

            CheckBoxAllOtherWithdrawals.Checked = False
            CheckBoxDeathWithdrawals.Checked = False
            CheckBoxHardshipWithdrawals.Checked = False
            '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
            'Start : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
            'CheckBoxSHIRA.Checked = False
            'End : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
            SessionShiraWithdrawals = False
            'End :
            '------------------------------------------------------------------------------------------------------

            'Amit Phase V Changes-Start 
            Dim l_StringDisbursement As String
            'To get the replaced disbursement details from table
            'Amit Phase V Changes-End 
            'START: SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
            'l_StringDisbursement = DropDownListlDisbursementType.SelectedItem.Text.Trim.ToUpper
            l_StringDisbursement = ddlCheckType.SelectedItem.Text.Trim.ToUpper
            'END: SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
            l_StringDisbursement = l_StringDisbursement.PadRight(19, " ")

            'START: MMR | 2020.01.03 | YRS-AT-4601 | Setting key value to false if disbursement type is other than First annuity
            If l_StringDisbursement.Substring(0, 13).Trim().ToUpper <> "FIRST ANNUITY" Then
                Me.IsPaymentOutSourcingKeyON = False
            End If
            'END: MMR | 2020.01.03 | YRS-AT-4601 | Setting key value to false if disbursement type is other than First annuity

            'Assign  the check Numbers to the default.
            'START : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Change Annuity Payroll Option to First Annuity Payment
            'If l_StringDisbursement.Substring(0, 15).Trim().ToUpper = "ANNUITY PAYROLL" Then
            If l_StringDisbursement.Substring(0, 13).Trim().ToUpper = "FIRST ANNUITY" Then
                SetPaymentOutsourcingKey() ' MMR | 2020.01.03 | YRS-AT-4601 | Setting key value for first annuity Payment outsourcing.
                'END : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Change Annuity Payroll Option to First Annuity Payment
                LoadDataGrid()
                'Amit Phase V Changes-Start 
                '*****************************************modification done for the payment manager
                SetVisibleProperty(False)
                '*****************************************modification done for the payment manager
                Me.TextBoxCheckNoUs.Text = Me.SessionLongUSCheckNo.ToString.Trim()
                Me.TextBoxCheckNoCanadian.Text = Me.SessionLongCANADACheckNo.ToString.Trim()
            ElseIf l_StringDisbursement.Substring(0, 6).Trim().ToUpper = "REFUND" Then
                '*****************************************modification done for the payment manager
                ClearSessions()
                ButtonPrint.Enabled = False
                ButtonPay.Enabled = False
                DataGridPayment.DataSource = Nothing
                DataGridPayment.DataBind()
                CheckBoxAllOtherWithdrawals.Checked = False
                CheckBoxDeathWithdrawals.Checked = False
                CheckBoxHardshipWithdrawals.Checked = False
                '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                'Start : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
                'CheckBoxSHIRA.Checked = False
                CheckBoxSHIRA.Visible = False
                'End : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list

                SetVisibleProperty(True)
                '*****************************************modification done for the payment manager
                'Me.TextBoxCheckNoUs.Text = Me.SessionLongRefundCheckNo.ToString.Trim()
                'Me.TextBoxCheckNoCanadian.Text = Me.SessionLongCANADACheckNo.ToString.Trim()
            ElseIf l_StringDisbursement.Trim().ToUpper = "TAXDEFERRED LOAN" Then
                '*****************************************modification done for the payment manager
                LoadDataGrid()
                SetVisibleProperty(False)
                '*****************************************modification done for the payment manager
                Me.TextBoxCheckNoUs.Text = Me.SessionLongTDLoanCheckNo.ToString.Trim
                Me.TextBoxCheckNoCanadian.Text = Me.SessionLongCANADACheckNo.ToString.Trim()

            ElseIf l_StringDisbursement.Substring(0, 19).Trim().ToUpper = "EXPERIENCE DIVIDEND" Then
                '*****************************************modification done for the payment manager
                LoadDataGrid()
                SetVisibleProperty(False)
                '*****************************************modification done for the payment manager
                'Amit Phase V Changes-End 
                Me.TextBoxCheckNoUs.Text = Me.SessionLongEXPCheckNo.ToString.Trim
                Me.TextBoxCheckNoCanadian.Text = Me.SessionLongCANADACheckNo.ToString.Trim()

            ElseIf l_StringDisbursement.Substring(0, 19).Trim().ToUpper = "SAFE HARBOR" Then
                LoadDataGrid()
                SetVisibleProperty(False)
                'Code commented by dinesh for get the shira check series from database.
                'Issue reported by raj on 14/10/2013
                'Me.TextBoxCheckNoUs.Text = Me.SessionLongEXPCheckNo.ToString.Trim
                Me.TextBoxCheckNoUs.Text = Me.SessionLongSHIRAMCheckNo.ToString.Trim
                Me.TextBoxCheckNoCanadian.Text = Me.SessionLongCANADACheckNo.ToString.Trim()
            Else
                'Commented by shashi:2009-12-04: Not used now
                'Me.Session_datatable_DisbursementsALL_REPL = Nothing
                Me.Session_datatable_DisbursementsALL = Nothing
                Me.Session_datatable_Disbursements = Nothing
                LoadDataGrid()
                SetVisibleProperty(False)
            End If
            ' START : SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
            'If DropDownListlDisbursementType.SelectedIndex = 0 Or DropDownListlDisbursementType.SelectedIndex = 3 Then
            If ddlCheckType.SelectedIndex = 0 Or ddlCheckType.SelectedIndex = 3 Then
                ' END : SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
                ButtonFind.Enabled = False
                'Button1.Enabled = False 'VC | 2018.09.19 | YRS-AT-3101 -  Commented code to remove Button1
                'CheckBoxReplacedDisbursements.Enabled = False
                btnRecalculateEFTCheck.Disabled = True 'VC | 2018.09.19 | YRS-AT-3101 -  Disable recalculation button for CHECK and EFT
                btnConfirmEFTRecalculate.Disabled = True 'VC | 2018.09.19 | YRS-AT-3101 -  Disable recalculation button for CONFIRM EFT
            Else
                ButtonFind.Enabled = True
                'Button1.Enabled = True 'VC | 2018.09.19 | YRS-AT-3101 -  Commented code to remove Button1
                'CheckBoxReplacedDisbursements.Enabled = True
                btnRecalculateEFTCheck.Disabled = False 'VC | 2018.09.19 | YRS-AT-3101 -  Enable recalculation button for CHECK and EFT
                btnConfirmEFTRecalculate.Disabled = False 'VC | 2018.09.19 | YRS-AT-3101 -  Enable recalculation button for CONFIRM EFT
            End If
            'Me.PopulateData(LoadDatasetMode.Session)
            'Amit Phase V Changes-start
            'fundId No text bix should be cleared
            TextBoxFundIdNo.Text = String.Empty
            DataGridPayment.SelectedIndex = -1
            'Amit Phase V Changes-end

            'START : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Change Annuity Payroll Option to First Annuity Payment
            ResetControlsForFirstAnnuityPaymentOutsourcing()
            'END : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642| Change Annuity Payroll Option to First Annuity Payment
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    'START : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Reset Control in Case of First Annuity Payment 
    Private Sub ResetControlsForFirstAnnuityPaymentOutsourcing()
        Dim l_StringDisbursement As String
        Try
            l_StringDisbursement = ddlCheckType.SelectedItem.Text.Trim()
            l_StringDisbursement = l_StringDisbursement.PadRight(19, " ")
            If l_StringDisbursement.Substring(0, 13).Trim().ToUpper = "FIRST ANNUITY" AndAlso (IsPaymentOutSourcingKeyON) Then
                divFundNoAndRecalculate.Visible = True
                divCheckSeries.Visible = True
                ButtonPay.Text = "Generate file"
                spCurrency.Visible = True
                If (rbCurrencyUSA.Checked) Then
                    tblCheckSeries.Style("visibility ") = "hidden"
                ElseIf (rbCurrencyCanada.Checked) Then
                    tblCheckSeries.Style("visibility ") = "Inline"
                    ButtonPay.Text = "Pay"
                    divCheckSeries.Visible = True
                    RadioButtonUSA.Visible = False
                    LabelNextUSCheck.Visible = False
                    TextBoxCheckNoUs.Visible = False
                    RadioButtonCanada.Visible = False
                    LabelCanadianCheck.Visible = True
                    TextBoxCheckNoCanadian.Visible = True
                End If
            Else
                ButtonPay.Text = "Pay"
                spCurrency.Visible = False
                rbCurrencyUSA.Checked = True
                rbCurrencyCanada.Checked = False
                tblCheckSeries.Style("visibility ") = "Inline"
                divCheckSeries.Visible = True
                RadioButtonUSA.Visible = True
                LabelNextUSCheck.Visible = True
                TextBoxCheckNoUs.Visible = True
                RadioButtonCanada.Visible = True
                LabelCanadianCheck.Visible = True
                TextBoxCheckNoCanadian.Visible = True
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Sub rbCurrencyUSA_CheckedChanged(sender As Object, e As EventArgs) Handles rbCurrencyUSA.CheckedChanged
        'DistroyCashedData() 'ML | YRS-AT-4601 | 2019.12.03 | Change method name due to spelling mistake
        DistroyCachedData()
        Me.PopulateData()
        ResetControlsForFirstAnnuityPaymentOutsourcing()
    End Sub

    Private Sub rbCurrencyCanada_CheckedChanged(sender As Object, e As EventArgs) Handles rbCurrencyCanada.CheckedChanged
        'DistroyCashedData() 'ML | YRS-AT-4601 | 2019.12.03 | Change method name due to spelling mistake
        DistroyCachedData()
        Me.PopulateData()
        ResetControlsForFirstAnnuityPaymentOutsourcing()
    End Sub
    'END : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Reset Control in Case of First Annuity Payment 

    'START: VC | 2018.09.11 | YRS-AT-3101 |  Commented to remove ButtonSelectNone
    'Private Sub ButtonSelectNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSelectNone.Click

    '    Dim l_CheckBox As CheckBox
    '    Dim l_bool_flag As Boolean
    '    Dim intSelected As Integer
    '    Try
    '        If PaymentMethodType = PaymentMethod.CHECK Or PaymentMethodType = PaymentMethod.EFT Then 'SR | 2018.04.26 | YRS-AT-3101 | Confirming selected payment method, if CHECK then old code will run else new
    '            If Not Me.Session_datatable_DisbursementsALL Is Nothing Then
    '                If ButtonSelectNone.Text = "Select None" Then
    '                    l_bool_flag = False
    '                    intSelected = 0
    '                    ButtonSelectNone.Text = "Select All"
    '                Else
    '                    l_bool_flag = True
    '                    intSelected = 1
    '                    ButtonSelectNone.Text = "Select None"
    '                End If
    '                'PopulateData()
    '                g_datatable_Disbursements = Me.Session_datatable_DisbursementsALL
    '                For Each l_DataRow As DataRow In g_datatable_Disbursements.Rows
    '                    If Not (l_DataRow Is Nothing) Then
    '                        l_DataRow("Selected") = intSelected
    '                    End If
    '                Next


    '                'g_datatable_Disbursements.AcceptChanges()
    '                '' Me.SessionDataLoadMode = False
    '                ''BindData() 'Added by shashi:2009-11-28
    '                'BindDataintheGrid()'Commented by shashi:2009-11-28
    '                BindData()
    '                SetNetAmount()
    '                clearFundIdandGridIndex()
    '            End If
    '            ' START : SR | 2018.04.26 | YRS-AT-3101 | deselect disbursement record based on payment method type selected
    '        ElseIf PaymentMethodType = PaymentMethod.APPROVED_EFT Then
    '            SelectNoneEFTDisbursements()
    '        End If
    '        ' END : SR | 2018.04.26 | YRS-AT-3101 | deselect disbursement record based on payment method type selected
    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Sub
    'END: VC | 2018.09.11 | YRS-AT-3101  |  Commented to remove ButtonSelectNone

    Private Sub CheckBoxReplacedDisbursements_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxReplacedDisbursements.CheckedChanged
        Try
            'Session("MandCheck") = "True"
            'If Me.DropDownListlDisbursementType.SelectedItem.Text.Trim.Substring(0, 6).Trim().ToUpper = "REFUND" Then
            '    If CheckBoxAllOtherWithdrawals.Checked = False And CheckBoxHardshipWithdrawals.Checked = False And CheckBoxDeathWithdrawals.Checked = False Then
            '        MessageBox.Show(PlaceHolder1, "Pay", "Please select atleast one of the refund type to continue.", MessageBoxButtons.Stop)
            '        Session("MandCheck") = "False"
            '        Exit Sub
            '    End If
            'End If

            'START: SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
            'If Me.DropDownListlDisbursementType.SelectedItem.Text.Trim = "" Then
            If Me.ddlCheckType.SelectedItem.Text.Trim = "" Then

                'ElseIf Me.DropDownListlDisbursementType.SelectedItem.Text.Trim.Substring(0, 6).Trim().ToUpper = "REFUND" Then
            ElseIf Me.ddlCheckType.SelectedItem.Text.Trim.Substring(0, 6).Trim().ToUpper = "REFUND" Then
                'END: SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
                ' Me.SessionDataLoadMode = False
                'ButtonSelectNone.Text = "Select None" 'VC | 2018.09.11 | YRS-AT-3101  |  Commented to remove ButtonSelectNone
                HandleReplaceableCheckBox()
            Else
                Me.SessionDataLoadMode = False
                clearFundIdandGridIndex()
                LoadDataGrid()
            End If

            'Added by shashi:2009-11-28


            'previous code commented by shashi:2009-11-28
            '-----------------------------------------------------------------------------------
            'If CheckBoxReplacedDisbursements.Checked = True Then
            '    ' If Not Me.Session_datatable_DisbursementsALL_REPL Is Nothing Then
            '    Me.SessionDataLoadMode = False
            '    clearFundIdandGridIndex()
            '    If CheckBoxReplacedDisbursements.Checked = True Then
            '       Me.PopulateData(LoadDatasetMode.Session)
            '    Else
            '       Me.PopulateData(LoadDatasetMode.Table)
            '        ' End If
            '    End If
            'Else
            'If Not Me.Session_datatable_DisbursementsALL Is Nothing Then
            '    Me.SessionDataLoadMode = False
            '    clearFundIdandGridIndex()
            '    If CheckBoxReplacedDisbursements.Checked = True Then
            '           Me.PopulateData(LoadDatasetMode.Session)
            '        Else
            '           Me.PopulateData(LoadDatasetMode.Table)
            '        End If
            '    End If
            'End If
            '---------------------------------------------------------------------------------------

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub

    Private Sub CheckBoxManualCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxManualCheck.CheckedChanged
        Try

            '--------------------------------------------------------------------------
            'Commented by shashi:2009-12-01

            'If CheckBoxReplacedDisbursements.Checked = True Then
            '    If Not Me.Session_datatable_DisbursementsALL Is Nothing Then
            '        PopulateData(LoadDatasetMode.Table)
            '    End If
            'Else
            '    If Not Me.Session_datatable_DisbursementsALL Is Nothing Then
            '        PopulateData(LoadDatasetMode.Session)
            '    End If
            'End If
            '-------------------------------------------------------------------------------
            ' PopulateData() 'Added by shashi:2009-12-01
            PersistSelectionsFromGrid()
            SetValuesInControles()


            'PopulateGrid()
            'SetNetAmount()
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub ButtonPay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPay.Click
        Dim l_double_sumTotal As Double
        Dim bool_Mismatch As Boolean
        'START: VC | 2018.09.19 | YRS-AT-3101 -  Declared variables.
        Dim message As String
        Dim messageParameters As Dictionary(Of String, String)
        'END: VC | 2018.09.19 | YRS-AT-3101 -  Declared variables.
        Try

            'Added by neeraj on 18-Nov-2009 : issue is YRS 5.0-940
            Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If
            'End : YRS 5.0-940
            ' START | SR | 2018.04.13 | YRS-AT-3101 |  Perform pay button funtionality based on Payment method selected (EFT/CHECK/Approve_EFT).
            If (PaymentMethodType = PaymentMethod.CHECK) Then
                ' START : SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType. Also, check condition only if payment method code is "CHECK"
                'If Me.DropDownListlDisbursementType.SelectedItem.Text.Trim.Substring(0, 6).Trim().ToUpper = "REFUND" Then
                If Me.ddlCheckType.SelectedItem.Text.Trim.Substring(0, 6).Trim().ToUpper = "REFUND" Then ' SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType. Also, check condition only if payment method code is "CHECK"
                    ' END : SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType. Also, check condition only if payment method code is "CHECK"
                    '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    'Added one checkboxshira here for yrs 5.0-1489 requirement
                    'Start : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
                    'If CheckBoxAllOtherWithdrawals.Checked = False And CheckBoxHardshipWithdrawals.Checked = False And CheckBoxDeathWithdrawals.Checked = False AndAlso CheckBoxSHIRA.Checked = False Then
                    If CheckBoxAllOtherWithdrawals.Checked = False And CheckBoxHardshipWithdrawals.Checked = False And CheckBoxDeathWithdrawals.Checked = False Then
                        'End: Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
                        MessageBox.Show(PlaceHolder1, "Pay", "Please select atleast one of the refund type to continue.", MessageBoxButtons.Stop)
                        Exit Sub
                    End If

                    If CheckMismatchForRefunds() = True Then
                        MessageBox.Show(PlaceHolder1, "Pay", "The selection criteria have been changed. Please click Load button to refresh the data.", MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                End If

                If Not Me.Session_datatable_DisbursementsALL Is Nothing Then
                    ''g_bool_setAmount = False
                    ' PopulateData(LoadDatasetMode.Session)'Commented by shashi:2009-12-01
                    ''PopulateData() 'Added by shashi:2009-12-01
                    PersistSelectionsFromGrid()
                    ''g_bool_setAmount = True
                    If ValidateData() Then

                        l_double_sumTotal = SumFields(g_datatable_Disbursements, "total")

                        Me.SessionStartPay = True
                        'Me.StartApprovalProcess = False
                        If l_double_sumTotal <> Me.SessionDoubleTotalNetAmount Then
                            MessageBox.Show(PlaceHolder1, "Payment Manager.", "There is a mismatch on the Total Net Amount, Please verify. Click Recalculate button to avoid this mismatch before the PAY.", MessageBoxButtons.Stop)
                        ElseIf (Convert.ToDateTime(Me.TextBoxCheckDate.Text.Trim()) < Me.SessionAccountingDate.AddMonths(-12).Date()) Then
                            MessageBox.Show(PlaceHolder1, "YRS-DateCheck and Pay", "Check date is more than a year prior to current accounting period " + Me.SessionAccountingDate.ToShortDateString() + " Do you wish to continue and pay these disbursements?", MessageBoxButtons.YesNo)
                        Else
                            MessageBox.Show(PlaceHolder1, "Pay", "Are you sure you want to pay these disbursements?", MessageBoxButtons.YesNo)
                        End If
                    End If
                    ' END : SR | 2018.04.13 | YRS-AT-3101 |  based on disbursement type(EFT/CHECK), validate disbursement record(s)
                End If
            ElseIf (PaymentMethodType = PaymentMethod.EFT) Then
                If Not Me.Session_datatable_DisbursementsALL Is Nothing Then
                    PersistSelectionsFromGrid()

                    If ValidateEFTData() Then

                        l_double_sumTotal = SumFields(g_datatable_Disbursements, "total")

                        Me.SessionStartPay = True
                        'Me.StartApprovalProcess = False
                        If l_double_sumTotal <> Me.SessionDoubleTotalNetAmount Then
                            'START: SR | 2018.10.29 | YRS-AT-3101 | Commented existing code and added new code to show warning messages from database driven messages.                  
                            'MessageBox.Show(PlaceHolder1, "Payment Manager.", "There is a mismatch on the Total Net Amount, Please verify. Click Recalculate button to avoid this mismatch before the PAY.", MessageBoxButtons.Stop)
                            MessageBox.Show(PlaceHolder1, "Payment Manager.", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_MISMATCH_RECALCULATION), MessageBoxButtons.Stop)
                            'END: SR | 2018.10.29 | YRS-AT-3101 | Commented existing code and added new code to show Confirmation and warning messages from database driven messages.                  
                        Else
                            'START: VC | 2018.11.16 | YRS-AT-3101 | Commented existing hard coded message and added new code to show message from AtsMetaMessages table                 
                            'MessageBox.Show(PlaceHolder1, "Pay", "Are you sure you want to generate EFT File for these disbursements?", MessageBoxButtons.YesNo)
                            MessageBox.Show(PlaceHolder1, "Pay", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_LOAN_EFT_CONFIRM_GENERATE_EFT), MessageBoxButtons.YesNo)
                            'END: VC | 2018.11.16 | YRS-AT-3101 | Commented existing hard coded message and added new code to show message from AtsMetaMessages table    
                        End If
                    End If
                End If
            ElseIf (PaymentMethodType = PaymentMethod.APPROVED_EFT) Then
                If Not Me.EFTDisbursementsPendingForApproval Is Nothing Then
                    PersistEFTDisbursementSelectionsFromGrid()

                    'If ValidateEFTData() Then


                    '    l_double_sumTotal = SumFields(EFTDisbursementsPendingForApproval, "NetAmount")

                    Me.SessionStartPay = False
                    'Me.StartApprovalProcess = True
                    '    If l_double_sumTotal <> Me.SessionDoubleTotalNetAmount Then
                    '        MessageBox.Show(PlaceHolder1, "Payment Manager.", "There is a mismatch on the Total Net Amount, Please verify. Click Recalculate button to avoid this mismatch before the PAY.", MessageBoxButtons.Stop)
                    '    Else
                    'MessageBox.Show(PlaceHolder1, "Pay", String.Format("Are you sure you want to proceed with Approval/Rejection <br /> Approval Status: {0} Records <br /> Rejection Status: {1} Records", ApprovalEFTCount, RejectionEFTCount), MessageBoxButtons.YesNo)
                    'START: VC | 2018.09.19 | YRS-AT-3101 -  Commented existing code and added new code to show Confirmation and warning messages.                  
                    l_double_sumTotal = SumFields(EFTDisbursementsPendingForApproval, "NetAmount")
                    'If calculated net amount not matching with session value then show message
                    If l_double_sumTotal <> Me.SessionDoubleTotalNetAmount Then
                        message = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_MISMATCH_RECALCULATION)
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", "OpenMismatchConfirmationDialog('" + message + "');", True)
                    Else
                        'If calculated net amount is matching with session value then show confirmation message
                        messageParameters = New Dictionary(Of String, String)()
                        messageParameters.Add("NumberOfRecords", ApprovalEFTCount.ToString())
                        message = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_CONFIRM_APPROVE_DISBURSEMENT, messageParameters)
                        ApproveConfirmation = message
                        Page.ClientScript.RegisterHiddenField("hfApproveConfirmation", ApproveConfirmation.ToString())
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", "OpenEFTConfirmationDialog();", True)
                    End If
                    'END: VC | 2018.09.19 | YRS-AT-3101 -  Commented existing code and added new code to show Confirmation and warning messages.
                    '    End If
                    'End If
                End If
            End If
            ' END | SR | 2018.04.13 | YRS-AT-3101 |  Perform pay button funtionality based on Payment method selected (EFT/CHECK/Approve_EFT).
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    'Added for Gemini Ticket 786 Amit
    Private Function CheckMismatchForRefunds()

        '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
        'Added  (Me.SessionShiraWithdrawals = CheckBoxSHIRA.Checked) for gemini yrs 5.0-1489 
        'Remove (Me.SessionShiraWithdrawals = CheckBoxSHIRA.Checked) for gemini yrs 5.0-1755
        If ((Me.SessionAllOtherWithdrawals = CheckBoxAllOtherWithdrawals.Checked) And _
           (Me.SessionHardWithdrawals = CheckBoxHardshipWithdrawals.Checked) And
          (Me.SessionDeathWithdrawals = CheckBoxDeathWithdrawals.Checked)) Then
            CheckMismatchForRefunds = False
        Else
            CheckMismatchForRefunds = True
        End If

    End Function
    'Added for Gemini Ticket 786 Amit

    Private Sub ButtonDeselectErroneousDisbursements_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDeselectErroneousDisbursements.Click
        Try
            'PopulateData(LoadDatasetMode.Session)'Commented by shashi:2009-12-01
            ''PopulateData() 'Added by shashi:2009-12-01
            PersistSelectionsFromGrid()

            deselecterroneousDisbursement()

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            ClearSessions()
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    Private Sub CheckBoxProcessAllSelectedDisbursements_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxProcessAllSelectedDisbursements.CheckedChanged
        Try
            ' PopulateGrid()'Commented by shashi:2009-11-28: Not used this checkbox in appl.
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try

    End Sub
    'This is click of Recalculate button click event
    'START: VC | 2018.09.19 | YRS-AT-3101 -  Commented code to remove Button1
    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Try
    '        ' Recalculate total amount based on payment method type. Check as well as EFT payment method type uses same data grid to display. hence same code can be used.
    '        If (PaymentMethodType = PaymentMethod.CHECK Or PaymentMethodType = PaymentMethod.EFT) Then 'SR | 2018.04.26 | YRS-AT-3101 | Maintaing EFT disbursement selection
    '            'PopulateData(LoadDatasetMode.Session)'Commented by shashi:2009-12-01
    '            ''PopulateData() 'Added by shashi:2009-12-01
    '            PersistSelectionsFromGrid()
    '            'PopulateGrid()
    '            'START: SR | 2018.04.26 | YRS-AT-3101 | Maintaing EFT disbursement selection
    '        ElseIf PaymentMethodType = PaymentMethod.APPROVED_EFT Then
    '            PersistEFTDisbursementSelectionsFromGrid()
    '        End If
    '        'END: SR | 2018.04.26 | YRS-AT-3101 | Maintaing EFT disbursement selection
    '        SetNetAmount()
    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Sub
    'END: VC | 2018.09.19 | YRS-AT-3101 -  Commented code to remove Button1

    Private Sub ButtonPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPrint.Click
        Dim l_StringDisbursement As String
        Dim l_String_ReportName As String
        Dim bool_mismatchPrint As Boolean

        Try

            ' START | SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
            'If Me.DropDownListlDisbursementType.SelectedItem.Text.Trim.Substring(0, 6).Trim().ToUpper = "REFUND" Then
            If Me.PaymentMethodType = PaymentMethod.CHECK AndAlso Me.ddlCheckType.SelectedItem.Text.Trim.Substring(0, 6).Trim().ToUpper = "REFUND" Then
                ' END | SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
                '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                'Added checkboxshira in if condition for gemini requirement yrs 5.0-1489
                'If CheckBoxAllOtherWithdrawals.Checked = False And CheckBoxHardshipWithdrawals.Checked = False And CheckBoxDeathWithdrawals.Checked = False AndAlso CheckBoxSHIRA.Checked = False Then
                'Remove code Checkboxshira in if condition for gemini requirement yrs 5.0-1755: By Dinesh Kanojia on 20/05/0213
                If CheckBoxAllOtherWithdrawals.Checked = False And CheckBoxHardshipWithdrawals.Checked = False And CheckBoxDeathWithdrawals.Checked = False Then
                    MessageBox.Show(PlaceHolder1, "Pay", "Please select atleast one of the refund type to continue.", MessageBoxButtons.Stop)
                    Exit Sub
                End If

                If CheckMismatchForRefunds() = True Then
                    MessageBox.Show(PlaceHolder1, "Pay", "The selection criteria have been changed. Please click Load button to refresh the data.", MessageBoxButtons.Stop)
                    Exit Sub
                End If
            End If


            ' PopulateData(LoadDatasetMode.Session) 'Commented by shashi:2009-12-01
            'PopulateData() 'Added by shashi:2009-12-01

            ' START : SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
            'l_StringDisbursement = DropDownListlDisbursementType.SelectedItem.Text.Trim()
            If Me.PaymentMethodType = PaymentMethod.CHECK Then
                l_StringDisbursement = ddlCheckType.SelectedItem.Text.Trim()
            ElseIf Me.PaymentMethodType = PaymentMethod.EFT Or Me.PaymentMethodType = PaymentMethod.APPROVED_EFT Then
                l_StringDisbursement = ddlEFTType.SelectedItem.Text.Trim()
            End If
            ' END : SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType

            'Aparna
            ' l_StringDisbursement = l_StringDisbursement.PadRight(16, " ")
            l_StringDisbursement = l_StringDisbursement.PadRight(19, " ")
            'Aparna
            'START : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Change Annuity Payroll Option to First Annuity Payment
            'If l_StringDisbursement.Substring(0, 15).Trim().ToUpper = "ANNUITY PAYROLL" Then
            If l_StringDisbursement.Substring(0, 13).Trim().ToUpper = "FIRST ANNUITY" Then
                'END : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Change Annuity Payroll Option to First Annuity Payment
                l_String_ReportName = "First Annuity Checks Request"
                Session("DisbType") = Nothing
            ElseIf l_StringDisbursement.Substring(0, 6).Trim().ToUpper = "REFUND" Then
                l_String_ReportName = "Disbursement Request"
                Session("DisbType") = "REF"
            ElseIf l_StringDisbursement.Trim.ToUpper = "TAXDEFERRED LOAN" Then
                l_String_ReportName = "Disbursement Request"
                Session("DisbType") = "TDLoan"
                'Start : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
            ElseIf l_StringDisbursement.Substring(0, 19).Trim().ToUpper = "SAFE HARBOR" Then
                l_String_ReportName = "Disbursement Request"
                Session("DisbType") = "REF"
                'End : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
                '2010-10-04:Shashi Shekhar Singh: For BT- 579
            ElseIf l_StringDisbursement.Substring(0, 19).Trim().ToUpper = "EXPERIENCE DIVIDEND" Then
                'l_String_ReportName = "Disbursement Request"
                'Session("DisbType") = "EXP"
                l_String_ReportName = ""
                MessageBox.Show(PlaceHolder1, "Print disbursements", "Please Select Annuity Payroll (A) from Disbursement type Dropdownlist to view the merge report of Annuity Payroll and Experience Dividend", MessageBoxButtons.OK)
            Else
                'showMessage('Please Select a disbursement type.',0,'Print disbursements')
                l_String_ReportName = ""
                'START: VC | 2018.11.16 | YRS-AT-3101 | Commented existing hard coded message and added new code to show message from AtsMetaMessages table                 
                'MessageBox.Show(PlaceHolder1, "Print disbursements", "Please Select a disbursement type", MessageBoxButtons.Stop)
                MessageBox.Show(PlaceHolder1, "Print disbursements", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_LOAN_EFT_SELECT_DISBURSEMENT_TYPE), MessageBoxButtons.Stop)
                'END: VC | 2018.11.16 | YRS-AT-3101 | Commented existing hard coded message and added new code to show message from AtsMetaMessages table                 
            End If

            'START : SR | 2018.04.25 | YRS-AT-3101 | New parameter added to report to display disbursements based on payment method type. 

            ' If payment method type is APPROVED_EFT then call different report.
            If Me.PaymentMethodType = PaymentMethod.CHECK Then
                Session("PaymentMethodCode") = PaymentMethod.CHECK
            ElseIf Me.PaymentMethodType = PaymentMethod.EFT Then
                l_String_ReportName = "PendingEFTDisbursements"
            ElseIf Me.PaymentMethodType = PaymentMethod.APPROVED_EFT Then
                l_String_ReportName = "ConfirmEFTDisbursements"
            End If
            'END : SR | 2018.04.25 | YRS-AT-3101 | New parameter added to report to display disbursements based on payment method type. 

            If l_String_ReportName <> "" Then
                ' Set Value and Call the report viewer.
                Session("strReportName") = l_String_ReportName

                Dim popupScript As String = "<script language='javascript'>" & _
                "window.open('FT\\ReportViewer.aspx', 'ReportPopUp', " & _
                "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                "</script>"
                If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                    Page.RegisterStartupScript("PopupScript1", popupScript)
                End If

                'PopulateData(LoadDatasetMode.Cache)

            End If

            'PopulateGrid()
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    'START : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642| Change CurrencyCode Description
    Private Sub DataGridPayment_ItemDataBound(sender As Object, e As DataGridItemEventArgs) Handles DataGridPayment.ItemDataBound
        Dim idxCurrencyCode As Integer
        Dim currencyCode As String
        If e.Item.ItemType <> ListItemType.Header Then
            idxCurrencyCode = 8
            currencyCode = e.Item.Cells(idxCurrencyCode).Text
            If currencyCode = "U" Then
                e.Item.Cells(idxCurrencyCode).Text = "USD"
            End If
            If currencyCode = "C" Then
                e.Item.Cells(idxCurrencyCode).Text = "CAD"
            End If

        End If
    End Sub
    'START : ML| 2019.10.10 | YRS-AT-4601,YRS-AT-4642 | Change CurrencyCode Description

    Private Sub DataGridPayment_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridPayment.SortCommand
        Try
            Dim dv As New DataView
            Dim SortExpression As String
            SortExpression = e.SortExpression
            Me.SessionDataLoadMode = True
            'Me.PopulateData()
            PersistSelectionsFromGrid()

            dv = g_datatable_Disbursements.DefaultView
            dv.Sort = SortExpression

            If Not Session("DisbursementsListSort") Is Nothing Then
                If SortExpression + " ASC" = Session("DisbursementsListSort").ToString.Trim Then
                    dv.Sort = SortExpression + " DESC"
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
            Else
                dv.Sort = SortExpression + " ASC"
            End If
            Session("DisbursementsListSort") = dv.Sort
            'Me.SessionSelectedUniqueID = ""
            BindData() 'Added by shashi:2009-11-28
            'Me.PopulateGrid()'Commented by shashi:2009-11-28

            'Me.PopulateData(LoadDatasetMode.Session)
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub

    'Amit Phase V Changes-Start April 08,2009
    Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Dim messageParameters = New Dictionary(Of String, String)() 'VC | 2018.11.16 | YRS-AT-3101 - Declared new variable  
        Try
            If PaymentMethodType = PaymentMethod.CHECK Or PaymentMethodType = PaymentMethod.EFT Then 'SR | 2018.04.23 | YRS-AT-3101 | If payment method is either CHECK or EFT then search in DataGridPayment else search in dgEFTDisbursements grid.
                If Me.DataGridPayment.Items.Count > 0 Then
                    Dim l_dataTable As DataTable
                    Dim l_double_sumTotal As Double
                    Dim l_CheckBox As CheckBox
                    Dim l_FundIdValid As Integer = 0
                    Dim nfi As New CultureInfo("en-US", False)
                    'PopulateData(LoadDatasetMode.Session)'Commented by shashi:2009-12-01
                    PersistSelectionsFromGrid() 'Added by shashi:2009-12-07

                    nfi.NumberFormat.CurrencyGroupSeparator = ","
                    nfi.NumberFormat.CurrencySymbol = "$"
                    nfi.NumberFormat.CurrencyDecimalDigits = 2

                    '-----------------------------------------------------------------------------------------------------------
                    'Commented by Shashi:2009-12-04: Now only one table is used i.e Me.Session_datatable_DisbursementsALL
                    'If CheckBoxReplacedDisbursements.Checked = True Then
                    '    l_dataTable = Me.Session_datatable_DisbursementsALL_REPL
                    'Else
                    l_dataTable = Me.Session_datatable_DisbursementsALL
                    ' End If
                    '-------------------------------------------------------------------------------------------

                    If TextBoxFundIdNo.Text = String.Empty Then
                        DataGridPayment.DataSource = l_dataTable
                        DataGridPayment.DataBind()
                    Else

                        For l_DataRow As Integer = 0 To DataGridPayment.Items.Count - 1
                            If DataGridPayment.Items(l_DataRow).Cells(1).Text = TextBoxFundIdNo.Text Then
                                'DataGridPayment.SelectedIndex = l_DataRow
                                DataGridPayment.Items(l_DataRow).BackColor = System.Drawing.Color.LightBlue
                                l_FundIdValid = 1
                            End If
                        Next
                        If l_FundIdValid = 1 Then
                            For Each l_DataGridItem As DataGridItem In Me.DataGridPayment.Items
                                l_CheckBox = l_DataGridItem.FindControl("CheckBoxSelect")
                                If l_DataGridItem.Cells(1).Text = TextBoxFundIdNo.Text Then
                                    l_CheckBox.Checked = True
                                    'Added to Register the Javascript for setting the focus in the grid
                                    SetFocus(l_CheckBox)
                                    'Added to Register the Javascript for setting the focus in the grid
                                End If
                                If l_CheckBox.Checked = True Then
                                    l_double_sumTotal += l_DataGridItem.Cells(6).Text
                                End If
                            Next

                            Me.TextBoxNetAmount.Text = System.Convert.ToString(l_double_sumTotal.ToString("C", nfi)).Trim()
                            Me.SessionDoubleTotalNetAmount = l_double_sumTotal
                        Else
                            'START: VC | 2018.11.16 | YRS-AT-3101 | Commented existing hard coded message and added new code to show message from AtsMetaMessages table                 
                            'MessageBox.Show(PlaceHolder1, "Invalid Fund Id", "Fund Id " + TextBoxFundIdNo.Text + " not found ", MessageBoxButtons.OK)
                            messageParameters.Add("FundId", TextBoxFundIdNo.Text)
                            MessageBox.Show(PlaceHolder1, "Invalid Fund Id", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_LOAN_EFT_FUNDID_NOTFOUND, messageParameters), MessageBoxButtons.OK)
                            'END: VC | 2018.11.16 | YRS-AT-3101 | Commented existing hard coded message and added new code to show message from AtsMetaMessages table                 
                            TextBoxFundIdNo.Text = String.Empty
                        End If

                    End If

                End If
                'START: SR | 2018.04.23 | YRS-AT-3101 | If payment method is either CHECK or EFT then search in DataGridPayment else search in dgEFTDisbursements grid.
            ElseIf PaymentMethodType = PaymentMethod.APPROVED_EFT Then
                FindEFTDisbursement()
            End If
            'END: SR | 2018.04.23 | YRS-AT-3101 | If payment method is either CHECK or EFT then search in DataGridPayment else search in dgEFTDisbursements grid.
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    'Added to Register the Javascript for setting the focus in the grid
    Private Sub SetFocus(ByVal ctrl As System.Web.UI.Control)
        Try
            Dim s As String = "<SCRIPT language='javascript'>document.getElementById('" & ctrl.ClientID & "').focus() </SCRIPT>"
            Page.RegisterStartupScript("focus", s)
        Catch ex As Exception
            Throw
        End Try
    End Sub
    'Commente the event as it is not required as par the change request regarding gemini issue- 786 Amit
    'Private Sub RadioButtonListRefundSelect_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonListRefundSelect.SelectedIndexChanged
    '    Try
    '        Me.TextBoxCheckNoUs.Text = Me.SessionLongRefundCheckNo.ToString.Trim()
    '        Me.TextBoxCheckNoCanadian.Text = Me.SessionLongCANADACheckNo.ToString.Trim()
    '        ButtonFind.Enabled = True
    '        Button1.Enabled = True
    '        CheckBoxReplacedDisbursements.Enabled = True
    '        clearFundIdandGridIndex()
    '        LoadDataGrid()
    '        SetValuesInControles()

    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Sub

    'Phase V Changes-End April 08,2009
#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    'Added for Gemini Ticket 786 Amit
    'Private Sub CheckBoxAllOtherWithdrawals_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxAllOtherWithdrawals.CheckedChanged
    '    Try

    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Sub

    'Private Sub CheckBoxDeathWithdrawals_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxDeathWithdrawals.CheckedChanged
    '    Try

    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Sub

    'Private Sub CheckBoxHardshipWithdrawals_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxHardshipWithdrawals.CheckedChanged
    '    Try

    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Sub
    Private Function CheckboxParameterValues() As Boolean
        Try
            l_string_checkRefundWithDrawalAll = String.Empty
            l_string_checkRefundWithDrawalHard = String.Empty
            l_string_checkRefundWithDrawalDeath = String.Empty
            l_string_checkRefundWithDrawalShira = String.Empty
            If CheckBoxAllOtherWithdrawals.Checked = True Then
                l_string_checkRefundWithDrawalAll = "ALL"
            End If
            If CheckBoxDeathWithdrawals.Checked = True Then
                l_string_checkRefundWithDrawalDeath = "DEATH"
            End If
            If CheckBoxHardshipWithdrawals.Checked = True Then
                l_string_checkRefundWithDrawalHard = "HARD"
            End If

            '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

            'START: Dinesh Kanojia 20/05/2013:BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
            'If CheckBoxSHIRA.Checked = True Then
            '	l_string_checkRefundWithDrawalShira = "SHIRA"
            'End If
            'END: Dinesh Kanojia 20/05/2013:BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list

            'End '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function onRefundCheckBoxCheckChanged() As Boolean

        Try
            Me.TextBoxCheckNoUs.Text = Me.SessionLongRefundCheckNo.ToString.Trim()
            'Remove Unwanted Code for safe hatbor.
            'If Me.SessionShiraWithdrawals = True Then
            '    Me.TextBoxCheckNoUs.Text = Me.SessionLongSHIRAMCheckNo.ToString.Trim()
            'End If

            Me.TextBoxCheckNoCanadian.Text = Me.SessionLongCANADACheckNo.ToString.Trim()
            ButtonFind.Enabled = True
            'Button1.Enabled = True'VC | 2018.09.19 | YRS-AT-3101 -  Commented code to remove Button1
            btnRecalculateEFTCheck.Disabled = False ' VC | 2018.09.19 | YRS-AT-3101 -  Disable recalculate button for CHECK and EFT
            CheckBoxReplacedDisbursements.Enabled = True
            clearFundIdandGridIndex()
            LoadDataGrid()
            SetValuesInControles()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub ButtonLoadRefund_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonLoadRefund.Click
        Try
            '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
            'Added checkboxshira in if condition ffor yrs 5.0-1489
            'Remove checkboxshira in if condition ffor yrs 5.0-1755: By Dinesh Kanojia on: 20/05/2013
            'If CheckBoxAllOtherWithdrawals.Checked = True Or CheckBoxDeathWithdrawals.Checked = True Or CheckBoxHardshipWithdrawals.Checked = True Or CheckBoxSHIRA.Checked = True Then
            If CheckBoxAllOtherWithdrawals.Checked = True Or CheckBoxDeathWithdrawals.Checked = True Or CheckBoxHardshipWithdrawals.Checked = True Then

                Me.SessionAllOtherWithdrawals = CheckBoxAllOtherWithdrawals.Checked
                Me.SessionDeathWithdrawals = CheckBoxDeathWithdrawals.Checked
                Me.SessionHardWithdrawals = CheckBoxHardshipWithdrawals.Checked
                '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                'Added shira for gmini yrs 5.0-2489
                'Commented By Dinesh Kanojia on 20/05/2013:BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
                'Me.SessionShiraWithdrawals = CheckBoxSHIRA.Checked

                CheckboxParameterValues()
                onRefundCheckBoxCheckChanged()
                'Start : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
                ' START | SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
                'ElseIf DropDownListlDisbursementType.SelectedItem.Text.Trim.ToUpper() = "SAFE HARBOR" Then
            ElseIf ddlCheckType.SelectedItem.Text.Trim.ToUpper() = "SAFE HARBOR" Then
                ' END | SR | 2018.04.10 | YRS-AT-3101 | renamed from DropDownListlDisbursementType to ddlCheckType
                Me.SessionShiraWithdrawals = True
                CheckboxParameterValues()
                onRefundCheckBoxCheckChanged()
                'End : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
            Else
                Me.sessionboolLoadDisbursement = True
                MessageBox.Show(PlaceHolder1, "Payment Manager.", "Please select the Refund type.", MessageBoxButtons.Stop)
            End If

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    'Added for Gemini Ticket 786 Amit

    'Shashi Shekhar:2009-11-25: To get the filtered datatable
    Public Function GetFilteredDataTable(ByVal dTable As DataTable, ByVal expr As String) As DataTable

        Dim l_disbtable As DataTable
        l_disbtable = dTable.Clone

        Try

            Dim dv As DataView = dTable.DefaultView
            dv.RowFilter = expr
            Dim i As Integer
            For i = 0 To dv.Count - 1
                l_disbtable.ImportRow(dv.Item(i).Row)
            Next
            Return l_disbtable

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        Finally
            l_disbtable = Nothing
            expr = Nothing
        End Try

    End Function


    'Added by shashi:2009-12-02

    'To check all enabled records in grid

    Private Function PrepareExpression() As String
        Dim expr As String = String.Empty

        Try
            '--------------------------------------------------------------------
            Dim l_StringDisbursementType As String

            ' START : SR | 2018.04.11 | YRS-AT-3101 | Select disbursement type based on Payment Method.
            'l_StringDisbursementType = DropDownListlDisbursementType.SelectedItem.Text.Trim()
            'START :ML |2019.11.04 | YRS-AT-4601 |Change Radiobuttonlist to Radiobutton individual
            'If (rblDisbursementType.SelectedValue = PaymentMethod.EFT Or rblDisbursementType.SelectedValue = PaymentMethod.APPROVED_EFT) Then
            If (rbDisbursementTypeEFT.Checked = True Or rbDisbursementTypeAPPROVEDEFT.Checked = True) Then
                'END :ML |2019.11.04 | YRS-AT-4601 |Change Radiobuttonlist to Radiobutton individual
                l_StringDisbursementType = ddlEFTType.SelectedItem.Text.Trim()
            Else
                l_StringDisbursementType = ddlCheckType.SelectedItem.Text.Trim()
            End If
            ' END : SR | 2018.04.11 | YRS-AT-3101 | Select disbursement type based on Payment Method.

            l_StringDisbursementType = l_StringDisbursementType.PadRight(19, " ")
            If l_StringDisbursementType.Substring(0, 6).Trim().ToUpper = "REFUND" Then
                l_StringDisbursementType = "REF"
                'Start : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
            ElseIf l_StringDisbursementType.Substring(0, 19).Trim().ToUpper = "SAFE HARBOR" Then
                l_StringDisbursementType = "REF"
                'End : Dinesh Kanojia: 20/05/2013 :BT:-1536: YRS 5.0-1755:Move Safe Harbor checkbox to dropdown list
            End If

            '------------------------------------------------------------------------------

            ' If Me.CheckBoxReplacedDisbursements.Checked = False Then expr = "Replaceable=False"


            If l_StringDisbursementType = "REF" Then

                Dim l_refundtype As String = String.Empty
                If (l_string_checkRefundWithDrawalAll <> String.Empty) Then
                    l_refundtype = ",3"
                End If
                If (l_string_checkRefundWithDrawalDeath <> String.Empty) Then
                    l_refundtype = l_refundtype + ",2"
                End If
                '28.09.2012		BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                'Remove last ","(after 1) from retfundtype of hard
                If (l_string_checkRefundWithDrawalHard <> String.Empty) Then
                    l_refundtype = l_refundtype + ",1"
                End If
                '28.09.2012		BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                If (l_string_checkRefundWithDrawalShira <> String.Empty) Then
                    l_refundtype = l_refundtype + ",4,"
                End If
                If (l_refundtype <> String.Empty) Then
                    l_refundtype = l_refundtype.Remove(0, 1)
                    If l_refundtype.Substring(l_refundtype.Length - 1) = "," Then
                        l_refundtype = l_refundtype.Remove(l_refundtype.Length - 1, 1)
                    End If

                    If expr = String.Empty Then
                        expr = expr + "RefundType in (" + l_refundtype + ")"
                        'Else
                        '    expr = expr + " AND RefundType in (" + l_refundtype + ")"
                    End If

                End If

            End If
            Return expr

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        Finally
            expr = Nothing
        End Try

    End Function

    'Added by shashi:2009-12-02:To distroy the cashed data
    'START : ML | YRS-AT-4601 | 2019.12.03 | Change method name due to spelling mistake
    ' Private Function DistroyCashedData()
    Private Function DistroyCachedData()
        'END : ML | YRS-AT-4601 | 2019.12.03 | Change method name due to spelling mistake
        Try
            ViewState("IsAllOtherExist") = String.Empty
            ViewState("IsHardExist") = String.Empty
            ViewState("IsDeathExist") = String.Empty
            '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
            ViewState("IsShiraExist") = String.Empty
            ViewState("DisbursementType") = String.Empty
            ClearSessions()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Function

    'Added by shashi:2009-12-03
#Region "Public Functions "
    'To remove duplicate row from datatable
    Public Function RemoveDuplicateRows(ByVal dTable As DataTable, ByVal colName As String) As DataTable
        Dim l_hTable As Hashtable
        Dim l_duplicateList As ArrayList

        Try
            l_hTable = New Hashtable
            l_duplicateList = New ArrayList
            Dim drow As DataRow
            For Each drow In dTable.Rows
                If l_hTable.Contains(drow(colName)) Then
                    l_duplicateList.Add(drow)
                Else
                    l_hTable.Add(drow(colName), String.Empty)
                End If
            Next
            Dim dRow1 As DataRow
            For Each dRow1 In l_duplicateList
                dTable.Rows.Remove(dRow1)
            Next
            Return dTable


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        Finally
            l_duplicateList = Nothing
            l_hTable = Nothing
        End Try

    End Function
#End Region

#Region "EFT Payment"
    ' START | SR | 2018.04.10 | YRS-AT-3101 | Based on selected disbursement option(EFT/CHECK/Approved EFT) change disbursement type.
    'START :ML |2019.11.04 | YRS-AT-4601 
    'Private Sub rblDisbursementType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblDisbursementType.SelectedIndexChanged
    '    If rblDisbursementType.SelectedValue = PaymentMethod.EFT Then
    '        ResetControls(PaymentMethod.EFT)
    '    ElseIf rblDisbursementType.SelectedValue = PaymentMethod.CHECK Then
    '        ResetControls(PaymentMethod.CHECK)
    '    ElseIf rblDisbursementType.SelectedValue = PaymentMethod.APPROVED_EFT Then
    '        ResetControls(PaymentMethod.APPROVED_EFT)
    '    End If
    '    SetWithdrawalControls(False)
    '    ClearSessions()
    '    ClearDatagrid()
    '    ButtonPrint.Enabled = False  ' SR | 2018.10.24 | YRS-AT-3101 - Bydefault Print button will be disbaled.
    '    ButtonPay.Enabled = False ' SR | 2018.10.24 | YRS-AT-3101 - Bydefault pay button will be disbaled.
    '    TextBoxNetAmount.Text = String.Empty  ' SR | 2018.10.24 | YRS-AT-3101 - Renamed from TextBoxboxNetAmount to TextBoxNetAmount.
    '    SetNetAmount()  ' SR | YRS-AT-3101 | 2018-11-06 | Reset total net amount text box
    '    ApprovalEFTCount = "0"
    '    RejectionEFTCount = "0"
    'End Sub

    Private Sub rbDisbursementTypeEFT_CheckedChanged(sender As Object, e As EventArgs) Handles rbDisbursementTypeEFT.CheckedChanged
        ResetControls(PaymentMethod.EFT)
        SetWithdrawalControls(False)
        ClearSessions()
        ClearDatagrid()
        ButtonPrint.Enabled = False  ' SR | 2018.10.24 | YRS-AT-3101 - Bydefault Print button will be disbaled.
        ButtonPay.Enabled = False ' SR | 2018.10.24 | YRS-AT-3101 - Bydefault pay button will be disbaled.
        TextBoxNetAmount.Text = String.Empty  ' SR | 2018.10.24 | YRS-AT-3101 - Renamed from TextBoxboxNetAmount to TextBoxNetAmount.
        SetNetAmount()  ' SR | YRS-AT-3101 | 2018-11-06 | Reset total net amount text box
        ApprovalEFTCount = "0"
        RejectionEFTCount = "0"
    End Sub
    Private Sub rbDisbursementTypeCHECK_CheckedChanged(sender As Object, e As EventArgs) Handles rbDisbursementTypeCHECK.CheckedChanged
        ResetControls(PaymentMethod.CHECK)
        SetWithdrawalControls(False)
        ClearSessions()
        ClearDatagrid()
        ButtonPrint.Enabled = False  ' SR | 2018.10.24 | YRS-AT-3101 - Bydefault Print button will be disbaled.
        ButtonPay.Enabled = False ' SR | 2018.10.24 | YRS-AT-3101 - Bydefault pay button will be disbaled.
        TextBoxNetAmount.Text = String.Empty  ' SR | 2018.10.24 | YRS-AT-3101 - Renamed from TextBoxboxNetAmount to TextBoxNetAmount.
        SetNetAmount()  ' SR | YRS-AT-3101 | 2018-11-06 | Reset total net amount text box
        ApprovalEFTCount = "0"
        RejectionEFTCount = "0"
    End Sub

    Private Sub rbDisbursementTypeAPPROVEDEFT_CheckedChanged(sender As Object, e As EventArgs) Handles rbDisbursementTypeAPPROVEDEFT.CheckedChanged
        ResetControls(PaymentMethod.APPROVED_EFT)
        SetWithdrawalControls(False)
        ClearSessions()
        ClearDatagrid()
        ButtonPrint.Enabled = False  ' SR | 2018.10.24 | YRS-AT-3101 - Bydefault Print button will be disbaled.
        ButtonPay.Enabled = False ' SR | 2018.10.24 | YRS-AT-3101 - Bydefault pay button will be disbaled.
        TextBoxNetAmount.Text = String.Empty  ' SR | 2018.10.24 | YRS-AT-3101 - Renamed from TextBoxboxNetAmount to TextBoxNetAmount.
        SetNetAmount()  ' SR | YRS-AT-3101 | 2018-11-06 | Reset total net amount text box
        ApprovalEFTCount = "0"
        RejectionEFTCount = "0"
    End Sub
    'END :ML |2019.11.04 | YRS-AT-4601 |Change Radiobuttonlist to Radiobutton individual 
    ' Reset form controls based on Payment method selected.
    Private Sub ResetControls(ByVal Value As String)
        spCurrency.Visible = False 'ML |2019.11.04 | YRS-AT-4601 |Handle
        If Value = PaymentMethod.EFT Then
            tblWithdrawalOptions.Style("display") = "none"
            CheckBoxReplacedDisbursements.Visible = False
            ddlCheckType.SelectedIndex = 0
            ddlCheckType.SelectedValue = ""
            ddlCheckType.Enabled = False
            'START: SR | 2018.10.24 | YRS-AT-3101 - Check for EFT disbursement type.
            If (ddlEFTType.Items.Count > 0) Then
                ddlEFTType.Enabled = True
                ddlEFTType.SelectedIndex = 0
                ddlEFTType.SelectedValue = ""
            End If
            'END: SR | 2018.10.24 | YRS-AT-3101 - Check for EFT disbursement type.
            SetCheckSeriesControlsVisibility(False)
            divApprovedEFT.Visible = False
            SetVisibleProperty(False)
            ClearCheckSeriesSessions()
            PaymentMethodType = PaymentMethod.EFT
            'START: VC | 2018.09.19 | YRS-AT-3101 -  Commented code and Added code to change button name from Pay to GenerateFile
            ButtonPay.Text = "Generate file"
            'END: VC | 2018.09.19 | YRS-AT-3101 -  Commented code and Added code to change button name from Pay to GenerateFile

            'START: VC | 2018.09.19 | YRS-AT-3101 -  To hide CONFIRM EFT controls
            divConfirmEFTRecalculate.Visible = False
            'END: VC | 2018.09.19 | YRS-AT-3101 -  To hide CONFIRM EFT controls
            CheckBoxReplacedDisbursements.Visible = False
            CheckBoxReplacedDisbursements.Checked = False
        ElseIf Value = PaymentMethod.APPROVED_EFT Then
            tblWithdrawalOptions.Style("display") = "none"
            CheckBoxReplacedDisbursements.Visible = False
            ddlCheckType.SelectedIndex = 0
            ddlCheckType.SelectedValue = ""
            ddlCheckType.Enabled = False
            'START: SR | 2018.10.24 | YRS-AT-3101 - Check for EFT disbursement type.
            If (ddlEFTType.Items.Count > 0) Then
                ddlEFTType.Enabled = True
                ddlEFTType.SelectedIndex = 0
                ddlEFTType.SelectedValue = ""
            End If
            'END: SR | 2018.10.24 | YRS-AT-3101 - Check for EFT disbursement type.
            SetCheckSeriesControlsVisibility(False)
            divApprovedEFT.Visible = True
            SetVisibleProperty(False)
            ClearCheckSeriesSessions()
            PaymentMethodType = PaymentMethod.APPROVED_EFT
            'START: VC | 2018.09.19 | YRS-AT-3101 -  Commented code and Added code to change button name from Save to Paid
            'ButtonPay.Text = "Save"
            ButtonPay.Text = "Paid"
            ButtonPay.Enabled = False
            'END: VC | 2018.09.19 | YRS-AT-3101 -  Commented code and Added code to change button name from Save to Paid
            ApprovalEFTCount = "0"
            RejectionEFTCount = "0"
            'START: VC | 2018.09.19 | YRS-AT-3101 -  To show CONFIRM EFT controls
            divConfirmEFTRecalculate.Visible = True
            'END: VC | 2018.09.19 | YRS-AT-3101 -  To show CONFIRM EFT controls
            CheckBoxReplacedDisbursements.Visible = False
            CheckBoxReplacedDisbursements.Checked = False
            btnImportEFTBankResponseFile.Enabled = False
        ElseIf Value = PaymentMethod.CHECK Then
            tblWithdrawalOptions.Style("display") = "normal"
            CheckBoxReplacedDisbursements.Visible = False ' SR | 2018.10.09 | YRS-AT-3101 | Replace disbursement checkbox should not be visible if Payment method as CHECK selected.
            ddlCheckType.SelectedIndex = 0
            ddlCheckType.SelectedValue = ""
            ddlCheckType.Enabled = True
            'START: SR | 2018.10.24 | YRS-AT-3101 - Check for EFT disbursement type.
            If (ddlEFTType.Items.Count > 0) Then
                ddlEFTType.Enabled = False
                ddlEFTType.SelectedIndex = 0
                ddlEFTType.SelectedValue = ""
            End If
            'END: SR | 2018.10.24 | YRS-AT-3101 - Check for EFT disbursement type.
            SetCheckSeriesControlsVisibility(True)
            divApprovedEFT.Visible = False
            SetValuesInControles()
            PaymentMethodType = PaymentMethod.CHECK
            ButtonPay.Text = "Pay"
            'START: VC | 2018.09.19 | YRS-AT-3101 -  To hide CONFIRM EFT controls
            divConfirmEFTRecalculate.Visible = False
            'END: VC | 2018.09.19 | YRS-AT-3101 -  To hide CONFIRM EFT controls
            CheckBoxReplacedDisbursements.Visible = True
            CheckBoxReplacedDisbursements.Checked = True
        End If
    End Sub

    ' Clear EFT as well as CHECK data grid.
    Public Sub ClearDatagrid()
        ' clear eft datagrid
        dgEFTDisbursements.DataSource = Nothing
        dgEFTDisbursements.DataBind()
        dgEFTDisbursements.Style("Display") = "none"

        ' clear CHECK datagrid
        DataGridPayment.DataSource = Nothing
        DataGridPayment.DataBind()
        DataGridPayment.Style("Display") = "none"
    End Sub

    ' Populate Disbursement type for EFT payment method
    Private Function PopulateEFTDisbursementType()
        Dim data As DataSet
        Try
            data = objPaymenManagerBO.GetEFTDisbursementType()
            If HelperFunctions.isNonEmpty(data.Tables(0)) Then
                Me.EFTDisbursementType = data.Tables(0)

                Me.ddlEFTType.Items.Clear()
                ddlEFTType.DataSource = data.Tables(0)
                ddlEFTType.DataTextField = "chvDescription"
                ddlEFTType.DataValueField = "chrDisbursementType"
                ddlEFTType.DataBind()
                Me.ddlEFTType.Items.Insert(0, "")

            End If
        Catch ex As Exception
            Throw
        Finally
            data = Nothing
        End Try
    End Function

    Private Sub ddlEFTType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEFTType.SelectedIndexChanged
        Try
            'DistroyCashedData() 'ML | YRS-AT-4601 | 2019.12.03 | Change method name due to spelling mistake
            DistroyCachedData()
            SetWithdrawalControls(False)
            SetVisibleProperty(False)
            ' START : SR | 2018.10.24 | YRS-AT-3101 - Get EFT file header description from database(AtsMetaEFTTypes).
            If HelperFunctions.isNonEmpty(EFTDisbursementType) Then
                Dim disbursementTypeRow() As DataRow = EFTDisbursementType.Select("chrDisbursementType = '" + ddlEFTType.SelectedValue + "'")
                If disbursementTypeRow.Length > 0 Then
                    Me.EFTHeader = disbursementTypeRow(0)("chvEFTHeader").ToString().Trim
                Else
                    Me.EFTHeader = String.Empty
                End If
            End If
            ' END : SR | 2018.10.24 | YRS-AT-3101 - Get EFT file header description from database(AtsMetaEFTTypes).
            If ddlEFTType.SelectedValue().ToString().Trim().ToUpper = "TDLOAN" Then
                If PaymentMethodType = PaymentMethod.EFT Then
                    LoadDataGridForEFTPaymentMethod()
                    DataGridPayment.SelectedIndex = -1
                    divFundNoAndRecalculate.Visible = True
                    TextBoxFundIdNo.Text = String.Empty
                ElseIf PaymentMethodType = PaymentMethod.APPROVED_EFT Then
                    LoadDatagridForApprovalRejection()
                    dgEFTDisbursements.SelectedIndex = -1
                    divConfirmEFTRecalculate.Visible = True ' VC | 2018.09.19 | YRS-AT-3101 -  To show recalculation controls of CONFIRM EFT
                End If
            Else
                ' clear datagrid on blank eft type selection.         
                If PaymentMethodType = PaymentMethod.EFT Then
                    Me.DataGridPayment.DataSource = Nothing
                    Me.DataGridPayment.DataBind()
                ElseIf PaymentMethodType = PaymentMethod.APPROVED_EFT Then
                    ' clear datagrid on blank eft type selection.             
                    Me.dgEFTDisbursements.DataSource = Nothing
                    Me.dgEFTDisbursements.DataBind()
                End If
                ButtonPay.Enabled = False
                ButtonPrint.Enabled = False
            End If

            SetNetAmount()

            If ddlEFTType.SelectedIndex = 0 Then
                ButtonFind.Enabled = False
                'START: VC | 2018.09.19 | YRS-AT-3101 -  To disable button controls
                btnRecalculateEFTCheck.Disabled = True
                btnConfirmEFTRecalculate.Disabled = True
                'END: VC | 2018.09.19 | YRS-AT-3101 -  To disable button controls
                ' Set controls for Approved/Reject EFT options for blank option selected as EFT disbursement type.
                If PaymentMethodType = PaymentMethod.APPROVED_EFT Then
                    btnImportEFTBankResponseFile.Enabled = False
                End If
            Else
                ButtonFind.Enabled = True
                'START: VC | 2018.09.19 | YRS-AT-3101 -  To enable button controls
                btnRecalculateEFTCheck.Disabled = False
                btnConfirmEFTRecalculate.Disabled = False
                'END: VC | 2018.09.19 | YRS-AT-3101 -  To enable button controls
                ' Set controls for Approved/Reject EFT options.
                If PaymentMethodType = PaymentMethod.APPROVED_EFT Then
                    ButtonPay.Enabled = False
                    btnImportEFTBankResponseFile.Enabled = True
                End If
            End If


        Catch ex As Exception
            HelperFunctions.LogException("ddlEFTType_SelectedIndexChanged", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
        End Try
    End Sub

    ' This function will hide/ unhide check series controls based on payment options selected.
    Private Function SetCheckSeriesControlsVisibility(ByVal value As Boolean)
        Try
            divFundNoAndRecalculate.Visible = value
            divCheckSeries.Visible = value
        Catch ex As Exception
            Throw
        End Try
    End Function

    ' This method is copy of PopulateData. This function will only handle EFT data for loan disbursements.
    Private Sub PopulateEFTDataForLoanDisbursement()
        Dim disbursementType As String = String.Empty
        Try
            ' Get disbursement type. 
            If (ddlEFTType.SelectedItem.Text.Trim.ToUpper = "TAXDEFERRED LOAN") Then
                disbursementType = "TDLOAN"
            End If

            ' Get Loan disbursements record for EFT payment
            objPaymenManagerBO.getdisbursementswithoutfunding(disbursementType, String.Empty, String.Empty, String.Empty, String.Empty, PaymentMethodType) ' SR | 2018.04.11 | YRS-AT-3101 | Added new parameter as PaymentMethodType to retrieve disbursement records based on PaymentMethodType

            g_datatable_DisbursementsALL = CType(objPaymenManagerBO.datatable_Disbursements, DataTable)
            If HelperFunctions.isNonEmpty(g_datatable_DisbursementsALL) Then
                ' Copy data in cache to avoid databse execution more often
                If Me.Session_datatable_DisbursementsCACHE Is Nothing Then
                    Me.Session_datatable_DisbursementsALL = g_datatable_DisbursementsALL
                    g_datatable_DisbursementsCACHE = g_datatable_DisbursementsALL.Copy()
                    Me.Session_datatable_DisbursementsCACHE = g_datatable_DisbursementsCACHE
                Else
                    For Each dr As DataRow In g_datatable_DisbursementsALL.Rows
                        Me.Session_datatable_DisbursementsCACHE.ImportRow(dr)
                    Next
                    Me.Session_datatable_DisbursementsCACHE = RemoveDuplicateRows(Me.Session_datatable_DisbursementsCACHE, "DisbursementID")
                End If
            End If

            ' clear all check nos. as it will be not be used for EFT payment method
            Me.SessionCheckDate = objPaymenManagerBO.DateTime_CheckDate
            Me.SessionLongCANADACheckNo = 0
            Me.SessionLongRefundCheckNo = 0
            Me.SessionLongUSCheckNo = 0
            Me.SessionLongTDLoanCheckNo = 0
            Me.SessionLongEXPCheckNo = 0
            Me.SessionLongSHIRAMCheckNo = 0

            BindData()
            SetCommandButtons(PaymentMethodType)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SetCommandButtons(ByVal paymentMethodType As String)
        If (paymentMethodType = PaymentMethod.EFT) Then
            If HelperFunctions.isNonEmpty(g_datatable_Disbursements) Then
                Me.ButtonPay.Enabled = True
                Me.ButtonPrint.Enabled = True
            Else
                Me.ButtonPay.Enabled = False
                Me.ButtonPrint.Enabled = False
            End If
        ElseIf (paymentMethodType = PaymentMethod.APPROVED_EFT) Then
            If HelperFunctions.isNonEmpty(Me.EFTDisbursementsPendingForApproval) Then
                Me.ButtonPay.Enabled = True
                Me.ButtonPrint.Enabled = True
            Else
                Me.ButtonPay.Enabled = False
                Me.ButtonPrint.Enabled = False
            End If
        End If
    End Sub

    ' This method is copy of PopulateData. It will handle only EFT data for loan disbursements.
    Private Sub LoadDataGridForEFTPaymentMethod()
        Try
            Me.SessionDataLoadMode = False
            Me.PopulateEFTDataForLoanDisbursement()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' This method will check or uncheck refund related checkboxes
    Private Sub SetWithdrawalControls(ByVal value As Boolean)
        CheckBoxAllOtherWithdrawals.Checked = value
        CheckBoxDeathWithdrawals.Checked = value
        CheckBoxHardshipWithdrawals.Checked = value
        SessionShiraWithdrawals = value
        CheckBoxReplacedDisbursements.Checked = value
    End Sub

    ' This method is Copy of PAY() method and will only handle disbursement with EFT payment method.
    Function PAY_EFT()
        Dim disbursementType As String
        Dim filterExpression As String
        Dim eftDate As DateTime
        Dim arrayFileList(4, 4) As String
        Dim finalDisbursement As DataTable
        Dim popupScript As String

        Try
            ' Check date will be used in EFT file preparation
            eftDate = Me.SessionPMStartDate

            If ValidateEFTData() Then  'For eft payment method, if AtsMetaPaymentTypes.bitIsEnabled is configured as “1”
                finalDisbursement = ArrangeRecordstoSortOrder()
                filterExpression = "Selected = 1"
                objPaymenManagerBO.EFTHeader = Me.EFTHeader ' SR | 2018.05.28 | get table driven Headef for EFT disbursement type.
                objPaymenManagerBO.DisbursementType = ddlEFTType.SelectedValue.Trim.ToUpper   ' SR | 2018.10.11 | get disbursement type
                ' call EFT file creation method.
                If (objPaymenManagerBO.Pay_EFT(finalDisbursement, eftDate, filterExpression, YMCAObjects.Module.PaymentManagerEFTLoan)) Then
                    'START: VC | 2018.11.16 | YRS-AT-3101 | Commented existing hard coded message and added new code to show message from AtsMetaMessages table                 
                    'MessageBox.Show(PlaceHolder1, "Process disbursements", "Successfully generated EFT File", MessageBoxButtons.OK)
                    MessageBox.Show(PlaceHolder1, "Process disbursements", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_LOAN_EFT_GENERATE_EFT_SUCCESS), MessageBoxButtons.OK)
                    'END: VC | 2018.11.16 | YRS-AT-3101 | Commented existing hard coded message and added new code to show message from AtsMetaMessages table                 
                    ' reset controls
                    ' ButtonSelectNone.Text = "Select None" ' SR | 2016.10.18 | YRS-AT-3101 | Commented code
                    Me.TextBoxManualCheckNo.Text = String.Empty
                    Me.TextBoxCheckNoUs.Text = String.Empty
                    Me.TextBoxCheckNoCanadian.Text = String.Empty
                    Session("FTFileList") = Nothing
                    Session("FTFileList") = objPaymenManagerBO.DataTableNameFileList
                    ''CALLING ASPX PAGE
                    popupScript = "<script language='javascript'>" & _
                      "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1', 'FileCopyPopUp', " & _
                      "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                      "</script>"
                    If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                        Page.RegisterStartupScript("PopupScript1", popupScript)
                    End If

                    'DistroyCashedData() 'ML | YRS-AT-4601 | 2019.12.03 | Change method name due to spelling mistake
                    DistroyCachedData()
                    ' START | SR | 2018.05.28 | YRS-AT-3101 | After payment reset page controls .
                    SetWithdrawalControls(False)
                    SetVisibleProperty(False)
                    'Me.PopulateData()
                    PopulateEFTDataForLoanDisbursement()
                    DataGridPayment.SelectedIndex = -1
                    divFundNoAndRecalculate.Visible = True
                    TextBoxFundIdNo.Text = String.Empty
                    SetNetAmount()
                    btnConfirmEFTRecalculate.Disabled = True
                    ' END | SR | 2018.05.28 | YRS-AT-3101 | After payment reset page controls.
                End If
            End If
        Catch ex As Exception
            'TODO: Need to restructure CATCH coding
            Dim l_string_ErrorMsg As String
            If ex.Message <> "" Then
                If objPaymenManagerBO.string_ErrorString <> String.Empty Then
                    l_string_ErrorMsg = objPaymenManagerBO.string_ErrorString
                    objPaymenManagerBO.string_ErrorString = String.Empty
                    Me.ButtonDeselectErroneousDisbursements.Enabled = True
                    MessageBox.Show(PlaceHolder1, "Error processing EFT disbursements", l_string_ErrorMsg, MessageBoxButtons.Stop)
                Else
                    'l_string_ErrorMsg = ex.Message()
                    l_string_ErrorMsg = "ERROR processing EFT disbursements." + vbCrLf + _
                    "Processed disbursements are reverted to their original state." + vbCrLf + _
                    "Please check with help desk for disbursements causing error" + vbCrLf + _
                    "de-select disbursements having error and continue processing."
                    Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString() + vbCrLf + " " + l_string_ErrorMsg), False)
                End If
            Else
                Throw
            End If
        End Try
    End Function

    ' This method will clear all check nos. as it will be not be used for EFT payment method
    Public Sub ClearCheckSeriesSessions()
        Me.SessionCheckDate = objPaymenManagerBO.DateTime_CheckDate
        Me.SessionLongCANADACheckNo = 0
        Me.SessionLongRefundCheckNo = 0
        Me.SessionLongUSCheckNo = 0
        Me.SessionLongTDLoanCheckNo = 0
        Me.SessionLongEXPCheckNo = 0
        Me.SessionLongSHIRAMCheckNo = 0
    End Sub

    ' This method validate only EFT data.
    Function ValidateEFTData() As Boolean
        Dim filterExpression As String
        Dim disbursements As DataTable
        Dim disbursementRow As DataRow()
        Try
            ValidateEFTData = True
            ' Select grid data based on payment method type.
            If PaymentMethodType = PaymentMethod.APPROVED_EFT Then
                disbursements = EFTDisbursementsPendingForApproval
            Else
                disbursements = g_datatable_Disbursements
            End If

            ' perform validation based on payment method type.
            filterExpression = "Selected = 1"
            disbursementRow = disbursements.Select(filterExpression)
            If disbursementRow.Length <= 0 Then
                MessageBox.Show(PlaceHolder1, "Pay disbursements", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_LOAN_EFT_RECORD_SELECTION), MessageBoxButtons.Stop) ' SR | 2018.10.24 | YRS-AT-3101 - Get database driven message.
                ValidateEFTData = False
                Exit Function
            End If

            If PaymentMethodType = PaymentMethod.EFT Then
                filterExpression = "bitIsEnabled = 0"
                ' For eft payment method, if AtsMetaPaymentTypes.bitIsEnabled is configured as “0” then page will show following error message: 
                If (HelperFunctions.isNonEmpty(EFTDisbursementType)) Then
                    If (EFTDisbursementType.Select(filterExpression).Length > 0) Then
                        MessageBox.Show(PlaceHolder1, "Pay disbursements", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_LOAN_EFT_DISABLED), MessageBoxButtons.Stop) ' SR | 2018.10.24 | YRS-AT-3101 - Get database driven message.
                        ValidateEFTData = False
                        Exit Function
                    End If
                End If
            End If
        Catch ex As Exception
            ValidateEFTData = False
        End Try
    End Function



    ' This method is Copy of Pay() method and will only handle disbursement with EFT payment method for Approval or rejection process.
    Function SetEFTDisbursementPaymentStatus()
        Dim disbursementType As String = String.Empty
        Dim filterExpression As String = String.Empty
        Dim eftDate As DateTime
        Dim finalDisbursement As DataTable
        Dim popupScript As String
        Dim rows As DataRow()
        Dim databaseUpdateFailedData As DataTable, rejectionEmailNotSent As DataTable
        Dim completedMessage, databseUpdateFailureMessage, notReceivedRejectionEmailMessage As String
        Dim totalApproved, totalRejected, totalFailed, totalNotReceivedRejectionEmail As Integer
        Dim messageParameters As Dictionary(Of String, String)

        totalRejected = 0
        totalFailed = 0
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "SetEFTDisbursementPaymentStatus START") ' SR | 2018.10.24 | YRS-AT-3101 - Trace data into file.
            ' Check date will be used in EFT file preparation
            eftDate = Me.SessionPMStartDate

            ' Set disbursement type.         
            objPaymenManagerBO.DisbursementType = ddlEFTType.SelectedValue.Trim.ToUpper   ' SR | 2018.10.11 | get disbursement type
            finalDisbursement = EFTDisbursementsPendingForApproval

            If BatchList IsNot Nothing AndAlso BatchList.Count = 0 Then
                'START : ML | YRS-AT-4601 | Handle Message Visibility 
                'divErrorMessage.Style("display") = "normal"
                divErrorMessage.Visible = True
                'END : ML | YRS-AT-4601 | Handle Message Visibility 
                HelperFunctions.ShowMessageToUser(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_LOAN_EFT_INVALID_BATCHID), EnumMessageTypes.Error, divErrorMessage)
                Exit Function
            End If

            objPaymenManagerBO.BatchList = BatchList

            ' call EFT file creation method.
            finalDisbursement = objPaymenManagerBO.SetEFTDisbursementPaymentStatus(finalDisbursement, eftDate, YMCAObjects.Module.PaymentManagerEFTApprovalRejection.ToString())

            ' Get error code converted into complete message
            finalDisbursement = FillMessageDescription(finalDisbursement)

            messageParameters = New Dictionary(Of String, String)()
            rows = finalDisbursement.Select("Selected = 1 and IsDatabaseUpdated = 1 and SelectedBatchId = 1")
            If HelperFunctions.isNonEmpty(rows) AndAlso rows.Count > 0 Then
                totalApproved = rows.Count
            End If
            messageParameters.Add("EFTApproved", totalApproved.ToString())

            rows = finalDisbursement.Select("Selected = 0 and IsDatabaseUpdated = 1 and SelectedBatchId = 1")
            If HelperFunctions.isNonEmpty(rows) AndAlso rows.Count > 0 Then
                totalRejected = rows.Count
            End If
            messageParameters.Add("EFTRejected", totalRejected.ToString())

            If totalApproved > 0 Or totalRejected > 0 Then
                completedMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_EFT_BANKRESPONSE_COMPLETE, messageParameters)
                divSuccessMessage.InnerHtml = completedMessage
                'START : ML | YRS-AT-4601 | Handle Message Visibility 
                'divSuccessMessage.Style("display") = "normal"
                divSuccessMessage.Visible = True
                'END : ML | YRS-AT-4601 | Handle Message Visibility 
            End If

            rows = finalDisbursement.Select("IsDatabaseUpdated = 0 and SelectedBatchId = 1")
            If HelperFunctions.isNonEmpty(rows) AndAlso rows.Count > 0 Then
                totalFailed = rows.Count

                databaseUpdateFailedData = finalDisbursement.Copy
                databaseUpdateFailedData.Clear()
                For Each row As DataRow In rows
                    databaseUpdateFailedData.ImportRow(row)
                Next
                messageParameters.Add("EFTFailedToUpdate", totalFailed.ToString())
                databseUpdateFailureMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_EFT_BANKRESPONSE_COMPLETE_WITH_ERROR, messageParameters)
                divDBErrorMessage.InnerHtml = String.Format("{0}<br />", databseUpdateFailureMessage)
                'START : ML | YRS-AT-4601 | Handle Message Visibility 
                'divDBErrorMessage.Style("display") = "normal"
                divDBErrorMessage.Visible = True
                'END : ML | YRS-AT-4601 | Handle Message Visibility 

                gvEFTFailedPayments.DataSource = databaseUpdateFailedData
                gvEFTFailedPayments.DataBind()
            End If

            rows = finalDisbursement.Select("IsDatabaseUpdated = 1 and SelectedBatchId = 1 and IsEmailSent = 0")
            If HelperFunctions.isNonEmpty(rows) AndAlso rows.Count > 0 Then
                totalNotReceivedRejectionEmail = rows.Count

                rejectionEmailNotSent = finalDisbursement.Copy
                rejectionEmailNotSent.Clear()
                For Each row As DataRow In rows
                    rejectionEmailNotSent.ImportRow(row)
                Next
                messageParameters.Add("EFTFailedEmailCounter", totalNotReceivedRejectionEmail.ToString())
                notReceivedRejectionEmailMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_EMAIL_NOT_SENT_COMMON, messageParameters)
                divStatusMessage.InnerHtml = String.Format("{0}<br />", notReceivedRejectionEmailMessage)

                'START : ML | YRS-AT-4601 | Handle Message Visibility 
                'divStatusMessage.Style("display") = "normal"
                divStatusMessage.Visible = True
                'END : ML | YRS-AT-4601 | Handle Message Visibility 

                gvEFTFailedToSendRejectionEmail.DataSource = rejectionEmailNotSent
                gvEFTFailedToSendRejectionEmail.DataBind()
            End If

            'DistroyCashedData() 'ML | YRS-AT-4601 | 2019.12.03 | Change method name due to spelling mistake
            DistroyCachedData()
            'Me.PopulateData()
            LoadDatagridForApprovalRejection()
            SetNetAmount()   ' SR | YRS-AT-3101 | 2018-11-06 | Reset total net amount text box
            Me.SessionDataLoadMode = True
            Me.ButtonPrint.Enabled = True

        Catch ex As Exception
            Dim message As String
            If ex.Message <> "" Then
                If objPaymenManagerBO.string_ErrorString <> String.Empty Then
                    message = objPaymenManagerBO.string_ErrorString
                    objPaymenManagerBO.string_ErrorString = String.Empty
                    Me.ButtonDeselectErroneousDisbursements.Enabled = True
                    MessageBox.Show(PlaceHolder1, "Error processing EFT disbursements", message, MessageBoxButtons.Stop)
                Else
                    message = "ERROR processing EFT disbursements Approval/Rejection." + vbCrLf + _
                    "Processed disbursements are reverted to their original state." + vbCrLf + _
                    "Please check with help desk for disbursements causing error" + vbCrLf + _
                    "de-select disbursements having error and continue processing."
                    Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString() + vbCrLf + " " + message), False)
                End If
            Else
                Throw
            End If
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "SetEFTDisbursementPaymentStatus END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function
    ' END: SR | 2018.04.10 | YRS-AT-3101 | Based on selected disbursement option(EFT/CHECK/Approved EFT) change disbursement type.

    'START: PPP | 04/11/2018 | YRS-AT-3101 
    ' Loads eft disbursements which are pending for approval
    Private Function BindEFTDisbursementData()
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "BindEFTDisbursementData() START")
            Dim disbursements As DataTable = Me.EFTDisbursementsPendingForApproval
            Me.dgEFTDisbursements.DataSource = Nothing
            If HelperFunctions.isNonEmpty(disbursements) Then
                Me.dgEFTDisbursements.DataSource = disbursements.DefaultView
                Me.dgEFTDisbursements.DataBind()
                ButtonPrint.Visible = True
                dgEFTDisbursements.Style("Display") = "block"
                DataGridPayment.Style("Display") = "none"
            Else
                ButtonPrint.Visible = False
            End If

        Catch ex As Exception
            Throw
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "BindEFTDisbursementData END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function

    'START : SR | 04/11/2018 | YRS-AT-3101 | If bank File is uploaded then only bind good-in-order eft records.
    Private Function BindGoodInOrderEFTData()
        Dim disbursements As DataTable
        Dim dataView As DataView
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "BindGoodInOrderEFTData START")
            disbursements = Me.EFTDisbursementsPendingForApproval
            dataView = disbursements.DefaultView
            Me.dgEFTDisbursements.DataSource = Nothing
            If HelperFunctions.isNonEmpty(disbursements) Then
                dataView.RowFilter = "Selected = '" & 1 & "'"
                Me.dgEFTDisbursements.DataSource = dataView
            End If
            Me.dgEFTDisbursements.DataBind()
            dgEFTDisbursements.Style("Display") = "block"
            DataGridPayment.Style("Display") = "none"
        Catch ex As Exception
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "BindGoodInOrderEFTData END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        Finally

        End Try

    End Function
    'END : SR | 04/11/2018 | YRS-AT-3101 | If bank File is uploaded then only bind good-in-order eft records.

    Private Sub LoadDatagridForApprovalRejection()
        Dim disbursements As DataTable = YMCARET.YmcaBusinessObject.PaymentManagerBOClass.GetEFTDisbursements(ddlEFTType.SelectedValue.ToUpper().Trim(), EFTPaymentStatus.PROOF)

        If HelperFunctions.isNonEmpty(disbursements) Then
            If Not disbursements.Columns.Contains("IsDatabaseUpdated") Then
                disbursements.Columns.Add(HelperFunctions.CreateDataTableColumn("IsDatabaseUpdated", "System.Boolean", "0"))
            End If
            If Not disbursements.Columns.Contains("IsEmailSent") Then
                disbursements.Columns.Add(HelperFunctions.CreateDataTableColumn("IsEmailSent", "System.Boolean", "0"))
            End If
            If Not disbursements.Columns.Contains("ReasonCode") Then
                disbursements.Columns.Add(HelperFunctions.CreateDataTableColumn("ReasonCode", "System.String", ""))
            End If
            If Not disbursements.Columns.Contains("Reason") Then
                disbursements.Columns.Add(HelperFunctions.CreateDataTableColumn("Reason", "System.String", ""))
            End If
            'START: VC | 2018.09.21 | YRS-AT-3101 -  Added new column in data table to enable and disable checkbox
            If Not disbursements.Columns.Contains("EnableCheckBox") Then
                disbursements.Columns.Add(HelperFunctions.CreateDataTableColumn("EnableCheckBox", "System.String", "True"))
            End If
            'END: VC | 2018.09.21 | YRS-AT-3101 -  Added new column in data table to enable and disable checkbox
            'START: SR | 2018.10.11 | YRS-AT-3101 -  Added new column in data table to get selected batchids & email status for secnd mail sent.
            If Not disbursements.Columns.Contains("SelectedBatchId") Then
                disbursements.Columns.Add(HelperFunctions.CreateDataTableColumn("SelectedBatchId", "System.Boolean", "0"))
            End If
            'END: SR | 2018.10.11 | YRS-AT-3101 -  Added new column in data table to get selected batchids & email status for secnd mail sent
            disbursements.AcceptChanges()
        End If
        ' By default Select all eft records and disable check box.
        For Each dataRow As DataRow In disbursements.Rows
            If Not (dataRow Is Nothing) Then
                dataRow("Selected") = 0
                dataRow("EnableCheckBox") = 0
            End If
        Next

        Me.EFTDisbursementsPendingForApproval = disbursements
        BindEFTDisbursementData()
        SetCommandButtons(PaymentMethodType)
    End Sub

    'Set selection of all disbursement
    Private Sub SetSelectionOfEFTDisbursements(ByVal isSelected As Boolean)
        Dim disbursements As DataTable = Me.EFTDisbursementsPendingForApproval
        If HelperFunctions.isNonEmpty(disbursements) Then
            For Each row As DataRow In disbursements.Rows
                row("Selected") = isSelected
            Next
            Me.EFTDisbursementsPendingForApproval = disbursements
        End If

        BindEFTDisbursementData()
    End Sub

    ' This method will be used in Find button for Approved eft method type.
    Private Sub FindEFTDisbursement()
        Dim disbursements As DataTable
        Dim totalSum As Double
        Dim checkBoxControl As CheckBox
        Dim isFundIDValid As Boolean
        Try
            isFundIDValid = False
            If Me.dgEFTDisbursements.Items.Count > 0 Then
                Dim nfi As New CultureInfo("en-US", False)
                PersistEFTDisbursementSelectionsFromGrid()

                nfi.NumberFormat.CurrencyGroupSeparator = ","
                nfi.NumberFormat.CurrencySymbol = "$"
                nfi.NumberFormat.CurrencyDecimalDigits = 2

                disbursements = Me.EFTDisbursementsPendingForApproval

                If TextBoxFundIdNo.Text = String.Empty Then
                    dgEFTDisbursements.DataSource = disbursements
                    dgEFTDisbursements.DataBind()
                Else

                    For l_DataRow As Integer = 0 To dgEFTDisbursements.Items.Count - 1
                        If dgEFTDisbursements.Items(l_DataRow).Cells(fundIdNoEFTGridIndex).Text = TextBoxFundIdNo.Text Then
                            dgEFTDisbursements.Items(l_DataRow).BackColor = System.Drawing.Color.LightBlue
                            isFundIDValid = True
                        End If
                    Next
                    If isFundIDValid Then
                        For Each l_DataGridItem As DataGridItem In Me.dgEFTDisbursements.Items
                            checkBoxControl = l_DataGridItem.FindControl("chkEFTDisbursement")
                            If l_DataGridItem.Cells(1).Text = TextBoxFundIdNo.Text Then
                                checkBoxControl.Checked = True
                                SetFocus(checkBoxControl)
                            End If
                            If checkBoxControl.Checked = True Then
                                totalSum += l_DataGridItem.Cells(netAmountEFTGridIndex).Text
                            End If
                        Next

                        Me.TextBoxNetAmount.Text = System.Convert.ToString(totalSum.ToString("C", nfi)).Trim()
                        Me.SessionDoubleTotalNetAmount = totalSum
                    Else
                        'START: VC | 2018.11.16 | YRS-AT-3101 | Commented existing hard coded message and added new code to show message from AtsMetaMessages table                 
                        'MessageBox.Show(PlaceHolder1, "Invalid Fund Id", String.Format("Fund Id {0} not found ", TextBoxFundIdNo.Text), MessageBoxButtons.OK)
                        Dim messageParameters = New Dictionary(Of String, String)()
                        messageParameters.Add("FundId", TextBoxFundIdNo.Text)
                        MessageBox.Show(PlaceHolder1, "Invalid Fund Id", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_LOAN_EFT_FUNDID_NOTFOUND, messageParameters), MessageBoxButtons.OK)
                        'END: VC | 2018.11.16 | YRS-AT-3101 | Commented existing hard coded message and added new code to show message from AtsMetaMessages table                 
                        TextBoxFundIdNo.Text = String.Empty
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())), False)
        Finally
            checkBoxControl = Nothing
            disbursements = Nothing
        End Try
    End Sub

    ' This method is used in Save method to derive approvel/Rejection EFT disbursments
    Private Function PersistEFTDisbursementSelectionsFromGrid()
        Dim disbursements As DataTable
        Dim searchText As String
        Dim foundDataRow As DataRow()
        Dim checkBoxControl As CheckBox
        Dim approvalCount As Int16 = 0
        Dim rejectionCount As Int16 = 0
        BatchList = New List(Of String) ' SR | 2018.10.05 | YRS-AT-3101 | initialise batch list

        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "PersistEFTDisbursementSelectionsFromGrid() START")

            disbursements = Me.EFTDisbursementsPendingForApproval
            BatchList.Clear()  ' SR | 2018.10.05 | YRS-AT-3101 | clear batch list
            For Each l_DataGridItem As DataGridItem In Me.dgEFTDisbursements.Items
                checkBoxControl = l_DataGridItem.FindControl("chkEFTDisbursement")
                searchText = String.Format("DisbursementEFTID = '{0}'", l_DataGridItem.Cells(disbursementEFTIDEFTGridIndex).Text)
                foundDataRow = disbursements.Select(searchText)
                If (Not checkBoxControl Is Nothing) And (foundDataRow.Length > 0) Then
                    If checkBoxControl.Checked = True Then
                        foundDataRow(0)("Selected") = 1
                        ' START | SR | 2018.10.05 | YRS-AT-3101 | Add selected batch list
                        If Not BatchList.Contains(foundDataRow(0)("BatchId")) Then
                            BatchList.Add(foundDataRow(0)("BatchId"))
                        End If
                        ' END | SR | 2018.10.05 | YRS-AT-3101 | Add selected batch list
                        approvalCount = approvalCount + 1
                    Else
                        foundDataRow(0)("Selected") = 0
                    End If
                End If
            Next
            ' START | SR | 2018.10.05 | YRS-AT-3101 | Count relected EFT records from selected batch list
            ' check only select batchids
            For Each row As DataRow In disbursements.Rows
                If BatchList.Contains(row("BatchId").ToString()) Then
                    If row("Selected") = 0 Then
                        rejectionCount = rejectionCount + 1
                    End If
                End If
            Next row
            ' END | SR | 2018.10.05 | YRS-AT-3101 | Count relected EFT records from selected batch list
            Me.EFTDisbursementsPendingForApproval = disbursements
            BindEFTDisbursementData()
            ApprovalEFTCount = approvalCount
            RejectionEFTCount = rejectionCount
        Catch
            Throw
        Finally
            searchText = Nothing
            checkBoxControl = Nothing
            foundDataRow = Nothing
            disbursements = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "PersistEFTDisbursementSelectionsFromGrid END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function
    'END: PPP | 04/11/2018 | YRS-AT-3101 

    'START: SB | 2018.04.09 | YRS-AT-3101 | Following functions will help to auto check approved EFT payments in the grid when bank acknowlegement file is imported
    'TODO: Need to restructure / Test following code
    Private Sub btnImportEFTBankResponseFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportEFTBankResponseFile.Click
        Dim fileImportClient As YMCARET.YmcaBusinessObject.PaymentManagerBOClass
        Dim checkSecurity As String
        Dim message As String
        Dim fileData As DataTable
        Dim searchText As String
        Dim rows As DataRow()
        Dim disbursementTable As DataTable
        Dim result As YMCAObjects.ReturnObject(Of Boolean)
        Dim batchIdFromFileName As String 'SR | 2018.10.05 | YRS-AT-3101 -  Declared variables
        Dim noRecordExists As Boolean = False 'SR | 2018.10.15 | YRS-AT-3101 - added flag to validate blank imported file.
        Dim eftFilePath As String = String.Empty
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("payment Manager", "btnImportEFTBankResponseFile_Click START")

            checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            If Not checkSecurity.Equals("True") Then

                'START : ML | YRS-AT-4601 | Handle Message Visibility 
                'divErrorMessage.Style("display") = "normal" 'Shilpa N : 03/18/2019 | YRS-AT-4248 | To display the divErrorMessage 
                divErrorMessage.Visible = True
                'END : ML | YRS-AT-4601 | Handle Message Visibility 
                HelperFunctions.ShowMessageToUser(checkSecurity, EnumMessageTypes.Error, divErrorMessage)
                Exit Sub
            End If

            eftFilePath = SaveFileIntoImportFolder()
            If Not String.IsNullOrEmpty(eftFilePath) Then
                fileImportClient = New YMCARET.YmcaBusinessObject.PaymentManagerBOClass
                fileImportClient.EFTHeader = EFTHeader
                result = fileImportClient.IsValidBankEFTFile(eftFilePath, EFTDisbursementsPendingForApproval)
                If result.Value Then
                    ImportedBankFileName = GetBankFileName(eftFilePath)
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("payment Manager", String.Format("ImportedBankFileName: {0}", ImportedBankFileName.ToString()))

                    batchIdFromFileName = fileImportClient.GetBatchIdFromFileName(eftFilePath)
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("payment Manager", String.Format("batchIdFromFileName: {0}", batchIdFromFileName.ToString()))

                    fileData = fileImportClient.ReadEFTFile(eftFilePath)
                    disbursementTable = Me.EFTDisbursementsPendingForApproval

                    'START: VC | 2018.09.21 | YRS-AT-3101 -  Changing Selected column value into zero
                    If HelperFunctions.isNonEmpty(disbursementTable) Then
                        For Each dataRow As DataRow In disbursementTable.Rows
                            If Not (dataRow Is Nothing) Then
                                dataRow("Selected") = 0
                            End If
                        Next
                    End If
                    'END: VC | 2018.09.21 | YRS-AT-3101 -  Changing Selected column value into zero.

                    If HelperFunctions.isNonEmpty(fileData) Then
                        For Each dtRow As DataRow In fileData.Rows
                            If Convert.ToBoolean(dtRow.Item("IsApproved")) = True Then
                                searchText = String.Format("FundIdNo = '{0}'", dtRow.Item("FundIdNo").ToString().Trim)
                                rows = disbursementTable.Select(searchText)
                                If (rows.Length > 0) Then
                                    rows(0)("Selected") = 1
                                    rows(0)("EnableCheckBox") = "False" 'VC | 2018.09.21 | YRS-AT-3101 - Set EnableCheckBox column value of the data table to false
                                    noRecordExists = False
                                End If
                            End If
                        Next
                    Else
                        noRecordExists = True 'SR | 2018.10.15 | YRS-AT-3101 - Added flag to validate blank imported file.
                    End If
                    BindGoodInOrderEFTData() ' SR | 2018.09.25 | YRS-AT-3101 | Bind only good in order eft records.
                    SetNetAmount()
                    If noRecordExists Then
                        BatchList = New List(Of String) ' SR | 2018.10.05 | YRS-AT-3101 | initialise batch list
                        ' START | SR | 2018.10.05 | YRS-AT-3101 | Add selected batch list
                        If Not BatchList.Contains(batchIdFromFileName) Then
                            BatchList.Add(batchIdFromFileName)
                        End If

                        Me.SessionStartPay = False
                        ' END | SR | 2018.10.05 | YRS-AT-3101 | Add selected batch list                      
                        Dim messageParameters = New Dictionary(Of String, String)()
                        messageParameters.Add("BankFile", ImportedBankFileName.ToString())
                        message = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByTextMessageNo(MetaMessageList.MESSAGE_PM_CONFIRM_ZERO_APPROVE_DISBURSEMENT, messageParameters)
                        ApproveConfirmation = message
                        Page.ClientScript.RegisterHiddenField("hfApproveConfirmation", ApproveConfirmation.ToString())
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", "OpenEFTConfirmationDialog();", True)
                    End If
                Else
                    If (Not result.MessageList Is Nothing) Then
                        For i As Integer = 0 To result.MessageList.Count - 1
                            'START : ML | YRS-AT-4601 | Handle Message Visibility 
                            'divErrorMessage.Style("display") = "normal"
                            divErrorMessage.Visible = True
                            'END : ML | YRS-AT-4601 | Handle Message Visibility 
                            HelperFunctions.ShowMessageToUser(result.MessageList(i), EnumMessageTypes.Error, divErrorMessage)
                        Next
                    End If
                End If
            Else
                'START : ML | YRS-AT-4601 | Handle Message Visibility 
                'divErrorMessage.Style("display") = "normal"
                divErrorMessage.Visible = True
                'END : ML | YRS-AT-4601 | Handle Message Visibility 
                HelperFunctions.ShowMessageToUser("Please Select the File to Import.", EnumMessageTypes.Error, divErrorMessage)
            End If

        Catch ex As Exception
            HelperFunctions.LogException("btnImportEFTBankResponseFile_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
        Finally
            fileImportClient = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("payment Manager", "btnImportEFTBankResponseFile_Click END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub
    'END: SB | 2018.04.09 | YRS-AT-3101 | method to display data in the grid when bank acknowlegement file is imported

    'START: PPP | 04/11/2018 | YRS-AT-3101 
    ' It records full description of message based on code
    Private Function FillMessageDescription(ByVal data As DataTable) As DataTable
        Dim distinctReasonCodeTable As DataTable
        Dim message As YMCAObjects.MetaMessage
        Dim listOfDistinctMessages As Dictionary(Of String, String)
        Dim messageCode, messageDisplayText As String
        Dim messageNo As Integer
        Try
            distinctReasonCodeTable = data.DefaultView.ToTable(True, "ReasonCode")
            listOfDistinctMessages = New Dictionary(Of String, String)()
            If Not distinctReasonCodeTable Is Nothing AndAlso HelperFunctions.isNonEmpty(distinctReasonCodeTable) Then
                For Each row As DataRow In distinctReasonCodeTable.Rows
                    If Not Convert.IsDBNull(row(0)) Then
                        messageCode = Convert.ToString(row(0))
                        If HelperFunctions.isNonEmpty(messageCode) Then
                            If (Integer.TryParse(messageCode, messageNo)) Then
                                message = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(messageNo)
                            Else
                                message = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByCode(messageCode)
                            End If
                        End If
                        If Not message Is Nothing Then
                            messageDisplayText = message.DisplayText
                        End If
                        listOfDistinctMessages.Add(messageCode, messageDisplayText)
                    End If
                Next
            End If

            For Each row As DataRow In data.Rows
                messageCode = Convert.ToString(row("ReasonCode"))
                If Not String.IsNullOrEmpty(messageCode) Then
                    If listOfDistinctMessages.ContainsKey(messageCode) Then
                        row("Reason") = listOfDistinctMessages(messageCode)
                    End If
                End If
            Next
            Return data
        Catch
            Throw
        Finally
            messageDisplayText = Nothing
            messageCode = Nothing
            listOfDistinctMessages = Nothing
            message = Nothing
            distinctReasonCodeTable = Nothing
        End Try
    End Function

    Private Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        Try
            Me.SetEFTDisbursementPaymentStatus()
        Catch ex As Exception
            HelperFunctions.LogException("btnYes_Click", ex)
            Response.Redirect(String.Format("ErrorPageForm.aspx?Message={0}", Server.UrlEncode(ex.Message.Trim.ToString())))
        End Try
    End Sub
    'END: PPP | 04/11/2018 | YRS-AT-3101 

    'START: VC | 2018.09.19 | YRS-AT-3101 -  Added web method to perform select all and recalculation
    'Perform select all and recalculation of EFT and CHECK
    <System.Web.Services.WebMethod(True)>
    Public Shared Function SelectAllCheckBox(ByVal totalAmount As String, ByVal checkedStatus As String) As String
        Dim selected As String()
        Try
            Dim paymentManager As PaymentManager = New PaymentManager()
            paymentManager.Session_datatable_DisbursementsALL = HttpContext.Current.Session("g_datatable_DisbursementsALL")
            paymentManager.g_datatable_Disbursements = paymentManager.Session_datatable_DisbursementsALL
            'Spliting coma separated checkbox status into string array
            selected = checkedStatus.Split(New Char() {","c})
            'Updating data tables's Selected column value based on splited checkbox statues
            If HelperFunctions.isNonEmpty(paymentManager.g_datatable_Disbursements) Then
                For Each l_DataRow As DataRow In paymentManager.g_datatable_Disbursements.Rows
                    If Not (l_DataRow Is Nothing) Then
                        If (selected(paymentManager.g_datatable_Disbursements.Rows.IndexOf(l_DataRow)).ToString() = "1") Then
                            l_DataRow("Selected") = 1
                        Else
                            l_DataRow("Selected") = 0
                        End If

                    End If
                Next
                paymentManager.g_datatable_Disbursements.AcceptChanges()
            End If
            HttpContext.Current.Session("Double_TotalNetAmount") = totalAmount
        Catch ex As Exception

        End Try
    End Function

    'Perform select all and recalculation of confirm EFT
    <System.Web.Services.WebMethod(True)>
    Public Shared Function SelectAllConfirmEFTCheckBox(ByVal totalAmount As String, ByVal checkedStatus As String) As String
        Dim selected As String()
        Dim searchText As String
        Dim rows As DataRow()
        Try
            Dim paymentManager As PaymentManager = New PaymentManager()
            paymentManager.Session_datatable_DisbursementsALL = HttpContext.Current.Session("EFTDisbursementsPendingForApproval")
            paymentManager.EFTDisbursementsPendingForApproval = paymentManager.Session_datatable_DisbursementsALL
            'Spliting coma separated checkbox status into string array
            selected = checkedStatus.Split(New Char() {","c})
            'Updating data tables's Selected column value based on splited checkbox statues
            For Each element As String In selected
                If Not String.IsNullOrEmpty(element.ToString) Then
                    searchText = String.Format("FundIdNo = '{0}'", element.ToString)
                    rows = paymentManager.EFTDisbursementsPendingForApproval.Select(searchText)
                    If (rows.Length > 0) Then
                        rows(0)("Selected") = 1
                    End If
                End If
            Next

            'paymentManager.SessionDoubleTotalNetAmount = totalAmount
            HttpContext.Current.Session("Double_TotalNetAmount") = totalAmount
        Catch ex As Exception
        End Try
    End Function
    'END: VC | 2018.09.19 | YRS-AT-3101 -  Added web method to perform select all and recalculation

#End Region

    'START : SR | 2018.10.10 | YRS-AT-3101 -  Added method to render message in non master page.
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        HelperFunctions.DisplayMessagesInNonMasterPages()
    End Sub
    'END : SR | 2018.10.10 | YRS-AT-3101 -  Added method to render message in non master page.

    ' START : SR | 2018.10.10 | YRS-AT-3101 -  Added method to imported bank file name.
    Private Function GetBankFileName(ByVal sourceFilePath As String) As String
        Dim filePathInArray As String()
        Dim fileName As String = String.Empty
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "GetBankFileName START")
            If Not sourceFilePath.Equals(String.Empty) Then
                filePathInArray = sourceFilePath.Split({"\"}, StringSplitOptions.None)
                fileName = filePathInArray(filePathInArray.Length - 1)
                fileName = fileName.ToString().ToUpper().Replace(".DAT", "")
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("payment Manager", String.Format("Bank Filename: {0}", fileName.ToString()))
            Return fileName
        Catch ex As Exception
            Return fileName
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "GetBankFileName END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try

    End Function
    ' END : SR | 2018.10.10 | YRS-AT-3101 -  Added method to imported bank file name.
    ' START : SR | 2018.10.10 | YRS-AT-3101 -  Apply sorting for confirm EFT payment datagrid
    Private Sub dgEFTDisbursements_SortCommand(source As Object, e As DataGridSortCommandEventArgs) Handles dgEFTDisbursements.SortCommand
        Dim dv As New DataView
        Dim sortExpression As String
        sortExpression = e.SortExpression
        PersistEFTDisbursementSelectionsFromGrid()

        dv = EFTDisbursementsPendingForApproval.DefaultView
        dv.Sort = sortExpression

        If Not Session("EFTDisbursementsListSort") Is Nothing Then
            If sortExpression + " ASC" = Session("EFTDisbursementsListSort").ToString.Trim Then
                dv.Sort = sortExpression + " DESC"
            Else
                dv.Sort = sortExpression + " ASC"
            End If
        Else
            dv.Sort = sortExpression + " ASC"
        End If
        Session("EFTDisbursementsListSort") = dv.Sort

        BindEFTDisbursementData()

    End Sub
    ' END : SR | 2018.10.10 | YRS-AT-3101 -  Apply sorting for confirm EFT payment datagrid

    ' START : SR | 2018.10.10 | YRS-AT-3101 -  Save Imported File.
    Private Function SaveFileIntoImportFolder() As String
        Dim uploadFile As HttpPostedFile
        Dim importFolderPath As String
        Dim arrayFileName() As String
        Dim fileName As String
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "SaveFileIntoImportFolder() START")
            uploadFile = FileFieldEFTAckFile.PostedFile
            importFolderPath = ConfigurationSettings.AppSettings("EFT") + "\\" + "IMPORT\\"
            ' Create directory if it doesnot exist
            If Not Directory.Exists(importFolderPath) Then
                Directory.CreateDirectory(importFolderPath)
            End If
            ' Get filename 
            arrayFileName = uploadFile.FileName.Split("\")
            fileName = arrayFileName.GetValue(arrayFileName.Length - 1)
            importFolderPath = importFolderPath & fileName
            'if EFT file does not exist in imported folder then upload, otherwise delete & Upload again.
            If Not File.Exists(ImportFolderPath) Then
                uploadFile.SaveAs(importFolderPath)
            Else
                File.Delete(ImportFolderPath)
                uploadFile.SaveAs(importFolderPath)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("SaveFileIntoImportFolder", ex)
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "SaveFileIntoImportFolder END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        Finally
            uploadFile = Nothing
        End Try
        Return importFolderPath
    End Function
    ' END : SR | 2018.10.10 | YRS-AT-3101 -  Save Imported File.
    ' START : MMR | 2018.12.03 | YRS-AT-3101 |  Added to display status of email count
    Private Function DisplayEmailErrorForCheck(ByVal finalDisbursement As DataTable) As String
        Dim rows As DataRow()
        Dim receivedEmailMessage As String = String.Empty
        Dim total As Integer, success As Integer
        Dim log As StringBuilder
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "DisplayEmailErrorForCheck() START")

            total = 0
            success = 0
            rows = finalDisbursement.Select("SELECTED = 1")
            If HelperFunctions.isNonEmpty(rows) AndAlso rows.Count > 0 Then
                total = rows.Count
                log = New StringBuilder
                For Each row As DataRow In rows
                    log.AppendLine(String.Format("FundIdNo.:{0}, LoanNumber:{1}, IsEmailSent:{2}, ReasonCode:{3}", Convert.ToString(row("FundIdNo")), Convert.ToString(row("LoanNumber")), Convert.ToString(row("IsEmailSent")), Convert.ToString(row("ReasonCode"))))
                Next
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "DisplayEmailErrorForCheck()", log.ToString)
            End If

            rows = finalDisbursement.Select("SELECTED = 1 and IsEmailSent = 1")
            If HelperFunctions.isNonEmpty(rows) AndAlso rows.Count > 0 Then
                success = rows.Count
            End If

            If (total > 0) Then
                receivedEmailMessage = String.Format("<br />{0} out of {1} email sent to participant(s)", success, total)
            End If
            Return receivedEmailMessage
        Catch ex As Exception
            HelperFunctions.LogException("DisplayEmailErrorForCheck", ex)
            Return receivedEmailMessage
        Finally
            receivedEmailMessage = Nothing
            rows = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "DisplayEmailErrorForCheck END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Function
    ' END : MMR | 2018.12.03 | YRS-AT-3101 |  Added to display status of email count

    'START :ML| 2019.10.10 | YRS-AT-4601 | Set flag based on database key for Payment Outsourcing Go live date
    Private Function SetPaymentOutsourcingKey()
        Dim paymentOutsourcingGoliveDate As Date
        paymentOutsourcingGoliveDate = YMCARET.YmcaBusinessObject.MetaConfigMaintenance.GetConfigurationKeyValue("PAYMENT_OUTSOURCING_START_DATE").ToString().ToLower().Trim()
        If Today.Date >= paymentOutsourcingGoliveDate Then
            objPaymenManagerBO.IsPaymentOutSourcingKeyON = True
            Me.IsPaymentOutSourcingKeyON = True
        Else
            objPaymenManagerBO.IsPaymentOutSourcingKeyON = False
            Me.IsPaymentOutSourcingKeyON = False
        End If
    End Function
    'END :ML| 2019.10.10 | YRS-AT-4601 | Set flag based on database key for Payment Outsourcing Go live date
End Class