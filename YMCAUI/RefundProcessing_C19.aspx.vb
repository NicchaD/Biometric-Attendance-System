
'/**************************************************************************************************************/
'//' Copyright YMCA Retirement Fund All Rights Reserved. 
'//
'// Author: Manthan Rajguru
'// Created on: 04/09/2020
'// Summary of Functionality: New Screen for COVID19 withdrawal processing
'// Declared in Version: 20.8.1.2| YRS-AT-4854 -  COVID - Special withdrawal functionality needed due to CARE Act/COVID-19 (Track IT - 41688) 
'//
'/**************************************************************************************************************/
'// REVISION HISTORY:
'// ------------------------------------------------------------------------------------------------------
'// Developer Name              | Date         | Version No      | Ticket
'// ------------------------------------------------------------------------------------------------------
'//  Megha Lad	                | 17.04.2020   |	20.8.1.2     | YRS-AT-4874 -  COVID - Special withdrawal functionality needed in YRS processing screen due to CARE Act/COVID-19 (Track IT - 41688)
'// ------------------------------------------------------------------------------------------------------
' /**************************************************************************************************************/

Public Class RefundProcessing_C19
    Inherits System.Web.UI.Page


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents MultiPageVoluntaryRefund As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents LabelStatus As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTerminationPIA As System.Web.UI.WebControls.Label
    Protected WithEvents LabelCurrentPIA As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxStatus As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTerminationPIA As System.Web.UI.WebControls.TextBox
    'Commented by neeraj 08-Sep-09 for issue id YRS 5.0-821
    'Protected WithEvents TextBoxCurrentPIA As System.Web.UI.WebControls.TextBox

    'Added Ganeswar 10thApril09 for BA Account Phase-V /Begin
    ''Protected WithEvents TextBoxTerminationBA As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxCurrentBA As System.Web.UI.WebControls.TextBox
    'Added Ganeswar 10thApril09 for BA Account Phase-V /End

    'Added By ganeswar on 14th OCT for new grid(First Instalment)
    Protected WithEvents DataGridInstallment As System.Web.UI.WebControls.DataGrid
    'Added By ganeswar on 14th OCT for new grid(First Instalment)

    Protected WithEvents LabelRequestedAccts As System.Web.UI.WebControls.Label
    Protected WithEvents DataGridRequestedAccts As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelTotalsRequestedAccts As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTotalsRequestedAccts1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTotalsRequestedAccts2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTotalsRequestedAccts3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTotalsRequestedAccts4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTotalsRequestedAccts5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsRequestedAccts6 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsRequestedAccts7 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsRequestedAccts8 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCurrentAccounts As System.Web.UI.WebControls.Label
    Protected WithEvents DatagridCurrentAccts As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelTotalsCurrentAccts As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxTotalsCurrentAccts1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsCurrentAccts2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsCurrentAccts3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsCurrentAccts4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsCurrentAccts5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsCurrentAccts6 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsCurrentAccts7 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsCurrentAccts8 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelNonFundedContributions As System.Web.UI.WebControls.Label
    Protected WithEvents DatagridNonFundedContributions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelTotalsNonFundedContributions As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxTotalsNonFundedContributions1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsNonFundedContributions2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsNonFundedContributions3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsNonFundedContributions4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsNonFundedContributions5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsNonFundedContributions6 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsNonFundedContributions7 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalsNonFundedContributions8 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelYes1 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNo1 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelYes2 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNo2 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelReleaseSigned As System.Web.UI.WebControls.Label
  
    Protected WithEvents TextBoxPayee1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridPayee1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelPayee2 As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPayee2 As RolloverInstitution 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
    Protected WithEvents DatagridPayee2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents RadioButtonNone As System.Web.UI.WebControls.RadioButton

    Protected WithEvents RadioButtonRolloverAll As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonTaxableOnly As System.Web.UI.WebControls.RadioButton
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents DatagridPayee3 As System.Web.UI.WebControls.DataGrid
   
    Protected WithEvents ButtonAddress As System.Web.UI.WebControls.Button
   
    Protected WithEvents LabelCity As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxRate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxable As System.Web.UI.WebControls.Label

    'Added By parveen on 04-Nov-2009 To Hide the First Installment   
    Protected trFirstInstallment As System.Web.UI.HtmlControls.HtmlTableRow
    Protected trInstallmentGrid As System.Web.UI.HtmlControls.HtmlTableRow
    'Added By parveen on 04-Nov-2009 To Hide the First Installment   

    Protected WithEvents LabelNonTaxable As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTax As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNet As System.Web.UI.WebControls.Label
   
    Protected WithEvents TextboxTaxRate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTax As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNet As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelTaxable2 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNonTaxable2 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNet2 As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxNonTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTaxable2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNonTaxable2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNet2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents DatagridDeductions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelTaxable3 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNonTaxable3 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNet3 As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxTaxable3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNonTaxable3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNet3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelDeductions As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxDeductions As System.Web.UI.WebControls.TextBox
    Protected WithEvents RadioButtonReleaseSignedYes As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonReleaseSignedNo As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonAddressUpdatingYes As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonAddressUpdatingNo As System.Web.UI.WebControls.RadioButton
    Protected WithEvents LabelNotarized As System.Web.UI.WebControls.Label
    Protected WithEvents RadioButtonNotarizedYes As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonNotarizedNo As System.Web.UI.WebControls.RadioButton
    Protected WithEvents LabelRollover As System.Web.UI.WebControls.Label
    
    Protected WithEvents LabelWaiver As System.Web.UI.WebControls.Label
    Protected WithEvents RadioButtonWaiverYes As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonWaiverNo As System.Web.UI.WebControls.RadioButton
    Protected WithEvents LabelAddnlWitholding As System.Web.UI.WebControls.Label
    
    Protected WithEvents LabelPayee1 As System.Web.UI.WebControls.Label
    Protected WithEvents LabelPayee3 As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPayee3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelTaxableFinal As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNonTaxableFinal As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxFinal As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNetFinal As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxTaxableFinal As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNonTaxableFinal As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTaxFinal As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxNetFinal As System.Web.UI.WebControls.TextBox
    Protected WithEvents TabStripVoluntaryRefund As Microsoft.Web.UI.WebControls.TabStrip
    'Protected WithEvents datagridCheckBox As CustomControls.CheckBoxColumn '--Manthan Rajguru 2015-09-24 YRS-AT-2550: Commented as control not used anywhere in the page
    Protected WithEvents CheckBoxDeduction As System.Web.UI.WebControls.CheckBox
    Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonTab1OK As System.Web.UI.WebControls.Button

    Protected WithEvents TextboxMinDistTaxRate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxMinDistAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxMinDistTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxMinDistNet As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelRequiredMinDisbAmount As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxMinDistNonTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents Button3 As System.Web.UI.WebControls.Button

    Protected WithEvents LabelPersonalMonies As System.Web.UI.WebControls.Label
    Protected WithEvents RadiobuttonPersonalMoniesYes As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadiobuttonPersonalMoniesNo As System.Web.UI.WebControls.RadioButton
    Protected WithEvents LabelTitle As System.Web.UI.WebControls.Label
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelShortDesc As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCodeValue As System.Web.UI.WebControls.TextBox

   
    Protected WithEvents LabelVoluntaryAccounts As System.Web.UI.WebControls.Label
    'Added Ganeswar 10thMay 09 for Hardship Roll over Account Phase-V /begin

    'Plan Split Changes
    Protected WithEvents LabelPlanChosen As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPlanChosen As System.Web.UI.WebControls.TextBox
    'Plan Split Changes

    'Integration of Hardship Processing
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxRequestAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTDAvailableAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxHardShipTaxRate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxHardShipAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents textboxHardShipNonTaxableAmount As System.Web.UI.WebControls.TextBox 'MMR | 2017.07.30 | YRS-AT-3870 | Added control for non-taxable hardship amount
    Protected WithEvents TextboxHardShip As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxHardShipNet As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxVoluntaryWithdrawalTotal As System.Web.UI.WebControls.TextBox
    'Integration of Hardship Processing
    'Added by Dilip on 31-20-2009 for market base withdrawal
    Protected WithEvents RadiobuttonRolloverOnlyFirstInstallment As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadiobuttonRolloverAllInstallment As System.Web.UI.WebControls.RadioButton
   
    Protected WithEvents LabelDeferedInstallmentAmount As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxDeferedInstallmentAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelTotalRolloverAmount As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxRemainingMoneyinMKT As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelRemainingMoneyinMKT As System.Web.UI.WebControls.Label

    Protected WithEvents TextboxTotalRolloverAmount As System.Web.UI.WebControls.TextBox
   

    'Added by Dilip on 31-20-2009 for market base withdrawal

    'Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
    Protected WithEvents CheckBoxRollovers As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxAddnlWitholding As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckboxVoluntaryRollover As System.Web.UI.WebControls.CheckBox
    'Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox

    Protected trHardshipVoluntary As System.Web.UI.HtmlControls.HtmlTableRow

    Protected WithEvents RadioButtonSpecificAmount As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TextboxRolloverAmount As System.Web.UI.WebControls.TextBox
    'AA:24.09.2013 : BT-1501: Displaying address with address control not with textboxes
    Protected WithEvents AddressWebUserControl1 As AddressUserControlNew

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Dim chkCreatePayeeProcessed As Boolean = False  'This flag is to keep check on wether createPayee function has already called while entering Required amount.
    Protected WithEvents m_DataGridCheckBox As CustomControls.CheckBoxColumn 'Manthan Rajguru   2015.09.24    YRS-AT-2550: Custom Control reference given for DatagridcheckBox
    Private designerPlaceholderDeclaration As System.Object
    Dim objRefundProcess As New Refunds_C19
    Dim IDM As New IDMforAll
    Protected WithEvents DivMainMessage As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents txtProcessingFee As System.Web.UI.WebControls.TextBox 'SR | 2016.06.06 | YRS-AT2962 | Add textbox for processing fee


    'START : ML | 22.04.20 | YRS-AT-4874 | Display COVID benefit breakup --%>
    Protected WithEvents txtTaxRateC19 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTaxableC19 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNonTaxableC19 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTaxC19 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNetC19 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNewCovidAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnChangeAmount As System.Web.UI.WebControls.Button

    'END : ML | 22.04.20 | YRS-AT-4874 | Display COVID benefit breakup --%>
    ' SC | 2020.04.05 | YRS-AT-4874 | Get covid requested, unused and available amounts
    Protected WithEvents txtCovidAmountRequested As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCovidAmountUsed As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCovidAmountAvailable As System.Web.UI.WebControls.TextBox
    ' SC | 2020.04.05 | YRS-AT-4874 | Get covid requested, unused and available amounts
    Protected WithEvents ReqFieldValidatorNewCovidAmount As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents RangeValidatorNewCovidAmount As System.Web.UI.WebControls.RangeValidator


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
    'Priya 14-Jan-2009 : YRS 5.0-637 AC Account interest components added new constant for AC account type group.
    Const m_const_RetirementPlan_AC As String = "RAC"
    'End 14-Jan-2009
    'Begin Code Merge by Dilip on 07-05-2009
    'END - Retirement plan account groups

    'Added Ganeswar 10thApril09 for BA Account Phase-V /Begin
    Const m_const_RetirementPlan_BA As String = "RBA"
    Dim blnFlagHardShip As Boolean = False
    'Added Ganeswar 10thApril09 for BA Account Phase-V /End

    'Phase V YMCA Legacy acct & YMCA Acct const variable 
    Const m_const_YMCA_Legacy_Acct As String = "YMCA (Legacy) Acct"
    Const m_const_YMCA__Acct As String = "YMCA Acct"
    'Phase V YMCA Legacy acct & YMCA Acct const variable

    'START - Ssvings plan account groups
    Const m_const_SavingsPlan_TD As String = "STD"
    Const m_const_SavingsPlan_TM As String = "STM"
    Const m_const_SavingsPlan_RT As String = "SRT"
    'END - Savings plan account groups
    'Added by Parveen On 16 Dec 2009 For Rollover Options
    Const m_const_PartialRollOver As String = "PARTIAL"
    Const m_const_RolloverAll As String = "ROLLOVERALL"
    Const m_const_AllTaxable As String = "ALLTAXABLE"
    Const m_const_RolloverNone As String = "NONE"
    'Added by Parveen On 16 Dec 2009 For Rollover Options
    Dim objRefundRequest As New Refunds_C19
    Dim l_bool_HardShipAmount As Boolean = False
    'Dim l_dec_TDAmount As Decimal = 0.0
#End Region
#Region " Form Properties "

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

    Private Property l_dec_TDAmount() As Decimal
        Get
            If Not Session("l_dec_TDAmount_C19") Is Nothing Then
                Return (CType(Session("l_dec_TDAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("l_dec_TDAmount_C19") = Value
        End Set
    End Property

    Private Property l_dec_TDRequestedAmount() As Decimal
        Get
            If Not Session("l_dec_TDRequestedAmount_C19") Is Nothing Then
                Return (CType(Session("l_dec_TDRequestedAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("l_dec_TDRequestedAmount_C19") = Value
        End Set
    End Property

    Private Property TotalAmount() As Decimal
        Get
            If Not Session("TotalAmount_C19") Is Nothing Then
                Return (CType(Session("TotalAmount_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("TotalAmount_C19") = Value
        End Set
    End Property
    Public Property VoluntaryWithdrawalTotal() As Decimal
        Get
            If Not Session("VoluntaryWithdrawalTotal_C19") Is Nothing Then
                Return (CType(Session("VoluntaryWithdrawalTotal_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("VoluntaryWithdrawalTotal_C19") = Value
        End Set
    End Property
    Public Property HardshipWithdrawalTotal() As Decimal
        Get
            If Not Session("HardshipWithdrawalTotal_C19") Is Nothing Then
                Return (CType(Session("HardshipWithdrawalTotal_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("HardshipWithdrawalTotal_C19") = Value
        End Set
    End Property
    'To get / set the PersonID property.    
    Private Property PersonID() As String
        Get
            If Not Session("PersonID") Is Nothing Then
                Return (CType(Session("PersonID"), String))
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
                Return (CType(Session("FundID"), String))
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
                Return (CType(Session("RefundType_C19"), String)).Trim.ToUpper
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
                Return (CType(Session("ISMarket_C19"), Int16))
            Else
                Return 1
            End If

        End Get
        Set(ByVal Value As Int16)
            Session("ISMarket_C19") = Value
        End Set
    End Property
    'Added By Dilip on 30-10-2009  for market base withdrawal
    ' To get / set Payee2 ID
    Private Property Payee2ID() As String
        Get
            If Not Session("Payee2ID_C19") Is Nothing Then
                Return (CType(Session("Payee2ID_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("Payee2ID_C19") = Value
        End Set
    End Property

    ' To get / set Payee3 ID
    Private Property Payee3ID() As String
        Get
            If Not Session("Payee3ID_C19") Is Nothing Then
                Return (CType(Session("Payee3ID_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("Payee3ID_C19") = Value
        End Set
    End Property


    ''To Keep the Currency Code.
    Private Property CurrencyCode() As String
        Get
            If Not Session("CurrencyCode_C19") Is Nothing Then
                Return (CType(Session("CurrencyCode_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("CurrencyCode_C19") = Value
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


    ' To get / set Payee1 Addresss ID
    Private Property PayeeAddressID() As String
        Get
            If Not Session("PayeeAddressID_C19") Is Nothing Then
                Return (CType(Session("PayeeAddressID_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PayeeAddressID_C19") = Value
        End Set
    End Property


    ' To get / set RefundType
    Private Property ChangedRefundType() As String
        Get
            If Not Session("ChangedRefundType_C19") Is Nothing Then
                Return (CType(Session("ChangedRefundType_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("ChangedRefundType_C19") = Value
        End Set
    End Property


    ' To Get / Set Termination PIA
    Private Property TerminationPIA() As Decimal
        Get
            If Not Session("TerminationPIA_C19") Is Nothing Then
                Return (CType(Session("TerminationPIA_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get

        Set(ByVal Value As Decimal)
            Session("TerminationPIA_C19") = Value
        End Set
    End Property

    ' To Get / Set YMCAAvailableAmount
    Private Property YMCAAvailableAmount() As Decimal
        Get
            If Not Session("YMCAAvailableAmount_C19") Is Nothing Then
                Return (CType(Session("YMCAAvailableAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get

        Set(ByVal Value As Decimal)
            Session("YMCAAvailableAmount_C19") = Value
        End Set
    End Property

    ' To Get / Set Personal Amount
    Private Property PersonalAmount() As Decimal
        Get
            If Not Session("PersonalAmount_C19") Is Nothing Then
                Return (CType(Session("PersonalAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get

        Set(ByVal Value As Decimal)
            Session("PersonalAmount_C19") = Value
        End Set
    End Property



    'To Get / Set Total Refund Amount
    Private Property TotalRefundAmount() As Decimal
        Get
            If Not Session("TotalRefundAmount_C19") Is Nothing Then
                Return (CType(Session("TotalRefundAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get

        Set(ByVal Value As Decimal)
            Session("TotalRefundAmount_C19") = Value
        End Set
    End Property


    'To Get / Set CurrentPIA
    Private Property CurrentPIA() As Decimal
        Get
            If Not Session("CurrentPIA_C19") Is Nothing Then
                Return (CType(Session("CurrentPIA_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get

        Set(ByVal Value As Decimal)
            Session("CurrentPIA_C19") = Value
        End Set
    End Property

    'Added Ganeswar 10thApril09 for BA Account Phase-V /Begin
    Private Property CurrentBA() As Decimal
        Get
            If Not Session("CurrentBA_C19") Is Nothing Then
                Return (CType(Session("CurrentBA_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get

        Set(ByVal Value As Decimal)
            Session("CurrentBA_C19") = Value
        End Set
    End Property

    Private Property TerminationBA() As Decimal
        Get
            If Not Session("TerminationBA_C19") Is Nothing Then
                Return (CType(Session("TerminationBA_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get

        Set(ByVal Value As Decimal)
            Session("TerminationBA_C19") = Value
        End Set
    End Property
    'Added Ganeswar 10thApril09 for BA Account Phase-V /End


    'To Keep the RefundRequest ID for Selected Member
    Private Property SessionRefundRequestID() As String
        Get
            If Not (Session("RefundRequestID_C19")) Is Nothing Then
                Return (CType(Session("RefundRequestID_C19"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("RefundRequestID_C19") = Value
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
                Return (CType(Session("PersonAge_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("PersonAge_C19") = Value
        End Set
    End Property

    ' To get / set Deductions amount.
    Private Property DeductionsAmount() As Decimal
        Get
            If Not Session("DeductionsAmount_C19") Is Nothing Then
                Return (CType(Session("DeductionsAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("DeductionsAmount_C19") = Value
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

    ' To Get / Set Is AM accountType 
    Private Property IsAcctTypeAMIsPresent() As Boolean
        Get
            If Not Session("IsAcctTypeAMIsPresent_C19") Is Nothing Then
                Return (CType(Session("IsAcctTypeAMIsPresent_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsAcctTypeAMIsPresent_C19") = Value
        End Set
    End Property


    ' To Get / Set Is HardShip flag
    Private Property IsHardShip() As Boolean
        Get
            If Not Session("IsHardShip_C19") Is Nothing Then
                Return (CType(Session("IsHardShip_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsHardShip_C19") = Value
        End Set
    End Property

    ' To Get / Set Is HardShip flag
    Private Property IsTookTDAccount() As Boolean
        Get
            If Not Session("IsTookTDAccount_C19") Is Nothing Then
                Return (CType(Session("IsTookTDAccount_C19"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsTookTDAccount_C19") = Value
        End Set
    End Property


    'To Get / Set Tax Rate
    Private Property TaxRate() As Decimal
        Get
            If Not Session("TaxRate_C19") Is Nothing Then
                Return (CType(Session("TaxRate_C19"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TaxRate_C19") = Value
        End Set
    End Property

    'To Get / Set Tax Rate
    Private Property MinDistributionTaxRate() As Decimal
        Get
            If Not Session("MinDistributionTaxRate_C19") Is Nothing Then
                Return (CType(Session("MinDistributionTaxRate_C19"), Decimal))
            Else
                Return 10
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinDistributionTaxRate_C19") = Value
        End Set
    End Property


    'To set & get Minimum Distribution Amount
    Private Property MinimumDistributionAmount() As Decimal
        Get
            If Not Session("MinimumDistributionAmount_C19") Is Nothing Then
                Return (CType(Session("MinimumDistributionAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinimumDistributionAmount_C19") = Value
        End Set
    End Property
    '' To Get / Set the Refund Expiry Date
    Private Property RefundExpiryDate() As Integer
        Get
            If Not Session("RefundExpiryDate_C19") Is Nothing Then
                Return (CType(Session("RefundExpiryDate_C19"), Integer))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("RefundExpiryDate_C19") = Value
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


    ' To Get / set the MinimumDistributedAge
    Private Property MinimumDistributedAge() As Decimal
        Get
            If Not Session("MinimumDistributedAge_C19") Is Nothing Then
            Else
                Return 70.5
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinimumDistributedAge_C19") = Value
        End Set
    End Property

    'To Get / Set Maximum PIA Amount.
    Private Property MaximumPIAAmount() As Decimal
        Get
            If Not Session("MaximumPIAAmount_C19") Is Nothing Then
                Return (CType(Session("MaximumPIAAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MaximumPIAAmount_C19") = Value
        End Set
    End Property

    'To Get / Set HardShip Amount, incase of HardShip.
    Private Property HardshipAmount() As Decimal
        Get
            If Not Session("HardshipAmount_C19") Is Nothing Then
                Return (CType(Session("HardshipAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("HardshipAmount_C19") = Value
        End Set
    End Property

    'To Get / Set HardShipTaxRate
    Private Property HardShipTaxRate() As Decimal
        Get
            If Not Session("HardShipTaxRate_C19") Is Nothing Then
                Return (CType(Session("HardShipTaxRate_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("HardShipTaxRate_C19") = Value
        End Set
    End Property

    ' To get / set RefundStatus, this Property is used in RefundRequestWebForm.
    Private Property RefundStatus() As String
        Get
            If Not Session("RefundStatus_C19") Is Nothing Then
                Return (CType(Session("RefundStatus_C19"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("RefundStatus_C19") = Value
        End Set
    End Property


    'To Get / Set HardShip Amount, incase of HardShip.
    Private Property TDUsedAmount() As Decimal
        Get
            If Not Session("TDUsedAmount_C19") Is Nothing Then
                Return (CType(Session("TDUsedAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TDUsedAmount_C19") = Value
        End Set
    End Property


    'To Keep the flag to store Whether Type is Personal Or Not.
    Private Property IsPersonalOnly() As Boolean
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
    Private Property TaxableAmount() As Decimal
        Get
            If Not (Session("TaxableAmount_C19")) Is Nothing Then
                Return (CType(Session("TaxableAmount_C19"), Decimal))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TaxableAmount_C19") = Value
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
    'Shubhrata Jan 30th 2006 - to replace hard coded value of 5000(min PIA to retire)
    Private Property MinimumPIAToRetire() As Decimal
        Get
            If Not Session("MinimumPIAToRetire_C19") Is Nothing Then
                Return (CType(Session("MinimumPIAToRetire_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinimumPIAToRetire_C19") = Value
        End Set
    End Property
    'Shubhrata Jan 30th 2006
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
    Private Property TotalRetirementPlanMoney() As Decimal
        Get
            If Not Session("TotalRetirementPlanMoney_C19") Is Nothing Then
                Return (CType(Session("TotalRetirementPlanMoney_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TotalRetirementPlanMoney_C19") = Value
        End Set
    End Property
    Private Property TotalSavingsPlanMoney() As Decimal
        Get
            If Not Session("TotalSavingsPlanMoney_C19") Is Nothing Then
                Return (CType(Session("TotalSavingsPlanMoney_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TotalSavingsPlanMoney_C19") = Value
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
    Private Property HardshipAvailable() As Decimal
        Get
            If Not Session("HardshipAvailable_C19") Is Nothing Then
                Return (CType(Session("HardshipAvailable_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("HardshipAvailable_C19") = Value
        End Set
    End Property
    Public Property RefundState() As String
        Get
            If Not Session("RefundState_C19") Is Nothing Then
                Return (CType(Session("RefundState_C19"), String))
            Else
                Return "P"

            End If

        End Get
        Set(ByVal Value As String)
            Session("RefundState_C19") = Value
        End Set
    End Property

    'To Get / Set TotalAvailable, incase of HardShip.
    Private Property TotalAvailable() As Decimal
        Get
            If Not Session("TotalAvailable_C19") Is Nothing Then
                Return (CType(Session("TotalAvailable_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TotalAvailable_C19") = Value
        End Set
    End Property
    Private Property RequestedAmount() As Decimal
        Get
            If Not Session("RequestedAmount_C19") Is Nothing Then
                Return (CType(Session("RequestedAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("RequestedAmount_C19") = Value
        End Set
    End Property
    'To Get / Set Tax Rate
    Private Property TDTaxRate() As Decimal
        Get
            If Not Session("TDTaxRate_C19") Is Nothing Then
                Return (CType(Session("TDTaxRate_C19"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TDTaxRate_C19") = Value
        End Set
    End Property
    Private Property inTaxable() As Decimal
        Get
            If Not Session("inTaxable_C19") Is Nothing Then
                Return (CType(Session("inTaxable_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inTaxable_C19") = Value
        End Set
    End Property

    Private Property inNonTaxable() As Decimal
        Get
            If Not Session("inNonTaxable_C19") Is Nothing Then
                Return (CType(Session("inNonTaxable_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inNonTaxable_C19") = Value
        End Set
    End Property

    Private Property inGross() As Decimal
        Get
            If Not Session("inGross_C19") Is Nothing Then
                Return (CType(Session("inGross_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inGross_C19") = Value
        End Set
    End Property

    Private Property inVolAcctsUsed() As Decimal
        Get
            If Not Session("inVolAcctsUsed_C19") Is Nothing Then
                Return (CType(Session("inVolAcctsUsed_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inVolAcctsUsed_C19") = Value
        End Set
    End Property

    Private Property inVolAcctsAvailable() As Decimal
        Get
            If Not Session("inVolAcctsAvailable_C19") Is Nothing Then
                Return (CType(Session("inVolAcctsAvailable_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inVolAcctsAvailable_C19") = Value
        End Set
    End Property

    'Added by Dilip MKT Withdrawal
    Private Property PartialRollOverMinLimit() As Decimal
        Get
            If Not Session("PartialRollOverMinLimit_C19") Is Nothing Then
                Return (CType(Session("PartialRollOverMinLimit_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("PartialRollOverMinLimit_C19") = Value
        End Set
    End Property
    Private Property NumTotalWithdrawalAmount() As Decimal
        Get
            If Not Session("NumTotalWithdrawalAmount_C19") Is Nothing Then
                Return (CType(Session("NumTotalWithdrawalAmount_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("NumTotalWithdrawalAmount_C19") = Value
        End Set
    End Property
    Private Property NumRequestedAmountPartialRollOver() As Decimal
        Get
            If Not Session("RequestedAmountPartialRollOver_C19") Is Nothing Then
                Return (CType(Session("RequestedAmountPartialRollOver_C19"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("RequestedAmountPartialRollOver_C19") = Value
        End Set
    End Property
    Private Property NumPercentageFactorofMoneyPartialRollOver() As Decimal
        Get
            If Not Session("PercentageFactorofMoneyPartialRollOver_C19") Is Nothing Then
                Return (CType(Session("PercentageFactorofMoneyPartialRollOver_C19"), Decimal))
            Else
                Return 1.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("PercentageFactorofMoneyRetirement_C19") = Value
        End Set
    End Property
    'Added by Dilip MKT Withdrawal
    'Added By Ganeswar Sahoo for MKT on 20-10-2009
    Public Property m_DataTableMarketDisplayPayeeGrid() As DataTable
        Get
            Return m_DataTableMarketDisplayPayeeGrid
        End Get
        Set(ByVal Value As DataTable)
            m_DataTableMarketDisplayPayeeGrid = Value
        End Set
    End Property
    'Added by Amit for on 23-11-2009
    Private Property FirstInstallment() As Decimal
        Get
            If Not Session("FirstInstallment_C19") Is Nothing Then
                Return (CType(Session("FirstInstallment_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("FirstInstallment_C19") = Value
        End Set
    End Property
    Private Property DefferredInstallment() As Decimal
        Get
            If Not Session("DefferredInstallment_C19") Is Nothing Then
                Return (CType(Session("DefferredInstallment_C19"), Decimal))
            Else
                Return 0.0
            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("DefferredInstallment_C19") = Value
        End Set
    End Property

    'Added by Parveen on 09-Dec-2009 for the Hardship and VOL withdrawal Issue
    Public Property ForceToVoluntaryWithdrawal() As Boolean
        Get
            If Not Session("ForceToVoluntaryWithdrawal_C19") Is Nothing Then
                Return (CType(Session("ForceToVoluntaryWithdrawal_C19"), Boolean))
            Else
                Return False

            End If

        End Get
        Set(ByVal Value As Boolean)
            Session("ForceToVoluntaryWithdrawal_C19") = Value
        End Set
    End Property


    'Added by Parveen on 15-Dec-2009 for the RolloverOptions
    Public Property RolloverOptions() As String
        Get
            If Not Session("RolloverOptions_C19") Is Nothing Then
                Return (CType(Session("RolloverOptions_C19"), String))
            Else
                Return String.Empty

            End If

        End Get
        Set(ByVal Value As String)
            Session("RolloverOptions_C19") = Value
        End Set
    End Property

    Public Property FirstRolloverAmt() As Decimal
        Get
            If Not Session("FirstRolloverAmt_C19") Is Nothing Then
                Return (CType(Session("FirstRolloverAmt_C19"), Decimal))
            Else
                Return 0.0

            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("FirstRolloverAmt_C19") = Value
        End Set
    End Property

    Public Property TotalRolloverAmt() As Decimal
        Get
            If Not Session("TotalRolloverAmt_C19") Is Nothing Then
                Return (CType(Session("TotalRolloverAmt_C19"), Decimal))
            Else
                Return 0.0

            End If

        End Get
        Set(ByVal Value As Decimal)
            Session("TotalRolloverAmt_C19") = Value
        End Set
    End Property

    'Property Added by Dinesh for gemini 2165
    Public Property MRDTaxrate() As Decimal
        Get
            If Not Session("MRDTaxrate_C19") Is Nothing Then
                Return (CType(Session("MRDTaxrate_C19"), Decimal))
            Else
                Return 0.0

            End If
        End Get
        Set(ByVal value As Decimal)
            Session("MRDTaxrate_C19") = value
        End Set
    End Property
    'Start: AA: 11.20.2015 YRS-AT-2639 Added to use the meta configuration key value 
    Public Property FedaralTaxrate() As Decimal
        Get
            If Not ViewState("FedaralTaxrate_C19") Is Nothing Then
                Return (CType(ViewState("FedaralTaxrate_C19"), Decimal))
            Else
                Return 20.0
            End If
        End Get
        Set(ByVal value As Decimal)
            ViewState("FedaralTaxrate_C19") = value
        End Set
    End Property
    'End: AA: 11.20.2015 YRS-AT-2639 Added to use the meta configuration key value 
    'Added by Parveen on 15-Dec-2009 for the RolloverOptions
    'Added By Ganeswar Sahoo for MKT on 20-10-2009
    'START - Chandra sekar | 2016.03.01 | YRS-AT-2599 - Retirement Processing screen:Annuity purchase, PT and RPT status validation (TrackIT 23822)
    Public Property FundEventStatus() As String
        Get
            Return ViewState("FundEventStatus_C19")
        End Get
        Set(ByVal Value As String)
            ViewState("FundEventStatus_C19") = Value
        End Set
    End Property
    ' START | SR | 2016.06.06 | YRS-AT-2962 - Create property for withdrawal processing fee
    Public Property WithdrawalProcessingFee() As String
        Get
            If Not String.IsNullOrEmpty(ViewState("WithdrawalProcessingFee_C19")) Then
                Return ViewState("WithdrawalProcessingFee_C19")
            Else
                Return "0.0"
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("WithdrawalProcessingFee_C19") = Value
        End Set
    End Property
    ' END | SR | 2016.06.06 | YRS-AT-2962 - Create property for withdrawal processing fee
    ' START | SR | 2016.09.23 | YRS-AT-3164 - Add new parameter to set Flag for special Hardship for Louisiana flood victim 
    Public Property IRSOverride() As Boolean
        Get
            If Not String.IsNullOrEmpty(ViewState("IRSOverride_C19")) Then
                Return ViewState("IRSOverride_C19")
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IRSOverride_C19") = Value
        End Set
    End Property
    ' END | SR | 2016.09.23 | YRS-AT-3164 - Add new parameter to set Flag for special Hardship for Louisiana flood victim 
    ' START | SR | 2016.10.04 | YRS-AT-2962 -  Create property to chech BA/Legacy combined amount is switch ON/OFF
    Public Property BA_Legacy_combined_Amt_Switch As Boolean
        Get
            If Not String.IsNullOrEmpty(ViewState("BA_Legacy_combined_Amt_Switch_C19")) Then
                Return ViewState("BA_Legacy_combined_Amt_Switch_C19")
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("BA_Legacy_combined_Amt_Switch_C19") = Value
        End Set
    End Property
    'END | SR | 2016.10.04 | YRS-AT-2962 -  Create property to chech BA/Legacy combined amount is switch ON/OFF

    'START | SR | 2017.05.09 | 20.2.0 | YRS-AT-3173 - Declare Property 
    Public Property DisplayedParticipantTaxableMoneyMessage As Boolean
        Get
            If Not String.IsNullOrEmpty(ViewState("DisplayedParticipantTaxableMoneyMessage_C19")) Then
                Return ViewState("DisplayedParticipantTaxableMoneyMessage_C19")
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            ViewState("DisplayedParticipantTaxableMoneyMessage_C19") = Value
        End Set
    End Property
    'END | SR | 2017.05.09 | 20.2.0 | YRS-AT-3173 - Declare Property 

    'START | SR | 2017.11.10 | 20.4.2 | YRS-AT-3742 - Declare Property 
    Private Property CurrentBalance() As DataTable
        Get
            If Not (Session("CurrentBalance_C19")) Is Nothing Then
                Return (DirectCast(Session("CurrentBalance_C19"), DataTable))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataTable)
            Session("CurrentBalance_C19") = Value
        End Set
    End Property
    'END | SR | 2017.11.10 | 20.4.2 | YRS-AT-3742 - Declare Property 

    'END - Chandra sekar | 2016.03.01 | YRS-AT-2599 - Retirement Processing screen:Annuity purchase, PT and RPT status validation (TrackIT 23822)

    'START | ML | 2020.04.22 | YRS-AT-4874 | Declare TaxRate For COVID Benefit
    Public Property COVIDTaxrate() As Decimal
        Get
            If Not ViewState("COVIDTaxrate") Is Nothing Then
                Return (CType(ViewState("COVIDTaxrate"), Decimal))
            Else
                Return 10.0
            End If
        End Get
        Set(ByVal value As Decimal)
            ViewState("COVIDTaxrate") = value
        End Set
    End Property

    Public Property COVIDFixedTaxrate() As Decimal
        Get
            If Not ViewState("COVIDFixedTaxrate") Is Nothing Then
                Return (CType(ViewState("COVIDFixedTaxrate"), Decimal))
            Else
                Return 10.0
            End If
        End Get
        Set(ByVal value As Decimal)
            ViewState("COVIDFixedTaxrate") = value
        End Set
    End Property
    'END | ML | 2020.04.22 | YRS-AT-4874 | Declare TaxRate For COVID Benefit
    'START : SC | 2020.04.05 | YRS-AT-4874 | Get covid requested and used amounts
    Public Property CovidAmountRequested() As Decimal
        Get
            If Not ViewState("CovidAmountRequested") Is Nothing Then
                Return (CType(ViewState("CovidAmountRequested"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            ViewState("CovidAmountRequested") = Value
        End Set
    End Property
    Public Property CovidAmountUsed() As Decimal
        Get
            If Not ViewState("CovidAmountUsed") Is Nothing Then
                Return (CType(ViewState("CovidAmountUsed"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            ViewState("CovidAmountUsed") = Value
        End Set
    End Property
    Public Property CovidAmountLimit() As Decimal
        Get
            If Not ViewState("CovidAmountLimit") Is Nothing Then
                Return (CType(ViewState("CovidAmountLimit"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            ViewState("CovidAmountLimit") = Value
        End Set
    End Property
    Public Property CurrentAccountTotalWithInterest() As Decimal
        Get
            If Not ViewState("CurrentAccountTotalWithInterest") Is Nothing Then
                Return (CType(ViewState("CurrentAccountTotalWithInterest"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            ViewState("CurrentAccountTotalWithInterest") = Value
        End Set
    End Property
    'END : SC | 2020.04.05 | YRS-AT-4874 | Get covid requested and used amounts
#End Region
    'Start:AA:11.17.2015 YRS-AT-2639 Added to use the enum for the rolloveroptions 
    Private Enum enumRolloverOptions
        None
        Full
        Taxable
        [Partial]
    End Enum
    'End:AA:11.17.2015 YRS-AT-2639 Added to use the enum for the rolloveroptions 
    'PRIYA 04-June-08 - added following function to validate Daily Interest and Last Payroll Date
    Private Function ValidateDailyInterest() As String
        Dim l_string_Message As String = ""
        Try
            'Priya 05-Dec-2008 Changed ValidateDailyInterest method of BO class as per discussion with Nikunj and Hafiz
            If YMCARET.YmcaBusinessObject.RefundRequestRegularBOClass.ValidateDailyInterest() = 0 Then '(PersonID, FundID) = 0 Then
                l_string_Message = "Interest records do not exist. Do you want to continue with withdrawal?"
            Else
                l_string_Message = ""
            End If
            Return l_string_Message
        Catch ex As Exception
            'Commented by Mohammed Hafiz on 21-Apr-2008
            'Response.Redirect("ErrorPageForm.aspx?Message=" & Server.UrlDecode(ex.Message), False)

            'Added by Mohammed Hafiz on 21-Apr-2008
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" & Server.UrlEncode(ex.Message), False)
        End Try


    End Function
    Private Function ValidateLastPayrollDate() As String
        Dim l_string_LastPayRollDateValidation As String = ""
        Try
            If YMCARET.YmcaBusinessObject.RefundRequestRegularBOClass.ValidateLastPayrollDate(PersonID, FundID) = 0 Then
                'l_string_LastPayRollDateValidation = "Not all contributions have been received\funded, this process cannot be completed."
                'Priya 12-Dec-2008 :Change Validation message of termination date as per Ragesh mail on December 09, 2008 
                'l_string_LastPayRollDateValidation = "Not all contributions have been received\funded. Do you want to continue with withdrawal?"
                'START - Chandra sekar | 2016.03.01 | YRS-AT-2599 - Retirement Processing screen:Annuity purchase, PT and RPT status validation (TrackIT 23822)
                If Me.FundEventStatus = "PT" Or Me.FundEventStatus = "RPT" Then
                    l_string_LastPayRollDateValidation = "We have not received the final contribution based upon the TD contract termination date.  Do you wish to continue?"
                Else
                    'END - Chandra sekar | 2016.03.01 | YRS-AT-2599 - Retirement Processing screen:Annuity purchase, PT and RPT status validation (TrackIT 23822)
                    l_string_LastPayRollDateValidation = "We have not received the final contribution based upon the termination date.  Do you wish to continue?"
                End If
            Else
                l_string_LastPayRollDateValidation = ""
            End If
            Return l_string_LastPayRollDateValidation
        Catch ex As Exception
            'Commented by Mohammed Hafiz on 21-Apr-2008
            'Response.Redirect("ErrorPageForm.aspx?Message=" & Server.UrlDecode(ex.Message), False)

            'Added by Mohammed Hafiz on 21-Apr-2008
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" & Server.UrlEncode(ex.Message), False)
        End Try

    End Function
    'PRIYA 04-June-08
    'IB:Added on 29/Sep/2010 YRS 5.0-1181 :Add validation for Hardhship withdrawal - No YMCA contact
    Private Function ValidateHardshipRefundProcess() As Boolean
        Dim l_string_Message As String = ""
        Dim l_DataSet_YmcaContact As DataSet
        Dim l_EmailAddr As String
        Dim l_bool_Email As Boolean
        Try
            l_bool_Email = True
            l_DataSet_YmcaContact = YMCARET.YmcaBusinessObject.RefundRequestRegularBOClass.ValidateRefundProcess(Session("RefundRequestID_C19"))
            If (isNonEmpty(l_DataSet_YmcaContact)) Then

                For Each l_DataRow In l_DataSet_YmcaContact.Tables(0).Rows()
                    If l_DataRow("EmailAddr").GetType.ToString() = "System.DBNull" Then
                        l_bool_Email = False
                    End If

                Next
            Else
                l_bool_Email = False
            End If


            Return l_bool_Email
        Catch ex As Exception
            'Commented by Mohammed Hafiz on 21-Apr-2008
            'Response.Redirect("ErrorPageForm.aspx?Message=" & Server.UrlDecode(ex.Message), False)

            'Added by Mohammed Hafiz on 21-Apr-2008
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" & Server.UrlEncode(ex.Message), False)
        End Try


    End Function
    Private Function isNonEmpty(ByRef ds As DataSet) As Boolean
        If ds Is Nothing Then Return False
        If ds.Tables.Count = 0 Then Return False
        If ds.Tables(0).Rows.Count = 0 Then Return False
        Return True
    End Function
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        Dim MRDforPartialorMarket As Boolean = False
        ' HttpContext.Current.Session.Timeout = 60 '2677 bug Ashutosh
        Me.TextboxTaxRate.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
        Me.TextboxTaxRate.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")

        'BT-724 Refund processing screen Tax Rate allowing -Ve value for MRD
        Me.TextboxMinDistTaxRate.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
        'START : ML | 05.07.2020 | YRS-AT-4874 | Added format control 
        Me.txtTaxRateC19.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
        Me.txtTaxRateC19.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
        Me.txtNewCovidAmount.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
        'END : ML | 05.07.2020 | YRS-AT-4784 | Added format control 
        If Me.RefundType = "HARD" Then
            Me.TextboxRequestAmount.Attributes.Add("onblur", "javascript: TextboxRequestAmount_Change();")
            Me.TextboxRequestAmount.Attributes.Add("onkeypress", "javascript:HandleAmountFiltering(this);")
            Me.TextboxHardShip.Attributes.Add("onblur", "javascript: TextboxRequestAmount_Change();")
            Me.TextboxHardShip.Attributes.Add("onkeypress", "javascript:HandleAmountFiltering(this);")
        End If
        Try
            If Not IsPostBack Then
                Session("DailyInterestValidation_C19") = Nothing
                Session("PayrollValidation_C19") = Nothing
                Session("NeedsNewRequests_C19") = Nothing
                Session("HardshipAmountWithInterest_C19") = Nothing
                Session("CalculatedDataTableForCurrentAccounts_Initial_C19") = Nothing
                'Neeraj BT-563
                Session("MinimumDistributionTable_C19") = Nothing
                Session("dtMrdRecords_C19") = Nothing
                Session("MinimumDistributionAmount_C19") = Nothing
                'IB:BT:800 Get participant's alll mrd record.To check any unsatisfied mrd record is pending at the time of saving 
                Session("dtParticipantAllMrdRecords_C19") = Nothing
                Session("PriorYearRMDRecords_C19") = Nothing
                ' START | SR | 2016.01.22 | YRS-AT-2664 | Clear session
                Session("CurrentAccountsWithMRD_C19") = Nothing
                Session("MinimumDistributionTable_temp_C19") = Nothing
                ' END | SR | 2016.01.22 | YRS-AT-2664 | Clear session

                'START : ML | 2020.05.06 | YRS-AT-4874 | REset Session Variable for COVID calculation
                Session("COVIDProrateAccountDataTable") = Nothing
                'END : ML | 2020.05.06 | YRS-AT-4874 | REset Session Variable for COVID Ccalculation

                ' START | SR | 2019.08.02 | YRS-AT-4498 | Clear session
                ClearHardshipSessions()
                ' END | SR | 2016.01.22 | YRS-AT-4498 | Clear session
                CurrentBalance = Nothing ' SR | 2017.11.22 | YRS-AT-3742 | Clear current balance
                Me.RefundState = "P"
                Me.LoadRefundConfiguration()
                objRefundProcess.LoadRefundConfiguration() 'MMR | YRS-AT-4498 | Get hardship configuration values. 
                Me.LoadAccountContribution()
                Me.LoadInformation()
                BA_Legacy_combined_Amt_Switch = objRefundProcess.BA_Legacy_combined_Amt_Switch_ON() 'SR | 2016.10.05 | YRS-AT-2962 - Get BA/Legacy combined amt switch status
                Me.CopyAccountContributionTableForCurrentAccounts()
                'Priya 5-Jan-2008 YRS 5.0-634 : SHIRA refund request for non-vested participant 
                'added Or Me.RefundType = "SHIRA"
                If Me.RefundType = "REG" Or Me.RefundType = "PERS" Or Me.RefundType = "SHIRA" Then
                    Me.DoRegularRefundForCurrentAccounts()
                ElseIf Me.RefundType = "VOL" Then
                    Me.DoVoluntryRefundForCurrentAccounts()
                ElseIf Me.RefundType = "HARD" Then
                    Me.DoHardshipRefundForCurrentAccount()
                Else
                    Me.DoGeneralFunctionForCurrentAccounts()
                End If

                'Plan Split Changes
                Me.TextboxPlanChosen.Text = Me.PlanTypeChosen
                'Plan Split Changes

                Session("ConvertHardToVol_C19") = Nothing
                Me.SetVolWithdrawalTotal()
                '' Populate Contribution Table
                If Me.RefundType = "HARD" Then
                    Me.Label1.Visible = True
                    Me.Label2.Visible = True
                    Me.Label3.Visible = True
                    Me.Label5.Visible = True
                    Me.TextboxRequestAmount.Visible = True
                    Me.TextboxTDAvailableAmount.Visible = True
                    Me.TextboxHardShipTaxRate.Visible = True
                    Me.TextboxHardShipAmount.Visible = True
                    Me.textboxHardShipNonTaxableAmount.Visible = True 'MMR | 2017.07.30 | YRS-AT-3870 | Unhiding non-taxable hardship control
                    Me.TextboxHardShip.Visible = True
                    Me.TextboxHardShipNet.Visible = True
                    Me.TextboxVoluntaryWithdrawalTotal.Visible = True
                    EnableDisableControlsForHardShip()
                End If
                CreateExpiredRequestWarning()     'SR:2014.09.17-BT 2633/YRS 5.0-2403 -  Remove expiration dates from withdrawal requests in YRS 
            End If

            If Me.IsPostBack Then
                Me.SessionIsRefundProcessPopupAllowed = False
                'New Modification for Market Based Withdrawal Amit Nigam 05/Nov/2009        
                trHardshipVoluntary.Visible = False
                'New Modification for Market Based Withdrawal Amit Nigam 05/Nov/2009    
                If Me.RefundType = "HARD" Then

                    'Commented and added by Ganeswar for Hardship Rollover on 28-05-2009
                    'LoadValuesForProcessedAmounts(False)
                    If Me.RefundType = "HARD" And TextboxVoluntaryWithdrawalTotal.Text > "0.00" Then
                        LoadValueForhardShipRollOver()
                        'DoFinalCalculation()
                    Else
                        LoadValuesForProcessedAmounts(False)
                        EnableDisableControlsForHardShip()
                    End If
                    'Commented and added by Ganeswar for Hardship Rollover on 28-05-2009
                End If
            Else
                If Not Session("Title_C19") Is Nothing Then
                    '**********Code Changed by Ashutosh on 07-June-06**********
                    'I have changed the code in aspx page <td class="Td_HeadingFormContainer" align="left">
                    '"Regular Withdrawal Request Processing" to New Dynamic binding of text.
                    'Purpose:For Dynamic Coming Of Regular ,Special.
                    '---------------------------------
                    Dim string_temp As String
                    Dim string_header As String
                    Dim int_Index As Integer

                    string_temp = Session("Title_C19").ToString()
                    int_Index = string_temp.IndexOf(" ")
                    string_header = string_temp.Substring(0, int_Index)

                    string_header = string_header + " Withdrawal Request Processing"
                    int_Index = string_temp.IndexOf("-")
                    string_temp = string_temp.Substring(int_Index)
                    Me.LabelTitle.Text = string_header + string_temp
                    '***************************End Ashu**********************
                End If
                Me.LoadDeafultValues()
                Me.LoadRequestedAccounts()
                Me.LoadPIA()
                ' integrate the below 7 functions into the class
                'Me.LoadSchemaRefundTable()
                'Me.LoadDeductions() ' ask datagrid binding
                'Me.CalculateMinimumDistributionAmount()
                'Me.CreatePayees()
                'Me.LoadRefundableDataTable()
                'Me.LoadAccountBreakDown()
                'Me.LoadCalculatedTableForPersonalMonies()
                objRefundProcess.LoadSchemaRefundTable()
                objRefundProcess.LoadDeductions(Me.DatagridDeductions) ' ask datagrid binding
                'objRefundProcess.CalculateMinimumDistributionAmount(MRDforPartialorMarket)

                'neeraj 
                objRefundProcess.CalculateMinimumDistributionAmount()
                'If Me.RefundType <> "HARD" Then
                If Me.RefundType <> "HARD" Then
                    Session("CalculatedDataTableForCurrentAccounts_C19") = YMCARET.YmcaBusinessObject.RefundRequest.GetTransactionsForMarket(Me.FundID, Session("RefundRequestID_C19"))
                End If
                UpdateCovidRequestedAmountWithCurrentInterest() 'SC | 05/12/2020 | YRS-AT-4874 | Get Covid Requested Amount inclusive of interest for requested withdrawal
                objRefundProcess.CalculateCOVIDDistributionAmount(CDec(txtCovidAmountRequested.Text), Me.COVIDTaxrate) ' ML : 05/05/2020 | YRS-AT-4874 | Calculate COVID Distribution
                objRefundProcess.CreatePayees()
                objRefundProcess.LoadRefundableDataTable()
                objRefundProcess.LoadAccountBreakDown()
                objRefundProcess.LoadCalculatedTableForPersonalMonies()
            End If


            If Not IsPostBack Then
                Me.CopyAccountContributionTableForRefundable()
                If Me.RefundType = "REG" Or Me.RefundType = "VOL" Or Me.RefundType = "HARD" Then
                    Me.DoRefundForRefundAccounts()
                End If
                Me.LoadPayeesDataGrid()
                Me.LoadPayee1ValuesIntoControls()
                Me.LoadPayee2ValuesIntoControls()
                Me.LoadPayee3ValuesIntoControls()
                Me.EnableMinimumDistributionControls()

                ' BT:800 -If current balance is greater than $5000 and requested amount<5000 and  not single mrd processed then show informative message
                If (Me.MinimumDistributionAmount > 0.01) AndAlso (Me.RefundType <> "MRD" And Me.RefundType <> "PART") AndAlso ( _
                    (DirectCast(Session("RequestedAccounts_C19"), DataTable).Select("PlanType = 'RETIREMENT'").Length > 0 AndAlso DirectCast(Session("dtMrdRecords_C19"), DataTable).Select("PlanType = 'RETIREMENT' and IsGreater5kBalance=true").Length > 0 AndAlso DirectCast(Session("RequestedAccounts_C19"), DataTable).Compute("SUM(Total)", "PlanType = 'RETIREMENT'") <= 5000.0) _
                    OrElse (DirectCast(Session("RequestedAccounts_C19"), DataTable).Select("PlanType = 'SAVINGS'").Length > 0 AndAlso DirectCast(Session("dtMrdRecords_C19"), DataTable).Select("PlanType = 'SAVINGS' and IsGreater5kBalance=true").Length > 0 AndAlso DirectCast(Session("RequestedAccounts_C19"), DataTable).Compute("SUM(Total)", "PlanType = 'SAVINGS'") <= 5000.0) _
                ) Then

                    MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "The participant has the option to withdraw only the RMD amount.  To exercise that option, you must cancel this request and create a new one.", MessageBoxButtons.OK)
                End If

                'Added to genrate the current account data in Payee table for MRD Issue=02-03-2010 
                'comment person age restication for MRD person and If MRd amount>0 then allow MRD 
                'If (Me.RefundType <> "HARD" And Me.PersonAge >= 70.5) Or Me.RefundType = "PART" Then
                If (Me.RefundType <> "HARD" And Me.MinimumDistributionAmount > 0.01) Or Me.RefundType = "PART" Then
                    MRDforPartialorMarket = True
                    Session("CalculatedDataTableForCurrentAccounts_C19") = YMCARET.YmcaBusinessObject.RefundRequest.GetTransactionsForMarket(Me.FundID, Session("RefundRequestID_C19"))
                    'neeraj()
                    'objRefundProcess.CalculateMinimumDistributionAmount(MRDforPartialorMarket)
                    objRefundProcess.CalculateMinimumDistributionAmount()
                    UpdateCovidRequestedAmountWithCurrentInterest() 'SC | 05/12/2020 | YRS-AT-4874 | Get Covid Requested Amount inclusive of interest for requested withdrawal
                    objRefundProcess.CalculateCOVIDDistributionAmount(CDec(txtCovidAmountRequested.Text), Me.COVIDTaxrate) ' ML : 05/05/2020 | YRS-AT-4874 | Calculate COVID Distribution
                    objRefundProcess.CreatePayees()
                    objRefundProcess.LoadRefundableDataTable()
                    objRefundProcess.LoadAccountBreakDown()
                    objRefundProcess.LoadCalculatedTableForPersonalMonies()
                    Me.CopyAccountContributionTableForRefundable()
                    If Me.RefundType = "REG" Or Me.RefundType = "VOL" Or Me.RefundType = "HARD" Then
                        Me.DoRefundForRefundAccounts()
                    End If
                    Me.LoadPayeesDataGrid()
                    Me.LoadPayee1ValuesIntoControls()
                    Me.LoadPayee2ValuesIntoControls()
                    Me.LoadPayee3ValuesIntoControls()
                    Me.EnableMinimumDistributionControls()


                End If
                GetWithdrawalOptionsVer2() 'AA:11.17.2015 YRS-AT-2639 Added to call fill refrequest options details into controls
                'Added to genrate the current account data in Payee table for MRD Issue=02-03-2010   
                Me.DoFinalCalculation()
                ' START | SR | 2017.12.12 | YRS-AT-3742 - Since now, RMD amount will be adjusted based on available non taxable amount, RMD taxable amount should not be rollover at any scenario. Hence, below line of codes are commented code & by default, set flag(ViewState("AllowRMDAmountToBeRollover")) value as false 
                '' START | SR | 2016.01.22 | YRS-AT-2664 - Check if Taxable RMD amount can be rollover
                'If IsRMDTaxableAmountCanBeRollover() Then
                '    ViewState("AllowRMDAmountToBeRollover") = "True"
                'Else ' SR | 2017.05.09 | 20.2.0 | YRS-AT-3173 - Set Viewstate value from Nothing
                '    ViewState("AllowRMDAmountToBeRollover") = "False"
                'End If
                '' END | SR | 2016.01.22 | YRS-AT-2664 - Check if Taxable RMD amount can be rollover
                ViewState("AllowRMDAmountToBeRollover") = "False"
                ' END | SR | 2017.12.12 | YRS-AT-3742 - Since now, RMD amount cab be rollover or not will be decided while adjusting RMD amount against non-taxable amount in Refunds.vb method(DoMinimumDistributionCalculation) . hence, below line of codes are commented code.
                'START: SB | 2017.10.23 | YRS-AT-3324 | Added method to validate withdrawal restricitions for RMD eligbile participants
                If HelperFunctions.isNonEmpty(Session("ReasonForRestriction_C19")) Then
                    DivMainMessage.InnerHtml = Session("ReasonForRestriction_C19").ToString()
                    DivMainMessage.Visible = True
                    ButtonSave.Enabled = False
                    Exit Sub
                End If
                'END: SB | 2017.10.23 | YRS-AT-3324 | Added method to validate withdrawal restricitions for RMD eligbile participants

                ' START | SR | 2017.11.23 | YRS-AT-3742 | Disable Rollover All option for RMD eligible participant.
                If (Not Session("MinimumDistributionTable_C19") Is Nothing) Then
                    RadioButtonRolloverAll.Enabled = False
                End If
                ' END | SR | 2017.11.23 | YRS-AT-3742 | Disable Rollover All option for RMD eligible participant.
            End If

            'Commented code deleted by SG: 2012.03.16

            If Me.RefundType = "HARD" Then
                Me.LoadHardShipvalues()
            End If

            Me.EnableDisableControls()
            Me.EnableDisableSaveButton()

            'Commented code deleted by SG: 2012.03.16

            'Code added by ashutosh 18/04/06*****
            If Not Session("ds_PrimaryAddress_C19") Is Nothing Then
                LoadFromPopUp()
            End If
            '************************************
            'Code Added by Ashutosh on 18-04-2006*************
            If Me.IsPostBack = False Then
                Session("ds_PrimaryAddress_C19") = Nothing
            End If
            '***************************
            'Shubhrata & Priya June 4th,2008 - Changes made to validate Daily Interest and Last Payroll Date
            Dim l_string_LastPayRollDateValidation As String = ""
            If Me.IsPostBack = True Then
                'START: SG: 2012.03.21: BT-1010
                If Not Session("PriorYearRMDRecords_C19") Is Nothing Then
                    If Not Request.Form("Yes") Is Nothing Then
                        If Request.Form("Yes").Trim.ToUpper = "YES" AndAlso Session("PriorYearRMDRecords_C19") = "Yes" Then
                            DoBtnSaveProcessing()
                            Session("PriorYearRMDRecords_C19") = Nothing
                            'If Session("DailyInterestValidation") = True OrElse Session("PayrollValidation") = True Then 'SP 2014.05.06 BT-2503\YRS 5.0-2354 -Commented (it was assigned Yes and checked true)
                            If Session("DailyInterestValidation_C19") = True OrElse Session("PayrollValidation_C19") = "Yes" Then 'SP 2014.05.06 BT-2503\YRS 5.0-2354 -Added
                                Exit Sub
                            End If
                        End If
                    End If
                End If
                'END: SG: 2012.03.21: BT-1010

                'Priya 2008-Oct-03 :Verifying Last Payroll date validation. & if user wants to continue with it then refunds process should proceed further. as per hafiz mail on 1stOct2008
                If Session("PayrollValidation_C19") = "Yes" Then
                    If Not Request.Form("Yes") Is Nothing Then
                        Session("PayrollValidation_C19") = Nothing
                        'Uncommented By SG: 2012.06.22
                        DoSaveProcessing()
                    End If
                End If
                'end '2008-Oct-03

                If Not Session("DailyInterestValidation_C19") Is Nothing Then
                    If Not Request.Form("Yes") Is Nothing Then
                        'If Request.Form("Yes").Trim.ToUpper = "YES" AND Session("DailyInterestValidation") = True Then
                        If Request.Form("Yes").Trim.ToUpper = "YES" And Session("DailyInterestValidation_C19") = True Then
                            If Me.RefundType.Trim.ToUpper = "PERS" Or Me.RefundType.Trim.ToUpper = "REG" Then
                                l_string_LastPayRollDateValidation = ValidateLastPayrollDate()
                                If l_string_LastPayRollDateValidation = "" Then
                                    DoSaveProcessing()
                                    Session("DailyInterestValidation_C19") = Nothing
                                    Session("PayrollValidation_C19") = Nothing
                                Else
                                    MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", l_string_LastPayRollDateValidation, MessageBoxButtons.YesNo)
                                    Session("PayrollValidation_C19") = "Yes"
                                    Session("DailyInterestValidation_C19") = Nothing ' SC | 05/15/2020 | YRS-AT-4874 | Clear sessionas it causes runtime error due to multiple msgbox 
                                    Exit Sub
                                    'DoSaveProcessing()
                                End If
                            Else
                                Session("DailyInterestValidation_C19") = Nothing
                                Session("PayrollValidation_C19") = Nothing
                                DoSaveProcessing()
                            End If
                        End If
                    End If

                End If
            End If
            'Added By Ganeswar on july 10th 2009
            If TextboxTDAvailableAmount.Text > "0.00" And Me.RefundType = "HARD" Then
                Me.TextboxHardShipTaxRate.ReadOnly = False
                Me.TextboxHardShip.ReadOnly = False
            Else
                Me.TextboxHardShipTaxRate.ReadOnly = True
                Me.TextboxHardShip.ReadOnly = True
            End If
            If TextboxHardShip.Text = "0.00" Then
                Me.l_dec_TDRequestedAmount = 0
                Me.l_dec_TDAmount = 0
            End If
            'Added By Ganeswar on july 10th 2009
            'Shubhrata & Priya June 4th,2008 - Daily Interest Validation
            DisableRadioButtons()

            'START | SR | 2017.05.09 | 20.2.0 | YRS-AT-3173 - If user select NO to include taxable money in participant account then revert taxable money option in rollover
            If Not Request.Form("No") Is Nothing Then
                If (DisplayedParticipantTaxableMoneyMessage = True AndAlso Request.Form("No").Trim.ToUpper = "NO") Then
                    Me.DoNone()
                    RadioButtonTaxableOnly.Checked = False
                    Me.RadioButtonNone.Checked = True
                    DisplayedParticipantTaxableMoneyMessage = False
                End If
            End If
            'END | SR | 2017.05.09 | 20.2.0 | YRS-AT-3173 - If user select NO to include taxable money in participant account then revert taxable money option in rollover
            If Not Page.IsPostBack Then
                RangeValidatorNewCovidAmount.MaximumValue = GetMaxPossibleCovidAmountAfterRMD() 'SN | YRS-AT-4874 | Assigned max limit of range validator
                RangeValidatorNewCovidAmount.ErrorMessage =
                        YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_COVID_INSUFFICIENT_AMT).DisplayText.Replace("$$AvailableAmount$$", RangeValidatorNewCovidAmount.MaximumValue)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("RefundProcessing_C19_Page_Load", ex) 'PK | 10/01/2019 | YRS-AT-2670 | Added exception logging
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" & Server.UrlDecode(ex.Message), False)
        End Try
    End Sub


    'Added Ganeswar 10thApril09 for HardShip RollOver Phase-V /begin
    Private Sub EnableDisableControlsForHardShip()
        If TextboxVoluntaryWithdrawalTotal.Text > "0.00" And Me.RefundType = "HARD" Then
            'Commented code deleted by SG: 2012.03.16

            'Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
            Me.CheckboxVoluntaryRollover.Enabled = True
            Me.CheckBoxRollovers.Enabled = True
            'Added till Here
            LabelVoluntaryAccounts.Visible = True
        Else
            'Commented code deleted by SG: 2012.03.16
            Me.CheckboxVoluntaryRollover.Visible = False 'Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
            LabelVoluntaryAccounts.Visible = False
        End If
        'Commented code deleted by SG: 2012.03.16
    End Sub
    'Added Ganeswar 10thApril09 for HardShip RollOver Phase-V /End

    'Function made by ashutosh 18/04/06****************
    Private Function EnableDisableControls()

        Try

            If Me.RefundType <> "REG" Then
                Me.RadiobuttonPersonalMoniesYes.Visible = False
                Me.RadiobuttonPersonalMoniesNo.Visible = False
            ElseIf Me.RefundType = "REG" Then
                If Me.PersonalAmount > 0 Then
                    Me.RadiobuttonPersonalMoniesYes.Visible = True
                    Me.RadiobuttonPersonalMoniesNo.Visible = True
                Else
                    Me.RadiobuttonPersonalMoniesNo.Checked = True
                    Me.RadiobuttonPersonalMoniesYes.Visible = True
                    Me.RadiobuttonPersonalMoniesNo.Visible = True
                    Me.RadiobuttonPersonalMoniesYes.Enabled = False
                    Me.RadiobuttonPersonalMoniesNo.Enabled = False

                End If
            End If
        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Function LoadFromPopUp()
        Dim l_str_zip As String

        If Not Session("ds_PrimaryAddress_C19") Is Nothing Then
            Dim dataset_AddressInfo As DataSet
            Dim l_DataSet As DataSet
            Dim l_DataTable As DataTable
            Dim datarow_Row As DataRow
            Dim datarow_NewRow As DataRow
            dataset_AddressInfo = (DirectCast(Session("ds_PrimaryAddress_C19"), DataSet))
            datarow_Row = dataset_AddressInfo.Tables("Address").Rows(0)
            'Start:AA:24.09.2013 : BT-1501: Commented For Displaying address with address control not with textboxes
            'TextboxAddress1.Text = datarow_Row.Item("addr1").ToString.Replace(",", "")
            'TextboxAddress2.Text = datarow_Row.Item("addr2").ToString.Replace(",", "")
            'TextboxAddress3.Text = datarow_Row.Item("addr3").ToString.Replace(",", "")
            'TextboxCity1.Text = datarow_Row.Item("city").ToString.Replace(",", "")
            ''TextboxCity2.Text = datarow_Row.Item("state")
            ''TextboxCity3.Text = datarow_Row.Item("Zip")
            'Me.TextBoxState.Text = datarow_Row.Item("StateName").ToString.Replace(",", "")
            'Me.TextBoxCountry.Text = datarow_Row.Item("CountryName").ToString.Replace(",", "")

            'If datarow_Row.Item("CountryName").ToString = "CANADA" Then
            '    l_str_zip = datarow_Row.Item("Zip").Replace(",", "")
            '    Me.TextBoxZip.Text = l_str_zip.Substring(0, 3) + " " + l_str_zip.Substring(3, 3)
            'Else
            '    Me.TextBoxZip.Text = datarow_Row.Item("Zip").ToString.Replace(",", "")
            'End If
            'End:AA:24.09.2013 : BT-1501: Commented For Displaying address with address control not with textboxes

            'AA:24.09.2013 : BT-1501: Displaying address with address control not with textboxes
            AddressWebUserControl1.LoadAddressDetail(dataset_AddressInfo.Tables("Address").Select("isPrimary = True"))

            dataset_AddressInfo = Nothing
        End If
    End Function
    '*******************************************************
    Private Function DoRefundForRefundAccounts()

        'Commented code deleted by SG: 2012.03.16
        Dim l_DataTable As DataTable

        Try
            'Hafiz 03Feb06 Cache-Session
            'Commented code deleted by SG: 2012.03.16
            l_DataTable = DirectCast(Session("CalculatedDataTableForRefundable_C19"), DataTable)
            'Hafiz 03Feb06 Cache-Session

            If Not l_DataTable Is Nothing Then
                '' Do calucation of Current Accounts.

                '' Reset YMCA Amount
                Me.YMCAAvailableAmount = 0


                '' To Avoid the Earlier Changes. 
                l_DataTable.RejectChanges()
                If Me.RefundType = "REG" Then
                    l_DataTable = Me.DoFullRefund(l_DataTable)
                ElseIf Me.RefundType = "VOL" Then
                    l_DataTable = Me.DoVoluntryRefund(l_DataTable)
                    Session("CalculatedDataTableForRefundable_C19") = l_DataTable
                ElseIf Me.RefundType = "HARD" Then
                    Me.DoHardshipRefund(l_DataTable)

                End If
            End If
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Function

    Private Function CopyAccountContributionTableForRefundable()

        Dim l_AccountContributionTable As DataTable
        Dim l_CalculationDataTable As DataTable

        Dim l_DataColumn As DataColumn
        Dim l_DataRow As DataRow

        Try

            l_AccountContributionTable = Session("AccountContribution_C19")

            If Not l_AccountContributionTable Is Nothing Then

                l_CalculationDataTable = l_AccountContributionTable.Clone
                ' Add a column Into the Table to allow selected.
                l_DataColumn = New DataColumn("Selected")
                l_DataColumn.DataType = System.Type.GetType("System.Boolean")
                l_DataColumn.AllowDBNull = True

                l_CalculationDataTable.Columns.Add(l_DataColumn)
                'Copy all Values into Calculation Table 


                For Each l_DataRow In l_AccountContributionTable.Rows

                    If Not (l_DataRow("AccountType").GetType.ToString = "System.DBNull") Then

                        If Not (CType(l_DataRow("AccountType"), String) = "Total") Then
                            l_CalculationDataTable.ImportRow(l_DataRow)

                        End If
                    End If
                Next

                Session("CalculatedDataTableForRefundable_C19") = l_CalculationDataTable


            End If

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Function

    Private Function DoRegularRefundForCurrentAccounts()

        'Commented code deleted by SG: 2012.03.16
        Dim l_DataTable As DataTable

        Try
            'Added Ganeswar 10thApril09 for BA Account Phase-V /begin
            'Commented code deleted by SG: 2012.03.16

            l_DataTable = DirectCast(Session("CalculatedDataTableForCurrentAccounts_Initial_C19"), DataTable)
            'Added Ganeswar 10thApril09 for BA Account Phase-V /End
            If Not l_DataTable Is Nothing Then
                '' Do calucation of Current Accounts.
                l_DataTable = Me.DoFullRefund(l_DataTable)
                Me.CalculateTotal(l_DataTable)
                ' If Me.IsPostBack Then
                Me.LoadCalculatedTableForCurrentAccounts()
                ' End If
                'Commented code deleted by SG: 2012.03.16
            End If

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try


    End Function

    Private Function DoGeneralFunctionForCurrentAccounts()

        Dim l_ContributionDataTable As DataTable
        Dim l_DataRow As DataRow

        Dim l_UserSide As Boolean
        Dim l_YMCASide As Boolean

        Dim l_AccountType As String


        Try

            l_ContributionDataTable = Session("CalculatedDataTableForCurrentAccounts_C19")

            If Not l_ContributionDataTable Is Nothing Then

                Session("CalculatedDataTableForCurrentAccounts_C19") = l_ContributionDataTable

                Me.CalculateTotal(l_ContributionDataTable)

                Me.LoadCalculatedTableForCurrentAccounts()

                'Me.SetSelectedIndex(Me.DatagridCurrentAccts, l_ContributionDataTable)

            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function EnableMinimumDistributionControls()
        'BT:726:Harshala-Application allowing to create MRD refund for Active participant-START : Added two condtions for status type.
        'If Me.MinimumDistributionAmount > 0.01 Then
        'If Me.SessionStatusType.Trim() <> "AE" AndAlso Me.SessionStatusType.Trim() <> "RA" AndAlso Me.MinimumDistributionAmount > 0.01 Then
        '--SR | 2016.08.25 | YRS-AT-3084 - QD participant do not have employment record hence allow them to bypass employment condition for RMD process.
        If ((objRefundProcess.IsTerminatedEmployment Or Me.SessionStatusType.Trim() = "QD") AndAlso Me.MinimumDistributionAmount > 0.01) Then 'SR/2015.06.24-BT 2903/YRS-AT-2542 - YRS bug discovered in which RMD (Required Minimum Distribution) records are being marked as eligible-yes for participants with pre-eligible (PE or RE) Fund statuses.
            'BT:726:Harshala-Application allowing to create MRD refund for Active participant-END.
            Me.TextboxMinDistTaxRate.Visible = True
            Me.TextboxMinDistAmount.Visible = True
            Me.TextboxMinDistTaxable.Visible = True
            Me.TextboxMinDistNet.Visible = True
            Me.TextboxMinDistNonTaxable.Visible = True

        Else

            Me.TextboxMinDistTaxRate.Visible = False
            Me.TextboxMinDistAmount.Visible = False
            Me.TextboxMinDistTaxable.Visible = False
            Me.TextboxMinDistNet.Visible = False
            Me.TextboxMinDistNonTaxable.Visible = False

        End If

        Me.LoadMinDistributionValuesIntoControls()
        Me.LoadCOVIDValuesIntoControls() ' ML : 05/05/2020 | YRS-AT-4874 | Display COVID Controls
    End Function

    Private Function LoadMinDistributionValuesIntoControls()

        Dim l_DataTable As DataTable

        Dim l_Decimal_Taxable As Decimal
        Dim l_Decimal_NonTaxable As Decimal
        Dim l_Decimal_Tax As Decimal
        Dim l_Decimal_Net As Decimal

        Try

            l_DataTable = DirectCast(Session("MinimumDistributionTable_C19"), DataTable)


            If Not l_DataTable Is Nothing Then

                If l_DataTable.Rows.Count > 0 Then
                    l_Decimal_Taxable = CType(l_DataTable.Compute("SUM (Taxable)", ""), Decimal)
                    l_Decimal_NonTaxable = CType(l_DataTable.Compute("SUM (NonTaxable)", ""), Decimal)
                    l_Decimal_Tax = CType(l_DataTable.Compute("SUM (Tax)", ""), Decimal)

                    'Me.TextboxMinDistAmount.Text = Math.Round(Me.MinimumDistributionAmount, 2)
                    Me.TextboxMinDistAmount.Text = Math.Round(l_Decimal_Taxable, 2)
                    Me.TextboxMinDistNonTaxable.Text = Math.Round(l_Decimal_NonTaxable, 2)
                    Me.TextboxMinDistTaxable.Text = Math.Round(l_Decimal_Tax, 2)
                    Me.TextboxMinDistNet.Text = Me.MinimumDistributionAmount - Math.Round(Convert.ToDecimal(Me.TextboxMinDistTaxable.Text), 2)
                    Me.LabelRequiredMinDisbAmount.Visible = True

                Else
                    Me.TextboxMinDistTaxRate.Text = Me.MinDistributionTaxRate
                    Me.TextboxMinDistAmount.Text = "0.00"
                    Me.TextboxMinDistTaxable.Text = "0.00"
                    Me.TextboxMinDistNet.Text = "0.00"
                    Me.TextboxMinDistNonTaxable.Text = "0.00"
                    Me.LabelRequiredMinDisbAmount.Visible = False
                End If


            Else
                Me.TextboxMinDistAmount.Text = "0.00"
                Me.TextboxMinDistTaxable.Text = "0.00"
                Me.TextboxMinDistNet.Text = "0.00"
                Me.TextboxMinDistNonTaxable.Text = "0.00"
                Me.LabelRequiredMinDisbAmount.Visible = False
            End If

            Me.TextboxMinDistAmount.ReadOnly = True
            Me.TextboxMinDistTaxable.ReadOnly = True
            Me.TextboxMinDistNet.ReadOnly = True
            Me.TextboxMinDistNonTaxable.ReadOnly = True

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Function
    'START : ML : 05/05/2020 | YRS-AT-4874 | Calculate COVID Distribution    
    Private Function LoadCOVIDValuesIntoControls()

        Dim l_DataTable As DataTable

        Dim l_Decimal_Taxable As Decimal
        Dim l_Decimal_NonTaxable As Decimal
        Dim l_Decimal_Tax As Decimal
        Dim l_Decimal_Net As Decimal

        Try

            l_DataTable = DirectCast(Session("COVIDProrateAccountDataTable"), DataTable)


            If Not l_DataTable Is Nothing Then

                If l_DataTable.Rows.Count > 0 Then
                    l_Decimal_Taxable = CType(l_DataTable.Compute("SUM (Taxable)", ""), Decimal)
                    l_Decimal_NonTaxable = CType(l_DataTable.Compute("SUM (NonTaxable)", ""), Decimal)
                    l_Decimal_Tax = CType(l_DataTable.Compute("SUM (Tax)", ""), Decimal)

                    Me.txtTaxableC19.Text = Math.Round(l_Decimal_Taxable, 2)
                    Me.txtNonTaxableC19.Text = Math.Round(l_Decimal_NonTaxable, 2)
                    Me.txtTaxC19.Text = Math.Round(l_Decimal_Tax, 2)
                    Me.txtNetC19.Text = (Math.Round(l_Decimal_Taxable, 2) + Math.Round(l_Decimal_NonTaxable, 2)) - Math.Round(Convert.ToDecimal(l_Decimal_Tax), 2)


                Else
                    Me.txtTaxRateC19.Text = Me.COVIDTaxrate
                    Me.txtTaxC19.Text = "0.00"
                    Me.txtTaxableC19.Text = "0.00"
                    Me.txtNetC19.Text = "0.00"
                    Me.txtNonTaxableC19.Text = "0.00"
                End If
            Else
                Me.txtTaxRateC19.Text = Me.COVIDTaxrate
                Me.txtTaxC19.Text = "0.00"
                Me.txtTaxableC19.Text = "0.00"
                Me.txtNetC19.Text = "0.00"
                Me.txtNonTaxableC19.Text = "0.00"
            End If

            Me.txtTaxableC19.ReadOnly = True
            Me.txtNonTaxableC19.ReadOnly = True
            Me.txtTaxC19.ReadOnly = True
            Me.txtNetC19.ReadOnly = True

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Function
    'END : ML

    Private Function LoadDeafultValues()
        Try
            'Start Added by Dinesh Kanojia on 17/10/2013 gemini 2165
            If Not String.IsNullOrEmpty(Me.PersonID.ToString()) Then
                Dim dsPerssConfig As DataSet
                dsPerssConfig = YMCARET.YmcaBusinessObject.MRDBO.GetPersonMetaConfigurationDetails(Me.PersonID, "RMD")
                If (Not dsPerssConfig Is Nothing) Then
                    If (dsPerssConfig.Tables.Count > 0) Then
                        If dsPerssConfig.Tables(0).Rows.Count > 0 Then
                            '2014.02.18         Dinesh k    BT:2139-YRS 5.0-2165:RMD enhancements - Change in table structure- Used view instead table
                            If Not String.IsNullOrEmpty(dsPerssConfig.Tables(0).Rows(0)("RMDTAXRATE").ToString().ToUpper) Then
                                Me.MRDTaxrate = dsPerssConfig.Tables(0).Rows(0)("RMDTAXRATE").ToString()
                            Else
                                Me.MRDTaxrate = 10
                            End If
                        End If
                    End If
                End If
            End If

            'End Added by Dinesh Kanojia on 17/10/2013 gemini 2165

            'Start: AA: 11.20.2015 YRS-AT-2639 Added to use the meta configuration key value 
            'Me.TaxRate = 20.0
            Me.TaxRate = FedaralTaxrate
            'End: AA: 11.20.2015 YRS-AT-2639 Added to use the meta configuration key value 
            objRefundProcess.TaxRate = Me.TaxRate
            'Code Added by Dinesh Kanojia 17/10/2013 gemini 2165
            'Me.MinDistributionTaxRate = 10.0
            Me.MinDistributionTaxRate = Me.MRDTaxrate
            Me.DeductionsAmount = 0.0

            Me.TextboxTaxRate.Text = Me.TaxRate.ToString
            Me.TextboxMinDistTaxRate.Text = Me.MinDistributionTaxRate.ToString

            'START : ML | 22.04.20 | YRS-AT-4854 | Display COVID benefit breakup --%>
            InitializeCOVIDAmountsForDisplay()
            Me.txtTaxRateC19.Text = Me.COVIDTaxrate
            Me.txtTaxRateC19.ReadOnly = True
            'END : ML | 22.04.20 | YRS-AT-4874 | Display COVID benefit breakup --%>
            Me.TextboxDeductions.Text = Math.Round(Me.DeductionsAmount, 2)

            Me.TextboxTaxable3.ReadOnly = True
            Me.TextboxNonTaxable3.ReadOnly = True
            Me.TextboxNet3.ReadOnly = True

            Me.TextboxPayee2.Text = ""
            Me.TextboxPayee3.Text = ""

            Me.IsTookTDAccount = False


            Me.RadioButtonNone.Visible = False
            Me.RadioButtonRolloverAll.Visible = False
            Me.RadioButtonTaxableOnly.Visible = False
            Me.RadioButtonSpecificAmount.Visible = False
            Me.TextboxRolloverAmount.Visible = False

            ''Added by Dilip for MKT base withdrawal 0n 01-11-2009
            'Commented and Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
            'If Me.ISMarket = True And Me.RadioButtonRolloverYes.Checked = True Then 
            If Me.ISMarket = -1 And Me.CheckBoxRollovers.Checked = True Then
                Me.RadiobuttonRolloverOnlyFirstInstallment.Checked = True
                EnableDisablePartialRolloverControl(True)
            Else
                EnableDisablePartialRolloverControl(False)
            End If
            ''Added by Dilip for MKT base withdrawal 0n 01-11-2009
            Me.TextBoxStatus.Text = Me.RefundStatus

            Me.TextBoxStatus.ReadOnly = True
            'Commented by neeraj 08-Sep-09 for issue id YRS 5.0-821
            'Me.TextBoxCurrentPIA.ReadOnly = True
            Me.TextBoxTerminationPIA.ReadOnly = True

            ' 'Added Ganeswar 10thApril09 for BA Account Phase-V /Begin
            ' Me.TextBoxTerminationBA.ReadOnly = True
            Me.TextBoxCurrentBA.ReadOnly = True
            'Added Ganeswar 10thApril09 for BA Account Phase-V /End

            '' Now disable the Address Button.. 
            'Code change by ashutosh 18/04/06
            ' Me.ButtonAddress.Visible = False
            'Commented as was not required-Amit 14-09-2009
            'Me.ButtonAddress.Enabled = False
            'Commented as was not required-Amit 14-09-2009
            '**********************
            'Start:AA:24.09.2013 : BT-1501: Commented For Displaying address with address control not with textboxes
            'Me.TextboxAddress1.ReadOnly = True
            'Me.TextboxAddress2.ReadOnly = True
            'Me.TextboxAddress3.ReadOnly = True

            'Me.TextboxCity1.ReadOnly = True
            'Me.TextBoxCountry.ReadOnly = True
            'Me.TextBoxState.ReadOnly = True
            'Me.TextBoxZip.ReadOnly = True
            'End:AA:24.09.2013 : BT-1501: Commented For Displaying address with address control not with textboxes
            'AA:24.09.2013 : BT-1501: Displaying address with address control not with textboxes
            Me.AddressWebUserControl1.EnableControls = False


            Me.TextboxPayee2.rReadOnly = True 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
            Me.TextboxPayee3.ReadOnly = True

            Me.TextboxDeductions.ReadOnly = True
            Me.TextboxTaxRate.ReadOnly = True
            'BT:884-MRD Amount Additional Withholding issue.
            'Me.TextboxMinDistTaxRate.ReadOnly = True   'SB | 2017.12.15 | YRS-AT-3756 | RMD tax rate can be edited through out the application 

            Me.LabelRequiredMinDisbAmount.Visible = False
            'we will set Min Distribution Tax Rate to be used by Refunds.vb class
            objRefundProcess.MinDistributionTaxRate = Me.MinDistributionTaxRate
            If Me.RefundType = "HARD" Then
                Me.TDTaxRate = 10.0
                Me.HardShipTaxRate = 10.0
                Me.DeductionsAmount = 0.0
                Me.TextboxRequestAmount.Text = "0.00"
                Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate

                Me.TextboxHardShipAmount.Text = "0.00"
                Me.textboxHardShipNonTaxableAmount.Text = "0.00" 'MMR | 2017.07.30 | YRS-AT-3870 | Assigning default values for non-taxable hardship amount
                Me.TextboxHardShip.Text = "0.00"
                Me.TextboxHardShipNet.Text = "0.00"
                Me.RequestedAmount = 0.0


                Me.TextboxTDAvailableAmount.ReadOnly = True
                Me.TextboxHardShipTaxRate.ReadOnly = True
                Me.TextboxHardShipAmount.ReadOnly = True
                Me.textboxHardShipNonTaxableAmount.ReadOnly = False 'MMR | 2017.07.30 | YRS-AT-3870 | Making control read only to not allow any edit
                Me.TextboxHardShip.ReadOnly = False
                Me.TextboxHardShipNet.ReadOnly = True

                'Commented by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
                'Me.RadioButtonRolloversNo.Enabled = False
                'Me.RadioButtonRolloverYes.Enabled = False
                'Me.RadioButtonRolloversNo.Checked = True

                'Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
                Me.CheckBoxRollovers.Enabled = False
                Me.CheckBoxRollovers.Checked = False
                'Added Till Here

                'Added by Ganeswar on 28th May 2009
                Me.TextboxHardShipTaxRate.ReadOnly = False
                'End by Ganeswar on 28th May 2009
            End If
            'Added by Ganeswar on 28th May 2009
            If TextboxVoluntaryWithdrawalTotal.Text > "0.00" And Me.RefundType = "HARD" Then
                Me.TDTaxRate = 10.0
                Me.HardShipTaxRate = 10.0
                Me.DeductionsAmount = 0.0
                Me.TextboxRequestAmount.Text = "0.00"
                Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate

                Me.TextboxHardShipAmount.Text = "0.00"
                Me.textboxHardShipNonTaxableAmount.Text = "0.00" 'MMR | 2017.07.30 | YRS-AT-3870 | Assigning default values for non-taxable hardship amount
                Me.TextboxHardShip.Text = "0.00"
                Me.TextboxHardShipNet.Text = "0.00"
                Me.RequestedAmount = 0.0


                Me.TextboxTDAvailableAmount.ReadOnly = True
                Me.TextboxHardShipTaxRate.ReadOnly = True
                Me.TextboxHardShipAmount.ReadOnly = True
                Me.textboxHardShipNonTaxableAmount.ReadOnly = False 'MMR | 2017.07.30 | YRS-AT-3870 | Making control read only to not allow any edit
                Me.TextboxHardShip.ReadOnly = False
                Me.TextboxHardShipNet.ReadOnly = True

                'Me.RadioButtonRolloverNo.Enabled = False
                'Me.RadioButtonRolloverYes.Enabled = False
                'Commented by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
                'Me.RadioButtonRolloversNo.Checked = True
                'Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
                Me.CheckBoxRollovers.Checked = False
                Me.TextboxHardShipTaxRate.ReadOnly = False
            End If

            If TextboxTDAvailableAmount.Text > "0.00" And Me.RefundType = "HARD" Then
                Me.TextboxHardShipTaxRate.ReadOnly = False
                Me.TextboxHardShip.ReadOnly = False
                LabelVoluntaryAccounts.Visible = True
            Else
                Me.TextboxHardShipTaxRate.ReadOnly = True
                Me.TextboxHardShip.ReadOnly = True
            End If
            EnableDisableControlsForHardShip()
            'Added by Ganeswar on 28th May 2009
            DisableRadioButtons()
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Function

    Private Function LoadPayee1ValuesIntoControls()

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_DataTable As DataTable

        Dim l_Decimal_Taxable As Decimal
        Dim l_Decimal_NonTaxable As Decimal
        Dim l_Decimal_Tax As Decimal
        Dim l_Decimal_Net As Decimal

        Try
            ' 06-05-09, Ganeswar Added to allow Rollover in Hard Ship when Total of Voluntaty account is greater than zero. 
            'If Me.RefundType <> "HARD"
            If Me.RefundType <> "HARD" Or (Me.RefundType = "HARD" And Me.TextboxVoluntaryWithdrawalTotal.Text > "0.00") Then
                'Hafiz 03Feb06 Cache-Session'l_CacheManager = CacheFactory.GetCacheManager
                'l_DataTable = CType(l_CacheManager.GetData("Payee1DataTable"), DataTable)
                l_DataTable = DirectCast(Session("Payee1DataTable_C19"), DataTable)
                'Hafiz 03Feb06 Cache-Session

                If Not l_DataTable Is Nothing Then

                    If l_DataTable.Rows.Count > 0 Then
                        l_Decimal_Taxable = CType(l_DataTable.Compute("SUM (Taxable)", ""), Decimal)
                        l_Decimal_NonTaxable = CType(l_DataTable.Compute("SUM (NonTaxable)", ""), Decimal)

                        If Me.TaxRate = 0 Then
                            l_Decimal_Tax = 0.0
                        Else
                            l_Decimal_Tax = (Me.TaxRate / 100) * l_Decimal_Taxable
                        End If

                        l_Decimal_Net = (l_Decimal_Taxable + l_Decimal_NonTaxable) - l_Decimal_Tax


                        '  Me.TextboxTaxRate.Text = Me.TaxRate.ToString
                        Me.TextboxTaxable.Text = Math.Round(l_Decimal_Taxable, 2)
                        ' Session("Taxable") = Math.Round(l_Decimal_Taxable, 2)
                        Me.TextboxNonTaxable.Text = Math.Round(l_Decimal_NonTaxable, 2)
                        Me.TextboxTax.Text = Math.Round(l_Decimal_Tax, 2)
                        Me.TextboxNet.Text = Math.Round(l_Decimal_Net, 2)

                    Else
                        Me.TextboxTaxRate.Text = Me.TaxRate
                        Me.TextboxTaxable.Text = "0.00"
                        Me.TextboxNonTaxable.Text = "0.00"
                        Me.TextboxTax.Text = "0.00"
                        Me.TextboxNet.Text = "0.00"
                    End If


                Else
                    Me.TextboxTaxRate.Text = Me.TaxRate
                    Me.TextboxTaxable.Text = "0.00"
                    Me.TextboxNonTaxable.Text = "0.00"
                    Me.TextboxTax.Text = "0.00"
                    Me.TextboxNet.Text = "0.00"
                End If

            End If


            If Me.RadioButtonRolloverAll.Checked Then
                'Bug Id 420 May21st,2008 Shubhrata & Priya
                'Commented and Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
                'If Me.RadioButtonRolloverYes.Checked = True Then
                If Me.CheckBoxRollovers.Checked = True Then
                    Me.TextboxTax.Text = "0.00"
                End If
                'Bug Id 420 May21st,2008 Shubhrata & Priya
            End If


            Me.TextboxTaxable.ReadOnly = True
            Me.TextboxNonTaxable.ReadOnly = True
            Me.TextboxTax.ReadOnly = True
            Me.TextboxNet.ReadOnly = True

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Function

    Private Function LoadPayee2ValuesIntoControls()

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_DataTable As DataTable

        Dim l_Decimal_Taxable As Decimal
        Dim l_Decimal_NonTaxable As Decimal
        Dim l_Decimal_Net As Decimal

        Try
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'l_DataTable = CType(l_CacheManager.GetData("Payee2DataTable"), DataTable)
            l_DataTable = DirectCast(Session("Payee2DataTable_C19"), DataTable)
            'Hafiz 03Feb06 Cache-Session

            If Not l_DataTable Is Nothing Then

                If l_DataTable.Rows.Count > 0 Then
                    l_Decimal_Taxable = CType(l_DataTable.Compute("SUM (Taxable)", ""), Decimal)
                    l_Decimal_NonTaxable = CType(l_DataTable.Compute("SUM (NonTaxable)", ""), Decimal)

                    l_Decimal_Net = (l_Decimal_Taxable + l_Decimal_NonTaxable) ' - l_Decimal_Tax

                    Me.TextboxTaxable2.Text = Math.Round(l_Decimal_Taxable, 2)
                    Me.TextboxNonTaxable2.Text = Math.Round(l_Decimal_NonTaxable, 2)
                    Me.TextboxNet2.Text = Math.Round(l_Decimal_Net, 2)

                Else

                    Me.TextboxTaxable2.Text = "0.00"
                    Me.TextboxNonTaxable2.Text = "0.00"
                    Me.TextboxNet2.Text = "0.00"
                End If
            Else

                'Me.TextboxTaxFinal.Text = "0.00"
                Me.TextboxTaxable2.Text = "0.00"
                Me.TextboxNonTaxable2.Text = "0.00"
                Me.TextboxNet2.Text = "0.00"
            End If

            Me.TextboxTaxable2.ReadOnly = True
            Me.TextboxNonTaxable2.ReadOnly = True
            Me.TextboxNet2.ReadOnly = True
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Function

    ''Function added by Ruchi to Load Payee3 Controls
    Private Function LoadPayee3ValuesIntoControls()
        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_DataTable As DataTable

        Dim l_Decimal_Taxable As Decimal
        Dim l_Decimal_NonTaxable As Decimal
        Dim l_Decimal_Net As Decimal

        Try
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'l_DataTable = CType(l_CacheManager.GetData("Payee3DataTable"), DataTable)
            l_DataTable = DirectCast(Session("Payee3DataTable_C19"), DataTable)
            'Hafiz 03Feb06 Cache-Session

            If Not l_DataTable Is Nothing Then

                If l_DataTable.Rows.Count > 0 Then

                    l_Decimal_Taxable = CType(l_DataTable.Compute("SUM (Taxable)", ""), Decimal)
                    l_Decimal_NonTaxable = CType(l_DataTable.Compute("SUM (NonTaxable)", ""), Decimal)

                    l_Decimal_Net = (l_Decimal_Taxable + l_Decimal_NonTaxable) ' - l_Decimal_Tax

                    Me.TextboxTaxable3.Text = Math.Round(l_Decimal_Taxable, 2)
                    Me.TextboxNonTaxable3.Text = Math.Round(l_Decimal_NonTaxable, 2)
                    Me.TextboxNet3.Text = Math.Round(l_Decimal_Net, 2)

                Else

                    Me.TextboxTaxable3.Text = "0.00"
                    Me.TextboxNonTaxable3.Text = "0.00"
                    Me.TextboxNet3.Text = "0.00"
                End If
            Else

                'Me.TextboxTaxFinal.Text = "0.00"
                Me.TextboxTaxable3.Text = "0.00"
                Me.TextboxNonTaxable3.Text = "0.00"
                Me.TextboxNet3.Text = "0.00"
            End If

            Me.TextboxTaxable3.ReadOnly = True
            Me.TextboxNonTaxable3.ReadOnly = True
            Me.TextboxNet3.ReadOnly = True

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function LoadPayeesDataGrid()
        'Added for Mkt Withdrawal 
        Dim l_DataRow As DataRow
        'Added for Mkt Withdrawal 
        Dim l_DataTable As DataTable

        Try

            l_DataTable = DirectCast(Session("Payee1DataTable_C19"), DataTable)
            'Added for Mkt Withdrawal 

            'Added for Mkt Withdrawal 
            If Me.RefundType = "HARD" Then
                ''If this column does'nt exist then only add it else not
                Dim I As Integer
                Dim ColumnFound As Boolean = False
                For I = 0 To Me.DataGridPayee1.Columns.Count - 1
                    If Me.DataGridPayee1.Columns(I).HeaderText = "Use" Then
                        ColumnFound = True
                        Exit For
                    End If
                Next

                If ColumnFound = False Then
                    m_DataGridCheckBox = New CustomControls.CheckBoxColumn(False, "Use") 'Manthan Rajguru   2015.09.24      YRS-AT-2550: Custom Control reference given for DatagridcheckBox
                    m_DataGridCheckBox.HeaderText = "Use"
                    m_DataGridCheckBox.DataField = "Use"
                    m_DataGridCheckBox.AutoPostBack = False
                    Me.DataGridPayee1.Columns.Add(m_DataGridCheckBox)

                End If
            End If

            'Commented code deleted by SG: 2012.03.16

            ' START | SR | 2017.11.22 | YRS-AT-3742 | Bind current balance of participant(Payee 1 & RMD balance) 
            'If RadioButtonNone.Checked = True And Me.ISMarket = -1 Then     '---Added dilip on 30-10-2009 for market base withdrawal
            '    For Each l_DataRow In l_DataTable.Rows
            '        l_DataRow.BeginEdit()
            '        l_DataRow("NonTaxable") = Math.Round(l_DataRow("NonTaxable") * Me.NumPercentageFactorofMoneyPartialRollOver, 2)
            '        l_DataRow("Taxable") = Math.Round(l_DataRow("Taxable") * Me.NumPercentageFactorofMoneyPartialRollOver, 2)

            '        l_DataRow.EndEdit()
            '    Next
            'End If
            'Me.DataGridPayee1.DataSource = l_DataTable
            'Me.DataGridPayee1.DataBind()
            Dim payee1AndRMDMergedBalance As DataTable

            Payee1BalanceWithRMD(l_DataTable)
            Payee1BalanceWithCOVID(CurrentBalance) ' ML : 05/05/2020 | YRS-AT-4874 | Calculate COVID Distribution
            If HelperFunctions.isNonEmpty(CurrentBalance) Then
                payee1AndRMDMergedBalance = DirectCast(CurrentBalance, DataTable)
                If RadioButtonNone.Checked = True And Me.ISMarket = -1 Then     '---Added dilip on 30-10-2009 for market base withdrawal
                    For Each payee1AndRMDMergedBalanceDataRow As DataRow In payee1AndRMDMergedBalance.Rows
                        payee1AndRMDMergedBalanceDataRow.BeginEdit()
                        payee1AndRMDMergedBalanceDataRow("NonTaxable") = Math.Round(payee1AndRMDMergedBalanceDataRow("NonTaxable") * Me.NumPercentageFactorofMoneyPartialRollOver, 2)
                        payee1AndRMDMergedBalanceDataRow("Taxable") = Math.Round(payee1AndRMDMergedBalanceDataRow("Taxable") * Me.NumPercentageFactorofMoneyPartialRollOver, 2)
                        payee1AndRMDMergedBalanceDataRow.EndEdit()
                    Next
                End If
                Me.DataGridPayee1.DataSource = payee1AndRMDMergedBalance
                Me.DataGridPayee1.DataBind()
                CurrentBalance = Nothing
            End If
            ' END | SR | 2017.11.22 | YRS-AT-3742 | Bind current balance of participant(Payee 1 & RMD balance) 

            'Commented code deleted by SG: 2012.03.16

            l_DataTable = DirectCast(Session("Payee2DataTable_C19"), DataTable)

            Me.DatagridPayee2.DataSource = l_DataTable
            Me.DatagridPayee2.DataBind()



            l_DataTable = DirectCast(Session("Payee3DataTable_C19"), DataTable)

            'Commented code deleted by SG: 2012.03.16

            Me.DatagridPayee3.DataSource = l_DataTable
            Me.DatagridPayee3.DataBind()


        Catch ex As Exception
            Throw
        End Try


    End Function



    Private Function CalculateTotal(Optional ByVal parameterDataTable As DataTable = Nothing)
        Dim l_CalculatedDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_BooleanFlag As Boolean = False
        Try

            If Not parameterDataTable Is Nothing Then
                l_CalculatedDataTable = parameterDataTable
            Else
                l_CalculatedDataTable = Session("CalculatedDataTable_C19")

            End If
            If Not l_CalculatedDataTable Is Nothing Then
                ' If the field Total is already exist in the Table then Delete the Row.
                For Each l_DataRow In l_CalculatedDataTable.Rows
                    If Not (l_DataRow("AccountType").GetType.ToString = "System.DBNull") Then
                        If CType(l_DataRow("AccountType"), String) = "Total" Then
                            l_BooleanFlag = True
                            Exit For
                        End If
                    End If
                Next

                If l_BooleanFlag = False Then
                    l_DataRow = l_CalculatedDataTable.NewRow
                    l_CalculatedDataTable.Rows.Add(l_DataRow)

                    l_DataRow = l_CalculatedDataTable.NewRow
                    l_CalculatedDataTable.Rows.Add(l_DataRow)

                    l_DataRow = l_CalculatedDataTable.NewRow
                Else
                    l_DataRow("Taxable") = "0.00"
                    l_DataRow("Non-Taxable") = "0.00"
                    l_DataRow("Interest") = "0.00"
                    l_DataRow("Emp.Total") = "0.00"
                    l_DataRow("YMCATaxable") = "0.00"
                    l_DataRow("YMCAInterest") = "0.00"
                    l_DataRow("YMCATotal") = "0.00"
                    l_DataRow("Total") = "0.00"
                End If


                l_DataRow("Taxable") = l_CalculatedDataTable.Compute("SUM (Taxable)", "")
                l_DataRow("Non-Taxable") = l_CalculatedDataTable.Compute("SUM ([Non-Taxable])", "")
                l_DataRow("Interest") = l_CalculatedDataTable.Compute("SUM (Interest)", "")
                l_DataRow("Emp.Total") = l_CalculatedDataTable.Compute("SUM ([Emp.Total])", "")
                l_DataRow("YMCATaxable") = l_CalculatedDataTable.Compute("SUM (YMCATaxable)", "")
                l_DataRow("YMCAInterest") = l_CalculatedDataTable.Compute("SUM (YMCAInterest)", "")
                l_DataRow("YMCATotal") = l_CalculatedDataTable.Compute("SUM (YMCATotal)", "")
                l_DataRow("Total") = l_CalculatedDataTable.Compute("SUM (Total)", "")


                If l_DataRow("Taxable").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Taxable") = "0.00"
                End If

                If l_DataRow("Non-Taxable").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Non-Taxable") = "0.00"
                End If

                If l_DataRow("Interest").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Interest") = "0.00"
                End If

                If l_DataRow("Emp.Total").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Emp.Total") = "0.00"
                End If

                If l_DataRow("YMCATaxable").GetType.ToString = "System.DBNull" Then
                    l_DataRow("YMCATaxable") = "0.00"
                End If

                If l_DataRow("YMCAInterest").GetType.ToString = "System.DBNull" Then
                    l_DataRow("YMCAInterest") = "0.00"
                End If

                If l_DataRow("YMCATotal").GetType.ToString = "System.DBNull" Then
                    l_DataRow("YMCATotal") = "0.00"
                End If

                If l_DataRow("Total").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Total") = "0.00"
                End If


                If l_BooleanFlag = False Then
                    l_DataRow("AccountType") = "Total"
                    l_CalculatedDataTable.Rows.Add(l_DataRow)
                End If

                'l_CacheManger.Add("CalculatedDataTable", l_CalculatedDataTable)

            End If

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Function
    Private Function CalculateTotalForDisplay(ByVal parameterDataTable As DataTable) As DataTable
        Dim l_CalculatedDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_BooleanFlag As Boolean = False

        Try
            l_CalculatedDataTable = parameterDataTable
            If Not l_CalculatedDataTable Is Nothing Then

                ' If the field Total is already exist in the Table then Delete the Row.
                For Each l_DataRow In l_CalculatedDataTable.Rows
                    If Not (l_DataRow("AccountType").GetType.ToString = "System.DBNull") Then
                        If CType(l_DataRow("AccountType"), String) = "Total" Then
                            l_BooleanFlag = True
                            Exit For
                        End If
                    End If
                Next

                If l_BooleanFlag = False Then
                    l_DataRow = l_CalculatedDataTable.NewRow
                    l_CalculatedDataTable.Rows.Add(l_DataRow)

                    l_DataRow = l_CalculatedDataTable.NewRow
                    l_CalculatedDataTable.Rows.Add(l_DataRow)

                    l_DataRow = l_CalculatedDataTable.NewRow
                Else
                    l_DataRow("Taxable") = "0.00"
                    l_DataRow("Non-Taxable") = "0.00"
                    l_DataRow("Interest") = "0.00"
                    l_DataRow("Total") = "0.00"
                End If


                l_DataRow("Taxable") = l_CalculatedDataTable.Compute("SUM (Taxable)", "")
                l_DataRow("Non-Taxable") = l_CalculatedDataTable.Compute("SUM ([Non-Taxable])", "")
                l_DataRow("Interest") = l_CalculatedDataTable.Compute("SUM (Interest)", "")
                l_DataRow("Total") = l_CalculatedDataTable.Compute("SUM (Total)", "")


                If l_DataRow("Taxable").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Taxable") = "0.00"
                End If

                If l_DataRow("Non-Taxable").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Non-Taxable") = "0.00"
                End If

                If l_DataRow("Interest").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Interest") = "0.00"
                End If
                If l_DataRow("Total").GetType.ToString = "System.DBNull" Then
                    l_DataRow("Total") = "0.00"
                End If


                If l_BooleanFlag = False Then
                    l_DataRow("AccountType") = "Total"
                    l_CalculatedDataTable.Rows.Add(l_DataRow)
                End If
                l_CalculatedDataTable.AcceptChanges()
                Return l_CalculatedDataTable
                'l_CacheManger.Add("CalculatedDataTable", l_CalculatedDataTable)

            End If

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Function
    'Modified for Plan Split Changes and Addition of new account types

    Private Function CopyAccountContributionTableForCurrentAccounts()

        Dim l_AccountContributionTable As DataTable
        Dim l_CalculationDataTable As DataTable
        Dim l_CalculationDataTable_VolWithdrawalTotal As DataTable
        Dim l_CalculationDataTable1 As DataTable
        Dim l_DataColumn As DataColumn
        Dim l_DataRow As DataRow

        Try

            l_AccountContributionTable = Session("AccountContribution_C19")

            If Not l_AccountContributionTable Is Nothing Then

                l_CalculationDataTable = l_AccountContributionTable.Clone
                l_CalculationDataTable1 = l_AccountContributionTable.Clone
                '
                ' Add a column Into the Table to allow selected.
                l_DataColumn = New DataColumn("Selected")
                l_DataColumn.DataType = System.Type.GetType("System.Boolean")
                l_DataColumn.AllowDBNull = True

                l_CalculationDataTable.Columns.Add(l_DataColumn)
                'Copy all Values into Calculation Table 

                For Each l_DataRow In l_AccountContributionTable.Rows

                    If Not (l_DataRow("AccountType").GetType.ToString = "System.DBNull") Then

                        If Not (CType(l_DataRow("AccountType"), String) = "Total") Then
                            l_CalculationDataTable.ImportRow(l_DataRow)
                            l_CalculationDataTable1.ImportRow(l_DataRow)
                        End If

                        If (CType(l_DataRow("AccountType"), String) = "Total") Then
                            Me.TotalRefundAmount = IIf(l_DataRow("Emp.Total").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("Emp.Total"), Decimal)) + IIf(l_DataRow("YMCATotal").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("YMCATotal"), Decimal))
                            Me.PersonalAmount = IIf(l_DataRow("Emp.Total").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("Emp.Total"), Decimal))
                        End If

                    End If
                Next

                If Me.RefundType = "REG" Then

                    Dim l_Bool_Useit As Boolean
                    Dim l_Bool_Yside As Boolean
                    Dim l_CurrentRow As DataRow

                    For Each l_CurrentRow In l_CalculationDataTable.Rows
                        l_Bool_Useit = True
                        l_Bool_Yside = True

                        If IsBasicAccount(l_CurrentRow) Then
                            l_Bool_Useit = True
                            ' START | SR | 2016.10.04 | YRS-AT-2962 - If BA/Legacy combined amount switch is ON then include Yside money 
                            If BA_Legacy_combined_Amt_Switch Then
                                l_Bool_Yside = True
                            Else
                                If Me.IsVested = True And Me.TerminationPIA <= Me.MaximumPIAAmount Then
                                    l_Bool_Yside = True
                                Else
                                    l_Bool_Yside = False
                                End If
                            End If
                            ' END | SR | 2016.10.04 | YRS-AT-2962 - If BA/Legacy combined amount switch is ON then include Yside money

                            ' START | SR | 2017.07.03 | YRS-AT-3161 | For QD participants, there will be no termination date hence Termination PIA condition not applicable.
                            If Me.FundEventStatus.Trim.ToUpper() = "QD" Then
                                l_Bool_Yside = True
                            End If
                            ' END | SR | 2017.07.03 | YRS-AT-3161 | For QD participants, there will be no termination date hence Termination PIA condition not applicable.

                            If Me.IsPersonalOnly Then
                                l_Bool_Yside = False
                            End If

                            'Start - Retirement Plan Group
                        ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_RetirementPlan_AM Then
                            l_Bool_Useit = True
                            l_Bool_Yside = True
                        ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_RetirementPlan_AP Then
                            l_Bool_Useit = True
                            l_Bool_Yside = False

                        ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_RetirementPlan_RP Then
                            l_Bool_Useit = True
                            l_Bool_Yside = False
                        ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_RetirementPlan_SR Then

                            'Modified by Shubhrata Feb13th 2007,YREN-3039 SR account is to be treated similarly to AM account
                            'on the condition that participant is not active
                            If Me.SessionStatusType.Trim.ToUpper <> "AE" Then
                                l_Bool_Useit = True
                                l_Bool_Yside = True
                            Else
                                l_Bool_Useit = False
                                l_Bool_Yside = False
                            End If
                            'COmmented by SHubhrata Feb 27th,2007
                            'Begin Code Merge by Dilip on 07-05-2009
                            'Priya 14-Jan-2009 : YRS 5.0-637 AC Account interest components Added ElseIf condition for AC account type
                        ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_RetirementPlan_AC Then
                            l_Bool_Useit = True
                            l_Bool_Yside = False
                            'End Code Merge by Dilip on 07-05-2009
                            'Start - Savings plan group 
                        ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_SavingsPlan_TD Then
                            l_Bool_Useit = True
                            l_Bool_Yside = False
                        ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_SavingsPlan_TM Then

                            l_Bool_Useit = True
                            l_Bool_Yside = True
                        ElseIf l_CurrentRow("AccountGroup").ToString.ToUpper() = m_const_SavingsPlan_RT Then
                            l_Bool_Useit = True
                            l_Bool_Yside = False
                            'End - Savings plan group 
                        End If

                        If Not l_Bool_Useit Then
                            l_CurrentRow.Delete()
                        End If
                        'If l_Bool_Yside = False Then  
                        'Condition Modified Due TO YREN 2510
                        If l_Bool_Yside = False And l_Bool_Useit = True Then
                            'Condition Modified Due TO YREN 2510
                            l_CurrentRow("YMCATaxable") = 0.0
                            l_CurrentRow("YMCAInterest") = 0.0
                            l_CurrentRow("Total") = Convert.ToDouble(l_CurrentRow("Total")) - Convert.ToDouble(l_CurrentRow("YMCATotal"))
                            l_CurrentRow("YMCATotal") = 0.0
                        End If

                    Next
                    l_CalculationDataTable.AcceptChanges()
                End If

                Session("CalculatedDataTableForCurrentAccounts_C19") = l_CalculationDataTable
                If Not l_CalculationDataTable Is Nothing Then
                    l_CalculationDataTable_VolWithdrawalTotal = l_CalculationDataTable.Clone()
                    If l_CalculationDataTable.Rows.Count > 0 Then
                        For Each dr As DataRow In l_CalculationDataTable.Rows
                            l_CalculationDataTable_VolWithdrawalTotal.ImportRow(dr)
                        Next
                    End If
                    Session("CalculatedDataTableForCurrentAccounts_Initial_C19") = l_CalculationDataTable_VolWithdrawalTotal

                End If

            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub LoadPIA()
        'Commented code deleted by SG: 2012.03.16

        Dim l_String_PIA As String

        Dim l_decimal_CurrentPIA As Decimal
        Dim l_decimal_PIATermination As Decimal

        'Harshala :: BT:978-YRS 5.0-1519 - 'YMCA acct at request" amt on withdrawal screen is incorrect-START
        Dim l_decimal_RequestedPIA As Decimal
        'Harshala :: BT:978-YRS 5.0-1519 - 'YMCA acct at request" amt on withdrawal screen is incorrect-END

        Try
            'Commented code deleted by SG: 2012.03.16
            Me.TerminationPIA = CType(Session("TerminationPIA_C19"), Decimal)
            Me.CurrentPIA = CType(Session("CurrentPIA_C19"), Decimal)
            'Hafiz 03Feb06 Cache-Session

            Me.TextBoxTerminationPIA.Text = FormatCurrency(Me.TerminationPIA)
            'Commented by neeraj 08-Sep-09 for issue id YRS 5.0-821
            'Me.TextBoxCurrentPIA.Text = FormatCurrency(Me.CurrentPIA)

            'Added Ganeswar 10thApril09 for BA Account Phase-V /Begin
            Me.TerminationBA = CType(Session("BATermination_C19"), Decimal)
            Me.CurrentBA = CType(Session("BACurrent_C19"), Decimal)

            'Me.TextBoxTerminationBA.Text = FormatCurrency(Me.TerminationBA)
            'Harshala :: BT:978-YRS 5.0-1519 - 'YMCA acct at request" amt on withdrawal screen is incorrect-START
            'Me.TextBoxCurrentBA.Text = FormatCurrency(Me.CurrentBA)
            l_decimal_RequestedPIA = YMCARET.YmcaBusinessObject.RefundRequest.GetBARequestedPIA(Me.FundID, Me.SessionRefundRequestID)
            Me.TextBoxCurrentBA.Text = FormatCurrency(l_decimal_RequestedPIA)
            'Harshala :: BT:978-YRS 5.0-1519 - 'YMCA acct at request" amt on withdrawal screen is incorrect-END

            'Added Ganeswar 10thApril09 for BA Account Phase-V /End
        Catch ex As Exception
            Throw ex
        End Try

    End Sub


    Private Sub TabStripVoluntaryRefund_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStripVoluntaryRefund.SelectedIndexChange
        Try
            Me.MultiPageVoluntaryRefund.SelectedIndex = Me.TabStripVoluntaryRefund.SelectedIndex
            If Me.RefundType = "HARD" Then
                If MultiPageVoluntaryRefund.SelectedIndex = 1 Then
                    TextboxRequestAmount.TabIndex = 0
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Sub MultiPageVoluntaryRefund_SelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MultiPageVoluntaryRefund.SelectedIndexChange

    End Sub

    Private Function LoadRequestedAccounts()

        Dim l_DataTable As DataTable
        'Commented code deleted by SG: 2012.03.16
        Dim l_finalDisplaytable As New DataTable
        Dim dec_GrossAmount As Decimal

        Try
            If Me.SessionRefundRequestID <> String.Empty Then

                l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundRequestsDetails(Me.SessionRefundRequestID)
                dec_GrossAmount = Me.SessionGrossAmount
                Me.NumTotalWithdrawalAmount = CType(l_DataTable.Compute("SUM (Total)", ""), Decimal)
                'Change the diplay of the grids
                'Added Ganeswar 10thApril09 for BA Account Phase-V /End
                l_finalDisplaytable = Me.CreatDisplayTables(l_DataTable)
                'Commented code deleted by SG: 2012.03.16

                'Added Ganeswar 10thApril09 for BA Account Phase-V /End
                Me.CalculateTotal(l_DataTable)

                'Me.DataGridRequestedAccts.DataSource = l_DataTable
                'Me.DataGridRequestedAccts.DataBind()

                l_finalDisplaytable = Me.CalculateTotalForDisplay(l_finalDisplaytable)

                Me.DataGridRequestedAccts.DataSource = l_finalDisplaytable
                Me.DataGridRequestedAccts.DataBind()

                ''Added By Dilip for MKT on 16-10-2009
                If Me.ISMarket = -1 Then
                    Dim l_InstallmentDataTable As DataTable
                    Dim l_FirstInstallmentdatatable As DataTable
                    l_FirstInstallmentdatatable = YMCARET.YmcaBusinessObject.RefundRequest.GetFirstInstallment(Me.SessionRefundRequestID)
                    l_InstallmentDataTable = CreatDisplayTables(l_FirstInstallmentdatatable) 'DisplayInstalmentGrid(l_finalDisplaytable)
                    l_InstallmentDataTable = Me.CalculateTotalForDisplay(l_InstallmentDataTable)
                    Me.DataGridInstallment.DataSource = l_InstallmentDataTable
                    Me.DataGridInstallment.DataBind()
                    'added by Ganeswar on 14thOct for new  screen design
                    Me.SetSelectedIndex(Me.DataGridInstallment, l_InstallmentDataTable)
                    'added by Ganeswar on 14thOct for new  screen design
                    'Added By parveen on 04-Nov-2009 To Hide the First Installment   
                Else
                    trFirstInstallment.Visible = False
                    trInstallmentGrid.Visible = False
                    'Added By parveen on 04-Nov-2009 To Hide the First Installment   
                End If
                'Added By Dilip for MKT on 16-10-2009

                'Commented code deleted by SG: 2012.03.16
                Session("RequestedAccounts_C19") = l_DataTable
                'Hafiz 03Feb06 Cache-Session


                'Commented code deleted by SG: 2012.03.16

                'Me.SetSelectedIndex(Me.DataGridRequestedAccts, l_DataTable)
                Me.SetSelectedIndex(Me.DataGridRequestedAccts, l_finalDisplaytable)

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function



    'Added By Ganeswar Sahoo for MKT on 16-10-2009
    Private Sub DataGridInstallment_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridInstallment.ItemDataBound
        Try
            e.Item.Cells(0).Visible = False
            e.Item.Cells(2).Visible = False
            If e.Item.ItemType <> ListItemType.Header Then
                If e.Item.Cells(0).Text.ToUpper <> "&NBSP;" Or e.Item.Cells(1).Text.ToUpper = "TOTAL" Then
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
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    'Added By Ganeswar Sahoo for MKT on 16-10-2009
    Private Function DisplayInstalmentGridPaye(ByVal l_finalDisplaytable As DataTable) As DataTable
        Dim l_QueryString As String
        Dim l_dr_NewRowFirstInstallment As DataRow
        Dim l_DataRow As DataRow
        Dim l_FoundRowsRetirement() As DataRow
        Dim l_FoundRowsSaving() As DataRow
        Dim l_DataTableFirstInstalment As DataTable
        l_DataTableFirstInstalment = l_finalDisplaytable.Clone
        Try
            If Not l_finalDisplaytable Is Nothing Then
                'Retirement Plan
                l_QueryString = "PlanType = 'RETIREMENT'"
                l_FoundRowsRetirement = l_finalDisplaytable.Select(l_QueryString)

                If Not l_FoundRowsRetirement Is Nothing Then
                    If l_FoundRowsRetirement.Length > 0 Then
                        l_dr_NewRowFirstInstallment = l_DataTableFirstInstalment.NewRow()
                        l_dr_NewRowFirstInstallment("Taxable") = 0
                        l_dr_NewRowFirstInstallment("Non-Taxable") = 0
                        l_dr_NewRowFirstInstallment("Interest") = 0
                        l_dr_NewRowFirstInstallment("Total") = 0
                        l_dr_NewRowFirstInstallment("YMCATaxable") = 0
                        l_dr_NewRowFirstInstallment("YMCAInterest") = 0

                        For Each l_DataRow In l_FoundRowsRetirement
                            If l_DataRow("AccountType").GetType.ToString <> "System.DBNull" Then
                                l_dr_NewRowFirstInstallment("AccountType") = l_DataRow("AccountType")
                                l_dr_NewRowFirstInstallment("AccountGroup") = l_DataRow("AccountGroup")
                                If l_DataRow("PlanType").GetType.ToString <> "System.DBNull" Then
                                    l_dr_NewRowFirstInstallment("PlanType") = l_DataRow("PlanType")
                                End If
                                l_dr_NewRowFirstInstallment("IsBasicAccount") = l_DataRow("IsBasicAccount")
                                If l_DataRow("AccountType") <> "Total" Then
                                    l_dr_NewRowFirstInstallment("Taxable") = CType(l_dr_NewRowFirstInstallment("Taxable"), Decimal) + Math.Round((l_DataRow("Taxable") * 10 / 100), 2)
                                    l_dr_NewRowFirstInstallment("Non-Taxable") = CType(l_dr_NewRowFirstInstallment("Non-Taxable"), Decimal) + Math.Round((l_DataRow("Non-Taxable") * 10 / 100), 2)
                                    l_dr_NewRowFirstInstallment("Interest") = CType(l_dr_NewRowFirstInstallment("Interest"), Decimal) + Math.Round((l_DataRow("Interest") * 10 / 100), 2)
                                    l_dr_NewRowFirstInstallment("YMCATaxable") = CType(l_dr_NewRowFirstInstallment("YMCATaxable"), Decimal) + Math.Round((l_DataRow("YMCATaxable") * 10 / 100), 2)
                                    l_dr_NewRowFirstInstallment("YMCAInterest") = CType(l_dr_NewRowFirstInstallment("YMCAInterest"), Decimal) + Math.Round((l_DataRow("YMCAInterest") * 10 / 100), 2)
                                End If
                                l_dr_NewRowFirstInstallment("Total") = CType(l_dr_NewRowFirstInstallment("Total"), Decimal) + Math.Round((l_DataRow("Total") * 10 / 100), 2)

                            End If
                        Next
                        l_DataTableFirstInstalment.Rows.Add(l_dr_NewRowFirstInstallment)
                    End If
                End If
                '' Added by Dilip Patade on 19-10-2009 for market base withdrawal
                'Saving Plan
                l_QueryString = "PlanType = 'SAVINGS'"
                l_FoundRowsSaving = l_finalDisplaytable.Select(l_QueryString)

                If Not l_FoundRowsSaving Is Nothing Then
                    If l_FoundRowsSaving.Length > 0 Then
                        l_dr_NewRowFirstInstallment = l_DataTableFirstInstalment.NewRow()
                        l_dr_NewRowFirstInstallment("Taxable") = 0
                        l_dr_NewRowFirstInstallment("Non-Taxable") = 0
                        l_dr_NewRowFirstInstallment("Interest") = 0
                        l_dr_NewRowFirstInstallment("Total") = 0
                        l_dr_NewRowFirstInstallment("YMCATaxable") = 0
                        l_dr_NewRowFirstInstallment("YMCAInterest") = 0
                        For Each l_DataRow In l_FoundRowsSaving
                            If l_DataRow("AccountType").GetType.ToString <> "System.DBNull" Then
                                l_dr_NewRowFirstInstallment("AccountType") = l_DataRow("AccountType")
                                l_dr_NewRowFirstInstallment("AccountGroup") = l_DataRow("AccountGroup")
                                If l_DataRow("PlanType").GetType.ToString <> "System.DBNull" Then
                                    l_dr_NewRowFirstInstallment("PlanType") = l_DataRow("PlanType")
                                End If
                                l_dr_NewRowFirstInstallment("IsBasicAccount") = l_DataRow("IsBasicAccount")
                                If l_DataRow("AccountType") <> "Total" Then
                                    l_dr_NewRowFirstInstallment("Taxable") = CType(l_dr_NewRowFirstInstallment("Taxable"), Decimal) + Math.Round((l_DataRow("Taxable") * 10 / 100), 2)
                                    l_dr_NewRowFirstInstallment("Non-Taxable") = CType(l_dr_NewRowFirstInstallment("Non-Taxable"), Decimal) + Math.Round((l_DataRow("Non-Taxable") * 10 / 100), 2)
                                    l_dr_NewRowFirstInstallment("Interest") = CType(l_dr_NewRowFirstInstallment("Interest"), Decimal) + Math.Round((l_DataRow("Interest") * 10 / 100), 2)
                                    l_dr_NewRowFirstInstallment("YMCATaxable") = CType(l_dr_NewRowFirstInstallment("YMCATaxable"), Decimal) + Math.Round((l_DataRow("YMCATaxable") * 10 / 100), 2)
                                    l_dr_NewRowFirstInstallment("YMCAInterest") = CType(l_dr_NewRowFirstInstallment("YMCAInterest"), Decimal) + Math.Round((l_DataRow("YMCAInterest") * 10 / 100), 2)
                                End If
                                l_dr_NewRowFirstInstallment("Total") = CType(l_dr_NewRowFirstInstallment("Total"), Decimal) + Math.Round((l_DataRow("Total") * 10 / 100), 2)

                            End If
                        Next
                        l_DataTableFirstInstalment.Rows.Add(l_dr_NewRowFirstInstallment)
                    End If
                End If
            End If
            Me.PartialRollOverMinLimit = CType(l_DataTableFirstInstalment.Compute("SUM (Total)", ""), Decimal)
            Return l_DataTableFirstInstalment
            '' Added by Dilip Patade on 19-10-2009 for market base withdrawal
        Catch ex As Exception
            Throw
        End Try
    End Function
    '' Added by Dilip Patade on 19-10-2009 for market base withdrawal
    Private Function DisplayInstalmentGrid(ByVal l_finalDisplaytable As DataTable) As DataTable
        Dim l_QueryString As String
        Dim l_dr_NewRowFirstInstallment As DataRow
        Dim l_DataRow As DataRow
        Dim l_FoundRowsRetirement() As DataRow
        Dim l_FoundRowsSaving() As DataRow
        Dim l_DataTableFirstInstalment As DataTable
        l_DataTableFirstInstalment = l_finalDisplaytable.Clone
        'Added By Dilip on 25-11-2009
        Dim l_BoolRetirementPlanISMarket As Boolean
        Dim l_BoolSavingPlanISMarket As Boolean
        Dim l_decimalRFirstInstalmentPercentage As Decimal = 0.0
        Dim l_decimalSFirstInstalmentPercentage As Decimal = 0.0
        'Added By Dilip on 25-11-2009
        Try
            If Not l_finalDisplaytable Is Nothing Then
                'Added By Dilip on 25-11-2009
                l_BoolRetirementPlanISMarket = objRefundProcess.GetIsMarket(Me.FundID, "RETIREMENT")
                l_BoolSavingPlanISMarket = objRefundProcess.GetIsMarket(Me.FundID, "SAVINGS")
                If l_BoolRetirementPlanISMarket = True Then
                    l_decimalRFirstInstalmentPercentage = Me.FirstInstallment / 100
                Else
                    l_decimalRFirstInstalmentPercentage = 1
                End If
                If l_BoolSavingPlanISMarket = True Then     ''Change By Dilip on 25-11-2009 internal bugid  1039 
                    l_decimalSFirstInstalmentPercentage = Me.FirstInstallment / 100
                Else
                    l_decimalSFirstInstalmentPercentage = 1
                End If
                'Added By Dilip on 25-11-2009

                'Retirement Plan
                l_QueryString = "PlanType = 'RETIREMENT'"
                l_FoundRowsRetirement = l_finalDisplaytable.Select(l_QueryString)

                If Not l_FoundRowsRetirement Is Nothing Then
                    If l_FoundRowsRetirement.Length > 0 Then
                        'Commented code deleted by SG: 2012.03.16
                        For Each l_DataRow In l_FoundRowsRetirement
                            l_dr_NewRowFirstInstallment = l_DataTableFirstInstalment.NewRow()
                            l_dr_NewRowFirstInstallment("Taxable") = 0
                            l_dr_NewRowFirstInstallment("Non-Taxable") = 0
                            l_dr_NewRowFirstInstallment("Interest") = 0
                            l_dr_NewRowFirstInstallment("Total") = 0
                            If l_DataRow("AccountType").GetType.ToString <> "System.DBNull" Then
                                l_dr_NewRowFirstInstallment("AccountType") = l_DataRow("AccountType")
                                l_dr_NewRowFirstInstallment("AccountGroup") = l_DataRow("AccountGroup")
                                If l_DataRow("PlanType").GetType.ToString <> "System.DBNull" Then
                                    l_dr_NewRowFirstInstallment("PlanType") = l_DataRow("PlanType")
                                End If
                                l_dr_NewRowFirstInstallment("IsBasicAccount") = l_DataRow("IsBasicAccount")
                                If l_DataRow("AccountType") <> "Total" Then
                                    l_dr_NewRowFirstInstallment("Taxable") = CType(l_dr_NewRowFirstInstallment("Taxable"), Decimal) + Math.Round((l_DataRow("Taxable") * l_decimalRFirstInstalmentPercentage), 2)
                                    l_dr_NewRowFirstInstallment("Non-Taxable") = CType(l_dr_NewRowFirstInstallment("Non-Taxable"), Decimal) + Math.Round((l_DataRow("Non-Taxable") * l_decimalRFirstInstalmentPercentage), 2)
                                    l_dr_NewRowFirstInstallment("Interest") = CType(l_dr_NewRowFirstInstallment("Interest"), Decimal) + Math.Round((l_DataRow("Interest") * l_decimalRFirstInstalmentPercentage), 2)
                                End If
                                l_dr_NewRowFirstInstallment("Total") = CType(l_dr_NewRowFirstInstallment("Total"), Decimal) + Math.Round((l_DataRow("Total") * l_decimalRFirstInstalmentPercentage), 2)

                            End If
                            l_DataTableFirstInstalment.Rows.Add(l_dr_NewRowFirstInstallment)
                        Next
                        'l_DataTableFirstInstalment.Rows.Add(l_dr_NewRowFirstInstallment)
                        'Moved this Code into foreach Loop by Parveen on 6-Dec-2010
                    End If
                End If
                'Saving Plan
                l_QueryString = "PlanType = 'SAVINGS'"
                l_FoundRowsSaving = l_finalDisplaytable.Select(l_QueryString)

                If Not l_FoundRowsSaving Is Nothing Then
                    If l_FoundRowsSaving.Length > 0 Then
                        l_dr_NewRowFirstInstallment = l_DataTableFirstInstalment.NewRow()
                        l_dr_NewRowFirstInstallment("Taxable") = 0
                        l_dr_NewRowFirstInstallment("Non-Taxable") = 0
                        l_dr_NewRowFirstInstallment("Interest") = 0
                        l_dr_NewRowFirstInstallment("Total") = 0
                        For Each l_DataRow In l_FoundRowsSaving
                            If l_DataRow("AccountType").GetType.ToString <> "System.DBNull" Then
                                l_dr_NewRowFirstInstallment("AccountType") = l_DataRow("AccountType")
                                l_dr_NewRowFirstInstallment("AccountGroup") = l_DataRow("AccountGroup")
                                If l_DataRow("PlanType").GetType.ToString <> "System.DBNull" Then
                                    l_dr_NewRowFirstInstallment("PlanType") = l_DataRow("PlanType")
                                End If
                                l_dr_NewRowFirstInstallment("IsBasicAccount") = l_DataRow("IsBasicAccount")
                                If l_DataRow("AccountType") <> "Total" Then
                                    l_dr_NewRowFirstInstallment("Taxable") = CType(l_dr_NewRowFirstInstallment("Taxable"), Decimal) + Math.Round((l_DataRow("Taxable") * l_decimalSFirstInstalmentPercentage), 2)
                                    l_dr_NewRowFirstInstallment("Non-Taxable") = CType(l_dr_NewRowFirstInstallment("Non-Taxable"), Decimal) + Math.Round((l_DataRow("Non-Taxable") * l_decimalSFirstInstalmentPercentage), 2)
                                    l_dr_NewRowFirstInstallment("Interest") = CType(l_dr_NewRowFirstInstallment("Interest"), Decimal) + Math.Round((l_DataRow("Interest") * l_decimalSFirstInstalmentPercentage), 2)
                                End If
                                l_dr_NewRowFirstInstallment("Total") = CType(l_dr_NewRowFirstInstallment("Total"), Decimal) + Math.Round((l_DataRow("Total") * l_decimalSFirstInstalmentPercentage), 2)

                            End If
                        Next
                        l_DataTableFirstInstalment.Rows.Add(l_dr_NewRowFirstInstallment)
                    End If
                End If
            End If
            Me.PartialRollOverMinLimit = CType(l_DataTableFirstInstalment.Compute("SUM (Total)", ""), Decimal)
            Return l_DataTableFirstInstalment

        Catch ex As Exception
            Throw
        End Try
    End Function
    '' Added by Dilip Patade on 19-10-2009 for market base withdrawal

    Private Function CreatDisplayTables(ByVal parameterDatable As DataTable) As DataTable
        Dim l_dt_DisplayDatatable As New DataTable
        Dim l_dr_DisplayDatatable As DataRow
        Dim l_datarow As DataRow
        Dim l_decimalTaxable As Decimal = 0.0
        Dim l_decimalNonTaxable As Decimal = 0.0
        Dim l_decimalInterest As Decimal = 0.0
        Dim l_decimalTotal As Decimal = 0.0
        Dim l_decimalYMCATaxable As Decimal = 0.0
        Dim l_decimalYMCAInterest As Decimal = 0.0
        Dim l_decimalYMCATotal As Decimal = 0.0
        Dim l_FinalDatatable As New DataTable
        Dim l_SortFinalDatatable As New DataTable

        Try
            If Not parameterDatable Is Nothing Then
                l_FinalDatatable.Columns.Add("IsBasicAccount", System.Type.GetType("System.Boolean"))
                'l_FinalDatatable.Columns.Add("Selected", System.Type.GetType("System.Boolean"))
                'l_dt_DisplayRetirementPlan.Columns("Selected").DefaultValue = True
                l_FinalDatatable.Columns.Add("AccountType", System.Type.GetType("System.String"))
                l_FinalDatatable.Columns.Add("AccountGroup", System.Type.GetType("System.String"))
                l_FinalDatatable.Columns.Add("PlanType", System.Type.GetType("System.String"))
                l_FinalDatatable.Columns.Add("Taxable", System.Type.GetType("System.Decimal"))
                l_FinalDatatable.Columns.Add("Non-Taxable", System.Type.GetType("System.Decimal"))
                l_FinalDatatable.Columns.Add("Interest", System.Type.GetType("System.Decimal"))
                l_FinalDatatable.Columns.Add("Total", System.Type.GetType("System.Decimal"))
                'set default values

                If parameterDatable.Rows.Count > 0 Then
                    For Each l_datarow In parameterDatable.Rows
                        'IsBasicAccount
                        If l_datarow("IsBasicAccount").GetType.ToString = "System.Boolean" Then
                            If CType(l_datarow("IsBasicAccount"), Boolean) = True Then
                                '    If l_datarow("AccountType").GetType.ToString <> "System.DBNull" Then
                                'If l_datarow("AccountType") = "RG" Or l_datarow("AccountType") = "SS" Or l_datarow("AccountType") = "SA" Then

                                ' If CType(l_datarow("IsBasicAccount"), Boolean) = True Then
                                'Emp.Total 
                                If CType(l_datarow("Emp.Total"), Decimal) > 0 Then
                                    l_decimalTaxable += CType(l_datarow("Taxable"), Decimal)
                                    l_decimalNonTaxable += CType(l_datarow("Non-Taxable"), Decimal)
                                    l_decimalInterest += CType(l_datarow("Interest"), Decimal)
                                    l_decimalTotal += CType(l_datarow("Emp.Total"), Decimal)
                                End If

                                'Added Ganeswar 10thApril09 for BA Account Phase-V /begin
                                If CType(l_datarow("YMCATotal"), Decimal) > 0 And CType(l_datarow("AccountGroup"), String) = m_const_RetirementPlan_BA Then
                                    l_dr_DisplayDatatable = l_FinalDatatable.NewRow()
                                    'l_dr_DisplayDatatable("AccountType") = "BA ER"
                                    l_dr_DisplayDatatable("AccountType") = m_const_YMCA__Acct
                                    l_dr_DisplayDatatable("AccountGroup") = l_datarow("AccountGroup")
                                    l_dr_DisplayDatatable("PlanType") = "Retirement"
                                    l_dr_DisplayDatatable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                    l_dr_DisplayDatatable("Taxable") = l_datarow("YMCATaxable")
                                    l_dr_DisplayDatatable("Non-Taxable") = 0
                                    l_dr_DisplayDatatable("Interest") = l_datarow("YMCAInterest")
                                    l_dr_DisplayDatatable("Total") = l_datarow("YMCATotal")
                                    l_FinalDatatable.Rows.Add(l_dr_DisplayDatatable)
                                ElseIf CType(l_datarow("YMCATotal"), Decimal) > 0 Then
                                    l_decimalYMCATaxable += CType(l_datarow("YMCATaxable"), Decimal)
                                    l_decimalYMCAInterest += CType(l_datarow("YMCAInterest"), Decimal)
                                    l_decimalYMCATotal += CType(l_datarow("YMCATotal"), Decimal)
                                End If
                                'Added Ganeswar 10thApril09 for BA Account Phase-V /End

                                'Commented code deleted by SG: 2012.03.16
                            Else

                                If CType(l_datarow("AccountGroup"), String) = m_const_RetirementPlan_AM Then
                                    If CType(l_datarow("Emp.Total"), Decimal) > 0 Then
                                        l_dr_DisplayDatatable = l_FinalDatatable.NewRow()
                                        l_dr_DisplayDatatable("AccountType") = "AM"
                                        l_dr_DisplayDatatable("AccountGroup") = l_datarow("AccountGroup")
                                        l_dr_DisplayDatatable("PlanType") = "RETIREMENT"
                                        l_dr_DisplayDatatable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                        l_dr_DisplayDatatable("Taxable") = l_datarow("Taxable")
                                        l_dr_DisplayDatatable("Non-Taxable") = l_datarow("Non-Taxable")
                                        l_dr_DisplayDatatable("Interest") = l_datarow("Interest")
                                        l_dr_DisplayDatatable("Total") = l_datarow("Emp.Total")
                                        'l_dr_DisplayDatatable("Selected") = l_datarow("Selected")
                                        l_FinalDatatable.Rows.Add(l_dr_DisplayDatatable)
                                    End If
                                    If CType(l_datarow("YMCATotal"), Decimal) > 0 Then
                                        l_dr_DisplayDatatable = l_FinalDatatable.NewRow()
                                        l_dr_DisplayDatatable("AccountType") = "AM-Matched"
                                        l_dr_DisplayDatatable("AccountGroup") = l_datarow("AccountGroup")
                                        l_dr_DisplayDatatable("PlanType") = "RETIREMENT"
                                        l_dr_DisplayDatatable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                        l_dr_DisplayDatatable("Taxable") = l_datarow("YMCATaxable")
                                        l_dr_DisplayDatatable("Non-Taxable") = 0
                                        l_dr_DisplayDatatable("Interest") = l_datarow("YMCAInterest")
                                        l_dr_DisplayDatatable("Total") = l_datarow("YMCATotal")
                                        'l_dr_DisplayDatatable("Selected") = l_datarow("Selected")
                                        l_FinalDatatable.Rows.Add(l_dr_DisplayDatatable)

                                    End If
                                Else
                                    If CType(l_datarow("AccountGroup"), String) = m_const_SavingsPlan_TM Then
                                        If CType(l_datarow("Emp.Total"), Decimal) > 0 Then
                                            l_dr_DisplayDatatable = l_FinalDatatable.NewRow()
                                            l_dr_DisplayDatatable("AccountType") = "TM"
                                            l_dr_DisplayDatatable("AccountGroup") = l_datarow("AccountGroup")
                                            l_dr_DisplayDatatable("PlanType") = "SAVINGS"
                                            l_dr_DisplayDatatable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                            l_dr_DisplayDatatable("Taxable") = l_datarow("Taxable")
                                            l_dr_DisplayDatatable("Non-Taxable") = l_datarow("Non-Taxable")
                                            l_dr_DisplayDatatable("Interest") = l_datarow("Interest")
                                            l_dr_DisplayDatatable("Total") = l_datarow("Emp.Total")
                                            'l_dr_DisplayDatatable("Selected") = l_datarow("Selected")
                                            l_FinalDatatable.Rows.Add(l_dr_DisplayDatatable)
                                        End If
                                        If CType(l_datarow("YMCATotal"), Decimal) > 0 Then
                                            l_dr_DisplayDatatable = l_FinalDatatable.NewRow()
                                            l_dr_DisplayDatatable("AccountType") = "TM-Matched"
                                            l_dr_DisplayDatatable("AccountGroup") = l_datarow("AccountGroup")
                                            l_dr_DisplayDatatable("PlanType") = "SAVINGS"
                                            l_dr_DisplayDatatable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                            l_dr_DisplayDatatable("Taxable") = l_datarow("YMCATaxable")
                                            l_dr_DisplayDatatable("Non-Taxable") = 0
                                            l_dr_DisplayDatatable("Interest") = l_datarow("YMCAInterest")
                                            l_dr_DisplayDatatable("Total") = l_datarow("YMCATotal")
                                            'l_dr_DisplayDatatable("Selected") = l_datarow("Selected")
                                            l_FinalDatatable.Rows.Add(l_dr_DisplayDatatable)

                                        End If
                                    Else

                                        If CType(l_datarow("YMCATotal"), Decimal) > 0 Then
                                            l_dr_DisplayDatatable = l_FinalDatatable.NewRow()
                                            l_dr_DisplayDatatable("AccountType") = l_datarow("AccountType")
                                            l_dr_DisplayDatatable("AccountGroup") = l_datarow("AccountGroup")
                                            l_dr_DisplayDatatable("PlanType") = l_datarow("PlanType")
                                            l_dr_DisplayDatatable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                            l_dr_DisplayDatatable("Taxable") = l_datarow("YMCATaxable")
                                            l_dr_DisplayDatatable("Non-Taxable") = 0
                                            l_dr_DisplayDatatable("Interest") = l_datarow("YMCAInterest")
                                            l_dr_DisplayDatatable("Total") = l_datarow("Total")
                                            ' l_dr_DisplayDatatable("Selected") = l_datarow("Selected")
                                            l_FinalDatatable.Rows.Add(l_dr_DisplayDatatable)

                                        Else
                                            l_dr_DisplayDatatable = l_FinalDatatable.NewRow()
                                            l_dr_DisplayDatatable("AccountType") = l_datarow("AccountType")
                                            l_dr_DisplayDatatable("AccountGroup") = l_datarow("AccountGroup")
                                            l_dr_DisplayDatatable("PlanType") = l_datarow("PlanType")
                                            l_dr_DisplayDatatable("IsBasicAccount") = l_datarow("IsBasicAccount")
                                            l_dr_DisplayDatatable("Taxable") = l_datarow("Taxable")
                                            l_dr_DisplayDatatable("Non-Taxable") = l_datarow("Non-Taxable")
                                            l_dr_DisplayDatatable("Interest") = l_datarow("Interest")
                                            l_dr_DisplayDatatable("Total") = l_datarow("Total")
                                            ' l_dr_DisplayDatatable("Selected") = l_datarow("Selected")
                                            l_FinalDatatable.Rows.Add(l_dr_DisplayDatatable)
                                        End If
                                    End If
                                End If
                            End If
                            ' End If
                        End If
                    Next
                    If l_decimalTotal > 0 Then
                        l_dr_DisplayDatatable = l_FinalDatatable.NewRow()
                        l_dr_DisplayDatatable("AccountType") = "Personal"
                        l_dr_DisplayDatatable("AccountGroup") = "BAG" 'todo
                        l_dr_DisplayDatatable("PlanType") = "RETIREMENT"
                        l_dr_DisplayDatatable("IsBasicAccount") = "True"
                        l_dr_DisplayDatatable("Taxable") = l_decimalTaxable
                        l_dr_DisplayDatatable("Non-Taxable") = l_decimalNonTaxable
                        l_dr_DisplayDatatable("Interest") = l_decimalInterest
                        l_dr_DisplayDatatable("Total") = l_decimalTotal
                        'l_dr_DisplayDatatable("Selected") = True
                        'l_FinalDatatable.Rows.Add(l_dr_DisplayDatatable)
                        l_FinalDatatable.Rows.InsertAt(l_dr_DisplayDatatable, 0)
                    End If
                    If l_decimalYMCATotal > 0 Then
                        l_dr_DisplayDatatable = l_FinalDatatable.NewRow()
                        'l_dr_DisplayDatatable("AccountType") = "YMCA"
                        l_dr_DisplayDatatable("AccountType") = m_const_YMCA_Legacy_Acct
                        l_dr_DisplayDatatable("AccountGroup") = "BAG" 'todo
                        l_dr_DisplayDatatable("PlanType") = "RETIREMENT"
                        l_dr_DisplayDatatable("IsBasicAccount") = "True"
                        l_dr_DisplayDatatable("Taxable") = l_decimalYMCATaxable
                        l_dr_DisplayDatatable("Non-Taxable") = 0
                        l_dr_DisplayDatatable("Interest") = l_decimalYMCAInterest
                        l_dr_DisplayDatatable("Total") = l_decimalYMCATotal
                        'l_dr_DisplayDatatable("Selected") = True
                        'l_FinalDatatable.Rows.Add(l_dr_DisplayDatatable)
                        l_FinalDatatable.Rows.InsertAt(l_dr_DisplayDatatable, 1)
                    End If
                End If
            End If
            l_FinalDatatable.AcceptChanges()
            Return l_FinalDatatable

        Catch ex As Exception
            Throw

        End Try
    End Function



    Private Function LoadAccountContribution()

        Dim l_DataTable As DataTable
        Dim l_finaldisplaydatatable As New DataTable

        Try


            l_DataTable = DirectCast(Session("AccountContribution_C19"), DataTable)



            l_DataTable = DirectCast(Session("AccountContribution_NonFund_C19"), DataTable)
            'by aparna modify display tables 29/10/2007

            l_finaldisplaydatatable = Me.CreatDisplayTables(l_DataTable)

            Me.CalculateTotal(l_DataTable)

            ' Me.DatagridNonFundedContributions.DataSource = l_DataTable
            ' Me.DatagridNonFundedContributions.DataBind()
            l_finaldisplaydatatable = Me.CalculateTotalForDisplay(l_finaldisplaydatatable)
            Me.DatagridNonFundedContributions.DataSource = l_finaldisplaydatatable
            Me.DatagridNonFundedContributions.DataBind()

            'Me.SetSelectedIndex(Me.DatagridNonFundedContributions, l_DataTable)
            Me.SetSelectedIndex(Me.DatagridNonFundedContributions, l_finaldisplaydatatable)



        Catch ex As Exception
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
            Throw
        End Try
    End Function

    Private Sub DataGridRequestedAccts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridRequestedAccts.ItemDataBound
        Try
            e.Item.Cells(0).Visible = False 'isbasicaccount
            ' e.Item.Cells(10).Visible = True 'plan type
            e.Item.Cells(2).Visible = False 'acctgroup
            If e.Item.ItemType <> ListItemType.Header Then

                If e.Item.Cells(0).Text.ToUpper <> "&NBSP;" Or e.Item.Cells(1).Text.ToUpper = "TOTAL" Then
                    'If e.Item.Cells(1).Text.ToUpper = "TOTAL" Then
                    Dim l_decimal_try As Decimal
                    'l_decimal_try = Convert.ToDecimal(e.Item.Cells(2).Text)
                    'e.Item.Cells(2).Text = FormatCurrency(l_decimal_try)

                    'l_decimal_try = Convert.ToDecimal(e.Item.Cells(3).Text)
                    'e.Item.Cells(3).Text = FormatCurrency(l_decimal_try)

                    l_decimal_try = Convert.ToDecimal(e.Item.Cells(4).Text)
                    e.Item.Cells(4).Text = FormatCurrency(l_decimal_try)

                    l_decimal_try = Convert.ToDecimal(e.Item.Cells(5).Text)
                    e.Item.Cells(5).Text = FormatCurrency(l_decimal_try)

                    l_decimal_try = Convert.ToDecimal(e.Item.Cells(6).Text)
                    e.Item.Cells(6).Text = FormatCurrency(l_decimal_try)

                    l_decimal_try = Convert.ToDecimal(e.Item.Cells(7).Text)
                    e.Item.Cells(7).Text = FormatCurrency(l_decimal_try)

                    'l_decimal_try = Convert.ToDecimal(e.Item.Cells(8).Text)
                    'e.Item.Cells(8).Text = FormatCurrency(l_decimal_try)

                    'l_decimal_try = Convert.ToDecimal(e.Item.Cells(9).Text)
                    'e.Item.Cells(9).Text = FormatCurrency(l_decimal_try)
                End If
            End If


        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub DatagridCurrentAccts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridCurrentAccts.ItemDataBound
        Try

            e.Item.Cells(0).Visible = False 'isbasicaccount
            'e.Item.Cells(10).Visible = True 'plan type
            e.Item.Cells(2).Visible = False 'account group
            'e.Item.Cells(12).Visible = False
            If e.Item.ItemType <> ListItemType.Header Then
                If e.Item.Cells(0).Text.ToUpper <> "&NBSP;" Or e.Item.Cells(1).Text.ToUpper = "TOTAL" Then
                    'If e.Item.Cells(1).Text.ToUpper = "TOTAL" Then
                    Dim l_decimal_try As Decimal
                    'l_decimal_try = Convert.ToDecimal(e.Item.Cells(2).Text)
                    'e.Item.Cells(2).Text = FormatCurrency(l_decimal_try)

                    'l_decimal_try = Convert.ToDecimal(e.Item.Cells(3).Text)
                    'e.Item.Cells(3).Text = FormatCurrency(l_decimal_try)

                    l_decimal_try = Convert.ToDecimal(e.Item.Cells(4).Text)
                    e.Item.Cells(4).Text = FormatCurrency(l_decimal_try)

                    l_decimal_try = Convert.ToDecimal(e.Item.Cells(5).Text)
                    e.Item.Cells(5).Text = FormatCurrency(l_decimal_try)

                    l_decimal_try = Convert.ToDecimal(e.Item.Cells(6).Text)
                    e.Item.Cells(6).Text = FormatCurrency(l_decimal_try)

                    l_decimal_try = Convert.ToDecimal(e.Item.Cells(7).Text)
                    e.Item.Cells(7).Text = FormatCurrency(l_decimal_try)

                    'l_decimal_try = Convert.ToDecimal(e.Item.Cells(8).Text)
                    'e.Item.Cells(8).Text = FormatCurrency(l_decimal_try)

                    'l_decimal_try = Convert.ToDecimal(e.Item.Cells(9).Text)
                    'e.Item.Cells(9).Text = FormatCurrency(l_decimal_try)
                End If
            End If
        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Function IsBasicAccount(ByVal parameterDataRow As DataRow) As Boolean

        Try

            If parameterDataRow("IsBasicAccount").GetType.ToString() = "System.DBNull" Then Return False

            If CType(parameterDataRow("IsBasicAccount"), Boolean) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function LoadCalculatedTableForCurrentAccounts()

        'Commented code deleted by SG: 2012.03.16
        Dim l_CalculatedDataTable As DataTable
        Dim l_displayCalculatedDatatable As DataTable


        Try
            'Commented code deleted by SG: 2012.03.16
            l_CalculatedDataTable = DirectCast(Session("CalculatedDataTableForCurrentAccounts_C19"), DataTable)
            'Hafiz 03Feb06 Cache-Session

            'by aparna 'modify display tables
            l_displayCalculatedDatatable = Me.CreatDisplayTables(l_CalculatedDataTable)


            'Me.CalculateTotal(l_CalculatedDataTable)
            l_displayCalculatedDatatable = Me.CalculateTotalForDisplay(l_displayCalculatedDatatable)

            'Commented code deleted by SG: 2012.03.16

            Me.CalculateTotal(l_CalculatedDataTable)

            Me.DatagridCurrentAccts.DataSource = l_displayCalculatedDatatable
            Me.DatagridCurrentAccts.DataBind()

            Me.SetSelectedIndex(DatagridCurrentAccts, l_displayCalculatedDatatable)




        Catch ex As Exception
            Throw
        End Try

    End Function

#Region " Load Personal Inforamation "




    Private Function LoadInformation()

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        'Commented code deleted by SG: 2012.03.16

        Try
            'PersonInformation

            'Hafiz 03Feb06 Cache-Session
            'Commented code deleted by SG: 2012.03.16
            l_DataSet = Session("PersonInformation_C19")
            'Hafiz 03Feb06 Cache-Session

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Member Details")

                If Not l_DataTable Is Nothing Then
                    If l_DataTable.Rows.Count > 0 Then
                        Me.LoadGeneralInfoToControls(l_DataTable.Rows.Item(0))
                    End If
                End If

                'Commented code deleted by SG: 2012.03.16

                ' To Fill Address Inforamation in General Tab Page
                l_DataTable = l_DataSet.Tables("Member Address")

                If Not l_DataTable Is Nothing Then
                    If l_DataTable.Rows.Count > 0 Then
                        Me.LoadAddressInfoToControls(l_DataTable.Rows.Item(0))
                    End If
                End If


            End If

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function LoadAddressInfoToControls(ByVal parameterDataRow As DataRow)
        Dim l_str_zipcode As String
        Try
            If Not parameterDataRow Is Nothing Then

                Me.PayeeAddressID = CType(parameterDataRow("AddressID"), String)

                If (parameterDataRow("Address1").ToString = "") Then
                    'AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                    'Me.TextboxAddress1.Text = String.Empty
                    Me.AddressWebUserControl1.Address1 = String.Empty
                Else
                    'AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                    'Me.TextboxAddress1.Text = CType(parameterDataRow("Address1"), String).Trim.Replace(",", "")
                    Me.AddressWebUserControl1.Address1 = CType(parameterDataRow("Address1"), String).Trim.Replace(",", "")
                End If

                If (parameterDataRow("Address2").ToString = "") Then
                    'AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                    'Me.TextboxAddress2.Text = String.Empty
                    Me.AddressWebUserControl1.Address2 = String.Empty
                Else
                    'AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                    'Me.TextboxAddress2.Text = CType(parameterDataRow("Address2"), String).Trim.Replace(",", "")
                    Me.AddressWebUserControl1.Address2 = CType(parameterDataRow("Address2"), String).Trim.Replace(",", "")
                End If

                If (parameterDataRow("Address3").ToString = "") Then
                    'AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                    'Me.TextboxAddress3.Text = String.Empty
                    Me.AddressWebUserControl1.Address3 = String.Empty
                Else
                    'AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                    'Me.TextboxAddress3.Text = CType(parameterDataRow("Address3"), String).Trim.Replace(",", "")
                    Me.AddressWebUserControl1.Address3 = CType(parameterDataRow("Address3"), String).Trim.Replace(",", "")
                End If

                'Commented code deleted by SG: 2012.03.16

                If (parameterDataRow("City").ToString = "") Then
                    'AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                    'Me.TextboxCity1.Text = String.Empty

                    Me.AddressWebUserControl1.City = String.Empty
                Else
                    'AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                    'Me.TextboxCity1.Text = CType(parameterDataRow("City"), String).Trim.Replace(",", "")
                    Me.AddressWebUserControl1.City = CType(parameterDataRow("City"), String).Trim.Replace(",", "")
                End If

                If (parameterDataRow("Country").ToString = "") Then
                    'AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                    'Me.TextBoxCountry.Text = String.Empty
                    Me.AddressWebUserControl1.DropDownListCountryValue = String.Empty
                Else
                    'AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                    'Me.TextBoxCountry.Text = CType(parameterDataRow("CountryName"), String).Trim.Replace(",", "")
                    Me.AddressWebUserControl1.DropDownListCountryValue = CType(parameterDataRow("Country"), String).Trim.Replace(",", "")
                End If

                'Added By Ashutosh Patil as on 11-Apr-2007
                'YREN-3028,YREN-3029
                If (parameterDataRow("StateType").ToString = "") Then
                    'AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                    'Me.TextBoxState.Text = String.Empty
                    Me.AddressWebUserControl1.DropDownListStateValue = String.Empty
                Else
                    'AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                    'Me.TextBoxState.Text = CType(parameterDataRow("StateName"), String).Trim.Replace(",", "")
                    Me.AddressWebUserControl1.DropDownListStateValue = CType(parameterDataRow("StateType"), String).Trim.Replace(",", "")
                End If


                If parameterDataRow("CountryName").ToString = "CANADA" Then
                    l_str_zipcode = CType(parameterDataRow("Zip"), String).Trim.Replace(",", "")
                    'AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                    'Me.TextBoxZip.Text = l_str_zipcode.Substring(0, 3) + " " + l_str_zipcode.Substring(3, 3)
                    Me.AddressWebUserControl1.ZipCode = l_str_zipcode.Substring(0, 3) + " " + l_str_zipcode.Substring(3, 3)
                Else
                    'AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                    'Me.TextBoxZip.Text = CType(parameterDataRow("Zip"), String).Trim.Replace(",", "")
                    Me.AddressWebUserControl1.ZipCode = CType(parameterDataRow("Zip"), String).Trim.Replace(",", "")
                End If

                'Start:AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                If Me.AddressWebUserControl1.Address1 <> "" Then
                    Me.AddressWebUserControl1.Address1 = Me.AddressWebUserControl1.Address1 + ","
                End If
                If Me.AddressWebUserControl1.Address2 <> "" Then
                    Me.AddressWebUserControl1.Address2 = Me.AddressWebUserControl1.Address2 + ","
                End If
                If Me.AddressWebUserControl1.Address3 <> "" Then
                    Me.AddressWebUserControl1.Address3 = Me.AddressWebUserControl1.Address3 + ","
                End If
                If Me.AddressWebUserControl1.City <> "" Then
                    Me.AddressWebUserControl1.City = Me.AddressWebUserControl1.City + ","
                End If
                If Me.AddressWebUserControl1.DropDownListStateValue <> "" Then
                    Me.AddressWebUserControl1.DropDownListStateValue = Me.AddressWebUserControl1.DropDownListStateValue
                End If
                If Me.AddressWebUserControl1.ZipCode <> "" Then
                    If Me.AddressWebUserControl1.ZipCode.Length = 9 And Me.AddressWebUserControl1.DropDownListCountryValue = "US" Then
                        Me.AddressWebUserControl1.ZipCode = Me.AddressWebUserControl1.ZipCode.Substring(0, 5) + "-" + Me.AddressWebUserControl1.ZipCode.Substring(5, 4)
                    End If
                    Me.AddressWebUserControl1.ZipCode = ", " + Me.AddressWebUserControl1.ZipCode
                End If
                'End:AA:24.09.2013 : BT-1501: Changes made For Displaying address with address control not with textboxes
                'Commented code deleted by SG: 2012.03.16

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    'Commented code deleted by SG: 2012.03.16

    Private Function LoadGeneralInfoToControls(ByVal parameterDataRow As DataRow)
        Try
            Dim l_personage As Decimal
            Dim l_QDROParticipantAge As Decimal
            If Not parameterDataRow Is Nothing Then

                'Commented code deleted by SG: 2012.03.16

                Me.TextBoxPayee1.Text = CType(parameterDataRow("Last Name"), String) & " " & CType(parameterDataRow("Middle Name"), String) & " " & CType(parameterDataRow("First Name"), String)
                Me.TextBoxPayee1.ReadOnly = True

                'Shubhrata While generalising the code in Refunds.vb class
                ' we will set the value of this text box in a property so that it can be used by the common class
                objRefundProcess.Payee1Name = Me.TextBoxPayee1.Text.ToString()
                'this Payee1Name property will be used by Refunds.vb class 
                'Me.TextboxPayee2.Text = ""
                'Me.TextboxPayee3.Text = ""

                If (parameterDataRow("VestingDate").GetType.ToString = "System.DBNull") Then
                    Me.IsVested = False
                Else
                    Me.IsVested = True
                End If
                'Use of select case implemented for new fund event status - Shubhrata  April 17, 2008 
                If (parameterDataRow("StatusType").GetType.ToString = "System.DBNull") Then
                    Me.IsTerminated = False
                    Me.IsRetiredActive = False
                Else
                    Dim l_string_FundEventStatus As String = ""
                    l_string_FundEventStatus = CType(parameterDataRow("StatusType"), String).Trim.ToUpper()
                    Me.FundEventStatus = l_string_FundEventStatus  'START - Chandra sekar | 2016.03.01 | YRS-AT-2599 - Retirement Processing screen:Annuity purchase, PT and RPT status validation (TrackIT 23822)
                    Select Case l_string_FundEventStatus
                        Case "RA"
                            Me.IsRetiredActive = True
                            Me.IsTerminated = False
                        Case "QD"
                            If (parameterDataRow("BirthDate").GetType.ToString = "System.DBNull") Then
                            ElseIf (parameterDataRow("DeathDate").GetType.ToString = "System.DBNull") Then
                                l_personage = ConvertFormatFromBase12toBase10(Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), String.Empty))
                            Else
                                l_personage = ConvertFormatFromBase12toBase10(Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), CType(parameterDataRow("DeathDate"), String)))
                            End If
                            If Not Session("QDROParticipantAge_C19") Is Nothing Then
                                l_QDROParticipantAge = CType(Session("QDROParticipantAge_C19"), Decimal)
                            End If
                            If l_personage < 55 Or l_QDROParticipantAge < 50 Then
                                Me.IsVested = True
                                Me.IsTerminated = True
                            End If
                        Case "AE", "PE", "RE"
                            Me.IsTerminated = False
                            Me.IsRetiredActive = False
                            'Shubhrata  05/22/2008  bug id 455 'ML will be non-terminated
                        Case "ML"
                            Me.IsTerminated = False
                            Me.IsRetiredActive = False
                            'Shubhrata  05/22/2008  bug id 455 'ML will be non-terminated
                        Case l_string_FundEventStatus
                            Me.IsTerminated = True
                            Me.IsRetiredActive = False
                    End Select
                End If
                'Commented code deleted by SG: 2012.03.16

                If (parameterDataRow("BirthDate").GetType.ToString = "System.DBNull") Then
                    Me.PersonAge = 0.0
                ElseIf (parameterDataRow("DeathDate").GetType.ToString = "System.DBNull") Then
                    Me.PersonAge = ConvertFormatFromBase12toBase10(Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), String.Empty))
                Else
                    Me.PersonAge = ConvertFormatFromBase12toBase10(Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), CType(parameterDataRow("DeathDate"), String)))


                End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function CalculateAge(ByVal parameterDOB As String, ByVal parameterDOD As String) As Decimal
        Dim numTotalNumberofDays As Decimal
        Dim numAge As Decimal
        Dim numReminder As Decimal
        Try
            'Commented code deleted by SG: 2012.03.16
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
            Throw
        End Try
    End Function
    'Added by imran on 11/APR/2010 for YRS 5.0-1075 Age not being treated correctly in withdrawal processing
    Private Function ConvertFormatFromBase12toBase10(ByVal val As Decimal) As Decimal
        Try
            Dim ReturnValue As Decimal = val
            Dim remainder As Decimal = ReturnValue - Math.Floor(ReturnValue)
            remainder = remainder * 100 / 12
            Return Math.Floor(ReturnValue) + remainder
        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region " Voluntry Refund For Current Accounts "

    Private Function DoVoluntryRefundForCurrentAccounts()

        'Commented code deleted by SG: 2012.03.16
        Dim l_ContributionDataTable As DataTable
        Dim l_DataRow As DataRow

        Dim l_UserSide As Boolean
        Dim l_YMCASide As Boolean

        Dim l_AccountGroup As String


        Try

            l_ContributionDataTable = Session("CalculatedDataTableForCurrentAccounts_C19")

            If Not l_ContributionDataTable Is Nothing Then
                l_ContributionDataTable = objRefundProcess.DoFlagSetForVoluntryRefund(l_ContributionDataTable, True)

                Me.VoluntaryWithdrawalTotal = objRefundProcess.VoluntryAmount
                Session("CalculatedDataTableForCurrentAccounts_C19") = l_ContributionDataTable
                Me.CalculateTotal(l_ContributionDataTable)
                Me.LoadCalculatedTableForCurrentAccounts()
                'Me.SetSelectedIndex(Me.DatagridCurrentAccts, l_ContributionDataTable)
            End If
        Catch ex As Exception
            Throw
        End Try

    End Function

#End Region

    Private Sub DataGridPayee1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridPayee1.ItemDataBound
        Try

            'Commented code deleted by SG: 2012.03.16
            If e.Item.ItemType = ListItemType.Header Then
                e.Item.Cells(0).Text = "AcctType"
                e.Item.Cells(1).Text = "Taxable"
                e.Item.Cells(2).Text = "NonTaxable"
            End If
            'Added by Ganeswar for Hardship rollover on 25-05-2009
            LoadPayee1ValuesIntoControls()
            'Added by Ganeswar for Hardship rollover on 25-05-2009
        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Sub DatagridPayee2_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridPayee2.ItemDataBound
        Try
            'Commented code deleted by SG: 2012.03.16
            If e.Item.ItemType = ListItemType.Header Then
                e.Item.Cells(0).Text = "AcctType"
                e.Item.Cells(1).Text = "Taxable"
                e.Item.Cells(2).Text = "NonTaxable"
            End If
            'Added by Ganeswar for Hardship rollover on 25-05-2009
            LoadPayee2ValuesIntoControls()
            'Added by Ganeswar for Hardship rollover on 25-05-2009

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Sub DatagridPayee3_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridPayee3.ItemDataBound
        Try

            If e.Item.ItemType = ListItemType.Header Then
                e.Item.Cells(0).Text = "AcctType"
                e.Item.Cells(1).Text = "Taxable"
                e.Item.Cells(2).Text = "NonTaxable"
            End If
            'Added by Ganeswar for Hardship rollover on 25-05-2009
            LoadPayee3ValuesIntoControls()
            'Added by Ganeswar for Hardship rollover on 25-05-2009
        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Sub TextboxTaxRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxTaxRate.TextChanged

        Dim l_TaxRateInteger As Integer

        Try

            If Me.TextboxTaxRate.Text.Trim = String.Empty Then
                Me.TextboxTaxRate.Text = 0
            End If

            '  Me.TaxRate = Me.TextboxTaxRate.Text

            If Convert.ToDouble(Me.TextboxTaxRate.Text) > 100 Or Convert.ToDouble(Me.TextboxTaxRate.Text < 20) Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Invalid Tax Rate entered. Tax Rate should between 20% to 100%.", MessageBoxButtons.Stop, False)
                'Start: AA: 11.20.2015 YRS-AT-2639 Added to use the meta configuration key value 
                'Me.TaxRate = 20
                Me.TaxRate = FedaralTaxrate
                'End: AA: 11.20.2015 YRS-AT-2639 Added to use the meta configuration key value 
                Me.TextboxTaxRate.Text = Me.TaxRate
            Else
                Me.TaxRate = CType(Me.TextboxTaxRate.Text, Integer)
                Me.TextboxTaxRate.Text = Me.TaxRate
            End If

            Dim l_DataTable As DataTable
            l_DataTable = CType(Session("Payee1DataTable_C19"), DataTable)
            Dim l_datarow As DataRow
            For Each l_datarow In l_DataTable.Rows
                l_datarow("TaxRate") = Me.TaxRate
            Next
            ' we will set the tax rate property in the refunds class as this is to be used by Create payees function
            objRefundProcess.TaxRate = Me.TaxRate
            Me.LoadPayee1ValuesIntoControls()
            If Me.RefundType = "HARD" Then
                Me.LoadValuesForProcessedAmounts(False)
            End If
            Me.DoFinalCalculation()

        Catch caEx As System.InvalidCastException
            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Invalid Tax Rate entered. Please enter the Valid Tax Rate.", MessageBoxButtons.Stop, False)
            'Start: AA: 11.20.2015 YRS-AT-2639 Added to use the meta configuration key value 
            'Me.TaxRate = 20
            Me.TaxRate = FedaralTaxrate
            'End: AA: 11.20.2015 YRS-AT-2639 Added to use the meta configuration key value 
            objRefundProcess.TaxRate = Me.TaxRate
            Me.TextboxTaxRate.Text = Me.TaxRate

        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub RadioButtonNone_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonNone.CheckedChanged
        If Me.RadioButtonNone.Checked = True Then
            If Me.ISMarket = -1 Then
                Me.RadiobuttonRolloverAllInstallment.Checked = False
                Me.RadiobuttonRolloverOnlyFirstInstallment.Checked = True
                EnableDisablePartialRolloverControl(False)
            End If
            TextboxRolloverAmount.Text = String.Empty
            TextboxRolloverAmount.Enabled = False
            Me.DoNone()
            'Vipul
            'Me.TextboxPayee2.ReadOnly = True

        End If
    End Sub

    Private Sub RadioButtonRolloverAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonRolloverAll.CheckedChanged

        If Me.RadioButtonRolloverAll.Checked = True Then

            If Me.ISMarket = -1 Then
                EnableDisablePartialRolloverControl(False) 'Added by Parveen for MKT withdrawal on 16-Dec-2009
            End If
            'START | SR | 2016.01.22 | YRS-AT-2664 | If taxable RMD amount allowed to be rollover then reset RMD values
            If (ViewState("AllowRMDAmountToBeRollover") = "True") Then
                If (ViewState("IsRolloverTaxableWasSelected") = "True") Then
                    Session("MinimumDistributionTable_C19") = Session("MinimumDistributionTable_temp_C19")
                    ViewState("IsRolloverTaxableWasSelected") = "False"
                    EnableMinimumDistributionControls()
                End If
            End If
            'END | SR | 2016.01.22 | YRS-AT-2664 | If taxable RMD amount allowed to be rollover then reset RMD values
            Me.DoRolloverAll()
            'Vipul
            If Me.RefundType = "REG" Then
                Me.TextboxPayee3.ReadOnly = True
                Me.TextboxPayee3.Text = String.Empty
            ElseIf Me.RefundType = "VOL" Or Me.RefundType = "HARD" Or Me.RefundType = "SPEC" Then
                Me.TextboxPayee2.rReadOnly = False 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
            Else
                Me.TextboxPayee3.ReadOnly = True
                Me.TextboxPayee3.Text = String.Empty
                Me.TextboxPayee2.rReadOnly = False 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
            End If
            TextboxRolloverAmount.Text = String.Empty
            TextboxRolloverAmount.Enabled = False

        End If
        Me.DoFinalCalculation()
    End Sub

    Private Sub RadioButtonTaxableOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonTaxableOnly.CheckedChanged
        Try ' SR | 2017.11.28 | YRS-AT-3742 | Add Try catch block
            If Me.RadioButtonTaxableOnly.Checked = True Then
                If Me.ISMarket = -1 Then
                    EnableDisablePartialRolloverControl(False) 'Added by Dilip for MKT withdrawal on 01-11-2009
                End If
                Me.DoTaxableOnly()
                If Me.RefundType = "VOL" Or Me.RefundType = "HARD" Then
                    Me.TextboxPayee2.rReadOnly = False 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
                Else
                    Me.TextboxPayee3.ReadOnly = True
                    Me.TextboxPayee3.Text = String.Empty
                    Me.TextboxPayee2.Text = String.Empty
                End If
                'BT:2672/YRS 5.0-2422: Rollover amount textbox should be blank and read only
                TextboxRolloverAmount.Text = String.Empty
                TextboxRolloverAmount.Enabled = False
                'BT:2672/YRS 5.0-2422: Rollover amount textbox should be blank and read only

                'START | SR | 2017.05.09 | 20.2.0 | YRS-AT-3173 - If user select NO to include taxable money in participant account then revert taxable money option in rollover
                If Not ViewState("AllowRMDAmountToBeRollover") Is Nothing Then
                    If (ViewState("AllowRMDAmountToBeRollover").ToString().ToUpper = "FALSE" AndAlso (Convert.ToDecimal(TextboxMinDistAmount.Text) > 0)) Then
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", GetGlobalResourceObject("Withdrawal", "MESSAGE_WD_RMD_WARNING_PARTICIPANTAMOUNTINCLUDETAXABLEMONEY").ToString(), MessageBoxButtons.YesNo, False)
                        DisplayedParticipantTaxableMoneyMessage = True
                        Exit Sub
                    End If
                End If
                'END | SR | 2017.05.09 | 20.2.0 | YRS-AT-3173 - If user select NO to include taxable money in participant account then revert taxable money option in rollover

            End If
            ' START | SR | 2017.11.28 | YRS-AT-3742 | logging exceptions
        Catch ex As Exception
            Dim exceptionMessage As String
            exceptionMessage = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + exceptionMessage, False)
        End Try
        ' END | SR | 2017.11.28 | YRS-AT-3742 | logging exceptions

    End Sub

    'dO NONE for hardship is not being integrated for the moment
    ' we will test if it works fine with regular one.
    Private Function DoNone()

        Dim l_Payee1DataTable As DataTable
        Dim l_Payee1TempDataTable As New DataTable
        Dim l_PayeeDataTable As DataTable
        Dim l_Payee2DataTable As DataTable
        Dim l_Payee3Datatable As DataTable
        Try
            'Added comment by Ganeswar For HardShip RollOver on 21-05-2009
            'If Me.RefundType = "HARD"  Then 


            If Me.RefundType = "HARD" And TextboxVoluntaryWithdrawalTotal.Text = "0.00" Then
                'Added comment by Ganeswar For HardShip RollOver on 21-05-2009
                If chkCreatePayeeProcessed = False Then
                    objRefundProcess.CreatePayees()
                    Me.SetPropertiesAfterCreatePayees()
                End If
                If chkCreatePayeeProcessed = False Then
                    objRefundProcess.CreatePayees()
                    Me.SetPropertiesAfterCreatePayees()
                End If

                l_Payee1DataTable = DirectCast(Session("Payee1DataTable_C19"), DataTable)


                l_Payee1TempDataTable = l_Payee1DataTable.Clone
                Dim l_DataRow As DataRow
                'Copying data from table of Payee1 to Payee2 & 3
                For Each l_DataRow In l_Payee1DataTable.Rows
                    l_Payee1TempDataTable.ImportRow(l_DataRow)
                Next


                Session("Payee1DataTable_C19") = l_Payee1DataTable
                Session("Payee1TempDataTable_C19") = l_Payee1TempDataTable

                'Commented code deleted by SG: 2012.03.16
                If (CheckBoxRollovers.Checked = True And RadioButtonNone.Checked = True) Then
                    TextboxPayee2.rReadOnly = False 'commented by Preeti 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
                End If

                Me.LoadPayeesDataGrid()
                Me.LoadPayee1ValuesIntoControls()
                Me.DoFinalCalculation()

                Me.EnableDisableSaveButton()
            Else
                'Commented code deleted by SG: 2012.03.16
                Me.TextboxPayee3.ReadOnly = True
                Me.TextboxPayee3.Text = String.Empty
                Me.TextboxPayee2.Text = String.Empty

                l_PayeeDataTable = DirectCast(Session("Payee1DataTable_C19"), DataTable)


                If Not l_PayeeDataTable Is Nothing Then
                    l_PayeeDataTable.Rows.Clear()
                End If

                l_PayeeDataTable = DirectCast(Session("Payee2DataTable_C19"), DataTable)


                If Not l_PayeeDataTable Is Nothing Then
                    l_PayeeDataTable.Rows.Clear()
                End If


                l_PayeeDataTable = DirectCast(Session("Payee3DataTable_C19"), DataTable)


                If Not l_PayeeDataTable Is Nothing Then
                    l_PayeeDataTable.Rows.Clear()
                End If

                objRefundProcess.CreatePayees()


                l_Payee1DataTable = DirectCast(Session("Payee1DataTable_C19"), DataTable)
                l_Payee2DataTable = DirectCast(Session("Payee2DataTable_C19"), DataTable)
                l_Payee3Datatable = DirectCast(Session("Payee3DataTable_C19"), DataTable)


                l_Payee1TempDataTable = l_Payee1DataTable.Clone
                Dim l_DataRow As DataRow
                'Copying data from table of Payee1 to Payee2 & 3
                For Each l_DataRow In l_Payee1DataTable.Rows
                    l_Payee2DataTable.ImportRow(l_DataRow)
                    l_Payee3Datatable.ImportRow(l_DataRow)
                    l_Payee1TempDataTable.ImportRow(l_DataRow)
                Next

                For Each l_DataRow In l_Payee2DataTable.Rows
                    l_DataRow("Taxable") = "0.00"
                    l_DataRow("NonTaxable") = "0.00"
                    l_DataRow("Tax") = "0.00"
                    l_DataRow("TaxRate") = "0.00"
                Next

                For Each l_DataRow In l_Payee3Datatable.Rows
                    l_DataRow("Taxable") = "0.00"
                    l_DataRow("NonTaxable") = "0.00"
                    l_DataRow("Tax") = "0.00"
                    l_DataRow("TaxRate") = "0.00"
                Next

                Session("Payee1DataTable_C19") = l_Payee1DataTable
                Session("Payee2DataTable_C19") = l_Payee2DataTable
                Session("Payee3DataTable_C19") = l_Payee3Datatable
                Session("Payee1TempDataTable_C19") = l_Payee1TempDataTable


                TextboxPayee3.ReadOnly = False



                Me.LoadPayeesDataGrid()
                Me.LoadPayee1ValuesIntoControls()
                Me.LoadPayee2ValuesIntoControls()
                Me.LoadPayee3ValuesIntoControls()
                Me.DoFinalCalculation()

                Me.EnableDisableSaveButton()
            End If



        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function LoadDeductions()
        'Commented code deleted by SG: 2012.03.16
        Dim l_DataTable As DataTable

        Try
            'Commented code deleted by SG: 2012.03.16

            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetDeductions

            'Commented code deleted by SG: 2012.03.16

            Me.DatagridDeductions.DataSource = l_DataTable
            Me.DatagridDeductions.DataBind()

            'Commented code deleted by SG: 2012.03.16
            Session("DeductionsDataTable_C19") = l_DataTable
            'Hafiz 03Feb06 Cache-Session

        Catch ex As Exception
            Throw
        End Try
    End Function

    ' todo : we have to test the Rollover all as it has been changed wrt to payee 3 datatable
    Private Function DoRolloverAll()


        Dim l_Payee1DataTable As DataTable
        Dim l_Payee2DataTable As DataTable
        Dim l_Payee3DataTable As DataTable
        Dim l_Payee1TempDataTable As New DataTable

        TextboxPayee3.ReadOnly = True

        Try

            If Me.RefundType = "HARD" And TextboxVoluntaryWithdrawalTotal.Text = "0.00" Then
                If chkCreatePayeeProcessed = False Then
                    objRefundProcess.CreatePayees()
                    Me.SetPropertiesAfterCreatePayees()
                End If
            Else
                objRefundProcess.CreatePayees()
            End If


            l_Payee1DataTable = DirectCast(Session("Payee1DataTable_C19"), DataTable)
            l_Payee2DataTable = DirectCast(Session("Payee2DataTable_C19"), DataTable)
            l_Payee3DataTable = DirectCast(Session("Payee3DataTable_C19"), DataTable)


            If Not l_Payee1DataTable Is Nothing Then

                If l_Payee2DataTable Is Nothing Then
                    l_Payee2DataTable = l_Payee1DataTable.Clone

                End If
                If l_Payee3DataTable Is Nothing Then
                    l_Payee3DataTable = l_Payee1DataTable.Clone
                End If

                l_Payee1TempDataTable = l_Payee1DataTable.Clone

                l_Payee1TempDataTable.Rows.Clear()
                l_Payee2DataTable.Rows.Clear()
                l_Payee3DataTable.Rows.Clear()

                For Each l_DataRow As DataRow In l_Payee1DataTable.Rows
                    l_Payee2DataTable.ImportRow(l_DataRow)
                    l_Payee3DataTable.ImportRow(l_DataRow)
                    l_Payee1TempDataTable.ImportRow(l_DataRow)
                Next

                Session("Payee1TempDataTable_C19") = l_Payee1TempDataTable



                For Each l_DataRow As DataRow In l_Payee1DataTable.Rows
                    l_DataRow("Taxable") = "0.00"
                    l_DataRow("NonTaxable") = "0.00"
                Next

                '' Just clear the Tax & Taxrate in the Payee 2 datatable.
                For Each l_DataRow As DataRow In l_Payee2DataTable.Rows
                    l_DataRow("TaxRate") = "0.00"
                    l_DataRow("Tax") = "0.00"
                Next

                '' Just clear the Tax & Taxrate in the Payee 3 datatable.
                For Each l_DataRow As DataRow In l_Payee3DataTable.Rows
                    l_DataRow("Taxable") = "0.00"
                    l_DataRow("NonTaxable") = "0.00"
                    l_DataRow("Tax") = "0.00"
                    l_DataRow("TaxRate") = "0.00"
                Next

            End If
            Session("Payee1DataTable_C19") = l_Payee1DataTable
            Session("Payee2DataTable_C19") = l_Payee2DataTable
            Session("Payee3DataTable_C19") = l_Payee3DataTable

            Session("Payee1TempDataTable_C19") = l_Payee1TempDataTable
            Me.LoadPayeesDataGrid()
            Me.LoadPayee1ValuesIntoControls()
            Me.LoadPayee2ValuesIntoControls()
            Me.LoadPayee3ValuesIntoControls()
            Me.DoFinalCalculation()
            Me.EnableDisableSaveButton()

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function DoTaxableOnly()

        Dim l_Payee1DataTable As DataTable
        Dim l_Payee2DataTable As DataTable
        Dim l_Payee3DataTable As DataTable

        Dim l_Payee1TempDataTable As DataTable

        Dim l_Payee2DataRow As DataRow
        ' START | SR | 2016.01.22 | YRS-AT-2664 - Declare Variables
        Dim dtCurrentAccountWithRMD As DataTable
        Dim dtCalculatedRMD As DataTable
        Dim dtRMDWithNonTaxableAmt As DataTable
        ' END | SR | 2016.01.22 | YRS-AT-2664 - Declare Variables

        Try

            'Added comment by Ganeswar For HardShip RollOver on 21-05-2009
            'If Me.RefundType = "HARD" Then
            If Me.RefundType = "HARD" And TextboxVoluntaryWithdrawalTotal.Text = "0.00" Then
                'End commnet by Ganeswar For HardShip RollOver on 21-05-2009
                If chkCreatePayeeProcessed = False Then
                    objRefundProcess.CreatePayees()
                    Me.SetPropertiesAfterCreatePayees()
                End If
            Else
                objRefundProcess.CreatePayees()
            End If


            l_Payee1DataTable = DirectCast(Session("Payee1DataTable_C19"), DataTable)
            l_Payee2DataTable = CType(Session("Payee2DataTable_C19"), DataTable)
            l_Payee3DataTable = CType(Session("Payee3DataTable_C19"), DataTable)

            If Not l_Payee1DataTable Is Nothing Then
                l_Payee1TempDataTable = l_Payee1DataTable.Clone
                If l_Payee2DataTable Is Nothing Then
                    l_Payee2DataTable = l_Payee1DataTable.Clone
                End If

                l_Payee2DataTable.Rows.Clear()


                If l_Payee3DataTable Is Nothing Then
                    l_Payee3DataTable = l_Payee1DataTable.Clone
                End If

                l_Payee3DataTable.Rows.Clear()


                For Each l_DataRow As DataRow In l_Payee1DataTable.Rows
                    l_Payee1TempDataTable.ImportRow(l_DataRow)
                    l_Payee2DataRow = l_Payee2DataTable.NewRow


                    l_Payee2DataRow("AcctType") = l_DataRow("AcctType")
                    l_Payee2DataRow("Taxable") = l_DataRow("Taxable")
                    l_Payee2DataRow("NonTaxable") = "0.00"
                    l_Payee2DataRow("TaxRate") = "0.00"
                    l_Payee2DataRow("Tax") = "0.00"
                    l_Payee2DataRow("Payee") = l_DataRow("Payee")
                    l_Payee2DataRow("FundedDate") = l_DataRow("FundedDate")
                    l_Payee2DataRow("RequestType") = l_DataRow("RequestType")
                    l_Payee2DataRow("RefRequestsID") = l_DataRow("RefRequestsID")

                    l_DataRow("Taxable") = "0.00"

                    l_Payee2DataTable.Rows.Add(l_Payee2DataRow)


                Next

                ' START | SR | 2017.12.12 | YRS-AT-3742 - Since now, RMD amount will be adjusted based on available non taxable amount, RMD taxable amount should not be rollover at any scenario. Hence, below line of codes are commented code.
                '' START | SR | 2016.01.22 | YRS-AT-2664 - Add RMD Taxable Amount in Rollover
                'If (ViewState("AllowRMDAmountToBeRollover") = "True") Then
                '    Dim dtCurrentAccountsWithMRDTaxable As DataTable
                '    Dim drPayee1() As DataRow
                '    Dim drRMD As DataRow
                '    Dim decPersTaxable As Decimal
                '    Dim decPersInterest As Decimal
                '    Dim decYMCATaxable As Decimal
                '    Dim decYMCAInterest As Decimal
                '    If Not Session("CurrentAccountsWithMRD") Is Nothing Then
                '        ' Get current account(including MRD amount) from session
                '        dtCurrentAccountsWithMRDTaxable = DirectCast(Session("CurrentAccountsWithMRD"), DataTable)
                '        ' Add RMD taxable amount in Payye1 taxable amount
                '        For i = 0 To l_Payee2DataTable.Rows.Count - 1
                '            drPayee1 = dtCurrentAccountsWithMRDTaxable.Select("AccountType='" + l_Payee2DataTable.Rows(i).Item("AcctType").ToString().Trim() + "'")
                '            If Not drPayee1 Is Nothing AndAlso drPayee1.Length > 0 Then
                '                decPersTaxable = IIf(IsDBNull(drPayee1(0)("Taxable")), 0, Convert.ToDecimal(drPayee1(0)("Taxable")))
                '                decPersInterest = IIf(IsDBNull(drPayee1(0)("Interest")), 0, Convert.ToDecimal(drPayee1(0)("Interest")))
                '                decYMCATaxable = IIf(IsDBNull(drPayee1(0)("YMCATaxable")), 0, Convert.ToDecimal(drPayee1(0)("YMCATaxable")))
                '                decYMCAInterest = IIf(IsDBNull(drPayee1(0)("YMCAInterest")), 0, Convert.ToDecimal(drPayee1(0)("YMCAInterest")))
                '                l_Payee2DataTable.Rows(i).Item("Taxable") = decPersTaxable + decPersInterest + decYMCATaxable + decYMCAInterest
                '            End If
                '        Next
                '        l_Payee2DataTable.AcceptChanges()
                '    End If
                '    ' store RMD amount temerorily in another session
                '    If Not Session("MinimumDistributionTable") Is Nothing Then
                '        dtCalculatedRMD = DirectCast(Session("MinimumDistributionTable"), DataTable)
                '        Session("MinimumDistributionTable_temp") = Session("MinimumDistributionTable")
                '    End If

                '    If Not dtCalculatedRMD Is Nothing Then
                '        dtRMDWithNonTaxableAmt = dtCalculatedRMD.Clone
                '    End If

                '    ' For RMD get only non taxable amount
                '    For Each l_DataRow As DataRow In dtCalculatedRMD.Rows
                '        drRMD = dtRMDWithNonTaxableAmt.NewRow
                '        drRMD("AcctType") = l_DataRow("AcctType")
                '        drRMD("Taxable") = 0.0
                '        drRMD("NonTaxable") = l_DataRow("NonTaxable")
                '        drRMD("TaxRate") = "0.00"
                '        drRMD("Tax") = "0.00"
                '        drRMD("Payee") = l_DataRow("Payee")
                '        drRMD("FundedDate") = l_DataRow("FundedDate")
                '        drRMD("RequestType") = l_DataRow("RequestType")
                '        drRMD("RefRequestsID") = l_DataRow("RefRequestsID")
                '        dtRMDWithNonTaxableAmt.Rows.Add(drRMD)
                '    Next
                '    ' Store RMD with non taxable amount only
                '    Session("MinimumDistributionTable") = dtRMDWithNonTaxableAmt
                '    ' Set RMD taxable amount, tax and net amount 
                '    TextboxMinDistAmount.Text = "0.00"
                '    TextboxMinDistTaxable.Text = "0.00"
                '    TextboxMinDistNet.Text = TextboxMinDistNonTaxable.Text
                '    ViewState("IsRolloverTaxableWasSelected") = "True"
                'End If
                '' END | SR | 2016.01.22 | YRS-AT-2664 - Add RMD Taxable Amount in Rollover.
                ' END | SR | 2017.12.12 | YRS-AT-3742 - Since now, RMD amount will be adjusted based on available non taxable amount, RMD taxable amount should not be rollover at any scenario. Hence, below line of codes are commented code.

                Dim l_DRow As DataRow
                For Each l_DRow In l_Payee2DataTable.Rows
                    l_Payee3DataTable.ImportRow(l_DRow)
                Next
                For Each l_DRow In l_Payee3DataTable.Rows
                    l_DRow("Taxable") = "0.00"
                    l_DRow("NonTaxable") = "0.00"
                Next
                TextboxPayee3.ReadOnly = False
                'end

            End If

            Session("Payee1DataTable_C19") = l_Payee1DataTable
            Session("Payee2DataTable_C19") = l_Payee2DataTable
            Session("Payee3DataTable_C19") = l_Payee3DataTable
            Session("Payee1TempDataTable_C19") = l_Payee1TempDataTable

            Me.LoadPayeesDataGrid()
            Me.LoadPayee1ValuesIntoControls()
            Me.LoadPayee2ValuesIntoControls()
            Me.LoadPayee3ValuesIntoControls()
            Me.DoFinalCalculation()
            Me.EnableDisableSaveButton()

        Catch ex As Exception
            HelperFunctions.LogException("DoTaxableOnly()", ex) ' SR | 2017.11.28 | YRS-AT-3742 | logging exceptions
            Throw
        End Try

    End Function

    Private Function IsExistInRequestedAccounts(ByVal parameterAccountType As String, Optional ByVal parameterDataTable As DataTable = Nothing) As Boolean

        'Commented code deleted by SG: 2012.03.16
        Dim l_RequestedDataTable As DataTable

        Dim l_FoundRows() As DataRow
        Dim l_QueryString As String

        Try
            'Commented code deleted by SG: 2012.03.16
            If IsNothing(parameterDataTable) Then
                l_RequestedDataTable = DirectCast(Session("RequestedAccounts_C19"), DataTable)
            Else
                l_RequestedDataTable = parameterDataTable
            End If

            'Hafiz 03Feb06 Cache-Session

            If Not l_RequestedDataTable Is Nothing Then
                l_QueryString = "AccountType = '" & parameterAccountType.Trim.ToUpper & "'"

                l_FoundRows = l_RequestedDataTable.Select(l_QueryString)

                If Not l_FoundRows Is Nothing Then
                    If l_FoundRows.Length > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If


            Else
                Return False
            End If



        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Sub DatagridDeductions_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridDeductions.ItemDataBound
        Try
            'Commented code deleted by SG: 2012.03.16


        Catch ex As Exception

            Throw

        End Try
    End Sub

    Public Sub CheckBoxDeduction_Checked(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxDeduction.CheckedChanged

        Dim l_CheckBox As CheckBox
        Dim l_Decimal_Amount As Decimal = 0.0

        Try

            For Each l_DataGridItem As DataGridItem In Me.DatagridDeductions.Items

                l_CheckBox = l_DataGridItem.FindControl("CheckBoxDeduction")

                If Not l_CheckBox Is Nothing Then

                    If l_CheckBox.Checked = True Then
                        'Commented code deleted by SG: 2012.03.16
                        l_Decimal_Amount += CType(l_DataGridItem.Cells.Item(3).Text.Trim, Decimal)
                        ''End If

                    End If
                End If
            Next

            Me.DeductionsAmount = l_Decimal_Amount
            Me.TextboxDeductions.Text = Math.Round(l_Decimal_Amount, 2)

            Me.DoFinalCalculation()

        Catch CaEx As InvalidCastException
            Me.DeductionsAmount = 0.0
            Me.TextboxDeductions.Text = Me.DeductionsAmount
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Function DoFinalCalculation()

        Dim l_Decimal_Payee1Taxable As Decimal
        Dim l_Decimal_Payee2Taxable As Decimal
        Dim l_Decimal_Payee3Taxable As Decimal
        Dim l_Decimal_Payee1NonTaxable As Decimal
        Dim l_Decimal_Payee2NonTaxable As Decimal
        Dim l_Decimal_Payee3NonTaxable As Decimal
        Dim l_Decimal_Payee1Net As Decimal
        Dim l_Decimal_Payee2Net As Decimal
        Dim l_Decimal_Payee3Net As Decimal
        Dim l_Decimal_Payee1Tax As Decimal

        Dim l_Decimal_HardShipAmount As Decimal
        Dim hardShipNonTaxable As Decimal 'MMR | 2017.07.30 | YRS-AT-3870 | Declared variable to hold non-taxable hardship amount
        Dim l_Decimal_HardShipTax As Decimal
        Dim l_Decimal_HardShipNet As Decimal

        Dim l_Decimal_MinDisbAmount As Decimal
        Dim l_Decimal_MinDisbTax As Decimal
        Dim l_decimal_MinDisbNet As Decimal
        Dim l_Decimal_MinDisbNonTaxable As Decimal
        'START : ML : 05/05/2020 | YRS-AT-4874 | Added controls for COVID Distribution
        Dim C19TaxableAmount As Decimal
        Dim C19Tax As Decimal
        Dim C19NetAmount As Decimal
        Dim C19NontaxableAmount As Decimal
        Dim C19TotalAmount As Decimal
        'END : ML : 05/05/2020 | YRS-AT-4874 | Added controls for COVID Distribution
        Try

            If TextboxHardShip.Text = String.Empty Then
                TextboxHardShip.Text = "0.00"
            End If
            If TextboxHardShip.Text = "." Then
                TextboxHardShip.Text = "0.00"
            End If


            If Me.TextboxNonTaxable.Text.Trim.Length > 1 Then
                l_Decimal_Payee1NonTaxable = CType(Me.TextboxNonTaxable.Text, Decimal)
            Else
                l_Decimal_Payee1NonTaxable = 0
            End If

            If Me.TextboxTaxable.Text.Trim.Length > 1 Then
                l_Decimal_Payee1Taxable = CType(Me.TextboxTaxable.Text, Decimal)
            Else
                l_Decimal_Payee1Taxable = 0.0
            End If
            'Added By Ganeswar Sahoo on 31-07-2009
            If Me.TextboxTax.Text.Trim = String.Empty Then
                l_Decimal_Payee1Tax = 0.0
            Else
                If Me.TextboxTax.Text.Trim > 0 Then
                    l_Decimal_Payee1Tax = CType(Me.TextboxTax.Text, Decimal)
                Else
                    l_Decimal_Payee1Tax = 0.0
                End If
            End If

            If Me.TextboxNet.Text.Trim = String.Empty Then
                l_Decimal_Payee1Net = 0.0
            Else
                If Me.TextboxNet.Text.Trim > 0 Then
                    l_Decimal_Payee1Net = CType(Me.TextboxNet.Text, Decimal)
                Else
                    l_Decimal_Payee1Net = 0.0
                End If
            End If
            'Added By Ganeswar Sahoo on 31-07-2009

            '' Calculation for payee 2
            If Me.TextboxNonTaxable2.Text = String.Empty Then
                l_Decimal_Payee2NonTaxable = 0.0
            Else
                If Me.TextboxNonTaxable2.Text > 0 Then
                    l_Decimal_Payee2NonTaxable = CType(Me.TextboxNonTaxable2.Text, Decimal)
                Else
                    l_Decimal_Payee2NonTaxable = 0
                End If
            End If

            If Me.TextboxTaxable2.Text = String.Empty Then
                l_Decimal_Payee2Taxable = 0.0
            Else
                If Me.TextboxTaxable2.Text > 0 Then
                    l_Decimal_Payee2Taxable = CType(Me.TextboxTaxable2.Text, Decimal)
                Else
                    l_Decimal_Payee2Taxable = 0.0
                End If
            End If

            If Me.TextboxNet2.Text = String.Empty Then
                l_Decimal_Payee2Net = 0.0
            Else
                If Me.TextboxNet2.Text > 0 Then
                    l_Decimal_Payee2Net = CType(Me.TextboxNet2.Text, Decimal)
                Else
                    l_Decimal_Payee2Net = 0.0
                End If
            End If

            If Me.TextboxNonTaxable3.Text = String.Empty Then
                l_Decimal_Payee3NonTaxable = 0.0
            Else
                If Me.TextboxNonTaxable3.Text > 0 Then
                    l_Decimal_Payee3NonTaxable = CType(Me.TextboxNonTaxable3.Text, Decimal)
                Else
                    l_Decimal_Payee3NonTaxable = 0.0
                End If
            End If

            If Me.TextboxTaxable3.Text = String.Empty Then
                l_Decimal_Payee3Taxable = 0.0
            Else
                If Me.TextboxTaxable3.Text > 1 Then
                    l_Decimal_Payee3Taxable = CType(Me.TextboxTaxable3.Text, Decimal)
                Else
                    l_Decimal_Payee3Taxable = 0.0
                End If
            End If


            If Me.TextboxNet3.Text = String.Empty Then
                l_Decimal_Payee3Net = 0.0
            Else
                If Me.TextboxNet3.Text.Trim.Length > 1 Then
                    l_Decimal_Payee3Net = CType(Me.TextboxNet3.Text, Decimal)
                Else
                    l_Decimal_Payee3Net = 0.0
                End If
            End If



            If Me.TextboxMinDistAmount.Text.Trim.Length > 1 Then
                l_Decimal_MinDisbAmount = CType(Me.TextboxMinDistAmount.Text, Decimal)
            Else
                l_Decimal_MinDisbAmount = 0.0
            End If

            If Me.TextboxMinDistNonTaxable.Text.Trim.Length > 1 Then ' SR | 2016.01.22 | YRS-AT-2664 - corretion done to check MRD Non taxable amount
                l_Decimal_MinDisbNonTaxable = CType(Me.TextboxMinDistNonTaxable.Text, Decimal)
            Else
                l_Decimal_MinDisbNonTaxable = 0.0
            End If

            If Me.TextboxMinDistTaxable.Text.Trim.Length > 1 Then
                l_Decimal_MinDisbTax = CType(Me.TextboxMinDistTaxable.Text, Decimal)
            Else
                l_Decimal_MinDisbTax = 0.0
            End If

            If Me.TextboxMinDistNet.Text.Trim.Length > 1 Then
                l_decimal_MinDisbNet = CType(Me.TextboxMinDistNet.Text, Decimal)
            Else
                l_decimal_MinDisbNet = 0.0
            End If
            'START : ML : 05/05/2020 | YRS-AT-4874 | Added controls for COVID Distribution
            C19TaxableAmount = If(Me.txtTaxableC19.Text.Trim.Length > 1, CType(Me.txtTaxableC19.Text, Decimal), 0.0)
            C19NontaxableAmount = If(Me.txtNonTaxableC19.Text.Trim.Length > 1, CType(Me.txtNonTaxableC19.Text, Decimal), 0.0)
            C19NetAmount = If(Me.txtNetC19.Text.Trim.Length > 1, CType(Me.txtNetC19.Text, Decimal), 0.0)
            C19TotalAmount = C19TaxableAmount + C19NontaxableAmount
            C19Tax = If(Me.txtTaxC19.Text.Trim.Length > 1, CType(Me.txtTaxC19.Text, Decimal), 0.0)
            'END : ML : 05/05/2020 | YRS-AT-4874 | Added controls for COVID Distribution
            'Added comment by Ganeswar For HardShip RollOver on 21-05-2009
            'If Me.RefundType = "HARD" Then
            If Me.RefundType = "HARD" And TextboxVoluntaryWithdrawalTotal.Text = "0.00" Then
                'End comment by Ganeswar For HardShip RollOver on 21-05-2009
                '' Get value for Hard Ship.. 

                If Me.TextboxHardShipAmount.Text.Trim.Length > 1 Then
                    l_Decimal_HardShipAmount = CType(Me.TextboxHardShipAmount.Text, Decimal)
                Else
                    l_Decimal_HardShipAmount = 0.0
                End If
                'START : MMR | 2017.07.30 | YRS-AT-3870 | Assigning value to local variable from control
                hardShipNonTaxable = 0.0
                If Me.textboxHardShipNonTaxableAmount.Text.Trim.Length > 1 Then
                    hardShipNonTaxable = CType(Me.textboxHardShipNonTaxableAmount.Text, Decimal)
                End If
                'END : MMR | 2017.07.30 | YRS-AT-3870 | Assigning value to local variable from control

                If Me.TextboxHardShip.Text.Trim.Length > 1 Then
                    l_Decimal_HardShipTax = CType(Me.TextboxHardShip.Text, Decimal)
                    l_Decimal_HardShipTax = Math.Round(l_Decimal_HardShipTax, 2)
                Else
                    l_Decimal_HardShipTax = 0.0
                End If

                'Priya : 26-Dec-2008 commented code and made changes as per ragesh mail as on 25-Dec-2008 to enable save button for 10 RS withdrawal.
                'If Me.TextboxHardShipNet.Text.Trim.Length > 1 Then
                If Me.TextboxHardShipNet.Text.Trim.Length > 0 Then
                    l_Decimal_HardShipNet = CType(Me.TextboxHardShipNet.Text, Decimal)
                    l_Decimal_HardShipNet = Math.Round(l_Decimal_HardShipNet, 2)
                Else
                    l_Decimal_HardShipNet = 0.0
                End If

                Me.TextboxTaxableFinal.Text = Math.Round((l_Decimal_Payee1Taxable + l_Decimal_HardShipAmount), 2)
                'START : MMR | 2017.07.30 | YRS-AT-3870 | Adding hardship non-taxable component to net non-taxable amount
                'Me.TextboxNonTaxableFinal.Text = Math.Round(l_Decimal_Payee1NonTaxable, 2)
                Me.TextboxNonTaxableFinal.Text = Math.Round(l_Decimal_Payee1NonTaxable + hardShipNonTaxable, 2)
                'END : MMR | 2017.07.30 | YRS-AT-3870 | Adding hardship non-taxable component to net non-taxable amount
                Me.TextboxTaxFinal.Text = Math.Round((l_Decimal_Payee1Tax + l_Decimal_HardShipTax), 2)
                Me.TextboxNetFinal.Text = Math.Round(((l_Decimal_Payee1Net + l_Decimal_HardShipNet) - Me.DeductionsAmount), 2)
            Else
                'START : ML : 05/05/2020 | YRS-AT-4874 | Added controls for COVID Distribution
                Me.TextboxTaxableFinal.Text = l_Decimal_Payee1Taxable + l_Decimal_Payee2Taxable + l_Decimal_Payee3Taxable + l_Decimal_MinDisbAmount + C19TaxableAmount
                Me.TextboxNonTaxableFinal.Text = l_Decimal_Payee1NonTaxable + l_Decimal_Payee2NonTaxable + l_Decimal_Payee3NonTaxable + l_Decimal_MinDisbNonTaxable + C19NontaxableAmount
                Me.TextboxTaxFinal.Text = l_Decimal_Payee1Tax + l_Decimal_MinDisbTax + C19Tax
                Me.TextboxNetFinal.Text = (l_Decimal_Payee1Net + l_Decimal_Payee2Net + l_Decimal_Payee3Net + l_decimal_MinDisbNet + C19NetAmount) - Me.DeductionsAmount
                'END : ML : 05/05/2020 | YRS-AT-4874 | Added controls for COVID Distribution
            End If
            'Ganeswar Added  for HardShip rollover on 03-06-2009

            If Me.RefundType = "HARD" And TextboxRequestAmount.Text > "0.00" Then
                'START : ML : 05/05/2020 | YRS-AT-4874 | Added controls for COVID Distribution
                Me.TextboxTaxableFinal.Text = l_Decimal_Payee1Taxable + l_Decimal_Payee2Taxable + l_Decimal_Payee3Taxable + CType(Me.TextboxHardShipAmount.Text, Decimal) + l_Decimal_MinDisbAmount + C19TaxableAmount
                'START : MMR | 2017.07.30 | YRS-AT-3870 | Adding hardship non-taxable component to net non-taxable amount
                'Me.TextboxNonTaxableFinal.Text = l_Decimal_Payee1NonTaxable + l_Decimal_Payee2NonTaxable + l_Decimal_Payee3NonTaxable + l_Decimal_MinDisbNonTaxable
                Me.TextboxNonTaxableFinal.Text = l_Decimal_Payee1NonTaxable + l_Decimal_Payee2NonTaxable + l_Decimal_Payee3NonTaxable + l_Decimal_MinDisbNonTaxable + CType(Me.textboxHardShipNonTaxableAmount.Text, Decimal) + C19NontaxableAmount
                'END : MMR | 2017.07.30 | YRS-AT-3870 | Adding hardship non-taxable component to net non-taxable amount
                Me.TextboxTaxFinal.Text = l_Decimal_Payee1Tax + l_Decimal_MinDisbTax + CType(Me.TextboxHardShip.Text, Decimal) + C19Tax
                Me.TextboxNetFinal.Text = (l_Decimal_Payee1Net + l_Decimal_Payee2Net + l_Decimal_Payee3Net + l_decimal_MinDisbNet + CType(Me.TextboxHardShipNet.Text, Decimal) + C19NetAmount) - Me.DeductionsAmount
                'END : ML : 05/05/2020 | YRS-AT-4874 | Added controls for COVID Distribution
            End If


            Me.TextboxTaxableFinal.ReadOnly = True
            Me.TextboxNonTaxableFinal.ReadOnly = True
            Me.TextboxTaxFinal.ReadOnly = True
            Me.TextboxNetFinal.ReadOnly = True

        Catch IcEx As InvalidCastException
            Throw
        Catch ex As Exception
            Throw
        End Try


    End Function

    Private Function LoadRefundConfiguration()

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        'Commented code deleted by SG: 2012.03.16
        Dim l_stringCategory As String = "Refund"
        Try

            'Commented code deleted by SG: 2012.03.16

            'BY Aparna 18/04/2007   
            'Changed the proc used to get the Refund configuration -so tht it can be universally used
            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetConfigurationCategoryWise(l_stringCategory)
            If Not l_DataTable Is Nothing Then

                For Each l_DataRow In l_DataTable.Rows

                    If (CType(l_DataRow("Key"), String).Trim = "REFUND_EXPIRE_DAYS") Then
                        Me.RefundExpiryDate = CType(l_DataRow("Value"), Integer)
                    End If

                    If (CType(l_DataRow("Key"), String).Trim = "MIN_DISTRIBUTION_AGE") Then
                        Me.MinimumDistributedAge = CType(l_DataRow("Value"), Decimal)
                    End If

                    If (CType(l_DataRow("Key"), String).Trim = "REFUND_MAX_PIA") Then
                        Me.MaximumPIAAmount = CType(l_DataRow("Value"), Decimal)
                    End If
                    'Start: AA: 11.20.2015 YRS-AT-2639 Added to use the meta configuration key value
                    If (CType(l_DataRow("Key"), String).Trim = "REFUND_FEDERALTAXRATE") Then
                        Me.FedaralTaxrate = CType(l_DataRow("Value"), Decimal)
                    End If
                    'End: AA: 11.20.2015 YRS-AT-2639 Added to use the meta configuration key value 

                    'START : ML | 2020.04.20 | YRS-AT-4874 | Set Covid taxrate value in session
                    'If (CType(l_DataRow("Key"), String).Trim = "REFUND_FEDERALTAXRATE_COVID19") Then
                    '    Me.COVIDTaxrate = CType(l_DataRow("Value"), Decimal)
                    '    Me.COVIDFixedTaxrate = CType(l_DataRow("Value"), Decimal)
                    'End If
                    'END : ML | 2020.04.20 | YRS-AT-4874 |  Set Covid taxrate value in session
                    'START : SC | 2020.04.05 | YRS-AT-4874 | Get covid amounnt limit
                    If (CType(l_DataRow("Key"), String).Trim = "COVID_REFUND_EXEMPT_AMOUNT") Then
                        Me.CovidAmountLimit = CType(l_DataRow("Value"), Decimal)
                    End If
                    'END : SC | 2020.04.05 | YRS-AT-4874 | Get covid amounnt limit

                Next
            End If

            '' This call to get the Account Break Down.. 

            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetAccountBreakDown()

            'Commented code deleted by SG: 2012.03.16
            Session("AccountBreakDown_C19") = l_DataTable
            'Hafiz 03Feb06 Cache-Session
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function IsQuestionAnswer() As Boolean

        Dim l_Boolean_QuestionAnswer As Boolean = True

        '' 
        If Me.RadioButtonReleaseSignedNo.Checked = False Then
            l_Boolean_QuestionAnswer = False
        End If

        ''
        If Me.RadioButtonNotarizedNo.Checked = False Then
            l_Boolean_QuestionAnswer = False
        End If

        ''
        If Me.RadioButtonWaiverNo.Checked = False Then
            l_Boolean_QuestionAnswer = False
        End If

        ''
        If Me.RadioButtonAddressUpdatingNo.Checked = False Then
            l_Boolean_QuestionAnswer = False
        End If

        ''
        'Commented and Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
        'If Me.RadioButtonRolloversNo.Checked = False Then
        If Me.CheckBoxRollovers.Checked = True Then
            l_Boolean_QuestionAnswer = False
        End If

        ''
        'Commented and Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
        'If Me.RadioButtonAddnlWitholdingNo.Checked = False Then
        If Me.CheckboxAddnlWitholding.Checked = True Then
            l_Boolean_QuestionAnswer = False
        End If
        'Added Ganeswar for HardShip Rollover Question Answer on 21-05-2009
        'Commented and Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
        'If RadioButtonVoluntaryRolloverNo.Checked = False Then
        If CheckboxVoluntaryRollover.Checked = True Then
            l_Boolean_QuestionAnswer = False
        End If
        'End Ganeswar for HardShip Rollover Question Answer on 21-05-2009
        Return l_Boolean_QuestionAnswer


    End Function

    Private Function SaveRefundProcess() As String

        Dim l_ErrorMessage As String = String.Empty
        Dim l_ProcessMessage As String = String.Empty
        Dim l_decimalTaxAmount As Decimal
        Dim ll_ForcetoPersonalWithdrawal As Boolean = False
        Dim l_Covid_Transactions As DataTable ' SC : 05/05/2020 | YRS-AT-4874 | Added controls for COVID Distribution
        Try
            'START : SC | 2020.05.06 | YRS-AT-4874 | Update Covid Transactions
            If HelperFunctions.isNonEmpty(Session("COVIDProrateAccountDataTable")) Then
                l_Covid_Transactions = HelperFunctions.DeepCopy(Of DataTable)(DirectCast(Session("COVIDProrateAccountDataTable"), DataTable))
            End If
            'END : SC | 2020.05.06 | YRS-AT-4874 | Update Covid Transactions
            l_ErrorMessage = Me.DoValidation


            If l_ErrorMessage <> String.Empty Then
                l_ErrorMessage = "Error: " + l_ErrorMessage
                SaveRefundProcess = l_ErrorMessage
                'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", l_ErrorMessage, MessageBoxButtons.Stop, True)
                Exit Function
            End If
            'Gemini 924 Amit 20-oct-2009
            'l_decimalTaxAmount = CDec(TextboxTax.Text)
            l_decimalTaxAmount = CDec(TextboxTaxFinal.Text)
            'Gemini 924 Amit 20-oct-2009
            '14-09-09, Ganeswar; Added to pass the value of the Radio button to allow personal side of money in processing.
            ll_ForcetoPersonalWithdrawal = Me.RadiobuttonPersonalMoniesYes.Checked
            '14-09-09, Ganeswar; Added to pass the value of the Radio button to allow personal side of money in processing.
            objRefundProcess.IRSOverride = Me.IRSOverride 'SR | 2016.09.23 | YRS-AT-3164 - Support Request:special Hardship Withdrawal - how to allow contributions to continue temporarily

            l_ProcessMessage = objRefundProcess.SaveRefundRequestProcess(Me.DatagridDeductions, Session("l_dec_TDAmount_C19"), Session("l_dec_TDRequestedAmount_C19"), CDec(l_decimalTaxAmount), CDec(ll_ForcetoPersonalWithdrawal), CDec(TextboxTotalRolloverAmount.Text))

            SaveRefundProcess = l_ProcessMessage

            If (SaveRefundProcess = "Requested refund is Processed Successfully.") Then
                UpdateCOVIDTransactions(l_Covid_Transactions, True) 'SC | 2020.05.06 | YRS-AT-4874 | Update Covid Transactions
                Me.ISMarket = 1
            End If



            'Commented code deleted by SG: 2012.03.16


        Catch ex As Exception
            'Begin Code Merge by Dilip on 07-05-2009
            Throw
            'MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", ex.Message.ToString(), MessageBoxButtons.Stop, False)
            'End Code Merge by Dilip on 07-05-2009
        End Try
    End Function
    Private Function DoValidation() As String

        Dim l_ErrorMessage As String
        Dim l_NetFinal As Decimal = 0

        l_NetFinal = CType(Me.TextboxNetFinal.Text, Decimal)

        'Ashutosh The two messages are same how it is possible 21-Jul
        If Me.TextboxNetFinal.Text.Trim.Length > 0 Then
            'If CType(Me.TextboxNetFinal.Text, Decimal) < 0.01 Then Return "The Net Amount of the Refund Can Not be Less Than $ 0.00. Please Adjust Amounts"
            If l_NetFinal < 0.01 Then
                Return "The Net Amount of the Refund Can Not be Less Than $ 0.00. Please Adjust Amounts"
            End If
        Else
            Return "The Net Amount of the Refund Can Not be Less Than $ 0.00. Please Adjust Amounts"
        End If

        'Commented code deleted by SG: 2012.03.16

        'Check for any type of rolover if its there
        'Commented and Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
        'If Me.RadioButtonRolloverYes.Checked = True And (Me.RadioButtonRolloverAll.Checked = True Or Me.RadioButtonNone.Checked = True Or Me.RadioButtonTaxableOnly.Checked = True) Then
        ' If Me.CheckBoxRollovers.Checked = True And (Me.RadioButtonRolloverAll.Checked = True Or Me.RadioButtonNone.Checked = True Or Me.RadioButtonTaxableOnly.Checked = True) Then
        If Me.CheckBoxRollovers.Checked = True And (Me.RadioButtonRolloverAll.Checked = True Or Me.RadioButtonNone.Checked = True Or Me.RadioButtonTaxableOnly.Checked = True Or Me.RadioButtonSpecificAmount.Checked) Then
            If Me.TextboxPayee2.Text.Trim.Length < 1 Then
                Return "A Rollover was Requested but there is No 2nd Payee Information. Please enter the Payee 2 Information"
            End If

            'Commented code deleted by SG: 2012.03.16

            '' Set the PayeeID & Payee Type..
            l_ErrorMessage = Me.GetPayeeID(Me.TextboxPayee2.Text, 2)

            If Not l_ErrorMessage = String.Empty Then
                Return l_ErrorMessage
            End If

        End If



        '' Check for the Third Party 
        If Me.TextboxPayee3.Text.Trim.Length > 0 Then
            'Commented code deleted by SG: 2012.03.16

            '' Set the Payee3ID & Payee Type..
            l_ErrorMessage = Me.GetPayeeID(Me.TextboxPayee3.Text, 3)

            If Not l_ErrorMessage = String.Empty Then
                Return l_ErrorMessage
            End If


        End If




        Return String.Empty

    End Function

    Private Function GetPayeeID(ByVal parameterPayeeName As String, ByVal parameterIndex As Integer) As String

        Dim l_ErrorMessage As String

        'Dim l_DataTable As DataTable
        'Dim l_DataRow As DataRow

        Try
            Dim l_string_RolloverInstitutionID As String
            'Get the InstitutionID 
            YMCARET.YmcaBusinessObject.RefundRequest.Get_RefundRolloverInstitutionID(parameterPayeeName, l_string_RolloverInstitutionID)
            If l_string_RolloverInstitutionID.Trim() = "" Then
                '  MessageBox.Show(MessageBoxPlaceHolder, "Refund Request- Rollover ", "Unable to retrive Rollover Institution Information Data", MessageBoxButtons.Stop)
                Return "Unable to retrieve Rollover Institution Information Data"
            Else

                If parameterIndex = 2 Then
                    Me.Payee2ID = l_string_RolloverInstitutionID
                Else
                    Me.Payee3ID = l_string_RolloverInstitutionID
                End If

                Return String.Empty
            End If

            'Commented code deleted by SG: 2012.03.16



        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Sub CheckForIsPersonalOnly()

        'Commented code deleted by SG: 2012.03.16
        Dim l_RefundRequestTable As DataTable
        Dim l_PersonalOnly As Boolean


        Try

            'Commented code deleted by SG: 2012.03.16



        Catch ex As Exception

        End Try

    End Sub

    Private Function GetAddressID() As String

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        'Commented code deleted by SG: 2012.03.16
        Dim l_DataRow As DataRow

        Try
            'PersonInformation
            'Commented code deleted by SG: 2012.03.16
            l_DataSet = Session("PersonInformation_C19")
            'Hafiz 03Feb06 Cache-Session

            If Not l_DataSet Is Nothing Then

                ' To Find Address ID
                l_DataTable = l_DataSet.Tables("Member Address")

                If Not l_DataTable Is Nothing Then

                    For Each l_DataRow In l_DataTable.Rows
                        If l_DataRow("AddressID").GetType.ToString <> "System.DBNull" Then
                            If CType(l_DataRow("AddressID"), String) <> "" Then
                                Return (CType(l_DataRow("AddressID"), String))
                            Else
                                Return ""
                            End If
                        Else
                            Return ""
                        End If
                    Next

                End If

                Return ""

            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function LoadAccountBreakDown()

        'Commented code deleted by SG: 2012.03.16
        Dim l_DataTable As DataTable

        Try
            'Commented code deleted by SG: 2012.03.16

            l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetAccountBreakDown()

            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager.Add("AccountBreakDown", l_DataTable)
            Session("AccountBreakDown_C19") = l_DataTable
            'Hafiz 03Feb06 Cache-Session

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub ButtonTab1OK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonTab1OK.Click
        ClearHardshipSessions() ' SR | 2019.08.02 | YRS-AT-4498 | Clear Hardship sessions.
        Me.SessionIsRefundProcessPopupAllowed = True
        Me.Response.Write("<script language='javascript'> { self.close() }</script>")
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        ClearHardshipSessions() ' SR | 2019.08.02 | YRS-AT-4498 | Clear Hardship sessions.
        Session("Title_C19") = Nothing
        Me.SessionIsRefundProcessPopupAllowed = True
        Me.Response.Write("<script language='javascript'> { self.close() }</script>")
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click

        Dim l_BooleanFlag As Boolean = False
        Dim l_BooleanValidationFlag As Boolean = False
        'Shubhrata & Priya  4-June-08
        Dim l_string_LastPayRollDateValidation As String
        Dim l_string_DailyInterestValidation As String = ""
        Dim l_string_HardshipValidation As String = ""

        'Added By: SG: 2012.06.19: BT-1043(Re-open)
        Dim drPriorMRD As DataRow()
        Dim dtPriorMRD As DataTable

        Try
            'Start 2013-09-27         Dinesh k    BT:2012:YRS 5.0-2063: Handling RMD candidates 
            If Me.RefundType = "MRD" Then
                If Not Session("PersonID") Is Nothing And Not Session("PersonInformation_C19") Is Nothing Then
                    Dim dtPersonDetails As DataTable
                    dtPersonDetails = CType(Session("PersonInformation_C19"), DataSet).Tables("Member Details") ' l_DataSet.Tables("Member Details")
                    If dtPersonDetails.Rows.Item(0)("IsAnnualMRDRequested").ToString() <> "" Then
                        If Convert.ToBoolean(dtPersonDetails.Rows.Item(0)("IsAnnualMRDRequested").ToString()) = False Then
                            MessageBox.Show(MessageBoxPlaceHolder, " YMCA - YRS", getmessage("MESSAGE_IS_ANNUAL_MRD_PAYMENT"), MessageBoxButtons.Stop, False)
                            Me.SessionIsRefundRequest = False
                            'Response.Write("<script> window.opener.document.forms(0).submit(); self.close(); </script>")
                            ButtonSave.Enabled = False
                            Exit Sub

                        End If
                    Else
                        MessageBox.Show(MessageBoxPlaceHolder, " YMCA - YRS", getmessage("MESSAGE_IS_ANNUAL_MRD_PAYMENT"), MessageBoxButtons.Stop, False)
                        Me.SessionIsRefundRequest = False
                        'Response.Write("<script> window.opener.document.forms(0).submit(); self.close(); </script>")
                        ButtonSave.Enabled = False
                        Exit Sub
                    End If
                End If
            End If
            'END 2013-09-27         Dinesh k    BT:2012:YRS 5.0-2063: Handling RMD candidates
            'START : SC | 2020.05.06 | YRS-AT-4874 | Validate Requested COVID Amount & Transactions
            Dim l_Error = ValidateRequestedCOVIDAmount(txtCovidAmountRequested.Text)
            If Not (String.IsNullOrEmpty(l_Error)) Then
                MessageBox.Show(MessageBoxPlaceHolder, " YMCA - YRS", l_Error, MessageBoxButtons.Stop, False)
                Exit Sub
            End If
            If Not (ValidateCOVIDTaxRate(txtTaxRateC19.Text)) Then
                Exit Sub
            End If
            'END : SC | 2020.05.06 | YRS-AT-4874 | Validate Requested COVID Amount
            If TextboxRequestAmount.Text <> String.Empty Then
                Me.l_dec_TDRequestedAmount = CDec(TextboxRequestAmount.Text)
            End If

            'START : ML | 2020.05.08 | YRS-AT-4874 | Validate Total Requested Amount With ( RMD + COVID + ROLLOVER + NON COVID) 
            If Not (ValidateProratedAmountWithRequestedAmount()) Then
                l_Error = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_COVID_TOTAL_MISMATCH).DisplayText
                MessageBox.Show(MessageBoxPlaceHolder, " YMCA - YRS", l_Error, MessageBoxButtons.Stop, False)
                Exit Sub
            End If
            'END : ML | 2020.05.08 | YRS-AT-4874 | Validate Total Requested Amount With ( RMD + COVID + ROLLOVER + NON COVID) 
            If TextboxHardShip.Text <> String.Empty Then
                Me.l_dec_TDAmount = CDec(TextboxHardShip.Text)
            End If
            'Modified By Parveen To show message only in case of Partial Rollover on 16 Dec 2009
            'If RadiobuttonRolloverAllInstallment.Checked = True  Then
            If RadiobuttonRolloverAllInstallment.Checked = True And RadioButtonNone.Checked = True Then
                If TextboxTotalRolloverAmount.Text = String.Empty Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Please enter the Defered Rollover Installment Amount.", MessageBoxButtons.Stop, False)
                    Exit Sub
                ElseIf CType(TextboxTotalRolloverAmount.Text, Decimal) <= 0 Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Please enter the valid Defered Rollover Installment Amount.", MessageBoxButtons.Stop, False)
                    Exit Sub
                End If
            Else
                TextboxTotalRolloverAmount.Text = 0.0
            End If

            'Commented By SG: 2012.03.21: BT-1010
            'IB:BT:800 Get participant's alll mrd record.To check any unsatisfied mrd record is pending at the time of saving 
            'If (Not Session("dtParticipantAllMrdRecords") Is Nothing) AndAlso ((CType(Session("dtParticipantAllMrdRecords"), DataTable).Select("dtmExpireDate < '" & System.DateTime.Now().AddDays(-1) & "' and Balance >0 ")).Length > 0) Then
            'SG:BT-962: YRS 5.0-1492:No RMD if person is actively employed (includes fund status RA) on 2012.03.03
            'If (Not Session("dtParticipantAllMrdRecords") Is Nothing) AndAlso ((CType(Session("dtParticipantAllMrdRecords"), DataTable).Select("dtmExpireDate < '" & System.DateTime.Now().AddDays(-1) & "' and Balance >0 ")).Length > 0) AndAlso Me.SessionStatusType.Trim() <> "AE" AndAlso Me.SessionStatusType.Trim() <> "RA" Then
            '    'Dim l_MrdYear As String = ""
            '    'For Each l_DataRow As DataRow In CType(Session("dtParticipantAllMrdRecords"), DataTable).Select("dtmExpireDate < '" & System.DateTime.Now().AddDays(-1) & "' and Balance >0 ")
            '    '    l_MrdYear += "," + l_DataRow("MrdYear").ToString()
            '    'Next
            '    'l_MrdYear = l_MrdYear.Substring(1, l_MrdYear.Length - 1)
            '    'MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "There are expired and unsatisfied MRD records for previous year(" + " " + l_MrdYear + "). Please correct the data to proceed.", MessageBoxButtons.Stop, False)
            '    MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "There are expired and unsatisfied MRD records for previous year. Please correct the data to proceed.", MessageBoxButtons.Stop, False)
            '    Exit Sub
            'End If

            'IB:Added on 29/Sep/2010 YRS 5.0-1181 :Add validation for Hardhship withdrawal - No YMCA contact

            'START | SB | YRS-AT-2169 | 20.6.5 | Hardship withdrawl is allowed for NP/PENP and RDNP participants which are terminated
            ' If Me.RefundType = "HARD" AndAlso Not IRSOverride Then 'SR | 2016.09.28 | YRS-AT-3164 | If IRSoverride flag is true then do not validate contact information 

            If Me.RefundType = "HARD" AndAlso Not IRSOverride AndAlso Not objRefundProcess.IsNonParticipatingPerson Then
                'END | SB | YRS-AT-2169 | 20.6.5 | Hardship withdrawl is allowed for NP/PENP and RDNP participants which are terminated
                'START | 2019.07.30 |  SR | YRS-AT-4498 | If TD contribution is allowed as per hardship configuration then do not validate contact information. 
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", String.Format("RefundType : {0}, IRSOverride: {1}, IsNonParticipatingPerson:{2} ", Me.RefundType, Me.IRSOverride, Me.objRefundProcess.IsNonParticipatingPerson))
                If (Not objRefundProcess.IsTDContributionsAllowed) Then
                    l_string_HardshipValidation = ""
                    If (ValidateHardshipRefundProcess() = False) Then
                        l_string_HardshipValidation = "Required YMCA contact information missing.  Hardship withdrawal cannot be processed at this time."
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", l_string_HardshipValidation, MessageBoxButtons.OK, True)
                        Exit Sub
                    End If
                End If
                'END | SR | 2019.07.30 | YRS-AT-4498 | If TD contribution is allowed as per hardship configuration then do not validate contact information. 
            End If
            'Added on 25/08/2011 BT:830-YRS 5.0-1326 : Rollover including non-taxable money.
            If CheckBoxRollovers.Checked = True And RadioButtonSpecificAmount.Checked = True Then
                If TextboxRolloverAmount.Text = String.Empty Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Please enter the Rollover requested amount.", MessageBoxButtons.Stop, False)
                    Exit Sub
                ElseIf CType(TextboxRolloverAmount.Text, Decimal) <= 0 Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Please enter the valid Rollover requested amount.", MessageBoxButtons.Stop, False)
                    Exit Sub
                End If
                Dim l_Payee2DataTable As DataTable
                l_Payee2DataTable = CType(Session("Payee2DataTable_C19"), DataTable)
                For i As Int16 = 0 To l_Payee2DataTable.Rows.Count - 1
                    Dim l_DataRow As DataRow = l_Payee2DataTable.Rows(i)
                    If (Convert.ToDecimal(l_DataRow("Taxable"))) < 0 Or (Convert.ToDecimal(l_DataRow("NonTaxable"))) < 0 Then
                        MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA-YRS", "Unable to process for requested Rollover amount $" & TextboxRolloverAmount.Text & ". Please contact IT support.", MessageBoxButtons.Stop, False)
                        HelperFunctions.LogMessage("Negative value encounter for payee2 of rollver amount is " & TextboxRolloverAmount.Text & ". Fund no. " & Me.FundID)
                        Exit Sub
                    End If
                Next
            End If

            'START: SG: 2012.03.21: BT-1010
            'If (Me.RefundType <> "SPEC" AndAlso Me.RefundType <> "DISAB" AndAlso Me.RefundType <> "MRD" AndAlso Not Session("dtParticipantAllMrdRecords") Is Nothing) AndAlso ((CType(Session("dtParticipantAllMrdRecords"), DataTable).Select("dtmExpireDate < '" & System.DateTime.Now().AddDays(-1) & "' and Balance > 0 ")).Length > 0) AndAlso Me.SessionStatusType.Trim() <> "AE" AndAlso Me.SessionStatusType.Trim() <> "RA" Then

            'START: SG: 2012.06.19: BT-1043(Re-open)
            If Not Session("dtParticipantAllMrdRecords_C19") Is Nothing Then
                dtPriorMRD = Session("dtParticipantAllMrdRecords_C19")
            End If

            If Not dtPriorMRD Is Nothing AndAlso dtPriorMRD.Rows.Count > 0 AndAlso Me.RefundType <> "SPEC" AndAlso Me.RefundType <> "DISAB" AndAlso Me.RefundType <> "MRD" Then
                If Me.PlanTypeChosen = "BOTH" Then
                    drPriorMRD = dtPriorMRD.Select("dtmExpireDate < '" & System.DateTime.Now().AddDays(-1) & "' AND Balance > 0 ")
                ElseIf Me.PlanTypeChosen = "SAVINGS" Then
                    drPriorMRD = dtPriorMRD.Select("PlanType = 'SAVINGS' AND dtmExpireDate < '" & System.DateTime.Now().AddDays(-1) & "' AND Balance > 0 ")
                ElseIf Me.PlanTypeChosen = "RETIREMENT" Then
                    drPriorMRD = dtPriorMRD.Select("PlanType = 'RETIREMENT' AND dtmExpireDate < '" & System.DateTime.Now().AddDays(-1) & "' AND Balance > 0 ")
                End If
            End If

            'START: PPP | 11/18/2016 | YRS-AT-3146 | For Partial refund, Validating fees against balanaces left after withdrawal, whether they are sufficient enough to deduct fee as per new fee charging hierarchy
            If (RefundType.ToUpper() = "PART") Then
                If Not (YMCARET.YmcaBusinessObject.RefundRequest.ValidateWithdrawalProcessingFee(SessionRefundRequestID)) Then
                    Dim validationMessage As String = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_INSUFFICIENT_BALANCE_FOR_FEE).DisplayText
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", validationMessage, MessageBoxButtons.Stop, False)
                    Exit Sub
                End If
            End If
            'END: PPP | 11/18/2016 | YRS-AT-3146 |For Partial refund, Validating fees against balanaces left after withdrawal, whether they are sufficient enough to deduct fee as per new fee charging hierarchy

            'If (Not drPriorMRD Is Nothing AndAlso drPriorMRD.Length > 0 AndAlso Me.SessionStatusType.Trim() <> "AE" AndAlso Me.SessionStatusType.Trim() <> "RA") Then
            If (Not drPriorMRD Is Nothing AndAlso drPriorMRD.Length > 0 AndAlso objRefundProcess.IsTerminatedEmployment()) Then
                'END: SG: 2012.06.19: BT-1043(Re-open)
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Unfulfilled RMD amounts for prior periods are inlcuded.  Do you wish to continue?", MessageBoxButtons.YesNo, False)
                Session("PriorYearRMDRecords_C19") = "Yes"
                Exit Sub
            Else
                DoBtnSaveProcessing()
            End If

            'l_string_DailyInterestValidation = ValidateDailyInterest()
            'If l_string_DailyInterestValidation <> "" Then
            '    MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", l_string_DailyInterestValidation, MessageBoxButtons.YesNo, True)
            '    Session("DailyInterestValidation") = True
            '    Session("PayrollValidation") = Nothing
            '    Exit Sub
            'ElseIf l_string_DailyInterestValidation = "" Then
            '    If Me.RefundType = "REG" Or Me.RefundType = "PERS" Then
            '        l_string_LastPayRollDateValidation = ValidateLastPayrollDate()
            '        If l_string_LastPayRollDateValidation <> "" Then
            '            'MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", l_string_LastPayRollDateValidation, MessageBoxButtons.ok, False)
            '            MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", l_string_LastPayRollDateValidation, MessageBoxButtons.YesNo, True)
            '            Session("PayrollValidation") = "Yes"
            '            Exit Sub
            '        ElseIf l_string_LastPayRollDateValidation = "" Then
            '            DoSaveProcessing()
            '        End If
            '    Else
            '        DoSaveProcessing()
            '    End If

            'End If
            'END: SG: 2012.03.21: BT-1010

            'Commented code deleted by SG: 2012.03.16

        Catch ex As Exception
            'Commented code deleted by SG: 2012.03.16

            'Added by Mohammed Hafiz on 21-Apr-2008
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" & Server.UrlEncode(ex.Message), False)
        Finally 'Added By: SG: 2012.06.19: BT-1043(Re-open)
            drPriorMRD = Nothing
            dtPriorMRD = Nothing
        End Try
    End Sub
    'START: SG: 2012.03.21: BT-1010
    'Create following function to continue processing after Validating prior years unfulfilled RMD records included in withdrawal.
    Private Sub DoBtnSaveProcessing()
        Dim l_string_LastPayRollDateValidation As String
        Dim l_string_DailyInterestValidation As String = ""

        Try
            l_string_DailyInterestValidation = ValidateDailyInterest()
            If l_string_DailyInterestValidation <> "" Then
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", l_string_DailyInterestValidation, MessageBoxButtons.YesNo, True)
                Session("DailyInterestValidation_C19") = True
                Session("PayrollValidation_C19") = Nothing
                Exit Sub
            ElseIf l_string_DailyInterestValidation = "" Then
                If Me.RefundType = "REG" Or Me.RefundType = "PERS" Then
                    l_string_LastPayRollDateValidation = ValidateLastPayrollDate()
                    If l_string_LastPayRollDateValidation <> "" Then
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", l_string_LastPayRollDateValidation, MessageBoxButtons.YesNo, True)
                        Session("PayrollValidation_C19") = "Yes"
                        Exit Sub
                    ElseIf l_string_LastPayRollDateValidation = "" Then
                        DoSaveProcessing()
                    End If
                Else
                    DoSaveProcessing()
                End If
            End If
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" & Server.UrlEncode(ex.Message), False)
        End Try
    End Sub
    'END: SG: 2012.03.21: BT-1010

    '''Shubhrata & priya 4-June-08
    ''' Create following function to save Processing after validating daily  interest and last payroll date.
    Private Sub DoSaveProcessing()

        Dim l_BooleanFlag As Boolean = False
        Dim l_BooleanValidationFlag As Boolean = False
        Dim l_stringMessage As String = String.Empty

        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "DoSaveProcessing() START")    ' SR | 2019.07.24 | YRS-AT- 4498 - Log data for auditing.
            '' This satement for Refresh the DataGrid in Parent form. 

            l_BooleanValidationFlag = ValidateTotal()
            'Added By Parveen To Set RolloverOptions and Rollover Amount on 15 Dec 2009
            Me.RolloverOptions = String.Empty
            Me.FirstRolloverAmt = 0
            Me.TotalRolloverAmt = 0
            If CheckBoxRollovers.Checked And Me.ISMarket = -1 Then
                SetRolloverOptionAndAmount()
            End If
            'Added By Parveen To Set RolloverOptions and Rollover Amount on 15 Dec 2009

            If l_BooleanValidationFlag = True Then
                l_stringMessage = Me.SaveRefundProcess()
                If Left(l_stringMessage.Trim(), 5) <> "Error" Then
                    l_BooleanFlag = True
                Else
                    l_BooleanFlag = False
                End If

            Else
                l_stringMessage = "Amounts are Invalid."

            End If

            If l_BooleanFlag = False Then
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", l_stringMessage, MessageBoxButtons.Stop, False)
            Else
                Session("sessionProcessMessage_C19") = l_stringMessage
            End If

            ' Commented by Aparna -30/08/2007 -Should only check if the refund type is Hardship or not.
            ' If Not Session("NeedsNewRequests") Is Nothing Then
            'If Session("NeedsNewRequests") <> True Then
            'START | SB | YRS-AT-2169 | 20.6.5 | Hardship withdrawl is allowed for NP/PENP and RDNP participants which are terminated
            'If Me.RefundType = "HARD" AndAlso IRSOverride <> True Then 'SR | 2016.09.23 | YRS-AT-3164 - Support Request:special Hardship Withdrawal - how to allow contributions to continue temporarily 
            'START | SR | 2019.07.24 | YRS-AT- 4498 - Log refund configuration.
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", String.Format("RefundType : {0}, IRSOverride: {1}, IsNonParticipatingPerson:{2}, IsTDContributionsAllowed: {3}", Me.RefundType, Me.IRSOverride, Me.objRefundProcess.IsNonParticipatingPerson, objRefundProcess.IsTDContributionsAllowed))
            'END | SR | 2019.07.24 | YRS-AT- 4498 - Log refund configuration.
            If Me.RefundType = "HARD" AndAlso IRSOverride <> True AndAlso Not objRefundProcess.IsNonParticipatingPerson Then
                'END | SB | YRS-AT-2169 | 20.6.5 | Hardship withdrawl is allowed for NP/PENP and RDNP participants which are terminated    
                'START | SR | 2019.07.24 | YRS-AT- 4498 - If TD contribution is allowed from configuration table then do not send mail.
                If (Not objRefundProcess.IsTDContributionsAllowed) Then
                    Session("MCARefundLetterHardship_Persid_C19") = Me.PersonID
                    '   Session("HardShipReport") = True
                    ' Aparna(-YREN - 3027)
                    '  If CType(Session("HardShipReport"), Boolean) = True Then
                    If l_BooleanFlag Then
                        Dim l_string_Message As String
                        'Session("CopyHardShipReport") = True
                        'To avoid opening of other reports through the RefundRequestForm
                        Session("R_ReportToLoad_6_C19") = True
                        Session("R_ReportToLoad_3_C19") = Nothing
                        Session("R_ReportToLoad_1_C19") = Nothing
                        Session("R_ReportToLoad_2_C19") = Nothing
                        Session("R_ReportToLoad_4_C19") = Nothing
                        Session("R_ReportToLoad_5_C19") = Nothing
                        Session("R_ReportToLoad_C19") = Nothing
                        Session("strReportName_C19") = "birefltr"
                        'by aparna -
                        '    l_string_Message = IDM.HardShipReports("birefltr.rpt", "BIREFLTR")
                        'Calling IDMForall for the generation of report 
                        SetPropertiesForIDM()
                        'IB:on 06/Oct/2010 YRS 5.0-1181  IDM.ExportToPDF() method call in Member employment loop.
                        'l_string_Message = IDM.ExportToPDF()
                        Session("FTFileList_C19") = IDM.SetdtFileList

                        'Session("HardshipMessage") = l_string_Message

                    End If
                End If
                'END | SR | 2019.07.24 | YRS-AT- 4498 - If TD contribution is allowed from configuration table then do not send mail.
            End If
            'End If
            ' End If

            If l_BooleanFlag = True Then
                '' This satement for Refresh the DataGrid in Parent form. 
                Me.SessionIsRefundRequest = True
                Response.Write("<script> window.opener.document.forms(0).submit(); self.close(); </script>")
            End If
            'Set the property value to nothing to initialize the session value after save-Amit 08-oct-2009
            Me.IsHardShip = Nothing

            'Set the property value to nothing to initialize the session value after save-Amit 08-oct-2009
            'Anudeep:21.02.2013  :YRS 5.0-1654:Email to finance dept when withdrawal to foreign address
            CheckCountryName()
            Me.SendMailForPuertoRico() 'PK |09-10-2019 |YRS-AT-2670 | calling the sendmail method here.
        Catch ex As Exception
            'Commented by Mohammed Hafiz on 21-Apr-2008
            'Response.Redirect("ErrorPageForm.aspx?Message=" & Server.UrlDecode(ex.Message), False)

            'Added by Mohammed Hafiz on 21-Apr-2008
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" & Server.UrlEncode(ex.Message), False)
            ' START | SR | 2019.07.24 | YRS-AT-4498 - Log data for auditing.
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "DoSaveProcessing() END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
            ' END | SR | 2019.07.24 | YRS-AT-4498 - Log data for auditing.
        End Try
    End Sub

    Private Sub RadioButtonAddressUpdatingYes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonAddressUpdatingYes.CheckedChanged

        If Me.RadioButtonAddressUpdatingYes.Checked = True Then

            'Commented code deleted by SG: 2012.03.16
            'Start:AA:24.09.2013 : BT-1501: Commented For Displaying address with address control not with textboxes
            'Me.TextboxAddress1.ReadOnly = True
            'Me.TextboxAddress2.ReadOnly = True
            'Me.TextboxAddress3.ReadOnly = True

            'Me.TextboxCity1.ReadOnly = True
            'Me.TextBoxState.ReadOnly = True
            'Me.TextBoxCountry.ReadOnly = True
            'Me.TextBoxZip.ReadOnly = True
            'End:AA:24.09.2013 : BT-1501: Commented For Displaying address with address control not with textboxes
            Me.AddressWebUserControl1.EnableControls = False

        Else
            'Commented code deleted by SG: 2012.03.16
        End If

        Me.EnableDisableSaveButton()
    End Sub

    Private Sub RadioButtonAddressUpdatingNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonAddressUpdatingNo.CheckedChanged
        If Me.RadioButtonAddressUpdatingNo.Checked = True Then

            'Commented code deleted by SG: 2012.03.16
            'Start:AA:24.09.2013 : BT-1501: Commented For Displaying address with address control not with textboxes
            'Me.TextboxAddress1.ReadOnly = True
            'Me.TextboxAddress2.ReadOnly = True
            'Me.TextboxAddress3.ReadOnly = True

            'Me.TextboxCity1.ReadOnly = True
            'Me.TextBoxCountry.ReadOnly = True
            'Me.TextBoxState.ReadOnly = True
            'Me.TextBoxZip.ReadOnly = True
            'End:AA:24.09.2013 : BT-1501: Commented For Displaying address with address control not with textboxes
            Me.AddressWebUserControl1.EnableControls = False

        End If

        Me.EnableDisableSaveButton()
    End Sub

    'Commented code deleted by SG: 2012.03.16

    'Added by dilip for 31-10-2009 for MKT 
    Private Sub EnableDisablePartialRolloverControl(ByVal flagVal As Boolean)
        'TextBoxTotalWithdrawalAmount.Text = Me.NumTotalWithdrawalAmount commented by dilip on 24-11-2009
        'TextboxDeferedInstallmentAmount.Text = Me.PartialRollOverMinLimit  commented by dilip on 24-11-2009
        If CheckBoxRollovers.Checked = True Then
            'Added by Gunanithi - YRS-AT-2725 - 14-Jan-2016, checking if the Market based value & accordingly enabling/disabling the RolloverOnlyFirstInstallment,RolloverAllInstallment. ----start
            If Me.ISMarket = 1 Then
                RadiobuttonRolloverOnlyFirstInstallment.Visible = True
                RadiobuttonRolloverAllInstallment.Visible = True
            Else
                RadiobuttonRolloverOnlyFirstInstallment.Visible = False
                RadiobuttonRolloverAllInstallment.Visible = False
            End If
            'RadiobuttonRolloverOnlyFirstInstallment.Visible = True
            'RadiobuttonRolloverAllInstallment.Visible = True
            'Gunanithi, YRS-AT-2725- 14-Jan-2016 ---------end
        Else
            RadiobuttonRolloverOnlyFirstInstallment.Visible = flagVal
            RadiobuttonRolloverAllInstallment.Visible = flagVal
        End If
        'Added by Parveen for MKT withdrawal on 16-Dec-2009
        If RadioButtonNone.Checked = True And RadiobuttonRolloverAllInstallment.Checked = True Then
            flagVal = True
        Else
            flagVal = False
        End If
        'Added by Parveen for MKT withdrawal on 16-Dec-2009
        If CheckBoxRollovers.Checked = False Then
            flagVal = False
        End If
        'LabelTotalWithdrawalAmount.Visible = flagVal commented by dilip on 24-11-2009
        'TextBoxTotalWithdrawalAmount.Visible = flagVal commented by dilip on 24-11-2009
        LabelDeferedInstallmentAmount.Visible = flagVal
        TextboxDeferedInstallmentAmount.Visible = flagVal
        TextboxRemainingMoneyinMKT.Visible = flagVal
        LabelRemainingMoneyinMKT.Visible = flagVal
        LabelTotalRolloverAmount.Visible = flagVal
        TextboxTotalRolloverAmount.Visible = flagVal
        TextboxTotalRolloverAmount.ReadOnly = Not flagVal
        'Commented code deleted by SG: 2012.03.16

    End Sub
    'Commented code deleted by SG: 2012.03.16

    Private Sub EnableDisableSaveButton()

        Dim l_Decimal_Net As Decimal
        Dim l_Bool_MaritalStatus As Boolean
        Dim l_DataTable_PIAMinToRetire As DataTable
        Dim l_boolean_MinPIAToRetire As Boolean = False
        Try


            If Not Session("MaritalStatusCode_C19") Is Nothing Then
                If Session("MaritalStatusCode_C19") = "M" Then
                    If RadioButtonWaiverYes.Checked = True Then
                        l_Bool_MaritalStatus = True
                    Else
                        l_Bool_MaritalStatus = False
                    End If
                Else
                    If RadioButtonWaiverYes.Checked = True Or RadioButtonWaiverNo.Checked = True Then
                        l_Bool_MaritalStatus = True
                    Else
                        l_Bool_MaritalStatus = False
                    End If
                End If
            End If

            If Me.RadioButtonReleaseSignedYes.Checked And _
                Me.RadioButtonNotarizedYes.Checked And _
               l_Bool_MaritalStatus Then

                'Commented and Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
                'If Me.RadioButtonRolloverYes.Checked = True Then
                If Me.CheckBoxRollovers.Checked = True Then

                    If Me.RadioButtonNone.Checked = True Then

                        If Me.TextboxNet.Text.Trim.Length < 1 Then
                            Me.ButtonSave.Visible = False
                        Else

                            l_Decimal_Net = CType(Me.TextboxNet.Text, Decimal)

                            If l_Decimal_Net > 0.0 Then
                                Me.ButtonSave.Visible = True
                            Else
                                Me.ButtonSave.Visible = False
                            End If


                        End If
                    Else

                        If Me.TextboxNet2.Text.Trim.Length < 1 Then
                            Me.ButtonSave.Visible = False
                        Else

                            l_Decimal_Net = CType(Me.TextboxNet2.Text, Decimal)

                            If l_Decimal_Net > 0.0 Then
                                Me.ButtonSave.Visible = True
                            Else
                                Me.ButtonSave.Visible = False
                            End If
                        End If

                    End If

                Else
                    '' Check for the Payee 1 Net Amount. 
                    If Me.TextboxNet.Text.Trim.Length < 1 Then
                        Me.ButtonSave.Visible = False
                    Else

                        l_Decimal_Net = CType(Me.TextboxNet.Text, Decimal)

                        If l_Decimal_Net > 0.0 Then
                            Me.ButtonSave.Visible = True
                        Else
                            Me.ButtonSave.Visible = False
                        End If
                    End If
                End If
                If Me.RefundType = "HARD" Then
                    If Me.TextboxNetFinal.Text.Trim.Length < 1 Then
                        Me.ButtonSave.Visible = False
                    Else

                        l_Decimal_Net = CType(Me.TextboxNetFinal.Text, Decimal)
                        'Rahul
                        l_Decimal_Net = Math.Round(l_Decimal_Net, 2)
                        'rahul
                        If l_Decimal_Net > 0.0 Then
                            Me.ButtonSave.Visible = True
                        Else
                            Me.ButtonSave.Visible = False
                        End If
                    End If


                End If

            Else
                '' Mantatory filds are not Checked so hide the save button.
                Me.ButtonSave.Visible = False
            End If


            '' this is for Hiding the Wiwer Radio button.. 

            If Me.TotalRefundAmount < 5000.0 Then
                Me.RadioButtonWaiverYes.Checked = True

                Me.RadioButtonWaiverNo.Enabled = False
                Me.RadioButtonWaiverYes.Enabled = False
            Else
                Me.RadioButtonWaiverNo.Enabled = True
                Me.RadioButtonWaiverYes.Enabled = True
            End If

            'Added By Ganeswar on 14-09-2009 for display the Personal money radio button in the Processing screen.
            If Me.RefundType.Trim.ToUpper() = "REG" Or Me.RefundType.Trim.ToUpper() = "PERS" Then
                'Added By Ganeswar on 14-09-2009 for display the Personal money radio button in the Processing screen.
                'Also hardcode value of 5000 will be replaced from COnfiguration Table
                'If Me.RefundType.Trim.ToUpper() = "REG" Then
                If Me.IsVested = True Then
                    l_DataTable_PIAMinToRetire = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("PIA_MINIMUM_TO_RETIRE")
                    If Not l_DataTable_PIAMinToRetire Is Nothing Then
                        For Each l_DataRow As DataRow In l_DataTable_PIAMinToRetire.Rows
                            If (CType(l_DataRow("Key"), String).Trim = "PIA_MINIMUM_TO_RETIRE") Then
                                l_boolean_MinPIAToRetire = True
                                Me.MinimumPIAToRetire = CType(l_DataRow("Value"), Decimal)
                            End If
                            Exit For
                        Next
                    End If

                    'Shubhrata jan 9th 2006 - if values n0t found in metatable set default values - YREN -3022
                    If l_boolean_MinPIAToRetire = False Then
                        Me.MinimumPIAToRetire = 5000
                    Else
                        Me.MinimumPIAToRetire = Me.MinimumPIAToRetire
                    End If
                    If Me.PersonalAmount > 0 Then
                        'Commneted By Ganeswar on 14-09-2009 for display the Personal money radio button in the Processing screen for BA Account.
                        'If Me.CurrentPIA >= Me.MinimumPIAToRetire Then
                        If Me.CurrentPIA >= Me.MinimumPIAToRetire Or Me.CurrentBA >= Me.MinimumPIAToRetire Then
                            'Added By Ganeswar on 14-09-2009 for display the Personal money radio button in the Processing screen for BA Account.
                            Me.RadiobuttonPersonalMoniesYes.Visible = True
                            Me.RadiobuttonPersonalMoniesYes.Enabled = True
                            Me.RadiobuttonPersonalMoniesNo.Enabled = True
                            Me.RadiobuttonPersonalMoniesNo.Visible = True
                            Me.LabelPersonalMonies.Visible = True
                        Else
                            Me.RadiobuttonPersonalMoniesYes.Visible = False
                            Me.RadiobuttonPersonalMoniesYes.Checked = False
                            Me.RadiobuttonPersonalMoniesNo.Checked = True
                            Me.RadiobuttonPersonalMoniesNo.Visible = False
                            Me.LabelPersonalMonies.Visible = False
                        End If
                    End If
                End If
            Else
                Me.RadiobuttonPersonalMoniesYes.Visible = False
                Me.RadiobuttonPersonalMoniesYes.Checked = False
                Me.RadiobuttonPersonalMoniesNo.Checked = True
                Me.RadiobuttonPersonalMoniesNo.Visible = False
                Me.LabelPersonalMonies.Visible = False
            End If
            'Added By Ganeswar on 14-09-2009 for display the Personal money radio button in the Processing screen.

            'Commented code deleted by SG: 2012.03.16

            ButtonSave.Visible = True
            ' End If

            'Commented and Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
            'If RadioButtonAddnlWitholdingYes.Checked = True Then
            If CheckboxAddnlWitholding.Checked = True Then
                TextboxTax.ReadOnly = False
            End If
            If RefundType = "HARD" And TextboxRequestAmount.Text = "0.00" Then
                ButtonSave.Visible = False
            End If
            If RefundType = "HARD" And TextboxRequestAmount.Text = "0" Then
                ButtonSave.Visible = False
            End If

            DisableRadioButtons()
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    'Added by Parveen on 03-Nov-2009 to Replace the Yes/No RadioButtons with the Checkbox     
    Private Sub CheckBoxRollovers_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxRollovers.CheckedChanged
        Try
            If Me.CheckBoxRollovers.Checked Then
                'Commented code deleted by SG: 2012.03.16

                If Me.RadioButtonNone.Checked = True Then
                    'Added by dilip for 31-10-2009 for MKT 
                    'Start: Bala: YRS-AT-2725: Checking is market accordingly enabling and disabling the controls
                    'If Me.ISMarket = -1 Then
                    If Me.ISMarket = 1 Then
                        'End: Bala: YRS-AT-2725: Checking is market accordingly enabling and disabling the controls
                        Me.RadiobuttonRolloverOnlyFirstInstallment.Checked = True
                        'Modified by amit to stop displaying the textboxes for the rollover of first installment 04-nov-2009
                        Me.RadiobuttonRolloverAllInstallment.Checked = False
                        EnableDisablePartialRolloverControl(False)
                        'Modified by amit to stop displaying the textboxes for the rollover of first installment 04-nov-2009
                    End If
                    'Added by dilip for 31-10-2009 for MKT 
                    Me.DoNone()
                ElseIf Me.RadioButtonRolloverAll.Checked = True Then
                    EnableDisablePartialRolloverControl(False)
                    Me.DoRolloverAll()
                ElseIf Me.RadioButtonTaxableOnly.Checked = True Then
                    EnableDisablePartialRolloverControl(False)
                    Me.DoTaxableOnly()
                Else
                    Me.DoNone()
                End If


                Me.RadioButtonNone.Visible = True
                Me.RadioButtonRolloverAll.Visible = True
                'Me.RadioButtonTaxableOnly.Visible = True
                'Aded on 23/08/2011 BT 830: YRS 5.0-1326 : Rollover including non-taxable money.
                Me.RadioButtonSpecificAmount.Visible = True
                Me.TextboxRolloverAmount.Visible = True
                Me.RadioButtonSpecificAmount.Checked = False
                Me.TextboxRolloverAmount.Text = String.Empty
                Me.TextboxRolloverAmount.Enabled = False
                '2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only
                Me.RadioButtonTaxableOnly.Visible = True

                '/s Vipul 30Nov05 To fix UI bug for Rollin Entry
                Me.TextboxPayee2.rReadOnly = False 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.

                'Ganeswar Added on 25-05-2009 for HardShip Roll Over
                If Me.RefundType = "HARD" And TextboxTDAvailableAmount.Text > "0.00" Then
                    TextboxHardShipTaxRate.Visible = True
                    Label3.Visible = True
                    TextboxHardShipAmount.Visible = True
                    textboxHardShipNonTaxableAmount.Visible = True 'MMR | 2017.07.30 | YRS-AT-3870 | Unhiding control
                    TextboxHardShip.Visible = True
                    TextboxHardShipNet.Visible = True
                    TextboxVoluntaryWithdrawalTotal.Visible = True
                    Label5.Visible = True
                    'New Modification for Market Based Withdrawal Amit Nigam 05/Nov/2009        
                    trHardshipVoluntary.Visible = True
                    'New Modification for Market Based Withdrawal Amit Nigam 05/Nov/2009    
                End If
                'Ganeswar Added on 25-05-2009 for HardShip Roll Over

                'Commented code deleted by SG: 2012.03.16

                Me.EnableDisableSaveButton()
            Else
                'If Me.RadioButtonRolloversNo.Checked = True Then
                EnableDisablePartialRolloverControl(False)
                Me.RadioButtonNone.Visible = False
                Me.RadioButtonRolloverAll.Visible = False
                Me.RadioButtonTaxableOnly.Visible = False
                'Aded on 23/08/2011 BT 830: YRS 5.0-1326 : Rollover including non-taxable money.
                Me.RadioButtonSpecificAmount.Visible = False
                Me.TextboxRolloverAmount.Visible = False
                Me.RadioButtonSpecificAmount.Checked = False
                Me.TextboxRolloverAmount.Text = String.Empty
                Me.TextboxRolloverAmount.Enabled = False
                '2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only
                Me.RadioButtonTaxableOnly.Visible = False

                'Added By Ganeswar for Hardship rollover
                Me.TextboxPayee2.Text = String.Empty
                Me.TextboxPayee3.Text = String.Empty
                'Added By Ganeswar for Hardship rollover
                'Commented/added by Swopna in response to bug id 355 on 14 Jan,2008
                '**************
                Me.DoNone()

                objRefundProcess.CreatePayees()
                Me.LoadPayeesDataGrid()
                Me.LoadPayee1ValuesIntoControls()
                Me.LoadPayee2ValuesIntoControls()
                Me.LoadPayee3ValuesIntoControls()
                '--------
                Me.RadioButtonRolloverAll.Checked = False
                Me.RadioButtonTaxableOnly.Checked = False
                '---------
                '**************
                Me.RadioButtonNone.Checked = True
                '/s Vipul 30Nov05 To fix UI bug for Rollin Entry
                Me.TextboxPayee2.rReadOnly = True 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
                Me.TextboxPayee3.ReadOnly = True

                '/e Vipul 30Nov05 To fix UI bug for Rollin Entry

                'End If
                'New Modification for Market Based Withdrawal Amit Nigam 05/Nov/2009        
                trHardshipVoluntary.Visible = False
                'New Modification for Market Based Withdrawal Amit Nigam 05/Nov/2009    
                Me.EnableDisableSaveButton()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub CheckboxAddnlWitholding_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxAddnlWitholding.CheckedChanged
        Try
            If Me.CheckboxAddnlWitholding.Checked Then
                'If Me.RadioButtonAddnlWitholdingYes.Checked = True Then
                ''Added by Dilip for MKT 
                'Commented and Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
                'If Me.RadioButtonRolloverYes.Checked = False Or Me.RadioButtonNone.Checked = False Then
                If Me.CheckBoxRollovers.Checked = False Or Me.RadioButtonNone.Checked = False Then
                    EnableDisablePartialRolloverControl(False)
                End If
                ''Added by Dilip for MKT 
                Me.TextboxTaxRate.ReadOnly = False
                Me.TextboxMinDistTaxRate.ReadOnly = False
                Me.txtTaxRateC19.ReadOnly = False ' ML : 05/05/2020 | YRS-AT-4874 | Added controls for COVID Distribution
                If Me.RefundType = "HARD" And TextboxVoluntaryWithdrawalTotal.Text = "0.00" Then
                    Me.TextboxHardShipTaxRate.ReadOnly = False
                End If
                'Commented code deleted by SG: 2012.03.16

                Me.EnableDisableSaveButton()
            Else
                'If Me.RadioButtonAddnlWitholdingNo.Checked = True Then
                ''Added by Dilip for MKT 
                'Commented and Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
                'If Me.RadioButtonRolloverYes.Checked = False Then
                If Me.CheckBoxRollovers.Checked = False Then
                    EnableDisablePartialRolloverControl(False)
                End If
                'START : ML : 05/05/2020 | YRS-AT-4874 | Added controls for COVID Distribution
                'Me.MinDistributionTaxRate = 10
                Me.MinDistributionTaxRate = Me.MRDTaxrate
                Me.TextboxMinDistTaxRate.Text = Me.MinDistributionTaxRate
                TextboxMinDistTaxRate_TextChanged(TextboxMinDistAmount, e)
                'Me.TextboxMinDistTaxRate.ReadOnly = True ''SB | 2017.12.15 | YRS-AT-3756 | RMD tax rate can be edited through out the application 

                Me.TaxRate = FedaralTaxrate
                Me.TextboxTaxRate_TextChanged(TextboxTaxRate, e)
                objRefundProcess.TaxRate = Me.TaxRate
                Me.TextboxTaxRate.Text = Me.TaxRate.ToString
                Me.TextboxTaxRate.ReadOnly = True
                'BT:884:Harshala-MRD Amount Additional Withholding issue-END.

                'End If

                Me.COVIDTaxrate = COVIDFixedTaxrate
                Me.txtTaxRateC19.Text = Me.COVIDTaxrate
                txtTaxRateC19_TextChanged(txtTaxC19, e)
                Me.txtTaxRateC19.ReadOnly = True
                'END : ML : 05/05/2020 | YRS-AT-4874 | Added controls for COVID Distribution
                Me.EnableDisableSaveButton()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'Commented code deleted by SG: 2012.03.16

    Private Sub RadioButtonNotarizedNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonNotarizedNo.CheckedChanged
        Me.EnableDisableSaveButton()
    End Sub

    Private Sub RadioButtonNotarizedYes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonNotarizedYes.CheckedChanged
        Me.EnableDisableSaveButton()
    End Sub

    Private Sub RadioButtonReleaseSignedNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonReleaseSignedNo.CheckedChanged
        Me.EnableDisableSaveButton()
    End Sub

    Private Sub RadioButtonReleaseSignedYes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonReleaseSignedYes.CheckedChanged
        Me.EnableDisableSaveButton()
    End Sub

    Private Sub RadioButtonWaiverNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonWaiverNo.CheckedChanged
        EnableDisablePartialRolloverControl(False) ''Added by Dilip for MKT 
        Me.EnableDisableSaveButton()
    End Sub

    Private Sub RadioButtonWaiverYes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonWaiverYes.CheckedChanged
        Me.EnableDisableSaveButton()
    End Sub

    Private Sub TextboxMinDistTaxRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxMinDistTaxRate.TextChanged

        Dim l_TaxRateInteger As Integer

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow


        Dim l_Decimal_Taxable As Decimal
        Dim l_Deimal_Tax As Decimal
        Dim RMDTaxEntered As Decimal  'SB | 2017.12.15 | YRS-AT-3756 | Added new variable that will check enterd RMD tax rate is valid or not
        Try


            If Not String.IsNullOrEmpty(Me.TextboxMinDistTaxRate.Text) Then
                RMDTaxEntered = Convert.ToDecimal(Me.TextboxMinDistTaxRate.Text)
                If (Not ((RMDTaxEntered = 0) Or ((RMDTaxEntered >= Me.MRDTaxrate) And (RMDTaxEntered <= 100)))) Then
                    MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_RMD_INVALID_TAXRATE_ERROR_MSG).DisplayText, MessageBoxButtons.Stop, False)
                    Me.TextboxMinDistTaxRate.Text = Me.MRDTaxrate.ToString
                End If
            End If


            Me.MinDistributionTaxRate = CType(Me.TextboxMinDistTaxRate.Text, Decimal)


            l_DataTable = Session("MinimumDistributionTable_C19")


            If Not l_DataTable Is Nothing Then

                For Each l_DataRow In l_DataTable.Rows

                    l_Decimal_Taxable = IIf(l_DataRow("Taxable").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("Taxable"), Decimal))

                    l_DataRow("Tax") = l_Decimal_Taxable * (Me.MinDistributionTaxRate / 100.0)
                    l_DataRow("TaxRate") = Me.MinDistributionTaxRate

                Next
            End If
            ' we will set Min Dis Tax Rate to be used by Refunds.vb class
            objRefundProcess.MinDistributionTaxRate = Me.MinDistributionTaxRate
            Me.TextboxMinDistTaxRate.Text = Me.MinDistributionTaxRate

            Me.LoadMinDistributionValuesIntoControls()
            Me.DoFinalCalculation()
        Catch caEx As System.InvalidCastException
            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Invalid Tax Rate entered. Please enter the Valid Tax Rate.", MessageBoxButtons.Stop, False)
            Me.MinDistributionTaxRate = Me.MRDTaxrate
            objRefundProcess.MinDistributionTaxRate = Me.MinDistributionTaxRate
            Me.TextboxMinDistTaxRate.Text = Me.MRDTaxrate
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

    'START : ML | 2020.05.04 | YRS-AT-4874 | Recalculate withdrawals amounts on change of COVID Tax Rate
    Private Sub txtTaxRateC19_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTaxRateC19.TextChanged
        Dim l_TaxRateInteger As Integer
        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_Decimal_Taxable As Decimal
        Dim l_Deimal_Tax As Decimal
        Dim l_COVIDTaxEntered As Decimal
        Try
            If Me.txtTaxRateC19.Text.Trim = String.Empty Then
                Me.txtTaxRateC19.Text = 0
            End If
            If (Not ValidateCOVIDTaxRate(Me.txtTaxRateC19.Text)) Then
                Me.COVIDTaxrate = Me.COVIDFixedTaxrate
                Me.txtTaxRateC19.Text = Me.COVIDTaxrate.ToString
            Else
                l_COVIDTaxEntered = CType(Me.txtTaxRateC19.Text, Integer)
                Me.txtTaxRateC19.Text = l_COVIDTaxEntered
            End If
            Me.COVIDTaxrate = CType(Me.txtTaxRateC19.Text, Decimal)
            l_DataTable = Session("COVIDProrateAccountDataTable")
            If Not l_DataTable Is Nothing Then
                For Each l_DataRow In l_DataTable.Rows
                    l_Decimal_Taxable = IIf(l_DataRow("Taxable").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("Taxable"), Decimal))
                    l_DataRow("Tax") = l_Decimal_Taxable * (Me.COVIDTaxrate / 100.0)
                    l_DataRow("TaxRate") = Me.COVIDTaxrate
                Next
            End If
            objRefundProcess.CovidTaxRate = Me.COVIDTaxrate
            Me.txtTaxRateC19.Text = Me.COVIDTaxrate
            Me.LoadCOVIDValuesIntoControls()
            Me.DoFinalCalculation()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub
    'END : ML | 2020.05.04 | YRS-AT-4874 | Recalculate withdrawals amounts on change of COVID Tax Rate

    Private Function DoVoluntryRefund(Optional ByVal parameterDataTable As DataTable = Nothing) As DataTable
        Try

            If Not parameterDataTable Is Nothing Then
                Session("CalculatedDataTable_C19") = parameterDataTable
            End If
            objRefundProcess.DoVoluntryRefund("IsConsolidated", True)
            DoVoluntryRefund = objRefundProcess.ContributionDataTable()
        Catch ex As Exception
            Throw
        End Try

    End Function

#Region " Full Refund "


    '' This function for Full Refund.
    Private Function DoFullRefund(Optional ByVal parameterDataTable As DataTable = Nothing) As DataTable
        Try

            If Not parameterDataTable Is Nothing Then
                Session("CalculatedDataTable_C19") = parameterDataTable
            End If
            objRefundProcess.DoFullOrPersRefund("IsConsolidated")
            DoFullRefund = objRefundProcess.ContributionDataTable()
        Catch ex As Exception
            Throw
        End Try

    End Function

#End Region

    Protected Sub Text_Changed(ByVal sender As Object, ByVal e As EventArgs)

        Dim l_Payee2DataTable As DataTable
        Dim l_Payee1DataTable As DataTable

        Try
            Dim TxtBox As TextBox = CType(sender, TextBox)
            Dim dgItem As DataGridItem = CType(TxtBox.NamingContainer, DataGridItem)
            Dim i As Integer = dgItem.ItemIndex
            Try

                If TxtBox.Text.Trim = String.Empty Then
                    TxtBox.Text = 0
                ElseIf TxtBox.Text.Trim <> 0 Then
                    TxtBox.Text = TxtBox.Text.TrimStart("0")
                End If
            Catch ex As Exception
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Amount is invalid.", MessageBoxButtons.Stop)
            End Try

            Try
                TxtBox.Text = Math.Round(Convert.ToDecimal(TxtBox.Text.Trim), 2)
            Catch ex As Exception
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Amount is invalid.", MessageBoxButtons.Stop)
                Exit Sub
            End Try

            If Convert.ToDouble(TxtBox.Text.Trim) < 0 Then
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Amount cannot be negative.", MessageBoxButtons.Stop)
                Exit Sub
            End If
            DoReverseCalculationForGrids()
            If Me.RefundType = "REG" Then
                Dim l_checkBox As CheckBox
                For Each l_DataGridItem As DataGridItem In Me.DatagridDeductions.Items
                    l_checkBox = l_DataGridItem.FindControl("CheckBoxDeduction")
                    If Not l_checkBox Is Nothing Then
                        If l_checkBox.Checked = True Then
                            CheckBoxDeduction_Checked(Nothing, Nothing)
                            Exit For
                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Public Function ValidateTotal() As Boolean

        Try
            Dim l_Taxable As Decimal
            Dim l_NonTaxable As Decimal
            Dim l_TxtBox As TextBox
            Dim l_Label As Label
            Dim l_index As Integer
            Dim l_lblTaxable As Label
            Dim l_lblNonTaxable As Label

            If Me.DataGridPayee1.Items.Count > 0 Then
                For l_index = 0 To Me.DataGridPayee1.Items.Count - 1
                    l_Label = CType(Me.DataGridPayee1.Items(l_index).Cells(1).FindControl("LabelPayee1Taxable"), Label)
                    l_Taxable = Math.Round(Convert.ToDecimal(l_Taxable), 2) + Math.Round(Convert.ToDecimal(l_Label.Text), 2)
                    l_Label = CType(Me.DataGridPayee1.Items(l_index).Cells(2).FindControl("LabelPayee1NonTaxable"), Label)
                    l_NonTaxable = Math.Round(Convert.ToDecimal(l_NonTaxable), 2) + Math.Round(Convert.ToDecimal(l_Label.Text), 2)
                Next
            End If

            If Me.DatagridPayee2.Items.Count > 0 Then
                For l_index = 0 To Me.DatagridPayee2.Items.Count - 1
                    'Commented code deleted by SG: 2012.03.16
                    'Added on 25/08/2011 BT:830- YRS 5.0-1326 : Rollover including non-taxable money.
                    l_lblTaxable = CType(Me.DatagridPayee2.Items(l_index).Cells(1).FindControl("LabelTaxable"), Label)
                    l_Taxable = Math.Round(Convert.ToDecimal(l_Taxable), 2) + Math.Round(Convert.ToDecimal(l_lblTaxable.Text), 2)
                    l_lblNonTaxable = CType(Me.DatagridPayee2.Items(l_index).Cells(2).FindControl("LabelNonTaxable"), Label)
                    l_NonTaxable = Math.Round(Convert.ToDecimal(l_NonTaxable), 2) + Math.Round(Convert.ToDecimal(l_lblNonTaxable.Text), 2)
                Next
            End If

            If Me.DatagridPayee3.Items.Count > 0 Then
                For l_index = 0 To Me.DatagridPayee3.Items.Count - 1
                    l_TxtBox = CType(Me.DatagridPayee3.Items(l_index).Cells(1).FindControl("TextboxPayee3Taxable"), TextBox)
                    If l_TxtBox.Text = "" Then
                        l_TxtBox.Text = "0.00"
                    End If
                    l_Taxable = Math.Round(Convert.ToDecimal(l_Taxable), 2) + Math.Round(Convert.ToDecimal(l_TxtBox.Text), 2)
                    l_TxtBox = CType(Me.DatagridPayee3.Items(l_index).Cells(2).FindControl("TextboxPayee3NonTaxable"), TextBox)
                    l_NonTaxable = Math.Round(Convert.ToDecimal(l_NonTaxable), 2) + Math.Round(Convert.ToDecimal(l_TxtBox.Text), 2)
                Next
            End If
            If Math.Round(l_Taxable, 2) > Convert.ToDecimal(TextboxTaxableFinal.Text) Then
                'Return False
                'MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Amounts are Invalid.", MessageBoxButtons.Stop, True)
            Else
                If Math.Round(l_NonTaxable, 2) > Convert.ToDecimal(TextboxNonTaxableFinal.Text) Then
                    Return False
                    '  MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Amounts are Invalid.", MessageBoxButtons.Stop, True)
                End If
            End If
            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Sub DoReverseCalculationForGrids()
        Dim l_double_FinalTaxable As Double
        Dim l_double_FinalNonTaxable As Double
        Dim l_index As Integer
        Dim l_gridindex As Integer
        Dim l_TxtTaxable As TextBox
        Dim l_TxtNonTaxable As TextBox
        Dim l_Taxable As Label
        Dim l_NonTaxable As Label
        Dim l_LblTaxable As Label
        Dim l_LblNonTaxable As Label
        Dim l_Payee1TempDataTable As DataTable
        Dim l_Payee1DataTable As DataTable
        Dim l_Payee2DataTable As DataTable
        Dim l_Payee3DataTable As DataTable
        Dim l_bool_Validate As Boolean
        l_bool_Validate = False
        Try

            l_Payee1TempDataTable = Session("Payee1TempDataTable_C19")
            'If Me.RefundType = "PART" Then
            '    l_Payee1TempDataTable = objRefundRequest.FixRoundingIssueProcessing(l_Payee1TempDataTable, Me.SessionGrossAmount, "PayeeDataDrid")
            'End If
            l_Payee1DataTable = Session("Payee1DataTable_C19")
            l_Payee2DataTable = Session("Payee2DataTable_C19")
            l_Payee3DataTable = Session("Payee3DataTable_C19")

            For l_index = 0 To Me.DataGridPayee1.Items.Count - 1
                l_LblTaxable = Me.DataGridPayee1.Items(l_index).Cells(1).FindControl("LabelPayee1Taxable")
                l_LblNonTaxable = Me.DataGridPayee1.Items(l_index).Cells(1).FindControl("LabelPayee1NonTaxable")

                l_double_FinalTaxable = Convert.ToDouble(l_Payee1TempDataTable.Rows(l_index).Item("Taxable"))
                l_double_FinalNonTaxable = Convert.ToDouble(l_Payee1TempDataTable.Rows(l_index).Item("NonTaxable"))

                If Me.DatagridPayee3.Items.Count > 0 Then
                    l_TxtTaxable = CType(Me.DatagridPayee3.Items(l_index).Cells(1).FindControl("TextBoxPayee3Taxable"), TextBox)
                    l_TxtNonTaxable = CType(Me.DatagridPayee3.Items(l_index).Cells(2).FindControl("TextBoxPayee3NonTaxable"), TextBox)
                    'Added Ganeswar 10thApril09 for BA Account Phase-V /begin
                    'On click of Tab out application was throwing Error Page as “Thread was being aborted. if user enter special characters” 

                    Try
                        If l_TxtTaxable.Text.Trim < 0 Or l_TxtNonTaxable.Text.Trim < 0 Then
                            MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Amount is invalid.", MessageBoxButtons.Stop)
                            Exit Sub
                        End If
                    Catch
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Amount is invalid.", MessageBoxButtons.Stop)
                        Exit Sub
                    End Try

                    'Added Ganeswar 10thApril09 for BA Account Phase-V /End
                    l_double_FinalTaxable = Math.Round(l_double_FinalTaxable, 2) - Math.Round(Convert.ToDouble(l_TxtTaxable.Text), 2)
                    l_double_FinalNonTaxable = Math.Round(l_double_FinalNonTaxable, 2) - Math.Round(Convert.ToDouble(l_TxtNonTaxable.Text), 2)


                End If
                Dim dblNonTaxable As Double = 0.0
                Dim dblTaxable As Double = 0.0
                If Me.DatagridPayee2.Items.Count > 0 Then
                    'l_TxtTaxable = CType(Me.DatagridPayee2.Items(l_index).Cells(1).FindControl("TextBoxPayee2Taxable"), TextBox)
                    'l_TxtNonTaxable = CType(Me.DatagridPayee2.Items(l_index).Cells(2).FindControl("TextBoxPayee2NonTaxable"), TextBox)
                    'Added on 24/08/2011 BT830:YRS 5.0-1326 : Rollover including non-taxable money.
                    l_Taxable = CType(Me.DatagridPayee2.Items(l_index).Cells(1).FindControl("LabelTaxable"), Label)
                    l_NonTaxable = CType(Me.DatagridPayee2.Items(l_index).Cells(2).FindControl("LabelNonTaxable"), Label)
                    'Added Ganeswar 10thApril09 for BA Account Phase-V /begin
                    'On click of  Tab out application was throwing Error Page as “Thread was being aborted. if user enter special characters” 
                    Try
                        If l_Taxable.Text.Trim < 0 Or l_NonTaxable.Text.Trim < 0 Then
                            MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Amount is invalid.", MessageBoxButtons.Stop)
                            Exit Sub
                        End If
                    Catch
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Amount is invalid.", MessageBoxButtons.Stop)
                        Exit Sub
                    End Try
                    'Added Ganeswar 10thApril09 for BA Account Phase-V /End

                    'l_double_FinalTaxable = Math.Round(l_double_FinalTaxable, 2) - Math.Round(Convert.ToDouble(l_TxtTaxable.Text), 2)
                    'l_double_FinalNonTaxable = Math.Round(l_double_FinalNonTaxable, 2) - Math.Round(Convert.ToDouble(l_TxtNonTaxable.Text), 2)
                    'Added on 24/08/2011 BT830:YRS 5.0-1326 : Rollover including non-taxable money.
                    l_double_FinalTaxable = Math.Round(l_double_FinalTaxable, 2) - Math.Round(Convert.ToDouble(l_Taxable.Text), 2)
                    l_double_FinalNonTaxable = Math.Round(l_double_FinalNonTaxable, 2) - Math.Round(Convert.ToDouble(l_NonTaxable.Text), 2)


                End If
                If l_double_FinalTaxable < 0 Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Amounts are Invalid.", MessageBoxButtons.Stop, False)
                    Exit Sub
                Else
                    If l_double_FinalNonTaxable < 0 Then
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Amounts are Invalid.", MessageBoxButtons.Stop, False)
                        Exit Sub
                    End If
                End If
                'l_LblTaxable.Text = l_double_FinalTaxable
                'l_LblNonTaxable.Text = l_double_FinalNonTaxable
                'Shubhrata & Priya BudId 458 : 22-May-2008
                l_LblTaxable.Text = Math.Round(Convert.ToDouble(l_double_FinalTaxable), 2)
                l_LblNonTaxable.Text = Math.Round(Convert.ToDouble(l_double_FinalNonTaxable), 2)
                'Shubhrata & Priya BudId 458 : 22-May-2008
            Next

            l_bool_Validate = ValidateTotal()

            If l_bool_Validate Then
                For l_gridindex = 0 To Me.DataGridPayee1.Items.Count - 1
                    l_LblTaxable = Me.DataGridPayee1.Items(l_gridindex).Cells(1).FindControl("LabelPayee1Taxable")
                    l_LblNonTaxable = Me.DataGridPayee1.Items(l_gridindex).Cells(1).FindControl("LabelPayee1NonTaxable")
                    'l_Payee1TempDataTable.Rows(l_index).Item("Taxable") = Convert.ToDouble(l_LblTaxable.Text)
                    'l_Payee1TempDataTable.Rows(l_index).Item("NonTaxable") = Convert.ToDouble(l_LblNonTaxable.Text)
                    l_Payee1DataTable.Rows(l_gridindex).Item("Taxable") = Convert.ToDouble(l_LblTaxable.Text)
                    l_Payee1DataTable.Rows(l_gridindex).Item("NonTaxable") = Convert.ToDouble(l_LblNonTaxable.Text)
                Next

                For l_gridindex = 0 To Me.DatagridPayee2.Items.Count - 1
                    'l_TxtTaxable = Me.DatagridPayee2.Items(l_gridindex).Cells(1).FindControl("TextboxPayee2Taxable")
                    'l_TxtNonTaxable = Me.DatagridPayee2.Items(l_gridindex).Cells(1).FindControl("TextboxPayee2NonTaxable")
                    'Added on 24/08/2011 BT830:YRS 5.0-1326 : Rollover including non-taxable money.
                    l_Taxable = Me.DatagridPayee2.Items(l_gridindex).Cells(1).FindControl("LabelTaxable")
                    l_NonTaxable = Me.DatagridPayee2.Items(l_gridindex).Cells(1).FindControl("LabelNonTaxable")
                    'l_Payee1TempDataTable.Rows(l_index).Item("Taxable") = Convert.ToDouble(l_LblTaxable.Text)
                    'l_Payee1TempDataTable.Rows(l_index).Item("NonTaxable") = Convert.ToDouble(l_LblNonTaxable.Text)
                    'Added by Ganeswar on 03-06-2009
                    If l_Payee2DataTable.Rows.Count = 0 Then
                        l_Payee2DataTable = l_Payee1DataTable.Copy
                    End If

                    If l_Payee2DataTable.Rows.Count > 0 Then
                        'Added on 24/08/2011 BT830:YRS 5.0-1326 : Rollover including non-taxable money.
                        l_Payee2DataTable.Rows(l_gridindex).Item("Taxable") = Convert.ToDouble(l_Taxable.Text)
                        l_Payee2DataTable.Rows(l_gridindex).Item("NonTaxable") = Convert.ToDouble(l_NonTaxable.Text)
                    End If
                    'Added by Ganeswar on 03-06-2009
                Next
                For l_gridindex = 0 To Me.DatagridPayee3.Items.Count - 1
                    l_TxtTaxable = Me.DatagridPayee3.Items(l_gridindex).Cells(1).FindControl("TextboxPayee3Taxable")
                    l_TxtNonTaxable = Me.DatagridPayee3.Items(l_gridindex).Cells(1).FindControl("TextboxPayee3NonTaxable")
                    'l_Payee1TempDataTable.Rows(l_index).Item("Taxable") = Convert.ToDouble(l_LblTaxable.Text)
                    'l_Payee1TempDataTable.Rows(l_index).Item("NonTaxable") = Convert.ToDouble(l_LblNonTaxable.Text)
                    'Added by Ganeswar on 03-06-2009
                    If l_Payee3DataTable.Rows.Count = 0 Then
                        l_Payee3DataTable = l_Payee1DataTable.Copy
                    End If
                    If l_Payee3DataTable.Rows.Count > 0 Then
                        l_Payee3DataTable.Rows(l_gridindex).Item("Taxable") = Convert.ToDouble(l_TxtTaxable.Text)
                        l_Payee3DataTable.Rows(l_gridindex).Item("NonTaxable") = Convert.ToDouble(l_TxtNonTaxable.Text)
                    End If
                    'Added by Ganeswar on 03-06-2009
                Next

                Session("Payee1DataTable_C19") = l_Payee1DataTable
                Session("Payee2DataTable_C19") = l_Payee2DataTable
                Session("Payee3DataTable_C19") = l_Payee3DataTable
                'Hafiz 03Feb06 Cache-Session
                LoadPayee1ValuesIntoControls()
                LoadPayee2ValuesIntoControls()
                LoadPayee3ValuesIntoControls()
                DoFinalCalculation()

            Else
                'Commented code deleted by SG: 2012.03.16
            End If


        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub
    'Commented code deleted by SG: 2012.03.16

    Private Sub TextboxPayee3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxPayee3.TextChanged
        Dim index As Integer
        Dim l_TxtTaxable As TextBox
        Dim l_TxtNonTaxable As TextBox
        Try
            If TextboxPayee3.Text.Trim.Length > 0 Then
                For index = 0 To Me.DatagridPayee3.Items.Count - 1
                    l_TxtTaxable = CType(Me.DatagridPayee3.Items(index).Cells(1).FindControl("TextBoxPayee3Taxable"), TextBox)
                    l_TxtNonTaxable = CType(Me.DatagridPayee3.Items(index).Cells(2).FindControl("TextBoxPayee3NonTaxable"), TextBox)
                    l_TxtTaxable.Enabled = True
                    If Me.RadioButtonTaxableOnly.Checked = False Then
                        l_TxtNonTaxable.Enabled = True
                    End If

                Next
            Else
                For index = 0 To Me.DatagridPayee3.Items.Count - 1
                    l_TxtTaxable = CType(Me.DatagridPayee3.Items(index).Cells(1).FindControl("TextBoxPayee3Taxable"), TextBox)
                    l_TxtNonTaxable = CType(Me.DatagridPayee3.Items(index).Cells(2).FindControl("TextBoxPayee3NonTaxable"), TextBox)
                    l_TxtTaxable.Enabled = True
                    l_TxtTaxable.Text = "0.00"
                    l_TxtNonTaxable.Text = "0.00"
                    DoReverseCalculationForGrids()
                    l_TxtNonTaxable.Enabled = True
                Next

            End If
            LoadPayee3ValuesIntoControls()
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub RadiobuttonPersonalMoniesYes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadiobuttonPersonalMoniesYes.CheckedChanged
        Dim l_PersonalMoniesDataTable As DataTable
        Dim l_CurrentAcctsDataTable As DataTable
        Try
            If RadiobuttonPersonalMoniesYes.Checked = True Then
                Session("YesClicked_C19") = True
                l_PersonalMoniesDataTable = Session("CalculatedDataTableForPersonalMonies_C19")
                l_CurrentAcctsDataTable = Session("CalculatedDataTableForCurrentAccounts_C19")
                Session("CalculatedDataTableForCurrentAccounts_C19") = l_PersonalMoniesDataTable
                Session("CalculatedDataTableForPersonalMonies_C19") = l_CurrentAcctsDataTable

                'Vipul 24Feb06 - fixing the Issue of 2088 - Pers side only refund using YMCA side interest and leaving a balance in the Pers side payts
                Me.IsPersonalOnly = True
                'Vipul 24Feb06 - fixing the Issue of 2088 - Pers side only refund using YMCA side interest and leaving a balance in the Pers side payts

                'Hafiz 03Feb06 Cache-Session
                objRefundProcess.CreatePayees()
                Me.LoadPayeesDataGrid()
                Me.LoadPayee1ValuesIntoControls()
                Me.LoadPayee2ValuesIntoControls()
                Me.LoadPayee3ValuesIntoControls()
                Me.EnableMinimumDistributionControls()

                Me.DoFinalCalculation()
                Me.DoNone()

                'Commented and Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
                'If Me.RadioButtonRolloverYes.Checked = False Then
                If Me.CheckBoxRollovers.Checked = False Then
                    Me.TextboxPayee3.ReadOnly = True
                    Me.TextboxPayee2.rReadOnly = True 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
                End If
            End If
        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try
    End Sub

    Private Sub RadiobuttonPersonalMoniesNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadiobuttonPersonalMoniesNo.CheckedChanged
        'Commented code deleted by SG: 2012.03.16
        Dim l_PersonalMoniesDataTable As DataTable
        Dim l_CurrentAcctsDataTable As DataTable
        Try
            'Commented code deleted by SG: 2012.03.16
            If RadiobuttonPersonalMoniesNo.Checked = True And Session("YesClicked_C19") = True Then
                Session("YesClicked_C19") = False
                'Commented code deleted by SG: 2012.03.16

                l_CurrentAcctsDataTable = Session("CalculatedDataTableForPersonalMonies_C19")
                l_PersonalMoniesDataTable = Session("PermanentDataTableForPersonalMonies_C19")
                Session("CalculatedDataTableForCurrentAccounts_C19") = l_CurrentAcctsDataTable
                Session("CalculatedDataTableForPersonalMonies_C19") = l_PersonalMoniesDataTable

                'Vipul 24Feb06 - fixing the Issue of 2088 - Pers side only refund using YMCA side interest and leaving a balance in the Pers side payts
                Me.IsPersonalOnly = False
                'Vipul 24Feb06 - fixing the Issue of 2088 - Pers side only refund using YMCA side interest and leaving a balance in the Pers side payts

                'Hafiz 03Feb06 Cache-Session
                objRefundProcess.CreatePayees()
                Me.LoadPayeesDataGrid()
                Me.LoadPayee1ValuesIntoControls()
                Me.LoadPayee2ValuesIntoControls()
                Me.LoadPayee3ValuesIntoControls()
                Me.EnableMinimumDistributionControls()
                Me.DoFinalCalculation()
                'Commented and Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
                'If Me.RadioButtonRolloverYes.Checked = False Then
                If Me.CheckBoxRollovers.Checked = False Then
                    Me.TextboxPayee3.ReadOnly = True
                    Me.TextboxPayee2.rReadOnly = True 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
                End If
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)

        End Try

    End Sub

    Private Sub TextboxPayee3_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxPayee3.Unload

    End Sub

    Private Function ChangeStatus()
        Try

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub DatagridNonFundedContributions_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridNonFundedContributions.ItemDataBound
        Try
            e.Item.Cells(0).Visible = False
            e.Item.Cells(2).Visible = False
            If e.Item.ItemType <> ListItemType.Header Then
                If e.Item.Cells(0).Text.ToUpper <> "&NBSP;" Or e.Item.Cells(1).Text.ToUpper = "TOTAL" Then
                    ' If e.Item.Cells(1).Text.ToUpper = "TOTAL" Then
                    Dim l_decimal_try As Decimal
                    'Commented code deleted by SG: 2012.03.16

                    l_decimal_try = Convert.ToDecimal(e.Item.Cells(4).Text)
                    e.Item.Cells(4).Text = FormatCurrency(l_decimal_try)

                    l_decimal_try = Convert.ToDecimal(e.Item.Cells(5).Text)
                    e.Item.Cells(5).Text = FormatCurrency(l_decimal_try)

                    l_decimal_try = Convert.ToDecimal(e.Item.Cells(6).Text)
                    e.Item.Cells(6).Text = FormatCurrency(l_decimal_try)

                    l_decimal_try = Convert.ToDecimal(e.Item.Cells(7).Text)
                    e.Item.Cells(7).Text = FormatCurrency(l_decimal_try)

                    'Commented code deleted by SG: 2012.03.16
                End If
            End If

        Catch ex As Exception
            Throw
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

    Private Sub ButtonAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddress.Click
        'Made by ashutosh 18/04/06**************************
        Session("ds_PrimaryAddress_C19") = Nothing
        Dim popupScript As String
        popupScript = "<script language='javascript'>" & _
                                                  "window.open('UpdateAddressDetails.aspx', 'CustomPopUp', " & _
                                                  "'width=700, height=600, menubar=no, Resizable=no,top=50,left=120, scrollbars=yes')" & _
                                                  "</script>"



        Page.RegisterStartupScript("PopupScript1", popupScript)
        '*********************************
    End Sub

#Region "Hard Ship Functions"
    Private Function DoHardshipRefundForCurrentAccount()

        Dim l_ContributionDataTable As DataTable
        Dim l_DataRow As DataRow

        Dim l_UserSide As Boolean
        Dim l_YMCASide As Boolean
        Dim l_Interest As Boolean
        Dim l_AccountGroup As String

        Dim l_Decimal_PersonalPreTax As Decimal
        Dim l_Decimal_PersonalInterest As Decimal
        Dim l_Decimal_personalTotal As Decimal

        Dim l_Decimal_Total As Decimal

        Me.HardshipAvailable = 0.0
        Me.TotalAvailable = 0.0


        Try


            l_ContributionDataTable = Session("CalculatedDataTableForCurrentAccounts_C19")

            l_ContributionDataTable.RejectChanges()

            l_ContributionDataTable = objRefundProcess.DoFlagSetForHardshipRefund(l_ContributionDataTable, True)
            'l_ContributionDataTable.AcceptChanges()

            Session("CalculatedDataTable_C19") = l_ContributionDataTable

            Me.CalculateTotal(l_ContributionDataTable)

            Me.LoadCalculatedTableForCurrentAccounts()
            ' Me.SetSelectedIndex(Me.DatagridCurrentAccts, l_ContributionDataTable)



        Catch ex As Exception
            Throw
        End Try



    End Function
    Private Function DoHardshipRefund(Optional ByVal parameterDataTable As DataTable = Nothing)
        Try

            'Commented code deleted by SG: 2012.03.16

            DoHardshipRefundForRefundAccounts()
        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function DoHardshipRefundForRefundAccounts()

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow


        Dim l_AccountGroup As String

        Try

            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL START
            l_DataTable = CType(Session("RefundableDataTable_C19"), DataTable)
            'Will have to validate this session Properly: VPR

            objRefundProcess.DoFlagSetForHardshipRefund(l_DataTable, False)

            For Each l_DataRow In l_DataTable.Rows
                If l_DataRow.RowState <> DataRowState.Deleted Then
                    l_DataRow("Emp.Total") = Convert.ToDecimal(l_DataRow("Taxable")) + Convert.ToDecimal(l_DataRow("Non-Taxable")) + Convert.ToDecimal(l_DataRow("Interest"))
                    l_DataRow("YMCATotal") = Convert.ToDecimal(l_DataRow("YMCATaxable")) + Convert.ToDecimal(l_DataRow("YMCAInterest"))
                    l_DataRow("Total") = Convert.ToDecimal(l_DataRow("Emp.Total")) + Convert.ToDecimal(l_DataRow("YMCATotal"))
                End If
            Next

            l_DataTable.AcceptChanges()
            Session("RefundableDataTable_C19") = l_DataTable

        Catch ex As Exception
            Throw
        End Try


    End Function

    Private Function LoadHardShipvalues()
        Try
            Me.TextboxTDAvailableAmount.Text = Math.Round(Me.HardshipAvailable, 2)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ProcessRequiredAmount()

        ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL - Start
        'Logic processed : TD alc is only used when all the other alc has less balance for Requested amount.Values of Alcs used 
        'will be reflectd in the Taxable/Nontaxable Textbox for the payee1
        Try
            Dim l_RequestedAmount As Decimal
            If Me.TextboxRequestAmount.Text <> "" Then
                l_RequestedAmount = Convert.ToDecimal(Me.TextboxRequestAmount.Text)
            End If
            If l_RequestedAmount <= 0.0 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", "Please Enter the Amount Desired.", MessageBoxButtons.Stop, False)
                Return 0
            End If

            'Commented code deleted by SG: 2012.03.16

            If l_RequestedAmount <= Math.Round(CDec(TextboxTDAvailableAmount.Text), 2) Then
                SetHardShipTaxableAndNonTaxableForDisplay(l_RequestedAmount) 'START : MMR | 2017.07.30 | YRS-AT-3870 | Adding hardship non-taxable component to net non-taxable amount
                'Added by Ganeswar for HardShip TaxAmount on 10th july09
                If l_bool_HardShipAmount Then
                    If TextboxHardShipTaxRate.Text = 0 Then
                        Me.TDTaxRate = 0
                        Me.HardShipTaxRate = 0
                    Else
                        Me.TDTaxRate = Me.HardShipTaxRate
                    End If
                Else
                    Me.IsHardShip = True
                    Me.HardshipAmount = l_RequestedAmount
                    Me.TDUsedAmount = Me.HardshipAmount
                End If
                'Commented code deleted by SG: 2012.03.16

                'Added by Ganeswar for HardShip TaxAmount on 10th july09
            Else
                'Commented code deleted by SG: 2012.03.16
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS ", "Requested Amount Exceeds TD Available Amount - Requested Amount has been Adjusted.", MessageBoxButtons.Stop, False)
                Me.TextboxRequestAmount.Text = Math.Round(CDec(TextboxTDAvailableAmount.Text), 2)
                Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate
                'LoadValuesForProcessedAmounts(True)
                'Entire Hardship amount has been used. - Set value for TD alc
                'START : MMR | 2017.07.30 | YRS-AT-3870 | Adding hardship non-taxable component to net non-taxable amount
                'Me.TextboxHardShipAmount.Text = Math.Round(Me.HardshipAvailable, 2).ToString("0.00")
                'Me.TextboxHardShip.Text = Math.Round(Me.HardshipAvailable * (Me.TDTaxRate / 100.0), 2).ToString("0.00")
                'Me.TextboxHardShipNet.Text = Math.Round(Me.HardshipAvailable - (Me.HardshipAvailable * (Me.TDTaxRate / 100.0)), 2).ToString("0.00")
                Me.TextboxHardShipAmount.Text = Math.Round(objRefundProcess.HardshipWithdrawalTaxable, 2).ToString("0.00")
                Me.textboxHardShipNonTaxableAmount.Text = Math.Round(objRefundProcess.HardshipWithdrawalNonTaxable, 2).ToString("0.00")
                Me.TextboxHardShip.Text = Math.Round(objRefundProcess.HardshipWithdrawalTaxable * (Me.TDTaxRate / 100.0), 2).ToString("0.00")
                Me.TextboxHardShipNet.Text = Math.Round(objRefundProcess.HardshipWithdrawalTaxable + objRefundProcess.HardshipWithdrawalNonTaxable - (objRefundProcess.HardshipWithdrawalTaxable * (Me.TDTaxRate / 100.0)), 2).ToString("0.00")
                'END : MMR | 2017.07.30 | YRS-AT-3870 | Adding hardship non-taxable component to net non-taxable amount
                ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL - Start
                Me.IsHardShip = True
                'START : MMR | 2017.07.30 | YRS-AT-3870 | Add hardship amount should be sum of taxable & non-taxable. 
                'Me.HardshipAmount = Me.TextboxHardShipAmount.Text
                Me.HardshipAmount = Math.Round(CDec(Me.TextboxHardShipAmount.Text), 2) + Math.Round(CDec(Me.textboxHardShipNonTaxableAmount.Text), 2)
                'START : MMR | 2017.07.30 | YRS-AT-3870 | Add hardship amount should be sum of taxable & non-taxable.
                Me.TDUsedAmount = Me.HardshipAmount
                Me.TextboxHardShipTaxRate.ReadOnly = False
                DoFinalCalculation()
                ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL - End
                Return 0
            End If
            'End comented by Ganeswar for Hardship roll over
            objRefundProcess.IsHardShip = Me.IsHardShip
            'Need to calculate TD factor used.
            Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate
            'START : MMR | 2017.07.30 | YRS-AT-3870 | Adding hardship non-taxable component to net non-taxable amount
            'Me.TextboxHardShipAmount.Text = Me.TDUsedAmount
            'Rahul
            'Me.TextboxHardShip.Text = Math.Round(Me.TDUsedAmount * (Me.TDTaxRate / 100.0), 2).ToString("0.00")
            'Me.TextboxHardShipNet.Text = Math.Round((Me.TDUsedAmount - Me.TextboxHardShip.Text), 2)
            Me.TextboxHardShip.Text = Math.Round(CDec(Me.TextboxHardShipAmount.Text) * (Me.TDTaxRate / 100.0), 2).ToString("0.00")
            Me.TextboxHardShipNet.Text = Math.Round((CDec(Me.TextboxHardShipAmount.Text) + CDec(Me.textboxHardShipNonTaxableAmount.Text) - Me.TextboxHardShip.Text), 2)
            'END : MMR | 2017.07.30 | YRS-AT-3870 | Adding hardship non-taxable component to net non-taxable amount
            'rahul
            'Load  values in grid
            'Start Commented By Ganeswar for Hardship roll over on 25/05/2009
            'LoadValuesForProcessedAmounts(False)

            If Me.RefundType = "HARD" And TextboxVoluntaryWithdrawalTotal.Text > "0.00" Then
                LoadValueForhardShipRollOver()
            Else
                LoadValuesForProcessedAmounts(False)
            End If
            DoFinalCalculation()
            'Start Commented By Ganeswar for Hardship roll over on 25/05/2009

        Catch IcEx As InvalidCastException
            MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", "Invalid Amount entered. Please enter valid Amount.", MessageBoxButtons.Stop, False)
            'Me.TextboxHardShipTaxRate.Text = "0.00"
        Catch ex As Exception
            Throw ex
        End Try

        ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL - End
    End Function

    Private Function LoadValuesForProcessedAmounts(ByVal ExceedAmountFlag As Boolean)
        Dim l_Decimal_TempValue As Decimal = 0.0
        Dim l_Payee1DataTable As DataTable
        Dim txtFinalTaxableAmount As Double = 0.0
        Dim txtFinalNonTaxableAmount As Double = 0.0
        Dim txtFinalTDTaxableAmount As Double = 0.0


        objRefundProcess.CreatePayees()
        Me.SetPropertiesAfterCreatePayees()

        chkCreatePayeeProcessed = True

        l_Payee1DataTable = CType(Session("Payee1DataTable_C19"), DataTable)
        'l_Decimal_TempValue = CType(Me.TextboxRequestAmount.Text, Decimal)
        'Shubhrata May23rd
        If Me.TextboxRequestAmount.Text.Trim <> "" Then
            l_Decimal_TempValue = Convert.ToDecimal(Me.TextboxRequestAmount.Text)
        End If

        'Shubhrata May23rd
        If Not l_Payee1DataTable Is Nothing Then
            For Each l_DataRow As DataRow In l_Payee1DataTable.Rows
                l_DataRow("Use") = "False"
            Next
            For Each l_DataRow As DataRow In l_Payee1DataTable.Rows
                If l_Decimal_TempValue > 0.0 Then
                    l_DataRow("Use") = "True"
                    l_Decimal_TempValue -= IIf(l_DataRow("Taxable").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("Taxable"), Decimal))
                    l_Decimal_TempValue -= IIf(l_DataRow("NonTaxable").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("NonTaxable"), Decimal))
                    txtFinalTaxableAmount = txtFinalTaxableAmount + CType(l_DataRow("Taxable"), Decimal)
                    txtFinalNonTaxableAmount = txtFinalNonTaxableAmount + CType(l_DataRow("NonTaxable"), Decimal)
                End If
            Next

            If (ExceedAmountFlag = False) Then
                Me.DoNone() 'By default it is donone process 
                Me.TextboxPayee2.rReadOnly = True 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
            End If

            TextboxTaxable.Text = Math.Round(txtFinalTaxableAmount, 2).ToString("0.00")
            TextboxNonTaxable.Text = Math.Round(txtFinalNonTaxableAmount, 2).ToString("0.00")

            Me.TextboxTax.Text = Math.Round(Math.Round(txtFinalTaxableAmount, 2) * (Me.TaxRate / 100.0), 2).ToString("0.00")
            Me.TextboxNet.Text = Double.Parse(TextboxTaxable.Text) + Double.Parse(TextboxNonTaxable.Text) - Double.Parse(TextboxTax.Text)
            'Commented code deleted by SG: 2012.03.16

        End If
    End Function

    Private Sub LoadValueForhardShipRollOver()
        Dim l_Decimal_TempValue As Decimal = 0.0
        Dim l_Payee1DataTable As DataTable
        Dim txtFinalTaxableAmount As Double = 0.0
        Dim txtFinalNonTaxableAmount As Double = 0.0
        Dim txtFinalTDTaxableAmount As Double = 0.0
        l_Payee1DataTable = CType(Session("Payee1DataTable_C19"), DataTable)

        'Shubhrata May23rd
        If Not l_Payee1DataTable Is Nothing Then
            For Each l_DataRow As DataRow In l_Payee1DataTable.Rows
                ' l_DataRow("Use") = "False"
            Next
            For Each l_DataRow As DataRow In l_Payee1DataTable.Rows
                ' l_DataRow("Use") = "True"
                txtFinalTaxableAmount = txtFinalTaxableAmount + CType(l_DataRow("Taxable"), Decimal)
                txtFinalNonTaxableAmount = txtFinalNonTaxableAmount + CType(l_DataRow("NonTaxable"), Decimal)
            Next
            TextboxTaxable.Text = Math.Round(txtFinalTaxableAmount, 2).ToString("0.00")
            TextboxNonTaxable.Text = Math.Round(txtFinalNonTaxableAmount, 2).ToString("0.00")
            Me.TextboxTax.Text = Math.Round(Math.Round(txtFinalTaxableAmount, 2) * (Me.TaxRate / 100.0), 2).ToString("0.00")
            Me.TextboxNet.Text = Double.Parse(TextboxTaxable.Text) + Double.Parse(TextboxNonTaxable.Text) - Double.Parse(TextboxTax.Text)
            Me.LoadPayee1ValuesIntoControls()
            Me.LoadPayee2ValuesIntoControls()
            Me.LoadPayee3ValuesIntoControls()
            DoFinalCalculation()
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End

        End If
    End Sub

    Private Sub SetPropertiesForIDM()
        'Ashutosh Patil as on 22-Mar-2007
        Try
            IDM.PreviewReport = True
            IDM.LogonToDb = True
            IDM.CreatePDF = True
            IDM.CreateIDX = True
            IDM.CopyFilesToIDM = True
            IDM.AppType = "A"
            'get YMCAID
            Dim l_String_StatusType As String
            Dim l_termdate As DateTime
            Dim l_stringPersonId As String
            Dim l_datatable As New DataTable
            Dim l_datarow_CurrentRow As DataRow()
            l_String_StatusType = "A"
            ' l_termdate = String.Empty
            l_stringPersonId = Session("PersonID")
            'Getting YMCAID for the participant
            If IDM.DatatableFileList(False) Then
            Else
                Throw New Exception("Unable to Process Hardship Report, Could not create dependent table")
            End If
            If Not Session("Member Employment_C19") Is Nothing Then
                l_datatable = CType(Session("Member Employment_C19"), DataTable)
                'StatusType
                l_datarow_CurrentRow = l_datatable.Select("FundEventID = '" & Session("FundID") & " ' and  (TermDate IS NULL OR StatusType =  '" & l_String_StatusType & "')")


                'SR:2013.11.07 - BT 2295 : During testimg(YRS 5.0-2229) we found that for multiple active employement, two reports are generated with same YMCA information
                ' To correct it below lines are commented and new lines of code are added.
                If l_datarow_CurrentRow.Length > 0 Then
                    'IDM.YMCAID = l_datarow_CurrentRow(0)("YMCAID")
                    'IDM.PersId = l_stringPersonId
                    'IB:on 06/Oct/2010 YRS 5.0-1181 No.of pdf generated depends on Member employment loop.
                    'For Each dr As DataRow In l_datarow_CurrentRow
                    '    IDM.YMCAID = l_datarow_CurrentRow(0)("YMCAID")
                    '    IDM.PersId = l_stringPersonId

                    '    IDM.ReportName = "birefltr.rpt"
                    '    IDM.OutputFileType = "BIREFLTR"
                    '    IDM.DocTypeCode = "BIREFLTR"
                    '    Call AssignParameterstoReport()
                    '    Session("HardshipMessage") += IDM.ExportToPDF()

                    'Next

                    For i = 0 To l_datarow_CurrentRow.Length - 1
                        IDM.YMCAID = l_datarow_CurrentRow(i)("YMCAID")
                        IDM.PersId = l_stringPersonId

                        IDM.ReportName = "birefltr.rpt"
                        IDM.OutputFileType = "BIREFLTR"
                        IDM.DocTypeCode = "BIREFLTR"
                        Call AssignParameterstoReport()
                        Session("HardshipMessage_C19") += IDM.ExportToPDF()
                    Next
                    'End, SR:2013.11.07-BT 2295: During testimg(YRS 5.0-2229) we found that for multiple active employement, two reports are generated with same YMCA information
                    ' To correct it below lines are commented and new lines of code are added.
                Else
                    '  HardShipReports = "Invalid YmcaId passed for IDX file generation"
                    Exit Sub
                End If

            Else
                '  HardShipReports = "Invalid YmcaId passed for IDX file generation"
                Exit Sub
            End If

            'Commented code deleted by SG: 2012.03.16
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub AssignParameterstoReport()
        Dim l_string_Persid As String
        Dim l_dataSet As New DataSet
        Dim l_datatable As New DataTable
        Dim l_datarow As DataRow
        Dim ArrListParamValues As New ArrayList
        Dim l_String_tmp As String = String.Empty

        Try
            l_string_Persid = CType(Session("PersonID"), String)
            If l_string_Persid.Trim = String.Empty Then
                Throw New Exception("Error: Person Id Not Found")
            End If
            'IB: Added on 08/Sep/2010 Change "Birefltr" report parameters list
            ArrListParamValues.Add(CType(l_string_Persid, String).Trim)
            'IB:on 06/Oct/2010 YRS 5.0-1181  Add new parameter in BIREFLTR.rpt
            ArrListParamValues.Add(CType(IDM.YMCAID, String).Trim)

            'Commented code deleted by SG: 2012.03.16

            'set the Report parameter
            IDM.ReportParameters = ArrListParamValues

        Catch
            Throw
        End Try
    End Sub

    Private Sub TextboxRequestAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxRequestAmount.TextChanged
        Try
            'Shubhrata May23rd
            'Ganeswar Added  for HardShip rollover on 03-06-2009
            Me.l_dec_TDRequestedAmount = 0
            Me.l_dec_TDAmount = 0
            If Me.TextboxRequestAmount.Text = "." Then
                Me.TextboxRequestAmount.Text = "0.00"
            End If
            Dim l_Decimal_Temp As Decimal
            'l_Decimal_Temp = Me.TextboxRequestAmount.Text.Trim()
            If Me.TextboxRequestAmount.Text <> "" Then
                l_Decimal_Temp = Convert.ToDecimal(Me.TextboxRequestAmount.Text)
                If l_Decimal_Temp < 1 Then
                    Me.TextboxRequestAmount.Text = "0.00"
                End If
            End If
            Me.RequestedAmount = l_Decimal_Temp
            'Shubhrata May23rd
            '  Me.RequestedAmount = CType(Me.TextboxRequestAmount.Text, Decimal)
            'Rahul 21 Feb,06
            Me.TextboxRequestAmount.Text = Math.Round(Me.RequestedAmount, 2)
            'Rahul 21 Feb,06
            If ValidateHardshipWithdrawal() = False Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS ", "Cannot take up a Hardship Withdrawal as Tax Deferred Amount exceeds $2000.", MessageBoxButtons.Stop, False)
                Exit Sub
            End If
            'changed by ruchi
            Me.EnableDisableSaveButton()
            ' end of change
            Me.ProcessRequiredAmount()

        Catch IcEx As InvalidCastException
            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS ", "Invalid Amount entered. Please enter valid Amount.", MessageBoxButtons.Stop, False)
            Me.RequestedAmount = 0.0
            Me.TextboxRequestAmount.Text = "0.00"
        Catch ex As Exception
            Throw
        End Try
    End Sub




    Private Sub TextboxHardShipTaxRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxHardShipTaxRate.TextChanged
        Try
            'Commented code deleted by SG: 2012.03.16
            'Changed by:Ganeswar for Hardship withdrawal 13 july 2009
            Me.l_dec_TDAmount = 0

            'Changed by:Ganeswar for Hardship withdrawal
            If TextboxHardShipTaxRate.Text.Trim.Length < 1 Then
                Me.TextboxHardShipTaxRate.Text = 10
            End If
            Me.HardShipTaxRate = CType(Me.TextboxHardShipTaxRate.Text, Decimal)
            Me.TDTaxRate = CType(Me.TextboxHardShipTaxRate.Text, Decimal)
            Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate

            'Ganeswar Start


            Dim l_RequestedAmount As Decimal
            If Me.TextboxRequestAmount.Text <> "" Then
                l_RequestedAmount = Convert.ToDecimal(Me.HardshipAvailable) + Convert.ToDecimal(Me.inVolAcctsAvailable)
            End If
            If Convert.ToDecimal(Me.TDTaxRate) > Convert.ToDecimal(l_RequestedAmount) Then '(Convert.ToDecimal(Me.HardshipAvailable) + Convert.ToDecimal(Me.inVolAcctsAvailable)) Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", "Requested Amount Exceeds Available Amount - Requested Amount has been Adjusted.", MessageBoxButtons.Stop, False)
                'Calculate total amount available
                Me.TextboxHardShipTaxRate.Text = Convert.ToDecimal(l_RequestedAmount)
            End If
            'Ganeswar End

            'Changed by:Preeti YRST-2254 9May06
            If Me.TDTaxRate < 0 Or Me.TDTaxRate > 100 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS ", "Tax Rate Should between 0% and 100%.", MessageBoxButtons.Stop, False)
                Me.TDTaxRate = 10
                Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate
            End If
            If Me.TDTaxRate < 0 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS ", "Tax Rate Should between 0% and 100%.", MessageBoxButtons.Stop, False)
                Me.TDTaxRate = 10
                Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate
            End If

            If Me.TDUsedAmount = 0 And Me.HardshipAvailable > 0.0 And Me.RequestedAmount > Me.HardshipAvailable Then
                Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate

                Me.TextboxHardShipAmount.Text = Math.Round(Me.HardshipAvailable, 2).ToString("0.00")
                Me.TextboxHardShip.Text = Math.Round(Me.HardshipAvailable * (Me.TDTaxRate / 100.0), 2).ToString("0.00")
                Me.TextboxHardShipNet.Text = Math.Round(Me.HardshipAvailable - (Me.HardshipAvailable * (Me.TDTaxRate / 100.0)), 2).ToString("0.00")

            Else
                Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate
                ' START | SR | 2019.07.31 | YRS-AT-3870 | Consider only taxable amount on tax rate change.
                'Me.TextboxHardShipAmount.Text = Math.Round(Me.TDUsedAmount, 2).ToString("0.00")
                'Me.TextboxHardShip.Text = Math.Round(Me.TDUsedAmount * (Me.TDTaxRate / 100.0), 2).ToString("0.00")
                ''Rahul
                'Me.TextboxHardShipNet.Text = Math.Round((Me.TDUsedAmount - (Math.Round(Me.TDUsedAmount * (Me.TDTaxRate / 100.0), 2).ToString("0.00"))), 2)
                ''Rahul
                Me.TextboxHardShip.Text = Math.Round(Me.TextboxHardShipAmount.Text * (Me.TDTaxRate / 100.0), 2).ToString("0.00")
                TextboxHardShipNet.Text = Math.Round((TextboxHardShipAmount.Text - TextboxHardShip.Text) + textboxHardShipNonTaxableAmount.Text, 2)
                ' END | SR | 2019.07.31 | YRS-AT-3870 | Consider only taxable amount on tax rate change.
            End If
            'Changed by:Ganeswar for Hardship withdrawal
            If Me.TextboxRequestAmount.Text = "0.00" Then
                Me.TextboxHardShipAmount.Text = "0.00"
                Me.textboxHardShipNonTaxableAmount.Text = "0.00" 'MMR | 2017.07.30 | YRS-AT-3870 | Initalizing control value for non-taxable hardship amount
                Me.TextboxHardShipNet.Text = "0.00"
                Me.TextboxHardShip.Text = "0.00"
            End If
            DoFinalCalculation()
            'Changed by:Ganeswar for Hardship withdrawal
        Catch IcEx As InvalidCastException
            MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", "Invalid Tax Rate entered. Please enter the Valid Tax Rate.", MessageBoxButtons.Stop, False)
            Me.TDTaxRate = 10
            Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Function SetPropertiesAfterCreatePayees()
        Try
            objRefundProcess.inTaxable = Me.inTaxable
            objRefundProcess.inNonTaxable = Me.inNonTaxable
            objRefundProcess.inGross = Me.inGross
            objRefundProcess.inVolAcctsUsed = Me.inVolAcctsUsed
            objRefundProcess.inVolAcctsAvailable = Me.inVolAcctsAvailable

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Function
    'Commented code deleted by SG: 2012.03.16
    Private Function SetVolWithdrawalTotal()
        Dim l_ContributionDataTable As DataTable
        Try
            If Not Session("CalculatedDataTableForCurrentAccounts_Initial_C19") Is Nothing Then
                l_ContributionDataTable = CType(Session("CalculatedDataTableForCurrentAccounts_Initial_C19"), DataTable)
                l_ContributionDataTable = objRefundProcess.DoFlagSetForVoluntryRefund(l_ContributionDataTable, True)

                If Not objRefundProcess.VoluntryAmountForProcessing = Nothing Then
                    Me.VoluntaryWithdrawalTotal = objRefundProcess.VoluntryAmountForProcessing
                    Me.TextboxVoluntaryWithdrawalTotal.Text = objRefundProcess.VoluntryAmountForProcessing
                Else
                    Me.TextboxVoluntaryWithdrawalTotal.Text = "0.00"
                    Me.VoluntaryWithdrawalTotal = 0
                End If

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Function ValidateHardshipWithdrawal() As Boolean
        Dim isHardshipWithdrawalWithLoanAllowed As String
        Dim isHardshipAllowed As Boolean
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "ValidateHardshipWithdrawal START")  ' START | SR | 07/17/2019 | YRS-AT-4498 | Log for auditing purpose.

            isHardshipAllowed = False
            If Me.TextboxRequestAmount.Text <> "" Then
                ' START | SB | 07/29/2019 | YRS-AT-2169 | Check if participant is non participating person
                'Check if participant is non participating person, if yes no need to check loan criteria as they are not eligible for any loan. so do no validate any loan related process
                If (objRefundProcess.IsNonParticipatingPerson) Then
                    isHardshipAllowed = True
                    Return isHardshipAllowed
                End If
                ' END | SB| 07/29/2019 | YRS-AT-2169 | Check if participant is non participating person

                If Me.TextboxRequestAmount.Text > Me.TextboxVoluntaryWithdrawalTotal.Text Then
                    'START: SR | 07/17/2019 | YRS-AT-4498 | Commented existing code and added condition to check whether loan validation should happen based on key and also fetching loan status from database
                    ''If Session("HardshipAmountWithInterest") 
                    'Dim l_count As Integer
                    'l_count = 0

                    'l_count = YMCARET.YmcaBusinessObject.RefundRequest.GetLoanStatus(Me.PersonID)
                    ''l_count = 0 when there is not any paid loan exists for this participlant

                    'If l_count > 0 Then
                    '    Return True
                    '    'allow hardship

                    'Else
                    '    If Not Session("HardshipAmountWithInterest") Is Nothing Then
                    '        If Session("HardshipAmountWithInterest") >= 2000.0 Then
                    '            'not allow hardship
                    '            Return False
                    '        Else
                    '            Return True
                    '        End If
                    '    End If
                    'End If

                    'Check if hardship loan reuirement key is on, then do not validate loan status
                    If (objRefundProcess.IsLoanCriteriaNotRequired) Then
                        isHardshipAllowed = True
                    Else
                        'Get loan status based on which hardship flag option will be set
                        isHardshipAllowed = YMCARET.YmcaBusinessObject.RefundRequest.IsPreLoanRequiredForHardship(Me.PersonID, Me.HardshipAmount)
                        'If (Not (isHardshipWithdrawalWithLoanAllowed)) Then
                        '    isHardshipAllowed = False
                        'End If
                    End If
                    'END: SR | 07/17/2019 | YRS-AT-4498 | Commented existing code and added condition to check whether loan validation should happen based on key and also fetching loan status from database               
                Else
                    Return True
                End If
            End If
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", String.Format("isHardshipAllowed : {0}", isHardshipAllowed)) ' SR | 07/17/2019 | YRS-AT-4498 | Log isHardshipAllowed Value
            Return isHardshipAllowed
        Catch ex As Exception
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", String.Format("Error : {0}", ex.Message)) ' SR | 07/17/2019 | YRS-AT-4498 | Log Error for auditing
            Throw ex
            ' START | SR | 07/17/2019 | YRS-AT-4498 | Add finally block for logging.
        Finally
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "ValidateHardshipWithdrawal END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
        ' END | SR | 07/17/2019 | YRS-AT-4498 | Add finally block for logging.
    End Function

#End Region

#Region " HardShip RollOver"

    'Commented code deleted by SG: 2012.03.16

    'Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
    Private Sub CheckboxVoluntaryRollover_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckboxVoluntaryRollover.CheckedChanged
        Try
            If Me.CheckboxVoluntaryRollover.Checked Then
                'New Modification for Market Based Withdrawal Amit Nigam 05/Nov/2009        
                If ISMarket = -1 Then
                    EnableDisablePartialRolloverControl(False)  ''MKT added by dilip 
                End If
                'New Modification for Market Based Withdrawal Amit Nigam 05/Nov/2009        


                If Me.RefundType = "HARD" Then
                    Me.RefundType = "VOL"
                    Me.IsHardShip = False
                    'Added by Parveen on 09-Dec-2009 for the Hardship Issue
                    Me.ForceToVoluntaryWithdrawal = True
                    'Added by Parveen on 09-Dec-2009 for the Hardship Issue
                    'Commented code deleted by SG: 2012.03.16
                    DoRefundForRefundAccounts()
                    Me.DoFinalCalculation()
                    EnableDisableControlsForHardShipYes()
                    Label3.Visible = False
                    TextboxHardShipTaxRate.Visible = False
                    TextboxHardShipAmount.Visible = False
                    textboxHardShipNonTaxableAmount.Visible = False 'MMR | 2017.07.30 | YRS-AT-3870 | hiding control for non-taxable hardship amount
                    TextboxHardShip.Visible = False
                    TextboxHardShipNet.Visible = False
                    EnableDisableSaveButton()
                    'New Modification for Market Based Withdrawal Amit Nigam 05/Nov/2009        
                    trHardshipVoluntary.Visible = False
                    'New Modification for Market Based Withdrawal Amit Nigam 05/Nov/2009    

                End If
            Else
                If Me.RefundType = "VOL" And Me.IsHardShip = False Then
                    Me.IsHardShip = True
                    Me.RefundType = "HARD"
                    'Added by Parveen on 09-Dec-2009 for the Hardship Issue
                    Me.ForceToVoluntaryWithdrawal = False
                    'Added by Parveen on 09-Dec-2009 for the Hardship Issue
                    'Commented code deleted by SG: 2012.03.16
                    Me.DoFinalCalculation()
                    EnableDisableControlsForHardshipNo()
                    Label3.Visible = True
                    TextboxHardShipTaxRate.Visible = True
                    TextboxHardShipAmount.Visible = True
                    textboxHardShipNonTaxableAmount.Visible = True 'MMR | 2017.07.30 | YRS-AT-3870 | Unhiding control for non-taxable hardship amount
                    TextboxHardShip.Visible = True
                    TextboxHardShipNet.Visible = True
                    blnFlagHardShip = True
                    'New Modification for Market Based Withdrawal Amit Nigam 05/Nov/2009        
                    trHardshipVoluntary.Visible = True
                    'New Modification for Market Based Withdrawal Amit Nigam 05/Nov/2009    

                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub EnableDisableControlsForHardShipYes()
        'This Function will set the controls values when the user  click on the RadioButtonVoluntarysRolloverYes 
        Try
            If Me.RefundType = "HARD" Then
                TextboxTDAvailableAmount.Visible = False
                Label1.Visible = False
                TextboxRequestAmount.Visible = False
                Label2.Visible = False
                'Commented code deleted by SG: 2012.03.16
            End If
            TextboxHardShipTaxRate.Visible = False
            Label3.Visible = False
            TextboxHardShipAmount.Visible = False
            textboxHardShipNonTaxableAmount.Visible = False 'MMR | 2017.07.30 | YRS-AT-3870 | Hiding control for non-taxable hardship amount
            TextboxHardShip.Visible = False
            TextboxHardShipNet.Visible = False
            TextboxRequestAmount.Visible = False
            LabelVoluntaryAccounts.Visible = True

            Label5.Visible = False
            TextboxVoluntaryWithdrawalTotal.Visible = False
            Label1.Visible = False
            TextboxTDAvailableAmount.Visible = False
            Label2.Visible = False
            TextboxTDAvailableAmount.Visible = False
            TextboxRequestAmount.Visible = False
            TextboxVoluntaryWithdrawalTotal.Visible = True

            Label1.Visible = False
            Label2.Visible = False
            TextboxVoluntaryWithdrawalTotal.Visible = False
            Label5.Visible = False
            TextboxVoluntaryWithdrawalTotal.Visible = False
            Label1.Visible = False
            TextboxTDAvailableAmount.Visible = False
            Label2.Visible = False
            TextboxRequestAmount.Visible = False

            TextboxHardShipNet.Text = "0.00"
            TextboxVoluntaryWithdrawalTotal.Text = "0.00"
            TextboxRequestAmount.Text = "0.00"
            'Commented and Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
            'If RadioButtonRolloverYes.Checked = False Then
            If CheckBoxRollovers.Checked = False Then
                TextboxPayee2.Text = String.Empty
                TextboxPayee3.Text = String.Empty
                TextboxPayee2.rReadOnly = True 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
                TextboxPayee3.ReadOnly = True
            Else
                TextboxPayee2.rReadOnly = False 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
                TextboxPayee3.ReadOnly = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Sub EnableDisableControlsForHardshipNo()
        'This Function will set the controls values when the user  click on the RadioButtonVoluntarysRolloverNO 
        Try
            If Me.RefundType = "HARD" Then
                TextboxTDAvailableAmount.Visible = True
                Label1.Visible = True
                TextboxRequestAmount.Visible = True
                Label2.Visible = True
                'Commented code deleted by SG: 2012.03.16
            End If
            Label5.Visible = True
            TextboxVoluntaryWithdrawalTotal.Visible = True
            Label1.Visible = True
            TextboxTDAvailableAmount.Visible = True
            Label2.Visible = True
            TextboxRequestAmount.Visible = True
            LabelVoluntaryAccounts.Visible = True

            TextboxHardShipTaxRate.Visible = False
            Label3.Visible = False
            TextboxHardShipAmount.Visible = False
            textboxHardShipNonTaxableAmount.Visible = False 'MMR | 2017.07.30 | YRS-AT-3870 | Hiding control for non-taxable hardship amount
            TextboxHardShip.Visible = False
            TextboxHardShipNet.Visible = False


            TextboxVoluntaryWithdrawalTotal.Visible = True
            Label5.Visible = True

            TextboxHardShipNet.Text = "0.00"
            TextboxTDAvailableAmount.Text = "0.00"
            TextboxHardShipTaxRate.Text = "10"
            TextboxHardShipAmount.Text = "0.00"
            textboxHardShipNonTaxableAmount.Text = "0.00" 'MMR | 2017.07.30 | YRS-AT-3870 | Initializing control for non-taxable hardship amount
            TextboxHardShip.Text = "0.00"
            TextboxHardShipNet.Text = "0.00"
            TextboxRequestAmount.Text = "0.00"
            TextboxDeductions.Text = "0.00"

            TextboxVoluntaryWithdrawalTotal.Text = Me.VoluntaryWithdrawalTotal
            Label5.Visible = True
            TextboxVoluntaryWithdrawalTotal.Visible = True
            Label1.Visible = True
            TextboxTDAvailableAmount.Visible = True
            Label2.Visible = True
            TextboxRequestAmount.Visible = True
            TextboxTDAvailableAmount.Text = Math.Round(Me.HardshipAvailable, 2)
            'Commented and Added by Parveen on 03-Nov-2009 to Replace Yes/No RadioButtons with the CheckBox
            'If RadioButtonRolloverYes.Checked = False Then
            If CheckBoxRollovers.Checked = False Then
                TextboxPayee2.Text = String.Empty
                TextboxPayee3.Text = String.Empty
                TextboxPayee2.rReadOnly = True 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
                TextboxPayee3.ReadOnly = True
            Else
                TextboxPayee2.rReadOnly = False 'SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name.
                TextboxPayee3.ReadOnly = False
            End If
            If TextboxRequestAmount.Text = "0.00" Then
                ButtonSave.Visible = False
            End If
            If TextboxRequestAmount.Text = "0" Then
                ButtonSave.Visible = False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'End new event by Ganeswar For HardShip RollOver on 21-05-2009
#End Region

    Private Sub TextboxHardShip_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxHardShip.TextChanged
        Try
            'Dim l_RequestedAmount As Decimal
            If TextboxHardShip.Text <> String.Empty Then
                If TextboxHardShip.Text = "." Then
                    TextboxHardShip.Text = "0.00"
                End If
                If CDec(TextboxHardShip.Text) >= CDec(TextboxRequestAmount.Text) Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Tax Withholding Amount Should be less than TD Requested Amount.", MessageBoxButtons.Stop, False)
                    TextboxHardShip.Text = "0.00"
                    Me.DoFinalCalculation()
                    l_bool_HardShipAmount = False
                    Me.HardShipTaxRate = 10.0
                    TextboxHardShipNet.Text = TextboxRequestAmount.Text

                    Exit Sub
                Else
                    TextboxHardShipTaxRate.Text = Math.Round((TextboxHardShip.Text / TextboxRequestAmount.Text * 100), 2)
                    ' START | SR | 2019.07.31 | YRS-AT-3870 | Net amount should be calculated by reducing tax from taxable component. Not on complete hardship amount.
                    'TextboxHardShipNet.Text = Math.Round((TextboxHardShipAmount.Text - TextboxHardShip.Text), 2)
                    TextboxHardShipNet.Text = Math.Round((TextboxHardShipAmount.Text - TextboxHardShip.Text) + textboxHardShipNonTaxableAmount.Text, 2)
                    ' END | SR | 2019.07.31 | YRS-AT-3870 | Net amount should be calculated by reducing tax from taxable component. Not on complete hardship amount.
                    Me.DoFinalCalculation()
                    'l_RequestedAmount = CDec(Me.TextboxNetFinal.Text)
                    Me.IsHardShip = True
                    ' START | SR | 2019.07.31 | YRS-AT-3870 | Net amount should be calculated by reducing tax from taxable component. Not on complete hardship amount.
                    'Added by Ganeswar for HardShipAmount on 10th july 2009.
                    'Me.HardShipTaxRate = Math.Round((TextboxHardShip.Text / TextboxRequestAmount.Text * 100), 14)
                    Me.HardShipTaxRate = Math.Round((TextboxHardShip.Text / TextboxHardShipAmount.Text * 100), 14)
                    ' END | SR | 2019.07.31 | YRS-AT-3870 | Net amount should be calculated by reducing tax from taxable component. Not on complete hardship amount.
                    'Added by Ganeswar for HardShipAmount on 10th july 2009.

                    l_bool_HardShipAmount = True
                    Me.l_dec_TDRequestedAmount = CDec(TextboxRequestAmount.Text)
                    Me.l_dec_TDAmount = CDec(TextboxHardShip.Text)
                    Me.EnableDisableSaveButton()
                End If
            Else
                TextboxHardShip.Text = "0.00"
                If TextboxHardShip.Text = "." Then
                    TextboxHardShip.Text = "0.00"
                End If
            End If
            'Me.l_dec_TDAmount = CDec(TextboxHardShip.Text)
            'Me.l_dec_TDRequestedAmount = CDec(TextboxRequestAmount.Text)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function DisableRadioButtons()
        Try
            'Commented code deleted by SG: 2012.03.16
            LabelReleaseSigned.Visible = False
            RadioButtonReleaseSignedYes.Visible = False
            RadioButtonReleaseSignedNo.Visible = False
            'AA:24.09.2013 : BT-1501: Commented below line because it is commented in aspx page
            'LabelAddressUpdating.Visible = False
            RadioButtonAddressUpdatingYes.Visible = False
            RadioButtonAddressUpdatingNo.Visible = False
            LabelNotarized.Visible = False
            RadioButtonNotarizedYes.Visible = False
            RadioButtonNotarizedNo.Visible = False
            LabelWaiver.Visible = False
            RadioButtonWaiverYes.Visible = False
            RadioButtonWaiverNo.Visible = False

            LabelPersonalMonies.Visible = False
            RadiobuttonPersonalMoniesYes.Visible = False
            RadiobuttonPersonalMoniesNo.Visible = False
        Catch ex As Exception

        End Try
    End Function

    'Commented code deleted by SG: 2012.03.16

    Private Sub TextboxTotalRolloverAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxTotalRolloverAmount.TextChanged
        Dim l_string_dollar As String
        l_string_dollar = "$"


        If Me.RadioButtonNone.Checked = True Then
            'Apply Partial amount percentage
            If TextboxTotalRolloverAmount.Text = String.Empty Or TextboxTotalRolloverAmount.Text = ".00" Then
                TextboxTotalRolloverAmount.Text = String.Empty
                Me.NumRequestedAmountPartialRollOver = 0.0
                Me.NumPercentageFactorofMoneyPartialRollOver = 1.0
            Else
                If Not IsNumeric(TextboxTotalRolloverAmount.Text) Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Please enter valid amount.", MessageBoxButtons.Stop)
                    TextboxTotalRolloverAmount.Text = String.Empty
                    Me.NumRequestedAmountPartialRollOver = 0.0
                    Me.NumPercentageFactorofMoneyPartialRollOver = 1.0
                ElseIf TextboxTotalRolloverAmount.Text = "." Or TextboxTotalRolloverAmount.Text < 0 Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Please enter valid amount.", MessageBoxButtons.Stop)
                    TextboxTotalRolloverAmount.Text = String.Empty
                    Me.NumRequestedAmountPartialRollOver = 0.0
                    Me.NumPercentageFactorofMoneyPartialRollOver = 1.0
                ElseIf TextboxTotalRolloverAmount.Text = 0.0 Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Partial Amount should be greater than zero.", MessageBoxButtons.Stop)
                    TextboxTotalRolloverAmount.Text = String.Empty
                    Me.NumRequestedAmountPartialRollOver = 0.0
                    Me.NumPercentageFactorofMoneyPartialRollOver = 1.0
                    'Modified by Amit Nigam to check the validation for the total rollover amount which should be more then the total withdrawal amount 11/11/2009
                    'ElseIf TextboxTotalRolloverAmount.Text >= Me.NumTotalWithdrawalAmount Then
                ElseIf CType(TextboxTotalRolloverAmount.Text, Decimal) > CType(Session("DefferredInstallmentAmount_C19"), Decimal) Then
                    'Modified by Amit Nigam to check the validation for the total rollover amount which should be more then the total withdrawal amount 11/11/2009
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Rollover Request Amount should Not be greater than  " & l_string_dollar & "" & Session("DefferredInstallmentAmount_C19") & ". ", MessageBoxButtons.Stop)
                    TextboxTotalRolloverAmount.Text = String.Empty
                    Me.NumRequestedAmountPartialRollOver = 0.0
                    Me.NumPercentageFactorofMoneyPartialRollOver = 1.0

                ElseIf TextboxTotalRolloverAmount.Text > 0 Then

                    Me.NumRequestedAmountPartialRollOver = TextboxTotalRolloverAmount.Text
                    TextboxTotalRolloverAmount.Text = Me.NumRequestedAmountPartialRollOver.ToString("0.00")
                    'Modified to add the two decimal places in the testboxes-Amit Nigam 02-11-2009
                    'Commented code deleted by SG: 2012.03.16
                End If
            End If
        End If
    End Sub

    Private Sub RadiobuttonRolloverAllInstallment_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadiobuttonRolloverAllInstallment.CheckedChanged
        Try
            Dim l_DefferedPayment As Decimal
            l_DefferedPayment = objRefundProcess.GetTransactionofDefferedPayment(Me.FundID)
            TextboxRemainingMoneyinMKT.Text = CType(Session("DefferredPaymentAmount_C19"), Decimal)
            TextboxDeferedInstallmentAmount.Text = CType(Session("DefferredInstallmentAmount_C19"), Decimal)
            EnableDisablePartialRolloverControl(True)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RadiobuttonRolloverOnlyFirstInstallment_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadiobuttonRolloverOnlyFirstInstallment.CheckedChanged
        Try
            EnableDisablePartialRolloverControl(False)
        Catch ex As Exception

        End Try
    End Sub
    'Added By Parveen To Set RolloverOptions and Rollover Amount on 15 Dec 2009
    Private Sub SetRolloverOptionAndAmount()
        Dim l_Payee2DataTable As DataTable
        Dim l_intNoOfInstallments As Integer
        Try
            If RadioButtonNone.Checked Then
                If RadiobuttonRolloverAllInstallment.Checked Then
                    Me.RolloverOptions = m_const_PartialRollOver
                    Me.FirstRolloverAmt = Decimal.Parse(TextboxNet2.Text)
                    l_intNoOfInstallments = (100 - Me.FirstInstallment) / Me.DefferredInstallment
                    Me.TotalRolloverAmt = Me.FirstRolloverAmt + Decimal.Parse(TextboxTotalRolloverAmount.Text) * l_intNoOfInstallments
                Else
                    Me.RolloverOptions = m_const_RolloverNone
                End If
            ElseIf RadioButtonRolloverAll.Checked Then
                If RadiobuttonRolloverAllInstallment.Checked Then
                    Me.RolloverOptions = m_const_RolloverAll
                Else
                    Me.RolloverOptions = m_const_RolloverNone
                End If
            ElseIf RadioButtonTaxableOnly.Checked Then
                If RadiobuttonRolloverAllInstallment.Checked Then
                    Me.FirstRolloverAmt = Decimal.Parse(TextboxNet2.Text)
                    Me.RolloverOptions = m_const_AllTaxable
                Else
                    Me.RolloverOptions = m_const_RolloverNone
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    'Added By Parveen To Set RolloverOptions and Rollover Amount on 15 Dec 2009
    'BT:830-YRS 5.0-1326 : Rollover including non-taxable money.
    Private Sub TextboxRolloverAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxRolloverAmount.TextChanged
        Dim l_string_dollar As String
        l_string_dollar = "$"
        Dim l_Payee1DataTable As DataTable
        Dim l_Total As Decimal
        Dim l_SumTaxable As Decimal
        Dim l_SumNonTaxable As Decimal

        Try
            If Me.RadioButtonSpecificAmount.Checked = True Then
                TextboxRolloverAmount.Enabled = True
                'START | SR | 2016.01.22 | YRS-AT-2664 | If taxable RMD amount allowed to be rollover then reset RMD values
                If (ViewState("AllowRMDAmountToBeRollover") = "True") Then
                    If (ViewState("IsRolloverTaxableWasSelected") = "True") Then
                        Session("MinimumDistributionTable_C19") = Session("MinimumDistributionTable_temp_C19")
                        ViewState("IsRolloverTaxableWasSelected") = "False"
                        EnableMinimumDistributionControls()
                    End If
                End If
                'END | SR | 2016.01.22 | YRS-AT-2664 | If taxable RMD amount allowed to be rollover then reset RMD values
                If Me.RefundType = "HARD" And TextboxVoluntaryWithdrawalTotal.Text = "0.00" Then
                    If chkCreatePayeeProcessed = False Then
                        objRefundProcess.CreatePayees()
                        Me.SetPropertiesAfterCreatePayees()
                    End If
                Else
                    objRefundProcess.CreatePayees()
                End If


                l_Payee1DataTable = DirectCast(Session("Payee1DataTable_C19"), DataTable)
                If Not l_Payee1DataTable Is Nothing Then
                    l_Total = IIf(l_Payee1DataTable.Compute("SUM(Taxable)", "Taxable>0").GetType.ToString() = "System.DBNull", 0, l_Payee1DataTable.Compute("SUM(Taxable)", "Taxable>0")) + IIf(l_Payee1DataTable.Compute("SUM(NonTaxable)", "NonTaxable>0").GetType.ToString() = "System.DBNull", 0, l_Payee1DataTable.Compute("SUM(NonTaxable)", "NonTaxable>0"))
                End If

                If TextboxRolloverAmount.Text = String.Empty Or TextboxRolloverAmount.Text = ".00" Then
                    TextboxRolloverAmount.Text = String.Empty
                    Me.NumRequestedAmountPartialRollOver = 0.0
                    Me.NumPercentageFactorofMoneyPartialRollOver = 1.0
                    Me.DoNone()
                Else
                    If Not IsNumeric(TextboxRolloverAmount.Text) Then
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Please enter valid amount.", MessageBoxButtons.Stop)
                        TextboxTotalRolloverAmount.Text = String.Empty
                        Me.NumRequestedAmountPartialRollOver = 0.0
                        Me.NumPercentageFactorofMoneyPartialRollOver = 1.0
                    ElseIf TextboxRolloverAmount.Text = "." Or TextboxRolloverAmount.Text < 0 Then
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Please enter valid amount.", MessageBoxButtons.Stop)
                        TextboxRolloverAmount.Text = String.Empty
                        Me.NumRequestedAmountPartialRollOver = 0.0
                        Me.NumPercentageFactorofMoneyPartialRollOver = 1.0
                    ElseIf TextboxRolloverAmount.Text = 0.0 Then
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Rollover Request Amount should be greater than zero.", MessageBoxButtons.Stop)
                        TextboxRolloverAmount.Text = String.Empty
                        Me.NumRequestedAmountPartialRollOver = 0.0
                        Me.NumPercentageFactorofMoneyPartialRollOver = 1.0
                    ElseIf CType(TextboxRolloverAmount.Text, Decimal) > l_Total Then
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA-YRS", "Rollover Request Amount should Not be greater than  " & l_string_dollar & "" & l_Total & ". ", MessageBoxButtons.Stop)
                        TextboxRolloverAmount.Text = String.Empty
                        Me.NumRequestedAmountPartialRollOver = 0.0
                        Me.NumPercentageFactorofMoneyPartialRollOver = 1.0
                    ElseIf TextboxRolloverAmount.Text > 0 Then
                        Me.NumRequestedAmountPartialRollOver = TextboxRolloverAmount.Text
                        TextboxTotalRolloverAmount.Text = Me.NumRequestedAmountPartialRollOver.ToString("0.00")
                        Me.DoSpecificAmount()
                    End If
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Rollover Amount", ex)
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try

    End Sub
    Private Function DoSpecificAmount()

        Dim l_Payee1DataTable As DataTable
        Dim l_Payee2DataTable As DataTable
        Dim l_Payee1TempDataTable As DataTable
        Dim l_Payee2DataRow As DataRow
        Dim l_adjustedDataRow As DataRow

        Dim l_SumTaxable As Decimal
        Dim l_SumNonTaxable As Decimal
        Dim l_SumAccountBalance As Decimal
        Dim l_RequestedAccount As Decimal
        Dim l_NumPercentageFactorofMoneyRollOver As Double
        Dim roundingValue As Decimal
        Dim l_runningTotal As Decimal
        'Start: 2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only
        Dim l_SumDiff As Decimal
        Dim l_SumNonTaxableAccoutable As Decimal
        Dim l_RequestedNonTaxable As Decimal
        Dim l_NumNonPercentageFactorofMoneyRollOver As Decimal
        'End: 2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only
        Try

            l_Payee1DataTable = DirectCast(Session("Payee1DataTable_C19"), DataTable)
            l_Payee2DataTable = CType(Session("Payee2DataTable_C19"), DataTable)

            If Not l_Payee1DataTable Is Nothing Then
                l_SumTaxable = IIf(l_Payee1DataTable.Compute("SUM(Taxable)", "Taxable>0").GetType.ToString() = "System.DBNull", 0, l_Payee1DataTable.Compute("SUM(Taxable)", "Taxable>0")) 'l_Payee1DataTable.Compute("SUM(Taxable)", "Taxable>0")
                l_SumNonTaxable = IIf(l_Payee1DataTable.Compute("SUM(NonTaxable)", "NonTaxable>0").GetType.ToString() = "System.DBNull", 0, l_Payee1DataTable.Compute("SUM(NonTaxable)", "NonTaxable>0")) 'l_Payee1DataTable.Compute("SUM(NonTaxable)", "NonTaxable>0")

                'Start: 2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only
                'l_NumPercentageFactorofMoneyRollOver = (l_SumTaxable + l_SumNonTaxable) / NumRequestedAmountPartialRollOver
                If NumRequestedAmountPartialRollOver <= l_SumTaxable Then
                    l_NumPercentageFactorofMoneyRollOver = l_SumTaxable / NumRequestedAmountPartialRollOver
                Else
                    Dim decRemainingRequestedAmt As Decimal
                    decRemainingRequestedAmt = NumRequestedAmountPartialRollOver - l_SumTaxable
                    l_NumPercentageFactorofMoneyRollOver = l_SumTaxable / (NumRequestedAmountPartialRollOver - decRemainingRequestedAmt)
                    l_SumDiff = decRemainingRequestedAmt
                    l_NumNonPercentageFactorofMoneyRollOver = l_SumNonTaxable / l_SumDiff
                End If
                'End: 2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only

                l_Payee1TempDataTable = l_Payee1DataTable.Clone
                If l_Payee2DataTable Is Nothing Then
                    l_Payee2DataTable = l_Payee1DataTable.Clone
                End If
                l_Payee2DataTable.Rows.Clear()
                For i As Int16 = 0 To l_Payee1DataTable.Rows.Count - 1
                    Dim l_DataRow As DataRow = l_Payee1DataTable.Rows(i)
                    l_Payee1TempDataTable.ImportRow(l_DataRow)
                    l_Payee2DataRow = l_Payee2DataTable.NewRow

                    'Start: 2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only
                    'l_SumAccountBalance = IIf(l_DataRow.IsNull("Taxable"), 0, l_DataRow("Taxable")) + IIf(l_DataRow.IsNull("NonTaxable"), 0, l_DataRow("NonTaxable"))
                    If NumRequestedAmountPartialRollOver <= l_SumTaxable Then
                        l_SumAccountBalance = IIf(l_DataRow.IsNull("Taxable"), 0, l_DataRow("Taxable"))
                    Else
                        l_SumAccountBalance = IIf(l_DataRow.IsNull("Taxable"), 0, l_DataRow("Taxable"))
                        l_SumNonTaxableAccoutable = IIf(l_DataRow.IsNull("NonTaxable"), 0, l_DataRow("NonTaxable"))
                    End If

                    If l_SumNonTaxableAccoutable > 0 Then
                        'Start-2014.11.14/SR/BT:2672/YRS 5.0-2422:Math.Round fumction removed
                        'l_RequestedNonTaxable = Math.Round(l_SumNonTaxableAccoutable / l_NumNonPercentageFactorofMoneyRollOver, 2) 
                        l_RequestedNonTaxable = l_SumNonTaxableAccoutable / l_NumNonPercentageFactorofMoneyRollOver
                        'End-2014.11.14/SR/BT:2672/YRS 5.0-2422:Math.Round fumction removed
                    Else
                        l_RequestedNonTaxable = 0 '2014.10.14/SR/BT:2672/YRS 5.0-2422:allow rollovoers of pre-tax money only
                    End If
                    'End: 2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only

                    l_RequestedAccount = Math.Round(l_SumAccountBalance / l_NumPercentageFactorofMoneyRollOver, 2)
                    If (i = l_Payee1DataTable.Rows.Count - 1) Then
                        l_RequestedAccount = Math.Round(NumRequestedAmountPartialRollOver - (l_runningTotal + l_RequestedNonTaxable), 2) '2014.11.11/SR/BT:2672/YRS 5.0-2422:allow rollovoers of pre-tax money only
                    End If
                    'If (i = l_Payee1DataTable.Rows.Count - 1) Then
                    '    l_RequestedAccount = l_SumTaxable - l_runningTotal
                    'End If
                    l_runningTotal += l_RequestedAccount
                    l_Payee2DataRow("AcctType") = l_DataRow("AcctType")

                    'Start: 2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only
                    'l_Payee2DataRow("NonTaxable") = IIf(Convert.ToDecimal(l_DataRow("NonTaxable")) = 0, "0.00", Math.Round(l_DataRow("NonTaxable") / l_NumPercentageFactorofMoneyRollOver, 2))
                    'l_Payee2DataRow("Taxable") = l_RequestedAccount - Convert.ToDecimal(l_Payee2DataRow("NonTaxable"))
                    If NumRequestedAmountPartialRollOver <= l_SumTaxable Then
                        l_Payee2DataRow("Taxable") = l_RequestedAccount
                        l_DataRow("Taxable") = Convert.ToDecimal(l_DataRow("Taxable")) - Convert.ToDecimal(l_Payee2DataRow("Taxable"))
                        l_Payee2DataRow("NonTaxable") = "0.00"
                    Else
                        l_Payee2DataRow("Taxable") = l_RequestedAccount
                        l_Payee2DataRow("NonTaxable") = Math.Round(l_RequestedNonTaxable, 2)  '2014.11.14/SR/BT:2672/YRS 5.0-2422: Math.Round fumction added
                        l_DataRow("Taxable") = "0.00"
                        l_DataRow("NonTaxable") = Math.Round(IIf(Convert.ToDecimal(l_DataRow("NonTaxable")) = 0, "0.00", Math.Round(l_DataRow("NonTaxable"), 2)) - l_RequestedNonTaxable, 2)  'Start-2014.11.14/SR/BT:2672/YRS 5.0-2422: Math.Round fumction added
                        l_runningTotal += l_RequestedNonTaxable '2014.10.14/SR/BT:2672/YRS 5.0-2422:allow rollovoers of pre-tax money only
                        l_RequestedNonTaxable = 0.0
                    End If
                    'End: 2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only

                    l_Payee2DataRow("TaxRate") = "0.00"
                    l_Payee2DataRow("Tax") = "0.00"
                    l_Payee2DataRow("Payee") = l_DataRow("Payee")
                    l_Payee2DataRow("FundedDate") = l_DataRow("FundedDate")
                    l_Payee2DataRow("RequestType") = l_DataRow("RequestType")
                    l_Payee2DataRow("RefRequestsID") = l_DataRow("RefRequestsID")
                    'Start: 2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only
                    'l_DataRow("Taxable") = Convert.ToDecimal(l_DataRow("Taxable")) - Convert.ToDecimal(l_Payee2DataRow("Taxable"))
                    'l_DataRow("NonTaxable") = Convert.ToDecimal(l_DataRow("NonTaxable")) - Convert.ToDecimal(l_Payee2DataRow("NonTaxable"))
                    'End: 2014.10.07         Dinesh k    BT:2672:YRS 5.0-2422:allow rollovoers of pre-tax money only
                    l_Payee2DataTable.Rows.Add(l_Payee2DataRow)
                Next
            End If
            'Start-2014.11.14/SR/BT:2672/YRS 5.0-2422: Handle rounding issue
            If NumRequestedAmountPartialRollOver > l_SumTaxable Then
                AdjustRoundingValue(l_Payee1DataTable, l_Payee2DataTable)
            End If
            'End-2014.11.14/SR/BT:2672/YRS 5.0-2422: Handle rounding issue
            Session("Payee1DataTable_C19") = l_Payee1DataTable
            Session("Payee2DataTable_C19") = l_Payee2DataTable
            Session("Payee1TempDataTable_C19") = l_Payee1TempDataTable
            Me.LoadPayeesDataGrid()
            Me.LoadPayee1ValuesIntoControls()
            Me.LoadPayee2ValuesIntoControls()
            'Me.LoadPayee3ValuesIntoControls()
            Me.DoFinalCalculation()
            Me.EnableDisableSaveButton()
            TextboxRolloverAmount.Enabled = True

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    'Start:Anudeep:21.02.2013  check whether the person country name is non-us/ca :YRS 5.0-1654:Email to finance dept when withdrawal to foreign address
    Public Sub CheckCountryName()
        Dim l_dataset As DataSet
        Dim l_string_countryname As String
        Dim l_string_fundno As String
        Try
            'Anudeep:10.07.2013 :YRS 5.0-1745:Capture Beneficiary addresses 
            If Not Session("ds_PrimaryAddress_C19") Is Nothing Then
                l_dataset = Session("ds_PrimaryAddress_C19")
                If l_dataset.Tables("Address").Rows.Count > 0 Then
                    l_string_countryname = l_dataset.Tables("Address").Rows(0)("country").ToString()
                End If
            End If

            If Not Session("PersonInformation_C19") Is Nothing Then
                l_dataset = Session("PersonInformation_C19")

                If l_string_countryname Is Nothing AndAlso l_dataset.Tables("Member Address").Rows.Count > 0 Then
                    l_string_countryname = l_dataset.Tables("Member Address").Rows(0)("Country").ToString()
                End If

                If l_dataset.Tables("Member Details").Rows.Count > 0 Then
                    l_string_fundno = l_dataset.Tables("Member Details").Rows(0)("Fund No").ToString()
                    If l_string_countryname <> "US" And l_string_countryname <> "CA" Then
                        SendMail(l_string_fundno)
                    End If
                End If

            End If

        Catch ex As Exception
            HelperFunctions.LogException("Withdrawal Request - Intimation mail could not be sent informing Finance Dept. about withdrawal involving non-US/Canadian address for Fund Id : " + l_string_fundno, ex)
        End Try
    End Sub
    'Sending a email to finance department when a non us/ca person requsted withdrawal
    Public Function SendMail(ByRef Fundno As String)
        Try
            Dim obj As MailUtil
            obj = New MailUtil
            obj.MailCategory = "REFUNDEMAIL"
            If obj.MailService = False Then Exit Function
            obj.MailMessage = "A withdrawal is in process for fund id " + Fundno + ". The address for this person is neither US nor Canadian."
            obj.Subject = "Withdrawal involving non-US/Canadian address."
            obj.MailFormatType = Mail.MailFormat.Text
            obj.Send()
        Catch
            Throw
        End Try
    End Function
    'End:Anudeep:21.02.2013 check whether the person country name is non-us/ca:YRS 5.0-1654:Email to finance dept when withdrawal to foreign address

    'Function added by Dinesh Kanojia on 17/10/2013 to read message from resource file for gemini 2165
    Public Function getmessage(ByVal resourcemessage As String)
        Try

            Dim strMessage As String
            Try
                strMessage = GetGlobalResourceObject("RegenerateRMDMessage", resourcemessage).ToString()
            Catch ex As Exception
                strMessage = resourcemessage
            End Try
            Return strMessage
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Start:SR:2014.09.17-BT 2633/YRS 5.0-2403 -  Remove expiration dates from withdrawal requests in YRS 
    Public Sub CreateExpiredRequestWarning()
        Dim dtRefundRequestDataTable As DataTable
        Dim drRefunRequestDataRow As DataRow
        Dim strQuery As String
        Dim drFoundRow() As DataRow
        Try
            dtRefundRequestDataTable = DirectCast(Session("RefundRequestTable_C19"), DataTable)
            If Not dtRefundRequestDataTable Is Nothing Then
                strQuery = "UniqueID ='" & Me.SessionRefundRequestID.Trim & "'"
                drFoundRow = dtRefundRequestDataTable.Select(strQuery)
                drRefunRequestDataRow = Nothing
                If Not drFoundRow Is Nothing Then
                    If drFoundRow.Length > 0 Then
                        drRefunRequestDataRow = drFoundRow(0)
                    End If
                End If

                If Not drRefunRequestDataRow Is Nothing Then
                    If Not String.IsNullOrEmpty(drRefunRequestDataRow("ExpireDate").ToString) Then
                        If Date.Parse(drRefunRequestDataRow("ExpireDate").ToString()) < Today Then
                            DivMainMessage.InnerHtml = GetGlobalResourceObject("Withdrawal", "MESSAGE_WD_WARNING_EXPIRYREQUEST").ToString()
                        Else
                            DivMainMessage.Visible = False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    'End:SR:2014.09.17-BT 2633/YRS 5.0-2403 -  Remove expiration dates from withdrawal requests in YRS 

    'Start-2014.11.14/SR/BT:2672/YRS 5.0-2422: Handle rounding issue
    Public Sub AdjustRoundingValue(ByVal l_Payee1DataTable As DataTable, ByVal l_Payee2DataTable As DataTable)
        Dim decPayee2TaxableSum As Decimal
        Dim decPayee2NonTaxableSum As Decimal
        Dim decPayee1TaxableSum As Decimal
        Dim decPayee1NonTaxableSum As Decimal
        Dim decPayee1Total As Decimal
        Dim decPayee2Total As Decimal
        Dim decRoundingVal As Decimal
        Dim cnt As Integer
        Dim strAccType As String
        Dim drFilter As DataRow
        Try
            decPayee2TaxableSum = IIf(l_Payee2DataTable.Compute("SUM(Taxable)", "Taxable>0").GetType.ToString() = "System.DBNull", 0, l_Payee2DataTable.Compute("SUM(Taxable)", "Taxable>0"))
            decPayee2NonTaxableSum = IIf(l_Payee2DataTable.Compute("SUM(NonTaxable)", "NonTaxable>0").GetType.ToString() = "System.DBNull", 0, l_Payee2DataTable.Compute("SUM(NonTaxable)", "NonTaxable>0"))
            decPayee2Total = decPayee2TaxableSum + decPayee2NonTaxableSum

            If (NumRequestedAmountPartialRollOver <> (decPayee2Total)) Then
                decRoundingVal = NumRequestedAmountPartialRollOver - decPayee2Total
                If decRoundingVal <> 0 Then
                    For cnt = 0 To l_Payee1DataTable.Rows.Count - 1
                        If (l_Payee1DataTable.Rows(cnt).Item("NonTaxable") > 0) Then
                            l_Payee1DataTable.Rows(cnt).Item("NonTaxable") = l_Payee1DataTable.Rows(cnt).Item("NonTaxable") - decRoundingVal
                            strAccType = l_Payee2DataTable.Rows(cnt).Item("AcctType")
                            Exit For
                        End If
                    Next
                    For cnt = 0 To l_Payee2DataTable.Rows.Count - 1
                        If (l_Payee2DataTable.Rows(cnt).Item("AcctType").ToString.Trim = strAccType.Trim) Then
                            l_Payee2DataTable.Rows(cnt).Item("NonTaxable") = l_Payee2DataTable.Rows(cnt).Item("NonTaxable") + decRoundingVal
                            Exit For
                        End If
                    Next
                End If
            End If
        Catch
            Throw
        End Try
    End Sub
    'Start-2014.11.14/SR/BT:2672/YRS 5.0-2422: Handle rounding issue
    'Start:AA:11.17.2015 YRS-AT-2639 Added below code to retreive ref request options lik erollover ,express fee and additinal withholding
#Region "WithdarwalPhase2"
    Private Sub GetWithdrawalOptionsVer2()
        Dim dtWithdawalOptions As DataTable
        Dim strRolloverOption As String
        Dim decRolloverAmount As Decimal = 0
        Dim decExpressFee As Decimal = 0
        Dim decAddlnWithholding As Decimal = 0
        Dim chkOvernightDeleivery As CheckBox
        Dim e As New EventArgs
        Try
            IRSOverride = False 'SR | 2016.09.23 | YRS-AT-3164 | Set default value to false
            dtWithdawalOptions = YMCARET.YmcaBusinessObject.RefundRequest.GetRefRequestOptionsVer2(SessionRefundRequestID)
            If HelperFunctions.isNonEmpty(dtWithdawalOptions) Then
                ' SR | 2016.06.08 | YRS-at-2962 | Get Withdrawal processing Fee from atsRefrequestoptions table
                WithdrawalProcessingFee = dtWithdawalOptions.Rows(0)("numProcessingFees").ToString()
                txtProcessingFee.Text = WithdrawalProcessingFee
                IRSOverride = Convert.ToBoolean(IIf(String.IsNullOrEmpty(dtWithdawalOptions.Rows(0)("bitIRSOverride").ToString()), 0, dtWithdawalOptions.Rows(0)("bitIRSOverride"))) 'SR | 2016.09.23 | YRS-AT-3164 | Get IRSOverride Flag for special Hardship for Louisiana flood victim 
                If (dtWithdawalOptions.Select("chvFormFormat='EFORM'").Length > 0) Then
                    ' SR | 2016.06.08 | YRS-at-2962 | Get Withdrawal processing Fee from atsRefrequestoptions table
                    strRolloverOption = dtWithdawalOptions.Rows(0)("chvRolloverOption").ToString()
                    strRolloverOption = strRolloverOption.Trim.ToUpper()
                    'Start: AA:12.02.2015 YRS-AT-2639 Handled DBnull - if express mail or additional withholding value is DBnull then sets to 0
                    'decRolloverAmount = dtWithdawalOptions.Rows(0)("numRolloverAmount")
                    'decAddlnWithholding = dtWithdawalOptions.Rows(0)("numAdditionalWithholdingPercentage")
                    'decExpressFee = dtWithdawalOptions.Rows(0)("numExpressMail")
                    decRolloverAmount = IIf(String.IsNullOrEmpty(dtWithdawalOptions.Rows(0)("numRolloverAmount").ToString()), 0, dtWithdawalOptions.Rows(0)("numRolloverAmount"))
                    decAddlnWithholding = IIf(String.IsNullOrEmpty(dtWithdawalOptions.Rows(0)("numAdditionalWithholdingPercentage").ToString()), 0, dtWithdawalOptions.Rows(0)("numAdditionalWithholdingPercentage"))
                    decExpressFee = IIf(String.IsNullOrEmpty(dtWithdawalOptions.Rows(0)("numExpressMail").ToString()), 0, dtWithdawalOptions.Rows(0)("numExpressMail"))
                    'End: AA:12.02.2015 YRS-AT-2639 Handled DBnull - if express mail or additional withholding value is DBnull then sets to 0
                    'Check rollover option if exists fill rollover details
                    If Not String.IsNullOrEmpty(strRolloverOption) AndAlso strRolloverOption <> enumRolloverOptions.None.ToString().Trim.ToUpper() Then
                        CheckBoxRollovers.Checked = True
                        CheckBoxRollovers_CheckedChanged(Me, e)

                        'if partial rollover option has been opted then fill the partial rollover amount and partial rollover radio button to b checked and call text change event
                        If strRolloverOption = enumRolloverOptions.Partial.ToString().Trim.ToUpper Then
                            RadioButtonSpecificAmount.Checked = True
                            TextboxRolloverAmount.Text = decRolloverAmount.ToString()
                            TextboxRolloverAmount_TextChanged(Me, e)

                            'if full rollover option has been opted then radio button full will be checked and radio button check change event will be called
                        ElseIf strRolloverOption = enumRolloverOptions.Full.ToString().Trim.ToUpper Then
                            RadioButtonRolloverAll.Checked = True
                            RadioButtonRolloverAll_CheckedChanged(Me, e)

                            'if taxable rollover option has been opted then radio button taxable will be checked and radio button check change event will be called
                        ElseIf strRolloverOption = enumRolloverOptions.Taxable.ToString().Trim.ToUpper Then
                            RadioButtonTaxableOnly.Checked = True
                            RadioButtonTaxableOnly_CheckedChanged(Me, e)
                        End If
                        'fills the rollover institution name which was given from website
                        TextboxPayee2.Text = dtWithdawalOptions.Rows(0)("chvInstitutionName1").ToString()
                    Else
                        CheckBoxRollovers.Checked = False
                        CheckBoxRollovers_CheckedChanged(Me, e)
                    End If

                    'Check if any additional withtholding opted then it will be applied to the withdrawal request
                    If decAddlnWithholding <> 0 Then
                        CheckboxAddnlWitholding.Checked = True
                        CheckboxAddnlWitholding_CheckedChanged(Me, e)
                        If TextboxTaxRate.Text.Trim <> "" Then
                            TextboxTaxRate.Text += decAddlnWithholding
                        Else
                            TextboxTaxRate.Text = decAddlnWithholding
                        End If
                        TextboxTaxRate_TextChanged(Me, e)
                    End If

                    'Check if any express fee hs opted then over night delivery fee amount will be deducted from withdrawal request.
                    If decExpressFee <> 0 Then
                        For Each DGI As DataGridItem In DatagridDeductions.Items
                            If DGI.Cells(1).Text.Trim.ToUpper = "OND" Then
                                chkOvernightDeleivery = DGI.Cells(0).FindControl("CheckBoxDeduction")
                                If chkOvernightDeleivery IsNot Nothing Then
                                    chkOvernightDeleivery.Checked = True
                                    CheckBoxDeduction_Checked(Me, e)
                                End If
                            End If
                        Next
                    End If
                End If ' SR | 2016.06.08 | YRS-AT-2962 | end of Eform check condition
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region
    'End:AA:11.17.2015 YRS-AT-2639 Added below code to retreive ref request options lik erollover ,express fee and additinal withholding

    ' START | SR | YRS-AT-2664 | Check if RMD taxable amount can be rollover
    Public Function IsRMDTaxableAmountCanBeRollover() As Boolean
        Dim blnAllowRMDAmounttobeRollover As Boolean
        Dim decRMDTaxableAmount As Decimal
        Dim decRMDNonTaxableAmount As Decimal
        Dim decTotalNonTaxableAmount As Decimal
        Try

            blnAllowRMDAmounttobeRollover = False
            If (Not Session("MinimumDistributionTable_C19") Is Nothing) Then
                If (Not (String.IsNullOrEmpty(TextboxNonTaxableFinal.Text))) Then
                    decTotalNonTaxableAmount = Convert.ToDecimal(TextboxNonTaxableFinal.Text)
                End If

                If (Not (String.IsNullOrEmpty(TextboxMinDistAmount.Text))) Then
                    decRMDTaxableAmount = Convert.ToDecimal(TextboxMinDistAmount.Text)
                End If

                If (Not (String.IsNullOrEmpty(TextboxMinDistNonTaxable.Text))) Then
                    decRMDNonTaxableAmount = Convert.ToDecimal(TextboxMinDistNonTaxable.Text)
                End If

                If (decTotalNonTaxableAmount > (decRMDTaxableAmount + decRMDNonTaxableAmount)) Then
                    blnAllowRMDAmounttobeRollover = True
                End If
            End If
            Return blnAllowRMDAmounttobeRollover
        Catch ex As Exception
            Return False
        End Try
    End Function


    ' START: SR | 2017.11.10 | YRS-AT-3742 - YRS Bug:(HOT FIX NEEDED) -participants with RMDs more than non-taxable amount are being forced to take excessive amt (TrackIT 31764) 
    ' Set Payee1 & RMD amount together to display in payee1 grid.
    Public Sub Payee1BalanceWithRMD(ByVal payee1Balance As DataTable)
        Dim RMD As DataTable
        Dim payee1AndRMDMergedBalance As DataTable
        Dim rmdTaxable As Decimal
        Dim rmdNonTaxable As Decimal
        Dim payee1Taxable As Decimal
        Dim payee1NonTaxable As Decimal
        Try
            If HelperFunctions.isNonEmpty(payee1Balance) Then
                CurrentBalance = HelperFunctions.DeepCopy(Of DataTable)(payee1Balance)
            End If

            If HelperFunctions.isNonEmpty(Session("MinimumDistributionTable_C19")) Then
                RMD = HelperFunctions.DeepCopy(Of DataTable)(DirectCast(Session("MinimumDistributionTable_C19"), DataTable))

                'Add RMD taxable &  non-taxable amount into CurrentBalance data table.
                If (HelperFunctions.isNonEmpty(CurrentBalance)) Then
                    payee1AndRMDMergedBalance = DirectCast(CurrentBalance, DataTable)
                    For Each payee1AndRMDMergedBalanceDataRow As DataRow In payee1AndRMDMergedBalance.Rows
                        For Each rmdDataRow As DataRow In RMD.Rows
                            If rmdDataRow("AcctType").ToString().Trim() = payee1AndRMDMergedBalanceDataRow("AcctType").ToString.Trim() Then

                                ' Add RMD taxable balance into payee1 non-taxable based on account type
                                If Not Convert.IsDBNull(rmdDataRow("Taxable")) Then
                                    If Not Convert.IsDBNull(payee1AndRMDMergedBalanceDataRow("Taxable")) Then
                                        rmdTaxable = CType(rmdDataRow("Taxable"), Decimal)
                                        payee1Taxable = CType(payee1AndRMDMergedBalanceDataRow("Taxable"), Decimal)
                                        If (rmdTaxable > 0) Then
                                            payee1AndRMDMergedBalanceDataRow("Taxable") = Math.Round((payee1Taxable + rmdTaxable), 2)
                                        End If
                                    End If
                                End If

                                ' Add RMD non-taxable balance into payee1 non-taxable based on account type
                                If Not Convert.IsDBNull(rmdDataRow("NonTaxable")) Then
                                    If Not Convert.IsDBNull(payee1AndRMDMergedBalanceDataRow("NonTaxable")) Then
                                        rmdNonTaxable = CType(rmdDataRow("NonTaxable"), Decimal)
                                        payee1NonTaxable = CType(payee1AndRMDMergedBalanceDataRow("NonTaxable"), Decimal)
                                        If (rmdNonTaxable > 0) Then
                                            payee1AndRMDMergedBalanceDataRow("NonTaxable") = Math.Round((payee1NonTaxable + rmdNonTaxable), 2)
                                        End If
                                    End If
                                End If
                            End If
                        Next
                        payee1AndRMDMergedBalanceDataRow.AcceptChanges()
                    Next
                    CurrentBalance = HelperFunctions.DeepCopy(Of DataTable)(payee1AndRMDMergedBalance)
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Payee1BalanceWithRMD()", ex)
            Throw
        End Try
    End Sub
    ' END: SR | 2017.11.10 | YRS-AT-3742 - YRS Bug:(HOT FIX NEEDED) -participants with RMDs more than non-taxable amount are being forced to take excessive amt (TrackIT 31764) 

    'START : ML : 05/05/2020 | YRS-AT-4874 | Added controls for COVID Distribution
    Public Sub Payee1BalanceWithCOVID(ByVal payee1withRMDBalance As DataTable)
        Dim COVID As DataTable
        Dim payee1AndRMDAndCOVIDMergedBalance As DataTable
        Dim covidTaxable As Decimal
        Dim covidNonTaxable As Decimal
        Dim payee1Taxable As Decimal
        Dim payee1NonTaxable As Decimal
        Try
            If HelperFunctions.isNonEmpty(payee1withRMDBalance) Then
                CurrentBalance = HelperFunctions.DeepCopy(Of DataTable)(payee1withRMDBalance)
            End If

            If HelperFunctions.isNonEmpty(Session("COVIDProrateAccountDataTable")) Then
                COVID = HelperFunctions.DeepCopy(Of DataTable)(DirectCast(Session("COVIDProrateAccountDataTable"), DataTable))

                'Add RMD taxable &  non-taxable amount into CurrentBalance data table.
                If (HelperFunctions.isNonEmpty(CurrentBalance)) Then
                    payee1AndRMDAndCOVIDMergedBalance = DirectCast(CurrentBalance, DataTable)
                    For Each drpayee1AndRMDAndCOVIDMergedBalance As DataRow In payee1AndRMDAndCOVIDMergedBalance.Rows
                        For Each drcovid As DataRow In COVID.Rows
                            If drcovid("AcctType").ToString().Trim() = drpayee1AndRMDAndCOVIDMergedBalance("AcctType").ToString.Trim() Then

                                ' Add RMD taxable balance into payee1 non-taxable based on account type
                                If Not Convert.IsDBNull(drcovid("Taxable")) Then
                                    If Not Convert.IsDBNull(drpayee1AndRMDAndCOVIDMergedBalance("Taxable")) Then
                                        covidTaxable = CType(drcovid("Taxable"), Decimal)
                                        payee1Taxable = CType(drpayee1AndRMDAndCOVIDMergedBalance("Taxable"), Decimal)
                                        If (covidTaxable > 0) Then
                                            drpayee1AndRMDAndCOVIDMergedBalance("Taxable") = Math.Round((payee1Taxable + covidTaxable), 2)
                                        End If
                                    End If
                                End If

                                ' Add RMD non-taxable balance into payee1 non-taxable based on account type
                                If Not Convert.IsDBNull(drcovid("NonTaxable")) Then
                                    If Not Convert.IsDBNull(drpayee1AndRMDAndCOVIDMergedBalance("NonTaxable")) Then
                                        covidNonTaxable = CType(drcovid("NonTaxable"), Decimal)
                                        payee1NonTaxable = CType(drpayee1AndRMDAndCOVIDMergedBalance("NonTaxable"), Decimal)
                                        If (covidNonTaxable > 0) Then
                                            drpayee1AndRMDAndCOVIDMergedBalance("NonTaxable") = Math.Round((payee1NonTaxable + covidNonTaxable), 2)
                                        End If
                                    End If
                                End If
                            End If
                        Next
                        drpayee1AndRMDAndCOVIDMergedBalance.AcceptChanges()
                    Next
                    CurrentBalance = HelperFunctions.DeepCopy(Of DataTable)(payee1AndRMDAndCOVIDMergedBalance)
                End If
            End If
        Catch ex As Exception
            HelperFunctions.LogException("Payee1BalanceWith()", ex)
            Throw
        End Try
    End Sub
    'END : ML : 05/05/2020 | YRS-AT-4874 | Added controls for COVID Distribution
    ' START: SR | 2019.07.31 | YRS-AT-3870 - Prorate Hardship taxable & Non taxable components. 
    Private Sub SetHardShipTaxableAndNonTaxableForDisplay(ByVal requestedAmount As Decimal)
        Dim prorateFactor As Decimal
        If requestedAmount > 0 Then
            prorateFactor = requestedAmount / Math.Round(CDec(TextboxTDAvailableAmount.Text), 2)
        End If

        Me.TextboxHardShipAmount.Text = Math.Round(objRefundProcess.HardshipWithdrawalTaxable * prorateFactor, 2).ToString("0.00")
        Me.textboxHardShipNonTaxableAmount.Text = Math.Round(objRefundProcess.HardshipWithdrawalNonTaxable * prorateFactor, 2).ToString("0.00")
    End Sub
    ' END: SR | 2019.07.31 | YRS-AT-3870 - Prorate Hardship taxable & Non taxable components. 
    ' START: SR | 2019.07.31 | YRS-AT-4498 - Prorate Hardship taxable & Non taxable components. 
    Private Sub ClearHardshipSessions()
        Session("hardshipWithdrawalTaxable_C19") = Nothing
        Session("hardshipWithdrawalNonTaxable_C19") = Nothing
    End Sub
    ' END: SR | 2019.07.31 | YRS-AT-4498 - Prorate Hardship taxable & Non taxable components. 

    ''' <summary>
    ''' This function will be used to send an email on successful withdrawal processing.
    ''' </summary>
    ''' <remarks></remarks>
    'START |PK | 09/10/2019 | YRS-AT-2670 |Once withdrawal request is processed an email is sent to finance department.
    Public Sub SendMailForPuertoRico()
        Dim dictParameters As Dictionary(Of String, String)
        Dim isRicoYmca As Boolean
        Dim mailClient As YMCARET.YmcaBusinessObject.MailUtil
        Try
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "SendMailForPuertoRico() START")
            isRicoYmca = YMCARET.YmcaBusinessObject.RefundRequest.IsYmcaPuertoRico(Me.FundID)
            If (isRicoYmca = True) Then
                mailClient = New YMCARET.YmcaBusinessObject.MailUtil
                dictParameters = New Dictionary(Of String, String)
                dictParameters.Add("FundNo", Session("FundNo"))

                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process_SendMailForPuertoRico", String.Format("Send mail for fundno : {0} BEGIN", Session("FundNo")))
                mailClient.SendMailMessage(YMCAObjects.EnumEmailTemplateTypes.WITHDRAWAL_PROCESSING_PUERTORICO, "", "", "", "", dictParameters, "", Nothing, Mail.MailFormat.Html)
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process_SendMailForPuertoRico", String.Format("Send mail for fundno : {0} END", Session("FundNo")))
            End If
        Catch
            Throw
        Finally
            mailClient = Nothing
            dictParameters = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Refund Process", "SendMailForPuertoRico() END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
        End Try
    End Sub
    'END |PK | 09/10/2019 | YRS-AT-2670 |Once withdrawal request is processed an email is sent to finance department.

    'START : SC | 2020.04.05 | YRS-AT-4874 | Get covid requested and used amounts
#Region "COVID19"
    ''' <summary>
    ''' This Method is used to set COVID Amounts required for Processing
    ''' Gets User Requested amount, Covid Tax Rate, Covid Used amount from database
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub InitializeCOVIDAmountsForDisplay()
        Try
            Dim dtCovidAmountsForProcessing As DataTable
            dtCovidAmountsForProcessing = YMCARET.YmcaBusinessObject.RefundRequest_C19.GetCovidAmountsForProcessing(SessionRefundRequestID)
            If HelperFunctions.isNonEmpty(dtCovidAmountsForProcessing) Then
                Me.CovidAmountUsed = IIf(String.IsNullOrEmpty(dtCovidAmountsForProcessing.Rows(0)("numCovidAmountUsed").ToString()), 0, dtCovidAmountsForProcessing.Rows(0)("numCovidAmountUsed"))
                Me.CovidAmountRequested = IIf(String.IsNullOrEmpty(dtCovidAmountsForProcessing.Rows(0)("numCovidAmountRequested").ToString()), 0, dtCovidAmountsForProcessing.Rows(0)("numCovidAmountRequested"))
                Me.COVIDTaxrate = IIf(String.IsNullOrEmpty(dtCovidAmountsForProcessing.Rows(0)("numCovidTaxRate").ToString()), 0, dtCovidAmountsForProcessing.Rows(0)("numCovidTaxRate"))
                Me.COVIDFixedTaxrate = Me.COVIDTaxrate
                txtCovidAmountUsed.Text = FormatCurrency(Me.CovidAmountUsed)
                txtCovidAmountAvailable.Text = FormatCurrency(IIf(Me.CovidAmountLimit <= Me.CovidAmountUsed, 0.0, Me.CovidAmountLimit - Me.CovidAmountUsed))
                txtTaxRateC19.Text = Me.COVIDTaxrate
                Me.CovidAmountRequested = GetCovidAmountWithinLimit(Me.CovidAmountRequested)
                txtCovidAmountRequested.Text = FormatCurrency(Me.CovidAmountRequested)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    'END : SC | 2020.04.05 | YRS-AT-4874 | Get covid requested and used amounts

    'START : ML | 2020.04.05 | YRS-AT-4874 | Validate COVID Amount input and reset the screen 
    ''' <summary>
    ''' This Method is used to take COVID amount input from Use
    ''' Validate Amount with COVIDWithdrawa limit
    ''' Reset Screen based on COVID amount
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnChangeAmount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChangeAmount.Click
        Dim RequestedCOVIDAmount As Decimal
        Dim l_Error As String
        Try

            l_Error = ValidateRequestedCOVIDAmount(txtNewCovidAmount.Text)
            If Not (String.IsNullOrEmpty(l_Error)) Then
                MessageBox.Show(MessageBoxPlaceHolder, " YMCA - YRS", l_Error, MessageBoxButtons.Stop, False)
            Else
                Me.CovidAmountRequested = CDec(txtNewCovidAmount.Text)
                'Validate Amount
                txtCovidAmountRequested.Text = FormatCurrency(Me.CovidAmountRequested)
                RecalculateBalanceAfterCOVIDAmountChanges()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SuccessMessage", "CloseChangeRequestedCOVIDAmountDialog();", True)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Protected Sub RecalculateBalanceAfterCOVIDAmountChanges()
        Dim e As New EventArgs
        CheckBoxRollovers.Checked = False
        CheckBoxRollovers_CheckedChanged(Me, e)

        Session("CalculatedDataTableForCurrentAccounts_C19") = YMCARET.YmcaBusinessObject.RefundRequest.GetTransactionsForMarket(Me.FundID, Session("RefundRequestID_C19"))

        objRefundProcess.CalculateMinimumDistributionAmount()
        objRefundProcess.CalculateCOVIDDistributionAmount(Me.CovidAmountRequested, Me.COVIDTaxrate)
        objRefundProcess.CreatePayees()
        objRefundProcess.LoadCalculatedTableForPersonalMonies()
        Me.CopyAccountContributionTableForRefundable()
        If Me.RefundType = "REG" Or Me.RefundType = "VOL" Or Me.RefundType = "HARD" Then
            Me.DoRefundForRefundAccounts()
        End If
        Me.LoadPayeesDataGrid()
        Me.LoadPayee1ValuesIntoControls()
        Me.LoadPayee2ValuesIntoControls()
        Me.LoadPayee3ValuesIntoControls()
        Me.EnableMinimumDistributionControls()
        Me.DoFinalCalculation()
    End Sub
    'END : ML | 2020.04.05 | YRS-AT-4874 | Validate COVID Amount input and reset the screen 
    'START : SC | 2020.04.05 | YRS-AT-4874 | Validate Requested COVID Amount
    ''' <summary>
    ''' This method is used to validate Requested COVID Amount
    ''' 1. should be numeric and not "." or negative
    ''' 2. should not be negative
    ''' 3. must be less than or equal to the whichever is less of (Requested Total - RMD Amount) & (Covid Limit - Covid Used)
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ValidateRequestedCOVIDAmount(ByVal paramRequestedCOVIDAmount As String) As String
        Dim l_CovidAmountAvailable As Decimal
        Dim l_CovidAmountErrorMsg As String
        Try
            l_CovidAmountAvailable = GetMaxPossibleCovidAmountAfterRMD()
            If Not IsNumeric(paramRequestedCOVIDAmount) Then
                l_CovidAmountErrorMsg = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_COVID_AMOUNT_NAN).DisplayText
            ElseIf paramRequestedCOVIDAmount = "." Then
                l_CovidAmountErrorMsg = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_COVID_AMOUNT_NAN).DisplayText
            ElseIf CType(paramRequestedCOVIDAmount, Decimal) < 0 Then
                l_CovidAmountErrorMsg = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_COVID_AMOUNT_NEGATIVE).DisplayText
            ElseIf CType(paramRequestedCOVIDAmount, Decimal) > l_CovidAmountAvailable Then
                l_CovidAmountErrorMsg = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_COVID_INSUFFICIENT_AMT).DisplayText.Replace("$$AvailableAmount$$", l_CovidAmountAvailable)
            End If
            Return l_CovidAmountErrorMsg
        Catch ex As Exception
            Throw
        End Try
    End Function
    'START: SN | 05/12/2020 | YRS-AT-4874 | function to get max covid amount available
    ''' <summary>
    ''' This function is returning Max Covid amount after RMD
    ''' 1. It subtracts the RMD amount from Total withdrawal amount
    ''' 2. Must be less than or equal to the whichever is less of (Requested Total - RMD Amount) & (Covid Limit - Covid Used)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetMaxPossibleCovidAmountAfterRMD() As Decimal
        Dim l_CovidAmountAvailable As Decimal
        Dim l_CovidAmountUsed As Decimal
        Dim l_CovidAmountLimit As Decimal
        Dim l_totalRMDamount As Decimal
        Dim l_MaxCovidAmountToBeAvailed As Decimal
        Dim dtCovidAmountsForProcessing As DataTable
        Try
            dtCovidAmountsForProcessing = YMCARET.YmcaBusinessObject.RefundRequest_C19.GetCovidAmountsForProcessing(SessionRefundRequestID)
            l_totalRMDamount = CType(TextboxMinDistAmount.Text, Decimal) + CType(TextboxMinDistNonTaxable.Text, Decimal)
            l_MaxCovidAmountToBeAvailed = Me.CurrentAccountTotalWithInterest - l_totalRMDamount
            If HelperFunctions.isNonEmpty(dtCovidAmountsForProcessing) Then
                l_CovidAmountUsed = IIf(String.IsNullOrEmpty(dtCovidAmountsForProcessing.Rows(0)("numCovidAmountUsed").ToString()), 0, dtCovidAmountsForProcessing.Rows(0)("numCovidAmountUsed"))
                l_CovidAmountLimit = IIf(String.IsNullOrEmpty(dtCovidAmountsForProcessing.Rows(0)("numCovidAmountMaxLimit").ToString()), 0, dtCovidAmountsForProcessing.Rows(0)("numCovidAmountMaxLimit"))
                l_CovidAmountAvailable = FormatCurrency(IIf(l_CovidAmountLimit <= l_CovidAmountUsed, 0.0, l_CovidAmountLimit - l_CovidAmountUsed))
            End If
            If (l_CovidAmountAvailable > l_MaxCovidAmountToBeAvailed) Then
                l_CovidAmountAvailable = l_MaxCovidAmountToBeAvailed
            End If

            Return l_CovidAmountAvailable
        Catch ex As Exception
            Throw
        End Try
    End Function
    'END: SN | 05/12/2020 | YRS-AT-4874 | function to get max covid amount available

    ''' <summary>
    ''' This method is used to validate entered Covid Tax Rate when additional withholdings are checked
    ''' 1. should be numeric
    ''' 2. should be an integer
    ''' 2. can either be 0% or between 10%(default tax rate) and 100%. 
    ''' </summary>
    Private Function ValidateCOVIDTaxRate(ByVal paramCOVIDTaxRate As String) As Boolean
        Dim l_CovidAmountAvailable As Decimal
        Dim l_Result As Boolean = True
        Dim l_CovidTaxRate As Decimal
        Try
            If Not IsNumeric(paramCOVIDTaxRate) Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_COVID_TAXRATE_NAN).DisplayText, MessageBoxButtons.Stop, False)
                l_Result = False
            ElseIf (CType(paramCOVIDTaxRate, Decimal) Mod 1 <> 0) Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_COVID_TAXRATE_NONINTEGER).DisplayText, MessageBoxButtons.Stop, False)
                l_Result = False
            ElseIf (Not ((CType(paramCOVIDTaxRate, Integer) = 0) Or ((CType(paramCOVIDTaxRate, Integer) >= 10) And (CType(paramCOVIDTaxRate, Integer) <= 19)))) Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_WITHDRAWAL_COVID_TAXRATE_INVALID).DisplayText, MessageBoxButtons.Stop, False)
                l_Result = False
            End If
            Return l_Result
        Catch ex As Exception
            Throw
        End Try
    End Function
    ''' <summary>
    ''' Validate Covid Taxable, non-taxable components if matching with that of displayed in textbox before Save Processing
    ''' If validate, update taxable, non-taxable and tax rate for covid amounts in Covid Transactions
    ''' </summary>
    Private Function UpdateCOVIDTransactions(ByVal paramDTCovidTransactions As DataTable, ByVal paramUpdateCovidTransaction As Boolean) As Boolean
        Dim l_CovidTransactions As DataTable
        Dim l_Covid_Taxable As Decimal = 0
        Dim l_Covid_NonTaxable As Decimal = 0
        Dim l_Covid_TaxRate As Decimal = 0
        Dim l_Result As Boolean = True
        Try
            l_CovidTransactions = paramDTCovidTransactions
            If HelperFunctions.isNonEmpty(l_CovidTransactions) Then
                If Not IsDBNull(l_CovidTransactions.Compute("Sum(Taxable)", "")) Then
                    l_Covid_Taxable = Math.Round(CType(l_CovidTransactions.Compute("Sum(Taxable)", ""), Decimal), 2)
                End If
                If Not IsDBNull(l_CovidTransactions.Compute("Sum(NonTaxable)", "")) Then
                    l_Covid_NonTaxable = Math.Round(CType(l_CovidTransactions.Compute("Sum(NonTaxable)", ""), Decimal), 2)
                End If
            End If
            l_Covid_TaxRate = Me.COVIDTaxrate
            If (paramUpdateCovidTransaction) Then
                l_Covid_TaxRate = CType((txtTaxRateC19.Text), Decimal)
                YMCARET.YmcaBusinessObject.RefundRequest_C19.UpdateCovidTransactactionAfterProcessing(SessionRefundRequestID, l_Covid_Taxable, l_Covid_NonTaxable, l_Covid_TaxRate)
            End If
            Return l_Result
        Catch ex As Exception
            Throw
        End Try
    End Function
    'END : SC | 2020.04.05 | YRS-AT-4874 | Validate Requested COVID Amount 
    'START : ML | 2020.05.08 | YRS-AT-4854 |Validate Requested Amount with Prorated Total Amount
    ''' <summary>
    ''' Validate Total amount (COVID, Non COVID, Rollover, RMD Prorated value) with total request amount.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateProratedAmountWithRequestedAmount() As Boolean

        Dim totalAmount As Decimal
        Dim totalCovidAmount As Decimal
        Dim totalRollOverAmount As Decimal
        Dim totalNonCovidAmount As Decimal
        Dim totalRMDamount As Decimal

        Try

            totalCovidAmount = CType(txtTaxableC19.Text, Decimal) + CType(txtNonTaxableC19.Text, Decimal)
            totalRMDamount = CType(TextboxMinDistAmount.Text, Decimal) + CType(TextboxMinDistNonTaxable.Text, Decimal)
            totalNonCovidAmount = CType(TextboxTaxable.Text, Decimal) + CType(TextboxNonTaxable.Text, Decimal)
            totalRollOverAmount = CType(TextboxTaxable2.Text, Decimal) + CType(TextboxNonTaxable2.Text, Decimal)
            totalAmount = totalCovidAmount + totalRMDamount + totalNonCovidAmount + totalRollOverAmount
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ValidateProratedAmountWithRequestedAmount", " START")
            If (Me.CurrentAccountTotalWithInterest <> totalAmount) Then
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("1.", String.Format("Person ID: {0}", Me.PersonID))
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("2.", String.Format("RMD Amount: {0}", totalRMDamount))
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("3.", String.Format("COVID Amount: {0}", totalCovidAmount))
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("4.", String.Format("RollOver Amount: {0}", totalRollOverAmount))
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("5.", String.Format("NonCovid Amount: {0}", totalNonCovidAmount))
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ValidateProratedAmountWithRequestedAmount", "Save END")
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function
    'END : ML | 2020.05.08 | YRS-AT-4854 |Validate Requested Amount with Prorated Total Amount
    ''' <summary>
    ''' This function will fetch the Requested COVID Amount in taking into account the change in balances and interest for requested accounts in case of Full Withdrawal
    ''' 1. Get the current account balances with interest (Only the ones requested by the user)
    ''' 2. Get the Total RMD Amount(if applicable), Rollover amount(if requested)
    ''' Requested Covid Amount = Sum(total) of current accounts - (RMD Total + Rollover Total)
    ''' If Requested Covid Amount > (Covid Limit - Used Covid) Then Requested Covid Amount = Covid Limit - Used Covid
    ''' </summary>
    Private Function UpdateCovidRequestedAmountWithCurrentInterest() As Decimal
        Dim l_CurrentDataTable As DataTable
        Dim l_RequestedDataTable As DataTable
        Dim l_MRDDataTable As DataTable
        Dim l_DecimalRolloverAmount As Decimal
        Dim l_DecimalMRDAmount As Decimal
        Dim l_AccountType As String
        Dim l_AccountGroup As String = ""
        Dim l_bool_AMMatched As Boolean = True
        Dim l_bool_TMMatched As Boolean = True
        Dim l_FinalDataTableForWithdrawal As DataTable
        Dim l_FinalDataRow As DataRow
        Dim l_Decimal_PersonalPreTax As Decimal
        Dim l_Decimal_PersonalInterest As Decimal
        Dim l_Decimal_YMCAPreTax As Decimal
        Dim l_Decimal_YMCAInterest As Decimal
        Dim l_Decimal_Result As Decimal
        Try
            'Get Current Balances with Interest
            l_CurrentDataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetTransactionsForMarket(Me.FundID, SessionRefundRequestID)
            'Get Requested Accounts
            l_RequestedDataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundRequestsDetails(Me.SessionRefundRequestID)
            ' Get RMD Amounts if applicable
            If HelperFunctions.isNonEmpty(Session("MinimumDistributionTable_C19")) Then
                l_MRDDataTable = HelperFunctions.DeepCopy(Of DataTable)(DirectCast(Session("MinimumDistributionTable_C19"), DataTable))
                If Not l_MRDDataTable Is Nothing AndAlso l_MRDDataTable.Rows.Count > 0 Then
                    l_DecimalMRDAmount = CType(l_MRDDataTable.Compute("SUM(Taxable)", ""), Decimal) + CType(l_MRDDataTable.Compute("SUM(NonTaxable)", ""), Decimal)
                End If
            End If
            'Get schema of atsRefunds
            l_FinalDataTableForWithdrawal = YMCARET.YmcaBusinessObject.RefundRequest.GetSchemaRefundTable.Tables("atsRefunds")
            'Get Requested Rollover Amount
            l_DecimalRolloverAmount = GetRequestedRolloverAmount()

            For Each dr As DataRow In l_RequestedDataTable.Rows
                If dr("AccountType").GetType.ToString <> "System.DBNull" Then
                    If dr("AccountType") = "AM" Then
                        If dr("YMCATotal") = 0 Then
                            l_bool_AMMatched = False
                        End If
                    End If
                    If dr("AccountType") = "TM" Then
                        If dr("YMCATotal") = 0 Then
                            l_bool_TMMatched = False
                        End If
                    End If
                End If
            Next
            If l_RequestedDataTable.Rows.Count > 0 Then
                For Each l_DataRow As DataRow In l_CurrentDataTable.Rows
                    If Not l_DataRow("AccountType").GetType.ToString.Trim = "System.DBNull" Then
                        l_AccountType = DirectCast(l_DataRow("AccountType"), String).Trim
                        If Not l_DataRow("AccountGroup").GetType.ToString.Trim = "System.DBNull" Then
                            l_AccountGroup = DirectCast(l_DataRow("AccountGroup"), String).Trim.ToUpper
                        End If
                    End If
                    If (l_AccountType = "") Or (l_AccountType = "Total") Then
                    Else
                        If Me.RefundType.Trim.ToUpper = "HARD" And (l_AccountGroup = m_const_SavingsPlan_TD Or l_AccountGroup = m_const_SavingsPlan_TM) Then
                        Else
                            If Me.IsExistInRequestedAccounts(l_AccountType.Trim.ToUpper, l_RequestedDataTable) = True Then
                                l_FinalDataRow = l_FinalDataTableForWithdrawal.NewRow

                                If l_DataRow("Taxable").GetType.ToString = "System.DBNull" Then
                                    l_Decimal_PersonalPreTax = 0.0
                                Else
                                    l_Decimal_PersonalPreTax = DirectCast(l_DataRow("Taxable"), Decimal)
                                End If

                                If l_DataRow("Interest").GetType.ToString = "System.DBNull" Then
                                    l_Decimal_PersonalInterest = 0.0
                                Else
                                    l_Decimal_PersonalInterest = DirectCast(l_DataRow("Interest"), Decimal)
                                End If
                                l_Decimal_YMCAPreTax = 0.0
                                l_Decimal_YMCAInterest = 0.0
                                If (l_AccountType.ToString.ToUpper() = "AM" And l_bool_AMMatched = True) Or _
                                    (l_AccountType.ToString.ToUpper() = "TM" And l_bool_TMMatched = True) Or _
                                    objRefundProcess.IncludeorExcludeYMCAMoney(l_AccountGroup.ToString.ToUpper(), l_AccountType.ToString.ToUpper()) = True Then
                                    If l_DataRow("YMCATaxable").GetType.ToString = "System.DBNull" Then
                                        l_Decimal_YMCAPreTax = 0.0
                                    Else
                                        l_Decimal_YMCAPreTax = DirectCast(l_DataRow("YMCATaxable"), Decimal)
                                    End If

                                    If l_DataRow("YMCAInterest").GetType.ToString = "System.DBNull" Then
                                        l_Decimal_YMCAInterest = 0.0
                                    Else
                                        l_Decimal_YMCAInterest = DirectCast(l_DataRow("YMCAInterest"), Decimal)
                                    End If
                                End If
                                l_FinalDataRow("AcctType") = l_DataRow("AccountType")
                                l_FinalDataRow("Taxable") = l_Decimal_PersonalInterest + l_Decimal_PersonalPreTax + l_Decimal_YMCAInterest + l_Decimal_YMCAPreTax
                                l_FinalDataRow("NonTaxable") = l_DataRow("Non-Taxable")
                                l_FinalDataRow("TaxRate") = Me.TaxRate
                                l_FinalDataRow("Tax") = (l_Decimal_PersonalInterest + l_Decimal_PersonalPreTax + l_Decimal_YMCAInterest + l_Decimal_YMCAPreTax) * (Me.TaxRate / 100.0)
                                l_FinalDataRow("Payee") = "COVID"
                                l_FinalDataRow("FundedDate") = System.DBNull.Value
                                l_FinalDataRow("RequestType") = Me.RefundType
                                l_FinalDataRow("RefRequestsID") = Me.SessionRefundRequestID
                                l_FinalDataTableForWithdrawal.Rows.Add(l_FinalDataRow)
                            End If
                        End If
                    End If
                Next
            End If
            If l_FinalDataTableForWithdrawal.Rows.Count > 0 Then
                l_Decimal_Result = CType(l_FinalDataTableForWithdrawal.Compute("SUM (Taxable)", ""), Decimal) + CType(l_FinalDataTableForWithdrawal.Compute("SUM (NonTaxable)", ""), Decimal)
            End If
            ' Set property to hold CurrentAccountTotallWithInterest
            Me.CurrentAccountTotalWithInterest = l_Decimal_Result

            'Calculate Covid Available Amount            
            txtCovidAmountAvailable.Text = FormatCurrency(GetCovidAmountWithinLimit(l_Decimal_Result))

            'Calculate Covid Requested Amount
            l_Decimal_Result -= l_DecimalRolloverAmount + l_DecimalMRDAmount ' reserve the Rollover & RMD Amounts from Available balance            
            Me.CovidAmountRequested = GetCovidAmountWithinLimit(l_Decimal_Result)
            txtCovidAmountRequested.Text = FormatCurrency(Me.CovidAmountRequested)

            Return l_Decimal_Result
        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function GetRequestedRolloverAmount() As Decimal
        Dim l_RefRequestOptions As DataTable
        Dim l_DecimalRolloverAmount As Decimal
        Dim l_stringRolloverOption As String
        Try
            l_RefRequestOptions = YMCARET.YmcaBusinessObject.RefundRequest.GetRefRequestOptionsVer2(SessionRefundRequestID)
            If HelperFunctions.isNonEmpty(l_RefRequestOptions) Then
                If (l_RefRequestOptions.Select("chvFormFormat='EFORM'").Length > 0) Then
                    l_stringRolloverOption = l_RefRequestOptions.Rows(0)("chvRolloverOption").ToString().Trim.ToUpper()
                    If l_stringRolloverOption = enumRolloverOptions.Partial.ToString().Trim.ToUpper Then
                        l_DecimalRolloverAmount = IIf(String.IsNullOrEmpty(l_RefRequestOptions.Rows(0)("numRolloverAmount").ToString()), 0, l_RefRequestOptions.Rows(0)("numRolloverAmount"))
                    End If
                End If
            End If
            Return l_DecimalRolloverAmount
        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function GetCovidAmountWithinLimit(ByVal paramCovidAmt As Decimal) As Decimal
        If paramCovidAmt < 0 Then
            paramCovidAmt = 0
        End If
        If Me.CovidAmountUsed + paramCovidAmt > Me.CovidAmountLimit Then
            paramCovidAmt = Me.CovidAmountLimit - Me.CovidAmountUsed
        End If
        Return paramCovidAmt
    End Function
#End Region
End Class
