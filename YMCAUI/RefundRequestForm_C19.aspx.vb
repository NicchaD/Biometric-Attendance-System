'/**************************************************************************************************************/
'//' Copyright YMCA Retirement Fund All Rights Reserved. 
'//
'// Author: Megha Lad
'// Created on: 04/17/2020
'// Summary of Functionality: New Screen for COVID19 withdrawal listing screen
'// Declared in Version: 20.8.1.2| YRS-AT-4854 -  COVID - Special withdrawal functionality needed due to CARE Act/COVID-19 (Track IT - 41688) 
'//
'/**************************************************************************************************************/
'// REVISION HISTORY:
'// ------------------------------------------------------------------------------------------------------
'// Developer Name              | Date         | Version No      | Ticket
'// ------------------------------------------------------------------------------------------------------
'// 			                | 	           |		         | 
'// ------------------------------------------------------------------------------------------------------
' /**************************************************************************************************************/

Imports System.Data
Imports System.Data.SqlClient
'Hafiz 03Feb06 Cache-Session
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 03Feb06 Cache-Session

Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class RefundRequestForm_C19
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    'Dim strFormName As String = New String("RefundRequestForm.aspx")'Commented by Shashi Shekhar:2009-12-31:To Resolve the security Issue (Ref:Security Review excel sheet)
    'End issue id YRS 5.0-940


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents RefundTabStrip As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents RefundMultiPage As Microsoft.Web.UI.WebControls.MultiPage
    'Protected WithEvents DataGridList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridEmployment As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridFundedAccountContributions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridNonFundedAccountContributions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridRequests As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridRefundNotes As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents datagridCheckBoxStatusType As CustomControls.CheckBoxColumn '--Manthan Rajguru 2015-09-24 YRS-AT-2550: Commented as control not used anywhere in the page


    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAdd As System.Web.UI.WebControls.Button
    'Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    'Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddItem As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddNote As System.Web.UI.WebControls.Button
    Protected WithEvents btnConfirmDialogOK As System.Web.UI.WebControls.Button 'PK|08/30/2019 | YRS-AT-2670 |Button control added.

    'Protected WithEvents LabelListSSNo As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxListSSNo As System.Web.UI.WebControls.TextBox

    'Protected WithEvents LabelListFundNo As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxListFundNo As System.Web.UI.WebControls.TextBox

    'Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox

    'Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox


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

    'Protected WithEvents LabelAddress1 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAddress1 As System.Web.UI.WebControls.TextBox


    'Protected WithEvents LabelAddress2 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAddress2 As System.Web.UI.WebControls.TextBox

    'Protected WithEvents LabelAddress3 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAddress3 As System.Web.UI.WebControls.TextBox

    'Protected WithEvents LabelCity As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox

    'Protected WithEvents LabelState As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxState As System.Web.UI.WebControls.TextBox

    'Protected WithEvents LabelZip As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxZip As System.Web.UI.WebControls.TextBox


    'Protected WithEvents LabelCountry As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxCountry As System.Web.UI.WebControls.TextBox
    Protected WithEvents AddressWebUserControl1 As AddressUserControlNew
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

    'BA Account YMCA PhaseV 03-04-2009 by Dilip
    Protected WithEvents LabelBATermination As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxBATermination As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelBACurrentPIA As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxBACurrent As System.Web.UI.WebControls.TextBox
    'BA Account YMCA PhaseV 03-04-2009 by Dilip
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
    Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelHeader As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonUpdateAddress As System.Web.UI.WebControls.Button
    Protected WithEvents NotesFlag As System.Web.UI.HtmlControls.HtmlInputHidden
    'Plan Split Changes
    Protected WithEvents DataGridFundedAccntContributionsSavingsPlan As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DatagridNonFundedAcctContributionSavingsPlan As System.Web.UI.WebControls.DataGrid
    Protected WithEvents HeaderControl As YMCA_Header_WebUserControl
    'Start: Bala: 01/19/2019: YRS-AT-2398: Control checkbox and hiddenfield is added 
    Protected WithEvents HiddenFieldOfficersDetails As System.Web.UI.WebControls.HiddenField
    Protected WithEvents LabelSpecialHandling As System.Web.UI.WebControls.Label
    'End: Bala: 01/19/2019: YRS-AT-2398: Control checkbox and hiddenfield is added
    Protected WithEvents DivErrorMessage As System.Web.UI.HtmlControls.HtmlGenericControl 'VC | 2018.11.22 | YRS-AT-4018 | Added Div control
    'Plan Split Changes
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    'Added by Shubhrata May 21st,2007
    Const mConst_DataGridFundedAcctContributionsIndexOfAcctType As Integer = 0
    Const mConst_DataGridFundedAcctContributionsIndexOfIsBasicAcct As Integer = 1
    Const mConst_DataGridFundedAcctContributionsIndexOfTaxable As Integer = 2
    Const mConst_DataGridFundedAcctContributionsIndexOfNonTaxable As Integer = 3
    Const mConst_DataGridFundedAcctContributionsIndexOfInterest As Integer = 4
    Const mConst_DataGridFundedAcctContributionsIndexOfEmpTotal As Integer = 5
    Const mConst_DataGridFundedAcctContributionsIndexOfYmcaTaxable As Integer = 6
    Const mConst_DataGridFundedAcctContributionsIndexOfYmcaInterest As Integer = 7
    Const mConst_DataGridFundedAcctContributionsIndexOfYmcaTotal As Integer = 8
    Const mConst_DataGridFundedAcctContributionsIndexOfTotal As Integer = 9
    Const mConst_DataGridFundedAcctContributionsIndexOfPlanType As Integer = 10
    Const mConst_DataGridFundedAcctContributionsIndexOfAcctGroup As Integer = 11

    'These constants will serve for Unfunded data grids
    Const mConst_DataGridNonFundedAcctContributionsIndexOfAcctType As Integer = 0
    Const mConst_DataGridNonFundedAcctContributionsIndexOfTaxable As Integer = 1
    Const mConst_DataGridNonFundedAcctContributionsIndexOfNonTaxable As Integer = 2
    Const mConst_DataGridNonFundedAcctContributionsIndexOfInterest As Integer = 3
    Const mConst_DataGridNonFundedAcctContributionsIndexOfEmpTotal As Integer = 4
    Const mConst_DataGridNonFundedAcctContributionsIndexOfYmcaTaxable As Integer = 5
    Const mConst_DataGridNonFundedAcctContributionsIndexOfYmcaInterest As Integer = 6
    Const mConst_DataGridNonFundedAcctContributionsIndexOfYmcaTotal As Integer = 7
    Const mConst_DataGridNonFundedAcctContributionsIndexOfTotal As Integer = 8
    Const mConst_DataGridNonFundedAcctContributionsIndexOfPlanType As Integer = 9
    Const mConst_DataGridNonFundedAcctContributionsIndexOfAcctGroup As Integer = 10
    Const mConst_DataGridNonFundedAcctContributionsIndexOfISBasicAcct As Integer = 11

    Private Sub Page_Error(sender As Object, e As EventArgs) Handles Me.Error

    End Sub


    'Added by Shubhrata May 21st,2007
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region


#Region " Enumeration "
    Public Enum FormMode
        View
        Add
        Edit
        Save
        None
    End Enum
#End Region
    Dim objRefundProcess As New Refunds_C19
    Dim objRefundRequest As New Refunds_C19
#Region "global declarations"
    'START - Retirement plan account groups
    Const m_const_RetirementPlan_AP As String = "RAP"
    Const m_const_RetirementPlan_AM As String = "RAM"
    Const m_const_RetirementPlan_RG As String = "RRG"
    Const m_const_RetirementPlan_SA As String = "RSA"
    Const m_const_RetirementPlan_SS As String = "RSS"
    Const m_const_RetirementPlan_RP As String = "RRP"
    Const m_const_RetirementPlan_SR As String = "RSR"
    'END - Retirement plan account groups
    'START - Ssvings plan account groups
    Const m_const_SavingsPlan_TD As String = "STD"
    Const m_const_SavingsPlan_TM As String = "STM"
    Const m_const_SavingsPlan_RT As String = "SRT"
    'END - Savings plan account groups
    'SP 2014.10.09 BT 2633/YRS 5.0-2403 -  Remove expiration dates from withdrawal requests in YRS -Start
    Const m_const_Withdrawal_Status_index As Int16 = 3
    Const m_const_Withdrawal_ExipreDate_index As Int16 = 5
    'SP 2014.10.09 BT 2633/YRS 5.0-2403 -  Remove expiration dates from withdrawal requests in YRS -End
#End Region
#Region " Form Properties "


    '** This property is used to handle session state.
    Private Property VoluntaryWithdrawalTotal() As Decimal
        Get
            If Not Session("VoluntaryWithdrawalTotal_C19") Is Nothing Then
                Return (DirectCast(Session("VoluntaryWithdrawalTotal_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("VoluntaryWithdrawalTotal_C19") = Value
        End Set
    End Property
    Private Property HardshipWithdrawalTotal() As Decimal
        Get
            If Not Session("HardshipWithdrawalTotal_C19") Is Nothing Then
                Return (DirectCast(Session("HardshipWithdrawalTotal_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("HardshipWithdrawalTotal_C19") = Value
        End Set
    End Property
    Private Property SessionFormMode() As FormMode
        Get
            If Not (Session("FormMode_C19")) Is Nothing Then
                Return (CType(Session("FormMode_C19"), FormMode))
            Else
                Return FormMode.None
            End If
        End Get
        Set(ByVal Value As FormMode)
            Session("FormMode_C19") = Value
        End Set
    End Property
    Public Property SessionAccessRights() As String
        Get
            If Not (Session("l_integer_AccPermission_C19")) Is Nothing Then
                Return Session("l_integer_AccPermission_C19")
            Else
                Return Session("l_integer_AccPermission_C19") = String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("l_integer_AccPermission_C19") = Value
        End Set
    End Property
    Public Property SessionGrossAmount() As Decimal
        Get
            If Not (Session("SessionGrossAmount_C19")) Is Nothing Then
                Return Session("SessionGrossAmount_C19")
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("SessionGrossAmount_C19") = Value
        End Set
    End Property
    ' To Get / Set terminated flag
    Private Property IsTerminated() As Boolean
        Get
            If Not Session("IsTerminated_C19") Is Nothing Then
                Return (CType(Session("IsTerminated_C19"), Boolean))
            Else
                Return False
            End If
        End Get

        Set(ByVal Value As Boolean)
            Session("IsTerminated_C19") = Value
        End Set
    End Property

    Private Property DeathDate() As String
        Get
            If Not Session("DeathDate_C19") Is Nothing Then
                Return (DirectCast(Session("DeathDate_C19"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("DeathDate_C19") = Value
        End Set
    End Property
    'To keep List DataGrid Index
    Private Property ListSessionRowIndex() As Integer
        Get
            If Not (Session("ListRowIndex_C19")) Is Nothing Then
                Return (DirectCast(Session("ListRowIndex_C19"), Integer))
            Else
                Return -1
            End If
        End Get

        Set(ByVal Value As Integer)
            Session("ListRowIndex_C19") = Value
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

    'To Keep the RefundRequest ID for Selected Member
    Public Property SessionRefundRequestID() As String
        Get
            If Not (Session("RefundRequestID_C19")) Is Nothing Then
                Return (DirectCast(Session("RefundRequestID_C19"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("RefundRequestID_C19") = Value
        End Set
    End Property


    'To Keep the StatusType for Selected Member
    Private Property SessionStatusType() As String
        Get
            If Not (Session("StatusType_C19")) Is Nothing Then
                Return (DirectCast(Session("StatusType_C19"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("StatusType_C19") = Value
        End Set
    End Property

    'To Keep the Refund Request Datagrid to Refresh 
    Private Property SessionIsRefundRequest() As Boolean
        Get
            If Not (Session("IsRefundRequest_C19")) Is Nothing Then
                Return (CType(Session("IsRefundRequest_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsRefundRequest_C19") = Value
        End Set
    End Property

    'To Keep the Notes DataGrid

    Private Property SessionIsNotes() As Boolean
        Get
            If Not (Session("IsNotes_C19")) Is Nothing Then
                Return (CType(Session("IsNotes_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsNotes_C19") = Value
        End Set
    End Property

    'To get / set the Notes ID, which is used in Notes Master form to View Details.
    Private Property SessionNotesIndex() As Integer
        Get
            If Not Session("NotesIndex_C19") Is Nothing Then
                Return (DirectCast(Session("NotesIndex_C19"), Integer))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("NotesIndex_C19") = Value
        End Set
    End Property


    ' To get / set RefundType, this Property is used in RefundRequestWebForm.
    Private Property RefundType() As String
        Get
            If Not Session("RefundType_C19") Is Nothing Then
                Return (DirectCast(Session("RefundType_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("RefundType_C19") = Value
        End Set
    End Property
    'Added By Dilip on 30-10-2009  for market base withdrawal
    Private Property ISMarket() As Int16
        Get
            If Not Session("ISMarket_C19") Is Nothing Then
                Return (DirectCast(Session("ISMarket_C19"), Int16))
            Else
                Return 1
            End If

        End Get
        Set(ByVal Value As Int16)
            Session("ISMarket_C19") = Value
        End Set
    End Property

    'Added By Dilip on 30-10-2009  for market base withdrawal
    ' To get / set RefundStatus, this Property is used in RefundRequestWebForm.
    Private Property RefundStatus() As String
        Get
            If Not Session("RefundStatus_C19") Is Nothing Then
                Return (DirectCast(Session("RefundStatus_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("RefundStatus_C19") = Value
        End Set
    End Property

    'To Keep the flag to Raise the Refund Popup window
    Private Property SessionIsRefundPopupAllowed() As Boolean
        Get
            If Not (Session("IsRefundPopupAllowed_C19")) Is Nothing Then
                Return (CType(Session("IsRefundPopupAllowed_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsRefundPopupAllowed_C19") = Value
        End Set
    End Property

    'To Keep the flag to Raise the Refund Popup window
    Private Property SessionIsRefundProcessPopupAllowed() As Boolean
        Get
            If Not (Session("IsRefundProcessPopupAllowed_C19")) Is Nothing Then
                Return (CType(Session("IsRefundProcessPopupAllowed_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsRefundProcessPopupAllowed_C19") = Value
        End Set
    End Property

    ' To Get / Set Is vested flag
    Private Property IsVested() As Boolean
        Get
            If Not Session("IsVested_C19") Is Nothing Then
                Return (CType(Session("IsVested_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsVested_C19") = Value
        End Set
    End Property

    'To Keep the flag to Raise the Notes Popup window
    Private Property SessionIsNotesPopupAllowed() As Boolean
        Get
            If Not (Session("IsNotesPopupAllowed_C19")) Is Nothing Then
                Return (CType(Session("IsNotesPopupAllowed_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsNotesPopupAllowed_C19") = Value
        End Set
    End Property

    'To Keep the flag to store Whether Type is Personal Or Not.
    Private Property SessionIsPersonalOnly() As Boolean
        Get
            If Not (Session("IsPersonalOnly_C19")) Is Nothing Then
                Return (CType(Session("IsPersonalOnly_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsPersonalOnly_C19") = Value
        End Set
    End Property
    'To get if the user belongs to the Notes Admin group
    Public Property NotesGroupUser() As Boolean
        Get
            If Not (Session("NotesGroupUser_C19")) Is Nothing Then
                Return (CType(Session("NotesGroupUser_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("NotesGroupUser_C19") = Value
        End Set
    End Property
    'Plan Split Changes
    Private Property PlanTypeChosen() As String
        Get
            If Not Session("PlanTypeChosen_C19") Is Nothing Then
                Return (CType(Session("PlanTypeChosen_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PlanTypeChosen_C19") = Value
        End Set
    End Property
    Private Property IsRetiredActive() As Boolean
        Get
            If Not Session("IsRetiredActive_C19") Is Nothing Then
                Return (CType(Session("IsRetiredActive_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsRetiredActive_C19") = Value
        End Set
    End Property
    'Added by Ganeswar
    Private Property IsSavingPartial() As Boolean
        Get
            If Not Session("IsSavingPartial_C19") Is Nothing Then
                Return (CType(Session("IsSavingPartial_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsSavingPartial_C19") = Value
        End Set
    End Property
    Private Property IsRetirementPartial() As Boolean
        Get
            If Not Session("IsRetirementPartial_C19") Is Nothing Then
                Return (CType(Session("IsRetirementPartial_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsRetirementPartial_C19") = Value
        End Set
    End Property
    'Shashi Shekhar:10 feb 2011: for YRS 5.0-1236
    Private Property IsAccountLock() As Boolean
        Get
            If Not Session("IsAccountLock_C19") Is Nothing Then
                Return (CType(Session("IsAccountLock_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsAccountLock_C19") = Value
        End Set
    End Property
    'Added by Ganeswar-
    'Plan Split Changes-
    'Start- chandrasekar.c:2016.01.11 YRS-AT-2524: Get and Set property for Refund Request Extend Expire days
    Public Property RefundExtendExpireDays() As Integer
        Get

            If ViewState("intRefundExtendExpireDays") Is Nothing Then

                ViewState("intRefundExtendExpireDays") = YMCARET.YmcaBusinessObject.RefundRequest.GetExtendWithdrawalExpireDay()

            End If

            Return CType(ViewState("intRefundExtendExpireDays"), Integer)

        End Get
        Set(ByVal Value As Integer)
            ViewState("intRefundExtendExpireDays") = Value
        End Set
    End Property
    'End- chandrasekar.c:2016.01.11 YRS-AT-2524: Get and Set property for Refund Request Extend Expire days

    'START: SB | 2017.08.02 | YRS-AT-3324 | Added property that will return the reason for restriction of withdrawals for RMD eligible participants whose RMD is not generated
    Private Property ReasonForRestriction() As String
        Get
            If Not Session("ReasonForRestriction_C19") Is Nothing Then
                Return (CType(Session("ReasonForRestriction_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("ReasonForRestriction_C19") = Value
        End Set
    End Property

    ' Added property that will vaildate withdrawals are allowed or not for RMD eligible participants 
    Public Property IsPersonEligibleForRMD() As Boolean
        Get
            If Session("WithdrawalAllowedForRMDEligibleParticipants_C19") Is Nothing Then
                Session("WithdrawalAllowedForRMDEligibleParticipants_C19") = Validation.IsRMDExist(YMCAObjects.Module.Withdrawal, Session("FundId"), Me.ReasonForRestriction)
            End If
            Return CType(Session("WithdrawalAllowedForRMDEligibleParticipants_C19"), Boolean)
        End Get
        Set(ByVal Value As Boolean)
            Session("WithdrawalAllowedForRMDEligibleParticipants_C19") = Value
        End Set
    End Property
    'END: SB | 2017.08.02 | YRS-AT-3324 | Added property that will vaildate withdrawals are allowed or not for RMD eligible participants 

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'ButtonOk.Attributes.Add("OnClick", "javascript:OpenCloseWindow(0);")
        'ButtonUpdateAddress.Attributes.Add("OnClick", "javascript:OpenCloseWindow(1);")
        Dim l_int_UpdateButtonhit As Integer
        Dim l_int_AccessRights As Integer

        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        CheckReadOnlyMode() 'Shilpa N | 02/28/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
        l_int_UpdateButtonhit = Session("Updatebuttonhit_C19")
        'ButtonOk.Attributes.Add("OnClick", "javascript:OpenCloseWindow()")
        'Added to clear the value of the session
        objRefundRequest.IsSaveProcessComplete = False
        'Added to clear the value of the session

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YRS", "BindEvents();", True) 'PK|08/30/2019 | YRS-AT-2670 |Code added to register the script.
        Try
            'Plan Split Changes
            If IsPostBack = False Then
                Session("RetirementPlanAcctContribution_C19") = Nothing
                Session("SavingsPlanAcctContribution_C19") = Nothing
                Session("RetirementPlan_UnfundedContributions_C19") = Nothing
                Session("SavingsPlan_UnfundedContributions_C19") = Nothing
                ViewState("RefCanReasonCode") = Nothing 'Shashi Shekhar:27-05-2011: For YRS 5.0-1298
                ViewState("CanReq") = Nothing  'Shashi Shekhar:27-05-2011: For YRS 5.0-1298
                Session("ProcessSelect_C19") = Nothing
                Session("CancelSelect_C19") = Nothing
                'AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Added To clear sessions on page load
                ClearSession()
                Session("GenerateErrors_C19") = Nothing  'MMR | 2020.05.06 | YRS-AT-4854 | Clearing session value to avoid redirection to error page on first load
            End If

            If Me.SessionIsRefundRequest = False Then
                Session("ServiceRequestReport_C19") = Nothing
            End If

            If Page.IsPostBack() Then
                'Shashi Shekhar:27-05-2011: For YRS 5.0-1298
                'when user select the reason code from popup screen
                If Not Session("RefCanReasonCode") Is Nothing Then
                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Are you sure you want to Cancel Refund?", MessageBoxButtons.YesNo, False)
                    ViewState("RefCanReasonCode") = Session("RefCanReasonCode").ToString.Trim 'Shashi Shekhar:27-05-2011: For YRS 5.0-1298
                    Session("RefCanReasonCode") = Nothing
                    Exit Sub
                End If

                'Rahul 13Feb06
                '
                'Start of Code Added by hafiz on 22-Sep-2006 - YREN-2702
                If Not Session("bool_RefundRequest_C19") Is Nothing Then
                    If CType(Session("bool_RefundRequest_C19"), Boolean) = True Then
                        If Request.Form("OK") = "OK" Then
                            If Not Session("CancelSelect_C19") Is Nothing Then
                                'Shashi Shekhar:27-05-2011: For YRS 5.0-1298
                                Me.CancelRefundRequest(Session("CancelSelect_C19"), "YRS")

                                'resetting the flags used.
                                Session("CancelSelect_C19") = Nothing
                                Session("RefCanReasonCode") = Nothing 'Shashi Shekhar:2011-25-05: YRS 5.0-1298: For Cancel code reason implementation
                                ViewState("RefCanReasonCode") = Nothing 'Shashi Shekhar:27-05-2011: For YRS 5.0-1298
                                Session("bool_RefundRequest_C19") = False
                                'Start- SR:2013.12.06 -  BT2275/YRS 5.0-2235:Incorrect message in withdrawal process
                                If LTrim(RTrim(ViewState("WD_RefundType"))) = "HARD" Then
                                    'Start: Dinesh k           2013.08.20          BT: incorrect wording in YRS for automated note for hardship withdrawals
                                    'YMCARET.YmcaBusinessObject.NotesBOClass.InsertNotes(Me.SessionPersonID, "Withdrawn request has been cancel by the system because the participant is now active", False)
                                    YMCARET.YmcaBusinessObject.NotesBOClass.InsertNotes(Me.SessionPersonID, GetMessage("MESSAGE_WD_CANCEL_HARDSHIP_WITHDRAWAL"), False)
                                    'End: Dinesh k           2013.08.20          BT: incorrect wording in YRS for automated note for hardship withdrawals
                                Else
                                    YMCARET.YmcaBusinessObject.NotesBOClass.InsertNotes(Me.SessionPersonID, GetMessage("MESSAGE_WD_CANCEL_WITHDRAWAL"), False)
                                End If
                                'End- SR:2013.12.06 -  BT2275/YRS 5.0-2235:Incorrect message in withdrawal process
                            End If
                        End If
                    End If
                End If
                'End of Code - Added by hafiz on 22-Sep-2006 - YREN-2702
                'Dim l_DataTable As DataTable
                'If Me.SessionPersonID <> String.Empty And Me.SessionFundID <> String.Empty Then
                '    l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.MemberRefundRequestDetails(Me.SessionPersonID, Me.SessionFundID)
                '    If Not l_DataTable Is Nothing Then
                '        For Each l_DataRow As DataRow In l_DataTable.Rows
                '            If Not l_DataRow Is Nothing Then
                '                If CType(l_DataRow.Item("RequestStatus"), String).Trim.ToUpper = "PEND" Then
                '                    Me.SessionIsRefundRequest = True
                '                    Me.Session("ProcessSelect") = True
                '                End If

                '            End If
                '        Next
                '    End If
                'End If


                If Request.Form("Yes") = "Yes" Then
                    'If user has selected the request and it has to be cancel by the user
                    If Not Session("CancelSelect_C19") Is Nothing Then
                        'This CancelRefundRequest will only executed when user select the cancel request and he has also select the cancel reason code from popup screen
                        If Not ViewState("CanReq") Is Nothing And ViewState("CanReq").ToString.ToUpper.Trim = "TRUE" And Not ViewState("RefCanReasonCode") Is Nothing Then
                            'Shashi Shekhar:27-05-2011: For YRS 5.0-1298
                            Me.CancelRefundRequest(Session("CancelSelect_C19"), ViewState("RefCanReasonCode").ToString.Trim)
                            ViewState("RefCanReasonCode") = Nothing 'Shashi Shekhar:27-05-2011: For YRS 5.0-1298
                            ViewState("CanReq") = Nothing
                            Session("CancelSelect_C19") = Nothing
                            Session("ProcessSelect_C19") = Nothing
                        Else
                            'This code will only execute when user select the cancel request and he has not select the cancel reason code from popup screen and in confirmation dialogue box he confirmed Yes
                            'To do display message if reason code is empty
                            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "You can not cancel the request without selecting the reason code.", MessageBoxButtons.OK, False)
                            Session("CancelSelect_C19") = Nothing
                            Session("ProcessSelect_C19") = Nothing
                        End If
                        Me.LoadRefundRequestDetails(Me.SessionPersonID, Me.SessionFundID)

                        'This part will execute when user confirm for ProcessRefundRequest
                    ElseIf Not Session("ProcessSelect_C19") Is Nothing Then
                        Me.ProcessRefundRequest(Session("ProcessSelect_C19"))
                        Session("ProcessSelect_C19") = Nothing
                        Session("CancelSelect_C19") = Nothing
                    End If
                    'Start- chandrasekar.c:2016.01.11 YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request
                    If Not ViewState("IsExpireOption") Is Nothing Then
                        If ViewState("IsExpireOption") = True Then
                            If Not ViewState("RefundRequestUniqueID") Is Nothing Then
                                YMCARET.YmcaBusinessObject.RefundRequest.UpdateRefundRequestsExtendExpireDates(Me.SessionPersonID, ViewState("RefundRequestUniqueID").ToString())
                                Me.LoadRefundRequestDetails(Me.SessionPersonID, Me.SessionFundID)
                                Me.LoadMemberNotes(Me.SessionPersonID)
                            End If
                            ViewState("IsExpireOption") = False ' SR | 2016.03.07 | YRS-AT-2524 : After extension of expired request, cancelation is not updating refundstatus Hence once request is extended set viewstate as false
                        End If
                    End If
                    'End- chandrasekar.c:2016.01.11 YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request
                    'This part will execute when user select No in confirmation dialogue box
                ElseIf Request.Form("No") = "No" Then
                    Session("CancelSelect_C19") = Nothing
                    Session("RefCanReasonCode") = Nothing 'Shashi Shekhar:2011-25-05: YRS 5.0-1298: For Cancel code reason implementation
                    ViewState("RefCanReasonCode") = Nothing 'Shashi Shekhar:27-05-2011: For YRS 5.0-1298
                    ViewState("CanReq") = Nothing 'Shashi Shekhar:27-05-2011: For YRS 5.0-1298
                    Session("ProcessSelect_C19") = Nothing
                    Session("CancelSelect_C19") = Nothing
                    'Anudeep:12.02.2013 to show account contribution tab if any negative exists while taking refund request Bt-650-YRS 5.0-521 : Program ignoring negative amount
                ElseIf Request.Form("OK") = "OK" Then
                    If Not ViewState("Negativeamounts") Is Nothing Then
                        RefundTabStrip.SelectedIndex = 2
                        RefundMultiPage.SelectedIndex = 2
                    End If
                Else
                    If Not Session("CancelSelect_C19") Is Nothing And ViewState("CanReq") = Nothing Then
                        Me.CancelRefundRequest(Session("CancelSelect_C19"), "YRS")
                        Session("CancelSelect_C19") = Nothing
                        Session("RefCanReasonCode") = Nothing 'Shashi Shekhar:2011-25-05: YRS 5.0-1298: For Cancel code reason implementation
                        ViewState("RefCanReasonCode") = Nothing 'Shashi Shekhar:27-05-2011: For YRS 5.0-1298
                        Me.LoadRefundRequestDetails(Me.SessionPersonID, Me.SessionFundID)
                    ElseIf Not Session("ProcessSelect_C19") Is Nothing Then
                        ' Me.ProcessRefundRequest(Session("ProcessSelect"))
                        Session("ProcessSelect_C19") = Nothing
                        Session("CancelSelect_C19") = Nothing

                    End If
                End If

                'Session("CancelSelect") = Nothing
                'Session("ProcessSelect") = Nothing
                'Session("RefCanReasonCode") = Nothing 'Shashi Shekhar:2011-25-05: YRS 5.0-1298: For Cancel code reason implementation

                If Session("HardshipMessage_C19") <> Nothing Then
                    Dim popupScript1 As String
                    Dim l_stringMessage As String
                    l_stringMessage = Session("HardshipMessage_C19")
                    Session("HardshipMessage_C19") = Nothing
                    ' MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", l_stringMessage, MessageBoxButtons.Stop)
                    popupScript1 = "<script language='javascript'>" & _
                      "alert(l_stringMessage);self.close();" & _
                      "</script>"
                    If (Not Me.IsStartupScriptRegistered("PopupScript5")) Then
                        Page.RegisterStartupScript("PopupScript5", popupScript1)
                    End If
                End If
                'by aparna 06/02/2007
                'Aparna -YREN -3027
                If Me.SessionIsNotes = True Then
                    Me.LoadMemberNotes(Me.SessionPersonID)
                    Me.SessionIsNotes = False
                    Me.SessionIsNotesPopupAllowed = True
                End If

                Select Case (Me.SessionFormMode)
                    Case FormMode.View

                    Case Else
                        'Load List DataGrid
                        'Me.LoadListDataGridFromCache()

                        '' This is segment is used to Refresh the Refund Request DataGrid
                        ' Dim l_DataTable As DataTable
                        'If Me.SessionPersonID <> String.Empty And Me.SessionFundID <> String.Empty Then
                        '    l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.MemberRefundRequestDetails(Me.SessionPersonID, Me.SessionFundID)
                        '    If Not l_DataTable Is Nothing Then
                        '        For Each l_DataRow As DataRow In l_DataTable.Rows
                        '            If Not l_DataRow Is Nothing Then
                        '                If CType(l_DataRow.Item("RequestStatus"), String).Trim.ToUpper = "PEND" Then
                        '                    Me.SessionIsRefundRequest = True
                        '                End If

                        '            End If
                        '        Next
                        '    End If
                        'End If
                        If Me.SessionIsRefundRequest = True Then
                            'Me.LoadCurrentPIA(Me.SessionFundID)
                            Me.LoadRefundRequestDetails(Me.SessionPersonID, Me.SessionFundID)
                            Me.SessionIsRefundRequest = False
                            ' This is to populate the message box.
                            If Not Session("sessionProcessMessage_C19") Is Nothing Then
                                MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", Session("sessionProcessMessage_C19").ToString(), MessageBoxButtons.OK, False)
                                Session("sessionProcessMessage_C19") = Nothing
                            End If
                        End If
                        If Not Session("VolRequestCreated_C19") Is Nothing Then
                            If Session("VolRequestCreated_C19") = True Then
                                Me.LoadRefundRequestDetails(Me.SessionPersonID, Me.SessionFundID)
                                Session("VolRequestCreated_C19") = Nothing
                                MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "A Voluntary withdrawal request record has been created.  Please return to the Withdrawal Request processing screen to process the new request.", MessageBoxButtons.OK, False)
                            End If
                        End If

                        'If Me.SessionIsNotes = True Then
                        '    Me.LoadMemberNotes(Me.SessionPersonID)
                        '    Me.SessionIsNotes = False
                        '    Me.SessionIsNotesPopupAllowed = True
                        'End If

                End Select

                'Code added by ashutosh 18/04/06*****
                If Not Session("ds_PrimaryAddress_C19") Is Nothing Then
                    Me.LoadGeneralInformation(Me.SessionPersonID, Me.SessionFundID)
                    'Anudeep:22.08.2013-YRS 5.0-1862:Add notes record when user enters address in any module.
                    LoadMemberNotes(SessionPersonID)
                    'LoadFromPopUp()
                End If
                '************************************
            Else
                'Aparna -YREN 3115 15/03/2007
                'To Find if the user belongs to teh Notes Admin Group so that all the checkboxes in the notes grid be enabled
                Dim l_intLoggedUser As Integer
                Dim l_Int_UserId As Integer
                l_Int_UserId = Convert.ToInt32(Session("LoggedUserKey"))
                l_intLoggedUser = YMCARET.YmcaBusinessObject.SecurityCheckBOClass.GetLoginNotesUser(l_Int_UserId)

                If l_intLoggedUser = 1 Then
                    Me.NotesGroupUser = True
                Else
                    Me.NotesGroupUser = False
                End If
                'Aparna -YREN 3115 15/03/2007

                '*******Code Added by Ashutosh on 3-05-06***********
                Session("ds_PrimaryAddress_C19") = Nothing

                '********************************************
                Session("RefundRequest_Sort") = Nothing

                'Me.DataGridList.DataSource = Nothing
                'Me.DataGridList.DataBind()

                Me.DataGridEmployment.DataSource = Nothing
                Me.DataGridEmployment.DataBind()

                Me.DataGridFundedAccountContributions.DataSource = Nothing
                Me.DataGridFundedAccountContributions.DataBind()

                Me.DataGridNonFundedAccountContributions.DataSource = Nothing
                Me.DataGridNonFundedAccountContributions.DataBind()

                Menu1.DataSource = Server.MapPath("SimpleXML.xml")
                Menu1.DataBind()

                Me.DataGridRequests.DataSource = Nothing
                Me.DataGridRequests.DataBind()

                Me.DataGridRefundNotes.DataSource = Nothing
                Me.DataGridRefundNotes.DataBind()

                'Me.LabelListSSNo.AssociatedControlID = Me.TextBoxListSSNo.ID
                'Me.LabelListFundNo.AssociatedControlID = Me.TextBoxListFundNo.ID
                'Me.LabelLastName.AssociatedControlID = Me.TextBoxLastName.ID
                'Me.LabelFirstName.AssociatedControlID = Me.TextBoxFirstName.ID

                Me.LabelSSNo.AssociatedControlID = Me.TextBoxSSNo.ID
                Me.LabelFundNo.AssociatedControlID = Me.TextBoxFundNo.ID
                Me.LabelSal.AssociatedControlID = Me.DropDownListSal.ID
                Me.LabelFirst.AssociatedControlID = Me.TextBoxFirst.ID
                Me.LabelMiddle.AssociatedControlID = Me.TextBoxMiddle.ID

                Me.LabelLast.AssociatedControlID = Me.TextBoxLast.ID
                Me.LabelSuffix.AssociatedControlID = Me.TextBoxSuffix.ID
                'Me.LabelAddress1.AssociatedControlID = Me.TextBoxAddress1.ID
                'Me.LabelAddress2.AssociatedControlID = Me.TextBoxAddress2.ID

                'Me.LabelAddress3.AssociatedControlID = Me.TextBoxAddress3.ID
                'Me.LabelCity.AssociatedControlID = Me.TextBoxCity.ID
                'Me.LabelState.AssociatedControlID = Me.TextBoxState.ID
                'Me.LabelZip.AssociatedControlID = Me.TextBoxZip.ID
                'Me.LabelCountry.AssociatedControlID = Me.TextBoxCountry.ID
                Me.AddressWebUserControl1.EnableControls = False
                Me.AddressWebUserControl1.MakeReadonly = True
                Me.LabelTelephone.AssociatedControlID = Me.LabelTelephone.ID
                Me.LabelUpdatedBy.AssociatedControlID = Me.TextBoxUpdatedBy.ID
                Me.LabelEmail.AssociatedControlID = Me.TextBoxEmail.ID

                Me.LabelUpdatedDate.AssociatedControlID = Me.TextBoxUpdatedDate.ID
                Me.LabelVested.AssociatedControlID = Me.TextBoxVested.ID
                Me.LabelYear.AssociatedControlID = Me.TextBoxYear.ID
                Me.LabelMonth.AssociatedControlID = Me.LabelMonth.ID
                Me.LabelPIATermination.AssociatedControlID = Me.TextBoxPIATermination.ID
                Me.LabelCurrentPIA.AssociatedControlID = Me.TextBoxCurrentPIA.ID
                'BA Account YMCA PhaseV 03-04-2009  by Dilip
                Me.LabelBATermination.AssociatedControlID = Me.TextBoxBATermination.ID
                Me.LabelBACurrentPIA.AssociatedControlID = Me.TextBoxBACurrent.ID
                'BA Account YMCA PhaseV 03-04-2009  by Dilip
                'Me.LabelTitle.Text = String.Empty
                Me.TextBoxSSNo.MaxLength = 9

                '' Set the Refund Type incase, this form is calling from the 
                '' Special & Disablity Refund Menu.

                If Not Page.Request.QueryString("RT") Is Nothing Then
                    Me.RefundType = CType(Page.Request.QueryString("RT"), String)
                    'Rahul
                    If Me.RefundType.Trim = "SPEC" Then
                        'Me.LabelTitle.Text = "Special Withdrawal Request Maintenance"
                        HeaderControl.PageTitle = "Special Withdrawal Request Maintenance"
                    ElseIf Me.RefundType.Trim = "DISAB" Then
                        'Me.LabelTitle.Text = "Disability Withdrawal Request Maintenance"
                        HeaderControl.PageTitle = "Disability Withdrawal Request Maintenance"
                    Else
                        'Me.LabelTitle.Text = "Regular Withdrawal Request Maintenance"
                        HeaderControl.PageTitle = "Regular Withdrawal Request Maintenance"
                    End If
                Else
                    Me.RefundType = String.Empty
                    'Me.LabelTitle.Text = "Regular Withdrawal Request Maintenance"
                    HeaderControl.PageTitle = "Regular Withdrawal COVID19 Request Maintenance"
                End If
                'Rahul
                '' Diable TabStrip Items. 
                Me.RefundTabStrip.Items.Item(0).Enabled = False
                Me.RefundTabStrip.Items.Item(1).Enabled = False
                Me.RefundTabStrip.Items.Item(2).Enabled = False
                Me.RefundTabStrip.Items.Item(3).Enabled = False
                Me.RefundTabStrip.Items.Item(4).Enabled = False

                '' Set TextBoxes Read Only mode.
                Me.SetTextBoxReadOnly()
                'Anudeep:29.10.2013-BT-2278:YRS 5.0-2236 :Added to get the data when Enter key is pressed while finding person
                'Me.TextBoxListFundNo.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
                'Me.TextBoxListSSNo.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
                'Me.TextBoxLastName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
                'Me.TextBoxFirstName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
                'AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Added to load all tabs on page load
                LoadAllTabs()
            End If

            If Request.Form("OK") = "OK" Then
                ' Refresh the DataGrid 

                If Me.SessionIsRefundRequest = True Then

                    Me.LoadRefundRequestDetails(Me.SessionPersonID, Me.SessionFundID)
                    Me.LoadAccountContribution(Me.SessionPersonID) ' After cancel teh refund we need to refresh the account contribution.
                    Me.LoadMemberNotes(Me.SessionPersonID)

                    Me.SessionIsRefundRequest = False
                End If

                '' Load List DataGrid.
                'Me.LoadListDataGridFromCache()
            End If

            If Not Session("IndexFileWriteError_C19") Is Nothing Then
                If Session("IndexFileWriteError_C19").GetType.ToString().Length > 0 Then
                    MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", Session("IndexFileWriteError_C19"), MessageBoxButtons.Stop)
                    Session("IndexFileWriteError_C19") = Nothing
                End If
            End If
            'Start:AA:02.17.2016 YRS-AT-2640 Added to open the withdrawal processing pop-up window
            If Not IsPostBack Then
                If Session("Seamless_refrequestid_C19") IsNot Nothing Then
                    RefundTabStrip.SelectedIndex = 3
                    RefundMultiPage.SelectedIndex = 3
                    Session("Seamless_refrequestid_C19") = Nothing
                End If
                'End:AA:02.17.2016 YRS-AT-2640 Added to open the withdrawal processing pop-up window
            End If
            Session("GenerateErrors_C19") = Nothing 'MMR | 2020.05.06 | YRS-AT-4854 | Clearing session value to avoid redirection to error page on post backs
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("RefundRequestForm_C19_Page_Load", ex) 'PK | 10/01/2019 | YRS-AT-2670 | Added exception logging
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        Finally
            ViewState("WD_RefunType") = Nothing
        End Try
    End Sub
    'Function made by ashutosh 18/04/06****************
    Private Function LoadFromPopUp()
        'Ashutosh Patil as on 08-Apr-2007
        'YREN-3028,YREN-3029
        If Not Session("ds_PrimaryAddress_C19") Is Nothing Then
            Try
                Dim dataset_AddressInfo As DataSet
                Dim l_DataSet As DataSet
                Dim l_DataTable As DataTable
                Dim datarow_Row As DataRow
                Dim datarow_NewRow As DataRow
                Dim l_str_zipcode As String
                dataset_AddressInfo = (DirectCast(Session("ds_PrimaryAddress_C19"), DataSet))
                If dataset_AddressInfo.Tables("Address").Rows.Count > 0 Then
                    datarow_Row = dataset_AddressInfo.Tables("Address").Rows(0)
                    'TextBoxAddress1.Text = datarow_Row.Item("Address1").ToString()
                    'TextBoxAddress2.Text = datarow_Row.Item("Address2").ToString()
                    'TextBoxAddress3.Text = datarow_Row.Item("Address3").ToString()
                    'TextBoxCity.Text = datarow_Row.Item("City").ToString()
                    'TextBoxState.Text = datarow_Row.Item("StateName").ToString() 'datarow_Row.Item("state").ToString()
                    'TextBoxCountry.Text = datarow_Row.Item("CountryName").ToString() 'datarow_Row.Item("Country").ToString()
                    'If datarow_Row.Item("CountryName").ToString() = "CANADA" Then
                    '    l_str_zipcode = datarow_Row.Item("Zip").ToString()
                    '    TextBoxZip.Text = l_str_zipcode.Substring(0, 3) + " " + l_str_zipcode.Substring(3, 3)
                    'Else
                    '    TextBoxZip.Text = datarow_Row.Item("Zip").ToString()
                    'End If

                    Me.AddressWebUserControl1.LoadAddressDetail(dataset_AddressInfo.Tables("Address").Select("isPrimary = True"))
                    'by aparna -YREN-3036

                    Me.TextBoxUpdatedBy.Text = CType(datarow_Row.Item("Updater"), String)
                    Me.TextBoxUpdatedDate.Text = CType(datarow_Row.Item("Updated"), String)

                    'By Aparna YREN-3036
                End If
                If dataset_AddressInfo.Tables("TelephoneInfo").Rows.Count > 0 Then
                    datarow_Row = dataset_AddressInfo.Tables("TelephoneInfo").Rows(0)
                    TextBoxTelephone.Text = datarow_Row.Item("PhoneNumber").ToString()
                End If
                dataset_AddressInfo = Nothing
            Catch ex As Exception
                Dim l_String_Exception_Message As String
                l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
                Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
            End Try
            Session("ds_PrimaryAddress_C19") = Nothing
        End If
    End Function
    '*******************************************************
    Private Sub SetTextBoxReadOnly()
        Try
            Me.TextBoxSSNo.ReadOnly = True
            Me.TextBoxFundNo.ReadOnly = True
            Me.DropDownListSal.Enabled = False
            Me.TextBoxFirst.ReadOnly = True
            Me.TextBoxMiddle.ReadOnly = True
            Me.TextBoxLast.ReadOnly = True
            Me.TextBoxSuffix.ReadOnly = True
            'Me.TextBoxAddress1.ReadOnly = True
            'Me.TextBoxAddress2.ReadOnly = True
            'Me.TextBoxAddress3.ReadOnly = True
            'Me.TextBoxCity.ReadOnly = True
            'Me.TextBoxState.ReadOnly = True
            'Me.TextBoxZip.ReadOnly = True
            'Me.TextBoxCountry.ReadOnly = True

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
            'BA Account YMCA PhaseV 03-04-2009  by Dilip
            Me.TextBoxBATermination.ReadOnly = True
            Me.TextBoxBACurrent.ReadOnly = True
            'BA Account YMCA PhaseV 03-04-2009  by Dilip
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Sub RefundMultiPage_SelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefundMultiPage.SelectedIndexChange

    End Sub

    Private Sub RefundTabStrip_SelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefundTabStrip.SelectedIndexChange
        Try
            Me.RefundMultiPage.SelectedIndex = Me.RefundTabStrip.SelectedIndex

            '' To Bind the DataSource to List DataGrid.
            'Me.LoadListDataGridFromCache()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Start:AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Commented below code because list tab has been removed and searching person will be done in findinfo.aspx
    'Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
    '    Try
    '        'Plan split changes
    '        Me.ISMarket = 1
    '        Session("RetirementPlanAcctContribution") = Nothing
    '        Session("SavingsPlanAcctContribution") = Nothing
    '        Session("RetirementPlan_UnfundedContributions") = Nothing
    '        Session("SavingsPlan_UnfundedContributions") = Nothing
    '        Session("DisplayRetirementPlanAcctContribution") = Nothing
    '        Session("DisplaySavingsPlanAcctContribution") = Nothing
    '        Session("RefundState") = Nothing
    '        'Added for the issue in hardship when being done after processing of terminated person-Parveen Kumar

    '        'Added for the issue in hardship when being done after processing of terminated person-Parveen Kumar
    '        'Added to hide SSN and Name  in Withdrawal Request Maintenance screen. March 25 2010 Amit Nigam
    '        'If Me.RefundType.Trim = "SPEC" Then
    '        '    'Me.LabelTitle.Text = "Special Withdrawal Request Maintenance"
    '        '    HeaderControl.PageTitle = "Special Withdrawal Request Maintenance"
    '        'ElseIf Me.RefundType.Trim = "DISAB" Then
    '        '    'Me.LabelTitle.Text = "Disability Withdrawal Request Maintenance"
    '        '    HeaderControl.PageTitle = "Disability Withdrawal Request Maintenance"
    '        'Else
    '        '    'Me.LabelTitle.Text = "Regular Withdrawal Request Maintenance"
    '        '    HeaderControl.PageTitle = "Regular Withdrawal Request Maintenance"
    '        'End If
    '        ''Added to hide SSN and Name  in Withdrawal Request Maintenance screen. March 25 2010 Amit Nigam
    '        'TextBoxListSSNo.Text = TextBoxListSSNo.Text.Replace("-", "")
    '        ''Plan split changes
    '        'If Me.TextBoxListSSNo.Text.Trim.Length < 1 And _
    '        '    Me.TextBoxListFundNo.Text.Trim.Length < 1 And _
    '        '    Me.TextBoxLastName.Text.Trim.Length < 1 And _
    '        '    Me.TextBoxFirstName.Text.Trim.Length < 1 Then

    '        '    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Please Enter Search Criteria.", MessageBoxButtons.Stop, False)

    '        '    Return

    '        'End If
    '        'Anudeep:2013.07.02:Bt-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    '        Me.SearchPerson(Me.TextBoxListSSNo.Text.Trim, Me.TextBoxListFundNo.Text.Trim, Me.TextBoxLastName.Text.Trim, Me.TextBoxFirstName.Text.Trim, Me.AddressWebUserControl1.City, Me.AddressWebUserControl1.DropDownListStateValue)
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try

    'End Sub

    'Private Function SearchPerson(ByVal parameterSSNo As String, ByVal parameterFundNo As String, ByVal paramterLastName As String, ByVal parameterFirstName As String, ByVal parameterCity As String, ByVal parameterState As String)

    '    Dim l_DataSet As DataSet
    '    'Hafiz 03Feb06 Cache-Session
    '    'Dim l_Cache As CacheManager
    '    'Hafiz 03Feb06 Cache-Session

    '    Try

    '        l_DataSet = YMCARET.YmcaBusinessObject.RefundRequest.LookupPerson(parameterSSNo, parameterFundNo, paramterLastName, parameterFirstName, "Refund", parameterCity, parameterState)
    '        ViewState("RefundRequest_Sort") = l_DataSet
    '        'DataGridList.DataSource = l_DataSet
    '        'DataGridList.DataBind()
    '        ''CommonModule.HideColumnsinDataGrid(l_DataSet, Me.DataGridList, "PersonID, guiFundEventId")

    '        'Hafiz 03Feb06 Cache-Session
    '        ' Adding the Dataset into Cache
    '        'l_Cache = CacheFactory.GetCacheManager
    '        'l_Cache.Add("FindPerson", l_DataSet)
    '        Session("FindPerson") = l_DataSet
    '        'Hafiz 03Feb06 Cache-Session


    '        ' Set the selecte index for keeping session RowIndex, if dataset is not empty.
    '        If Not l_DataSet Is Nothing Then
    '            If l_DataSet.Tables.Count > 0 Then
    '                Me.ListSessionRowIndex = 0
    '            Else

    '                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "No Record Found.", MessageBoxButtons.OK, False)

    '                Me.ListSessionRowIndex = -1
    '                Me.SessionPersonID = String.Empty
    '            End If

    '        Else
    '            Me.ListSessionRowIndex = -1
    '        End If

    '        Me.DataGridList.SelectedIndex = -1

    '        Me.RefundTabStrip.Items.Item(1).Enabled = False
    '        Me.RefundTabStrip.Items.Item(2).Enabled = False
    '        Me.RefundTabStrip.Items.Item(3).Enabled = False
    '        Me.RefundTabStrip.Items.Item(4).Enabled = False
    '        Me.RefundTabStrip.Items.Item(5).Enabled = False

    '        Me.DataGridEmployment.DataSource = Nothing
    '        Me.DataGridRequests.DataSource = Nothing
    '        Me.DataGridRefundNotes.DataSource = Nothing
    '        Me.DataGridFundedAccountContributions.DataSource = Nothing
    '        Me.DataGridNonFundedAccountContributions.DataSource = Nothing

    '        'Me.DataGridList.AllowSorting = True

    '    Catch ex As Exception

    '        MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", ex.Message, MessageBoxButtons.OK, False)

    '        'If ex.Message.Equals("No record(s) found for the specified 'criteria'.") Then
    '        '    message()
    '        '    Me.DataGridList.DataSource = Nothing
    '        'Else
    '        '    Response.Redirect("ErrorPageForm.aspx")
    '        'End If

    '    End Try

    'End Function

    'Private Function LoadListDataGridFromCache()

    '    Dim l_DataSet As DataSet
    '    'Hafiz 03Feb06 Cache-Session
    '    'Dim l_Cache As CacheManager
    '    'Hafiz 03Feb06 Cache-Session

    '    Try
    '        'Hafiz 03Feb06 Cache-Session
    '        'l_Cache = CacheFactory.GetCacheManager
    '        'l_DataSet = CType(l_Cache.GetData("FindPerson"), DataSet)
    '        l_DataSet = DirectCast(Session("FindPerson"), DataSet)
    '        'Hafiz 03Feb06 Cache-Session

    '        ViewState("RefundRequest_Sort") = l_DataSet
    '        '' CommonModule.HideColumnsinDataGrid(l_DataSet, Me.DataGridList, "PersonID, guiFundEventId")
    '        'Changed By:preeti On:10thFeb06 IssueId:YRST-2092 start 
    '        'no need to bind data again as sorting is getting cleared up after this
    '        'DataGridList.DataSource = l_DataSet
    '        'DataGridList.DataBind()
    '        'Changed By:preeti On:10thFeb06 IssueId:YRST-2092 end

    '    Catch
    '        Throw
    '    End Try
    'End Function

    'Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
    '    Try
    '        Me.TextBoxFirstName.Text = String.Empty
    '        Me.TextBoxLastName.Text = String.Empty
    '        Me.TextBoxListFundNo.Text = String.Empty
    '        Me.TextBoxListSSNo.Text = String.Empty
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub

    'Private Sub DataGridList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridList.SelectedIndexChanged
    '    Dim l_bool_readonly As Boolean
    '    'Load the employee details for corresponding selected employee.
    '    Try
    '        '----Shashi Shekhar:2010-02-12: Code to handle Archived Participants from list--------------
    '        If Me.DataGridList.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
    '            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
    '            'Disabling Tabs
    '            Me.RefundTabStrip.Items.Item(1).Enabled = False
    '            Me.RefundTabStrip.Items.Item(2).Enabled = False
    '            Me.RefundTabStrip.Items.Item(3).Enabled = False
    '            Me.RefundTabStrip.Items.Item(4).Enabled = False
    '            Me.RefundTabStrip.Items.Item(5).Enabled = False
    '            'Clear Title
    '            'LabelTitle.Text = ""
    '            HeaderControl.PageTitle = String.Empty
    '            Exit Sub
    '        End If
    '        '---------------------------------------------------------------------------------------


    '        ' Clear the Session Variable for RefundRequestID & FundID
    '        Me.SessionPersonID = String.Empty
    '        Me.SessionFundID = String.Empty
    '        Me.IsAccountLock = Nothing 'SS:10 feb 2011:For YRS 5.0-1236
    '        'Plan split changes
    '        'Neeraj 26-Oct-2009 for issue id YRS 5.0-932 :  addded below line of code to make Session("MinimumDistributionTable") as null

    '        'END: SG: BT-1012(Re-Opened): 2012.06.01

    '        'Plan split changes
    '        'Me.LoadListDataGridFromCache()

    '        Me.SessionPersonID = Me.GetSelectedPersonID(Me.DataGridList.SelectedIndex)
    '        Me.SessionFundID = Me.GetSelectedFundID(Me.DataGridList.SelectedIndex)
    '        Me.IsAccountLock = Me.GetSelectedAccountLockStatus(Me.DataGridList.SelectedIndex) 'SS:10 feb 2011:For YRS 5.0-1236



    '        If Me.SessionPersonID = String.Empty Then
    '            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", " Please select a Member to view the Details.", MessageBoxButtons.Stop, False)

    '            Me.RefundTabStrip.Items.Item(1).Enabled = False
    '            Me.RefundTabStrip.Items.Item(2).Enabled = False
    '            Me.RefundTabStrip.Items.Item(3).Enabled = False
    '            Me.RefundTabStrip.Items.Item(4).Enabled = False
    '            Me.RefundTabStrip.Items.Item(5).Enabled = False

    '            Return
    '        End If

    '        'Me.LoadGeneralInformation(Me.SessionPersonID, Me.SessionFundID)

    '        'If Me.SessionFundID = String.Empty Then
    '        '    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "There is no Employment Present for this Member." & vbCrLf & vbCrLf & "Details of Selected Member could not be loaded.", MessageBoxButtons.Stop, False)

    '        '    Me.RefundTabStrip.Items.Item(1).Enabled = False
    '        '    Me.RefundTabStrip.Items.Item(2).Enabled = False
    '        '    Me.RefundTabStrip.Items.Item(3).Enabled = False
    '        '    Me.RefundTabStrip.Items.Item(4).Enabled = False
    '        '    Me.RefundTabStrip.Items.Item(5).Enabled = False

    '        'Else
    '        '    Me.LoadAccountContribution(Me.SessionFundID)
    '        '    Me.LoadRefundRequestDetails(Me.SessionPersonID, Me.SessionFundID)
    '        '    Me.LoadMemberNotes(Me.SessionPersonID)
    '        '    Me.LoadCurrentPIA(Me.SessionFundID)

    '        '    '' Enable Notes PopupWindow
    '        '    Me.SessionIsNotesPopupAllowed = True

    '        '    '' Enable TabPages in the Page.
    '        '    Me.RefundTabStrip.Items.Item(1).Enabled = True
    '        '    Me.RefundTabStrip.Items.Item(2).Enabled = True
    '        '    Me.RefundTabStrip.Items.Item(3).Enabled = True
    '        '    Me.RefundTabStrip.Items.Item(4).Enabled = True
    '        '    Me.RefundTabStrip.Items.Item(5).Enabled = True
    '        'End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub

    'Private Function GetSelectedPersonID(ByVal index As Integer) As String

    '    Dim l_DataRow As DataRow
    '    Dim l_DataSet As DataSet
    '    Dim l_DataTable As DataTable
    '    'Hafiz 03Feb06 Cache-Session
    '    'Dim l_Cache As CacheManager
    '    'Hafiz 03Feb06 Cache-Session

    '    Try

    '        'Hafiz 03Feb06 Cache-Session
    '        'l_Cache = CacheFactory.GetCacheManager
    '        'l_DataSet = CType(l_Cache.GetData("FindPerson"), DataSet)
    '        l_DataSet = DirectCast(Session("FindPerson"), DataSet)
    '        'Hafiz 03Feb06 Cache-Session

    '        If Not l_DataSet Is Nothing Then
    '            l_DataTable = l_DataSet.Tables("Persons")

    '            If Not l_DataTable Is Nothing Then
    '                'Changed By:preeti On:10thFeb06 IssueId:YRST-2092 start
    '                Dim arrDr() As DataRow = l_DataTable.Select("[guiFundEventId]='" & Me.DataGridList.SelectedItem.Cells(5).Text.Trim() & "'")
    '                'Dim arrDr() As DataRow = l_DataTable.Select("[SS No]=" & Me.DataGridList.SelectedItem.Cells(1).Text.Trim())
    '                'l_DataRow = l_DataTable.Rows.Item(index)
    '                If arrDr.Length > 0 Then
    '                    l_DataRow = arrDr(0)
    '                Else
    '                    Exit Function
    '                End If
    '                'Changed By:preeti On:10thFeb06 IssueId:YRST-2092 End



    '                If Not l_DataRow Is Nothing Then
    '                    Me.SessionPersonID = CType(l_DataRow("PersonID"), String)
    '                    Return (CType(l_DataRow("PersonID"), String))
    '                Else
    '                    Me.SessionPersonID = String.Empty
    '                    Return String.Empty
    '                End If

    '            End If

    '        End If


    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'End:AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Commented below code because list tab has been removed and searching person will be done in findinfo.aspx

    Private Function LoadGeneralInformationFromCache(ByVal parameterPersonID As String)

    End Function

    Private Function LoadGeneralInformation(ByVal parameterPersonID As String, ByVal parameterFundEventID As String)

        Dim l_DataSet As DataSet = Nothing
        Dim l_DataTable As DataTable = Nothing
        Dim l_integer_QDROParticipantAge As Integer
        Dim l_address_Dataset As DataSet
        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session

        Try
            l_DataSet = YMCARET.YmcaBusinessObject.RefundRequest.LookupMemberDetails(parameterPersonID, parameterFundEventID)

            If l_DataSet Is Nothing Then Return 0

            ' To Fill general Information
            l_DataTable = l_DataSet.Tables("Member Details")

            If Not l_DataTable Is Nothing Then
                If l_DataTable.Rows.Count > 0 Then
                    Me.LoadGeneralInfoToControls(l_DataTable.Rows.Item(0))
                    Session("MaritalStatusCode_C19") = l_DataTable.Rows(0).Item(13).ToString
                End If
            End If

            l_DataTable = l_DataSet.Tables("Member Address")
            ' To Fill Address Inforamation in General Tab Page
            If Not l_DataTable Is Nothing Then
                If l_DataTable.Rows.Count > 0 Then
                    Me.LoadAddressInfoToControls(l_DataTable.Rows.Item(0))
                End If
            End If
            'Anudeep
            l_address_Dataset = Address.GetAddressByEntity(parameterPersonID, EnumEntityCode.PERSON)
            l_DataTable = l_address_Dataset.Tables("Address")
            If Not l_DataTable Is Nothing Then
                If l_DataTable.Rows.Count > 0 Then
                    AddressWebUserControl1.LoadAddressDetail(l_DataTable.Select("isPrimary = True"))
                Else
                    AddressWebUserControl1.LoadAddressDetail(Nothing)
                End If
            End If

            ' To fill Employment Information 
            l_DataTable = l_DataSet.Tables("Member Employment")
            Session("Member Employment_C19") = l_DataTable
            If Not l_DataTable Is Nothing Then
                LoadEmploymentDetails(l_DataTable, l_DataSet)
            End If

            'Aparna 25/10/2007
            If Me.SessionStatusType.ToUpper.Trim() = "QD" Then
                l_integer_QDROParticipantAge = YMCARET.YmcaBusinessObject.RefundRequest.GetQDROParticipantAge(parameterFundEventID)
                'If l_integer_QDROParticipantAge > 0 Then
                Session("QDROParticipantAge_C19") = l_integer_QDROParticipantAge
                ' End If
            End If
            'Aparna 25/10/2007

            '' Add DataSet Into Cache
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'l_CacheManager.Add("PersonInformation", l_DataSet)
            Session("PersonInformation_C19") = l_DataSet
            'Hafiz 03Feb06 Cache-Session

            'Start: Bala: 01/19/2019: YRS-AT-2398: Customer Service requires a Special Handling alert for officers
            l_DataSet = YMCARET.YmcaBusinessObject.RefundRequest.GetSpecialHandlingDetails(parameterPersonID)
            l_DataTable = l_DataSet.Tables("SpecialHandlingDetails")
            'If Not l_DataTable Is Nothing Then 'Bala: 03.16.2016: YRS-AT-2398: Commented.
            If HelperFunctions.isNonEmpty(l_DataTable) Then 'Bala: 03.16.2016: YRS-AT-2398: Check if the table has no records.
                If Convert.ToBoolean(l_DataTable.Rows(0).Item("RequireSpecialHandling").ToString) Then
                    Me.LabelSpecialHandling.Text = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWALS_SPECIAL_HANDLING).DisplayText
                    Me.LabelSpecialHandling.Visible = True
                    Me.HiddenFieldOfficersDetails.Value = l_DataTable.Rows(0).Item("EmploymentDetail").ToString
                End If
            End If
            'End: Bala: 01/19/2019: YRS-AT-2398: Customer Service requires a Special Handling alert for officers
        Catch
            Throw
        End Try
    End Function

    Private Function LoadEmploymentDetails(ByVal parameterDataTable As DataTable, ByVal parameterDataSet As DataSet)
        Try
            If Not parameterDataTable Is Nothing Then
                'datagridCheckBoxStatusType = New Infotech.DataGridCheckBox.CheckBoxColumn(False, "Status Type")
                'datagridCheckBoxStatusType.DataField = "StatusType"
                'datagridCheckBoxStatusType.AutoPostBack = False

                'Me.DataGridEmployment.Columns.Add(datagridCheckBoxStatusType)

                Me.DataGridEmployment.DataSource = parameterDataTable
                Me.DataGridEmployment.DataBind()

                '' To set the Fund Event ID
                'StatusType datagridCheckBoxStatusType

                'If parameterDataTable.Rows.Count > 0 Then
                '    Me.SetFundID(parameterDataTable.Rows(0))
                'End If

            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Private Function LoadAddressInfoToControls(ByVal parameterDataRow As DataRow)

        Dim l_str_zipcode As String


        Try

            If Not parameterDataRow Is Nothing Then

                ''If (parameterDataRow("Address1").ToString = "") Then
                ''    Me.TextBoxAddress1.Text = String.Empty
                ''Else
                ''    Me.TextBoxAddress1.Text = CType(parameterDataRow("Address1"), String).Trim
                ''End If

                ''If (parameterDataRow("Address2").ToString = "") Then
                ''    Me.TextBoxAddress2.Text = String.Empty
                ''Else
                ''    Me.TextBoxAddress2.Text = CType(parameterDataRow("Address2"), String).Trim
                ''End If

                ''If (parameterDataRow("Address3").ToString = "") Then
                ''    Me.TextBoxAddress3.Text = String.Empty
                ''Else
                ''    Me.TextBoxAddress3.Text = CType(parameterDataRow("Address3"), String).Trim
                ''End If

                ''Commented By Ashutosh Patil as on 05-Apr-2007
                ''YREN-3028,YREN-3029
                ''If (parameterDataRow("StateType").ToString = "") Then
                ''    Me.TextBoxState.Text = String.Empty
                ''Else
                ''    Me.TextBoxState.Text = CType(parameterDataRow("StateType"), String).Trim
                ''End If
                ''Modified By Ashutosh Patil as on 05-Apr-2007
                ''YREN-3028,YREN-3029
                'If (parameterDataRow("StateName").ToString = "") Then
                '    Me.TextBoxState.Text = String.Empty
                'Else
                '    Me.TextBoxState.Text = CType(parameterDataRow("StateName"), String).Trim
                'End If

                'If (parameterDataRow("City").ToString = "") Then
                '    Me.TextBoxCity.Text = String.Empty
                'Else
                '    Me.TextBoxCity.Text = CType(parameterDataRow("City"), String).Trim
                'End If

                ''Commented By Ashutosh Patil as on 05-Apr-2007
                ''YREN-3028,YREN-3029
                ''If (parameterDataRow("Country").ToString = "") Then
                ''    Me.TextBoxCountry.Text = String.Empty
                ''Else
                ''    Me.TextBoxCountry.Text = CType(parameterDataRow("Country"), String).Trim
                ''End If
                ''Modified By Ashutosh Patil as on 05-Apr-2007
                ''YREN-3028,YREN-3029
                'If (parameterDataRow("CountryName").ToString = "") Then
                '    Me.TextBoxCountry.Text = String.Empty
                'Else
                '    Me.TextBoxCountry.Text = CType(parameterDataRow("CountryName"), String).Trim
                '    If (parameterDataRow("Zip").ToString = "") Then
                '        Me.TextBoxZip.Text = String.Empty
                '    Else
                '        If parameterDataRow("CountryName").ToString = "CANADA" Then
                '            l_str_zipcode = CType(parameterDataRow("Zip"), String).Trim
                '            Me.TextBoxZip.Text = l_str_zipcode.Substring(0, 3) + " " + l_str_zipcode.Substring(3, 3)
                '        Else
                '            Me.TextBoxZip.Text = CType(parameterDataRow("Zip"), String).Trim
                '        End If
                '    End If
                'End If

                If (parameterDataRow("Email").ToString = "") Then
                    Me.TextBoxEmail.Text = String.Empty
                Else
                    Me.TextBoxEmail.Text = CType(parameterDataRow("Email"), String)
                End If

                If (parameterDataRow("PhoneNumber").ToString = "") Then
                    Me.TextBoxTelephone.Text = String.Empty
                Else
                    Me.TextBoxTelephone.Text = CType(parameterDataRow("PhoneNumber"), String)
                End If
                'by Aparna -YREN-3036

                If (parameterDataRow("Updater").ToString = "") Then
                    Me.TextBoxUpdatedBy.Text = String.Empty
                Else
                    Me.TextBoxUpdatedBy.Text = CType(parameterDataRow("Updater"), String)
                End If

                If (parameterDataRow("Updated").ToString = "") Then
                    Me.TextBoxUpdatedDate.Text = String.Empty
                Else
                    Me.TextBoxUpdatedDate.Text = CType(parameterDataRow("Updated"), String)
                End If

                'by Aparna -YREN-3036
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Function

    Private Function CalculateAge(ByVal parameterDOB As String, ByVal parameterDOD As String) As Decimal
        Dim numTotalNumberofDays As Decimal
        Dim numAge As Decimal
        Dim numReminder As Decimal
        Try

            'Commented the code for to get the Accurate date difference in this function on 24-06-2009 based on Hafiz mail
            'If parameterDOB = String.Empty Then Return 0.0

            'If parameterDOD = String.Empty Then
            '    Return (CType(DateDiff(DateInterval.Month, CType(parameterDOB, DateTime), Now.Date), Decimal) / 12.0)
            'Else
            '    Return ((CType(DateDiff(DateInterval.Year, CType(parameterDOB, DateTime), CType(parameterDOD, DateTime)), Decimal)) / 12.0)
            'End If
            If parameterDOB = String.Empty Then Return 0
            numTotalNumberofDays = DateDiff(DateInterval.Day, CType(parameterDOB, DateTime), Now.Date)
            numReminder = (numTotalNumberofDays Mod 365.2524)
            If numReminder > 0 Then
                numAge = Math.Floor((numTotalNumberofDays - numReminder) / 365.2425) + (Math.Floor(numReminder / 30.5) / 100)
            Else
                numAge = Math.Floor((numTotalNumberofDays - numReminder) / 365.2425)
            End If
            CalculateAge = numAge
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Function

    Private Function LoadGeneralInfoToControls(ByVal parameterDataRow As DataRow)
        Dim l_personage As Decimal
        Dim l_QDROParticipantAge As Decimal
        Try
            'Code Added By Ashutosh****************
            Session("ds_PrimaryAddress_C19") = Nothing
            '***************************************
            If Not parameterDataRow Is Nothing Then
                'Code Edited by ashutosh on 23-05-06*********
                'Me.TextBoxSSNo.Text = CType(parameterDataRow("SS No"), String)
                'Me.TextBoxFundNo.Text = CType(parameterDataRow("Fund No"), String)
                'Me.TextBoxLast.Text = CType(parameterDataRow("Last Name"), String)
                'Me.TextBoxMiddle.Text = CType(parameterDataRow("Middle Name"), String)
                'Me.TextBoxFirst.Text = CType(parameterDataRow("First Name"), String)
                '***************************************************
                Me.TextBoxSSNo.Text = parameterDataRow("SS No").ToString()
                Me.TextBoxFundNo.Text = parameterDataRow("Fund No").ToString()
                'by Aparna 15/10/2007
                Session("FundNo") = parameterDataRow("Fund No").ToString()
                Me.TextBoxLast.Text = parameterDataRow("Last Name").ToString()
                Me.TextBoxMiddle.Text = parameterDataRow("Middle Name").ToString()
                Me.TextBoxFirst.Text = parameterDataRow("First Name").ToString()
                'Name:Preeti Date:10th Feb 06 Issue :YMCA-1858 Start
                Dim strSSN As String = Me.TextBoxSSNo.Text.Insert(3, "-")
                strSSN = strSSN.Insert(6, "-")
                '----------------------------------------------------------------------------------------
                ' Shashi Shekhar: 2010-01-03: For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
                'Rahul
                'If Me.RefundType.Trim = "SPEC" Then
                '    Me.LabelTitle.Text = "Special Withdrawal Request Maintenance" + "--" + Me.TextBoxLast.Text + ", " + Me.TextBoxFirst.Text + " " + Me.TextBoxMiddle.Text.Trim + ",SS#: " + strSSN
                'ElseIf Me.RefundType.Trim = "DISAB" Then
                '    Me.LabelTitle.Text = "Disability Withdrawal Request Maintenance" + "--" + Me.TextBoxLast.Text + ", " + Me.TextBoxFirst.Text + " " + Me.TextBoxMiddle.Text.Trim + ",SS#: " + strSSN
                'Else
                '    Me.LabelTitle.Text = "Regular Withdrawal Request Maintenance" + "--" + Me.TextBoxLast.Text + ", " + Me.TextBoxFirst.Text + " " + Me.TextBoxMiddle.Text.Trim + ",SS#: " + strSSN
                'End If
                Dim strFundNo As String = String.Empty
                If ((parameterDataRow("Fund No").ToString() <> "System.dbnull") And (parameterDataRow("Fund No").ToString() <> "")) Then
                    strFundNo = parameterDataRow("Fund No").ToString().Trim()
                End If

                If Me.RefundType.Trim = "SPEC" Then
                    'Me.LabelTitle.Text = "Special Withdrawal Request Maintenance" + "--" + Me.TextBoxLast.Text + ", " + Me.TextBoxFirst.Text + " " + Me.TextBoxMiddle.Text.Trim + ", Fund No: " + strFundNo
                    HeaderControl.PageTitle = "Special Withdrawal Request Maintenance"
                    HeaderControl.SSNo = parameterDataRow("SS No").ToString().Trim
                ElseIf Me.RefundType.Trim = "DISAB" Then
                    'Me.LabelTitle.Text = "Disability Withdrawal Request Maintenance" + "--" + Me.TextBoxLast.Text + ", " + Me.TextBoxFirst.Text + " " + Me.TextBoxMiddle.Text.Trim + ", Fund No: " + strFundNo
                    HeaderControl.PageTitle = "Disability Withdrawal Request Maintenance"
                    HeaderControl.SSNo = parameterDataRow("SS No").ToString().Trim
                Else
                    ' Me.LabelTitle.Text = "Regular Withdrawal Request Maintenance" + "--" + Me.TextBoxLast.Text + ", " + Me.TextBoxFirst.Text + " " + Me.TextBoxMiddle.Text.Trim + ", Fund No: " + strFundNo
                    HeaderControl.PageTitle = "Regular Withdrawal COVID19 Request Maintenance"
                    HeaderControl.SSNo = parameterDataRow("SS No").ToString().Trim
                End If
                '----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                'Rahul
                'Name:Preeti Date:10th Feb 06 Issue :YMCA-1858  End

                ' Me.LabelTitle.Text = Me.TextBoxSSNo.Text & "--" & Me.TextBoxFirst.Text.Trim & " " & Me.TextBoxMiddle.Text.Trim & " " & Me.TextBoxLast.Text
                'Session("Title") = Me.LabelTitle.Text
                If (parameterDataRow("SalutationCode").GetType.ToString = "System.DBNull") Then
                    Me.DropDownListSal.SelectedValue = " "
                Else
                    'SR:2013.08.21- BT 2157:In below if condition, one more condition added to handle 0 value from databae
                    If CType(parameterDataRow("SalutationCode"), String).Trim = "" Or CType(parameterDataRow("SalutationCode"), String).Trim = "0" Then
                        Me.DropDownListSal.SelectedValue = " "
                    Else
                        Me.DropDownListSal.SelectedValue = CType(parameterDataRow("SalutationCode"), String).Trim
                    End If
                End If
                'Commented by Aparna -YREN-3036
                'Me.TextBoxUpdatedBy.Text = CType(parameterDataRow("Updater"), String)
                'Me.TextBoxUpdatedDate.Text = CType(parameterDataRow("Updated"), String)


                If (parameterDataRow("VestingDate").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxVested1.Text = "No"
                    Me.TextBoxVested.Text = "No"
                    Me.IsVested = False
                Else
                    Me.TextBoxVested1.Text = "Yes"
                    Me.TextBoxVested.Text = "Yes"
                    Me.IsVested = True
                    'Me.TextBoxVested1.Text = CType(parameterDataRow("VestingDate"), String)
                End If
                ' to bring in vested conditiong

                '' Set StatusType
                If (parameterDataRow("StatusType").GetType.ToString = "String.DBNull") Then
                    Me.SessionStatusType = String.Empty
                Else
                    Me.SessionStatusType = CType(parameterDataRow("StatusType"), String)
                End If


                If (parameterDataRow("NonPaid").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxYear.Text = "0"
                    Me.TextBoxMonth.Text = "0"
                Else
                    Me.TextBoxYear.Text = Me.CalculateNonPaid(CType(parameterDataRow("NonPaid"), Integer), "YEAR")
                    Me.TextBoxMonth.Text = Me.CalculateNonPaid(CType(parameterDataRow("NonPaid"), Integer), "MONTH")
                End If
                'Commented by Shubhrata Apr 17th,2008 -- Modified to Select Case and included new fund status
                'If (parameterDataRow("StatusType").GetType.ToString = "System.DBNull") Then
                '    Me.TextBoxTerminated.Text = String.Empty
                '    Me.IsTerminated = False
                '    Me.IsRetiredActive = False

                'ElseIf (CType(parameterDataRow("StatusType"), String).Trim = "RA") Then
                '    Me.IsRetiredActive = True
                '    Me.IsTerminated = False
                '    Me.TextBoxTerminated.Text = "No"
                '    'BY APARNA 25/10/2007
                '    'If the fundstatus is QD check if personage<55 or the qdroparticipantage<50 then
                '    'treat teh system as TM Vested
                'ElseIf (CType(parameterDataRow("StatusType"), String).Trim = "QD") Then

                '    If (parameterDataRow("BirthDate").GetType.ToString = "System.DBNull") Then
                '        Me.TextBoxAge.Text = "0.00"
                '    ElseIf (parameterDataRow("DeathDate").GetType.ToString = "System.DBNull") Then
                '        l_personage = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), String.Empty)
                '    Else
                '        l_personage = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), CType(parameterDataRow("DeathDate"), String))
                '    End If
                '    If Not Session("QDROParticipantAge") Is Nothing Then
                '        l_QDROParticipantAge = CType(Session("QDROParticipantAge"), Decimal)
                '    End If
                '    If l_personage < 55 Or l_QDROParticipantAge < 50 Then
                '        Me.IsVested = True
                '        Me.IsTerminated = True
                '        Me.TextBoxTerminated.Text = "Yes"
                '    End If

                'Else
                '    'Added RE status - Shubhrata Apr 17th 2008- New Fund Event Status
                '    If ((CType(parameterDataRow("StatusType"), String).Trim = "AE") Or _
                '        ((CType(parameterDataRow("StatusType"), String).Trim = "PE"))) Then
                '        Me.TextBoxTerminated.Text = "No"
                '        Me.IsTerminated = False
                '        Me.IsRetiredActive = False


                '    Else
                '        Me.TextBoxTerminated.Text = "Yes"
                '        Me.IsTerminated = True
                '        Me.IsRetiredActive = False
                '    End If

                'End If
                ' Changed by Shubhrata into SELECT CAse 
                If (parameterDataRow("StatusType").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxTerminated.Text = String.Empty
                    Me.IsTerminated = False
                    Me.IsRetiredActive = False
                Else
                    Dim l_string_FundStatus As String = ""
                    l_string_FundStatus = CType(parameterDataRow("StatusType"), String).Trim().ToUpper()
                    Select Case l_string_FundStatus
                        Case "RA"
                            Me.IsRetiredActive = True
                            Me.IsTerminated = False
                            Me.TextBoxTerminated.Text = "No"
                        Case "QD"
                            If (parameterDataRow("BirthDate").GetType.ToString = "System.DBNull") Then
                                Me.TextBoxAge.Text = "0.00"
                            ElseIf (parameterDataRow("DeathDate").GetType.ToString = "System.DBNull") Then
                                l_personage = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), String.Empty)
                            Else
                                l_personage = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), CType(parameterDataRow("DeathDate"), String))
                            End If
                            If Not Session("QDROParticipantAge") Is Nothing Then
                                l_QDROParticipantAge = CType(Session("QDROParticipantAge"), Decimal)
                            End If
                            If l_personage < 55 Or l_QDROParticipantAge < 50 Then
                                Me.IsVested = True
                                Me.IsTerminated = True
                                Me.TextBoxTerminated.Text = "Yes"
                            End If
                        Case "AE"
                            'START: Added by Sanjeev on 14th Dec 2011 for BT:955
                            Me.IsTerminated = False
                            Me.IsRetiredActive = False
                            Me.TextBoxTerminated.Text = "No"
                            'END:
                        Case "PE", "RE", "NP", "PENP", "RDNP"
                            Me.TextBoxTerminated.Text = "No"
                            Me.IsTerminated = False
                            Me.IsRetiredActive = False
                            'NEERAJ     29/SEP/2009  bug id YRS 5.0-820 for fund status NP,PENP, RDNP set termination status as "No"
                        Case l_string_FundStatus
                            Me.TextBoxTerminated.Text = "Yes"
                            Me.IsTerminated = True
                            Me.IsRetiredActive = False
                    End Select

                End If
                ' Changed by Shubhrata into SELECT CAse    


                If (parameterDataRow("MaritalStatus").GetType.ToString() = "System.DBNull") Then
                    Me.TextBoxMaritalStatus.Text = "Unknown"
                Else
                    Me.TextBoxMaritalStatus.Text = CType(parameterDataRow("MaritalStatus"), String)
                End If

                If (parameterDataRow("BirthDate").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxAge.Text = "0.0"
                ElseIf (parameterDataRow("DeathDate").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxAge.Text = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), String.Empty).ToString("00.00")
                Else
                    Me.TextBoxAge.Text = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), CType(parameterDataRow("DeathDate"), String)).ToString("00.00")
                End If

                'Neeraj 22-Jult-10 : Issue fixed id YRS 5.0-1108 fixed .. date format
                If (Convert.ToString(Me.TextBoxAge.Text).IndexOf(".") > -1) Then
                    'IB:As on 14/June/2010 for  BT:540 convert decimal to string with "00.00" format
                    Me.TextBoxAge.Text = CType(Me.TextBoxAge.Text, Decimal).ToString("00.00")
                    Me.TextBoxAge.Text = Me.TextBoxAge.Text.Replace(".", "Y/") + "M"
                Else
                    Me.TextBoxAge.Text = Convert.ToString(Me.TextBoxAge.Text) + "Y"
                End If
                'BT 568: change Age format for exact in year
                Me.TextBoxAge.Text = Me.TextBoxAge.Text.Replace("/00M", "").Trim

                If (parameterDataRow("DeathDate").GetType.ToString = "System.DBNull") Then
                    Me.DeathDate = String.Empty
                Else
                    Me.DeathDate = CType(parameterDataRow("DeathDate"), String)
                End If
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Function

    Private Function CalculateNonPaid(ByVal parameterNonPaid As Integer, ByVal parameterType As String) As Integer
        Try
            If parameterType = "YEAR" Then
                Return CType((System.Math.Floor(parameterNonPaid / 12.0)), Integer)
            Else
                Return CType(System.Math.Floor(parameterNonPaid Mod 12.0), Integer)
            End If
        Catch ex As Exception
            Throw ex
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

            '' Set StatusType, Which is used in Disabilty Refund. 
            'StatusType SessionStatusType()


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Function

    Private Function LoadAccountContribution(ByVal parameterFundID As String)

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Dim l_emptotal As Decimal

        ' Plan Split variables
        Dim l_datarowarray_RetirementPlan As DataRow()
        Dim l_datarowarray_SavingsPlan As DataRow()
        Dim l_datatable_RetirementPlan As DataTable
        Dim l_datatable_RetirementPlanTotal As DataTable
        Dim l_datatable_SavingsPlanTotal As DataTable
        Dim l_datatable_SavingsPlan As DataTable
        Dim l_datarow_RetirementPlan As DataRow
        Dim l_datarow_SavingsPlan As DataRow
        Dim l_datarow_RetirementPlanTotal As DataRow
        Dim l_datarow_SavingsPlanTotal As DataRow
        Dim i As Integer = 0
        Dim j As Integer = 0
        '*************
        'variables for Unfunded Calculations
        Dim l_datarowarray_RetirementPlan_Unfunded As DataRow()
        Dim l_datarowarray_SavingsPlan_Unfunded As DataRow()
        Dim l_datatable_RetirementPlan_Unfunded As DataTable
        Dim l_datatable_RetirementPlanTotal_Unfunded As DataTable
        Dim l_datatable_SavingsPlanTotal_Unfunded As DataTable
        Dim l_datatable_SavingsPlan_Unfunded As DataTable
        Dim l_datarow_RetirementPlan_Unfunded As DataRow
        Dim l_datarow_SavingsPlan_Unfunded As DataRow
        Dim l_datarow_RetirementPlanTotal_Unfunded As DataRow
        Dim l_datarow_SavingsPlanTotal_Unfunded As DataRow

        Dim l_dataset_AcctContforVolCalculation As DataSet
        '**************************
        ' Plan Split variables
        Try
            'by Aparna 28/11/2007
            Session("RetirementTotal_C19") = "0.00"
            Session("SavingsTotal_C19") = "0.00"
            'by Aparna 28/11/2007
            'AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to display the loan accounts in the tab
            l_DataSet = YMCARET.YmcaBusinessObject.RefundRequest.LookupMemberAccounts(parameterFundID, True)
            If l_DataSet Is Nothing Then Return 0
            ' we need original Acct contribution data table to be used for converting Hard to Vol
            Dim l_dt_AcctContributionInitial As DataTable
            l_dt_AcctContributionInitial = l_DataSet.Tables("AccountForPaid").Clone()
            For Each dr As DataRow In l_DataSet.Tables("AccountForPaid").Rows
                l_dt_AcctContributionInitial.ImportRow(dr)
            Next
            Session("AcctContributionInitial_C19") = l_dt_AcctContributionInitial
            ' we need original Acct contribution data table to be used for converting Hard to Vol
            'Vipul 13Apr06 YRS Enhancement 50 ... To consider Unfunded Transactions during Refund Distribution & Release Blank Report
            Dim l_dataTable_AccountContribution_NonFunded As Data.DataTable
            Dim l_DataRow_nonfunded As Data.DataRow
            Dim l_decimal_Total As Double = 0
            l_dataTable_AccountContribution_NonFunded = l_DataSet.Tables("AccountForNonPaid")
            'Shubhrata YREN 2607
            Session("l_dataTable_AccountContribution_NonFunded_C19") = l_dataTable_AccountContribution_NonFunded
            'Shubhrata YREN 2607
            For Each l_DataRow_nonfunded In l_dataTable_AccountContribution_NonFunded.Rows
                If l_DataRow_nonfunded("Total").GetType.ToString() <> "System.DBNull" Then
                    l_decimal_Total += l_DataRow_nonfunded("Total")
                End If
            Next
            Session("AccountContribution_NonFunded_Total_C19") = l_decimal_Total
            'Vipul 13Apr06 YRS Enhancement 50 ... To consider Unfunded Transactions during Refund Distribution & Release Blank Report
            'Shubhrata May21st,2007 Plan Split Changes
            'Me.DataGridFundedAccountContributions.DataSource = Me.AddTotalIntoTable(l_DataSet.Tables("AccountForPaid"), l_DataSet.Tables("AccountForPaidTotal"))
            'Me.DataGridFundedAccountContributions.DataBind()

            'Me.DataGridNonFundedAccountContributions.DataSource = Me.AddTotalIntoTable(l_DataSet.Tables("AccountForNonPaid"), l_DataSet.Tables("AccountForNonPaidTotal"))
            'Me.DataGridNonFundedAccountContributions.DataBind()
            '************************************

            If Not l_DataSet Is Nothing Then

                ' The DataTable for Retirement Plan(the funded accout contributions) is being populated here 
                l_datatable_RetirementPlan = l_DataSet.Tables("AccountForPaid").Clone()
                l_datarowarray_RetirementPlan = l_DataSet.Tables("AccountForPaid").Select("PlanType = 'RETIREMENT '")
                If l_datarowarray_RetirementPlan.Length > 0 Then
                    For i = 0 To l_datarowarray_RetirementPlan.Length - 1
                        l_datatable_RetirementPlan.ImportRow(l_datarowarray_RetirementPlan(i))
                    Next

                Else
                    Dim drow_dummy_retirementplan As DataRow
                    drow_dummy_retirementplan = l_datatable_RetirementPlan.NewRow()
                    l_datatable_RetirementPlan.Rows.Add(drow_dummy_retirementplan)
                End If
                Session("RetirementPlanAcctContribution_C19") = l_datatable_RetirementPlan

                ' The DataTable for Retirement Plan(the funded accout contributions) is now ready.

                'The DataTable for Savings Plan(the funded accout contributions) is being populated here 
                l_datatable_SavingsPlan = l_DataSet.Tables("AccountForPaid").Clone()
                l_datarowarray_SavingsPlan = l_DataSet.Tables("AccountForPaid").Select("PlanType = 'SAVINGS '")
                If l_datarowarray_SavingsPlan.Length > 0 Then
                    For j = 0 To l_datarowarray_SavingsPlan.Length - 1
                        l_datatable_SavingsPlan.ImportRow(l_datarowarray_SavingsPlan(j))
                    Next

                Else
                    Dim drow_dummy_SavingsPlan As DataRow
                    drow_dummy_SavingsPlan = l_datatable_SavingsPlan.NewRow()
                    l_datatable_SavingsPlan.Rows.Add(drow_dummy_SavingsPlan)
                End If
                Session("SavingsPlanAcctContribution_C19") = l_datatable_SavingsPlan
                'here we are getting the total ready for the Retirement & Savings Plan respectively 
                Dim l_String_PlanType As String = ""
                l_datatable_RetirementPlanTotal = l_DataSet.Tables("AccountForPaidTotal").Clone
                l_datatable_SavingsPlanTotal = l_DataSet.Tables("AccountForPaidTotal").Clone
                For Each drow As DataRow In l_DataSet.Tables("AccountForPaidTotal").Rows
                    If Not IsDBNull(drow("PlanType")) Then
                        l_String_PlanType = drow("PlanType")
                    End If
                    If l_String_PlanType <> "" Then
                        Select Case (l_String_PlanType.Trim.ToUpper)
                            Case "RETIREMENT"
                                l_datatable_RetirementPlanTotal.ImportRow(drow)
                            Case "SAVINGS"
                                l_datatable_SavingsPlanTotal.ImportRow(drow)
                        End Select
                    End If
                Next
                'here we are getting the total ready for the Retirement & Savings Plan respectively 
                'retirement plan funded account contribution data grid 
                Me.DataGridFundedAccountContributions.DataSource = Me.AddTotalIntoTable(l_datatable_RetirementPlan, l_datatable_RetirementPlanTotal)
                Me.DataGridFundedAccountContributions.DataBind()



                'savings plan funded account contribution data grid 
                Me.DataGridFundedAccntContributionsSavingsPlan.DataSource = Me.AddTotalIntoTable(l_datatable_SavingsPlan, l_datatable_SavingsPlanTotal)
                Me.DataGridFundedAccntContributionsSavingsPlan.DataBind()


                'by Aparna -To get the total Amount for Annuity check. 22/11/2007
                Dim l_RetirementTotalDataRow As DataRow

                If l_datatable_RetirementPlanTotal.Rows.Count > 0 Then

                    l_RetirementTotalDataRow = l_datatable_RetirementPlanTotal.Rows.Item(0)
                    Session("RetirementTotal_C19") = l_RetirementTotalDataRow("Total")
                Else
                    Session("RetirementTotal_C19") = "0.00"
                End If


                'by Aparna -To get the total Amount for Annuity check.
                Dim l_SavingsTotalDataRow As DataRow
                If l_datatable_SavingsPlanTotal.Rows.Count > 0 Then
                    l_SavingsTotalDataRow = l_datatable_SavingsPlanTotal.Rows.Item(0)
                    Session("SavingsTotal_C19") = l_SavingsTotalDataRow("Total")
                Else
                    Session("SavingsTotal_C19") = "0.00"
                End If
                'by Aparna 22/11/2007

                '***********************************
                'This section is for Non-funded contributions
                '**********************************

                ' The DataTable for Retirement Plan(the unfunded accout contributions) is being populated here 
                l_datatable_RetirementPlan_Unfunded = l_DataSet.Tables("AccountForNonPaid").Clone()
                l_datarowarray_RetirementPlan_Unfunded = l_DataSet.Tables("AccountForNonPaid").Select("PlanType = 'RETIREMENT '")
                If l_datarowarray_RetirementPlan_Unfunded.Length > 0 Then
                    For i = 0 To l_datarowarray_RetirementPlan_Unfunded.Length - 1
                        l_datatable_RetirementPlan_Unfunded.ImportRow(l_datarowarray_RetirementPlan_Unfunded(i))
                    Next
                End If
                ' The DataTable for Retirement Plan(the unfunded accout contributions) is now ready.

                'The DataTable for Savings Plan(the unfunded accout contributions) is being populated here 
                l_datatable_SavingsPlan_Unfunded = l_DataSet.Tables("AccountForNonPaid").Clone()
                l_datarowarray_SavingsPlan_Unfunded = l_DataSet.Tables("AccountForNonPaid").Select("PlanType = 'SAVINGS '")
                If l_datarowarray_SavingsPlan_Unfunded.Length > 0 Then
                    For j = 0 To l_datarowarray_SavingsPlan_Unfunded.Length - 1
                        l_datatable_SavingsPlan_Unfunded.ImportRow(l_datarowarray_SavingsPlan_Unfunded(j))
                    Next
                End If
                'here we are getting the total ready for the Retirement & Savings Plan respectively 
                Dim l_String_PlanType_Unfunded As String = ""
                l_datatable_RetirementPlanTotal_Unfunded = l_DataSet.Tables("AccountForNonPaidTotal").Clone
                l_datatable_SavingsPlanTotal_Unfunded = l_DataSet.Tables("AccountForNonPaidTotal").Clone
                For Each drow As DataRow In l_DataSet.Tables("AccountForNonPaidTotal").Rows
                    If Not IsDBNull(drow("PlanType")) Then
                        l_String_PlanType_Unfunded = drow("PlanType")
                    End If
                    If l_String_PlanType_Unfunded <> "" Then
                        Select Case (l_String_PlanType_Unfunded.Trim.ToUpper)
                            Case "RETIREMENT"
                                l_datatable_RetirementPlanTotal_Unfunded.ImportRow(drow)
                            Case "SAVINGS"
                                l_datatable_SavingsPlanTotal_Unfunded.ImportRow(drow)
                        End Select
                    End If
                Next
                'here we are getting the total ready for the Retirement & Savings Plan respectively 
                'retirement plan funded account contribution data grid
                Me.DataGridNonFundedAccountContributions.DataSource = Me.AddTotalIntoTable(l_datatable_RetirementPlan_Unfunded, l_datatable_RetirementPlanTotal_Unfunded)
                Me.DataGridNonFundedAccountContributions.DataBind()

                'savings plan funded account contribution data grid 
                Me.DatagridNonFundedAcctContributionSavingsPlan.DataSource = Me.AddTotalIntoTable(l_datatable_SavingsPlan_Unfunded, l_datatable_SavingsPlanTotal_Unfunded)
                Me.DatagridNonFundedAcctContributionSavingsPlan.DataBind()
                'We will need the unfunded datatables for Retirement and Savings plan to check for the unfunded transactions
                Session("RetirementPlan_UnfundedContributions_C19") = l_datatable_RetirementPlan_Unfunded
                Session("SavingsPlan_UnfundedContributions_C19") = l_datatable_SavingsPlan_Unfunded
            End If
            ' To set the selected  Index so that we can show total row in Bold Letters. :)
            Me.SetSelectedIndex(Me.DataGridFundedAccountContributions, l_datatable_RetirementPlan)
            Me.SetSelectedIndex(Me.DataGridFundedAccntContributionsSavingsPlan, l_datatable_SavingsPlan)
            Me.SetSelectedIndex(Me.DataGridNonFundedAccountContributions, l_datatable_RetirementPlan_Unfunded)
            Me.SetSelectedIndex(Me.DatagridNonFundedAcctContributionSavingsPlan, l_datatable_SavingsPlan_Unfunded)

            'Shubhrata May21st,2007 Plan Split Changes

            '' Add the Found .. for Paid Contribution. 
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


            Session("AccountContribution_C19") = l_DataSet.Tables("AccountForPaid")
            Session("AccountContribution_NonFund_C19") = l_DataSet.Tables("AccountForNonPaid")

            ' 

        Catch
            Throw
        End Try
    End Function

    Private Function SetSelectedIndex(ByVal parameterDataGrid As DataGrid, ByVal parameterDataTable As DataTable)

        Try

            If parameterDataTable Is Nothing Then
                parameterDataGrid.SelectedIndex = -1
            Else
                parameterDataGrid.SelectedIndex = (parameterDataTable.Rows.Count - 1)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
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

                'Shubhrata May22nd,2007 
                If parameterDataTable.Rows.Count > 0 Then
                    l_TotalDataRow = parameterDataTable.Rows.Item(0)
                End If


                l_DataRow = paramaterSourceDataTable.NewRow

                If Not l_TotalDataRow Is Nothing Then

                    l_DataRow("AccountType") = "Total"
                    'l_DataRow("YmcaID") = "0"
                    l_DataRow("YmcaID") = Guid.NewGuid.ToString()
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

                Else
                    l_DataRow("AccountType") = "Total"
                    'l_DataRow("YmcaID") = "0"
                    l_DataRow("YmcaID") = Guid.NewGuid.ToString()
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
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Function

    Private Function LoadRefundRequestDetails(ByVal parameterPersonID As String, ByVal parameterFundID As String)

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_DataTable As DataTable
        Dim l_grossamt As Decimal
        Dim blnPreviousPlanType As Boolean = False
        l_grossamt = 0.0
        Try
            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.MemberRefundRequestDetails(parameterPersonID, parameterFundID)

            Me.DataGridRequests.DataSource = l_DataTable
            Me.DataGridRequests.DataBind()

            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager()
            'l_CacheManager.Add("RefundRequestTable", l_DataTable)
            Session("RefundRequestTable_C19") = l_DataTable
            Session("RefundRequestDataTable_C19") = l_DataTable 'by APARNA 08/08/07 -to avoid Error records dont exist as same Session is used in processing

            'Hafiz 03Feb06 Cache-Session

            '' This segment for Enable / Disable "Add Item" Button in Refund request Tab.

            'Commented by Ganeswar for Partial withdrawal on july 10th 2009

            'If Me.IsRequestPending(l_DataTable) Then
            '    Me.ButtonAddItem.Enabled = False
            '    Me.SessionIsRefundPopupAllowed = False
            '    Me.SessionIsRefundProcessPopupAllowed = True
            'Else
            '    Me.ButtonAddItem.Enabled = True
            '    Me.SessionIsRefundPopupAllowed = True
            '    Me.SessionIsRefundProcessPopupAllowed = False
            'End If

            Dim l_bool_Savings As Boolean = False
            Dim l_bool_Retirement As Boolean = False
            Dim l_bool_Both As Boolean = False
            If Not l_DataTable Is Nothing Then
                For Each l_DataRow As DataRow In l_DataTable.Rows
                    If Not l_DataRow Is Nothing Then
                        If CType(l_DataRow.Item("RequestStatus"), String).Trim.ToUpper = "PEND" Then
                            If l_DataRow.Item("PlanType").ToString() = "System.DBNull" Then
                            Else
                                'added the if condition to check the null value of plantype 
                                'which is an addd column in the the database as per requested
                                If l_DataRow.Item("PlanType").ToString() <> "System.DBNull" And l_DataRow.Item("PlanType").ToString() <> String.Empty Then
                                    'modified the if condition to include the check for available balances in the savings and retriement plan 
                                    'so that the add request button should be enable or disabled Amit.
                                    If CType(l_DataRow.Item("PlanType"), String).Trim.ToUpper = "SAVINGS" Or Session("SavingsTotal_C19") <= 0 Then
                                        l_bool_Savings = True
                                    End If
                                End If

                                If l_DataRow.Item("PlanType").ToString() <> "System.DBNull" And l_DataRow.Item("PlanType").ToString() <> String.Empty Then
                                    If CType(l_DataRow.Item("PlanType"), String).Trim.ToUpper = "RETIREMENT" Or Session("RetirementTotal_C19") <= 0 Then
                                        l_bool_Retirement = True
                                    End If
                                End If


                                If l_bool_Savings = True And l_bool_Retirement = True Then
                                    Me.ButtonAddItem.Enabled = False
                                    Me.SessionIsRefundPopupAllowed = False
                                    'Added to set the value of the sesson to open the process screen-Amit
                                    Me.SessionIsRefundProcessPopupAllowed = True
                                    'Added to set the value of the sesson to open the process screen-Amit
                                End If
                                If l_DataRow.Item("PlanType").ToString() <> "System.DBNull" And l_DataRow.Item("PlanType").ToString() <> String.Empty Then
                                    If CType(l_DataRow.Item("PlanType"), String).Trim.ToUpper = "BOTH" Then
                                        Me.ButtonAddItem.Enabled = False
                                    End If
                                    l_bool_Both = True
                                    Me.SessionIsRefundProcessPopupAllowed = True
                                End If
                            End If
                            'added the if comdition to check the null value of plantype 
                            'which is an addd column in the the database as per requested
                        End If
                    End If

                    If CType(l_DataRow.Item("RequestStatus"), String).Trim.ToUpper = "PEND" And (l_DataRow.Item("PlanType").ToString() = "System.DBNull" Or l_DataRow.Item("PlanType").ToString() = String.Empty) Then
                        blnPreviousPlanType = True
                    End If
                    'Modified if condition to disable the request adding for active participant if he has any pending request-Amit
                    If Me.RefundType = "SPEC" Or Me.RefundType = "DISAB" Or Me.IsTerminated = False Then
                        'Modified if condition to disable the request adding for active participant if he has any pending request-Amit
                        If CType(l_DataRow.Item("RequestStatus"), String).Trim.ToUpper = "PEND" Then
                            l_bool_Savings = True
                            l_bool_Retirement = True
                            Me.ButtonAddItem.Enabled = False
                            Me.SessionIsRefundPopupAllowed = False
                            Me.SessionIsRefundProcessPopupAllowed = True
                        End If
                    End If
                Next
                If l_bool_Both = False Then
                    Me.ButtonAddItem.Enabled = True
                    Me.SessionIsRefundPopupAllowed = True
                    Me.SessionIsRefundProcessPopupAllowed = True
                ElseIf l_bool_Retirement = True And l_bool_Savings = False Then
                    Me.ButtonAddItem.Enabled = True
                    Me.SessionIsRefundPopupAllowed = True
                    Me.SessionIsRefundProcessPopupAllowed = True
                ElseIf l_bool_Retirement = False And l_bool_Savings = True Then
                    Me.ButtonAddItem.Enabled = True
                    Me.SessionIsRefundPopupAllowed = True
                    Me.SessionIsRefundProcessPopupAllowed = True
                End If
            End If

            If blnPreviousPlanType Then
                Me.ButtonAddItem.Enabled = False
            End If
            If Me.IsTerminated = False And l_bool_Savings = True Then
                Me.ButtonAddItem.Enabled = False
            End If
            'If l_bool_Both = True Then
            '    Me.ButtonAddItem.Enabled = False
            'End If

            'Commented by Ganeswar for Partial withdrawal on july 10th 2009
            Me.DataGridRequests.SelectedIndex = -1
            If Not Session("GenerateErrors_C19") Is Nothing Then

                Dim popupScriptError As String = "<script language='javascript'>" & _
                "window.open('GenerateError.aspx', 'GenerateError', " & _
                "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                "</script>"
                If (Not Me.IsStartupScriptRegistered("GenerateError")) Then
                    Page.RegisterStartupScript("GenerateError", popupScriptError)
                End If
            End If

            'If Not Session("R_ReportToLoad") Is Nothing And CType(Session("R_ReportToLoad"), Boolean) = True Then
            '    PreviewRefundReport()
            'ElseIf Not Session("R_ReportToLoad_1") Is Nothing And CType(Session("R_ReportToLoad_1"), Boolean) = True Then
            '    PreviewRefundReport()
            'ElseIf Not Session("R_ReportToLoad_2") Is Nothing And CType(Session("R_ReportToLoad_2"), Boolean) = True Then
            '    PreviewRefundReport()
            '    'by aparna -yren-3027 24/01/2007
            'ElseIf Not Session("R_ReportToLoad_3") Is Nothing And CType(Session("R_ReportToLoad_3"), Boolean) = True Then
            '    PreviewRefundReport()
            'ElseIf Not Session("R_ReportToLoad_4") Is Nothing And CType(Session("R_ReportToLoad_4"), Boolean) = True Then
            '    PreviewRefundReport()
            'ElseIf Not Session("R_ReportToLoad_5") Is Nothing And CType(Session("R_ReportToLoad_5"), Boolean) = True Then
            '    PreviewRefundReport()
            'ElseIf Not Session("R_ReportToLoad_6") Is Nothing And CType(Session("R_ReportToLoad_6"), Boolean) = True Then
            '    PreviewRefundReport()
            'End If

            PreviewWSRefundReport()

        Catch
            Throw
        End Try
    End Function
    Private Function PreviewWSRefundReport() As Boolean
        Dim popupScript As String
        Dim l_StringReportMessage As String
        Dim ds As DataSet
        Dim dtReport As DataTable


        Try



            If Not Session("ServiceRequestReport_C19") Is Nothing Then
                ds = CType(Session("ServiceRequestReport_C19"), DataSet)

                Dim i As Int16 = 0
                For Each l_Row As DataRow In ds.Tables("Report").Rows


                    If l_Row("ReportNo").GetType.ToString() <> "System.DBNull" Then
                        i = i + 1
                        popupScript = "<script language='javascript'>" & _
                    "window.open('FT\\RefundReportViewer.aspx?ReportNo=" & l_Row("ReportNo").ToString() & "&ReportName=" & l_Row("ReportName").ToString() & " ', 'ReportPopUp_" & i.ToString() & "', " & _
                        "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                                        "</script>"
                        If (Not Me.IsStartupScriptRegistered("PopupScript" + i.ToString())) Then
                            Page.RegisterStartupScript("PopupScript" + i.ToString(), popupScript)
                        End If

                    End If

                Next
                'Return True
                'Priya:12-March-2010 : YRS 5.0-1029:Cannot withdraw a Hardship withdrawal.
                'When we process Hardship withdrawal it is suppose to generate a letter "birefltr.rpt'. It is not generating this letter.
            ElseIf CType(Session("R_ReportToLoad_6_C19"), Boolean) = True Then

                'Dim l_popupScript As String
                'l_popupScript = "<script language='javascript'>" & _
                '"window.open('FT\\ReportViewer.aspx', 'ReportPopUp_4', " & _
                '"'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                '"</script>"
                'If (Not Me.IsStartupScriptRegistered("PopupScript7")) Then
                '    Page.RegisterStartupScript("PopupScript7", l_popupScript)
                'End If


                'IB:YRS 5.0-1181 :call harship report with correspond YMCA.

                Dim l_datatable As New DataTable
                Dim l_String_StatusType As String = "A"
                Dim l_datarow_CurrentRow As DataRow()
                Dim i As Int16 = 0
                If Not Session("Member Employment_C19") Is Nothing Then
                    l_datatable = CType(Session("Member Employment_C19"), DataTable)
                    l_datarow_CurrentRow = l_datatable.Select("FundEventID = '" & Session("FundID") & " ' and  (TermDate IS NULL OR StatusType =  '" & l_String_StatusType & "')")
                    If l_datarow_CurrentRow.Length > 0 Then
                        For Each dr As DataRow In l_datarow_CurrentRow

                            popupScript = "<script language='javascript'>" & _
                 "window.open('FT\\RefundReportViewer.aspx?ReportNo=" & i.ToString() & "&ReportName=birefltr.rpt', 'ReportPopUp_" & i.ToString() & "', " & _
                     "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                                     "</script>"
                            If (Not Me.IsStartupScriptRegistered("PopupScript" + i.ToString())) Then
                                Page.RegisterStartupScript("PopupScript" + i.ToString(), popupScript)
                            End If
                            i = i + 1
                        Next
                    End If
                End If

                ''IB:Added on 03/Sep/2010 call copytoserver method for hardship refund
                'Dim popupScript1 As String = "<script language='javascript'>" & _
                '   "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                '   "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                '   "</script>"
                'If (Not Me.IsStartupScriptRegistered("PopupScripthard")) Then
                '    Page.RegisterStartupScript("PopupScripthard", popupScript1)
                'End If
                'IB:Added on 03/Sep/2010 BT-576 call copytoserver method for hardship refund
                If Not Session("FTFileList_C19") Is Nothing Then
                    Try
                        ' Call the calling of the ASPX to copy the file.
                        Dim popupScriptCopytoServer As String = "<script language='javascript'>" & _
                        "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                        "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                        "</script>"
                        If (Not Me.IsStartupScriptRegistered("PopupCopytoServer")) Then
                            Page.RegisterStartupScript("PopupCopytoServer", popupScriptCopytoServer)
                        End If

                    Catch ex As Exception
                        MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Error while transfering documents to IDM", MessageBoxButtons.OK)
                        Exit Function
                    End Try
                End If



                Session("R_ReportToLoad_6_C19") = Nothing

            End If
            'End YRS 5.0-1029:Cannot withdraw a Hardship withdrawal.



            ' If Session("ServiceRequestReport") Is Nothing AndAlso Not Session("GenerateErrors") Is Nothing Then
            If Not Session("GenerateErrors_C19") Is Nothing Then

                Dim popupScriptError As String = "<script language='javascript'>" & _
                                "window.open('GenerateError.aspx', 'GenerateError', " & _
                                "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                                "</script>"
                If (Not Me.IsStartupScriptRegistered("GenerateError")) Then
                    Page.RegisterStartupScript("GenerateError", popupScriptError)
                End If
            End If
            Return True
        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Function PreviewRefundReport() As Boolean
        Dim popupScript As String
        Dim l_StringReportMessage As String
        Try


            If CType(Session("R_ReportToLoad_C19"), Boolean) = True Then

                popupScript = "<script language='javascript'>" & _
                "window.open('FT\\ReportViewer.aspx', 'ReportPopUp_1', " & _
                "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                "</script>"
                If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                    Page.RegisterStartupScript("PopupScript1", popupScript)
                End If
            End If

            If CType(Session("R_ReportToLoad_1_C19"), Boolean) = True Then

                popupScript = "<script language='javascript'>" & _
                "window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp_2', " & _
                "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                "</script>"
                If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                    Page.RegisterStartupScript("PopupScript2", popupScript)
                End If
            End If

            If CType(Session("R_ReportToLoad_2_C19"), Boolean) = True Then
                popupScript = "<script language='javascript'>" & _
                "window.open('FT\\ReportViewer_2.aspx', 'ReportPopUp_3', " & _
                "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                "</script>"
                If (Not Me.IsStartupScriptRegistered("PopupScript3")) Then
                    Page.RegisterStartupScript("PopupScript3", popupScript)
                End If
            End If

            ''By Aparna -YREN-3027
            'If CType(Session("R_ReportToLoad_3"), Boolean) = True Then
            '    Dim l_popupScript As String
            '    l_popupScript = "<script language='javascript'>" & _
            '    "window.open('FT\\ReportViewer.aspx', 'ReportPopUp_4', " & _
            '    "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
            '    "</script>"
            '    If (Not Me.IsStartupScriptRegistered("PopupScript4")) Then
            '        Page.RegisterStartupScript("PopupScript4", l_popupScript)
            '    End If
            'End If
            ''By Aparna -YREN-3027

            If CType(Session("R_ReportToLoad_3_C19"), Boolean) = True Then
                Dim l_popupScript As String
                l_popupScript = "<script language='javascript'>" & _
                "window.open('FT\\ReportViewer_3.aspx', 'ReportPopUp_4', " & _
                "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                "</script>"
                If (Not Me.IsStartupScriptRegistered("PopupScript4")) Then
                    Page.RegisterStartupScript("PopupScript4", l_popupScript)
                End If
            End If
            'By Aparna -YREN-3027

            If CType(Session("R_ReportToLoad_4_C19"), Boolean) = True Then
                Dim l_popupScript As String
                l_popupScript = "<script language='javascript'>" & _
                "window.open('FT\\ReportViewer_4.aspx', 'ReportPopUp_5', " & _
                "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                "</script>"
                If (Not Me.IsStartupScriptRegistered("PopupScript5")) Then
                    Page.RegisterStartupScript("PopupScript5", l_popupScript)
                End If
            End If
            'By Aparna -YREN-3027

            If CType(Session("R_ReportToLoad_5_C19"), Boolean) = True Then
                Dim l_popupScript As String
                l_popupScript = "<script language='javascript'>" & _
                "window.open('FT\\ReportViewer_5.aspx', 'ReportPopUp_6', " & _
                "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                "</script>"
                If (Not Me.IsStartupScriptRegistered("PopupScript6")) Then
                    Page.RegisterStartupScript("PopupScript6", l_popupScript)
                End If
            End If
            'By Aparna -YREN-3027


            If CType(Session("R_ReportToLoad_6_C19"), Boolean) = True Then
                Dim l_popupScript As String
                l_popupScript = "<script language='javascript'>" & _
                "window.open('FT\\ReportViewer.aspx', 'ReportPopUp_4', " & _
                "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
                "</script>"
                If (Not Me.IsStartupScriptRegistered("PopupScript7")) Then
                    Page.RegisterStartupScript("PopupScript7", l_popupScript)
                End If
            End If

            If CType(Session("R_ReportToLoad_C19"), Boolean) = True Or _
                CType(Session("R_ReportToLoad_1_C19"), Boolean) = True Or _
                CType(Session("R_ReportToLoad_2_C19"), Boolean) = True Or _
                CType(Session("R_ReportToLoad_3_C19"), Boolean) = True Or _
                CType(Session("R_ReportToLoad_4_C19"), Boolean) = True Or _
                CType(Session("R_ReportToLoad_5_C19"), Boolean) = True Or _
                CType(Session("R_ReportToLoad_6_C19"), Boolean) = True Then


                ' Check weather the Session is available to call the popup.
                If Not Session("FTFileList_C19") Is Nothing Then
                    Try
                        ' Call the calling of the ASPX to copy the file.
                        Dim popupScriptCopytoServer As String = "<script language='javascript'>" & _
                        "var a = window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', " & _
                        "'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no')" & _
                        "</script>"
                        If (Not Me.IsStartupScriptRegistered("PopupCopytoServer")) Then
                            Page.RegisterStartupScript("PopupCopytoServer", popupScriptCopytoServer)
                        End If

                    Catch ex As Exception
                        MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Error while transfering documents to IDM", MessageBoxButtons.OK)
                        Exit Function
                    End Try
                End If

            End If
            Session("R_ReportToLoad_C19") = Nothing
            Session("R_ReportToLoad_1_C19") = Nothing
            Session("R_ReportToLoad_2_C19") = Nothing
            'By Aparna -YREN-3027
            Session("R_ReportToLoad_3_C19") = Nothing
            'By Aparna -YREN-3027

            Session("R_ReportToLoad_4_C19") = Nothing
            Session("R_ReportToLoad_5_C19") = Nothing
            Session("R_ReportToLoad_6_C19") = Nothing
            'Modified as per the additional partial request for the reports to display the message if the participant age is more then 70.5-Amit 29.06.2009

            If objRefundRequest.DisplayMessageForLetters = True Then
                'YRS 5.0-913 Amit 07-oct-2009
                'l_StringReportMessage = "Participant is or will be age 70-1/2 in the current year.  Please refer this request to Benefits Processing"
                l_StringReportMessage = "Participant is or will be age 70-1/2 in the current year. Please refer this request to Benefits for correct letter before mailing."
                'YRS 5.0-913 Amit 07-oct-2009
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", l_StringReportMessage, MessageBoxButtons.Stop)
            End If
            'Modified as per the additional partial request for the reports to display the message if the participant age is more then 70.5-Amit 29.06.2009

        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Function IsRequestPending(ByVal parameterDataTable As DataTable) As Boolean

        Try
            If Not parameterDataTable Is Nothing Then

                For Each l_DataRow As DataRow In parameterDataTable.Rows
                    If Not l_DataRow Is Nothing Then
                        If CType(l_DataRow.Item("RequestStatus"), String).Trim.ToUpper = "PEND" Then
                            Return True
                        End If
                    End If

                Next

                Return False

            Else
                Return False
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Function

    Private Function LoadMemberNotes(ByVal parameterPersonID)

        Dim l_DataTable As DataTable
        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session

        Try
            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.MemberNotes(parameterPersonID)
            If l_DataTable.Rows.Count > 0 Then
                Me.NotesFlag.Value = "Notes"
            Else
                Me.NotesFlag.Value = ""
            End If
            Me.DataGridRefundNotes.DataSource = l_DataTable
            Me.DataGridRefundNotes.DataBind()
            'by Aparna -yren-3115 15/03/2007
            If Me.NotesFlag.Value = "Notes" Then
                Me.RefundTabStrip.Items(4).Text = "<font color=orange>Notes</font>"
            ElseIf Me.NotesFlag.Value = "MarkedImportant" Then
                Me.RefundTabStrip.Items(4).Text = "<font color=red>Notes</font>"
            Else
                Me.RefundTabStrip.Items(4).Text = "Notes"
            End If
            'by Aparna -yren-3115 15/03/2007
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'l_CacheManager.Add("MemberNotes", l_DataTable)
            Session("MemberNotes") = l_DataTable
            'Hafiz 03Feb06 Cache-Session

        Catch
            Throw
        End Try
    End Function

    Private Function LoadCurrentPIA(ByVal parameterFundID As String)

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManger As CacheManager
        'Hafiz 03Feb06 Cache-Session

        Dim l_decimal_CurrentPIA As Decimal
        Dim l_decimal_PIATermination As Decimal

        'BA Account YMCA PhaseV 03-04-2009 by Dilip
        Dim l_decimal_BACurrent As Decimal
        Dim l_decimal_BATermination As Decimal
        'BA Account YMCA PhaseV 03-04-2009  by Dilip

        Try

            l_decimal_CurrentPIA = YMCARET.YmcaBusinessObject.RefundRequest.GetCurrentPIA(parameterFundID)
            l_decimal_PIATermination = YMCARET.YmcaBusinessObject.RefundRequest.GetTerminatePIA(parameterFundID)
            'BA Account YMCA PhaseV 03-04-2009  by Dilip
            l_decimal_BACurrent = YMCARET.YmcaBusinessObject.RefundRequest.GetBACurrentPIA(parameterFundID)
            l_decimal_BATermination = YMCARET.YmcaBusinessObject.RefundRequest.GetBATerminatePIA(parameterFundID)
            'BA Account YMCA PhaseV 03-04-2009  by Dilip

            If Me.IsTerminated = False Then
                l_decimal_BATermination = l_decimal_BACurrent 'BA Account YMCA PhaseV 03-04-2009  by Dilip
                l_decimal_PIATermination = l_decimal_CurrentPIA
            End If
            Me.TextBoxCurrentPIA.Text = FormatCurrency(l_decimal_CurrentPIA)
            Me.TextBoxPIATermination.Text = FormatCurrency(l_decimal_PIATermination)

            'BA Account YMCA PhaseV 03-04-2009  by Dilip
            Me.TextBoxBACurrent.Text = FormatCurrency(l_decimal_BACurrent)
            Me.TextBoxBATermination.Text = FormatCurrency(l_decimal_BATermination)
            'BA Account YMCA PhaseV 03-04-2009  by Dilip

            '' Add the PIA into Cache.. to populate into Refund Request form. 
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManger = CacheFactory.GetCacheManager
            'l_CacheManger.Add("CurrentPIA", l_decimal_CurrentPIA)
            'l_CacheManger.Add("TerminationPIA", l_decimal_PIATermination)
            Session("CurrentPIA_C19") = l_decimal_CurrentPIA
            Session("TerminationPIA_C19") = l_decimal_PIATermination
            'BA Account YMCA PhaseV 03-04-2009  by Dilip
            Session("BACurrent_C19") = l_decimal_BACurrent
            Session("BATermination_C19") = l_decimal_BATermination
            'BA Account YMCA PhaseV 03-04-2009  by Dilip
            'Hafiz 03Feb06 Cache-Session

        Catch
            Throw
        End Try
    End Function

    Private Sub ButtonAddItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddItem.Click

        Dim l_integer_QDROParticipantAge As Integer
        Dim l_string_message As String
        '' Check for the PendingQDRO, if Exists then raise the error
        Try
            'Commented by Shashi Shekhar:2009-12-31
            ''Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            'Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            'If Not checkSecurity.Equals("True") Then
            '    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            '    Exit Sub
            'End If
            ''End : YRS 5.0-940 

            '---------------------------------------------------------------------------------------
            'Added by Shashi Shekhar:2009-12-31: To resolve the security Issue(Ref:Security Review excel sheet)
            Dim checkSecurity As String
            Dim strFormName As String

            'If Me.RefundType.Trim = "DISAB" Then
            '    'AA:2014.02.03:BT:2292:YRS 5.0 - 2248 : Changed the Formname according to the rights given in acuyussi table
            '    'strFormName = "RefundRequestForm.aspx?RT=DISAB"
            '    strFormName = "FindInfo.aspx?Name=Refund&RT=DISAB"
            '    checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            'ElseIf Me.RefundType.Trim = "SPEC" Then
            '    'AA:2014.02.03:BT:2292:YRS 5.0 - 2248 : Changed the Formname according to the rights given in acuyussi table
            '    'strFormName = "RefundRequestForm.aspx?RT=SPEC"
            '    strFormName = "FindInfo.aspx?Name=Refund&RT=SPEC"
            '    checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            'Else
            'AA:2014.02.03:BT:2292:YRS 5.0 - 2248 : Changed the Formname according to the rights given in acuyussi table
            'strFormName = "RefundRequestForm.aspx"
            strFormName = "FindInfo.aspx?Name=Refund&RT=C19"
            checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            'End If

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If

            '--------------------------------------------------------------------------------------
            'Shashi Shekhar:10 Feb 2011:  For YRS 5.0-1236 : Need ability to freeze/lock account
            If Not Me.IsAccountLock = Nothing Then
                If Me.IsAccountLock.ToString.Trim.ToLower = "true" Then
                    '---Shashi Shekhar :14 - Feb -2011: For BT-750 While QDRO split message showing wrong.-
                    Dim l_dsLockResDetails As DataSet
                    Dim l_reasonLock As String
                    If Not Me.TextBoxSSNo.Text = String.Empty Then
                        l_dsLockResDetails = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetLockReasonDetails(Me.TextBoxSSNo.Text.ToString.Trim)
                    End If

                    If Not l_dsLockResDetails Is Nothing Then
                        If l_dsLockResDetails.Tables("GetLockReasonDetails").Rows.Count > 0 Then
                            If (l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString() <> "System.DBNull" And l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString() <> "") Then

                                l_reasonLock = l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString().Trim
                            End If
                        End If
                    End If
                    If l_reasonLock = "" Then
                        MessageBox.Show(MessageBoxPlaceHolder, " YMCA - YRS", "Participant account is locked. Please refer to Customer Service Supervisor.", MessageBoxButtons.Stop, False)
                    Else
                        MessageBox.Show(MessageBoxPlaceHolder, " YMCA - YRS", "Participant account is locked due to " + l_reasonLock + "." + " Please refer to Customer Service Supervisor.", MessageBoxButtons.Stop, False)
                    End If

                    Exit Sub
                End If

            End If

            '-----------------------------------------------------------------------------

            '--START :Suvarna B | 2020.04.30 | YRS-AT 4854 | Added validation for participant has exceed Covid amount limit
            objRefundRequest.LoadRefundConfiguration()
            objRefundRequest.CovidAmountUsed = objRefundRequest.GetCovidAmountUsed(Integer.Parse(Session("FundNo")))
            If objRefundRequest.CovidAmountUsed >= objRefundRequest.CovidAmountLimit Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_ADD_COVID).DisplayText, MessageBoxButtons.Stop, False)
                Return
            End If
            '--END :Suvarna B | 2020.04.30 | YRS-AT 4854 | Added validation for participant has exceed Covid amount limit

            '--Start: Manthan Rajguru | 2015.10.06 | YRS-AT 2595 | commented old Code and added new code to validate deceased status and message changed
            'If Me.SessionStatusType.Trim = "DA" Or Me.SessionStatusType.Trim = "DR" Then
            'MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "This Person is Deceased. So, Unable to Continue Refund Process.", MessageBoxButtons.Stop, False)
            'Exit Sub
            If Me.IsDeceased(Me.SessionStatusType) = True Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Withdrawals not allowed for deceased participants.", MessageBoxButtons.Stop, False)
                Return
                '--End: Manthan Rajguru | 2015.10.06 | YRS-AT 2595 | commented old Code and added new code to validate deceased status and message changed
            End If
            If Me.IsPendingQDRO(Me.SessionPersonID) = True Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "This Person has a pending QDRO. So, Unable to Continue Refund Process.", MessageBoxButtons.Stop, False)
                Return
            End If

            '' Check for the address. 

            If Me.AddressWebUserControl1.Address1.Trim.Length < 1 And Me.AddressWebUserControl1.Address2.Trim.Length < 1 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "An Address is not present for this Person.Please Correct this situation and Retry.", MessageBoxButtons.Stop, False)
                Return
            End If

            If Me.TextBoxTerminated.Text.Trim() = "No" And Me.RefundType.Trim = "SPEC" Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "The Participant Must be Terminated in Order to Receive a Special Refund.", MessageBoxButtons.Stop, False)
                Return
            End If

            If Me.TextBoxTerminated.Text.Trim() = "No" And Me.RefundType.Trim = "DISAB" Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "The Participant Must be Terminated in Order to Receive a Disability Refund.", MessageBoxButtons.Stop, False)
                Return
            End If

            If Me.TextBoxVested.Text.Trim = "No" And Me.RefundType.Trim = "DISAB" Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "The Participant Must be Vested in Order to Get a Disability Refund.", MessageBoxButtons.Stop, False)
                Return
            End If

            If Me.SessionStatusType.ToUpper.Trim = "QD" Then
                If Not Session("QDROParticipantAge_C19") Is Nothing Then
                    l_integer_QDROParticipantAge = CType(Session("QDROParticipantAge_C19"), Integer)
                    'Neeraj Singh 03.09.2010: issue id BT-577 
                    Dim textboxAge As Decimal

                    If Me.TextBoxAge.Text.IndexOf("Y/") > -1 Then
                        Dim tempStr As String = Me.TextBoxAge.Text.Replace("Y/", ".")
                        textboxAge = Convert.ToDecimal(IIf(tempStr.EndsWith("M"), tempStr.Trim("M"), tempStr))
                    Else
                        textboxAge = Convert.ToDecimal(Me.TextBoxAge.Text.Replace("Y", "."))
                    End If
                    If textboxAge < 55 Then
                        If l_integer_QDROParticipantAge = 0 Then
                            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Original Participant Information is not available, cannot proceed.", MessageBoxButtons.Stop, False)
                            Me.ButtonAddItem.Enabled = False
                            Return
                        End If
                    End If
                End If
            End If

            If Me.RefundType = "DISAB" Then

                If Me.SessionStatusType.Trim = "TM" Then
                ElseIf Me.SessionStatusType.Trim = "DF" Then
                Else
                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "The Participant Must be On Transition or Deferred to Get a Disability Refund.", MessageBoxButtons.Stop, False)
                    Return
                End If

            End If
            l_string_message = CheckNegativeamounts()
            If Not l_string_message Is String.Empty Then
                l_string_message = l_string_message + "<br/>" + "You must correct the data before creating the withdrawal request."
                If l_string_message.Contains("have") Then
                    MessageBox.Show(170, 300, 440, 150, Me.MessageBoxPlaceHolder, " YMCA - YRS", l_string_message, MessageBoxButtons.Stop, False)
                ElseIf l_string_message.Contains("has") Then
                    MessageBox.Show(170, 300, 440, 100, Me.MessageBoxPlaceHolder, " YMCA - YRS", l_string_message, MessageBoxButtons.Stop, False)
                End If
                ViewState("Negativeamounts") = True
                Exit Sub
            End If
            'commented by ruchi to check the pop up problem of not refreshing the data for the new selection
            ' If Me.SessionIsRefundPopupAllowed = True Then
            'Me.LoadCurrentPIA(Me.SessionFundID)
            Me.LoadAccountContribution(Me.SessionFundID)
            If Session("Count_C19") Is Nothing Then
                Session("Count_C19") = 0
            End If
            'Dim popupScript As String = "<script language='javascript'>var a;" & _
            '                    "{a=window.open('RefundRequestWebForm.aspx', 'YMCAYRS', " & _
            '                    "'width=1000,height=730,menubar=no,status=Yes,Resizable=Yes,top=0,left=0,scrollbars=yes');a.focus();}" & _
            '                    " </script>"
            Dim popupScript As String = "<script language='javascript'>var a;" & _
                                       "if(a==null ||a.closed || a.name==undefined){ a=window.open('RefundRequestWebForm_C19.aspx', 'YMCAYRS', " & _
                                       "'width=1000,height=800,menubar=no,status=Yes,Resizable=Yes,top=0,left=0,scrollbars=yes');a.location.reload();a.focus();}" & _
                                       " </script>"
            'Dim popupScript As String = "<script language='javascript'>var a;" & _
            '                    "if(a==null ||a.closed || a.name==undefined){ a=window.open('RefundRequestWebForm.aspx', 'YMCAYRS', " & _
            '                    "'width=1000,height=730,menubar=no,status=Yes,Resizable=Yes,top=0,left=0,scrollbars=yes');a.focus();}" & _
            '                    " </script>"
            Dim RegisterScript As String
            RegisterScript = "PopupScriptRR" + Convert.ToString(Session("Count_C19"))
            If (Not Me.IsStartupScriptRegistered(RegisterScript)) Then
                Session("Count_C19") = Session("Count_C19") + 1
                Page.RegisterStartupScript(RegisterScript, popupScript)
            End If
            '  Me.SessionIsRefundPopupAllowed = False
            '  End If
            'START: SG: 2012.03.23: BT-1012
            Dim l_DataTable As DataTable
            If Not Session("Member Employment_C19") Is Nothing Then
                l_DataTable = CType(Session("Member Employment_C19"), DataTable)
            Else
                Dim l_DataSet As DataSet
                l_DataSet = YMCARET.YmcaBusinessObject.RefundRequest.LookupMemberDetails(Me.SessionPersonID, Me.SessionFundID)
                l_DataTable = l_DataSet.Tables("Member Employment")
            End If

            Dim q = (From c In l_DataTable.AsEnumerable() Order By c.Field(Of DateTime)("StatusDate") Descending Take 1)
            If q.Count > 0 Then
                Dim l_TermDateDataTabel As DataTable = q.CopyToDataTable
                Session("TermDateDataTabel_C19") = l_TermDateDataTabel
            End If
            'END: SG: 2012.03.23: BT-1012

            'me.Response.Write(popupScript)
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub
    'Start: 2014.09.17:SR-BT 2633/YRS 5.0-2403 -  Remove expiration dates from withdrawal requests in YRS 
    Private Sub DataGridRequests_ItemDataBound(sender As Object, e As DataGridItemEventArgs) Handles DataGridRequests.ItemDataBound
        Try
            Dim lnkextendrequest As LinkButton
            'START: Shilpa N | 03/05/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
            If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
                lnkextendrequest = e.Item.FindControl("LinkButtonExtendExpireDate")
                If lnkextendrequest IsNot Nothing Then
                    lnkextendrequest.Enabled = False
                    lnkextendrequest.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
                End If
            End If
            'END: Shilpa N | 03/05/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
                If e.Item.Cells(m_const_Withdrawal_Status_index).Text.Trim = "PEND" Then
                    If Not String.IsNullOrEmpty(e.Item.Cells(m_const_Withdrawal_ExipreDate_index).Text) Then
                        If Date.Parse(e.Item.Cells(m_const_Withdrawal_ExipreDate_index).Text) < Today Then
                            e.Item.Cells(m_const_Withdrawal_ExipreDate_index).ForeColor = Drawing.Color.Red
                            e.Item.Cells(m_const_Withdrawal_ExipreDate_index).Font.Bold = True
                        End If
                    End If
                End If
                'START :Suvarna B | 2020.05.05 | YRS-AT 4854 | Added backcolor to row for highlighting covid request
                If e.Item.Cells(13).Text = "1" Then
                    e.Item.BackColor = Drawing.ColorTranslator.FromHtml("#FFB6C1")
                End If
                'END :Suvarna B | 2020.04.30 | YRS-AT 4854 | Added backcolor to row for highlighting covid request
            End If
        Catch ex As Exception
            HelperFunctions.LogException("DataGridRequests_ItemDataBound()", ex)
            Throw
        End Try
    End Sub
    'End: 2014.09.17:SR-BT 2633/YRS 5.0-2403 -  Remove expiration dates from withdrawal requests in YRS 


    Private Sub DataGridRequests_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridRequests.SelectedIndexChanged

        'Me.CancelRefundRequest(Me.DataGridRequests.SelectedIndex)

        'If Me.DataGridRequests.SelectedIndex <> -1 Then
        'MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "You have selected a Refund to Cancel. Do you want to Cancel the Refund Request?", MessageBoxButtons.YesNo, False)
        'End If

    End Sub

    Private Function ProcessRefundRequest(ByVal parameterSelectedIndex As Integer)

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_DataTable_RefundRequest As DataTable
        Dim l_DataRow As DataRow

        'by Aparna 03/07/2007 - To send only the Pend row for processing-YREN-3658
        'As per the Mail-To avoid the DBNull value in mnyDeductions

        Dim l_DataTable As New DataTable
        Dim l_String_RequestStatus As String
        Dim l_String_RefundRequestID As String
        Dim l_String_ErrorMessage As String
        Dim l_stringCategory As String = "Refund"
        Dim AllowPartialRefundProcess As Boolean
        Dim l_ConfigDataTable As DataTable
        Dim l_RefRequestId As String
        Dim l_RequestedDataRows As DataRow()
        Dim l_RequestedDataRow As DataRow
        Dim l_refundtype As String = String.Empty





        Try
            'Me.ISMarket = 1
            l_ConfigDataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetConfigurationCategoryWise(l_stringCategory)
            Dim drMarketValue = l_ConfigDataTable.Select("Key = 'ALLOW_MARKET_BASED_WITHDRAWAL'")
            If Convert.ToBoolean(drMarketValue(0)("Value").ToString) Then
                Me.ISMarket = 1
            Else
                Me.ISMarket = 0
            End If

            If parameterSelectedIndex < 0 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMACA - YRS", " No rows selected to Process a Refund Request.", MessageBoxButtons.OK, False)
            Else
                l_DataTable_RefundRequest = DirectCast(Session("RefundRequestDataTable_C19"), DataTable)

                l_DataTable = l_DataTable_RefundRequest.Clone()

                If Not l_DataTable_RefundRequest Is Nothing Then

                    If l_DataTable_RefundRequest.Rows.Count >= parameterSelectedIndex Then

                        l_DataRow = l_DataTable_RefundRequest.Rows.Item(parameterSelectedIndex)

                        If Not l_DataRow Is Nothing Then

                            Me.SessionRefundRequestID = CType(l_DataRow("UniqueID"), String)


                            l_RequestedDataRows = l_DataTable_RefundRequest.Select("UniqueID = '" + Me.SessionRefundRequestID + "'")

                            If l_RequestedDataRows.Length > 0 Then

                                For Each l_RequestedDataRow In l_RequestedDataRows
                                    'Added and Commented by Parveen to set the RefundType From RequestHeader Table on 17-Dec-2009 Bug ID-1073
                                    'If l_refundtype = String.Empty Then

                                    '    l_refundtype = CType(l_RequestedDataRow("RefundType"), String).Trim

                                    '    Me.RefundType = l_refundtype

                                    'Else

                                    '    If l_refundtype = "REG" Or l_refundtype = "PERS" Or l_refundtype = "HARD" Then

                                    '        'Donot change the refund type

                                    '    Else

                                    '        l_refundtype = CType(l_RequestedDataRow("RefundType"), String).Trim

                                    '        Me.RefundType = l_refundtype

                                    '    End If


                                    'End If
                                    Me.RefundType = CType(l_RequestedDataRow("RefundTypeHeader"), String).Trim
                                    l_refundtype = CType(l_RequestedDataRow("RefundType"), String).Trim
                                    'Added and Commented by Parveen to set the RefundType From RequestHeader Table on 17-Dec-2009 Bug ID-1073


                                    'Start: 4/2/2016: Bala: YRS-AT-2725: No need to reassign the value here as per this ticket
                                    'If Me.ISMarket = 1 Then
                                    '    If CType(l_RequestedDataRow("ISMarket"), Int16) = -1 Then
                                    '        Me.ISMarket = CType(l_RequestedDataRow("ISMarket"), Int16)
                                    '    End If
                                    'End If
                                    'End: 4/2/2016: Bala: YRS-AT-2725: No need to reassign the value here as per this ticket
                                    objRefundProcess.FirstInstallment = CType(l_RequestedDataRow("FirstPercentage"), Decimal)
                                    objRefundProcess.DefferredInstallment = CType(l_RequestedDataRow("DeferredPercentage"), Decimal)

                                    'implementation of Configuration settings-Amit
                                    If l_refundtype = "PART" Then
                                        'l_ConfigDataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetConfigurationCategoryWise(l_stringCategory) 'Gunanithi G: 28.01.2016: Commented for: YRS-AT-2725 moved to the top
                                        If Not l_ConfigDataTable Is Nothing Then

                                            For Each l_ConfigDataRow As DataRow In l_ConfigDataTable.Rows
                                                'Added to check the condition to allow partial withdrawal
                                                If (CType(l_ConfigDataRow("Key"), String).Trim = "ALLOW_PARTIAL_WITHDRAWAL_PROCESS") Then
                                                    AllowPartialRefundProcess = CType(l_ConfigDataRow("Value"), Boolean)
                                                End If
                                                'Added to check the condition to allow partial withdrawal
                                            Next
                                        End If
                                        If AllowPartialRefundProcess = False Then
                                            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMACA - YRS", "Partial Withdrawal Process is disabled by the Administrator. Cannot continue!", MessageBoxButtons.Stop, False)
                                            Exit Function
                                        End If
                                    End If
                                Next

                            Else
                                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", " No Refund Request Found.", MessageBoxButtons.OK, False)
                                Exit Function
                            End If


                            l_String_RequestStatus = CType(l_DataRow("RequestStatus"), String)
                            'viewstate("SessionGrossAmount") = CType(l_DataRow("Gross Amt."), Decimal)
                            Dim objRefProc As New RefundProcessing
                            objRefProc.SessionGrossAmount = CType(l_DataRow("Gross Amt."), Decimal)

                            'Plan Split Change
                            'Ragesh: this part of code has to be revisited
                            Me.GetPlanType()
                            'IB-Added on 08/08/2011-YRS 5.0-1362 : allow withdrawal when unfunded transctions exist
                            If Me.CheckForUnfundedTransaction(Me.RefundType.ToUpper.Trim, Me.PlanTypeChosen.ToUpper.Trim) = False And Request.Form("Yes") <> "Yes" Then
                                'MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Unfunded transactions exist. Withdrawal cannot be completed.", MessageBoxButtons.Stop, False)
                                Session("ProcessSelect_C19") = parameterSelectedIndex
                                Session("CancelSelect_C19") = Nothing
                                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Unfunded transactions exist. Do you want to process Refund Request?.", MessageBoxButtons.YesNo, False)
                                Exit Function
                            End If


                            If Me.RefundType.Trim = "PERS" Then
                                Me.SessionIsPersonalOnly = True
                            Else
                                Me.SessionIsPersonalOnly = False
                            End If
                            'Session("Title") = Me.LabelTitle.Text
                            If l_String_RequestStatus <> String.Empty Then

                                '' If Request Status is 'PEND' then call Cancel the Pending Request.
                                If l_String_RequestStatus.Trim = "PEND" Then

                                    l_DataTable.ImportRow(l_DataRow)
                                    Session("RefundRequestTable_C19") = l_DataTable

                                    Me.RefundStatus = "Pending"
                                   
                                    If (Me.IsYmcaPuertoRico() = True) Then  '|PK |08/30/2019|YRS-AT-2670| function to check if a participant is from Puerto Rico ymca.
                                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DialogMessage", "ShowDialog();", True)

                                    ElseIf (Me.IsYmcaPuertoRico() = False) Then

                                        Dim popupScript As String = "<script language='javascript'>" & _
                                                                                            "window.open('RefundProcessing_C19.aspx', 'YMCAYRS', " & _
                                                                                            "'width=1000, height=750, menubar=no, status=Yes,Resizable=Yes,top=0,left=0, scrollbars=yes')" & _
                                                                                            "</script>"

                                        If Me.RefundType = "PART" Then
                                            Me.SessionIsRefundProcessPopupAllowed = True
                                        End If

                                        If Me.SessionIsRefundProcessPopupAllowed = True Then

                                            If (Not Me.IsStartupScriptRegistered("PopupScriptRR")) Then
                                                Page.RegisterStartupScript("PopupScriptRR", popupScript)

                                            End If

                                            If Me.RefundType.Trim = "PART" Then
                                                Me.SessionIsRefundProcessPopupAllowed = True
                                            Else
                                                Me.SessionIsRefundProcessPopupAllowed = False
                                            End If


                                        End If
                                    End If '|PK |08/30/2019|YRS-AT-2670| function to check if a participant is from Puerto Rico ymca.
                                Else
                                    If l_String_RequestStatus.Trim = "DISB" Or l_String_RequestStatus.Trim = "PAID" Then
                                        ' This flag is used to Refresh the Refund Request Data Grid.
                                        'Me.SessionIsRefundRequest = True
                                        MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", " Request has been Processed already. Please Select 'PENDING' Refund Requests.", MessageBoxButtons.OK, False)
                                    ElseIf l_String_RequestStatus.Trim = "CANCEL" Then
                                        MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", " Request has been Cancelled. You cannot process the cancelled Refund Request.", MessageBoxButtons.OK, False)
                                    Else
                                        MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", " Request cannot be Processed. Please Select 'PENDING' Refund Requests.", MessageBoxButtons.OK, False)
                                    End If
                                End If
                                End If
                        Else
                            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", " No Refund Request Found.", MessageBoxButtons.OK, False)
                        End If
                    End If
                End If
            End If
        Catch
            Throw
        End Try
    End Function
    'Shubhrata Enhancement checks for unfunded transactions depending on Refund Type YREN 2607 Aug 14th 2006
    'Private Function CheckForUnfundedTransaction1(ByVal parameterRefundType As String)
    '    Try
    '        Dim l_flag As Boolean
    '        l_flag = True
    '        Dim dr_unfundcheck As DataRow
    '        Dim dt_unfundcheck As New DataTable
    '        Dim l_decimal_total As Decimal
    '        l_decimal_total = 0.0
    '        If Not Session("l_dataTable_AccountContribution_NonFunded") Is Nothing Then
    '            dt_unfundcheck = CType(Session("l_dataTable_AccountContribution_NonFunded"), DataTable)
    '        End If
    '        Select Case (parameterRefundType)
    '            Case "REG"
    '                If Not Session("AccountContribution_NonFunded_Total") Is Nothing Then
    '                    If Convert.ToDouble(Session("AccountContribution_NonFunded_Total")) > 0 Then
    '                        l_flag = False
    '                    End If
    '                End If
    '            Case "VOL"
    '                If dt_unfundcheck.Rows.Count > 0 Then
    '                    For Each dr_unfundcheck In dt_unfundcheck.Rows
    '                        If dr_unfundcheck("AccountType").ToString = "TD" Or dr_unfundcheck("AccountType").ToString = "AP" Then
    '                            If dr_unfundcheck("Total").GetType.ToString() <> "System.DBNull" Then
    '                                l_decimal_total += Convert.ToDecimal(dr_unfundcheck("Total"))
    '                            End If
    '                        End If
    '                    Next
    '                    If l_decimal_total > 0 Then
    '                        l_flag = False
    '                    End If
    '                End If
    '            Case "HARD"
    '                If dt_unfundcheck.Rows.Count > 0 Then
    '                    For Each dr_unfundcheck In dt_unfundcheck.Rows
    '                        If dr_unfundcheck("AccountType").ToString = "TD" Then
    '                            If dr_unfundcheck("Total").GetType.ToString() <> "System.DBNull" Then
    '                                l_decimal_total = Convert.ToDecimal(dr_unfundcheck("Total"))
    '                            End If
    '                        End If
    '                    Next
    '                    'Modified by Shubhrata YREN-3095 Mar 1st,2007
    '                    'If l_decimal_total > 0 Then
    '                    '    l_flag = False
    '                    'End If
    '                    'we will not allow unfunded transactions for negative unfunded transaction(OPPR) Issue YREN-3095
    '                    If l_decimal_total < 0 Then
    '                        l_flag = False
    '                    End If
    '                End If
    '        End Select
    '        Return l_flag
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Shubhrata Enhancement checks for unfunded transactions depending on Refund Type YREN 2607 Aug 14th 2006
    Private Function CancelRefundRequest(ByVal parameterSelectedIndex As Integer, ByVal paramRefCanResCode As String)
        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_DataTable_RefundRequest As DataTable
        Dim l_DataRow As DataRow

        Dim l_String_RequestStatus As String
        Dim l_String_RefundRequestID As String
        Dim l_String_ErrorMessage As String
        Dim var As String

        Try
            If parameterSelectedIndex < 0 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "No rows selected to Cancel a Refund Request.", MessageBoxButtons.OK, False)
            Else

                'Hafiz 03Feb06 Cache-Session
                'l_CacheManager = CacheFactory.GetCacheManager
                'l_DataTable_RefundRequest = CType(l_CacheManager.GetData("RefundRequestTable"), DataTable)
                'l_DataTable_RefundRequest = CType(Session("RefundRequestTable"), DataTable) 'by Aparna 08/08/07
                l_DataTable_RefundRequest = DirectCast(Session("RefundRequestDataTable_C19"), DataTable)
                'Hafiz 03Feb06 Cache-Session

                If Not l_DataTable_RefundRequest Is Nothing Then

                    If l_DataTable_RefundRequest.Rows.Count >= parameterSelectedIndex Then

                        l_DataRow = l_DataTable_RefundRequest.Rows.Item(parameterSelectedIndex)

                        If Not l_DataRow Is Nothing Then

                            l_String_RefundRequestID = CType(l_DataRow("UniqueID"), String)
                            'Modified the column name to fetch the status from teh header table-Amit15-12-2009
                            'l_String_RequestStatus = CType(l_DataRow("RequestStatus"), String)
                            l_String_RequestStatus = CType(l_DataRow("RequestStatusHeader"), String)
                            'Modified the column name to fetch the status from teh header table-Amit15-12-2009

                            ViewState("WD_RefundType") = CType(l_DataRow("RefundType"), String) 'SR:2013.12.06 -  BT2275/YRS 5.0-2235:Incorrect message in withdrawal process

                            If l_String_RequestStatus <> String.Empty Then

                                If l_String_RequestStatus.Trim = "PEND" Or l_String_RequestStatus.Trim = "DISB" Then

                                    '' If Request Status is 'PEND' then call Cancel the Pending Request.

                                    If l_String_RequestStatus.Trim = "PEND" Then

                                        ' This flag is used to Refresh the Refund Request Data Grid.
                                        Me.SessionIsRefundRequest = True

                                        'Shashi Shekhar:2011-25-05: YRS 5.0-1298: added one parameter for Refund cancel reason code.
                                        YMCARET.YmcaBusinessObject.RefundRequest.CancelPendingRefundRequest(l_String_RefundRequestID, paramRefCanResCode)


                                        If l_String_ErrorMessage = String.Empty Then
                                            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Refund Request has been cancelled.", MessageBoxButtons.OK, False)

                                            '' Refresh the Account Contribution DataTable
                                            Me.LoadAccountContribution(Me.SessionFundID)
                                            'added to update the session og status type-Amit Dec 30 2009
                                            Me.LoadGeneralInformation(Me.SessionPersonID, Me.SessionFundID)
                                            'added to update the session og status type-Amit Dec 30 2009
                                        End If

                                    ElseIf l_String_RequestStatus.Trim = "DISB" Then
                                        ' This flag is used to Refresh the Refund Request Data Grid.
                                        Me.SessionIsRefundRequest = True

                                        'Shashi Shekhar:2011-25-05: YRS 5.0-1298: added one parameter for Refund cancel reason code.
                                        l_String_ErrorMessage = YMCARET.YmcaBusinessObject.RefundRequest.CancelDisbursementRefund(l_String_RefundRequestID, paramRefCanResCode)

                                        If l_String_ErrorMessage = String.Empty Then
                                            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Refund Request has been cancelled.", MessageBoxButtons.OK, False)

                                            '' Refresh the Account Contribution DataTable
                                            Me.LoadAccountContribution(Me.SessionFundID)

                                        Else
                                            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", l_String_ErrorMessage, MessageBoxButtons.OK, False)
                                        End If

                                    End If

                                Else
                                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", " This Request cannot be cancelled. Please Select 'PENDING' or 'DISB' Refund Requests.", MessageBoxButtons.OK, False)
                                End If

                            End If

                        End If

                    Else
                        MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", " No Refund Request Found.", MessageBoxButtons.OK, False)
                    End If
                End If
            End If

        Catch
            Throw
        End Try
    End Function

    'Private Sub DataGridList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridList.ItemDataBound
    '    'Dim l_button_Select As ImageButton
    '    'l_button_Select = e.Item.FindControl("ImageButtonSelect")
    '    'If (e.Item.ItemIndex = Me.DataGridList.SelectedIndex And Me.DataGridList.SelectedIndex >= 0) Then
    '    '    l_button_Select.ImageUrl = "images\selected.gif"
    '    'End If
    'End Sub
    'Start:AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Commented below code because list tab has been removed and searching person will be done in findinfo.aspx
    'Private Sub DataGridList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridList.ItemCommand
    '    Try
    '        Dim l_button_Select As ImageButton
    '        l_button_Select = e.Item.FindControl("ImageButtonSelect")

    '        If (e.Item.ItemIndex = Me.DataGridList.SelectedIndex And Me.DataGridList.SelectedIndex >= 0) Then
    '            l_button_Select.ImageUrl = "images\selected.gif"
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub
    'End:AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Commented below code because list tab has been removed and searching person will be done in findinfo.aspc
    Private Sub ButtonAddNote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddNote.Click

        Try
            If Me.SessionPersonID <> String.Empty Then

                'Commented By Ashutosh Patil as on 27-Apr-2007
                'No requirement of checking Me.SessionIsNotesPopupAllowed = True
                'If Me.SessionIsNotesPopupAllowed = True Then
                Dim popupScript As String = "<script language='javascript'>" & _
                                    "window.open('NotesMasterWebForm.aspx?CF=RR', 'YMCAYRS', " & _
                                    "'width=750, height=360, menubar=no, status=Yes,Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                                    "</script>"

                If (Not Me.IsStartupScriptRegistered("PopupScriptRRN")) Then
                    Page.RegisterStartupScript("PopupScriptRRN", popupScript)
                End If

                Me.SessionIsNotesPopupAllowed = False

                'End If

                'If Me.SessionIsNotes = True Then
                '    Me.LoadMemberNotes(Me.SessionPersonID)
                '    Me.SessionIsNotes = False
                '    Me.SessionIsNotesPopupAllowed = True
                'End If

            Else
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "No Member is Selected. Please select a Member to add Notes.", MessageBoxButtons.OK, False)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub

    Private Function IsPendingQDRO(ByVal parameterPersonID As String) As Boolean

        Dim l_DataTable As DataTable

        Try

            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetQDRODetails(parameterPersonID)

            If Not (l_DataTable) Is Nothing Then

                If l_DataTable.Rows.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Function

    Private Sub DataGridRefundNotes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridRefundNotes.SelectedIndexChanged
        Try
            If Me.DataGridRefundNotes.SelectedIndex <> -1 Then
                Me.SessionNotesIndex = Me.DataGridRefundNotes.SelectedIndex

                Dim popupScript As String = "<script language='javascript'>" & _
                        "window.open('NotesMasterWebForm.aspx?CF=RR&MD=VW', 'YMCAYRS', " & _
                        "'width=750, height=360, status=Yes,menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                        "</script>"

                If (Not Me.IsStartupScriptRegistered("PopupScriptRRNW")) Then
                    Page.RegisterStartupScript("PopupScriptRRNW", popupScript)
                End If


            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ShowNotes()

        Try

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub DataGridEmployment_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridEmployment.ItemDataBound
        Try


            e.Item.Cells(0).Visible = False
            e.Item.Cells(1).Visible = False
            e.Item.Cells(2).Visible = False
            e.Item.Cells(3).Visible = False
            e.Item.Cells(4).Visible = False
            e.Item.Cells(8).Visible = False
            e.Item.Cells(9).Visible = False
            e.Item.Cells(10).Visible = False
            e.Item.Cells(11).Visible = False
            e.Item.Cells(12).Visible = False
            e.Item.Cells(13).Visible = False
            e.Item.Cells(14).Visible = False
            e.Item.Cells(15).Visible = False
            e.Item.Cells(16).Visible = False
            e.Item.Cells(17).Visible = False
            'e.Item.Cells(18).Visible = False
            'e.Item.Cells(19).Visible = False
            'e.Item.Cells(20).Visible = False
            e.Item.Cells(21).Visible = False
            e.Item.Cells(22).Visible = False
            e.Item.Cells(23).Visible = False
            e.Item.Cells(24).Visible = False
            e.Item.Cells(25).Visible = False
            ' e.Item.Cells(26).Visible = False

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub DataGridFundedAccountContributions_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridFundedAccountContributions.ItemDataBound
        Try
            e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfIsBasicAcct).Visible = False
            e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfPlanType).Visible = False
            e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfAcctGroup).Visible = False

            'anita and shubhrata apr21
            If e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfAcctType).Text.ToUpper <> "&NBSP;" And e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfAcctType).Text.ToUpper <> "ACCOUNTTYPE" Then
                '    If e.Item.Cells(0).Text.ToUpper = "TOTAL" Then
                'anita and shubhrata apr21


                Dim l_decimal_try As Decimal
                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfTaxable).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfTaxable).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfNonTaxable).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfNonTaxable).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfInterest).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfInterest).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfEmpTotal).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfEmpTotal).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfYmcaTaxable).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfYmcaTaxable).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfYmcaInterest).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfYmcaInterest).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfYmcaTotal).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfYmcaTotal).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfTotal).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfTotal).Text = FormatCurrency(l_decimal_try)
            End If

            ' End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub


    Private Sub DataGridRefundNotes_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridRefundNotes.ItemDataBound
        Try
            ''START: Added by Dilip Yadav : 2009.09.18 : To restrict the text display till only 50 characters.
            If e.Item.Cells(1).Text.Length > 50 Then
                e.Item.Cells(1).Text = e.Item.Cells(1).Text.Substring(0, 50)
            End If
            ''END: Added by Dilip Yadav : 2009.09.18 : To restrict the text display till only 50 characters.

            'commented by aparna -Change in position of the uniqueid column-yren-3115 15/03/2007
            ' e.Item.Cells(1).Visible = False
            'yren-3115 15/03/2007
            Dim l_checkbox As New CheckBox
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                l_checkbox = e.Item.FindControl("CheckBoxImportant")
                If l_checkbox.Checked Then
                    Me.NotesFlag.Value = "MarkedImportant"

                End If
                'If e.Item.Cells(2).Text.Trim = Session("LoginId") Or NotesGroupUser = True Then
                '    l_checkbox.Enabled = True
                'End If
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Function IsRequestOpen(ByVal param_string_RequestStatus As String) As Boolean
        Dim l_dataset_RequestStatus As DataSet
        Dim l_datarows As DataRow()
        Dim l_bool_RequestOpen As Boolean = False
        Try
            l_dataset_RequestStatus = YMCARET.YmcaBusinessObject.RefundRequest.GetRequestStatus(param_string_RequestStatus)


            If Not l_dataset_RequestStatus Is Nothing Then
                If l_dataset_RequestStatus.Tables.Count > 0 Then
                    If l_dataset_RequestStatus.Tables(0).Rows.Count = 1 Then
                        If l_dataset_RequestStatus.Tables(0).Rows(0)("bitClosed") = 0 Then
                            l_bool_RequestOpen = True
                        End If
                    ElseIf l_dataset_RequestStatus.Tables(0).Rows.Count > 1 Then
                        l_datarows = l_dataset_RequestStatus.Tables(0).Select("chrRequestStatus='" & param_string_RequestStatus & "'")
                        If l_datarows.Length > 0 Then
                            If l_datarows(0)("bitClosed") = 0 Then
                                l_bool_RequestOpen = True
                            End If
                        End If
                    End If
                End If
            End If

            Return l_bool_RequestOpen
        Catch
            Throw
        End Try
    End Function
    Private Function ValidateRefundRequest(ByVal param_string_RequestStatus As String, ByVal param_string_RefundType As String) As String
        'Start of Code Added by hafiz on 22-Sep-2006 - YREN-2702
        Dim l_String_Message As String = ""
        Dim l_bool_RequestOpen As Boolean = False
        Try
            l_bool_RequestOpen = IsRequestOpen(param_string_RequestStatus)

            If l_bool_RequestOpen = True Then
                ' --Start: Manthan Rajguru | 2015.10.06 | YRS-AT 2595 | commented old Code and added new code to validate deceased status
                'If Me.SessionStatusType.Trim = "DA" Or Me.SessionStatusType.Trim = "DR" Then
                If Me.IsDeceased(Me.SessionStatusType) = True Then
                    ' --End: Manthan Rajguru | 2015.10.06 | YRS-AT 2595 | commented old Code and added new code to validate deceased status
                    l_String_Message = "This Person is Deceased. Withdrawal Request is being Canceled."
                End If

                If Me.IsPendingQDRO(Me.SessionPersonID) = True Then
                    l_String_Message = "This Person has a pending QDRO. Withdrawal Request is being Canceled."
                End If

                'START : Added by Sanjeev on 21th Dec 2011 for BT:955
                'If param_string_RefundType.Trim() <> "VOL" Then
                If param_string_RefundType.Trim() <> "VOL" AndAlso param_string_RefundType.Trim() <> "PART" Then
                    If param_string_RefundType.Trim() = "HARD" Then
                        If Not objRefundProcess.IsNonParticipatingPerson Then 'SB | YRS-AT-2169 | 20.6.5 | Hardship withdrawal is allowed for NO/PENP and RDNP participants which are terminated
                            If IsTerminated = True Then
                                'l_String_Message = "This Fund Event's Status has become Active" 'message as in foxpro
                                l_String_Message = "Participant is terminated. Withdrawal Request is being Canceled" 'message as in gemini
                            End If
                        End If 'SB | YRS-AT-2169 | 20.6.5 | Hardship withdrawl is allowed for NO/PENP and RDNP participants which are terminated
                    Else
                        If IsTerminated = False Then
                            'l_String_Message = "This Fund Event's Status has become Active" 'message as in foxpro
                            l_String_Message = "Participant is now active. Withdrawal Request is being Canceled" 'message as in gemini
                        End If
                    End If
                End If
            End If

            Return l_String_Message
        Catch
            Throw
        End Try
        'End of Code Added by hafiz on 22-Sep-2006 - YREN-2702
    End Function
    Private Sub DataGridRequests_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridRequests.ItemCommand

        Dim l_bool_readonly As Boolean
        Dim refRequestID As String 'PK | 05/08/2020 | Declared variable.
        Try
            'Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            If e.CommandName = "ProcessSelect" Or e.CommandName = "CancelSelect" Then
                'AA:2014.02.03:BT:2292:YRS 5.0 - 2248 : Changed the Formname according to the rights given in acuyussi table
                'Dim strFormName As String = "RefundRequestForm.aspx" 'Added by Shashi Shekhar:2009-12-31
                Dim strFormName As String = "FindInfo.aspx?Name=Refund&RT=C19" 'Added by Shashi Shekhar:2009-12-31

                Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

                If Not checkSecurity.Equals("True") Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                    Exit Sub
                End If

            End If
            'End : YRS 5.0-940 

            'Rahul 13Feb06
            Dim l_DataTable_RefundRequest As DataTable
            Dim l_DataRow As DataRow
            Dim l_String_RequestStatus As String = ""
            Dim l_String_RefundType As String = ""
            Dim l_String_Message As String = ""
            Dim l_String_PlanType As String = "" 'Bala: 03.02.2016: YRS-AT-2821: Declaring Plan type.

            ' l_DataTable_RefundRequest = CType(Session("RefundRequestTable"), DataTable) 'by Aparna 08/08/07
            l_DataTable_RefundRequest = DirectCast(Session("RefundRequestDataTable_C19"), DataTable)

            If Not l_DataTable_RefundRequest Is Nothing Then
                If l_DataTable_RefundRequest.Rows.Count >= e.Item.ItemIndex Then
                    l_DataRow = l_DataTable_RefundRequest.Rows.Item(e.Item.ItemIndex)
                    If Not l_DataRow Is Nothing Then
                        'Modified the column name to fetch the status from the header table-Parveen 15-12-2009
                        'l_String_RequestStatus = CType(l_DataRow("RequestStatus"), String)
                        l_String_RequestStatus = CType(l_DataRow("RequestStatusHeader"), String)
                        'Modified the column name to fetch the status from the header table-Parveen 15-12-2009
                        l_String_RefundType = CType(l_DataRow("RefundType"), String)
                        l_String_PlanType = CType(l_DataRow("PlanType"), String) 'Bala: 03.02.2016: YRS-AT-2821: Getting Plan type.
                    End If
                End If
            End If
            'Start- chandrasekar.c:2016.01.11 YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request
            If e.CommandName = "ExtendExpireSelect" Then

                'START: PK | 05/05/2020 | YRS-AT-4854 | Validation to check requested refund withdrawal is covid or not.
                refRequestID = Me.GetSelectedRefRequestID(e.Item.ItemIndex)
                If refRequestID <> String.Empty Then
                    If (IsRequestCovid(refRequestID) = False) Then
                        Dim message As String = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_EXTEND_COVID_SCREEN).DisplayText
                        MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", message, MessageBoxButtons.Stop, False)
                        Exit Sub
                    End If
                Else
                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", " No refund request found. Please reload the page and try again.", MessageBoxButtons.OK, False)
                    Exit Sub
                End If
               
                'END: PK | 05/05/2020 | YRS-AT-4854 | Validation to check requested refund withdrawal is covid or not.

                l_String_Message = String.Format("Are you sure you want to extend the expiration date of selected withdrawal request? The new expiration date will be {0}", System.DateTime.Now.AddDays(Me.RefundExtendExpireDays).ToString("MM/dd/yyyy"))
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", l_String_Message, MessageBoxButtons.YesNo, False)

                If Not l_DataTable_RefundRequest Is Nothing Then
                    If l_DataTable_RefundRequest.Rows.Count >= e.Item.ItemIndex Then
                        l_DataRow = l_DataTable_RefundRequest.Rows.Item(e.Item.ItemIndex)
                        If Not l_DataRow Is Nothing Then
                            ViewState("IsExpireOption") = True
                            ViewState("RefundRequestUniqueID") = CType(l_DataRow("UniqueID"), String)
                            ViewState("RefundRequestExpireDate") = CType(l_DataRow("ExpireDate"), String)
                            ViewState("RefundRequestAmount") = CType(l_DataRow("Gross Amt."), String)
                            ViewState("RefundRequestPlanType") = CType(l_DataRow("PlanType"), String)
                            ViewState("RefundRequestExtendExpireDate") = System.DateTime.Now.AddDays(Me.RefundExtendExpireDays).ToString("MM/dd/yyyy")
                        End If
                    End If
                End If
            End If
            'End- chandrasekar.c:2016.01.11 YRS-AT-2524: Give Customer Service the ability to unexpire an expired withdrawal request

            'Added By Ashutosh Patil as on 16-Apr-2007
            'For Security Check
            'l_bool_readonly = CheckReadOnlyRights()
            'If l_bool_readonly = True Then
            '    Exit Sub
            'End If                    
            'Added by Parveen on 09-Dec-2009 for the Hardship Issue
            Session("ForceToVoluntaryWithdrawal_C19") = Nothing
            'Added by Parveen on 09-Dec-2009 for the Hardship Issue
            If e.CommandName = "ProcessSelect" Then

                'START: PK | 05/05/2020 | YRS-AT-4854 | Validation to check requested refund withdrawal is covid or not.
                refRequestID = Me.GetSelectedRefRequestID(e.Item.ItemIndex)
                If refRequestID <> String.Empty Then
                    If (Me.IsRequestCovid(refRequestID) = False) Then
                        Dim message As String = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_PROCESS_COVID_SCREEN).DisplayText
                        MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", message, MessageBoxButtons.Stop, False)
                        Exit Sub
                    End If
                Else
                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", " No refund request found. Please reload the page and try again.", MessageBoxButtons.OK, False)
                    Exit Sub
                End If
                'END: PK | 05/05/2020 | YRS-AT-4854 | Validation to check requested refund withdrawal is covid or not.

                '--------------------------------------------------------------------------------------
                'Shashi Shekhar:10 Feb 2011:  For YRS 5.0-1236 : Need ability to freeze/lock account
                If Not Me.IsAccountLock = Nothing Then
                    If Me.IsAccountLock.ToString.Trim.ToLower = "true" Then
                        '---Shashi Shekhar :14 - Feb -2011: For BT-750 While QDRO split message showing wrong.-
                        Dim l_dsLockResDetails As DataSet
                        Dim l_reasonLock As String
                        If Not Me.TextBoxSSNo.Text = String.Empty Then
                            l_dsLockResDetails = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.GetLockReasonDetails(Me.TextBoxSSNo.Text.ToString.Trim)
                        End If

                        If Not l_dsLockResDetails Is Nothing Then
                            If l_dsLockResDetails.Tables("GetLockReasonDetails").Rows.Count > 0 Then
                                If (l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString() <> "System.DBNull" And l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString() <> "") Then

                                    l_reasonLock = l_dsLockResDetails.Tables("GetLockReasonDetails").Rows(0).Item("ReasonDesc").ToString().Trim
                                End If
                            End If
                        End If
                        If l_reasonLock = "" Then
                            MessageBox.Show(MessageBoxPlaceHolder, " YMCA - YRS", "Participant account is locked. Please refer to Customer Service Supervisor.", MessageBoxButtons.Stop, False)
                        Else
                            MessageBox.Show(MessageBoxPlaceHolder, " YMCA - YRS", "Participant account is locked due to " + l_reasonLock + "." + " Please refer to Customer Service Supervisor.", MessageBoxButtons.Stop, False)
                        End If

                        Exit Sub
                    End If

                End If

                '-----------------------------------------------------------------------------

                If l_String_RequestStatus.Trim = "PEND" Then
                    'If l_String_RequestStatus.Trim = "PEND" Then
                    'MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Are you sure you want to Process Refund.", MessageBoxButtons.YesNo, True)
                    'Session("ProcessSelect") = e.Item.ItemIndex
                    'End If

                    'Start of Code Added by hafiz on 22-Sep-2006 - YREN-2702
                    l_String_Message = ValidateRefundRequest(l_String_RequestStatus, l_String_RefundType)
                    If l_String_Message.Trim <> "" Then
                        Session("bool_RefundRequest_C19") = True
                        Session("CancelSelect_C19") = e.Item.ItemIndex
                        MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", l_String_Message, MessageBoxButtons.OK, False)
                        Exit Sub
                    End If
                    'End of Code Added by hafiz on 22-Sep-2006 - YREN-2702

                    l_String_Message = CheckNegativeamounts()
                    If Not l_String_Message Is String.Empty Then
                        l_String_Message = l_String_Message + "<br/>" + "You must correct the data before processing the withdrawal request."
                        If l_String_Message.Contains("have") Then
                            MessageBox.Show(170, 300, 440, 150, Me.MessageBoxPlaceHolder, " YMCA - YRS", l_String_Message, MessageBoxButtons.Stop, False)
                        ElseIf l_String_Message.Contains("has") Then
                            MessageBox.Show(170, 300, 440, 100, Me.MessageBoxPlaceHolder, " YMCA - YRS", l_String_Message, MessageBoxButtons.Stop, False)
                        End If
                        ViewState("Negativeamounts") = True
                        Exit Sub
                    End If

                    'START: SB | 2017.08.02 | YRS-AT-3324 | Added method to validate withdrawal restricitions for RMD eligbile participants
                    If Not Me.IsPersonEligibleForRMD Then
                        Dim index As Integer = Me.ReasonForRestriction.IndexOf("<br />")
                        'For multiple reasons for restricition display message with exteneded height and width
                        If index >= 0 Then
                            MessageBox.Show(170, 300, 560, 175, Me.MessageBoxPlaceHolder, "YMCA-YRS", Me.ReasonForRestriction, MessageBoxButtons.Stop, False)
                            Exit Sub
                        End If
                        MessageBox.Show(170, 300, 480, 130, Me.MessageBoxPlaceHolder, "YMCA-YRS", Me.ReasonForRestriction, MessageBoxButtons.Stop, False)
                        Exit Sub
                    End If
                    'END: SB | 2017.07.28 | YRS-AT-3324 | Added method to validate withdrawal restricitions for RMD eligbile participants

                    Me.ProcessRefundRequest(e.Item.ItemIndex)
                Else
                    If l_String_RequestStatus.Trim = "DISB" Or l_String_RequestStatus.Trim = "PAID" Then
                        ' This flag is used to Refresh the Refund Request Data Grid.
                        'Me.SessionIsRefundRequest = True
                        l_String_Message = " Request has been Processed already. Please Select 'PENDING' Refund Requests."
                    ElseIf l_String_RequestStatus.Trim = "CANCEL" Then
                        l_String_Message = " Request has been Cancelled. You cannot process the cancelled Refund Request."
                    Else
                        l_String_Message = " Request cannot be Processed. Please Select 'PENDING' Refund Requests."
                    End If

                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", l_String_Message, MessageBoxButtons.OK, False)
                End If


            ElseIf e.CommandName = "CancelSelect" Then

                'START: PK | 05/05/2020 | YRS-AT-4854 | Validation to check requested refund withdrawal is covid or not.
                refRequestID = Me.GetSelectedRefRequestID(e.Item.ItemIndex)
                If refRequestID <> String.Empty Then
                    If (IsRequestCovid(refRequestID) = False) Then
                        Dim message As String = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_CANCEL_COVID_SCREEN).DisplayText
                        MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", message, MessageBoxButtons.Stop, False)
                        Exit Sub
                    End If
                Else
                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", " No refund request found. Please reload the page and try again.", MessageBoxButtons.OK, False)
                    Exit Sub
                End If
                'END: PK | 05/05/2020 | YRS-AT-4854 | Validation to check requested refund withdrawal is covid or not.

                If l_String_RequestStatus.Trim = "PEND" Or l_String_RequestStatus.Trim = "DISB" Then
                    'START: PPP | 11/27/2017 | YRS-AT-3653 | Checks whether SHIRA request can be canceled or not
                    '-- OLD CODE
                    ''Start: Bala: 03.02.2016: YRS-AT-2821: User should not cancel the pending SHIRA request if it has other cancelled SHIRA request in the list.
                    ''1. Check if the request for cancellation has SHIRA type.
                    ''2. Check if there is any SHIRA request already cancelled based on the plan
                    ''2.1. Request for cancellation has not plan type of both. Check if the participant has already cancelled SHIRA request in the same plan or in both plans.
                    ''2.2. Request for cancellation has plan type of both. Check if the participant has already cancelled SHIRA request any plans.
                    'Dim l_String_Query = IIf(l_String_PlanType.ToUpper() <> "BOTH",
                    '                         "RefundType = 'SHIRA' AND RequestStatus = 'CANCEL' AND PlanType IN ('BOTH','" & l_String_PlanType.ToUpper() & "')",
                    '                         "RefundType = 'SHIRA' AND RequestStatus = 'CANCEL'")
                    'If l_String_RefundType.Trim.ToUpper() = "SHIRA" And l_DataTable_RefundRequest.Select(l_String_Query).Count > 0 Then
                    '    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", GetMessage("MESSAGE_WD_SHIRA_CANCEL_ONCE"), MessageBoxButtons.OK, False)
                    '    Exit Sub
                    'End If
                    ''End: Bala: 03.02.2016: YRS-AT-2821: User should not cancel the pending SHIRA request if it has other cancelled SHIRA request in the list.
                    '-- NEW CODE
                    ' First checking refund type, if it is SHIRA then looking for canceled request.
                    ' if a cancel request is exists then only look into db to check eligibility of canceling multiple SHIRA request.
                    If l_String_RefundType.Trim.ToUpper() = "SHIRA" Then
                        Dim l_String_Query = "RefundType = 'SHIRA' AND RequestStatus = 'CANCEL'"
                        If l_DataTable_RefundRequest.Select(l_String_Query).Count > 0 Then
                            If Not YMCARET.YmcaBusinessObject.RefundRequest.IsSHIRARequestCancelable(Me.SessionPersonID) Then
                                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", GetMessage("MESSAGE_WD_SHIRA_CANCEL_ONCE"), MessageBoxButtons.OK, False)
                                Exit Sub
                            End If
                        End If
                    End If
                    'END: PPP | 11/27/2017 | YRS-AT-3653 | Checks whether SHIRA request can be canceled or not
                    'If l_String_RequestStatus.Trim = "PEND" Or l_String_RequestStatus.Trim = "DISB" Then

                    '----Shashi-----------------
                    'Shashi Shekhar:2011-25-05: YRS 5.0-1298: added one parameter for Refund cancel reason code.
                    Dim popupScript As String = "<script language='javascript'>" & _
                    "window.open('RefundRequestCancel.aspx', 'YMCAYRS', " & _
                    "'width=800, height=360, menubar=no, status=Yes,Resizable=Yes,top=120,left=120, scrollbars=yes')" & _
                    "</script>"

                    If (Not Me.IsStartupScriptRegistered("PopupScriptRRN")) Then
                        Page.RegisterStartupScript("PopupScriptRRN", popupScript)
                    End If

                    ViewState("CanReq") = "TRUE"
                    ' MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Are you sure you want to Cancel Refund?", MessageBoxButtons.YesNo, False)
                    Session("CancelSelect_C19") = e.Item.ItemIndex
                    'Else
                    'End If
                    'Me.CancelRefundRequest(e.Item.ItemIndex)
                Else
                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", " This Request cannot be cancelled. Please Select 'PENDING' or 'DISB' Refund Requests.", MessageBoxButtons.OK, False)
                End If
            End If

            'Me.CancelRefundRequest(e.Item.ItemIndex)

            'Rahul 13Feb06
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Dim closePopupscript As String

        Try
            Session("RefundRequest_Sort_C19") = Nothing
            Session("l_emptotal_C19") = Nothing
            Session("l_grossamt_C19") = Nothing
            Session("ProcessSelect_C19") = Nothing
            Session("CancelSelect_C19") = Nothing
            'Start:AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Added below code because list tab has been removed and searching person will be done in findinfo.aspx
            Session("Page_C19") = "Person"

            'START: SB | 2017.08.08 | YRS-AT-3324 | Clearing session variables 
            Session("ReasonForRestriction_C19") = Nothing
            Session("WithdrawalAllowedForRMDEligibleParticipants_C19") = Nothing
            'END: SB | 2017.08.08 | YRS-AT-3324 | Clearing session variables 

            'If Page.Request.QueryString("RT") Is Nothing Then
            Response.Redirect("FindInfo.aspx?Name=Refund&RT=C19", False)
            'Else
            '    Dim RefundType As String
            '    RefundType = CType(Page.Request.QueryString("RT"), String)
            '    If RefundType.Trim = "SPEC" Then
            '        Response.Redirect("FindInfo.aspx?Name=Refund&RT=SPEC", False)
            '    ElseIf RefundType.Trim = "DISAB" Then
            '        Response.Redirect("FindInfo.aspx?Name=Refund&RT=DISAB", False)
            '    End If
            'End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub ButtonUpdateAddress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUpdateAddress.Click
        '*************Code Added by Ashutosh on 3-05-06***********
        ' Session("ds_PrimaryAddress") = Nothing
        Dim popupScript As String
        'property setting
        'Session("l_integer_AccPermission") = 1

        Try

            'Commented by Shashi Shekhar:2009-12-31
            ''Added by neeraj on 24-Nov-2009 : issue is YRS 5.0-940
            'Dim checkSecurity As String = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))

            'If Not checkSecurity.Equals("True") Then
            '    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
            '    Exit Sub
            'End If
            ''End : YRS 5.0-940 

            '---------------------------------------------------------------------------------------
            'Added by Shashi Shekhar:2009-12-31: To resolve the security Issue(Ref:Security Review excel sheet)
            Dim checkSecurity As String
            Dim strFormName As String
            'START : VC | 2018.11.22 | YRS-AT-4018 - To show error message if user tries to update address while loan is in progress
            Dim messageNumber As Integer
            messageNumber = YMCARET.YmcaBusinessObject.CommonBOClass.ValidatePIIRestrictions(Session("PersonID"))
            'If messageNumber is exist then show error message
            If HelperFunctions.isNonEmpty(messageNumber) And messageNumber <> 0 Then
                DivErrorMessage.InnerHtml = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(messageNumber).DisplayText
                Exit Sub
            End If
            'END : VC | 2018.11.22 | YRS-AT-4018 - To show error message if user tries to update address while loan is in progress

            'If Me.RefundType.Trim = "DISAB" Then
            '    'AA:2014.02.03:BT:2292:YRS 5.0 - 2248 : Changed the Formname according to the rights given in acuyussi table
            '    'strFormName = "RefundRequestForm.aspx?RT=DISAB"
            '    strFormName = "FindInfo.aspx?Name=Refund&RT=DISAB"
            '    checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            'ElseIf Me.RefundType.Trim = "SPEC" Then
            '    'AA:2014.02.03:BT:2292:YRS 5.0 - 2248 : Changed the Formname according to the rights given in acuyussi table
            '    'strFormName = "RefundRequestForm.aspx?RT=SPEC"
            '    strFormName = "FindInfo.aspx?Name=Refund&RT=SPEC"
            '    checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            'Else
            'AA:2014.02.03:BT:2292:YRS 5.0 - 2248 : Changed the Formname according to the rights given in acuyussi table
            'strFormName = "RefundRequestForm.aspx"
            strFormName = "FindInfo.aspx?Name=Refund&RT=C19"
            checkSecurity = SecurityCheck.Check_Authorization(strFormName, Convert.ToInt32(Session("LoggedUserKey")))
            'End If

            If Not checkSecurity.Equals("True") Then
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", checkSecurity, MessageBoxButtons.Stop)
                Exit Sub
            End If

            '--------------------------------------------------------------------------------------

            'Ashutosh Patil as on 08-Apr-2007
            'For adjusting height and width of the Pop-Up Window.
            popupScript = "<script language='javascript'>" & _
                                                      "var UpdateAddress=window.open('UpdateAddressDetails.aspx', 'CustomPopUp', " & _
                                                      "'width=710, height=550, status=Yes,menubar=no, Resizable=no,top=50,left=120, scrollbars=no')" & _
                                                      "</script>"



            Page.RegisterStartupScript("PopupScript1", popupScript)

            '*********************************
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Start:AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Commented below code because list tab has been removed and searching person will be done in findinfo.aspc
    'Private Sub DataGridList_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridList.SortCommand
    '    Try
    '        Dim l_DataSet As DataSet
    '        Me.DataGridList.SelectedIndex = -1
    '        If Not ViewState("RefundRequest_Sort") Is Nothing Then
    '            Dim dv As New DataView
    '            Dim SortExpression As String
    '            SortExpression = e.SortExpression
    '            l_DataSet = ViewState("RefundRequest_Sort")
    '            dv = l_DataSet.Tables(0).DefaultView
    '            dv.Sort = SortExpression
    '            If Not Session("RefundRequest_Sort") Is Nothing Then
    '                If Session("RefundRequest_Sort").ToString.Trim.EndsWith("ASC") Then
    '                    dv.Sort = SortExpression + " DESC"
    '                Else
    '                    dv.Sort = SortExpression + " ASC"
    '                End If
    '            Else
    '                dv.Sort = SortExpression + " ASC"
    '            End If
    '            Me.DataGridList.DataSource = Nothing
    '            Me.DataGridList.DataSource = dv
    '            Me.DataGridList.DataBind()
    '            Session("RefundRequest_Sort") = dv.Sort
    '        End If
    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try
    'End Sub

    'Private Sub DataGridList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridList.ItemDataBound
    '    Try
    '        e.Item.Cells(4).Visible = False
    '        e.Item.Cells(5).Visible = False
    '        e.Item.Cells(8).Visible = False 'Shashi Shekhar:2010-02-12 :Hide IsArchived Field in grid
    '        e.Item.Cells(9).Visible = False 'Shashi Shekhar:2011-02-10 :Hide IsLock Field in grid

    '    Catch ex As Exception
    '        Dim l_String_Exception_Message As String
    '        l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
    '    End Try

    'End Sub
    'End:AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Commented below code because list tab has been removed and searching person will be done in findinfo.aspc
    'anita and shubhrata
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
    'anita and shubhrata

    Private Sub DataGridNonFundedAccountContributions_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridNonFundedAccountContributions.ItemDataBound
        Try
            e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfPlanType).Visible = False
            e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfAcctGroup).Visible = False
            e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfISBasicAcct).Visible = False



            If e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfAcctType).Text.ToUpper <> "&NBSP;" And e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfAcctType).Text.ToUpper <> "ACCOUNTTYPE" Then
                Dim l_decimal_try As Decimal
                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfTaxable).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfTaxable).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfNonTaxable).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfNonTaxable).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfInterest).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfInterest).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfEmpTotal).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfEmpTotal).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfYmcaTaxable).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfYmcaTaxable).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfYmcaInterest).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfYmcaInterest).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfYmcaTotal).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfYmcaTotal).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfTotal).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfTotal).Text = FormatCurrency(l_decimal_try)
            End If


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

    Private Sub ButtonAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click

    End Sub
    'Start:AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Commented below code because list tab has been removed and searching person will be done in findinfo.aspc
    'Private Sub DataGridList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridList.PageIndexChanged

    'End Sub
    'Private Function GetSelectedFundID(ByVal index As Integer) As String

    '    Dim l_DataRow As DataRow
    '    Dim l_DataSet As DataSet
    '    Dim l_DataTable As DataTable
    '    'Hafiz 03Feb06 Cache-Session
    '    'Dim l_Cache As CacheManager
    '    'Hafiz 03Feb06 Cache-Session

    '    Try

    '        'Hafiz 03Feb06 Cache-Session
    '        'l_Cache = CacheFactory.GetCacheManager
    '        'l_DataSet = CType(l_Cache.GetData("FindPerson"), DataSet)
    '        l_DataSet = DirectCast(Session("FindPerson"), DataSet)
    '        'Hafiz 03Feb06 Cache-Session

    '        If Not l_DataSet Is Nothing Then
    '            l_DataTable = l_DataSet.Tables("Persons")

    '            If Not l_DataTable Is Nothing Then
    '                'Changed By:preeti On:10thFeb06 IssueId:YRST-2092 start
    '                Dim arrDr() As DataRow = l_DataTable.Select("[guiFundEventId]='" & Me.DataGridList.SelectedItem.Cells(5).Text.Trim() & "'")
    '                'Dim arrDr() As DataRow = l_DataTable.Select("[SS No]=" & Me.DataGridList.SelectedItem.Cells(1).Text.Trim())
    '                'l_DataRow = l_DataTable.Rows.Item(index)
    '                If arrDr.Length > 0 Then
    '                    l_DataRow = arrDr(0)
    '                Else
    '                    Exit Function
    '                End If
    '                'Changed By:preeti On:10thFeb06 IssueId:YRST-2092 End

    '                If Not l_DataRow Is Nothing Then
    '                    Me.SessionFundID = CType(l_DataRow("guiFundEventId"), String)
    '                    Return (CType(l_DataRow("guiFundEventId"), String))
    '                Else
    '                    Me.SessionFundID = String.Empty
    '                    Return String.Empty
    '                End If
    '            End If
    '        End If
    '    Catch
    '        Throw
    '    End Try
    'End Function

    'Shashi Shekhar:10 feb 2011: For YRS 5.0-1236
    'Private Function GetSelectedAccountLockStatus(ByVal index As Integer) As String

    '    Dim l_DataRow As DataRow
    '    Dim l_DataSet As DataSet
    '    Dim l_DataTable As DataTable

    '    Try
    '        l_DataSet = DirectCast(Session("FindPerson"), DataSet)

    '        If Not l_DataSet Is Nothing Then
    '            l_DataTable = l_DataSet.Tables("Persons")

    '            If Not l_DataTable Is Nothing Then
    '                Dim arrDr() As DataRow = l_DataTable.Select("[IsLocked]='" & Me.DataGridList.SelectedItem.Cells(9).Text.Trim() & "'")
    '                If arrDr.Length > 0 Then
    '                    l_DataRow = arrDr(0)
    '                Else
    '                    Exit Function
    '                End If
    '                If Not l_DataRow Is Nothing Then
    '                    Me.IsAccountLock = CType(l_DataRow("IsLocked"), String)
    '                    Return (CType(l_DataRow("IsLocked"), String))
    '                Else
    '                    Me.IsAccountLock = String.Empty
    '                    Return String.Empty
    '                End If
    '            End If
    '        End If
    '    Catch
    '        Throw
    '    End Try
    'End Function
    'End:AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Commented below code because list tab has been removed and searching person will be done in findinfo.aspc



    'By aparna -YREN-3115 -15/03/2007
    Protected Sub Check_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim l_datatable_Notes As New DataTable
            Dim l_string_Uniqueid As String
            Dim l_String_Search As String
            Dim l_datarow As DataRow()
            Dim l_datarowUpdated As DataRow
            l_datatable_Notes = Session("MemberNotes")
            'Vipul 01Feb06 Cache-Session
            Dim l_checkbox As CheckBox = CType(sender, CheckBox)
            Dim dgItem As DataGridItem = CType(l_checkbox.NamingContainer, DataGridItem)
            'now we've got what we need!

            If dgItem.Cells(4).Text.ToUpper <> "&NBSP;" Then
                l_string_Uniqueid = dgItem.Cells(4).Text
                l_String_Search = " UniqueID = '" + l_string_Uniqueid + "'"

                l_datarow = l_datatable_Notes.Select(l_String_Search)
                l_datarowUpdated = l_datarow(0)

                If l_checkbox.Checked = True Then
                    l_datarowUpdated("bitImportant") = 1
                Else
                    l_datarowUpdated("bitImportant") = 0
                End If
                Session("MemberNotes") = l_datatable_Notes
                '  Me.ButtonSave.Visible = True
                ' Me.ButtonSave.Enabled = True
            End If



        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
        End Try
    End Sub
    'Shubhrata May21st,2007 Plan Split Changes
    Private Sub DataGridFundedAccntContributionsSavingsPlan_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridFundedAccntContributionsSavingsPlan.ItemDataBound
        Try
            e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfIsBasicAcct).Visible = False
            e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfPlanType).Visible = False
            e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfAcctGroup).Visible = False

            'anita and shubhrata apr21
            If e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfAcctType).Text.ToUpper <> "&NBSP;" And e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfAcctType).Text.ToUpper <> "ACCOUNTTYPE" Then

                Dim l_decimal_try As Decimal
                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfTaxable).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfTaxable).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfNonTaxable).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfNonTaxable).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfInterest).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfInterest).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfEmpTotal).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfEmpTotal).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfYmcaTaxable).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfYmcaTaxable).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfYmcaInterest).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfYmcaInterest).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfYmcaTotal).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfYmcaTotal).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfTotal).Text)
                e.Item.Cells(mConst_DataGridFundedAcctContributionsIndexOfTotal).Text = FormatCurrency(l_decimal_try)
            End If

            ' End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Shubhrata May21st,2007 Plan Split Changes

    Private Sub DatagridNonFundedAcctContributionSavingsPlan_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridNonFundedAcctContributionSavingsPlan.ItemDataBound
        Try
            e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfPlanType).Visible = False
            e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfAcctGroup).Visible = False
            e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfISBasicAcct).Visible = False

            If e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfAcctType).Text.ToUpper <> "&NBSP;" And e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfAcctType).Text.ToUpper <> "ACCOUNTTYPE" Then
                Dim l_decimal_try As Decimal
                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfTaxable).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfTaxable).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfNonTaxable).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfNonTaxable).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfInterest).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfInterest).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfEmpTotal).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfEmpTotal).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfYmcaTaxable).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfYmcaTaxable).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfYmcaInterest).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfYmcaInterest).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfYmcaTotal).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfYmcaTotal).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfTotal).Text)
                e.Item.Cells(mConst_DataGridNonFundedAcctContributionsIndexOfTotal).Text = FormatCurrency(l_decimal_try)
            End If


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'Plan Split Changes
    Public Sub GetPlanType()
        Try
            Dim l_string_PlanType As String = ""
            l_string_PlanType = YMCARET.YmcaBusinessObject.RefundRequest.GetPlanType(Me.SessionRefundRequestID)
            If Not l_string_PlanType = "" Then
                Me.PlanTypeChosen = l_string_PlanType.Trim.ToUpper()
            Else
                Me.PlanTypeChosen = ""
            End If
            ' we will set Plan Type for Refunds.vb class
            'Modified the plantype to make it both -Amit
            If Me.PlanTypeChosen = "RETIREMENT&SAVINGS" Then
                Me.PlanTypeChosen = "BOTH"
            End If
            'Modified the plantype to make it both -Amit
            objRefundProcess.PlanTypeChosen = Me.PlanTypeChosen
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Function CheckForUnfundedTransaction(ByVal parameterRefundType As String, ByVal parameterPlanType As String)
        Try
            Dim l_flag_IsUnfunded As Boolean = True
            Dim l_datatable_RetirementPlan_Unfunded As DataTable
            Dim l_datatable_SavingsPlan_Unfunded As DataTable
            Dim l_decimal_total_SavingsPlan_Unfunded As Decimal
            Dim l_decimal_total_RetirementPlan_Unfunded As Decimal
            Dim l_Account_Group As String = ""

            If Not Session("RetirementPlan_UnfundedContributions_C19") Is Nothing Then
                l_datatable_RetirementPlan_Unfunded = DirectCast(Session("RetirementPlan_UnfundedContributions_C19"), DataTable)
            End If
            If Not Session("SavingsPlan_UnfundedContributions_C19") Is Nothing Then
                l_datatable_SavingsPlan_Unfunded = DirectCast(Session("SavingsPlan_UnfundedContributions_C19"), DataTable)
            End If
            If parameterPlanType = "SAVINGS&RETIREMENT" Then
                parameterPlanType = "RETIREMENT&SAVINGS"
            End If
            Select Case (parameterRefundType)
                Case "REG", "SPEC", "DISAB", "PERS"
                    l_decimal_total_SavingsPlan_Unfunded = 0.0
                    l_decimal_total_RetirementPlan_Unfunded = 0.0
                    If l_datatable_RetirementPlan_Unfunded.Rows.Count > 0 Then
                        For Each drow_RetirementPlan_Unfunded As DataRow In l_datatable_RetirementPlan_Unfunded.Rows
                            If drow_RetirementPlan_Unfunded("AccountType").ToString() <> "Total" Then
                                If drow_RetirementPlan_Unfunded("Total").GetType.ToString() <> "System.DBNull" Then
                                    l_decimal_total_RetirementPlan_Unfunded += drow_RetirementPlan_Unfunded("Total")
                                End If
                            End If

                        Next
                    End If
                    If l_datatable_SavingsPlan_Unfunded.Rows.Count > 0 Then
                        For Each drow_SavingsPlan_Unfunded As DataRow In l_datatable_SavingsPlan_Unfunded.Rows
                            If drow_SavingsPlan_Unfunded("AccountType").ToString() <> "Total" Then
                                If drow_SavingsPlan_Unfunded("Total").GetType.ToString() <> "System.DBNull" Then
                                    l_decimal_total_SavingsPlan_Unfunded += drow_SavingsPlan_Unfunded("Total")
                                End If
                            End If

                        Next
                    End If
                    If parameterPlanType = "RETIREMENT" Then
                        If l_decimal_total_RetirementPlan_Unfunded > 0 Then
                            l_flag_IsUnfunded = False
                        End If
                    ElseIf parameterPlanType = "SAVINGS" Then
                        If l_decimal_total_SavingsPlan_Unfunded > 0 Then
                            l_flag_IsUnfunded = False
                        End If
                        'Neeraj singh : MRD YRS 5.0-1132
                    ElseIf parameterPlanType = "RETIREMENT&SAVINGS" Or parameterPlanType = "BOTH" Then

                        If l_decimal_total_RetirementPlan_Unfunded + l_decimal_total_SavingsPlan_Unfunded > 0 Then
                            l_flag_IsUnfunded = False
                        End If
                    End If
                Case "VOL"
                    'l_decimal_total_SavingsPlan_Unfunded = 0.0
                    'l_decimal_total_RetirementPlan_Unfunded = 0.0

                    'If l_datatable_RetirementPlan_Unfunded.Rows.Count > 0 Then
                    '    For Each drow_RetirementPlan_Unfunded As DataRow In l_datatable_RetirementPlan_Unfunded.Rows
                    '        If drow_RetirementPlan_Unfunded("AccountGroup").ToString() <> "System.DBNull" Then
                    '            l_Account_Group = drow_RetirementPlan_Unfunded("AccountGroup").ToString()
                    '            Select Case (l_Account_Group)
                    '                Case m_const_RetirementPlan_AM, m_const_RetirementPlan_AP, m_const_RetirementPlan_RP
                    '                    l_decimal_total_RetirementPlan_Unfunded += drow_RetirementPlan_Unfunded("Total")
                    '            End Select
                    '        End If
                    '    Next
                    'End If
                    'If l_datatable_SavingsPlan_Unfunded.Rows.Count > 0 Then
                    '    For Each drow_SavingsPlan_Unfunded As DataRow In l_datatable_SavingsPlan_Unfunded.Rows
                    '        If drow_SavingsPlan_Unfunded("AccountGroup").ToString() <> "System.DBNull" Then
                    '            l_Account_Group = drow_SavingsPlan_Unfunded("AccountGroup").ToString()
                    '            Select Case (l_Account_Group)
                    '                Case m_const_SavingsPlan_TD, m_const_SavingsPlan_TM, m_const_SavingsPlan_RT
                    '                    l_decimal_total_SavingsPlan_Unfunded += drow_SavingsPlan_Unfunded("Total")
                    '            End Select
                    '        End If
                    '    Next
                    'End If

                    'If parameterPlanType = "RETIREMENT" Then
                    '    If l_decimal_total_RetirementPlan_Unfunded < 0 Then
                    '        l_flag_IsUnfunded = False
                    '    End If
                    'ElseIf parameterPlanType = "SAVINGS" Then
                    '    If l_decimal_total_SavingsPlan_Unfunded < 0 Then
                    '        l_flag_IsUnfunded = False
                    '    End If
                    'ElseIf parameterPlanType = "RETIREMENT&SAVINGS" Then
                    '    If l_decimal_total_RetirementPlan_Unfunded + l_decimal_total_SavingsPlan_Unfunded > 0 Then
                    '        l_flag_IsUnfunded = False
                    '    End If
                    'End If

                Case "HARD"
                    l_decimal_total_SavingsPlan_Unfunded = 0.0
                    If l_datatable_SavingsPlan_Unfunded.Rows.Count > 0 Then
                        For Each drow_SavingsPlan_Unfunded As DataRow In l_datatable_SavingsPlan_Unfunded.Rows
                            If drow_SavingsPlan_Unfunded("AccountType").ToString() = "TD" Then
                                If drow_SavingsPlan_Unfunded("Total").GetType.ToString() <> "System.DBNull" Then
                                    l_decimal_total_SavingsPlan_Unfunded += drow_SavingsPlan_Unfunded("Total")
                                End If
                            End If
                        Next
                    End If
                    If parameterPlanType = "SAVINGS" Or parameterPlanType = "RETIREMENT&SAVINGS" Then
                        'we will not allow unfunded transactions for negative unfunded transaction(OPPR) Issue YREN-3095
                        If l_decimal_total_SavingsPlan_Unfunded < 0 Then
                            l_flag_IsUnfunded = False
                        End If
                    End If
            End Select
            Return l_flag_IsUnfunded
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CheckNegativeamounts() As String
        Dim l_datatable_retirement As DataTable
        Dim l_datatable_savings As DataTable
        Dim l_string_negative_Taxable_accounts As String
        Dim l_string_negative_NonTaxable_accounts As String
        Dim l_string_negative_YMCATaxable_accounts As String
        Dim l_string_negative_Interest_accounts As String
        Dim l_string_negative_YMCAInterest_accounts As String
        Dim l_string_message As String
        Try
            'Start Anudeep:24.01.2013 :Bt-650:YRS 5.0-521 : Program ignoring negative amount
            'getting acount balances from existing session variables
            If (Not Session("RetirementPlanAcctContribution_C19") Is Nothing) And (Not Session("SavingsPlanAcctContribution_C19") Is Nothing) Then
                l_datatable_retirement = Session("RetirementPlanAcctContribution_C19")
                l_datatable_savings = Session("SavingsPlanAcctContribution_C19")
                l_string_negative_Taxable_accounts = String.Empty
                l_string_negative_NonTaxable_accounts = String.Empty
                l_string_negative_YMCATaxable_accounts = String.Empty
                l_string_negative_Interest_accounts = String.Empty
                l_string_negative_YMCAInterest_accounts = String.Empty
                l_string_message = String.Empty
                'checking for each account if taxable or non-taxable or ymcataxable is negative then collect those accounts for retirement plan
                For Each dr As DataRow In l_datatable_retirement.Rows
                    If IsDBNull(dr("AccountType")) Then
                        Continue For
                    End If

                    If dr("AccountType") <> "Total" Then
                        If (dr("Taxable") < 0) Then
                            If l_string_negative_Taxable_accounts Is String.Empty Then
                                l_string_negative_Taxable_accounts = dr("AccountType")
                            Else
                                l_string_negative_Taxable_accounts = l_string_negative_Taxable_accounts + ", " + dr("AccountType")
                            End If
                        End If
                        If (dr("Non-Taxable") < 0) Then
                            If l_string_negative_NonTaxable_accounts Is String.Empty Then
                                l_string_negative_NonTaxable_accounts = dr("AccountType")
                            Else
                                l_string_negative_NonTaxable_accounts = l_string_negative_NonTaxable_accounts + ", " + dr("AccountType")
                            End If
                        End If
                        If (dr("YMCATaxable") < 0) Then
                            If l_string_negative_YMCATaxable_accounts Is String.Empty Then
                                l_string_negative_YMCATaxable_accounts = dr("AccountType")
                            Else
                                l_string_negative_YMCATaxable_accounts = l_string_negative_YMCATaxable_accounts + ", " + dr("AccountType")
                            End If
                        End If
                        If (dr("Interest") < 0) Then
                            If l_string_negative_Interest_accounts Is String.Empty Then
                                l_string_negative_Interest_accounts = dr("AccountType")
                            Else
                                l_string_negative_Interest_accounts = l_string_negative_Interest_accounts + ", " + dr("AccountType")
                            End If
                        End If
                        If (dr("YMCAInterest") < 0) Then
                            If l_string_negative_YMCAInterest_accounts Is String.Empty Then
                                l_string_negative_YMCAInterest_accounts = dr("AccountType")
                            Else
                                l_string_negative_YMCAInterest_accounts = l_string_negative_YMCAInterest_accounts + ", " + dr("AccountType")
                            End If
                        End If
                    End If
                Next
                'checking for each account if taxable or non-taxable or ymcataxable is negative then collect those accounts for savings plan
                For Each dr As DataRow In l_datatable_savings.Rows
                    If IsDBNull(dr("AccountType")) Then
                        Continue For
                    End If
                    If dr("AccountType") <> "Total" Then
                        If (dr("Taxable") < 0) Then
                            If l_string_negative_Taxable_accounts Is String.Empty Then
                                l_string_negative_Taxable_accounts = dr("AccountType")
                            Else
                                l_string_negative_Taxable_accounts = l_string_negative_Taxable_accounts + ", " + dr("AccountType")
                            End If
                        End If
                        If (dr("Non-Taxable") < 0) Then
                            If l_string_negative_NonTaxable_accounts Is String.Empty Then
                                l_string_negative_NonTaxable_accounts = dr("AccountType")
                            Else
                                l_string_negative_NonTaxable_accounts = l_string_negative_NonTaxable_accounts + ", " + dr("AccountType")
                            End If
                        End If
                        If (dr("YMCATaxable") < 0) Then
                            If l_string_negative_YMCATaxable_accounts Is String.Empty Then
                                l_string_negative_YMCATaxable_accounts = dr("AccountType")
                            Else
                                l_string_negative_YMCATaxable_accounts = l_string_negative_YMCATaxable_accounts + ", " + dr("AccountType")
                            End If
                        End If
                        If (dr("Interest") < 0) Then
                            If l_string_negative_Interest_accounts Is String.Empty Then
                                l_string_negative_Interest_accounts = dr("AccountType")
                            Else
                                l_string_negative_Interest_accounts = l_string_negative_Interest_accounts + ", " + dr("AccountType")
                            End If
                        End If
                        If (dr("YMCAInterest") < 0) Then
                            If l_string_negative_YMCAInterest_accounts Is String.Empty Then
                                l_string_negative_YMCAInterest_accounts = dr("AccountType")
                            Else
                                l_string_negative_YMCAInterest_accounts = l_string_negative_YMCAInterest_accounts + ", " + dr("AccountType")
                            End If
                        End If
                    End If
                Next
                'checking if any negative taxable accounts is exists then those accounts are shown in message
                If Not l_string_negative_Taxable_accounts Is String.Empty Then
                    If l_string_negative_Taxable_accounts.Contains(",") Then
                        l_string_message = "* Taxable(Personal Pre Tax) for accounts " + l_string_negative_Taxable_accounts + "."
                    Else
                        l_string_message = "* Taxable(Personal Pre Tax) for account " + l_string_negative_Taxable_accounts + "."
                    End If
                End If
                'checking if any negative Non-taxable accounts is exists then those accounts are shown in message
                If Not l_string_negative_NonTaxable_accounts Is String.Empty Then
                    If Not l_string_message Is String.Empty Then
                        l_string_message = l_string_message + "<br/>"
                    End If
                    If l_string_negative_NonTaxable_accounts.Contains(",") Then
                        l_string_message = l_string_message + "* Non-Taxable(Personal Post Tax) for accounts " + l_string_negative_NonTaxable_accounts + "."
                    Else
                        l_string_message = l_string_message + "* Non-Taxable(Personal Post Tax) for account " + l_string_negative_NonTaxable_accounts + "."
                    End If
                End If
                'checking if any negative YMCAtaxable accounts is exists then those accounts are shown in message
                If Not l_string_negative_YMCATaxable_accounts Is String.Empty Then
                    If Not l_string_message Is String.Empty Then
                        l_string_message = l_string_message + "<br/>"
                    End If
                    If l_string_negative_YMCATaxable_accounts.Contains(",") Then
                        l_string_message = l_string_message + "* YMCA Taxable(YMCA Pre Tax) for accounts " + l_string_negative_YMCATaxable_accounts + "."
                    Else
                        l_string_message = l_string_message + "* YMCA Taxable(YMCA Pre Tax) for account " + l_string_negative_YMCATaxable_accounts + "."
                    End If
                End If

                If Not l_string_negative_Interest_accounts Is String.Empty Then
                    If Not l_string_message Is String.Empty Then
                        l_string_message = l_string_message + "<br/>"
                    End If
                    If l_string_negative_Interest_accounts.Contains(",") Then
                        l_string_message = l_string_message + "* Interest for accounts " + l_string_negative_Interest_accounts + "."
                    Else
                        l_string_message = l_string_message + "* Interest for account " + l_string_negative_Interest_accounts + "."
                    End If
                End If


                If Not l_string_negative_YMCAInterest_accounts Is String.Empty Then
                    If Not l_string_message Is String.Empty Then
                        l_string_message = l_string_message + "<br/>"
                    End If
                    If l_string_negative_YMCAInterest_accounts.Contains(",") Then
                        l_string_message = l_string_message + "* YMCA Interest for accounts " + l_string_negative_YMCAInterest_accounts + "."
                    Else
                        l_string_message = l_string_message + "* YMCA Interest for account " + l_string_negative_YMCAInterest_accounts + "."
                    End If
                End If

                If Not l_string_message Is String.Empty Then

                    If l_string_message.Contains("<br/>") Then
                        l_string_message = "Following amounts have negative balance." + "<br/>" + l_string_message
                    Else
                        l_string_message = "Following amount has negative balance." + "<br/>" + l_string_message
                    End If
                End If

                Return l_string_message

            End If
            'End Anudeep:24.01.2013 :Bt-650:YRS 5.0-521 : Program ignoring negative amount
        Catch
            Throw
        End Try
    End Function

    'Plan Split Changes


    '****************************GetMessage()****************************************'
    ' 1. Get message from rsource file based on resource Key.
    '********************************************************************************'
    Public Function GetMessage(ByVal ResorceKey As String)
        Try
            Dim strMessage As String
            Try
                strMessage = GetGlobalResourceObject("Withdrawal", ResorceKey).ToString()
            Catch ex As Exception
                strMessage = ResorceKey
            End Try
            Return strMessage
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'START: Shilpa N | 02/28/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.
    Public Sub CheckReadOnlyMode()
        If (SecurityCheck.IsApplicationInReadOnlyMode()) Then
            ButtonAddNote.Enabled = False
            ButtonAddNote.ToolTip = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.TOOLTIP_READONLY).DisplayText
        End If
    End Sub
    'END: Shilpa N | 02/28/2019 | YRS-AT-4248 | Check ReadOnly Flag, if true then disable the buttons on which security is not implemented and add Tool tip.

    'AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Clears the sessions
    Public Sub ClearSession()
        Session("RetirementPlanAcctContribution_C19") = Nothing
        Session("SavingsPlanAcctContribution_C19") = Nothing
        Session("RetirementPlan_UnfundedContributions_C19") = Nothing
        Session("SavingsPlan_UnfundedContributions_C19") = Nothing
        Session("DisplayRetirementPlanAcctContribution_C19") = Nothing
        Session("DisplaySavingsPlanAcctContribution_C19") = Nothing
        Session("RefundState_C19") = Nothing

        Session("MinimumDistributionTable_C19") = Nothing
        Session("RetirementPlanAcctContribution_C19") = Nothing
        Session("SavingsPlanAcctContribution_C19") = Nothing
        Session("RetirementPlan_UnfundedContributions_C19") = Nothing
        Session("SavingsPlan_UnfundedContributions_C19") = Nothing

        Session("DisplayRetirementPlanAcctContribution_C19") = Nothing
        Session("DisplaySavingsPlanAcctContribution_C19") = Nothing

        'START: SG: BT-1012(Re-Opened): 2012.06.01
        Session("MaritalStatusCode_C19") = Nothing
        Session("Member Employment_C19") = Nothing
        Session("TermDateDataTabel_C19") = Nothing
        Session("QDROParticipantAge_C19") = Nothing
        Session("PersonInformation_C19") = Nothing
        Session("AccountContribution_NonFund_C19") = Nothing
        Session("AccountContribution_C19") = Nothing
        Session("AccountContribution_NonFunded_Total_C19") = Nothing
        Session("l_dataTable_AccountContribution_NonFunded_C19") = Nothing
        Session("AcctContributionInitial_C19") = Nothing

        Session("RefundRequestTable_C19") = Nothing
        Session("RefundRequestDataTable_C19") = Nothing

        Session("MemberNotes") = Nothing
        Session("CurrentPIA_C19") = Nothing
        Session("TerminationPIA_C19") = Nothing
        Session("BACurrent_C19") = Nothing
        Session("BATermination_C19") = Nothing

        'START: SB | 2017.08.02 | YRS-AT-3324 | Clearing session variables
        Session("ReasonForRestriction_C19") = Nothing
        Session("WithdrawalAllowedForRMDEligibleParticipants_C19") = Nothing
        'END: SB | 2017.08.02 | YRS-AT-3324 | Clearing session variables

    End Sub
    'AA:03.02.2014 :BT :2292 :YRS 5.0-2248 - Load the refund data and fill in all tabs
    Public Sub LoadAllTabs()
        Me.LoadGeneralInformation(Me.SessionPersonID, Me.SessionFundID)

        If Me.SessionFundID = String.Empty Then
            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "There is no Employment Present for this Member." & vbCrLf & vbCrLf & "Details of Selected Member could not be loaded.", MessageBoxButtons.Stop, False)
            Me.RefundTabStrip.Items.Item(0).Enabled = False
            Me.RefundTabStrip.Items.Item(1).Enabled = False
            Me.RefundTabStrip.Items.Item(2).Enabled = False
            Me.RefundTabStrip.Items.Item(3).Enabled = False
            Me.RefundTabStrip.Items.Item(4).Enabled = False

        Else
            Me.LoadAccountContribution(Me.SessionFundID)
            Me.LoadRefundRequestDetails(Me.SessionPersonID, Me.SessionFundID)
            Me.LoadMemberNotes(Me.SessionPersonID)
            Me.LoadCurrentPIA(Me.SessionFundID)
            '' Enable Notes PopupWindow
            Me.SessionIsNotesPopupAllowed = True
            '' Enable TabPages in the Page.
            Me.RefundTabStrip.Items.Item(0).Enabled = True
            Me.RefundTabStrip.Items.Item(1).Enabled = True
            Me.RefundTabStrip.Items.Item(2).Enabled = True
            Me.RefundTabStrip.Items.Item(3).Enabled = True
            Me.RefundTabStrip.Items.Item(4).Enabled = True
        End If
    End Sub
    '--Start: Manthan Rajguru | 2015.10.06 | YRS-AT 2595 | Method to store deceased status and validating it with current fund status of selected person
    Function IsDeceased(ByVal currentStatus As String) As Boolean
        Dim arrfundStatusType() As String = {"DA", "DI", "DR", "DD", "DQ"}
        Dim bResult As Boolean = False

        For Each fundStatus As String In arrfundStatusType
            If currentStatus.Trim = fundStatus Then
                bResult = True
                Exit For
            End If
        Next
        Return bResult
    End Function
    '--End: Manthan Rajguru | 2015.10.06 | YRS-AT 2595 | Method to store deceased status and validating it with current fund status of selected person

    'START |PK | 08/30/2019 | YRS-AT-2670 |To Check participant from Puerto Rico Ymca and to give a warning message.
    Public Function IsYmcaPuertoRico() As Boolean
        Dim IsRicoYmca As Boolean
        Try
            IsRicoYmca = YMCARET.YmcaBusinessObject.RefundRequest.IsYmcaPuertoRico(Me.SessionFundID)
            If (IsRicoYmca = True) Then 'if isRicoYmca is true then show warning message.
                Return True
            End If
        Catch
            Throw
        Finally
            IsRicoYmca = Nothing
        End Try
    End Function
    ''' <summary>
    ''' On click of modal popup OK button a RefundProcessing popup will open.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnConfirmDialogOK_Click(sender As Object, e As EventArgs) Handles btnConfirmDialogOK.Click
        Try
            If (Me.IsYmcaPuertoRico() = True) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "scriptKey", "closeDialog('ConfirmDialog');", True)
                ClientScript.RegisterStartupScript(Me.GetType(), "Process", "window.open('RefundProcessing_C19.aspx', 'YMCAYRS', 'width=1000, height=750, menubar=no, status=Yes,Resizable=Yes,top=0,left=0, scrollbars=yes')", True)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            HelperFunctions.LogException("btnConfirmDialogOK_Click", ex)
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'END |PK | 08/30/2019 | YRS-AT-2670 |To Check participant from Puerto Rico Ymca and to give a warning message.

    'START: PK | 05/05/2020 | YRS-AT-4854 | Validation to check requested refund withdrawal is covid or not.
    Public Function GetSelectedRefRequestID(ByVal parameterSelectedIndex As Integer) As String
        Dim refRequestID As String
        Dim l_DataTable_RefundRequest As DataTable
        Dim l_DataRow As DataRow
        Try
            l_DataTable_RefundRequest = DirectCast(Session("RefundRequestDataTable_C19"), DataTable)

            If Not l_DataTable_RefundRequest Is Nothing Then
                If l_DataTable_RefundRequest.Rows.Count >= parameterSelectedIndex Then
                    l_DataRow = l_DataTable_RefundRequest.Rows.Item(parameterSelectedIndex)
                    If Not l_DataRow Is Nothing Then
                        refRequestID = CType(l_DataRow("UniqueID"), String)
                        Return refRequestID
                    End If
                Else
                    Return String.Empty
                End If
            Else
                Return String.Empty
            End If
        Catch
            Throw
        Finally
            l_DataTable_RefundRequest = Nothing
            l_DataRow = Nothing
        End Try
    End Function

    Public Function IsRequestCovid(ByVal refRequestID As String) As Boolean
        Dim IsCovid As Boolean
        Try
            IsCovid = YMCARET.YmcaBusinessObject.RefundRequest.IsRequestCovid(refRequestID)
            Return IsCovid
        Catch
            Throw
        Finally
            IsCovid = Nothing
        End Try
    End Function
    'END: PK | 05/05/2020 | YRS-AT-4854 | Validation to check requested refund withdrawal is covid or not.
End Class
