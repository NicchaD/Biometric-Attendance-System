'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	DeathBenefitsCalculatorForm.aspx.vb
' Author Name		:	Vipul Patel 
' Employee ID		:	32900 .... 
' Email				:	vipul.patel@3i-infotech.com
' Contact No		:	55928738
' Creation Time		:	10/05/2005 
' Program Specification Name	:	YMCA PS 3.13.1
' Unit Test Plan Name			:	
' Description					:	This form is used to View & store Death calculations
'*******************************************************************************
' Cache-Session     :   Vipul 02Feb06
'*******************************************************************************
'Changed By:preeti On:10thFeb06 IssueId:YRST-2092
'*******************************************************************************
'****************************************************
'Modification History
'****************************************************
'Modified by        Date                Description
'****************************************************
'Aparna Samala      09/04/2007          YREN -3271
'Nikunj Patel       2007.07.18          Incorporating changes for Plan Split into the code
'Nikunj Patel       2007.07.27          Updated code to show more specific messages when settlement options are not available for a plan
'Nikunj Patel       2007.08.07          Updated code to display TD Savings plan label when calculate button is clicked and the label was previously hidden.
'Nikunj Patel       2007.08.14          Updated code to hide extra columns from the Find person list
'Nikunj Patel       2007.08.16          Updated code to obtain the Annuity Types based on a Fund Event
'Nikunj Patel       2007.08.31          Updated code to allow 11 characters in the SSN field
'Nikunj Patel       2007.08.29          Updating code to properly handle the display of the selected image in radio buttons. This only worked partially. Updated code again on 2007.09.13
'Nikunj Patel       2007.09.03          Updating code to handle cases where beneficiaries of type RETIRE or MEMEBER have not been defined. Process does not continue further.
'                                       Updated code to delink Active money calculations from the retired money calculations.
'Nikunj Patel       2007.09.06          Updated code to prevent default selection of Benefit options
'                                       Updated code to enable Reports and Forms button by default and add validation code to ensure options are selected when these buttons are clicked
'                                       PlanType column has been hidden from view from the benefit options grid
'Nikunj Patel       2007.09.10          Code added to block calculations for status RA/RT/DD before 1-July-2006
'                                       Added code to avoid the display of Message boxes and use labels instead
'Nikunj Patel       2007.09.13          Updated code to ensure the correct selection of radio buttons
'                   2007.09.14          Updated code to prevent Thread being aborted type of errors
'                   2007.09.17          Updated code to allow sorting for the search control
'Nikunj Patel       2007.09.27          Updated code to clear the Fund Id number when clicking clear button
'Aparna Samala      2007-12-13          YRPS -4121
'Nikunj Patel       2007.12.14          Changing code to send parameters to the Form only when required. Setting the other parameters to string.empty
'Nikunj Patel       2007.12.18          Changing code to call the report viewer for retired participants and also fixed condition when the forms button is displayed
'Aparna Samala      2007.12.27          modified the processforms logic
'Nikunj Patel       2008.04.09          Changes related to YMCA Phase IV - Part 1
'                   2008.04.11          Changes related to handling of ML status
'                   2008.04.16          BT-386 - Handling issue where upon clicking Find and selecting a details the message for ML was not being prompted.
'                   2008.04.18          BT-392 - Disabling fund selection for PENP participants
'                   2008.05.29          Adding status RPT to the list since it can contain both retired and non-retired funds
'                   2008.07.09          YRS-5.0-464 - Handling NP status similar to ML status where users are prompted to identify if death is active or inactive. Changed ML so that the question is only asked if balance is less than $10000 in basic accounts
'Hafiz Rehman       2008.07.23          YRS-5.0-464 - Reverting changes for reopened issue.
'Nikunj Patel       2008.08.06          Changes related to YMCA Phase IV - Part 2
'                   2008.08.21          Changed the enabling of the Calculate Button
'Priya Jawale       2008.09.19          If the status is "DQ", "RQ" and "RQTA" then we cannot perform calculations before 1-July-2008
'Nikunj Patel       2008.09.29          Report button should be disabled if the beneficiary options are not available
'Nikunj Patel       2008.11.06          YRS-5.0-464 - Reverting changes made for NP as per request in the issue. NP goes to DI.
'Nikunj Patel       2008.11.19          Making changes to limit the number of search results displayed on the screen
'Nikunj Patel       2009.04.20          Making use of common HelperFunctions class for checking isNonEmpty and isEmpty
'Neeraj Singh       12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Sanjay Rawat       2009.12.17          YRS 5.0-973 - Removed MBW related code from the page
'Shashi Shekhar     12/Feb/2010         Restrict Data Archived Participants To proceed in Find list 
'Shashi Shekhar     11/Mar/2010         Allow the user to access certain functions only for Retired participants (status RD) even if they are archived (Ref:Handling usability issue of Data Archive)
'Shashi Shekhar     08/April/2010       Allow the user to access certain functions only for Retired participants (status RD and DR) even if they are archived (Ref:Handling usability issue of Data Archive)
'Sanjay R.          2010.05.14          YRS 5.0 1077
'Sanjay R.          2010.06.15          Enhancement changes(CType to DirectCast)
'Sanjay R.          2010.07.12          Code Review changes(Region,variable declarations etc.)
'Imran              2010.11.24          YRS 5.0-1209 / BT-673- Pass new fund id parameter for RetireeDeathBenefitsClaimForm and Pre-RetirementDeathBenefitsClaimForm report
'Shashi Shekhar:    23-Dec-2010         For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
'Shashi Shekhar     28 Feb 2011         Replacing Header formating with user control (YRS 5.0-450 )
'Sanjay R.		    2011.04.26          YRS 5.0-1292: A warning msg should appear for the participant whose annuity disursement after moth of death is not voided									
'Bhavna             2011.12.13			YRS 5.0-1350:SR acct not moved over if death before 7/1/2006
'Sanjay R.          2012.10.29          YRS 5.0-1707:New Death Benefit Application form 
'Anudeep A.         2012.12.04          YRS 5.0-1707:New Death Benefit Application form for jquery popup
'Anudeep A.         2012.12.13          YRS 5.0-1707: Populate beneficiaries for RA,RT,DD
'Anudeep A.         2012.12.14          YRS 5.0-1707: In Form Pop-up mandatory checkbox should be selected always.
'Sanjay R.          2012.12.14          YRS 5.0-1707: Populate JS Annuities and clear session data
'Anudeep A.         2012.12.17          YRS 5.0-1707: As per observations UI changes Done ( As to show header retired and active money and on find click not to show more items label)
'Anudeep            2013.03.25          Bt-1303-YRS 5.0-1707:New Death Benefit Application form -To display Death benfit options if annity beneficiary is selected
'Sanjay R.          2013.06.26          BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
'Anudeep A          2013.08.12          Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
'Anudeep A          2013.10.01          BT-2225:Changes in Death caluculator in viewing forms list
'Anudeep A          2014.08.12          BT:2460:YRS 5.0-2331 - Death Benefit Application Form 
'Shashank P         2014.12.04          BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN
'Manthan Rajguru    2015.09.16          YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Anudeep A          2015.09.28          YRS-AT-2548:YRS problem with Death Benefit Application - blank benefit type and amount (TrackIT 23250)
'Anudeep A          2015.10.09          YRS-AT-2553 - YRS: Death Benefit Application-amounts s/b based value on Date of Death (TrackIT 23289)
'Anudeep A          2015.10.09          YRS-AT-2478 - Death Benefit Application to show taxable and non-taxable amounts(TrackIT 21695)
'Anudeep A          2015.10.15          YRS-AT-2553 - YRS: Death Benefit Application-amounts s/b based value on Date of Death (TrackIT 23289)
'Anudeep A          2015.10.21          YRS-AT-2548:YRS problem with Death Benefit Application - blank benefit type and amount (TrackIT 23250)
'Anudeep A          2015.10.23          YRS-AT-2478 - Death Benefit Application to show taxable and non-taxable amounts(TrackIT 21695)
'Anudeep A          2015.10.26          YRS-AT-2478 - Death Benefit Application to show taxable and non-taxable amounts(TrackIT 21695)
'Sanjay R.           2015.12.15         YRS-AT-2718 - YRS enh: Annuity Estimate - change for Retired Death Benefit, use new effective date 1/1/2019 for calculations 
'Manthan Rajguru    2016.05.31          YRS-AT-3003 -  YRS bug: problem with Death Benefit Application-adding RDB to Retirement Plan pre-retired benefit (TrackIt 26441) 
'Chandra Sekar      2016.08.05          YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
'Santosh Bura       2017.06.19          YRS-AT-3371 -  YRS bug: Death Settlement before 7/2007 - Amount formatting glitch causes Error: Thread being aborted (TrackIT 29291)
'Dharmesh CB        2018-11-21          YRS-AT-3837 - YRS Enh: Death Benefit Application for RDB rule change NEEDED by DECEMBER 2018 (TrackIT 32497)
'Manthan R          2020.02.10          YRS-AT-4770 -  Death Calculator must restrict annuity options under SECURE Act (TrackIT-41080)
'*****************************************************
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.SessionState
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports YMCAUI.SessionManager.SessionDeathCalc

Public Class DeathBenefitsCalculatorForm
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("DeathBenefitsCalculatorForm.aspx")
    'End issue id YRS 5.0-940

#Region "Local Variables"
    Dim l_dataset_SearchResults As DataSet      'Store search results information of the search that was performed
    Dim l_dataset_Beneficiaries As DataSet      'Store any active beneficiaries of the deceased participant
    Dim l_dataset_SettlementOption_RetirementPlan As DataSet  'Store Retirement Plan Settlement Options for Active Beneficiaries
    Dim l_dataset_SettlementOption_SavingsPlan As DataSet     'Store Savings Plan Settlement Options for Active Beneficiaries
    Dim l_string_ForceMLComputationAs As String 'Store the selection of the user for treating ML as either DA or DI. Value must be reset when a new participant is selected.
    Dim l_string_PageOperation As PAGEOPERATIONS = PAGEOPERATIONS.NORMAL    'Tracks the current page operation. Used when messageboxes are prompted to the user.
    Dim l_int_SelectedDataGridItem As Integer   'Tracks the selected index of the Search Datagrid
    Dim l_SSNumber As String
    Dim strBeneFirstName As String
    Dim strBeneLastName As String

#End Region

#Region "Enum" 'AA:1.10.2015 YRS-AT-2548 : Added a region Enum 
    Private Enum PAGEOPERATIONS
        NORMAL = 0
        PROMPT_FOR_10K_BENEFIT = 1
    End Enum
    'Start: AA:1.10.2015 YRS-AT-2548 : Added Enum for datagrid cell indexes 
    Private Enum DataGrid_Search_ForAll_index
        Select_Image
        SSNO
        LastName
        FirstName
        MiddleName
        FundDesc
        FundEventId
        StatusType
        DeathDate
        IsArchived
        FundIdNo
        BirthDate
    End Enum

    Private Enum DataGrid_BeneficiariesList_ForAll_index
        PersiD
        Select_Image
        SSN
        FirstName
        LastName
        RelationShip
        Member
        Saving
        Retire
        Insres
        DueSince
        Re_GenerateForm
        VIEW_PRINT_FORM
    End Enum
    Private Enum DataGridAnnuities_index
        Select_Image
        SSN
        FirstName
        LastName
        Monthly_ANN_Amount
        Annuity_Source
        Annuity_Type
        Plan_Type
        ID
        AnnuityJointSurvivorsID
        DatDueSince
        Re_GenerateForm
        VIEW_PRINT_FORM
    End Enum
    Private Enum DataGrid_Retired_money_PlanOptions_index
        Select_Image
        PMT
        Lump_Sum
        Annuity_M
        Annuity_C
        Reserves
        Death_Benefit
        SSN
        'Start:AA:10.12.2015 YRS_AT-2478 Added to hide the columns of pretax,post tax and ymcapretax in grid 
        PersonalPreTax
        PersonalPostTax
        YMCAPreTax
        'End:AA:10.12.2015 YRS_AT-2478 Added to hide the columns of pretax,post tax and ymcapretax in grid 
        PlanType
        Status
        BeneficiaryTypeCode 'Chandra Sekar| 2016.08.05| YRS-AT-3027 Added to hide the column of BeneficiaryTypeCode in grid  
    End Enum
    Private Enum DataGrid_Active_money_PlanOptions_index
        Select_Image
        PMT
        Lump_Sum
        Annuity_M
        Annuity_C
        Basic_Reserves
        Voluantry
        Death_Benefit
        TD_Savings
        MB_Account
        Total
        SSN
        'Start:AA:10.12.2015 YRS_AT-2478 Added to hide the columns of pretax,post tax and ymcapretax in grid 
        PersonalPreTax
        PersonalPostTax
        YMCAPreTax
        'End:AA:10.12.2015 YRS_AT-2478 Added to hide the columns of pretax,post tax and ymcapretax in grid 
        PlanType
        Status
    End Enum
    'End: AA:1.10.2015 YRS-AT-2548 : Added Enum for datagrid cell indexes 

    ' START: SB | YRS-AT-3371 | 2017.06.19 | Added Enum for dataGrid used where in case where death date is before 7/1/2006
    Private Enum DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate
        Select_Image = 1
        PMT = 2
        Lump_Sum = 3
        Annuity_M = 4
        Annuity_C = 5
        Reserves = 6
        Death_Benefit = 7
        PIA = 8
        Total = 9
        SSN = 10
        PersonalPostTax = 11
        PersonalPreTax = 12
        YMCAPreTax = 13
        PlanType = 14
        Status = 15
        BeneficiaryTYpeCode = 16
    End Enum
    ' END: SB | YRS-AT-3371 | 2017.06.19 | Added Enum for dataGrid used where in case where death date is before 7/1/2006

#End Region 'AA:1.10.2015 YRS-AT-2548 : Added a region Enum 

#Region "Local Constants"
    Const SELECTED_IMAGE_BUTTON_URL = "images\selected.gif"
    Const NORMAL_IMAGE_BUTTON_URL = "images\select.gif"
    Const ACTIVE_FUNDS_INDEX = 1
    Const RETIRED_FUNDS_INDEX = 0
    Const LINEBREAK = "<BR />" & vbCrLf
#End Region

#Region "FormProperties"
    Private Property String_PersId() As String
        'Preserve the Pers ID for Reports
        Get
            If Not Session("String_PersId") Is Nothing Then
                Return Session("String_PersId")
            End If

        End Get
        Set(ByVal Value As String)
            Session("String_PersId") = Value
        End Set
    End Property
    Private Property DataSet_ProcessedData() As DataSet
        Get
            ''Converting AppDomain to Session  07-Oct-05
            'If Not System.AppDomain.CurrentDomain.GetData("DataSet_LookUpMemberListforDeath") Is Nothing Then
            '    Return System.AppDomain.CurrentDomain.GetData("DataSet_ProcessedData")          ''viewstate("l_dataset_DeathCalc_ProcessedData") = l_dataset_DeathCalc_ProcessedData 
            'Else
            '    Return Nothing
            'End If
            If Not Session("DataSet_ProcessedData") Is Nothing Then
                Return Session("DataSet_ProcessedData")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            'System.AppDomain.CurrentDomain.SetData("DataSet_ProcessedData", Value)          ''viewstate("l_dataset_DeathCalc_ProcessedData") = l_dataset_DeathCalc_ProcessedData 
            Session("DataSet_ProcessedData") = Value
        End Set
    End Property
    ''SR:2012.11.22: YRS 5.0-1707 - Added 
    Private Property DataSet_SelectedData() As DataSet
        Get
            If Not Session("DataSet_SelectedData") Is Nothing Then
                Return Session("DataSet_SelectedData")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("DataSet_SelectedData") = Value
        End Set
    End Property
    ''End, SR:2012.11.22: YRS 5.0-1707 - Added 
    'Start:Anudeep A:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for jquery popup
    Private Property DataSet_AdditionalForms() As DataSet
        Get
            If Not Session("DataSet_AdditionalForms") Is Nothing Then
                Return Session("DataSet_AdditionalForms")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("DataSet_AdditionalForms") = Value
        End Set
    End Property
    'End:Anudeep A:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for jquery popup
    'Start:AA:10.21.2015 YRS-AT-2548: Added to capture fundevent id to avoid indexing error while reloading page
    Private Property String_FundeventId() As String
        Get
            If Not ViewState("String_FundeventId") Is Nothing Then
                Return ViewState("String_FundeventId")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("String_FundeventId") = Value
        End Set
    End Property
    'End:AA:10.21.2015 YRS-AT-2548: Added to capture fundevent id to avoid indexing error while reloading page
    ' START: SB | YRS-AT-3371 | 2017.06.19 | Added to capture death date used for displaying records whose death date is before "7/1/2006"
    Private Property ParticipantDeathDate() As String
        Get
            If Not ViewState("ParticipantDeathDate") Is Nothing Then
                Return ViewState("ParticipantDeathDate")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("ParticipantDeathDate") = Value
        End Set
    End Property
    ' END: SB | YRS-AT-3371 | 2017.06.19 | Added to capture death date used for displaying records whose death date is before "7/1/2006"

    ' START: MMR | YRS-AT-4770 | 2020.02.10 | Added to capture participant birth date which will be used in secured act rule logic
    Private Property ParticipantBirthDate() As Date
        Get
            If Not ViewState("ParticipantBirthDate") Is Nothing Then
                Return DirectCast(ViewState("ParticipantBirthDate"), Date)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As Date)
            ViewState("ParticipantBirthDate") = Value
        End Set
    End Property
    ' END: MMR | YRS-AT-4770 | 2020.02.10 | Added to capture participant birth date which will be used in secured act rule logic
#End Region

#Region "EnumMaxlength"
    Public Enum EnumMaxlength
        SSNo = 11   'NP:PS:2007.08.31 - Changing value from 9 to 11
        FirstName = 20
        LastName = 30
    End Enum
#End Region

    'Protected WithEvents rbtnList_MoneySelection As System.Web.UI.WebControls.RadioButtonList ''SR:2012.12.11:YRS 5.0-1707 - commented
    'Protected WithEvents pnlMoneySelection As System.Web.UI.WebControls.Panel ''SR:2012.12.11:YRS 5.0-1707 - commented
    'Protected WithEvents lbl_RetirementPlanOptions As System.Web.UI.WebControls.Label
    'Protected WithEvents lbl_SavingsPlanOptions As System.Web.UI.WebControls.Label
    Protected WithEvents RegularExpressionValidator1 As System.Web.UI.WebControls.RegularExpressionValidator
    Protected WithEvents TextBoxFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblErr_BeneficiarySettled As System.Web.UI.WebControls.Label
    Protected WithEvents lblErr_BeneficiariesNotDefinedProperly As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Search_MoreItems As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Beneficiaries_All As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid_BeneficiariesList_ForAll As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGrid_RetirementPlan_BenefitOptions_ForRetired As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGrid_SavingsPlan_BenefitOptions_ForRetired As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGrid_RetirementPlan_BenefitOptions_ForActive As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGrid_SavingsPlan_BenefitOptions_ForActive As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridAnnuities As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lbl_RetirementPlanOptions_ForRetired As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_RetirementPlanOptions_ForActive As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_SavingsPlanOptions_ForRetired As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_SavingsPlanOptions_ForActive As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_JSBeneficiaries As System.Web.UI.WebControls.Label
    ' This variable is used to keep track of which option to display to the user
    ' This comes into the picture when we are handling the display of options for
    ' RA, DD and RT type of participants who have both active money and retired money
    ' If showRetiredMoneyOptions is true then Active money options are not shown and vice-versa
    Protected WithEvents lblInformation As System.Web.UI.WebControls.Label ' SR | 2015.12.01 | YRS-AT-2718 | Add label to display informative messages.

    Dim _showRetiredMoneyOptions As Boolean = False

#Region " Web Form Designer Generated Code "


    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    '    Protected WithEvents DataGrid_Search As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGrid_Search As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TabStripDeathCalc As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents MultiPageDeathCalc As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents LabelFormHead As System.Web.UI.WebControls.Label
    'Protected WithEvents LabelSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxSSNo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxLastName As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelDataBaseDeath As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxDataBaseDeathDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelDeathCalculations As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxCalcDeathDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxCalcDeathDate As YMCAUI.DateUserControl
    Protected WithEvents Menu1 As skmMenu.Menu
    Protected WithEvents ButtonFind As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCalculate As System.Web.UI.WebControls.Button
    'Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up
    Protected WithEvents ButtonForms As System.Web.UI.HtmlControls.HtmlInputButton
    'Anudeep A.2013.01.07 - code commented below for YRS 5.0-1707:New Death Benefit Application form
    'Protected WithEvents RadioButtonMonthly As System.web.UI.WebControls.RadioButton
    Protected WithEvents RadioButtonAnnual As System.Web.UI.WebControls.RadioButton
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents ButtonClear As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonSelect As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonCancel As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonReports As System.Web.UI.WebControls.Button

    Protected WithEvents ButtonOk As System.Web.UI.WebControls.Button
    Protected WithEvents LabelNoDataFound As System.Web.UI.WebControls.Label
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    Protected WithEvents tbForms As System.Web.UI.HtmlControls.HtmlTable
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            HelperFunctions.LogMessage("Begin Page Load event")
            If Session("LoggedUserKey") Is Nothing Then
                Response.Redirect("Login.aspx", False)
            End If
            Menu1.DataSource = Server.MapPath("SimpleXML.xml")
            Menu1.DataBind()
            If IsPostBack AndAlso HelperFunctions.isNonEmpty(l_dataset_SearchResults) Then
                DataGrid_Search.DataSource = l_dataset_SearchResults.Tables(0).DefaultView
                DataGrid_Search.SelectedIndex = l_int_SelectedDataGridItem
                DataGrid_Search.DataBind()
            End If
            Me.LabelDataBaseDeath.AssociatedControlID = Me.TextBoxDataBaseDeathDate.ID

            'Enable/Disable the items not required
            Me.ButtonSelect.Visible = False
            Me.TextBoxSSNo.MaxLength = EnumMaxlength.SSNo
            Me.TextBoxFirstName.MaxLength = EnumMaxlength.FirstName
            Me.TextBoxLastName.MaxLength = EnumMaxlength.LastName

            If Not Me.IsPostBack Then
                Session("DC_l_dataset_SearchResults") = Nothing
                Session("DC_l_dataset_Beneficiaries") = Nothing
                Session("DC_l_dataset_SettlementOption_RetirementPlan") = Nothing
                Session("DC_l_dataset_SettlementOption_SavingsPlan") = Nothing
                DataSet_AdditionalForms = Nothing
                Session("DeathCalc_Sort") = Nothing
                'Session("Change_FundStatus") = Nothing 'SR:2012.12.08 - YRS 5.0-1707:Added
                ' Me.TextBoxCalcDeathDate.Enabled = True
                Me.ButtonReports.Enabled = True
                'Anudeep A:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for jquery popup Buttonforms changed from Asp button to html button
                Me.ButtonForms.Disabled = True
                Me.TabStripDeathCalc.Items(1).Enabled = False
                TextBoxDataBaseDeathDate.ReadOnly = True
                'pnlMoneySelection.Visible = False
                Me.SavingsPlanStartDate = GetSavingsPlanStartDate()  'SB | YRS-AT-3371 | 2017.06.27 | on page load set Date before plan split in viewstate 
            Else
                'SR:2012.10.07-YRS 5.0-1707: Commented belo lines of code
                'NP:PS:2007.06.15 - Repopulate the variable for _showRetiredMoneyOptions from the radio button
                'If rbtnList_MoneySelection.SelectedValue = "Retired" Then _showRetiredMoneyOptions = True
                'If rbtnList_MoneySelection.SelectedValue = "Active" Then _showRetiredMoneyOptions = False
                'End, SR:2012.10.07-YRS 5.0-1707: Commented belo lines of code

                If Request.Form("OK") = "OK" Then
                    BindGrid(DataGrid_Search, l_dataset_SearchResults)
                End If
                'NP:IVP1:2008.04.11 - Continue computations after prompting for Active employment status prompt for ML
                If l_string_PageOperation = PAGEOPERATIONS.PROMPT_FOR_10K_BENEFIT Then
                    l_string_PageOperation = PAGEOPERATIONS.NORMAL
                    If Request.Form("Yes") = "Yes" Then
                        l_string_ForceMLComputationAs = "DA"
                    ElseIf Request.Form("No") = "No" Then
                        l_string_ForceMLComputationAs = "DI"
                    Else
                        MessageBox.Show(PlaceHolder1, "Death Calculations", "Unexpected value was obtained from Message Prompt. Please report this error.", MessageBoxButtons.OK)
                        Exit Sub
                    End If
                    Process_DeathBenefitCalculations()
                End If
                'Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up 
                If Session("ProcessData") = "Yes" Then
                    SaveFormDetails()
                End If
                AdditionalForms()
                'Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up
            End If
            HelperFunctions.LogMessage("End Page Load event")
        Catch ex As SqlException
            HelperFunctions.LogException("Load Report", ex)
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            Session("ProcessData") = Nothing
        Catch ex As Exception
            HelperFunctions.LogException("Load Report", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            'Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up 
            Session("ProcessData") = Nothing
        End Try
    End Sub

    Private Sub TabStripDeathCalc_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabStripDeathCalc.SelectedIndexChange
        Try
            If Me.TabStripDeathCalc.SelectedIndex > -1 Then
                HelperFunctions.LogMessage("Bgin Tab strip change event")
                Me.MultiPageDeathCalc.SelectedIndex = Me.TabStripDeathCalc.SelectedIndex
                If Me.TabStripDeathCalc.SelectedIndex = 0 Then
                    If HelperFunctions.isNonEmpty(l_dataset_SearchResults) Then
                        BindGrid(DataGrid_Search, l_dataset_SearchResults.Tables(0).DefaultView)
                    Else
                        BindGrid(DataGrid_Search, l_dataset_SearchResults)
                    End If
                    ClearSession() 'SR:2012.12.14:YRS 5..0-1707 : Clear Session data
                    'Anudeep clear the datagrids while selecting on another record
                    DataGrid_BeneficiariesList_ForAll.DataSource = Nothing
                    DataGrid_BeneficiariesList_ForAll.DataBind()
                    DataGrid_RetirementPlan_BenefitOptions_ForActive.DataSource = Nothing
                    DataGrid_RetirementPlan_BenefitOptions_ForActive.DataBind()
                    DataGrid_RetirementPlan_BenefitOptions_ForRetired.DataSource = Nothing
                    DataGrid_RetirementPlan_BenefitOptions_ForRetired.DataBind()
                    DataGrid_SavingsPlan_BenefitOptions_ForActive.DataSource = Nothing
                    DataGrid_SavingsPlan_BenefitOptions_ForActive.DataBind()
                    DataGrid_SavingsPlan_BenefitOptions_ForRetired.DataSource = Nothing
                    DataGrid_SavingsPlan_BenefitOptions_ForRetired.DataBind()
                    DataGridAnnuities.DataSource = Nothing
                    DataGridAnnuities.DataBind()
                Else
                    ' Perform calculations for the participant by simulating the click of the Calculate button
                    'NP:PS:2007.09.17 - Check if a decendent has been selected for calculation
                    If DataGrid_Search.SelectedIndex < 0 Then
                        MessageBox.Show(PlaceHolder1, "Error", "Please select a participant from the list", MessageBoxButtons.Stop)
                        Me.TabStripDeathCalc.SelectedIndex = 0
                        Me.MultiPageDeathCalc.SelectedIndex = 0
                        Exit Sub
                    End If

                    'Session("First_Calculation") = True ''SR:2012.12.10 - YRS 5.0-1707 - Added
                    'Session("Change_FundStatus") = Nothing 'SR:2012.12.08 - YRS 5.0-1707:Added
                    'rbtnList_MoneySelection.SelectedIndex = -1
                    ButtonCalculate_Click(Me, Nothing)
                    'Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up 
                    AdditionalForms()
                End If
                HelperFunctions.LogMessage("Completed Tab strip change event")
            End If
        Catch ex As SqlException
            If sender Is Me Then
                HelperFunctions.LogException("TabStripDeathCalc_SelectedIndexChange", ex)
                Throw ex
            Else
                Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            End If
        Catch ex As Exception
            If sender Is Me Then
                HelperFunctions.LogException("TabStripDeathCalc_SelectedIndexChange", ex)
                Throw ex
            Else
                HelperFunctions.LogException("TabStripDeathCalc_SelectedIndexChange", ex)
                Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            End If
        End Try
    End Sub

#Region "Beneficiary related code"
    Public Sub PopulateBeneficiariesGrid()
        Try
            HelperFunctions.LogMessage("Begin PopulateBeneficiariesGrid() method")
            Dim l_dataset_DeathCalc_ProcessedData As DataSet = DataSet_ProcessedData
            DataGrid_BeneficiariesList_ForAll.DataSource = l_dataset_DeathCalc_ProcessedData.Tables(0)
            If l_dataset_DeathCalc_ProcessedData.Tables(0).Rows.Count > 0 Then DataGrid_BeneficiariesList_ForAll.SelectedIndex = 0
            DataGrid_BeneficiariesList_ForAll.DataBind()
            lbl_Beneficiaries_All.Visible = True
            DataGrid_BeneficiariesList_ForAll.Visible = True
            DataGrid_BeneficiariesList_ForAll_SelectedIndexChanged(Me, Nothing)
            'LoadJSAnnuities()
            EnableDisableFormsButton()
            ButtonReports.Enabled = True
            HandleDisplay()
            HelperFunctions.LogMessage("Completed PopulateBeneficiariesGrid() method")
        Catch
            Throw
        End Try
    End Sub

    Public Sub Process_DeathBenefitCalculations()
        Dim l_dataset_DeathCalc_ProcessedData As DataSet
        Dim l_dataRowMemberList As DataRow
        Dim l_string_PersId As String, l_string_FundEventId As String
        Dim l_date_DeathDate As Date
        Dim l_int_ReturnStatus As Int16
        Dim l_string_FundStatus As String   'NP:PS:2007.09.03 - Adding Fund Status for overriding status in case of RA/RT/DD
        Dim l_string_ForceCalculationAs As String   'NP:IVP1:2008.04.11 - Adding new variable to track if ML are to be treated as DA/DI

        Try
            HelperFunctions.LogMessage("Begin Process_DeathBenefitCalculations() method")
            'start the process for calculations
            'Reinitialize Details Tab
            lblErr_BeneficiariesNotDefinedProperly.Text = ""
            lblErr_BeneficiariesNotDefinedProperly.Visible = False
            lblErr_BeneficiarySettled.Text = ""
            lblErr_BeneficiarySettled.Visible = False
            'BindGrid(DataGrid_BeneficiariesList_ForAll, CType(Nothing, DataSet))
            BindGrid(DataGrid_RetirementPlan_BenefitOptions_ForRetired, CType(Nothing, DataSet))
            BindGrid(DataGrid_SavingsPlan_BenefitOptions_ForRetired, CType(Nothing, DataSet))
            BindGrid(DataGrid_RetirementPlan_BenefitOptions_ForActive, CType(Nothing, DataSet))
            BindGrid(DataGrid_SavingsPlan_BenefitOptions_ForActive, CType(Nothing, DataSet))
            BindGrid(DataGridAnnuities, CType(Nothing, DataSet))
            lbl_RetirementPlanOptions_ForActive.Visible = False
            lbl_RetirementPlanOptions_ForRetired.Visible = False
            lbl_SavingsPlanOptions_ForActive.Visible = False
            lbl_SavingsPlanOptions_ForRetired.Visible = False
            lbl_JSBeneficiaries.Visible = False

            ButtonCalculate.Enabled = False
            'Anudeep A:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for jquery popup Buttonforms changed from Asp button to html button
            ButtonForms.Disabled = True
            ButtonReports.Enabled = True  'by aparna -YRPS -4180
            lbl_Beneficiaries_All.Visible = False

            ''Changed By:preeti On:10thFeb06 IssueId:YRST-2092 start
            'Dim arrDr() As DataRow = l_dataset_SearchResults.Tables("r_MemberListForDeath").Select("[SS No.]=" & Me.DataGrid_Search.SelectedItem.Cells(1).Text.Trim())
            'If arrDr.Length > 0 Then
            '    l_dataRowMemberList = arrDr(0)
            'Else
            '    Exit Sub
            'End If
            ''_dataRowMemberList = l_dataset_LookUp_MemberListForDeath.Tables("r_MemberListForDeath").Rows(Me.DataGrid_Search.SelectedIndex)
            ''Changed By:preeti On:10thFeb06 IssueId:YRST-2092 End

            'NP:PS:2007.08.17 - Obtaining the Selected row by another mechanism. The previous mechanism did not work as expected in the case of participant with multiple fund-events.
            l_dataRowMemberList = l_dataset_SearchResults.Tables("r_MemberListForDeath").DefaultView.Item(DataGrid_Search.SelectedIndex).Row

            If l_dataRowMemberList Is Nothing Then
                'If selection not made exit from here........this is Highly unlikely but still play safe
                Exit Sub
            End If

            l_string_PersId = l_dataRowMemberList("PersID").ToString().Trim()
            l_string_FundEventId = l_dataRowMemberList("FundEventID").ToString().Trim()
            'l_string_FundStatus = Trim(CType(l_dataRowMemberList("StatusType"), String))
            l_string_FundStatus = Trim(DirectCast(l_dataRowMemberList("StatusType"), String))
            'Session("FundStatus") = l_string_FundStatus
            If YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetMarketBasedAmount(l_string_FundEventId, l_string_FundStatus) <> 0 And (l_string_FundStatus = "DR" Or l_string_FundStatus = "RD") Then
                l_string_FundStatus = "DD"
            End If

            'Handling of Death Dates.
            ' If Death date is available from DB then use that else use the current date
            ' For calculation date if no date has been defined then use the death date which may
            ' be the actual death date for deceased participants or current date.
            If l_dataRowMemberList.IsNull("DeathDate") Then
                TextBoxDataBaseDeathDate.Text = DateTime.Now.Date
            Else
                TextBoxDataBaseDeathDate.Text = l_dataRowMemberList("DeathDate")
            End If
            If Trim(TextBoxCalcDeathDate.Text) = "" Then
                TextBoxCalcDeathDate.Text = TextBoxDataBaseDeathDate.Text
            End If

            'Assign the death date for calculations from the calculation death date
            l_date_DeathDate = CType(Me.TextBoxCalcDeathDate.Text, System.DateTime)
            Me.ParticipantDeathDate = l_date_DeathDate ' SB | YRS-AT-3371 | 2017.06.19 | Date is saved in viewstate,further used in displaying records as per death date 
            LoadJSAnnuities(l_string_FundEventId)


            Select Case l_string_FundStatus
                Case "RA", "RT", "DD"
                    'NP:PS:2007.09.10 - If the status is RA/RT/DD then we cannot perform calculations before 1-Jul-2006
                    'START: SB | YRS-AT-3371 | 2017.06.27 | Death date is compared based on the value from the database instead of hardcoded values
                    ' If DateTime.Parse(l_date_DeathDate) < DateTime.Parse("7/1/2006") Then
                    If DateTime.Parse(l_date_DeathDate) < Me.SavingsPlanStartDate Then
                        'END: SB | YRS-AT-3371 | 2017.06.27 | Death date is compared based on the value from the database instead of hardcoded values
                        MessageBox.Show(PlaceHolder1, "Invalid Operation", "Calculations cannot be performed for status RA/RT/DD before 1 July 2007.", MessageBoxButtons.Stop)
                        TextBoxCalcDeathDate.Text = ""
                        ButtonReports.Enabled = False   'NP:BT-649:2008.11.06 - Disabling Reports button when no calculations can be performed
                        Exit Sub
                    End If
                Case "RE", "RDNP", "RP", "ML", "PEML", "NP", "PE", "PT", "WP", "RPT", "DQ", "RQ", "RQTA"
                    'NP:IVP1:2008.04.09 - If the status is RE/RDNP then we cannot perform calculations before 1-Jul-2008
                    'NP:IVP1:2008.04.09 - If the status is RP then we cannot perform calculations before 1-Jul-2008
                    'NP:IVP1:2008.04.09 - If the status is ML/PEML/NP/PE/PT/WP then we cannot perform calculations before 1-Jul-2008
                    'NP:IVP1:2008.05.29 - If the status is RPT then we cannot perform calculations before 1-Jul-2008
                    'Priya Jawale :2008.09.19  -If the status is "DQ", "RQ" and "RQTA" then we cannot perform calculations before 1-July-2008
                    If DateTime.Parse(l_date_DeathDate) < DateTime.Parse("7/1/2008") Then
                        MessageBox.Show(PlaceHolder1, "Invalid Operation", "Calculations cannot be performed for status ML/PEML/NP/PE/PT/WP/RE/RDNP/RP before 1 March 2008.", MessageBoxButtons.Stop)
                        TextBoxCalcDeathDate.Text = ""
                        ButtonReports.Enabled = False   'NP:BT-649:2008.11.06 - Disabling Reports button when no calculations can be performed
                        Exit Sub
                    End If
            End Select

            'Identify if we need to force calculations for a particular fund status and if so then set the status accordingly
            Select Case l_string_FundStatus
                'NP:IVP1:2008.04.09 - Adding statuses RE and RDNP to the list since they can contain both funds
                'NP:IVP1:2008.05.29 - Adding status RPT to the list since it can contain both retired and non-retired funds
                Case "DD", "RA", "RT", "RE", "RDNP", "RPT"   'This could be anything. Decide on the basis of the value of the radio button

                    'If Session("First_Calculation") = True Then
                    '    PopulateBeneficiaries(l_string_PersId, l_string_FundEventId, l_string_FundStatus, l_date_DeathDate)
                    '    If DataGrid_BeneficiariesList_ForAll.SelectedIndex > -1 Then
                    '        If (String.IsNullOrEmpty(DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(8).Text.ToString.Trim()) Or DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(8).Text.ToString.Trim() = "&nbsp;") And (String.IsNullOrEmpty(DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(9).Text.ToString().Trim()) Or (DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(9).Text.ToString().Trim() = "&nbsp;")) Then
                    '            l_string_FundStatus = "DR"
                    '        Else
                    '            l_string_FundStatus = "DA"
                    '        End If
                    '    Else
                    '        l_string_FundStatus = ""
                    '    End If
                    '    Session("Change_FundStatus") = l_string_FundStatus
                    'Else
                    '    l_string_FundStatus = Session("Change_FundStatus")

                    'End If
                    l_string_FundStatus = "DD"


                    'NP:IVP1:2008.04.09 - Adding status RP to the list
                    'Priya Jawale :2008.09.19  - adding status "DQ", "RQ" and "RQTA" to the list
                Case "RD", "DR", "RP", "DQ", "RQ", "RQTA"             'Consider this as retired funds
                    ''SR:2010.01.06 For Market Based Withdrawal
                    'If YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetMarketBasedAmount(l_string_FundEventId, l_string_FundStatus) <> 0 Then
                    '    pnlMoneySelection.Enabled = True
                    '    If rbtnList_MoneySelection.SelectedIndex = ACTIVE_FUNDS_INDEX Then    'Perform Active calcuations
                    '        l_string_FundStatus = "DA"
                    '    ElseIf rbtnList_MoneySelection.SelectedIndex = RETIRED_FUNDS_INDEX Then   'Perform Retired calculations
                    '        l_string_FundStatus = "DR"
                    '    Else        'Perform calculations for the retired side by default
                    '        rbtnList_MoneySelection.SelectedIndex = RETIRED_FUNDS_INDEX
                    '        l_string_FundStatus = "DR"
                    '    End If
                    '    Exit Select
                    'End If
                    ''SR Codes ends here.
                    'pnlMoneySelection.Enabled = False
                    l_string_FundStatus = "DR"
                    'NP:IVP1:2008.04.09 - Adding statuses PEML, NP, PE, PT, WP, PENP to the list    
                    'NP:IVP1:BT-396:2008.04.18 - Adding PENP to the list
                    'NP:YRS-5.0-464:2008.07.09 - Removing NP from the list - They need to be prompted - Reverting these changes as per request in the issue. NP goes to DI.
                Case "DA", "DI", "AE", "TM", "PE", "WD", "QD", "DF", "PEML", "PE", "PT", "WP", "PENP", "NP"   'Consider these as active funds
                    'pnlMoneySelection.Enabled = False
                    l_string_FundStatus = "DA"
                    'NP:IVP1:2008.04.11 - If status is ML then prompt the user to provide a selection for their death status
                    'NP:YRS-5.0-464:2008.07.09 - Adding NP also in the list for prompting. Further the prompt will be made only if the balance in the basic accounts is less than $10,000. - Reverting these changes as per request in the issue. NP goes to DI.
                Case "ML"
                    'pnlMoneySelection.Enabled = False
                    'NP:YRS-5.0-464:2008.07.09 - Check if the basic account balance is greater than or equal to $10,000. If so then consider as DI.
                    If l_string_FundStatus = "ML" Then
                        If YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetBasicAccountBalance(l_string_FundEventId) >= 10000 Then
                            l_string_ForceMLComputationAs = "DI"
                        End If
                    End If

                    If l_string_ForceMLComputationAs = Nothing _
                        OrElse (l_string_ForceMLComputationAs <> "DA" _
                                AndAlso l_string_ForceMLComputationAs <> "DI") Then
                        ' Prompt the user to identify if the participant is actively employed by the YMCA
                        l_string_PageOperation = PAGEOPERATIONS.PROMPT_FOR_10K_BENEFIT
                        If l_string_FundStatus = "ML" Then
                            MessageBox.Show(PlaceHolder1, "Death Calculation", "Person is on Military Leave. Are they still employed by the YMCA? Selecting Yes will provide the $10,000 death benefit if applicable.", MessageBoxButtons.YesNo)
                            Exit Sub
                        End If
                    Else
                        l_string_ForceCalculationAs = l_string_ForceMLComputationAs
                    End If
                    'pnlMoneySelection.Enabled = False
                    l_string_FundStatus = "DA"
                Case Else           'Default case force as Null
                    'pnlMoneySelection.Enabled = True
                    l_string_FundStatus = ""
            End Select
            'Finally if everything is ok, call the processing method
            'l_dataset_DeathCalc_ProcessedData = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.Calculate_Death_Benefits(l_string_PersId, l_string_FundEventId, l_date_DeathDate, l_string_FundStatus, l_int_ReturnStatus)   'NP:PS:2007.09.03 - Adding extra parameter to force calculations for RA/RT/DD status
            l_dataset_DeathCalc_ProcessedData = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.Calculate_Death_Benefits(l_string_PersId, l_string_FundEventId, l_date_DeathDate, l_string_FundStatus, l_string_ForceCalculationAs, l_int_ReturnStatus)   'NP:PS:2008.04.11 - Adding extra parameter to force calculations for ML status as either DA or DI. Can be used generically for other non-retired statuses.
            'Session("First_Calculation") = False
            'l_int_ReturnStatus is -2 if no Beneficiaries have been defined
            If l_int_ReturnStatus <> -2 Then
                'System.AppDomain.CurrentDomain.SetData("appDataset_ProcessedData", l_dataset_DeathCalc_ProcessedData)          ''viewstate("l_dataset_DeathCalc_ProcessedData") = l_dataset_DeathCalc_ProcessedData 
                Me.DataSet_ProcessedData = l_dataset_DeathCalc_ProcessedData
            End If

            'Check the Return Status
            If l_int_ReturnStatus < 0 Then  ' Some special conditions occured
                If l_int_ReturnStatus = -1 Then 'When Participant Already Settled 
                    'Vipul 14Mar06 YMCA 2139 
                    Me.TabStripDeathCalc.Items(1).Enabled = True
                    'Vipul 14Mar06 YMCA 2139 
                    'Me.TabStripDeathCalc.SelectedIndex = 1
                    'Me.MultiPageDeathCalc.SelectedIndex = 1
                    'MessageBox.Show(PlaceHolder1, "Death Calculation ", "Participant Already Settled .. !", MessageBoxButtons.OK)
                    'NP:IVP2:2008.08.08 - Displaying existing message of one - or more beneficiaries are already settled
                    lblErr_BeneficiarySettled.Visible = True
                    lblErr_BeneficiarySettled.Text = "One or more beneficiaries are already settled.<br />Recalculations are not permitted for settled beneficiaries."
                ElseIf l_int_ReturnStatus = -4 Then 'NP:PS:2007.09.03 - Handling the conditions when beneficiaries are not defined
                    'When Beneficiary type "RETIRE" does not exist 
                    BindGrid(DataGrid_Search, CType(Nothing, DataSet))
                    'MessageBox.Show(PlaceHolder1, "Death Calculation ", "No Beneficiaries defined for the Retired money ... Cannot Proceed.", MessageBoxButtons.OK)
                    'Me.TabStripDeathCalc.SelectedIndex = 0  'NP:PS:2007.09.10 - 0
                    'Me.MultiPageDeathCalc.SelectedIndex = 0 'NP:PS:2007.09.10 - 0
                    'Me.TabStripDeathCalc.Items(1).Enabled = False 'NP:PS:2007.09.10 - False
                    lblErr_BeneficiariesNotDefinedProperly.Visible = True
                    lblErr_BeneficiariesNotDefinedProperly.Text = "Atleast one Beneficiary of type RETIRE must be defined for the Participant."
                    'Exit Sub
                ElseIf l_int_ReturnStatus = -5 Then 'NP:PS:2007.09.03 - Handling the conditions when beneficiaries are not defined
                    'When Beneficiary type "MEMBER" does not exist
                    BindGrid(DataGrid_Search, CType(Nothing, DataSet))
                    'MessageBox.Show(PlaceHolder1, "Death Calculation ", "No Beneficiaries defined for the non-retired money ... Cannot Proceed.", MessageBoxButtons.OK)
                    'Me.TabStripDeathCalc.SelectedIndex = 0  'NP:PS:2007.09.10 - 0
                    'Me.MultiPageDeathCalc.SelectedIndex = 0 'NP:PS:2007.09.10 - 0
                    'Me.TabStripDeathCalc.Items(1).Enabled = False 'NP:PS:2007.09.10 - False
                    lblErr_BeneficiariesNotDefinedProperly.Visible = True
                    lblErr_BeneficiariesNotDefinedProperly.Text = "Atleast one Beneficiary of type MEMBER must be defined for the Participant."
                    'Exit Sub
                    'START : BD : 11/28/2018 : YRS-AT-3837 : Message for Participant enrolled on or after 1/1/2019 with C annuity and INSRES beneficiary not defined
                ElseIf l_int_ReturnStatus = -6 Then
                    BindGrid(DataGrid_Search, CType(Nothing, DataSet))
                    lblErr_BeneficiariesNotDefinedProperly.Visible = True
                    lblErr_BeneficiariesNotDefinedProperly.Text = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(YMCAObjects.MetaMessageList.MESSAGE_DEATH_CALC_INSRES_REQUIRED).DisplayText
                    'END : BD : 11/28/2018 : YRS-AT-3837 : Message for Participant enrolled on or after 1/1/2019 with C annuity and INSRES beneficiary not defined
                ElseIf l_int_ReturnStatus = -9 Then 'NP:PS:2007.09.03 - Handling the conditions when beneficiaries are not defined
                    'When Beneficiary type "RETIRE" or "MEMBER" does not exist
                    BindGrid(DataGrid_Search, CType(Nothing, DataSet))
                    MessageBox.Show(PlaceHolder1, "Death Calculation ", "No Beneficiaries defined for the retired or the non-retired money ... Cannot Proceed.", MessageBoxButtons.OK)
                    Me.TabStripDeathCalc.SelectedIndex = 0
                    Me.MultiPageDeathCalc.SelectedIndex = 0
                    Me.TabStripDeathCalc.Items(1).Enabled = False
                    Exit Sub
                ElseIf l_int_ReturnStatus = -2 Then 'THIS IS DEPRECATED. WE DO NOT EXPECT TO GO INTO THIS CASE ANY MORE.
                    'When no Beneficiaries are defined 
                    BindGrid(DataGrid_Search, CType(Nothing, DataSet))
                    MessageBox.Show(PlaceHolder1, "Death Calculation ", "No Beneficiaries defined ... Cannot Proceed.", MessageBoxButtons.OK)
                    'Process_LookupData()
                    Me.TabStripDeathCalc.SelectedIndex = 0
                    Me.MultiPageDeathCalc.SelectedIndex = 0
                    Me.TabStripDeathCalc.Items(1).Enabled = False
                    Exit Sub
                ElseIf l_int_ReturnStatus = -10 Then 'NP:IVP2:2008.08.08 - One or more beneficiaries are settled and there are unsettled beneficiaries with funds that have been split for them
                    'In this case the message to be displayed to the user is different since the DA layer already handles the recomputations for the unsettled beneficiaries by making a 
                    'call to the Calc function. Further recalculations are permitted with a different date
                    lblErr_BeneficiariesNotDefinedProperly.Visible = True
                    lblErr_BeneficiariesNotDefinedProperly.Text = "One or more beneficiaries are already settled.<br />Recalculations are not permitted for settled beneficiaries."
                    ButtonCalculate.Enabled = True  'NP:IVP2:2008.08.21 - Enabling the calculate button
                ElseIf l_int_ReturnStatus = -11 Then 'BS:2011.12.15-YRS 5.0-1350:SR acct not moved over if death before 7/1/2006
                    BindGrid(DataGrid_Search, CType(Nothing, DataSet))
                    MessageBox.Show(PlaceHolder1, "Death Calculation ", "Participant has SR account and the date of death is earlier to 7/1/2006. Please change the SR account to BA before proceeding. Once settled, the BA account should be changed back to SR.", MessageBoxButtons.OK)
                    Me.TabStripDeathCalc.SelectedIndex = 0
                    Me.MultiPageDeathCalc.SelectedIndex = 0
                    Me.TabStripDeathCalc.Items(1).Enabled = False
                    Exit Sub
                End If
            Else
                'NP:IVP2:2008.08.21 - Putting code to enable the calculate button if all is well
                'Everything went smoothly enable the calculate button
                ButtonCalculate.Enabled = True
                ButtonForms.Disabled = False
            End If
            Me.String_PersId = l_string_PersId  ' Persist PersId to Session

            'Shashi Shekhar     28 Feb 2011         Replacing Header formating with user control (YRS 5.0-450 )
            Headercontrol.PageTitle = "Death Benefits Calculator"
            Headercontrol.FundNo = l_dataRowMemberList("FundNo").ToString().Trim()

            'Me.LabelTitle.Text = String.Format(" -- {0}, {1} {2}", l_dataRowMemberList("First Name").ToString().Trim(), _
            '                                                        l_dataRowMemberList("Middle Name").ToString().Trim(), _
            '                                                        l_dataRowMemberList("Last Name").ToString().Trim())
            ''If Retired, add retirement AnnuityType Details
            'If l_dataRowMemberList("StatusType").ToString().Trim() = "RD" OrElse l_dataRowMemberList("StatusType").ToString().Trim() = "DR" Then
            '    'NP:PS:2007.08.16 - Updating code to obtain the Annuity Types based on a Fund Event
            '    'Me.LabelTitle.Text += "Option " + l_dataRowMemberList("AnnuityType").ToString().Trim() + " "
            '    Me.LabelTitle.Text += " Option " + YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetAnnuities(l_dataRowMemberList("FundEventID").ToString().Trim()) + " "

            '    Me.LabelTitle.Text += IIf(l_dataRowMemberList("StatusType").ToString().Trim() = "DR", "Deceased/Retired", l_dataRowMemberList("FundDesc").ToString().Trim())
            'Else
            '    Me.LabelTitle.Text += IIf(l_dataRowMemberList("StatusType").ToString().Trim() = "DA", "Deceased/Active", l_dataRowMemberList("FundDesc").ToString().Trim())
            'End If
            ''----------------------------------------------------------------------------------------------------------------
            ''Shashi Shekhar: 23-Dec-2010: For YRS 5.0-450, BT-643 : Replace SSN with Fund Id in title on all screens.
            '' Me.LabelTitle.Text += " " + Left(l_dataRowMemberList("SS No.").ToString().Trim(), 3) + "-" + l_dataRowMemberList("SS No.").ToString().Substring(3, 2).Trim() + "-" + l_dataRowMemberList("SS No.").ToString().Substring(5, 4).Trim()
            'Me.LabelTitle.Text += " " + l_dataRowMemberList("FundNo").ToString().Trim()

            '--------------------------------------------------------------------------------------------------------------------------------------------------------
            If l_dataset_DeathCalc_ProcessedData.Tables.Count <> 0 Then
                'SR:2012.10.07-YRS 5.0-1707: Commented below lines of code

                '    ' Check if active money options were returned
                '    If l_dataset_DeathCalc_ProcessedData.Tables(2).Rows.Count > 0 Then
                '        _showRetiredMoneyOptions = False
                '        rbtnList_MoneySelection.SelectedIndex = 1

                '    End If
                '    ' Check if retired money options were returned
                '    If l_dataset_DeathCalc_ProcessedData.Tables(1).Rows.Count > 0 Then
                '        _showRetiredMoneyOptions = True
                '        rbtnList_MoneySelection.SelectedIndex = 0
                '    End If
                '    If l_dataset_DeathCalc_ProcessedData.Tables(1).Rows.Count > 0 AndAlso l_dataset_DeathCalc_ProcessedData.Tables(2).Rows.Count > 0 Then
                '        ' We have settlement options under both Active and Retired parts
                '        ' In all probability we are handling a RT, RA or DD participant
                '        ' We need to display the retirement selection panel and set it to retired money
                '        pnlMoneySelection.Enabled = True  'Visible = True
                '        'rbtnList_MoneySelection.Visible = True
                '        rbtnList_MoneySelection.SelectedIndex = 0
                '        _showRetiredMoneyOptions = True
                '        'NP:PS:2007.09.04 - Moving the settign of this visibility up the order and make it dependent on the Fund Event Status
                '        'Else
                '        '    pnlMoneySelection.Enabled = False 'Visible = False
                '        '    'rbtnList_MoneySelection.Visible = False
                '    End If
                'SR:2012.10.07-YRS 5.0-1707: Commented below lines of code
                ''SR:2011.04.26 YRS 5.0-1292: A warning msg should appear for the participant whose annuity disbursement after moth of death is not voided									
                If l_dataset_DeathCalc_ProcessedData.Tables(4).Rows.Count > 0 Then
                    lblErr_BeneficiariesNotDefinedProperly.Visible = True
                    If l_dataset_DeathCalc_ProcessedData.Tables(4).Rows(0).Item(0).ToString() <> "" Then
                        lblErr_BeneficiariesNotDefinedProperly.Text = l_dataset_DeathCalc_ProcessedData.Tables(4).Rows(0).Item(0).ToString()
                        ButtonCalculate.Enabled = True  'NP:IVP2:2008.08.21 - Enabling the calculate button
                    End If
                End If
                PopulateBeneficiariesGrid()
                Me.TabStripDeathCalc.SelectedIndex = 1
                Me.MultiPageDeathCalc.SelectedIndex = 1
                Me.TabStripDeathCalc.Items(1).Enabled = True
            End If
            HelperFunctions.LogMessage("Completed Process_DeathBenefitCalculations() method")
        Catch
            Throw
        End Try

    End Sub

    'NP:PS:2007.09.10 - Adding new function to identify if beneficiaries are settled or not
    'This function takes a filtered dataview and identifies if any of those beneficiaries are settled or not.
    'If the view is filtered by BeneficiaryTypeCode then we can run this function independently for each side of the money.

    Private Function AreAnyBeneficiariesSettled(ByRef dv As DataView) As Boolean
        Dim i As Integer
        For i = 0 To dv.Count - 1
            If dv.Item(i)("Status") = "SETTLD" Then
                Return True
            End If
        Next
        Return False
    End Function
#End Region

#Region "Benefit Options related code"
    'SR:2012.10.29: YRS 5.0-1707:coding
    Private Sub DataGrid_BenefitOptions_ForAll_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid_RetirementPlan_BenefitOptions_ForRetired.ItemDataBound, DataGrid_SavingsPlan_BenefitOptions_ForRetired.ItemDataBound, DataGrid_RetirementPlan_BenefitOptions_ForActive.ItemDataBound, DataGrid_SavingsPlan_BenefitOptions_ForActive.ItemDataBound
        Dim l_date_DeathDate As Date  ' SB | YRS-AT-3371 | 2017.06.19 | Declaring a variable to set death date
        Try
            HelperFunctions.LogMessage("Begin DataGrid_BenefitOptions_ForAll_ItemDataBound() method")
            l_date_DeathDate = Me.ParticipantDeathDate  ' SB | YRS-AT-3371 | 2017.06.19 | Assigning death date value
            With e.Item
                'Start:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                If .Cells.Count = 14 Then
                    .Cells(DataGrid_Retired_money_PlanOptions_index.SSN).Visible = False   'NP:PS:2007.09.06 - Hiding PlanType column
                    .Cells(DataGrid_Retired_money_PlanOptions_index.PlanType).Visible = False
                    'Start:AA:10.12.2015 YRS_AT-2478 Added to hide the columns of pretax,post tax and ymcapretax in grid 
                    .Cells(DataGrid_Retired_money_PlanOptions_index.PersonalPreTax).Visible = False
                    .Cells(DataGrid_Retired_money_PlanOptions_index.PersonalPostTax).Visible = False
                    .Cells(DataGrid_Retired_money_PlanOptions_index.YMCAPreTax).Visible = False
                    .Cells(DataGrid_Retired_money_PlanOptions_index.BeneficiaryTypeCode).Visible = False 'Chandra Sekar| 2016.08.05| YRS-AT-3027 Added to hide the column of BeneficiaryTypeCode in grid 
                    'End:AA:10.12.2015 YRS_AT-2478 Added to hide the columns of pretax,post tax and ymcapretax in grid 
                ElseIf .Cells.Count = 17 Then 'AA:10.12.2015 YRS_AT-2478 Changed as pretax,post tax and ymcapretax included in grid 
                    ' START: SB | YRS-AT-3371 | 2017.06.19 | If the death date is before "7/1/2006" display the records as per the records fetched from the database 
                    If DateTime.Parse(l_date_DeathDate) < Me.SavingsPlanStartDate Then
                        .Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.Select_Image).Visible = False
                        .Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.SSN).Visible = False
                        .Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.PlanType).Visible = False
                        .Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.PersonalPreTax).Visible = False
                        .Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.PersonalPostTax).Visible = False
                        .Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.YMCAPreTax).Visible = False
                        .Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.BeneficiaryTYpeCode).Visible = False
                    Else
                        ' END: SB | YRS-AT-3371 | 2017.06.19 | If the death date is before "7/1/2006" display the records as per the records fetched from the database 
                        '.Cells(10).Visible = False  'NP:PS:2007.09.06 - Hiding PlanType column
                        .Cells(DataGrid_Active_money_PlanOptions_index.SSN).Visible = False  'NP:PS:2007.09.06 - Hiding PlanType column
                        .Cells(DataGrid_Active_money_PlanOptions_index.PlanType).Visible = False
                        'Start:AA:10.12.2015 YRS_AT-2478 Added to hide the columns of pretax,post tax and ymcapretax in grid 
                        .Cells(DataGrid_Active_money_PlanOptions_index.PersonalPreTax).Visible = False
                        .Cells(DataGrid_Active_money_PlanOptions_index.PersonalPostTax).Visible = False
                        .Cells(DataGrid_Active_money_PlanOptions_index.YMCAPreTax).Visible = False
                        'End:AA:10.12.2015 YRS_AT-2478 Added to hide the columns of pretax,post tax and ymcapretax in grid 
                    End If
                End If

                If .ItemType = ListItemType.Item OrElse .ItemType = ListItemType.AlternatingItem OrElse .ItemType = ListItemType.SelectedItem Then

                    If .Cells.Count = 14 Then
                        .Cells(DataGrid_Retired_money_PlanOptions_index.Lump_Sum).Text = Decimal.Parse(.Cells(DataGrid_Retired_money_PlanOptions_index.Lump_Sum).Text).ToString("###,###,##0.00")
                        .Cells(DataGrid_Retired_money_PlanOptions_index.Annuity_M).Text = Decimal.Parse(.Cells(DataGrid_Retired_money_PlanOptions_index.Annuity_M).Text).ToString("###,###,##0.00")
                        .Cells(DataGrid_Retired_money_PlanOptions_index.Annuity_C).Text = Decimal.Parse(.Cells(DataGrid_Retired_money_PlanOptions_index.Annuity_C).Text).ToString("###,###,##0.00")
                        .Cells(DataGrid_Retired_money_PlanOptions_index.Reserves).Text = Decimal.Parse(.Cells(DataGrid_Retired_money_PlanOptions_index.Reserves).Text).ToString("###,###,##0.00")
                        .Cells(DataGrid_Retired_money_PlanOptions_index.Death_Benefit).Text = Decimal.Parse(.Cells(DataGrid_Retired_money_PlanOptions_index.Death_Benefit).Text).ToString("###,###,##0.00")
                    End If
                    If .Cells.Count = 17 Then 'AA:10.12.2015 YRS_AT-2478 Changed as pretax,post tax and ymcapretax included in grid
                        ' START: SB | YRS-AT-3371 | 2017.06.19 | If the death date is before "7/1/2006" display the records as per the records fetched from the database 
                        If DateTime.Parse(l_date_DeathDate) < Me.SavingsPlanStartDate Then
                            .Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.Lump_Sum).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.Lump_Sum).Text).ToString("###,###,##0.00")
                            .Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.Annuity_M).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.Annuity_M).Text).ToString("###,###,##0.00")
                            .Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.Annuity_C).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.Annuity_C).Text).ToString("###,###,##0.00")
                            .Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.Reserves).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.Reserves).Text).ToString("###,###,##0.00")
                            .Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.PIA).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.PIA).Text).ToString("###,###,##0.00")
                            .Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.Death_Benefit).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.Death_Benefit).Text).ToString("###,###,##0.00")
                            .Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.Total).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index_PreJuly2006DeathDate.Total).Text).ToString("###,###,##0.00")
                        Else
                            ' END: SB | YRS-AT-3371 | 2017.06.19 | If the death date is before "7/1/2006" display the records as per the records fetched from the database 
                            .Cells(DataGrid_Active_money_PlanOptions_index.Lump_Sum).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index.Lump_Sum).Text).ToString("###,###,##0.00")
                            .Cells(DataGrid_Active_money_PlanOptions_index.Annuity_M).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index.Annuity_M).Text).ToString("###,###,##0.00")
                            .Cells(DataGrid_Active_money_PlanOptions_index.Annuity_C).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index.Annuity_C).Text).ToString("###,###,##0.00")
                            .Cells(DataGrid_Active_money_PlanOptions_index.Basic_Reserves).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index.Basic_Reserves).Text).ToString("###,###,##0.00")
                            .Cells(DataGrid_Active_money_PlanOptions_index.Voluantry).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index.Voluantry).Text).ToString("###,###,##0.00")
                            .Cells(DataGrid_Active_money_PlanOptions_index.Death_Benefit).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index.Death_Benefit).Text).ToString("###,###,##0.00")
                            .Cells(DataGrid_Active_money_PlanOptions_index.TD_Savings).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index.TD_Savings).Text).ToString("###,###,##0.00")
                            .Cells(DataGrid_Active_money_PlanOptions_index.MB_Account).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index.MB_Account).Text).ToString("###,###,##0.00")
                            .Cells(DataGrid_Active_money_PlanOptions_index.Total).Text = Decimal.Parse(.Cells(DataGrid_Active_money_PlanOptions_index.Total).Text).ToString("###,###,##0.00")
                        End If
                    End If
                    'End:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                End If
                'anita and shubhrata apr21
            End With
            HelperFunctions.LogMessage("Completed DataGrid_BenefitOptions_ForAll_ItemDataBound() method")
        Catch ex As SqlException
            HelperFunctions.LogException("DataGrid_BenefitOptions_ForAll_ItemDataBound", ex)
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            HelperFunctions.LogException("DataGrid_BenefitOptions_ForAll_ItemDataBound", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    Private Sub DataGrid_RetirementPlan_BenefitOptions_ForRetired_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid_RetirementPlan_BenefitOptions_ForRetired.PreRender, DataGrid_SavingsPlan_BenefitOptions_ForRetired.PreRender, DataGrid_RetirementPlan_BenefitOptions_ForActive.PreRender, DataGrid_SavingsPlan_BenefitOptions_ForActive.PreRender
        Dim dgi As DataGridItem
        'Dim dg As DataGrid = CType(sender, DataGrid) commented by SR:2010.06.15 for migration
        Dim dg As DataGrid = DirectCast(sender, DataGrid)
        Dim rBtn As ImageButton
        Try
            For Each dgi In dg.Items
                rBtn = dgi.Cells(DataGrid_Retired_money_PlanOptions_index.Select_Image).FindControl("Imagebutton1")  'AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                If Not rBtn Is Nothing Then
                    If dgi.ItemType = ListItemType.SelectedItem Then
                        rBtn.ImageUrl = SELECTED_IMAGE_BUTTON_URL
                    Else
                        rBtn.ImageUrl = NORMAL_IMAGE_BUTTON_URL
                    End If
                End If
            Next
        Catch ex As SqlException
            HelperFunctions.LogException("DataGrid_RetirementPlan_BenefitOptions_ForRetired_PreRender", ex)
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            HelperFunctions.LogException("DataGrid_RetirementPlan_BenefitOptions_ForRetired_PreRender", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    'SR:2012.10.29: YRS 5.0-1707:Coding
    Private Sub DataGrid_BenefitOptions_ForAll_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid_RetirementPlan_BenefitOptions_ForRetired.SelectedIndexChanged, DataGrid_SavingsPlan_BenefitOptions_ForRetired.SelectedIndexChanged, DataGrid_RetirementPlan_BenefitOptions_ForActive.SelectedIndexChanged, DataGrid_SavingsPlan_BenefitOptions_ForActive.SelectedIndexChanged
        Try
            EnableDisableFormsButton()
            PopulateBeneficiaryOptionsGrid()
        Catch ex As SqlException
            HelperFunctions.LogException("DataGrid_RetirementPlan_BenefitOptions_ForRetired_PreRender", ex)
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            HelperFunctions.LogException("DataGrid_RetirementPlan_BenefitOptions_ForRetired_PreRender", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    'End, SR:2012.10.29: YRS 5.0-1707:Coding
    Public Sub PopulateBeneficiaryOptionsGrid()
        Dim l_drow_BeneficiaryOptions As DataRow
        Dim l_ctr As Integer
        Dim l_dt_RetiredMoney_Options As New DataTable
        Dim l_dt_ActiveMoney_Options As New DataTable
        Dim l_dt_JSAnnuities_Options As New DataTable
        Dim l_dt_BasicReserves_Options As New DataTable
        Dim l_dt_Options As DataTable
        Dim l_ds_tempdataset As DataSet
        Dim l_str_selectedBeneficiaryId As String
        Dim l_str_filterCriteria As String
        Dim dr As DataRow
        ' START | SR | 2015.12.15 | YRS-AT-2718 - Add variables. 
        Dim dtDeathBenefitNotAllowed As New DataTable
        Dim l_string_DeathBenefitRestrictionMessage As String
        ' END | SR | 2015.12.15 | YRS-AT-2718 - Add variables. 

        'Dim tblJSAnnuities As DataTable = Session("JSannuities")
        Try
            HelperFunctions.LogMessage("Begin PopulateBeneficiaryOptionsGrid() method")
            'SR:2012.10.29: YRS 5.0-1707
            'Anudeep:25.03.2013:Bt-1303-YRS 5.0-1707:New Death Benefit Application form 
            'Checking if annuity benficiary is also selected
            If Me.DataGrid_BeneficiariesList_ForAll.SelectedIndex < 0 And DataGridAnnuities.SelectedIndex < 0 Then Exit Sub

            ' Initialize local variables
            l_ds_tempdataset = Me.DataSet_ProcessedData
            l_dt_RetiredMoney_Options = l_ds_tempdataset.Tables(1).Copy
            l_dt_ActiveMoney_Options = l_ds_tempdataset.Tables(2).Copy
            'l_dt_JSAnnuities_Options = tblJSAnnuities
            l_dt_BasicReserves_Options = l_ds_tempdataset.Tables(5).Copy

            ' START | SR | 2015.12.15 | YRS-AT-2718 - Get SSNo list for whom Annuity option not available from Death benefit amount.
            If (HelperFunctions.isNonEmpty(l_ds_tempdataset) AndAlso l_ds_tempdataset.Tables.Count >= 7 AndAlso HelperFunctions.isNonEmpty(l_ds_tempdataset.Tables(6).Copy)) Then
                dtDeathBenefitNotAllowed = l_ds_tempdataset.Tables(6).Copy
            End If
            ' END | SR | 2015.12.15 | YRS-AT-2718 - Get SSNo list for whom Annuity option not available from Death benefit amount.

            'If after filtering the beneficiary dataset, there are no records left then we do not have to do anything

            If HelperFunctions.isEmpty(l_ds_tempdataset.Tables(0).DefaultView) Then
                BindGrid(DataGrid_RetirementPlan_BenefitOptions_ForRetired, CType(Nothing, DataSet))
                BindGrid(DataGrid_SavingsPlan_BenefitOptions_ForRetired, CType(Nothing, DataSet))
                BindGrid(DataGrid_RetirementPlan_BenefitOptions_ForActive, CType(Nothing, DataSet))
                BindGrid(DataGrid_SavingsPlan_BenefitOptions_ForActive, CType(Nothing, DataSet))
                BindGrid(DataGridAnnuities, CType(Nothing, DataSet))
                HandleDisplay()
                Exit Sub
            End If
            'End Select

            'SR:2012.10.29: YRS 5.0-1707
            Dim l_dt_Options_RetiredMoney As DataTable
            Dim l_dt_Options_ActiveMoney As DataTable
            Dim l_dt_tmpJSAnnuities As DataTable
            Dim l_dt_tmpBasicReserves As DataTable
            Dim l_str_selectedSSNo As String

            Dim strRelationShip As String 'SP 2014.12.04 BT-2310\YRS 5.0-2255

            'Start:Anudeep:25.03.2013:Bt-1303-YRS 5.0-1707:New Death Benefit Application form 
            If Me.DataGrid_BeneficiariesList_ForAll.SelectedIndex > -1 Then
                'Start:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                l_str_selectedSSNo = l_ds_tempdataset.Tables(0).DefaultView.Item(DataGrid_BeneficiariesList_ForAll.SelectedIndex)("SSNO").ToString().Trim()
                Session("DeathCalc_BenFirstName") = IIf(DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.FirstName).Text.ToString.Trim() = "&nbsp;", "", DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.FirstName).Text.ToString.Trim())
                Session("DeathCalc_BenLastName") = IIf(DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.LastName).Text.ToString.Trim() = "&nbsp;", "", DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.LastName).Text.ToString.Trim())

                'If annuity beneficiary is selected then it will extract ssno of beneficary and gets the benefit options 

                'SP 2014.12.04 BT-2310\YRS 5.0-2255 -Start
                'Get relation ship to check it is non human beneficiary or not
                strRelationShip = IIf(DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.RelationShip).Text.ToString.Trim() = "&nbsp;", "", DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.RelationShip).Text.ToString.Trim())
                'End:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                If (String.IsNullOrEmpty(strRelationShip)) Then
                    Session("SelectedBeneficiaryIsNonHuman") = False
                ElseIf strRelationShip.Trim().Equals("TR") Or strRelationShip.Trim().Equals("IN") Or strRelationShip.Trim().Equals("ES") Then
                    Session("SelectedBeneficiaryIsNonHuman") = True
                Else
                    Session("SelectedBeneficiaryIsNonHuman") = False
                End If
                'SP 2014.12.04 BT-2310\YRS 5.0-2255 -End

            ElseIf DataGridAnnuities.SelectedIndex > -1 Then
                l_str_selectedSSNo = DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(DataGridAnnuities_index.SSN).Text.Trim() 'AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                'Start: AA:08.12.2014- BT:2460:YRS 5.0-2331 - Commented below line because to remove unneccesary sessions which are not in use here
                'Session("DeathCalc_BenFirstName") = IIf(DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(2).Text.ToString().Trim() = "&nbsp;", "", DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(2).Text.ToString().Trim())
                'Session("DeathCalc_BenLastName") = IIf(DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(3).Text.ToString().Trim() = "&nbsp;", "", DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(3).Text.ToString().Trim())
                'End: AA:08.12.2014- BT:2460:YRS 5.0-2331 - Commented below line because to remove unneccesary sessions which are not in use here
                Session("SelectedBeneficiaryIsNonHuman") = False 'SP 2014.12.04 BT-2310\YRS 5.0-2255
            End If
            Session("DeathCalc_BenTaxNo") = l_str_selectedSSNo.Trim() 'SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
            If l_str_selectedSSNo = "&nbsp;" Then
                Exit Sub
            End If
            'End:Anudeep:25.03.2013:Bt-1303-YRS 5.0-1707:New Death Benefit Application form 


            'l_str_filterCriteria = "SSNo='" & l_str_selectedSSNo & "' or BeneficiaryID='" & l_str_selectedBID & "'" '& l_ds_tempdataset.Tables(0).Rows(Me.DataGrid_BeneficiariesList.SelectedIndex)("UniqueID").ToString().Trim() & "' "
            l_str_filterCriteria = "SSNo='" & l_str_selectedSSNo & "'"

            'Start:AA:10.23.2015 YRS_AT-2478 commented to get columns of pretax,post tax in retired money grid 
            'l_dt_RetiredMoney_Options.Columns.Remove("PersonalPreTax")
            'l_dt_RetiredMoney_Options.Columns.Remove("PersonalPostTax")
            'End:AA:10.23.2015 YRS_AT-2478 commented to get columns of pretax,post tax in retired money grid 
            'Start:AA:10.12.2015 YRS_AT-2478 commented to get columns of pretax,post tax in grid 
            'l_dt_ActiveMoney_Options.Columns.Remove("PersonalPreTax")
            'l_dt_ActiveMoney_Options.Columns.Remove("PersonalPostTax")
            'End:AA:10.12.2015 YRS_AT-2478 commented to get columns of pretax,post tax in grid 

            'Populated table for Retired Money options
            l_dt_Options_RetiredMoney = l_dt_RetiredMoney_Options

            ' START | SR | 2015.12.09 | YRS-AT-2718 | Display message if annuity options will not be available to the  beneficiary. 
            lblInformation.Text = String.Empty
            lblInformation.Visible = False
            l_string_DeathBenefitRestrictionMessage = GetMessageFromResourceFile("MESSAGE_DC_DTH_BENEFIT_ANN_PURCHASE_RESTRICTION").ToString().Replace("$$RDB_ADB_2019Plan_Change_CutOFF_Date$$", Me.DeathBenefitAnnuityPurchaseRestrictedDate) 'Dharmesh : 11/28/2018 : YRS-AT-3837 : change the config key which is used for cut off date 1/1/2019
            If (HelperFunctions.isNonEmpty(dtDeathBenefitNotAllowed)) Then
                If (dtDeathBenefitNotAllowed.Select(l_str_filterCriteria).Length > 0) Then
                    lblInformation.Visible = True
                    lblInformation.Text = IIf((lblErr_BeneficiariesNotDefinedProperly.Text = "" AndAlso lblErr_BeneficiarySettled.Text = ""), l_string_DeathBenefitRestrictionMessage, "<br/>" + l_string_DeathBenefitRestrictionMessage)
                End If
            End If
            ' END | SR | 2015.12.09 | YRS-AT-2718 | Display message if annuity options will not be available to the  beneficiary.

            'Populated table for Active Money options
            l_dt_Options_ActiveMoney = l_dt_ActiveMoney_Options
            'Populated table for JS Annuity options
            l_dt_tmpJSAnnuities = l_dt_JSAnnuities_Options
            'Populated table to find basic resreves excluding 3 months interest
            l_dt_tmpBasicReserves = l_dt_BasicReserves_Options

            'Filter beeficiary for Retired Money options 
            l_dt_Options_RetiredMoney.DefaultView.RowFilter = l_str_filterCriteria
            Dim dtRetiredMoney As DataTable = l_dt_Options_RetiredMoney.Clone()
            For Each dr In l_dt_Options_RetiredMoney.Select(l_str_filterCriteria)
                dtRetiredMoney.ImportRow(dr)
            Next
            dtRetiredMoney.AcceptChanges()

            'Bind retirement plan for Retired Money options 
            dtRetiredMoney.DefaultView.RowFilter = "PlanType = 'RETIREMENT' OR PlanType is null"
            Me.DataGrid_RetirementPlan_BenefitOptions_ForRetired.DataSource = dtRetiredMoney.DefaultView
            Me.DataGrid_RetirementPlan_BenefitOptions_ForRetired.DataBind()
            'Me.DataGrid_RetirementPlan_BenefitOptions_ForRetired.Columns.Item(6).Visible = False

            'Bind saving plan for Retired Money options 
            dtRetiredMoney.DefaultView.RowFilter = "PlanType = 'SAVINGS'"
            Me.DataGrid_SavingsPlan_BenefitOptions_ForRetired.DataSource = dtRetiredMoney.DefaultView
            Me.DataGrid_SavingsPlan_BenefitOptions_ForRetired.DataBind()
            'Me.DataGrid_SavingsPlan_BenefitOptions_ForRetired.Columns.Item(6).Visible = False

            'Filter beeficiary for active Money options 
            l_dt_Options_ActiveMoney.DefaultView.RowFilter = l_str_filterCriteria
            Dim dtActiveMoney As DataTable = l_dt_Options_ActiveMoney.Clone()
            For Each dr In l_dt_Options_ActiveMoney.Select(l_str_filterCriteria)
                dtActiveMoney.ImportRow(dr)
            Next
            dtActiveMoney.AcceptChanges()

            'Bind Retirement plan for active Money options 
            dtActiveMoney.DefaultView.RowFilter = "PlanType = 'RETIREMENT' OR PlanType is null"
            Me.DataGrid_RetirementPlan_BenefitOptions_ForActive.DataSource = dtActiveMoney.DefaultView
            Me.DataGrid_RetirementPlan_BenefitOptions_ForActive.DataBind()

            'Bind Saving plan for active Money options 
            dtActiveMoney.DefaultView.RowFilter = "PlanType = 'SAVINGS'"
            Me.DataGrid_SavingsPlan_BenefitOptions_ForActive.DataSource = dtActiveMoney.DefaultView
            Me.DataGrid_SavingsPlan_BenefitOptions_ForActive.DataBind()
            'Me.DataGrid_SavingsPlan_BenefitOptions_ForActive.Columns.Item(6).Visible = False

            'Filter beneficiary for JS annuities 
            'l_str_filterCriteria = "Output_txtSSN='" & l_str_selectedSSNo & "'" '& l_ds_tempdataset.Tables(0).Rows(Me.DataGrid_BeneficiariesList.SelectedIndex)("UniqueID").ToString().Trim() & "' "
            'l_dt_tmpJSAnnuities.DefaultView.RowFilter = l_str_filterCriteria
            'Dim dtJSAnnuities As DataTable = l_dt_tmpJSAnnuities.Clone()
            'For Each dr In l_dt_tmpJSAnnuities.Select(l_str_filterCriteria)
            '    dtJSAnnuities.ImportRow(dr)
            'Next
            'dtJSAnnuities.AcceptChanges()

            'Filter beneficiary for basic reserves
            l_str_filterCriteria = "SSNo='" & l_str_selectedSSNo & "'"
            l_dt_tmpBasicReserves.DefaultView.RowFilter = l_str_filterCriteria
            Dim dtBasicResrves As DataTable = l_dt_tmpBasicReserves.Clone()
            For Each dr In l_dt_tmpBasicReserves.Select(l_str_filterCriteria)
                dtBasicResrves.ImportRow(dr)
            Next
            dtBasicResrves.AcceptChanges()

            Session("SelectedBeneficiaryDetails_ForRetired") = dtRetiredMoney
            Session("SelectedBeneficiaryDetails_ForActive") = dtActiveMoney
            'Session("SelectedBeneficiaryDetails_ForJSAnnuity") = dtJSAnnuities
            Session("SelectedBeneficiaryDetails_ForBasicReserves") = dtBasicResrves
            'Ends, SR:2012.10.29: YRS 5.0-1707
            Me.EnableDisableFormsButton()
            HandleDisplay()
            HelperFunctions.LogMessage("Completed PopulateBeneficiaryOptionsGrid() method")
        Catch ex As Exception
            HelperFunctions.LogException("PopulateBeneficiaryOptionsGrid()", ex)
            Throw
        End Try
    End Sub
#End Region

    Public Sub HandleDisplay()
        Try
            HelperFunctions.LogMessage("Begin HandleDisplay() method")
            'SR:2012.10.29: YRS 5.0-1707
            If DataGrid_BeneficiariesList_ForAll.SelectedIndex > -1 Then
                If DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.Member).Text.ToString.Trim() = "SETTLD" Then 'AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                    ButtonCalculate.Enabled = False
                Else
                    ButtonCalculate.Enabled = True
                End If
            End If

            If DataGridAnnuities.Items.Count > 0 Then
                lbl_JSBeneficiaries.Text = "List of options under JS Annuity"
                lbl_JSBeneficiaries.Visible = True
                Me.DataGridAnnuities.Visible = True
            Else    ' Hide this particular panel
                DataGridAnnuities.Visible = False
                lbl_JSBeneficiaries.Text = "No Options available under JS Annuity"
            End If
            'Anudeep: 17.12.2012 Changes made Not show retired and active money in label as we are showing in header
            If DataGrid_RetirementPlan_BenefitOptions_ForRetired.Items.Count > 0 Then
                lbl_RetirementPlanOptions_ForRetired.Text = "List of options under the Retirement plan"
                lbl_RetirementPlanOptions_ForRetired.Visible = True
                Me.DataGrid_RetirementPlan_BenefitOptions_ForRetired.Visible = True
                If DateTime.Parse(TextBoxCalcDeathDate.Text) < New Date(2006, 7, 1) Then 'If this is before plan split then we do not want to differentiate this as Retirement plan.
                    lbl_RetirementPlanOptions_ForRetired.Text = "List of options"
                End If
            Else    ' Hide this particular panel
                DataGrid_RetirementPlan_BenefitOptions_ForRetired.Visible = False
                lbl_RetirementPlanOptions_ForRetired.Text = "No Options available under Retirement plan"
                lbl_RetirementPlanOptions_ForRetired.Visible = True
                If DateTime.Parse(TextBoxCalcDeathDate.Text) < New Date(2006, 7, 1) Then 'If this is before plan split then we do not want to differentiate this as Retirement plan.
                    lbl_RetirementPlanOptions_ForRetired.Text = "No Options available under this plan"
                End If
            End If
            If DataGrid_SavingsPlan_BenefitOptions_ForRetired.Items.Count > 0 Then
                lbl_SavingsPlanOptions_ForRetired.Text = "List of options under the TD Savings plan"
                lbl_SavingsPlanOptions_ForRetired.Visible = True   'NP:PS:2007.08.07 - Fixing issue where the label was not displayed for the Savings Plan Option datagrid after performing a calculation for pre-july 2006 person.
                DataGrid_SavingsPlan_BenefitOptions_ForRetired.Visible = True
            Else    ' Hide this particular panel
                DataGrid_SavingsPlan_BenefitOptions_ForRetired.Visible = False
                lbl_SavingsPlanOptions_ForRetired.Text = "No Options available under TD Savings plan"
                lbl_SavingsPlanOptions_ForRetired.Visible = True
                If DateTime.Parse(TextBoxCalcDeathDate.Text) < New Date(2006, 7, 1) Then ' If this is before plan split we do not want to show any options under the Savings plan
                    lbl_SavingsPlanOptions_ForRetired.Visible = False
                End If
            End If

            If DataGrid_RetirementPlan_BenefitOptions_ForActive.Items.Count > 0 Then
                lbl_RetirementPlanOptions_ForActive.Text = "List of options under the Retirement plan"
                lbl_RetirementPlanOptions_ForActive.Visible = True
                Me.DataGrid_RetirementPlan_BenefitOptions_ForActive.Visible = True
                If DateTime.Parse(TextBoxCalcDeathDate.Text) < New Date(2006, 7, 1) Then 'If this is before plan split then we do not want to differentiate this as Retirement plan.
                    lbl_RetirementPlanOptions_ForActive.Text = "List of options"
                End If
            Else    ' Hide this particular panel
                DataGrid_RetirementPlan_BenefitOptions_ForActive.Visible = False
                lbl_RetirementPlanOptions_ForActive.Text = "No Options available under Retirement plan"
                lbl_RetirementPlanOptions_ForActive.Visible = True
                If DateTime.Parse(TextBoxCalcDeathDate.Text) < New Date(2006, 7, 1) Then 'If this is before plan split then we do not want to differentiate this as Retirement plan.
                    lbl_RetirementPlanOptions_ForActive.Text = "No Options available under this plan"
                End If
            End If

            If DataGrid_SavingsPlan_BenefitOptions_ForActive.Items.Count > 0 Then
                lbl_SavingsPlanOptions_ForActive.Text = "List of options under the TD Savings plan"
                lbl_SavingsPlanOptions_ForActive.Visible = True   'NP:PS:2007.08.07 - Fixing issue where the label was not displayed for the Savings Plan Option datagrid after performing a calculation for pre-july 2006 person.
                DataGrid_SavingsPlan_BenefitOptions_ForActive.Visible = True
            Else    ' Hide this particular panel
                DataGrid_SavingsPlan_BenefitOptions_ForActive.Visible = False
                lbl_SavingsPlanOptions_ForActive.Text = "No Options available under TD Savings plan"
                lbl_SavingsPlanOptions_ForActive.Visible = True
                If DateTime.Parse(TextBoxCalcDeathDate.Text) < New Date(2006, 7, 1) Then ' If this is before plan split we do not want to show any options under the Savings plan
                    lbl_SavingsPlanOptions_ForActive.Visible = False
                End If
            End If
            'Ends, SR:2012.10.29: YRS 5.0-1707
            HelperFunctions.LogMessage("Completed HandleDisplay() method")
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub ButtonCalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCalculate.Click
        Try
            Process_DeathBenefitCalculations()
        Catch ex As SqlException
            If sender Is Me Then
                HelperFunctions.LogException("ButtonCalculate_Click()", ex)
                Throw ex
            Else
                HelperFunctions.LogException("ButtonCalculate_Click()", ex)
                Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            End If
        Catch ex As Exception
            If sender Is Me Then
                HelperFunctions.LogException("ButtonCalculate_Click()", ex)
                Throw ex
            Else
                HelperFunctions.LogException("ButtonCalculate_Click()", ex)
                Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            End If
        End Try
    End Sub

    Private Sub RadioButtonAnnual_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonAnnual.CheckedChanged
        Try
            If Me.RadioButtonAnnual.Checked = True Then
                PopulateBeneficiaryOptionsGrid()
            End If
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    'Anudeep A.2013.01.07 - code commented below for YRS 5.0-1707:New Death Benefit Application form
    'Private Sub RadioButtonMonthly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonMonthly.CheckedChanged
    '    Try
    '        If Me.RadioButtonMonthly.Checked Then
    '            PopulateBeneficiaryOptionsGrid()
    '        End If
    '    Catch ex As SqlException
    '        Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    End Try
    'End Sub
#Region "Search Section related code"
    'NP:PS:2007.09.17 - Updated function to handle sorting and display of data without the use of a viewstate.
    Private Sub DataGrid_Search_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid_Search.SortCommand
        Try
            '-------------------------------------------------------------
            ' Logic for this routine
            '-------------------------------------------------------------
            ' Reinitialize the selected Index
            ' If there are search results only then we perform the sort
            ' If this is the second time around then get the previous sort expression from viewstate
            ' If last time we had sorted on the same column then toggle the sorting order else sort with the normal order
            ' Save the new sort expression to the ViewState
            DataGrid_Search.SelectedIndex = -1
            If HelperFunctions.isNonEmpty(l_dataset_SearchResults) Then
                If Not ViewState("previousSearchSortExpression") Is Nothing AndAlso ViewState("previousSearchSortExpression") <> "" Then
                    'If SortExpression is not the same as the previous one then initialize new one
                    If (e.SortExpression <> ViewState("previousSearchSortExpression")) Then
                        ViewState("previousSearchSortExpression") = e.SortExpression
                        l_dataset_SearchResults.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    Else
                        'else toggle existing sort expression
                        l_dataset_SearchResults.Tables(0).DefaultView.Sort = IIf(l_dataset_SearchResults.Tables(0).DefaultView.Sort.EndsWith("ASC") = True, e.SortExpression + " DESC", e.SortExpression + " ASC")
                    End If
                Else
                    'First time in the sort function
                    l_dataset_SearchResults.Tables(0).DefaultView.Sort = e.SortExpression + " ASC"
                    ViewState("previousSearchSortExpression") = e.SortExpression
                End If
                BindGrid(DataGrid_Search, l_dataset_SearchResults.Tables(0).DefaultView)
            End If
        Catch ex As Exception
            HelperFunctions.LogException("DataGrid_Search_SortCommand()", ex)
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    Private Sub DataGrid_Search_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid_Search.ItemDataBound
        Try
            'NP:IVP2:2008.11.19 - Adding code to not display more than specified number of results
            If e.Item.ItemIndex > DataGrid_Search.PageSize Then
                e.Item.Visible = False
                lbl_Search_MoreItems.Visible = True
                lbl_Search_MoreItems.Text = "Results truncated. Showing only " + DataGrid_Search.PageSize.ToString() + " rows out of " + l_dataset_SearchResults.Tables(0).DefaultView.Count.ToString()
            End If
            'NP:PS:2007.08.14 - Updating code to only hide those columns that exist. This can be done better at the DataBind level by simply hiding the columns.
            'Start:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
            If e.Item.Cells.Count > DataGrid_Search_ForAll_index.FundEventId Then e.Item.Cells(DataGrid_Search_ForAll_index.FundEventId).Visible = False
            If e.Item.Cells.Count > DataGrid_Search_ForAll_index.StatusType Then e.Item.Cells(DataGrid_Search_ForAll_index.StatusType).Visible = False
            If e.Item.Cells.Count > DataGrid_Search_ForAll_index.DeathDate Then e.Item.Cells(DataGrid_Search_ForAll_index.DeathDate).Visible = False
            If e.Item.Cells.Count > DataGrid_Search_ForAll_index.IsArchived Then e.Item.Cells(DataGrid_Search_ForAll_index.IsArchived).Visible = False 'Shashi shekhar:2010-02-12: Hiding IsArchived Col 
            If e.Item.Cells.Count > DataGrid_Search_ForAll_index.FundIdNo Then e.Item.Cells(DataGrid_Search_ForAll_index.FundIdNo).Visible = False 'AA:09.28.2015 YRS-AT-2548 Added to hide the fundno column because as the persid and fundeventid columns are getting binded
            'End:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
            If e.Item.Cells.Count > DataGrid_Search_ForAll_index.BirthDate Then e.Item.Cells(DataGrid_Search_ForAll_index.BirthDate).Visible = False 'MMR | 2020.02.10 | YRS-AT-4770 | Hiding birth date column in grid as not required to be displayed on UI
        Catch ex As Exception
            HelperFunctions.LogException("DataGrid_Search_ItemDataBound()", ex)
        End Try
    End Sub
    Private Sub DataGrid_Search_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid_Search.SelectedIndexChanged
        Try
            HelperFunctions.LogMessage("Begin DataGrid_Search SelectedIndexChanged method")
            '----Shashi Shekhar:2010-02-12: Code to handle Archived Participants from list--------------
            'Shashi Shekhar:2010-03-11:Handling usability issue of Data Archive
            Dim l_SSNo As String
            Dim dr As DataRow()
            ClearSession()
            'Session("First_Calculation") = True ''SR:2012.12.10 - YRS 5.0-1707 - Added
            'Session("Change_FundStatus") = Nothing 'SR:2012.12.08 - YRS 5.0-1707:Added
            'Start:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
            l_SSNo = Me.DataGrid_Search.SelectedItem.Cells(DataGrid_Search_ForAll_index.SSNO).Text.Trim
            l_SSNumber = DataGrid_Search.SelectedItem.Cells(DataGrid_Search_ForAll_index.SSNO).Text.Trim
            String_FundeventId = DataGrid_Search.SelectedItem.Cells(DataGrid_Search_ForAll_index.FundEventId).Text.Trim 'AA:10.21.2015 YRS-AT-2548: Added to capture fundevent id to avoid indexing error while reloading page
            dr = l_dataset_SearchResults.Tables(0).Select("[SS No.]= '" & l_SSNo & "' And FundEventID='" + String_FundeventId + "'") 'AA:09.28.2015 YRS-AT-2548 Added fundeventid column to get the exact selected participant details, to avoid multiple fundevents problem and using enum instead of hardcorde values
            'End:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
            ' If Me.DataGrid_Search.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" And Me.DataGrid_Search.SelectedItem.Cells(7).Text.ToUpper.Trim() <> "RD" Then
            If ((dr(0).Item("IsArchived") = True) And (dr(0).Item("StatusType").trim <> "RD")) Then
                'Shashi Shekhar:2010-04-08: Adding Stauts "DR" also with "RD" for Handling usability issue of Data Archive
                If ((dr(0).Item("IsArchived") = True) And (dr(0).Item("StatusType").trim <> "DR")) Then
                    ' l_dataset_SearchResults()
                    MessageBox.Show(Me.PlaceHolder1, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
                    Me.TabStripDeathCalc.Items(1).Enabled = False

                    Exit Sub
                End If
            End If
            '---------------------------------------------------------------------------------------

            OnSelectedParticipantChanged()
            'Simulate click of the Tab strip button on index change
            TabStripDeathCalc.SelectedIndex = 1
            TabStripDeathCalc_SelectedIndexChange(Me, Nothing)
            If HelperFunctions.isNonEmpty(l_dataset_SearchResults) Then
                BindGrid(DataGrid_Search, l_dataset_SearchResults.Tables(0).DefaultView)
                'Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up 
                DataSet_AdditionalForms = Nothing
                AdditionalForms()
            Else
                BindGrid(DataGrid_Search, l_dataset_SearchResults)
            End If
            HelperFunctions.LogMessage("End DataGrid_Search SelectedIndexChanged method")
        Catch ex As SqlException
            If sender Is Me Then
                HelperFunctions.LogException("DataGrid_Search_SelectedIndexChanged()", ex)
                Throw ex
            Else
                HelperFunctions.LogException("DataGrid_Search_SelectedIndexChanged()", ex)
                Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            End If
        Catch ex As Exception
            If sender Is Me Then
                HelperFunctions.LogException("DataGrid_Search_SelectedIndexChanged()", ex)
                Throw ex
            Else
                HelperFunctions.LogException("DataGrid_Search_SelectedIndexChanged()", ex)
                Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            End If
        End Try

    End Sub

    Private Sub ButtonFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonFind.Click
        Try
            TextBoxSSNo.Text = TextBoxSSNo.Text.Replace("-", "")    'Feature: User can put SSN in 222-22-2222 format also
            lbl_Search_MoreItems.Visible = False    ' Anudeep :17.12.2012 changes made to not show more items label when there are few records in search grid
            If Me.TextBoxSSNo.Text.Trim = "" And Me.TextBoxLastName.Text.Trim = "" And Me.TextBoxFirstName.Text.Trim = "" And Me.TextBoxFundNo.Text.Trim = "" Then
                BindGrid(DataGrid_Search, CType(Nothing, DataSet))
                MessageBox.Show(PlaceHolder1, "Death Calculation ", "Please Enter a Search Value. ", MessageBoxButtons.OK)
                Exit Sub
            End If

            'We need to re-initialize the controls on the Details Tab when we reload
            TextBoxCalcDeathDate.Text = ""
            TextBoxDataBaseDeathDate.Text = ""
            ViewState("previousSearchSortExpression") = ""  'NP:PS:2007.09.17 - Reinitialize the Sort Expression.

            'Then process the lookup
            l_dataset_SearchResults = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.LookUp_DeathCalc_MemberListForDeath(Me.TextBoxSSNo.Text.Trim, Me.TextBoxLastName.Text.Trim, Me.TextBoxFirstName.Text.Trim, TextBoxFundNo.Text.Trim)
            'Check if any results were returned??
            If HelperFunctions.isEmpty(l_dataset_SearchResults) Then
                Me.LabelNoDataFound.Visible = True
                Me.TabStripDeathCalc.Items(1).Enabled = False

                BindGrid(DataGrid_Search, l_dataset_SearchResults)
                Exit Sub
            End If
            Me.TabStripDeathCalc.Items(1).Enabled = True
            DataGrid_Search.SelectedIndex = 0
            Me.LabelNoDataFound.Visible = False
            BindGrid(DataGrid_Search, l_dataset_SearchResults)
            'Anudeep A:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for jquery popup
            DataSet_AdditionalForms = Nothing
            '----Shashi Shekhar:2010-02-12: Code to handle Archived Participants from list--------------
            'If the first participants in find list, which is by default selected in this find screen is archived then restrict to go ahead
            'Shashi Shekhar:2010-03-11:Handling usability issue of Data Archive
            Dim l_SSNo As String
            Dim dr As DataRow()
            'Start:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
            l_SSNo = Me.DataGrid_Search.SelectedItem.Cells(DataGrid_Search_ForAll_index.SSNO).Text.Trim
            Me.ParticipantBirthDate = Me.DataGrid_Search.SelectedItem.Cells(DataGrid_Search_ForAll_index.BirthDate).Text.Trim 'MMR | 2020.02.10 | YRS-AT-4770 | Setting participant birth date in property from grid value
            dr = l_dataset_SearchResults.Tables(0).Select("[SS No.]= '" & l_SSNo & "' And FundEventID='" + DataGrid_Search.SelectedItem.Cells(DataGrid_Search_ForAll_index.FundEventId).Text.Trim + "'") 'AA:09.28.2015 YRS-AT-2548 Added fundeventid column to get the exact selected participant details, to avoid multiple fundevents problem and using enum instead of hardcorde values
            'End:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
            ' If Me.DataGrid_Search.SelectedItem.Cells(8).Text.ToUpper.Trim() = "TRUE" Then
            If ((dr(0).Item("IsArchived") = True) And (dr(0).Item("StatusType").trim <> "RD")) Then
                'Shashi Shekhar:2010-04-08: Adding Stauts "DR" also with "RD" for Handling usability issue of Data Archive
                If ((dr(0).Item("IsArchived") = True) And (dr(0).Item("StatusType").trim <> "DR")) Then
                    MessageBox.Show(Me.PlaceHolder1, " YMCA - YRS", "Selected participant's data has been archived, Please unarchive the data to continue the process.", MessageBoxButtons.Stop, False)
                    Me.TabStripDeathCalc.Items(1).Enabled = False

                    Exit Sub
                End If
            End If
            '---------------------------------------------------------------------------------------

            'DataGrid_Search_SelectedIndexChanged(Me, Nothing)
            'NP:IVP1:BT-386:2008.04.16 - Calling ParticipantChanged Function to reset the prompt for active service for ML participants.
            'Since we select the first user by default, we will raise the selectedParticipantChanged event here
            OnSelectedParticipantChanged()
        Catch ex As SqlException
            HelperFunctions.LogException("ButtonFind_Click()", ex)
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            HelperFunctions.LogException("ButtonFind_Click()", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    'NP:IVP1:2008.04.11 - Added event call that is executed when the selected participant changes
    'Any cleanup/initialization code dependent on a participant must be written here.
    Private Sub OnSelectedParticipantChanged()
        l_string_ForceMLComputationAs = ""
    End Sub
#End Region

#Region "Report related code"
    Private Sub ButtonReports_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonReports.Click
        Try
            'By Aparna YRPS -4180 13/12/2007
            ''NP:PS:2007.09.06 - Check if available options have been selected before proceeding further
            'Dim errString As String = checkIfOptionsAreSelected(True)
            'If errString <> String.Empty Then
            '    MessageBox.Show(PlaceHolder1, "Reports Error", errString, MessageBoxButtons.OK)
            '    Exit Sub
            'End If
            ''END Validations - NP:PS:2007.09.06
            'By Aparna YRPS -4180  13/12/2007
            ProcessReport()
        Catch ex As Exception
            HelperFunctions.LogException("ButtonReports_Click()", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    Private Sub ProcessReport()
        Try
            'Report Name = DeathBenefitOptions.rpt
            'Number of Parameters = 1
            'Parameter : PersID
            'Set the Report Variables in Session
            Session("strReportName") = "DeathBenefitOptions"
            Session("PersID") = Me.String_PersId.ToUpper()
            'Call ReportViewer.aspx 
            Me.OpenReportViewer()
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    Private Sub OpenReportViewer()
        Try
            'Call ReportViewer.aspx 
            Dim popupScript2 As String = "<script language='javascript'>" & _
            "window.open('FT\\ReportViewer.aspx', 'ReportPopUp2', " & _
            "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                Page.RegisterStartupScript("PopupScript1", popupScript2)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "Forms related code"
    'Start Commented by Anudeep on 07.12.2012 for yrs 5.01707
    'Private Sub ButtonForms_Click()
    '    Try
    '        ''NP:PS:2007.09.06 - Check if an available option was selected before proceeding
    '        'Dim errString As String = checkIfOptionsAreSelected(True)
    '        'If errString <> String.Empty Then
    '        '    MessageBox.Show(PlaceHolder1, "Forms Error", errString, MessageBoxButtons.OK)
    '        '    Exit Sub
    '        'End If
    '        ''End validation - NP:PS:2007.09.06
    '        'ProcessForms()
    '        ProcessSelectedData()
    '    Catch ex As SqlException
    '        Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    End Try

    'End Sub
    'End Commented by Anudeep on 07.12.2012 for yrs 5.01707
    'SR:2012.12.06 - YRS 5.0-1707-Commented
    'Private Sub ProcessForms()
    '    Dim l_drow_BeneficiaryOptions As DataRow
    '    Dim l_dataRow_BeneficiaryDetail As DataRow
    '    Dim l_dt_RetirementPlan_Options As New DataTable
    '    Dim l_dataset_LookUp_MemberListForDeath As DataSet
    '    Dim l_dataRowMemberList As DataRow
    '    Dim Para1_MemberName, Para2_BeneficiaryName, Para_MemberSSNo As String
    '    Dim l_dataRow_BeneficiaryMaster As DataRow
    '    Dim l_dataRow_BeneficiarySavingsDetail As DataRow
    '    Try
    '        Session("strReportName") = ""
    '        Session("Para1_NameofDeceased") = ""
    '        Session("Para2_NameofClaimant") = ""
    '        Session("Para3_MemberSSNo") = ""
    '        Session("Para3_ReservesAcctName") = ""
    '        Session("Para4_Reserves_NonTaxable") = "0"
    '        Session("Para5_Reserves_Taxable") = "0"
    '        Session("Para6_Reserves_Balance") = "0"

    '        Session("Para7_Savings_ReservesAcctName") = ""
    '        Session("Para8_Savings_NonTaxable") = "0"
    '        Session("Para9_Savings_Taxable") = "0"
    '        Session("Para10_Savings_Balance") = "0"
    '        Session("Para11_DBAcctName") = ""
    '        Session("Para12_DB_NonTaxable") = "0"
    '        Session("Para13_DB_Taxable") = "0"
    '        Session("Para14_DB_Balance") = "0"
    '        Session("Para15_TotalAcctname") = ""
    '        Session("Para16_Total_NonTaxable") = "0"
    '        Session("Para17_Total_Taxable") = "0"
    '        Session("Para18_Total_Balance") = "0"
    '        Session("Para_FundId") = ""

    '        'Get the dataset that we had preserved from Session
    '        l_dataset_LookUp_MemberListForDeath = l_dataset_SearchResults

    '        'Get the Record Selected
    '        '       l_dataRowMemberList = l_dataset_LookUp_MemberListForDeath.Tables("r_MemberListForDeath").Rows(Me.DataGrid_Search.SelectedIndex)
    '        l_dataRowMemberList = l_dataset_LookUp_MemberListForDeath.Tables("r_MemberListForDeath").DefaultView.Item(Me.DataGrid_Search.SelectedIndex).Row

    '        Para1_MemberName = l_dataRowMemberList("First Name").ToString().Trim()
    '        Para1_MemberName += " " + l_dataRowMemberList("Middle Name").ToString().Trim()
    '        Para1_MemberName += " " + l_dataRowMemberList("Last Name").ToString().Trim() + " "
    '        Para_MemberSSNo = Left(l_dataRowMemberList("SS No.").ToString().Trim(), 3) + "-" + l_dataRowMemberList("SS No.").ToString().Substring(3, 2).Trim() + "-" + l_dataRowMemberList("SS No.").ToString().Substring(5, 4).Trim()

    '        ' Table 0 contains BeneficiaryMaster Information
    '        l_dataRow_BeneficiaryMaster = Me.DataSet_ProcessedData.Tables(0).Rows(Me.DataGrid_BeneficiariesList.SelectedIndex)

    '        Para2_BeneficiaryName = l_dataRow_BeneficiaryMaster("First Name").ToString.Trim() + " " + l_dataRow_BeneficiaryMaster("Last Name").ToString.Trim()
    '        'modified by aparna 'Check the report based on radio list 19/12/2007
    '        '  If l_dataRowMemberList("StatusType").ToString().Trim() = "RD" Or l_dataRowMemberList("StatusType").ToString().Trim() = "DR" Then
    '        If rbtnList_MoneySelection.SelectedIndex = RETIRED_FUNDS_INDEX Then
    '            Session("strReportName") = "RetireeDeathBenefitsClaimForm"
    '            'Session("Para1_MemberName") = Para1_MemberName.ToString
    '            'Session("Para2_BeneficiaryName") = Para2_BeneficiaryName.ToString
    '            'Session("Para3_MemberSSNo") = Para_MemberSSNo.ToString
    '            Session("Para1_NameofDeceased") = Para1_MemberName.ToString
    '            Session("Para2_NameofClaimant") = Para2_BeneficiaryName.ToString
    '            Session("Para3_MemberSSNo") = Para_MemberSSNo.ToString
    '            'By Imran on 24/11/2010 - YRS 5.0-1209 / BT-673- Pass new fund id parameter for RetireeDeathBenefitsClaimForm and Pre-RetirementDeathBenefitsClaimForm report
    '            Session("Para_FundId") = l_dataRowMemberList("FundNo")
    '        Else


    '            Session("strReportName") = "Pre-RetirementDeathBenefitsClaimForm"
    '            'Session("Para1_MemberName") = Para1_MemberName.ToString
    '            'Session("Para2_BeneficiaryName") = Para2_BeneficiaryName.ToString
    '            Session("Para1_NameofDeceased") = Para1_MemberName.ToString
    '            Session("Para2_NameofClaimant") = Para2_BeneficiaryName.ToString
    '            'By Imran on 24/11/2010 - YRS 5.0-1209 / BT-673- Pass new fund id parameter for RetireeDeathBenefitsClaimForm and Pre-RetirementDeathBenefitsClaimForm report
    '            Session("Para_FundId") = l_dataRowMemberList("FundNo").ToString().Trim()

    '            'get the other paramteres for the report
    '            l_dt_RetirementPlan_Options = Me.DataSet_ProcessedData.Tables(2).Clone
    '            For Each l_drow_BeneficiaryOptions In Me.DataSet_ProcessedData.Tables(2).Rows
    '                If l_drow_BeneficiaryOptions("BeneficiaryID").ToString().Trim() = Me.DataSet_ProcessedData.Tables(0).Rows(Me.DataGrid_BeneficiariesList.SelectedIndex)("UniqueID").ToString().Trim() Then
    '                    'Add the new Row  into the Beneficiary Options Grid Table
    '                    l_dt_RetirementPlan_Options.ImportRow(l_drow_BeneficiaryOptions)
    '                End If
    '            Next
    '            If l_dt_RetirementPlan_Options.Rows.Count > 0 Then

    '                Dim l_ctr As Integer
    '                Dim l_dt_RetiredMoney_Options As New DataTable
    '                Dim l_dt_ActiveMoney_Options As New DataTable
    '                Dim l_dt_Options As DataTable
    '                Dim l_ds_tempdataset As DataSet
    '                Dim l_str_selectedBeneficiaryId As String
    '                Dim l_str_filterCriteria As String

    '                ' If we have not selected any beneficiary then there is no point in populating this
    '                If Me.DataGrid_BeneficiariesList.SelectedIndex < 0 Then Exit Sub

    '                ' Initialize local variables
    '                l_ds_tempdataset = Me.DataSet_ProcessedData
    '                l_dt_RetiredMoney_Options = l_ds_tempdataset.Tables(1).Copy
    '                l_dt_ActiveMoney_Options = l_ds_tempdataset.Tables(2).Copy

    '                ' Obtain the beneficiary Id of the currently selected Beneficiary from the filtered Beneficiary list
    '                l_str_selectedBeneficiaryId = l_ds_tempdataset.Tables(0).DefaultView.Item(DataGrid_BeneficiariesList.SelectedIndex)("UniqueID").ToString().Trim()
    '                l_str_filterCriteria = "BeneficiaryID='" & l_str_selectedBeneficiaryId & "' " '& l_ds_tempdataset.Tables(0).Rows(Me.DataGrid_BeneficiariesList.SelectedIndex)("UniqueID").ToString().Trim() & "' "
    '                l_dt_Options = IIf(_showRetiredMoneyOptions, l_dt_RetiredMoney_Options, l_dt_ActiveMoney_Options)
    '                ' We want to persist the options available to the current beneficiary
    '                ' Copy the RetirementPlan Datarows and the SavingsPlan Datarows into a new table
    '                l_dt_Options.DefaultView.RowFilter = l_str_filterCriteria
    '                Dim l_datatableBeneficiaryOptions As DataTable = l_dt_Options.Clone()
    '                Dim dr As DataRow
    '                For Each dr In l_dt_Options.Select(l_str_filterCriteria)
    '                    l_datatableBeneficiaryOptions.ImportRow(dr)
    '                Next
    '                l_datatableBeneficiaryOptions.AcceptChanges()

    '                If Me.DataGrid_RetirementPlan_BenefitOptions.SelectedIndex <> -1 Then
    '                    l_datatableBeneficiaryOptions.DefaultView.RowFilter = "PlanType = 'RETIREMENT' OR PlanType is null"
    '                    l_dataRow_BeneficiaryDetail = l_datatableBeneficiaryOptions.DefaultView.Item(Me.DataGrid_RetirementPlan_BenefitOptions.SelectedIndex).Row
    '                    If l_dataRow_BeneficiaryDetail("Lump Sum") > 0 Then
    '                        Me.GeneratparametersforFormButtonReports(l_dataRow_BeneficiaryDetail, l_dataRowMemberList, "RETIREMENT")
    '                    End If

    '                End If

    '                If Me.DataGrid_SavingsPlan_BenefitOptions.SelectedIndex <> -1 Then
    '                    l_datatableBeneficiaryOptions.DefaultView.RowFilter = "PlanType = 'SAVINGS'"
    '                    l_dataRow_BeneficiarySavingsDetail = l_datatableBeneficiaryOptions.DefaultView.Item(Me.DataGrid_SavingsPlan_BenefitOptions.SelectedIndex).Row
    '                    If l_dataRow_BeneficiarySavingsDetail("Lump Sum") > 0 Then
    '                        Me.GeneratparametersforFormButtonReports(l_dataRow_BeneficiarySavingsDetail, l_dataRowMemberList, "SAVINGS")
    '                    End If
    '                End If


    '                Dim l_Total_NontaxableAmount As Decimal = 0
    '                Dim l_Total_TaxableAmount As Decimal = 0
    '                Dim l_Total_Balance As Decimal = 0

    '                'l_Total_NontaxableAmount = CType(IIf(Session("Para4_Reserves_NonTaxable") = String.Empty, "0", Session("Para4_Reserves_NonTaxable")), Decimal) + CType(IIf(Session("Para8_Savings_NonTaxable") = String.Empty, "0", Session("Para8_Savings_NonTaxable")), Decimal) + CType(IIf(Session("Para12_DB_NonTaxable") = String.Empty, "0", Session("Para12_DB_NonTaxable")), Decimal)   commented by SR:2010.06.15 for migration
    '                'l_Total_TaxableAmount = CType(IIf(Session("Para5_Reserves_Taxable") = String.Empty, "0", Session("Para5_Reserves_Taxable")), Decimal) + CType(IIf(Session("Para9_Savings_Taxable") = String.Empty, "0", Session("Para9_Savings_Taxable")), Decimal) + CType(IIf(Session("Para13_DB_Taxable") = String.Empty, "0", Session("Para13_DB_Taxable")), Decimal)   commented by SR:2010.06.15 for migration
    '                'l_Total_Balance = CType(IIf(Session("Para6_Reserves_Balance") = String.Empty, "0", Session("Para6_Reserves_Balance")), Decimal) + CType(IIf(Session("Para10_Savings_Balance") = String.Empty, "0", Session("Para10_Savings_Balance")), Decimal) + CType(IIf(Session("Para14_DB_Balance") = String.Empty, "0", Session("Para14_DB_Balance")), Decimal)  commented by SR:2010.06.15 for migration

    '                l_Total_NontaxableAmount = Decimal.Parse(IIf(Session("Para4_Reserves_NonTaxable") = String.Empty, "0", Session("Para4_Reserves_NonTaxable"))) + Decimal.Parse(IIf(Session("Para8_Savings_NonTaxable") = String.Empty, "0", Session("Para8_Savings_NonTaxable"))) + Decimal.Parse(IIf(Session("Para12_DB_NonTaxable") = String.Empty, "0", Session("Para12_DB_NonTaxable")))
    '                l_Total_TaxableAmount = Decimal.Parse(IIf(Session("Para5_Reserves_Taxable") = String.Empty, "0", Session("Para5_Reserves_Taxable"))) + Decimal.Parse(IIf(Session("Para9_Savings_Taxable") = String.Empty, "0", Session("Para9_Savings_Taxable"))) + Decimal.Parse(IIf(Session("Para13_DB_Taxable") = String.Empty, "0", Session("Para13_DB_Taxable")))
    '                l_Total_Balance = Decimal.Parse(IIf(Session("Para6_Reserves_Balance") = String.Empty, "0", Session("Para6_Reserves_Balance"))) + Decimal.Parse(IIf(Session("Para10_Savings_Balance") = String.Empty, "0", Session("Para10_Savings_Balance"))) + Decimal.Parse(IIf(Session("Para14_DB_Balance") = String.Empty, "0", Session("Para14_DB_Balance")))


    '                Session("Para16_Total_NonTaxable") = l_Total_NontaxableAmount.ToString
    '                Session("Para17_Total_Taxable") = l_Total_TaxableAmount.ToString
    '                Session("Para18_Total_Balance") = l_Total_Balance.ToString
    '                'By Imran on 24/11/2010 - YRS 5.0-1209 / BT-673- Pass new fund id parameter for RetireeDeathBenefitsClaimForm and Pre-RetirementDeathBenefitsClaimForm report
    '                Session("Para_FundId") = l_dataRowMemberList("FundNo")

    '            End If

    '        End If

    '        'NP:2007.12.18 - Removed it outside to ensure it is called for all participants if applicable.
    '        SetupSessionParametersForReports()
    '        'Call the report viewer   
    '        Me.OpenReportViewer()

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub
    'Ends, SR:2012.12.06 - YRS 5.0-1707-Commented

    'NP:YRPS-4121 - Setting the parameter values to string.empty if they are not required
    'This routine sets the amounts to blanks if the heading text is not defined
    Private Sub SetupSessionParametersForReports()
        If Session("Para3_ReservesAcctName") Is Nothing OrElse Session("Para3_ReservesAcctName") = String.Empty Then
            Session("Para4_Reserves_NonTaxable") = ""
            Session("Para5_Reserves_Taxable") = ""
            Session("Para6_Reserves_Balance") = ""
        End If

        If Session("Para7_Savings_ReservesAcctName") Is Nothing OrElse Session("Para7_Savings_ReservesAcctName") = String.Empty Then
            Session("Para8_Savings_NonTaxable") = ""
            Session("Para9_Savings_Taxable") = ""
            Session("Para10_Savings_Balance") = ""
        End If

        If Session("Para11_DBAcctName") Is Nothing OrElse Session("Para11_DBAcctName") = String.Empty Then
            Session("Para12_DB_NonTaxable") = ""
            Session("Para13_DB_Taxable") = ""
            Session("Para14_DB_Balance") = ""
        End If

        If Session("Para15_TotalAcctname") Is Nothing OrElse Session("Para15_TotalAcctname") = String.Empty Then
            Session("Para16_Total_NonTaxable") = ""
            Session("Para17_Total_Taxable") = ""
            Session("Para18_Total_Balance") = ""
        End If
    End Sub

    Private Sub GeneratparametersforFormButtonReports(ByVal l_dataRow_BeneficiaryDetail As DataRow, ByVal l_dataRowMemberList As DataRow, ByVal plantype As String)

        Dim lcOption, lcAccountName1, lcAccountName2, lcAccountNameT As String
        Dim lcNonTax1, lcTaxable1, lcBalance1 As Decimal
        Dim lcNonTax2, lcTaxable2, lcBalance2 As Decimal
        Dim lcNonTaxT, lcTaxableT, lcBalanceT As Decimal

        Try

            'Initialise the variables
            lcOption = ""
            lcAccountName1 = ""
            lcNonTax1 = 0D
            lcTaxable1 = 0D
            lcBalance1 = 0D
            lcAccountName2 = ""
            lcNonTax2 = 0D
            lcTaxable2 = 0D
            lcBalance2 = 0D
            lcAccountNameT = ""
            lcNonTaxT = 0D
            lcTaxableT = 0D
            lcBalanceT = 0D


            'These parameters are required only for Active Participants 
            'by aparna 19/12/2007 this check ont necessary
            ' If Not (l_dataRowMemberList("StatusType").ToString().Trim() = "RD" Or l_dataRowMemberList("StatusType").ToString().Trim() = "DR") Then

            'since calc field was removed from the relevant view - just handling the exception as per discussion with vipul on 23Jun2006
            'lcOption = l_dataRow_BeneficiaryDetail("cOption").ToString().Trim + l_dataRow_BeneficiaryDetail("pOption").ToString().Trim
            Dim l_string_Calc As String
            Try
                l_string_Calc = l_dataRow_BeneficiaryDetail("Calc").ToString().Trim()
            Catch
                l_string_Calc = "2"
            End Try
            'lcOption = l_dataRow_BeneficiaryDetail("Calc").ToString().Trim + l_dataRow_BeneficiaryDetail("Pmt").ToString().Trim
            lcOption = l_string_Calc + l_dataRow_BeneficiaryDetail("Pmt").ToString().Trim
            lcNonTax2 = 0D

            If lcOption = "1B" Or lcOption = "2C" Or lcOption = "3C" Or lcOption = "4C" Then ' && Personal Reserves
                lcAccountName1 = "Reserves"
                lcNonTax1 = IIf(Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPostTax")) > 0, ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPostTax").ToString()), 0D)
                lcTaxable1 = IIf(Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPreTax")) > 0, ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPreTax").ToString()), 0D)
                lcBalance1 = IIf(lcNonTax1 + lcTaxable1 > 0, lcNonTax1 + lcTaxable1, 0D)

            ElseIf lcOption = "2B" Or lcOption = "3B" Then      '	&& Personal Reserves and Death Benefit
                lcAccountName1 = "Reserves"
                lcNonTax1 = IIf(Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPostTax")) > 0, ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPostTax").ToString()), 0D)
                lcTaxable1 = IIf(Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPreTax")) > 0, ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPreTax").ToString()), 0D)
                lcBalance1 = IIf(lcNonTax1 + lcTaxable1 > 0, lcNonTax1 + lcTaxable1, 0D)

                'Important Bug - lnNonTax2 is not defined in the method cmdForms.Click()
                'lcNonTax2 depends upon lnNonTax2 so it will always be blank ... Vipul 15Nov05

                lcAccountName2 = "Death Benefit"
                lcTaxable2 = IIf(Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("Death Benefit")) > 0, ConvertToDecimal(l_dataRow_BeneficiaryDetail("Death Benefit").ToString()), 0D)
                lcBalance2 = IIf(lcTaxable2 + lcNonTax2 > 0, lcTaxable2 + lcNonTax2, 0D)

            ElseIf lcOption = "2D" Or lcOption = "3D" Then          ' && Death Benefit
                lcAccountName2 = "Death Benefit"
                lcTaxable2 = IIf(Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("Death Benefit")) > 0, ConvertToDecimal(l_dataRow_BeneficiaryDetail("Death Benefit").ToString()), 0D)
                lcBalance2 = IIf(lcTaxable2 + lcNonTax2 > 0, lcTaxable2 + lcNonTax2, 0D)

            ElseIf lcOption = "4B" Then
                lcAccountName1 = "Reserves"
                lcNonTax1 = IIf(Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPostTax")) > 0, ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPostTax").ToString()), 0D)
                lcTaxable1 = IIf(Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("Reserves")) - Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPostTax")) + Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("PIA")) > 0, _
                                     Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("Reserves")) - Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPostTax")) + Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("PIA")), 0D)
                lcBalance1 = IIf(lcNonTax1 + lcTaxable1 > 0, lcNonTax1 + lcTaxable1, 0D)
            End If

            lcAccountNameT = "Totals"
            lcNonTaxT = IIf(lcNonTax1 + lcNonTax2 > 0, lcNonTax1 + lcNonTax2, 0D)
            lcTaxableT = IIf(lcTaxable1 + lcTaxable2 > 0, lcTaxable1 + lcTaxable2, 0D)
            lcBalanceT = IIf(lcBalance1 + lcBalance2 > 0, lcBalance1 + lcBalance2, 0D)

            'If death Date is greater than 01July2006, we need to pass different parameters
            '23 Aug 2006 Vipul - Gemini Issue # YREN-2568
            'Re-Initialise the variables if post 01 July 2006
            Dim l_deathDate As Date
            'l_deathDate = Convert.ToDateTime(Me.TextBoxDataBaseDeathDate.Text)
            l_deathDate = Convert.ToDateTime(Me.TextBoxCalcDeathDate.Text)

            'START: SB | YRS-AT-3371 | 2017.06.27 | Death date is compared based on the value from the database instead of hardcoded values
            'If DateDiff("d", "07/01/2006", l_deathDate) > 0 Then
            If DateDiff("d", Me.SavingsPlanStartDate, l_deathDate) > 0 Then
                'END: SB | YRS-AT-3371 | 2017.06.27 | Death date is compared based on the value from the database instead of hardcoded values
                lcNonTax1 = 0D
                lcTaxable1 = 0D
                lcBalance1 = 0D
                lcAccountName2 = ""
                lcNonTax2 = 0D
                lcTaxable2 = 0D
                lcBalance2 = 0D
                lcAccountNameT = ""
                lcNonTaxT = 0D
                lcTaxableT = 0D
                lcBalanceT = 0D

                'lcAccountName1 = "Reserves"
                ''lcNonTax1 = 0  --SR:2009.12.17 FOR YRS 5.0:973
                'lcNonTax1 = IIf(Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPostTax")) > 0, ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPostTax").ToString()), 0D)
                ''COMMENTED BY APARNA post july the column name changed from Personal reserves to Basic reserves and it also include YMCA Amount
                ''lcTaxable1 = Convert.ToString(Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("Personal Reserves")) + Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("YMCA Account")) + Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("Voluntary")) + Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("Death Benefit")))
                'lcTaxable1 = Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("Basic Reserves")) + Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("Voluntary")) + ConvertToDecimal(l_dataRow_BeneficiaryDetail("TD Savings")) + Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("Death Benefit"))

                'lcTaxable1 = IIf(lcTaxable1 > 0, lcTaxable1 - lcNonTax1, 0D) '--SR:2009.12.17 FOR YRS 5.0:973

                'lcBalance1 = IIf(lcNonTax1 + lcTaxable1 > 0, lcNonTax1 + lcTaxable1, 0D)

                'lcAccountNameT = "Totals"
                'lcNonTaxT = IIf(lcNonTax1 + lcNonTax2 > 0, lcNonTax1 + lcNonTax2, 0D)
                'lcTaxableT = IIf(lcTaxable1 + lcTaxable2 > 0, lcTaxable1 + lcTaxable2, 0D)
                'lcBalanceT = IIf(lcBalance1 + lcBalance2 > 0, lcBalance1 + lcBalance2, 0D)


                lcAccountName1 = "Reserves"
                lcNonTax1 = Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("PersonalPostTax").ToString())
                lcTaxable1 = Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("Basic Reserves")) + Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("Voluntary")) + ConvertToDecimal(l_dataRow_BeneficiaryDetail("TD Savings")) + Me.ConvertToDecimal(l_dataRow_BeneficiaryDetail("Death Benefit"))
                lcTaxable1 = IIf(lcTaxable1 > 0, lcTaxable1 - lcNonTax1, 0D) '--SR:2009.12.17 FOR YRS 5.0:973

                lcBalance1 = IIf(lcNonTax1 + lcTaxable1 > 0, lcNonTax1 + lcTaxable1, 0D)


                '--SR:2010.05.13 FOR YRS 5.0 1077

                If (lcNonTax1 < 0) Then
                    lcTaxable1 = lcTaxable1 + lcNonTax1
                    If (lcTaxable1 < 0) Then
                        lcTaxable1 = 0
                    End If
                    lcNonTax1 = 0
                End If

                If (lcTaxable1 < 0) Then
                    lcNonTax1 = lcNonTax1 + lcTaxable1
                    If (lcNonTax1 < 0) Then
                        lcNonTax1 = 0
                    End If
                    lcTaxable1 = 0
                End If

                '------ end,SR:2010.05.13 FOR YRS 5.0 1077


                lcAccountNameT = "Totals"
                lcNonTaxT = IIf(lcNonTax1 + lcNonTax2 > 0, lcNonTax1 + lcNonTax2, 0D)
                lcTaxableT = IIf(lcTaxable1 + lcTaxable2 > 0, lcTaxable1 + lcTaxable2, 0D)
                lcBalanceT = IIf(lcBalance1 + lcBalance2 > 0, lcBalance1 + lcBalance2, 0D)
            End If
            '  End If


            If plantype = "RETIREMENT" Then
                Session("Para3_ReservesAcctName") = "Reserves"  'for Retirement Plan
                Session("Para4_Reserves_NonTaxable") = lcNonTax1.ToString
                Session("Para5_Reserves_Taxable") = lcTaxable1.ToString
                Session("Para6_Reserves_Balance") = lcBalance1.ToString
            ElseIf plantype = "SAVINGS" Then
                Session("Para7_Savings_ReservesAcctName") = "Reserve for savings plan" '"Reserves - TD Savings Plan"
                Session("Para8_Savings_NonTaxable") = lcNonTax1.ToString
                Session("Para9_Savings_Taxable") = lcTaxable1.ToString
                Session("Para10_Savings_Balance") = lcBalance1.ToString
            Else
                Session("Para3_ReservesAcctName") = "Reserves"
                Session("Para4_Reserves_NonTaxable") = lcNonTax1.ToString
                Session("Para5_Reserves_Taxable") = lcTaxable1.ToString
                Session("Para6_Reserves_Balance") = lcBalance1.ToString
            End If

            Session("Para11_DBAcctName") = lcAccountName2.ToString
            Session("Para12_DB_NonTaxable") = lcNonTax2.ToString
            Session("Para13_DB_Taxable") = lcTaxable2.ToString
            Session("Para14_DB_Balance") = lcBalance2.ToString
            Session("Para15_TotalAcctname") = lcAccountNameT.ToString
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub EnableDisableFormsButton()
        ' This function enables or disables the form button which is used 
        ' if a lumpsum option is selected
        Dim l_dataset_LookUp_MemberListForDeath As DataSet
        Dim l_dataRowMemberList, l_dataRow_BeneficiaryDetail As DataRow
        Dim RetirementPlanRows As DataRow()
        Dim SavingsPlanRows As DataRow()
        Dim dt As DataTable = Session("SelectedBeneficiaryDetails")
        Dim blnShowForm_Activemoney As Boolean
        Dim blnShowForm_Retiredmoney As Boolean
        Try
            HelperFunctions.LogMessage("Begin EnableDisableFormsButton() method")
            'commented by anudeep
            'If DataGrid_BeneficiariesList_ForAll.SelectedIndex > -1 Then
            '    If DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(6).Text.ToString.Trim() = "SETTLD" Then
            '        ButtonForms.Disabled = True
            '        Exit Sub
            '    End If
            'End If

            ''SR:2012.10.07 - YRS 5.0-1707 - Coding
            Dim dtRetiredMoneyOptions As DataTable = Session("SelectedBeneficiaryDetails_ForRetired")
            Dim dtActiveMoneyOptions As DataTable = Session("SelectedBeneficiaryDetails_ForActive")



            'NP:IVP2:2008.09.29 - In some cases the datatable is not available. In that case the button should be disabled.
            'Anudeep:25.03.2013-Bt1303:YRS 5.0-1707:New Death Benefit Application form  To show the Button Form,On selecting Annuitiy beneficiary 
            If HelperFunctions.isEmpty(dtRetiredMoneyOptions) And HelperFunctions.isEmpty(dtActiveMoneyOptions) And (DataGridAnnuities.SelectedIndex < 0) Then
                'Anudeep A:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for jquery popup Buttonforms changed from Asp button to html button
                ButtonForms.Disabled = True
                Exit Sub
            End If

            'Get the dataset that we had preserved from Session
            l_dataset_LookUp_MemberListForDeath = l_dataset_SearchResults
            'Get the Record Selected
            l_dataRowMemberList = l_dataset_LookUp_MemberListForDeath.Tables("r_MemberListForDeath").DefaultView.Item(Me.DataGrid_Search.SelectedIndex).Row

            'If death Date is not null and Options were selected which have lumpsum values then enable the forms button
            'Anudeep A:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for jquery popup Buttonforms changed from Asp button to html button
            ButtonForms.Disabled = True 'NP:PS:2007.12.18 - Setting value to False. The forms button is always enabled if person is dead

            'BY Aparna YRPS -4121 13/12/2007
            If l_dataRowMemberList.IsNull("DeathDate") = True Then
                'If (l_dataRowMemberList("StatusType").ToString().Trim() = "AE" OrElse l_dataRowMemberList("StatusType").ToString().Trim() = "RA") Then
                'Anudeep A:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for jquery popup Buttonforms changed from Asp button to html button
                ButtonForms.Disabled = True
            Else
                'Anudeep A:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for jquery popup Buttonforms changed from Asp button to html button
                ButtonForms.Disabled = False
            End If
            ''Ends, SR:2012.10.07 - YRS 5.0-1707 - Coding
            'Anudeep
            If dtActiveMoneyOptions.Rows.Count > 0 Then
                If dtActiveMoneyOptions.Rows(0).Item("Status").ToString.Trim() = "SETTLD" Then
                    blnShowForm_Activemoney = True
                End If
            Else
                blnShowForm_Activemoney = True
            End If

            If dtRetiredMoneyOptions.Rows.Count > 0 Then
                If dtRetiredMoneyOptions.Rows(0).Item("Status").ToString.Trim() = "SETTLD" Then
                    blnShowForm_Retiredmoney = True
                End If
            Else
                blnShowForm_Retiredmoney = True
            End If

            'Anudeep:25.03.2013-Bt1303:YRS 5.0-1707:New Death Benefit Application form 
            ' To show the 'Button Form',On selecting annuitiy beneficiary 
            'Conditions when 'Button Forms' is disabled
            'When a selected death beneficiary has settld pr does not have money in any plan
            'and annuity beneficiary is not selected 
            If (blnShowForm_Activemoney And blnShowForm_Retiredmoney) And (DataGridAnnuities.SelectedIndex < 0) Then
                ButtonForms.Disabled = True
            End If
            HelperFunctions.LogMessage("Completed EnableDisableFormsButton() method")
        Catch
            Throw
        End Try
    End Sub
#End Region

    Private Sub ButtonOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
        Try
            ''SR:2012.12.10 - YRS 5.0-1707 - Clear all session
            ClearSession()
            ''End, SR:2012.12.10 - YRS 5.0-1707 - Clear all session
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try

    End Sub

    Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            Me.TextBoxFirstName.Text = ""
            Me.TextBoxLastName.Text = ""
            Me.TextBoxSSNo.Text = ""
            Me.TextBoxFundNo.Text = ""  'NP:PS:2007.09.27 - Clearing Fund Number box.

            Me.LabelNoDataFound.Visible = False
            BindGrid(DataGrid_Search, CType(Nothing, DataSet))
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            Session("DeathCalc_Sort") = Nothing
            Response.Redirect("MainWebForm.aspx", False)
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub
    'SR:2012.12.06 - YRS 5.0-1707 - need to remove this function 
    'Private Function checkIfOptionsAreSelected(Optional ByVal returnErrorIfNoOptionsAvailable As Boolean = True) As String
    '    Dim l_dataset_LookUp_MemberListForDeath As DataSet
    '    Dim l_dataRowMemberList, l_dataRow_BeneficiaryDetail As DataRow
    '    Dim RetirementPlanRows As DataRow()
    '    Dim SavingsPlanRows As DataRow()
    '    Dim dt As DataTable = Session("SelectedBeneficiaryDetails")
    '    Dim returnvalue As String = String.Empty

    '    Try
    '        'SR:2012.10.07- YRS 5.0-1707
    '        Dim dtSelRetiredMoneyOptions As DataTable = Session("SelectedBeneficiaryDetails_ForRetired")
    '        Dim dtSelActiveMoneyOptions As DataTable = Session("SelectedBeneficiaryDetails_ForActive")
    '        'Get the dataset that we had preserved from Session
    '        Dim RetirementPlanRows_ForRetired As DataRow()
    '        Dim SavingsPlanRows_ForRetired As DataRow()
    '        Dim RetirementPlanRows_ForActive As DataRow()
    '        Dim SavingsPlanRows_ForActive As DataRow()

    '        'Get the dataset that we had preserved from Session
    '        l_dataset_LookUp_MemberListForDeath = l_dataset_SearchResults
    '        'Get the Record Selected
    '        l_dataRowMemberList = l_dataset_LookUp_MemberListForDeath.Tables("r_MemberListForDeath").Rows(Me.DataGrid_Search.SelectedIndex)

    '        RetirementPlanRows_ForRetired = dtSelRetiredMoneyOptions.Select("PlanType='RETIREMENT' or PlanType is null")
    '        SavingsPlanRows_ForRetired = dtSelRetiredMoneyOptions.Select("PlanType='SAVINGS'")
    '        RetirementPlanRows_ForActive = dtSelActiveMoneyOptions.Select("PlanType='RETIREMENT' or PlanType is null")
    '        SavingsPlanRows_ForActive = dtSelActiveMoneyOptions.Select("PlanType='SAVINGS'")

    '        If RetirementPlanRows_ForRetired.Length = 0 AndAlso SavingsPlanRows_ForRetired.Length = 0 AndAlso RetirementPlanRows_ForActive.Length = 0 AndAlso SavingsPlanRows_ForActive.Length = 0 Then
    '            If returnErrorIfNoOptionsAvailable Then returnvalue = "No benefit option available to process form with."
    '            Return returnvalue
    '        End If
    '        If RetirementPlanRows_ForRetired.Length > 0 AndAlso DataGrid_RetirementPlan_BenefitOptions_ForRetired.SelectedIndex < 0 Then
    '            returnvalue = "Please select an benefit option from the grid to generate the Form"
    '            Return returnvalue
    '        End If
    '        If SavingsPlanRows_ForRetired.Length > 0 AndAlso DataGrid_SavingsPlan_BenefitOptions_ForRetired.SelectedIndex < 0 Then
    '            returnvalue = "Please select an benefit option from the TD savings grid to generate the Form"
    '            Return returnvalue
    '        End If
    '        If RetirementPlanRows_ForActive.Length > 0 AndAlso DataGrid_RetirementPlan_BenefitOptions_ForActive.SelectedIndex < 0 Then
    '            returnvalue = "Please select an benefit option from the grid to generate the Form"
    '            Return returnvalue
    '        End If
    '        If SavingsPlanRows_ForActive.Length > 0 AndAlso DataGrid_SavingsPlan_BenefitOptions_ForActive.SelectedIndex < 0 Then
    '            returnvalue = "Please select an benefit option from the TD savings grid to generate the Form"
    '            Return returnvalue
    '        End If
    '        'Ends,SR:2012.10.07- YRS 5.0-1707
    '        Return returnvalue
    '    Catch
    '        Throw
    '    End Try
    'End Function
    'This Utility section works as a charm
#Region "General Utility Functions"
    'dg = The datagrid to bind data to
    'ds = The dataset which contains the data
    'forceVisible = Whether the datagrid should be displayed if it does not contain any data
    Private Sub BindGrid(ByRef dg As DataGrid, ByRef ds As DataSet, Optional ByVal forceVisible As Boolean = False)
        Try
            If ds Is Nothing OrElse ds.Tables.Count = 0 OrElse ds.Tables(0).Rows.Count = 0 Then
                dg.DataSource = Nothing
                dg.DataBind()
                dg.Visible = forceVisible
                Exit Sub
            Else
                dg.DataSource = ds.Tables(0)
                dg.DataBind()
                dg.Visible = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub BindGrid(ByRef dg As DataGrid, ByRef dv As DataView, Optional ByVal forceVisible As Boolean = False)
        Try
            If dv Is Nothing OrElse dv.Count = 0 Then
                dg.DataSource = Nothing
                dg.DataBind()
                dg.Visible = forceVisible
                Exit Sub
            Else
                dg.DataSource = dv
                dg.DataBind()
                dg.Visible = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Function ConvertToDecimal(ByVal strNumber As String) As Decimal
        'This function is written to handle empty string conversion to decimal
        Dim strNumberToReturn As Decimal
        Try
            strNumberToReturn = Convert.ToDecimal(strNumber.Trim)
        Catch ex As Exception
            strNumberToReturn = 0
        End Try

        Return strNumberToReturn
    End Function
#End Region

#Region "Local Variable Persistence mechanism"
    Protected Overrides Function SaveViewState() As Object
        l_int_SelectedDataGridItem = DataGrid_Search.SelectedIndex
        Dim al As New ArrayList
        al.Add(StoreLocalVariablesToCache())
        al.Add(MyBase.SaveViewState())
        Return al
    End Function

    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        'Dim al As ArrayList = CType(savedState, ArrayList) commented by SR:2010.06.15 for migration
        Dim al As ArrayList = DirectCast(savedState, ArrayList)
        InitializeLocalVariablesFromCache(al.Item(0))
        MyBase.LoadViewState(al.Item(1))
    End Sub

    Private Sub InitializeLocalVariablesFromCache(ByRef obj As Object)
        Try
            'Session: Load data from Session so that it is accessible on Postback - This can be replaced by a load from viewstate or database
            l_dataset_SearchResults = Session("DC_l_dataset_SearchResults")
            l_dataset_Beneficiaries = Session("DC_l_dataset_Beneficiaries")
            l_dataset_SettlementOption_RetirementPlan = Session("DC_l_dataset_SettlementOption_RetirementPlan")
            l_dataset_SettlementOption_SavingsPlan = Session("DC_l_dataset_SettlementOption_SavingsPlan")
            l_int_SelectedDataGridItem = Session("DC_l_int_SelectedDataGridItem")
            'Dim al As ArrayList = CType(obj, ArrayList) commented by SR:2010.06.15 for migration
            Dim al As ArrayList = DirectCast(obj, ArrayList)
            l_string_ForceMLComputationAs = al.Item(0)
            l_string_PageOperation = al.Item(1)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function StoreLocalVariablesToCache() As Object
        Try
            'Session: Pass data into Session so that it is accessible on Postback - This can be replaced by a save to viewstate
            Session("DC_l_dataset_SearchResults") = l_dataset_SearchResults
            Session("DC_l_dataset_Beneficiaries") = l_dataset_Beneficiaries
            Session("DC_l_dataset_SettlementOption_RetirementPlan") = l_dataset_SettlementOption_RetirementPlan
            Session("DC_l_dataset_SettlementOption_SavingsPlan") = l_dataset_SettlementOption_SavingsPlan
            Session("DC_l_int_SelectedDataGridItem") = l_int_SelectedDataGridItem
            Dim al As ArrayList = New ArrayList
            al.Add(l_string_ForceMLComputationAs)
            al.Add(l_string_PageOperation)
            Return al
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
    'Start:Anudeep:07.08.2013 YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
    Private Sub DataGrid_BeneficiariesList_ForAll_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid_BeneficiariesList_ForAll.ItemCommand
        If e.CommandName = "RePrint" Then
            'Start:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
            If e.Item.Cells(DataGrid_BeneficiariesList_ForAll_index.VIEW_PRINT_FORM).Controls.Count > 1 Then
                Session("strReportName") = "Death Benefit Application"
                Session("intDBAppFormID") = e.Item.Cells(DataGrid_BeneficiariesList_ForAll_index.Re_GenerateForm).Text
                OpenReportViewer()
            End If
            'End:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring

        End If
    End Sub

    Private Sub DataGrid_BeneficiariesList_ForAll_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid_BeneficiariesList_ForAll.ItemDataBound
        Try
            'e.Item.Cells(1).Visible = False
            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                'Start:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                If String.IsNullOrEmpty(e.Item.Cells(DataGrid_BeneficiariesList_ForAll_index.Re_GenerateForm).Text) Or e.Item.Cells(DataGrid_BeneficiariesList_ForAll_index.Re_GenerateForm).Text = "&nbsp;" Then
                    e.Item.Cells(DataGrid_BeneficiariesList_ForAll_index.DueSince).Text = "N\A"
                    e.Item.Cells(DataGrid_BeneficiariesList_ForAll_index.VIEW_PRINT_FORM).Enabled = False
                End If
                'End:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
            End If
        Catch
            Throw
        End Try
    End Sub
    'End:Anudeep:07.08.2013 Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.

    Private Sub DataGrid_BeneficiariesList_ForAll_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid_BeneficiariesList_ForAll.PreRender
        Dim dgi As DataGridItem
        Dim rBtn As ImageButton
        Try
            Dim dg As DataGrid = DirectCast(sender, DataGrid) '  Changed(CType to directcast) by SR:2010.06.15 for migration
            For Each dgi In dg.Items
                rBtn = dgi.Cells(DataGrid_BeneficiariesList_ForAll_index.Select_Image).FindControl("ImagebuttonGrid1") 'AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                If Not rBtn Is Nothing Then
                    If dgi.ItemType = ListItemType.SelectedItem Then
                        rBtn.ImageUrl = SELECTED_IMAGE_BUTTON_URL
                    Else
                        rBtn.ImageUrl = NORMAL_IMAGE_BUTTON_URL
                    End If
                End If
            Next
        Catch ex As SqlException
            HelperFunctions.LogException("DataGrid_BeneficiariesList_ForAll_PreRender()", ex)
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            HelperFunctions.LogException("DataGrid_BeneficiariesList_ForAll_PreRender()", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    Private Sub DataGrid_BeneficiariesList_ForAll_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid_BeneficiariesList_ForAll.SelectedIndexChanged
        Try
            'SR:2012.10.29 - YRS 5.0-1707
            DataGrid_RetirementPlan_BenefitOptions_ForRetired.SelectedIndex = -1
            DataGrid_SavingsPlan_BenefitOptions_ForRetired.SelectedIndex = -1
            DataGrid_RetirementPlan_BenefitOptions_ForActive.SelectedIndex = -1
            DataGrid_SavingsPlan_BenefitOptions_ForActive.SelectedIndex = -1
            DataGridAnnuities.SelectedIndex = -1
            'Ends,SR:2012.10.29 - YRS 5.0-1707
            PopulateBeneficiaryOptionsGrid()
            AdditionalForms()
            DeathCalc_AnnuityJointSurviour = Nothing 'AA:08.12.2014- BT:2460:YRS 5.0-2331 - Added below line to clear the annutiy joint surviour session because death beneficiary is selected
        Catch ex As SqlException
            If sender Is Me Then
                HelperFunctions.LogException("DataGrid_BeneficiariesList_ForAll_SelectedIndexChanged()", ex)
                Throw ex
            Else
                HelperFunctions.LogException("DataGrid_BeneficiariesList_ForAll_SelectedIndexChanged()", ex)
                Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            End If
        Catch ex As Exception
            If sender Is Me Then
                HelperFunctions.LogException("DataGrid_BeneficiariesList_ForAll_SelectedIndexChanged()", ex)
                Throw ex
            Else
                HelperFunctions.LogException("DataGrid_BeneficiariesList_ForAll_SelectedIndexChanged()", ex)
                Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            End If
        End Try
    End Sub

    'SR:2012.11.20 - YRS 5.0-1707 - Added
    Public Sub LoadJSAnnuities(ByVal l_string_FundEventId As String)
        Dim l_string_PersId As String
        Dim l_string_FundId As String
        Dim dsJSAnnuity As DataSet
        Dim dtJSAnnuity As DataTable
        Dim l_string_AnnuityId As String
        Dim l_datarow As DataRow()
        Dim l_string_AnnuityJointSurvivorsID As String = String.Empty
        Dim l_dataset_JSannuities As DataSet
        Try
            HelperFunctions.LogMessage("Begin LoadJSAnnuities() method")
            l_dataset_JSannuities = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.PopulateJSAnnuities(l_string_FundEventId)
            Session("JSAnnuities") = l_dataset_JSannuities.Tables(0)
            dtJSAnnuity = l_dataset_JSannuities.Tables(0)
            If (dtJSAnnuity.Rows.Count > 0) Then
                DataGridAnnuities.DataSource = dtJSAnnuity
                DataGridAnnuities.DataBind()
                Me.DataGridAnnuities.SelectedIndex = -1
            End If
            HelperFunctions.LogMessage("Completed LoadJSAnnuities() method")
        Catch
            Throw
        End Try
    End Sub
    'Start:Anudeep:07.08.2013 Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
    Private Sub DataGridAnnuities_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridAnnuities.ItemCommand
        If e.CommandName = "RePrint" Then
            'Start:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
            If e.Item.Cells(DataGridAnnuities_index.VIEW_PRINT_FORM).Controls.Count > 1 Then
                Session("strReportName") = "Death Benefit Application"
                Session("intDBAppFormID") = e.Item.Cells(DataGridAnnuities_index.Re_GenerateForm).Text
                OpenReportViewer()
            End If
            'End:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
        End If
    End Sub

    Private Sub DataGridAnnuities_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridAnnuities.ItemDataBound
        Try
            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
                'Start:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                If String.IsNullOrEmpty(e.Item.Cells(DataGridAnnuities_index.Re_GenerateForm).Text) Or e.Item.Cells(DataGridAnnuities_index.Re_GenerateForm).Text = "&nbsp;" Then
                    e.Item.Cells(DataGridAnnuities_index.DatDueSince).Text = "N\A"
                    e.Item.Cells(DataGridAnnuities_index.VIEW_PRINT_FORM).Enabled = False
                End If
                'End:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
            End If
        Catch
            Throw
        End Try
    End Sub
    'End:Anudeep:07.08.2013 Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
    Private Sub DataGridAnnuities_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridAnnuities.PreRender
        Dim dgi As DataGridItem
        Dim rBtn As ImageButton
        Try
            Dim dg As DataGrid = DirectCast(sender, DataGrid) '  Changed(CType to directcast) by SR:2010.06.15 for migration
            For Each dgi In dg.Items
                rBtn = dgi.Cells(DataGridAnnuities_index.Select_Image).FindControl("Imagebutton6") 'AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                If Not rBtn Is Nothing Then
                    If dgi.ItemType = ListItemType.SelectedItem Then
                        rBtn.ImageUrl = SELECTED_IMAGE_BUTTON_URL
                    Else
                        rBtn.ImageUrl = NORMAL_IMAGE_BUTTON_URL
                    End If
                End If
            Next
        Catch ex As SqlException
            HelperFunctions.LogException("DataGridAnnuities_PreRender()", ex)
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            HelperFunctions.LogException("DataGridAnnuities_PreRender()", ex)
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    Private Sub DataGridAnnuities_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridAnnuities.SelectedIndexChanged
        Try
            DataGrid_BeneficiariesList_ForAll.SelectedIndex = -1
            Me.EnableDisableFormsButton()
            HandleDisplay()
            'Start:Anudeep:25.03.2013 - Bt1303:YRS 5.0-1707:New Death Benefit Application form 
            'DataGrid_RetirementPlan_BenefitOptions_ForActive.Visible = False
            'DataGrid_RetirementPlan_BenefitOptions_ForRetired.Visible = False
            'DataGrid_SavingsPlan_BenefitOptions_ForActive.Visible = False
            'DataGrid_SavingsPlan_BenefitOptions_ForRetired.Visible = False
            'lbl_RetirementPlanOptions_ForRetired.Text = "No Options available under Retirement plan"
            'lbl_SavingsPlanOptions_ForRetired.Text = "No Options available under TD Savings plan"
            'lbl_RetirementPlanOptions_ForActive.Text = "No Options available under Retirement plan"
            'lbl_SavingsPlanOptions_ForActive.Text = "No Options available under TD Savings plan"

            'To show Death Benefit Options if annuity beneficiary is also death beneficiary
            PopulateBeneficiaryOptionsGrid()
            EnableDisableFormsButton()
            'End:Anudeep:25.03.2013 - Bt1303:YRS 5.0-1707:New Death Benefit Application form 
            'Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up 
            AdditionalForms()
            DeathCalc_AnnuityJointSurviour = "True" 'AA:08.12.2014- BT:2460:YRS 5.0-2331 - Added below line to set the annutiy joint surviour session because annuity joint surviour is selected
        Catch ex As Exception
            HelperFunctions.LogException("DataGrid_BeneficiariesList_ForAll_SelectedIndexChanged()", ex)
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub
    'End, SR:2012.11.20 - YRS 5.0-1707 - Added
    'SP 2014.12.04 BT-2310\YRS 5.0-2255:Added new parameter "dsBeneRepDetails"
    Public Function ProcessSelectedData(ByVal blnCopyIDM As Boolean, ByVal blnFollowUp As Boolean, ByVal dsBeneRepDetails As DataSet) As String
        Dim l_dataset_LookUp_MemberListForDeath As DataSet
        Dim l_dataRowMemberList As DataRow()
        Dim dtSelRetiredMoneyOptions As DataTable = Session("SelectedBeneficiaryDetails_ForRetired")
        Dim dtSelActiveMoneyOptions As DataTable = Session("SelectedBeneficiaryDetails_ForActive")
        Dim dtSelJSAnnuityOptions As DataTable = Session("JSAnnuities")
        Dim RetirementPlanRows_ForRetired As DataRow()
        Dim SavingsPlanRows_ForRetired As DataRow()
        Dim RetirementPlanRows_ForActive As DataRow()
        Dim SavingsPlanRows_ForActive As DataRow()
        'START - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
        Dim drInsuredReserverAmtRetirementPlanRows_ForRetired As DataRow()
        Dim drInsuredReserverAmtSavingsPlanRows_ForRetired As DataRow()
        'END - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
        Dim JSAnnuitiesRows As DataRow()
        Dim strReturnString As String
        Try
            HelperFunctions.LogMessage("Begin routine ProcessSelectedData()")
            If Me.DataGrid_BeneficiariesList_ForAll.SelectedIndex < 0 And DataGridAnnuities.SelectedIndex < 0 Then Exit Function
            'Get the dataset that we had preserved from Session
            l_dataset_LookUp_MemberListForDeath = l_dataset_SearchResults
            'Get the Record Selected
            'Anudeep:Bt-1303:YRS 5.0-1707:New Death Benefit Application form 
            'Changed to get selected participant details from search grid
            'l_dataRowMemberList = l_dataset_LookUp_MemberListForDeath.Tables("r_MemberListForDeath").Rows(Me.DataGrid_Search.SelectedIndex)
            If DataGrid_Search.SelectedIndex > -1 Then
                l_dataRowMemberList = l_dataset_LookUp_MemberListForDeath.Tables("r_MemberListForDeath").Select("[SS No.]=" + DataGrid_Search.SelectedItem.Cells(DataGrid_Search_ForAll_index.SSNO).Text + " And FundEventID='" + DataGrid_Search.SelectedItem.Cells(DataGrid_Search_ForAll_index.FundEventId).Text.Trim + "'") 'AA:09.28.2015 YRS-AT-2548 Added fundeventid column to get the exact selected participant details, to avoid multiple fundevents problem and using enum instead of hardcorde values
            End If
            'Anudeep:15.12.2012 Code added to save only Unsetteled data 
            RetirementPlanRows_ForRetired = dtSelRetiredMoneyOptions.Select("(PlanType='RETIREMENT' or PlanType is null) And Status = 'Unsettled'")
            SavingsPlanRows_ForRetired = dtSelRetiredMoneyOptions.Select("PlanType='SAVINGS'  And Status = 'Unsettled'")
            RetirementPlanRows_ForActive = dtSelActiveMoneyOptions.Select("(PlanType='RETIREMENT' or PlanType is null)  And Status = 'Unsettled'")
            SavingsPlanRows_ForActive = dtSelActiveMoneyOptions.Select("PlanType='SAVINGS'  And Status = 'Unsettled'")
            'START - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
            drInsuredReserverAmtRetirementPlanRows_ForRetired = dtSelRetiredMoneyOptions.Select("(PlanType='RETIREMENT' or PlanType is null) And Status = 'Unsettled' And BeneficiaryTypeCode IN('INSRES','RETIRE')")
            drInsuredReserverAmtSavingsPlanRows_ForRetired = dtSelRetiredMoneyOptions.Select("PlanType='SAVINGS' And Status = 'Unsettled'")
            'END - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
            JSAnnuitiesRows = dtSelJSAnnuityOptions.Select()
            strReturnString = SaveBeneficiaryOptions(l_dataRowMemberList(0), RetirementPlanRows_ForRetired, SavingsPlanRows_ForRetired, RetirementPlanRows_ForActive, SavingsPlanRows_ForActive, JSAnnuitiesRows, blnCopyIDM, blnFollowUp, dsBeneRepDetails, drInsuredReserverAmtRetirementPlanRows_ForRetired, drInsuredReserverAmtSavingsPlanRows_ForRetired)
            HelperFunctions.LogMessage("Function ProcessSelectedData() completed successfully")
            Return strReturnString
        Catch ex As Exception
            Throw
        End Try
    End Function

    'SP 2014.12.04 BT-2310\YRS 5.0-2255:Added new parameter "dsBeneRepDetails"
    Public Function SaveBeneficiaryOptions(ByVal l_dataRowMemberList As DataRow, ByVal RetirementPlanRows_ForRetired As DataRow(), ByVal SavingsPlanRows_ForRetired As DataRow(), ByVal RetirementPlanRows_ForActive As DataRow(), ByVal SavingsPlanRows_ForActive As DataRow(), ByVal JSAnnuitiesRows As DataRow(), ByVal blnCopyIDM As Boolean, ByVal blnFollowUp As Boolean, ByVal dsBeneRepDetails As DataSet, ByVal drInsuredReserverAmtRetirementPlanRows_ForRetired As DataRow(), ByVal drInsuredReserverAmtSavingsPlanRows_ForRetired As DataRow()) As String
        Dim strdecsdPersID As String
        Dim strdecsdFundeventID As String
        Dim StrBeneficiaryName As String
        Dim strBeneficiarySSNo As String
        Dim strBeneficiaryType As String
        Dim decJSAnnuityAmount As Decimal
        Dim decRetPlan As Decimal
        Dim decPrincipalGuaranteeAnnuity_RP As Decimal
        Dim decSavPlan As Decimal
        Dim decPrincipalGuaranteeAnnuity_SP As Decimal
        Dim decDeathBenefit As Decimal
        Dim decAnnuityMFromRP As Decimal
        Dim decFirstMAnnuityFromRP As Decimal
        Dim decAnnuityCFromRP As Decimal
        Dim decFirstCAnnuityFromRP As Decimal
        Dim decLumpSumFromNonHumanBen As Decimal
        Dim decAnnuityMFromSP As Decimal
        Dim decFirstMAnnuityFromSP As Decimal
        Dim decAnnuityCFromSP As Decimal
        Dim decFirstCAnnuityFromSP As Decimal
        Dim decAnnuityMFromRDB As Decimal
        Dim decFirstMAnnuityFromRDB As Decimal
        Dim decAnnuityFromJSAndRDB As Decimal
        Dim decFirstAnnuityFromJSAndRDB As Decimal
        Dim decAnnuityMFromResRemainingOfRP As Decimal
        Dim decFirstMAnnuityFromResRemainingOfRP As Decimal
        Dim decAnnuityCFromResRemainingOfRP As Decimal
        Dim decFirstCAnnuityFromResRemainingOfRP As Decimal
        Dim decAnnuityMFromResRemainingOfSP As Decimal
        Dim decFirstMAnnuityFromResRemainingOfSP As Decimal
        Dim decAnnuityCFromResRemainingOfSP As Decimal
        Dim decFirstCAnnuityFromResRemainingOfSP As Decimal
        Dim intMonths As Integer
        Dim blnActiveDeathBenfit As Boolean = False
        Dim blnSection1Visible As Boolean = False
        Dim blnSection2Visible As Boolean = False
        Dim blnSection3Visible As Boolean = False
        Dim blnSection4Visible As Boolean = False
        Dim blnSection5Visible As Boolean = False
        Dim blnSection6Visible As Boolean = False
        Dim blnSection7Visible As Boolean = False
        Dim blnSection8Visible As Boolean = False
        Dim blnSection9Visible As Boolean = False
        Dim blnSection10Visible As Boolean = False
        Dim blnSection11Visible As Boolean = False
        Dim blnSection12Visible As Boolean = False
        Dim strReturnStatus As String
        Dim l_ds_tempdataset As DataSet
        Dim BasicReserves_RetPlan As DataRow()
        Dim BasicReserves_SavPlan As DataRow()
        'SR:2013.01.07 - YRS 5.0-1707: Either section 7 or section 8 should be visible for a benficiary.
        Dim blnJSBeneficiaryExist As Boolean = False
        'Anudeep:225.03.2013:Bt-1303-YRS 5.0-1707:New Death Benefit Application form 
        Dim blnShowBenefitOptions As Boolean = False
        Dim strAddressID As String
        Dim strBeneficiaryFirstName As String
        Dim strBeneficiaryLastName As String
        Dim dsBenAddress As New DataSet

        'SP 2014.12.04 BT-2310\YRS 5.0-2255: Start
        Dim strRepFirstName As String
        Dim strRepLastName As String
        Dim strRepSalutation As String
        Dim strRepTelephone As String
        'SP 2014.12.04 BT-2310\YRS 5.0-2255: End
        'Start:AA::10.12.2015 YRS-AT-2478
        Dim decRetTaxable As Decimal
        Dim decRetNonTaxable As Decimal
        Dim decSavTaxable As Decimal
        Dim decSavNonTaxable As Decimal
        'End:AA::10.12.2015 YRS-AT-2478
        'START - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
        Dim decPrincipalGuaranteeAnnuityRetTaxableAmt As Decimal
        Dim decPrincipalGuaranteeAnnuityRetNonTaxableAmt As Decimal
        Dim decPrincipalGuaranteeAnnuitySavTaxableAmt As Decimal
        Dim decPrincipalGuaranteeAnnuitySavNonTaxableAmt As Decimal
        'END - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
        'START: MMR | 2020.02.10 | YRS-AT-4770 | Added local variables for Secure act rule
        Dim isSecureActApplicable As Boolean
        Dim beneficiaryBirthDate As String
        Dim beneficiaryRelationshipCode As String
        Dim beneficiaryDetailsTable As DataTable
        Dim participantDeathDate As Date
        'END: MMR | 2020.02.10 | YRS-AT-4770 | Added local variables for Secure act rule
        Try
            HelperFunctions.LogMessage("Begin function SaveBeneficiaryOptions()")
            Dim dtSelBasicReservesOptions As DataTable = Session("SelectedBeneficiaryDetails_ForBasicReserves")
            BasicReserves_RetPlan = dtSelBasicReservesOptions.Select("PlanType='RETIREMENT' or PlanType is null")
            BasicReserves_SavPlan = dtSelBasicReservesOptions.Select("PlanType='SAVINGS'")
            l_ds_tempdataset = Me.DataSet_ProcessedData
            strdecsdPersID = l_dataRowMemberList("PersID").ToString().Trim
            strdecsdFundeventID = l_dataRowMemberList("FundEventID").ToString().Trim
            strAddressID = Session("DeathCalc_BenAddressId")
            If DataGridAnnuities.SelectedIndex > -1 Then
                'Start:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                StrBeneficiaryName = IIf(DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(DataGridAnnuities_index.FirstName).Text.ToString().Trim() = "&nbsp;", "", DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(DataGridAnnuities_index.FirstName).Text.ToString().Trim()) + " " + IIf(DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(DataGridAnnuities_index.LastName).Text.ToString().Trim() = "&nbsp;", "", DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(DataGridAnnuities_index.LastName).Text.ToString().Trim())
                strBeneficiaryFirstName = IIf(DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(DataGridAnnuities_index.FirstName).Text.ToString().Trim() = "&nbsp;", "", DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(DataGridAnnuities_index.FirstName).Text.ToString().Trim())
                strBeneficiaryLastName = IIf(DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(DataGridAnnuities_index.LastName).Text.ToString().Trim() = "&nbsp;", "", DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(DataGridAnnuities_index.LastName).Text.ToString().Trim())
                'StartAnudeep:25.03.2013:Bt-1303-YRS 5.0-1707:New Death Benefit Application form 
                strBeneficiaryType = ""
                strBeneficiarySSNo = DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(DataGridAnnuities_index.SSN).Text.ToString().Trim()
                'End:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                'Checking if annuity beneficiary is also a death beneficiary
                'If yes then displays death benefit options in death benefit application form
                'If no it will not display benefit options
                For iCount As Integer = 0 To DataGrid_BeneficiariesList_ForAll.Items.Count - 1
                    If strBeneficiarySSNo = DataGrid_BeneficiariesList_ForAll.Items(iCount).Cells(DataGrid_BeneficiariesList_ForAll_index.SSN).Text.ToString.Trim() Then 'AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                        blnShowBenefitOptions = True
                        'it is used for displaying either section 7 or section 8
                        blnJSBeneficiaryExist = True
                        Exit For
                    End If
                Next
                'End-Anudeep:25.03.2013:Bt-1303-YRS 5.0-1707:New Death Benefit Application form 
                'Get JS annuity Amount
                'If Not JSAnnuitiesRows Is Nothing Then
                '    For Each dr As DataRow In JSAnnuitiesRows
                '        decJSAnnuityAmount = decJSAnnuityAmount + DataGridAnnuities.Items(DataGridAnnuities.SelectedIndex).Cells(6).Text.ToString().Trim()
                '    Next
                'Else

                'Anudeep:25.03.2013:Bt-1303-YRS 5.0-1707:New Death Benefit Application form 
                'Displaying the total Annuity amount exist for user
                If JSAnnuitiesRows.Length > 0 Then
                    For Each dr As DataRow In JSAnnuitiesRows
                        decJSAnnuityAmount = decJSAnnuityAmount + dr("Output_txtTotalMonthlyAnnuityAmount")
                        'Either section 7 or section 8 should be visible for a benficiary.
                        blnJSBeneficiaryExist = True
                    Next
                End If

                'End If
            ElseIf DataGrid_BeneficiariesList_ForAll.SelectedIndex > -1 Then
                'StrBeneficiaryName = l_ds_tempdataset.Tables(0).DefaultView.Item(DataGrid_BeneficiariesList_ForAll.SelectedIndex)("FirstName").ToString().Trim() + " " + l_ds_tempdataset.Tables(0).DefaultView.Item(DataGrid_BeneficiariesList_ForAll.SelectedIndex)("LastName").ToString().Trim()
                'strBeneficiarySSNo = l_ds_tempdataset.Tables(0).DefaultView.Item(DataGrid_BeneficiariesList_ForAll.SelectedIndex)("SSNO").ToString().Trim()
                'StrBeneficiaryID = l_ds_tempdataset.Tables(0).DefaultView.Item(DataGrid_BeneficiariesList_ForAll.SelectedIndex)("BeneficiaryID").ToString().Trim()
                'strBeneficiaryType = l_ds_tempdataset.Tables(0).DefaultView.Item(DataGrid_BeneficiariesList_ForAll.SelectedIndex)("RelationShip").ToString().Trim()
                'Start:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                StrBeneficiaryName = IIf(DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.FirstName).Text.ToString.Trim() = "&nbsp;", "", DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.FirstName).Text.ToString.Trim()) + " " + IIf(DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.LastName).Text.ToString.Trim() = "&nbsp;", "", DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.LastName).Text.ToString.Trim())
                strBeneficiaryFirstName = IIf(DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.FirstName).Text.ToString.Trim() = "&nbsp;", "", DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.FirstName).Text.ToString.Trim())
                strBeneficiaryLastName = IIf(DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.LastName).Text.ToString.Trim() = "&nbsp;", "", DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.LastName).Text.ToString.Trim())
                strBeneficiarySSNo = DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.SSN).Text.ToString.Trim()
                strBeneficiaryType = DataGrid_BeneficiariesList_ForAll.Items(DataGrid_BeneficiariesList_ForAll.SelectedIndex).Cells(DataGrid_BeneficiariesList_ForAll_index.RelationShip).Text.ToString.Trim()
                'End:AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                'Get JS annuity Amount
                If Not JSAnnuitiesRows Is Nothing Then
                    For Each dr As DataRow In JSAnnuitiesRows
                        If strBeneficiarySSNo = dr("Output_txtSSN") Then
                            decJSAnnuityAmount = decJSAnnuityAmount + dr("Output_txtTotalMonthlyAnnuityAmount")
                            'SR:2013.01.07 - YRS 5.0-1707: Either section 7 or section 8 should be visible for a benficiary.
                            blnJSBeneficiaryExist = True
                        End If
                    Next
                    'Else
                    '    If Me.DataGridAnnuities.SelectedIndex >= 0 Then
                    '        decJSAnnuityAmount = decJSAnnuityAmount + CType(l_ds_tempdataset.Tables(3).DefaultView.Item(DataGridAnnuities.SelectedIndex)("Output_txtTotalMonthlyAnnuityAmount").ToString().Trim(), Decimal)
                    '    End If
                End If
                'Anudeep:25.03.2013:Bt-1303-YRS 5.0-1707:New Death Benefit Application form 
                blnShowBenefitOptions = True
            End If


            'Anudeep:25.03.2013:Bt-1303-YRS 5.0-1707:New Death Benefit Application form 
            If blnShowBenefitOptions Then
                intMonths = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetPayrollMonthsSinceDeath(CType(Me.TextBoxCalcDeathDate.Text, System.DateTime))
                'Get decRetPlan Amount
                'If Not RetirementPlanRows_ForRetired Is Nothing Then
                '    For Each dr As DataRow In RetirementPlanRows_ForRetired
                '        If (dr("pmt") = "A" Or dr("pmt") = "D") And dr("Reserves") <> 0 Then
                '            decRetPlan = decRetPlan + dr("Reserves")
                '        End If
                '    Next
                'Else
                '    If Not BasicReserves_RetPlan Is Nothing Then
                '        For Each drBR As DataRow In BasicReserves_RetPlan
                '            decRetPlan = decRetPlan + drBR("BasicReserve")
                '        Next
                '    End If
                'End If
                If Not RetirementPlanRows_ForActive Is Nothing Then
                    For Each dr As DataRow In RetirementPlanRows_ForActive
                        If Not String.IsNullOrEmpty(dr("Total").ToString()) Then
                            If (dr("Death Benefit") <> 0) Then
                                decRetPlan = decRetPlan + dr("Total")
                                blnActiveDeathBenfit = True
                                Exit For
                            ElseIf (dr("pmt") = "A" And dr("Total") <> 0) Then
                                decRetPlan = decRetPlan + dr("Total")
                                Exit For
                            End If
                        End If
                    Next
                    'Anudeep:15.12.2012 Code added to save only Unsetteled data 
                    If decRetPlan = 0 And RetirementPlanRows_ForActive.Length > 0 Then
                        If Not BasicReserves_RetPlan Is Nothing Then
                            For Each drBR As DataRow In BasicReserves_RetPlan
                                If Not String.IsNullOrEmpty(drBR("BasicReserve").ToString()) Then
                                    If (drBR("BasicReserve")) > 0 Then 'AA | 2015.10.14 | YRS-AT-2553 : Handled for the multiple records exist in retirement plan
                                        decRetPlan = decRetPlan + drBR("BasicReserve")
                                        Exit For
                                    End If
                                End If

                            Next
                        End If
                    End If
                End If

                'Get decPrincipalGuaranteeAnnuity_RP Amount
                Select Case l_dataRowMemberList(6).ToString().Trim
                    Case "DD", "RA", "RT", "RE", "RDNP", "RPT", "RD", "DR", "RP", "DQ", "RQ", "RQTA"
                        If Not RetirementPlanRows_ForRetired Is Nothing Then
                            For Each dr As DataRow In RetirementPlanRows_ForRetired
                                If Not String.IsNullOrEmpty(dr("Reserves").ToString()) Then
                                    If dr("Reserves") > 0 Then  'AA | 2015.10.14 | YRS-AT-2553Handled for the multiple records exist in retirement plan
                                        decPrincipalGuaranteeAnnuity_RP = decPrincipalGuaranteeAnnuity_RP + dr("Reserves")
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                        'If Not RetirementPlanRows_ForActive Is Nothing Then
                        '    For Each dr As DataRow In RetirementPlanRows_ForActive
                        '        If dr("pmt") = "A" And dr("Annuity C") <> 0 Then
                        '            decPrincipalGuaranteeAnnuity_RP = decPrincipalGuaranteeAnnuity_RP + dr("Lump Sum")
                        '        End If
                        '    Next
                        'End If
                End Select

                'Get decSavPlan Amount
                'If Not SavingsPlanRows_ForRetired Is Nothing Then
                '    For Each dr As DataRow In SavingsPlanRows_ForRetired
                '        If (dr("pmt") = "B" Or dr("pmt") = "C") And dr("Lump Sum") <> 0 Then
                '            decSavPlan = decSavPlan + dr("Lump Sum")
                '        End If
                '    Next
                'Else
                '    If Not BasicReserves_SavPlan Is Nothing Then
                '        For Each drBR As DataRow In BasicReserves_RetPlan
                '            decSavPlan = decSavPlan + drBR("BasicReserve")
                '        Next
                '    End If

                'End If

                If Not SavingsPlanRows_ForActive Is Nothing Then
                    For Each dr As DataRow In SavingsPlanRows_ForActive
                        If Not String.IsNullOrEmpty(dr("Total").ToString()) Then
                            If (dr("Death Benefit") <> 0) Then
                                decSavPlan = decSavPlan + dr("Total")
                                Exit For
                            ElseIf (dr("pmt") = "A" And dr("Total") <> 0) Then
                                decSavPlan = decSavPlan + dr("Total")
                                Exit For
                            End If
                        End If
                    Next
                    'Anudeep:15.12.2012 Code added to save only Unsetteled data 
                    If decSavPlan = 0 And SavingsPlanRows_ForActive.Length > 0 Then
                        If Not BasicReserves_SavPlan Is Nothing Then
                            For Each drBR As DataRow In BasicReserves_SavPlan
                                If Not String.IsNullOrEmpty(drBR("BasicReserve").ToString()) Then
                                    If (drBR("BasicReserve") > 0) Then 'AA | 2015.10.14 | YRS-AT-2553 : Handled for the multiple records exist in retirement plan
                                        decSavPlan = decSavPlan + drBR("BasicReserve")
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If

                'get decPrincipalGuaranteeAnnuity_SavingPlan
                Select Case l_dataRowMemberList(6).ToString().Trim
                    Case "DD", "RA", "RT", "RE", "RDNP", "RPT", "RD", "DR", "RP", "DQ", "RQ", "RQTA"
                        If Not SavingsPlanRows_ForRetired Is Nothing Then
                            For Each dr As DataRow In SavingsPlanRows_ForRetired
                                If Not String.IsNullOrEmpty(dr("Reserves").ToString()) Then
                                    If dr("Reserves") > 0 Then  'AA | 2015.10.14 | YRS-AT-2553 - Handled for the multiple records exist in Savings plan
                                        decPrincipalGuaranteeAnnuity_SP = decPrincipalGuaranteeAnnuity_SP + dr("Reserves")
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                        'If Not SavingsPlanRows_ForActive Is Nothing Then
                        '    For Each dr As DataRow In SavingsPlanRows_ForActive
                        '        If (dr("pmt") = "A" Or dr("pmt") = "D") And dr("Annuity C") <> 0 Then
                        '            decPrincipalGuaranteeAnnuity_SP = decPrincipalGuaranteeAnnuity_SP + dr("Death Benefit")
                        '        End If
                        '    Next
                        'End If
                End Select

                'Get decDeathBenefit
                Select Case l_dataRowMemberList(6).ToString().Trim
                    Case "DD", "RA", "RT", "RE", "RDNP", "RPT", "RD", "DR", "RP", "DQ", "RQ", "RQTA"
                        'If Not RetirementPlanRows_ForActive Is Nothing Then
                        '    For Each dr As DataRow In RetirementPlanRows_ForActive
                        '        If (dr("pmt") = "A" And dr("Annuity C") <> 0) Or (dr("pmt") = "D") Then
                        '            decDeathBenefit = decDeathBenefit + dr("Death Benefit")
                        '        End If
                        '    Next
                        'End If
                        If Not RetirementPlanRows_ForRetired Is Nothing Then
                            For Each dr As DataRow In RetirementPlanRows_ForRetired
                                If Not String.IsNullOrEmpty(dr("Death Benefit").ToString()) Then
                                    If dr("Death Benefit") > 0 Then  'AA | 2015.10.14 | YRS-AT-2553 - Handled for the multiple records exist in Retirement plan
                                        decDeathBenefit = decDeathBenefit + dr("Death Benefit")
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                        If Not SavingsPlanRows_ForRetired Is Nothing Then
                            For Each dr As DataRow In SavingsPlanRows_ForRetired
                                If Not String.IsNullOrEmpty(dr("Death Benefit").ToString()) Then
                                    If dr("Death Benefit") > 0 Then   'AA | 2015.10.14 | YRS-AT-2553 - Handled for the multiple records exist in Savings plan
                                        decDeathBenefit = decDeathBenefit + dr("Death Benefit")
                                        Exit For
                                    End If
                                End If
                            Next

                        End If
                End Select

                'Get decAnnuityMFromRP
                If l_dataRowMemberList(6).ToString().Trim <> "RD" And l_dataRowMemberList(6).ToString().Trim <> "DR" And l_dataRowMemberList(6).ToString().Trim <> "RP" And l_dataRowMemberList(6).ToString().Trim <> "DQ" And l_dataRowMemberList(6).ToString().Trim <> "RQ" And l_dataRowMemberList(6).ToString().Trim <> "RQTA" Then
                    'If Not RetirementPlanRows_ForRetired Is Nothing Then
                    '    For Each dr As DataRow In RetirementPlanRows_ForRetired
                    '    If (dr("pmt") = "A" Or dr("pmt") = "D") And (dr("Annuity M") <> 0) Then
                    '        decAnnuityMFromRP = decAnnuityMFromRP + dr("Annuity M")
                    '    End If
                    '    Next
                    'End If
                    If Not RetirementPlanRows_ForActive Is Nothing Then
                        For Each dr As DataRow In RetirementPlanRows_ForActive
                            If Not String.IsNullOrEmpty(dr("Annuity M").ToString()) Then
                                If (dr("pmt") = "A" Or dr("pmt") = "D") And (dr("Annuity M") <> 0) Then
                                    decAnnuityMFromRP = decAnnuityMFromRP + dr("Annuity M")
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                End If

                'Get decFirstMAnnuityFromRP
                decFirstMAnnuityFromRP = decAnnuityMFromRP * intMonths

                'Get decAnnuityCFromRP
                If l_dataRowMemberList(6).ToString().Trim <> "RD" And l_dataRowMemberList(6).ToString().Trim <> "DR" And l_dataRowMemberList(6).ToString().Trim <> "RP" And l_dataRowMemberList(6).ToString().Trim <> "DQ" And l_dataRowMemberList(6).ToString().Trim <> "RQ" And l_dataRowMemberList(6).ToString().Trim <> "RQTA" Then
                    'If Not RetirementPlanRows_ForRetired Is Nothing Then
                    '    For Each dr As DataRow In RetirementPlanRows_ForRetired
                    '        If (dr("pmt") = "A" Or dr("pmt") = "D") And (dr("Annuity C") <> 0) Then
                    '            decAnnuityCFromRP = decAnnuityCFromRP + dr("Annuity C")
                    '        End If
                    '    Next
                    'End If
                    If Not RetirementPlanRows_ForActive Is Nothing Then
                        For Each dr As DataRow In RetirementPlanRows_ForActive
                            If Not String.IsNullOrEmpty(dr("Annuity C").ToString()) Then
                                If (dr("pmt") = "A" Or dr("pmt") = "D") And (dr("Annuity C") <> 0) Then
                                    decAnnuityCFromRP = decAnnuityCFromRP + dr("Annuity C")
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                End If

                'Get decFirstCAnnuityFromRP
                decFirstCAnnuityFromRP = decAnnuityCFromRP * intMonths

                'Get decLumpSumFromNonHumanBen(ES,IN,TR)
                If (strBeneficiaryType = "ES" Or strBeneficiaryType = "IN" Or strBeneficiaryType = "TR") Then
                    If Not RetirementPlanRows_ForRetired Is Nothing Then
                        For Each dr As DataRow In RetirementPlanRows_ForRetired
                            If Not String.IsNullOrEmpty(dr("Lump Sum").ToString()) Then
                                If dr("pmt") = "B" And dr("Lump Sum") <> 0 Then
                                    decLumpSumFromNonHumanBen = decLumpSumFromNonHumanBen + dr("Lump Sum")
                                    'Exit For  'AA | 2015.10.14 | YRS-AT-2553 - commented to include multiple records exist in retirement plan
                                End If
                            End If
                        Next
                    End If

                    If Not RetirementPlanRows_ForActive Is Nothing Then
                        For Each dr As DataRow In RetirementPlanRows_ForActive
                            'Start:AA:2015.10.09 YRS-AT-2553 : Changed to add retplan money for lumpsum
                            If Not String.IsNullOrEmpty(decRetPlan.ToString()) Then
                                If dr("pmt") = "B" And decRetPlan <> 0 Then
                                    'decLumpSumFromNonHumanBen = decLumpSumFromNonHumanBen + dr("Lump Sum") 
                                    decLumpSumFromNonHumanBen = decLumpSumFromNonHumanBen + decRetPlan
                                    Exit For
                                End If
                            End If
                            'End:AA:2015.10.09 YRS-AT-2553 : Changed to add retplan money for lumpsum

                        Next
                    End If

                    If Not SavingsPlanRows_ForRetired Is Nothing Then
                        For Each dr As DataRow In SavingsPlanRows_ForRetired
                            If Not String.IsNullOrEmpty(dr("Lump Sum").ToString()) Then
                                If dr("pmt") = "B" And dr("Lump Sum") <> 0 Then
                                    decLumpSumFromNonHumanBen = decLumpSumFromNonHumanBen + dr("Lump Sum")
                                    'Exit For
                                End If
                            End If
                        Next
                    End If
                    If Not SavingsPlanRows_ForActive Is Nothing Then
                        For Each dr As DataRow In SavingsPlanRows_ForActive
                            'Start:AA:2015.10.09 YRS-AT-2553 : Changed to add retplan money for lumpsum
                            If Not String.IsNullOrEmpty(decSavPlan.ToString()) Then
                                If dr("pmt") = "B" And decSavPlan <> 0 Then
                                    'decLumpSumFromNonHumanBen = decLumpSumFromNonHumanBen + dr("Lump Sum") --SR/2015.09.14
                                    decLumpSumFromNonHumanBen = decLumpSumFromNonHumanBen + decSavPlan
                                    Exit For
                                End If
                            End If
                            'End:AA:2015.10.09 YRS-AT-2553 : Changed to add retplan money for lumpsum
                        Next
                    End If
                End If

                ' Get decAnnuityMFromSP
                If l_dataRowMemberList(6).ToString().Trim <> "RD" And l_dataRowMemberList(6).ToString().Trim <> "DR" And l_dataRowMemberList(6).ToString().Trim <> "RP" And l_dataRowMemberList(6).ToString().Trim <> "DQ" And l_dataRowMemberList(6).ToString().Trim <> "RQ" And l_dataRowMemberList(6).ToString().Trim <> "RQTA" Then
                    'If Not SavingsPlanRows_ForRetired Is Nothing Then
                    '    For Each dr As DataRow In SavingsPlanRows_ForRetired
                    '        If (dr("pmt") = "A" Or dr("pmt") = "D") And (dr("Annuity M") <> 0) Then
                    '            decAnnuityMFromSP = decAnnuityMFromSP + dr("Annuity M")
                    '        End If
                    '    Next
                    'End If
                    If Not SavingsPlanRows_ForActive Is Nothing Then
                        For Each dr As DataRow In SavingsPlanRows_ForActive
                            If Not String.IsNullOrEmpty(dr("Annuity M").ToString()) Then
                                If (dr("pmt") = "A" Or dr("pmt") = "D") And (dr("Annuity M") <> 0) Then
                                    decAnnuityMFromSP = decAnnuityMFromSP + dr("Annuity M")
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                End If

                'get decFirstMAnnuityFromSP
                decFirstMAnnuityFromSP = decAnnuityMFromSP * intMonths

                'Get decAnnuityCFromSP
                If l_dataRowMemberList(6).ToString().Trim <> "RD" And l_dataRowMemberList(6).ToString().Trim <> "DR" And l_dataRowMemberList(6).ToString().Trim <> "RP" And l_dataRowMemberList(6).ToString().Trim <> "DQ" And l_dataRowMemberList(6).ToString().Trim <> "RQ" And l_dataRowMemberList(6).ToString().Trim <> "RQTA" Then
                    'If Not SavingsPlanRows_ForRetired Is Nothing Then
                    '    For Each dr As DataRow In SavingsPlanRows_ForRetired
                    '        If (dr("pmt") = "A" Or dr("pmt") = "D") And (dr("Annuity C") <> 0) Then
                    '            decAnnuityCFromSP = decAnnuityCFromSP + dr("Annuity C")
                    '        End If
                    '    Next
                    'End If
                    If Not SavingsPlanRows_ForActive Is Nothing Then
                        For Each dr As DataRow In SavingsPlanRows_ForActive
                            If Not String.IsNullOrEmpty(dr("Annuity C").ToString()) Then
                                If (dr("pmt") = "A" Or dr("pmt") = "D") And (dr("Annuity C") <> 0) Then
                                    decAnnuityCFromSP = decAnnuityCFromSP + dr("Annuity C")
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                End If

                'get decFirstCAnnuityFromSP
                decFirstCAnnuityFromSP = decAnnuityCFromSP * intMonths

                'Get decAnnuityMFromRDB
                Select Case l_dataRowMemberList(6).ToString().Trim
                    Case "DD", "RA", "RT", "RE", "RDNP", "RPT", "RD", "DR", "RP", "DQ", "RQ", "RQTA"
                        'If Not RetirementPlanRows_ForActive Is Nothing Then
                        '    For Each dr As DataRow In RetirementPlanRows_ForActive
                        '        If (dr("pmt") = "A" Or dr("pmt") = "C") And (dr("Annuity M") <> 0) Then
                        '            decAnnuityMFromRDB = decAnnuityMFromRDB + dr("Annuity M")
                        '        End If
                        '    Next
                        'End If
                        'changed by anudeep in observation
                        If Not RetirementPlanRows_ForRetired Is Nothing Then
                            For Each dr As DataRow In RetirementPlanRows_ForRetired
                                If Not String.IsNullOrEmpty(dr("Annuity M").ToString()) Then
                                    If (dr("pmt") = "C") And (dr("Annuity M") <> 0) Then
                                        decAnnuityMFromRDB = decAnnuityMFromRDB + dr("Annuity M")
                                        Exit For
                                    ElseIf (dr("pmt") = "A" And dr("Reserves") = 0 And dr("Annuity M") <> 0 And dr("Death Benefit") >= 5000) Then
                                        decAnnuityMFromRDB = decAnnuityMFromRDB + dr("Annuity M")
                                        Exit For
                                    End If
                                End If

                            Next
                        End If
                        'changed by anudeep in observation
                        If Not SavingsPlanRows_ForRetired Is Nothing Then
                            For Each dr As DataRow In SavingsPlanRows_ForRetired
                                If Not String.IsNullOrEmpty(dr("Annuity M").ToString()) Then
                                    If (dr("pmt") = "C") And (dr("Annuity M") <> 0) Then
                                        decAnnuityMFromRDB = decAnnuityMFromRDB + dr("Annuity M")
                                        Exit For
                                    ElseIf (dr("pmt") = "A" And dr("Reserves") = 0 And dr("Annuity M") <> 0 And dr("Death Benefit") >= 5000) Then
                                        decAnnuityMFromRDB = decAnnuityMFromRDB + dr("Annuity M")
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                End Select

                'Get decFirstMAnnuityFromRDB
                decFirstMAnnuityFromRDB = decAnnuityMFromRDB * intMonths

                'Get decAnnuityFromJSAndRDB
                Select Case l_dataRowMemberList(6).ToString().Trim
                    Case "DD", "RA", "RT", "RE", "RDNP", "RPT", "RD", "DR", "RP", "DQ", "RQ", "RQTA"
                        If Not RetirementPlanRows_ForRetired Is Nothing Then
                            For Each dr As DataRow In RetirementPlanRows_ForRetired
                                If Not String.IsNullOrEmpty(dr("Annuity M").ToString()) Then
                                    If (dr("pmt") = "C") And (dr("Annuity M") <> 0) Then
                                        decAnnuityFromJSAndRDB = decAnnuityFromJSAndRDB + dr("Annuity M")
                                        Exit For
                                    ElseIf (dr("pmt") = "A" And dr("Reserves") = 0 And dr("Annuity M") <> 0 And dr("Death Benefit") >= 5000) Then
                                        decAnnuityFromJSAndRDB = decAnnuityFromJSAndRDB + dr("Annuity M")
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                        If Not SavingsPlanRows_ForRetired Is Nothing Then
                            For Each dr As DataRow In SavingsPlanRows_ForRetired
                                If Not String.IsNullOrEmpty(dr("Annuity M").ToString()) Then
                                    If (dr("pmt") = "C") And (dr("Annuity M") <> 0) Then
                                        decAnnuityFromJSAndRDB = decAnnuityFromJSAndRDB + dr("Annuity M")
                                        Exit For
                                    ElseIf (dr("pmt") = "A" And dr("Reserves") = 0 And dr("Annuity M") <> 0 And dr("Death Benefit") >= 5000) Then
                                        decAnnuityFromJSAndRDB = decAnnuityFromJSAndRDB + dr("Annuity M")
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                        decAnnuityFromJSAndRDB = decAnnuityFromJSAndRDB + decJSAnnuityAmount
                End Select

                'Get decFirstAnnuityFromJSAndRDB
                decFirstAnnuityFromJSAndRDB = decAnnuityFromJSAndRDB * intMonths

                'Get decAnnuityMFromResRemainingOfRP
                Select Case l_dataRowMemberList(6).ToString().Trim
                    Case "DD", "RA", "RT", "RE", "RDNP", "RPT", "RD", "DR", "RP", "DQ", "RQ", "RQTA"
                        If Not RetirementPlanRows_ForRetired Is Nothing Then
                            For Each dr As DataRow In RetirementPlanRows_ForRetired
                                If Not String.IsNullOrEmpty(dr("Annuity M").ToString()) Then
                                    If (dr("pmt") = "D" And dr("Annuity M") <> 0) Then
                                        decAnnuityMFromResRemainingOfRP = decAnnuityMFromResRemainingOfRP + dr("Annuity M")
                                        Exit For
                                    ElseIf (dr("pmt") = "A" And dr("Death Benefit") = 0 And dr("Reserves") > 5000 And dr("Annuity M") <> 0) Then
                                        decAnnuityMFromResRemainingOfRP = decAnnuityMFromResRemainingOfRP + dr("Annuity M")
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                        'If Not RetirementPlanRows_ForActive Is Nothing Then
                        '    For Each dr As DataRow In RetirementPlanRows_ForActive
                        '    If (dr("pmt") = "D") And (dr("Annuity M") <> 0) Then
                        '        decAnnuityMFromResRemainingOfRP = decAnnuityMFromResRemainingOfRP + dr("Annuity M")
                        '    End If
                        '    Next
                        'End If
                End Select

                'decFirstMAnnuityFromResRemainingOfRP
                decFirstMAnnuityFromResRemainingOfRP = decAnnuityMFromResRemainingOfRP * intMonths

                'Get decAnnuityCFromResRemainingOfRP
                Select Case l_dataRowMemberList(6).ToString().Trim
                    Case "DD", "RA", "RT", "RE", "RDNP", "RPT", "RD", "DR", "RP", "DQ", "RQ", "RQTA"
                        If Not RetirementPlanRows_ForRetired Is Nothing Then
                            For Each dr As DataRow In RetirementPlanRows_ForRetired
                                If Not String.IsNullOrEmpty(dr("Annuity C").ToString()) Then
                                    If (dr("pmt") = "D" And dr("Annuity C") <> 0) Then
                                        decAnnuityCFromResRemainingOfRP = decAnnuityCFromResRemainingOfRP + dr("Annuity C")
                                        Exit For
                                    ElseIf (dr("pmt") = "A" And dr("Death Benefit") = 0 And dr("Reserves") > 5000 And dr("Annuity C") <> 0) Then
                                        decAnnuityCFromResRemainingOfRP = decAnnuityCFromResRemainingOfRP + dr("Annuity C")
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                        'If Not RetirementPlanRows_ForActive Is Nothing Then
                        '    For Each dr As DataRow In RetirementPlanRows_ForActive
                        '        If (dr("pmt") = "A" Or dr("pmt") = "D") And (dr("Annuity C") <> 0) Then
                        '            decAnnuityCFromResRemainingOfRP = decAnnuityCFromResRemainingOfRP + dr("Annuity M")
                        '        End If
                        '    Next
                        'End If
                End Select

                'decFirstCAnnuitySection12
                decFirstCAnnuityFromResRemainingOfRP = decAnnuityCFromResRemainingOfRP * intMonths

                'Get decAnnuityMFromResRemainingOfSP
                Select Case l_dataRowMemberList(6).ToString().Trim
                    Case "DD", "RA", "RT", "RE", "RDNP", "RPT", "RD", "DR", "RP", "DQ", "RQ", "RQTA"
                        If Not SavingsPlanRows_ForRetired Is Nothing Then
                            For Each dr As DataRow In SavingsPlanRows_ForRetired
                                If Not String.IsNullOrEmpty(dr("Annuity M").ToString()) Then
                                    If (dr("pmt") = "D" And dr("Annuity M") <> 0) Then
                                        decAnnuityMFromResRemainingOfSP = decAnnuityMFromResRemainingOfSP + dr("Annuity M")
                                        Exit For
                                    ElseIf (dr("pmt") = "A" And dr("Death Benefit") = 0 And dr("Reserves") > 5000 And dr("Annuity M") <> 0) Then
                                        decAnnuityMFromResRemainingOfSP = decAnnuityMFromResRemainingOfSP + dr("Annuity M")
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                        'If Not SavingsPlanRows_ForActive Is Nothing Then
                        '    For Each dr As DataRow In SavingsPlanRows_ForActive
                        '        If (dr("pmt") = "A" Or dr("pmt") = "D") And (dr("Annuity M") <> 0) Then
                        '            decAnnuityMFromResRemainingOfSP = decAnnuityMFromResRemainingOfSP + dr("Annuity M")
                        '        End If
                        '    Next
                        'End If
                End Select

                'decAnnuityMFromResRemainingOfSP
                decFirstMAnnuityFromResRemainingOfSP = decAnnuityMFromResRemainingOfSP * intMonths

                'Get decAnnuityCFromResRemainingOfSP
                Select Case l_dataRowMemberList(6).ToString().Trim
                    Case "DD", "RA", "RT", "RE", "RDNP", "RPT", "RD", "DR", "RP", "DQ", "RQ", "RQTA"
                        If Not SavingsPlanRows_ForRetired Is Nothing Then
                            For Each dr As DataRow In SavingsPlanRows_ForRetired
                                If Not String.IsNullOrEmpty(dr("Annuity C").ToString()) Then
                                    If (dr("pmt") = "D" And dr("Annuity C") <> 0) Then
                                        decAnnuityCFromResRemainingOfSP = decAnnuityCFromResRemainingOfSP + dr("Annuity C")
                                        Exit For
                                    ElseIf (dr("pmt") = "A" And dr("Death Benefit") = 0 And dr("Reserves") > 5000 And dr("Annuity C") <> 0) Then
                                        decAnnuityCFromResRemainingOfSP = decAnnuityCFromResRemainingOfSP + dr("Annuity C")
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                        'If Not SavingsPlanRows_ForActive Is Nothing Then
                        '    For Each dr As DataRow In SavingsPlanRows_ForActive
                        '    If (dr("pmt") = "D") And (dr("Annuity C") <> 0) Then
                        '        decAnnuityCFromResRemainingOfSP = decAnnuityCFromResRemainingOfSP + dr("Annuity C")
                        '    End If
                        '    Next
                        'End If
                End Select

                'decFirstCAnnuityFromResRemainingOfSP
                decFirstCAnnuityFromResRemainingOfSP = decAnnuityCFromResRemainingOfSP * intMonths
                'Start Commented by - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Only Active money will Inserted into Colummn of mnyRetTaxableAmt ,mnyRetNontaxableAmt,mnySavTaxableAmt ,mnySavNontaxableAmt of AtsDeathBenefitApplicationForm  table
                'Start:AA:10.12.2015 YRS-AT-2478 Capturing taxable and nontaxable money for the savings and retirement plan wise
                'StartAA:10.23.2015 YRS-At-2478 Capture the retirement plan options reserves postax money and pretax money 
                ' Start:AA:10.26.2015 YRS-At-2478 Changed to capture death benefit and reserves into same amount
                'If Not RetirementPlanRows_ForRetired Is Nothing Then
                '    For Each dr As DataRow In RetirementPlanRows_ForRetired
                '        If Not String.IsNullOrEmpty(dr("PersonalPreTax").ToString()) Then
                '            If (dr("pmt") = "B") And (dr("PersonalPreTax") > 0) Then
                '                decRetTaxable = decRetTaxable + dr("PersonalPreTax")
                '            End If
                '        End If
                '        If Not String.IsNullOrEmpty(dr("YmcaPreTax").ToString()) Then
                '            If (dr("pmt") = "B") And (dr("YmcaPreTax") > 0) Then
                '                decRetTaxable = decRetTaxable + dr("YmcaPreTax") - dr("Death Benefit") 'Manthan Rajguru | 2016.05.31 | YRS-AT-3003 | Excluding Death benefit amt from Retirement plan amt
                '            End If
                '        End If

                '        If Not String.IsNullOrEmpty(dr("PersonalPostTax").ToString()) Then
                '            If (dr("pmt") = "B") And (dr("PersonalPostTax") > 0) Then
                '                decRetNonTaxable = decRetNonTaxable + dr("PersonalPostTax")
                '            End If
                '        End If
                '    Next
                'End If

                'If Not SavingsPlanRows_ForRetired Is Nothing Then
                '    For Each dr As DataRow In SavingsPlanRows_ForRetired
                '        If Not String.IsNullOrEmpty(dr("PersonalPreTax").ToString()) Then
                '            If (dr("pmt") = "B") And (dr("PersonalPreTax") > 0) Then
                '                decSavTaxable = decSavTaxable + dr("PersonalPreTax")
                '            End If
                '        End If
                '        If Not String.IsNullOrEmpty(dr("YmcaPreTax").ToString()) Then
                '            If (dr("pmt") = "B") And (dr("YmcaPreTax") > 0) Then
                '                decSavTaxable = decSavTaxable + dr("YmcaPreTax")
                '            End If
                '        End If
                '        If Not String.IsNullOrEmpty(dr("PersonalPostTax").ToString()) Then
                '            If (dr("pmt") = "B") And (dr("PersonalPostTax") > 0) Then
                '                decSavNonTaxable = decSavNonTaxable + dr("PersonalPostTax")
                '            End If
                '        End If
                '    Next
                'End If
                'End:AA:10.26.2015 YRS-At-2478 Changed to capture death benefit and reserves into same amount
                'End:10.23.2015 YRS-At-2478 Capture the retirement plan options reserves postax money and pretax money 
                'End Commented by - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Only Active money will Inserted into Colummn of mnyRetTaxableAmt ,mnyRetNontaxableAmt,mnySavTaxableAmt ,mnySavNontaxableAmt of AtsDeathBenefitApplicationForm  table
                If Not RetirementPlanRows_ForActive Is Nothing Then
                    If Not BasicReserves_RetPlan Is Nothing Then
                        For Each drBR As DataRow In BasicReserves_RetPlan
                            If Not String.IsNullOrEmpty(drBR("BasicReserve").ToString()) Then
                                If (drBR("BasicReserve")) > 0 Then
                                    If Not String.IsNullOrEmpty(drBR("TaxableMoney").ToString()) Then
                                        If (drBR("TaxableMoney") > 0) Then
                                            decRetTaxable = decRetTaxable + drBR("TaxableMoney")
                                        End If
                                    End If

                                    If Not String.IsNullOrEmpty(drBR("NonTaxableMoney").ToString()) Then
                                        If (drBR("NonTaxableMoney") > 0) Then
                                            decRetNonTaxable = decRetNonTaxable + drBR("NonTaxableMoney")
                                            Exit For
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If

                If Not SavingsPlanRows_ForActive Is Nothing Then
                    If Not BasicReserves_SavPlan Is Nothing Then
                        For Each drBR As DataRow In BasicReserves_SavPlan
                            If Not String.IsNullOrEmpty(drBR("TaxableMoney").ToString()) Then
                                If (drBR("TaxableMoney") > 0) Then
                                    decSavTaxable = decSavTaxable + drBR("TaxableMoney")
                                End If
                            End If

                            If Not String.IsNullOrEmpty(drBR("NonTaxableMoney").ToString()) Then
                                If (drBR("NonTaxableMoney") > 0) Then
                                    decSavNonTaxable = decSavNonTaxable + drBR("NonTaxableMoney")
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                End If
                'End:AA:10.12.2015 YRS-AT-2478 Capturing taxable and nontaxable money for the savings and retirement plan wisw
                'START - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
                If Not drInsuredReserverAmtRetirementPlanRows_ForRetired Is Nothing Then
                    For Each dr As DataRow In drInsuredReserverAmtRetirementPlanRows_ForRetired
                        If Not String.IsNullOrEmpty(dr("Reserves").ToString()) Then
                            If dr("Reserves") > 0 Then
                                If Not String.IsNullOrEmpty(dr("PersonalPreTax").ToString()) Then
                                    If (dr("pmt") = "B" And dr("PersonalPreTax") > 0) Then
                                        decPrincipalGuaranteeAnnuityRetTaxableAmt = decPrincipalGuaranteeAnnuityRetTaxableAmt + dr("PersonalPreTax")
                                    End If
                                End If
                                If Not String.IsNullOrEmpty(dr("YmcaPreTax").ToString()) Then
                                    If (dr("pmt") = "B" And dr("YmcaPreTax") > 0) Then
                                        decPrincipalGuaranteeAnnuityRetTaxableAmt = decPrincipalGuaranteeAnnuityRetTaxableAmt + dr("YmcaPreTax") - dr("Death Benefit")
                                    End If
                                End If

                                If Not String.IsNullOrEmpty(dr("PersonalPostTax").ToString()) Then
                                    If (dr("pmt") = "B" And dr("PersonalPostTax") > 0) Then
                                        decPrincipalGuaranteeAnnuityRetNonTaxableAmt = decPrincipalGuaranteeAnnuityRetNonTaxableAmt + dr("PersonalPostTax")
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
                If Not drInsuredReserverAmtSavingsPlanRows_ForRetired Is Nothing Then
                    For Each drInResAmtSav As DataRow In drInsuredReserverAmtSavingsPlanRows_ForRetired
                        If Not String.IsNullOrEmpty(drInResAmtSav("Reserves").ToString()) Then
                            If drInResAmtSav("Reserves") > 0 Then
                                If Not String.IsNullOrEmpty(drInResAmtSav("PersonalPreTax").ToString()) Then
                                    If (drInResAmtSav("pmt") = "B" And drInResAmtSav("PersonalPreTax") > 0) Then
                                        decPrincipalGuaranteeAnnuitySavTaxableAmt = decPrincipalGuaranteeAnnuitySavTaxableAmt + drInResAmtSav("PersonalPreTax")
                                    End If
                                End If
                                If Not String.IsNullOrEmpty(drInResAmtSav("YmcaPreTax").ToString()) Then
                                    If (drInResAmtSav("pmt") = "B" And drInResAmtSav("YmcaPreTax") > 0) Then
                                        decPrincipalGuaranteeAnnuitySavTaxableAmt = decPrincipalGuaranteeAnnuitySavTaxableAmt + drInResAmtSav("YmcaPreTax") - drInResAmtSav("Death Benefit")
                                    End If
                                End If
                                If Not String.IsNullOrEmpty(drInResAmtSav("PersonalPostTax").ToString()) Then
                                    If (drInResAmtSav("pmt") = "B" And drInResAmtSav("PersonalPostTax") > 0) Then
                                        decPrincipalGuaranteeAnnuitySavNonTaxableAmt = decPrincipalGuaranteeAnnuitySavNonTaxableAmt + drInResAmtSav("PersonalPostTax")
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
                'END - Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)

                ' get blnSection1Visible
                'Anudeep A.2013.01.07 - Added for YRS 5.0-1707:New Death Benefit Application form to check beneficiaries ES/IN/TR
                If (strBeneficiaryType = "ES" Or strBeneficiaryType = "IN" Or strBeneficiaryType = "TR") Then
                    blnSection3Visible = True
                Else
                    'Anudeep Commented below line beacuse it is already checking in if else part 
                    'If (decRetPlan > 0 And decRetPlan <= 5000) Then
                    '    blnSection1Visible = True
                    'End If
                    'Anudeep Merged two mutual exclusive if conditions into if else 
                    If (decRetPlan > 0) Then
                        If (decRetPlan <= 5000) Then
                            blnSection1Visible = True
                        ElseIf (decRetPlan > 5000) Then
                            blnSection2Visible = True
                        End If
                    End If

                    'Anudeep Commented below line beacuse it is already checking in if else part 
                    'If (decRetPlan > 5000) Then
                    '    blnSection2Visible = True
                    'End If

                    'Anudeep Merged two mutual exclusive if conditions into if else 
                    If (decSavPlan > 0) Then
                        If (decSavPlan <= 5000) Then
                            blnSection4Visible = True
                        ElseIf (decSavPlan > 5000) Then
                            blnSection5Visible = True
                        End If
                    End If

                    'Anudeep Commented below line beacuse it is already checking in if else part 
                    'If (decSavPlan > 0 And decSavPlan <= 5000) Then
                    '    blnSection4Visible = True
                    'End If
                    'If (decSavPlan > 5000) Then
                    '    blnSection5Visible = True
                    'End If

                    'Anudeep Merged two mutual exclusive if conditions into if else 
                    If (decDeathBenefit > 0) Then
                        'If (decDeathBenefit <= 5000) Then
                        If (decDeathBenefit <= 5000 Or decAnnuityMFromRDB = 0) Then 'Dharmesh : 11/21/2018 : YRS-AT-3837 : added new validation to not display the RDB option section when there is annuity amount has zero value
                            blnSection6Visible = True
                            'SR:2013.01.07 - YRS 5.0-1707: Either section 7 or section 8 should be visible for a benficiary.
                        ElseIf (decDeathBenefit > 5000) Then
                            blnSection7Visible = Not blnJSBeneficiaryExist
                            'SR:2013.01.07 - YRS 5.0-1707: Either section 7 or section 8 should be visible for a benficiary.
                            blnSection8Visible = blnJSBeneficiaryExist
                        End If
                    End If
                    'Anudeep Commented below line beacuse it is already checking in if else part 
                    'If (decDeathBenefit > 0 And decDeathBenefit <= 5000) Then
                    '    blnSection6Visible = True
                    'End If
                    'SR:2013.01.07 - YRS 5.0-1707: Either section 7 or section 8 should be visible for a benficiary.
                    'If (decDeathBenefit > 5000 And blnJSBeneficiaryExist = False) Then
                    '    blnSection7Visible = True
                    'End If
                    'SR:2013.01.07 - YRS 5.0-1707: Either section 7 or section 8 should be visible for a benficiary.
                    'If (decDeathBenefit > 5000 And blnJSBeneficiaryExist = True) Then
                    '    blnSection8Visible = True
                    'End If

                    'If (decPrincipalGuaranteeAnnuity_RP > 0 And decPrincipalGuaranteeAnnuity_RP <= 5000) Then
                    '    blnSection9Visible = True
                    'End If

                    'Anudeep Merged two mutual exclusive if conditions into if else 
                    If (decPrincipalGuaranteeAnnuity_RP > 0) Then
                        If (decPrincipalGuaranteeAnnuity_RP <= 5000) Then
                            blnSection9Visible = True
                        ElseIf (decPrincipalGuaranteeAnnuity_RP > 5000) Then
                            blnSection11Visible = True
                        End If
                    End If

                    'Anudeep Commented below line beacuse it is already checking in if else part 
                    'If (decPrincipalGuaranteeAnnuity_RP > 5000) Then
                    '    blnSection11Visible = True
                    'End If

                    'If (decPrincipalGuaranteeAnnuity_SP > 0 And decPrincipalGuaranteeAnnuity_SP <= 5000) Then
                    '    blnSection10Visible = True
                    'End If

                    If (decPrincipalGuaranteeAnnuity_SP > 0) Then
                        If (decPrincipalGuaranteeAnnuity_SP <= 5000) Then
                            blnSection10Visible = True
                        ElseIf (decPrincipalGuaranteeAnnuity_SP > 5000) Then
                            blnSection12Visible = True
                        End If
                    End If

                    'Anudeep Commented below line beacuse it is already checking in if else part 
                    'If (decPrincipalGuaranteeAnnuity_SP > 5000) Then
                    '    blnSection12Visible = True
                    'End If

                End If
            End If

            'SP 2014.12.04 BT-2310\YRS 5.0-2255: - Start
            'Adding representative details to save in case of 
            If HelperFunctions.isNonEmpty(dsBeneRepDetails) Then
                Dim drRepRow As DataRow = dsBeneRepDetails.Tables(0).Rows(0)
                strRepFirstName = drRepRow("chvRepFirstName")
                strRepLastName = drRepRow("chvRepLastName")
                strRepSalutation = drRepRow("chvRepSalutation")
                strRepTelephone = drRepRow("chvRepTelephone")
            End If
            'SP 2014.12.04 BT-2310\YRS 5.0-2255: - End

            'START: MMR | 2020.02.10 | YRS-AT-4770 | If Secure act applicable, then do not display annuity option section in Death benefit application report

            'Getting Beneficiary Details (Birth date, relationship) based on beneficiary Tax number (SSNo)
            isSecureActApplicable = False
            beneficiaryDetailsTable = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetBeneficiaryDetails(strBeneficiarySSNo)

            If HelperFunctions.isNonEmpty(beneficiaryDetailsTable) Then
                If Not IsDBNull(beneficiaryDetailsTable.Rows(0)("BirthDate")) Then
                    beneficiaryBirthDate = Convert.ToDateTime(beneficiaryDetailsTable.Rows(0)("BirthDate"))
                End If
                If Not IsDBNull(beneficiaryDetailsTable.Rows(0)("RelationshipCode")) Then
                    beneficiaryRelationshipCode = beneficiaryDetailsTable.Rows(0)("RelationshipCode").ToString()
                End If
            End If
            participantDeathDate = Me.ParticipantDeathDate

            'Fetch secure act applicable or not
            If (Not String.IsNullOrEmpty(beneficiaryBirthDate) And beneficiaryRelationshipCode.ToUpper.Trim <> "SP") Then
                isSecureActApplicable = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.IsSecureActApplicable(Me.ParticipantBirthDate, beneficiaryBirthDate, beneficiaryRelationshipCode, participantDeathDate, False)
            End If

            If isSecureActApplicable Then
                'Non-Retired beneficiaries
                If (decRetPlan > 0) Then
                    blnSection1Visible = True
                End If
                If (decSavPlan > 0) Then
                    blnSection4Visible = True
                End If
                blnSection2Visible = False
                blnSection5Visible = False
                'Retired Beneficiaries
                If (decPrincipalGuaranteeAnnuity_RP > 0) Then
                    blnSection9Visible = True
                End If
                If (decPrincipalGuaranteeAnnuity_SP > 0) Then
                    blnSection10Visible = True
                End If
                blnSection11Visible = False
                blnSection12Visible = False
            End If
            'END: MMR | 2020.02.10 | YRS-AT-4770 | If Secure act applicable, then do not display annuity option section in Death benefit application report

            'SP 2014.12.04 BT-2310\YRS 5.0-2255: Added above parameters into below method "strRepFirstName, strRepLastName, strRepSalutation &strRepTelephone"
            strReturnStatus = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.SaveDeathBenefitCalculatorFormDetails(strdecsdPersID, strdecsdFundeventID, _
            StrBeneficiaryName, _
            decJSAnnuityAmount, _
            decRetPlan, _
            decPrincipalGuaranteeAnnuity_RP, _
            decSavPlan, _
            decPrincipalGuaranteeAnnuity_SP, _
            decDeathBenefit, _
            decAnnuityMFromRP, _
            decFirstMAnnuityFromRP, _
            decAnnuityCFromRP, _
            decFirstCAnnuityFromRP, _
            decLumpSumFromNonHumanBen, _
            decAnnuityMFromSP, _
            decFirstMAnnuityFromSP, _
            decAnnuityCFromSP, _
            decFirstCAnnuityFromSP, _
            decAnnuityMFromRDB, _
            decFirstMAnnuityFromRDB, _
            decAnnuityFromJSAndRDB, _
            decFirstAnnuityFromJSAndRDB, _
            decAnnuityMFromResRemainingOfRP, _
            decFirstMAnnuityFromResRemainingOfRP, _
            decAnnuityCFromResRemainingOfRP, _
            decFirstCAnnuityFromResRemainingOfRP, _
            decAnnuityMFromResRemainingOfSP, _
            decFirstMAnnuityFromResRemainingOfSP, _
            decAnnuityCFromResRemainingOfSP, _
            decFirstCAnnuityFromResRemainingOfSP, _
            intMonths, _
            blnActiveDeathBenfit,
            blnSection1Visible, _
            blnSection2Visible, _
            blnSection3Visible, _
            blnSection4Visible, _
            blnSection5Visible, _
            blnSection6Visible, _
            blnSection7Visible, _
            blnSection8Visible, _
            blnSection9Visible, _
            blnSection10Visible, _
            blnSection11Visible, _
            blnSection12Visible, _
            blnCopyIDM, _
            blnFollowUp, _
            strAddressID, _
            strBeneficiaryFirstName, _
            strBeneficiaryLastName, _
            strBeneficiarySSNo,
            strRepFirstName,
            strRepLastName,
            strRepSalutation,
            strRepTelephone,
            decRetTaxable,
            decRetNonTaxable,
            decSavTaxable,
            decSavNonTaxable,
            decPrincipalGuaranteeAnnuityRetTaxableAmt,
            decPrincipalGuaranteeAnnuityRetNonTaxableAmt,
            decPrincipalGuaranteeAnnuitySavTaxableAmt,
            decPrincipalGuaranteeAnnuitySavNonTaxableAmt
            )
            ' Chandra Sekar - 2016.08.05 - YRS-AT-3027 - Include the correct Principle guarantee annuity amount (Death Beneficiary/Annuity)
            'AA:10.12.2015 YRS_AT-2478 Added to new columns to store pretax,post tax and ymcapretax in atsdeathbeniftapplicationform
            'SR:2013.05.06 - Add covering letter with death benefit application form
            'SetCoveringLetter(blnSection1Visible, blnSection2Visible, blnSection3Visible, blnSection4Visible, blnSection5Visible, blnSection6Visible, blnSection7Visible, blnSection8Visible, blnSection9Visible, blnSection10Visible, blnSection11Visible, blnSection12Visible, strReturnStatus)
            'End, SR:2013.05.06 - Add covering letter with death benefit application form
            HelperFunctions.LogMessage("Completed function SaveBeneficiaryOptions()")
            Return strReturnStatus
        Catch
            Throw
        End Try

    End Function
    'Anudeep A.:2012.12.04-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up 
    'Gets Forms list from database and inserts into table tbForms in div tag to show in jquerypopup
    Public Sub AdditionalForms()
        Dim row As HtmlTableRow
        Dim cell1 As HtmlTableCell
        Dim cell2 As HtmlTableCell
        Dim Chk As CheckBox
        Dim txt As TextBox
        Dim img As Image
        Dim innerHtml As String
        Dim l_dataset_Additonal_Forms As DataSet
        Dim l_datarow As DataRow()
        Dim strFundEventID As String = String.Empty
        Dim strName As String = String.Empty
        Dim decValue As Decimal = 5000 'It is the minimum balance to take annuity 
        Try
            HelperFunctions.LogMessage("Begin method AdditionalForms()")
            'Anudeep:26.03.2013:Commented for the because it causing initialising problem
            'If ButtonForms.Disabled Then
            '    Exit Sub
            'End If
            'If any rows exists then clears the table
            If tbForms.Rows.Count <> 0 Then
                tbForms.Rows.Clear()
            End If

            ' get the form details and store in dataset
            If DataSet_AdditionalForms Is Nothing Then
                DataSet_AdditionalForms = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetMetaAdditionalForms()
            End If

            l_dataset_Additonal_Forms = DataSet_AdditionalForms
            'Anudeep:Bt-1303:YRS 5.0-1707:New Death Benefit Application form 
            'Changed to get selected participant details from search grid
            If DataGrid_Search.SelectedIndex > -1 Then
                'l_datarow = l_dataset_SearchResults.Tables("r_MemberListForDeath").Select("[SS No.]=" + DataGrid_Search.SelectedItem.Cells(1).Text)
                If l_SSNumber Is Nothing Then
                    l_SSNumber = DataGrid_Search.SelectedItem.Cells(DataGrid_Search_ForAll_index.SSNO).Text 'AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                End If
                'Anudeep:08.08.2013 Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
                l_datarow = l_dataset_SearchResults.Tables("r_MemberListForDeath").Select("[SS No.]='" + l_SSNumber + "' And FundEventID='" + String_FundeventId + "'") 'AA:10.21.2015 YRS-AT-2548 Added fundeventid column to get the exact selected participant details, to avoid multiple fundevents problem and using enum instead of hardcorde values
                If Not l_datarow Is Nothing And l_datarow.Length > 0 Then
                    strFundEventID = l_datarow(0).Item("FundEventID").ToString()
                    strName = l_datarow(0).Item("First Name").ToString() + " " + l_datarow(0).Item("Last Name").ToString()
                End If
            End If
            ''SR:2013.05.29- Add two checkboxes
            'Start:AA:2013:10.01 - BT-2225:Changes made to show note after First checkbox
            For i As Integer = 0 To 3
                row = New HtmlTableRow
                cell1 = New HtmlTableCell
                cell1.Style.Value = "font-size:small;"
                If i = 2 Then
                    'Anudeep:08.08.2013 Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
                    Chk = New CheckBox
                    Chk.ID = "chkIDM"
                    Chk.Checked = False
                    Chk.CssClass = "checkbox"
                    cell1.Controls.Add(Chk)
                ElseIf i = 3 Then
                    cell1.InnerHtml = "Death Benefit Application - Required Forms or Additional Documents"
                    cell1.Style.Value = "font-size:small;font-weight:bold;color:Black"
                    cell1.BgColor = "#D8D8D8"
                    cell1.ColSpan = 2
                ElseIf i = 0 Then
                    Chk = New CheckBox
                    Chk.ID = "chkFollowUp"
                    Chk.Checked = False
                    Chk.CssClass = "checkbox"
                    cell1.Controls.Add(Chk)
                ElseIf i = 1 Then
                    'cell1.InnerHtml = "Note: if above option is checked, future 60-day and 90-day follow-up letters will be calculated from today’s date, to be generated as needed."
                    'cell1.Style.Value = "font-size:small;color:Black;" ' 
                    'cell1.ColSpan = 2
                    cell1.InnerHtml = ""
                End If
                row.Cells.Insert(0, cell1)
                If i <> 3 Then
                    cell2 = New HtmlTableCell
                    cell2.Style.Value = "font-size:small;color:#8A0829"
                    If i = 0 Then
                        'Anudeep:08.08.2013 Bt-1512:YRS 5.0-1751:Process to generate Death Beneficiary follow-up letters.
                        cell2.InnerHtml = "Record this form/letter as the first communication with beneficiary."
                    ElseIf i = 1 Then
                        cell2.InnerHtml = "Note: If above option is checked, future 60-day and 90-day follow-up letters will be calculated from today’s date, to be generated as needed."
                        cell2.Style.Value = "font-size:small;color:Black;"
                    ElseIf i = 2 Then
                        cell2.InnerHtml = "Send a copy of Form & Letter to IDM"
                    End If
                    row.Cells.Insert(1, cell2)
                End If
                tbForms.Rows.Add(row)
            Next
            'End:AA:2013:10.01 - BT-2225:Changes made to show note after First checkbox
            ''End, SR:2013.05.29- Add two checkboxes
            For i As Integer = 0 To l_dataset_Additonal_Forms.Tables(0).Rows.Count - 1
                row = New HtmlTableRow
                cell1 = New HtmlTableCell
                cell2 = New HtmlTableCell
                Chk = New CheckBox
                ' checks if the person has more than $5000 balace then it will show forms which has bitvalidateBalance is set true
                If l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("bitValidateBalance") Then

                    Try
                        'Anudeep:Bt-1303:YRS 5.0-1707:New Death Benefit Application form 
                        'Changed to get selected participant details from search grid
                        'If YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetBasicAccountBalance(l_dataset_SearchResults.Tables("r_MemberListForDeath").Item(DataGrid_Search.SelectedIndex).Row.Item("FundEventID").ToString()) <= 5000 Then
                        If l_dataset_Additonal_Forms.Tables.Count > 1 Then
                            If l_dataset_Additonal_Forms.Tables(1).Rows.Count > 0 Then
                                Try
                                    If Not String.IsNullOrEmpty(l_dataset_Additonal_Forms.Tables(1).Rows(0)("Value").ToString()) Then
                                        'Gets the 'Minimum balance to retire'Value from atsmetaconfiguration for checking balance
                                        decValue = Convert.ToDecimal(l_dataset_Additonal_Forms.Tables(1).Rows(0)("Value").ToString())
                                    End If
                                Catch
                                    Exit Try
                                End Try
                            End If
                        End If
                        'Checking the particiapant balance to minimum value to retire
                        'if less than or equql to minimun value it does not show the form
                        'else it shows the form
                        If YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetBasicAccountBalance(strFundEventID) <= decValue Then
                            Continue For
                        End If
                    Catch
                        'if any error returns while caluculating the balance then that form will not be shown
                        Continue For
                    End Try
                End If

                Chk.ID = "chk" + l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("intUniqueID").ToString()
                Chk.Checked = l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("bitDefaultSelected")
                Chk.CssClass = "checkbox"
                'cell1.Width = "5%"
                cell1.Controls.Add(Chk)
                row.Cells.Insert(0, cell1)
                cell2.Style.Value = "font-size:small;"
                'cell2.Width = "95%"
                'For the form "A copy of the death certificate for" the name is concatinated to the form label
                If l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("chvText").ToString().Trim() = "A copy of the death certificate for" Then
                    'Anudeep:Bt-1303:YRS 5.0-1707:New Death Benefit Application form 
                    'Changed to get selected participant details from search grid
                    'innerHtml = l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("chvText").ToString() + " " + DataGrid_Search.Items(DataGrid_Search.SelectedIndex).Cells(3).Text.ToString().Trim() + " " + DataGrid_Search.Items(DataGrid_Search.SelectedIndex).Cells(2).Text.ToString().Trim() + " "
                    'innerHtml = l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("chvText").ToString() + " " + l_dataset_SearchResults.Tables("r_MemberListForDeath").Rows(DataGrid_Search.SelectedIndex)("First Name").ToString().Trim + " " + l_dataset_SearchResults.Tables("r_MemberListForDeath").Rows(DataGrid_Search.SelectedIndex)("Last Name").ToString().Trim + " "
                    innerHtml = l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("chvText").ToString() + " " + strName + " "
                Else
                    innerHtml = l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("chvText").ToString() + " "
                End If
                cell2.InnerText = innerHtml
                ' If bitadditionalInfo is set to true then it should it adds the textbox
                If l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("bitAdditionalInfo") Then
                    txt = New TextBox
                    img = New Image
                    txt.ID = "txt" + l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("intUniqueID").ToString()
                    If l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("chvText") = "Other" Then
                        txt.Width = 450
                    End If
                    txt.Style.Value = "vertical-align:middle;"
                    img.Style.Value = "vertical-align:middle;"
                    img.ToolTip = l_dataset_Additonal_Forms.Tables(0).Rows(i).Item("chvAdditonalHelpText")
                    img.Width = 20
                    img.Height = 20
                    img.ImageUrl = "~/images/help.jpg"
                    cell2.Controls.Add(txt)
                    cell2.Controls.Add(img)
                End If
                row.Cells.Insert(1, cell2)
                tbForms.Rows.Add(row)
            Next
            HelperFunctions.LogMessage("Completd AdditionalForms() method")
        Catch
            Throw
        End Try
    End Sub
    'on click of Show form this event fill fire this will get the selected data and stores into session variable
    <System.Web.Services.WebMethod()> _
    Public Shared Function ShowFormclick(ByVal Formlist As String) As String
        Dim DeathBenefit As New DeathBenefitsCalculatorForm
        Dim DataSet_AdditionalForms As New DataSet
        Try
            If HttpContext.Current.Session("Formlist") Is Nothing Then
                HttpContext.Current.Session("Formlist") = Formlist
            End If
            HttpContext.Current.Session("ProcessData") = "Yes"

        Catch
            Throw
        End Try
    End Function

    'SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
    <System.Web.Services.WebMethod()> _
    Public Shared Function GetBeneFiciaryAddress() As String
        Dim dsBenAddress As New DataSet
        Dim dsRepDetails As DataSet 'SP 2014.12.04 BT-2310\YRS 5.0-2255:
        Dim isNonHumanBene As Boolean
        Dim strGuiBeneficiaryId As String
        Try
            HelperFunctions.LogMessage("Begin GetBeneFiciaryAddress() method")
            'Start: AA:08.12.2014- BT:2460:YRS 5.0-2331 - Added below lines to check whether death beneficiary is selected or annuity joint survivor is selected and call different stored procedures.
            If DeathCalc_AnnuityJointSurviour IsNot Nothing AndAlso DeathCalc_AnnuityJointSurviour = "True" Then
                dsBenAddress = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetJointSurviourAdress(HttpContext.Current.Session("DeathCalc_BenTaxNo"), HttpContext.Current.Session("String_PersId"))
            ElseIf DeathCalc_AnnuityJointSurviour Is Nothing Then
                'End: AA:08.12.2014- BT:2460:YRS 5.0-2331 - Added below lines to check whether death beneficiary is selected or annuity joint survivor is selected and call different stored procedures.
                dsBenAddress = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetBeneficiaryAdress(HttpContext.Current.Session("DeathCalc_BenTaxNo"), HttpContext.Current.Session("String_PersId"), HttpContext.Current.Session("DeathCalc_BenFirstName"), HttpContext.Current.Session("DeathCalc_BenLastName"))
            End If

            'SP 2014.12.04 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN -Start
            'Check if representative details is missing 

            If (HelperFunctions.isNonEmpty(dsBenAddress)) Then
                If (dsBenAddress.Tables(0).Columns.Contains("guiEntityID")) Then
                    strGuiBeneficiaryId = dsBenAddress.Tables(0).Rows(0)("guiEntityID")
                End If
                If HttpContext.Current.Session("SelectedBeneficiaryIsNonHuman") IsNot Nothing AndAlso HttpContext.Current.Session("SelectedBeneficiaryIsNonHuman") = True Then
                    dsRepDetails = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.GetFollowUpRepresentativeDetails(HttpContext.Current.Session("String_PersId"), strGuiBeneficiaryId, HttpContext.Current.Session("DeathCalc_BenTaxNo"))
                    isNonHumanBene = IsRepresentativeDetailsMissing(dsRepDetails)
                End If
            End If
            'SP 2014.12.04 BT-2310\YRS 5.0-2255:Add contact info to bene records for ES, TR and IN -End
            'SP 2014.12.04 BT-2310\YRS 5.0-2255:Added if condition to check representative details is not exits then return 2 else execute existing condition
            HttpContext.Current.Session("BeneRepDetails") = dsRepDetails
            If isNonHumanBene AndAlso HelperFunctions.isNonEmpty(dsBenAddress) Then
                Return 2
            ElseIf HelperFunctions.isNonEmpty(dsBenAddress) Then
                HttpContext.Current.Session("DeathCalc_BenAddressId") = dsBenAddress.Tables(0).Rows(0).Item(0).ToString.Trim
                Return 0
            Else
                Return 1
            End If
            HelperFunctions.LogMessage("Completed GetBeneFiciaryAddress() method")
        Catch
            Throw
        End Try
    End Function
    'End, SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
    'It gets the details which has selected forms and details by users and save into database
    Public Sub SaveFormDetails()
        Dim intDBAppFormID As Integer = 0
        Dim intMetaDBAdditionalFormID As Integer
        Dim chvAdditionalText As String
        Dim strFormlist As String
        Dim strForms() As String
        Dim chvText As String
        Dim intUniqueID As Integer
        Dim dtDeathBenefitFormReqdDocs As New DataTable
        Dim drDeathBenefitFormReqdDocs As DataRow
        Dim l_datarow As DataRow()
        Dim strName As String = String.Empty
        Dim blnCopyIDM As Boolean = False
        Dim blnFollowUp As Boolean = False

        Try
            HelperFunctions.LogMessage("Begin routine SaveFormDetails()")
            dtDeathBenefitFormReqdDocs.Columns.Add("intDBAppFormID")
            dtDeathBenefitFormReqdDocs.Columns.Add("intMetaDBAdditionalFormID")
            dtDeathBenefitFormReqdDocs.Columns.Add("chvAdditionalText")

            If Not Session("Formlist") Is Nothing And Session("Formlist") <> "" Then
                'Gets the form uniqueid For form "A copy of the death certificate for" to add User name in additional text for this form
                For i As Integer = 0 To DataSet_AdditionalForms.Tables(0).Rows.Count - 1
                    If DataSet_AdditionalForms.Tables(0).Rows(i).Item("chvText").ToString().Trim() = "A copy of the death certificate for" Then
                        intUniqueID = DataSet_AdditionalForms.Tables(0).Rows(i).Item("intUniqueID")
                    End If
                Next
                'Anudeep:Bt-1303:YRS 5.0-1707:New Death Benefit Application form 
                'Changed to get selected participant details from search grid
                If DataGrid_Search.SelectedIndex > -1 Then
                    l_datarow = l_dataset_SearchResults.Tables("r_MemberListForDeath").Select("[SS No.]=" + DataGrid_Search.SelectedItem.Cells(DataGrid_Search_ForAll_index.SSNO).Text) 'AA:1.10.2015 YRS_AT-2548 : Changed to hardcode Values to enum for refactoring
                    If l_datarow.Length > 0 Then
                        strName = l_datarow(0).Item("First Name").ToString() + " " + l_datarow(0).Item("Last Name").ToString()
                    End If
                End If
                'gets the concatinated string from session and store into variable
                strFormlist = Session("Formlist")
                If strFormlist.Contains("$$") Then
                    strForms = strFormlist.Split("$$")
                Else
                    ReDim strForms(0)
                    strForms(0) = strFormlist
                End If
                ' Splits and extract the form id and additional text and inserts into the datatable
                'SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
                For i As Integer = 0 To strForms.Length - 1
                    If strForms(i) = "IDM" Then
                        blnCopyIDM = True
                    End If
                    If strForms(i) = "FollowUp" Then
                        blnFollowUp = True
                    End If
                Next
                'SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
                'Gets the intDBFormId From the Main table after process
                intDBAppFormID = Convert.ToInt32(ProcessSelectedData(blnCopyIDM, blnFollowUp, Session("BeneRepDetails")))

                For i As Integer = 0 To strForms.Length - 1
                    If Not (strForms(i) = "" Or strForms(i) = "IDM" Or strForms(i) = "FollowUp") Then ''SR:2013.06.26 - For BT-2016/YRS 5.0-2071 added check on IDM and followup
                        If strForms(i).Contains(",") Then
                            intMetaDBAdditionalFormID = Convert.ToInt32(strForms(i).Substring(0, strForms(i).IndexOf(",")))

                            If intMetaDBAdditionalFormID = intUniqueID Then
                                'concatinates the user name with additional text for the respective unique id
                                'chvAdditionalText = DataGrid_Search.Items(DataGrid_Search.SelectedIndex).Cells(3).Text.ToString().Trim() + " " + DataGrid_Search.Items(DataGrid_Search.SelectedIndex).Cells(2).Text.ToString().Trim()
                                'chvAdditionalText = l_dataset_SearchResults.Tables("r_MemberListForDeath").Rows(DataGrid_Search.SelectedIndex)("First Name").ToString().Trim + " " + l_dataset_SearchResults.Tables("r_MemberListForDeath").Rows(DataGrid_Search.SelectedIndex)("Last Name").ToString().Trim
                                chvAdditionalText = strName
                                chvAdditionalText = chvAdditionalText + "," + strForms(i).Substring(strForms(i).IndexOf(",") + 1, strForms(i).Length - (strForms(i).IndexOf(",") + 1))
                            Else
                                chvAdditionalText = strForms(i).Substring(strForms(i).IndexOf(",") + 1, strForms(i).Length - (strForms(i).IndexOf(",") + 1))
                            End If
                        Else
                            intMetaDBAdditionalFormID = Convert.ToInt32(strForms(i))
                            If intMetaDBAdditionalFormID = intUniqueID Then
                                'concatinates the user name with additional text for the respective unique id
                                'chvAdditionalText = DataGrid_Search.Items(DataGrid_Search.SelectedIndex).Cells(3).Text.ToString().Trim() + " " + DataGrid_Search.Items(DataGrid_Search.SelectedIndex).Cells(2).Text.ToString().Trim()
                                'chvAdditionalText = l_dataset_SearchResults.Tables("r_MemberListForDeath").Rows(DataGrid_Search.SelectedIndex)("First Name").ToString().Trim + " " + l_dataset_SearchResults.Tables("r_MemberListForDeath").Rows(DataGrid_Search.SelectedIndex)("Last Name").ToString().Trim
                                chvAdditionalText = strName
                            Else
                                chvAdditionalText = ""
                            End If
                        End If
                        ' the extracted values are stored in datarow and inserted in atable
                        drDeathBenefitFormReqdDocs = dtDeathBenefitFormReqdDocs.NewRow()
                        drDeathBenefitFormReqdDocs("intDBAppFormID") = intDBAppFormID
                        drDeathBenefitFormReqdDocs("intMetaDBAdditionalFormID") = intMetaDBAdditionalFormID
                        drDeathBenefitFormReqdDocs("chvAdditionalText") = chvAdditionalText
                        dtDeathBenefitFormReqdDocs.Rows.Add(drDeathBenefitFormReqdDocs)
                    End If
                Next
                'Finnaly datatable is sent to database for storing form details
                If dtDeathBenefitFormReqdDocs.Rows.Count <> 0 Then
                    YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.SaveDeathFormDetails(dtDeathBenefitFormReqdDocs)
                End If
            End If
            'OpenReportViewer()
            HelperFunctions.LogMessage("Begin CreateAndCopyDBAForm() method")
            CreateAndCopyDBAForm(intDBAppFormID, blnCopyIDM) ''SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
            HelperFunctions.LogMessage("Completed CreateAndCopyDBAForm() method")
            HelperFunctions.LogMessage("Begin Createandcopycoveringletter() method")
            Createandcopycoveringletter(intDBAppFormID, blnCopyIDM) 'SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
            HelperFunctions.LogMessage("Completed Createandcopycoveringletter() method")
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
            HelperFunctions.LogMessage("Completed SaveFormDetails() method")
        Catch
            Throw
        Finally
            Session("Formlist") = Nothing
            dtDeathBenefitFormReqdDocs = Nothing
        End Try
    End Sub
    'Anudeep A.:2012.12.13-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up 
    'Public Sub PopulateBeneficiaries(ByVal l_string_PersId As String, ByVal l_string_FundEventId As String, ByVal l_string_FundStatus As String, ByVal l_Datetime_DeathDate As DateTime)
    '    Dim l_dataset_Beneficiaries As DataSet
    '    Try
    '        l_dataset_Beneficiaries = YMCARET.YmcaBusinessObject.DeathBenefitsCalculatorBOClass.PopulateBeneficiaries(l_string_PersId, l_string_FundEventId, l_Datetime_DeathDate, l_string_FundStatus)
    '        Session("BeneficiaryList") = l_dataset_Beneficiaries.Tables(0)
    '        DataGrid_BeneficiariesList_ForAll.DataSource = l_dataset_Beneficiaries.Tables(0)
    '        If l_dataset_Beneficiaries.Tables(0).Rows.Count > 0 Then DataGrid_BeneficiariesList_ForAll.SelectedIndex = 0
    '        DataGrid_BeneficiariesList_ForAll.DataBind()
    '        lbl_Beneficiaries_All.Visible = True
    '        DataGrid_BeneficiariesList_ForAll.Visible = True
    '    Catch
    '        Throw
    '    End Try
    'End Sub

    'Anudeep A.:2012.12.14-YRS 5.0-1707:New Death Benefit Application form for Jquery Pop up 
    'To Populate jquery after cilcking show form
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Try
            If Session("ProcessData") = "Yes" Then
                AdditionalForms()
            End If
        Catch
            Throw
        Finally
            Session("ProcessData") = Nothing
        End Try
    End Sub
    'SR:2012.12.14:YRS 5..0-1707 : Clear Session data
    Public Sub ClearSession()
        Try
            Session("First_Calculation") = Nothing
            Session("Change_FundStatus") = Nothing
            Session("FundStatus") = Nothing
            'Session("BeneficiaryList") = Nothing
            Session("DeathCalc_Sort") = Nothing
            Session("Formlist") = Nothing
            Session("SelectedBeneficiaryDetails") = Nothing
            Session("SelectedBeneficiaryDetails_ForRetired") = Nothing
            Session("SelectedBeneficiaryDetails_ForActive") = Nothing
            Session("SelectedBeneficiaryDetails_ForBasicReserves") = Nothing
            Session("Para3_ReservesAcctName") = Nothing
            Session("Para4_Reserves_NonTaxable") = Nothing
            Session("Para5_Reserves_Taxable") = Nothing
            Session("Para6_Reserves_Balance") = Nothing
            Session("Para7_Savings_ReservesAcctName") = Nothing
            Session("Para7_Savings_ReservesAcctName") = Nothing
            Session("Para8_Savings_NonTaxable") = Nothing
            Session("Para9_Savings_Taxable") = Nothing
            Session("Para10_Savings_Balance") = Nothing
            Session("Para11_DBAcctName") = Nothing
            Session("Para11_DBAcctName") = Nothing
            Session("Para12_DB_NonTaxable") = Nothing
            Session("Para13_DB_Taxable") = Nothing
            Session("Para14_DB_Balance") = Nothing
            Session("Para15_TotalAcctname") = Nothing
            Session("Para15_TotalAcctname") = Nothing
            Session("Para16_Total_NonTaxable") = Nothing
            Session("Para17_Total_Taxable") = Nothing
            Session("Para18_Total_Balance") = Nothing
            Session("strReportName") = Nothing
            Session("intDBAppFormID") = Nothing
            Session("JSAnnuities") = Nothing
            Session("strReportName") = Nothing
            'SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
            Session("strReportName_1") = Nothing
            Session("DeathCalc_BenAddressId") = Nothing
            Session("DeathCalc_BenTaxNo") = Nothing
            Session("Formlist") = Nothing
            Session("ProcessData") = Nothing
            Session("DeathCalc_BenFirstName") = Nothing
            Session("DeathCalc_BenLastName") = Nothing
            'End, SR:2013.06.26 - BT-2016/YRS 5.0-2071: Need cover letter added to Death Benefit Application Form.
            DeathCalc_AnnuityJointSurviour = Nothing 'End: AA:08.12.2014- BT:2460:YRS 5.0-2331 - Added line to clear the session of annutiy joint surviour select
            'SP 2014.12.04 BT-2310\YRS 5.0-2255 - Start
            Session("SelectedBeneficiaryIsNonHuman") = Nothing
            Session("BeneRepDetails") = Nothing
            'SP 2014.12.04 BT-2310\YRS 5.0-2255 -End
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function CopyToIDM(ByVal l_StringDocType As String, ByVal l_StringReportName As String, ByVal l_ArrListParamValues As ArrayList, ByVal l_string_OutputFileType As String) As String
        Dim l_StringErrorMessage As String
        Dim IDM As New IDMforAll
        Try
            'Anudeep 04.12.2012 Code changes to copy report into IDM folder
            'gets the columns for idm and stored in session varilable 

            If Session("FTFileList") Is Nothing Then
                If IDM.DatatableFileList(False) Then
                    Session("FTFileList") = IDM.SetdtFileList
                End If
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

            IDM.PersId = Session("String_PersId")
            IDM.DocTypeCode = l_StringDocType
            IDM.OutputFileType = l_string_OutputFileType
            IDM.ReportName = l_StringReportName.ToString().Trim & ".rpt"
            IDM.ReportParameters = l_ArrListParamValues

            l_StringErrorMessage = IDM.ExportToPDF()
            l_ArrListParamValues.Clear()

            Session("FTFileList") = IDM.SetdtFileList

            Return l_StringErrorMessage

        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Finally
            IDM = Nothing
        End Try
    End Function

    Private Sub Createandcopycoveringletter(ByVal intDBAppFormID As Integer, ByVal blnCopyIDM As Boolean)
        Dim l_stringDocType As String = String.Empty
        Dim l_StringReportName As String = String.Empty
        Dim l_ArrListParamValues As New ArrayList
        Dim l_string_OutputFileType As String = String.Empty
        Dim l_StringErrorMessage As String = String.Empty
        Try
            Session("strReportName_1") = "Death Letter for all beneficiaries"
            l_StringReportName = "Death Letter for all beneficiaries"
            'Call ReportViewer.aspx 
            Dim popupScript2 As String = "<script language='javascript'>" & _
            "window.open('FT\\ReportViewer_1.aspx', 'ReportPopUp2', " & _
            "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
            "</script>"

            If (Not Me.IsStartupScriptRegistered("PopupScript2")) Then
                Page.RegisterStartupScript("PopupScript2", popupScript2)
            End If


            If blnCopyIDM Then
                l_stringDocType = "DTBNAPLR"
                l_ArrListParamValues.Add(intDBAppFormID)
                l_string_OutputFileType = "DeathBenefit_" + l_stringDocType
                'Copies report into idm convert into pdf and stores information idx 
                l_StringErrorMessage = CopyToIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)

                If Not l_StringErrorMessage Is String.Empty Then
                    MessageBox.Show(PlaceHolder1, "IDM Error", l_StringErrorMessage, MessageBoxButtons.Stop, False)
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub CreateAndCopyDBAForm(ByVal intDBAppFormID As Integer, ByVal blnCopyIDM As Boolean)
        Dim l_stringDocType As String = String.Empty
        Dim l_StringReportName As String = String.Empty
        Dim l_ArrListParamValues As New ArrayList
        Dim l_string_OutputFileType As String = String.Empty
        Dim l_StringErrorMessage As String = String.Empty
        Try
            Session("strReportName") = "Death Benefit Application"
            Session("intDBAppFormID") = intDBAppFormID
            Dim popupScript1 As String = "<script language='javascript'>" & _
            "window.open('FT\\ReportViewer.aspx', 'ReportPopUp1', " & _
            "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')" & _
            "</script>"
            If (Not Me.IsStartupScriptRegistered("PopupScript1")) Then
                Page.RegisterStartupScript("PopupScript1", popupScript1)
            End If

            If blnCopyIDM Then
                l_stringDocType = "DTHBENAP"
                l_StringReportName = "Death Benefit Application"
                l_ArrListParamValues.Add(intDBAppFormID)
                l_string_OutputFileType = "DeathBenefit_" + l_stringDocType
                'Copies report into idm convert into pdf and stores information idx 
                l_StringErrorMessage = CopyToIDM(l_stringDocType, l_StringReportName, l_ArrListParamValues, l_string_OutputFileType)
                If Not l_StringErrorMessage Is String.Empty Then
                    MessageBox.Show(PlaceHolder1, "IDM Error", l_StringErrorMessage, MessageBoxButtons.Stop, False)
                End If
                'Call ReportViewer.aspx 
            End If
        Catch
            Throw
        End Try
    End Sub
    'SP 2014.12.04 BT-2310\YRS 5.0-2255 - Start
    Private Shared Function IsRepresentativeDetailsMissing(ByVal dsFollowUpDetails As DataSet) As Boolean
        Dim isNotExists As Boolean
        Try
            If HelperFunctions.isNonEmpty(dsFollowUpDetails) Then
                Dim strFirstName As String = dsFollowUpDetails.Tables(0).Rows(0)("chvRepFirstName").ToString().Trim
                Dim strLastName As String = dsFollowUpDetails.Tables(0).Rows(0)("chvRepLastName").ToString().Trim
                If String.IsNullOrEmpty(strFirstName) AndAlso String.IsNullOrEmpty(strLastName) Then
                    isNotExists = True
                End If
            Else
                isNotExists = True
            End If
        Catch
            Throw
        End Try
        Return isNotExists
    End Function
    'SP 2014.12.04 BT-2310\YRS 5.0-2255 - End

    'START | SR | 2015.12.08 | YRS-AT-2718      
    Private Function GetMessageFromResourceFile(ByVal strMessage As String) As String
        Return GetGlobalResourceObject("DeathMessages", strMessage)
    End Function

    Public Shared Function DeathBenefitAnnuityPurchaseRestrictedDate() As DateTime
        Dim dsDth_Benefit_Ann_Purchase_Retriction_date As DataSet = Nothing

        Dim dthBenRestrictedDate As DateTime = Convert.ToDateTime("1/1/2019")
        Try
            dsDth_Benefit_Ann_Purchase_Retriction_date = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("RDB_ADB_2019Plan_Change_CutOFF_Date") 'Dharmesh : 11/28/2018 : YRS-AT-3837 : change the config key which is used for cut off date 1/1/2019

            If HelperFunctions.isNonEmpty(dsDth_Benefit_Ann_Purchase_Retriction_date) Then
                If dsDth_Benefit_Ann_Purchase_Retriction_date.Tables(0).Rows(0)("Value").ToString().Trim() <> String.Empty Then
                    dthBenRestrictedDate = Convert.ToDateTime(dsDth_Benefit_Ann_Purchase_Retriction_date.Tables(0).Rows(0)("Value"))
                End If
            End If
            Return dthBenRestrictedDate
        Catch
            Throw
        End Try
    End Function

    Public Property AnnuityPurchaseRestrictedDate() As Date
        Get
            If ViewState("AnnuityPurchaseRestrictedDate") Is Nothing Then
                ViewState("AnnuityPurchaseRestrictedDate") = DeathBenefitAnnuityPurchaseRestrictedDate()
            End If

            Return CType(ViewState("AnnuityPurchaseRestrictedDate"), Date)

        End Get
        Set(ByVal Value As Date)
            ViewState("AnnuityPurchaseRestrictedDate") = Value
        End Set
    End Property

    'END | SR | 2015.12.08 | YRS-AT-2718

    'START | SB | 2017.06.26 | YRS-AT-3371 | Function to get Value of the plan split date based on which death calculation logic changes 
    Private Function GetSavingsPlanStartDate() As Date
        Dim Death_Calc_SavingS_Plan_Start_Date As DataSet = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("DEATH_CALC_SAVINGS_PLAN_START_DATE")
        Dim DefaultSavingsPlanStartDate As Date = Nothing
        If HelperFunctions.isNonEmpty(Death_Calc_SavingS_Plan_Start_Date) Then
            DefaultSavingsPlanStartDate = DateTime.Parse(Death_Calc_SavingS_Plan_Start_Date.Tables(0).Rows(0)("Value"))
        End If
        Return DefaultSavingsPlanStartDate
    End Function
    Public Property SavingsPlanStartDate() As Date
        Get
            If Not ViewState("SavingsPlanStartDate") Is Nothing Then
                Return ViewState("SavingsPlanStartDate")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As Date)
            ViewState("SavingsPlanStartDate") = Value
        End Set
    End Property
    'END | SB | 2017.06.26 | YRS-AT-3371 | Function to get Value of the plan split date based on which death calculation logic changes 
End Class
