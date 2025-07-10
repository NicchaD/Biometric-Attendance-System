'/**************************************************************************************************************/
'//' Copyright YMCA Retirement Fund All Rights Reserved. 
'//
'// Author: Manthan Rajguru
'// Created on: 04/09/2020
'// Summary of Functionality: New Screen for COVID19 withdrawal request
'// Declared in Version: 20.8.1.2| YRS-AT-4854 -  COVID - Special withdrawal functionality needed due to CARE Act/COVID-19 (Track IT - 41688) 
'//
'/**************************************************************************************************************/
'// REVISION HISTORY:
'// ------------------------------------------------------------------------------------------------------
'// Developer Name              | Date         | Version No      | Ticket
'// ------------------------------------------------------------------------------------------------------
'// Manthan Rajguru			    | 05/13/2020   | 20.8.1.2		 | YRS-AT-4854 -  COVID - Special withdrawal functionality needed due to CARE Act/COVID-19 (Track IT - 41688)
'// ------------------------------------------------------------------------------------------------------
' /**************************************************************************************************************/

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web
Imports System.IO
Imports System.Collections
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Web.Services
Imports YMCARET


Public Class RefundRequestWebForm_C19
    Inherits System.Web.UI.Page


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents TextboxVested As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTerminated As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxAge As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridAccountContribution As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ListBox1 As System.Web.UI.WebControls.ListBox
    Protected WithEvents CheckboxSpecial As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TextboxTerminatePIA As System.Web.UI.WebControls.TextBox
    'BA Account YMCA PhaseV 03-04-2009 by Dilip
    Protected WithEvents TextboxBATerminate As System.Web.UI.WebControls.TextBox
    'Protected WithEvents datagridCheckBox_Retirement As CustomControls.CheckBoxColumn '--Manthan Rajguru 2015-09-24 YRS-AT-2550: Commented as control not used anywhere in the page

    'BA Account YMCA PhaseV 03-04-2009 by Dilip
    Protected WithEvents TextboxNonTaxed As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTaxed As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTax As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNet As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents CheckboxRegular As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxVoluntryAccounts As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TextboxTaxRate As System.Web.UI.WebControls.TextBox
    '--STart Manthan Rajguru 2015.09.24 YRS-AT-2550: Custom Control reference given for DatagridcheckBox
    Protected WithEvents datagridCheckBox As CustomControls.CheckBoxColumn
    Protected WithEvents datagridCheckBox_Savings As CustomControls.CheckBoxColumn
    '--End Manthan Rajguru 2015.09.24 YRS-AT-2550: Custom Control reference given for DatagridcheckBox
    Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label
    Protected WithEvents CheckboxDisability As System.Web.UI.WebControls.CheckBox
    'Plan Split Changes May 22nd,2007
    Protected WithEvents CheckboxRetirementPlan As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxSavingsPlan As System.Web.UI.WebControls.CheckBox
    Protected WithEvents DataGridAccContributionSavings As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridAccContributionRetirement As System.Web.UI.WebControls.DataGrid

    'Plan Split Changes May22nd,2007    
    'Protected WithEvents TextboxRequestAmtforHardship As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelMessage As System.Web.UI.WebControls.Label
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.

    Protected WithEvents CheckboxHardship As System.Web.UI.WebControls.CheckBox
    'START Chandra sekar | 2016.09.23 | YRS-AT-3164 - Support Request:special Hardship Withdrawal - how to allow contributions to continue temporarily
    Protected WithEvents CheckboxIRSOverride As System.Web.UI.WebControls.CheckBox
    Protected WithEvents DivIRSOverrideRules As System.Web.UI.HtmlControls.HtmlGenericControl
    'END Chandra sekar | 2016.09.23 | YRS-AT-3164 - Support Request:special Hardship Withdrawal - how to allow contributions to continue temporarily
    Dim dtFileList As New DataTable
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents LabelParticipantName As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTermination As System.Web.UI.WebControls.Label
    'BA Account YMCA PhaseV 03-04-2009 by Dilip
    Protected WithEvents LabelBATermination As System.Web.UI.WebControls.Label
    'BA Account YMCA PhaseV 03-04-2009 by Dilip
    Protected WithEvents LabelFundNo As System.Web.UI.WebControls.Label
    Protected WithEvents DatagridConsolidateTotal As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DatagridGrandTotal As System.Web.UI.WebControls.DataGrid
    Protected WithEvents CheckboxExcludeYMCA As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxPartialRetirement As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelNonTaxableSavings As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxableSavings As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxWithheldSavings As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNetAmountSavings As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPartialRetirement As System.Web.UI.WebControls.TextBox
    Protected WithEvents CheckboxPartialSavings As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TextboxPartialSavings As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxRetirementNonTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxRetirementTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxRetirementTaxWithheld As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxRetirementNetAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelNonTaxableRetirement As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxSavingsNonTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelTaxableRetirement As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxSavingsTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelTaxWithheldRetirement As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxSavingsTaxWithheld As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelNetAmountRetirement As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxSavingsNetAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents CheckboxSavingsVoluntary As System.Web.UI.WebControls.CheckBox
    Dim objRefundRequest As New Refunds_C19
    'New Modification for Market Based Withdrawal Amit Nigam
    Protected WithEvents LabelFirstInstallment As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxFirstInstallment As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPercentage As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPercentage As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelFirstInstallmentSav As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxFirstInstallmentSav As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPercentageSav As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPercentageSav As System.Web.UI.WebControls.TextBox
    Protected WithEvents CheckBoxMarket As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelMarketRetirement As System.Web.UI.WebControls.Label
    Protected WithEvents LabelMarketSavings As System.Web.UI.WebControls.Label
    Protected WithEvents LabelMarket As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNonTaxableMarket As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxableMarket As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxWithHeldMarket As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNetAmount As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxNonTaxableMarket As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTaxWithHeldMarket As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNetAmountMarket As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTaxableMarket As System.Web.UI.WebControls.TextBox
    'MRD PHASE -II

    Protected WithEvents CheckboxMRDSavings As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxMRDRetirement As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxMRDSavingsCurrentYear As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxMRDRetirementCurrentYear As System.Web.UI.WebControls.CheckBox

    Protected WithEvents DivMainMessage As System.Web.UI.HtmlControls.HtmlGenericControl 'SR | 2016.05.24 | YRS-AT-2962 | create div control to display informative message.

    'START | SR | YRS-AT-4055 | define controls to diaplay aggregate of current PIA & BA from service data.
    Protected WithEvents lblAggregateBALegacyatRequest As System.Web.UI.WebControls.Label
    Protected WithEvents txtAggregateBALegacyatRequest As System.Web.UI.WebControls.TextBox
    'END | SR | YRS-AT-4055 | define controls to diaplay aggregate of current PIA & BA from service data.

    'START - MMR | 04/20/2020 |YRS-AT-4854 | Declared controls for covid
    Protected WithEvents lblCovidAmountUsed As System.Web.UI.WebControls.Label
    Protected WithEvents lblCovidAmountAvailable As System.Web.UI.WebControls.Label
    Protected WithEvents txtCovidAmountUsed As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCovidAmountAvailable As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblCovidTaxableAmount As System.Web.UI.WebControls.Label
    Protected WithEvents lblDisplayCovidTaxableAmount As System.Web.UI.WebControls.Label
    Protected WithEvents lblCovidNonTaxableAmount As System.Web.UI.WebControls.Label
    Protected WithEvents lblDisplayCovidNonTaxableAmount As System.Web.UI.WebControls.Label
    Protected WithEvents lblCovidTaxRate As System.Web.UI.WebControls.Label
    Protected WithEvents txtCovidTaxRate As System.Web.UI.WebControls.TextBox
    'END - MMR | 04/20/2020 |YRS-AT-4854 | Declared controls for covid

    'New Modification for Market Based Withdrawal Amit Nigam

    ' Dim IDM As New IDMforAll 'commented by hafiz on 3-May-2007
    Private designerPlaceholderDeclaration As System.Object
    Dim objRefundProcess As New Refunds_C19

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "global declarations"
    'START - Retirement plan account groups
    Const m_const_RetirementPlan_AP As String = "RAP"
    Const m_const_RetirementPlan_AM As String = "RAM"
    Const m_const_RetirementPlan_RG As String = "RRG"
    Const m_const_RetirementPlan_SA As String = "RSA"
    Const m_const_RetirementPlan_SS As String = "RSS"
    Const m_const_RetirementPlan_RP As String = "RRP"
    Const m_const_RetirementPlan_SR As String = "RSR"
    'Begin Code Merge by Dilip on 07-05-2009
    'Priya 13-Jan-2009 : YRS 5.0-637 AC Account interest components added new constant for AC account type group.
    Const m_const_RetirementPlan_AC As String = "RAC"
    'End Code Merge by Dilip on 07-05-2009
    Const m_const_RetirementPlan_BA As String = "RBA"
    'Phase V YMCA Legacy acct & YMCA Acct const variable by Dilip
    Const m_const_YMCA_Legacy_Acct As String = "YMCA (Legacy) Acct"
    Const m_const_YMCA__Acct As String = "YMCA Acct"
    'Phase V YMCA Legacy acct & YMCA Acct const variable by Dilip
    'END - Retirement plan account groups
    'START - Ssvings plan account groups
    Const m_const_SavingsPlan_TD As String = "STD"
    Const m_const_SavingsPlan_TM As String = "STM"
    Const m_const_SavingsPlan_RT As String = "SRT"
    'END - Savings plan account groups

    Dim blnRetirementPlan As Boolean = False
    Dim blnSavingsPlan As Boolean = False
    'New Modification for Market Based Withdrawal Amit Nigam
    Dim blnMarketCheck As Boolean = False
    'New Modification for Market Based Withdrawal Amit Nigam

    '------ Added to implement WebServiece by pavan ---- Begin  -----------
    Dim objService As New YRSWebService.YRSWithdrawalService
    Dim objWebServiceReturn As New YRSWebService.WebServiceReturn
    Protected FinalDataSet As DataSet = Nothing
    Protected OriginalDataSet As DataSet = Nothing
    Dim ArraySlectedRetirementAccounts() As String
    Dim ArraySlectedSavingsAccounts() As String
    Dim myStringRetirementElements As Integer = 0
    Dim myStringSavingsElements As Integer = 0
    '------ Added to implement WebServiece by pavan ---- END    -----------
#End Region

#Region " Form Properties "

    Private Property AllowPartialRefund() As Boolean
        Get
            If Not Session("AllowPartialRefund_C19") Is Nothing Then
                Return (CType(Session("AllowPartialRefund_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("AllowPartialRefund_C19") = Value
        End Set
    End Property

    Private Property RetirementPlanForgrid() As Boolean
        Get
            If Not Session("RetirementPlanForgrid_C19") Is Nothing Then
                Return (CType(Session("RetirementPlanForgrid_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("RetirementPlanForgrid_C19") = Value
        End Set
    End Property
    Private Property SavingPlanforGrid() As Boolean
        Get
            If Not Session("SavingPlanforGrid_C19") Is Nothing Then
                Return (CType(Session("SavingPlanforGrid_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("SavingPlanforGrid_C19") = Value
        End Set
    End Property
    'Added By Ganesh for Partial Withdraw
    Private Property CalculatedDataTableSavingPlan() As DataTable
        Get
            If Not (Session("CalculatedDataTableSavingPlan_C19")) Is Nothing Then
                Return (DirectCast(Session("CalculatedDataTableSavingPlan_C19"), DataTable))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataTable)
            Session("CalculatedDataTableSavingPlan_C19") = Value
        End Set
    End Property

    Private Property CalculatedDataTableRetirementPlan() As DataTable
        Get
            If Not (Session("CalculatedDataTableSavingPlan_C19")) Is Nothing Then
                Return (DirectCast(Session("CalculatedDataTableSavingPlan_C19"), DataTable))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataTable)
            Session("CalculatedDataTableSavingPlan_C19") = Value
        End Set
    End Property
    'Added By Ganesh for Partial Withdraw

    Private Property NumRequestedAmountRetirement() As Decimal
        Get
            If Not Session("RequestedAmountRetirement_C19") Is Nothing Then
                Return (CType(Session("RequestedAmountRetirement_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("RequestedAmountRetirement_C19") = Value
        End Set
    End Property

    Private Property NumRequestedAmountSavings() As Decimal
        Get
            If Not Session("RequestedAmountSavings_C19") Is Nothing Then
                Return (DirectCast(Session("RequestedAmountSavings_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("RequestedAmountSavings_C19") = Value
        End Set
    End Property
    Private Property NumPercentageFactorofMoneyRetirementTemp() As Decimal
        Get
            If Not Session("NumPercentageFactorofMoneyRetirementTemp_C19") Is Nothing Then
                Return (DirectCast(Session("NumPercentageFactorofMoneyRetirementTemp_C19"), Decimal))
            Else
                Return 1.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("NumPercentageFactorofMoneyRetirementTemp_C19") = Value
        End Set
    End Property
    Private Property NumPercentageFactorofMoneySavingsTemp() As Decimal
        Get
            If Not Session("NumPercentageFactorofMoneySavingsTemp_C19") Is Nothing Then
                Return (DirectCast(Session("NumPercentageFactorofMoneySavingsTemp_C19"), Decimal))
            Else
                Return 1.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("NumPercentageFactorofMoneySavingsTemp_C19") = Value
        End Set
    End Property

    Private Property NumPercentageFactorofMoneyRetirement() As Decimal
        Get
            If Not Session("PercentageFactorofMoneyRetirement_C19") Is Nothing Then
                Return (DirectCast(Session("PercentageFactorofMoneyRetirement_C19"), Decimal))
            Else
                Return 1.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("PercentageFactorofMoneyRetirement_C19") = Value
        End Set
    End Property

    Private Property NumPercentageFactorofMoneySavings() As Decimal
        Get
            If Not Session("PercentageFactorofMoneySavings_C19") Is Nothing Then
                Return (DirectCast(Session("PercentageFactorofMoneySavings_C19"), Decimal))
            Else
                Return 1.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("PercentageFactorofMoneySavings_C19") = Value
        End Set
    End Property

    'To set & get Minimum Distribution Amount
    Private Property MinimumDistributionAmount() As Decimal
        Get
            If Not Session("MinimumDistributionAmount_C19") Is Nothing Then
                Return (DirectCast(Session("MinimumDistributionAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinimumDistributionAmount_C19") = Value
        End Set
    End Property

    'To Get / Set the default Tax Rate for a Minimum
    Private Property MinimumDistributionTaxRate() As Decimal
        Get
            If Not Session("MinimumDistributionTaxRate_C19") Is Nothing Then
                Return (DirectCast(Session("MinimumDistributionTaxRate_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinimumDistributionTaxRate_C19") = Value
        End Set
    End Property

    'To Get / Set Federal Tax Rate
    Private Property FederalTaxRate() As Decimal
        Get
            If Not Session("FederalTaxRate_C19") Is Nothing Then
                Return (DirectCast(Session("FederalTaxRate_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("FederalTaxRate_C19") = Value
        End Set
    End Property
    Private Property TaxWithheldRetirement() As Decimal
        Get
            If Not Session("TaxWithheldRetirement_C19") Is Nothing Then
                Return (DirectCast(Session("TaxWithheldRetirement_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TaxWithheldRetirement_C19") = Value
        End Set
    End Property
    Private Property TaxWithheldSavings() As Decimal
        Get
            If Not Session("TaxWithheldSavings_C19") Is Nothing Then
                Return (DirectCast(Session("TaxWithheldSavings_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TaxWithheldSavings_C19") = Value
        End Set
    End Property

    '' To Get / Set the Refund Expiry Date
    Private Property RefundExpiryDate() As Integer
        Get
            If Not Session("RefundExpiryDate_C19") Is Nothing Then
                Return (DirectCast(Session("RefundExpiryDate_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("RefundExpiryDate_C19") = Value
        End Set
    End Property

    ' To Get / set the MinimumDistributedAge
    Private Property MinimumDistributedAge() As Decimal
        Get
            If Not Session("MinimumDistributedAge_C19") Is Nothing Then
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinimumDistributedAge_C19") = Value
        End Set
    End Property
    'To Get / Set Maximum Amount Limit for BA age under 55.
    Private Property BAMaxLimit() As Decimal
        Get
            If Not Session("BAMaxLimit_C19") Is Nothing Then
                Return (DirectCast(Session("BAMaxLimit_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("BAMaxLimit_C19") = Value
        End Set
    End Property
    'To Get / Set Maximum Amount Limit for BA age above 55.


    'To Get / Set Maximum PIA Amount.
    Private Property MaximumPIAAmount() As Decimal
        Get
            If Not Session("MaximumPIAAmount_C19") Is Nothing Then
                Return (DirectCast(Session("MaximumPIAAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MaximumPIAAmount_C19") = Value
        End Set
    End Property
    'Shubhrata Jan 9th 2006 - to replace hard coded value of 5000(min PIA to retire)
    Private Property MinimumPIAToRetire() As Decimal
        Get
            If Not Session("MinimumPIAToRetire_C19") Is Nothing Then
                Return (DirectCast(Session("MinimumPIAToRetire_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinimumPIAToRetire_C19") = Value
        End Set
    End Property
    'Shubhrata Jan 9th 2006
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

    Private Property NoRefundAllowed() As Boolean
        Get
            If Not Session("NoRefundAllowed_C19") Is Nothing Then
                Return (CType(Session("NoRefundAllowed_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("NoRefundAllowed_C19") = Value
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


    ' To get / set Person Age
    Private Property PersonAge() As Decimal
        Get
            If Not Session("PersonAge_C19") Is Nothing Then
                Return (DirectCast(Session("PersonAge_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("PersonAge_C19") = Value
        End Set
    End Property


    Private Property PersonAgeatEndofYear() As Decimal
        Get
            If Not Session("PersonAgeatEndofYear_C19") Is Nothing Then
                Return (DirectCast(Session("PersonAgeatEndofYear_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("PersonAgeatEndofYear_C19") = Value
        End Set
    End Property


    ' To Get / Set Termination PIA
    Private Property TerminationPIA() As Decimal
        Get
            If Not Session("TerminationPIA_C19") Is Nothing Then
                Return (DirectCast(Session("TerminationPIA_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TerminationPIA_C19") = Value
        End Set
    End Property

    'To Get / Set CurrentPIA
    Private Property CurrentPIA() As Decimal
        Get
            If Not Session("CurrentPIA_C19") Is Nothing Then
                Return (DirectCast(Session("CurrentPIA_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("CurrentPIA_C19") = Value
        End Set
    End Property

    ' BA Account YMCA PhaseV 08-04-2009 by Dilip
    ' To Get / Set BATermination 
    Private Property BATermination() As Decimal
        Get
            If Not Session("BATermination_C19") Is Nothing Then
                Return (DirectCast(Session("BATermination_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("BATermination_C19") = Value
        End Set
    End Property
    'Added By ganeswar on july-21-2009
    Private Property CurrentBAAtRequest() As Decimal
        Get
            If Not Session("CurrentBAAtRequest_C19") Is Nothing Then
                Return (DirectCast(Session("CurrentBAAtRequest_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("CurrentBAAtRequest_C19") = Value
        End Set
    End Property
    'Added By ganeswar on july-21-2009


    'To Get / Set BA Current
    Private Property BACurrent() As Decimal
        Get
            If Not Session("BACurrent_C19") Is Nothing Then
                Return (DirectCast(Session("BACurrent_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("BACurrent_C19") = Value
        End Set
    End Property
    ' BA Account YMCA PhaseV 08-04-2009 by Dilip

    'To get / set YMCA Amount
    Private Property YMCAAvailableAmount() As Decimal
        Get
            If Not Session("YMCAAvailableAmount_C19") Is Nothing Then
                Return (DirectCast(Session("YMCAAvailableAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("YMCAAvailableAmount_C19") = Value
        End Set
    End Property

    ' To get / set VoluntryAmount
    Private Property VoluntryAmount() As Decimal
        Get
            If Not Session("VoluntryAmount_C19") Is Nothing Then
                Return (DirectCast(Session("VoluntryAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("VoluntryAmount_C19") = Value
        End Set
    End Property

    ' To get / set TD Account Amount
    Private Property TDAccountAmount() As Decimal
        Get
            If Not Session("TDAccountAmount_C19") Is Nothing Then
                Return (DirectCast(Session("TDAccountAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TDAccountAmount_C19") = Value
        End Set
    End Property


    'To get / set the PersonID property.    
    Private Property PersonID() As String
        Get
            If Not Session("PersonID") Is Nothing Then
                Return (DirectCast(Session("PersonID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PersonID") = Value
        End Set
    End Property

    ' To get / set FundID
    Private Property FundID() As String
        Get
            If Not Session("FundID") Is Nothing Then
                Return (DirectCast(Session("FundID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("FundID") = Value
        End Set
    End Property

    ' To get / set RefundType
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

    'To Keep the flag to Raise the Popup window
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


    'To Get / Set Total Amount
    Private Property TotalAmount() As Decimal
        Get
            If Not Session("TotalAmount_C19") Is Nothing Then
                Return (CType(Session("TotalAmount_C19"), String))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("TotalAmount_C19") = Value
        End Set
    End Property

    'To Get / Set Employee Total Amount
    Private Property PersonTotalAmount() As Decimal
        Get
            If Not Session("PersonTotalAmount_C19") Is Nothing Then
                Return (CType(Session("PersonTotalAmount_C19"), String))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("PersonTotalAmount_C19") = Value
        End Set
    End Property


    'To Get / Set Type Choosen.
    Private Property IsTypeChoosen() As Boolean
        Get
            If Not Session("IsTypeChoosen_C19") Is Nothing Then
                Return (CType(Session("IsTypeChoosen_C19"), Boolean))
            Else
                Return False
            End If

        End Get
        Set(ByVal Value As Boolean)
            Session("IsTypeChoosen_C19") = Value
        End Set
    End Property
    ''added by ruchi to keep track of the amount available
    Private Property CanRequest() As Boolean
        Get
            If Not Session("CanRequest_C19") Is Nothing Then
                Return (CType(Session("CanRequest_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("CanRequest_C19") = Value
        End Set
    End Property
    'added by Shubhrata YREN-3039,Feb13th 2007
    'To Keep the StatusType for Selected Member
    Private Property SessionStatusType() As String
        Get
            If Not (Session("StatusType_C19")) Is Nothing Then
                Return (CType(Session("StatusType_C19"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("StatusType_C19") = Value
        End Set
    End Property
    'Shubhrata Plan Split Changes
    Private Property PlanChosen() As String
        Get
            If Not Session("PlanChosen_C19") Is Nothing Then
                Return (DirectCast(Session("PlanChosen_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PlanChosen_C19") = Value
        End Set
    End Property
    Private Property RetirementPlan_TotalAmount() As Decimal
        Get
            If Not Session("RetirementPlan_TotalAmount_C19") Is Nothing Then
                Return (DirectCast(Session("RetirementPlan_TotalAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("RetirementPlan_TotalAmount_C19") = Value
        End Set
    End Property
    Private Property RetirementPlan_TotalAvailableAmount() As Decimal
        Get
            If Not Session("RetirementPlan_TotalAvailableAmount_C19") Is Nothing Then
                Return (DirectCast(Session("RetirementPlan_TotalAvailableAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("RetirementPlan_TotalAvailableAmount_C19") = Value
        End Set
    End Property
    Private Property SavingsPlan_TotalAvailableAmount() As Decimal
        Get
            If Not Session("SavingsPlan_TotalAvailableAmount_C19") Is Nothing Then
                Return (DirectCast(Session("SavingsPlan_TotalAvailableAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("SavingsPlan_TotalAvailableAmount_C19") = Value
        End Set
    End Property

    'To Get / Set Employee Total Amount
    Private Property RetirementPlan_PersonTotalAmount() As Decimal
        Get
            If Not Session("RetirementPlan_PersonTotalAmount_C19") Is Nothing Then
                Return (DirectCast(Session("RetirementPlan_PersonTotalAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("RetirementPlan_PersonTotalAmount_C19") = Value
        End Set
    End Property
    Private Property RetirementPlan_PersonTotalAmountInitial() As Decimal
        Get
            If Not Session("RetirementPlan_PersonTotalAmountInitial_C19") Is Nothing Then
                Return (DirectCast(Session("RetirementPlan_PersonTotalAmountInitial_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("RetirementPlan_PersonTotalAmountInitial_C19") = Value
        End Set
    End Property
    Private Property SavingsPlan_TotalAmount() As Decimal
        Get
            If Not Session("SavingsPlan_TotalAmount_C19") Is Nothing Then
                Return (DirectCast(Session("SavingsPlan_TotalAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("SavingsPlan_TotalAmount_C19") = Value
        End Set
    End Property

    'To Get / Set Employee Total Amount
    Private Property SavingsPlan_PersonTotalAmount() As Decimal
        Get
            If Not Session("SavingsPlan_PersonTotalAmount_C19") Is Nothing Then
                Return (DirectCast(Session("SavingsPlan_PersonTotalAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("SavingsPlan_PersonTotalAmount_C19") = Value
        End Set
    End Property
    Private Property SavingsPlan_PersonTotalAmountInitial() As Decimal
        Get
            If Not Session("SavingsPlan_PersonTotalAmountInitial_C19") Is Nothing Then
                Return (DirectCast(Session("SavingsPlan_PersonTotalAmountInitial_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("SavingsPlan_PersonTotalAmountInitial_C19") = Value
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
    Private Property AllowedRetirementPlan() As Boolean
        Get
            If Not Session("AllowedRetirementPlan_C19") Is Nothing Then
                Return (CType(Session("AllowedRetirementPlan_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("AllowedRetirementPlan_C19") = Value
        End Set
    End Property
    Private Property AllowedPersonalSideRefund() As Boolean
        Get
            If Not Session("AllowPersonalSideRefund_C19") Is Nothing Then
                Return (CType(Session("AllowPersonalSideRefund_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("AllowPersonalSideRefund_C19") = Value
        End Set
    End Property
    'by Aparna -new changes 15/10/2007
    Private Property FirstName() As String
        Get
            If Not Session("FirstName") Is Nothing Then
                Return (CType(Session("FirstName"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("FirstName") = Value
        End Set
    End Property
    Private Property MiddleName() As String
        Get
            If Not Session("MiddleName") Is Nothing Then
                Return (DirectCast(Session("MiddleName"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("MiddleName") = Value
        End Set
    End Property
    Private Property LastName() As String
        Get
            If Not Session("LastName") Is Nothing Then
                Return (DirectCast(Session("LastName"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("LastName") = Value
        End Set
    End Property
    Private Property HardShipAllowed() As Boolean
        Get
            If Not Session("HardShipAllowed_C19") Is Nothing Then
                Return (CType(Session("HardShipAllowed_C19"), Boolean))
            Else
                Return False
            End If

        End Get
        Set(ByVal Value As Boolean)
            Session("HardShipAllowed_C19") = Value
        End Set
    End Property

    Private Property CompulsoryRetirement() As Boolean
        Get
            If Not (Session("CompulsoryRetirement_C19")) Is Nothing Then
                Return (CType(Session("CompulsoryRetirement_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("CompulsoryRetirement_C19") = Value
        End Set
    End Property
    Private Property CompulsorySavings() As Boolean
        Get
            If Not (Session("CompulsorySavings_C19")) Is Nothing Then
                Return (CType(Session("CompulsorySavings_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("CompulsorySavings_C19") = Value
        End Set
    End Property
    'Shubhrata Plan Split Changes

    'To get/set the value of checked ymca account
    Private Property IsMessageCheck() As Boolean
        Get
            If Not Session("IsMessageCheck_C19") Is Nothing Then
                Return (CType(Session("IsMessageCheck_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsMessageCheck_C19") = Value
        End Set
    End Property
    'Added for validation the partial withdrawal requested amount -Amit 01 Sep 2009
    Private Property PartialMinLimit() As Decimal
        Get
            If Not Session("PartialMinLimit_C19") Is Nothing Then
                Return (DirectCast(Session("PartialMinLimit_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("PartialMinLimit_C19") = Value
        End Set
    End Property
    'Added for validation the partial withdrawal requested amount -Amit 01 Sep 2009

    'Added the Property for the total available amount in the retirement plan-Amit 09-09-2009
    Private Property RetirementPlanTotalAmount() As Decimal
        Get
            If Not Session("RetirementPlanTotalAmount_C19") Is Nothing Then
                Return (DirectCast(Session("RetirementPlanTotalAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("RetirementPlanTotalAmount_C19") = Value
        End Set
    End Property
    'Added the Property for the total available amount in the retirement plan-Amit 09-09-2009

    'Added the Property to check the tab out fucntion took place or not-Amit 09-09-2009
    Private Property IsPartialRetirementAmountGivenTabOutDone() As Boolean
        Get
            If Not Session("IsPartialRetirementAmountGivenTabOutDone_C19") Is Nothing Then
                Return (CType(Session("IsPartialRetirementAmountGivenTabOutDone_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsPartialRetirementAmountGivenTabOutDone_C19") = Value
        End Set
    End Property

    Private Property IsPartialSavingsAmountGivenTabOutDone() As Boolean
        Get
            If Not Session("IsPartialSavingsAmountGivenTabOutDone_C19") Is Nothing Then
                Return (CType(Session("IsPartialSavingsAmountGivenTabOutDone_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsPartialSavingsAmountGivenTabOutDone_C19") = Value
        End Set
    End Property
    'Added the Property to check the threshold amount for market based withdrawal-Amit 10-01-2009
    Private Property MarketBasedThreshold() As Decimal
        Get
            If Not Session("MarketBasedThreshold_C19") Is Nothing Then
                Return (DirectCast(Session("MarketBasedThreshold_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MarketBasedThreshold_C19") = Value
        End Set
    End Property
    'Added the Property to check the threshold amount for market based withdrawal-Amit 10-01-2009
    'Added the Property to check the tab out fucntion took place or not-Amit 09-09-2009
    'New Modification for Market Based Withdrawal Amit Nigam
    Private Property MarketBasedFirstInstPercentage() As Decimal
        Get
            If Not Session("MarketBasedFirstInstPercentage_C19") Is Nothing Then
                Return (DirectCast(Session("MarketBasedFirstInstPercentage_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MarketBasedFirstInstPercentage_C19") = Value
        End Set
    End Property
    Private Property MarketBasedDefferedPmtPercentage() As Decimal

        Get
            If Not Session("MarketBasedDefferedPmtPercentage_C19") Is Nothing Then
                Return (DirectCast(Session("MarketBasedDefferedPmtPercentage_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MarketBasedDefferedPmtPercentage_C19") = Value

        End Set
    End Property
    'Added the Property to check the threshold amount for market based withdrawal-Amit 10-01-2009
    'Added the Property used to  check  wheteher market based withdrawal is allowed or not -Amit 10-01-2009
    Private Property AllowMarketBased() As Boolean
        Get
            If Not Session("AllowMarketBased_C19") Is Nothing Then
                Return (CType(Session("AllowMarketBased_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("AllowMarketBased_C19") = Value
        End Set
    End Property
    'Added the Property used to  check  wheteher market based withdrawal is allowed or not -Amit 10-01-2009

    'Added the Property used to  check  wheteher market based withdrawal for Retirement Plan is allowed or not -Amit 10-26-2009
    Private Property AllowMarketBasedForRetirementPlan() As Boolean
        Get
            If Not Session("AllowMarketBasedForRetirementPlan_C19") Is Nothing Then
                Return (CType(Session("AllowMarketBasedForRetirementPlan_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("AllowMarketBasedForRetirementPlan_C19") = Value
        End Set
    End Property
    'Added the Property used to  check  wheteher market based withdrawal for Retirement Plan is allowed or not -Amit 10-26-2009

    'Added the Property used to  check  wheteher market based withdrawal for Savings Plan is allowed or not -Amit 10-26-2009

    Private Property AllowMarketBasedForSavingsPlan() As Boolean
        Get
            If Not Session("AllowMarketBasedForSavingsPlan_C19") Is Nothing Then
                Return (CType(Session("AllowMarketBasedForSavingsPlan_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("AllowMarketBasedForSavingsPlan_C19") = Value
        End Set
    End Property
    'Added the Property by Dilip on 02-12-2009
    Private Property Bool_NotIncludeAMMatched() As Boolean
        Get
            If Not Session("Bool_NotIncludeAMMatched_C19") Is Nothing Then
                Return (CType(Session("Bool_NotIncludeAMMatched_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("Bool_NotIncludeAMMatched_C19") = Value
        End Set
    End Property
    'Added the Property by Dilip on 02-12-2009
    'Added the Property by Dilip on 02-12-2009
    Private Property Bool_NotIncludeTMMatched() As Boolean
        Get
            If Not Session("Bool_NotIncludeTMMatched_C19") Is Nothing Then
                Return (CType(Session("Bool_NotIncludeTMMatched_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("Bool_NotIncludeTMMatched_C19") = Value
        End Set
    End Property
    'Added the Property by Dilip on 02-12-2009
    'Added the Property used to  check  wheteher market based withdrawal for Savings Plan is allowed or not -Amit 10-26-2009
    'New Modification for Market Based Withdrawal Amit Nigam


    '------ Added to implement WebServiece by pavan ---- Begin  -----------
    Private Property BothPlans() As Boolean
        Get
            If Not Session("BothPlans_C19") Is Nothing Then
                Return (CType(Session("BothPlans_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("BothPlans_C19") = Value
        End Set
    End Property


    Public Property SSNNo() As String
        Get
            If Not Session("SSNNo") Is Nothing Then
                Return (CType(Session("SSNNo"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            Session("SSNNo") = value
        End Set
    End Property
    Private l_FundNo As String
    Public Property FundNo() As String
        Get
            Return DirectCast(l_FundNo, String)
        End Get

        Set(ByVal value As String)
            l_FundNo = value
        End Set
    End Property

    Public Property CopyFinalDataSet_C19() As DataSet
        Get
            If Not Session("CopyFinalDataSet_C19") Is Nothing Then
                Return (DirectCast(Session("CopyFinalDataSet_C19"), DataSet))
            End If
        End Get
        Set(ByVal value As DataSet)
            Session("CopyFinalDataSet_C19") = value
        End Set
    End Property

    Private l_BACurrentAtRequest As Decimal
    Public Property BACurrentAtRequest() As Decimal
        Get
            If l_BACurrentAtRequest <> System.[Decimal].Zero Then
                Return CDec(l_BACurrentAtRequest)
            Else
                Return System.[Decimal].Zero
            End If
        End Get
        Set(ByVal value As Decimal)
            l_BACurrentAtRequest = value
        End Set
    End Property
    Public l_InitialDisplayConsolidatedTotal As DataTable
    Public Property InitialDisplayConsolidatedTotal() As DataTable
        Get
            Return DirectCast(l_InitialDisplayConsolidatedTotal, DataTable)
        End Get
        Set(ByVal value As DataTable)
            l_InitialDisplayConsolidatedTotal = value
        End Set
    End Property
    Public l_DisplayRetirementPlanAcctContribution As DataTable
    Public Property DisplayRetirementPlanAcctContribution() As DataTable
        Get
            Return DirectCast(l_DisplayRetirementPlanAcctContribution, DataTable)
        End Get
        Set(ByVal value As DataTable)
            l_DisplayRetirementPlanAcctContribution = value
        End Set
    End Property
    Public l_DisplaySavingsPlanAcctContribution As DataTable
    Public Property DisplaySavingsPlanAcctContribution() As DataTable
        Get
            Return DirectCast(l_DisplaySavingsPlanAcctContribution, DataTable)
        End Get
        Set(ByVal value As DataTable)
            l_DisplaySavingsPlanAcctContribution = value
        End Set
    End Property
    Public l_DisplayConsolidatedTotal As DataTable
    Public Property DisplayConsolidatedTotal() As DataTable
        Get
            Return DirectCast(l_DisplayConsolidatedTotal, DataTable)
        End Get
        Set(ByVal value As DataTable)
            l_DisplayConsolidatedTotal = value
        End Set
    End Property

    Private l_VoluntryAmount_Retirement As Decimal
    Public Property VoluntryAmount_Retirement() As Decimal
        Get
            If l_VoluntryAmount_Retirement <> System.[Decimal].Zero Then
                Return CDec(l_VoluntryAmount_Retirement)
            Else
                Return System.[Decimal].Zero
            End If
        End Get
        Set(ByVal value As Decimal)
            l_VoluntryAmount_Retirement = value
        End Set
    End Property
    Private l_VoluntryAmount_Savings As Decimal
    Public Property VoluntryAmount_Savings() As Decimal
        Get
            If l_VoluntryAmount_Savings <> System.[Decimal].Zero Then
                Return CDec(l_VoluntryAmount_Savings)
            Else
                Return System.[Decimal].Zero
            End If
        End Get
        Set(ByVal value As Decimal)
            l_VoluntryAmount_Savings = value
        End Set
    End Property
    Private l_Calculated_RetirementPlanAcctContribution As DataTable
    Public Property Calculated_RetirementPlanAcctContribution() As DataTable
        Get
            Return DirectCast(l_Calculated_RetirementPlanAcctContribution, DataTable)
        End Get
        Set(ByVal value As DataTable)
            l_Calculated_RetirementPlanAcctContribution = value
        End Set
    End Property
    Private l_Calculated_SavingsPlanAcctContribution As DataTable
    Public Property Calculated_SavingsPlanAcctContribution() As DataTable
        Get
            Return DirectCast(l_Calculated_SavingsPlanAcctContribution, DataTable)
        End Get
        Set(ByVal value As DataTable)
            l_Calculated_SavingsPlanAcctContribution = value
        End Set
    End Property
    Private l_CalculatedDataTable As DataTable
    Public Property CalculatedDataTable() As DataTable
        Get
            Return DirectCast(l_CalculatedDataTable, DataTable)
        End Get
        Set(ByVal value As DataTable)
            l_CalculatedDataTable = value
        End Set
    End Property
    Private l_Calculated_DisplayRetirementPlanAcctContribution As DataTable
    Public Property Calculated_DisplayRetirementPlanAcctContribution() As DataTable
        Get
            Return DirectCast(l_Calculated_DisplayRetirementPlanAcctContribution, DataTable)
        End Get
        Set(ByVal value As DataTable)
            l_Calculated_DisplayRetirementPlanAcctContribution = value
        End Set
    End Property
    Private l_Calculated_DisplaySavingsPlanAcctContribution As DataTable
    Public Property Calculated_DisplaySavingsPlanAcctContribution() As DataTable
        Get
            Return DirectCast(l_Calculated_DisplaySavingsPlanAcctContribution, DataTable)
        End Get
        Set(ByVal value As DataTable)
            l_Calculated_DisplaySavingsPlanAcctContribution = value
        End Set
    End Property
    Private l_CalculatedDataTableDisplay As DataTable
    Public Property CalculatedDataTableDisplay() As DataTable
        Get
            Return DirectCast(l_CalculatedDataTableDisplay, DataTable)
        End Get
        Set(ByVal value As DataTable)
            l_CalculatedDataTableDisplay = value
        End Set
    End Property

    Private l_CheckboxPartialRetirementEnabled As Boolean
    Public Property CheckboxPartialRetirementEnabled() As Boolean
        Get
            Return CBool(l_CheckboxPartialRetirementEnabled)
        End Get

        Set(ByVal value As Boolean)
            l_CheckboxPartialRetirementEnabled = value
        End Set
    End Property
    Private l_OnlyRetirementPlan As Boolean
    Public Property OnlyRetirementPlan() As Boolean
        Get
            Return CBool(l_OnlyRetirementPlan)
        End Get

        Set(ByVal value As Boolean)
            l_OnlyRetirementPlan = value
        End Set
    End Property
    Private l_OnlySavingsPlan As Boolean
    Public Property OnlySavingsPlan() As Boolean
        Get
            Return CBool(l_OnlySavingsPlan)
        End Get

        Set(ByVal value As Boolean)
            l_OnlySavingsPlan = value
        End Set
    End Property
    Dim m_DataTableDisplayConsolidatedTotal As DataTable
    Public Property DataTableDisplayConsolidatedTotal() As DataTable
        Get
            Return m_DataTableDisplayConsolidatedTotal
        End Get
        Set(ByVal Value As DataTable)
            m_DataTableDisplayConsolidatedTotal = Value
        End Set
    End Property
    Dim m_DataTableDisplayRetirementPlan As DataTable
    Public Property DataTableDisplayRetirementPlan() As DataTable
        Get
            Return m_DataTableDisplayRetirementPlan
        End Get
        Set(ByVal Value As DataTable)
            m_DataTableDisplayRetirementPlan = Value
        End Set
    End Property
    Dim m_DataTableDisplaySavingsPlan As DataTable
    Public Property DataTableDisplaySavingsPlan() As DataTable
        Get
            Return m_DataTableDisplaySavingsPlan
        End Get
        Set(ByVal Value As DataTable)
            m_DataTableDisplaySavingsPlan = Value
        End Set
    End Property
    'To Get / Set HardShip Amount, incase of HardShip.   
    Private l_HardshipAmount As Decimal
    Public Property HardshipAmount() As Decimal
        Get
            If l_HardshipAmount <> System.[Decimal].Zero Then
                Return CDec(l_HardshipAmount)
            Else
                Return System.[Decimal].Zero
            End If
        End Get
        Set(ByVal value As Decimal)
            l_HardshipAmount = value
        End Set
    End Property
    Private l_HasPersonalMoney As Decimal
    Public Property HasPersonalMoney() As Decimal
        Get
            If l_HasPersonalMoney <> System.[Decimal].Zero Then
                Return CDec(l_HasPersonalMoney)
            Else
                Return System.[Decimal].Zero
            End If
        End Get
        Set(ByVal value As Decimal)
            l_HasPersonalMoney = value
        End Set
    End Property
    Private Property VoluntryAmount_Retirement_Initial() As Decimal
        Get
            If Not Session("VoluntryAmount_Retirement_Initial_C19") Is Nothing Then
                Return (DirectCast(Session("VoluntryAmount_Retirement_Initial_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("VoluntryAmount_Retirement_Initial_C19") = Value
        End Set
    End Property
    Private l_AnnuityExists_Retirement As Boolean
    Public Property AnnuityExists_Retirement() As Boolean
        Get
            Return CBool(l_AnnuityExists_Retirement)
        End Get

        Set(ByVal value As Boolean)
            l_AnnuityExists_Retirement = value
        End Set
    End Property
    Private l_AnnuityExists_Savings As Boolean
    Public Property AnnuityExists_Savings() As Boolean
        Get
            Return CBool(l_AnnuityExists_Savings)
        End Get

        Set(ByVal value As Boolean)
            l_AnnuityExists_Savings = value
        End Set
    End Property
    Private l_bool_AnnuityExists As Boolean
    Public Property bool_AnnuityExists() As Boolean
        Get
            Return CBool(l_bool_AnnuityExists)
        End Get

        Set(ByVal value As Boolean)
            l_bool_AnnuityExists = value
        End Set
    End Property
    Private l_SixMonthsValidationForPartial As Boolean
    Public Property SixMonthsValidationForPartial() As Boolean
        Get
            Return CBool(l_SixMonthsValidationForPartial)
        End Get

        Set(ByVal value As Boolean)
            l_SixMonthsValidationForPartial = value
        End Set
    End Property
    Private Property VoluntryAmount_Savings_Initial() As Decimal
        Get
            If Not Session("VoluntryAmount_Savings_Initial_C19") Is Nothing Then
                Return (DirectCast(Session("VoluntryAmount_Savings_Initial_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("VoluntryAmount_Savings_Initial_C19") = Value
        End Set
    End Property
    Public l_TMAmount As Decimal
    Public Property TMAmount() As Decimal
        Get
            If l_TMAmount <> System.[Decimal].Zero Then
                Return CDec(l_TMAmount)
            Else
                Return System.[Decimal].Zero
            End If
        End Get
        Set(ByVal value As Decimal)
            l_TMAmount = value
        End Set
    End Property
    Public l_TDAmount As Decimal
    Public Property TDAmount() As Decimal
        Get
            If l_TDAmount <> System.[Decimal].Zero Then
                Return CDec(l_TDAmount)
            Else
                Return System.[Decimal].Zero
            End If
        End Get
        Set(ByVal value As Decimal)
            l_TDAmount = value
        End Set
    End Property
    Public l_RTAmount As Decimal
    Public Property RTAmount() As Decimal
        Get
            If l_RTAmount <> System.[Decimal].Zero Then
                Return CDec(l_RTAmount)
            Else
                Return System.[Decimal].Zero
            End If
        End Get
        Set(ByVal value As Decimal)
            l_RTAmount = value
        End Set
    End Property
    Private l_Terminateddate As String
    Public Property Terminateddate() As String
        Get
            Return l_Terminateddate
        End Get
        Set(ByVal Value As String)
            l_Terminateddate = Value
        End Set
    End Property
    Private l_RetirementRefundType As String
    Public Property RetirementRefundType() As String
        Get
            Return l_RetirementRefundType
        End Get
        Set(ByVal value As String)
            l_RetirementRefundType = value
        End Set
    End Property
    Private l_SavingsRefundType As String
    Public Property SavingsRefundType() As String
        Get
            Return l_SavingsRefundType
        End Get
        Set(ByVal value As String)
            l_SavingsRefundType = value
        End Set
    End Property
    'START CS | 2016.09.23 | YRS-AT-3164 - Support Request:special Hardship Withdrawal - how to allow contributions to continue temporarily
    Public intIRSOverrideHardship As Integer
    Public Property IRSOverrideHardship() As Integer
        Get
            Return intIRSOverrideHardship
        End Get
        Set(ByVal value As Integer)
            intIRSOverrideHardship = value
        End Set
    End Property
    'END CS | 2016.09.23 | YRS-AT-3164 - Support Request:special Hardship Withdrawal - how to allow contributions to continue temporarily
    '-- START | SR | 2016.06.02 | YRS-AT-2962 | Define Properties
    Private Property RetirementPlanProcessingFee() As Decimal
        Get
            If Not ViewState("RetirementPlanProcessingFee") Is Nothing Then
                Return (CType(ViewState("RetirementPlanProcessingFee"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            ViewState("RetirementPlanProcessingFee") = Value
        End Set
    End Property

    Private Property SavingsPlanProcessingFee() As Decimal
        Get
            If Not ViewState("SavingsPlanProcessingFee") Is Nothing Then
                Return (CType(ViewState("SavingsPlanProcessingFee"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            ViewState("SavingsPlanProcessingFee") = Value
        End Set
    End Property
    Private Property RetirementProcessingFeeMessage() As String
        Get
            If Not ViewState("RetirementProcessingFeeMessage") Is Nothing Then
                Return (CType(ViewState("RetirementProcessingFeeMessage"), String))
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("RetirementProcessingFeeMessage") = Value
        End Set
    End Property
    Private Property SavingsProcessingFeeMessage() As String
        Get
            If Not ViewState("SavingsProcessingFeeMessage") Is Nothing Then
                Return (CType(ViewState("SavingsProcessingFeeMessage"), String))
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("SavingsProcessingFeeMessage") = Value
        End Set
    End Property
    '-- END | SR | 2016.06.02 | YRS-AT-2962 | Define Properties

    '-- START | SR | 2016.07.18 | YRS-AT-3015 | Define Properties
    Private Property IsBALegacyCombinedAmountSwitchedON() As Boolean
        Get
            If Not ViewState("IsBALegacyCombinedAmountSwitchedON") Is Nothing Then
                Return (CType(ViewState("IsBALegacyCombinedAmountSwitchedON"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsBALegacyCombinedAmountSwitchedON") = Value
        End Set
    End Property
    Private Property MaxCombinedBasicAccountAmt() As Decimal
        Get
            If Not ViewState("MaxCombinedBasicAccountAmt") Is Nothing Then
                Return (CType(ViewState("MaxCombinedBasicAccountAmt"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            ViewState("MaxCombinedBasicAccountAmt") = Value
        End Set
    End Property

    Private Property MaxYMCAAcctAmt() As Decimal
        Get
            If Not ViewState("MaxYMCAAcctAmt") Is Nothing Then
                Return (CType(ViewState("MaxYMCAAcctAmt"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            ViewState("MaxYMCAAcctAmt") = Value
        End Set
    End Property

    Private Property MaxYMCALegacyAcctAmt() As Decimal
        Get
            If Not ViewState("MaxYMCALegacyAcctAmt") Is Nothing Then
                Return (CType(ViewState("MaxYMCALegacyAcctAmt"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            ViewState("MaxYMCALegacyAcctAmt") = Value
        End Set
    End Property
    '-- END | SR | 2016.07.18 | YRS-AT-3015 | Define Properties


    '------ Added to implement WebServiece by pavan ---- END    -----------

    ' START: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)
    Private Property IsRMDCheckBoxChecked() As Boolean
        Get
            If Not ViewState("IsRMDCheckBoxChecked") Is Nothing Then
                Return (DirectCast(ViewState("IsRMDCheckBoxChecked"), Boolean))
            Else
                Return False
            End If

        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsRMDCheckBoxChecked") = Value
        End Set
    End Property

    Private Property RMDLastYrsWarningMessage() As String
        Get
            If Not ViewState("RMDLastYrsWarningMessage") Is Nothing Then
                Return (DirectCast(ViewState("RMDLastYrsWarningMessage"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            ViewState("RMDLastYrsWarningMessage") = Value
        End Set
    End Property

    Private Property RMDCurrentYrsWarningMessage() As String
        Get
            If Not ViewState("RMDCurrentYrsWarningMessage") Is Nothing Then
                Return (DirectCast(ViewState("RMDCurrentYrsWarningMessage"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            ViewState("RMDCurrentYrsWarningMessage") = Value
        End Set
    End Property

    Private Property RMDCurrentYearSavings() As String
        Get
            If Not ViewState("RMDCurrentYearSavings") Is Nothing Then
                Return (DirectCast(ViewState("RMDCurrentYearSavings"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            ViewState("RMDCurrentYearSavings") = Value
        End Set
    End Property

    Private Property RMDCurrentYearRetirement() As String
        Get
            If Not ViewState("RMDCurrentYearRetirement") Is Nothing Then
                Return (DirectCast(ViewState("RMDCurrentYearRetirement"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            ViewState("RMDCurrentYearRetirement") = Value
        End Set
    End Property

    Private Property RMDPreviousYearRetirement() As String
        Get
            If Not ViewState("RMDPreviousYearRetirement") Is Nothing Then
                Return (DirectCast(ViewState("RMDPreviousYearRetirement"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            ViewState("RMDPreviousYearRetirement") = Value
        End Set
    End Property

    Private Property RMDPreviousYearSavings() As String
        Get
            If Not ViewState("RMDPreviousYearSavings") Is Nothing Then
                Return (DirectCast(ViewState("RMDPreviousYearSavings"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            ViewState("RMDPreviousYearSavings") = Value
        End Set
    End Property

    Private Property RMDPreviousYearPlanType() As String
        Get
            If Not ViewState("RMDPreviousYearPlanType") Is Nothing Then
                Return (DirectCast(ViewState("RMDPreviousYearPlanType"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            ViewState("RMDPreviousYearPlanType") = Value
        End Set
    End Property

    Private Property RMDSelectedPlanType() As String
        Get
            If Not ViewState("RMDSelectedPlanType") Is Nothing Then
                Return (DirectCast(ViewState("RMDSelectedPlanType"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            ViewState("RMDSelectedPlanType") = Value
        End Set
    End Property
    ' END: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)

    ' START: SB | 2016.12.16 | YRS-AT-3073 | To store MRDCurrentYear check box result of savings and retriement checkboxes
    Private Property IsMRDSavingsCurrentYear() As Boolean
        Get
            If Not ViewState("IsMRDSavingsCurrentYear") Is Nothing Then
                Return (DirectCast(ViewState("IsMRDSavingsCurrentYear"), Boolean))
            Else
                Return False
            End If

        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsMRDSavingsCurrentYear") = Value
        End Set
    End Property

    Private Property IsMRDRetirementCurrentYear() As Boolean
        Get
            If Not ViewState("IsMRDRetirementCurrentYear") Is Nothing Then
                Return (DirectCast(ViewState("IsMRDRetirementCurrentYear"), Boolean))
            Else
                Return False
            End If

        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsMRDRetirementCurrentYear") = Value
        End Set
    End Property
    ' END: SB | 2016.12.16 | YRS-AT-3073 | To store MRDCurrentYear check box result of savings and retriement checkboxes

    Private Property IsAccountSelectionChangeFromRetirement() As Boolean
        Get
            If Not ViewState("IsAccountSelectionChangeFromRetirement") Is Nothing Then
                Return (DirectCast(ViewState("IsAccountSelectionChangeFromRetirement"), Boolean))
            Else
                Return False
            End If

        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsAccountSelectionChangeFromRetirement") = Value
        End Set
    End Property

    Private Property BlendedTaxRate() As Decimal
        Get
            If Not ViewState("BlendedTaxRate") Is Nothing Then
                Return (DirectCast(ViewState("BlendedTaxRate"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            ViewState("BlendedTaxRate") = Value
        End Set
    End Property

    Private Property TotalTaxWithheld() As Decimal
        Get
            If Not ViewState("TotalTaxWithheld") Is Nothing Then
                Return (DirectCast(ViewState("TotalTaxWithheld"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            ViewState("TotalTaxWithheld") = Value
        End Set
    End Property
#End Region

#Region "Page Load"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim l_bool_readonly As Boolean

        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Me.TextboxTaxRate.Attributes.Add("onkeypress", "Javascript: return ValidateNumeric();")
        Me.TextboxPartialRetirement.Attributes.Add("onkeypress", "Javascript: return ValidateNumeric();")
        Me.TextboxPartialSavings.Attributes.Add("onkeypress", "Javascript: return ValidateNumeric();")
        Me.TextboxPartialSavings.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
        Me.TextboxPartialSavings.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
        Me.TextboxPartialRetirement.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
        Me.TextboxPartialRetirement.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
        '-----------------  Service  START ----------------------
        objService.Timeout = -1
        '------------------------- Service END  ----------------------------------

        Try
            If Not Page.IsPostBack Then
                ClearSession()
                objRefundRequest.LoadRefundConfiguration() 'MMR | 07/23/2019 | YRS-AT-4498 | Set configuration values related to refund
                'START: CS | 2016.10.12 | YRS-AT-3073 | Initializing null  
                Me.RMDCurrentYrsWarningMessage = Nothing
                Me.RMDCurrentYearSavings = Nothing
                Me.RMDCurrentYearRetirement = Nothing
                Me.RMDPreviousYearRetirement = Nothing
                Me.RMDPreviousYearSavings = Nothing
                Me.RMDPreviousYearPlanType = Nothing
                Me.RMDSelectedPlanType = Nothing
                'END: CS | 2016.10.12 | YRS-AT-3073 | Initializing null  

                

                '----------------- Implementing the Service  START ----------------------
                If FinalDataSet Is Nothing Then
                    DivIRSOverrideRules.Visible = False 'CS | 2016.09.23 | YRS-AT-3164 - To Visiable /Invisiable Check box of IRS Override Hard ship rules
                    objService.PreAuthenticate = True
                    objService.Credentials = System.Net.CredentialCache.DefaultCredentials

                    'IB:08-Dec-2010 for BT:607 Special/Disability withdrawals should not apply withdrawal rules
                    'objWebServiceReturn = objService.GetParticipantData(Me.FundID)
                    If Me.RefundType = "SPEC" Or Me.RefundType = "DISAB" Then
                        objWebServiceReturn = objService.GetParticipantForSpecOrDisbData(Me.FundID, Me.RefundType)
                    Else
                        objWebServiceReturn = objService.GetParticipantData_C19(Me.FundID)
                    End If

                    FinalDataSet = objWebServiceReturn.WebServiceDataSet
                    If objWebServiceReturn.ReturnStatus Then
                        Session("FinalDataSet_C19") = FinalDataSet
                    Else
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", objWebServiceReturn.Message, MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                End If
                LoadInformationToControls()
                getSecuredControls() 'CS | 2016.09.23 | YRS-AT-3164 - To Checking access right for the Check box of IRS Override Hard ship rules
                'New Modification for Market Based Withdrawal Amit Nigam
                DisplayToolTip()
                'New Modification for Market Based Withdrawal Amit Nigam
                'Added to verify the totals-Amit
                'New Modification for Market Based Withdrawal Amit Nigam
                If LabelMarket.Visible = True Then
                    If CheckboxPartialRetirement.Checked = False Or CheckboxPartialSavings.Checked = False Then
                        'DisplayMarketInstallments()
                    Else
                        ClearMarketTextBoxes()
                    End If

                End If
                EnableDisablePartialCheckBox()
                Me.SetTextBoxReadOnly()
                Me.SetNoRefundAllowed()

                'Comment code Deleted By Sanjeev on 06/02/2012

                'START: SG: 2012.03.23: BT-1012
                Dim l_TermDateDataTabel As DataTable
                'SG: BT-1012(Re-Opened): 2012.05.31
                'Dim l_dtTermDate As DateTime
                Dim l_dtTermDate As Nullable(Of DateTime)

                If Not Session("TermDateDataTabel_C19") Is Nothing Then
                    l_TermDateDataTabel = CType(Session("TermDateDataTabel_C19"), DataTable)
                    If l_TermDateDataTabel.Rows.Count > 0 Then
                        If Not l_TermDateDataTabel.Rows(0)("TermDate") Is Nothing AndAlso Not IsDBNull(l_TermDateDataTabel.Rows(0)("TermDate")) Then
                            'l_dtTermDate = l_TermDateDataTabel.Rows(0)("TermDate")
                            l_dtTermDate = Convert.ToDateTime(l_TermDateDataTabel.Rows(0)("TermDate"))
                        End If
                    End If
                End If

                If Me.TerminationPIA > Me.MaximumPIAAmount AndAlso IsTerminated = True AndAlso l_dtTermDate < "1996-01-01" AndAlso Me.RefundType <> "SPEC" Then
                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Participant terminated before 1/1/1996.  Please refer to 'Participant History Report: Legacy' report for balance at termination and use Special Withdrawal process if necessary.", MessageBoxButtons.OK, False)
                End If
                'END: SG: 2012.03.23: BT-1012
                DivMainMessage.Visible = False  'SR | 2016.06.03 | YRS-AT-2962 | Hide message div
            Else
                '------ Added for WebServiece by pavan ---- Begin  -----------                
                'SetGlobalFlags()
                Me.SetTextBoxReadOnly()
                '------ Added for WebServiece by pavan ---- END    -----------
                LabelMessage.Visible = False
                Dim l_string_MessageForRT As String = ""
                If Me.SessionStatusType.Trim.ToUpper = "TM" Then
                    l_string_MessageForRT = ValidateBalanceInAccountsForVol(True)
                    If l_string_MessageForRT <> "" Then
                        'wrote if condition to validate the refund type-Amit
                        If Me.RefundType = "" Then
                            Me.RefundType = "VOL"
                        End If
                        'Modified the if condition to validate tthe totals. Amit
                        If CheckboxPartialSavings.Checked = False And Me.CheckboxPartialSavings.Enabled = True Then
                            Me.CheckboxSavingsVoluntary.Checked = True
                        End If
                    End If
                End If

                Me.LoadCalculatedTable("IsRetirement")
                Me.LoadCalculatedTable("IsSavings")
                Me.LoadCalculatedTable("IsConsolidated")

                If Request.Form("Ok") = "OK" Then
                    Exit Sub
                End If
                ' START: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)
                If Me.IsRMDCheckBoxChecked Then
                    If Not Request.Form("No") Is Nothing Then
                        If Request.Form("No").Trim.ToUpper = "NO" Then
                            SetCheckboxNoWarningMessage()
                            BindMrdGrid("SAVINGS")
                            BindMrdGrid("RETIREMENT")
                            Me.IsRMDCheckBoxChecked = False
                            Exit Sub
                        End If
                    ElseIf Not Request.Form("Yes") Is Nothing Then
                        If Request.Form("Yes").Trim.ToUpper = "YES" Then
                            BindMrdGrid("SAVINGS")
                            BindMrdGrid("RETIREMENT")
                            Me.RMDCurrentYrsWarningMessage = Nothing
                            Me.RMDLastYrsWarningMessage = Nothing
                            If CheckboxMRDRetirement.Checked Or CheckboxMRDSavings.Checked Or CheckboxMRDRetirementCurrentYear.Checked Or CheckboxMRDSavingsCurrentYear.Checked Then
                                Me.ButtonSave.Visible = True
                            End If
                            Me.IsRMDCheckBoxChecked = False
                            Exit Sub
                        End If
                    End If
                End If
                ' END: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)

                'START: SG: 2012.06.14: BT-1043
                If Session("PriorYearRMDRecords_C19") = "Yes" Then
                    If Not Request.Form("Yes") Is Nothing Then
                        If Request.Form("Yes").Trim.ToUpper = "YES" AndAlso Session("PriorYearRMDRecords_C19") = "Yes" Then
                            DoBtnSaveProcessing()
                            Session("PriorYearRMDRecords_C19") = Nothing
                            Exit Sub
                        End If
                    End If
                End If
                'END: SG: 2012.06.14: BT-1043
            End If

            'to clear the value of the session for displaying the message in the fot the participant over the age 75 and half-Amit 30-09-2009
            objRefundRequest.DisplayMessageForLetters = False
            'to clear the value of the session for displaying the message in the fot the participant over the age 75 and half-Amit 30-09-2009

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("Page_Load-RefundRequestWebForm_C19", ex)              ' SB | 2016.12.13 | YRS-AT-3073 | Handled Exception and it is been logged 
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
#End Region

#Region "Functions"

    Private Function PopulateAvailableCheckBoxPerPlan()
        Try
            Select Case Me.SessionStatusType.Trim.ToUpper
                Case "PE", "RE"
                    Me.CheckboxRetirementPlan.Checked = False
                    Me.CheckboxRetirementPlan.Enabled = False
                    Me.CheckboxRegular.Checked = False
                    Me.CheckboxRegular.Enabled = False
                    Me.CheckboxExcludeYMCA.Checked = False
                    Me.CheckboxExcludeYMCA.Enabled = False
                    Me.CheckboxVoluntryAccounts.Checked = False
                    Me.CheckboxVoluntryAccounts.Enabled = False
                    'START - SB | 2019.07.24 | YRS-AT-2169 | 20.6.5 | Allow hardship withdrawal to non particpants
                Case "NP", "PENP", "RDNP"
                    Me.CheckboxRegular.Checked = False
                    Me.CheckboxRegular.Enabled = False
                    Me.CheckboxExcludeYMCA.Checked = False
                    Me.CheckboxExcludeYMCA.Enabled = False
                    Me.CheckboxHardship.Checked = False
                    Me.CheckboxHardship.Enabled = True

                    ' Case "ML", "PEML", "NP", "PENP", "RDNP" 'SB | 2019.07.17 | YRS-AT-2169 | Old code commented
                Case "ML", "PEML"
                    'END - SB | 2019.07.24 | YRS-AT-2169 | 20.6.5 | Allow hardship withdrawal to non particpants
                    'with new fund event status the above status can only take Voluntary Refunds
                    Me.CheckboxRegular.Checked = False
                    Me.CheckboxRegular.Enabled = False
                    Me.CheckboxExcludeYMCA.Checked = False
                    Me.CheckboxExcludeYMCA.Enabled = False
                    Me.CheckboxHardship.Checked = False
                    Me.CheckboxHardship.Enabled = False

                Case "RP", "RDTA", "RQTA", "DQ"
                    'the above status are not entitled for any refund
                    Me.CheckboxRetirementPlan.Checked = False
                    Me.CheckboxRetirementPlan.Enabled = False
                    Me.CheckboxRegular.Checked = False
                    Me.CheckboxRegular.Enabled = False
                    Me.CheckboxExcludeYMCA.Checked = False
                    Me.CheckboxExcludeYMCA.Enabled = False
                    Me.CheckboxVoluntryAccounts.Checked = False
                    Me.CheckboxVoluntryAccounts.Enabled = False
                    Me.CheckboxSavingsPlan.Checked = False
                    Me.CheckboxSavingsPlan.Enabled = False
                    Me.CheckboxHardship.Checked = False
                    Me.CheckboxHardship.Enabled = False
                    Me.CheckboxSavingsVoluntary.Checked = False
                    Me.CheckboxSavingsVoluntary.Enabled = False
            End Select

        Catch
            Throw
        End Try

    End Function

    Private Function PopulateConsolidatedTotals()
        Dim bool_RetirementPlanForgrid As Boolean = False
        Dim bool_SavingPlanforGrid As Boolean = False
        Try
            If Me.CheckboxSpecial.Checked = True Or _
               Me.CheckboxDisability.Checked = True Or _
               Me.CheckboxRegular.Checked = True Or _
               Me.CheckboxExcludeYMCA.Checked = True Or _
               Me.CheckboxVoluntryAccounts.Checked = True Or _
               (Me.CheckboxPartialRetirement.Checked = True And Not (Me.TextboxPartialRetirement.Text.ToString() = String.Empty)) Then
                bool_RetirementPlanForgrid = True
            Else
                bool_RetirementPlanForgrid = False
            End If

            If Me.CheckboxSavingsVoluntary.Checked = True Or _
               Me.CheckboxHardship.Checked = True Or _
               (Me.CheckboxPartialSavings.Checked = True And Not (Me.TextboxPartialSavings.Text.ToString() = String.Empty)) Then
                bool_SavingPlanforGrid = True
            Else
                bool_SavingPlanforGrid = False
            End If

            Me.MakeDisplayCalculationDataTables(bool_RetirementPlanForgrid, bool_SavingPlanforGrid)

        Catch
            Throw
        End Try
    End Function
    Private Function LoadandSetRefundTypes() As Boolean
        Try
            Select Case Me.RefundType.Trim.ToUpper

                Case "SPEC"
                    'Comment code Deleted By Sanjeev on 06/02/2012
                Case "DISAB"
                    Me.CheckboxRegular.Visible = False
                    Me.CheckboxSpecial.Visible = False
                    Me.CheckboxRetirementPlan.Enabled = False
                    settextboxvisibleRetirement(False)
                    settextboxvisibleSavings(False)
                    CheckboxPartialRetirement.Enabled = False
                    CheckboxPartialSavings.Enabled = False
                    'Comment code Deleted By Sanjeev on 06/02/2012
                Case Else
                    ' we will call full refund to get the consolidated data table
                    Me.FullRefund("BothPlans")

                    If Me.RefundType.Trim.ToUpper() <> "SPEC" And Me.RefundType.Trim.ToUpper() <> "DISAB" Then
                        Me.RefundType = "REG"
                    End If
                    FullRefundForDisplay("BothPlans")
                    If Me.RefundType.Trim.ToUpper() <> "SPEC" And Me.RefundType.Trim.ToUpper() <> "DISAB" Then
                        Me.RefundType = ""
                    End If
                    'we need this session to again set/reset the personal check box
                    Session("YMCAAvailableAmountInitial_C19") = objRefundRequest.YMCAAvailableAmount
                    'we need this session to again set/reset the personal check box
                    'by Aparna 18/10/2007
                    'need the original total amount to determine the balance amount after the withdrawal
                    'to check if the person has a balance of $5000 to take annnuity or not.
                    Session("HasPersonalMoney_C19") = objRefundRequest.HasPersonalMoney
                    Session("RetirementAmountTotal_C19") = Me.RetirementPlan_TotalAmount

                    Session("SavingsPlanAmountTotal_C19") = Me.SavingsPlan_TotalAmount
                    Me.RetirementPlan_TotalAvailableAmount = Session("RetirementAmountTotal_C19")
                    Me.SavingsPlan_TotalAvailableAmount = Session("SavingsPlanAmountTotal_C19")
                    'CheckAnnuityExistence()
                    If Me.IsTerminated = True Then
                        CheckTotalsForAnnuity() 'by Aparna 23/11/2007
                    End If

                    'by Aparna 18/10/2007
            End Select

        Catch
            Throw
        End Try
    End Function
    Private Sub SetTextBoxReadOnly()
        Try
            'Comment code Deleted By Sanjeev on 06/02/2012
            Me.TextboxVested.ReadOnly = True
            Me.TextBoxTerminated.ReadOnly = True
            Me.TextboxAge.ReadOnly = True
            Me.TextboxTerminatePIA.ReadOnly = True
            'BA Account YMCA PhaseV 03-04-2009 by Dilip
            Me.TextboxBATerminate.ReadOnly = True
            'BA Account YMCA PhaseV 03-04-2009 by Dilip
            ' Me.TextboxCurrentPIA.ReadOnly = True 'Not used as per the new requirement commented by Aparna 15/10/2007
            'Priya 12-Dec-2008 : YRS 5.0-523 In Withdrawal request screen change the Tax Rate to not editable.
            'Me.TextboxTaxRate.ReadOnly = False
            Me.TextboxTaxRate.ReadOnly = True
            Me.TextboxNonTaxed.ReadOnly = True
            Me.TextboxTaxed.ReadOnly = True
            Me.TextboxTax.ReadOnly = True
            Me.TextboxNet.ReadOnly = True
            'Added by amit to make the readonly properties for new added textboxed for partial withdrawal
            Me.TextboxRetirementNetAmount.ReadOnly = True
            Me.TextboxRetirementNonTaxable.ReadOnly = True
            Me.TextboxRetirementTaxable.ReadOnly = True
            Me.TextboxRetirementTaxWithheld.ReadOnly = True
            Me.TextboxSavingsNetAmount.ReadOnly = True
            Me.TextboxSavingsNonTaxable.ReadOnly = True
            Me.TextboxSavingsTaxable.ReadOnly = True
            Me.TextboxSavingsTaxWithheld.ReadOnly = True
            Me.txtAggregateBALegacyatRequest.ReadOnly = True 'SN | 05/13/2019 | YRS-AT-4055 | Added to set textbox as readonly
            'Added by amit to make the readonly properties for new added textboxed for partial withdrawal
        Catch
            Throw
        End Try

    End Sub


    Private Function LoadDefaults()

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow


        Try

            'Default Minimum Distribution Amount
            Me.MinimumDistributionAmount = 0.0

            'Default Minimum Distribution Tax Rate
            Me.MinimumDistributionTaxRate = 10.0

            'Default Tax Rate 
            'Me.FederalTaxRate = 20.0 'Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code to remove harcoded tax rate

            'Default Expiry Days
            Me.RefundExpiryDate = 0

            'Default Voluntry Amount 
            Me.VoluntryAmount = 0.0

            'Default TD Account Amount
            Me.TDAccountAmount = 0.0


            'This segment for Calculating the Voluntry Amount
            l_DataTable = DirectCast(Session("AccountContribution_C19"), DataTable)
            If Not l_DataTable Is Nothing Then

                For Each l_DataRow In l_DataTable.Rows

                    If Not (l_DataRow("AccountType").GetType.ToString = "System.DBNull") Then

                        If Not CType(l_DataRow("AccountType"), String).Trim = "Total" Then '' Avoid the last row to calcuate ... B'cos it consits the total..

                            If Me.IsBasicAccount(l_DataRow) = False Then

                                '' Assign the Voluntry amount

                                If Not l_DataRow("Total").GetType.ToString = "System.DBNull" Then
                                    Me.VoluntryAmount += CType(l_DataRow("Total"), Decimal)
                                End If

                                If (CType(l_DataRow("AccountGroup"), String).Trim = m_const_SavingsPlan_TD) Or (CType(l_DataRow("AccountGroup"), String).Trim = m_const_SavingsPlan_TM) Then
                                    Me.TDAccountAmount += CType(l_DataRow("Total"), Decimal)
                                End If

                            End If

                        End If

                    End If

                Next

            End If


            '' Disable the Save Button
            Me.ButtonSave.Visible = False



        Catch
            Throw
        End Try
    End Function

    Private Function LoadInformation()

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        Try
            'PersonInformation

            l_DataSet = Session("PersonInformation_C19")
            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Member Details")

                If Not l_DataTable Is Nothing Then
                    If l_DataTable.Rows.Count > 0 Then
                        Me.LoadGeneralInfoToControls(l_DataTable.Rows.Item(0))
                    End If
                End If

                ' This segment for 
                l_DataTable = l_DataSet.Tables("Member Employment")

                If Not l_DataTable Is Nothing Then
                    If l_DataTable.Rows.Count > 0 Then
                        'Me.LoadEmploymentDetails(l_DataTable.Rows.Item(0))
                    End If
                End If


            End If

        Catch
            Throw
        End Try
    End Function

    Private Function LoadEmploymentDetails(ByVal parameterDataRow As DataRow)
        Try

            If Not parameterDataRow Is Nothing Then

                If parameterDataRow("PersonID").GetType.ToString = "System.DBNull" Then
                    Me.PersonID = String.Empty
                Else
                    Me.PersonID = CType(parameterDataRow("PersonID"), String)
                End If

                If parameterDataRow("FundEventID").GetType.ToString = "System.DBNull" Then
                    Me.FundID = String.Empty
                Else
                    Me.FundID = CType(parameterDataRow("FundEventID"), String)
                End If

            End If

        Catch
            Throw
        End Try

    End Function

    Private Function LoadGeneralInfoToControls(ByVal parameterDataRow As DataRow)
        Try

            Dim l_personage As Decimal
            Dim l_QDROParticipantAge As Decimal
            If Not parameterDataRow Is Nothing Then
                'Comment code Deleted By Sanjeev on 06/02/2012

                'by aparna 15/10/2007
                If Not Session("FundNo") Is Nothing Then
                    Me.LabelFundNo.Text = CType(Session("FundNo"), String).Trim()
                End If
                If Not IsDBNull(parameterDataRow("SS No")) Then

                    LabelSSNo.Text = CType(parameterDataRow("SS No"), String)
                    Dim strSSN As String = Me.LabelSSNo.Text.Insert(3, "-")
                    strSSN = strSSN.Insert(6, "-")
                    Me.LabelSSNo.Text = strSSN
                End If
                If Not IsDBNull(parameterDataRow("Last Name")) Then
                    'modified by Aparna 15/10/2007 change in UI
                    'Me.TextBoxLastName.Text = CType(parameterDataRow("Last Name"), String)
                    Me.LastName = CType(parameterDataRow("Last Name"), String)
                End If
                If Not IsDBNull(parameterDataRow("Middle Name")) Then
                    'modified by Aparna 15/10/2007 change in UI
                    ' Me.TextBoxMiddleName.Text = CType(parameterDataRow("Middle Name"), String)
                    Me.MiddleName = CType(parameterDataRow("Middle Name"), String)
                End If
                If Not IsDBNull(parameterDataRow("First Name")) Then
                    'modified by Aparna 15/10/2007 change in UI
                    'Me.TextBoxFirstName.Text = CType(parameterDataRow("First Name"), String)
                    Me.FirstName = CType(parameterDataRow("First Name"), String)
                End If
                'Modified by Shubhrata Apr4th,2007 Handled DBNUll condition 
                'by Aparna 15/10/2007 change in UI 
                'Me.LabelTitle.Text = Me.LabelSSNo.Text & " " & Me.TextboxLastName.Text.Trim & " " & Me.TextboxMiddleName.Text.Trim() & " " & me.TextboxFirstName.Text.trim()
                Me.LabelParticipantName.Text = Me.LastName.Trim() & " " & MiddleName.Trim() & " " & FirstName.Trim()

                If (parameterDataRow("VestingDate").GetType.ToString = "System.DBNull") Then
                    Me.TextboxVested.Text = "No"
                    Me.IsVested = False
                Else
                    Me.TextboxVested.Text = "Yes"
                    Me.IsVested = True
                End If

                'Comment code Deleted By Sanjeev on 06/02/2012

                If (parameterDataRow("BirthDate").GetType.ToString = "System.DBNull") Then
                    Me.TextboxAge.Text = "0.00"
                    Me.PersonAge = 0.0
                ElseIf (parameterDataRow("DeathDate").GetType.ToString = "System.DBNull") Then
                    Me.PersonAge = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), String.Empty)
                    'Commented to verify the age
                    'Me.TextboxAge.Text = Me.PersonAge.ToString("0.0")
                    Me.TextboxAge.Text = Me.PersonAge.ToString("0.00")
                Else
                    Me.PersonAge = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), CType(parameterDataRow("DeathDate"), String))
                    'Commented to verify the age
                    'Me.TextboxAge.Text = Me.PersonAge.ToString("0.0")
                    Me.TextboxAge.Text = Me.PersonAge.ToString("0.00")
                End If

                If (parameterDataRow("StatusType").GetType.ToString = "System.DBNull") Then
                    Me.TextBoxTerminated.Text = String.Empty
                    Me.IsTerminated = False
                    Me.IsRetiredActive = False
                Else
                    Dim l_string_FundStatus As String = ""
                    l_string_FundStatus = CType(parameterDataRow("StatusType"), String).Trim.ToUpper()
                    Select Case l_string_FundStatus
                        Case "RA"
                            Me.IsTerminated = False
                            Me.IsRetiredActive = True
                            Me.TextBoxTerminated.Text = "No"
                        Case "QD"
                            If (parameterDataRow("BirthDate").GetType.ToString = "System.DBNull") Then
                                Me.TextboxAge.Text = "0.00"
                            ElseIf (parameterDataRow("DeathDate").GetType.ToString = "System.DBNull") Then
                                l_personage = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), String.Empty)
                            Else
                                l_personage = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), CType(parameterDataRow("DeathDate"), String))
                            End If
                            If Not Session("QDROParticipantAge_C19") Is Nothing Then
                                l_QDROParticipantAge = DirectCast(Session("QDROParticipantAge_C19"), Decimal)
                            End If
                            If l_personage < 55 Or l_QDROParticipantAge < 50 Then
                                Me.IsVested = True
                                Me.IsTerminated = True
                                Me.TextBoxTerminated.Text = "Yes"
                            Else
                                'Me.IsVested = False
                                'Me.IsTerminated = False
                                Me.TextBoxTerminated.Text = "No"
                            End If
                        Case "AE", "PE", "RE", "ML", "NP", "PENP", "RDNP"
                            Me.TextBoxTerminated.Text = "No"
                            Me.IsTerminated = False
                            Me.IsRetiredActive = False
                            'Shubhrata  05/22/2008  bug id 455 'ML will be non-terminated
                            'Shubhrata  05/22/2008  bug id 455 'ML will be non-terminated
                            'NEERAJ     14/SEP/2008  bug id YRS 5.0-820 ML will be non-terminated
                            'NEERAJ     29/SEP/2009  bug id YRS 5.0-820 for fund status NP,PENP, RDNP set termination status as "No"

                        Case l_string_FundStatus
                            Me.TextBoxTerminated.Text = "Yes"
                            Me.IsTerminated = True
                            Me.IsRetiredActive = False
                    End Select
                End If

                'Changed to Select Case by Shubhrata - Apr 17,2008 - Also included the new fund status
                'Modified as per the additional partial request for the reports to display the message if the participant age is more then 70.5 and stop the generating withdrawal letter-Amit 30.09.2009
                Dim l_datetime_personAge As DateTime = parameterDataRow("BirthDate")
                Me.PersonAgeatEndofYear = CalculateAgeattheEndofYear(l_datetime_personAge, String.Empty)
                'Modified as per the additional partial request for the reports to display the message if the participant age is more then 70.5 and stop the generating withdrawal letter-Amit 30.09.2009

            End If

        Catch
            Throw
        End Try

    End Function

    Private Function CalculateAge(ByVal parameterDOB As String, ByVal parameterDOD As String) As Decimal
        Dim numTotalNumberofDays As Decimal
        Dim numAge As Decimal
        Dim numReminder As Decimal
        Try
            'Comment code Deleted By Sanjeev on 06/02/2012
            If parameterDOB = String.Empty Then Return 0
            numTotalNumberofDays = DateDiff(DateInterval.Day, CType(parameterDOB, DateTime), Now.Date)
            numReminder = (numTotalNumberofDays Mod 365.2524)
            If numReminder > 0 Then
                numAge = Math.Floor((numTotalNumberofDays - numReminder) / 365.2425) + (Math.Floor(numReminder / 30.5) / 100)
            Else
                numAge = Math.Floor((numTotalNumberofDays - numReminder) / 365.2425)
            End If
            CalculateAge = numAge
        Catch
            Throw
        End Try
    End Function
    'Modified as per the additional partial request for the reports to display the message if the participant age is more then 70.5 and stop the generating withdrawal letter-Amit 30.09.2009
    Private Function CalculateAgeattheEndofYear(ByVal parameterDOB As String, ByVal parameterDOD As String) As Decimal
        Dim numTotalNumberofDays As Decimal
        Dim numAge As Decimal
        Dim numReminder As Decimal
        Dim numLastDateofYear As DateTime
        Try
            'Comment code Deleted By Sanjeev on 06/02/2012
            numLastDateofYear = DateAdd(DateInterval.Second, -3, DateAdd(DateInterval.Year, DateDiff(DateInterval.Year, Date.MinValue, Today()) + 1, Date.MinValue))
            If parameterDOB = String.Empty Then Return 0
            numTotalNumberofDays = DateDiff(DateInterval.Day, CType(parameterDOB, DateTime), numLastDateofYear)
            numReminder = (numTotalNumberofDays Mod 365.2524)
            If numReminder > 0 Then
                numAge = Math.Floor((numTotalNumberofDays - numReminder) / 365.2425) + (Math.Floor(numReminder / 30.5) / 100)
            Else
                numAge = Math.Floor((numTotalNumberofDays - numReminder) / 365.2425)
            End If
            CalculateAgeattheEndofYear = numAge
        Catch
            Throw
        End Try
    End Function
    'Modified as per the additional partial request for the reports to display the message if the participant age is more then 70.5 and stop the generating withdrawal letter-Amit 30.09.2009

    Private Function LoadPIAAmountold()
        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManger As CacheManager
        'Hafiz 03Feb06 Cache-Session

        Try
            'Comment code Deleted By Sanjeev on 06/02/2012

            Me.TerminationPIA = DirectCast(Session("TerminationPIA_C19"), Decimal)
            Me.CurrentPIA = DirectCast(Session("CurrentPIA_C19"), Decimal)

            Me.BATermination = DirectCast(Session("BATermination_C19"), Decimal)
            Me.BACurrent = DirectCast(Session("BACurrent_C19"), Decimal)
            'Added by ganeswar sahoo on july 09
            Me.TextboxBATerminate.Text = DirectCast(Session("BATermination_C19"), Decimal) ' FormatCurrency(Me.BATermination) ' FormatCurrency(Me.BATermination) ' CType(Session("BACurrent_C19"), Decimal) '
            'Added by ganeswar sahoo on july 09
            '  Me.TextboxCurrentPIA.Text = FormatCurrency(Me.CurrentPIA) 'by Aparna 15/10/2007 as per new requirement
            Me.TextboxTerminatePIA.Text = FormatCurrency(Me.TerminationPIA)
            'Hafiz 03Feb06 Cache-Session
        Catch
            Throw
        End Try
    End Function
    Private Function LoadPIAAmount()
        Try
            Dim l_decimal_CurrentPIA As Decimal
            Dim l_decimal_PIATermination As Decimal
            Dim l_decimal_BACurrent As Decimal
            Dim l_decimal_BATermination As Decimal
            'Added by ganeswar sahoo on july 09 for Gemini Issue
            Dim l_decimal_BACurrentAtRequest As Decimal
            'Added by ganeswar sahoo on july 09 for Gemini Issue
            If IsPostBack Then
                Me.TerminationPIA = DirectCast(Session("TerminationPIA_C19"), Decimal)
                Me.CurrentPIA = DirectCast(Session("CurrentPIA_C19"), Decimal)

                Me.BATermination = DirectCast(Session("BATermination_C19"), Decimal)
                Me.BACurrent = DirectCast(Session("BACurrent_C19"), Decimal)
                'Added by ganeswar sahoo on july 09
                Me.TextboxBATerminate.Text = FormatCurrency(Me.BATermination)
                'Added by ganeswar sahoo on july 09
                Me.TextboxTerminatePIA.Text = FormatCurrency(Me.TerminationPIA)
            Else
                l_decimal_CurrentPIA = YMCARET.YmcaBusinessObject.RefundRequest.GetCurrentPIA(Me.FundID)
                l_decimal_PIATermination = YMCARET.YmcaBusinessObject.RefundRequest.GetTerminatePIA(Me.FundID)
                l_decimal_BACurrent = YMCARET.YmcaBusinessObject.RefundRequest.GetBACurrentPIA(Me.FundID)
                l_decimal_BATermination = YMCARET.YmcaBusinessObject.RefundRequest.GetBATerminatePIA(Me.FundID)
                'Added by ganeswar sahoo on july 09 for Gemini Issue
                l_decimal_BACurrentAtRequest = YMCARET.YmcaBusinessObject.RefundRequest.GetBATerminatePIAAtRequest(Me.FundID)
                'Added by ganeswar sahoo on july 09 for Gemini Issue
                If Me.IsTerminated = False Then
                    l_decimal_BATermination = l_decimal_BACurrent
                    l_decimal_PIATermination = l_decimal_CurrentPIA
                End If
                Session("CurrentPIA_C19") = l_decimal_CurrentPIA
                Session("TerminationPIA_C19") = l_decimal_PIATermination
                Session("BACurrent_C19") = l_decimal_BACurrent
                Session("BATermination_C19") = l_decimal_BATermination
                'Added by ganeswar sahoo on july 09 for Gemini Issue
                Session("CurrentBAAtRequest_C19") = l_decimal_BACurrentAtRequest
                Me.TextboxBATerminate.Text = DirectCast(Session("CurrentBAAtRequest_C19"), Decimal)
                'Added by ganeswar sahoo on july 09 for Gemini Issue
                Me.TextboxTerminatePIA.Text = FormatCurrency(Me.TerminationPIA)
            End If


        Catch
            Throw
        End Try
    End Function

    Private Function DisplayAccountContribution(ByVal parameter_PlanType As String)

        Dim l_DataTable As DataTable
        Dim l_DataTable_RetirementPlanAcctContribution As DataTable
        Dim l_DataTable_SavingsPlanAcctContribution As DataTable
        Dim l_DataTable_DisplayConsolidatedTotal As DataTable
        Dim NumPercentageFactorofMoneySavings As Decimal
        Dim NumPercentageFactorofMoneyRetirement As Decimal
        Dim displayConsolidatedTotal As New DataTable
        Try
            'Comment code Deleted By Sanjeev on 06/02/2012

            '------ Added to implement WebServiece by pavan ---- Begin  -----------           
            CreateDisplayDatatables(DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable), DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable), Me.NumPercentageFactorofMoneySavings, Me.NumPercentageFactorofMoneyRetirement)

            

            l_DataTable_RetirementPlanAcctContribution = DataTableDisplayRetirementPlan
            l_DataTable_SavingsPlanAcctContribution = DataTableDisplaySavingsPlan
            '------ Added to implement WebServiece by pavan ---- END    -----------

            'To Fix rounding issue
            If CheckboxPartialRetirement.Checked = True Then
                If TextboxPartialRetirement.Text <> String.Empty Then
                    '------ Commented for WebServiece by pavan ---- Begin  -----------
                    l_DataTable_RetirementPlanAcctContribution = objRefundRequest.FixRoundingIssue(l_DataTable_RetirementPlanAcctContribution, Convert.ToDecimal(TextboxPartialRetirement.Text), "Retirement")
                    '------ Commented for WebServiece by pavan ---- END    -----------
                End If
            End If
            If CheckboxPartialSavings.Checked = True Then
                If TextboxPartialSavings.Text <> String.Empty Then
                    '------ Commented for WebServiece by pavan ---- Begin  -----------
                    'Start: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1746:Include partial withdrawals 
                    'Added two parameter for partial withdrawal
                    'Remove two parameter for partial withdrawal 
                    '2013.10.23        Dinesh Kanojia(DK)   BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                    'l_DataTable_SavingsPlanAcctContribution = objRefundRequest.FixRoundingIssue(l_DataTable_SavingsPlanAcctContribution, Convert.ToDecimal(TextboxPartialSavings.Text), "Savings", CheckboxPartialSavings.Checked, TextBoxTerminated.Text.Trim.ToUpper)
                    l_DataTable_SavingsPlanAcctContribution = objRefundRequest.FixRoundingIssue(l_DataTable_SavingsPlanAcctContribution, Convert.ToDecimal(TextboxPartialSavings.Text), "Savings")
                    '------ Commented for WebServiece by pavan ---- END    -----------
                End If
            End If
            'Comment code Deleted By Sanjeev on 06/02/2012

            'START: MMR | 2020.04.24 | YRS-AT-4854 | Get Blended tax rate and apply tax rate on retirement plan account and savings plan account
            'Do not execute for Exclude YMCA Account option selected
            If Me.RefundType <> "PERS" Then
                Me.BlendedTaxRate = Me.GetBlendedTaxRate(l_DataTable_RetirementPlanAcctContribution, l_DataTable_SavingsPlanAcctContribution)

                'Calculating tax as per Blended tax rate and updating in display table for retirement plan
                l_DataTable_RetirementPlanAcctContribution = UpdateDisplayTableTaxValueWithBlendedTaxRate(l_DataTable_RetirementPlanAcctContribution, Me.BlendedTaxRate)

                'Calculating tax as per Blended tax rate and updating in display table for savings plan
                l_DataTable_SavingsPlanAcctContribution = UpdateDisplayTableTaxValueWithBlendedTaxRate(l_DataTable_SavingsPlanAcctContribution, Me.BlendedTaxRate)

                'Updating consolidated display total table after tax re-calculation of both the plan
                displayConsolidatedTotal = Me.DataTableDisplaySavingsPlan.Clone()
                For Each retirementPlanRow As DataRow In l_DataTable_RetirementPlanAcctContribution.Rows
                    displayConsolidatedTotal.ImportRow(retirementPlanRow)
                Next
                For Each savingsPlanRow As DataRow In l_DataTable_SavingsPlanAcctContribution.Rows
                    displayConsolidatedTotal.ImportRow(savingsPlanRow)
                Next
                Me.DataTableDisplayConsolidatedTotal = displayConsolidatedTotal

                'Display calculated Covid taxable and non-taxable amount on UI
                Me.DisplayCovidTaxableAndNonTaxableAmount()
            End If
            'END: MMR | 2020.04.24 | YRS-AT-4854 | Get Blended tax rate and apply tax rate on retirement plan account and savings plan account

            '------ Added to implement WebServiece by pavan ---- Begin  -----------
            Session("DisplayConsolidatedTotal_C19") = DataTableDisplayConsolidatedTotal
            '------ Added to implement WebServiece by pavan ---- END    -----------

            If Not Session("DisplayConsolidatedTotal_C19") Is Nothing Then
                l_DataTable = DirectCast(Session("DisplayConsolidatedTotal_C19"), DataTable)
                Session("InitialDisplayConsolidatedTotal_C19") = l_DataTable
                Me.DatagridConsolidateTotal.DataSource = l_DataTable
                Me.DatagridConsolidateTotal.DataBind()
            End If


            Select Case parameter_PlanType
                Case "Retirement"
                    If Not l_DataTable_RetirementPlanAcctContribution Is Nothing Then
                        'Comment code Deleted By Sanjeev on 06/02/2012

                        '------ Added to implement WebServiece by pavan ---- Begin  -----------
                        BindDataGrid("IsRetirement", l_DataTable_RetirementPlanAcctContribution)
                        '------ Added to implement WebServiece by pavan ---- END    -----------
                        Me.CheckboxRetirementPlan.Checked = True
                        Me.CheckboxRetirementPlan.Enabled = True

                    Else
                        'Me.CheckboxRetirementPlan.Visible = False
                        Me.CheckboxRetirementPlan.Checked = False
                        Me.CheckboxSavingsPlan.Enabled = False
                    End If
                    Me.SetSelectedIndex(Me.DataGridAccContributionRetirement, l_DataTable_RetirementPlanAcctContribution)
                    Session("DisplayRetirementPlanAcctContribution_C19") = l_DataTable_RetirementPlanAcctContribution

                Case "Savings", "Both"
                    If Not l_DataTable_SavingsPlanAcctContribution Is Nothing Then
                        'Comment code Deleted By Sanjeev on 06/02/2012


                        '------ Added to implement WebServiece by pavan ---- Begin  -----------
                        BindDataGrid("IsSavings", l_DataTable_SavingsPlanAcctContribution)
                        '------ Added to implement WebServiece by pavan ---- END    -----------
                        Me.CheckboxSavingsPlan.Checked = True
                        Me.CheckboxSavingsPlan.Enabled = True
                    Else
                        'Me.CheckboxSavingsPlan.Visible = False
                        Me.CheckboxSavingsPlan.Checked = False
                        Me.CheckboxRetirementPlan.Enabled = False
                    End If
                    Me.SetSelectedIndex(Me.DataGridAccContributionSavings, l_DataTable_SavingsPlanAcctContribution)
                    Session("DisplaySavingsPlanAcctContribution_C19") = l_DataTable_SavingsPlanAcctContribution

            End Select

            Me.SetSelectedIndex(Me.DatagridConsolidateTotal, l_DataTable)
            Session("DisplayConsolidatedTotal_C19") = l_DataTable
            ' For display
        Catch
            Throw
        End Try
    End Function

    Private Function DisplayCopyAccountContributionTable(ByVal string_PlanType As String)
        Dim l_DataTable_DisplayRetirementPlanAcctContribution As DataTable
        Dim l_DataTable_DisplaySavingsPlanAcctContribution As DataTable
        Dim l_DataTable_DisplayConsolidatedAcctContributions As DataTable
        Dim l_CalculationDataTable_Display_DisplayRetirementPlan As DataTable
        Dim l_CalculationDataTable_Display_DisplaySavingsPlan As DataTable
        Dim l_CalculationDataTable_Display As DataTable
        Dim l_DataColumn_RetirementPlan As DataColumn
        Dim l_DataRow_RetirementPlan As DataRow
        Dim l_DataColumn_SavingsPlan As DataColumn
        Dim l_DataRow_SavingsPlan As DataRow

        Try

            'Get the Calculated Data Table for Retirement Plan

            Select Case string_PlanType
                Case "Retirement"
                    If Not Session("DisplayRetirementPlanAcctContribution_C19") Is Nothing Then
                        l_DataTable_DisplayRetirementPlanAcctContribution = objRefundRequest.DataTableDisplayRetirementPlan
                        If l_DataTable_DisplayRetirementPlanAcctContribution Is Nothing Then
                            l_DataTable_DisplayRetirementPlanAcctContribution = DirectCast(Session("DisplayRetirementPlanAcctContribution_C19"), DataTable)
                        End If
                    End If
                    If Not l_DataTable_DisplayRetirementPlanAcctContribution Is Nothing Then

                        l_CalculationDataTable_Display_DisplayRetirementPlan = l_DataTable_DisplayRetirementPlanAcctContribution.Clone

                        ' Add a column Into the Table to allow selected.
                        l_DataColumn_RetirementPlan = New DataColumn("Selected")
                        l_DataColumn_RetirementPlan.DataType = System.Type.GetType("System.Boolean")
                        l_DataColumn_RetirementPlan.AllowDBNull = True

                        l_CalculationDataTable_Display_DisplayRetirementPlan.Columns.Add(l_DataColumn_RetirementPlan)

                        'Copy all Values into Calculation Table 

                        For Each l_DataRow_RetirementPlan In l_DataTable_DisplayRetirementPlanAcctContribution.Rows

                            If Not (l_DataRow_RetirementPlan("AccountType").GetType.ToString = "System.DBNull") Then

                                If Not (CType(l_DataRow_RetirementPlan("AccountType"), String) = "Total") Then
                                    l_CalculationDataTable_Display_DisplayRetirementPlan.ImportRow(l_DataRow_RetirementPlan)
                                End If
                            End If
                        Next
                    End If
                    Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = l_CalculationDataTable_Display_DisplayRetirementPlan
                Case "Savings"
                    'Get the Calculated Data Table for Savings Plan

                    If Not Session("DisplaySavingsPlanAcctContribution_C19") Is Nothing Then

                        l_DataTable_DisplaySavingsPlanAcctContribution = objRefundRequest.DataTableDisplaySavingsPlan

                        If l_DataTable_DisplaySavingsPlanAcctContribution Is Nothing Then
                            l_DataTable_DisplaySavingsPlanAcctContribution = DirectCast(Session("DisplaySavingsPlanAcctContribution_C19"), DataTable)
                        End If
                        'l_DataTable_DisplaySavingsPlanAcctContribution = CType(Session("DisplaySavingsPlanAcctContribution"), DataTable)


                    End If
                    If Not l_DataTable_DisplaySavingsPlanAcctContribution Is Nothing Then

                        l_CalculationDataTable_Display_DisplaySavingsPlan = l_DataTable_DisplaySavingsPlanAcctContribution.Clone

                        ' Add a column Into the Table to allow selected.
                        l_DataColumn_SavingsPlan = New DataColumn("Selected")
                        l_DataColumn_SavingsPlan.DataType = System.Type.GetType("System.Boolean")
                        l_DataColumn_SavingsPlan.AllowDBNull = True

                        l_CalculationDataTable_Display_DisplaySavingsPlan.Columns.Add(l_DataColumn_SavingsPlan)

                        'Copy all Values into Calculation Table 

                        For Each l_DataRow_SavingsPlan In l_DataTable_DisplaySavingsPlanAcctContribution.Rows

                            If Not (l_DataRow_SavingsPlan("AccountType").GetType.ToString = "System.DBNull") Then

                                If Not (CType(l_DataRow_SavingsPlan("AccountType"), String) = "Total") Then
                                    l_CalculationDataTable_Display_DisplaySavingsPlan.ImportRow(l_DataRow_SavingsPlan)
                                End If
                            End If
                        Next
                    End If
                    Session("Calculated_DisplaySavingsPlanAcctContribution_C19") = l_CalculationDataTable_Display_DisplaySavingsPlan
            End Select



            'now we will create a combined data table for Retirement and Savings Plan to be shown in the consolidated data grid
            If Not l_CalculationDataTable_Display_DisplayRetirementPlan Is Nothing Then
                l_DataTable_DisplayConsolidatedAcctContributions = l_CalculationDataTable_Display_DisplayRetirementPlan.Clone
            ElseIf Not l_CalculationDataTable_Display_DisplaySavingsPlan Is Nothing Then
                l_DataTable_DisplayConsolidatedAcctContributions = l_CalculationDataTable_Display_DisplaySavingsPlan.Clone
            End If

            ' If Not Session("OnlySavingsPlan") = True Or Session("BothPlans") = True Then
            If Session("OnlyRetirementPlan_C19") = True Or Session("BothPlans_C19") = True Then
                If Not l_CalculationDataTable_Display_DisplayRetirementPlan Is Nothing Then
                    For Each drow As DataRow In l_CalculationDataTable_Display_DisplayRetirementPlan.Rows
                        l_DataTable_DisplayConsolidatedAcctContributions.ImportRow(drow)
                    Next
                End If
            End If
            'If Not Session("OnlyRetirementPlan_C19") = True Or Session("BothPlans") = True Then
            If Session("OnlySavingsPlan_C19") = True Or Session("BothPlans_C19") = True Then
                If Not l_DataTable_DisplaySavingsPlanAcctContribution Is Nothing Then
                    For Each drow As DataRow In l_CalculationDataTable_Display_DisplaySavingsPlan.Rows
                        l_DataTable_DisplayConsolidatedAcctContributions.ImportRow(drow)
                    Next
                End If
            End If



            Session("CalculatedDataTableDisplay_C19") = l_DataTable_DisplayConsolidatedAcctContributions
        Catch
            Throw
        End Try
    End Function

    Private Function CopyAccountContributionTable()
        Dim l_DataTable_RetirementPlanAcctContribution As DataTable
        Dim l_DataTable_SavingsPlanAcctContribution As DataTable
        Dim l_DataTable_ConsolidatedAcctContributions As DataTable
        Dim l_CalculationDataTable_RetirementPlan As DataTable
        Dim l_CalculationDataTable_SavingsPlan As DataTable
        Dim l_CalculationDataTable As DataTable
        Dim l_DataColumn_RetirementPlan As DataColumn
        Dim l_DataRow_RetirementPlan As DataRow
        Dim l_DataColumn_SavingsPlan As DataColumn
        Dim l_DataRow_SavingsPlan As DataRow

        Try
            'Get the Calculated Data Table for Retirement Plan
            If Not Session("RetirementPlanAcctContribution_C19") Is Nothing Then
                l_DataTable_RetirementPlanAcctContribution = DirectCast(Session("RetirementPlanAcctContribution_C19"), DataTable)
            End If
            If Not l_DataTable_RetirementPlanAcctContribution Is Nothing Then

                l_CalculationDataTable_RetirementPlan = l_DataTable_RetirementPlanAcctContribution.Clone

                ' Add a column Into the Table to allow selected.
                l_DataColumn_RetirementPlan = New DataColumn("Selected")
                l_DataColumn_RetirementPlan.DataType = System.Type.GetType("System.Boolean")
                l_DataColumn_RetirementPlan.AllowDBNull = True

                l_CalculationDataTable_RetirementPlan.Columns.Add(l_DataColumn_RetirementPlan)

                'Copy all Values into Calculation Table 

                For Each l_DataRow_RetirementPlan In l_DataTable_RetirementPlanAcctContribution.Rows

                    If Not (l_DataRow_RetirementPlan("AccountType").GetType.ToString = "System.DBNull") Then

                        If Not (CType(l_DataRow_RetirementPlan("AccountType"), String) = "Total") Then
                            l_CalculationDataTable_RetirementPlan.ImportRow(l_DataRow_RetirementPlan)
                        End If
                    End If
                Next
            End If
            'Get the Calculated Data Table for Savings Plan
            If Not Session("SavingsPlanAcctContribution_C19") Is Nothing Then
                l_DataTable_SavingsPlanAcctContribution = DirectCast(Session("SavingsPlanAcctContribution_C19"), DataTable)
            End If
            If Not l_DataTable_SavingsPlanAcctContribution Is Nothing Then

                l_CalculationDataTable_SavingsPlan = l_DataTable_SavingsPlanAcctContribution.Clone

                ' Add a column Into the Table to allow selected.
                l_DataColumn_SavingsPlan = New DataColumn("Selected")
                l_DataColumn_SavingsPlan.DataType = System.Type.GetType("System.Boolean")
                l_DataColumn_SavingsPlan.AllowDBNull = True

                l_CalculationDataTable_SavingsPlan.Columns.Add(l_DataColumn_SavingsPlan)

                'Copy all Values into Calculation Table 

                For Each l_DataRow_SavingsPlan In l_DataTable_SavingsPlanAcctContribution.Rows

                    If Not (l_DataRow_SavingsPlan("AccountType").GetType.ToString = "System.DBNull") Then

                        If Not (CType(l_DataRow_SavingsPlan("AccountType"), String) = "Total") Then
                            l_CalculationDataTable_SavingsPlan.ImportRow(l_DataRow_SavingsPlan)
                        End If
                    End If
                Next
            End If
            'now we will create a combined data table for Retirement and Savings Plan to be shown in the consolidated data grid
            If Not l_CalculationDataTable_RetirementPlan Is Nothing Then
                l_DataTable_ConsolidatedAcctContributions = l_CalculationDataTable_RetirementPlan.Clone
            ElseIf Not l_CalculationDataTable_SavingsPlan Is Nothing Then
                l_DataTable_ConsolidatedAcctContributions = l_CalculationDataTable_SavingsPlan.Clone
            End If

            ' If Not Session("OnlySavingsPlan") = True Or Session("BothPlans") = True Then
            If Session("OnlyRetirementPlan_C19") = True Or Session("BothPlans_C19") = True Then
                If Not l_CalculationDataTable_RetirementPlan Is Nothing Then
                    For Each drow As DataRow In l_CalculationDataTable_RetirementPlan.Rows
                        l_DataTable_ConsolidatedAcctContributions.ImportRow(drow)
                    Next
                End If
            End If
            'If Not Session("OnlyRetirementPlan_C19") = True Or Session("BothPlans") = True Then
            If Session("OnlySavingsPlan_C19") = True Or Session("BothPlans_C19") = True Then
                If Not l_DataTable_SavingsPlanAcctContribution Is Nothing Then
                    For Each drow As DataRow In l_CalculationDataTable_SavingsPlan.Rows
                        l_DataTable_ConsolidatedAcctContributions.ImportRow(drow)
                    Next
                End If
            End If

            Session("Calculated_RetirementPlanAcctContribution_C19") = l_CalculationDataTable_RetirementPlan
            Session("Calculated_SavingsPlanAcctContribution_C19") = l_CalculationDataTable_SavingsPlan
            Session("CalculatedDataTable_C19") = l_DataTable_ConsolidatedAcctContributions
        Catch
            Throw
        End Try
    End Function


    Private Function LoadAccountContribution()
        Dim l_DataTable As DataTable
        Dim l_DataTable_RetirementPlanAcctContribution As DataTable
        Dim l_DataTable_SavingsPlanAcctContribution As DataTable
        Try
            If Not Session("AccountContribution_C19") Is Nothing Then
                l_DataTable = DirectCast(Session("AccountContribution_C19"), DataTable)
                Session("InitialAccountContribution_C19") = l_DataTable
                Me.DataGridAccountContribution.DataSource = l_DataTable
                Me.DataGridAccountContribution.DataBind()
            End If


            If Not Session("RetirementPlanAcctContribution_C19") Is Nothing Then
                l_DataTable_RetirementPlanAcctContribution = DirectCast(Session("RetirementPlanAcctContribution_C19"), DataTable)
                'If l_DataTable_RetirementPlanAcctContribution.Rows.Count > 0 Then
                Me.DataGridAccContributionRetirement.DataSource = l_DataTable_RetirementPlanAcctContribution
                Me.DataGridAccContributionRetirement.DataBind()
                'Me.CheckboxRetirementPlan.Visible = True
                Me.CheckboxRetirementPlan.Checked = True
                Me.CheckboxRetirementPlan.Enabled = True

            Else
                'Me.CheckboxRetirementPlan.Visible = False
                Me.CheckboxRetirementPlan.Checked = False
                Me.CheckboxSavingsPlan.Enabled = False
            End If
            If Not Session("SavingsPlanAcctContribution_C19") Is Nothing Then
                l_DataTable_SavingsPlanAcctContribution = DirectCast(Session("SavingsPlanAcctContribution_C19"), DataTable)
                'If l_DataTable_SavingsPlanAcctContribution.Rows.Count > 0 Then
                Me.DataGridAccContributionSavings.DataSource = l_DataTable_SavingsPlanAcctContribution
                Me.DataGridAccContributionSavings.DataBind()
                'Me.CheckboxSavingsPlan.Visible = True
                Me.CheckboxSavingsPlan.Checked = True
                Me.CheckboxSavingsPlan.Enabled = True
            Else
                'Me.CheckboxSavingsPlan.Visible = False
                Me.CheckboxSavingsPlan.Checked = False
                Me.CheckboxRetirementPlan.Enabled = False
            End If
            If Not Me.RefundType = "PART" Then
                Me.SetSelectedIndex(Me.DataGridAccountContribution, l_DataTable)
                Me.SetSelectedIndex(Me.DataGridAccContributionRetirement, l_DataTable_RetirementPlanAcctContribution)
                Me.SetSelectedIndex(Me.DataGridAccContributionSavings, l_DataTable_SavingsPlanAcctContribution)
            End If

        Catch
            Throw
        End Try


    End Function

    Private Function SetSelectedIndex(ByVal parameterDataGrid As DataGrid, ByVal parameterDataTable As DataTable)
        Dim cnt As Integer
        Try
            If parameterDataTable Is Nothing Then
                parameterDataGrid.SelectedIndex = -1
            Else
                'Added By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009
                For cnt = 0 To parameterDataTable.Rows.Count - 1
                    If parameterDataTable.Rows(cnt)("AccountType").ToString = "Total" Then
                        parameterDataGrid.SelectedIndex = cnt
                    ElseIf parameterDataTable.Rows(cnt)("AccountType").ToString = "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)" Then
                        parameterDataGrid.Items(cnt).BackColor = System.Drawing.Color.FromArgb(201, 219, 237)
                    End If
                Next
                'Added By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009
            End If

        Catch
            Throw
        End Try
    End Function

    Private Function LoadRefundConfiguration()

        Dim l_DataTable As DataTable
        Dim l_DataTable_PIAMinToRetire As DataTable
        'BA Account changes Phase V by Dilip
        Dim l_DataTable_BAMaxLimit As DataTable
        'BA Account changes Phase V by Dilip

        Dim l_DataRow As DataRow
        'Shubhrata Jan 9th 2007 YREN 3019
        Dim l_boolean_MaxPIAAmt As Boolean = False
        Dim l_boolean_MinPIAToRetire As Boolean = False
        'Shubhrata Jan 9th 2007 YREN 3019
        Dim l_stringCategory As String = "Refund"
        Try

            'BA Account changes Phase V by Dilip
            If Me.PersonAge < 55 Then
                l_DataTable_BAMaxLimit = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("BA_MAX_LIMIT_UNDER_55")
                If Not l_DataTable_BAMaxLimit Is Nothing Then
                    For Each l_DataRow In l_DataTable_BAMaxLimit.Rows
                        If (CType(l_DataRow("Key"), String).Trim = "BA_MAX_LIMIT_UNDER_55") Then
                            Me.BAMaxLimit = CType(l_DataRow("Value"), Decimal)
                        End If
                        Exit For
                    Next
                End If
            ElseIf Me.PersonAge >= 55 Then
                l_DataTable_BAMaxLimit = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("BA_MAX_LIMIT_55_ABOVE")
                If Not l_DataTable_BAMaxLimit Is Nothing Then
                    For Each l_DataRow In l_DataTable_BAMaxLimit.Rows
                        If (CType(l_DataRow("Key"), String).Trim = "BA_MAX_LIMIT_55_ABOVE") Then
                            Me.BAMaxLimit = CType(l_DataRow("Value"), Decimal)
                        End If
                        Exit For
                    Next
                End If
            End If

            'Comment code Deleted By Sanjeev on 06/02/2012

            'Changed the proc used to get the Refund configuration -so tht it can be universally used
            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetConfigurationCategoryWise(l_stringCategory)
            'BY Aparna 18/04/2007   

            'Shubhrata JAn 22nd 2007 -PIA_MINIMUM_TO_RETIRE is being mapped to RETIRE Key hence we will 
            'fetch it using Different SP
            l_DataTable_PIAMinToRetire = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("PIA_MINIMUM_TO_RETIRE")
            'Shubhrata
            If Not l_DataTable Is Nothing Then

                For Each l_DataRow In l_DataTable.Rows

                    If (CType(l_DataRow("Key"), String).Trim = "REFUND_EXPIRE_DAYS") Then
                        Me.RefundExpiryDate = CType(l_DataRow("Value"), Integer)
                    End If

                    If (CType(l_DataRow("Key"), String).Trim = "MIN_DISTRIBUTION_AGE") Then
                        Me.MinimumDistributedAge = CType(l_DataRow("Value"), Decimal)
                    End If

                    If (CType(l_DataRow("Key"), String).Trim = "REFUND_MAX_PIA") Then
                        l_boolean_MaxPIAAmt = True
                        Me.MaximumPIAAmount = CType(l_DataRow("Value"), Decimal)
                    End If

                    'Added to check the condition to allow partial withdrawal
                    If (CType(l_DataRow("Key"), String).Trim = "ALLOW_PARTIAL_WITHDRAWAL_REQUEST") Then
                        Me.AllowPartialRefund = CType(l_DataRow("Value"), Boolean)
                    End If

                    'Added for validation the partial withdrawal requested amount -Amit 01 Sep 2009
                    If (CType(l_DataRow("Key"), String).Trim = "MIN_PARTIAL_WITHDRAWAL_LIMIT") Then
                        Me.PartialMinLimit = CType(l_DataRow("Value"), Decimal)
                    End If
                    'Added for validation the partial withdrawal requested amount -Amit 01 Sep 2009
                    'New Modification for Market Based Withdrawal Amit Nigam
                    'Added for setting the value of threshold amount for the Market Based withdrawal -Amit 01 Oct 2009
                    If (CType(l_DataRow("Key"), String).Trim = "MARKET_BASED_WITHDRAWAL_THRESHOLD") Then
                        Me.MarketBasedThreshold = CType(l_DataRow("Value"), Decimal)
                    End If
                    'Added for setting the value of threshold amount for the Market Based withdrawal -Amit 01 Oct 2009

                    'Added for setting the value of Percentage for the Market Based withdrawal -Amit 01 Oct 2009
                    If (CType(l_DataRow("Key"), String).Trim = "MARKET_BASED_WITHDRAWAL_PERCENTAGE") Then
                        Me.MarketBasedFirstInstPercentage = CType(l_DataRow("Value"), Decimal)
                    End If
                    'Added for setting the value of Percentage for the Market Based withdrawal -Amit 01 Oct 2009

                    'Added to check the condition to allow Market Based withdrawal -Amit 01 Oct 2009
                    If (CType(l_DataRow("Key"), String).Trim = "ALLOW_MARKET_BASED_WITHDRAWAL") Then
                        Me.AllowMarketBased = CType(l_DataRow("Value"), Boolean)
                        'PP
                        'objRefundRequest.AllowMarketBased = Me.AllowMarketBased
                    End If
                    'Added for setting the value of Second Installment Percentage for the Market Based withdrawal -Amit 23 Nov 2009
                    If (CType(l_DataRow("Key"), String).Trim = "MARKET_BASED_DEFERREDPAYMENT_PERCENTAGE") Then
                        Me.MarketBasedDefferedPmtPercentage = CType(l_DataRow("Value"), Decimal)
                    End If
                    'Added for setting the value of Second Installment Percentage for the Market Based withdrawal -Amit 23 Nov 2009
                    'Added to check the condition to allow Market Based withdrawal -Amit 01 Oct 2009
                    'New Modification for Market Based Withdrawal Amit Nigam

                Next
            End If
            'Shubhrata jan 9th 2006 to replace hard coding of 5000 as stated in Ragesh's mail dated Jan8th(YREN - 3019)

            If Not l_DataTable_PIAMinToRetire Is Nothing Then
                For Each l_DataRow In l_DataTable_PIAMinToRetire.Rows
                    If (CType(l_DataRow("Key"), String).Trim = "PIA_MINIMUM_TO_RETIRE") Then
                        l_boolean_MinPIAToRetire = True
                        Me.MinimumPIAToRetire = CType(l_DataRow("Value"), Decimal)
                    End If
                    Exit For
                Next
            End If

            'Shubhrata jan 9th 2006 - if values n0t found in metatable set default values - YREN - 3019
            If l_boolean_MinPIAToRetire = False Then
                Me.MinimumPIAToRetire = 5000
            Else
                Me.MinimumPIAToRetire = Me.MinimumPIAToRetire
            End If
            If l_boolean_MaxPIAAmt = False Then
                Me.MaximumPIAAmount = 15000
            Else
                Me.MaximumPIAAmount = Me.MaximumPIAAmount
            End If
            'Shubhrata jan 9th 2006 - if values n0t found in metatable set default values - YREN - 3019
            '' This call to get the Account Break Down.. 

            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetAccountBreakDown()

            'Comment code Deleted By Sanjeev on 06/02/2012
            Session("AccountBreakDown_C19") = l_DataTable

        Catch
            Throw
        End Try
    End Function
    Private Function SetCheckBoxes()
        Try
            If Me.RefundType.Trim = "SPEC" Then
                Me.CheckboxSpecial.Visible = True
                Me.CheckboxSpecial.Checked = True

                '' Disable Other Check Boxes.
                Me.CheckboxRegular.Visible = False
                Me.CheckboxDisability.Visible = False

                Me.ButtonSave.Visible = True
                Me.ButtonSave.Enabled = True

            ElseIf Me.RefundType.Trim = "DISAB" Then

                Me.CheckboxSpecial.Visible = False
                Me.CheckboxSpecial.Checked = False
                Me.CheckboxRegular.Visible = False
                Me.CheckboxDisability.Visible = True
                Me.CheckboxDisability.Checked = True

                Me.ButtonSave.Visible = True
                Me.ButtonSave.Enabled = True
            Else
                Me.CheckboxSpecial.Visible = False
                Me.CheckboxRegular.Visible = True
                Me.CheckboxDisability.Visible = False
                Me.CheckboxSpecial.Checked = False

                'Me.ButtonSave.Visible = False
                'Me.ButtonSave.Enabled = False
            End If

            'Comment code Deleted By Sanjeev on 06/02/2012

            If Me.IsTerminated Then
                Me.CheckboxRegular.Enabled = True
                Me.CheckboxVoluntryAccounts.Enabled = True
                Me.CheckboxHardship.Enabled = False
            Else
                Me.CheckboxRegular.Enabled = False
                Me.CheckboxVoluntryAccounts.Enabled = True
                Me.CheckboxHardship.Enabled = True
            End If

            If Me.PersonAge >= 59.06 Then
                Me.CheckboxHardship.Enabled = False
            Else
                Me.CheckboxHardship.Enabled = True
            End If

            If Me.CheckboxRegular.Checked = True Then
                Me.CheckboxVoluntryAccounts.Enabled = False
                Me.CheckboxHardship.Enabled = False

                Me.CheckboxVoluntryAccounts.Checked = False
                Me.CheckboxHardship.Checked = False
            End If


            ''' Refresh the check Boxes.. 

            If Me.RefundType = "SPEC" Then
                Me.CheckboxRegular.Checked = False
                Me.CheckboxRegular.Visible = False

                Me.CheckboxVoluntryAccounts.Checked = False
                Me.CheckboxVoluntryAccounts.Enabled = False

                Me.CheckboxHardship.Checked = False
                Me.CheckboxHardship.Enabled = False
            End If

            If Me.RefundType.Trim = "DISAB" Then
                Me.CheckboxRegular.Checked = False
                Me.CheckboxRegular.Visible = False

                Me.CheckboxVoluntryAccounts.Checked = False
                Me.CheckboxVoluntryAccounts.Enabled = False

                Me.CheckboxHardship.Checked = False
                Me.CheckboxHardship.Enabled = False
            End If


            ''Regular CheckBox..

            If Me.IsTypeChoosen And Me.CheckboxRegular.Checked = True Then
                Me.CheckboxRegular.Enabled = True
            Else
                Me.CheckboxRegular.Enabled = False
            End If

            If Me.IsTerminated = False Then
                Me.CheckboxRegular.Enabled = False
            End If

            '' Refresh Voluntry Check Box.. 

            If Me.IsTerminated And Me.CheckboxVoluntryAccounts.Checked = False Then
                Me.CheckboxVoluntryAccounts.Enabled = False
            Else
                Me.CheckboxVoluntryAccounts.Enabled = True
            End If

            If Me.VoluntryAmount < 0.01 And Me.CheckboxVoluntryAccounts.Checked = False Then
                Me.CheckboxVoluntryAccounts.Enabled = False
            End If
            '****************************************************
            '' Refresh the Hardship CheckBox..
            'Start - Modified by Shubhrata - Combine the conditions together
            '*****************************************************
            If Me.IsTypeChoosen And Me.CheckboxHardship.Checked = False Then
                Me.CheckboxHardship.Enabled = False
            Else
                Me.CheckboxHardship.Enabled = True
            End If
            If Me.IsTerminated = False And Me.PersonAge < 59.06 And Me.TDAccountAmount > 0.01 Then
                Me.CheckboxHardship.Enabled = True
            Else
                Me.CheckboxHardship.Enabled = False
            End If

            If Me.CheckboxHardship.Checked = True Then
                Me.FederalTaxRate = 0.0
                Me.TextboxTaxRate.Text = 0.0
            Else
                'Start - Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code to remove harcoded tax rate and assigned configurable tax rate
                'Me.FederalTaxRate = 20.0 
                'Me.TextboxTaxRate.Text = 20.0 
                'End - Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code to remove harcoded tax rate and assigned configurable tax rate
                Me.TextboxTaxRate.Text = Me.FederalTaxRate
            End If
            '****************************************************
            'End - Modified by Shubhrata - Combine the conditions together
            '*****************************************************
            '' Refresh Speacial CheckBox.
            If Me.RefundType = "SPEC" Then

                If Me.CheckboxSpecial.Checked = True Then

                    If Me.RefundType = "SPEC" Then
                        Me.SpecialRefund("BothPlans")
                    End If

                    If Me.RefundType = "DISAB" Then
                        Me.DisabilityRefund("BothPlans")
                    End If

                Else
                    Me.RefundType = ""
                    Me.IsTypeChoosen = False
                End If

            End If
            '*************************************************
            'Person with QD status can take only Reg and Special Refund
            '*************************************************
            If Me.SessionStatusType.Trim.ToUpper = "QD" Then
                Me.CheckboxRegular.Enabled = True
                Me.CheckboxSpecial.Enabled = True
                Me.CheckboxVoluntryAccounts.Enabled = False
                Me.CheckboxHardship.Enabled = False
                Me.CheckboxDisability.Enabled = False
                CheckboxExcludeYMCA.Enabled = False
            End If
            '' refresh Diability CheckBox.


        Catch
            Throw
        End Try
    End Function

    Private Function IsBasicAccount(ByVal parameterDataRow As DataRow) As Boolean

        Try

            If parameterDataRow("IsBasicAccount").GetType.ToString() = "System.DBNull" Then Return False

            If CType(parameterDataRow("IsBasicAccount"), Boolean) Then
                Return True
            Else
                Return False
            End If

        Catch
            Throw
        End Try

    End Function

    Private Function LoadCalculatedTable1(ByVal parameterCalledBy As String)

        Dim l_CalculatedDataTable As DataTable

        Try
            If parameterCalledBy = "IsRetirement" Then
                If Not Session("Calculated_RetirementPlanAcctContribution_C19") Is Nothing Then
                    l_CalculatedDataTable = DirectCast(Session("Calculated_RetirementPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsSavings" Then
                If Not Session("Calculated_SavingsPlanAcctContribution_C19") Is Nothing Then
                    l_CalculatedDataTable = DirectCast(Session("Calculated_SavingsPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsConsolidated" Then
                If Not Session("CalculatedDataTable_C19") Is Nothing Then
                    l_CalculatedDataTable = DirectCast(Session("CalculatedDataTable_C19"), DataTable)
                End If
            End If

            If Me.RefundType = "VOL" Then
                If parameterCalledBy = "IsRetirement" Then
                    '--STart Manthan Rajguru 2015.09.24 YRS-AT-2550: Custom Control reference given for DatagridcheckBox
                    datagridCheckBox = New CustomControls.CheckBoxColumn(False, "Selected")
                Else
                    If Me.CheckboxVoluntryAccounts.Checked = True Then
                        datagridCheckBox = New CustomControls.CheckBoxColumn(True, "Selected")
                    Else
                        datagridCheckBox = New CustomControls.CheckBoxColumn(True, "Selected")
                    End If
                End If
            Else
                datagridCheckBox = New CustomControls.CheckBoxColumn(False, "Selected")
                '--End Manthan Rajguru 2015.09.24 YRS-AT-2550: Custom Control reference given for DatagridcheckBox
            End If


            'Comment code Deleted By Sanjeev on 06/02/2012

            datagridCheckBox.DataField = "Selected"
            datagridCheckBox.AutoPostBack = True
            If parameterCalledBy = "IsRetirement" Then
                Me.DataGridAccContributionRetirement.Columns.Clear()
                Me.DataGridAccContributionRetirement.Columns.Add(datagridCheckBox)
                Me.DataGridAccContributionRetirement.DataSource = l_CalculatedDataTable
                Me.DataGridAccContributionRetirement.DataBind()
            ElseIf parameterCalledBy = "IsSavings" Then
                Me.DataGridAccContributionSavings.Columns.Clear()
                Me.DataGridAccContributionSavings.Columns.Add(datagridCheckBox)
                Me.DataGridAccContributionSavings.DataSource = l_CalculatedDataTable
                Me.DataGridAccContributionSavings.DataBind()
            ElseIf parameterCalledBy = "IsConsolidated" Then
                Me.DataGridAccountContribution.Columns.Clear()
                Me.DataGridAccountContribution.Columns.Add(datagridCheckBox)
                Me.DataGridAccountContribution.DataSource = l_CalculatedDataTable
                Me.DataGridAccountContribution.DataBind()
            End If



        Catch
            Throw
        End Try

    End Function

    Private Function LoadCalculatedTable(ByVal parameterCalledBy As String)
        Dim l_CalculatedDataTable As DataTable

        Try
            'Added By Parveen For Market Based Withdrawal on 16-Nov 2009             

            'Added By Parveen For Market Based Withdrawal on 16-Nov 2009 
            Select Case parameterCalledBy
                Case "IsRetirement"
                    RemoveMarketBasedRecords(parameterCalledBy, CType(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable))
                    If Not Session("Calculated_DisplayRetirementPlanAcctContribution_C19") Is Nothing Then
                        l_CalculatedDataTable = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable)
                    End If
                    If Me.RefundType = "VOL" And Me.CheckboxExcludeYMCA.Checked = False Or _
                        (Me.CheckboxVoluntryAccounts.Checked = True And Me.RefundType = "PART") Then

                        If Me.CheckboxVoluntryAccounts.Checked = True Then
                            '--STart Manthan Rajguru 2015.09.24 YRS-AT-2550: Custom Control reference given for DatagridcheckBox
                            datagridCheckBox = New CustomControls.CheckBoxColumn(True, "Selected")
                        Else
                            datagridCheckBox = New CustomControls.CheckBoxColumn(False, "Selected")
                        End If

                        'Comment code Deleted By Sanjeev on 06/02/2012
                    Else
                        datagridCheckBox = New CustomControls.CheckBoxColumn(False, "Selected")
                        '--End Manthan Rajguru 2015.09.24 YRS-AT-2550: Custom Control reference given for DatagridcheckBox
                    End If
                    datagridCheckBox.DataField = "Selected"
                    datagridCheckBox.AutoPostBack = True
                    Me.DataGridAccContributionRetirement.Columns.Clear()
                    Me.DataGridAccContributionRetirement.Columns.Add(datagridCheckBox)
                    Me.DataGridAccContributionRetirement.DataSource = l_CalculatedDataTable
                    Me.DataGridAccContributionRetirement.DataBind()

                Case "IsSavings"
                    RemoveMarketBasedRecords(parameterCalledBy, CType(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable))
                    If Not Session("Calculated_DisplaySavingsPlanAcctContribution_C19") Is Nothing Then
                        l_CalculatedDataTable = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable)
                    End If
                    'If Me.RefundType = "VOL" Or Me.RefundType = "SPEC" Or Me.RefundType = "DISAB" Or Me.RefundType = "REG" Or Me.RefundType = "PERS" _
                    '                     Or (Me.RefundType = "PART" And Me.CheckboxPartialSavings.Checked = False) Then
                    If Me.RefundType = "VOL" Or Me.RefundType = "DISAB" Or Me.RefundType = "REG" Or Me.RefundType = "PERS" _
                      Or (Me.RefundType = "PART" And Me.CheckboxPartialSavings.Checked = False) Then

                        If Me.CheckboxSavingsVoluntary.Checked = True And Me.CompulsorySavings = False Then
                            '--STart Manthan Rajguru 2015.09.24 YRS-AT-2550: Custom Control reference given for DatagridcheckBox
                            datagridCheckBox_Savings = New CustomControls.CheckBoxColumn(True, "Selected")
                        ElseIf Me.CompulsorySavings = True And Me.CheckboxSavingsVoluntary.Checked = True Then
                            datagridCheckBox_Savings = New CustomControls.CheckBoxColumn(False, "Selected")
                        Else
                            datagridCheckBox_Savings = New CustomControls.CheckBoxColumn(False, "Selected")
                        End If
                    Else
                        datagridCheckBox_Savings = New CustomControls.CheckBoxColumn(False, "Selected")
                        '--End Manthan Rajguru 2015.09.24 YRS-AT-2550: Custom Control reference given for DatagridcheckBox
                    End If
                    datagridCheckBox_Savings.DataField = "Selected"
                    datagridCheckBox_Savings.AutoPostBack = True

                    Me.DataGridAccContributionSavings.Columns.Clear()
                    Me.DataGridAccContributionSavings.Columns.Add(datagridCheckBox_Savings)
                    Me.DataGridAccContributionSavings.DataSource = l_CalculatedDataTable
                    Me.DataGridAccContributionSavings.DataBind()

            End Select


        Catch
            Throw
        End Try

    End Function


    Private Function MakeValuntryCalculationTable()

        Dim l_CalculatedDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_BooleanFlag As Boolean = False
        Dim l_Counter As Integer
        Dim l_CheckBox As CheckBox

        Try


            l_CalculatedDataTable = Session("Calculated_DisplaySavingsPlanAcctContribution_C19")


            If l_CalculatedDataTable Is Nothing Then Return 0

            l_Counter = 0

            For Each l_DataGridItem As DataGridItem In Me.DataGridAccContributionSavings.Items
                l_CheckBox = l_DataGridItem.FindControl("Selected")
                If l_CalculatedDataTable.Rows.Count > l_Counter Then
                    l_DataRow = l_CalculatedDataTable.Rows(l_Counter)

                    If Not l_DataRow Is Nothing Then

                        If (l_DataRow("Selected").GetType.ToString = "System.DBNull") Then
                            If Not l_CheckBox Is Nothing Then
                                If l_CheckBox.Checked = True Then
                                    l_DataRow("Selected") = "True"
                                Else
                                    l_DataRow("Selected") = "False"
                                End If
                            End If

                        End If
                    End If
                End If
                l_Counter += 1
            Next
        Catch
            Throw
        End Try

    End Function

    Private Function MakeValuntryCalculationTable(ByVal parameterIndex As Integer, ByVal parameterIsSelected As Boolean)

        'Comment code Deleted By Sanjeev on 06/02/2012
        Dim l_CalculatedDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_BooleanFlag As Boolean = False
        Dim l_Counter As Integer
        Dim l_CheckBox As CheckBox

        Try

            'Comment code Deleted By Sanjeev on 06/02/2012
            l_CalculatedDataTable = Session("CalculatedDataTable_C19")

            If parameterIndex < 0 Then Return 0

            If l_CalculatedDataTable Is Nothing Then Return 0

            If l_CalculatedDataTable.Rows.Count > parameterIndex Then

                l_DataRow = l_CalculatedDataTable.Rows(parameterIndex)

                If Not (l_DataRow("AccountType").GetType.ToString() = "System.DBNull") Then

                    If CType(l_DataRow("AccountType"), String).ToString.Trim = "Total" Or CType(l_DataRow("AccountType"), String).ToString.Trim = String.Empty Then Exit Function

                    If Not l_DataRow Is Nothing Then
                        l_DataRow("Selected") = parameterIsSelected
                    End If

                End If


            End If

        Catch
            Throw
        End Try

    End Function

    Private Function MakeValuntryCalculationTableForDisplay(ByVal parameterIndex As Integer, ByVal parameterIsSelected As Boolean, ByVal parameterCalledBy As String, Optional ByVal parameterSelectAccounts As Boolean = False)


        Dim l_CalculatedDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_BooleanFlag As Boolean = False
        Dim l_Counter As Integer
        Dim l_CheckBox As CheckBox

        Try

            Select Case parameterCalledBy

                Case "IsRetirement"
                    l_CalculatedDataTable = Session("Calculated_DisplayRetirementPlanAcctContribution_C19")
                    If parameterIndex < 0 Then Return 0

                    If l_CalculatedDataTable Is Nothing Then Return 0

                    If l_CalculatedDataTable.Rows.Count > parameterIndex Then

                        l_DataRow = l_CalculatedDataTable.Rows(parameterIndex)

                        If Not (l_DataRow("AccountType").GetType.ToString() = "System.DBNull") Then

                            If CType(l_DataRow("AccountType"), String).ToString.Trim = "Total" Or CType(l_DataRow("AccountType"), String).ToString.Trim = String.Empty Then Exit Function

                            If Not l_DataRow Is Nothing Then
                                l_DataRow("Selected") = parameterIsSelected
                            End If
                        End If
                    End If
                    If parameterSelectAccounts = True Then
                        For Each dr As DataRow In l_CalculatedDataTable.Rows
                            If dr("AccountType").GetType.ToString <> "System.DBNull" Then
                                If dr("AccountType") = "AM" Then
                                    dr("Selected") = "True"
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                Case "IsSavings"
                    l_CalculatedDataTable = Session("Calculated_DisplaySavingsPlanAcctContribution_C19")
                    If parameterIndex < 0 Then Return 0

                    If l_CalculatedDataTable Is Nothing Then Return 0

                    If l_CalculatedDataTable.Rows.Count > parameterIndex Then

                        l_DataRow = l_CalculatedDataTable.Rows(parameterIndex)

                        If Not (l_DataRow("AccountType").GetType.ToString() = "System.DBNull") Then

                            If CType(l_DataRow("AccountType"), String).ToString.Trim = "Total" Or CType(l_DataRow("AccountType"), String).ToString.Trim = String.Empty Then Exit Function

                            If Not l_DataRow Is Nothing Then
                                l_DataRow("Selected") = parameterIsSelected
                            End If
                        End If
                    End If
                    If parameterSelectAccounts = True Then
                        For Each dr As DataRow In l_CalculatedDataTable.Rows
                            If dr("AccountType").GetType.ToString <> "System.DBNull" Then
                                If dr("AccountType") = "TM" Then
                                    dr("Selected") = "True"
                                    Exit For
                                End If
                            End If
                        Next
                    End If

            End Select



        Catch
            Throw
        End Try

    End Function
    'Phase V BA Account by Dilip 
    Private Function MakePersonalCalculationTableForDisplay(ByVal parameterIndex As Integer, ByVal parameterIsSelected As Boolean, ByVal parameterCalledBy As String, Optional ByVal parameterSelectAccounts As Boolean = False)


        Dim l_CalculatedDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_BooleanFlag As Boolean = False
        Dim l_Counter As Integer
        Dim l_CheckBox As CheckBox

        Try

            Select Case parameterCalledBy

                Case "IsRetirement"
                    l_CalculatedDataTable = Session("Calculated_DisplayRetirementPlanAcctContribution_C19")
                    If parameterIndex < 0 Then Return 0

                    If l_CalculatedDataTable Is Nothing Then Return 0

                    If l_CalculatedDataTable.Rows.Count > parameterIndex Then

                        l_DataRow = l_CalculatedDataTable.Rows(parameterIndex)

                        If Not (l_DataRow("AccountType").GetType.ToString() = "System.DBNull") Then

                            If CType(l_DataRow("AccountType"), String).ToString.Trim = "Total" Or CType(l_DataRow("AccountType"), String).ToString.Trim = String.Empty Then Exit Function

                            If Not l_DataRow Is Nothing Then
                                l_DataRow("Selected") = parameterIsSelected
                            End If
                        End If
                    End If
                    If parameterSelectAccounts = True Then
                        For Each dr As DataRow In l_CalculatedDataTable.Rows
                            If dr("AccountType").GetType.ToString <> "System.DBNull" Then
                                If dr("AccountType") = "TM" Then
                                    dr("Selected") = "True"
                                    Exit For
                                End If
                            End If
                        Next

                    End If
                Case "IsSavings"
                    l_CalculatedDataTable = Session("Calculated_DisplaySavingsPlanAcctContribution_C19")
                    If parameterIndex < 0 Then Return 0

                    If l_CalculatedDataTable Is Nothing Then Return 0

                    If l_CalculatedDataTable.Rows.Count > parameterIndex Then

                        l_DataRow = l_CalculatedDataTable.Rows(parameterIndex)

                        If Not (l_DataRow("AccountType").GetType.ToString() = "System.DBNull") Then

                            If CType(l_DataRow("AccountType"), String).ToString.Trim = "Total" Or CType(l_DataRow("AccountType"), String).ToString.Trim = String.Empty Then Exit Function

                            If Not l_DataRow Is Nothing Then
                                l_DataRow("Selected") = parameterIsSelected
                            End If
                        End If
                    End If
                    If parameterSelectAccounts = True Then
                        For Each dr As DataRow In l_CalculatedDataTable.Rows
                            If dr("AccountType").GetType.ToString <> "System.DBNull" Then
                                If dr("AccountType") = "TM" Then
                                    dr("Selected") = "True"
                                    Exit For
                                End If
                            End If
                        Next
                    End If

            End Select



        Catch
            Throw
        End Try

    End Function
    'Phase V BA Account by Dilip


    Private Function CalculateTotal(ByVal parameterCalledBy As String)

        Dim l_CalculatedDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_BooleanFlag As Boolean = False
        Dim l_DataRows As DataRow()

        Try
            If parameterCalledBy = "IsRetirement" Then
                If Not Session("Calculated_RetirementPlanAcctContribution_C19") Is Nothing Then
                    l_CalculatedDataTable = DirectCast(Session("Calculated_RetirementPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsSavings" Then
                If Not Session("Calculated_SavingsPlanAcctContribution_C19") Is Nothing Then
                    l_CalculatedDataTable = DirectCast(Session("Calculated_SavingsPlanAcctContribution_C19"), DataTable)
                End If
            ElseIf parameterCalledBy = "IsConsolidated" Then
                If Not Session("CalculatedDataTable_C19") Is Nothing Then
                    l_CalculatedDataTable = DirectCast(Session("CalculatedDataTable_C19"), DataTable)
                End If
            End If

            l_CalculatedDataTable = objRefundRequest.CalculateTotal(l_CalculatedDataTable)

            If Not l_CalculatedDataTable Is Nothing Then

                ' If the field Total is already exist in the Table then Delete the Row.
                If parameterCalledBy = "IsConsolidated" Then
                    l_DataRows = l_CalculatedDataTable.Select("AccountType = 'Total'")
                    If l_DataRows.Length > 0 Then
                        Me.FillValuesToControls(l_DataRows(0))
                    End If
                End If


                'Comment code Deleted By Sanjeev on 06/02/2012

                If parameterCalledBy = "IsRetirement" Then
                    Session("Calculated_RetirementPlanAcctContribution_C19") = l_CalculatedDataTable
                    'Comment code Deleted By Sanjeev on 06/02/2012
                ElseIf parameterCalledBy = "IsSavings" Then
                    Session("Calculated_SavingsPlanAcctContribution_C19") = l_CalculatedDataTable
                    'Comment code Deleted By Sanjeev on 06/02/2012
                ElseIf parameterCalledBy = "IsConsolidated" Then
                    Session("CalculatedDataTable_C19") = l_CalculatedDataTable
                End If

            End If
        Catch
            Throw
        End Try

    End Function

    Public Function CreateFinalDataset1()
        Dim l_datatable_Final As New DataTable
        Dim l_datatable_DisplayFinal As DataTable
        Dim l_datatable_RetirementAccount As DataTable
        Dim drow As DataRow
        Dim l_datarow_finalTable As DataRow
        Dim l_GetYmcadatarow As DataRow()
        Dim l_getPersonalDatarow As DataRow()
        Dim l_bool_PersonalFlag As Boolean = False
        Dim l_bool_YmcaFlag As Boolean = False
        Dim l_datarowfindrow As DataRow()
        Dim l_string_ymcaid As String = String.Empty
        Dim l_datatable As New DataTable


        Dim l_datarow As DataRow
        Try
            'for personal and ymca rows in datagrid get teh data from Session("RetirementPlanAcctContribution")
            'Session("CalculatedDataTableDisplay")- Final datatable from the display
            'get the format of teh saving datatable from Session("CalculatedDataTable")
            If Not Session("RetirementPlanAcctContribution_C19") Is Nothing Then
                l_datatable_RetirementAccount = DirectCast(Session("RetirementPlanAcctContribution_C19"), DataTable)
                'l_string_ymcaid = l_datatable_RetirementAccount.Rows(0)("ymcaid")
            End If
            If Not Session("CalculatedDataTable_C19") Is Nothing Then
                l_datatable_Final = DirectCast(Session("CalculatedDataTable_C19"), DataTable).Clone
                l_datatable = DirectCast(Session("CalculatedDataTable_C19"), DataTable)
                l_string_ymcaid = l_datatable.Rows(0)("ymcaid").ToString()

            End If

            If Not Session("CalculatedDataTableDisplay_C19") Is Nothing Then
                l_datatable_DisplayFinal = DirectCast(Session("CalculatedDataTableDisplay_C19"), DataTable)
                If l_datatable_DisplayFinal.Rows.Count > 0 Then
                    l_GetYmcadatarow = l_datatable_DisplayFinal.Select("AccountType = '" + "Ymca" + "' ")

                    If l_GetYmcadatarow.Length > 0 And l_GetYmcadatarow(0)("Selected") = True Then
                        l_bool_YmcaFlag = True
                    End If
                    l_getPersonalDatarow = l_datatable_DisplayFinal.Select("AccountType = '" + "Personal" + "' ")
                    If l_getPersonalDatarow.Length > 0 And l_getPersonalDatarow(0)("Selected") = True Then
                        l_bool_PersonalFlag = True
                    End If

                    If l_bool_YmcaFlag = True And l_bool_PersonalFlag = True Then
                        For Each drow In l_datatable_RetirementAccount.Rows
                            If drow("IsBasicAccount").GetType.ToString = "System.Boolean" Then
                                If CType(drow("IsBasicAccount"), Boolean) = True Then
                                    l_datarow_finalTable = l_datatable_Final.NewRow()
                                    l_datarow_finalTable("AccountType") = drow("AccountType")
                                    l_datarow_finalTable("YmcaID") = drow("ymcaid")
                                    l_datarow_finalTable("IsBasicAccount") = drow("IsBasicAccount")
                                    l_datarow_finalTable("Taxable") = drow("Taxable")
                                    l_datarow_finalTable("Non-Taxable") = drow("Non-Taxable")
                                    l_datarow_finalTable("Interest") = drow("Interest")
                                    l_datarow_finalTable("Emp.Total") = drow("Emp.Total")
                                    l_datarow_finalTable("YMCATaxable") = drow("YMCATaxable")
                                    l_datarow_finalTable("YMCAInterest") = drow("YMCAInterest")
                                    l_datarow_finalTable("YMCATotal") = drow("YMCATotal")
                                    l_datarow_finalTable("Total") = drow("Total")
                                    l_datarow_finalTable("PlanType") = drow("PlanType")
                                    l_datarow_finalTable("AccountGroup") = drow("AccountGroup")
                                    l_datarow_finalTable("Selected") = True
                                    l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                End If
                            End If
                        Next
                    ElseIf l_bool_YmcaFlag = False And l_bool_PersonalFlag = True Then

                        For Each drow In l_datatable_RetirementAccount.Rows
                            If drow("IsBasicAccount").GetType.ToString = "System.Boolean" Then
                                If CType(drow("IsBasicAccount"), Boolean) = True Then
                                    l_datarow_finalTable = l_datatable_Final.NewRow()
                                    l_datarow_finalTable("AccountType") = drow("AccountType")
                                    l_datarow_finalTable("YmcaID") = drow("ymcaid")
                                    l_datarow_finalTable("IsBasicAccount") = drow("IsBasicAccount")
                                    l_datarow_finalTable("Taxable") = drow("Taxable")
                                    l_datarow_finalTable("Non-Taxable") = drow("Non-Taxable")
                                    l_datarow_finalTable("Interest") = drow("Interest")
                                    l_datarow_finalTable("Emp.Total") = drow("Emp.Total")
                                    l_datarow_finalTable("YMCATaxable") = "0.0"
                                    l_datarow_finalTable("YMCAInterest") = "0.0"
                                    l_datarow_finalTable("YMCATotal") = "0.0"
                                    l_datarow_finalTable("Total") = drow("Emp.Total")
                                    l_datarow_finalTable("PlanType") = drow("PlanType")
                                    l_datarow_finalTable("AccountGroup") = drow("AccountGroup")
                                    l_datarow_finalTable("Selected") = True
                                    l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                End If
                            End If
                        Next
                    ElseIf l_bool_YmcaFlag = True And l_bool_PersonalFlag = False Then
                        For Each drow In l_datatable_RetirementAccount.Rows
                            If drow("IsBasicAccount").GetType.ToString = "System.Boolean" Then
                                If CType(drow("IsBasicAccount"), Boolean) = True Then
                                    l_datarow_finalTable = l_datatable_Final.NewRow()
                                    l_datarow_finalTable("AccountType") = drow("AccountType")
                                    l_datarow_finalTable("YmcaID") = drow("ymcaid")
                                    l_datarow_finalTable("IsBasicAccount") = drow("IsBasicAccount")
                                    l_datarow_finalTable("Taxable") = "0.0"
                                    l_datarow_finalTable("Non-Taxable") = "0.0"
                                    l_datarow_finalTable("Interest") = "0.0"
                                    l_datarow_finalTable("Emp.Total") = "0.0"
                                    l_datarow_finalTable("YMCATaxable") = drow("YMCATaxable")
                                    l_datarow_finalTable("YMCAInterest") = drow("YMCAInterest")
                                    l_datarow_finalTable("YMCATotal") = drow("YMCATotal")
                                    l_datarow_finalTable("Total") = drow("YMCATotal")
                                    l_datarow_finalTable("PlanType") = drow("PlanType")
                                    l_datarow_finalTable("AccountGroup") = drow("AccountGroup")
                                    l_datarow_finalTable("Selected") = True
                                    l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                End If
                            End If
                        Next
                    End If
                    For Each l_datarow In l_datatable_DisplayFinal.Rows
                        If Not (l_datarow("AccountType").GetType.ToString = "System.DBNull") Then
                            If l_datarow("Selected") = True Then
                                If l_datarow("IsBasicAccount").GetType.ToString = "System.Boolean" Then
                                    If l_datarow("IsBasicAccount").ToString.ToUpper.Trim() = False Then
                                        If l_datarow("AccountType").ToString.ToUpper.Trim() = "AM-MATCHED" Then

                                            l_datarowfindrow = l_datatable_Final.Select("AccountType = '" + "AM" + "'")
                                            If l_datarowfindrow.Length > 0 Then
                                                l_datarowfindrow(0)("YMCATaxable") = l_datarow("YMCATaxable")
                                                l_datarowfindrow(0)("YMCAInterest") = l_datarow("YMCAInterest")
                                                l_datarowfindrow(0)("YMCATotal") = l_datarow("YMCATotal")
                                                l_datarowfindrow(0)("Total") = l_datarow("Emp.Total") + l_datarow("YMCATotal")
                                            Else
                                                l_datarow_finalTable = l_datatable_Final.NewRow()
                                                l_datarow_finalTable("AccountType") = "AM"
                                                l_datarow_finalTable("YmcaID") = l_string_ymcaid
                                                l_datarow_finalTable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                                l_datarow_finalTable("Taxable") = "0.0"
                                                l_datarow_finalTable("Non-Taxable") = "0.0"
                                                l_datarow_finalTable("Interest") = "0.0"
                                                l_datarow_finalTable("Emp.Total") = "0.0"
                                                l_datarow_finalTable("YMCATaxable") = l_datarow("Taxable")
                                                l_datarow_finalTable("YMCAInterest") = l_datarow("Interest")
                                                l_datarow_finalTable("YMCATotal") = l_datarow("Total")
                                                l_datarow_finalTable("Total") = l_datarow("Total")
                                                l_datarow_finalTable("PlanType") = l_datarow("PlanType")
                                                l_datarow_finalTable("AccountGroup") = l_datarow("AccountGroup")
                                                l_datarow_finalTable("Selected") = True
                                                l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                            End If

                                            'l_datatable_Final.Rows.Add(l_datarow_finalTable)


                                        ElseIf l_datarow("AccountType").ToString.ToUpper.Trim() = "TM-MATCHED" Then

                                            l_datarowfindrow = l_datatable_Final.Select("AccountType = '" + "TM" + "'")
                                            If l_datarowfindrow.Length > 0 Then
                                                l_datarowfindrow(0)("YMCATaxable") = l_datarow("YMCATaxable")
                                                l_datarowfindrow(0)("YMCAInterest") = l_datarow("YMCAInterest")
                                                l_datarowfindrow(0)("YMCATotal") = l_datarow("YMCATotal")
                                                l_datarowfindrow(0)("Total") = l_datarow("Emp.Total") + l_datarow("YMCATotal")
                                            Else
                                                l_datarow_finalTable = l_datatable_Final.NewRow()
                                                l_datarow_finalTable("AccountType") = "TM"
                                                l_datarow_finalTable("YmcaID") = l_string_ymcaid
                                                l_datarow_finalTable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                                l_datarow_finalTable("Taxable") = "0.0"
                                                l_datarow_finalTable("Non-Taxable") = "0.0"
                                                l_datarow_finalTable("Interest") = "0.0"
                                                l_datarow_finalTable("Emp.Total") = "0.0"
                                                l_datarow_finalTable("YMCATaxable") = l_datarow("Taxable")
                                                l_datarow_finalTable("YMCAInterest") = l_datarow("Interest")
                                                l_datarow_finalTable("YMCATotal") = l_datarow("Total")
                                                l_datarow_finalTable("Total") = l_datarow("Total")
                                                l_datarow_finalTable("PlanType") = l_datarow("PlanType")
                                                l_datarow_finalTable("AccountGroup") = l_datarow("AccountGroup")
                                                l_datarow_finalTable("Selected") = True
                                                l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                            End If
                                        Else

                                            l_datarow_finalTable = l_datatable_Final.NewRow()
                                            l_datarow_finalTable("AccountType") = l_datarow("AccountType")
                                            l_datarow_finalTable("YmcaID") = l_string_ymcaid
                                            l_datarow_finalTable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                            l_datarow_finalTable("Taxable") = l_datarow("Taxable")
                                            l_datarow_finalTable("Non-Taxable") = l_datarow("Non-Taxable")
                                            l_datarow_finalTable("Interest") = l_datarow("Interest")
                                            l_datarow_finalTable("Emp.Total") = l_datarow("Total")
                                            l_datarow_finalTable("YMCATaxable") = "0.0"
                                            l_datarow_finalTable("YMCAInterest") = "0.0"
                                            l_datarow_finalTable("YMCATotal") = "0.0"
                                            l_datarow_finalTable("Total") = l_datarow("Total")
                                            l_datarow_finalTable("PlanType") = l_datarow("PlanType")
                                            l_datarow_finalTable("AccountGroup") = l_datarow("AccountGroup")
                                            l_datarow_finalTable("Selected") = True
                                            l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                        End If
                                    End If
                                End If
                            End If

                        End If
                    Next

                End If

            End If

            l_datatable_Final.AcceptChanges()
            Session("CalculatedDataTable_C19") = l_datatable_Final


        Catch
            Throw
        End Try
    End Function

    Public Function CreateFinalDataset()
        Dim l_datatable_Final As New DataTable
        Dim l_datatable_DisplayFinal As DataTable
        Dim l_datatable_RetirementAccount As DataTable
        Dim drow As DataRow
        Dim l_datarow_finalTable As DataRow
        Dim l_GetYmcadatarow As DataRow()
        Dim l_getPersonalDatarow As DataRow()
        Dim l_bool_PersonalFlag As Boolean = False
        Dim l_bool_YmcaFlag As Boolean = False
        Dim l_bool_BaErFlag As Boolean = False
        Dim l_bool_PIAFlag As Boolean = False
        Dim l_datarowfindrow As DataRow()
        Dim l_string_ymcaid As String = String.Empty
        Dim l_datatable As New DataTable


        Dim l_datarow As DataRow
        Try

            'for personal and ymca rows in datagrid get teh data from Session("RetirementPlanAcctContribution")
            'Session("CalculatedDataTableDisplay")- Final datatable from the display
            'get the format of teh saving datatable from Session("CalculatedDataTable")

            If Not Session("RetirementPlanAcctContribution_C19") Is Nothing Then
                l_datatable_RetirementAccount = DirectCast(Session("RetirementPlanAcctContribution_C19"), DataTable)
                'l_string_ymcaid = l_datatable_RetirementAccount.Rows(0)("ymcaid")
            End If


            If Not Session("CalculatedDataTable_C19") Is Nothing Then
                l_datatable_Final = DirectCast(Session("CalculatedDataTable_C19"), DataTable).Clone
                l_datatable = DirectCast(Session("CalculatedDataTable_C19"), DataTable)
                l_string_ymcaid = l_datatable.Rows(0)("ymcaid").ToString()
            End If


            If Not Session("CalculatedDataTableDisplay_C19") Is Nothing Then
                'Comment code Deleted By Sanjeev on 06/02/2012

                If Me.RefundType <> "PART" And blnSavingsPlan = False And blnRetirementPlan = False Then

                    l_datatable_RetirementAccount = CType(Session("RetirementPlanAcctContribution_C19"), DataTable)
                    l_datatable_DisplayFinal = CType(Session("CalculatedDataTableDisplay_C19"), DataTable)

                ElseIf blnRetirementPlan = True Then

                    l_datatable_RetirementAccount = GetPartialRetirementDatatable(CType(Session("RetirementPlanAcctContribution_C19"), DataTable))
                    l_datatable_DisplayFinal = GetPartialVOlDatatable(CType(Session("CalculatedDataTableDisplay_C19"), DataTable), True, False, 1) 'Me.NumPercentageFactorofMoneyRetirement)

                ElseIf blnSavingsPlan = True Then
                    l_datatable_DisplayFinal = GetPartialVOlDatatable(CType(Session("CalculatedDataTableDisplay_C19"), DataTable), False, True, 1) 'Me.NumPercentageFactorofMoneySavings
                End If

                'Commented By Ganesh for Partial

                If l_datatable_DisplayFinal.Rows.Count > 0 Then
                    'l_GetYmcadatarow = l_datatable_DisplayFinal.Select("AccountType = '" + "YMCA" + "' ")
                    l_GetYmcadatarow = l_datatable_DisplayFinal.Select("AccountType = '" + m_const_YMCA_Legacy_Acct + "' ")
                    If l_GetYmcadatarow.Length > 0 Then
                        If l_GetYmcadatarow(0)("Selected") = True Then
                            l_bool_YmcaFlag = True
                            'BA Account Include Phase V 28-04-2009 by Dilip
                            l_bool_PIAFlag = True
                            'BA Account Include Phase V 28-04-2009 by Dilip
                        End If

                    End If
                    l_getPersonalDatarow = l_datatable_DisplayFinal.Select("AccountType = '" + "Personal" + "' ")
                    If l_getPersonalDatarow.Length > 0 Then
                        If l_getPersonalDatarow(0)("Selected") = True Then
                            l_bool_PersonalFlag = True
                        End If

                    End If
                    'BA Account Phase V 22-04-2009 by Dilip
                    l_GetYmcadatarow = l_datatable_DisplayFinal.Select("AccountType = '" + m_const_YMCA__Acct + "' ")
                    If l_GetYmcadatarow.Length > 0 Then
                        If l_GetYmcadatarow(0)("Selected") = True Then
                            l_bool_BaErFlag = True
                            l_bool_YmcaFlag = True
                        End If

                    End If


                    'BA Account Phase V 22-04-2009 by Dilip
                    If l_bool_YmcaFlag = True And l_bool_PersonalFlag = True Then
                        For Each drow In l_datatable_RetirementAccount.Rows
                            If drow("IsBasicAccount").GetType.ToString = "System.Boolean" Then
                                'If CType(drow("IsBasicAccount"), Boolean) = True 
                                If CType(drow("IsBasicAccount"), Boolean) = True Then
                                    If l_bool_BaErFlag = True And l_bool_PIAFlag = True Then
                                        l_datarow_finalTable = l_datatable_Final.NewRow()
                                        l_datarow_finalTable("AccountType") = drow("AccountType")
                                        l_datarow_finalTable("YmcaID") = drow("ymcaid")
                                        l_datarow_finalTable("IsBasicAccount") = drow("IsBasicAccount")
                                        l_datarow_finalTable("Taxable") = drow("Taxable")
                                        l_datarow_finalTable("Non-Taxable") = drow("Non-Taxable")
                                        l_datarow_finalTable("Interest") = drow("Interest")
                                        l_datarow_finalTable("Emp.Total") = drow("Emp.Total")
                                        l_datarow_finalTable("YMCATaxable") = drow("YMCATaxable")
                                        l_datarow_finalTable("YMCAInterest") = drow("YMCAInterest")
                                        l_datarow_finalTable("YMCATotal") = drow("YMCATotal")
                                        l_datarow_finalTable("Total") = drow("Total")
                                        l_datarow_finalTable("PlanType") = drow("PlanType")
                                        l_datarow_finalTable("AccountGroup") = drow("AccountGroup")
                                        l_datarow_finalTable("Selected") = True
                                        l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                    ElseIf l_bool_BaErFlag = False And l_bool_PIAFlag = True Then
                                        l_datarow_finalTable = l_datatable_Final.NewRow()
                                        l_datarow_finalTable("AccountType") = drow("AccountType")
                                        l_datarow_finalTable("YmcaID") = drow("ymcaid")
                                        l_datarow_finalTable("IsBasicAccount") = drow("IsBasicAccount")
                                        l_datarow_finalTable("Taxable") = drow("Taxable")
                                        l_datarow_finalTable("Non-Taxable") = drow("Non-Taxable")
                                        l_datarow_finalTable("Interest") = drow("Interest")
                                        l_datarow_finalTable("Emp.Total") = drow("Emp.Total")
                                        If drow("AccountGroup") <> "RBA" Then
                                            l_datarow_finalTable("YMCATaxable") = drow("YMCATaxable")
                                            l_datarow_finalTable("YMCAInterest") = drow("YMCAInterest")
                                            l_datarow_finalTable("YMCATotal") = drow("YMCATotal")
                                            l_datarow_finalTable("Total") = drow("Total")
                                        Else
                                            l_datarow_finalTable("Total") = drow("Emp.Total")
                                        End If

                                        l_datarow_finalTable("PlanType") = drow("PlanType")
                                        l_datarow_finalTable("AccountGroup") = drow("AccountGroup")
                                        l_datarow_finalTable("Selected") = True
                                        l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                    Else
                                        l_datarow_finalTable = l_datatable_Final.NewRow()
                                        l_datarow_finalTable("AccountType") = drow("AccountType")
                                        l_datarow_finalTable("YmcaID") = drow("ymcaid")
                                        l_datarow_finalTable("IsBasicAccount") = drow("IsBasicAccount")
                                        l_datarow_finalTable("Taxable") = drow("Taxable")
                                        l_datarow_finalTable("Non-Taxable") = drow("Non-Taxable")
                                        l_datarow_finalTable("Interest") = drow("Interest")
                                        l_datarow_finalTable("Emp.Total") = drow("Emp.Total")
                                        If drow("AccountGroup") = "RBA" Then
                                            l_datarow_finalTable("YMCATaxable") = drow("YMCATaxable")
                                            l_datarow_finalTable("YMCAInterest") = drow("YMCAInterest")
                                            l_datarow_finalTable("YMCATotal") = drow("YMCATotal")
                                            l_datarow_finalTable("Total") = drow("Total")
                                        Else
                                            l_datarow_finalTable("Total") = drow("Emp.Total")
                                        End If

                                        l_datarow_finalTable("PlanType") = drow("PlanType")
                                        l_datarow_finalTable("AccountGroup") = drow("AccountGroup")
                                        l_datarow_finalTable("Selected") = True
                                        l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                    End If

                                End If
                            End If
                        Next

                    ElseIf l_bool_YmcaFlag = False And l_bool_PersonalFlag = True Then

                        For Each drow In l_datatable_RetirementAccount.Rows
                            If drow("IsBasicAccount").GetType.ToString = "System.Boolean" Then
                                If CType(drow("IsBasicAccount"), Boolean) = True Then
                                    l_datarow_finalTable = l_datatable_Final.NewRow()
                                    l_datarow_finalTable("AccountType") = drow("AccountType")
                                    l_datarow_finalTable("YmcaID") = drow("ymcaid")
                                    l_datarow_finalTable("IsBasicAccount") = drow("IsBasicAccount")
                                    l_datarow_finalTable("Taxable") = drow("Taxable")
                                    l_datarow_finalTable("Non-Taxable") = drow("Non-Taxable")
                                    l_datarow_finalTable("Interest") = drow("Interest")
                                    l_datarow_finalTable("Emp.Total") = drow("Emp.Total")
                                    l_datarow_finalTable("YMCATaxable") = "0.0"
                                    l_datarow_finalTable("YMCAInterest") = "0.0"
                                    l_datarow_finalTable("YMCATotal") = "0.0"
                                    l_datarow_finalTable("Total") = drow("Emp.Total")
                                    l_datarow_finalTable("PlanType") = drow("PlanType")
                                    l_datarow_finalTable("AccountGroup") = drow("AccountGroup")
                                    l_datarow_finalTable("Selected") = True
                                    l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                End If
                            End If
                        Next
                    ElseIf l_bool_YmcaFlag = True And l_bool_PersonalFlag = False Then
                        For Each drow In l_datatable_RetirementAccount.Rows
                            If drow("IsBasicAccount").GetType.ToString = "System.Boolean" Then
                                ' Dilip Phase V changes to include BA ER and YMCA account validations. 
                                If CType(drow("IsBasicAccount"), Boolean) = True Then
                                    If l_bool_BaErFlag = True And l_bool_PIAFlag = True Then
                                        l_datarow_finalTable = l_datatable_Final.NewRow()
                                        l_datarow_finalTable("AccountType") = drow("AccountType")
                                        l_datarow_finalTable("YmcaID") = drow("ymcaid")
                                        l_datarow_finalTable("IsBasicAccount") = drow("IsBasicAccount")
                                        l_datarow_finalTable("Taxable") = "0.0"
                                        l_datarow_finalTable("Non-Taxable") = "0.0"
                                        l_datarow_finalTable("Interest") = "0.0"
                                        l_datarow_finalTable("Emp.Total") = "0.0"
                                        l_datarow_finalTable("YMCATaxable") = drow("YMCATaxable")
                                        l_datarow_finalTable("YMCAInterest") = drow("YMCAInterest")
                                        l_datarow_finalTable("YMCATotal") = drow("YMCATotal")
                                        l_datarow_finalTable("Total") = drow("YMCATotal")
                                        l_datarow_finalTable("PlanType") = drow("PlanType")
                                        l_datarow_finalTable("AccountGroup") = drow("AccountGroup")
                                        l_datarow_finalTable("Selected") = True
                                        l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                    ElseIf l_bool_BaErFlag = False And l_bool_PIAFlag = True Then
                                        If drow("AccountGroup") <> "RBA" Then
                                            l_datarow_finalTable = l_datatable_Final.NewRow()
                                            l_datarow_finalTable("AccountType") = drow("AccountType")
                                            l_datarow_finalTable("YmcaID") = drow("ymcaid")
                                            l_datarow_finalTable("IsBasicAccount") = drow("IsBasicAccount")
                                            l_datarow_finalTable("Taxable") = "0.0"
                                            l_datarow_finalTable("Non-Taxable") = "0.0"
                                            l_datarow_finalTable("Interest") = "0.0"
                                            l_datarow_finalTable("Emp.Total") = "0.0"
                                            l_datarow_finalTable("YMCATaxable") = drow("YMCATaxable")
                                            l_datarow_finalTable("YMCAInterest") = drow("YMCAInterest")
                                            l_datarow_finalTable("YMCATotal") = drow("YMCATotal")
                                            l_datarow_finalTable("Total") = drow("YMCATotal")
                                            l_datarow_finalTable("PlanType") = drow("PlanType")
                                            l_datarow_finalTable("AccountGroup") = drow("AccountGroup")
                                            l_datarow_finalTable("Selected") = True
                                            l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                        End If
                                    Else
                                        If drow("AccountGroup") = "RBA" Then
                                            l_datarow_finalTable = l_datatable_Final.NewRow()
                                            l_datarow_finalTable("AccountType") = drow("AccountType")
                                            l_datarow_finalTable("YmcaID") = drow("ymcaid")
                                            l_datarow_finalTable("IsBasicAccount") = drow("IsBasicAccount")
                                            l_datarow_finalTable("Taxable") = "0.0"
                                            l_datarow_finalTable("Non-Taxable") = "0.0"
                                            l_datarow_finalTable("Interest") = "0.0"
                                            l_datarow_finalTable("Emp.Total") = "0.0"
                                            l_datarow_finalTable("YMCATaxable") = drow("YMCATaxable")
                                            l_datarow_finalTable("YMCAInterest") = drow("YMCAInterest")
                                            l_datarow_finalTable("YMCATotal") = drow("YMCATotal")
                                            l_datarow_finalTable("Total") = drow("YMCATotal")
                                            l_datarow_finalTable("PlanType") = drow("PlanType")
                                            l_datarow_finalTable("AccountGroup") = drow("AccountGroup")
                                            l_datarow_finalTable("Selected") = True
                                            l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                    For Each l_datarow In l_datatable_DisplayFinal.Rows
                        If Not (l_datarow("AccountType").GetType.ToString = "System.DBNull") Then
                            If l_datarow("IsBasicAccount").GetType.ToString = "System.Boolean" Then
                                If l_datarow("IsBasicAccount").ToString.ToUpper.Trim() = False Then
                                    If l_datarow("AccountType").ToString.ToUpper.Trim() = "AM-MATCHED" Then
                                        l_datarow_finalTable = l_datatable_Final.NewRow()
                                        l_datarow_finalTable("AccountType") = "AM"
                                        l_datarow_finalTable("YmcaID") = l_string_ymcaid
                                        l_datarow_finalTable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                        l_datarow_finalTable("Taxable") = "0.0"
                                        l_datarow_finalTable("Non-Taxable") = "0.0"
                                        l_datarow_finalTable("Interest") = "0.0"
                                        l_datarow_finalTable("Emp.Total") = "0.0"
                                        l_datarow_finalTable("YMCATaxable") = l_datarow("Taxable")
                                        l_datarow_finalTable("YMCAInterest") = l_datarow("Interest")
                                        l_datarow_finalTable("YMCATotal") = l_datarow("Total")
                                        l_datarow_finalTable("Total") = l_datarow("Total")
                                        l_datarow_finalTable("PlanType") = l_datarow("PlanType")
                                        l_datarow_finalTable("AccountGroup") = l_datarow("AccountGroup")
                                        l_datarow_finalTable("Selected") = True


                                        l_datarowfindrow = l_datatable_DisplayFinal.Select("AccountType = '" + "AM" + "'")
                                        If l_datarowfindrow.Length > 0 Then
                                            l_datarow_finalTable("Taxable") = l_datarowfindrow(0)("Taxable")
                                            l_datarow_finalTable("Non-Taxable") = l_datarowfindrow(0)("Non-Taxable")
                                            l_datarow_finalTable("Interest") = l_datarowfindrow(0)("Interest")
                                            l_datarow_finalTable("Emp.Total") = l_datarowfindrow(0)("Total")
                                            l_datarow_finalTable("Total") += l_datarowfindrow(0)("Total")

                                            l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                        Else
                                            l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                        End If

                                        'l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                    ElseIf l_datarow("AccountType").ToString.ToUpper.Trim() = "AM" Then
                                        l_datarowfindrow = l_datatable_DisplayFinal.Select("AccountType = '" + "AM-MATCHED" + "'")
                                        If l_datarowfindrow.Length > 0 Then
                                        Else
                                            l_datarow_finalTable = l_datatable_Final.NewRow()
                                            l_datarow_finalTable("AccountType") = "AM"
                                            l_datarow_finalTable("YmcaID") = l_string_ymcaid
                                            l_datarow_finalTable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                            l_datarow_finalTable("Taxable") = l_datarow("Taxable")
                                            l_datarow_finalTable("Non-Taxable") = l_datarow("Non-Taxable")
                                            l_datarow_finalTable("Interest") = l_datarow("Interest")
                                            l_datarow_finalTable("Emp.Total") = l_datarow("Total")
                                            l_datarow_finalTable("YMCATaxable") = "0.0"
                                            l_datarow_finalTable("YMCAInterest") = "0.0"
                                            l_datarow_finalTable("YMCATotal") = "0.0"
                                            l_datarow_finalTable("Total") = l_datarow("Total")
                                            l_datarow_finalTable("PlanType") = l_datarow("PlanType")
                                            l_datarow_finalTable("AccountGroup") = l_datarow("AccountGroup")
                                            l_datarow_finalTable("Selected") = True
                                            l_datatable_Final.Rows.Add(l_datarow_finalTable)

                                        End If


                                    ElseIf l_datarow("AccountType").ToString.ToUpper.Trim() = "TM-MATCHED" Then
                                        l_datarow_finalTable = l_datatable_Final.NewRow()
                                        l_datarow_finalTable("AccountType") = "TM"
                                        l_datarow_finalTable("YmcaID") = l_string_ymcaid
                                        l_datarow_finalTable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                        l_datarow_finalTable("Taxable") = "0.0"
                                        l_datarow_finalTable("Non-Taxable") = "0.0"
                                        l_datarow_finalTable("Interest") = "0.0"
                                        l_datarow_finalTable("Emp.Total") = "0.0"
                                        l_datarow_finalTable("YMCATaxable") = l_datarow("Taxable")
                                        l_datarow_finalTable("YMCAInterest") = l_datarow("Interest")
                                        l_datarow_finalTable("YMCATotal") = l_datarow("Total")
                                        l_datarow_finalTable("Total") = l_datarow("Total")
                                        l_datarow_finalTable("PlanType") = l_datarow("PlanType")
                                        l_datarow_finalTable("AccountGroup") = l_datarow("AccountGroup")
                                        l_datarow_finalTable("Selected") = True

                                        l_datarowfindrow = l_datatable_DisplayFinal.Select("AccountType = '" + "TM" + "'")
                                        If l_datarowfindrow.Length > 0 Then
                                            l_datarow_finalTable("Taxable") = l_datarowfindrow(0)("Taxable")
                                            l_datarow_finalTable("Non-Taxable") = l_datarowfindrow(0)("Non-Taxable")
                                            l_datarow_finalTable("Interest") = l_datarowfindrow(0)("Interest")
                                            l_datarow_finalTable("Emp.Total") = l_datarowfindrow(0)("Total")
                                            l_datarow_finalTable("Total") += l_datarowfindrow(0)("Total")

                                            l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                        Else
                                            l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                        End If
                                    ElseIf l_datarow("AccountType").ToString.ToUpper.Trim() = "TM" Then
                                        l_datarowfindrow = l_datatable_DisplayFinal.Select("AccountType = '" + "TM-MATCHED" + "'")
                                        If l_datarowfindrow.Length > 0 Then
                                        Else
                                            l_datarow_finalTable = l_datatable_Final.NewRow()
                                            l_datarow_finalTable("AccountType") = "TM"
                                            l_datarow_finalTable("YmcaID") = l_string_ymcaid
                                            l_datarow_finalTable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                            l_datarow_finalTable("Taxable") = l_datarow("Taxable")
                                            l_datarow_finalTable("Non-Taxable") = l_datarow("Non-Taxable")
                                            l_datarow_finalTable("Interest") = l_datarow("Interest")
                                            l_datarow_finalTable("Emp.Total") = l_datarow("Total")
                                            l_datarow_finalTable("YMCATaxable") = "0.0"
                                            l_datarow_finalTable("YMCAInterest") = "0.0"
                                            l_datarow_finalTable("YMCATotal") = "0.0"
                                            l_datarow_finalTable("Total") = l_datarow("Total")
                                            l_datarow_finalTable("PlanType") = l_datarow("PlanType")
                                            l_datarow_finalTable("AccountGroup") = l_datarow("AccountGroup")
                                            l_datarow_finalTable("Selected") = True
                                            l_datatable_Final.Rows.Add(l_datarow_finalTable)

                                        End If
                                    Else
                                        l_datarow_finalTable = l_datatable_Final.NewRow()
                                        l_datarow_finalTable("AccountType") = l_datarow("AccountType")
                                        l_datarow_finalTable("YmcaID") = l_string_ymcaid
                                        l_datarow_finalTable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                        If l_datarow("AccountGroup") = m_const_RetirementPlan_SR Then
                                            l_datarow_finalTable("Taxable") = "0.0"
                                            l_datarow_finalTable("Non-Taxable") = "0.0"
                                            l_datarow_finalTable("Interest") = "0.0"
                                            l_datarow_finalTable("Emp.Total") = "0.0"
                                            l_datarow_finalTable("YMCATaxable") = l_datarow("Taxable")
                                            l_datarow_finalTable("YMCAInterest") = l_datarow("Interest")
                                            l_datarow_finalTable("YMCATotal") = l_datarow("Total")
                                            l_datarow_finalTable("Total") = l_datarow("Total")
                                            'Begin Code Merge by Dilip on 07-05-2009
                                            'Priya 13-Jan-2009 : YRS 5.0-637 AC Account interest components, Added elseif condition for AC account type
                                        ElseIf l_datarow("AccountGroup") = m_const_RetirementPlan_AC Then
                                            l_datarow_finalTable("Taxable") = l_datarow("Taxable")
                                            l_datarow_finalTable("Non-Taxable") = l_datarow("Non-Taxable")
                                            l_datarow_finalTable("Interest") = l_datarow("Interest")
                                            l_datarow_finalTable("Emp.Total") = l_datarow("Total")
                                            l_datarow_finalTable("YMCATaxable") = "0.0"
                                            l_datarow_finalTable("YMCAInterest") = "0.0"
                                            l_datarow_finalTable("YMCATotal") = "0.0"
                                            l_datarow_finalTable("Total") = l_datarow("Total")
                                            'End 13-Jan-2009
                                            'End Code Merge by Dilip on 07-05-2009
                                        Else
                                            l_datarow_finalTable("Taxable") = l_datarow("Taxable")
                                            l_datarow_finalTable("Non-Taxable") = l_datarow("Non-Taxable")
                                            l_datarow_finalTable("Interest") = l_datarow("Interest")
                                            l_datarow_finalTable("Emp.Total") = l_datarow("Total")
                                            l_datarow_finalTable("YMCATaxable") = "0.0"
                                            l_datarow_finalTable("YMCAInterest") = "0.0"
                                            l_datarow_finalTable("YMCATotal") = "0.0"
                                            l_datarow_finalTable("Total") = l_datarow("Total")
                                        End If
                                        l_datarow_finalTable("PlanType") = l_datarow("PlanType")
                                        l_datarow_finalTable("AccountGroup") = l_datarow("AccountGroup")
                                        l_datarow_finalTable("Selected") = True
                                        l_datatable_Final.Rows.Add(l_datarow_finalTable)
                                    End If

                                End If
                                'l_datatable_Final.Rows.Add(l_datarow_finalTable)
                            End If
                        End If
                    Next

                End If

            End If

            'Comment code Deleted By Sanjeev on 06/02/2012

            'Add By Ganesh for partial
            If blnSavingsPlan = False And blnRetirementPlan = False Then
                l_datatable_Final.AcceptChanges()
                Session("CalculatedDataTable_C19") = l_datatable_Final
                objRefundRequest.CalculateTotalForDisplay(l_datatable_Final, False)
            ElseIf blnRetirementPlan = True Then
                l_datatable_Final.AcceptChanges()
                Me.Session("CalculatedDataTableRetirementPlan_C19") = l_datatable_Final
                objRefundRequest.CalculateTotalForDisplay(l_datatable_Final, False)
            ElseIf blnSavingsPlan = True Then
                l_datatable_Final.AcceptChanges()
                Me.Session("CalculatedDataTableSavingPlan_C19") = l_datatable_Final
                'Start: Dinesh Kanojia(DK)     2013.07.14      BT-1502: YRS 5.0-1746:Include partial withdrawals 
                'Added two parameter for partial withdrawal
                'Remove two parameter for partial withdrawal
                '2013.10.23        Dinesh Kanojia(DK)   BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                'objRefundRequest.CalculateTotalForDisplay(l_datatable_Final, False, CheckboxPartialSavings.Checked, TextBoxTerminated.Text.Trim.Trim.ToUpper)
                objRefundRequest.CalculateTotalForDisplay(l_datatable_Final, False)

            End If
            'Add By Ganesh for partial


        Catch
            Throw
        End Try
    End Function

    Private Function GetPartialRetirementDatatable(ByVal parameterdatatable As DataTable)
        Dim drPartialdata As DataRow
        Dim drNewPartialdata As DataRow
        Dim dtPartialDataTable As New DataTable
        If Not parameterdatatable Is Nothing Then
            dtPartialDataTable = parameterdatatable.Clone

            For intcount As Integer = 0 To parameterdatatable.Rows.Count - 1
                drPartialdata = parameterdatatable.Rows(intcount)
                drNewPartialdata = dtPartialDataTable.NewRow
                If drPartialdata("PlanType").ToString() <> "SAVINGS" Then
                    If drPartialdata("PlanType").ToString() = String.Empty Then
                        dtPartialDataTable.Rows.Add(drNewPartialdata)
                    Else
                        drNewPartialdata("AccountType") = drPartialdata("AccountType")
                        drNewPartialdata("YmcaID") = drPartialdata("YmcaID")
                        drNewPartialdata("IsBasicAccount") = drPartialdata("IsBasicAccount")
                        drNewPartialdata("Taxable") = Math.Round((drPartialdata("Taxable") * Me.NumPercentageFactorofMoneyRetirement), 2)
                        drNewPartialdata("Non-Taxable") = Math.Round((drPartialdata("Non-Taxable") * Me.NumPercentageFactorofMoneyRetirement), 2)
                        drNewPartialdata("Interest") = Math.Round((drPartialdata("Interest") * Me.NumPercentageFactorofMoneyRetirement), 2)
                        'changed the way the code was written to reduce the rounding issue- Amit 09-09-2009
                        'drNewPartialdata("Emp.Total") = Math.Round((drPartialdata("Emp.Total") * Me.NumPercentageFactorofMoneyRetirement), 2)
                        drNewPartialdata("Emp.Total") = drNewPartialdata("Taxable") + drNewPartialdata("Non-Taxable") + drNewPartialdata("Interest")
                        drNewPartialdata("YMCATaxable") = Math.Round((drPartialdata("YMCATaxable") * Me.NumPercentageFactorofMoneyRetirement), 2)
                        drNewPartialdata("YMCAInterest") = Math.Round((drPartialdata("YMCAInterest") * Me.NumPercentageFactorofMoneyRetirement), 2)
                        'drNewPartialdata("YMCATotal") = Math.Round((drPartialdata("YMCATotal") * Me.NumPercentageFactorofMoneyRetirement), 2)
                        'drNewPartialdata("Total") = Math.Round((drPartialdata("Total") * Me.NumPercentageFactorofMoneyRetirement), 2)
                        drNewPartialdata("YMCATotal") = drNewPartialdata("YMCATaxable") + drNewPartialdata("YMCAInterest")
                        drNewPartialdata("Total") = drNewPartialdata("Emp.Total") + drNewPartialdata("YMCATotal")
                        drNewPartialdata("PlanType") = drPartialdata("PlanType")
                        drNewPartialdata("AccountGroup") = drPartialdata("AccountGroup")
                        ' drNewPartialdata("Selected") = True 'drPartialdata("Selected")
                        'changed the way the code was written to reduce the rounding issue- Amit 09-09-2009
                        dtPartialDataTable.Rows.Add(drNewPartialdata)
                    End If

                End If
            Next
            Return dtPartialDataTable
        End If
    End Function

    Private Function GetPartialVOlDatatable(ByVal parameterdatatable As DataTable, ByVal parameterbool_IncludeRetirementAccounts As Boolean, ByVal parameterbool_IncludeSavingsAccounts As Boolean, ByVal parameterNumFactor As Decimal)
        Dim drPartialdata As DataRow
        Dim drNewPartialdata As DataRow
        Dim dtPartialDataTable As New DataTable
        If Not parameterdatatable Is Nothing Then
            dtPartialDataTable = parameterdatatable.Clone

            For intcount As Integer = 0 To parameterdatatable.Rows.Count - 1
                drPartialdata = parameterdatatable.Rows(intcount)
                drNewPartialdata = dtPartialDataTable.NewRow

                If drPartialdata("PlanType").ToString() = String.Empty Then
                    dtPartialDataTable.Rows.Add(drNewPartialdata)

                ElseIf (drPartialdata("PlanType").ToString().ToUpper() = "RETIREMENT" And parameterbool_IncludeRetirementAccounts = True) Or (drPartialdata("PlanType").ToString().ToUpper() = "SAVINGS" And parameterbool_IncludeSavingsAccounts = True) Then
                    drNewPartialdata("AccountType") = drPartialdata("AccountType")
                    drNewPartialdata("IsBasicAccount") = drPartialdata("IsBasicAccount")
                    drNewPartialdata("Taxable") = Math.Round((drPartialdata("Taxable") * parameterNumFactor), 2)
                    drNewPartialdata("Non-Taxable") = Math.Round((drPartialdata("Non-Taxable") * parameterNumFactor), 2)
                    drNewPartialdata("Interest") = Math.Round((drPartialdata("Interest") * parameterNumFactor), 2)
                    'changed the way the code was written to reduce the rounding issue- Amit 09-09-2009
                    'drNewPartialdata("Total") = Math.Round((drPartialdata("Total") * parameterNumFactor), 2)
                    drNewPartialdata("Total") = drNewPartialdata("Taxable") + drNewPartialdata("Non-Taxable") + drNewPartialdata("Interest")
                    'changed the way the code was written to reduce the rounding issue-Amit 09-09-2009
                    drNewPartialdata("PlanType") = drPartialdata("PlanType")
                    drNewPartialdata("AccountGroup") = drPartialdata("AccountGroup")
                    drNewPartialdata("Selected") = drPartialdata("Selected")
                    dtPartialDataTable.Rows.Add(drNewPartialdata)
                End If

            Next
            Return dtPartialDataTable
        End If
    End Function

    Private Function GetPartialSavingsAllDatatable(ByVal parameterdatatable As DataTable)
        Dim drPartialdata As DataRow
        Dim drNewPartialdata As DataRow
        Dim dtPartialDataTable As New DataTable
        If Not parameterdatatable Is Nothing Then
            dtPartialDataTable = parameterdatatable.Clone

            For intcount As Integer = 0 To parameterdatatable.Rows.Count - 1
                drPartialdata = parameterdatatable.Rows(intcount)
                drNewPartialdata = dtPartialDataTable.NewRow
                If drPartialdata("PlanType").ToString() = "SAVINGS" Then
                    If drPartialdata("PlanType").ToString() <> String.Empty Then
                        '    dtPartialDataTable.Rows.Add(drNewPartialdata)
                        'Else
                        drNewPartialdata("IsBasicAccount") = drPartialdata("IsBasicAccount")
                        drNewPartialdata("AccountType") = drPartialdata("AccountType")
                        drNewPartialdata("Taxable") = Math.Round((drPartialdata("Taxable") * Me.NumPercentageFactorofMoneySavings), 2)
                        drNewPartialdata("Non-Taxable") = Math.Round((drPartialdata("Non-Taxable") * Me.NumPercentageFactorofMoneySavings), 2)
                        drNewPartialdata("Interest") = Math.Round((drPartialdata("Interest") * Me.NumPercentageFactorofMoneySavings), 2)
                        drNewPartialdata("PlanType") = drPartialdata("PlanType")
                        drNewPartialdata("AccountGroup") = drPartialdata("AccountGroup")
                        'Modified the way of the code written to reduce the rounding issue-Amit 09-09-2009
                        'drNewPartialdata("Total") = Math.Round((drPartialdata("Total") * Me.NumPercentageFactorofMoneySavings), 2)
                        drNewPartialdata("Total") = drNewPartialdata("Taxable") + drNewPartialdata("Non-Taxable") + drNewPartialdata("Interest")
                        'Modified the way of the code written to reduce the rounding issue-Amit 09-09-2009
                        drNewPartialdata("Selected") = True 'drPartialdata("Selected")
                        dtPartialDataTable.Rows.Add(drNewPartialdata)
                    End If

                End If
            Next
            Return dtPartialDataTable
        End If
    End Function

    Private Function CalculateTotalForDisplay(ByVal parameterCalledBy As String)

        Dim l_CalculatedDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_BooleanFlag As Boolean = False
        Dim l_DataRows As DataRow()

        Try
            If parameterCalledBy = "IsRetirement" Then
                If Not Session("Calculated_DisplayRetirementPlanAcctContribution_C19") Is Nothing Then
                    l_CalculatedDataTable = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable)
                    'Code Added by Dinesh on 30/07/2013
                    l_CalculatedDataTable = objRefundRequest.CalculateTotalForDisplay(l_CalculatedDataTable, True)
                End If
            ElseIf parameterCalledBy = "IsSavings" Then
                If Not Session("Calculated_DisplaySavingsPlanAcctContribution_C19") Is Nothing Then
                    l_CalculatedDataTable = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable)
                    'Code Added by Dinesh on 30/07/2013
                    'Start: Dinesh Kanojia(DK)     2013.07.14      BT-1502: YRS 5.0-1746:Include partial withdrawals 
                    'Added two parameter for partial withdrawal
                    '2013.10.23        Dinesh Kanojia(DK)   BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                    'Remove two parameter for partia withdrawal
                    'l_CalculatedDataTable = objRefundRequest.CalculateTotalForDisplay(l_CalculatedDataTable, True, CheckboxPartialSavings.Checked, TextBoxTerminated.Text.Trim.Trim.ToUpper)
                    l_CalculatedDataTable = objRefundRequest.CalculateTotalForDisplay(l_CalculatedDataTable, True)
                End If
            ElseIf parameterCalledBy = "IsConsolidated" Then
                If Not Session("CalculatedDataTableDisplay_C19") Is Nothing Then
                    l_CalculatedDataTable = DirectCast(Session("CalculatedDataTableDisplay_C19"), DataTable)
                    'Code Added by Dinesh on 30/07/2013
                    l_CalculatedDataTable = objRefundRequest.CalculateTotalForDisplay(l_CalculatedDataTable, True)
                End If
            End If
            'CheckboxPartialSavings.Checked, TextBoxTerminated.Text.Trim.Trim.ToUpper
            'Code commented by Dinesh on 30/07/2013
            'l_CalculatedDataTable = objRefundRequest.CalculateTotalForDisplay(l_CalculatedDataTable, True)

            If Not l_CalculatedDataTable Is Nothing Then

                ' If the field Total is already exist in the Table then Delete the Row.
                If parameterCalledBy = "IsConsolidated" Then
                    l_DataRows = l_CalculatedDataTable.Select("AccountType = 'Total'")
                    If l_DataRows.Length > 0 Then
                        Me.FillValuesToControls(l_DataRows(0))
                    End If
                End If


                'Comment code Deleted By Sanjeev on 06/02/2012

                If parameterCalledBy = "IsRetirement" Then
                    Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = l_CalculatedDataTable
                    If Me.RefundType = "PART" Then
                    Else
                        Me.RetirementPlan_TotalAmount = Me.TotalAmount
                        Me.RetirementPlan_PersonTotalAmount = Me.PersonTotalAmount
                    End If
                ElseIf parameterCalledBy = "IsSavings" Then
                    Session("Calculated_DisplaySavingsPlanAcctContribution_C19") = l_CalculatedDataTable
                    'Added an if condition to restrict the  session SavingsPlan_TotalAmount  to get updated everytime
                    If Me.RefundType = "PART" Then
                    Else
                        Me.SavingsPlan_TotalAmount = Me.TotalAmount
                        Me.SavingsPlan_PersonTotalAmount = Me.PersonTotalAmount
                    End If
                    'Comment code Deleted By Sanjeev on 06/02/2012


                ElseIf parameterCalledBy = "IsConsolidated" Then
                    Session("CalculatedDataTableDisplay_C19") = l_CalculatedDataTable
                    Me.TotalAmount = Me.RetirementPlan_TotalAmount + Me.SavingsPlan_TotalAmount
                End If

            End If
        Catch
            Throw
        End Try

    End Function

    Private Function CheckForDistributionDate() As Boolean

        Dim l_DataTable As DataTable = Nothing
        Dim l_DataRow As DataRow
        Dim l_DateTime As DateTime
        Dim l_TerminationDate As DateTime

        Try

            l_DataTable = DirectCast(Session("Member Employment_C19"), DataTable)

            If l_DataTable Is Nothing Then Return False

            For Each l_DataRow In l_DataTable.Rows

                If Not l_DataRow("TermDate").GetType.ToString = "System.DBNull" Then

                    If DateTime.Compare(l_TerminationDate, CType(l_DataRow("TermDate"), DateTime)) < 0 Then
                        l_TerminationDate = CType(l_DataRow("TermDate"), DateTime)
                    End If

                End If

            Next

            If Now.Year >= l_TerminationDate.Year And Now.Month > 3 And Now.Day > 31 Then
                Return True
            Else
                Return False
            End If


        Catch
            Throw
        End Try

    End Function

    Private Function GetAddressID() As Integer

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable

        Dim l_DataRow As DataRow

        Try

            l_DataSet = Session("PersonInformation_C19")

            If Not l_DataSet Is Nothing Then


                l_DataTable = l_DataSet.Tables("Member Address")

                If Not l_DataTable Is Nothing Then

                    For Each l_DataRow In l_DataTable.Rows
                        If l_DataRow("AddressHistoryID").GetType.ToString <> "System.DBNull" Then
                            If CType(l_DataRow("AddressHistoryID"), Integer) > 0 Then
                                Return (CType(l_DataRow("AddressHistoryID"), Integer))
                            End If
                        Else
                            Return 0
                        End If
                    Next

                End If

                Return 0

            End If

        Catch
            Throw
        End Try
    End Function

    Private Function GetDataRow(ByVal parameterAccountBreakDownType As String, ByVal parameterSortOrder As Integer, ByVal parameterDataTable As DataTable) As DataRow

        Dim l_QueryString As String
        Dim l_FoundRow As DataRow()
        Try
            If Not parameterDataTable Is Nothing Then
                l_QueryString = "AcctBreakDownType = '" & parameterAccountBreakDownType.Trim.ToUpper & "' AND SortOrder = " & parameterSortOrder

                l_FoundRow = parameterDataTable.Select(l_QueryString)

                If l_FoundRow.Length > 0 Then
                    Return l_FoundRow(0)
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        Catch
            Throw
        End Try
    End Function

    Private Function GetDataRow(ByVal parameterAccountBreakDownType As String, ByVal parameterAcctType As String, ByVal parameterDataTable As DataTable) As DataRow

        Dim l_QueryString As String
        Dim l_FoundRow As DataRow()
        Try
            If Not parameterDataTable Is Nothing Then
                l_QueryString = "AcctBreakDownType = '" & parameterAccountBreakDownType.Trim.ToUpper & "' AND AcctType = '" & parameterAcctType & "'"

                l_FoundRow = parameterDataTable.Select(l_QueryString)

                If l_FoundRow.Length > 0 Then
                    Return l_FoundRow(0)
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        Catch
            Throw
        End Try
    End Function

    Private Function ExportReportToFile(ByVal p_StringReportName As String, ByVal p_StringDestFilePath As String, ByVal p_StringUniqueId As String)
        '*******************************************************************************
        ' Called From       :	RefundRequestWebForm 
        ' Author Name		:	Mohammed Hafiz
        ' Employee ID		:	33284
        ' Email				:	hafiz.rehman@3i-infotech.com
        ' Creation Time		:	01/13/2006
        ' Description		:	
        '*******************************************************************************
        Dim l_string_filename As String
        Dim l_string_filepath As String
        Dim l_string_reportpath As String
        Dim crCon As New ConnectionInfo
        Dim CrTableLogonInfo As New TableLogOnInfo

        Dim CrTables As Tables
        Dim CrTable As Table

        Dim l_string_DataSource As String
        Dim l_string_DatabaseName As String
        Dim l_string_UserID As String
        Dim l_string_Password As String

        Try

            'getting all the database related configuration details
            l_string_DataSource = ConfigurationSettings.AppSettings("DataSource")
            l_string_DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            l_string_UserID = ConfigurationSettings.AppSettings("UserID")
            l_string_Password = ConfigurationSettings.AppSettings("Password")

            'getting the report folder path

            l_string_reportpath = System.Configuration.ConfigurationSettings.AppSettings("ReportPath")
            l_string_filepath = l_string_reportpath.Trim() & "\\" & p_StringReportName

            'Comment code Deleted By Sanjeev on 06/02/2012


            'loading/opening the report.
            Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
            objRpt.Load(l_string_filepath)

            Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = objRpt.ParameterFields
            Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
            Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
            Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue.Value = p_StringUniqueId.ToString()

            curValues.Add(discreteValue)
            objRpt.ParameterFields.Item(0).CurrentValues = curValues

            CrTables = objRpt.Database.Tables

            crCon.ServerName = l_string_DataSource
            crCon.DatabaseName = l_string_DatabaseName
            crCon.UserID = l_string_UserID
            crCon.Password = l_string_Password

            For Each CrTable In CrTables
                CrTableLogonInfo = CrTable.LogOnInfo
                CrTableLogonInfo.ConnectionInfo = crCon
                CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                CrTable.Location = l_string_DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
            Next

            objRpt.ExportToDisk(ExportFormatType.PortableDocFormat, p_StringDestFilePath)


            objRpt.Close()
            objRpt.Dispose()
            objRpt = Nothing
            GC.Collect()

            'Comment code Deleted By Sanjeev on 06/02/2012

        Catch
            Throw
        End Try
    End Function

    Private Function ExportReportToFile_1(ByVal p_StringReportName As String, ByVal p_StringDestFilePath As String, ByVal p_StringUniqueId As String, ByVal p_double_Mintax As Double)
        '*******************************************************************************
        ' Called From       :	RefundRequestWebForm 
        ' Author Name		:	Mohammed Hafiz
        ' Employee ID		:	33284
        ' Email				:	hafiz.rehman@3i-infotech.com
        ' Creation Time		:	01/13/2006
        ' Description		:	
        '*******************************************************************************
        Dim l_string_filename As String
        Dim l_string_filepath As String
        Dim l_string_reportpath As String
        Dim crCon As New ConnectionInfo
        Dim CrTableLogonInfo As New TableLogOnInfo

        Dim CrTables As Tables
        Dim CrTable As Table

        Dim l_string_DataSource As String
        Dim l_string_DatabaseName As String
        Dim l_string_UserID As String
        Dim l_string_Password As String

        Try

            'getting all the database related configuration details
            l_string_DataSource = ConfigurationSettings.AppSettings("DataSource")
            l_string_DatabaseName = ConfigurationSettings.AppSettings("DatabaseName")
            l_string_UserID = ConfigurationSettings.AppSettings("UserID")
            l_string_Password = ConfigurationSettings.AppSettings("Password")

            'getting the report folder path

            l_string_reportpath = System.Configuration.ConfigurationSettings.AppSettings("ReportPath")
            l_string_filepath = l_string_reportpath.Trim() & "\\" & p_StringReportName

            'Comment code Deleted By Sanjeev on 06/02/2012


            'loading/opening the report.
            Dim objRpt As New CrystalDecisions.CrystalReports.Engine.ReportDocument
            objRpt.Load(l_string_filepath)

            Dim paramFieldsCollection As CrystalDecisions.Shared.ParameterFields = objRpt.ParameterFields
            Dim paramField As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(0)
            Dim curValues As CrystalDecisions.Shared.ParameterValues = paramField.CurrentValues
            Dim discreteValue As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue.Value = p_StringUniqueId.ToString()

            curValues.Add(discreteValue)
            objRpt.ParameterFields.Item(0).CurrentValues = curValues

            Dim paramField1 As CrystalDecisions.Shared.ParameterField = paramFieldsCollection(1)
            Dim curValues1 As CrystalDecisions.Shared.ParameterValues = paramField1.CurrentValues
            Dim discreteValue1 As CrystalDecisions.Shared.ParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            discreteValue1.Value = p_double_Mintax

            curValues1.Add(discreteValue1)
            objRpt.ParameterFields.Item(1).CurrentValues = curValues1

            CrTables = objRpt.Database.Tables

            crCon.ServerName = l_string_DataSource
            crCon.DatabaseName = l_string_DatabaseName
            crCon.UserID = l_string_UserID
            crCon.Password = l_string_Password

            For Each CrTable In CrTables
                CrTableLogonInfo = CrTable.LogOnInfo
                CrTableLogonInfo.ConnectionInfo = crCon
                CrTable.ApplyLogOnInfo(CrTableLogonInfo)
                CrTable.Location = l_string_DatabaseName & ".dbo." & CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)
            Next

            objRpt.ExportToDisk(ExportFormatType.PortableDocFormat, p_StringDestFilePath)

            objRpt.Close()
            objRpt.Dispose()
            objRpt = Nothing
            GC.Collect()
            'Comment code Deleted By Sanjeev on 06/02/2012

        Catch
            Throw
        End Try
    End Function

    Private Function GetVignettePath() As String
        Dim l_string_ServerName As String
        Dim l_string_FilePath As String
        Try
            l_string_ServerName = YMCARET.YmcaBusinessObject.RefundRequest.GetServerName()
            l_string_FilePath = YMCARET.YmcaBusinessObject.RefundRequest.GetVignettePath(l_string_ServerName)
            If Right(l_string_FilePath, 1) = "\" Then
                l_string_FilePath = l_string_FilePath & "participant\"
            Else
                l_string_FilePath = l_string_FilePath & "\participant\"
            End If
            GetVignettePath = l_string_FilePath
        Catch
            Throw
        End Try
    End Function


    'BY Aparna YREN-3660

    Public Function CallReports() As String
        Dim sRefundType As String = String.Empty
        Dim l_stringDocType As String = String.Empty
        Dim l_StringReportName As String = String.Empty
        Dim l_ArrListParamValues As New ArrayList
        Dim l_double_totalamtforreleaseblnk As Double = 0.0

        Dim l_string_OutputFileType As String = String.Empty

        Dim l_DataTable_RefRequest As DataTable
        Dim l_DataTable_RefRequestDetail As DataTable
        Dim l_Double_OtherAmount As Double

        Dim l_RefRequestRow As DataRow
        Dim l_String_HoldMe As String
        Dim l_String_RefRequestPK As String

        Dim l_Double_TaxAmountHold As Double
        Dim l_Double_TempTax As Double
        Dim l_Double_RecordHold As Double
        Dim l_Bool_Flag As Boolean

        Dim l_DataRow As DataRow

        Dim l_StringDocFileName As String
        Dim l_StringIndexFileName As String
        Dim l_StringDestFilePath As String
        Dim l_StringDestReportPath As String
        Dim DataTable_IdxDetails As New DataTable
        Dim l_StringStaffFL, l_StringSLevel, l_StringTabCode As String

        Dim l_StringErrorMessage As String

        Dim l_tmpTax As Double

        Dim drFileRow As DataRow
        Dim l_StringVignettePath As String
        Dim l_stringRefRequestid As String = String.Empty

        Dim intCtr As Integer = -3

        Dim IDM As IDMforAll
        Dim RefRequestIds As ArrayList
        Dim l_Dataset As DataSet
        Dim l_string_RefundType As String
        Dim l_string_ProcessId As String
        Dim l_double_PersonAge As Double
        Dim bool_AllowMessage As Boolean = False
        Try
            IDM = New IDMforAll   'added by hafiz on 4-May-2007


            Session("R_ReportToLoad_C19") = Nothing
            Session("R_ReportToLoad_1_C19") = Nothing
            Session("R_ReportToLoad_2_C19") = Nothing
            'Comment code Deleted By Sanjeev on 06/02/2012

            'create the Datatable -Filelist
            If IDM.DatatableFileList(False) Then
                Session("FTFileList") = IDM.SetdtFileList
            Else
                Throw New Exception("Unable to generate Release Blanks, Could not create dependent table")
            End If
            'Generate Release Blank Report.
            'get Which report should be called-base on the total Amount
            'Total Amount = Funded + Unfunded
            'Incase of Hardship the Account to be considered for Release Blank Report are AP,RP,RT

            RefRequestIds = CType(Session("RefundRequestIDs"), ArrayList)
            'Modified to update the refundtype as per the Plantype
            l_string_RefundType = Me.RefundType
            For Each RefRequestId As String In RefRequestIds
                l_Dataset = YMCARET.YmcaBusinessObject.RefundRequest.GetWithdrawalReportData(RefRequestId)
                'l_string_ProcessId = l_Dataset.Tables(0).Rows(0).Item("ProcessId").ToString()
                l_string_ProcessId = RefRequestId.ToString()
                l_DataTable_RefRequest = Nothing
                l_DataTable_RefRequestDetail = Nothing
                l_DataTable_RefRequest = l_Dataset.Tables("atsRefundRequest")
                l_DataTable_RefRequestDetail = l_Dataset.Tables("atsRefundRequestDetails")
                Me.RefundType = l_Dataset.Tables("atsRefundRequest").Rows(0)("RefundType").ToString().Trim()
                'Commented as per the additional partial request for the reports-Amit 25-09-2009
                'intCtr = intCtr + 3
                'Commented as per the additional partial request for the reports-Amit 25-09-2009

                'Start change by Aparna -YREN-3262 -To check the Hardship Amount
                l_stringDocType = "REFREQST"
                'Modified as per the additional partial request for the reports to display the message if the participant age is more then 70.5 and stop the generating withdrawal letter-Amit 29.06.2009
                If Me.RefundType.Trim() = "PART" Then
                    bool_AllowMessage = True
                End If
                'Modified as per the additional partial request for the reports to display the message if the participant age is more then 70.5 and stop the generating withdrawal letter-Amit 29.06.2009
                If Me.RefundType = "HARD" Then
                    Dim l_dataTable_AccountContribution_NonFunded As New Data.DataTable
                    Dim l_DataRow_nonfunded As Data.DataRow
                    Dim l_decimal_Total As Double = 0
                    If Not Session("l_dataTable_AccountContribution_NonFunded_C19") Is Nothing Then
                        l_dataTable_AccountContribution_NonFunded = DirectCast(Session("l_dataTable_AccountContribution_NonFunded_C19"), DataTable)
                        For Each l_DataRow_nonfunded In l_dataTable_AccountContribution_NonFunded.Rows
                            If l_DataRow_nonfunded("AccountType").GetType.ToString() <> "System.DBNull" Then
                                If (l_DataRow_nonfunded("AccountType").Trim.ToUpper() = "AP") Or (l_DataRow_nonfunded("AccountType").Trim.ToUpper() = "RP") Or (l_DataRow_nonfunded("AccountType").Trim.ToUpper() = "RT") Then
                                    If l_DataRow_nonfunded("Total").GetType.ToString() <> "System.DBNull" Then
                                        l_decimal_Total += l_DataRow_nonfunded("Total")
                                    End If
                                End If
                            End If
                        Next
                        'Session("AccountContribution_NonFunded_Total") = l_decimal_Total
                        Session("AccountContribution_NonFunded_Total_C19") = Session("AccountContribution_NonFunded_Total_C19") + l_decimal_Total
                    End If
                End If
                'End of Change by Aparna -YREN-3262 -21/05/2007

                'Comment code Deleted By Sanjeev on 06/02/2012

                If Me.RefundType.Trim() = "HARD" Then
                    If Not Session("TotalAmtForReleaseBlnk_C19") Is Nothing Then
                        'l_double_totalamtforreleaseblnk = Convert.ToDouble(Session("TotalAmtForReleaseBlnk_C19"))
                        l_double_totalamtforreleaseblnk = l_double_totalamtforreleaseblnk + Convert.ToDouble(Session("TotalAmtForReleaseBlnk_C19"))
                    End If
                Else
                    If Not l_Dataset.Tables("atsRefundRequest").Rows(0)("Amount").ToString() = String.Empty Then
                        'l_double_totalamtforreleaseblnk = Convert.ToDouble(l_Dataset.Tables("atsRefundRequest").Rows(0)("Amount").ToString())
                        l_double_totalamtforreleaseblnk = l_double_totalamtforreleaseblnk + Convert.ToDouble(l_Dataset.Tables("atsRefundRequest").Rows(0)("Amount").ToString())
                    End If
                End If

                If Not Session("AccountContribution_NonFunded_Total_C19") Is Nothing Then
                    'l_double_totalamtforreleaseblnk += Convert.ToDouble(Session("AccountContribution_NonFunded_Total"))
                    l_double_totalamtforreleaseblnk = l_double_totalamtforreleaseblnk + Convert.ToDouble(Session("AccountContribution_NonFunded_Total_C19"))
                End If
            Next

            If l_double_totalamtforreleaseblnk < 1000 Then
                l_StringReportName = "ReleaseBlankLess1K.rpt"
            ElseIf l_double_totalamtforreleaseblnk < 5000 Then
                l_StringReportName = "ReleaseBlank1kto5k.rpt"
            Else
                l_StringReportName = "ReleaseBlankOver5k.rpt"
            End If

            'added by Shubhrata YREN-3118 Mar 2nd,2007
            If l_DataTable_RefRequest Is Nothing Then
                Return False
            End If
            If l_DataTable_RefRequestDetail Is Nothing Then
                Return False
            End If

            l_RefRequestRow = l_DataTable_RefRequest.Rows(0)
            l_stringRefRequestid = l_RefRequestRow("UniqueId")
            l_String_HoldMe = l_RefRequestRow("UniqueId")

            Dim dsPersonDetails As DataSet
            Dim dtMemberDetails As DataTable

            dsPersonDetails = DirectCast(Session("PersonInformation_C19"), DataSet)
            dtMemberDetails = dsPersonDetails.Tables("Member Details")
            l_String_RefRequestPK = dtMemberDetails.Rows(0)("Fund No")
            'modified the if condition to trim the refund type-Amit
            'If l_RefRequestRow("RefundType").ToString.ToUpper() = "HARD" Then
            If l_RefRequestRow("RefundType").ToString.ToUpper().Trim() = "HARD" Then
                If l_double_totalamtforreleaseblnk > 0.0 Then
                    l_Double_TaxAmountHold = l_RefRequestRow("Tax")
                    l_Double_TempTax = 0.0

                    For Each l_DataRow In l_DataTable_RefRequestDetail.Rows
                        'Vipul 16Feb06 - YMCA-1942
                        If l_DataRow("RefRequestsID") <> l_RefRequestRow("UniqueId") Then
                            l_Bool_Flag = False
                        End If
                        'Vipul 16Feb06 - YMCA-1942 
                        If l_Bool_Flag = True Then
                            If l_DataRow("AcctType").ToString() = "TD" Or l_DataRow("AcctType").ToString() = "TM" Then
                                l_Bool_Flag = False
                            End If
                        End If

                        If l_Bool_Flag = True Then
                            l_Double_TempTax = l_Double_TempTax + l_DataRow("PersonalPreTax") + l_DataRow("PersonalInterest") + l_DataRow("YmcaPreTax") + l_DataRow("YmcaInterest")
                        End If
                    Next

                    l_tmpTax = l_Double_TempTax * (20 / 100.0)

                    l_string_OutputFileType = "ReleaseBlank_" & l_stringDocType & "_1"
                    l_ArrListParamValues.Clear()
                    If l_StringReportName = "ReleaseBlankOver5k.rpt" Then
                        'Commented as per the additional partial request for the reports-Amit 25-09-2009
                        'l_ArrListParamValues.Add(CType(l_stringRefRequestid, String).ToString.Trim)
                        l_ArrListParamValues.Add(CType(l_string_ProcessId, String).ToString.Trim)
                        'Commented as per the additional partial request for the reports-Amit 25-09-2009

                        l_ArrListParamValues.Add(CType(l_tmpTax, String).ToString.Trim)
                    Else
                        'Commented as per the additional partial request for the reports-Amit 25-09-2009
                        l_ArrListParamValues.Add(CType(l_string_ProcessId, String).ToString.Trim)
                        'Commented as per the additional partial request for the reports-Amit 25-09-2009
                    End If
                    l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)
                    'Commented as per the additional partial request for the reports-Amit 25-09-2009
                    'PreviewReport(l_StringReportName, l_stringRefRequestid, l_tmpTax, 0 + intCtr)
                    'Commented as per the additional partial request for the reports-Amit 2-09-2009
                    PreviewReport(l_StringReportName, l_string_ProcessId, l_tmpTax, 0)

                End If
            Else

                l_stringDocType = "REFREQST"
                l_string_OutputFileType = "ReleaseBlank_" & l_stringDocType & "_1"

                If l_StringReportName = "ReleaseBlankOver5k.rpt" Then
                    'Commented as per the additional partial request for the reports-Amit 29-09-2009
                    'l_ArrListParamValues.Add(CType(l_stringRefRequestid, String).ToString.Trim)
                    l_ArrListParamValues.Add(CType(l_string_ProcessId, String).ToString.Trim)
                    'Commented as per the additional partial request for the reports-Amit 29-09-2009
                    l_ArrListParamValues.Add(CType(l_tmpTax, String).ToString.Trim)
                Else
                    'Commented as per the additional partial request for the reports-Amit 29-09-2009
                    'l_ArrListParamValues.Add(CType(l_stringRefRequestid, String).ToString.Trim)
                    'Commented as per the additional partial request for the reports-Amit 29-09-2009
                    l_ArrListParamValues.Add(CType(l_string_ProcessId, String).ToString.Trim)
                End If
                l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)
                'Commented as per the additional partial request for the reports-Amit 25-09-2009
                'PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 0 + intCtr)
                'Commented as per the additional partial request for the reports-Amit 25-09-2009
                PreviewReport(l_StringReportName, l_string_ProcessId, 0, 0)

            End If

            l_double_PersonAge = Me.PersonAge

            sRefundType = Me.RefundType.ToString().ToUpper().Trim()


            Select Case sRefundType

                Case "HARD"
                    'Reports to be called
                    'Release Blank -If Account types include AP,RP,RT and TD
                    'Hardship Release Blank -
                    'HWL4 -If account type =Only TD 
                    'Else If Account types include AP,RP,RT also then HWL5
                    If CType(Me.TDAccountAmount, Double) > 0.0 Then
                        l_StringReportName = "HardshipWithDrawalofTDForm.rpt"
                        l_stringDocType = "REFREQST"
                        l_string_OutputFileType = "ReleaseBlank_" & l_stringDocType & "_2"
                        l_ArrListParamValues.Clear()
                        l_ArrListParamValues.Add(CType(l_stringRefRequestid, String).ToString.Trim)
                        'set IDM proporties
                        l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)
                        'calling report
                        'Commented as per the additional partial request for the reports-Amit 25-09-2009
                        'PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1 + intCtr)
                        'Commented as per the additional partial request for the reports-Amit 25-09-2009
                        PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1)

                        'by Aparna YREN-3262 21/05/2007
                        If CType(Session("ReportHWL5_C19"), Boolean) = True Then
                            l_StringReportName = "HWL_5_TDAndOtherAcc.rpt"
                            Session("ReportHWL5_C19") = Nothing
                        Else
                            l_StringReportName = "HWL_4_TDOnly.rpt"
                            Session("ReportHWL5_C19") = Nothing
                        End If

                        l_stringDocType = "REFLTTRS"
                        l_string_OutputFileType = "ReleaseBlank_" & l_stringDocType & "_2a"
                        l_ArrListParamValues.Clear()
                        l_ArrListParamValues.Add(CType(l_stringRefRequestid, String).ToString.Trim)
                        'set IDM proporties
                        l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)
                        'calling report
                        'Commented as per the additional partial request for the reports-Amit 25-09-2009
                        'PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 2 + intCtr)
                        'Commented as per the additional partial request for the reports-Amit 25-09-2009
                        PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 2)
                    End If
                    'Modified as per the additional partial request for the reports to display the message if the participant age is more then 70.5 and stop the generating withdrawal letter-Amit 30.09.2009
                Case "VOL"
                    If bool_AllowMessage = True Then
                        If (Me.PersonAgeatEndofYear < 70.06) Then
                            l_StringReportName = "RL_2_FullorPartialRefund.rpt"
                            l_stringDocType = "REFLTTRS"
                            l_string_OutputFileType = "ReleaseBlank_" & l_stringDocType & "_2"
                            l_ArrListParamValues.Clear()
                            l_ArrListParamValues.Add(CType(l_stringRefRequestid, String).ToString.Trim)

                            'set IDM proporties
                            l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)
                            ' Call the Report show on the Crystal Report Viewer
                            'Commented as per the additional partial request for the reports-Amit 25-09-2009
                            'PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1 + intCtr)
                            'Commented as per the additional partial request for the reports-Amit 25-09-2009
                            PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1)
                            'Same reports to be generated for regular and pers refund types.
                        ElseIf (Me.PersonAgeatEndofYear >= 70.06) Then
                            objRefundRequest.DisplayMessageForLetters = True
                        End If
                    Else
                        l_StringReportName = "RL_2_FullorPartialRefund.rpt"
                        l_stringDocType = "REFLTTRS"
                        l_string_OutputFileType = "ReleaseBlank_" & l_stringDocType & "_2"
                        l_ArrListParamValues.Clear()
                        l_ArrListParamValues.Add(CType(l_stringRefRequestid, String).ToString.Trim)

                        'set IDM proporties
                        l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)
                        ' Call the Report show on the Crystal Report Viewer
                        'Commented as per the additional partial request for the reports-Amit 25-09-2009
                        'PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1 + intCtr)
                        'Commented as per the additional partial request for the reports-Amit 25-09-2009
                        PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1)
                        'Same reports to be generated for regular and pers refund types.
                    End If


                Case "PERS", "REG"

                    ' = (REG or PERS) and person is VESTED and PIA (at termination ) < MaximumPIAAmount and age < 70.5
                    'Refund(Type = (REG Or PERS) And person Is Not VESTED And age < 70.5)
                    If bool_AllowMessage = True Then
                        If (Me.PersonAgeatEndofYear < 70.06) Then
                            l_stringDocType = "REFLTTRS"
                            If CType(Me.TextboxAge.Text, Double) < 70.5 Then
                                'Commented as per the changes for Gemini Issue YRS 5.0-799 -Amit
                                'If Me.IsVested = False Then
                                '    l_StringReportName = "RL_2_FullorPartialRefund.rpt"
                                'ElseIf Me.IsVested = True And CType(Me.TextboxTerminatePIA.Text, Double) < CType(Me.MaximumPIAAmount, Double) Then
                                '    l_StringReportName = "RL_2_FullorPartialRefund.rpt"
                                'Else
                                '    l_StringReportName = "RL_3_FullPIAOver15000.rpt"
                                'End If
                                l_StringReportName = "RL_2_FullorPartialRefund.rpt"
                                'Commented as per the changes for Gemini Issue YRS 5.0-799
                            Else
                                If CType(Me.TextboxTerminatePIA.Text, Double) >= CType(Me.MaximumPIAAmount, Double) Then
                                    l_StringReportName = "RL_6a_Over70PIAOver15000.rpt"
                                ElseIf CType(Me.TextboxTerminatePIA.Text, Double) < 5000 Then
                                    l_StringReportName = "RL_6c_Over70PIAUnder5000.rpt"
                                Else
                                    l_StringReportName = "RL_6b_Over70PIA5000-15000.rpt"
                                End If

                            End If
                            l_string_OutputFileType = "ReleaseBlank_" & l_stringDocType & "_2"
                            l_ArrListParamValues.Clear()
                            l_ArrListParamValues.Add(CType(l_stringRefRequestid, String).ToString.Trim)
                            'set IDM proporties
                            l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)

                            '' Call the Report show on the Crystal Report Viewer
                            'Commented as per the additional partial request for the reports-Amit 25-09-2009
                            'PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1 + intCtr)
                            'Commented as per the additional partial request for the reports-Amit 25-09-2009
                            PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1)
                        ElseIf (Me.PersonAgeatEndofYear >= 70.06) Then
                            objRefundRequest.DisplayMessageForLetters = True
                        End If
                    Else

                        l_stringDocType = "REFLTTRS"
                        If CType(Me.TextboxAge.Text, Double) < 70.5 Then
                            'Commented as per the changes for Gemini Issue YRS 5.0-799 -Amit
                            'If Me.IsVested = False Then
                            '    l_StringReportName = "RL_2_FullorPartialRefund.rpt"
                            'ElseIf Me.IsVested = True And CType(Me.TextboxTerminatePIA.Text, Double) < CType(Me.MaximumPIAAmount, Double) Then
                            '    l_StringReportName = "RL_2_FullorPartialRefund.rpt"
                            'Else
                            '    l_StringReportName = "RL_3_FullPIAOver15000.rpt"
                            'End If
                            l_StringReportName = "RL_2_FullorPartialRefund.rpt"
                            'Commented as per the changes for Gemini Issue YRS 5.0-799
                        Else
                            If CType(Me.TextboxTerminatePIA.Text, Double) >= CType(Me.MaximumPIAAmount, Double) Then
                                l_StringReportName = "RL_6a_Over70PIAOver15000.rpt"
                            ElseIf CType(Me.TextboxTerminatePIA.Text, Double) < 5000 Then
                                l_StringReportName = "RL_6c_Over70PIAUnder5000.rpt"
                            Else
                                l_StringReportName = "RL_6b_Over70PIA5000-15000.rpt"
                            End If

                        End If
                        l_string_OutputFileType = "ReleaseBlank_" & l_stringDocType & "_2"
                        l_ArrListParamValues.Clear()
                        l_ArrListParamValues.Add(CType(l_stringRefRequestid, String).ToString.Trim)
                        'set IDM proporties
                        l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)

                        '' Call the Report show on the Crystal Report Viewer
                        'Commented as per the additional partial request for the reports-Amit 25-09-2009
                        'PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1 + intCtr)
                        'Commented as per the additional partial request for the reports-Amit 25-09-2009
                        PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1)

                    End If


                    'Comment code Deleted By Sanjeev on 06/02/2012
                Case "PART"
                    l_stringDocType = "REFLTTRS"
                    'Modified as per the additional partial request for the reports to display the message if the participant age is more then 70.5 and stop the generating withdrawal letter-Amit 29.09.2009
                    If (Me.PersonAgeatEndofYear < 70.06) Then
                        'Modified as per the additional partial request for the reports to display the message if the participant age is more then 70.5 and stop the generating withdrawal letter-Amit 29.09.2009
                        If CType(Me.TextboxAge.Text, Double) < 70.5 Then
                            l_StringReportName = "RL_2_FullorPartialRefund.rpt"
                        Else
                            If CType(Me.TextboxTerminatePIA.Text, Double) >= CType(Me.MaximumPIAAmount, Double) Then
                                l_StringReportName = "RL_6a_Over70PIAOver15000.rpt"
                            ElseIf CType(Me.TextboxTerminatePIA.Text, Double) < 5000 Then
                                l_StringReportName = "RL_6c_Over70PIAUnder5000.rpt"
                            Else
                                l_StringReportName = "RL_6b_Over70PIA5000-15000.rpt"
                            End If

                        End If
                        l_string_OutputFileType = "ReleaseBlank_" & l_stringDocType & "_2"
                        l_ArrListParamValues.Clear()
                        l_ArrListParamValues.Add(CType(l_stringRefRequestid, String).ToString.Trim)
                        'set IDM proporties
                        l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)

                        '' Call the Report show on the Crystal Report Viewer
                        PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1)
                        'Modified as per the additional partial request for the reports to display the message if the participant age is more then 70.5 and stop the generating withdrawal letter-Amit 29.09.2009
                    ElseIf (Me.PersonAgeatEndofYear >= 70.06) Then
                        objRefundRequest.DisplayMessageForLetters = True
                    End If
                    'Modified as per the additional partial request for the reports to display the message if the participant age is more then 70.5 and stop the generating withdrawal letter-Amit 29.09.2009
                    'Added the case for partial withdrawal request-Amit 30-09-2009
                Case "SPEC"
                    If CType(Me.TextboxAge.Text, Double) < 70.5 Then
                        l_StringReportName = "RL_2_FullorPartialRefund.rpt"
                    Else
                        If CType(Me.TextboxTerminatePIA.Text, Double) >= CType(Me.MaximumPIAAmount, Double) Then
                            l_StringReportName = "RL_6a_Over70PIAOver15000.rpt"
                        ElseIf CType(Me.TextboxTerminatePIA.Text, Double) < 5000 Then
                            l_StringReportName = "RL_6c_Over70PIAUnder5000.rpt"
                        Else
                            l_StringReportName = "RL_6b_Over70PIA5000-15000.rpt"
                        End If
                    End If

                    l_stringDocType = "REFLTTRS"
                    l_string_OutputFileType = "ReleaseBlank_" & l_stringDocType & "_2"
                    l_ArrListParamValues.Clear()
                    l_ArrListParamValues.Add(CType(l_stringRefRequestid, String).ToString.Trim)
                    'set IDM proporties
                    l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)
                    ' Call the Report 
                    'Commented as per the additional partial request for the reports-Amit 25-09-2009
                    'PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1 + intCtr)
                    'Commented as per the additional partial request for the reports-Amit 25-09-2009
                    PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1)

                Case "DISAB"
                    l_StringReportName = "RL_2_FullorPartialRefund.rpt"
                    l_stringDocType = "REFLTTRS"
                    l_string_OutputFileType = "ReleaseBlank_" & l_stringDocType & "_2"
                    l_ArrListParamValues.Clear()
                    l_ArrListParamValues.Add(CType(l_stringRefRequestid, String).ToString.Trim)
                    'set IDM proporties
                    l_StringErrorMessage = Me.SetPropertiesForIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)
                    ' Call the Report 
                    'Commented as per the additional partial request for the reports-Amit 25-09-2009
                    'PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1 + intCtr)
                    'Commented as per the additional partial request for the reports-Amit 25-09-2009
                    PreviewReport(l_StringReportName, l_stringRefRequestid, 0, 1)
            End Select

            CallReports = l_StringErrorMessage

            Me.RefundType = l_string_RefundType
        Catch ex As Exception
            'by Aparna 27/09/2007
            'Shubhrata--bug id 388, to correct spelling, May 19th,2008  
            HelperFunctions.LogException("RefundRequestWebForm_C19-CallReports", ex)
            Dim l_string_message As String = "Error Occured while generating reports"
            Session("GenerateErrors_C19") = "Error: " + l_string_message
            CallReports = l_string_message

        End Try
    End Function
    'Modified as per the additional partial request for the reports to display the message if the participant age is more then 70.5-Amit 29.09.2009


    'by Aparna 03/07/2007 
    'To change the calling of the Reports.


    Private Function SetPropertiesForIDM(ByVal l_StringDocType As String, ByVal l_StringReportName As String, ByVal l_ArrListParamValues As ArrayList, ByVal l_string_OutputFileType As String) As String
        Dim IDM As IDMforAll
        Dim l_StringErrorMessage As String = String.Empty

        Try
            If IDM Is Nothing Then
                IDM = New IDMforAll
            End If
            'Assign values to the properties
            IDM.PreviewReport = True
            IDM.LogonToDb = True
            IDM.CreatePDF = True
            IDM.CreateIDX = True
            IDM.CopyFilesToIDM = True
            IDM.AppType = "P"


            If Not Session("PersonID") Is Nothing Then
                IDM.PersId = DirectCast(Session("PersonID"), String)
            End If

            If Not Session("FTFileList") Is Nothing Then
                IDM.SetdtFileList = DirectCast(Session("FTFileList"), DataTable)
            End If
            'added by hafiz on 4-May-2007


            IDM.DocTypeCode = l_StringDocType
            IDM.OutputFileType = l_string_OutputFileType
            IDM.ReportName = l_StringReportName
            IDM.ReportParameters = l_ArrListParamValues

            l_StringErrorMessage = IDM.ExportToPDF()

            l_ArrListParamValues.Clear()

            Session("FTFileList") = IDM.SetdtFileList
            IDM = Nothing 'added by hafiz on 4-May-2007

        Catch
            Throw
        End Try
    End Function

    'Comment code Deleted By Sanjeev on 06/02/2012

    Private Function PreviewReport(ByVal p_string_ReportName As String, ByVal p_String_RefRequestsID As String, ByVal p_minTax As Double, ByVal int_Level As Integer) As Boolean
        Dim popupScript As String
        Try

            Select Case int_Level
                Case 0
                    Session("strReportName") = Left(p_string_ReportName, p_string_ReportName.IndexOfAny("."))
                    Session("RefRequestsID") = p_String_RefRequestsID
                    Session("R_minTax") = p_minTax
                    Session("R_ReportToLoad_C19") = True

                    'Comment code Deleted By Sanjeev on 06/02/2012
                Case 1
                    Session("strReportName_1") = Left(p_string_ReportName, p_string_ReportName.IndexOfAny("."))
                    Session("RefRequestsID_1") = p_String_RefRequestsID
                    Session("R_minTax_1") = p_minTax
                    Session("R_ReportToLoad_1_C19") = True

                    'Comment code Deleted By Sanjeev on 06/02/2012
                Case 2
                    Session("strReportName_2") = Left(p_string_ReportName, p_string_ReportName.IndexOfAny("."))
                    Session("RefRequestsID_2") = p_String_RefRequestsID
                    Session("R_minTax_2") = p_minTax
                    Session("R_ReportToLoad_2_C19") = True
                    'Comment code Deleted By Sanjeev on 06/02/2012
                Case 3
                    Session("strReportName_3") = Left(p_string_ReportName, p_string_ReportName.IndexOfAny("."))
                    Session("RefRequestsID_3") = p_String_RefRequestsID
                    Session("R_minTax_3") = p_minTax
                    Session("R_ReportToLoad_3") = True
                    'Comment code Deleted By Sanjeev on 06/02/2012
                Case 4
                    Session("strReportName_4") = Left(p_string_ReportName, p_string_ReportName.IndexOfAny("."))
                    Session("RefRequestsID_4") = p_String_RefRequestsID
                    Session("R_minTax_4") = p_minTax
                    Session("R_ReportToLoad_4") = True

                    'Comment code Deleted By Sanjeev on 06/02/2012
                Case 5
                    Session("strReportName_5") = Left(p_string_ReportName, p_string_ReportName.IndexOfAny("."))
                    Session("RefRequestsID_5") = p_String_RefRequestsID
                    Session("R_minTax_5") = p_minTax
                    Session("R_ReportToLoad_5") = True
                    'Comment code Deleted By Sanjeev on 06/02/2012
            End Select

        Catch
            Throw
        End Try

    End Function

    'by Aparna -YREN-3197 16/04/2007
    Private Function GetSourceFilePath(ByVal p_boolParticipant As Boolean) As String
        Dim l_string_ServerName As String
        Dim l_string_FilePath As String
        Try
            ' l_string_ServerName = YMCARET.YmcaBusinessObject.RefundRequest.GetServerName()
            l_string_FilePath = ConfigurationSettings.AppSettings("IDMPath")
            If p_boolParticipant = True Then
                If Right(l_string_FilePath, 2) = "\\" Then
                    l_string_FilePath = l_string_FilePath.Trim() & "PARTICIPANT\\"
                Else
                    If Right(l_string_FilePath, 1) = "\" Then
                        l_string_FilePath = l_string_FilePath.Trim() & "\PARTICIPANT\\"
                    Else
                        l_string_FilePath = l_string_FilePath.Trim() & "\\PARTICIPANT\\"
                    End If
                End If
            Else
                If Right(l_string_FilePath, 2) = "\\" Then
                    l_string_FilePath = l_string_FilePath.Trim() & "YMCA\\"
                Else
                    If Right(l_string_FilePath, 1) = "\" Then
                        l_string_FilePath = l_string_FilePath.Trim() & "\YMCA\\"
                    Else
                        l_string_FilePath = l_string_FilePath.Trim() & "\\YMCA\\"
                    End If
                End If

            End If
            GetSourceFilePath = l_string_FilePath
        Catch
            Throw
        End Try
    End Function
    'by Aparna -YREN-3197 16/04/2007

    'This function is not called anywhere in the page 
    Private Function GetDestinationFolderName() As String
        Dim l_dataset As DataSet
        Dim l_datarow As DataRow
        'Dim l_string_FileName As String
        Dim l_string_FolderName As String
        Try
            l_dataset = YMCARET.YmcaBusinessObject.InterestProcessingBOClass.getMetaOutputFileType("REL_BK")

            If Not l_dataset Is Nothing Then

                If l_dataset.Tables(0).Rows.Count < 1 Then
                    'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Please Add the Record for the Output Directory in AtsMetaOutputFileTypes table to generate report", MessageBoxButtons.Stop)
                    Session("IndexFileWriteError") = "Please Add the Record for the Output Directory in AtsMetaOutputFileTypes table to generate report"
                    GetDestinationFolderName = String.Empty
                    Exit Function
                End If
                l_datarow = l_dataset.Tables(0).Rows(0)

                If (l_datarow("OutputDirectory").GetType.ToString = "System.DBNull") Or CType(l_datarow("OutputDirectory"), String) = "" Then
                    'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Output Directory value not found in the Table", MessageBoxButtons.Stop)
                    Session("IndexFileWriteError") = "Output Directory value not found in the Table"
                    GetDestinationFolderName = String.Empty
                    Exit Function
                End If

                If Not Directory.Exists(CType(l_datarow("OutputDirectory"), String).Trim()) Then
                    'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Directory does not exist for creating report file.", MessageBoxButtons.Stop)
                    Session("IndexFileWriteError") = "Directory does not exist for creating report file."
                    GetDestinationFolderName = String.Empty
                    Exit Function
                End If

                If (l_datarow("FilenamePrefix").GetType.ToString = "System.DBNull") Or CType(l_datarow("FilenamePrefix"), String) = "" Then
                    'l_string_FileName = CType(l_datarow("OutputDirectory"), String).Trim() + "\ReleaseBlank_" + Format(Now, "ddMMMyy_HHmmss") + ".PDF"
                    l_string_FolderName = CType(l_datarow("OutputDirectory"), String).Trim()
                Else
                    'l_string_FileName = CType(l_datarow("OutputDirectory"), String).Trim() + "\" + CType(l_datarow("FilenamePrefix"), String).Trim() + "_" + Format(Now, "ddMMMyy_HHmmss") + ".PDF"
                    l_string_FolderName = CType(l_datarow("OutputDirectory"), String).Trim()
                End If

                'GetDestinationFolderName = l_string_FileName
                GetDestinationFolderName = l_string_FolderName
            Else
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Please Add the Record for the Output Directory in AtsMetaOutputFileTypes table to generate report", MessageBoxButtons.Stop)
                Session("IndexFileWriteError") = "Please Add the Record for the Output Directory in AtsMetaOutputFileTypes table to generate report"
                GetDestinationFolderName = String.Empty
            End If


        Catch ex As Exception
            HelperFunctions.LogException("RefundRequestWebForm_C19-GetDestinationFolderName", ex)
            ' Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
            Throw
        End Try

    End Function

    ' This function is not called anywhere in the page which returns blank value
    Private Function GenerateFileName(ByVal p_StringIndex As String) As String
        Try
            Return "ReleaseBlank_" & p_StringIndex & "_" & Format(Now, "ddMMMyyyy_HHmmss")
        Catch ex As Exception
            HelperFunctions.LogException("RefundRequestWebForm_C19-GenerateFileName", ex)
            Return ""
        End Try
    End Function

    Private Function CreatePersIdxForTower(ByVal p_StringFilePath As String, ByVal p_StringIndexFileName As String, ByVal p_StringFundId As String, ByVal p_StringSSNo As String, ByVal p_StringFirstName As String, ByVal p_StringMiddleName As String, ByVal p_StringLastName As String, ByVal p_StringLevel As String, ByVal p_StringDocCode As String, ByVal p_StringStaffFL As String, ByVal p_StringTabCode As String) As Boolean
        Dim l_StreamWriter As StreamWriter
        Dim l_BoolFileOpened As Boolean

        Try
            'Return True

            If p_StringFilePath.Trim.Length < 1 Then
                Session("IndexFileWriteError") = "Invalid File Path Passed for Index file creation."
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid File Path Passed for Index file creation.", MessageBoxButtons.Stop)
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If

            If p_StringIndexFileName.Trim.Length < 1 Then
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid File Name Passed for Index file creation.", MessageBoxButtons.Stop)
                Session("IndexFileWriteError") = "Invalid File Name Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If

            If p_StringIndexFileName.Trim.Substring(p_StringIndexFileName.Trim.LastIndexOf(".")).ToUpper <> ".IDX" Then
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid File Name Passed for Index file creation.", MessageBoxButtons.Stop)
                Session("IndexFileWriteError") = "Invalid File Name Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If

            If File.Exists(p_StringFilePath & "\\" & p_StringIndexFileName) = True Then
                File.Delete(p_StringFilePath & "\\" & p_StringIndexFileName)
            End If

            l_StreamWriter = File.CreateText(p_StringFilePath & "\\" & p_StringIndexFileName)
            l_BoolFileOpened = True

            If File.Exists(p_StringFilePath & "\\" & p_StringIndexFileName) = False Then
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Index file could not be created.", MessageBoxButtons.Stop, True)
                Session("IndexFileWriteError") = "Index file could not be created."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If

            If p_StringFundId.Trim.Length < 1 Then
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid FundID Number Passed for Index file creation.", MessageBoxButtons.Stop)
                Session("IndexFileWriteError") = "Invalid FundID Number Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            l_StreamWriter.WriteLine("fundid=" & p_StringFundId.Trim)

            If p_StringSSNo.Trim.Length < 1 Then
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid SS Number Passed for Index file creation.", MessageBoxButtons.Stop)
                Session("IndexFileWriteError") = "Invalid SS Number Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            l_StreamWriter.WriteLine("ssn=" & p_StringSSNo.Trim)

            If p_StringFirstName.Trim.Length < 1 Then
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid First Name Passed for Index file creation.", MessageBoxButtons.Stop)
                Session("IndexFileWriteError") = "Invalid First Name Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            l_StreamWriter.WriteLine("fname=" & p_StringFirstName.Trim)
            'Rahul 06Feb06
            If p_StringMiddleName = "System.DBNull" Then
                'Rahul 06Feb06
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid Middle Name Passed for Index file creation.", MessageBoxButtons.Stop)
                Session("IndexFileWriteError") = "Invalid Middle Name Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            l_StreamWriter.WriteLine("mname=" & p_StringMiddleName.Trim)

            If p_StringLastName.Trim.Length < 1 Then
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid Last Name Passed for Index file creation.", MessageBoxButtons.Stop)
                Session("IndexFileWriteError") = "Invalid Last Name Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            l_StreamWriter.WriteLine("lname=" & p_StringLastName.Trim)

            If p_StringLevel.Trim.Length < 1 Then
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid S Level Code Passed for Index file creation.", MessageBoxButtons.Stop)
                Session("IndexFileWriteError") = "Invalid S Level Code Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            l_StreamWriter.WriteLine("slevel=" & p_StringLevel.Trim)

            If p_StringDocCode.Trim.Length < 1 Then
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid Document Code Passed for Index file creation.", MessageBoxButtons.Stop)
                Session("IndexFileWriteError") = "Invalid Document Code Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            l_StreamWriter.WriteLine("doccode=" & p_StringDocCode.Trim)

            If p_StringStaffFL.Trim.Length < 1 Then
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid Staff Flag Code Passed for Index file creation.", MessageBoxButtons.OK, MessageBoxButtons.Stop)
                Session("IndexFileWriteError") = "Invalid Staff Flag Code Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            l_StreamWriter.WriteLine("stafffl=" & p_StringStaffFL.Trim)

            If p_StringTabCode.Trim.Length < 1 Then
                'MessageBox.Show(MessageBoxPlaceHolder, "Release Blank", "Invalid Tab Code Passed for Index file creation.", MessageBoxButtons.Stop)
                Session("IndexFileWriteError") = "Invalid Tab Code Passed for Index file creation."
                If l_BoolFileOpened = True Then l_StreamWriter.Close()
                Return False
            End If
            l_StreamWriter.WriteLine("subtab=" & p_StringTabCode.Trim)

            l_StreamWriter.Close()
            l_BoolFileOpened = False

            Return True
        Catch ex As Exception
            HelperFunctions.LogException("RefundRequestWebForm_C19-CreatePersIdxForTower", ex)
            If l_BoolFileOpened = True Then l_StreamWriter.Close()
            Return False
        End Try
    End Function

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
            HelperFunctions.LogException("RefundRequestWebForm_C19-FormatCurrency", ex)
            Return paramNumber
        End Try

    End Function

    Private Function GetConsolidatedTotal()
        Dim l_dt_GetConsolidatedTotal As DataTable
        Dim l_dt_DisplayRetirementPlan As New DataTable
        Dim l_dt_DisplaySavingsPlan As New DataTable
        Dim l_dr_RetirementPlanTotal As DataRow
        Try
            l_dt_GetConsolidatedTotal = DirectCast(Session("DisplayConsolidatedTotal_C19"), DataTable)
            l_dt_DisplayRetirementPlan = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable)
            l_dt_DisplaySavingsPlan = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable)
            If Not l_dt_GetConsolidatedTotal Is Nothing Then
                For Each dr As DataRow In l_dt_DisplayRetirementPlan.Rows
                    If dr("AccountType").GetType.ToString() <> "System.DBNull" Then
                        If dr("AccountType") = "Total" Then
                            l_dt_GetConsolidatedTotal.Rows(0)("Taxable") = dr("Taxable")
                            l_dt_GetConsolidatedTotal.Rows(0)("Non-Taxable") = dr("Non-Taxable")
                            l_dt_GetConsolidatedTotal.Rows(0)("Interest") = dr("Interest")
                            l_dt_GetConsolidatedTotal.Rows(0)("Total") = dr("Total")
                        End If
                    End If
                Next
                For Each dr As DataRow In l_dt_DisplaySavingsPlan.Rows
                    If dr("AccountType").GetType.ToString() <> "System.DBNull" Then
                        If dr("AccountType") = "Total" Then
                            l_dt_GetConsolidatedTotal.Rows(0)("Taxable") += dr("Taxable")
                            l_dt_GetConsolidatedTotal.Rows(0)("Non-Taxable") += dr("Non-Taxable")
                            l_dt_GetConsolidatedTotal.Rows(0)("Interest") += dr("Interest")
                            l_dt_GetConsolidatedTotal.Rows(0)("Total") += dr("Total")
                        End If
                    End If
                Next
                Me.DatagridConsolidateTotal.DataSource = l_dt_GetConsolidatedTotal
                Me.DatagridConsolidateTotal.DataBind()
                Me.SetSelectedIndex(Me.DatagridConsolidateTotal, l_dt_GetConsolidatedTotal)
            End If

        Catch
            Throw
        End Try
    End Function

    Private Function MakeDisplayCalculationDataTables(ByVal NeedRetirementPlan As Boolean, ByVal NeedSavingsPlan As Boolean)

        Dim l_dt_RetPlanCalculatedDataTable As DataTable
        Dim l_dt_SavingsPlanCalculatedDataTable As DataTable
        Dim l_dt_ConsolidatedDataTable As DataTable
        Dim l_CalculatedDataTable As DataTable
        Dim l_DataRow As DataRow()
        Dim l_BooleanFlag As Boolean = False
        Dim l_Counter As Integer = 0
        Dim dgItem As DataGridItem

        Try
            l_dt_ConsolidatedDataTable = DirectCast(Session("CalculatedDataTableDisplay_C19"), DataTable).Clone()
            If NeedRetirementPlan = True Then
                l_dt_RetPlanCalculatedDataTable = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable)
                If Not l_dt_RetPlanCalculatedDataTable Is Nothing Then
                    For Each dr As DataRow In l_dt_RetPlanCalculatedDataTable.Rows
                        If dr("AccountType").GetType.ToString <> "System.DBNull" Then
                            If dr("AccountType") <> "Total" Then
                                'Phase V Changes Dilip to include total row
                                'If Me.RefundType = "VOL" Then
                                If Me.RefundType = "VOL" Or Me.RefundType = "PERS" Then
                                    If dr("Selected").GetType.ToString() = "System.Boolean" Then
                                        If dr("Selected") = "True" Then
                                            l_dt_ConsolidatedDataTable.ImportRow(dr)
                                        End If
                                    End If
                                Else
                                    l_dt_ConsolidatedDataTable.ImportRow(dr)
                                End If

                            End If

                        End If

                    Next
                End If
            End If
            If NeedSavingsPlan = True Then
                l_dt_SavingsPlanCalculatedDataTable = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable)
                If Not l_dt_SavingsPlanCalculatedDataTable Is Nothing Then
                    For Each dr As DataRow In l_dt_SavingsPlanCalculatedDataTable.Rows
                        If dr("AccountType").GetType.ToString <> "System.DBNull" Then
                            If dr("AccountType") <> "Total" Then
                                If Me.RefundType = "VOL" Or Me.RefundType = "SPEC" Or Me.RefundType = "DISAB" Then
                                    If dr("Selected").GetType.ToString() = "System.Boolean" Then
                                        If dr("Selected") = "True" Then
                                            l_dt_ConsolidatedDataTable.ImportRow(dr)
                                        End If
                                    End If
                                Else
                                    'Added by parveen to solve "TD amount is dispalyed in Total Withdrawal if it is unchecked" on 15-Jan-2010
                                    If dr("Selected").GetType.ToString() = "System.Boolean" Then
                                        If dr("Selected") = "True" Then
                                            l_dt_ConsolidatedDataTable.ImportRow(dr)
                                        End If
                                    End If
                                    'Added by parveen to solve "TD amount is dispalyed in Total Withdrawal if it is unchecked" on 15-Jan-2010
                                End If
                            End If

                        End If
                    Next
                End If
            End If
            l_dt_ConsolidatedDataTable.AcceptChanges()
            l_CalculatedDataTable = objRefundRequest.CalculateTotalForDisplay(l_dt_ConsolidatedDataTable, True)
            l_DataRow = l_CalculatedDataTable.Select("AccountType = 'Total'")
            If l_DataRow.Length > 0 Then
                Me.FillValuesToControls(l_DataRow(0))
            End If
            Session("CalculatedDataTableDisplay_C19") = l_dt_ConsolidatedDataTable
            Me.DatagridConsolidateTotal.DataSource = l_dt_ConsolidatedDataTable
            Me.Session("CalculatedDataTableRetirementPlan_C19") = l_dt_ConsolidatedDataTable
            Me.Session("CalculatedDataTableSavingPlan_C19") = l_dt_ConsolidatedDataTable
            Me.DatagridConsolidateTotal.DataBind()
            BindFinalDatagrid() 'aparna 29/10/2007
        Catch
            Throw
        End Try



    End Function
    '986
    Public Function BindFinalDatagrid()
        Dim l_dt_GrandTotalDatatable As New DataTable
        Dim l_datatable As DataTable
        Dim drow As DataRow
        'Added By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009
        Dim l_dt_RetPlanCalculatedDataTable As DataTable
        Dim l_dt_SavingsPlanCalculatedDataTable As DataTable
        Dim MarketAccountType As String
        Dim l_RetRow As DataRow()
        Dim l_SavingsRow As DataRow()
        Try
            MarketAccountType = "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)"
            'Added By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009
            If Not Session("CalculatedDataTableDisplay_C19") Is Nothing Then
                l_datatable = DirectCast(Session("CalculatedDataTableDisplay_C19"), DataTable)
                If l_datatable.Rows.Count > 0 Then
                    l_dt_GrandTotalDatatable = l_datatable.Clone
                    For Each drow In l_datatable.Rows
                        If drow("AccountType").ToString.ToUpper.Trim() = "TOTAL" Then
                            'Added By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009
                            drow("Taxable") = CType(drow("Taxable"), Decimal) + CType(drow("Interest"), Decimal)
                            'if me.RefundType="HARD" and 

                            'drow("Interest") = Math.Round((CType(drow("Taxable"), Decimal) * (Me. / 100.0)), 2)
                            'drow("Interest") = Math.Round((((drow("Taxable")) * Me.FederalTaxRate) / 100), 2)
                            drow("Interest") = Math.Round(drow("TaxWithheld"), 2)

                            drow("Total") = (((CType(drow("Non-Taxable"), Decimal)) + (CType(drow("Taxable"), Decimal))) - drow("Interest"))
                            l_dt_GrandTotalDatatable.ImportRow(drow)
                            'Added By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009
                        End If
                    Next
                    'Added By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009
                    l_dt_RetPlanCalculatedDataTable = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable)
                    l_dt_SavingsPlanCalculatedDataTable = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable)
                    l_RetRow = l_dt_RetPlanCalculatedDataTable.Select("AccountType='" + MarketAccountType + "'")
                    l_SavingsRow = l_dt_SavingsPlanCalculatedDataTable.Select("AccountType='" + MarketAccountType + "'")
                    If l_RetRow.Length > 0 And l_SavingsRow.Length > 0 Then
                        drow = l_dt_GrandTotalDatatable.NewRow()
                        drow("AccountType") = "First Disbursement"
                        drow("Taxable") = CType(l_RetRow(0)("Taxable"), Decimal) + CType(l_RetRow(0)("Interest"), Decimal) + CType(l_SavingsRow(0)("Taxable"), Decimal) + CType(l_SavingsRow(0)("Interest"), Decimal)
                        drow("Non-Taxable") = CType(l_RetRow(0)("Non-Taxable"), Decimal) + CType(l_SavingsRow(0)("Non-Taxable"), Decimal)
                        'drow("Interest") = Math.Round(CType(drow("Taxable"), Decimal) * (Me.FederalTaxRate / 100.0), 2)
                        'drow("Interest") = Math.Round((((drow("Taxable")) * Me.FederalTaxRate) / 100), 2)

                        drow("Interest") = CType(l_RetRow(0)("TaxWithheld"), Decimal) + CType(l_SavingsRow(0)("TaxWithheld"), Decimal)


                        drow("Total") = CType(drow("Taxable"), Decimal) + CType(drow("Non-Taxable"), Decimal) - CType(drow("Interest"), Decimal)
                        l_dt_GrandTotalDatatable.Rows.Add(drow)
                    ElseIf l_RetRow.Length > 0 Then
                        l_SavingsRow = l_dt_SavingsPlanCalculatedDataTable.Select("AccountType='Total'")
                        drow = l_dt_GrandTotalDatatable.NewRow()
                        drow("AccountType") = "First Disbursement"
                        If (CheckboxSavingsVoluntary.Checked = False And CheckboxHardship.Checked = False And CheckboxPartialSavings.Checked = False) _
                           Or (CheckboxPartialSavings.Checked = True And TextboxPartialSavings.Text.Trim.Length = 0) Then
                            drow("Taxable") = CType(l_RetRow(0)("Taxable"), Decimal) + CType(l_RetRow(0)("Interest"), Decimal)
                            drow("Non-Taxable") = CType(l_RetRow(0)("Non-Taxable"), Decimal)
                        Else
                            drow("Taxable") = CType(l_RetRow(0)("Taxable"), Decimal) + CType(l_RetRow(0)("Interest"), Decimal) + CType(l_SavingsRow(0)("Taxable"), Decimal) + CType(l_SavingsRow(0)("Interest"), Decimal)
                            drow("Non-Taxable") = CType(l_RetRow(0)("Non-Taxable"), Decimal) + CType(l_SavingsRow(0)("Non-Taxable"), Decimal)
                        End If
                        'Modified as per the issue created in market based row 
                        'drow("Interest") = CType(l_SavingsRow(0)("TaxWithheld"), Decimal)
                        drow("Interest") = CType(l_RetRow(0)("TaxWithheld"), Decimal)
                        'Modified as per the issue created in market based row 
                        'Comment code Deleted By Sanjeev on 06/02/2012

                        drow("Total") = CType(drow("Taxable"), Decimal) + CType(drow("Non-Taxable"), Decimal) - CType(drow("Interest"), Decimal)
                        l_dt_GrandTotalDatatable.Rows.Add(drow)
                    ElseIf l_SavingsRow.Length > 0 Then
                        l_RetRow = l_dt_RetPlanCalculatedDataTable.Select("AccountType='Total'")
                        drow = l_dt_GrandTotalDatatable.NewRow()
                        drow("AccountType") = "First Disbursement"
                        If (CheckboxRegular.Checked = False And CheckboxExcludeYMCA.Checked = False And CheckboxVoluntryAccounts.Checked = False And CheckboxPartialRetirement.Checked = False) _
                            Or (CheckboxPartialRetirement.Checked = True And TextboxPartialRetirement.Text.Trim.Length = 0) Then
                            drow("Taxable") = CType(l_SavingsRow(0)("Taxable"), Decimal) + CType(l_SavingsRow(0)("Interest"), Decimal)
                            drow("Non-Taxable") = CType(l_SavingsRow(0)("Non-Taxable"), Decimal)
                        Else
                            drow("Taxable") = CType(l_RetRow(0)("Taxable"), Decimal) + CType(l_RetRow(0)("Interest"), Decimal) + CType(l_SavingsRow(0)("Taxable"), Decimal) + CType(l_SavingsRow(0)("Interest"), Decimal)
                            drow("Non-Taxable") = CType(l_RetRow(0)("Non-Taxable"), Decimal) + CType(l_SavingsRow(0)("Non-Taxable"), Decimal)
                        End If
                        'Comment code Deleted By Sanjeev on 06/02/2012
                        drow("Interest") = Math.Round((((drow("Taxable")) * Me.FederalTaxRate) / 100), 2)
                        'Comment code Deleted By Sanjeev on 06/02/2012
                        drow("Total") = CType(drow("Taxable"), Decimal) + CType(drow("Non-Taxable"), Decimal) - CType(drow("Interest"), Decimal)
                        l_dt_GrandTotalDatatable.Rows.Add(drow)
                    End If
                    'Me.DatagridGrandTotal.Columns(6).HeaderText = "Tax Withheld(" + Me.FederalTaxRate.ToString + "%)"
                    Me.DatagridGrandTotal.Columns(6).HeaderText = "Tax Withheld"
                    'Added By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009
                    l_dt_GrandTotalDatatable.AcceptChanges()
                    Me.DatagridGrandTotal.DataSource = l_dt_GrandTotalDatatable
                    Me.DatagridGrandTotal.DataBind()
                    Me.SetSelectedIndex(DatagridGrandTotal, l_dt_GrandTotalDatatable)
                End If


            End If


        Catch
            Throw
        End Try
    End Function

    Public Function CheckAnnuityExistence()
        Dim l_intAnnuity As Integer
        Dim l_stringMessage As String = String.Empty
        Dim l_boolAnnuityCheck As Boolean = False
        Dim l_Dataset_AnnuityDetails As New DataSet
        Try

            If Not Me.SessionStatusType = String.Empty Then
                If Me.IsTerminated = True Then
                    'by Aparna 14/11/2007
                    'Comment code Deleted By Sanjeev on 06/02/2012
                    l_Dataset_AnnuityDetails = YMCARET.YmcaBusinessObject.RefundRequest.GetAnnuityExistence(Me.PersonID, Me.FundID)
                    Session("AnnuityExists_C19") = l_Dataset_AnnuityDetails
                    'by Aparna 14/11/2007
                End If
            End If


        Catch
            Throw
        End Try
    End Function
    'This function is not being used anymore as checked.Start

    Public Function ValidateAnnuityCheck1(ByVal parameterCalledBy As String) As Boolean
        Dim l_integer_Annuity As Integer
        Dim l_bool_AnnuityCheck As Boolean = False
        Dim l_decimal_RetirementTotal As Decimal
        Dim l_decimal_Savingstotal As Decimal
        Dim l_decimal_balance As Decimal
        Try

            If Not Session("AnnuityExists_C19") Is Nothing Then
                'if 0 then annuity doesnt exist ,if 1 then it exists
                l_integer_Annuity = DirectCast(Session("AnnuityExists_C19"), Integer)
                'if 0 then check if the account balance after withdrawal will be > 5000 or not
                If l_integer_Annuity = 0 Then
                    If parameterCalledBy = "IsRetirement" Then
                        l_decimal_RetirementTotal = CType(Session("RetirementAmountTotal_C19"), Decimal)
                        If Me.RetirementPlan_TotalAmount > 0 Then
                            l_decimal_balance = l_decimal_RetirementTotal - Me.RetirementPlan_TotalAmount
                            If l_decimal_balance > 0 And l_decimal_balance < 5000 Then
                                Return True
                            Else
                                Return False
                            End If
                        Else
                            Return False
                        End If
                    ElseIf parameterCalledBy = "IsSavings" Then
                        l_decimal_Savingstotal = CType(Session("SavingsPlanAmountTotal_C19"), Decimal)
                        If Me.SavingsPlan_TotalAmount > 0 Then
                            l_decimal_balance = l_decimal_Savingstotal - Me.SavingsPlan_TotalAmount
                            If l_decimal_balance > 0 And l_decimal_balance < 5000 Then
                                Return True
                            Else
                                Return False
                            End If
                        Else
                            Return False
                        End If
                    End If
                Else
                    Return False
                End If
            End If

        Catch
            Throw
        End Try
    End Function
    'This function is not being used anymore as checked.End

    Public Function IsAnnuityExists(ByVal parameterCalledBy As String) As Boolean
        Dim l_integer_Annuity As Integer
        Dim l_bool_AnnuityExists As Boolean = True
        Dim l_decimal_RetirementTotal As Decimal
        Dim l_decimal_Savingstotal As Decimal
        Dim l_decimal_balance As Decimal
        Dim l_datasetAnnuityCheck As New DataSet
        Dim l_datarow As DataRow()
        Try


            If Not Session("AnnuityExists_C19") Is Nothing Then
                l_datasetAnnuityCheck = DirectCast(Session("AnnuityExists_C19"), DataSet)
                If l_datasetAnnuityCheck.Tables.Count > 0 Then
                    If l_datasetAnnuityCheck.Tables(0).Rows.Count > 0 Then
                        If parameterCalledBy = "IsRetirement" Then
                            l_datarow = l_datasetAnnuityCheck.Tables(0).Select("chvPlanType='" + "RETIREMENT" + "'")
                            If l_datarow.Length > 0 Then
                                l_bool_AnnuityExists = True
                            End If
                        ElseIf parameterCalledBy = "IsSavings" Then
                            l_datarow = l_datasetAnnuityCheck.Tables(0).Select("chvPlanType='" + "SAVINGS" + "'")
                            If l_datarow.Length > 0 Then
                                l_bool_AnnuityExists = True
                            End If
                        End If
                    End If
                Else
                    l_bool_AnnuityExists = False
                End If
            End If
            Return l_bool_AnnuityExists
        Catch
            Throw
        End Try

    End Function

    Public Function ValidateAnnuityExistsORMinimumPIAtoRetire(ByVal parameterCalledBy As String) As Boolean
        Dim l_integer_Annuity As Integer
        Dim l_bool_AnnuityExists As Boolean = True
        Dim l_decimal_RetirementTotal As Decimal
        Dim l_decimal_Savingstotal As Decimal
        Dim l_decimal_balance As Decimal
        Dim l_datasetAnnuityCheck As New DataSet
        Dim l_datarow As DataRow()
        'Start : AA:05.11.2015 BT-2699 - YRS 5.0-2411: Added to exclude ln and ld accounts while validating
        Dim dtSavingsPlanAccount As DataTable
        Dim isLNLDExists As Boolean
        Dim decLNLDTotal As Decimal
        'End: AA:05.11.2015 BT-2699 - YRS 5.0-2411: Added to exclude ln and ld accounts while validating
        Try

            'Comment code Deleted By Sanjeev on 06/02/2012
            l_bool_AnnuityExists = Me.l_bool_AnnuityExists

            'If the annuity exists then there is no need for the 5000 check else you have to 
            'check whether the balance is > 5000 or not
            If l_bool_AnnuityExists = False And parameterCalledBy = "IsRetirement" Then
                'if the person is vested include the YMCA Amt in the total else dont include
                If Me.IsVested = True Then
                    l_decimal_RetirementTotal = CType(Session("RetirementTotal_C19"), Decimal)
                Else
                    l_decimal_RetirementTotal = CType(Session("RetirementAmountTotal_C19"), Decimal)
                End If
                ' l_decimal_RetirementTotal = CType(Session("RetirementAmountTotal"), Decimal)

                If Me.RetirementPlan_TotalAmount > 0 Then
                    l_decimal_balance = l_decimal_RetirementTotal - Me.RetirementPlan_TotalAmount
                    If l_decimal_balance > 0 And l_decimal_balance <= Me.MinimumPIAToRetire Then
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return False
                End If
            ElseIf l_bool_AnnuityExists = False And parameterCalledBy = "IsSavings" Then
                ' l_decimal_Savingstotal = CType(Session("SavingsPlanAmountTotal_C19"), Decimal)
                'Start : AA:05.11.2015 BT-2699 - YRS 5.0-2411: Added to exclude ln and ld accounts while validating
                dtSavingsPlanAccount = Session("SavingsPlanAcctContribution_C19")
                If dtSavingsPlanAccount.Select("AccountType = 'LN' OR AccountType = 'LD'").Count > 0 Then
                    isLNLDExists = True
                End If
                l_decimal_Savingstotal = CType(Session("SavingsTotal_C19"), Decimal)
                If Me.SavingsPlan_TotalAmount > 0 Then
                    If Not isLNLDExists Then
                        l_decimal_balance = l_decimal_Savingstotal - Me.SavingsPlan_TotalAmount
                    Else
                        decLNLDTotal = dtSavingsPlanAccount.Compute("SUM(Total)", "AccountType = 'LN' OR AccountType = 'LD'")
                        If decLNLDTotal > 0 Then
                            l_decimal_balance = l_decimal_Savingstotal - Me.SavingsPlan_TotalAmount - decLNLDTotal
                        Else
                            l_decimal_balance = l_decimal_Savingstotal - Me.SavingsPlan_TotalAmount
                        End If
                    End If
                    'End: AA:05.11.2015 BT-2699 - YRS 5.0-2411: Added to exclude ln and ld accounts while validating
                    If l_decimal_balance > 0 And l_decimal_balance <= Me.MinimumPIAToRetire Then
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return False
                End If
            Else
                Return l_bool_AnnuityExists
            End If



        Catch
            Throw
        End Try
    End Function

    Public Function CheckTotalsForAnnuity()
        Dim l_datasetAnnuityCheck As New DataSet
        Dim l_datarow As DataRow()
        Dim l_decimal_RetirementTotal As Decimal
        Dim l_decimal_Savingstotal As Decimal
        Dim l_bool_AnnuityNotExists As Boolean = True
        Try
            'check for the existence of the annuity per plan
            Me.CheckAnnuityExistence()
            'If annuity exists then nothing should be done
            'else check if the total amount is per plan <5000
            'if so then the total amount should be withdrawn in each plan
            'Hence setting up the properties in this function

            If Not Session("AnnuityExists_C19") Is Nothing Then
                l_datasetAnnuityCheck = DirectCast(Session("AnnuityExists_C19"), DataSet)
                If l_datasetAnnuityCheck.Tables.Count > 0 Then
                    If l_datasetAnnuityCheck.Tables(0).Rows.Count > 0 Then
                        l_datarow = l_datasetAnnuityCheck.Tables(0).Select("chvPlanType='" + "RETIREMENT" + "'")
                        If l_datarow.Length > 0 Then
                            Me.CompulsoryRetirement = False
                        Else
                            If Me.IsVested = True Then
                                l_decimal_RetirementTotal = CType(Session("RetirementTotal_C19"), Decimal)
                            Else
                                l_decimal_RetirementTotal = CType(Session("RetirementAmountTotal_C19"), Decimal)
                            End If
                            ' l_decimal_RetirementTotal = CType(Session("RetirementTotal"), Decimal)
                            If l_decimal_RetirementTotal > 0 And l_decimal_RetirementTotal < 5000 Then
                                Me.CompulsoryRetirement = True
                            Else
                                Me.CompulsoryRetirement = False
                            End If
                        End If
                        l_datarow = l_datasetAnnuityCheck.Tables(0).Select("chvPlanType='" + "SAVINGS" + "'")
                        If l_datarow.Length > 0 Then
                            Me.CompulsorySavings = False
                        Else
                            l_decimal_Savingstotal = CType(Session("SavingsTotal_C19"), Decimal)
                            If l_decimal_Savingstotal > 0 And l_decimal_Savingstotal < 5000 Then
                                Me.CompulsorySavings = True
                            Else
                                Me.CompulsorySavings = False
                            End If
                        End If

                    End If
                Else
                    'if no Annuity Exists
                    l_decimal_RetirementTotal = CType(Session("RetirementTotal_C19"), Decimal)
                    If l_decimal_RetirementTotal > 0 And l_decimal_RetirementTotal < 5000 Then
                        Me.CompulsoryRetirement = True
                    Else
                        Me.CompulsoryRetirement = False
                    End If
                    l_decimal_Savingstotal = CType(Session("SavingsTotal_C19"), Decimal)
                    If l_decimal_Savingstotal > 0 And l_decimal_Savingstotal < 5000 Then
                        Me.CompulsorySavings = True
                    Else
                        Me.CompulsorySavings = False
                    End If

                End If
            End If

        Catch
            Throw

        End Try
    End Function

    Public Function SetRefundType()
        Try

            'get the refund type based on the checkboxes.

            'If Me.CheckboxRetirementPlan.Checked = True And Me.CheckboxSavingsPlan.Checked = True Then
            If Me.CheckboxSpecial.Checked = True Then
                Me.RefundType = "SPEC"
            ElseIf Me.CheckboxDisability.Checked = True Then
                Me.RefundType = "DISAB"
            ElseIf Me.CheckboxRegular.Checked = False And Me.CheckboxExcludeYMCA.Checked = False And Me.CheckboxVoluntryAccounts.Checked = False And Me.CheckboxSavingsVoluntary.Checked = True Then
                Me.RefundType = "VOL"
                'Added to set the refund type for the partial refund - Amit
            ElseIf Me.CheckboxRegular.Checked = False And Me.CheckboxExcludeYMCA.Checked = False And Me.CheckboxVoluntryAccounts.Checked = False And Me.CheckboxSavingsVoluntary.Checked = False And (Me.CheckboxPartialSavings.Checked = True Or Me.CheckboxPartialRetirement.Checked = True) Then
                Me.RefundType = "PART"
                'Added to set the refund type for the partial refund - Amit
            ElseIf Me.CheckboxRegular.Checked = True And Me.CheckboxSavingsVoluntary.Checked = True Then
                Me.RefundType = "REG"
            ElseIf Me.CheckboxRegular.Checked = True And Me.CheckboxSavingsVoluntary.Checked = False Then
                Me.RefundType = "REG"
            ElseIf Me.CheckboxRegular.Checked = False And Me.CheckboxExcludeYMCA.Checked = False And Me.CheckboxSavingsVoluntary.Checked = True Then
                Me.RefundType = "VOL"
            ElseIf Me.CheckboxRegular.Checked = False And Me.CheckboxExcludeYMCA.Checked = True And Me.CheckboxSavingsVoluntary.Checked = True Then
                Me.RefundType = "PERS"
            ElseIf Me.CheckboxExcludeYMCA.Checked = True And Me.CheckboxSavingsVoluntary.Checked = False Then
                Me.RefundType = "PERS"
            ElseIf Me.CheckboxVoluntryAccounts.Checked = True And Me.CheckboxSavingsVoluntary.Checked = True Then
                Me.RefundType = "VOL"
            ElseIf Me.CheckboxVoluntryAccounts.Checked = True And Me.CheckboxSavingsVoluntary.Checked = False And Me.CheckboxHardship.Checked = False Then
                Me.RefundType = "VOL"
            ElseIf Me.CheckboxVoluntryAccounts.Checked = True And Me.CheckboxHardship.Checked = True Then
                Me.RefundType = "HARD"
            ElseIf Me.CheckboxHardship.Checked = True Then
                Me.RefundType = "HARD"
            End If

            '--START | SR | 2016.07.19 | YRS-AT-3015 | Added below condition to set refund type based on new combined limit rule.
            If (IsBALegacyCombinedAmountSwitchedON) Then
                If ((Me.BACurrent + Me.CurrentPIA) > Me.MaxCombinedBasicAccountAmt) Then '--SR | 2016.08.24 | YRS-AT-3096 | check for combined amount. If combined amount greater than maximum combined amount then check for individual account
                    If (Me.TerminationPIA > Me.MaxYMCALegacyAcctAmt And Me.IsVested = True And Me.CheckboxRegular.Checked = True) Then
                        Me.RefundType = "PERS"
                    End If

                    If (Me.BACurrent > Me.MaxYMCAAcctAmt And Me.IsVested = True And Me.CheckboxRegular.Checked = True) Then
                        Me.RefundType = "PERS"
                    End If
                End If
            Else
                '--END | SR | 2016.07.19 | YRS-AT-3015 | Added below condition to set refund type based on new combined limit rule.
                'Modified by Amit  14-05-2009-Start. Commented the code and changed the condition.
                'If Me.TerminationPIA >= Me.MaximumPIAAmount And Me.IsVested = True And Me.CheckboxRegular.Checked = True Then
                If Me.TerminationPIA > Me.MaximumPIAAmount And Me.IsVested = True And Me.CheckboxRegular.Checked = True Then
                    'Modified by Amit  14-05-2009-End
                    Me.RefundType = "PERS"
                End If

                If Me.BACurrent > Me.BAMaxLimit And Me.IsVested = True And Me.CheckboxRegular.Checked = True Then
                    'Modified by Amit  14-05-2009-End
                    Me.RefundType = "PERS"
                End If
            End If


            'if person is non vested refund type = PERS 'Shubhrata Jan 30th 2008

            If Me.IsVested = False And CheckboxRegular.Checked = True Then
                Me.RefundType = "PERS"
            End If
            'if person is non vested refund type = PERS 'Shubhrata Jan 30th 2008
            ' End If

            'Refund type for exclude YMCA By Amit 14-05-2009-Start.
            Dim l_CheckBox As CheckBox
            Dim l_exclude As Boolean
            If Me.CheckboxExcludeYMCA.Checked = True Or CheckboxRegular.Checked = True Then
                For Each l_DataGridItem As DataGridItem In Me.DataGridAccContributionRetirement.Items
                    l_CheckBox = l_DataGridItem.Cells(0).Controls(0)
                    If (l_CheckBox.Checked = False) And (l_DataGridItem.Cells(2).Text = m_const_YMCA__Acct Or l_DataGridItem.Cells(2).Text = m_const_YMCA_Legacy_Acct) Then
                        Me.RefundType = "PERS"
                        Exit For
                    End If
                Next
            End If

            'Refund type for exclude YMCA By AMit 14-05-2009-End.

        Catch
            Throw
        End Try
    End Function

    Private Function RefreshDataGridTotal() As Boolean
        Dim l_CheckBox As CheckBox
        Dim l_bool_SelectAMAccount As Boolean
        Dim l_Counter As Integer
        Dim retirementPlanTable As DataTable
        Dim savingsPlanTable As DataTable

        If Me.RefundType = "PERS" Then
            For Each l_DataGridItem As DataGridItem In Me.DataGridAccContributionRetirement.Items

                l_CheckBox = l_DataGridItem.FindControl("Selected")


                If l_CheckBox.Checked = True Then
                    l_bool_SelectAMAccount = True
                End If

                If Not l_CheckBox Is Nothing Then

                    If l_CheckBox.Checked = True Then
                        Me.MakePersonalCalculationTableForDisplay(l_Counter, True, "IsRetirement", l_bool_SelectAMAccount)
                    Else
                        Me.MakePersonalCalculationTableForDisplay(l_Counter, False, "IsRetirement", l_bool_SelectAMAccount)
                    End If

                End If

                l_Counter += 1

            Next
            'START: MMR | 2020.05.07 | YRS-AT-4854 | Get Blended tax rate and apply tax rate on retirement plan account and savings plan account for exclude YMCA Account option selected
            If Not Session("Calculated_DisplayRetirementPlanAcctContribution_C19") Is Nothing Then
                retirementPlanTable = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable)
            End If

            If Not Session("Calculated_DisplaySavingsPlanAcctContribution_C19") Is Nothing Then
                savingsPlanTable = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable)
            End If

            Me.BlendedTaxRate = Me.GetBlendedTaxRate(retirementPlanTable, savingsPlanTable)

            'Calculating tax as per Blended tax rate and updating in display table for retirement plan
            retirementPlanTable = UpdateDisplayTableTaxValueWithBlendedTaxRate(retirementPlanTable, Me.BlendedTaxRate)

            'Calculating tax as per Blended tax rate and updating in display table for savings plan
            savingsPlanTable = UpdateDisplayTableTaxValueWithBlendedTaxRate(savingsPlanTable, Me.BlendedTaxRate)

            If HelperFunctions.isNonEmpty(retirementPlanTable) Then
                Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = retirementPlanTable
            End If

            If HelperFunctions.isNonEmpty(savingsPlanTable) Then
                BindDataGrid("IsSavings", savingsPlanTable)
                Session("Calculated_DisplaySavingsPlanAcctContribution_C19") = savingsPlanTable
            End If

            'Display calculated Covid taxable and non-taxable amount on UI
            Me.DisplayCovidTaxableAndNonTaxableAmount()
            'END: MMR | 2020.05.07 | YRS-AT-4854 | Get Blended tax rate and apply tax rate on retirement plan account and savings plan account for exclude YMCA Account option selected

            Me.CalculateTotalForDisplay("IsRetirement")
            Me.LoadCalculatedTable("IsRetirement")
            MakeDisplayCalculationDataTables(True, True)
            BindFinalDatagrid()
        End If
    End Function

    Private Function ValidateBalanceInAccountsForVol(Optional ByVal parameterCompulsoryVol As Boolean = False) As String
        Try
            Dim l_DataRow As DataRow
            Dim l_CheckBox As CheckBox
            Dim l_DataGridItem As DataGridItem
            Dim l_string_AccountType As String = ""
            Dim l_CalculatedDataTable As DataTable

            Dim parameterTMAmount As Decimal
            Dim parameterTDAmount As Decimal
            Dim parameterRTAmount As Decimal

            Dim l_decimal_BalanceAmount As Decimal
            Dim l_string_message As String = ""
            Dim l_bool_TakeRT As Boolean = False
            Dim l_bool_AllAccountsSelected As Boolean = True
            Dim FinalDataSet As DataSet
            'Comment code Deleted By Sanjeev on 06/02/2012

            '------ Modified for WebServiece by pavan ---- Begin  -----------
            If Not Session("FinalDataSet_C19") Is Nothing Then
                FinalDataSet = Session("FinalDataSet_C19")
                parameterTMAmount = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TMAmount"))
                parameterTDAmount = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TDAmount"))
                parameterRTAmount = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RTAmount"))
            End If
            '------ Modified for WebServiece by pavan ---- END    -----------

            For Each l_DataGridItem In Me.DataGridAccContributionSavings.Items
                l_CheckBox = l_DataGridItem.FindControl("Selected")
                If l_DataGridItem.Cells(2).Text.ToString().ToUpper <> "&NBSP;" Then
                    l_string_AccountType = l_DataGridItem.Cells(2).Text.Trim.ToString()
                Else
                    l_string_AccountType = ""
                End If
                'Priya 16-june-2008 changed if conditon to treate Account type PE and RE like AE and RA as per mail send by Hafiz
                'If Me.SessionStatusType.Trim.ToUpper = "AE" Or Me.SessionStatusType.Trim.ToUpper = "RA" Then
                If Me.SessionStatusType.Trim.ToUpper = "AE" Or Me.SessionStatusType.Trim.ToUpper = "RA" Or Me.SessionStatusType.Trim.ToUpper = "PE" Or Me.SessionStatusType.Trim.ToUpper = "RE" Then
                    If Not l_CheckBox Is Nothing Then
                        If l_CheckBox.Checked = True Then
                            If l_string_AccountType = "RT" And parameterCompulsoryVol = False Then
                                Exit Function
                            End If
                        End If
                    End If

                End If
                If Not l_CheckBox Is Nothing Then

                    If l_CheckBox.Checked = False Then

                        If l_string_AccountType <> "" And l_string_AccountType <> "Total" Then
                            l_bool_AllAccountsSelected = False
                            If l_string_AccountType = "RT" Then
                                l_decimal_BalanceAmount = l_decimal_BalanceAmount + parameterRTAmount
                                l_bool_TakeRT = True

                            End If
                            If l_string_AccountType = "TD" Then
                                l_decimal_BalanceAmount = l_decimal_BalanceAmount + parameterTDAmount
                            End If
                            If l_string_AccountType = "TM" Or l_string_AccountType = "TM-Matched" Then
                                l_decimal_BalanceAmount = l_decimal_BalanceAmount + parameterTMAmount
                            End If

                        End If

                    End If
                End If
            Next
            If parameterCompulsoryVol = True Then
                l_bool_AllAccountsSelected = False
                l_decimal_BalanceAmount = parameterRTAmount + parameterTDAmount + parameterTMAmount
            End If
            'Added By Sanjeev on 03/02/2012 for BT-936
            'If l_decimal_BalanceAmount >= 5000 And l_bool_AllAccountsSelected = False Then
            If l_decimal_BalanceAmount >= Me.MinimumPIAToRetire And l_bool_AllAccountsSelected = False Then
                l_string_message = ""
            ElseIf l_bool_AllAccountsSelected = True Then
                l_string_message = ""
            Else
                'Comment code Deleted By Sanjeev on 06/02/2012
                If l_decimal_BalanceAmount <> 0 Then
                    'Added By Sanjeev on 03/02/2012 for BT-936
                    'l_string_message = "Withdrawal cannot be completed as requested.The balance in the accounts must be greater than 5000$."
                    l_string_message = "Withdrawal cannot be completed as requested.The balance in the accounts must be greater than $ " + MinimumPIAToRetire.ToString() + "."
                End If

            End If
            Return l_string_message
        Catch
            Throw
        End Try
    End Function

    Public Function IsOnlyVoluntaryAllowed() As Boolean
        Try
            Select Case Me.SessionStatusType.Trim.ToUpper
                Case "ML", "PEML", "NP", "PENP", "RDNP"
                    Return True
                Case Me.SessionStatusType.Trim.ToUpper
                    Return False
            End Select
        Catch
            Throw
        End Try
    End Function

    Private Function clearTextBoxSavings()
        Try
            TextboxSavingsNetAmount.Text = "0.00"
            TextboxSavingsNonTaxable.Text = "0.00"
            TextboxSavingsTaxable.Text = "0.00"
            TextboxSavingsTaxWithheld.Text = "0.00"

        Catch
            Throw
        End Try
    End Function

    Private Function EnableDisablePartialCheckBox()
        Dim l_DataTable As DataTable
        'Comment code Deleted By Sanjeev on 06/02/2012

        '------ Added to implement WebServiece by pavan ---- Begin  -----------
        l_DataTable = DirectCast(Session("MemberRefundRequestDetails_C19"), DataTable)
        '------ Added to implement WebServiece by pavan ---- END    -----------
        If Not l_DataTable Is Nothing Then
            For Each l_DataRow As DataRow In l_DataTable.Rows
                If CType(l_DataRow.Item("RequestStatus"), String).Trim.ToUpper = "PEND" Then

                    If CType(l_DataRow.Item("PlanType"), String).Trim.ToUpper = "SAVINGS" Then
                        DisableAllCheckBoxes("Savings")
                        DeselelectAllCheckBoxes("Savings")
                        settextboxvisibleSavings(False)
                        Me.CheckboxSavingsPlan.Checked = False
                    End If

                    If CType(l_DataRow.Item("PlanType"), String).Trim.ToUpper = "RETIREMENT" Then
                        DisableAllCheckBoxes("Retirement")
                        DeselelectAllCheckBoxes("Retirement")
                        settextboxvisibleRetirement(False)
                        Me.CheckboxRetirementPlan.Checked = False
                    End If
                    'Comment code Deleted By Sanjeev on 06/02/2012
                End If
            Next
        End If
    End Function

    Private Function DisableAllCheckBoxes(ByVal string_plantype As String)
        Try
            Select Case string_plantype
                Case "Retirement"
                    Me.CheckboxRegular.Enabled = False
                    Me.CheckboxDisability.Enabled = False
                    Me.CheckboxSpecial.Enabled = False
                    Me.CheckboxExcludeYMCA.Enabled = False
                    Me.CheckboxVoluntryAccounts.Enabled = False
                    Me.CheckboxPartialRetirement.Enabled = False
                    Me.TextboxPartialRetirement.Enabled = False
                Case "Savings"
                    Me.CheckboxHardship.Enabled = False
                    Me.CheckboxSavingsVoluntary.Enabled = False
                    Me.CheckboxPartialSavings.Enabled = False
                    'Comment code Deleted By Sanjeev on 06/02/2012
            End Select
        Catch
            Throw
        End Try
    End Function

    Private Function cleartextBoxRetirement()
        Try
            TextboxRetirementNonTaxable.Text = "0.00"
            TextboxRetirementTaxable.Text = "0.00"
            TextboxRetirementTaxWithheld.Text = "0.00"
            TextboxRetirementNetAmount.Text = "0.00"

        Catch
            Throw
        End Try
    End Function

    Private Function SetcheckboxPartialRetirement()
        Try
            Me.CheckboxRetirementPlan.Checked = False
            Me.CheckboxSavingsPlan.Enabled = False
            If Me.IsTerminated Then
                CheckboxExcludeYMCA.Enabled = Me.AllowPersonalSideRefund()
            Else
                CheckboxExcludeYMCA.Enabled = False
            End If

            If CType(Session("VoluntryAmount_Retirement_Initial_C19"), Decimal) < 0.01 Then
                Me.CheckboxVoluntryAccounts.Enabled = False
            Else
                Me.CheckboxVoluntryAccounts.Enabled = True
            End If

            If CType(Session("VoluntryAmount_Savings_Initial_C19"), Decimal) < 0.01 Then
                Me.CheckboxSavingsVoluntary.Enabled = False
                Me.CheckboxSavingsVoluntary.Checked = False
            Else
                Me.CheckboxSavingsPlan.Enabled = True
                Me.CheckboxSavingsVoluntary.Enabled = True
            End If

            If Me.CheckboxSavingsVoluntary.Checked = True And Me.CheckboxSavingsPlan.Checked = True Then
                Me.RefundType = "VOL"
                objRefundRequest.DoVoluntryRefundForDisplay("IsSavings", True)
                'datagridCheckBox_Savings_CheckedChanged(Me.datagridCheckBox_Savings, e)
                MakeDisplayCalculationDataTables(False, True)
            Else
                Me.RefundType = ""
                MakeDisplayCalculationDataTables(False, False)
            End If
            If Me.TotalAmount > 0 Then
                Me.ButtonSave.Visible = True
            Else
                Me.ButtonSave.Visible = False
            End If

            If Me.CompulsorySavings = True Then
                Me.CheckboxSavingsVoluntary.Enabled = False
            End If

        Catch
            Throw
        End Try
    End Function

    Private Function onTextboxPartialRetirement() As Boolean
        Dim mnyrequestedamount As Decimal = 0.0
        Dim drowRetiredtotal As DataRow
        Dim dttable As DataTable
        Dim drow As DataRow
        Dim l_boolValidateAnnuity As Boolean = False

        'Comment code Deleted By Sanjeev on 06/02/2012
        Try
            If Me.RetirementPlan_TotalAvailableAmount > (Me.NumRequestedAmountRetirement + Me.RetirementPlanProcessingFee) Then  ' SR | 2016.06.07 | YRS-AT-2962 | Total available amount should be greater then Requested amount and Processing fee 
                'Comment code Deleted By Sanjeev on 06/02/2012
                If Me.RetirementPlanTotalAmount - (Me.NumRequestedAmountRetirement + Me.RetirementPlanProcessingFee) > Me.MinimumPIAToRetire Or AnnuityExists_Retirement = True Then ' SR | 2016.06.07 | YRS-AT-2962 | Total available amount available after Requested amount and Processing fee should be greater than minimumamount required
                    Me.NumPercentageFactorofMoneyRetirement = (Me.NumRequestedAmountRetirement / Me.RetirementPlan_TotalAvailableAmount)
                    Me.NumPercentageFactorofMoneyRetirementTemp = Me.NumPercentageFactorofMoneyRetirement
                    Me.IsPartialRetirementAmountGivenTabOutDone = True
                    'Added the condition to manage the save button-Amit 09-09-2009
                    ManageSaveButton(True)
                    'Added the condition to manage the save button-Amit 09-09-2009
                Else
                    'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "YMCA account balance after the Partial withdrawal should be more then " & Me.MinimumPIAToRetire & " . Hence Refund is not possible", MessageBoxButtons.Stop)
                    ' MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Minimum $5,000 balance required after Withdrawal for Retirement plan, so this withdrawal is not permitted as requested.", MessageBoxButtons.Stop)
                    'BT 936:Validation message for minimum balance required after partial or voluntary value should be pick from atsMetaConfiguration table.
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Minimum $" & Me.MinimumPIAToRetire.ToString("##,##0") & " balance required after Withdrawal for Retirement plan, so this withdrawal is not permitted as requested.", MessageBoxButtons.Stop)
                    Me.NumRequestedAmountRetirement = Nothing
                    'Modifed and added a if condition to check the amount. amit
                    If Me.RetirementPlan_TotalAmount = Me.RetirementPlan_TotalAvailableAmount Then
                        TextboxPartialRetirement.Text = String.Empty
                    Else
                        TextboxPartialRetirement.Text = Me.RetirementPlan_TotalAmount
                    End If
                End If

                'START: SB | 2018.08.27 | YRS-AT-4058 | Checking requested withdrawal amount is not equal to the full available balance in the plan, if equal then show error message
            ElseIf Me.RetirementPlan_TotalAvailableAmount = (Me.NumRequestedAmountRetirement + Me.RetirementPlanProcessingFee) Then
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", GetMessage("MESSAGE_PARTIALMAXAMT_IS_EQUALTO_FULLBALANCE_ERROR"), MessageBoxButtons.Stop)
                Me.NumRequestedAmountRetirement = Nothing
                If Me.RetirementPlan_TotalAmount = Me.RetirementPlan_TotalAvailableAmount Then
                    TextboxPartialRetirement.Text = String.Empty
                Else
                    TextboxPartialRetirement.Text = Me.RetirementPlan_TotalAmount
                End If
                'END: SB | 2018.08.27 | YRS-AT-4058 | Checking requested withdrawal amount is not equal to the full available balance in the plan, if equal then show error message
            Else
                'START: JT | 2018.08.27 | YRS-AT-4058 | YRS enh:-correct error message for maximum withdrawal amount (TrackIT 34548)
                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Amount Requested for Withdrawal is greater than or equal to the total amount with the plan. Hence Withdrawal is not possible", MessageBoxButtons.Stop)
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", GetMessage("MESSAGE_PARTIALMAXAMOUNT_ERROR"), MessageBoxButtons.Stop)
                'END:  JT | 2018.08.27 | YRS-AT-4058 | YRS enh:-correct error message for maximum withdrawal amount (TrackIT 34548)
                Me.NumRequestedAmountRetirement = Nothing
                'Modifed and added a if condition to check the amount. amit
                If Me.RetirementPlan_TotalAmount = Me.RetirementPlan_TotalAvailableAmount Then
                    TextboxPartialRetirement.Text = String.Empty
                Else
                    TextboxPartialRetirement.Text = Me.RetirementPlan_TotalAmount
                End If
            End If
        Catch
            Throw
        End Try
    End Function

#Region "Function onTextboxPartialRetirementIsActive() - Added By Sanjeev on 31-01-2012 for BT-955"
    Private Function onTextboxPartialRetirementIsActive() As Boolean
        Dim mnyrequestedamount As Decimal = 0.0
        Dim drowRetiredtotal As DataRow
        Dim dttable As DataTable
        Dim drow As DataRow
        Dim l_boolValidateAnnuity As Boolean = False

        Try
            If Me.VoluntryAmount_Retirement_Initial > Me.NumRequestedAmountRetirement Then
                'If (Me.VoluntryAmount_Retirement_Initial - Me.NumRequestedAmountRetirement > Me.MinimumPIAToRetire) Or (AnnuityExists_Retirement = True) Then
                Me.NumPercentageFactorofMoneyRetirement = (Me.NumRequestedAmountRetirement / Me.VoluntryAmount_Retirement_Initial)
                Me.NumPercentageFactorofMoneyRetirementTemp = Me.NumPercentageFactorofMoneyRetirement
                Me.IsPartialRetirementAmountGivenTabOutDone = True
                ManageSaveButton(True)

                'START: SB | 2018.08.27 | YRS-AT-4058 | Checking requested withdrawal amount is not equal to the full available balance in the plan, if equal then show error message
            ElseIf Me.VoluntryAmount_Retirement_Initial = Me.NumRequestedAmountRetirement Then
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", GetMessage("MESSAGE_PARTIALMAXAMT_IS_EQUALTO_FULLBALANCE_ERROR"), MessageBoxButtons.Stop)
                Me.NumRequestedAmountRetirement = Nothing
                If Me.VoluntryAmount_Retirement_Initial = Me.RetirementPlan_TotalAmount Then
                    TextboxPartialRetirement.Text = String.Empty
                Else
                    TextboxPartialRetirement.Text = Me.RetirementPlan_TotalAmount
                End If
                'END: SB | 2018.08.27 | YRS-AT-4058 | Checking requested withdrawal amount is not equal to the full available balance in the plan, if equal then show error message

                'Comment code Deleted By Sanjeev on 06/02/2012
            Else
                'START: JT | 2018.08.27 | YRS-AT-4058 | YRS enh:-correct error message for maximum withdrawal amount (TrackIT 34548)
                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Amount Requested for Withdrawal is greater than or equal to the total amount with the plan. Hence Withdrawal is not possible", MessageBoxButtons.Stop)
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", GetMessage("MESSAGE_PARTIALMAXAMOUNT_ERROR"), MessageBoxButtons.Stop)
                'END: JT | 2018.08.27 | YRS-AT-4058 | YRS enh:-correct error message for maximum withdrawal amount (TrackIT 34548)
                Me.NumRequestedAmountRetirement = Nothing
                If Me.VoluntryAmount_Retirement_Initial = Me.RetirementPlan_TotalAmount Then
                    TextboxPartialRetirement.Text = String.Empty
                Else
                    TextboxPartialRetirement.Text = Me.RetirementPlan_TotalAmount
                End If
            End If
        Catch
            Throw
        End Try
    End Function
#End Region

    Private Function fillNetValuesforAddedControls(ByVal ParameterCalculatedTable As DataTable)
        Try
            Dim dttable As DataTable
            Dim drow As DataRow
            'dttable = objRefundRequest.CalculateTotalForDisplay(ParameterCalculatedTable)
            dttable = ParameterCalculatedTable
            For Each drow In dttable.Rows
                If drow("AccountType").ToString() = "System.DBNull" Then
                Else
                    If drow("AccountType").ToString() = "Total" Then
                        FillRetirementValuesToControls(drow)
                    End If
                End If
            Next
        Catch
            Throw
        End Try

    End Function

    Private Function fillNetValuesforAddedControlsSavings(ByVal ParameterCalculatedTable As DataTable)
        Try
            Dim dttable As DataTable
            Dim drow As DataRow
            'dttable = objRefundRequest.CalculateTotalForDisplay(ParameterCalculatedTable)
            dttable = ParameterCalculatedTable
            For Each drow In dttable.Rows
                If drow("AccountType").ToString() = "System.DBNull" Then
                Else
                    If drow("AccountType").ToString() = "Total" Then
                        FillSavingValuesToControls(drow)
                    End If
                End If
            Next
        Catch
            Throw
        End Try
    End Function

    Public Function FillRetirementValuesToControls(ByVal parameterDataRow As DataRow)
        Dim l_TaxedAmount As Decimal
        Dim l_TaxedInterestAmount As Decimal
        Dim l_NonTaxedPartialRetirementAmount As Decimal
        Try
            If Not parameterDataRow Is Nothing Then
                ''Assign Federal Rate to Rate Text box
                If Me.TextboxTaxRate.Text.Trim.Length > 0 Then
                    Me.FederalTaxRate = CType(Me.TextboxTaxRate.Text.Trim, Decimal)
                    Me.TextboxTaxRate.Text = Me.FederalTaxRate.ToString("0")
                Else
                    'Me.FederalTaxRate = 20.0 'Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code to remove harcoded tax rate
                    Me.TextboxTaxRate.Text = Me.FederalTaxRate.ToString("0")
                End If
                If parameterDataRow("Non-Taxable").GetType.ToString = "System.DBNull" Then
                    l_NonTaxedPartialRetirementAmount = 0.0
                Else
                    l_NonTaxedPartialRetirementAmount = CType(parameterDataRow("Non-Taxable"), Decimal)
                End If
                If parameterDataRow("Total").GetType.ToString = "System.DBNull" Then
                    l_TaxedAmount = 0.0
                Else
                    l_TaxedAmount = CType(parameterDataRow("Total"), Decimal)
                End If
                Me.TaxWithheldRetirement = CType(parameterDataRow("TaxWithheld"), Decimal)
                l_TaxedAmount = l_TaxedAmount - l_NonTaxedPartialRetirementAmount
                Me.TextboxRetirementNonTaxable.Text = l_NonTaxedPartialRetirementAmount.ToString("0.00")
                Me.TextboxRetirementTaxable.Text = l_TaxedAmount.ToString("0.00")
                Me.TextboxRetirementTaxWithheld.Text = Me.TaxWithheldRetirement.ToString("0.00")
                Me.TextboxRetirementNetAmount.Text = (((l_NonTaxedPartialRetirementAmount) + (l_TaxedAmount)) - ((l_TaxedAmount) * (Me.FederalTaxRate / 100.0))).ToString("0.00")
            End If
        Catch
            Throw
        End Try
    End Function

    Private Function settextboxvisibleRetirement(ByVal Value As Boolean) As Boolean
        Try
            'Commented to stop the display of the textboxes in the page-16-11-2009
            Value = False
            'Commented to stop the display of the textboxes in the page-16-11-2009
            LabelNonTaxableRetirement.Visible = Value
            LabelTaxableRetirement.Visible = Value
            LabelTaxWithheldRetirement.Visible = Value
            LabelNetAmountRetirement.Visible = Value
            TextboxRetirementNonTaxable.Visible = Value
            TextboxRetirementTaxable.Visible = Value
            TextboxRetirementTaxWithheld.Visible = Value
            TextboxRetirementNetAmount.Visible = Value
        Catch
            Throw
        End Try
    End Function

    Private Function settextboxvisibleSavings(ByVal Value As Boolean) As Boolean
        Try
            'Commented to stop the display of the textboxes in the page-16-11-2009
            Value = False
            'Commented to stop the display of the textboxes in the page-16-11-2009
            LabelNonTaxableSavings.Visible = Value
            LabelTaxableSavings.Visible = Value
            LabelTaxWithheldSavings.Visible = Value
            LabelNetAmountSavings.Visible = Value
            TextboxSavingsNonTaxable.Visible = Value
            TextboxSavingsTaxable.Visible = Value
            TextboxSavingsTaxWithheld.Visible = Value
            TextboxSavingsNetAmount.Visible = Value
        Catch
            Throw
        End Try
    End Function

    Private Function SetNetTextAmountsPerPlan()
        Dim Value As Boolean
        Try
            If CheckboxPartialRetirement.Checked = True Or CheckboxPartialSavings.Checked = True Then
                Value = True
            Else
                Value = False
            End If
            LabelNonTaxableRetirement.Visible = Value
            LabelTaxableRetirement.Visible = Value
            LabelTaxWithheldRetirement.Visible = Value
            LabelNetAmountRetirement.Visible = Value
            TextboxRetirementNonTaxable.Visible = Value
            TextboxRetirementTaxable.Visible = Value
            TextboxRetirementTaxWithheld.Visible = Value
            TextboxRetirementNetAmount.Visible = Value
            LabelNonTaxableSavings.Visible = Value
            LabelTaxableSavings.Visible = Value
            LabelTaxWithheldSavings.Visible = Value
            LabelNetAmountSavings.Visible = Value
            TextboxSavingsNonTaxable.Visible = Value
            TextboxSavingsTaxable.Visible = Value
            TextboxSavingsTaxWithheld.Visible = Value
            TextboxSavingsNetAmount.Visible = Value
        Catch
            Throw
        End Try
    End Function

    Private Function onTextboxPartialSavings() As Boolean
        Dim mnyrequestedamount As Decimal = 0.0
        Dim drowsavingtotal As DataRow
        Dim dttable As DataTable
        Dim drow As DataRow
        Dim l_boolValidateAnnuity As Boolean = False
        Try

            If Me.SavingsPlan_TotalAvailableAmount > (Me.NumRequestedAmountSavings + Me.SavingsPlanProcessingFee) Then ' SR | 2016.06.07 | YRS-AT-2962 | Total available amount should be greater then Requested amount and Processing fee 
                'Modified for WebServiece by pavan IsAnnuityExists("IsSavings")
                If (Me.SavingsPlan_TotalAvailableAmount - (Me.NumRequestedAmountSavings + Me.SavingsPlanProcessingFee) > Me.MinimumPIAToRetire) Or (AnnuityExists_Savings = True) Then ' SR | 2016.06.07 | YRS-AT-2962 | Total available amount available after Requested amount and Processing fee should be greater than minimumamount required
                    Me.NumPercentageFactorofMoneySavings = ((Me.NumRequestedAmountSavings / Me.SavingsPlan_TotalAvailableAmount))
                    Me.NumPercentageFactorofMoneySavingsTemp = Me.NumPercentageFactorofMoneySavings
                    IsPartialSavingsAmountGivenTabOutDone = True
                    'Added the condition to manage the save button-Amit 09-09-2009
                    ManageSaveButton(True)
                    'Added the condition to manage the save button-Amit 09-09-2009
                Else
                    'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "YMCA account balance after the Partial withdrawal should be more then " & Me.MinimumPIAToRetire & " . hence Refund is not possible", MessageBoxButtons.Stop)
                    'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Minimum $5,000 balance required after Withdrawal per plan, so this Withdrawal is not permitted as requested.", MessageBoxButtons.Stop)
                    'BT 936:Validation message for minimum balance required after partial or voluntary value should be pick from atsMetaConfiguration table.
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Minimum $" & Me.MinimumPIAToRetire.ToString("##,##0") & " balance required after Withdrawal per plan, so this Withdrawal is not permitted as requested.", MessageBoxButtons.Stop)
                    Me.NumRequestedAmountSavings = Nothing
                    'Modifed and added a if condition to check the amount. amit
                    If Me.SavingsPlan_TotalAmount = Me.SavingsPlan_TotalAvailableAmount Then
                        TextboxPartialSavings.Text = String.Empty
                    Else
                        TextboxPartialSavings.Text = Me.SavingsPlan_TotalAmount
                    End If
                End If

                'START: SB | 2018.08.27 | YRS-AT-4058 | Checking requested withdrawal amount is not equal to the full available balance in the plan, if equal then show error message
            ElseIf Me.SavingsPlan_TotalAvailableAmount = (Me.NumRequestedAmountSavings + Me.SavingsPlanProcessingFee) Then
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", GetMessage("MESSAGE_PARTIALMAXAMT_IS_EQUALTO_FULLBALANCE_ERROR"), MessageBoxButtons.Stop)
                Me.NumRequestedAmountSavings = Nothing
                If Me.SavingsPlan_TotalAmount = Me.SavingsPlan_TotalAvailableAmount Then
                    TextboxPartialSavings.Text = String.Empty
                Else
                    TextboxPartialSavings.Text = Me.SavingsPlan_TotalAmount
                End If
                'END: SB | 2018.08.27 | YRS-AT-4058 | Checking requested withdrawal amount is not equal to the full available balance in the plan, if equal then show error message
            Else
                'START: JT | 2018.08.27 | YRS-AT-4058 | YRS enh:-correct error message for maximum withdrawal amount (TrackIT 34548)
                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Amount Requested for Withdrawal is greater than or equal to the total amount with the plan. Hence Withdrawal is not possible", MessageBoxButtons.Stop)
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", GetMessage("MESSAGE_PARTIALMAXAMOUNT_ERROR"), MessageBoxButtons.Stop)
                'END: JT | 2018.08.27 | YRS-AT-4058 | YRS enh:-correct error message for maximum withdrawal amount (TrackIT 34548)
                Me.NumRequestedAmountSavings = Nothing
                'Modifed and added a if condition to check the amount. amit
                If Me.SavingsPlan_TotalAmount = Me.SavingsPlan_TotalAvailableAmount Then
                    TextboxPartialSavings.Text = String.Empty
                Else
                    TextboxPartialSavings.Text = Me.SavingsPlan_TotalAmount
                End If
            End If
        Catch
            Throw
        End Try
    End Function

#Region "Function onTextboxPartialSavingsIsActive() - Added By Sanjeev on 31-01-2012 for BT-955"
    Private Function onTextboxPartialSavingsIsActive() As Boolean
        Dim mnyrequestedamount As Decimal = 0.0
        Dim drowsavingtotal As DataRow
        Dim dttable As DataTable
        Dim drow As DataRow
        Dim l_boolValidateAnnuity As Boolean = False
        Try
            If Me.VoluntryAmount_Savings_Initial > Me.NumRequestedAmountSavings Then
                'If (Me.VoluntryAmount_Savings_Initial - Me.NumRequestedAmountSavings > Me.MinimumPIAToRetire) Or (AnnuityExists_Savings = True) Then
                Me.NumPercentageFactorofMoneySavings = ((Me.NumRequestedAmountSavings / Me.VoluntryAmount_Savings_Initial))
                Me.NumPercentageFactorofMoneySavingsTemp = Me.NumPercentageFactorofMoneySavings
                IsPartialSavingsAmountGivenTabOutDone = True
                ManageSaveButton(True)

                'START: SB | 2018.08.27 | YRS-AT-4058 | Checking requested withdrawal amount is not equal to the full available balance in the plan, if equal then show error message
            ElseIf Me.VoluntryAmount_Savings_Initial = Me.NumRequestedAmountSavings Then
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", GetMessage("MESSAGE_PARTIALMAXAMT_IS_EQUALTO_FULLBALANCE_ERROR"), MessageBoxButtons.Stop)
                Me.NumRequestedAmountSavings = Nothing
                If Me.VoluntryAmount_Savings_Initial = Me.SavingsPlan_TotalAmount Then
                    TextboxPartialSavings.Text = String.Empty
                Else
                    TextboxPartialSavings.Text = Me.SavingsPlan_TotalAmount
                End If
                'END: SB | 2018.08.27 | YRS-AT-4058 | Checking requested withdrawal amount is not equal to the full available balance in the plan, if equal then show error message

                'Comment code Deleted By Sanjeev on 06/02/2012
            Else
                'START: JT | 2018.08.27 | YRS-AT-4058 | YRS enh:-correct error message for maximum withdrawal amount (TrackIT 34548)
                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Amount Requested for Withdrawal is greater than or equal to the total amount with the plan. Hence Withdrawal is not possible", MessageBoxButtons.Stop)
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", GetMessage("MESSAGE_PARTIALMAXAMOUNT_ERROR"), MessageBoxButtons.Stop)
                'END: JT | 2018.08.27 | YRS-AT-4058 | YRS enh:-correct error message for maximum withdrawal amount (TrackIT 34548)
                Me.NumRequestedAmountSavings = Nothing
                If Me.VoluntryAmount_Savings_Initial = Me.SavingsPlan_TotalAmount Then
                    TextboxPartialSavings.Text = String.Empty
                Else
                    TextboxPartialSavings.Text = Me.SavingsPlan_TotalAmount
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

    Public Function FillSavingValuesToControls(ByVal parameterDataRow As DataRow)
        Dim l_TaxedAmount As Decimal
        Dim l_TaxedInterestAmount As Decimal
        Dim l_NonTaxedPartialSavingsAmount As Decimal
        Try
            If Not parameterDataRow Is Nothing Then
                ''Assign Federal Rate to Rate Text box
                If Me.TextboxTaxRate.Text.Trim.Length > 0 Then
                    Me.FederalTaxRate = CType(Me.TextboxTaxRate.Text.Trim, Decimal)
                    Me.TextboxTaxRate.Text = Me.FederalTaxRate.ToString("0")
                Else
                    'Me.FederalTaxRate = 20.0 'Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code to remove harcoded tax rate
                    Me.TextboxTaxRate.Text = Me.FederalTaxRate.ToString("0")
                End If
                If parameterDataRow("Non-Taxable").GetType.ToString = "System.DBNull" Then
                    l_NonTaxedPartialSavingsAmount = 0.0
                Else
                    l_NonTaxedPartialSavingsAmount = CType(parameterDataRow("Non-Taxable"), Decimal)
                End If
                If parameterDataRow("Total").GetType.ToString = "System.DBNull" Then
                    l_TaxedAmount = 0.0
                Else
                    l_TaxedAmount = CType(parameterDataRow("Total"), Decimal)
                End If
                l_TaxedAmount = l_TaxedAmount - l_NonTaxedPartialSavingsAmount
                Me.TaxWithheldSavings = CType(parameterDataRow("TaxWithheld"), Decimal)
                Me.TextboxSavingsNonTaxable.Text = l_NonTaxedPartialSavingsAmount.ToString("0.00")
                Me.TextboxSavingsTaxable.Text = l_TaxedAmount.ToString("0.00")
                Me.TextboxSavingsTaxWithheld.Text = Me.TaxWithheldSavings.ToString("0.00")
                Me.TextboxSavingsNetAmount.Text = (((l_NonTaxedPartialSavingsAmount) + (l_TaxedAmount)) - ((l_TaxedAmount) * (Me.FederalTaxRate / 100.0))).ToString("0.00")
            End If
        Catch
            Throw
        End Try
    End Function

    'Created a function to clear the sessions- Amit 06/10/2009

    Private Function ClearSession() As Boolean
        Try
            Session("VoluntaryAmountInitial_C19") = Nothing
            Session("YMCAAvailableAmountInitial_C19") = Nothing
            Session("VoluntryAmount_Savings_Initial_C19") = Nothing
            Session("VoluntryAmount_Retirement_Initial_C19") = Nothing
            Session("HardNotAllowed_C19") = Nothing
            Session("OnlySavingsPlan_C19") = Nothing
            Session("OnlyRetirementPlan_C19") = Nothing
            Session("RetirementAmountTotal_C19") = Nothing
            Session("SavingsPlanAmountTotal_C19") = Nothing
            Session("HasPersonalMoney_C19") = Nothing
            Session("CompulsorySavings_C19") = Nothing
            Session("CompulsoryRetirement_C19") = Nothing
            Session("FederalTaxRate_C19") = Nothing 'Manthan | 2016.06.16 | YRS-AT-2962 | Setting session value to nothing
            ''SG: 2012.03.23: BT-1012
            'Session("TermDateDataTabel") = Nothing
            Me.NumRequestedAmountRetirement = Nothing
            Me.NumRequestedAmountSavings = Nothing
            Me.NumPercentageFactorofMoneySavings = 1.0
            Me.NumPercentageFactorofMoneyRetirement = 1.0
            Me.NumPercentageFactorofMoneyRetirementTemp = 1.0
            Me.NumPercentageFactorofMoneySavingsTemp = 1.0
            'New Modification for Market Based Withdrawal Amit Nigam 
            Me.AllowMarketBasedForRetirementPlan = False
            Me.AllowMarketBasedForSavingsPlan = False
            'New Modification for Market Based Withdrawal Amit Nigam 
        Catch
            Throw
        End Try
    End Function

    Private Function PartialDisplayCopyAccountContributionTable()
        Dim l_DataTable_DisplayRetirementPlanAcctContribution As DataTable
        Dim l_DataTable_DisplaySavingsPlanAcctContribution As DataTable
        Dim l_DataTable_DisplayConsolidatedAcctContributions As DataTable
        Dim l_CalculationDataTable_Display_DisplayRetirementPlan As DataTable
        Dim l_CalculationDataTable_Display_DisplaySavingsPlan As DataTable
        Dim l_CalculationDataTable_Display As DataTable
        Dim l_DataColumn_RetirementPlan As DataColumn
        Dim l_DataRow_RetirementPlan As DataRow
        Dim l_DataColumn_SavingsPlan As DataColumn
        Dim l_DataRow_SavingsPlan As DataRow

        Try

            'Get the Calculated Data Table for Retirement Plan
            'Modified by Amit for Partial Refunds
            If Not Session("DisplayRetirementPlanAcctContribution_C19") Is Nothing Then
                l_DataTable_DisplayRetirementPlanAcctContribution = objRefundRequest.DataTableDisplayRetirementPlan
                If l_DataTable_DisplayRetirementPlanAcctContribution Is Nothing Then
                    l_DataTable_DisplayRetirementPlanAcctContribution = CType(Session("DisplayRetirementPlanAcctContribution_C19"), DataTable)
                End If
            End If
            If Not l_DataTable_DisplayRetirementPlanAcctContribution Is Nothing Then

                l_CalculationDataTable_Display_DisplayRetirementPlan = l_DataTable_DisplayRetirementPlanAcctContribution.Clone

                ' Add a column Into the Table to allow selected.
                l_DataColumn_RetirementPlan = New DataColumn("Selected")
                l_DataColumn_RetirementPlan.DataType = System.Type.GetType("System.Boolean")
                l_DataColumn_RetirementPlan.AllowDBNull = True

                l_CalculationDataTable_Display_DisplayRetirementPlan.Columns.Add(l_DataColumn_RetirementPlan)

                'Copy all Values into Calculation Table 

                For Each l_DataRow_RetirementPlan In l_DataTable_DisplayRetirementPlanAcctContribution.Rows

                    If Not (l_DataRow_RetirementPlan("AccountType").GetType.ToString = "System.DBNull") Then

                        If Not (CType(l_DataRow_RetirementPlan("AccountType"), String) = "Total") Then
                            l_CalculationDataTable_Display_DisplayRetirementPlan.ImportRow(l_DataRow_RetirementPlan)
                        End If
                    End If
                Next
            End If
            'Get the Calculated Data Table for Savings Plan
            'Modified by Amit for Partial Refunds
            If Not Session("DisplaySavingsPlanAcctContribution_C19") Is Nothing Then
                'If Me.RefundType = "PART" Then
                l_DataTable_DisplaySavingsPlanAcctContribution = objRefundRequest.DataTableDisplaySavingsPlan
                ' Els'e
                If l_DataTable_DisplaySavingsPlanAcctContribution Is Nothing Then
                    l_DataTable_DisplaySavingsPlanAcctContribution = DirectCast(Session("DisplaySavingsPlanAcctContribution_C19"), DataTable)
                End If
                'l_DataTable_DisplaySavingsPlanAcctContribution = CType(Session("DisplaySavingsPlanAcctContribution"), DataTable)
                ' End If

            End If
            If Not l_DataTable_DisplaySavingsPlanAcctContribution Is Nothing Then

                l_CalculationDataTable_Display_DisplaySavingsPlan = l_DataTable_DisplaySavingsPlanAcctContribution.Clone

                ' Add a column Into the Table to allow selected.
                l_DataColumn_SavingsPlan = New DataColumn("Selected")
                l_DataColumn_SavingsPlan.DataType = System.Type.GetType("System.Boolean")
                l_DataColumn_SavingsPlan.AllowDBNull = True

                l_CalculationDataTable_Display_DisplaySavingsPlan.Columns.Add(l_DataColumn_SavingsPlan)

                'Copy all Values into Calculation Table 

                For Each l_DataRow_SavingsPlan In l_DataTable_DisplaySavingsPlanAcctContribution.Rows

                    If Not (l_DataRow_SavingsPlan("AccountType").GetType.ToString = "System.DBNull") Then

                        If Not (CType(l_DataRow_SavingsPlan("AccountType"), String) = "Total") Then
                            l_CalculationDataTable_Display_DisplaySavingsPlan.ImportRow(l_DataRow_SavingsPlan)
                        End If
                    End If
                Next
            End If
            'now we will create a combined data table for Retirement and Savings Plan to be shown in the consolidated data grid
            If Not l_CalculationDataTable_Display_DisplayRetirementPlan Is Nothing Then
                l_DataTable_DisplayConsolidatedAcctContributions = l_CalculationDataTable_Display_DisplayRetirementPlan.Clone
            ElseIf Not l_CalculationDataTable_Display_DisplaySavingsPlan Is Nothing Then
                l_DataTable_DisplayConsolidatedAcctContributions = l_CalculationDataTable_Display_DisplaySavingsPlan.Clone
            End If

            ' If Not Session("OnlySavingsPlan") = True Or Session("BothPlans") = True Then
            If Session("OnlyRetirementPlan_C19") = True Or Session("BothPlans_C19") = True Then
                If Not l_CalculationDataTable_Display_DisplayRetirementPlan Is Nothing Then
                    For Each drow As DataRow In l_CalculationDataTable_Display_DisplayRetirementPlan.Rows
                        l_DataTable_DisplayConsolidatedAcctContributions.ImportRow(drow)
                    Next
                End If
            End If
            'If Not Session("OnlyRetirementPlan") = True Or Session("BothPlans") = True Then
            If Session("OnlySavingsPlan_C19") = True Or Session("BothPlans_C19") = True Then
                If Not l_DataTable_DisplaySavingsPlanAcctContribution Is Nothing Then
                    For Each drow As DataRow In l_CalculationDataTable_Display_DisplaySavingsPlan.Rows
                        l_DataTable_DisplayConsolidatedAcctContributions.ImportRow(drow)
                    Next
                End If
            End If

            Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = l_CalculationDataTable_Display_DisplayRetirementPlan
            Session("Calculated_DisplaySavingsPlanAcctContribution_C19") = l_CalculationDataTable_Display_DisplaySavingsPlan
            Session("CalculatedDataTableDisplay_C19") = l_DataTable_DisplayConsolidatedAcctContributions
        Catch
            Throw
        End Try
    End Function

    Private Function PartialRefund(ByVal parameterPlanType As String)
        Dim l_ContributionDataTable As DataTable
        Try


            Select Case parameterPlanType
                Case "RetirementPlan"

                    objRefundRequest.DoFullOrPersRefund("IsRetirement")
                    refundActions("IsRetirement")
                    objRefundRequest.DoFullOrPersRefund("IsConsolidated")
                    refundActions("IsConsolidated")
                Case "SavingsPlan"

                    objRefundRequest.DoFullOrPersRefund("IsSavings")
                    refundActions("IsSavings")
                    objRefundRequest.DoFullOrPersRefund("IsConsolidated")
                    refundActions("IsConsolidated")

                Case "BothPlans"

                    objRefundRequest.DoFullOrPersRefund("IsRetirement")
                    refundActions("IsRetirement")
                    objRefundRequest.DoFullOrPersRefund("IsSavings")
                    refundActions("IsSavings")
                    objRefundRequest.DoFullOrPersRefund("IsConsolidated")
                    refundActions("IsConsolidated")

            End Select
            Me.YMCAAvailableAmount = objRefundRequest.YMCAAvailableAmount

        Catch
            Throw
        End Try
    End Function
    'Created a function to clear the sessions- Amit 06/10/2009


    Private Function DisableAllCheckBoxes(ByVal string_plantype As String, ByVal bool_value As Boolean)
        Try
            Select Case string_plantype
                Case "Retirement"
                    Me.CheckboxRegular.Enabled = bool_value
                    Me.CheckboxExcludeYMCA.Enabled = bool_value
                    Me.CheckboxVoluntryAccounts.Enabled = bool_value


                Case "Savings"
                    Me.CheckboxHardship.Enabled = bool_value
                    Me.CheckboxSavingsVoluntary.Enabled = bool_value

            End Select
        Catch
            Throw
        End Try
    End Function

    Private Function ControlAllcheckBoxes(ByVal string_WithdrawalType As String, ByVal parameter_checked As Boolean, ByVal parameter_Type As String)

        Try
            'Comment code Deleted By Sanjeev on 06/02/2012
            AllowVoluntaryWithdrawalOnRetirement()

            'Added By Sanjeev on 25-01-2012 for BT-955
            AllowPartialWithdrawalOnRetirementIsActive()

            ResetGridData()

            Select Case string_WithdrawalType
                Case "Regular"

                    DeselelectAllCheckBoxes("Retirement")

                    If parameter_checked = True Then
                        If CheckboxRegular.Enabled = True Then
                            Me.CheckboxRegular.Checked = True
                            Me.CheckboxRetirementPlan.Checked = True
                        End If

                        Me.RefundType = "REG"
                        Me.IsTypeChoosen = True
                        Me.DisplayAccountContribution("Retirement")
                        'Comment code Deleted By Sanjeev on 06/02/2012

                    Else
                        Me.IsTypeChoosen = False
                        Me.CheckboxRetirementPlan.Checked = False
                        Me.DisplayAccountContribution("Retirement")
                        SetwithdrawalTypeonDeselectCheckBoxes("R")
                    End If

                Case "ExcludeYMCA"
                    DeselelectAllCheckBoxes("Retirement")

                    If parameter_checked = True Then
                        Me.CheckboxExcludeYMCA.Checked = True
                        Me.CheckboxRetirementPlan.Checked = True

                        Me.IsTypeChoosen = True

                        Me.RefundType = "PERS"

                        Me.DisplayAccountContribution("Retirement")
                        'Comment code Deleted By Sanjeev on 06/02/2012
                        CalculateTotalForDisplay("IsRetirement")

                    Else
                        Me.IsTypeChoosen = False
                        Me.DisplayAccountContribution("Retirement")
                        SetwithdrawalTypeonDeselectCheckBoxes("R")
                    End If

                    'RefreshDataGridTotal()

                Case "VolRet"
                    DeselelectAllCheckBoxes("Retirement")
                    If parameter_checked = True Then

                        Me.CheckboxVoluntryAccounts.Checked = True
                        Me.IsTypeChoosen = True

                        Me.RefundType = "VOL"
                        Me.CheckboxRetirementPlan.Checked = True

                        '------ Added to implement WebServiece by pavan ---- Begin  -----------
                        Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = Session("VoluntryRetirementDatatable_C19")
                        '------ Added to implement WebServiece by pavan ---- END    -----------
                        Me.DisplayAccountContribution("Retirement")
                        '------ Commented for WebServiece by pavan ---- Begin  -----------
                        'Me.DisplayCopyAccountContributionTable("Retirement")
                        '------ Commented for WebServiece by pavan ---- END    -----------

                    Else
                        Me.IsTypeChoosen = False
                        Me.DisplayAccountContribution("Retirement")
                        'Comment code Deleted By Sanjeev on 06/02/2012
                        SetwithdrawalTypeonDeselectCheckBoxes("R")
                    End If
                Case "PartialRet"
                    DeselelectAllCheckBoxes("Retirement")

                    If parameter_checked = True Then
                        If Me.CheckboxSavingsPlan.Checked = True Then
                            If Me.CheckboxSavingsVoluntary.Checked = True Then
                                Me.RefundType = "VOL"
                            ElseIf CheckboxPartialRetirement.Enabled = True Then
                                Me.RefundType = "REG"
                            End If
                        Else
                            Me.RefundType = "REG"
                        End If

                        Me.CheckboxPartialRetirement.Checked = True
                        Me.CheckboxPartialRetirement.Enabled = True
                        Me.NumPercentageFactorofMoneyRetirement = Me.NumPercentageFactorofMoneyRetirementTemp
                        'Added by Imran on 16/04/2010 for Rounding issues
                        If Me.NumRequestedAmountRetirement <> 0 Then
                            TextboxPartialRetirement.Text = Me.NumRequestedAmountRetirement
                        End If
                        Me.DisplayAccountContribution("Retirement")
                        Me.IsTypeChoosen = True
                        'Comment code Deleted By Sanjeev on 06/02/2012

                        SetNetTextAmountsPerPlan()
                        TextboxPartialRetirement.Visible = True
                        'Me.RefundType = "PART"
                        If Me.NumRequestedAmountRetirement <> 0 Then
                            TextboxPartialRetirement.Text = Me.NumRequestedAmountRetirement
                            'START: Added By Sanjeev on 31-01-2012 for BT-955
                            'onTextboxPartialRetirement()
                            If (Me.SessionStatusType.Trim.ToUpper = "AE" Or Me.SessionStatusType.Trim.ToUpper = "RA") AndAlso (CheckboxVoluntryAccounts.Enabled = True) Then
                                onTextboxPartialRetirementIsActive()
                            Else
                                onTextboxPartialRetirement()
                            End If
                            'END: Added By Sanjeev on 31-01-2012 for BT-955
                        End If
                    Else
                        Me.IsTypeChoosen = False
                        Me.CheckboxRetirementPlan.Checked = True
                        'Me.RefundType = "REG"
                        Me.NumPercentageFactorofMoneyRetirement = 1.0
                        '****************
                        Me.DisplayAccountContribution("Retirement")
                        'Comment code Deleted By Sanjeev on 06/02/2012
                        TextboxPartialRetirement.Visible = False
                        CheckboxPartialRetirement.Enabled = True
                        TextboxPartialRetirement.Text = String.Empty
                        'SetRegularWithdrawalValuesANDdDeselectCheckBox()
                        SetwithdrawalTypeonDeselectCheckBoxes("R")
                    End If

                Case "VolSav"
                    DeselelectAllCheckBoxes("Savings")

                    If parameter_checked = True Then
                        If Session("VoluntryAmount_Savings_Initial_C19") > 0 Then
                            Me.CheckboxSavingsVoluntary.Checked = True
                        End If
                        Me.IsTypeChoosen = True
                        Me.CheckboxSavingsPlan.Enabled = True
                        Me.CheckboxSavingsPlan.Checked = True
                        '------ Added to implement WebServiece by pavan ---- Begin  -----------
                        If CheckboxVoluntryAccounts.Checked = True Then
                            Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = Session("VoluntryRetirementDatatable_C19")
                        End If
                        '------ Added to implement WebServiece by pavan ---- END    -----------
                        Me.DisplayAccountContribution("Savings")
                        'Comment code Deleted By Sanjeev on 06/02/2012

                        If Me.CheckboxRetirementPlan.Checked = True Then
                            If Me.CheckboxVoluntryAccounts.Checked = True Then
                                Me.RefundType = "VOL"
                            End If
                            If Me.CheckboxExcludeYMCA.Checked = True Or _
                               (Me.TerminationPIA >= Me.MaximumPIAAmount And Me.IsVested = True And Me.IsTerminated = True) Then
                                Me.RefundType = "PERS"
                                Me.CheckboxRetirementPlan.Enabled = True
                            ElseIf Me.IsTerminated Then
                                If Me.RetirementPlan_TotalAmount > 0 Then
                                    Me.CheckboxRetirementPlan.Enabled = True
                                    Me.RefundType = "REG"
                                End If
                            ElseIf CheckboxSavingsVoluntary.Enabled = True Then
                                Me.RefundType = "VOL"
                                Me.CheckboxSavingsVoluntary.Checked = True
                            ElseIf CheckboxHardship.Enabled = True Then

                                Me.RefundType = "HARD"
                                Me.CheckboxHardship.Checked = True
                                DeselelectAllCheckBoxes("Both")
                                If Me.IsTerminated = False Then
                                    Me.CheckboxSavingsVoluntary.Checked = False
                                Else
                                    Me.CheckboxSavingsVoluntary.Checked = True
                                End If
                                ' 
                                Me.IsTypeChoosen = True
                                Me.CheckboxSavingsPlan.Enabled = True
                                Me.CheckboxSavingsPlan.Checked = True
                                Me.DisplayAccountContribution("Retirement")
                                Me.DisplayAccountContribution("Savings")
                                'Comment code Deleted By Sanjeev on 06/02/2012

                            End If
                            'Me.FullRefundForDisplay("BothPlans")
                        Else
                            Me.RefundType = "VOL"
                            'Me.VoluntryRefundForDisplay("BothPlans")
                        End If

                    Else
                        Me.IsTypeChoosen = False
                        SetwithdrawalTypeonDeselectCheckBoxes("S")
                        '------ Added to implement WebServiece by pavan ---- Begin  -----------
                        If CheckboxVoluntryAccounts.Checked = True Then
                            Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = Session("VoluntryRetirementDatatable_C19")
                        End If
                        Me.DisplayAccountContribution("Savings")
                        '------ Added to implement WebServiece by pavan ---- END    -----------


                    End If

                Case "Hardship"
                    DeselelectAllCheckBoxes("Both")

                    If parameter_checked = True Then
                        Me.CheckboxHardship.Checked = True
                        Me.IsTypeChoosen = True
                        Me.RefundType = "HARD"
                        Me.CheckboxSavingsPlan.Checked = True
                        Session("BothPlans_C19") = True
                        'Me.NumPercentageFactorofMoneyRetirement = 1.0
                        'Me.DisplayAccountContribution("Retirement")
                        '------ Added to implement WebServiece by pavan ---- Begin  -----------
                        If CheckboxVoluntryAccounts.Checked = True Then
                            Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = Session("VoluntryRetirementDatatable_C19")
                        End If
                        '------ Added to implement WebServiece by pavan ---- END    -----------
                        '------ Added to implement WebServiece by pavan ---- Begin  -----------
                        Session("Calculated_DisplaySavingsPlanAcctContribution_C19") = Session("HardshipDatatable_C19")
                        '------ Added to implement WebServiece by pavan ---- END    -----------
                        Me.NumPercentageFactorofMoneySavings = 1.0
                        Me.DisplayAccountContribution("Savings")
                        'Me.DisplayCopyAccountContributionTable("Retirement")
                        '------ Commented for WebServiece by pavan ---- Begin  -----------
                        'Me.DisplayCopyAccountContributionTable("Savings")
                        '------ Commented for WebServiece by pavan ---- END    -----------
                        SetPlanCheckBoxes()

                        If parameter_Type = "S" Then
                            If CType(Session("VoluntryAmount_Retirement_Initial_C19"), Decimal) > 0 Then
                                Me.CheckboxVoluntryAccounts.Checked = True
                                Me.CheckboxVoluntryAccounts.Enabled = False
                                'Added By Sanjeev on 25th Jan 2012 for BT-955
                                Me.CheckboxPartialRetirement.Enabled = False
                            End If
                        End If

                    Else
                        '986
                        If CheckboxRetirementPlan.Checked = False Then
                            Me.RefundType = String.Empty
                        End If
                        '986
                        Me.IsTypeChoosen = False
                        SetwithdrawalTypeonDeselectCheckBoxes("S")
                        Me.NumPercentageFactorofMoneySavings = 1.0
                        Me.DisplayAccountContribution("Savings")
                        'Comment code Deleted By Sanjeev on 06/02/2012
                    End If

                Case "PartialSav"
                    DeselelectAllCheckBoxes("Savings")

                    Dim drowsavingtotal As DataRow
                    Dim dtDisplaySavingsPlanAcctContribution As DataTable
                    If parameter_checked = True Then
                        Me.CheckboxPartialSavings.Checked = True
                        Me.CheckboxPartialSavings.Enabled = True
                        'Me.RefundType = "VOL"
                        If Me.CheckboxRetirementPlan.Checked = True Then
                            If Me.CheckboxVoluntryAccounts.Checked = True Then
                                Me.RefundType = "VOL"
                            ElseIf Me.CheckboxExcludeYMCA.Checked = True Or _
                               (Me.TerminationPIA >= Me.MaximumPIAAmount And Me.IsVested = True And Me.IsTerminated = True) Then
                                Me.RefundType = "PERS"
                                Me.CheckboxRetirementPlan.Enabled = True
                            ElseIf Me.IsTerminated Then
                                If Me.RetirementPlan_TotalAmount > 0 Then
                                    Me.CheckboxRetirementPlan.Enabled = True
                                    Me.RefundType = "REG"
                                End If
                            ElseIf CheckboxPartialSavings.Enabled = True Then
                                Me.RefundType = "VOL"
                            End If
                        Else
                            Me.RefundType = "VOL"

                        End If
                        Me.NumPercentageFactorofMoneySavings = Me.NumPercentageFactorofMoneySavingsTemp
                        '------ Added to implement WebServiece by pavan ---- Begin  -----------
                        If CheckboxVoluntryAccounts.Checked = True Then
                            Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = Session("VoluntryRetirementDatatable_C19")
                        End If
                        '------ Added to implement WebServiece by pavan ---- END    -----------
                        Me.DisplayAccountContribution("Savings")
                        Me.IsTypeChoosen = True
                        'Comment code Deleted By Sanjeev on 06/02/2012
                        SetNetTextAmountsPerPlan()
                        'Me.RefundType = "PART"
                        TextboxPartialSavings.Visible = True
                        If Me.NumRequestedAmountSavings <> 0 Then
                            TextboxPartialSavings.Text = Me.NumRequestedAmountSavings
                            'START: Added By Sanjeev on 31-01-2012 for BT-955
                            'onTextboxPartialSavings()
                            If (Me.SessionStatusType.Trim.ToUpper = "AE" Or Me.SessionStatusType.Trim.ToUpper = "RA") AndAlso CheckboxSavingsVoluntary.Enabled = True Then
                                onTextboxPartialSavingsIsActive()
                            Else
                                onTextboxPartialSavings()
                            End If
                            'END: Added By Sanjeev on 31-01-2012 for BT-955
                        End If

                    Else
                        Me.IsTypeChoosen = False
                        'Me.RefundType = "VOL"
                        If Not TextboxPartialSavings.Text = String.Empty Then
                            Me.NumRequestedAmountSavings = TextboxPartialSavings.Text
                        End If
                        Me.NumPercentageFactorofMoneySavings = 1.0
                        '------ Added to implement WebServiece by pavan ---- Begin  -----------
                        If CheckboxVoluntryAccounts.Checked = True Then
                            Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = Session("VoluntryRetirementDatatable_C19")
                        End If
                        '------ Added to implement WebServiece by pavan ---- END    -----------
                        Me.DisplayAccountContribution("Savings")
                        'Comment code Deleted By Sanjeev on 06/02/2012
                        TextboxPartialSavings.Text = String.Empty
                        Me.TextboxPartialSavings.Visible = False
                        Me.CheckboxPartialSavings.Enabled = True
                        SetwithdrawalTypeonDeselectCheckBoxes("S")
                    End If
            End Select
            SetWithdrawalForDisplay(parameter_Type)
            'New Modification for Market Based Withdrawal Amit Nigam
            DisplayToolTip()
            'New Modification for Market Based Withdrawal Amit Nigam
            If Me.RefundType = "PERS" Then
                RefreshDataGridTotal()
            End If
            'Removed the if condition to fix the amount issues in case of partial off-Amit Nigam-17-02-2010
            'If Me.AllowPartialRefund = True Then
            SetVisiblityofTextBoxesAsPerPlan()
            ' End If
            'Removed the if condition to fix the amount issues in case of partial off-Amit Nigam-17-02-2010


            EnableDisablePartialCheckBox()
            ''Added the function to manage the save button-Amit 09-09-2009
            'ManageSaveButton(True)
            ''Added the function to manage the save button-Amit 09-09-2009
            'New Modification for Market Based Withdrawal Amit Nigam
            If LabelMarket.Visible = True Then
                If CheckboxPartialRetirement.Checked = False Or CheckboxPartialSavings.Checked = False Then
                    'DisplayMarketInstallments()
                Else
                    ClearMarketTextBoxes()
                End If

            End If
            'New Modification for Market Based Withdrawal Amit Nigam
            'PopulateConsolidatedTotals()
            'MRD Phase-II
            If CheckboxMRDRetirement.Checked Or CheckboxMRDSavings.Checked Then
                BindMrdGrid("RETIREMENT")
            Else
                PopulateConsolidatedTotals()
            End If

            'Moved from above -Amit 16-11-2009
            ManageSaveButton(True)
            'Moved from above -Amit 16-11-2009
        Catch
            Throw
        End Try
    End Function
    'Added the function to manage the save button-Amit 09-09-2009
    'New Modification for Market Based Withdrawal Amit Nigam
    Private Function ClearMarketTextBoxes()
        TextboxFirstInstallment.Text = "0.00"
        TextboxPercentage.Text = "0.00"
        TextboxNonTaxableMarket.Text = "0.00"
        TextboxTaxableMarket.Text = "0.00"
        TextboxTaxWithHeldMarket.Text = "0.00"
        TextboxNetAmountMarket.Text = "0.00"
    End Function
    'New Modification for Market Based Withdrawal Amit Nigam
    Private Function ManageSaveButton(ByVal bool_Visible As Boolean) As Boolean
        Try
            If Me.TotalAmount > 0 Then
                If bool_Visible = True Then
                    If (Me.CheckboxPartialRetirement.Checked = True And Me.TextboxPartialRetirement.Text = String.Empty) Or (Me.CheckboxPartialSavings.Checked = True And TextboxPartialSavings.Text = String.Empty) Then
                        Me.ButtonSave.Visible = False
                    Else
                        Me.ButtonSave.Visible = bool_Visible
                    End If
                End If
            Else
                Me.ButtonSave.Visible = False
            End If
        Catch
            Throw
        End Try
    End Function
    'Added the function to manage the save button-Amit 09-09-2009

    Private Function AllowVoluntaryWithdrawalOnRetirement()
        Try
            If Session("VoluntryAmount_Retirement_Initial_C19") > 0 Then
                CheckboxVoluntryAccounts.Enabled = True
            Else
                CheckboxVoluntryAccounts.Enabled = False
            End If
        Catch
            Throw
        End Try

    End Function

#Region "Added the function to manage the Partial withdrawal when participant is Active-Added by Sanjeev on 25-01-2012 for BT-955"
    Private Function AllowPartialWithdrawalOnRetirementIsActive()
        Try
            If (Me.SessionStatusType.Trim.ToUpper = "AE" Or Me.SessionStatusType.Trim.ToUpper = "RA") AndAlso Me.CheckboxVoluntryAccounts.Enabled = True Then
                If (VoluntryAmount_Retirement_Initial > PartialMinLimit) Then
                    CheckboxPartialRetirement.Enabled = CheckSixMonthsValidationForPartial("Retirement")
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

    'New Modification for Market Based Withdrawal Amit Nigam 
    Private Function AllowMarketBasedWithdrawalForRetirementPlan()
        Try

            'Modified to stop displaying the Market Based amount in the request tab
            If Me.MarketBasedThreshold <= Me.RetirementPlan_TotalAmount Then
                Me.AllowMarketBasedForRetirementPlan = True
            Else
                Me.AllowMarketBasedForRetirementPlan = False
            End If
            'Modified to stop displaying the Market Based amount in the request tab

            'Comment code Deleted By Sanjeev on 06/02/2012

        Catch
            Throw
        End Try

    End Function
    Private Function AllowMarketBasedWithdrawalForSavingsPlan()
        Try
            If Me.CheckboxSavingsPlan.Checked = True Then
                If Me.MarketBasedThreshold <= Me.SavingsPlan_TotalAmount Then
                    Me.AllowMarketBasedForSavingsPlan = True
                Else
                    Me.AllowMarketBasedForSavingsPlan = False
                End If
            Else
                Me.AllowMarketBasedForSavingsPlan = False
            End If


        Catch
            Throw
        End Try

    End Function
    'Comment code Deleted By Sanjeev on 06/02/2012
    Private Function DisplayToolTip()
        Try
            Dim bool_RetToolTip As Boolean = False
            Dim bool_SavToolTip As Boolean = False

            If CheckboxRegular.Checked = True Or CheckboxExcludeYMCA.Checked = True Or CheckboxVoluntryAccounts.Checked = True Then
                If AllowMarketBasedForRetirementPlan = True Then
                    bool_RetToolTip = True
                End If
            End If

            If CheckboxSavingsVoluntary.Checked = True Then
                If AllowMarketBasedForSavingsPlan = True Then
                    bool_SavToolTip = True
                End If
            End If

            If bool_RetToolTip = True And bool_SavToolTip = True Then
                LabelMarket.Visible = True
                LabelMarket.Text = "Both Plans"
                LabelMarket.ToolTip = "The total withdrawal money in Both plan is above the running threshold limit so the withdrawal will be saved as Market Based Withdrawal."
            ElseIf bool_RetToolTip = True And bool_SavToolTip = False Then
                LabelMarket.Visible = True
                LabelMarket.Text = "Retirement Plan"
                LabelMarket.ToolTip = "The total withdrawal money in Retirement plan is above the running threshold limit so the withdrawal will be saved as Market Based Withdrawal."
            ElseIf bool_RetToolTip = False And bool_SavToolTip = True Then
                LabelMarket.Visible = True
                LabelMarket.Text = "Savings Plan"
                LabelMarket.ToolTip = "The total withdrawal money in Savings plan is above the running threshold limit so the withdrawal will be saved as Market Based Withdrawal."
            ElseIf bool_RetToolTip = False And bool_SavToolTip = False Then
                LabelMarket.Visible = True
                LabelMarket.Text = "N/A"
                LabelMarket.ToolTip = "The total withdrawal money in any plan is below the running threshold limit so the Market Based Withdrawal will not be possible."
            End If

            If CheckboxPartialRetirement.Checked = True And CheckboxPartialSavings.Checked = True Then
                LabelMarket.Visible = True
                LabelMarket.Text = "N/A"
                LabelMarket.ToolTip = "The Partial withdrawal is not allowed for the Market Based Withdrawal."
            End If

        Catch
            Throw
        End Try
    End Function
    'New Modification for Market Based Withdrawal Amit Nigam
    Private Function AllowVoluntaryWithdrawalOnSavings()
        Try
            If Session("VoluntryAmount_Savings_Initial_C19") > 0 Then
                CheckboxSavingsVoluntary.Enabled = True
            Else
                CheckboxSavingsVoluntary.Enabled = False
            End If
            If Me.CompulsorySavings = True Then
                Me.CheckboxSavingsVoluntary.Enabled = False
                Me.CheckboxHardship.Enabled = False
            End If
        Catch
            Throw
        End Try

    End Function

    Private Function AllowPartialWithdrawal()

        Try

            If Me.IsTerminated = True Then
                If Me.RetirementPlan_TotalAvailableAmount > Me.MinimumPIAToRetire Then
                    Me.CheckboxPartialRetirement.Enabled = True
                    'Validates the six months validation for the partial request.
                    CheckboxPartialRetirement.Enabled = CheckSixMonthsValidationForPartial("Retirement")
                    'Validates the six months validation for the partial request.
                Else
                    Me.CheckboxPartialRetirement.Enabled = False
                End If
            Else
                CheckboxPartialRetirement.Enabled = False
            End If

        Catch
            Throw
        End Try
    End Function

    Private Function CheckSixMonthsValidationForPartial(ByVal PlanType As String)
        Dim RequestedMonthDiff As Decimal
        Dim SixMonthsBackDate As Date
        Dim requestedmonthdate As Date
        Try
            requestedmonthdate = YMCARET.YmcaBusinessObject.RefundRequest.GetPartialMonthDifferenceDate(Me.FundID, PlanType)

            SixMonthsBackDate = System.DateTime.Now.AddMonths(-6)

            If SixMonthsBackDate > requestedmonthdate Then
                'code Added by Dinesh kanojia on 27/08/2013
                'Start: DInesh.k:2013.06.28:BT-1502: YRS 5.0-1746:Partial withdrawal not available if employed 
                'Code commented : '2013.10.23        Dinesh Kanojia(DK)   BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                If Me.IsTerminated Then
                    Return True
                End If
                'Return True
                'End: DInesh.k:2013.06.28:BT-1502: YRS 5.0-1746:Partial withdrawal not available if employed 
            Else
                Return False
            End If
            'Comment code Deleted By Sanjeev on 06/02/2012
        Catch
            Throw
        End Try
    End Function

    Private Function AllowRegularWithdrawal()
        Try
            Me.CheckboxSpecial.Visible = False
            Me.CheckboxDisability.Visible = False

            If Me.IsTerminated = True Then
                CheckboxRegular.Enabled = True
                CheckboxExcludeYMCA.Enabled = Me.AllowPersonalSideRefund()
            Else
                CheckboxRegular.Enabled = False
                CheckboxExcludeYMCA.Enabled = False
            End If
        Catch
            Throw
        End Try
    End Function

    Private Function AllowSpecialtyWithdrawal()
        Try
            If Me.RefundType = "SPEC" Then
                CheckboxRegular.Checked = False
                CheckboxDisability.Checked = False
                CheckboxPartialRetirement.Checked = False
                CheckboxSavingsPlan.Checked = False
                If Me.IsVested = True Then
                    Me.CheckboxSpecial.Visible = True
                    Me.CheckboxSpecial.Enabled = False
                    Me.CheckboxSpecial.Checked = True
                    Me.SpecialRefund("RetirementPlan")
                    If CType(Session("VoluntryAmount_Savings_Initial_C19"), Decimal) < 0.01 Then
                        Me.CheckboxSavingsVoluntary.Enabled = False
                        Me.CheckboxSavingsVoluntary.Checked = False
                    Else
                        Me.CheckboxSavingsVoluntary.Enabled = False
                        Me.CheckboxSavingsVoluntary.Checked = True
                        'Me.RefundType = "VOL"
                        Me.VoluntryRefundForDisplay("SavingsPlan")
                        Me.RefundType = "SPEC"
                    End If
                Else
                    Me.CheckboxSpecial.Visible = True
                    Me.CheckboxSpecial.Checked = False
                    Me.CheckboxSpecial.Enabled = False
                End If
            End If
        Catch
            Throw
        End Try
    End Function

    Private Function AllowDisabilityWithdrawal()
        Try
            If Me.RefundType = "DISAB" Then
                CheckboxRegular.Checked = False
                CheckboxSpecial.Checked = False
                CheckboxPartialRetirement.Checked = False
                CheckboxSavingsPlan.Checked = False

                If Me.IsVested = True Then
                    Me.CheckboxDisability.Visible = True
                    Me.CheckboxDisability.Checked = True
                    Me.CheckboxDisability.Enabled = False
                    Me.DisabilityRefund("RetirementPlan")
                    If CType(Session("VoluntryAmount_Savings_Initial_C19"), Decimal) < 0.01 Then
                        Me.CheckboxSavingsVoluntary.Enabled = False
                        Me.CheckboxSavingsVoluntary.Checked = False
                    Else
                        Me.CheckboxSavingsVoluntary.Enabled = False
                        Me.CheckboxSavingsVoluntary.Checked = True
                        'Me.RefundType = "VOL"
                        Me.VoluntryRefundForDisplay("SavingsPlan")
                        Me.RefundType = "DISAB"
                    End If


                Else
                    Me.CheckboxDisability.Visible = True
                    Me.CheckboxDisability.Checked = False
                    Me.CheckboxDisability.Enabled = False
                End If
            End If
        Catch
            Throw
        End Try
    End Function

    'START | 2019.08.07 |  SR | YRS-AT-4498 | Commented below line of code as its not used anywhere.
    'Private Function AllowHarshipWithdrawal()
    '    Try
    '        Me.CheckboxHardship.Enabled = False
    '        If Me.IsTerminated = False And Me.PersonAge < 59.06 And Me.TDAccountAmount > 0.01 Then
    '            Me.CheckboxHardship.Enabled = True
    '            Me.HardShipAllowed = True
    '            Dim l_count As Integer
    '            l_count = 0

    '            l_count = YMCARET.YmcaBusinessObject.RefundRequest.GetLoanStatus(Session("PersonID"))
    '            'l_count = 0 when there is not any paid loan exists for this participlant

    '            If l_count > 0 Then
    '                Me.CheckboxHardship.Enabled = True
    '                Me.HardShipAllowed = True
    '            Else
    '                If Me.TDAccountAmount >= 2000.0 Then
    '                    Me.CheckboxHardship.Enabled = False
    '                    Me.HardShipAllowed = False
    '                End If

    '            End If
    '        Else
    '            Me.CheckboxHardship.Enabled = False
    '            Me.HardShipAllowed = False
    '        End If

    '    Catch
    '        Throw
    '    End Try

    'End Function
    'END | 2019.08.07 |  SR | YRS-AT-4498 | Commented below line of code as its not used anywhere.

    Private Function AllowSavingsPartial()
        Try
            If Me.IsTerminated = True And Me.SavingsPlan_TotalAvailableAmount > Me.MinimumPIAToRetire Then
                CheckboxPartialSavings.Enabled = True
                CheckboxPartialSavings.Enabled = CheckSixMonthsValidationForPartial("Savings")
            Else
                CheckboxPartialSavings.Enabled = False
            End If
        Catch
            Throw
        End Try
    End Function

    Private Function DeselelectAllCheckBoxes(ByVal string_plantype As String)
        Try
            Select Case string_plantype
                Case "Retirement"
                    Me.CheckboxRegular.Checked = False
                    Me.CheckboxExcludeYMCA.Checked = False
                    Me.CheckboxVoluntryAccounts.Checked = False
                    Me.CheckboxPartialRetirement.Checked = False
                    'MRD PHASE-II
                    Me.CheckboxMRDRetirement.Checked = False

                    Me.TextboxPartialRetirement.Visible = False
                    'New Modification for Market Based Withdrawal Amit Nigam
                    LabelFirstInstallment.Visible = False
                    LabelPercentage.Visible = False
                    TextboxFirstInstallment.Visible = False
                    TextboxPercentage.Visible = False
                    LabelNonTaxableMarket.Visible = False
                    LabelTaxableMarket.Visible = False
                    LabelTaxWithHeldMarket.Visible = False
                    LabelNetAmount.Visible = False
                    TextboxNonTaxableMarket.Visible = False
                    TextboxTaxableMarket.Visible = False
                    TextboxTaxWithHeldMarket.Visible = False
                    TextboxNetAmountMarket.Visible = False
                    'New Modification for Market Based Withdrawal Amit Nigam
                    cleartextBoxRetirement()

                Case "Savings"
                    Me.CheckboxHardship.Checked = False
                    Me.CheckboxSavingsVoluntary.Checked = False
                    Me.CheckboxPartialSavings.Checked = False
                    'MRD PHASE-II
                    Me.CheckboxMRDSavings.Checked = False
                    Me.TextboxPartialSavings.Visible = False
                    'New Modification for Market Based Withdrawal Amit Nigam
                    LabelFirstInstallment.Visible = False
                    LabelPercentage.Visible = False
                    TextboxFirstInstallment.Visible = False
                    TextboxPercentage.Visible = False
                    LabelNonTaxableMarket.Visible = False
                    LabelTaxableMarket.Visible = False
                    LabelTaxWithHeldMarket.Visible = False
                    LabelNetAmount.Visible = False
                    TextboxNonTaxableMarket.Visible = False
                    TextboxTaxableMarket.Visible = False
                    TextboxTaxWithHeldMarket.Visible = False
                    TextboxNetAmountMarket.Visible = False
                    'New Modification for Market Based Withdrawal Amit Nigam
                    clearTextBoxSavings()
                Case "Both"
                    'START:Added By Sanjeev on 20th Dec 2011 for BT - 955
                    Me.TextboxPartialSavings.Visible = False
                    Me.TextboxPartialRetirement.Visible = False
                    'END:
                    Me.CheckboxRegular.Checked = False
                    Me.CheckboxExcludeYMCA.Checked = False
                    Me.CheckboxVoluntryAccounts.Checked = False
                    Me.CheckboxPartialRetirement.Checked = False
                    Me.CheckboxMRDSavings.Checked = False
                    Me.CheckboxMRDRetirement.Checked = False
                    Me.CheckboxHardship.Checked = False
                    Me.CheckboxSavingsVoluntary.Checked = False
                    Me.CheckboxPartialSavings.Checked = False
                    cleartextBoxRetirement()
                    clearTextBoxSavings()
                Case "Specialty"
                    Me.CheckboxRegular.Visible = False
                    Me.CheckboxSpecial.Visible = True
                    If Me.RetirementPlan_TotalAmount > 0 Then
                        Me.CheckboxSpecial.Checked = True
                    End If
                    Me.CheckboxDisability.Visible = False
                    Me.CheckboxExcludeYMCA.Enabled = False
                    Me.CheckboxVoluntryAccounts.Enabled = False
                    Me.CheckboxPartialRetirement.Visible = False
                    Me.CheckboxPartialSavings.Visible = False
                    Me.CheckboxSavingsVoluntary.Enabled = False
                    'MRD PHASE-II
                    Me.CheckboxMRDSavings.Checked = False
                    Me.CheckboxMRDRetirement.Checked = False
                    If Session("VoluntryAmount_Savings_Initial_C19") > 0 Then
                        Me.CheckboxSavingsVoluntary.Checked = True
                    End If
                    Me.CheckboxHardship.Enabled = False
                    cleartextBoxRetirement()
                    clearTextBoxSavings()
                    settextboxvisibleRetirement(False)
                    settextboxvisibleSavings(False)

                Case "Disability"
                    Me.CheckboxRegular.Visible = False
                    Me.CheckboxDisability.Visible = True
                    If Me.RetirementPlan_TotalAmount > 0 Then
                        Me.CheckboxDisability.Checked = True
                    End If

                    Me.CheckboxSpecial.Visible = False
                    Me.CheckboxExcludeYMCA.Enabled = False
                    Me.CheckboxVoluntryAccounts.Enabled = False
                    Me.CheckboxPartialRetirement.Visible = False
                    Me.CheckboxPartialSavings.Visible = False
                    Me.CheckboxSavingsVoluntary.Enabled = False
                    Me.CheckboxMRDSavings.Checked = False
                    Me.CheckboxMRDRetirement.Checked = False
                    If Session("VoluntryAmount_Savings_Initial_C19") > 0 Then
                        Me.CheckboxSavingsVoluntary.Checked = True
                    End If
                    Me.CheckboxHardship.Enabled = False
                    cleartextBoxRetirement()
                    clearTextBoxSavings()
                    settextboxvisibleRetirement(False)
                    settextboxvisibleSavings(False)
                    'MRD PHASE-II
                Case "RetirementMRD"
                    Me.CheckboxRegular.Checked = False
                    Me.CheckboxExcludeYMCA.Checked = False
                    Me.CheckboxVoluntryAccounts.Checked = False
                    Me.CheckboxPartialRetirement.Checked = False
                    Me.TextboxPartialRetirement.Visible = False
                    'New Modification for Market Based Withdrawal Amit Nigam
                    LabelFirstInstallment.Visible = False
                    LabelPercentage.Visible = False
                    TextboxFirstInstallment.Visible = False
                    TextboxPercentage.Visible = False
                    LabelNonTaxableMarket.Visible = False
                    LabelTaxableMarket.Visible = False
                    LabelTaxWithHeldMarket.Visible = False
                    LabelNetAmount.Visible = False
                    TextboxNonTaxableMarket.Visible = False
                    TextboxTaxableMarket.Visible = False
                    TextboxTaxWithHeldMarket.Visible = False
                    TextboxNetAmountMarket.Visible = False
                    'New Modification for Market Based Withdrawal Amit Nigam
                    cleartextBoxRetirement()
                Case "SavingsMRD"
                    Me.CheckboxHardship.Checked = False
                    Me.CheckboxSavingsVoluntary.Checked = False
                    Me.CheckboxPartialSavings.Checked = False
                    Me.TextboxPartialSavings.Visible = False
                    'New Modification for Market Based Withdrawal Amit Nigam
                    LabelFirstInstallment.Visible = False
                    LabelPercentage.Visible = False
                    TextboxFirstInstallment.Visible = False
                    TextboxPercentage.Visible = False
                    LabelNonTaxableMarket.Visible = False
                    LabelTaxableMarket.Visible = False
                    LabelTaxWithHeldMarket.Visible = False
                    LabelNetAmount.Visible = False
                    TextboxNonTaxableMarket.Visible = False
                    TextboxTaxableMarket.Visible = False
                    TextboxTaxWithHeldMarket.Visible = False
                    TextboxNetAmountMarket.Visible = False
                    'New Modification for Market Based Withdrawal Amit Nigam
                    clearTextBoxSavings()
            End Select
        Catch
            Throw
        End Try
    End Function


    Private Function ControlAllWithdrawalTypes(ByVal string_refundType As String)
        Try
            AllowSpecialtyWithdrawal()
            AllowDisabilityWithdrawal()
            Select Case string_refundType
                Case "SPEC"
                    DeselelectAllCheckBoxes("Specialty")

                Case "DISAB"
                    DeselelectAllCheckBoxes("Disability")
            End Select
            Me.ButtonSave.Visible = True
            Me.ButtonSave.Enabled = True
            If Me.TotalAmount > 0 Then
                Me.ButtonSave.Visible = True
            Else
                Me.ButtonSave.Visible = False
            End If
            Me.SetCheckBoxOnLoad()
        Catch
            Throw
        End Try
    End Function

    Private Function SetWithdrawalForDisplay(ByVal parameter_Type As String)
        Dim l_stringRefundType As String
        Dim bool_RetPlanSelected As Boolean = False
        Dim bool_SavPlanSelected As Boolean = False

        l_stringRefundType = Me.RefundType


        If parameter_Type = "L" Then
            Me.LoadCalculatedTable("IsConsolidated")
        ElseIf parameter_Type = "R" Then

            Me.LoadCalculatedTable("IsRetirement")
            Me.LoadCalculatedTable("IsSavings")
        ElseIf parameter_Type = "S" Then
            Me.LoadCalculatedTable("IsRetirement")
            Me.LoadCalculatedTable("IsSavings")
        End If


        Try
            If Me.CheckboxVoluntryAccounts.Checked = False And Me.CheckboxHardship.Checked = False And Me.CheckboxPartialRetirement.Checked = False And _
               Me.CheckboxRegular.Checked = False And Me.CheckboxExcludeYMCA.Checked = False And Me.CheckboxSavingsVoluntary.Checked = False And _
               Me.CheckboxPartialSavings.Checked = False Then
                If Me.IsTerminated = False Then
                    Me.RefundType = "VOL"

                End If
                FullRefundForDisplay("BothPlans")
                Me.RefundType = l_stringRefundType
            Else
                If (Me.CheckboxRegular.Checked = True Or Me.CheckboxExcludeYMCA.Checked = True Or Me.CheckboxPartialRetirement.Checked = True) _
                    And (parameter_Type = "L" Or parameter_Type = "R") Then
                    FullRefundForDisplay("RetirementPlan")

                    bool_RetPlanSelected = True
                    If parameter_Type <> "L" Then
                        Exit Function
                    End If
                End If

                If Me.CheckboxVoluntryAccounts.Checked = True And (parameter_Type = "L" Or parameter_Type = "R") Then
                    VoluntryRefundForDisplay("RetirementPlan")
                    bool_RetPlanSelected = True
                    If parameter_Type <> "L" Then
                        Exit Function
                    End If
                End If

                'Comment code Deleted By Sanjeev on 06/02/2012

                If Me.CheckboxSavingsVoluntary.Checked = True And (parameter_Type = "L" Or parameter_Type = "S") Then
                    VoluntryRefundForDisplay("SavingsPlan")
                    bool_SavPlanSelected = True
                    If parameter_Type <> "L" Then
                        Exit Function
                    End If
                End If

                If Me.CheckboxHardship.Checked = True Then
                    bool_SavPlanSelected = True
                    bool_RetPlanSelected = True
                    HardshipRefundForDisplay("SavingsPlan")
                    If Me.IsTerminated = False And CheckboxRetirementPlan.Checked = True Then
                        'Comment code Deleted By Sanjeev on 06/02/2012
                        Me.VoluntryRefundForDisplay("RetirementPlan")
                        Me.RefundType = l_stringRefundType
                    End If
                    Exit Function
                End If




                If Me.CheckboxPartialSavings.Checked = True And (parameter_Type = "L" Or parameter_Type = "S") Then
                    FullRefundForDisplay("SavingsPlan")
                    bool_SavPlanSelected = True
                    If parameter_Type <> "L" Then
                        Exit Function
                    End If
                End If


                If bool_RetPlanSelected = False And parameter_Type = "R" Then
                    FullRefundForDisplay("RetirementPlan")
                    If parameter_Type = "R" Then
                        Exit Function
                    End If
                End If

                If bool_SavPlanSelected = False And parameter_Type = "S" Then
                    FullRefundForDisplay("SavingsPlan")
                End If


            End If

            Me.RefundType = l_stringRefundType


        Catch
            Throw
        End Try
    End Function

    Private Function SetwithdrawalTypeonDeselectCheckBoxes(ByVal paramter_plantype As String)
        Try
            Select Case paramter_plantype
                Case "R"
                    If Me.CheckboxSavingsVoluntary.Checked = True Then
                        Me.RefundType = "VOL"
                    ElseIf Me.CheckboxPartialSavings.Checked = True Then
                        Me.RefundType = "PART"
                    End If
                    'New Modification for Market Based Withdrawal Amit Nigam
                    DisplayToolTip()
                    'New Modification for Market Based Withdrawal Amit Nigam
                Case "S"
                    If Me.TerminationPIA >= Me.MaximumPIAAmount And Me.IsVested = True And Me.IsTerminated = True And CheckboxRegular.Checked = True Then
                        Me.RefundType = "PERS"
                    ElseIf Me.CheckboxRegular.Checked = True Then
                        Me.RefundType = "REG"
                    ElseIf Me.CheckboxExcludeYMCA.Checked = True Then
                        Me.RefundType = "PERS"
                    ElseIf Me.CheckboxPartialRetirement.Checked = True Then
                        Me.RefundType = "PART"
                        'New Modification for Market Based Withdrawal Amit Nigam
                    ElseIf Me.CheckboxVoluntryAccounts.Checked = True Then
                        Me.RefundType = "VOL"
                    End If
                    DisplayToolTip()
                    'New Modification for Market Based Withdrawal Amit Nigam
            End Select

        Catch
            Throw

        End Try
    End Function

    Private Function SetRegularWithdrawalValuesANDdDeselectCheckBox()
        Try
            Me.RefundType = ""
            LabelMessage.Visible = False
            Me.CheckboxRetirementPlan.Checked = False
            'Comment code Deleted By Sanjeev on 06/02/2012



            If Me.TerminationPIA >= Me.MaximumPIAAmount And Me.IsVested = True And Me.IsTerminated = True Then
                Me.RefundType = "PERS"
                Me.CheckboxRetirementPlan.Enabled = True
            ElseIf Me.IsTerminated Then
                If Me.RetirementPlan_TotalAmount > 0 Then
                    Me.CheckboxRetirementPlan.Enabled = True
                    Me.RefundType = "REG"
                End If
            End If

        Catch
            Throw
        End Try
    End Function

    Private Function SetNoWithdrawalsInSavingsPlanANDdDeselectCheckBox()
        Try
            Me.RefundType = ""
            LabelMessage.Visible = False
            Me.CheckboxSavingsPlan.Checked = False
            Me.DisplayAccountContribution("Savings")
            Me.DisplayCopyAccountContributionTable("Savings")

            If Me.CheckboxExcludeYMCA.Checked = True Or _
                (Me.TerminationPIA >= Me.MaximumPIAAmount And Me.IsVested = True And Me.IsTerminated = True) Then
                Me.RefundType = "PERS"
                Me.CheckboxRetirementPlan.Enabled = True
            ElseIf Me.IsTerminated Then
                If Me.RetirementPlan_TotalAmount > 0 Then
                    Me.CheckboxRetirementPlan.Enabled = True
                    Me.RefundType = "REG"
                End If
            End If

        Catch
            Throw
        End Try
    End Function

    Private Function SetVisiblityofTextBoxesAsPerPlan()
        Try

            settextboxvisibleSavings(True)
            settextboxvisibleRetirement(True)
            If CheckboxRegular.Checked = True Or _
                CheckboxExcludeYMCA.Checked = True Or _
                CheckboxVoluntryAccounts.Checked = True Or _
                CheckboxSpecial.Checked = True Or _
                CheckboxDisability.Checked = True Or _
                (CheckboxPartialRetirement.Checked = True _
                And Not TextboxPartialRetirement.Text = String.Empty) Then
                fillNetValuesforAddedControls(Me.Session("Calculated_DisplayRetirementPlanAcctContribution_C19"))


            Else
                cleartextBoxRetirement()
            End If
            If CheckboxSavingsVoluntary.Checked = True Or _
            CheckboxHardship.Checked = True Or _
            (CheckboxPartialSavings.Checked = True _
            And Not TextboxPartialSavings.Text = String.Empty) Then
                fillNetValuesforAddedControlsSavings(Me.Session("Calculated_DisplaySavingsPlanAcctContribution_C19"))

            Else
                clearTextBoxSavings()
            End If

        Catch
            Throw
        End Try
    End Function

    Public Function FillValuesToControls(ByVal parameterDataRow As DataRow)

        Dim l_TaxedAmount As Decimal
        Dim l_TaxedInterestAmount As Decimal
        Dim l_NonTaxedAmount As Decimal
        Dim l_Tax As Decimal

        Try
            If Not parameterDataRow Is Nothing Then

                'Assign Federal Rate to Rate Text box
                If Me.TextboxTaxRate.Text.Trim.Length > 0 Then
                    Me.FederalTaxRate = CType(Me.TextboxTaxRate.Text.Trim, Decimal)
                    Me.TextboxTaxRate.Text = Me.FederalTaxRate.ToString("0")
                Else
                    'Me.FederalTaxRate = 20.0 'Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code to remove harcoded tax rate
                    Me.TextboxTaxRate.Text = Me.FederalTaxRate.ToString("0")
                End If

                If parameterDataRow("Non-Taxable").GetType.ToString = "System.DBNull" Then
                    l_NonTaxedAmount = 0.0
                Else
                    l_NonTaxedAmount = CType(parameterDataRow("Non-Taxable"), Decimal)
                End If

                If parameterDataRow("Total").GetType.ToString = "System.DBNull" Then
                    l_TaxedAmount = 0.0
                Else
                    l_TaxedAmount = CType(parameterDataRow("Total"), Decimal)

                End If


                l_Tax = CType(Me.TaxWithheldRetirement + Me.TaxWithheldSavings, Decimal)



                l_TaxedAmount = l_TaxedAmount - l_NonTaxedAmount


                Me.TextboxNonTaxed.Text = l_NonTaxedAmount.ToString("0.00")
                Me.TextboxTaxed.Text = l_TaxedAmount.ToString("0.00")
                'Me.TextboxTax.Text = ((l_TaxedAmount) * (Me.FederalTaxRate / 100.0)).ToString("0.00")
                Me.TextboxTax.Text = l_Tax
                Me.TextboxNet.Text = (((l_NonTaxedAmount) + (l_TaxedAmount)) - ((l_TaxedAmount) * (Me.FederalTaxRate / 100.0))).ToString("0.00")

            End If

        Catch
            Throw
        End Try
    End Function

#End Region

#Region "PS - SetCheckBoxOnLoad"
    Private Function SetCheckBoxOnLoad1()

        '' This segment for Special Refund. 
        Try
            If Me.RefundType.Trim = "SPEC" Then
                'Shubhrata Mar30th,2007 YREN-3183 SPEC will be allowed only when member is vested done in accordance to 
                'mail by Mark Posey dated 29th Mar,2007
                If Me.IsVested = True Then
                    Me.CheckboxSpecial.Visible = True
                    Me.CheckboxSpecial.Checked = True
                    Me.CheckboxSpecial.Enabled = True
                    Me.ButtonSave.Visible = True

                Else
                    Me.CheckboxSpecial.Visible = True
                    Me.CheckboxSpecial.Checked = False
                    Me.CheckboxSpecial.Enabled = False
                    Me.ButtonSave.Visible = False

                End If
                'Shubhrata Mar30th,2007 YREN-3183 SPEC will be allowed only when member is vested done in accordance to 
                'mail by Mark Posey dated 29th Mar,2007
                Me.CheckboxRegular.Visible = False
                Me.CheckboxDisability.Visible = False



                '' Disable all other checkbox.
                Me.CheckboxHardship.Enabled = False
                CheckboxExcludeYMCA.Enabled = False
                Me.CheckboxVoluntryAccounts.Enabled = False

                Me.CheckboxRegular.Checked = False
                Me.CheckboxHardship.Checked = False
                CheckboxExcludeYMCA.Checked = False
                Me.CheckboxVoluntryAccounts.Checked = False


            ElseIf Me.RefundType.Trim = "DISAB" Then

                '' This segment Disability Refund.

                Me.CheckboxSpecial.Visible = False
                Me.CheckboxRegular.Visible = False
                'Shubhrata Mar30th,2007 YREN-3194 DISAB will be allowed only when member is vested done in accordance to 
                'mail by Mark Posey dated 29th Mar,2007
                If Me.IsVested = True Then
                    Me.CheckboxDisability.Visible = True
                    Me.CheckboxDisability.Checked = True
                    Me.CheckboxDisability.Enabled = True
                    Me.ButtonSave.Visible = True
                Else
                    Me.CheckboxDisability.Visible = False
                    Me.CheckboxDisability.Checked = False
                    Me.CheckboxDisability.Enabled = False
                    Me.ButtonSave.Visible = False
                End If
                'Shubhrata Mar30th,2007 YREN-3194 DISAB will be allowed only when member is vested done in accordance to 
                'mail by Mark Posey dated 29th Mar,2007


                '' Disable all other checkbox.
                Me.CheckboxHardship.Enabled = False
                CheckboxExcludeYMCA.Enabled = False
                Me.CheckboxVoluntryAccounts.Enabled = False

                Me.CheckboxRegular.Checked = False
                Me.CheckboxHardship.Checked = False
                CheckboxExcludeYMCA.Checked = False
                Me.CheckboxVoluntryAccounts.Checked = False

            Else

                'Me.ButtonSave.Visible = False

                Me.CheckboxSpecial.Visible = False
                Me.CheckboxRegular.Visible = True
                Me.CheckboxDisability.Visible = False

                Me.CheckboxRegular.Checked = False

                Me.CheckboxRegular.Enabled = True
                Me.CheckboxHardship.Enabled = True
                CheckboxExcludeYMCA.Enabled = True
                Me.CheckboxVoluntryAccounts.Enabled = True

                Me.CheckboxRegular.Checked = False
                Me.CheckboxHardship.Checked = False
                CheckboxExcludeYMCA.Checked = False
                Me.CheckboxVoluntryAccounts.Checked = False

                '' This is for other Refunds.

                If Me.IsTerminated Then
                    Me.CheckboxRegular.Enabled = True
                    Me.CheckboxVoluntryAccounts.Enabled = True
                    Me.CheckboxHardship.Enabled = False
                Else
                    Me.CheckboxRegular.Enabled = False
                    Me.CheckboxVoluntryAccounts.Enabled = True
                    Me.CheckboxHardship.Enabled = True
                End If
                '**********************************************************
                'Start - Conditions to allow hardship withdrawal
                'Shubhrata - For hardship refund a) a participant must be active or 'RA' b) age less than 59.06 years c) if TD acct > $2k must have taken loan first
                '************************************************************
                If Me.IsTerminated = False And Me.PersonAge < 59.06 And Me.TDAccountAmount > 0.01 Then
                    Me.CheckboxHardship.Enabled = True
                    Dim l_count As Integer
                    l_count = 0

                    l_count = YMCARET.YmcaBusinessObject.RefundRequest.GetLoanStatus(Session("PersonID"))
                    'l_count = 0 when there is not any paid loan exists for this participlant

                    If l_count > 0 Then
                        Me.CheckboxHardship.Enabled = True
                    Else
                        If Me.TDAccountAmount >= 2000.0 Then
                            Me.CheckboxHardship.Enabled = False
                        End If

                    End If
                Else
                    Me.CheckboxHardship.Enabled = False
                End If

                '**********************************************************
                'End - Conditions to allow hardship withdrawal
                '************************************************************
                '************************************************
                'Shubhrata - Start- Voluntary Refund
                'a) Both active and terminated person can take up vol refund,however the account types will be allowed in accordance to the status(see 'Do Voluntary Refund' function)
                'b) There must be money in Voluntary accounts
                '******************************************
                If CType(Session("VoluntaryAmountInitial_C19"), Decimal) < 0.01 Then
                    Me.CheckboxVoluntryAccounts.Enabled = False
                Else
                    Me.CheckboxVoluntryAccounts.Enabled = True
                End If
                '**********************************************************
                'End - Conditions to allow Voluntary withdrawal
                '************************************************************
                '************************************************
                'Shubhrata - Start- Personal Refund
                'If the user opts for Personal Refund and if Me.CurrentPIA >= Me.MinimumPIAToRetire And Me.IsTerminated And Me.RetirementPlan_PersonTotalAmount > 0.0
                'then the user cannot opt for Retirement Plan and hence the Ret Plan will be disabled.
                'The user can opt for only Savings Plan then.  
                '******************************************
                If Me.IsTerminated Then
                    'Comment code Deleted By Sanjeev on 06/02/2012

                    CheckboxExcludeYMCA.Enabled = Me.AllowPersonalSideRefund()

                Else
                    CheckboxExcludeYMCA.Enabled = False
                End If


                '**********************************************************
                'End - Conditions to allow Personal withdrawal


            End If
            '************************************************************    
            If Me.SavingsPlan_TotalAmount <= 0 Then
                Me.CheckboxSavingsPlan.Checked = False
                Me.CheckboxSavingsPlan.Enabled = False
                Session("OnlySavingsPlan_C19") = Nothing
                Session("BothPlans_C19") = Nothing
                Session("OnlyRetirementPlan_C19") = True
                Me.GetConsolidatedAvailableContributions()
            Else
                'Commnetd on Sep07th,2007
                'If Me.AllowedRetirementPlan = True Then
                If Me.RetirementPlan_TotalAmount > 0 Then
                    Me.CheckboxSavingsPlan.Enabled = True
                Else
                    Me.CheckboxSavingsPlan.Enabled = False
                End If

                Me.CheckboxSavingsPlan.Checked = True
                'End If
                'Commnetd on Sep07th,2007
            End If

            If Me.RetirementPlan_TotalAmount <= 0 Then
                Me.CheckboxRetirementPlan.Checked = False
                Me.CheckboxRetirementPlan.Enabled = False
                Session("OnlySavingsPlan_C19") = True
                Session("BothPlans_C19") = Nothing
                Session("OnlyRetirementPlan_C19") = Nothing
                Me.GetConsolidatedAvailableContributions()
            Else
                'Commented on Sep07th,2007
                'If Me.AllowedRetirementPlan = True Then
                If Me.SavingsPlan_TotalAmount > 0 Then
                    Me.CheckboxRetirementPlan.Enabled = True
                Else
                    Me.CheckboxRetirementPlan.Enabled = False
                End If

                Me.CheckboxRetirementPlan.Checked = True
                'End If
                'Commented on Sep07th,2007
            End If
            If Me.CheckboxDisability.Checked = True Or Me.CheckboxHardship.Checked = True Or CheckboxExcludeYMCA.Checked = True Or Me.CheckboxRegular.Checked = True Or Me.CheckboxSpecial.Checked = True Or Me.CheckboxVoluntryAccounts.Checked = True Then
                If Me.RetirementPlan_TotalAmount <= 0 And Me.SavingsPlan_TotalAmount <= 0 Then
                    Me.ButtonSave.Visible = False
                Else
                    Me.ButtonSave.Visible = True
                End If
            End If

            'Plan Split Changes
            '*************************************************
            'Person with QD status cannot take Hardship
            '*************************************************
            If Me.SessionStatusType.Trim.ToUpper = "QD" Then
                Me.CheckboxHardship.Enabled = False
            End If

            'None of the refunds should be allowed if total personal amt = 0 and Termination PIA > = Max PIA amt and person is vested
            If Me.TerminationPIA >= Me.MaximumPIAAmount And Me.IsVested = True And (Me.RetirementPlan_PersonTotalAmount + Me.SavingsPlan_PersonTotalAmount) = 0 Then
                Me.CheckboxRegular.Enabled = False
                Me.CheckboxSpecial.Enabled = False
                Me.CheckboxVoluntryAccounts.Enabled = False
                Me.CheckboxHardship.Enabled = False
                CheckboxExcludeYMCA.Enabled = False
                Me.CheckboxRetirementPlan.Checked = False
                Me.CheckboxRetirementPlan.Enabled = False
                Me.CheckboxSavingsPlan.Checked = False
                Me.CheckboxSavingsPlan.Enabled = False

                Dim l_DataTable As New DataTable
                Dim l_DataRow As DataRow
                l_DataTable = Session("PersonInformation_C19").Tables("Member Employment")
                If l_DataTable.Rows.Count > 0 Then
                    Dim l_terminated_date As New Date
                    'by Aparna -YREN-3019
                    For Each l_DataRow In l_DataTable.Rows
                        If Not l_DataRow("TermDate").GetType.ToString = "System.DBNull" Then

                            If DateTime.Compare(l_terminated_date, CType(l_DataRow("TermDate"), DateTime)) < 0 Then
                                l_terminated_date = CType(l_DataRow("TermDate"), DateTime)
                            End If

                        End If

                    Next

                    Dim l_string_dollar As String
                    l_string_dollar = "$"
                    If l_terminated_date >= "01/01/1996" Then

                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "YMCA account balance at the time of termination was greater than  " & l_string_dollar & " " & Me.MaximumPIAAmount & " ", MessageBoxButtons.Stop)
                    Else
                        'IB:BT687/YRS 5.0-1228 : Remove unnecessary validation
                        ' MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Termination prior to 1/1/1996. YMCA account balance as of 12/31/1995 is greater than " & l_string_dollar & " " & Me.MaximumPIAAmount & "  ; You must use Special Refund option to process this request.", MessageBoxButtons.Stop)
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Termination prior to 1/1/1996. YMCA account balance as of 12/31/1995 is greater than " & l_string_dollar & " " & Me.MaximumPIAAmount & "  ; Please validate actual YMCA Account balance at termination and use Special Withdrawal option to process this request.", MessageBoxButtons.Stop)
                    End If

                End If


            End If
            If (Me.SavingsPlan_TotalAmount + Me.RetirementPlan_TotalAmount) <= 0 Then
                Me.CheckboxRegular.Enabled = False
                CheckboxExcludeYMCA.Enabled = False
                Me.CheckboxHardship.Enabled = False
                Me.CheckboxVoluntryAccounts.Enabled = False
                Me.CheckboxDisability.Enabled = False
                Me.CheckboxSpecial.Enabled = False
            End If
            ' After all the refund types are enabled/disabled we will disable both the plans if none of them are checked
            If Me.RefundType.Trim.ToUpper <> "DISAB" And Me.RefundType.Trim.ToUpper <> "SPEC" Then
                If Me.CheckboxHardship.Enabled = False And CheckboxExcludeYMCA.Enabled = False And Me.CheckboxRegular.Enabled = False And Me.CheckboxVoluntryAccounts.Enabled = False Then
                    Me.CheckboxRetirementPlan.Enabled = False
                    Me.CheckboxSavingsPlan.Enabled = False
                End If
            End If
        Catch
            Throw
        End Try

    End Function
#End Region

#Region "Modified - SetCheckBoxOnLoad"

    Private Function SetCheckBoxOnLoad()

        '' This segment for Special Refund. 
        Try
            If Me.RefundType.Trim = "SPEC" Then
                'Shubhrata Mar30th,2007 YREN-3183 SPEC will be allowed only when member is vested done in accordance to 
                'mail by Mark Posey dated 29th Mar,2007
                Me.CheckboxRetirementPlan.Enabled = False
                If Me.IsVested = True Then
                    Me.CheckboxSpecial.Visible = True
                    Me.CheckboxSpecial.Checked = True
                    Me.CheckboxSpecial.Enabled = False
                    Me.ButtonSave.Visible = True

                Else
                    Me.CheckboxSpecial.Visible = True
                    Me.CheckboxSpecial.Checked = False
                    Me.CheckboxSpecial.Enabled = False
                    Me.ButtonSave.Visible = False

                End If
                'Shubhrata Mar30th,2007 YREN-3183 SPEC will be allowed only when member is vested done in accordance to 
                'mail by Mark Posey dated 29th Mar,2007
                Me.CheckboxRegular.Visible = False
                Me.CheckboxDisability.Visible = False



                '' Disable all other checkbox.
                Me.CheckboxHardship.Enabled = False
                Me.CheckboxExcludeYMCA.Enabled = False
                Me.CheckboxVoluntryAccounts.Enabled = False
                Me.CheckboxSavingsVoluntary.Enabled = False
                Me.CheckboxRegular.Checked = False
                Me.CheckboxHardship.Checked = False
                CheckboxExcludeYMCA.Checked = False
                Me.CheckboxVoluntryAccounts.Checked = False


            ElseIf Me.RefundType.Trim = "DISAB" Then

                '' This segment Disability Refund.
                Me.CheckboxRetirementPlan.Enabled = False
                Me.CheckboxSpecial.Visible = False
                Me.CheckboxRegular.Visible = False
                'Shubhrata Mar30th,2007 YREN-3194 DISAB will be allowed only when member is vested done in accordance to 
                'mail by Mark Posey dated 29th Mar,2007
                If Me.IsVested = True Then
                    Me.CheckboxDisability.Visible = True
                    Me.CheckboxDisability.Checked = True
                    Me.CheckboxDisability.Enabled = False
                    Me.ButtonSave.Visible = True
                Else
                    Me.CheckboxDisability.Visible = False
                    Me.CheckboxDisability.Checked = False
                    Me.CheckboxDisability.Enabled = False
                    Me.ButtonSave.Visible = False
                End If
                'Shubhrata Mar30th,2007 YREN-3194 DISAB will be allowed only when member is vested done in accordance to 
                'mail by Mark Posey dated 29th Mar,2007


                '' Disable all other checkbox.
                Me.CheckboxHardship.Enabled = False
                Me.CheckboxExcludeYMCA.Enabled = False
                Me.CheckboxVoluntryAccounts.Enabled = False
                Me.CheckboxSavingsVoluntary.Enabled = False
                Me.CheckboxRegular.Checked = False
                Me.CheckboxHardship.Checked = False
                CheckboxExcludeYMCA.Checked = False
                Me.CheckboxVoluntryAccounts.Checked = False

            ElseIf Me.RefundType = "PART" Then
                If CheckboxPartialRetirement.Checked = True Then
                    Me.CheckboxSpecial.Visible = False
                    Me.CheckboxDisability.Visible = False
                    Me.CheckboxRegular.Visible = True
                    Me.CheckboxRegular.Checked = False
                    Me.CheckboxRegular.Enabled = False
                    Me.CheckboxExcludeYMCA.Visible = True
                    Me.CheckboxExcludeYMCA.Enabled = False
                    Me.CheckboxExcludeYMCA.Checked = False
                    Me.CheckboxVoluntryAccounts.Visible = True
                    Me.CheckboxVoluntryAccounts.Enabled = False
                    Me.CheckboxVoluntryAccounts.Checked = False
                ElseIf CheckboxPartialSavings.Checked = True Then
                    Me.CheckboxHardship.Visible = True
                    Me.CheckboxHardship.Checked = False
                    Me.CheckboxHardship.Enabled = False
                    Me.CheckboxSavingsVoluntary.Visible = True
                    Me.CheckboxSavingsVoluntary.Checked = False
                    Me.CheckboxSavingsVoluntary.Enabled = False
                End If
            Else

                'Me.ButtonSave.Visible = False

                Me.CheckboxSpecial.Visible = False
                Me.CheckboxRegular.Visible = True
                Me.CheckboxDisability.Visible = False

                Me.CheckboxRegular.Checked = False

                Me.CheckboxRegular.Enabled = True
                Me.CheckboxHardship.Enabled = True
                Me.CheckboxExcludeYMCA.Enabled = True
                Me.CheckboxVoluntryAccounts.Enabled = True
                Me.CheckboxSavingsVoluntary.Enabled = True
                Me.CheckboxRegular.Checked = False
                Me.CheckboxHardship.Checked = False
                CheckboxExcludeYMCA.Checked = False
                Me.CheckboxVoluntryAccounts.Checked = False

                If Me.IsTerminated Then
                    Me.CheckboxRegular.Enabled = True
                    Me.CheckboxVoluntryAccounts.Enabled = True
                    Me.CheckboxHardship.Enabled = False
                    Me.CheckboxSavingsVoluntary.Enabled = True
                    'to enable  Partial Checkbox for partial withdrawal-Amit
                    Me.CheckboxPartialRetirement.Enabled = True
                    Me.CheckboxPartialSavings.Enabled = True
                    'to enable  Partial Checkbox for partial withdrawal-Amit
                Else
                    Me.CheckboxRegular.Enabled = False
                    Me.CheckboxVoluntryAccounts.Enabled = True
                    Me.CheckboxHardship.Enabled = True
                    Me.CheckboxSavingsVoluntary.Enabled = True
                    Me.CheckboxExcludeYMCA.Enabled = False
                    'to disable  Partial Checkbox for partial withdrawal-Amit
                    Me.CheckboxPartialRetirement.Enabled = False
                    Me.CheckboxPartialSavings.Enabled = False
                    'to enable  Partial Checkbox for partial withdrawal-Amit
                End If
                '**********************************************************
                'Start - Conditions to allow hardship withdrawal
                'Shubhrata - For hardship refund a) a participant must be active or 'RA' b) age less than 59.06 years c) if TD acct > $2k must have taken loan first
                '************************************************************
                If Me.IsTerminated = False And Me.PersonAge < 59.06 And Me.TDAccountAmount > 0.01 Then
                    Me.CheckboxHardship.Enabled = True
                    Me.HardShipAllowed = True
                    Dim l_count As Integer
                    l_count = 0

                    l_count = YMCARET.YmcaBusinessObject.RefundRequest.GetLoanStatus(Session("PersonID"))
                    'l_count = 0 when there is not any paid loan exists for this participlant

                    If l_count > 0 Then
                        Me.CheckboxHardship.Enabled = True
                        Me.HardShipAllowed = True
                    Else
                        If Me.TDAccountAmount >= 2000.0 Then
                            Me.CheckboxHardship.Enabled = False
                            Me.HardShipAllowed = False
                        End If

                    End If
                Else
                    Me.CheckboxHardship.Enabled = False
                    Me.HardShipAllowed = False
                End If

                '**********************************************************
                'End - Conditions to allow hardship withdrawal
                '************************************************************
                '************************************************
                'Shubhrata - Start- Voluntary Refund
                'a) Both active and terminated person can take up vol refund,however the account types will be allowed in accordance to the status(see 'Do Voluntary Refund' function)
                'b) There must be money in Voluntary accounts
                '******************************************
                If CType(Session("VoluntryAmount_Retirement_Initial_C19"), Decimal) < 0.01 Then
                    Me.CheckboxVoluntryAccounts.Enabled = False
                Else
                    Me.CheckboxVoluntryAccounts.Enabled = True
                End If
                If CType(Session("VoluntryAmount_Savings_Initial_C19"), Decimal) < 0.01 Then
                    Me.CheckboxSavingsVoluntary.Enabled = False
                    Me.CheckboxSavingsVoluntary.Checked = False
                Else
                    Me.CheckboxSavingsVoluntary.Enabled = True
                    Me.CheckboxSavingsVoluntary.Checked = True
                End If
                '**********************************************************
                'End - Conditions to allow Voluntary withdrawal
                '************************************************************
                '************************************************
                'Shubhrata - Start- Personal Refund
                'If the user opts for Personal Refund and if Me.CurrentPIA >= Me.MinimumPIAToRetire And Me.IsTerminated And Me.RetirementPlan_PersonTotalAmount > 0.0
                'then the user cannot opt for Retirement Plan and hence the Ret Plan will be disabled.
                'The user can opt for only Savings Plan then.  

                Dim bool_EnablePersonalCheckBox As Boolean
                bool_EnablePersonalCheckBox = True
                If Me.IsTerminated Then
                    CheckboxExcludeYMCA.Enabled = Me.AllowPersonalSideRefund()
                Else
                    CheckboxExcludeYMCA.Enabled = False
                End If
            End If
            If Me.SavingsPlan_TotalAmount <= 0 Then
                Me.CheckboxSavingsPlan.Checked = False
                Me.CheckboxSavingsPlan.Enabled = False
                Session("OnlySavingsPlan_C19") = Nothing
                Session("BothPlans_C19") = Nothing
                Session("OnlyRetirementPlan_C19") = True
                'Me.GetConsolidatedAvailableContributions()
            Else
                'Commnetd on Sep07th,2007
                'If Me.AllowedRetirementPlan = True Then
                If Me.RetirementPlan_TotalAmount > 0 Then
                    Me.CheckboxSavingsPlan.Enabled = True
                Else
                    Me.CheckboxSavingsPlan.Enabled = False
                End If

                Me.CheckboxSavingsPlan.Checked = True
                'End If
                'Commnetd on Sep07th,2007
            End If

            If Me.RetirementPlan_TotalAmount <= 0 Then
                Me.CheckboxRetirementPlan.Checked = False
                Me.CheckboxRetirementPlan.Enabled = False
                Session("OnlySavingsPlan_C19") = True
                Session("BothPlans_C19") = Nothing
                Session("OnlyRetirementPlan_C19") = Nothing
                'Me.GetConsolidatedAvailableContributions()
            Else
                'Commented on Sep07th,2007
                'If Me.AllowedRetirementPlan = True Then
                If Me.SavingsPlan_TotalAmount > 0 Then
                    Me.CheckboxRetirementPlan.Enabled = True
                Else
                    Me.CheckboxRetirementPlan.Enabled = False
                End If

                Me.CheckboxRetirementPlan.Checked = True
                'End If
                'Commented on Sep07th,2007
            End If


            'Plan Split Changes
            '*************************************************
            'Person with QD status cannot take Hardship
            '*************************************************
            If Me.SessionStatusType.Trim.ToUpper = "QD" Then
                Me.CheckboxHardship.Enabled = False
            End If

            'None of the refunds should be allowed if total personal amt = 0 and Termination PIA > = Max PIA amt and person is vested
            'If Me.TerminationPIA >= Me.MaximumPIAAmount And Me.IsVested = True And (Me.RetirementPlan_PersonTotalAmount + Me.SavingsPlan_PersonTotalAmount) = 0 Then'
            SetNoRefundAllowed()

            SetEnableDisableForNoMoney()
            ' After all the refund types are enabled/disabled we will disable both the plans if none of them are checked
            disableBothPlanTypes()


        Catch
            Throw
        End Try

    End Function

    Private Function SetNoRefundAllowed()
        Try
            Dim l_string_dollar As String
            Dim StringMessage As String
            Dim bool_continue As Boolean

            l_string_dollar = "$"
            StringMessage = String.Empty
            bool_continue = True

            'If Me.TerminationPIA >= Me.MaximumPIAAmount And Me.IsVested = True And (Me.RetirementPlan_PersonTotalAmount + Me.SavingsPlan_PersonTotalAmount) = 0 Then

            'If Me.IsVested = True And Me.RetirementPlan_PersonTotalAmount + Me.SavingsPlan_PersonTotalAmount <= 0 And Me.VoluntryAmount <= 0 And Not (Me.RefundType = "DISAB" Or Me.RefundType = "SPEC") Then
            If Me.IsVested = True And Me.RetirementPlan_PersonTotalAmountInitial + Me.SavingsPlan_PersonTotalAmountInitial <= 0 And Me.VoluntryAmount <= 0 And Not (Me.RefundType = "DISAB" Or Me.RefundType = "SPEC") Then

                'Added By SG: 2012.06.15: BT-1012(Re-Opened)
                Dim l_terminated_date As Nullable(Of Date)
                'Dim l_terminated_date As New Date

                'Comment code Deleted By Sanjeev on 06/02/2012
                If Not l_Terminateddate Is Nothing Then
                    l_terminated_date = CType(l_Terminateddate, DateTime)
                End If

                '-- START | SR | 2016.07.18 | YRS-AT-3015 - YRS enh: Configurable Withdrawals project (YRSwebService/yrfWebsite)
                If (Me.IsBALegacyCombinedAmountSwitchedON) Then
                    If ((Me.BACurrent + Me.CurrentPIA) > Me.MaxCombinedBasicAccountAmt) Then '--SR | 2016.08.11 | YRS-AT-3096 | check for combined amount. If combined amount greater than maximum combined amount then check for individual account
                        If (Me.BACurrent() > Me.MaxYMCAAcctAmt AndAlso SessionStatusType.ToUpper() <> "QD") Then
                            StringMessage = "YMCA account balance at the time of request is greater than  " & l_string_dollar & Me.MaxYMCAAcctAmt & " "
                            bool_continue = False
                        End If

                        If Me.TerminationPIA > Me.MaxYMCALegacyAcctAmt Then
                            If Me.BACurrent() > 0 And bool_continue = True Then
                                bool_continue = True
                            Else
                                bool_continue = False
                            End If

                        ElseIf Me.CurrentPIA > 0 Then
                            bool_continue = True
                        End If

                    Else
                        If (Me.CurrentPIA > 0) Then
                            bool_continue = True
                        End If

                        If (Me.BACurrent > 0) Then
                            bool_continue = True
                        End If
                    End If

                    'If (Me.BACurrent() > Me.MaxYMCAAcctAmt AndAlso SessionStatusType.ToUpper() <> "QD") Then
                    '    StringMessage = "YMCA account balance at the time of request is greater than  " & l_string_dollar & Me.MaxYMCAAcctAmt & " "
                    '    bool_continue = False
                    'End If

                    'If Me.TerminationPIA > Me.MaxYMCALegacyAcctAmt Then
                    '    If Me.BACurrent() > 0 And bool_continue = True Then
                    '        bool_continue = True
                    '    Else
                    '        bool_continue = False
                    '    End If

                    'ElseIf Me.CurrentPIA > 0 Then
                    '    bool_continue = True
                    'End If
                Else
                    If Me.BACurrent() > Me.BAMaxLimit AndAlso SessionStatusType.ToUpper() <> "QD" Then 'SP 2015.02.12  BT-1936/YRS 5.0-2011: -Append condition to skip for QD fundevents for BA account limit validation
                        StringMessage = "YMCA account balance at the time of request is greater than  " & l_string_dollar & Me.BAMaxLimit & " "
                        bool_continue = False
                    End If

                    If Me.TerminationPIA > Me.MaximumPIAAmount Then

                        'Commented By SG: 2012.08.07: Removing unnecessary validation
                        'If Not StringMessage = String.Empty Then
                        '    StringMessage = StringMessage & " AND "
                        'End If
                        ''IB:BT687/YRS 5.0-1228 : Remove unnecessary validation
                        'If l_terminated_date < "01/01/1996" Then
                        '    'StringMessage = StringMessage & "Termination prior to 1/1/1996. YMCA account balance as of 12/31/1995 is greater than " + l_string_dollar & Me.MaximumPIAAmount & "  ; You must use Special Refund option to process this request."
                        '    StringMessage = StringMessage & "Termination prior to 1/1/1996. YMCA account balance as of 12/31/1995 is greater than " + l_string_dollar & Me.MaximumPIAAmount & "  ;   Please validate actual YMCA Account balance at termination and use Special Withdrawal option to process this request."
                        'End If
                        ''StringMessage = StringMessage & "YMCA account balance at the time of termination was greater than  " & l_string_dollar & Me.MaximumPIAAmount

                        If Me.BACurrent() > 0 And bool_continue = True Then
                            bool_continue = True
                        Else
                            bool_continue = False
                        End If

                    ElseIf Me.CurrentPIA > 0 Then
                        bool_continue = True
                    End If
                End If
                '-- END | SR | 2016.07.18 | YRS-AT-3015 - YRS enh: Configurable Withdrawals project (YRSwebService/yrfWebsite)



                If bool_continue = False Then

                    'Comment code Deleted By Sanjeev on 06/02/2012
                    NoRefundAllowed = True
                    'BT 735: Shows "Object reference not set to an instance of an object." page while adding MRD refund.
                    If Not StringMessage = String.Empty Then
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", StringMessage, MessageBoxButtons.Stop)
                    End If
                    Exit Function
                End If

            End If

            'End If
        Catch
            Throw
        End Try
    End Function

    Private Function SetEnableDisableForNoMoney()
        Try
            If (Me.SavingsPlan_TotalAmount + Me.RetirementPlan_TotalAmount) <= 0 Then
                Me.CheckboxRegular.Enabled = False
                CheckboxExcludeYMCA.Enabled = False
                Me.CheckboxHardship.Enabled = False
                Me.CheckboxVoluntryAccounts.Enabled = False
                Me.CheckboxDisability.Enabled = False
                Me.CheckboxSpecial.Enabled = False
            End If
        Catch
            Throw
        End Try
    End Function

    Private Function disableBothPlanTypes()
        Try
            If Me.RefundType.Trim.ToUpper <> "DISAB" And Me.RefundType.Trim.ToUpper <> "SPEC" Then
                If Me.CheckboxRegular.Enabled = False And Me.CheckboxExcludeYMCA.Enabled = False And Me.CheckboxVoluntryAccounts.Enabled = False Then
                    Me.CheckboxRetirementPlan.Enabled = False
                    Me.CheckboxRetirementPlan.Checked = False
                End If
                If Me.CheckboxSavingsVoluntary.Enabled = False And Me.CheckboxHardship.Enabled = False Then
                    Me.CheckboxSavingsPlan.Enabled = False
                    Me.CheckboxSavingsPlan.Checked = False
                End If

            End If
        Catch
            Throw
        End Try
    End Function
#End Region

    'Comment code Deleted By Sanjeev on 06/02/2012

#Region " Save Refund "

    Private Function SaveRefund() As Boolean
        Dim l_boolValidateAnnuity As Boolean = False
        Dim l_stringMessage As String = String.Empty

        Dim l_intAddressID As Integer

        Dim l_string_ErrorMessage As String = ""
        Dim l_string_MessageForRT As String = ""

        Try

            '' Check for Address ID in Address Information

            l_intAddressID = Me.GetAddressID
            If l_intAddressID = 0 Then
                '' Error Message
                'No Information was found in the Address History Table for this Person
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "No Information was found in the Address History Table for this Person.", MessageBoxButtons.Stop, False)
                Return False
            End If
            If Me.SessionStatusType.Trim.ToUpper <> "RT" Then
                If Me.CheckboxSavingsVoluntary.Checked = True Then
                    l_string_MessageForRT = ValidateBalanceInAccountsForVol()
                    If l_string_MessageForRT <> "" Then
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", l_string_MessageForRT, MessageBoxButtons.Stop)
                        Exit Function
                    End If
                End If
            End If


            'Added by Ganeswar on 17-06-2009 for gemin Issue 787
            Dim l_DataGridItem As DataGridItem
            Dim l_bool_RetirementSelectAcct As Boolean = False
            Dim l_bool_SavingsSelectAcct As Boolean = False
            Dim l_RetirementCheckBox As CheckBox
            Dim l_SavingsCheckBox As CheckBox

            'Comment code Deleted By Sanjeev on 06/02/2012

            For Each l_DataGridItem In Me.DataGridAccContributionSavings.Items
                l_SavingsCheckBox = l_DataGridItem.FindControl("Selected")
                If l_bool_SavingsSelectAcct = False Then
                    If l_SavingsCheckBox.Checked Then
                        l_bool_SavingsSelectAcct = True
                    End If
                End If
            Next
            If l_bool_SavingsSelectAcct = False Then
                CheckboxSavingsVoluntary.Checked = False
            End If
            'Added by Ganeswar on 17-06-2009 for gemin Issue 787





            'by aparna 25/10/2007
            'If Me.RefundType <> "REG" Then
            If Me.IsTerminated = True Then
                'Added By SG: 2013.01.29: BT-1671
                If Me.RefundType <> "SPEC" And Me.RefundType <> "DISAB" Then ''SP 2015.01.12 - BT-2737 -Added condition to skip validation for disability & special type of withdrawal
                    If Me.CheckboxRetirementPlan.Checked = True AndAlso CheckboxMRDRetirement.Checked = False AndAlso CheckboxMRDRetirementCurrentYear.Checked = False Then
                        'If Me.CheckboxRetirementPlan.Checked = True Then

                        l_boolValidateAnnuity = Me.ValidateAnnuityExistsORMinimumPIAtoRetire("IsRetirement")
                        'If l_boolValidateAnnuity = False 
                        If l_boolValidateAnnuity = False Then
                            l_stringMessage = "Above $5,000 balance required after withdrawal for Retirement plan, so this withdrawal is not permitted as requested."
                            If Me.CheckboxSavingsPlan.Checked = True Then
                                l_boolValidateAnnuity = Me.ValidateAnnuityExistsORMinimumPIAtoRetire("IsSavings")
                                'If l_boolValidateAnnuity = False
                                'Added by Ganeswar on 17-06-2009 for gemin Issue 787
                                If l_boolValidateAnnuity = False And l_bool_SavingsSelectAcct Then
                                    'Added by Ganeswar on 17-06-2009 for gemin Issue 787
                                    l_stringMessage = "Above $5,000 balance required after withdrawal per plan, so this withdrawal is not permitted as requested."
                                End If
                            End If
                        Else
                            If Me.CheckboxSavingsPlan.Checked = True Then
                                l_boolValidateAnnuity = Me.ValidateAnnuityExistsORMinimumPIAtoRetire("IsSavings")
                                'Added by Ganeswar on 17-06-2009 for gemin Issue 787
                                'If l_boolValidateAnnuity = False
                                If l_boolValidateAnnuity = False And l_bool_SavingsSelectAcct Then
                                    'Added by Ganeswar on 17-06-2009 for gemin Issue 787
                                    l_stringMessage = "Above $5,000 balance required after withdrawal for Savings plan, so this withdrawal is not permitted as requested."
                                End If
                            End If
                        End If
                    Else
                        If Me.CheckboxSavingsPlan.Checked = True Then ' SR | 2017.07.18 | YRS-AT-3161 | If Savings plan refund option is selected then only go to check further condition to display 5k Balance validation 
                            l_boolValidateAnnuity = Me.ValidateAnnuityExistsORMinimumPIAtoRetire("IsSavings")
                            'Added by Ganeswar on 17-06-2009 for gemin Issue 787
                            'If l_boolValidateAnnuity = False
                            If l_boolValidateAnnuity = False And l_bool_SavingsSelectAcct Then
                                'Added by Ganeswar on 17-06-2009 for gemin Issue 787
                                l_stringMessage = "Above $5,000 balance required after withdrawal for Savings plan, so this withdrawal is not permitted as requested."
                            End If
                        End If
                    End If
                End If

                If Not l_stringMessage = String.Empty Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", l_stringMessage, MessageBoxButtons.Stop)
                    Exit Function
                End If
            End If
            'End If

            'by aparna 25/10/2007
            'Set the necessary properties before calling the save function
            If l_intAddressID <> 0 Then
                objRefundRequest.AddressId = l_intAddressID
            End If
            'Save the Request by setting the necessary Property values which are required in the object.
            Me.SetRefundType()

            'Added by Ganesh for Partial

            'Comment code Deleted By Sanjeev on 06/02/2012

            '------ Added to implement WebServiece by pavan ---- Begin  -----------               
            MakeFinalDataTables("IsRetirement")
            MakeFinalDataTables("IsSavings")
            Dim dt_Retirement, dt_Savings As DataTable

            If Not (Session("Calculated_DisplayRetirementPlanAcctContribution_Final_C19") Is Nothing And Session("Calculated_DisplaySavingsPlanAcctContribution_Final_C19") Is Nothing) Then
                dt_Retirement = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_Final_C19"), DataTable)
                dt_Savings = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_Final_C19"), DataTable)
            Else
                dt_Retirement = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable)
                dt_Savings = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable)
            End If

            If CheckboxMRDRetirement.Checked Or CheckboxMRDRetirementCurrentYear.Checked Or CheckboxMRDSavings.Checked Or CheckboxMRDSavingsCurrentYear.Checked Then
                If FinalDataSet Is Nothing Then
                    Me.FinalDataSet = Session("FinalDataSet_C19")
                End If
                If (CheckboxMRDRetirement.Checked Or CheckboxMRDRetirementCurrentYear.Checked) Then
                    ArraySlectedRetirementAccounts = ArrayOfSelectedRetirementAccountTypes(FinalDataSet.Tables("MRDRetirement_" + GetMRDYear("RETIREMENT")))
                End If
                If (CheckboxMRDSavings.Checked Or CheckboxMRDSavingsCurrentYear.Checked) Then
                    ArraySlectedSavingsAccounts = ArrayOfSelectedSavingsAccountTypes(FinalDataSet.Tables("MRDSavings_" + GetMRDYear("SAVINGS")))
                End If

            Else
                ArraySlectedRetirementAccounts = ArrayOfSelectedRetirementAccountTypes(dt_Retirement)
                ArraySlectedSavingsAccounts = ArrayOfSelectedSavingsAccountTypes(dt_Savings)
            End If
            'MMR | 2020.05.05 YRS-AT-4854 | Set Total tax withheld which will be pased to web method for saving
            Me.TotalTaxWithheld = GetTotalTaxWithheld(dt_Retirement, dt_Savings)

            objService.PreAuthenticate = True
            objService.Credentials = System.Net.CredentialCache.DefaultCredentials

            'Neeraj : 23-Mar-2010 added new bool parameter
            If Me.RefundType = "SPEC" Or Me.RefundType = "DISAB" Then
                objWebServiceReturn = objService.SaveRefundRequestForSpecOrDisb(SaveArraylistPropertys(ArraySlectedRetirementAccounts, ArraySlectedSavingsAccounts), True)
            Else
                'START: PPP | 11/13/2015 | YRS-AT-2639 | Calling new method created for version 2, this will ensure that new request will get saved in version 2 compatible format 
                'objWebServiceReturn = objService.SaveRefundRequest(SaveArraylistPropertys(ArraySlectedRetirementAccounts, ArraySlectedSavingsAccounts), True)
                objWebServiceReturn = objService.SaveRefundRequestForYRS_C19(SaveArraylistPropertys(ArraySlectedRetirementAccounts, ArraySlectedSavingsAccounts), True)
                'END: PPP | 11/13/2015 | YRS-AT-2639 | Calling new method created for version 2, this will ensure that new request will get saved in version 2 compatible format 
                'objWebServiceReturn = objService.SaveRefundRequestAndProcess(SaveArraylistPropertys(ArraySlectedRetirementAccounts, ArraySlectedSavingsAccounts), True)
            End If

            'objWebServiceReturn = objService.SaveRefundRequest(SaveArraylistPropertys(ArraySlectedRetirementAccounts, ArraySlectedSavingsAccounts), True)

            If objWebServiceReturn.ReturnStatus.Equals(YRSWebService.Status.Success) Then
                Session("ServiceRequestReport_C19") = objWebServiceReturn.WebServiceDataSet
                'START: SG: 2012.07.18: BT-1053
                If Not objWebServiceReturn.Message Is Nothing Then
                    Session("GenerateErrors_C19") = objWebServiceReturn.Message
                End If
                'END: SG: 2012.07.18: BT-1053
                'If objWebServiceReturn.WebServiceDataSet Is Nothing Then Session("GenerateErrors") = objWebServiceReturn.Message
            ElseIf objWebServiceReturn.ReturnStatus.Equals(YRSWebService.Status.Warning) Then
                If (Not objWebServiceReturn.MessageCode Is Nothing) AndAlso (objWebServiceReturn.MessageCode.ToUpper() = "MESSAGE_WITHDRAWAL_CopyToServerError".ToUpper()) Then
                    Session("ServiceRequestReport_C19") = objWebServiceReturn.WebServiceDataSet
                End If
                Session("GenerateErrors_C19") = objWebServiceReturn.Message
            Else
                ClearSession()
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", objWebServiceReturn.Message, MessageBoxButtons.Stop)
                Exit Function
            End If
            '------ Added to implement WebServiece by pavan ---- END    -----------


            'Added by Ganesh for Partial on Aug 12
            'Added by Amit for including/excluding YMCA/LeagacyYMCA Account on Aug 14




            'Added by Ganesh for Partial on Aug 12
            ClearSession()

            'Comment code Deleted By Sanjeev on 06/02/2012
            Session("TotalAmtForReleaseBlnk_C19") = Nothing
            'Comment code Deleted By Sanjeev on 06/02/2012

            If Me.RefundType.Trim = "SPEC" Then
            ElseIf Me.RefundType.Trim = "DISAB" Then
            Else
                Me.RefundType = ""
                Me.IsTypeChoosen = False
            End If
            Return True


        Catch

            Throw
        End Try
    End Function


    'Comment code Deleted By Sanjeev on 06/02/2012


    'Added by ganeswar insert request data to the atsRefRequestTransacts table
    Private Sub InsetrtatsRefRequestTransacts(ByVal RefRequestId As String, ByVal NumPercentage As Decimal, ByVal bool_NOTIncludeYMCAAcct As Boolean, ByVal bool_NOTIncludeYMCALegacyAcct As Boolean)
        Try
            YMCARET.YmcaBusinessObject.RefundRequest.GetPartialRefundsRequest(RefRequestId, NumPercentage, bool_NOTIncludeYMCAAcct, bool_NOTIncludeYMCALegacyAcct)
        Catch
            Throw
        End Try
    End Sub
    'Added by ganeswar insert request data to the atsRefRequestTransacts table
#End Region



#Region " Minimum Distribution "
    '***********************************************************************************************
    '* Determine if a Minimum Distribution is Required for this Refund
    '* 1. Must be Terminated
    '* 2. Must have attained the Minimum age (70.5) at this Writing
    '* 3. Today's Date Must be after March 31st in the Year Following the Termination
    '***********************************************************************************************

    Private Function CalculateMinimumDistributionAmount()

        Dim l_DecimalDistributionPeriod As Decimal

        Me.MinimumDistributionAmount = 0.0

        Try

            ' Check for Termination.
            If Not Me.IsTerminated Then
                Return 0
            End If

            '' Check for Minimum age.
            If Me.PersonAge < Me.MinimumDistributedAge Then
                Return 0
            End If

            If Me.CheckForDistributionDate = True Then
                '***********************************************************************************************
                '* Okay a Minimum Distribution is Required for this Person
                '* Let's go to the Table and find out how much
                '***********************************************************************************************

                l_DecimalDistributionPeriod = YMCARET.YmcaBusinessObject.RefundRequest.GetDistributionPeriod(Me.PersonAge)

                If Not Me.TextboxTaxed.Text.Trim.Length = 0 Then

                    If l_DecimalDistributionPeriod <> 0 Then
                        Me.MinimumDistributionAmount = (CType(Me.TextboxTaxed.Text.Trim, Decimal)) / l_DecimalDistributionPeriod
                    End If

                End If


            End If
        Catch
            Throw
        End Try


    End Function

    Private Function GetAccountBreakDown()

        Dim l_DataTable As DataTable

        Try

            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetAccountBreakDown()

            Session("AccountBreakDown_C19") = l_DataTable

        Catch
            Throw
        End Try
    End Function

    Private Function GetAccountBreakDownType(ByVal parameterAccountType As String, ByVal parameterPersonal As Boolean, ByVal parameterYMCA As Boolean, ByVal parameterPreTax As Boolean, ByVal parameterPostTax As Boolean) As String

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Dim l_FoundRows As DataRow()
        Dim l_QueryString As String

        Try

            l_DataTable = DirectCast(Session("AccountBreakDown_C19"), DataTable)

            If Not l_DataTable Is Nothing Then

                l_QueryString = "chrAcctType = '" & parameterAccountType.Trim & "' "

                If parameterPersonal = True Then
                    l_QueryString &= " AND bitPersonal = 1 "
                End If

                If parameterYMCA = True Then
                    l_QueryString &= " AND bitYmca = 1 "
                End If

                If parameterPreTax = True Then
                    l_QueryString &= " AND bitPreTax = 1 "
                End If

                If parameterPostTax = True Then
                    l_QueryString &= " AND bitPostTax = 1 "
                End If


                'Comment code Deleted By Sanjeev on 06/02/2012

                l_FoundRows = l_DataTable.Select(l_QueryString)

                If l_FoundRows.Length > 0 Then
                    l_DataRow = l_FoundRows(0)

                    Return (CType(l_DataRow("chrAcctBreakDownType"), String))
                Else
                    Return String.Empty
                End If

            Else
                Return String.Empty
            End If


        Catch
            Throw
        End Try

    End Function

    Private Function GetAccountBreakDownSortOrder(ByVal paramterAccountType As String) As Integer

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_AccountType As String

        Try

            l_DataTable = DirectCast(Session("AccountBreakDown_C19"), DataTable)

            If Not l_DataTable Is Nothing Then

                For Each l_DataRow In l_DataTable.Rows

                    l_AccountType = CType(l_DataRow("chrAcctBreakDownType"), String)

                    If l_AccountType <> "" Then
                        If l_AccountType.Trim.ToUpper = paramterAccountType.Trim.ToUpper Then
                            Return (CType(l_DataRow("intSortOrder"), Integer))
                        End If
                    End If
                Next

            Else
                Return 0
            End If


        Catch
            Throw
        End Try


    End Function


#End Region

#Region " Refund Functions "

    Private Function refundActions(ByVal parameterCalledBy As String)
        Dim l_ContributionDataTable As DataTable

        l_ContributionDataTable = DirectCast(objRefundRequest.ContributionDataTable, DataTable)

        Try
            If parameterCalledBy = "IsRetirement" Then
                Session("Calculated_RetirementPlanAcctContribution_C19") = l_ContributionDataTable
                Me.CalculateTotal("IsRetirement")
                Me.LoadCalculatedTable("IsRetirement")
                Me.SetSelectedIndex(Me.DataGridAccContributionRetirement, l_ContributionDataTable)
            ElseIf parameterCalledBy = "IsSavings" Then
                Session("Calculated_SavingsPlanAcctContribution_C19") = l_ContributionDataTable
                Me.CalculateTotal("IsSavings")
                Me.LoadCalculatedTable("IsSavings")
                Me.SetSelectedIndex(Me.DataGridAccContributionSavings, l_ContributionDataTable)
            ElseIf parameterCalledBy = "IsConsolidated" Then
                Session("CalculatedDataTable_C19") = l_ContributionDataTable
                Me.CalculateTotal("IsConsolidated")
                Me.LoadCalculatedTable("IsConsolidated")
                Me.SetSelectedIndex(Me.DataGridAccountContribution, l_ContributionDataTable)
            End If


        Catch
            Throw
        End Try
    End Function

    Private Function refundActionsForDisplay(ByVal parameterCalledBy As String)
        Dim l_ContributionDataTable As DataTable

        '------ Commented for WebServiece by pavan ---- Begin  -----------
        ' l_ContributionDataTable = CType(objRefundRequest.ContributionDataTable, DataTable)
        '------ Commented for WebServiece by pavan ---- END    -----------
        Try
            If parameterCalledBy = "IsRetirement" Then
                'Comment code Deleted By Sanjeev on 06/02/2012

                '------ Added to implement WebServiece by pavan ---- Begin  -----------
                l_ContributionDataTable = Session("Calculated_DisplayRetirementPlanAcctContribution_C19")
                '------ Added to implement WebServiece by pavan ---- END    -----------
                Me.CalculateTotalForDisplay("IsRetirement")
                Me.LoadCalculatedTable("IsRetirement")
                Me.SetSelectedIndex(Me.DataGridAccContributionRetirement, l_ContributionDataTable)
            ElseIf parameterCalledBy = "IsSavings" Then
                'Comment code Deleted By Sanjeev on 06/02/2012
                '------ Added to implement WebServiece by pavan ---- Begin  -----------
                l_ContributionDataTable = Session("Calculated_DisplaySavingsPlanAcctContribution_C19")
                '------ Added to implement WebServiece by pavan ---- END    -----------
                Me.CalculateTotalForDisplay("IsSavings")
                Me.LoadCalculatedTable("IsSavings")
                Me.SetSelectedIndex(Me.DataGridAccContributionSavings, l_ContributionDataTable)
            ElseIf parameterCalledBy = "IsConsolidated" Then
                Session("CalculatedDataTableDisplay_C19") = l_ContributionDataTable
                Me.CalculateTotalForDisplay("IsConsolidated")
                Me.LoadCalculatedTable("IsConsolidated")
                Me.SetSelectedIndex(Me.DatagridConsolidateTotal, l_ContributionDataTable)
            End If


        Catch
            Throw
        End Try
    End Function

#Region " Full Refund "


    '' This function for Full Refund.
    Private Function FullRefund(ByVal parameterPlanType As String)
        Dim l_ContributionDataTable As DataTable
        Try


            Select Case parameterPlanType
                Case "RetirementPlan"

                    objRefundRequest.DoFullOrPersRefund("IsRetirement")
                    refundActions("IsRetirement")
                    objRefundRequest.DoFullOrPersRefund("IsConsolidated")
                    refundActions("IsConsolidated")
                Case "SavingsPlan"

                    objRefundRequest.DoFullOrPersRefund("IsSavings")
                    refundActions("IsSavings")
                    objRefundRequest.DoFullOrPersRefund("IsConsolidated")
                    refundActions("IsConsolidated")

                Case "BothPlans"

                    objRefundRequest.DoFullOrPersRefund("IsRetirement")
                    refundActions("IsRetirement")
                    objRefundRequest.DoFullOrPersRefund("IsSavings")
                    refundActions("IsSavings")
                    objRefundRequest.DoFullOrPersRefund("IsConsolidated")
                    refundActions("IsConsolidated")

            End Select
            Me.YMCAAvailableAmount = objRefundRequest.YMCAAvailableAmount

        Catch
            Throw
        End Try
    End Function


#End Region

#Region " Full Refund For Display"


    '' This function for Full Refund.
    Private Function FullRefundForDisplay(ByVal parameterPlanType As String)
        Dim l_ContributionDataTable As DataTable
        Try


            Select Case parameterPlanType
                Case "RetirementPlan"

                    'Comment code Deleted By Sanjeev on 06/02/2012
                    refundActionsForDisplay("IsRetirement")
                    ' objRefundRequest.DoFullOrPersRefundForDisplay("IsConsolidated")
                    'refundActionsForDisplay("IsConsolidated")
                Case "SavingsPlan"

                    'Comment code Deleted By Sanjeev on 06/02/2012
                    refundActionsForDisplay("IsSavings")
                    'objRefundRequest.DoFullOrPersRefundForDisplay("IsConsolidated")
                    'refundActionsForDisplay("IsConsolidated")

                Case "BothPlans"

                    'Comment code Deleted By Sanjeev on 06/02/2012
                    refundActionsForDisplay("IsRetirement")
                    refundActionsForDisplay("IsSavings")
                    'Comment code Deleted By Sanjeev on 06/02/2012

            End Select
            'Comment code Deleted By Sanjeev on 06/02/2012

            '------ Added to implement WebServiece by pavan ---- Begin  -----------
            Me.YMCAAvailableAmount = YMCAAvailableAmount
            '------ Added to implement WebServiece by pavan ---- END    -----------

        Catch
            Throw
        End Try
    End Function


#End Region

#Region " Voluntry Refund "

    Private Function VoluntryRefund(ByVal parameterPlanType As String)
        Dim boolVolChecked As Boolean
        Dim l_ContributionDataTable As DataTable
        Try
            boolVolChecked = Me.CheckboxVoluntryAccounts.Checked
            Select Case parameterPlanType
                Case "RetirementPlan"

                    objRefundRequest.DoVoluntryRefund("IsRetirement", boolVolChecked)
                    refundActions("IsRetirement")
                    objRefundRequest.DoVoluntryRefund("IsConsolidated", boolVolChecked)
                    refundActions("IsConsolidated")

                Case "SavingsPlan"

                    objRefundRequest.DoVoluntryRefund("IsSavings", boolVolChecked)
                    refundActions("IsSavings")
                    objRefundRequest.DoVoluntryRefund("IsConsolidated", boolVolChecked)
                    refundActions("IsConsolidated")

                Case "BothPlans"

                    objRefundRequest.DoVoluntryRefund("IsRetirement", boolVolChecked)
                    refundActions("IsRetirement")
                    objRefundRequest.DoVoluntryRefund("IsSavings", boolVolChecked)
                    refundActions("IsSavings")
                    objRefundRequest.DoVoluntryRefund("IsConsolidated", boolVolChecked)
                    refundActions("IsConsolidated")

            End Select
            Me.VoluntryAmount = objRefundRequest.VoluntryAmount
            Me.YMCAAvailableAmount = objRefundRequest.YMCAAvailableAmount


        Catch
            Throw
        End Try
    End Function

#End Region

#Region " Voluntry Refund For Display "

    Private Function VoluntryRefundForDisplay(ByVal parameterPlanType As String)
        Dim boolVolChecked As Boolean
        Dim l_ContributionDataTable As DataTable
        Try
            boolVolChecked = Me.CheckboxVoluntryAccounts.Checked
            Select Case parameterPlanType
                Case "RetirementPlan"
                    'Comment code Deleted By Sanjeev on 06/02/2012
                    refundActionsForDisplay("IsRetirement")
                    'Comment code Deleted By Sanjeev on 06/02/2012

                Case "SavingsPlan"
                    'Comment code Deleted By Sanjeev on 06/02/2012
                    refundActionsForDisplay("IsSavings")
                    'Comment code Deleted By Sanjeev on 06/02/2012

                Case "BothPlans"


                    'Comment code Deleted By Sanjeev on 06/02/2012
                    refundActionsForDisplay("IsRetirement")
                    'Comment code Deleted By Sanjeev on 06/02/2012
                    refundActionsForDisplay("IsSavings")
                    'Comment code Deleted By Sanjeev on 06/02/2012

            End Select
            '------ Added to implement WebServiece by pavan ---- Begin  -----------
            Me.VoluntryAmount = VoluntryAmount
            Me.YMCAAvailableAmount = YMCAAvailableAmount
            '------ Added to implement WebServiece by pavan ---- END    -----------

            'Comment code Deleted By Sanjeev on 06/02/2012


        Catch
            Throw
        End Try
    End Function

#End Region

#Region " Hardship Refund "

    Private Function HardshipRefund(ByVal parameterPlanType As String)

        Dim l_ContributionDataTable As DataTable
        Try
            Select Case parameterPlanType
                Case "RetirementPlan"

                    If objRefundRequest.DoHardshipRefund("IsRetirement") = True Then
                        refundActions("IsRetirement")
                    End If
                    If objRefundRequest.DoHardshipRefund("IsConsolidated") = True Then
                        refundActions("IsConsolidated")
                    End If
                Case "SavingsPlan"

                    If objRefundRequest.DoHardshipRefund("IsSavings") = True Then
                        refundActions("IsSavings")
                    End If
                    If objRefundRequest.DoHardshipRefund("IsConsolidated") = True Then
                        refundActions("IsConsolidated")
                    End If
                Case "BothPlans"

                    If objRefundRequest.DoHardshipRefund("IsRetirement") = True Then
                        refundActions("IsRetirement")
                    End If

                    If objRefundRequest.DoHardshipRefund("IsSavings") = True Then
                        refundActions("IsSavings")
                    End If

                    If objRefundRequest.DoHardshipRefund("IsConsolidated") = True Then
                        refundActions("IsConsolidated")
                    End If


            End Select
            Me.YMCAAvailableAmount = objRefundRequest.YMCAAvailableAmount


        Catch
            Throw
        End Try
    End Function


#End Region

#Region " Hardship Refund For Display "

    Private Function HardshipRefundForDisplay(ByVal parameterPlanType As String)

        Dim l_ContributionDataTable As DataTable
        Try
            Select Case parameterPlanType
                Case "RetirementPlan"

                    If objRefundRequest.DoHardshipRefundForDisplay("IsRetirement") = True Then
                        refundActionsForDisplay("IsRetirement")
                    End If
                    'Comment code Deleted By Sanjeev on 06/02/2012
                Case "SavingsPlan"

                    'Comment code Deleted By Sanjeev on 06/02/2012
                    refundActionsForDisplay("IsSavings")

                    'Comment code Deleted By Sanjeev on 06/02/2012
                Case "BothPlans"

                    If objRefundRequest.DoHardshipRefundForDisplay("IsRetirement") = True Then
                        refundActionsForDisplay("IsRetirement")
                    End If

                    If objRefundRequest.DoHardshipRefundForDisplay("IsSavings") = True Then
                        refundActionsForDisplay("IsSavings")
                    End If

                    'Comment code Deleted By Sanjeev on 06/02/2012


            End Select
            'Comment code Deleted By Sanjeev on 06/02/2012
            '------ Added to implement WebServiece by pavan ---- Begin  -----------
            Me.YMCAAvailableAmount = YMCAAvailableAmount
            '------ Added to implement WebServiece by pavan ---- END    -----------


        Catch
            Throw
        End Try
    End Function

#End Region

#Region " Personal Refund "

    Private Function PersonalRefund(ByVal parameterPlanType As String)
        Dim l_ContributionDataTable As DataTable
        Try
            Select Case parameterPlanType
                Case "RetirementPlan"

                    objRefundRequest.DoFullOrPersRefund("IsRetirement")
                    refundActions("IsRetirement")
                    objRefundRequest.DoFullOrPersRefund("IsConsolidated")
                    refundActions("IsConsolidated")

                Case "SavingsPlan"

                    objRefundRequest.DoFullOrPersRefund("IsSavings")
                    refundActions("IsSavings")
                    objRefundRequest.DoFullOrPersRefund("IsConsolidated")
                    refundActions("IsConsolidated")

                Case "BothPlans"

                    objRefundRequest.DoFullOrPersRefund("IsRetirement")
                    refundActions("IsRetirement")
                    objRefundRequest.DoFullOrPersRefund("IsSavings")
                    refundActions("IsSavings")
                    objRefundRequest.DoFullOrPersRefund("IsConsolidated")
                    refundActions("IsConsolidated")

            End Select
        Catch
            Throw
        End Try

    End Function

#End Region

#Region " Special Refund "
    'Shubhrata Mar30th,2007 YREN-3183 In accordance to Mark Posey's mail all money has to be withdrawn from all accounts
    'for SPEC refund.
    Private Function SpecialRefund(ByVal parameterPlanType As String)

        Dim l_ContributionDataTable As DataTable

        Try
            Select Case parameterPlanType
                Case "RetirementPlan"

                    objRefundRequest.DoSpecialRefund("IsRetirement")
                    refundActionsForDisplay("IsRetirement")
                    'Comment code Deleted By Sanjeev on 06/02/2012

                Case "SavingsPlan"

                    objRefundRequest.DoSpecialRefund("IsSavings")
                    refundActionsForDisplay("IsSavings")
                    'Comment code Deleted By Sanjeev on 06/02/2012

                Case "BothPlans"

                    objRefundRequest.DoSpecialRefund("IsRetirement")
                    refundActionsForDisplay("IsRetirement")
                    objRefundRequest.DoSpecialRefund("IsSavings")
                    refundActionsForDisplay("IsSavings")
                    'Comment code Deleted By Sanjeev on 06/02/2012

            End Select

        Catch
            Throw
        End Try

    End Function



    '#Region " Disability Refund "

    Private Function DisabilityRefund(ByVal parameterPlanType As String)

        Dim l_ContributionDataTable As DataTable
        Try

            Select Case parameterPlanType
                Case "RetirementPlan"

                    objRefundRequest.DoDisabilityRefund("IsRetirement")
                    refundActionsForDisplay("IsRetirement")
                    'Comment code Deleted By Sanjeev on 06/02/2012

                Case "SavingsPlan"

                    objRefundRequest.DoDisabilityRefund("IsSavings")
                    refundActionsForDisplay("IsSavings")
                    'Comment code Deleted By Sanjeev on 06/02/2012

                Case "BothPlans"

                    objRefundRequest.DoDisabilityRefund("IsRetirement")
                    refundActionsForDisplay("IsRetirement")
                    objRefundRequest.DoDisabilityRefund("IsSavings")
                    refundActionsForDisplay("IsSavings")
                    'Comment code Deleted By Sanjeev on 06/02/2012

            End Select
        Catch
            Throw
        End Try

    End Function

#End Region

#End Region

#Region "Events"

    Private Sub OnCheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles datagridCheckBox.CheckedChanged

        Dim l_CheckBox As CheckBox
        Dim l_CheckBox_AM As CheckBox
        Dim l_DataGridItem As DataGridItem
        Dim l_Counter As Integer
        Dim l_bool_SelectAMAccount As Boolean = False
        Dim l_bool_SelectAcct As Boolean = False
        Dim l_string_MessageForRT As String
        Dim l_CalculatedDataTable As New DataTable
        Dim l_drCalculatedDataTable As DataRow
        Dim blnRefundType As Boolean = False
        Dim retirementPlanTable As DataTable
        Dim savingsPlanTable As DataTable

        Try
            l_Counter = 0

            If Me.RefundType.Trim.ToUpper() <> "SPEC" And Me.RefundType.Trim.ToUpper() <> "DISAB" Then
                If CheckboxExcludeYMCA.Checked = True Then
                    Me.RefundType = "PERS"
                End If
            End If


            'Comment code Deleted By Sanjeev on 06/02/2012

            'PhaseV BA Account 10-04-2009 by Dilip
            '------ Added to implement WebServiece by pavan ---- Begin  -----------
            If CheckboxVoluntryAccounts.Checked = True Then
                Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = Session("VoluntryRetirementDatatable_C19")
            End If
            '------ Added to implement WebServiece by pavan ---- END    -----------
            l_CalculatedDataTable = Session("Calculated_DisplayRetirementPlanAcctContribution_C19")

            'START: MMR | 2020.04.24 | YRS-AT-4854 | Get Blended tax rate and apply tax rate on retirement plan account and savings plan account
            If Not Session("Calculated_DisplayRetirementPlanAcctContribution_C19") Is Nothing Then
                retirementPlanTable = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable)
            End If

            If Not Session("Calculated_DisplaySavingsPlanAcctContribution_C19") Is Nothing Then
                savingsPlanTable = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable)
            End If

            Me.BlendedTaxRate = Me.GetBlendedTaxRate(retirementPlanTable, savingsPlanTable, "Retirement")

            'Calculating tax as per Blended tax rate and updating in display table for savings plan
            retirementPlanTable = UpdateDisplayTableTaxValueWithBlendedTaxRate(retirementPlanTable, Me.BlendedTaxRate)

            'Calculating tax as per Blended tax rate and updating in display table for retirement plan
            If HelperFunctions.isNonEmpty(savingsPlanTable) Then
                savingsPlanTable = UpdateDisplayTableTaxValueWithBlendedTaxRate(savingsPlanTable, Me.BlendedTaxRate)
                'Binding retirement plan data to grid after tax calculation for updated data display
                BindDataGrid("IsSavings", savingsPlanTable)
            End If

            'Updating session object after recalculating tax for retirement plan
            Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = retirementPlanTable

            'Updating session object after recalculating tax for savings plan
            Session("Calculated_DisplaySavingsPlanAcctContribution_C19") = savingsPlanTable

            'Display calculated Covid taxable and non-taxable amount on UI
            Me.DisplayCovidTaxableAndNonTaxableAmount()
            'END: MMR | 2020.04.24 | YRS-AT-4854 | Get Blended tax rate and apply tax rate on retirement plan account and savings plan account

            If Me.RefundType = "VOL" Then
                For Each l_DataGridItem In Me.DataGridAccContributionRetirement.Items

                    l_CheckBox = l_DataGridItem.FindControl("Selected")

                    If l_DataGridItem.Cells(2).Text.ToString() = "AM-Matched" Then
                        If l_CheckBox.Checked = True Then
                            l_bool_SelectAMAccount = True
                            'Added by Dilip on 02-12-2009
                            Me.Bool_NotIncludeAMMatched = False
                        ElseIf l_CheckBox.Checked = False Then
                            Me.Bool_NotIncludeAMMatched = True
                            'Added by Dilip on 02-12-2009
                        End If
                    End If
                    If Not l_CheckBox Is Nothing Then

                        If l_CheckBox.Checked = True Then
                            Me.MakeValuntryCalculationTableForDisplay(l_Counter, True, "IsRetirement", l_bool_SelectAMAccount)
                        Else
                            Me.MakeValuntryCalculationTableForDisplay(l_Counter, False, "IsRetirement", l_bool_SelectAMAccount)
                        End If

                    End If

                    l_Counter += 1

                Next
            End If

            If Me.RefundType = "PERS" Then
                For Each l_DataGridItem In Me.DataGridAccContributionRetirement.Items

                    l_CheckBox = l_DataGridItem.FindControl("Selected")


                    If l_CheckBox.Checked = True Then
                        l_bool_SelectAMAccount = True
                    End If

                    If Not l_CheckBox Is Nothing Then

                        If l_CheckBox.Checked = True Then
                            Me.MakePersonalCalculationTableForDisplay(l_Counter, True, "IsRetirement", l_bool_SelectAMAccount)
                        Else
                            Me.MakePersonalCalculationTableForDisplay(l_Counter, False, "IsRetirement", l_bool_SelectAMAccount)
                        End If

                    End If

                    l_Counter += 1

                Next
            End If

            If Me.RefundType = "PERS" Then
                For Each l_DataGridItem In Me.DataGridAccContributionRetirement.Items
                    l_CheckBox = l_DataGridItem.FindControl("Selected")
                    If l_DataGridItem.Cells(2).Text.ToString() = m_const_YMCA_Legacy_Acct And l_CheckBox.Checked = True Then
                        For intcnt As Integer = 0 To l_CalculatedDataTable.Rows.Count - 1
                            l_drCalculatedDataTable = l_CalculatedDataTable.Rows(intcnt)
                            If l_drCalculatedDataTable("AccountType").GetType.ToString <> "System.DBNull" Then
                                'Modified for WebServiece by pavan IsAnnuityExists("IsRetirement")
                                If l_drCalculatedDataTable("AccountType") = m_const_YMCA__Acct And Me.BACurrent < 5000 And AnnuityExists_Retirement = False Then
                                    l_drCalculatedDataTable("Selected") = "True"
                                End If
                            End If
                        Next
                    Else
                    End If
                Next
            End If


            'PhaseV BA Account 10-04-2009 by Dilip

            Me.CalculateTotalForDisplay("IsRetirement")
            Me.LoadCalculatedTable("IsRetirement")
            fillNetValuesforAddedControls(Me.Session("Calculated_DisplayRetirementPlanAcctContribution_C19"))

            'Session("CalculatedDataTableDisplay")
            'New Modification for Market Based Withdrawal Amit Nigam
            If LabelMarket.Visible = True Then
                If CheckboxPartialRetirement.Checked = False Or CheckboxPartialSavings.Checked = False Then
                    'DisplayMarketInstallments()
                Else
                    ClearMarketTextBoxes()
                End If

            End If
            'New Modification for Market Based Withdrawal Amit Nigam






            'Comment code Deleted By Sanjeev on 06/02/2012

            If Me.CheckboxSavingsVoluntary.Checked = True Or Me.CheckboxHardship.Checked = True Then
                If Me.RefundType = "PERS" Then
                    blnRefundType = True
                    Me.RefundType = "VOL"
                End If
                Me.IsAccountSelectionChangeFromRetirement = True 'MMR | 2020.04.24 | YRS-AT-4854 | Setting property value to true to indicate in savings checkbox event that code is being executed from retirement grid checkbox event
                datagridCheckBox_Savings_CheckedChanged(Me.datagridCheckBox_Savings, e)
                Me.IsAccountSelectionChangeFromRetirement = False 'MMR | 2020.04.24 | YRS-AT-4854 | Resetting property value to false
                Me.MakeDisplayCalculationDataTables(True, True)
            Else
                Me.MakeDisplayCalculationDataTables(True, False)
            End If

            If blnRefundType = True Then
                Me.RefundType = "PERS"
            End If
            'Added by Ganeswar on 17-06-2009 for Savings Plan Total

            'Comment code Deleted By Sanjeev on 06/02/2012


            If Me.TotalAmount > 0 Then
                Me.ButtonSave.Visible = True
            Else
                Me.ButtonSave.Visible = False
            End If
            PopulateConsolidatedTotals()
            '------ Added to implement WebServiece by pavan ---- Begin  -----------
            MakeFinalDataTables("IsRetirement")
            MakeFinalDataTables("IsSavings")
            '------ Added to implement WebServiece by pavan ---- END    -----------

            DoNotForceSavingPlanWithdrawal(Me.RetirementPlan_TotalAmount) 'SR:2013.12.26-BT 2240:YRS 5.0-2226:modification to partial withdrawal processing


        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-datagridCheckBox.CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Private Sub TextboxTaxRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxTaxRate.TextChanged

        Dim l_TaxedAmount As Decimal
        Dim l_NonTaxedAmount As Decimal

        Try


            'Assign Federal Rate to Rate Text box
            If Me.TextboxTaxRate.Text.Trim.Length > 0 Then
                Me.FederalTaxRate = CType(Me.TextboxTaxRate.Text.Trim, Decimal)
                Me.TextboxTaxRate.Text = Me.FederalTaxRate.ToString("0")
            Else
                'Me.FederalTaxRate = 20.0 'Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code to remove harcoded tax rate
                Me.TextboxTaxRate.Text = Me.FederalTaxRate.ToString("0")
            End If
            'Comment code Deleted By Sanjeev on 06/02/2012
            If Me.FederalTaxRate < 20 Or Me.FederalTaxRate > 100 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", " Tax Rate should be from 20% to 100%", MessageBoxButtons.Stop, False)
                'Me.FederalTaxRate = 20.0 'Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code to remove harcoded tax rate
                Me.TextboxTaxRate.Text = Me.FederalTaxRate.ToString("0")
            End If

            'End If

            If Me.TextboxNonTaxed.Text.Trim.Length < 1 Then
                l_NonTaxedAmount = 0.0
            Else
                l_NonTaxedAmount = CType(Me.TextboxNonTaxed.Text.Trim, Decimal)
            End If

            If Me.TextboxTaxed.Text.Trim.Length < 1 Then
                l_TaxedAmount = 0.0
            Else
                l_TaxedAmount = CType(Me.TextboxTaxed.Text.Trim, Decimal)
            End If

            Me.TextboxTax.Text = ((l_TaxedAmount) * (Me.FederalTaxRate / 100.0)).ToString("0.00")

            Me.TextboxNet.Text = (((l_NonTaxedAmount) + (l_TaxedAmount)) - ((l_TaxedAmount) * (Me.FederalTaxRate / 100.0))).ToString("0.00")

        Catch caEx As System.InvalidCastException
            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Invalid Tax Rate entered. Please enter the Valid Tax Rate.", MessageBoxButtons.Stop, False)
            'Me.FederalTaxRate = 20.0 'Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code to remove harcoded tax rate
            Me.TextboxTaxRate.Text = Me.FederalTaxRate.ToString("0")
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-TextboxTaxRate.TextChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Private Sub ButtonCalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim l_CheckBox As CheckBox

        Try

            For Each l_DataGridItem As DataGridItem In Me.DataGridAccountContribution.Items

                l_CheckBox = l_DataGridItem.FindControl("Selected")

                If l_CheckBox.Checked = True Then

                End If
            Next


        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-ButtonCalculate_Click", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Try
            Dim l_SaveStatus As Boolean
            Dim l_ymcaAccountsCheckBoxStatus As Boolean = True
            Me.IsMessageCheck = False
            Dim l_CheckBox As CheckBox
            Dim l_exclude As Boolean
            Dim l_stringMessagePartialRet As String
            Dim l_stringMessagePartialSav As String

            AllowMarketBasedWithdrawalForRetirementPlan()
            AllowMarketBasedWithdrawalForSavingsPlan()

            '------ Added to implement WebServiece by pavan ---- Begin  -----------
            BindDataGrid("IsRetirement", CType(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable))
            BindDataGrid("IsSavings", CType(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable))
            '------ Added to implement WebServiece by pavan ---- END    -----------

            'Added the Condition to close the page if the save buton is clicked more then one time-Amit -10-09-2009
            If objRefundRequest.IsSaveProcessComplete = False Then
                'Added the Condition to close the page if the save buton is clicked more then one time-Amit -10-09-2009
                If Me.CheckboxExcludeYMCA.Checked = True Then
                    For Each l_DataGridItem As DataGridItem In Me.DataGridAccContributionRetirement.Items
                        l_CheckBox = l_DataGridItem.Cells(0).Controls(0)
                        If (l_CheckBox.Checked = False) And (l_DataGridItem.Cells(2).Text = m_const_YMCA__Acct Or l_DataGridItem.Cells(2).Text = m_const_YMCA_Legacy_Acct) Then
                            l_ymcaAccountsCheckBoxStatus = False
                            Exit For
                        End If
                    Next
                    If l_ymcaAccountsCheckBoxStatus = True Then
                        Dim l_stringMessage As String
                        'l_messageBoxCheck = True
                        Me.IsMessageCheck = True
                        l_stringMessage = "None of the YMCA account(s) are excluded. Please deselect any of the YMCA account(s) to save the Withdrawal Request."
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA", l_stringMessage, MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                End If

                'modified to check validity of partial withdrawal
                If CheckboxPartialRetirement.Checked = True And _
                CheckboxPartialSavings.Checked = True And _
                TextboxPartialRetirement.Text = String.Empty And _
                TextboxPartialSavings.Text = String.Empty Then
                    l_stringMessagePartialRet = "Please enter the requested amount in Retirement Plan & Saving Plan to save the Partial Withdrawal Request."
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA", l_stringMessagePartialRet, MessageBoxButtons.Stop)
                End If
                If CheckboxPartialRetirement.Checked = True Then
                    If TextboxPartialRetirement.Text = String.Empty Then
                        l_stringMessagePartialRet = "Please enter the requested amount in Retirement Plan to save the Partial Withdrawal Request."
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA", l_stringMessagePartialRet, MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                End If
                If CheckboxPartialSavings.Checked = True Then
                    If TextboxPartialSavings.Text = String.Empty Then
                        l_stringMessagePartialSav = "Please enter the requested amount in Saving Plan to save the Partial Withdrawal Request."
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA", l_stringMessagePartialSav, MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                End If
                'modified to check validity of partial withdrawal


                'Plan Split changes
                'Since the check boxes are no more visible to the user the message is not required.(Shubhrata)
                If Me.CheckboxRetirementPlan.Checked = False And Me.CheckboxSavingsPlan.Checked = False Then
                    'commented by Shubhrata on 31-Jan-2008 in response to bug id 355(re-opened)
                    ' MessageBox.Show(MessageBoxPlaceHolder, "YMCA", "Please select either Retirement or Savings plan or both.", MessageBoxButtons.Stop)
                    Exit Sub
                End If
                'Disable the partial checkboxes when market based withdrawal is enabaled-Amit


                'Comment code Deleted By Sanjeev on 06/02/2012

                If (Me.CheckboxRegular.Checked = True Or CheckboxExcludeYMCA.Checked = True Or CheckboxVoluntryAccounts.Checked = True) And Me.AllowMarketBasedForRetirementPlan = True And CheckboxPartialSavings.Checked = True And Me.AllowMarketBased = True Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA", "Partial Withdrawal is not allowed if the Market based withdrawal is enabled in one Plan.", MessageBoxButtons.Stop)
                    Exit Sub
                End If
                'Comment code Deleted By Sanjeev on 06/02/2012

                If (Me.CheckboxSavingsVoluntary.Checked = True And Me.AllowMarketBasedForSavingsPlan = True And (Me.CheckboxPartialRetirement.Checked = True) And Me.AllowMarketBased = True) Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA", "Partial Withdrawal is not allowed if the Market based withdrawal is enabled in one Plan.", MessageBoxButtons.Stop)
                    Exit Sub
                End If

                'Comment code Deleted By Sanjeev on 06/02/2012

                'START: SG: 2012.06.14: BT-1043
                CheckeckPlanType()

                Dim drPriorMRD As DataRow()

                If Me.FinalDataSet Is Nothing Then
                    Me.FinalDataSet = Session("FinalDataSet_C19")
                End If

                If Not FinalDataSet.Tables("MRDRecords") Is Nothing AndAlso FinalDataSet.Tables("MRDRecords").Rows.Count > 0 AndAlso Me.RefundType <> "SPEC" AndAlso Me.RefundType <> "DISAB" Then
                    If Me.CheckboxSavingsPlan.Checked = True AndAlso Me.CheckboxRetirementPlan.Checked = True AndAlso Me.CheckboxMRDSavings.Checked = False AndAlso Me.CheckboxMRDRetirement.Checked = False Then
                        drPriorMRD = FinalDataSet.Tables("MRDRecords").Select("dtmExpireDate < '" & System.DateTime.Now().AddDays(-1) & "' AND Balance > 0 ")
                    ElseIf Me.CheckboxSavingsPlan.Checked = True AndAlso Me.CheckboxMRDSavings.Checked = False Then
                        drPriorMRD = FinalDataSet.Tables("MRDRecords").Select("PlanType = 'SAVINGS' AND dtmExpireDate < '" & System.DateTime.Now().AddDays(-1) & "' AND Balance > 0 ")
                    ElseIf Me.CheckboxRetirementPlan.Checked = True AndAlso Me.CheckboxMRDRetirement.Checked = False Then
                        drPriorMRD = FinalDataSet.Tables("MRDRecords").Select("PlanType = 'RETIREMENT' AND dtmExpireDate < '" & System.DateTime.Now().AddDays(-1) & "' AND Balance > 0 ")
                    End If
                End If

                'If (Not drPriorMRD Is Nothing AndAlso drPriorMRD.Length > 0 AndAlso Me.SessionStatusType.Trim() <> "AE" AndAlso Me.SessionStatusType.Trim() <> "RA") Then
                If (Not drPriorMRD Is Nothing AndAlso drPriorMRD.Length > 0 AndAlso objRefundProcess.IsTerminatedEmployment()) Then  '2015.06.25/SR-  BT 2903/YRS-AT-2542 - YRS bug discovered in which RMD (Required Minimum Distribution) records are being marked as eligible-yes for participants with pre-eligible (PE or RE) Fund statuses.
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Unfulfilled RMD amounts for prior periods are inlcuded.  Do you wish to continue?", MessageBoxButtons.YesNo, False)
                    Session("PriorYearRMDRecords_C19") = "Yes"
                    Exit Sub
                Else
                    DoBtnSaveProcessing()
                End If

                'l_SaveStatus = Me.SaveRefund()

                '' This satement for Refresh the DataGrid in Parent form.
                'If l_SaveStatus = True Then
                '    Me.SessionIsRefundRequest = True

                '    'Comment code Deleted By Sanjeev on 06/02/2012
                '    Response.Write("<script> function process(){window.opener.document.forms(0).submit(); self.close();}process();</script>")
                '    objRefundRequest.IsSaveProcessComplete = True
                'End If
                'Added the Condition to close the page if the save buton is clicked more then one time-Amit -10-09-2009
                'END: SG: 2012.06.14: BT-1043
            Else
                Response.Write("<script> function process(){window.opener.document.forms(0).submit(); self.close();}process();</script>")
                Exit Sub
            End If
            'Comment code Deleted By Sanjeev on 06/02/2012

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-ButtonSave_Click", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub CheckboxVoluntryAccounts_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckboxVoluntryAccounts.CheckedChanged
        Try
            'Added by Dilip on 02-12-2009
            Me.Bool_NotIncludeAMMatched = False
            'Added by Dilip on 02-12-2009
            CheckboxRetirementPlan.Checked = True
            ''CheckboxSavingsPlan.Checked = False
            Me.NumPercentageFactorofMoneyRetirement = 1
            ControlAllcheckBoxes("VolRet", CheckboxVoluntryAccounts.Checked, "R")
            '------ Added to implement WebServiece by pavan ---- Begin  -----------
            MakeFinalDataTables("IsRetirement")
            MakeFinalDataTables("IsSavings")
            '------ Added to implement WebServiece by pavan ---- END    -----------

            DoNotForceSavingPlanWithdrawal(Me.RetirementPlan_TotalAmount) 'SR:2013.12.26-BT 2240:YRS 5.0-2226:modification to partial withdrawal processing
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxVoluntryAccounts_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub CheckboxHardship_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckboxHardship.CheckedChanged

        Try
            'Added by Dilip on 02-12-2009
            Me.Bool_NotIncludeTMMatched = False
            'Added by Dilip on 02-12-2009
            CheckboxSavingsPlan.Checked = True
            ControlAllcheckBoxes("Hardship", CheckboxHardship.Checked, "S")
            DivIRSOverrideRules.Visible = IIf(CheckboxHardship.Checked, True, False) 'CS | 2016.09.23 | YRS-AT-3164 - To Visiable /Invisiable Check box of IRS Override Hard ship rules
            '------ Added to implement WebServiece by pavan ---- Begin  -----------
            MakeFinalDataTables("IsRetirement")
            MakeFinalDataTables("IsSavings")
            '------ Added to implement WebServiece by pavan ---- END    -----------

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxHardship_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Private Sub CheckboxPersonalSide_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim popupScript1 As String
        Try
            If CheckboxExcludeYMCA.Checked = True Then
                'Plan Split Changes
                'Do not allow Retirement Plan for CurrentPIA >= PIAMin to Retire and RetirePersTotAmt=0 And terminated
                If Me.AllowedPersonalSideRefund = False Then
                    Me.CheckboxRetirementPlan.Checked = False
                    Me.CheckboxRetirementPlan.Enabled = False
                    If Me.SavingsPlan_TotalAmount > 0 Then
                        Me.CheckboxSavingsPlan.Enabled = True
                        LabelMessage.Visible = False
                    Else
                        LabelMessage.Visible = True
                        LabelMessage.Text = "* Refund is not allowed as Current PIA is less than PIA Minimum to retire"
                        'Comment code Deleted By Sanjeev on 06/02/2012
                    End If
                End If

                Me.IsTypeChoosen = True
                Me.RefundType = "PERS"

                'Copy from the Source DataTabe
                If Me.CheckboxSavingsPlan.Checked = True And Me.CheckboxRetirementPlan.Checked = False Then
                    Session("OnlySavingsPlan_C19") = True
                    Session("OnlyRetirementPlan_C19") = False
                    Session("BothPlans_C19") = False
                ElseIf Me.CheckboxRetirementPlan.Checked = True And Me.CheckboxSavingsPlan.Checked = False Then
                    Session("OnlySavingsPlan_C19") = False
                    Session("OnlyRetirementPlan_C19") = True
                    Session("BothPlans_C19") = False
                ElseIf Me.CheckboxSavingsPlan.Checked = True And Me.CheckboxRetirementPlan.Checked = True Then
                    Session("OnlySavingsPlan_C19") = False
                    Session("OnlyRetirementPlan_C19") = False
                    Session("BothPlans_C19") = True
                Else
                    Session("OnlySavingsPlan_C19") = False
                    Session("OnlyRetirementPlan_C19") = False
                    Session("BothPlans_C19") = False
                End If
                Me.CopyAccountContributionTable()
                If Session("OnlyRetirementPlan_C19") = True Then
                    Me.PersonalRefund("RetirementPlan")
                ElseIf Session("OnlySavingsPlan_C19") = True Then
                    Me.PersonalRefund("SavingsPlan")
                ElseIf Session("BothPlans_C19") = True Then
                    Me.PersonalRefund("BothPlans")
                Else

                    Me.objRefundRequest.DoFullOrPersRefund("IsConsolidated")
                    Me.refundActions("IsConsolidated")
                End If



                Me.CheckboxVoluntryAccounts.Enabled = False
                Me.CheckboxVoluntryAccounts.Checked = False

                Me.CheckboxHardship.Enabled = False
                Me.CheckboxHardship.Checked = False

                Me.CheckboxRegular.Enabled = False
                Me.CheckboxRegular.Checked = False
                If Me.TotalAmount > 0 Then
                    Me.ButtonSave.Visible = True
                Else
                    Me.ButtonSave.Visible = False
                End If
                'AllowRefundRequest()
            Else
                LabelMessage.Visible = False
                Me.IsTypeChoosen = False
                Me.RefundType = ""

                'Copy from the Source DataTabe
                'added Plan Split Changes
                Me.GetConsolidatedAvailableContributions()
                'added Plan Split Changes
                'Comment code Deleted By Sanjeev on 06/02/2012

                Me.ButtonSave.Visible = False

                Me.SetCheckBoxOnLoad()
                AllowFullRefund()

            End If
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxPersonalSide_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub CheckboxSpecial_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckboxSpecial.CheckedChanged
        Try
            If Me.CheckboxSpecial.Checked = True Then
                Me.ButtonSave.Visible = True
                Me.ButtonSave.Enabled = True
            End If
            If Me.TotalAmount > 0 Then
                Me.ButtonSave.Visible = True
            Else
                Me.ButtonSave.Visible = False
            End If

            Me.SetCheckBoxOnLoad()
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxSpecial_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Me.SessionIsRefundPopupAllowed = True
        ClearSession()
        Me.Response.Write("<script language='javascript'> { self.close() }</script>")
        '  Response.Write("<script> window.opener.location.reload(); self.close();" & Chr(60) & "/script>")

    End Sub

    'Comment code Deleted By Sanjeev on 06/02/2012

    'Comment code Deleted By Sanjeev on 06/02/2012
    'Comment code Deleted By Sanjeev on 06/02/2012

    Private Sub CheckboxDisability_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxDisability.CheckedChanged

        Try
            If Me.CheckboxDisability.Checked = True Then

                Me.ButtonSave.Visible = True
                Me.ButtonSave.Enabled = True
            End If
            If Me.TotalAmount > 0 Then
                Me.ButtonSave.Visible = True
            Else
                Me.ButtonSave.Visible = False
            End If

            'Me.SetCheckBoxes()
            Me.SetCheckBoxOnLoad()
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxDisability_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Private Sub AddFileListRow(ByVal p_String_SourceFolder As String, ByVal p_String_SourceFile As String, ByVal p_String_DestFolder As String, ByVal p_String_DestFile As String)
        Dim drFileRow As DataRow
        Try
            If p_String_SourceFolder <> "" And p_String_SourceFile <> "" And p_String_DestFolder <> "" And p_String_DestFile <> "" Then
                drFileRow = dtFileList.NewRow()
                drFileRow("SourceFolder") = p_String_SourceFolder
                If Right(p_String_SourceFolder, 1) = "\" Then
                    drFileRow("SourceFile") = p_String_SourceFolder & p_String_SourceFile
                Else
                    drFileRow("SourceFile") = p_String_SourceFolder & "\" & p_String_SourceFile
                End If

                drFileRow("DestFolder") = p_String_DestFolder
                If Right(p_String_DestFolder, 1) = "\" Then
                    drFileRow("DestFile") = p_String_DestFolder & p_String_DestFile
                Else
                    drFileRow("DestFile") = p_String_DestFolder & "\" & p_String_DestFile
                End If

                dtFileList.Rows.Add(drFileRow)
            End If
        Catch
            Throw
        End Try
    End Sub

    'Plan Split Changes May23rd,2007 Shubhrata
    Private Sub DataGridAccContributionRetirement_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridAccContributionRetirement.ItemDataBound
        Try
            'Comment code Deleted By Sanjeev on 06/02/2012
            'Phase V BA Account by Dilip
            If (Me.RefundType = "PERS" Or Me.RefundType = "VOL") And CheckboxExcludeYMCA.Checked = True Then
                'Comment code Deleted By Sanjeev on 06/02/2012
                Dim dgItems As DataGridItem
                dgItems = e.Item
                Dim CheckboxSelected As CheckBox = CType(dgItems.Cells(0).FindControl("Selected"), CheckBox)

                If (dgItems.Cells(2).Text = m_const_YMCA_Legacy_Acct Or dgItems.Cells(2).Text = m_const_YMCA__Acct) And CheckboxExcludeYMCA.Checked = True And Not (Me.RefundType = "VOL") Then

                    If Me.CurrentPIA <= Me.MinimumPIAToRetire And Me.BACurrent <= Me.MinimumPIAToRetire And (Me.BACurrent + Me.CurrentPIA) > Me.MinimumPIAToRetire Then
                        CheckboxSelected.Checked = False
                        CheckboxSelected.Enabled = False
                    Else
                        If CheckboxExcludeYMCA.Checked = True Then
                            CheckboxSelected.Enabled = True
                        ElseIf CheckboxRegular.Checked = True Then
                            CheckboxSelected.Enabled = False
                        End If
                    End If
                    'Added by Amit May 18,2009 to fix the excluding error of Voluntary Account-Start
                ElseIf CheckboxVoluntryAccounts.Checked = True And Me.RefundType = "VOL" And Not (CheckboxSelected Is Nothing) Then
                    CheckboxSelected.Enabled = True
                    'Added by Amit May 18,2009 to fix the excluding error of Voluntary Account-End
                End If
                'End If
            End If
            'Phase V BA Account by Dilip
            If e.Item.Cells.Count = 9 Then
                e.Item.Cells(0).Visible = False
                e.Item.Cells(2).Visible = False
                e.Item.Cells(3).Visible = False
            ElseIf e.Item.Cells.Count > 9 Then
                e.Item.Cells(1).Visible = False
                e.Item.Cells(3).Visible = False
                e.Item.Cells(4).Visible = False
                e.Item.Cells(10).Visible = False
                e.Item.Cells(11).Visible = False
            End If

            If e.Item.ItemType <> ListItemType.Header Then

                If e.Item.Cells(0).Text.ToUpper <> "&NBSP;" Then


                    Dim l_decimal_try As Decimal
                    '''l_decimal_try = Convert.ToDecimal(e.Item.Cells(10).Text)
                    '''e.Item.Cells(10).Text = FormatCurrency(l_decimal_try)
                    If e.Item.Cells.Count = 8 Then
                        If e.Item.Cells(4).Text.ToUpper <> "&NBSP;" Then
                            l_decimal_try = Convert.ToDecimal(e.Item.Cells(4).Text)
                            e.Item.Cells(4).Text = FormatCurrency(l_decimal_try)
                        End If
                    End If
                    If e.Item.Cells(5).Text.ToUpper <> "&NBSP;" Then
                        l_decimal_try = Convert.ToDecimal(e.Item.Cells(5).Text)
                        e.Item.Cells(5).Text = FormatCurrency(l_decimal_try)
                    End If
                    If e.Item.Cells(6).Text.ToUpper <> "&NBSP;" Then
                        l_decimal_try = Convert.ToDecimal(e.Item.Cells(6).Text)
                        e.Item.Cells(6).Text = FormatCurrency(l_decimal_try)
                    End If
                    If e.Item.Cells(7).Text.ToUpper <> "&NBSP;" Then
                        l_decimal_try = Convert.ToDecimal(e.Item.Cells(7).Text)
                        e.Item.Cells(7).Text = FormatCurrency(l_decimal_try)
                    End If
                    If e.Item.Cells.Count > 8 Then
                        If e.Item.Cells(8).Text.ToUpper <> "&NBSP;" Then
                            l_decimal_try = Convert.ToDecimal(e.Item.Cells(8).Text)
                            e.Item.Cells(8).Text = FormatCurrency(l_decimal_try)
                        End If
                    End If
                    'Added for Masking-Amit 15-03-2010
                    If e.Item.Cells.Count > 9 Then
                        If e.Item.Cells(9).Text.ToUpper <> "&NBSP;" Then
                            l_decimal_try = Convert.ToDecimal(e.Item.Cells(9).Text)
                            e.Item.Cells(9).Text = FormatCurrency(l_decimal_try)
                        End If
                    End If
                    'Added for Masking-Amit 15-03-2010
                End If
            End If
            'Added By Parveen To Hide The CheckBox in Case Of Market Based Withdrawal
            If e.Item.Cells(2).Text = "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)" Then
                Dim CheckboxSelected As CheckBox = CType(e.Item.Cells(0).FindControl("Selected"), CheckBox)
                CheckboxSelected.Visible = False
                e.Item.BackColor = System.Drawing.Color.FromArgb(201, 219, 237)
            End If
            'Added By Parveen To Hide The CheckBox in Case Of Market Based Withdrawal

        Catch

            Throw

        End Try
    End Sub

    Private Sub DataGridAccContributionSavings_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridAccContributionSavings.ItemDataBound
        Try
            If Me.DataGridAccContributionSavings.DataSource Is Nothing Then
                Exit Sub
            End If

            If e.Item.Cells.Count = 9 Then
                e.Item.Cells(0).Visible = False
                e.Item.Cells(2).Visible = False
                e.Item.Cells(3).Visible = False
            ElseIf e.Item.Cells.Count > 9 Then
                e.Item.Cells(1).Visible = False
                e.Item.Cells(3).Visible = False
                e.Item.Cells(4).Visible = False
                e.Item.Cells(10).Visible = False
                e.Item.Cells(11).Visible = False
            End If

            If e.Item.ItemType <> ListItemType.Header Then

                If e.Item.Cells(0).Text.ToUpper <> "&NBSP;" Then


                    Dim l_decimal_try As Decimal
                    '''l_decimal_try = Convert.ToDecimal(e.Item.Cells(10).Text)
                    '''e.Item.Cells(10).Text = FormatCurrency(l_decimal_try)
                    If e.Item.Cells.Count = 8 Then
                        If e.Item.Cells(4).Text.ToUpper <> "&NBSP;" Then
                            l_decimal_try = Convert.ToDecimal(e.Item.Cells(4).Text)
                            e.Item.Cells(4).Text = FormatCurrency(l_decimal_try)
                        End If
                    End If
                    If e.Item.Cells(5).Text.ToUpper <> "&NBSP;" Then
                        l_decimal_try = Convert.ToDecimal(e.Item.Cells(5).Text)
                        e.Item.Cells(5).Text = FormatCurrency(l_decimal_try)
                    End If
                    If e.Item.Cells(6).Text.ToUpper <> "&NBSP;" Then
                        l_decimal_try = Convert.ToDecimal(e.Item.Cells(6).Text)
                        e.Item.Cells(6).Text = FormatCurrency(l_decimal_try)
                    End If
                    If e.Item.Cells(7).Text.ToUpper <> "&NBSP;" Then
                        l_decimal_try = Convert.ToDecimal(e.Item.Cells(7).Text)
                        e.Item.Cells(7).Text = FormatCurrency(l_decimal_try)
                    End If
                    If e.Item.Cells.Count > 8 Then
                        If e.Item.Cells(8).Text.ToUpper <> "&NBSP;" Then
                            l_decimal_try = Convert.ToDecimal(e.Item.Cells(8).Text)
                            e.Item.Cells(8).Text = FormatCurrency(l_decimal_try)
                        End If
                    End If
                    'Added for Masking-Amit 15-03-2010
                    If e.Item.Cells.Count > 9 Then
                        If e.Item.Cells(9).Text.ToUpper <> "&NBSP;" Then
                            l_decimal_try = Convert.ToDecimal(e.Item.Cells(9).Text)
                            e.Item.Cells(9).Text = FormatCurrency(l_decimal_try)
                        End If
                    End If
                    'Added for Masking-Amit 15-03-2010

                    'Start : AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to disable select checkbox for ln & ld accounts 
                    If (e.Item.Cells(2).Text.ToUpper.Trim = "LD" OrElse e.Item.Cells(2).Text.ToUpper.Trim = "LN") Then
                        Dim CheckboxSelected As CheckBox = CType(e.Item.Cells(0).FindControl("Selected"), CheckBox)
                        CheckboxSelected.Checked = False
                        CheckboxSelected.Enabled = False
                    End If
                    'End : AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to disable select checkbox for ln & ld accounts 

                End If
            End If
            'Added By Parveen To Hide The CheckBox in Case Of Market Based Withdrawal
            If e.Item.Cells(2).Text = "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)" Then
                Dim CheckboxSelected As CheckBox = CType(e.Item.Cells(0).FindControl("Selected"), CheckBox)
                CheckboxSelected.Visible = False
                e.Item.BackColor = System.Drawing.Color.FromArgb(201, 219, 237)
            End If
            'Added By Parveen To Hide The CheckBox in Case Of Market Based Withdrawal
            'End If
        Catch

            Throw
        End Try
    End Sub

    Private Sub datagridCheckBox_Savings_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles datagridCheckBox_Savings.CheckedChanged
        Dim l_CheckBox As CheckBox
        Dim l_DataGridItem As DataGridItem
        Dim l_Counter As Integer
        Dim l_bool_SelectTMAccount As Boolean = False
        Dim l_bool_SelectAcct As Boolean = False
        Dim l_string_MessageForRT As String = Nothing
        Dim retirementPlanTable As DataTable
        Dim savingsPlanTable As DataTable

        Try
            'START: MMR | 2020.04.24 | YRS-AT-4854 | Get Blended tax rate and apply tax rate on retirement plan account and savings plan account
            If Not Me.IsAccountSelectionChangeFromRetirement Then 'Check if this checkbox change event is being fired on retirement grid checkbox change event than no need to execute covid related code to avoid multiple callings and unusual behaviour on UI
                If Not Session("Calculated_DisplayRetirementPlanAcctContribution_C19") Is Nothing Then
                    retirementPlanTable = DirectCast(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable)
                End If

                If Not Session("Calculated_DisplaySavingsPlanAcctContribution_C19") Is Nothing Then
                    savingsPlanTable = DirectCast(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable)
                End If

                Me.BlendedTaxRate = Me.GetBlendedTaxRate(retirementPlanTable, savingsPlanTable, "Savings")

                'Calculating tax as per Blended tax rate and updating in display table for retirement plan
                If HelperFunctions.isNonEmpty(retirementPlanTable) Then
                    retirementPlanTable = UpdateDisplayTableTaxValueWithBlendedTaxRate(retirementPlanTable, Me.BlendedTaxRate)
                    'Binding retirement plan data to grid after tax calculation for updated data display
                    BindDataGrid("IsRetirement", retirementPlanTable)
                End If

                'Calculating tax as per Blended tax rate and updating in display table for savings plan
                savingsPlanTable = UpdateDisplayTableTaxValueWithBlendedTaxRate(savingsPlanTable, Me.BlendedTaxRate)

                'Updating session object after recalculating tax for retirement plan
                Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = retirementPlanTable

                'Updating session object after recalculating tax for savings plan
                Session("Calculated_DisplaySavingsPlanAcctContribution_C19") = savingsPlanTable

                'Display calculated Covid taxable and non-taxable amount on UI
                Me.DisplayCovidTaxableAndNonTaxableAmount()
            End If
            'END: MMR | 2020.04.24 | YRS-AT-4854 | Get Blended tax rate and apply tax rate on retirement plan account and savings plan account


            l_Counter = 0

            '****************
            If Me.RefundType.Trim.ToUpper() <> "SPEC" And Me.RefundType.Trim.ToUpper() <> "DISAB" Then
                If CheckboxSavingsVoluntary.Checked = True Then
                    Me.RefundType = "VOL"
                End If
            End If


            '****************
            ' Added by Ganeswar on 17-06-2009 for gemin Issue 787
            For Each l_DataGridItem In Me.DataGridAccContributionSavings.Items
                l_CheckBox = l_DataGridItem.FindControl("Selected")
                If l_bool_SelectAcct = False Then
                    If l_CheckBox.Checked Then
                        l_bool_SelectAcct = True
                    End If
                End If
            Next

            'Added by Ganeswar on 17-06-2009 for gemin Issue 787
            If l_bool_SelectAcct = True Then
                Me.CheckboxSavingsVoluntary.Checked = True
            Else
                Me.CheckboxSavingsVoluntary.Checked = False
            End If


            If Me.SessionStatusType.Trim.ToUpper <> "RT" Then
                If Me.CheckboxSavingsVoluntary.Checked = False Then
                    l_string_MessageForRT = ValidateBalanceInAccountsForVol()
                    If l_string_MessageForRT <> "" Then
                        Me.CheckboxSavingsVoluntary.Checked = True
                    End If
                End If
            End If
            Dim blnRefundType As Boolean = False
            If Me.CheckboxSavingsVoluntary.Checked = False Or Me.CheckboxHardship.Checked = False Then
                If Me.RefundType = "PERS" Then
                    blnRefundType = True
                    Me.RefundType = "VOL"
                End If
                'datagridCheckBox_Savings_CheckedChanged(Me.datagridCheckBox_Savings, e)
                Me.MakeDisplayCalculationDataTables(True, False)
            End If

            If blnRefundType = True Then
                Me.RefundType = "PERS"
            End If

            'Added by Ganeswar on 17-06-2009 for gemin Issue 787
            If Not (Me.RefundType = "VOL" Or Me.RefundType = "SPEC" Or Me.RefundType = "DISAB") Then Return
            'If Me.CheckboxSavingsVoluntary.Checked = False Then Return

            For Each l_DataGridItem In Me.DataGridAccContributionSavings.Items

                l_CheckBox = l_DataGridItem.FindControl("Selected")
                If l_DataGridItem.Cells(2).Text.ToString() = "TM-Matched" Then
                    If l_CheckBox.Checked = True Then
                        l_bool_SelectTMAccount = True
                        'Added by Dilip on 02-12-2009
                        Me.Bool_NotIncludeTMMatched = False
                    ElseIf l_CheckBox.Checked = False Then
                        Me.Bool_NotIncludeTMMatched = True
                        'Added by Dilip on 02-12-2009
                    End If
                End If

                If Not l_CheckBox Is Nothing Then
                    If l_CheckBox.Checked = True Then

                        Me.MakeValuntryCalculationTableForDisplay(l_Counter, True, "IsSavings", l_bool_SelectTMAccount)
                    Else
                        Me.MakeValuntryCalculationTableForDisplay(l_Counter, False, "IsSavings", l_bool_SelectTMAccount)
                    End If

                End If

                l_Counter += 1

            Next

            If Me.CheckboxSavingsVoluntary.Checked = False Or Me.CheckboxHardship.Checked = False Then
                Me.MakeDisplayCalculationDataTables(True, True)
            End If


            'MakeVoluntaryTableAfterRTValidation()
            Me.CalculateTotalForDisplay("IsSavings")
            Me.LoadCalculatedTable("IsSavings")

            If Me.CheckboxRegular.Checked = True Or Me.CheckboxExcludeYMCA.Checked = True Or Me.CheckboxVoluntryAccounts.Checked = True Or _
            (CheckboxPartialRetirement.Checked = True And Not TextboxPartialRetirement.Text = String.Empty) Then
                Me.MakeDisplayCalculationDataTables(True, True)
            ElseIf Me.CheckboxSpecial.Checked = True Then
                Me.MakeDisplayCalculationDataTables(True, True)
            ElseIf Me.CheckboxDisability.Checked = True Then
                Me.MakeDisplayCalculationDataTables(True, True)
            Else
                Me.MakeDisplayCalculationDataTables(False, True)
            End If



            If Me.TotalAmount > 0 Then
                Me.ButtonSave.Visible = True
            Else
                Me.ButtonSave.Visible = False
            End If
            fillNetValuesforAddedControlsSavings(Me.Session("Calculated_DisplaySavingsPlanAcctContribution_C19"))
            PopulateConsolidatedTotals()
            If Me.CheckboxExcludeYMCA.Checked = True Then
                Me.RefundType = "PERS"
            End If
            '------ Added to implement WebServiece by pavan ---- Begin  -----------
            MakeFinalDataTables("IsRetirement")
            MakeFinalDataTables("IsSavings")
            '------ Added to implement WebServiece by pavan ---- END    -----------
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-datagridCheckBox_Savings_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    'Comment code Deleted By Sanjeev on 06/02/2012

    Private Sub CheckboxExcludeYMCA_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxExcludeYMCA.CheckedChanged
        Try

            'Added by Dilip on 02-12-2009
            Me.Bool_NotIncludeAMMatched = False
            'Added by Dilip on 02-12-2009
            CheckboxRetirementPlan.Checked = True
            Me.NumPercentageFactorofMoneyRetirement = 1
            'Added to implement WebServiece by pavan
            If CheckboxExcludeYMCA.Checked = True Then
                SetGlobalFlags("RET")
            End If
            'Added to implement WebServiece by pavan

            ControlAllcheckBoxes("ExcludeYMCA", CheckboxExcludeYMCA.Checked, "R")
            'Comment code Deleted By Sanjeev on 06/02/2012
            DoNotForceSavingPlanWithdrawal(Me.RetirementPlan_TotalAmount) 'SR:2013.12.26-BT 2240:YRS 5.0-2226:modification to partial withdrawal processing

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxExcludeYMCA_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub DatagridGrandTotal_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridGrandTotal.ItemDataBound
        Try
            'Comment code Deleted By Sanjeev on 06/02/2012

            If e.Item.Cells(1).Text.ToUpper = "TOTAL" Or e.Item.Cells(1).Text.ToUpper() = "FIRST DISBURSEMENT" Then
                Dim l_decimal_try As Decimal

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(4).Text)
                e.Item.Cells(4).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(5).Text)
                e.Item.Cells(5).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(6).Text)
                e.Item.Cells(6).Text = FormatCurrency(l_decimal_try)

                l_decimal_try = Convert.ToDecimal(e.Item.Cells(7).Text)
                e.Item.Cells(7).Text = FormatCurrency(l_decimal_try)

            End If

        Catch
            Throw
        End Try
    End Sub

    'Comment code Deleted By Sanjeev on 06/02/2012

    Private Sub CheckboxPartialSavings_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxPartialSavings.CheckedChanged

        Try
            'Added by Dilip on 02-12-2009
            Me.Bool_NotIncludeTMMatched = False
            'Added by Dilip on 02-12-2009
            CheckboxSavingsPlan.Checked = True
            If CheckboxPartialSavings.Checked Then
                TextboxPartialSavings.Visible = True
            End If
            'Added to implement WebServiece by pavan 
            If CheckboxPartialRetirement.Checked Then
                SetGlobalFlags("BOTH")
            ElseIf CheckboxPartialSavings.Checked Then
                SetGlobalFlags("SAV")
            End If
            'Added to implement WebServiece by pavan 
            ControlAllcheckBoxes("PartialSav", CheckboxPartialSavings.Checked, "S")
            If CheckboxPartialSavings.Checked = False Then
                ManageSaveButton(True)
            ElseIf Me.NumRequestedAmountSavings = 0 Then
                ManageSaveButton(False)
            Else
                ManageSaveButton(True)
            End If
            'Comment code Deleted By Sanjeev on 06/02/2012


        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxPartialSavings_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Private Sub CheckboxPartialRetirement_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxPartialRetirement.CheckedChanged
        Try
            'Added by Dilip on 02-12-2009
            Me.Bool_NotIncludeAMMatched = False
            'Added by Dilip on 02-12-2009
            CheckboxRetirementPlan.Checked = True
            If CheckboxPartialRetirement.Checked Then
                TextboxPartialRetirement.Visible = True
                '------ Added to implement WebServiece by pavan ---- Begin  -----------
                'Comment code Deleted By Sanjeev on 06/02/2012
                SetGlobalFlags("RET")
                '------ Added to implement WebServiece ---- END  -----------
            End If
            ControlAllcheckBoxes("PartialRet", CheckboxPartialRetirement.Checked, "R")

            If CheckboxPartialRetirement.Checked = False Then
                ManageSaveButton(True)
            ElseIf Me.NumRequestedAmountRetirement = 0 Then
                ManageSaveButton(False)
            Else
                ManageSaveButton(True)
            End If

            If CheckboxPartialRetirement.Checked = True Then
                If NumRequestedAmountRetirement > 0 Then
                    DoNotForceSavingPlanWithdrawal(Me.NumRequestedAmountRetirement) 'SR:2013.12.26-BT 2240:YRS 5.0-2226:modification to partial withdrawal processing
                End If
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxPartialRetirement_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub TextboxPartialRetirement_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxPartialRetirement.TextChanged
        'Added for validation the partial withdrawal requested amount -Amit 01 Sep 2009
        Dim l_string_dollar As String
        l_string_dollar = "$"

        'Added for validation the partial withdrawal requested amount -Amit 01 Sep 2009
        Try
            If TextboxPartialRetirement.Text = String.Empty Or TextboxPartialRetirement.Text = ".00" Then
                TextboxPartialRetirement.Text = String.Empty
                Me.NumRequestedAmountRetirement = 0.0
                Me.NumPercentageFactorofMoneyRetirementTemp = 1.0
            Else
                If Not IsNumeric(TextboxPartialRetirement.Text) Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Please enter valid amount.", MessageBoxButtons.Stop)
                    TextboxPartialRetirement.Text = String.Empty
                    Me.NumRequestedAmountRetirement = 0.0
                    Me.NumPercentageFactorofMoneyRetirementTemp = 1.0
                ElseIf TextboxPartialRetirement.Text = "." Or TextboxPartialRetirement.Text < 0 Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Please enter valid amount.", MessageBoxButtons.Stop)
                    TextboxPartialRetirement.Text = String.Empty
                    Me.NumRequestedAmountRetirement = 0.0
                    Me.NumPercentageFactorofMoneyRetirementTemp = 1.0
                ElseIf TextboxPartialRetirement.Text = 0.0 Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Partial Amount should be greater than zero.", MessageBoxButtons.Stop)
                    TextboxPartialRetirement.Text = String.Empty
                    Me.NumRequestedAmountRetirement = 0.0
                    Me.NumPercentageFactorofMoneyRetirementTemp = 1.0
                    'Added for validation the partial withdrawal requested amount -Amit 01 Sep 2009
                ElseIf TextboxPartialRetirement.Text < Me.PartialMinLimit Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Partial Request Amount should be greater than or equal to  " & l_string_dollar & "" & Me.PartialMinLimit & ". ", MessageBoxButtons.Stop)
                    TextboxPartialRetirement.Text = String.Empty
                    Me.NumRequestedAmountRetirement = 0.0
                    Me.NumPercentageFactorofMoneyRetirementTemp = 1.0

                    'Added for validation the partial withdrawal requested amount -Amit 01 Sep 2009
                    'New Modification for Market Based Withdrawal Amit Nigam 16/Nov/2009        
                ElseIf TextboxPartialRetirement.Text > Me.MarketBasedThreshold And Me.AllowMarketBased = True Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Partial Request Amount is greater than Market Based Withdrawal Threshold Amount i.e. " & l_string_dollar & "" & Me.MarketBasedThreshold & ". ", MessageBoxButtons.Stop)
                    TextboxPartialRetirement.Text = String.Empty
                    Me.NumRequestedAmountRetirement = 0.0
                    Me.NumPercentageFactorofMoneyRetirementTemp = 1.0
                    'New Modification for Market Based Withdrawal Amit Nigam 16/Nov/2009        
                ElseIf TextboxPartialRetirement.Text > 0 Then
                    Session("RetirementAccountTypeAdjusted_C19") = Nothing
                    Me.NumRequestedAmountRetirement = TextboxPartialRetirement.Text

                    'START: Added By Sanjeev on 31-01-2012 for BT-955
                    'onTextboxPartialRetirement()
                    If (Me.SessionStatusType.Trim.ToUpper = "AE" Or Me.SessionStatusType.Trim.ToUpper = "RA") AndAlso (CheckboxVoluntryAccounts.Enabled = True) Then
                        onTextboxPartialRetirementIsActive()
                    Else
                        onTextboxPartialRetirement()
                    End If
                    'END: Added By Sanjeev on 31-01-2012 for BT-955
                End If
            End If
            ControlAllcheckBoxes("PartialRet", True, "R")
            'Added the condition to manage the save button-Amit 09-09-2009
            If Me.TextboxPartialRetirement.Text = String.Empty Then
                ManageSaveButton(False)
            End If
            'Added the condition to manage the save button-Amit 09-09-2009

            If NumRequestedAmountRetirement > 0 Then
                DoNotForceSavingPlanWithdrawal(Me.NumRequestedAmountRetirement) 'SR:2013.12.26-BT 2240:YRS 5.0-2226:modification to partial withdrawal processing
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-TextboxPartialRetirement_TextChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub TextboxPartialSavings_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxPartialSavings.TextChanged
        'Added for validation the partial withdrawal requested amount -Amit 01 Sep 2009
        Dim l_string_dollar As String
        l_string_dollar = "$"
        'Added for validation the partial withdrawal requested amount -Amit 01 Sep 2009
        Try
            If TextboxPartialSavings.Text = String.Empty Or TextboxPartialSavings.Text = ".00" Then
                TextboxPartialSavings.Text = String.Empty
                Me.NumRequestedAmountSavings = 0.0
                Me.NumPercentageFactorofMoneySavingsTemp = 1.0
            Else
                If Not IsNumeric(TextboxPartialSavings.Text) Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Please enter valid amount.", MessageBoxButtons.Stop)
                    TextboxPartialSavings.Text = String.Empty
                    Me.NumRequestedAmountSavings = 0.0
                    Me.NumPercentageFactorofMoneySavingsTemp = 1.0
                ElseIf TextboxPartialSavings.Text = "." Or TextboxPartialSavings.Text < 0 Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Please enter valid amount.", MessageBoxButtons.Stop)
                    TextboxPartialSavings.Text = String.Empty
                    Me.NumRequestedAmountSavings = 0.0
                    Me.NumPercentageFactorofMoneySavingsTemp = 1.0
                ElseIf TextboxPartialSavings.Text = 0.0 Then
                    Me.NumRequestedAmountSavings = 0.0
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Partial Amount should be greater than zero.", MessageBoxButtons.Stop)
                    TextboxPartialSavings.Text = String.Empty
                    Me.NumRequestedAmountSavings = 0.0
                    Me.NumPercentageFactorofMoneySavingsTemp = 1.0
                    'Added for validation the partial withdrawal requested amount -Amit 01 Sep 2009
                ElseIf TextboxPartialSavings.Text < Me.PartialMinLimit Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Partial Request Amount should be greater than or equal to  " & l_string_dollar & "" & Me.PartialMinLimit & ". ", MessageBoxButtons.Stop)
                    TextboxPartialSavings.Text = String.Empty
                    Me.NumRequestedAmountSavings = 0.0
                    Me.NumPercentageFactorofMoneySavingsTemp = 1.0
                    'Added for validation the partial withdrawal requested amount -Amit 01 Sep 2009
                    'New Modification for Market Based Withdrawal Amit Nigam 16/Nov/2009        
                ElseIf TextboxPartialSavings.Text > Me.MarketBasedThreshold And Me.AllowMarketBased = True Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Partial Request Amount is greater than Market Based Withdrawal Threshold Amount i.e. " & l_string_dollar & "" & Me.MarketBasedThreshold & ". ", MessageBoxButtons.Stop)
                    TextboxPartialSavings.Text = String.Empty
                    Me.NumRequestedAmountSavings = 0.0
                    Me.NumPercentageFactorofMoneySavingsTemp = 1.0
                    'New Modification for Market Based Withdrawal Amit Nigam 16/Nov/2009        
                ElseIf TextboxPartialSavings.Text > 0 Then
                    Session("SavingsAccountTypeAdjusted_C19") = Nothing
                    Me.NumRequestedAmountSavings = TextboxPartialSavings.Text
                    'START: Added By Sanjeev on 31-01-2012 for BT-955
                    'onTextboxPartialSavings()
                    If (Me.SessionStatusType.Trim.ToUpper = "AE" Or Me.SessionStatusType.Trim.ToUpper = "RA") AndAlso CheckboxSavingsVoluntary.Enabled = True Then
                        onTextboxPartialSavingsIsActive()
                    Else
                        onTextboxPartialSavings()
                    End If
                    'END: Added By Sanjeev on 31-01-2012 for BT-955
                End If

            End If
            ControlAllcheckBoxes("PartialSav", True, "S")
            If Me.TextboxPartialSavings.Text = String.Empty Then
                ManageSaveButton(False)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-TextboxPartialSavings_TextChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub CheckboxSavingsVoluntary_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxSavingsVoluntary.CheckedChanged
        Try
            'Added by Dilip on 02-12-2009
            Me.Bool_NotIncludeTMMatched = False
            'Added by Dilip on 02-12-2009
            'Added to implement WebServiece by pavan
            If CheckboxPartialRetirement.Checked Then
                SetGlobalFlags("BOTH")
            ElseIf CheckboxSavingsVoluntary.Checked Then
                SetGlobalFlags("SAV")
            End If
            'Added to implement WebServiece by pavan
            CheckboxSavingsPlan.Checked = True
            Me.NumPercentageFactorofMoneySavings = 1
            ControlAllcheckBoxes("VolSav", CheckboxSavingsVoluntary.Checked, "S")


        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxSavingsVoluntary_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub


#End Region

#Region "Modified - Reg Check Box EVent"

    Private Sub CheckboxRegular_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckboxRegular.CheckedChanged

        Try
            CheckboxRetirementPlan.Checked = True

            Me.NumPercentageFactorofMoneyRetirement = 1
            If CheckboxRegular.Checked And CheckboxPartialSavings.Checked Then
                SetGlobalFlags("BOTH")
            ElseIf CheckboxRegular.Checked Then
                SetGlobalFlags("RET")
            End If

            ControlAllcheckBoxes("Regular", CheckboxRegular.Checked, "R")
            'Comment code Deleted By Sanjeev on 06/02/2012

            DoNotForceSavingPlanWithdrawal(Me.RetirementPlan_TotalAmount) 'SR:2013.12.26-BT 2240:YRS 5.0-2226:modification to partial withdrawal processing

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxRegular_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try



    End Sub

#End Region

#Region "Old Code"
    'Comment code Deleted By Sanjeev on 06/02/2012
#End Region

#Region "Plan Split Changes"
    'Function to validate the creation of refund request if Ymca Accnt at termination is greater than MaxPIaAmt
    Private Function AllowRefundRequest() As Boolean
        Try
            Dim l_flag_AllowRefundRequest As Boolean = True

            'If Me.PlanChosen <> "SAVINGS" Then
            If Me.TerminationPIA >= Me.MaximumPIAAmount And Me.IsVested = True And Me.RetirementPlan_PersonTotalAmountInitial = 0 Then
                l_flag_AllowRefundRequest = False
                Me.CheckboxRetirementPlan.Checked = False
                Me.CheckboxRetirementPlan.Enabled = False
                'Since Retirement Plan is not possible the Savings Plan is the only option left and hence we will not allow the user to deselect it.
                Me.CheckboxSavingsPlan.Enabled = False
                Session("OnlySavingsPlan_C19") = True
                Session("OnlyRetirementPlan_C19") = False
                Session("BothPlans_C19") = False
                Me.AllowedRetirementPlan = False
                Me.GetConsolidatedAvailableContributions()

                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "The YMCA account balance at the time of termination is more than 15000. Manual Process needs to be done for this person.", MessageBoxButtons.Stop)

                'If termination date is 1/1/96 or later, the following message should appear:
                'YMCA account balance at the time of termination was over $15,000.
                'If the termination date is prior to 1/1/1996, the following message should appear:
                'Current YMCA account balance is over $15,000; You must use Special Refund option to process this request

                Dim l_DataTable As New DataTable
                Dim l_DataRow As DataRow
                l_DataTable = Session("PersonInformation_C19").Tables("Member Employment")
                If l_DataTable.Rows.Count > 0 Then
                    Dim l_terminated_date As New Date
                    'by Aparna -YREN-3019
                    For Each l_DataRow In l_DataTable.Rows
                        If Not l_DataRow("TermDate").GetType.ToString = "System.DBNull" Then

                            If DateTime.Compare(l_terminated_date, CType(l_DataRow("TermDate"), DateTime)) < 0 Then
                                l_terminated_date = CType(l_DataRow("TermDate"), DateTime)
                            End If

                        End If

                    Next

                    Dim l_string_dollar As String
                    l_string_dollar = "$"
                    If Me.PlanChosen.ToUpper.Trim = "RETIREMENT" Then
                        If l_terminated_date >= "01/01/1996" Then
                            MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "YMCA account balance at the time of termination was greater than  " & l_string_dollar & " " & Me.MaximumPIAAmount & " ", MessageBoxButtons.Stop)
                        Else
                            'IB:BT687/YRS 5.0-1228 : Remove unnecessary validation
                            ' MessageBox.Show(MessageBoxPlaceHoldear, "YMCA-YRS", "Termination prior to 1/1/1996. YMCA account balance as of 12/31/1995 is greater than " & l_string_dollar & " " & Me.MaximumPIAAmount & "  ; You must use Special Refund option to process this request.", MessageBoxButtons.Stop)
                            MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Termination prior to 1/1/1996. YMCA account balance as of 12/31/1995 is greater than " & l_string_dollar & " " & Me.MaximumPIAAmount & "  ; Please validate actual YMCA Account balance at termination and use Special Withdrawal option to process this request.", MessageBoxButtons.Stop)
                        End If
                    Else
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Can take only the Savings Plan as YMCA account balance at the time of termination was greater than  " & l_string_dollar & " " & Me.MaximumPIAAmount & " ", MessageBoxButtons.Stop)
                    End If


                End If
                If Me.SavingsPlan_PersonTotalAmountInitial = 0 Then
                    'if Retirement Plan is not allowed and there is no money in Savings Plan as well then none of the refund type is allowed.
                    Me.TotalAmount = 0
                    Me.CheckboxRegular.Enabled = False
                    Me.CheckboxSpecial.Enabled = False
                    Me.CheckboxVoluntryAccounts.Enabled = False
                    Me.CheckboxHardship.Enabled = False
                    CheckboxExcludeYMCA.Enabled = False
                ElseIf Me.SavingsPlan_PersonTotalAmountInitial > 0 Then
                    'if Savings Plan has money then we need to do nothing and retain the previous conditions.
                End If
            End If

            'End If

            Return l_flag_AllowRefundRequest
        Catch
            Throw
        End Try
    End Function
    'Full refund will not be allowed when Termination PIA > = Max PIA Amt

    Private Sub AllowFullRefund()
        Try
            If Me.TerminationPIA >= Me.MaximumPIAAmount And Me.IsVested = True And Me.IsTerminated = True Then
                Me.RefundType = "PERS"

                FullRefundForDisplay("BothPlans")
                If Me.RetirementPlan_TotalAmount > 0 Then
                    Me.CheckboxRegular.Enabled = True
                    Me.CheckboxRegular.Checked = False
                Else
                    Me.CheckboxRegular.Enabled = False
                    Me.CheckboxRegular.Checked = False
                End If

            ElseIf Me.IsTerminated Then
                FullRefundForDisplay("BothPlans")
                If Me.RetirementPlan_TotalAmount > 0 Then
                    Me.CheckboxRegular.Enabled = True
                    If Me.CheckboxExcludeYMCA.Checked = False Then
                        Me.CheckboxRegular.Checked = True
                    Else
                        Me.CheckboxRegular.Checked = False
                    End If

                Else
                    Me.CheckboxRegular.Enabled = False
                    Me.CheckboxRegular.Checked = False
                End If

            End If
            If Me.CheckboxRegular.Enabled = False And Me.CheckboxExcludeYMCA.Enabled = False And Me.CheckboxVoluntryAccounts.Enabled = False Then
                Me.CheckboxRetirementPlan.Checked = False
                Me.CheckboxRetirementPlan.Enabled = False
            End If
            If NoRefundAllowed = True Then
                Me.CheckboxRetirementPlan.Checked = False
                Me.CheckboxRetirementPlan.Enabled = False
                Me.CheckboxRegular.Enabled = False
                Me.CheckboxRegular.Checked = False
            End If
        Catch
            Throw New ArgumentException("Exception Occured")
        End Try
    End Sub

    Private Sub CheckboxRetirementPlan_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxRetirementPlan.CheckedChanged
        Dim l_ContributionDataTable As DataTable
        Try

            If Me.CheckboxDisability.Checked = True Or CheckboxExcludeYMCA.Checked = True Or Me.CheckboxRegular.Checked = True Or Me.CheckboxSpecial.Checked = True Or Me.CheckboxVoluntryAccounts.Checked = True _
                Or Me.CheckboxRegular.Enabled = True Or Me.CheckboxExcludeYMCA.Enabled = True Or Me.CheckboxVoluntryAccounts.Enabled = True Then
                If CheckboxRetirementPlan.Checked = False Then
                    If Me.CheckboxSavingsPlan.Checked = True And Me.CheckboxSavingsVoluntary.Checked = True Then
                        Me.PlanChosen = "SAVINGS"
                        Me.RefundType = "VOL"
                        Me.VoluntryRefundForDisplay("SavingsPlan")
                        Me.MakeDisplayCalculationDataTables(False, True)
                        Me.CheckboxRegular.Checked = False
                        Me.CheckboxExcludeYMCA.Checked = False
                        If Me.CheckboxVoluntryAccounts.Checked = True Then
                            Me.RefundType = ""
                            Me.CheckboxVoluntryAccounts.Checked = False
                            Me.VoluntryRefundForDisplay("RetirementPlan")
                        Else
                            Me.CheckboxVoluntryAccounts.Checked = False
                        End If

                        Me.CheckboxSavingsVoluntary.Enabled = False
                    Else
                        Me.CheckboxRetirementPlan.Checked = True
                        If Me.CheckboxRegular.Enabled = True And Me.CheckboxExcludeYMCA.Checked = False And Me.CheckboxVoluntryAccounts.Checked = False Then
                            Me.CheckboxRegular.Checked = True
                        End If
                        If Me.CheckboxExcludeYMCA.Enabled = True And Me.CheckboxRegular.Checked = False And Me.CheckboxVoluntryAccounts.Checked = False Then
                            Me.CheckboxExcludeYMCA.Checked = True
                        End If
                        If Me.CheckboxVoluntryAccounts.Enabled = True And Me.CheckboxRegular.Checked = False And Me.CheckboxExcludeYMCA.Checked = False Then
                            Me.CheckboxVoluntryAccounts.Checked = True
                        End If
                        If Me.CheckboxRegular.Checked = True Or Me.CheckboxExcludeYMCA.Checked = True Then
                            Me.FullRefundForDisplay("RetirementPlan")
                        ElseIf Me.CheckboxVoluntryAccounts.Checked = True Then
                            Me.RefundType = "VOL"
                            Me.VoluntryRefundForDisplay("RetirementPlan")
                        End If
                        Me.MakeDisplayCalculationDataTables(True, False)
                        Me.PlanChosen = "RETIREMENT"
                    End If
                    Me.CheckboxSavingsPlan.Enabled = False
                End If
                If Me.CheckboxRetirementPlan.Checked = True Then
                    If Me.SavingsPlan_TotalAmount > 0 Then
                        Me.CheckboxSavingsPlan.Enabled = True
                    End If
                    If Me.CheckboxRegular.Enabled = True And Me.CheckboxExcludeYMCA.Checked = False And Me.CheckboxVoluntryAccounts.Checked = False Then
                        Me.CheckboxRegular.Checked = True
                    End If
                    If Me.CheckboxExcludeYMCA.Enabled = True And Me.CheckboxRegular.Checked = False And Me.CheckboxVoluntryAccounts.Checked = False Then
                        Me.CheckboxExcludeYMCA.Checked = True
                    End If
                    If Me.CheckboxVoluntryAccounts.Enabled = True And Me.CheckboxRegular.Checked = False And Me.CheckboxExcludeYMCA.Checked = False Then
                        Me.CheckboxVoluntryAccounts.Checked = True
                    End If
                    If Me.CheckboxRegular.Checked = True Or Me.CheckboxExcludeYMCA.Checked = True Then
                        Me.FullRefundForDisplay("RetirementPlan")
                    ElseIf Me.CheckboxVoluntryAccounts.Checked = True Then
                        Me.RefundType = "VOL"
                        Me.VoluntryRefundForDisplay("RetirementPlan")
                    End If
                    If Me.CheckboxSavingsPlan.Checked = True Then
                        If Me.CheckboxSavingsVoluntary.Checked = True Then
                            Me.RefundType = "VOL"
                            Me.VoluntryRefundForDisplay("SavingsPlan")
                            Me.MakeDisplayCalculationDataTables(True, True)
                        Else
                            Me.MakeDisplayCalculationDataTables(True, False)
                        End If
                    End If
                    If Me.CheckboxSavingsPlan.Checked = False Then
                        Me.PlanChosen = "RETIREMENT"
                        Me.MakeDisplayCalculationDataTables(True, False)
                    End If
                    If Me.CheckboxRetirementPlan.Checked = True Then
                        If Me.CheckboxSavingsPlan.Checked = True Then
                            Me.CheckboxSavingsPlan.Enabled = True
                        End If
                        If Me.CheckboxSavingsVoluntary.Checked = True Then
                            Me.CheckboxSavingsVoluntary.Enabled = True
                        End If
                        If Me.CheckboxSavingsVoluntary.Checked = False And Me.CheckboxHardship.Checked = False Then
                            Me.CheckboxSavingsPlan.Enabled = False
                        End If
                    End If
                End If
                If Me.CheckboxSavingsPlan.Checked = True Or Me.CheckboxRetirementPlan.Checked = True Then
                    If Me.TotalAmount > 0 Then
                        Me.ButtonSave.Visible = True
                    Else
                        Me.ButtonSave.Visible = False
                    End If
                Else
                    Me.ButtonSave.Visible = False
                End If
            Else
                If Me.CheckboxSavingsPlan.Checked = False Then
                    Me.CheckboxRetirementPlan.Checked = True
                End If
            End If
            '------ Added to implement WebServiece by pavan ---- Begin  -----------
            MakeFinalDataTables("IsRetirement")
            MakeFinalDataTables("IsSavings")
            '------ Added to implement WebServiece by pavan ---- END    -----------



        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxRetirementPlan_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Function GetConsolidatedAvailableContributions()
        Try
            Dim l_DataTable_SavingsPlanAcctContribution As DataTable
            Dim l_DataTable_RetirementPlanAcctContribution As DataTable
            Dim l_DataTable_ConsolidatedAcctContribution As DataTable
            If Session("OnlySavingsPlan_C19") = True Then
                If Not Session("SavingsPlanAcctContribution_C19") Is Nothing Then
                    l_DataTable_SavingsPlanAcctContribution = DirectCast(Session("SavingsPlanAcctContribution_C19"), DataTable)
                    Session("AccountContribution_C19") = l_DataTable_SavingsPlanAcctContribution
                    Me.CopyAccountContributionTable()
                End If
            ElseIf Session("OnlyRetirementPlan_C19") = True Then
                If Not Session("RetirementPlanAcctContribution_C19") Is Nothing Then
                    l_DataTable_RetirementPlanAcctContribution = DirectCast(Session("RetirementPlanAcctContribution_C19"), DataTable)
                    Session("AccountContribution_C19") = l_DataTable_RetirementPlanAcctContribution
                    Me.CopyAccountContributionTable()
                End If
            ElseIf Session("BothPlans_C19") = True Then
                If Not Session("InitialAccountContribution_C19") Is Nothing Then
                    l_DataTable_ConsolidatedAcctContribution = DirectCast(Session("InitialAccountContribution_C19"), DataTable)
                    Session("AccountContribution_C19") = l_DataTable_ConsolidatedAcctContribution
                    Me.CopyAccountContributionTable()
                End If
            End If

            If Me.CheckboxRegular.Checked = True Then
                Me.FullRefund("BothPlans")
            ElseIf Me.CheckboxVoluntryAccounts.Checked = True Then
                Me.VoluntryRefund("BothPlans")
            ElseIf Me.CheckboxHardship.Checked = True Then
                Me.HardshipRefund("BothPlans")
            ElseIf CheckboxExcludeYMCA.Checked = True Then
                Me.PersonalRefund("BothPlans")
            ElseIf Me.CheckboxSpecial.Checked = True Then
                Me.SpecialRefund("BothPlans")
            ElseIf Me.CheckboxDisability.Checked = True Then
                Me.DisabilityRefund("BothPlans")
            Else
                Me.FullRefund("BothPlans")


            End If

        Catch
            Throw
        End Try
    End Function

    Private Function GetConsolidatedAvailableContributionsForDisplay()
        Try
            Dim l_DataTable_SavingsPlanAcctContribution As DataTable
            Dim l_DataTable_RetirementPlanAcctContribution As DataTable
            Dim l_DataTable_ConsolidatedAcctContribution As DataTable
            If Session("OnlySavingsPlan_C19") = True Then
                If Not Session("DisplaySavingsPlanAcctContribution_C19") Is Nothing Then
                    l_DataTable_SavingsPlanAcctContribution = DirectCast(Session("DisplaySavingsPlanAcctContribution_C19"), DataTable)
                    Session("AccountContributionForDisplay_C19") = l_DataTable_SavingsPlanAcctContribution
                    'Me.DisplayCopyAccountContributionTable()
                    Me.DisplayCopyAccountContributionTable("Savings")
                End If
            ElseIf Session("OnlyRetirementPlan_C19") = True Then
                If Not Session("DisplayRetirementPlanAcctContribution_C19") Is Nothing Then
                    l_DataTable_RetirementPlanAcctContribution = DirectCast(Session("DisplayRetirementPlanAcctContribution_C19"), DataTable)
                    Session("AccountContributionForDisplay_C19") = l_DataTable_RetirementPlanAcctContribution
                    'Me.DisplayCopyAccountContributionTable()
                    Me.DisplayCopyAccountContributionTable("Retirement")
                End If
            ElseIf Session("BothPlans_C19") = True Then
                If Not Session("InitialAccountContribution_C19") Is Nothing Then
                    l_DataTable_ConsolidatedAcctContribution = DirectCast(Session("InitialAccountContribution_C19"), DataTable)
                    Session("AccountContributionForDisplay_C19") = l_DataTable_ConsolidatedAcctContribution
                    'Me.DisplayCopyAccountContributionTable()
                    Me.DisplayCopyAccountContributionTable("Retirement")
                    Me.DisplayCopyAccountContributionTable("Savings")
                End If
            End If

            If Me.CheckboxRegular.Checked = True Then
                Me.FullRefundForDisplay("BothPlans")
            ElseIf Me.CheckboxVoluntryAccounts.Checked = True Then
                Me.VoluntryRefundForDisplay("BothPlans")
            ElseIf Me.CheckboxHardship.Checked = True Then
                Me.HardshipRefund("BothPlans")
            ElseIf CheckboxExcludeYMCA.Checked = True Then
                Me.PersonalRefund("BothPlans")
            ElseIf Me.CheckboxSpecial.Checked = True Then
                Me.SpecialRefund("BothPlans")
            ElseIf Me.CheckboxDisability.Checked = True Then
                Me.DisabilityRefund("BothPlans")
            Else
                Me.FullRefundForDisplay("BothPlans")


            End If

        Catch
            Throw
        End Try
    End Function

    Private Sub CheckboxSavingsPlan_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxSavingsPlan.CheckedChanged
        Dim l_ContributionDataTable As DataTable
        Try
            'If Me.SavingsPlan_TotalAmount > 0 Then
            If Me.CheckboxSavingsVoluntary.Enabled = True Or Me.CheckboxSavingsVoluntary.Checked = True Or Me.CheckboxHardship.Checked = True Or Me.CheckboxHardship.Enabled = True Then
                If Me.CheckboxSavingsPlan.Checked = False Then
                    If Me.CheckboxRetirementPlan.Checked = True Then
                        Me.PlanChosen = "RETIREMENT"
                        If Me.CheckboxRegular.Checked = True Or Me.CheckboxExcludeYMCA.Checked = True Then
                            'Me.CheckboxRegular.Enabled = False
                            'Me.CheckboxExcludeYMCA.Enabled = False
                            Me.FullRefundForDisplay("RetirementPlan")
                            Me.MakeDisplayCalculationDataTables(True, False)
                        ElseIf Me.CheckboxVoluntryAccounts.Checked = True Then
                            Me.RefundType = "VOL"
                            Me.VoluntryRefundForDisplay("RetirementPlan")
                            Me.MakeDisplayCalculationDataTables(True, False)
                        Else
                            Me.MakeDisplayCalculationDataTables(False, False)
                        End If
                        If Me.CheckboxSavingsVoluntary.Checked = True Then
                            Me.CheckboxSavingsVoluntary.Checked = False
                            Me.RefundType = ""
                            Me.VoluntryRefundForDisplay("SavingsPlan")
                        Else
                            Me.CheckboxSavingsVoluntary.Checked = False
                        End If

                        Me.CheckboxHardship.Checked = False
                    Else
                        Me.CheckboxSavingsPlan.Checked = True
                        Me.RefundType = "VOL"
                        Me.VoluntryRefundForDisplay("SavingsPlan")
                        Me.PlanChosen = "SAVINGS"
                        Me.MakeDisplayCalculationDataTables(False, True)
                    End If
                    Me.CheckboxRetirementPlan.Enabled = False

                End If
                If Me.CheckboxSavingsPlan.Checked = True Then
                    If Me.RetirementPlan_TotalAmount > 0 Then
                        If Me.CheckboxRegular.Enabled = True Or Me.CheckboxExcludeYMCA.Enabled = True Or Me.CheckboxVoluntryAccounts.Enabled = True Then
                            Me.CheckboxRetirementPlan.Enabled = True
                        End If

                    End If
                    If Me.CheckboxSavingsVoluntary.Enabled = True And Me.CheckboxHardship.Checked = False Then
                        Me.CheckboxSavingsVoluntary.Checked = True
                    End If
                    ''If Me.CheckboxHardship.Enabled = True And Me.CheckboxSavingsVoluntary.Checked = False Then
                    ''    Me.CheckboxHardship.Checked = True
                    ''End If
                    Me.RefundType = "VOL"
                    Me.VoluntryRefundForDisplay("SavingsPlan")

                    If Me.CheckboxRetirementPlan.Checked = True Then
                        If Me.CheckboxRegular.Checked = True Or Me.CheckboxExcludeYMCA.Checked = True Then
                            Me.FullRefundForDisplay("RetirementPlan")
                            Me.MakeDisplayCalculationDataTables(True, True)
                        ElseIf Me.CheckboxVoluntryAccounts.Checked = True Then
                            Me.RefundType = "VOL"
                            Me.VoluntryRefundForDisplay("RetirementPlan")
                            Me.MakeDisplayCalculationDataTables(True, True)
                        Else
                            Me.MakeDisplayCalculationDataTables(False, True)
                        End If
                        Me.PlanChosen = "RETIREMENT&SAVINGS"
                    ElseIf Me.CheckboxRetirementPlan.Checked = False Then
                        Me.RefundType = "VOL"
                        Me.VoluntryRefundForDisplay("SavingsPlan")
                        Me.MakeDisplayCalculationDataTables(False, True)
                        Me.PlanChosen = "SAVINGS"
                    End If
                    If Me.CheckboxRetirementPlan.Checked = True Then
                        Me.CheckboxRetirementPlan.Enabled = True
                    End If
                    If Me.CheckboxRegular.Checked = True Then
                        Me.CheckboxRegular.Enabled = True
                    End If
                End If
                If Me.CheckboxSavingsPlan.Checked = True Or Me.CheckboxRetirementPlan.Checked = True Then
                    If Me.TotalAmount > 0 Then
                        Me.ButtonSave.Visible = True
                    Else
                        Me.ButtonSave.Visible = False
                    End If
                Else
                    Me.ButtonSave.Visible = False
                End If



            Else
                If Me.CheckboxRetirementPlan.Checked = False Then
                    Me.CheckboxSavingsPlan.Checked = True
                End If

            End If

            '------ Added to implement WebServiece by pavan ---- Begin  -----------
            MakeFinalDataTables("IsRetirement")
            MakeFinalDataTables("IsSavings")
            '------ Added to implement WebServiece by pavan ---- END    -----------

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxSavingsPlan_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Function SetPlanCheckBoxes()
        Try

            If Me.RetirementPlan_TotalAvailableAmount <= 0 Then
                Me.CheckboxRetirementPlan.Checked = False
                Me.CheckboxRetirementPlan.Enabled = False
            Else
                'Comment code Deleted By Sanjeev on 06/02/2012

            End If
            If Me.SavingsPlan_TotalAvailableAmount <= 0 Then
                Me.CheckboxSavingsPlan.Checked = False
                Me.CheckboxSavingsPlan.Enabled = False
                'Else
                '   Me.CheckboxSavingsPlan.Enabled = True
            End If
            If Me.RefundType = "HARD" Then
                Me.CheckboxSavingsPlan.Enabled = False
                Me.CheckboxRetirementPlan.Enabled = False
            End If
        Catch
            Throw
        End Try
    End Function

    Private Function AllowPersonalSideRefund() As Boolean
        Dim boolContinue As Boolean
        Dim bool_personalMoney As Boolean
        Dim bool_ymcaMoney As Boolean
        Dim bool_ymcaLegacyMoney As Boolean

        Try
            bool_ymcaLegacyMoney = False
            bool_ymcaMoney = False
            bool_personalMoney = False
            boolContinue = False

            If Me.RetirementPlan_PersonTotalAmountInitial > 0.0 Then
                bool_personalMoney = True
            End If

            'START | SR | 2016.07.19 | YRS-AT-3015 Added below condition to check whether the BAsic accounts are available or not based on new combined limit rule
            If (IsBALegacyCombinedAmountSwitchedON) Then

                If ((Me.BACurrent + Me.CurrentPIA) > Me.MaxCombinedBasicAccountAmt) Then

                    If (Me.CurrentPIA > 0 AndAlso Me.TerminationPIA <= Me.MaxYMCALegacyAcctAmt) Then
                        bool_ymcaLegacyMoney = True
                    End If

                    If (Me.BACurrent > 0 AndAlso Me.BACurrent <= Me.MaxYMCAAcctAmt) Then
                        bool_ymcaMoney = True
                    End If
                Else
                    If (Me.CurrentPIA > 0) Then
                        bool_ymcaLegacyMoney = True
                    End If

                    If (Me.BACurrent > 0) Then
                        bool_ymcaMoney = True
                    End If
                End If
            Else
                'END | SR | 2016.07.19 | YRS-AT-3015 Added below condition to check whether the BAsic accounts are available or not based on new combined limit rule
                'modified the if condition to include the CurrentPIA in place of TerminationPIA 08/07/2009
                'If Me.TerminationPIA > 0 And Me.TerminationPIA > Me.MaximumPIAAmount Then
                If Me.CurrentPIA > 0 And Me.TerminationPIA > Me.MaximumPIAAmount Then
                    bool_ymcaLegacyMoney = False
                    ' ElseIf Me.TerminationPIA > 0 Then
                ElseIf Me.CurrentPIA > 0 Then
                    bool_ymcaLegacyMoney = True
                End If

                If Me.BACurrent > Me.BAMaxLimit And Me.BACurrent > 0 Then
                    bool_ymcaMoney = False
                ElseIf Me.BACurrent > 0 Then
                    bool_ymcaMoney = True
                End If
            End If

            If bool_personalMoney = True And (bool_ymcaLegacyMoney = True Or bool_ymcaMoney = True) Then

                boolContinue = True

                'Comment code Deleted By Sanjeev on 06/02/2012
                '------ Added to implement WebServiece by pavan ---- Begin  -----------
                If Me.CurrentPIA + Me.BACurrent <= Me.MinimumPIAToRetire And AnnuityExists_Retirement = False Then
                    boolContinue = False
                End If
                '------ Added to implement WebServiece by pavan ---- END    -----------

            ElseIf bool_ymcaLegacyMoney = True And bool_ymcaMoney = True Then
                boolContinue = True
                'Comment code Deleted By Sanjeev on 06/02/2012
                '------ Added to implement WebServiece by pavan ---- Begin  -----------
                If Me.CurrentPIA + Me.BACurrent <= Me.MinimumPIAToRetire And AnnuityExists_Retirement = False Then
                    boolContinue = False
                End If
                '------ Added to implement WebServiece by pavan ---- END    -----------
            Else
                boolContinue = False
            End If

            AllowPersonalSideRefund = boolContinue

        Catch
            Throw
        End Try
    End Function
#End Region
    'Comment code Deleted By Sanjeev on 06/02/2012

    Private Function DisplayMarketInstallments()
        Try
            Dim l_decimal_firstinstallmentforRetirementPlan As Decimal = 0
            Dim l_decimal_firstinstallmentforSavingsPlan As Decimal = 0
            Dim l_decimal_nontaxret As Decimal = 0
            Dim l_decimal_taxret As Decimal = 0
            Dim l_decimal_interest As Decimal = 0
            Dim l_decimal_taxable As Decimal = 0
            Dim l_decimal_taxwithheldret As Decimal = 0
            Dim l_decimal_netamountret As Decimal = 0
            Dim l_decimal_nontaxsav As Decimal = 0
            Dim l_decimal_taxsav As Decimal = 0
            Dim l_decimal_interestsav As Decimal = 0
            Dim l_decimal_taxablesav As Decimal = 0
            Dim l_decimal_taxwithheldsav As Decimal = 0
            Dim l_decimal_netamountsav As Decimal = 0
            Dim l_decimal_marketPercentageFactor As Decimal = 0

            If CheckboxRegular.Checked = True Or CheckboxExcludeYMCA.Checked = True Or CheckboxVoluntryAccounts.Checked = True Then
                If AllowMarketBasedForRetirementPlan = True Then
                    DisplayLabelandTextBoxes()
                    l_decimal_marketPercentageFactor = Me.MarketBasedFirstInstPercentage / 100
                    If DataGridAccContributionRetirement.Items.Count > 0 Then
                        For intcount As Integer = 0 To DataGridAccContributionRetirement.Items.Count - 1
                            If DataGridAccContributionRetirement.Items.Item(intcount).Cells(2).Text.ToString() = "Total" Then
                                If DataGridAccContributionRetirement.Items.Item(intcount).Cells(8).Text.ToString() > Me.MarketBasedThreshold Then
                                    l_decimal_firstinstallmentforRetirementPlan = DataGridAccContributionRetirement.Items.Item(intcount).Cells(8).Text.ToString()
                                    l_decimal_nontaxret = TextboxRetirementNonTaxable.Text
                                    l_decimal_taxable = TextboxRetirementTaxable.Text
                                    l_decimal_taxwithheldret = TextboxRetirementTaxWithheld.Text
                                    l_decimal_netamountret = TextboxRetirementNetAmount.Text
                                Else
                                    l_decimal_firstinstallmentforRetirementPlan = 0
                                    l_decimal_nontaxret = 0
                                    l_decimal_taxable = 0
                                    l_decimal_taxwithheldret = 0
                                    l_decimal_netamountret = 0
                                End If

                            End If
                        Next
                    End If

                End If
            End If

            'Savings Plan
            If CheckboxSavingsVoluntary.Checked = True Then
                If AllowMarketBasedForSavingsPlan = True Then
                    DisplayLabelandTextBoxes()
                    l_decimal_marketPercentageFactor = Me.MarketBasedFirstInstPercentage / 100
                    If DataGridAccContributionSavings.Items.Count > 0 Then
                        For intcount As Integer = 0 To DataGridAccContributionSavings.Items.Count - 1
                            If DataGridAccContributionSavings.Items.Item(intcount).Cells(2).Text.ToString() = "Total" Then
                                If DataGridAccContributionSavings.Items.Item(intcount).Cells(8).Text.ToString() > Me.MarketBasedThreshold Then
                                    l_decimal_firstinstallmentforSavingsPlan = DataGridAccContributionSavings.Items.Item(intcount).Cells(8).Text.ToString()
                                    l_decimal_nontaxsav = TextboxSavingsNonTaxable.Text
                                    l_decimal_taxablesav = TextboxSavingsTaxable.Text
                                    l_decimal_taxwithheldsav = TextboxSavingsTaxWithheld.Text
                                    l_decimal_netamountsav = TextboxSavingsNetAmount.Text
                                Else
                                    l_decimal_firstinstallmentforSavingsPlan = 0
                                    l_decimal_nontaxsav = 0
                                    l_decimal_taxablesav = 0
                                    l_decimal_taxwithheldsav = 0
                                    l_decimal_netamountsav = 0
                                End If
                            End If
                        Next
                    End If
                End If
            End If
            If AllowMarketBasedForRetirementPlan = True Or AllowMarketBasedForSavingsPlan = True Then
                TextboxFirstInstallment.Text = Convert.ToDecimal(Math.Round(((l_decimal_firstinstallmentforRetirementPlan + l_decimal_firstinstallmentforSavingsPlan) * l_decimal_marketPercentageFactor), 2)).ToString()
                TextboxPercentage.Text = Me.MarketBasedFirstInstPercentage
                TextboxNonTaxableMarket.Text = Convert.ToDecimal(Math.Round((l_decimal_nontaxret + l_decimal_nontaxsav) * l_decimal_marketPercentageFactor, 2))
                TextboxTaxableMarket.Text = Convert.ToDecimal(Math.Round((l_decimal_taxable + l_decimal_taxablesav) * l_decimal_marketPercentageFactor, 2))
                TextboxTaxWithHeldMarket.Text = Convert.ToDecimal(Math.Round((l_decimal_taxwithheldret + l_decimal_taxwithheldsav) * l_decimal_marketPercentageFactor, 2))
                TextboxNetAmountMarket.Text = Convert.ToDecimal(Math.Round((l_decimal_netamountret + l_decimal_netamountsav) * l_decimal_marketPercentageFactor, 2))
            End If

        Catch
            Throw
        End Try
    End Function


    Private Function DisplayLabelandTextBoxes()
        Try
            LabelFirstInstallment.Visible = True
            LabelPercentage.Visible = True
            TextboxFirstInstallment.Visible = True
            TextboxPercentage.Visible = True
            LabelNonTaxableMarket.Visible = True
            LabelTaxableMarket.Visible = True
            LabelTaxWithHeldMarket.Visible = True
            LabelNetAmount.Visible = True
            TextboxNonTaxableMarket.Visible = True
            TextboxTaxableMarket.Visible = True
            TextboxTaxWithHeldMarket.Visible = True
            TextboxNetAmountMarket.Visible = True
        Catch
            Throw
        End Try
    End Function
    'New Modification for Market Based Withdrawal Amit Nigam

    'Added By Parveen To Hide Market Based Records
    Private Function RemoveMarketBasedRecords(ByVal parameterCalledBy As String, ByVal parameterDatatable As DataTable)
        Dim l_RetDataTable As DataTable
        Dim l_SavingsDataTable As DataTable
        Dim drow As DataRow()
        Dim cnt As Integer
        Try
            If parameterCalledBy = "IsRetirement" Then
                If (CheckboxRegular.Checked = False And CheckboxExcludeYMCA.Checked = False And CheckboxVoluntryAccounts.Checked = False) _
             Or (CheckboxPartialRetirement.Checked = True And TextboxPartialRetirement.Text.Trim.Length = 0) Then
                    l_RetDataTable = parameterDatatable
                    If Not l_RetDataTable Is Nothing Then
                        For cnt = 0 To l_RetDataTable.Rows.Count - 1
                            If l_RetDataTable.Rows(cnt)("AccountType").ToString = "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)" Then
                                l_RetDataTable.Rows.RemoveAt(cnt)
                            End If
                        Next
                    End If
                End If
            ElseIf parameterCalledBy = "IsSavings" Then
                If (CheckboxSavingsVoluntary.Checked = False And CheckboxHardship.Checked = False) _
             Or (CheckboxPartialSavings.Checked = True And TextboxPartialSavings.Text.Trim.Length = 0) Then
                    l_SavingsDataTable = parameterDatatable
                    If Not l_SavingsDataTable Is Nothing Then
                        For cnt = 0 To l_SavingsDataTable.Rows.Count - 1
                            If l_SavingsDataTable.Rows(cnt)("AccountType").ToString = "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)" Then
                                l_SavingsDataTable.Rows.RemoveAt(cnt)
                            End If
                        Next
                    End If
                End If
            End If
        Catch
            Throw
        End Try

    End Function
    'Added By Parveen To Hide Market Based Records

#Region "Implementing WebServiece"

    '------ Added to implement WebServiece by pavan ---- Begin  -----------
    Private Function LoadInformationToControls()

        Try
            If (FinalDataSet.Tables("WithdrawalFlags").Rows.Count > 0) Then

                'LoadInformation
                Me.LabelSSNo.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SSNo"))
                Me.LabelParticipantName.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("LabelParticipantName"))
                If (Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("IsVested"))) Then
                    Me.TextboxVested.Text = "Yes"
                Else
                    Me.TextboxVested.Text = "No"
                End If
                If (Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("IsTerminated"))) Then
                    Me.TextBoxTerminated.Text = "Yes"
                Else
                    Me.TextBoxTerminated.Text = "No"
                End If
                'Me.TextboxAge.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("PersonAge"))


                'IB:As on 11/June/2010 for Gemini:YRS 5.0-1108 -Display age in year with month format e.g.42Y/11M
                If (Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("PersonAge")).IndexOf(".") > -1) Then
                    'IB:As on 14/June/2010 for  BT:540 convert decimal to string with "00.00" format
                    Me.TextboxAge.Text = CType(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("PersonAge"), Decimal).ToString("00.00")
                    Me.TextboxAge.Text = Me.TextboxAge.Text.Replace(".", "Y/") + "M"
                Else
                    Me.TextboxAge.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("PersonAge")) + "Y"
                End If

                Me.LabelFundNo.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("LabelFundNo"))

                'LoadPIAAmount
                'modified for the masking issue in the textbox-Amit Nigam


                'TextboxBATerminate.Text = FormatCurrency(Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("BACurrentAtRequest")))
                TextboxBATerminate.Text = FormatCurrency(Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("BACurrentAtRequest")))
                'modified for the masking issue in the textbox-Amit Nigam

                TextboxTerminatePIA.Text = FormatCurrency(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TerminationPIA"))

                CheckboxRetirementPlan.Checked = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CheckboxRetirementPlan"))
                CheckboxRetirementPlan.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CheckboxRetirementPlanEnabled"))
                CheckboxSavingsPlan.Checked = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CheckboxSavingsPlan"))
                CheckboxSavingsPlan.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CheckboxSavingsPlanEnabled"))


                'ControlAllWithdrawalTypes

                Me.ButtonSave.Visible = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("ButtonSaveVisible"))
                Me.ButtonSave.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("ButtonSaveEnabled"))

                'Checkbox Enabled
                Me.CheckboxRegular.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementPlanFullCheckboxEnable"))
                Me.CheckboxExcludeYMCA.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementPlanExcludeYMCACheckboxEnable"))
                Me.CheckboxPartialRetirement.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementPlanPartialCheckboxEnable"))
                Me.CheckboxVoluntryAccounts.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementPlanVoluntaryCheckboxEnable"))
                Me.CheckboxPartialSavings.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SavingPlanPartialCheckboxEnable"))
                Me.CheckboxSavingsVoluntary.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SavingPlanVoluntaryCheckboxEnable"))
                Me.CheckboxHardship.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SavingPlanHardshipCheckboxEnable"))
                Me.CheckboxSpecial.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SpecialCheckboxEnable"))
                Me.CheckboxDisability.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("DisabilityCheckboxEnable"))
                Me.CheckboxSavingsPlan.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CheckboxSavingsPlanEnabled"))

                'Checkbox Checked
                Me.CheckboxRegular.Checked = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementPlanFullCheckbox"))
                Me.CheckboxExcludeYMCA.Checked = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementPlanExcludeYMCACheckbox"))
                Me.CheckboxPartialRetirement.Checked = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementPlanPartialCheckbox"))
                Me.CheckboxVoluntryAccounts.Checked = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementPlanVoluntaryCheckbox"))
                Me.CheckboxPartialSavings.Checked = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SavingPlanPartialCheckbox"))
                Me.CheckboxSavingsVoluntary.Checked = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SavingPlanVoluntaryCheckbox"))
                Me.CheckboxHardship.Checked = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SavingPlanHardshipCheckbox"))
                Me.CheckboxSpecial.Checked = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SpecialCheckbox"))
                Me.CheckboxDisability.Checked = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("DisabilityCheckbox"))
                Me.CheckboxSavingsPlan.Checked = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CheckboxSavingsPlan"))

                'Checkbox Visible
                'Me.CheckboxRegular.Visible = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CheckboxRegularVisible"))
                Me.CheckboxSpecial.Visible = False 'Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CheckboxSpecialVisible"))
                Me.CheckboxDisability.Visible = False 'Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CheckboxDisabilityVisible"))
                'Comment code Deleted By Sanjeev on 06/02/2012
                Me.CompulsorySavings = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CompulsorySavings"))
                'IB:08-Dec-2010 for BT:607 Special/Disability withdrawals should not apply withdrawal rules
                If Me.RefundType = "SPEC" Then
                    Me.CheckboxSpecial.Visible = True 'Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CheckboxSpecialVisible"))
                    Me.CheckboxDisability.Visible = False 'Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CheckboxDisabilityVisible"))
                    Me.CheckboxRegular.Visible = False
                End If
                If Me.RefundType = "DISAB" Then
                    Me.CheckboxSpecial.Visible = False 'Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CheckboxSpecialVisible"))
                    Me.CheckboxDisability.Visible = True 'Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CheckboxDisabilityVisible"))
                    Me.CheckboxRegular.Visible = False
                End If
                'Comment code Deleted By Sanjeev on 06/02/2012
                Me.LabelTermination.Visible = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("LabelTerminationVisible"))
                Me.LabelBATermination.Visible = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("LabelBATerminationVisible"))

                'TextBox Value
                Me.TextboxRetirementNonTaxable.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxRetirementNonTaxable"))
                Me.TextboxRetirementTaxable.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxRetirementTaxable"))
                Me.TextboxRetirementTaxWithheld.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxRetirementTaxWithheld"))
                Me.TextboxRetirementNetAmount.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxRetirementNetAmount"))
                Me.TextboxSavingsNetAmount.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxSavingsNetAmount"))
                Me.TextboxSavingsNonTaxable.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxSavingsNonTaxable"))
                Me.TextboxSavingsTaxable.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxSavingsTaxable"))
                Me.TextboxSavingsTaxWithheld.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxSavingsTaxWithheld"))

                'FillValuesToControls
                TextboxTaxRate.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxTaxRate"))
                TextboxNonTaxed.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxNonTaxed"))
                TextboxTaxed.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxTaxed"))
                TextboxTax.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxTax"))
                TextboxNet.Text = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxNet"))
                Me.FederalTaxRate = TextboxTaxRate.Text 'Manthan | 2016.06.16 | YRS-AT-2962 | Assigning configurable tax rate to property
                'TextBox Visible
                Me.TextboxPartialSavings.Visible = False 'Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxPartialSavingsVisible"))
                'Comment code Deleted By Sanjeev on 06/02/2012
                Me.TextboxPartialRetirement.Visible = False 'Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxPartialRetirementVisible"))
                'Comment code Deleted By Sanjeev on 06/02/2012
                Me.TextboxTerminatePIA.Visible = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxTerminatePIAVisible"))
                Me.TextboxBATerminate.Visible = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextboxBATerminateVisible"))

                'START : Shilpa N | 05/22/2019 | YRS-AT-4055 | Assigning visibility
                Me.txtAggregateBALegacyatRequest.Visible = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TextAggregateBALegacyatRequestVisible"))
                Me.lblAggregateBALegacyatRequest.Visible = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("LabelAggregateBALegacyatRequestVisible"))
                'END : Shilpa N | 05/22/2019 | YRS-AT-4055 | Assigning visibility

                'New Modification for Market Based Withdrawal Amit Nigam
                settextboxvisibleRetirement(False)
                settextboxvisibleSavings(False)
                LabelFirstInstallment.Visible = False
                LabelPercentage.Visible = False
                TextboxFirstInstallment.Visible = False
                TextboxPercentage.Visible = False
                LabelNonTaxableMarket.Visible = False
                LabelTaxableMarket.Visible = False
                LabelTaxWithHeldMarket.Visible = False
                LabelNetAmount.Visible = False
                TextboxNonTaxableMarket.Visible = False
                TextboxTaxableMarket.Visible = False
                TextboxTaxWithHeldMarket.Visible = False
                TextboxNetAmountMarket.Visible = False
                'New Modification for Market Based Withdrawal Amit Nigam
                Me.RefundType = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RefundType"))
                'START - MMR | 04/20/2020 |YRS-AT-4854 | Update tax value of Retirement plan and savings plan data with derived blended tax rate for default display of data
                'Initialize required details for Covid calculation
                InitializeCovidDetails()
                Me.BlendedTaxRate = GetBlendedTaxRate(FinalDataSet.Tables("RetirementPlanTable"), FinalDataSet.Tables("SavingPlanTable"))
                Me.UpdateDisplayTableTaxValueWithBlendedTaxRate(FinalDataSet.Tables("RetirementPlanTable"), Me.BlendedTaxRate)
                Me.UpdateDisplayTableTaxValueWithBlendedTaxRate(FinalDataSet.Tables("SavingPlanTable"), Me.BlendedTaxRate)
                Me.UpdateDisplayTableTaxValueWithBlendedTaxRate(FinalDataSet.Tables("CalculatedDatatableDisplay"), Me.BlendedTaxRate)
                'Display calculated Covid taxable and non-taxable amount on UI
                Me.DisplayCovidTaxableAndNonTaxableAmount()
                Session("FinalDataSet_C19") = FinalDataSet
                'END - MMR | 04/20/2020 |YRS-AT-4854 | Update tax value of Retirement plan and savings plan data with derived blended tax rate for default display of data
                'DataGrid Binding.
                BindDataGrid("IsRetirement", FinalDataSet.Tables("RetirementPlanTable"))

                BindDataGrid("IsSavings", FinalDataSet.Tables("SavingPlanTable"))
                'BindDataGrid("IsConsolidated", FinalDataSet.Tables("GrandTotalDatatable"))
                SetGlobalFlags("BOTH")
                '------ Added for Issue ID  1117 by pavan ---- Begin  -----------
                If Me.AllowPartialRefund = True Then
                    CheckboxPartialRetirement.Visible = True
                    CheckboxPartialSavings.Visible = True
                Else
                    CheckboxPartialRetirement.Visible = False
                    CheckboxPartialSavings.Visible = False
                End If
                '------ Added for Issue ID  1117 by pavan ---- END  -----------
                BindFinalDatagrid()
                '----BT:694:  MRD Request - Phase 2 

                CheckboxMRDSavingsCurrentYear.Visible = False
                CheckboxMRDRetirementCurrentYear.Visible = False

                If (Not FinalDataSet.Tables("MRDRecords") Is Nothing) Then
                    Me.CheckboxMRDRetirement.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementMRDCheckboxEnable"))
                    Me.CheckboxMRDSavings.Enabled = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SavingPlanMRDCheckboxEnable"))
                    PopulateMRDYear(FinalDataSet.Tables("MRDRecords"))
                Else
                    CheckboxMRDSavings.Visible = False
                    CheckboxMRDRetirement.Visible = False
                End If

                ' START | SR | 2016.05.24 | YRS-AT-2962 : Set configured Processing fee details                               
                RetirementPlanProcessingFee = IIf(String.IsNullOrEmpty(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementProcessingFee").ToString()), "0", Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementProcessingFee")))
                SavingsPlanProcessingFee = IIf(String.IsNullOrEmpty(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SavingsProcessingFee").ToString()), "0", Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SavingsProcessingFee")))
                RetirementProcessingFeeMessage = IIf(String.IsNullOrEmpty(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementProcessingFeeMessage").ToString()), "", Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementProcessingFeeMessage")))
                SavingsProcessingFeeMessage = IIf(String.IsNullOrEmpty(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SavingsProcessingFeeMessage").ToString()), "", Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SavingsProcessingFeeMessage")))
                ' END | SR | 2016.05.24 | YRS-AT-2962 : Set configured Processing fee details 

                ' START | SR | 2016.07.19 | YRS-AT-3015 : Set BA/Legacy account related details
                Me.IsBALegacyCombinedAmountSwitchedON = IIf(String.IsNullOrEmpty(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("MaxCombinedBasicAccountAmt").ToString()), False, Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("IsBALegacyCombinedAmountSwitchedON")))
                MaxCombinedBasicAccountAmt = IIf(String.IsNullOrEmpty(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("MaxCombinedBasicAccountAmt").ToString()), "0", Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("MaxCombinedBasicAccountAmt")))
                MaxYMCAAcctAmt = IIf(String.IsNullOrEmpty(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("MaxYMCAAcctAmt").ToString()), "0", Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("MaxYMCAAcctAmt")))
                MaxYMCALegacyAcctAmt = IIf(String.IsNullOrEmpty(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("MaxYMCALegacyAcctAmt").ToString()), "0", Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("MaxYMCALegacyAcctAmt")))
                ' END | SR | 2016.07.19 | YRS-AT-3015 : Set BA/Legacy account related details                               

                'START | Shilpa N | 05/09/2019 | YRS-AT-4055 | Commented Existing code , Populate aggregate of funded current PIA & funded BA from service data.
                'START | SR | YRS-AT-4055 | Populate aggregate of current PIA & BA from service data.
                'txtAggregateBALegacyatRequest.Text = FormatCurrency(IIf(String.IsNullOrEmpty(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CurrentPIA").ToString()), "0", Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CurrentPIA"))) _
                '    + IIf(String.IsNullOrEmpty(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("BACurrent").ToString()), "0", Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("BACurrent"))))
                txtAggregateBALegacyatRequest.Text = FormatCurrency(IIf(String.IsNullOrEmpty(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CurrentPIAFunded").ToString()), "0", Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CurrentPIAFunded"))) _
                   + IIf(String.IsNullOrEmpty(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("BACurrentFunded").ToString()), "0", Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("BACurrentFunded"))))
                'END | SR | YRS-AT-4055 | Populate aggregate of current PIA & BA from service data.
                'END | Shilpa N | 05/09/2019 | YRS-AT-4055 | Commented Existing code , Populate aggregate of funded current PIA & funded BA from service data.



            End If
        Catch
            Throw
        End Try

    End Function

    Private Function SetGlobalFlags(ByVal strPlanType As String)
        Try
            If FinalDataSet Is Nothing Then
                Me.FinalDataSet = Session("FinalDataSet_C19")
            End If
            OriginalDataSet = New DataSet
            Me.OriginalDataSet = Me.FinalDataSet.Copy()

            'Set flag values
            Me.IsTerminated = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("IsTerminated"))
            Me.IsVested = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("IsVested"))
            Me.CurrentPIA = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CurrentPIA"))
            Me.TerminationPIA = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TerminationPIA"))
            Me.BACurrent = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("BACurrent"))
            Me.BACurrentAtRequest = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("BACurrentAtRequest"))
            Me.BATermination = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("BATermination"))
            ''Me.RefundType = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RefundType"))
            ''Me.RefundType = ""
            Me.MinimumPIAToRetire = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("MinimumPIAToRetire"))
            Me.VoluntryAmount = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("VoluntryAmount"))
            Me.TDAccountAmount = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TDAccountAmount"))
            Me.RetirementPlan_PersonTotalAmountInitial = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementPlan_PersonTotalAmountInitial"))
            Me.BAMaxLimit = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("BAMaxLimit"))
            Me.MaximumPIAAmount = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("MaximumPIAAmount"))
            Me.RetirementPlan_TotalAvailableAmount = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementPlan_TotalAvailableAmount"))
            Me.VoluntryAmount_Retirement_Initial = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("VoluntryAmount_Retirement_Initial"))
            Me.l_AnnuityExists_Retirement = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("AnnuityExists_Retirement"))
            Me.l_AnnuityExists_Savings = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("AnnuityExists_Savings"))
            Me.l_SixMonthsValidationForPartial = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SixMonthsValidationForPartial"))
            Me.AllowPartialRefund = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("AllowPartialRefund"))
            Me.SessionStatusType = Convert.ToString(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("StatusType"))
            Me.RetirementPlanTotalAmount = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RetirementPlanTotalAmount"))
            Me.SavingsPlan_TotalAvailableAmount = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("SavingsPlan_TotalAvailableAmount"))
            Me.VoluntryAmount_Savings_Initial = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("VoluntryAmount_Savings_Initial"))
            Me.l_TMAmount = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TMAmount"))
            Me.l_TDAmount = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("TDAmount"))
            Me.l_RTAmount = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("RTAmount"))
            Me.PartialMinLimit = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("PartialMinLimit"))
            Me.CompulsoryRetirement = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CompulsoryRetirement"))
            Me.CompulsorySavings = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("CompulsorySavings"))
            Me.l_bool_AnnuityExists = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("AnnuityExists"))
            Me.FederalTaxRate = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("FederalTaxRate"))
            If (FinalDataSet.Tables("WithdrawalFlags").Rows(0)("Terminateddate").GetType.ToString <> "System.DBNull") Then
                Me.Terminateddate = Convert.ToDateTime(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("Terminateddate"))
            End If
            'Market Based withdrawal
            Me.MarketBasedThreshold = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("MarketBasedThreshold"))
            Me.MarketBasedFirstInstPercentage = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("MarketBasedPercentage"))
            Me.AllowMarketBased = Convert.ToBoolean(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("AllowMarketBased"))
            Me.VoluntryAmount_Retirement_Initial = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("VoluntryAmount_Retirement"))
            Me.VoluntryAmount_Savings_Initial = Convert.ToDecimal(FinalDataSet.Tables("WithdrawalFlags").Rows(0)("VoluntryAmount_Savings"))
            'Session("RetirementPlanAcctContribution") = OriginalDataSet.Tables("RetirementPlanFunded")
            'Session("SavingsPlanAcctContribution") = OriginalDataSet.Tables("SavingPlanFunded")
            If strPlanType = "BOTH" Then
                Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = OriginalDataSet.Tables("RetirementPlanTable")
                Session("Calculated_DisplaySavingsPlanAcctContribution_C19") = OriginalDataSet.Tables("SavingPlanTable")
                Me.RetirementPlan_TotalAmount = Me.RetirementPlan_TotalAvailableAmount
                Me.SavingsPlan_TotalAmount = Me.SavingsPlan_TotalAvailableAmount
            ElseIf strPlanType = "RET" Then
                Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = OriginalDataSet.Tables("RetirementPlanTable")
                Me.RetirementPlan_TotalAmount = Me.RetirementPlan_TotalAvailableAmount
            ElseIf strPlanType = "SAV" Then
                Session("Calculated_DisplaySavingsPlanAcctContribution_C19") = OriginalDataSet.Tables("SavingPlanTable")
                Me.SavingsPlan_TotalAmount = Me.SavingsPlan_TotalAvailableAmount
            End If

            Session("CalculatedDataTableDisplay_C19") = OriginalDataSet.Tables("CalculatedDataTableDisplay")
            Session("MemberRefundRequestDetails_C19") = OriginalDataSet.Tables("MemberRefundRequestDetails")
            ' Session("ContributionDataTable") = OriginalDataSet.Tables("ContributionDataTable")
            Session("VoluntryRetirementDatatable_C19") = OriginalDataSet.Tables("VoluntryRetirementDatatable")
            Session("HardshipDatatable_C19") = OriginalDataSet.Tables("HardshipDatatable")




        Catch
            Throw
        End Try
    End Function
    Private Function GetOriginalDataSet()
        Try
            If FinalDataSet Is Nothing Then
                Me.FinalDataSet = Session("FinalDataSet_C19")
            End If
            OriginalDataSet = New DataSet
            Me.OriginalDataSet = Me.FinalDataSet.Copy()

            'Session("RetirementPlanAcctContribution") = OriginalDataSet.Tables("RetirementPlanFunded")
            'Session("SavingsPlanAcctContribution") = OriginalDataSet.Tables("SavingPlanFunded")
            Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = OriginalDataSet.Tables("RetirementPlanTable")
            Session("Calculated_DisplaySavingsPlanAcctContribution_C19") = OriginalDataSet.Tables("SavingPlanTable")
            Session("CalculatedDataTableDisplay_C19") = OriginalDataSet.Tables("CalculatedDataTableDisplay")
            Session("MemberRefundRequestDetails_C19") = OriginalDataSet.Tables("MemberRefundRequestDetails")
            'Session("ContributionDataTable") = OriginalDataSet.Tables("ContributionDataTable")
            Session("VoluntryRetirementDatatable_C19") = OriginalDataSet.Tables("VoluntryRetirementDatatable")
            Session("HardshipDatatable_C19") = OriginalDataSet.Tables("HardshipDatatable")

        Catch
            Throw
        End Try
    End Function

    Public Function CreateDisplayDatatables(ByVal parameterDatatableRetirement As DataTable, ByVal parameterDatatableSavings As DataTable, ByVal parameterNumPercentageFactorSavingsPlan As Decimal, ByVal parameterNumPercentageFactorRetirementPlan As Decimal)
        'START: PPP | 07/28/2016 | YRS-AT-3065 | Account type and its taxable balance required to be checked in Savings Account area
        Dim strAccountType As String
        Dim bIsTaxableAmountExists As Boolean
        'END: PPP | 07/28/2016 | YRS-AT-3065 | Account type and its taxable balance required to be checked in Savings Account area
        Try
            Dim l_dt_DisplayRetirementPlan As New DataTable
            Dim DataRow As Integer
            Dim l_dr_DisplayRetirementPlan As DataRow
            Dim l_dt_DisplaySavingsPlan As New DataTable
            Dim l_dr_DisplaySavingsPlan As DataRow
            Dim l_dt_DisplayConsolidatedTotal As New DataTable

            Me.NumPercentageFactorofMoneyRetirement = parameterNumPercentageFactorRetirementPlan
            Me.NumPercentageFactorofMoneySavings = parameterNumPercentageFactorSavingsPlan

            'Me.FederalTaxRate = 20 'Manthan | 2016.06.16 | YRS-AT-2962 | Commented existing code to remove harcoded tax rate

            If Not parameterDatatableRetirement Is Nothing Then
                For DataRow = 0 To parameterDatatableRetirement.Rows.Count - 1
                    l_dr_DisplayRetirementPlan = parameterDatatableRetirement.Rows(DataRow)
                    If l_dr_DisplayRetirementPlan("AccountType").GetType.ToString <> "System.DBNull" Then
                        l_dr_DisplayRetirementPlan.BeginEdit()
                        l_dr_DisplayRetirementPlan("AccountType") = parameterDatatableRetirement.Rows(DataRow)("AccountType")
                        l_dr_DisplayRetirementPlan("AccountGroup") = parameterDatatableRetirement.Rows(DataRow)("AccountGroup")
                        l_dr_DisplayRetirementPlan("PlanType") = parameterDatatableRetirement.Rows(DataRow)("PlanType")
                        l_dr_DisplayRetirementPlan("IsBasicAccount") = parameterDatatableRetirement.Rows(DataRow)("IsBasicAccount")
                        l_dr_DisplayRetirementPlan("Taxable") = Math.Round((parameterDatatableRetirement.Rows(DataRow)("Taxable") * parameterNumPercentageFactorRetirementPlan), 2)
                        l_dr_DisplayRetirementPlan("Non-Taxable") = Math.Round((parameterDatatableRetirement.Rows(DataRow)("Non-Taxable") * parameterNumPercentageFactorRetirementPlan), 2)
                        l_dr_DisplayRetirementPlan("Interest") = Math.Round((parameterDatatableRetirement.Rows(DataRow)("Interest") * parameterNumPercentageFactorRetirementPlan), 2)
                        l_dr_DisplayRetirementPlan("TaxWithheld") = Math.Round(((((parameterDatatableRetirement.Rows(DataRow)("Taxable") + parameterDatatableRetirement.Rows(DataRow)("Interest"))) * Me.FederalTaxRate) / 100), 2)
                        'l_dr_DisplayRetirementPlan("Total") = Math.Round(parameterDatatableRetirement.Rows(DataRow)("Taxable") + parameterDatatableRetirement.Rows(DataRow)("Non-Taxable") + parameterDatatableRetirement.Rows(DataRow)("Interest"), 2)

                        l_dr_DisplayRetirementPlan("Total") = l_dr_DisplayRetirementPlan("Taxable") + l_dr_DisplayRetirementPlan("Non-Taxable") + l_dr_DisplayRetirementPlan("Interest")
                        l_dr_DisplayRetirementPlan.EndEdit()
                    End If
                Next
                Me.DataTableDisplayRetirementPlan = parameterDatatableRetirement
            End If
            If parameterDatatableSavings.Rows.Count > 0 Then
                For DataRow = 0 To parameterDatatableSavings.Rows.Count - 1
                    l_dr_DisplaySavingsPlan = parameterDatatableSavings.Rows(DataRow)

                    'START: PPP | 07/28/2016 | YRS-AT-3065 | If LD account exists and there is no TD account then "AccountType=Total" row contains null value for taxable, Non-Taxable and Interest columns
                    strAccountType = IIf(Convert.IsDBNull(l_dr_DisplaySavingsPlan("AccountType")), String.Empty, Convert.ToString(l_dr_DisplaySavingsPlan("AccountType")).Trim().ToUpper())
                    bIsTaxableAmountExists = IIf(Convert.IsDBNull(l_dr_DisplaySavingsPlan("Taxable")), False, True)

                    ''AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added to exclude select checkbox for ln & ld accounts while computations
                    'If l_dr_DisplaySavingsPlan("AccountType").GetType.ToString <> "System.DBNull" AndAlso (l_dr_DisplaySavingsPlan("AccountType").ToString.ToUpper.Trim <> "LN") AndAlso (l_dr_DisplaySavingsPlan("AccountType").ToString.ToUpper.Trim <> "LD") Then
                    If Not String.IsNullOrEmpty(strAccountType) AndAlso (strAccountType <> "LN") AndAlso (strAccountType <> "LD") AndAlso bIsTaxableAmountExists Then
                        'END: PPP | 07/28/2016 | YRS-AT-3065 | If LD account exists and there is no TD account then "AccountType=Total" row contains null value for taxable, Non-Taxable and Interest columns
                        'Code commented '2013.10.23        Dinesh Kanojia(DK)   BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                        'Code Added by Dinesh Kanojia on 30/07/2013
                        'If CheckboxPartialSavings.Checked Then
                        '    If parameterDatatableSavings.Rows(DataRow)("AccountType").ToString.ToUpper.Trim = "TD" And parameterDatatableSavings.Rows(DataRow)("PlanType").ToString.ToUpper.Trim = "SAVINGS" And TextBoxTerminated.Text.Trim.ToUpper = "YES" Then
                        l_dr_DisplaySavingsPlan.BeginEdit()
                        l_dr_DisplaySavingsPlan("AccountType") = parameterDatatableSavings.Rows(DataRow)("AccountType")
                        l_dr_DisplaySavingsPlan("AccountGroup") = parameterDatatableSavings.Rows(DataRow)("AccountGroup")
                        l_dr_DisplaySavingsPlan("PlanType") = parameterDatatableSavings.Rows(DataRow)("PlanType")
                        l_dr_DisplaySavingsPlan("IsBasicAccount") = parameterDatatableSavings.Rows(DataRow)("IsBasicAccount")
                        l_dr_DisplaySavingsPlan("Taxable") = Math.Round((parameterDatatableSavings.Rows(DataRow)("Taxable") * parameterNumPercentageFactorSavingsPlan), 2)
                        l_dr_DisplaySavingsPlan("Non-Taxable") = Math.Round((parameterDatatableSavings.Rows(DataRow)("Non-Taxable") * parameterNumPercentageFactorSavingsPlan), 2)
                        l_dr_DisplaySavingsPlan("Interest") = Math.Round((parameterDatatableSavings.Rows(DataRow)("Interest") * parameterNumPercentageFactorSavingsPlan), 2)
                        'l_dr_DisplaySavingsPlan("TaxWithheld") = Math.Round(((((parameterDatatableSavings.Rows(DataRow)("Taxable") + parameterDatatableSavings.Rows(DataRow)("Interest"))) * Me.FederalTaxRate) / 100), 2)
                        If Me.RefundType.Trim() = "HARD" Then
                            If l_dr_DisplaySavingsPlan("AccountType") = "TD" Then
                                Dim FedTaxrate As Decimal = Nothing
                                FedTaxrate = 10
                                Me.TextboxTaxRate.Text = 10
                                Me.FederalTaxRate = 10
                                If Not objRefundProcess.IsInterestAllowed Then 'MMR | 07/16/2019 | YRS-AT-4498 | Added condition to check if key is off then do not condsider interest for tax calculation
                                    l_dr_DisplaySavingsPlan("TaxWithheld") = Math.Round((((CDec(l_dr_DisplaySavingsPlan("Taxable"))) * FedTaxrate) / 100), 2)
                                    'START: MMR | 07/16/2019 | YRS-AT-4498 | Added condition to check if key is on then condsider interest for tax calculation
                                Else
                                    l_dr_DisplaySavingsPlan("TaxWithheld") = Math.Round(((((CDec(l_dr_DisplaySavingsPlan("Taxable")) + CDec(l_dr_DisplaySavingsPlan("Interest")))) * FedTaxrate) / 100), 2)
                                End If
                                'END: MMR | 07/16/2019 | YRS-AT-4498 | Added condition to check if key is on then condsider interest for tax calculation
                            Else
                                l_dr_DisplaySavingsPlan("TaxWithheld") = Math.Round(((((CDec(l_dr_DisplaySavingsPlan("Taxable")) + CDec(l_dr_DisplaySavingsPlan("Interest")))) * Me.FederalTaxRate) / 100), 2)
                            End If
                        Else
                            l_dr_DisplaySavingsPlan("TaxWithheld") = Math.Round(((((CDec(l_dr_DisplaySavingsPlan("Taxable")) + CDec(l_dr_DisplaySavingsPlan("Interest")))) * Me.FederalTaxRate) / 100), 2)
                        End If
                        l_dr_DisplaySavingsPlan("Total") = Math.Round(parameterDatatableSavings.Rows(DataRow)("Taxable") + parameterDatatableSavings.Rows(DataRow)("Non-Taxable") + parameterDatatableSavings.Rows(DataRow)("Interest"), 2)
                        l_dr_DisplaySavingsPlan.EndEdit()
                    End If
                    'Start :Code commented '2013.10.23        Dinesh Kanojia(DK)   BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                    '    If parameterDatatableSavings.Rows(DataRow)("AccountType").ToString.ToUpper.Trim <> "TD" And parameterDatatableSavings.Rows(DataRow)("PlanType").ToString.ToUpper.Trim = "SAVINGS" Then
                    '        l_dr_DisplaySavingsPlan.BeginEdit()
                    '        l_dr_DisplaySavingsPlan("AccountType") = parameterDatatableSavings.Rows(DataRow)("AccountType")
                    '        l_dr_DisplaySavingsPlan("AccountGroup") = parameterDatatableSavings.Rows(DataRow)("AccountGroup")
                    '        l_dr_DisplaySavingsPlan("PlanType") = parameterDatatableSavings.Rows(DataRow)("PlanType")
                    '        l_dr_DisplaySavingsPlan("IsBasicAccount") = parameterDatatableSavings.Rows(DataRow)("IsBasicAccount")
                    '        l_dr_DisplaySavingsPlan("Taxable") = Math.Round((parameterDatatableSavings.Rows(DataRow)("Taxable") * parameterNumPercentageFactorSavingsPlan), 2)
                    '        l_dr_DisplaySavingsPlan("Non-Taxable") = Math.Round((parameterDatatableSavings.Rows(DataRow)("Non-Taxable") * parameterNumPercentageFactorSavingsPlan), 2)
                    '        l_dr_DisplaySavingsPlan("Interest") = Math.Round((parameterDatatableSavings.Rows(DataRow)("Interest") * parameterNumPercentageFactorSavingsPlan), 2)
                    '        'l_dr_DisplaySavingsPlan("TaxWithheld") = Math.Round(((((parameterDatatableSavings.Rows(DataRow)("Taxable") + parameterDatatableSavings.Rows(DataRow)("Interest"))) * Me.FederalTaxRate) / 100), 2)
                    '        If Me.RefundType.Trim() = "HARD" Then
                    '            If l_dr_DisplaySavingsPlan("AccountType") = "TD" Then
                    '                Dim FedTaxrate As Decimal = Nothing
                    '                FedTaxrate = 10
                    '                Me.TextboxTaxRate.Text = 10
                    '                Me.FederalTaxRate = 10
                    '                l_dr_DisplaySavingsPlan("TaxWithheld") = Math.Round((((CDec(l_dr_DisplaySavingsPlan("Taxable"))) * FedTaxrate) / 100), 2)
                    '            Else
                    '                l_dr_DisplaySavingsPlan("TaxWithheld") = Math.Round(((((CDec(l_dr_DisplaySavingsPlan("Taxable")) + CDec(l_dr_DisplaySavingsPlan("Interest")))) * Me.FederalTaxRate) / 100), 2)
                    '            End If
                    '        Else
                    '            l_dr_DisplaySavingsPlan("TaxWithheld") = Math.Round(((((CDec(l_dr_DisplaySavingsPlan("Taxable")) + CDec(l_dr_DisplaySavingsPlan("Interest")))) * Me.FederalTaxRate) / 100), 2)
                    '        End If
                    '        l_dr_DisplaySavingsPlan("Total") = Math.Round(parameterDatatableSavings.Rows(DataRow)("Taxable") + parameterDatatableSavings.Rows(DataRow)("Non-Taxable") + parameterDatatableSavings.Rows(DataRow)("Interest"), 2)
                    '        l_dr_DisplaySavingsPlan.EndEdit()
                    '    End If
                    'End If

                    'If CheckboxVoluntryAccounts.Checked Then
                    '    If parameterDatatableSavings.Rows(DataRow)("AccountType").ToString.ToUpper.Trim = "TD" And parameterDatatableSavings.Rows(DataRow)("PlanType").ToString.ToUpper.Trim = "SAVINGS" And TextBoxTerminated.Text.Trim.ToUpper = "NO" Then

                    '    End If
                    'End If
                    'End If
                    'End :Code commented '2013.10.23        Dinesh Kanojia(DK)   BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                Next
                Me.DataTableDisplaySavingsPlan = parameterDatatableSavings
            End If
            'Start: Dinesh Kanojia(DK)     2013.07.14      BT-1502: YRS 5.0-1746:Include partial withdrawals 
            'Added two parameter for partial withdrawal
            '2013.10.23        Dinesh Kanojia(DK)   BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
            'Renmove two parameter for partial withdrawal
            'parameterDatatableSavings = objRefundRequest.CalculateTotalForDisplay(parameterDatatableSavings, False, CheckboxPartialSavings.Checked, TextBoxTerminated.Text.Trim.Trim.ToUpper)
            parameterDatatableSavings = objRefundRequest.CalculateTotalForDisplay(parameterDatatableSavings, False)
            parameterDatatableRetirement = objRefundRequest.CalculateTotalForDisplay(parameterDatatableRetirement, False)

            l_dt_DisplayConsolidatedTotal = DataTableDisplaySavingsPlan.Clone()
            For Each dr As DataRow In DataTableDisplayRetirementPlan.Rows
                l_dt_DisplayConsolidatedTotal.ImportRow(dr)
            Next
            For Each dr As DataRow In DataTableDisplaySavingsPlan.Rows
                l_dt_DisplayConsolidatedTotal.ImportRow(dr)
            Next
            Me.DataTableDisplayConsolidatedTotal = l_dt_DisplayConsolidatedTotal
            Me.DataTableDisplayRetirementPlan = parameterDatatableRetirement
            Me.DataTableDisplaySavingsPlan = parameterDatatableSavings

        Catch
            Throw
        End Try
    End Function

    Private Function BindDataGrid(ByVal parameterCalledBy As String, ByVal parameterDatatable As DataTable)
        Try
            'DisplayAccountContribution 
            Dim l_DataTable As DataTable
            Dim l_DataTable_RetirementPlanAcctContribution As DataTable
            Dim l_DataTable_SavingsPlanAcctContribution As DataTable
            Dim l_DataTable_CalculatedDataTable As DataTable
            Dim NumPercentageFactorofMoneySavings As Decimal
            Dim NumPercentageFactorofMoneyRetirement As Decimal

            'Added By Parveen For Market Based Withdrawal on 16-Nov 2009 
            RemoveMarketBasedRecords(parameterCalledBy, parameterDatatable)
            'Added By Parveen For Market Based Withdrawal on 16-Nov 2009 

            If parameterCalledBy = "IsRetirement" Then
                'DataGrid Binding.
                l_DataTable_RetirementPlanAcctContribution = parameterDatatable
                If Not l_DataTable_RetirementPlanAcctContribution Is Nothing Then

                    If Me.RefundType = "VOL" And Me.CheckboxExcludeYMCA.Checked = False Or _
                    (Me.CheckboxVoluntryAccounts.Checked = True And Me.RefundType = "PART") Then
                        If Me.CheckboxVoluntryAccounts.Checked = True Then
                            '--STart Manthan Rajguru 2015.09.24 YRS-AT-2550: Custom Control reference given for DatagridcheckBox
                            datagridCheckBox = New CustomControls.CheckBoxColumn(True, "Selected")
                        Else
                            datagridCheckBox = New CustomControls.CheckBoxColumn(False, "Selected")
                        End If
                    Else
                        datagridCheckBox = New CustomControls.CheckBoxColumn(False, "Selected")
                        '--End Manthan Rajguru 2015.09.24 YRS-AT-2550: Custom Control reference given for DatagridcheckBox
                    End If


                    'If Me.TextBoxTerminated.Text.ToUpper().Trim = "NO" Then
                    '    CheckboxPartialRetirement.Enabled = False
                    'End If

                    datagridCheckBox.DataField = "Selected"
                    datagridCheckBox.AutoPostBack = True
                    Me.DataGridAccContributionRetirement.Columns.Clear()
                    Me.DataGridAccContributionRetirement.Columns.Add(datagridCheckBox)
                    Me.DataGridAccContributionRetirement.DataSource = Nothing
                    Me.DataGridAccContributionRetirement.DataSource = l_DataTable_RetirementPlanAcctContribution
                    Me.DataGridAccContributionRetirement.DataBind()
                    Me.SetSelectedIndex(Me.DataGridAccContributionRetirement, l_DataTable_RetirementPlanAcctContribution)
                End If
            End If
            If parameterCalledBy = "IsSavings" Then
                l_DataTable_SavingsPlanAcctContribution = parameterDatatable
                If Not l_DataTable_SavingsPlanAcctContribution Is Nothing Then

                    If Me.RefundType = "VOL" Or Me.RefundType = "DISAB" Or Me.RefundType = "REG" Or Me.RefundType = "PERS" _
                 Or (Me.RefundType = "PART" And Me.CheckboxPartialSavings.Checked = False) Then

                        If Me.CheckboxSavingsVoluntary.Checked = True And Me.CompulsorySavings = False Then
                            '--STart Manthan Rajguru 2015.09.24 YRS-AT-2550: Custom Control reference given for DatagridcheckBox
                            datagridCheckBox_Savings = New CustomControls.CheckBoxColumn(True, "Selected")
                        ElseIf Me.CompulsorySavings = True And Me.CheckboxSavingsVoluntary.Checked = True Then
                            datagridCheckBox_Savings = New CustomControls.CheckBoxColumn(False, "Selected")
                        Else
                            datagridCheckBox_Savings = New CustomControls.CheckBoxColumn(False, "Selected")
                        End If
                    Else
                        datagridCheckBox_Savings = New CustomControls.CheckBoxColumn(False, "Selected")
                        '--End Manthan Rajguru 2015.09.24 YRS-AT-2550: Custom Control reference given for DatagridcheckBox
                    End If

                    'Start Code Commneted : '2013.10.23        Dinesh Kanojia(DK)   BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account
                    'START DInesh.k 2013.06.28 BT-1502: YRS 5.0-1746:Partial withdrawal not available if employed 

                    'If l_DataTable_SavingsPlanAcctContribution.Rows.Count <= 4 And Me.TextBoxTerminated.Text.ToUpper.Trim = "NO" Then
                    '    If l_DataTable_SavingsPlanAcctContribution.Rows(0)("AccountType").ToString().ToUpper() = "TD" Then
                    '        CheckboxPartialSavings.Enabled = False
                    '    End If
                    'End If
                    'If CheckboxPartialSavings.Checked Then
                    '    If l_DataTable_SavingsPlanAcctContribution.Rows.Count > 0 And Me.TextBoxTerminated.Text.ToUpper.Trim = "NO" Then
                    '        For iCount As Integer = 0 To l_DataTable_SavingsPlanAcctContribution.Rows.Count - 1
                    '            If l_DataTable_SavingsPlanAcctContribution.Rows(iCount)("AccountType").ToString().ToUpper() = "TD" And l_DataTable_SavingsPlanAcctContribution.Rows(iCount)("Selected").ToString.Trim <> "False" Then
                    '                l_DataTable_SavingsPlanAcctContribution.Rows(iCount)("Selected") = "False"
                    '                Me.SavingsPlan_TotalAvailableAmount = Me.SavingsPlan_TotalAvailableAmount - Convert.ToDecimal(l_DataTable_SavingsPlanAcctContribution.Rows(iCount)("Total").ToString.Trim)
                    '                Me.VoluntryAmount_Savings_Initial = Me.VoluntryAmount_Savings_Initial - Convert.ToDecimal(l_DataTable_SavingsPlanAcctContribution.Rows(iCount)("Total").ToString.Trim)
                    '            End If
                    '        Next
                    '    End If
                    'End If


                    'If l_DataTable_SavingsPlanAcctContribution.Rows.Count > 0 And Me.TextBoxTerminated.Text.ToUpper.Trim = "NO" And Me.PersonAge >= 59.5 Then
                    '    For iCount As Integer = 0 To l_DataTable_SavingsPlanAcctContribution.Rows.Count - 1
                    '        If l_DataTable_SavingsPlanAcctContribution.Rows(iCount)("AccountType").ToString().ToUpper() = "TD" Then
                    '            l_DataTable_SavingsPlanAcctContribution.Rows(iCount)("Selected") = "False"
                    '            l_DataTable_SavingsPlanAcctContribution.Rows(iCount)("IsAvailable") = "False"
                    '            Me.SavingsPlan_TotalAvailableAmount = Me.SavingsPlan_TotalAvailableAmount - Convert.ToDecimal(l_DataTable_SavingsPlanAcctContribution.Rows(iCount)("Total").ToString.Trim)
                    '            Me.VoluntryAmount_Savings_Initial = Me.VoluntryAmount_Savings_Initial - Convert.ToDecimal(l_DataTable_SavingsPlanAcctContribution.Rows(iCount)("Total").ToString.Trim)
                    '        End If
                    '    Next
                    'End If

                    'END DInesh.k 2013.06.28 BT-1502: YRS 5.0-1746:Partial withdrawal not available if employed 
                    'End Code Commneted : '2013.10.23        Dinesh Kanojia(DK)   BT-1502: YRS 5.0-1746:Include partial withdrawals - Re-Opened by Mark to allow partial withdrawal from TD account

                    datagridCheckBox_Savings.DataField = "Selected"
                    datagridCheckBox_Savings.AutoPostBack = True
                    Me.DataGridAccContributionSavings.Columns.Clear()
                    Me.DataGridAccContributionSavings.Columns.Add(datagridCheckBox_Savings)
                    Me.DataGridAccContributionSavings.DataSource = Nothing
                    Me.DataGridAccContributionSavings.DataSource = l_DataTable_SavingsPlanAcctContribution
                    Me.DataGridAccContributionSavings.DataBind()
                    Me.SetSelectedIndex(Me.DataGridAccContributionSavings, l_DataTable_SavingsPlanAcctContribution)
                    Me.l_DisplaySavingsPlanAcctContribution = l_DataTable_SavingsPlanAcctContribution


                End If
            End If
            If parameterCalledBy = "IsConsolidated" Then
                l_DataTable_CalculatedDataTable = parameterDatatable
                If Not l_DataTable_CalculatedDataTable Is Nothing Then
                    Me.DatagridGrandTotal.Columns(6).HeaderText = "Tax Withheld(" + Me.FederalTaxRate.ToString + "%)"
                    Me.DatagridGrandTotal.DataSource = l_DataTable_CalculatedDataTable
                    Me.DatagridGrandTotal.DataBind()
                    Me.SetSelectedIndex(Me.DatagridGrandTotal, l_DataTable_CalculatedDataTable)
                    Me.l_DisplayConsolidatedTotal = l_DataTable_CalculatedDataTable
                End If
                ' Me.l_DisplayConsolidatedTotal = l_DataTable
            End If
        Catch
            Throw
        End Try
    End Function
    Private Sub PopulateMRDYear(ByVal parameterDataTable As DataTable)
        Dim drRet As DataRow()
        Dim drSaving As DataRow()
        ' START: CS | 2016.10.12 | YRS-AT-3073 | Declaring Variable's for handing RMD year(s)
        Dim currentYearsRetirement As String
        Dim currentYearsSavings As String
        Dim previousYearsRetirement As New StringBuilder
        Dim previousYearSavings As New StringBuilder
        ' END: CS | 2016.10.12 | YRS-AT-3073 | Declaring Variable's for handing RMD year(s)
        Try
            If FinalDataSet Is Nothing Then
                Me.FinalDataSet = Session("FinalDataSet_C19")
            End If
            If parameterDataTable.Rows.Count > 0 Then
                drRet = FinalDataSet.Tables("MRDRecords").Select("PlanType='RETIREMENT'")
                If drRet.Length > 0 Then
                    If drRet.Length > 1 Then
                        'CheckboxMRDRetirementCurrentYear.Visible = True
                        ' START: CS | 2016.10.12 | YRS-AT-3073 | Assiging RMD Previous Year(s) to Variable 
                        currentYearsRetirement = String.Empty
                        If CheckboxMRDRetirementCurrentYear.Text.IndexOf(drRet(drRet.Length - 1)("MRDYear").ToString()) < 0 Then
                            currentYearsRetirement = drRet(drRet.Length - 1)("MRDYear").ToString()
                            CheckboxMRDRetirementCurrentYear.Text += String.Format(" ({0})", currentYearsRetirement)
                            Me.RMDCurrentYearRetirement = currentYearsRetirement
                        End If
                        ' END: CS | 2016.10.12 | YRS-AT-3073 | Declaring Variable's for handing RMD year(s)
                        'Commented By SG: 2012.03.13: BT-1010
                        'If CheckboxMRDRetirement.Text.IndexOf(drRet(drRet.Length - 2)("MRDYear").ToString()) < 0 Then
                        '    CheckboxMRDRetirement.Text += drRet(drRet.Length - 2)("MRDYear").ToString()
                        'End If

                        'START: SG: 2012.03.13: BT-1010
                        ' START: CS | 2016.10.12 | YRS-AT-3073 | Assiging RMD Previous Year(s) to Variable 

                        If CheckboxMRDRetirement.Text.IndexOf("(") < 0 Then
                            previousYearsRetirement.Append(" (")
                            For intRetcount As Integer = 0 To drRet.Length - 2
                                If CheckboxMRDRetirement.Text.IndexOf(drRet(intRetcount)("MRDYear").ToString()) < 0 Then
                                    previousYearsRetirement.Append(drRet(intRetcount)("MRDYear").ToString())
                                    If intRetcount <> drRet.Length - 2 Then
                                        previousYearsRetirement.Append(", ")
                                    End If
                                End If
                            Next
                            previousYearsRetirement.Append(")")
                            CheckboxMRDRetirement.Text += previousYearsRetirement.ToString()
                        End If
                        ' END: CS | 2016.10.12 | YRS-AT-3073 | Assiging RMD Previous Year(s) to Variable 
                        'END: SG: 2012.03.13: BT-1010
                    Else
                        ' START: CS | 2016.10.12 | YRS-AT-3073 | Assiging RMD Previous Year(s) to Variable 
                        If CheckboxMRDRetirement.Text.IndexOf(drRet(0)("MRDYear")) < 0 Then
                            previousYearsRetirement.Append(drRet(0)("MRDYear").ToString())
                            CheckboxMRDRetirement.Text += String.Format(" ({0})", previousYearsRetirement.ToString())
                        End If
                        ' END: CS | 2016.10.12 | YRS-AT-3073 | Assiging RMD Previous Year(s) to Variable 

                    End If
                End If


                drSaving = parameterDataTable.Select("PlanType='SAVINGS'")
                If drSaving.Length > 0 Then
                    If drSaving.Length > 1 Then
                        'CheckboxMRDSavingsCurrentYear.Visible = True
                        ' START: CS | 2016.10.12 | YRS-AT-3073 | Assiging RMD Current Year(s) to Variable 
                        currentYearsSavings = String.Empty
                        If CheckboxMRDSavingsCurrentYear.Text.IndexOf(drSaving(drSaving.Length - 1)("MRDYear").ToString()) < 0 Then
                            currentYearsSavings = drSaving(drSaving.Length - 1)("MRDYear")
                            CheckboxMRDSavingsCurrentYear.Text += String.Format(" ({0})", currentYearsSavings)
                            Me.RMDCurrentYearSavings = currentYearsSavings
                        End If
                        ' END: CS | 2016.10.12 | YRS-AT-3073 | Assiging RMD Current Year(s) to Variable 
                        'Commented By SG: 2012.03.13: BT-1010
                        'If CheckboxMRDSavings.Text.IndexOf(drSaving(drSaving.Length - 2)("MRDYear")) < 0 Then
                        '    CheckboxMRDSavings.Text += " (" & drSaving(drSaving.Length - 2)("MRDYear") & ")"
                        'End If

                        'START: SG: 2012.03.13: BT-1010
                        ' START: CS | 2016.10.12 | YRS-AT-3073 | Assiging RMD Previous Year(s) to Variable 
                        If CheckboxMRDSavings.Text.IndexOf("(") < 0 Then
                            previousYearSavings.Append(" (")
                            For intSavcount As Integer = 0 To drSaving.Length - 2
                                If CheckboxMRDSavings.Text.IndexOf(drSaving(intSavcount)("MRDYear").ToString()) < 0 Then
                                    previousYearSavings.Append(drSaving(intSavcount)("MRDYear").ToString())
                                    If intSavcount <> drSaving.Length - 2 Then
                                        previousYearSavings.Append(", ")
                                    End If
                                End If
                            Next
                            previousYearSavings.Append(")")
                            CheckboxMRDSavings.Text += previousYearSavings.ToString()
                        End If
                        'END: SG: 2012.03.13: BT-1010
                    Else
                        If CheckboxMRDSavings.Text.IndexOf(drSaving(0)("MRDYear")) < 0 Then
                            previousYearSavings.Append(drSaving(0)("MRDYear"))
                            CheckboxMRDSavings.Text += String.Format(" ({0})", previousYearSavings.ToString())
                        End If
                        ' END: CS | 2016.10.12 | YRS-AT-3073 | Assiging RMD Previous Year(s) to Variable 

                    End If
                End If
            End If

        Catch
            Throw
        End Try
    End Sub
    Private Function GetMRDYear(ByVal parameterCalledBy As String) As String
        Dim drRet As DataRow()
        Dim drSaving As DataRow()
        Try
            If FinalDataSet Is Nothing Then
                Me.FinalDataSet = Session("FinalDataSet_C19")
            End If

            If parameterCalledBy = "RETIREMENT" Then
                drRet = FinalDataSet.Tables("MRDRecords").Select("PlanType='RETIREMENT'")

                ' START: SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState to check given condition
                'If drRet.Length > 1 And Session("MRDRetirementCurrentYear") Then
                If drRet.Length > 1 And Me.IsMRDRetirementCurrentYear Then
                    ' END: SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState to check given condition
                    Return drRet(drRet.Length - 1)("MRDYear").ToString()
                Else
                    If drRet.Length > 1 Then
                        CheckboxMRDRetirementCurrentYear.Visible = True
                        'START: SG:2012.03.13: BT-1010
                        Return drRet(drRet.Length - 2)("MRDYear").ToString()
                    Else
                        'END: SG:2012.03.13: BT-1010
                        Return drRet(0)("MRDYear").ToString()
                    End If

                End If
            End If

            'For Each dr As DataRow In drRet
            '    dropdownlistRetirementMrdYear.Items.Add(New ListItem(dr("MRDYear").ToString(), dr("Balance").ToString()))
            'Next

            If parameterCalledBy = "SAVINGS" Then
                drSaving = FinalDataSet.Tables("MRDRecords").Select("PlanType='SAVINGS'")

                ' START: SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState to check given condition
                'If drSaving.Length > 1 And Session("MRDSavingsCurrentYear") Then
                If drSaving.Length > 1 And Me.IsMRDSavingsCurrentYear Then
                    ' END: SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState to check given condition
                    Return drSaving(drSaving.Length - 1)("MRDYear").ToString()
                Else
                    If drSaving.Length > 1 Then
                        CheckboxMRDSavingsCurrentYear.Visible = True
                        'START: SG:2012.03.13: BT-1010
                        Return drSaving(drSaving.Length - 2)("MRDYear").ToString()
                    Else
                        'END: SG:2012.03.13: BT-1010
                        Return drSaving(0)("MRDYear").ToString()
                    End If
                End If
            End If





        Catch
            Throw
        End Try
    End Function

    Public Function ArrayOfStringPropertys() As String()
        Try
            Dim StingArrayProperties() As String = {Me.SSNNo.Trim(), Me.RefundType, CType(Me.CheckboxRegular.Checked, String), CType(Me.CheckboxExcludeYMCA.Checked, String), CType(Me.CheckboxPartialRetirement.Checked, String), CType(Me.CheckboxVoluntryAccounts.Checked, String), CType(Me.CheckboxPartialSavings.Checked, String), CType(Me.CheckboxSavingsVoluntary.Checked, String), CType(Me.CheckboxHardship.Checked, String), CType(Me.CheckboxSpecial.Checked, String), CType(Me.CheckboxDisability.Checked, String), CType(Me.CheckboxSavingsPlan.Checked, String), CType(Me.NumPercentageFactorofMoneyRetirement, String), CType(Me.NumPercentageFactorofMoneySavings, String), CType(Me.IsPartialRetirementAmountGivenTabOutDone, String), CType(Me.TextboxTaxed.Text, String), CType(Me.TextboxNonTaxed.Text, String), CType(Me.TextboxTax.Text, String), CType(Me.TextboxRetirementTaxable.Text, String), CType(Me.TextboxRetirementNonTaxable.Text, String), CType(Me.TextboxRetirementTaxWithheld.Text, String), CType(Me.TextboxSavingsTaxable.Text, String), CType(Me.TextboxSavingsNonTaxable.Text, String), CType(Me.TextboxSavingsTaxWithheld.Text, String), CType(Me.TextboxPartialRetirement.Text, String), CType(Me.TextboxPartialSavings.Text, String)}
            Return StingArrayProperties
        Catch
            Throw
        End Try
    End Function

    Public Function ArrayOfSelectedRetirementAccountTypes(ByVal parameterDatatable As DataTable) As String()
        Try
            Dim l_drSelectedAccount As DataRow

            For intcount As Integer = 0 To parameterDatatable.Rows.Count - 1
                l_drSelectedAccount = parameterDatatable.Rows(intcount)
                If l_drSelectedAccount("AccountType").GetType.ToString <> "System.DBNull" And l_drSelectedAccount("Selected").GetType.ToString <> "System.DBNull" Then
                    If l_drSelectedAccount("AccountType") <> "Total" And l_drSelectedAccount("Selected") = "True" Then
                        AddElementToRetirementStringArray(l_drSelectedAccount("AccountType"))
                    End If
                End If
            Next
            Return ArraySlectedRetirementAccounts
        Catch
            Throw
        End Try
    End Function

    Public Function ArrayOfSelectedSavingsAccountTypes(ByVal parameterDatatable As DataTable) As String()
        Try
            Dim l_drSelectedAccount As DataRow
            For intcount As Integer = 0 To parameterDatatable.Rows.Count - 1
                l_drSelectedAccount = parameterDatatable.Rows(intcount)
                If l_drSelectedAccount("AccountType").GetType.ToString <> "System.DBNull" And l_drSelectedAccount("Selected").GetType.ToString <> "System.DBNull" Then
                    If l_drSelectedAccount("AccountType") <> "Total" And l_drSelectedAccount("Selected") = "True" Then
                        AddElementToSavingsStringArray(l_drSelectedAccount("AccountType"))
                    End If
                End If
            Next
            Return ArraySlectedSavingsAccounts
        Catch
            Throw
        End Try
    End Function

    Public Sub AddElementToRetirementStringArray(ByVal stringToAdd As String)
        Try
            ReDim Preserve ArraySlectedRetirementAccounts(myStringRetirementElements)
            ArraySlectedRetirementAccounts(myStringRetirementElements) = stringToAdd
            myStringRetirementElements += 1
        Catch
            Throw
        End Try

    End Sub

    Public Sub AddElementToSavingsStringArray(ByVal stringToAdd As String)
        Try
            ReDim Preserve ArraySlectedSavingsAccounts(myStringSavingsElements)
            ArraySlectedSavingsAccounts(myStringSavingsElements) = stringToAdd
            myStringSavingsElements += 1
        Catch
            Throw
        End Try

    End Sub

    Public Sub MakeFinalDataTables(ByVal parameterCalledBy As String)
        Dim l_CheckBox As CheckBox
        Dim l_DataGridItem As DataGridItem
        Dim l_CalculatedRetirementDataTable As New DataTable
        Dim l_drCalculatedRetirementDataTable As DataRow
        Dim l_CalculatedSavingsDataTable As New DataTable
        Dim l_drCalculatedSavingsDataTable As DataRow
        Dim l_DataTable_RetirementDataTable As DataTable
        Dim l_DataTable_SavingsDataTable As DataTable
        Dim intcnt As Integer = 0
        Dim MarketAccountType As String
        Dim l_RetRow As DataRow()
        Dim l_SavingsRow As DataRow()

        Try
            MarketAccountType = "Market Based (" + CType(Session("MarketBasedFirstInstPercentage_C19"), String) + "%)"
            If parameterCalledBy = "IsRetirement" Then
                l_CalculatedRetirementDataTable = Session("Calculated_DisplayRetirementPlanAcctContribution_C19")
                l_DataTable_RetirementDataTable = l_CalculatedRetirementDataTable.Clone
                l_RetRow = l_CalculatedRetirementDataTable.Select("AccountType='" + MarketAccountType + "'")
                For Each l_DataGridItem In Me.DataGridAccContributionRetirement.Items
                    l_CheckBox = l_DataGridItem.FindControl("Selected")
                    l_drCalculatedRetirementDataTable = l_CalculatedRetirementDataTable.Rows(intcnt)
                    intcnt = intcnt + 1
                    If l_drCalculatedRetirementDataTable("AccountType").GetType.ToString <> "System.DBNull" Then
                        If Not ((CType(l_drCalculatedRetirementDataTable("AccountType"), String) = "Total")) Then
                            If l_CheckBox.Checked = True Then
                                l_drCalculatedRetirementDataTable.BeginEdit()
                                l_drCalculatedRetirementDataTable("Selected") = "True"
                                l_drCalculatedRetirementDataTable.EndEdit()
                            Else
                                l_drCalculatedRetirementDataTable.BeginEdit()
                                l_drCalculatedRetirementDataTable("Selected") = "False"
                                l_drCalculatedRetirementDataTable.EndEdit()
                            End If
                        End If
                    End If
                    l_DataTable_RetirementDataTable.ImportRow(l_drCalculatedRetirementDataTable)
                Next
                Session("Calculated_DisplayRetirementPlanAcctContribution_Final_C19") = l_DataTable_RetirementDataTable
            End If
            If parameterCalledBy = "IsSavings" Then
                l_CalculatedSavingsDataTable = Session("Calculated_DisplaySavingsPlanAcctContribution_C19")
                l_DataTable_SavingsDataTable = l_CalculatedSavingsDataTable.Clone
                l_SavingsRow = l_CalculatedSavingsDataTable.Select("AccountType='" + MarketAccountType + "'")
                For Each l_DataGridItem In Me.DataGridAccContributionSavings.Items
                    l_CheckBox = l_DataGridItem.FindControl("Selected")
                    l_drCalculatedSavingsDataTable = l_CalculatedSavingsDataTable.Rows(intcnt)
                    intcnt = intcnt + 1
                    If l_drCalculatedSavingsDataTable("AccountType").GetType.ToString <> "System.DBNull" Then
                        If Not ((CType(l_drCalculatedSavingsDataTable("AccountType"), String) = "Total")) Then
                            If l_CheckBox.Checked = True Then
                                l_drCalculatedSavingsDataTable.BeginEdit()
                                l_drCalculatedSavingsDataTable("Selected") = "True"
                                l_drCalculatedSavingsDataTable.EndEdit()
                            Else
                                l_drCalculatedSavingsDataTable.BeginEdit()
                                l_drCalculatedSavingsDataTable("Selected") = "False"
                                l_drCalculatedSavingsDataTable.EndEdit()
                            End If
                        End If
                    End If
                    l_DataTable_SavingsDataTable.ImportRow(l_drCalculatedSavingsDataTable)
                Next
                Session("Calculated_DisplaySavingsPlanAcctContribution_Final_C19") = l_DataTable_SavingsDataTable
            End If
        Catch
            Throw
        End Try

    End Sub

    'Remove this method
    <Serializable()> _
    Public Class SavePropertys

        Private m_FundEventID As String
        Public Property FundEventID() As String
            Get
                Return m_FundEventID
            End Get
            Set(ByVal value As String)
                m_FundEventID = value
            End Set
        End Property

        Private m_RetirementRefundType As String
        Public Property RetirementPlanWithdrawalType() As String
            Get
                Return m_RetirementRefundType
            End Get
            Set(ByVal value As String)
                m_RetirementRefundType = value
            End Set
        End Property

        Private m_SavingsRefundType As String
        Public Property SavingsPlanWithdrawalType() As String
            Get
                Return m_SavingsRefundType
            End Get
            Set(ByVal value As String)
                m_SavingsRefundType = value
            End Set
        End Property

        Private m_RetirementPlanPartialAmount As String
        Public Property RetirementPlanPartialAmount() As String
            Get
                Return m_RetirementPlanPartialAmount
            End Get
            Set(ByVal value As String)
                m_RetirementPlanPartialAmount = value
            End Set
        End Property

        Private m_SavingsPlanPartialAmount As String
        Public Property SavingsPlanPartialAmount() As String
            Get
                Return m_SavingsPlanPartialAmount
            End Get
            Set(ByVal value As String)
                m_SavingsPlanPartialAmount = value
            End Set
        End Property

        Private m_RetirementAcounts As String()
        Public Property SelectedRetirementPlanAccounts() As String()
            Get
                Return m_RetirementAcounts
            End Get
            Set(ByVal value As String())
                m_RetirementAcounts = value
            End Set
        End Property

        Private m_SlectedSavingsAcounts As String()
        Public Property SelectedSavingsPlanAccounts() As String()
            Get
                Return m_SlectedSavingsAcounts
            End Get
            Set(ByVal value As String())
                m_SlectedSavingsAcounts = value
            End Set
        End Property
        'MRD PHASE-II
        Private m_RetirementMRDYear As Integer
        Public Property RetirementMRDYear() As Integer
            Get
                Return m_RetirementMRDYear
            End Get
            Set(ByVal value As Integer)
                m_RetirementMRDYear = value
            End Set
        End Property
        Private m_SavingsMRDYear As Integer
        Public Property SavingsMRDYear() As Integer
            Get
                Return m_SavingsMRDYear
            End Get
            Set(ByVal value As Integer)
                m_SavingsMRDYear = value
            End Set
        End Property
        '  START - CS | 2016.09.23 | YRS-AT-3164 - To Set/Get Check box of IRS Override Hard ship rules ,it is been used in the XML Format
        Private intIRSOverRideHardShipWithdrawal As Integer
        Public Property IRSOverride() As Integer
            Get
                Return intIRSOverRideHardShipWithdrawal
            End Get
            Set(ByVal value As Integer)
                intIRSOverRideHardShipWithdrawal = value
            End Set
        End Property
        'END - CS | 2016.09.23 | YRS-AT-3164 - To  Set/Get Check box of IRS Override Hard ship rules,it is been used in the XML Format
        'Comment code Deleted By Sanjeev on 06/02/2012

        'Start:'Dinesh k               2015.02.15           BT:2804: YRS 5.0-2483:RL_2_FullRefundQDRO Letter not generating from YRS.
        Private strSource As String
        Public Property WithdrawalRequest() As String
            Get
                Return strSource
            End Get
            Set(ByVal value As String)
                strSource = value
            End Set
        End Property

        'START - MMR | 05/05/2020 |YRS-AT-4854 | Declared property for COVID
        Private m_CovidDetails As CovidInformation
        Public Property CovidInformation() As CovidInformation
            Get
                Return m_CovidDetails
            End Get
            Set(ByVal value As CovidInformation)
                m_CovidDetails = value
            End Set
        End Property
        'END - MMR | 05/05/2020 |YRS-AT-4854 | Declared property for COVID

    End Class
    'End: 'Dinesh k               2015.02.15           BT:2804: YRS 5.0-2483:RL_2_FullRefundQDRO Letter not generating from YRS.

    'START - MMR | 05/05/2020 |YRS-AT-4854 | Added class for storing covid details which will be passed ro web method for saving
    <Serializable()> _
    Public Class CovidInformation
        Private decimalcovidTaxableAmount As Decimal
        Public Property CovidTaxableAmount() As Decimal
            Get
                Return decimalcovidTaxableAmount
            End Get
            Set(ByVal value As Decimal)
                decimalcovidTaxableAmount = value
            End Set
        End Property

        Private decimalcovidNonTaxableAmount As Decimal
        Public Property CovidNonTaxableAmount() As Decimal
            Get
                Return decimalcovidNonTaxableAmount
            End Get
            Set(ByVal value As Decimal)
                decimalcovidNonTaxableAmount = value
            End Set
        End Property

        Private decimalTaxWithheld As Decimal
        Public Property TaxWithheld() As Decimal
            Get
                Return decimalTaxWithheld
            End Get
            Set(ByVal value As Decimal)
                decimalTaxWithheld = value
            End Set
        End Property

        Private decimalTaxRate As Decimal
        Public Property BlendedTaxRate() As Decimal
            Get
                Return decimalTaxRate
            End Get
            Set(ByVal value As Decimal)
                decimalTaxRate = value
            End Set
        End Property
    End Class
    'END - MMR | 05/05/2020 |YRS-AT-4854 | Added class for storing covid details which will be passed ro web method for saving

    Private Function SaveArraylistPropertys(ByVal ArraySlectedRetirementAccounts As String(), ByVal ArraySlectedSavingsAccounts As String()) As String
        Try
            Dim objProperty As New SavePropertys
            'objProperty.StrSSNO = Me.SSNNo.Trim()
            objProperty.FundEventID = Me.FundID
            SetFundType("IsRetirement")
            SetFundType("IsSavings")
            objProperty.RetirementPlanWithdrawalType = Me.RetirementRefundType
            objProperty.SavingsPlanWithdrawalType = Me.SavingsRefundType

            objProperty.SelectedRetirementPlanAccounts = ArraySlectedRetirementAccounts
            objProperty.SelectedSavingsPlanAccounts = ArraySlectedSavingsAccounts
            'Bt:961- Taxable amount updating wrongly in atsrefrequestperplan table
            objProperty.RetirementPlanPartialAmount = 0.0
            objProperty.SavingsPlanPartialAmount = 0.0
            If CheckboxPartialRetirement.Checked Then
                objProperty.RetirementPlanPartialAmount = IIf(CType(Me.TextboxPartialRetirement.Text.Trim, String) <> String.Empty, CType(Me.TextboxPartialRetirement.Text.Trim, String), "0.0")
            End If
            If CheckboxPartialSavings.Checked Then
                objProperty.SavingsPlanPartialAmount = IIf(CType(Me.TextboxPartialSavings.Text, String) <> String.Empty, CType(Me.TextboxPartialSavings.Text, String), "0.0")
            End If


            'MRD PHASE -II
            If (Me.RetirementRefundType = "MRD") Then
                objProperty.RetirementMRDYear = Convert.ToInt32(GetMRDYear("RETIREMENT"))
            End If
            If (Me.SavingsRefundType = "MRD") Then
                objProperty.SavingsMRDYear = Convert.ToInt32(GetMRDYear("SAVINGS"))
            End If
            objProperty.IRSOverride = Me.IRSOverrideHardship 'CS | 2016.09.23 | YRS-AT-3164 - To Set Check box  values of IRS Override Hard ship rules ,it is been used in the XML Format

            'START - MMR | 05/05/2020 |YRS-AT-4854 | Set Covid details for saving purpose. This will be an input to web service save method
            Dim covidDetails As New CovidInformation

            covidDetails.CovidTaxableAmount = objRefundRequest.CovidTaxableAmount
            covidDetails.CovidNonTaxableAmount = objRefundRequest.CovidNonTaxableAmount
            covidDetails.BlendedTaxRate = Me.BlendedTaxRate
            covidDetails.TaxWithheld = Me.TotalTaxWithheld

            objProperty.CovidInformation = covidDetails
            'END - MMR | 05/05/2020 |YRS-AT-4854 | Set Covid details for saving purpose. This will be an input to web service save method

            Dim strPropertysXML As String
            strPropertysXML = ConvertToXML(objProperty)
            strPropertysXML = strPropertysXML.Replace("<string>", "<AcctId>")
            strPropertysXML = strPropertysXML.Replace("</string>", "</AcctId>")
            Return strPropertysXML
        Catch
            Throw
        End Try
    End Function


    'Remove
    Public Function ConvertToXML(ByVal objCl As Object) As String
        Dim objXml As New XmlSerializer(objCl.GetType())

        Dim objSW As New StringWriter
        objXml.Serialize(objSW, objCl)

        Dim xmlDoc As New XmlDocument
        xmlDoc.LoadXml(objSW.ToString())

        Dim XmlStr As String = ""
        XmlStr += "<" & xmlDoc.DocumentElement.Name & ">"
        XmlStr += xmlDoc.DocumentElement.InnerXml
        XmlStr += "</" & xmlDoc.DocumentElement.Name & ">"

        Return XmlStr
    End Function
    Private Sub SetFundType(ByVal parameterCalledBy As String)
        Try
            If parameterCalledBy = "IsRetirement" Then
                If (CheckboxRegular.Checked = True And CheckboxPartialRetirement.Checked = False And CheckboxExcludeYMCA.Checked = False And CheckboxVoluntryAccounts.Checked = False) Then
                    Me.RetirementRefundType = "REG"
                ElseIf (CheckboxRegular.Checked = False And CheckboxPartialRetirement.Checked = True And CheckboxExcludeYMCA.Checked = False And CheckboxVoluntryAccounts.Checked = False) Then
                    Me.RetirementRefundType = "PART"
                ElseIf (CheckboxRegular.Checked = False And CheckboxPartialRetirement.Checked = False And CheckboxExcludeYMCA.Checked = True And CheckboxVoluntryAccounts.Checked = False) Then
                    Me.RetirementRefundType = "PERS"
                ElseIf (CheckboxRegular.Checked = False And CheckboxPartialRetirement.Checked = False And CheckboxExcludeYMCA.Checked = False And CheckboxVoluntryAccounts.Checked = True) Then
                    Me.RetirementRefundType = "VOL"
                    'IB:08-Dec-2010 for BT:607 Special/Disability withdrawals should not apply withdrawal rules
                    'SP 2015.01.12 BT-2737 -Added else if to check that if plan have balance then set as true -Start
                ElseIf (CheckboxSpecial.Checked = True And RetirementPlan_TotalAvailableAmount <= 0) Then
                    Me.RetirementRefundType = ""
                    'SP 2015.01.12 BT-2737 -Added else if to check that if plan have balance then set as true -End
                ElseIf (CheckboxSpecial.Checked = True) Then
                    Me.RetirementRefundType = "SPEC"
                    'SP 2015.01.12 BT-2737 -Added else if to check that if plan have balance then set as true -Start
                ElseIf Me.CheckboxDisability.Checked = True And RetirementPlan_TotalAvailableAmount <= 0 Then
                    Me.RetirementRefundType = ""
                    'SP 2015.01.12 BT-2737 -Added else if to check that if plan have balance then set as true -End
                ElseIf (CheckboxDisability.Checked = True) Then
                    Me.RetirementRefundType = "DISAB"
                    'MRD PHASE -II
                ElseIf (CheckboxMRDRetirement.Checked = True) Then
                    Me.RetirementRefundType = "MRD"
                ElseIf (CheckboxMRDRetirementCurrentYear.Checked = True) Then
                    Me.RetirementRefundType = "MRD"
                Else
                    Me.RetirementRefundType = ""
                End If
            End If
            If parameterCalledBy = "IsSavings" Then
                If (CheckboxSavingsVoluntary.Checked = True And CheckboxPartialSavings.Checked = False And CheckboxHardship.Checked = False) Then
                    Me.SavingsRefundType = "VOL"
                ElseIf (CheckboxSavingsVoluntary.Checked = False And CheckboxPartialSavings.Checked = True And CheckboxHardship.Checked = False) Then
                    Me.SavingsRefundType = "PART"
                ElseIf (CheckboxSavingsVoluntary.Checked = False And CheckboxPartialSavings.Checked = False And CheckboxHardship.Checked = True) Then
                    Me.SavingsRefundType = "HARD"
                    Me.IRSOverrideHardship = IIf(CheckboxIRSOverride.Checked And CheckboxIRSOverride.Enabled = True, 1, 0) ''CS | 2016.09.23 | YRS-AT-3164 - To Set Check box  values of IRS Override Hard ship rules 
                    'MRD PHASE -II
                ElseIf (CheckboxMRDSavings.Checked = True) Then
                    Me.SavingsRefundType = "MRD"
                ElseIf (CheckboxMRDSavingsCurrentYear.Checked = True) Then
                    Me.SavingsRefundType = "MRD"
                Else
                    Me.SavingsRefundType = ""
                End If
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Sub ResetGridData()
        Dim RetRows As DataRow()
        Dim cnt As Integer
        Dim SavRows As DataRow()
        If FinalDataSet Is Nothing Then
            Me.FinalDataSet = Session("FinalDataSet_C19")
        End If
        Dim DisplaydataSet As DataSet
        DisplaydataSet = New DataSet
        DisplaydataSet = Me.FinalDataSet.Copy()
        RetRows = CType(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable).Select("Selected=False")
        SavRows = CType(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable).Select("Selected=False")
        If Not DisplaydataSet Is Nothing Then
            Session("Calculated_DisplayRetirementPlanAcctContribution_C19") = DisplaydataSet.Tables("RetirementPlanTable")
            Session("Calculated_DisplaySavingsPlanAcctContribution_C19") = DisplaydataSet.Tables("SavingPlanTable")
        End If
        If RetRows.Length > 0 Then
            For cnt = 0 To RetRows.Length - 1
                CType(Session("Calculated_DisplayRetirementPlanAcctContribution_C19"), DataTable).Select("AccountType='" & RetRows(cnt)("AccountType").ToString() & "'")(0)("Selected") = False
            Next
        End If
        If SavRows.Length > 0 Then
            For cnt = 0 To SavRows.Length - 1
                CType(Session("Calculated_DisplaySavingsPlanAcctContribution_C19"), DataTable).Select("AccountType='" & SavRows(cnt)("AccountType").ToString() & "'")(0)("Selected") = False
            Next
        End If
    End Sub
    '------ Added to implement WebServiece by pavan ---- END    -----------
#End Region
    'MRD PHASE- II
    Private Sub CheckboxMRDRetirement_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxMRDRetirement.CheckedChanged
        Try
            'Comment code Deleted By Sanjeev on 06/02/2012

            ' START : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState
            'Session("MRDRetirementCurrentYear") = False
            Me.IsMRDRetirementCurrentYear = False
            ' END : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState 

            ' START: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)
            Me.IsRMDCheckBoxChecked = False
            Me.RMDLastYrsWarningMessage = Nothing
            If CheckboxMRDRetirement.Checked = False Then
                If CheckboxMRDSavings.Checked Then
                    Me.IsRMDCheckBoxChecked = True
                    Me.RMDSelectedPlanType = "PreviousYearRetirement"
                    ' START: SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client
                    'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Participant also has an RMD due from the Savings Plan. Are you sure you want to exclude the Retirement Plan from this withdrawal?", MessageBoxButtons.YesNo)
                    ShowCustomMessage("Participant also has an RMD due from the Retirement Plan. Are you sure you want to exclude the Retirement Plan from this withdrawal?", MessageBoxButtons.YesNo)
                    ' END: SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client
                End If
            Else
                PreviouseYearRMDCheckAndUncheck("Savings")
            End If

            ' END: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)
            BindMrdGrid("RETIREMENT")
            BindMrdGrid("SAVINGS")
            If CheckboxMRDRetirement.Checked = False Then
                CheckboxMRDRetirementCurrentYear.Checked = False
                CheckboxMRDRetirementCurrentYear.Visible = False
            End If
            DeselelectAllCheckBoxes("RetirementMRD")
            DeselelectAllCheckBoxes("SavingsMRD")
            If CheckboxMRDRetirement.Checked Or CheckboxMRDSavings.Checked Or CheckboxMRDRetirementCurrentYear.Checked Or CheckboxMRDSavingsCurrentYear.Checked Then
                Me.ButtonSave.Visible = True
            End If
            If CheckboxMRDRetirement.Checked Or CheckboxMRDSavings.Checked Or CheckboxMRDRetirementCurrentYear.Checked Or CheckboxMRDSavingsCurrentYear.Checked Then
                EnableDisableCheckboxOtherThanMRD("IsSavings", False)
            Else
                EnableDisableCheckboxOtherThanMRD("IsSavings", True)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxMRDRetirement_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub CheckboxMRDSavings_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxMRDSavings.CheckedChanged
        Try

            ' START: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)
            Me.IsRMDCheckBoxChecked = False
            If CheckboxMRDSavings.Checked = False Then
                If CheckboxMRDRetirement.Checked Then
                    Me.IsRMDCheckBoxChecked = True
                    Me.RMDSelectedPlanType = "PreviousYearSavings"

                    ' START: SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client
                    'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Participant also has an RMD due from the Retirement Plan. Are you sure you want to exclude the Savings Plan from this withdrawal?", MessageBoxButtons.YesNo)
                    ShowCustomMessage("Participant also has an RMD due from the Savings Plan. Are you sure you want to exclude the Savings Plan from this withdrawal?", MessageBoxButtons.YesNo)
                    ' END: SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client
                End If
            Else
                PreviouseYearRMDCheckAndUncheck("Retirement")
            End If

            ' END: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)

            ' START: SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState 
            'Session("MRDSavingsCurrentYear") = False
            Me.IsMRDSavingsCurrentYear = False
            ' END: SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState 

            BindMrdGrid("SAVINGS")
            BindMrdGrid("RETIREMENT")
            CheckboxMRDSavingsCurrentYear.Checked = False
            If CheckboxMRDSavings.Checked = False Then
                CheckboxMRDSavingsCurrentYear.Checked = False
                CheckboxMRDSavingsCurrentYear.Visible = False
            End If
            DeselelectAllCheckBoxes("SavingsMRD")
            DeselelectAllCheckBoxes("RetirementMRD")
            If CheckboxMRDRetirement.Checked Or CheckboxMRDSavings.Checked Then
                Me.ButtonSave.Visible = True
            End If
            If CheckboxMRDRetirement.Checked Or CheckboxMRDSavings.Checked Or CheckboxMRDRetirementCurrentYear.Checked Or CheckboxMRDSavingsCurrentYear.Checked Then
                EnableDisableCheckboxOtherThanMRD("IsRetirement", False)
            Else
                EnableDisableCheckboxOtherThanMRD("IsRetirement", True)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxMRDSavings_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub


    Private Sub EnableDisableCheckboxOtherThanMRD(ByVal parameterCalled As String, ByVal paraflag As Boolean)
        Try
            ' If parameterCalled = "IsRetirement" Then
            If paraflag = False Then
                CheckboxPartialRetirement.Enabled = paraflag
                CheckboxRegular.Enabled = paraflag
                CheckboxSpecial.Enabled = paraflag
                CheckboxVoluntryAccounts.Enabled = paraflag
                CheckboxExcludeYMCA.Enabled = paraflag
                CheckboxPartialSavings.Enabled = paraflag
                CheckboxSavingsVoluntary.Enabled = paraflag
            Else

                If FinalDataSet Is Nothing Then
                    Me.FinalDataSet = Session("FinalDataSet_C19")
                End If

                'Comment code Deleted By Sanjeev on 06/02/2012

                LoadInformationToControls()

            End If

            'Comment code Deleted By Sanjeev on 06/02/2012
        Catch
            Throw
        End Try
    End Sub

    Private Sub BindMrdGrid(ByVal parametercalledBy As String)

        Dim l_MRDYear As String
        Try
            If parametercalledBy = "RETIREMENT" Then
                If CheckboxMRDRetirement.Checked Or CheckboxMRDRetirementCurrentYear.Checked Then
                    If FinalDataSet Is Nothing Then
                        Me.FinalDataSet = Session("FinalDataSet_C19")
                    End If
                    'dropdownlistRetirementMrdYear.Visible = True
                    l_MRDYear = GetMRDYear("RETIREMENT")
                    'BindDataGrid("IsRetirement", FinalDataSet.Tables("MRDRetirement_" + dropdownlistRetirementMrdYear.SelectedItem.Text))
                    BindDataGrid("IsRetirement", FinalDataSet.Tables("MRDRetirement_" + l_MRDYear))
                Else
                    ButtonSave.Visible = False
                    CheckboxMRDRetirementCurrentYear.Checked = False
                    CheckboxMRDRetirementCurrentYear.Visible = False
                End If
            ElseIf parametercalledBy = "SAVINGS" Then
                If CheckboxMRDSavings.Checked Or CheckboxMRDSavingsCurrentYear.Checked Then
                    If FinalDataSet Is Nothing Then
                        Me.FinalDataSet = Session("FinalDataSet_C19")
                    End If

                    l_MRDYear = GetMRDYear("SAVINGS")
                    BindDataGrid("IsSavings", FinalDataSet.Tables("MRDSavings_" + l_MRDYear))
                Else
                    ButtonSave.Visible = False
                    CheckboxMRDSavingsCurrentYear.Checked = False
                    CheckboxMRDSavingsCurrentYear.Visible = False
                End If
            End If
            BindMrdGrandTotalGrid()

        Catch
            Throw
        End Try



    End Sub
    Private Sub BindMrdGrandTotalGrid()

        Dim l_dt_GrandTotalDatatable As New DataTable
        Dim l_datatable As DataTable
        Dim drow As DataRow
        Dim l_dt_RetPlanCalculatedDataTable As DataTable
        Dim l_dt_SavingsPlanCalculatedDataTable As DataTable
        Dim MarketAccountType As String
        Dim l_RetRow As DataRow()
        Dim l_SavingsRow As DataRow()
        Dim l_Taxable As Decimal
        Dim l_Non_Taxable As Decimal
        Dim l_Interest As Decimal
        Dim l_TaxWithHeld As Decimal
        Dim l_Total As Decimal
        Dim objItem As DataGridItem
        Try


            If CheckboxMRDRetirement.Checked Or CheckboxMRDRetirementCurrentYear.Checked Then
                For Each objItem In DataGridAccContributionRetirement.Items
                    If objItem.ItemType <> ListItemType.Header And _
                     objItem.ItemType <> ListItemType.Footer And _
                   objItem.ItemType <> ListItemType.Pager And objItem.Cells(2).Text = "Total" Then
                        l_Taxable = IIf(objItem.Cells(5).Text = "", 0, Convert.ToDecimal(objItem.Cells(5).Text)) + IIf(objItem.Cells(7).Text = "", 0, Convert.ToDecimal(objItem.Cells(7).Text))
                        l_Non_Taxable = IIf(objItem.Cells(6).Text = "", 0, Convert.ToDecimal(objItem.Cells(6).Text))
                        l_TaxWithHeld = IIf(objItem.Cells(7).Text = "", 0, Convert.ToDecimal(objItem.Cells(8).Text))
                        l_Total = IIf(objItem.Cells(9).Text = 0, 0, Convert.ToDecimal(objItem.Cells(9).Text))
                    End If
                Next
            End If
            If CheckboxMRDSavings.Checked Or CheckboxMRDSavingsCurrentYear.Checked Then
                For Each objItem In DataGridAccContributionSavings.Items
                    If objItem.ItemType <> ListItemType.Header And _
                     objItem.ItemType <> ListItemType.Footer And _
                   objItem.ItemType <> ListItemType.Pager And objItem.Cells(2).Text = "Total" Then
                        l_Taxable += IIf(objItem.Cells(5).Text = "", 0, Convert.ToDecimal(objItem.Cells(5).Text)) + IIf(objItem.Cells(7).Text = "", 0, Convert.ToDecimal(objItem.Cells(7).Text))
                        l_Non_Taxable += IIf(objItem.Cells(6).Text = "", 0, Convert.ToDecimal(objItem.Cells(6).Text))
                        l_TaxWithHeld += IIf(objItem.Cells(7).Text = "", 0, Convert.ToDecimal(objItem.Cells(8).Text))
                        l_Total += IIf(objItem.Cells(9).Text = 0, 0, Convert.ToDecimal(objItem.Cells(9).Text))
                    End If
                Next
            End If

            If Not Session("CalculatedDataTableDisplay_C19") Is Nothing Then
                l_datatable = DirectCast(Session("CalculatedDataTableDisplay_C19"), DataTable)
                If l_datatable.Rows.Count > 0 Then
                    l_dt_GrandTotalDatatable = l_datatable.Clone
                    drow = l_dt_GrandTotalDatatable.NewRow()
                    drow("AccountType") = "Total"
                    drow("Taxable") = l_Taxable
                    drow("Non-Taxable") = l_Non_Taxable
                    drow("Interest") = l_TaxWithHeld
                    'BT 725 :Net Amout shows wrong for MRD request in Withdrawal Request screen.
                    drow("Total") = l_Total - l_TaxWithHeld
                    l_dt_GrandTotalDatatable.Rows.Add(drow)
                End If
            End If
            Me.DatagridGrandTotal.Columns(6).HeaderText = "Tax Withheld"
            'Added By Parveen for Design Changes For Market Based Withdrawal on 13/Nov/2009
            l_dt_GrandTotalDatatable.AcceptChanges()
            '  If CheckboxMRDRetirement.Checked Or CheckboxMRDSavings.Checked Then
            Me.DatagridGrandTotal.DataSource = l_dt_GrandTotalDatatable
            Me.DatagridGrandTotal.DataBind()
            Me.SetSelectedIndex(DatagridGrandTotal, l_dt_GrandTotalDatatable)
            '  End If


        Catch
            Throw
        End Try

    End Sub

    Private Sub CheckboxMRDRetirementCurrentYear_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxMRDRetirementCurrentYear.CheckedChanged
        Try
            ' START: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)
            Dim confirmationMessage As String

            Me.IsRMDCheckBoxChecked = False
            Me.RMDCurrentYrsWarningMessage = Nothing

            ' START : SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client
            'confirmationMessage = "Participant also has a current year RMD due from the Savings Plan. Are you sure you want to exclude Retirement Plan from this withdrawal?"
            confirmationMessage = "Participant also has a current year RMD due from the Retirement Plan. Are you sure you want to exclude Retirement Plan from this withdrawal?"
            ' END : SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client

            If Not CheckboxMRDSavingsCurrentYear.Visible And Not CheckboxMRDRetirementCurrentYear.Checked And CheckboxMRDRetirement.Checked And CheckboxMRDSavings.Checked Then
                '   If CheckboxMRDSavingsCurrentYear.Checked Then
                Me.IsRMDCheckBoxChecked = True
                Me.RMDSelectedPlanType = "CurrentYearRetirement"

                ' START: SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client
                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", confirmationMessage, MessageBoxButtons.YesNo)
                ShowCustomMessage(confirmationMessage, MessageBoxButtons.YesNo)
                ' END: SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client

                'End If
            ElseIf Not CheckboxMRDRetirementCurrentYear.Checked Then
                If CheckboxMRDSavingsCurrentYear.Checked Then
                    Me.IsRMDCheckBoxChecked = True
                    Me.RMDSelectedPlanType = "CurrentYearRetirement"

                    ' START : SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client
                    'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", confirmationMessage, MessageBoxButtons.YesNo)
                    ShowCustomMessage(confirmationMessage, MessageBoxButtons.YesNo)
                    ' END : SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client

                End If
            Else
                IncludeCurrentYearRMDCheckBox("Savings")
            End If
            ' END: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)
            'Comment code Deleted By Sanjeev on 06/02/2012

            ' START : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState 
            'Session("MRDRetirementCurrentYear") = CheckboxMRDRetirementCurrentYear.Checked
            Me.IsMRDRetirementCurrentYear = CheckboxMRDRetirementCurrentYear.Checked
            ' END : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState 

            BindMrdGrid("RETIREMENT")
            BindMrdGrid("SAVINGS")
            '  CheckboxMRDRetirement.Checked = True  CS | 2016.10.12 | YRS-AT-3073 | Logic to mark it as checked is moved to function PreviouseYearRMDCheckAndUncheck()
            DeselelectAllCheckBoxes("RetirementMRD")
            DeselelectAllCheckBoxes("SavingsMRD")
            If CheckboxMRDRetirement.Checked Or CheckboxMRDSavings.Checked Or CheckboxMRDRetirementCurrentYear.Checked Or CheckboxMRDSavingsCurrentYear.Checked Then
                Me.ButtonSave.Visible = True
            End If
            If CheckboxMRDRetirement.Checked Or CheckboxMRDSavings.Checked Or CheckboxMRDRetirementCurrentYear.Checked Or CheckboxMRDSavingsCurrentYear.Checked Then
                EnableDisableCheckboxOtherThanMRD("IsSavings", False)
            Else
                EnableDisableCheckboxOtherThanMRD("IsSavings", True)
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxMRDRetirementCurrentYear_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    Private Sub CheckboxMRDSavingsCurrentYear_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxMRDSavingsCurrentYear.CheckedChanged
        Try
            Dim confirmationMessage As String
            ' START: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)
            Me.IsRMDCheckBoxChecked = False
            Me.RMDCurrentYrsWarningMessage = Nothing

            ' START: SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client
            'confirmationMessage = "Participant also has a current year RMD due from the Retirement Plan. Are you sure you want to exclude Savings Plan from this withdrawal?"
            confirmationMessage = "Participant also has a current year RMD due from the Savings Plan. Are you sure you want to exclude Savings Plan from this withdrawal?"
            ' END: SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client

            If Not CheckboxMRDRetirementCurrentYear.Visible And Not CheckboxMRDSavingsCurrentYear.Checked And CheckboxMRDRetirement.Checked And CheckboxMRDSavings.Checked Then
                Me.IsRMDCheckBoxChecked = True

                ' START: SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client
                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", confirmationMessage, MessageBoxButtons.YesNo)
                ShowCustomMessage(confirmationMessage, MessageBoxButtons.YesNo)
                ' END: SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client

                Me.RMDSelectedPlanType = "CurrentYearSavings"
            ElseIf Not CheckboxMRDSavingsCurrentYear.Checked Then
                If CheckboxMRDRetirementCurrentYear.Checked Then

                    ' START: SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client
                    'MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", confirmationMessage, MessageBoxButtons.YesNo)
                    ShowCustomMessage(confirmationMessage, MessageBoxButtons.YesNo)
                    ' END: SB | 2016.12.13 | YRS-AT-3073 | Changing the message text as per observation reported by client

                    Me.IsRMDCheckBoxChecked = True
                    Me.RMDSelectedPlanType = "CurrentYearSavings"
                End If
            Else
                IncludeCurrentYearRMDCheckBox("Retirement")
            End If
            ' END: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)

            ' START : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState 
            'Session("MRDSavingsCurrentYear") = CheckboxMRDSavingsCurrentYear.Checked 'True
            Me.IsMRDSavingsCurrentYear = CheckboxMRDSavingsCurrentYear.Checked
            ' END : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState

            BindMrdGrid("SAVINGS")
            BindMrdGrid("RETIREMENT")
            'CheckboxMRDSavings.Checked = True  CS | 2016.10.12 | YRS-AT-3073 |  Logic to mark it as checked is moved to function PreviouseYearRMDCheckAndUncheck()
            DeselelectAllCheckBoxes("SavingsMRD")
            DeselelectAllCheckBoxes("RetirementMRD")
            If CheckboxMRDRetirement.Checked Or CheckboxMRDSavings.Checked Or CheckboxMRDRetirementCurrentYear.Checked Or CheckboxMRDSavingsCurrentYear.Checked Then
                Me.ButtonSave.Visible = True
            End If
            If CheckboxMRDRetirement.Checked Or CheckboxMRDSavings.Checked Or CheckboxMRDRetirementCurrentYear.Checked Or CheckboxMRDSavingsCurrentYear.Checked Then
                EnableDisableCheckboxOtherThanMRD("IsRetirement", False)
            Else
                EnableDisableCheckboxOtherThanMRD("IsRetirement", True)
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-CheckboxMRDSavingsCurrentYear_CheckedChanged", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub

    'START: SG: 2012.06.14: BT-1043
    Private Sub DoBtnSaveProcessing()
        Dim l_SaveStatus As Boolean

        l_SaveStatus = Me.SaveRefund()

        ' This satement for Refresh the DataGrid in Parent form.
        If l_SaveStatus = True Then
            Me.SessionIsRefundRequest = True

            'Added the Condition to close the page if the save buton is clicked more then one time-Amit -10-09-2009
            Response.Write("<script> function process(){window.opener.document.forms(0).submit(); self.close();}process();</script>")
            objRefundRequest.IsSaveProcessComplete = True
        End If
    End Sub

    Private Sub CheckeckPlanType()
        If Me.CheckboxSavingsVoluntary.Checked = True Or Me.CheckboxMRDSavings.Checked = True Or Me.CheckboxPartialSavings.Checked = True Or Me.CheckboxHardship.Checked = True Then
            Me.CheckboxSavingsPlan.Checked = True
        Else
            Me.CheckboxSavingsPlan.Checked = False
        End If

        If Me.CheckboxRegular.Checked = True Or Me.CheckboxSpecial.Checked = True Or Me.CheckboxDisability.Checked = True Or Me.CheckboxMRDRetirement.Checked = True Or Me.CheckboxExcludeYMCA.Checked = True Or Me.CheckboxVoluntryAccounts.Checked = True Or Me.CheckboxPartialRetirement.Checked = True Then
            Me.CheckboxRetirementPlan.Checked = True
        Else
            Me.CheckboxRetirementPlan.Checked = False
        End If
    End Sub
    'END: SG: 2012.06.14: BT-1043
    'Start-SR:2014.01.06 : BT 2240/YRS 5.0-2226:modification to partial withdrawal processing
    Private Sub DoNotForceSavingPlanWithdrawal(ByVal dblRequestedRetirementPlanAmount As Decimal)
        Try
            If (Me.RetirementPlanTotalAmount - dblRequestedRetirementPlanAmount > Me.MinimumPIAToRetire) And (Me.SavingsPlan_TotalAvailableAmount > 0) Then
                Me.CompulsorySavings = False
                CheckboxSavingsVoluntary.Enabled = True
            ElseIf (Me.RetirementPlanTotalAmount - dblRequestedRetirementPlanAmount < Me.MinimumPIAToRetire) And (Me.SavingsPlan_TotalAvailableAmount < Me.MinimumPIAToRetire) Then
                Me.CompulsorySavings = True
                CheckboxSavingsVoluntary.Checked = True
                CheckboxSavingsVoluntary.Enabled = False
                'SR:2013.03.21 : BT2479/YRS 5.0-2226 - YRS 5.0-2226:modification to partial withdrawal processing(Re-work)
                Me.Bool_NotIncludeTMMatched = False
                SetGlobalFlags("BOTH")
                CheckboxSavingsPlan.Checked = True
                Me.NumPercentageFactorofMoneySavings = 1
                ControlAllcheckBoxes("VolSav", CheckboxSavingsVoluntary.Checked, "S")
                'SR:2013.03.21:BT2479/YRS 5.0-2226-YRS 5.0-2226:modification to partial withdrawal processing(Re-work)
            End If
        Catch
            Throw
        End Try
    End Sub
    'End-SR:2014.01.06 : BT 2240/YRS 5.0-2226:modification to partial withdrawal processing
    ' SR | 2016.06.08 | YRS-AT-2962 | If Processing fee applicable then display message
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            DivMainMessage.InnerHtml = ""
            DivMainMessage.Visible = False
            ' check for Retirement plan Processing message
            If CheckboxPartialRetirement.Checked Then
                If (Convert.ToDecimal(RetirementPlanProcessingFee) > 0) Then
                    DivMainMessage.Visible = True
                    If (DivMainMessage.InnerHtml = "") Then
                        DivMainMessage.InnerHtml = Trim(RetirementProcessingFeeMessage) + "<br/>"
                    End If
                End If
            End If
            ' check for Savings plan Processing message
            If CheckboxPartialSavings.Checked Then
                If (Convert.ToDecimal(SavingsPlanProcessingFee) > 0) Then
                    DivMainMessage.Visible = True
                    If (DivMainMessage.InnerHtml = "") Then
                        DivMainMessage.InnerHtml = SavingsProcessingFeeMessage + "<br/>"
                    Else
                        DivMainMessage.InnerHtml += SavingsProcessingFeeMessage + "<br/>"
                    End If
                End If
            End If
            ' START: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)
            DisplayRMDWarningMessages()
            ' END: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            HelperFunctions.LogException("RefundRequestWebForm_C19-Page_PreRender", ex)
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
        ' SR | 2016.06.08 | YRS-AT-2962 | If Processing fee applicable then display message
    End Sub

    'START Chandra sekar | 2016.09.23 | YRS-AT-3164 - Support Request:special Hardship Withdrawal - Setting Access Right to Controls
    Public Sub getSecuredControls()
        Try
            CheckboxIRSOverride.Enabled = False
            Dim checkSecurity As String = SecurityCheck.Check_Authorization("ChkIRSOverride", Convert.ToInt32(Session("LoggedUserKey")), True)
            If checkSecurity.Equals("True") Then
                CheckboxIRSOverride.Enabled = True
            End If
        Catch
            Throw
        End Try
        'END Chandra sekar | 2016.09.23 | YRS-AT-3164 - - Support Request:special Hardship Withdrawal - Setting Access Right to Controls
    End Sub

    ' START: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)
    Private Sub CheckAndUnCheckRMDCheckbox(ByVal planType As String)
        Dim warningMessage As String = "Participant also has an RMD due from the {0} plan, hence it has been selected by default for year {1}."
        DivMainMessage.Visible = False

        Dim retirementMRDYear As Integer
        Dim savingsMRDYear As Integer

        ' START : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState with default valueas False
        'Session("MRDSavingsCurrentYear") = Nothing
        'Session("MRDRetirementCurrentYear") = Nothing
        Me.IsMRDSavingsCurrentYear = False
        Me.IsMRDRetirementCurrentYear = False
        ' END : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState to check given condition

        retirementMRDYear = Convert.ToInt32(GetMRDYear("RETIREMENT"))
        savingsMRDYear = Convert.ToInt32(GetMRDYear("SAVINGS"))
        'If user select the RMD withdrawal for the Retirement Plan, then savings RMD Checkbox is automatically get selected for withdrawal  
        If CheckboxMRDRetirement.Checked And CheckboxMRDSavings.Enabled And CheckboxMRDRetirement.Enabled And CheckboxMRDSavings.Visible And Not CheckboxMRDSavings.Checked Then
            CheckboxMRDSavings.Checked = True
            FetchMRDYear("SAVINGS")
            If retirementMRDYear > savingsMRDYear Then
                If CheckboxMRDSavingsCurrentYear.Visible And Not CheckboxMRDRetirementCurrentYear.Visible Then
                    CheckboxMRDSavingsCurrentYear.Checked = True

                    ' START : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState
                    'Session("MRDSavingsCurrentYear") = CheckboxMRDSavingsCurrentYear.Checked
                    Me.IsMRDSavingsCurrentYear = CheckboxMRDSavingsCurrentYear.Checked
                    ' END : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState

                    savingsMRDYear = Convert.ToInt32(GetMRDYear("SAVINGS"))
                    Me.RMDPreviousYearSavings = Me.RMDPreviousYearSavings & "," & savingsMRDYear
                    planType = "Savings"
                End If
            End If
            Me.RMDLastYrsWarningMessage = String.Format(warningMessage, planType, Me.RMDPreviousYearSavings)
            'If user select the RMD withdrawal for the Saving Plan, then Retirement RMD Checkbox is automatically get selected for withdrawal  
        ElseIf CheckboxMRDSavings.Checked And CheckboxMRDRetirement.Visible And CheckboxMRDSavings.Enabled And CheckboxMRDRetirement.Enabled And Not CheckboxMRDRetirement.Checked Then

            CheckboxMRDRetirement.Checked = True
            FetchMRDYear("RETIREMENT")
            If savingsMRDYear > retirementMRDYear Then
                If Not CheckboxMRDSavingsCurrentYear.Visible And CheckboxMRDRetirementCurrentYear.Visible Then
                    CheckboxMRDRetirementCurrentYear.Checked = True

                    ' START: SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState 
                    'Session("MRDRetirementCurrentYear") = CheckboxMRDRetirementCurrentYear.Checked
                    Me.IsMRDRetirementCurrentYear = CheckboxMRDRetirementCurrentYear.Checked
                    ' END : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState to check given condition

                    retirementMRDYear = Convert.ToInt32(GetMRDYear("RETIREMENT"))
                    Me.RMDPreviousYearRetirement = Me.RMDPreviousYearRetirement & "," & retirementMRDYear
                    planType = "Retirement"
                End If
            End If
            Me.RMDLastYrsWarningMessage = String.Format(warningMessage, planType, Me.RMDPreviousYearRetirement)
            'If user select the RMD withdrawal for the Retirement Plan, then savings included year Checkbox is automatically get selected for withdrawal, if Retirement RMD is greater than Savings RMD 
        ElseIf CheckboxMRDRetirement.Checked And CheckboxMRDSavings.Visible And CheckboxMRDRetirement.Visible And CheckboxMRDSavings.Checked And Not CheckboxMRDRetirementCurrentYear.Visible Then
            If retirementMRDYear > savingsMRDYear Then
                If CheckboxMRDSavingsCurrentYear.Visible And Not CheckboxMRDRetirementCurrentYear.Visible Then
                    FetchMRDYear("SAVINGS")
                    CheckboxMRDSavingsCurrentYear.Checked = True

                    ' START : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState
                    'Session("MRDSavingsCurrentYear") = CheckboxMRDSavingsCurrentYear.Checked
                    Me.IsMRDSavingsCurrentYear = CheckboxMRDSavingsCurrentYear.Checked
                    ' END : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState

                    savingsMRDYear = Convert.ToInt32(GetMRDYear("SAVINGS"))
                    Me.RMDPreviousYearSavings = Me.RMDPreviousYearSavings & "," & savingsMRDYear
                    Me.RMDLastYrsWarningMessage = String.Format(warningMessage, "Savings", Me.RMDPreviousYearSavings)
                End If
            End If
            'If user select the RMD withdrawal for the Savings Plan, then Retirement included year Checkbox is automatically get selected for withdrawal, if Savings Plan RMD is greater than Retirement Plan RMD 
        ElseIf CheckboxMRDSavings.Checked And CheckboxMRDSavings.Visible And CheckboxMRDRetirement.Visible And CheckboxMRDRetirement.Checked And Not CheckboxMRDSavingsCurrentYear.Visible Then
            If savingsMRDYear > retirementMRDYear Then
                If Not CheckboxMRDSavingsCurrentYear.Visible And CheckboxMRDRetirementCurrentYear.Visible Then
                    FetchMRDYear("RETIREMENT")
                    CheckboxMRDRetirementCurrentYear.Checked = True

                    ' START : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState
                    'Session("MRDRetirementCurrentYear") = CheckboxMRDRetirementCurrentYear.Checked
                    Me.IsMRDRetirementCurrentYear = CheckboxMRDRetirementCurrentYear.Checked
                    ' END : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState to check given condition

                    retirementMRDYear = Convert.ToInt32(GetMRDYear("RETIREMENT"))
                    Me.RMDPreviousYearRetirement = Me.RMDPreviousYearRetirement & "," & retirementMRDYear
                    Me.RMDLastYrsWarningMessage = String.Format(warningMessage, "Retirement", Me.RMDPreviousYearRetirement)
                End If
            End If
        End If

    End Sub

    Private Sub CheckAndUnCheckIncludeCurrentRMDCheckbox(ByVal planType As String)
        Dim currentWarningMessage As String = "Participant also has a current year RMD due from the {0} plan, hence it has been selected by default for year {1}."

        'If user select the Include current year  RMD withdrawal for the Retirement Plan, then Include current year (savings) RMD Checkbox is automatically get selected for withdrawal  
        If CheckboxMRDRetirementCurrentYear.Checked And CheckboxMRDSavingsCurrentYear.Visible And Not CheckboxMRDSavingsCurrentYear.Checked And CheckboxMRDRetirement.Enabled Then
            CheckboxMRDSavingsCurrentYear.Checked = True

            ' START : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState
            'Session("MRDSavingsCurrentYear") = CheckboxMRDSavingsCurrentYear.Checked
            Me.IsMRDSavingsCurrentYear = CheckboxMRDSavingsCurrentYear.Checked
            ' END : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState

            Me.RMDCurrentYrsWarningMessage = String.Format(currentWarningMessage, planType, Me.RMDCurrentYearRetirement)
            'If user select the Include current year  RMD withdrawal for the Savings Plan, then Include current year (Retirement) RMD Checkbox is automatically get selected for withdrawal  
        ElseIf CheckboxMRDSavingsCurrentYear.Checked And CheckboxMRDRetirementCurrentYear.Visible And Not CheckboxMRDRetirementCurrentYear.Checked And CheckboxMRDRetirement.Enabled Then
            CheckboxMRDRetirementCurrentYear.Checked = True

            ' START : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState
            'Session("MRDRetirementCurrentYear") = CheckboxMRDRetirementCurrentYear.Checked
            Me.IsMRDRetirementCurrentYear = CheckboxMRDRetirementCurrentYear.Checked
            ' END : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState

            Me.RMDCurrentYrsWarningMessage = String.Format(currentWarningMessage, planType, Me.RMDCurrentYearSavings)
        End If
    End Sub

    'If user then unchecks one of RMD Check box (Retirement/Savings)
    Private Sub DisplayRMDWarningMessages()
        Try
            'DivMainMessage.InnerHtml = "" -- PPP | 11/21/2016 | YRS-AT-3146 | If processing fee message is set to display it on screen then it was resetting it to empty.. RMD and Process fee will not co-exists so commenting this line will work.
            If Not CheckboxMRDRetirementCurrentYear.Checked Or Not CheckboxMRDSavingsCurrentYear.Checked Then
                Me.RMDCurrentYrsWarningMessage = Nothing
            End If
            If Not CheckboxMRDRetirement.Checked Or Not CheckboxMRDSavings.Checked Then
                Me.RMDLastYrsWarningMessage = Nothing
            End If
            If (DivMainMessage.InnerHtml = "") And Not String.IsNullOrEmpty(Me.RMDLastYrsWarningMessage) And Not String.IsNullOrEmpty(Me.RMDCurrentYrsWarningMessage) Then
                DivMainMessage.Visible = True
                DivMainMessage.InnerHtml = Me.RMDLastYrsWarningMessage + "<br/>" + Me.RMDCurrentYrsWarningMessage
            ElseIf (DivMainMessage.InnerHtml = "") And Not String.IsNullOrEmpty(Me.RMDLastYrsWarningMessage) Then
                DivMainMessage.Visible = True
                DivMainMessage.InnerHtml = Me.RMDLastYrsWarningMessage
            ElseIf (DivMainMessage.InnerHtml = "") And Not String.IsNullOrEmpty(Me.RMDCurrentYrsWarningMessage) Then
                DivMainMessage.Visible = True
                DivMainMessage.InnerHtml = Me.RMDCurrentYrsWarningMessage
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Sub PreviouseYearRMDCheckAndUncheck(ByVal planType As String)
        Dim retirementMRDYear As Integer
        Dim savingsMRDYear As Integer
        If CheckboxMRDSavings.Enabled And CheckboxMRDRetirement.Enabled Then
            retirementMRDYear = Convert.ToInt32(GetMRDYear("RETIREMENT"))
            savingsMRDYear = Convert.ToInt32(GetMRDYear("SAVINGS"))
            If retirementMRDYear >= savingsMRDYear And planType = "Savings" Then
                CheckAndUnCheckRMDCheckbox(planType)
            ElseIf savingsMRDYear >= retirementMRDYear And planType = "Retirement" Then
                CheckAndUnCheckRMDCheckbox(planType)
            End If
        End If
    End Sub

    Private Sub IncludeCurrentYearRMDCheckBox(ByVal planType As String)
        Dim retirementMRDYear As Integer
        Dim savingsMRDYear As Integer
        If CheckboxMRDSavings.Enabled And CheckboxMRDRetirement.Enabled Then
            retirementMRDYear = Convert.ToInt32(GetMRDYear("RETIREMENT"))
            savingsMRDYear = Convert.ToInt32(GetMRDYear("SAVINGS"))
            If planType = "Savings" Then
                If CheckboxMRDRetirementCurrentYear.Visible And Not CheckboxMRDSavingsCurrentYear.Visible Then

                    ' START : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState
                    'Session("MRDRetirementCurrentYear") = True
                    Me.IsMRDRetirementCurrentYear = True
                    ' END : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState

                    retirementMRDYear = Convert.ToInt32(GetMRDYear("RETIREMENT"))
                    savingsMRDYear = Convert.ToInt32(GetMRDYear("SAVINGS"))
                End If
                If savingsMRDYear >= retirementMRDYear Then
                    CheckAndUnCheckRMDCheckbox(planType)
                    CheckAndUnCheckIncludeCurrentRMDCheckbox(planType)
                End If
            ElseIf planType = "Retirement" Then
                If Not CheckboxMRDRetirementCurrentYear.Visible And CheckboxMRDSavingsCurrentYear.Visible Then
                    retirementMRDYear = Convert.ToInt32(GetMRDYear("RETIREMENT"))

                    ' START : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState
                    'Session("MRDSavingsCurrentYear") = True
                    Me.IsMRDSavingsCurrentYear = True
                    ' END : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState

                    savingsMRDYear = Convert.ToInt32(GetMRDYear("SAVINGS"))
                End If
                If retirementMRDYear >= savingsMRDYear Then
                    CheckAndUnCheckRMDCheckbox(planType)
                    CheckAndUnCheckIncludeCurrentRMDCheckbox(planType)
                End If
            End If
        End If
    End Sub

    ' Fetching RMD Year for appending in the warning messages
    Private Sub FetchMRDYear(ByVal PlanType As String)
        Dim retirementRow As DataRow()
        Dim savingsRow As DataRow()
        Dim previousYearsRetirement As New StringBuilder
        Dim previousYearSavings As New StringBuilder
        Try
            If FinalDataSet Is Nothing Then
                Me.FinalDataSet = Session("FinalDataSet_C19")
            End If
            If HelperFunctions.isNonEmpty(Me.FinalDataSet) Then
                If PlanType = "RETIREMENT" Then
                    retirementRow = FinalDataSet.Tables("MRDRecords").Select("PlanType='RETIREMENT'")
                    If retirementRow.Length > 0 Then
                        If retirementRow.Length > 1 Then
                            For intRetcount As Integer = 0 To retirementRow.Length - 2
                                If Not retirementRow(intRetcount)("MRDYear").ToString() Is Nothing Then
                                    previousYearsRetirement.Append(retirementRow(intRetcount)("MRDYear").ToString())
                                    If intRetcount <> retirementRow.Length - 2 Then
                                        previousYearsRetirement.Append(", ")
                                    End If
                                End If
                            Next
                        Else
                            previousYearsRetirement.Append(retirementRow(0)("MRDYear").ToString())
                        End If
                        Me.RMDPreviousYearRetirement = previousYearsRetirement.ToString()
                    End If
                ElseIf PlanType = "SAVINGS" Then
                    savingsRow = FinalDataSet.Tables("MRDRecords").Select("PlanType='SAVINGS'")
                    If savingsRow.Length > 0 Then
                        If savingsRow.Length > 1 Then
                            For intSavcount As Integer = 0 To savingsRow.Length - 2
                                If Not savingsRow(intSavcount)("MRDYear").ToString() Is Nothing Then
                                    previousYearSavings.Append(savingsRow(intSavcount)("MRDYear").ToString())
                                    If intSavcount <> savingsRow.Length - 2 Then
                                        previousYearSavings.Append(", ")
                                    End If
                                End If
                            Next
                        Else
                            previousYearSavings.Append(savingsRow(0)("MRDYear").ToString())
                        End If
                        Me.RMDPreviousYearSavings = previousYearSavings.ToString()
                    End If
                End If
            End If
        Catch
            Throw
        End Try
    End Sub

    Private Sub SetCheckboxNoWarningMessage()
        Dim currentWarningMessage As String = "Participant also has a current year RMD due from the {0} plan, hence it has been selected by default for year {1}."
        Dim warningMessage As String = "Participant also has an RMD due from the {0} plan, hence it has been selected by default for year {1}."

        Select Case Me.RMDSelectedPlanType
            Case "PreviousYearRetirement"
                CheckboxMRDRetirement.Checked = True
                FetchMRDYear("SAVINGS")
                GetMRDYear("RETIREMENT")
                GetMRDYear("SAVINGS")
                If CheckboxMRDSavingsCurrentYear.Visible And Not CheckboxMRDRetirementCurrentYear.Visible Then
                    CheckboxMRDSavingsCurrentYear.Checked = True
                    Me.RMDPreviousYearSavings = Me.RMDPreviousYearSavings & "," & Me.RMDCurrentYearSavings
                ElseIf Not CheckboxMRDSavingsCurrentYear.Visible And CheckboxMRDRetirementCurrentYear.Visible Then
                    CheckboxMRDRetirementCurrentYear.Checked = True
                End If
                Me.RMDLastYrsWarningMessage = String.Format(warningMessage, "Savings", Me.RMDPreviousYearSavings)
            Case "PreviousYearSavings"
                CheckboxMRDSavings.Checked = True
                FetchMRDYear("RETIREMENT")
                GetMRDYear("RETIREMENT")
                GetMRDYear("SAVINGS")
                If CheckboxMRDSavingsCurrentYear.Visible And Not CheckboxMRDRetirementCurrentYear.Visible Then
                    CheckboxMRDSavingsCurrentYear.Checked = True
                ElseIf Not CheckboxMRDSavingsCurrentYear.Visible And CheckboxMRDRetirementCurrentYear.Visible Then
                    CheckboxMRDRetirementCurrentYear.Checked = True
                    Me.RMDPreviousYearRetirement = Me.RMDPreviousYearRetirement & "," & Me.RMDCurrentYearRetirement
                End If
                Me.RMDLastYrsWarningMessage = String.Format(warningMessage, "Retirement", Me.RMDPreviousYearRetirement)
            Case "CurrentYearRetirement"
                CheckboxMRDRetirementCurrentYear.Checked = True

                ' START : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState
                'Session("MRDRetirementCurrentYear") = True 'SR | 2016.12.15 | YRS-AT-3073 | Set CurrentYearRMD as true if user select to remain current Year RMD checkbox  checked.
                Me.IsMRDRetirementCurrentYear = True
                ' END : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState
                Me.RMDCurrentYrsWarningMessage = String.Format(currentWarningMessage, "Savings", Me.RMDCurrentYearRetirement)
            Case "CurrentYearSavings"
                CheckboxMRDSavingsCurrentYear.Checked = True

                ' START : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState
                'Session("MRDSavingsCurrentYear") = True 'SR | 2016.12.15 | YRS-AT-3073 | Set CurrentYearRMD as true if user select to remain current Year RMD checkbox  checked.
                Me.IsMRDSavingsCurrentYear = True
                ' END : SB | 2016.12.16 | YRS-AT-3073 | Replacing Session variable by ViewState

                Me.RMDCurrentYrsWarningMessage = String.Format(currentWarningMessage, "Retirement", Me.RMDCurrentYearSavings)
        End Select

    End Sub
    ' END: CS | 2016.10.12 | YRS-AT-3073 | YRS enh- Default RMD withdrawal requests to both plans (TrackIT 26866)

    ' START: SB | 2016.12.13 | YRS-AT-3073 | Creating a common sub routine to display messagebox on screen
    Private Sub ShowCustomMessage(ByVal message As String, ByVal msgBoxButtons As MessageBoxButtons)
        MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", message, msgBoxButtons)
    End Sub
    ' END: SB | 2016.12.13 | YRS-AT-3073 | Creating a common sub routine to display messagebox on screen

    'START: JT | 2018.08.27 | YRS-AT-4058 | To Get messeage from resource file
    Public Function GetMessage(ByVal resourceMessage As String)
        Dim message As String
        Try
            Try
                message = GetGlobalResourceObject("Withdrawal", resourceMessage).ToString()
            Catch ex As Exception
                message = resourceMessage
            End Try
            Return message
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'END: JT | 2018.08.27 | YRS-AT-4058 | To Get messeage from resource file

    'START - MMR | 04/20/2020 |YRS-AT-4854 | Method to get blended tax rate to apply covid benefit rules
    ''' <summary>
    ''' Returns Blended tax rate for tax calculation (Retirement/Savings)
    ''' </summary>
    ''' <param name="retirementTable">Retirement Table/param>
    ''' <param name="savingsTable">Savings Table</param>
    ''' <param name="planAccountSelectionChange">Selected Plan Account (Retirement/Savings)</param>
    ''' <returns>Blended Tax Rate</returns>
    ''' <remarks></remarks>
    Private Function GetBlendedTaxRate(ByVal retirementTable As DataTable, ByVal savingsTable As DataTable, Optional ByVal planAccountSelectionChange As String = "") As Decimal
        Dim totalAvailableAmountRetirementPlan As Decimal = 0
        Dim totalTaxableAmountRetirementPlan As Decimal = 0
        Dim totalInterestRetirementPlan As Decimal = 0
        Dim totalNonTaxableAmountRetirementPlan As Decimal = 0
        Dim totalAvailableAmountSavingsPlan As Decimal = 0
        Dim totalTaxableAmountSavingsPlan As Decimal = 0
        Dim totalInterestSavingsPlan As Decimal = 0
        Dim totalNonTaxableAmountSavingsPlan As Decimal = 0
        Dim totalAvailableAmountConsolidated As Decimal = 0
        Dim totalTaxableAmountConsolidated As Decimal = 0
        Dim totalNonTaxableAmountConsolidated As Decimal = 0
        Dim taxRate As Decimal


        '--------------- START - Set Total Avaialble amount and total taxable amount which will be used to calculate covid taxable and non-covid taxable---------------
        If IsRefundOptionsSelectedForRetirement() Then
            If HelperFunctions.isNonEmpty(retirementTable) Then
                'Update selected column value (True/False) in table for accounts selected/Unselected in Retirement grid data
                If Not String.IsNullOrEmpty(planAccountSelectionChange) And planAccountSelectionChange.ToUpper() = "RETIREMENT" Then
                    retirementTable = SetAccountSelectedForPlan(retirementTable, DataGridAccContributionRetirement)
                End If
                'Set Retirement plan total amount and total taxable amount
                If Not IsDBNull(retirementTable.Compute("Sum(Total)", "Selected = True")) Then
                    totalAvailableAmountRetirementPlan = retirementTable.Compute("Sum(Total)", "Selected = True")
                End If

                If Not IsDBNull(retirementTable.Compute("Sum(Taxable)", "Selected = True")) Then
                    totalTaxableAmountRetirementPlan = retirementTable.Compute("Sum(Taxable)", "Selected = True")
                End If

                If Not IsDBNull(retirementTable.Compute("Sum(Interest)", "Selected = True")) Then
                    totalInterestRetirementPlan = retirementTable.Compute("Sum(Interest)", "Selected = True")
                End If

                If Not IsDBNull(retirementTable.Compute("Sum([Non-Taxable])", "Selected = True")) Then
                    totalNonTaxableAmountRetirementPlan = retirementTable.Compute("Sum([Non-Taxable])", "Selected = True")
                End If
            End If
        End If

        If IsRefundOptionsSelectedForSavings() Then
            If HelperFunctions.isNonEmpty(savingsTable) Then
                'Update selected column value (True/False) in table for accounts selected/Unselected in Savings grid data
                If Not String.IsNullOrEmpty(planAccountSelectionChange) And planAccountSelectionChange.ToUpper() = "SAVINGS" Then
                    savingsTable = SetAccountSelectedForPlan(savingsTable, DataGridAccContributionSavings)
                End If
                'Set Savings plan total amount and total taxable amount
                If Not IsDBNull(savingsTable.Compute("Sum(Total)", "Selected = True")) Then
                    totalAvailableAmountSavingsPlan = savingsTable.Compute("Sum(Total)", "Selected = True")
                End If

                If Not IsDBNull(savingsTable.Compute("Sum(Taxable)", "Selected = True")) Then
                    totalTaxableAmountSavingsPlan = savingsTable.Compute("Sum(Taxable)", "Selected = True")
                End If

                If Not IsDBNull(savingsTable.Compute("Sum(Interest)", "Selected = True")) Then
                    totalInterestSavingsPlan = savingsTable.Compute("Sum(Interest)", "Selected = True")
                End If

                If Not IsDBNull(savingsTable.Compute("Sum([Non-Taxable])", "Selected = True")) Then
                    totalNonTaxableAmountSavingsPlan = savingsTable.Compute("Sum([Non-Taxable])", "Selected = True")
                End If
            End If
        End If
        
        'Set combined plan total amount and total taxable amount
        totalAvailableAmountConsolidated = totalAvailableAmountRetirementPlan + totalAvailableAmountSavingsPlan
        totalTaxableAmountConsolidated = totalTaxableAmountRetirementPlan + totalTaxableAmountSavingsPlan + totalInterestRetirementPlan + totalInterestSavingsPlan
        totalNonTaxableAmountConsolidated = totalNonTaxableAmountRetirementPlan + totalNonTaxableAmountSavingsPlan

        '--------------- END - Set Total Avaialble amount and total taxable amount which will be used to calculate covid taxable and non-covid taxable---------------
        If totalAvailableAmountConsolidated > 0 Then
            'Set Covid taxable and non-covid taxable which will be used to calculate blended tax rate
            objRefundRequest.GetTaxableAmountForCovidTaxRateCalculation(totalAvailableAmountConsolidated, totalTaxableAmountConsolidated, totalNonTaxableAmountConsolidated, objRefundRequest.CovidAmountRemaining)

            'Get Blended Tax Rate
            taxRate = objRefundRequest.GetCovidApplicableTaxRate(objRefundRequest.CovidTaxableAmount, objRefundRequest.NonCovidTaxableAmount)
        Else
            taxRate = objRefundRequest.ApplicableFederalTaxRate
        End If


        Return taxRate
    End Function
    'END - MMR | 04/20/2020 |YRS-AT-4854 | Method to get blended tax rate to apply covid benefit rules

    'START - MMR | 04/20/2020 |YRS-AT-4854 | Method to set account selected value for check/uncheck of checkbox in retirement and savings plan grid. This will help to get amount for calculation of selected accounts only.
    ''' <summary>
    ''' Returns table by updating selected column value (True/False) in table based on Checkbox selection
    ''' </summary>
    ''' <param name="planTable">Plan Table (Retirement/Savings)</param>
    ''' <param name="planDataGrid">Datagrid (Retirement/Savings) </param>
    ''' <returns>DataTable (Retirement/Savings)</returns>
    ''' <remarks></remarks>
    Private Function SetAccountSelectedForPlan(ByVal planTable As DataTable, ByVal planDataGrid As DataGrid) As DataTable
        Dim checkBox As CheckBox
        Dim accountType As String
        Dim IsSelected As Boolean

        If Not planDataGrid Is Nothing Then
            For Each item As DataGridItem In planDataGrid.Items
                checkBox = item.FindControl("Selected")
                If checkBox.Checked = False Then
                    IsSelected = False
                Else
                    IsSelected = True
                End If
                accountType = item.Cells(2).Text.ToString()
                If accountType <> "Total" Then
                    If HelperFunctions.isNonEmpty(planTable) Then
                        For Each accountRow In planTable.Rows
                            If accountRow("AccountType").ToString() = accountType Then
                                accountRow("Selected") = IsSelected
                            End If
                            'Exit For
                        Next
                        planTable.AcceptChanges()
                    End If
                End If
            Next
        End If

        Return planTable
    End Function
    'END - MMR | 04/20/2020 |YRS-AT-4854 | Method to set account selected value for check/uncheck of checkbox in retirement and savings plan grid. This will help to get amount for calculation of selected accounts only.

    'START - MMR | 04/20/2020 |YRS-AT-4854 | Method will update tax value by applying blended tax rate for each account in Retirement and savings plan
    ''' <summary>
    ''' Returns updated table with tax value by applying blended tax rate
    ''' </summary>
    ''' <param name="planTable">Plan Table (Retirement/Savings)</param>
    ''' <param name="blendedTaxRate">Blended Tax Rate</param>
    ''' <returns>DataTable (Retirement/Savings)</returns>
    ''' <remarks></remarks>
    Private Function UpdateDisplayTableTaxValueWithBlendedTaxRate(ByVal planTable As DataTable, ByVal blendedTaxRate As Decimal) As DataTable

        'Calcuating tax based on blended tax rate
        If HelperFunctions.isNonEmpty(planTable) Then
            For Each accountRow In planTable.Rows
                If accountRow("AccountType").GetType.ToString <> "System.DBNull" Then
                    accountRow("TaxWithheld") = Math.Round(((((accountRow("Taxable") + accountRow("Interest"))) * blendedTaxRate) / 100), 2)
                    If DirectCast(accountRow("AccountType"), String) = "Total" Then
                        If Not IsDBNull(planTable.Compute("Sum(Total)", "Selected = True")) Then
                            accountRow("TaxWithheld") = planTable.Compute("Sum(TaxWithheld)", "Selected = True")
                        Else
                            accountRow("TaxWithheld") = 0
                        End If
                    End If
                End If
            Next
            planTable.AcceptChanges()
        End If

        Return planTable
    End Function
    'END - MMR | 04/20/2020 |YRS-AT-4854 | Method will update tax value by applying blended tax rate for each account in Retirement and savings plan

    'START - MMR | 04/20/2020 |YRS-AT-4854 | Method will Initialize Covid related details
    ''' <summary>
    ''' Method will intialize Covid related details required for UI Display purpose and tax rate calculation
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeCovidDetails()
        'Fetch covid amount used so far and display on UI
        objRefundRequest.CovidAmountUsed = objRefundRequest.GetCovidAmountUsed(Integer.Parse(Session("FundNo")))
        txtCovidAmountUsed.Text = FormatCurrency(objRefundRequest.CovidAmountUsed)
        'Assigning existing federal tax rate to covid related declared property
        objRefundRequest.ApplicableFederalTaxRate = Me.FederalTaxRate
        'Calculate covid benefit amount remaining and display on UI 
        If objRefundRequest.CovidAmountUsed <= objRefundRequest.CovidAmountLimit Then
            objRefundRequest.CovidAmountRemaining = objRefundRequest.CovidAmountLimit - objRefundRequest.CovidAmountUsed
            'Display Covid Amount Available for withdrawal
            txtCovidAmountAvailable.Text = FormatCurrency(GetMaximumCovidAmountAvailableForWithdrawal())
        End If
        'Set covid tax rate value for display on UI
        txtCovidTaxRate.Text = Convert.ToString(objRefundRequest.CovidTaxRate)
    End Sub
    'END - MMR | 04/20/2020 |YRS-AT-4854 | Method will Initialize Covid related details

    'START - MMR | 04/20/2020 |YRS-AT-4854 | Method will identify all refund options selected or not for Retirement Plan
    ''' <summary>
    ''' Method will return whether all refund options selected or not for Retirement Plan
    ''' </summary>
    ''' <returns> TRUE/FALSE</returns>
    ''' <remarks></remarks>
    Private Function IsRefundOptionsSelectedForRetirement() As Boolean
        Dim flag As Boolean = True
        If (CheckboxRegular.Checked = False And CheckboxExcludeYMCA.Checked = False And CheckboxVoluntryAccounts.Checked = False And CheckboxPartialRetirement.Checked = False) Or (CheckboxPartialRetirement.Checked = True And TextboxPartialRetirement.Text.Trim.Length = 0) Then
            flag = False
        End If
        Return flag
    End Function
    'END - MMR | 04/20/2020 |YRS-AT-4854 | Method will identify all refund options selected or not for Retirement Plan

    'START - MMR | 04/20/2020 |YRS-AT-4854 | Method will identify all refund options selected or not for Savings Plan
    ''' <summary>
    ''' Method will return whether all refund options selected or not for Savings Plan
    ''' </summary>
    ''' <returns>TRUE/FALSE</returns>
    ''' <remarks></remarks></returns>
    ''' <remarks></remarks>
    Private Function IsRefundOptionsSelectedForSavings() As Boolean
        Dim flag As Boolean = True
        If (CheckboxSavingsVoluntary.Checked = False And CheckboxPartialSavings.Checked = False) Or (CheckboxPartialSavings.Checked = True And TextboxPartialSavings.Text.Trim.Length = 0) Then
            flag = False
        End If
        Return flag
    End Function
    'END - MMR | 04/20/2020 |YRS-AT-4854 | Method will identify all refund options selected or not for Retirement Plan

    'START - MMR | 04/20/2020 |YRS-AT-4854 | Method will display Covid taxable and non-taxable amount on UI
    ''' <summary>
    ''' Method will display Covid taxable and non-taxable amount on UI
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayCovidTaxableAndNonTaxableAmount()
        If Not IsRefundOptionsSelectedForRetirement() And Not IsRefundOptionsSelectedForSavings() Then
            lblDisplayCovidTaxableAmount.Text = 0.0
            lblDisplayCovidNonTaxableAmount.Text = 0.0
        Else
            lblDisplayCovidTaxableAmount.Text = FormatCurrency(objRefundRequest.CovidTaxableAmount)
            lblDisplayCovidNonTaxableAmount.Text = FormatCurrency(objRefundRequest.CovidNonTaxableAmount)
        End If
    End Sub
    'END - MMR | 04/20/2020 |YRS-AT-4854 | Method will display Covid taxable and non-taxable amount on UI

    'START - MMR | 05/05/2020 |YRS-AT-4854 | Method will get total tax to be withheld for requested amount which will be used for saving request
    ''' <summary>
    ''' Method will get total tax to be withheld for requested amount
    ''' </summary>
    ''' <param name="retirementPlanTable">Retirement Plan Table</param>
    ''' <param name="savingsPlanTable">Savings Plan Table</param>
    ''' <returns>Total Tax Withheld</returns>
    ''' <remarks></remarks>
    Private Function GetTotalTaxWithheld(ByVal retirementPlanTable As DataTable, ByVal savingsPlanTable As DataTable) As Decimal
        Dim totalTaxWithheld As Decimal = 0
        Dim totalTaxWithheldRetirementPlan As Decimal = 0
        Dim totalTaxWithheldSavingsPlan As Decimal = 0
        Dim totalTaxWithheldTable As DataTable

        If IsRefundOptionsSelectedForRetirement() Then
            If HelperFunctions.isNonEmpty(retirementPlanTable) Then
                'Set Retirement plan tax withheld
                If Not IsDBNull(retirementPlanTable.Compute("Sum(TaxWithheld)", "Selected = True")) Then
                    totalTaxWithheldRetirementPlan = retirementPlanTable.Compute("Sum(TaxWithheld)", "Selected = True")
                End If
            End If
        End If

        If IsRefundOptionsSelectedForSavings() Then
            If HelperFunctions.isNonEmpty(savingsPlanTable) Then
                'Set Savings plan tax withheld
                If Not IsDBNull(savingsPlanTable.Compute("Sum(TaxWithheld)", "Selected = True")) Then
                    totalTaxWithheldSavingsPlan = savingsPlanTable.Compute("Sum(TaxWithheld)", "Selected = True")
                End If
            End If
        End If
        'Set combined tax withheld amount
        totalTaxWithheld = totalTaxWithheldRetirementPlan + totalTaxWithheldSavingsPlan
        Return totalTaxWithheld
    End Function
    'END - MMR | 05/05/2020 |YRS-AT-4854 | Method will get total tax to be withheld for requested amount which will be used for saving request

    'START - MMR | 05/13/2020 |YRS-AT-4854 | Method will return maximum amount available for COVID Withdrawal
    ''' <summary>
    ''' Method will return maximum amount available for COVID Withdrawal
    ''' </summary>
    ''' <returns>Covid Withdrawal Amount</returns>
    ''' <remarks></remarks>
    Private Function GetMaximumCovidAmountAvailableForWithdrawal() As Decimal
        Dim totalRetirementPlanWithdrawalAmount As Decimal = 0
        Dim totalSavingsPlanWithdrawalAmount As Decimal = 0
        Dim totalWithdrawalAmount As Decimal = 0

        'Set Retirement plan available amount for withdrawal
        If IsRefundOptionsSelectedForRetirement() Then 'Check if Refund option available for retirment plan then only set retirement plan total
            If HelperFunctions.isNonEmpty(FinalDataSet.Tables("RetirementPlanTable")) Then
                If Not IsDBNull(FinalDataSet.Tables("RetirementPlanTable").Compute("Sum(Total)", "Selected = True")) Then
                    totalRetirementPlanWithdrawalAmount = FinalDataSet.Tables("RetirementPlanTable").Compute("Sum(Total)", "Selected = True")
                End If
            End If
        End If
      

        'Set Savings plan available amount for withdrawal
        If IsRefundOptionsSelectedForSavings() Then 'Check if Refund option available for savings plan then only set savings plan total
            If HelperFunctions.isNonEmpty(FinalDataSet.Tables("SavingPlanTable")) Then
                If Not IsDBNull(FinalDataSet.Tables("SavingPlanTable").Compute("Sum(Total)", "Selected = True")) Then
                    totalSavingsPlanWithdrawalAmount = FinalDataSet.Tables("SavingPlanTable").Compute("Sum(Total)", "Selected = True")
                End If
            End If
        End If

        'Set total amount available for withdrawal
        totalWithdrawalAmount = totalRetirementPlanWithdrawalAmount + totalSavingsPlanWithdrawalAmount

        'If total amount available for withdrawal is less than covid amount available for use, then return total amount as Covid amount available for withdrawal
        If totalWithdrawalAmount < objRefundRequest.CovidAmountRemaining Then
            Return totalWithdrawalAmount
        Else
            Return objRefundRequest.CovidAmountRemaining
        End If
    End Function
    'END - MMR | 05/13/2020 |YRS-AT-4854 | Method will return maximum amount available for COVID Withdrawal
End Class