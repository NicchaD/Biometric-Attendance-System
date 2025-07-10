
'****************************************************************************************************************************
' Project Name		:	YMCA-YRS
' FileName			:	RetirementEstimateBatchWebForm.aspx
' Author Name		:	Shashank Patel
' Employee ID		:	55381
' Email			    :	shashank.patel@3i-infotech.com
' Creation Date	    :	11/16/2010 
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:YRS 5.0-1365 : Need new batch processing option
'****************************************************************************************************************************
' Change History
'****************************************************************************************************************************
'Modification History
'****************************************************
' Date           Modified by          Description
'****************************************************
' 2012.01.27	Nikunj Patel		Made changes to handle string incorrect format error
' 2012.01.30	Shashank Patel		Made changes to enable save button & Calculation of estimated Retirement date
' 2012.01.31	Shashank Patel		Made changes to handle string incorrect format error
' 2012.02.02	Shashank Patel		Made changes to handle RDB amount & handling Reason Message
' 2012.02.09	Shashank Patel      Made changes to remove add button & parameter screen
' 2012.02.29    Shashank Patel      Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant
' 2012.03.05    Harshala Trimukhe   Bt Id:1007 Batch Estimate observation.
' 2012.03.15    Harshala Trimukhe   Bt Id:1009 Application does not show pagination after clicking on "Add To List" button.
' 2012.03.22    Harshala Trimukhe   Bt Id:1007 Batch Estimate observation.
' 2012.05.08	Shashank Patel		BT-1032:Additional comments added for Gemini ID YRS 5.0-1574
' 2012.07.13	Shashank Patel		BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page 
' 2012.07.23	Shashank Patel		BT-1007 Batch Estimate observation.
' 2012-09-22    Anudeep A           BT-1126/Additional change YRS 5.0-1246 : : Handling DLIN records
' 2012.11.15    Sanjay R.           YRS 5.0-1329 - J&S options available to non-spouse beneficiaries
' 2012.11.19    Sanjay R.           Made changes to handle multiple beneficiaries. If there are multiple beneficiaries defined then system calculate wrong annuties for spouse
' 2012.12.04    Sanjeev Gupta(SG)   BT-1432: YRS 5.0-1727: Retirement batch estimates not showing J annuity options correctly.
' 2012.12.11    Sanjeev Gupta(SG)   BT-1426: YRS 5.0-1726:Write "*Estate" to beneficiary name field in atsPraReport 
' 2013.01.10    Sanjeev Gupta(SG)   BT-1432 (Re-Opened): YRS 5.0-1727: Retirement batch estimates not showing J annuity options correctly.
' 2013.01.30    Dinesh Kanojia(DK)  BT-1262: YRS 5.0-1697: Message to display if multiple active employments Exist.
' 2013.11.12	Shashank Patel		BT-2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee (re-open) 
' 2014.10.10    Anudeep  Adusumilli BT:2357 YRS 5.0-2285 - Need ability to exclude pre-eligible participants 
' 2014.10.13    Anudeep  Adusumilli BT:2628 YRS 5.0-2405 -  Consistent screen header sections 
' 2015.09.16    Manthan Rajguru     YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
' 2015.11.09    Manthan Rajguru     YRS-AT-2431: Batch estimates issue with status RA.
' 2015.11.26    Gunanithi G         YRS-AT-2677: YRS enh: Annuity Estimate - change for Retired Death Benefit, use new effective date 1/1/2019 for calculations
' 2016.02.02    Bala                YRS-AT-2677: YRS enh: Annuity Estimate - change for Retired Death Benefit, use new effective date 1/1/2019 for calculations (for adding new assumptions text box for non-eligible persons)
' 2018.11.01    Benhan David        YRS-AT-4133 -  YRS enh: Annuity Estimate calculator & Goal Calculator: RDB Plan Rule change NEEDED by DECEMBER 2018 (TrackIT 32497)
' 2020.02.06    Megha Lad           YRS-AT-4783 - YRS enhancement-modify Batch Estimates for Secure Act (TrackIt - 41132)
' 2020.05.22    Megha Lad          	YRS-AT-4885 - Allow 0% Interest in annuity estimate calculator
'******************************************************************************************************************************

Imports System.Data.SqlClient
Imports YMCARET.YmcaBusinessObject

Public Class RetirementEstimateBatchWebForm
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents PageCaption As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents YMCA_Footer_WebUserControl1 As YMCAUI.YMCA_Footer_WebUserControl
    Protected WithEvents HeaderControl As YMCAUI.YMCA_Header_WebUserControl
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents tblShowNotes As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tabStripRetirementEstimate As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents MultiPageRetirementEstimate As Microsoft.Web.UI.WebControls.MultiPage

    'Protected WithEvents DataGridFindInfo As System.Web.UI.WebControls.DataGrid
    Protected WithEvents gvFindInfo As System.Web.UI.WebControls.GridView

    'Protected WithEvents DataGridParameter As System.Web.UI.WebControls.DataGrid
    Protected WithEvents gvParameter As System.Web.UI.WebControls.GridView

    Protected WithEvents LabelFundNo As System.Web.UI.WebControls.Label
    Protected WithEvents lblNotes As System.Web.UI.WebControls.Label
    Protected WithEvents TextboxFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAssociation As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAssociation As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    'Protected WithEvents LabelNoDataFound As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents dgPager As DataGridPager
    'Protected WithEvents DgPagerParam As DataGridPager
    Protected WithEvents TextBoxYmcaNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonImportAssociation As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    Protected WithEvents LabelAge As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListAge As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelAccount As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListPlanType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelRDB As System.Web.UI.WebControls.Label
    Protected WithEvents DropdownlistPercentageToUse As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelInterestRate As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownlistProjectedYearInterest As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelSalaryPercentage As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListSalaryPercentage As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelFutureSalaryEffDate As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxFutureSalaryEffDate As YMCAUI.DateUserControl
    Protected WithEvents CustomValidatorFutureSalaryDate As System.Web.UI.WebControls.CustomValidator
    Protected WithEvents ButtonSaveParamater As System.Web.UI.WebControls.Button

    'Protected WithEvents DataGridExceptions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents gvExceptions As System.Web.UI.WebControls.GridView

    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonExecute As System.Web.UI.WebControls.Button
    'Protected WithEvents ButtonRemoveList As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonClearList As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonAddList As System.Web.UI.WebControls.Button
    'Protected WithEvents ButtonShowList As System.Web.UI.WebControls.Button
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        InitializeComponent()
    End Sub

#End Region


#Region "Public Property"
    Private Property FindPersonPageIndex() As Int32
        Get
            If Session("RetirementBatchEstimateFindPersonPageIndex") = Nothing Then
                Return 0
            Else
                Return Convert.ToInt32(Session("RetirementBatchEstimateFindPersonPageIndex"))
            End If
        End Get
        Set(value As Int32)
            Session("RetirementBatchEstimateFindPersonPageIndex") = value
        End Set
    End Property

    Private Property FindPersonSortState() As GridViewCustomSort
        Get
            If Session("RetirementBatchEstimateFindPersonSortState") IsNot Nothing Then
                Return DirectCast(Session("RetirementBatchEstimateFindPersonSortState"), GridViewCustomSort)
            Else
                Return (New GridViewCustomSort)
            End If

        End Get
        Set(value As GridViewCustomSort)
            Session("RetirementBatchEstimateFindPersonSortState") = value
        End Set
    End Property

    Private Property ParameterPageIndex() As Int32
        Get
            If Session("RetirementBatchEstimateParameterPageIndex") = Nothing Then
                Return 0
            Else
                Return Convert.ToInt32(Session("RetirementBatchEstimateParameterPageIndex"))
            End If
        End Get
        Set(value As Int32)
            Session("RetirementBatchEstimateParameterPageIndex") = value
        End Set
    End Property

    Private Property ParameterSortState() As GridViewCustomSort
        Get
            If Session("RetirementBatchEstimateParameterSortState") IsNot Nothing Then
                Return DirectCast(Session("RetirementBatchEstimateParameterSortState"), GridViewCustomSort)
            Else
                Return (New GridViewCustomSort)
            End If

        End Get
        Set(value As GridViewCustomSort)
            Session("RetirementBatchEstimateParameterSortState") = value
        End Set
    End Property

    Private Property strBackgroundcolor() As Drawing.Color
        Get
            If ViewState("strBackgroundcolor") = Nothing Then
                Return Nothing
            Else
                Return ViewState("strBackgroundcolor")
            End If

        End Get
        Set(ByVal value As Drawing.Color)
            ViewState("strBackgroundcolor") = value
        End Set
    End Property


    Public Property ISLoadDBMode() As Boolean
        Get
            If ViewState("isLoadDbMode") Is Nothing Then
                ViewState("isLoadDbMode") = False
            End If

            Return Convert.ToBoolean(ViewState("isLoadDbMode"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("isLoadDbMode") = value
        End Set
    End Property
    Public Property ISSearchByYmcaNo() As Boolean
        Get
            If ViewState("iSSearchByYmcaNo") Is Nothing Then
                ViewState("iSSearchByYmcaNo") = False
            End If

            Return Convert.ToBoolean(ViewState("iSSearchByYmcaNo"))
        End Get
        Set(ByVal value As Boolean)
            ViewState("iSSearchByYmcaNo") = value
        End Set
    End Property
    Public Property ViewState_SortExpression() As String
        Get
            If ViewState("SortExpression") Is Nothing Then
                ViewState("SortExpression") = False
            End If

            Return Convert.ToString(ViewState("SortExpression"))
        End Get
        Set(ByVal value As String)
            ViewState("SortExpression") = value
        End Set
    End Property
    Public Property ViewState_ParamSortExpression() As String
        Get
            If ViewState("SortParamExpression") Is Nothing Then
                ViewState("SortParamExpression") = False
            End If

            Return Convert.ToString(ViewState("SortParamExpression"))
        End Get
        Set(ByVal value As String)
            ViewState("SortParamExpression") = value
        End Set
    End Property

    Private Property SessionDataSetdsMemberInfo() As DataSet
        Get
            If Not (Session("datasetdsMemberInfo")) Is Nothing Then

                Return (DirectCast(Session("datasetdsMemberInfo"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("datasetdsMemberInfo") = Value
        End Set
    End Property

    Private Property ViewStatedtException() As DataTable
        Get
            If Not (ViewState("dtException")) Is Nothing Then

                Return (DirectCast(ViewState("dtException"), DataTable))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataTable)
            ViewState("dtException") = Value
        End Set
    End Property
    'START : ML | 2020.02.06 | YRS-AT-4783 | Declare public property to store beneficary RelationShip Code
    Public Property BeneRelationShip() As String
        Get
            Return (DirectCast(ViewState("BeneRelationShip"), String))
        End Get
        Set(ByVal value As String)
            ViewState("BeneRelationShip") = value
        End Set
    End Property
    'END : ML | 2020.02.06 | YRS-AT-4783 | Declare public property to store beneficary RelationShip Code
    'SP -2012.07.23  : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
    'Private Property SessionDataSetSavedList() As DataSet
    '	Get
    '		If Not (Session("dsSavedList")) Is Nothing Then

    '			Return (DirectCast(Session("dsSavedList"), DataSet))
    '		Else
    '			Return Nothing
    '		End If
    '	End Get

    '	Set(ByVal Value As DataSet)
    '		Session("dsSavedList") = Value
    '	End Set
    'End Property
    'SP -2012.07.23 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
    Private Property SessionDataSetExistingSaved() As DataSet
        Get
            If Not (Session("dtExistingSaved")) Is Nothing Then

                Return (DirectCast(Session("dtExistingSaved"), DataSet))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As DataSet)
            Session("dtExistingSaved") = Value
        End Set
    End Property
    Private Property SessionUserID() As Int32
        Get
            If Not (Session("LoggedUserKey")) Is Nothing Then

                Return (DirectCast(Session("LoggedUserKey"), Int32))
            Else
                Return Nothing
            End If
        End Get

        Set(ByVal Value As Int32)
            Session("LoggedUserKey") = Value
        End Set
    End Property

    'Guna -2015-11-26 : YRS-AT-2677: fetching the configured restricted date  : START
    Public Property DeathBenefitAnnuityPurchaseRestrictedDate() As Date
        Get
            If ViewState("DeathBenefitAnnuityPurchaseRestrictedDate") Is Nothing Then
                ViewState("DeathBenefitAnnuityPurchaseRestrictedDate") = RetirementBOClass.DeathBenefitAnnuityPurchaseRestrictedDate()
            End If

            Return CType(ViewState("DeathBenefitAnnuityPurchaseRestrictedDate"), Date)

        End Get
        Set(ByVal Value As Date)
            ViewState("DeathBenefitAnnuityPurchaseRestrictedDate") = Value
        End Set
    End Property
    'Guna -2015-11-26 : YRS-AT-2677: fetching the Minimum age to retire  : START
    Public Property MinimumAgeToRetire() As Integer
        Get
            If ViewState("MinimumAgeToRetire") Is Nothing Then
                ViewState("MinimumAgeToRetire") = RetirementBOClass.GetMinimumAgeToRetire()
            End If

            Return CType(ViewState("MinimumAgeToRetire"), Integer)

        End Get
        Set(ByVal Value As Integer)
            ViewState("MinimumAgeToRetire") = Value
        End Set
    End Property
#End Region

#Region "Public Enum"
    Public Enum LoadDatasetMode
        Table
        Session
    End Enum

    Public Enum BeneficiaryType
        Manual = 0
        Spouse = 1
        NonSpouse = 2
    End Enum
#End Region

#Region "DatagridPersonalInfo Cell Index Constant"
    Private Const RET_SEL_PERS_CHK As Integer = 0

    'Private Const RET_PERSID As Integer = 1
    Private Const RET_PERSID As Integer = 2

    'Private Const RET_PERS_SSN As Integer = 2
    Private Const RET_PERS_SSN As Integer = 3

    'Private Const RET_FUND_IDNO As Integer = 2
    Private Const RET_FUND_IDNO As Integer = 4

    'Private Const RET_FIRST_NAME As Integer = 4
    Private Const RET_FIRST_NAME As Integer = 5

    'Private Const RET_LAST_NAME As Integer = 5
    Private Const RET_LAST_NAME As Integer = 6

    'Private Const RET_FUND_STATUS As Integer = 6
    Private Const RET_FUND_STATUS As Integer = 7

    'Private Const RET_AGE As Integer = 7
    Private Const RET_AGE As Integer = 8

    'Private Const RET_EMP_FUND_EVENT_ID As Integer = 8
    Private Const RET_EMP_FUND_EVENT_ID As Integer = 0

    Private Const RET_EMP_BIRTH_DATE As Integer = 9

    Private Const RET_PLAN_RETIREMENT As Integer = 10

    Private Const RET_PLAN_SAVING As Integer = 11

    'Private Const RET_ADD_IMAGE As Integer = 12
    Private Const RET_COUNTS As Integer = 12

#End Region
#Region "gvParameter Cell Index Constant"
    'Private Const PARAM_SEL_PERS_CHK As Integer = 0

    'Private Const PARAM_PERSID As Integer = 0
    Private Const PARAM_PERSID As Integer = 2

    'Private Const PARAM_PERS_SSN As Integer = 1
    Private Const PARAM_PERS_SSN As Integer = 3

    'Private Const PARAM_FUND_IDNO As Integer = 2
    Private Const PARAM_FUND_IDNO As Integer = 4

    'Private Const PARAM_FIRST_NAME As Integer = 3
    Private Const PARAM_FIRST_NAME As Integer = 5

    'Private Const PARAM_LAST_NAME As Integer = 4
    Private Const PARAM_LAST_NAME As Integer = 6

    'Private Const PARAM_FUND_STATUS As Integer = 5
    Private Const PARAM_FUND_STATUS As Integer = 7

    'Private Const PARAM_AGE As Integer = 6
    Private Const PARAM_AGE As Integer = 8

    'Private Const PARAM_EMP_FUND_EVENT_ID As Integer = 7
    Private Const PARAM_EMP_FUND_EVENT_ID As Integer = 0

    'Private Const PARAM_EMP_BIRTH_DATE As Integer = 8
    Private Const PARAM_EMP_BIRTH_DATE As Integer = 9

    'Private Const PARAM_PLAN_RETIREMENT As Integer = 9
    Private Const PARAM_PLAN_RETIREMENT As Integer = 10

    'Private Const PARAM_PLAN_SAVING As Integer = 10
    Private Const PARAM_PLAN_SAVING As Integer = 11

    'Private Const PARAM_ADD_IMAGE As Integer = 11
#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            TextBoxSSNo.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
            TextBoxAssociation.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
            TextBoxFirstName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
            TextboxFundNo.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
            TextBoxLastName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonFind.UniqueID + "').click();return false;}} else {return true}; ")
            TextBoxYmcaNo.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + ButtonImportAssociation.UniqueID + "').click();return false;}} else {return true}; ")


            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If


            If strBackgroundcolor = Nothing Then
                strBackgroundcolor = Drawing.Color.FromName(System.Configuration.ConfigurationSettings.AppSettings("RetirementBatchestimate_Color"))
            End If

            'dgPager.Visible = False

            '2012.02.29 SP: Made changes to remove message box confirmation
            'If Session("DeleteRecords") = True AndAlso Request.Form("Yes") = "Yes" Then
            '    Session("DeleteRecords") = False
            '    'Me.EmptyBatchEstimateParameterTable(SessionUserID) 'Commented bu SP:2012.02.27 by adding remove all features
            '    Me.RemoveAll()
            '    Dim alertMessageJS As String = "<script language='javascript'>$(document).ready(function() { alertMessage('" & Resources.RetirementBatchEstimateMessages.MESSAGE_RETIREMENT_BATCH_ESTIMATE_REMOVE_LIST_SUCCESS & "'); })</script>"
            '    Page.RegisterClientScriptBlock("MEssageBox", alertMessageJS)
            '    ' 2012.01.30	Shashank Patel		Made changes to enable save button & Calculation of estimated Retirement date
            '    ' If (HelperFunctions.isNonEmpty(SessionDataSetExistingSaved)) Then
            '    'Me.PopulateShowListData(SessionDataSetExistingSaved)
            '    ButtonRemoveList.Enabled = HelperFunctions.isNonEmpty(SessionDataSetExistingSaved)
            '    Me.PopulateParameterGridData()
            '    'Else
            '    '    Me.PopulateBatchEstimateData(SessionUserID, LoadDatasetMode.Table)
            '    'End If
            '    'Me.ButtonSave.Enabled = True
            '    ' 2012.01.30	Shashank Patel		Made changes to enable save button & Calculation of estimated Retirement date
            'End If
            '2012.02.29 SP: Made changes to remove message box confirmation -End

            If Not (Page.IsPostBack) Then
                SessionUserID = Session("LoggedUserKey")

                'Me.Menu1.DataSource = Server.MapPath("SimpleXML.xml")
                'Menu1.DataBind()

                Me.ClearSession()
                Me.LoadDefaultValues()

                'Me.ISLoadDBMode = False
                'Me.PopulateBatchEstimateData(SessionUserID)
                'Me.PopulateRetirementBatchEstimateParamater(SessionUserID)

                Me.ButtonSave.Visible = False
                Me.ButtonAddList.Enabled = False
                Me.ButtonShowList.Visible = False
                Me.ButtonClearList.Visible = False
                gvFindInfo.DataSource = Nothing
                Session("GUID") = Nothing

                'SP-2012.07.23 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START -Commented following statement
                'HARSHALA-2012.03.05 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
                'Dim dsSavedList As DataSet
                'dsSavedList = RetirementBatchEstimateBOClass.SearchBatchEstimatePerson(SessionUserID)
                'If (HelperFunctions.isNonEmpty(dsSavedList)) Then
                '	HiddenFieldSavedListCount.Value = Convert.ToString(dsSavedList.Tables(0).Rows.Count)
                'Else
                '	HiddenFieldSavedListCount.Value = "0"
                'End If
                'HARSHALA-2012.03.05 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : END
                HiddenFieldSavedListCount.Value = Convert.ToString(RetirementBatchEstimateBOClass.GetRetirementBatchListCount(SessionUserID))
                'SP-2012.07.23: BT ID-1007 BATCH ESTIMATE OBSERVATIONS : END
            End If
            'HARSHALA-2012.03.15 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : START
            'DataGridFindInfo.PageSize = 15
            'HARSHALA-2012.03.15 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : END
            'dgPager.Grid = DataGridFindInfo
            'dgPager.PagesToDisplay = 10
            'Start: Bala: 2/2/2016: YRS-AT-2677: Adding the label text for non eligible person assumption text box.
            LabelSubPraAssumptionForNonEligible.Text = String.Format("(for participants who will not attain age of {0}yrs as of {1})", Me.MinimumAgeToRetire, Me.DeathBenefitAnnuityPurchaseRestrictedDate.ToString("MM/dd/yyyy"))
            LabelSubPraAssumptionForEligible.Text = String.Format("(for participants who will attain age of {0}yrs as of {1})", Me.MinimumAgeToRetire, Me.DeathBenefitAnnuityPurchaseRestrictedDate.ToString("MM/dd/yyyy"))
            'End: Bala: 2/2/2016: YRS-AT-2677: Adding the label text for non eligible person assumption text box.
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_Page_Load", ex)
        End Try
    End Sub
#End Region

    Sub ValidateFutureSalaryDate(ByVal source As Object, ByVal args As ServerValidateEventArgs)
        Dim dateIsValid As Boolean = True
        'Dim errorMessage As String = "Invalid Parameters Details"//commented by Anudeep on 22-sep for BT-1126
        Dim errorMessage As String = getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_INVALID_PARAMETERS")
        'Validate Future Salary date
        Try
            If DropDownListSalaryPercentage.SelectedValue > 0 AndAlso TextBoxFutureSalaryEffDate.Text <> String.Empty Then
                Dim futureSalaryDate As DateTime = Convert.ToDateTime(TextBoxFutureSalaryEffDate.Text)
                If futureSalaryDate <= DateTime.Today Then
                    dateIsValid = False
                    'Added by Anudeep on 22-sep for BT-1126
                    errorMessage = errorMessage + "<br>" & getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_FUTURE_SAL_DATE")

                End If
            End If
            If DropDownListSalaryPercentage.SelectedValue > 0 AndAlso TextBoxFutureSalaryEffDate.Text = String.Empty Then
                dateIsValid = False
                'Added by Anudeep on 22-sep for BT-1126
                errorMessage = errorMessage + "<br>" & getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_FUTURE_SAL_EMPTY")
            End If
            If Not dateIsValid Then
                CustomValidatorFutureSalaryDate.ErrorMessage = errorMessage
                tabStripRetirementEstimate.SelectedIndex = 1
                Me.MultiPageRetirementEstimate.SelectedIndex = Me.tabStripRetirementEstimate.SelectedIndex
            End If
        Catch ex As Exception
            dateIsValid = False
            errorMessage = errorMessage + ex.Message
        End Try
        args.IsValid = dateIsValid
    End Sub

    Sub ValidateAssumption(ByVal source As Object, ByVal args As ServerValidateEventArgs)
        Dim TextIsValid As Boolean = True
        'Dim errorMessage As String = "Invalid Parameters Details"//commented by Anudeep on 22-sep for BT-1126
        Dim errorMessage As String = getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_INVALID_PARAMETERS")
        Try
            'If TextBoxPraAssumption.Text.Trim = String.Empty Then
            '    TextIsValid = False
            '    errorMessage = errorMessage + "<br>" & getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_ASSUMPTION_EMPTY
            'End If

            If TextBoxPraAssumption.Text.Trim().Length > 800 Then
                TextIsValid = False
                errorMessage = errorMessage + "<br>" & String.Format(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_ASSUMPTION_LENGTH"), TextBoxPraAssumption.MaxLength)
            End If
            If Not TextIsValid Then
                CustomValidatorAssumption.ErrorMessage = errorMessage
                tabStripRetirementEstimate.SelectedIndex = 1
                Me.MultiPageRetirementEstimate.SelectedIndex = Me.tabStripRetirementEstimate.SelectedIndex
            End If
        Catch ex As Exception
            TextIsValid = False
            errorMessage = errorMessage + ex.Message
        End Try
        args.IsValid = TextIsValid
    End Sub

#Region "Private Methods"

    Private Sub ClearSession()
        Try
            Session("GUID") = Nothing
            SessionDataSetdsMemberInfo = Nothing
            SessionDataSetExistingSaved = Nothing
            Session("strReportName") = Nothing
            ParameterSortState = Nothing
            ParameterPageIndex = Nothing
            FindPersonSortState = Nothing
            FindPersonPageIndex = Nothing
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "ClearSession", ex)
        End Try
    End Sub

    Private Sub LoadDefaultValues()

        Dim dtMetaNormalProjInterestRate As DataTable
        Dim lnMetaNormalProjInterestRate As Double = 0
        dtMetaNormalProjInterestRate = RefundRequest.SearchConfigurationMaintenance("EST_NORMAL_PROJECTED_INTEREST_RATE")
        Dim lstkeys As New List(Of String)

        If (HelperFunctions.isNonEmpty(dtMetaNormalProjInterestRate)) Then

            Dim dataRowFound As DataRow()
            Dim keys = From dtNormalInterest In dtMetaNormalProjInterestRate.AsEnumerable()
            Where dtNormalInterest.Field(Of String)("Key").Equals("EST_NORMAL_PROJECTED_INTEREST_RATE", StringComparison.CurrentCultureIgnoreCase)
            Select dtNormalInterest.Field(Of String)("Value")
            lstkeys = keys.ToList()
        End If

        Dim i As Integer
        Try
            'Binding Retirement Age
            For i = 55 To 70
                DropDownListAge.Items.Add(i)
            Next

            'Binding Retirement Death benifit
            DropdownlistPercentageToUse.Items.Add(0)
            For i = 90 To 1 Step -1
                DropdownlistPercentageToUse.Items.Add(i)
            Next


            'Binding Year Interest Rate
            'START : ML | 2020.05.22 | YRS-AT-4885 |Interest Rate changes
            'For i = 1 To 12
            '    DropDownlistProjectedYearInterest.Items.Add(i)
            'Next

            For i = 0 To 12
                DropDownlistProjectedYearInterest.Items.Add(i)
            Next
            'END : ML | 2020.05.22 | YRS-AT-4885 | Interest Rate changes

            If lstkeys.Count > 0 Then
                DropDownlistProjectedYearInterest.SelectedValue = lstkeys(0)
            End If

            'Binding salary increase percentages
            For i = 0 To 20
                DropDownListSalaryPercentage.Items.Add(i)
            Next
            DropDownListSalaryPercentage.SelectedValue = 0
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "LoadDefaultValues", ex)
        End Try
    End Sub

    Private Sub AddToList(ByVal lstFundEventId As List(Of String))
        Dim batchEstimateXml As String
        'Dim userID As Integer
        Try
            'userID = Convert.ToInt32(Session("LoggedUserKey"))
            batchEstimateXml = CreateFundEventIdXml(lstFundEventId)

            RetirementBatchEstimateBOClass.SaveBatchEstimate(batchEstimateXml, SessionUserID)

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "AddToList", ex)
        End Try
    End Sub

    Private Function GetDateByMonth(ByVal month As String) As String
        Dim dtFutureEffDate As DateTime
        Try
            dtFutureEffDate = Convert.ToDateTime(month.Trim() & "/" & Date.Today.Day.ToString() & "/" & Date.Today.Year.ToString())
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "GetDateByMonth", ex)
        End Try
        Return dtFutureEffDate.ToString()
    End Function
    'Private Sub AddBatchEstimateParameter()
    '    Try
    '        Dim dtmfutureSalEffDate As String = String.Empty
    '        If (DropDownListFutureSalaryEffMon.SelectedValue > 0) Then
    '            dtmfutureSalEffDate = GetDateByMonth(DropDownListFutureSalaryEffMon.SelectedValue).ToString()
    '        End If
    '        RetirementBatchEstimateBOClass.SaveBatchEstimateParamater(DropDownListAge.SelectedValue.ToString(), DropdownlistPercentageToUse.SelectedValue.ToString(), DropDownListSalaryPercentage.SelectedValue.ToString(), DropDownListPlanType.SelectedValue.ToString(), DropDownlistProjectedYearInterest.SelectedValue.ToString(), dtmfutureSalEffDate, SessionUserID)
    '    Catch ex As Exception
    '        HelperFunctions.LogException("AddBatchEstimateParameter", ex)
    '        Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Sub
    'Private Sub AddBatchEstimateParameter()
    '    Try
    '        RetirementBatchEstimateBOClass.SaveBatchEstimateParamater(DropDownListAge.SelectedValue.ToString(), DropdownlistPercentageToUse.SelectedValue.ToString(), DropDownListSalaryPercentage.SelectedValue.ToString(), DropDownListPlanType.SelectedValue.ToString(), DropDownlistProjectedYearInterest.SelectedValue.ToString(), TextBoxFutureSalaryEffDate.Text, SessionUserID)
    '    Catch ex As Exception
    '        HelperFunctions.LogException("AddBatchEstimateParameter", ex)
    '        Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Sub
    'Private Sub EmptyBatchEstimateParameterTable(ByVal tduserID As Integer)
    '    Try
    '        Dim lstFundEventId As List(Of String) = GetSelectedDatagridPersonInfoPerseID()
    '        Dim xmlGuiFundEventId As String = CreateFundEventIdXml(lstFundEventId)
    '        RetirementBatchEstimateBOClass.DeleteBatchEstimateByUserID(tduserID) 'Deleting from database
    '        Me.DeleteBatchEstimateTable(lstFundEventId:=lstFundEventId) 'Deleting from temporary session
    '    Catch ex As Exception
    '        HelperFunctions.LogException("EmptyBatchEstimateParameterTable", ex)
    '        Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Sub

    Private Sub DeleteBatchEstimateTable(ByVal lstFundEventId As List(Of String))
        Try
            If (HelperFunctions.isNonEmpty(SessionDataSetExistingSaved)) Then
                Dim i As Integer = 0
                Dim dscopySessionDataSetExistingSaved As DataSet
                Dim dritem As DataRow
                dscopySessionDataSetExistingSaved = SessionDataSetExistingSaved.Copy()
                For Each item In lstFundEventId
                    For i = 0 To dscopySessionDataSetExistingSaved.Tables(0).Rows.Count - 1
                        dritem = dscopySessionDataSetExistingSaved.Tables(0).Rows(i)
                        If (dritem("FundEventId").ToString().Equals(item.ToString(), StringComparison.InvariantCultureIgnoreCase)) Then
                            SessionDataSetExistingSaved.Tables(0).Rows(i).Delete()
                        End If
                    Next
                Next
                SessionDataSetExistingSaved.AcceptChanges()
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "DeleteBatchEstimateTable", ex)
        End Try
    End Sub
    'Private Function CreateXml(ByVal tdUserID As Integer, ByVal lstFundEventId As List(Of String)) As String
    '    Dim sbBatchEstimateDetailXml As New StringBuilder
    '    sbBatchEstimateDetailXml.Append("<ROOT>")
    '    For Each fundeventid In lstFundEventId

    '        sbBatchEstimateDetailXml.Append("<BatchEstimate>")
    '        'sbBatchEstimateDetailXml.Append("<PersID>")
    '        'sbBatchEstimateDetailXml.Append(fundeventid.Value)
    '        'sbBatchEstimateDetailXml.Append("</PersID>")
    '        sbBatchEstimateDetailXml.Append("<FundEventID>")
    '        sbBatchEstimateDetailXml.Append(fundeventid)
    '        sbBatchEstimateDetailXml.Append("</FundEventID>")
    '        sbBatchEstimateDetailXml.Append("<UserID>")
    '        sbBatchEstimateDetailXml.Append(tdUserID)
    '        sbBatchEstimateDetailXml.Append("</UserID>")
    '        sbBatchEstimateDetailXml.Append("</BatchEstimate>")
    '    Next
    '    sbBatchEstimateDetailXml.Append("</ROOT>")
    '    Return sbBatchEstimateDetailXml.ToString()

    'End Function
    Private Function CreateFundEventIdXml(ByVal lstFundEventId As List(Of String)) As String
        Try
            Dim sbBatchEstimateDetailXml As New StringBuilder
            sbBatchEstimateDetailXml.Append("<ROOT>")
            For Each fundEventId In lstFundEventId

                sbBatchEstimateDetailXml.Append("<BatchEstimate ")

                sbBatchEstimateDetailXml.Append(" FundEventID='")
                sbBatchEstimateDetailXml.Append(fundEventId)
                'sbBatchEstimateDetailXml.Append("</FundEventID>")
                sbBatchEstimateDetailXml.Append("'></BatchEstimate>")
            Next
            sbBatchEstimateDetailXml.Append("</ROOT>")
            Return sbBatchEstimateDetailXml.ToString()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "DeleteBatchEstimateTable", ex)
        End Try
    End Function

    'Private Function PopulateRetirementBatchEstimateParamater(ByVal tdUserID As Integer)
    '    Dim dsEstimateParameterInfo As DataSet
    '    Dim dtmFuturesalEffDate As String = String.Empty
    '    Try
    '        dsEstimateParameterInfo = RetirementBatchEstimateBOClass.SearchBatchEstimateParameter(tdUserID)
    '        If Not dsEstimateParameterInfo Is Nothing AndAlso dsEstimateParameterInfo.Tables.Count > 0 Then
    '            If dsEstimateParameterInfo.Tables(0).Rows.Count > 0 Then
    '                DropDownListAge.SelectedValue = dsEstimateParameterInfo.Tables(0).Rows(0)("Age")
    '                DropdownlistPercentageToUse.SelectedValue = dsEstimateParameterInfo.Tables(0).Rows(0)("DeathBenf")
    '                DropDownListSalaryPercentage.SelectedValue = dsEstimateParameterInfo.Tables(0).Rows(0)("Sal")
    '                DropDownlistProjectedYearInterest.SelectedValue = dsEstimateParameterInfo.Tables(0).Rows(0)("IntRates")
    '                DropDownListPlanType.SelectedValue = dsEstimateParameterInfo.Tables(0).Rows(0)("PlanType")
    '                dtmFuturesalEffDate = dsEstimateParameterInfo.Tables(0).Rows(0)("SalEffective").ToString()
    '                If (dtmFuturesalEffDate <> String.Empty) Then
    '                    'DropDownListFutureSalaryEffMon.SelectedValue = Convert.ToDateTime(dtmFuturesalEffDate).Month.ToString()
    '                    TextBoxFutureSalaryEffDate.Text = Convert.ToDateTime(dtmFuturesalEffDate).ToString("MM/dd/yyyy")
    '                End If

    '                'DropDownListPlanType.SelectedIndex = DropDownListPlanType.Items.IndexOf(DropDownListPlanType.Items.FindByValue(dsEstimateParameterInfo.Tables(0).Rows(0)("PlanType").ToString()))
    '            End If
    '        End If
    '    Catch ex As Exception
    '        HelperFunctions.LogException("PopulateRetirementBatchEstimateParamater", ex)
    '        Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Function

    Private Function PopulateFindData(ByVal isImportYmcano As Boolean, ByVal parameterload As LoadDatasetMode)
        Dim pagingOn As Boolean
        Dim dsMemberInfo As DataSet
        Dim strSelectedFundeventsToExlude As String = String.Empty
        Try

            If parameterload = LoadDatasetMode.Table Then
                If isImportYmcano Then
                    'Start: AA:10.10.2014 BT:2357 YRS 5.0-2285 - Added the code to exclude the fund events pe , ra.
                    For Each li As ListItem In chkList.Items
                        If li.Selected = True Then
                            strSelectedFundeventsToExlude += "'" + li.Value + "',"
                        End If
                    Next
                    If strSelectedFundeventsToExlude.Length > 0 Then
                        strSelectedFundeventsToExlude = strSelectedFundeventsToExlude.Substring(0, strSelectedFundeventsToExlude.Length - 1)
                    End If
                    If (TextBoxYmcaNo.Text.Trim = "") Then
                        HelperFunctions.ShowMessageToUser("This search will take a longer time. It is suggested that you enter a search criteria.", EnumMessageTypes.Error)
                    Else
                        dsMemberInfo = RetirementBatchEstimateBOClass.SearchPersonInfo(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, Me.TextBoxYmcaNo.Text.Trim().PadLeft(6, "0"), strSelectedFundeventsToExlude)
                    End If
                    'End: AA:10.10.2014 BT:2357 YRS 5.0-2285 - Added the code to exclude the fund events pe , ra.

                Else
                    If (TextBoxFirstName.Text.Trim = "" And TextBoxLastName.Text.Trim = "" And TextboxFundNo.Text.Trim = "" And TextBoxSSNo.Text.Trim = "" And TextBoxAssociation.Text.Trim = "") Then
                        HelperFunctions.ShowMessageToUser("This search will take a longer time. It is suggested that you enter a search criteria.", EnumMessageTypes.Error)
                    Else
                        dsMemberInfo = RetirementBatchEstimateBOClass.SearchPersonInfo(Me.TextboxFundNo.Text.Trim(), Me.TextBoxFirstName.Text.Trim(), Me.TextBoxLastName.Text.Trim(), Me.TextBoxSSNo.Text.Trim(), Me.TextBoxAssociation.Text.Trim())
                    End If
                End If
                SessionDataSetdsMemberInfo = dsMemberInfo
            Else
                dsMemberInfo = SessionDataSetdsMemberInfo
            End If
            If Not dsMemberInfo Is Nothing AndAlso dsMemberInfo.Tables.Count > 0 Then
                gvFindInfo.PageIndex = Me.FindPersonPageIndex
                Me.gvFindInfo.SelectedIndex = -1
            End If
            HelperFunctions.BindGrid(gvFindInfo, dsMemberInfo, True)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "PopulateFindData", ex)
        End Try

    End Function
    Private Sub ClearSavedList()
        Try
            '2012.02.29   Shashank Patel  :     Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant (remove parameter xmlfundeeventsids)
            RetirementBatchEstimateBOClass.DeleteBatchEstimateByUserID(SessionUserID) 'Deleting from database
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "PopulateFindData", ex)
        End Try
    End Sub


    Private Function PopulateBatchEstimateData(ByVal tdUserID As Integer, ByVal parameterload As LoadDatasetMode)
        'Dim pagingOn As Boolean
        Dim dsMemberInfo As DataSet
        Try

            If parameterload = LoadDatasetMode.Table Then
                dsMemberInfo = RetirementBatchEstimateBOClass.SearchBatchEstimatePerson(tdUserID)
                SessionDataSetExistingSaved = dsMemberInfo
            Else
                dsMemberInfo = SessionDataSetExistingSaved
            End If

            If Not dsMemberInfo Is Nothing AndAlso dsMemberInfo.Tables.Count > 0 Then

                'pagingOn = dsMemberInfo.Tables(0).Rows.Count > 500

                'If Me.DataGridParameter.CurrentPageIndex >= Me.DataGridParameter.PageCount And Me.DataGridParameter.PageCount <> 0 Then Exit Function
                'If Me.gvParameter.PageIndex >= Me.gvParameter.PageCount And Me.gvParameter.PageCount <> 0 Then Exit Function

                If (dsMemberInfo.Tables(0).Rows.Count > 0) Then

                    'LabelNoDataFound.Visible = False
                    Me.gvParameter.Visible = True
                    'If pagingOn Then
                    '    DataGridParameter.AllowPaging = True
                    'Else

                    '    DataGridParameter.AllowPaging = False
                    'End If
                    Me.ButtonExecute.Enabled = True


                Else

                    Me.ButtonExecute.Enabled = False

                End If


                Dim dv As New DataView
                Dim Sorting As GridViewCustomSort

                dv = dsMemberInfo.Tables(0).DefaultView()

                If ParameterSortState IsNot Nothing Then
                    Sorting = ParameterSortState
                    If Sorting.SortExpression IsNot Nothing And Sorting.SortDirection IsNot Nothing Then
                        dv.Sort = Sorting.SortExpression + " " + Sorting.SortDirection
                    End If
                End If

                gvParameter.PageIndex = Me.ParameterPageIndex

                Me.gvParameter.SelectedIndex = -1
                'Me.gvParameter.DataSource = dsMemberInfo
                'Me.gvParameter.DataBind()


                HelperFunctions.BindGrid(gvParameter, dsMemberInfo, True)

                'HARSHALA-2012.03.22 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
                Me.ButtonRemoveList.Enabled = True
                'HARSHALA-2012.03.22 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : END
            Else

                Me.gvParameter.Visible = False
                Me.ButtonExecute.Enabled = False
            End If

        Catch ex As Exception
            Me.gvParameter.Visible = False
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "PopulateBatchEstimateData", ex)
        End Try

    End Function
    '2012.02.09	Shashank Patel      Made changes to remove add button & parameter screen
    Public Sub PopulateParameterGridData()
        Try
            'Dim pagingOn As Boolean
            If HelperFunctions.isNonEmpty(SessionDataSetExistingSaved) Then

                'pagingOn = SessionDataSetExistingSaved.Tables(0).Rows.Count > 500

                'If Me.DataGridParameter.CurrentPageIndex >= Me.DataGridParameter.PageCount And Me.DataGridParameter.PageCount <> 0 Then Exit Sub
                'If Me.gvParameter.PageIndex >= Me.gvParameter.PageCount And Me.gvParameter.PageCount <> 0 Then Exit Sub

                If (SessionDataSetExistingSaved.Tables(0).Rows.Count > 0) Then


                    Me.gvParameter.Visible = True

                    'If pagingOn Then
                    '    'DgPagerParam.Visible = True
                    '    DataGridParameter.AllowPaging = True
                    'Else
                    '    'DgPagerParam.Visible = False
                    '    DataGridParameter.AllowPaging = False
                    'End If

                    'If pagingOn Then
                    '    DataGridParameter.AllowPaging = False
                    '    DataGridParameter.AllowPaging = True
                    '    DataGridParameter.CurrentPageIndex = 0
                    '    DataGridParameter.PageSize = 15

                    'End If
                Else
                    Me.ButtonExecute.Enabled = False
                End If

                Dim dv As New DataView
                Dim Sorting As GridViewCustomSort

                dv = SessionDataSetExistingSaved.Tables(0).DefaultView()

                If ParameterSortState IsNot Nothing Then
                    Sorting = ParameterSortState
                    If Sorting.SortExpression IsNot Nothing And Sorting.SortDirection IsNot Nothing Then
                        dv.Sort = Sorting.SortExpression + " " + Sorting.SortDirection
                    End If
                End If

                gvParameter.PageIndex = Me.ParameterPageIndex

                Me.gvParameter.SelectedIndex = -1

                HelperFunctions.BindGrid(gvParameter, dv, True)
                'Me.gvParameter.DataSource = Nothing
                'Me.gvParameter.DataSource = SessionDataSetExistingSaved
                'Me.gvParameter.DataBind()
            Else

                'Me.gvParameter.Visible = False
                Me.ButtonExecute.Enabled = False
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "PopulateParameterGridData", ex)
        End Try
    End Sub
    '2012.02.09	Shashank Patel      Made changes to remove add button & parameter screen
    Private Function GetMergeDataset(ByVal dsExisting As DataSet, ByVal dsAddtional As DataSet) As DataSet
        Dim dsNewdataset As DataSet = dsExisting.Copy()
        Try

            dsNewdataset.Tables(0).Merge(dsAddtional.Tables(0))
        Catch ex As Exception
            dsNewdataset = Nothing
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "GetMergeDataset", ex)
        End Try
        Return dsNewdataset
    End Function
    Private Function GetSelectedDatagridPersonInfoPerseID() As List(Of String)
        Dim lstPerson As New List(Of String)
        Dim gvRow As GridViewRow

        For Each gvRow In gvFindInfo.Rows
            Try
                Dim checkbox As CheckBox = CType(gvRow.FindControl("chkSelect"), CheckBox)
                If checkbox.Checked Then

                    'lstPerson.Add(gvRow.Cells(0).Text.Trim())
                    lstPerson.Add(gvRow.Cells(RET_EMP_FUND_EVENT_ID).Text.Trim())

                End If

            Catch ex As Exception
                HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
                HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "GetSelectedDatagridPersonInfoPerseID", ex)
            End Try
        Next

        Return lstPerson
    End Function
    Private Function GetSelectedDatagridParameterFundEventid() As List(Of String)
        Dim lstFunEventIds As New List(Of String)
        Dim gvRow As GridViewRow

        'For Each item As DataGridItem In DataGridParameter.Items
        For Each gvRow In gvParameter.Rows
            Try
                Dim checkbox As CheckBox = CType(gvRow.FindControl("chkParamSelect"), CheckBox)
                If checkbox.Checked Then
                    'lstFunEventIds.Add(gvRow.Cells(0).Text.Trim())
                    lstFunEventIds.Add(gvRow.Cells(PARAM_EMP_FUND_EVENT_ID).Text.Trim())
                End If

            Catch ex As Exception
                HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
                HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "GetSelectedDatagridParameterFundEventid", ex)
            End Try
        Next
        Return lstFunEventIds
    End Function
    Private Function GetSelectedFundEventsIDs() As List(Of String)
        Dim lstPerson As New List(Of String)
        Try
            If HelperFunctions.isNonEmpty(SessionDataSetExistingSaved) Then
                Dim fundEventsIds = From sessiondataset In SessionDataSetExistingSaved.Tables(0).AsEnumerable()
                  Select sessiondataset.Field(Of String)("FundEventId")
                lstPerson = fundEventsIds.ToList()
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "GetSelectedFundEventsIDs", ex)
        End Try

        Return lstPerson
    End Function


    Private Function GetFirstDayOfNextMonthIfNotFirstOfMonth(ByVal endDate As DateTime) As DateTime
        Try
            If (endDate.Day = 1) Then
                Return endDate
            End If

            If (endDate.Month = 12) Then
                Return New DateTime(endDate.Year + 1, 1, 1)
            End If

            Return New DateTime(endDate.Year, endDate.Month + 1, 1)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "GetFirstDayOfNextMonthIfNotFirstOfMonth", ex)
        End Try
    End Function
    'Added by Gunanithi - YRS-AT-2677- 08 Dec,2015, For fetching the calculated value of a person's age is above 55 as of restricted date.---------start
    Public Function IsDeathBenefitAnnuityPurchaseRestricted(parmPersDOB As Date) As Boolean
        Return RetirementBOClass.IsDeathBenefitAnnuityPurchaseRestricted(Me.MinimumAgeToRetire, parmPersDOB, Me.DeathBenefitAnnuityPurchaseRestrictedDate)
    End Function

#End Region

#Region "Exceute button Private Methods"

    Private Sub FilterAgeRecord(ByRef dsPersonalInfo As DataSet, ByRef dtException As DataTable, ByVal tdRetirementAge As String)
        If Not (dsPersonalInfo Is Nothing AndAlso dsPersonalInfo.Tables.Count = 0) Then

            Dim drOverAgePerson() As DataRow
            Try
                drOverAgePerson = dsPersonalInfo.Tables(0).Select("Age > " + tdRetirementAge)
                For Each drpersoninfo As DataRow In drOverAgePerson
                    dtException.Rows.Add(drpersoninfo.Item("SSN").ToString(), drpersoninfo.Item("FundIDNo").ToString(), drpersoninfo("FirstName"), drpersoninfo("LastName"), getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_PERSON_CROSS_AGE"))
                    drpersoninfo.Delete()
                Next
                dtException.AcceptChanges()
                dsPersonalInfo.Tables(0).AcceptChanges()

            Catch ex As Exception
                HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
                HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "FilterAgeRecord", ex)
                Throw
            End Try
        End If
    End Sub

    Private Sub CalculateRetriementDate(ByRef dsPersonalInfo As DataSet, ByVal tdRetirementAge As Integer)
        Try
            Dim dcRetirement As New DataColumn("EstimatedRetirementDate", GetType(DateTime))
            If (Not dsPersonalInfo.Tables(0).Columns.Contains("EstimatedRetirementDate")) Then
                dsPersonalInfo.Tables(0).Columns.Add(dcRetirement)
            End If
            Dim dtbirthdate As DateTime
            For Each drPersonInfo As DataRow In dsPersonalInfo.Tables(0).Rows
                If (Convert.ToInt32(drPersonInfo.Item("Age")) < tdRetirementAge) Then
                    dtbirthdate = (Convert.ToDateTime(drPersonInfo.Item("BirthDate")))
                    drPersonInfo.Item("EstimatedRetirementDate") = GetFirstDayOfNextMonthIfNotFirstOfMonth(dtbirthdate.AddYears(tdRetirementAge))
                Else
                    ' 2012.01.30	Shashank Patel		Made changes to enable save button & Calculation of estimated Retirement date
                    drPersonInfo.Item("EstimatedRetirementDate") = GetFirstDayOfNextMonthIfNotFirstOfMonth(Date.Today)
                End If
            Next
            dsPersonalInfo.AcceptChanges()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "CalculateRetriementDate", ex)
        End Try
    End Sub

    Private Function ValidateEstimate(ByRef errorMessage As String, ByVal fundEventId As String, ByVal retirementDate As String, ByVal personId As String, ByVal SSNO As String, ByVal fundEventStatus As String, ByVal planType As String) As Boolean
        Dim IsRetValid As Boolean = False
        Try
            IsRetValid = RetirementBOClass.IsRetirementValid(errorMessage, True, fundEventId, retirementDate, "NORMAL", personId, SSNO, fundEventStatus, planType)
        Catch
            IsRetValid = False
        End Try
        Return IsRetValid
    End Function

    Private Function CreateNewActiveSalaryInformation(ByRef dsEmploymentDetails As DataSet) As DataTable
        Dim dtCol As DataColumn
        Dim dtEmploymentSalary As New DataTable
        Try
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

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "CreateNewActiveSalaryInformation", ex)
            Throw
        End Try
        Return dtEmploymentSalary
    End Function

    Public Sub UpdateActiveSalaryInformation(ByRef employmentSalaryInformation As DataTable, ByVal dtActiveEmployement As DataRow(), ByVal annualSalaryIncrease As String, ByVal annualSalaryIncreaseEffDate As String)
        Dim drEmpSalryRow As DataRow
        Try
            For Each dtEmpRow As DataRow In dtActiveEmployement
                drEmpSalryRow = employmentSalaryInformation.NewRow

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

                drEmpSalryRow("AnnualSalaryIncrease") = annualSalaryIncrease
                drEmpSalryRow("AnnualSalaryIncreaseEffDate") = annualSalaryIncreaseEffDate

                employmentSalaryInformation.Rows.Add(drEmpSalryRow)
            Next
            employmentSalaryInformation.AcceptChanges()

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "UpdateActiveSalaryInformation", ex)
            Throw
        End Try
    End Sub
    '2012.05.08 SP : BT-1032:Additional comments added for Gemini ID YRS 5.0-1574
    Private Function BatchEstimate(ByRef dtException As DataTable, ByRef businessLogic As RetirementBOClass, ByRef dsPersonalInfo As DataSet) As String
        Dim combinedDataset As DataSet
        Dim errorMessage As String
        Dim isReturn As Boolean = False
        Dim beneficiaryFullName As String
        Dim benDOB As DateTime
        Dim deathBenefitPercentAmount As Decimal = 0
        Dim fullDeathBenifitAmount As Decimal = 0
        Dim InputBeneficiaryType As String
        Dim planType As String
        Dim lstPraGuids As New ArrayList()
        Dim strPraGuid As String = String.Empty
        Session("GUID") = Nothing
        Dim isAvalBalSaving, isAvalBalRetirement As Boolean
        Dim decDeathBenefitPercentageToUse As Decimal
        Try
            For Each drPersonInfo As DataRow In dsPersonalInfo.Tables(0).Rows
                combinedDataset = New DataSet
                isReturn = False
                beneficiaryFullName = String.Empty
                deathBenefitPercentAmount = 0
                fullDeathBenifitAmount = 0
                InputBeneficiaryType = String.Empty

                planType = DropDownListPlanType.SelectedValue
                planType = IIf(planType = "A", "B", planType)
                isReturn = GetProjectedAcctBalances(errorMessage:=errorMessage, businessLogic:=businessLogic, FundEventId:=drPersonInfo("FundEventId") _
                             , PersRetirementDate:=drPersonInfo("EstimatedRetirementDate"), PersBirthDate:=drPersonInfo("BirthDate") _
                         , PersonId:=drPersonInfo("PersID"), SSNO:=drPersonInfo("SSN"), FundEventStatus:=drPersonInfo("FundStatus") _
                         , PlanType:=planType, ProjectedYearInterest:=DropDownlistProjectedYearInterest.SelectedValue, combinedDataset:=combinedDataset)




                If Not (isReturn) Then
                    Dim drOverAgePerson() As DataRow
                    dtException.Rows.Add(drPersonInfo("SSN"), drPersonInfo("FundIDNo"), drPersonInfo("FirstName"), drPersonInfo("LastName"), errorMessage)
                    dtException.AcceptChanges()
                    errorMessage = String.Empty
                    Continue For
                End If
                planType = DropDownListPlanType.SelectedValue
                ' 2012.02.02	SP : Made changes to handle RDB amount & handling Reason Message
                If Not (IsAvailableSufficientAmountbyPlan(dsCombinedDataset:=combinedDataset, paraPlanType:=planType, isAvailinSaving:=isAvalBalSaving, isAvailinRetirement:=isAvalBalRetirement)) Then
                    Dim drOverAgePerson() As DataRow
                    ' 2012.02.02	SP : Made changes to handle RDB amount & handling Reason Message
                    errorMessage = String.Format(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_INSUFFICIENT_BALANCE"), GetSelectedPlanName(DropDownListPlanType.SelectedValue, isAvalBalSaving, isAvalBalRetirement))
                    dtException.Rows.Add(drPersonInfo("SSN"), drPersonInfo("FundIDNo"), drPersonInfo("FirstName"), drPersonInfo("LastName"), errorMessage)
                    dtException.AcceptChanges()
                    errorMessage = String.Empty
                    Continue For
                End If

                'Added by Gunanithi - YRS-AT-2677- 08 Dec,2015, checking if the person's age is less than minimum age of retirement as of restricted date & accordingly calculating the death benefit to use fields. ----start
                decDeathBenefitPercentageToUse = 0
                If (IsDeathBenefitAnnuityPurchaseRestricted(Convert.ToDateTime(drPersonInfo("BirthDate").ToString())) = False) Then
                    decDeathBenefitPercentageToUse = DropdownlistPercentageToUse.SelectedValue
                End If

                CalculateAnnuties(errorMessage:=errorMessage, businessLogic:=businessLogic, combinedDataset:=combinedDataset, deathBenefitPercentageToUSe:=decDeathBenefitPercentageToUse,
                                  PlanType:=planType, retireType:="NORMAL", retirementDate:=drPersonInfo("EstimatedRetirementDate") _
                 , personBirthDate:=drPersonInfo("BirthDate"), SSNO:=drPersonInfo("SSN"), PersonId:=drPersonInfo("PersID") _
                 , deathBenefitPercentAmount:=deathBenefitPercentAmount, fullDeathBenefitAmount:=fullDeathBenifitAmount, beneficiaryFullName:=beneficiaryFullName, benDOB:=benDOB, InputBeneficiaryType:=InputBeneficiaryType _
                 , FundEventId:=drPersonInfo("FundEventId")) '//BD: YRS-AT-4133 Adding fund event id to validate RDB restriction
                'Gunanithi, YRS-AT-2677- 08-Dec-2015 ---------end

                'CalculateAnnuties(errorMessage:=errorMessage, businessLogic:=businessLogic, combinedDataset:=combinedDataset, deathBenifitPercentageToUSe:=DropdownlistPercentageToUse.SelectedValue _
                ' , PlanType:=planType, retireType:="NORMAL", retirementDate:=drPersonInfo("EstimatedRetirementDate") _
                ' , personBirthDate:=drPersonInfo("BirthDate"), SSNO:=drPersonInfo("SSN"), PersonId:=drPersonInfo("PersID") _
                ' , deathBenifitPercentAmount:=deathBenifitPercentAmount, fullDeathBenifitAmount:=fullDeathBenifitAmount, benificaryFullName:=benificaryFullName, benDOB:=benDOB, InputBeneficiaryType:=InputBeneficiaryType)

                errorMessage = String.Empty

                strPraGuid = PrintEstimation(errorMessage, combinedDataset, planType, fullDeathBenifitAmount, beneficiaryFullName, drPersonInfo("PersID") _
                  , drPersonInfo("EstimatedRetirementDate"), drPersonInfo("BirthDate"), benDOB, InputBeneficiaryType _
                  , drPersonInfo("FundEventId"), fullDeathBenifitAmount, deathBenefitPercentAmount _
                  , decDeathBenefitPercentageToUse, drPersonInfo("FundIDNo") _
                  , businessLogic.ProjFinalYearSal _
                  )
                lstPraGuids.Add(strPraGuid)

            Next

            BindExceptionGrid(dtException)
            ViewStatedtException = dtException

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "BatchEstimate", ex)
            Throw
        End Try
        Return GetPipeSeperated(lstPraGuids)
    End Function

    ' 2012.02.02	SP : Made changes to handle RDB amount & handling Reason Message
    Private Function GetSelectedPlanName(ByVal para_planType As String, ByVal isAvailBalSaving As Boolean, ByVal isAvailBalRetirement As Boolean) As String
        Dim strPlanName As String = String.Empty
        Try
            Select Case para_planType
                Case "A"
                    If (Not (isAvailBalSaving) AndAlso Not (isAvailBalRetirement)) Then
                        strPlanName = "any"
                    End If

                Case "B"
                    If (Not (isAvailBalSaving) AndAlso Not (isAvailBalRetirement)) Then
                        strPlanName = "both"
                    ElseIf Not (isAvailBalRetirement) Then
                        strPlanName = "retirement"
                    Else
                        strPlanName = "savings"
                    End If

                Case "S"
                    strPlanName = "savings"

                Case "R"
                    strPlanName = "retirement"

            End Select
            Return strPlanName
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "GetSelectedPlanName", ex)
        End Try
    End Function
    ' 2012.02.02	SP : Made changes to handle RDB amount & handling Reason Message -End

    Private Function IsAvailableSufficientAmountbyPlan(ByRef dsCombinedDataset As DataSet, ByRef paraPlanType As String, ByRef isAvailinSaving As Boolean, ByRef isAvailinRetirement As Boolean) As Boolean
        Dim isReturn As Boolean = False
        isAvailinSaving = False
        isAvailinRetirement = False
        Try
            If isNonEmpty(dsCombinedDataset) Then

                Dim filterExpression As String
                Dim val As String = String.Empty
                Dim totalBalance As Decimal
                Dim localPlanType As String

                If paraPlanType = "S" Then
                    filterExpression = "[chvPlanType] = 'SAVINGS'"
                    val = dsCombinedDataset.Tables(0).Compute("SUM(mnyBalance)", filterExpression).ToString()
                    'SP:2012.01.31: Handling issue where the string is in incorrect format error was happening - 'totalBalance = (IIf(val <> String.Empty, Convert.ToDecimal(val), 0))
                    If val <> String.Empty Then
                        totalBalance = Convert.ToDecimal(val)
                    Else
                        totalBalance = 0
                    End If
                    isReturn = (totalBalance > 5000)
                    ' 2012.02.02	SP : Made changes to handle RDB amount & handling Reason Message
                    isAvailinSaving = isReturn

                ElseIf (paraPlanType = "R") Then
                    filterExpression = "[chvPlanType] = 'RETIREMENT'"
                    val = dsCombinedDataset.Tables(0).Compute("SUM(mnyBalance)", filterExpression).ToString()
                    'SP:2012.01.31: Handling issue where the string is in incorrect format error was happening - 'totalBalance = (IIf(val <> String.Empty, Convert.ToDecimal(val), 0))
                    If val <> String.Empty Then
                        totalBalance = Convert.ToDecimal(val)
                    Else
                        totalBalance = 0
                    End If
                    isReturn = (totalBalance > 5000)
                    ' 2012.02.02	SP : Made changes to handle RDB amount & handling Reason Messagee
                    isAvailinRetirement = isReturn

                ElseIf paraPlanType = "B" Then
                    Dim valSav As String = String.Empty
                    Dim totalbalSav As Decimal

                    filterExpression = "[chvPlanType] = 'RETIREMENT'"
                    val = dsCombinedDataset.Tables(0).Compute("SUM(mnyBalance)", filterExpression).ToString()
                    'SP:2012.01.31: Handling issue where the string is in incorrect format error was happening - 'totalBalance = (IIf(val <> String.Empty, Convert.ToDecimal(val), 0))
                    If val <> String.Empty Then
                        totalBalance = Convert.ToDecimal(val)
                    Else
                        totalBalance = 0
                    End If

                    filterExpression = "[chvPlanType] = 'SAVINGS'"
                    valSav = dsCombinedDataset.Tables(0).Compute("SUM(mnyBalance)", filterExpression).ToString()
                    'SP:2012.01.31: Handling issue where the string is in incorrect format error was happening - 'totalBalance = (IIf(val <> String.Empty, Convert.ToDecimal(val), 0))
                    If valSav <> String.Empty Then
                        totalbalSav = Convert.ToDecimal(valSav)
                    Else
                        totalbalSav = 0
                    End If
                    ' 2012.02.02	SP : Made changes to handle RDB amount & handling Reason Message
                    isAvailinRetirement = totalBalance > 5000
                    isAvailinSaving = totalbalSav > 5000

                    If (totalBalance > 5000 AndAlso totalbalSav > 5000) Then
                        isReturn = True
                        paraPlanType = "B"
                    Else
                        isReturn = False
                    End If

                ElseIf paraPlanType = "A" Then

                    Dim valSav As String = String.Empty
                    Dim totalbalSav As Decimal

                    filterExpression = "[chvPlanType] = 'RETIREMENT'"
                    val = dsCombinedDataset.Tables(0).Compute("SUM(mnyBalance)", filterExpression).ToString()
                    'NP:2012.01.27: Handling issue where the string is in incorrect format error was happening - 'totalBalance = (IIf(val <> String.Empty, Convert.ToDecimal(val), 0))
                    If val <> String.Empty Then
                        totalBalance = Convert.ToDecimal(val)
                    Else
                        totalBalance = 0
                    End If

                    filterExpression = "[chvPlanType] = 'SAVINGS'"
                    valSav = dsCombinedDataset.Tables(0).Compute("SUM(mnyBalance)", filterExpression).ToString()
                    'NP:2012.01.27: Handling issue where the string is in incorrect format error was happening - 'totalbalSav = (IIf(valSav <> String.Empty, Convert.ToDecimal(valSav), 0))
                    If valSav <> String.Empty Then
                        totalbalSav = Convert.ToDecimal(valSav)
                    Else
                        totalbalSav = 0
                    End If
                    ' 2012.02.02	SP : Made changes to handle RDB amount & handling Reason Message
                    isAvailinRetirement = totalBalance > 5000
                    isAvailinSaving = totalbalSav > 5000

                    If (totalBalance > 5000 AndAlso totalbalSav > 5000) Then
                        isReturn = True
                        paraPlanType = "B"
                    ElseIf (totalbalSav > 5000) Then
                        isReturn = True
                        paraPlanType = "S"
                    ElseIf (totalBalance > 5000) Then
                        isReturn = True
                        paraPlanType = "R"
                    Else
                        isReturn = False
                    End If
                End If
            End If
            paraPlanType = IIf(paraPlanType = "A", "B", paraPlanType)
            Return isReturn
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "IsAvailableSufficientAmountbyPlan", ex)
        End Try
    End Function

    Private Function GetElectiveAccoutsByPlan(ByRef errorMessage As String, ByVal fundEventID As String, ByVal planType As String, ByVal retirementDate As String) As DataSet
        Dim dsElctiveAccountsByPlan As DataSet
        Dim dtNewColumn As DataColumn
        errorMessage = String.Empty
        Try

            dsElctiveAccountsByPlan = RetirementBOClass.GetElectiveAccountsByPlan(fundEventID, planType, retirementDate)
            If Not dsElctiveAccountsByPlan Is Nothing Then
                If dsElctiveAccountsByPlan.Tables.Count > 1 Then

                    If Not dsElctiveAccountsByPlan.Tables("RetireeElectiveAccountsInformation") Is Nothing Then

                        'Added new column in RetireeElectiveAccountsInformation table
                        dtNewColumn = dsElctiveAccountsByPlan.Tables("RetireeElectiveAccountsInformation").Columns.Add("Selected", Type.GetType("System.Boolean"))
                        dtNewColumn.DefaultValue = False
                    End If
                    If Not dsElctiveAccountsByPlan.Tables("RetireeGroupedElectiveAccounts") Is Nothing Then

                        'Added new column in RetireeGroupedElectiveAccounts Table                        
                        dtNewColumn = dsElctiveAccountsByPlan.Tables("RetireeGroupedElectiveAccounts").Columns.Add("Selected", Type.GetType("System.Boolean"))
                        dtNewColumn.DefaultValue = False
                        dtNewColumn = dsElctiveAccountsByPlan.Tables("RetireeGroupedElectiveAccounts").Columns.Add("bitEnabled", Type.GetType("System.Boolean"))
                        dtNewColumn.DefaultValue = False
                        'set added column value
                        If dsElctiveAccountsByPlan.Tables("RetireeGroupedElectiveAccounts").Rows.Count > 0 Then
                            'set selected column value 
                            For Each dr As DataRow In dsElctiveAccountsByPlan.Tables("RetireeGroupedElectiveAccounts").Rows
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

                        End If
                    End If

                End If 'l_dsElctiveAccountsByPlan.Tables.Count
                dsElctiveAccountsByPlan.AcceptChanges()
            Else
                'errorMessage = "Unable to retrive account information"//commented by Anudeep on 22-sep for BT-1126
                errorMessage = getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_UNABLE_TO_RETRIEVE")
            End If 'l_dsElctiveAccountsByPlan is not nothing

            Return dsElctiveAccountsByPlan

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "GetElectiveAccountsByPlan", ex)
            Throw
        End Try
    End Function

    Private Function GetElectiveAccounts(ByRef errorMessage As String, ByVal PlanType As String, ByVal FundEventID As String, ByVal PersonId As String, ByVal retirementDate As String) As DataSet
        Dim dsNewElectiveAccounts As DataSet
        Dim dsElectiveAccounts As DataSet
        Dim dsElectiveAccountDet As DataSet
        Dim dsUpdatedElectiveAccounts As DataSet
        Dim dtUpdatedNonGroupedElectiveAccounts As DataTable
        Try
            dsElectiveAccountDet = GetElectiveAccoutsByPlan(errorMessage, FundEventID, PlanType, retirementDate)
            If Not dsElectiveAccountDet Is Nothing Then
                dsNewElectiveAccounts = dsElectiveAccountDet.Copy()

                For Each dr As DataRow In dsNewElectiveAccounts.Tables("RetireeGroupedElectiveAccounts").Rows
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
                dsElectiveAccounts = RetirementBOClass.SearchElectiveAccounts(PersonId)

                Dim dtExistingElectiveAccounts As New DataTable

                If Not dsElectiveAccounts Is Nothing And TypeOf dsElectiveAccounts Is DataSet Then

                    If dsElectiveAccounts.Tables.Count > 0 Then
                        dtExistingElectiveAccounts = dsElectiveAccounts.Tables(0).Copy()
                        If Not dtExistingElectiveAccounts.Columns.Contains("Selected") Then
                            dtExistingElectiveAccounts.Columns.Add("Selected", System.Type.GetType("System.Boolean"))
                        End If
                    End If
                End If

                dsUpdatedElectiveAccounts = New DataSet

                dtUpdatedNonGroupedElectiveAccounts = New DataTable

                GetUpdatedNonGroupedElectiveAcct(PlanType, dtUpdatedNonGroupedElectiveAccounts, dsNewElectiveAccounts)

                Dim drExistingElectAcctFoundRow As DataRow()
                Dim dtUpdatedNonGroupedAcctCopy As DataTable
                dtUpdatedNonGroupedAcctCopy = dtUpdatedNonGroupedElectiveAccounts.Copy()
                ' Get the Retirement accounts            
                'Added existing elective retirement account row for projection & estimate
                'If start date are defind by user then put these start date as termination date for existing elective account row
                If PlanType = "R" Or PlanType = "B" Then

                    For Each drUpdatedNonGroupedCopyRow As DataRow In dtUpdatedNonGroupedAcctCopy.Rows
                        If drUpdatedNonGroupedCopyRow("PlanType") = "RETIREMENT" Then
                            drExistingElectAcctFoundRow = dtExistingElectiveAccounts.Select("chrAcctType='" & drUpdatedNonGroupedCopyRow("chrAcctType") & "' And PlanType='" & drUpdatedNonGroupedCopyRow("PlanType") & "'")
                            If drExistingElectAcctFoundRow.Length > 0 Then
                                If drUpdatedNonGroupedCopyRow("dtmEffDate").ToString().Trim() <> String.Empty Then
                                    drExistingElectAcctFoundRow(0)("dtsTerminationDate") = DateAdd(DateInterval.Day, -1, Convert.ToDateTime(drUpdatedNonGroupedCopyRow("dtmEffDate"))).ToString()
                                Else
                                    If drUpdatedNonGroupedCopyRow("dtsTerminationDate").ToString().Trim() <> String.Empty Then
                                        drExistingElectAcctFoundRow(0)("dtsTerminationDate") = drUpdatedNonGroupedCopyRow("dtsTerminationDate")
                                    End If
                                End If
                                dtUpdatedNonGroupedElectiveAccounts.ImportRow(drExistingElectAcctFoundRow(0))
                            End If
                        End If
                    Next
                End If

                ' Get the Savings accounts
                'Added existing elective saving accounts row for projection & estimate
                'If start date are defind by user then put these start date as termination date for existing elective account row
                If PlanType = "S" Or PlanType = "B" Then

                    For Each drUpdatedNonGroupedCopyRow As DataRow In dtUpdatedNonGroupedAcctCopy.Rows
                        If drUpdatedNonGroupedCopyRow("PlanType") = "SAVINGS" Then
                            drExistingElectAcctFoundRow = dtExistingElectiveAccounts.Select("chrAcctType='" & drUpdatedNonGroupedCopyRow("chrAcctType") & "' And PlanType='" & drUpdatedNonGroupedCopyRow("PlanType") & "'")
                            If drExistingElectAcctFoundRow.Length > 0 Then
                                If drUpdatedNonGroupedCopyRow("dtmEffDate").ToString().Trim() <> String.Empty Then
                                    drExistingElectAcctFoundRow(0)("dtsTerminationDate") = DateAdd(DateInterval.Day, -1, Convert.ToDateTime(drUpdatedNonGroupedCopyRow("dtmEffDate"))).ToString()
                                Else
                                    If drUpdatedNonGroupedCopyRow("dtsTerminationDate").ToString().Trim() <> String.Empty Then
                                        drExistingElectAcctFoundRow(0)("dtsTerminationDate") = drUpdatedNonGroupedCopyRow("dtsTerminationDate")
                                    End If
                                End If

                                dtUpdatedNonGroupedElectiveAccounts.ImportRow(drExistingElectAcctFoundRow(0))
                            End If
                        End If
                    Next
                End If
                dtUpdatedNonGroupedElectiveAccounts.AcceptChanges()
                dsUpdatedElectiveAccounts.Tables.Add(dtUpdatedNonGroupedElectiveAccounts)
            End If

            Return dsUpdatedElectiveAccounts

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "GetElectiveAccounts", ex)
            Throw
        End Try
    End Function

    Private Function GetTerminationDateForProjBalValidation(ByVal RetirementDate As String, ByVal dtEmploymentSalaryInfo As DataTable, ByVal para_dtEmploymentDetails As DataTable) As String
        Dim dtEmpTermination As DataTable
        Dim drEmpSalaryFoundRow As DataRow()
        Dim drEmpTerminationNewRow As DataRow
        Dim strTerminationDate As String = String.Empty
        Try
            dtEmpTermination = New DataTable
            dtEmpTermination.Columns.Add(New DataColumn("guiEmpEventID", System.Type.GetType("System.String")))
            dtEmpTermination.Columns.Add(New DataColumn("TerminationDate", System.Type.GetType("System.DateTime")))
            If Not para_dtEmploymentDetails Is Nothing AndAlso Not dtEmploymentSalaryInfo Is Nothing Then
                If para_dtEmploymentDetails.Rows.Count > 0 Then
                    For Each drEmpDetailRow As DataRow In para_dtEmploymentDetails.Rows
                        drEmpTerminationNewRow = dtEmpTermination.NewRow()
                        drEmpTerminationNewRow("guiEmpEventID") = drEmpDetailRow("guiEmpEventID").ToString().ToUpper()

                        If drEmpDetailRow.Item("dtmTerminationDate") Is DBNull.Value Or drEmpDetailRow.Item("dtmTerminationDate").ToString() = String.Empty Then
                            If dtEmploymentSalaryInfo.Rows.Count > 0 Then
                                drEmpSalaryFoundRow = dtEmploymentSalaryInfo.Select("EmpEventID='" & drEmpDetailRow("guiEmpEventID").ToString().Trim().ToUpper() & "'")
                                If drEmpSalaryFoundRow.Length > 0 Then

                                    If drEmpSalaryFoundRow(0).Item("EndWorkDate") Is DBNull.Value Or drEmpSalaryFoundRow(0).Item("EndWorkDate").ToString() = String.Empty Then
                                        drEmpTerminationNewRow("TerminationDate") = RetirementDate
                                    Else
                                        drEmpTerminationNewRow("TerminationDate") = drEmpSalaryFoundRow(0)("EndWorkDate")
                                    End If
                                Else
                                    drEmpTerminationNewRow("TerminationDate") = RetirementDate
                                End If
                            Else
                                drEmpTerminationNewRow("TerminationDate") = RetirementDate
                            End If
                        Else
                            drEmpTerminationNewRow("TerminationDate") = Convert.ToDateTime(drEmpDetailRow("dtmTerminationDate")).ToString("MM/dd/yyyy")
                        End If
                        dtEmpTermination.Rows.Add(drEmpTerminationNewRow)
                    Next
                    dtEmpTermination.AcceptChanges()
                End If
            End If
            Dim l_drTerminationFoundRow As DataRow() = dtEmpTermination.Select("TerminationDate=MAX(TerminationDate)")
            If l_drTerminationFoundRow.Length > 0 Then
                strTerminationDate = l_drTerminationFoundRow(0)("TerminationDate")
            End If
            Return strTerminationDate
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "GetTerminationDateFoProjValidation", ex)
            Throw ex
        End Try
    End Function

    'Private Function CalculateAccountBalancesAndProjections(ByRef businessLogic As RetirementBOClass, ByVal dsRetEstimateEmployment As DataSet, ByVal txtRetireeBirthday As String _
    '        , ByVal txtRetirementDate As String, ByVal personID As String, ByVal fundeventID As String, ByVal retireType As String _
    '        , ByVal projectedInterestRate As Double, ByVal dsElectiveAccounts As DataSet, ByVal dsCombinedDataSet As DataSet _
    '        , ByVal isEstimate As Boolean, ByVal planType As String, ByVal fundStatus As String, ByRef errorMessage As String _
    '        , ByRef warningMessage As String, ByVal dtExcludedAccounts As DataTable, ByVal dtEmploymentDetails As DataTable _
    '        , ByVal isEsimateProjBal As Boolean, ByVal paraMaxTerminationDate As String, ByRef isYmcaLegacyAcctTotalExceed As Boolean, ByRef isYmcaAcctTotalExceed As Boolean) As Boolean

    '    Dim hasnoError As Boolean
    '    Try
    '        hasnoError = businessLogic.CalculateAccountBalancesAndProjections(dsRetEstimateEmployment, txtRetireeBirthday, txtRetirementDate _
    '                                                        , personID, fundeventID, retireType, projectedInterestRate, dsElectiveAccounts _
    '                                                        , dsCombinedDataSet, isEstimate, planType, fundStatus, errorMessage, warningMessage _
    '                                                        , dtExcludedAccounts, dtEmploymentDetails, isEsimateProjBal, paraMaxTerminationDate _
    '                                                        , isYmcaLegacyAcctTotalExceed, isYmcaAcctTotalExceed)
    '    Catch ex As Exception
    '        hasnoError = False
    '    End Try
    '    Return hasnoError
    'End Function

    Private Function GetUpdatedNonGroupedElectiveAcct(ByVal PlanType As String, ByRef paradtUpdatedNonGroupedElectiveAcct As DataTable, ByVal paradsElectiveAccounts As DataSet)

        Dim dtNonGroupedElectiveAcct As DataTable
        Dim dtGroupedElectiveAcct As DataTable
        Dim drNonGroupedAcctFoundRows As DataRow()
        Dim drGroupedAcctFoundRows As DataRow()
        Dim filterExpression As String = String.Empty
        Try
            If Not paradsElectiveAccounts Is Nothing Then
                If paradsElectiveAccounts.Tables.Count > 1 Then

                    If paradsElectiveAccounts.Tables.Contains("RetireeElectiveAccountsInformation") AndAlso paradsElectiveAccounts.Tables.Contains("RetireeGroupedElectiveAccounts") Then
                        paradtUpdatedNonGroupedElectiveAcct = paradsElectiveAccounts.Tables("RetireeElectiveAccountsInformation").Clone()
                        If paradsElectiveAccounts.Tables("RetireeElectiveAccountsInformation").Rows.Count > 0 AndAlso paradsElectiveAccounts.Tables("RetireeGroupedElectiveAccounts").Rows.Count > 0 Then
                            dtNonGroupedElectiveAcct = New DataTable
                            dtGroupedElectiveAcct = New DataTable
                            dtNonGroupedElectiveAcct = paradsElectiveAccounts.Tables("RetireeElectiveAccountsInformation")
                            dtGroupedElectiveAcct = paradsElectiveAccounts.Tables("RetireeGroupedElectiveAccounts")
                        End If

                    End If
                End If
            End If

            If Not paradtUpdatedNonGroupedElectiveAcct Is Nothing AndAlso Not dtNonGroupedElectiveAcct Is Nothing AndAlso Not dtGroupedElectiveAcct Is Nothing Then
                paradtUpdatedNonGroupedElectiveAcct = dtNonGroupedElectiveAcct.Clone()
                ' Retirement Plane
                'Added basic account rows 
                If PlanType = "R" Or PlanType = "B" Then

                    drNonGroupedAcctFoundRows = dtNonGroupedElectiveAcct.Select("bitBasicAcct=True And PlanType='RETIREMENT'")
                    If drNonGroupedAcctFoundRows.Length > 0 Then
                        drGroupedAcctFoundRows = dtGroupedElectiveAcct.Select("bitBasicAcct=True And PlanType='RETIREMENT'")
                        For Each nonGroupedAcctRow As DataRow In drNonGroupedAcctFoundRows
                            If drGroupedAcctFoundRows.Length > 0 Then
                                nonGroupedAcctRow("guiEmpEventID") = drGroupedAcctFoundRows(0)("guiEmpEventID")
                                paradtUpdatedNonGroupedElectiveAcct.ImportRow(nonGroupedAcctRow)
                            End If

                        Next
                    End If
                    'added non basic account and update account contribution
                    drNonGroupedAcctFoundRows = Nothing
                    drGroupedAcctFoundRows = dtGroupedElectiveAcct.Select("bitBasicAcct=False And PlanType='RETIREMENT'")
                    If drGroupedAcctFoundRows.Length Then
                        For Each groupedAcctRow As DataRow In drGroupedAcctFoundRows
                            If groupedAcctRow("Selected") = True Then
                                drNonGroupedAcctFoundRows = dtNonGroupedElectiveAcct.Select("bitBasicAcct=False And PlanType='RETIREMENT' And  chrAcctType='" & groupedAcctRow("chrLegacyAcctType") & "'")
                                If drNonGroupedAcctFoundRows.Length > 0 Then
                                    drNonGroupedAcctFoundRows(0)("guiEmpEventID") = groupedAcctRow("guiEmpEventID")
                                    drNonGroupedAcctFoundRows(0)("chrAdjustmentBasisCode") = groupedAcctRow("chrAdjustmentBasisCode")
                                    drNonGroupedAcctFoundRows(0)("mnyAddlContribution") = IIf(groupedAcctRow("mnyAddlContribution").ToString() <> String.Empty, Convert.ToDecimal(groupedAcctRow("mnyAddlContribution")), 0)
                                    drNonGroupedAcctFoundRows(0)("dtmEffDate") = IIf(groupedAcctRow("dtmEffDate").ToString() <> String.Empty, groupedAcctRow("dtmEffDate"), String.Empty)
                                    drNonGroupedAcctFoundRows(0)("dtsTerminationDate") = IIf(groupedAcctRow("dtsTerminationDate").ToString() <> String.Empty, groupedAcctRow("dtsTerminationDate"), String.Empty)
                                    drNonGroupedAcctFoundRows(0)("Selected") = groupedAcctRow("Selected")
                                    paradtUpdatedNonGroupedElectiveAcct.ImportRow(drNonGroupedAcctFoundRows(0))
                                End If
                            End If
                        Next
                    End If
                End If
                'Saving Plan
                If PlanType = "S" Or PlanType = "B" Then

                    'added non basic account and update account contribution
                    drNonGroupedAcctFoundRows = Nothing
                    drGroupedAcctFoundRows = dtGroupedElectiveAcct.Select("bitBasicAcct=False And PlanType='SAVINGS'")
                    If drGroupedAcctFoundRows.Length Then
                        For Each groupedAcctRow As DataRow In drGroupedAcctFoundRows
                            If groupedAcctRow("Selected") = True Then
                                drNonGroupedAcctFoundRows = dtNonGroupedElectiveAcct.Select("bitBasicAcct=False And PlanType='SAVINGS'  And chrAcctType='" & groupedAcctRow("chrLegacyAcctType") & "'")
                                If drNonGroupedAcctFoundRows.Length > 0 Then
                                    drNonGroupedAcctFoundRows(0)("guiEmpEventID") = groupedAcctRow("guiEmpEventID")
                                    drNonGroupedAcctFoundRows(0)("chrAdjustmentBasisCode") = groupedAcctRow("chrAdjustmentBasisCode")
                                    drNonGroupedAcctFoundRows(0)("mnyAddlContribution") = IIf(groupedAcctRow("mnyAddlContribution").ToString() <> String.Empty, Convert.ToDecimal(groupedAcctRow("mnyAddlContribution")), 0)
                                    drNonGroupedAcctFoundRows(0)("dtmEffDate") = IIf(groupedAcctRow("dtmEffDate").ToString() <> String.Empty, groupedAcctRow("dtmEffDate"), String.Empty)
                                    drNonGroupedAcctFoundRows(0)("dtsTerminationDate") = IIf(groupedAcctRow("dtsTerminationDate").ToString() <> String.Empty, groupedAcctRow("dtsTerminationDate"), String.Empty)
                                    drNonGroupedAcctFoundRows(0)("Selected") = True
                                    paradtUpdatedNonGroupedElectiveAcct.ImportRow(drNonGroupedAcctFoundRows(0))
                                End If
                            End If
                        Next
                    End If
                End If
            End If
            paradtUpdatedNonGroupedElectiveAcct.AcceptChanges()
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "GetUpdatedNonGroupedElectiveAccount", ex)
            Throw ex
        End Try
    End Function

    'Private Function GetPersonEmploymentDetails(ByVal FundEventId As String) As DataSet
    '    Dim dsPersonEmploymentDetails As DataSet
    '    dsPersonEmploymentDetails = RetirementBOClass.SearchRetEmpInfo(FundEventId)
    '    Return dsPersonEmploymentDetails
    'End Function

    Private Function isNonEmpty(ByRef ds As DataSet) As Boolean
        If ds Is Nothing Then Return False
        If ds.Tables.Count = 0 Then Return False
        If ds.Tables(0).Rows.Count = 0 Then Return False
        Return True
    End Function

    Private Sub CalculateAnnuties(ByVal errorMessage As String, ByRef businessLogic As RetirementBOClass _
             , ByRef combinedDataset As DataSet, ByVal deathBenefitPercentageToUSe As String _
             , ByVal PlanType As String, ByVal retireType As String, ByVal retirementDate As String _
             , ByVal personBirthDate As String, ByVal SSNO As String, ByVal PersonId As String _
             , ByRef deathBenefitPercentAmount As Decimal, ByRef fullDeathBenefitAmount As Decimal _
             , ByRef beneficiaryFullName As String, ByRef benDOB As DateTime, ByRef InputBeneficiaryType As String _
             , ByRef FundEventId As String) '//BD: YRS-AT-4133 Adding fund event id to validate RDB restriction

        fullDeathBenefitAmount = 0
        deathBenefitPercentAmount = 0

        InputBeneficiaryType = String.Empty

        Dim ExactAgeEffDate As String
        Dim dtAnnuitiesList_Ret As DataTable
        Dim finalAnnuity As Decimal
        Dim dtAnnuitiesParam As DataTable
        Dim dtAnnuitiesFullBalanceParam As DataTable
        Dim dtAnnuitiesList_Sav As DataTable
        Dim dtAnnuitiesList_final As DataTable
        Dim dsParticipantBeneficiaries As DataSet
        Dim dsSelectedBeneficiary As New DataSet
        Dim beneficiaryDOB As String = String.Empty
        'Dim InputBeneficiaryType As String
        Dim beneficiaryFirstName As String
        Dim beneficiaryLastName As String

        Try

            benDOB = New DateTime(1900, 1, 1)
            businessLogic.calculateFinalAmounts(combinedDataset.Tables(0), "R")

            businessLogic.UpdateBasisTypeAsPerAnnuitizeFactor(retireType, retirementDate)

            If (PlanType = "B" Or PlanType = "R" Or PlanType = String.Empty) Then 'And Me.OrgBenTypeIsQDROorRBEN = False Then
                Dim stRDBwarningMessage As String = String.Empty 'BD YRS-AT-4133 -  Adding field to hold RDB warning message
                fullDeathBenefitAmount = businessLogic.GetRetiredDeathBenefit(retireType, Convert.ToDateTime(retirementDate), Convert.ToDateTime(personBirthDate), FundEventId, stRDBwarningMessage)  'BD YRS-AT-4133 - Adding fund event id to check 2019 plan rule change and getting the RDB message 

                deathBenefitPercentAmount = Math.Round((fullDeathBenefitAmount * (deathBenefitPercentageToUSe) / 100), 2)
            End If
            dsParticipantBeneficiaries = RetirementBOClass.getParticipantBeneficiaries(PersonId)
            If dsParticipantBeneficiaries.Tables(0).Rows.Count > 0 Then
                Dim drdatarowsAnnuityBen As DataRow()
                Dim drdatarowAnnuityBen As DataRow

                'Look for spouse with 'PRIMARY' Beneficiary Group
                'START--Manthan Rajguru | 2015.11.09 | YRS-AT-2431 | Commented the exisiting code and added "chvBeneficiaryTypeCode = 'MEMBER' to the existing query
                'drdatarowsAnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvRelationshipCode = 'SP' AND chvBeneficiaryGroupCode = 'PRIM' ")
                drdatarowsAnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvRelationshipCode = 'SP' AND chvBeneficiaryGroupCode = 'PRIM' AND chvBeneficiaryTypeCode = 'MEMBER' ")
                'END--Manthan Rajguru | 2015.11.09 | YRS-AT-2431 | Commented the exisiting code and added "chvBeneficiaryTypeCode = 'MEMBER' to the existing query
                If drdatarowsAnnuityBen.Length > 0 Then
                    drdatarowAnnuityBen = drdatarowsAnnuityBen(0)
                    InputBeneficiaryType = BeneficiaryType.Spouse
                Else
                    'SG: 2012.12.04: BT-1432
                    'Look for spouse with 'CONTIGENT' Beneficiary Group
                    'START--Manthan Rajguru | 2015.11.09 | YRS-AT-2431 | Commented the exisiting code and added "chvBeneficiaryTypeCode = 'MEMBER' to the existing query
                    'drdatarowsAnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvRelationshipCode = 'SP' AND chvBeneficiaryGroupCode = 'CONT' ")
                    drdatarowsAnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvRelationshipCode = 'SP' AND chvBeneficiaryGroupCode = 'CONT' AND chvBeneficiaryTypeCode = 'MEMBER' ")
                    'END--Manthan Rajguru | 2015.11.09 | YRS-AT-2431 | Commented the exisiting code and added "chvBeneficiaryTypeCode = 'MEMBER' to the existing query
                    If drdatarowsAnnuityBen.Length > 0 Then
                        drdatarowAnnuityBen = drdatarowsAnnuityBen(0)
                        InputBeneficiaryType = BeneficiaryType.Spouse
                    End If
                End If
                'SG: 2012.12.04: BT-1432

                If String.IsNullOrEmpty(InputBeneficiaryType) Then
                    'Look for single PRIMARY beneficiary with 100%
                    'START--Manthan Rajguru | 2015.11.09 | YRS-AT-2431 | Commented the exisiting code and added "chvBeneficiaryTypeCode = 'MEMBER' to the existing query
                    'drdatarowsAnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg = 100 AND chvRelationshipCode NOT IN ('ES', 'IN', 'TR')")
                    drdatarowsAnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvBeneficiaryGroupCode = 'PRIM' AND chvBeneficiaryTypeCode = 'MEMBER' AND intBenefitPctg = 100 AND chvRelationshipCode NOT IN ('ES', 'IN', 'TR')")
                    'END--Manthan Rajguru | 2015.11.09 | YRS-AT-2431 | Commented the exisiting code and added "chvBeneficiaryTypeCode = 'MEMBER' to the existing query
                    'SR:2012.11.19 : If more then one beneficiary exists then do not cosider them for JS annuities.
                    If drdatarowsAnnuityBen.Length = 1 Then
                        InputBeneficiaryType = BeneficiaryType.NonSpouse
                        drdatarowAnnuityBen = drdatarowsAnnuityBen(0)
                    Else
                        drdatarowAnnuityBen = Nothing
                    End If
                    'End, SR:2012.11.19 : If more then one beneficiary exists then do not cosider them for JS annuities.
                End If

                'Commented By SG: 2013.01.10: BT-1432 (Re-Opened)
                ''SG: 2012.12.06: BT-1432
                'If String.IsNullOrEmpty(InputBeneficiaryType) Then
                '    'Look for single CONTIGENT beneficiary with 100%
                '    drdatarowsAnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvBeneficiaryGroupCode = 'CONT' AND intBenefitPctg = 100 AND chvRelationshipCode NOT IN ('ES', 'IN', 'TR')")
                '    'If more then one beneficiary exists then do not cosider them for JS annuities.
                '    If drdatarowsAnnuityBen.Length = 1 Then
                '        InputBeneficiaryType = BeneficiaryType.NonSpouse
                '        drdatarowAnnuityBen = drdatarowsAnnuityBen(0)
                '    Else
                '        drdatarowsAnnuityBen = Nothing
                '    End If
                'End If
                'SG: 2012.12.06: BT-1432

                If Not (drdatarowsAnnuityBen Is Nothing) Then
                    If drdatarowsAnnuityBen.Length > 0 Then
                        drdatarowAnnuityBen = drdatarowsAnnuityBen(0)
                        beneficiaryLastName = CType(drdatarowAnnuityBen("BenLastName") & "", String)
                        beneficiaryFirstName = CType(drdatarowAnnuityBen("BenFirstName") & "", String)
                        beneficiaryDOB = CType(drdatarowAnnuityBen("BenBirthDate") & "", String)
                        Me.BeneRelationShip = CType(drdatarowAnnuityBen("chvRelationshipCode") & "", String)  ' ML | 2020.02.06 | YRS-AT-4783 | Declare public property to store beneficary RelationShip Code
                        'SR:2012.11.19 : Remove all other beneficiaries from dataset to calculate annuities for one beneficiary
                        If (Not String.IsNullOrEmpty(beneficiaryDOB)) Then
                            For i As Integer = dsParticipantBeneficiaries.Tables(0).Rows.Count - 1 To 0 Step -1
                                Dim dtBirthdate As String
                                dtBirthdate = IIf(String.IsNullOrEmpty(dsParticipantBeneficiaries.Tables(0).Rows(i)("BenBirthDate").ToString()), "01/01/1900", dsParticipantBeneficiaries.Tables(0).Rows(i)("BenBirthDate").ToString())
                                'dtBirthdate = IIf(dsParticipantBeneficiaries.Tables(0).Rows(i)("BenBirthDate").ToString() = "" Or dsParticipantBeneficiaries.Tables(0).Rows(i)("BenBirthDate").ToString() = "System.DBNull", "01/01/1900", dsParticipantBeneficiaries.Tables(0).Rows(i)("BenBirthDate").ToString())
                                If Convert.ToDateTime(dtBirthdate) <> Convert.ToDateTime(beneficiaryDOB) Then
                                    dsParticipantBeneficiaries.Tables(0).Rows.RemoveAt(i)
                                End If
                            Next
                            dsParticipantBeneficiaries.AcceptChanges()
                        End If
                    Else
                        InputBeneficiaryType = BeneficiaryType.Manual
                        Me.BeneRelationShip = ""  ' ML | 2020.02.06 | YRS-AT-4783 | Declare public property to store beneficary RelationShip Code
                    End If
                Else
                    InputBeneficiaryType = BeneficiaryType.Manual
                    Me.BeneRelationShip = ""  ' ML | 2020.02.06 | YRS-AT-4783 | Declare public property to store beneficary RelationShip Code
                End If

                'Added By SG: 2012.12.11: BT-1426:
                If InputBeneficiaryType = BeneficiaryType.Manual Then
                    'Look for single PRIMARY non-human beneficiary with 100%
                    ''SP : 2013.11.12 BT-2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee  -start 
                    If (dsParticipantBeneficiaries.Tables(0).Rows.Count = 1) Then
                        drdatarowsAnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvBeneficiaryGroupCode = 'PRIM' AND BenLastName ='*Estate'  AND chvRelationshipCode ='ES'")
                        If drdatarowsAnnuityBen.Length = 1 Then
                            beneficiaryFirstName = CType(drdatarowsAnnuityBen(0)("BenFirstName") & "", String)
                            beneficiaryLastName = CType(drdatarowsAnnuityBen(0)("BenLastName") & "", String)
                        End If
                    End If
                    ''SP : 2013.11.12 BT-2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee  -end

                    ''SP : 2013.11.12 BT-2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee  -start -commented
                    'drdatarowsAnnuityBen = dsParticipantBeneficiaries.Tables(0).Select("chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg = 100 AND chvRelationshipCode IN ('ES')")
                    'If drdatarowsAnnuityBen.Length = 1 Then
                    '    beneficiaryFirstName = CType(drdatarowsAnnuityBen(0)("BenFirstName") & "", String)
                    '    beneficiaryLastName = CType(drdatarowsAnnuityBen(0)("BenLastName") & "", String)
                    'End If
                    ''SP : 2013.11.12 BT-2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee  -end -commented
                    drdatarowsAnnuityBen = Nothing
                    dsParticipantBeneficiaries = Nothing
                End If
                'END: Added By SG: 2012.12.11: BT-1426:
                ''SP : 2013.11.12 BT-2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee  -start (added else condition)
            Else
                beneficiaryLastName = "*Estate"
                ''SP : 2013.11.12 BT-2062/YRS 5.0-2114: Error msg when trying to run PRA for alternate payee  -End
            End If
            'End, SR:2012.11.19 : Remove all other beneficiaries from dataset to calculate annuities for one beneficiary

            'START: Added By SG: 2012.12.18: BT-1426:
            If Not String.IsNullOrEmpty(beneficiaryFirstName) AndAlso Not String.IsNullOrEmpty(beneficiaryLastName) Then
                beneficiaryFullName = beneficiaryFirstName.Trim() & " " & beneficiaryLastName.Trim()
            ElseIf Not String.IsNullOrEmpty(beneficiaryFirstName) Then
                beneficiaryFullName = beneficiaryFirstName.Trim()
            ElseIf Not String.IsNullOrEmpty(beneficiaryLastName) Then
                beneficiaryFullName = beneficiaryLastName.Trim()
            End If
            'END: Added By SG: 2012.12.18: BT-1426:

            If (Not String.IsNullOrEmpty(beneficiaryDOB)) Then
                benDOB = Convert.ToDateTime(beneficiaryDOB)
            End If

            If PlanType.ToUpper() = "R" Or PlanType.ToUpper() = "B" Then

                'businessLogic.calculateFinalAmounts(combinedDataset.Tables(0), "R")

                'businessLogic.UpdateBasisTypeAsPerAnnuitizeFactor(retireType, retirementDate)

                'Set businessLogic.g_dtAcctBalancesByBasisType only with Retirement plan balances
                If retireType.ToUpper() = "DISABL" Then
                    dtAnnuitiesList_Ret = businessLogic.CalculateAnnuitiesWithExactAgeForDisability(0, retireType _
                   , benDOB, Convert.ToDateTime(personBirthDate), Convert.ToDateTime(retirementDate) _
                   , dsParticipantBeneficiaries _
                   , deathBenefitPercentAmount, SSNO, PersonId _
                   , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)
                Else
                    'For Normal Retirement
                    dtAnnuitiesList_Ret = businessLogic.CalculateAnnuitiesWithExactAge(0, retireType _
                   , benDOB, Convert.ToDateTime(personBirthDate), Convert.ToDateTime(retirementDate) _
                   , dsParticipantBeneficiaries _
                   , deathBenefitPercentAmount, SSNO, PersonId _
                   , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)
                End If

                'Step 3.1 Get the Combo table
                If (deathBenefitPercentAmount > 0) Then
                    Dim dtAnnuitiesListWithDeathBenefit As DataTable
                    dtAnnuitiesListWithDeathBenefit = businessLogic.CalculateAnnuitiesWithExactAge(deathBenefitPercentAmount, retireType _
                        , benDOB, Convert.ToDateTime(personBirthDate), Convert.ToDateTime(retirementDate) _
                        , dsParticipantBeneficiaries _
                        , deathBenefitPercentAmount, SSNO, PersonId _
                        , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)

                    dtAnnuitiesList_Ret = RetirementBOClass.CreateComboTable(dtAnnuitiesList_Ret, dtAnnuitiesListWithDeathBenefit) 'isFullBalancePassed)
                End If

            End If

            If PlanType.ToUpper() = "S" Or PlanType.ToUpper() = "B" Then

                businessLogic.g_dtAcctBalancesByBasisType = Nothing
                businessLogic.calculateFinalAmounts(combinedDataset.Tables(0), "S")
                'For saving plan no need to update PRE and PST group basis type with effective basistype of PST group,
                'that is the reson even if retirement type is Disability pass retirement type is NORMAL 
                'Find Max Annuitized factor and update g_dtAcctBalancesByBasis
                businessLogic.UpdateBasisTypeAsPerAnnuitizeFactor("NORMAL", retirementDate)
                'If retirement date is greater than Ecact age effective date then use exact age logic
                dtAnnuitiesList_Sav = businessLogic.CalculateAnnuitiesWithExactAge(0, retireType _
                , benDOB, Convert.ToDateTime(personBirthDate), Convert.ToDateTime(retirementDate) _
                , dsParticipantBeneficiaries _
                , deathBenefitPercentAmount, SSNO, PersonId _
                , dtAnnuitiesFullBalanceParam, dtAnnuitiesParam, finalAnnuity)
            End If

            ' Combined both plan annuity
            If PlanType.ToUpper() = "R" Then
                dtAnnuitiesList_final = dtAnnuitiesList_Ret
            ElseIf PlanType.ToUpper() = "S" Then
                dtAnnuitiesList_final = dtAnnuitiesList_Sav
            Else
                dtAnnuitiesList_final = RetirementBOClass.CombinedRetAndSavAnnuityListTable(dtAnnuitiesList_Ret, dtAnnuitiesList_Sav)

            End If

            If Not (dtAnnuitiesList_final Is Nothing) Then
                combinedDataset.Tables.Add(dtAnnuitiesList_final)
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "CalculateAnnuties", ex)
            Throw
        End Try
    End Sub

    Private Function AreAnyAnnuitiesNA(ByVal p_Annuity As DataTable) As Boolean
        Try
            For Each dr In p_Annuity.Rows
                If (dr("Retire") = RetirementBOClass.JSAnnuityUnAvailableValue) Then
                    Return True
                End If
            Next
            Return False
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "AreAnyAnnuitiesNA", ex)
        End Try
    End Function
    '2012.07.13 SP :BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page (adding Addtional parameter salary)
    Private Function PrintEstimation(ByRef errorMessage As String, ByVal dsCombinedDataset As DataSet, ByVal PlanType As String _
           , ByVal RetiredDeathBenAmt As Decimal, ByVal BeneficiaryFullName As String _
           , ByVal PersonId As String, ByVal retirementDate As String _
           , ByVal retireeBirthDate As String, ByVal beneficiaryBirthDate As DateTime _
           , ByVal InputBeneficiaryType As String, ByVal FundEventId As String _
           , ByVal fullDeathBenifitAmount As Decimal, ByVal deathBenifitPercentAmount As Decimal _
     , ByVal deathBenifitPercentageToUSe As String, ByVal FundNo As String _
     , ByVal projFinalYearSal As Decimal) As String

        Dim lcAnnuitiesListCursor As DataTable
        Dim dtAccountsBasisByProjection As DataTable
        Dim printMsg As String

        lcAnnuitiesListCursor = dsCombinedDataset.Tables(1)
        dtAccountsBasisByProjection = dsCombinedDataset.Tables(0)

        Dim dtFinalAnnuity As New DataTable

        Try


            dtFinalAnnuity = RetirementBOClass.CalculatePayments((deathBenifitPercentageToUSe > 0), RetiredDeathBenAmt _
             , beneficiaryBirthDate, Convert.ToDateTime(retireeBirthDate) _
             , Convert.ToDateTime(retirementDate), lcAnnuitiesListCursor, dtFinalAnnuity _
             , PlanType, PersonId, FundEventId)

            dtFinalAnnuity = RetirementBOClass.CalculateFinalAnnuity(dtFinalAnnuity)

            'SR:2012.11.15: YRS 5.0-1329 - Commented below line of code
            'Dim ageDiff As Int16
            'ageDiff = RetirementBOClass.GetRetireeAndBeneficiaryAgeDiff(retireeBirthDate, beneficiaryBirthDate, retirementDate)
            'RetirementBOClass.AvailJandSAnnuityOptionsForPrintEstimate(ageDiff, lcAnnuitiesListCursor)
            'dtFinalAnnuity = RetirementBOClass.AvailJandSAnnuityOptions(ageDiff, dtFinalAnnuity)
            'End, SR:2012.11.15: YRS 5.0-1329 - Commented below line of code

            'SR:2012.11.15: YRS 5.0-1329 - J&S options available to non-spouse beneficiaries
            'YRS 5.0-1329:J&S options available to non-spouse beneficiaries
            Dim dtbeneficiaryBirthDate As String = String.Empty

            ' Dinesh.k: 12/04/2013 :BT:1687:Retirement batch estimate giving error when no beneficiaries defined.
            If Not String.IsNullOrEmpty(InputBeneficiaryType) Then
                If InputBeneficiaryType = BeneficiaryType.Manual Or InputBeneficiaryType = BeneficiaryType.NonSpouse Then
                    dtbeneficiaryBirthDate = Convert.ToDateTime(beneficiaryBirthDate)
                End If
            End If

            If dtbeneficiaryBirthDate <> String.Empty Then
                Dim ageDiff As Int16
                ageDiff = RetirementBOClass.GetRetireeAndBeneficiaryAgeDiff(retireeBirthDate, beneficiaryBirthDate, retirementDate)
                'START : ML | 2020.02.06 |YRS-AT-4783 | J&S Annuities would be displayed as "N/A" based on different Condition
                'RetirementBOClass.AvailJandSAnnuityOptionsForPrintEstimate(ageDiff, lcAnnuitiesListCursor)
                'dtFinalAnnuity = RetirementBOClass.AvailJandSAnnuityOptions(ageDiff, dtFinalAnnuity)
                Dim chronicallyIll As Boolean
                Dim secureActApplicable As Boolean
                chronicallyIll = False
                secureActApplicable = YMCARET.YmcaBusinessObject.RetirementBOClass.IsSecureActApplicable(Convert.ToDateTime(retireeBirthDate), dtbeneficiaryBirthDate, Me.BeneRelationShip.ToString().Trim, Convert.ToDateTime(retirementDate), chronicallyIll)
                RetirementBOClass.AvailJandSAnnuityOptionsForPrintEstimate(ageDiff, lcAnnuitiesListCursor, secureActApplicable)
                dtFinalAnnuity = RetirementBOClass.AvailJandSAnnuityOptions(ageDiff, dtFinalAnnuity, secureActApplicable)
                'END : ML | 2020.02.06 |YRS-AT-4783 | J&S Annuities would be displayed as "N/A" based on different Condition
            End If

            'End, SR:2012.11.15 : YRS 5.0-1329-J&S options available to non-spouse beneficiaries

            Select Case PlanType
                Case "R"
                    'printMsg = "Annuity Estimate based on Retirement Plan only."//commented by Anudeep on 22-sep for BT-1126
                    printMsg = getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_ANNUTITY_RETIREMENT")
                Case "S"
                    'printMsg = "Annuity Estimate based on Savings Plan only."//commented by Anudeep on 22-sep for BT-1126
                    printMsg = getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_ANNUTITY_SAVINGS")
                Case "B"
                    'printMsg = "Annuity Estimate based on both Retirement Plan and Savings Plan."//commented by Anudeep on 22-sep for BT-1126
                    printMsg = getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_ANNUTITY_BOTH")
                Case Else
            End Select


            If AreAnyAnnuitiesNA(dtFinalAnnuity) Then
                'printMsg += " Due to the age difference between you and the Survivor you have selected, this option is not available."//commented by Anudeep on 22-sep for BT-1126
                'START : ML |2020.02.06 | YRS-AT-4783 | YRS enhancement-modify Batch Estimates for Secure Act (TrackIt - 41132)
                'printMsg += getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_DIFFERENCE_YOU_SURVIVOR")
                printMsg += " " & vbCrLf & vbCrLf & YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_BATCH_ESTIMATE_JSTXT_CHRONICALY_NOT_ILL).DisplayText
                'END : ML |2020.02.06 | YRS-AT-4783 | YRS enhancement-modify Batch Estimates for Secure Act (TrackIt - 41132)
            End If
            'Start: Bala: 2/2/2016: YRS-AT-2677: Assumption is taken based on the restricted age form user assumption textbox.
            'printMsg += " " + TextBoxPraAssumption.Text
            If (IsDeathBenefitAnnuityPurchaseRestricted(Convert.ToDateTime(retireeBirthDate)) = False) Then
                printMsg += " " + TextBoxPraAssumption.Text
            Else
                printMsg += " " + TextBoxPraAssumptionForNonEligible.Text
            End If
            'End: Bala: 2/2/2016: YRS-AT-2677: Assumption is taken based on the restricted age form user assumption textbox.
            '****************Declaration******************************************
            Dim AgeRetired As Integer = Convert.ToDecimal(DateDiff(DateInterval.Day, Convert.ToDateTime(retireeBirthDate), Convert.ToDateTime(retirementDate)) / 365).ToString.Substring(0, 2)
            Dim BenBirthDate As String = beneficiaryBirthDate
            Dim BeneficiaryName As String = BeneficiaryFullName
            Dim PRAAssumption As String = printMsg
            Dim RetBirthDate As String = retireeBirthDate
            Dim RetiredDate As String = retirementDate
            Dim RetiredDeathBen As Decimal = fullDeathBenifitAmount
            Dim sslIncrease As Decimal = 0
            Dim C_Insured, C_Monthly, C_Reduction, C62_Insured, C62_Monthly, C62_Reduction, CS_Insured _
             , CS_Monthly, CS_Reduction, guiUniqueID, J1_Retiree, J1_Survivor, J162_Retiree, J162_Survivor _
             , J1I_Retiree, J1I_Survivor, J1P_Retiree, J1P_Survivor, J1P62_Retiree, J1P62_Survivor, J1PS_Retiree _
             , J1PS_Survivor, J1S_Retiree, J1S_Survivor, J5_Retiree, J5_Survivor, J562_Retiree, J562_Survivor _
             , J5I_Retiree, J5I_Survivor, J5L_Retiree, J5L_Survivor, J5L62_Retiree, J5L62_Survivor, J5LS_Retiree _
             , J5LS_Survivor, J5P_Retiree, J5P_Survivor, J5P62_Retiree, J5P62_Survivor, J5PS_Retiree, J5PS_Survivor _
             , J5S_Retiree, J5S_Survivor, J7_Retiree, J7_Survivor, J762_Retiree, J762_Survivor, J7I_Retiree _
             , J7I_Survivor, J7L_Retiree, J7L_Survivor, J7L62_Retiree, J7L62_Survivor, J7LS_Retiree, J7LS_Survivor _
             , J7P_Retiree, J7P_Survivor, J7P62_Retiree, J7P62_Survivor, J7PS_Retiree, J7PS_Survivor, J7S_Retiree _
             , J7S_Survivor, M_Retiree, M62_Retiree, MI_Retiree, MS_Retiree, ZC_Annually, ZC62_Annually _
             , ZCS_Annually, ZJ1_Retiree, ZJ1_Survivor, ZJ162_Retiree, ZJ162_Survivor, ZJ1I_Retiree, ZJ1I_Survivor _
             , ZJ1P_Retiree, ZJ1P_Survivor, ZJ1P62_Retiree, ZJ1P62_Survivor, ZJ1PS_Retiree, ZJ1PS_Survivor, ZJ1S_Retiree _
             , ZJ1S_Survivor, ZJ5_Retiree, ZJ5_Survivor, ZJ562_Retiree, ZJ562_Survivor, ZJ5I_Retiree, ZJ5I_Survivor _
             , ZJ5L_Retiree, ZJ5L_Survivor, ZJ5L62_Retiree, ZJ5L62_Survivor, ZJ5LS_Retiree, ZJ5LS_Survivor, ZJ5P_Retiree _
             , ZJ5P_Survivor, ZJ5P62_Retiree, zJ5P62_Survivor, ZJ5PS_Retiree, ZJ5PS_Survivor, ZJ5S_Retiree, ZJ5S_Survivor _
             , ZJ7_Retiree, ZJ7_Survivor, ZJ762_Retiree, ZJ762_Survivor, ZJ7I_Retiree, ZJ7I_Survivor, ZJ7L_Retiree _
             , ZJ7L_Survivor, zJ7L62_Retiree, ZJ7L62_Survivor, ZJ7LS_Retiree, ZJ7LS_Survivor, ZJ7P_Retiree, zJ7P_Survivor _
             , ZJ7P62_Retiree, zJ7P62_Survivor, ZJ7PS_Retiree, zJ7PS_Survivor, ZJ7S_Retiree, ZJ7S_Survivor, ZM_Retiree _
             , ZM62_Retiree, ZMI_Retiree, ZMS_Retiree, lnInsuredReserve As Decimal

            Dim i As Integer
            For i = 0 To lcAnnuitiesListCursor.Rows.Count - 1
                If Not lcAnnuitiesListCursor.Rows(i).Item("mnySSIncrease") = 0 Then
                    sslIncrease = Convert.ToDecimal(Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnySSIncrease")), 2))
                End If
            Next

            If Not dtAccountsBasisByProjection.Rows.Count = 0 Then
                If PlanType = "B" Then
                    lnInsuredReserve = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(mnyBalance)", "")), 2)
                ElseIf PlanType = "R" Then
                    lnInsuredReserve = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(mnyBalance)", "chvPlanType='RETIREMENT'")), 2)
                Else
                    lnInsuredReserve = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(mnyBalance)", "chvPlanType='SAVINGS'")), 2)
                End If
            End If

            For i = 0 To lcAnnuitiesListCursor.Rows.Count - 1

                ''''C
                If lcAnnuitiesListCursor.Rows(i).Item("chrAnnuityType").ToString.Trim.ToUpper = "C" And Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")) <> 0 Then
                    C_Insured = lnInsuredReserve
                    C_Monthly = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("mnyCurrentPayment")), 2)
                    C_Reduction = Math.Round(Convert.ToDecimal(lcAnnuitiesListCursor.Rows(i).Item("AnnuityWithoutRDB")), 2)
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
            ZJ1S_Survivor = 12 * J1S_Survivor
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

            Dim strGUID As String

            strGUID = RetirementBOClass.InsertPRA_ReportValues(PersonId, AgeRetired, _
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
                         Convert.ToDecimal("0.00"), _
                         sslIncrease, _
                         deathBenifitPercentAmount, _
             Convert.ToDecimal(deathBenifitPercentageToUSe), FundNo _
             , Math.Round((projFinalYearSal * 12), 2) _
             )


            Return strGUID
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "PrintEstimation", ex)
        End Try
    End Function

    Private Function GetPipeSeperated(ByVal lstGuids As ArrayList) As String
        Dim sbuilder As New StringBuilder()

        For Each item In lstGuids
            sbuilder.Append("|" & item)
        Next
        If (sbuilder.Length > 1) Then
            sbuilder.Remove(0, 1)
        End If
        Return sbuilder.ToString()
    End Function
    Private Sub PrintReport()
        Dim popupScript As String
        popupScript = "<script language='javascript'>openReportPrinter()</script>"
        If (Not Me.IsStartupScriptRegistered("PopupPrintColor")) Then
            Page.RegisterStartupScript("PopupPrintColor", popupScript)
        End If
    End Sub
    Private Sub PopulateShowListData(ByVal SessionDataSetExistingSaved As DataSet)
        Dim pagingOn As Boolean
        Try
            If HelperFunctions.isNonEmpty(SessionDataSetExistingSaved) Then

                pagingOn = SessionDataSetExistingSaved.Tables(0).Rows.Count > 500

                'If Me.DataGridFindInfo.CurrentPageIndex >= Me.DataGridFindInfo.PageCount And Me.DataGridFindInfo.PageCount <> 0 Then Exit Sub
                If Me.gvFindInfo.PageIndex >= Me.gvFindInfo.PageCount And Me.gvFindInfo.PageCount <> 0 Then Exit Sub

                If (SessionDataSetExistingSaved.Tables(0).Rows.Count > 0) Then

                    'LabelNoDataFound.Visible = False
                    Me.gvFindInfo.Visible = True
                    Me.ButtonExecute.Enabled = True
                    'HARSHALA-2012.03.15 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : START
                    'If pagingOn Then
                    '    dgPager.Visible = True
                    '    DataGridFindInfo.AllowPaging = True
                    'Else
                    '    dgPager.Visible = False
                    '    DataGridFindInfo.AllowPaging = False
                    'End If

                    'If pagingOn Then
                    '    DataGridFindInfo.AllowPaging = False
                    '    DataGridFindInfo.AllowPaging = True
                    '    DataGridFindInfo.CurrentPageIndex = 0
                    '    'DataGridFindInfo.PageSize = 15
                    '    dgPager.Grid = DataGridFindInfo
                    '    dgPager.PagesToDisplay = 10
                    '    dgPager.Visible = True

                    'End If
                    'HARSHALA-2012.03.15 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : END
                Else
                    'dgPager.Visible = False
                    Me.ButtonExecute.Enabled = False
                End If

                Me.gvFindInfo.SelectedIndex = -1
                Me.gvFindInfo.DataSource = SessionDataSetExistingSaved
                Me.gvFindInfo.DataBind()
                'gvFindInfo.Columns(RET_ADD_IMAGE).Visible = False
            Else
                'LabelNoDataFound.Visible = True
                Me.gvFindInfo.Visible = False
                'dgPager.Visible = False
                'Me.ButtonExecute.Enabled = False
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "PopulateShowListData", ex)
        End Try
    End Sub
    Private Function GetProjectedAcctBalances(ByRef errorMessage As String, ByRef businessLogic As RetirementBOClass, ByVal FundEventId As String, ByVal PersRetirementDate As String, ByVal PersBirthDate As String, ByVal PersonId As String, ByVal SSNO As String, ByVal FundEventStatus As String, ByVal PlanType As String, ByVal ProjectedYearInterest As Double, ByRef combinedDataset As DataSet) As Boolean
        Dim dsRetEstimateEmployment As DataSet
        Dim dtNonGroupedElectiveAcct As DataTable
        Dim dtNonGroupedProjectedAcctWiseBalances As DataTable
        Dim dtGroupedProjectedBalances As DataTable
        Dim dtGroupedElectiveAcct As DataTable
        Dim boolReturn As Boolean = True
        Dim dtPersonEmploymentDet As DataTable
        Try
            ''True parameter for ignoring validation message
            boolReturn = ValidateEstimate(errorMessage, FundEventId, PersRetirementDate, PersonId, SSNO, FundEventStatus, PlanType)
            errorMessage = combinemessage(errorMessage)
            If (boolReturn = False) Then
                Return boolReturn
            End If

            'Get Employment Details
            dsRetEstimateEmployment = RetirementBOClass.SearchRetEmpInfo(FundEventId, "NORMAL", "01/01/1900") 'MMR | 2017.03.09 | YRS-AT-2625 | Addeed "NORMAL" retire type and default date for retirement as parameters need to be passed

            If Not dsRetEstimateEmployment Is Nothing Then

                Dim drActiveEmployment As DataRow() = dsRetEstimateEmployment.Tables(0).Select("dtmTerminationDate is Null")
                'Create copy of active Employment
                dtPersonEmploymentDet = CreateNewActiveSalaryInformation(dsRetEstimateEmployment)
                ' 2012.02.09	Shashank Patel      Made changes to remove add button & parameter screen
                'Dim dtmfuturesaleffDate As String = String.Empty
                'If (DropDownListFutureSalaryEffMon.SelectedValue > 0) Then
                '    dtmfuturesaleffDate = GetDateByMonth(DropDownListFutureSalaryEffMon.SelectedValue)
                'End If
                ' 2012.02.09	Shashank Patel      Made changes to remove add button & parameter screen

                UpdateActiveSalaryInformation(dtPersonEmploymentDet, drActiveEmployment, DropDownListSalaryPercentage.SelectedValue, TextBoxFutureSalaryEffDate.Text)

                ' Get the correct elective account details as per the Plan type selected by the user.
                Dim dataSetElectiveAccounts As DataSet
                dataSetElectiveAccounts = GetElectiveAccounts(errorMessage, PlanType, FundEventId, PersonId, PersRetirementDate)

                'Step 1. Calculate the Account Balances
                Dim hasNoErrors As Boolean = True

                Dim warningMessage As String = String.Empty
                Dim isYmcaLegacyAcctTotalExceed As Boolean = False
                Dim isYmcaAcctTotalExceed As Boolean = False
                Dim strMaxEmpTerminationDate As String

                Dim excludedDataTable As DataTable = New DataTable
                Dim employmentDetails As DataTable = dtPersonEmploymentDet
                'get max employe termination date
                strMaxEmpTerminationDate = GetTerminationDateForProjBalValidation(PersRetirementDate, employmentDetails, dsRetEstimateEmployment.Tables(0))

                hasNoErrors = businessLogic.CalculateAccountBalancesAndProjections(dsRetEstimateEmployment _
                  , PersBirthDate, PersRetirementDate _
                  , PersonId, FundEventId, "NORMAL" _
                  , ProjectedYearInterest, dataSetElectiveAccounts _
                  , combinedDataset, True, PlanType _
                  , FundEventStatus, errorMessage, warningMessage, excludedDataTable, employmentDetails, True _
                  , strMaxEmpTerminationDate, isYmcaLegacyAcctTotalExceed, isYmcaAcctTotalExceed)
                'Check if any error has been reported.  
                If Not hasNoErrors And (errorMessage <> "R" And errorMessage <> "S") Then
                    boolReturn = False
                    errorMessage = errorMessage & getmessage(errorMessage)
                End If

                errorMessage = errorMessage & warningMessage

            End If
            Return boolReturn
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "GetProjectedAccBalance", ex)
            Throw ex
        End Try
    End Function

#End Region

#Region "Controls Events"
    Private Sub tabStripRetirementEstimate_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabStripRetirementEstimate.SelectedIndexChange
        Try
            Me.MultiPageRetirementEstimate.SelectedIndex = Me.tabStripRetirementEstimate.SelectedIndex


            If (Me.tabStripRetirementEstimate.SelectedIndex = 1) Then
                Me.PopulateParameterGridData()
                ButtonSave.Visible = True
                ButtonExecute.Enabled = True
                ButtonRemoveList.Enabled = HelperFunctions.isNonEmpty(SessionDataSetExistingSaved)
                ButtonAddList.Enabled = False
                ButtonClearList.Visible = True
                ButtonShowList.Visible = True
            ElseIf (Me.tabStripRetirementEstimate.SelectedIndex = 0) Then
                ButtonSave.Visible = False
                ButtonExecute.Enabled = False
                'HARSHALA-2012.03.22 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
                'ButtonRemoveList.Enabled = False
                ButtonRemoveList.Enabled = True
                'HARSHALA-2012.03.22 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : END
                ButtonAddList.Enabled = HelperFunctions.isNonEmpty(SessionDataSetdsMemberInfo)
                ButtonClearList.Visible = False
                ButtonShowList.Visible = False
            ElseIf (Me.tabStripRetirementEstimate.SelectedIndex = 2) Then
                ButtonSave.Visible = False
                ButtonExecute.Enabled = False
                ButtonRemoveList.Enabled = False
                ButtonAddList.Enabled = False
                ButtonClearList.Visible = False
                ButtonShowList.Visible = False
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "tabStripRetirementEstimate_SelectedIndexChange", ex)
        End Try
    End Sub

    Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Try
            'ISSearchMode = True
            ISSearchByYmcaNo = False
            'ButtonSave.Enabled = False
            Me.ButtonAddList.Enabled = True
            ViewState_SortExpression = Nothing
            PopulateFindData(False, LoadDatasetMode.Table)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "ButtonFind_Click", ex)
        End Try
    End Sub

    Private Sub ButtonImportAssociation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonImportAssociation.Click
        Try
            'ISSearchMode = True
            ISSearchByYmcaNo = True
            Me.ButtonAddList.Enabled = True
            'ButtonSave.Enabled = False
            ViewState_SortExpression = Nothing
            PopulateFindData(True, LoadDatasetMode.Table)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "ButtonImportAssociation_Click", ex)
        End Try
    End Sub

    Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            Me.TextBoxFirstName.Text = String.Empty
            Me.TextboxFundNo.Text = String.Empty
            Me.TextBoxLastName.Text = String.Empty
            Me.TextBoxSSNo.Text = String.Empty
            Me.TextBoxAssociation.Text = String.Empty
            ISSearchByYmcaNo = False
            ViewState_SortExpression = Nothing
            'added
            'If ISSearchMode Then
            gvFindInfo.DataSource = Nothing
            gvFindInfo.DataBind()
            ' End If
            'ISSearchMode = False
            'dgPager.Visible = False
            'end
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "ButtonClear_Click", ex)
        End Try
    End Sub

    Private Sub BindExceptionGrid(ByVal dtException As DataTable)
        HelperFunctions.BindGrid(gvExceptions, dtException.DefaultView, True)
    End Sub

    'Private Sub ButtonEmptyTable_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonEmptyTable.Click
    '    If (GetSelectedDatagridPersonInfoPerseID().Count > 1) Then
    '        MessageBox.Show(PlaceHolder1, "Empty Table", "Do you want to delete estimated Save Data ?", MessageBoxButtons.YesNo, False)
    '        Session("DeleteRecords") = True
    '    Else
    '        MessageBox.Show(PlaceHolder1, "Empty Table", "Please select at least one record to delete", MessageBoxButtons.Stop, False)
    '        Session("DeleteRecords") = False
    '    End If

    '    'Me.EmptyBatchEstimateParameterTable(Convert.ToInt32(Session("LoggedUserKey")))
    'End Sub

    Private Sub DisplayJQueryMessage(ByVal strMessage As String)
        If (strMessage <> String.Empty) Then
            Dim alertMessageJS As String = "<script language='javascript'>$(document).ready(function() { alertMessage('" & strMessage & "'); })</script>"
            Page.RegisterClientScriptBlock("MEssageBox", alertMessageJS)
        End If
    End Sub
    'HARSHALA-2012.03.05 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
    Private Sub InfoJqueryMessage(ByVal strMessage As String)
        Dim alertMessageJS As String = "<script language='javascript'>$(document).ready(function() { alert('" & strMessage & "'); })</script>"
        Page.RegisterClientScriptBlock("MessageBox", alertMessageJS)
    End Sub
    'HARSHALA-2012.03.05 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : END

    Private Sub ButtonRemoveList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRemoveList.Click
        Dim lstFundEventIds As New List(Of String)
        Try
            lstFundEventIds = GetSelectedDatagridParameterFundEventid()

            If lstFundEventIds.Count = 0 Then
                'Me.DisplayJQueryMessage(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_REMOVE_LIST"))
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_REMOVE_LIST"), EnumMessageTypes.Error)
                'MessageBox.Show(PlaceHolder1, "Remove List", getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_REMOVE_LIST, MessageBoxButtons.Stop, False)
            Else
                Me.DeleteBatchEstimateTable(lstFundEventIds)
                Me.PopulateBatchEstimateData(SessionUserID, LoadDatasetMode.Session)
                'Me.DisplayJQueryMessage(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_REMOVE_LIST_SUCCESS"))
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_REMOVE_LIST_SUCCESS"), EnumMessageTypes.Success)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "ButtonRemoveList_Click", ex)
        End Try
    End Sub

    'Private Sub ButtonRefreshGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRefreshGrid.Click
    '    ISSearchMode = False
    '    'Me.PopulateBatchEstimateData(Convert.ToInt32(Session("LoggedUserKey")), LoadDatasetMode.Table)
    '    'Dim dsPersonInfo As DataSet
    '    'dsPersonInfo = GetMergeDataset(SessionDataSetExistingSaved, SessionDataSetAddtoList)
    '    'DataGridFindInfo.DataBind()
    '    Me.PopulateShowListData(SessionDataSetExistingSaved)
    'End Sub
    Private Sub ButtonShowList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonShowList.Click
        Try
            Me.PopulateBatchEstimateData(SessionUserID, LoadDatasetMode.Table)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "ButtonShowList_Click", ex)
        End Try
    End Sub

    'Private Sub ButtonSaveParamater_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSaveParamater.Click
    '    Page.Validate()
    '    If Page.IsValid Then
    '        Me.AddBatchEstimateParameter()
    '        Dim alertMessageJS As String = "<script language='javascript'>$(document).ready(function() { alertMessage('" & getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_SAVE_PARAMETER & "'); })</script>"
    '        Page.RegisterClientScriptBlock("MEssageBox", alertMessageJS)
    '    End If
    'End Sub

    Private Sub ButtonExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonExecute.Click
        Try
            Page.Validate()
            Dim dsPersonalInfo As DataSet
            If Page.IsValid Then
                dsPersonalInfo = SessionDataSetExistingSaved
                'End :sp 2012.02.09
                If HelperFunctions.isNonEmpty(dsPersonalInfo) Then
                    ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "PopupScriptCancel", "ShowConfirmDialog();", True)
                End If
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "ButtonExecute_Click", ex)
        End Try
    End Sub

    Private Sub ButtonAddList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAddList.Click
        'SP :Commented on 28 feb 2012 - for adding to list
        'Try
        '    If HelperFunctions.isNonEmpty(SessionDataSetdsMemberInfo) Then
        '        If HelperFunctions.isEmpty(SessionDataSetExistingSaved) Then
        '            SessionDataSetExistingSaved = SessionDataSetdsMemberInfo.Clone()
        '        End If
        '        Dim bExist As Boolean = False
        '        If (HelperFunctions.isNonEmpty(SessionDataSetExistingSaved)) Then
        '            Dim i As Integer = 0
        '            Dim dscopySessionDataSetExistingSaved As DataSet
        '            Dim dritem As DataRow
        '            dscopySessionDataSetExistingSaved = SessionDataSetExistingSaved.Copy()
        '            For Each drParticipant As DataRow In SessionDataSetdsMemberInfo.Tables(0).Rows
        '                bExist = dscopySessionDataSetExistingSaved.Tables(0).Select("FundEventId ='" & drParticipant("FundEventId").ToString() & "'").Count > 0
        '                If Not (bExist) Then
        '                    SessionDataSetExistingSaved.Tables(0).ImportRow(SessionDataSetdsMemberInfo.Tables(0).Select("FundEventId ='" & drParticipant("FundEventId").ToString() & "'")(0))
        '                End If

        '            Next

        '        Else
        '            SessionDataSetExistingSaved = SessionDataSetdsMemberInfo.Copy()
        '            SessionDataSetdsMemberInfo = Nothing
        '            'SessionDataSetdsMemberInfo.AcceptChanges()
        '            Me.PopulateFindData(isImportYmcano:=ISSearchByYmcaNo, parameterload:=LoadDatasetMode.Session)
        '        End If
        '        SessionDataSetExistingSaved.AcceptChanges()

        '        Dim alertMessageJS As String = "<script language='javascript'>$(document).ready(function() { alertMessage(' " & Resources.RetirementBatchEstimateMessages.MESSAGE_RETIREMENT_BATCH_ESTIMATE_ADD_LIST_SUCCESS & "'); })</script>"
        '        Page.RegisterClientScriptBlock("MEssageBox", alertMessageJS)
        '    End If
        'Catch ex As Exception
        '    HelperFunctions.LogException("ButtonAddList_Click", ex)
        '    Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)

        'End Try
        ' 'SP :Commented on 28 feb 2012 - for adding to list -end


        Dim lstfundEventId As List(Of String)
        lstfundEventId = GetSelectedDatagridPersonInfoPerseID()
        If (lstfundEventId.Count = 0) Then
            'MessageBox.Show(PlaceHolder1, "Add to List", getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_ADD_LIST"), MessageBoxButtons.Stop, False)
            HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_ADD_LIST"), EnumMessageTypes.Error)
        Else

            'Me.ButtonSave.Enabled = True

            Try
                If HelperFunctions.isEmpty(SessionDataSetExistingSaved) Then
                    SessionDataSetExistingSaved = SessionDataSetdsMemberInfo.Clone()
                End If
                Dim bExist As Boolean = False
                If (HelperFunctions.isNonEmpty(SessionDataSetExistingSaved)) Then
                    Dim i As Integer = 0
                    Dim dscopySessionDataSetExistingSaved As DataSet
                    Dim dritem As DataRow
                    dscopySessionDataSetExistingSaved = SessionDataSetExistingSaved.Copy()
                    For Each item In lstfundEventId
                        bExist = dscopySessionDataSetExistingSaved.Tables(0).Select("FundEventId ='" & item & "'").Count > 0

                        If Not (bExist) Then
                            SessionDataSetExistingSaved.Tables(0).ImportRow(SessionDataSetdsMemberInfo.Tables(0).Select("FundEventId ='" & item & "'")(0))
                        End If

                    Next

                Else

                    For Each item As String In lstfundEventId
                        SessionDataSetExistingSaved.Tables(0).ImportRow(SessionDataSetdsMemberInfo.Tables(0).Select("FundEventId ='" & item & "'")(0))
                    Next

                End If
                SessionDataSetExistingSaved.AcceptChanges()
            Catch ex As Exception
                HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
                HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "ButtonAddList_Click", ex)
            End Try


            'Me.DisplayJQueryMessage(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_ADD_LIST_SUCCESS"))
            HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_ADD_LIST_SUCCESS"), EnumMessageTypes.Success)
        End If

        'Me.AddToList(lstfundEventId)
    End Sub
    Private Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        Try
            ' Me.ButtonAddList.Enabled = False
            Page.Validate()
            If Page.IsValid Then
                Dim lstFunEventIds As New List(Of String)
                lstFunEventIds = GetSelectedFundEventsIDs()
                If (lstFunEventIds.Count > 0) Then
                    Me.AddToList(lstFunEventIds) 'Adding to database all records
                    'SP-2012.07.23 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
                    'HARSHALA-2012.03.05 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
                    'HiddenFieldSavedListCount.Value = Convert.ToString(lstFunEventIds.Count)
                    'HARSHALA-2012.03.05 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
                    HiddenFieldSavedListCount.Value = Convert.ToString(RetirementBatchEstimateBOClass.GetRetirementBatchListCount(SessionUserID))
                    'SP-2012.07.23 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : END
                    'Me.DisplayJQueryMessage(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_SAVE_LIST"))
                    HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_SAVE_LIST"), EnumMessageTypes.Success)
                End If
            End If

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "ButtonSave_Click", ex)
        End Try

    End Sub
    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            ClearSession()
            Me.Page.Dispose()
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "ButtonCancel_Click", ex)
        End Try
        'Server.Transfer("MainWebForm.aspx", False)
    End Sub
    Private Sub ButtonClearList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClearList.Click
        Try
            Me.ClearSavedList()
            'HARSHALA-2012.03.05 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
            HiddenFieldSavedListCount.Value = "0"
            'HARSHALA-2012.03.05 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : END
            'HARSHALA-2012.03.22 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
            Me.ButtonRemoveList.Enabled = True
            'HARSHALA-2012.03.22 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : END
            'Me.DisplayJQueryMessage(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_CLEAR_LIST"))
            HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_CLEAR_LIST"), EnumMessageTypes.Success)
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "ButtonClearList_Click", ex)
        End Try
    End Sub


    Private Sub gvParameter_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvParameter.RowDataBound
        Try
            HelperFunctions.SetSortingArrows(ParameterSortState, e)

            If e.Row.RowType = ListItemType.Header Or e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
                e.Row.Cells(PARAM_EMP_FUND_EVENT_ID).Attributes.Add("style", "display:none") 'FundEventId
                e.Row.Cells(PARAM_PERSID).Attributes.Add("style", "display:none") 'PersId
                'e.Row.Cells(PARAM_FUND_STATUS).Attributes.Add("style", "display:none") 'FundStatus
                e.Row.Cells(PARAM_EMP_BIRTH_DATE).Attributes.Add("style", "display:none") 'BirthDate
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "gvParameter_RowDataBound", ex)
        End Try
    End Sub


    ' 2012.02.09	Shashank Patel      Made changes to remove add button & parameter screen
    'Private Sub DataGridFindInfo_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridFindInfo.ItemCommand
    '    If e.CommandName.ToUpper = "ADD" Then
    '        Dim strFundEventId As String = CType(e.Item.Cells(RET_EMP_FUND_EVENT_ID).Text, String)
    '        Try
    '            If HelperFunctions.isEmpty(SessionDataSetExistingSaved) Then
    '                SessionDataSetExistingSaved = SessionDataSetdsMemberInfo.Clone()
    '            End If
    '            Dim bExist As Boolean = False
    '            If Not (String.IsNullOrEmpty(strFundEventId)) Then

    '                If (HelperFunctions.isNonEmpty(SessionDataSetExistingSaved)) Then
    '                    Dim i As Integer = 0
    '                    Dim dscopySessionDataSetExistingSaved As DataSet
    '                    Dim dritem As DataRow
    '                    dscopySessionDataSetExistingSaved = SessionDataSetExistingSaved.Copy()

    '                    bExist = dscopySessionDataSetExistingSaved.Tables(0).Select("FundEventId ='" & strFundEventId & "'").Count > 0
    '                    If Not (bExist) Then
    '                        SessionDataSetExistingSaved.Tables(0).ImportRow(SessionDataSetdsMemberInfo.Tables(0).Select("FundEventId ='" & strFundEventId & "'")(0))
    '                    End If

    '                Else
    '                    SessionDataSetExistingSaved.Tables(0).ImportRow(SessionDataSetdsMemberInfo.Tables(0).Select("FundEventId ='" & strFundEventId & "'")(0))
    '                End If
    '                SessionDataSetExistingSaved.AcceptChanges()
    '                'SP: 2012.02.27 - Added cahnges to remove added item from gridfindinfo
    '                Me.SessionDataSetdsMemberInfo.Tables(0).Rows.Remove(SessionDataSetdsMemberInfo.Tables(0).Select("FundEventId ='" & strFundEventId & "'")(0))
    '                Me.SessionDataSetdsMemberInfo.AcceptChanges()
    '                Me.PopulateFindData(isImportYmcano:=ISSearchByYmcaNo, parameterload:=LoadDatasetMode.Session)
    '                'SP: 2012.02.27 - Added cahnges to remove added item from gridfindinfo -End
    '                'Dim alertMessageJS As String = "<script language='javascript'>$(document).ready(function() { alertMessage(' " & Resources.RetirementBatchEstimateMessages.MESSAGE_RETIREMENT_BATCH_ESTIMATE_ADD_LIST_SUCCESS & "'); })</script>"
    '                'Page.RegisterClientScriptBlock("MEssageBox", alertMessageJS)
    '            End If
    '        Catch ex As Exception
    '            HelperFunctions.LogException("ButtonAddList_Click", ex)
    '            Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '        End Try

    '    End If
    'End Sub

    ' 2012.02.09	Shashank Patel      Made changes to remove add button & parameter screen
    'Private Sub ButtonClearList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClearList.Click
    '    ViewState_SortExpression = Nothing
    '    ISSearchByYmcaNo = False
    '    If ISSearchMode Then
    '        DataGridFindInfo.DataSource = Nothing
    '        DataGridFindInfo.DataBind()
    '    End If
    '    ISSearchMode = False
    '    dgPager.Visible = False
    'End Sub

    'Private Sub DataGridFindInfo_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridFindInfo.ItemDataBound
    '    'e.Item.Cells(RET_EMP_FUND_EVENT_ID).Visible = False
    '    'e.Item.Cells(RET_PERSID).Visible = False
    '    'e.Item.Cells(RET_FUND_STATUS).Visible = False
    '    ''e.Item.Cells(RET_FUND_IDNO).Visible = False
    '    'e.Item.Cells(RET_EMP_BIRTH_DATE).Visible = False
    'End Sub




    ' 2012.02.09	Shashank Patel      Made changes to remove add button & parameter screen
    'Private Sub DgPagerParam_PageChanged(ByVal PgNumber As Integer) Handles DgPagerParam.PageChanged
    '    Me.PopulateParameterGridData()
    'End Sub


    'Private Sub DataGridParameter_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridParameter.ItemCommand
    '    If e.CommandName.ToUpper = "REMOVE" Then
    '        Dim strFundEventId As String = CType(e.Item.Cells(0).Text, String)
    '        If Not (String.IsNullOrEmpty(strFundEventId)) Then
    '            Try
    '                Dim lstFundEventId As New List(Of String)
    '                lstFundEventId.Add(strFundEventId)
    '                Dim xmlGuiFundEventId As String = CreateFundEventIdXml(lstFundEventId)
    '                RetirementBatchEstimateBOClass.DeleteBatchEstimateByUserID(SessionUserID, xmlGuiFundEventId) 'Deleting from database
    '                Me.DeleteBatchEstimateTable(lstFundEventId:=lstFundEventId) 'Deleting from temporary session
    '                'Dim alertMessageJS As String = "<script language='javascript'>$(document).ready(function() { alertMessage('" & Resources.RetirementBatchEstimateMessages.MESSAGE_RETIREMENT_BATCH_ESTIMATE_REMOVE_LIST_SUCCESS & "'); })</script>"
    '                'Page.RegisterClientScriptBlock("MEssageBox", alertMessageJS)
    '                Me.PopulateParameterGridData()
    '            Catch ex As Exception
    '                HelperFunctions.LogException("EmptyBatchEstimateParameterTable", ex)
    '                Server.Transfer("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '            End Try
    '        End If
    '    End If

    'End Sub
    'Private Sub DataGridParameter_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridParameter.ItemDataBound
    '    'e.Item.Cells(PARAM_EMP_FUND_EVENT_ID).Visible = False
    '    'e.Item.Cells(PARAM_PERSID).Visible = False
    '    'e.Item.Cells(PARAM_FUND_STATUS).Visible = False
    '    ''e.Item.Cells(RET_FUND_IDNO).Visible = False
    '    'e.Item.Cells(PARAM_EMP_BIRTH_DATE).Visible = False
    'End Sub

    Private Sub gvParameter_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvParameter.Sorting
        Dim dv As New DataView

        Dim SortExpression As String
        SortExpression = e.SortExpression
        Try

            If (HelperFunctions.isNonEmpty(SessionDataSetExistingSaved)) Then
                dv = DirectCast(SessionDataSetExistingSaved, DataSet).Tables(0).DefaultView
            Else
                dv = Nothing
            End If

            If dv IsNot Nothing Then

                dv.Sort = SortExpression

                HelperFunctions.gvSorting(ParameterSortState, e.SortExpression, dv)
                HelperFunctions.BindGrid(gvParameter, dv, True)

            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "gvParameter_Sorting", ex)
        End Try
    End Sub

    Private Sub gvFindInfo_OnRowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFindInfo.RowDataBound
        'Dinesh Kanojia(DK):2013.01.30:BT-1262: YRS 5.0-1697: Message to display if multiple active employments Exist.
        Dim iCount As Integer
        Dim chkSelect As CheckBox
        Dim lblcount As Label
        Dim drv As DataRowView = DirectCast(e.Row.DataItem, DataRowView)
        Try
            HelperFunctions.SetSortingArrows(FindPersonSortState, e)

            'If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If e.Row.RowType = ListItemType.Header Or e.Row.RowType = ListItemType.Item Or e.Row.RowType = ListItemType.AlternatingItem Then
                e.Row.Cells(RET_EMP_FUND_EVENT_ID).Attributes.Add("style", "display:none") 'FundEventId
                e.Row.Cells(RET_PERSID).Attributes.Add("style", "display:none") 'PersId
                'e.Row.Cells(RET_FUND_STATUS).Attributes.Add("style", "display:none") 'FundStatus
                e.Row.Cells(RET_EMP_BIRTH_DATE).Attributes.Add("style", "display:none") 'BirthDate
                e.Row.Cells(RET_COUNTS).Attributes.Add("style", "display:none") 'Counts 
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                iCount = Convert.ToInt32(e.Row.Cells(RET_COUNTS).Text)
                'iCount = drv("Counts")

                If (iCount > 1) Then

                    chkSelect = e.Row.FindControl("chkSelect")
                    chkSelect.Enabled = False

                    If strBackgroundcolor = Nothing Then
                        strBackgroundcolor = Drawing.Color.FromName(System.Configuration.ConfigurationSettings.AppSettings("RetirementBatchestimate_Color"))
                    End If

                    e.Row.BackColor = strBackgroundcolor
                    tblShowNotes.Visible = True
                    lblNotes.Visible = True
                    lblNotes.Text = getmessage("MESSAGE_REIREMENT_ESTIMATE_MULTIPLE_ACTIVE_EMPLOYMENTS_EXISTS")
                End If
            End If

            ''HARSHALA-2012.03.15 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : START
            'If e.Row.RowIndex > gvFindInfo.PageSize Then
            '    e.Row.Visible = False
            '    lbl_Search_MoreItems.Visible = True
            '    'lbl_Search_MoreItems.Text = "Results truncated. Showing only " + DataGridFindInfo.PageSize.ToString() + " rows out of " + SessionDataSetdsMemberInfo.Tables(0).DefaultView.Count.ToString()//commented by Anudeep on 22-sep for BT-1126
            '    lbl_Search_MoreItems.Text = String.Format(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_RESULTS_TRUNCATED"), gvFindInfo.PageSize.ToString(), SessionDataSetdsMemberInfo.Tables(0).DefaultView.Count.ToString())
            'End If


            'HARSHALA-2012.03.15 : BT ID-1009 Application does not show pagination after clicking on "Add To List" button : END
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "gvFindInfo_OnRowDataBound", ex)
        End Try
    End Sub
    'Private Sub DataGridFindInfo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridFindInfo.SelectedIndexChanged

    'End Sub
    ' 2012.02.09	Shashank Patel      Made changes to remove add button & parameter screen


    Private Sub gvFindInfo_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvFindInfo.Sorting
        Dim dv As New DataView

        Dim SortExpression As String
        SortExpression = e.SortExpression
        Try
            'If (ISSearchMode) Then

            'If (HelperFunctions.isNonEmpty(SessionDataSetdsMemberInfo)) Then
            '    dv = DirectCast(SessionDataSetdsMemberInfo, DataSet).Tables(0).DefaultView
            'Else
            '    dv = Nothing
            'End If

            'Else
            '    If (HelperFunctions.isNonEmpty(SessionDataSetExistingSaved)) Then
            '        dv = DirectCast(SessionDataSetExistingSaved, DataSet).Tables(0).DefaultView
            '    Else
            '        dv = Nothing
            '    End If

            'End If

            'If Not dv Is Nothing Then
            '    Dim SortExpression As String
            '    SortExpression = e.SortExpression
            '    'dv = DirectCast(SessionDataSetdsMemberInfo, DataSet).Tables(0).DefaultView
            '    dv.Sort = SortExpression
            '    If Not ViewState_SortExpression Is Nothing Then
            '        If ViewState_SortExpression.ToString.Trim.EndsWith("ASC") Then
            '            dv.Sort = SortExpression + " DESC"
            '        Else
            '            dv.Sort = SortExpression + " ASC"
            '        End If
            '    Else
            '        dv.Sort = SortExpression + " ASC"
            '    End If
            '    Me.gvFindInfo.DataSource = Nothing
            '    Me.gvFindInfo.DataSource = dv
            '    Me.gvFindInfo.DataBind()
            '    ViewState_SortExpression = dv.Sort
            'End If

            dv = DirectCast(SessionDataSetdsMemberInfo, DataSet).Tables(0).DefaultView
            dv.Sort = SortExpression

            HelperFunctions.gvSorting(FindPersonSortState, e.SortExpression, dv)
            HelperFunctions.BindGrid(gvFindInfo, dv, True)

        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "gvFindInfo_Sorting", ex)
        End Try
    End Sub
#End Region


    '2012.09.21 added by anudeep :start
    '2012.09.21 added by anudeep :start
    'gets the message from resource file
    Public Function getmessage(ByVal resourcemessage As String)
        Try

            Dim strMessage As String
            Try
                'Added By SG: 2013.01.10:
                'strMessage = GetGlobalResourceObject("RetirementBatchEstimateMessages", resourcemessage).ToString()
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

    Private Sub gvExceptions_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvExceptions.PageIndexChanging
        Try
            If e.NewPageIndex >= 0 Then
                Me.FindPersonPageIndex = e.NewPageIndex
                gvExceptions.PageIndex = e.NewPageIndex
                BindExceptionGrid(ViewStatedtException)
            End If
        Catch ex As Exception
            HelperFunctions.ShowMessageToUser(ex.Message, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_gvExceptions_PageIndexChanging", ex)
        End Try
    End Sub

    Private Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        Dim dtException As New DataTable
        Dim dsPersonalInfo As DataSet
        dtException.Columns.Add("SSN", GetType(String))
        dtException.Columns.Add("FundNo", GetType(String))
        dtException.Columns.Add("FirstName", GetType(String))
        dtException.Columns.Add("LastName", GetType(String))
        dtException.Columns.Add("Reason", GetType(String))
        Try

            'HARSHALA-2012.03.05 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
            'tabStripRetirementEstimate.SelectedIndex = 2
            'HARSHALA-2012.03.05 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : END

            'Getting all the record of estimated person by user 
            ' 2012.02.09	Shashank Patel      Made changes to remove add button & parameter screen -
            'dsPersonalInfo = RetirementBatchEstimateBOClass.SearchBatchEstimatePerson(SessionUserID)
            dsPersonalInfo = SessionDataSetExistingSaved
            'End :sp 2012.02.09
            If HelperFunctions.isNonEmpty(dsPersonalInfo) Then
                'HARSHALA-2012.03.05 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START

                'HARSHALA-2012.03.05 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : END
                FilterAgeRecord(dsPersonalInfo, dtException, DropDownListAge.SelectedValue)
                CalculateRetriementDate(dsPersonalInfo, Convert.ToInt32(DropDownListAge.SelectedValue))
                Dim businessLogic As New RetirementBOClass()

                Session("GUID") = Me.BatchEstimate(businessLogic:=businessLogic, dsPersonalInfo:=dsPersonalInfo, dtException:=dtException)

                If dtException.Rows.Count <> 0 Then
                    tabStripRetirementEstimate.SelectedIndex = 2
                    tabStripRetirementEstimate_SelectedIndexChange(sender, e)
                End If

                If (Not Session("GUID") Is Nothing AndAlso Session("GUID").ToString().Length > 1) Then
                    '2012.02.29    Shashank Patel      Made changes to remove save paramater functionality & remove selcted fundevents ids parameter from delete paraticipant
                    If (rdFormToPrint.SelectedValue <> String.Empty) Then
                        Session("strReportName") = rdFormToPrint.SelectedValue
                        Me.PrintReport()
                    End If

                End If
                HelperFunctions.ShowMessageToUser(getmessage("MESSAGE_RETIREMENT_BATCH_ESTIMATE_SUCCESFULLY"), EnumMessageTypes.Success)
                'HARSHALA-2012.03.05 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : START
            Else
                'Me.InfoJqueryMessage(Resources.RetirementBatchEstimateMessages.MESSAGE_RETIREMENT_BATCH_ESTIMATE_NO_ITEM)
                'HARSHALA-2012.03.05 : BT ID-1007 BATCH ESTIMATE OBSERVATIONS : END
            End If
        Catch ex As Exception
            Dim exmsg As String

            If (ex.Message.ToString().Contains("MESSAGE")) Then
                exmsg = getmessage(ex.Message.ToString())
            Else
                exmsg = ex.Message.Trim.ToString()
            End If
            HelperFunctions.ShowMessageToUser(exmsg, EnumMessageTypes.Error)
            HelperFunctions.LogException("RetirementEstimateBatchWebForm_" + "btnYes_Click", ex)
        End Try
    End Sub
End Class