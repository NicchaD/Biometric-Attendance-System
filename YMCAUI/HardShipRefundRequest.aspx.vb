
'*******************************************************************************
' Copyright 3i Infotech Ltd. All Rights Reserved. 
'
' Project Name		:	YMCA - YRS
' FileName			:	VoluntaryRefundRequest.aspx.vb
' Author Name		:	SrimuruganG
' Employee ID		:	32365
' Email				:	srimurugan.ag@icici-infotech.com
' Contact No		:	8744
' Creation Time		:	10/6/2005 7:58:01 PM
' Program Specification Name	:	
' Unit Test Plan Name			:	
'
' Designed by			:	SrimuruganG
' Designed on			:	24. Oct 2005
'
' Changed by			:   Rahul Nasa   
' Changed on			:	21 Feb,06
' Change Description	:	Does not Round the data in the RefundRequest TextBox.
' Hafiz 03Feb06 Cache-Session
'*******************************************************************************
'Hafiz 03Feb06 Cache-Session
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 03Feb06 Cache-Session

' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL
'--This Enhancement is different from Foxpro as In Foxpro Rollover option is enabled. This option is disabled here.
'--This Change is confirmed by Mr. Mark Posy.
' Change By:Preeti On:09Mar06 IssueId:YMCA-2134 TD hardship refund creating two disbursements.
'********************************************************************************************************************************
'Modified By        Date(MM/DD/YYYY)    Description
'********************************************************************************************************************************
'Aparna Samala      01/22/2007          YREN -3027
'Aparna Samala      02/12/2007          YREN-3071
'Shubhrata          02/13/2007          YREN-3039,the SR account is to be treated in the same manner as that of AM provided that the participant is not active.
'Shubhrata          03/07/2007          to validate only numbers for TD Request Amount
'Aparna Samala      04/23/2007          Change in folder structure
'Ashutosh Patil     04/23/2007          YREN-3028,YREN-3029 
'Shubhrata          05/30/2007          Plan Split Changes
'********************************************************************************************************************************

Public Class HardShipRefundRequest
    Inherits System.Web.UI.Page
    Dim objRefundProcess As New Refunds
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
    Protected WithEvents TextBoxCurrentPIA As System.Web.UI.WebControls.TextBox
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
    Protected WithEvents LabelAddressUpdating As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxPayee1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents DataGridPayee1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelPayee2 As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPayee2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents DatagridPayee2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents RadioButtonNone As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonRolloverAll As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonTaxableOnly As System.Web.UI.WebControls.RadioButton
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents DatagridPayee3 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LabelAddress1 As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonAddress As System.Web.UI.WebControls.Button
    Protected WithEvents LabelAddress2 As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxAddress2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAddress3 As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxAddress3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelCity As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxRate As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTaxable As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNonTaxable As System.Web.UI.WebControls.Label
    Protected WithEvents LabelTax As System.Web.UI.WebControls.Label
    Protected WithEvents LabelNet As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxAddress1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxCity1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxZip As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxState As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxCountry As System.Web.UI.WebControls.TextBox
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
    Protected WithEvents RadioButtonRolloverYes As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonRolloverNo As System.Web.UI.WebControls.RadioButton
    Protected WithEvents LabelWaiver As System.Web.UI.WebControls.Label
    Protected WithEvents RadioButtonWaiverYes As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonWaiverNo As System.Web.UI.WebControls.RadioButton
    Protected WithEvents LabelAddnlWitholding As System.Web.UI.WebControls.Label
    Protected WithEvents RadioButtonAddnlWitholdingYes As System.Web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonAddnlWitholdingNo As System.Web.UI.WebControls.RadioButton
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
    Protected WithEvents datagridCheckBox As Infotech.DataGridCheckBox.CheckBoxColumn
    Protected WithEvents CheckBoxDeduction As System.Web.UI.WebControls.CheckBox
    Protected WithEvents MessageBoxPlaceHolder As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonTab1OK As System.Web.UI.WebControls.Button

    Protected WithEvents TextboxMinDistTaxRate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxMinDistAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxMinDistTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxMinDistNet As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelRequiredMinDisbAmount As System.Web.UI.WebControls.Label
    Protected WithEvents ButtonOK As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents Button2 As System.Web.UI.WebControls.Button
    Protected WithEvents Button3 As System.Web.UI.WebControls.Button


    Protected WithEvents TextboxTDAvailableAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxRequestAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxHardShipTaxRate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxHardShipAmount As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxHardShipTax As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxHardShipNet As System.Web.UI.WebControls.TextBox

    Protected WithEvents m_DataGridCheckBox As Infotech.DataGridCheckBox.CheckBoxColumn


    ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start 
    Dim chkCreatePayeeProcessed As Boolean = False  'This flag is to keep check on wether createPayee function has already called while entering Required amount.
    ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End

    'Plan Split Changes
    Protected WithEvents LabelPlanChosen As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxPlanChosen As System.Web.UI.WebControls.TextBox
    'Plan Split Changes



    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

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
    'END - Retirement plan account groups
    'START - Ssvings plan account groups
    Const m_const_SavingsPlan_TD As String = "STD"
    Const m_const_SavingsPlan_TM As String = "STM"
    Const m_const_SavingsPlan_RT As String = "SRT"
    'END - Savings plan account groups
#End Region
#Region " Form Properties "

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
            If Not Session("RefundType") Is Nothing Then
                Return (CType(Session("RefundType"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("RefundType") = Value
        End Set
    End Property


    ' To get / set Payee2 ID
    Private Property Payee2ID() As String
        Get
            If Not Session("Payee2ID") Is Nothing Then
                Return (CType(Session("Payee2ID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("Payee2ID") = Value
        End Set
    End Property

    ' To get / set Payee3 ID
    Private Property Payee3ID() As String
        Get
            If Not Session("Payee3ID") Is Nothing Then
                Return (CType(Session("Payee3ID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("Payee3ID") = Value
        End Set
    End Property


    ''To Keep the Currency Code.
    Private Property CurrencyCode() As String
        Get
            If Not Session("CurrencyCode") Is Nothing Then
                Return (CType(Session("CurrencyCode"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("CurrencyCode") = Value
        End Set
    End Property

    'To Keep the flag to store Whether Type is Personal Or Not.
    Private Property SessionIsPersonalOnly() As Boolean
        Get
            If Not (Session("IsPersonalOnly")) Is Nothing Then
                Return (CType(Session("IsPersonalOnly"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsPersonalOnly") = Value
        End Set
    End Property


    ' To get / set Payee1 Addresss ID
    Private Property PayeeAddressID() As String
        Get
            If Not Session("PayeeAddressID") Is Nothing Then
                Return (CType(Session("PayeeAddressID"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PayeeAddressID") = Value
        End Set
    End Property


    ' To get / set RefundType
    Private Property ChangedRefundType() As String
        Get
            If Not Session("ChangedRefundType") Is Nothing Then
                Return (CType(Session("ChangedRefundType"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("ChangedRefundType") = Value
        End Set
    End Property


    ' To Get / Set Termination PIA
    Private Property TerminationPIA() As Decimal
        Get
            If Not Session("TerminationPIA") Is Nothing Then
                Return (CType(Session("TerminationPIA"), Decimal))
            Else
                Return 0.0
            End If
        End Get

        Set(ByVal Value As Decimal)
            Session("TerminationPIA") = Value
        End Set
    End Property

    ' To Get / Set YMCAAvailableAmount
    Private Property YMCAAvailableAmount() As Decimal
        Get
            If Not Session("YMCAAvailableAmount") Is Nothing Then
                Return (CType(Session("YMCAAvailableAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get

        Set(ByVal Value As Decimal)
            Session("YMCAAvailableAmount") = Value
        End Set
    End Property

    ' To Get / Set VoluntryAmount
    Private Property VoluntryAmount() As Decimal
        Get
            If Not Session("VoluntryAmount") Is Nothing Then
                Return (CType(Session("VoluntryAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get

        Set(ByVal Value As Decimal)
            Session("VoluntryAmount") = Value
        End Set
    End Property


    ' To Get / Set Personal Amount
    Private Property PersonalAmount() As Decimal
        Get
            If Not Session("PersonalAmount") Is Nothing Then
                Return (CType(Session("PersonalAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get

        Set(ByVal Value As Decimal)
            Session("PersonalAmount") = Value
        End Set
    End Property



    'To Get / Set Total Refund Amount
    Private Property TotalRefundAmount() As Decimal
        Get
            If Not Session("TotalRefundAmount") Is Nothing Then
                Return (CType(Session("TotalRefundAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get

        Set(ByVal Value As Decimal)
            Session("TotalRefundAmount") = Value
        End Set
    End Property


    'To Get / Set CurrentPIA
    Private Property CurrentPIA() As Decimal
        Get
            If Not Session("CurrentPIA") Is Nothing Then
                Return (CType(Session("CurrentPIA"), Decimal))
            Else
                Return 0.0
            End If
        End Get

        Set(ByVal Value As Decimal)
            Session("CurrentPIA") = Value
        End Set
    End Property

    'To Keep the RefundRequest ID for Selected Member
    Private Property SessionRefundRequestID() As String
        Get
            If Not (Session("RefundRequestID")) Is Nothing Then
                Return (CType(Session("RefundRequestID"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("RefundRequestID") = Value
        End Set
    End Property


    ' To Get / Set terminated flag
    Private Property IsTerminated() As Boolean
        Get
            If Not Session("IsTerminated") Is Nothing Then
                Return (CType(Session("IsTerminated"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsTerminated") = Value
        End Set
    End Property

    ' To get / set Person Age
    Private Property PersonAge() As Decimal
        Get
            If Not Session("PersonAge") Is Nothing Then
                Return (CType(Session("PersonAge"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("PersonAge") = Value
        End Set
    End Property

    ' To get / set Deductions amount.
    Private Property DeductionsAmount() As Decimal
        Get
            If Not Session("DeductionsAmount") Is Nothing Then
                Return (CType(Session("DeductionsAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("DeductionsAmount") = Value
        End Set
    End Property


    ' To Get / Set Is vested flag
    Private Property IsVested() As Boolean
        Get
            If Not Session("IsVested") Is Nothing Then
                Return (CType(Session("IsVested"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsVested") = Value
        End Set
    End Property

    ' To Get / Set Is AM accountType 
    Private Property IsAcctTypeAMIsPresent() As Boolean
        Get
            If Not Session("IsAcctTypeAMIsPresent") Is Nothing Then
                Return (CType(Session("IsAcctTypeAMIsPresent"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsAcctTypeAMIsPresent") = Value
        End Set
    End Property


    ' To Get / Set Is HardShip flag
    Private Property IsHardShip() As Boolean
        Get
            If Not Session("IsHardShip") Is Nothing Then
                Return (CType(Session("IsHardShip"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsHardShip") = Value
        End Set
    End Property

    ' To Get / Set Is HardShip flag
    Private Property IsTookTDAccount() As Boolean
        Get
            If Not Session("IsTookTDAccount") Is Nothing Then
                Return (CType(Session("IsTookTDAccount"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsTookTDAccount") = Value
        End Set
    End Property


    'To Get / Set Tax Rate
    Private Property TaxRate() As Decimal
        Get
            If Not Session("TaxRate") Is Nothing Then
                Return (CType(Session("TaxRate"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TaxRate") = Value
        End Set
    End Property

    'To Get / Set Tax Rate
    Private Property MinDistributionTaxRate() As Integer
        Get
            If Not Session("MinDistributionTaxRate") Is Nothing Then
                Return (CType(Session("MinDistributionTaxRate"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("MinDistributionTaxRate") = Value
        End Set
    End Property

    'To Get / Set Tax Rate
    Private Property TDTaxRate() As Decimal
        Get
            If Not Session("TDTaxRate") Is Nothing Then
                Return (CType(Session("TDTaxRate"), Decimal))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TDTaxRate") = Value
        End Set
    End Property


    'To set & get Minimum Distribution Amount
    Private Property MinimumDistributionAmount() As Decimal
        Get
            If Not Session("MinimumDistributionAmount") Is Nothing Then
                Return (CType(Session("MinimumDistributionAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinimumDistributionAmount") = Value
        End Set
    End Property
    '' To Get / Set the Refund Expiry Date
    Private Property RefundExpiryDate() As Integer
        Get
            If Not Session("RefundExpiryDate") Is Nothing Then
                Return (CType(Session("RefundExpiryDate"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Integer)
            Session("RefundExpiryDate") = Value
        End Set
    End Property

    'To Keep the flag to Raise the Refund Popup window
    Private Property SessionIsRefundProcessPopupAllowed() As Boolean
        Get
            If Not (Session("IsRefundProcessPopupAllowed")) Is Nothing Then
                Return (CType(Session("IsRefundProcessPopupAllowed"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsRefundProcessPopupAllowed") = Value
        End Set
    End Property


    ' To Get / set the MinimumDistributedAge
    Private Property MinimumDistributedAge() As Decimal
        Get
            If Not Session("MinimumDistributedAge") Is Nothing Then
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MinimumDistributedAge") = Value
        End Set
    End Property

    'To Get / Set Maximum PIA Amount.
    Private Property MaximumPIAAmount() As Decimal
        Get
            If Not Session("MaximumPIAAmount") Is Nothing Then
                Return (CType(Session("MaximumPIAAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("MaximumPIAAmount") = Value
        End Set
    End Property

    'To Get / Set HardShip Amount, incase of HardShip.
    Private Property HardshipAmount() As Decimal
        Get
            If Not Session("HardshipAmount") Is Nothing Then
                Return (CType(Session("HardshipAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("HardshipAmount") = Value
        End Set
    End Property



    'To Get / Set TotalAvailable, incase of HardShip.
    Private Property TotalAvailable() As Decimal
        Get
            If Not Session("TotalAvailable") Is Nothing Then
                Return (CType(Session("TotalAvailable"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TotalAvailable") = Value
        End Set
    End Property



    'To Get / Set HardShip Amount, incase of HardShip.
    Private Property HardshipAvailable() As Decimal
        Get
            If Not Session("HardshipAvailable") Is Nothing Then
                Return (CType(Session("HardshipAvailable"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("HardshipAvailable") = Value
        End Set
    End Property



    Private Property inTaxable() As Decimal
        Get
            If Not Session("inTaxable") Is Nothing Then
                Return (CType(Session("inTaxable"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inTaxable") = Value
        End Set
    End Property

    Private Property inNonTaxable() As Decimal
        Get
            If Not Session("inNonTaxable") Is Nothing Then
                Return (CType(Session("inNonTaxable"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inNonTaxable") = Value
        End Set
    End Property

    Private Property inGross() As Decimal
        Get
            If Not Session("inGross") Is Nothing Then
                Return (CType(Session("inGross"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inGross") = Value
        End Set
    End Property

    Private Property inVolAcctsUsed() As Decimal
        Get
            If Not Session("inVolAcctsUsed") Is Nothing Then
                Return (CType(Session("inVolAcctsUsed"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inVolAcctsUsed") = Value
        End Set
    End Property

    Private Property inVolAcctsAvailable() As Decimal
        Get
            If Not Session("inVolAcctsAvailable") Is Nothing Then
                Return (CType(Session("inVolAcctsAvailable"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("inVolAcctsAvailable") = Value
        End Set
    End Property





    'To Get / Set HardShipTaxRate
    Private Property HardShipTaxRate() As Decimal
        Get
            If Not Session("HardShipTaxRate") Is Nothing Then
                Return (CType(Session("HardShipTaxRate"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("HardShipTaxRate") = Value
        End Set
    End Property

    ' To get / set RefundStatus, this Property is used in RefundRequestWebForm.
    Private Property RefundStatus() As String
        Get
            If Not Session("RefundStatus") Is Nothing Then
                Return (CType(Session("RefundStatus"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("RefundStatus") = Value
        End Set
    End Property


    Private Property RequestedAmount() As Decimal
        Get
            If Not Session("RequestedAmount") Is Nothing Then
                Return (CType(Session("RequestedAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("RequestedAmount") = Value
        End Set
    End Property


    'To Get / Set HardShip Amount, incase of HardShip.
    Private Property TDUsedAmount() As Decimal
        Get
            If Not Session("TDUsedAmount") Is Nothing Then
                Return (CType(Session("TDUsedAmount"), Decimal))
            Else
                Return 0.0
            End If
        End Get
        Set(ByVal Value As Decimal)
            Session("TDUsedAmount") = Value
        End Set
    End Property


    'To Keep the flag to store Whether Type is Personal Or Not.
    Private Property IsPersonalOnly() As Boolean
        Get
            If Not (Session("IsPersonalOnly")) Is Nothing Then
                Return (CType(Session("IsPersonalOnly"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsPersonalOnly") = Value
        End Set
    End Property

    'To Keep the Refund Request Datagrid to Refresh 

    Private Property SessionIsRefundRequest() As Boolean
        Get
            If Not (Session("IsRefundRequest")) Is Nothing Then
                Return (CType(Session("IsRefundRequest"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsRefundRequest") = Value
        End Set
    End Property
    'added by Shubhrata YREN-3039,Feb13th 2007
    'To Keep the StatusType for Selected Member
    Private Property SessionStatusType() As String
        Get
            If Not (Session("StatusType")) Is Nothing Then
                Return (CType(Session("StatusType"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("StatusType") = Value
        End Set
    End Property
    'Plan Split Changes
    Private Property PlanTypeChosen() As String
        Get
            If Not Session("PlanTypeChosen") Is Nothing Then
                Return (CType(Session("PlanTypeChosen"), String))
            Else
                Return String.Empty
            End If

        End Get
        Set(ByVal Value As String)
            Session("PlanTypeChosen") = Value
        End Set
    End Property
    Private Property IsRetiredActive() As Boolean
        Get
            If Not Session("IsRetiredActive") Is Nothing Then
                Return (CType(Session("IsRetiredActive"), Boolean))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("IsRetiredActive") = Value
        End Set
    End Property
    'Plan Split Changes
#End Region

#Region "Global Declaration"
    'by Aparna YREN-3027
    Dim IDM As New IDMforAll
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", False)
        End If
        'Code Added by Ashutosh on 18-04-2006
        If Me.IsPostBack = False Then
            Session("ds_PrimaryAddress") = Nothing
        End If
        '************************************
        Try
            '' Populate Contribution Table 
            Me.LoadAccountContribution()
            '' Load Personal Inforamtion
            Me.LoadInformation()

            '' Here onwards the Logic Starts

            '' Copy the Values from Account Contribution Table for doing calculation of 
            '' Current accounts.

            Me.CopyAccountContributionTableForCurrentAccounts()

            ''Do calucation of Current Accounts.

            Me.DoHardshipRefundForCurrentAccount()



            If Me.IsPostBack Then
                Me.SessionIsRefundProcessPopupAllowed = False
                ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start
                Me.CopyAccountContributionTableForRefundable()
                Me.DoHardshipRefundForRefundAccounts()
                Me.EnableMinimumDistributionControls()
                LoadValuesForProcessedAmounts(False)
                ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End
            Else
                'Plan Split Changes
                Me.TextboxPlanChosen.Text = Me.PlanTypeChosen
                'Plan Split Changes

                Me.LoadDeafultValues()
                Me.LoadRequestedAccounts()
                Me.LoadPIA()

                objRefundProcess.LoadSchemaRefundTable()
                objRefundProcess.LoadDeductions(Me.DatagridDeductions) ' ask datagrid binding
                objRefundProcess.CalculateMinimumDistributionAmount()
                objRefundProcess.CreatePayees()
                'the below fucntion will set the necessary properties from RefundProcess obj to the Hardhsip processing screen
                Me.SetPropertiesAfterCreatePayees()
                objRefundProcess.LoadRefundableDataTable()
                objRefundProcess.LoadAccountBreakDown()



                '' Load Refund Request configuration 
                Me.LoadRefundConfiguration()



                '' Now do calculation of what can be Refund to the member.
                '' So do Voluntry refund and have a table

                Me.CopyAccountContributionTableForRefundable()


                ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start
                Me.DoHardshipRefundForRefundAccounts()
                ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End



                Me.LoadPayeesDataGrid()
                Me.LoadPayee1ValuesIntoControls()
                Me.EnableMinimumDistributionControls()
                Me.DoFinalCalculation()
                ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End
                '' To Enable / Disable Save Button
                Me.EnableDisableSaveButton()
            End If




            '' to load LoadHardShipvalues
            Me.LoadHardShipvalues()







            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start
            TextboxRequestAmount.Attributes.Add("onblur", "javascript: TextboxRequestAmount_Change();")
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End
            'Code Added by Ashutosh 18/04/06
            'sHUBHRATA mAR07th,2007 to validate only numbers for TD Request Amount
            Me.TextboxRequestAmount.Attributes.Add("onkeypress", "javascript:ValidateDecimal();")
            'sHUBHRATA mAR07th,2007 to validate only numbers for TD Request Amount
            If Not Session("ds_PrimaryAddress") Is Nothing Then

                LoadFromPopUp()
            End If
            '***********************************
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" & Server.UrlDecode(ex.Message), False)
        End Try

    End Sub
    'Function made by ashutosh 18/04/06******************************
    Private Function LoadFromPopUp()
        Dim l_str_zip As String
        If Not Session("ds_PrimaryAddress") Is Nothing Then
            Dim dataset_AddressInfo As DataSet
            Dim l_DataSet As DataSet
            Dim l_DataTable As DataTable
            Dim datarow_Row As DataRow
            Dim datarow_NewRow As DataRow
            dataset_AddressInfo = (CType(Session("ds_PrimaryAddress"), DataSet))
            datarow_Row = dataset_AddressInfo.Tables("AddressInfo").Rows(0)
            Me.TextboxAddress1.Text = datarow_Row.Item("Address1").ToString
            Me.TextboxAddress2.Text = datarow_Row.Item("Address2").ToString
            Me.TextboxAddress3.Text = datarow_Row.Item("Address3").ToString
            Me.TextboxCity1.Text = datarow_Row.Item("City").ToString
            'TextboxCity2.Text = datarow_Row.Item("state")
            'TextboxCity3.Text = datarow_Row.Item("Zip")
            Me.TextBoxState.Text = datarow_Row.Item("StateName").ToString
            Me.TextBoxCountry.Text = datarow_Row.Item("CountryName").ToString

            If datarow_Row.Item("CountryName").ToString = "CANADA" Then
                l_str_zip = datarow_Row.Item("Zip")
                Me.TextBoxZip.Text = l_str_zip.Substring(0, 3) + " " + l_str_zip.Substring(3, 3)
            Else
                Me.TextBoxZip.Text = datarow_Row.Item("Zip").ToString
            End If
            dataset_AddressInfo = Nothing
        End If
    End Function
    '*****************************************************
    Private Function LoadHardShipvalues()

        Try
            '''''Rahul 16 Feb,2006
            Me.TextboxTDAvailableAmount.Text = Math.Round(Me.HardshipAvailable, 2)
            '''''Rahul 16 Feb,2006
        Catch ex As Exception
            Throw
        End Try
    End Function
    'Changed the function to now process the logic with the Acct Group rather than with Acct Type in view of the addition of new acct types.
    Private Function DoHardshipRefundForRefundAccounts()

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Dim l_UserSide As Boolean
        Dim l_YMCASide As Boolean
        Dim l_Interest As Boolean

        Dim l_AccountGroup As String

        Try
           
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL START
            l_DataTable = CType(Session("RefundableDataTable"), DataTable)
            'l_DataTable = CType(Session("CalculatedDataTableForRefundable"), DataTable)
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL END


            If Not l_DataTable Is Nothing Then


                For Each l_DataRow In l_DataTable.Rows

                    'Reset the flag
                    l_UserSide = True
                    l_YMCASide = True
                    l_Interest = True

                    If l_DataRow.RowState <> DataRowState.Deleted AndAlso Not (l_DataRow("AccountGroup").GetType.ToString = "System.DBNull") Then

                        If Not (CType(l_DataRow("AccountGroup"), String).Trim = "Total") Then


                            l_AccountGroup = CType(l_DataRow("AccountGroup"), String).Trim.ToUpper

                            If Me.IsBasicAccount(l_DataRow) Then
                                l_UserSide = False
                                l_YMCASide = False
                                l_Interest = False
                            End If

                            Select Case (l_AccountGroup.ToUpper.Trim)
                                'Start - Retirement Plan Group
                            Case m_const_RetirementPlan_AM
                                    l_UserSide = True
                                    l_YMCASide = False
                                    l_Interest = True

                                    If Me.IsTerminated = True Then
                                        l_UserSide = True
                                        l_YMCASide = True
                                        l_Interest = True
                                    End If

                                Case m_const_RetirementPlan_AP
                                    l_UserSide = True
                                    l_YMCASide = False
                                    l_Interest = True


                                Case m_const_RetirementPlan_RP
                                    l_UserSide = True
                                    l_YMCASide = False
                                    l_Interest = True



                                Case m_const_RetirementPlan_SR
                                    'Modified by Shubhrata Feb13th 2007,YREN-3039 SR account is to be treated similarly to AM account
                                    'on the condition that participant is not active
                                    'l_UserSide = False
                                    'l_YMCASide = False
                                    'l_Interest = False
                                    If Me.SessionStatusType.Trim.ToUpper <> "AE" Then
                                        l_UserSide = True
                                        l_YMCASide = False
                                        l_Interest = True

                                        If Me.IsTerminated = True Then
                                            l_UserSide = True
                                            l_YMCASide = True
                                            l_Interest = True
                                        End If
                                    Else
                                        l_UserSide = False
                                        l_YMCASide = False
                                        l_Interest = False
                                    End If
                                    'End - Retirement Plan Group
                                    'Start - Savings Plan Group
                                Case m_const_SavingsPlan_TD
                                    l_UserSide = False
                                    l_YMCASide = False
                                    l_Interest = False

                                    If Me.IsTerminated = True And Me.PersonAge >= 59.5 Then
                                        l_UserSide = True
                                        l_YMCASide = False
                                        l_Interest = True
                                    End If

                                Case m_const_SavingsPlan_TM
                                    l_UserSide = False
                                    l_YMCASide = False
                                    l_Interest = False

                                    If Me.IsTerminated = True Then
                                        l_UserSide = True
                                        l_YMCASide = True
                                        l_Interest = True
                                    ElseIf Me.PersonAge >= 59.5 Then
                                        l_UserSide = True
                                        l_YMCASide = False
                                        l_Interest = True
                                    End If

                                Case m_const_SavingsPlan_RT
                                    l_UserSide = True
                                    l_YMCASide = False
                                    l_Interest = True
                                    'End - Savings Plan Group

                            End Select


                            ''Modify the values

                            If l_UserSide Then

                                If l_YMCASide = False Then

                                    l_DataRow("YMCATaxable") = "0.00"
                                    l_DataRow("YMCAInterest") = "0.00"
                                    l_DataRow("Total") = CType(l_DataRow("Total"), Decimal) - CType(l_DataRow("YMCATotal"), Decimal)
                                    l_DataRow("YMCATotal") = "0.00"

                                End If

                                'Me.YMCAAvailableAmount += CType(l_DataRow("YMCATaxable"), Decimal)

                            Else
                                l_DataRow.Delete()
                            End If

                        End If
                    End If ' Main If...

                Next

            End If

            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL START
            For Each l_DataRow In l_DataTable.Rows
                If l_DataRow.RowState <> DataRowState.Deleted Then
                    l_DataRow("Emp.Total") = Convert.ToDecimal(l_DataRow("Taxable")) + Convert.ToDecimal(l_DataRow("Non-Taxable")) + Convert.ToDecimal(l_DataRow("Interest"))
                    l_DataRow("YMCATotal") = Convert.ToDecimal(l_DataRow("YMCATaxable")) + Convert.ToDecimal(l_DataRow("YMCAInterest"))
                    l_DataRow("Total") = Convert.ToDecimal(l_DataRow("Emp.Total")) + Convert.ToDecimal(l_DataRow("YMCATotal"))
                End If
            Next

            l_DataTable.AcceptChanges()
            Session("RefundableDataTable") = l_DataTable
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL END
            '            Select Case c_Refundable This part seems to be not integrated
            'LOCATE

            'SCAN
            '	REPLACE	PersonalTotal	WITH	PersonalPreTax		+	;	
            '											PersonalPostTax	+ 	;
            '											PersonalInterest,		;
            '				YmcaTotal 		WITH 	YmcaPreTax 			+ 	;
            '											YMCAInterest,			;
            '				TotalTotal		WITH 	PersonalTotal 		+ 	;
            '											YmcaTotal				;
            '		IN c_Refundable

            'ENDSCAN
        Catch ex As Exception
            Throw
        End Try


    End Function

    Private Function CopyAccountContributionTableForRefundable()

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_AccountContributionTable As DataTable
        Dim l_CalculationDataTable As DataTable
        Dim l_DataColumn As DataColumn
        Dim l_DataRow As DataRow

        Try

            '' Get the Account contribution Table from Cache.
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'Hafiz 03Feb06 Cache-Session

            '' Take the Contribution Table, which is available in Cache
            'Hafiz 03Feb06 Cache-Session
            'l_AccountContributionTable = l_CacheManager.GetData("AccountContribution")
            l_AccountContributionTable = Session("AccountContribution")
            'Hafiz 03Feb06 Cache-Session

            If Not l_AccountContributionTable Is Nothing Then

                l_CalculationDataTable = l_AccountContributionTable.Clone

                'Copy all Values into Calculation Table 

                For Each l_DataRow In l_AccountContributionTable.Rows

                    If Not (l_DataRow("AccountType").GetType.ToString = "System.DBNull") Then

                        If Not (CType(l_DataRow("AccountType"), String) = "Total") Then
                            l_CalculationDataTable.ImportRow(l_DataRow)
                        End If
                    End If
                Next

                '' add the copied table to Cache.
                'Hafiz 03Feb06 Cache-Session
                'l_CacheManager.Add("CalculatedDataTableForRefundable", l_CalculationDataTable)
                Session("CalculatedDataTableForRefundable") = l_CalculationDataTable
                'Hafiz 03Feb06 Cache-Session

            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    'Private Function DoRegularRefundForCurrentAccounts()

    '    Dim l_CacheManager As CacheManager
    '    Dim l_DataTable As DataTable

    '    Try
    '        l_CacheManager = CacheFactory.GetCacheManager
    '        l_DataTable = CType(l_CacheManager.GetData("CalculatedDataTableForCurrentAccounts"), DataTable)

    '        If Not l_DataTable Is Nothing Then
    '            '' Do calucation of Current Accounts.
    '            Me.DoFullRefund(l_DataTable)
    '            Me.CalculateTotal(l_DataTable)
    '            Me.LoadCalculatedTableForCurrentAccounts()
    '            Me.SetSelectedIndex(Me.DatagridCurrentAccts, l_DataTable)
    '        End If

    '    Catch ex As Exception
    '        Throw
    '    End Try



    'End Function

    Private Function EnableMinimumDistributionControls()

        If Me.MinimumDistributionAmount > 0.01 Then

            Me.TextboxMinDistTaxRate.Visible = True
            Me.TextboxMinDistAmount.Visible = True
            Me.TextboxMinDistTaxable.Visible = True
            Me.TextboxMinDistNet.Visible = True

        Else

            Me.TextboxMinDistTaxRate.Visible = False
            Me.TextboxMinDistAmount.Visible = False
            Me.TextboxMinDistTaxable.Visible = False
            Me.TextboxMinDistNet.Visible = False

        End If

        Me.LoadMinDistributionValuesIntoControls()

    End Function

    Private Function LoadMinDistributionValuesIntoControls()

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_DataTable As DataTable

        Dim l_Decimal_Taxable As Decimal
        Dim l_Decimal_NonTaxable As Decimal
        Dim l_Decimal_Tax As Decimal
        Dim l_Decimal_Net As Decimal

        Try
            'Hafiz 03Feb06 Cache-Sessionl_CacheManager = CacheFactory.GetCacheManager
            'l_DataTable = CType(l_CacheManager.GetData("MinimumDistributionTable"), DataTable)
            l_DataTable = CType(Session("MinimumDistributionTable"), DataTable)
            'Hafiz 03Feb06 Cache-Session

            If Not l_DataTable Is Nothing Then

                If l_DataTable.Rows.Count > 0 Then

                    l_Decimal_Taxable = CType(l_DataTable.Compute("SUM (Taxable)", ""), Decimal)
                    l_Decimal_NonTaxable = CType(l_DataTable.Compute("SUM (NonTaxable)", ""), Decimal)
                    l_Decimal_Tax = CType(l_DataTable.Compute("SUM (Tax)", ""), Decimal)


                    '  Me.TextboxTaxRate.Text = Me.TaxRate.ToString

                    Me.TextboxMinDistAmount.Text = Math.Round(l_Decimal_Taxable, 2)
                    Me.TextboxMinDistTaxable.Text = Math.Round(l_Decimal_Tax, 2)

                    Me.TextboxMinDistNet.Text = (l_Decimal_Taxable + l_Decimal_NonTaxable) - l_Decimal_Tax

                    Me.LabelRequiredMinDisbAmount.Visible = True

                Else
                    Me.TextboxMinDistTaxRate.Text = Me.MinDistributionTaxRate
                    Me.TextboxMinDistAmount.Text = "0.00"
                    Me.TextboxMinDistTaxable.Text = "0.00"
                    Me.TextboxMinDistNet.Text = "0.00"
                    Me.LabelRequiredMinDisbAmount.Visible = False
                End If


            Else
                'Me.TextboxMinDistTaxRate.Text = Me.MinDistributionTaxRate
                Me.TextboxMinDistAmount.Text = "0.00"
                Me.TextboxMinDistTaxable.Text = "0.00"
                Me.TextboxMinDistNet.Text = "0.00"
                Me.LabelRequiredMinDisbAmount.Visible = False
            End If

            'Me.TextboxMinDistTaxRate.ReadOnly = True
            Me.TextboxMinDistAmount.ReadOnly = True
            Me.TextboxMinDistTaxable.ReadOnly = True
            Me.TextboxMinDistNet.ReadOnly = True

        Catch ex As Exception
            Throw
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
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", "Please Enter the Amount Desired.", MessageBoxButtons.Stop, True)
                Return 0
            End If

            If Convert.ToDecimal(l_RequestedAmount) > (Convert.ToDecimal(Me.HardshipAvailable) + Convert.ToDecimal(Me.inVolAcctsAvailable)) Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", "Requested Amount Exceeds Available Amount - Requested Amount has been Adjusted.", MessageBoxButtons.Stop, True)
                'Calculate total amount available
                Me.TextboxRequestAmount.Text = Convert.ToDecimal(Me.HardshipAvailable) + Convert.ToDecimal(Me.inVolAcctsAvailable)
                Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate

                'This function will set total values for other accounts.
                LoadValuesForProcessedAmounts(True)

                'Entire Hardship amount has been used. - Set value for TD alc
                Me.TextboxHardShipAmount.Text = Math.Round(Me.HardshipAvailable, 2).ToString("0.00")
                Me.TextboxHardShipTax.Text = Math.Round(Me.HardshipAvailable * (Me.TDTaxRate / 100.0), 2).ToString("0.00")
                Me.TextboxHardShipNet.Text = Math.Round(Me.HardshipAvailable - (Me.HardshipAvailable * (Me.TDTaxRate / 100.0)), 2).ToString("0.00")
                ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL - Start
                Me.IsHardShip = True
                Me.HardshipAmount = Me.TextboxHardShipAmount.Text
                Me.TDUsedAmount = Me.HardshipAmount
                ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL - End
                Return 0
            End If


            If l_RequestedAmount > Me.inVolAcctsAvailable Then
                Me.IsHardShip = True
                Me.HardshipAmount = l_RequestedAmount - Me.inVolAcctsAvailable
                'Me.TDUsedAmount = l_RequestedAmount - Me.inVolAcctsAvailable ''' ????? Preeti
                Me.TDUsedAmount = Me.HardshipAmount

            Else
                Me.IsHardShip = False
                Me.HardshipAmount = 0
                Me.TDUsedAmount = 0

                'This function will set total values for other accounts.

            End If

            'Need to calculate TD factor used.
            Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate
            Me.TextboxHardShipAmount.Text = Me.TDUsedAmount
            'Rahul
            Me.TextboxHardShipTax.Text = Math.Round(Me.TDUsedAmount * (Me.TDTaxRate / 100.0), 2).ToString("0.00")
            Me.TextboxHardShipNet.Text = Math.Round((Me.TDUsedAmount - Me.TextboxHardShipTax.Text), 2)
            'rahul
            'Load  values in grid
            LoadValuesForProcessedAmounts(False)

        Catch IcEx As InvalidCastException
            MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", "Invalid Amount entered. Please enter valid Amount.", MessageBoxButtons.Stop, True)
            Me.TextboxHardShipTaxRate.Text = "0.00"
        Catch ex As Exception
            Throw
        End Try
        ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL - End
    End Function
    ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL - LoadValuesForProcessedAmounts Function added
    'Which will calculate actually used values across account types and add it for calculation of the final taxable / nontaxable amount
    Private Function LoadValuesForProcessedAmounts(ByVal ExceedAmountFlag As Boolean)


        Dim l_Decimal_TempValue As Decimal = 0.0
        Dim l_Payee1DataTable As DataTable
        Dim txtFinalTaxableAmount As Double = 0.0
        Dim txtFinalNonTaxableAmount As Double = 0.0
        Dim txtFinalTDTaxableAmount As Double = 0.0

        objRefundProcess.CreatePayees()
        Me.SetPropertiesAfterCreatePayees()
        chkCreatePayeeProcessed = True

        l_Payee1DataTable = CType(Session("Payee1DataTable"), DataTable)
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
                Me.TextboxPayee2.ReadOnly = True
            End If

            TextboxTaxable.Text = Math.Round(txtFinalTaxableAmount, 2).ToString("0.00")
            TextboxNonTaxable.Text = Math.Round(txtFinalNonTaxableAmount, 2).ToString("0.00")

            Me.TextboxTax.Text = Math.Round(Math.Round(txtFinalTaxableAmount, 2) * (Me.TaxRate / 100.0), 2).ToString("0.00")
            Me.TextboxNet.Text = Double.Parse(TextboxTaxable.Text) + Double.Parse(TextboxNonTaxable.Text) - Double.Parse(TextboxTax.Text)

            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End

        End If

    End Function


    Private Function LoadDeafultValues()
        Try
            Me.TaxRate = 20.0
            Me.MinDistributionTaxRate = 20.0
            Me.TDTaxRate = 10.0
            'Changed On:28thFeb06 By:Preeti Added HardShipTaxRate
            Me.HardShipTaxRate = 10.0
            Me.DeductionsAmount = 0.0

            Me.TextboxTaxRate.Text = Me.TaxRate.ToString
            Me.TextboxMinDistTaxRate.Text = Me.MinDistributionTaxRate.ToString

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

            Me.TextBoxStatus.Text = Me.RefundStatus

            Me.TextBoxStatus.ReadOnly = True
            Me.TextBoxCurrentPIA.ReadOnly = True
            Me.TextBoxTerminationPIA.ReadOnly = True

            '' Now disable the Address Button.. 
            'Code edited by ashutosh 18/04/06
            ' Me.ButtonAddress.Visible = False
            Me.ButtonAddress.Enabled = False
            '******************************
            Me.TextboxAddress1.ReadOnly = True
            Me.TextboxAddress2.ReadOnly = True
            Me.TextboxAddress3.ReadOnly = True

            Me.TextboxCity1.ReadOnly = True
            Me.TextBoxState.ReadOnly = True
            Me.TextBoxCountry.ReadOnly = True
            Me.TextBoxZip.ReadOnly = True


            Me.TextboxPayee2.ReadOnly = True
            Me.TextboxPayee3.ReadOnly = True

            Me.TextboxDeductions.ReadOnly = True
            Me.TextboxTaxRate.ReadOnly = True

            Me.LabelRequiredMinDisbAmount.Visible = False

            Me.TextboxRequestAmount.Text = "0.00"
            Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate

            Me.TextboxHardShipAmount.Text = "0.00"
            Me.TextboxHardShipTax.Text = "0.00"
            Me.TextboxHardShipNet.Text = "0.00"
            Me.RequestedAmount = 0.0


            Me.TextboxTDAvailableAmount.ReadOnly = True
            Me.TextboxHardShipTaxRate.ReadOnly = True
            Me.TextboxHardShipAmount.ReadOnly = True
            Me.TextboxHardShipTax.ReadOnly = True
            Me.TextboxHardShipNet.ReadOnly = True

        Catch ex As Exception
            Throw
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
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'l_DataTable = CType(l_CacheManager.GetData("Payee1DataTable"), DataTable)
            'l_DataTable = CType(Session("Payee1DataTable"), DataTable)
            'Hafiz 03Feb06 Cache-Session
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start
            'Commented by preeti 
            'If Not l_DataTable Is Nothing Then

            '    'If l_DataTable.Rows.Count > 0 Then
            '    '    l_Decimal_Taxable = CType(l_DataTable.Compute("SUM (Taxable)", ""), Decimal)
            '    '    l_Decimal_NonTaxable = CType(l_DataTable.Compute("SUM (NonTaxable)", ""), Decimal)

            '    '    If Me.TaxRate = 0 Then
            '    '        l_Decimal_Tax = 0.0
            '    '    Else
            '    '        l_Decimal_Tax = (Me.TaxRate / 100) * l_Decimal_Taxable
            '    '    End If

            '    '    l_Decimal_Net = (l_Decimal_Taxable + l_Decimal_NonTaxable) - l_Decimal_Tax


            '    '    '  Me.TextboxTaxRate.Text = Me.TaxRate.ToString

            '    '    'Me.TextboxTaxable.Text = Math.Round(l_Decimal_Taxable, 2)
            '    '    'Me.TextboxNonTaxable.Text = Math.Round(l_Decimal_NonTaxable, 2)
            '    '    'Me.TextboxTax.Text = Math.Round(l_Decimal_Tax, 2)
            '    '    'Me.TextboxNet.Text = Math.Round(l_Decimal_Net, 2)

            '    'Else
            '    '    Me.TextboxTaxRate.Text = Me.TaxRate
            '    '    Me.TextboxTaxable.Text = "0.00"
            '    '    Me.TextboxNonTaxable.Text = "0.00"
            '    '    Me.TextboxTax.Text = "0.00"
            '    '    Me.TextboxNet.Text = "0.00"
            '    'End If


            'Else
            'Me.TextboxTaxRate.Text = Me.TaxRate
            'Me.TextboxTaxable.Text = "0.00"
            'Me.TextboxNonTaxable.Text = "0.00"
            'Me.TextboxTax.Text = "0.00"
            'Me.TextboxNet.Text = "0.00"
            'End If
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End
            If Me.RadioButtonRolloverAll.Checked Then
                Me.TextboxTax.Text = "0.00"
            End If


            Me.TextboxTaxable.ReadOnly = True
            Me.TextboxNonTaxable.ReadOnly = True
            Me.TextboxTax.ReadOnly = True
            Me.TextboxNet.ReadOnly = True

        Catch ex As Exception
            Throw
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
            l_DataTable = CType(Session("Payee2DataTable"), DataTable)
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
            Throw
        End Try
    End Function
    Private Function LoadPayeesDataGrid()

        Dim l_DataTable As DataTable

        Try

            l_DataTable = CType(Session("Payee1DataTable"), DataTable)
       
            'If this column does'nt exist then only add it else not
            Dim I As Integer
            Dim ColumnFound As Boolean = False
            For I = 0 To Me.DataGridPayee1.Columns.Count - 1
                If Me.DataGridPayee1.Columns(I).HeaderText = "Use" Then
                    ColumnFound = True
                    Exit For
                End If
            Next

            If ColumnFound = False Then
                m_DataGridCheckBox = New Infotech.DataGridCheckBox.CheckBoxColumn(False, "Use")
                m_DataGridCheckBox.HeaderText = "Use"
                m_DataGridCheckBox.DataField = "Use"
                m_DataGridCheckBox.AutoPostBack = False
                Me.DataGridPayee1.Columns.Add(m_DataGridCheckBox)

            End If


            Me.DataGridPayee1.DataSource = l_DataTable
            Me.DataGridPayee1.DataBind()

           
            l_DataTable = CType(Session("Payee2DataTable"), DataTable)
          
            Me.DatagridPayee2.DataSource = l_DataTable
            Me.DatagridPayee2.DataBind()

           
            l_DataTable = CType(Session("Payee3DataTable"), DataTable)
           
            Me.DatagridPayee3.DataSource = l_DataTable
            Me.DatagridPayee3.DataBind()


        Catch ex As Exception
            Throw
        End Try


    End Function

    Private Function CalculateTotal(Optional ByVal parameterDataTable As DataTable = Nothing, Optional ByVal parameterNeedTotalAmount As Boolean = False)

       
        Dim l_CalculatedDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_BooleanFlag As Boolean = False

        Try

          

            If Not parameterDataTable Is Nothing Then
                l_CalculatedDataTable = parameterDataTable
            Else
               
                l_CalculatedDataTable = Session("CalculatedDataTable")

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


                If parameterNeedTotalAmount = True Then
                    Me.TotalAvailable = IIf(l_DataRow("Total").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("Total"), Decimal))
                End If

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
            Throw
        End Try

    End Function


    Private Function CopyAccountContributionTableForCurrentAccounts()

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_AccountContributionTable As DataTable
        Dim l_CalculationDataTable As DataTable
        Dim l_DataColumn As DataColumn
        Dim l_DataRow As DataRow

        Try

            '' Get the Account contribution Table from Cache.
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'Hafiz 03Feb06 Cache-Session

            '' Take the Contribution Table, which is available in Cache
            'Hafiz 03Feb06 Cache-Session
            'l_AccountContributionTable = l_CacheManager.GetData("AccountContribution")
            l_AccountContributionTable = Session("AccountContribution")
            'Hafiz 03Feb06 Cache-Session

            If Not l_AccountContributionTable Is Nothing Then

                l_CalculationDataTable = l_AccountContributionTable.Clone

                'Copy all Values into Calculation Table 

                For Each l_DataRow In l_AccountContributionTable.Rows

                    If Not (l_DataRow("AccountType").GetType.ToString = "System.DBNull") Then

                        If Not (CType(l_DataRow("AccountType"), String) = "Total") Then
                            l_CalculationDataTable.ImportRow(l_DataRow)
                        End If

                        If (CType(l_DataRow("AccountType"), String) = "Total") Then
                            Me.TotalRefundAmount = IIf(l_DataRow("Emp.Total").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("Emp.Total"), Decimal)) + IIf(l_DataRow("YMCATotal").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("YMCATotal"), Decimal))
                            Me.PersonalAmount = IIf(l_DataRow("Emp.Total").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("Emp.Total"), Decimal))
                        End If

                    End If
                Next

                '' add the copied table to Cache.
                'Hafiz 03Feb06 Cache-Session
                'l_CacheManager.Add("CalculatedDataTableForCurrentAccounts", l_CalculationDataTable)
                Session("CalculatedDataTableForCurrentAccounts") = l_CalculationDataTable
                'Hafiz 03Feb06 Cache-Session

            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub LoadPIA()

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_String_PIA As String

        Dim l_decimal_CurrentPIA As Decimal
        Dim l_decimal_PIATermination As Decimal

        Try
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'Me.TerminationPIA = CType(l_CacheManager.GetData("TerminationPIA"), Decimal)
            'Me.CurrentPIA = CType(l_CacheManager.GetData("CurrentPIA"), Decimal)

            Me.TerminationPIA = CType(Session("TerminationPIA"), Decimal)
            Me.CurrentPIA = CType(Session("CurrentPIA"), Decimal)
            'Hafiz 03Feb06 Cache-Session

            Me.TextBoxTerminationPIA.Text = Math.Round(Me.TerminationPIA, 2).ToString("0.00")
            Me.TextBoxCurrentPIA.Text = Math.Round(Me.CurrentPIA, 2).ToString("0.00")

        Catch ex As Exception
            Throw ex
        End Try

    End Sub


    Private Sub TabStripVoluntaryRefund_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStripVoluntaryRefund.SelectedIndexChange
        Me.MultiPageVoluntaryRefund.SelectedIndex = Me.TabStripVoluntaryRefund.SelectedIndex
        ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL start
        If MultiPageVoluntaryRefund.SelectedIndex = 1 Then
            TextboxRequestAmount.TabIndex = 0
        End If
        ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL end
    End Sub




    Private Function LoadRequestedAccounts()

        Dim l_DataTable As DataTable
        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session

        Try
            If Me.SessionRefundRequestID <> String.Empty Then

                l_DataTable = Infotech.YmcaBusinessObject.RefundRequest.GetRefundRequestsDetails(Me.SessionRefundRequestID)

                Me.CalculateTotal(l_DataTable)

                Me.DataGridRequestedAccts.DataSource = l_DataTable
                Me.DataGridRequestedAccts.DataBind()

                '' Load into Cache 
                'Hafiz 03Feb06 Cache-Session
                'l_CacheManager = CacheFactory.GetCacheManager
                'l_CacheManager.Add("RequestedAccounts", l_DataTable)
                Session("RequestedAccounts") = l_DataTable
                'Hafiz 03Feb06 Cache-Session

                Me.SetSelectedIndex(Me.DataGridRequestedAccts, l_DataTable)

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function LoadAccountContribution()

        Dim l_DataTable As DataTable
        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session

        Try

            '' Get the DataSet from the Cache, bcos this DataSet is already in the Cache.

            ''Contribution
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'l_DataTable = CType(l_CacheManager.GetData("AccountContribution"), DataTable)
            l_DataTable = CType(Session("AccountContribution"), DataTable)
            'Hafiz 03Feb06 Cache-Session
            Me.CalculateTotal(l_DataTable)
            Me.DatagridCurrentAccts.DataSource = l_DataTable
            Me.DatagridCurrentAccts.DataBind()

            ' To set the selected  Index so that we can show total row in Bold Letters. :)
            Me.SetSelectedIndex(Me.DatagridCurrentAccts, l_DataTable)


            '' Popoulate Non Contribution
            'Hafiz 03Feb06 Cache-Session
            'l_DataTable = CType(l_CacheManager.GetData("AccountContribution_NonFund"), DataTable)
            l_DataTable = CType(Session("AccountContribution_NonFund"), DataTable)
            'Hafiz 03Feb06 Cache-Session
            Me.CalculateTotal(l_DataTable)
            Me.DatagridNonFundedContributions.DataSource = l_DataTable
            Me.DatagridNonFundedContributions.DataBind()

            Me.SetSelectedIndex(Me.DatagridNonFundedContributions, l_DataTable)



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
            e.Item.Cells(0).Visible = False
            e.Item.Cells(11).Visible = False
            'e.Item.Cells(1).Visible = False
            'e.Item.Cells(11).Visible = False
            'e.Item.Cells(12).Visible = False
            ''e.Item.Cells(13).Visible = False

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DatagridCurrentAccts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridCurrentAccts.ItemDataBound
        Try

            e.Item.Cells(1).Visible = False
            e.Item.Cells(10).Visible = True
            e.Item.Cells(11).Visible = False
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

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_CalculatedDataTable As DataTable

        Try
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'l_CalculatedDataTable = CType(l_CacheManager.GetData("CalculatedDataTableForCurrentAccounts"), DataTable)
            l_CalculatedDataTable = CType(Session("CalculatedDataTableForCurrentAccounts"), DataTable)
            'Hafiz 03Feb06 Cache-Session
            Me.CalculateTotal(l_CalculatedDataTable)
            Me.DatagridCurrentAccts.DataSource = l_CalculatedDataTable
            Me.DatagridCurrentAccts.DataBind()

        Catch ex As Exception
            Throw
        End Try

    End Function

    'Private Function CreatePayees()

    '    'Hafiz 03Feb06 Cache-Session
    '    'Dim l_CacheManager As CacheManager
    '    'Hafiz 03Feb06 Cache-Session
    '    Dim l_CurrentDataTable As DataTable
    '    Dim l_PayeeDataTable As DataTable

    '    Dim l_Payee1DataTable As DataTable
    '    Dim l_Payee2DataTable As DataTable
    '    Dim l_Payee3DataTable As DataTable

    '    Dim l_PayeeDataRow As DataRow

    '    Dim l_AccountType As String

    '    Dim l_Decimal_PersonalPreTax As Decimal
    '    Dim l_Decimal_PersonalInterest As Decimal
    '    Dim l_Decimal_YMCAPreTax As Decimal
    '    Dim l_Decimal_YMCAInterest As Decimal

    '    Dim l_DataColumn As DataColumn




    '    'ThisForm.inTaxable = 0.0
    '    'ThisForm.inNonTaxable = 0.0
    '    'ThisForm.inGross = 0.0

    '    'ThisForm.inVolAcctsUsed = 0.0
    '    'ThisForm.inVolAcctsAvailable = 0.0
    '    Me.inTaxable = 0.0
    '    Me.inNonTaxable = 0.0
    '    Me.inGross = 0.0

    '    Me.inVolAcctsUsed = 0.0
    '    Me.inVolAcctsAvailable = 0.0


    '    Try
    '        '' this segment for Current Accounts
    '        'Hafiz 03Feb06 Cache-Session
    '        'l_CacheManager = CacheFactory.GetCacheManager
    '        'l_CurrentDataTable = CType(l_CacheManager.GetData("CalculatedDataTableForCurrentAccounts"), DataTable)
    '        l_CurrentDataTable = CType(Session("CalculatedDataTableForCurrentAccounts"), DataTable)
    '        'Hafiz 03Feb06 Cache-Session

    '        '' this segment for Setup the payees.
    '        'Hafiz 03Feb06 Cache-Session
    '        'l_PayeeDataTable = CType(l_CacheManager.GetData("AtsRefund"), DataTable)
    '        l_PayeeDataTable = CType(Session("AtsRefund"), DataTable)
    '        'Hafiz 03Feb06 Cache-Session

    '        If (Not l_CurrentDataTable Is Nothing) And (Not l_PayeeDataTable Is Nothing) Then

    '            l_Payee1DataTable = l_PayeeDataTable.Clone
    '            l_Payee2DataTable = l_PayeeDataTable.Clone
    '            l_Payee3DataTable = l_PayeeDataTable.Clone

    '            l_DataColumn = New DataColumn("Use", System.Type.GetType("System.Boolean"))
    '            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start
    '            l_DataColumn.DefaultValue = "False"
    '            '' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End

    '            l_Payee1DataTable.Columns.Add(l_DataColumn)

    '            l_DataColumn = New DataColumn("Use", System.Type.GetType("System.Boolean"))
    '            l_DataColumn.DefaultValue = "False"

    '            l_Payee2DataTable.Columns.Add(l_DataColumn)

    '            l_DataColumn = New DataColumn("Use", System.Type.GetType("System.Boolean"))
    '            l_DataColumn.DefaultValue = "False"

    '            l_Payee3DataTable.Columns.Add(l_DataColumn)


    '            For Each l_DataRow As DataRow In l_CurrentDataTable.Rows

    '                If Not l_DataRow("AccountType").GetType.ToString.Trim = "System.DBNull" Then

    '                    l_AccountType = CType(l_DataRow("AccountType"), String).Trim

    '                    If (l_AccountType = "") Or (l_AccountType = "Total") Then
    '                    Else

    '                        If l_AccountType = "TD" Or l_AccountType = "TM" Then
    '                        Else
    '                            If Me.IsExistInRequestedAccounts(l_AccountType.Trim.ToUpper) = True Then
    '                                ''l_Payee1DataTable.ImportRow(l_DataRow)

    '                                l_PayeeDataRow = l_Payee1DataTable.NewRow


    '                                If l_DataRow("Taxable").GetType.ToString = "System.DBNull" Then
    '                                    l_Decimal_PersonalPreTax = 0.0
    '                                Else
    '                                    l_Decimal_PersonalPreTax = CType(l_DataRow("Taxable"), Decimal)
    '                                End If

    '                                If l_DataRow("Interest").GetType.ToString = "System.DBNull" Then
    '                                    l_Decimal_PersonalInterest = 0.0
    '                                Else
    '                                    l_Decimal_PersonalInterest = CType(l_DataRow("Interest"), Decimal)
    '                                End If

    '                                If l_DataRow("YMCATaxable").GetType.ToString = "System.DBNull" Then
    '                                    l_Decimal_YMCAPreTax = 0.0
    '                                Else
    '                                    l_Decimal_YMCAPreTax = CType(l_DataRow("YMCATaxable"), Decimal)
    '                                End If

    '                                If l_DataRow("YMCAInterest").GetType.ToString = "System.DBNull" Then
    '                                    l_Decimal_YMCAInterest = 0.0
    '                                Else
    '                                    l_Decimal_YMCAInterest = CType(l_DataRow("YMCAInterest"), Decimal)
    '                                End If


    '                                l_PayeeDataRow("AcctType") = l_DataRow("AccountType")
    '                                l_PayeeDataRow("Taxable") = l_Decimal_PersonalInterest + l_Decimal_PersonalPreTax + l_Decimal_YMCAInterest + l_Decimal_YMCAPreTax
    '                                l_PayeeDataRow("NonTaxable") = l_DataRow("Non-Taxable")
    '                                l_PayeeDataRow("TaxRate") = Me.TaxRate
    '                                l_PayeeDataRow("Tax") = (l_Decimal_PersonalInterest + l_Decimal_PersonalPreTax + l_Decimal_YMCAInterest + l_Decimal_YMCAPreTax) * (Me.TaxRate / 100.0)
    '                                l_PayeeDataRow("Payee") = Me.TextBoxPayee1.Text
    '                                l_PayeeDataRow("FundedDate") = System.DBNull.Value
    '                                l_PayeeDataRow("RequestType") = Me.RefundType
    '                                l_PayeeDataRow("RefRequestsID") = Me.SessionRefundRequestID

    '                                l_PayeeDataRow("Use") = "True"

    '                                l_Payee1DataTable.Rows.Add(l_PayeeDataRow)


    '                                '' Assign the Values to the Session Varialbles 

    '                                'ThisForm.inTaxable = ThisForm.inTaxable + m.Taxable
    '                                'ThisForm.inNonTaxable = ThisForm.inNonTaxable + m.NonTaxable
    '                                'ThisForm.inGross = ThisForm.inGross + m.Taxable + m.NonTaxable
    '                                'ThisForm.inVolAcctsUsed = ThisForm.inVolAcctsUsed + m.Taxable + m.NonTaxable
    '                                'ThisForm.inVolAcctsAvailable = ThisForm.inVolAcctsAvailable + m.Taxable + m.NonTaxable

    '                                Me.inTaxable += CType(l_PayeeDataRow("Taxable"), Decimal)
    '                                Me.inNonTaxable += CType(l_PayeeDataRow("NonTaxable"), Decimal)
    '                                Me.inGross += CType(l_PayeeDataRow("Taxable"), Decimal) + CType(l_PayeeDataRow("NonTaxable"), Decimal)

    '                                Me.inVolAcctsUsed += CType(l_PayeeDataRow("Taxable"), Decimal) + CType(l_PayeeDataRow("NonTaxable"), Decimal)
    '                                Me.inVolAcctsAvailable += CType(l_PayeeDataRow("Taxable"), Decimal) + CType(l_PayeeDataRow("NonTaxable"), Decimal)

    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            Next

    '            'Hafiz 03Feb06 Cache-Session
    '            'l_CacheManager.Add("Payee1DataTable", l_Payee1DataTable)
    '            'l_CacheManager.Add("Payee2DataTable", l_Payee2DataTable)
    '            'l_CacheManager.Add("Payee3DataTable", l_Payee3DataTable)

    '            Session("Payee1DataTable") = l_Payee1DataTable
    '            Session("Payee2DataTable") = l_Payee2DataTable
    '            Session("Payee3DataTable") = l_Payee3DataTable
    '            'Hafiz 03Feb06 Cache-Session
    '        End If


    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Function


#Region " Load Personal Inforamation "




    Private Function LoadInformation()

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManger As CacheManager
        'Hafiz 03Feb06 Cache-Session

        Try
            'PersonInformation

            'Hafiz 03Feb06 Cache-Session
            'l_CacheManger = CacheFactory.GetCacheManager
            'l_DataSet = l_CacheManger.GetData("PersonInformation")
            l_DataSet = Session("PersonInformation")
            'Hafiz 03Feb06 Cache-Session

            If Not l_DataSet Is Nothing Then

                l_DataTable = l_DataSet.Tables("Member Details")

                If Not l_DataTable Is Nothing Then
                    If l_DataTable.Rows.Count > 0 Then
                        Me.LoadGeneralInfoToControls(l_DataTable.Rows.Item(0))
                    End If
                End If

                ' This segment for 
                'start of comment by hafiz on 27Jul2006 - YREN-2510
                'l_DataTable = l_DataSet.Tables("Member Employment")

                'If Not l_DataTable Is Nothing Then
                '    If l_DataTable.Rows.Count > 0 Then
                '        Me.LoadEmploymentDetails(l_DataTable.Rows.Item(0))
                '    End If
                'End If
                'end of comment by hafiz on 27Jul2006 - YREN-2510

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
                    Me.TextboxAddress1.Text = String.Empty
                Else
                    Me.TextboxAddress1.Text = CType(parameterDataRow("Address1"), String).Trim
                End If

                If (parameterDataRow("Address2").ToString = "") Then
                    Me.TextboxAddress2.Text = String.Empty
                Else
                    Me.TextboxAddress2.Text = CType(parameterDataRow("Address2"), String).Trim
                End If

                If (parameterDataRow("Address3").ToString = "") Then
                    Me.TextboxAddress3.Text = String.Empty
                Else
                    Me.TextboxAddress3.Text = CType(parameterDataRow("Address3"), String).Trim
                End If

                'If (parameterDataRow("StateType").ToString = "") Then
                '    Me.TextboxCity2.Text = String.Empty
                'Else
                '    Me.TextboxCity2.Text = CType(parameterDataRow("StateType"), String).Trim
                'End If

                If (parameterDataRow("City").ToString = "") Then
                    Me.TextboxCity1.Text = String.Empty
                Else
                    Me.TextboxCity1.Text = CType(parameterDataRow("City"), String).Trim
                End If

                If (parameterDataRow("CountryName").ToString = "") Then
                    Me.TextBoxCountry.Text = String.Empty
                Else
                    Me.TextBoxCountry.Text = CType(parameterDataRow("CountryName"), String).Trim
                End If

                'If (parameterDataRow("Zip").ToString = "") Then
                '    Me.TextboxCity3.Text = String.Empty
                'Else
                '    Me.TextboxCity3.Text = CType(parameterDataRow("Zip"), String).Trim
                'End If

                'Added By Ashutosh Patil as on 23-Apr-2007
                'YREN-3028,YREN-3029
                If (parameterDataRow("StateName").ToString = "") Then
                    Me.TextBoxState.Text = String.Empty
                Else
                    Me.TextBoxState.Text = CType(parameterDataRow("StateName"), String).Trim
                End If

                If parameterDataRow("CountryName").ToString = "CANADA" Then
                    l_str_zipcode = CType(parameterDataRow("Zip"), String).Trim
                    Me.TextBoxZip.Text = l_str_zipcode.Substring(0, 3) + " " + l_str_zipcode.Substring(3, 3)
                Else
                    Me.TextBoxZip.Text = CType(parameterDataRow("Zip"), String).Trim
                End If

                'If (parameterDataRow("Email").ToString = "") Then
                '    Me.TextBoxEmail.Text = String.Empty
                'Else
                '    Me.TextBoxEmail.Text = CType(parameterDataRow("Email"), String)
                'End If

                'If (parameterDataRow("PhoneNumber").ToString = "") Then
                '    Me.TextBoxTelephone.Text = String.Empty
                'Else
                '    Me.TextBoxTelephone.Text = CType(parameterDataRow("PhoneNumber"), String)
                'End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    'start of comment by hafiz on 27Jul2006 - YREN-2510
    'Private Function LoadEmploymentDetails(ByVal parameterDataRow As DataRow)
    '    Try

    '        If Not parameterDataRow Is Nothing Then

    '            If parameterDataRow("PersonID").GetType.ToString = "System.DBNull" Then
    '                Me.PersonID = String.Empty
    '            Else
    '                Me.PersonID = CType(parameterDataRow("PersonID"), String)
    '            End If

    '            If parameterDataRow("FundEventID").GetType.ToString = "System.DBNull" Then
    '                Me.FundID = String.Empty
    '            Else
    '                Me.FundID = CType(parameterDataRow("FundEventID"), String)
    '            End If

    '        End If

    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Function
    'end of comment by hafiz on 27Jul2006 - YREN-2510

    Private Function LoadGeneralInfoToControls(ByVal parameterDataRow As DataRow)
        Try

            If Not parameterDataRow Is Nothing Then

                'Me.TextBoxSSNo.Text = CType(parameterDataRow("SS No"), String)
                'Me.TextBoxLastName.Text = CType(parameterDataRow("Last Name"), String)
                'Me.TextBoxMiddleName.Text = CType(parameterDataRow("Middle Name"), String)
                'Me.TextBoxFirstName.Text = CType(parameterDataRow("First Name"), String)

                Me.TextBoxPayee1.Text = CType(parameterDataRow("Last Name"), String) & " " & CType(parameterDataRow("Middle Name"), String) & " " & CType(parameterDataRow("First Name"), String)
                Me.TextBoxPayee1.ReadOnly = True

                'Me.TextboxPayee2.Text = ""
                'Me.TextboxPayee3.Text = ""

                If (parameterDataRow("VestingDate").GetType.ToString = "System.DBNull") Then
                    Me.IsVested = False
                Else
                    Me.IsVested = True
                End If

                If (parameterDataRow("StatusType").GetType.ToString = "System.DBNull") Then
                    Me.IsTerminated = False
                    Me.IsRetiredActive = False
                ElseIf (CType(parameterDataRow("StatusType"), String).Trim = "RA") Then
                    Me.IsRetiredActive = True
                    Me.IsTerminated = False

                Else

                    If ((CType(parameterDataRow("StatusType"), String).Trim = "AE") Or ((CType(parameterDataRow("StatusType"), String).Trim = "PE"))) Then
                        Me.IsTerminated = False
                        Me.IsRetiredActive = False
                    Else
                        Me.IsTerminated = True
                        Me.IsRetiredActive = False
                    End If
                End If

                If (parameterDataRow("BirthDate").GetType.ToString = "System.DBNull") Then
                    Me.PersonAge = 0.0
                ElseIf (parameterDataRow("DeathDate").GetType.ToString = "System.DBNull") Then
                    Me.PersonAge = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), String.Empty)
                Else
                    Me.PersonAge = Me.CalculateAge(CType(parameterDataRow("BirthDate"), String), CType(parameterDataRow("DeathDate"), String))
                End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function CalculateAge(ByVal parameterDOB As String, ByVal parameterDOD As String) As Decimal
        Try

            If parameterDOB = String.Empty Then Return 0

            If parameterDOD = String.Empty Then
                Return ((DateDiff(DateInterval.Month, CType(parameterDOB, DateTime), Now.Date)) / 12.0)
            Else
                Return ((DateDiff(DateInterval.Year, CType(parameterDOB, DateTime), CType(parameterDOD, DateTime))) / 12.0)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function



#End Region

#Region " Voluntry Refund For Current Accounts "

    Private Function DoVoluntryRefundForCurrentAccounts()

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_ContributionDataTable As DataTable
        Dim l_DataRow As DataRow

        Dim l_UserSide As Boolean
        Dim l_YMCASide As Boolean

        Dim l_AccountType As String


        Try

            '' Get the Account contribution Table from Cache.
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'l_ContributionDataTable = l_CacheManager.GetData("CalculatedDataTableForCurrentAccounts")
            l_ContributionDataTable = Session("CalculatedDataTableForCurrentAccounts")
            'Hafiz 03Feb06 Cache-Session

            If Not l_ContributionDataTable Is Nothing Then

                'For Each l_DataRow In l_ContributionDataTable.Rows

                '        'Reset the flag
                '        l_UserSide = True
                '        l_YMCASide = True


                'If Not (l_DataRow("AccountType").GetType.ToString = "System.DBNull") Then

                '            If Not (((CType(l_DataRow("AccountType"), String).Trim = "Total")) Or (((CType(l_DataRow("AccountType"), String).Trim = "")))) Then


                '                l_AccountType = CType(l_DataRow("AccountType"), String).Trim.ToUpper

                '                If Me.IsBasicAccount(l_DataRow) Then
                '                    l_UserSide = False
                '                    l_YMCASide = False
                '                End If

                '                Select Case l_AccountType

                '                    Case "AP"
                '                        l_UserSide = True
                '                        l_YMCASide = False


                '                    Case "TD"
                '                        l_UserSide = False
                '                        l_YMCASide = False

                '                        If Me.IsTerminated Or Me.PersonAge >= 59.5 Then
                '                            l_UserSide = True
                '                            l_YMCASide = False
                '                        End If

                '                    Case "TM"
                '                        l_UserSide = True
                '                        l_YMCASide = False


                '                        If Me.IsTerminated Then
                '                            l_UserSide = True
                '                            l_YMCASide = True
                '                        End If

                '                        If Me.PersonAge >= 59.5 Then
                '                            l_UserSide = True
                '                            l_YMCASide = False
                '                        End If


                '                    Case "RP"
                '                        l_UserSide = True
                '                        l_YMCASide = False

                '                    Case "RT"
                '                        l_UserSide = True
                '                        l_YMCASide = False

                '                    Case "AM"
                '                        l_UserSide = True
                '                        l_YMCASide = False


                '                        If (Me.IsTerminated) Then
                '                            l_UserSide = True
                '                            l_YMCASide = True

                '                        End If

                '                    Case "SR"
                '                        l_UserSide = False
                '                        l_YMCASide = False

                '                End Select

                '                If Me.IsTerminated = False Then
                '                    l_YMCASide = False
                '                End If



                '                ''Modify the values

                '                If l_UserSide Then

                '                    If l_YMCASide = False Then

                '                        l_DataRow("YMCATaxable") = "0.00"
                '                        l_DataRow("YMCAInterest") = "0.00"
                '                        l_DataRow("Total") = CType(l_DataRow("Total"), Decimal) - CType(l_DataRow("YMCATotal"), Decimal)
                '                        l_DataRow("YMCATotal") = "0.00"

                '                    End If

                '                    'l_DataRow("Selected") = "True"

                '                Else
                '                    l_DataRow.Delete()
                '                End If

                '            Else
                '                'l_DataRow.Delete()
                '            End If

                'End If ' Main If...

                'Next

                l_ContributionDataTable.AcceptChanges()

                'Hafiz 03Feb06 Cache-Session
                'l_CacheManager.Add("CalculatedDataTableForCurrentAccounts", l_ContributionDataTable)
                Session("CalculatedDataTableForCurrentAccounts") = l_ContributionDataTable
                'Hafiz 03Feb06 Cache-Session

                Me.CalculateTotal(l_ContributionDataTable, True)

                Me.LoadCalculatedTableForCurrentAccounts()
                Me.SetSelectedIndex(Me.DatagridCurrentAccts, l_ContributionDataTable)

            End If


        Catch ex As Exception
            Throw
        End Try

    End Function

#End Region


    Private Sub DataGridPayee1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridPayee1.ItemDataBound
        Try
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start

            ''e.Item.Cells(3).Visible = False
            'e.Item.Cells(4).Visible = False
            'e.Item.Cells(5).Visible = False
            'e.Item.Cells(6).Visible = False
            'e.Item.Cells(7).Visible = False
            'e.Item.Cells(8).Visible = False
            'e.Item.Cells(9).Visible = False
            'e.Item.Cells(10).Visible = False

            If e.Item.ItemType = ListItemType.Header Then
                e.Item.Cells(0).Text = "AcctType"
                e.Item.Cells(1).Text = "Taxable"
                e.Item.Cells(2).Text = "NonTaxable"
            End If

            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End


        Catch ex As Exception
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start
            'Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End
        End Try
    End Sub

    Private Sub DatagridPayee2_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridPayee2.ItemDataBound
        Try
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start

            'e.Item.Cells(3).Visible = False
            'e.Item.Cells(4).Visible = False
            'e.Item.Cells(5).Visible = False
            'e.Item.Cells(6).Visible = False
            'e.Item.Cells(7).Visible = False
            'e.Item.Cells(8).Visible = False
            'e.Item.Cells(9).Visible = False
            If e.Item.ItemType = ListItemType.Header Then
                e.Item.Cells(0).Text = "AcctType"
                e.Item.Cells(1).Text = "Taxable"
                e.Item.Cells(2).Text = "NonTaxable"
            End If
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End


        Catch ex As Exception
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start
            'Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End
        End Try
    End Sub

    Private Sub DatagridPayee3_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridPayee3.ItemDataBound
        Try
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start
            'e.Item.Cells(3).Visible = False
            'e.Item.Cells(4).Visible = False
            'e.Item.Cells(5).Visible = False
            'e.Item.Cells(6).Visible = False
            'e.Item.Cells(7).Visible = False
            'e.Item.Cells(8).Visible = False
            'e.Item.Cells(9).Visible = False

            If e.Item.ItemType = ListItemType.Header Then
                e.Item.Cells(0).Text = "AcctType"
                e.Item.Cells(1).Text = "Taxable"
                e.Item.Cells(2).Text = "NonTaxable"
            End If
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End

        Catch ex As Exception
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start
            'Throw
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End
        End Try
    End Sub

    Private Sub TextboxTaxRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxTaxRate.TextChanged

        Dim l_TaxRateInteger As Integer

        Try

            If Me.TextboxTaxRate.Text.Trim = String.Empty Then
                Me.TextboxTaxRate.Text = 0
            End If

            Me.TaxRate = CType(Me.TextboxTaxRate.Text, Decimal)
            Me.TextboxTaxRate.Text = Me.TaxRate

            If Me.TaxRate > 100 Or Me.TaxRate < 20 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Invalid Tax Rate entered. Tax Rate should between 20% to 100%.", MessageBoxButtons.Stop, True)
                Me.TaxRate = 20
                Me.TextboxTaxRate.Text = Me.TaxRate
            End If

            Dim l_DataTable As DataTable
            l_DataTable = CType(Session("Payee1DataTable"), DataTable)
            Dim l_datarow As DataRow
            For Each l_datarow In l_DataTable.Rows
                l_datarow("TaxRate") = Me.TaxRate
            Next
            Me.LoadPayee1ValuesIntoControls()
            Me.DoFinalCalculation()
        Catch caEx As System.InvalidCastException
            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Invalid Tax Rate entered. Please enter the Valid Tax Rate.", MessageBoxButtons.Stop, True)
            Me.TaxRate = 20
            Me.TextboxTaxRate.Text = Me.TaxRate

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RadioButtonNone_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonNone.CheckedChanged
        If Me.RadioButtonNone.Checked = True Then
            Me.DoNone()
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start
            'Me.TextboxPayee2.ReadOnly = True
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End
        End If
    End Sub

    Private Sub RadioButtonRolloverAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonRolloverAll.CheckedChanged
        If Me.RadioButtonRolloverAll.Checked = True Then
            Me.DoRolloverAll()
            Me.TextboxPayee2.ReadOnly = False
        End If
    End Sub

    Private Sub RadioButtonTaxableOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonTaxableOnly.CheckedChanged
        If Me.RadioButtonTaxableOnly.Checked = True Then
            Me.DoTaxableOnly()
            Me.TextboxPayee2.ReadOnly = False
        End If
    End Sub
    'dO NONE for hardship is not being integrated for the moment
    ' we will test if it works fine with regular one.
    Private Function DoNone()

        Dim l_PayeeDataTable As DataTable

        Try
            If chkCreatePayeeProcessed = False Then
                objRefundProcess.CreatePayees()
                Me.SetPropertiesAfterCreatePayees()
            End If
          
            Dim l_Payee1DataTable As DataTable
            Dim l_Payee1TempDataTable As New DataTable

            l_Payee1DataTable = CType(Session("Payee1DataTable"), DataTable)
          

            l_Payee1TempDataTable = l_Payee1DataTable.Clone
            Dim l_DataRow As DataRow
            'Copying data from table of Payee1 to Payee2 & 3
            For Each l_DataRow In l_Payee1DataTable.Rows
                l_Payee1TempDataTable.ImportRow(l_DataRow)
            Next

            
            Session("Payee1DataTable") = l_Payee1DataTable
            Session("Payee1TempDataTable") = l_Payee1TempDataTable

            If (RadioButtonRolloverYes.Checked = True And RadioButtonNone.Checked = True) Then
                TextboxPayee2.ReadOnly = False 'commented by Preeti
            End If

            Me.LoadPayeesDataGrid()
            Me.LoadPayee1ValuesIntoControls()
            Me.DoFinalCalculation()

            Me.EnableDisableSaveButton()

        Catch ex As Exception
            Throw
        End Try

    End Function


    Private Function DoRolloverAll()

      
        Dim l_Payee1DataTable As DataTable
        Dim l_Payee2DataTable As DataTable

      
        Dim l_Payee1TempDataTable As New DataTable
        Dim l_Payee3DataTable As DataTable
        TextboxPayee3.ReadOnly = True


        Try
          
            If chkCreatePayeeProcessed = False Then
                objRefundProcess.CreatePayees()
                Me.SetPropertiesAfterCreatePayees()
            End If


            l_Payee1DataTable = CType(Session("Payee1DataTable"), DataTable)
            l_Payee2DataTable = CType(Session("Payee2DataTable"), DataTable)

            l_Payee3DataTable = CType(Session("Payee3DataTable"), DataTable)

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



                Session("Payee1TempDataTable") = l_Payee1TempDataTable
                For Each l_DataRow As DataRow In l_Payee1DataTable.Rows
                    l_DataRow("Taxable") = "0.00"
                    l_DataRow("NonTaxable") = "0.00"
                Next

                '' Just clear the Tax & Taxrate in the Payee 3 datatable.
                For Each l_DataRow As DataRow In l_Payee3DataTable.Rows
                    l_DataRow("Taxable") = "0.00"
                    l_DataRow("NonTaxable") = "0.00"
                    l_DataRow("Tax") = "0.00"
                    l_DataRow("TaxRate") = "0.00"
                Next


                '' Just clear the Tax & Taxrate in the Payee 2 datatable.
                For Each l_DataRow As DataRow In l_Payee2DataTable.Rows
                    l_DataRow("TaxRate") = "0.00"
                    l_DataRow("Tax") = "0.00"
                Next

            End If
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start - Added this code Start   
            Session("Payee1DataTable") = l_Payee1DataTable
            Session("Payee2DataTable") = l_Payee2DataTable
            Session("Payee3DataTable") = l_Payee3DataTable
            Session("Payee1TempDataTable") = l_Payee1TempDataTable
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End - Added this code End
            Me.LoadPayeesDataGrid()
            Me.LoadPayee1ValuesIntoControls()
            Me.LoadPayee2ValuesIntoControls()
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start - Added this code Start   
            Me.LoadPayee3ValuesIntoControls()
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End - Added this code End
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

        Try
           
            If chkCreatePayeeProcessed = False Then
                objRefundProcess.CreatePayees()
                Me.SetPropertiesAfterCreatePayees()
            End If
            
            l_Payee1DataTable = CType(Session("Payee1DataTable"), DataTable)
            l_Payee2DataTable = CType(Session("Payee2DataTable"), DataTable)
            l_Payee3DataTable = CType(Session("Payee3DataTable"), DataTable)
          
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

                Dim l_DRow As DataRow
                For Each l_DRow In l_Payee2DataTable.Rows
                    l_Payee3DataTable.ImportRow(l_DRow)
                Next
                For Each l_DRow In l_Payee3DataTable.Rows
                    l_DRow("Taxable") = "0.00"
                    l_DRow("NonTaxable") = "0.00"
                Next
                TextboxPayee3.ReadOnly = False

            End If

            Session("Payee1DataTable") = l_Payee1DataTable
            Session("Payee2DataTable") = l_Payee2DataTable
            Session("Payee3DataTable") = l_Payee3DataTable
            Session("Payee1TempDataTable") = l_Payee1TempDataTable

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

    Private Function IsExistInRequestedAccounts(ByVal parameterAccountType As String) As Boolean

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_RequestedDataTable As DataTable

        Dim l_FoundRows() As DataRow
        Dim l_QueryString As String

        Try
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'l_RequestedDataTable = CType(l_CacheManager.GetData("RequestedAccounts"), DataTable)
            l_RequestedDataTable = CType(Session("RequestedAccounts"), DataTable)
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
            e.Item.Cells(1).Visible = False
            'added by ruchi to hide the extra row in the deductions grid
            If e.Item.Cells(1).Text.Trim.ToUpper = "OTHER" Then
                e.Item.Visible = False
            End If
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

                        If l_DataGridItem.Cells.Item(3).Text.Trim = "" Then
                        Else
                            l_Decimal_Amount += CType(l_DataGridItem.Cells.Item(3).Text.Trim, Decimal)
                        End If

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
            Throw
        End Try
    End Sub

    Private Function DoFinalCalculation()

        Dim l_Decimal_Payee1Taxable As Decimal
        Dim l_Decimal_Payee1NonTaxable As Decimal
        Dim l_Decimal_Payee1Net As Decimal
        Dim l_Decimal_Payee1Tax As Decimal
        Dim l_Decimal_MinDisbAmount As Decimal
        Dim l_Decimal_MinDisbTax As Decimal
        Dim l_decimal_MinDisbNet As Decimal
        Dim l_Decimal_HardShipAmount As Decimal
        Dim l_Decimal_HardShipTax As Decimal
        Dim l_Decimal_HardShipNet As Decimal


        Try


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

            If Me.TextboxTax.Text.Trim.Length > 1 Then
                l_Decimal_Payee1Tax = CType(Me.TextboxTax.Text, Decimal)
            Else
                l_Decimal_Payee1Tax = 0.0
            End If

            If Me.TextboxNet.Text.Trim.Length > 1 Then
                l_Decimal_Payee1Net = CType(Me.TextboxNet.Text, Decimal)
            Else
                l_Decimal_Payee1Net = 0.0
            End If


            '' Get value for Hard Ship.. 


            If Me.TextboxHardShipAmount.Text.Trim.Length > 1 Then
                l_Decimal_HardShipAmount = CType(Me.TextboxHardShipAmount.Text, Decimal)
            Else
                l_Decimal_HardShipAmount = 0.0
            End If

            If Me.TextboxHardShipTax.Text.Trim.Length > 1 Then
                l_Decimal_HardShipTax = CType(Me.TextboxHardShipTax.Text, Decimal)
                l_Decimal_HardShipTax = Math.Round(l_Decimal_HardShipTax, 2)
            Else
                l_Decimal_HardShipTax = 0.0
            End If

            If Me.TextboxHardShipNet.Text.Trim.Length > 1 Then
                l_Decimal_HardShipNet = CType(Me.TextboxHardShipNet.Text, Decimal)
                l_Decimal_HardShipNet = Math.Round(l_Decimal_HardShipNet, 2)
            Else
                l_Decimal_HardShipNet = 0.0
            End If
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start
            'Only Final values are considered while calculation.
            Me.TextboxTaxableFinal.Text = Math.Round((l_Decimal_Payee1Taxable + l_Decimal_HardShipAmount), 2)
            Me.TextboxNonTaxableFinal.Text = Math.Round(l_Decimal_Payee1NonTaxable, 2)
            Me.TextboxTaxFinal.Text = Math.Round((l_Decimal_Payee1Tax + l_Decimal_HardShipTax), 2)
            Me.TextboxNetFinal.Text = Math.Round(((l_Decimal_Payee1Net + l_Decimal_HardShipNet) - Me.DeductionsAmount), 2)

            Me.TextboxTaxableFinal.ReadOnly = True
            Me.TextboxNonTaxableFinal.ReadOnly = True
            Me.TextboxTaxFinal.ReadOnly = True
            Me.TextboxNetFinal.ReadOnly = True
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End

        Catch IcEx As InvalidCastException
            Throw
        Catch ex As Exception
            Throw
        End Try


    End Function

    Private Function LoadRefundConfiguration()

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow
        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_stringCategory As String = "Refund"

        Try
            'commented BY Aparna 18/04/2007 
            ' l_DataTable = Infotech.YmcaBusinessObject.RefundRequest.GetRefundConfiguration

            'BY Aparna 18/04/2007   
            'Changed the proc used to get the Refund configuration -so tht it can be universally used
            l_DataTable = Infotech.YmcaBusinessObject.RefundRequest.GetConfigurationCategoryWise(l_stringCategory)

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
                Next
            End If

            '' This call to get the Account Break Down.. 

            l_DataTable = Infotech.YmcaBusinessObject.RefundRequest.GetAccountBreakDown()

            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'l_CacheManager.Add("AccountBreakDown", l_DataTable)
            Session("AccountBreakDown") = l_DataTable
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
        If Me.RadioButtonRolloverNo.Checked = False Then
            l_Boolean_QuestionAnswer = False
        End If

        ''
        If Me.RadioButtonAddnlWitholdingNo.Checked = False Then
            l_Boolean_QuestionAnswer = False
        End If

        Return l_Boolean_QuestionAnswer


    End Function
#Region "Commented Code for mIn distribution "
    'we will be using the Regular Refund fucntion for Min dist assuming it to be correct
    '#Region " Calculation of Minimum Distribution Amount "

    '    '*****************************************************************************************************************
    '    '* Determine if a Minimum Distribution is Required for this Refund
    '    '* 1. Must be Terminated
    '    '* 2. Must have attained the Minimum age (70.5) at this Writing
    '    '* 3. Must be Rolling Money Over
    '    '* 4. Today's Date Must be after March 31st in the Year Following the Termination
    '    '*****************************************************************************************************************

    '    Private Function CalculateMinimumDistributionAmount()

    '        Dim l_DecimalDistributionPeriod As Decimal

    '        Me.MinimumDistributionAmount = 0.0

    '        Try

    '            ' Check for Termination. This for Step 1.
    '            If Not Me.IsTerminated Then
    '                Return 0
    '            End If

    '            '' Check for Minimum age.
    '            If Me.PersonAge < Me.MinimumDistributedAge Then
    '                Return 0
    '            End If

    '            '' Check for the Date.
    '            If Me.CheckForTerminationDate = True Then

    '                '***********************************************************************************************
    '                '* Okay a Minimum Distribution is Required for this Person
    '                '* Let's go to the Table and find out how much
    '                '* Then Deduct it From the Payee's Accounts Until We Reach That Amount
    '                '***********************************************************************************************

    '                l_DecimalDistributionPeriod = Infotech.YmcaBusinessObject.RefundRequest.GetDistributionPeriod(Me.PersonAge)

    '                If Not Me.TextboxTaxable.Text.Trim.Length = 0 Then

    '                    If l_DecimalDistributionPeriod <> 0 Then
    '                        Me.MinimumDistributionAmount = (CType(Me.TextboxTaxable.Text.Trim, Decimal)) / l_DecimalDistributionPeriod
    '                    End If
    '                Else
    '                    Me.MinimumDistributionAmount = 0.0
    '                End If


    '                ''**********************************************************************************************
    '                '* Let's Take away taxable Monies from the current amounts table
    '                ''**********************************************************************************************

    '                Me.DoMinimumDistributionCalculation()


    '            End If
    '        Catch ex As Exception
    '            Throw
    '        End Try


    '    End Function
    'WE are nt using this function for min distribution calc assuming that the Regular refund function for the same
    'is correct.
    'Private Function DoMinimumDistributionCalculation() As String

    '    'Hafiz 03Feb06 Cache-Session
    '    'Dim l_CacheManger As CacheManager
    '    'Hafiz 03Feb06 Cache-Session

    '    Dim l_MinimumDistributionTable As DataTable
    '    Dim l_CurrentAccountDataTable As DataTable
    '    Dim l_DataTable As DataTable

    '    Dim l_Decimal_PersonalPreTax As Decimal
    '    Dim l_Decimal_PersoanlPostTax As Decimal
    '    Dim l_Decimal_PersonalInterest As Decimal
    '    Dim l_Decimal_PersonalTotal As Decimal

    '    Dim l_Decimal_YMCAPreTax As Decimal
    '    Dim l_Decimal_YMCAInterest As Decimal
    '    Dim l_Decimal_YMCATotal As Decimal

    '    Dim l_Decimal_TaxRate As Decimal

    '    Dim l_Decimal_MinimumDistributionAmount As Decimal

    '    Dim l_Boolean_IsMinimumDistribution As Boolean
    '    Dim l_MinimumdistributionDataRow As DataRow


    '    Try
    '        'Hafiz 03Feb06 Cache-Session
    '        'l_CacheManger = CacheFactory.GetCacheManager
    '        'Hafiz 03Feb06 Cache-Session

    '        '' Get Schema Table to Keep the Minimum Distribution Account.
    '        'Hafiz 03Feb06 Cache-Session
    '        'l_DataTable = CType(l_CacheManger.GetData("AtsRefund"), DataTable)
    '        l_DataTable = CType(Session("AtsRefund"), DataTable)
    '        'Hafiz 03Feb06 Cache-Session

    '        If Not l_DataTable Is Nothing Then
    '            l_MinimumDistributionTable = l_DataTable.Clone
    '        Else
    '            Return 0
    '        End If


    '        '' Get the Current Account to calculate. 
    '        'Hafiz 03Feb06 Cache-Session
    '        'l_CurrentAccountDataTable = CType(l_CacheManger.GetData("CalculatedDataTableForCurrentAccounts"), DataTable)
    '        l_CurrentAccountDataTable = CType(Session("CalculatedDataTableForCurrentAccounts"), DataTable)
    '        'Hafiz 03Feb06 Cache-Session

    '        If Not l_CurrentAccountDataTable Is Nothing Then Return 0

    '        If Me.MinimumDistributionAmount = 0 Then Return 0

    '        '' Assign the values 
    '        l_Decimal_MinimumDistributionAmount = Me.MinimumDistributionAmount
    '        Me.MinimumDistributionAmount = 0.0


    '        For Each l_DataRow As DataRow In l_CurrentAccountDataTable.Rows

    '            If l_Decimal_MinimumDistributionAmount = 0 Then Exit For

    '            '' Get the all values to Caluclate.

    '            If l_DataRow("Taxable").GetType.ToString = "System.DBNull" Then
    '                l_Decimal_PersonalPreTax = CType(l_DataRow("Taxable"), Decimal)
    '            Else
    '                l_Decimal_PersonalPreTax = 0
    '            End If

    '            If l_DataRow("Non-Taxable").GetType.ToString = "System.DBNull" Then
    '                l_Decimal_PersoanlPostTax = CType(l_DataRow("Non-Taxable"), Decimal)
    '            Else
    '                l_Decimal_PersoanlPostTax = 0
    '            End If

    '            If l_DataRow("Interest").GetType.ToString = "System.DBNull" Then
    '                l_Decimal_PersonalInterest = CType(l_DataRow("Interest"), Decimal)
    '            Else
    '                l_Decimal_PersonalInterest = 0
    '            End If

    '            If l_DataRow("YMCATaxable").GetType.ToString = "System.DBNull" Then
    '                l_Decimal_YMCAPreTax = CType(l_DataRow("YMCATaxable"), Decimal)
    '            Else
    '                l_Decimal_YMCAInterest = 0
    '            End If

    '            If l_DataRow("YMCAInterest").GetType.ToString = "System.DBNull" Then
    '                l_Decimal_YMCAInterest = CType(l_DataRow("YMCAInterest"), Decimal)
    '            Else
    '                l_Decimal_YMCAInterest = 0
    '            End If


    '            '' Start calculation .. 

    '            If (l_Decimal_PersonalPreTax + l_Decimal_PersonalInterest + l_Decimal_YMCAPreTax + l_Decimal_YMCAInterest) > 0 Then ''Main If

    '                l_Boolean_IsMinimumDistribution = False

    '                '' For Personal Taxable
    '                If l_Decimal_MinimumDistributionAmount >= l_Decimal_PersonalPreTax Then

    '                    'm.lMinDist = .T.
    '                    'm.Taxable = m.PersonalPreTax
    '                    'ThisForm.inMinDistAmt = ThisForm.inMinDistAmt + m.PersonalPreTax
    '                    'lnMinAmt = lnMinAmt - m.PersonalPreTax

    '                    'REPLACE	PersonalTotal	WITH	c_Current.PersonalPreTax	+ 	;
    '                    '										c_Current.PersonalPostTax	+	;
    '                    '										c_Current.PersonalInterest, 	;
    '                    '			TotalTotal		WITH 	c_Current.PersonalTotal  	+	;
    '                    '										c_Current.YmcaTotal				;
    '                    '	IN c_Current

    '                    l_Boolean_IsMinimumDistribution = True

    '                    Me.MinimumDistributionAmount += l_Decimal_PersonalPreTax
    '                    l_Decimal_MinimumDistributionAmount -= l_Decimal_PersonalPreTax

    '                Else
    '                    'm.lMinDist = .T.
    '                    'ThisForm.inMinDistAmt = ThisForm.inMinDistAmt + lnMinAmt

    '                    'REPLACE PersonalPreTax WITH PersonalPreTax - lnMinAmt ;
    '                    '	IN c_Current
    '                    'm.Taxable = lnMinAmt
    '                    'lnMinAmt = 0.0

    '                    l_Boolean_IsMinimumDistribution = True
    '                    Me.MinimumDistributionAmount += l_Decimal_MinimumDistributionAmount

    '                    l_DataRow("Taxable") = l_Decimal_PersonalPreTax + l_Decimal_MinimumDistributionAmount

    '                    l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Non-Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Interest"), Decimal)

    '                    l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + _
    '                                                CType(l_DataRow("YMCATotal"), Decimal)



    '                    l_Decimal_PersonalPreTax = l_Decimal_MinimumDistributionAmount
    '                    l_Decimal_MinimumDistributionAmount = 0.0

    '                End If


    '                '' For Personal Interest
    '                If l_Decimal_MinimumDistributionAmount >= l_Decimal_PersonalInterest Then

    '                    ' m.lMinDist = .T.
    '                    ' m.Taxable = m.Taxable + m.PersonalInterest
    '                    ' ThisForm.inMinDistAmt = ThisForm.inMinDistAmt + m.PersonalInterest
    '                    ' lnMinAmt = lnMinAmt - m.PersonalInterest

    '                    'REPLACE PersonalInterest	WITH 0.00	;
    '                    '	IN c_Current

    '                    'REPLACE	PersonalTotal	WITH	c_Current.PersonalPreTax	+	;
    '                    '										c_Current.PersonalPostTax	+	;
    '                    '										c_Current.PersonalInterest 	;
    '                    '			TotalTotal		WITH	c_Current.PersonalTotal		+	;
    '                    '										c_Current.YmcaTotal				;
    '                    '	IN c_Current

    '                    l_Boolean_IsMinimumDistribution = True
    '                    l_Decimal_PersonalPreTax += l_Decimal_PersonalInterest

    '                    Me.MinimumDistributionAmount += l_Decimal_PersonalInterest
    '                    l_Decimal_MinimumDistributionAmount -= l_Decimal_PersonalInterest

    '                    l_DataRow("Interest") = "0.00"

    '                    l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Non-Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Interest"), Decimal)

    '                    l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + _
    '                                                CType(l_DataRow("YMCATotal"), Decimal)


    '                Else

    '                    'm.lMinDist = .T.
    '                    'ThisForm.inMinDistAmt = ThisForm.inMinDistAmt + lnMinAmt

    '                    'REPLACE PersonalInterest WITH PersonalInterest - lnMinAmt ;
    '                    '	IN c_Current

    '                    'REPLACE	PersonalTotal	WITH 	c_Current.PersonalPreTax	+	;
    '                    '										c_Current.PersonalPostTax	+	;
    '                    '										c_Current.PersonalInterest 	;
    '                    '			TotalTotal		WITH	c_Current.PersonalTotal		+	;
    '                    '										c_Current.YmcaTotal				;
    '                    'IN c_Current

    '                    'm.Taxable = m.Taxable + lnMinAmt
    '                    'lnMinAmt = 0.0

    '                    l_Boolean_IsMinimumDistribution = True
    '                    Me.MinimumDistributionAmount += l_Decimal_MinimumDistributionAmount

    '                    l_DataRow("Interest") = l_Decimal_PersonalInterest - l_Decimal_MinimumDistributionAmount

    '                    l_DataRow("Emp.Total") = CType(l_DataRow("Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Non-Taxable"), Decimal) + _
    '                                                CType(l_DataRow("Interest"), Decimal)

    '                    l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)


    '                    l_Decimal_PersonalPreTax += l_Decimal_MinimumDistributionAmount
    '                    l_Decimal_MinimumDistributionAmount = 0.0

    '                End If


    '                ''For YMCA Taxable.

    '                If l_Decimal_MinimumDistributionAmount >= l_Decimal_YMCAPreTax Then
    '                    ' m.lMinDist = .T.
    '                    ' m.Taxable = m.Taxable + m.YmcaPreTax
    '                    ' ThisForm.inMinDistAmt = ThisForm.inMinDistAmt + m.YMCAPreTax
    '                    ' lnMinAmt = lnMinAmt - m.YmcaPreTax

    '                    ' REPLACE YmcaPreTax	WITH 0.00	;
    '                    '	IN c_Current

    '                    ' REPLACE	YmcaTotal	WITH c_Current.YmcaPreTax + c_Current.YmcaInterest,	;
    '                    '			TotalTotal	WITH c_Current.PersonalTotal  + c_Current.YmcaTotal	;
    '                    'IN c_Current

    '                    l_Boolean_IsMinimumDistribution = True
    '                    l_Decimal_PersonalPreTax += l_Decimal_YMCAPreTax
    '                    Me.MinimumDistributionAmount += l_Decimal_YMCAPreTax

    '                    l_Decimal_MinimumDistributionAmount -= l_Decimal_YMCAPreTax

    '                    l_DataRow("YMCATaxable") = "0.00"

    '                    l_DataRow("YMCATotal") = CType(l_DataRow("YMCATaxable"), Decimal) + CType(l_DataRow("YMCAInterest"), Decimal)
    '                    l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)

    '                Else

    '                    'm.lMinDist = .T.
    '                    'ThisForm.inMinDistAmt = ThisForm.inMinDistAmt + lnMinAmt

    '                    'REPLACE YmcaPreTax WITH YmcaPreTax - lnMinAmt ;
    '                    '	IN c_Current

    '                    'REPLACE	YMCATotal	WITH c_Current.YMCAPreTax + c_Current.YMCAInterest,	;
    '                    '			TotalTotal	WITH c_Current.PersonalTotal  + c_Current.YmcaTotal	;
    '                    '	IN c_Current

    '                    'm.Taxable = m.Taxable + lnMinAmt
    '                    'lnMinAmt = 0.0

    '                    l_Boolean_IsMinimumDistribution = True
    '                    Me.MinimumDistributionAmount += l_Decimal_MinimumDistributionAmount

    '                    l_DataRow("YMCATaxable") = l_Decimal_YMCAPreTax - l_Decimal_MinimumDistributionAmount

    '                    l_DataRow("YMCATotal") = CType(l_DataRow("YMCATaxable"), Decimal) + CType(l_DataRow("YMCAInterest"), Decimal)
    '                    l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)

    '                    l_Decimal_PersonalPreTax += l_Decimal_MinimumDistributionAmount
    '                    l_Decimal_MinimumDistributionAmount = 0.0

    '                End If

    '                If l_Decimal_MinimumDistributionAmount >= l_Decimal_YMCAInterest Then
    '                    ' m.lMinDist	= .T.
    '                    ' m.Taxable = m.Taxable + m.YMCAInterest
    '                    ' ThisForm.inMinDistAmt = ThisForm.inMinDistAmt + m.YMCAInterest
    '                    ' lnMinAmt = lnMinAmt - m.YMCAInterest

    '                    ' REPLACE YMCAInterest	WITH 0.00	;
    '                    ' IN c_Current

    '                    ' REPLACE	YMCATotal	WITH c_Current.YMCAPreTax + c_Current.YMCAInterest,	;
    '                    '			TotalTotal	WITH c_Current.PersonalTotal  + c_Current.YmcaTotal	;
    '                    ' IN c_Current

    '                    l_Boolean_IsMinimumDistribution = True

    '                    l_Decimal_PersonalPreTax += l_Decimal_YMCAInterest
    '                    Me.MinimumDistributionAmount += l_Decimal_YMCAInterest
    '                    l_Decimal_MinimumDistributionAmount -= l_Decimal_YMCAInterest

    '                    l_DataRow("YMCAInterest") = "0.00"

    '                    l_DataRow("YMCATotal") = CType(l_DataRow("YMCATaxable"), Decimal) + CType(l_DataRow("YMCAInterest"), Decimal)
    '                    l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)


    '                Else

    '                    ' m.lMinDist = .T.
    '                    ' ThisForm.inMinDistAmt = ThisForm.inMinDistAmt + lnMinAmt

    '                    ' REPLACE YMCAInterest WITH YMCAInterest - lnMinAmt ;
    '                    '	IN c_Current

    '                    ' REPLACE	YMCATotal	WITH c_Current.YMCAPreTax + c_Current.YMCAInterest,	;
    '                    '			TotalTotal	WITH c_Current.PersonalTotal  + c_Current.YmcaTotal	;
    '                    '	IN c_Current

    '                    ' m.Taxable = m.Taxable + lnMinAmt
    '                    ' lnMinAmt = 0.0

    '                    l_Boolean_IsMinimumDistribution = True
    '                    Me.MinimumDistributionAmount += l_Decimal_MinimumDistributionAmount

    '                    l_DataRow("YMCAInterest") = l_Decimal_YMCAInterest - l_Decimal_MinimumDistributionAmount

    '                    l_DataRow("YMCATotal") = CType(l_DataRow("YMCATaxable"), Decimal) + CType(l_DataRow("YMCAInterest"), Decimal)
    '                    l_DataRow("Total") = CType(l_DataRow("Emp.Total"), Decimal) + CType(l_DataRow("YMCATotal"), Decimal)

    '                    l_Decimal_PersonalPreTax += l_Decimal_MinimumDistributionAmount

    '                End If

    '                '' This Values has to be updated in the Table.

    '                If l_Boolean_IsMinimumDistribution = True Then

    '                    ''Get TaxRate

    '                    l_MinimumdistributionDataRow = l_MinimumDistributionTable.NewRow

    '                    'm.TaxRate = ThisForm.inMinDistTaxRate
    '                    'm.Tax = ROUND(m.Taxable * (m.TaxRate / 100.0), 2)
    '                    'm.NonTaxable = 0.0
    '                    'm.AcctType = UPPER(ALLTRIM(c_Current.AcctType))
    '                    'M.Payee = ThisForm.icPayee1
    '                    'm.FundedDate= .NULL.
    '                    'm.RefRequestsID = r_RefREquests.UniqueID

    '                    l_MinimumdistributionDataRow("Taxable") = l_Decimal_PersonalPreTax
    '                    l_MinimumdistributionDataRow("TaxRate") = Me.MinDistributionTaxRate
    '                    l_MinimumdistributionDataRow("Tax") = Math.Round(l_Decimal_PersonalPreTax * (Me.MinDistributionTaxRate / 100.0))
    '                    l_MinimumdistributionDataRow("NonTaxable") = "0.00"
    '                    l_MinimumdistributionDataRow("AcctType") = l_DataRow("AcctType")
    '                    l_MinimumdistributionDataRow("Payee") = Me.TextBoxPayee1.Text.Trim
    '                    l_MinimumdistributionDataRow("FundedDate") = System.DBNull.Value
    '                    l_MinimumdistributionDataRow("RefRequestsID") = Me.SessionRefundRequestID

    '                    l_MinimumdistributionDataRow("RequestType") = l_DataRow("RequestType")
    '                    l_MinimumdistributionDataRow("TransactID") = l_DataRow("TransactID")
    '                    l_MinimumdistributionDataRow("AnnuityBasisType") = l_DataRow("AnnuityBasisType")
    '                    l_MinimumdistributionDataRow("DisbursementID") = l_DataRow("DisbursementID")

    '                    l_MinimumDistributionTable.Rows.Add(l_MinimumdistributionDataRow)

    '                End If


    '            End If '' Main if

    '        Next

    '        'If lnMinAmt > THIS.inMinDistAmt Then
    '        'THISFORM.icMessage	=	"A Minimum Distribution of " + LTRIM(STR(THISFORM.inMinDistAmt.VALUE,8,2)) + CHR(13) + ;
    '        '								"is Required.. There isn't enough Taxable Money to Roll any over"
    '        '=MESSAGEBOX(THISFORM.icMessage,0,"Minimum Distrubtion Not Met")
    '        '                  End If

    '        'ThisForm.inMinDistTaxAmt = ROUND(THISFORM.inMinDistAmt * (THISFORM.inMinDistTaxRate / 100.0), 2)
    '        'ThisForm.inMinDistNet = THISFORM.inMinDistAmt - ThisForm.inMinDistTaxAmt

    '        If l_Decimal_MinimumDistributionAmount > Me.MinimumDistributionAmount Then

    '            '"A Minimum Distribution of " + LTRIM(STR(THISFORM.inMinDistAmt.VALUE,8,2)) + CHR(13) + ;
    '            '								"is Required.. There isn't enough Taxable Money to Roll any over"
    '            '=MESSAGEBOX(THISFORM.icMessage,0,"Minimum Distrubtion Not Met")

    '            ''MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA YRS ", " Minimum Distrubtion Not Met.  A Minimum Distribution of " + Math.Round(Me.MinimumDistributionAmount, 2).ToString + " is Required.. There isn't enough Taxable Money to Roll any over", MessageBoxButtons.Stop, True)

    '            Return (" Minimum Distrubtion Not Met.  A Minimum Distribution of " + Math.Round(Me.MinimumDistributionAmount, 2).ToString + " is Required.. There isn't enough Taxable Money to Roll any over.")

    '        End If


    '        'Hafiz 03Feb06 Cache-Session
    '        'l_CacheManger = CacheFactory.GetCacheManager
    '        'l_CacheManger.Add("MinimumDistributionTable", l_MinimumDistributionTable)
    '        Session("MinimumDistributionTable") = l_MinimumDistributionTable
    '        'Hafiz 03Feb06 Cache-Session


    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function


    '    Private Function CheckForTerminationDate() As Boolean

    '        ' Rule : This segment is used to check for the termination Date 

    '        'Hafiz 03Feb06 Cache-Session
    '        'Dim l_CacheManager As CacheManager
    '        'Hafiz 03Feb06 Cache-Session
    '        Dim l_DataTable As DataTable = Nothing
    '        Dim l_DataRow As DataRow
    '        Dim l_DateTime As DateTime
    '        Dim l_TerminationDate As DateTime

    '        Try
    '            'Hafiz 03Feb06 Cache-Session
    '            'l_CacheManager = CacheFactory.GetCacheManager
    '            'Hafiz 03Feb06 Cache-Session

    '            '' Get employment Table from cache, which is stored while selecting the user.
    '            'Hafiz 03Feb06 Cache-Session
    '            'l_DataTable = CType(l_CacheManager.GetData("Member Employment"), DataTable)
    '            l_DataTable = CType(Session("Member Employment"), DataTable)
    '            'Hafiz 03Feb06 Cache-Session

    '            If l_DataTable Is Nothing Then Return False

    '            For Each l_DataRow In l_DataTable.Rows

    '                If Not l_DataRow("TermDate").GetType.ToString = "System.DBNull" Then

    '                    If DateTime.Compare(l_TerminationDate, CType(l_DataRow("TermDate"), DateTime)) < 0 Then
    '                        l_TerminationDate = CType(l_DataRow("TermDate"), DateTime)
    '                    End If

    '                End If

    '            Next

    '            If Now.Year >= l_TerminationDate.Year And Now.Month > 3 And Now.Day > 31 Then
    '                Return True
    '            Else
    '                Return False
    '            End If


    '        Catch ex As Exception
    '            Throw
    '        End Try

    '    End Function

    '#End Region

#End Region 'Commented Code for mIn distribution
    Private Function GetAddressID() As String

        Dim l_DataSet As DataSet
        Dim l_DataTable As DataTable
        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManger As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_DataRow As DataRow

        Try
            'PersonInformation
            'Hafiz 03Feb06 Cache-Session
            'l_CacheManger = CacheFactory.GetCacheManager
            'l_DataSet = l_CacheManger.GetData("PersonInformation")
            l_DataSet = Session("PersonInformation")
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




    






    Private Function HardShipCalculation()

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_CurrentAccountDataTable As DataTable

        Dim l_Decimal_PersonalTotal As Decimal
        Dim l_Decimal_YMCATotal As Decimal
        Dim l_Decimal_PersonalPreTax As Decimal
        Dim l_Decimal_Tax As Decimal

        Dim l_Payee1DataTable As DataTable

        Dim l_FoundRows() As DataRow
        Dim l_QueryString As String
        Dim l_Payee1DataRow As DataRow
        Dim l_index As Integer
        Dim l_CounterAmount As Decimal

        Try

            Me.IsHardShip = False

            If Me.HardshipAmount <= 0.0 Then Return 0

            If Me.TDUsedAmount <= 0.0 Then Return 0

            Me.IsHardShip = True

            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager
            'l_CurrentAccountDataTable = CType(l_CacheManager.GetData("CalculatedDataTableForCurrentAccounts"), DataTable)
            'l_Payee1DataTable = CType(l_CacheManager.GetData("Payee1DataTable"), DataTable)

            l_CurrentAccountDataTable = CType(Session("CalculatedDataTableForCurrentAccounts"), DataTable)
            l_Payee1DataTable = CType(Session("Payee1DataTable"), DataTable)
            'Hafiz 03Feb06 Cache-Session

            l_CounterAmount = 0.0

            If Not l_CurrentAccountDataTable Is Nothing Then


                For Each l_DataRow As DataRow In l_CurrentAccountDataTable.Rows

                    If Not l_DataRow("Emp.Total").GetType.ToString = "System.DBNull" Then
                        l_Decimal_PersonalTotal = CType(l_DataRow("Emp.Total"), Decimal)
                    Else
                        l_Decimal_PersonalTotal = 0.0
                    End If


                    If Not l_DataRow("YMCATotal").GetType.ToString = "System.DBNull" Then
                        l_Decimal_YMCATotal = CType(l_DataRow("YMCATotal"), Decimal)
                    Else
                        l_Decimal_YMCATotal = 0.0
                    End If

                    If Not l_DataRow("Taxable").GetType.ToString = "System.DBNull" Then
                        l_Decimal_PersonalPreTax = CType(l_DataRow("Taxable"), Decimal)
                    Else
                        l_Decimal_PersonalPreTax = 0.0
                    End If



                    If l_Decimal_PersonalTotal + l_Decimal_YMCATotal > 0.0 Then

                        '' If the Total is > o then Do update in the Payee1 DataTable 
                        If l_DataRow("AccountType") = "TD" Or l_DataRow("AccountType") = "TM" Then


                            l_QueryString = "AcctType = '" & l_DataRow("AccountType") & "'"

                            If Not l_Payee1DataTable Is Nothing Then

                                l_FoundRows = l_Payee1DataTable.Select(l_QueryString)



                                If l_FoundRows Is Nothing Or l_FoundRows.Length = 0 Then

                                    l_Payee1DataRow = l_Payee1DataTable.NewRow

                                    l_Payee1DataRow("Taxable") = "0.00"
                                    l_Payee1DataRow("Tax") = "0.00"
                                    If l_Decimal_PersonalPreTax < (Me.HardshipAmount - l_CounterAmount) Then
                                        l_Payee1DataRow("Taxable") = CType(l_Payee1DataRow("Taxable"), Decimal) + l_Decimal_PersonalPreTax
                                        l_CounterAmount += l_Decimal_PersonalPreTax
                                    Else
                                        l_Payee1DataRow("Taxable") = CType(l_Payee1DataRow("Taxable"), Decimal) + (Me.HardshipAmount - l_CounterAmount)
                                        l_CounterAmount += (Me.HardshipAmount - l_CounterAmount)
                                    End If

                                    l_Payee1DataRow("Tax") = CType(l_Payee1DataRow("Tax"), Decimal) + (CType(l_Payee1DataRow("Taxable"), Decimal) * (Me.HardShipTaxRate / 100.0))


                                    l_Payee1DataRow("RefRequestsID") = Me.SessionRefundRequestID
                                    l_Payee1DataRow("AcctType") = l_DataRow("AccountType")
                                    l_Payee1DataRow("Payee") = Me.TextBoxPayee1.Text
                                    l_Payee1DataRow("RequestType") = "HARD"
                                    l_Payee1DataRow("TaxRate") = Me.HardShipTaxRate
                                    l_Payee1DataRow("NonTaxable") = "0.00"

                                    l_Payee1DataTable.Rows.Add(l_Payee1DataRow)

                                Else
                                    If l_FoundRows.Length > 0 Then
                                        l_Payee1DataRow = l_FoundRows(0)

                                        Dim l_row As DataRow

                                        For Each l_row In l_Payee1DataTable.Rows
                                            If l_DataRow("AccountType") = l_row("AcctType") Then
                                                Exit For
                                            End If
                                            l_index += 1
                                        Next
                                    End If
                                    If l_Decimal_PersonalPreTax < (Me.HardshipAmount - l_CounterAmount) Then
                                        l_Payee1DataTable.Rows(l_index).Item("Taxable") = CType(l_Payee1DataTable.Rows(l_index).Item("Taxable"), Decimal) + l_Decimal_PersonalPreTax
                                        l_CounterAmount += l_Decimal_PersonalPreTax
                                    Else
                                        l_Payee1DataTable.Rows(l_index).Item("Taxable") = CType(l_Payee1DataTable.Rows(l_index).Item("Taxable"), Decimal) + (Me.HardshipAmount - l_CounterAmount)
                                        l_CounterAmount += (Me.HardshipAmount - l_CounterAmount)
                                    End If

                                    l_Payee1DataTable.Rows(l_index).Item("Tax") = CType(l_Payee1DataTable.Rows(l_index).Item("Tax"), Decimal) + (CType(l_Payee1DataRow("Taxable"), Decimal) * (Me.HardShipTaxRate / 100.0))


                                    l_Payee1DataTable.Rows(l_index).Item("RefRequestsID") = Me.SessionRefundRequestID
                                    l_Payee1DataTable.Rows(l_index).Item("AcctType") = l_DataRow("AccountType")
                                    l_Payee1DataTable.Rows(l_index).Item("Payee") = Me.TextBoxPayee1.Text
                                    l_Payee1DataTable.Rows(l_index).Item("RequestType") = "HARD"
                                    l_Payee1DataTable.Rows(l_index).Item("TaxRate") = Me.HardShipTaxRate
                                    l_Payee1DataTable.Rows(l_index).Item("NonTaxable") = "0.00"

                                End If

                                l_Payee1DataTable.AcceptChanges()
                                If l_CounterAmount = Me.HardshipAmount Then
                                    Exit For
                                End If

                            End If

                        End If
                    End If
                Next

            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    '***********************************************************************************************
    '* Determine what Currency Code to Use
    '***********************************************************************************************
    Private Function GetCurrencyCode() As String
        Try
            Return Infotech.YmcaBusinessObject.RefundRequest.GetPersonBankingBeforeEffectiveDate(Me.PersonID)
        Catch ex As Exception
            Throw
        End Try
    End Function


    '***********************************************************************************************
    '* Create an Array of the Available Annuity Basis Types
    '***********************************************************************************************
    Private Function CreateAnnuityBasisTypes()

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_AnnuityBasisTypeDataTable As DataTable
        Dim l_DataRow As DataRow
        Dim l_Counter As Integer

        Dim l_AnnuityBasisType() As String

        Try
            l_AnnuityBasisTypeDataTable = Infotech.YmcaBusinessObject.RefundRequestRegularBOClass.LookUpAnnuityBasisTypes()

            If Not l_AnnuityBasisTypeDataTable Is Nothing Then

                l_Counter = 0

                ReDim l_AnnuityBasisType(l_AnnuityBasisTypeDataTable.Rows.Count)

                For Each l_DataRow In l_AnnuityBasisTypeDataTable.Rows
                    If Not (l_DataRow("AnnuityBasisType").GetType.ToString.Trim = "System.DBNull") Then
                        l_AnnuityBasisType(l_Counter) = CType(l_DataRow("AnnuityBasisType"), String)
                    End If

                    l_Counter += 1
                Next
            End If

            'Hafiz 03Feb06 Cache-Session
            'l_CacheManager = CacheFactory.GetCacheManager()
            'l_CacheManager.Add("AnnuityBasisTypeArray", l_AnnuityBasisType)
            Session("AnnuityBasisTypeArray") = l_AnnuityBasisType
            'Hafiz 03Feb06 Cache-Session

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function DoValidation() As String

        Dim l_ErrorMessage As String
        Dim l_decimal_NetFinal As Decimal = 0
        l_decimal_NetFinal = CType(Me.TextboxNetFinal.Text, Decimal)
        If Me.TextboxNetFinal.Text.Trim.Length > 0 Then
            If l_decimal_NetFinal < 0.01 Then Return "The Net Amount of the Refund Can Not be Less Than $ 0.00. Please Adjust Amounts"
        Else
            Return "The Net Amount of the Refund Can Not be Less Than $ 0.00. Please Adjust Amounts"
        End If




        '********************************************************************************************
        '* Check to see if they wanted a Rollover but Didn;t do it
        '********************************************************************************************

        '' Check for RollOver all option
        If Me.RadioButtonRolloverAll.Checked = True Then
            If Me.TextboxPayee2.Text.Trim.Length < 1 Then
                Return "A Rollover was Requested but there is No 2nd Payee Information. Please enter the Payee 2 Information"
            End If
            Dim l_decimal_Net2 As Decimal = 0
            l_decimal_Net2 = CType(Me.TextboxNet2.Text, Decimal)
            If Me.TextboxNet2.Text.Trim.Length > 0 Then
                If l_decimal_Net2 < 0.01 Then
                    Return "The Net Amount of the Refund Can Not be Less Than $ 0.00. Please Adjust Amounts"
                End If
            Else
                Return "The Net Amount of the Refund Can Not be Less Than $ 0.00. Please Adjust Amounts"
            End If

            '' Set the PayeeID & Payee Type..
            l_ErrorMessage = Me.GetPayeeID(Me.TextboxPayee2.Text, 2)

            If Not l_ErrorMessage = String.Empty Then
                Return l_ErrorMessage
            End If

        End If

        '' Check for Taxable only

        If Me.RadioButtonTaxableOnly.Checked = True Then

            If Me.TextboxPayee2.Text.Trim.Length < 1 Then
                Return "A Taxable was Requested but there is No 2nd Payee Information. Please enter the Payee 2 Information"
            End If
            Dim l_decimal_Net2 As Decimal = 0
            l_decimal_Net2 = CType(Me.TextboxNet2.Text, Decimal)
            If Me.TextboxNet2.Text.Trim.Length > 0 Then
                If l_decimal_Net2 < 0.01 Then
                    Return "The Net Amount of the Refund Can Not be Less Than $ 0.00. Please Adjust Amounts"
                End If
            Else
                Return "The Net Amount of the Refund Can Not be Less Than $ 0.00. Please Adjust Amounts"
            End If

            '' Set the PayeeID & Payee Type..
            l_ErrorMessage = Me.GetPayeeID(Me.TextboxPayee2.Text, 2)

            If Not l_ErrorMessage = String.Empty Then
                Return l_ErrorMessage
            End If

        End If



        '' Check for the Third Party 
        If Me.TextboxNet3.Text.Trim.Length > 0 Then
            If (CType(Me.TextboxNet3.Text.Trim, Decimal) > 0.0) And (Me.TextboxPayee3.Text.Trim.Length < 1) Then
                Return "Please enter the Name of the Third Payee."
            End If

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

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        Try
            ' by aparna -YREN-3071 12/02/2007 
            'to add the payee if it does not exist
            Dim l_string_RolloverInstitutionID As String
            'Get the InstitutionID 
            Infotech.YmcaBusinessObject.RefundRequest.Get_RefundRolloverInstitutionID(parameterPayeeName, l_string_RolloverInstitutionID)
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
            'l_DataTable = Infotech.YmcaBusinessObject.RefundRequest.GetPayeeDetails(parameterPayeeName)

            'If Not l_DataTable Is Nothing Then

            '    If l_DataTable.Rows.Count > 0 Then

            '        l_DataRow = l_DataTable.Rows(0)

            '        If Not l_DataRow Is Nothing Then

            '            If parameterIndex = 2 Then
            '                Me.Payee2ID = CType(l_DataRow("UniqueID"), String)
            '            Else
            '                Me.Payee3ID = CType(l_DataRow("UniqueID"), String)
            '            End If

            '            Return String.Empty

            '        Else
            '            If parameterIndex = 2 Then
            '                Return "Details of Payee 2 could not be found. Please select valid Payee 2."
            '            Else
            '                Return "Details of Payee 3 could not be found. Please select valid Payee 3."
            '            End If
            '        End If
            '    Else
            '        If parameterIndex = 2 Then
            '            Return "Details of Payee 2 could not be found. Please select valid Payee 2."
            '        Else
            '            Return "Details of Payee 3 could not be found. Please select valid Payee 3."
            '        End If
            '    End If


            'Else
            '    If parameterIndex = 2 Then
            '        Return "Details of Payee 2 could not be found. Please select valid Payee 2."
            '    Else
            '        Return "Details of Payee 3 could not be found. Please select valid Payee 3."
            '    End If
            'End If



        Catch ex As Exception
            Throw
        End Try

    End Function





    Private Sub CheckForIsPersonalOnly()

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManager As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_RefundRequestTable As DataTable
        Dim l_PersonalOnly As Boolean


        Try

            'l_CacheManager = CacheFactory.GetCacheManager()

            'l_RefundRequestTable = CType(l_CacheManager.GetData("RefundRequestTable"), DataTable)

            'If Me.RefundType = "REG" And IsPersonalOnly() Then

            '    l_PersonalOnly = True

            '    For l_RowCounter = 0 To l_DataTable_RefRequests.Rows.Count - 1
            '        l_DataTable_RefRequests.Rows(l_RowCounter).Item("RefundType") = "PERS"
            '    Next



            'ElseIf SessionIsPersonalOnly() Then
            '    l_PersonalOnly = True
            'Else
            '    l_PersonalOnly = False
            'End If



        Catch ex As Exception

        End Try

    End Sub


    Private Sub ButtonTab1OK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonTab1OK.Click
        Me.SessionIsRefundProcessPopupAllowed = True
        Me.Response.Write("<script language='javascript'> { self.close() }</script>")
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Me.SessionIsRefundProcessPopupAllowed = True
        Me.Response.Write("<script language='javascript'> { self.close() }</script>")
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click

        Dim l_BooleanFlag As Boolean = False
        Dim l_Bool_Report As Boolean = False
        ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL- l_BooleanValidationFlag variable Has been Added
        Dim l_BooleanValidationFlag As Boolean = False
        Try
            l_BooleanValidationFlag = ValidateTotal()
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL- If conditin has been Added Start
            If l_BooleanValidationFlag = True Then
                '' This satement for Refresh the DataGrid in Parent form.
                l_BooleanFlag = Me.SaveRefundProcess()
                'l_BooleanFlag = Me.SaveRefundRequestProcess()
            End If
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL- If conditin has been Added End

            Session("MCARefundLetterHardship_Persid") = Me.PersonID
            '   Session("HardShipReport") = True
            ' Aparna(-YREN - 3027)
            '  If CType(Session("HardShipReport"), Boolean) = True Then
            If l_BooleanFlag Then
                Dim l_string_Message As String
                'Session("CopyHardShipReport") = True
                'To avoid opening of other reports through the RefundRequestForm
                Session("R_ReportToLoad_3") = True
                Session("R_ReportToLoad_1") = Nothing
                Session("R_ReportToLoad_2") = Nothing
                Session("strReportName") = "birefltr"
                'by aparna -
                '    l_string_Message = IDM.HardShipReports("birefltr.rpt", "BIREFLTR")
                'Calling IDMForall for the generation of report 
                SetPropertiesForIDM()
                l_string_Message = IDM.ExportToPDF()
                Session("FTFileList") = IDM.SetdtFileList

                Session("HardshipMessage") = l_string_Message

            End If


            If l_BooleanFlag = True Then
                '' This satement for Refresh the DataGrid in Parent form. 
                Me.SessionIsRefundRequest = True
                Response.Write("<script> window.opener.document.forms(0).submit(); self.close(); </script>")
            End If
            'by Aparna -YREN-3027


            'If Not Session("PersonInformation") Is Nothing Then
            '    Dim l_DataSet_PersonInfo As New DataSet
            '    Dim l_DataTable As DataTable
            '    l_DataSet_PersonInfo = CType(Session("PersonInformation"), DataSet)
            '    If Not l_DataSet_PersonInfo Is Nothing Then
            '        l_DataTable = l_DataSet_PersonInfo.Tables("Member Details")
            '        If l_DataTable.Rows.Count > 0 Then
            '            IDM.ExportToPDF("BIREFLTR", "birefltr", l_DataTable.Rows.Item(0))
            '        End If
            '    End If

            'End If
            'by Aparna -YREN-3027
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" & Server.UrlEncode(ex.Message), False)
        End Try
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
            If Not Session("Member Employment") Is Nothing Then
                l_datatable = CType(Session("Member Employment"), DataTable)
                'StatusType
                l_datarow_CurrentRow = l_datatable.Select("FundEventID = '" & Session("FundID") & " ' and  (TermDate IS NULL OR StatusType =  '" & l_String_StatusType & "')")
                If l_datarow_CurrentRow.Length > 0 Then
                    IDM.YMCAID = l_datarow_CurrentRow(0)("YMCAID")
                    IDM.PersId = l_stringPersonId
                Else
                    '  HardShipReports = "Invalid YmcaId passed for IDX file generation"
                    Exit Sub
                End If

            Else
                '  HardShipReports = "Invalid YmcaId passed for IDX file generation"
                Exit Sub
            End If

            IDM.ReportName = "birefltr.rpt"
            IDM.OutputFileType = "BIREFLTR"
            IDM.DocTypeCode = "BIREFLTR"
            Call AssignParameterstoReport()
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
            l_dataSet = Infotech.YmcaBusinessObject.RefundRequest.YMCARefundLetterHardship(l_string_Persid)

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
            l_String_tmp = CType(l_datarow("City"), String) + ", " + CType(l_datarow("StateType"), String) + "   " + CType(l_datarow("zip"), String)
            ArrListParamValues.Add(CType(l_String_tmp, String).Trim)

            l_String_tmp = CType(l_datarow("FirstName"), String) + " "
            If l_datarow("MiddleName").GetType.ToString() = "System.DBNull" Or CType(l_datarow("MiddleName"), String).Trim = "" Then
                l_String_tmp = l_String_tmp + CType(l_datarow("LastName"), String)
            Else
                l_String_tmp = l_String_tmp + CType(l_datarow("MiddleName"), String).Substring(0, 1) + ". "
                l_String_tmp = l_String_tmp + CType(l_datarow("LastName"), String)

            End If
            ArrListParamValues.Add(CType(l_String_tmp, String).Trim)

            l_String_tmp = CType(l_datarow("SSNo"), String).Substring(0, 3) + "-"
            l_String_tmp += CType(l_datarow("SSNo"), String).Substring(3, 2) + "-"
            l_String_tmp += CType(l_datarow("SSNo"), String).Substring(5, 4)

            ArrListParamValues.Add(CType(l_String_tmp, String).Trim)

            ArrListParamValues.Add(CType(l_datarow("YmcaNo"), String).Trim)
            l_String_tmp = " "

            ArrListParamValues.Add(CType(l_String_tmp, String).Trim)

            'set the Report parameter
            IDM.ReportParameters = ArrListParamValues

        Catch
            Throw
        End Try
    End Sub
    Private Sub RadioButtonAddressUpdatingYes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonAddressUpdatingYes.CheckedChanged

        If Me.RadioButtonAddressUpdatingYes.Checked = True Then

            '' Now disable the Address Button.. 
            'Code Edited by ashutosh 18/04/06
            'Me.ButtonAddress.Visible = True
            Me.ButtonAddress.Enabled = True
            '************************
            Me.TextboxAddress1.ReadOnly = True
            Me.TextboxAddress2.ReadOnly = True
            Me.TextboxAddress3.ReadOnly = True

            Me.TextboxCity1.ReadOnly = True
            Me.TextBoxState.ReadOnly = True
            Me.TextBoxCountry.ReadOnly = True
            Me.TextBoxZip.ReadOnly = True


        Else
            'Code Edited by ashutosh 18/04/06
            'Me.ButtonAddress.Visible = false
            Me.ButtonAddress.Enabled = False
            '***************************

        End If

        Me.EnableDisableSaveButton()
    End Sub

    Private Sub RadioButtonAddressUpdatingNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonAddressUpdatingNo.CheckedChanged
        If Me.RadioButtonAddressUpdatingNo.Checked = True Then

            '' Now disable the Address Button.. 
            'Code Edited by ashutosh 18/04/06***********
            'Me.ButtonAddress.Visible = false
            Me.ButtonAddress.Enabled = False
            '********************************
            Me.TextboxAddress1.ReadOnly = True
            Me.TextboxAddress2.ReadOnly = True
            Me.TextboxAddress3.ReadOnly = True

            Me.TextboxCity1.ReadOnly = True
            Me.TextBoxCountry.ReadOnly = True
            Me.TextBoxState.ReadOnly = True
            Me.TextBoxZip.ReadOnly = True

        End If

        Me.EnableDisableSaveButton()
    End Sub

    Private Sub RadioButtonRolloverYes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonRolloverYes.CheckedChanged

        If Me.RadioButtonRolloverYes.Checked = True Then

            ''Me.DoNone()

            'Me.RadioButtonNone.Checked = True
            'Me.RadioButtonNone_CheckedChanged(sender, New EventArgs)

            If Me.RadioButtonNone.Checked = True Then
                Me.DoNone()
            End If

            If Me.RadioButtonRolloverAll.Checked = True Then
                Me.DoRolloverAll()
            End If


            If Me.RadioButtonTaxableOnly.Checked = True Then
                Me.DoTaxableOnly()
            End If


            Me.RadioButtonNone.Visible = True
            Me.RadioButtonRolloverAll.Visible = True
            Me.RadioButtonTaxableOnly.Visible = True



        End If

        Me.EnableDisableSaveButton()

    End Sub

    Private Sub RadioButtonRolloverNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonRolloverNo.CheckedChanged
        If Me.RadioButtonRolloverNo.Checked = True Then

            Me.RadioButtonNone.Visible = False
            Me.RadioButtonRolloverAll.Visible = False
            Me.RadioButtonTaxableOnly.Visible = False

            Me.TextboxPayee2.Text = String.Empty
            Me.TextboxPayee3.Text = String.Empty

            Me.DoNone()
            Me.RadioButtonNone.Checked = True

        End If
        Me.EnableDisableSaveButton()

    End Sub

    Private Sub EnableDisableSaveButton()

        Dim l_Decimal_Net As Decimal

        Try
            If Me.RadioButtonReleaseSignedYes.Checked And _
                Me.RadioButtonNotarizedYes.Checked And _
                Me.RadioButtonWaiverYes.Checked And _
                Me.RequestedAmount > 0.0 Then


                If Me.RadioButtonRolloverYes.Checked = True Then

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

                '' Check the final Net 

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

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub RadioButtonAddnlWitholdingYes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonAddnlWitholdingYes.CheckedChanged

        If Me.RadioButtonAddnlWitholdingYes.Checked = True Then
            Me.TextboxTaxRate.ReadOnly = False
            Me.TextboxHardShipTaxRate.ReadOnly = False
        Else
            Me.TextboxTaxRate.ReadOnly = True
            Me.TextboxHardShipTaxRate.ReadOnly = True
        End If

        Me.EnableDisableSaveButton()

    End Sub

    Private Sub RadioButtonAddnlWitholdingNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonAddnlWitholdingNo.CheckedChanged
        If Me.RadioButtonAddnlWitholdingNo.Checked = True Then
            Me.TextboxTaxRate.ReadOnly = False
        End If

        Me.EnableDisableSaveButton()
    End Sub

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
        Me.EnableDisableSaveButton()
    End Sub

    Private Sub RadioButtonWaiverYes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonWaiverYes.CheckedChanged
        Me.EnableDisableSaveButton()
    End Sub

    Private Sub TextboxMinDistTaxRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxMinDistTaxRate.TextChanged

        Dim l_TaxRateInteger As Integer

        Dim l_DataTable As DataTable
        Dim l_DataRow As DataRow

        'Hafiz 03Feb06 Cache-Session
        'Dim l_CacheManger As CacheManager
        'Hafiz 03Feb06 Cache-Session
        Dim l_Decimal_Taxable As Decimal
        Dim l_Deimal_Tax As Decimal


        Try

            If Me.TextboxMinDistTaxRate.Text.Trim = String.Empty Then
                Me.TextboxMinDistTaxRate.Text = 0
            End If

            Me.MinDistributionTaxRate = CType(Me.TextboxMinDistTaxRate.Text, Integer)

            '' Replace in the dataTable

            'Hafiz 03Feb06 Cache-Session
            'l_CacheManger = CacheFactory.GetCacheManager
            'l_DataTable = l_CacheManger.GetData("MinimumDistributionTable")
            l_DataTable = Session("MinimumDistributionTable")
            'Hafiz 03Feb06 Cache-Session

            If Not l_DataTable Is Nothing Then

                For Each l_DataRow In l_DataTable.Rows

                    l_Decimal_Taxable = IIf(l_DataRow("Taxable").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("Taxable"), Decimal))

                    l_DataRow("Tax") = l_Decimal_Taxable * (Me.MinDistributionTaxRate / 100.0)
                    l_DataRow("TaxRate") = Me.MinDistributionTaxRate

                Next
            End If

            Me.TextboxMinDistTaxRate.Text = Me.MinDistributionTaxRate

            Me.LoadMinDistributionValuesIntoControls()

        Catch caEx As System.InvalidCastException
            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS", "Invalid Tax Rate entered. Please enter the Valid Tax Rate.", MessageBoxButtons.Stop, True)
            Me.MinDistributionTaxRate = 20
            Me.TextboxMinDistTaxRate.Text = Me.TaxRate
        Catch ex As Exception
            Throw
        End Try

    End Sub


#Region " Hardship Refund "
    'Changed for Addition of new accounts
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


            l_ContributionDataTable = Session("CalculatedDataTableForCurrentAccounts")

            l_ContributionDataTable.RejectChanges()

            If Not l_ContributionDataTable Is Nothing Then

                For Each l_DataRow In l_ContributionDataTable.Rows

                    'Reset the flag
                    l_UserSide = True
                    l_YMCASide = True


                    If Not (l_DataRow("AccountGroup").GetType.ToString = "System.DBNull") Then

                        If Not (CType(l_DataRow("AccountGroup"), String).Trim = "Total") Then


                            l_AccountGroup = CType(l_DataRow("AccountGroup"), String).Trim.ToUpper

                            If Me.IsBasicAccount(l_DataRow) Or l_AccountGroup.Trim = "SR" Then
                                l_UserSide = False
                                l_YMCASide = False
                            End If

                            Select Case (l_AccountGroup.ToUpper.Trim)
                                'Start - Retirement Plan Group

                            Case m_const_RetirementPlan_AM
                                    l_UserSide = True
                                    l_YMCASide = False

                                    If Me.IsTerminated = True Then
                                        l_UserSide = True
                                        l_YMCASide = True
                                    End If

                                Case m_const_RetirementPlan_AP
                                    l_UserSide = True
                                    l_YMCASide = False

                                Case m_const_RetirementPlan_RP
                                    l_UserSide = True
                                    l_YMCASide = False


                                    'End - Retirement Plan Group
                                    'Start - Savings Plan Group 
                                Case m_const_SavingsPlan_TD
                                    l_UserSide = True
                                    l_YMCASide = False

                                    ' m.TotalTotal = m.TotalTotal - m.PersonalInterest
                                    ' m.PersonalTotal = m.PersonalTotal - m.PersonalInterest
                                    ' m.PersonalInterest = 0.0
                                    ' ThisForm.inHardshipAvailable	=	ThisForm.inHardshipAvailable + c_Current.PersonalPreTax()

                                    l_DataRow("Total") = CType(l_DataRow("Total"), Decimal) - CType(l_DataRow("Interest"), Decimal)
                                    l_DataRow("Emp.Total") = CType(l_DataRow("Emp.Total"), Decimal) - CType(l_DataRow("Interest"), Decimal)
                                    l_DataRow("Interest") = "0.00"

                                    Me.HardshipAvailable += CType(l_DataRow("Taxable"), Decimal)


                                Case m_const_SavingsPlan_TM

                                    l_UserSide = True
                                    l_YMCASide = False

                                    ' m.TotalTotal = m.TotalTotal - m.PersonalInterest
                                    ' m.PersonalTotal = m.PersonalTotal - m.PersonalInterest
                                    ' m.PersonalInterest = 0.0
                                    ' ThisForm.inHardshipAvailable	=	ThisForm.inHardshipAvailable + c_Current.PersonalPreTax + c_Current.YmcaPreTax()

                                    l_DataRow("Total") = CType(l_DataRow("Total"), Decimal) - CType(l_DataRow("Interest"), Decimal)
                                    l_DataRow("Emp.Total") = CType(l_DataRow("Emp.Total"), Decimal) - CType(l_DataRow("Interest"), Decimal)
                                    l_DataRow("Interest") = "0.00"

                                    Me.HardshipAvailable += CType(l_DataRow("Taxable"), Decimal) + CType(l_DataRow("YMCATaxable"), Decimal)

                                Case m_const_SavingsPlan_RT
                                    l_UserSide = True
                                    l_YMCASide = False
                                    'End - Savings Plan Group 

                            End Select


                            ''Modify the values

                            If l_UserSide Then

                                If l_YMCASide = False Then

                                    l_DataRow("YMCATaxable") = "0.00"
                                    l_DataRow("YMCAInterest") = "0.00"
                                    l_DataRow("Total") = CType(l_DataRow("Total"), Decimal) - CType(l_DataRow("YMCATotal"), Decimal)
                                    l_DataRow("YMCATotal") = "0.00"

                                End If

                                'l_DataRow("Selected") = "True"

                                Me.YMCAAvailableAmount += CType(l_DataRow("YMCATaxable"), Decimal)

                            Else
                                l_DataRow.Delete()
                            End If

                        End If
                    End If ' Main If...

                Next

                l_ContributionDataTable.AcceptChanges()

                Session("CalculatedDataTable") = l_ContributionDataTable

                Me.CalculateTotal(l_ContributionDataTable)

                Me.LoadCalculatedTableForCurrentAccounts()
                Me.SetSelectedIndex(Me.DatagridCurrentAccts, l_ContributionDataTable)

            End If


        Catch ex As Exception
            Throw
        End Try



    End Function

#End Region

    Private Sub TextboxRequestAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxRequestAmount.TextChanged

        Try
            'Shubhrata May23rd
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
            'changed by ruchi
            Me.EnableDisableSaveButton()
            ' end of change
            Me.ProcessRequiredAmount()

        Catch IcEx As InvalidCastException
            MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS ", "Invalid Amount entered. Please enter valid Amount.", MessageBoxButtons.Stop, True)
            Me.RequestedAmount = 0.0
            Me.TextboxRequestAmount.Text = "0.00"
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub TextboxHardShipTaxRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxHardShipTaxRate.TextChanged

        Try
            'Changes by ruchi on 14 jun 2006
            '''Changes on :28Feb06 By:Preeti Initialized HardShipTaxRate
            '''Me.HardShipTaxRate = 10
            If TextboxHardShipTaxRate.Text.Trim.Length < 1 Then
                Me.TextboxHardShipTaxRate.Text = 10
            End If
            Me.HardShipTaxRate = CType(Me.TextboxHardShipTaxRate.Text, Decimal)
            Me.TDTaxRate = CType(Me.TextboxHardShipTaxRate.Text, Decimal)
            Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate
            'Changed by:Preeti YRST-2254 9May06
            If Me.TDTaxRate < 0 Or Me.TDTaxRate > 100 Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, " YMCA - YRS ", "Tax Rate Should between 0% and 100%.", MessageBoxButtons.Stop, True)
                Me.TDTaxRate = 10
                Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate
            End If

            If Me.TDUsedAmount = 0 And Me.HardshipAvailable > 0.0 And Me.RequestedAmount > Me.HardshipAvailable Then
                Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate

                Me.TextboxHardShipAmount.Text = Math.Round(Me.HardshipAvailable, 2).ToString("0.00")
                Me.TextboxHardShipTax.Text = Math.Round(Me.HardshipAvailable * (Me.TDTaxRate / 100.0), 2).ToString("0.00")
                Me.TextboxHardShipNet.Text = Math.Round(Me.HardshipAvailable - (Me.HardshipAvailable * (Me.TDTaxRate / 100.0)), 2).ToString("0.00")

            Else
                Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate

                Me.TextboxHardShipAmount.Text = Math.Round(Me.TDUsedAmount, 2).ToString("0.00")
                Me.TextboxHardShipTax.Text = Math.Round(Me.TDUsedAmount * (Me.TDTaxRate / 100.0), 2).ToString("0.00")
                'Rahul
                Me.TextboxHardShipNet.Text = Math.Round((Me.TDUsedAmount - (Math.Round(Me.TDUsedAmount * (Me.TDTaxRate / 100.0), 2).ToString("0.00"))), 2)
                'Rahul
            End If
            DoFinalCalculation()

        Catch IcEx As InvalidCastException
            MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", "Invalid Tax Rate entered. Please enter the Valid Tax Rate.", MessageBoxButtons.Stop, True)
            Me.TDTaxRate = 10
            Me.TextboxHardShipTaxRate.Text = Me.TDTaxRate
        Catch ex As Exception
            Throw
        End Try
    End Sub

#Region "Event Added For Missing Functionality IssueId:YRST-2123 "
    ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start - TextboxPayee2_TextChanged Event Has been Added
    Private Sub TextboxPayee2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxPayee2.TextChanged
        Dim index As Integer
        Dim l_TxtTaxable As TextBox
        Dim l_TxtNonTaxable As TextBox
        Try
            If TextboxPayee2.Text.Trim.Length > 0 Then
                For index = 0 To Me.DatagridPayee2.Items.Count - 1
                    l_TxtTaxable = CType(Me.DatagridPayee2.Items(index).Cells(1).FindControl("TextBoxPayee2Taxable"), TextBox)
                    l_TxtNonTaxable = CType(Me.DatagridPayee2.Items(index).Cells(2).FindControl("TextBoxPayee2NonTaxable"), TextBox)
                    l_TxtTaxable.Enabled = True
                    If Me.RadioButtonTaxableOnly.Checked = False Then
                        l_TxtNonTaxable.Enabled = True
                    End If

                Next
                'If Not RadioButtonRolloverAll.Checked = True Then 'Commented by Preeti

                TextboxPayee3.ReadOnly = False

            Else
                For index = 0 To Me.DatagridPayee2.Items.Count - 1
                    l_TxtTaxable = CType(Me.DatagridPayee2.Items(index).Cells(1).FindControl("TextBoxPayee2Taxable"), TextBox)
                    l_TxtNonTaxable = CType(Me.DatagridPayee2.Items(index).Cells(2).FindControl("TextBoxPayee2NonTaxable"), TextBox)
                    l_TxtTaxable.Enabled = True
                    l_TxtTaxable.Text = "0.00"
                    l_TxtNonTaxable.Text = "0.00"
                    DoReverseCalculationForGrids()
                    l_TxtNonTaxable.Enabled = True
                Next

                TextboxPayee3.ReadOnly = True

            End If

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try
    End Sub
    ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start - TextboxPayee2_TextChanged Event Has been Added
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
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try
    End Sub
    ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start - Text_Changed Event Has been Added
    Protected Sub Text_Changed(ByVal sender As Object, ByVal e As EventArgs)

        Dim l_Payee2DataTable As DataTable
        Dim l_Payee1DataTable As DataTable

        Try
            Dim TxtBox As TextBox = CType(sender, TextBox)
            Dim dgItem As DataGridItem = CType(TxtBox.NamingContainer, DataGridItem)
            Dim i As Integer = dgItem.ItemIndex
            If TxtBox.Text.Trim = String.Empty Then
                TxtBox.Text = 0
            Else
                TxtBox.Text = TxtBox.Text.TrimStart("0")

            End If
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

        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try

    End Sub
    ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start - ValidateTotal Method Has been Added
    Public Function ValidateTotal() As Boolean

        Try
            Dim l_Taxable As Decimal
            Dim l_NonTaxable As Decimal
            Dim l_TxtBox As TextBox
            Dim l_Label As Label
            Dim l_index As Integer
            Dim Chk_Use As CheckBox 'Infotech.DataGridCheckBox.CheckBoxColumn

            If Me.DataGridPayee1.Items.Count > 0 Then
                For l_index = 0 To Me.DataGridPayee1.Items.Count - 1
                    Chk_Use = CType(Me.DataGridPayee1.Items(l_index).Cells(3).FindControl("Use"), CheckBox)
                    If Chk_Use.Checked = True Then
                        l_Label = CType(Me.DataGridPayee1.Items(l_index).Cells(1).FindControl("LabelPayee1Taxable"), Label)
                        l_Taxable = Math.Round(Convert.ToDecimal(l_Taxable), 2) + Math.Round(Convert.ToDecimal(l_Label.Text), 2)
                        l_Label = CType(Me.DataGridPayee1.Items(l_index).Cells(2).FindControl("LabelPayee1NonTaxable"), Label)
                        l_NonTaxable = Math.Round(Convert.ToDecimal(l_NonTaxable), 2) + Math.Round(Convert.ToDecimal(l_Label.Text), 2)
                    End If
                Next
            End If


            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start 
            'Commented Following code
            'If Me.DatagridPayee2.Items.Count > 0 Then
            '    For l_index = 0 To Me.DatagridPayee2.Items.Count - 1
            '        l_TxtBox = CType(Me.DatagridPayee2.Items(l_index).Cells(1).FindControl("TextboxPayee2Taxable"), TextBox)
            '        l_Taxable = Math.Round(Convert.ToDecimal(l_Taxable), 2) + Math.Round(Convert.ToDecimal(l_TxtBox.Text), 2)
            '        l_TxtBox = CType(Me.DatagridPayee2.Items(l_index).Cells(2).FindControl("TextboxPayee2NonTaxable"), TextBox)
            '        l_NonTaxable = Math.Round(Convert.ToDecimal(l_NonTaxable), 2) + Math.Round(Convert.ToDecimal(l_TxtBox.Text), 2)
            '    Next
            'End If

            'If Me.DatagridPayee3.Items.Count > 0 Then
            '    For l_index = 0 To Me.DatagridPayee3.Items.Count - 1
            '        l_TxtBox = CType(Me.DatagridPayee3.Items(l_index).Cells(1).FindControl("TextboxPayee3Taxable"), TextBox)
            '        l_Taxable = Math.Round(Convert.ToDecimal(l_Taxable), 2) + Math.Round(Convert.ToDecimal(l_TxtBox.Text), 2)
            '        l_TxtBox = CType(Me.DatagridPayee3.Items(l_index).Cells(2).FindControl("TextboxPayee3NonTaxable"), TextBox)
            '        l_NonTaxable = Math.Round(Convert.ToDecimal(l_NonTaxable), 2) + Math.Round(Convert.ToDecimal(l_TxtBox.Text), 2)
            '    Next
            'End If
            ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL End

            If Math.Round(l_Taxable, 2) > Convert.ToDecimal(TextboxTaxableFinal.Text) Then
                Return False
            Else
                If Math.Round(l_NonTaxable, 2) > Convert.ToDecimal(TextboxNonTaxableFinal.Text) Then
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function
    ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start - DoReverseCalculationForGrids Method Has been Added
    Public Sub DoReverseCalculationForGrids()
        Dim l_double_FinalTaxable As Double
        Dim l_double_FinalNonTaxable As Double
        Dim l_index As Integer
        Dim l_gridindex As Integer
        Dim l_TxtTaxable As TextBox
        Dim l_TxtNonTaxable As TextBox
        Dim l_LblTaxable As Label
        Dim l_LblNonTaxable As Label
        Dim l_Payee1TempDataTable As DataTable
        Dim l_Payee1DataTable As DataTable
        Dim l_Payee2DataTable As DataTable
        Dim l_Payee3DataTable As DataTable
        Dim l_bool_Validate As Boolean
        l_bool_Validate = False
        Try

            l_Payee1TempDataTable = Session("Payee1TempDataTable")
            l_Payee1DataTable = Session("Payee1DataTable")
            l_Payee2DataTable = Session("Payee2DataTable")
            l_Payee3DataTable = Session("Payee3DataTable")

            For l_index = 0 To Me.DataGridPayee1.Items.Count - 1
                l_LblTaxable = Me.DataGridPayee1.Items(l_index).Cells(1).FindControl("LabelPayee1Taxable")
                l_LblNonTaxable = Me.DataGridPayee1.Items(l_index).Cells(1).FindControl("LabelPayee1NonTaxable")

                l_double_FinalTaxable = Convert.ToDouble(l_Payee1TempDataTable.Rows(l_index).Item("Taxable"))
                l_double_FinalNonTaxable = Convert.ToDouble(l_Payee1TempDataTable.Rows(l_index).Item("NonTaxable"))

                If Me.DatagridPayee3.Items.Count > 0 Then
                    l_TxtTaxable = CType(Me.DatagridPayee3.Items(l_index).Cells(1).FindControl("TextBoxPayee3Taxable"), TextBox)
                    l_TxtNonTaxable = CType(Me.DatagridPayee3.Items(l_index).Cells(2).FindControl("TextBoxPayee3NonTaxable"), TextBox)

                    l_double_FinalTaxable = Math.Round(l_double_FinalTaxable, 2) - Math.Round(Convert.ToDouble(l_TxtTaxable.Text), 2)
                    l_double_FinalNonTaxable = Math.Round(l_double_FinalNonTaxable, 2) - Math.Round(Convert.ToDouble(l_TxtNonTaxable.Text), 2)


                End If

                If Me.DatagridPayee2.Items.Count > 0 Then
                    l_TxtTaxable = CType(Me.DatagridPayee2.Items(l_index).Cells(1).FindControl("TextBoxPayee2Taxable"), TextBox)
                    l_TxtNonTaxable = CType(Me.DatagridPayee2.Items(l_index).Cells(2).FindControl("TextBoxPayee2NonTaxable"), TextBox)

                    l_double_FinalTaxable = Math.Round(l_double_FinalTaxable, 2) - Math.Round(Convert.ToDouble(l_TxtTaxable.Text), 2)
                    l_double_FinalNonTaxable = Math.Round(l_double_FinalNonTaxable, 2) - Math.Round(Convert.ToDouble(l_TxtNonTaxable.Text), 2)

                End If
                If l_double_FinalTaxable < 0 Then
                    MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Amounts are Invalid.", MessageBoxButtons.Stop, True)
                    Exit Sub
                Else
                    If l_double_FinalNonTaxable < 0 Then
                        MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Amounts are Invalid.", MessageBoxButtons.Stop, True)
                        Exit Sub
                    End If
                End If
                l_LblTaxable.Text = l_double_FinalTaxable
                l_LblNonTaxable.Text = l_double_FinalNonTaxable
            Next


            l_bool_Validate = ValidateTotal()

            If l_bool_Validate Then
                For l_gridindex = 0 To Me.DataGridPayee1.Items.Count - 1
                    l_LblTaxable = Me.DataGridPayee1.Items(l_gridindex).Cells(1).FindControl("LabelPayee1Taxable")
                    l_LblNonTaxable = Me.DataGridPayee1.Items(l_gridindex).Cells(1).FindControl("LabelPayee1NonTaxable")
                    l_Payee1DataTable.Rows(l_gridindex).Item("Taxable") = Convert.ToDouble(l_LblTaxable.Text)
                    l_Payee1DataTable.Rows(l_gridindex).Item("NonTaxable") = Convert.ToDouble(l_LblNonTaxable.Text)
                Next

                For l_gridindex = 0 To Me.DatagridPayee2.Items.Count - 1
                    l_TxtTaxable = Me.DatagridPayee2.Items(l_gridindex).Cells(1).FindControl("TextboxPayee2Taxable")
                    l_TxtNonTaxable = Me.DatagridPayee2.Items(l_gridindex).Cells(1).FindControl("TextboxPayee2NonTaxable")
                    l_Payee2DataTable.Rows(l_gridindex).Item("Taxable") = Convert.ToDouble(l_TxtTaxable.Text)
                    l_Payee2DataTable.Rows(l_gridindex).Item("NonTaxable") = Convert.ToDouble(l_TxtNonTaxable.Text)
                Next
                For l_gridindex = 0 To Me.DatagridPayee3.Items.Count - 1
                    l_TxtTaxable = Me.DatagridPayee3.Items(l_gridindex).Cells(1).FindControl("TextboxPayee3Taxable")
                    l_TxtNonTaxable = Me.DatagridPayee3.Items(l_gridindex).Cells(1).FindControl("TextboxPayee3NonTaxable")
                    l_Payee3DataTable.Rows(l_gridindex).Item("Taxable") = Convert.ToDouble(l_TxtTaxable.Text)
                    l_Payee3DataTable.Rows(l_gridindex).Item("NonTaxable") = Convert.ToDouble(l_TxtNonTaxable.Text)
                Next

                Session("Payee1DataTable") = l_Payee1DataTable
                Session("Payee2DataTable") = l_Payee2DataTable
                Session("Payee3DataTable") = l_Payee3DataTable
                LoadPayee1ValuesIntoControls()
                LoadPayee2ValuesIntoControls()
                ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start - Added this code Start
                Me.LoadPayee3ValuesIntoControls()
                ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start - Added this code End
                DoFinalCalculation()

            Else
                MessageBox.Show(MessageBoxPlaceHolder, "YMCA_YRS", "Amounts are Invalid.", MessageBoxButtons.Stop, True)
            End If


        Catch ex As Exception

            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message, False)

        End Try
    End Sub
    ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL Start - LoadPayee3ValuesIntoControls Method Has been Added
    Private Function LoadPayee3ValuesIntoControls()
        Dim l_DataTable As DataTable
        Dim l_Decimal_Taxable As Decimal
        Dim l_Decimal_NonTaxable As Decimal
        Dim l_Decimal_Net As Decimal

        Try

            l_DataTable = CType(Session("Payee3DataTable"), DataTable)

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
#End Region
    ' this function will be called each time after Create Payees is called in order to necessary properties for 
    ' the hardship type of refund.
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
    Private Sub ButtonAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddress.Click

        'Made By Ashutosh on 18/04/06****************
        Session("ds_PrimaryAddress") = Nothing
        Dim popupScript As String
        popupScript = "<script language='javascript'>" & _
                                                  "window.open('UpdateAddressDetails.aspx', 'CustomPopUp', " & _
                                                  "'width=700, height=600, menubar=no, Resizable=no,top=50,left=120, scrollbars=yes')" & _
                                                  "</script>"
        Page.RegisterStartupScript("PopupScript1", popupScript)
        '*********************************
    End Sub
#Region "Old Code"
    'Private Function DoHardshipRefundForRefundAccounts()

    '    'Hafiz 03Feb06 Cache-Session
    '    'Dim l_CacheManager As CacheManager
    '    'Hafiz 03Feb06 Cache-Session

    '    Dim l_DataTable As DataTable
    '    Dim l_DataRow As DataRow

    '    Dim l_UserSide As Boolean
    '    Dim l_YMCASide As Boolean
    '    Dim l_Interest As Boolean

    '    Dim l_AccountType As String

    '    Try
    '        'Hafiz 03Feb06 Cache-Session
    '        'l_CacheManager = CacheFactory.GetCacheManager
    '        'l_DataTable = CType(l_CacheManager.GetData("CalculatedDataTableForRefundable"), DataTable)
    '        ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL START
    '        l_DataTable = CType(Session("RefundableDataTable"), DataTable)
    '        'l_DataTable = CType(Session("CalculatedDataTableForRefundable"), DataTable)
    '        ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL END
    '        'Hafiz 03Feb06 Cache-Session

    '        If Not l_DataTable Is Nothing Then


    '            For Each l_DataRow In l_DataTable.Rows

    '                'Reset the flag
    '                l_UserSide = True
    '                l_YMCASide = True
    '                l_Interest = True

    '                If l_DataRow.RowState <> DataRowState.Deleted AndAlso Not (l_DataRow("AccountType").GetType.ToString = "System.DBNull") Then

    '                    If Not (CType(l_DataRow("AccountType"), String).Trim = "Total") Then


    '                        l_AccountType = CType(l_DataRow("AccountType"), String).Trim.ToUpper

    '                        If Me.IsBasicAccount(l_DataRow) Then
    '                            l_UserSide = False
    '                            l_YMCASide = False
    '                            l_Interest = False
    '                        End If

    '                        Select Case l_AccountType

    '                            Case "AP"
    '                                l_UserSide = True
    '                                l_YMCASide = False
    '                                l_Interest = True

    '                            Case "TD"
    '                                l_UserSide = False
    '                                l_YMCASide = False
    '                                l_Interest = False

    '                                If Me.IsTerminated = True And Me.PersonAge >= 59.5 Then
    '                                    l_UserSide = True
    '                                    l_YMCASide = False
    '                                    l_Interest = True
    '                                End If

    '                            Case "TM"
    '                                l_UserSide = False
    '                                l_YMCASide = False
    '                                l_Interest = False

    '                                If Me.IsTerminated = True Then
    '                                    l_UserSide = True
    '                                    l_YMCASide = True
    '                                    l_Interest = True
    '                                ElseIf Me.PersonAge >= 59.5 Then
    '                                    l_UserSide = True
    '                                    l_YMCASide = False
    '                                    l_Interest = True
    '                                End If

    '                            Case "RP"
    '                                l_UserSide = True
    '                                l_YMCASide = False
    '                                l_Interest = True

    '                            Case "RT"
    '                                l_UserSide = True
    '                                l_YMCASide = False
    '                                l_Interest = True


    '                            Case "AM"
    '                                l_UserSide = True
    '                                l_YMCASide = False
    '                                l_Interest = True

    '                                If Me.IsTerminated = True Then
    '                                    l_UserSide = True
    '                                    l_YMCASide = True
    '                                    l_Interest = True
    '                                End If

    '                            Case "SR"
    '                                'Modified by Shubhrata Feb13th 2007,YREN-3039 SR account is to be treated similarly to AM account
    '                                'on the condition that participant is not active
    '                                'l_UserSide = False
    '                                'l_YMCASide = False
    '                                'l_Interest = False
    '                                If Me.SessionStatusType.Trim.ToUpper <> "AE" Then
    '                                    l_UserSide = True
    '                                    l_YMCASide = False
    '                                    l_Interest = True

    '                                    If Me.IsTerminated = True Then
    '                                        l_UserSide = True
    '                                        l_YMCASide = True
    '                                        l_Interest = True
    '                                    End If
    '                                Else
    '                                    l_UserSide = False
    '                                    l_YMCASide = False
    '                                    l_Interest = False
    '                                End If


    '                        End Select


    '                        ''Modify the values

    '                        If l_UserSide Then

    '                            If l_YMCASide = False Then

    '                                l_DataRow("YMCATaxable") = "0.00"
    '                                l_DataRow("YMCAInterest") = "0.00"
    '                                l_DataRow("Total") = CType(l_DataRow("Total"), Decimal) - CType(l_DataRow("YMCATotal"), Decimal)
    '                                l_DataRow("YMCATotal") = "0.00"

    '                            End If

    '                            'Me.YMCAAvailableAmount += CType(l_DataRow("YMCATaxable"), Decimal)

    '                        Else
    '                            l_DataRow.Delete()
    '                        End If

    '                    End If
    '                End If ' Main If...

    '            Next

    '        End If

    '        ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL START
    '        For Each l_DataRow In l_DataTable.Rows
    '            If l_DataRow.RowState <> DataRowState.Deleted Then
    '                l_DataRow("Emp.Total") = Convert.ToDecimal(l_DataRow("Taxable")) + Convert.ToDecimal(l_DataRow("Non-Taxable")) + Convert.ToDecimal(l_DataRow("Interest"))
    '                l_DataRow("YMCATotal") = Convert.ToDecimal(l_DataRow("YMCATaxable")) + Convert.ToDecimal(l_DataRow("YMCAInterest"))
    '                l_DataRow("Total") = Convert.ToDecimal(l_DataRow("Emp.Total")) + Convert.ToDecimal(l_DataRow("YMCATotal"))
    '            End If
    '        Next

    '        l_DataTable.AcceptChanges()
    '        Session("RefundableDataTable") = l_DataTable
    '        ' Change By:Preeti On:23thFeb06 IssueId:YRST-2123 ERROR WHILE PROCESSING A HARDSHIP WITHDRAWAL END
    '        '            Select Case c_Refundable This part seems to be not integrated
    '        'LOCATE

    '        'SCAN
    '        '	REPLACE	PersonalTotal	WITH	PersonalPreTax		+	;	
    '        '											PersonalPostTax	+ 	;
    '        '											PersonalInterest,		;
    '        '				YmcaTotal 		WITH 	YmcaPreTax 			+ 	;
    '        '											YMCAInterest,			;
    '        '				TotalTotal		WITH 	PersonalTotal 		+ 	;
    '        '											YmcaTotal				;
    '        '		IN c_Refundable

    '        'ENDSCAN
    '    Catch ex As Exception
    '        Throw
    '    End Try


    'End Function
    'Private Function CopyAccountContributionTableForCurrentAccounts()

    '    'Hafiz 03Feb06 Cache-Session
    '    'Dim l_CacheManager As CacheManager
    '    'Hafiz 03Feb06 Cache-Session
    '    Dim l_AccountContributionTable As DataTable
    '    Dim l_CalculationDataTable As DataTable
    '    Dim l_DataColumn As DataColumn
    '    Dim l_DataRow As DataRow

    '    Try

    '        '' Get the Account contribution Table from Cache.
    '        'Hafiz 03Feb06 Cache-Session
    '        'l_CacheManager = CacheFactory.GetCacheManager
    '        'Hafiz 03Feb06 Cache-Session

    '        '' Take the Contribution Table, which is available in Cache
    '        'Hafiz 03Feb06 Cache-Session
    '        'l_AccountContributionTable = l_CacheManager.GetData("AccountContribution")
    '        l_AccountContributionTable = Session("AccountContribution")
    '        'Hafiz 03Feb06 Cache-Session

    '        If Not l_AccountContributionTable Is Nothing Then

    '            l_CalculationDataTable = l_AccountContributionTable.Clone

    '            'Copy all Values into Calculation Table 

    '            For Each l_DataRow In l_AccountContributionTable.Rows

    '                If Not (l_DataRow("AccountType").GetType.ToString = "System.DBNull") Then

    '                    If Not (CType(l_DataRow("AccountType"), String) = "Total") Then
    '                        l_CalculationDataTable.ImportRow(l_DataRow)
    '                    End If

    '                    If (CType(l_DataRow("AccountType"), String) = "Total") Then
    '                        Me.TotalRefundAmount = IIf(l_DataRow("Emp.Total").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("Emp.Total"), Decimal)) + IIf(l_DataRow("YMCATotal").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("YMCATotal"), Decimal))
    '                        Me.PersonalAmount = IIf(l_DataRow("Emp.Total").GetType.ToString = "System.DBNull", 0.0, CType(l_DataRow("Emp.Total"), Decimal))
    '                    End If

    '                End If
    '            Next

    '            '' add the copied table to Cache.
    '            'Hafiz 03Feb06 Cache-Session
    '            'l_CacheManager.Add("CalculatedDataTableForCurrentAccounts", l_CalculationDataTable)
    '            Session("CalculatedDataTableForCurrentAccounts") = l_CalculationDataTable
    '            'Hafiz 03Feb06 Cache-Session

    '        End If

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function
#End Region 'Old Code

    Private Sub DatagridNonFundedContributions_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DatagridNonFundedContributions.ItemDataBound
        Try
            e.Item.Cells(9).Visible = True
            e.Item.Cells(10).Visible = False
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?FormType=Popup&Message=" + l_String_Exception_Message)
        End Try
    End Sub
    Private Function SaveRefundProcess() As Boolean

        Dim l_ErrorMessage As String
        Dim l_ProcessMessage As String = String.Empty
        Try

            l_ErrorMessage = Me.DoValidation

            If l_ErrorMessage <> String.Empty Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", l_ErrorMessage, MessageBoxButtons.Stop, True)
                Return False
            End If


            l_ProcessMessage = objRefundProcess.SaveRefundRequestProcess(Me.DatagridDeductions)

            If Left(l_ProcessMessage.Trim(), 5) <> "Error" Then
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", l_ProcessMessage, MessageBoxButtons.Stop, True)
                Return True
            Else
                MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", l_ProcessMessage, MessageBoxButtons.Stop, True)
                Return False
            End If


        Catch ex As Exception
            MessageBox.Show(Me.MessageBoxPlaceHolder, "YMCA - YRS", ex.Message.ToString(), MessageBoxButtons.Stop, True)
        End Try
    End Function

End Class


