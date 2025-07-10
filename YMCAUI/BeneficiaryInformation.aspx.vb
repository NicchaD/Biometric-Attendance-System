'*******************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	BeneficiaryInformation.aspx
' Author Name		:	
' Employee ID		:	34451
' Email				:	jaya.kutty@3i-infotech.com
' Contact No		:	9892569434
' Creation Time		:	
' Program Specification Name	:	YMCA PS 3.14.1.doc
' Unit Test Plan Name			:	
' Description					:	
'   This page collects information about the beneficiary which is required to convert the 
'   beneficiary into a participant. A new participant is created for the beneficiary if 
'   one does not exist along with all associated records for the participant i.e. address, etc
'
' Changed by			:	Jaya Kutty
' Changed on			:	21/10/2005
' Change Description	:	Added Functions
'*******************************************************************************
' Cache-Session     :   Vipul 02Feb06
'*******************************************************************************
' NP - 2007.02.09 - YREN-3043: Code has been updated to create a beneficiary of type Estate
' for new participants that are created as a result of the Death Settlement process. The 
' beneficiary is only created if annuity option type C is selected by the user.
' Involves change to the SaveDetails procedure
'*******************************************************************************
'Changed By:            On:             IssueId: 
'Aparna Samala          06/03/2007     YREN-3015
'********************************************************************************************************************************
'Modification History
'********************************************************************************************************************************
'Modified By        Date            Description
'********************************************************************************************************************************
'Ashutosh Patil     14-Mar-2007     YREN-3028,YREN-3029 
'Mohammed Hafiz     02-May-2007     YREN-3112
'********************************************************************************************************************************
'Nikunj Patel       10-Jul-2007     Plan Split UI level code changes - Trying to refactor code from 2375 lines to fewer lines
'Nikunj Patel       20-Aug-2007     Change to fix bug 31 in bugtracker.
'Nikunj Patel       28-Aug-2007     Changed to use the Savings plan option id incase the Retirement plan option id is not there to identify the new beneficiary. Error when performing settlement on only TD Savings plan.
'Nikunj Patel       05-Sep-2007     Fixing issue where Withholding tab was not being initialized when annuity option was selected for settlement in the Savings plan
'                                   Setting the default value to Unknown as per conf call with Elliot
'                   17-Sep-2007     Changing code to avoid errors due to duplicate message boxes being shown. - 'dragbar' errors.
'Nikunj Patel       2008.01.03      Changing code to create only one fund Event for each source of funds - YRPS-4046
'Anil Gupta         2008.01.07      Changing Code for Integer Parsing of phony SSno for US Participants only. 
'Priya Jawale       03-Oct-2008     YRS 5.0-424 
'Nikunj Patel       2008.12.05      Passing Selected benefit Option Id so that the new fund event if required is created of the right type either DBEN or RBEN.
'Nikunj Patel       2009.04.20      Making use of common HelperFunctions class for checking isNonEmpty and isEmpty
'Dilip Yadav        2009.09.09      YRS 5.0.852
'Neeraj Singh       12/Nov/2009         Added form name for security issue YRS 5.0-940 
'Sanjay R.          2010.06.21      Enhancement changes(CType to DirectCast)
'Sanjay R.          2010.07.12      Code Review changes.(Region,variable Decalration,Try catch block etc.) 
'Priya              2010.08.11      YRS 5.0-1147 : Issue with simultaneous users doing death settlement 
'Sanjay R.          2010.09.25      variable is set instead of viewstate for withholding issue
'Imran              2010.12.18      Comment address validation on page level and call validation from address user control
'Shashi             14-Apr.-2011    For YRS 5.0-877 : Changes to Banking Information maintenance.
'bhavna S           2010.07.08      TextBoxGeneralPOA.Text fill by Session("POAName") instead of l_DataSet_POA if you update or add new POA in RetireesPowerAttorneyWebForm.aspx.vb then that session will take current value of POA else if session value is null then its will take value from database against by perid on Page Load
'bhavna S           18 july 2011    YRS 5.0-1339 : handle X value on MaritalStatus Dropdown when record is non actuary on page load
'bhavna S           19 july 2011    Reopen POA Issue show all active poa based on termination date > than current date effective date < than current date, termination date is null ,l_DataSet_POA. Handled BT-902 as well.
'bhavna S           19 july 2011    YRS 5.0-1339:handle X value on gender Dropdown
'bhavna S           2011.08.01      Revert :YRS 5.0-1339
'bhavna S           2011.08.10      BS:2011.08.10:YRS 5.0-1339:BT:852 - Reopen issue 
'bhavna S           2011.09.15      BS:2011.09.15:YRS 5.0-1416:BT:932 - realted to YRS 5.0-1339:existing code: pass BenefitOptionId into validateNonHuman() when money having both plan or retriement only,if money is in saving plan only so it will not working,Changed code: for saving plan :use savingoptionId,for retirement and both :use benefitoptionId   
'bhavna S           18-May-2012     YRS 5.0-1470: Link to Address Edit program from Person Maintenance 
'Anudeep            08-10-2012      Bt-1245:Address issue replication and analysis on client machine 
'Anudeep            11-03-2013      Bt-1236:YRS 5.0-1685:Add Category/Type field to Power of attorney and allow 3 types
'Shashank Patel		2013.04.12		YRS 5.0-1990:similar SSNs are being updates across the board
'Anudeep            13.04.2013      BT-1555:YRS 5.0-1769:Length of phone numbers
'Anudeep            20.06.2013      BT-1555:YRS 5.0-1769:Length of phone numbers
'Anudeep            21.06.2013      BT-1555:YRS 5.0-1769:Length of phone numbers
'Anudeep			01.06.2013 		BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
'Anudeep            29.07.2013      Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
'Sanjay R.          2013.08.05      YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
'Anudeep            2013.11.07      BT:2269-YRS 5.0-2234:Change labelling of Power of Attorney / POW 
'Anudeep            2013.11.13      BT:2190-YRS 5.0-2199:Create view mode for Power of Attorney display 
'Anudeep            2014.02.16      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Anudeep            2014.02.20      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Anudeep            2014.05.26      BT:2541-YRS 5.0-2372 - Address records being deactivated, also causing Address Change letter 
'Anudeep            2014.05.26      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Anudeep            2014.05.28      BT:2306:YRS 5.0-2251 : YRS enhancement-in Beneficiary tab, allow default option to use participant address
'Sanjay             2014.07.09      BT 2593 - UI changes in Beneficiary information page
'Anudeep            2015.05.05      BT:2824 : YRS 5.0-2499:Web Service for Password Rewrite
'Manthan Rajguru    2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace/reference (Changed the Namespace/reference from 'Infotech' to 'YMCARET')
'Pramod P. Pokale   2015.10.13      YRS-AT-2588: implement some basic telephone number validation Rules
'Manthan Rajguru    2016.04.22      YRS-AT-2206 -  Applying Fees and deductions to death payments - Part A.1 
'Manthan Rajguru    2016.07.18      YRS-AT-2919 -  YRS Enh: Beneficiary Details - nonPerson - should allow optional Date
'Pooja Kumkar       2019.10.04      YRS-AT-4605 -  YRS enh:State Withholding Project - Beneficiary Settlement
'Megha Lad          2019.11.27      YRS-AT-4719 - State Withholding - Additional text & warning messages for AL, CA and MA.
'************************************************************************************************************

Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Web.Services ' - Manthan | 2016.04.22 | YRS-AT-2206 | Added namespace for Web method

Public Class BeneficiaryInformation
    Inherits System.Web.UI.Page
    'below line is Added by Neeraj for issue id YRS 5.0-940 
    Dim strFormName As String = New String("BeneficiaryInformation.aspx")
    'End issue id YRS 5.0-940

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ButtonDeathNotification As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonGeneralAddress As System.Web.UI.WebControls.Button
    Protected WithEvents txtboxSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtboxFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtboxFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtboxLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtboxCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtboxState As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnFind As System.Web.UI.WebControls.Button
    Protected WithEvents btnClear As System.Web.UI.WebControls.Button
    Protected WithEvents TabStripRetireesInformation As Microsoft.Web.UI.WebControls.TabStrip
    Protected WithEvents ButtonTelephone As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridBankInfoList As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridFederalWithholding As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridAnnuities As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridAnnuitiesPaid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridBeneficiaries As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGridBeneficiariesGroupName As System.Web.UI.WebControls.DataGrid
    Protected WithEvents btnRetireesInfoSave As System.Web.UI.WebControls.Button
    Protected WithEvents btnRetireesInfoCancel As System.Web.UI.WebControls.Button
    Protected WithEvents btnRetireesInfoPHR As System.Web.UI.WebControls.Button
    Protected WithEvents btnRetireesInfoOK As System.Web.UI.WebControls.Button
    Protected WithEvents DataGridGeneralWithhold As System.Web.UI.WebControls.DataGrid
    'Commented below line :Anudeep:11-03-2013-Bt-1236:YRS 5.0-1685:Add Category/Type field to Power of attorney and allow 3 types
    'Protected WithEvents GridViewRetireesAttorney As System.Web.UI.WebControls.GridView
    Protected WithEvents DataGridNotes As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ButtonRetireesInfoSave As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonRetireesInfoCancel As System.Web.UI.WebControls.Button
    Protected WithEvents LabelAnnuitiesSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents LabelWelcome As System.Web.UI.WebControls.Label
    Protected WithEvents LabelHeading As System.Web.UI.WebControls.Label

    Protected WithEvents ButtonGeneralEmailId As System.Web.UI.WebControls.Button

    Protected WithEvents LabelListFundNo As System.Web.UI.WebControls.Label
    Protected WithEvents LabelListFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents LabelListLastName As System.Web.UI.WebControls.Label
    Protected WithEvents LabelListCity As System.Web.UI.WebControls.Label
    Protected WithEvents LabelState As System.Web.UI.WebControls.Label
    Protected WithEvents LabelGeneralSalute As System.Web.UI.WebControls.Label
    Protected WithEvents cboSalute As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelGeneralFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGeneralFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGeneralMiddleName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGeneralMiddleName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGeneralLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGeneralLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGeneralSuffixName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGeneralSuffix As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGeneralSSNo As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGeneralSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGeneralQDROPending As System.Web.UI.WebControls.Label
    Protected WithEvents chkGeneralQDROPending As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LabelGeneralQDROStatudDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGeneralQDROStatudDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGeneralQDROStatus As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGeneralFundNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGeneralGender As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListGeneralGender As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelGeneralMaritalStatus As System.Web.UI.WebControls.Label
    Protected WithEvents cboGeneralMaritalStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LabelGeneralDOB As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGeneralDOB As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGeneralRetireDate As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGeneralRetireDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGeneralDateDeceased As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGeneralDateDeceased As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelGeneralAddress1 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAddress1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelGeneralAddress2 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAddress2 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelGeneralAddress3 As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxAddress3 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelGeneralCity As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxCity As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelGeneralState As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxState As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelZip As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxZip As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LabelCountry As System.Web.UI.WebControls.Label
    'Protected WithEvents TextBoxCountry As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGeneralTelephone As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxTelephone As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelEmailId As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxEmailId As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelPOA As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGeneralPOA As System.Web.UI.WebControls.TextBox

    'Protected WithEvents TextBoxSecAddress1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecAddress2 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecAddress3 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecCity As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecState As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecZip As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TextBoxSecCountry As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxSecTelephone As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBoxSecEmail As System.Web.UI.WebControls.TextBox

    Protected WithEvents TextBoxAnnuitiesSSNo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAnnuitiesFirstName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuitiesFirstName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAnnuitiesMiddleName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuitiesMiddleName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAnnuitiesLastName As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuitiesLastName As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelAnnuitiesDOB As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxAnnuitiesDOB As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelDateDeceased As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxDateDeceased As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelRetiredPrimaryPercent As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxRetiredPrimaryPercent As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelRetiredCont1Percent As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxRetiredCont1Percent As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelRetiredCont2Percent As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxRetiredCont2Percent As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelRetiredCont3Percent As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxRetiredCont3Percent As System.Web.UI.WebControls.TextBox

    Protected WithEvents LabelGroupNamePrimaryRetDB As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGroupNamePrimaryRetDB As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGroupNameCon1RetDB As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGroupNameCon1RetDB As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGroupNameCon2RetDB As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGroupNameCon2RetDB As System.Web.UI.WebControls.TextBox
    Protected WithEvents LabelGroupNameCon3RetDB As System.Web.UI.WebControls.Label
    Protected WithEvents LabelAnnuityOption_RetirementPlan As System.Web.UI.WebControls.Label
    Protected WithEvents TextBoxGroupNameCon3RetDB As System.Web.UI.WebControls.TextBox
    Protected WithEvents ButtonBankingInfoUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents ButtonGeneralPOA As System.Web.UI.WebControls.Button
    Protected WithEvents RadioButtonAnnuityType_RetirementPlan As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents MultiPageRetireesInformation As Microsoft.Web.UI.WebControls.MultiPage
    Protected WithEvents ButtonGeneralWithholdAdd As System.Web.UI.WebControls.Button
    'Protected WithEvents DropdownlistSecCountry As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents DropDownListCountry As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents DropdownlistState As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents DropdownlistSecState As System.Web.UI.WebControls.DropDownList
    Protected WithEvents MenuRetireesInformation As skmMenu.Menu
    Protected WithEvents AddressWebUserControl1 As AddressUserControlNew
    Protected WithEvents AddressWebUserControl2 As AddressUserControlNew
    Protected WithEvents PlaceHolder1 As System.Web.UI.WebControls.PlaceHolder
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents LabelAnnuityOption_SavingsPlan As System.Web.UI.WebControls.Label
    Protected WithEvents RadioButtonAnnuityType_SavingsPlan As System.Web.UI.WebControls.RadioButtonList
    'AA:16.02.2013 : BT:2306 :YRS 5.0-2251 -Aded two link buttons to get the participant address
    Protected WithEvents lnkParticipantAddress1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkParticipantAddress2 As System.Web.UI.WebControls.LinkButton

    'Start-SR:2014.07.09-BT 2593 - UI changes in Beneficiary information page
    Protected WithEvents Headercontrol As YMCA_Header_WebUserControl
    Protected WithEvents divSavingPlanAnnuityoption As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents divRetirementPlanAnnuityoption As System.Web.UI.HtmlControls.HtmlGenericControl
    'End-SR:2014.07.09-BT 2593 - UI changes in Beneficiary information page
    'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Declaring controls
    Protected WithEvents lnkDeductions As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents lblDedunctionsmsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblDeductionamt As System.Web.UI.WebControls.Label
    Protected WithEvents txtFundCostAmt As System.Web.UI.WebControls.TextBox
    Protected WithEvents dgDeductions As System.Web.UI.WebControls.DataGrid
    Protected WithEvents chkBoxDeduction As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblPlantype As System.Web.UI.WebControls.Label
    'End - Manthan | 2016.04.22 | YRS-AT-2206 | Declaring controls
    'START: PK |10.04.2019| YRS-AT-4605 |Declare Controls
    Public WithEvents stwListUserControl As StateWithholdingListingControl
    Public WithEvents divErrorMsg As System.Web.UI.HtmlControls.HtmlGenericControl
    'END: PK |12.04.2019| YRS-AT-4605 |Declare Controls 

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Dim l_DataSet_BeneficiaryInformation As DataSet
    Dim l_DataSet_BeneficiaryInformation_SavingsPlan As DataSet
    Dim l_DataSet_POA As DataSet
    'Priya 2010.08.11 : made shared variable into dim for YRS 5.0-1147 : Issue with simultaneous users doing death settlement 
    'Shared l_string_BenefitOptionID As String
    'Shared l_string_BenefitOptionID_SavingsPlan As String
    Dim l_string_BenefitOptionID As String
    Dim l_string_BenefitOptionID_SavingsPlan As String

    'Shared l_string_SSNo As String
    'Shared l_string_PrevSSNo As String
    Dim l_string_SSNo As String
    Dim l_string_PrevSSNo As String
    ''Dim l_String_guiUniqueID As String
    'Shared TaxWithHoldRequired As Boolean
    Dim TaxWithHoldRequired As Boolean


    Dim l_bool_CloseForm As Boolean
    Dim str_Pri_Address1 As String
    Dim str_Pri_Address2 As String
    Dim str_Pri_Address3 As String
    Dim str_Pri_City As String
    Dim str_Pri_Zip As String
    Dim str_Pri_CountryValue As String
    Dim str_Pri_StateValue As String
    Dim str_Pri_CountryText As String
    Dim str_Pri_StateText As String
    Dim str_Sec_Address1 As String
    Dim str_Sec_Address2 As String
    Dim str_Sec_Address3 As String
    Dim str_Sec_City As String
    Dim str_Sec_Zip As String
    Dim str_Sec_CountryValue As String
    Dim str_Sec_StateValue As String
    Dim str_Sec_CountryText As String
    Dim str_Sec_StateText As String
    Dim l_Str_Msg As String
    Dim l_int_Ok As Integer
    Dim l_show_MessageBox As Boolean    'NP:PS:2007.09.17 - This variable keeps track of whether we have already shown a message box on this page or not.

#Region "FormProperties"

    Private Property DataSet_LookUpBenefitInformation() As DataSet
        Get
            If Not (Session("DataSet_LookUpBenefitInformation")) Is Nothing Then
                Return Session("DataSet_LookUpBenefitInformation")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("DataSet_LookUpBenefitInformation") = Value
        End Set
    End Property ''
    Private Property DataSet_LookUpBenefitInformation_SavingsPlan() As DataSet
        Get
            If Not (Session("DataSet_LookUpBenefitInformation_SavingsPlan")) Is Nothing Then
                Return Session("DataSet_LookUpBenefitInformation_SavingsPlan")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As DataSet)
            Session("DataSet_LookUpBenefitInformation_SavingsPlan") = Value
        End Set
    End Property
    Private Property CloseFormStatus() As Boolean
        Get
            If Not (Session("l_bool_CloseForm")) Is Nothing Then
                Return Session("l_bool_CloseForm")
            Else
                Return True
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("l_bool_CloseForm") = Value
        End Set
    End Property

    Private Property GetUniqueID() As String
        Get

            If Not (Session("l_String_guiUniqueID")) Is Nothing Then
                Return Session("l_String_guiUniqueID")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As String)
            Session("l_String_guiUniqueID") = Value
        End Set
    End Property

    Private Property FormCalledForFirstTime() As Boolean
        Get
            If Not (Session("FormCalledForFirstTime")) Is Nothing Then
                Return Session("FormCalledForFirstTime")
            Else
                Return True
            End If
        End Get
        Set(ByVal Value As Boolean)
            Session("FormCalledForFirstTime") = Value
        End Set
    End Property
    'START :PK |2019.10.30 | YRS-AT-4605 | Variable to Store Annuity Amount
    Public Property AnnuityAmount As String
        Get
            If Not (ViewState("AnnuityAmount")) Is Nothing Then
                Return (CType(ViewState("AnnuityAmount"), String))
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As String)
            ViewState("AnnuityAmount") = Value
        End Set
    End Property
    'START : ML | 2019.11.27 | YRS-AT-4719 | Declared Proerty to Store Feredal Amount
    Public Property stwFederalAmount As Double?
        Get
            Return ViewState("stwFederalAmount")
        End Get
        Set(value As Double?)
            ViewState("stwFederalAmount") = value
        End Set
    End Property
    Public Property stwFederalType As String
        Get
            Return ViewState("stwFederalType")
        End Get
        Set(value As String)
            ViewState("stwFederalType") = value
        End Set
    End Property
    'END : ML | 2019.11.27 | YRS-AT-4719 | Declared Proerty to Store Feredal Amount
    'END :PK|2019.10.30 | YRS-AT-4605 | Variable to Store Annuity Amount

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Session("LoggedUserKey") Is Nothing Then
            Response.Redirect("Login.aspx", True)
            Exit Sub
        End If

        'To Allow only numeric values
        'Me.TextBoxTelephone.Attributes.Add("onkeypress", "Javascript:return HandleAmountFilteringWithNoDecimals(this);")
        ' Me.TextBoxSecTelephone.Attributes.Add("onkeypress", "Javascript:return HandleAmountFilteringWithNoDecimals(this);")
        'Anudeep:13.04.2013 :BT-1555:YRS 5.0-1769:Length of phone numbers
        Me.TextBoxTelephone.Attributes.Add("onclick", "javascript:return Validatephone(document.Form1.all.TextBoxTelephone);")
        Me.TextBoxTelephone.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxTelephone);")
        'Anudeep:13.04.2013 :BT-1555:YRS 5.0-1769:Length of phone numbers
        Me.TextBoxSecTelephone.Attributes.Add("onclick", "javascript:return Validatephone(document.Form1.all.TextBoxSecTelephone);")
        Me.TextBoxSecTelephone.Attributes.Add("onblur", "javascript:return ValidateTelephoneNo(document.Form1.all.TextBoxSecTelephone);")
        'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
        Me.TextBoxTelephone.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
        Me.TextBoxSecTelephone.Attributes.Add("OnKeyPress", "javascript:ValidateNumeric();")
        Me.LabelGeneralSalute.AssociatedControlID = Me.cboSalute.ID
        Me.LabelGeneralFirstName.AssociatedControlID = Me.TextBoxGeneralFirstName.ID
        Me.LabelGeneralMiddleName.AssociatedControlID = Me.TextBoxGeneralMiddleName.ID
        Me.LabelGeneralLastName.AssociatedControlID = Me.TextBoxGeneralLastName.ID
        Me.LabelGeneralSuffixName.AssociatedControlID = Me.TextBoxGeneralSuffix.ID
        Me.LabelGeneralSSNo.AssociatedControlID = Me.TextBoxGeneralSSNo.ID
        Me.LabelGeneralGender.AssociatedControlID = Me.DropDownListGeneralGender.ID
        Me.LabelGeneralMaritalStatus.AssociatedControlID = Me.cboGeneralMaritalStatus.ID
        Me.LabelGeneralDOB.AssociatedControlID = Me.TextBoxGeneralDOB.ID
        Me.LabelPOA.AssociatedControlID = Me.TextBoxGeneralPOA.ID
        Me.TextBoxGeneralSuffix.MaxLength = 6

        '' MenuRetireesInformation.DataSource = Server.MapPath("SimpleXML.xml")
        '' MenuRetireesInformation.DataBind()
        TextBoxGeneralFundNo.ReadOnly = True
        Try
            If (Session("POAClicked") = True) Then

                ' l_DataSet_POA = Session("POADetailsStore")
                'BT-706 bhavna 2011.07.22:doesnt fill dataset by Session("POADetailsStore") because this session will not reflect current POA which have add recently 
                If l_DataSet_POA Is Nothing Then
                    l_DataSet_POA = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpPOAInfo(Session("PersId"))
                    If HelperFunctions.isEmpty(l_DataSet_POA) Then
                        Session("POADetailsStore") = l_DataSet_POA
                    End If
                End If
                LoadPOADetails(l_DataSet_POA)
                'BT-706

                'Dim l_DataSet_POA As DataSet = Session("POADetailsStore")
                'If (l_DataSet_POA Is Nothing) Then
                '    l_DataSet_POA = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpPOAInfo(Session("PersId"))
                '    If l_DataSet_POA.Tables("POAInfo").Rows.Count > 0 Then
                '        If Not (IsDBNull(l_DataSet_POA.Tables("POAInfo").Rows(0).Item("Name1"))) Then
                '            'bhavnaS :BT 706:to display active poa separated by ,
                '            Dim builder As New StringBuilder
                '            Dim vStr As String
                '            For Each dr As DataRow In l_DataSet_POA.Tables("POAInfo").Rows

                '                vStr = builder.Append(dr("Name1").ToString() + ",").ToString()

                '            Next
                '            TextBoxGeneralPOA.Text = vStr.Remove(vStr.Length - 1, 1)
                '            'bhavnaS BT 706
                '            'TextBoxGeneralPOA.Text = l_DataSet_POA.Tables("POAInfo").Rows(0).Item("Name1").ToString()
                '        End If
                '    End If
                '    Session("POADetailsStore") = l_DataSet_POA
                'Else
                '    If (l_DataSet_POA.Tables("POAInfo").Rows.Count > 0) Then
                '        If l_DataSet_POA.Tables("POAInfo").Rows.Count > 0 Then
                '            If Not (IsDBNull(l_DataSet_POA.Tables("POAInfo").Rows(0).Item("Name1"))) Then
                '                TextBoxGeneralPOA.Text = l_DataSet_POA.Tables("POAInfo").Rows(0).Item("Name1").ToString()
                '            End If
                '        End If
                '    End If
                '    'bhavnaS 2011.07.11, YRS 9.0-706: this session("POAName") will take current value of POAName from RetireesPowerAttorneyWebForm.aspx,if it is null/blank then it will come from database. 
                '    '    If Not Session("POAName") = String.Empty Then
                '    '        TextBoxGeneralPOA.Text = Session("POAName").ToString() 'Add by BhavnaS 2011.07.08 for YRS 9.0-706

                '    '    Else
                '    '        If l_DataSet_POA.Tables("POAInfo").Rows.Count > 0 Then
                '    '            If Not (IsDBNull(l_DataSet_POA.Tables("POAInfo").Rows(0).Item("Name1"))) Then
                '    '                TextBoxGeneralPOA.Text = l_DataSet_POA.Tables("POAInfo").Rows(0).Item("Name1").ToString()
                '    '            End If
                '    '        End If
                '    '    End If


                'End If
                Session("POAClicked") = Nothing
            End If

            If Session("blnAddFedWithHoldings") = True Or Session("blnUpdateFedWithHoldings") = True Then
                Me.MultiPageRetireesInformation.SelectedIndex = 1
                Me.TabStripRetireesInformation.SelectedIndex = 1
                LoadFedWithDrawalTab()
            End If
            stwListUserControl.STWDataSaveAtMainPage = True 'PK |12.06.2019| YRS-AT-4605 | Set Session Variable for State Withholding User Control
            ''If Not Page.IsPostBack And Me.FormCalledForFirstTime = True Then
            If Not Page.IsPostBack Then
                SessionManager.SessionStateWithholding.LstSWHPerssDetail = Nothing 'PK | 12/04/2019 |YRS-AT-4605 | Clear Session Variable.
                TextBoxGeneralPOA.ReadOnly = True 'BS:2011.08.02:BT:911- TextBoxPOA should be readonly if multiple poa exist and all editing/adding of Poa information handle on POAscreen so there is no need to enable Poa Textbox
                'added by hafiz on 2-May-2007 for YREN-3112
                Dim l_dataset_MaritalTypes As DataSet
                l_dataset_MaritalTypes = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.MaritalTypes(0)
                cboGeneralMaritalStatus.DataSource = l_dataset_MaritalTypes
                cboGeneralMaritalStatus.DataTextField = "Description"
                cboGeneralMaritalStatus.DataValueField = "Code"
                cboGeneralMaritalStatus.DataBind()
                'NP:PS:2007.09.05 - Set the default value to Unknown as per conf call with Elliot
                If Not cboGeneralMaritalStatus.Items.FindByText("Unknown") Is Nothing Then
                    cboGeneralMaritalStatus.SelectedIndex = cboGeneralMaritalStatus.Items.IndexOf(cboGeneralMaritalStatus.Items.FindByText("Unknown"))
                End If



                'added by hafiz on 2-May-2007 for YREN-3112
                'Start : added by Dilip yadav on 2009.09.08 for YRS 5.0-852
                Dim l_dataset_GenderTypes As DataSet
                l_dataset_GenderTypes = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.GenderTypes()
                If HelperFunctions.isNonEmpty(l_dataset_GenderTypes) Then
                    Me.DropDownListGeneralGender.DataSource = l_dataset_GenderTypes.Tables(0)
                    Me.DropDownListGeneralGender.DataTextField = "Description"
                    Me.DropDownListGeneralGender.DataValueField = "Code"
                    Me.DropDownListGeneralGender.DataBind()
                End If
                'End :  by Dilip yadav on 2009.09.08 for YRS 5.0-852
                Session("Sort_BenInfo") = Nothing
                Session("CurrentForm") = "BI"


                RadioButtonAnnuityType_RetirementPlan.Visible = False
                RadioButtonAnnuityType_SavingsPlan.Visible = False
                LabelAnnuityOption_RetirementPlan.Visible = False
                LabelAnnuityOption_SavingsPlan.Visible = False
                'Start- SR:2014.07.09 - BT 2593 - UI changes in Beneficiary information page
                divRetirementPlanAnnuityoption.Visible = False
                divSavingPlanAnnuityoption.Visible = False
                'End- SR:2014.07.09 - BT 2593 - UI changes in Beneficiary information page

                'Priya 2010.08.11 : Added value to viewstate to add persistence mechanism for YRS 5.0-1147
                TaxWithHoldRequired = False  'SR:2010.09.25 variable set instead of viewstate for withholding issue
                'ViewState("TaxWithHoldRequired") = False
                'End 2010.08.11 
                Session("l_String_guiUniqueID") = Nothing
                GetUniqueID = ""
                l_string_BenefitOptionID = ""
                l_string_BenefitOptionID_SavingsPlan = ""
                CloseFormStatus = True

                Session("Success_BIScreen") = False
                TextBoxGeneralSSNo.MaxLength = 9

                If Session("blnAddFedWithHoldings") = True Or Session("blnUpdateFedWithHoldings") = True Then
                    'Me.MultiPageRetireesInformation.SelectedIndex = 1
                    'Me.TabStripRetireesInformation.SelectedIndex = 1
                    'LoadFedWithDrawalTab()

                Else

                    Session("PersId") = Nothing
                    Session("BeneficiaryPersonalDetails") = Nothing
                    Session("AddressDetailsStore") = Nothing
                    Session("POADetailsStore") = Nothing
                    GetUniqueID = Nothing
                    DataSet_LookUpBenefitInformation = Nothing
                    DataSet_LookUpBenefitInformation_SavingsPlan = Nothing
                    'Session("TextBoxGeneralSSNo") = Nothing
                    ViewState("TextBoxGeneralSSNo") = Nothing
                    Session("FedWithDrawals") = Nothing

                    Session("SP_Parameters_DeathBenefitOptionID") = Nothing
                    Session("SP_Parameters_AnnuityOption") = Nothing
                    Session("POAClicked") = Nothing
                End If

                Dim l_DataSet_CountryNames As DataSet

                'If Not Request.QueryString("BenefitOptionID") Is Nothing OrElse Not Request.QueryString("BenefitOptionID") = "" Then
                If (Not Session("BS_SelectedOption_RP") Is Nothing OrElse Not Session("BS_SelectedOption_RP") = "") OrElse _
                  (Not Session("BS_SelectedOption_SP") Is Nothing OrElse Not Session("BS_SelectedOption_SP") = "") Then

                    ''Store Country Details
                    Session("CountryNames") = l_DataSet_CountryNames

                    'Moving storage of Option Id to a session variable
                    'l_string_BenefitOptionID = Request.QueryString("BenefitOptionID").ToString()
                    'l_string_BenefitOptionID_SavingsPlan = Request.QueryString("BenefitOptionID_SavingsPlan").ToString()
                    l_string_BenefitOptionID = ""
                    l_string_BenefitOptionID_SavingsPlan = ""
                    If (Not Session("BS_SelectedOption_RP") Is Nothing AndAlso Not Session("BS_SelectedOption_RP") = "") Then
                        l_string_BenefitOptionID = Session("BS_SelectedOption_RP")
                        l_DataSet_BeneficiaryInformation = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_BeneficiaryInformation(l_string_BenefitOptionID)
                    End If
                    If (Not Session("BS_SelectedOption_SP") Is Nothing AndAlso Not Session("BS_SelectedOption_SP") = "") Then
                        l_string_BenefitOptionID_SavingsPlan = Session("BS_SelectedOption_SP")
                        l_DataSet_BeneficiaryInformation_SavingsPlan = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_BeneficiaryInformation(l_string_BenefitOptionID_SavingsPlan)
                    End If

                    If l_string_BenefitOptionID <> "" AndAlso HelperFunctions.isEmpty(l_DataSet_BeneficiaryInformation) Then
                        CloseFormStatus = showMessage(PlaceHolder1, "Error", "Unable to Locate a Beneficary Record for Benefit option", MessageBoxButtons.Stop) 'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                        Exit Sub
                    Else
                        DataSet_LookUpBenefitInformation = l_DataSet_BeneficiaryInformation
                    End If
                    If l_string_BenefitOptionID_SavingsPlan <> "" AndAlso HelperFunctions.isEmpty(l_DataSet_BeneficiaryInformation_SavingsPlan) Then
                        CloseFormStatus = showMessage(PlaceHolder1, "Error", "Unable to Locate a Beneficary Record for Benefit option", MessageBoxButtons.Stop) 'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                        Exit Sub
                    Else
                        DataSet_LookUpBenefitInformation_SavingsPlan = l_DataSet_BeneficiaryInformation_SavingsPlan
                    End If

                    'BS:2011.09.15:YRS 5.0-1416:BT:932 --if both benifitoptionID is null
                    If (l_string_BenefitOptionID Is Nothing Or l_string_BenefitOptionID = String.Empty) And (l_string_BenefitOptionID_SavingsPlan Is Nothing Or l_string_BenefitOptionID_SavingsPlan = String.Empty) Then
                        CloseFormStatus = showMessage(PlaceHolder1, "Error", "Unable to Settle a Beneficary Record for Benefit option", MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                    ''Show SSNo to Form
                    SetValuesToForm()

                    'Vipul 26Dec06 - To Handle Beneficiary information with blank SSN 
                    'If (TextBoxGeneralSSNo.Text.Trim() <> "") Then
                    'Vipul 26Dec06 - To Handle Beneficiary information with blank SSN 

                    ''//Show Person details to Form  for selected SSNo
                    Lookup_PersonalDetails(TextBoxGeneralSSNo.Text.Trim())

                    'Ashutosh Patil as on 14-Mar-2007
                    'YREN - 3028,YREN-3029

                    'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    'If Not Session("PersId") = "" And GetUniqueID <> "AddMode" Then
                    '    Dim dsAddress As DataSet
                    '    'AddressWebUserControl1.guiPerssId = Session("PersId")
                    '    AddressWebUserControl1.IsPrimary = 1
                    '    AddressWebUserControl2.IsPrimary = 0

                    'End If
                    'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
                    'If Not Session("TextBoxGeneralSSNo") Is Nothing Then
                    TextBoxGeneralSSNo.Text = l_string_SSNo 'Session("TextBoxGeneralSSNo")
                    'End If
                    ''Show/Hide Annuity Option Details
                    LookupDeathBenefitOption()
                    '' ValidateSettlementPrerequisites()
                    'End If
                Else
                    'showMessage(PlaceHolder1, "Error", "Beneficiary Information Details could not be found. Error !! ", MessageBoxButtons.Stop)    'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                    CloseFormStatus = True
                End If
                'BS:2011.08.10:YRS 5.0-1339:BT:852 - Reopen
                Dim dsValidateNonHumanData As Boolean
                Dim string_BenefitOptionID As Boolean
                'BS:2011.09.15:YRS 5.0-1416:BT:932-related to YRS 5.0-1339, l_string_BenefitOptionID_SavingsPlan: use for Saving Paln only,l_string_BenefitOptionID:use for Retriement and both paln
                If (l_string_BenefitOptionID IsNot Nothing AndAlso l_string_BenefitOptionID <> String.Empty) Then
                    dsValidateNonHumanData = ValidateNonHumanInfo(l_string_BenefitOptionID)
                Else
                    dsValidateNonHumanData = ValidateNonHumanInfo(l_string_BenefitOptionID_SavingsPlan)
                End If
                If dsValidateNonHumanData = True Then
                    DropDownListGeneralGender.Items.Insert(0, "X")
                    DropDownListGeneralGender.SelectedValue = "X"
                    DropDownListGeneralGender.Enabled = False
                    cboGeneralMaritalStatus.Items.Insert(0, "X")
                    cboGeneralMaritalStatus.SelectedValue = "X"
                    cboGeneralMaritalStatus.Enabled = False
                    'Start - Manthan Rajguru | 2016.07.04 | YRS-AT-2919 | Commented existing code to enable date control and allowing empty date field
                    'TextBoxGeneralDOB.Text = String.Empty
                    'TextBoxGeneralDOB.Enabled = False
                    LabelGeneralDOB.Text = "Established Date"
                    TextBoxGeneralDOB.ReadOnly = False
                    'End - Manthan Rajguru | 2016.07.04 | YRS-AT-2919 | Commented existing code to enable date control and allowing empty date field
                Else
                    DropDownListGeneralGender.Enabled = True
                    cboGeneralMaritalStatus.Enabled = True
                    TextBoxGeneralDOB.Enabled = True
                End If
                AddressWebUserControl1.IsPrimary = 1
                AddressWebUserControl2.IsPrimary = 0
                'Start-SR:2014.07.09-BT 2593 - UI changes in Beneficiary information page
                Headercontrol.PageTitle = " Beneficiary Information"
                Headercontrol.SSNo = l_string_SSNo.Trim()
                'End-SR:2014.07.09-BT 2593 - UI changes in Beneficiary information page
                'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Calling method to load deductions in grid and clearing session value
                LoadDeductions(dgDeductions)
                Session("FinalDeductionsAnnuity") = Nothing
                Session("TotDeductionsAmt") = Nothing
                'End - Manthan | 2016.04.22 | YRS-AT-2206 | Calling method to load deductions in grid and clearing session value
            End If

            'Ashutosh Patil as on 14-Mar-2007
            'YREN - 3028,YREN-3029
            If Request.Form("OK") = "OK" And CloseFormStatus = True Then
                If Session("AddressOk") <> 1 Then
                    CloseForm()
                Else
                    Session("AddressOk") = 0
                End If

            End If
            'BS:2012.05.17:YRS 5.0-1470: here check if page request is yes then save data in database
            If Session("VerifiedAddress") = "VerifiedAddress" Then
                If Request.Form("Yes") = "Yes" Then
                    Session("VerifiedAddress") = ""
                    If (SaveDetails() = False) Then
                        Exit Sub
                    Else
                        CloseForm()
                    End If
                ElseIf Request.Form("No") = "No" Then
                    Session("VerifiedAddress") = ""
                    Exit Sub
                End If
            End If

            If Request.Form("Yes") = "Yes" Then
                'Vipul 03Feb06 Cache-Session
                'Dim cache As CacheManager
                'cache = CacheFactory.GetCacheManager()

                l_string_SSNo = TextBoxGeneralSSNo.Text
                Session("PersId") = Nothing
                Session("BeneficiaryPersonalDetails") = Nothing
                Session("AddressDetailsStore") = Nothing
                Session("POADetailsStore") = Nothing
                GetUniqueID = Nothing
                DataSet_LookUpBenefitInformation = Nothing
                DataSet_LookUpBenefitInformation_SavingsPlan = Nothing
                Session("FedWithDrawals") = Nothing

                Session("SP_Parameters_DeathBenefitOptionID") = Nothing
                Session("SP_Parameters_AnnuityOption") = Nothing
                Session("POAClicked") = Nothing
                'Session("TextBoxGeneralSSNo") = TextBoxGeneralSSNo.Text
                ViewState("TextBoxGeneralSSNo") = TextBoxGeneralSSNo.Text
                Lookup_PersonalDetails(l_string_SSNo)
                LookupDeathBenefitOption()
                '   SaveDetails()
            End If

            'Start - Manthan | 2016.04.22 | YRS-AT-2206 | To display total deductions amt in the label on post back            
            If Not Session("TotDeductionsAmt") Is Nothing Then
                lblDeductionamt.Text = Session("TotDeductionsAmt")
            End If
            'End - Manthan | 2016.04.22 | YRS-AT-2206 | To display total deductions amt in the label on post back
            'START: PK | 2019.10.30 | YRS-AT-4605 | Refresh State Withholding User Control and validate method
            stwListUserControl.Refresh()
            If Me.ValidateSTWvsFedtaxforMA() = False Then
                Exit Sub
            End If
           
            'END: PK | 2019.10.30 | YRS-AT-4605 | Refresh State Withholding User Control and validate method
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try

    End Sub

    Private Sub RetireesInformationTabStrip_SelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabStripRetireesInformation.SelectedIndexChange
        Dim i As Integer
        Try
            Me.MultiPageRetireesInformation.SelectedIndex = Me.TabStripRetireesInformation.SelectedIndex

            'VP 18Spet07 .. Trigger Validator Controls & not allow user to chnage tab,  if any validator control fails
            'i.e. Stay on General tab (Withholding tab not allowed)
            Me.Validate()

            For i = 0 To Me.Validators.Count() - 1
                If Not Me.Validators.Item(i).IsValid Then
                    TabStripRetireesInformation.SelectedIndex = 0
                    Me.MultiPageRetireesInformation.SelectedIndex = 0
                    Exit Sub
                End If
            Next
            'VP 18Spet07 .. Trigger Validator Controls & not allow user to chnage tab,  if any validator control fails

            If (TabStripRetireesInformation.SelectedIndex = 1) Then
                LoadFedWithDrawalTab()
                'START : PK| 10/10/2019 |YRS-AT-4605 | code to refresh state value 
                stwListUserControl.PersonStateName = AddressWebUserControl1.DropDownListStateText
                stwListUserControl.PersonStateCode = AddressWebUserControl1.DropDownListStateValue
                'START : PK| 10/10/2019 |YRS-AT-4605 | code to refresh state value
            End If
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try

    End Sub

#Region "Code Manupulation Functions/Procedures"

    'This function obtains information about the selected option from the Session and sets the values on the page
    Private Sub SetValuesToForm()

        Dim l_DataSet_SelectedOption As DataSet
        Dim l_DataTable_SelectedOption As DataTable
        Dim l_DataSet_SelectedOption_SavingsPlan As DataSet
        Dim l_DataTable_SelectedOption_SavingsPlan As DataTable

        Try
            l_DataSet_SelectedOption = Me.DataSet_LookUpBenefitInformation
            l_DataSet_SelectedOption_SavingsPlan = Me.DataSet_LookUpBenefitInformation_SavingsPlan
            If HelperFunctions.isEmpty(l_DataSet_SelectedOption) Then
                divRetirementPlanAnnuityoption.Visible = False  'SR:2014.07.09 - BT 2593 - UI changes in Beneficiary information page
                RadioButtonAnnuityType_RetirementPlan.Visible = False
                LabelAnnuityOption_RetirementPlan.Visible = False
            End If
            If HelperFunctions.isEmpty(l_DataSet_SelectedOption_SavingsPlan) Then
                divRetirementPlanAnnuityoption.Visible = False  'SR:2014.07.09 - BT 2593 - UI changes in Beneficiary information page
                RadioButtonAnnuityType_RetirementPlan.Visible = False
                LabelAnnuityOption_RetirementPlan.Visible = False

            End If
            If HelperFunctions.isEmpty(l_DataSet_SelectedOption) AndAlso HelperFunctions.isEmpty(l_DataSet_SelectedOption_SavingsPlan) Then
                TextBoxGeneralSSNo.Text = ""
                Exit Sub
            End If
            'Perform calculations for the Retirement Plan
            'Start- SR:2014.07.09 - BT 2593 - UI changes in Beneficiary information page
            divRetirementPlanAnnuityoption.Visible = False
            RadioButtonAnnuityType_RetirementPlan.Visible = False
            LabelAnnuityOption_RetirementPlan.Visible = False
            'End- SR:2014.07.09 - BT 2593 - UI changes in Beneficiary information page
            If HelperFunctions.isNonEmpty(l_DataSet_SelectedOption) Then
                l_DataTable_SelectedOption = l_DataSet_SelectedOption.Tables("r_BenefitInformation")

                'MultiPageRetireesInformation.SelectedIndex = 0
                'TabStripRetireesInformation.SelectedIndex = 0
                'If (Session("TextBoxGeneralSSNo") Is Nothing) Then
                If (l_string_SSNo Is Nothing) Then
                    ViewState("TextBoxGeneralSSNo") = l_DataTable_SelectedOption.Rows(0)("BeneficiaryTaxNumber").ToString().Trim()
                    l_string_SSNo = ViewState("TextBoxGeneralSSNo")
                Else
                    l_string_SSNo = ViewState("TextBoxGeneralSSNo")
                End If
                'Priya 2010.08.11 : Added veiewstate for persistence mechanism for YRS 5.0-1147 
                'l_string_PrevSSNo = l_DataTable_SelectedOption.Rows(0)("BeneficiaryTaxNumber").ToString().Trim()
                ViewState("string_PrevSSNo") = l_DataTable_SelectedOption.Rows(0)("BeneficiaryTaxNumber").ToString().Trim()
                l_string_PrevSSNo = ViewState("string_PrevSSNo")
                'End 2010.08.11 : YRS 5.0-1147
                TextBoxGeneralSSNo.Text = l_string_SSNo

                Dim l_decimal_AnnuityM, l_decimal_AnnuityC As Decimal
                Try
                    l_decimal_AnnuityM = Decimal.Parse(l_DataTable_SelectedOption.Rows(0)("AnnuityM").ToString())
                Catch ex As Exception
                    l_decimal_AnnuityM = 0
                End Try
                Try
                    l_decimal_AnnuityC = Decimal.Parse(l_DataTable_SelectedOption.Rows(0)("AnnuityC").ToString())
                Catch ex As Exception
                    l_decimal_AnnuityC = 0
                End Try
                If (l_decimal_AnnuityM > 0) Then
                    divRetirementPlanAnnuityoption.Visible = True
                    RadioButtonAnnuityType_RetirementPlan.Visible = True
                    LabelAnnuityOption_RetirementPlan.Visible = True
                    RadioButtonAnnuityType_RetirementPlan.Items(0).Selected = True
                    RadioButtonAnnuityType_RetirementPlan.Enabled = False
                ElseIf (l_decimal_AnnuityC > 0) Then
                    divRetirementPlanAnnuityoption.Visible = True
                    RadioButtonAnnuityType_RetirementPlan.Visible = True
                    LabelAnnuityOption_RetirementPlan.Visible = True
                    RadioButtonAnnuityType_RetirementPlan.Items(1).Selected = True
                    RadioButtonAnnuityType_RetirementPlan.Enabled = False
                End If
                If (l_decimal_AnnuityM > 0 AndAlso l_decimal_AnnuityC > 0) Then
                    RadioButtonAnnuityType_RetirementPlan.Enabled = True
                    RadioButtonAnnuityType_RetirementPlan.Items(0).Selected = True
                End If
            End If
            'Perform calculations for the Savings Plan
            divSavingPlanAnnuityoption.Visible = False           'SR:2014.07.09 - BT 2593 - UI changes in Beneficiary information page
            RadioButtonAnnuityType_SavingsPlan.Visible = False
            LabelAnnuityOption_SavingsPlan.Visible = False
            If HelperFunctions.isNonEmpty(l_DataSet_SelectedOption_SavingsPlan) Then
                l_DataTable_SelectedOption_SavingsPlan = l_DataSet_SelectedOption_SavingsPlan.Tables("r_BenefitInformation")

                'MultiPageRetireesInformation.SelectedIndex = 0
                'TabStripRetireesInformation.SelectedIndex = 0
                'If (Session("TextBoxGeneralSSNo") Is Nothing) Then
                If (l_string_SSNo Is Nothing) Then
                    ViewState("TextBoxGeneralSSNo") = l_DataTable_SelectedOption_SavingsPlan.Rows(0)("BeneficiaryTaxNumber").ToString().Trim()
                    l_string_SSNo = ViewState("TextBoxGeneralSSNo")
                Else
                    l_string_SSNo = ViewState("TextBoxGeneralSSNo") 'Session("TextBoxGeneralSSNo")
                End If
                'Priya 2010.08.11 : Added veiewstate for persistence mechanism for YRS 5.0-1147 
                'l_string_PrevSSNo = l_string_PrevSSNo = l_DataTable_SelectedOption_SavingsPlan.Rows(0)("BeneficiaryTaxNumber").ToString().Trim()
                'ViewState("string_PrevSSNo") = l_string_PrevSSNo = l_DataTable_SelectedOption_SavingsPlan.Rows(0)("BeneficiaryTaxNumber").ToString().Trim()'/SP:2013.04.12 :YRS 5.0-1990 -Commented becuase view state assignment store boolean vlaue(chaining doedt support in vb.net)
                ViewState("string_PrevSSNo") = l_DataTable_SelectedOption_SavingsPlan.Rows(0)("BeneficiaryTaxNumber").ToString().Trim() 'SP:2013.04.12 :YRS 5.0-1990 -Added
                l_string_PrevSSNo = ViewState("string_PrevSSNo")
                'End 2010.08.11 : YRS 5.0-1147
                TextBoxGeneralSSNo.Text = l_string_SSNo

                Dim l_decimal_AnnuityM, l_decimal_AnnuityC As Decimal
                Try
                    l_decimal_AnnuityM = Decimal.Parse(l_DataTable_SelectedOption_SavingsPlan.Rows(0)("AnnuityM").ToString())
                Catch ex As Exception
                    l_decimal_AnnuityM = 0
                End Try
                Try
                    l_decimal_AnnuityC = Decimal.Parse(l_DataTable_SelectedOption_SavingsPlan.Rows(0)("AnnuityC").ToString())
                Catch ex As Exception
                    l_decimal_AnnuityC = 0
                End Try
                If (l_decimal_AnnuityM > 0) Then
                    LabelAnnuityOption_SavingsPlan.Visible = True
                    RadioButtonAnnuityType_SavingsPlan.Visible = True
                    divSavingPlanAnnuityoption.Visible = True    'SR:2014.07.09 - BT 2593 - UI changes in Beneficiary information page
                    RadioButtonAnnuityType_SavingsPlan.Items(0).Selected = True
                    RadioButtonAnnuityType_SavingsPlan.Enabled = False
                ElseIf (l_decimal_AnnuityC > 0) Then
                    divSavingPlanAnnuityoption.Visible = True  'SR:2014.07.09 - BT 2593 - UI changes in Beneficiary information page
                    LabelAnnuityOption_SavingsPlan.Visible = True
                    RadioButtonAnnuityType_SavingsPlan.Visible = True
                    RadioButtonAnnuityType_SavingsPlan.Items(1).Selected = True
                    RadioButtonAnnuityType_SavingsPlan.Enabled = False
                End If
                If (l_decimal_AnnuityM > 0 OrElse l_decimal_AnnuityC > 0) Then
                    RadioButtonAnnuityType_SavingsPlan.Enabled = True
                    RadioButtonAnnuityType_SavingsPlan.Items(0).Selected = True
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Lookup_PersonalDetails(ByVal l_string_SSNo As String)

        Dim l_DataSet_BIPersonalDetails As DataSet
        Dim dsAddress As DataSet
        Dim l_DataSet_NewBeneficiary As DataSet
        Dim drAddress As DataRow()
        Try
            l_DataSet_BIPersonalDetails = DirectCast(Session("BeneficiaryPersonalDetails"), DataSet)  'Changed from CType to Directcast by SR:2010.06.21 for migration
            If (Session("BeneficiaryPersonalDetails") Is Nothing) Then
                l_DataSet_BIPersonalDetails = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_BeneficiaryPersonalDetails(l_string_SSNo)
            End If
            If (l_DataSet_BIPersonalDetails.Tables("r_BeneficiaryPersonalDetails").Rows.Count > 0) Then
                With l_DataSet_BIPersonalDetails.Tables("r_BeneficiaryPersonalDetails")
                    TextBoxGeneralFirstName.Text = .Rows(0)("First Name").ToString().Trim()
                    TextBoxGeneralMiddleName.Text = .Rows(0)("Middle Name").ToString().Trim()
                    TextBoxGeneralLastName.Text = .Rows(0)("Last Name").ToString().Trim()
                    TextBoxGeneralSuffix.Text = .Rows(0)("Suffix").ToString().Trim()
                    TextBoxGeneralFundNo.Text = .Rows(0)("Fund No").ToString().Trim()
                    TextBoxGeneralDOB.Text = .Rows(0)("Birth Date").ToString().Trim()
                    If (.Rows(0)("Salute").ToString().Trim <> "" And .Rows(0)("Salute").ToString().Length > 1) Then
                        cboSalute.SelectedValue = .Rows(0)("Salute").ToString().Trim()
                    End If
                    'START : Commented and added by Dilip Yadav : YRS 5.0.852 : 2009.09.09
                    '
                    'Select Case .Rows(0)("Gender").ToString().Trim()
                    '    Case "U"
                    '        DropDownListGeneralGender.SelectedIndex = 0
                    '    Case "M"
                    '        DropDownListGeneralGender.SelectedIndex = 1
                    '    Case "F"
                    '        DropDownListGeneralGender.SelectedIndex = 2
                    'End Select
                    'If (.Rows(0)("Gender").ToString().Trim() <> "") Then
                    '    'Commented & Added by Dilip yadav : 2009.09.10
                    '    'DropDownListGeneralGender.SelectedValue = .Rows(0)("Gender").ToString().Trim()
                    '    Dim strGenderType As String
                    '    strGenderType = .Rows(0)("Gender").ToString.Trim()
                    '    If (DropDownListGeneralGender.Items.FindByValue(strGenderType) Is Nothing) Then
                    '        DropDownListGeneralGender.Items.Insert(0, strGenderType)
                    '    End If
                    '    DropDownListGeneralGender.SelectedValue = strGenderType
                    'End If

                    'BS:2011.07.19:BT-852:YRS(5.0 - 1339)- assign X value when Gender is non human/non actuary
                    Dim strGenderType As String = "U"
                    If (.Rows(0)("Gender").ToString().Trim() <> "" And .Rows(0)("Gender").ToString().Trim() <> "System.DBNull") Then
                        strGenderType = .Rows(0)("Gender").ToString.Trim()
                    End If
                    If (Me.DropDownListGeneralGender.Items.FindByValue(strGenderType) Is Nothing) Then
                        Me.DropDownListGeneralGender.Items.Insert(0, strGenderType)
                    End If
                    Me.DropDownListGeneralGender.SelectedValue = strGenderType

                    'If (.Rows(0)("Gender").ToString().Trim() <> "" And .Rows(0)("Gender").ToString().Trim() <> "System.DBNull") Then
                    '    Dim strGenderType As String
                    '    strGenderType = .Rows(0)("Gender").ToString.Trim()
                    '    If (DropDownListGeneralGender.Items.FindByValue(strGenderType) Is Nothing) Then
                    '        DropDownListGeneralGender.Items.Insert(0, strGenderType)
                    '    End If
                    '    DropDownListGeneralGender.SelectedValue = strGenderType
                    'Else
                    '    Dim strGenderType As String
                    '    Try
                    '        strGenderType = "U"
                    '        If (DropDownListGeneralGender.Items.FindByValue(strGenderType) Is Nothing) Then
                    '            DropDownListGeneralGender.Items.Insert(0, strGenderType)
                    '        End If
                    '        DropDownListGeneralGender.SelectedValue = strGenderType
                    '    Catch ex As Exception
                    '        Throw 'DropDownListGeneralGender.SelectedValue = "U"
                    '    End Try
                    'End If

                    'End YRS(5.0 - 1339)

                    'END : Commented and added by Dilip Yadav : YRS 5.0.852 : 2009.09.09
                    'If (.Rows(0)("Marital Status").ToString().Trim() <> "") Then
                    '    cboGeneralMaritalStatus.SelectedValue = .Rows(0)("Marital Status").ToString().Trim()
                    'End If
                    'Commented & Added by Dilip yadav : 2009.09.10
                    'DropDownListGeneralGender.SelectedValue = .Rows(0)("Gender").ToString().Trim()
                    'BS:2011.07.19:BT-852:YRS(5.0 - 1339) - assign X value when MaritalStatus is non human/non actuary
                    Dim strMaritalStatus As String = "U"
                    If (.Rows(0)("Marital Status").ToString().Trim() <> "" And .Rows(0)("Marital Status").ToString().Trim() <> "System.DBNull") Then
                        strMaritalStatus = .Rows(0)("Marital Status").ToString.Trim()
                    End If
                    If (Me.cboGeneralMaritalStatus.Items.FindByValue(strMaritalStatus) Is Nothing) Then
                        Me.cboGeneralMaritalStatus.Items.Insert(0, strMaritalStatus)
                    End If
                    Me.cboGeneralMaritalStatus.SelectedValue = strMaritalStatus
                    'If (.Rows(0)("Marital Status").ToString().Trim() <> "" And .Rows(0)("Marital Status").ToString().Trim() <> "System.DBNull") Then
                    '    Dim strMaritalStatus As String
                    '    strMaritalStatus = .Rows(0)("Marital Status").ToString.Trim()
                    '    If (cboGeneralMaritalStatus.Items.FindByValue(strMaritalStatus) Is Nothing) Then
                    '        cboGeneralMaritalStatus.Items.Insert(0, strMaritalStatus)
                    '    End If
                    '    cboGeneralMaritalStatus.SelectedValue = strMaritalStatus
                    'Else
                    '    Dim strMaritalStatus As String
                    '    Try
                    '        strMaritalStatus = "U"
                    '        If (cboGeneralMaritalStatus.Items.FindByValue(strMaritalStatus) Is Nothing) Then
                    '            cboGeneralMaritalStatus.Items.Insert(0, strMaritalStatus)
                    '        End If
                    '        cboGeneralMaritalStatus.SelectedValue = strMaritalStatus
                    '    Catch ex As Exception
                    '        Throw 'cboGeneralMaritalStatus.SelectedValue = "U"
                    '    End Try
                    'End If

                    'End YRS(5.0 - 1339)

                    'If (GetUniqueID <> "AddMode") Then
                    '    GetUniqueID = l_DataSet_BIPersonalDetails.Tables("r_BeneficiaryPersonalDetails").Rows(0)("guiUniqueID").ToString()
                    'End If
                    Session("PersId") = .Rows(0)("guiUniqueID").ToString().Trim()
                    stwListUserControl.PersonID = Session("PersId") ' PK | 2019.10.30 | YRS-AT-4605 | Assign PersonId to State Withholding User Control
                    'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 


                    'AddressWebUserControl1.IsPrimary = 1
                    'AddressWebUserControl2.IsPrimary = 0

                    dsAddress = Address.GetAddressByEntity(Session("PersId"), EnumEntityCode.PERSON)
                    If HelperFunctions.isNonEmpty(dsAddress) Then
                        AddressWebUserControl1.LoadAddressDetail(dsAddress.Tables(0).Select("isPrimary = True"))
                        AddressWebUserControl2.LoadAddressDetail(dsAddress.Tables(0).Select("isPrimary = False"))
                        'START: PK | 2019.10.30 | YRS-AT-4605 | Assign value to property
                        stwListUserControl.PersonStateName = dsAddress.Tables(0).Rows(0)("StateName").ToString().Trim()
                        stwListUserControl.PersonStateCode = dsAddress.Tables(0).Rows(0)("state").ToString().Trim()
                        'END: PK | 2019.10.30 | YRS-AT-4605 | Assign value to property
                    Else
                        AddressWebUserControl1.LoadAddressDetail(Nothing)
                        AddressWebUserControl2.LoadAddressDetail(Nothing)
                    End If
                    'START: PK | 2019.10.30 | YRS-AT-4605 | Assign value to property
                    stwListUserControl.PersonName = TextBoxGeneralFirstName.Text + " " + TextBoxGeneralLastName.Text
                    stwListUserControl.PersonFundNo = TextBoxGeneralFundNo.Text
                    'END: PK | 2019.10.30 | YRS-AT-4605 | Assign value to property

                End With
            Else    '  If (l_DataSet_BIPersonalDetails.Tables("r_BeneficiaryPersonalDetails").Rows.Count > 0) Then
                'The beneficiary does not exist as a participant in the system. We need to add him/her as a participant
                GetUniqueID = "AddMode"
                If (Session("PersId") Is Nothing) OrElse (DirectCast(Session("PersId"), String).Trim() = "") Then  'Changed from CType to Directcast by SR:2010.06.21 for migration
                    Session("PersId") = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_GetUniqueID()
                    stwListUserControl.PersonID = Session("PersId") ' PK | 2019.10.30 | YRS-AT-4605 | Assign PersonId to State Withholding User Control
                End If



                'Populate the new beneficiary from the Retirement plan or the savings plan option id -NP:PS:2007.08.20
                If Not l_string_BenefitOptionID Is Nothing AndAlso Not l_string_BenefitOptionID = String.Empty Then
                    l_DataSet_NewBeneficiary = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_NewBeneficiary(l_string_BenefitOptionID)
                ElseIf Not l_string_BenefitOptionID_SavingsPlan Is Nothing AndAlso Not l_string_BenefitOptionID_SavingsPlan = String.Empty Then
                    l_DataSet_NewBeneficiary = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_NewBeneficiary(l_string_BenefitOptionID_SavingsPlan)
                End If

                If (HelperFunctions.isEmpty(l_DataSet_NewBeneficiary)) Then
                    'Beneficiary Information was not obtained from the database.
                    CloseFormStatus = showMessage(PlaceHolder1, "Error", "Unable to Locate a Beneficary Record for Benefit option ", MessageBoxButtons.Stop)    'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                    Exit Sub
                End If

                'Obtained Beneficiary Information from the Database which will be used to create the new Participant
                TextBoxGeneralFirstName.Text = l_DataSet_NewBeneficiary.Tables(0).Rows(0)("BeneficiaryName1").ToString().Trim()
                TextBoxGeneralLastName.Text = l_DataSet_NewBeneficiary.Tables(0).Rows(0)("BeneficiaryName2").ToString().Trim()
                TextBoxGeneralSSNo.Text = l_DataSet_NewBeneficiary.Tables(0).Rows(0)("BeneficiaryTaxNumber").ToString().Trim()
                TextBoxGeneralDOB.Text = l_DataSet_NewBeneficiary.Tables(0).Rows(0)("BirthDate").ToString().Trim()
                'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses
                'START: PK | 2019.10.30 | YRS-AT-4605 | Assign value to property
                stwListUserControl.PersonName = TextBoxGeneralFirstName.Text + " " + TextBoxGeneralLastName.Text
                stwListUserControl.PersonFundNo = ""
                'END: PK | 2019.10.30 | YRS-AT-4605 | Assign value to property
                'dsAddress = Address.GetAddressByEntity(l_DataSet_NewBeneficiary.Tables(0).Rows(0)("UniqueID").ToString().Trim(), EnumEntityCode.PERSON)
                dsAddress = Address.GetBeneficiariesAddress(Session("ParticipantEntityId"), TextBoxGeneralSSNo.Text, TextBoxGeneralFirstName.Text, TextBoxGeneralLastName.Text)
                If HelperFunctions.isNonEmpty(dsAddress) Then
                    'Start: AA:26.05.2014- BT:2541 - YRS 5.0-2372-Added not to update beneficiary address to bit active and bitprimary to 1
                    drAddress = dsAddress.Tables(0).Select("isPrimary = True")
                    If drAddress IsNot Nothing AndAlso drAddress.Length > 0 Then
                        drAddress(0)("UniqueId") = ""
                    End If
                    'End: AA:26.05.2014- BT:2541 - YRS 5.0-2372-Added not to update beneficiary address to bit active and bitprimary to 1
                    AddressWebUserControl1.LoadAddressDetail(drAddress)
                    'START: PK | 2019.10.30 | YRS-AT-4605 | Assign value to property
                    stwListUserControl.PersonStateName = dsAddress.Tables(0).Rows(0)("StateName").ToString().Trim()
                    stwListUserControl.PersonStateCode = dsAddress.Tables(0).Rows(0)("state").ToString().Trim()
                    'END: PK | 2019.10.30 | YRS-AT-4605 | Assign value to property
                Else
                    AddressWebUserControl1.LoadAddressDetail(Nothing)
                End If
                AddressWebUserControl2.LoadAddressDetail(Nothing)

            End If
            If TextBoxGeneralSSNo.Text.Trim() = "" Then
                TextBoxGeneralSSNo.Text = "0"
            End If
            'Store Beneficiarydetails
            Session("BeneficiaryPersonalDetails") = l_DataSet_BIPersonalDetails
        Catch ex As Exception
            Throw
        End Try
    End Sub

    'This function registers client side javascript to close the form and call a submit on the calling page.
    Private Sub CloseForm()
        Dim l_string_clientScript As String = "<script language='javascript'>" & _
         "window.opener.document.forms(0).submit();" & _
          "self.close()" & _
            "</script>" '  "window.opener.location.href=window.opener.location.href;" & _

        If (Not IsClientScriptBlockRegistered("clientScript")) Then
            RegisterClientScriptBlock("clientScript", l_string_clientScript)
        End If
    End Sub

    Private Sub LookupDeathBenefitOption()
        ''LookUp_BI_DeathBeneficiaryOption
        Dim l_DataSet_deatBenefitOption As DataSet
        Dim l_DataTable_deatBenefitOption As DataTable
        Dim l_string_Option As String

        Dim l_DataSet_deatBenefitOption_SavingsPlan As DataSet
        Dim l_DataTable_deatBenefitOption_SavingsPlan As DataTable
        Dim l_string_Option_SavingsPlan As String

        Dim l_DataSet_AnnuityCount As DataSet
        Dim l_DataSet_AnnuityCount_SavingsPlan As DataSet

        Dim l_DataSet_POA As DataSet
        Dim l_DataSet_Email As DataSet
        Dim l_DataSet_PhoneNumber As DataSet
        Dim l_DataSet_Address As DataSet
        Dim l_DataSet_FedWithHold As DataSet

        Try
            If l_string_BenefitOptionID <> "" Then l_DataSet_deatBenefitOption = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_DeathBeneficiaryOption(l_string_BenefitOptionID)
            If l_string_BenefitOptionID_SavingsPlan <> "" Then l_DataSet_deatBenefitOption_SavingsPlan = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_DeathBeneficiaryOption(l_string_BenefitOptionID_SavingsPlan)

            'Check if both options are undefined and raise error and exit sub
            If HelperFunctions.isEmpty(l_DataSet_deatBenefitOption) AndAlso HelperFunctions.isEmpty(l_DataSet_deatBenefitOption_SavingsPlan) Then
                showMessage(PlaceHolder1, "Error", "Unable to load Benefit Information for the selected benefit option.", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                Session("Success_BIScreen") = False
                CloseForm()
                Exit Sub
            End If

            'General Initialization
            TabStripRetireesInformation.Items(1).Enabled = False
            DataGridGeneralWithhold.DataSource = Nothing
            DataGridGeneralWithhold.DataBind()
            'Priya 2010.08.11 : Added value to viewstate to add persistence mechanism for YRS 5.0-1147
            TaxWithHoldRequired = False 'SR:2010.09.25 variable set instead of viewstate for withholding issue
            'ViewState("TaxWithHoldRequired") = False
            'End 2010.08.11 

            'Retirement Plan specific control initialization

            divRetirementPlanAnnuityoption.Visible = False  'SR:2014.07.09 - BT 2593 - UI changes in Beneficiary information page
            RadioButtonAnnuityType_RetirementPlan.Visible = False
            LabelAnnuityOption_RetirementPlan.Visible = False
            If (Not l_DataSet_deatBenefitOption Is Nothing AndAlso HelperFunctions.isNonEmpty(l_DataSet_deatBenefitOption.Tables("r_BI_DeathBeneficiaryOption"))) Then
                l_DataTable_deatBenefitOption = l_DataSet_deatBenefitOption.Tables("r_BI_DeathBeneficiaryOption")
                If (Not l_DataTable_deatBenefitOption.Rows(0)("AnnuityM") Is Nothing And Not l_DataTable_deatBenefitOption.Rows(0)("AnnuityC") Is Nothing And (Decimal.Parse(l_DataTable_deatBenefitOption.Rows(0)("AnnuityM").ToString()) > 0 Or Decimal.Parse(l_DataTable_deatBenefitOption.Rows(0)("AnnuityM").ToString()) > 0)) Then

                    divRetirementPlanAnnuityoption.Visible = True  'SR:2014.07.09 - BT 2593 - UI changes in Beneficiary information page
                    RadioButtonAnnuityType_RetirementPlan.Visible = True
                    LabelAnnuityOption_RetirementPlan.Visible = True
                End If

                If (l_DataTable_deatBenefitOption.Rows(0)("COption") Is Nothing) Then
                    l_string_Option = l_DataTable_deatBenefitOption.Rows(0)("POption").ToString().ToUpper().Trim()
                Else
                    l_string_Option = l_DataTable_deatBenefitOption.Rows(0)("COption").ToString().ToUpper().Trim() + l_DataTable_deatBenefitOption.Rows(0)("POption").ToString().ToUpper().Trim()
                End If

                l_DataSet_AnnuityCount = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_AnnuityCount(l_string_Option)
                If (Not l_DataSet_AnnuityCount Is Nothing AndAlso HelperFunctions.isNonEmpty(l_DataSet_AnnuityCount.Tables("r_AnnuityCount"))) Then
                    If (Int32.Parse(l_DataSet_AnnuityCount.Tables("r_AnnuityCount").Rows(0)("CNT").ToString()) > 0) Then
                        TabStripRetireesInformation.Items(1).Enabled = True

                        'Priya 2010.08.11 : Added value to viewstate to add persistence mechanism for YRS 5.0-1147
                        TaxWithHoldRequired = True 'SR:2010.09.25 variable set instead of viewstate for withholding issue
                        'ViewState("TaxWithHoldRequired") = True
                        'End 2010.08.11
                    End If
                End If
            End If
            'Savings Plan specific control initialization
            RadioButtonAnnuityType_SavingsPlan.Visible = False
            LabelAnnuityOption_SavingsPlan.Visible = False
            divSavingPlanAnnuityoption.Visible = False  'SR:2014.07.09 - BT 2593 - UI changes in Beneficiary information page

            If (Not l_DataSet_deatBenefitOption_SavingsPlan Is Nothing AndAlso HelperFunctions.isNonEmpty(l_DataSet_deatBenefitOption_SavingsPlan.Tables("r_BI_DeathBeneficiaryOption"))) Then
                l_DataTable_deatBenefitOption_SavingsPlan = l_DataSet_deatBenefitOption_SavingsPlan.Tables("r_BI_DeathBeneficiaryOption")
                If (Not l_DataTable_deatBenefitOption_SavingsPlan.Rows(0)("AnnuityM") Is Nothing And Not l_DataTable_deatBenefitOption_SavingsPlan.Rows(0)("AnnuityC") Is Nothing And (Decimal.Parse(l_DataTable_deatBenefitOption_SavingsPlan.Rows(0)("AnnuityM").ToString()) > 0 Or Decimal.Parse(l_DataTable_deatBenefitOption_SavingsPlan.Rows(0)("AnnuityM").ToString()) > 0)) Then
                    RadioButtonAnnuityType_SavingsPlan.Visible = True
                    LabelAnnuityOption_SavingsPlan.Visible = True
                    divSavingPlanAnnuityoption.Visible = True  'SR:2014.07.09 - BT 2593 - UI changes in Beneficiary information page
                End If

                If (l_DataTable_deatBenefitOption_SavingsPlan.Rows(0)("COption") Is Nothing) Then
                    l_string_Option_SavingsPlan = l_DataTable_deatBenefitOption_SavingsPlan.Rows(0)("POption").ToString().ToUpper().Trim()
                Else
                    l_string_Option_SavingsPlan = l_DataTable_deatBenefitOption_SavingsPlan.Rows(0)("COption").ToString().ToUpper().Trim() + l_DataTable_deatBenefitOption_SavingsPlan.Rows(0)("POption").ToString().ToUpper().Trim()
                End If

                l_DataSet_AnnuityCount_SavingsPlan = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_AnnuityCount(l_string_Option_SavingsPlan)
                If (Not l_DataSet_AnnuityCount_SavingsPlan Is Nothing AndAlso HelperFunctions.isNonEmpty(l_DataSet_AnnuityCount_SavingsPlan.Tables("r_AnnuityCount"))) Then
                    If (Int32.Parse(l_DataSet_AnnuityCount_SavingsPlan.Tables("r_AnnuityCount").Rows(0)("CNT").ToString()) > 0) Then
                        TabStripRetireesInformation.Items(1).Enabled = True
                        'Priya 2010.08.11 : Added value to viewstate to add persistence mechanism for YRS 5.0-1147
                        TaxWithHoldRequired = True 'SR:2010.09.25 variable set instead of viewstate for withholding issue
                        'ViewState("TaxWithHoldRequired") = True
                        'End 2010.08.11
                    End If
                End If
            End If

            If (GetUniqueID <> "AddMode") Then
                '' GetUniqueID = l_String_guiUniqueID.ToUpper().Trim()
                'BT 706 2011.07.22 ,bhavna In this dataset fillby Session("POADetailsStore") if first time entry for Beneficiary
                l_DataSet_POA = Session("POADetailsStore")
                If l_DataSet_POA Is Nothing Then
                    l_DataSet_POA = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpPOAInfo(Session("PersId"))
                    If HelperFunctions.isEmpty(l_DataSet_POA) Then
                        Session("POADetailsStore") = l_DataSet_POA
                    End If

                End If
                LoadPOADetails(l_DataSet_POA)
                'BT 706 2011.07.22 ,bhavna
                l_DataSet_Address = Session("AddressDetailsStore")

                Session("AddressDetailsStore") = l_DataSet_Address

                If (TaxWithHoldRequired = True) Then
                    LoadFedWithDrawalTab()
                    'START: PK |10.04.2019| YRS-AT-4605 |Enabling/Disabling control based on option selected by user.
                    stwListUserControl.Visible = True
                    stwListUserControl.STWDataSaveAtMainPage = True
                    'END: PK |10.04.2019| YRS-AT-4605 |Enabling/Disabling control based on option selected by user.
                End If
                l_DataSet_PhoneNumber = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_PersonalDetailsAll(Session("PersId"), "TEL")
                If HelperFunctions.isNonEmpty(l_DataSet_PhoneNumber) Then
                    Dim j As Integer
                    For j = 0 To l_DataSet_PhoneNumber.Tables(0).Rows.Count - 1 Step 1
                        If (l_DataSet_PhoneNumber.Tables(0).Rows(j)("bitPrimary").ToString().ToLower() = "true") Then
                            TextBoxTelephone.Text = l_DataSet_PhoneNumber.Tables(0).Rows(j)("chvPhoneNumber").ToString().Trim()
                            'YRS 5.0-424 : Priya
                            Session("TelephoneNo") = TextBoxTelephone.Text
                        Else
                            TextBoxSecTelephone.Text = l_DataSet_PhoneNumber.Tables(0).Rows(j)("chvPhoneNumber").ToString().Trim()
                            'YRS 5.0-424 : Priya
                            Session("SecTelephoneNo") = TextBoxSecTelephone.Text
                        End If
                    Next
                End If

                l_DataSet_Email = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_PersonalDetailsAll(Session("PersId"), "EMAIL")
                If HelperFunctions.isNonEmpty(l_DataSet_Email) Then
                    Dim k As Integer
                    For k = 0 To l_DataSet_Email.Tables(0).Rows.Count - 1 Step 1
                        If (l_DataSet_Email.Tables(0).Rows(k)("bitPrimary").ToString().Trim().ToLower() = "true") Then
                            TextBoxEmailId.Text = l_DataSet_Email.Tables(0).Rows(k)("Email").ToString().Trim()
                        Else
                            TextBoxSecEmail.Text = l_DataSet_Email.Tables(0).Rows(k)("Email").ToString().Trim()
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Function ValidateSettlementPrerequisites() As Boolean
        Dim l_DataSet_Validate As New DataSet
        Dim l_string_PrereqErrMsg As String
        Try
            l_DataSet_Validate = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_PersonalDetailsAll(Session("PersId"), "VALIDATE")
            If (l_DataSet_Validate.Tables(0).Rows.Count <> 1) Then
                CloseFormStatus = showMessage(PlaceHolder1, "Data Error Encountered", "invalid settlement prereq data encountered in valsettlprereq function of benedetail form", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                Return False
            End If
            If (l_DataSet_Validate.Tables(0).Rows.Count > 0) Then
                If (Decimal.Parse(l_DataSet_Validate.Tables(0).Rows(0)("PersCnt").ToString()) < 1 Or Decimal.Parse(l_DataSet_Validate.Tables(0).Rows(0)("FundEventCnt").ToString()) < 1 Or Decimal.Parse(l_DataSet_Validate.Tables(0).Rows(0)("PrimaryAddrCnt").ToString()) < 1 Or (TaxWithHoldRequired = True And Decimal.Parse(l_DataSet_Validate.Tables(0).Rows(0)("FedTaxWhhCnt").ToString()) < 1)) Then
                    If (Decimal.Parse(l_DataSet_Validate.Tables(0).Rows(0)("PersCnt").ToString()) < 1) Then
                        showMessage(PlaceHolder1, "Error", "Person data does not exist for this beneficiary", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                        CloseFormStatus = False
                        Return False
                    End If
                    If (Decimal.Parse(l_DataSet_Validate.Tables(0).Rows(0)("FundEventCnt").ToString()) < 1) Then
                        showMessage(PlaceHolder1, "Error", "Fundevent data does not exist for this beneficiary", MessageBoxButtons.Stop)    'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                        CloseFormStatus = False
                        Return False
                    End If
                    If (Decimal.Parse(l_DataSet_Validate.Tables(0).Rows(0)("PrimaryAddrCnt").ToString()) < 1) Then
                        CloseFormStatus = showMessage(PlaceHolder1, "Error", "Primary address data does not exist for this beneficiary", MessageBoxButtons.Stop)    'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                        Return False
                    End If
                    If ((TaxWithHoldRequired = True And Decimal.Parse(l_DataSet_Validate.Tables(0).Rows(0)("FedTaxWhhCnt").ToString()) < 1)) Then
                        CloseFormStatus = showMessage(PlaceHolder1, "Error", "Federal tax withholding data does not exist for this beneficiary", MessageBoxButtons.Stop)    'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                        Return False
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function SaveDetails() As Boolean
        'Save all information into the Database from here 
        Dim l_DataSet_MemberDetails As DataSet
        Dim l_DataTable_MemberDetails As DataTable




        Dim l_DataSet_MemberTelehone As DataSet
        Dim l_DataTable_MemberTelehone As DataTable

        Dim l_DataSet_MemberEmail As DataSet
        Dim l_DataTable_MemberEmail As DataTable

        Dim dsFedWithDrawals As DataSet

        Dim l_DataSet_Adress
        Dim bool_Status As Boolean
        Dim l_string_BeneFundEventID As String

        Dim l_string_BenePersUniqueID As String
        'If (GetUniqueID = "AddMode") Then
        '    l_string_BenePersUniqueID = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_GetUniqueID()
        'Else
        'Load the existing beneficiary's PersId from Session or Create a new one from the Database
        'START: PK |12/04/2019|YRS-AT-4605| Declaring variable
        Dim isStateWithholdingDataSave As Boolean
        Dim LstSWHPerssDetail As List(Of YMCAObjects.StateWithholdingDetails)
        'END: PK |12/04/2019|YRS-AT-4605| Declaring variable
        Try

            'Creating atsPerss record for Beneficary

            l_string_BenePersUniqueID = Session("PersId")
            If (Session("PersId") Is Nothing) Then
                l_string_BenePersUniqueID = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_GetUniqueID()
            End If

            Dim l_string_OrigFundEventID As String
            'NP:PS:2007.08.28
            If l_string_BenefitOptionID <> "" Then
                l_string_OrigFundEventID = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_FunEventsUniqueID(l_string_BenefitOptionID)
            ElseIf l_string_BenefitOptionID_SavingsPlan <> "" Then
                l_string_OrigFundEventID = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_FunEventsUniqueID(l_string_BenefitOptionID_SavingsPlan)
            End If

            If (l_string_BenePersUniqueID <> "") Then
                l_DataSet_MemberDetails = YMCARET.YmcaBusinessObject.BeneficiarySettlement.CreateDataSetForSave("MEMBER")
                l_DataTable_MemberDetails = l_DataSet_MemberDetails.Tables("Member Details")
                'Create a new row in the MemberDetails dataset which contains information from AtsPerss table
                Dim newrow As DataRow
                newrow = l_DataTable_MemberDetails.NewRow()
                newrow("guiUniqueID") = l_string_BenePersUniqueID
                newrow("chrSSNo") = TextBoxGeneralSSNo.Text.Trim()
                newrow("chvLastName") = Left(TextBoxGeneralLastName.Text.Trim(), 30)
                newrow("chvFirstName") = Left(TextBoxGeneralFirstName.Text.Trim(), 20)
                newrow("chvMiddleName") = Left(TextBoxGeneralMiddleName.Text.Trim(), 20)
                newrow("chvSalutationCode") = cboSalute.SelectedItem.Text
                newrow("chvSuffixTitle") = TextBoxGeneralSuffix.Text
                newrow("chvgenderCode") = DropDownListGeneralGender.SelectedItem.Value
                If (TextBoxGeneralDOB.Text.Trim() = "") Then
                    newrow("dtmBirthDate") = DBNull.Value
                Else
                    newrow("dtmBirthDate") = DateTime.Parse(TextBoxGeneralDOB.Text)
                End If
                newrow("dtmDeathDate") = DBNull.Value
                newrow("chvMaritalStatusCode") = cboGeneralMaritalStatus.SelectedItem.Value

                l_DataTable_MemberDetails.Rows.Add(newrow)

                'If we are in AddMode then the beneficiary is not a participant in the system, we need to create a new participant record
                '   else we can simply update the existing participant's record
                If (GetUniqueID = "AddMode") Then

                    ' NP - 2007.02.09 - YREN-3043: Handling the case of adding a default Estate Beneficiary when Annuity Type C selected for a new Participant.
                    ' Insert into atsBeneficiaries if annuity option C
                    If RadioButtonAnnuityType_RetirementPlan.SelectedValue = "C" OrElse RadioButtonAnnuityType_SavingsPlan.SelectedValue = "C" Then
                        Dim l_string_PersId As String = l_string_BenePersUniqueID 'New Beneficiary's GUID
                        Dim Beneficiaries As New DataSet
                        Dim drUpdated As DataRow
                        Beneficiaries = YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.LookUpRetiredBeneficiariesInfo(l_string_PersId)

                        drUpdated = Beneficiaries.Tables(0).NewRow()
                        drUpdated("PersID") = l_string_PersId
                        'drUpdated("Name") = "Estate"
                        drUpdated("Name2") = "Estate" '- Removing this so that the Last Name is not taken
                        'drUpdated("TaxID") = "" - Removing this so that the SSN no is not submitted
                        drUpdated("Rel") = "ES"
                        'drUpdated("Birthdate") = "" '- Removing this so that the Birthdate is not taken as 1/1/1900 as the default
                        drUpdated("Groups") = "PRIM"
                        'drUpdated("Lvl") = "" - No need to set the Lvl of the benefit
                        drUpdated("Pct") = "100.0000"
                        'NP:PS:2007.07.12 - Updating Beneficiary Type code to RETIRE from INSRES since RETIRE type of beneficiaries also get Insured reserves if INSRES/INSSAV type of beneficiaries are not defined
                        'drUpdated("BeneficiaryTypeCode") = "INSRES" ' INSRES=Insured Annuity Reserves / RETIRE=Retiree Death Benefit
                        drUpdated("BeneficiaryTypeCode") = "RETIRE"
                        Beneficiaries.Tables(0).Rows.Add(drUpdated)
                        YMCARET.YmcaBusinessObject.ParticipantsInformationBOClass.InsertRetiredBeneficiaries(Beneficiaries)
                        Beneficiaries.AcceptChanges()
                    End If
                    'Insert into atsPerss
                    YMCARET.YmcaBusinessObject.BeneficiarySettlement.Insert_BeneficiaryMember(l_DataSet_MemberDetails)
                Else
                    'Since the beneficiary already exists as a participant, we will simply update his/her details provided on this page.
                    YMCARET.YmcaBusinessObject.BeneficiarySettlement.Update_BeneficiaryMember(l_DataSet_MemberDetails)

                End If
                l_DataSet_MemberDetails.AcceptChanges()

                'End of Creation / Updation  of atsPerss record for the beneficiary

                'NP:PS:2007.08.08 - We want to avoid insertions if we are not taking an annuity
                '   The Radiobuttons for selecting the annuity type are only visible when the beneficiary opts for an annuity from the respective plan. If both are hidden then only refund option has been elected.

                '-------------------------------------------------------------------------------------------------------
                'Shashi Shekhar singh:14-Apr.-2011: For YRS 5.0-877 : Changes to Banking Information maintenance.

                'If RadioButtonAnnuityType_RetirementPlan.Visible = False AndAlso RadioButtonAnnuityType_SavingsPlan.Visible = False Then
                If RadioButtonAnnuityType_RetirementPlan.Visible = True Or RadioButtonAnnuityType_SavingsPlan.Visible = True Then
                    'By Aparna Inserting Record for the New Member in Atsperssbanking table
                    '05/03/2007 YREN-3015
                    YMCARET.YmcaBusinessObject.BeneficiarySettlement.Insert_AtsperssBanking(Session("ParticipantEntityId"), l_string_BenePersUniqueID)
                    '05/03/2007 YREN-3015
                End If
                '-------------------------------------------------------------------------------------------------------


                ''Insert With Holding Details
                If (TaxWithHoldRequired = True) Then
                    'Vipul 03Feb06 Cache-Session
                    'dsFedWithDrawals = Cache("FedWithDrawals")
                    dsFedWithDrawals = Session("FedWithDrawals")
                    'Vipul 03Feb06 Cache-Session
                    If Not dsFedWithDrawals Is Nothing Then
                        YMCARET.YmcaBusinessObject.RetireesInformationBOClass.InsertRetireesFedWithdrawals(dsFedWithDrawals)
                        dsFedWithDrawals.AcceptChanges()

                        'Vipul 03Feb06 Cache-Session
                        'Cache.Add("FedWithDrawals", dsFedWithDrawals)
                        Session("FedWithDrawals") = dsFedWithDrawals
                        'Vipul 03Feb06 Cache-Session

                        Me.DataGridGeneralWithhold.DataSource = dsFedWithDrawals
                        ViewState("DS_Sort_BenInfo") = dsFedWithDrawals
                        Me.DataGridGeneralWithhold.DataBind()
                    End If
                End If
                'START : PK | 2019.11.25 | YRS-AT-4598 | Save State Withholding Data
                'If Statewithholding data save from main page flag is True, Statewithholding session is non empty and  bitactive is false (Means data get changed and not saved in DB)
                isStateWithholdingDataSave = False
                If (stwListUserControl.STWDataSaveAtMainPage) And (HelperFunctions.isNonEmpty(SessionManager.SessionStateWithholding.LstSWHPerssDetail.FirstOrDefault)) Then
                    LstSWHPerssDetail = SessionManager.SessionStateWithholding.LstSWHPerssDetail
                    If (LstSWHPerssDetail.FirstOrDefault.bitActive = False) Then
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Beneficiary Information", "Save State Withholding START")
                        isStateWithholdingDataSave = YMCARET.YmcaBusinessObject.StateWithholdingBO.SavePersStateTaxdetails(LstSWHPerssDetail.FirstOrDefault)
                        SessionManager.SessionStateWithholding.LstSWHPerssDetail = Nothing
                    End If
                End If
                'END : PK | 2019.11.25 | YRS-AT-4598 | Save State Withholding Data

                'Anudeep
                SaveAddressDetails(l_string_BenePersUniqueID)


                ''Insert/update Telephone Details
                Dim l_DataSet_LookupMemberTelehone As DataSet = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_MemberTelephone(l_string_BenePersUniqueID)
                'If (TextBoxTelephone.Text.Trim() <> "") Then
                Dim l_bool_PrimTelExists As Boolean = False
                l_DataSet_MemberTelehone = YMCARET.YmcaBusinessObject.BeneficiarySettlement.CreateDataSetForSave("TEL")
                l_DataTable_MemberTelehone = l_DataSet_MemberTelehone.Tables("Member Details")

                newrow = l_DataTable_MemberTelehone.NewRow()

                If (Not l_DataSet_LookupMemberTelehone Is Nothing) Then
                    If (Not l_DataSet_LookupMemberTelehone.Tables(0) Is Nothing) Then
                        If (l_DataSet_LookupMemberTelehone.Tables(0).Rows.Count = 0) Then
                            newrow("guiUniqueID") = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_GetUniqueID()
                            l_bool_PrimTelExists = False
                        Else
                            Dim l_DataView_Guid As DataView = l_DataSet_LookupMemberTelehone.Tables(0).DefaultView
                            l_DataView_Guid.RowFilter = "guiUniqueID <>'' and  bitPrimary=1 and bitActive=1"
                            'Priya
                            If (l_DataView_Guid.Count > 0) Then
                                If (Session("TelephoneNo") <> TextBoxTelephone.Text) Then
                                    If Session("TelephoneNo") <> "" Then


                                        newrow("guiUniqueID") = l_DataView_Guid(0)(0).ToString()
                                        l_bool_PrimTelExists = False

                                        newrow("guiEntityID") = l_string_BenePersUniqueID
                                        newrow("chvEntityCode") = "PERSON"
                                        newrow("dtmEffDate") = System.DateTime.Now.Date
                                        newrow("chvPhoneTypeCode") = "Home"
                                        newrow("chvPhoneNumber") = Session("TelephoneNo")
                                        newrow("bitActive") = 0
                                        newrow("bitPrimary") = 0

                                        l_DataSet_MemberTelehone.Tables("Member Details").Rows.Add(newrow)

                                        YMCARET.YmcaBusinessObject.BeneficiarySettlement.Update_MemberTelephone(l_DataSet_MemberTelehone)

                                        'l_DataSet_MemberTelehone.Tables("Member Details").AcceptChanges()
                                        l_DataSet_MemberTelehone.Tables("Member Details").Rows.Clear()
                                    Else
                                        l_bool_PrimTelExists = False
                                    End If

                                    newrow("guiUniqueID") = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_GetUniqueID()
                                    'ElseIf TextBoxTelephone.Text = "" Then

                                ElseIf (Session("TelephoneNo") = TextBoxTelephone.Text) Then
                                    newrow("guiUniqueID") = l_DataView_Guid(0)(0).ToString()
                                    l_bool_PrimTelExists = True
                                End If
                                'newrow("guiUniqueID") = l_DataView_Guid(0)(0).Tostring()
                                'l_bool_PrimTelExists = True
                                'Priya


                            Else
                                newrow("guiUniqueID") = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_GetUniqueID()
                                l_bool_PrimTelExists = False
                            End If
                        End If
                    End If
                    'End If
                    If (TextBoxTelephone.Text.Trim() <> "") Then
                        newrow("guiEntityID") = l_string_BenePersUniqueID
                        newrow("chvEntityCode") = "PERSON"
                        newrow("dtmEffDate") = System.DateTime.Now.Date
                        newrow("chvPhoneTypeCode") = "Home"
                        newrow("chvPhoneNumber") = TextBoxTelephone.Text.Trim()
                        newrow("bitActive") = 1
                        newrow("bitPrimary") = 1  '' SR:2010.07.21 

                        l_DataTable_MemberTelehone.Rows.Add(newrow)

                        If (l_bool_PrimTelExists = False) Then
                            YMCARET.YmcaBusinessObject.BeneficiarySettlement.Insert_MemberTelephone(l_DataSet_MemberTelehone)
                        Else
                            YMCARET.YmcaBusinessObject.BeneficiarySettlement.Update_MemberTelephone(l_DataSet_MemberTelehone)
                        End If
                    End If
                    l_DataSet_MemberTelehone.AcceptChanges()
                    l_DataTable_MemberTelehone.Rows.Clear()
                End If

                Dim l_bool_SecTelExists As Boolean = False
                'If (TextBoxSecTelephone.Text.Trim() <> "") Then
                newrow = l_DataTable_MemberTelehone.NewRow()

                If (Not l_DataSet_LookupMemberTelehone Is Nothing) Then
                    If (Not l_DataSet_LookupMemberTelehone.Tables(0) Is Nothing) Then
                        If (l_DataSet_LookupMemberTelehone.Tables(0).Rows.Count = 0) Then
                            newrow("guiUniqueID") = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_GetUniqueID()
                            l_bool_SecTelExists = False
                        Else
                            Dim l_DataView_Guid As DataView = l_DataSet_LookupMemberTelehone.Tables(0).DefaultView
                            l_DataView_Guid.RowFilter = "guiUniqueID <>'' and  bitPrimary=0 and bitActive=0"

                            If (l_DataView_Guid.Count > 0) Then
                                'YRS 5.0-424 : Priya 
                                If (Session("SecTelephoneNo") <> TextBoxSecTelephone.Text) Then
                                    If (Session("SecTelephoneNo") <> "") Then

                                        newrow("guiUniqueID") = l_DataView_Guid(0)(0).ToString()
                                        l_bool_SecTelExists = False

                                        newrow("guiEntityID") = l_string_BenePersUniqueID
                                        newrow("chvEntityCode") = "PERSON"
                                        newrow("dtmEffDate") = System.DateTime.Now.Date
                                        newrow("chvPhoneTypeCode") = "Home"
                                        newrow("chvPhoneNumber") = Session("SecTelephoneNo")
                                        newrow("bitActive") = 0
                                        newrow("bitPrimary") = 0

                                        l_DataSet_MemberTelehone.Tables("Member Details").Rows.Add(newrow)

                                        YMCARET.YmcaBusinessObject.BeneficiarySettlement.Update_MemberTelephone(l_DataSet_MemberTelehone)

                                        'l_DataSet_MemberTelehone.Tables("Member Details").AcceptChanges()
                                        l_DataSet_MemberTelehone.Tables("Member Details").Rows.Clear()
                                    Else
                                        l_bool_SecTelExists = False
                                    End If
                                    newrow("guiUniqueID") = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_GetUniqueID()
                                    'ElseIf TextBoxTelephone.Text = "" Then

                                ElseIf (Session("SecTelephoneNo") = TextBoxSecTelephone.Text) Then
                                    newrow("guiUniqueID") = l_DataView_Guid(0)(0).ToString()
                                    l_bool_SecTelExists = True
                                End If

                                'newrow("guiUniqueID") = l_DataView_Guid(0)(0).Tostring()
                                'l_bool_SecTelExists = True
                                'YRS 5.0-424 : Priya 
                            Else
                                newrow("guiUniqueID") = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_GetUniqueID()
                                l_bool_SecTelExists = False
                            End If
                        End If
                    End If
                    ' End If
                    'YRS 5.0-424 : Priya 
                    If (TextBoxSecTelephone.Text <> "") Then
                        newrow("guiEntityID") = l_string_BenePersUniqueID
                        newrow("chvEntityCode") = "PERSON"
                        newrow("dtmEffDate") = System.DateTime.Now.Date
                        newrow("chvPhoneTypeCode") = "Home"
                        newrow("chvPhoneNumber") = TextBoxSecTelephone.Text.Trim()
                        newrow("bitActive") = 1
                        newrow("bitPrimary") = 0

                        l_DataTable_MemberTelehone.Rows.Add(newrow)

                        If (l_bool_SecTelExists = False) Then
                            YMCARET.YmcaBusinessObject.BeneficiarySettlement.Insert_MemberTelephone(l_DataSet_MemberTelehone)
                        Else
                            YMCARET.YmcaBusinessObject.BeneficiarySettlement.Update_MemberTelephone(l_DataSet_MemberTelehone)
                        End If
                    End If
                    'YRS 5.0-424  
                    l_DataSet_MemberTelehone.AcceptChanges()
                End If
                'Insert/Update Email data //EMAIL

                Dim l_DataSet_LookupMemberEmail As DataSet = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_MemberEmailAddress(l_string_BenePersUniqueID)
                If (TextBoxEmailId.Text.Trim() <> "") Then
                    Dim l_bool_PrimEmailExists As Boolean = False
                    l_DataSet_MemberEmail = YMCARET.YmcaBusinessObject.BeneficiarySettlement.CreateDataSetForSave("EMAIL")
                    l_DataTable_MemberEmail = l_DataSet_MemberEmail.Tables("Member Details")
                    newrow = l_DataTable_MemberEmail.NewRow()

                    If (Not l_DataSet_LookupMemberEmail Is Nothing) Then
                        If (Not l_DataSet_LookupMemberEmail.Tables(0) Is Nothing) Then
                            If (l_DataSet_LookupMemberEmail.Tables(0).Rows.Count = 0) Then
                                newrow("guiUniqueID") = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_GetUniqueID()
                                l_bool_PrimEmailExists = False
                            Else
                                Dim l_DataView_Guid As DataView = l_DataSet_LookupMemberEmail.Tables(0).DefaultView
                                l_DataView_Guid.RowFilter = "guiUniqueID <>'' and  bitPrimary=1 and bitActive=1"
                                If (l_DataView_Guid.Count > 0) Then
                                    newrow("guiUniqueID") = l_DataView_Guid(0)(0).ToString()
                                    l_bool_PrimEmailExists = True
                                Else
                                    newrow("guiUniqueID") = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_GetUniqueID()
                                    l_bool_PrimEmailExists = False
                                End If
                            End If
                        End If
                    End If

                    newrow("guiEntityID") = l_string_BenePersUniqueID
                    newrow("chvEntityCode") = "PERSON"
                    newrow("dtmEffDate") = System.DateTime.Now.Date
                    newrow("chvEmailCode") = ""
                    newrow("chvEMailAddr") = TextBoxEmailId.Text.Trim()
                    newrow("bitActive") = 1
                    newrow("bitPrimary") = 1

                    l_DataTable_MemberEmail.Rows.Add(newrow)

                    If (l_bool_PrimEmailExists = False) Then
                        YMCARET.YmcaBusinessObject.BeneficiarySettlement.Insert_MemberEmailAddress(l_DataSet_MemberEmail)
                    Else
                        YMCARET.YmcaBusinessObject.BeneficiarySettlement.Update_MemberEmailAddress(l_DataSet_MemberEmail)
                    End If
                    l_DataSet_MemberEmail.AcceptChanges()
                    l_DataTable_MemberEmail.Rows.Clear()
                End If

                Dim l_bool_SecEmailExists As Boolean = False
                If (TextBoxSecEmail.Text.Trim() <> "") Then
                    newrow = l_DataTable_MemberEmail.NewRow()

                    If (Not l_DataSet_LookupMemberEmail Is Nothing) Then
                        If (Not l_DataSet_LookupMemberEmail.Tables(0) Is Nothing) Then
                            If (l_DataSet_LookupMemberEmail.Tables(0).Rows.Count = 0) Then
                                newrow("guiUniqueID") = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_GetUniqueID()
                                l_bool_SecEmailExists = False
                            Else
                                Dim l_DataView_Guid As DataView = l_DataSet_LookupMemberEmail.Tables(0).DefaultView
                                l_DataView_Guid.RowFilter = "guiUniqueID <>'' and  bitPrimary=0 and bitActive=0"
                                If (l_DataView_Guid.Count > 0) Then
                                    newrow("guiUniqueID") = l_DataView_Guid(0)(0).ToString()
                                    l_bool_SecEmailExists = True
                                Else
                                    newrow("guiUniqueID") = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_GetUniqueID()
                                    l_bool_SecEmailExists = False
                                End If
                            End If
                        End If
                    End If

                    newrow("guiEntityID") = l_string_BenePersUniqueID
                    newrow("chvEntityCode") = "PERSON"
                    newrow("dtmEffDate") = System.DateTime.Now.Date
                    newrow("chvEmailCode") = ""
                    newrow("chvEMailAddr") = TextBoxSecEmail.Text.Trim()
                    newrow("bitActive") = 0
                    newrow("bitPrimary") = 0


                    l_DataTable_MemberEmail.Rows.Add(newrow)

                    If (l_bool_SecEmailExists = False) Then
                        YMCARET.YmcaBusinessObject.BeneficiarySettlement.Insert_MemberEmailAddress(l_DataSet_MemberEmail)
                    Else
                        YMCARET.YmcaBusinessObject.BeneficiarySettlement.Update_MemberEmailAddress(l_DataSet_MemberEmail)
                    End If
                    l_DataSet_MemberEmail.AcceptChanges()
                End If

                ''Beneficiary SSnO updation
                If (l_string_PrevSSNo <> TextBoxGeneralSSNo.Text) Then
                    'SP:2013.04.12 :YRS 5.0-1990 -Added' -start
                    Dim string_guiuniqueID As String
                    If (Not Session("SelectedActiveBeneficiaryUniqueID") Is Nothing AndAlso Session("SelectedActiveBeneficiaryUniqueID").ToString() <> String.Empty) Then
                        string_guiuniqueID = Session("SelectedActiveBeneficiaryUniqueID").ToString().Trim()
                    ElseIf (Not Session("SelectedRetiredBeneficiaryUniqueID") Is Nothing AndAlso Session("SelectedRetiredBeneficiaryUniqueID").ToString() <> String.Empty) Then
                        string_guiuniqueID = Session("SelectedRetiredBeneficiaryUniqueID").ToString().Trim()
                    End If
                    If (String.IsNullOrEmpty(string_guiuniqueID)) Then
                        CloseFormStatus = showMessage(PlaceHolder1, "Error", "Couldn't Update New SSNo  due to Error", MessageBoxButtons.Stop)
                        Return False
                    End If
                    'SP:2013.04.12 :YRS 5.0-1990 -Added' -end
                    'SP:2013.04.12 :YRS 5.0-1990 -Added addtional parameter 'string_guiuniqueID' in below method "Update_BS_BeneficiariesSSNoWithNew"
                    Dim l_string_UpdateStatus As String = YMCARET.YmcaBusinessObject.BeneficiarySettlement.Update_BS_BeneficiariesSSNoWithNew(l_string_PrevSSNo, TextBoxGeneralSSNo.Text.Trim(), string_guiuniqueID)
                    If (l_string_UpdateStatus <> "Done") Then
                        CloseFormStatus = showMessage(PlaceHolder1, "Error", "Couldn't Update New SSNo  due to Error", MessageBoxButtons.Stop)  'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                        Return False
                    End If
                End If
                ''Inserton/Updation Email Address


                ''FOR UPDATION/INSERTION OF FundEvents
                Dim l_string_FunEventExists As String   ''This use later
                l_string_FunEventExists = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_Lookp_BeneficiaryFundEvent(l_string_BenePersUniqueID, GetEitherSettlementOptionId())
                If (l_string_FunEventExists = "NotValid") Then
                    CloseFormStatus = showMessage(PlaceHolder1, "Error", "Invalid benefundevent data encountered in save of BeneDetail form", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                    Return False
                ElseIf (l_string_FunEventExists <> "") Then
                    l_string_BeneFundEventID = l_string_FunEventExists
                ElseIf (l_string_FunEventExists = "") Then
                    ''Getting new UniqueID for BeneFundEvent
                    l_string_BeneFundEventID = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_GetUniqueID()
                End If

                Dim l_string_InsertFundDataStatus As String
                If (l_string_BeneFundEventID <> "" And l_string_OrigFundEventID <> "" And l_string_BenePersUniqueID <> "") Then
                    ' If (GetUniqueID = "AddMode") Then
                    'NP:IVP2:2008.12.05 - Passing Selected benefit Option Id so that the new fund event if required is created of the right type either DBEN or RBEN.
                    l_string_InsertFundDataStatus = YMCARET.YmcaBusinessObject.BeneficiarySettlement.Insert_BS_FundEventData(l_string_BeneFundEventID, l_string_OrigFundEventID, l_string_BenePersUniqueID, GetEitherSettlementOptionId())
                    If (l_string_InsertFundDataStatus = "") Then
                        CloseFormStatus = showMessage(PlaceHolder1, "Error", "Invalid benefundevent data encountered in save of BeneDetail form", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                        Return False
                    End If
                    '  End If
                    'NP:2008.01.03:YRPS-4046 - Changing the parameter being passed to the function call from SSNo to the selected BenefitOptionId.
                    ' We want to update the guiBenePersId field and the guiBeneFundEventId field only for the current beneficiary not for all beneficiaries sharing the same SSN
                    'l_string_InsertFundDataStatus = YMCARET.YmcaBusinessObject.BeneficiarySettlement.Update_BS_BeneficiariesFunEventandPersID(l_string_BeneFundEventID, l_string_BenePersUniqueID, TextBoxGeneralSSNo.Text.Trim)
                    l_string_InsertFundDataStatus = YMCARET.YmcaBusinessObject.BeneficiarySettlement.Update_BS_BeneficiariesFunEventandPersID(l_string_BeneFundEventID, l_string_BenePersUniqueID, GetEitherSettlementOptionId())
                    If (l_string_InsertFundDataStatus = "") Then
                        CloseFormStatus = showMessage(PlaceHolder1, "Error", "Invalid benefundevent data encountered in save of BeneDetail form", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                        Return False
                    End If
                Else
                    CloseFormStatus = showMessage(PlaceHolder1, "Error", "Error encountered creating beneficiary fundevent save of BeneDetail form", MessageBoxButtons.Stop)    'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                    Return False
                End If

                Me.GetUniqueID = l_string_BenePersUniqueID
                Session("PersId") = l_string_BenePersUniqueID
                Session("BeneficiarySSNo") = TextBoxGeneralSSNo.Text.Trim() 'SR | 2017.12.04 | YRS-AT-3756 | Set beneficiary information in session for manual RMD Screen
                'Validation before Saving 
                bool_Status = ValidateSettlementPrerequisites()
                If (bool_Status = False) Then
                    Return False
                End If
                Me.AddressWebUserControl1 = Nothing
                Me.AddressWebUserControl2 = Nothing
            Else
                CloseFormStatus = showMessage(PlaceHolder1, "Error", "Unable to save Beneficiary Information due to Error", MessageBoxButtons.Stop) 'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                Return False
            End If
            Session("Success_BIScreen") = True
            Session("SP_Parameters_DeathBenefitOptionID_RP") = Session("BS_SelectedOption_RP") 'Request.QueryString("BenefitOptionID")
            Session("SP_Parameters_DeathBenefitOptionID_SP") = Session("BS_SelectedOption_SP")
            If (RadioButtonAnnuityType_RetirementPlan.Visible = True AndAlso RadioButtonAnnuityType_RetirementPlan.SelectedIndex > -1) Then
                Session("SP_Parameters_AnnuityOption_RP") = RadioButtonAnnuityType_RetirementPlan.SelectedItem.Value
            End If
            If (RadioButtonAnnuityType_SavingsPlan.Visible = True AndAlso RadioButtonAnnuityType_SavingsPlan.SelectedIndex > -1) Then
                Session("SP_Parameters_AnnuityOption_SP") = RadioButtonAnnuityType_SavingsPlan.SelectedItem.Value
            End If
            Return True
        Catch ex As Exception
            Throw
            'START: PK |12/04/2019|YRS-AT-4605| Added finally to make variables clear and also to log trace.
        Finally
            isStateWithholdingDataSave = Nothing
            LstSWHPerssDetail = Nothing
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Beneficiary Information", "Save State Withholding END")
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace()
            'END: PK |12/04/2019|YRS-AT-4605| Added finally to make variables clear and also to log trace.
        End Try
    End Function

    Public Sub LoadFedWithDrawalTab()

        'Vipul 03Feb06 Cache-Session
        'Dim cache As CacheManager
        'cache = CacheFactory.GetCacheManager()
        'Vipul 03Feb06 Cache-Session

        Dim dr As DataRow

        'LoadFedWithDrawalTab
        Dim l_string_PersId As String
        Dim l_dataset_FedWith As DataSet

        Try
            l_string_PersId = Session("PersId")
            'Vipul 03Feb06 Cache-Session
            'l_dataset_FedWith = Cache("FedWithDrawals")
            l_dataset_FedWith = Session("FedWithDrawals")
            'Vipul 03Feb06 Cache-Session

            If Session("blnAddFedWithHoldings") = True Or Session("blnUpdateFedWithHoldings") = True Then

                If l_dataset_FedWith Is Nothing Then
                    Dim dt As New DataTable
                    l_dataset_FedWith = New DataSet
                    dt.Columns.Add("Tax Entity")
                    dt.Columns.Add("Type")
                    dt.Columns.Add("Add'l Amount")
                    dt.Columns.Add("Marital Status")
                    dt.Columns.Add("Exemptions")
                    dt.Columns.Add("persid")
                    l_dataset_FedWith.Tables.Add(dt)
                End If
                If Not (Session("blnUpdateFedWithHoldings") = True) Then
                    dr = l_dataset_FedWith.Tables(0).NewRow
                    dr("Tax Entity") = Session("cmbTaxEntity")
                    dr("Type") = Session("cmbWithHolding")
                    dr("Add'l Amount") = Session("txtAddlAmount")
                    dr("Marital Status") = Session("cmbMaritalStatus")
                    dr("Exemptions") = Session("txtExemptions")
                    dr("persid") = l_string_PersId

                    l_dataset_FedWith.Tables(0).Rows.Add(dr)
                End If

                'After adding the row to the dataset reset the session variable
                Session("blnAddFedWithHoldings") = False
                Session("blnUpdateFedWithHoldings") = False

            Else
                If l_dataset_FedWith Is Nothing Then
                    l_dataset_FedWith = YMCARET.YmcaBusinessObject.RetireesInformationBOClass.LookUpFedWithDrawals(l_string_PersId)
                Else
                    l_dataset_FedWith = Session("FedWithDrawals")
                End If

            End If

            DataGridGeneralWithhold.DataSource = l_dataset_FedWith
            ViewState("DS_Sort_BenInfo") = l_dataset_FedWith
            DataGridGeneralWithhold.DataBind()

            If (l_dataset_FedWith.Tables(0).Rows.Count > 0) Then

                DataGridGeneralWithhold.SelectedIndex = 0
                ButtonGeneralWithholdAdd.Enabled = True
            Else
                ButtonGeneralWithholdAdd.Enabled = True
            End If

            Session("FedWithDrawals") = l_dataset_FedWith
            SetGrossAmountToSTWUserControl() ' PK |10.24.2019| YRS-AT-4605 | Assign GrossAmount to State Withholding Control
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub LoadFederalWithHoldingForm()
        Session("cmbTaxEntity") = Me.DataGridGeneralWithhold.SelectedItem.Cells(4).Text
        Session("cmbWithHolding") = Me.DataGridGeneralWithhold.SelectedItem.Cells(3).Text.Trim
        Session("txtExemptions") = Me.DataGridGeneralWithhold.SelectedItem.Cells(1).Text.Trim
        Session("txtAddlAmount") = Me.DataGridGeneralWithhold.SelectedItem.Cells(2).Text.Trim
        Session("cmbMaritalStatus") = Me.DataGridGeneralWithhold.SelectedItem.Cells(5).Text.Trim
        'Shubhrata Mar 2nd,2007 YREN-3112
        Session("IsFedTaxForMaritalStatus") = True
        'Shubhrata Mar 2nd,2007 YREN-3112
        Dim popupScript As String
        If Me.DataGridGeneralWithhold.SelectedItem.Cells(6).Text = "&nbsp;" Then
            popupScript = "<script language='javascript'>" & _
                 "window.open('UpdateFedWithHoldingInfo.aspx?Index=" & Me.DataGridGeneralWithhold.SelectedIndex & "', 'CustomPopUp', " & _
                 "'width=450, height=400, menubar=no, resizable=yes,top=80,left=120, scrollbars=yes')" & _
                 "</script>"
        Else
            popupScript = "<script language='javascript'>" & _
               "window.open('UpdateFedWithHoldingInfo.aspx?UniqueID=" & Me.DataGridGeneralWithhold.SelectedItem.Cells(6).Text & "', 'CustomPopUp', " & _
               "'width=450, height=400, menubar=no, resizable=yes,top=80,left=120, scrollbars=yes')" & _
               "</script>"
        End If

        Page.RegisterStartupScript("PopupScript2", popupScript)



    End Sub

    Private Sub StoreFormDetails()
        Dim l_DataSet_BeneficiaryInformationStore As DataSet 'r_BeneficiaryPersonalDetails
        Try
            'Session("TextBoxGeneralSSNo") = TextBoxGeneralSSNo.Text
            l_string_SSNo = TextBoxGeneralSSNo.Text

            ' l_DataSet_BeneficiaryInformationStore.Tables.Add("r_BeneficiaryPersonalDetails")
            If (Not Session("BeneficiaryPersonalDetails") Is Nothing) Then
                l_DataSet_BeneficiaryInformationStore = DirectCast(Session("BeneficiaryPersonalDetails"), DataSet)  'Changed from CType to Directcast by SR:2010.06.21 for migration
            End If
            If (l_DataSet_BeneficiaryInformationStore.Tables("r_BeneficiaryPersonalDetails").Rows.Count = 0) Then
                Dim l_datarow As DataRow
                l_datarow = l_DataSet_BeneficiaryInformationStore.Tables("r_BeneficiaryPersonalDetails").NewRow
                l_DataSet_BeneficiaryInformationStore.Tables("r_BeneficiaryPersonalDetails").Rows.Add(l_datarow)
            End If
            With l_DataSet_BeneficiaryInformationStore.Tables("r_BeneficiaryPersonalDetails")
                .Rows(0)("guiUniqueID") = Session("PersID")
                If (TextBoxGeneralFundNo.Text.Trim() <> "") Then
                    .Rows(0)("Fund No") = Int32.Parse(TextBoxGeneralFundNo.Text)
                Else
                    .Rows(0)("Fund No") = 0
                End If
                .Rows(0)("Last Name") = TextBoxGeneralLastName.Text.Trim()
                .Rows(0)("First Name") = TextBoxGeneralFirstName.Text.Trim()
                .Rows(0)("Middle Name") = TextBoxGeneralMiddleName.Text.Trim()
                .Rows(0)("Salute") = cboSalute.SelectedItem.Text
                .Rows(0)("Suffix") = TextBoxGeneralSuffix.Text.Trim()
                .Rows(0)("Gender") = DropDownListGeneralGender.SelectedValue.ToString().Trim()
                If (TextBoxGeneralDOB.Text.Trim() <> "") Then
                    .Rows(0)("Birth Date") = DateTime.Parse(TextBoxGeneralDOB.Text.Trim())
                Else
                    .Rows(0)("Birth Date") = DBNull.Value
                End If
                .Rows(0)("Marital Status") = cboGeneralMaritalStatus.SelectedValue.ToString().Trim()
                .Rows(0)("Death Date") = DBNull.Value
            End With
            Session("BeneficiaryPersonalDetails") = l_DataSet_BeneficiaryInformationStore
            StoreBenefitInformationDetails()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub StoreBenefitInformationDetails()

        Dim l_DataSet_POA As DataSet
        Dim l_DataSet_Address As New DataSet
        Try
            l_DataSet_POA = Session("POADetialsStore")
            If (l_DataSet_POA Is Nothing) Then
                Dim dt As New DataTable
                l_DataSet_POA = New DataSet
                Dim dr As DataRow

                dt.Columns.Add("chvPoaName1")
                dr = dt.NewRow()
                dr("chvPoaName1") = TextBoxGeneralPOA.Text.Trim()
                dt.Rows.Add(dr)
                l_DataSet_POA.Tables.Add(dt)

            Else
                l_DataSet_POA.Tables(0).Rows(0)("chvPoaName1") = TextBoxGeneralPOA.Text.Trim()
            End If
            Session("POADetialsStore") = l_DataSet_POA


            l_DataSet_Address = Session("AddressDetailsStore")
            If (Not l_DataSet_Address Is Nothing) Then
                If (l_DataSet_Address.Tables(0).Rows.Count > 0) Then
                    l_DataSet_Address.Tables(0).Rows(0)("bitPrimary") = 1
                    '    l_DataSet_Address.Tables(0).Rows(0)("bitActive") = 1
                    l_DataSet_Address.Tables(0).Rows(0)("chvaddr1") = AddressWebUserControl1.Address1.ToString().Trim()  'TextBoxAddress1.Text.Trim()
                    l_DataSet_Address.Tables(0).Rows(0)("chvaddr2") = AddressWebUserControl1.Address2.ToString().Trim()  'TextBoxAddress2.Text.Trim()
                    l_DataSet_Address.Tables(0).Rows(0)("chvaddr3") = AddressWebUserControl1.Address3.ToString().Trim()  'TextBoxAddress3.Text.Trim()
                    l_DataSet_Address.Tables(0).Rows(0)("ChvCity") = AddressWebUserControl1.City.ToString().Trim()  'TextBoxCity.Text.Trim()
                    If AddressWebUserControl1.DropDownListCountryValue.ToString().Trim() = "CA" Then
                        l_DataSet_Address.Tables(0).Rows(0)("chrzip") = Replace(AddressWebUserControl1.ZipCode.ToString().Trim(), " ", "")    'TextBoxZip.Text.Trim()
                    Else
                        l_DataSet_Address.Tables(0).Rows(0)("chrzip") = AddressWebUserControl1.ZipCode.ToString().Trim()      'TextBoxZip.Text.Trim()
                    End If
                    l_DataSet_Address.Tables(0).Rows(0)("chrStateType") = IIf(AddressWebUserControl1.DropDownListStateValue = "-Select State-", "", AddressWebUserControl1.DropDownListStateValue.ToString().Trim())    'DropdownlistState.SelectedValue.ToString().Trim()
                    l_DataSet_Address.Tables(0).Rows(0)("chvCountry") = IIf(AddressWebUserControl1.DropDownListCountryValue = "-Select Country-", "", AddressWebUserControl1.DropDownListCountryValue.ToString().Trim()) 'DropDownListCountry.SelectedValue.ToString().Trim()
                    If (l_DataSet_Address.Tables(0).Rows.Count > 1) Then
                        l_DataSet_Address.Tables(0).Rows(1)("bitPrimary") = 0
                        ' l_DataSet_Address.Tables(0).Rows(1)("bitActive") = 0
                        l_DataSet_Address.Tables(0).Rows(1)("chvaddr1") = AddressWebUserControl2.Address1.ToString().Trim() ' TextBoxSecAddress1.Text.Trim()
                        l_DataSet_Address.Tables(0).Rows(1)("chvaddr2") = AddressWebUserControl2.Address2.ToString().Trim()  'TextBoxSecAddress2.Text.Trim()
                        l_DataSet_Address.Tables(0).Rows(1)("chvaddr3") = AddressWebUserControl2.Address3.ToString().Trim()  'TextBoxSecAddress3.Text.Trim()
                        l_DataSet_Address.Tables(0).Rows(1)("ChvCity") = AddressWebUserControl2.City.ToString().Trim()  'TextBoxSecCity.Text.Trim()
                        If AddressWebUserControl2.DropDownListCountryValue.ToString().Trim() = "CA" Then
                            l_DataSet_Address.Tables(0).Rows(1)("chrzip") = Replace(AddressWebUserControl2.ZipCode.ToString().Trim(), " ", "")   'TextBoxSecZip.Text.Trim()
                        Else
                            l_DataSet_Address.Tables(0).Rows(1)("chrzip") = AddressWebUserControl2.ZipCode.ToString().Trim()
                        End If
                        l_DataSet_Address.Tables(0).Rows(1)("chrStateType") = IIf(AddressWebUserControl2.DropDownListStateValue = "-Select State-", "", AddressWebUserControl2.DropDownListStateValue.ToString().Trim())        'DropdownlistSecState.SelectedValue.ToString().Trim()
                        l_DataSet_Address.Tables(0).Rows(1)("chvCountry") = IIf(AddressWebUserControl2.DropDownListCountryValue = "-Select Country-", "", AddressWebUserControl2.DropDownListCountryValue.ToString().Trim())  'DropdownlistSecCountry.SelectedValue.ToString().Trim()
                    Else
                        Dim dr As DataRow

                        dr = l_DataSet_Address.Tables(0).NewRow()

                        dr("bitPrimary") = 0
                        '  dr("bitActive") = 0
                        dr("chvaddr1") = AddressWebUserControl2.Address1.ToString().Trim() 'TextBoxSecAddress1.Text.Trim()
                        dr("chvaddr2") = AddressWebUserControl2.Address2.ToString().Trim() 'TextBoxSecAddress2.Text.Trim()
                        dr("chvaddr3") = AddressWebUserControl2.Address3.ToString().Trim() 'TextBoxSecAddress3.Text.Trim()
                        dr("ChvCity") = AddressWebUserControl2.City.ToString().Trim() 'TextBoxSecCity.Text.Trim()
                        If AddressWebUserControl2.DropDownListCountryValue.ToString().Trim() = "CA" Then
                            dr("chrzip") = Replace(AddressWebUserControl2.ZipCode.ToString().Trim(), " ", "")
                        Else
                            dr("chrzip") = AddressWebUserControl2.ZipCode.ToString().Trim()  'TextBoxSecZip.Text.Trim()
                        End If
                        dr("chrStateType") = IIf(AddressWebUserControl2.DropDownListStateValue = "-Select State-", "", AddressWebUserControl2.DropDownListStateValue.ToString().Trim())     'DropdownlistSecState.SelectedValue.ToString().Trim()
                        dr("chvCountry") = IIf(AddressWebUserControl2.DropDownListCountryValue = "-Select Country-", "", AddressWebUserControl2.DropDownListCountryValue.ToString().Trim())  'DropdownlistSecCountry.SelectedValue.ToString().Trim()

                        l_DataSet_Address.Tables(0).Rows.Add(dr)

                    End If
                Else
                    Dim dt As New DataTable
                    l_DataSet_Address = New DataSet
                    Dim dr As DataRow
                    dt.Columns.Add("bitPrimary")
                    '  dt.Columns.Add("bitActive")
                    dt.Columns.Add("chvaddr1")
                    dt.Columns.Add("chvaddr2")
                    dt.Columns.Add("chvaddr3")
                    dt.Columns.Add("ChvCity")
                    dt.Columns.Add("chrzip")
                    dt.Columns.Add("chrStateType")
                    dt.Columns.Add("chvCountry")
                    dr = dt.NewRow()
                    dr("bitPrimary") = 1
                    '  dr("bitActive") = 1
                    dr("chvaddr1") = AddressWebUserControl1.Address1.ToString().Trim() 'TextBoxAddress1.Text.Trim()
                    dr("chvaddr2") = AddressWebUserControl1.Address2.ToString().Trim() 'TextBoxAddress2.Text.Trim()
                    dr("chvaddr3") = AddressWebUserControl1.Address3.ToString().Trim() 'TextBoxAddress3.Text.Trim()
                    dr("ChvCity") = AddressWebUserControl1.City.ToString().Trim() 'TextBoxCity.Text.Trim()
                    If AddressWebUserControl1.DropDownListCountryValue.ToString().Trim() = "CA" Then
                        dr("chrzip") = Replace(AddressWebUserControl1.ZipCode.ToString().Trim(), " ", "")   'TextBoxZip.Text.Trim()
                    Else
                        dr("chrzip") = AddressWebUserControl1.ZipCode.ToString().Trim()
                    End If
                    dr("chrStateType") = IIf(AddressWebUserControl1.DropDownListStateValue = "-Select State-", "", AddressWebUserControl1.DropDownListStateValue.ToString().Trim()) 'DropdownlistState.SelectedValue.ToString().Trim()
                    dr("chvCountry") = IIf(AddressWebUserControl1.DropDownListCountryValue = "-Select Country-", "", AddressWebUserControl1.DropDownListCountryValue.ToString().Trim()) 'DropDownListCountry.SelectedValue.ToString().Trim()
                    dt.Rows.Add(dr)

                    dr = dt.NewRow()

                    dr("bitPrimary") = 0
                    '  dr("bitActive") = 0
                    dr("chvaddr1") = AddressWebUserControl2.Address1.ToString().Trim()       'TextBoxSecAddress1.Text.ToString().Trim()
                    dr("chvaddr2") = AddressWebUserControl2.Address2.ToString().Trim()      'TextBoxSecAddress2.Text.ToString().Trim()
                    dr("chvaddr3") = AddressWebUserControl2.Address3.ToString().Trim()      'TextBoxSecAddress3.Text.ToString().Trim()
                    dr("ChvCity") = AddressWebUserControl2.City.ToString().Trim()   'TextBoxSecCity.Text.Trim()
                    If AddressWebUserControl2.DropDownListCountryValue.ToString().Trim() = "CA" Then
                        dr("chrzip") = Replace(AddressWebUserControl2.ZipCode.ToString().Trim(), " ", "")   'dr("chrzip") = TextBoxSecZip.Text.Trim()
                    Else
                        dr("chrzip") = AddressWebUserControl2.ZipCode.ToString().Trim() 'dr("chrzip") = TextBoxSecZip.Text.Trim()
                    End If
                    dr("chrStateType") = IIf(AddressWebUserControl2.DropDownListStateValue = "-Select State-", "", AddressWebUserControl2.DropDownListStateValue.ToString().Trim()) 'DropdownlistSecState.SelectedValue.ToString().Trim()
                    dr("chvCountry") = IIf(AddressWebUserControl2.DropDownListCountryValue = "-Select Country-", "", AddressWebUserControl2.DropDownListCountryValue.ToString().Trim()) 'DropdownlistSecCountry.SelectedValue.ToString().Trim()

                    dt.Rows.Add(dr)

                    l_DataSet_Address.Tables.Add(dt)
                End If
            Else
                Dim dt As New DataTable
                l_DataSet_Address = New DataSet
                Dim dr As DataRow
                dt.Columns.Add("bitPrimary")
                '  dt.Columns.Add("bitActive")
                dt.Columns.Add("chvaddr1")
                dt.Columns.Add("chvaddr2")
                dt.Columns.Add("chvaddr3")
                dt.Columns.Add("ChvCity")
                dt.Columns.Add("chrzip")
                dt.Columns.Add("chrStateType")
                dt.Columns.Add("chvCountry")
                dr = dt.NewRow()
                dr("bitPrimary") = 1
                '  dr("bitActive") = 1

                'dr("chvaddr1") =  TextBoxAddress1.Text.Trim()
                dr("chvaddr1") = AddressWebUserControl1.Address1.ToString().Trim()

                'dr("chvaddr2") = TextBoxAddress2.Text.Trim()
                dr("chvaddr2") = AddressWebUserControl1.Address2.ToString().Trim()

                'dr("chvaddr3") = TextBoxAddress3.Text.Trim()
                dr("chvaddr3") = AddressWebUserControl1.Address3.ToString().Trim()

                'dr("ChvCity") = TextBoxCity.Text.Trim()
                dr("ChvCity") = AddressWebUserControl1.City.ToString().Trim()

                'dr("chrzip") = TextBoxZip.Text.Trim()
                If AddressWebUserControl1.DropDownListCountryValue.ToString().Trim() = "CA" Then
                    dr("chrzip") = Replace(AddressWebUserControl1.ZipCode.ToString().Trim(), " ", "")
                    'dr("chrStateType") = DropdownlistState.SelectedValue.ToString().Trim()
                Else
                    dr("chrzip") = AddressWebUserControl1.ZipCode.ToString().Trim()
                    'dr("chrStateType") = DropdownlistState.SelectedValue.ToString().Trim()
                End If

                dr("chrStateType") = IIf(AddressWebUserControl1.DropDownListStateValue = "-Select State-", "", AddressWebUserControl1.DropDownListStateValue.ToString().Trim())

                'dr("chvCountry") = DropDownListCountry.SelectedValue.ToString().Trim()
                dr("chvCountry") = IIf(AddressWebUserControl1.DropDownListCountryValue = "-Select Country-", "", AddressWebUserControl1.DropDownListCountryValue.ToString().Trim())

                dt.Rows.Add(dr)

                dr = dt.NewRow()

                dr("bitPrimary") = 0
                '    dr("bitActive") = 0
                dr("chvaddr1") = AddressWebUserControl2.Address1.ToString().Trim() 'TextBoxSecAddress1.Text.Trim()
                dr("chvaddr2") = AddressWebUserControl2.Address2.ToString().Trim() 'TextBoxSecAddress2.Text.Trim()
                dr("chvaddr3") = AddressWebUserControl2.Address3.ToString().Trim() 'TextBoxSecAddress3.Text.Trim()
                dr("ChvCity") = AddressWebUserControl2.City.ToString().Trim() 'TextBoxSecCity.Text.Trim()
                If AddressWebUserControl2.DropDownListCountryValue.ToString().Trim() = "CA" Then
                    dr("chrzip") = Replace(AddressWebUserControl2.ZipCode.ToString().Trim(), " ", "")    'TextBoxSecZip.Text.Trim()
                Else
                    dr("chrzip") = AddressWebUserControl2.ZipCode.ToString().Trim()     'TextBoxSecZip.Text.Trim()
                End If

                dr("chrStateType") = IIf(AddressWebUserControl2.DropDownListStateValue = "-Select State-", "", AddressWebUserControl2.DropDownListStateValue.ToString().Trim())     'DropdownlistSecState.SelectedValue.ToString().Trim()
                dr("chvCountry") = IIf(AddressWebUserControl2.DropDownListCountryValue = "-Select Country-", "", AddressWebUserControl2.DropDownListCountryValue.ToString().Trim())   ' DropdownlistSecCountry.SelectedValue.ToString().Trim()

                dt.Rows.Add(dr)

                l_DataSet_Address.Tables.Add(dt)

            End If
            '    cache.Add("AddressDetailsStore", l_DataSet_Address)
            Session("AddressDetailsStore") = l_DataSet_Address

        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Private Sub SetStateValuesBasedonCountrySelected()
    '    Try
    '        Dim l_DataSet_StateNames As DataSet
    '        If (DropDownListCountry.SelectedIndex <> 0) Then
    '            l_DataSet_StateNames = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_StateNames(DropDownListCountry.SelectedValue.ToString().Trim())
    '            DropdownlistState.DataSource = l_DataSet_StateNames.Tables(0)
    '            DropdownlistState.DataTextField = "Description"
    '            DropdownlistState.DataValueField = "chvcodevalue"
    '            DropdownlistState.DataBind()
    '            DropdownlistState.Items.Insert(0, "-Select State-")
    '        Else
    '            DropdownlistState.Items.Clear()
    '            DropdownlistState.Items.Insert(0, "-Select State-")
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Sub

    'Private Sub SetSecStateValuesBasedonSecCountrySelected()
    '    Try
    '        Dim l_DataSet_StateNames As DataSet
    '        If (DropdownlistSecCountry.SelectedIndex <> 0) Then
    '            l_DataSet_StateNames = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BS_StateNames(DropdownlistSecCountry.SelectedValue.ToString().Trim())
    '            DropdownlistSecState.DataSource = l_DataSet_StateNames.Tables(0)
    '            DropdownlistSecState.DataTextField = "Description"
    '            DropdownlistSecState.DataValueField = "chvcodevalue"
    '            DropdownlistSecState.DataBind()
    '            DropdownlistSecState.Items.Insert(0, "-Select State-")
    '        Else
    '            DropdownlistSecState.Items.Clear()
    '            DropdownlistSecState.Items.Insert(0, "-Select State-")
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Sub

    Private Sub SetPrimaryAddressDetails()
        str_Pri_Address1 = AddressWebUserControl1.Address1
        str_Pri_Address2 = AddressWebUserControl1.Address2
        str_Pri_Address3 = AddressWebUserControl1.Address3
        str_Pri_City = AddressWebUserControl1.City
        str_Pri_Zip = AddressWebUserControl1.ZipCode
        str_Pri_StateValue = AddressWebUserControl1.DropDownListStateValue
        str_Pri_CountryValue = AddressWebUserControl1.DropDownListCountryValue
        str_Pri_CountryText = AddressWebUserControl1.DropDownListCountryText
        str_Pri_StateText = AddressWebUserControl1.DropDownListStateText
    End Sub

    Private Sub SetSecondaryAddressDetails()
        str_Sec_Address1 = AddressWebUserControl2.Address1
        str_Sec_Address2 = AddressWebUserControl2.Address2
        str_Sec_Address3 = AddressWebUserControl2.Address3
        str_Sec_City = AddressWebUserControl2.City
        str_Sec_Zip = AddressWebUserControl2.ZipCode
        str_Sec_StateValue = AddressWebUserControl2.DropDownListStateValue
        str_Sec_CountryValue = AddressWebUserControl2.DropDownListCountryValue
        str_Sec_CountryText = AddressWebUserControl2.DropDownListCountryText
        str_Sec_StateText = AddressWebUserControl2.DropDownListStateText
    End Sub

    'Private Function ValidateCountrySelStateZip(ByVal CountryValue As String, ByVal StateValue As String, ByVal str_Pri_Zip As String) As String
    '    '**********************************************************************************************************************
    '    ' Author            : Ashutosh Patil 
    '    ' Created On        : 25-Jan-2007
    '    ' Desc              : This function will validate for the state and Zip Code selected against country USA and Canada
    '    '                     FOR USA and CANADA - State and Zip Code is mandatory
    '    ' Related To        : YREN-3029,YREN-3028
    '    ' Modifed By        : 
    '    ' Modifed On        :
    '    ' Reason For Change : 
    '    '***********************************************************************************************************************
    '    Dim l_Str_Msg As String = ""

    '    Try
    '        If (CountryValue = "US" Or CountryValue = "CA") And StateValue = "" Then
    '            l_Str_Msg = "Please select state"
    '        ElseIf (CountryValue = "US" Or CountryValue = "CA") And str_Pri_Zip = "" Then
    '            l_Str_Msg = "Please enter Zip Code"
    '        ElseIf CountryValue = "US" And (Len(str_Pri_Zip) <> 5 And Len(str_Pri_Zip) <> 9) Then
    '            l_Str_Msg = "Invalid Zip Code format"
    '        ElseIf CountryValue = "CA" And (Len(str_Pri_Zip) <> 7) Then
    '            l_Str_Msg = "Invalid Zip Code format"
    '        End If
    '        If CountryValue = "US" And l_Str_Msg <> "" Then
    '            l_Str_Msg = l_Str_Msg & " for United States"
    '        ElseIf CountryValue = "CA" And l_Str_Msg <> "" Then
    '            l_Str_Msg = l_Str_Msg & " for Canada"
    '        End If

    '        ValidateCountrySelStateZip = l_Str_Msg
    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()), False)
    '    End Try
    'End Function

    'NP:2008.01.03 - Adding utility function to obtain a benefitOptionID selected for settlement.
    'This function returns the RetirementPlan Settlement Option if specified else the Savings Plan option.
    'If neither are specified then it returns string.empty

    Private Function GetEitherSettlementOptionId() As String
        If Not l_string_BenefitOptionID Is Nothing AndAlso l_string_BenefitOptionID <> String.Empty Then
            Return l_string_BenefitOptionID
        ElseIf Not l_string_BenefitOptionID_SavingsPlan Is Nothing AndAlso l_string_BenefitOptionID_SavingsPlan <> String.Empty Then
            Return l_string_BenefitOptionID_SavingsPlan
        Else
            Return String.Empty
        End If
    End Function

#End Region

#Region "Event Methods"

    Private Sub ButtonRetireesInfoSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRetireesInfoSave.Click
        'Perform all validations here and then call the save function
        Dim l_int_SSNoCount As Integer
        Dim l_string_Message As String = ""
        Dim l_DataSet_Neweneficiary As DataSet
        'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Declaring object variable
        Dim dsActiveSettlementOption_RP As DataSet
        Dim dsActiveSettlementOption_SP As DataSet
        Dim strDeathBenOptionID_RP As String
        Dim strDeathBenOptionID_SP As String
        Dim dvActiveSettlementOption_RP As DataView
        Dim dvActiveSettlementOption_SP As DataView
        Dim dtmDeathDate As DateTime
        Dim dtmAnnuityPurchaseDate As DateTime
        Dim intPayrollMonthCount As Integer
        'End - Manthan | 2016.04.22 | YRS-AT-2206 | Declaring object variable


        Dim i As Integer
        Dim l_String_len As String
        Dim strWSMessage As String
        Dim stTelephoneError As String 'PPP | 2015.10.13 | YRS-AT-2588

        Try
            'SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
            If Session("ParticipantEntityId") <> Nothing Then
                'AA:2015.05.06 - BT:2824 : Changed class name and namespace ServiceManager.AdminConsoleBeneficiaryTracking
                strWSMessage = ServiceManager.AdminConsoleBeneficiaryTracking.IsValidPerson(Session("ParticipantEntityId"))
                If strWSMessage <> "NoPending" Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "Process Restricted", "openDialog('" + strWSMessage + "','Pers');", True)
                    Exit Sub
                End If
            End If
            'End, SR:2013.08.05 - YRS 5.0-2070 : Restrict add and edit operation if person has pending record in Tom's console application
            If RadioButtonAnnuityType_RetirementPlan.Enabled = True AndAlso RadioButtonAnnuityType_RetirementPlan.Visible = True AndAlso RadioButtonAnnuityType_RetirementPlan.SelectedIndex < 0 Then
                showMessage(PlaceHolder1, "Error", "Please select an annuity option for the Retirement Plan", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                CloseFormStatus = False
                Exit Sub
            End If
            If RadioButtonAnnuityType_SavingsPlan.Enabled = True AndAlso RadioButtonAnnuityType_SavingsPlan.Visible = True AndAlso RadioButtonAnnuityType_SavingsPlan.SelectedIndex < 0 Then
                showMessage(PlaceHolder1, "Error", "Please select an annuity option for the Savings Plan", MessageBoxButtons.Stop)  'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                CloseFormStatus = False
                Exit Sub
            End If

            If (GetUniqueID = "AddMode") Then
                'NP:PS:2007.08.28 - 'l_DataSet_Neweneficiary = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_NewBeneficiary(l_string_BenefitOptionID) 
                If l_string_BenefitOptionID <> "" Then
                    l_DataSet_Neweneficiary = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_NewBeneficiary(Me.l_string_BenefitOptionID)
                ElseIf l_string_BenefitOptionID_SavingsPlan <> "" Then
                    l_DataSet_Neweneficiary = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_NewBeneficiary(l_string_BenefitOptionID_SavingsPlan)
                End If
                If HelperFunctions.isEmpty(l_DataSet_Neweneficiary) Then
                    CloseFormStatus = showMessage(PlaceHolder1, "Error", "Unable to Locate a Beneficary Record for Benefit option ", MessageBoxButtons.Stop)    'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                    Exit Sub
                End If
            End If

            'Validation for SSN No
            If TextBoxGeneralSSNo.Text.Length < 9 Then
                showMessage(PlaceHolder1, "Verify", "SSNo. must be nine digits", MessageBoxButtons.Stop)    'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                CloseFormStatus = False
                Exit Sub
            End If

            Try
                'Anil 07/01/2008 -  US Participants should have a valid SSN no and Parsed into Integer
                If AddressWebUserControl1.DropDownListCountryText.ToString().Trim() = "UNITED STATES" Then
                    Int32.Parse(TextBoxGeneralSSNo.Text.Trim())
                End If
                'Anil 07/01/2008 -  US Participants should have a valid SSN no and Parsed into Integer
            Catch ex As Exception
                showMessage(PlaceHolder1, "Verify", "Invalid Social Security Number/ Federal Tax Number", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                CloseFormStatus = False
                Exit Sub
            End Try

            l_String_len = TextBoxGeneralSSNo.Text.Trim().Substring(0, 1)

            If (TextBoxGeneralLastName.Text.Trim = "") Then
                showMessage(PlaceHolder1, "Verify", "Last Name can not be empty", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                CloseFormStatus = False
                Exit Sub
            End If

            If (TextBoxGeneralFirstName.Text.Trim = "") Then
                showMessage(PlaceHolder1, "Verify", "First Name can not be empty", MessageBoxButtons.Stop)  'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                CloseFormStatus = False
                Exit Sub
            End If

            'Ashutosh Patil as on 14-Mar-2007
            'YREN - 3028,YREN-3029
            'Primary Address
            Call SetPrimaryAddressDetails()

            'Secondary Address
            Call SetSecondaryAddressDetails()

            'Ashutosh Patil as on 09-mar-2007
            'YREN - 3028, YREN - 3029
            'If (TextBoxAddress1.Text = "" And TextBoxAddress2.Text = "" And TextBoxAddress3.Text = "") Then
            '    showMessage(PlaceHolder1, "Verify", "Primary address data does not exist for this beneficiary", MessageBoxButtons.Stop)
            '    CloseFormStatus = False
            '    Exit Sub
            'End If

            If (TaxWithHoldRequired = True And TabStripRetireesInformation.Items(0).Enabled = True) Then
                If (DataGridGeneralWithhold.Items.Count = 0) Then
                    showMessage(PlaceHolder1, "Error", "Federal tax withholding data does not exist for this beneficiary", MessageBoxButtons.Stop)  'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                    CloseFormStatus = False
                    Exit Sub
                End If
            End If
            'START: PK |12/04/2019 |YRS-AT-4605|Called Validation method here.
            If Me.ValidateSTWvsFedtaxforMA() = False Then
                Exit Sub
            End If
            'END: PK |12/04/2019 |YRS-AT-4605|Called Validation method here.
            'Primary Address Validations
            'Ashutosh Patil as on 14-Mar-2007
            'YREN - 3028,YREN-3029
            'If Trim(str_Pri_Address1) = "" Then
            '    Session("AddressOk") = 1
            '    showMessage(PlaceHolder1, "YMCA", "Please enter Address1 for Primary Address.", MessageBoxButtons.Stop) 'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
            '    Exit Sub
            'End If

            'If Trim(str_Pri_City) = "" Then
            '    Session("AddressOk") = 1
            '    showMessage(PlaceHolder1, "YMCA", "Please enter City for Primary Address.", MessageBoxButtons.Stop) 'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
            '    Exit Sub
            'End If

            'If Trim(str_Pri_Address1) <> "" Then
            '    If str_Pri_CountryText = "-Select Country-" And str_Pri_StateText = "-Select State-" Then
            '        Session("AddressOk") = 1
            '        showMessage(PlaceHolder1, "YMCA", "Please select either State or Country for Primary Address.", MessageBoxButtons.Stop) 'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
            '        Exit Sub
            '    End If
            'End If

            'If (str_Pri_CountryText = "-Select Country-") Then
            '    If str_Pri_StateText = "-Select State-" Then
            '        Session("AddressOk") = 1
            '        showMessage(PlaceHolder1, "YMCA", "Please select either State or Country for Primary Address.", MessageBoxButtons.Stop) 'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
            '        Exit Sub
            '    End If
            'End If


            'l_string_Message = ValidateCountrySelStateZip(str_Pri_CountryValue, str_Pri_StateValue, str_Pri_Zip)
            'If l_string_Message <> "" Then
            '    Session("AddressOk") = 1
            '    showMessage(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop) 'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
            '    Exit Sub
            'End If

            'If str_Pri_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(str_Pri_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
            '    Session("AddressOk") = 1
            '    TextBoxZip.Text = ""
            '    showMessage(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop) 'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
            '    Exit Sub
            'End If
            l_string_Message = AddressWebUserControl1.ValidateAddress()
            If l_string_Message <> "" Then
                Session("AddressOk") = 1
                showMessage(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop) 'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                Exit Sub

            End If


            ''Secondary Address Validations
            ''Ashutosh Patil as on 14-Mar-2007
            ''YREN - 3028,YREN-3029
            'If Trim(str_Sec_Address1) <> "" Then
            '    If Trim(str_Sec_City) = "" Then
            '        Session("AddressOk") = 1
            '        showMessage(PlaceHolder1, "YMCA", "Please enter City for Secondary Address.", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
            '        Exit Sub
            '    End If
            '    If (str_Sec_CountryText = "-Select Country-") Then
            '        If str_Sec_StateText = "-Select State-" Then
            '            Session("AddressOk") = 1
            '            showMessage(PlaceHolder1, "YMCA", "Please select either State or Country for Secondary Address.", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
            '            Exit Sub
            '        End If
            '    End If
            'End If

            'If Trim(str_Sec_Address2) <> "" Then
            '    If Trim(str_Sec_Address1) = "" Then
            '        Session("AddressOk") = 1
            '        showMessage(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
            '        Exit Sub
            '    End If
            'End If

            'If Trim(str_Sec_Address3) <> "" Then
            '    If Trim(str_Sec_Address1) = "" Then
            '        Session("AddressOk") = 1
            '        showMessage(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
            '        Exit Sub
            '    End If
            'End If

            'If (str_Sec_CountryText <> "-Select Country-" Or str_Sec_StateText <> "-Select State-") Then
            '    If Trim(str_Sec_Address1) = "" Then
            '        Session("AddressOk") = 1
            '        showMessage(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
            '        Exit Sub
            '    End If
            'End If

            'If Trim(str_Sec_Zip) <> "" Then
            '    If Trim(str_Sec_Address1) = "" Then
            '        Session("AddressOk") = 1
            '        showMessage(PlaceHolder1, "YMCA", "Please enter Address1 for Secondary Address.", MessageBoxButtons.Stop)   'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
            '        Exit Sub
            '    End If
            'End If

            'l_string_Message = ValidateCountrySelStateZip(str_Sec_CountryValue, str_Sec_StateValue, str_Sec_Zip)

            'If l_string_Message <> "" Then
            '    Session("AddressOk") = 1
            '    showMessage(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop) 'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
            '    Exit Sub
            'End If

            'If str_Sec_CountryValue = "CA" And (System.Text.RegularExpressions.Regex.IsMatch(str_Sec_Zip, "^([a-zA-Z])([0-9])([a-zA-Z]) ([0-9])([a-zA-Z])([0-9])")) = False Then
            '    Session("AddressOk") = 1
            '    showMessage(PlaceHolder1, "YMCA", "Invalid Zip Code format For Country Canada", MessageBoxButtons.Stop) 'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
            '    Exit Sub
            'End If

            l_string_Message = AddressWebUserControl2.ValidateAddress()
            If l_string_Message <> "" Then
                Session("AddressOk") = 1
                showMessage(PlaceHolder1, "YMCA", l_string_Message, MessageBoxButtons.Stop) 'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                Exit Sub
            End If

            'BS:2012.05.17:YRS 5.0-1470 :-validate verify address
            Dim i_str_Message1 As String = String.Empty
            i_str_Message1 = AddressWebUserControl1.ValidateAddress()
            If i_str_Message1 <> "" Then
                Session("VerifiedAddress") = "VerifiedAddress"
                MessageBox.Show(PlaceHolder1, "YMCA", i_str_Message1, MessageBoxButtons.YesNo)
                Exit Sub
            End If

            'BS:2012.05.17:YRS 5.0-1470 :-validate verify address
            Dim i_str_Message2 As String = String.Empty
            i_str_Message2 = AddressWebUserControl2.ValidateAddress()
            If i_str_Message2 <> "" Then
                Session("VerifiedAddress") = "VerifiedAddress"
                MessageBox.Show(PlaceHolder1, "YMCA", i_str_Message2, MessageBoxButtons.YesNo)
                Exit Sub
            End If

            If (l_string_SSNo <> TextBoxGeneralSSNo.Text) Then
                YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_BI_ExistingSSNo(TextBoxGeneralSSNo.Text, l_string_Message)

                If (l_string_Message <> "") Then
                    showMessage(PlaceHolder1, "Verify", l_string_Message, MessageBoxButtons.YesNo)  'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
                    Exit Sub
                End If

            End If
            'Anudeep:2013.04.13-BT-1555:YRS 5.0-1769:Length of phone numbers
            If AddressWebUserControl1.DropDownListCountryValue = "US" Or AddressWebUserControl1.DropDownListCountryValue = "CA" Then
                'START: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                ''Anudeep:2013.06.20-BT-1555:YRS 5.0-1769:Length of phone numbers
                'If TextBoxTelephone.Text.Trim().Length <> 10 And TextBoxTelephone.Text.Trim().Length > 0 Then
                '    Session("AddressOk") = 1
                '    'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '    showMessage(PlaceHolder1, "Verify", "Telephone number must be 10 digits.", MessageBoxButtons.Stop)
                '    Exit Sub
                'End If
                If TextBoxTelephone.Text.Trim().Length > 0 Then
                    stTelephoneError = Validation.Telephone(TextBoxTelephone.Text.Trim(), YMCAObjects.TelephoneType.Telephone)
                    If (Not String.IsNullOrEmpty(stTelephoneError)) Then
                        Session("AddressOk") = 1
                        showMessage(PlaceHolder1, "Verify", stTelephoneError, MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                End If
                'END: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
            End If
            If AddressWebUserControl2.DropDownListCountryValue = "US" Or AddressWebUserControl2.DropDownListCountryValue = "CA" Then
                'START: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
                ''Anudeep:2013.06.20-BT-1555:YRS 5.0-1769:Length of phone numbers
                'If TextBoxSecTelephone.Text.Trim().Length <> 10 And TextBoxSecTelephone.Text.Trim().Length > 0 Then
                '    Session("AddressOk") = 1
                '    'Anudeep:21.06.2013 -BT-1555:YRS 5.0-1769:Length of phone numbers
                '    showMessage(PlaceHolder1, "Verify", "Telephone number must be 10 digits.", MessageBoxButtons.Stop)
                '    Exit Sub
                'End If
                If TextBoxSecTelephone.Text.Trim().Length > 0 Then
                    stTelephoneError = Validation.Telephone(TextBoxSecTelephone.Text.Trim(), YMCAObjects.TelephoneType.Telephone)
                    If (Not String.IsNullOrEmpty(stTelephoneError)) Then
                        Session("AddressOk") = 1
                        showMessage(PlaceHolder1, "Verify", stTelephoneError, MessageBoxButtons.Stop)
                        Exit Sub
                    End If
                End If
                'END: PPP | 2015.10.13 | YRS-AT-2588 | New validation rules applied and also error message is retrieved from AtsMetaMessages
            End If
            'Start - Manthan | 2016.04.22 | YRS-AT-2206 | storing selected option ID in variable and session value to dataset and checking total deduction amt not exceeding selected annuity option plan wise and showing error message
            If Not Session("g_DeathDate") Is Nothing Then
                ' Get Annuity Purchase date for beneficiary
                dtmDeathDate = Date.Parse(Session("g_DeathDate"))
                dtmAnnuityPurchaseDate = DateAdd(DateInterval.Month, 1, dtmDeathDate)
                dtmAnnuityPurchaseDate = DateAdd(DateInterval.Day, (-1 * dtmAnnuityPurchaseDate.Day) + 1, dtmAnnuityPurchaseDate)

                ' Get no of months for to calculate annuity amount
                intPayrollMonthCount = YMCARET.YmcaBusinessObject.BeneficiarySettlement.GetPastPayrollCount(dtmAnnuityPurchaseDate)
                intPayrollMonthCount = IIf(intPayrollMonthCount > 0, intPayrollMonthCount, 1)

                If Not Session("BS_SelectedOption_RP") Is Nothing Then
                    strDeathBenOptionID_RP = Session("BS_SelectedOption_RP")
                End If
                If Not Session("BS_SelectedOption_SP") Is Nothing Then
                    strDeathBenOptionID_SP = Session("BS_SelectedOption_SP")
                End If

                If Not Session("BSF_l_dataset_ActiveSettlementOption_RetirementPlan") Is Nothing Then
                    dsActiveSettlementOption_RP = DirectCast(Session("BSF_l_dataset_ActiveSettlementOption_RetirementPlan"), DataSet)
                End If
                If Not Session("BSF_l_dataset_ActiveSettlementOption_SavingsPlan") Is Nothing Then
                    dsActiveSettlementOption_SP = DirectCast(Session("BSF_l_dataset_ActiveSettlementOption_SavingsPlan"), DataSet)
                End If

                If HelperFunctions.isNonEmpty(dsActiveSettlementOption_RP) Then
                    dvActiveSettlementOption_RP = dsActiveSettlementOption_RP.Tables(0).DefaultView
                    dvActiveSettlementOption_RP.RowFilter = "UniqueID = '" + strDeathBenOptionID_RP + "'"
                End If

                If HelperFunctions.isNonEmpty(dsActiveSettlementOption_SP) Then
                    dvActiveSettlementOption_SP = dsActiveSettlementOption_SP.Tables(0).DefaultView
                    dvActiveSettlementOption_SP.RowFilter = "UniqueID = '" + strDeathBenOptionID_SP + "'"
                End If
                'Checking deduction amt not exceeding M annutiy option amt and C Annuity option amt based on option selected from Retirement plan
                If HelperFunctions.isNonEmpty(dvActiveSettlementOption_RP) AndAlso HelperFunctions.isEmpty(dvActiveSettlementOption_SP) Then
                    If RadioButtonAnnuityType_RetirementPlan.SelectedValue = "M" AndAlso (Convert.ToDecimal(dvActiveSettlementOption_RP(0)("Annuity M")) * intPayrollMonthCount) < Convert.ToDecimal(lblDeductionamt.Text) Then
                        showMessage(PlaceHolder1, "Beneficiary Information", "Deductions amount cannot be greater than annuity amount", MessageBoxButtons.Stop)
                        CloseFormStatus = False
                        Exit Sub
                    ElseIf RadioButtonAnnuityType_RetirementPlan.SelectedValue = "C" AndAlso (Convert.ToDecimal(dvActiveSettlementOption_RP(0)("Annuity C")) * intPayrollMonthCount) < Convert.ToDecimal(lblDeductionamt.Text) Then
                        showMessage(PlaceHolder1, "Beneficiary Information", "Deductions amount cannot be greater than annuity amount", MessageBoxButtons.Stop)
                        CloseFormStatus = False
                        Exit Sub
                    End If
                End If
                'Checking deduction amt not exceeding M annutiy option amt and C Annuity option amt based on option selected from Savings plan
                If HelperFunctions.isNonEmpty(dvActiveSettlementOption_SP) AndAlso HelperFunctions.isEmpty(dvActiveSettlementOption_RP) Then
                    If RadioButtonAnnuityType_SavingsPlan.SelectedValue = "M" AndAlso (Convert.ToDecimal(dvActiveSettlementOption_SP(0)("Annuity M")) * intPayrollMonthCount) < Convert.ToDecimal(lblDeductionamt.Text) Then
                        showMessage(PlaceHolder1, "Beneficiary Information", "Deductions amount cannot be greater than annuity amount", MessageBoxButtons.Stop)
                        CloseFormStatus = False
                        Exit Sub
                    ElseIf RadioButtonAnnuityType_SavingsPlan.SelectedValue = "C" AndAlso (Convert.ToDecimal(dvActiveSettlementOption_SP(0)("Annuity C")) * intPayrollMonthCount) < Convert.ToDecimal(lblDeductionamt.Text) Then
                        showMessage(PlaceHolder1, "Beneficiary Information", "Deductions amount cannot be greater than annuity amount", MessageBoxButtons.Stop)
                        CloseFormStatus = False
                        Exit Sub
                    End If
                End If
                'Checking deduction amt not exceeding M annutiy option amt and C Annuity option amt based on combination of option selected from retirement and savings plan
                If HelperFunctions.isNonEmpty(dvActiveSettlementOption_SP) AndAlso HelperFunctions.isNonEmpty(dvActiveSettlementOption_RP) Then
                    If RadioButtonAnnuityType_RetirementPlan.SelectedValue = "M" AndAlso RadioButtonAnnuityType_SavingsPlan.SelectedValue = "M" Then
                        If ((Convert.ToDecimal(dvActiveSettlementOption_RP(0)("Annuity M")) + Convert.ToDecimal(dvActiveSettlementOption_SP(0)("Annuity M"))) * intPayrollMonthCount) < Convert.ToDecimal(lblDeductionamt.Text) Then
                            showMessage(PlaceHolder1, "Beneficiary Information", "Deductions amount cannot be greater than annuity amount", MessageBoxButtons.Stop)
                            CloseFormStatus = False
                            Exit Sub
                        End If
                    ElseIf RadioButtonAnnuityType_RetirementPlan.SelectedValue = "M" AndAlso RadioButtonAnnuityType_SavingsPlan.SelectedValue = "C" Then
                        If ((Convert.ToDecimal(dvActiveSettlementOption_RP(0)("Annuity M")) + Convert.ToDecimal(dvActiveSettlementOption_SP(0)("Annuity C"))) * intPayrollMonthCount) < Convert.ToDecimal(lblDeductionamt.Text) Then
                            showMessage(PlaceHolder1, "Beneficiary Information", "Deductions amount cannot be greater than annuity amount", MessageBoxButtons.Stop)
                            CloseFormStatus = False
                            Exit Sub
                        End If
                    ElseIf RadioButtonAnnuityType_RetirementPlan.SelectedValue = "C" AndAlso RadioButtonAnnuityType_SavingsPlan.SelectedValue = "M" Then
                        If ((Convert.ToDecimal(dvActiveSettlementOption_RP(0)("Annuity C")) + Convert.ToDecimal(dvActiveSettlementOption_SP(0)("Annuity M"))) * intPayrollMonthCount) < Convert.ToDecimal(lblDeductionamt.Text) Then
                            showMessage(PlaceHolder1, "Beneficiary Information", "Deductions amount cannot be greater than annuity amount", MessageBoxButtons.Stop)
                            Exit Sub
                        End If
                    ElseIf RadioButtonAnnuityType_RetirementPlan.SelectedValue = "C" AndAlso RadioButtonAnnuityType_SavingsPlan.SelectedValue = "C" Then
                        If ((Convert.ToDecimal(dvActiveSettlementOption_RP(0)("Annuity C")) + Convert.ToDecimal(dvActiveSettlementOption_SP(0)("Annuity C"))) * intPayrollMonthCount) < Convert.ToDecimal(lblDeductionamt.Text) Then
                            showMessage(PlaceHolder1, "Beneficiary Information", "Deductions amount cannot be greater than annuity amount", MessageBoxButtons.Stop)
                            CloseFormStatus = False
                            Exit Sub
                        End If
                    End If
                End If
            End If
            'End - Manthan | 2016.04.22 | YRS-AT-2206 | storing selected option ID in variable and session value to dataset and checking total deduction amt not exceeding selected annuity option plan wise and showing error message
            If (SaveDetails() = False) Then
                Exit Sub
            Else
                CloseForm()
            End If
            'Dim msg As String = ""
            'msg = msg + "<Script Language='JavaScript'>"
            'msg = msg + "window.opener.location.href=window.opener.location.href;"
            'msg = msg + "self.close();"
            'msg = msg + "</Script>"
            'Response.Write(msg)
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    'Private Sub DropDownListCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownListCountry.SelectedIndexChanged
    '    Try
    '        Me.SetStateValuesBasedonCountrySelected()
    '    Catch ex As SqlException
    '        Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    End Try
    'End Sub

    'Private Sub DropdownlistSecCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropdownlistSecCountry.SelectedIndexChanged
    '    Try
    '        Me.SetSecStateValuesBasedonSecCountrySelected()
    '    Catch ex As SqlException
    '        Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    Catch ex As Exception
    '        Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
    '    End Try
    'End Sub

    Private Sub ButtonGeneralWithholdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGeneralWithholdAdd.Click
        Try
            Session("blnAddFedWithHolding") = True
            StoreFormDetails()
            'Shubhrata Mar 2nd,2007 YREN-3112
            Session("IsFedTaxForMaritalStatus") = True
            'Shubhrata Mar 2nd,2007 YREN-3112
            'PK |12/10/2019 |YRS-AT-4605|change window width from 450 to 600.
            Dim popupScript As String = "<script language='javascript'>" & _
                "window.open('UpdateFedWithHoldingInfo.aspx', 'CustomPopUpOpen', " & _
                "'width=600, height=400, menubar=no, resizable=yes,top=80,left=300, scrollbars=yes')" & _
                "</script>"


            Page.RegisterStartupScript("PopupScript2", popupScript)
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    Private Sub DataGridGeneralWithhold_ItemCommand(source As Object, e As DataGridCommandEventArgs) Handles DataGridGeneralWithhold.ItemCommand
        If e.CommandName.ToLower = "edit" Then
            Dim l_dgitem As DataGridItem
            Dim l_button_Select As ImageButton
            Try
                StoreFormDetails()
                If (DataGridGeneralWithhold.SelectedIndex >= 0) Then
                    LoadFederalWithHoldingForm()
                ElseIf (DataGridGeneralWithhold.Items.Count > 0) Then
                    DataGridGeneralWithhold.SelectedIndex = 0
                    LoadFederalWithHoldingForm()
                End If
            Catch ex As SqlException
                Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            Catch ex As Exception
                Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
            End Try

        End If

    End Sub

    Private Sub ButtonRetireesInfoCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRetireesInfoCancel.Click
        Try
            Session("Success_BIScreen") = False
            'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Clearing Session value
            Session("FinalDeductionsAnnuity") = Nothing
            Session("TotDeductionsAmt") = Nothing
            Session("DeductionsDataTable") = Nothing
            'End - Manthan | 2016.04.22 | YRS-AT-2206 | Clearing Session value
            CloseForm()
        Catch ex As SqlException
            Response.Redirect("ErrorPageForm.aspx?DBMessage=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        Catch ex As Exception
            Response.Redirect("ErrorPageForm.aspx?Message=" + Server.UrlEncode(ex.Message.Trim.ToString()))
        End Try
    End Sub

    Private Sub ButtonGeneralPOA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonGeneralPOA.Click
        Try
            'Anudeep:13.11.2013:BT:2190-YRS 5.0-2199: Added below line to get read-only access to POA 
            Dim CheckSecurity = SecurityCheck.Check_Authorization("btnbenePOA", Convert.ToInt32(Session("LoggedUserKey")), True)
            If Not CheckSecurity.Equals("True") And Not CheckSecurity.Equals("Read-Only") Then
                MessageBox.Show(PlaceHolder1, "YMCA-YRS", CheckSecurity, MessageBoxButtons.Stop, True)
                Session("AddressOk") = 1
                Exit Sub
            End If
            'Anudeep:13.11.2013:BT:2190-YRS 5.0-2199: Added below code for setting the rights of poa in session variable for setting poa in view/edit mode.
            Session("POA_Rights") = CheckSecurity

            'StoreFormDetails()
            'If (TaxWithHoldRequired = True) Then
            '    LoadFedWithDrawalTab()
            'End If
            Session("POAClicked") = True
            Dim l_DataSet_POA As DataSet
            Dim popupScript As String = "<script language='javascript'>" & _
                 " window.open('RetireesPowerAttorneyWebForm.aspx', 'CustomPopUpOpen', " & _
                 "'width=800, height=600, menubar=no, resizable=yes,top=80,left=120, scrollbars=yes')" & _
                 "</script>"


            Page.RegisterStartupScript("PopupScript10", popupScript)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub DataGridGeneralWithhold_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGridGeneralWithhold.SortCommand
        Try
            Dim l_ds_BenInfo As DataSet
            Me.DataGridGeneralWithhold.SelectedIndex = -1
            If Not ViewState("DS_Sort_BenInfo") Is Nothing Then
                Dim dv As New DataView
                Dim SortExpression As String
                SortExpression = e.SortExpression
                l_ds_BenInfo = ViewState("DS_Sort_BenInfo")
                dv = l_ds_BenInfo.Tables(0).DefaultView
                dv.Sort = SortExpression
                If Not Session("Sort_BenInfo") Is Nothing Then
                    If Session("Sort_BenInfo").ToString.Trim.EndsWith("ASC") Then
                        dv.Sort = SortExpression + " DESC"
                    Else
                        dv.Sort = SortExpression + " ASC"
                    End If
                Else
                    dv.Sort = SortExpression + " ASC"
                End If
                Me.DataGridGeneralWithhold.DataSource = Nothing
                Me.DataGridGeneralWithhold.DataSource = dv
                Me.DataGridGeneralWithhold.DataBind()
                Session("Sort_BenInfo") = dv.Sort
            End If
        Catch ex As Exception
            Dim l_String_Exception_Message As String
            l_String_Exception_Message = Server.UrlEncode(ex.Message.Trim.ToString())
            Response.Redirect("ErrorPageForm.aspx?Message=" + l_String_Exception_Message, False)
        End Try
    End Sub

#End Region


#Region "General Utility Function"

    'NP:PS:2007.09.17 - Avoiding errors due to duplicate messageboxes
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

#End Region

    'Priya 2010.08.11 : Added persistence mechanism for YRS 5.0-1147 : Issue with simultaneous users doing death settlement 
#Region "Persistence Mechanism"

    Protected Overrides Function SaveViewState() As Object
        'Session("Address Information") = g_dataset_dsAddressInformation
        'Session("Telephone Information") = g_dataset_dsTelephoneInformation
        'Session("Email Information") = g_dataset_dsEmailInformation
        'ViewState("Page_Mode") = Page_Mode

        Session("BS_SelectedOption_RP") = l_string_BenefitOptionID
        Session("BS_SelectedOption_SP") = l_string_BenefitOptionID_SavingsPlan
        ViewState("TextBoxGeneralSSNo") = l_string_SSNo
        ViewState("string_PrevSSNo") = l_string_PrevSSNo
        ViewState("TaxWithHoldRequired") = TaxWithHoldRequired ' Boolean
        Return MyBase.SaveViewState()
    End Function

    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        MyBase.LoadViewState(savedState)
        l_string_BenefitOptionID = Session("BS_SelectedOption_RP")
        l_string_BenefitOptionID_SavingsPlan = Session("BS_SelectedOption_SP")
        l_string_SSNo = ViewState("TextBoxGeneralSSNo")

        l_string_PrevSSNo = ViewState("string_PrevSSNo")
        TaxWithHoldRequired = ViewState("TaxWithHoldRequired")

        'g_dataset_dsAddressInformation = DirectCast(Session("Address Information"), DataSet)
        'g_dataset_dsTelephoneInformation = DirectCast(Session("Telephone Information"), DataSet)
        'g_dataset_dsEmailInformation = DirectCast(Session("Email Information"), DataSet)

    End Sub
#End Region
    'End 2010.08.11 
    'bhavnaS 2011.07.18 :BT 706:to display active poa separated by ,
    Private Sub LoadPOADetails(ByVal l_DataSet_POA As DataSet)
        Dim builder As New StringBuilder
        'Start:Anudeep 11.03.2013 -Bt-1236:YRS 5.0-1685:Add Category/Type field to Power of attorney and allow 3 types
        'GridViewRetireesAttorney.DataSource = l_DataSet_POA
        'GridViewRetireesAttorney.DataBind()
        If HelperFunctions.isNonEmpty(l_DataSet_POA) Then
            'Anudeep:07.11.2013-BT:2269-YRS 5.0-2234:Added to show the Button text without POA
            ButtonGeneralPOA.Text = "Show/Edit all (" + l_DataSet_POA.Tables("POAInfo").Rows.Count.ToString() + ")"
        End If
        'End:Anudeep 11.03.2013 -Bt-1236:YRS 5.0-1685:Add Category/Type field to Power of attorney and allow 3 types
        'If HelperFunctions.isNonEmpty(l_DataSet_POA) Then

        'For Each dr As DataRow In l_DataSet_POA.Tables("POAInfo").Rows
        '    If dr.IsNull("Name1") = True OrElse dr.Item("Name1") = "" Then Continue For
        '    If builder.Length > 0 Then builder.Append(", ")
        '    builder.Append(dr("Name1").ToString())
        'Next
        '        End If
        'TextBoxGeneralPOA.Text = builder.ToString()
    End Sub
    'BS:2011.08.10:YRS 5.0-1339:BT:852 - Reopen issue 
    Public Function ValidateNonHumanInfo(ByVal l_string_BenefitOptionID As String) As Boolean
        Dim l_DataSet_NonHumBenInfo As DataSet
        If l_DataSet_NonHumBenInfo Is Nothing Then
            l_DataSet_NonHumBenInfo = YMCARET.YmcaBusinessObject.BeneficiarySettlement.LookUp_NonHumanBenfInfo(l_string_BenefitOptionID)
            If l_DataSet_NonHumBenInfo.Tables(0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    'Anudeep:01.06.2013 BT-1501:YRS 5.0-1745:Capture Beneficiary addresses 
    Public Sub SaveAddressDetails(ByVal l_string_BenePersUniqueID As String)


        '''Insert Into Address table 
        'Anudeep YRS 5.0-1745
        Dim l_DataTable_Address As DataTable
        Dim l_DataTable_PrimaryAddress As DataTable
        Dim l_DataTable_SecondaryAddress As DataTable


        AddressWebUserControl1.EntityCode = EnumEntityCode.PERSON.ToString()
        AddressWebUserControl2.EntityCode = EnumEntityCode.PERSON.ToString()
        AddressWebUserControl1.AddrCode = "HOME"
        AddressWebUserControl2.AddrCode = "HOME"
        AddressWebUserControl1.guiEntityId = l_string_BenePersUniqueID
        AddressWebUserControl2.guiEntityId = l_string_BenePersUniqueID
        'Anudeep:29.07.2013:Bt-1683-YRS 5.0-1862:Add notes record when user enters address in any module.
        If Not AddressWebUserControl1.Notes Is String.Empty Then
            AddressWebUserControl1.Notes = "Beneficiary Settlement: " + AddressWebUserControl1.Notes
        End If
        If Not AddressWebUserControl2.Notes Is String.Empty Then
            AddressWebUserControl2.Notes = "Beneficiary Settlement: " + AddressWebUserControl2.Notes
        End If


        l_DataTable_PrimaryAddress = AddressWebUserControl1.GetAddressTable()
        l_DataTable_SecondaryAddress = AddressWebUserControl2.GetAddressTable()

        l_DataTable_Address = l_DataTable_PrimaryAddress.Clone
        If HelperFunctions.isNonEmpty(l_DataTable_PrimaryAddress) Then
            l_DataTable_Address.ImportRow(l_DataTable_PrimaryAddress.Rows(0))
        End If
        If HelperFunctions.isNonEmpty(l_DataTable_SecondaryAddress) Then
            l_DataTable_Address.ImportRow(l_DataTable_SecondaryAddress.Rows(0))
        End If
        Address.SaveAddress(l_DataTable_Address)

    End Sub
    'Start:Anudeep:16.02.2013:BT:2306:YRS 5.0-2251 : Added code to fill the participant address in address control
    Private Sub lnkParticipantAddress1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkParticipantAddress1.Click
        Dim dr_PrimaryAddress As DataRow()
        Dim dsAddress As DataSet
        Try
            If Session("ParticipantEntityId") IsNot Nothing Then
                dsAddress = Address.GetAddressByEntity(Session("ParticipantEntityId").ToString(), EnumEntityCode.PERSON)
                If HelperFunctions.isNonEmpty(dsAddress) Then
                    dr_PrimaryAddress = dsAddress.Tables("Address").Select("isPrimary = True")
                End If
            End If
            If dr_PrimaryAddress IsNot Nothing AndAlso dr_PrimaryAddress.Length > 0 Then
                dr_PrimaryAddress(0)("UniqueId") = AddressWebUserControl1.UniqueId 'AA:28.05.2014 BT:2306:YRS 5.0-2251 - Beneficiary effective date changed to current date from particiapant effective date
                dr_PrimaryAddress(0)("guiEntityId") = ""
                dr_PrimaryAddress(0)("effectiveDate") = Today.ToShortDateString 'AA:26.05.2014 BT:2306:YRS 5.0-2251 - Beneficiary effective date changed to current date from particiapant effective date
                AddressWebUserControl1.LoadAddressDetail(dr_PrimaryAddress)
                AddressWebUserControl1.Notes = "Address has been added / updated from participant address."
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub lnkParticipantAddress2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkParticipantAddress2.Click
        Dim dr_PrimaryAddress As DataRow()
        Dim dsAddress As DataSet
        Try
            If Session("ParticipantEntityId") IsNot Nothing Then
                dsAddress = Address.GetAddressByEntity(Session("ParticipantEntityId").ToString(), EnumEntityCode.PERSON)
                If HelperFunctions.isNonEmpty(dsAddress) Then
                    dr_PrimaryAddress = dsAddress.Tables("Address").Select("isPrimary = False")
                End If
            End If
            If dr_PrimaryAddress IsNot Nothing AndAlso dr_PrimaryAddress.Length > 0 Then
                dr_PrimaryAddress(0)("UniqueId") = AddressWebUserControl2.UniqueId 'AA:28.05.2014 BT:2306:YRS 5.0-2251 - Beneficiary effective date changed to current date from particiapant effective date
                dr_PrimaryAddress(0)("guiEntityId") = ""
                dr_PrimaryAddress(0)("effectiveDate") = Today.ToShortDateString 'AA:26.05.2014 BT:2306:YRS 5.0-2251 - Beneficiary effective date changed to current date from participant effective date
                AddressWebUserControl2.LoadAddressDetail(dr_PrimaryAddress)
                AddressWebUserControl2.Notes = "Address has been added / updated from participant address."
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'End:Anudeep:16.02.2013:BT:2306:YRS 5.0-2251 : Added code to fill the participant address in address control
    'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Method to bind grid from database
    Public Function LoadDeductions(ByVal parameterDataGridDeductions As DataGrid)
        Dim dtDeductions As DataTable

        Try
            dtDeductions = YMCARET.YmcaBusinessObject.BeneficiarySettlement.GetDeductions()
            If HelperFunctions.isNonEmpty(dtDeductions) Then
                parameterDataGridDeductions.DataSource = dtDeductions
                parameterDataGridDeductions.DataBind()
                Session("DeductionsDataTable") = dtDeductions
            End If

        Catch ex As Exception
            HelperFunctions.LogException("Beneficiary Information --> LoadDeductions", ex)
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
            HelperFunctions.LogException("Beneficiary Information --> dgDeductions_ItemDataBound", ex)
        End Try
    End Sub
    'End - Manthan | 2016.04.22 | YRS-AT-2206 | Enabling textbox in grid on checkbox selection

    'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Method to split deduction values stored in a string variable and adding it to datatable
    <WebMethod()> _
    Public Shared Function SaveDeductionValues(ByVal strDeductionval As String)
        Dim dtDeductionVal As New DataTable
        Dim dsDeductionVal As New DataSet
        Dim dtDeductions As DataTable
        Dim strDedVal As String
        Dim strSplitDedVal As String()
        Dim strSplit As String()
        Dim drDeductionVal As DataRow
        Dim drDeduction() As DataRow

        Try
            dtDeductionVal.TableName = "Deductions"
            dtDeductionVal.Columns.Add("ControlID", GetType(String))
            dtDeductionVal.Columns.Add("Description", GetType(String))
            dtDeductionVal.Columns.Add("Amount", GetType(Decimal))
            dtDeductionVal.Columns.Add("CodeValue", GetType(String))

            dtDeductions = DirectCast(HttpContext.Current.Session("DeductionsDataTable"), DataTable)

            strDedVal = strDeductionval
            strSplitDedVal = strDedVal.Split("##")

            For Each strlines As String In strSplitDedVal
                If Not String.IsNullOrEmpty(strlines) Then
                    strSplit = strlines.Split(":")
                    drDeductionVal = dtDeductionVal.NewRow()
                    drDeduction = dtDeductions.Select("ShortDescription ='" + strSplit(1) + "'")

                    drDeductionVal.SetField("ControlID", strSplit(0))
                    drDeductionVal.SetField("Description", strSplit(1))
                    If strsplit(1) = "Fund Costs" Then
                        drDeductionVal.SetField("Amount", strSplit(2))
                    Else
                        drDeductionVal.SetField("Amount", drDeduction(0)("Amount"))
                    End If
                    drDeductionVal.SetField("CodeValue", drDeduction(0)("CodeValue").ToString())
                    dtDeductionVal.Rows.Add(drDeductionVal)
                End If
            Next
            dsDeductionVal.Tables.Add(dtDeductionVal)
            HttpContext.Current.Session("FinalDeductionsAnnuity") = dsDeductionVal
            HttpContext.Current.Session("TotDeductionsAmt") = dsDeductionVal.Tables(0).Compute("Sum(Amount)", "")

        Catch ex As Exception
            HelperFunctions.LogException("Beneficiary Information --> SaveDeductionValues", ex)
        End Try
    End Function
    'End - Manthan | 2016.04.22 | YRS-AT-2206 | Method to split deduction values stored in a string variable and adding it to datatable

    'Start - Manthan | 2016.04.22 | YRS-AT-2206 | Method to concate all the selected deductions values from grid
    <WebMethod()> _
    Public Shared Function GetSelectedDeductionVal() As String
        Dim strId As String
        Dim dsDeduction As DataSet
        Try
            If Not HttpContext.Current.Session("FinalDeductionsAnnuity") Is Nothing Then
                dsDeduction = DirectCast(HttpContext.Current.Session("FinalDeductionsAnnuity"), DataSet)
                If HelperFunctions.isNonEmpty(dsDeduction) Then
                    For Each drDeduction As DataRow In dsDeduction.Tables(0).Rows
                        If drDeduction("Description").ToString() = "Fund Costs" Then
                            strId += String.Concat(drDeduction("ControlID").ToString(), "|", drDeduction("Amount").ToString(), "#")
                        Else
                            strId += String.Concat(drDeduction("ControlID").ToString(), "#")
                        End If
                    Next
                End If
            End If
            Return strId
        Catch ex As Exception
            HelperFunctions.LogException("Beneficiary Information --> GetSelectedDeductionVal", ex)
        End Try
    End Function
    'End - Manthan | 2016.04.22 | YRS-AT-2206 | Method to concate all the selected deductions values from grid

    'START : PK |10.29.2019| YRS-AT-4605 | Assign GrossAmount to State Withholding Control
    Public Sub SetGrossAmountToSTWUserControl()
        Dim totalWithholdAmount As Double
        Dim totalAnnuityAmount As Double
        Dim totalFederalAmount As Double
        Dim dsGenWithdrawals As DataSet
        Dim dsFedWithdrawals As DataSet
        Dim dtmDeathDate As DateTime
        Dim dtmAnnuityPurchaseDate As DateTime
        Dim intPayrollMonthCount As Integer
        Try

            If (Not Session("FedWithDrawals") Is Nothing) Then
                dsFedWithdrawals = Session("FedWithDrawals")
            Else
                dsFedWithdrawals = Nothing
            End If
            dsGenWithdrawals = Nothing

            SetAnnuityAmountForSelectedPlan()
            totalAnnuityAmount = Convert.ToDouble(Me.AnnuityAmount)

            If ((totalAnnuityAmount) > 0) Then
                Session("AnnuityAmount") = totalAnnuityAmount
                If Not Session("g_DeathDate") Is Nothing Then

                    dtmDeathDate = Date.Parse(Session("g_DeathDate"))
                    dtmAnnuityPurchaseDate = DateAdd(DateInterval.Month, 1, dtmDeathDate)
                    dtmAnnuityPurchaseDate = DateAdd(DateInterval.Day, (-1 * dtmAnnuityPurchaseDate.Day) + 1, dtmAnnuityPurchaseDate)

                    ' Get no of months for to calculate annuity amount
                    intPayrollMonthCount = YMCARET.YmcaBusinessObject.BeneficiarySettlement.GetPastPayrollCount(dtmAnnuityPurchaseDate)
                    intPayrollMonthCount = IIf(intPayrollMonthCount > 0, intPayrollMonthCount, 1)

                    totalWithholdAmount = YMCARET.YmcaBusinessObject.RetirementBOClass.GetTotalWithHoldingAmount(dsFedWithdrawals, dsGenWithdrawals, intPayrollMonthCount, totalAnnuityAmount, dtmAnnuityPurchaseDate)
                    stwListUserControl.GrossAmount = totalAnnuityAmount - totalWithholdAmount
                    If (HelperFunctions.isNonEmpty(dsFedWithdrawals)) Then

                        totalFederalAmount = YMCARET.YmcaBusinessObject.RetirementBOClass.GetFedWithDrawalAmount(dsFedWithdrawals, 0, totalAnnuityAmount)
                        Me.stwFederalAmount = totalFederalAmount
                        Me.stwFederalType = dsFedWithdrawals.Tables(0).Rows(0)("Type").ToString().ToUpper()

                    Else
                        Me.stwFederalAmount = Nothing
                        Me.stwFederalType = String.Empty
                    End If
                    stwListUserControl.FederalAmount = Me.stwFederalAmount
                    stwListUserControl.FederalType = Me.stwFederalType
                End If
            End If
        Catch ex As Exception
            Throw
        Finally
            totalWithholdAmount = Nothing
            totalAnnuityAmount = Nothing
            dsGenWithdrawals = Nothing
            dsFedWithdrawals = Nothing
            dtmDeathDate = Nothing
        End Try
    End Sub

    Public Sub SetAnnuityAmountForSelectedPlan()
        Dim l_DataSet_SelectedOption_RetirementPlan As DataSet
        Dim l_DataTable_SelectedOption_RetirementPlan As DataTable
        Dim l_DataSet_SelectedOption_SavingsPlan As DataSet
        Dim l_DataTable_SelectedOption_SavingsPlan As DataTable

        Dim M_retAnnuityAmount As Double
        Dim C_retAnnuityAmount As Double
        Dim M_savAnnuityAmount As Double
        Dim C_savAnnuityAmount As Double
        Try
            l_DataSet_SelectedOption_RetirementPlan = Me.DataSet_LookUpBenefitInformation
            l_DataSet_SelectedOption_SavingsPlan = Me.DataSet_LookUpBenefitInformation_SavingsPlan
            M_retAnnuityAmount = 0
            C_retAnnuityAmount = 0
            M_savAnnuityAmount = 0
            C_savAnnuityAmount = 0

            If HelperFunctions.isNonEmpty(l_DataSet_SelectedOption_RetirementPlan) Then
                l_DataTable_SelectedOption_RetirementPlan = l_DataSet_SelectedOption_RetirementPlan.Tables("r_BenefitInformation")

                If (RadioButtonAnnuityType_RetirementPlan.SelectedValue = "M") Then
                    M_retAnnuityAmount = Double.Parse(l_DataTable_SelectedOption_RetirementPlan.Rows(0)("AnnuityM").ToString())
                End If

                If (RadioButtonAnnuityType_RetirementPlan.SelectedValue = "C") Then
                    C_retAnnuityAmount = Double.Parse(l_DataTable_SelectedOption_RetirementPlan.Rows(0)("AnnuityC").ToString())
                End If
            End If

            If HelperFunctions.isNonEmpty(l_DataSet_SelectedOption_SavingsPlan) Then
                l_DataTable_SelectedOption_SavingsPlan = l_DataSet_SelectedOption_SavingsPlan.Tables("r_BenefitInformation")

                If (RadioButtonAnnuityType_SavingsPlan.SelectedValue = "M") Then
                    M_savAnnuityAmount = Double.Parse(l_DataTable_SelectedOption_SavingsPlan.Rows(0)("AnnuityM").ToString())
                End If

                If (RadioButtonAnnuityType_SavingsPlan.SelectedValue = "C") Then
                    C_savAnnuityAmount = Double.Parse(l_DataTable_SelectedOption_SavingsPlan.Rows(0)("AnnuityC").ToString())
                End If
            End If
            AnnuityAmount = M_retAnnuityAmount + M_savAnnuityAmount + C_retAnnuityAmount + C_savAnnuityAmount
        Catch
            Throw
        Finally
            l_DataSet_SelectedOption_RetirementPlan = Nothing
            l_DataTable_SelectedOption_RetirementPlan = Nothing
            l_DataSet_SelectedOption_SavingsPlan = Nothing
            l_DataTable_SelectedOption_SavingsPlan = Nothing

            M_retAnnuityAmount = Nothing
            C_retAnnuityAmount = Nothing
            M_savAnnuityAmount = Nothing
            C_savAnnuityAmount = Nothing

        End Try
    End Sub

    Private Sub RadioButtonAnnuityType_RetirementPlan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioButtonAnnuityType_RetirementPlan.SelectedIndexChanged
        SetGrossAmountToSTWUserControl()
    End Sub

    Private Sub RadioButtonAnnuityType_SavingsPlan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioButtonAnnuityType_SavingsPlan.SelectedIndexChanged
        SetGrossAmountToSTWUserControl()
    End Sub
    ' State Tax Withholding MA validation 
    Public Function ValidateSTWvsFedtaxforMA() As Boolean
        Dim message As String
        Dim LstSWHPerssDetail As List(Of YMCAObjects.StateWithholdingDetails)
        If (Not SessionManager.SessionStateWithholding.LstSWHPerssDetail Is Nothing) Then
            If (SessionManager.SessionStateWithholding.LstSWHPerssDetail.Count > 0) Then

                LstSWHPerssDetail = SessionManager.SessionStateWithholding.LstSWHPerssDetail
                If (Not YMCARET.YmcaBusinessObject.StateWithholdingBO.ValidateFedTaxVSStateTaxInputDetailForMA(LstSWHPerssDetail.FirstOrDefault, Me.stwFederalAmount, Me.stwFederalType, message)) Then
                    If (divErrorMsg.InnerHtml.Contains(message)) Then
                        Return False
                    Else
                        ShowErrorMessage(message)
                        Return False
                    End If
                End If
            End If
        End If
        Return True
    End Function
    Private Sub ShowErrorMessage(stMessage As String)
        divErrorMsg.InnerHtml = IIf(String.IsNullOrEmpty(divErrorMsg.InnerHtml.Trim), "", divErrorMsg.InnerHtml + "</br>")
        divErrorMsg.InnerHtml = divErrorMsg.InnerHtml + stMessage
        divErrorMsg.Visible = True
    End Sub
    'END : PK |10.29.2019|YRS-AT-4605 | Assign GrossAmount to State Withholding Control
End Class
