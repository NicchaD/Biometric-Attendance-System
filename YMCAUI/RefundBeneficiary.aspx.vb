'********************************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	RefundBEneficiary.aspx.vb
' Author Name		:	Vipul Patel 
' Employee ID		:	32900 
' Email				:	vipul.patel@3i-infotech.com
' Contact No		:	55928738
' Creation Time		:	10/29/2005 
' Program Specification Name :	
' Unit Test Plan Name		 :	
' Description				 :	This form is used for Refunds for Beneficiary Settlement
' Hafiz 03Feb06 Cache-Session
' NP    2007.07.16      Updated code for Plan Split changes
' NP    2007.07.26      Updated code to only access and change Session variables used by the screen. Now the screen will not change the variables for Savings plan if it is only asked to collect information for the Retirement plan
' NP    2007.08.13      Fixing issue reported by Purushottam - Issue 72
' NP    2007.09.05      Fixing issue reported by Purushottam - Issue 118
'                       Fixing issue reported by Purushottam - Issue 133
' NP    2007.12.12      Updated code to convert and round the data to two decimal places on display Issue 315 in Bugtracker
' NP    2009.01.22      YRS 5.0-653 - Removed code that was rounding the tax rate to two decimals
'****************************************************
'Modification History
'****************************************************
'Modified by    Date            Description
'****************************************************
'Neeraj Singh   12/Nov/2009     Added form name for security issue YRS 5.0-940 
'Sanjay Rawat   2010.02.12      To add 6E,7E,6F,7F option For Market Based Withdrawa Refund in GetRefundableAmounts Proc.
'Sanjay R.      2010.06.21      Enhancement changes(CType to DirectCast)
'Sanjay R.      2010.07.12      Code Review changes.(Region,Try..catch block etc.) 
'Sanjay R.      2010.11.02      Change tax withholidng to 20% for Human Beneficiary
'Sanjay R.      2010.11.02      YRS:1064 - Reserves are incorrect in two situations.
'Shashi Shekhar 2010-12-08      For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'Sanjay R       2011.01.10      For YRS 5.0-1233 - Using JQuery
'Priya			11/1/2011		BT-696,YRS 5.0-1233 : Allow input of rollover amount Remove range validator as it will handle by j-query
'Shashi Shekhar 28 Feb 2011     Replacing Header formating with user control (YRS 5.0-450 )
'Bhavna         2011.09.09      BT:739 - put condition for rollover retrie and saving. 
'Sanjay R       2014.05.06      YRS 5.0-2188: RMDs for Beneficieries
'Sanjay R.      2014.07.25      BT 2615/YRS 5.0-2404 :Second Beneficiary get settled automatically after settling first Beneficiary
'Sanjay R.      2014.08.21      BT 2635: Attempted to divide by zero
'Sanjay R       2014.10.07      BT 2672/YRS 5.0-2422:allow rollovoers of pre-tax money only.
'Anudeep A.     2014.12.18      BT 2745 - Revert changes of YRS 5.0-2188 in 14.1.3 
'Manthan Rajguru 2015.09.16     YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Sanjay R       2015.01.11      YRS-AT-2188: RMDs for Beneficieries
'Manthan Rajguru    2016.04.22  YRS-AT-2206 -  Applying Fees and deductions to death payments - Part A.1
'Manthan Rajguru    2016.07.11  YRS-AT-2911 -  YRS enhancement-RMD for benes if nontaxable greater than RMD amount allow rollover taxable only
'Santosh Bura	2016.11.25 		YRS-AT-3022 -  YRS enhancement.--YRS death settlement screen.Track it 26636
'Sanjay GS Rawat 2016.12.07 	YRS-AT-3222 - YRS enh-allow regenerate RMD for deceased participants Phase 2 of 2 (TrackIT 27024) 
'Sanjay GS Rawat 2017.04.17     YRS-AT-3326 - YRS bug-no RMD for Active Death Benefit (TrackIT 28902) 
'Manthan Rajguru 2017.12.12     YRS-AT-3742 -  YRS Bug:(HOT FIX NEEDED) -participants with RMDs more than non-taxable amount are being forced to take excessive amt (TrackIT 31764)
'Sanjay GS Rawat 2017.12.04     YRS-AT-3756 - YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) 
'Santosh Bura   2017.12.18      YRS-AT-3756 - YRS enh: Additional calculation requirements for RMDs for beneficiaries(death on/after required RMD start date) (TrackIT 31836) 
'********************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports Microsoft.Practices.EnterpriseLibrary.Logging


'Hafiz 03Feb06 Cache-Session
'Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Hafiz 03Feb06 Cache-Session
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports YMCAUI.SessionManager
Imports System.Web.Services ' - Manthan | 2016.04.22 | YRS-AT-2206 | Added namespace for Web method

Public Class RefundBeneficiary
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("RefundBeneficiary.aspx")
    'End issue id YRS 5.0-940
    'Start- SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries
    Dim dblTaxableAmtforComputingRMD As Double
    Dim dblNonTaxableAmtforComputingRMD As Double
    'End- SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    End Sub
    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents MenuRetireesInformation As skmMenu.Menu
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents TextboxTotalTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalNonTaxable As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalTax As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextboxTotalNet As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonSave As System.Web.UI.WebControls.Button
    Protected WithEvents TextBoxPayee1Name_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPayee1 As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxNet_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTax_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxNonTaxable_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTaxable_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTaxRate_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxNet_SP As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTax_SP As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxNonTaxable_SP As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTaxable_SP As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxTaxRate_SP As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxPayee1Name_SP As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents RolloverNet_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents RolloverNonTaxable_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents RolloverOptions_RP As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents LabelPayee2 As System.Web.UI.WebControls.Label
    Protected WithEvents RolloverTaxable_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents RolloverNet_SP As System.Web.UI.WebControls.TextBox
    Protected WithEvents RolloverNonTaxable_SP As System.Web.UI.WebControls.TextBox
    Protected WithEvents RolloverTaxable_SP As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents tblRolloverOptions_RP As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblRolloverOptions_SP As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblRP As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSP As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents RolloverOptions_SP As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    Protected WithEvents RolloverPartialAmount_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents RolloverPartialAmount_SP As System.Web.UI.WebControls.TextBox
    'Start- SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries
    Protected WithEvents txtRMDTaxRate_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRMDTaxable_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRMDNonTaxable_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRMDTax_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRMDNet_RP As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblRMD_RP As System.Web.UI.WebControls.Label
    Protected WithEvents lblRMDNotes_RP As System.Web.UI.WebControls.Label
    Protected WithEvents txtRMDTaxRate_SP As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRMDTaxable_SP As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRMDNonTaxable_SP As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRMDTax_SP As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRMDNet_SP As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblRMD_SP As System.Web.UI.WebControls.Label
    Protected WithEvents lblRMDNotes_SP As System.Web.UI.WebControls.Label
    Protected WithEvents lblMaxRollover_RP As System.Web.UI.WebControls.Label
    Protected WithEvents lblMaxRollover_SP As System.Web.UI.WebControls.Label
    Protected WithEvents lnkRMD_RP As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkRMD_SP As System.Web.UI.WebControls.LinkButton
    Protected WithEvents gvBeneficiaryRMDs As System.Web.UI.WebControls.GridView
    'End- SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries
    'Start:SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name 
    Protected WithEvents RolloverPayeeName_RP As RolloverInstitution
    Protected WithEvents RolloverPayeeName_SP As RolloverInstitution
    'End :SR:2014.08.19 :BT 2632/YRS 5.0-2404 - Intelli-sense to select rollover institution name 
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Declaring controls
    Protected WithEvents TextboxDeductions As System.Web.UI.WebControls.TextBox
    Protected WithEvents lnkDeductions As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents lblDedunctionsmsg As System.Web.UI.WebControls.Label
    Protected WithEvents txtFundCostAmt As System.Web.UI.WebControls.TextBox
    Protected WithEvents dgDeductions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents chkBoxDeduction As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblPlantype As System.Web.UI.WebControls.Label
    'End - Manthan | 2016.04.22 | YRS-AT-2206 | Declaring controls
    Private designerPlaceholderDeclaration As System.Object
    'START : SB | 2016.11.25 | YRS-AT-3022 | Declare Checkbox control for Retirement and Savings Plan for Rollover to own IRA option
    Protected WithEvents chkRolloverToOwnIRA_RP As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkRolloverToOwnIRA_SP As System.Web.UI.WebControls.CheckBox
    Protected WithEvents hdnRelationshipWithParticipant As System.Web.UI.HtmlControls.HtmlInputHidden
    'END : SB | 2016.11.25 | YRS-AT-3022 | Declare Checkbox control for Retirement and Savings Plan for Rollover to own IRA option
    'START: MMR | 2017.12.11 | YRS-AT-3742 | Declared hidden field control to hold plan for RMD greater than non-taxable in case of rollover option selected
    Protected WithEvents hdnIsNonTaxableGreaterThanRMDRET As System.Web.UI.WebControls.HiddenField
    Protected WithEvents hdnIsNonTaxableGreaterThanRMDSAV As System.Web.UI.WebControls.HiddenField
    'END: MMR | 2017.12.11 | YRS-AT-3742 | Declared hidden field control to hold plan for RMD greater than non-taxable in case of rollover option selected

    'START : SB | 2017.12.18 | YRS-AT-3756 | Declare Varaible to store minimum RMD tax rate of participant along with error message that will be displayed if any invalid tax rate is entered
    Protected WithEvents hdnMinRMDTaxRate As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hdnRMDTaxRateErrorMessage As System.Web.UI.HtmlControls.HtmlInputHidden
    'END : SB | 2017.12.18 | YRS-AT-3756 | Declare Varaible to store minimum RMD tax rate of participant along with error message that will be displayed if any invalid tax rate is entered

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Local Variables"
    Dim RetirementPlan_OptionId As String
    Dim SavingsPlan_OptionId As String
    Dim RetirementPlan_RefundDetails As DataSet
    Dim SavingsPlan_RefundDetails As DataSet
    'Dim IsSpouse As Boolean
    Dim IsHumanBeneficiary As Boolean  '' New variable defined to replace IsSpouse variable
    Dim ErrorOnForm As Boolean
    Dim MinimumTaxRate As Double
    Dim IsRollover As Boolean
    Dim RetirementPlan_RefundCalculations As DataTable
    Dim SavingsPlan_RefundCalculations As DataTable
    Dim RolloverOption As String
    ''SR:2011.01.10 - New variable defined to pass parametr in JQuery function 'InitializeScript'
    Dim dblTaxable_RP As Double
    Dim dblNonTaxable_RP As Double
    Dim dblTaxable_SP As Double
    Dim dblNonTaxable_SP As Double
    'Start - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 | Declared variables for RMD
    Dim dblRMDTaxable_RP As Double
    Dim dblRMDNonTaxable_RP As Double
    Dim dblRMDTaxable_SP As Double
    Dim dblRMDNonTaxable_SP As Double
    'End - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 | Declared variables for RMD

    Dim dsBeneficiaryRMDS As DataSet 'SR:2014.05.29 -YRS 5.0-2188: RMDs for Beneficieries
    'START: MMR | 2017.12.11 | YRS-AT-3742 | Declared variables to indicate which plan option selected for RMD Beneficiaries
    Dim IsRetirementPlanOptionAvailable As Boolean
    Dim IsSavingsPlanOptionAvailable As Boolean
    'END: MMR | 2017.12.11 | YRS-AT-3742 | Declared variables to indicate which plan option selected for RMD Beneficiaries
#End Region

#Region "Local Variable Persistence mechanism"
    Protected Overrides Function SaveViewState() As Object
        ViewState("Temp") = String.Empty  'Added by SR:2010.06.21 for migration
        StoreLocalVariablesToCache()
        Return MyBase.SaveViewState()
    End Function
    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        ViewState("Temp") = String.Empty  'Added by SR:2010.06.21 for migration
        InitializeLocalVariablesFromCache()
        MyBase.LoadViewState(savedState)
    End Sub

    Private Sub InitializeLocalVariablesFromCache()
        Try
            'Session: Load data from Session so that it is accessible on Postback - This can be replaced by a load from viewstate or database
            RetirementPlan_OptionId = DirectCast(Session("RetirementPlan_OptionId"), String)  'Changed from CType to Directcast by SR:2010.06.21 for migration
            SavingsPlan_OptionId = DirectCast(Session("SavingsPlan_OptionId"), String) 'Changed from CType to Directcast by SR:2010.06.21 for migration
            RetirementPlan_RefundDetails = DirectCast(Session("RetirementPlan_RefundDetails"), DataSet) 'Changed from CType to Directcast by SR:2010.06.21 for migration
            SavingsPlan_RefundDetails = DirectCast(Session("SavingsPlan_RefundDetails"), DataSet) 'Changed from CType to Directcast by SR:2010.06.21 for migration
            IsHumanBeneficiary = DirectCast(Session("IsSpouse"), Boolean) 'Changed from CType to Directcast by SR:2010.06.21 for migration
            ErrorOnForm = DirectCast(Session("ErrorOnForm"), Boolean) 'Changed from CType to Directcast by SR:2010.06.21 for migration
            MinimumTaxRate = DirectCast(Session("MinimumTaxRate"), Double) 'Changed from CType to Directcast by SR:2010.06.21 for migration
            RetirementPlan_RefundCalculations = DirectCast(Session("RetirementPlan_RefundCalculations"), DataTable) 'Changed from CType to Directcast by SR:2010.06.21 for migration
            SavingsPlan_RefundCalculations = DirectCast(Session("SavingsPlan_RefundCalculations"), DataTable) 'Changed from CType to Directcast by SR:2010.06.21 for migration
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    Private Sub StoreLocalVariablesToCache()
        Try
            'Session: Pass data into Session so that it is accessible on Postback - This can be replaced by a save to viewstate
            Session("RetirementPlan_OptionId") = RetirementPlan_OptionId
            Session("SavingsPlan_OptionId") = SavingsPlan_OptionId
            Session("RetirementPlan_RefundDetails") = RetirementPlan_RefundDetails
            Session("SavingsPlan_RefundDetails") = SavingsPlan_RefundDetails
            Session("IsSpouse") = IsHumanBeneficiary
            Session("ErrorOnForm") = ErrorOnForm
            Session("MinimumTaxRate") = MinimumTaxRate
            Session("RetirementPlan_RefundCalculations") = RetirementPlan_RefundCalculations
            Session("SavingsPlan_RefundCalculations") = SavingsPlan_RefundCalculations
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "General Utility Functions"
    Private Function isNonEmpty(ByRef ds As DataSet) As Boolean
        If ds Is Nothing Then Return False
        If ds.Tables.Count = 0 Then Return False
        If ds.Tables(0).Rows.Count = 0 Then Return False
        Return True
    End Function
    Private Function isNonEmpty(ByRef dt As DataTable) As Boolean
        If dt Is Nothing Then Return False
        If dt.Rows.Count = 0 Then Return False
        Return True
    End Function
    Private Function isNonEmpty(ByRef dv As DataView) As Boolean
        If dv Is Nothing Then Return False
        If dv.Count = 0 Then Return False
        Return True
    End Function
    Private Function isEmpty(ByRef ds As DataSet) As Boolean
        Return Not isNonEmpty(ds)
    End Function
    Private Function isEmpty(ByRef dt As DataTable) As Boolean
        Return Not isNonEmpty(dt)
    End Function
    Private Function isEmpty(ByRef dv As DataView) As Boolean
        Return Not isNonEmpty(dv)
    End Function
    Dim l_show_MessageBox As Boolean = False
    Private Function showMessage(ByRef paramPlaceHolder As PlaceHolder, ByRef paramTitle As String, _
     ByRef paramMessage As String, ByRef paramMessageButton As MessageBoxButtons, Optional ByVal paramIsFromPopup As Boolean = False) As Boolean
        If l_show_MessageBox = False Then
            'show the message box 
            MessageBox.Show(paramPlaceHolder, paramTitle, paramMessage, paramMessageButton, paramIsFromPopup)
            l_show_MessageBox = True
            Return True
        Else
            Return False
        End If

    End Function

    'NP:PS:2007.07.18 - Changing code to handle Show/Hide for the newly added controls
    Private Sub ShowHideControls()
        'Show or hide the Savings and Retirement Rollover options based on whether the checkbox is selected
        tblRolloverOptions_RP.Visible = True
        tblRolloverOptions_SP.Visible = True
        'Show or hide the Savings and Retirement sections of the screen based on what parameters have been passed
        tblRP.Visible = IIf(RetirementPlan_OptionId = "", False, True)
        tblSP.Visible = IIf(SavingsPlan_OptionId = "", False, True)

    End Sub

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Session("LoggedUserKey") Is Nothing Then
            'Response.Redirect("Login.aspx", True) : Exit Sub
        End If

        'RolloverTaxable_RP.Attributes.Add("Onblur", "javascript:Calculate_Total();")
        'onblur="Calculate_Taxable_RP();Calculate_Total()"
        'Put user code to initialize the page here
        ' ************************** Important ********************************
        ' Call this Screen only when POption = "B" or "C" or "D"

        Try
            If Request.Form("OK") = "OK" And ErrorOnForm = True Then
                Me.CloseThisForm()
                Exit Sub
            End If

            'Shubhrata YRST 2474
            If Request.Form("CheckBoxYes") <> Nothing Then
                If Request.Form("CheckBoxYes") = "on" Then
                    Me.RolloverNonTaxable_RP.Text = Me.TextboxTotalNonTaxable.Text
                    Me.RolloverTaxable_RP.Text = Me.TextboxTotalTaxable.Text
                    Me.HandleCalculations()
                End If
            End If
            'Shubhrata YRST 2474
            If Not Me.IsPostBack Then
                'Start-SR:2014.05.23- Beneficiary RMDs
                SessionBeneficiaryRMDs.BeneficiaryRMDs_RP = Nothing
                SessionBeneficiaryRMDs.BeneficiaryRMDs_SP = Nothing
                'End-SR:2014.05.23- Beneficiary RMDs
                'Removing use of Query string for security reasons
                'If Not Request.QueryString("BenefitOptionID") Is Nothing OrElse Not Request.QueryString("BenefitOptionID") = "" Then
                If Not Session("BS_SelectedOption_RP") Is Nothing OrElse Not Session("BS_SelectedOption_RP") = "" Then
                    RetirementPlan_OptionId = Session("BS_SelectedOption_RP").ToString()
                End If
                If Not Session("BS_SelectedOption_SP") Is Nothing OrElse Not Session("BS_SelectedOption_SP") = "" Then
                    SavingsPlan_OptionId = Session("BS_SelectedOption_SP").ToString()
                End If

                'Code added for debugging only. Remove in production
                If RetirementPlan_OptionId = "" Then RetirementPlan_OptionId = Request.QueryString.Item("Option_RP")
                If SavingsPlan_OptionId = "" Then SavingsPlan_OptionId = Request.QueryString.Item("Option_SP")
                'End Code added for debugging

                If RetirementPlan_OptionId <> "" OrElse SavingsPlan_OptionId <> "" Then
                    Session("CurrentForm") = "BR"
                    ErrorOnForm = False
                    'CALL the Proc yrs_usp_BS_Get_BeneficiaryBenefitOption into a dataset 
                    Dim ds As DataSet
                    If RetirementPlan_OptionId <> "" AndAlso Get_BeneficiaryBenefitDetails4Refund(RetirementPlan_OptionId, ds) Then
                        RetirementPlan_RefundDetails = ds
                    End If
                    ds = Nothing
                    If SavingsPlan_OptionId <> "" AndAlso Get_BeneficiaryBenefitDetails4Refund(SavingsPlan_OptionId, ds) Then
                        SavingsPlan_RefundDetails = ds
                    End If
                    BindDataToControls()
                    ShowHideControls()
                    'Fire the Calculations
                    HandleCalculations()
                    'ClientScript.RegisterClientScriptBlock(Me.GetType(), "InitializeScript", "<script language='javascript'>$(document).ready(function () { initializeControls(" & Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nTaxable")) & ", " & Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nNonTaxable")) & ", " & Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nTaxable")) & ", " & Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nNonTaxable")) & "); });</script>")
                    'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Calling method to load deductions in grid and clearing session value
                    LoadDeductions(dgDeductions)
                    Session("FinalDeductionsLumpsum") = Nothing
                    'End - Manthan | 2016.04.22 | YRS-AT-2206 | Calling method to load deductions in grid and clearing session value
                Else
                    showMessage(PlaceHolder1, "Error", "Beneficiary Settlement Details couldn't be found due to Error", MessageBoxButtons.Stop)
                    ErrorOnForm = True
                    Exit Sub
                End If
            Else  ''SR:2011.01.10 - To register JQuery function 'InitializeScript'
                If isNonEmpty(RetirementPlan_RefundCalculations) Then
                    dblTaxable_RP = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nTaxable"))
                    dblNonTaxable_RP = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nNonTaxable"))
                    'Start - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |Assigning RMD taxable and non-taxable values to variable for retirement plan
                    dblRMDTaxable_RP = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nRMDTaxable"))
                    dblRMDNonTaxable_RP = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nRMDNonTaxable"))
                    'End - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |Assigning RMD taxable and non-taxable values to variable for retirement plan
                End If
                If isNonEmpty(SavingsPlan_RefundCalculations) Then
                    dblTaxable_SP = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nTaxable"))
                    dblNonTaxable_SP = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nNonTaxable"))
                    'Start - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |Assigning RMD taxable and non-taxable values to variable for savings plan
                    dblRMDTaxable_SP = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nRMDTaxable"))
                    dblRMDNonTaxable_SP = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nRMDNonTaxable"))
                    'End - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |Assigning RMD taxable and non-taxable values to variable for savings plan
                End If
                'Start - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |Commented existing code and passing RMD taxable and Non-taxable parameters to initializeControls method
                'ClientScript.RegisterClientScriptBlock(Me.GetType(), "InitializeScript", "<script language='javascript'>$(document).ready(function () { initializeControls(" & dblTaxable_RP & ", " & dblNonTaxable_RP & ", " & dblTaxable_SP & ", " & dblNonTaxable_SP & "); });</script>")
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "InitializeScript", "<script language='javascript'>$(document).ready(function () { initializeControls(" & dblTaxable_RP & ", " & dblNonTaxable_RP & ", " & dblTaxable_SP & ", " & dblNonTaxable_SP & ", " & dblRMDTaxable_RP & ", " & dblRMDNonTaxable_RP & ", " & dblRMDTaxable_SP & ", " & dblRMDNonTaxable_SP & "); });</script>")
                'End - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |Commented existing code and passing RMD taxable and Non-taxable parameters to initializeControls method
            End If
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try

    End Sub

#Region "Event Handling code"
    'NP:PS:2007.07.19 - Updated code 
    'Private Sub RolloverOptions_RP_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) '' Handles RolloverOptions_RP.SelectedIndexChanged
    '    Try
    '        RolloverOption = "RP"
    '        HandleCalculations()
    '    Catch ex As SqlException
    '        Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    End Try
    'End Sub
    ''SR:2011.01.10 - commented to handlede by JQuery'
    ''NP:PS:2007.07.19 - New code
    'Private Sub TextBoxTaxRate_RP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) ''Handles TextBoxTaxRate_RP.TextChanged
    '    Try
    '        If TextBoxTaxRate_RP.Text = "" Then TextBoxTaxRate_RP.Text = 0.0
    '        If isNonEmpty(RetirementPlan_RefundCalculations) Then
    '            RetirementPlan_RefundCalculations.Rows(0).Item("nTaxRate") = Convert.ToDecimal(TextBoxTaxRate_RP.Text)
    '        End If
    '        HandleCalculations()
    '    Catch ex As SqlException
    '        Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    End Try
    'End Sub
    ''NP:PS:2007.07.20 - New code
    'Private Sub TextBoxTax_RP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles TextBoxTax_RP.TextChanged
    '    Try
    '        If TextBoxTax_RP.Text = "" Then TextBoxTax_RP.Text = 0.0
    '        If isNonEmpty(RetirementPlan_RefundCalculations) Then
    '            Dim taxRate, taxable, taxAmount As Decimal
    '            taxAmount = Convert.ToDecimal(TextBoxTax_RP.Text)
    '            taxable = RetirementPlan_RefundCalculations.Rows(0).Item("nTaxable")
    '            taxRate = (taxAmount / taxable) * 100
    '            RetirementPlan_RefundCalculations.Rows(0).Item("nTaxRate") = taxRate
    '        End If
    '        HandleCalculations()
    '    Catch ex As SqlException
    '        Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    End Try
    'End Sub
    'NP:PS:2007.07.19 - Updated code 
    'Private Sub RolloverOptions_SP_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RolloverOptions_SP.SelectedIndexChanged
    '    Try
    '        RolloverOption = "SP"
    '        HandleCalculations()
    '    Catch ex As SqlException
    '        Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    End Try
    'End Sub
    'NP:PS:2007.07.19 - New code
    ''SR:2011.01.10 - handles removed to handlede by JQuery'
    Private Sub TextBoxTaxRate_SP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles TextBoxTaxRate_SP.TextChanged
        Try
            If TextBoxTaxRate_SP.Text = "" Then TextBoxTaxRate_SP.Text = 0.0
            If isNonEmpty(SavingsPlan_RefundCalculations) Then
                SavingsPlan_RefundCalculations.Rows(0).Item("nTaxRate") = Convert.ToDecimal(TextBoxTaxRate_SP.Text)
            End If
            HandleCalculations()
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    'NP:PS:2007.07.20 - New code
    ''SR:2011.01.10 - handles removed to handlede by JQuery'
    Private Sub TextBoxTax_SP_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles TextBoxTax_SP.TextChanged
        Try
            If isNonEmpty(SavingsPlan_RefundCalculations) Then
                Dim taxRate, taxable, taxAmount As Decimal
                taxAmount = Convert.ToDecimal(TextBoxTax_SP.Text)
                taxable = SavingsPlan_RefundCalculations.Rows(0).Item("nTaxable")
                taxRate = (taxAmount / taxable) * 100
                SavingsPlan_RefundCalculations.Rows(0).Item("nTaxRate") = taxRate
            End If
            HandleCalculations()
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Try
            'If Page.IsValid() = False Then Exit Sub ''SR:2011.01.10 - Handled by JQuery.Hence this validation is no more required

            Session("SP_Parameters_RolloverInstitutionID_RP") = Nothing
            If RolloverOptions_RP.SelectedValue = "taxable" OrElse RolloverOptions_RP.SelectedValue = "all" OrElse RolloverOptions_RP.SelectedValue = "Partial" Then
                If Me.RolloverPayeeName_RP.Text.Trim() = "" Then
                    showMessage(PlaceHolder1, "Refund Beneficiary - Rollover Name is Blank", "Please Enter the Name of the Rollover Institution", MessageBoxButtons.Stop)
                    RolloverPayeeName_RP.Enabled = True
                    Exit Sub
                Else
                    Dim l_string_RolloverInstitutionID As String
                    'Get the InstitutionID 
                    YMCARET.YmcaBusinessObject.RefundBeneficiaryBOClass.Get_RefundRolloverInstitutionID(Me.RolloverPayeeName_RP.Text.Trim(), l_string_RolloverInstitutionID)
                    If l_string_RolloverInstitutionID.Trim() = "" Then
                        showMessage(PlaceHolder1, "Refund Beneficiary - Rollover ", "Unable to retrive Rollover Institution Information Data", MessageBoxButtons.Stop)
                        Session("Success_BRScreen") = False
                        Exit Sub
                    Else
                        Session("SP_Parameters_RolloverInstitutionID_RP") = l_string_RolloverInstitutionID
                    End If
                End If
            End If
            Session("SP_Parameters_RolloverInstitutionID_SP") = Nothing
            If RolloverOptions_SP.SelectedValue = "taxable" OrElse RolloverOptions_SP.SelectedValue = "all" OrElse RolloverOptions_SP.SelectedValue = "Partial" Then
                If Me.RolloverPayeeName_SP.Text.Trim() = "" Then    'NP:PS:2007.08.13 - Checking the wrong textbox - was checking RP instead of SP.
                    showMessage(PlaceHolder1, "Refund Beneficiary - Rollover Name is Blank", "Please Enter the Name of the Rollover Institution", MessageBoxButtons.Stop)
                    RolloverPayeeName_SP.Enabled = True
                    Exit Sub
                Else
                    Dim l_string_RolloverInstitutionID As String
                    'Get the InstitutionID 
                    YMCARET.YmcaBusinessObject.RefundBeneficiaryBOClass.Get_RefundRolloverInstitutionID(Me.RolloverPayeeName_SP.Text.Trim(), l_string_RolloverInstitutionID)
                    If l_string_RolloverInstitutionID.Trim() = "" Then
                        showMessage(PlaceHolder1, "Refund Beneficiary - Rollover ", "Unable to retrive Rollover Institution Information Data", MessageBoxButtons.Stop)
                        Session("Success_BRScreen") = False
                        Exit Sub
                    Else
                        Session("SP_Parameters_RolloverInstitutionID_SP") = l_string_RolloverInstitutionID
                    End If
                End If
            End If

            If SaveValidationsPassed() Then  ''SR:2011.01.10 - Validating JQuery functions'
                If Me.tblRP.Visible Then
                    Session("SP_Parameters_RolloverTaxable_RP") = Convert.ToDouble(Me.RolloverTaxable_RP.Text)
                    Session("SP_Parameters_RolloverNonTaxable_RP") = Convert.ToDouble(Me.RolloverNonTaxable_RP.Text)
                    'Start- SR:2014.05.06 - YRS 5.0-2188: Calculate prorated taxrate 
                    'Start-SR:2014.08.21-BT 2635-Attempted to divide by zero
                    If String.IsNullOrEmpty(TextBoxTaxable_RP.Text) Then
                        TextBoxTaxable_RP.Text = 0.0
                    End If
                    If String.IsNullOrEmpty(txtRMDTaxable_RP.Text) Then
                        txtRMDTaxable_RP.Text = 0.0
                    End If
                    If String.IsNullOrEmpty(TextBoxTax_RP.Text) Then
                        TextBoxTax_RP.Text = 0.0
                    End If
                    If String.IsNullOrEmpty(txtRMDTax_RP.Text) Then
                        txtRMDTax_RP.Text = 0.0
                    End If

                    If (Convert.ToDecimal(TextBoxTaxable_RP.Text) > 0 Or Convert.ToDecimal(txtRMDTaxable_RP.Text) > 0) Then
                        Session("SP_Parameters_WithholdingPct_RP") = ((Convert.ToDecimal(TextBoxTax_RP.Text) + Convert.ToDecimal(txtRMDTax_RP.Text)) / (Convert.ToDecimal(TextBoxTaxable_RP.Text) + Convert.ToDecimal(txtRMDTaxable_RP.Text))) * 100
                    Else
                        Session("SP_Parameters_WithholdingPct_RP") = 0
                    End If
                    'End-SR:2014.08.21-BT 2635-Attempted to divide by zero
                    'End- SR:2014.05.06 - YRS 5.0-2188: Calculate prorated taxrate 
                End If
                If Me.tblSP.Visible = True Then
                    Session("SP_Parameters_RolloverTaxable_SP") = Convert.ToDouble(Me.RolloverTaxable_SP.Text)
                    Session("SP_Parameters_RolloverNonTaxable_SP") = Convert.ToDouble(Me.RolloverNonTaxable_SP.Text)
                    'Start- SR:2014.05.06 - YRS 5.0-2188: Calculate prorated taxrate 
                    'Start-SR:2014.08.21-BT 2635-Attempted to divide by zero
                    If String.IsNullOrEmpty(TextBoxTaxable_SP.Text) Then
                        TextBoxTaxable_SP.Text = 0.0
                    End If
                    If String.IsNullOrEmpty(txtRMDTaxable_SP.Text) Then
                        txtRMDTaxable_SP.Text = 0.0
                    End If
                    If String.IsNullOrEmpty(TextBoxTax_SP.Text) Then
                        TextBoxTax_SP.Text = 0.0
                    End If
                    If String.IsNullOrEmpty(txtRMDTax_SP.Text) Then
                        txtRMDTax_SP.Text = 0.0
                    End If
                    If (Convert.ToDecimal(TextBoxTaxable_SP.Text) > 0 Or Convert.ToDecimal(txtRMDTaxable_SP.Text) > 0) Then
                        Session("SP_Parameters_WithholdingPct_SP") = ((Convert.ToDecimal(TextBoxTax_SP.Text) + Convert.ToDecimal(txtRMDTax_SP.Text)) / (Convert.ToDecimal(TextBoxTaxable_SP.Text) + Convert.ToDecimal(txtRMDTaxable_SP.Text))) * 100
                    Else
                        Session("SP_Parameters_WithholdingPct_SP") = 0
                    End If
                    'End-SR:2014.08.21-BT 2635-Attempted to divide by zero
                    'End- SR:2014.05.06 - YRS 5.0-2188: Calculate prorated taxrate 
                End If

                Session("Success_BRScreen") = True
                Me.CloseThisForm()
            End If

            'START : SB | 2016.11.25 | YRS-AT-3022 | Assign IsRolloverToOwnIRA session variables 
            SP_Parameters_IsRolloverToOwnIRA_RP = False
            SP_Parameters_IsRolloverToOwnIRA_SP = False
            If chkRolloverToOwnIRA_SP.Checked Then
                SP_Parameters_IsRolloverToOwnIRA_SP = True
            End If
            If chkRolloverToOwnIRA_RP.checked Then
                SP_Parameters_IsRolloverToOwnIRA_RP = True
            End If
            'END : SB | 2016.11.25 | YRS-AT-3022 | Assign IsRolloverToOwnIRA session variables 

        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    Private Sub ButtonOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        'NP:PS:2007.09.05 - No need to perform calculations simply close the form and cancel the process.
        'HandleCalculations()
        'Exit Sub
        'Start - SR:2014.07.25 :BT 2615 : Second Beneficiary get settled automatically after settling first Beneficiary
        'Session("Success_RefundScreen") = False 'BT 2615  : Since this session has been not used anywhere else, removing this session.
        Session("Success_BRScreen") = False
        'End - SR:2014.07.25 :BT 2615 : Second Beneficiary get settled automatically after settling first Beneficiary
        'Me.CloseThisForm()
        'Shubhrata Modified due to YRST 2474
        'Start-SR:2014.05.23- Beneficiary RMDs
        SessionBeneficiaryRMDs.BeneficiaryRMDs_RP = Nothing
        SessionBeneficiaryRMDs.BeneficiaryRMDs_SP = Nothing
        'End-SR:2014.05.23- Beneficiary RMDs
        'Start -- Manthan | 2016.04.22 | YRS-AT-2206 | Clearing session value  
        Session("FinalDeductionsLumpsum") = Nothing
        Session("TotDeductionsLumpsumAmt") = Nothing
        'End -- Manthan | 2016.04.22 | YRS-AT-2206 | Clearing session value  
        Dim msg As String
        msg = msg + "<Script Language='JavaScript'> self.close(); </Script>"
        Response.Write(msg)
        'Shubhrata Modified due to YRST 2474
    End Sub

#End Region

#Region "Data Manipulation Functions"
    'NP:PS:2007.07.18 - Modified Sub to get Benefit Details for both plans based on parameter
    Private Function Get_BeneficiaryBenefitDetails4Refund(ByVal paramBeneficiaryBenefitOptionID As String, ByRef ds As DataSet) As Boolean
        Dim l_String_ErrorMessage As String
        l_String_ErrorMessage = ""
        Dim l_dataset_RefundDetails As DataSet
        Try
            l_dataset_RefundDetails = YMCARET.YmcaBusinessObject.RefundBeneficiaryBOClass.Get_BeneficiaryBenefitDetails4Refund(paramBeneficiaryBenefitOptionID)
            'This dataset will contain 3 tables 
            '0 = DeathBenefitOption
            '1 = BeneficiaryNameSSNDetails
            '2 = BeneficiaryRelationshipCode	
            ''Lets do some basic Validations
            ds = l_dataset_RefundDetails

            If l_dataset_RefundDetails.Tables.Count <> 3 Then
                ' Validation for proper Fetching of 3 tables
                l_String_ErrorMessage = "Data Not Fetch Properly from Database  " + vbCrLf + "Please contact Administrator"
            ElseIf l_dataset_RefundDetails.Tables("DeathBenefitOption").Rows.Count <> 1 Then
                ' Not the validations for the 1st table "BeneficiaryBenefitOption" data 
                If l_dataset_RefundDetails.Tables("DeathBenefitOption").Rows.Count < 1 Then
                    l_String_ErrorMessage = "Unable to locate a row in table 'AtsDeathBenefitOptions' for the passed value"
                Else
                    l_String_ErrorMessage = "More than one row was located in table 'AtsDeathBenefitOptions' for the passed value"
                End If
            ElseIf l_dataset_RefundDetails.Tables("DeathBenefitOption").Rows(0)("POption").ToString().Trim() = "A" Then
                l_String_ErrorMessage = "The Selected Option 'A' does NOT get Refund Monies"
            ElseIf l_dataset_RefundDetails.Tables("BeneficiaryNameSSNDetails").Rows.Count < 1 Then
                ' Not the validations for the 2nd table "BeneficiaryBenefitOption" data 
                l_String_ErrorMessage = "Unable to Locate this Beneficiary in the Beneficiaries Table"
            ElseIf l_dataset_RefundDetails.Tables("BeneficiaryRelationshipCode").Rows.Count < 1 Then
                '3rd Validation for Relationship Type
                l_String_ErrorMessage = "Unable to Determine the Beneficiaries Relationship Type"
            End If

            If l_String_ErrorMessage.Trim.Length <> 0 Then
                showMessage(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                ' Return to Main Form 
                Return False
                'Exit Function
            End If

            'Determine if Spouse
            'SR:2010.11.02  Change tax withholidng to 20% for Human Beneficiary
            IsHumanBeneficiary = IIf((l_dataset_RefundDetails.Tables("BeneficiaryRelationshipCode").Rows(0)("RelationshipCode").trim() <> "ES" And l_dataset_RefundDetails.Tables("BeneficiaryRelationshipCode").Rows(0)("RelationshipCode").trim() <> "TR" And l_dataset_RefundDetails.Tables("BeneficiaryRelationshipCode").Rows(0)("RelationshipCode").trim() <> "IN"), True, False)
            Return True
        Catch ex As Exception
            Throw
        End Try

    End Function

    'NP:PS:2007.07.19 - Updated code to calculate options and then bind the values to controls
    'Here we read values from the calculation tables and then populate the controls appropriately

    Private Sub BindDataToControls()

        'Here we populate the payee(Beneficiary) name and SSN details
        Dim l_DataRowNameSSNDetail As DataRow
        Dim dsBenRMDTaxrate As DataSet
        Dim strBenRMDTaxrate As String = String.Empty
        hdnRelationshipWithParticipant.Value = ""    ' SB | 2016.11.29 | YRS-AT-3022 | Set blank value for hidden parameter for isRolloverToOwn IRA option 

        'START : SB | 2017.12.18 | YRS-AT-3756 | Set default value for RMD minimum tax rate and default error message
        hdnMinRMDTaxRate.Value = "10"
        hdnRMDTaxRateErrorMessage.Value = "RMD Tax Rate can either be 0% or between 10% and 100%"
        'END : SB | 2017.12.18 | YRS-AT-3756 | Set default value for RMD minimum tax rate and default error message

        Try
            'Try to populate the information from the Retirement Plan option and if it is not available then try from the Savings Plan
            If isNonEmpty(RetirementPlan_RefundDetails) AndAlso isNonEmpty(RetirementPlan_RefundDetails.Tables("BeneficiaryNameSSNDetails")) Then
                l_DataRowNameSSNDetail = RetirementPlan_RefundDetails.Tables("BeneficiaryNameSSNDetails").Rows(0)

                'START : SB | 2016.11.29 | YRS-AT-3022 | Set the value for hidden parameter to detemine the relationship with beneficiary from retirement plan
                If HelperFunctions.isNonEmpty(RetirementPlan_RefundDetails.Tables("BeneficiaryRelationshipCode")) Then
                    hdnRelationshipWithParticipant.Value = Trim(RetirementPlan_RefundDetails.Tables("BeneficiaryRelationshipCode").Rows(0)("RelationshipCode")).ToUpper
                End If
                'END : SB | 2016.11.29 | YRS-AT-3022 | Set the value for hidden parameter to detemine the relationship with beneficiary from retirement plan

            Else
                If isNonEmpty(SavingsPlan_RefundDetails) AndAlso isNonEmpty(SavingsPlan_RefundDetails.Tables("BeneficiaryNameSSNDetails")) Then
                    l_DataRowNameSSNDetail = SavingsPlan_RefundDetails.Tables("BeneficiaryNameSSNDetails").Rows(0)

                    'START : SB | 2016.11.29 | YRS-AT-3022 | Set the value for hidden parameter to detemine the relationship with beneficiary from savings plan
                    If HelperFunctions.isNonEmpty(SavingsPlan_RefundDetails.Tables("BeneficiaryRelationshipCode")) Then
                        hdnRelationshipWithParticipant.Value = Trim(SavingsPlan_RefundDetails.Tables("BeneficiaryRelationshipCode").Rows(0)("RelationshipCode")).ToUpper
                    End If
                    'END : SB | 2016.11.29 | YRS-AT-3022 | Set the value for hidden parameter to detemine the relationship with beneficiary from savings plan

                End If
            End If

            If l_DataRowNameSSNDetail Is Nothing Then
                ' We were unable to obtain the Payee/Beneficiary name from the data we pulled from the database
                showMessage(PlaceHolder1, "Error", "Unable to obtain Payee/Beneficiary name", MessageBoxButtons.Stop)
                ErrorOnForm = True
                Exit Sub
            End If

            'Set the BeneficiaryName in the textbox
            Me.TextBoxPayee1Name_RP.Text = l_DataRowNameSSNDetail("FirstName").ToString.Trim() + " "
            If l_DataRowNameSSNDetail("MiddleName").ToString.Trim <> "" Then
                Me.TextBoxPayee1Name_RP.Text += l_DataRowNameSSNDetail("MiddleName").ToString.TrimStart.Substring(0, 1) + " "
            End If
            Me.TextBoxPayee1Name_RP.Text += l_DataRowNameSSNDetail("LastName").ToString.Trim()
            'Set the Savings Plan Beneficiary name also to the same name as the Retirement Plan name
            Me.TextBoxPayee1Name_SP.Text = Me.TextBoxPayee1Name_RP.Text

            'Shashi Shekhar     28 Feb 2011         Replacing Header formating with user control (YRS 5.0-450 )
            Headercontrol.PageTitle = " Beneficiary Withdrawal"
            Headercontrol.FundNo = l_DataRowNameSSNDetail("FundIdNo").ToString.Trim()


            'Call Function to set the values
            If isNonEmpty(RetirementPlan_RefundDetails) Then
                IsRetirementPlanOptionAvailable = True 'MMR | 2017.12.11 | YRS-AT-3742 | Set variable value to true if retirement plan option available
                RetirementPlan_RefundCalculations = CreateDataTable4FormData()          'Create a Datatable to Store the Key Values
                If GetRefundableAmounts(RetirementPlan_RefundDetails, RetirementPlan_RefundCalculations) = False Then
                    'Error Encountered
                    Exit Sub
                End If
                'Start-SR:2014.05.06 - YRS 5.0-2188: RMDs for beneficiaries
                If HelperFunctions.isNonEmpty(dsBeneficiaryRMDS) Then
                    SessionBeneficiaryRMDs.BeneficiaryRMDs_RP = dsBeneficiaryRMDS 'SR:2014.05.06 - YRS 5.0-2188: RMDs for beneficiaries
                Else
                    SessionBeneficiaryRMDs.BeneficiaryRMDs_RP = Nothing
                End If
                'End-SR:2014.05.06 - YRS 5.0-2188: RMDs for beneficiaries
            End If
            If isNonEmpty(SavingsPlan_RefundDetails) Then
                IsSavingsPlanOptionAvailable = True 'MMR | 2017.12.11 | YRS-AT-3742 | Set variable value to true if savings plan option available
                SavingsPlan_RefundCalculations = CreateDataTable4FormData()        'Create a Datatable to Store the Key Values
                If GetRefundableAmounts(SavingsPlan_RefundDetails, SavingsPlan_RefundCalculations) = False Then
                    'Error Encountered
                    Exit Sub
                End If
                'Start-SR:2014.05.06 - YRS 5.0-2188: RMDs for beneficiaries
                If HelperFunctions.isNonEmpty(dsBeneficiaryRMDS) Then
                    SessionBeneficiaryRMDs.BeneficiaryRMDs_SP = dsBeneficiaryRMDS
                Else
                    SessionBeneficiaryRMDs.BeneficiaryRMDs_SP = Nothing
                End If
                'End-SR:2014.05.06 - YRS 5.0-2188: RMDs for beneficiaries
            End If

            Dim g_dataTable_RefundCalculations As DataTable
            If isNonEmpty(RetirementPlan_RefundCalculations) Then
                'Set the default tax rate and fill the values in the textboxes
                g_dataTable_RefundCalculations = RetirementPlan_RefundCalculations
                If IsHumanBeneficiary = False Then
                    MinimumTaxRate = 0
                    g_dataTable_RefundCalculations.Rows(0)("nTaxRate") = 10
                Else
                    MinimumTaxRate = 20
                    g_dataTable_RefundCalculations.Rows(0)("nTaxRate") = 20
                End If
            End If
            If isNonEmpty(SavingsPlan_RefundCalculations) Then
                'Set the default tax rate and fill the values in the textboxes
                g_dataTable_RefundCalculations = SavingsPlan_RefundCalculations
                If IsHumanBeneficiary = False Then
                    MinimumTaxRate = 0
                    g_dataTable_RefundCalculations.Rows(0)("nTaxRate") = 10
                Else
                    MinimumTaxRate = 20
                    g_dataTable_RefundCalculations.Rows(0)("nTaxRate") = 20
                End If
            End If

            'Start- SR:2014.05.06 - YRS 5.0-2188: RMDs for beneficiaries
            SetVisibility_RMDControls_RetirementPlan(False)
            SetVisibility_RMDControls_SavingPlan(False)
            SetDefaultValue_RMDControls_RetirementPlan()
            SetDefaultValue_RMDControls_SavingPlan()
            RolloverOptions_RP.Items(1).Enabled = True
            RolloverOptions_SP.Items(1).Enabled = True

            dsBenRMDTaxrate = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("BENEFICIARY_RMD_TAXRATE")
            If HelperFunctions.isNonEmpty(dsBenRMDTaxrate) Then
                strBenRMDTaxrate = dsBenRMDTaxrate.Tables(0).Rows(0).Item("Value").ToString()
            End If

            'START : SB | 2017.12.18 | YRS-AT-3756 | Set RMD minimum tax rate and default error message
            If Not (String.IsNullOrEmpty(strBenRMDTaxrate)) Then
                hdnMinRMDTaxRate.Value = strBenRMDTaxrate
                hdnRMDTaxRateErrorMessage.Value = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_BENEF_SETTLE_RMD_INVALID_TAX_ERROR_MSG).DisplayText
            End If
            'END : SB | 2017.12.18 | YRS-AT-3756 | Set RMD minimum tax rate and default error message

            If isNonEmpty(RetirementPlan_RefundCalculations) Then
                If RetirementPlan_RefundCalculations.Rows(0)("nRMDTaxable") = 0 And RetirementPlan_RefundCalculations.Rows(0)("nRMDNonTaxable") = 0 Then
                    SetVisibility_RMDControls_RetirementPlan(False)
                    SetDefaultValue_RMDControls_RetirementPlan()
                Else
                    RolloverOptions_RP.Items(1).Enabled = False
                    SetVisibility_RMDControls_RetirementPlan(True)
                    PopulateBeneficiaryRMDs_Retirementplan(RetirementPlan_RefundCalculations, strBenRMDTaxrate)
                End If
            End If

            If isNonEmpty(SavingsPlan_RefundCalculations) Then
                If SavingsPlan_RefundCalculations.Rows(0)("nRMDTaxable") = 0 And SavingsPlan_RefundCalculations.Rows(0)("nRMDNonTaxable") = 0 Then
                    SetVisibility_RMDControls_SavingPlan(False)
                    SetDefaultValue_RMDControls_SavingPlan()
                Else
                    RolloverOptions_SP.Items(1).Enabled = False
                    SetVisibility_RMDControls_SavingPlan(True)
                    PopulateBeneficiaryRMDs_Savingplan(SavingsPlan_RefundCalculations, strBenRMDTaxrate)
                End If
            End If
            'End- SR:2014.05.06 - YRS 5.0-2188: RMDs for beneficiaries
            HandleCalculations()
        Catch ex As Exception
            HelperFunctions.LogException("Beneficiary Settlement --> BindDataToControls", ex)
            Throw
        End Try

    End Sub
    'NP:PS:2007.07.18 - Updated code to return the DataTable rather than populating it directly
    Private Function CreateDataTable4FormData() As DataTable
        Dim g_dataTable_RefundCalculations As New DataTable
        g_dataTable_RefundCalculations.Columns.Add(New DataColumn("nTaxRate", GetType(Double)))
        g_dataTable_RefundCalculations.Columns.Add(New DataColumn("nTaxable", GetType(Double)))
        g_dataTable_RefundCalculations.Columns.Add(New DataColumn("nNonTaxable", GetType(Double)))
        g_dataTable_RefundCalculations.Columns.Add(New DataColumn("RolloverInstitutionID", GetType(String)))
        g_dataTable_RefundCalculations.Columns.Add(New DataColumn("DeathBenefitOptionID", GetType(String)))
        g_dataTable_RefundCalculations.Columns.Add(New DataColumn("cAnnuityOption", GetType(String)))
        'Start- SR:2014.05.06 - YRS 5.0-2188: RMDs for beneficiaries
        g_dataTable_RefundCalculations.Columns.Add(New DataColumn("nRMDtaxable", GetType(Double)))
        g_dataTable_RefundCalculations.Columns.Add(New DataColumn("nRMDNonTaxable", GetType(Double)))
        'End-SR:2014.05.06 - YRS 5.0-2188: RMDs for beneficiaries
        Return g_dataTable_RefundCalculations
    End Function
    'NP:PS:2007.07.19 - Updated code to fill data into the parameters passed to the function
    'This function looks at the database rows and computes values for taxable, non-taxable and total and stores it into RefundCalculations
    Private Function GetRefundableAmounts(ByVal refundDetails As DataSet, ByRef refundCalculations As DataTable) As Boolean
        'GetRefundableAmounts
        'Determine what amounts are to use. The amount in the column mnyLumpSum is the Total amount to be Refunded
        Dim l_DataRowDeathBenefitOption As DataRow
        Dim l_StringOption As String
        Dim g_dataTable_RefundCalculations As New DataTable
        Dim l_datarow_RefundCalculations As DataRow
        Dim d_Deathbenefit As Decimal
        'Start- SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries
        Dim PersDeathdate As Date
        Dim dblTaxableAmtforComputingRMD As Decimal
        Dim dblNonTaxableAmtforComputingRMD As Decimal
        Dim blnIsHumanBeneficiary As Boolean
        Dim strRMDEligibledt As Date
        Dim strDecsdSeventyAndHalfdt As Date
        Dim dsDecsdDtls As DataSet
        Dim blnOnlyDeathBenfitAmountExist As Boolean
        Dim blnSettlementyearisnotDecsdyear As Boolean
        Dim strBeneficiaryType As String
        Dim intYearofParticipantsSeventynHalfage As Integer
        Dim intYearFollowingParticipantDeathDate As Integer
        'End- SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries
        Dim deceasedOrigBeneType As String = String.Empty    ' SR | 2016.12.07 | YRS-AT-3222 - Define Variable for Handling RMD for QDRO beneficiary
        'START: MMR | 2017.12.11 | YRS-AT-3742 | Declared variables to store participant taxable and non-taxable component
        Dim taxableAmt As Decimal
        Dim nonTaxableAmt As Decimal
        Dim rmdTaxableAmt As Decimal
        Dim rmdNonTaxableAmt As Decimal
        'END: MMR | 2017.12.11 | YRS-AT-3742 | Declared variables to store participant taxable and non-taxable component
        Dim requestResult As YMCAObjects.ReturnObject(Of Boolean) ' SR | 2017.12.04 | YRS-AT-3756 | initialise variable

        Try
            l_DataRowDeathBenefitOption = refundDetails.Tables("DeathBenefitOption").Rows(0)
            l_StringOption = l_DataRowDeathBenefitOption("COption").ToString.Trim + l_DataRowDeathBenefitOption("POption").ToString.Trim()
            'g_dataTable_RefundCalculations = CType(CacheFactory.GetCacheManager().GetData("g_dataTable_RefundCalculations"), DataTable)
            g_dataTable_RefundCalculations = refundCalculations

            'Add a new Blank Row
            l_datarow_RefundCalculations = g_dataTable_RefundCalculations.NewRow()

            'Start- SR:2014.05.06 - YRS 5.0-2188: DeathBenefit amt required to calculate beneficiary RMDs, hence removed outside from below if-else Statement
            If (IsDBNull(l_DataRowDeathBenefitOption("DeathBenefit"))) Then
                d_Deathbenefit = 0.0
            Else
                d_Deathbenefit = Convert.ToDecimal(l_DataRowDeathBenefitOption("DeathBenefit"))
            End If
            'End- SR:2014.05.06 - YRS 5.0-2188: DeathBenefit amt required to calculate beneficiary RMDs, hence remoived outside from below if-else Statement

            If (l_StringOption = "1B" Or l_StringOption = "C" Or l_StringOption = "2C" Or l_StringOption = "3C" Or l_StringOption = "4C") _
             Or (l_StringOption = "B" Or l_StringOption = "2B" Or l_StringOption = "3B" Or l_StringOption = "5B") _
             Or (l_StringOption = "4B" Or l_StringOption = "5B" Or l_StringOption = "5E" Or l_StringOption = "5F") _
             Or l_StringOption = "6A" Or l_StringOption = "6B" Or l_StringOption = "6E" Or l_StringOption = "7E" Or l_StringOption = "7F" Or l_StringOption = "6F" Then  '  added on 30 Oct 06 Vipul 2767 ... added new values for chvOptions 
                'Sanjay Rawat   2010.02.12 To add 6E,7E,6F,7F option For Market Based Withdrawa Refund in GetRefundableAmounts Proc.
                '&& Refund From Personal Reserves OR && Personal Reserves Plus Death Benefit OR && Personal Reserves + PIASR
                ' Case 1 OR Case 2 Or Case 4 in Foxpro
                'g_dataTable_RefundCalculations.Rows(0)("nTaxable") = Convert.ToDouble(l_DataRowDeathBenefitOption("LumpSum")) - Convert.ToDouble(l_DataRowDeathBenefitOption("PersonalPostTax"))
                'g_dataTable_RefundCalculations.Rows(0)("nNonTaxable") = Convert.ToDouble(l_DataRowDeathBenefitOption("PersonalPostTax").ToString)


                'l_datarow_RefundCalculations("nTaxable") = Convert.ToDouble(l_DataRowDeathBenefitOption("LumpSum")) - Convert.ToDouble(l_DataRowDeathBenefitOption("PersonalPostTax"))
                'l_datarow_RefundCalculations("nNonTaxable") = Convert.ToDouble(l_DataRowDeathBenefitOption("PersonalPostTax").ToString)

                If Convert.ToDecimal(l_DataRowDeathBenefitOption("LumpSum")) > Convert.ToDecimal(l_DataRowDeathBenefitOption("PersonalPostTax")) Then
                    l_datarow_RefundCalculations("nTaxable") = Convert.ToDecimal(l_DataRowDeathBenefitOption("LumpSum")) - Convert.ToDecimal(l_DataRowDeathBenefitOption("PersonalPostTax"))
                    l_datarow_RefundCalculations("nNonTaxable") = Convert.ToDecimal(l_DataRowDeathBenefitOption("PersonalPostTax").ToString)
                End If


                'SR:2010.11.02 YRS:1064 - If PostTaxAmt greater then LumpSum
                If Convert.ToDecimal(l_DataRowDeathBenefitOption("PersonalPostTax")) >= Convert.ToDecimal(l_DataRowDeathBenefitOption("LumpSum")) Then
                    'd_Deathbenefit = IIf(IsDBNull(l_DataRowDeathBenefitOption("DeathBenefit")), 0, Convert.ToDouble(l_DataRowDeathBenefitOption("DeathBenefit")))
                    l_datarow_RefundCalculations("nTaxable") = Convert.ToDecimal(l_DataRowDeathBenefitOption("DeathBenefit"))
                    l_datarow_RefundCalculations("nNonTaxable") = Convert.ToDecimal(l_DataRowDeathBenefitOption("LumpSum").ToString) - d_Deathbenefit
                End If

                ' Case 3 && Death Benefit
            ElseIf l_StringOption = "D" Or l_StringOption = "2D" Or l_StringOption = "3D" _
             Or l_StringOption = "7A" Or l_StringOption = "7B" Then '  added on 30 Oct 06 Vipul 2767 ... added new values for chvOptions
                'g_dataTable_RefundCalculations.Rows(0)("nTaxable") = Convert.ToDouble(l_DataRowDeathBenefitOption("LumpSum"))
                'g_dataTable_RefundCalculations.Rows(0)("nNonTaxable") = 0.0
                ' START | SR | 2017.04.17 | YRS-AT-3326 - If Deceased active participant have voluntary account then derive taxable, non-taxable components from posttax, pretax component of atsdeathbenefitoptions
                If (Convert.ToDecimal(l_DataRowDeathBenefitOption("PersonalPostTax") <> 0) AndAlso l_StringOption = "7B") Then
                    l_datarow_RefundCalculations("nTaxable") = Convert.ToDecimal(l_DataRowDeathBenefitOption("LumpSum")) - Convert.ToDecimal(l_DataRowDeathBenefitOption("PersonalPostTax"))
                    l_datarow_RefundCalculations("nNonTaxable") = Convert.ToDecimal(l_DataRowDeathBenefitOption("PersonalPostTax").ToString)
                Else
                    l_datarow_RefundCalculations("nTaxable") = Convert.ToDecimal(l_DataRowDeathBenefitOption("LumpSum"))
                    l_datarow_RefundCalculations("nNonTaxable") = 0.0
                End If
                ' END | SR | 2017.04.17 | YRS-AT-3326 - If Deceased active participant have voluntary account then derive taxable, non-taxable components from posttax, pretax component of atsdeathbenefitoptions
            Else
                Dim l_String_ErrorMessage As String
                l_String_ErrorMessage = "The Payment Option of the Selected Record: " + l_StringOption + " is NOT Recognized  for a Refund"
                showMessage(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                ' Return to Main Form 
                Return False
                Exit Function
            End If


            'Start- SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries
            PersDeathdate = Date.Parse(Session("g_DeathDate"))
            l_datarow_RefundCalculations("nRMDTaxable") = 0
            l_datarow_RefundCalculations("nRMDNonTaxable") = 0
            strDecsdSeventyAndHalfdt = Today.AddYears(1)
            strRMDEligibledt = Today.AddDays(1)
            blnIsHumanBeneficiary = IIf(IsHumanBeneficiary = False, False, True)
            blnSettlementyearisnotDecsdyear = IIf(PersDeathdate.Year = Today.Year, False, True)
            blnOnlyDeathBenfitAmountExist = IIf(((SessionBeneficiaryRMDs.DeceasedFundStatus = "DR" Or SessionBeneficiaryRMDs.DeceasedFundStatus = "DD") And (Convert.ToDecimal(l_DataRowDeathBenefitOption("LumpSum")) = d_Deathbenefit)), True, False)
            dblTaxableAmtforComputingRMD = 0
            dblNonTaxableAmtforComputingRMD = 0
            'START | SR | 2016.01.14 | YRS-AT-2188 - Get Beneficiary type to apply validation.
            If HelperFunctions.isNonEmpty(refundDetails.Tables("BeneficiaryRelationshipCode")) Then
                strBeneficiaryType = refundDetails.Tables("BeneficiaryRelationshipCode").Rows(0)(0).ToString().Trim()
            End If
            'END | SR | 2016.01.14 | YRS-AT-2188 - Get Beneficiary type to apply validation.

            ' START | SR | 2017.12.05 | YRS-AT-3756 | Commented below line of code to implement according to new requirement
            'If blnIsHumanBeneficiary AndAlso blnSettlementyearisnotDecsdyear Then 'SR | 2016.01.14 | YRS-AT-2188 - Remove changes to activate RMDs for Beneficiary.
            '    'If blnIsHumanBeneficiary AndAlso blnSettlementyearisnotDecsdyear AndAlso 1 = 2 Then '2015.02.11/AA/BT 2745 - Revert changes of YRS 5.0-2188 in 14.1.3 
            '    If blnOnlyDeathBenfitAmountExist = False Then
            '        'strRMDEligibledt = Date.Parse(YMCARET.YmcaBusinessObject.RefundBeneficiaryBOClass.GetPersonRMDEligibledate(Session("ParticipantEntityId")))
            '        'If strRMDEligibledt <= Today Then                
            '        dsDecsdDtls = YMCARET.YmcaBusinessObject.RefundBeneficiaryBOClass.GetPersonRMDEligibledate(Session("ParticipantEntityId"))
            '        If HelperFunctions.isNonEmpty(dsDecsdDtls) Then
            '            strRMDEligibledt = Date.Parse(dsDecsdDtls.Tables(0).Rows(0).Item("RMDBeginingDate").ToString())
            '            strDecsdSeventyAndHalfdt = Date.Parse(dsDecsdDtls.Tables(0).Rows(0).Item("PersSeventyAndHalfDate").ToString())
            '            deceasedOrigBeneType = dsDecsdDtls.Tables(0).Rows(0).Item("DeceasedOrigBeneType").ToString() 'SR | 2016.12.07 | YRS-AT-3222 - Get Deceased Beneficiary type
            '        End If
            '        'START | SR | 2016.12.07 | YRS-AT-3222 - Get Deceased Beneficiary type is QDRO then get QDRO details for RMD process
            '        If (deceasedOrigBeneType = "QDRO") Then
            '            strBeneficiaryType = "Non-Spouse"
            '        End If
            '        'END | SR | 2016.12.07 | YRS-AT-3222 - Get Deceased Beneficiary type is QDRO then get QDRO details for RMD process

            '        'If strDecsdSeventyAndHalfdt.Year <= Today.Year AndAlso strRMDEligibledt <= Today Then  'SR | 2016.01.12 | YRS-AT-2188 - Remove Validation
            '        ' SR | 2015.01.12 | Add Validation 
            '        '1. Beneficiary is eligible for RMD only if participant’s date of death is in a calendar year prior to the year of settlement.
            '        '2. If Dec. 31st of the year in which participant would have turned 70 ½ is later than Dec 31st of the year following the participant’s death, the spouse beneficiary is not eligible 
            '        If (PersDeathdate.Year < Today.Year) AndAlso
            '            ((strBeneficiaryType <> "SP") Or (strBeneficiaryType = "SP" And PersDeathdate.AddYears(1).Year >= strDecsdSeventyAndHalfdt.Year)) Then ' SR | 2015.01.12 | Add Validation
            '            If d_Deathbenefit > 0 AndAlso (SessionBeneficiaryRMDs.DeceasedFundStatus = "DR" Or SessionBeneficiaryRMDs.DeceasedFundStatus = "DD") AndAlso l_StringOption.Contains("B") Then
            '                dblTaxableAmtforComputingRMD = l_datarow_RefundCalculations("nTaxable") - d_Deathbenefit
            '                dblNonTaxableAmtforComputingRMD = l_datarow_RefundCalculations("nNonTaxable")
            '            Else
            '                dblTaxableAmtforComputingRMD = l_datarow_RefundCalculations("nTaxable")
            '                dblNonTaxableAmtforComputingRMD = l_datarow_RefundCalculations("nNonTaxable")
            '            End If

            '            If dblTaxableAmtforComputingRMD > 0 Or dblNonTaxableAmtforComputingRMD > 0 Then
            '                'START | SR | 2016.12.07 | YRS-AT-3222 - If deceased participant is alternate payee then call new set of procedure for RMD of alternate payee beneficiary
            '                If (deceasedOrigBeneType = "QDRO") Then
            '                    dsBeneficiaryRMDS = YMCARET.YmcaBusinessObject.RefundBeneficiaryBOClass.GetBeneficiaryRMDsForAltPayee(l_DataRowDeathBenefitOption("Uniqueid").ToString.Trim, dblTaxableAmtforComputingRMD, dblNonTaxableAmtforComputingRMD)
            '                Else
            '                    dsBeneficiaryRMDS = YMCARET.YmcaBusinessObject.RefundBeneficiaryBOClass.GetBeneficiaryRMDs(l_DataRowDeathBenefitOption("Uniqueid").ToString.Trim, dblTaxableAmtforComputingRMD, dblNonTaxableAmtforComputingRMD, strRMDEligibledt)
            '                End If
            '                'END | SR | 2016.12.07 | YRS-AT-3222 - If deceased participant is alternate payee then call new set of procedure for RMD of alternate payee beneficiary

            '            End If

            '            If HelperFunctions.isNonEmpty(dsBeneficiaryRMDS) Then
            '                If (String.IsNullOrEmpty(dsBeneficiaryRMDS.Tables(0).Rows(0).Item(0).ToString())) Then
            '                    l_datarow_RefundCalculations("nRMDTaxable") = 0.0
            '                Else
            '                    l_datarow_RefundCalculations("nRMDTaxable") = Convert.ToDecimal(dsBeneficiaryRMDS.Tables(0).Rows(0).Item(0))
            '                End If

            '                If (String.IsNullOrEmpty(dsBeneficiaryRMDS.Tables(0).Rows(0).Item(1).ToString())) Then
            '                    l_datarow_RefundCalculations("nRMDNonTaxable") = 0.0
            '                Else
            '                    l_datarow_RefundCalculations("nRMDNonTaxable") = Convert.ToDecimal(dsBeneficiaryRMDS.Tables(0).Rows(0).Item(1))
            '                End If

            '                'l_datarow_RefundCalculations("nRMDTaxable") = IIf(String.IsNullOrEmpty(dsBeneficiaryRMDS.Tables(0).Rows(0).Item(0).ToString), 0.0, Convert.ToDecimal(dsBeneficiaryRMDS.Tables(0).Rows(0).Item(0)))
            '                'l_datarow_RefundCalculations("nRMDNonTaxable") = IIf(String.IsNullOrEmpty(dsBeneficiaryRMDS.Tables(0).Rows(0).Item(0).ToString), 0.0, Convert.ToDecimal(dsBeneficiaryRMDS.Tables(0).Rows(0).Item(1)))
            '                l_datarow_RefundCalculations("nTaxable") = Convert.ToDecimal(l_datarow_RefundCalculations("nTaxable")) - l_datarow_RefundCalculations("nRMDTaxable")
            '                l_datarow_RefundCalculations("nNonTaxable") = Convert.ToDecimal(l_datarow_RefundCalculations("nNonTaxable")) - Convert.ToDecimal(l_datarow_RefundCalculations("nRMDNonTaxable"))
            '            End If
            '            'End If SR | 2015.01.01 | YRS-AT-2188 - Remove Validation
            '            'End If
            '        End If 'SR | 2015.01.01 | YRS-AT-2188 - Add Validation
            '    End If
            'End If
            ' END | SR | 2017.12.05 | YRS-AT-3756 | Commented below line of code to implement according to new requirement
            ' START | SR | 2017.12.04 | YRS-AT-3756 - Validate participant is RMD eligible or not. If participant is RMD eligible then allow him to calculate RMD.
            Logger.Write("GetRefundableAmounts-->ValidateParticipantRMDEligibility()", "Application", 1, 1, System.Diagnostics.TraceEventType.Information)
            requestResult = YMCARET.YmcaBusinessObject.RefundBeneficiaryBOClass.ValidateParticipantRMDEligibility(l_DataRowDeathBenefitOption("Uniqueid").ToString.Trim, IsHumanBeneficiary, d_Deathbenefit, Convert.ToDecimal(l_DataRowDeathBenefitOption("LumpSum")), SessionBeneficiaryRMDs.DeceasedFundStatus, PersDeathdate)

            'If Not requestResult.MessageList Is Nothing AndAlso requestResult.MessageList.Count > 0 Then
            '    If requestResult.MessageList(0).ToString() = "Non-HumanBeneficiary" Then
            '        IsNonHumanBeneficiaryExist = True
            '    End If
            'End If
            If requestResult.Value Then
                dsDecsdDtls = YMCARET.YmcaBusinessObject.RefundBeneficiaryBOClass.GetPersonRMDEligibledate(Session("ParticipantEntityId"))
                If HelperFunctions.isNonEmpty(dsDecsdDtls) Then
                    strRMDEligibledt = Date.Parse(dsDecsdDtls.Tables(0).Rows(0).Item("RMDBeginingDate").ToString())
                    strDecsdSeventyAndHalfdt = Date.Parse(dsDecsdDtls.Tables(0).Rows(0).Item("PersSeventyAndHalfDate").ToString())
                    deceasedOrigBeneType = dsDecsdDtls.Tables(0).Rows(0).Item("DeceasedOrigBeneType").ToString() 'SR | 2016.12.07 | YRS-AT-3222 - Get Deceased Beneficiary type
                End If
                'START | SR | 2016.12.07 | YRS-AT-3222 - Get Deceased Beneficiary type is QDRO then get QDRO details for RMD process
                If (deceasedOrigBeneType = "QDRO") Then
                    strBeneficiaryType = "Non-Spouse"
                End If
                'END | SR | 2016.12.07 | YRS-AT-3222 - Get Deceased Beneficiary type is QDRO then get QDRO details for RMD process


                '1. If Dec. 31st of the year in which participant would have turned 70 ½ is later than Dec 31st of the year following the participant’s death, the spouse beneficiary is not eligible 
                If ((strBeneficiaryType <> "SP") Or (strBeneficiaryType = "SP" And Today.Year >= strDecsdSeventyAndHalfdt.Year)) Then ' SR | 2015.01.12 | Add Validation
                    If d_Deathbenefit > 0 AndAlso (SessionBeneficiaryRMDs.DeceasedFundStatus = "DR" Or SessionBeneficiaryRMDs.DeceasedFundStatus = "DD") AndAlso l_StringOption.Contains("B") Then
                        dblTaxableAmtforComputingRMD = l_datarow_RefundCalculations("nTaxable") - d_Deathbenefit
                        dblNonTaxableAmtforComputingRMD = l_datarow_RefundCalculations("nNonTaxable")
                    Else
                        dblTaxableAmtforComputingRMD = l_datarow_RefundCalculations("nTaxable")
                        dblNonTaxableAmtforComputingRMD = l_datarow_RefundCalculations("nNonTaxable")
                    End If

                    If dblTaxableAmtforComputingRMD > 0 Or dblNonTaxableAmtforComputingRMD > 0 Then
                        'START | SR | 2016.12.07 | YRS-AT-3222 - If deceased participant is alternate payee then call new set of procedure for RMD of alternate payee beneficiary
                        If (deceasedOrigBeneType = "QDRO") Then
                            dsBeneficiaryRMDS = YMCARET.YmcaBusinessObject.RefundBeneficiaryBOClass.GetBeneficiaryRMDsForAltPayee(l_DataRowDeathBenefitOption("Uniqueid").ToString.Trim, dblTaxableAmtforComputingRMD, dblNonTaxableAmtforComputingRMD)
                        Else
                            dsBeneficiaryRMDS = YMCARET.YmcaBusinessObject.RefundBeneficiaryBOClass.GetBeneficiaryRMDs(l_DataRowDeathBenefitOption("Uniqueid").ToString.Trim, dblTaxableAmtforComputingRMD, dblNonTaxableAmtforComputingRMD, strRMDEligibledt)
                        End If
                        'END | SR | 2016.12.07 | YRS-AT-3222 - If deceased participant is alternate payee then call new set of procedure for RMD of alternate payee beneficiary
                    End If

                    If HelperFunctions.isNonEmpty(dsBeneficiaryRMDS) Then
                        If (String.IsNullOrEmpty(dsBeneficiaryRMDS.Tables(0).Rows(0).Item(0).ToString())) Then
                            l_datarow_RefundCalculations("nRMDTaxable") = 0.0
                        Else
                            l_datarow_RefundCalculations("nRMDTaxable") = Convert.ToDecimal(dsBeneficiaryRMDS.Tables(0).Rows(0).Item(0))
                        End If

                        If (String.IsNullOrEmpty(dsBeneficiaryRMDS.Tables(0).Rows(0).Item(1).ToString())) Then
                            l_datarow_RefundCalculations("nRMDNonTaxable") = 0.0
                        Else
                            l_datarow_RefundCalculations("nRMDNonTaxable") = Convert.ToDecimal(dsBeneficiaryRMDS.Tables(0).Rows(0).Item(1))
                        End If

                        l_datarow_RefundCalculations("nTaxable") = Convert.ToDecimal(l_datarow_RefundCalculations("nTaxable")) - l_datarow_RefundCalculations("nRMDTaxable")
                        l_datarow_RefundCalculations("nNonTaxable") = Convert.ToDecimal(l_datarow_RefundCalculations("nNonTaxable")) - Convert.ToDecimal(l_datarow_RefundCalculations("nRMDNonTaxable"))
                        'START: MMR | 2017.12.11 | YRS-AT-3742 | YRS Bug:(HOT FIX NEEDED) -participants with RMDs more than non-taxable amount are being forced to take excessive amt (TrackIT 31764)
                        taxableAmt = l_datarow_RefundCalculations("nTaxable")
                        nonTaxableAmt = l_datarow_RefundCalculations("nNonTaxable")
                        rmdTaxableAmt = l_datarow_RefundCalculations("nRMDTaxable")
                        rmdNonTaxableAmt = l_datarow_RefundCalculations("nRMDNonTaxable")

                        If (nonTaxableAmt + rmdNonTaxableAmt) >= (rmdTaxableAmt + rmdNonTaxableAmt) Then
                            'For total RMD Less than non-taxable amount, below steps are performed
                            '1. Add RMD taxable amount to RMD Non-taxable amount
                            '2. Deduct RMD Taxable amount from payee1 non-taxable amount
                            '3. Set RMD taxable to 0
                            '4. Add RMD taxable to rollover amt
                            l_datarow_RefundCalculations("nRMDNonTaxable") = rmdNonTaxableAmt + rmdTaxableAmt
                            l_datarow_RefundCalculations("nNonTaxable") = nonTaxableAmt - rmdTaxableAmt
                            l_datarow_RefundCalculations("nRMDTaxable") = 0
                            l_datarow_RefundCalculations("nTaxable") = taxableAmt + rmdTaxableAmt
                            'Set hidden field value to 1 if retiement plan option is available for rollover
                            If (IsRetirementPlanOptionAvailable) Then
                                hdnIsNonTaxableGreaterThanRMDRET.Value = "1"
                            End If
                            'Set hidden field value to 1 if savings plan option is available for rollover
                            If (IsSavingsPlanOptionAvailable) Then
                                hdnIsNonTaxableGreaterThanRMDSAV.Value = "1"
                            End If
                        ElseIf (nonTaxableAmt + rmdNonTaxableAmt) < (rmdTaxableAmt + rmdNonTaxableAmt) Then
                            'For total RMD greater than or equal to non-taxable amount, below steps are performed
                            '1. Add payee1 non-taxable amount to RMD Non-taxable amount
                            '2. Set payee1 non-taxable to 0
                            '3. Deduct payee1 non-Taxable amount from RMD taxable amount
                            '4. Add payee1 non-taxable to rollover amt
                            l_datarow_RefundCalculations("nRMDNonTaxable") = rmdNonTaxableAmt + nonTaxableAmt
                            l_datarow_RefundCalculations("nNonTaxable") = 0
                            l_datarow_RefundCalculations("nRMDTaxable") = rmdTaxableAmt - nonTaxableAmt
                            l_datarow_RefundCalculations("nTaxable") = taxableAmt + nonTaxableAmt
                        End If
                        'END: MMR | 2017.12.11 | YRS-AT-3742 | YRS Bug:(HOT FIX NEEDED) -participants with RMDs more than non-taxable amount are being forced to take excessive amt (TrackIT 31764)
                    End If
                    'End If SR | 2015.01.01 | YRS-AT-2188 - Remove Validation
                    'End If
                End If 'SR | 2015.01.01 | YRS-AT-2188 - Add Validation
            End If
            ' END | SR | 2017.12.04 | YRS-AT-3756 - Validate participant is RMD eligible or not. If participant is RMD eligible then allow him to calculate RMD.

            g_dataTable_RefundCalculations.Rows.Add(l_datarow_RefundCalculations)
            g_dataTable_RefundCalculations.AcceptChanges()
            refundCalculations = g_dataTable_RefundCalculations
            Return True
        Catch ex As Exception
            HelperFunctions.LogException("Beneficiary Settlement --> GetRefundableAmounts", ex)
            Throw
        End Try
    End Function

    'NP:PS:2007.07.19 - Updated code to fill data into the right textboxes and to perform calculations based on the data
    'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.

    Private Sub HandleCalculations()
        ' we will handle all the form calculations here
        'First we populate the Rollover textboxes with the selected rollover values

        Try
            Page.Validate()
            If Not Page.IsValid Then Exit Sub

            If RolloverOptions_RP.SelectedValue = "taxable" Then
                RolloverTaxable_RP.Text = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nTaxable").ToString()).ToString("#0.00")    'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
                RolloverNonTaxable_RP.Text = 0.0
                RolloverNet_RP.Text = RolloverTaxable_RP.Text
                RolloverPayeeName_RP.Enabled = True     'NP:PS:2007.09.05 - Handling issue 133
                ''SR:2011:01:03 - Handling issue YRS:5.0-1233
                RolloverNet_RP.Enabled = False
                RolloverTaxable_RP.Enabled = False
                RolloverNonTaxable_RP.Enabled = False
                RolloverPayeeName_RP.Enabled = False

            ElseIf RolloverOptions_RP.SelectedValue = "all" Then
                RolloverTaxable_RP.Text = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nTaxable").ToString()).ToString("#0.00")    'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
                RolloverNonTaxable_RP.Text = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nNonTaxable").ToString()).ToString("#0.00")  'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
                RolloverNet_RP.Text = (Convert.ToDecimal(RolloverTaxable_RP.Text) + Convert.ToDecimal(RolloverNonTaxable_RP.Text)).ToString("#0.00")    'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
                RolloverPayeeName_RP.Enabled = True     'NP:PS:2007.09.05 - Handling issue 133
                ''SR:2011:01:03 - Handling issue YRS:5.0-1233
                RolloverTaxable_RP.Enabled = False
                RolloverNonTaxable_RP.Enabled = False
                RolloverPayeeName_RP.Enabled = False
                RolloverNet_RP.Enabled = False

            ElseIf RolloverOptions_RP.SelectedValue = "Partial" Then  ''SR:2011:01:03 - Handling issue YRS:5.0-1233
                If RolloverOption = "RP" Then
                    RolloverTaxable_RP.Text = 0.0
                    RolloverNonTaxable_RP.Text = 0.0
                    RolloverNet_RP.Text = 0.0
                    RolloverTaxable_RP.Enabled = True
                    RolloverNonTaxable_RP.Enabled = True
                    RolloverTaxable_RP.ReadOnly = False
                    RolloverNonTaxable_RP.ReadOnly = False
                    RolloverPayeeName_RP.Enabled = True
                    RolloverNet_RP.Enabled = False
                End If
            Else
                RolloverTaxable_RP.Text = 0.0
                RolloverNonTaxable_RP.Text = 0.0
                RolloverNet_RP.Text = 0.0
                RolloverPayeeName_RP.Text = ""      'NP:PS:2007.09.05 - Handling issue 133
                RolloverPayeeName_RP.Enabled = False    'NP:PS:2007.09.05 - Handling issue 133                
            End If

            If RolloverOptions_SP.SelectedValue = "taxable" Then
                RolloverTaxable_SP.Text = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nTaxable").ToString()).ToString("#0.00")   'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
                RolloverNonTaxable_SP.Text = 0.0
                RolloverNet_SP.Text = RolloverTaxable_SP.Text
                RolloverPayeeName_SP.Enabled = True     'NP:PS:2007.09.05 - Handling issue 133
                ''SR:2011:01:03 - Handling issue YRS:5.0-1233
                RolloverTaxable_SP.Enabled = False
                RolloverNonTaxable_SP.Enabled = False
                RolloverPayeeName_SP.Enabled = False
                RolloverNet_SP.Enabled = False

            ElseIf RolloverOptions_SP.SelectedValue = "all" Then
                RolloverTaxable_SP.Text = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nTaxable").ToString()).ToString("#0.00")   'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
                RolloverNonTaxable_SP.Text = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nNonTaxable").ToString()).ToString("#0.00") 'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
                RolloverNet_SP.Text = (Convert.ToDecimal(RolloverTaxable_SP.Text) + Convert.ToDecimal(RolloverNonTaxable_SP.Text)).ToString("#0.00")    'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
                RolloverPayeeName_SP.Enabled = True     'NP:PS:2007.09.05 - Handling issue 133
                ''SR:2011:01:03 - Handling issue YRS:5.0-1233
                RolloverTaxable_SP.Enabled = False
                RolloverNonTaxable_SP.Enabled = False
                RolloverPayeeName_SP.Enabled = False
                RolloverNet_SP.Enabled = False
            ElseIf RolloverOptions_SP.SelectedValue = "Partial" Then
                If RolloverOption = "SP" Then
                    RolloverTaxable_SP.Text = 0.0
                    RolloverNonTaxable_SP.Text = 0.0
                    RolloverNet_SP.Text = 0.0
                    RolloverTaxable_SP.Enabled = True
                    RolloverNonTaxable_SP.Enabled = True
                    RolloverTaxable_SP.ReadOnly = False
                    RolloverNonTaxable_SP.ReadOnly = False
                    RolloverPayeeName_SP.Enabled = True
                    RolloverNet_SP.Enabled = False
                End If
            Else
                RolloverTaxable_SP.Text = 0.0
                RolloverNonTaxable_SP.Text = 0.0
                RolloverNet_SP.Text = 0.0
                RolloverPayeeName_SP.Text = ""      'NP:PS:2007.09.05 - Handling issue 133
                RolloverPayeeName_SP.Enabled = False    'NP:PS:2007.09.05 - Handling issue 133
            End If
            'Second we populate the Main Planwise textboxes

            If isNonEmpty(RetirementPlan_RefundCalculations) Then
                Me.TextBoxTaxable_RP.Text = (Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nTaxable")) _
                  - Convert.ToDecimal(Me.RolloverTaxable_RP.Text)).ToString("#0.00")  'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
                Me.TextBoxNonTaxable_RP.Text = (Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nNonTaxable")) _
                  - Convert.ToDecimal(Me.RolloverNonTaxable_RP.Text)).ToString("#0.00")   'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
                Me.TextBoxTaxRate_RP.Text = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nTaxRate"))
                Me.TextBoxTax_RP.Text = Math.Round(Convert.ToDouble(Me.TextBoxTaxable_RP.Text) * Convert.ToDouble(Me.TextBoxTaxRate_RP.Text) / 100, 2)
                Me.TextBoxNet_RP.Text = (Convert.ToDecimal(TextBoxTaxable_RP.Text) _
                 + Convert.ToDecimal(TextBoxNonTaxable_RP.Text) _
                  - Convert.ToDecimal(TextBoxTax_RP.Text)).ToString("#0.00")  'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
                dblTaxable_RP = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nTaxable"))
                dblNonTaxable_RP = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nNonTaxable"))
                'Start - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |Assigning RMD taxable and non-taxable values to variable for Retirement plan
                dblRMDTaxable_RP = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nRMDTaxable"))
                dblRMDNonTaxable_RP = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nRMDNonTaxable"))
                'End - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |Assigning RMD taxable and non-taxable values to variable for Retirement plan
            Else
                Me.TextBoxTaxable_RP.Text = 0.0
                Me.TextBoxNonTaxable_RP.Text = 0.0
                Me.TextBoxTaxRate_RP.Text = 0.0
                Me.TextBoxTax_RP.Text = 0.0
                Me.TextBoxNet_RP.Text = 0.0
                Me.RolloverPayeeName_RP.Text = ""       'NP:PS:2007.09.05 - Handling issue 133
                Me.RolloverPayeeName_RP.Enabled = False 'NP:PS:2007.09.05 - Handling issue 133
                dblTaxable_RP = 0
                dblNonTaxable_RP = 0
                'Start - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |setting taxable and non-taxable values to 0 for Retirement plan
                dblRMDTaxable_RP = 0
                dblRMDNonTaxable_RP = 0
                'End - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |setting taxable and non-taxable values to 0 for Retirement plan
            End If

            If isNonEmpty(SavingsPlan_RefundCalculations) Then

                Me.TextBoxTaxable_SP.Text = (Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nTaxable")) _
                  - Convert.ToDecimal(Me.RolloverTaxable_SP.Text)).ToString("#0.00")  'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
                Me.TextBoxNonTaxable_SP.Text = (Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nNonTaxable")) _
                  - Convert.ToDecimal(Me.RolloverNonTaxable_SP.Text)).ToString("#0.00")   'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
                Me.TextBoxTaxRate_SP.Text = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nTaxRate")) 'NP:YRS 5.0-653:2009.01.22 - Removing code for formatting of tax rate - .ToString("#0.00")    'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
                Me.TextBoxTax_SP.Text = Math.Round(Convert.ToDouble(Me.TextBoxTaxable_SP.Text) * Convert.ToDouble(Me.TextBoxTaxRate_SP.Text) / 100, 2)
                Me.TextBoxNet_SP.Text = (Convert.ToDecimal(TextBoxTaxable_SP.Text) _
                 + Convert.ToDecimal(TextBoxNonTaxable_SP.Text) _
                  - Convert.ToDecimal(TextBoxTax_SP.Text)).ToString("#0.00")
                dblTaxable_SP = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nTaxable"))
                dblNonTaxable_SP = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nNonTaxable"))
                'Start - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |Assigning RMD taxable and non-taxable values to variable for Savings plan
                dblRMDTaxable_SP = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nRMDTaxable"))
                dblRMDNonTaxable_SP = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nRMDNonTaxable"))
                'End - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |Assigning RMD taxable and non-taxable values to variable for Savings plan

            Else
                Me.TextBoxTaxable_SP.Text = 0.0
                Me.TextBoxNonTaxable_SP.Text = 0.0
                Me.TextBoxTaxRate_SP.Text = 0.0
                Me.TextBoxTax_SP.Text = 0.0
                Me.TextBoxNet_SP.Text = 0.0
                Me.RolloverPayeeName_SP.Text = ""       'NP:PS:2007.09.05 - Handling issue 133
                Me.RolloverPayeeName_SP.Enabled = False 'NP:PS:2007.09.05 - Handling issue 133
                dblTaxable_SP = 0
                dblNonTaxable_SP = 0
                'Start - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |setting taxable and non-taxable values to 0 for Savings plan
                dblRMDTaxable_SP = 0
                dblRMDNonTaxable_SP = 0
                'End - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |setting taxable and non-taxable values to 0 for Retirement plan

            End If

            'Finally we populate the Total textboxes based on values from the individual textboxes
            'Start- SR:2014.05.06 - YRS 5.0-2188: Added RMD taxable,nontaxable,taxrate, tax & net component
            Me.TextboxTotalTaxable.Text = IIf(TextBoxTaxable_RP.Text <> "", Convert.ToDecimal(TextBoxTaxable_RP.Text), 0.0) + IIf(TextBoxTaxable_SP.Text = "", 0.0, Convert.ToDecimal(TextBoxTaxable_SP.Text)) _
               + IIf(RolloverTaxable_RP.Text = "", 0.0, Convert.ToDecimal(RolloverTaxable_RP.Text)) + IIf(RolloverTaxable_SP.Text = "", 0.0, Convert.ToDecimal(RolloverTaxable_SP.Text)) + IIf(txtRMDTaxable_RP.Text = "", 0.0, Convert.ToDecimal(txtRMDTaxable_RP.Text)) + IIf(txtRMDTaxable_SP.Text = "", 0.0, Convert.ToDecimal(txtRMDTaxable_SP.Text)) 'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.

            Me.TextboxTotalNonTaxable.Text = IIf(TextBoxNonTaxable_RP.Text = "", 0.0, Convert.ToDecimal(TextBoxNonTaxable_RP.Text)) + IIf(TextBoxNonTaxable_SP.Text = "", 0.0, Convert.ToDecimal(TextBoxNonTaxable_SP.Text)) _
               + IIf(RolloverNonTaxable_RP.Text = "", 0.0, Convert.ToDecimal(RolloverNonTaxable_RP.Text)) + IIf(RolloverNonTaxable_SP.Text = "", 0.0, Convert.ToDecimal(RolloverNonTaxable_SP.Text)) + IIf(txtRMDNonTaxable_RP.Text = "", 0.0, Convert.ToDecimal(txtRMDNonTaxable_RP.Text)) + IIf(txtRMDNonTaxable_SP.Text = "", 0.0, Convert.ToDecimal(txtRMDNonTaxable_SP.Text)) 'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
            Me.TextboxTotalTax.Text = IIf(TextBoxTax_RP.Text = "", 0.0, Convert.ToDecimal(TextBoxTax_RP.Text)) + IIf(TextBoxTax_SP.Text = "", 0.0, Convert.ToDecimal(TextBoxTax_SP.Text)) + IIf(txtRMDTax_RP.Text = "", 0.0, Convert.ToDecimal(txtRMDTax_RP.Text)) + IIf(txtRMDTax_SP.Text = "", 0.0, Convert.ToDecimal(txtRMDTax_SP.Text))  'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
            Me.TextboxTotalNet.Text = IIf(TextBoxNet_RP.Text = "", 0.0, Convert.ToDecimal(TextBoxNet_RP.Text)) + IIf(TextBoxNet_SP.Text = "", 0.0, Convert.ToDecimal(TextBoxNet_SP.Text)) _
               + IIf(RolloverNet_RP.Text = "", 0.0, Convert.ToDecimal(RolloverNet_RP.Text)) + IIf(RolloverNet_SP.Text = "", 0.0, Convert.ToDecimal(RolloverNet_SP.Text)) + IIf(txtRMDNet_RP.Text = "", 0.0, Convert.ToDecimal(txtRMDNet_RP.Text)) + IIf(txtRMDNet_SP.Text = "", 0.0, Convert.ToDecimal(txtRMDNet_SP.Text))   'NP:BT-315:2007.12.12 - Updated code to convert and round the data to two decimal places.
            'End- SR:2014.05.06 - YRS 5.0-2188: Added RMD taxable,nontaxable,taxrate, tax & net component

            'Finally set the textbox controls to autosubmit and autoformat from the UI side
            'NP:PS: dont think we need this piece of code.
            'Me.RolloverTaxable_RP.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            'Me.RolloverTaxable_RP.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
            'Me.RolloverNonTaxable_RP.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            'Me.RolloverNonTaxable_RP.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")

            Me.TextBoxTaxRate_RP.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            ''Me.TextBoxTaxRate_RP.Attributes.Add("onChange", "if (ValidateRange(this, 0, 100) == true) { __doPostBack('" + Me.TextBoxTaxRate_RP.ClientID + "', ''); } else { return false; }")
            Me.TextBoxTax_RP.Attributes.Add("onkeypress", "return HandleAmountFiltering(this);")
            ''Me.TextBoxTax_RP.Attributes.Add("onChange", "if (ValidateRange(this, 0, " + Me.TextBoxTaxable_RP.Text + ") == true) { __doPostBack('" + Me.TextBoxTax_RP.ClientID + "', ''); } else { return false; }")

            'Me.RolloverTaxable_SP.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            'Me.RolloverTaxable_SP.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")
            'Me.RolloverNonTaxable_SP.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            'Me.RolloverNonTaxable_SP.Attributes.Add("onchange", "javascript:FormatAmtControl(this);")

            Me.TextBoxTaxRate_SP.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")   'HandleAmountFilteringWithNoDecimals
            ''Me.TextBoxTaxRate_SP.Attributes.Add("onChange", "if (ValidateRange(this, 0, 100) == true) { __doPostBack('" + Me.TextBoxTaxRate_SP.ClientID + "', ''); } else { return false; }")
            Me.TextBoxTax_SP.Attributes.Add("onkeypress", "Javascript:return HandleAmountFiltering(this);")
            ''Me.TextBoxTax_SP.Attributes.Add("onChange", "if (ValidateRange(this, 0, " + Me.TextBoxTaxable_SP.Text + ") == true) { __doPostBack('" + Me.TextBoxTax_SP.ClientID + "', ''); } else { return false; }")

            'Alternate approach using Range validators
            'Priya : 11/1/2011 BT-696,YRS 5.0-1233 : Allow input of rollover amount Remove range validator as it will handle by j-query
            'rvTaxAmount_RP.MaximumValue = TextBoxTaxable_RP.Text
            'rvTaxAmount_SP.MaximumValue = TextBoxTaxable_SP.Text
            '' 2010.12.27 Commented by sanjay to avoid postback
            'TextBoxTax_RP.AutoPostBack = True : TextBoxTax_SP.AutoPostBack = True
            'TextBoxTaxRate_RP.AutoPostBack = True : TextBoxTaxRate_SP.AutoPostBack = True
            '    RolloverTaxable_RP.Attributes.Add("Onblur", "javascript:Calculate_Taxable_RP(" & Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nTaxable")) & ");javascript:Calculate_Total();")
            '    RolloverNonTaxable_RP.Attributes.Add("Onblur", "javascript:Calculate_NonTaxable_RP(" & Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nNonTaxable")) & ");javascript:Calculate_Total();")
            '    RolloverTaxable_SP.Attributes.Add("Onblur", "javascript:Calculate_Taxable_SP(" & Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nTaxable")) & ");javascript:Calculate_Total();")
            'RolloverNonTaxable_SP.Attributes.Add("Onblur", "javascript:Calculate_NonTaxable_SP(" & Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nTaxable")) & ");javascript:Calculate_Total();")
            'TextBoxTaxRate_RP.Attributes.Add("Onblur", "javascript:Calculate_Tax_RP();javascript:Calculate_Total();")
            'RolloverTaxable_RP.Attributes.Add("Onblur", "javascript:Calculate_Taxable_RP(" & Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nTaxable")) & ", " & Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nNonTaxable")) & ");javascript:Calculate_Total();")
            'RolloverNonTaxable_RP.Attributes.Add("Onblur", "javascript:Calculate_Taxable_RP(" & Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nTaxable")) & ", " & Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nNonTaxable")) & ");javascript:Calculate_Total();")
            'RolloverTaxable_SP.Attributes.Add("Onblur", "javascript:Calculate_Taxable_SP(" & Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nTaxable")) & ", " & Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nNonTaxable")) & ");javascript:Calculate_Total();")
            'RolloverNonTaxable_SP.Attributes.Add("Onblur", "javascript:Calculate_Taxable_SP(" & Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nTaxable")) & ", " & Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nNonTaxable")) & ");javascript:Calculate_Total();")

            ''SR:2011.01.10 - YRS 5.0:1233 - To register JQuery function 'InitializeScript'
            'Start - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |Commented existing code and passing RMD taxable and Non-taxable parameters to initializeControls method
            'ClientScript.RegisterClientScriptBlock(Me.GetType(), "InitializeScript", "<script language='javascript'>$(document).ready(function () { initializeControls(" & dblTaxable_RP & ", " & dblNonTaxable_RP & ", " & dblTaxable_SP & ", " & dblNonTaxable_SP & "); });</script>")
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "InitializeScript", "<script language='javascript'>$(document).ready(function () { initializeControls(" & dblTaxable_RP & ", " & dblNonTaxable_RP & ", " & dblTaxable_SP & ", " & dblNonTaxable_SP & ", " & dblRMDTaxable_RP & ", " & dblRMDNonTaxable_RP & ", " & dblRMDTaxable_SP & ", " & dblRMDNonTaxable_SP & "); });</script>")
            'End - Manthan Rajguru | 2016.07.11 | YRS-AT-2911 |Commented existing code and passing RMD taxable and Non-taxable parameters to initializeControls method
        Catch ex As Exception
            HelperFunctions.LogException("Beneficiary Settlement --> HandleCalculations", ex)
            Throw
        End Try
    End Sub

    'NP:PS:2007.07.19 - No need to change this code
    Private Sub CloseThisForm()

        Dim l_string_clientScript As String = "<script language='javascript'>" & _
          "window.opener.document.forms(0).submit();" & _
           "self.close()" & _
          "</script>"

        If (Not IsClientScriptBlockRegistered("clientScript")) Then
            RegisterClientScriptBlock("clientScript", l_string_clientScript)
        End If
    End Sub


#End Region

#Region "Unwanted Code"
    'NP:PS:2007.07.20 - Replaced this code with builtin Range Validator controls.
    ''SR:2011.01.10 - validation to validate JQuery code
    'NP:TODO - This section needs to be rewritten
    Private Function SaveValidationsPassed()
        Dim l_String_ErrorMessage As String
        l_String_ErrorMessage = ""

        If isNonEmpty(RetirementPlan_RefundCalculations) Then
            If (Convert.ToDecimal(TextBoxTax_RP.Text) < 0 Or Convert.ToDecimal(TextBoxTax_RP.Text) > Convert.ToDecimal(TextBoxTaxable_RP.Text)) Then
                l_String_ErrorMessage = "Tax amount must be between 0 and value in Taxable Box."
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If

            If (Convert.ToDecimal(TextBoxTaxRate_RP.Text) < 0 Or Convert.ToDecimal(TextBoxTaxRate_RP.Text) > 100) Then
                l_String_ErrorMessage = "Tax rate must be between 0 and 100."
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If
            'BS:2011.09.09:BT:739--put condition for rollover retrie and saving 
            If (Convert.ToDecimal(TextBoxNet_RP.Text) < 0.01) AndAlso (Convert.ToDecimal(Me.TextBoxTaxable_RP.Text) <> 0 Or Convert.ToDecimal(Me.TextBoxNonTaxable_RP.Text) <> 0) Then
                l_String_ErrorMessage = "Net amount can not be less than $0.01."
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If
        End If

        If isNonEmpty(SavingsPlan_RefundCalculations) Then
            If (Convert.ToDecimal(TextBoxTax_SP.Text) < 0 Or Convert.ToDecimal(TextBoxTax_SP.Text) > Convert.ToDecimal(TextBoxTaxable_SP.Text)) Then
                l_String_ErrorMessage = "Tax amount must be between 0 and value in Taxable Box."
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If

            If (Convert.ToDecimal(TextBoxTaxRate_SP.Text) < 0 Or Convert.ToDecimal(TextBoxTaxRate_SP.Text) > 100) Then
                l_String_ErrorMessage = "Tax rate must be between 0 and 100."
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If
            'BS:2011.09.09:BT:739--put condition for rollover retrie and saving 
            If (Convert.ToDecimal(TextBoxNet_SP.Text) < 0.01) AndAlso (Convert.ToDecimal(Me.TextBoxTaxable_SP.Text) <> 0 Or Convert.ToDecimal(Me.TextBoxNonTaxable_SP.Text) <> 0) Then
                l_String_ErrorMessage = "Net amount can not be less than $0.01."
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If

        End If

        'Validation for Retirement plan
        If RolloverOptions_RP.SelectedValue = "all" Then

            If Convert.ToDecimal(Me.RolloverTaxable_RP.Text) <> Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nTaxable")) Then
                l_String_ErrorMessage = "Rollover Taxable cannot be less than taxable amount"
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If

            If Convert.ToDecimal(Me.RolloverNonTaxable_RP.Text) <> Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nNonTaxable")) Then
                l_String_ErrorMessage = "Rollover Non-taxable cannot be less than non-taxable amount"
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If

            If Convert.ToDecimal(Me.TextBoxTaxable_RP.Text) <> 0 Or Convert.ToDecimal(Me.TextBoxNonTaxable_RP.Text) <> 0 Then
                l_String_ErrorMessage = "Taxable/Non-Taxable cannot be greater than zero"
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If

        ElseIf RolloverOptions_RP.SelectedValue = "taxable" Then

            'If Me.RolloverPayeeName_RP.Text.Trim() = "" Then
            '    l_String_ErrorMessage = "Refund Beneficiary - Rollover Name is Blank, Please Enter the Name of the Rollover Institution"
            '    MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
            '    Return False
            '    Exit Function
            'End If

        ElseIf RolloverOptions_RP.SelectedValue = "Partial" Then

            If (Convert.ToDecimal(Me.RolloverTaxable_RP.Text) = 0 And Convert.ToDecimal(Me.RolloverNonTaxable_RP.Text) = 0) Then
                l_String_ErrorMessage = "Rollover Taxable/Non-Taxable cannot be zero"
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If
            If RetirementPlan_OptionId <> "" AndAlso HelperFunctions.isEmpty(RetirementPlan_RefundCalculations) Then
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", "There seems to be problem. Please attempt this process again. If you continue to see this message please contact IT support.", MessageBoxButtons.Stop)
                HelperFunctions.LogMessage(String.Format("Retirement Plan option id = {0}, but the RetirementPlan_RefundCalculations dataset was not found.", SavingsPlan_OptionId))
                Return False
            End If
            'If HelperFunctions.isNonEmpty(RetirementPlan_RefundCalculations) Then
            '	Dim taxable As Decimal = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0)("nTaxable"))
            '	Dim nontaxable As Decimal = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0)("nNonTaxable"))
            '	Dim total As Decimal = taxable + nontaxable
            '	Dim reqRolloverAmt As Decimal = Decimal.Parse(RolloverPartialAmount_RP.Text)
            '	Dim reqRollTaxableAmt As Decimal = Decimal.Parse(RolloverTaxable_RP.Text)
            '	Dim reqRollNonTaxableAmt As Decimal = Decimal.Parse(RolloverNonTaxable_RP.Text)
            '	Dim expRollNonTaxableAmt As Decimal = Math.Round(nontaxable * reqRolloverAmt / total, 2)
            '	Dim expRollTaxableAmt As Decimal = reqRolloverAmt - expRollNonTaxableAmt
            '	Dim reqTaxableAmt As Decimal = Decimal.Parse(TextBoxTaxable_RP.Text)
            '	Dim reqNonTaxableAmt As Decimal = Decimal.Parse(TextBoxNonTaxable_RP.Text)
            '	Dim expTaxableAmt As Decimal = taxable - expRollTaxableAmt
            '	Dim expNonTaxableAmt As Decimal = nontaxable - expRollNonTaxableAmt
            '	If (reqRollNonTaxableAmt <> expRollNonTaxableAmt OrElse reqRollTaxableAmt <> expRollTaxableAmt OrElse reqTaxableAmt <> expTaxableAmt OrElse reqNonTaxableAmt <> expNonTaxableAmt) Then
            '		MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", "There has been an error in computing the values. Please verify the values on the screen or contact support.", MessageBoxButtons.Stop)
            '		HelperFunctions.LogMessage( _
            '		  String.Format("Requested Rollover of {0} out of {1}. Available: T={2}, NT={3}. Expected Rollover: T={4}, NT={5}. Actual Rollover: T={6}, NT={7}. Expected Payee1: T={8}, NT={9}. Actual Payee1: T={10}, NT={11}.", _
            '		  reqRolloverAmt, total, taxable, nontaxable, expRollTaxableAmt, expRollNonTaxableAmt, reqRollTaxableAmt, reqRollNonTaxableAmt, expTaxableAmt, expNonTaxableAmt, reqTaxableAmt, reqNonTaxableAmt))
            '		Return False
            '	End If
            'BS:2011.08.25:YRS 5.0-1326 - call common fuction for refund calculation
            If (RefundCalculation(RetirementPlan_RefundCalculations, RolloverPartialAmount_RP, RolloverTaxable_RP,
            RolloverNonTaxable_RP, TextBoxTaxable_RP,
            TextBoxNonTaxable_RP)) = False Then Return False

        ElseIf RolloverOptions_RP.SelectedValue = "none" Then
            If Convert.ToDecimal(Me.RolloverTaxable_RP.Text) <> 0 Then
                l_String_ErrorMessage = "Rollover Taxable cannot be greater than zero"
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If
            If Convert.ToDecimal(Me.RolloverNonTaxable_RP.Text) <> 0 Then
                l_String_ErrorMessage = "Rollover non-Taxable cannot be greater than zero"
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If

        End If

        'Valiation for saving plan
        If RolloverOptions_SP.SelectedValue = "all" Then

            If Convert.ToDecimal(Me.RolloverTaxable_SP.Text) <> Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nTaxable")) Then
                l_String_ErrorMessage = "Rollover taxable cannot be less than Texable amount"
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If

            If Convert.ToDecimal(Me.RolloverNonTaxable_SP.Text) <> Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nNonTaxable")) Then
                l_String_ErrorMessage = "Rollover Non-taxable cannot be less than Non-Texable amount"
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If

            If Convert.ToDecimal(Me.TextBoxTaxable_SP.Text) <> 0 Or Convert.ToDecimal(Me.TextBoxNonTaxable_SP.Text) <> 0 Then
                l_String_ErrorMessage = "Taxable/Non-Taxable cannot be greater than zero"
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If

        ElseIf RolloverOptions_SP.SelectedValue = "taxable" Then

            'If Me.RolloverPayeeName_SP.Text.Trim() = "" Then
            '    l_String_ErrorMessage = "Refund Beneficiary - Rollover Name is Blank, Please Enter the Name of the Rollover Institution"
            '    MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
            '    Return False
            '    Exit Function
            'End If


        ElseIf RolloverOptions_SP.SelectedValue = "Partial" Then

            If (Convert.ToDecimal(Me.RolloverTaxable_SP.Text) = 0 And Convert.ToDecimal(Me.RolloverNonTaxable_SP.Text) = 0) Then
                l_String_ErrorMessage = "Rollover taxable/Non-taxable cannot be zero"
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If
            If SavingsPlan_OptionId <> "" AndAlso HelperFunctions.isEmpty(SavingsPlan_RefundCalculations) Then
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", "There seems to be problem. Please attempt this process again. If you continue to see this message please contact IT support.", MessageBoxButtons.Stop)
                HelperFunctions.LogMessage(String.Format("Savings Plan option id = {0}, but the SavingsPlan_RefundCalculations dataset was not found.", SavingsPlan_OptionId))
                Return False
            End If
            'If HelperFunctions.isNonEmpty(SavingsPlan_RefundCalculations) Then
            'Dim taxable As Decimal = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0)("nTaxable"))
            'Dim nontaxable As Decimal = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0)("nNonTaxable"))
            'Dim total As Decimal = taxable + nontaxable
            'Dim reqRolloverAmt As Decimal = Decimal.Parse(RolloverPartialAmount_SP.Text)
            'Dim reqRollTaxableAmt As Decimal = Decimal.Parse(RolloverTaxable_SP.Text)
            'Dim reqRollNonTaxableAmt As Decimal = Decimal.Parse(RolloverNonTaxable_SP.Text)
            'Dim expRollNonTaxableAmt As Decimal = Math.Round(nontaxable * reqRolloverAmt / total, 2)
            'Dim expRollTaxableAmt As Decimal = total - expRollNonTaxableAmt
            'Dim reqTaxableAmt As Decimal = Decimal.Parse(TextBoxTaxable_SP.Text)
            'Dim reqNonTaxableAmt As Decimal = Decimal.Parse(TextBoxNonTaxable_SP.Text)
            'Dim expTaxableAmt As Decimal = taxable - expTaxableAmt
            'Dim expNonTaxableAmt As Decimal = nontaxable - expRollNonTaxableAmt
            'If (reqRollNonTaxableAmt <> expRollNonTaxableAmt OrElse reqRollTaxableAmt <> expRollTaxableAmt OrElse reqTaxableAmt <> expTaxableAmt OrElse reqNonTaxableAmt <> expNonTaxableAmt) Then
            '	MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", "There has been an error in computing the values. Please verify the values on the screen or contact support.", MessageBoxButtons.Stop)
            '	HelperFunctions.LogMessage( _
            '	  String.Format("Requested Rollover of {0} out of {1}. Available: T={2}, NT={3}. Expected Rollover: T={4}, NT={5}. Actual Rollover: T={6}, NT={7}. Expected Payee1: T={8}, NT={9}. Actual Payee1: T={10}, NT={11}.", _
            '	  reqRolloverAmt, total, taxable, nontaxable, expRollTaxableAmt, expRollNonTaxableAmt, reqRollTaxableAmt, reqRollNonTaxableAmt, expTaxableAmt, expNonTaxableAmt, reqTaxableAmt, reqNonTaxableAmt))
            '	Return False
            'End If
            'BS:2011.08.25:YRS 5.0-1326 - call common fuction for refund calculation
            If (RefundCalculation(SavingsPlan_RefundCalculations, RolloverPartialAmount_SP, RolloverTaxable_SP,
             RolloverNonTaxable_SP, TextBoxTaxable_SP,
             TextBoxNonTaxable_SP)) = False Then Return False

        ElseIf RolloverOptions_SP.SelectedValue = "none" Then
            If Convert.ToDecimal(Me.RolloverTaxable_SP.Text) <> 0 Then
                l_String_ErrorMessage = "Rollover Taxable cannot be greater than zero"
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If
            If Convert.ToDecimal(Me.RolloverNonTaxable_SP.Text) <> 0 Then
                l_String_ErrorMessage = "Rollover non-Taxable cannot be greater than zero"
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", l_String_ErrorMessage, MessageBoxButtons.Stop)
                Return False
                Exit Function
            End If

        End If
        'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Validating Deduction Amount
        If Convert.ToDecimal(Me.TextboxDeductions.Text) > (Convert.ToDecimal(Me.TextboxTotalTaxable.Text) + Convert.ToDecimal(Me.TextboxTotalNonTaxable.Text) - Convert.ToDecimal(Me.TextboxTotalTax.Text)) Then
            l_String_ErrorMessage = "Deductions amount cannot be greater than net amount"
            MessageBox.Show(PlaceHolder1, "Refund Beneficiary", l_String_ErrorMessage, MessageBoxButtons.Stop)
            Return False
        End If
        'End - Manthan | 2016.04.22 | YRS-AT-2206 | Validating Deduction Amount
        'NP:2011.08.22 - Removing unwanted session code - Session("SP_Parameters_RolloverInstitutionID") = Nothing
        Return True
    End Function
    'BS:2011.08.25:YRS 5.0-1326 - create common fuction for refund calculation
    Private Function RefundCalculation(ByRef dsRetrSav As DataTable, ByRef reqRolloverAmt As TextBox,
   ByRef reqRollTaxableAmt As TextBox, ByRef reqRollNonTaxableAmt As TextBox,
   ByRef reqTaxableAmt As TextBox, ByRef reqNonTaxableAmt As TextBox) As Boolean
        If HelperFunctions.isNonEmpty(dsRetrSav) Then
            Dim taxable As Decimal = Convert.ToDecimal(dsRetrSav.Rows(0)("nTaxable"))
            Dim nontaxable As Decimal = Convert.ToDecimal(dsRetrSav.Rows(0)("nNonTaxable"))
            Dim total As Decimal = taxable + nontaxable
            'Start:2014.10.07/SR/BT 2672/YRS 5.0-2422:allow rollovoers of pre-tax money only.
            'Dim expRollNonTaxableAmt As Decimal = Math.Round(nontaxable * Decimal.Parse(reqRolloverAmt.Text) / total, 2)
            'Dim expRollTaxableAmt As Decimal = Decimal.Parse(reqRolloverAmt.Text) - expRollNonTaxableAmt
            'Dim expTaxableAmt As Decimal = taxable - expRollTaxableAmt
            'Dim expNonTaxableAmt As Decimal = nontaxable - expRollNonTaxableAmt
            Dim expRollTaxableAmt As Decimal
            Dim expRollNonTaxableAmt As Decimal
            Dim expTaxableAmt As Decimal
            Dim expNonTaxableAmt As Decimal

            If (reqRolloverAmt.Text <= taxable) Then
                expRollTaxableAmt = Decimal.Parse(reqRolloverAmt.Text)
                expRollNonTaxableAmt = 0
                expTaxableAmt = taxable - expRollTaxableAmt
                expNonTaxableAmt = nontaxable - expRollNonTaxableAmt
            End If
            If (reqRolloverAmt.Text > taxable) Then
                expRollTaxableAmt = taxable
                expRollNonTaxableAmt = Decimal.Parse(reqRolloverAmt.Text) - taxable
                expTaxableAmt = taxable - expRollTaxableAmt
                expNonTaxableAmt = nontaxable - expRollNonTaxableAmt
            End If
            'End:2014.10.07/SR/BT 2672/YRS 5.0-2422:allow rollovoers of pre-tax money only.




            If (Decimal.Parse(reqRollNonTaxableAmt.Text) <> expRollNonTaxableAmt OrElse Decimal.Parse(reqRollTaxableAmt.Text) <> expRollTaxableAmt OrElse Decimal.Parse(reqTaxableAmt.Text) <> expTaxableAmt OrElse Decimal.Parse(reqNonTaxableAmt.Text) <> expNonTaxableAmt) Then
                MessageBox.Show(PlaceHolder1, "Refund Beneficiary ", "There has been an error in computing the values. Please verify the values on the screen or contact support.", MessageBoxButtons.Stop)
                HelperFunctions.LogMessage( _
                  String.Format("Requested Rollover of {0} out of {1}. Available: T={2}, NT={3}. Expected Rollover: T={4}, NT={5}. Actual Rollover: T={6}, NT={7}. Expected Payee1: T={8}, NT={9}. Actual Payee1: T={10}, NT={11}.", _
                  Decimal.Parse(reqRolloverAmt.Text), total, taxable, nontaxable, expRollTaxableAmt, expRollNonTaxableAmt, Decimal.Parse(reqRollTaxableAmt.Text), Decimal.Parse(reqRollNonTaxableAmt.Text), expTaxableAmt, expNonTaxableAmt, Decimal.Parse(reqTaxableAmt.Text), Decimal.Parse(reqNonTaxableAmt.Text)))
                Return False
                Exit Function
            End If
        End If
        Return True
    End Function
#End Region

    'Start- SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries
    Public Sub SetVisibility_RMDControls_RetirementPlan(ByVal value As Boolean)
        Try
            'lblRMD_RP.Visible = value
            txtRMDTaxRate_RP.Visible = value
            txtRMDTaxable_RP.Visible = value
            txtRMDNonTaxable_RP.Visible = value
            txtRMDNet_RP.Visible = value
            txtRMDTax_RP.Visible = value
            lblRMDNotes_RP.Visible = value
            lblMaxRollover_RP.Visible = value
            lnkRMD_RP.Visible = value
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub SetVisibility_RMDControls_SavingPlan(ByVal value As Boolean)
        Try
            'lblRMD_SP.Visible = value
            txtRMDTaxRate_SP.Visible = value
            txtRMDTaxable_SP.Visible = value
            txtRMDNonTaxable_SP.Visible = value
            txtRMDNet_SP.Visible = value
            txtRMDTax_SP.Visible = value
            lblRMDNotes_SP.Visible = value
            lblMaxRollover_SP.Visible = value
            lnkRMD_SP.Visible = value
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub SetDefaultValue_RMDControls_SavingPlan()
        Try
            txtRMDTaxRate_SP.Text = 0.0
            txtRMDTaxable_SP.Text = 0.0
            txtRMDNonTaxable_SP.Text = 0.0
            txtRMDNet_SP.Text = 0.0
            txtRMDTax_SP.Text = 0.0
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub SetDefaultValue_RMDControls_RetirementPlan()
        Try
            txtRMDTaxRate_RP.Text = 0.0
            txtRMDTaxable_RP.Text = 0.0
            txtRMDNonTaxable_RP.Text = 0.0
            txtRMDNet_RP.Text = 0.0
            txtRMDTax_RP.Text = 0.0
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub PopulateBeneficiaryRMDs_Retirementplan(ByVal RetirementPlan_RefundCalculations As DataTable, ByVal strBenRMDTaxrate As String)
        Try
            txtRMDTaxRate_RP.Text = IIf(String.IsNullOrEmpty(strBenRMDTaxrate), 10, strBenRMDTaxrate)
            txtRMDTaxable_RP.Text = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0)("nRMDTaxable").ToString()).ToString("#0.00")
            txtRMDNonTaxable_RP.Text = Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0)("nRMDNonTaxable").ToString()).ToString("#0.00")
            txtRMDTax_RP.Text = Math.Round(Convert.ToDouble(Me.txtRMDTaxable_RP.Text) * Convert.ToDouble(Me.txtRMDTaxRate_RP.Text) / 100, 2)
            txtRMDNet_RP.Text = (Convert.ToDecimal(txtRMDTaxable_RP.Text) + Convert.ToDecimal(txtRMDNonTaxable_RP.Text) - Convert.ToDecimal(txtRMDTax_RP.Text)).ToString("#0.00")
            lblMaxRollover_RP.Text = "(max. allowable rollover: $" & (Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nTaxable")) + Convert.ToDecimal(RetirementPlan_RefundCalculations.Rows(0).Item("nNonTaxable"))).ToString("#0.00") & ")"
        Catch ex As Exception
            HelperFunctions.LogException("Beneficiary RMDs --> PopulateBeneficiaryRMDs_Retirementplan", ex)
            Throw
        End Try
    End Sub
    Public Sub PopulateBeneficiaryRMDs_Savingplan(ByVal SavingsPlan_RefundCalculations As DataTable, ByVal strBenRMDTaxrate As String)
        Try
            txtRMDTaxRate_SP.Text = IIf(String.IsNullOrEmpty(strBenRMDTaxrate), 10, strBenRMDTaxrate)
            txtRMDTaxable_SP.Text = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0)("nRMDTaxable").ToString()).ToString("#0.00")
            txtRMDNonTaxable_SP.Text = Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0)("nRMDNonTaxable").ToString()).ToString("#0.00")
            txtRMDTax_SP.Text = Math.Round(Convert.ToDouble(Me.txtRMDTaxable_SP.Text) * Convert.ToDouble(Me.txtRMDTaxRate_SP.Text) / 100, 2)
            txtRMDNet_SP.Text = (Convert.ToDecimal(txtRMDTaxable_SP.Text) + Convert.ToDecimal(txtRMDNonTaxable_SP.Text) - Convert.ToDecimal(txtRMDTax_SP.Text)).ToString("#0.00")
            lblMaxRollover_SP.Text = "(max. allowable rollover: $" & (Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nTaxable")) + Convert.ToDecimal(SavingsPlan_RefundCalculations.Rows(0).Item("nNonTaxable"))).ToString("#0.00") & ")"
        Catch ex As Exception
            HelperFunctions.LogException("Beneficiary RMDs --> PopulateBeneficiaryRMDs_Savingplan", ex)
            Throw
        End Try
    End Sub
    <System.Web.Services.WebMethod()> _
    Public Shared Function Binddata(ByVal Type As String) As String
        Dim dsRMDS As DataSet
        Dim lstRMDs As New List(Of Dictionary(Of String, String))
        Dim js As New Script.Serialization.JavaScriptSerializer()
        Dim strJson As String
        Try
            If Type = "Retirement Plan" Then
                dsRMDS = SessionBeneficiaryRMDs.BeneficiaryRMDs_RP
            ElseIf Type = "Saving Plan" Then
                dsRMDS = SessionBeneficiaryRMDs.BeneficiaryRMDs_SP
            End If

            For Each dr As DataRow In dsRMDS.Tables(1).Rows
                lstRMDs.Add(New Dictionary(Of String, String)() From { _
                         {"intYear", dr("intYear").ToString().Trim},
                         {"RMDTaxableAmount", dr("RMDTaxableAmount").ToString().Trim},
                         {"RMDNonTaxableAmount", dr("RMDNonTaxableAmount").ToString().Trim}
                        })
            Next

            strJson = js.Serialize(lstRMDs)
            Return strJson

        Catch ex As Exception
            HelperFunctions.LogException("Beneficiary RMDs --> Binddata", ex)
            Throw
        End Try
    End Function
    'End- SR:2014.05.06 - YRS 5.0-2188: RMDs for Beneficieries
    'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Method to bind grid from database
    Public Function LoadDeductions(ByVal parameterDataGridDeductions As DataGrid)
        Dim dtDeductions As DataTable

        Try
            If Not Session("DeductionsDataTable") Is Nothing Then
                dtDeductions = DirectCast(Session("DeductionsDataTable"), DataTable)
                If HelperFunctions.isNonEmpty(dtDeductions) Then
                    parameterDataGridDeductions.DataSource = dtDeductions
                    parameterDataGridDeductions.DataBind()
                End If
            End If

        Catch ex As Exception
            HelperFunctions.LogException("Refund Beneficiary --> LoadDeductions", ex)
        End Try
    End Function
    'End - Manthan | 2016.04.22 | YRS-AT-2206 | Method to bind grid from database

    'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Enabling textbox in grid on checkbox selection
    Private Sub dgDeductions_ItemDataBound(sender As Object, e As DataGridItemEventArgs) Handles dgDeductions.ItemDataBound
        Dim i As Integer
        Dim editcell As TableCell
        Dim txt As TextBox
        Dim chkbox As CheckBox
        Dim lblAmt As Label

        Try
            For i = 0 To e.Item.Cells.Count - 1
                If e.Item.Cells(i).Text = "Fund Costs" Then
                    editcell = e.Item.Cells(i + 1)
                    txt = e.Item.Cells(2).FindControl("txtFundCostAmt")
                    txt.Visible = True
                    chkbox = e.Item.Cells(0).FindControl("chkBoxDeduction")
                    chkbox.Attributes.Add("onclick", "EnableTextbox(" + e.Item.ItemIndex.ToString + ");")
                    lblAmt = e.Item.Cells(2).FindControl("lblAmount")
                    lblAmt.Visible = False
                End If
            Next

        Catch ex As Exception
            HelperFunctions.LogException("Refund Beneficiary --> dgDeductions_ItemDataBound", ex)
        End Try
    End Sub
    'End - Manthan | 2016.04.22 | YRS-AT-2206 | Enabling textbox in grid on checkbox selection

    'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Method to split deduction values stored in a string variable and adding it to datatable
    <WebMethod()> _
    Public Shared Function SaveDeductionValues(ByVal strDeductionval As String)
        Dim dtDedValLumpsum As New DataTable
        Dim dsDedValLumpsum As New DataSet
        Dim dtDeductions As DataTable
        Dim strDedVal As String
        Dim strSplitDedVal As String()
        Dim strSplit As String()
        Dim drDeductionVal As DataRow
        Dim drDeduction() As DataRow

        Try
            dtDedValLumpsum.TableName = "Deductions"
            dtDedValLumpsum.Columns.Add("ControlID", GetType(String))
            dtDedValLumpsum.Columns.Add("Description", GetType(String))
            dtDedValLumpsum.Columns.Add("Amount", GetType(Decimal))
            dtDedValLumpsum.Columns.Add("CodeValue", GetType(String))

            dtDeductions = DirectCast(HttpContext.Current.Session("DeductionsDataTable"), DataTable)

            strDedVal = strDeductionval
            strSplitDedVal = strDedVal.Split("##")

            For Each strlines As String In strSplitDedVal
                If Not String.IsNullOrEmpty(strlines) Then
                    strSplit = strlines.Split(":")
                    drDeductionVal = dtDedValLumpsum.NewRow()
                    drDeduction = dtDeductions.Select("ShortDescription ='" + strSplit(1) + "'")

                    drDeductionVal.SetField("ControlID", strSplit(0))
                    drDeductionVal.SetField("Description", strSplit(1))
                    If strSplit(1) = "Fund Costs" Then
                        drDeductionVal.SetField("Amount", strSplit(2))
                    Else
                        drDeductionVal.SetField("Amount", drDeduction(0)("Amount"))
                    End If
                    drDeductionVal.SetField("CodeValue", drDeduction(0)("CodeValue").ToString())
                    dtDedValLumpsum.Rows.Add(drDeductionVal)
                End If
            Next
            dsDedValLumpsum.Tables.Add(dtDedValLumpsum)
            HttpContext.Current.Session("FinalDeductionsLumpsum") = dsDedValLumpsum
            HttpContext.Current.Session("TotDeductionsLumpsumAmt") = dsDedValLumpsum.Tables(0).Compute("Sum(Amount)", "")

        Catch ex As Exception
            HelperFunctions.LogException("Refund Beneficiary --> SaveDeductionValues", ex)
        End Try
    End Function
    'End - Manthan | 2016.04.22 | YRS-AT-2206 | Method to split deduction values stored in a string variable and adding it to datatable

    'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Method to concate all the selected deductions values from grid
    <WebMethod()> _
    Public Shared Function GetSelectedDeductionVal() As String
        Dim strId As String
        Dim dsDeduction As DataSet
        Try
            If Not HttpContext.Current.Session("FinalDeductionsLumpsum") Is Nothing Then
                dsDeduction = DirectCast(HttpContext.Current.Session("FinalDeductionsLumpsum"), DataSet)
                If HelperFunctions.isNonEmpty(dsDeduction) Then
                    For Each dr As DataRow In dsDeduction.Tables(0).Rows
                        If dr("Description").ToString() = "Fund Costs" Then
                            strId += String.Concat(dr("ControlID").ToString(), "|", dr("Amount").ToString(), "#")
                        Else
                            strId += String.Concat(dr("ControlID").ToString(), "#")
                        End If
                    Next
                End If
            End If
            Return strId
        Catch ex As Exception
            HelperFunctions.LogException("Refund Beneficiary --> GetSelectedDeductionVal", ex)
        End Try
    End Function
    'End - Manthan | 2016.04.22 | YRS-AT-2206 | Method to concate all the selected deductions values from grid

    'START : SB | 2016.11.25 | YRS-AT-3022 |  Property to save values in session
    Public Property SP_Parameters_IsRolloverToOwnIRA_RP() As String     ' for rollover to own IRA option for Retirement plan
        Get
            If Not (Session("SP_Parameters_IsRolloverToOwnIRA_RP")) Is Nothing Then
                Return (CType(Session("SP_Parameters_IsRolloverToOwnIRA_RP"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("SP_Parameters_IsRolloverToOwnIRA_RP") = Value
        End Set
    End Property

    Public Property SP_Parameters_IsRolloverToOwnIRA_SP() As String     ' for rollover to own IRA option for Savings plan
        Get
            If Not (Session("SP_Parameters_IsRolloverToOwnIRA_SP")) Is Nothing Then
                Return (CType(Session("SP_Parameters_IsRolloverToOwnIRA_SP"), String))
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal Value As String)
            Session("SP_Parameters_IsRolloverToOwnIRA_SP") = Value
        End Set
    End Property
    'END : SB | 2016.11.25 | YRS-AT-3022 |  Property to save values in session

End Class
